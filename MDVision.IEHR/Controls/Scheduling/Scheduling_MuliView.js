var appstatuses;
var appstatus;
Scheduling_MuliView = {
    bIsFirstLoad: true,
    params: [],
    slotHeight: 20,
    padding: -5,
    buttonHeightAdj: 7,
    zIndexDraggable: 1017,
    Load: function (params) {

        Scheduling_MuliView.params = params;
        Scheduling_MuliView.StickyHeading();
        if (Scheduling_MuliView.bIsFirstLoad) {
            Scheduling_MuliView.bIsFirstLoad = false;
            var self = $('#pnlScheduleMuliView');
            var events;
            self.loadDropDowns(true).done(function () {

                MDVisionService.lookups("GetAppointmentStatus").done(function (result) {
                    result = JSON.parse(result["GetAppointmentStatus"]);
                    appstatus = result;
                });

            });
        }

        Scheduling_MuliView.ValidateMultiViewSearch();
        Scheduling_Calendar.closeDropDownOnHtmlClick();

        Scheduling_MuliView.LoadMultipleViewCalendar();
    },

    OpenGroup: function () {
        var params = [];

        LoadActionPan('multipleViewGroup', params);
    },
    LoadClinicalNote: function (AppointmentId, PatientId, AppTime, VisitId, Reason, RefProviderName, RefProviderId, isNoteCreated, noteid, provid, provname, facid, facname, status, resourceid) {

        //       Clinical_ProgressNote.params.AppointmentVisitId = VisitId;
        if (status != 'true') {

            if (isNoteCreated == "true") {
                AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        params["QuickAddPatient"] = true;
                        // params["patientID"] = PatientId;
                        var AppointmentTime = AppTime;
                        var VisitDate = $('#pnlScheduleMuliView #searchdate').text().trim();
                        var FacilityId = facid;
                        var ProviderId = provid;
                        var ParentCntrlLoadid = "Schedular"
                        var FacilityName = facname;
                        var ProviderName = provname;
                        var RefProviderName = (RefProviderName == "undefined" || RefProviderName == null) ? null : RefProviderName;
                        var RefProviderId = (RefProviderId == "undefined" || RefProviderId == null) ? null : RefProviderId;
                        var Reason = (Reason == "undefined" || Reason == null) ? null : Reason;
                        var ForProgressNote = false;
                        var NotesId = noteid != null ? noteid : '';
                        var Room = "";
                        EMRUtility.CreateNote("Edit", PatientId, AppointmentId, ProviderId, ProviderName, AppointmentTime, VisitId, VisitDate, Reason, FacilityName,
                            FacilityId, Room, NotesId, ForProgressNote, ParentCntrlLoadid, RefProviderName, RefProviderId);

                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
            }
            else {
                AppPrivileges.GetFormPrivileges("Notes_Notes", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        params["QuickAddPatient"] = true;
                        // params["patientID"] = PatientId;
                        var AppointmentTime = AppTime;
                        var VisitDate = $('#pnlScheduleCalendar #daydate').text().trim();
                        var FacilityId = $('#pnlScheduleCalendar #Facility').val();
                        var ProviderId = $('#pnlScheduleCalendar #Provider').val();
                        var ParentCntrlLoadid = "Schedular"
                        var FacilityName = $('#pnlScheduleCalendar #Facility option:selected').text();
                        var ProviderName = $('#pnlScheduleCalendar #Provider option:selected').text();
                        if (provid != null) {
                            ProviderId = provid;
                            ProviderName = provname;

                        }
                        var RefProviderName = (RefProviderName == "undefined" || RefProviderName == null) ? null : RefProviderName;
                        var RefProviderId = (RefProviderId == "undefined" || RefProviderId == null) ? null : RefProviderId;
                        var Reason = (Reason == "undefined" || Reason == null) ? null : Reason;
                        var ForProgressNote = false;
                        var NotesId = '';
                        var Room = "";
                         var ResourceproviderId = null;
                        var ResourceproviderName = null;
                        var ResourceId = null;
                        var ResourceName = null;
                        if (resourceid != "undefined") {
                            var resourceDetail = null;
                            $('#pnlScheduleCalendar #Resource option').each(function (e) {
                                if ($(this).val() == resourceid) {
                                    resourceDetail = $(this).attr("refname");
                                    ResourceName = $(this).text();
                                    ResourceId = resourceid;
                                    return false;
                                }
                                    });
                                    if (resourceDetail != null) {
                                ResourceproviderId = resourceDetail.split("-")[0];
                                ResourceproviderName = resourceDetail.split("-")[1];
                            }
                                    }
                        EMRUtility.CreateNote("Add", PatientId, AppointmentId, ProviderId, ProviderName, AppointmentTime, VisitId, VisitDate, Reason, FacilityName,
                            FacilityId, Room, NotesId, ForProgressNote, ParentCntrlLoadid, RefProviderName, RefProviderId, ResourceId, ResourceName, ResourceproviderId, ResourceproviderName);
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
            }
        } else {
            AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    params["QuickAddPatient"] = true;
                    params["patientID"] = PatientId;
                    var AppointmentTime = AppTime;
                    var VisitDate = $('#pnlScheduleMuliView #searchdate').text().trim();
                    var FacilityId = facid;
                    var ProviderId = provid;
                    var ParentCntrlLoadid = "Schedular"
                    var FacilityName = facname;
                    var ProviderName = provname;
                    var RefProviderName = (RefProviderName == "undefined" || RefProviderName == null) ? null : RefProviderName;
                    var RefProviderId = (RefProviderId == "undefined" || RefProviderId == null) ? null : RefProviderId;
                    var Reason = (Reason == "undefined" || Reason == null) ? null : Reason;
                    var ForProgressNote = false;
                    var NotesId = noteid != null ? noteid : '';
                    var Room = "";
                    EMRUtility.CreateNote("View", PatientId, AppointmentId, ProviderId, ProviderName, AppointmentTime, VisitId, VisitDate, Reason, FacilityName,
                        FacilityId, Room, NotesId, ForProgressNote, ParentCntrlLoadid, RefProviderName, RefProviderId);

                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }

    },
    LoadMultipleViewCalendar: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Schedule Group", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var criteriaDate = $("#pnlScheduleMuliView #searchdate").val();
                var Groupid = $("#pnlScheduleMuliView #Group").val();
                var statusslots = Scheduling_Calendar.FilterCriteria();
                Scheduling_MuliView.SearchScheduleGroupProRes(Groupid, criteriaDate, statusslots).done(function (response1) {
                    if (response1 != false) {
                        var JSONData = JSON.parse(response1.ScheduleGroupsProRes_JSON);
                        var SchRecord = response1.ScheduleRecord_JSON;

                        $('#maincontainer').empty();
                        $('#maincontainer').removeAttr("style");
                        if (response1.totalrecord <= 4) {
                            for (var mi = 0; mi < JSONData.length; mi++) {
                                var facilityidss = JSONData[mi].FacilityId;
                                var providerids = JSONData[mi].Provider.split(',');
                                var resourceids = JSONData[mi].Resource.split(',');

                                var facilitywidth = (100 / parseInt(response1.totalrecord)) * (parseInt(JSONData[mi].ProCount) + parseInt(JSONData[mi].ResCount));
                                var FacilityName = $("#Facility option[value=" + facilityidss + "]").text();
                                $("#maincontainer").append('<div style="width:' + facilitywidth + '%; float:left; " class="th-text-center " id="facility' + facilityidss + '"><div class="col-sm-12 multiple-heading center">Facility:' + FacilityName + '</div></div>');
                                for (var pid = 0; pid < providerids.length; pid++) {

                                    var provideridss = $.trim(providerids[pid]);
                                    var findid = "Provider" + provideridss + "Facility" + facilityidss;
                                    for (var x = 0; x < SchRecord.length; x++) {
                                        var json = JSON.parse(SchRecord[x]);
                                        var id = json.id;
                                        if (findid == id) {
                                            if (json.status != false) {
                                                var PatientsDtl = [];
                                                var AppointmentsDtl = [];
                                                var Slot_Detail = JSON.parse(json.ProviderScheduleFill_JSON);
                                                var providerwidth = 100 / (parseInt(JSONData[mi].ProCount) + parseInt(JSONData[mi].ResCount));
                                                var ProviderName = $("#Provider option[value=" + provideridss + "]").text();
                                                $('#facility' + facilityidss + '').append('<div class="th-text-center " id="provider' + provideridss + '" style="width:' + providerwidth + '%; float:left; "><div class=" multiple-heading center">Provider:' + ProviderName + '</div></div>');
                                                if (Slot_Detail.length != 0) {
                                                    var table = $('<table class="table table-responsive table-bordered table-condensed" id="table' + provideridss + '"><tbody></tbody></table>');
                                                    $('#table' + provideridss + '').children('tbody');
                                                    table.append('<tr align="center"><td colspan="3"><div class="col-sm-12">   <div class="col-xs-1 pl-none pr-none"><a id="pdate' + provideridss + '" providerid="' + provideridss + '" facilityid="' + facilityidss + '" resourceid="" tableid="table' + provideridss + '" class="btn btn-link btn-xs bdate"><i class="fa fa-angle-left"></i></a></div>    <div class="col-xs-9" id="pdate' + provideridss + '">   <p ><span>' + criteriaDate + '</span></p>      </div>        <div class="col-xs-1 pl-none pr-none"><a id="pdate' + provideridss + '" providerid="' + provideridss + '" facilityid="' + facilityidss + '" resourceid="" tableid="ptable' + provideridss + '" class="btn btn-link btn-xs fdate"><i class="fa fa-angle-right "></i></a></div> <div class="col-xs-1 pl-none pr-none"><a data-plugin-datepicker=""   id="pcalendar' + provideridss + facilityidss + '" dateid="pdate' + provideridss + '" providerid="' + provideridss + '" facilityid="' + facilityidss + '" resourceid="" class="btn btn-link btn-xs form-control cal"><i class="fa fa-calendar cal"></i></a></div>                                    </div> </td> </tr>');
                                                    var appids = [];
                                                    var slotappcount;
                                                    for (var i = 0; i < Slot_Detail.length; i++) {
                                                        var appointment;
                                                        var margin = Slot_Detail[i].MaxCountApp;

                                                        var provid = Slot_Detail[i].ProviderId;
                                                        if (provid == '' || provid == 'undefined')
                                                            provid = null;
                                                        var refprovid = Slot_Detail[i].RefProviderId;
                                                        if (refprovid == '' || refprovid == 'undefined')
                                                            refprovid = null;

                                                        var provname = Slot_Detail[i].ProviderName;
                                                        if (provname == '' || provname == 'undefined')
                                                            provname = null;

                                                        var resid = Slot_Detail[i].ResourceId;
                                                        if (resid == '' || resid == 'undefined')
                                                            resid = null;
                                                        var resname = Slot_Detail[i].ResourceName;
                                                        if (resname == '' || resname == 'undefined')
                                                            resname = null;
                                                        var facid = Slot_Detail[i].FacilityId;
                                                        if (facid == '' || facid == 'undefined')
                                                            facid = null;
                                                        var facname = Slot_Detail[i].FacilityName;
                                                        if (facname == '' || facname == 'undefined')
                                                            facname = null;
                                                        var resonid = Slot_Detail[i].ScheduleReasonId;
                                                        if (resonid == '' || resonid == 'undefined')
                                                            resonid = null;

                                                        var color = "";

                                                        if (Slot_Detail[i].FacilityColor == "" || Slot_Detail[i].FacilityColor == "undefined" || Slot_Detail[i].FacilityColor == "null" || Slot_Detail[i].FacilityColor == null) {
                                                            color = '';
                                                        } else {
                                                            color = 'bgcolor = \'' + Slot_Detail[i].FacilityColor + '\'';
                                                        }

                                                        var dateid = 'pdate' + provideridss + '';


                                                        var appointments = '<div class="pull-left">&nbsp;</div>';
                                                        if (Slot_Detail[i].AppDtl != "") {
                                                            var app;
                                                            var test = 0;

                                                            var data = Slot_Detail[i].AppDtl;
                                                            var arr = data.split('|');

                                                            for (var j = 0; j < arr.length; j++) {
                                                                slotappcount = (arr.length) - 1;
                                                                var split = arr[j].split(',')
                                                                if (split.length == 22 || split.length == 24) {

                                                                    var appid = split[0];
                                                                    var patientid = split[1];
                                                                    var patientname = split[2];
                                                                    patientname = patientname.replace('-', ', ');
                                                                    var appointmentstatus = split[3];
                                                                    var appcolor = split[4];
                                                                    var appcopay = split[6];
                                                                    var appreason = split[3];
                                                                    var appcount = split[7];
                                                                    var noteStatus = split[16];
                                                                    var appcomments = split[8];
                                                                    var patientvisitid = split[9];
                                                                    var patvisitstatusid = split[10];
                                                                    var patvisitname = split[11];
                                                                    var patientType = split[17];
                                                                    var visitType = split[18];
                                                                    var isNonBillable = split[19];
                                                                    //Edit By Mohsin Nasir Bug # 2922,2931
                                                                    var patEligibility = split[12];
                                                                    var appschreason = split[5];
                                                                    appschreason = appschreason.replace(/#@#/g, ',');
                                                                    //start clinical notes check
                                                                    var noteid = split[13];

                                                                    var noteTitle = "";
                                                                    var isNoteCreated = false;

                                                                    if (noteid == "0") {
                                                                        noteTitle = "Create Note";
                                                                        isNoteCreated = false;
                                                                    } else {
                                                                        noteTitle = "Edit Note";
                                                                        isNoteCreated = true;
                                                                    }
                                                                    var isViewNote = false;
                                                                    if (patvisitname.toUpperCase() == "CHECK OUT") {


                                                                        if (noteStatus == "Signed") {
                                                                            noteTitle = "View Note";
                                                                            isNoteCreated = true;
                                                                            isViewNote = true;
                                                                        }
                                                                    }
                                                                    //
                                                                    var eligibilityIcon;

                                                                    if (patEligibility == "Active") {
                                                                        eligibilityIcon = '<i class="fa fa-check-circle green"></i>';
                                                                    }
                                                                    else if (patEligibility == "Inactive") {
                                                                        eligibilityIcon = '<i class="fa fa-times-circle red"></i>';
                                                                    }
                                                                    else if (patEligibility.toLowerCase() == "waiting") {
                                                                        eligibilityIcon = '<i class="fa fa-circle black"></i>';
                                                                    }
                                                                    else {
                                                                        eligibilityIcon = '<i class="fa fa-circle blue"></i>';
                                                                    }
                                                                    //END Edit By Mohsin Nasir Bug # 2922,2931

                                                                    appcopay = appcopay.split('-');
                                                                    var copaycolor;
                                                                    //var copaycolor = appcopay[1];
                                                                    // appcopay = appcopay[0];

                                                                    //For last Status
                                                                    var lastStatusId = split[14];
                                                                    var lastStatusName = split[15];
                                                                    var patientType = split[17];
                                                                    var visitType = split[18];
                                                                    var isNonBillable = split[19];
                                                                    var appcopayamt = parseInt(appcopay[0]);
                                                                    //var appcopayamtpaid = parseInt(appcopay[1]);
                                                                    if (appcopay.length == 3) {
                                                                        var appcopayamtpaid = -Math.abs(parseFloat(appcopay[2]));
                                                                    } else {
                                                                        var appcopayamtpaid = parseFloat(appcopay[1]);
                                                                    }



                                                                    if (appcopayamtpaid == 0) {
                                                                        copaycolor = 'Green'; //copay paid
                                                                    }
                                                                    if (appcopayamtpaid > 0) {
                                                                        copaycolor = 'Black';//copay blance
                                                                    }
                                                                    if (appcopayamtpaid == appcopayamt && appcopayamtpaid > 0) {
                                                                        appcopay = appcopayamt;
                                                                        copaycolor = 'Red'; // amount copay
                                                                    }

                                                                    appids.push(appid);

                                                                    for (var w = 0; w < appstatus.length; w++) {
                                                                        if (appstatus[w].Name.toUpperCase() != "- SELECT -" && appstatus[w].Name != appointmentstatus && appstatus[w].Name.toUpperCase() != "CHECK IN" && appstatus[w].Name.toUpperCase() != "CHECK OUT")
                                                                            appstatuses += '<li data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id=' + appstatus[w].Value + ' ><a onclick="Scheduling_MuliView.AppointmentStatusUpdate(' + appid + ',' + appstatus[w].Value + ',\'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');" href="#" >' + appstatus[w].Name + '</a></li> ';
                                                                    }
                                                                    appstatuses = appstatuses.replace('undefined', '');
                                                                    var showapp = 0;

                                                                    for (z = 0; z < appids.length; z++) {
                                                                        if (appids[z] == appid && appid != 0) {
                                                                            showapp = showapp + 1;
                                                                        }

                                                                    }
                                                                    /* Start Code for Reminder Call */
                                                                    var sms = "SMS";
                                                                    var voice = "VOICE";
                                                                    var email = "EMAIL";
                                                                    var Patient_email = "";
                                                                    var Appointmentdatetime = "";

                                                                    try {
                                                                        Patient_email = Slot_Detail[i].PatientDetail.split(',')[10];
                                                                        Appointmentdatetime = $('#pnlScheduleMuliView #searchdate').val() + " " + Slot_Detail[i].FromTimeSlots;
                                                                    } catch (ex) {
                                                                        console.log(ex);
                                                                    }
                                                                    reminderType = '<li id=' + appid + ' ><a onclick="Scheduling_MuliView.SendQuickReminder(' + appid + ',\'' + sms + '\',' + patientid + ',\'' + patientname + '\',\'' + Patient_email + '\',\'' + Appointmentdatetime + '\',\'' + Slot_Detail[i].ProviderId + '\',\'' + Slot_Detail[i].ProviderName + '\');" href="#" >SMS</a></li> <li id=' + appid + ' ><a onclick="Scheduling_MuliView.SendQuickReminder(' + appid + ',\'' + voice + '\',' + patientid + ',\'' + patientname + '\',\'' + Patient_email + '\',\'' + Appointmentdatetime + '\',\'' + Slot_Detail[i].ProviderId + '\',\'' + Slot_Detail[i].ProviderName + '\');" href="#" >Voice</a></li> <li id=' + appid + ' ><a onclick="Scheduling_MuliView.SendQuickReminder(' + appid + ',\'' + email + '\',' + patientid + ',\'' + patientname + '\',\'' + Patient_email + '\',\'' + Appointmentdatetime + '\',\'' + Slot_Detail[i].ProviderId + '\',\'' + Slot_Detail[i].ProviderName + '\');" href="#" >Email</a></li> ';
                                                                    /* End Code for Reminder Call*/
                                                                    if ($.trim(appcount) > '1' && showapp == 1) {

                                                                        if (appointmentstatus.toUpperCase() != 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="providerappdrag apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckIn(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\', \'' + dateid + '\');">Reschedule</a></li>    <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                        if (appointmentstatus.toUpperCase() == 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="providerappdrag apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckIn(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\', \'' + dateid + '\');">Reschedule</a></li>    <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                        //create note task
                                                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() != "SIGNED") {

                                                                            var eSuperbillLink = Scheduling_Calendar.getESuperBillLink(patientid, patientvisitid, noteid, "day", patientType, provid, refprovid, facid, appid, billInfoId, billInfoStatus);
                                                                            if (isNonBillable == "1") {
                                                                                eSuperbillLink = '';
                                                                            }
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo"  onclick=" Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ');">Check Out</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CancelPatientCheckIn(' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + appid + ',this);">Cancel CheckIn</a></li> <li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadClinicalNote(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\',\'' + provid + '\',\'' + provname + '\',\'' + facid + '\',\'' + facname + '\',null);">' + noteTitle + '</a></li> <li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#" >Create Charge</a></li><li  id="' + appid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.PrintLetter(' + patientid + ',\'' + patientvisitid + '\',\'' + appid + '\');" >Print Super Bill</a></li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '</ul> </li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> <li  id="' + patientvisitid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.createSuperbill(\'' + patientid.trim() + '\',\'' + patientvisitid.trim() + '\',\'' + noteid.trim() + '\',\'-1\',\'day\',\'' + patientType.trim() + '\',\'' + provid + '\',\'' + refprovid + '\',\'' + facid + '\',\'' + appid + '\');" >Create eSuperbill</a></li> </div></div> ';
                                                                        }
                                                                        //


                                                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() == "SIGNED")
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + '<a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + patientvisitid + ', \'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',1,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Check Out</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CancelPatientCheckIn(' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + appid + ',this);">Cancel CheckIn</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CancelPatientCheckIn(' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + appid + ',this);">Cancel CheckIn</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CancelPatientCheckIn(' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + appid + ',this);">Cancel CheckIn</a></li><li  id="' + appid + '"><a href="#" >Create Charge</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PrintLetter();">Print Super Bill</a></li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + '<a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + patientvisitid + ', \'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',1,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li>  </ul>  </div></div> ';
                                                                        if (appointmentstatus.toUpperCase() == 'CANCEL' || appointmentstatus.toUpperCase() == "NO SHOW")
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + '<a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',2,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>  <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                        if (patientvisitid != "" && patvisitname.toUpperCase() != 'CHECK IN' && patvisitname.toUpperCase() != 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + '<a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',2,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                    }

                                                                    if ($.trim(appcount) == '1' && showapp == 1) {

                                                                        if (appointmentstatus.toUpperCase() != 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="providerappdrag apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckIn(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\', \'' + dateid + '\');">Reschedule</a></li>   <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                        if (appointmentstatus.toUpperCase() == 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="providerappdrag apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + ' </div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckIn(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\', \'' + dateid + '\');">Reschedule</a></li>         <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                        // create note
                                                                        if (patientvisitid != "" && (patvisitname.toUpperCase() == 'CHECK IN') && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() != "SIGNED") {

                                                                            var eSuperbillLink = Scheduling_Calendar.getESuperBillLink(patientid, patientvisitid, noteid, "day", patientType, provid, refprovid, facid, appid, billInfoId, billInfoStatus);
                                                                            if (isNonBillable == "1") {
                                                                                eSuperbillLink = '';
                                                                            }
                                                                            //appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo"  onclick=" Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1);">Edit Appointment</a></li>      <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ');">Check Out</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CancelPatientCheckIn(' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + appid + ',this);">Cancel CheckIn</a></li> <li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadClinicalNote(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\',\'' + provid + '\',\'' + provname + '\',\'' + facid + '\',\'' + facname + '\');">' + noteTitle + '</a></li> <li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#" >Create Charge</a></li><li  id="' + appid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.PrintLetter(' + patientid + ',\'' + patientvisitid + '\',\'' + appid + '\');" >Print Super Bill</a></li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '</ul></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li>  <li  id="' + patientvisitid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.createSuperbill(\'' + patientid.trim() + '\',\'' + patientvisitid.trim() + '\',\'' + noteid.trim() + '\',\'-1\',\'day\',\'' + patientType.trim() + '\',\'' + provid + '\',\'' + refprovid + '\',\'' + facid + '\',\'' + appid + '\');" >Create eSuperbill</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li></div></div> ';
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo"  onclick=" Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ');">Check Out</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CancelPatientCheckIn(' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + appid + ',this);">Cancel CheckIn</a></li> <li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadClinicalNote(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\',\'' + provid + '\',\'' + provname + '\',\'' + facid + '\',\'' + facname + '\',null);">' + noteTitle + '</a></li> <li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#" >Create Charge</a></li><li  id="' + appid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.PrintLetter(' + patientid + ',\'' + patientvisitid + '\',\'' + appid + '\');" >Print Super Bill</a></li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '</ul></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li>' + eSuperbillLink + '</div></div> '; //'<li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li>

                                                                        }
                                                                        //
                                                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() == "SIGNED")
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + ' </div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + patientvisitid + ', \'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',1,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>     <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Check Out</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CancelPatientCheckIn(' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + appid + ',this);">Cancel CheckIn</a></li><li  id="' + appid + '"><a href="#" >Create Charge</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PrintLetter();">Print Super Bill</a></li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL') {

                                                                            var eSuperbillLink = Scheduling_Calendar.getESuperBillLink(patientid, patientvisitid, noteid, "day", patientType, provid, refprovid, facid, appid, billInfoId, billInfoStatus);
                                                                            if (isNonBillable == "1") {
                                                                                eSuperbillLink = '';
                                                                            }


                                                                            if ($(eSuperbillLink).find('a').text().toLowerCase() == "create esuperbill" && noteTitle.toLowerCase() == "create note") {
                                                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li>' + eSuperbillLink + '<li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\');">' + noteTitle + '</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> </ul>  </div></div> ';
                                                                            }
                                                                            else if ($(eSuperbillLink).find('a').text().toLowerCase() == "edit esuperbill" && noteTitle.toLowerCase() == "create note") {
                                                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\');">' + noteTitle + '</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> ' + eSuperbillLink + '  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> </ul>  </div></div> ';
                                                                            }
                                                                            else if ($(eSuperbillLink).find('a').text().toLowerCase() == "create esuperbill" && noteTitle.toLowerCase() == "edit note") {
                                                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li>' + eSuperbillLink + ' <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\');">' + noteTitle + '</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> </ul>  </div></div> ';
                                                                            }
                                                                            else if ($(eSuperbillLink).find('a').text().toLowerCase() == "view esuperbill" && noteTitle.toLowerCase() == "create note") {
                                                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\');">' + noteTitle + '</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#">View Charge</a></li>' + eSuperbillLink + ' </ul>  </div></div> ';
                                                                            }
                                                                            else if ($(eSuperbillLink).find('a').text().toLowerCase() == "view esuperbill" && noteTitle.toLowerCase() == "edit note") {
                                                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\');">' + noteTitle + '</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#">View Charge</a></li>' + eSuperbillLink + ' </ul>  </div></div> ';
                                                                            }
                                                                            else if ($(eSuperbillLink).find('a').text().toLowerCase() == "view esuperbill" && noteTitle.toLowerCase() == "view note") {
                                                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#">View Charge</a></li>' + eSuperbillLink + '  <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\');">' + noteTitle + '</a></li></ul>  </div></div> ';
                                                                            }
                                                                            else if ($(eSuperbillLink).find('a').text().toLowerCase() == "create esuperbill" && noteTitle.toLowerCase() == "view note") {
                                                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li>' + eSuperbillLink + ' <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\');">' + noteTitle + '</a></li>  </ul>  </div></div> ';
                                                                            }
                                                                            else if ($(eSuperbillLink).find('a').text().toLowerCase() == "edit esuperbill" && noteTitle.toLowerCase() == "view note") {
                                                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> ' + eSuperbillLink + '  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\');">' + noteTitle + '</a></li>  </ul>  </div></div> ';
                                                                            }
                                                                            else if ($(eSuperbillLink).find('a').text().toLowerCase() == "edit esuperbill" && noteTitle.toLowerCase() == "edit note") {
                                                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> ' + eSuperbillLink + ' <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\');">' + noteTitle + '</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li>  </ul>  </div></div> ';
                                                                            }






















                                                                            //if ($(eSuperbillLink).find('a').text() == "View eSuperbill") {
                                                                            //    appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + '<a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + ' </div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + patientvisitid + ', \'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',1,\'' + patientvisitid + '\');">Edit Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> <li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadClinicalNote(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\',\'' + provid + '\',\'' + provname + '\',\'' + facid + '\',\'' + facname + '\',\'' + isViewNote + '\');">' + noteTitle + '</a></li>' + eSuperbillLink + ' <li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#" >View Charge</a></li>  </ul>  </div></div> ';
                                                                            //} else {
                                                                            //    appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + '<a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + ' </div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + patientvisitid + ', \'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',1,\'' + patientvisitid + '\');">Edit Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> <li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadClinicalNote(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\',\'' + provid + '\',\'' + provname + '\',\'' + facid + '\',\'' + facname + '\',\'' + isViewNote + '\');">' + noteTitle + '</a></li>' + eSuperbillLink + '   </ul>  </div></div> ';
                                                                            //}




                                                                        }
                                                                        if (appointmentstatus.toUpperCase() == 'CANCEL' || appointmentstatus.toUpperCase() == "NO SHOW")
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',2,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>  <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                        if (patientvisitid != "" && patvisitname.toUpperCase() != 'CHECK IN' && patvisitname.toUpperCase() != 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + '<a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',2,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li>  </ul>  </div></div> ';

                                                                    }
                                                                    if (showapp != 1) {
                                                                        // appointments = appointments + '<div  style="width:250px;" </div> ';
                                                                    }
                                                                    appointments = appointments.replace("undefined", "");
                                                                    appstatuses = '';
                                                                    test++;
                                                                }
                                                            }

                                                        }
                                                        var dateid = 'pdate' + provideridss + '';
                                                        if (Slot_Detail[i].AppDtl == "" && Slot_Detail[i].BlockUnblock == "Blocked") {
                                                            table.append('<tr id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + '"><td onclick="Scheduling_MuliView.EditSlot(' + Slot_Detail[i].TimeSlotDtlId + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');" width="63">' + Slot_Detail[i].FromTimeSlots + '</td><td id="' + Slot_Detail[i].TimeSlotDtlId + '"  width="25" align="center">' + Slot_Detail[i].PatientBooked + '</td><td style="color:#fff;"  BGCOLOR="#f88379" id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + '" >Blocked:' + Slot_Detail[i].BlockReason + '-' + Slot_Detail[i].Comments + ' </td></tr>');
                                                        }
                                                        else if (appointments == '<div class="pull-left">&nbsp;</div>') {
                                                            table.append('<tr id="' + Slot_Detail[i].Patients + '"><td onclick="Scheduling_MuliView.EditSlot(' + Slot_Detail[i].TimeSlotDtlId + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');" width="63">' + Slot_Detail[i].FromTimeSlots + '</td><td id="' + Slot_Detail[i].TimeSlotDtlId + '"  width="25" align="center">' + Slot_Detail[i].PatientBooked + '</td><td class="providerappdrop slot" ' + color + ' style="position:relative;" onclick="Scheduling_MuliView.AppointmentAdd(' + Slot_Detail[i].TimeSlotId + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + provid + ', \'' + provname + '\',' + resid + ',\'' + resname + '\',' + facid + ',\'' + facname + '\',' + resonid + ',\'' + dateid + '\',event,\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + Slot_Detail[i].ToTimeSlots + '\',\'' + Slot_Detail[i].IsSpecialist + '\',\'' + Slot_Detail[i].SlotMinutes + '\', \'' + Slot_Detail[i].BlockReason + '\',\'' + Slot_Detail[i].ScheduleDate + '\');" id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + ',' + Slot_Detail[i].Patients + ',pdate' + provid + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '">' + appointments + '</td></tr>');
                                                        }
                                                        else {

                                                            table.append('<tr id="' + Slot_Detail[i].Patients + '"><td onclick="Scheduling_MuliView.EditSlot(' + Slot_Detail[i].TimeSlotDtlId + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');" width="63">' + Slot_Detail[i].FromTimeSlots + '</td><td id="' + Slot_Detail[i].TimeSlotDtlId + '"  width="25" align="center">' + Slot_Detail[i].PatientBooked + '</td><td class="providerappdrop slot" ' + color + ' style="position:relative;"   onclick="Scheduling_MuliView.AppointmentAdd(' + Slot_Detail[i].TimeSlotId + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + provid + ', \'' + provname + '\',' + resid + ',\'' + resname + '\',' + facid + ',\'' + facname + '\',' + resonid + ',\'' + dateid + '\',event,\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + Slot_Detail[i].ToTimeSlots + '\',\'' + Slot_Detail[i].IsSpecialist + '\',\'' + Slot_Detail[i].SlotMinutes + '\', \'' + Slot_Detail[i].BlockReason + '\',\'' + Slot_Detail[i].ScheduleDate + '\');" id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + ',' + Slot_Detail[i].Patients + ',pdate' + provid + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"  >' + appointments + '</td></tr>');
                                                        }
                                                        appointments = '';
                                                        PatientsDtl.push(Slot_Detail[i].PatientDetail);
                                                        AppointmentsDtl.push(Slot_Detail[i].AppDtl);
                                                    }
                                                    $('#facility' + facilityidss + ' #provider' + provideridss + '').append(table);
                                                    Scheduling_MuliView.MoveProviderAppointment();
                                                    Scheduling_MuliView.dropDownMenuClick(Scheduling_MuliView.params.PanelID);
                                                    if (PatientsDtl.length == AppointmentsDtl.length) {
                                                        for (var ini = 0; ini < PatientsDtl.length; ini++) {
                                                            Scheduling_MuliView.MultiViewPatientDetails(PatientsDtl[ini], AppointmentsDtl[ini], ProviderName);
                                                        }
                                                        PatientsDtl = [];
                                                        AppointmentsDtl = [];
                                                    }

                                                }
                                            }
                                            else {
                                                var providerwidth = 100 / (parseInt(JSONData[mi].ProCount) + parseInt(JSONData[mi].ResCount));
                                                var ProviderName = $("#Provider option[value=" + provideridss + "]").text();
                                                $('#facility' + facilityidss + '').append('<div class="th-text-center " id="provider' + provideridss + '" style="width:' + providerwidth + '%; float:left; "><div class=" multiple-heading center">Provider:' + ProviderName + '</div></div>');
                                                var table = $('<table class="table table-responsive table-bordered table-condensed" id="table' + provideridss + '"><tbody></tbody></table>');
                                                $('#table' + provideridss + '').children('tbody');
                                                table.append('<tr align="center"><td colspan="3"><div class="col-sm-12">   <div class="col-xs-1 pl-none pr-none"><a id="pdate' + provideridss + '" providerid="' + provideridss + '" facilityid="' + facilityidss + '" resourceid="" tableid="table' + provideridss + '" class="btn btn-link btn-xs bdate"><i class="fa fa-angle-left"></i></a></div>    <div class="col-xs-9" id="pdate' + provideridss + '">   <p ><span>' + criteriaDate + '</span></p>      </div>        <div class="col-xs-1 pl-none pr-none"><a id="pdate' + provideridss + '" providerid="' + provideridss + '" facilityid="' + facilityidss + '" resourceid="" tableid="ptable' + provideridss + '" class="btn btn-link btn-xs fdate"><i class="fa fa-angle-right "></i></a></div> <div class="col-xs-1 pl-none pr-none"><a data-plugin-datepicker=""   id="pcalendar' + provideridss + facilityidss + '" dateid="pdate' + provideridss + '" providerid="' + provideridss + '" facilityid="' + facilityidss + '" resourceid="" class="btn btn-link btn-xs form-control cal"><i class="fa fa-calendar cal"></i></a></div>                                    </div> </td> </tr>');
                                                table.append('<tr align="center"><th style="text-align: center;" >No Schedule Found.</th>  </tr>');
                                                $('#facility' + facilityidss + ' #provider' + provideridss + '').append(table);
                                            }
                                        }
                                    }
                                }
                                for (var rid = 0; rid < resourceids.length; rid++) {
                                    var resourceidss = $.trim(resourceids[rid]);
                                    var findid = "Resource" + resourceidss + "Facility" + facilityidss;
                                    for (var x = 0; x < SchRecord.length; x++) {
                                        var json = JSON.parse(SchRecord[x]);
                                        var id = json.id;
                                        if (findid == id) {
                                            if (json.status != false) {
                                                var PatientsDtl = [];
                                                var AppointmentsDtl = [];
                                                var Slot_Detail = JSON.parse(json.ProviderScheduleFill_JSON);
                                                var resourcerwidth = 100 / (parseInt(JSONData[mi].ProCount) + parseInt(JSONData[mi].ResCount));
                                                var ResourceName = $("#Resource option[value=" + resourceidss + "]").text();
                                                $('#facility' + facilityidss + '').append('<div class="th-text-center " id="resource' + resourceidss + '" style="width:' + resourcerwidth + '%; float:left; "><div class=" multiple-heading center">Resource:' + ResourceName + '</div></div>');
                                                if (Slot_Detail.length != 0) {
                                                    var table = $('<table  class="table table-responsive table-bordered table-condensed" id="table' + resourceidss + '"><tbody></tbody></table>');
                                                    $('#table' + resourceidss + '').children('tbody');
                                                    table.append('<tr align="center"><td colspan="3"><div class="col-sm-12">   <div class="col-xs-1 pl-none pr-none"><a id="rdate' + resourceidss + '" providerid="" facilityid="' + facilityidss + '" resourceid="' + resourceidss + '" tableid="rtable' + resourceidss + '" class="btn btn-link btn-xs bdate"><i class="fa fa-angle-left"></i></a></div>    <div class="col-xs-9" id="rdate' + resourceidss + '">   <p ><span>' + criteriaDate + '</span></p>      </div>        <div class="col-xs-1 pl-none pr-none"><a id="rdate' + resourceidss + '" providerid="" facilityid="' + facilityidss + '" resourceid="' + resourceidss + '" tableid="rtable' + resourceidss + '" class="btn btn-link btn-xs fdate"><i class="fa fa-angle-right "></i></a></div> <div class="col-xs-1 pl-none pr-none"><a data-plugin-datepicker=""   id="rcalendar' + resourceidss + facilityidss + '" dateid="rdate' + resourceidss + '" providerid="" facilityid="' + facilityidss + '" resourceid="' + resourceidss + '" class="btn btn-link btn-xs form-control cal"><i class="fa fa-calendar cal"></i></a></div>                                </div> </td> </tr>');
                                                    var appids = [];
                                                    var slotappcount;
                                                    for (var i = 0; i < Slot_Detail.length; i++) {
                                                        var appointment;
                                                        var margin = Slot_Detail[i].MaxCountApp;

                                                        var provid = Slot_Detail[i].ProviderId;
                                                        if (provid == '' || provid == 'undefined')
                                                            provid = null;

                                                        var refprovid = Slot_Detail[i].RefProviderId;
                                                        if (refprovid == '' || refprovid == 'undefined')
                                                            refprovid = null;

                                                        var provname = Slot_Detail[i].ProviderName;
                                                        if (provname == '' || provname == 'undefined')
                                                            provname = null;

                                                        var resid = Slot_Detail[i].ResourceId;
                                                        if (resid == '' || resid == 'undefined')
                                                            resid = null;
                                                        var resname = Slot_Detail[i].ResourceName;
                                                        if (resname == '' || resname == 'undefined')
                                                            resname = null;
                                                        var facid = Slot_Detail[i].FacilityId;
                                                        if (facid == '' || facid == 'undefined')
                                                            facid = null;
                                                        var facname = Slot_Detail[i].FacilityName;
                                                        if (facname == '' || facname == 'undefined')
                                                            facname = null;
                                                        var resonid = Slot_Detail[i].ScheduleReasonId;
                                                        if (resonid == '' || resonid == 'undefined')
                                                            resonid = null;

                                                        var color = "";

                                                        if (Slot_Detail[i].FacilityColor == "" || Slot_Detail[i].FacilityColor == "undefined" || Slot_Detail[i].FacilityColor == "null" || Slot_Detail[i].FacilityColor == null) {
                                                            color = '';
                                                        } else {
                                                            color = 'bgcolor = \'' + Slot_Detail[i].FacilityColor + '\'';
                                                        }

                                                        var dateid = 'rdate' + resourceidss + '';

                                                        var appointments = '<div class="pull-left">&nbsp;</div>';
                                                        if (Slot_Detail[i].AppDtl != "") {
                                                            var app;
                                                            var test = 0;

                                                            var data = Slot_Detail[i].AppDtl;
                                                            var arr = data.split('|');

                                                            for (var j = 0; j < arr.length; j++) {
                                                                slotappcount = (arr.length) - 1;
                                                                var split = arr[j].split(',')
                                                                if (split.length == 22 || split.length == 24) {

                                                                    var appid = split[0];
                                                                    var patientid = split[1];
                                                                    var patientname = split[2];
                                                                    patientname = patientname.replace('-', ', ');
                                                                    var appointmentstatus = split[3];
                                                                    var appcolor = split[4];
                                                                    var appcopay = split[6];
                                                                    var noteStatus = split[16];
                                                                    var appreason = split[3];
                                                                    var appcount = split[7];
                                                                    var patientvisitid = split[9];
                                                                    var appcomments = split[8];
                                                                    var patvisitstatusid = split[10];
                                                                    var patvisitname = split[11];
                                                                    //Edit By Mohsin Nasir Bug # 2922,2931
                                                                    var patEligibility = split[12];
                                                                    var appschreason = split[5];
                                                                    appschreason = appschreason.replace(/#@#/g, ',');
                                                                    var noteid = split[13];

                                                                    var noteTitle = "";
                                                                    var isNoteCreated = false;

                                                                    if (noteid == "0") {
                                                                        noteTitle = "Create Note";
                                                                        isNoteCreated = false;
                                                                    } else {
                                                                        noteTitle = "Edit Note";
                                                                        isNoteCreated = true;
                                                                    }
                                                                    var eligibilityIcon;

                                                                    if (patEligibility == "Active") {
                                                                        eligibilityIcon = '<i class="fa fa-check-circle green"></i>';
                                                                    }
                                                                    else if (patEligibility == "Inactive") {
                                                                        eligibilityIcon = '<i class="fa fa-times-circle red"></i>';
                                                                    }
                                                                    else if (patEligibility.toLowerCase() == "waiting") {
                                                                        eligibilityIcon = '<i class="fa fa-circle black"></i>';
                                                                    }
                                                                    else {
                                                                        eligibilityIcon = '<i class="fa fa-circle blue"></i>';
                                                                    }
                                                                    //END Edit By Mohsin Nasir Bug # 2922,2931
                                                                    appcopay = appcopay.split('-');
                                                                    var copaycolor;
                                                                    //var copaycolor = appcopay[1];
                                                                    // appcopay = appcopay[0];

                                                                    //For last Status
                                                                    var lastStatusId = split[14];
                                                                    var lastStatusName = split[15];
                                                                    var patientType = split[17];
                                                                    var visitType = split[18];
                                                                    var isNonBillable = split[19];
                                                                    var appcopayamt = parseInt(appcopay[0]);
                                                                    if (appcopay.length == 3) {
                                                                        var appcopayamtpaid = -Math.abs(parseFloat(appcopay[2])); // In Resource Case
                                                                    } else {
                                                                        var appcopayamtpaid = parseFloat(appcopay[1]);    // In Provider Case
                                                                    }
                                                                    var billInfoId = split[20];
                                                                    var billInfoStatus = split[21];
                                                                    if ((provid == null || provid == "") && split.length == 24) {
                                                                        provid = split[22];
                                                                        provname = split[23];
                                                                        if (split[21].indexOf("-") > 0) {
                                                                            provname = split[21].replace("-", ", ");
                                                                        }
                                                                    }
                                                                    if (appcopayamtpaid == 0) {
                                                                        copaycolor = 'Green'; //copay paid
                                                                    }
                                                                    if (appcopayamtpaid > 0) {
                                                                        copaycolor = 'Black';//copay blance
                                                                    }
                                                                    if (appcopayamtpaid == appcopayamt && appcopayamtpaid > 0) {
                                                                        appcopay = appcopayamt;
                                                                        copaycolor = 'Red'; // amount copay
                                                                    }

                                                                    appids.push(appid);

                                                                    for (var w = 0; w < appstatus.length; w++) {
                                                                        if (appstatus[w].Name.toUpperCase() != "- SELECT -" && appstatus[w].Name != appointmentstatus && appstatus[w].Name.toUpperCase() != "CHECK IN" && appstatus[w].Name.toUpperCase() != "CHECK OUT")
                                                                            appstatuses += '<li data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id=' + appstatus[w].Value + ' ><a onclick="Scheduling_MuliView.AppointmentStatusUpdate(' + appid + ',' + appstatus[w].Value + ',\'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');" href="#" >' + appstatus[w].Name + '</a></li> ';
                                                                    }
                                                                    appstatuses = appstatuses.replace('undefined', '');
                                                                    var showapp = 0;

                                                                    for (z = 0; z < appids.length; z++) {
                                                                        if (appids[z] == appid && appid != 0) {
                                                                            showapp = showapp + 1;
                                                                        }

                                                                    }
                                                                    /* Start Code for Reminder Call */
                                                                    var sms = "SMS";
                                                                    var voice = "VOICE";
                                                                    var email = "EMAIL";
                                                                    var Patient_email = "";
                                                                    var Appointmentdatetime = "";

                                                                    try {
                                                                        Patient_email = Slot_Detail[i].PatientDetail.split(',')[10];
                                                                        Appointmentdatetime = $('#pnlScheduleMuliView #searchdate').val() + " " + Slot_Detail[i].FromTimeSlots;
                                                                    } catch (ex) {
                                                                        console.log(ex);
                                                                    }
                                                                    reminderType = '<li id=' + appid + ' ><a onclick="Scheduling_MuliView.SendQuickReminder(' + appid + ',\'' + sms + '\',' + patientid + ',\'' + patientname + '\',\'' + Patient_email + '\',\'' + Appointmentdatetime + '\',\'' + Slot_Detail[i].ProviderId + '\',\'' + Slot_Detail[i].ProviderName + '\');" href="#" >SMS</a></li> <li id=' + appid + ' ><a onclick="Scheduling_MuliView.SendQuickReminder(' + appid + ',\'' + voice + '\',' + patientid + ',\'' + patientname + '\',\'' + Patient_email + '\',\'' + Appointmentdatetime + '\',\'' + Slot_Detail[i].ProviderId + '\',\'' + Slot_Detail[i].ProviderName + '\');" href="#" >Voice</a></li> <li id=' + appid + ' ><a onclick="Scheduling_MuliView.SendQuickReminder(' + appid + ',\'' + email + '\',' + patientid + ',\'' + patientname + '\',\'' + Patient_email + '\',\'' + Appointmentdatetime + '\',\'' + Slot_Detail[i].ProviderId + '\',\'' + Slot_Detail[i].ProviderName + '\');" href="#" >Email</a></li> ';
                                                                    /* End Code for Reminder Call*/
                                                                    if (appcount > '1' && showapp == 1) {

                                                                        if (appointmentstatus.toUpperCase() != 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="resourceappdrag apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckIn(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\', \'' + dateid + '\');">Reschedule</a></li>   <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                        if (appointmentstatus.toUpperCase() == 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="resourceappdrag apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckIn(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\', \'' + dateid + '\');">Reschedule</a></li>   <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() != "SIGNED") {
                                                                            var eSuperbillLink = Scheduling_Calendar.getESuperBillLink(patientid, patientvisitid, noteid, "day", patientType, provid, refprovid, facid, appid, billInfoId, billInfoStatus);
                                                                            if (isNonBillable == "1") {
                                                                                eSuperbillLink = '';
                                                                            }
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + patientvisitid + ', \'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',1,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Check Out</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CancelPatientCheckIn(' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + appid + ',this);">Cancel CheckIn</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadClinicalNote(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\',\'' + provid + '\',\'' + provname + '\',\'' + facid + '\',\'' + facname + '\',null,' + resid + ');">' + noteTitle + '</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateVisitCharge(' + appid + ',' + patientid + ',' + patientvisitid + ');" >Create Charge</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PrintLetter();">Print Super Bill</a></li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li><li  id="' + patientvisitid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.createSuperbill(\'' + patientid.trim() + '\',\'' + patientvisitid.trim() + '\',\'' + noteid.trim() + '\',\'-1\',\'day\',\'' + patientType.trim() + '\',\'' + provid + '\',\'' + refprovid + '\',\'' + facid + '\',\'' + appid + '\');" >Create eSuperbill</a></li> </ul>  </div></div> ';
                                                                        }
                                                                        //if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() != "SIGNED") {

                                                                        //    var eSuperbillLink = Scheduling_Calendar.getESuperBillLink(patientid, patientvisitid, noteid, "day", patientType, provid, refprovid, facid, appid, billInfoId, billInfoStatus);
                                                                        //    if (isNonBillable == "1") {
                                                                        //        eSuperbillLink = '';
                                                                        //    }
                                                                        //    appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo"  onclick=" Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ');">Check Out</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CancelPatientCheckIn(' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + appid + ',this);">Cancel CheckIn</a></li> <li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadClinicalNote(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\',\'' + provid + '\',\'' + provname + '\',\'' + facid + '\',\'' + facname + '\',null);">' + noteTitle + '</a></li> <li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#" >Create Charge</a></li><li  id="' + appid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.PrintLetter(' + patientid + ',\'' + patientvisitid + '\',\'' + appid + '\');" >Print Super Bill</a></li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '</ul> </li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> <li  id="' + patientvisitid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.createSuperbill(\'' + patientid.trim() + '\',\'' + patientvisitid.trim() + '\',\'' + noteid.trim() + '\',\'-1\',\'day\',\'' + patientType.trim() + '\',\'' + provid + '\',\'' + refprovid + '\',\'' + facid + '\',\'' + appid + '\');" >Create eSuperbill</a></li> </div></div> ';
                                                                        //}
                                                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() == "SIGNED")
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + patientvisitid + ', \'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',1,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Check Out</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CancelPatientCheckIn(' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + appid + ',this);">Cancel CheckIn</a></li><li  id="' + appid + '"><a href="#">Create Charge</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PrintLetter();">Print Super Bill</a></li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li></ul>  </div></div> ';
                                                                            if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + patientvisitid + ', \'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',1,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li>    </ul>  </div></div> ';
                                                                        if (appointmentstatus.toUpperCase() == 'CANCEL' || appointmentstatus.toUpperCase() == "NO SHOW")
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',2,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                        if (patientvisitid != "" && patvisitname.toUpperCase() != 'CHECK IN' && patvisitname.toUpperCase() != 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul  data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',2,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                    }

                                                                    if (appcount == '1' && showapp == 1) {

                                                                        if (appointmentstatus.toUpperCase() != 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="resourceappdrag apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + ' </div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckIn(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\', \'' + dateid + '\');">Reschedule</a></li>     <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                        if (appointmentstatus.toUpperCase() == 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="resourceappdrag apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + ' </div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckIn(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li>      <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\', \'' + dateid + '\');">Reschedule</a></li>      <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() != "SIGNED") {
                                                                            // appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + ' </div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + patientvisitid + ', \'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',1,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Check Out</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CancelPatientCheckIn(' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + appid + ',this);">Cancel CheckIn</a></li><li  id="' + appid + '"><a href="#" >Create Charge</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PrintLetter();">Print Super Bill</a></li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li></ul>  </div></div> ';
                                                                            var eSuperbillLink = Scheduling_Calendar.getESuperBillLink(patientid, patientvisitid, noteid, "day", patientType, provid, refprovid, facid, appid, billInfoId, billInfoStatus);
                                                                            if (isNonBillable == "1") {
                                                                                eSuperbillLink = '';
                                                                            }
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + patientvisitid + ', \'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',1,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Check Out</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CancelPatientCheckIn(' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + appid + ',this);">Cancel CheckIn</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadClinicalNote(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\',\'' + provid + '\',\'' + provname + '\',\'' + facid + '\',\'' + facname + '\',null,' + resid + ');">' + noteTitle + '</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateVisitCharge(' + appid + ',' + patientid + ',' + patientvisitid + ');" >Create Charge</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PrintLetter();">Print Super Bill</a></li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li><li  id="' + patientvisitid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.createSuperbill(\'' + patientid.trim() + '\',\'' + patientvisitid.trim() + '\',\'' + noteid.trim() + '\',\'-1\',\'day\',\'' + patientType.trim() + '\',\'' + provid + '\',\'' + refprovid + '\',\'' + facid + '\',\'' + appid + '\');" >Create eSuperbill</a></li> </ul>  </div></div> ';
                                                                        }
                                                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() == "SIGNED")
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + patientvisitid + ', \'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',1,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Check Out</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CancelPatientCheckIn(' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + appid + ',this);">Cancel CheckIn</a></li><li  id="' + appid + '"><a href="#" >Create Charge</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PrintLetter();">Print Super Bill</a></li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li></ul>  </div></div> ';
                                                                            if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + ' </div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + patientvisitid + ', \'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',1,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li>  </ul>  </div></div> ';
                                                                        if (appointmentstatus.toUpperCase() == 'CANCEL' || appointmentstatus.toUpperCase() == "NO SHOW")
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',2,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li>  </ul>  </div></div> ';
                                                                        if (patientvisitid != "" && patvisitname.toUpperCase() != 'CHECK IN' && patvisitname.toUpperCase() != 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + '<a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',2,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                    }
                                                                    if (showapp != 1) {
                                                                        // appointments = appointments + '<div  style="width:250px;" </div> ';
                                                                    }
                                                                    appointments = appointments.replace("undefined", "");
                                                                    appstatuses = '';
                                                                    test++;
                                                                }
                                                            }

                                                        }
                                                        var dateid = 'rdate' + resourceidss + '';
                                                        if (Slot_Detail[i].AppDtl == "" && Slot_Detail[i].BlockUnblock == "Blocked") {
                                                            table.append('<tr id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + '"><td onclick="Scheduling_MuliView.EditSlot(' + Slot_Detail[i].TimeSlotDtlId + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');" width="63">' + Slot_Detail[i].FromTimeSlots + '</td><td id="' + Slot_Detail[i].TimeSlotDtlId + '"  width="25" align="center">' + Slot_Detail[i].PatientBooked + '</td><td style="color:#fff;" BGCOLOR="#f88379" id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + '" >Blocked:' + Slot_Detail[i].BlockReason + '-' + Slot_Detail[i].Comments + ' </td></tr>');
                                                        }
                                                        else if (appointments == '<div class="pull-left">&nbsp;</div>') {
                                                            table.append('<tr id="' + Slot_Detail[i].Patients + '"><td onclick="Scheduling_MuliView.EditSlot(' + Slot_Detail[i].TimeSlotDtlId + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');" width="63">' + Slot_Detail[i].FromTimeSlots + '</td><td id="' + Slot_Detail[i].TimeSlotDtlId + '"  width="25" align="center">' + Slot_Detail[i].PatientBooked + '</td><td class="resourceappdrop slot" ' + color + ' style="position:relative;" onclick="Scheduling_MuliView.AppointmentAdd(' + Slot_Detail[i].TimeSlotId + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + provid + ', \'' + provname + '\',' + resid + ',\'' + resname + '\',' + facid + ',\'' + facname + '\',' + resonid + ',\'' + dateid + '\',event,\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + Slot_Detail[i].ToTimeSlots + '\',\'' + Slot_Detail[i].IsSpecialist + '\',\'' + Slot_Detail[i].SlotMinutes + '\', \'' + Slot_Detail[i].BlockReason + '\',\'' + Slot_Detail[i].ScheduleDate + '\');" id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + ',' + Slot_Detail[i].Patients + ',rdate' + resid + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '">' + appointments + '</td></tr>');
                                                        }
                                                        else {

                                                            table.append('<tr id="' + Slot_Detail[i].Patients + '"><td onclick="Scheduling_MuliView.EditSlot(' + Slot_Detail[i].TimeSlotDtlId + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');" width="63">' + Slot_Detail[i].FromTimeSlots + '</td><td id="' + Slot_Detail[i].TimeSlotDtlId + '"  width="25" align="center">' + Slot_Detail[i].PatientBooked + '</td><td class="resourceappdrop slot" ' + color + ' style="position:relative;"   onclick="Scheduling_MuliView.AppointmentAdd(' + Slot_Detail[i].TimeSlotId + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + provid + ', \'' + provname + '\',' + resid + ',\'' + resname + '\',' + facid + ',\'' + facname + '\',' + resonid + ',\'' + dateid + '\',event,\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + Slot_Detail[i].ToTimeSlots + '\',\'' + Slot_Detail[i].IsSpecialist + '\',\'' + Slot_Detail[i].SlotMinutes + '\', \'' + Slot_Detail[i].BlockReason + '\',\'' + Slot_Detail[i].ScheduleDate + '\');" id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + ',' + Slot_Detail[i].Patients + ',rdate' + resid + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"  >' + appointments + '</td></tr>');
                                                        }
                                                        appointments = '';
                                                        PatientsDtl.push(Slot_Detail[i].PatientDetail);
                                                        AppointmentsDtl.push(Slot_Detail[i].AppDtl);
                                                    }
                                                    $('#facility' + facilityidss + ' #resource' + resourceidss + '').append(table);

                                                    Scheduling_MuliView.MoveResourceAppointment();
                                                    if (PatientsDtl.length == AppointmentsDtl.length) {
                                                        for (var ini = 0; ini < PatientsDtl.length; ini++) {
                                                            Scheduling_MuliView.MultiViewPatientDetails(PatientsDtl[ini], AppointmentsDtl[ini], ProviderName);
                                                        }
                                                        PatientsDtl = [];
                                                        AppointmentsDtl = [];
                                                    }

                                                }
                                            }
                                            else {
                                                var resourcerwidth = 100 / (parseInt(JSONData[mi].ProCount) + parseInt(JSONData[mi].ResCount));
                                                var ResourceName = $("#Resource option[value=" + resourceidss + "]").text();
                                                $('#facility' + facilityidss + '').append('<div class="th-text-center " id="resource' + resourceidss + '" style="width:' + resourcerwidth + '%; float:left; "><div class=" multiple-heading center">Resource:' + ResourceName + '</div></div>');
                                                var table = $('<table  class="table table-responsive table-bordered table-condensed" id="table' + resourceidss + '"><tbody></tbody></table>');
                                                $('#table' + resourceidss + '').children('tbody');
                                                table.append('<tr align="center"><td colspan="3"><div class="col-sm-12">   <div class="col-xs-1 pl-none pr-none"><a id="rdate' + resourceidss + '" providerid="" facilityid="' + facilityidss + '" resourceid="' + resourceidss + '" tableid="rtable' + resourceidss + '" class="btn btn-link btn-xs bdate"><i class="fa fa-angle-left"></i></a></div>    <div class="col-xs-9" id="rdate' + resourceidss + '">   <p ><span>' + criteriaDate + '</span></p>      </div>        <div class="col-xs-1 pl-none pr-none"><a id="rdate' + resourceidss + '" providerid="" facilityid="' + facilityidss + '" resourceid="' + resourceidss + '" tableid="rtable' + resourceidss + '" class="btn btn-link btn-xs fdate"><i class="fa fa-angle-right "></i></a></div> <div class="col-xs-1 pl-none pr-none"><a data-plugin-datepicker=""   id="rcalendar' + resourceidss + facilityidss + '" dateid="rdate' + resourceidss + '" providerid="" facilityid="' + facilityidss + '" resourceid="' + resourceidss + '" class="btn btn-link btn-xs form-control cal"><i class="fa fa-calendar cal"></i></a></div>                                </div> </td> </tr>');
                                                table.append('<tr align="center"><th style="text-align: center;" >No Schedule Found.</th>  </tr>');
                                                $('#facility' + facilityidss + ' #resource' + resourceidss + '').append(table);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                            //record greater then 4
                        else {
                            $('#maincontainer').empty();
                            $('#maincontainer').removeAttr("style");
                            $("#maincontainer").css("width", (200 * parseInt(response1.totalrecord)));
                            $('#maincontainer').width((200 * parseInt(response1.totalrecord)));
                            for (var mi = 0; mi < JSONData.length; mi++) {
                                var facilityidss = JSONData[mi].FacilityId;
                                var providerids = JSONData[mi].Provider.split(',');
                                var resourceids = JSONData[mi].Resource.split(',');


                                var facilitywidth = (200 * (parseInt(JSONData[mi].ProCount) + parseInt(JSONData[mi].ResCount)));

                                var FacilityName = $("#Facility option[value=" + facilityidss + "]").text();
                                $("#maincontainer").append('<div style="width:' + facilitywidth + 'px; float:left; " class="th-text-center " id="facility' + facilityidss + '"><div class="col-sm-12 multiple-heading center">Facility:' + FacilityName + '</div></div>');
                                for (var pid = 0; pid < providerids.length; pid++) {
                                    var provideridss = $.trim(providerids[pid]);
                                    var findid = "Provider" + provideridss + "Facility" + facilityidss;
                                    for (var x = 0; x < SchRecord.length; x++) {
                                        var json = JSON.parse(SchRecord[x]);
                                        var id = json.id;
                                        if (findid == id) {
                                            if (json.status != false) {
                                                var PatientsDtl = [];
                                                var AppointmentsDtl = [];
                                                var Slot_Detail = JSON.parse(json.ProviderScheduleFill_JSON);
                                                var providerwidth = 200;
                                                var ProviderName = $("#Provider option[value=" + provideridss + "]").text();
                                                $('#facility' + facilityidss + '').append('<div class="th-text-center " id="provider' + provideridss + '" style="width:' + providerwidth + 'px; float:left; "><div class=" multiple-heading center">Provider:' + ProviderName + '</div></div>');
                                                if (Slot_Detail.length != 0) {
                                                    var table = $('<table class="table table-responsive table-bordered table-condensed" id="table' + provideridss + '"><tbody></tbody></table>');
                                                    $('#table' + provideridss + '').children('tbody');
                                                    table.append('<tr align="center"><td colspan="3"><div class="col-sm-12">   <div class="col-xs-1 pl-none pr-none"><a id="pdate' + provideridss + '" providerid="' + provideridss + '" facilityid="' + facilityidss + '" resourceid="" tableid="table' + provideridss + '" class="btn btn-link btn-xs bdate"><i class="fa fa-angle-left"></i></a></div>    <div class="col-xs-9" id="pdate' + provideridss + '">   <p ><span>' + criteriaDate + '</span></p>      </div>        <div class="col-xs-1 pl-none pr-none"><a id="pdate' + provideridss + '" providerid="' + provideridss + '" facilityid="' + facilityidss + '" resourceid="" tableid="ptable' + provideridss + '" class="btn btn-link btn-xs fdate"><i class="fa fa-angle-right "></i></a></div> <div class="col-xs-1 pl-none pr-none"><a data-plugin-datepicker=""   id="pcalendar' + provideridss + facilityidss + '" dateid="pdate' + provideridss + '" providerid="' + provideridss + '" facilityid="' + facilityidss + '" resourceid="" class="btn btn-link btn-xs form-control cal"><i class="fa fa-calendar cal"></i></a></div>                                    </div> </td> </tr>');
                                                    var appids = [];
                                                    var slotappcount;
                                                    for (var i = 0; i < Slot_Detail.length; i++) {
                                                        var appointment;
                                                        var margin = Slot_Detail[i].MaxCountApp;

                                                        var provid = Slot_Detail[i].ProviderId;
                                                        if (provid == '' || provid == 'undefined')
                                                            provid = null;
                                                        var refprovid = Slot_Detail[i].RefProviderId;
                                                        if (refprovid == '' || refprovid == 'undefined')
                                                            refprovid = null;

                                                        var provname = Slot_Detail[i].ProviderName;
                                                        if (provname == '' || provname == 'undefined')
                                                            provname = null;

                                                        var resid = Slot_Detail[i].ResourceId;
                                                        if (resid == '' || resid == 'undefined')
                                                            resid = null;
                                                        var resname = Slot_Detail[i].ResourceName;
                                                        if (resname == '' || resname == 'undefined')
                                                            resname = null;
                                                        var facid = Slot_Detail[i].FacilityId;
                                                        if (facid == '' || facid == 'undefined')
                                                            facid = null;
                                                        var facname = Slot_Detail[i].FacilityName;
                                                        if (facname == '' || facname == 'undefined')
                                                            facname = null;
                                                        var resonid = Slot_Detail[i].ScheduleReasonId;
                                                        if (resonid == '' || resonid == 'undefined')
                                                            resonid = null;

                                                        var color = "";

                                                        if (Slot_Detail[i].FacilityColor == "" || Slot_Detail[i].FacilityColor == "undefined" || Slot_Detail[i].FacilityColor == "null" || Slot_Detail[i].FacilityColor == null) {
                                                            color = '';
                                                        } else {
                                                            color = 'bgcolor = \'' + Slot_Detail[i].FacilityColor + '\'';
                                                        }

                                                        var dateid = 'pdate' + provideridss + '';


                                                        var appointments = '<div class="pull-left">&nbsp;</div>';
                                                        if (Slot_Detail[i].AppDtl != "") {
                                                            var app;
                                                            var test = 0;

                                                            var data = Slot_Detail[i].AppDtl;
                                                            var arr = data.split('|');

                                                            for (var j = 0; j < arr.length; j++) {
                                                                slotappcount = (arr.length) - 1;
                                                                var split = arr[j].split(',')
                                                                if (split.length == 22 || split.length == 24) {

                                                                    var appid = split[0];
                                                                    var patientid = split[1];
                                                                    var patientname = split[2];
                                                                    patientname = patientname.replace('-', ', ');
                                                                    var appointmentstatus = split[3];
                                                                    var appcolor = split[4];
                                                                    var appcopay = split[6];
                                                                    var appreason = split[3];
                                                                    var appcount = split[7];
                                                                    var appcomments = split[8];
                                                                    var patientvisitid = split[9];
                                                                    var patvisitstatusid = split[10];
                                                                    var patvisitname = split[11];
                                                                    var noteStatus = split[16];
                                                                    //Edit By Mohsin Nasir Bug # 2922,2931
                                                                    var patEligibility = split[12];
                                                                    var appschreason = split[5];
                                                                    appschreason = appschreason.replace(/#@#/g, ',');
                                                                    //start clinical notes check
                                                                    var noteid = split[13];

                                                                    var noteTitle = "";
                                                                    var isNoteCreated = false;

                                                                    if (noteid == "0") {
                                                                        noteTitle = "Create Note";
                                                                        isNoteCreated = false;
                                                                    } else {
                                                                        noteTitle = "Edit Note";
                                                                        isNoteCreated = true;
                                                                    }
                                                                    //
                                                                    var eligibilityIcon;

                                                                    if (patEligibility == "Active") {
                                                                        eligibilityIcon = '<i class="fa fa-check-circle green"></i>';
                                                                    }
                                                                    else if (patEligibility == "Inactive") {
                                                                        eligibilityIcon = '<i class="fa fa-times-circle red"></i>';
                                                                    }
                                                                    else if (patEligibility.toLowerCase() == "waiting") {
                                                                        eligibilityIcon = '<i class="fa fa-circle black"></i>';
                                                                    }
                                                                    else {
                                                                        eligibilityIcon = '<i class="fa fa-circle blue"></i>';
                                                                    }
                                                                    //END Edit By Mohsin Nasir Bug # 2922,2931

                                                                    appcopay = appcopay.split('-');
                                                                    var copaycolor;
                                                                    //var copaycolor = appcopay[1];
                                                                    // appcopay = appcopay[0];

                                                                    //For last Status
                                                                    var lastStatusId = split[14];
                                                                    var lastStatusName = split[15];
                                                                    var patientType = split[17];
                                                                    var visitType = split[18];
                                                                    var isNonBillable = split[19];
                                                                    var appcopayamt = parseInt(appcopay[0]);
                                                                    var appcopayamtpaid = parseInt(appcopay[1]);

                                                                    if (appcopayamtpaid == 0) {
                                                                        copaycolor = 'Green'; //copay paid
                                                                    }
                                                                    if (appcopayamtpaid > 0) {
                                                                        copaycolor = 'Black';//copay blance
                                                                    }
                                                                    if (appcopayamtpaid == appcopayamt && appcopayamtpaid > 0) {
                                                                        appcopay = appcopayamt;
                                                                        copaycolor = 'Red'; // amount copay
                                                                    }


                                                                    appids.push(appid);

                                                                    for (var w = 0; w < appstatus.length; w++) {
                                                                        if (appstatus[w].Name.toUpperCase() != "- SELECT -" && appstatus[w].Name != appointmentstatus && appstatus[w].Name.toUpperCase() != "CHECK IN" && appstatus[w].Name.toUpperCase() != "CHECK OUT")
                                                                            appstatuses += '<li data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id=' + appstatus[w].Value + ' ><a onclick="Scheduling_MuliView.AppointmentStatusUpdate(' + appid + ',' + appstatus[w].Value + ',\'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');" href="#" >' + appstatus[w].Name + '</a></li> ';
                                                                    }
                                                                    appstatuses = appstatuses.replace('undefined', '');
                                                                    var showapp = 0;

                                                                    for (z = 0; z < appids.length; z++) {
                                                                        if (appids[z] == appid && appid != 0) {
                                                                            showapp = showapp + 1;
                                                                        }

                                                                    }
                                                                    /* Start Code for Reminder Call */
                                                                    var sms = "SMS";
                                                                    var voice = "VOICE";
                                                                    var email = "EMAIL";
                                                                    var Patient_email = "";
                                                                    var Appointmentdatetime = "";

                                                                    try {
                                                                        Patient_email = Slot_Detail[i].PatientDetail.split(',')[10];
                                                                        Appointmentdatetime = $('#pnlScheduleMuliView #searchdate').val() + " " + Slot_Detail[i].FromTimeSlots;
                                                                    } catch (ex) {
                                                                        console.log(ex);
                                                                    }
                                                                    reminderType = '<li id=' + appid + ' ><a onclick="Scheduling_MuliView.SendQuickReminder(' + appid + ',\'' + sms + '\',' + patientid + ',\'' + patientname + '\',\'' + Patient_email + '\',\'' + Appointmentdatetime + '\',\'' + Slot_Detail[i].ProviderId + '\',\'' + Slot_Detail[i].ProviderName + '\');" href="#" >SMS</a></li> <li id=' + appid + ' ><a onclick="Scheduling_MuliView.SendQuickReminder(' + appid + ',\'' + voice + '\',' + patientid + ',\'' + patientname + '\',\'' + Patient_email + '\',\'' + Appointmentdatetime + '\',\'' + Slot_Detail[i].ProviderId + '\',\'' + Slot_Detail[i].ProviderName + '\');" href="#" >Voice</a></li> <li id=' + appid + ' ><a onclick="Scheduling_MuliView.SendQuickReminder(' + appid + ',\'' + email + '\',' + patientid + ',\'' + patientname + '\',\'' + Patient_email + '\',\'' + Appointmentdatetime + '\',\'' + Slot_Detail[i].ProviderId + '\',\'' + Slot_Detail[i].ProviderName + '\');" href="#" >Email</a></li> ';
                                                                    /* End Code for Reminder Call*/
                                                                    if (appcount > '1' && showapp == 1) {

                                                                        if (appointmentstatus.toUpperCase() != 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="providerappdrag apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + '  <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckIn(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\', \'' + dateid + '\');">Reschedule</a></li>   <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li></ul>  </div></div> ';
                                                                        if (appointmentstatus.toUpperCase() == 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="providerappdrag apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + '  <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckIn(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\', \'' + dateid + '\');">Reschedule</a></li>   <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                        //create note
                                                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() != "SIGNED") {

                                                                            var eSuperbillLink = Scheduling_Calendar.getESuperBillLink(patientid, patientvisitid, noteid, "day", patientType, provid, refprovid, facid, appid, billInfoId, billInfoStatus);
                                                                            if (isNonBillable == "1") {
                                                                                eSuperbillLink = '';
                                                                            }
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo"  onclick=" Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ');">Check Out</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CancelPatientCheckIn(' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + appid + ',this);">Cancel CheckIn</a></li> <li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadClinicalNote(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\',\'' + provid + '\',\'' + provname + '\',\'' + facid + '\',\'' + facname + '\',null);">' + noteTitle + '</a></li> <li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#" >Create Charge</a></li><li  id="' + appid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.PrintLetter(' + patientid + ',\'' + patientvisitid + '\',\'' + appid + '\');" >Print Super Bill</a></li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '</ul></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li>  <li  id="' + patientvisitid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.createSuperbill(\'' + patientid.trim() + '\',\'' + patientvisitid.trim() + '\',\'' + noteid.trim() + '\',\'-1\',\'day\',\'' + patientType.trim() + '\',\'' + provid + '\',\'' + refprovid + '\',\'' + facid + '\',\'' + appid + '\');" >Create eSuperbill</a></li> </div></div> ';
                                                                        }
                                                                        //
                                                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() == "SIGNED")
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + patientvisitid + ', \'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',1,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Check Out</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CancelPatientCheckIn(' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + appid + ',this);">Cancel CheckIn</a></li><li  id="' + appid + '"><a href="#" >Create Charge</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PrintLetter();">Print Super Bill</a></li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + '  <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + patientvisitid + ', \'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',1,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li>   </ul>  </div></div> ';
                                                                        if (appointmentstatus.toUpperCase() == 'CANCEL' || appointmentstatus.toUpperCase() == "NO SHOW")
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + '  <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',2,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>   <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                        if (patientvisitid != "" && patvisitname.toUpperCase() != 'CHECK IN' && patvisitname.toUpperCase() != 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + '  <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',2,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                    }

                                                                    if (appcount == '1' && showapp == 1) {

                                                                        if (appointmentstatus.toUpperCase() != 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="providerappdrag apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + ' </div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckIn(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\', \'' + dateid + '\');">Reschedule</a></li>   <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                        if (appointmentstatus.toUpperCase() == 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="providerappdrag apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + ' </div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckIn(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li>     <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\', \'' + dateid + '\');">Reschedule</a></li>       <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                        //create note
                                                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() != "SIGNED") {

                                                                            var eSuperbillLink = Scheduling_Calendar.getESuperBillLink(patientid, patientvisitid, noteid, "day", patientType, provid, refprovid, facid, appid, billInfoId, billInfoStatus);
                                                                            if (isNonBillable == "1") {
                                                                                eSuperbillLink = '';
                                                                            }
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo"  onclick=" Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ');">Check Out</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CancelPatientCheckIn(' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + appid + ',this);">Cancel CheckIn</a></li> <li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadClinicalNote(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\',\'' + provid + '\',\'' + provname + '\',\'' + facid + '\',\'' + facname + '\',null);">' + noteTitle + '</a></li> <li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#" >Create Charge</a></li><li  id="' + appid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.PrintLetter(' + patientid + ',\'' + patientvisitid + '\',\'' + appid + '\');" >Print Super Bill</a></li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '</ul></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> <li  id="' + patientvisitid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.createSuperbill(\'' + patientid.trim() + '\',\'' + patientvisitid.trim() + '\',\'' + noteid.trim() + '\',\'-1\',\'day\',\'' + patientType.trim() + '\',\'' + provid + '\',\'' + refprovid + '\',\'' + facid + '\',\'' + appid + '\');" >Create eSuperbill</a></li>  </div></div> ';
                                                                        }
                                                                        //

                                                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() == "SIGNED")
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + ' </div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + patientvisitid + ', \'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',1,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Check Out</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CancelPatientCheckIn(' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + appid + ',this);">Cancel CheckIn</a></li><li  id="' + appid + '"><a href="#" >Create Charge</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PrintLetter();">Print Super Bill</a></li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + ' </div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + patientvisitid + ', \'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',1,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li>   </ul>  </div></div> ';
                                                                        if (appointmentstatus.toUpperCase() == 'CANCEL' || appointmentstatus.toUpperCase() == "NO SHOW")
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',2,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>    <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li>  </ul>  </div></div> ';
                                                                        if (patientvisitid != "" && patvisitname.toUpperCase() != 'CHECK IN' && patvisitname.toUpperCase() != 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',2,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                    }
                                                                    if (showapp != 1) {
                                                                        // appointments = appointments + '<div  style="width:250px;" </div> ';
                                                                    }
                                                                    appointments = appointments.replace("undefined", "");
                                                                    appstatuses = '';
                                                                    test++;
                                                                }
                                                            }

                                                        }
                                                        var dateid = 'pdate' + provideridss + '';
                                                        if (Slot_Detail[i].AppDtl == "" && Slot_Detail[i].BlockUnblock == "Blocked") {
                                                            table.append('<tr id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + '"><td onclick="Scheduling_MuliView.EditSlot(' + Slot_Detail[i].TimeSlotDtlId + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');" width="63">' + Slot_Detail[i].FromTimeSlots + '</td><td id="' + Slot_Detail[i].TimeSlotDtlId + '"  width="25" align="center">' + Slot_Detail[i].PatientBooked + '</td><td style="color:#fff;" BGCOLOR="#f88379" id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + '" >Blocked:' + Slot_Detail[i].BlockReason + '-' + Slot_Detail[i].Comments + ' </td></tr>');
                                                        }
                                                        else if (appointments == '<div class="pull-left">&nbsp;</div>') {
                                                            table.append('<tr id="' + Slot_Detail[i].Patients + '"><td onclick="Scheduling_MuliView.EditSlot(' + Slot_Detail[i].TimeSlotDtlId + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');" width="63">' + Slot_Detail[i].FromTimeSlots + '</td><td id="' + Slot_Detail[i].TimeSlotDtlId + '"  width="25" align="center">' + Slot_Detail[i].PatientBooked + '</td><td class="providerappdrop slot" ' + color + ' style="position:relative;" onclick="Scheduling_MuliView.AppointmentAdd(' + Slot_Detail[i].TimeSlotId + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + provid + ', \'' + provname + '\',' + resid + ',\'' + resname + '\',' + facid + ',\'' + facname + '\',' + resonid + ',\'' + dateid + '\',event,\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + Slot_Detail[i].ToTimeSlots + '\',\'' + Slot_Detail[i].IsSpecialist + '\',\'' + Slot_Detail[i].SlotMinutes + '\', \'' + Slot_Detail[i].BlockReason + '\',\'' + Slot_Detail[i].ScheduleDate + '\');" id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + ',' + Slot_Detail[i].Patients + ',pdate' + provid + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '">' + appointments + '</td></tr>');
                                                        }
                                                        else {

                                                            table.append('<tr id="' + Slot_Detail[i].Patients + '"><td onclick="Scheduling_MuliView.EditSlot(' + Slot_Detail[i].TimeSlotDtlId + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');" width="63">' + Slot_Detail[i].FromTimeSlots + '</td><td id="' + Slot_Detail[i].TimeSlotDtlId + '"  width="25" align="center">' + Slot_Detail[i].PatientBooked + '</td><td class="providerappdrop slot" ' + color + ' style="position:relative;"   onclick="Scheduling_MuliView.AppointmentAdd(' + Slot_Detail[i].TimeSlotId + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + provid + ', \'' + provname + '\',' + resid + ',\'' + resname + '\',' + facid + ',\'' + facname + '\',' + resonid + ',\'' + dateid + '\',event,\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + Slot_Detail[i].ToTimeSlots + '\',\'' + Slot_Detail[i].IsSpecialist + '\',\'' + Slot_Detail[i].SlotMinutes + '\', \'' + Slot_Detail[i].BlockReason + '\',\'' + Slot_Detail[i].ScheduleDate + '\');" id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + ',' + Slot_Detail[i].Patients + ',pdate' + provid + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"  >' + appointments + '</td></tr>');
                                                        }
                                                        appointments = '';
                                                        PatientsDtl.push(Slot_Detail[i].PatientDetail);
                                                        AppointmentsDtl.push(Slot_Detail[i].AppDtl);
                                                    }
                                                    $('#facility' + facilityidss + ' #provider' + provideridss + '').append(table);
                                                    Scheduling_MuliView.MoveProviderAppointment();
                                                    Scheduling_MuliView.dropDownMenuClick(Scheduling_MuliView.params.PanelID);
                                                    if (PatientsDtl.length == AppointmentsDtl.length) {
                                                        for (var ini = 0; ini < PatientsDtl.length; ini++) {
                                                            Scheduling_MuliView.MultiViewPatientDetails(PatientsDtl[ini], AppointmentsDtl[ini], ProviderName);
                                                        }
                                                        PatientsDtl = [];
                                                        AppointmentsDtl = [];
                                                    }
                                                }
                                            }
                                            else {
                                                var providerwidth = 200;
                                                var ProviderName = $("#Provider option[value=" + provideridss + "]").text();
                                                $('#facility' + facilityidss + '').append('<div class="th-text-center " id="provider' + provideridss + '" style="width:' + providerwidth + 'px; float:left; "><div class=" multiple-heading center">Provider:' + ProviderName + '</div></div>');
                                                var table = $('<table class="table table-responsive table-bordered table-condensed" id="table' + provideridss + '"><tbody></tbody></table>');
                                                $('#table' + provideridss + '').children('tbody');
                                                table.append('<tr align="center"><td colspan="3"><div class="col-sm-12">   <div class="col-xs-1 pl-none pr-none"><a id="pdate' + provideridss + '" providerid="' + provideridss + '" facilityid="' + facilityidss + '" resourceid="" tableid="table' + provideridss + '" class="btn btn-link btn-xs bdate"><i class="fa fa-angle-left"></i></a></div>    <div class="col-xs-9" id="pdate' + provideridss + '">   <p ><span>' + criteriaDate + '</span></p>      </div>        <div class="col-xs-1 pl-none pr-none"><a id="pdate' + provideridss + '" providerid="' + provideridss + '" facilityid="' + facilityidss + '" resourceid="" tableid="ptable' + provideridss + '" class="btn btn-link btn-xs fdate"><i class="fa fa-angle-right "></i></a></div> <div class="col-xs-1 pl-none pr-none"><a data-plugin-datepicker=""   id="pcalendar' + provideridss + facilityidss + '" dateid="pdate' + provideridss + '" providerid="' + provideridss + '" facilityid="' + facilityidss + '" resourceid="" class="btn btn-link btn-xs form-control cal"><i class="fa fa-calendar cal"></i></a></div>                                    </div> </td> </tr>');
                                                table.append('<tr align="center"><th style="text-align: center;" >No Schedule Found.</th>  </tr>');
                                                $('#facility' + facilityidss + ' #provider' + provideridss + '').append(table);

                                            }
                                        }
                                    }
                                }
                                for (var rid = 0; rid < resourceids.length; rid++) {
                                    var resourceidss = $.trim(resourceids[rid]);
                                    var findid = "Resource" + resourceidss + "Facility" + facilityidss;
                                    for (var x = 0; x < SchRecord.length; x++) {
                                        var json = JSON.parse(SchRecord[x]);
                                        var id = json.id;
                                        if (findid == id) {
                                            if (json.status != false) {
                                                var PatientsDtl = [];
                                                var AppointmentsDtl = [];
                                                var Slot_Detail = JSON.parse(json.ProviderScheduleFill_JSON);
                                                var resourcerwidth = 200;
                                                var ResourceName = $("#Resource option[value=" + resourceidss + "]").text();
                                                $('#facility' + facilityidss + '').append('<div class="th-text-center " id="resource' + resourceidss + '" style="width:' + resourcerwidth + 'px; float:left; "><div class=" multiple-heading center">Resource:' + ResourceName + '</div></div>');
                                                if (Slot_Detail.length != 0) {
                                                    var table = $('<table  class="table table-responsive table-bordered table-condensed" id="table' + resourceidss + '"><tbody></tbody></table>');
                                                    $('#table' + resourceidss + '').children('tbody');
                                                    table.append('<tr align="center"><td colspan="3"><div class="col-sm-12">   <div class="col-xs-1 pl-none pr-none"><a id="rdate' + resourceidss + '" providerid="" facilityid="' + facilityidss + '" resourceid="' + resourceidss + '" tableid="rtable' + resourceidss + '" class="btn btn-link btn-xs bdate"><i class="fa fa-angle-left"></i></a></div>    <div class="col-xs-9" id="rdate' + resourceidss + '">   <p ><span>' + criteriaDate + '</span></p>      </div>        <div class="col-xs-1 pl-none pr-none"><a id="rdate' + resourceidss + '" providerid="" facilityid="' + facilityidss + '" resourceid="' + resourceidss + '" tableid="rtable' + resourceidss + '" class="btn btn-link btn-xs fdate"><i class="fa fa-angle-right "></i></a></div> <div class="col-xs-1 pl-none pr-none"><a data-plugin-datepicker=""   id="rcalendar' + resourceidss + facilityidss + '" dateid="rdate' + resourceidss + '" providerid="" facilityid="' + facilityidss + '" resourceid="' + resourceidss + '" class="btn btn-link btn-xs form-control cal"><i class="fa fa-calendar cal"></i></a></div>                                </div> </td> </tr>');
                                                    var appids = [];
                                                    var slotappcount;
                                                    for (var i = 0; i < Slot_Detail.length; i++) {
                                                        var appointment;
                                                        var margin = Slot_Detail[i].MaxCountApp;

                                                        var provid = Slot_Detail[i].ProviderId;
                                                        if (provid == '' || provid == 'undefined')
                                                            provid = null;

                                                        var refprovid = Slot_Detail[i].RefProviderId;
                                                        if (refprovid == '' || refprovid == 'undefined')
                                                            refprovid = null;

                                                        var provname = Slot_Detail[i].ProviderName;
                                                        if (provname == '' || provname == 'undefined')
                                                            provname = null;

                                                        var resid = Slot_Detail[i].ResourceId;
                                                        if (resid == '' || resid == 'undefined')
                                                            resid = null;
                                                        var resname = Slot_Detail[i].ResourceName;
                                                        if (resname == '' || resname == 'undefined')
                                                            resname = null;
                                                        var facid = Slot_Detail[i].FacilityId;
                                                        if (facid == '' || facid == 'undefined')
                                                            facid = null;
                                                        var facname = Slot_Detail[i].FacilityName;
                                                        if (facname == '' || facname == 'undefined')
                                                            facname = null;
                                                        var resonid = Slot_Detail[i].ScheduleReasonId;
                                                        if (resonid == '' || resonid == 'undefined')
                                                            resonid = null;

                                                        var color = "";

                                                        if (Slot_Detail[i].FacilityColor == "" || Slot_Detail[i].FacilityColor == "undefined" || Slot_Detail[i].FacilityColor == "null" || Slot_Detail[i].FacilityColor == null) {
                                                            color = '';
                                                        } else {
                                                            color = 'bgcolor = \'' + Slot_Detail[i].FacilityColor + '\'';
                                                        }
                                                        var dateid = 'rdate' + resourceidss + '';

                                                        var appointments = '<div class="pull-left">&nbsp;</div>';
                                                        if (Slot_Detail[i].AppDtl != "") {
                                                            var app;
                                                            var test = 0;

                                                            var data = Slot_Detail[i].AppDtl;
                                                            var arr = data.split('|');

                                                            for (var j = 0; j < arr.length; j++) {
                                                                slotappcount = (arr.length) - 1;
                                                                var split = arr[j].split(',')
                                                                if (split.length == 22 || split.length == 24) {

                                                                    var appid = split[0];
                                                                    var patientid = split[1];
                                                                    var patientname = split[2];
                                                                    patientname = patientname.replace('-', ', ');
                                                                    var appointmentstatus = split[3];
                                                                    var appcolor = split[4];
                                                                    var appcopay = split[6];
                                                                    var appreason = split[3];
                                                                    var appcount = split[7];
                                                                    var appcomments = split[8];
                                                                    var patientvisitid = split[9];
                                                                    var patvisitstatusid = split[10];
                                                                    var patvisitname = split[11];
                                                                    var noteid = split[13];
                                                                    var noteStatus = split[16];
                                                                    var appschreason = split[5];
                                                                    appschreason = appschreason.replace(/#@#/g, ',');
                                                                    // create note
                                                                    var noteTitle = "";
                                                                    var isNoteCreated = false;

                                                                    if (noteid == "0") {
                                                                        noteTitle = "Create Note";
                                                                        isNoteCreated = false;
                                                                    } else {
                                                                        noteTitle = "Edit Note";
                                                                        isNoteCreated = true;
                                                                    }
                                                                    //
                                                                    //Edit By Mohsin Nasir Bug # 2922,2931
                                                                    var patEligibility = split[12];
                                                                    var eligibilityIcon;

                                                                    if (patEligibility == "Active") {
                                                                        eligibilityIcon = '<i class="fa fa-check-circle green"></i>';
                                                                    }
                                                                    else if (patEligibility == "Inactive") {
                                                                        eligibilityIcon = '<i class="fa fa-times-circle red"></i>';
                                                                    }
                                                                    else if (patEligibility.toLowerCase() == "waiting") {
                                                                        eligibilityIcon = '<i class="fa fa-circle black"></i>';
                                                                    }
                                                                    else {
                                                                        eligibilityIcon = '<i class="fa fa-circle blue"></i>';
                                                                    }
                                                                    //END Edit By Mohsin Nasir Bug # 2922,2931

                                                                    appcopay = appcopay.split('-');
                                                                    var copaycolor;
                                                                    //var copaycolor = appcopay[1];
                                                                    // appcopay = appcopay[0];

                                                                    //For last Status
                                                                    var lastStatusId = split[14];
                                                                    var lastStatusName = split[15];
                                                                    var patientType = split[17];
                                                                    var visitType = split[18];
                                                                    var isNonBillable = split[19];
                                                                    var appcopayamt = parseInt(appcopay[0]);
                                                                    var appcopayamtpaid = parseInt(appcopay[1]);

                                                                    if (appcopayamtpaid == 0) {
                                                                        copaycolor = 'Green'; //copay paid
                                                                    }
                                                                    if (appcopayamtpaid > 0) {
                                                                        copaycolor = 'Black';//copay blance
                                                                    }
                                                                    if (appcopayamtpaid == appcopayamt && appcopayamtpaid > 0) {
                                                                        appcopay = appcopayamt;
                                                                        copaycolor = 'Red'; // amount copay
                                                                    }


                                                                    appids.push(appid);

                                                                    for (var w = 0; w < appstatus.length; w++) {
                                                                        if (appstatus[w].Name.toUpperCase() != "- SELECT -" && appstatus[w].Name != appointmentstatus && appstatus[w].Name.toUpperCase() != "CHECK IN" && appstatus[w].Name.toUpperCase() != "CHECK OUT")
                                                                            appstatuses += '<li data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id=' + appstatus[w].Value + ' ><a onclick="Scheduling_MuliView.AppointmentStatusUpdate(' + appid + ',' + appstatus[w].Value + ',\'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');" href="#" >' + appstatus[w].Name + '</a></li> ';
                                                                    }
                                                                    appstatuses = appstatuses.replace('undefined', '');
                                                                    var showapp = 0;

                                                                    for (z = 0; z < appids.length; z++) {
                                                                        if (appids[z] == appid && appid != 0) {
                                                                            showapp = showapp + 1;
                                                                        }

                                                                    }
                                                                    /* Start Code for Reminder Call */
                                                                    var sms = "SMS";
                                                                    var voice = "VOICE";
                                                                    var email = "EMAIL";
                                                                    var Patient_email = "";
                                                                    var Appointmentdatetime = "";

                                                                    try {
                                                                        Patient_email = Slot_Detail[i].PatientDetail.split(',')[10];
                                                                        Appointmentdatetime = $('#pnlScheduleMuliView #searchdate').val() + " " + Slot_Detail[i].FromTimeSlots;
                                                                    } catch (ex) {
                                                                        console.log(ex);
                                                                    }
                                                                    reminderType = '<li id=' + appid + ' ><a onclick="Scheduling_MuliView.SendQuickReminder(' + appid + ',\'' + sms + '\',' + patientid + ',\'' + patientname + '\',\'' + Patient_email + '\',\'' + Appointmentdatetime + '\',\'' + Slot_Detail[i].ProviderId + '\',\'' + Slot_Detail[i].ProviderName + '\');" href="#" >SMS</a></li> <li id=' + appid + ' ><a onclick="Scheduling_MuliView.SendQuickReminder(' + appid + ',\'' + voice + '\',' + patientid + ',\'' + patientname + '\',\'' + Patient_email + '\',\'' + Appointmentdatetime + '\',\'' + Slot_Detail[i].ProviderId + '\',\'' + Slot_Detail[i].ProviderName + '\');" href="#" >Voice</a></li> <li id=' + appid + ' ><a onclick="Scheduling_MuliView.SendQuickReminder(' + appid + ',\'' + email + '\',' + patientid + ',\'' + patientname + '\',\'' + Patient_email + '\',\'' + Appointmentdatetime + '\',\'' + Slot_Detail[i].ProviderId + '\',\'' + Slot_Detail[i].ProviderName + '\');" href="#" >Email</a></li> ';
                                                                    /* End Code for Reminder Call*/
                                                                    if (appcount > '1' && showapp == 1) {

                                                                        if (appointmentstatus.toUpperCase() != 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="resourceappdrag apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + '   <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckIn(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\', \'' + dateid + '\');">Reschedule</a></li>   <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li></ul>  </div></div> ';
                                                                        if (appointmentstatus.toUpperCase() == 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="resourceappdrag apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + '   <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckIn(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check In</a></li>      <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                        //create note
                                                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() != "SIGNED") {

                                                                            var eSuperbillLink = Scheduling_Calendar.getESuperBillLink(patientid, patientvisitid, noteid, "day", patientType, provid, refprovid, facid, appid, billInfoId, billInfoStatus);
                                                                            if (isNonBillable == "1") {
                                                                                eSuperbillLink = '';
                                                                            }
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo"  onclick=" Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ');">Check Out</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CancelPatientCheckIn(' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + appid + ',this);">Cancel CheckIn</a></li> <li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadClinicalNote(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\',\'' + provid + '\',\'' + provname + '\',\'' + facid + '\',\'' + facname + '\',null,' + resid + ');">' + noteTitle + '</a></li> <li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#" >Create Charge</a></li><li  id="' + appid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.PrintLetter(' + patientid + ',\'' + patientvisitid + '\',\'' + appid + '\');" >Print Super Bill</a></li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '</ul></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li>  <li  id="' + patientvisitid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.createSuperbill(\'' + patientid.trim() + '\',\'' + patientvisitid.trim() + '\',\'' + noteid.trim() + '\',\'-1\',\'day\',\'' + patientType.trim() + '\',\'' + provid + '\',\'' + refprovid + '\',\'' + facid + '\',\'' + appid + '\');" >Create eSuperbill</a></li> </div></div> ';
                                                                        }
                                                                        //
                                                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() == "SIGNED")
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + '  <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + patientvisitid + ', \'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',1,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Check Out</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CancelPatientCheckIn(' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + appid + ',this);">Cancel CheckIn</a></li><li  id="' + appid + '"><a href="#" >Create Charge</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PrintLetter();">Print Super Bill</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + '   <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + patientvisitid + ', \'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',1,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li>  </ul>  </div></div> ';
                                                                        if (appointmentstatus.toUpperCase() == 'CANCEL' || appointmentstatus.toUpperCase() == "NO SHOW")
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + '   <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',2,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>   <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li>  </ul>  </div></div> ';
                                                                        if (patientvisitid != "" && patvisitname.toUpperCase() != 'CHECK IN' && patvisitname.toUpperCase() != 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + '  <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',2,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                    }

                                                                    if (appcount == '1' && showapp == 1) {

                                                                        if (appointmentstatus.toUpperCase() != 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="resourceappdrag apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + '   <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + ' </div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>      <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckIn(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\', \'' + dateid + '\');">Reschedule</a></li>      <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                        if (appointmentstatus.toUpperCase() == 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="resourceappdrag apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + '   <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + ' </div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckIn(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check In</a></li>            <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                        // create note
                                                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() != "SIGNED") {

                                                                            var eSuperbillLink = Scheduling_Calendar.getESuperBillLink(patientid, patientvisitid, noteid, "day", patientType, provid, refprovid, facid, appid, billInfoId, billInfoStatus);
                                                                            if (isNonBillable == "1") {
                                                                                eSuperbillLink = '';
                                                                            }
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo"  onclick=" Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ');">Check Out</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CancelPatientCheckIn(' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + appid + ',this);">Cancel CheckIn</a></li> <li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadClinicalNote(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\',\'' + provid + '\',\'' + provname + '\',\'' + facid + '\',\'' + facname + '\',null,' + resid + ');">' + noteTitle + '</a></li> <li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#" >Create Charge</a></li><li  id="' + appid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.PrintLetter(' + patientid + ',\'' + patientvisitid + '\',\'' + appid + '\');" >Print Super Bill</a></li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '</ul></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li>  <li  id="' + patientvisitid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.createSuperbill(\'' + patientid.trim() + '\',\'' + patientvisitid.trim() + '\',\'' + noteid.trim() + '\',\'-1\',\'day\',\'' + patientType.trim() + '\',\'' + provid + '\',\'' + refprovid + '\',\'' + facid + '\',\'' + appid + '\');" >Create eSuperbill</a></li> </div></div> ';
                                                                        }
                                                                        //
                                                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() == "SIGNED")
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + '   <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + ' </div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + patientvisitid + ', \'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',1,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>     <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Check Out</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CancelPatientCheckIn(' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + appid + ',this);">Cancel CheckIn</a></li><li  id="' + appid + '"><a href="#" >Create Charge</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PrintLetter();">Print Super Bill</a></li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + '  <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + ' </div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + patientvisitid + ', \'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',1,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li>  </ul>  </div></div> ';
                                                                        if (appointmentstatus.toUpperCase() == 'CANCEL' || appointmentstatus.toUpperCase() == "NO SHOW")
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + '   <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',2,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>   <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                        if (patientvisitid != "" && patvisitname.toUpperCase() != 'CHECK IN' && patvisitname.toUpperCase() != 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL')
                                                                            appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + '  <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',2,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                                    }
                                                                    if (showapp != 1) {
                                                                        // appointments = appointments + '<div  style="width:250px;" </div> ';
                                                                    }
                                                                    appointments = appointments.replace("undefined", "");
                                                                    appstatuses = '';
                                                                    test++;
                                                                }
                                                            }

                                                        }
                                                        var dateid = 'rdate' + resourceidss + '';
                                                        if (Slot_Detail[i].AppDtl == "" && Slot_Detail[i].BlockUnblock == "Blocked") {
                                                            table.append('<tr id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + '"><td onclick="Scheduling_MuliView.EditSlot(' + Slot_Detail[i].TimeSlotDtlId + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');" width="63">' + Slot_Detail[i].FromTimeSlots + '</td><td id="' + Slot_Detail[i].TimeSlotDtlId + '"  width="25" align="center">' + Slot_Detail[i].PatientBooked + '</td><td style="color:#fff;" BGCOLOR="#f88379" id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + '" >Blocked:' + Slot_Detail[i].BlockReason + '-' + Slot_Detail[i].Comments + ' </td></tr>');
                                                        }
                                                        else if (appointments == '<div class="pull-left">&nbsp;</div>') {
                                                            table.append('<tr id="' + Slot_Detail[i].Patients + '"><td onclick="Scheduling_MuliView.EditSlot(' + Slot_Detail[i].TimeSlotDtlId + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');" width="63">' + Slot_Detail[i].FromTimeSlots + '</td><td id="' + Slot_Detail[i].TimeSlotDtlId + '"  width="25" align="center">' + Slot_Detail[i].PatientBooked + '</td><td class="resourceappdrop slot" ' + color + ' style="position:relative;" onclick="Scheduling_MuliView.AppointmentAdd(' + Slot_Detail[i].TimeSlotId + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + provid + ', \'' + provname + '\',' + resid + ',\'' + resname + '\',' + facid + ',\'' + facname + '\',' + resonid + ',\'' + dateid + '\',event,\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + Slot_Detail[i].ToTimeSlots + '\',\'' + Slot_Detail[i].IsSpecialist + '\',\'' + Slot_Detail[i].SlotMinutes + '\', \'' + Slot_Detail[i].BlockReason + '\',\'' + Slot_Detail[i].ScheduleDate + '\');" id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + ',' + Slot_Detail[i].Patients + ',rdate' + resid + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '">' + appointments + '</td></tr>');
                                                        }
                                                        else {

                                                            table.append('<tr id="' + Slot_Detail[i].Patients + '"><td onclick="Scheduling_MuliView.EditSlot(' + Slot_Detail[i].TimeSlotDtlId + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');" width="63">' + Slot_Detail[i].FromTimeSlots + '</td><td id="' + Slot_Detail[i].TimeSlotDtlId + '"  width="25" align="center">' + Slot_Detail[i].PatientBooked + '</td><td class="resourceappdrop slot" ' + color + ' style="position:relative;"   onclick="Scheduling_MuliView.AppointmentAdd(' + Slot_Detail[i].TimeSlotId + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + provid + ', \'' + provname + '\',' + resid + ',\'' + resname + '\',' + facid + ',\'' + facname + '\',' + resonid + ',\'' + dateid + '\',event,\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + Slot_Detail[i].ToTimeSlots + '\',\'' + Slot_Detail[i].IsSpecialist + '\',\'' + Slot_Detail[i].SlotMinutes + '\', \'' + Slot_Detail[i].BlockReason + '\',\'' + Slot_Detail[i].ScheduleDate + '\');" id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + ',' + Slot_Detail[i].Patients + ',rdate' + resid + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"  >' + appointments + '</td></tr>');
                                                        }
                                                        appointments = '';
                                                        PatientsDtl.push(Slot_Detail[i].PatientDetail);
                                                        AppointmentsDtl.push(Slot_Detail[i].AppDtl);

                                                    }
                                                    $('#facility' + facilityidss + ' #resource' + resourceidss + '').append(table);

                                                    Scheduling_MuliView.MoveResourceAppointment();
                                                    if (PatientsDtl.length == AppointmentsDtl.length) {
                                                        for (var ini = 0; ini < PatientsDtl.length; ini++) {
                                                            Scheduling_MuliView.MultiViewPatientDetails(PatientsDtl[ini], AppointmentsDtl[ini], ProviderName);
                                                        }
                                                        PatientsDtl = [];
                                                        AppointmentsDtl = [];
                                                    }
                                                }
                                            }
                                            else {
                                                var resourcerwidth = 200;
                                                var ResourceName = $("#Resource option[value=" + resourceidss + "]").text();
                                                $('#facility' + facilityidss + '').append('<div class="th-text-center " id="resource' + resourceidss + '" style="width:' + resourcerwidth + 'px; float:left; "><div class=" multiple-heading center">Resource:' + ResourceName + '</div></div>');
                                                var table = $('<table  class="table table-responsive table-bordered table-condensed" id="table' + resourceidss + '"><tbody></tbody></table>');
                                                $('#table' + resourceidss + '').children('tbody');
                                                table.append('<tr align="center"><td colspan="3"><div class="col-sm-12">   <div class="col-xs-1 pl-none pr-none"><a id="rdate' + resourceidss + '" providerid="" facilityid="' + facilityidss + '" resourceid="' + resourceidss + '" tableid="rtable' + resourceidss + '" class="btn btn-link btn-xs bdate"><i class="fa fa-angle-left"></i></a></div>    <div class="col-xs-9" id="rdate' + resourceidss + '">   <p ><span>' + criteriaDate + '</span></p>      </div>        <div class="col-xs-1 pl-none pr-none"><a id="rdate' + resourceidss + '" providerid="" facilityid="' + facilityidss + '" resourceid="' + resourceidss + '" tableid="rtable' + resourceidss + '" class="btn btn-link btn-xs fdate"><i class="fa fa-angle-right "></i></a></div> <div class="col-xs-1 pl-none pr-none"><a data-plugin-datepicker=""   id="rcalendar' + resourceidss + facilityidss + '" dateid="rdate' + resourceidss + '" providerid="" facilityid="' + facilityidss + '" resourceid="' + resourceidss + '" class="btn btn-link btn-xs form-control cal"><i class="fa fa-calendar cal"></i></a></div>                                </div> </td> </tr>');
                                                table.append('<tr align="center"><th style="text-align: center;" >No Schedule Found.</th>  </tr>');
                                                $('#facility' + facilityidss + ' #resource' + resourceidss + '').append(table);
                                            }
                                        }
                                    }

                                }
                            }

                        }





                    }

                    //Initialization of Data Picker
                    Scheduling_MuliView.InitializationDatePicker();
                    //Scheduling_Calendar.Initializationtooltip();

                    Scheduling_Calendar.dropDownMenuClick(Scheduling_MuliView.params.PanelID);
                    Scheduling_Calendar.subDropdownMenuMouseEnter();
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    InitializationDatePicker: function () {

        $('.cal').each(function (i, obj) {

            $(obj).attr('id');
            $('#' + $(obj).attr('id')).datepicker({
                format: "mm/dd/yyyy",
                todayHighlight: true
            }).on('changeDate', function (e) {
                var providerid = $(this).attr('providerid');
                var facilityid = $(this).attr('facilityid');
                var resourceid = $(this).attr('resourceid');
                var clickid = $(this).attr('dateid');
                var inputDate = $.datepicker.formatDate('mm/dd/yy', new Date($('#' + $(this).attr('id')).datepicker('getDate')));
                $('#facility' + $.trim(facilityid) + ' #' + $.trim($(this).attr('dateid')) + ' span').html(inputDate);

                Scheduling_MuliView.BackDate(clickid, providerid, resourceid, facilityid, 0);

                $(this).datepicker('hide');

            });

        });
    },

    EditSlot: function (TimeslotDetailid, DateId, ProviderId, ResourceId, FacilityId) {

        // var criteria = $('#pnlScheduleCalendar #daydate').text().trim();

        // alert(DatId);
        var params = [];
        params["ParentCtrl"] = "schTabMultipleView";
        params["TimeslotDetailid"] = TimeslotDetailid;
        params["DateId"] = DateId;
        params["ProviderId"] = ProviderId;
        params["ResourceId"] = ResourceId;
        params["FacilityId"] = FacilityId;
        params["MultipleView"] = 1;
        LoadActionPan('schEditSlot', params);
    },

    AppointmentAdd: function (Slotid, TimeSlotDtlId, ProviderId, ProviderName, ResourceId, ResourceName, FacilityId, FacilityName, ScheduleReasonId, DateId, event, Time, ToTime, IsSpecialist, SlotMinutes, ScheduleReason, ScheduleDate) {
        var senderElement = event.target.tagName;
        //alert(DateId);
        if (senderElement == 'TD') {
            var strMessage = "";
            AppPrivileges.GetFormPrivileges("Appointment", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    var params = [];
                    params["SlotId"] = Slotid;
                    params["SlotdetailId"] = TimeSlotDtlId;

                    params["ProviderId"] = ProviderId;
                    params["ProviderName"] = ProviderName;
                    if (ResourceId != null) {
                        params["ProviderId"] = null;
                        params["ProviderName"] = null;
                    }
                    params["ResourceId"] = ResourceId;
                    params["ResourceName"] = ResourceName;
                    params["FacilityId"] = FacilityId;
                    params["FacilityName"] = FacilityName;
                    params["ScheduleReasonId"] = ScheduleReasonId;
                    params["DateId"] = DateId;
                    params["DayDate"] = $('#daydate').text().trim();

                    params["Time"] = Time;
                    params["ToTime"] = ToTime;
                    params["SlotMinutes"] = SlotMinutes;
                    params["IsSpecialist"] = IsSpecialist;
                    params["ScheduleReason"] = ScheduleReason;
                    params["ScheduleDate"] = ScheduleDate;
                    params["mode"] = "Add";
                    params["ParentCtrl"] = "schTabMultipleView";
                    LoadActionPan('appointmentDetail', params);
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
    },

    MoveProviderAppointment: function () {
        //var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Appointment", "UPDATE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        $('a[data-toggle="tooltip"]').tooltip({
            animated: 'fade',
            placement: 'bottom',
            container: 'body',
            html: true,

        });
        $(".providerappdrag").draggable({
            revert: true,
            appendTo: 'body',
            stack: '.providerappdrop',

            start: function () {
                $('#myTable').find(".open").removeClass("open");
                $(this).parent().find('.appdrag').css("z-index", '');
                $(this).parent().css("z-index", Scheduling_Calendar.zIndexDraggable);
                $(this).css("z-index", '1018');
                $('#tabs').css('z-index', '9999');
                $('.tooltip').remove();
            },
            stop: function () {
                $(this).css("z-index", "");
                $('#tabs').css('z-index', '0');
                $('.tooltip').remove();
            }
        });


        $(".providerappdrop").droppable({
            accept: ".providerappdrag",
            activeClass: 'droppable-active',
            hoverClass: 'droppable-hover',
            tolerance: 'pointer',
            drop: function (ev, ui) {
                $(this).css("z-index", "");
                $('.zIndex').parent().css("z-index", "");
                //alert(ui.draggable.attr('id'));
                //alert($(this).attr('id'));
                var test = $(this);
                var exist = 0;
                var a = $(this).attr('id');
                var b = ui.draggable.attr('id');
                var arr = a.split(',');
                var arr1 = b.split(',');
                var dragslotdetailid = arr1[1];
                CutAppointmentId = arr1[0];
                var apppatientid = arr1[2];
                var appcount = arr1[3];
                slotid = arr[0].replace('Slotid', '');
                slotdetailid = arr[1].replace('TimeSlotDtlId', '');
                var dateid = arr[arr.length - 4];
                var providerid = arr[arr.length - 3].replace('providerid', '');
                var facilityid = arr[arr.length - 2].replace('facilityid', '');

                var providerid1 = arr1[4].replace('providerid', '');
                var facilityid1 = arr1[5].replace('facilityid', '');

                var droptime = arr[arr.length - 1];
                var dragtime = arr1[arr1.length - 1];
                var againexist = 0;
                //var genericMessage = false;
                //var currentTr = "";
                if ((facilityid + providerid) == (facilityid1 + providerid1)) {
                    for (var x1 = 2; x1 <= arr.length; x1++) {
                        if ($.trim(arr[x1]) == $.trim(apppatientid) && $.trim(dragslotdetailid) == $.trim(slotdetailid)) {

                            exist = 1;
                        }

                    }
                    //for (var ini = 0; ini < appcount - 1; ini++) {
                    //    var nextTr = $(this).closest("tr").next("tr");

                    //    if (currentTr == "") currentTr = nextTr;
                    //    var nextTrId = currentTr.attr("id").split(',');
                    //    if (!(nextTrId == "")) {
                    //        for (var init = 0; init < nextTrId.length; init++) {
                    //            if (apppatientid == nextTrId[init]) {
                    //                exist = 1;
                    //                genericMessage = true;
                    //            }
                    //        }
                    //    }
                    //    currentTr = currentTr.closest("tr").next("tr");
                    //}
                    if (appcount > 1) {
                        if (dragslotdetailid < slotdetailid) {
                            for (var dr = 0; dr < appcount; dr++) {
                                var sameappmoveid = parseInt(dragslotdetailid) + parseInt(dr);
                                if (slotdetailid == sameappmoveid) {
                                    againexist = 1;
                                }
                            }
                            //if ($(this).parent().attr('id') != undefined && againexist == 0) {

                            //    for (var l = 1; l <= appcount - 1; l++) {

                            //        if (l == 1)
                            //            var nextrow = $(this).parent().attr('id').split(',');
                            //        else
                            //            if ($(this).parent().next().attr('id') != undefined)
                            //                var nextrow = $(this).parent().next().attr('id').split(',');
                            //        //var nextrow = $(this).parent().next().attr('id').split(',');
                            //        for (var x1 = 0; x1 <= nextrow.length; x1++) {
                            //            if ($.trim(nextrow[x1]) == $.trim(apppatientid)) {

                            //                exist = 1;
                            //            }

                            //        }
                            //    }
                            //}
                        }
                        if (dragslotdetailid > slotdetailid) {
                            if ($(this).parent().attr('id') != undefined) {
                                for (var l = 1; l <= appcount - 1; l++) {

                                    if (l == 1)
                                        var nextrow = $(this).parent().attr('id').split(',');
                                    else
                                        if ($(this).parent().prev().attr('id') != undefined)
                                            var nextrow = $(this).parent().prev().attr('id').split(',');
                                    //var nextrow = $(this).parent().next().attr('id').split(',');
                                    for (var x1 = 0; x1 <= nextrow.length; x1++) {
                                        if ($.trim(nextrow[x1]) == $.trim(apppatientid)) {

                                            exist = 1;
                                        }

                                    }
                                }
                            }
                        }
                    }

                    params["TimeSlotDtlId"] = slotdetailid;
                    params["CutAppointmentId"] = CutAppointmentId;

                    var drag_element = ui.draggable[0];

                    if ($.trim(dragslotdetailid) != $.trim(slotdetailid) && exist != 1) {


                        utility.myConfirm('Are you sure want to move appointment from ' + dragtime + ' to ' + droptime + '', function () {

                            Scheduling_Calendar.MoveAppointment(CutAppointmentId, slotdetailid).done(function (response) {
                                if (response.status != false) {
                                    utility.DisplayMessages(response.Message, 1);
                                    ui.draggable.detach().appendTo(test);

                                    Scheduling_MuliView.BackDate(dateid, providerid, "null", facilityid, 0);


                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });

                        }, function () { },
         'Move Appointment'
     );
                    }

                    else if ($.trim(dragslotdetailid) != $.trim(slotdetailid) && exist == 1) {
                        //if (genericMessage) {
                        //    utility.DisplayMessages('Patient is already booked in current time range.', 3);
                        //} else {
                        utility.DisplayMessages('Patient is already booked on ' + droptime + ' slot.', 3);
                        // }
                    }
                    //Scheduling_MuliView.LoadMultipleViewCalendar();
                }
            }
        });

        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
    },

    MoveResourceAppointment: function () {
        //var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Appointment", "UPDATE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        $('a[data-toggle="tooltip"]').tooltip({
            animated: 'fade',
            placement: 'bottom',
            container: 'body',
            html: true,

        });
        $(".resourceappdrag").draggable({
            revert: true,
            appendTo: 'body',
            stack: '.resourceappdrop',

            start: function () {
                $('#myTable').find(".open").removeClass("open");
                $(this).parent().find('.appdrag').css("z-index", '');
                $(this).parent().css("z-index", Scheduling_Calendar.zIndexDraggable);
                $(this).css("z-index", '1018');
                $('#tabs').css('z-index', '9999');
                $('.tooltip').remove();


            },
            stop: function () {
                $(this).css("z-index", "");
                $('#tabs').css('z-index', '0');
                $('.tooltip').remove();
            }
        });


        $(".resourceappdrop").droppable({
            accept: ".resourceappdrag",
            activeClass: 'droppable-active',
            hoverClass: 'droppable-hover',
            tolerance: 'pointer',
            drop: function (ev, ui) {
                $(this).css("z-index", "");
                $('.zIndex').parent().css("z-index", "");
                //alert(ui.draggable.attr('id'));
                //alert($(this).attr('id'));
                var test = $(this);
                var exist = 0;
                var a = $(this).attr('id');
                var b = ui.draggable.attr('id');
                var arr = a.split(',');
                var arr1 = b.split(',');
                var dragslotdetailid = arr1[1];
                CutAppointmentId = arr1[0];
                var apppatientid = arr1[2];
                var appcount = arr1[3];
                slotid = arr[0].replace('Slotid', '');
                var dateid = arr[arr.length - 4];
                var resourceid = arr[arr.length - 3].replace('resourceid', '');
                var facilityid = arr[arr.length - 2].replace('facilityid', '');
                var resourceid1 = arr1[4].replace('resourceid', '');
                var facilityid1 = arr1[5].replace('facilityid', '');
                slotdetailid = arr[1].replace('TimeSlotDtlId', '');

                var droptime = arr[arr.length - 1];
                var dragtime = arr1[arr1.length - 1];
                var againexist = 0;
                if ((facilityid + resourceid) == (facilityid1 + resourceid1)) {
                    for (var x1 = 2; x1 <= arr.length; x1++) {
                        if ($.trim(arr[x1]) == $.trim(apppatientid) && $.trim(dragslotdetailid) == $.trim(slotdetailid)) {

                            exist = 1;
                        }

                    }

                    if (appcount > 1) {
                        if (dragslotdetailid < slotdetailid) {
                            for (var dr = 0; dr < appcount; dr++) {
                                var sameappmoveid = parseInt(dragslotdetailid) + parseInt(dr);
                                if (slotdetailid == sameappmoveid) {
                                    againexist = 1;
                                }
                            }
                            //if ($(this).parent().attr('id') != undefined && againexist == 0) {
                            //    for (var l = 1; l <= appcount - 1; l++) {

                            //        if (l == 1)
                            //            var nextrow = $(this).parent().attr('id').split(',');
                            //        else
                            //            if ($(this).parent().next().attr('id') != undefined)
                            //                var nextrow = $(this).parent().next().attr('id').split(',');
                            //        //var nextrow = $(this).parent().next().attr('id').split(',');
                            //        for (var x1 = 0; x1 <= nextrow.length; x1++) {
                            //            if ($.trim(nextrow[x1]) == $.trim(apppatientid)) {

                            //                exist = 1;
                            //            }

                            //        }
                            //    }
                            //}
                        }
                        if (dragslotdetailid > slotdetailid) {
                            if ($(this).parent().attr('id') != undefined) {
                                for (var l = 1; l <= appcount - 1; l++) {

                                    if (l == 1)
                                        var nextrow = $(this).parent().attr('id').split(',');
                                    else
                                        if ($(this).parent().prev().attr('id') != undefined)
                                            var nextrow = $(this).parent().prev().attr('id').split(',');
                                    //var nextrow = $(this).parent().next().attr('id').split(',');
                                    for (var x1 = 0; x1 <= nextrow.length; x1++) {
                                        if ($.trim(nextrow[x1]) == $.trim(apppatientid)) {

                                            exist = 1;
                                        }

                                    }
                                }
                            }
                        }
                    }
                    params["TimeSlotDtlId"] = slotdetailid;
                    params["CutAppointmentId"] = CutAppointmentId;

                    var drag_element = ui.draggable[0];

                    if ($.trim(dragslotdetailid) != $.trim(slotdetailid) && exist != 1) {
                        utility.myConfirm('Are you sure want to move appointment from ' + dragtime + ' to ' + droptime + '', function () {

                            Scheduling_Calendar.MoveAppointment(CutAppointmentId, slotdetailid).done(function (response) {
                                if (response.status != false) {
                                    utility.DisplayMessages(response.Message, 1);
                                    ui.draggable.detach().appendTo(test);

                                    Scheduling_MuliView.BackDate(dateid, "null", resourceid, facilityid, 0);

                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });

                        }, function () { },
          'Move Appointment'
      );


                    }

                    else if ($.trim(dragslotdetailid) != $.trim(slotdetailid) && exist == 1)
                        utility.DisplayMessages('Patient is already booked on ' + droptime + ' slot.', 3);
                    //Scheduling_MuliView.LoadMultipleViewCalendar();
                }
            }
        });

        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
    },

    AppointmentStatusUpdate: function (appid, statusid, DateId, ProviderId, ResourceId, FacilityId) {

        AppPrivileges.GetFormPrivileges("Appointment", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                //params["CutAppointmentId"] = appid;

                //params = [];
                //params["AppointmentId"] = appid;
                Scheduling_Calendar.UpdateAppointmentStatus(appid, statusid).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        if (ResourceId == null)
                        { Scheduling_MuliView.BackDate(DateId, ProviderId, ResourceId, FacilityId, 0); }
                        else { Scheduling_MuliView.BackDate(DateId, null, ResourceId, FacilityId, 0); }

                    }
                });

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    PatientAppointmentDelete: function (AppointmentId, DateId, ProviderId, ResourceId, FacilityId) {
        if (ResourceId != null) {
            ProviderId = null;
        }
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Appointment", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = AppointmentId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        appointmentDetail.DeletePatientAppointment(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#dgvSlotEdit').DataTable();
                                table1.row('.active').remove().draw(false);
                                utility.DisplayMessages(response.Message, 1);

                                Scheduling_MuliView.BackDate(DateId, ProviderId, ResourceId, FacilityId, 0);

                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                    '1'
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    CancelPatientCheckIn: function (VisitId, DateId, ProviderId, ResourceId, FacilityId, AppointmentId, refObj) {
        if (ResourceId != null)
        { ProviderId = null; }
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Appointment", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('Are you sure want to Cancel CheckIn?', function () {
                    var selectedValue = VisitId;
                    if (selectedValue == "" || selectedValue == "undefined" || AppointmentId == "" || AppointmentId == "undefined") {
                    }
                    else {
                        var lastStatusName = $(refObj).parent().parent().attr('data-laststatusname');
                        var lastStatusId = $(refObj).parent().parent().attr('data-laststatusid');
                        schcheckin.CancelPatCheckIn(selectedValue, AppointmentId, lastStatusId, lastStatusName).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#dgvSlotEdit').DataTable();
                                table1.row('.active').remove().draw(false);
                                utility.DisplayMessages(response.Message, 1);

                                Scheduling_MuliView.BackDate(DateId, ProviderId, ResourceId, FacilityId, 0);

                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                    'Confirm Cancel CheckIn'
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    PatientDemographics: function (patientid) {
        //var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Demographic", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        //        var params = [];
        //        params["mode"] = 'Edit';
        //        params["patientID"] = patientid;
        //        params["FromAdmin"] = "0";
        //        params["ParentCtrl"] = "schTabMultipleView";
        //        LoadActionPan('demographicDetail', params);
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
        params["QuickAddPatient"] = true;
        SelectTab('mstrTabPatient', 'false');
        setTimeout(function () {
            Patient_Search.SelectPatient(patientid, null);
            $('#patTabDemographic').click();
            $("body").removeClass("modal-open").removeAttr("style");
        }, 200);
    },

    LoadCopayment: function (appid, patientid, DateId, ProviderId, ResourceId, FacilityId, patientvisitid, patvisitname) {

        // if (ResourceId != null) {

        //    utility.DisplayMessages("Copayment can't be collected for Resource Appointmnet", 3);
        // }
        // else {
        var params = [];
        params["FromAdmin"] = "0";
        params["ProviderId"] = ProviderId;
        if (ResourceId != null) {
            params["ProviderId"] = null;
        }
        params["ResourceId"] = ResourceId;
        params["FacilityId"] = FacilityId;
        params["DateId"] = DateId;
        params["PatientVisitId"] = patientvisitid;
        params["PatientVisitName"] = patvisitname;
        params["AppointmentId"] = appid;
        params["PatientId"] = patientid;
        params["ParentCtrl"] = 'schTabMultipleView';
        LoadActionPan('schcopayment', params);
        //}
    },

    LoadCheckIn: function (appid, patientid, DateId, ProviderId, ResourceId, FacilityId, scheduleDate) {

        // if (ResourceId != null) {

        //    utility.DisplayMessages("Resource Appointment can't CheckedIn", 3);
        //}
        //else {

        //var selectedDate = $('#' + DateId + ' p span').text();
        var currentDate = ($.datepicker.formatDate('mm/dd/yy', new Date()));
        if (new Date(scheduleDate) <= new Date(currentDate)) {
            var params = [];
            params["FromAdmin"] = "0";
            params["ProviderId"] = ProviderId;
            if (ResourceId != null) {
                params["ProviderId"] = null;
            }
            params["ResourceId"] = ResourceId;

            params["FacilityId"] = FacilityId;
            params["DateId"] = DateId;
            params["AppointmentId"] = appid;
            params["PatientId"] = patientid;
            params["ParentCtrl"] = 'schTabMultipleView';
            LoadActionPan('schcheckin', params);
        } else {
            utility.DisplayMessages("Future Appointment can't CheckedIn", 3);
        }
        //}

    },

    LoadCheckOut: function (appid, patientid, patientvisitid, DateId, ProviderId, ResourceId, FacilityId) {

        var params = [];
        params["FromAdmin"] = "0";
        params["ProviderId"] = ProviderId;
        if (ResourceId != null) {
            params["ProviderId"] = null;
        }
        params["ResourceId"] = ResourceId;
        params["FacilityId"] = FacilityId;
        params["DateId"] = DateId;
        params["AppointmentId"] = appid;
        params["PatientId"] = patientid;
        params["PatientVisitId"] = patientvisitid;
        params["ParentCtrl"] = 'schTabMultipleView';
        LoadActionPan('schcheckout', params);

    },

    PrintLetter: function () {

        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'schTabMultipleView';
        LoadActionPan('designLetterPrinting', params);
    },

    AppointmentEdit: function (appid, DateId, ProviderId, ResourceId, FacilityId, checkin, visitid, duration, noteid) {
        var isNoteCreated = false;
        isNoteCreated = noteid == "0" ? false : true;
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Appointment", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["checkin"] = checkin;
                params["ProviderId"] = ProviderId;
                if (ResourceId != null && ResourceId > 0) {
                    params["ProviderId"] = null;
                }
                params["ResourceId"] = ResourceId;
                params["FacilityId"] = FacilityId;
                params["DateId"] = DateId;
                params["AppointmentId"] = appid;
                params["PatientVisitId"] = visitid;
                params["SlotMinutes"] = duration;
                //params["SlotdetailId"] = arr[1].replace('TimeSlotDtlId', '');
                params["isNoteCreated"] = isNoteCreated;
                params["mode"] = "Edit";
                params["ParentCtrl"] = "schTabMultipleView";
                LoadActionPan('appointmentDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    BackDate: function (id, ProviderIDs, ResourceIDs, FacilityIDs, add) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Schedule Group", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var d = null;
                if (FacilityIDs != null && id != null) {
                    d = new Date($('#facility' + $.trim(FacilityIDs) + ' #' + $.trim(id) + ' span').html());
                } else {
                    d = new Date();
                }
                var newdate = Scheduling_MuliView.AddSubDays(d, add);
                var curdte = $.datepicker.formatDate('mm/dd/yy', newdate);
                var statusslots = Scheduling_Calendar.FilterCriteria();
                Scheduling_MuliView.SearchMultipleView(ProviderIDs, ResourceIDs, FacilityIDs, curdte, statusslots).done(function (response1) {

                    if (ProviderIDs == "" || ProviderIDs == null || ProviderIDs == "null") {
                        var json = JSON.parse(response1[0])
                        if (json.status != false) {
                            var PatientsDtl = [];
                            var AppointmentsDtl = [];
                            var JSONData = JSON.parse(json.ProviderScheduleFill_JSON);
                            var Slot_Detail = JSON.parse(json.ProviderScheduleFill_JSON);
                            if (Slot_Detail.length != 0) {
                                var table = $('<table class="table table-responsive table-bordered table-condensed" id="table' + $.trim(ResourceIDs) + '"><tbody></tbody></table>');
                                $('#table' + $.trim(ResourceIDs) + '').children('tbody');
                                table.append('<tr align="center"><td colspan="3"><div class="col-sm-12">   <div class="col-xs-1 pl-none pr-none"><a id="rdate' + $.trim(ResourceIDs) + '" providerid="' + $.trim(ProviderIDs) + '" facilityid="' + $.trim(FacilityIDs) + '" resourceid="' + $.trim(ResourceIDs) + '" tableid="rtable' + $.trim(ResourceIDs) + '" class="btn btn-link btn-xs bdate"><i class="fa fa-angle-left"></i></a></div>    <div class="col-xs-9" id="rdate' + $.trim(ResourceIDs) + '">   <p ><span>' + curdte + '</span></p>      </div>        <div class="col-xs-1 pl-none pr-none"><a id="rdate' + $.trim(ResourceIDs) + '" providerid="' + $.trim(ProviderIDs) + '" facilityid="' + $.trim(FacilityIDs) + '" resourceid="' + $.trim(ResourceIDs) + '" tableid="rtable' + $.trim(ResourceIDs) + '" class="btn btn-link btn-xs fdate"><i class="fa fa-angle-right "></i></a></div> <div class="col-xs-1 pl-none pr-none"><a data-plugin-datepicker=""   id="rcalendar' + $.trim(ResourceIDs) + $.trim(FacilityIDs) + '" dateid="rdate' + $.trim(ResourceIDs) + '" providerid="" facilityid="' + $.trim(FacilityIDs) + '" resourceid="' + $.trim(ResourceIDs) + '" class="btn btn-link btn-xs form-control cal"><i class="fa fa-calendar cal"></i></a></div>                               </div> </td> </tr>');
                                var appids = [];
                                var slotappcount;
                                for (var i = 0; i < Slot_Detail.length; i++) {
                                    var appointment;
                                    var margin = Slot_Detail[i].MaxCountApp;

                                    var provid = Slot_Detail[i].ProviderId;
                                    if (provid == '' || provid == 'undefined')
                                        provid = null;

                                    var refprovid = Slot_Detail[i].RefProviderId;
                                    if (refprovid == '' || refprovid == 'undefined')
                                        refprovid = null;

                                    var provname = Slot_Detail[i].ProviderName;
                                    if (provname == '' || provname == 'undefined')
                                        provname = null;

                                    var resid = Slot_Detail[i].ResourceId;
                                    if (resid == '' || resid == 'undefined')
                                        resid = null;
                                    var resname = Slot_Detail[i].ResourceName;
                                    if (resname == '' || resname == 'undefined')
                                        resname = null;
                                    var facid = Slot_Detail[i].FacilityId;
                                    if (facid == '' || facid == 'undefined')
                                        facid = null;
                                    var facname = Slot_Detail[i].FacilityName;
                                    if (facname == '' || facname == 'undefined')
                                        facname = null;
                                    var resonid = Slot_Detail[i].ScheduleReasonId;
                                    if (resonid == '' || resonid == 'undefined')
                                        resonid = null;

                                    var color = "";

                                    if (Slot_Detail[i].FacilityColor == "" || Slot_Detail[i].FacilityColor == "undefined" || Slot_Detail[i].FacilityColor == "null" || Slot_Detail[i].FacilityColor == null) {
                                        color = '';
                                    } else {
                                        color = 'bgcolor = \'' + Slot_Detail[i].FacilityColor + '\'';
                                    }
                                    var dateid = 'rdate' + $.trim(ResourceIDs) + '';
                                    var appointments = '<div class="pull-left">&nbsp;</div>';
                                    if (Slot_Detail[i].AppDtl != "") {
                                        var app;
                                        var test = 0;

                                        var data = Slot_Detail[i].AppDtl;
                                        var arr = data.split('|');

                                        for (var j = 0; j < arr.length; j++) {
                                            slotappcount = (arr.length) - 1;
                                            var split = arr[j].split(',')
                                            if (split.length == 22 || split.length == 24) {

                                                var appid = split[0];
                                                var patientid = split[1];
                                                var patientname = split[2];
                                                patientname = patientname.replace('-', ', ');
                                                var appointmentstatus = split[3];
                                                var appcolor = split[4];
                                                var appcopay = split[6];
                                                var appreason = split[3];
                                                var appcount = split[7];
                                                var patientvisitid = split[9];
                                                var patvisitstatusid = split[10];
                                                var patvisitname = split[11];
                                                var noteStatus = split[16];
                                                var appcomments = split[8];
                                                //Edit By Mohsin Nasir Bug # 2922,2931
                                                var patEligibility = split[12];
                                                var appschreason = split[5];
                                                var noteid = split[13];

                                                var noteTitle = "";
                                                var isNoteCreated = false;

                                                if (noteid == "0") {
                                                    noteTitle = "Create Note";
                                                    isNoteCreated = false;
                                                } else {
                                                    noteTitle = "Edit Note";
                                                    isNoteCreated = true;
                                                }
                                                appschreason = appschreason.replace(/#@#/g, ',');
                                                var eligibilityIcon;

                                                if (patEligibility == "Active") {
                                                    eligibilityIcon = '<i class="fa fa-check-circle green"></i>';
                                                }
                                                else if (patEligibility == "Inactive") {
                                                    eligibilityIcon = '<i class="fa fa-times-circle red"></i>';
                                                }
                                                else if (patEligibility.toLowerCase() == "waiting") {
                                                    eligibilityIcon = '<i class="fa fa-circle black"></i>';
                                                }
                                                else {
                                                    eligibilityIcon = '<i class="fa fa-circle blue"></i>';
                                                }
                                                //END Edit By Mohsin Nasir Bug # 2922,2931

                                                appcopay = appcopay.split('-');
                                                var copaycolor;
                                                //var copaycolor = appcopay[1];
                                                // appcopay = appcopay[0];

                                                //For last Status
                                                var lastStatusId = split[14];
                                                var lastStatusName = split[15];
                                                var patientType = split[17];
                                                var visitType = split[18];
                                                var isNonBillable = split[19];
                                                var billInfoId = split[20];
                                                var billInfoStatus = split[21];
                                                if (provid == null && split.length == 24) {
                                                    provid = split[21];
                                                    provname = split[23];
                                                }
                                                var appcopayamt = parseInt(appcopay[0]);
                                                if (appcopay.length == 3) {
                                                    var appcopayamtpaid = -Math.abs(parseFloat(appcopay[2])); // In Resource Case
                                                } else {
                                                    var appcopayamtpaid = parseFloat(appcopay[1]);    // In Provider Case
                                                }
                                                if (appcopayamtpaid == 0) {
                                                    copaycolor = 'Green'; //copay paid
                                                }
                                                if (appcopayamtpaid > 0) {
                                                    copaycolor = 'Black';//copay blance
                                                }
                                                if (appcopayamtpaid == appcopayamt && appcopayamtpaid > 0) {
                                                    appcopay = appcopayamt;
                                                    copaycolor = 'Red'; // amount copay
                                                }


                                                appids.push(appid);

                                                for (var w = 0; w < appstatus.length; w++) {
                                                    if (appstatus[w].Name.toUpperCase() != "- SELECT -" && appstatus[w].Name != appointmentstatus && appstatus[w].Name.toUpperCase() != "CHECK IN" && appstatus[w].Name.toUpperCase() != "CHECK OUT")
                                                        appstatuses += '<li data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id=' + appstatus[w].Value + ' ><a onclick="Scheduling_MuliView.AppointmentStatusUpdate(' + appid + ',' + appstatus[w].Value + ',\'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');" href="#" >' + appstatus[w].Name + '</a></li> ';
                                                }
                                                appstatuses = appstatuses.replace('undefined', '');
                                                var showapp = 0;

                                                for (z = 0; z < appids.length; z++) {
                                                    if (appids[z] == appid && appid != 0) {
                                                        showapp = showapp + 1;
                                                    }

                                                }
                                                /* Start Code for Reminder Call */
                                                var sms = "SMS";
                                                var voice = "VOICE";
                                                var email = "EMAIL";
                                                var Patient_email = "";
                                                var Appointmentdatetime = "";

                                                try {
                                                    Patient_email = Slot_Detail[i].PatientDetail.split(',')[10];
                                                    Appointmentdatetime = $('#pnlScheduleMuliView #searchdate').val() + " " + Slot_Detail[i].FromTimeSlots;
                                                } catch (ex) {
                                                    console.log(ex);
                                                }
                                                reminderType = '<li id=' + appid + ' ><a onclick="Scheduling_MuliView.SendQuickReminder(' + appid + ',\'' + sms + '\',' + patientid + ',\'' + patientname + '\',\'' + Patient_email + '\',\'' + Appointmentdatetime + '\',\'' + Slot_Detail[i].ProviderId + '\',\'' + Slot_Detail[i].ProviderName + '\');" href="#" >SMS</a></li> <li id=' + appid + ' ><a onclick="Scheduling_MuliView.SendQuickReminder(' + appid + ',\'' + voice + '\',' + patientid + ',\'' + patientname + '\',\'' + Patient_email + '\',\'' + Appointmentdatetime + '\',\'' + Slot_Detail[i].ProviderId + '\',\'' + Slot_Detail[i].ProviderName + '\');" href="#" >Voice</a></li> <li id=' + appid + ' ><a onclick="Scheduling_MuliView.SendQuickReminder(' + appid + ',\'' + email + '\',' + patientid + ',\'' + patientname + '\',\'' + Patient_email + '\',\'' + Appointmentdatetime + '\',\'' + Slot_Detail[i].ProviderId + '\',\'' + Slot_Detail[i].ProviderName + '\');" href="#" >Email</a></li> ';
                                                /* End Code for Reminder Call*/
                                                if (appcount > '1' && showapp == 1) {

                                                    if (appointmentstatus.toUpperCase() != 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                                                        appointments = appointments + '<div class="resourceappdrag apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul  data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckIn(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\', \'' + dateid + '\');">Reschedule</a></li>    <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li></ul>  </div></div> ';
                                                    if (appointmentstatus.toUpperCase() == 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                                                        appointments = appointments + '<div class="resourceappdrag apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + '<a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckIn(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\', \'' + dateid + '\');">Reschedule</a></li>   <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li></ul>  </div></div> ';
                                                    if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() != "SIGNED") {

                                                        var eSuperbillLink = Scheduling_Calendar.getESuperBillLink(patientid, patientvisitid, noteid, "day", patientType, provid, refprovid, facid, appid, billInfoId, billInfoStatus);
                                                        if (isNonBillable == "1") {
                                                            eSuperbillLink = '';
                                                        }
                                                        appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo"  onclick=" Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ');">Check Out</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CancelPatientCheckIn(' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + appid + ',this);">Cancel CheckIn</a></li> <li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadClinicalNote(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\',\'' + provid + '\',\'' + provname + '\',\'' + facid + '\',\'' + facname + '\',null,' + resid + ');">' + noteTitle + '</a></li> <li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#" >Create Charge</a></li><li  id="' + appid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.PrintLetter(' + patientid + ',\'' + patientvisitid + '\',\'' + appid + '\');" >Print Super Bill</a></li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '</ul> </li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> <li  id="' + patientvisitid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.createSuperbill(\'' + patientid.trim() + '\',\'' + patientvisitid.trim() + '\',\'' + noteid.trim() + '\',\'-1\',\'day\',\'' + patientType.trim() + '\',\'' + provid + '\',\'' + refprovid + '\',\'' + facid + '\',\'' + appid + '\');" >Create eSuperbill</a></li> </div></div> ';
                                                    }
                                                    if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() == "SIGNED")
                                                        appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + '<a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + patientvisitid + ', \'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',1,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Check Out</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CancelPatientCheckIn(' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + appid + ',this);">Cancel CheckIn</a></li><li  id="' + appid + '"><a href="#" >Create Charge</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PrintLetter();">Print Super Bill</a></li>  <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                    if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL')
                                                        appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + '<a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + patientvisitid + ', \'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',1,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li>  </ul>  </div></div> ';
                                                    if (appointmentstatus.toUpperCase() == 'CANCEL' || appointmentstatus.toUpperCase() == "NO SHOW")
                                                        appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + '<a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>    <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li>   </ul>  </div></div> ';
                                                    if (patientvisitid != "" && patvisitname.toUpperCase() != 'CHECK IN' && patvisitname.toUpperCase() != 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL')
                                                        appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                }

                                                if (appcount == '1' && showapp == 1) {

                                                    if (appointmentstatus.toUpperCase() != 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                                                        appointments = appointments + '<div class="resourceappdrag apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + '<a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + ' </div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>      <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckIn(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\', \'' + dateid + '\');">Reschedule</a></li>    <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                    if (appointmentstatus.toUpperCase() == 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                                                        appointments = appointments + '<div class="resourceappdrag apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + '<a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + ' </div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckIn(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li>     <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\', \'' + dateid + '\');">Reschedule</a></li>       <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                    if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() != "SIGNED") {

                                                        var eSuperbillLink = Scheduling_Calendar.getESuperBillLink(patientid, patientvisitid, noteid, "day", patientType, provid, refprovid, facid, appid, billInfoId, billInfoStatus);
                                                        if (isNonBillable == "1") {
                                                            eSuperbillLink = '';
                                                        }
                                                        appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo"  onclick=" Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ');">Check Out</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CancelPatientCheckIn(' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + appid + ',this);">Cancel CheckIn</a></li> <li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadClinicalNote(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\',\'' + provid + '\',\'' + provname + '\',\'' + facid + '\',\'' + facname + '\',null,' + resid + ');">' + noteTitle + '</a></li> <li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#" >Create Charge</a></li><li  id="' + appid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.PrintLetter(' + patientid + ',\'' + patientvisitid + '\',\'' + appid + '\');" >Print Super Bill</a></li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '</ul> </li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> <li  id="' + patientvisitid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.createSuperbill(\'' + patientid.trim() + '\',\'' + patientvisitid.trim() + '\',\'' + noteid.trim() + '\',\'-1\',\'day\',\'' + patientType.trim() + '\',\'' + provid + '\',\'' + refprovid + '\',\'' + facid + '\',\'' + appid + '\');" >Create eSuperbill</a></li> </div></div> ';
                                                    }
                                                    if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() == "SIGNED")
                                                        appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + '<a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + ' </div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + patientvisitid + ', \'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',1,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>     <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CancelPatientCheckIn(' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + appid + ',this);">Cancel CheckIn</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Check Out</a></li><li  id="' + appid + '"><a href="#" >Create Charge</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PrintLetter();">Print Super Bill</a></li>  <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li></ul>  </div></div> ';
                                                    if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL')
                                                        appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + ' </div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + patientvisitid + ', \'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',1,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li>    </ul>  </div></div> ';
                                                    if (appointmentstatus.toUpperCase() == 'CANCEL' || appointmentstatus.toUpperCase() == "NO SHOW")
                                                        appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>    <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li>  </ul>  </div></div> ';
                                                    if (patientvisitid != "" && patvisitname.toUpperCase() != 'CHECK IN' && patvisitname.toUpperCase() != 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL')
                                                        appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + '<a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">  <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li>  </ul>  </div></div> ';
                                                }
                                                if (showapp != 1) {
                                                    // appointments = appointments + '<div  style="width:250px;" </div> ';
                                                }
                                                appointments = appointments.replace("undefined", "");
                                                appstatuses = '';
                                                test++;
                                            }
                                        }

                                    }
                                    var dateid = 'rdate' + $.trim(ResourceIDs) + '';
                                    if (Slot_Detail[i].AppDtl == "" && Slot_Detail[i].BlockUnblock == "Blocked") {
                                        table.append('<tr id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + '"><td onclick="Scheduling_MuliView.EditSlot(' + Slot_Detail[i].TimeSlotDtlId + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');" width="63">' + Slot_Detail[i].FromTimeSlots + '</td><td id="' + Slot_Detail[i].TimeSlotDtlId + '"  width="25" align="center">' + Slot_Detail[i].PatientBooked + '</td><td style="color:#fff;" BGCOLOR="#f88379" id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + '" >Blocked:' + Slot_Detail[i].BlockReason + '-' + Slot_Detail[i].Comments + ' </td></tr>');
                                    }
                                    else if (appointments == '<div class="pull-left">&nbsp;</div>') {
                                        table.append('<tr id="' + Slot_Detail[i].Patients + '"><td onclick="Scheduling_MuliView.EditSlot(' + Slot_Detail[i].TimeSlotDtlId + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');" width="63">' + Slot_Detail[i].FromTimeSlots + '</td><td id="' + Slot_Detail[i].TimeSlotDtlId + '"  width="25" align="center">' + Slot_Detail[i].PatientBooked + '</td><td class="resourceappdrop slot" ' + color + ' style="position:relative;" onclick="Scheduling_MuliView.AppointmentAdd(' + Slot_Detail[i].TimeSlotId + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + provid + ', \'' + provname + '\',' + resid + ',\'' + resname + '\',' + facid + ',\'' + facname + '\',' + resonid + ',\'' + dateid + '\',event,\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + Slot_Detail[i].ToTimeSlots + '\',\'' + Slot_Detail[i].IsSpecialist + '\',\'' + Slot_Detail[i].SlotMinutes + '\', \'' + Slot_Detail[i].BlockReason + '\',\'' + Slot_Detail[i].ScheduleDate + '\');" id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + ',' + Slot_Detail[i].Patients + ',rdate' + resid + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '">' + appointments + '</td></tr>');
                                    }
                                    else {

                                        table.append('<tr id="' + Slot_Detail[i].Patients + '"><td onclick="Scheduling_MuliView.EditSlot(' + Slot_Detail[i].TimeSlotDtlId + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');" width="63">' + Slot_Detail[i].FromTimeSlots + '</td><td id="' + Slot_Detail[i].TimeSlotDtlId + '"  width="25" align="center">' + Slot_Detail[i].PatientBooked + '</td><td class="resourceappdrop slot" ' + color + ' style="position:relative;"   onclick="Scheduling_MuliView.AppointmentAdd(' + Slot_Detail[i].TimeSlotId + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + provid + ', \'' + provname + '\',' + resid + ',\'' + resname + '\',' + facid + ',\'' + facname + '\',' + resonid + ',\'' + dateid + '\',event,\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + Slot_Detail[i].ToTimeSlots + '\',\'' + Slot_Detail[i].IsSpecialist + '\',\'' + Slot_Detail[i].SlotMinutes + '\', \'' + Slot_Detail[i].BlockReason + '\',\'' + Slot_Detail[i].ScheduleDate + '\');" id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + ',' + Slot_Detail[i].Patients + ',rdate' + resid + ',resourceid' + resid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"  >' + appointments + '</td></tr>');
                                    }
                                    appointments = '';
                                    PatientsDtl.push(Slot_Detail[i].PatientDetail);
                                    AppointmentsDtl.push(Slot_Detail[i].AppDtl);
                                }
                                $('#facility' + $.trim(FacilityIDs) + ' #resource' + $.trim(ResourceIDs) + '').empty();
                                var ResourceName = $("#Resource option[value=" + $.trim(ResourceIDs) + "]").text();
                                $('#facility' + $.trim(FacilityIDs) + ' #resource' + $.trim(ResourceIDs) + '').append('<div class=" multiple-heading center">Resource:' + ResourceName + '</div>');
                                $('#facility' + $.trim(FacilityIDs) + ' #resource' + $.trim(ResourceIDs) + '').append(table);
                                Scheduling_MuliView.MoveResourceAppointment();
                                if (PatientsDtl.length == AppointmentsDtl.length) {
                                    for (var ini = 0; ini < PatientsDtl.length; ini++) {
                                        Scheduling_MuliView.MultiViewPatientDetails(PatientsDtl[ini], AppointmentsDtl[ini], ProviderName);
                                    }
                                    PatientsDtl = [];
                                    AppointmentsDtl = [];
                                }

                            }
                            else {


                                var table = $('<table  class="table table-responsive table-bordered table-condensed" id="table' + $.trim(ResourceIDs) + '"><tbody></tbody></table>');
                                $('#table' + $.trim(ResourceIDs) + '').children('tbody');
                                table.append('<tr align="center"><td colspan="3"><div class="col-sm-12">   <div class="col-xs-1 pl-none pr-none"><a id="rdate' + $.trim(ResourceIDs) + '" providerid="' + $.trim(ProviderIDs) + '" facilityid="' + $.trim(FacilityIDs) + '" resourceid="' + $.trim(ResourceIDs) + '" tableid="rtable' + $.trim(ResourceIDs) + '" class="btn btn-link btn-xs bdate"><i class="fa fa-angle-left"></i></a></div>    <div class="col-xs-9" id="rdate' + $.trim(ResourceIDs) + '">   <p ><span>' + curdte + '</span></p>      </div>        <div class="col-xs-1 pl-none pr-none"><a id="rdate' + $.trim(ResourceIDs) + '" providerid="' + $.trim(ProviderIDs) + '" facilityid="' + $.trim(FacilityIDs) + '" resourceid="' + $.trim(ResourceIDs) + '" tableid="rtable' + $.trim(ResourceIDs) + '" class="btn btn-link btn-xs fdate"><i class="fa fa-angle-right "></i></a></div> <div class="col-xs-1 pl-none pr-none"><a data-plugin-datepicker=""   id="rcalendar' + $.trim(ResourceIDs) + $.trim(FacilityIDs) + '" dateid="rdate' + $.trim(ResourceIDs) + '" providerid="" facilityid="' + $.trim(FacilityIDs) + '" resourceid="' + $.trim(ResourceIDs) + '" class="btn btn-link btn-xs form-control cal"><i class="fa fa-calendar cal"></i></a></div>                               </div> </td> </tr>');
                                table.append('<tr align="center"><th style="text-align: center;" >No Schedule Found.</th>  </tr>');

                                $('#facility' + $.trim(FacilityIDs) + ' #resource' + $.trim(ResourceIDs) + '').empty();
                                var ResourceName = $("#Resource option[value=" + $.trim(ResourceIDs) + "]").text();
                                $('#facility' + $.trim(FacilityIDs) + ' #resource' + $.trim(ResourceIDs) + '').append('<div class=" multiple-heading center">Rsource:' + ResourceName + '</div>');
                                $('#facility' + $.trim(FacilityIDs) + ' #resource' + $.trim(ResourceIDs) + '').append(table);



                            }
                        }
                        else {
                            var table = $('<table  class="table table-responsive table-bordered table-condensed" id="table' + $.trim(ResourceIDs) + '"><tbody></tbody></table>');
                            $('#table' + $.trim(ResourceIDs) + '').children('tbody');
                            table.append('<tr align="center"><td colspan="3"><div class="col-sm-12">   <div class="col-xs-1 pl-none pr-none"><a id="rdate' + $.trim(ResourceIDs) + '" providerid="' + $.trim(ProviderIDs) + '" facilityid="' + $.trim(FacilityIDs) + '" resourceid="' + $.trim(ResourceIDs) + '" tableid="rtable' + $.trim(ResourceIDs) + '" class="btn btn-link btn-xs bdate"><i class="fa fa-angle-left"></i></a></div>    <div class="col-xs-9" id="rdate' + $.trim(ResourceIDs) + '">   <p ><span>' + curdte + '</span></p>      </div>        <div class="col-xs-1 pl-none pr-none"><a id="rdate' + $.trim(ResourceIDs) + '" providerid="' + $.trim(ProviderIDs) + '" facilityid="' + $.trim(FacilityIDs) + '" resourceid="' + $.trim(ResourceIDs) + '" tableid="rtable' + $.trim(ResourceIDs) + '" class="btn btn-link btn-xs fdate"><i class="fa fa-angle-right "></i></a></div> <div class="col-xs-1 pl-none pr-none"><a data-plugin-datepicker=""   id="rcalendar' + $.trim(ResourceIDs) + $.trim(FacilityIDs) + '" dateid="rdate' + $.trim(ResourceIDs) + '" providerid="" facilityid="' + $.trim(FacilityIDs) + '" resourceid="' + $.trim(ResourceIDs) + '" class="btn btn-link btn-xs form-control cal"><i class="fa fa-calendar cal"></i></a></div>                               </div> </td> </tr>');
                            table.append('<tr align="center"><th style="text-align: center;" >No Schedule Found.</th>  </tr>');

                            $('#facility' + $.trim(FacilityIDs) + ' #resource' + $.trim(ResourceIDs) + '').empty();
                            var ResourceName = $("#Resource option[value=" + $.trim(ResourceIDs) + "]").text();
                            $('#facility' + $.trim(FacilityIDs) + ' #resource' + $.trim(ResourceIDs) + '').append('<div class=" multiple-heading center">Rsource:' + ResourceName + '</div>');
                            $('#facility' + $.trim(FacilityIDs) + ' #resource' + $.trim(ResourceIDs) + '').append(table);

                        }
                    }
                    if (ResourceIDs == "" || ResourceIDs == null || ResourceIDs == "null") {
                        var json = JSON.parse(response1[0])
                        if (json.status != false) {
                            var PatientsDtl = [];
                            var AppointmentsDtl = [];
                            var JSONData = JSON.parse(json.ProviderScheduleFill_JSON);
                            var Slot_Detail = JSON.parse(json.ProviderScheduleFill_JSON);
                            if (Slot_Detail.length != 0) {
                                var table = $('<table class="table table-responsive table-bordered table-condensed" id="table' + $.trim(ProviderIDs) + '"><tbody></tbody></table>');
                                $('#table' + $.trim(ProviderIDs) + '').children('tbody');

                                table.append('<tr align="center"><td colspan="3"><div class="col-sm-12">   <div class="col-xs-1 pl-none pr-none"><a id="pdate' + $.trim(ProviderIDs) + '" providerid="' + $.trim(ProviderIDs) + '" facilityid="' + $.trim(FacilityIDs) + '" resourceid="' + $.trim(ResourceIDs) + '" tableid="ptable' + $.trim(ProviderIDs) + '" class="btn btn-link btn-xs bdate"><i class="fa fa-angle-left"></i></a></div>    <div class="col-xs-9" id="pdate' + $.trim(ProviderIDs) + '">   <p ><span>' + curdte + '</span></p>      </div>        <div class="col-xs-1 pl-none pr-none"><a id="pdate' + $.trim(ProviderIDs) + '" providerid="' + $.trim(ProviderIDs) + '" facilityid="' + $.trim(FacilityIDs) + '" resourceid="' + $.trim(ResourceIDs) + '" tableid="ptable' + $.trim(ProviderIDs) + '" class="btn btn-link btn-xs fdate"><i class="fa fa-angle-right "></i></a></div> <div class="col-xs-1 pl-none pr-none"><a data-plugin-datepicker=""   id="pcalendar' + $.trim(ProviderIDs) + $.trim(FacilityIDs) + '" dateid="pdate' + $.trim(ProviderIDs) + '" providerid="' + $.trim(ProviderIDs) + '" facilityid="' + $.trim(FacilityIDs) + '" resourceid="" class="btn btn-link btn-xs form-control cal"><i class="fa fa-calendar cal"></i></a></div>                                </div> </td> </tr>');
                                var appids = [];
                                var slotappcount;
                                for (var i = 0; i < Slot_Detail.length; i++) {
                                    var appointment;
                                    var margin = Slot_Detail[i].MaxCountApp;

                                    var provid = Slot_Detail[i].ProviderId;
                                    if (provid == '' || provid == 'undefined')
                                        provid = null;

                                    var refprovid = Slot_Detail[i].RefProviderId;
                                    if (refprovid == '' || refprovid == 'undefined')
                                        refprovid = null;

                                    var provname = Slot_Detail[i].ProviderName;
                                    if (provname == '' || provname == 'undefined')
                                        provname = null;

                                    var resid = Slot_Detail[i].ResourceId;
                                    if (resid == '' || resid == 'undefined')
                                        resid = null;
                                    var resname = Slot_Detail[i].ResourceName;
                                    if (resname == '' || resname == 'undefined')
                                        resname = null;
                                    var facid = Slot_Detail[i].FacilityId;
                                    if (facid == '' || facid == 'undefined')
                                        facid = null;
                                    var facname = Slot_Detail[i].FacilityName;
                                    if (facname == '' || facname == 'undefined')
                                        facname = null;
                                    var resonid = Slot_Detail[i].ScheduleReasonId;
                                    if (resonid == '' || resonid == 'undefined')
                                        resonid = null;

                                    var color = "";

                                    if (Slot_Detail[i].FacilityColor == "" || Slot_Detail[i].FacilityColor == "undefined" || Slot_Detail[i].FacilityColor == "null" || Slot_Detail[i].FacilityColor == null) {
                                        color = '';
                                    } else {
                                        color = 'bgcolor = \'' + Slot_Detail[i].FacilityColor + '\'';
                                    }
                                    var dateid = 'pdate' + $.trim(ProviderIDs) + '';
                                    var appointments = '<div class="pull-left">&nbsp;</div>';
                                    if (Slot_Detail[i].AppDtl != "") {
                                        var app;
                                        var test = 0;

                                        var data = Slot_Detail[i].AppDtl;
                                        var arr = data.split('|');

                                        for (var j = 0; j < arr.length; j++) {
                                            slotappcount = (arr.length) - 1;
                                            var split = arr[j].split(',')
                                            if (split.length == 22) {

                                                var appid = split[0];
                                                var patientid = split[1];
                                                var patientname = split[2];
                                                patientname = patientname.replace('-', ', ');
                                                var appointmentstatus = split[3];
                                                var appcolor = split[4];
                                                var appcopay = split[6];
                                                var appreason = split[3];
                                                var appcount = split[7];
                                                var patientvisitid = split[9];
                                                var patvisitstatusid = split[10];
                                                var noteStatus = split[16];
                                                var patvisitname = split[11];
                                                var appcomments = split[8];
                                                var appschreason = split[5];
                                                appschreason = appschreason.replace(/#@#/g, ',');
                                                //Edit By Mohsin Nasir Bug # 2922,2931
                                                var patEligibility = split[12];
                                                var eligibilityIcon;
                                                var noteid = split[13];

                                                var noteTitle = "";
                                                var isNoteCreated = false;

                                                if (noteid == "0") {
                                                    noteTitle = "Create Note";
                                                    isNoteCreated = false;
                                                } else {
                                                    noteTitle = "Edit Note";
                                                    isNoteCreated = true;
                                                }
                                                if (patEligibility == "Active") {
                                                    eligibilityIcon = '<i class="fa fa-check-circle green"></i>';
                                                }
                                                else if (patEligibility == "Inactive") {
                                                    eligibilityIcon = '<i class="fa fa-times-circle red"></i>';
                                                }
                                                else if (patEligibility.toLowerCase() == "waiting") {
                                                    eligibilityIcon = '<i class="fa fa-circle black"></i>';
                                                }
                                                else {
                                                    eligibilityIcon = '<i class="fa fa-circle blue"></i>';
                                                }
                                                //END Edit By Mohsin Nasir Bug # 2922,2931
                                                appcopay = appcopay.split('-');
                                                var copaycolor;
                                                //var copaycolor = appcopay[1];
                                                // appcopay = appcopay[0];

                                                //For last Status
                                                var lastStatusId = split[14];
                                                var lastStatusName = split[15];
                                                var patientType = split[17];
                                                var visitType = split[18];

                                                var isNonBillable = split[19];
                                                var appcopayamt = parseInt(appcopay[0]);
                                                var appcopayamtpaid = parseInt(appcopay[1]);

                                                if (appcopayamtpaid == 0) {
                                                    copaycolor = 'Green'; //copay paid
                                                }
                                                if (appcopayamtpaid > 0) {
                                                    copaycolor = 'Black';//copay blance
                                                }
                                                if (appcopayamtpaid == appcopayamt && appcopayamtpaid > 0) {
                                                    appcopay = appcopayamt;
                                                    copaycolor = 'Red'; // amount copay
                                                }
                                                appids.push(appid);

                                                for (var w = 0; w < appstatus.length; w++) {
                                                    if (appstatus[w].Name.toUpperCase() != "- SELECT -" && appstatus[w].Name != appointmentstatus && appstatus[w].Name.toUpperCase() != "CHECK IN" && appstatus[w].Name.toUpperCase() != "CHECK OUT")
                                                        appstatuses += '<li data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id=' + appstatus[w].Value + ' ><a onclick="Scheduling_MuliView.AppointmentStatusUpdate(' + appid + ',' + appstatus[w].Value + ',\'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');" href="#" >' + appstatus[w].Name + '</a></li> ';
                                                }
                                                appstatuses = appstatuses.replace('undefined', '');
                                                var showapp = 0;

                                                for (z = 0; z < appids.length; z++) {
                                                    if (appids[z] == appid) {
                                                        showapp = showapp + 1;
                                                    }

                                                }
                                                /* Start Code for Reminder Call */
                                                var sms = "SMS";
                                                var voice = "VOICE";
                                                var email = "EMAIL";
                                                var Patient_email = "";
                                                var Appointmentdatetime = "";

                                                try {
                                                    Patient_email = Slot_Detail[i].PatientDetail.split(',')[10];
                                                    Appointmentdatetime = $('#pnlScheduleMuliView #searchdate').val() + " " + Slot_Detail[i].FromTimeSlots;
                                                } catch (ex) {
                                                    console.log(ex);
                                                }
                                                reminderType = '<li id=' + appid + ' ><a onclick="Scheduling_MuliView.SendQuickReminder(' + appid + ',\'' + sms + '\',' + patientid + ',\'' + patientname + '\',\'' + Patient_email + '\',\'' + Appointmentdatetime + '\',\'' + Slot_Detail[i].ProviderId + '\',\'' + Slot_Detail[i].ProviderName + '\');" href="#" >SMS</a></li> <li id=' + appid + ' ><a onclick="Scheduling_MuliView.SendQuickReminder(' + appid + ',\'' + voice + '\',' + patientid + ',\'' + patientname + '\',\'' + Patient_email + '\',\'' + Appointmentdatetime + '\',\'' + Slot_Detail[i].ProviderId + '\',\'' + Slot_Detail[i].ProviderName + '\');" href="#" >Voice</a></li> <li id=' + appid + ' ><a onclick="Scheduling_MuliView.SendQuickReminder(' + appid + ',\'' + email + '\',' + patientid + ',\'' + patientname + '\',\'' + Patient_email + '\',\'' + Appointmentdatetime + '\',\'' + Slot_Detail[i].ProviderId + '\',\'' + Slot_Detail[i].ProviderName + '\');" href="#" >Email</a></li> ';
                                                /* End Code for Reminder Call*/
                                                if (appcount > '1' && showapp == 1) {

                                                    if (appointmentstatus.toUpperCase() != 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                                                        appointments = appointments + '<div class="providerappdrag apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckIn(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\', \'' + dateid + '\');">Reschedule</a></li>  <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                    //azharH
                                                    if (appointmentstatus.toUpperCase() == 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                                                        appointments = appointments + '<div class="providerappdrag apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + '  <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">    <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckIn(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\', \'' + dateid + '\');">Reschedule</a></li>   <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                    if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() != "SIGNED") {

                                                        var eSuperbillLink = Scheduling_Calendar.getESuperBillLink(patientid, patientvisitid, noteid, "day", patientType, provid, refprovid, facid, appid, billInfoId, billInfoStatus);
                                                        if (isNonBillable == "1") {
                                                            eSuperbillLink = '';
                                                        }
                                                        appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo"  onclick=" Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ');">Check Out</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CancelPatientCheckIn(' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + appid + ',this);">Cancel CheckIn</a></li> <li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadClinicalNote(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\',\'' + provid + '\',\'' + provname + '\',\'' + facid + '\',\'' + facname + '\',null);">' + noteTitle + '</a></li> <li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#" >Create Charge</a></li><li  id="' + appid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.PrintLetter(' + patientid + ',\'' + patientvisitid + '\',\'' + appid + '\');" >Print Super Bill</a></li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '</ul></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li>  <li  id="' + patientvisitid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.createSuperbill(\'' + patientid.trim() + '\',\'' + patientvisitid.trim() + '\',\'' + noteid.trim() + '\',\'-1\',\'day\',\'' + patientType.trim() + '\',\'' + provid + '\',\'' + refprovid + '\',\'' + facid + '\',\'' + appid + '\');" >Create eSuperbill</a></li> </div></div> ';
                                                    }
                                                    if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() == "SIGNED")
                                                        appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + '  <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">    <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + patientvisitid + ', \'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',1,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Check Out</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CancelPatientCheckIn(' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + appid + ',this);">Cancel CheckIn</a></li><li  id="' + appid + '"><a href="#" >Create Charge</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PrintLetter();">Print Super Bill</a></li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li></ul>  </div></div> ';
                                                    if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL')
                                                        appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + '  <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">    <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + patientvisitid + ', \'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',1,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li>   </ul>  </div></div> ';
                                                    if (appointmentstatus.toUpperCase() == 'CANCEL' || appointmentstatus.toUpperCase() == "NO SHOW")
                                                        appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + '  <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">    <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',2,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                    if (patientvisitid != "" && patvisitname.toUpperCase() != 'CHECK IN' && patvisitname.toUpperCase() != 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL')
                                                        appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">  ' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">    <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',2,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li>  </ul>  </div></div> ';
                                                }

                                                if (appcount == '1' && showapp == 1) {

                                                    if (appointmentstatus.toUpperCase() != 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                                                        appointments = appointments + '<div class="providerappdrag apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + '  <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span> </div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">     <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>     <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckIn(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li>     <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\', \'' + dateid + '\');">Reschedule</a></li>   <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                    if (appointmentstatus.toUpperCase() == 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                                                        appointments = appointments + '<div class="providerappdrag apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">  ' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + ' </div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">     <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',0,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckIn(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li>      <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\', \'' + dateid + '\');">Reschedule</a></li>      <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li></ul>  </div></div> ';
                                                    if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() != "SIGNED") {

                                                        var eSuperbillLink = Scheduling_Calendar.getESuperBillLink(patientid, patientvisitid, noteid, "day", patientType, provid, refprovid, facid, appid, billInfoId, billInfoStatus);
                                                        if (isNonBillable == "1") {
                                                            eSuperbillLink = '';
                                                        }
                                                        appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo"  onclick=" Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ');">Check Out</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CancelPatientCheckIn(' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + appid + ',this);">Cancel CheckIn</a></li> <li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadClinicalNote(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\',\'' + provid + '\',\'' + provname + '\',\'' + facid + '\',\'' + facname + '\',null);">' + noteTitle + '</a></li> <li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#" >Create Charge</a></li><li  id="' + appid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.PrintLetter(' + patientid + ',\'' + patientvisitid + '\',\'' + appid + '\');" >Print Super Bill</a></li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '</ul></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li>  <li  id="' + patientvisitid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.createSuperbill(\'' + patientid.trim() + '\',\'' + patientvisitid.trim() + '\',\'' + noteid.trim() + '\',\'-1\',\'day\',\'' + patientType.trim() + '\',\'' + provid + '\',\'' + refprovid + '\',\'' + facid + '\',\'' + appid + '\');" >Create eSuperbill</a></li> </div></div> ';
                                                    }
                                                    if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() == "SIGNED")
                                                        appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">  ' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + ' </div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">     <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + patientvisitid + ', \'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',1,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Check Out</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CancelPatientCheckIn(' + patientvisitid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + appid + ',this);">Cancel CheckIn</a></li><li  id="' + appid + '"><a href="#" >Create Charge</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PrintLetter();">Print Super Bill</a></li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="">Appointment Reminder</a>   <ul> ' + reminderType + '   </ul>  </li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                    if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL')
                                                        appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">  ' + eligibilityIcon + ' <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + ' </div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">     <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadCopayment(' + appid + ',' + patientid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',' + patientvisitid + ', \'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',1,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li>   </ul>  </div></div> ';
                                                    if (appointmentstatus.toUpperCase() == 'CANCEL' || appointmentstatus.toUpperCase() == "NO SHOW")
                                                        appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + '  <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">    <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',2,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientAppointmentDelete(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Delete Appointment</a></li> <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_MuliView.AppointmentStatusUpdate( \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                    if (patientvisitid != "" && patvisitname.toUpperCase() != 'CHECK IN' && patvisitname.toUpperCase() != 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL')
                                                        appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%;height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) + Scheduling_MuliView.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + '  <a href="#" class="helllo" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');" id=' + patientid + '-1' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '</div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_MuliView.slotHeight) - Scheduling_MuliView.buttonHeightAdj) + 'px;">    <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.AppointmentEdit(' + appid + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ',2,\'' + patientvisitid + '\',\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_MuliView.CreateLetter(' + patientid + ');">Create Letter</a></li>  </ul>  </div></div> ';
                                                }
                                                if (showapp != 1) {
                                                    // appointments = appointments + '<div  style="width:250px;" </div> ';
                                                }
                                                appointments = appointments.replace("undefined", "");
                                                appstatuses = '';
                                                test++;
                                            }
                                        }

                                    }
                                    var dateid = 'pdate' + $.trim(ProviderIDs) + '';
                                    if (Slot_Detail[i].AppDtl == "" && Slot_Detail[i].BlockUnblock == "Blocked") {
                                        table.append('<tr id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + '"><td onclick="Scheduling_MuliView.EditSlot(' + Slot_Detail[i].TimeSlotDtlId + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');" width="63">' + Slot_Detail[i].FromTimeSlots + '</td><td id="' + Slot_Detail[i].TimeSlotDtlId + '"  width="25" align="center">' + Slot_Detail[i].PatientBooked + '</td><td style="color:#fff;" BGCOLOR="#f88379" id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + '" >Blocked:' + Slot_Detail[i].BlockReason + '-' + Slot_Detail[i].Comments + ' </td></tr>');
                                    }
                                    else if (appointments == '<div class="pull-left">&nbsp;</div>') {
                                        table.append('<tr id="' + Slot_Detail[i].Patients + '"><td onclick="Scheduling_MuliView.EditSlot(' + Slot_Detail[i].TimeSlotDtlId + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');" width="63">' + Slot_Detail[i].FromTimeSlots + '</td><td id="' + Slot_Detail[i].TimeSlotDtlId + '"  width="25" align="center">' + Slot_Detail[i].PatientBooked + '</td><td class="providerappdrop slot" ' + color + ' style="position:relative;" onclick="Scheduling_MuliView.AppointmentAdd(' + Slot_Detail[i].TimeSlotId + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + provid + ', \'' + provname + '\',' + resid + ',\'' + resname + '\',' + facid + ',\'' + facname + '\',' + resonid + ',\'' + dateid + '\',event,\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + Slot_Detail[i].ToTimeSlots + '\',\'' + Slot_Detail[i].IsSpecialist + '\',\'' + Slot_Detail[i].SlotMinutes + '\', \'' + Slot_Detail[i].BlockReason + '\',\'' + Slot_Detail[i].ScheduleDate + '\');" id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + ',' + Slot_Detail[i].Patients + ',pdate' + provid + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '">' + appointments + '</td></tr>');
                                    }
                                    else {

                                        table.append('<tr id="' + Slot_Detail[i].Patients + '"><td onclick="Scheduling_MuliView.EditSlot(' + Slot_Detail[i].TimeSlotDtlId + ', \'' + dateid + '\',' + provid + ',' + resid + ',' + facid + ');" width="63">' + Slot_Detail[i].FromTimeSlots + '</td><td id="' + Slot_Detail[i].TimeSlotDtlId + '"  width="25" align="center">' + Slot_Detail[i].PatientBooked + '</td><td class="providerappdrop slot" ' + color + ' style="position:relative;"   onclick="Scheduling_MuliView.AppointmentAdd(' + Slot_Detail[i].TimeSlotId + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + provid + ', \'' + provname + '\',' + resid + ',\'' + resname + '\',' + facid + ',\'' + facname + '\',' + resonid + ',\'' + dateid + '\',event,\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + Slot_Detail[i].ToTimeSlots + '\',\'' + Slot_Detail[i].IsSpecialist + '\',\'' + Slot_Detail[i].SlotMinutes + '\', \'' + Slot_Detail[i].BlockReason + '\',\'' + Slot_Detail[i].ScheduleDate + '\');" id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + ',' + Slot_Detail[i].Patients + ',pdate' + provid + ',providerid' + provid + ',facilityid' + facid + ',' + Slot_Detail[i].FromTimeSlots + '"  >' + appointments + '</td></tr>');
                                    }
                                    appointments = '';
                                    PatientsDtl.push(Slot_Detail[i].PatientDetail);
                                    AppointmentsDtl.push(Slot_Detail[i].AppDtl);
                                }

                                $('#facility' + $.trim(FacilityIDs) + ' #provider' + $.trim(ProviderIDs) + '').empty();
                                var ProviderName = $("#Provider option[value=" + $.trim(ProviderIDs) + "]").text();
                                $('#facility' + $.trim(FacilityIDs) + ' #provider' + $.trim(ProviderIDs) + '').append('<div class=" multiple-heading center">Provider:' + ProviderName + '</div>');
                                $('#facility' + $.trim(FacilityIDs) + ' #provider' + $.trim(ProviderIDs) + '').append(table);
                                Scheduling_MuliView.MoveProviderAppointment();
                                Scheduling_MuliView.dropDownMenuClick(Scheduling_MuliView.params.PanelID);
                                if (PatientsDtl.length == AppointmentsDtl.length) {
                                    for (var ini = 0; ini < PatientsDtl.length; ini++) {
                                        Scheduling_MuliView.MultiViewPatientDetails(PatientsDtl[ini], AppointmentsDtl[ini], ProviderName);
                                    }
                                    PatientsDtl = [];
                                    AppointmentsDtl = [];
                                }

                            }
                            else {


                                var table = $('<table  class="table table-responsive table-bordered table-condensed" id="table' + $.trim(ProviderIDs) + '"><tbody></tbody></table>');
                                $('#table' + $.trim(ProviderIDs) + '').children('tbody');
                                table.append('<tr align="center"><td colspan="3"><div class="col-sm-12">   <div class="col-xs-1 pl-none pr-none"><a id="pdate' + $.trim(ProviderIDs) + '" providerid="' + $.trim(ProviderIDs) + '" facilityid="' + $.trim(FacilityIDs) + '" resourceid="' + $.trim(ResourceIDs) + '" tableid="ptable' + $.trim(ProviderIDs) + '" class="btn btn-link btn-xs bdate"><i class="fa fa-angle-left"></i></a></div>    <div class="col-xs-9" id="pdate' + $.trim(ProviderIDs) + '">   <p ><span>' + curdte + '</span></p>      </div>        <div class="col-xs-1 pl-none pr-none"><a id="pdate' + $.trim(ProviderIDs) + '" providerid="' + $.trim(ProviderIDs) + '" facilityid="' + $.trim(FacilityIDs) + '" resourceid="' + $.trim(ResourceIDs) + '" tableid="ptable' + $.trim(ProviderIDs) + '" class="btn btn-link btn-xs fdate"><i class="fa fa-angle-right "></i></a></div> <div class="col-xs-1 pl-none pr-none"><a data-plugin-datepicker=""   id="pcalendar' + $.trim(ProviderIDs) + $.trim(FacilityIDs) + '" dateid="pdate' + $.trim(ProviderIDs) + '" providerid="' + $.trim(ProviderIDs) + '" facilityid="' + $.trim(FacilityIDs) + '" resourceid="" class="btn btn-link btn-xs form-control cal"><i class="fa fa-calendar cal"></i></a></div>                                </div> </td> </tr>');
                                table.append('<tr align="center"><th style="text-align: center;" >No Schedule Found.</th>  </tr>');

                                $('#facility' + $.trim(FacilityIDs) + ' #provider' + $.trim(ProviderIDs) + '').empty();
                                var ProviderName = $("#Provider option[value=" + $.trim(ProviderIDs) + "]").text();
                                $('#facility' + $.trim(FacilityIDs) + ' #provider' + $.trim(ProviderIDs) + '').append('<div class=" multiple-heading center">Provider:' + ProviderName + '</div>');
                                $('#facility' + $.trim(FacilityIDs) + ' #provider' + $.trim(ProviderIDs) + '').append(table);



                            }
                        }
                        else {
                            var table = $('<table  class="table table-responsive table-bordered table-condensed" id="table' + $.trim(ProviderIDs) + '"><tbody></tbody></table>');
                            $('#table' + $.trim(ProviderIDs) + '').children('tbody');
                            table.append('<tr align="center"><td colspan="3"><div class="col-sm-12">   <div class="col-xs-1 pl-none pr-none"><a id="pdate' + $.trim(ProviderIDs) + '" providerid="' + $.trim(ProviderIDs) + '" facilityid="' + $.trim(FacilityIDs) + '" resourceid="' + $.trim(ResourceIDs) + '" tableid="ptable' + $.trim(ProviderIDs) + '" class="btn btn-link btn-xs bdate"><i class="fa fa-angle-left"></i></a></div>    <div class="col-xs-9" id="pdate' + $.trim(ProviderIDs) + '">   <p ><span>' + curdte + '</span></p>      </div>        <div class="col-xs-1 pl-none pr-none"><a id="pdate' + $.trim(ProviderIDs) + '" providerid="' + $.trim(ProviderIDs) + '" facilityid="' + $.trim(FacilityIDs) + '" resourceid="' + $.trim(ResourceIDs) + '" tableid="ptable' + $.trim(ProviderIDs) + '" class="btn btn-link btn-xs fdate"><i class="fa fa-angle-right "></i></a></div> <div class="col-xs-1 pl-none pr-none"><a data-plugin-datepicker=""   id="pcalendar' + $.trim(ProviderIDs) + $.trim(FacilityIDs) + '" dateid="pdate' + $.trim(ProviderIDs) + '" providerid="' + $.trim(ProviderIDs) + '" facilityid="' + $.trim(FacilityIDs) + '" resourceid="" class="btn btn-link btn-xs form-control cal"><i class="fa fa-calendar cal"></i></a></div>                                </div> </td> </tr>');
                            table.append('<tr align="center"><th style="text-align: center;" >No Schedule Found.</th>  </tr>');

                            $('#facility' + $.trim(FacilityIDs) + ' #provider' + $.trim(ProviderIDs) + '').empty();
                            var ProviderName = $("#Provider option[value=" + $.trim(ProviderIDs) + "]").text();
                            $('#facility' + $.trim(FacilityIDs) + ' #provider' + $.trim(ProviderIDs) + '').append('<div class=" multiple-heading center">Provider:' + ProviderName + '</div>');
                            $('#facility' + $.trim(FacilityIDs) + ' #provider' + $.trim(ProviderIDs) + '').append(table);
                        }
                    }
                    Scheduling_MuliView.InitializationDatePicker();
                    //Scheduling_Calendar.Initializationtooltip();
                    Scheduling_Calendar.dropDownMenuClick(Scheduling_MuliView.params.PanelID);
                    Scheduling_Calendar.subDropdownMenuMouseEnter();
                });

            }
            else
                utility.DisplayMessages(strMessage, 2);

        });
    },
    LoadUnallocatedCopayment: function (appid, patientid, facid, provid,visitid) {
        var params = [];
        params["FromAdmin"] = "0";
        params["AppointmentId"] = appid;
        params["ProviderId"] = provid;
        params["FacilityId"] = facid;
        params["PatientId"] = patientid;
        params["ParentCtrl"] = 'schTabMultipleView';
        params["VisitId"] = visitid ? visitid : 0;
        LoadActionPan('Scheduling_UnallocatedCopayment', params);
    },

    AddSubDays: function (theDate, days) {

        return new Date(theDate.getTime() + days * 24 * 60 * 60 * 1000);

    },

    ValidateMultiViewSearch: function () {
        $('#frmSchedulingMuliView')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   Group: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Date: {
                       group: '.col-sm-3',
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
            Scheduling_MuliView.LoadMultipleViewCalendar();
        });
    },

    SearchMultipleView: function (ProviderIDs, ResourceIDs, FacilityIDs, CriteriaDate, StatusId) {
        var data = "ProviderID=" + ProviderIDs + "&ResourceID=" + ResourceIDs + "&FacilityID=" + FacilityIDs + "&SlotDate=" + CriteriaDate + "&StatusId=" + StatusId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_MULIVIEW", "SEARCH_MULTIPLEVIEW");
    },

    SearchScheduleGroupProRes: function (MSGroupId, CriteriaDate, statusslots) {
        var data = "MSGroupId=" + MSGroupId + "&SlotDate=" + CriteriaDate + "&StatusId=" + statusslots;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_MULIVIEW", "LOAD_SCHEDULE_GROUP_PRORES");
    },

    StickyHeading: function () {
        //for sticky heading
        $("#maincontainer").parent().append("<div class='maincontainer-clone hidden'><div class='maincontainer-inner'></div></div>");
        var containerCloneDiv = $('.maincontainer-clone');
        var containerCloneInnerDiv = $('.maincontainer-inner');
        var containerCloneInnerDivWidth = null;
        var checkWidthParameter;
        //remove clone's on new data
        $("#btnsearch").click(function () {
            containerCloneInnerDiv.html("");
        });
        function cloneHeader() {
            var cols = $("#maincontainer > div");
            var headerWidthSum = null;
            containerCloneInnerDiv.html("");
            $(cols).each(function (e) {
                var facility = $(this).children();//facility
                var facilityFormat = facility.prev(".multiple-heading");//geting heading

                var provider = facility.children('div');//provider
                var providerDiv = provider.parent();//provider parent div's

                var table = facility.children("table");
                //set clone
                var facilityFormatClone = facilityFormat.clone().removeAttr("style");//facility
                var providerDivClone = providerDiv.clone().removeAttr("id");
                var colDiv = facility.prev(".multiple-heading").parent().clone().html("").addClass('col-clone').removeAttr('id');

                colDiv.append(facilityFormatClone);

                containerCloneInnerDiv.append(colDiv);
                $(providerDivClone).each(function (provClone) {
                    var providerInner = $(this).children('div');//provider
                    var providerOuterDiv = $(this).html("");//provider outerDiv
                    providerOuterDiv.append(providerInner);
                    colDiv.append(providerOuterDiv);
                });
            });
            //sum of children width
            $(containerCloneInnerDiv.children()).each(function (cc) {
                var $colDivWidth = $(this).outerWidth();
                checkWidthParameter = $(this).attr("style");
                headerWidthSum += $colDivWidth;
            });
            checkWidthParameter = checkWidthParameter;
            containerCloneInnerDivWidth = headerWidthSum;

            if (checkWidthParameter != undefined) {
                //if width in % then remove the width from all childrens
                if (checkWidthParameter.indexOf("%") > 0) {
                    containerCloneDiv.children().removeAttr('style');
                }
                else {
                    //assigning the width to clone inner div
                    containerCloneInnerDiv.css('width', containerCloneInnerDivWidth);
                }
            }
            containerCloneDiv.css('width', containerCloneDiv.parent().width());
        }
        $(window).scroll(function () {
            var $scroll = $(window).scrollTop();
            if ($scroll >= 150) {
                containerCloneDiv.removeClass("hidden");
                containerCloneDiv.scrollLeft(containerCloneDiv.parent().scrollLeft());//adjest header scroll accourding to body
                if (containerCloneInnerDiv.children().length <= 0) {
                    cloneHeader();
                }
            } else {
                containerCloneDiv.addClass("hidden");
            }
        });
        //scroll x
        $(containerCloneDiv.parent()).scroll(function () {
            containerCloneDiv.scrollLeft(containerCloneDiv.parent().scrollLeft());
        });
        //on div resize
        $($('.maincontainer-clone').parent()).resize(function () {
            containerCloneDiv.css('width', containerCloneDiv.parent().width());
            //check the width paremater and reset width
            if (checkWidthParameter != undefined) {
                //if width in % then remove the width from all childrens
                if (checkWidthParameter.indexOf("%") > 0) {
                    containerCloneDiv.children().removeAttr('style');
                }
                else {
                    //assigning the width to clone inner div
                    containerCloneInnerDiv.css('width', containerCloneInnerDivWidth);
                }
            }
        });

    },

    OpenPatientEligibility: function (appid) {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Appointment", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                appointmentDetail.FillAppointment(appid).done(function (response) {

                    if (response.status != false) {
                        var appointment_detail = JSON.parse(response.AppointmentFill_JSON);

                        var patientname = appointment_detail.PatientName.split(',');

                        var params = [];
                        params["FromAdmin"] = "0";
                        params["patientID"] = appointment_detail.hfpatientid;
                        params["patientAccount"] = appointment_detail.AccountNumber;
                        if (patientname.length > 0) {
                            params["patientLastName"] = patientname[1];
                            params["patientFirstName"] = patientname[0];
                        }
                        params["patientInsurancePlanId"] = appointment_detail.ddlInsurancePlan;
                        params["Provider"] = appointment_detail.txtProvider;
                        params["ProviderId"] = appointment_detail.hfProviderId;
                        params["ParentCtrl"] = 'schTabMultipleView';
                        LoadActionPan('Patient_Eligibility', params);


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

    SendQuickReminder: function (appId, type, patientId, patientName, PatientEmail, Appointmentdate, providerId, providerName) {
        Scheduling_Calendar.FillPatientPreferences(patientId).done(function (response) {
            if (response.status != false) {

                var patData = JSON.parse(response.PreferencesFill_JSON);

                var patientCellNo = patData.patientCellNo;
                var patientHomeNo = patData.patientHomeNo;
                var patientWorkNo = patData.patientWorkNo;
                var patientGuarantorId = patData.patientGuarantorId;
                var guarantorNumber = patData.guarantorNumber;
                var isGuarantorAttached = patData.chkcommnwithgrntr;
                var guarantorRelation = patData.guarantorRelationText;
                if (isGuarantorAttached.toLowerCase() != "true" || guarantorRelation.toLowerCase() == "self") {
                    patientGuarantorId = "";
                }

                guarantorNumber = guarantorNumber.replace(/[_\W]+/g, "");
                var patientNumber = "";
                if (patientCellNo == "" && patientHomeNo == "" && patientWorkNo == "") {
                    patientNumber = "";
                } else if (patientCellNo == "" && patientHomeNo != "") {
                    patientNumber = patientHomeNo.replace(/[_\W]+/g, "");
                } else if (patientCellNo == "" && patientHomeNo == "" && patientWorkNo != "") {
                    patientNumber = patientWorkNo.replace(/[_\W]+/g, "");
                } else if (patientCellNo != "") {
                    patientNumber = patientCellNo.replace(/[_\W]+/g, "");
                }
                if (patData.chkcommnoptout == "True") {
                    utility.DisplayMessages(patientName + " has opted out for appointment reminders", 2);
                } else {
                    var params = [];
                    params["RemindersTemplateId"] = "-1";
                    params["AppointmentId"] = appId;
                    params["ProviderId"] = $('#pnlScheduleCalendar #Provider').val();
                    params["PatientId"] = patientId;
                    params["ScreenType"] = type;
                    params["patientNumber"] = patientNumber;
                    params["patientGuarantorId"] = patientGuarantorId;
                    params["PatientName"] = patientName;
                    params["PatientEmail"] = PatientEmail;
                    params["ProviderName"] = $("#pnlScheduleCalendar #Provider option:selected").text();
                    params["AppointmentDate"] = Appointmentdate;
                    params["patientGuarantorNumber"] = guarantorNumber;
                    params["mode"] = "Add";
                    params["ParentCtrl"] = 'schTabMultipleView';
                    LoadActionPan('remindersDetail', params);
                }

            }

        });

    },

    MultiViewPatientDetails: function (responsePatients, responseAppDtl, ProvName) {



        var resultPat = responsePatients.split('|');

        var resultApp = responseAppDtl.split('|');
        //if ($("#pnlScheduleCalendar input:radio[name='RadioProvider']").is(":checked")) {
        //    var provName = $('#pnlScheduleCalendar #Provider option:selected').text();
        //} else if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {
        //    var provName = $('#pnlScheduleCalendar #Resource option:selected').text();
        //}
        var provName = ProvName;


        if (resultApp.length == resultPat.length) {



            for (var i = 0; i < resultPat.length - 1; i++) {

                var patientDtl = resultPat[i].split(',');


                var fullDate = new Date(patientDtl[2].replace(/\-/g, "/"));

                var DOB = fullDate.toLocaleDateString();



                var appDtl = resultApp[i].split(',');

                var patientname = appDtl[2].replace('-', ', ');

                var ctrl = appDtl[1] + '-1' + appDtl[0]

                var age = Scheduling_Calendar.getAge(fullDate);
                var appcopay = appDtl[6];
                var appschreason = appDtl[5];
                appschreason = appschreason.replace(/#@#/g, ',');
                appcopay = appcopay.split('-');
                var appcopayamtpaid = 0;
                if (appcopay.length == 3) {
                    appcopayamtpaid = -Math.abs(parseFloat(appcopay[2]));
                } else {
                    appcopayamtpaid = parseFloat(appcopay[1]);
                }


                var reminder_delivery_status = patientDtl[13] == (undefined) ? "" : patientDtl[13];
                var reminder_delivery_response = patientDtl[14] == (undefined) ? "" : patientDtl[14];
                var status_style_color = "";

                if (reminder_delivery_status != "") {
                    reminder_delivery_status = reminder_delivery_status.toLocaleLowerCase() == 'new' ? 'Waiting' : reminder_delivery_status;
                    reminder_delivery_status = reminder_delivery_status.toLocaleLowerCase() == 'failed' ? 'Not Delivered' : reminder_delivery_status;
                    reminder_delivery_status = reminder_delivery_status.toLocaleLowerCase() == 'made' ? 'Successfully Delivered' : reminder_delivery_status;
                }

                if (reminder_delivery_response != "") {
                    reminder_delivery_response = reminder_delivery_response.toLocaleLowerCase() == '1' ? 'Confirm' : reminder_delivery_response;
                    reminder_delivery_response = reminder_delivery_response.toLocaleLowerCase() == '2' ? 'Cancel' : reminder_delivery_response;
                    //reminder_delivery_response = reminder_delivery_response.toLocaleLowerCase() == '0' ? 'Invalid Key Press' : reminder_delivery_response;
                    if (reminder_delivery_response.toLocaleLowerCase() == '3' || reminder_delivery_response.toLocaleLowerCase() == '4' || reminder_delivery_response.toLocaleLowerCase() == '5' || reminder_delivery_response.toLocaleLowerCase() == '6' || reminder_delivery_response.toLocaleLowerCase() == '7' || reminder_delivery_response.toLocaleLowerCase() == '8' || reminder_delivery_response.toLocaleLowerCase() == '9') {
                        reminder_delivery_response = 'Invalid Key Press';
                    }
                    if (reminder_delivery_response.toLocaleLowerCase() == '0') {
                        reminder_delivery_response = 'No Response';
                    }
                }
                if (reminder_delivery_response.toLocaleLowerCase() == "confirm") {
                    status_style_color = 'style = "color:green"';
                }

                popover = '<h5 class="pull-left m-none pr-xxs">' + patientname + '</h5> <span>' + age + ' Y, ' + patientDtl[4] + '; '
                               + '<b>Account:</b> ' + patientDtl[1] + '; '
                               + '<b>DOB: </b> ' + patientDtl[2].replace(/\-/g, "/") + '</span>'
                               + '<hr class="stooltip " >'

                               + '<ul class="list-unstyled pl-none">'

                               + '<li><b>Mobile:</b> ' + patientDtl[3] + '</li>'
                               + '<li><b>Reason:</b> ' + appschreason + '</li>'
                               + '<li><b>Primary Plan:</b> ' + patientDtl[12] + '</li>'
                               + '<li><b>Provider/Resource:</b> ' + provName + '</li>'
                               + '<li><b>Copay:</b> ' + appcopayamtpaid + '</li>'
                               + '<li><b>Status:</b> ' + appDtl[3] + '</li>'
                               + '<li><b>Reminder Delivery Status:</b> ' + reminder_delivery_status + '</li>'
                               + '<li><b>Reminder Response:</b> <span ' + status_style_color + '>' + reminder_delivery_response + '</span></li>'
                               + '<li><b>Time:</b> ' + patientDtl[5] + '(Minutes)</li>'
                               + '<li><b>Patient Type:</b> ' + appDtl[17] + '</li>'
                               + '<li><b>Visit Type:</b> ' + appDtl[18] + '</li>'
                               + '<li><b>Comments:</b> ' + appDtl[8] + '</li>'
                               + '<li><b>Created by:</b> ' + patientDtl[6].replace('_',', ') + '</li>'
                               + '<li><b>Created on:</b> ' + patientDtl[7] + '</li>'
                               + '<li><b>Last Modified by:</b> ' + patientDtl[8].replace('_', ', ') + '</li>'
                               + '<li><b>Last Modified on:</b> ' + patientDtl[9] + '</li>'

                               + '<li><b>E.mail:</b>' + patientDtl[10] + '</li>';
                +'</ul>'

                //if (PatientInsurance_Detail == []) {

                //    + 'Primary Plan: ' + PatientInsurance_Detail[0].InsurancePlanName + '<br>'
                //    + 'Copay: ' + PatientInsurance_Detail[0].AmtCopay + '<br>'

                //}
                //else {
                //    + 'Primary Plan: No Plan Found<br>'
                //}
                $('#' + ctrl).tooltipster({
                    theme: 'tooltipster-shadow',
                    content: $(popover),
                    functionReady: function (instance, helper) {
                        var posTop = $(helper)[0].getBoundingClientRect().top;
                        var anchorBottom = $(this)[0].getBoundingClientRect().bottom;
                        if (posTop < 0) {
                            $('.tooltipster-base').css("top", (anchorBottom + 13) + "px");
                            $('.tooltipster-arrow').removeClass("tooltipster-arrow-top").addClass("tooltipster-arrow-bottom");
                        }
                    }

                });

            }

        }


    },

    CreateLetter: function (PatientId) {
        var params = [];
        params["ParentCtrl"] = "schTabMultipleView";
        params["FromAdmin"] = 0;
        params["mode"] = "Add";
        params["PatientId"] = PatientId;
        LoadActionPan("SelectLetter_Template", params);
    },

    dropDownMenuClick: function (parentId) {
        $("#" + parentId + " .buttonDrop.dropdown").on("click", Scheduling_MuliView.parentMenuPosition);
    },

    RescheduleAppointmentSearch: function (facilityId, facilityName, providerId, providerName, resourceId, resourceName, appointmentId, dateid) {

        var params = [];
        params["FromAdmin"] = "0";
        params["FacilityId"] = facilityId;
        params["FacilityName"] = facilityName;
        params["ProviderId"] = providerId;
        params["ProviderName"] = providerName;
        if (resourceId != null) {
            params["ProviderId"] = "null";
            params["ProviderName"] = "null";
        }
        params["ResourceId"] = resourceId;
        params["ResourceName"] = resourceName;
        params["AppointmentId"] = appointmentId;
        params["DateId"] = dateid;
        params["MultipleView"] = '1';
        params["ParentCtrl"] = 'schTabMultipleView';
        LoadActionPan('Scheduling_RescheduleSearch', params);

    },

}


