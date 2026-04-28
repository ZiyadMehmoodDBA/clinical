/*
    Author: Muhammad Azhar Shahzad
    Creation Date: November 18,2015
    OverView:This File Is created for Clinical Notes
*/

Clinical_PhoneEncounter = {
    bIsFirstLoad: true,
    params: [],
    CopyNotes: false,
    PrevNoteId: 0,
    NewInsertTables: "",
    IsNoteAlreadyCreated: false,
    arrCQMReasoning: [],
    Load: function (params) {
        Clinical_Notes.IsNoteAlreadyCreated = false;
        $('#clinicalTabPhoneEncounter').addClass("active");
        $('#clinicalTabNotes').removeClass("active");
        Clinical_PhoneEncounter.params = params;
        Clinical_PhoneEncounter.CopyNotes = false;
        Clinical_PhoneEncounter.PrevNoteId = 0;
        Clinical_PhoneEncounter.NewInsertTables = "";

        //If Note is edited from Dashboard, Global Notes, Appointments, Schedular, than this condition check for editing notes
        if (Clinical_PhoneEncounter.params.ForProgressNote == true) {

            Clinical_PhoneEncounter.params["mode"] = "Edit";
            $('#' + Clinical_PhoneEncounter.params.PanelID + ' #frmClinicalPhoneEncounter').resetAllControls();
            $('#' + Clinical_PhoneEncounter.params.PanelID + ' #frmClinicalPhoneEncounter [type=hidden]').val('');
            $('#' + Clinical_PhoneEncounter.params.PanelID + ' #ProgressnoteHTML').html('');
            Clinical_PhoneEncounter.params.ForProgressNote = null;
            Clinical_PhoneEncounter.AddProgressNoteTab();

            return false;
        }

        var self = $('#pnlClinicalPhoneEncounter');
        if (Clinical_PhoneEncounter.params.PanelID != 'pnlClinicalPhoneEncounter') {
            self = $('#' + Clinical_PhoneEncounter.params.PanelID + " #pnlClinicalPhoneEncounter");
        }

        // var self = $('#' + Clinical_PhoneEncounter.params.PanelID);
        var PatientId = Clinical_PhoneEncounter.params.patientID;
        CacheManager.BindDropDownsByEntityID('#' + Clinical_PhoneEncounter.params["PanelID"] + ' #ddlDuration', 'GetPhoneEncounterDuration', false, PatientId);

        $('#' + Clinical_PhoneEncounter.params.PanelID + ' #frmClinicalPhoneEncounter').resetAllControls();
        $('#' + Clinical_PhoneEncounter.params.PanelID + ' #frmClinicalPhoneEncounter [type=hidden]').val('');
        $('#' + Clinical_PhoneEncounter.params.PanelID + ' #ProgressnoteHTML').html('');

        //removing attribute of Note Type and Note template, so that it will not make request with self.loaddropdown
        self.find('.NoteType > select').removeAttr('ddlist');
        self.find('.NoteTemplate > select').removeAttr('ddlist');

        if (Clinical_PhoneEncounter.bIsFirstLoad) {

            Clinical_PhoneEncounter.bIsFirstLoad = false;

            //Load all autocompletes for this form (Ref Providers, Providers, Facility)
            Clinical_PhoneEncounter.LoadAllAutocomplete();
        }

        Clinical_PhoneEncounter.ValidateNotes('pnlClinicalPhoneEncounter #frmClinicalPhoneEncounter');
        Clinical_PhoneEncounter.bindDateAndTimepicker();
        Clinical_PhoneEncounter.params.mode = "Add";

        var ParentCntrlLoadid = Clinical_PhoneEncounter.params["ParentCntrlLoadid"] || null;

        Clinical_PhoneEncounter.LoadNoteGrid().done(function () {

            if (Clinical_PhoneEncounter.bIsFirstLoad && ParentCntrlLoadid == "Schedular") {
                Scheduling_Calendar.isVisitUpdForAppointment = true;
            }

            if (ParentCntrlLoadid == "Schedular" || ParentCntrlLoadid == "Dashboard") {

                //If call is from Dashboard, Global Notes, Appointments or Schedular, than this will load default values to Notes
                Clinical_PhoneEncounter.LoadNotesDefaults();
                $('#' + Clinical_PhoneEncounter.params.PanelID + ' #providerDiv').addClass('disableAll');
                Clinical_PhoneEncounter.params["ParentCntrlLoadid"] = null;

            } else {

                $('#' + Clinical_PhoneEncounter.params.PanelID + ' #providerDiv').removeClass('disableAll');

                // Setting Default room to Lobby
                var LobbyId = $('#' + Clinical_PhoneEncounter.params.PanelID + ' #frmClinicalPhoneEncounter #RoomNo option').filter(function () { return $(this).html() == "Lobby"; }).val();
                if (LobbyId != null && LobbyId != '') {
                    $('#' + Clinical_PhoneEncounter.params.PanelID + ' #frmClinicalPhoneEncounter #RoomNo').val(LobbyId);
                }
            }

            if (Clinical_PhoneEncounter.params.CCMDuration) {

                var Duration = Clinical_PhoneEncounter.params.CCMDuration;
                Clinical_PhoneEncounter.setDurationTimeOnNote(Duration, Clinical_PhoneEncounter.params["PanelID"]);

                $('#pnlClinicalPhoneEncounter #ddlEncounterType').val(3);
                $('#pnlClinicalPhoneEncounter #txtVisitReason').val(Clinical_PhoneEncounter.params.VisitReason);
                $('#pnlClinicalPhoneEncounter #txtCaller').val(Clinical_PhoneEncounter.params.Caller);
                $('#pnlClinicalPhoneEncounter #txtReceiver').val(Clinical_PhoneEncounter.params.Receiver);
                $("#DivDuration input").attr('disabled', true);

                // fixMe
                if ($('#pnlClinicalPhoneEncounter #txtVisitReason').val() == "") {
                    $('#pnlClinicalPhoneEncounter #txtVisitReason').val("CCM Program");
                    $('#pnlClinicalPhoneEncounter #ddlEncounterType').val(3);
                }

                // fixMe
                if (!$('#pnlClinicalPhoneEncounter #ddlEncounterType').val())
                    $('#pnlClinicalPhoneEncounter #ddlEncounterType').val(3);
            }
            else {
                Clinical_PhoneEncounter.ResetTaskTime();
            }

            $('#pnlClinicalPhoneEncounter #frmClinicalPhoneEncounter #txtUser').val(globalAppdata.AppUserNameFullName);
            $('#pnlClinicalPhoneEncounter #frmClinicalPhoneEncounter #hfUserId').val(globalAppdata.AppUserId);

            Clinical_PhoneEncounter.GetNoteTemplateType($('#' + Clinical_PhoneEncounter.params.PanelID + ' #frmClinicalPhoneEncounter #ddlNoteType'));
        });

        // Unlinking Appointments and Previous Note
        Clinical_PhoneEncounter.UnlinkAppointment($("#pnlClinicalPhoneEncounter #ChkBox_LinkedAppointment"), 'pnlClinicalPhoneEncounter', true);
        Clinical_PhoneEncounter.UnLinkPreviousNotePatient($("#pnlClinicalPhoneEncounter #chkCopayPreviousNote"), 'pnlClinicalPhoneEncounter');



        //Adding Notes Tab
        Clinical_PhoneEncounter.AddProgressNoteTab(true);

        //Hide Delete button if user does not have the rights
        if (localStorage.getItem('IsNoteRightCollasped') && localStorage.getItem('IsNoteRightCollasped') == 'true') {
            $('html').addClass('sidebar-left-collapsed')
        }
    },

    setDurationTimeOnNote: function (Duration, PanelID) {
        Clinical_ProgressNote.params.CCMDuration = Duration;
        if (Duration) {
            Duration = Duration.split(':');
        }

        var self = $("#" + PanelID + " #DivDuration");

        if (Duration != null) {

            if (Duration.length > 2) {
                self.find("#txtTaskHours").val(Duration[0]);
                self.find("#txtTaskMinutes").val(Duration[1]);
                self.find("#txtTaskSeconds").val(Duration[2]);
            }
            else {
                if (Duration.length > 1) {

                    // Change to Hours and Minutes
                    var min = parseInt(Duration[0]);
                    if (min > 59) {
                        var minut = min % 60;
                        self.find("#txtTaskMinutes").val(minut);
                        self.find("#txtTaskHours").val(function (_, v) {
                            return Math.floor(min / 60);
                        });
                    }
                    else {
                        self.find("#txtTaskHours").val(0);
                        self.find("#txtTaskMinutes").val(Duration[0]);
                    }

                    self.find("#txtTaskSeconds").val(Duration[1]);
                }
                else {
                    if (Duration.length > 0) {

                        self.find("#txtTaskHours").val(0);
                        self.find("#txtTaskMinutes").val(0);
                        self.find("#txtTaskSeconds").val(Duration[0]);
                    }
                }
            }
        }
    },


    ResetTaskTime: function () {
        var self = $("#" + Clinical_PhoneEncounter.params["PanelID"] + " #DivDuration");

        self.find("#txtTaskHours").val(0);
        self.find("#txtTaskMinutes").val(0);
        self.find("#txtTaskSeconds").val(0);

    },

    bindDateAndTimepicker: function () {

        utility.CreateDatePicker('pnlClinicalPhoneEncounter #frmClinicalPhoneEncounter #dtpVisitDate',
             //on-change callback method
             function (ev) {
                 if ($('#pnlClinicalPhoneEncounter #frmClinicalPhoneEncounter').data("bootstrapValidator") != null && typeof $('#frmClinicalPhoneEncounter').data('bootstrapValidator') != 'undefined') {
                     $('#pnlClinicalPhoneEncounter #frmClinicalPhoneEncounter').bootstrapValidator('revalidateField', 'VisitDate');
                 }
             }, true);

        //on date change, validating time
        $('#pnlClinicalPhoneEncounter #frmClinicalPhoneEncounter #VisitTime').timepicker().on('changeTime.timepicker', function (e) {
            disableFocus: false;
            if ($('#pnlClinicalPhoneEncounter #frmClinicalPhoneEncounter').data("bootstrapValidator") != null && typeof $('#frmClinicalPhoneEncounter').data('bootstrapValidator') != 'undefined') {
                $('#pnlClinicalPhoneEncounter #frmClinicalPhoneEncounter').bootstrapValidator('revalidateField', 'VisitTime');
            }
        });
        $('#pnlClinicalPhoneEncounter #frmClinicalPhoneEncounter #VisitTime').timepicker({
            defaultTime: 'now',
            minuteStep: 5//,
        });
        $('#pnlClinicalPhoneEncounter #frmClinicalPhoneEncounter #VisitTime').timepicker('setTime', new Date());
    },
    //If call is from Dashboard, Global Notes, Appointments or Schedular, than this will load default values to Notes
    LoadNotesDefaults: function () {
        var LobbyId = $('#pnlClinicalPhoneEncounter #frmClinicalPhoneEncounter #RoomNo option').filter(function () { return $(this).html() == "Lobby"; }).val();
        if (LobbyId != null) {
            $('#pnlClinicalPhoneEncounter #frmClinicalPhoneEncounter #RoomNo').val(LobbyId);
        }

        if (Clinical_PhoneEncounter.params != null && Object.keys(Clinical_PhoneEncounter.params).length > 0) {
            $('#pnlClinicalPhoneEncounter #frmClinicalPhoneEncounter #hfPatientId').val(Clinical_PhoneEncounter.params["PatientId"]);
            if (Clinical_PhoneEncounter.params["NotesVisitId"] != null) {
                var AppointmentId = Clinical_PhoneEncounter.params["AppointmentId"];
                var NotesVisitId = Clinical_PhoneEncounter.params["NotesVisitId"];
                var NotesVisitDate = Clinical_PhoneEncounter.params["NotesVisitDate"];
                var NotesVisitTime = Clinical_PhoneEncounter.params["NotesVisitTime"];
                var NotesProviderId = Clinical_PhoneEncounter.params["NotesProviderId"];
                var NotesProviderName = Clinical_PhoneEncounter.params["NotesProviderName"];
                var NotesFacilityName = Clinical_PhoneEncounter.params["NotesFacilityName"];
                var NotesFacilityId = Clinical_PhoneEncounter.params["NotesFacilityId"];
                var ScheduleReason = Clinical_PhoneEncounter.params["ScheduleReason"];
                var NotesRoom = Clinical_PhoneEncounter.params["NotesRoom"];
                var RefProviderId = Clinical_PhoneEncounter.params["RefProviderId"];
                var RefProviderName = Clinical_PhoneEncounter.params["RefProviderName"];
                Clinical_PhoneEncounter.BindVisitsValues(AppointmentId, NotesVisitId, NotesVisitDate, NotesVisitTime, NotesProviderId, NotesProviderName, NotesFacilityName, NotesFacilityId, ScheduleReason, NotesRoom, RefProviderId, RefProviderName, false);
                Clinical_PhoneEncounter.VisitSearch(Clinical_PhoneEncounter.params["NotesVisitId"], Clinical_PhoneEncounter.params["patientID"]);

            }
            Clinical_PhoneEncounter.UnLinkPreviousNotePatient($("#pnlClinicalPhoneEncounter #chkCopayPreviousNote"), 'pnlClinicalPhoneEncounter');
            Clinical_PhoneEncounter.UnlinkAppointment($("#pnlClinicalPhoneEncounter #ChkBox_LinkedAppointment"), 'pnlClinicalPhoneEncounter', true);


        }

    },

    //ON page load, if paramaters has visit id, than data of that visit is loading by default in controls
    BindVisitsValues: function (AppointmentId, NotesVisitId, NotesVisitDate, NotesVisitTime, NotesProviderId, NotesProviderName, NotesFacilityName, NotesFacilityId, ScheduleReason, NotesRoom, RefProviderId, RefProviderName, IsBindVisitInfo) {
        $('#' + Clinical_PhoneEncounter.params.PanelID + ' #hfAppointmentId').val(AppointmentId);
        $('#' + Clinical_PhoneEncounter.params.PanelID + ' #hfVisitId').val(NotesVisitId);

        if (!IsBindVisitInfo) {
            $('#' + Clinical_PhoneEncounter.params.PanelID + ' #dtpVisitDate').val(NotesVisitDate);
            $('#' + Clinical_PhoneEncounter.params.PanelID + ' #dtpVisitDate').datepicker('setDate', NotesVisitDate)
            $('#' + Clinical_PhoneEncounter.params.PanelID + ' #VisitTime').val(NotesVisitTime);
            $('#' + Clinical_PhoneEncounter.params.PanelID + ' #VisitTime').timepicker('setTime', NotesVisitTime);
            if (NotesVisitTime != '' && NotesVisitDate != '' && NotesVisitTime != null && NotesVisitDate != null) {
                $('#' + Clinical_PhoneEncounter.params.PanelID + ' #txtLinkedAppointment').val(NotesVisitDate + " " + NotesVisitTime);

                $('#' + Clinical_PhoneEncounter.params.PanelID + ' #ChkBox_LinkedAppointment').prop('checked', true);
            } else {
                $('#' + Clinical_PhoneEncounter.params.PanelID + ' #ChkBox_LinkedAppointment').prop('checked', false);
                $('#' + Clinical_PhoneEncounter.params.PanelID + ' #txtLinkedAppointment').val('No Appointment Selected');
                $('#' + Clinical_PhoneEncounter.params.PanelID + ' #VisitTime').timepicker('setTime', new Date());
                var date_format = 'dd/mm/yyyy';
                //set default Date Formate
                if (globalAppdata['DateFormat'])
                    date_format = globalAppdata['DateFormat'];
                $('#' + Clinical_PhoneEncounter.params.PanelID + ' #dtpVisitDate').datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), new Date()));
            }

            if (ScheduleReason != null) {
                var VisitReasonId = $('#' + Clinical_PhoneEncounter.params.PanelID + ' #VisitReason option').filter(function () { return $(this).html() == ScheduleReason; }).val();
                if (VisitReasonId != null && VisitReasonId != '') {
                    $('#' + Clinical_PhoneEncounter.params.PanelID + ' #VisitReason').val(VisitReasonId);
                }
            }
            if (NotesRoom != null) {
                var RoomNoId = $('#' + Clinical_PhoneEncounter.params.PanelID + ' #RoomNo option').filter(function () { return $(this).html() == NotesRoom; }).val();
                if (RoomNoId != null && RoomNoId != '') {
                    $('#' + Clinical_PhoneEncounter.params.PanelID + ' #RoomNo').val(RoomNoId);
                }
            }
        }
        if ((NotesProviderName != null && NotesProviderId != null) && (NotesProviderName != '' && NotesProviderId != '')) {
            $('#' + Clinical_PhoneEncounter.params.PanelID + ' #txtProvider').val(NotesProviderName);
            $('#' + Clinical_PhoneEncounter.params.PanelID + ' #hfProvider').val(NotesProviderId);
            if (Clinical_PhoneEncounter.params.PanelID != 'pnlClinicalProgressNote') {
                $('#' + Clinical_PhoneEncounter.params.PanelID + ' #lnkProviderEdit').css("display", "inline");
                $('#' + Clinical_PhoneEncounter.params.PanelID + ' #lblProvider').css("display", "none");
            }
        } else {
            if ($('#' + Clinical_PhoneEncounter.params.PanelID + ' #txtProvider').val() == '') {
                if (Clinical_PhoneEncounter.params.PanelID != 'pnlClinicalProgressNote') {
                    $('#' + Clinical_PhoneEncounter.params.PanelID + ' #lnkProviderEdit').css("display", "none");
                    $('#' + Clinical_PhoneEncounter.params.PanelID + ' #lblProvider').css("display", "inline");
                }
            }

        }
        if ((NotesFacilityId != null && NotesFacilityName != null) && (NotesFacilityId != '' && NotesFacilityName != '')) {
            $('#' + Clinical_PhoneEncounter.params.PanelID + ' #txtFacility').val(NotesFacilityName);
            $('#' + Clinical_PhoneEncounter.params.PanelID + ' #hfFacility').val(NotesFacilityId);
            if (Clinical_PhoneEncounter.params.PanelID != 'pnlClinicalProgressNote') {
                $('#' + Clinical_PhoneEncounter.params.PanelID + ' #lnkFacilityEdit').css("display", "inline");
                $('#' + Clinical_PhoneEncounter.params.PanelID + ' #lblFacility').css("display", "none");
            }
        } else {
            if ($('#' + Clinical_PhoneEncounter.params.PanelID + ' #txtFacility').val() == '') {
                if (Clinical_PhoneEncounter.params.PanelID != 'pnlClinicalProgressNote') {
                    $('#' + Clinical_PhoneEncounter.params.PanelID + ' #lnkFacilityEdit').css("display", "none");
                    $('#' + Clinical_PhoneEncounter.params.PanelID + ' #lblFacility').css("display", "inline");
                }
            }
        }

        if ((RefProviderId != null && RefProviderName != null) && (RefProviderId != '' && RefProviderName != '')) {
            $('#' + Clinical_PhoneEncounter.params.PanelID + ' #txtRefProvider').val(RefProviderName);
            $('#' + Clinical_PhoneEncounter.params.PanelID + ' #hfRefProvider').val(RefProviderId);
            $('#' + Clinical_PhoneEncounter.params.PanelID + ' #lnkRefProviderEdit').css("display", "inline");
            $('#' + Clinical_PhoneEncounter.params.PanelID + ' #lblRefProvider').css("display", "none");
        } else {
            if ($('#' + Clinical_PhoneEncounter.params.PanelID + ' #txtRefProvider').val() == '') {
                $('#' + Clinical_PhoneEncounter.params.PanelID + ' #lnkRefProviderEdit').css("display", "none");
                $('#' + Clinical_PhoneEncounter.params.PanelID + ' #lblRefProvider').css("display", "inline");
            }
        }
        //Getting User Rooms for selected facility of Patient
        if (NotesFacilityId && NotesFacilityId != '') {
            Clinical_PhoneEncounter.GetRooms($('#' + Clinical_PhoneEncounter.params.PanelID + ' #txtFacility'), NotesFacilityId);
        }

    },

    // Function to get visit information on basses of Patient & Visit
    VisitSearch: function (VisitId, patientID) {

        var myJSON = null;
        Encounter_Visits.SearchVisits(myJSON, patientID, VisitId).done(function (response) {
            if (response.status != false) {
                if (response.VisitsCount > 0) {
                    var VisitsLoadJSONData = JSON.parse(response.VisitsLoad_JSON)[0];
                    var AppointmentId = VisitsLoadJSONData.AppointmentId;
                    var NotesVisitId = VisitsLoadJSONData.VisitId;
                    var NotesVisitDate = null;// VisitsLoadJSONData.VisitDate;
                    var NotesVisitTime = null;// VisitsLoadJSONData.VisitTime;
                    var NotesProviderId = VisitsLoadJSONData.ProviderId;
                    var NotesProviderName = VisitsLoadJSONData.ProviderName;
                    var NotesFacilityName = VisitsLoadJSONData.FacilityName;
                    var NotesFacilityId = VisitsLoadJSONData.FacilityId;
                    var ScheduleReason = null;
                    var NotesRoom = null;
                    var RefProviderId = VisitsLoadJSONData.RefProviderId;
                    var RefProviderName = VisitsLoadJSONData.ReferrringProviderName;
                    Clinical_PhoneEncounter.BindVisitsValues(AppointmentId, NotesVisitId, NotesVisitDate, NotesVisitTime, NotesProviderId, NotesProviderName, NotesFacilityName, NotesFacilityId, ScheduleReason, NotesRoom, RefProviderId, RefProviderName, true);
                    $('#pnlClinicalPhoneEncounter #frmClinicalPhoneEncounter #VisitReason').val(VisitsLoadJSONData.SchReasonId);
                    $('#pnlClinicalPhoneEncounter #frmClinicalPhoneEncounter #RoomNo').val(VisitsLoadJSONData.RoomsId);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    //Binding Validation Functionk
    ValidateNotes: function (FrmCtrl) {
        $('#' + FrmCtrl).bootstrapValidator('destroy');
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
                  },
                  //Duration: {
                  //    group: '.col-sm-4',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        },
                  //    }
                  //},
                  User: {
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
           if ($("#" + Clinical_PhoneEncounter.params["PanelID"] + " #frmClinicalPhoneEncounter #ddlNoteType option:selected").text().replace(' ', '').toLowerCase() == "phoneencounter") {
               $.when(Clinical_PhoneEncounter.saveNote()).done(function () {
                   Clinical_NotesSearch.SetNotesCount();
               });
           } else {
               utility.DisplayMessages("Note Type is Mandatory", 3);
           }
       });
    },

    UpdateSoapText_VitalInNotes: function (NotesId, VitalSignsId) {
        Clinical_ProgressNote.FillNotes(null, NotesId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);
                if (Clinical_Notes_detail.NoteText == null || Clinical_Notes_detail.NoteText == '') {
                    $('#' + Clinical_Vitals.params["PanelID"] + ' #ProgressnoteHTML').html('<ul class="initialVisit" id="ProgressNoteComponentList"></ul>');
                    $('#' + Clinical_Vitals.params["PanelID"] + ' #ProgressnoteHTML').html('');
                } else {
                    $('#' + Clinical_Vitals.params["PanelID"] + ' #ProgressnoteHTML').html(Clinical_Notes_detail.NoteText);
                }
                Clinical_Vitals.Get_Vitalsigns_ForSOAP(VitalSignsId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        if (response.VitalSignSoapCount > 0) {
                            Clinical_PhoneEncounter.CreateVitalBodyHTML_VitalInNotes(response, '#' + Clinical_Vitals.params["PanelID"] + ' #ProgressnoteHTML', NotesId);
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
        });

        Clinical_ProgressNote.updateProgressNoteHTML_DBCALL(null, Clinical_ProgressNote.params.NotesId == null ? NotesId : Clinical_ProgressNote.params.NotesId, $(NoteHTMLCtrl).html()).done(function (response) {
            response = JSON.parse(response);
            $(NoteHTMLCtrl).html('');
            if (response.status != false) {
                utility.DisplayMessages(response.Message, 1);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    CopyPreviousNoteHTML: function (PreviousNoteHTML, NotesId) {

    },


    // Add or Update Notes, this function is called from Notes And Progress Note
    saveNote: function ($dtpVisitDate, FromNotes) {
        var dfd = $.Deferred();
        var Duration = "0:0:0";
        var self = $("#" + Clinical_PhoneEncounter.params["PanelID"]);
        if (FromNotes && FromNotes != null) {
            var Duration = $('#pnlClinicalProgressNote #txtTaskHours').val() + ":" + $('#pnlClinicalProgressNote #txtTaskMinutes').val() + ":" + $('#pnlClinicalProgressNote #txtTaskSeconds').val();

        } else {
            Duration = $('#' + Clinical_PhoneEncounter.params.PanelID + ' #txtTaskHours').val() + ":" + $('#' + Clinical_PhoneEncounter.params.PanelID + ' #txtTaskMinutes').val() + ":" + $('#' + Clinical_PhoneEncounter.params.PanelID + ' #txtTaskSeconds').val();

        }

        Clinical_PhoneEncounter.params.CalculatedDuration = Duration;
        var visit_date = "";

        if ($dtpVisitDate)
            visit_date = $dtpVisitDate.val();
        else
            visit_date = $("#" + Clinical_PhoneEncounter.params.PanelID + " #frmClinicalPhoneEncounter").find("#dtpVisitDate").val();

        var current_date = $.datepicker.formatDate('mm/dd/yy', new Date());

        var objDeffered = $.Deferred();
        if (utility.DateCompare(visit_date, current_date) == 1) {
            utility.myConfirm("The note that you are creating is for a future date", function () {
                objDeffered.resolve("ok");
            }, function () { }, "Confirmation Alert", 'Ok', null, true);

        }
        else
            objDeffered.resolve("ok");

        objDeffered.done(function (message) {

            if (Clinical_PhoneEncounter.params.mode == "Add") {
                if (Clinical_PhoneEncounter.params.FromCCM) {
                    $("#pnlClinicalPhoneEncounter #EitherCCM").show();
                    if (Duration == "::" && Duration == "0:0:0") {
                        utility.DisplayMessages('Please fill Duration', 3);
                        dfd.resolve();
                    }
                }
                else
                    $("#pnlClinicalPhoneEncounter #EitherCCM").hide();

                //if (Duration != "::" && Duration != "0:0:0") {
                var ArrayNoteDate = new Array(); var ArrayNoteType = new Array();
                $("#dgvClinicalDraftPhoneEncounter tr td:nth-child(3)").each(function (i) { ArrayNoteDate.push($(this).text()); });
                $("#dgvClinicalSignedPhoneEncounter tr td:nth-child(3)").each(function (i) { ArrayNoteDate.push($(this).text()); });
                $("#dgvClinicalDraftPhoneEncounter tr td:nth-child(9)").each(function (i) { ArrayNoteType.push($(this).text()); });
                $("#dgvClinicalSignedPhoneEncounter tr td:nth-child(6)").each(function (i) { ArrayNoteType.push($(this).text()); });

                Clinical_PhoneEncounter.SearchNotes(0, null, null, 'Signed').done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        if (response.ClinicalNotesCount > 0) {
                            if (response.NotesLoad_JSON != "") {
                                var NotesLoadJSONData = JSON.parse(response.NotesLoad_JSON);
                                $.each(NotesLoadJSONData, function (i, item) {
                                    ArrayNoteDate.push(utility.RemoveTimeFromDate(null, item.VisitDate));
                                    ArrayNoteType.push(item.NoteTempType);
                                });
                            }
                        }

                        Clinical_PhoneEncounter.SearchNotes(0, null, null, 'Draft').done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                if (response.ClinicalNotesCount > 0) {
                                    if (response.NotesLoad_JSON != "") {
                                        var NotesLoadJSONData = JSON.parse(response.NotesLoad_JSON);
                                        $.each(NotesLoadJSONData, function (i, item) {
                                            ArrayNoteDate.push(utility.RemoveTimeFromDate(null, item.VisitDate));
                                            ArrayNoteType.push(item.NoteTempType);
                                        });
                                    }
                                }
                                if ((ArrayNoteDate.indexOf($("#frmClinicalPhoneEncounter #dtpVisitDate").val())) != -1 && (ArrayNoteType.indexOf($("#frmClinicalPhoneEncounter #ddlNoteType").text().replace("- Select -", ""))) != -1) {

                                    Clinical_PhoneEncounter.saveNoteAddCase(dfd, true, Duration);
                                }
                                else {
                                    var myJSON = self.getMyJSONByName();
                                    Clinical_PhoneEncounter.NotesSave(myJSON, Clinical_PhoneEncounter.params.patientID, Duration, false).done(function (response) {
                                        response = JSON.parse(response);
                                        if (response.status != false && response.NotesId > 0) {
                                            Clinical_PhoneEncounter.params["NotesId"] = response.NotesId;

                                            Clinical_PhoneEncounter.params["mode"] = "Edit";

                                            if (response.MUAlertsCount && parseInt(response.MUAlertsCount) > 0)
                                                utility.toggelMU3Alerts(true, parseInt(response.MUAlertsCount));

                                            if ($('#pnlClinicalPhoneEncounter #ddlEncounterType').val() == 4) {
                                                Clinical_ProgressNote.params["IsDateOfDischarge"] = true;
                                            } else {
                                                Clinical_ProgressNote.params["IsDateOfDischarge"] = false;
                                            }
                                            $('#' + Clinical_PhoneEncounter.params.PanelID + ' #frmClinicalPhoneEncounter').resetAllControls();
                                            $('#' + Clinical_PhoneEncounter.params.PanelID + ' #frmClinicalPhoneEncounter [type=hidden]').val('');
                                            $('#' + Clinical_PhoneEncounter.params.PanelID + ' #ProgressnoteHTML').html('');
                                            params['IsFromCreateNote'] = true;
                                            if (Clinical_PhoneEncounter.CopyNotes) {
                                                Clinical_PhoneEncounter.NewInsertTables = response.NewInsertTables;
                                            }
                                            Clinical_PhoneEncounter.AddProgressNoteTab();

                                            utility.DisplayMessages(response.Message, 1);
                                            dfd.resolve();
                                        }
                                        else {
                                            utility.DisplayMessages(response.Message, 3);
                                            dfd.resolve();
                                        }
                                    });
                                }
                            }
                        });
                        //
                    }
                });
                //} else {
                //utility.DisplayMessages('Please fill Duration', 3);
                //dfd.resolve();
                //}
                //End//22/12/2015//Ahmad Raza//Privileges Logic Implemented
            }
            else if (Clinical_PhoneEncounter.params.mode == "Edit") {
                if (Clinical_PhoneEncounter.params.FromCCM) {
                    $("#pnlClinicalPhoneEncounter #EitherCCM").removeClass('hidden');
                    if (Duration == "::" && Duration == "0:0:0") {
                        utility.DisplayMessages('Please fill Duration', 3);
                        dfd.resolve();
                    }
                }
                else
                    $("#pnlClinicalPhoneEncounter #EitherCCM").addClass('hidden');

                //if (Duration != "::" && Duration != "0:0:0") {
                $('#pnlClinicalProgressNote #hfNoteText').val($('#pnlClinicalProgressNote #ProgressnoteHTML').html());
                self = $("#pnlClinicalProgressNote");
                var myJSON = self.getMyJSONByName();
                Clinical_PhoneEncounter.NotesUpdate(myJSON, Clinical_PhoneEncounter.params.NotesId, Duration).done(function (response) {
                    response = JSON.parse(response);
                    Clinical_ProgressNote.params.triggerCount = 0;
                    if (response.status != false) {
                        if (Clinical_ProgressNote.params.IsPhoneEncounter && $('#' + Clinical_ProgressNote.params.PanelID + '  #frmClinicalProgressNote #btnBillingInfo').hasClass('disableAll')) {
                            SelectTab("clinicalTabProgressNote", "false");
                        }
                        //Clinical_PhoneEncounter.AddProgressNoteTab();
                        utility.DisplayMessages(response.Message, 1);
                        dfd.resolve();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                        dfd.resolve();
                    }
                });
                //} else {
                //    utility.DisplayMessages('Please fill Duration', 3);
                //    dfd.resolve();
                //}
                //End//22/12/2015//Ahmad Raza//Privileges Logic Implemented
            }
            Clinical_PhoneEncounter.params.FromCCM = "";

        });
        return dfd;
    },

    saveNoteAddCase: function (dfd, IsToCheckForTodaysNote, Duration) {
        var self = $('#' + Clinical_PhoneEncounter.params.PanelID);
        var myJSON = self.getMyJSONByName();
        Clinical_PhoneEncounter.NotesSave(myJSON, Clinical_PhoneEncounter.params.patientID, Duration, IsToCheckForTodaysNote).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false && response.NotesId > 0) {
                Clinical_PhoneEncounter.params["NotesId"] = response.NotesId;

                Clinical_PhoneEncounter.params["mode"] = "Edit";
                if ($('#pnlClinicalPhoneEncounter #ddlEncounterType').val() == 4) {
                    Clinical_ProgressNote.params["IsDateOfDischarge"] = true;
                } else {
                    Clinical_ProgressNote.params["IsDateOfDischarge"] = false;
                }
                $('#' + Clinical_PhoneEncounter.params.PanelID + ' #frmClinicalPhoneEncounter').resetAllControls();
                $('#' + Clinical_PhoneEncounter.params.PanelID + ' #frmClinicalPhoneEncounter [type=hidden]').val('');
                $('#' + Clinical_PhoneEncounter.params.PanelID + ' #ProgressnoteHTML').html('');
                params['IsFromCreateNote'] = true;
                if (Clinical_PhoneEncounter.CopyNotes) {
                    Clinical_PhoneEncounter.NewInsertTables = response.NewInsertTables;
                }
                Clinical_PhoneEncounter.AddProgressNoteTab();
                utility.DisplayMessages(response.Message, 1);
                dfd.resolve();
            }
            else if (response.IsTodaysNoteCreated) {

                utility.myConfirm('A Note already exists for the selected Visit Date. Do you still want to create another Note?', function () {
                    Clinical_PhoneEncounter.saveNoteAddCase(dfd, false, Duration);
                    Clinical_Notes.IsNoteAlreadyCreated = true;
                },
               function () {

               }, 'Confirm Action');

            }
            else {
                utility.DisplayMessages(response.Message, 3);
                dfd.resolve();
            }

        });

    },

    //Setting Default Data of Logged in User and Getting List of notes in Grid
    LoadNoteGrid: function () {
        var dfd = new $.Deferred();
        if (Clinical_PhoneEncounter.params.mode == "Add") {
            //EMR-651 fix change
            Clinical_PhoneEncounter.fillPatientIinfo_DBCall(Clinical_PhoneEncounter.params.patientID).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var demographic_detail = JSON.parse(response.DemographicFill_JSON);
                    Clinical_PhoneEncounter.BindVisitsValues(null, null, null, null, demographic_detail.ProviderID, demographic_detail.Provider, demographic_detail.Facility,
                                       demographic_detail.FacilityID, null, null, demographic_detail.RefProviderID, demographic_detail.RefProvider, true);

                    Clinical_PhoneEncounter.NotesDraftSearch(0, null, null, 'Draft');
                    Clinical_PhoneEncounter.NotesSignedSearch(0, null, null, 'Signed');

                    //serialize Data after all controls loaded.
                    $('#' + Clinical_PhoneEncounter.params.PanelID + ' #frmClinicalPhoneEncounter').data('serialize', $('#frmClinicalPhoneEncounter').serialize());
                    dfd.resolve('ok');
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                    dfd.resolve('ok');
                }
            });
        }
        else if (Clinical_PhoneEncounter.params.mode == "Edit") {
            utility.DisplayMessages('Edit Mode is called on Notes, Please Report bug!', 3);
            dfd.resolve('ok');
        }
        return dfd.promise();
    },


    // On Dropdown Changes Template, Call Back Function to Load Template Type
    GetTemplates: function (obj) {
        var self = $('#' + Clinical_PhoneEncounter.params.PanelID);
        if ($(obj).val() == "") {
            self.find('.NoteTemplate > select').val("");
        } else {
            if (self.find('.NoteTemplate > select').val() == "") {

                self.find('.NoteTemplate > select').attr('ddlist', 'GetTemplate');
                var data = "IsActive=&ID=" + $(obj).val();
                self.find('.NoteTemplate').loadDropDowns(true, data).done(function () {
                    // $("#" + Clinical_PhoneEncounter.params.PanelID + " #frmClinicalPhoneEncounter").bootstrapValidator('revalidateField', self.find('.NoteType > select').attr("name"));
                });
            }
        }
    },

    // On Dropdown Changes Template Type, Call Back Function to Load Template
    GetTemplatesType: function (obj) {
        var self = $('#' + Clinical_PhoneEncounter.params.PanelID);
        if ($(obj).val() == "") {
            self.find('.NoteType > select').val("");
        } else {
            if (self.find('.NoteType > select').val() == "") {
                self.find('.NoteType > select').attr('ddlist', 'GetNoteTemplateType');

                var data = "IsActive=&ID=&ID2=" + $(obj).val();

                self.find('.NoteType').loadDropDowns(true, data).done(function () {
                    self.find('.NoteType > select').val(self.find('.NoteType > select option[value!=""]:first').val());
                    //$("#" + Clinical_PhoneEncounter.params.PanelID + " #frmClinicalPhoneEncounter").bootstrapValidator('revalidateField', self.find('.NoteType > select').attr("name"));
                });
            }
        }
    },

    // On Dropdown Changes Template, Call Back Function to Load Template Type
    GetNotesTemplates: function (ProviderId) {
        setTimeout(function () {
            var self = $('#' + Clinical_PhoneEncounter.params.PanelID);
            if ($("#pnlClinicalPhoneEncounter #txtProvider").val() == '' && ProviderId == null) {
                self.find('.NoteTemplate > select').val("");
                self.find('.NoteTemplate').addClass('disableAll');
                // $("#" + Clinical_PhoneEncounter.params.PanelID + " #frmClinicalPhoneEncounter").bootstrapValidator('revalidateField', self.find('.NoteType > select').attr("name"));
            } else {
                if (ProviderId == null) {
                    ProviderId = $("#pnlClinicalPhoneEncounter #hfProvider").val();
                }

                if (ProviderId == "") {
                    self.find('.NoteTemplate > select').val("");
                    self.find('.NoteTemplate').addClass('disableAll');
                } else {
                    if (self.find('.NoteTemplate > select').val() == "") {

                        self.find('.NoteTemplate > select').attr('ddlist', 'GetNoteTemplate');
                        var NoteType = $("#" + Clinical_PhoneEncounter.params.PanelID + " #frmClinicalPhoneEncounter #ddlNoteType").val();
                        NoteType = NoteType == '' ? -1 : NoteType;
                        var data = "IsActive=&ID=" + NoteType + "&ID2=" + ProviderId;
                        self.loadDropDowns(true, data).done(function () {
                            self.find('.NoteTemplate').removeClass('disableAll');
                            if ($('#pnlClinicalPhoneEncounter #frmClinicalPhoneEncounter').data("bootstrapValidator") != null && typeof $('#frmClinicalPhoneEncounter').data('bootstrapValidator') != 'undefined') {
                                $("#" + Clinical_PhoneEncounter.params.PanelID + " #frmClinicalPhoneEncounter").bootstrapValidator('revalidateField', self.find('.NoteType > select').attr("name"));
                            }
                        });
                    }
                }
            }
        }, 2);
    },
    validateDuration: function () {
        if ($('#pnlClinicalPhoneEncounter #frmClinicalPhoneEncounter').data("bootstrapValidator") != null && typeof $('#frmClinicalPhoneEncounter').data('bootstrapValidator') != 'undefined') {
            $("#" + Clinical_PhoneEncounter.params.PanelID + " #frmClinicalPhoneEncounter").bootstrapValidator('revalidateField', 'Duration');
        }
    },

    GetNoteTemplateType: function (obj) {
        var self = $('#' + Clinical_PhoneEncounter.params.PanelID);

        self.find('.NoteType > select').attr('ddlist', 'GetPhoneEncounterTemplateType');

        var data = "IsActive=1&ID=" + $(obj).val();
        self.find('.NoteType').loadDropDowns(true, data).done(function () {

            $("#" + Clinical_PhoneEncounter.params["PanelID"] + " #frmClinicalPhoneEncounter #ddlNoteType option").each(function () {
                if ($(this).text().replace(' ', '').toLowerCase() == "phoneencounter") {
                    $(this).prop('selected', true);
                    $("#" + Clinical_PhoneEncounter.params["PanelID"] + " #frmClinicalPhoneEncounter #ddlNoteType").trigger("onchange");
                }
                else {
                    $(this).removeAttr("selected");
                }
            });

            if ($('#pnlClinicalPhoneEncounter #frmClinicalPhoneEncounter').data("bootstrapValidator")) {
                $("#" + Clinical_PhoneEncounter.params.PanelID + " #frmClinicalPhoneEncounter").bootstrapValidator('revalidateField', self.find('.NoteType > select').attr("name"));
            }
        });

    },

    /*
        Purpose: this function get the rooms ddl based on facility selected
        Author: Muhammad Azhar Shahzad
        Created Date: March 24,2016
    */
    GetRooms: function (obj, FacilityId) {
        setTimeout(function () {
            var self = $('#' + Clinical_PhoneEncounter.params.PanelID);
            if ($('#' + Clinical_PhoneEncounter.params.PanelID + ' #txtFacility').val() == '') {
                self.find('#divRoomNo.GetDDLRooms > select').val("");
                self.find('#divRoomNo.GetDDLRooms > select').val("");
                self.find('#divRoomNo.GetDDLRooms > select option[value!=""]').remove();
            } else {
                if (FacilityId == null) {
                    FacilityId = $('#' + Clinical_PhoneEncounter.params.PanelID + ' #hfFacility').val();
                }

                if (FacilityId == "") {
                    self.find('#divRoomNo.GetDDLRooms > select').val("");
                    self.find('#divRoomNo.GetDDLRooms > select').val("");
                    self.find('#divRoomNo.GetDDLRooms > select option[value!=""]').remove();
                } else {

                    self.find('#divRoomNo.GetDDLRooms > select').attr('ddlist', 'GetRooms');
                    var data = "IsActive=&ID=" + FacilityId;
                    self.find('#divRoomNo.GetDDLRooms').loadDropDowns(true, data).done(function () {
                        // Setting Default room to Lobby
                        var LobbyId = $('#' + Clinical_PhoneEncounter.params.PanelID + ' #frmClinicalPhoneEncounter #RoomNo option').filter(function () { return $(this).html() == "Lobby"; }).val();
                        if (LobbyId != null && LobbyId != '') {
                            $('#' + Clinical_PhoneEncounter.params.PanelID + ' #frmClinicalPhoneEncounter #RoomNo').val(LobbyId);
                        }
                        //$("#" + Clinical_PhoneEncounter.params.PanelID + " #frmClinicalPhoneEncounter").bootstrapValidator('revalidateField', self.find('.NoteType > select').attr("name"));
                    });
                }

            }
        }, 2);
    },


    //Load all autocompletes for this form (Ref Providers, Providers, Facility)
    LoadAllAutocomplete: function () {
        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            $("#frmClinicalPhoneEncounter #txtProvider").autocomplete({
                autoFocus: true,
                source: Providers, // pass an array
                select: function (event, ui) {

                    setTimeout(function () {
                        if ($("#pnlClinicalPhoneEncounter #lnkProviderEdit").css("display") == "none") {
                            $("#pnlClinicalPhoneEncounter #lnkProviderEdit").css("display", "");
                            $("#pnlClinicalPhoneEncounter #lblProvider").css("display", "none");
                        }
                        $("#pnlClinicalPhoneEncounter #txtProvider").attr("ProviderId", ui.item.id); // add the selected id
                        $("#pnlClinicalPhoneEncounter #hfProvider").val(ui.item.id);

                    }, 100);
                }
            });
        });

        CacheManager.BindCodes('GetFacility', false).done(function (result) {
            $("#frmClinicalPhoneEncounter input#txtFacility").autocomplete({
                autoFocus: true,
                source: Facilities, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#pnlClinicalPhoneEncounter #hfFacility").val(ui.item.id); // add the selected id
                        $("#pnlClinicalPhoneEncounter #txtFacility").val(ui.item.value);

                        if ($("#pnlClinicalPhoneEncounter #lnkFacilityEdit").css("display") == "none") {
                            $("#pnlClinicalPhoneEncounter #lnkFacilityEdit").css("display", "inline");
                            $("#pnlClinicalPhoneEncounter #lblFacility").css("display", "none");
                        }
                    }, 100);
                }
            });
        });

        CacheManager.BindCodes('GetRefProviders', false).done(function (result) {
            $("#frmClinicalPhoneEncounter input#txtRefProvider").autocomplete({
                autoFocus: true,
                source: RefProviders, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#pnlClinicalPhoneEncounter #hfRefProvider").val(ui.item.id); // add the selected id
                        if ($("#pnlClinicalPhoneEncounter #lnkRefProviderEdit").css("display") == "none") {
                            $("#pnlClinicalPhoneEncounter #lnkRefProviderEdit").css("display", "inline");
                            $("#pnlClinicalPhoneEncounter #lblRefProvider").css("display", "none");
                        }
                    }, 100);

                }
            });
        });

    },

    BindBlockReason: function () {

        var shortName = $("#frmClinicalPhoneEncounter input#txtVisitReason").val();
        Clinical_PhoneEncounter.GetBlockHours(shortName).done(function (response) {

            $("#frmClinicalPhoneEncounter input#txtVisitReason").autocomplete({
                autoFocus: true,
                source: response,
                select: function (event, ui) {
                    setTimeout(function () {

                        $("#pnlClinicalPhoneEncounter #hfVisitReason").val(ui.item.id);

                    }, 100);
                }
            });
            $("#frmClinicalPhoneEncounter input#txtVisitReason").autocomplete("search");
        });
    },

    GetBlockHours: function (name, IsGetAll) {
        var AllBlockReasons = [];
        var dfd = new $.Deferred();
        Clinical_PhoneEncounter.LoadBlockhoursDBCall(name).done(function (responseData) {
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

        var clinicalTabActiveClass = "";
        var clinicalTabid = "clinicalTabPhoneEncounter";
        var clinicalTabtext = "Notes";
        var clinicalTab = "clinicalTabPhoneEncounter";


        var clinicalTabElement = $('<span class="btn btn-default btn-sm tab_space"><button type="button" class="btn btn-default btn-sm tab_space' + clinicalTabActiveClass + '" title="' + clinicalTabtext
        + '" id="' + clinicalTabid + '" onclick=SelectTab("' + clinicalTab + '","false");ClinicalMenuClick(event,null,null,null,\'' + clinicalTabid + '\',\'' + 'button' + '\');>' + clinicalTabtext + '</button></span>');


        if ($("div#mstrDivNotes").find("button#clinicalTabPhoneEncounter").length < 1) {
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

            var ProgressNoteTabElement = $('<span class="btn btn-default btn-sm tab_space"><button type="button" class="btn btn-default btn-sm tab_space' + ProgressNoteActiveClass + '" title="' + ProgressNotetext
                        + '" id="' + ProgressNoteTabid + '" onclick=SelectTab("' + ProgressNoteTab + '","false");ClinicalMenuClick(event,null,null,null,\'' + ProgressNoteTabid + '\',\'' + 'button' + '\');>' + ProgressNotetext + '</button></span>');

            if ($("div#mstrDivNotes").find("button#clinicalTabProgressNote").length < 1) {
                $("div#mstrDivNotes").find("span").removeClass('tab_selected');
                $("div#mstrDivNotes").find("button").removeClass("active");
                $("div#mstrDivNotes").append(ProgressNoteTabElement);
            }

            Clinical_PhoneEncounter.params["IsPhoneEncounter"] = true;

            $("#clinicalTabProgressNote").trigger("click");
            //Start Farooq Ahmad 20/07/2016 EMR-60
            $("#ClinicalUL").find('li:not(#clinicalMenuNotes)').removeClass("active nav-active nav-expanded");
            //End Farooq Ahmad 20/07/2016 EMR-60
        }

        if ($("div#mstrDivNotes").css("display") == "none") {
            $("div#mstrDivNotes").css("display", "inline");
        }
    },

    //This Function is used to Get Notes information from db and Load that information to Grid and create pagination
    //This Function is called by    NotesActiveInactive, NotesDelete, ActiveInactiveNotes, SignNotes, LoadNoteGrid Functions of Clinical_PhoneEncounter.js
    NotesSearch: function (NotesId, PageNo, rpp, NoteStatus) {
        //Start//22/12/2015//Ahmad Raza//Privileges Logic Implemented

        if ($("#pnlClinicalPhoneEncounter #pnlNotes_Result").css("display") == "none") {
            $("#pnlClinicalPhoneEncounter #pnlNotes_Result").show();
        }


        Clinical_PhoneEncounter.SearchNotes(NotesId, PageNo, rpp, NoteStatus).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_PhoneEncounter.NotesGridLoad(response);
                var TableControl = "pnlClinicalPhoneEncounter #dgvClinicalPhoneEncounter";
                var PagingPanelControlID = "pnlClinicalPhoneEncounter #divClinicalNotesPaging";
                var ClassControlName = "Clinical_PhoneEncounter";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.ClinicalNotesCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                    Clinical_PhoneEncounter.NotesSearch(PrimaryID, PageNumber, ResultPerPage);
                }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },
    NotesDraftSearch: function (NotesId, PageNo, rpp, NoteStatus) {
        //Start//22/12/2015//Ahmad Raza//Privileges Logic Implemented
        NoteStatus = 'Draft';
        if ($("#pnlClinicalPhoneEncounter #pnlNotes_Result").css("display") == "none") {
            $("#pnlClinicalPhoneEncounter #pnlNotes_Result").show();
        }
        Clinical_PhoneEncounter.NotesSignedSearch(0, null, null, 'Signed');
        Clinical_PhoneEncounter.SearchNotes(NotesId, PageNo, rpp, NoteStatus).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.NotesLoad_JSON != "") {
                    $("#pnlClinicalPhoneEncounter #draftPhoneEncounterCount").text(response.iTotalDraftDisplayRecords);
                    Clinical_PhoneEncounter.NotesDraftGridLoad(response, PageNo, rpp);
                    var TableControl = "pnlClinicalPhoneEncounter #dgvClinicalDraftPhoneEncounter";
                    var PagingPanelControlID = "pnlClinicalPhoneEncounter #divClinicalDraftNotesPaging";
                    var ClassControlName = "Clinical_PhoneEncounter";
                    var PagesToDisplay = 5;
                    var iTotalDraftDisplayRecords = response.iTotalDraftDisplayRecords;
                    setTimeout(CreatePagination(response.iTotalDraftDisplayRecords, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDraftDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage, NoteStatus) {
                        Clinical_PhoneEncounter.NotesDraftSearch(PrimaryID, PageNumber, ResultPerPage, NoteStatus);
                    }), 10);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    NotesSignedSearch: function (NotesId, PageNo, rpp, NoteStatus) {
        //Start//22/12/2015//Ahmad Raza//Privileges Logic Implemented
        NoteStatus = 'Signed';
        if ($("#pnlClinicalPhoneEncounter #pnlNotes_Result").css("display") == "none") {
            $("#pnlClinicalPhoneEncounter #pnlNotes_Result").show();
        }


        Clinical_PhoneEncounter.SearchNotes(NotesId, PageNo, rpp, NoteStatus).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $("#pnlClinicalPhoneEncounter #signedPhoneEncounterCount").text(response.iTotalSignedDisplayRecords);
                if (response.NotesLoad_JSON != "") {
                    Clinical_PhoneEncounter.NotesSignedGridLoad(response, PageNo, rpp);
                    var TableControl = "pnlClinicalPhoneEncounter #dgvClinicalSignedPhoneEncounter";
                    var PagingPaneldfgControlID = "pnlClinicalPhoneEncounter #divClinicalSignedNotesPaging";
                    var ClassControlName = "Clinical_PhoneEncounter";
                    var PagesToDisplay = 5;
                    var iTotalSignedDisplayRecords = response.iTotalSignedDisplayRecords;
                    setTimeout(CreatePagination(response.iTotalSignedDisplayRecords, PageNo, rpp, PagingPaneldfgControlID, TableControl, ClassControlName, PagesToDisplay, iTotalSignedDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage, NoteStatus) {
                        Clinical_PhoneEncounter.NotesSignedSearch(PrimaryID, PageNumber, ResultPerPage, NoteStatus);
                    }), 10);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    NoteGridTabChange: function (NoteStatus) {
        if (NoteStatus == 'Draft')
            Clinical_PhoneEncounter.NotesDraftSearch(0, null, null, 'Draft');
        else if (NoteStatus == 'Signed')
            Clinical_PhoneEncounter.NotesSignedSearch(0, null, null, 'Signed');
    },
    //NotesGridLoad
    NotesEdit: function (NotesId, mode, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        //Start//22/12/2015//Ahmad Raza//Privileges Logic Implemented
        AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                // var params = [];
                Clinical_PhoneEncounter.params["NotesId"] = NotesId;
                Clinical_PhoneEncounter.params["PatientId"] = Clinical_PhoneEncounter.params.patientID;
                Clinical_PhoneEncounter.params["IsPhoneEncounter"] = true;
                Clinical_PhoneEncounter.params["mode"] = mode;
                Clinical_PhoneEncounter.params["ParentCtrl"] = Clinical_PhoneEncounter.params["TabID"];
                if (mode == "Edit") {
                    //SelectTab("clinicalTabProgressNote", "false");
                    Clinical_PhoneEncounter.AddProgressNoteTab();
                }

                if (Clinical_PhoneEncounter.params.FromCCM)
                    Clinical_PhoneEncounter.params["IsFromCreateNote"] = true;

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        //End//22/12/2015//Ahmad Raza//Privileges Logic Implemented
    },

    //NotesGridLoad
    NotesDelete: function (NotesId, event, NoteStatus, PatientId) {

        if (event != null) {
            event.stopPropagation();
        }

        utility.myConfirm('1', function () {
            $('body').removeClass('modal-open');
            var selectedValue = NotesId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Clinical_PhoneEncounter.NotesDeleted(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        if ($("#PatientProfile #hfPatientId").val() == PatientId) {
                            setPatientBanner(PatientId);
                        }
                        if (NoteStatus == 'Draft')
                            Clinical_PhoneEncounter.NotesDraftSearch(0, null, null, 'Draft');

                        //Signed Note Can't be deleted
                        //if (NoteStatus == 'Signed')
                        //    Clinical_PhoneEncounter.NotesSignedSearch(0, null, null, 'Signed');
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

        //End//22/12/2015//Ahmad Raza//Privileges Logic Implemented
    },

    //NotesSearch
    NotesGridLoad: function (response) {

        $("#pnlClinicalPhoneEncounter #dgvClinicalDraftPhoneEncounter").dataTable().fnDestroy();
        $("#pnlClinicalPhoneEncounter #pnlNotes_Result #dgvClinicalDraftPhoneEncounter tbody").find("tr").remove();

        Clinical_Notes.canSignNote().done(function (canSign) {
            if (response.ClinicalNotesCount > 0) {
                //EMR-602 fix
                $('#pnlClinicalPhoneEncounter #PreviousNotediv').removeClass('disableAll');

                var NotesLoadJSONData = JSON.parse(response.NotesLoad_JSON);
                $.each(NotesLoadJSONData, function (i, item) {
                    var $row = $('<tr/>');
                    $row.attr("onclick", "utility.SelectGridRow($('#gvNotes_row" + item.NotesId + "'))");
                    $row.attr("id", "gvNotes_row" + item.NotesId);
                    $row.attr("NotesId", item.NotesId);
                    $row.attr("Date", utility.RemoveTimeFromDate(null, item.VisitDate));
                    $row.attr("Time", item.VisitTime);
                    $row.attr("Duration", item.Duration);
                    $row.attr("NoteType", item.NoteType);
                    $row.attr("CC", item.ChiefComplaint);
                    $row.attr("Status", item.EntityId);
                    $row.attr("Provider", item.ProviderName);
                    $row.attr("SignedBy", item.SignedBy);

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
                        Isdisabled = "";
                    }
                    //on row click, edit the record, requirement from dr Hajjar
                    if (Isdisabled == '') {
                        $row.attr("onclick", "Clinical_PhoneEncounter.NotesRowEdit(" + item.NotesId + ",'Edit'" + ", event);");
                    }

                    var NotesPreview = "Clinical_PhoneEncounter.NotesPreview('" + item.NotesId + "',null,'" + item.PatientId + "','" + item.ProviderId + "','" + utility.RemoveTimeFromDate(null, item.VisitDate) + "','" + item.BillingInfoId + "','" + item.AppointmentDate + "'," + item.VisitId + ",'" + item.NoteDate + "','" + item.PatientTypeId + "', '" + item.FacilityId + "', '" + item.POS + "'," + (!item.RefProviderId ? null : +item.RefProviderId) + "," + (item.IsPhoneEncounter == "0" ? false : true) + ");";
                    if (canSign) {
                        $row.append('<td style="display:none;">' + item.NotesId + '</td><td><a ' + Isdisabled + ' class="btn  btn-xs" href="#" onclick="Clinical_PhoneEncounter.NotesDelete(' + item.NotesId + ',event,\'' + item.NoteStatus + '\',\'' + item.PatientId + '\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a ' + Isdisabled + ' class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_PhoneEncounter.NotesEdit(' + item.NotesId + ",'Edit'" + ', event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a title="View Note" class="btn  btn-xs" href="#" onclick="' + NotesPreview + '"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Sign Note" onclick="Clinical_PhoneEncounter.SignNotes(' + item.NotesId + ',event,\'' + item.NoteStatus + '\',false,\'' + utility.RemoveTimeFromDate(null, item.VisitDate) + '\',\'' + item.ProviderId + '\');" ' + Isdisabled + '> <i class="fa fa-calculator black"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Record History" onclick="Clinical_PhoneEncounter.rowHistory(' + item.NotesId + ',\'' + utility.RemoveTimeFromDate(null, item.VisitDate) + '\',\'' + item.VisitReasonComments + '\',\'' + item.NoteTempType + '\',event);"> <i class="fa fa-history blue"></i></a></td><td>' + utility.RemoveTimeFromDate(null, item.VisitDate) + '</td><td>' + item.VisitTime + '</td>' + '</td><td>' + item.Duration + '</td><td>' + item.TemplateTypeName + '</td>' + '<td>' + item.NoteStatus + '</td><td>' + item.ProviderName + '</td>' + '</td><td>' + item.SignedBy + '</td><td>' + item.VisitReasonComments + '</td>');
                    }
                    else {
                        $row.append('<td style="display:none;">' + item.NotesId + '</td><td><a ' + Isdisabled + ' class="btn  btn-xs" href="#" onclick="Clinical_PhoneEncounter.NotesDelete(' + item.NotesId + ',event,\'' + item.NoteStatus + '\',\'' + item.PatientId + '\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a ' + Isdisabled + ' class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_PhoneEncounter.NotesEdit(' + item.NotesId + ",'Edit'" + ', event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a title="View Note" class="btn  btn-xs" href="#" onclick="' + NotesPreview + '"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Record History" onclick="Clinical_PhoneEncounter.rowHistory(' + item.NotesId + ',\'' + utility.RemoveTimeFromDate(null, item.VisitDate) + '\',\'' + item.VisitReasonComments + '\',\'' + item.NoteTempType + '\',event);"> <i class="fa fa-history blue"></i></a></td><td>' + utility.RemoveTimeFromDate(null, item.VisitDate) + '</td><td>' + item.VisitTime + '</td>' + '</td><td>' + item.Duration + '</td><td>' + item.TemplateTypeName + '</td>' + '<td>' + item.NoteStatus + '</td><td>' + item.ProviderName + '</td>' + '</td><td>' + item.SignedBy + '</td><td>' + item.VisitReasonComments + '</td>');
                    }

                    $("#pnlNotes_Result #dgvClinicalDraftPhoneEncounter tbody").last().append($row);
                });
            }
            else {
                //EMR-602 fix
                $('#pnlClinicalPhoneEncounter #PreviousNotediv').addClass('disableAll')
                $('#pnlClinicalPhoneEncounter #dgvClinicalDraftPhoneEncounter').DataTable({
                    "language": {
                        "emptyTable": "No Note is Found"
                    }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
            }


            if ($.fn.dataTable.isDataTable('#pnlClinicalPhoneEncounter #dgvClinicalDraftPhoneEncounter'))
                ;
            else {
                $("#pnlClinicalPhoneEncounter #pnlNotes_Result #dgvClinicalDraftPhoneEncounter").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

            }
        });
    },
    NotesDraftGridLoad: function (response, PageNo, rpp) {
        $("#pnlClinicalPhoneEncounter #dgvClinicalDraftPhoneEncounter").dataTable().fnDestroy();
        $("#pnlClinicalPhoneEncounter #pnlNotes_Result #dgvClinicalDraftPhoneEncounter tbody").find("tr").remove();

        Clinical_Notes.canSignNote().done(function (canSign) {
            if (response.ClinicalNotesCount > 0) {
                //EMR-602 fix
                $('#pnlClinicalPhoneEncounter #PreviousNotediv').removeClass('disableAll')
                var NotesLoadJSONData = JSON.parse(response.NotesLoad_JSON);
                $.each(NotesLoadJSONData, function (i, item) {
                    var $row = $('<tr/>');
                    $row.attr("onclick", "utility.SelectGridRow($('#gvNotes_row" + item.NotesId + "'))");
                    $row.attr("id", "gvNotes_row" + item.NotesId);
                    $row.attr("NotesId", item.NotesId);
                    $row.attr("Date", utility.RemoveTimeFromDate(null, item.VisitDate));
                    $row.attr("Time", item.VisitTime);
                    $row.attr("Duration", item.Duration);
                    $row.attr("NoteType", item.NoteType);
                    $row.attr("CC", item.ChiefComplaint);
                    $row.attr("Status", item.EntityId);
                    $row.attr("Provider", item.ProviderName);
                    $row.attr("SignedBy", item.SignedBy);

                    $row.attr("Active", item.IsActive);


                    var arrDuration = item.Duration.split(':');

                    var _hour = arrDuration[0];
                    var _minutes = arrDuration[1];
                    var _seconds = arrDuration[2];

                    if (_hour == "" || _hour == "0")
                        _hour = "00";
                    if (_minutes == "" || _minutes == "0")
                        _minutes = "00";
                    if (_seconds == "" || _seconds == "0")
                        _seconds = "00";

                    item.Duration = _hour + ":" + _minutes + ":" + _seconds;


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
                    //on row click, edit the record, requirement from dr Hajjar
                    if (Isdisabled == '') {
                        $row.attr("onclick", "Clinical_PhoneEncounter.NotesRowEdit(" + item.NotesId + ",'Edit'" + ", event);");
                    }
                    var isVisible = 'style="display:none;';
                    if (response.HasDeleteRights != "No") {
                        isVisible = "";
                    }
                    var NotesPreview = "Clinical_PhoneEncounter.NotesPreview('" + item.NotesId + "',null,'" + item.PatientId + "','" + item.ProviderId + "','" + utility.RemoveTimeFromDate(null, item.VisitDate) + "','" + item.BillingInfoId + "','" + item.AppointmentDate + "'," + item.VisitId + ",'" + item.NoteDate + "','" + item.PatientTypeId + "', '" + item.FacilityId + "', '" + item.POS + "'," + (!item.RefProviderId ? null : +item.RefProviderId) + "," + (item.IsPhoneEncounter == "0" ? false : true) + ");";
                    if (canSign) {
                        var NoteSignMethod = "Clinical_PhoneEncounter.SignNotes(" + item.NotesId + ",event,'" + item.NoteStatus + "',false,'" + utility.RemoveTimeFromDate(null, item.VisitDate) + "','" + item.ProviderId + "', '" + item.PatientId + "','" + item.BillingInfoId + "','" + item.AppointmentDate + "'," + item.VisitId + ",'" + item.NoteDate + "','" + item.PatientTypeId + "', '" + item.FacilityId + "', '" + item.POS + "'," + (!item.RefProviderId ? null : +item.RefProviderId) + "," + (item.IsPhoneEncounter == "0" ? false : true) + ");";
                        $row.append('<td style="display:none;">' + item.NotesId + '</td><td><a ' + Isdisabled + ' ' + isVisible + ' class="btn  btn-xs" href="#" onclick="Clinical_PhoneEncounter.NotesDelete(' + item.NotesId + ',event,\'' + item.NoteStatus + '\',\'' + item.PatientId + '\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a ' + Isdisabled + ' class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_PhoneEncounter.NotesEdit(' + item.NotesId + ",'Edit'" + ', event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a title="View Note" class="btn  btn-xs" href="#" onclick="' + NotesPreview + '"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Sign Note" onclick="' + NoteSignMethod + '" ' + Isdisabled + '> <i class="fa fa-calculator black"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Record History" onclick="Clinical_PhoneEncounter.rowHistory(' + item.NotesId + ',\'' + utility.RemoveTimeFromDate(null, item.VisitDate) + '\',\'' + item.VisitReasonComments + '\',\'' + item.NoteTempType + '\',event);"> <i class="fa fa-history blue"></i></a>&nbsp;<a data-toggle="tooltip" class="btn btn-xs " href="javascript:void(0)"  style="display:none;" data-placement="right" onclick="Clinical_PhoneEncounter.GetNoteInfo(' + item.NotesId + ',event);" title="Copy Note"><i class="fa fa-clipboard blue"></i></a></td><td>' + utility.RemoveTimeFromDate(null, item.VisitDate) + '</td><td>' + item.VisitTime + '</td>' + '</td><td>' + item.Duration + '</td><td>' + item.ProviderName + '</td><td>' + item.FacilityName + '</td><td>' + item.EncounterTypeName + '</td>' + '</td><td>' + item.UserName + '</td><td>' + item.VisitReasonComments + '</td>');
                    }
                    else {
                        $row.append('<td style="display:none;">' + item.NotesId + '</td><td><a ' + Isdisabled + ' ' + isVisible + ' class="btn  btn-xs" href="#" onclick="Clinical_PhoneEncounter.NotesDelete(' + item.NotesId + ',event,\'' + item.NoteStatus + '\',\'' + item.PatientId + '\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a ' + Isdisabled + ' class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_PhoneEncounter.NotesEdit(' + item.NotesId + ",'Edit'" + ', event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a title="View Note" class="btn  btn-xs" href="#" onclick="' + NotesPreview + '"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Record History" onclick="Clinical_PhoneEncounter.rowHistory(' + item.NotesId + ',\'' + utility.RemoveTimeFromDate(null, item.VisitDate) + '\',\'' + item.VisitReasonComments + '\',\'' + item.NoteTempType + '\',event);"> <i class="fa fa-history blue"></i></a>&nbsp;<a data-toggle="tooltip" class="btn btn-xs " href="javascript:void(0)" style="display:none;" data-placement="right" onclick="Clinical_PhoneEncounter.GetNoteInfo(' + item.NotesId + ',event);" title="Copy Note"><i class="fa fa-clipboard blue"></i></a></td><td>' + utility.RemoveTimeFromDate(null, item.VisitDate) + '</td><td>' + item.VisitTime + '</td>' + '</td><td>' + item.Duration + '</td><td>' + item.ProviderName + '</td><td>' + item.FacilityName + '</td><td>' + item.EncounterTypeName + '</td>' + '</td><td>' + item.UserName + '</td><td>' + item.VisitReasonComments + '</td>');
                    }

                    $("#pnlNotes_Result #dgvClinicalDraftPhoneEncounter tbody").last().append($row);
                });
            }
            else {
                //EMR-602 fix
                $('#pnlClinicalPhoneEncounter #PreviousNotediv').addClass('disableAll')
                $('#pnlClinicalPhoneEncounter #dgvClinicalDraftPhoneEncounter').DataTable({
                    "language": {
                        "emptyTable": "No Note is Found"
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
            }


            if ($.fn.dataTable.isDataTable('#pnlClinicalPhoneEncounter #dgvClinicalDraftPhoneEncounter'))
                ;
            else {
                $("#pnlClinicalPhoneEncounter #pnlNotes_Result #dgvClinicalDraftPhoneEncounter").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

            }
        });
    },
    //SignedNotes
    NotesSignedGridLoad: function (response, PageNo, rpp) {
        $("#pnlClinicalPhoneEncounter #dgvClinicalSignedPhoneEncounter").dataTable().fnDestroy();
        $("#pnlClinicalPhoneEncounter #pnlNotes_Result #dgvClinicalSignedPhoneEncounter tbody").find("tr").remove();
        Clinical_Notes.canSignNote().done(function (canSign) {
            if (response.ClinicalNotesCount > 0) {
                //EMR-602 fix
                $('#pnlClinicalPhoneEncounter #PreviousNotediv').removeClass('disableAll')
                var NotesLoadJSONData = JSON.parse(response.NotesLoad_JSON);
                $.each(NotesLoadJSONData, function (i, item) {
                    var $row = $('<tr/>');
                    $row.attr("onclick", "utility.SelectGridRow($('#gvSignedNotes_row" + item.NotesId + "'))");
                    $row.attr("id", "gvSignedNotes_row" + item.NotesId);
                    $row.attr("NotesId", item.NotesId);
                    $row.attr("Date", utility.RemoveTimeFromDate(null, item.VisitDate));
                    $row.attr("Time", item.VisitTime);
                    $row.attr("Duration", item.Duration);
                    $row.attr("NoteType", item.NoteType);
                    $row.attr("CC", item.ChiefComplaint);
                    $row.attr("Status", item.EntityId);
                    $row.attr("Provider", item.ProviderName);
                    $row.attr("SignedBy", item.SignedBy);

                    $row.attr("Active", item.IsActive);

                    var arrDuration = item.Duration.split(':');

                    var _hour = arrDuration[0];
                    var _minutes = arrDuration[1];
                    var _seconds = arrDuration[2];

                    if (_hour == "" || _hour == "0")
                        _hour = "00";
                    if (_minutes == "" || _minutes == "0")
                        _minutes = "00";
                    if (_seconds == "" || _seconds == "0")
                        _seconds = "00";

                    item.Duration = _hour + ":" + _minutes + ":" + _seconds;


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
                    //on row click, edit the record, requirement from dr Hajjar
                    if (Isdisabled == '') {
                        $row.attr("onclick", "Clinical_PhoneEncounter.NotesRowEdit(" + item.NotesId + ",'Edit'" + ", event);");
                    }

                    var NotesPreview = "Clinical_PhoneEncounter.NotesPreview('" + item.NotesId + "',null,'" + item.PatientId + "','" + item.ProviderId + "', undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined, undefined,true);"
                    if (canSign) {
                        $row.append('<td style="display:none;">' + item.NotesId + '</td><td><a ' + Isdisabled + '' + isVisible + ' class="btn  btn-xs" href="#" onclick="Clinical_PhoneEncounter.NotesDelete(' + item.NotesId + ',event,\'' + item.NoteStatus + '\',\'' + item.PatientId + '\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a ' + Isdisabled + ' class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_PhoneEncounter.NotesEdit(' + item.NotesId + ",'Edit'" + ', event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a title="View Note" class="btn  btn-xs" href="#" onclick="' + NotesPreview + '"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Sign Note" onclick="Clinical_PhoneEncounter.SignNotes(' + item.NotesId + ',event,\'' + item.NoteStatus + '\',false,\'' + utility.RemoveTimeFromDate(null, item.VisitDate) + '\',\'' + item.ProviderId + '\');" ' + Isdisabled + '> <i class="fa fa-calculator black"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Record History" onclick="Clinical_PhoneEncounter.rowHistory(' + item.NotesId + ',\'' + utility.RemoveTimeFromDate(null, item.VisitDate) + '\',\'' + item.VisitReasonComments + '\',\'' + item.NoteTempType + '\',event);"> <i class="fa fa-history blue"></i></a>&nbsp;<a data-toggle="tooltip" class="btn btn-xs " style="display:none;" href="javascript:void(0)" data-placement="right" onclick="Clinical_PhoneEncounter.GetNoteInfo(' + item.NotesId + ',event);" title="Copy Note"><i class="fa fa-clipboard blue"></i></a></td><td>' + utility.RemoveTimeFromDate(null, item.VisitDate) + '</td><td>' + item.VisitTime + '</td>' + '</td><td>' + item.Duration + '</td><td>' + item.TemplateTypeName + '</td>' + '<td>' + item.ProviderName + '</td>' + '</td><td>' + item.SignedBy + '</td><td>' + item.VisitReasonComments + '</td>');
                    }
                    else {
                        $row.append('<td style="display:none;">' + item.NotesId + '</td><td><a ' + Isdisabled + ' ' + isVisible + ' class="btn  btn-xs" href="#" onclick="Clinical_PhoneEncounter.NotesDelete(' + item.NotesId + ',event,\'' + item.NoteStatus + '\',\'' + item.PatientId + '\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a ' + Isdisabled + ' class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_PhoneEncounter.NotesEdit(' + item.NotesId + ",'Edit'" + ', event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a title="View Note" class="btn  btn-xs" href="#" onclick="' + NotesPreview + '"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Record History" onclick="Clinical_PhoneEncounter.rowHistory(' + item.NotesId + ',\'' + utility.RemoveTimeFromDate(null, item.VisitDate) + '\',\'' + item.VisitReasonComments + '\',\'' + item.NoteTempType + '\',event);"> <i class="fa fa-history blue"></i></a>&nbsp;<a data-toggle="tooltip" class="btn btn-xs " href="javascript:void(0)" style="display:none;" data-placement="right" onclick="Clinical_PhoneEncounter.GetNoteInfo(' + item.NotesId + ',event);" title="Copy Note"><i class="fa fa-clipboard blue"></i></a></td><td>' + utility.RemoveTimeFromDate(null, item.VisitDate) + '</td><td>' + item.VisitTime + '</td>' + '</td><td>' + item.Duration + '</td><td>' + item.TemplateTypeName + '</td>' + '<td>' + item.ProviderName + '</td>' + '</td><td>' + item.SignedBy + '</td><td>' + item.VisitReasonComments + '</td>');
                    }

                    //<a class="btn  btn-xs" href="#" onclick="Clinical_PhoneEncounter.NotesActiveInactive(' + item.NotesId + "," + isactive + ');" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>
                    $("#pnlNotes_Result #dgvClinicalSignedPhoneEncounter tbody").last().append($row);
                });
            }
            else {
                //EMR-602 fix
                $('#pnlClinicalPhoneEncounter #PreviousNotediv').addClass('disableAll')
                if ($.fn.dataTable.isDataTable('#pnlClinicalPhoneEncounter #dgvClinicalSignedPhoneEncounter'))
                    ;
                else {
                    $('#pnlClinicalPhoneEncounter #dgvClinicalSignedPhoneEncounter').DataTable({
                        "language": {
                            "emptyTable": "No Note is Found"
                        }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                    });
                }
            }


            if ($.fn.dataTable.isDataTable('#pnlClinicalPhoneEncounter #dgvClinicalSignedPhoneEncounter'))
                ;
            else {
                $("#pnlClinicalPhoneEncounter #pnlNotes_Result #dgvClinicalSignedPhoneEncounter").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

            }
        });
    },
    NotesRowEdit: function (NotesId, mode, event) {
        if (event != null) {
            event.stopPropagation();
        }
        if ((event.srcElement instanceof HTMLAnchorElement || event.srcElement.nodeName.toLowerCase() == 'i') != true) {
            Clinical_PhoneEncounter.NotesEdit(NotesId, mode, event);
        }
    },
    /// Author: ZeeshanAK
    /// Purpose:  Call for showing history of current item
    /// Date : April 22, 2016
    ShowHistory: function (NotesId, VisitDate, VisitReasonComments, NoteTempType) {

        var params = [];
        params["FromAdmin"] = "0";
        params["NotesId"] = NotesId;
        params["ParentCtrl"] = 'clinicalTabPhoneEncounter';
        params["VisitDate"] = VisitDate;
        params["VisitReasonComments"] = VisitReasonComments;
        params["NoteType"] = NoteTempType;
        LoadActionPan('Clinical_Note_Components_Audit', params);

        //EMRUtility.showCurrentItemHistory(Clinical_PhoneEncounter.params.PanelID, null, NotesId, "Notes", Clinical_PhoneEncounter.params.patientID, Clinical_PhoneEncounter.params.TabID, null);
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
            Clinical_PhoneEncounter.ShowHistory(currentNotesId, VisitDate, VisitReasonComments, NoteTempType);
        }
    },
    //This function is called by Sign btn of Notes Grid. it ask for signing Progress note
    //NotesGridLoad
    SignNotes: function (NotesId, event, NoteStatus, isComponentSelect, VisitDate, ProviderId, PatientId, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter) {
        Clinical_ProgressNote.params.VisitDateTime = NoteDate;
        var objDeffered = $.Deferred();
        var notesID = NotesId;
        if (event != null) {
            event.stopPropagation();
        }
        //Start//02-05-2016//Ahmad Raza//logic for CDS Alert
        var CDSAlertCount = parseInt($(" #mainForm  li#CDSAlert span").text());
        if (!Clinical_PhoneEncounter.params.IsPhoneEncounter && CDSAlertCount > 0) {
            utility.DisplayMessages("Please change the Status of all the CDS Alerts before signing the Note.", 3);
        }
        else {
            Clinical_ProgressNote.loadCQMWithReasoning(VisitDate, VisitDate, Clinical_PhoneEncounter.params.patientID, ProviderId, NotesId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    Clinical_Notes.params["CQMResponse"] = response;
                    if (response.AllMeasuresReasoningDetailCount > 0) {
                        var arrNonCompliantPatients = $.grep(JSON.parse(response.AllMeasuresReasoningDetailLoad_JSON), function (a) {
                            return a.Patientid == Clinical_PhoneEncounter.params.patientID;
                        });

                        Clinical_PhoneEncounter.arrCQMReasoning[Clinical_PhoneEncounter.params.patientID] = JSON.stringify(arrNonCompliantPatients);
                        var CQMFoundMsg = "Our System found some <span class='red'>missing data</span> related to this patient."
                                        + " In order to qualify for the <b>2016 CQM incentives,</b> you must enter those <span class='red'>missing data values</span>"
                                        + " against the CQM measures that you have planned to report this year. Do you want to enter the data here before signing off this note?";

                        utility.myConfirm(CQMFoundMsg, function () {
                            objDeffered.resolve();
                            Clinical_Notes.openPatientList(Clinical_PhoneEncounter.params.patientID, isComponentSelect, ProviderId, VisitDate, NotesId, NoteStatus, "clinicalTabPhoneEncounter");
                        }, function () {
                            $.when(Clinical_PhoneEncounter.SignNotesAfterCQM(NotesId, event, NoteStatus, isComponentSelect, VisitDate, ProviderId, PatientId, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter)).then(function () {
                                objDeffered.resolve();
                            });
                        },
                              '<b>2016 CQM Missing Data Alert</b>', "Yes, I do", "No, not this time"
                          );

                    }
                    else {
                        $.when(Clinical_PhoneEncounter.SignNotesAfterCQM(NotesId, event, NoteStatus, isComponentSelect, VisitDate, ProviderId, PatientId, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter)).then(function () {
                            objDeffered.resolve();
                        });
                    }
                }
                else {
                    objDeffered.resolve();
                    utility.DisplayMessages(response.Message, 3);
                }
            });
            //Clinical_PhoneEncounter.SignNotesAfterCQM(NotesId, event);
        }
    },

    SignNotesAfterCQM: function (NotesId, event, NoteStatus, isComponentSelect, VisitDate, ProviderId, PatientId, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter) {
        var notesID = NotesId;
        AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                $.when(Clinical_ProgressNote.signNote(NotesId, PatientId, false, IsPhoneEncounter)).done(function () {
                    Clinical_PhoneEncounter.NotesDraftSearch(0, null, null, 'Draft');
                    Clinical_NotesSearch.SetNotesCount();
                });
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
        //End//02-05-2016//Ahmad Raza//logic for CDS Alert
        //End//29/12/2015//Ahmad Raza//Privileges logic implemented
    },
    CreateCharges: function (Obj, NotesId, PatientId, ProviderId, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID) {
        BillingInformation.params = Clinical_Notes.initializeBillingInfoParams(NotesId, PatientId, ProviderId, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID);
        Clinical_NotesSearch.CreateObjectForBilling(POS);
    },
    initializeBillingInfoParams: function (NotesId, PatientId, ProviderId, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID) {
        var params = [];
        params["ParentCtrl"] = "clinicalTabPhoneEncounter";
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
    LoadAttachecdICDsAndCPTs: function (notesID) {
        var objData = new Object();
        objData["NotesId"] = notesID;
        objData["PatientId"] = Clinical_PhoneEncounter.params.patientID;
        objData["commandType"] = "LoadProceduresAndProblemsByNoteAndPatientId";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "BillingInformation", "BillingInformation");
    },

    UnLoad: function () {

        RemoveAdminTab('clinicalTabPhoneEncounter');

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

    NotesDeleted: function (NotesId) {
        var objData = {};
        objData["NotesId"] = NotesId;

        objData["commandType"] = "DELETE_CLINICAL_NOTES";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    NotesUpdate: function (NotesData, NotesID, Duration) {
        var objData = JSON.parse(NotesData);

        if (Clinical_ProgressNote.ComeFromCopyNote) {
            Clinical_ProgressNote.ComeFromCopyNote = false;
            objData["ComeFromCopyNote"] = "1";
        }
        else {
            objData["ComeFromCopyNote"] = "0";
        }

        var DurationText = Duration;
        objData["DurationText"] = DurationText;
        objData["NotesId"] = NotesID;
        objData["PatientID"] = Clinical_PhoneEncounter.params.patientID;
        objData["AppointmentID"] = Clinical_PhoneEncounter.params.AppointmentID;
        objData["commandType"] = "UPDATE_CLINICAL_NOTES";

        var data = JSON.stringify(objData);
        // sNotesch parameter , class name, command name of class
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    NotesSave: function (NotesData, patientID, Duration, IsToCheckForTodaysNote) {
        var objData = JSON.parse(NotesData);
        var NoteType = $('#' + Clinical_PhoneEncounter.params.PanelID + ' #ddlNoteType option:selected').text();
        var DurationText = Duration;
        objData["PatientID"] = patientID;
        objData["IsToCheckForTodaysNote"] = IsToCheckForTodaysNote;
        objData["AppointmentID"] = Clinical_PhoneEncounter.params.AppointmentID;
        objData["NoteTypeText"] = NoteType;
        objData["DurationText"] = DurationText;
        objData["IsHPIComplaint"] = globalAppdata["IsDefaultHPI"] == "False" ? false : true;
        objData["commandType"] = "SAVE_CLINICAL_NOTES";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    NotesUpdateActiveInactive: function (NotesId, IsActive) {
        var objData = {};
        objData["NotesId"] = NotesId;
        objData["IsActive"] = IsActive;

        objData["commandType"] = "UPDATE_CLINICAL_NOTES_ACTIVE_INACTIVE";
        var data = JSON.stringify(objData);

        // sNotesch parameter , class name, command name of class
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

        if (Clinical_PhoneEncounter.params.patientID != null) {
            objData["PatientId"] = Clinical_PhoneEncounter.params.patientID;
        }

        objData["NoteStatus"] = NoteStatus;
        objData["commandType"] = "LOAD_CLINICAL_NOTES";
        objData["isPhoneEncounter"] = true;

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    NotesPreview: function (NotesId, ParentCtrl, PatientId, ProviderId, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter) {
        var params = [];
        params["FromAdmin"] = "0";
        params["NotesId"] = NotesId;
        params["PatientId"] = PatientId;
        params["ProviderId"] = ProviderId;
        if (ParentCtrl == null) {
            params["ParentCtrl"] = 'clinicalTabPhoneEncounter'; //Clinical_PhoneEncounter
        } else {
            params["ParentCtrl"] = ParentCtrl; //Clinical_PhoneEncounter
        }

        params["BillingInfoId"] = BillingInfoId;
        params["VisitDate"] = VisitDate;
        params["AppointmentDate"] = AppointmentDate;
        params["FacilityId"] = FacilityId;
        params["RefProviderID"] = RefProviderID;
        params["VisitId"] = VisitId;
        params["NoteDate"] = NoteDate;
        params["PatientTypeId"] = PatientTypeId;
        params["POS"] = POS;
        params["IsPhoneEncounter"] = IsPhoneEncounter;

        LoadActionPan('Clinical_NotesView', params);
    },

    GetPreviousNotePatient_DBCall: function () {
        var objData = {};
        if (Clinical_PhoneEncounter.params.patientID == "" || Clinical_PhoneEncounter.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_PhoneEncounter.params.patientID;
        }
        objData["commandType"] = "copy_previous_note_patient";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    // this function is used by both Notes and Progress Note Form
    GetLinkedAppointment_DBCall: function () {
        var objData = {};
        if (Clinical_PhoneEncounter.params.patientID == "" || Clinical_PhoneEncounter.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_PhoneEncounter.params.patientID;
        }
        objData["commandType"] = "fill_linked_appointment_notes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },
    //------------------END DB Call Functions------------------


    //----------------Progress Notes----------------------
    // this function is used by both Notes and Progress Note Form
    LinkedAppointment: function (AppointmentId, VisitId, AppointmentDate) {
        var CurrentPanelId = 'pnlClinicalPhoneEncounter';
        if (GetCurrentSelectedTab() != null) {
            CurrentPanelId = GetCurrentSelectedTab().PanelID;
        }
        Clinical_PhoneEncounter.UnLoadLinkedAppointmentList();
        $("#" + CurrentPanelId + " #hfAppointmentId").val(AppointmentId);
        $("#" + CurrentPanelId + " #hfVisitId").val(VisitId);
        $("#" + CurrentPanelId + " #txtLinkedAppointment").val(AppointmentDate);
        $("#" + CurrentPanelId + " #ChkBox_LinkedAppointment").prop('checked', true);
        $('#' + Clinical_PhoneEncounter.params.PanelID + ' #dtpVisitDate').val(utility.RemoveTimeFromDate(null, AppointmentDate));
        $('#' + Clinical_PhoneEncounter.params.PanelID + ' #dtpVisitDate').datepicker('setDate', utility.RemoveTimeFromDate(null, AppointmentDate))
        $('#' + Clinical_PhoneEncounter.params.PanelID + ' #VisitTime').val(new Date(AppointmentDate).toLocaleTimeString());
        $('#' + Clinical_PhoneEncounter.params.PanelID + ' #VisitTime').timepicker('setTime', new Date(AppointmentDate).toLocaleTimeString());
        Clinical_PhoneEncounter.VisitSearch(VisitId, Clinical_PhoneEncounter.params["patientID"]);

        Clinical_PhoneEncounter.UnlinkAppointment($("#" + CurrentPanelId + " #ChkBox_LinkedAppointment"), CurrentPanelId, true);
    },
    // this function is used by both Notes and Progress Note Form
    UnlinkAppointment: function (Cntrl, PanelID, IsCurrentDate) {
        if ($(Cntrl).is(':checked')) {
            $("#" + PanelID + " #lblLinkedAppointment").show();
            $("#" + PanelID + " #lnkLinkedAppointment").hide();
            // $("#"+PanelID+" #txtLinkedAppointment").attr("disabled", "disabled");
            $("#" + PanelID + " #ChkBox_LinkedAppointment").removeAttr("disabled");
        } else {
            $("#" + PanelID + " #lblLinkedAppointment").hide();
            $("#" + PanelID + " #lnkLinkedAppointment").show();
            $("#" + PanelID + " #txtLinkedAppointment").val("");
            $("#" + PanelID + " #hfAppointmentId").val("");
            if (IsCurrentDate) {
                $('#' + Clinical_PhoneEncounter.params.PanelID + ' #VisitTime').timepicker('setTime', new Date());
                var date_format = 'dd/mm/yyyy';
                //set default Date Formate
                if (globalAppdata['DateFormat'])
                    date_format = globalAppdata['DateFormat'];
                $('#' + Clinical_PhoneEncounter.params.PanelID + ' #dtpVisitDate').datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), new Date()));

            }
            $('#' + Clinical_PhoneEncounter.params.PanelID + ' #txtLinkedAppointment').val('No Appointment Selected');
            $("#" + PanelID + " #ChkBox_LinkedAppointment").attr("disabled", "disabled");
        }

    },
    // this function is used by both Notes and Progress Note Form
    GetLinkedAppointment: function (ActionPanID) {

        Clinical_PhoneEncounter.GetLinkedAppointment_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                if (response.status != false) {
                    Clinical_PhoneEncounter.LinkedAppointmentGridLoad(response);
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
        var ActionPanId = '#pnlClinicalPhoneEncounter #actionPanClinicalNotes';
        if (CurrentTab != null) {
            ActionPanId = '#' + CurrentTab.PanelID + ' #' + CurrentTab.ActionPanContainer;
        }

        $(ActionPanId).modal('hide');

        setTimeout(function () {
            $(ActionPanId).find('div').first().remove();
        }, 300);

    },

    LinkedAppointmentGridLoad: function (response) {
        $('#pnlLinekdAppointment_Result #dgvLinekdAppointment').dataTable().fnDestroy();
        $("#pnlLinekdAppointment_Result #dgvLinekdAppointment tbody").find("tr").remove();
        if (response.LinkedAppointmentCount > 0) {
            var Clinical_Notes_LinkedAppointment_detail = JSON.parse(response.LinkedAppointment_JSON);
            $.each(Clinical_Notes_LinkedAppointment_detail, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvNotes_row" + item.NotesId + "'))");
                $row.attr("id", "gvAppointment_row" + item.AppointmentId);
                $row.attr("AppointmentId", item.AppointmentId);
                $row.attr("VisitId", item.VisitId);
                $row.attr("DOS", utility.RemoveTimeFromDate(null, item.DOS));
                $row.attr("AppointmentDate", utility.RemoveTimeFromDate(null, item.AppointmentDate));
                $row.attr("Appointment", item.VisitTime);
                $row.attr("Provider", item.ProviderName);
                $row.attr("Facility", item.FacilityName);
                $row.attr("Status", item.VisitStatusName);

                $row.append('<td style="display:none;">' + item.AppointmentId + '</td><td><a  class="btn  btn-xs" href="#" onclick="Clinical_PhoneEncounter.LinkedAppointment(' + item.AppointmentId + ',' + item.VisitId + ',\'' + item.AppointmentDate + '\');" title="Linked Appointment"><i class="fa fa-check black"></i></a></td><td>' + item.AppointmentDate + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOS) + '</td><td>' + item.ProviderName + '</td><td>' + item.FacilityName + '</td><td>' + item.VisitStatusName + '</td>');

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


        if ($.fn.dataTable.isDataTable('#pnlLinekdAppointment_Result #dgvLinekdAppointment'))
            ;
        else
            $("#pnlLinekdAppointment_Result #dgvLinekdAppointment").DataTable({ "bInfo": false, "bPaginate": true, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    GetPreviousNotePatient: function () {

        Clinical_PhoneEncounter.GetPreviousNotePatient_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var Clinical_Notes_PreviousNote_detail = JSON.parse(response.PreviousNote_JSON);
                if (response.PreviousNoteCount > 0) {
                    var noteData = Clinical_Notes_PreviousNote_detail;
                    //EMR-652 fix
                    $.when(Clinical_ProgressNote.GetNotesTemplates(noteData.ProviderId, noteData.NoteTemplate, noteData.NoteType, Clinical_PhoneEncounter.params.PanelID)).done(function () {
                        $("#pnlClinicalPhoneEncounter #ddlNoteType").val(noteData.NoteType);
                        $.when(Clinical_ProgressNote.GetNoteTemplateType(noteData.NoteTemplate, Clinical_PhoneEncounter.params.PanelID)).done(function () {
                            $("#pnlClinicalPhoneEncounter #NoteTemplate").val(noteData.NoteTemplate);
                            var self = $('#' + Clinical_ProgressNote.params.PanelID);
                            self.find('#divRoomNo.GetDDLRooms > select').attr('ddlist', 'GetRooms');
                            var data = "IsActive=&ID=" + noteData.FacilityId;
                            self.find('#divRoomNo.GetDDLRooms').loadDropDowns(true, data).done(function () {

                                var self = $('#pnlClinicalPhoneEncounter #frmClinicalPhoneEncounter');
                                //binding values to form controls
                                Clinical_PhoneEncounter.BindVisitsValues(null, null, null, null, noteData.ProviderId, noteData.Provider, noteData.Facility,
                                    noteData.FacilityId, noteData.VisitReason, noteData.RoomNo, noteData.RefProviderId, noteData.RefProvider, true);
                                $('#pnlClinicalPhoneEncounter #frmClinicalPhoneEncounter #VisitReason').val(noteData.VisitReason);
                                $('#pnlClinicalPhoneEncounter #frmClinicalPhoneEncounter #RoomNo').val(noteData.RoomNo);
                                $('#pnlClinicalPhoneEncounter #frmClinicalPhoneEncounter #NoteTemplate').val(noteData.NoteTemplate);
                                $('#pnlClinicalPhoneEncounter #frmClinicalPhoneEncounter #ddlNoteType').val(noteData.NoteType);
                                var CC = noteData.CheifComplaint;
                                $("#pnlClinicalPhoneEncounter #txtCopayPreviousNote").val(utility.RemoveTimeFromDate(null, noteData.NoteDate) + ((CC != null && CC != '') ? " - " + Clinical_Notes_PreviousNote_detail.CheifComplaint : ""));

                                $('#pnlClinicalPhoneEncounter #ProgressnoteHTML').html($(noteData.NoteText).get(0));
                                $('#pnlClinicalPhoneEncounter #ProgressnoteHTML').find('clinical_vitals').parent().parent().remove();
                                $("#pnlClinicalPhoneEncounter #hfNoteText").val($('#pnlClinicalPhoneEncounter #ProgressnoteHTML').html());
                                $("#pnlClinicalPhoneEncounter #hfPrevNotesId").val(noteData.PrevNotesId);
                                $("#pnlClinicalPhoneEncounter #chkCopayPreviousNote").prop('checked', true);
                                Clinical_PhoneEncounter.UnLinkPreviousNotePatient($("#pnlClinicalPhoneEncounter #chkCopayPreviousNote"), 'pnlClinicalPhoneEncounter');
                                $('#pnlClinicalPhoneEncounter .NoteTemplate').removeClass('disableAll');
                                $('#pnlClinicalPhoneEncounter .NoteType').removeClass('disableAll');

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
        params["RefForm"] = "frmClinicalPhoneEncounter";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "clinicalTabPhoneEncounter";
        LoadActionPan('Admin_Provider', params);
    },

    OpenProviderDetail: function () {
        //Admin_Provider.ProviderEdit($('#pnlClinicalPhoneEncounter #hfProvider').val(),'clinicalTabPhoneEncounter');
        var params = [];
        params["ProviderId"] = $('#pnlClinicalPhoneEncounter #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'clinicalTabPhoneEncounter';
        LoadActionPan('providerDetail', params);
    },

    HideProviderLink: function () {
        $('#pnlClinicalPhoneEncounter #txtProvider').attr("ProviderId", "-1");
        $('#pnlClinicalPhoneEncounter #hfProvider').val("-1");
        $("#pnlClinicalPhoneEncounter #lnkProviderEdit").css("display", "none");
        $("#pnlClinicalPhoneEncounter #lblProvider").css("display", "inline");
    },
    // -------------- End Provider -----------------

    // -------------- Facility ---------------------

    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmClinicalPhoneEncounter";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "clinicalTabPhoneEncounter";
        LoadActionPan('Admin_Facility', params);
    },

    OpenFacilityDetail: function () {
        //Admin_Facility.FacilityEdit($('#pnlClinicalPhoneEncounter #hfFacility').val(), 'clinicalTabPhoneEncounter');
        var params = [];
        params["FacilityId"] = $('#pnlClinicalPhoneEncounter #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'clinicalTabPhoneEncounter';
        params["RefCtrl"] = "txtFacility";
        LoadActionPan('facilityDetail', params);
    },

    BindUserName: function () {
        var UserName = $('#pnlClinicalPhoneEncounter #txtUser').val();
        if (UserName) {
            utility.Keyupdelay(function () {
                var AllPatients = utility.GetUserArray(UserName, 1, 1).done(function (response) {
                    $('#pnlClinicalPhoneEncounter #txtUser').autocomplete({
                        autoFocus: true,
                        source: response,
                        minLength: 0,
                        open: function (event, ui) { disable = true },
                        close: function (event, ui) {
                            disable = false; $(this).focus();
                        },
                        select: function (event, ui) {
                            setTimeout(function () {
                                $('#pnlClinicalPhoneEncounter #txtUser').val(ui.item.value);
                                $('#pnlClinicalPhoneEncounter #hfUserId').val(ui.item.id);
                                if ($('#frmClinicalPhoneEncounter').data('bootstrapValidator') != null && typeof $('#frmClinicalPhoneEncounter').data('bootstrapValidator') != 'undefined') {
                                    $('#frmClinicalPhoneEncounter').bootstrapValidator('revalidateField', 'User');
                                }
                            }, 100);
                        }
                    }).blur(function () {
                        setTimeout(function () {
                            utility.ValidateAutoComplete($('#frmClinicalPhoneEncounter #txtUser'), "frmClinicalPhoneEncounter #hfUserId", false, null, null, null);
                        }, 200);
                    });
                    $('#frmClinicalPhoneEncounter #txtUser').autocomplete("search", "");
                });
            });
        }

    },
    HideFacilityLink: function () {
        $('#pnlClinicalPhoneEncounter #txtFacility').attr("FacilityId", "-1");
        $('#pnlClinicalPhoneEncounter #hfFacility').val("-1");
        $('#pnlClinicalPhoneEncounter #lnkFacilityEdit').css("display", "none");
        $('#pnlClinicalPhoneEncounter #lblFacility').css("display", "inline");
    },
    // -------------- End Facility -----------------

    // -------------- Ref Provider -----------------

    FillRefProviderName: function (RefProviderId, RefProviderName) {
        $('#pnlClinicalPhoneEncounter #txtRefProvider').val(RefProviderName);
        $('#pnlClinicalPhoneEncounter #hfRefProvider').val(RefProviderId);
        UnloadActionPan(Admin_ReferringProvider.params["ParentCtrl"]);
    },

    OpenRefProvider: function () {
        var params = [];
        params["ReferringProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtRefProvider";

        params["ParentCtrl"] = "clinicalTabPhoneEncounter";
        LoadActionPan('Admin_ReferringProvider', params);
    },

    OpenRefProviderDetail: function () {

        var params = [];
        params["ReferringProviderId"] = $('#pnlClinicalPhoneEncounter #hfRefProvider').val();
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtRefProvider";
        params["mode"] = "Edit";
        params["ParentCtrl"] = "clinicalTabPhoneEncounter";

        LoadActionPan('referringproviderDetail', params);
    },

    HideRefProviderLink: function () {
        $('#pnlClinicalPhoneEncounter #hfRefProvider').val("-1");
        $('#pnlClinicalPhoneEncounter #lnkRefProviderEdit').css("display", "none");
        $('#pnlClinicalPhoneEncounter #lblRefProvider').css("display", "inline");
    },
    // -------------- End Ref Provider -------------

    LoadPatientDemogrphic: function () {
        //utility.ClearFormValidation('#pnlDemographic #frmDemographic', true);
        AppPrivileges.GetFormPrivileges("Demographic", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                $('#pnlDemographic  #frmDemographic').resetAllControls();
                if (Patient_Demographic.params.mode == "Add") {
                    //utility.ClearFormValidation('#' + Patient_Demographic.params.PanelID + " #frmDemographic");
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
                        //Patient_Demographic.FillPatientInfo(Patient_Demographic.params.patientID);
                        //Patient_Demographic.params["DemographicId"] = Patient_Demographic.params.patientID;

                    });
                }
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });

    },

    GetNoteInfo: function (NoteId, Obj) {
        if (Obj != null) {
            if ($(Obj.target).is('i[class*="fa-clipboard"]')) {
                Obj.stopPropagation();
            }
        }
        Clinical_ProgressNote.FillNotes(null, NoteId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);

                var FormId = "pnlClinicalPhoneEncounter";
                $.when(Clinical_PhoneEncounter.GetRooms(null, Clinical_Notes_detail.FacilityId)).done(function () {//2 Buss Logic

                    var self = $('#' + FormId);

                    utility.bindMyJSONByName(true, Clinical_Notes_detail, false, self);
                    if ($('#pnlClinicalPhoneEncounter #frmClinicalPhoneEncounter').data("bootstrapValidator") != null && typeof $('#frmClinicalPhoneEncounter').data('bootstrapValidator') != 'undefined') {
                        $("#" + FormId + " #frmClinicalPhoneEncounter").bootstrapValidator('revalidateField', 'VisitReason');
                        $("#" + FormId + " #frmClinicalPhoneEncounter").bootstrapValidator('revalidateField', 'NoteType');
                    }
                    Clinical_PhoneEncounter.bindDateAndTimepicker();//1 Buss Logic
                    $("#pnlClinicalPhoneEncounter #providerDiv").addClass("disableAll");//3 Buss Logic

                    // var html = $(Clinical_Notes_detail.NoteText);
                    $("#pnlClinicalPhoneEncounter #CopyNoteText").html("");
                    $("#pnlClinicalPhoneEncounter #CopyNoteText").append(Clinical_Notes_detail.NoteText);
                    $("#pnlClinicalPhoneEncounter #CopyNoteText").find("section[id*='Cli_Prescription_Main']").remove();
                    $("#pnlClinicalPhoneEncounter #CopyNoteText").find("section[id*='Cli_LabOrderDetail_Main']").remove();
                    //$("#pnlClinicalNotes #CopyNoteText").find("section[id*='Cli_LabResultDetail_Main']").remove();
                    $("#pnlClinicalPhoneEncounter #CopyNoteText").find("section[id*='Cli_ProcedureOrderDetail_Main']").remove();
                    $("#pnlClinicalPhoneEncounter #CopyNoteText").find("section[id*='Cli_RadiologyOrderDetail_Main']").remove();
                    //$("#pnlClinicalNotes #CopyNoteText").find("section[id*='Cli_RadiologyResultDetail_Main']").remove();
                    $("#pnlClinicalPhoneEncounter #CopyNoteText").find("section[id*='Cli_ConsultationOrderDetail_Main']").remove();
                    $("#pnlClinicalPhoneEncounter #CopyNoteText").find("section[id*='Cli_FollowUp_Main']").remove();

                    HTMLNotes = $("#pnlClinicalPhoneEncounter #CopyNoteText").html();
                    HTMLNotes = HTMLNotes.replace(/&quot;/g, '"');
                    HTMLNotes = HTMLNotes.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
                    HTMLNotes = HTMLNotes.replace(/&nbsp;/g, '');
                    $("#pnlClinicalPhoneEncounter #hfNoteText").val(HTMLNotes);

                    $("#pnlClinicalPhoneEncounter #hfPrevNotesId").val(NoteId);
                    $("#pnlClinicalPhoneEncounter #hfVisitId").val("");
                    Clinical_PhoneEncounter.CopyNotes = true;
                    Clinical_PhoneEncounter.PrevNoteId = NoteId;

                    //----------------------
                    var Duration = "";
                    if (Clinical_Notes_detail.Duration && Clinical_Notes_detail.Duration != "") {
                        Duration = Clinical_Notes_detail.Duration.split(':');
                        var self = $("#pnlClinicalPhoneEncounter #DivDuration");

                        self.find("#txtTaskHours").val(Duration[0]);
                        self.find("#txtTaskMinutes").val(Duration[1]);
                        self.find("#txtTaskSeconds").val(Duration[2]);
                    }

                    $("#pnlClinicalPhoneEncounter #txtUser").val(globalAppdata["AppUserNameFullName"]);
                    $("#pnlClinicalPhoneEncounter #hfUserId").val(globalAppdata["AppUserId"]);
                    $("#pnlClinicalPhoneEncounter #txtUser").attr("disabled", true);
                    Clinical_PhoneEncounter.params.IsDisableDateTimeCtrl = true;
                    //----------------------

                    $('#' + Clinical_PhoneEncounter.params.PanelID + ' #frmClinicalPhoneEncounter').submit();


                });
            }
        });
    },
}