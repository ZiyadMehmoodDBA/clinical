/*
    Author: Muhammad Azhar Shahzad
    Creation Date: November 18,2015
    OverView:This File Is created for Clinical Notes
*/

Clinical_Notes = {
    bIsFirstLoad: true,
    params: [],
    CopyNotes: false,
    PrevNoteId: 0,
    NewInsertTables: "",
    IsNoteAlreadyCreated: false,
    arrCQMReasoning: [],
    arrVBPReasoning: [],
    totalNotesCount: 0,
    totalSignedNotesCount: 0,
    totalDraftNotesCount: 0,
    Load: function (params) {
        Clinical_Notes.params.NoteComponentList = []
        if ($("#signNotePrint #ulContent").length > 0) {
            $("#signNotePrint #ulContent").html('');
        }
        Clinical_ProgressNote.params.isProgressNoteSelected = false;
        Clinical_ProgressNote.params.newlyAddedProblemLists = [];

        Clinical_Notes.IsNoteAlreadyCreated = false;
        $('#clinicalTabPhoneEncounter').removeClass("active");
        $('#clinicalTabNotes').addClass("active");
        Clinical_Notes.params = params;
        Clinical_Notes.CopyNotes = false;
        Clinical_Notes.PrevNoteId = 0;
        Clinical_Notes.NewInsertTables = "";
        var ParentCntrlLoadId = Clinical_Notes.params["ParentCntrlLoadid"] || null;
        //If Note is edited from Dashboard, Global Notes, Appointments, Schedular, than this condition check for editing notes
        if (Clinical_Notes.params.ForProgressNote) {

            Clinical_Notes.params["mode"] = "Edit";
            $('#' + Clinical_Notes.params.PanelID + ' #frmClinicalNotes').resetAllControls();
            $('#' + Clinical_Notes.params.PanelID + ' #frmClinicalNotes [type=hidden]').val('');
            $('#' + Clinical_Notes.params.PanelID + ' #ProgressnoteHTML').html('');
            Clinical_Notes.params.ForProgressNote = null;
            Clinical_Notes.AddProgressNoteTab();

            return false;
        }

        var self = $('#pnlClinicalNotes');
        if (Clinical_Notes.params.PanelID != 'pnlClinicalNotes') {
            self = $('#' + Clinical_Notes.params.PanelID + " #pnlClinicalNotes");
        }

        $('#' + Clinical_Notes.params.PanelID + ' #frmClinicalNotes').resetAllControls();
        $('#' + Clinical_Notes.params.PanelID + ' #frmClinicalNotes [type=hidden]').val('');
        $('#' + Clinical_Notes.params.PanelID + ' #ProgressnoteHTML').html('');

        //removing attribute of Note Type and Note template, so that it will not make request with self.loaddropdown
        self.find('.NoteType > select').removeAttr('ddlist');
        self.find('.NoteTemplate > select').removeAttr('ddlist');

        if (ParentCntrlLoadId == "Schedular") {

            // Schedular should always bring Mode View or Add
            if (Clinical_Notes.params.mode != "View") {
                Clinical_Notes.params.mode = "Add";
            }
        } else {
            Clinical_Notes.params.mode = "Add";
        }

        if (Clinical_Notes.bIsFirstLoad) {

            var self = $('#' + Clinical_Notes.params.PanelID);
            self.loadDropDowns(true).done(function () {

            });
            Clinical_Notes.bIsFirstLoad = false;
            Clinical_Notes.LoadAllAutocomplete();
            Clinical_Notes.BindProvider();
            Clinical_Notes.BindResourceProvider();
            Clinical_Notes.BindFacility();
            Clinical_Notes.BindReferralProvider();
        }
        if (globalAppdata["isTransitonCancerRegistries"] && globalAppdata["isTransitonCancerRegistries"].toLowerCase() == "false")
            Clinical_Notes.LoadVisitTypeddlWO_CancerRegistries();
        else
            $('#' + Clinical_Notes.params.PanelID + ' #divPatientVisityType #ddlVisitType').attr('ddlist', "GetPatientVisitType");
        var self = $('#' + Clinical_Notes.params.PanelID);
        self.find('#divPatientVisityType').loadDropDowns(true);
        if (Clinical_Notes.params["ResourceId"] && Clinical_Notes.params.ParentCntrlLoadid == "Schedular") {
            $('#' + Clinical_Notes.params.PanelID + ' #frmClinicalNotes #ResourceDiv').show();
        } else {
            $('#' + Clinical_Notes.params.PanelID + ' #frmClinicalNotes #ResourceDiv').hide();
        }

        Clinical_Notes.bindDateAndTimepicker();
        Clinical_Notes.LoadNoteGrid().done(function () {

            // Follwoing check Only calling for first load note
            if (Clinical_Notes.bIsFirstLoad && ParentCntrlLoadId == "Schedular") {
                Scheduling_Calendar.isVisitUpdForAppointment = true;
            }

            if (ParentCntrlLoadId == "Schedular" || ParentCntrlLoadId == "Dashboard") {

                //If call is from Dashboard, Global Notes, Appointments or Schedular, than this will load default values to Notes
                Clinical_Notes.LoadNotesDefaults();

                $('#' + Clinical_Notes.params.PanelID + ' #providerDiv').addClass('disableAll');
                Clinical_Notes.params["ParentCntrlLoadid"] = null;

            } else {

                $('#' + Clinical_Notes.params.PanelID + ' #providerDiv').removeClass('disableAll');
                // Setting Default room to Lobby
                var LobbyId = $('#' + Clinical_Notes.params.PanelID + ' #frmClinicalNotes #RoomNo option').filter(function () { return $(this).html() == "Lobby"; }).val();
                if (LobbyId != null && LobbyId != '') {
                    $('#' + Clinical_Notes.params.PanelID + ' #frmClinicalNotes #RoomNo').val(LobbyId);
                }

                //Added by Nabeel
                if (!Clinical_Notes.bIsFirstLoad) {

                    // Unlinking Appointments and Previous Note
                    Clinical_Notes.UnlinkAppointment($("#pnlClinicalNotes #ChkBox_LinkedAppointment"), 'pnlClinicalNotes', true);
                    Clinical_Notes.UnLinkPreviousNotePatient($("#pnlClinicalNotes #chkCopayPreviousNote"), 'pnlClinicalNotes');
                }
            }


            Clinical_Notes.GetNotesTemplates(null, true);

            // Remove Phone Encounter Options
            $("#" + Clinical_Notes.params["PanelID"] + " #frmClinicalNotes #NoteType option").each(function () {
                if ($(this).text().replace(' ', '').toLowerCase() == "phoneencounter") {
                    $(this).remove();
                }
            });
        });

        Clinical_Notes.AddProgressNoteTab(true);

        $("#" + Clinical_Notes.params["PanelID"] + " #frmClinicalNotes :input").change(function () {
            Clinical_Notes.params.triggerCount = 0;
        });

        if (Clinical_Notes.params.mode == "View") {
            Clinical_Notes.NotesPreview(Clinical_Notes.params.NotesId, null, Clinical_Notes.params.patientID, Clinical_Notes.params.NotesProviderId);
        } else {
            if (Clinical_Notes.params["ParentCntrlLoadid"] == "Schedular" && Clinical_Notes.params.NotesId != null) {
                Clinical_Notes.NotesEdit(Clinical_Notes.params.NotesId, 'Edit', window.event);
            }
        }
       
        Clinical_Notes.GetOrderSetTemplate();
        Clinical_Notes.ValidateNotes('pnlClinicalNotes #frmClinicalNotes');
        if (localStorage.getItem('IsNoteRightCollasped') && localStorage.getItem('IsNoteRightCollasped') == 'true') {
            $('html').addClass('sidebar-left-collapsed')
        }
    },
    GetOrderSetTemplate: function () {
        setTimeout(function () {
            var self = $('#pnlClinicalNotes');
            var NoteTemplateVal = $("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes #NoteTemplate").val();

            var data = "IsActive=1&StrID=" + $("#pnlClinicalNotes #hfProvider").val();
            var orderSetDefaultValue = "";
            self.find('.OrderSet > select').attr('ddlist', 'GetOrderSetTemplate');
            self.find('.OrderSet').loadDropDowns(true, data).done(function () {
                var data1 = "IsActive=1&StrID=" + +$("#pnlClinicalNotes #hfProvider").val() + "&StrID2=" + NoteTemplateVal;
                return MDVisionService.lookups('GetOrderSetTemplateByID', true, data1).done(function (result) {
                    if (result["GetOrderSetTemplateByID"]) {
                        result = JSON.parse(result["GetOrderSetTemplateByID"]);
                        $.each(result, function (j, result) {
                            if (result.Value) {
                                orderSetDefaultValue = result.Name;
                            }
                        });

                        if (orderSetDefaultValue) {
                            self.find(".OrderSet option").prop('selected', false).filter(function () {
                                return $(this).text() == orderSetDefaultValue;
                            }).prop('selected', true);
                        }
                    }
                })

            });
        }, 5);
    },
    CheckNoteTemplate: function (obj) {

        var noteTemplateId = $('#' + Clinical_Notes.params.PanelID + " #NoteTemplate").val();
        if (noteTemplateId == '') {
            $(obj).val('');
            utility.DisplayMessages("Please select note template.", 3);
        }
    },
    LoadVisitTypeddlWO_CancerRegistries: function () {
        var dfd = $.Deferred();
        MDVisionService.lookups('GetPatientVisitTypeWithoutCancerRegistries', true, "").done(function (results) {
            var htmlddl = "";
            if (results["GetPatientVisitTypeWithoutCancerRegistries"])
                results = JSON.parse(results["GetPatientVisitTypeWithoutCancerRegistries"]);
            if (results) {
                $.each(results, function (j, result) {
                    htmlddl += '<option refval="' + result.RefValue + '" value="' + result.Value + '">' + result.Name + '</option>';
                });
            }
            $('#' + Clinical_Notes.params.PanelID + ' #divPatientVisityType #ddlVisitType').html(htmlddl);
            dfd.resolve();
        });
        return dfd;
    },
    bindDateAndTimepicker: function () {

        utility.CreateDatePicker('pnlClinicalNotes #frmClinicalNotes #dtpVisitDate',
             //on-change callback method
             function (ev) {
                 if ($('#pnlClinicalNotes #frmClinicalNotes').data("bootstrapValidator") != null && typeof $('#frmClinicalNotes').data('bootstrapValidator') != 'undefined') {
                     $('#pnlClinicalNotes #frmClinicalNotes').bootstrapValidator('revalidateField', 'VisitDate');
                 }
             }, true);

        //on date change, validating time
        $('#pnlClinicalNotes #frmClinicalNotes #VisitTime').timepicker().on('changeTime.timepicker', function (e) {
            disableFocus: false;
            if ($('#pnlClinicalNotes #frmClinicalNotes').data("bootstrapValidator") != null && typeof $('#frmClinicalNotes').data('bootstrapValidator') != 'undefined') {
                $('#pnlClinicalNotes #frmClinicalNotes').bootstrapValidator('revalidateField', 'VisitTime');
            }
        });

        $('#pnlClinicalNotes #frmClinicalNotes #VisitTime').timepicker({
            defaultTime: 'now',
            minuteStep: 5//,
        });

        $('#pnlClinicalNotes #frmClinicalNotes #VisitTime').timepicker('setTime', new Date());
    },

    //If call is from Dashboard, Global Notes, Appointments or Schedular, than this will load default values to Notes
    LoadNotesDefaults: function () {

        var LobbyId = $('#pnlClinicalNotes #frmClinicalNotes #RoomNo option').filter(function () { return $(this).html() == "Lobby"; }).val();
        if (LobbyId != null) {
            $('#pnlClinicalNotes #frmClinicalNotes #RoomNo').val(LobbyId);
        }

        if (Clinical_Notes.params != null && Object.keys(Clinical_Notes.params).length > 0) {

            $('#pnlClinicalNotes #frmClinicalNotes #hfPatientId').val(Clinical_Notes.params["PatientId"]);

            if (Clinical_Notes.params["NotesVisitId"] != null) {
                var AppointmentId = Clinical_Notes.params["AppointmentId"];
                var NotesVisitId = Clinical_Notes.params["NotesVisitId"];
                var NotesVisitDate = Clinical_Notes.params["NotesVisitDate"];
                var NotesVisitTime = Clinical_Notes.params["NotesVisitTime"];
                var NotesProviderId = Clinical_Notes.params["NotesProviderId"];
                var NotesProviderName = Clinical_Notes.params["NotesProviderName"];
                var NotesFacilityName = Clinical_Notes.params["NotesFacilityName"];
                var NotesFacilityId = Clinical_Notes.params["NotesFacilityId"];
                var ScheduleReason = Clinical_Notes.params["ScheduleReason"];
                var NotesRoom = Clinical_Notes.params["NotesRoom"];
                var RefProviderId = Clinical_Notes.params["RefProviderId"];
                var RefProviderName = Clinical_Notes.params["RefProviderName"];
                var ResourceId = Clinical_Notes.params["ResourceId"];
                var ResourceName = Clinical_Notes.params["ResourceName"];
                var ResourceproviderId = Clinical_Notes.params["ResourceproviderId"];
                var ResourceproviderName = Clinical_Notes.params["ResourceproviderName"];
                var VisitTypeId = Clinical_Notes.params["VisitTypeId"];
                var PatientTypeId = Clinical_Notes.params["PatientTypeId"];
                Clinical_Notes.BindVisitsValues(AppointmentId, NotesVisitId, NotesVisitDate, NotesVisitTime, NotesProviderId, NotesProviderName, NotesFacilityName, NotesFacilityId, ScheduleReason, NotesRoom, RefProviderId, RefProviderName, false, ResourceId, ResourceName, ResourceproviderId, ResourceproviderName, VisitTypeId, PatientTypeId);
                $.when(Clinical_Notes.VisitSearch(Clinical_Notes.params["NotesVisitId"], Clinical_Notes.params["patientID"])).then(function () {
                    if (Clinical_Notes.params["AppointmentId"] && Clinical_Notes.params["AppointmentId"] > 0) {
                        $('#' + Clinical_Notes.params.PanelID + ' #hfAppointmentId').val(Clinical_Notes.params["AppointmentId"]);
                    }
                });

            }

            Clinical_Notes.UnLinkPreviousNotePatient($("#pnlClinicalNotes #chkCopayPreviousNote"), 'pnlClinicalNotes');
            Clinical_Notes.UnlinkAppointment($("#pnlClinicalNotes #ChkBox_LinkedAppointment"), 'pnlClinicalNotes', true);
        }
    },
    getdatetime: function (dt, bitTimeNeed) {
        var res = "";
        res += Clinical_Notes.formatdigits(dt.getMonth() + 1);
        res += "/";
        res += Clinical_Notes.formatdigits(dt.getDate());
        res += "/";
        res += Clinical_Notes.formatdigits(dt.getFullYear());
        if (bitTimeNeed) {
            res += " ";
            var hours = Clinical_Notes.formatdigits(dt.getHours() > 12 ? dt.getHours() - 12 : dt.getHours());
            if (hours == "00")
                hours = "12"
            res += hours
            res += ":";
            res += Clinical_Notes.formatdigits(dt.getMinutes());
            res += " " + dt.getHours() > 11 ? " PM" : " AM";
        }
        return res;
    },
    formatdigits: function (val) {
        val = val.toString();
        return val.length == 1 ? "0" + val : val;
    },
    //ON page load, if paramaters has visit id, than data of that visit is loading by default in controls
    BindVisitsValues: function (AppointmentId, NotesVisitId, NotesVisitDate, NotesVisitTime, NotesProviderId, NotesProviderName, NotesFacilityName, NotesFacilityId, ScheduleReason, NotesRoom, RefProviderId, RefProviderName, IsBindVisitInfo, ResourceId, ResourceName, ResourceproviderId, ResourceproviderName, VisitTypeId, PatientTypeId) {
        $('#' + Clinical_Notes.params.PanelID + ' #hfAppointmentId').val(AppointmentId);
        $('#' + Clinical_Notes.params.PanelID + ' #hfVisitId').val(NotesVisitId);

        //if (Clinical_Notes.params.TabID == 'clinicalTabProgressNote') {
        //    Clinical_ProgressNote.params.VisitId = NotesVisitId;
        //    $('#' + Clinical_ProgressNote.params.PanelID + ' #hfVisitId').val(NotesVisitId);
        //}
        if (Clinical_Notes.params.TabID == 'clinicalTabNotes') {
            $('#' + Clinical_Notes.params.PanelID + ' #txtVisitReason').val(ScheduleReason);
        }
        if (!IsBindVisitInfo) {
            var dtpVisitDate = null;
            if (NotesVisitDate) {
                dtpVisitDate = Clinical_Notes.getdatetime(new Date(NotesVisitDate.replace(/-/g, "/")), false);
            }
            $('#' + Clinical_Notes.params.PanelID + ' #dtpVisitDate').val(dtpVisitDate);
            $('#' + Clinical_Notes.params.PanelID + ' #dtpVisitDate').datepicker('setDate', dtpVisitDate)

            if (NotesVisitTime != '' && NotesVisitDate != '' && NotesVisitTime != null && NotesVisitDate != null) {
                var visitDatetime = new Date(NotesVisitDate + " " + NotesVisitTime);
                $('#' + Clinical_Notes.params.PanelID + ' #VisitTime').val(utility.getTimeIn12HrFromDatetime(visitDatetime));
                $('#' + Clinical_Notes.params.PanelID + ' #VisitTime').timepicker('setTime', utility.getTimeIn12HrFromDatetime(visitDatetime));

                $('#' + Clinical_Notes.params.PanelID + ' #txtLinkedAppointment').val(Clinical_Notes.getdatetime(visitDatetime, true));

                $('#' + Clinical_Notes.params.PanelID + ' #ChkBox_LinkedAppointment').prop('checked', true);
            } else {
                $('#' + Clinical_Notes.params.PanelID + ' #ChkBox_LinkedAppointment').prop('checked', false);
                $('#' + Clinical_Notes.params.PanelID + ' #txtLinkedAppointment').val('No Appointment Selected');
                $('#' + Clinical_Notes.params.PanelID + ' #VisitTime').timepicker('setTime', new Date());
                var date_format = 'dd/mm/yyyy';
                //set default Date Formate
                if (globalAppdata['DateFormat'])
                    date_format = globalAppdata['DateFormat'];
                $('#' + Clinical_Notes.params.PanelID + ' #dtpVisitDate').datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), new Date()));
            }

            $('#' + Clinical_Notes.params.PanelID + ' #txtVisitReason').val(ScheduleReason);
            // MU3-32 Faizan Ameen

            var objDefferedDLL = $.Deferred();
            if ($('#' + Clinical_Notes.params.PanelID + ' #ddlVisitType option').length > 0) {
                objDefferedDLL.resolve("ok");

            }
            else {
                var self = $("#" + Clinical_Notes.params["PanelID"]);
                self.find('.VisitType').loadDropDowns(true).done(function () {
                    objDefferedDLL.resolve("ok");
                });

            }
            $.when(objDefferedDLL).then(function () {
                Clinical_Notes.showDropdownOptions(PatientTypeId).done(function () {
                    $('#' + Clinical_Notes.params.PanelID + ' #ddlVisitType option[value="' + VisitTypeId + '"]').prop('selected', true);
                    if (Clinical_Notes.params.PanelID == "pnlClinicalProgressNote") {
                        //  Clinical_ProgressNote.UpdateVisitType();
                        Clinical_ProgressNote.EnableDisableCancerReportButton()
                    }

                });






            });



            if (NotesRoom != null) {
                var RoomNoId = $('#' + Clinical_Notes.params.PanelID + ' #RoomNo option').filter(function () { return $(this).html() == NotesRoom; }).val();
                if (RoomNoId != null && RoomNoId != '') {
                    $('#' + Clinical_Notes.params.PanelID + ' #RoomNo').val(RoomNoId);
                }
            }
        }

        if ((NotesProviderName != null && NotesProviderId != null) && (NotesProviderName != '' && NotesProviderId != '')) {
            $('#' + Clinical_Notes.params.PanelID + ' #txtProvider').val(NotesProviderName);
            $('#' + Clinical_Notes.params.PanelID + ' #hfProvider').val(NotesProviderId);

            if (Clinical_Notes.params.PanelID != 'pnlClinicalProgressNote') {
                $('#' + Clinical_Notes.params.PanelID + ' #lnkProviderEdit').css("display", "inline");
                $('#' + Clinical_Notes.params.PanelID + ' #lblProvider').css("display", "none");
            }
            if (Clinical_Notes.params.TabID == 'clinicalTabProgressNote') {
                $('#' + Clinical_ProgressNote.params.PanelID + ' #hfProviderId').val(NotesFacilityId);
            }

            $Ctrl_p = $('#' + Clinical_Notes.params.PanelID + ' #txtProvider');
            $hfCtrl_p = $('#' + Clinical_Notes.params.PanelID + ' #hfProvider');
            //Provider
            utility.SetKendoAutoCompleteSourceforValidate($Ctrl_p, $Ctrl_p.val(), $hfCtrl_p, $hfCtrl_p.val());

        } else {
            if ($('#' + Clinical_Notes.params.PanelID + ' #txtProvider').val() == '') {

                if (Clinical_Notes.params.PanelID != 'pnlClinicalProgressNote') {
                    $('#' + Clinical_Notes.params.PanelID + ' #lnkProviderEdit').css("display", "none");
                    $('#' + Clinical_Notes.params.PanelID + ' #lblProvider').css("display", "inline");
                }
            }
        }
        if ((NotesFacilityName != null && NotesFacilityId != null) && (NotesFacilityName != '' && NotesFacilityId != '')) {
            $('#' + Clinical_Notes.params.PanelID + ' #txtFacility').val(NotesFacilityName);
            $('#' + Clinical_Notes.params.PanelID + ' #hfFacility').val(NotesFacilityId);
            if (Clinical_Notes.params.PanelID != 'pnlClinicalProgressNote') {
                $('#' + Clinical_Notes.params.PanelID + ' #lnkFacilityEdit').css("display", "inline");
                $('#' + Clinical_Notes.params.PanelID + ' #lblFacility').css("display", "none");
            }
            if (Clinical_Notes.params.TabID == 'clinicalTabProgressNote') {
                $('#' + Clinical_ProgressNote.params.PanelID + ' #hfFacilityId').val(NotesFacilityId);
                Clinical_ProgressNote.params.NotesFacilityIDForFollowUp = NotesFacilityId;
                Clinical_ProgressNote.params.NotesFacilityName = NotesFacilityName;
                Clinical_ProgressNote.params.CurrentNotesFacilityId = NotesFacilityId;
            }
            $Ctrl_p = $('#' + Clinical_Notes.params.PanelID + ' #txtFacility');
            $hfCtrl_p = $('#' + Clinical_Notes.params.PanelID + ' #lblFacility');
            utility.SetKendoAutoCompleteSourceforValidate($Ctrl_p, $Ctrl_p.val(), $hfCtrl_p, $hfCtrl_p.val());

        } else {
            if ($('#' + Clinical_Notes.params.PanelID + ' #txtFacility').val() == '') {

                if (Clinical_Notes.params.PanelID != 'pnlClinicalProgressNote') {
                    $('#' + Clinical_Notes.params.PanelID + ' #lnkFacilityEdit').css("display", "none");
                    $('#' + Clinical_Notes.params.PanelID + ' #lblFacility').css("display", "inline");
                }
            }
        }
        if ((ResourceId != null && ResourceName != null) && (ResourceId != '' && ResourceName != '')) {
            //$('#' + Clinical_Notes.params.PanelID + ' #frmClinicalNotes #ResourceDiv').show();
            $('#' + Clinical_Notes.params.PanelID + ' #txtResource').val(ResourceName);
            $('#' + Clinical_Notes.params.PanelID + ' #hfResourceId').val(ResourceId);
            $('#' + Clinical_Notes.params.PanelID + ' #txtResourceProvider').val(ResourceproviderName);
            $('#' + Clinical_Notes.params.PanelID + ' #hfResourceProvider').val(ResourceproviderId);

            $Ctrl_p = $('#' + Clinical_Notes.params.PanelID + ' #txtResourceProvider');
            $hfCtrl_p = $('#' + Clinical_Notes.params.PanelID + ' #hfResourceProvider');
            //Resource Provider
            utility.SetKendoAutoCompleteSourceforValidate($Ctrl_p, $Ctrl_p.val(), $hfCtrl_p, $hfCtrl_p.val());

        } else {
            //$('#' + Clinical_Notes.params.PanelID + ' #frmClinicalNotes #ResourceDiv').hide();
        }

        if ((NotesFacilityId != null && NotesFacilityName != null) && (NotesFacilityId != '' && NotesFacilityName != '')) {
            $('#' + Clinical_Notes.params.PanelID + ' #txtFacility').val(NotesFacilityName);
            $('#' + Clinical_Notes.params.PanelID + ' #hfFacility').val(NotesFacilityId);
            if (Clinical_Notes.params.PanelID != 'pnlClinicalProgressNote') {
                $('#' + Clinical_Notes.params.PanelID + ' #lnkFacilityEdit').css("display", "inline");
                $('#' + Clinical_Notes.params.PanelID + ' #lblFacility').css("display", "none");
            }

            $Ctrl_p = $('#' + Clinical_Notes.params.PanelID + ' #txtFacility');
            $hfCtrl_p = $('#' + Clinical_Notes.params.PanelID + ' #hfFacility');
            //Facility
            utility.SetKendoAutoCompleteSourceforValidate($Ctrl_p, $Ctrl_p.val(), $hfCtrl_p, $hfCtrl_p.val());

        } else {
            if ($('#' + Clinical_Notes.params.PanelID + ' #txtFacility').val() == '') {
                if (Clinical_Notes.params.PanelID != 'pnlClinicalProgressNote') {
                    $('#' + Clinical_Notes.params.PanelID + ' #lnkFacilityEdit').css("display", "none");
                    $('#' + Clinical_Notes.params.PanelID + ' #lblFacility').css("display", "inline");
                }
            }
        }

        if ((RefProviderId != null && RefProviderName != null) && (RefProviderId != '' && RefProviderName != '')) {
            $('#' + Clinical_Notes.params.PanelID + ' #txtRefProvider').val(RefProviderName);
            $('#' + Clinical_Notes.params.PanelID + ' #hfRefProvider').val(RefProviderId);
            $('#' + Clinical_Notes.params.PanelID + ' #lnkRefProviderEdit').css("display", "inline");
            $('#' + Clinical_Notes.params.PanelID + ' #lblRefProvider').css("display", "none");
            $Ctrl_p = $('#' + Clinical_Notes.params.PanelID + ' #txtRefProvider');
            $hfCtrl_p = $('#' + Clinical_Notes.params.PanelID + ' #hfRefProvider');
            //RefProvider
            utility.SetKendoAutoCompleteSourceforValidate($Ctrl_p, $Ctrl_p.val(), $hfCtrl_p, $hfCtrl_p.val());
        } else {
            if ($('#' + Clinical_Notes.params.PanelID + ' #txtRefProvider').val() == '') {
                $('#' + Clinical_Notes.params.PanelID + ' #lnkRefProviderEdit').css("display", "none");
                $('#' + Clinical_Notes.params.PanelID + ' #lblRefProvider').css("display", "inline");
            }
        }
        //Getting User Rooms for selected facility of Patient
        if (NotesFacilityId && NotesFacilityId != '') {
            Clinical_Notes.GetRooms($('#' + Clinical_Notes.params.PanelID + ' #txtFacility'), NotesFacilityId);
        }
    },

    // Function to get visit information on basses of Patient & Visit
    VisitSearch: function (VisitId, patientID) {
        var dfd = $.Deferred();
        var myJSON = null;

        Encounter_Visits.SearchVisits(myJSON, patientID, VisitId).done(function (response) {

            if (response.status != false) {

                if (response.VisitsCount > 0) {

                    var VisitsLoadJSONData = JSON.parse(response.VisitsLoad_JSON)[0];
                    var AppointmentId = VisitsLoadJSONData.AppointmentId;
                    var NotesVisitId = VisitsLoadJSONData.VisitId;
                    var NotesVisitDate = null;
                    var NotesVisitTime = null;
                    var NotesProviderId = VisitsLoadJSONData.ProviderId;
                    var NotesProviderName = VisitsLoadJSONData.ProviderName;
                    var NotesFacilityName = VisitsLoadJSONData.FacilityName;
                    var NotesFacilityId = VisitsLoadJSONData.FacilityId;
                    var ScheduleReason = VisitsLoadJSONData.ReasonComments;
                    var NotesRoom = null;
                    var RefProviderId = VisitsLoadJSONData.RefProviderId;
                    var RefProviderName = VisitsLoadJSONData.ReferrringProviderName;

                    Clinical_Notes.BindVisitsValues(AppointmentId, NotesVisitId, NotesVisitDate, NotesVisitTime, NotesProviderId, NotesProviderName, NotesFacilityName, NotesFacilityId, ScheduleReason, NotesRoom, RefProviderId, RefProviderName, true);

                    $('#pnlClinicalNotes #frmClinicalNotes #RoomNo').val(VisitsLoadJSONData.RoomsId);
                    dfd.resolve();
                }
                else {
                    dfd.resolve();
                }
            }
            else {

                utility.DisplayMessages(response.Message, 3);
                dfd.resolve();
            }
        });
        return dfd;
    },

    //Binding Validation Functionk
    ValidateNotes: function (FrmCtrl) {

        $('#' + Clinical_Notes.params.PanelID + ' #frmClinicalNotes').bootstrapValidator('destroy');
        $('#' + FrmCtrl)
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  VisitDate: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  VisitTime: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  VisitReason: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  VisitTime: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  NoteType: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Provider: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Facility: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  }
              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           if (Clinical_Notes.params.triggerCount != 1) {
               if (Clinical_Notes.params.mode == "Add" && $('#' + FrmCtrl).find('#ChkBox_IsNonBilable').is(':checked')) {
                   utility.myConfirm('Are you sure you want to make the Visit as Non Billable?', function () {
                       Clinical_Notes.saveNote();
                   }, function () {
                       if (Clinical_Notes.CopyNotes) {
                           $('#' + Clinical_Notes.params["PanelID"] + ' #ChkBox_IsNonBilable').prop('checked', false);
                           Clinical_Notes.saveNote();
                       }
                       else {
                           Clinical_Notes.params.triggerCount = 0;
                           $('#' + Clinical_Notes.params["PanelID"] + ' #ChkBox_IsNonBilable').prop('checked', false);
                       }

                   },
                   'Confirm Non Billable');
               } else {
                   if (CheckPatientDemoMissingDetails() == false)
                       Clinical_Notes.saveNote();
                   else
                       $("li#clinicalMenuNotes a:first").trigger("click");
               }
               Clinical_Notes.params.triggerCount = 1;
           }
           else {
               Clinical_Notes.params.triggerCount = 0;
               $("li#clinicalMenuNotes a:first").trigger("click");
           }
       });
    },

    UpdateSoapText_VitalInNotes: function (NotesId, VitalSignsId) {

        Clinical_ProgressNote.FillNotes(null, NotesId).done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {

                var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);
                if (Clinical_Notes_detail.NoteText == null || Clinical_Notes_detail.NoteText == '') {

                    $('#' + Clinical_Vitals.params["PanelID"] + ' #ProgressnoteHTML').html('<h4 class="green hidden">Subjective</h4><ul class="initialVisit ui-sortable" id="SubjectiveNoteComponentList" style="min-height: 3px;"><li class="sopTextEditable defaultli ui-sortable-handle" style="min-height: 3px !important"></li></ul>'
                        + '<h4 class="green hidden">Objective</h4><ul class="initialVisit ui-sortable" id="ObjectiveNoteComponentList" style="min-height: 3px;"><li class="sopTextEditable defaultli ui-sortable-handle" style="min-height: 3px !important"></li></ul>'
                        + '<h4 class="green hidden">Assessment</h4><ul class="initialVisit ui-sortable" id="AssessmentNoteComponentList" style="min-height: 3px;"><li class="sopTextEditable defaultli ui-sortable-handle" style="min-height: 3px !important"></li></ul>'
                        + '<h4 class="green hidden">Plan</h4><ul class="initialVisit ui-sortable" id="PlanNoteComponentList" style="min-height: 3px;"><li class="sopTextEditable defaultli ui-sortable-handle" style="min-height: 3px !important"></li></ul>'
                        + '<ul class="initialVisit ui-sortable" id="MiscellaneousNoteComponentList" style="min-height: 3px;"><li class="sopTextEditable defaultli ui-sortable-handle" style="min-height: 3px !important"></li></ul>'
                        + '<ul class="initialVisit ui-sortable" id="ProgressNoteComponentList" style="min-height: 20px;"><li class="BillingInfoComponent initialVisitBody ui-sortable-handle hidden" NoteComponentId="NCDummyId" style="width: auto; right: auto; height: auto; bottom: auto;"><header><clinical_billinginfo title="eSuperbill" id="clinicalMenu_BillingInfo" class="NotesComponent"><a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.BillingInfo();" title="eSuperbill"> eSuperbill</a></clinical_billinginfo></header></li><li class="sopTextEditable defaultli ui-sortable-handle"></li></ul>');

                    $('#' + Clinical_Vitals.params["PanelID"] + ' #ProgressnoteHTML').html('');

                } else {
                    $('#' + Clinical_Vitals.params["PanelID"] + ' #ProgressnoteHTML').html(Clinical_Notes_detail.NoteText);
                }

                Clinical_Vitals.Get_Vitalsigns_ForSOAP(VitalSignsId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        if (response.VitalSignSoapCount > 0) {
                            Clinical_Notes.CreateVitalBodyHTML_VitalInNotes(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', NotesId);
                        }
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        });
    },

    CreateVitalBodyHTML_VitalInNotes: function (response, NoteHTMLCtrl, NotesId) {

        var VitalSignSoap_JSON = JSON.parse(response.VitalSignSoap_JSON);
        var $mainDivVital = $(document.createElement('div'));
        if (VitalSignSoap_JSON == null || VitalSignSoap_JSON.length == 0) {
            return "";
        }

        var VitalId = VitalSignSoap_JSON[0].VitalSignId;

        Clinical_Vitals.Get_Vitalsigns_ForSOAP(VitalId).done(function (responseVitals) {
            var vitalResponce = JSON.parse(responseVitals);

            $.each(VitalSignSoap_JSON, function (index, element) {

                var Vid = element.VitalSignId;
                var $SectionBodyVital = $(document.createElement('section'));
                $SectionBodyVital.attr('id', "Cli_Vitals_Main" + Vid);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_Vitals_" + Vid);
                var $ListVital = $(document.createElement('ul'));

                $ListVital.attr('class', 'list-unstyled');
                $SectionBodyVital.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Vitals_" + Vid + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Vitals_Main" + Vid + '"  ><i class="fa fa-times"></i></a></div> ');

                //$ListVital.append("<li>Vitals were taken by " + element.CreatedBy + " on " + utility.RemoveTimeFromDate(null, element.VitalSignDate) + (element.VitalSignTime == "" ? "" : " at " + element.VitalSignTime + ""));

                var vitalSoapText = Clinical_Vitals.FormateVitalSoapText(vitalResponce, element);
                $ListVital.append('<li>' + vitalSoapText + '</li>');

                $ListVital.append((element.Comments == "" ? "" : "<li>Comments: " + element.Comments + "</li>"));
                $DetailsDiv.append($ListVital);
                $SectionBodyVital.append($DetailsDiv);
                //if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML clinical_Vitals').parent().parent().find('#Cli_Vitals_Main' + Vid).length == 0) {
                if ($(NoteHTMLCtrl + ' clinical_Vitals').parent().parent().find('#Cli_Vitals_Main' + Vid).length == 0) {

                    $mainDivVital.append($SectionBodyVital);
                } else {
                    // VitalId = VitalId.split(',').length > 1 ? VitalId.split(',' + Vid).join('') : ''
                    VitalId = VitalId.split(',' + Vid).join('');
                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' clinical_Vitals').parent().parent().find('#Cli_Vitals_Main' + Vid + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' clinical_Vitals').parent().parent().find('#Cli_Vitals_Main' + Vid + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' clinical_Vitals').parent().parent().find('#Cli_Vitals_Main' + Vid).html($SectionBodyVital.html());
                    $(NoteHTMLCtrl + ' clinical_Vitals').parent().parent().find('#Cli_Vitals_Main' + Vid + ' ul').append(CommentHTML);;
                }
            });

            if ($mainDivVital.html() != '') {
                $(NoteHTMLCtrl + ' clinical_Vitals').parent().parent().find('section').remove();
                $(NoteHTMLCtrl + ' clinical_Vitals').parent().parent().append($mainDivVital.html());
            }
            Clinical_ProgressNote.saveComponentSOAPText('Vitals', false);
        });

        //Clinical_ProgressNote.updateProgressNoteHTML_DBCALL(null, Clinical_ProgressNote.params.NotesId == null ? NotesId : Clinical_ProgressNote.params.NotesId, $(NoteHTMLCtrl).html()).done(function (response) {
        //    response = JSON.parse(response);
        //    $(NoteHTMLCtrl).html('');
        //    if (response.status != false) {
        //        utility.DisplayMessages(response.Message, 1);
        //    }
        //    else {
        //        utility.DisplayMessages(response.Message, 3);
        //    }
        //});
    },

    saveNoteAddCase: function (IsToCheckForTodaysNote) {
        var self = $("#" + Clinical_Notes.params["PanelID"]);
        var myJSON = self.getMyJSONByName();

        if ($('#' + Clinical_Notes.params.PanelID + ' #OrderSet option:selected').text() != "- Select -") {
            Clinical_ProgressNote.DefaultOrderSetName = $('#' + Clinical_Notes.params.PanelID + ' #OrderSet option:selected').text();
            Clinical_ProgressNote.DefaultOrderSetID = $("#" + Clinical_Notes.params.PanelID + " #OrderSet").val();
            Clinical_ProgressNote.IsDefaultOrderSet = "1";
        }
        else {
            Clinical_ProgressNote.DefaultOrderSetName = "";
            Clinical_ProgressNote.DefaultOrderSetID = 0;
            Clinical_ProgressNote.IsDefaultOrderSet = "0";
        }
        if ($('#' + Clinical_Notes.params["PanelID"] + ' #chkPrevNoteROS').find('#chkPrevNoteROS').is(':checked')) {
            Clinical_ProgressNote.IsPreviousNoteROS = true;
        } else {
            Clinical_ProgressNote.IsPreviousNoteROS = false;
        }

        if ($('#' + Clinical_Notes.params["PanelID"] + ' #chkPrevNotePE').find('#chkPrevNotePE').is(':checked')) {
            Clinical_ProgressNote.IsPreviousNotePE = true;
        } else {
            Clinical_ProgressNote.IsPreviousNotePE = false;
        }

        if ($('#' + Clinical_Notes.params["PanelID"] + ' #chkPrevComplaints').find('#chkPrevComplaints').is(':checked')) {
            Clinical_ProgressNote.IsPreviousNoteComplaints = true;
        } else {
            Clinical_ProgressNote.IsPreviousNoteComplaints = false;
        }
        if ($('#' + Clinical_Notes.params["PanelID"] + ' #chkPrevNoteProblems').find('#chkPrevNoteProblems').is(':checked')) {
            Clinical_ProgressNote.IsPreviousNoteProblems = true;
        } else {
            Clinical_ProgressNote.IsPreviousNoteProblems = false;
        }
        Clinical_Notes.NotesSave(myJSON, Clinical_Notes.params.patientID, IsToCheckForTodaysNote).done(function (response) {
            response = JSON.parse(response);

            if (response.status != false) {

                Clinical_Notes.params["NotesId"] = response.NotesId;
                Clinical_Notes.params["ProviderId"] = $("#pnlClinicalNotes #hfProvider").val();

                if (response.MUAlertsCount && parseInt(response.MUAlertsCount) > 0)
                    utility.toggelMU3Alerts(true, parseInt(response.MUAlertsCount));
                Clinical_NotesSearch.SetNotesCount();
                var count = parseInt($('#spnNotesCount').text()) + 1;
                if ($('#spnNotesCount').text() == "0") {
                    $('#pnlDashboard div.wEncounter .badge').css("display", "none");
                    $('#spnNotesCount').css("display", "none");
                    $('#wpanel #widgetpanel .slick-track div').eq(24).find('span:last').hide();

                } else {

                    $('#pnlDashboard div.wEncounter .badge').css("display", "inline");
                    $('#spnNotesCount').css("display", "inline");
                    //$('#wpanel #widgetpanel .slick-track div').eq(24).find('span:last').show();
                    $('#pnlDashboard div.wEncounter .badge').text(count);
                    $('#spnNotesCount').text(count);

                    $('#wpanel .slick-track div').each(function (i) {

                        if ($(this).find('span:first').text() == 'Notes') {
                            $(this).find('span:last').text(count);
                            $(this).find('span:last').show();
                        }
                    });
                }

                Clinical_Notes.params["TemplateAddOrEdit"] = "1";
                Clinical_Notes.params["mode"] = "Edit";
                $('#' + Clinical_Notes.params.PanelID + ' #frmClinicalNotes').resetAllControls();
                $('#' + Clinical_Notes.params.PanelID + ' #frmClinicalNotes [type=hidden]').val('');
                $('#' + Clinical_Notes.params.PanelID + ' #ProgressnoteHTML').html('');
                params['IsFromCreateNote'] = true;

                if (Clinical_Notes.CopyNotes) {
                    Clinical_Notes.NewInsertTables = response.NewInsertTables;
                }
                Clinical_Notes.AddProgressNoteTab();
                utility.DisplayMessages(response.Message, 1);
            }
            else if (response.IsTodaysNoteCreated) {

                utility.myConfirm('A Note already exists for the selected Visit Date. Do you still want to create another Note?', function () {
                    Clinical_Notes.saveNoteAddCase(false);
                    Clinical_Notes.IsNoteAlreadyCreated = true;
                },
                function () {
                    SelectTab('clinicalTabNotes', 'false');
                    ClinicalMenuClick(event, function () {
                        ClinicalMenuSettings.selectClinicalMenu('clinicalMenuNotes');
                    }, null, null, 'clinicalTabNotes', 'button');
                }, 'Confirm Action');
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    // Add or Update Notes, this function is called from Notes And Progress Note
    saveNote: function ($dtpVisitDate, bResetSOAPText) {

        $('#frmClinicalProgressNote .splitterBody').removeClass('disableAll');
        if ($('#' + Clinical_Notes.params.PanelID + ' #frmClinicalNotes #ChkBox_LinkedAppointment').prop('checked') != true) {
            Clinical_ProgressNote.params.AppointmentVisitId = -1;
        }

        var self = $("#" + Clinical_Notes.params["PanelID"]);

        var visit_date = "";
        if ($dtpVisitDate)
            visit_date = $dtpVisitDate.val();
        else
            visit_date = $("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes").find("#dtpVisitDate").val();

        var current_date = $.datepicker.formatDate('mm/dd/yy', new Date());

        var objDeffered = $.Deferred();
        if (utility.DateCompare(visit_date, current_date) == 1) {
            if (Clinical_Notes.params.mode != "Edit") {
                utility.myConfirm("The note that you are creating is for a future date", function () {
                    objDeffered.resolve("ok");
                }, function () { }, "Confirmation Alert", 'Ok', null, true);
            }
            else
                objDeffered.resolve("ok");
        }
        else
            objDeffered.resolve("ok");

        objDeffered.done(function (message) {

            if (Clinical_Notes.params.mode == "Add") {

                Clinical_Notes.saveNoteAddCase(true);

                //End//22/12/2015//Ahmad Raza//Privileges Logic Implemented
            }
            else if (Clinical_Notes.params.mode == "Edit") {

                $('#pnlClinicalProgressNote #hfNoteText').val($('#pnlClinicalProgressNote #ProgressnoteHTML').html());
                var myJSON = self.getMyJSONByName();

                Clinical_Notes.NotesUpdate(myJSON, Clinical_Notes.params.NotesId).done(function (response) {

                    response = JSON.parse(response);
                    Clinical_Notes.params["TemplateAddOrEdit"] = "0";
                    Clinical_ProgressNote.params.triggerCount = 0;

                    if (response.status != false) {
                        Clinical_Notes.AddProgressNoteTab();
                        utility.DisplayMessages(response.Message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        });
    },

    //Setting Default Data of Logged in User and Getting List of notes in Grid
    LoadNoteGrid: function () {

        var dfd = new $.Deferred();
        if (Clinical_Notes.params.mode == "Add") {
            //EMR-651 fix change
            Clinical_Notes.fillPatientIinfo_DBCall(Clinical_Notes.params.patientID).done(function (response) {

                response = JSON.parse(response);
                if (response.status != false) {
                    var demographic_detail = JSON.parse(response.DemographicFill_JSON);

                    var DefaultFacilityName = globalAppdata['DefaultFacilityName'];
                    var DefaultFacilityId = globalAppdata['DefaultFacilityId'];

                    if (DefaultFacilityName == "- Select -" || !DefaultFacilityId) {
                        DefaultFacilityName = demographic_detail.Facility;
                        DefaultFacilityId = demographic_detail.FacilityID;
                    }

                    var DefaultProviderName = globalAppdata['DefaultProviderName'];
                    var DefaultProviderId = globalAppdata['DefaultProviderId'];

                    if (DefaultProviderName == "- Select -" || !DefaultProviderId) {
                        DefaultProviderName = demographic_detail.Provider;
                        DefaultProviderId = demographic_detail.ProviderID;
                    }

                    Clinical_Notes.BindVisitsValues(null, null, null, null, DefaultProviderId, DefaultProviderName, DefaultFacilityName,
                                   DefaultFacilityId, null, null, demographic_detail.RefProviderID, demographic_detail.RefProvider, true);


                    //if ($('#' + Clinical_Notes.params.PanelID + ' #ulTabs #liSignedNotes').hasClass('active')) {
                    Clinical_Notes.NotesSignedSearch(0, null, null, "Signed");
                    //} else {
                    Clinical_Notes.NotesDraftSearch(0, null, null, "Draft");
                    //}
                    //serialize Data after all controls loaded.                  
                    $('#' + Clinical_Notes.params.PanelID + ' #frmClinicalNotes').data('serialize', $('#frmClinicalNotes').serialize());
                    dfd.resolve('ok');
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                    dfd.resolve('ok');
                }
            });
        }
        else if (Clinical_Notes.params.mode == "Edit") {
            utility.DisplayMessages('Edit Mode is called on Notes, Please Report bug!', 3);
            dfd.resolve('ok');
        }

        return dfd.promise();
    },


    // On Dropdown Changes Template, Call Back Function to Load Template Type
    GetTemplates: function (obj) {
        var self = $('#' + Clinical_Notes.params.PanelID);
        if ($(obj).val() == "") {
            self.find('.NoteTemplate > select').val("");
        } else {
            if (self.find('.NoteTemplate > select').val() == "") {

                self.find('.NoteTemplate > select').attr('ddlist', 'GetTemplate');
                var data = "IsActive=&ID=" + $(obj).val();
                self.find('.NoteTemplate').loadDropDowns(true, data).done(function () {
                    // $("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes").bootstrapValidator('revalidateField', self.find('.NoteType > select').attr("name"));
                });
            }
        }
    },

    // On Dropdown Changes Template Type, Call Back Function to Load Template
    GetTemplatesType: function (obj) {
        var self = $('#' + Clinical_Notes.params.PanelID);
        if ($(obj).val() == "") {
            self.find('.NoteType > select').val("");
        } else {
            if (self.find('.NoteType > select').val() == "") {
                self.find('.NoteType > select').attr('ddlist', 'GetNoteTemplateType');
                //var data = "IsActive=&ID=" + $Clinical_Notes.params.NoteId + "&ID2=" + $(obj).val();
                var data = "IsActive=&ID=&ID2=" + $(obj).val();
                self.find('.NoteType').loadDropDowns(true, data).done(function () {
                    self.find('.NoteType > select').val(self.find('.NoteType > select option[value!=""]:first').val());
                    //$("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes").bootstrapValidator('revalidateField', self.find('.NoteType > select').attr("name"));
                });
            }
        }
    },

    // On Dropdown Changes Template, Call Back Function to Load Template Type
    GetNotesTemplates: function (ProviderId, NotcheckRevalidation) {

        var self = $('#' + Clinical_Notes.params.PanelID);
        if ($("#pnlClinicalNotes #txtProvider").val() == '' && ProviderId == null) {

            self.find('.NoteTemplate > select').val("");
            self.find('.NoteType > select').val("");
            self.find('.NoteTemplate').addClass('disableAll');
            self.find('.NoteType').addClass('disableAll');
            self.find('.OrderSet').addClass('disableAll');

        } else {

            if (ProviderId == null) {
                ProviderId = $("#pnlClinicalNotes #hfProvider").val();
            }

            if (ProviderId == "") {
                self.find('.NoteTemplate > select').val("");
                self.find('.NoteType > select').val("");
                self.find('.NoteTemplate').addClass('disableAll');
                self.find('.NoteType').addClass('disableAll');
                self.find('.OrderSet').addClass('disableAll');
            }
            else {

                //if (self.find('.NoteTemplate > select').val() == "") {

                self.find('.NoteTemplate > select').attr('ddlist', 'GetNoteTemplate');
                var NoteType = $("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes #NoteType").val();
                NoteType = NoteType == '' ? 1 : NoteType;
                var data = "IsActive=&ID=" + NoteType + "&ID2=" + ProviderId;

                self.find('.NoteTemplate').loadDropDowns(true, data).done(function () {
                    self.find('.NoteTemplate').removeClass('disableAll');
                    self.find('.NoteType').removeClass('disableAll');
                    //AST - 389 by:MAHMAD
                    if (self.find('#NoteTemplate').val() == "") {
                        if (!self.find('.OrderSet').hasClass('disableAll')) {
                            self.find('.OrderSet').addClass('disableAll')
                        }
                    }
                    else {
                        self.find('.OrderSet').removeClass('disableAll');
                    }
                    //AST - 389 by:MAHMAD
                    //Begin Edit by Fahad Malik 06-Dec-2016, Bug# PMS-885

                    var template = null;
                    if (globalAppdata.DefaultTemplate) {

                        self.find('.NoteTemplate option').filter(function () {
                            if (ProviderId != 'false') {
                                if ($.trim($(this).val()) == "") {
                                    return $.trim($(this).val()) == $("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes #NoteTemplate").find("option:contains('Blank')").val();
                                }
                                else {
                                    return $.trim($(this).val()) == globalAppdata.DefaultTemplate;
                                }

                            }
                        }).attr('selected', true).trigger("change");

                    } else {
                        self.find('.NoteTemplate option').filter(function () {

                            if (ProviderId != 'false') {
                                if ($.trim($(this).val()) == "") {
                                    return $.trim($(this).val()) == $("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes #NoteTemplate").find("option:contains('Blank')").val();
                                }
                                else {
                                    return $.trim($(this).val()) == $("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes #NoteTemplate").find("option:contains('Default Template')").val();
                                }
                            } else {

                            }

                        }).attr('selected', true).trigger("change");
                    }

                    //End Edit by Fahad Malik 06-Dec-2016, Bug# PMS-885
                    if (typeof NotcheckRevalidation == typeof undefined) {
                        if ($('#pnlClinicalNotes #frmClinicalNotes').data("bootstrapValidator") != null && typeof $('#frmClinicalNotes').data('bootstrapValidator') != 'undefined') {
                            $("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes").bootstrapValidator('revalidateField', self.find('.NoteType > select').attr("name"));
                        }
                    }
                });
                //}
            }
        }
    },

    TemplateDetail: function (ThisCTRL) {
        var tempId = $(ThisCTRL).attr("data-templateid");
        if (tempId != 0 && tempId > 0) {
            var params = [];
            params["TemplateId"] = tempId;
            LoadActionPan('NoteTemplatePreview', params);
        } else {
            $("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes #lnkNoteTemplateView").css("display", "none");
            $("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes #lblNoteTemplateView").css("display", "");
            $("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes #lnkNoteTemplateView").attr("data-templateid", 0);
            $("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes #lnkNoteTemplateView").attr("data-refvalue", 0);
        }
    },

    TemplateDetailFromDB: function (tempId) {
        var obj = new Object();
        obj.TemplateId = tempId;
        obj.PatientId = Clinical_Notes.params["patientID"];
        return MDVisionService.APIServiceComplex(obj, "ClinicalNotes", "NotesTemplateDataSelect");
    },


    GetNoteTemplateType: function (obj) {

        var self = $('#' + Clinical_Notes.params.PanelID);

        self.find('.NoteType > select').attr('ddlist', 'GetNoteTemplateType');
        var data = "IsActive=1&ID=" + $(obj).val();
        if ($(obj).val() != "" && $(obj).val() > 0) {
            $("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes #lblNoteTemplateView").css("display", "none");
            $("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes #lnkNoteTemplateView").attr("data-templateid", $(obj).val());
            $("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes #lnkNoteTemplateView").attr("data-refvalue", $('option:selected', obj).attr('refvalue'));
            $("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes #lnkNoteTemplateView").css("display", "");
        } else {
            $("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes #lnkNoteTemplateView").css("display", "none");
            $("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes #lblNoteTemplateView").css("display", "");
            $("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes #lnkNoteTemplateView").attr("data-templateid", 0);
            $("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes #lnkNoteTemplateView").attr("data-refvalue", 0);
        }

        self.find('.NoteType').loadDropDowns(true, data).done(function () {

            if ($(obj).val() == "") {
                self.find('.NoteType > select').val(self.find('.NoteType > select option[value!=""]:first').val());
                self.find('.OrderSet').addClass('disableAll');
            } else {
                self.find('.NoteType > select').val(self.find('.NoteType > select option[value!=""]:first').val());
                self.find('.OrderSet').removeClass('disableAll');
            }

            $("#" + Clinical_Notes.params["PanelID"] + " #frmClinicalNotes #NoteType option").each(function () {
                if ($(this).text().replace(' ', '').toLowerCase() == "phoneencounter") {
                    $(this).remove();
                }
            });
            if ($('#pnlClinicalNotes #frmClinicalNotes').data("bootstrapValidator") != null && typeof $('#frmClinicalNotes').data('bootstrapValidator') != 'undefined') {
                $("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes").bootstrapValidator('revalidateField', self.find('.NoteType > select').attr("name"));
            }
        });
        Clinical_Notes.GetOrderSetTemplate();

    },

    /*
        Purpose: this function get the rooms ddl based on facility selected
        Author: Muhammad Azhar Shahzad
        Created Date: March 24,2016
    */
    GetRooms: function (obj, FacilityId) {

        var def = $.Deferred();
        var self = $('#' + Clinical_Notes.params.PanelID);

        if ($('#' + Clinical_Notes.params.PanelID + ' #txtFacility').val() == '') {
            self.find('#divRoomNo.GetDDLRooms > select').val("");
            self.find('#divRoomNo.GetDDLRooms > select').val("");
            self.find('#divRoomNo.GetDDLRooms > select option[value!=""]').remove();
        } else {
            if (FacilityId == null) {
                FacilityId = $('#' + Clinical_Notes.params.PanelID + ' #hfFacility').val();
            }

            if (FacilityId == "") {
                self.find('#divRoomNo.GetDDLRooms > select').val("");
                self.find('#divRoomNo.GetDDLRooms > select').val("");
                self.find('#divRoomNo.GetDDLRooms > select option[value!=""]').remove();
                def.resolve();
            } else {

                self.find('#divRoomNo.GetDDLRooms > select').attr('ddlist', 'GetRooms');
                var data = "IsActive=&ID=" + FacilityId;
                self.find('#divRoomNo.GetDDLRooms').loadDropDowns(true, data).done(function () {
                    // Setting Default room to Lobby
                    var LobbyId = $('#' + Clinical_Notes.params.PanelID + ' #frmClinicalNotes #RoomNo option').filter(function () { return $(this).html() == "Lobby"; }).val();
                    if (LobbyId != null && LobbyId != '') {
                        $('#' + Clinical_Notes.params.PanelID + ' #frmClinicalNotes #RoomNo').val(LobbyId);
                    }
                    //$("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes").bootstrapValidator('revalidateField', self.find('.NoteType > select').attr("name"));
                });

                def.resolve();
            }
        }
    },

    //Load all autocompletes for this form (Ref Providers, Providers, Facility)
    LoadAllAutocomplete: function () {

        //to Load All Notes Components in NoteComponents[];
        CacheManager.BindCodes('GetNoteComponents', false);
        //to Load All Notes Sections in NoteSections[];
        CacheManager.BindCodes('GetNoteSections', false);
    },

    BindReferralProvider: function () {
        var Ctrl = $("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes #txtRefProvider");
        var hfCtrl = $("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes #hfRefProvider");
        var func = function () { return utility.GetRefProviderArray(Ctrl.val()) };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    BindFacility: function () {
        var Ctrl = $("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes #txtFacility");
        var hfCtrl = $("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes #hfFacility");
        var func = function () { return utility.GetFacilityArray(Ctrl.val()) };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    BindResourceProvider: function () {
        var Ctrl = $("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes #txtResourceProvider");
        var hfCtrl = $("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes #hfResourceProvider");
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    BindProvider: function () {
        var Ctrl = $("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes #txtProvider");
        var hfCtrl = $("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes #hfProvider");
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        var onSelect = function (e) {
            $("#" + Clinical_Notes.params.PanelID + " #hfProvider").val(e.id);
        };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect);        
    },







    BindBlockReason: function () {

        var shortName = $("#frmClinicalNotes input#txtVisitReason").val();
        Clinical_Notes.GetBlockHours(shortName).done(function (response) {

            $("#frmClinicalNotes input#txtVisitReason").autocomplete({
                autoFocus: true,
                source: response,
                select: function (event, ui) {
                    setTimeout(function () {

                        $("#pnlClinicalNotes #hfVisitReason").val(ui.item.id); // add the selected id

                    }, 100);
                }
            });

            $("#frmClinicalNotes input#txtVisitReason").autocomplete("search");
        });
    },


    GetBlockHours: function (name, IsGetAll) {

        var AllBlockReasons = [];
        var dfd = new $.Deferred();

        Clinical_Notes.LoadBlockhoursDBCall(name).done(function (responseData) {

            responseData = JSON.parse(responseData);
            if (responseData.status != false) {
                if (responseData.ResonsCount > 0) {
                    var Resons = JSON.parse(responseData.ResonsLoad_JSON);
                    $.each(Resons, function (i, item) {

                        AllBlockReasons.push({ id: item.ScheduleReasonId, value: item.ShortName });
                    });
                }
            }

            dfd.resolve(AllBlockReasons);
        });

        return dfd.promise();
    },

    LoadBlockhoursDBCall: function (name) {
        var objData = {};
        objData["ShortName"] = name;
        objData["commandType"] = "REASONS_LOOKUP_AUTOCOMPLETE";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    // Function To add Progress Note and Notes Tab in Clinical Note Menu
    AddProgressNoteTab: function (IsFromCreateNote) {

        if ($("#pnlTab3 .full-tab-row #mstrDivNotes").length == 0) {
            $("#pnlTab3 .full-tab-row").append('<div class="tab-pane" id="mstrDivNotes"></div>');
        }

        var clinicalTabActiveClass = "";

        var clinicalTabid = "clinicalTabNotes";
        var clinicalTabtext = "Notes";
        var clinicalTab = "clinicalTabNotes";

        var clinicalTabElement = $('<span class="btn btn-default btn-sm tab_space"><button type="button" class="btn btn-default btn-sm tab_space' + clinicalTabActiveClass + '" title="' + clinicalTabtext
        + '" id="' + clinicalTabid + '" onclick=SelectTab("' + clinicalTab + '","false");ClinicalMenuClick(event,null,null,null,\'' + clinicalTabid + '\',\'' + 'button' + '\');>' + clinicalTabtext + '</button></span>');

        if ($("div#mstrDivNotes").find("button#clinicalTabNotes").length < 1) {
            $("div#mstrDivNotes").find("span").removeClass('tab_selected');
            $("div#mstrDivNotes").find("button").removeClass("active");
            $("div#mstrDivNotes").append(clinicalTabElement);

        }

        if (IsFromCreateNote != null && IsFromCreateNote == true) {
            $("div#mstrDivNotes").find('span:has(> button#clinicalTabProgressNote)').remove();
        }
        else {

            var ProgressNoteActiveClass = " active";
            var ProgressNoteTabid = "clinicalTabProgressNote";
            var ProgressNotetext = "Progress Note";
            var ProgressNoteTab = "clinicalTabProgressNote";

            var ProgressNoteTabElement = $('<span class="btn btn-default btn-sm tab_space tab_selected"><button type="button" class="btn btn-default btn-sm tab_space' + '" title="' + ProgressNotetext
                        + '" id="' + ProgressNoteTabid + '" onclick=SelectTab("' + ProgressNoteTab + '","false");ClinicalMenuClick(event,null,null,null,\'' + ProgressNoteTabid + '\',\'' + 'button' + '\');>' + ProgressNotetext + '</button></span>');

            if ($("div#mstrDivNotes").find("button#clinicalTabProgressNote").length < 1) {
                $("div#mstrDivNotes").find("span").removeClass('tab_selected');
                $("div#mstrDivNotes").find("button").removeClass("active");
                $("div#mstrDivNotes").append(ProgressNoteTabElement);
            }

            Clinical_Notes.params["IsPhoneEncounter"] = false;
            if (GetSelectedTab("mstrTabClinical").ContainerControlID != 'Clinical_ProgressNote') {
                Clinical_Notes.params.triggerCount = 0;
                $("#clinicalTabProgressNote").trigger("click");
            }
            else {
                Clinical_ProgressNote.DisableFields(true);
            }

            //Start Farooq Ahmad 20/07/2016 EMR-60
            $("#ClinicalUL").find('li:not(#clinicalMenuNotes)').removeClass("active nav-active nav-expanded");
            //End Farooq Ahmad 20/07/2016 EMR-60
        }

        if ($("div#mstrDivNotes").css("display") == "none") {
            $("div#mstrDivNotes").css("display", "inline");
        }
    },

    //This Function is used to Get Notes information from db and Load that information to Grid and create pagination
    //This Function is called by    NotesActiveInactive, NotesDelete, ActiveInactiveNotes, SignNotes, LoadNoteGrid Functions of Clinical_notes.js
    NotesSearch: function (NotesId, PageNo, rpp, NoteStatus) {
        //Start//22/12/2015//Ahmad Raza//Privileges Logic Implemented

        if ($("#pnlClinicalNotes #pnlNotes_Result").css("display") == "none") {
            $("#pnlClinicalNotes #pnlNotes_Result").show();
        }

        var self = $("#pnlClinicalNotes");

        Clinical_Notes.SearchNotes(NotesId, PageNo, rpp, NoteStatus).done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {

                if (response.NotesLoad_JSON != "") {
                    Clinical_Notes.NotesDraftGridLoad(response, PageNo, rpp);
                    Clinical_Notes.NotesSignedGridLoad(response, PageNo, rpp);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    NotesDraftSearch: function (NotesId, PageNo, rpp, NoteStatus) {
        //Start//22/12/2015//Ahmad Raza//Privileges Logic Implemented
        NoteStatus = 'Draft';
        if ($("#pnlClinicalNotes #pnlNotes_Result").css("display") == "none") {
            $("#pnlClinicalNotes #pnlNotes_Result").show();
        }

        var self = $("#pnlClinicalNotes");

        Clinical_Notes.SearchNotes(NotesId, PageNo, rpp, NoteStatus).done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_Notes.totalDraftNotesCount = 0;

                if (response.NotesLoad_JSON != "") {
                    $("#pnlClinicalNotes #draftNotesCount").text(response.iTotalDraftDisplayRecords);
                    Clinical_Notes.NotesDraftGridLoad(response, PageNo, rpp);
                    var TableControl = "pnlClinicalNotes #dgvClinicalDraftNotes";
                    var PagingPanelControlID = "pnlClinicalNotes #divClinicalDraftNotesPaging";
                    var ClassControlName = "Clinical_Notes";
                    var PagesToDisplay = 5;
                    var iTotalDraftDisplayRecords = response.iTotalDraftDisplayRecords;
                    if (!$.fn.DataTable.isDataTable('#' + TableControl)) {
                        setTimeout(CreatePagination(response.iTotalDraftDisplayRecords, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDraftDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage, NoteStatus) {
                            Clinical_Notes.NotesDraftSearch(PrimaryID, PageNumber, ResultPerPage, NoteStatus);
                        }), 10);
                    }
                    Clinical_Notes.totalDraftNotesCount = parseInt(response.iTotalDraftDisplayRecords);
                    Clinical_Notes.DashBoardDefaultCheckBoxes();
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    NotesSignedSearch: function (NotesId, PageNo, rpp, NoteStatus) {

        NoteStatus = 'Signed';
        if ($("#pnlClinicalNotes #pnlNotes_Result").css("display") == "none") {
            $("#pnlClinicalNotes #pnlNotes_Result").show();
        }

        Clinical_Notes.SearchNotes(NotesId, PageNo, rpp, NoteStatus).done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {
                totalSignedNotesCount = 0;
                if (response.NotesLoad_JSON != "") {
                    $("#pnlClinicalNotes #signedNotesCount").text(response.iTotalSignedDisplayRecords);
                    Clinical_Notes.NotesSignedGridLoad(response, PageNo, rpp);

                    var TableControl = "pnlClinicalNotes #dgvClinicalSignedNotes";
                    var PagingPaneldfgControlID = "pnlClinicalNotes #divClinicalSignedNotesPaging";
                    var ClassControlName = "Clinical_Notes";
                    var PagesToDisplay = 5;
                    var iTotalSignedDisplayRecords = response.iTotalSignedDisplayRecords;
                    if (!$.fn.DataTable.isDataTable('#' + TableControl)) {
                        setTimeout(CreatePagination(response.iTotalSignedDisplayRecords, PageNo, rpp, PagingPaneldfgControlID, TableControl, ClassControlName, PagesToDisplay, iTotalSignedDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage, NoteStatus) {
                            Clinical_Notes.NotesSignedSearch(PrimaryID, PageNumber, ResultPerPage, NoteStatus);
                        }), 10);
                    }
                    Clinical_Notes.totalSignedNotesCount = parseInt(response.iTotalSignedDisplayRecords);
                    Clinical_Notes.DashBoardDefaultCheckBoxes();
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    //NotesGridLoad
    NotesEdit: function (NotesId, mode, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }

        //Privileges Logic Implemented
        AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {

            if (strMessage == "") {

                params["NotesId"] = NotesId;
                params["PatientId"] = $("#" + Clinical_Notes.params["PanelID"] + " #hfPatientId").val();
                params["IsPhoneEncounter"] = false;
                params["mode"] = mode;
                params["ParentCtrl"] = Clinical_Notes.params["TabID"];
                params["VisitType"] = $("#" + Clinical_Notes.params["PanelID"] + "#PatientVisitType").val();
                if (mode == "Edit") {

                    Clinical_Notes.params["TemplateAddOrEdit"] = "0";
                    Clinical_Notes.AddProgressNoteTab();
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    //NotesGridLoad
    NotesDelete: function (NotesId, event, NoteStatus, PatientId) {

        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }

        utility.myConfirm('1', function () {

            $('body').removeClass('modal-open');
            var selectedValue = NotesId;
            var IsValue = (selectedValue == "" || selectedValue == "undefined");

            if (!IsValue) {
                Clinical_Notes.NotesDeleted(selectedValue).done(function (response) {
                    response = JSON.parse(response);

                    if (response.status != false) {
                        if ($("#PatientProfile #hfPatientId").val() == PatientId) {
                            setPatientBanner(PatientId);
                        }
                        var count = parseInt($('#spnNotesCount').text()) - 1;
                        if ($('#spnNotesCount').text() == "0") {

                            $('#pnlDashboard div.wEncounter .badge').css("display", "none");
                            $('#spnNotesCount').css("display", "none");

                        }
                        else {

                            $('#pnlDashboard div.wEncounter .badge').css("display", "inline");
                            $('#spnNotesCount').css("display", "inline");
                            $('#pnlDashboard div.wEncounter .badge').text(count);
                            $('#spnNotesCount').text(count);

                            $('#wpanel .slick-track div').each(function (i) {
                                if ($(this).find('span:first').text() == 'Notes') {
                                    $(this).find('span:last').text(count);
                                    $(this).find('span:last').show();
                                }
                            });
                        }

                        if (NoteStatus == 'Draft')
                            Clinical_Notes.NotesDraftSearch(0, null, null, 'Draft');

                        //Signed note can't be deleted
                        //if (NoteStatus == 'Signed')
                        //    Clinical_Notes.NotesSignedSearch(0, null, null, 'Signed');

                        utility.DisplayMessages(response.Message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { },
            '1'
        );
    },

    NoteGridTabChange: function (NoteStatus) {
        if (NoteStatus == 'Draft')
            Clinical_Notes.NotesDraftSearch(0, null, null, 'Draft');
        else if (NoteStatus == 'Signed')
            Clinical_Notes.NotesSignedSearch(0, null, null, 'Signed');
    },
    //NotesSearch
    NotesGridLoad: function (response) {

        $("#pnlClinicalNotes #dgvClinicalNotes").dataTable().fnDestroy();
        $("#pnlClinicalNotes #pnlNotes_Result #dgvClinicalNotes tbody").find("tr").remove();
        Clinical_Notes.canSignNote().done(function (canSign) {
            if (response.ClinicalNotesCount > 0) {

                //EMR-602 fix
                $('#pnlClinicalNotes #PreviousNotediv').removeClass('disableAll')
                var NotesLoadJSONData = JSON.parse(response.NotesLoad_JSON);
                var HasDeleteRights = response.HasDeleteRights;
                $.each(NotesLoadJSONData, function (i, item) {

                    var $row = $('<tr/>');
                    $row.attr("onclick", "utility.SelectGridRow($('#gvNotes_row" + item.NotesId + "'))");
                    $row.attr("id", "gvNotes_row" + item.NotesId);
                    $row.attr("NotesId", item.NotesId);
                    $row.attr("VisitDate", utility.RemoveTimeFromDate(null, item.VisitDate));
                    $row.attr("VisitTime", item.VisitTime);
                    $row.attr("NoteType", item.NoteType);
                    $row.attr("CC", item.ChiefComplaint);
                    $row.attr("Status", item.EntityId);
                    $row.attr("Provider", item.ProviderName);
                    $row.attr("SignedBy", item.SignedBy);
                    $row.attr("Facility", item.FacilityName);
                    $row.attr("Room", item.RoomNo);
                    $row.attr("Comments", item.Comments);
                    $row.attr("Active", item.IsActive);

                    if (item.IsActive == "True") {
                        isactive = 0;
                        activeTitle = "Active Record";
                        tglclass = "fa fa-toggle-on green";
                    }
                    else {
                        isactive = 1;
                        activeTitle = "Inactive Record";
                        tglclass = "fa fa-toggle-on red";
                    }

                    var Isdisabled = "disabled =true";
                    if (item.NoteStatus == "Draft" || item.NoteStatus == "") {
                        if (HasDeleteRights != "No") { // Do not show delete button if user does not have the rights
                            Isdisabled = "";
                        }
                    }

                    //on row click, edit the record, requirement from dr Hajjar
                    if (Isdisabled == '') {
                        $row.attr("onclick", "Clinical_Notes.NotesRowEdit(" + item.NotesId + ",'Edit'" + ", event);");
                    }

                    var NotesPreview = "Clinical_Notes.NotesPreview('" + item.NotesId + "',null,'" + item.PatientId + "','" + item.ProviderId + "','" + utility.RemoveTimeFromDate(null, item.VisitDate) + "','" + item.BillingInfoId + "','" + item.AppointmentDate + "'," + item.VisitId + ",'" + item.NoteDate + "','" + item.PatientTypeId + "', '" + item.FacilityId + "', '" + item.POS + "'," + (!item.RefProviderId ? null : item.RefProviderId) + ",'" + utility.RemoveTimeFromDate(null, item.CreatedOn) + "'," + (item.IsPhoneEncounter == "0" ? false : true) + ",'" + item.BillingStatus + "');";
                    if (canSign) {
                        var NoteSignMethod = "Clinical_Notes.SignNotes(" + item.NotesId + ",event,'" + item.NoteStatus + "',false,'" + utility.RemoveTimeFromDate(null, item.VisitDate) + "','" + item.PatientId + "', '" + item.ProviderId + "','" + item.BillingInfoId + "','" + item.AppointmentDate + "'," + item.VisitId + ",'" + item.NoteDate + "','" + item.PatientTypeId + "', '" + item.FacilityId + "', '" + item.POS + "'," + (!item.RefProviderId ? null : item.RefProviderId) + "," + (item.IsPhoneEncounter == "0" ? false : true) + ");";
                        $row.append('<td style="display:none;">' + item.NotesId + '</td><td><a ' + Isdisabled + ' class="btn  btn-xs" href="#" onclick="Clinical_Notes.NotesDelete(' + item.NotesId + ',event,\'' + item.NoteStatus + '\',\'' + item.PatientId + '\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a ' + Isdisabled + ' class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_Notes.NotesEdit(' + item.NotesId + ",'Edit'" + ', event,\'' + item.NoteStatus + '\');"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a title="View Note" class="btn  btn-xs" href="#" onclick="' + NotesPreview + '"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a id="btnSignNote" class="btn btn-xs " href="javascript:void(0)" title="Sign Note" onclick="' + NoteSignMethod + '" ' + Isdisabled + '> <i class="fa fa-calculator black"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Audit Log" onclick="Clinical_Notes.rowHistory(' + item.NotesId + ',\'' + utility.RemoveTimeFromDate(null, item.VisitDate) + '\',\'' + item.VisitReasonComments + '\',\'' + item.NoteTempType + '\',event);"> <i class="fa fa-history blue"></i></a></td><td>' + utility.RemoveTimeFromDate(null, item.VisitDate) + '</td><td>' + item.VisitTime + '</td><td>' + item.VisitType + '</td><td>' + item.TemplateTypeName + '</td>' + '</td><td>' + item.NoteStatus + '</td>' + '</td><td>' + item.ProviderName + '</td>' + '</td><td>' + item.FacilityName + '</td><td>' + item.RoomName + '</td><td style="display:none;">' + item.Comments + '</td><td>' + item.VisitReasonComments + '</td>');
                    }
                    else {
                        $row.append('<td style="display:none;">' + item.NotesId + '</td><td><a ' + Isdisabled + ' class="btn  btn-xs" href="#" onclick="Clinical_Notes.NotesDelete(' + item.NotesId + ',event,\'' + item.NoteStatus + '\',\'' + item.PatientId + '\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a ' + Isdisabled + ' class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_Notes.NotesEdit(' + item.NotesId + ",'Edit'" + ', event,\'' + item.NoteStatus + '\');"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a title="View Note" class="btn  btn-xs" href="#" onclick="' + NotesPreview + '"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Audit Log" onclick="Clinical_Notes.rowHistory(' + item.NotesId + ',\'' + utility.RemoveTimeFromDate(null, item.VisitDate) + '\',\'' + item.VisitReasonComments + '\',\'' + item.NoteTempType + '\',event);"> <i class="fa fa-history blue"></i></a></td><td>' + utility.RemoveTimeFromDate(null, item.VisitDate) + '</td><td>' + item.VisitTime + '</td><td>' + item.VisitType + '</td><td>' + item.TemplateTypeName + '</td>' + '</td><td>' + item.NoteStatus + '</td>' + '</td><td>' + item.ProviderName + '</td>' + '</td><td>' + item.FacilityName + '</td><td>' + item.RoomName + '</td><td style="display:none;">' + item.Comments + '</td><td>' + item.VisitReasonComments + '</td>');
                    }
                    $("#pnlNotes_Result #dgvClinicalNotes tbody").last().append($row);
                });
            }
            else {
                //EMR-602 fix
                $('#pnlClinicalNotes #PreviousNotediv').addClass('disableAll');
                if (!$.fn.DataTable.isDataTable('#pnlClinicalNotes #dgvClinicalNotes')) {
                    $('#pnlClinicalNotes #dgvClinicalNotes').DataTable({
                        "language": {
                            "emptyTable": "No Note is Found"
                        }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                    });
                }
            }

            var IsDataTable = $.fn.dataTable.isDataTable('#pnlClinicalNotes #dgvClinicalNotes');

            if (!IsDataTable) {
                // to remove records per page dropdown
                $("#pnlClinicalNotes #pnlNotes_Result #dgvClinicalNotes").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] });
            }
        });
    },

    //Draft Notes
    NotesDraftGridLoad: function (response, PageNo, rpp) {

        $("#pnlClinicalNotes #dgvClinicalDraftNotes").dataTable().fnDestroy();
        $("#pnlClinicalNotes #pnlNotes_Result #dgvClinicalDraftNotes tbody").find("tr").remove();

        Clinical_Notes.canSignNote().done(function (canSign) {
            if (response.ClinicalNotesCount > 0) {

                //EMR-602 fix
                $('#pnlClinicalNotes #PreviousNotediv').removeClass('disableAll')
                var NotesLoadJSONData = JSON.parse(response.NotesLoad_JSON);

                $.each(NotesLoadJSONData, function (i, item) {
                    if (item.NoteStatus == "Draft") {

                        var $row = $('<tr/>');
                        $row.attr("onclick", "utility.SelectGridRow($('#gvDraftNotes_row" + item.NotesId + "'))");
                        $row.attr("id", "gvDraftNotes_row" + item.NotesId);
                        $row.attr("NotesId", item.NotesId);
                        $row.attr("VisitDate", utility.RemoveTimeFromDate(null, item.VisitDate));
                        $row.attr("VisitTime", item.VisitTime);
                        $row.attr("NoteType", item.NoteTempType);
                        $row.attr("CC", item.ChiefComplaint);
                        $row.attr("Status", item.EntityId);
                        $row.attr("Provider", item.ProviderName);
                        $row.attr("SignedBy", item.SignedBy);
                        $row.attr("Facility", item.FacilityName);
                        $row.attr("Room", item.RoomNo);
                        $row.attr("Comments", item.Comments);
                        $row.attr("Active", item.IsActive);
                        $row.attr("IsUnSigned", item.UnSignedStatus);

                        if (item.IsActive == "True") {
                            isactive = 0;
                            activeTitle = "Active Record";
                            tglclass = "fa fa-toggle-on green";
                        }
                        else {
                            isactive = 1;
                            activeTitle = "Inactive Record";
                            tglclass = "fa fa-toggle-on red";
                        }

                        var Isdisabled = "disabled =true";
                        if (item.NoteStatus == "Draft" || item.NoteStatus == "") {
                            Isdisabled = "";
                        }
                        var isVisible = 'style="display:none;';
                        if (response.HasDeleteRights != "No") {
                            isVisible = "";
                        }
                        //on row click, edit the record, requirement from dr Hajjar
                        if (Isdisabled == '') {
                            $row.attr("onclick", "Clinical_Notes.NotesRowEdit(" + item.NotesId + ",'Edit'" + ", event);");
                        }

                        var NotesPreview = "Clinical_Notes.NotesPreview('" + item.NotesId + "',null,'" + item.PatientId + "','" + item.ProviderId + "','" + utility.RemoveTimeFromDate(null, item.VisitDate) + "','" + item.BillingInfoId + "','" + item.AppointmentDate + "'," + item.VisitId + ",'" + item.NoteDate + "','" + item.PatientTypeId + "', '" + item.FacilityId + "', '" + item.POS + "'," + (!item.RefProviderId ? null : item.RefProviderId) + ",'" + utility.RemoveTimeFromDate(null, item.CreatedOn) + "'," + (item.IsPhoneEncounter == "0" ? false : true) + ",'" + item.BillingStatus + "');";
                        if (canSign) {
                            var NoteSignMethod = "Clinical_Notes.SignNotes(" + item.NotesId + ",event,'" + item.NoteStatus + "',false,'" + utility.RemoveTimeFromDate(null, item.VisitDate) + "','" + item.PatientId + "', '" + item.ProviderId + "','" + item.BillingInfoId + "','" + item.AppointmentDate + "'," + item.VisitId + ",'" + item.NoteDate + "','" + item.PatientTypeId + "', '" + item.FacilityId + "', '" + item.POS + "'," + (!item.RefProviderId ? null : item.RefProviderId) + "," + (item.IsPhoneEncounter == "0" ? false : true) + ");";

                            $row.append('<td style="display:none;">' + item.NotesId + '</td><td><a ' + Isdisabled + ' ' + isVisible + ' class="btn  btn-xs" href="#" onclick="Clinical_Notes.NotesDelete(' + item.NotesId + ',event,\'' + item.NoteStatus + '\',\'' + item.PatientId + '\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a ' + Isdisabled + ' class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_Notes.NotesEdit(' + item.NotesId + ",'Edit'" + ', event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a title="View Note" class="btn  btn-xs" href="#" onclick="' + NotesPreview + '" ' + Isdisabled + '> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a id="btnSignDraftNote" class="btn btn-xs " href="javascript:void(0)" title="Sign Note" onclick="' + NoteSignMethod + '" > <i class="fa fa-calculator black"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Audit Log" onclick="Clinical_Notes.rowHistory(' + item.NotesId + ',\'' + utility.RemoveTimeFromDate(null, item.VisitDate) + '\',\'' + item.VisitReasonComments + '\',\'' + item.NoteTempType + '\',event);"> <i class="fa fa-history blue"></i></a>&nbsp;<a data-toggle="tooltip" class="btn btn-xs " href="javascript:void(0)" data-placement="right" onclick="Clinical_Notes.GetNoteInfo(' + item.NotesId + ',event,\'' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '\');" title="Copy Note"><i class="fa fa-clipboard blue"></i></a></td><td>' + utility.RemoveTimeFromDate(null, item.VisitDate) + '</td><td>' + item.VisitTime + '</td><td>' + item.VisitType + '</td><td>' + item.TemplateTypeName + '</td>' + '</td><td>' + item.ProviderName + '</td>' + '</td><td>' + item.CreatedBy + '</td><td>' + item.FacilityName + '</td><td>' + item.RoomName + '</td><td style="display:none;">' + item.Comments + '</td><td>' + item.VisitReasonComments + '</td>');
                        }
                        else {
                            $row.append('<td style="display:none;">' + item.NotesId + '</td><td><a ' + Isdisabled + ' ' + isVisible + ' class="btn  btn-xs" href="#" onclick="Clinical_Notes.NotesDelete(' + item.NotesId + ',event,\'' + item.NoteStatus + '\',\'' + item.PatientId + '\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a ' + Isdisabled + ' class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_Notes.NotesEdit(' + item.NotesId + ",'Edit'" + ', event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a title="View Note" class="btn  btn-xs" href="#" onclick="' + NotesPreview + '"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Audit Log" onclick="Clinical_Notes.rowHistory(' + item.NotesId + ',\'' + utility.RemoveTimeFromDate(null, item.VisitDate) + '\',\'' + item.VisitReasonComments + '\',\'' + item.NoteTempType + '\',event);"> <i class="fa fa-history blue"></i></a>&nbsp;<a data-toggle="tooltip" class="btn btn-xs " href="javascript:void(0)" data-placement="right" onclick="Clinical_Notes.GetNoteInfo(' + item.NotesId + ',event,\'' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '\');" title="Copy Note"><i class="fa fa-clipboard blue"></i></a></td><td>' + utility.RemoveTimeFromDate(null, item.VisitDate) + '</td><td>' + item.VisitTime + '</td><td>' + item.VisitType + '</td><td>' + item.TemplateTypeName + '</td>' + '</td><td>' + item.ProviderName + '</td>' + '</td><td>' + item.CreatedBy + '</td><td>' + item.FacilityName + '</td><td>' + item.RoomName + '</td><td style="display:none;">' + item.Comments + '</td><td>' + item.VisitReasonComments + '</td>');
                        }
                        $("#pnlNotes_Result #dgvClinicalDraftNotes tbody").last().append($row);
                    }
                });

                var draftNotesRows = $("#pnlClinicalNotes div#pnlNotes_Result #dgvClinicalDraftNotes tbody").find("tr");

                if (draftNotesRows.length < 1) {
                    if (!$.fn.DataTable.isDataTable("#pnlClinicalNotes div#pnlNotes_Result #dgvClinicalDraftNotes")) {
                        $("#pnlClinicalNotes div#pnlNotes_Result #dgvClinicalDraftNotes").DataTable({
                            "language": {
                                "emptyTable": "No Draft Note is Found"
                            }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }, { "targets": [7], "visible": false }]
                        });
                    }
                    $("#pnlClinicalNotes #divClinicalDraftNotesPaging").css("display", "none");
                }
            }
            else {
                $("#pnlClinicalNotes #divClinicalDraftNotesPaging").css("display", "none");
                //EMR-602 fix
                $('#pnlClinicalNotes #PreviousNotediv').addClass('disableAll')
                if (!$.fn.dataTable.isDataTable('#pnlClinicalNotes #dgvClinicalDraftNotes')) {
                    $('#pnlClinicalNotes #dgvClinicalDraftNotes').DataTable({
                        "language": {
                            "emptyTable": "No Draft Note is Found"
                        }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                    });
                }

            }

            var IsDataTable = $.fn.dataTable.isDataTable('#pnlClinicalNotes #dgvClinicalDraftNotes')
            if (!IsDataTable) {
                $("#pnlClinicalNotes #pnlNotes_Result #dgvClinicalDraftNotes").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
            }

        });
    },
    //SignedNotes
    NotesSignedGridLoad: function (response, PageNo, rpp) {
        var IsNoteUnSign = false;
        Clinical_Notes.params.HasUnSignPermission = false;
        $("#pnlClinicalNotes #dgvClinicalSignedNotes").dataTable().fnDestroy();
        $("#pnlClinicalNotes #pnlNotes_Result #dgvClinicalSignedNotes tbody").find("tr").remove();

        $.when(IsUserHaveUnSignRights = EMRUtility.IsNoteUnSign(globalAppdata["AppUserId"])).then(function () {
            if (IsUserHaveUnSignRights.response == true) {
                IsNoteUnSign = true;
                Clinical_Notes.params.HasUnSignPermission = true;
            }
            Clinical_Notes.canSignNote().done(function (canSign) {
                if (response.ClinicalNotesCount > 0) {

                    //EMR-602 fix
                    $('#pnlClinicalNotes #PreviousNotediv').removeClass('disableAll')
                    var NotesLoadJSONData = JSON.parse(response.NotesLoad_JSON);

                    $.each(NotesLoadJSONData, function (i, item) {
                        if (item.NoteStatus == "Signed") {

                            var $row = $('<tr/>');
                            $row.attr("id", "gvSignedNotes_row" + item.NotesId);
                            $row.attr("NotesId", item.NotesId);
                            $row.attr("VisitDate", utility.RemoveTimeFromDate(null, item.VisitDate));
                            $row.attr("VisitTime", item.VisitTime);
                            $row.attr("NoteType", item.NoteTempType);
                            $row.attr("CC", item.ChiefComplaint);
                            $row.attr("Status", item.EntityId);
                            $row.attr("Provider", item.ProviderName);
                            $row.attr("SignedBy", item.SignedBy);
                            $row.attr("Facility", item.FacilityName);
                            $row.attr("Room", item.RoomNo);
                            $row.attr("Comments", item.Comments);
                            $row.attr("Active", item.IsActive);
                            $row.attr("IsUnSigned", item.UnSignedStatus);

                            if (item.IsActive == "True") {
                                isactive = 0;
                                activeTitle = "Active Record";
                                tglclass = "fa fa-toggle-on green";
                            }
                            else {
                                isactive = 1;
                                activeTitle = "Inactive Record";
                                tglclass = "fa fa-toggle-on red";
                            }
                            var isVisible = 'style="display:none;';
                            var Isdisabled = "disabled =true";
                            if (item.NoteStatus == "Draft" || item.NoteStatus == "") {
                                Isdisabled = "";
                            }

                            var NotesPreview = "Clinical_Notes.NotesPreview('" + item.NotesId + "',null,'" + item.PatientId + "','" + item.ProviderId + "','" + utility.RemoveTimeFromDate(null, item.VisitDate) + "','" + item.BillingInfoId + "','" + item.AppointmentDate + "'," + item.VisitId + ",'" + item.NoteDate + "','" + item.PatientTypeId + "', '" + item.FacilityId + "', '" + item.POS + "'," + (!item.RefProviderId ? null : "'" + item.RefProviderId + "'") + ",'" + utility.RemoveTimeFromDate(null, item.CreatedOn) + "'," + (item.IsPhoneEncounter == "0" ? false : true) + ",'" + item.BillingStatus + "');";
                            $row.attr("onclick", NotesPreview);
                            if (canSign) {
                                if (IsNoteUnSign == true) {
                                    $row.append('<td style="display:none;">' + item.NotesId + '</td><td><a ' + Isdisabled + ' ' + isVisible + ' class="btn  btn-xs" href="#" onclick="Clinical_Notes.NotesDelete(' + item.NotesId + ',event,\'' + item.NoteStatus + '\',\'' + item.PatientId + '\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a ' + Isdisabled + ' class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_Notes.NotesEdit(' + item.NotesId + ",'Edit'" + ', event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a title="View Note" class="btn  btn-xs" href="#" onclick="' + NotesPreview + '"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Audit Log" onclick="Clinical_Notes.rowHistory(' + item.NotesId + ',\'' + utility.RemoveTimeFromDate(null, item.VisitDate) + '\',\'' + item.VisitReasonComments + '\',\'' + item.NoteType + '\',event);"> <i class="fa fa-history blue"></i></a> &nbsp;<a data-toggle="tooltip" class="btn btn-xs " href="javascript:void(0)" data-placement="right" onclick="Clinical_Notes.GetNoteInfo(' + item.NotesId + ',event,\'' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '\');" title="Copy Note"><i class="fa fa-clipboard blue"></i></a>&nbsp;<a data-toggle="tooltip" class="btn btn-xs " href="javascript:void(0)" data-placement="right" onclick="Clinical_Notes.NoteUnSign(' + item.NotesId + ',event,\'' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '\');" title="UnSign Note"><i class="fa fa-unlock" aria-hidden="true"></i></a></td><td>' + utility.RemoveTimeFromDate(null, item.VisitDate) + '</td><td>' + item.VisitTime + '</td><td>' + item.VisitType + '</td><td>' + item.TemplateTypeName + '</td>' + '</td><td>' + item.ProviderName + '</td>' + '</td><td>' + item.SignedBy + '</td><td>' + item.FacilityName + '</td><td>' + item.RoomName + '</td><td style="display:none;">' + item.Comments + '</td><td>' + item.VisitReasonComments + '</td>');
                                }
                                else {
                                    $row.append('<td style="display:none;">' + item.NotesId + '</td><td><a ' + Isdisabled + ' ' + isVisible + ' class="btn  btn-xs" href="#" onclick="Clinical_Notes.NotesDelete(' + item.NotesId + ',event,\'' + item.NoteStatus + '\',\'' + item.PatientId + '\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a ' + Isdisabled + ' class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_Notes.NotesEdit(' + item.NotesId + ",'Edit'" + ', event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a title="View Note" class="btn  btn-xs" href="#" onclick="' + NotesPreview + '"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Audit Log" onclick="Clinical_Notes.rowHistory(' + item.NotesId + ',\'' + utility.RemoveTimeFromDate(null, item.VisitDate) + '\',\'' + item.VisitReasonComments + '\',\'' + item.NoteType + '\',event);"> <i class="fa fa-history blue"></i></a> &nbsp;<a data-toggle="tooltip" class="btn btn-xs " href="javascript:void(0)" data-placement="right" onclick="Clinical_Notes.GetNoteInfo(' + item.NotesId + ',event,\'' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '\');" title="Copy Note"><i class="fa fa-clipboard blue"></i></a></td><td>' + utility.RemoveTimeFromDate(null, item.VisitDate) + '</td><td>' + item.VisitTime + '</td><td>' + item.VisitType + '</td><td>' + item.TemplateTypeName + '</td>' + '</td><td>' + item.ProviderName + '</td>' + '</td><td>' + item.SignedBy + '</td><td>' + item.FacilityName + '</td><td>' + item.RoomName + '</td><td style="display:none;">' + item.Comments + '</td><td>' + item.VisitReasonComments + '</td>');
                                }
                            }
                            else {
                                $row.append('<td style="display:none;">' + item.NotesId + '</td><td><a ' + Isdisabled + ' ' + isVisible + ' class="btn  btn-xs" href="#" onclick="Clinical_Notes.NotesDelete(' + item.NotesId + ',event,\'' + item.NoteStatus + '\',\'' + item.PatientId + '\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a ' + Isdisabled + ' class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_Notes.NotesEdit(' + item.NotesId + ",'Edit'" + ', event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a title="View Note" class="btn  btn-xs" href="#" onclick="' + NotesPreview + '"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Audit Log" onclick="Clinical_Notes.rowHistory(' + item.NotesId + ',\'' + utility.RemoveTimeFromDate(null, item.VisitDate) + '\',\'' + item.VisitReasonComments + '\',\'' + item.NoteType + '\',event);"> <i class="fa fa-history blue"></i></a> &nbsp;<a data-toggle="tooltip" class="btn btn-xs " href="javascript:void(0)" data-placement="right" onclick="Clinical_Notes.GetNoteInfo(' + item.NotesId + ',event,\'' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '\');" title="Copy Note"><i class="fa fa-clipboard blue"></i></a></td><td>' + utility.RemoveTimeFromDate(null, item.VisitDate) + '</td><td>' + item.VisitTime + '</td><td>' + item.VisitType + '</td><td>' + item.TemplateTypeName + '</td>' + '</td><td>' + item.ProviderName + '</td>' + '</td><td>' + item.SignedBy + '</td><td>' + item.FacilityName + '</td><td>' + item.RoomName + '</td><td style="display:none;">' + item.Comments + '</td><td>' + item.VisitReasonComments + '</td>');
                            }
                            $("#pnlNotes_Result #dgvClinicalSignedNotes tbody").last().append($row);
                        }
                    });

                    var signedNotesRows = $("#pnlClinicalNotes div#pnlNotes_Result #dgvClinicalSignedNotes tbody").find("tr");

                    if (signedNotesRows.length < 1) {
                        if (!$.fn.dataTable.isDataTable("#pnlClinicalNotes div#pnlNotes_Result #dgvClinicalSignedNotes")) {
                            $("#pnlClinicalNotes div#pnlNotes_Result #dgvClinicalSignedNotes").DataTable({
                                "language": {
                                    "emptyTable": "No Signed Notes Found"
                                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                            });
                        }
                        $("#pnlClinicalNotes #divClinicalSignedNotesPaging").css("display", "none");
                    }


                }
                else {
                    $("#pnlClinicalNotes #divClinicalSignedNotesPaging").css("display", "none");
                    $("#pnlClinicalNotes #divCloseVisitsPaging").css("display", "none");
                    //EMR-602 fix
                    $('#pnlClinicalNotes #PreviousNotediv').addClass('disableAll')
                    if (!$.fn.dataTable.isDataTable('#pnlClinicalNotes #dgvClinicalSignedNotes')) {
                        $('#pnlClinicalNotes #dgvClinicalSignedNotes').DataTable({
                            "language": {
                                "emptyTable": "No Note is Found"
                            }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                        });
                    }
                }

                var IsDataTable = $.fn.dataTable.isDataTable('#pnlClinicalNotes #dgvClinicalSignedNotes');

                if (!IsDataTable) {
                    // to remove records per page dropdown
                    $("#pnlClinicalNotes #pnlNotes_Result #dgvClinicalSignedNotes").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] });
                }
            });
        });
    },
    SetModifiedNoteCount: function () {
        var dfd = $.Deferred();
        if ($('#pnlDashboard div.wModifiedNotes').length > 0) {
            Clinical_Notes.GetModifiedNoteCount_DB_CALL().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    dfd.response = response.ModifiedNoteCount;
                    if (response.ModifiedNoteCount > 0) {
                        $('#pnlDashboard div.wModifiedNotes .badge').css("display", "inline");
                        $('#spnModifiedNotes').css("display", "inline");
                        $('#pnlDashboard div.wModifiedNotes .badge').text(response.ModifiedNoteCount);
                        $('#spnModifiedNotes').text(response.ModifiedNoteCount);
                        $('#wpanel .slick-track div').each(function (i) {
                            if ($(this).find('span:first').text() == 'Modified Notes') {
                                $(this).find('span:last').text(response.ModifiedNoteCount);
                                $(this).find('span:last').show();
                            }
                        });
                        if ($('#pnlDashboard div.wModifiedNotes').addClass('active')) {
                            DashBoard.DashBoardModifiedNotesSearch();
                        }
                    }
                    else {
                        $('#pnlDashboard div.wModifiedNotes .badge').css("display", "none");
                        $('#spnNotesCount').css("display", "none");
                        $('#pnlDashboard div.wModifiedNotes .badge').text("");
                        $('#spnModifiedNotes').text("0");
                        $('#spnModifiedNotes').hide();
                        $('#wpanel .slick-track div').each(function (i) {
                            if ($(this).find('span:first').text() == 'Modified Notes') {
                                $(this).find('span:last').text("0");
                                $(this).find('span:last').hide();
                            }
                        });
                        if ($('#pnlDashboard div.wModifiedNotes').addClass('active')) {
                            DashBoard.DashBoardModifiedNotesSearch();
                        }
                    }
                    dfd.resolve();
                }
                else {
                    dfd.resolve();
                    utility.DisplayMessages(response.message, 3);
                }
            });
        }
        else {
            dfd.resolve();
        }
        return dfd;
    },

    GetModifiedNoteCount_DB_CALL: function () {
        var objData = new Object();
        objData["UserId"] = globalAppdata.AppUserId;
        objData["commandType"] = "Get_ModifiedNoteCount";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },
    NoteUnSign: function (NotesId, event, createdDate) {
        if (event != null) {
            event.stopPropagation();
        }
        var visDate = new Date(createdDate);
        var cutOffDate = new Date('05/09/2017');
        if (visDate < cutOffDate) {
            utility.DisplayMessages("Provider Note created before May 09, 2017 cannot be UnSigned!", 2);

        } else {
            utility.myConfirm('42', function () {
                Clinical_Notes.unsignNote_DBCall(NotesId).done(function (response) {
                    if (response != null) {
                        response = JSON.parse(response);
                    }
                    if (response.status != false) {
                        Clinical_Notes.SetModifiedNoteCount();
                        utility.DisplayMessages(response.Message, 1);
                        Clinical_Notes.NotesSignedSearch(0, null, null, "Signed");
                    } else {
                        utility.DisplayMessages(response.Message, 3);
                    }


                });
            }, function () {

            });
        }



    },
    NotesRowEdit: function (NotesId, mode, event) {

        if (event != null) {
            event.stopPropagation();
        }

        if ((event.srcElement instanceof HTMLAnchorElement || event.srcElement.nodeName.toLowerCase() == 'i') != true) {
            Clinical_Notes.NotesEdit(NotesId, mode, event);
        }
    },

    /// Author: ZeeshanAK
    /// Purpose:  Call for showing history of current item
    /// Date : April 22, 2016
    ShowHistory: function (NotesId, VisitDate, VisitReasonComments, NoteTempType) {

        var params = [];
        params["FromAdmin"] = "0";
        params["NotesId"] = NotesId;
        if ($('#mstrTabDashBoard').hasClass('active')) {
            params["ParentCtrl"] = 'mstrTabDashBoard';
        } else {
            params["ParentCtrl"] = 'clinicalTabNotes';
        }
        params["VisitDate"] = VisitDate;
        params["VisitReasonComments"] = VisitReasonComments;
        params["NoteType"] = NoteTempType;
        LoadActionPan('Clinical_Note_Components_Audit', params);

    },

    /// Author: ZeeshanAK
    /// Purpose:  Row history
    /// Date : April 22, 2016
    rowHistory: function (NotesId, VisitDate, VisitReasonComments, NoteTempType, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var currentNotesId = NotesId != null ? NotesId : -1;
        if (currentNotesId > 0) {
            Clinical_Notes.ShowHistory(currentNotesId, VisitDate, VisitReasonComments, NoteTempType);
        }
    },

    //Opening Patient List from PQRS Report Dashboard
    openPatientList: function (patientID, isComponentSelect, ProviderId, VisitDate, NotesId, NoteStatus, ParentCtrl, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId) {

        var params = [];
        params["mode"] = "Add";
        params["PatientIds"] = patientID;
        params["isComponentSelect"] = isComponentSelect;
        params["ProviderId"] = ProviderId;
        var arrcurrentMeasureReasoning = null;
        if (ParentCtrl == "clinicalTabNotes") {
            arrcurrentMeasureReasoning = JSON.parse(Clinical_Notes.arrCQMReasoning[patientID]);
        }
        else if (ParentCtrl == "mstrTabDashBoard") {
            arrcurrentMeasureReasoning = JSON.parse(DashBoard.arrCQMReasoning[patientID]);
        }
        else if (ParentCtrl == "clinicalTabPhoneEncounter") {
            arrcurrentMeasureReasoning = JSON.parse(Clinical_PhoneEncounter.arrCQMReasoning[patientID]);
        }
        else if (ParentCtrl == "Clinical_NotesView") {
            arrcurrentMeasureReasoning = JSON.parse(Clinical_NotesView.arrCQMReasoning[patientID]);
        }
        else if (ParentCtrl == "Clinical_NotesSearch") {
            arrcurrentMeasureReasoning = JSON.parse(Clinical_NotesSearch.arrCQMReasoning[patientID]);
        }
        if (arrcurrentMeasureReasoning != null && arrcurrentMeasureReasoning.length > 0) {
            params["arrcurrentMeasureReasoning"] = arrcurrentMeasureReasoning;
        }
        else {
            params["arrcurrentMeasureReasoning"] = "";
        }
        params["FromParentCtrl"] = ParentCtrl;
        params["PatientId"] = patientID;
        params["FromAdmin"] = 0;
        params["ReportFromDate"] = VisitDate;
        params["ReportToDate"] = VisitDate;
        params["ParentCtrl"] = ParentCtrl;
        params["VisitDate"] = VisitDate;
        params["NotesId"] = NotesId;
        params["NoteStatus"] = NoteStatus;
        params["BillingInfoId"] = BillingInfoId;
        params["AppointmentDate"] = AppointmentDate;
        params["VisitId"] = VisitId;
        params["NoteDate"] = NoteDate;
        params["PatientTypeId"] = PatientTypeId;
        LoadActionPan('PQRS_Patient_List', params);
    },

    SignNotes: function (NotesId, event, NoteStatus, isComponentSelect, VisitDate, PatientId, ProviderId, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter) {

        Clinical_ProgressNote.params.VisitDateTime = NoteDate;
        var objDeffered = $.Deferred();
        if (event != null) {
            event.stopPropagation();
        }

        //Start//02-05-2016//Ahmad Raza//logic for CDS Alert
        var CDSAlertCount = parseInt($(" #mainForm  li#CDSAlert span").text());
        if (CDSAlertCount > 0) {
            utility.DisplayMessages("Please change the Status of all the CDS Alerts before signing the Note.", 3);
        }
        else {
            var ProviderId = $("#" + Clinical_Notes.params.PanelID + " #hfProvider").val();
            Clinical_Notes.VBPWithReasoningLoad(NotesId, event, NoteStatus, isComponentSelect, VisitDate, PatientId, ProviderId, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter);

        }
    },
    VBPWithReasoningLoad: function (NotesId, event, NoteStatus, isComponentSelect, VisitDate, PatientId, ProviderId, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter) {
        var objDeffered = $.Deferred();

        Clinical_ProgressNote.loadVBPWithReasoning(VisitDate, VisitDate, PatientId, ProviderId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_Notes.params["VBPResponse"] = response;
                if (response.AllMeasuresReasoningDetailCount > 0) {
                    var arrNonCompliantPatients = $.grep(JSON.parse(response.AllMeasuresReasoningDetailLoad_JSON), function (a) {
                        return a.Patientid == PatientId && a.NoteId == NotesId;
                    });

                    if (arrNonCompliantPatients.length > 0) {

                        Clinical_Notes.arrVBPReasoning[PatientId] = JSON.stringify(arrNonCompliantPatients);
                        var CQMFoundMsg = "Our System found some <span class='red'>missing data</span> related to this patient."
                                        + " In order to qualify for the <b>2017 Value Based program incentives,</b> you must enter those <span class='red'>missing data values</span>"
                                        + " against the Value Based program that you have planned to report this year. Do you want to enter the data here before signing off this note?";
                        var PHQ2Missing = false;
                        var PHQ9Missing = false;
                        utility.myConfirm(CQMFoundMsg, function () {
                            $.each(arrNonCompliantPatients, function (key) {
                                if (arrNonCompliantPatients[key].MeasureId == "PHQ2") {
                                    PHQ2Missing = true;
                                }
                                else if (arrNonCompliantPatients[key].MeasureId == "PHQ9") {
                                    PHQ9Missing = true;
                                }
                            });
                            $.when(Clinical_Notes.openMissingAlert_VBP(null, null, null, 'clinicalTabNotes', NotesId, event, NoteStatus, isComponentSelect, VisitDate, PatientId, ProviderId, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, PHQ2Missing, PHQ9Missing)).then(function () {
                                Clinical_Notes.params.isVBPExists = 1;
                                objDeffered.resolve();
                            });
                        }, function () {
                            $.when(Clinical_Notes.CQMWithReasoningLoad(NotesId, event, NoteStatus, isComponentSelect, VisitDate, PatientId, ProviderId, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter)).then(function () {
                                objDeffered.resolve();
                            });
                        },
                               '<b>2017 Value Based Program Missing Data Alert</b>', "Yes, I do", "No, not this time"
                          );
                    }
                    else {
                        $.when(Clinical_Notes.CQMWithReasoningLoad(NotesId, event, NoteStatus, isComponentSelect, VisitDate, PatientId, ProviderId, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter)).then(function () {
                            objDeffered.resolve();
                        });
                    }
                }
                else {
                    $.when(Clinical_Notes.CQMWithReasoningLoad(NotesId, event, NoteStatus, isComponentSelect, VisitDate, PatientId, ProviderId, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter)).then(function () {
                        objDeffered.resolve();
                    });
                }
            }
            else {
                $.when(Clinical_Notes.CQMWithReasoningLoad(NotesId, event, NoteStatus, isComponentSelect, VisitDate, PatientId, ProviderId, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter)).then(function () {
                    objDeffered.resolve();
                });
                utility.DisplayMessages(response.Message, 3);
            }
        });
        return objDeffered;
    },

    openMissingAlert_VBP: function (BillingInformation, Obj, customSigMsg, prntctrl, NotesId, event, NoteStatus, isComponentSelect, VisitDate, PatientId, ProviderId, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, PHQ2Missing, PHQ9Missing) {
        var params = [];
        params["FromAdmin"] = "0";
        params["BillingInformation"] = BillingInformation;
        params["Obj"] = Obj;
        params["customSigMsg"] = customSigMsg;
        params["ParentCtrl"] = prntctrl;
        params["NoteId"] = NotesId;
        params["event"] = event;
        params["NoteStatus"] = NoteStatus;
        params["PatientIds"] = PatientId;
        params["PatientId"] = PatientId;
        params["ProviderId"] = ProviderId;
        params["isComponentSelect"] = isComponentSelect;
        params["VisitDate"] = VisitDate;
        params["BillingInfoId"] = BillingInfoId;
        params["AppointmentDate"] = AppointmentDate;
        params["VisitId"] = VisitId;
        params["NoteDate"] = NoteDate;
        params["PatientTypeId"] = PatientTypeId;
        params["FacilityId"] = FacilityId;
        params["POS"] = POS;
        params["RefProviderID"] = RefProviderID;
        params["PHQ2Missing"] = PHQ2Missing;
        params["PHQ9Missing"] = PHQ9Missing;

        LoadActionPan('VBP_MissingDataAlert', params);
    },
    CQMWithReasoningLoad: function (NotesId, event, NoteStatus, isComponentSelect, VisitDate, PatientId, ProviderId, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter) {
        var objDeffered = $.Deferred();
        Clinical_ProgressNote.loadCQMWithReasoning(VisitDate, VisitDate, Clinical_Notes.params.patientID, ProviderId, NotesId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_Notes.params["CQMResponse"] = response;
                if (response.AllMeasuresReasoningDetailCount > 0) {
                    var arrNonCompliantPatients = $.grep(JSON.parse(response.AllMeasuresReasoningDetailLoad_JSON), function (a) {
                        return a.Patientid == Clinical_Notes.params.patientID;
                    });

                    Clinical_Notes.arrCQMReasoning[Clinical_Notes.params.patientID] = JSON.stringify(arrNonCompliantPatients);
                    var CQMFoundMsg = "Our System found some <span class='red'>missing data</span> related to this patient."
                                    + " In order to qualify for the <b>2016 CQM incentives,</b> you must enter those <span class='red'>missing data values</span>"
                                    + " against the CQM measures that you have planned to report this year. Do you want to enter the data here before signing off this note?";

                    utility.myConfirm(CQMFoundMsg, function () {
                        objDeffered.resolve();
                        var currentProviderId = $("#" + Clinical_Notes.params.PanelID + " #hfProvider").val();
                        Clinical_Notes.openPatientList(Clinical_Notes.params.patientID, isComponentSelect, ProviderId, VisitDate, NotesId, NoteStatus, "clinicalTabNotes");
                    }, function () {
                        $.when(Clinical_Notes.SignNotesAfterCQM(NotesId, event, NoteStatus, isComponentSelect, VisitDate, PatientId, ProviderId, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter)).then(function () {
                            objDeffered.resolve();
                        });
                    },
                          '<b>2016 CQM Missing Data Alert</b>', "Yes, I do", "No, not this time"
                      );

                }
                else {
                    $.when(Clinical_Notes.SignNotesAfterCQM(NotesId, event, NoteStatus, isComponentSelect, VisitDate, PatientId, ProviderId, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter)).then(function () {
                        objDeffered.resolve();
                    });
                }
            }
            else {
                objDeffered.resolve();
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    //This function is called by Sign btn of Notes Grid. it ask for signing Progress note
    //NotesGridLoad
    SignNotesAfterCQM: function (NotesId, event, NoteStatus, isComponentSelect, VisitDate, PatientId, ProviderId, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter) {

        //Start//29/12/2015//Ahmad Raza//Privileges logic implemented
        AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                $.when(Clinical_ProgressNote.signNote(NotesId, PatientId, false, IsPhoneEncounter)).done(function () {
                    Clinical_NotesSearch.SetNotesCount();
                    Clinical_Notes.NotesDraftSearch(0, null, null, NoteStatus);
                    Clinical_Notes.NotesSignedSearch(0, null, null, 'Signed');
                });
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    CreateCharges: function (Obj, NotesId, PatientId, ProviderId, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID) {
        BillingInformation.params = Clinical_Notes.initializeBillingInfoParams(NotesId, PatientId, ProviderId, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID);
        Clinical_NotesSearch.CreateObjectForBilling(POS);
    },
    initializeBillingInfoParams: function (NotesId, PatientId, ProviderId, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID) {
        var params = [];
        params["ParentCtrl"] = "clinicalTabNotes";
        params["FromAdmin"] = 0;
        params["NotesId"] = NotesId;
        params["VisitId"] = VisitId;
        params["NoteDate"] = NoteDate;
        params["BillingInfoId"] = BillingInfoId;
        params["VisitDate"] = VisitDate;
        params["PatientId"] = PatientId;
        params["ProviderId"] = ProviderId;
        params["PatientTypeId"] = PatientTypeId;
        params["AppointmentDate"] = AppointmentDate;
        params["FacilityId"] = FacilityId;
        BillingInformation.PatientInfoJSON = {};
        BillingInformation.PatientInfoJSON.RefProviderID = RefProviderID;
        BillingInformation.PatientInfoJSON.FacilityID = FacilityId;
        return params;
    },

    LoadAttachecdICDsAndCPTs: function () {

        var objData = new Object();
        objData["NotesId"] = Clinical_Notes.params.NotesId;
        objData["PatientId"] = Clinical_Notes.params.patientID;
        objData["commandType"] = "LoadProceduresAndProblemsByNoteAndPatientId";
        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "BillingInformation", "BillingInformation");
    },

    UnLoad: function () {

        RemoveAdminTab('clinicalTabNotes');
    },

    //------------------DB Call Functions------------------
    fillPatientIinfo_DBCall: function (patientID) {
        var objData = {};
        objData["PatientID"] = patientID;
        objData["commandType"] = "fill_patient_info";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },
    unsignNote_DBCall: function (noteId) {
        var objData = {};
        objData["NotesID"] = noteId;
        objData["commandType"] = "unsign_note";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    NotesDeleted: function (NotesId) {
        var objData = {};
        objData["NotesId"] = NotesId;
        //   var data = "NotesID=" + NotesId;
        objData["commandType"] = "DELETE_CLINICAL_NOTES";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    NotesUpdate: function (NotesData, NotesID) {
        var objData = JSON.parse(NotesData);
        if (Clinical_ProgressNote.ComeFromCopyNote) {

            Clinical_ProgressNote.ComeFromCopyNote = false;
            objData["ComeFromCopyNote"] = "1";
        }
        else {
            objData["ComeFromCopyNote"] = "0";
        }

        objData["NotesId"] = NotesID;
        objData["PatientID"] = Clinical_Notes.params.patientID;
        objData["AppointmentID"] = Clinical_Notes.params.AppointmentID;


        objData["commandType"] = "UPDATE_CLINICAL_NOTES";

        var data = JSON.stringify(objData);

        // sNotesch parameter , class name, command name of class
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },
    VisitTypeUpdate: function (visitTypeId, NotesId, AppointmentId)
    { },
    UpdateVisitType: function () {
        Clinical_Notes.VisitTypeUpdate(VisitTypeId, Clinical_Notes.params.NotesId, AppointmentId).done(function (response) {

            response = JSON.parse(response);
            Clinical_Notes.params["TemplateAddOrEdit"] = "0";
            Clinical_ProgressNote.params.triggerCount = 0;

            if (response.status != false) {
                Clinical_Notes.AddProgressNoteTab();
                utility.DisplayMessages(response.Message, 1);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    NotesSave: function (NotesData, patientID, IsToCheckForTodaysNote) {

        var objData = JSON.parse(NotesData);
        objData["PatientID"] = patientID;
        objData["IsToCheckForTodaysNote"] = IsToCheckForTodaysNote;
        objData["AppointmentID"] = Clinical_Notes.params.AppointmentID;
        if (!objData["VisitId"] || parseInt(objData["VisitId"]) <= 0)
        {
            objData["VisitId"] = Clinical_Notes.params["AppointmentVisitId"] && parseInt(Clinical_Notes.params["AppointmentVisitId"]) > 0 ? Clinical_Notes.params["AppointmentVisitId"] : "";
        }
        
        objData["IsHPIComplaint"] = globalAppdata["IsDefaultHPI"] == "False" ? false : true;
        objData["commandType"] = "SAVE_CLINICAL_NOTES";
        if (Clinical_Notes.params.NoteComponentList) {
            objData["NoteComponentList"] = Clinical_Notes.params.NoteComponentList;
        }
        else {
            objData["NoteComponentList"] = []
        }

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    NotesUpdateActiveInactive: function (NotesId, IsActive) {

        var objData = {};
        objData["NotesId"] = NotesId;
        objData["IsActive"] = IsActive;

        objData["commandType"] = "UPDATE_CLINICAL_NOTES_ACTIVE_INACTIVE";
        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    SearchNotes: function (NotesId, PageNumber, RowsPerPage, NoteStatus) {

        if (PageNumber == null) {
            PageNumber = 1;
        }

        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }

        var objData = {};
        objData["NotesId"] = NotesId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;

        if (Clinical_Notes.params.patientID != null) {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }

        objData["NoteStatus"] = NoteStatus;
        objData["commandType"] = "LOAD_CLINICAL_NOTES";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    NotesPreview: function (NotesId, ParentCtrl, PatientId, ProviderId, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, createdOn, IsPhoneEncounter, BillingInfoStatus, IsFromDocument) {
        //  Clinical_Notes.canSignNote().done(function (canSign) {
        var params = [];

        params["FromAdmin"] = "0";
        params["NotesId"] = NotesId;
        params["PatientId"] = PatientId;
        if ($('#pnlNotes_Result #liSignedNotes').hasClass('active')) {
            params["RefSearch"] = "SignedSearch";
        } else {
            params["RefSearch"] = "DraftSearch";
        }

        params["ProviderId"] = ProviderId;
        params["VisitDate"] = VisitDate;
        params["VisitDateWithoutTime"] = createdOn;
        params["ParentCtrl"] = ParentCtrl || 'clinicalTabNotes';
        params["ParentCtrlPanelID"] = Clinical_Notes.params.PanelID;
        params["HasUnSignPermission"] = Clinical_Notes.params.HasUnSignPermission;

        params["BillingInfoId"] = BillingInfoId;
        params["AppointmentDate"] = AppointmentDate;
        params["FacilityId"] = FacilityId;
        params["RefProviderID"] = RefProviderID;
        params["VisitId"] = VisitId;
        params["NoteDate"] = NoteDate;
        params["PatientTypeId"] = PatientTypeId;
        params["POS"] = POS;
        params["IsPhoneEncounter"] = IsPhoneEncounter;
        params["BillingInfoStatus"] = BillingInfoStatus;
        params["IsFromDocument"] = IsFromDocument;
        //   params["CanSign"] = canSign;
        LoadActionPan('Clinical_NotesView', params);
        // });
    },

    GetPreviousNotePatient_DBCall: function () {

        var objData = {};
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }

        objData["commandType"] = "copy_previous_note_patient";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    // this function is used by both Notes and Progress Note Form
    GetLinkedAppointment_DBCall: function (ActionPanID) {

        if ($("#" + Clinical_Notes.params.PanelID + " #txtProvider").val() != "") {
            var objData = {};
            if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
                objData["PatientId"] = 0;
            } else {
                objData["PatientId"] = Clinical_Notes.params.patientID;
            }

            if (ActionPanID.indexOf("actionPanClinicalProgressNote") > -1) {
                objData["ProviderId"] = $("#" + Clinical_ProgressNote.params.PanelID + " #hfProviderId").val();
            }
            else {
                objData["ProviderId"] = $("#" + Clinical_Notes.params.PanelID + " #hfProvider").val();
            }

            objData["commandType"] = "fill_linked_appointment_notes";
            var data = JSON.stringify(objData);
            return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
        }
        else {
            utility.DisplayMessages("Select Provider", 3);
        }
    },
    //------------------END DB Call Functions------------------


    //----------------Progress Notes----------------------
    // this function is used by both Notes and Progress Note Form
    LinkedAppointment: function (AppointmentId, VisitId, AppointmentDate, IsNonBilable, VisitTypeId, PatientType) {

        var CurrentPanelId = 'pnlClinicalNotes';
        if (GetCurrentSelectedTab() != null) {
            CurrentPanelId = GetCurrentSelectedTab().PanelID;
        }

        Clinical_Notes.UnLoadLinkedAppointmentList();

        $("#" + CurrentPanelId + " #hfAppointmentId").val(AppointmentId);
        Clinical_Notes.params.AppointmentId = AppointmentId;

        $("#" + CurrentPanelId + " #hfVisitId").val(VisitId);
        $("#" + CurrentPanelId + " #txtLinkedAppointment").val(AppointmentDate);
        $("#" + CurrentPanelId + " #ChkBox_LinkedAppointment").prop('checked', true);

        $('#' + Clinical_Notes.params.PanelID + ' #dtpVisitDate').val(utility.RemoveTimeFromDate(null, AppointmentDate));
        $('#' + Clinical_Notes.params.PanelID + ' #dtpVisitDate').datepicker('setDate', utility.RemoveTimeFromDate(null, AppointmentDate))
        $('#' + Clinical_Notes.params.PanelID + ' #VisitTime').val(new Date(AppointmentDate).toLocaleTimeString());
        $('#' + Clinical_Notes.params.PanelID + ' #VisitTime').timepicker('setTime', new Date(AppointmentDate).toLocaleTimeString());


        Clinical_Notes.showDropdownOptions(PatientType).done(function () {
            $('#' + Clinical_Notes.params.PanelID + ' #ddlVisitType').val(VisitTypeId);
            $('#' + Clinical_Notes.params.PanelID + ' #ddlVisitType').val(VisitTypeId).attr('selected', 'selected');

            if (Clinical_Notes.params.PanelID == "pnlClinicalProgressNote") {
                //   Clinical_ProgressNote.UpdateVisitType();
                Clinical_ProgressNote.EnableDisableCancerReportButton();
            }

        });



        Clinical_Notes.VisitSearch(VisitId, Clinical_Notes.params["patientID"]);

        Clinical_Notes.UnlinkAppointment($("#" + CurrentPanelId + " #ChkBox_LinkedAppointment"), CurrentPanelId, true, IsNonBilable);
    },

    showDropdownOptions: function (value) {
        var objDeffered = $.Deferred();
        if (value != 'undefined') {
            //$('#' + Clinical_Notes.params.PanelID + ' #ddlVisitType option').map(function () {
            //    return $(this).parent('span').length === 0 ? this : null;
            //}).wrap('<span>').hide();
            $('#' + Clinical_Notes.params.PanelID + ' #ddlVisitType option').addClass('hidden');

            $('#' + Clinical_Notes.params.PanelID + ' #ddlVisitType').find('option[refvalue=""]').removeClass('hidden');
            $('#' + Clinical_Notes.params.PanelID + ' #ddlVisitType option[refvalue="' + value + '"]').removeClass('hidden');




            //$('#' + Clinical_Notes.params.PanelID + ' #ddlVisitType').find('option[refvalue=""]').unwrap().show();
            //$('#' + Clinical_Notes.params.PanelID + ' #ddlVisitType').find('option[refvalue="' + value + '"]').unwrap().show();
            objDeffered.resolve("ok");
        }
        else {
            objDeffered.resolve("ok");
        }
        return objDeffered.promise();

    },
    // this function is used by both Notes and Progress Note Form
    UnlinkAppointment: function (Cntrl, PanelID, IsCurrentDate, IsNonBilable) {

        if ($(Cntrl).is(':checked')) {
            $("#" + PanelID + " #lblLinkedAppointment").show();
            $("#" + PanelID + " #lnkLinkedAppointment").hide();
            // $("#"+PanelID+" #txtLinkedAppointment").attr("disabled", "disabled");
            if (PanelID == 'pnlClinicalProgressNote')
                $("#" + PanelID + " #ChkBox_LinkedAppointment").attr("disabled", "disabled");
            else if (PanelID == 'pnlClinicalNotes')
                $("#" + PanelID + " #ChkBox_LinkedAppointment").removeAttr("disabled");

            if (IsNonBilable == true) {
                $("#" + PanelID + " #ChkBox_IsNonBilable").prop('checked', IsNonBilable);
                if (PanelID == 'pnlClinicalProgressNote')
                    $("#" + PanelID + " #ChkBox_IsNonBilable").change();
            }
            else if (IsNonBilable == false) {
                $("#" + PanelID + " #ChkBox_IsNonBilable").prop('checked', IsNonBilable);
                if (PanelID == 'pnlClinicalProgressNote')
                    $("#" + PanelID + " #ChkBox_IsNonBilable").change();
            } else if (Clinical_Notes.params.AppointmentId > 0) {

                appointmentDetail.FillAppointment(Clinical_Notes.params.AppointmentId).done(function (response) {

                    if (response.status != false) {

                        var appointment_detail = JSON.parse(response.AppointmentFill_JSON);

                        $("#" + PanelID + " #ChkBox_IsNonBilable").prop('checked', (appointment_detail.chkNonBilable.toLowerCase() == "true"));
                        if (PanelID == 'pnlClinicalProgressNote')
                            $("#" + PanelID + " #ChkBox_IsNonBilable").change();

                    } else {
                        utility.DisplayMessages(response.Message, 3);
                        BackgroundLoaderShow(false);
                    }
                });
            } else {

                $("#" + PanelID + " #ChkBox_IsNonBilable").prop('checked', false);
            }

        } else {

            $("#" + PanelID + " #providerDiv").removeClass("disableAll");
            $("#" + PanelID + " #ChkBox_LinkedAppointment").prop('checked', false);
            $("#" + PanelID + " #ChkBox_IsNonBilable").prop('checked', false);
            $("#" + PanelID + " #lblLinkedAppointment").hide();
            $("#" + PanelID + " #lnkLinkedAppointment").show();
            $("#" + PanelID + " #txtLinkedAppointment").val("");
            $("#" + PanelID + " #hfAppointmentId").val("");
            $("#" + PanelID + "  #hfAppointmentId").val('');
            $("#" + PanelID + "  #hfVisitId").val('');
            $("#" + PanelID + "  #txtLinkedAppointment").val('');
            // Faizan Ameen MU3-32 
            if (PanelID == 'pnlClinicalNotes') {
                $('#' + Clinical_Notes.params.PanelID + ' #ddlVisitType option').each(function () {
                    $(this).removeClass('hidden');
                });
                Clinical_Notes.params["PatientTypeId"]
                $('#' + Clinical_Notes.params.PanelID + ' #ddlVisitType').val("");
            }
            // End of code changed by Faizan Ameen.
            Clinical_Notes.params.AppointmentId = 0;
            Clinical_Notes.params.NotesVisitId = -1;
            //fahad

            if (IsCurrentDate) {

                $('#' + Clinical_Notes.params.PanelID + ' #VisitTime').timepicker('setTime', new Date());
                var date_format = 'dd/mm/yyyy';
                //set default Date Formate
                if (globalAppdata['DateFormat'])
                    date_format = globalAppdata['DateFormat'];
                $('#' + Clinical_Notes.params.PanelID + ' #dtpVisitDate').datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), new Date()));

            }

            $('#' + Clinical_Notes.params.PanelID + ' #txtLinkedAppointment').val('No Appointment Selected');
            $("#" + PanelID + " #ChkBox_LinkedAppointment").attr("disabled", "disabled");
        }
    },

    changeIsNonBilable: function (chkBox) {
        // Commentee For IMP IMP-1932 
        //if ($(chkBox).is(':checked')) {
        //    utility.myConfirm('Are you sure you want to make the Visit as Non Billable?', function () {
        //        //
        //    }, function () {
        //        $(chkBox).prop('checked', false);
        //    },
        //           'Confirm Non Billable'
        //       );
        //} else {
        //    //
        //}
    },
    changeIsNonBilableForProgressNote: function (chkBox) {

        if ($(chkBox).is(':checked')) {
            utility.myConfirm('Are you sure you want to make the Visit as Non Billable?', function () {
                Clinical_Notes.NotesUpdateForNonBillable(Clinical_ProgressNote.params.NotesId);
            }, function () {
                $(chkBox).prop('checked', false);
                Clinical_Notes.NotesUpdateForNonBillable(Clinical_ProgressNote.params.NotesId);
            }, 'Confirm Non Billable');
        }
        else {
            Clinical_Notes.NotesUpdateForNonBillable(Clinical_ProgressNote.params.NotesId);
        }
    },
    NotesUpdateForNonBillable: function (NotesId) {
        var dfd = $.Deferred();
        var self = $("#" + Clinical_Notes.params["PanelID"]);
        var myJSON = self.getMyJSONByName();
        Clinical_Notes.NotesUpdate(myJSON, NotesId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                dfd.resolve();
            }
            else {
                dfd.resolve();
                utility.DisplayMessages(response.Message, 3);
            }
        });
        return dfd;
    },


    // this function is used by both Notes and Progress Note Form
    GetLinkedAppointment: function (ActionPanID) {

        Clinical_Notes.GetLinkedAppointment_DBCall(ActionPanID).done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {

                if (response.status != false) {
                    Clinical_Notes.LinkedAppointmentGridLoad(response);
                    $(ActionPanID).prepend($("#pnlLinekdAppointment_Result").html());
                    $(ActionPanID).modal({
                        show: 'true',
                        backdrop: 'static',
                        keyboard: false

                    }).on('hidden.bs.modal', function () {
                        $('body').addClass('modal-open');
                    });

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    // this function is used by both Notes and Progress Note Form
    UnLoadLinkedAppointmentList: function () {

        var CurrentTab = GetCurrentSelectedTab();
        var ActionPanId = '#pnlClinicalNotes #actionPanClinicalNotes';
        if (CurrentTab != null) {
            ActionPanId = '#' + CurrentTab.PanelID + ' #' + CurrentTab.ActionPanContainer;
        }

        $(ActionPanId).modal('hide');

        setTimeout(function () {
            $(ActionPanId).find('div').first().remove();
            if ($('#' + GetCurrentSelectedTab().PanelID).find('div').first().attr('aria-hidden') == "false") {
                $("body").addClass("modal-open");
            }
            else
                $("body").removeClass("modal-open");
        }, 500);

    },

    LinkedAppointmentGridLoad: function (response) {

        if ($('#pnlLinekdAppointment_Result #dgvLinekdAppointment').length > 1)
            $('#pnlLinekdAppointment_Result #dgvLinekdAppointment')[1].remove();

        $('#pnlLinekdAppointment_Result #dgvLinekdAppointment').dataTable().fnDestroy();
        $("#pnlLinekdAppointment_Result #dgvLinekdAppointment tbody").find("tr").remove();

        if (response.LinkedAppointmentCount > 0) {

            var Clinical_Notes_LinkedAppointment_detail = JSON.parse(response.LinkedAppointment_JSON);

            $.each(Clinical_Notes_LinkedAppointment_detail, function (i, item) {

                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvNotes_row" + item.NotesId + "'))");
                $row.attr("id", "gvAppointment_row" + item.AppointmentId);
                $row.attr("IsNonBilable", item.IsNonBilable);
                $row.attr("AppointmentId", item.AppointmentId);
                $row.attr("VisitId", item.VisitId);
                $row.attr("DOS", utility.RemoveTimeFromDate(null, item.DOS));
                $row.attr("AppointmentDate", utility.RemoveTimeFromDate(null, item.AppointmentDate));
                $row.attr("Appointment", item.VisitTime);
                $row.attr("Provider", item.ProviderName);
                $row.attr("Facility", item.FacilityName);
                $row.attr("Status", item.VisitStatusName);
                $row.attr("RrefProvider", item.RefProviderName);

                $row.append('<td style="display:none;">' + item.AppointmentId + '</td><td><a  class="btn  btn-xs" href="#" onclick="Clinical_Notes.LinkedAppointment(' + item.AppointmentId + ',' + item.VisitId + ',\'' + (Clinical_Notes.getdatetime(new Date(item.AppointmentDate), true)) + '\',' + item.IsNonBilable.toLowerCase() + ',' + item.VisitTypeId + ',\'' + item.PatientType.toLowerCase() + '\');" title="Linked Appointment"><i class="fa fa-check black"></i></a></td><td>' + (Clinical_Notes.getdatetime(new Date(item.AppointmentDate), true)) + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOS) + '</td><td>' + item.ProviderName + '</td><td>' + item.FacilityName + '</td><td>' + item.VisitStatusName + '</td>');

                $("#pnlLinekdAppointment_Result #dgvLinekdAppointment tbody").last().append($row);
            });
        }
        else {
            $('#pnlLinekdAppointment_Result #dgvLinekdAppointment').DataTable({
                "language": {
                    "emptyTable": "No Linked Appointment is Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }

        var IsDataTable = $.fn.dataTable.isDataTable('#pnlLinekdAppointment_Result #dgvLinekdAppointment');

        if (!IsDataTable) {
            // to remove records per page dropdown
            $("#pnlLinekdAppointment_Result #dgvLinekdAppointment").DataTable({ "bInfo": false, "bPaginate": true, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] });
        }
    },

    GetPreviousNotePatient: function () {

        Clinical_Notes.GetPreviousNotePatient_DBCall().done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {

                var Clinical_Notes_PreviousNote_detail = JSON.parse(response.PreviousNote_JSON);
                if (response.PreviousNoteCount > 0) {
                    var noteData = Clinical_Notes_PreviousNote_detail;

                    $.when(Clinical_ProgressNote.GetNotesTemplates(noteData.ProviderId, noteData.NoteTemplate, noteData.NoteType, Clinical_Notes.params.PanelID)).done(function () {
                        $("#pnlClinicalNotes #NoteType").val(noteData.NoteType);
                        $.when(Clinical_ProgressNote.GetNoteTemplateType(noteData.NoteTemplate, Clinical_Notes.params.PanelID)).done(function () {
                            $("#pnlClinicalNotes #NoteTemplate").val(noteData.NoteTemplate);
                            var self = $('#' + Clinical_ProgressNote.params.PanelID);
                            self.find('#divRoomNo.GetDDLRooms > select').attr('ddlist', 'GetRooms');
                            var data = "IsActive=&ID=" + noteData.FacilityId;
                            self.find('#divRoomNo.GetDDLRooms').loadDropDowns(true, data).done(function () {

                                var self = $('#pnlClinicalNotes #frmClinicalNotes');
                                //binding values to form controls
                                Clinical_Notes.BindVisitsValues(null, null, null, null, noteData.ProviderId, noteData.Provider, noteData.Facility,
                                    noteData.FacilityId, noteData.VisitReason, noteData.RoomNo, noteData.RefProviderId, noteData.RefProvider, true);
                                $('#pnlClinicalNotes #frmClinicalNotes #VisitReason').val(noteData.VisitReason);
                                $('#pnlClinicalNotes #frmClinicalNotes #RoomNo').val(noteData.RoomNo);
                                $('#pnlClinicalNotes #frmClinicalNotes #NoteTemplate').val(noteData.NoteTemplate);
                                $('#pnlClinicalNotes #frmClinicalNotes #NoteType').val(noteData.NoteType);
                                var CC = noteData.CheifComplaint;
                                $("#pnlClinicalNotes #txtCopayPreviousNote").val(utility.RemoveTimeFromDate(null, noteData.NoteDate) + ((CC != null && CC != '') ? " - " + Clinical_Notes_PreviousNote_detail.CheifComplaint : ""));

                                $('#pnlClinicalNotes #ProgressnoteHTML').html($(noteData.NoteText).get(0));
                                $('#pnlClinicalNotes #ProgressnoteHTML').find('clinical_vitals').parent().parent().remove();
                                $("#pnlClinicalNotes #hfNoteText").val($('#pnlClinicalNotes #ProgressnoteHTML').html());
                                $("#pnlClinicalNotes #hfPrevNotesId").val(noteData.PrevNotesId);
                                $("#pnlClinicalNotes #chkCopayPreviousNote").prop('checked', true);
                                Clinical_Notes.UnLinkPreviousNotePatient($("#pnlClinicalNotes #chkCopayPreviousNote"), 'pnlClinicalNotes');
                                $('#pnlClinicalNotes .NoteTemplate').removeClass('disableAll');
                                $('#pnlClinicalNotes .NoteType').removeClass('disableAll');

                            });
                        });
                    });
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    //this function is used by Progress Notes and Notes Form
    UnLinkPreviousNotePatient: function (Cntrl, PanelID) {
        if ($(Cntrl).is(':checked')) {
            $("#" + PanelID + " #lblPreviousNote").show();
            $("#" + PanelID + " #lnkPreviousNote").hide();
            $("#" + PanelID + " #txtCopayPreviousNote").attr("disabled", "disabled");
            $("#" + PanelID + " #chkCopayPreviousNote").removeAttr("disabled");
        } else {
            $("#" + PanelID + " #lblPreviousNote").hide();
            $("#" + PanelID + " #lnkPreviousNote").show();
            $("#" + PanelID + " #txtCopayPreviousNote").val("");
            $("#" + PanelID + " #hfNoteText").val("");
            $("#" + PanelID + " #txtCopayPreviousNote").removeAttr("disabled");
            $("#" + PanelID + " #chkCopayPreviousNote").attr("disabled", "disabled");
        }

    },


    //--------------- End Progress Notes ----------------------------------


    // -------------- Provider ---------------------

    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmClinicalNotes";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "clinicalTabNotes";
        LoadActionPan('Admin_Provider', params);
    },

    OpenProviderDetail: function (HiddenCtrl, TxtBoxCtrl) {

        var params = [];
        params["ProviderId"] = $('#' + Clinical_Notes.params["PanelID"] + ' #' + HiddenCtrl).val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = TxtBoxCtrl;
        params["ParentCtrl"] = 'clinicalTabNotes';
        LoadActionPan('providerDetail', params);
    },

    OpenResourceProvider: function (Title, RefCtrl, RefCtrlHidden, RefCtrlLabel, RefCtrlLink) {
        var params = [];
        var PanelID = Clinical_Notes.params["TabID"];
        //if (RefCtrl == 'frmClinicalNotes #txtProvider' || RefCtrl == 'frmClinicalNotes #txtResourceProvider')
        //    params["IsOptional"] = false;
        //else
        params["IsOptional"] = true;
        params["RefForm"] = 'frmClinicalNotes';
        params["Title"] = Title;
        params["RefCtrl"] = RefCtrl;
        params["RefCtrlHidden"] = RefCtrlHidden;
        params["RefCtrlLabel"] = RefCtrlLabel;
        params["RefCtrlLink"] = RefCtrlLink;
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'clinicalTabNotes';
        LoadActionPan('Admin_Provider', params);
    },

    HideProviderLink: function (value) {
        if (value == "") {
            $('#pnlClinicalNotes #txtProvider').attr("ProviderId", "-1");
            $('#pnlClinicalNotes #hfProvider').val("-1");
            $("#pnlClinicalNotes #lnkProviderEdit").css("display", "none");
            $("#pnlClinicalNotes #lblProvider").css("display", "inline");
        }
    },
    // -------------- End Provider -----------------

    // -------------- Facility ---------------------

    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmClinicalNotes";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "clinicalTabNotes";
        LoadActionPan('Admin_Facility', params);
    },

    OpenFacilityDetail: function () {

        var params = [];
        params["FacilityId"] = $('#pnlClinicalNotes #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'clinicalTabNotes';
        params["RefCtrl"] = "txtFacility";
        LoadActionPan('facilityDetail', params);
    },

    HideFacilityLink: function (value) {
        if (value == "") {
            $('#pnlClinicalNotes #txtFacility').attr("FacilityId", "-1");
            $('#pnlClinicalNotes #hfFacility').val("-1");
            $('#pnlClinicalNotes #lnkFacilityEdit').css("display", "none");
            $('#pnlClinicalNot0es #lblFacility').css("display", "inline");
        }
    },
    // -------------- End Facility -----------------

    // -------------- Ref Provider -----------------

    FillRefProviderName: function (RefProviderId, RefProviderName) {
        $('#pnlClinicalNotes #txtRefProvider').val(RefProviderName);
        $('#pnlClinicalNotes #hfRefProvider').val(RefProviderId);
        UnloadActionPan(Admin_ReferringProvider.params["ParentCtrl"]);
    },

    OpenRefProvider: function () {
        var params = [];
        params["ReferringProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtRefProvider";
        params["ParentCtrl"] = "clinicalTabNotes";
        LoadActionPan('Admin_ReferringProvider', params);
    },

    OpenRefProviderDetail: function () {

        var params = [];
        params["ReferringProviderId"] = $('#pnlClinicalNotes #hfRefProvider').val();
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtRefProvider";
        params["mode"] = "Edit";
        params["ParentCtrl"] = "clinicalTabNotes";

        LoadActionPan('referringproviderDetail', params);
    },

    HideRefProviderLink: function (value) {
        if (value == "") {
            $('#pnlClinicalNotes #hfRefProvider').val("-1");
            $('#pnlClinicalNotes #lnkRefProviderEdit').css("display", "none");
            $('#pnlClinicalNotes #lblRefProvider').css("display", "inline");
        }
    },
    // -------------- End Ref Provider -------------

    LoadPatientDemogrphic: function () {

        AppPrivileges.GetFormPrivileges("Demographic", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                $('#pnlDemographic  #frmDemographic').resetAllControls();
                if (Patient_Demographic.params.mode == "Add") {

                    Patient_Demographic.FillPatientInfo(params).done(function () {
                        if ($('#pnlDemographic #lnkProviderEdit').css("display") == "none") {
                            $('#pnlDemographic #lnkProviderEdit').css("display", "inline");
                            $('#pnlDemographic #lblProvider').css("display", "none");
                        }
                        if ($('#pnlDemographic #lnkFacilityEdit').css("display") == "none") {
                            $('#pnlDemographic #lnkFacilityEdit').css("display", "inline");
                            $('#pnlDemographic #lblFacility').css("display", "none");
                        }
                        if ($('#pnlDemographic #hfRefProvider').val() != "") {
                            $('#pnlDemographic #lnkRefProviderEdit').css("display", "inline");
                            $('#pnlDemographic #lblRefProvider').css("display", "none");
                        }
                        else {
                            $('#pnlDemographic #lblRefProvider').css("display", "inline");
                            $('#pnlDemographic #lnkRefProviderEdit').css("display", "none");
                        }

                        if ($('#pnlDemographic #hfPCP').val() != "") {
                            $('#pnlDemographic #lnkPCPEdit').css("display", "inline");
                            $('#pnlDemographic #lblPCP').css("display", "none");
                        }
                        else {
                            $('#pnlDemographic #lblPCP').css("display", "inline");
                            $('#pnlDemographic #lnkPCPEdit').css("display", "none");
                        }

                        if ($('#pnlDemographic #hfGuarantor').val() != "") {
                            $('#pnlDemographic #lnkGuarantorEdit').css("display", "inline");
                            $('#pnlDemographic #lblGuarantor').css("display", "none");
                        }
                        else {
                            $('#pnlDemographic #lblGuarantor').css("display", "inline");
                            $('#pnlDemographic #lnkGuarantorEdit').css("display", "none");
                        }
                        //serialize data
                        $('#frmDemographic').data('serialize', $('#frmDemographic').serialize());

                    });
                }
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });

    },

    GetNoteInfo: function (NoteId, Obj, createdDate) {

        if (Obj != null) {
            if ($(Obj.target).is('i[class*="fa-clipboard"]')) {
                Obj.stopPropagation();
            }
        }

        var visDate = new Date(createdDate);
        var cutOffDate = new Date('05/09/2017');
        if (visDate < cutOffDate) {
            utility.DisplayMessages("Provider Note created before May 09, 2017 cannot be copied!", 2);

        } else {
            Clinical_ProgressNote.FillNotes(null, NoteId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {

                    var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);

                    var FormId = "pnlClinicalNotes";
                    $.when(Clinical_Notes.GetRooms(null, Clinical_Notes_detail.FacilityId)).done(function () {//2 Buss Logic

                        var self = $('#' + FormId);

                        // When User comes from Scheduler to create Note these values reset by FillNote Call which cause issues.
                        // That's why these are reset with Check. EMR-7232
                        if (Clinical_Notes_detail.LinkedAppointment == "" && $("#" + FormId + " #txtLinkedAppointment").val())
                        {
                            Clinical_Notes_detail.LinkedAppointment = $("#" + FormId + " #txtLinkedAppointment").val();
                        }
                        if (Clinical_Notes_detail.IsLinkedAppointment == false && $("#" + FormId + " #ChkBox_LinkedAppointment").prop('checked'))
                        {
                            Clinical_Notes_detail.IsLinkedAppointment = $("#" + FormId + " #ChkBox_LinkedAppointment").prop('checked');
                        }

                        utility.bindMyJSONByName(true, Clinical_Notes_detail, false, self);
                        Clinical_Notes.bindDateAndTimepicker();
                        if ($('#pnlClinicalNotes #frmClinicalNotes').data("bootstrapValidator") != null && typeof $('#frmClinicalNotes').data('bootstrapValidator') != 'undefined') {
                            $("#" + FormId + " #frmClinicalNotes").bootstrapValidator('revalidateField', 'NoteType');
                        }

                        //    Clinical_Notes.bindDateAndTimepicker();//1 Buss Logic
                        $("#pnlClinicalNotes #providerDiv").addClass("disableAll");//3 Buss Logic


                        $("#pnlClinicalNotes #CopyNoteText").html("");
                        //$("#pnlClinicalNotes #CopyNoteText").append("");//start 13 Buss Logic
                        //$("#pnlClinicalNotes #CopyNoteText").find("section[id*='Cli_Prescription_Main']").remove();
                        //$("#pnlClinicalNotes #CopyNoteText").find("section[id*='Cli_LabOrderDetail_Main']").remove();

                        //$("#pnlClinicalNotes #CopyNoteText").find("section[id*='Cli_ProcedureOrderDetail_Main']").remove();
                        //$("#pnlClinicalNotes #CopyNoteText").find("section[id*='Cli_RadiologyOrderDetail_Main']").remove();

                        //$("#pnlClinicalNotes #CopyNoteText").find("section[id*='Cli_ConsultationOrderDetail_Main']").remove();
                        //$("#pnlClinicalNotes #CopyNoteText").find("section[id*='Cli_FollowUp_Main']").remove();

                        HTMLNotes = $("#pnlClinicalNotes #CopyNoteText").html();
                        HTMLNotes = HTMLNotes.replace(/&quot;/g, '"');
                        HTMLNotes = HTMLNotes.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
                        HTMLNotes = HTMLNotes.replace(/&nbsp;/g, '');
                        $("#pnlClinicalNotes #hfNoteText").val(HTMLNotes);
                        //End 13 Buss Logic

                        $("#pnlClinicalNotes #hfPrevNotesId").val(NoteId);
                        Clinical_Notes.CopyNotes = true;
                        Clinical_Notes.PrevNoteId = NoteId;
                        $("#pnlClinicalNotes #hfVisitId").val("");
                        if (globalAppdata.IsSelectCompOnCopyNote.toLowerCase() == "true") {
                            Clinical_Notes.loadNoteComponentsName_DBCall(NoteId).done(function (response) {
                                response = JSON.parse(response);
                                if (response.NoteComponentsCount && response.NoteComponentsCount > 0) {
                                    Clinical_Notes.openCopyNoteComponent(NoteId, response.NoteComponents_JSON);
                                }
                                else {
                                    utility.DisplayMessages("No components to copy!", 3);
                                        SelectTab('clinicalTabNotes', 'false');
                                        ClinicalMenuClick(event, function () {
                                            ClinicalMenuSettings.selectClinicalMenu('clinicalMenuNotes');
                                        }, null, null, 'clinicalTabNotes', 'button');
                                }
                            })
                        }
                        else {

                            $('#' + Clinical_Notes.params.PanelID + ' #frmClinicalNotes').submit();

                        }
                    });

                }
            });
        }
    },
    loadNoteComponentsName_DBCall: function (NoteId) {
        var objData = new Object();
        objData["NotesId"] = NoteId;
        objData["commandType"] = "search_NoteComponentsName";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalNotes", "ClinicalNotes");
    },
    openCopyNoteComponent: function (NoteId, NoteComponents) {
        Clinical_Notes.params.NoteComponentList = [];
        var params = [];
        params["NoteId"] = NoteId;
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "clinicalTabNotes";
        params["NoteComponents"] = NoteComponents;
        LoadActionPan('Clinical_Copy_Note_Component_Selection', params);
    },
    checkSignRights: function (panelId, btnCtrl, btnCtrlBottom) {

        Clinical_Notes.canSignNote().done(function (canSign) {
            if (canSign) {
                $('#' + panelId + ' #' + btnCtrl).removeClass('hidden');
                if (btnCtrlBottom)
                    $('#' + panelId + ' #' + btnCtrlBottom).removeClass('hidden');
            }
            else {
                $('#' + panelId + ' #' + btnCtrl).addClass('hidden');
                if (btnCtrlBottom)
                    $('#' + panelId + ' #' + btnCtrlBottom).addClass('hidden');
            }
        });
    },

    canSignNote: function () {
        var def = $.Deferred();
        var canSign = true;

        AppPrivileges.GetFormPrivileges("Notes_Sign", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                canSign = true;
            }
            else {
                canSign = false;
            }
            def.resolve(canSign);
        });

        return def.promise();
    },

    checkCoSignRights: function (panelId, btnCtrl) {

        Clinical_Notes.canCoSignNote().done(function (canCoSign) {
            if (canCoSign) {
                $('#' + panelId + ' #' + btnCtrl).removeClass('hidden');
            }
            else {
                $('#' + panelId + ' #' + btnCtrl).addClass('hidden');
            }
        });
    },

    canCoSignNote: function () {
        var def = $.Deferred();
        var canCoSign = true;

        AppPrivileges.GetFormPrivileges("Notes_Co-Sign", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                canCoSign = true;
            }
            else {
                canCoSign = false;
            }
            def.resolve(canCoSign);
        });

        return def.promise();
    },

    DashBoardDefaultCheckBoxes: function () {

        Clinical_Notes.totalNotesCount = parseInt(Clinical_Notes.totalDraftNotesCount) + parseInt(Clinical_Notes.totalSignedNotesCount);

        if (Clinical_Notes.totalNotesCount > 0) {
            if (globalAppdata["IsPreviousNotePE"] == "True") {
                $('#' + Clinical_Notes.params["PanelID"] + ' #divchkPrevPE').removeClass('hidden');
                $('#' + Clinical_Notes.params["PanelID"] + ' #chkPrevNotePE').prop('checked', true);
                $('#' + Clinical_Notes.params["PanelID"] + ' #chkPrevNotePE').prop('disabled', false);
            } else {
                $('#' + Clinical_Notes.params["PanelID"] + ' #divchkPrevPE').addClass('hidden');
            }
            if (globalAppdata["IsPreviousNoteROS"] == "True") {
                $('#' + Clinical_Notes.params["PanelID"] + ' #divchkPrevROS').removeClass('hidden');
                $('#' + Clinical_Notes.params["PanelID"] + ' #chkPrevNoteROS').prop('checked', true);
                $('#' + Clinical_Notes.params["PanelID"] + ' #chkPrevNoteROS').prop('disabled', false);
            } else {
                $('#' + Clinical_Notes.params["PanelID"] + ' #divchkPrevROS').addClass('hidden');
            }
            if (globalAppdata["IsPreviousNoteComplaints"] == "True") {
                $('#' + Clinical_Notes.params["PanelID"] + ' #divchkPrevComplaints').removeClass('hidden');
                $('#' + Clinical_Notes.params["PanelID"] + ' #chkPrevComplaints').prop('checked', true);
                $('#' + Clinical_Notes.params["PanelID"] + ' #chkPrevComplaints').prop('disabled', false);
            } else {
                $('#' + Clinical_Notes.params["PanelID"] + ' #divchkPrevComplaints').addClass('hidden');
            }
            if (globalAppdata["IsPreviousNoteProblems"] == "True") {
                $('#' + Clinical_Notes.params["PanelID"] + ' #divchkPrevProblems').removeClass('hidden');
                $('#' + Clinical_Notes.params["PanelID"] + ' #chkPrevNoteProblems').prop('checked', true);
                $('#' + Clinical_Notes.params["PanelID"] + ' #chkPrevNoteProblems').prop('disabled', false);
            } else {
                $('#' + Clinical_Notes.params["PanelID"] + ' #divchkPrevProblems').addClass('hidden');
            }
        }
        else {

            if (globalAppdata["IsPreviousNotePE"] == "True") {
                $('#' + Clinical_Notes.params["PanelID"] + ' #divchkPrevPE').removeClass('hidden');
                $('#' + Clinical_Notes.params["PanelID"] + ' #chkPrevNotePE').prop('checked', false);
                $('#' + Clinical_Notes.params["PanelID"] + ' #chkPrevNotePE').prop('disabled', true);
            } else {
                $('#' + Clinical_Notes.params["PanelID"] + ' #divchkPrevPE').addClass('hidden');
            }
            if (globalAppdata["IsPreviousNoteROS"] == "True") {
                $('#' + Clinical_Notes.params["PanelID"] + ' #divchkPrevROS').removeClass('hidden');
                $('#' + Clinical_Notes.params["PanelID"] + ' #chkPrevNoteROS').prop('checked', false);
                $('#' + Clinical_Notes.params["PanelID"] + ' #chkPrevNoteROS').prop('disabled', true);
            } else {
                $('#' + Clinical_Notes.params["PanelID"] + ' #divchkPrevROS').addClass('hidden');
            }
            if (globalAppdata["IsPreviousNoteComplaints"] == "True") {
                $('#' + Clinical_Notes.params["PanelID"] + ' #divchkPrevComplaints').removeClass('hidden');
                $('#' + Clinical_Notes.params["PanelID"] + ' #chkPrevComplaints').prop('checked', false);
                $('#' + Clinical_Notes.params["PanelID"] + ' #chkPrevComplaints').prop('disabled', true);
            } else {
                $('#' + Clinical_Notes.params["PanelID"] + ' #divchkPrevComplaints').addClass('hidden');
            }
            if (globalAppdata["IsPreviousNoteProblems"] == "True") {
                $('#' + Clinical_Notes.params["PanelID"] + ' #divchkPrevProblems').removeClass('hidden');
                $('#' + Clinical_Notes.params["PanelID"] + ' #chkPrevNoteProblems').prop('checked', false);
                $('#' + Clinical_Notes.params["PanelID"] + ' #chkPrevNoteProblems').prop('disabled', true);
            } else {
                $('#' + Clinical_Notes.params["PanelID"] + ' #divchkPrevProblems').addClass('hidden');
            }
        }

    },

}
