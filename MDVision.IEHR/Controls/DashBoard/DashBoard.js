var next = 0;
var increment = 0;
var a = 0;
var kpi;


var VarArrayDelivered = [];

DashBoard = {

    labOrderRows: [],
    params: [],
    bIsFirstLoad: true,
    EditableGridOrder: null,
    EditableGridOrderResult: null,
    labResultRows: [],
    LabItem: null,
    LabResultItem: null,
    myGrid: null,
    arrCQMReasoning: [],
    arrVBPReasoning: [],
    toggleStatus: null,
    UnsolicitedResultCount: 0,
    LabResultCount: 0,
    PendingDocList: [],
    isfromQuickDoc: false,
    bulkSignNotes: [],
    CDSIds: [],
    pageNoReadyToSign: null,
    pageNoMissingInfo: null,
    MissingInfoMarkAsReady: [],
    InnerCtrl: "",
    IsAssignedResultsFromQuick: false,
    SignalRHubTasks : null,

    Load: function (params) {

        try {
            // reset URL #EMR-5433
            if (window.history.replaceState) {

                var url_ = window.location.href.split(".aspx#");
                if (url_.length > 1) {
                    window.history.replaceState(null, null, url_[0] + ".aspx#")
                }

            }
        } catch (e) {
            console.log(e.message);
        }

        $('.DashBoardKPI_Drop').resize(function () { });
        if (DashBoard.bIsFirstLoad) {
            //Start//15-03-2016//Ahmad Raza//hiding CDS ALert icon on dashboard's first load
            $(" #mainForm  li#CDSAlert").hide();
            //$(" #mainForm  li#ImmunizationAlert").hide();
            //End//15-03-2016//Ahmad Raza//hiding CDS ALert icon on dashboard's first load
            DashBoard.bIsFirstLoad = false;

            var self = $('#pnlDashboard #frmDashboard');
            self.loadDropDowns(true).done(function () {
                DashBoard.IntializeMultiSelectDropDown();
                if (globalAppdata.IsProviderBulkSign == "True") {
                    $('#pnlDashboard #pnlEncounterGridOld select[id=OldddlNoteStatus] option:eq(1)').prop('selected', 'selected');
                }
                // 30/11/2015 Muhammad Irfan to select Draft as selected
                $('#pnlDashboard .tabs-custom').eq(0).find('li').eq(0).attr('class', 'active');
                DashBoard.LoadDashBoardSetting(0);
                Patient_Referrals_Incoming_Detail.IntializeMultiSelectDropDownStatusReason('pnlDashboard #ctrlPanIncomingReferral');
            });
            DashBoard.BindProviderLabResultSearch();
            DashBoard.BindProvider();
            DashBoard.BindPatientName();
            DashBoard.BindAppointmentsProvider();
            DashBoard.BindAppointmentsFacility();
            //DashBoard.BindPatientNameAtNoteTab();
            DashBoard.BindPatientChangesProvider();
            DashBoard.BindReferralInsurancePlan();
            DashBoard.BindCCMProvider();
            DashBoard.BindCCMInsurancePlan();
            DashBoard.BindProviderAutoCompPatientportalSignupReq();
            DashBoard.BindModifiedNotesProvider();
            DashBoard.BindLiveRequestProvider();
            DashBoard.BindPatientName_LiveRequest();
            DashBoard.BindDocumentPatientName();
            DashBoard.BindOrderPatientName();
            DashBoard.BindResultPatientName();
            DashBoard.BindPatientAccount();
            DashBoard.BindPatNameForActiveAccounts();
            DashBoard.BindPatAccountForActiveAccounts();
            DashBoard.BindPatientReferral();
        } else {
            if ($('#ActiveWidget').val() == "Orders&Results") {
                DashBoard.LoadDefaultValueForDashboardWidgetLabResultTab();
                if (DashBoard.params.IsAssignedResultsFromQuick != true)
                    DashBoard.DashBoardLabResultLoad(null, null, null);
            }
            // 27/11/2015 Muhammad Irfan to refresh the selected icon
            //$($('.slick-active').map(function () { if ($(this).hasClass('active')) { return this; } })).trigger('click');

            if ($('#ActiveWidget').val() == "Referrals") {

                if ($('#pnlDashboard #pnlReferralsGrid #pnlReferrals ul#ulReferralsTabsItems #listIncomingReferrals').hasClass('active')) {
                    DashBoard.DashBoardIncomingReferralGridLoad();
                }
                else if ($('#pnlDashboard #pnlReferralsGrid #pnlReferrals ul#ulReferralsTabsItems #listOutgoingReferrals').hasClass('active')) {
                    DashBoard.DashBoardOutgoingReferralGridLoad();
                }
                //DashBoard.DashBoardReferralSearch();
            }

            if ($('#ActiveWidget').val() == "Appointments") {
                DashBoard.DashBoardAppointmentSearch(null, null, null, null);
            }
            if ($('#ActiveWidget').val() == "CCM") {
                DashBoard.params["IsCCMLoaded"] = true;
                DashBoard.DashBoardCCMEnrollmentInfoSearch(1, 1000, null, null);
            }
            if ($('#ActiveWidget').val() == "Encounter") {
                if (globalAppdata.IsProviderBulkSign == "True") {
                    if ($("#pnlDashboard #pnlEncounterGrid #listReadyToSign").hasClass("active")) {
                        DashBoard.SearchDashBoardEncounter(null, null, null);
                    } else if ($("#pnlDashboard #pnlEncounterGrid #listSigned").hasClass("active")) {
                        DashBoard.DashBoardEncounterSearchSigned(null, null, null);
                    } else if ($("#pnlDashboard #pnlEncounterGrid #listMissingInfo").hasClass("active")) {
                        DashBoard.DashBoardEncounterSearchMissingInfo(null, null, null);
                    } else {
                        DashBoard.SearchDashBoardEncounter(null, null, null);
                    }
                    Clinical_Notes.LoadAllAutocomplete();
                } else {
                    DashBoard.DashBoardEncounterSearchOld(null, null, null);
                }
            }
            if ($('#ActiveWidget').val() == "Tasks") {
                DashBoard.DashBoardTasksSearch(null, null, null);
            }

        }
        if ($('#pnlDashboard .prev1-next1').length > 0) {
            $('#pnlDashboard .slider-wrapper').fadeOut();
            $('#pnlDashboard .prev1-next1').click();
            $('#pnlDashboard .slider-wrapper').fadeIn(1000);
        }

        //to Set DashBoard Widget.
        try {
            if ($('#pnlDashboard .slide-ul .slick-track').css("width") == "0px") {
                $('#pnlDashboard .slide-ul').unslick();
                DashBoard.InitalizeWidget();
            }

        } catch (ex) {
            console.log(ex);
        }

        //if (DashBoard.params["IsCCMLoaded"] != true) {
        //    DashBoard.LoadCCMEnrollmentInfo(1, 999, "pending").done(function (response) {
        //        if (response.status != false) {
        //            if (response.CCMEnrollmentInfoCount > 0) {
        //                $("#pnlDashboard div[id='CCM'] .badge").css("display", "inline");
        //                $("#spnCCM").text(response.CCMEnrollmentInfoCount);
        //            }
        //        }
        //    });
        //}

        utility.CreateMonthViewDatePicker("pnlDashboard #frmDashboard #dpMonth", function () { }, true, null, null);
        utility.CreateDatePicker("pnlDashboard #frmDashboard #dtpMsgDate", function () { }, false, null, null);
        utility.CreateDatePicker("pnlDashboard #frmDashboard #ctrlPanPatPortalAccounts #dpDOB", function () { }, false, null, null);
        utility.CreateDatePicker("pnlDashboard #frmDashboard #ctrlPanIncomingReferral #dpDateStatusReason", function () { }, false, null, null);

        $('#pnlDashboard #frmDashboard #ctrlPanIncomingReferral #timeStatusReason').timepicker({});
        $('#pnlDashboard #frmDashboard #ctrlPanIncomingReferral #timeStatusReason').timepicker('setTime', "");

        DashBoard.documentReady(true);
        DashBoard.LoadCDSAlert();
    },

    documentReady: function (reload) {

        $("#pnlDashboard  #txtFullName").on('keyup keypress', function (e) {
            var keyCode = e.keyCode || e.which;
            if (keyCode === 13) {
                e.preventDefault();
                return false;
            }
        });
        $("#pnlDashboard #pnlTCMGrid #txtProvider").on('keyup keypress', function (e) {
            var keyCode = e.keyCode || e.which;
            if (keyCode === 13) {
                e.preventDefault();
                return false;
            }
        });
        $("#pnlDashboard #pnlDataChangeRequestGrid #txtProvider").on('keyup keypress', function (e) {
            var keyCode = e.keyCode || e.which;
            if (keyCode === 13) {
                e.preventDefault();
                return false;
            }
        });
        $("#pnlDashboard #pnlDataChangeRequestGrid #txtFullName").on('keyup keypress', function (e) {
            var keyCode = e.keyCode || e.which;
            if (keyCode === 13) {
                e.preventDefault();
                return false;
            }
        });
        $("#pnlDashboard #pnlLabOrderGrid #txtResultFullName").on('keyup keypress', function (e) {
            var keyCode = e.keyCode || e.which;
            if (keyCode === 13) {
                e.preventDefault();
                return false;
            }
        });
        $("#pnlDashboard #pnlLabOrderGrid #txtOrderFullName").on('keyup keypress', function (e) {
            var keyCode = e.keyCode || e.which;
            if (keyCode === 13) {
                e.preventDefault();
                return false;
            }
        });


        $("#pnlDashboard  #txtProvider").on('keyup keypress', function (e) {
            var keyCode = e.keyCode || e.which;
            if (keyCode === 13) {
                e.preventDefault();
                return false;
            }
        });

        $("#pnlDashboard  #txtFacility").on('keyup keypress', function (e) {
            var keyCode = e.keyCode || e.which;
            if (keyCode === 13) {
                e.preventDefault();
                return false;
            }
        });

        $("#pnlDashboard #ctrlPanPatPortalAccounts #txtFullName").on('keyup keypress', function (e) {
            var keyCode = e.keyCode || e.which;
            if (keyCode === 13) {
                e.preventDefault();
                return false;
            }
        });

        $("#pnlDashboard #ctrlPanPatPortalAccounts #txtAccountNumber").on('keyup keypress', function (e) {
            var keyCode = e.keyCode || e.which;
            if (keyCode === 13) {
                e.preventDefault();
                return false;
            }
        });

        $("#pnlDashboard #ctrlPanPatPortalAccounts #dpDOB").on('keyup keypress', function (e) {
            var keyCode = e.keyCode || e.which;
            if (keyCode === 13) {
                e.preventDefault();
                return false;
            }
        });

        var today = new Date();
        if ($('#pnlDashboard #appDate').text() == "") {
            var cur = $.datepicker.formatDate('mm/dd/yy', new Date(today));
            $('#pnlDashboard #appDate').text(cur);
        }
        $('#pnlDashboard #appDate').datepicker({
            format: "mm/dd/yyyy",
            todayHighlight: true,
            defaultViewDate: $.datepicker.formatDate('mm/dd/yy', new Date($('#pnlDashboard #appDate').text())),
        }).on('changeDate', function (e) {
            $(this).datepicker('hide');
            var newDate = $.datepicker.formatDate('mm/dd/yy', new Date(e.date));
            if (newDate != $('#pnlDashboard #appDate').text()) {
                $('#pnlDashboard #appDate').text(newDate);
                DashBoard.DashBoardAppointmentSearch(null, null, null, $.datepicker.formatDate('mm/dd/yy', new Date(e.date)));
            }
        });
        utility.CreateDatePicker('pnlDashboard #ctrlPanReadyToSign #dpVisitFromR', function () {
            if ($('#pnlDashboard #ctrlPanReadyToSign #dpVisitFromR').val() != '') {
                $('#pnlDashboard #ctrlPanReadyToSign #dpVisitToR').removeAttr('disabled');
            } else {
                $('#pnlDashboard #ctrlPanReadyToSign #dpVisitToR').attr('disabled', true); $('#pnlDashboard #ctrlPanReadyToSign #dpVisitToR').val('');
            }
        })
        $('#pnlDashboard #ctrlPanReadyToSign #dpVisitFromR').on('blur', function () {
            if ($('#pnlDashboard #ctrlPanReadyToSign #dpVisitFromR').val() != '') {
                $('#pnlDashboard #ctrlPanReadyToSign #dpVisitToR').removeAttr('disabled');
            } else {
                $('#pnlDashboard #ctrlPanReadyToSign #dpVisitToR').attr('disabled', true); $('#pnlDashboard #ctrlPanReadyToSign #dpVisitToR').val('');
            }
        });
        utility.CreateDatePicker('pnlDashboard #ctrlPanReadyToSign #dpVisitToR', function () {

        });

        //
        utility.CreateDatePicker('pnlDashboard #ctrlPanSigned #dpVisitFromS', function () {
            if ($('#pnlDashboard #ctrlPanSigned #dpVisitFromS').val() != '') {
                $('#pnlDashboard #ctrlPanSigned #dpVisitToS').removeAttr('disabled');
            } else {
                $('#pnlDashboard #ctrlPanSigned #dpVisitToS').attr('disabled', true); $('#pnlDashboard #ctrlPanSigned #dpVisitToS').val('');
            }
        })
        $('#pnlDashboard #ctrlPanSigned #dpVisitFromS').on('blur', function () {
            if ($('#pnlDashboard #ctrlPanSigned #dpVisitFromS').val() != '') {
                $('#pnlDashboard #ctrlPanSigned #dpVisitToS').removeAttr('disabled');
            } else {
                $('#pnlDashboard #ctrlPanSigned #dpVisitToS').attr('disabled', true); $('#pnlDashboard #ctrlPanSigned #dpVisitToS').val('');
            }
        });
        utility.CreateDatePicker('pnlDashboard #ctrlPanSigned #dpVisitToS', function () {

        });

        //
        utility.CreateDatePicker('pnlDashboard #ctrlMissingInfo #dpVisitFromM', function () {
            if ($('#pnlDashboard #ctrlMissingInfo #dpVisitFromM').val() != '') {
                $('#pnlDashboard #ctrlMissingInfo #dpVisitToM').removeAttr('disabled');
            } else {
                $('#pnlDashboard #ctrlMissingInfo #dpVisitToM').attr('disabled', true); $('#pnlDashboard #ctrlMissingInfo #dpVisitToM').val('');
            }
        })
        utility.CreateDatePicker('pnlDashboard #OldgridControlEncounter #OlddpVisitFrom', function () {
            if ($('#pnlDashboard #OldgridControlEncounter #OlddpVisitFrom').val() != '') {
                $('#pnlDashboard #OldgridControlEncounter #OlddpVisitTo').removeAttr('disabled');
            } else {
                $('#pnlDashboard #OldgridControlEncounter #OlddpVisitTo').attr('disabled', true);
                $('#pnlDashboard #OldgridControlEncounter #OlddpVisitTo').val('');
            }
        }, false, null, null);
        utility.CreateDatePicker('pnlDashboard #OldgridControlEncounter #OlddpVisitTo', function () {
        }, false, null, null);
        $('#pnlDashboard #ctrlMissingInfo #dpVisitFroMm').on('blur', function () {
            if ($('#pnlDashboard #ctrlMissingInfo #dpVisitFromM').val() != '') {
                $('#pnlDashboard #ctrlMissingInfo #dpVisitToM').removeAttr('disabled');
            } else {
                $('#pnlDashboard #ctrlMissingInfo #dpVisitToM').attr('disabled', true); $('#pnlDashboard #ctrlMissingInfo #dpVisitToM').val('');
            }
        });
        utility.CreateDatePicker('pnlDashboard #ctrlMissingInfo #dpVisitToM', function () {

        });


        //entrydatefrom and entrydateto
        utility.CreateDatePicker('pnlDashboard #dpEntryDateFrom', function () {

        });
        DashBoard.CreateDatePickerOfModifiedNote();
        if ($('#pnlDashboard #dpEntryDateFrom').val() != '') {
            $('#pnlDashboard #dpEntryDateTo').removeAttr('disabled');
        } else {
            $('#pnlDashboard #dpEntryDateTo').attr('disabled', true); $('#pnlDashboard #dpEntryDateTo').val('');
        }
        $('#pnlDashboard #dpEntryDateFrom').on('blur', function () {
            if ($('#pnlDashboard #dpEntryDateFrom').val() != '') {
                $('#pnlDashboard #dpEntryDateTo').removeAttr('disabled');
            } else {
                $('#pnlDashboard #dpEntryDateTo').attr('disabled', true); $('#pnlDashboard #dpEntryDateTo').val('');
            }
        });

        utility.CreateDatePicker('pnlDashboard #dpEntryDateTo', function () {

        });
        if ($('#pnlDashboard #dpEntryDateTo').val() != '') {
            $('#pnlDashboard #dpEntryDateTo').removeAttr('disabled');
        }
        //
        //DOSfrom and DOSto
        utility.CreateDatePicker('pnlDashboard #dpDOSDateFrom', function () {

        });
        if ($('#pnlDashboard #dpDOSDateFrom').val() != '') {
            $('#pnlDashboard #dpDOSDateTo').removeAttr('disabled');
        } else {
            $('#pnlDashboard #dpDOSDateTo').attr('disabled', true); $('#pnlDashboard #dpDOSDateTo').val('');
        }
        $('#pnlDashboard #dpDOSDateFrom').on('blur', function () {
            if ($('#pnlDashboard #dpDOSDateFrom').val() != '') {
                $('#pnlDashboard #dpDOSDateTo').removeAttr('disabled');
            } else {
                $('#pnlDashboard #dpDOSDateTo').attr('disabled', true); $('#pnlDashboard #dpDOSDateTo').val('');
            }
        });

        utility.CreateDatePicker('pnlDashboard #dpDOSDateTo', function () {

        });
        if ($('#pnlDashboard #dpDOSDateTo').val() != '') {
            $('#pnlDashboard #dpDOSDateTo').removeAttr('disabled');
        }
        DashBoard.SwicthWidgetInializatoin();


        //if ($('#pnlDashboard .ios-switch').length > 1) {
        //    $('#pnlDashboard .ios-switch').not($('#pnlDashboard .ios-switch')[$('#pnlDashboard .ios-switch').length - 1]).remove();
        //}

        utility.CreateMonthViewDatePicker("pnlDashboard #dpMonth", function () { }, false, null, null);
        if (reload) {
            //For Lab Order, Result and Unsolicited
            var panelId = 'pnlDashboard';
            //Load All Users
            CacheManager.BindDropDownsByID('#pnlDashboard #ddlAssigneeTemplate', 'GetUsersFullName', true, 1).done(function () {
                //childRows.find('#ddlAssigneeId').val(item.AssigneeId)
            });
            Clinical_LabOrder.LoadLabs('ddllabUnsoResultLaboratory, #' + panelId + ' #ddllabResultLabId, #' + panelId + ' #ddlLabId', panelId);
            //Clinical_LabOrder.LoadLabs('ddllabResultLabId', panelId);
            //Clinical_LabOrder.LoadLabs('ddlLabId', panelId);
        }
        //------------------------------

        if ($('#pnlDashboard #appRequestDate').text() == "") {
            var cur = $.datepicker.formatDate('mm/dd/yy', new Date(today));
            $('#pnlDashboard #appRequestDate').text(cur);
        }
        $('#pnlDashboard #appRequestDate').datepicker({
            format: "mm/dd/yyyy",
            todayHighlight: true,
            defaultViewDate: $.datepicker.formatDate('mm/dd/yy', new Date($('#pnlDashboard #appRequestDate').text())),
        }).on('changeDate', function (e) {
            $(this).datepicker('hide');
            var newDate = $.datepicker.formatDate('mm/dd/yy', new Date(e.date));
            if (newDate != $('#pnlDashboard #appRequestDate').text()) {
                $('#pnlDashboard #appRequestDate').text(newDate);
                //DashBoard.DashBoardAppRequestSearch();
            }
        });


        // This is a temporary Implementation until we apply SignalR for all Quicj Links
        setInterval(function () {

            DashBoard.RefreshTasksCount().done(function (responseTasksCount) {

                if (responseTasksCount > 0) {
                    if (responseTasksCount != Number($("#spnUserTasksCount").html())) {
                        $("#spnUserTasksCount").empty();
                        $("#spnUserTasksCount").html(responseTasksCount);
                    }
                }
                else {

                }
            })

        }, 10000);
        
    },

    RefreshTasksCount: function () {
        var objData = {
        };
        objData["AppUserId"] = globalAppdata['AppUserId'];

        var data = JSON.stringify(objData);
        return MDVisionService.APIServiceSync(data, "Messages", "TasksCount");
    },

    TasksCountHub: function () {
        try {

            $.connection.hub.start()
                            .done(function () {
                                console.log('You have connected to the server.');
                                DashBoard.SignalRHubTasks.server.getTasksCount().done(function (TasksCount) {


                                })

                            })


            //$.connection.hub.stop();
            //DashBoard.SignalRHubTasks = null;

            //$.connection.hub.disconnected(function () {
            //    setTimeout(function () {
            //        $.connection.hub.start();
            //    }, 6000); // Restart connection after 5 seconds. 
            //});

            DashBoard.SignalRHubTasks = $.connection.providerNoteAccessHub;

            DashBoard.SignalRHubTasks.client.RefreshTasks = function (TasksCount) {
               // response = JSON.parse(response);
                $("#spnUserTasksCount").html("2500");
            };

           // $.connection.hub.start()
           //.done(function () {
           //    console.log('You have connected to the server.');
           //})
           //.fail(function () {
           //    console.log('You could not have connected to the server.');
            //});

          
        } catch (e) {
            console.log(e.message);
        }


       

    },

    CreateDatePickerOfModifiedNote: function () {
        utility.CreateDatePicker('pnlDashboard #dpModifiedNoteFrom', function () {
            if ($('#pnlDashboard #dpModifiedNoteFrom').val() != '') {
                $('#pnlDashboard #dpModifiedNoteTo').removeAttr('disabled');
            } else {
                $('#pnlDashboard #dpModifiedNoteTo').attr('disabled', true); $('#pnlDashboard #dpModifiedNoteTo').val('');
            }
        })
        $('#pnlDashboard #dpModifiedNoteFrom').on('blur', function () {
            if ($('#pnlDashboard #dpModifiedNoteFrom').val() != '') {
                $('#pnlDashboard #dpModifiedNoteTo').removeAttr('disabled');
            } else {
                $('#pnlDashboard #dpModifiedNoteTo').attr('disabled', true); $('#pnlDashboard #dpModifiedNoteTo').val('');
            }
        });
        utility.CreateDatePicker('pnlDashboard #dpModifiedNoteTo', function () {

        });
    },
    SwapArray: function (array) {
        var DocObjIndex = $.map(array, function (obj, index) {
            if (obj.WidgetsName == "Documents")
                return index;
        });
        var MsgsObjIndex = $.map(array, function (obj, index) {
            if (obj.WidgetsName == "Messages")
                return index;
        });
        var tmp = array[MsgsObjIndex];
        array[MsgsObjIndex] = array[DocObjIndex];
        array[DocObjIndex] = tmp;

        return array;
    },
    //#region Widget
    LoadDashBoardSetting: function (incdec) {
        var currentwidgetload;

        // Appointment
        var myJSON = new Object();
        myJSON.SlotDate = $.datepicker.formatDate('mm/dd/yy', new Date());
        //Message
        myJSON.Status = 2;
        myJSON.RowsPerPage = 15;
        myJSON.PageNumber = 1;
        myJSON.AppointmentDateFrom = $.datepicker.formatDate('mm/dd/yy', new Date());
        myJSON.AppointmentDateTo = $.datepicker.formatDate('mm/dd/yy', new Date());
        //document
        myJSON.FromEntry = $.datepicker.formatDate('mm/dd/yy', new Date());
        myJSON.ToEntry = $.datepicker.formatDate('mm/dd/yy', new Date());
        var myString = JSON.stringify(myJSON);

        //Load Patient Changes Grid by default
        //DashBoard.LoadWidgetControl('PatientChanges', null, null);
        IsBackgroundLoaderShow = false;
        DashBoard.LoadDashBoard(myString, 0, null).done(function (response) {
            IsBackgroundLoaderShow = true;
            if (response.status != false) {
                var isrecord = 0;
                //var TopCountElementId = 0;
                //var TopCountElementCount = 0;
                //var FirstElementId = 0;

                var dashboardsettings = JSON.parse(response.DASHBOARDSETTING_WIDGET_JSON);
                if ($('#spnPrescriptionsRefillCount').text() == '0' || $('#spnPrescriptionsRefillCount').text() == '') {
                    $('#spnPrescriptionsRefillCount').hide();
                } else {
                    $('#spnPrescriptionsRefillCount').show();
                }

                dashboardsettings = DashBoard.SwapArray(dashboardsettings);

                // $('#spnPrescriptionsRefillCount').text(response.refillPrescriptionCount);

                for (var d = 0; d < dashboardsettings.length; d++) {
                    if (dashboardsettings[d].IsActive == "True") {
                        isrecord++;
                        var InlineStyle = "style='display:none'";
                        if (dashboardsettings[d].Count > 0) {
                            InlineStyle = "style='display:inline'";
                        }

                        var WidgetsName = dashboardsettings[d].WidgetsName;
                        dashboardsettings[d].WidgetsName = dashboardsettings[d].WidgetsName.replace(/\s+/g, '');

                        //if (isrecord == 1)
                        //    FirstElementId = dashboardsettings[d].WidgetsName;

                        //if (TopCountElementCount == 0) {
                        //    TopCountElementId = dashboardsettings[d].WidgetsName;
                        //    TopCountElementCount = parseInt(dashboardsettings[d].Count);

                        //}
                        //if (TopCountElementCount < dashboardsettings[d].Count) {
                        //    TopCountElementId = dashboardsettings[d].WidgetsName;
                        //    TopCountElementCount = parseInt(dashboardsettings[d].Count);
                        //}
                        var WidgetName = dashboardsettings[d].WidgetsName;
                        if (WidgetName == "Orders &amp; Results" || WidgetName == "Orders&amp;Results") {
                            WidgetName = "OrdersResults";
                        } else if (WidgetName == "Messages") {
                            if (dashboardsettings[d].Count != '') {
                                var msgCountObj = JSON.parse(dashboardsettings[d].Count.replace(/&quot;/g, "\""));
                                var totalCount = 0;
                                $.each(msgCountObj, function (index, element) {
                                    if (element.MessageType != "Task") {
                                        totalCount += parseInt(element.MessageCounts);
                                    }

                                });
                                dashboardsettings[d].Count = totalCount;
                                InlineStyle = "style='display:inline'";
                                if (totalCount == 0) {
                                    $('.notifications #Messages .badge').hide();
                                    $('#wpanel div.wMessages .badge').hide();
                                } else {
                                    $('#wpanel div.wMessages .badge').show();
                                    $('.notifications #Messages .badge').show();
                                    $('.notifications #Messages .badge').text(totalCount);
                                }



                                $('#spnMessageCount').text(totalCount == 0 ? '' : totalCount);
                                $('#liMsgsPractice span').text(msgCountObj[1].MessageCounts == 0 ? '' : msgCountObj[1].MessageCounts);
                                $('#liMsgsPatient span').text(msgCountObj[2].MessageCounts == 0 ? '' : msgCountObj[2].MessageCounts);
                                $('#liMsgsDirect span').text(msgCountObj[0].MessageCounts == 0 ? '' : msgCountObj[0].MessageCounts);
                            }
                        }

                        if (WidgetsName != null && WidgetsName == "Active Accounts") {
                            if (dashboardsettings[d].Count != '') {
                                var activeAccountscount = JSON.parse(dashboardsettings[d].Count.replace(/&quot;/g, "\""));
                                dashboardsettings[d].Count = activeAccountscount.PatientPortalAccountsCount;
                                InlineStyle = "style='display:none'";
                                if (activeAccountscount.PatientPortalAccountsCount > 0) {
                                    InlineStyle = "style='display:inline'";
                                }
                            }
                        }
                        if (WidgetsName != null && WidgetsName == "ccm") {
                            var ccmcount = json.parse(dashboardsettings[d].count.replace(/&quot;/g, "\""));
                            ccmcount = parseint(ccmcount);
                            if (ccmcount > 0) {
                                $("#pnldashboard div[id='ccm'] .badge").css("display", "inline");
                                $("#pnldashboard div[id='ccm'] .badge").text(ccmcount);
                            }

                        }
                        if (WidgetsName != null && WidgetsName == "Data Change Request") {
                            if (dashboardsettings[d].Count != '') {
                                var DataChangeRequestCount = JSON.parse(dashboardsettings[d].Count.replace(/&quot;/g, "\""));
                                DataChangeRequestCount = parseint(DataChangeRequestCount);
                                if (DataChangeRequestCount > 0) {
                                    $("#pnldashboard div[id='DataChangeRequest'] .badge").css("display", "inline");
                                    $("#pnldashboard div[id='DataChangeRequest'] .badge").text(DataChangeRequestCount);
                                }
                            }

                        }
                        if (WidgetsName != null && WidgetsName == "Orders &amp; Results") {
                            if (dashboardsettings[d].Count != '') {
                                var labCountObj = JSON.parse(dashboardsettings[d].Count.replace(/&quot;/g, "\""));
                                $("#spnDashboard_LabOrderCount").html(labCountObj[0].LabOrderResultRecordCount == 0 ? '' : labCountObj[0].LabOrderResultRecordCount);
                                $("#pnlDashboard #spnListLabResultCount").text(labCountObj[0].LabOrderRecordCount == 0 ? '' : labCountObj[0].LabOrderRecordCount);
                                $("#pnlDashboard #spnListLabUnsolicitedCount").text(labCountObj[0].LabOrderUnsolicitedCount == 0 ? '' : labCountObj[0].LabOrderUnsolicitedCount);
                                $("#pnlDashboard #spnAbnormalResultCount").html(labCountObj[0].AbnormalResultCount == 0 ? '' : '<button class="btn btn-danger btn-xs tab_space" onclick="DashBoard.LoadAbmormalLabResults();">' + labCountObj[0].AbnormalResultCount + ' Abnormal Lab Result(s)</button>');
                                dashboardsettings[d].Count = labCountObj[0].LabOrderResultRecordCount;
                                InlineStyle = "style='display:none'";
                                if (labCountObj[0].LabOrderResultRecordCount > 0) {
                                    InlineStyle = "style='display:inline'";
                                }
                            }
                        }
                        $('#widgetpanel').append('<div id="' + dashboardsettings[d].WidgetsName + '" class="widgetDiv li w' + WidgetName + '" onclick="DashBoard.LoadWidgetControl(' + "'" + dashboardsettings[d].WidgetsName + "'" + ',this);"><a href="#" class="btn-widget-close  closeBtn hidden" onclick="DashBoard.DeleteWidget(' + dashboardsettings[d].DBSId + ')"><i class="fa fa-close"></i></a>  <span class="slide-title" title="' + WidgetsName + '">' + WidgetsName + '</span>  <span class="icon"><i class="' + dashboardsettings[d].ImageName + '"></i><span id="spn' + WidgetName + '" class="badge" ' + InlineStyle + '>' + dashboardsettings[d].Count + '</span></span> </div>');
                        if ($('#wpanel div.wMessages .badge').text() == "0") {
                            $('#wpanel div.wMessages .badge').hide();
                        }
                        $('#widhtml').show();

                        if ($('#widgetpanel #Encounter .slide-title').text() == "Encounter")
                            $('#widgetpanel #Encounter .slide-title').text('Notes');
                    }
                }
                var notesCount = $('#spnNotesCount').text() == "" ? '0' : $('#spnNotesCount').text();
                if (notesCount == "0") {
                    $('#pnlDashboard div.wEncounter .badge').hide();
                    $('#wpanel #widgetpanel .slick-track div').eq(24).find('span:last').hide();
                } else {
                    $('#wpanel .slick-track div').each(function (i) {
                        if ($(this).find('span:first').text() == 'Notes') {
                            $(this).find('span:last').text($('#spnNotesCount').text());
                            $(this).find('span:last').show();
                        }

                    });
                    $('#pnlDashboard div.wEncounter .badge').css("display", "inline");
                    // $('#wpanel #widgetpanel .slick-track div').eq(24).find('span:last').show();
                    $('#pnlDashboard div.wEncounter .badge').text($('#spnNotesCount').text());
                    // $('#wpanel #widgetpanel .slick-track div').eq(24).find('span:last').text($('#spnNotesCount').text());

                }
                if (dashboardsettings.length > 0) {
                    if (dashboardsettings[0].IsActive == "True") {
                        var _name = $('#widgetpanel div:first-child').attr("id");
                        $('#widgetpanel div:first-child').addClass("active");
                        DashBoard.LoadWidgetControl(_name, null, null);
                    }
                    else {
                        DashBoard.EnableDisableControl();
                    }
                }

                if (isrecord != 0) {
                    DashBoard.InitalizeWidget();
                    $('#pnlDashboard .slick-cloned span').each(function (ee) {
                        if ($(this).text() == currentwidgetload) {
                            $(this).parent(".slick-cloned").addClass('active');
                        }
                    });

                    //Load Grid of Element that have more count
                    //if (TopCountElementId != 0) {
                    //    var resObj = JSON.parse(response.DASHBOARDSETTING_KPI_WIDGET_GRID_JSON);
                    //    if (TopCountElementCount == 0) {
                    //        DashBoard.LoadWidgetControl('' + FirstElementId + '', null, resObj);
                    //        $('#pnlDashboard #' + FirstElementId).addClass('active');
                    //    } else {
                    //        DashBoard.LoadWidgetControl('' + TopCountElementId + '', null, resObj);
                    //        $('#pnlDashboard #' + TopCountElementId).addClass('active');
                    //    }

                    //}
                } else
                    $('#widhtml').css('display', 'none');

                DashBoard.LoadKPISettings(response);

            }
            else {

                utility.DisplayMessages(response.Message, 3);
            }
            $("#pnlDashboard div.widgetDiv").hover(function () {
                $(this).find('.closeBtn').removeClass("hidden")
            }, function () {
                //if (!$(this).find('.closeBtn').hasClass("hidden")) {
                $(this).find('.closeBtn').addClass("hidden")
                //}
            });
        });



    },

    LoadDashboardAppointmentCount: function () {
        DashBoard.loaddashboardAppointmentLabelCount().done(function (response) {
            if (response.status != false) {
                var resAppointmentCount = JSON.parse(response.AppointmentsCount);
                if (resAppointmentCount == undefined || resAppointmentCount == null || resAppointmentCount == 0) {
                    $('#pnlDashboard div.wAppointments .badge').css("display", "none");
                    // AST-461
                    $('#Appointments #spnAppCount').css("display", "none");
                } else {
                    $('#pnlDashboard div.wAppointments .badge').css("display", "inline");
                    $('#pnlDashboard div.wAppointments .badge').text(resAppointmentCount);
                    // AST-461
                    $('#Appointments #spnAppCount').css("display", "inline");
                    $('#Appointments #spnAppCount').text(resAppointmentCount);
                }
            }
        });
    },


    InitalizeWidget: function () {
        $('.slide-ul').slick({

            autoplay: false,
            speed: 300,
            centerMode: false,
            slidesToShow: 9,
            slidesToScroll: 1,
            responsive: [
                {
                    breakpoint: 1200,
                    settings: {
                        slidesToShow: 5,
                        slidesToScroll: 1,
                        infinite: true
                    }
                },
                {
                    breakpoint: 950,
                    settings: {
                        slidesToShow: 4,
                        slidesToScroll: 1
                    }
                },
                {
                    breakpoint: 694,
                    settings: {
                        slidesToShow: 2,
                        slidesToScroll: 1
                    }
                }
              ,
                {
                    breakpoint: 450,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1
                    }
                }
            ]
        });
    },
    LoadWidgetControl: function (id, ev, GridData) {
        DashBoard.EnableDisableControl();
        var ctrl = id.id;
        if (ctrl == undefined)
            ctrl = id[0].id;
        if (ctrl == undefined)
            ctrl = id;
        $('#ActiveWidget').val(ctrl);
        switch (ctrl) {
            case 'Payments':
                if (GridData && GridData.Payments)
                    DashBoard.DashBoardPaymentsSearch(null, null, JSON.parse(GridData.Payments));
                else
                    DashBoard.DashBoardPaymentsSearch(null, null, null);
                break;
            case 'TCM':
                // if (GridData && GridData.Payments)
                //     DashBoard.DashBoardTCMSearch(null, null, JSON.parse(GridData.Payments));
                // else
                DashBoard.DashBoardTCMSearch(null, null, null);
                break;
            case 'Appointments':
                //Start 13-10-2017 Humaira Yousaf IMP-1195
                $('#pnlDashboard #pnlSchAppointmentGrid #hfProviderId').val("");
                $('#pnlDashboard #pnlSchAppointmentGrid #hfFacilityId').val("");
                $('#pnlDashboard #pnlSchAppointmentGrid #txtProvider').val("");
                $('#pnlDashboard #pnlSchAppointmentGrid #txtFacility').val("");
                DashBoard.IntializeMultiSelectDropDown();
                //End 13-10-2017 Humaira Yousaf IMP-1195
                if (GridData && GridData.Appointments)
                    DashBoard.DashBoardAppointmentSearch(null, null, JSON.parse(GridData.Appointments), null, true);
                else
                    DashBoard.DashBoardAppointmentSearch(null, null, null, null, true);
                break;
            case 'Messages':
                if (GridData && GridData.Messages)
                    DashBoard.DashBoardMessagesSearch(null, null, JSON.parse(GridData.Messages));
                else {
                    $("#pnlDashboard #pnlUserMessagesGrid").show();
                    if (globalAppdata['IsDirectAddress'] != 'True') {
                        $('#pnlUserMessagesGrid .tabs-custom #liMsgsDirect').hide();
                    }

                    if (globalAppdata["isTransitionCareDirectProject"] && globalAppdata["isTransitionCareDirectProject"].toLowerCase() == "false")
                        $("#pnlDashboard #widgetgridpanel #pnlUserMessagesGrid #liMsgsDirectOutgoing").addClass("hidden");
                    else
                        $("#pnlDashboard #widgetgridpanel #pnlUserMessagesGrid #liMsgsDirectOutgoing").removeClass("hidden");

                    if (globalAppdata["DefaultTabMessages"] == 1) {
                        if (globalAppdata["RecentMessagesTab"] == "Patient") {
                            if ($("#pnlDashboard #pnlUserMessagesGrid #msgsPatient").css("display") == "none") {
                                $("#pnlDashboard #pnlUserMessagesGrid #msgsPractice").hide();
                                $("#pnlDashboard #pnlUserMessagesGrid #msgsPatient").show();
                                $("#pnlDashboard #pnlUserMessagesGrid #msgsLog").hide();
                            }
                            DashBoard.DashBoardPatientMessagesSearch();

                        } else {
                            if ($("#pnlDashboard #pnlUserMessagesGrid #msgsPractice").css("display") == "none") {
                                $("#pnlDashboard #pnlUserMessagesGrid #msgsPractice").show();
                                $("#pnlDashboard #pnlUserMessagesGrid #msgsPatient").hide();
                                $("#pnlDashboard #pnlUserMessagesGrid #msgsLog").hide();
                            }
                            DashBoard.DashBoardMessagesSearch();
                        }
                    } else if (globalAppdata["DefaultTabMessages"] == 2) {
                        if ($("#pnlDashboard #pnlUserMessagesGrid #msgsPatient").css("display") == "none") {
                            $("#pnlDashboard #pnlUserMessagesGrid #msgsPractice").hide();
                            $("#pnlDashboard #pnlUserMessagesGrid #msgsPatient").show();
                            $("#pnlDashboard #pnlUserMessagesGrid #msgsLog").hide();
                        }
                        DashBoard.DashBoardPatientMessagesSearch();
                    } else if (globalAppdata["DefaultTabMessages"] == 3) {
                        if ($("#pnlDashboard #pnlUserMessagesGrid #msgsPractice").css("display") == "none") {
                            $("#pnlDashboard #pnlUserMessagesGrid #msgsPractice").show();
                            $("#pnlDashboard #pnlUserMessagesGrid #msgsPatient").hide();
                            $("#pnlDashboard #pnlUserMessagesGrid #msgsLog").hide();
                        }
                        DashBoard.DashBoardMessagesSearch();
                    } else if (globalAppdata["DefaultTabMessages"] == 4) {
                        if ($("#pnlDashboard #pnlUserMessagesGrid #msgsLog").css("display") == "none") {
                            $("#pnlDashboard #pnlUserMessagesGrid #msgsPractice").hide();
                            $("#pnlDashboard #pnlUserMessagesGrid #msgsPatient").hide();
                            $("#pnlDashboard #pnlUserMessagesGrid #msgsLog").show();
                        }
                        DashBoard.DashBoardLogMessagesSearch();
                    }
                }

                break;
            case 'Tasks':
                if (GridData && GridData.Tasks)
                    DashBoard.DashBoardTasksSearch(null, null, JSON.parse(GridData.Tasks));
                else
                    DashBoard.DashBoardTasksSearch(null, null, null);
                break;
            case 'Encounter':
                if (globalAppdata.IsProviderBulkSign == "True") {
                    $('#pnlDashboard #pnlEncounterGrid #ctrlPanReadyToSign #ddlNoteType').val(1); // Progress Note
                    $('#pnlDashboard #pnlEncounterGrid #ctrlPanSigned #ddlNoteType').val(1); // Progress Note
                    $('#pnlDashboard #pnlEncounterGrid #ctrlMissingInfo #ddlNoteType').val(1); // Progress Note

                    $('#pnlDashboard #pnlEncounterGrid #ctrlPanReadyToSign #dpVisitFromR').val('');
                    $('#pnlDashboard #pnlEncounterGrid #ctrlPanReadyToSign #dpVisitToR').val('');
                    $('#pnlDashboard #pnlEncounterGrid #ctrlPanSigned #dpVisitFromS').val('');
                    $('#pnlDashboard #pnlEncounterGrid #ctrlPanSigned #dpVisitToS').val('');
                    $('#pnlDashboard #pnlEncounterGrid #ctrlMissingInfo #dpVisitFromM').val('');
                    $('#pnlDashboard #pnlEncounterGrid #ctrlMissingInfo #dpVisitToM').val('');
                    $('#pnlDashboard #pnlEncounterGrid #ctrlMissingInfo #ddlMissingInfo').val("");
                    if (globalAppdata.DefaultProviderName.toLowerCase().indexOf("select") <= 0) {
                        $("#pnlDashboard #ctrlPanReadyToSign #txtProvider").val(globalAppdata.DefaultProviderName);
                        $("#pnlDashboard #ctrlPanSigned #txtProvider").val(globalAppdata.DefaultProviderName);
                        $("#pnlDashboard #ctrlMissingInfo #txtProvider").val(globalAppdata.DefaultProviderName);
                    }
                    else {
                        $("#pnlDashboard #ctrlPanReadyToSign #txtProvider").val("");
                        $("#pnlDashboard #ctrlPanSigned #txtProvider").val("");
                        $("#pnlDashboard #ctrlMissingInfo #txtProvider").val("");
                    }
                    $("#pnlDashboard #ctrlPanReadyToSign #hfProviderId").val(globalAppdata.DefaultProviderId);
                    $("#pnlDashboard #ctrlPanSigned #hfProviderId").val(globalAppdata.DefaultProviderId);
                    $("#pnlDashboard #ctrlMissingInfo #hfProviderId").val(globalAppdata.DefaultProviderId);


                    var $CtrlR = $("#pnlDashboard #pnlEncounterGrid #ctrlPanReadyToSign #txtProvider");
                    var $hfCtrlR = $("#pnlDashboard #pnlEncounterGrid #ctrlPanReadyToSign #hfProviderId");
                    utility.SetAutoCompleteSource($CtrlR, $hfCtrlR);

                    var $CtrlM = $("#pnlDashboard #pnlEncounterGrid #ctrlMissingInfo #txtProvider");
                    var $hfCtrlM = $("#pnlDashboard #pnlEncounterGrid #ctrlMissingInfo #hfProviderId");
                    utility.SetAutoCompleteSource($CtrlM, $hfCtrlM);

                    var $CtrlS = $("#pnlDashboard #pnlEncounterGrid #ctrlPanSigned #txtProvider");
                    var $hfCtrlS = $("#pnlDashboard #pnlEncounterGrid #ctrlPanSigned #hfProviderId");
                    utility.SetAutoCompleteSource($CtrlS, $hfCtrlS);


                    if (GridData && GridData.Encounter) {
                        DashBoard.SearchDashBoardEncounter(null, null, JSON.parse(GridData.Encounter));
                    }
                    else {
                        if ($("#pnlDashboard #pnlEncounterGrid #listReadyToSign").hasClass("active")) {
                            DashBoard.SearchDashBoardEncounter(null, null, null);
                        }
                        else if ($("#pnlDashboard #pnlEncounterGrid #listSigned").hasClass("active")) {
                            DashBoard.DashBoardEncounterSearchSigned(null, null, null);
                        }
                        else if ($("#pnlDashboard #pnlEncounterGrid #listMissingInfo").hasClass("active")) {
                            DashBoard.DashBoardEncounterSearchMissingInfo(null, null, null);
                        }
                        else {
                            $("#pnlDashboard #pnlEncounterGrid #listReadyToSign").addClass("active")
                            DashBoard.SearchDashBoardEncounter(null, null, null);
                        }
                    }

                    Clinical_Notes.LoadAllAutocomplete();
                }

                else {
                    $('#pnlDashboard #pnlEncounterGridOld #OldddlNoteType').val(1); // Progress Note
                    if (GridData && GridData.Encounter) {
                        if (globalAppdata.DefaultProviderName.toLowerCase().indexOf("select") <= 0)
                            $("#pnlDashboard #OldNoteProviderText").val(globalAppdata.DefaultProviderName);
                        else
                            $("#pnlDashboard #OldNoteProviderText").val('');
                        $("#pnlDashboard #pnlEncounterGridOld #OldhfProviderId").val(globalAppdata.DefaultProviderId);
                        DashBoard.DashBoardEncounterSearchOld(null, null, JSON.parse(GridData.Encounter));
                    }
                    else {
                        if (globalAppdata.DefaultProviderName.toLowerCase().indexOf("select") <= 0)
                            $("#pnlDashboard #OldNoteProviderText").val(globalAppdata.DefaultProviderName);
                        else
                            $("#pnlDashboard #OldNoteProviderText").val('');
                        $("#pnlDashboard #pnlEncounterGridOld #OldhfProviderId").val(globalAppdata.DefaultProviderId);
                        DashBoard.DashBoardEncounterSearchOld(null, null, null);
                    }
                    if (globalAppdata.DefaultProviderName.toLowerCase().indexOf("select") <= 0)
                        $("#pnlDashboard #OldNoteProviderText").val(globalAppdata.DefaultProviderName);
                    else
                        $("#pnlDashboard #OldNoteProviderText").val('');
                    $("#pnlDashboard #pnlEncounterGridOld #OldhfProviderId").val(globalAppdata.DefaultProviderId);
                    CacheManager.BindCodes('GetNoteSections', false);
                }
                break;
            case 'Documents':
                $('#pnlDashboard #pnlPatientDocumentGrid #ddlDocumentReview').val(globalAppdata.AppUserId);
                $('#pnlDashboard #pnlPatientDocumentGrid #ddlDocumentStatus option[value="Pending"]').prop('selected', true);
                $('#pnlDashboard #pnlPatientDocumentGrid #txtFullName').val("");
                $('#pnlDashboard #pnlPatientDocumentGrid #dpDOSDateFrom').val("");
                $('#pnlDashboard #pnlPatientDocumentGrid #dpDOSDateTo').val("");
                $('#pnlDashboard #pnlPatientDocumentGrid #dpDOSDateTo').attr('disabled', true);
                $('#pnlDashboard #pnlPatientDocumentGrid #ddlDocumentPriority option:first-child').prop('selected', true);
                $('#pnlDashboard #pnlPatientDocumentGrid #hfPatientId').val("");
                $('#pnlDashboard #pnlPatientDocumentGrid #ddlDocumentReview option:contains(- Select -)').text("All");
                if (GridData && GridData.Documents)
                { DashBoard.DashBoardDocumentSearch(null, null, JSON.parse(GridData.Documents)); }
                else
                {

                    DashBoard.DashBoardDocumentSearch(null, null, null);
                }
                $("#pnlDashboard #ddlDocumentPriority").html("");

                CacheManager.BindCodes('GetDocumentPriority', false).done(function (result) {
                    var priorities = JSON.parse(result.GetDocumentPriority)
                    $.each(priorities, function (k, obj) {
                        var color = "";
                        if (obj.Name.toLowerCase().trim() == "low")
                            color = "green bold";
                        else if (obj.Name.toLowerCase().trim() == "medium")
                            color = "dark-yellow bold";
                        else if (obj.Name.toLowerCase().trim() == "high")
                            color = "red bold";
                        $("#pnlDashboard #ddlDocumentPriority").append("<option value='" + obj.Value + "' class='" + color + "' " + ">" + obj.Name + "</option>");
                    });
                });
                if (ev == null) {
                    $('#pnlDashboard #widgetpanel .slick-list').find('.li.active').removeClass('active');
                    $("#pnlDashboard #widgetpanel div.wDocuments").addClass("active");
                }
                break;
            case 'Copayment':
                if (GridData && GridData.Copayment)
                    DashBoard.DashBoardCopaySearch(null, null, JSON.parse(GridData.Copayment));
                else
                    DashBoard.DashBoardCopaySearch(null, null, null);
                break;
            case 'PatientChanges':
                if (GridData && GridData["Patient Changes"])
                    DashBoard.DashBoardPatientChangesSearch(null, null, JSON.parse(GridData["Patient Changes"]));
                else
                    DashBoard.DashBoardPatientChangesSearch(null, null, null);
                break;
            case 'Orders&Results':
                IsBackgroundLoaderShow = false;
                DashBoard.LoadDefaultValueForDashboardWidgetLabResultTab();
                if (GridData && GridData["Orders&Results"])
                    DashBoard.DashBoardLabResultLoad(null, null, null);
                else
                    DashBoard.DashBoardLabResultLoad(null, null, null);
                DashBoard.ShowPanel('pnlLabOrder', $('#pnlDashboard #dashboard_LabOrder'));
                if (ev == null) {
                    $('#pnlDashboard #widgetpanel .slick-list').find('.li.active').removeClass('active');
                    $("#pnlDashboard #widgetpanel div.wOrdersResults").addClass("active");
                }
                IsBackgroundLoaderShow = true;
                break;

            case 'Referrals':
                if (GridData && GridData["Referrals"])
                    DashBoard.DashBoardReferralSearch(null, null, JSON.parse(GridData["Referrals"]));
                else
                    DashBoard.DashBoardReferralSearch(null, null, null);
                $('#pnlDashboard .order').css('display', 'none');
                $('#pnlDashboard #pnlReferrals').css('display', '');
                break;

            //case 'PortalRequests':
            //    if (GridData && GridData.PortalRequests)
            //        DashBoard.DashBoardAppRequestSearch(JSON.parse(GridData.PortalRequests));
            //    else
            //        DashBoard.DashBoardAppRequestSearch(null);
            //    break;
            case 'CCM':
                if (GridData && GridData.CCMEnrollmentInfo_JSON)
                    DashBoard.DashBoardCCMEnrollmentInfoSearch(null, null, JSON.parse(GridData.CCMEnrollmentInfo_JSON));
                else
                    DashBoard.DashBoardCCMEnrollmentInfoSearch(null, null, null, "pending");
                break;
            case 'ModifiedNotes':
                if (GridData && GridData.ModifiedNote_JSON)
                    DashBoard.SearchDashBoardModifiedNotesSearch(null, null, JSON.parse(GridData.ModifiedNote_JSON));
                else
                    DashBoard.SearchDashBoardModifiedNotesSearch(null, null, null);
                break;
            case 'ActiveAccounts':
                DashBoard.SearchPatientPortalAccounts(null, null, null);
                break;
            case 'DataChangeRequest':

                DashBoard.SearchDashBoardDataChangeRequest(null, null, null);
            case 'LiveRequests':

                DashBoard.SearchDashBoardDataChangeRequest(null, null, null);
                break;

            case 'PatientPortalRequests':

                DashBoard.SearchDashBoardPatientPortalRequests(null, null, null);
                break;

        }
        //outputs "jQuery Wins!"
        if (ev != null) {
            $('#pnlDashboard .slick-list').find('.li.active').removeClass('active');
            $(ev).addClass('active');

            var slideTitle = $(ev).children('.slide-title').text();
            $('#pnlDashboard .slick-cloned span').each(function (ee) {
                if ($(this).text() == slideTitle) {
                    $(this).parent(".slick-cloned").addClass('active');
                }
            });
        }

        if (GridData) {
            $('#pnlDashboard .slick-list').find('.li.active').removeClass('active');
            $("#pnlDashboard").find('#' + ctrl).addClass('active');
        }

    },
    EnableDisableControl: function () {
        $('#pnlDashboard #pnlSchAppointmentGrid').css("display", "none");
        $('#pnlDashboard #pnlTCMGrid').css("display", "none");
        $('#pnlDashboard #pnlUserMessagesGrid').css("display", "none");
        $('#pnlDashboard #pnlUserTaskGrid').css("display", "none");
        $('#pnlDashboard #pnlEncounterGrid').css("display", "none");
        $('#pnlDashboard #pnlPatientDocumentGrid').css("display", "none");
        $('#pnlDashboard #pnlPaymentsGrid').css("display", "none");
        $('#pnlDashboard #pnlCopayGrid').css("display", "none");
        $('#pnlDashboard #pnlPatientChangesGrid').css("display", "none");
        $('#pnlDashboard #pnlLabOrderGrid').css("display", "none");
        $('#pnlDashboard #pnlReferralsGrid').css("display", "none");
        //$('#pnlDashboard #pnlAppRequestGrid').css("display", "none");
        $('#pnlDashboard #pnlCCMEnrollmentInfoGrid').css("display", "none");
        $('#pnlDashboard #pnlModifiedNoteGrid').css("display", "none");
        $('#pnlDashboard #pnlDataChangeRequestGrid').css("display", "none");
        $('#pnlDashboard #pnlPatientPortalRequests').css("display", "none");
        $('#pnlDashboard #pnlActiveAccountsGrid').css("display", "none");
        $('#pnlDashboard #pnlEncounterGridOld').css("display", "none");
    },
    //#endregion Widget
    ShowPanel: function (panelName, btn) {
        $('#pnlDashboard #mstrLabOrder button').removeClass('active');
        $(btn).addClass('active');
        $('#pnlDashboard .order').css('display', 'none');
        $('#pnlDashboard #' + panelName).css('display', '');
    },

    //#region KPI
    LoadKPISettings: function (response) {
        var KPIdsettings = JSON.parse(response.DASHBOARDSETTING_KPI_JSON);
        KPIdsettings = $.grep(KPIdsettings, function (element, index) {
            if (element.IsDefault == '1' && element.IsActive.toLowerCase() == 'true') {
                return element;
            }

        });
        if (response.status != false) {

            //InactiveList
            //var InactiveList = "";
            //for (var i = 0; i < KPIdsettings.length; i++) {
            //    if (KPIdsettings.length > 0) {
            //        if (KPIdsettings[i].IsActive != "True") {
            //            InactiveList += '<li><a href="#" onclick="DashBoard.SwitchKPI(this,' + KPIdsettings[i].DBSId + ')" >' + KPIdsettings[i].KPIName + '</a></li>';
            //        }
            //    }
            //}
            $('#pnlDashboard #kpipanel').empty();
            if (KPIdsettings.length == 0) {
                KPIdsettings.length = 4;
                for (var k = 0; k < KPIdsettings.length; k++) {
                    $('#pnlDashboard #kpipanel').append('<div class="DashBoardKPI_Drop col-lg-6 col-md-6 col-sm-6" style="min-height: 300px; height:auto;"></div>');
                }
            } else {
                for (var k = 0; k < KPIdsettings.length; k++) {


                    //  if (KPIdsettings[k].IsActive == "True") {
                    $('#pnlDashboard #kdhtml').css('display', 'block');


                    //  KPI Panels
                    $('#pnlDashboard #kpipanel').append(' <div class="DashBoardKPI_Drop col-lg-6 col-md-6 col-sm-6" id="' + KPIdsettings[k].KPIId + '">'
                        + '<div class="panel panel-featured">'
                        + '<header id="KPIHeader_' + KPIdsettings[k].DBSId + '" class="panel-heading">'
                        + '<div class="panel-actions"><a href="#" class="fa fa-times pull-right closeBtn  moveRT" onclick="DashBoard.DeleteWidget(' + KPIdsettings[k].DBSId + ')"></a> </div>'
                        + '<h2 class="panel-title pull-left">' + KPIdsettings[k].KPIName + '</h2>'
                        + '</header>'
                        + '<div id="chart_' + KPIdsettings[k].DBSId + '" style="min-height: 300px; height:auto;" class=" panel-body"></div>'
                        + '</div>'
                        + '</div>');

                    //  Settings Button
                    //if (InactiveList != "") {
                    var btnSetting = '<div class="dropdown btn  pull-right tiny-btn graph-btn settingsKPIBtn" style="width: 30px;margin-right: -7px;">'
                        + '<a id="dLabel" role="button" onclick="DashBoard.DeleteKpi(' + KPIdsettings[k].KPIId + ',' + KPIdsettings[k].DBSId + ')">'
                        + '<i class="fa fa-close  white"></i>'
                        + '</a>'
                        //+ '<ul id="' + KPIdsettings[k].DBSId + '" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">'
                        //    + InactiveList
                        //+ '</ul>'
                        + '</div>';

                    //    $('#pnlDashboard #kpipanel #KPIHeader_' + KPIdsettings[k].DBSId).append(btnSetting);
                    //}

                    $('#pnlDashboard #kpipanel #KPIHeader_' + KPIdsettings[k].DBSId).append('<div class="clearfix"></div>');
                    DashBoard.KPICharts(response, KPIdsettings[k].KPIName, KPIdsettings[k].DBSId);
                    // }


                }
                if (KPIdsettings.length < 4) {

                    for (var k = 0; k < 4 - KPIdsettings.length; k++) {
                        $('#pnlDashboard #kpipanel').append('<div class="DashBoardKPI_Drop col-lg-6 col-md-6 col-sm-6" style="min-height: 300px; height:auto;"></div>');
                    }

                }
            }
            DashBoard.LoadKPIinDashboardMenu(response)
        }

    },

    DeleteKpi: function (KpiControl, DBSID) {
        utility.myConfirm('3', function () {
            $('#' + KpiControl + ".DashBoardKPI_Drop").empty();
            $('#' + KpiControl + ".DashBoardKPI_Drop").attr('id') = "";
        }, function () { },
            '3'
        );
    },

    LoadKPIinDashboardMenu: function (response) {

        var KPIdsettings = JSON.parse(response.DASHBOARDSETTING_KPI_JSON);
        KPIdsettings = $.grep(KPIdsettings, function (element, index) {
            if (element.IsActive.toLowerCase() == 'true') {
                return element;
            }

        });

        if (response.status != false) {

            //InactiveList
            //   var InactiveList = "";
            var KpiDashboardList = '<ul class="nav nav-children">';
            for (var i = 0; i < KPIdsettings.length; i++) {
                if (KPIdsettings.length > 0) {

                    //if (KPIdsettings[i].IsActive != "True") {
                    //    InactiveList += '<li><a href="#" onclick="DashBoard.SwitchKPI(this,' + KPIdsettings[i].DBSId + ')" >' + KPIdsettings[i].KPIName + '</a></li>';
                    //}
                    KpiDashboardList += '<li id=' + KPIdsettings[i].DBSId + ' class="DashBoardKPI_Drag" visible="false"><input type="hidden" id=' + KPIdsettings[i].KPIId + ' /><a href="javascript:void(0);">' + KPIdsettings[i].KPIName + '</a></li>'
                }
            }

            KpiDashboardList += "</ul>";
            $('#mstrMenuDashBoard ul').remove();
            $('#mstrMenuDashBoard').append(KpiDashboardList);
            $(".DashBoardKPI_Drag").draggable({
                revert: true,
                appendTo: 'body',
                helper: 'clone',
                stack: '.DashBoardKPI_Drop',
                start: function () {
                    // $(this).css('z-index', '9999');
                    $(this).css('z-index', '2000');
                    //  $(this).css('position', 'absolute');
                },
                stop: function () {
                    //  $(ui.helper).clone(true);
                    // $(this).css('z-index', '0');
                    $(this).css('z-index', '2000');
                }
            });


            $(".DashBoardKPI_Drop").droppable({
                accept: ".DashBoardKPI_Drag",
                activeClass: 'droppable-active',
                hoverClass: 'droppable-hover',
                drop: function (ev, ui) {
                    var OldKPI = this.id
                    //getting Information Of KPI  Dropped
                    var DBSId = ui.draggable.attr('id');
                    var NewKpi = ui.draggable.find('input[type=hidden]').attr('id');
                    // this.id = NewKpi;
                    var RefID = this;
                    DashBoard.DropKPI(RefID, DBSId, OldKPI, NewKpi)

                }
            });
        }
    },

    DropKPI: function (DropedKPIDiv, DBSId, OldKPI, NewKpi) {

        //check is already this KPI is there on Dashboard.
        if ($("#kpipanel").find("#" + NewKpi).length <= 0) {

            DashBoard.Update_Dashboard_KPI_ONDashboard_KPIDrop(OldKPI, NewKpi).done(function (response) {
                if (response.status) {
                    DropedKPIDiv.id = NewKpi;

                    // Appointment
                    var myJSON = new Object();
                    myJSON.SlotDate = $.datepicker.formatDate('mm/dd/yy', new Date());
                    //Message
                    //myJSON.dtpEntryDate = $.datepicker.formatDate('mm/dd/yy', new Date());
                    myJSON.dtpEntryDate = null;
                    myJSON.Status = 2;
                    myJSON.ddlType = "";
                    myJSON.dtpCalledDate = "";
                    myJSON.PageNumber = 1;
                    myJSON.RowsPerPage = 5000;
                    //Encounter
                    myJSON.ProviderId = null;
                    myJSON.FacilityId = null;
                    myJSON.dtpAppointmentDateFrom = $.datepicker.formatDate('mm/dd/yy', new Date());
                    myJSON.dtpAppointmentDateTo = $.datepicker.formatDate('mm/dd/yy', new Date());
                    myJSON.txtClaimNumber = "";
                    //document
                    myJSON.FromEntry = $.datepicker.formatDate('mm/dd/yy', new Date());
                    myJSON.ToEntry = $.datepicker.formatDate('mm/dd/yy', new Date());
                    myJSON.EnteredBy = null;
                    myJSON.AssignedtoReview = null;
                    myJSON.DocumentId = null;
                    myJSON.Active = null;
                    var myString = JSON.stringify(myJSON);
                    DashBoard.LoadDashBoard(myString, DBSId, null).done(function (response) {
                        var KPIdsettings = JSON.parse(response.DASHBOARDSETTING_KPI_JSON);

                        if (response.status != false) {
                            //InactiveList
                            //var InactiveList = "";
                            //for (var i = 0; i < KPIdsettings.length; i++) {
                            //    if (KPIdsettings.length > 0) {
                            //        if (KPIdsettings[i].IsActive != "True") {
                            //            InactiveList += '<li><a href="#" onclick="DashBoard.SwitchKPI(this,' + KPIdsettings[i].DBSId + ')" >' + KPIdsettings[i].KPIName + '</a></li>';
                            //        }
                            //    }
                            //}
                            $(DropedKPIDiv).empty();
                            for (var k = 0; k < KPIdsettings.length; k++) {
                                if (KPIdsettings.length > 0) {

                                    // if (KPIdsettings[k].IsActive == "True") {
                                    $('#pnlDashboard #kdhtml').css('display', 'block');


                                    // KPI Panels
                                    $(DropedKPIDiv).append(' <div class="col-sm-12 pull-left">'
                                        + '<div class="panel panel-featured">'
                                        + '<header id="KPIHeader_' + DBSId + '" class="panel-heading">'
                                        + '<div class="panel-actions"><a href="#" class="fa fa-times pull-right closeBtn  moveRT" onclick="DashBoard.DeleteWidget(' + DBSId + ')"></a> </div>'
                                        + '<h2 class="panel-title pull-left">' + KPIdsettings[k].KPIName + '</h2>'
                                        + '</header>'
                                        + '<div id="chart_' + DBSId + '" style="min-height: 300px; height:auto;" class=" panel-body"></div>'
                                        + '</div>'
                                        + '</div>');

                                    //Settings Button
                                    // if (InactiveList != "") {
                                    var btnSetting = '<div class="dropdown btn  pull-right tiny-btn graph-btn settingsKPIBtn" style="width: 30px;margin-right: -7px;">'
                                        + '<a id="dLabel" role="button" onclick="DashBoard.DeleteKpi(' + KPIdsettings[k].KPIId + ',' + KPIdsettings[k].DBSId + ')">'
                                        + '<i class="fa fa-close  white"></i>'
                                        + '</a>'
                                        //+ '<ul id="' + KPIdsettings[k].DBSId + '" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">'
                                        //    + InactiveList
                                        //+ '</ul>'
                                        + '</div>';

                                    // $('#pnlDashboard #kpipanel #KPIHeader_' + KPIdsettings[k].DBSId).append(btnSetting);
                                    // }

                                    $('#pnlDashboard #kpipanel #KPIHeader_' + DBSId).append('<div class="clearfix"></div>');
                                    DashBoard.KPICharts(response, KPIdsettings[k].KPIName, DBSId);
                                    //}


                                }
                            }
                        }

                    });
                } else {
                    utility.DisplayMessages(response.Message, 2)
                }
            });
        }



    },

    KPICharts: function (response, kpiname, k) {
        var KPIlist = JSON.parse(response.DASHBOARDSETTING_KPI_CHARTS_JSON);
        var data = null;
        if (KPIlist[kpiname] != null && KPIlist[kpiname] != "") {
            data = JSON.parse(KPIlist[kpiname]);


            var command = kpiname;
            switch (command) {


                case 'Weekly Patient’s Visits':
                    {
                        var myJSON = [];
                        $.each(data, function (i, item) {

                            var jsonArray = { month: item.CreatedOn, a: item.NewPatients, b: item.EstablishedPatients };
                            var temp = jsonArray;
                            myJSON.push(temp);

                        });

                        var weeklyPatientVisitsChart = Morris.Bar({
                            xLabelMargin: 10,
                            xLabelAngle: 45,
                            resize: true,
                            element: 'chart_' + k,
                            data: myJSON,
                            hoverCallback: function (index, options, content) {
                                return (content);
                            },
                            xkey: 'month',
                            ykeys: ['a', 'b'],
                            stacked: true,
                            labels: ['New Patient(s)', 'Established Patient(s)'],
                            barColors: function (row, series, type) {

                                if (series.label == "New Patient(s)") return "#AD1D28";
                                if (series.label == "Established Patient(s)") return "#DEBB27";

                            }
                        });
                        $('#chart_' + k).resize(function () {
                            if ($("#ctrlPanDashBoard").css("display") != "none") {
                                weeklyPatientVisitsChart.redraw();
                            }
                        });
                    }
                    break;
                case 'Collected Revenue':
                    {
                        // By Default, Current Month Selection
                        var Currentdate = new Date();
                        var CurrentMonthIndex = -1;
                        var month = new Array();
                        month[0] = "January";
                        month[1] = "February";
                        month[2] = "March";
                        month[3] = "April";
                        month[4] = "May";
                        month[5] = "June";
                        month[6] = "July";
                        month[7] = "August";
                        month[8] = "September";
                        month[9] = "October";
                        month[10] = "November";
                        month[11] = "December";
                        var CurrentMonth = month[Currentdate.getMonth()];


                        var myJSON = [];
                        $.each(data, function (i, item) {

                            var jsonArray = { value: item.Revenue, label: item.CreatedOn };
                            var temp = jsonArray;
                            myJSON.push(temp);

                            if (CurrentMonth == item.CreatedOn.split('-')[0])
                                CurrentMonthIndex = parseInt(i);

                        });

                        var collectedRevenueChart = Morris.Donut({
                            element: 'chart_' + k,
                            parseTime: false,
                            resize: true,
                            data: myJSON,
                            backgroundColor: '#ccc',
                            labelColor: '#060',
                            colors: [
                                '#0BA462',
                                '#39B580',
                                '#67C69D',
                                '#95D7BB'
                            ],
                            formatter: function (x) { return "$" + x }
                        });
                        $('#chart_' + k).resize(function () {
                            if ($("#ctrlPanDashBoard").css("display") != "none") {
                                collectedRevenueChart.redraw();
                            }
                        });

                        collectedRevenueChart.select(CurrentMonthIndex);
                    }
                    break;
                case 'Collected Copayment':
                    {
                        var myJSON = [];
                        $.each(data, function (i, item) {

                            var jsonArray = { year: item.CreatedOn, value: item.Copay };
                            var temp = jsonArray;
                            myJSON.push(temp);

                        });

                        var collectedCopaymentChart = Morris.Area({
                            element: 'chart_' + k,
                            xLabelMargin: 10,
                            xLabelAngle: 45,
                            resize: true,
                            parseTime: false,
                            data: myJSON,
                            xkey: 'year',
                            ykeys: ['value'],
                            labels: ['Co-Pay'],
                            preUnits: ['$'],
                            lineColors: ['red'],
                            pointFillColors: ['#ffffff'],
                            pointStrokeColors: ['black']
                        });
                        $('#chart_' + k).resize(function () {
                            if ($("#ctrlPanDashBoard").css("display") != "none") {
                                collectedCopaymentChart.redraw();
                            }
                        });
                    }

                    break;
                case 'Charges Vs Payments':
                    {
                        var myJSON = [];
                        $.each(data, function (i, item) {

                            var jsonArray = { year: item.CreatedOn, value: item.Charges, value1: item.Payments };
                            var temp = jsonArray;
                            myJSON.push(temp);

                        });

                        var chargesPaymentsChart = Morris.Line({
                            element: 'chart_' + k,
                            xLabelMargin: 10,
                            xLabelAngle: 45,
                            parseTime: false,
                            resize: true,
                            data: myJSON,
                            xkey: 'year',
                            ykeys: ['value', 'value1'],
                            labels: ['Charges', 'Payments'],
                            preUnits: ['$'],
                            lineColors: ['red', 'orange'],
                            pointFillColors: ['#00ff00', '#00ff00']

                        });
                        $('#chart_' + k).resize(function () {
                            if ($("#ctrlPanDashBoard").css("display") != "none") {
                                chargesPaymentsChart.redraw();
                            }
                        });
                    }

                    break;
                case 'Accounts Receivables':
                    {
                        var myJSON = [];
                        $.each(data, function (i, item) {

                            var jsonArray = { Age: item.MonthAge + "+", a: item.InsBal, b: item.PatBal };
                            var temp = jsonArray;
                            myJSON.push(temp);

                        });

                        var accountReceivableChart = Morris.Bar({
                            xLabelMargin: 10,
                            xLabelAngle: 45,
                            resize: true,
                            element: 'chart_' + k,
                            data: myJSON,
                            hoverCallback: function (index, options, content) {
                                return (content);
                            },
                            xkey: 'Age',
                            ykeys: ['a', 'b'],
                            labels: ['Insurance Balance', 'Patient Balance'],
                            preUnits: ['$'],
                            barColors: function (row, series, type) {

                                if (series.label == "Insurance Balance") return "#1569C7";
                                if (series.label == "Patient Balance") return "#5E7D7E";

                            },
                        });
                        $('#chart_' + k).resize(function () {
                            if ($("#ctrlPanDashBoard").css("display") != "none") {
                                accountReceivableChart.redraw();
                            }
                        });
                    }
                    break;

                case 'MIPS':
                    {
                        var myJSON = [];
                        $.each(data, function (i, item) {

                            var jsonArray = { Age: item.MonthAge + "+", a: item.InsBal, b: item.PatBal };
                            var temp = jsonArray;
                            myJSON.push(temp);

                        });

                        var accountReceivableChart = Morris.Bar({
                            xLabelMargin: 10,
                            xLabelAngle: 45,
                            resize: true,
                            element: 'chart_' + k,
                            data: myJSON,
                            hoverCallback: function (index, options, content) {
                                return (content);
                            },
                            xkey: 'Age',
                            ykeys: ['a', 'b'],
                            labels: ['Insurance Balance', 'Patient Balance'],
                            preUnits: ['$'],
                            barColors: function (row, series, type) {

                                if (series.label == "Insurance Balance") return "#1569C7";
                                if (series.label == "Patient Balance") return "#5E7D7E";

                            },
                        });
                        $('#chart_' + k).resize(function () {
                            if ($("#ctrlPanDashBoard").css("display") != "none") {
                                accountReceivableChart.redraw();
                            }
                        });
                    }
                    break;
            }
        }

    },

    SwitchKPI: function (obj, Id) {

        var CurrentKPIId = $(obj).parent().parent().attr("Id");
        var SelectedKPIId = Id;

        DashBoard.Update_KPIIsActive(CurrentKPIId, SelectedKPIId).done(function (response) {

            if (response.status != false) {

                DashBoard.LoadDashBoard(null, 0, null).done(function (response) {

                    if (response.status != false) {
                        DashBoard.LoadKPISettings(response);
                    }
                    else {

                        utility.DisplayMessages(response.Message, 3);
                    }
                });

            }
            else {

                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    Update_KPIIsActive: function (CurrentKPIId, SelectedKPIId) {


        var objData = new JSON.constructor();
        objData["CurrentKPIId"] = CurrentKPIId;
        objData["SelectedKPIId"] = SelectedKPIId;
        objData["CommandType"] = "UPDATE_DASHBOARD_KPI";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "UpdateDashBoard");
    },

    Update_Dashboard_KPI_ONDashboard_KPIDrop: function (CurrentKPIId, SelectedKPIId) {

        var objData = new JSON.constructor();
        objData["CurrentKPIId"] = CurrentKPIId;
        objData["SelectedKPIId"] = SelectedKPIId;
        objData["CommandType"] = "UPDATE_DASHBOARD_KPI_ON_DROP";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "UpdateDashBoard");
    },
    //#endregion KPI

    //#region Appointments
    DashBoardAppointmentSearch: function (PageNo, rpp, GridData, AppointmentDateForCreateNotes, isFromSetting) {
        DashBoard.EnableDisableControl();
        if ($("#pnlDashboard #pnlSchAppointmentGrid").css("display") == "none")
            $("#pnlDashboard #pnlSchAppointmentGrid").show();

        if (GridData) {
            DashBoard.DashBoardAppointmentGridLoad(GridData, PageNo, rpp, null, isFromSetting);
        }
        else {

            DashBoard.DashBoardAppointmentCall(PageNo, rpp).done(function (response) {
                if (response.status != false) {

                    DashBoard.DashBoardAppointmentGridLoad(response, PageNo, rpp, AppointmentDateForCreateNotes, isFromSetting);


                }
                else {
                    utility.DisplayMessages(response.Message, 3);

                }
            });
        }
    },

    LoadClinicalNote: function (PatientId, AppointmentId, ProviderId, ProviderName, AppointmentTime, VisitId, VisitDate, Reason, FacilityName, FacilityId, Room, NotesId) {
        //if (Reason && VisitDate && AppointmentTime) {
        //    var paramsTemp = {};
        //    paramsTemp["mode"] = "Add";
        //    paramsTemp["FacilityId"] = FacilityId;
        //    paramsTemp["PatientId"] = PatientId;
        //    paramsTemp["AppointmentId"] = AppointmentId;
        //    paramsTemp["VisitTime"] = AppointmentTime;
        //    paramsTemp["VisitId"] = VisitId;
        //    paramsTemp["ParentCntrlLoadid"] = "Dashboard";
        //    paramsTemp["ProviderId"] = ProviderId;
        //    paramsTemp["NotesProviderName"] = ProviderName;
        //    paramsTemp["VisitDate"] = VisitDate;
        //    paramsTemp['VisitReason'] = Reason;
        //    paramsTemp['NotesRoom'] = Room;
        //    paramsTemp['Facility'] = FacilityName;
        //    paramsTemp["RefProviderName"] = null;
        //    paramsTemp["RefProviderId"] = null;
        //    paramsTemp['ForProgressNote'] = false;
        //    paramsTemp["NoteType"] = 1;
        //    paramsTemp["LinkedAppointment"] = VisitDate + ' ' + AppointmentTime;
        //    paramsTemp["IsLinkedAppointment"] = true;
        //    jsonString = JSON.stringify(paramsTemp);
        //    DashBoard.EditsNotes(jsonString, PatientId);
        //} else {
        //    DashBoard.CreateNote(PatientId, AppointmentId, ProviderId, ProviderName, AppointmentTime, VisitId, VisitDate, Reason, FacilityName, FacilityId, Room, NotesId)
        //}
    },

    DashBoardAppointmentGridLoad: function (response, PageNo, rpp, AppointmentDateForCreateNotes, isFromSetting) {

        if (AppointmentDateForCreateNotes == null) {
            AppointmentDateForCreateNotes = utility.formatDate(Date($('#pnlDashboard #appDate').text()));
        }
        var isactive = $('#pnlDashboard #pnlSchAppointmentGrid #gridControl #divSwitch #switchVisit').attr('visitstatus');
        //var isactive = globalAppdata["PreferredAppointmentStatus"] == "" ? "0" : globalAppdata["PreferredAppointmentStatus"];
        if (isactive == undefined || isactive == "undefined") {
            isactive = globalAppdata["PreferredAppointmentStatus"] == "" ? "0" : globalAppdata["PreferredAppointmentStatus"];
        }
        var checked = '';


        //if (isFromSetting == true) {
        //    if (globalAppdata.PreferredAppointmentStatus == "1") {
        //        isactive = "1";
        //        checked = 'checked="checked"';

        //    }
        //}
        //else {
        if (isactive == "0") {
        } else if (isactive == null) {
            isactive = "1";
            checked = 'checked="checked"';
        } else {
            isactive = "1";
            checked = 'checked="checked"';
        }
        //}

        var HtmlOfSwitch = '<span class="pr-xs">All</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                                          '<input id="switchVisit" type="checkbox" visitstatus="' + isactive + '" ' + checked + ' name="Switch" data-plugin-ios-switch="" style="display: inline;" onchange="DashBoard.IsCheckedIn(this);">' +
                                           '</div><span class="pl-xs">Checked In</span>';
        $('#pnlDashboard #pnlSchAppointmentGrid #gridControl #divSwitch').html(HtmlOfSwitch);
        var gridControl = $('#pnlDashboard #pnlSchAppointmentGrid #gridControl').html();
        var gridControlIsChecked = $('#pnlDashboard #pnlSchAppointmentGrid #gridControl input[type=checkbox]').is(':checked')
        var appDate = $('#pnlDashboard #appDate').text();
        $('#pnlDashboard #pnlSchAppointmentGrid #gridControl').remove();
        if (response.iTotalDisplayRecords == undefined || response.iTotalDisplayRecords == null || response.iTotalDisplayRecords == 0) {
            $('#pnlDashboard div.wAppointments .badge').css("display", "none");
            // if appointment Status will be check in then quick link count will be updated PRD-127
            if ($("#pnlSchAppointmentGrid #ddlAppointmentStatus  option:selected").text() == "Check In") {
                $('.notifications #Appointments .badge').css("display", "none");
            }

        } else {
            $('#pnlDashboard div.wAppointments .badge').css("display", "inline");
            $('#pnlDashboard div.wAppointments .badge').text(response.iTotalDisplayRecords);
            // if appointment Status will be check in then quick link count will be updated PRD-127
            if ($("#pnlSchAppointmentGrid #ddlAppointmentStatus  option:selected").text() == "Check In") {
                $('.notifications #Appointments .badge').text(response.iTotalDisplayRecords);
            }

        }



        if (response.SchAppStatusCount > 0) {
            $("#pnlDashboard #divAppointmentPaging").css("display", "inline");
            //Showing 1 to 15 of 15 Record(s)
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            params: [];
            params["CurrentPage"] = CurrentPage;
            params["RecordsPerPage"] = RecordsPerPage;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("divAppointmentPaging", response.iTotalDisplayRecords, 5, "DashBoard", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#pnlDashboard #divAppointmentPaging #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $("#pnlDashboard li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
        }
        else {
            $("#pnlDashboard #divAppointmentPaging").css("display", "none");
        }


        $("#pnlDashboard #pnlSchAppointmentGrid #dgvSchAppointmentGrid").dataTable().fnDestroy();
        $("#pnlDashboard #pnlSchAppointmentGrid #dgvSchAppointmentGrid tbody").find("tr").remove();
        if (response.SchAppStatusCount > 0) {
            var AppointmentSearchJSONData = JSON.parse(response.SchAppStatus_JSON);
            $.each(AppointmentSearchJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", "gvDashBoard_Appointments_row" + i + "");
                $row.attr("onclick", "utility.SelectGridRow($('#gvDashBoard_Appointments_row" + i + "'))");


                //var VisitStatus = "";
                //var AppointmentStatus = "";
                //if (item.VisitId != "")
                //    VisitStatus = item.Status;
                //else
                //    AppointmentStatus = item.Status;

                //var CheckInMethod = "DashBoard.LoadCheckIn('" + item.Date + "','" + item.AppointmentId + "','" + item.PatientId + "','" + item.VisitId + "','" + AppointmentStatus + "');";
                //var EditAppointmentMethod = "DashBoard.AppointmentEdit('" + item.AppointmentId + "','" + item.VisitId + "','" + item.ResourceId + "','" + item.FacilityId + "','" + item.ProviderId + "','" + AppointmentStatus + "','" + VisitStatus + "');";

                var DemographicsMethod = "utility.PatientDemographics('" + item.PatientId + "', 'mstrTabDashBoard', event);";


                //--- Start Action Column Checks on Visit Status lo

                var Action = "";
                var ActionForNotes = "";

                // Checkin > CreateNote
                // Roomed > EditNote
                // Signed > Create e Superbill
                //

                if (item.AppointmentStatus.replace(" ", "").toUpperCase() == 'CHECKIN' && (item.NotesId == null || item.NotesId == '' || item.NotesId == '0' || item.NotesId == 0) && item.NoteStatus != 'Signed') {
                    var Method = "DashBoard.CreateNote('" + item.PatientId + "',\'" + item.AppointmentId + "\',\'" + item.ProviderId + "\',\'" + item.ProviderName + "\',\'" + item.AppointmentTime + "\',\'" + item.VisitId + "\',\'" + utility.RemoveTimeFromDate(null, AppointmentDateForCreateNotes) + "\',\'" + item.Reason + "\',\'" + item.FacilityName + "\',\'" + item.FacilityId + "\',\'" + item.Room + "\',\'" + item.NotesId + "\');";
                    ActionForNotes = '<a href="javascript:void(0);" onclick="' + Method + '"  title="Create Note">Create Note</a>';
                }
                else if ((item.NotesId != null && item.NotesId != '' && item.NotesId != '0' && item.NotesId != 0) && item.NoteStatus != 'Signed') {
                    var Method = "DashBoard.EditProgressNote('" + item.NotesId + "',\'" + item.PatientId + "\');";
                    ActionForNotes = '<a href="javascript:void(0);" onclick="' + Method + '"  title="Edit Note">Edit Note</a>';
                }
                else if ((item.NotesId != null && item.NotesId != '' && item.NotesId != '0' && item.NotesId != 0) && item.NoteStatus == 'Signed') {
                    var Method = "DashBoard.NotesPreview('" + item.NotesId + "',\'" + item.PatientId + "\',\'" + item.ProviderId + "\');";
                    ActionForNotes = '<a href="javascript:void(0);" onclick="' + Method + '"  title="Preview Note">Preview Note</a>';
                }
                var Room = "";

                //if (item.AppointmentStatus.toUpperCase() == "CHECKIN" && (item.NotesId == null || item.NotesId == '' || item.NotesId == '0' || item.NotesId == 0)) {
                //    //<a href="#" onclick="' + DemographicsMethod + '"  title="View Patient"> </a>
                //    var Method = "DashBoard.CreateNote('" + item.PatientId + "',\'" + item.AppointmentId + "\',\'" + item.ProviderId + "\',\'" + item.ProviderName + "\',\'" + item.AppointmentTime + "\',\'" + item.VisitId + "\',\'" + utility.RemoveTimeFromDate(null, AppointmentDateForCreateNotes) + "\',\'" + item.Reason + "\',\'" + item.FacilityName + "\',\'" + item.FacilityId + "\',\'" + item.Room + "\',\'" + item.NotesId + "\');";
                //    Action = '<a href="javascript:void(0);" onclick="' + Method + '"  title="Create Note">Create Note</a>';
                //    Room = "Lobby";

                //}
                //else if ((item.NotesId != null || item.NotesId != '' || item.NotesId > 0) && item.NotesId != '0' && item.NoteStatus != 'Signed' && item.AppointmentStatus.toUpperCase() != "CHECKOUT" && (item.BillingInfoId == null || item.BillingInfoId == '0')) {
                //    //<a href="#" onclick="' + DemographicsMethod + '"  title="View Patient"> </a>
                //    var Method = "DashBoard.EditNote('" + item.PatientId + "',\'" + item.AppointmentId + "\',\'" + item.ProviderId + "\',\'" + item.ProviderName + "\',\'" + item.AppointmentTime + "\',\'" + item.VisitId + "\',\'" + utility.RemoveTimeFromDate(null, item.VisitDate) + "\',\'" + item.Reason + "\',\'" + item.FacilityName + "\',\'" + item.FacilityId + "\',\'" + item.Room + "\',\'" + item.NotesId + "\');";
                //    Action = '<a href="javascript:void(0);" onclick="' + Method + '"  title="Edit Note">Edit Note</a>';
                //    //Room = "Lobby";

                //} else if ((item.NoteStatus == 'Signed' || parseInt(item.BillingInfoId) > 0) && item.BillingStatus != 'Signed') {
                //    var Method = "return false;";//DashBoard.EditNote('" + item.PatientId + "',\'" + item.AppointmentId + "\',\'" + item.ProviderId + "\',\'" + item.ProviderName + "\',\'" + item.AppointmentTime + "\',\'" + item.VisitId + "\',\'" + utility.RemoveTimeFromDate(null, item.VisitDate) + "\',\'" + item.Reason + "\',\'" + item.FacilityName + "\',\'" + item.FacilityId + "\',\'" + item.Room + "\',\'" + item.NotesId + "\');";
                //    var Method = "DashBoard.CreateNoteSuperbill('" + item.PatientId + "',\'" + item.AppointmentId + "\',\'" + item.ProviderId + "\',\'" + item.ProviderName + "\',\'" + item.AppointmentTime + "\',\'" + item.VisitId + "\',\'" + utility.RemoveTimeFromDate(null, item.VisitDate) + "\',\'" + item.Reason + "\',\'" + item.FacilityName + "\',\'" + item.FacilityId + "\',\'" + item.Room + "\',\'" + item.NotesId + "\',\'" + item.PatientType + "\',\'" + item.NoteDate + "\',\'" + item.BillingInfoId + "\');";
                //    var superbillTitle = "Create eSuperbill";
                //    if (item.BillingInfoId != 0) {
                //        superbillTitle = "Edit eSuperbill";
                //    }
                //    Action = '<a href="javascript:void(0);" onclick="' + Method + '"  title="' + superbillTitle + '">' + superbillTitle + '</a>';

                //}
                //else if (item.BillingStatus == 'Signed') {

                //    var Method = "return false;";//DashBoard.EditNote('" + item.PatientId + "',\'" + item.AppointmentId + "\',\'" + item.ProviderId + "\',\'" + item.ProviderName + "\',\'" + item.AppointmentTime + "\',\'" + item.VisitId + "\',\'" + utility.RemoveTimeFromDate(null, item.VisitDate) + "\',\'" + item.Reason + "\',\'" + item.FacilityName + "\',\'" + item.FacilityId + "\',\'" + item.Room + "\',\'" + item.NotesId + "\');";
                //    var Method = "DashBoard.LoadVisitDetail(\'" + item.VisitId + "\','" + item.PatientId + "');";
                //    var superbillTitle = "View Charges";

                //    Action = '<a href="javascript:void(0);" onclick="' + Method + '"  title="' + superbillTitle + '">' + superbillTitle + '</a>';
                //}
                if (Room == "") {
                    Room = item.Room;
                }
                var Wait = "";

                //if (item.minsWait != "")
                //    Wait = item.minsWait + '(min)';
                //else if (VisitStatus == "Roomed") {
                //    var Method = "DashBoard.EditNote('" + item.PatientId + "');";
                //    Action = '"<a href="#" onclick="' + Method + '"  title="Edit Note">Edit Note</a>"';
                //}
                //else if (VisitStatus == "Signed") {
                //    var Method = "DashBoard.CreateSuperBill('" + item.PatientId + "');";
                //    Action = '"<a href="#" onclick="' + Method + '"  title="Create eSuperBill">Create eSuperBill</a>"';
                //}
                //else if (VisitStatus == "E Super Bill Generated") {
                //    Action = "";
                //}

                //--- End Action Column Checks on Visit Status base

                // Previous
                //$row.append('<td><a class="btn btn-xs" href="#" onclick="' + EditAppointmentMethod + '"  title="Edit Appointment"><i class="fa fa-edit black"></i></a><a class="btn  btn-xs" href="#" onclick="' + CheckInMethod + '" title="CheckIn Appointment"><i class="fa fa-check"></i></a>&nbsp;</td><td>' + item.Facility + '</td><td>' + item.Provider + '' + item.Resource + '</td><td>' + item.AccountNumber + '</td><td>' + item.Name + '</td><td>' + String(item.Time).substr(0, String(item.Time).indexOf('-')) + '</td><td class="size-max120 ellipses" data-toggle="tooltip" data-placement="left" title="' + item.Minutes + '">' + item.Minutes + '</td><td>' + item.Reason + '</td><td>' + item.Status + '</td><td class="size-max120 ellipses" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '">' + item.Comments + '</td>');

                //$row.append('<td>' + item.Facility + '</td><td>' + item.Provider + '' + item.Resource + '</td><td>' + item.AccountNumber + '</td><td>' + item.Name + '</td><td>' + String(item.Time).substr(0, String(item.Time).indexOf('-')) + '</td><td class="size-max120 ellipses" data-toggle="tooltip" data-placement="left" title="' + item.Minutes + '">' + item.Minutes + '</td><td>' + item.Reason + '</td><td>' + item.Status + '</td><td class="size-max120 ellipses" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '">' + item.Comments + '</td>');

                $row.append('<td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.AccountNumber + '</a></td><td  class="ellip100" title="' + item.PatientName + '" data-toggle="tooltip" data-placement="right"><a href="#" onclick="' + DemographicsMethod + '" >' + item.PatientName + '</a></td><td>' + item.ProviderName + '</td><td>' + item.FacilityName + '</td><td>' + item.PatientType.replace(" Patient", "") + '</td><td>' + item.AppointmentTime + '</td><td class="size-max120 ellipses" data-toggle="tooltip" data-placement="left" title="' + item.Duration + '(min)">' + item.Duration + '(min)</td><td>' + item.VisitTime + '</td><td>' + item.AppointmentStatus + '</td><td>' + ActionForNotes + '</td>');

                $("#pnlDashboard #dgvSchAppointmentGrid tbody").last().append($row);
            });
        }
        else {
            $("#pnlDashboard #divAppointmentPaging").css("display", "none");
            $('#pnlDashboard #dgvSchAppointmentGrid').DataTable({
                "language": {
                    "emptyTable": "No Appointment Found"
                }, "searching": false, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
                //}, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false }]
            });
        }
        DashBoard.sortGridByDurationAndTime();
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });

        if ($.fn.dataTable.isDataTable('#pnlDashboard #dgvSchAppointmentGrid'))
            ;
        else
            //$("#pnlDashboard #dgvSchAppointmentGrid").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false }] }); // to remove records per page dropdown
            $("#pnlDashboard #dgvSchAppointmentGrid").DataTable({
                "searching": false, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": []
                //, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }, { "sType": "num-html", "aTargets": [8] }, { 'sType': 'time-uni', targets: [7] }]
            }); // to remove records per page dropdown
        // $('.table-responsive').css('min-height', '220px');
        $('#pnlDashboard #dgvSchAppointmentGrid_wrapper .datatables-header div:first').html(' <div id="gridControl">' + gridControl + '</div>');
        $('#pnlDashboard #pnlSchAppointmentGrid #gridControl input[type=checkbox]').prop('checked', gridControlIsChecked);
        //$('#pnlDashboard #appDate').text(appDate);
        DashBoard.documentReady(false);
        DashBoard.SwicthWidgetInializatoin();
        //$('#pnlDashboard #appDate').datepicker('setDate', new Date($('#pnlDashboard #appDate').text()))
        $('#pnlDashboard #pnlSchAppointmentGrid #ddlAppointmentStatus').multiselect('rebuild');
    },

    sortGridByDurationAndTime: function () {

        jQuery.extend(jQuery.fn.dataTableExt.oSort, {
            "num-html-pre": function (a) {
                var x = String(a).replace(/(?!^-)[^0-9.]/g, "");
                return parseFloat(x);
            },

            "num-html-asc": function (a, b) {
                return ((a < b) ? -1 : ((a > b) ? 1 : 0));
            },

            "num-html-desc": function (a, b) {
                return ((a < b) ? 1 : ((a > b) ? -1 : 0));
            }
        });
        jQuery.extend(jQuery.fn.dataTableExt.oSort, {
            "time-uni-pre": function (a) {
                var uniTime;

                if (a.toLowerCase().indexOf("am") > -1 || (a.toLowerCase().indexOf("pm") > -1 && Number(a.split(":")[0]) === 12)) {
                    uniTime = a.toLowerCase().split("pm")[0].split("am")[0];
                    while (uniTime.indexOf(":") > -1) {
                        uniTime = uniTime.replace(":", "");
                    }
                } else if (a.toLowerCase().indexOf("pm") > -1 || (a.toLowerCase().indexOf("am") > -1 && Number(a.split(":")[0]) === 12)) {
                    uniTime = Number(a.split(":")[0]) + 12;
                    var leftTime = a.toLowerCase().split("pm")[0].split("am")[0].split(":");
                    for (var i = 1; i < leftTime.length; i++) {
                        uniTime = uniTime + leftTime[i].trim().toString();
                    }
                } else {
                    uniTime = a.replace(":", "");
                    while (uniTime.indexOf(":") > -1) {
                        uniTime = uniTime.replace(":", "");
                    }
                }
                return Number(uniTime);
            },

            "time-uni-asc": function (a, b) {
                return ((a < b) ? -1 : ((a > b) ? 1 : 0));
            },

            "time-uni-desc": function (a, b) {
                return ((a < b) ? 1 : ((a > b) ? -1 : 0));
            }
        });

    },

    LoadVisitDetail: function (VisitId, PatientId) {

        AppPrivileges.GetFormPrivileges("Charges", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FromAdmin"] = 0;
                params["ParentCtrl"] = 'mstrTabDashBoard';

                params["VisitId"] = VisitId;
                params["patientID"] = PatientId;

                LoadActionPan('EncounterChargeCapture', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    LoadCheckIn: function (date, appid, patientid, visitid, appoinmentstatus) {

        if (visitid == "") {

            if (appoinmentstatus.toUpperCase() == 'CONFIRM' && appoinmentstatus.toUpperCase() != 'CANCEL') {

                var formatted_date = function (date_) {
                    var date = new Date(date_);
                    var m = ("0" + (date.getMonth() + 1)).slice(-2);
                    var d = ("0" + date.getDate()).slice(-2);
                    var y = date.getFullYear();
                    return m + '/' + d + '/' + y;
                }

                var DayDate = ($.datepicker.formatDate('mm/dd/yy', new Date(formatted_date(date))));
                var params = [];
                params["FromAdmin"] = "0";
                params["ProviderId"] = $('#pnlScheduleCalendar #Provider').val();
                params["ResourceId"] = $('#pnlScheduleCalendar #Resource').val();
                params["FacilityId"] = $('#pnlScheduleCalendar #Facility').val();
                params["DayDate"] = DayDate;
                params["AppointmentId"] = appid;
                params["PatientId"] = patientid;
                params["ParentCtrl"] = 'mstrTabDashBoard';
                LoadActionPan('schcheckin', params);

            }
            else {
                utility.DisplayMessages("Appointment is not confirmed or canceled.", 3);
            }
        }
        else {
            utility.DisplayMessages("Appointment is already checked in.", 3);
        }

    },

    DashBoardAppointmentCall: function (PageNumber, RowsPerPage) {

        var calDate = $('#pnlDashboard #appDate').text();
        // Start 02/12/2015 Muhammad Irfan Bug # 64
        //var cur1 = $.datepicker.formatDate('yy/mm/dd', new Date(calDate));
        var newDatee = DashBoard.FormatDate(calDate);
        var AppDate = newDatee;
        var IsCheckedIn = null;


        IsCheckedIn = $('#pnlDashboard #pnlSchAppointmentGrid #divSwitch #switchVisit').attr('VisitStatus');

        if (IsCheckedIn == undefined || IsCheckedIn == "undefined") {
            IsCheckedIn = globalAppdata["PreferredAppointmentStatus"] == "" ? "0" : globalAppdata["PreferredAppointmentStatus"];
        }
        if (!IsCheckedIn) {
            IsCheckedIn = 0;
        }
        var AppointmentStatusIds = $('#pnlDashboard #ddlAppointmentStatus option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 15 : RowsPerPage;

        var objData = new JSON.constructor();
        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        objData["AppDate"] = AppDate;
        objData["IsCheckedIn"] = IsCheckedIn;
        objData["AppointmentStatusIds"] = AppointmentStatusIds;
        //Start 13-10-2017 Humaira Yousaf IMP-1195
        objData["ProviderId"] = $('#pnlDashboard #pnlSchAppointmentGrid #hfProviderId').val();
        objData["FacilityId"] = $('#pnlDashboard #pnlSchAppointmentGrid #hfFacilityId').val();
        //End 13-10-2017 Humaira Yousaf IMP-1195
        objData["CommandType"] = "Load_App_By_Status";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "LoadDashBoard");
    },

    MessageCount: function () {
        Patient_MessageCompose.UserMessageCount().done(function (response) {
            if (response.status != false) {
                var MessageCountJSONData = JSON.parse(response.MessageCount_JSON);
                var totalCount = 0;
                $.each(MessageCountJSONData, function (index, element) {
                    if (element.MessageType != "Task") {
                        totalCount += parseInt(element.MessageCounts);
                    }

                });


                if (totalCount == 0) {
                    $('#wpanel div.wMessages .badge').hide();
                    $('.notifications div.wMessages .badge').hide();
                } else {
                    $('#wpanel div.wMessages .badge').show();
                    $('.notifications #Messages .badge').show();
                    $('#wpanel div.wMessages .badge').text(totalCount);
                    $('.notifications #Messages .badge').text(totalCount);
                }

                $('#spnMessageCount').text(totalCount == 0 ? '' : totalCount);
                $('#liMsgsPractice span').text(MessageCountJSONData[1].MessageCounts == 0 ? '' : MessageCountJSONData[1].MessageCounts);
                $('#liMsgsPatient span').text(MessageCountJSONData[2].MessageCounts == 0 ? '' : MessageCountJSONData[2].MessageCounts);
                $('#liMsgsDirect span').text(MessageCountJSONData[0].MessageCounts == 0 ? '' : MessageCountJSONData[0].MessageCounts);

            }
        });
    },
    //#endregion Appointments
    DashBoardPatientMessagesSearch: function (PageNo, rpp, GridData) {
        //  DashBoard.EnableDisableControl();
        if ($("#pnlDashboard #pnlUserMessagesGrid").css("display") == "none")
            $("#pnlDashboard #pnlUserMessagesGrid").show();

        globalAppdata.RecentMessagesTab = "Patient"
        $("#pnlDashboard #pnlUserMessagesGrid #msgsPractice").hide();
        $("#pnlDashboard #pnlUserMessagesGrid #msgsPatient").show();
        $("#pnlDashboard #pnlUserMessagesGrid #msgsLog").hide();
        utility.CreateDatePicker("pnlDashboard #frmDashboard #dtpPatientMsgDate", function () { }, false, null, null);
        DashBoard.LoadPatientMessages(PageNo, rpp).done(function (response) {
            if (response.status != false) {

                DashBoard.DashBoardPatientMessagesGridLoad(response, PageNo, rpp);
                $('#pnlDashboard .tabs-custom').eq(0).find('li').eq(1).attr('class', 'active');

            }
            else {
                utility.DisplayMessages(response.Message, 3);

            }
        });

        DashBoard.MessageCount();



    },
    //#region Messages
    DashBoardMessagesSearch: function (PageNo, rpp, GridData) {
        DashBoard.EnableDisableControl();

        if ($("#pnlDashboard #pnlUserMessagesGrid").css("display") == "none")
            $("#pnlDashboard #pnlUserMessagesGrid").show();

        globalAppdata.RecentMessagesTab = "Practice";
        $("#pnlDashboard #pnlUserMessagesGrid #msgsPractice").show();
        $("#pnlDashboard #pnlUserMessagesGrid #msgsPatient").hide();
        $("#pnlDashboard #pnlUserMessagesGrid #msgsLog").hide();
        if (globalAppdata['IsDirectAddress'] != 'True') {
            $('#pnlUserMessagesGrid .tabs-custom #liMsgsDirect').hide();
        }

        if (globalAppdata["isTransitionCareDirectProject"] && globalAppdata["isTransitionCareDirectProject"].toLowerCase() == "false")
            $("#pnlDashboard #widgetgridpanel #pnlUserMessagesGrid #liMsgsDirectOutgoing").addClass("hidden");
        else
            $("#pnlDashboard #widgetgridpanel #pnlUserMessagesGrid #liMsgsDirectOutgoing").removeClass("hidden");

        if (GridData) {
            DashBoard.DashBoardMessagesGridLoad(GridData, PageNo, rpp);
        }
        else {
            DashBoard.LoadMessages(PageNo, rpp).done(function (response) {
                if (response.status != false) {

                    DashBoard.DashBoardMessagesGridLoad(response, PageNo, rpp);
                    $('#pnlDashboard .tabs-custom').eq(0).find('li').eq(0).attr('class', 'active');

                }
                else {
                    utility.DisplayMessages(response.Message, 3);

                }
            });
        }
        DashBoard.MessageCount();



    },
    //adnan maqbool, EMR-916

    DashBoardDirectMessagesSearch: function (PageNo, rpp, GridData) {
        utility.CreateDatePicker("pnlDashboard #frmDashboard #dtpMsgDateDirect", function () { }, false, null, null);

        if ($("#pnlDashboard #pnlUserMessagesGrid").css("display") == "none")
            $("#pnlDashboard #pnlUserMessagesGrid").show();
        if (GridData) {
            DashBoard.DashBoardDirectMessagesGridLoad(GridData, PageNo, rpp);
        }
        else {
            DashBoard.LoadDirectMessages(PageNo, rpp).done(function (response) {
                if (response.status != false) {
                    DashBoard.DashBoardDirectMessagesGridLoad(response, PageNo, rpp);
                    $('#pnlDashboard .tabs-custom').eq(0).find('li').eq(2).attr('class', 'active');

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

        }
        DashBoard.MessageCount();
    },


    DashBoardLogMessagesSearch: function (PageNo, rpp, GridData) {
        if ($("#pnlDashboard #pnlUserMessagesGrid").css("display") == "none")
            $("#pnlDashboard #pnlUserMessagesGrid").show();

        $("#pnlDashboard #pnlUserMessagesGrid #msgsPractice").hide();
        $("#pnlDashboard #pnlUserMessagesGrid #msgsPatient").hide();
        $("#pnlDashboard #pnlUserMessagesGrid #msgsLog").show();
        if (GridData) {
            DashBoard.DashBoardLogMessagesGridLoad(GridData, PageNo, rpp);
        }
        else {
            DashBoard.LoadLogMessages(PageNo, rpp).done(function (response) {
                if (response.status != false) {
                    DashBoard.DashBoardLogMessagesGridLoad(response, PageNo, rpp);
                    $('#pnlDashboard .tabs-custom').eq(0).find('li').eq(4).attr('class', 'active');
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },


    checkUncheckAllOrders: function (obj) {
        if ($(obj).is(':checked')) {
            $("#pnlDashboard  #dgvLabOrderGrid #dgvLabOrderDashboard input[name='SelectCheckBoxOrder']:checkbox").prop('checked', true);
        } else {
            $("#pnlDashboard  #dgvLabOrderGrid #dgvLabOrderDashboard input[name='SelectCheckBoxOrder']:checkbox").prop('checked', false);
        }
    },
    checkUncheckAllResults: function (obj) {
        if ($(obj).is(':checked')) {
            $("#pnlDashboard  #LabResult #dgvLabOrderResult input[name='SelectCheckBoxResult']:checkbox").prop('checked', true);
        } else {
            $("#pnlDashboard  #LabResult #dgvLabOrderResult input[name='SelectCheckBoxResult']:checkbox").prop('checked', false);
        }
    },
    checkUncheckAllUnSolicitedResults: function (obj) {
        if ($(obj).is(':checked')) {
            $("#pnlDashboard  #LabUnsolicited #dgvLabUnsoOrderResult input[name='SelectCheckBoxResult']:checkbox").prop('checked', true);
        } else {
            $("#pnlDashboard  #LabUnsolicited #dgvLabUnsoOrderResult input[name='SelectCheckBoxResult']:checkbox").prop('checked', false);
        }
    },
    checkUncheckAllMessages: function (obj) {
        var tableId = $(obj.parentNode.parentNode.parentNode.parentNode).attr("id");
        if ($(obj).is(':checked')) {
            $("#pnlDashboard #pnlUserMessagesGrid #" + tableId + " input[name='SelectCheckBoxOrder']:checkbox").prop('checked', true);
        } else {
            $("#pnlDashboard #pnlUserMessagesGrid #" + tableId + " input[name='SelectCheckBoxOrder']:checkbox").prop('checked', false);
        }

    },
    checkUncheckMessage: function (event) {
        event.stopPropagation();
    },
    DashBoardMessagesGridLoad: function (response, PageNo, rpp) {
        //if (response.MessageCount == 0) {
        //    $('#pnlDashboard #Messages .badge').css("display", "none");
        //} else {
        //    $('#pnlDashboard #Messages .badge').css("display", "inline");
        //    $('#pnlDashboard #Messages .badge').text(response.MessageCount);
        //}
        if ($("#pnlDashboard #pnlUserMessagesGrid #dgvUserMessagesGrid thead tr #selectMessages").length > 0) {
            $("#pnlDashboard #pnlUserMessagesGrid #dgvUserMessagesGrid thead tr #selectMessages").prop('checked', false);
            $("#pnlDashboard #pnlUserMessagesGrid #dgvPatientMessagesGrid thead tr #selectMessages").prop('checked', false);
        }
        if (response.MessageCount > 0) {

            $("#pnlDashboard #divUserMessagesPaging").css("display", "inline");
            //Showing 1 to 15 of 15 Record(s)
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var params = [];
            params["CurrentPage"] = CurrentPage;
            params["RecordsPerPage"] = RecordsPerPage;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("divUserMessagesPaging", response.iTotalDisplayRecords, 5, "DashBoard", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#pnlDashboard #divUserMessagesPaging #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $("#pnlDashboard li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
        }
        else {
            $("#pnlDashboard #divUserMessagesPaging").css("display", "none");
        }

        $("#pnlDashboard #pnlUserMessagesGrid #dgvUserMessagesGrid").dataTable().fnDestroy();
        $("#pnlDashboard #pnlUserMessagesGrid #dgvUserMessagesGrid tbody").find("tr").remove();
        if ($("#pnlDashboard #pnlUserMessagesGrid #dgvUserMessagesGrid thead tr #selectMessages").length == 0) {
            $("#pnlDashboard #pnlUserMessagesGrid #dgvUserMessagesGrid thead tr").prepend('<th style="width:50px"><input type="checkbox" onchange="DashBoard.checkUncheckAllMessages(this);" controlname="selectMessages" id="selectMessages" name="chkHeader" class="input-block pull-left" coltype="checkbox"/></th>');
        } else {
            $("#pnlDashboard #pnlUserMessagesGrid #dgvUserMessagesGrid thead tr #selectMessages").prop('checked', false);
            $("#pnlDashboard #pnlUserMessagesGrid #dgvPatientMessagesGrid thead tr #selectMessages").prop('checked', false);
        }
        if (response.MessageCount > 0) {
            var MessageSearchJSONData = JSON.parse(response.MessageLoad_JSON);
            $.each(MessageSearchJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", '' + item.UserMessagesId + '');
                //$row.attr("onclick", "DashBoard.ViewUserMessage(" + item.UserMessagesId + ",event,'Practice'); utility.SelectGridRow($('#gvDashBoard_Message_row" + i + "'))");

                if (item.IsActive == "True") {
                    isactive = 0;
                    tglclass = "fa fa-toggle-on green";
                } else {
                    isactive = 1;
                    tglclass = "fa fa-toggle-on red";
                }

                var color = "";
                if (item.Priority == "High") {
                    color = 'style = "color:red"'
                } else if (item.Priority == "Medium") {
                    color = 'style = "color:orange"'
                } else if (item.Priority == "Low") {
                    color = 'style = "color:green"'
                }

                var font = "";

                if (item.IsRead == "False") {
                    font = 'style = "font-weight:bold"'
                } else if (item.IsRead == "True") {
                    font = "";
                }
                var messageisread = "";
                if (item.IsRead == "False") {
                    messageisread = "bg-info active";

                }
                $row.addClass(messageisread);

                var TaskMethod = "";
                var TaskTD = "";
                if (item.IsTaskAssociated == '0') {
                    //TaskMethod = "";

                    TaskTD = "";

                } else {
                    TaskMethod = "DashBoard.ViewUserTask(" + item.UserMessagesId + ",event);";
                    TaskTD = "<button type='button' class='btn btn-success btn-xs' title='Task' id='" + item.UserMessagesId + "' onclick='" + TaskMethod + "'>View</button>";
                }
                var onclick = 'onclick="DashBoard.ViewUserMessage(\'' + item.UserMessagesId + '\',event,\'Practice\');"';
                var SelectionCheckBoxColumn = '<td><input type="checkbox" onclick="DashBoard.checkUncheckMessage(event);" id="' + item.UserMessagesId + '" name="SelectCheckBoxOrder" class="input-block"/></td>';

                $row.append(SelectionCheckBoxColumn + '<td ' + onclick + '>' + item.UserNameWithPractice + '</td><td ' + onclick + '>' + item.AssignedFrom + '</td><td ' + onclick + '>' + item.Subject + '</td><td ' + onclick + '>' + item.CreatedOn + '</td><td ' + onclick + ' ' + color + '>' + item.Priority + '</td>');

                $("#pnlDashboard #dgvUserMessagesGrid tbody").last().append($row);
            });
        }
        else {
            $("#pnlDashboard #divUserMessagesPaging").css("display", "none");
            $('#pnlDashboard #dgvUserMessagesGrid').DataTable({
                "language": {
                    "emptyTable": "No Message Found"
                    //}, "autoWidth": false, "pageLength": 5, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false }]
                }, "searching": false, "autoWidth": false, "pageLength": 5, "bLengthChange": false, "order": [], "bPaginate": false, "bInfo": false, "aoColumnDefs": [{
                    "bSortable": false, "aTargets": [0
                    ]
                }]
            });
        }
        if (globalAppdata["isTransitionCareDirectProject"] && globalAppdata["isTransitionCareDirectProject"].toLowerCase() == "false")
            $("#pnlDashboard #widgetgridpanel #pnlUserMessagesGrid #liMsgsDirectOutgoing").addClass("hidden");
        else
            $("#pnlDashboard #widgetgridpanel #pnlUserMessagesGrid #liMsgsDirectOutgoing").removeClass("hidden");
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable('#pnlDashboard #dgvUserMessagesGrid'))
            ;
        else
            //$("#pnlDashboard #dgvUserMessagesGrid").DataTable({ "bInfo": false, "bPaginate": false, "pageLength": 5, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
            $("#pnlDashboard #dgvUserMessagesGrid").DataTable({ "searching": false, "bInfo": false, "bPaginate": false, "pageLength": 5, "bLengthChange": false, "autoWidth": false, "order": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
        //  $('.table-responsive').css('min-height', '220px');
    },
    DeleteSelectedMessages: function (gridId) {
        var tableId = gridId;
        var messageIDs = "";
        if ($("#pnlDashboard #pnlUserMessagesGrid #" + tableId + " tbody tr input:checked").length == 0) {
            utility.DisplayMessages('Please select any message to delete', 4);
        } else {
            $("#pnlDashboard #pnlUserMessagesGrid #" + tableId + " tbody tr input:checked").each(function (i, item) {

                messageIDs += "," + $(item).attr('id');
            });

            utility.myConfirm('66', function () {
                DashBoard.DeleteSelectedUserMessages(messageIDs);

            }, function () {
            },
                '1'
            );
        }


    },
    DashBoardPatientMessagesGridLoad: function (response, PageNo, rpp) {
        //if (response.MessageCount == 0) {
        //    $('#pnlDashboard #Messages .badge').css("display", "none");
        //} else {
        //    $('#pnlDashboard #Messages .badge').css("display", "inline");
        //    $('#pnlDashboard #Messages .badge').text(response.MessageCount);
        //}
        if ($("#pnlDashboard #pnlUserMessagesGrid #dgvPatientMessagesGrid thead tr #selectMessages").length > 0) {
            $("#pnlDashboard #pnlUserMessagesGrid #dgvUserMessagesGrid thead tr #selectMessages").prop('checked', false);
            $("#pnlDashboard #pnlUserMessagesGrid #dgvPatientMessagesGrid thead tr #selectMessages").prop('checked', false);
        }
        if (response.MessageCount > 0) {

            $("#pnlDashboard #divPatientMessagesPaging").css("display", "inline");
            //Showing 1 to 15 of 15 Record(s)
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var params = [];
            params["CurrentPage"] = CurrentPage;
            params["RecordsPerPage"] = RecordsPerPage;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("divPatientMessagesPaging", response.iTotalDisplayRecords, 5, "DashBoard", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#pnlDashboard #divPatientMessagesPaging #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $("#pnlDashboard li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
        }
        else {
            $("#pnlDashboard #divPatientMessagesPaging").css("display", "none");
        }

        $("#pnlDashboard #pnlUserMessagesGrid #dgvPatientMessagesGrid").dataTable().fnDestroy();
        $("#pnlDashboard #pnlUserMessagesGrid #dgvPatientMessagesGrid tbody").find("tr").remove();
        if ($("#pnlDashboard #pnlUserMessagesGrid #dgvPatientMessagesGrid thead tr #selectMessages").length == 0) {
            $("#pnlDashboard #pnlUserMessagesGrid #dgvPatientMessagesGrid thead tr").prepend('<th style="width:50px"><input type="checkbox" onchange="DashBoard.checkUncheckAllMessages(this);" controlname="selectMessages" id="selectMessages" name="chkHeader" class="input-block pull-left" coltype="checkbox"/></th>');
        } else {
            $("#pnlDashboard #pnlUserMessagesGrid #dgvUserMessagesGrid thead tr #selectMessages").prop('checked', false);
            $("#pnlDashboard #pnlUserMessagesGrid #dgvPatientMessagesGrid thead tr #selectMessages").prop('checked', false);
        }
        if (response.MessageCount > 0) {
            var MessageSearchJSONData = JSON.parse(response.MessageLoad_JSON);
            $.each(MessageSearchJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", '' + item.UserMessagesId + '');
                //$row.attr("onclick", "DashBoard.ViewUserMessage(" + item.UserMessagesId + ",event,'Patient'); utility.SelectGridRow($('#gvDashBoard_Message_row" + i + "'))");

                if (item.IsActive == "True") {
                    isactive = 0;
                    tglclass = "fa fa-toggle-on green";
                } else {
                    isactive = 1;
                    tglclass = "fa fa-toggle-on red";
                }

                var color = "";
                if (item.Priority == "High") {
                    color = 'style = "color:red"'
                } else if (item.Priority == "Medium") {
                    color = 'style = "color:orange"'
                } else if (item.Priority == "Low") {
                    color = 'style = "color:green"'
                }

                var font = "";

                if (item.IsRead == "False") {
                    font = 'style = "font-weight:bold"'
                } else if (item.IsRead == "True") {
                    font = "";
                }
                var messageisread = "";
                if (item.IsRead == "False") {
                    messageisread = "bg-info active";

                }
                $row.addClass(messageisread);

                var TaskMethod = "";
                var TaskTD = "";
                if (item.IsTaskAssociated == '0') {
                    //TaskMethod = "";

                    TaskTD = "";

                } else {
                    TaskMethod = "DashBoard.ViewUserTask(" + item.UserMessagesId + ",event);";
                    TaskTD = "<button type='button' class='btn btn-success btn-xs' title='Task' id='" + item.UserMessagesId + "' onclick='" + TaskMethod + "'>View</button>";
                }
                var onclick = 'onclick="DashBoard.ViewUserMessage(\'' + item.UserMessagesId + '\',event,\'Patient\');"';
                var SelectionCheckBoxColumn = '<td><input type="checkbox" onclick="DashBoard.checkUncheckMessage(event);" id="' + item.UserMessagesId + '" name="SelectCheckBoxOrder" class="input-block"/></td>';
                $row.append(SelectionCheckBoxColumn + '<td ' + onclick + '>' + item.PatientName + '</td><td ' + onclick + '>' + item.AssignedFrom + '</td><td ' + onclick + '>' + item.Subject + '</td><td ' + onclick + '>' + item.CreatedOn + '</td><td ' + onclick + ' ' + color + '>' + item.Priority + '</td>');

                $("#pnlDashboard #dgvPatientMessagesGrid tbody").last().append($row);
            });
        }
        else {
            $("#pnlDashboard #divPatientMessagesPaging").css("display", "none");
            $('#pnlDashboard #dgvPatientMessagesGrid').DataTable({
                "language": {
                    "emptyTable": "No Message Found"
                    //}, "autoWidth": false, "pageLength": 5, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false }]
                }, "searching": false, "autoWidth": false, "pageLength": 5, "bLengthChange": false, "order": [], "bPaginate": false, "bInfo": false, "aoColumnDefs": [{
                    "bSortable": false, "aTargets": [0
                    ]
                }]
            });
        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable('#pnlDashboard #dgvPatientMessagesGrid'))
            ;
        else
            //$("#pnlDashboard #dgvUserMessagesGrid").DataTable({ "bInfo": false, "bPaginate": false, "pageLength": 5, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
            $("#pnlDashboard #dgvPatientMessagesGrid").DataTable({ "searching": false, "bInfo": false, "bPaginate": false, "pageLength": 5, "bLengthChange": false, "autoWidth": false, "order": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
        //  $('.table-responsive').css('min-height', '220px');
    },

    DashBoardDirectMessagesGridLoad: function (response, PageNo, rpp) {
        //if (response.MessageCount == 0) {
        //    $('#pnlDashboard #Messages .badge').css("display", "none");
        //} else {
        //    $('#pnlDashboard #Messages .badge').css("display", "inline");
        //    $('#pnlDashboard #Messages .badge').text(response.MessageCount);
        //}
        if (response.MessageCount > 0) {

            $("#pnlDashboard #divDirectMessagesPaging").css("display", "inline");
            //Showing 1 to 15 of 15 Record(s)
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var params = [];
            params["CurrentPage"] = CurrentPage;
            params["RecordsPerPage"] = RecordsPerPage;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("divDirectMessagesPaging", response.iTotalDisplayRecords, 5, "DashBoard", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#pnlDashboard #divDirectMessagesPaging #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $("#pnlDashboard li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
        }
        else {
            $("#pnlDashboard #divDirectMessagesPaging").css("display", "none");
        }

        $("#pnlDashboard #pnlUserMessagesGrid #dgvDirectMessagesGrid").dataTable().fnDestroy();
        $("#pnlDashboard #pnlUserMessagesGrid #dgvDirectMessagesGrid tbody").find("tr").remove();
        if (response.MessageCount > 0) {
            var MessageSearchJSONData = JSON.parse(response.DirectMessageLoad_JSON);
            $.each(MessageSearchJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", '' + item.UserMessagesId + '');
                $row.attr("onclick", "utility.SelectGridRow($('#gvDashBoard_Message_row" + i + "'))");

                if (item.IsActive == "True") {
                    isactive = 0;
                    tglclass = "fa fa-toggle-on green";
                } else {
                    isactive = 1;
                    tglclass = "fa fa-toggle-on red";
                }

                var color = "";
                if (item.Priority == "High") {
                    color = 'style = "color:red"'
                } else if (item.Priority == "Medium") {
                    color = 'style = "color:orange"'
                } else if (item.Priority == "Low") {
                    color = 'style = "color:green"'
                }

                var font = "";
                if (item.IsRead == "False") {
                    font = 'style = "font-weight:bold"'
                } else if (item.IsRead == "True") {
                    font = "";
                }

                var messageisread = "";
                if (item.IsRead == "False") {
                    messageisread = "bg-info active";

                }
                $row.addClass(messageisread);

                $row.append('<td><a class="btn  btn-xs"  href="#" onclick="DashBoard.DeleteUserMessage(' + item.UserMessagesId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="DashBoard.ViewDirectMessage(' + item.UserMessagesId + ',event,\'DIRECT\');"  title="View Message"><i class="fa fa-eye"></i></a></td><td>' + item.AssignedFrom + '</td><td>' + item.Subject + '</td><td>' + item.CreatedOn + '</td>');

                $("#pnlDashboard #dgvDirectMessagesGrid tbody").last().append($row);
            });
        }
        else {
            $("#pnlDashboard #divDirectMessagesPaging").css("display", "none");
            $('#pnlDashboard #dgvDirectMessagesGrid').DataTable({
                "language": {
                    "emptyTable": "No Message Found"
                    //}, "autoWidth": false, "pageLength": 5, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false }]
                }, "autoWidth": false, "pageLength": 5, "bLengthChange": false, "order": [], "aoColumnDefs": [{
                    "bSortable": false, "aTargets": [0
                    ]
                }]
            });
        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable('#pnlDashboard #dgvDirectMessagesGrid'))
            ;
        else
            //$("#pnlDashboard #dgvUserMessagesGrid").DataTable({ "bInfo": false, "bPaginate": false, "pageLength": 5, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
            $("#pnlDashboard #dgvDirectMessagesGrid").DataTable({ "bInfo": false, "bPaginate": false, "pageLength": 5, "bLengthChange": false, "autoWidth": false, "order": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
        //  $('.table-responsive').css('min-height', '220px');
    },



    DashBoardLogMessagesGridLoad: function (response, PageNo, rpp) {
        //if (response.MessageCount == 0) {
        //    $('#pnlDashboard #Messages .badge').css("display", "none");
        //} else {
        //    $('#pnlDashboard #Messages .badge').css("display", "inline");
        //    $('#pnlDashboard #Messages .badge').text(response.MessageCount);
        //}
        if (response.MessageCount > 0) {

            $("#pnlDashboard #divLogMessagesPaging").css("display", "inline");
            //Showing 1 to 15 of 15 Record(s)
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var params = [];
            params["CurrentPage"] = CurrentPage;
            params["RecordsPerPage"] = RecordsPerPage;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("divLogMessagesPaging", response.iTotalDisplayRecords, 5, "DashBoard", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#pnlDashboard #divLogMessagesPaging #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $("#pnlDashboard li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
        }
        else {
            $("#pnlDashboard #divLogMessagesPaging").css("display", "none");
        }

        $("#pnlDashboard #pnlUserMessagesGrid #dgvLogMessagesGrid").dataTable().fnDestroy();
        $("#pnlDashboard #pnlUserMessagesGrid #dgvLogMessagesGrid tbody").find("tr").remove();
        if (response.MessageCount > 0) {
            var MessageSearchJSONData = JSON.parse(response.MessageLogLoad_JSON);
            $.each(MessageSearchJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", '' + item.UserMessagesId + '');
                $row.attr("onclick", "utility.SelectGridRow($('#gvDashBoard_Message_row" + i + "'))");

                if (item.IsActive == "True") {
                    isactive = 0;
                    tglclass = "fa fa-toggle-on green";
                } else {
                    isactive = 1;
                    tglclass = "fa fa-toggle-on red";
                }


                var font = "";
                if (item.IsRead == "False") {
                    font = 'style = "font-weight:bold"'
                } else if (item.IsRead == "True") {
                    font = "";
                }

                $row.append('<td>' + item.Date + '</td><td>' + item.from + '</td><td>' + item.to + '</td><td>' + item.Subject + '</td><td>' + item.messagetype + '</td>');

                $("#pnlDashboard #dgvLogMessagesGrid tbody").last().append($row);
            });
        }
        else {
            $("#pnlDashboard #divLogMessagesPaging").css("display", "none");
            $('#pnlDashboard #dgvLogMessagesGrid').DataTable({
                "language": {
                    "emptyTable": "No Message Found"
                    //}, "autoWidth": false, "pageLength": 5, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false }]
                }, "searching": false, "autoWidth": false, "pageLength": 5, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{
                    "bSortable": false, "aTargets": [0
                    ]
                }]
            });
        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable('#pnlDashboard #dgvLogMessagesGrid'))
            ;
        else
            //$("#pnlDashboard #dgvUserMessagesGrid").DataTable({ "bInfo": false, "bPaginate": false, "pageLength": 5, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
            $("#pnlDashboard #dgvLogMessagesGrid").DataTable({ "searching": false, "bInfo": false, "bPaginate": false, "pageLength": 5, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]] }); // to remove records per page dropdown
        //  $('.table-responsive').css('min-height', '220px');
    },

    UserMessageAddEdit: function (MessageId, mode, AssignedToId, StatusId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Tasks", "Edit", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["MessageId"] = MessageId;
                params["mode"] = mode;
                params["ParentCtrl"] = "mstrTabDashBoard";
                params["AssignedToId"] = AssignedToId;
                params["FromDashboard"] = "1";
                if (mode == "Add") {
                    LoadActionPan('Patient_MessageAdd', params);
                    //LoadActionPan('Patient_MessageReply', params);
                }
                else if (mode == "Edit") {
                    LoadActionPan('Patient_MessageEdit', params);
                }
                else if (mode == "Reply") {
                    params["StatusId"] = StatusId;
                    LoadActionPan('Patient_MessageReply', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    //#endregion Messages

    //#region Tasks
    DashBoardTasksSearch: function (PageNo, rpp, GridData) {
        DashBoard.EnableDisableControl();
        if ($("#pnlDashboard #pnlUserTaskGrid").css("display") == "none")
            $("#pnlDashboard #pnlUserTaskGrid").show();

        if (GridData) {

            DashBoard.DashBoardTasksGridLoad(GridData, PageNo, rpp);
        }
        else {

        }
        // PRD-260
        DashBoard.UserTaskSearch(0, globalAppdata["AppUserId"], "Task,Reminder,Amendment,Other", 2, PageNo, rpp).done(function (response) {
            if (response.status != false) {
                DashBoard.DashBoardTasksGridLoad(response, PageNo, rpp);
            }
            else {
                utility.DisplayMessages(response.Message, 3);

            }
        });


    },
    DashBoardTasksGridLoad: function (response, PageNo, rpp) {

        if (response.UserTaskCount > 0) {
            $("#pnlDashboard #divUserTasksPaging").css("display", "inline");
            //Showing 1 to 15 of 15 Record(s)
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var params = [];
            params["CurrentPage"] = CurrentPage;
            params["RecordsPerPage"] = RecordsPerPage;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("divUserTasksPaging", response.iTotalDisplayRecords, 5, "DashBoard", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#pnlDashboard #divUserTasksPaging #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $("#pnlDashboard li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
        }
        else {
            $("#pnlDashboard #divUserTasksPaging").css("display", "none");
        }

        if (response.UserTaskCount == 0) {
            $('#pnlDashboard div.wTasks .badge').css("display", "none");
        } else {
            $('#pnlDashboard div.wTasks .badge').css("display", "inline");
            $('#pnlDashboard div.wTasks .badge').text(response.iTotalDisplayRecords);
        }

        $("#pnlDashboard #pnlUserTaskGrid #dgvUserTaskGrid").dataTable().fnDestroy();
        $("#pnlDashboard #pnlUserTaskGrid #dgvUserTaskGrid tbody").find("tr").remove();
        if (response.UserTaskCount > 0) {
            var TaskSearchJSONData = JSON.parse(response.UserTaskLoad_JSON);
            $.each(TaskSearchJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", "gvDashBoard_Task_row" + i + "");
                $row.attr("onclick", "utility.SelectGridRow($('#gvDashBoard_Task_row" + i + "'))");

                var EditMethod = "DashBoard.UserMessageAddEdit(" + item.PatMsgId.trim() + ",'Edit');";
                var AddMessageReplyMethod = "DashBoard.UserMessageAddEdit(" + item.PatMsgId.trim() + ",'Reply'," + item.AssignedToId + "," + item.MsgStatusId + ");";


                $row.append('<td><a class="btn btn-xs" href="#" onclick="' + EditMethod + '"  title="Edit Record"><i class="fa fa-edit black"></i></a><a class="btn  btn-xs" href="#" onclick="' + AddMessageReplyMethod + '" title="Reply"><i class="fa fa-reply"></i></a>&nbsp;</td><td><a href="#" onclick="utility.PatientDemographics(' + item.PatientId + ', ' + "mstrTabDashBoard" + ' ,event);"  title="View Patient">' + item.AccountNumber + '</a></td><td><a href="#" onclick="utility.PatientDemographics(' + item.PatientId + ', ' + "mstrTabDashBoard" + ' ,event);"  title="View Patient">' + item.PatientName + '</a></td><td>' + item.ProviderName + '</td><td>' + item.FacilityName + '</td><td>' + item.MsgDetail + '</td><td>' + item.MessageStatus + '</td><td>' + item.CreatedBy + '</td><td>' + item.EntryDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td>');



                $("#pnlDashboard #dgvUserTaskGrid tbody").last().append($row);
            });
        }
        else {
            $("#pnlDashboard #divUserTasksPaging").css("display", "none");
            $('#pnlDashboard #dgvUserTaskGrid').DataTable({
                "language": {
                    "emptyTable": "No Task Found"
                }, "searching": false, "autoWidth": false, "bLengthChange": false, "order": [], "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
                //}, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false}]
            });
        }
        if ($.fn.dataTable.isDataTable('#pnlDashboard #dgvUserTaskGrid'))
            ;
        else
            $("#pnlDashboard #dgvUserTaskGrid").DataTable({
                "searching": false, "bInfo": false, "bPaginate": false, "aaSorting": [], "bSortable": false, "bLengthChange": false, "autoWidth": false
            , "order": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            }); // to remove records per page dropdown
        //  $('.table-responsive').css('min-height', '220px');
    },
    //#endregion Task
    DashBoardEncounterSearchOld: function (PageNo, rpp, GridData) {
        //Start By Khaleel Ur Rehman to Validate From and To Date , on 28 january 2016.
        utility.ValidateFromToDate('frmDashboard', 'OlddpVisitFrom', 'OlddpVisitTo', true);
        //End By Khaleel Ur Rehman to Validate From and To Date , on 28 january 2016.
        DashBoard.EnableDisableControl();
        if ($("#pnlDashboard #pnlEncounterGridOld").css("display") == "none")
            $("#pnlDashboard #pnlEncounterGridOld").show();
        var myJSON = new Object();

        myJSON.txtProvider = null;
        myJSON.providerId = null;
        myJSON.txtFacility = null;
        myJSON.dtpAppointmentDateFrom = $.datepicker.formatDate('mm/dd/yy', new Date());
        myJSON.dtpAppointmentDateTo = $.datepicker.formatDate('mm/dd/yy', new Date());
        myJSON.txtClaimNumber = "";
        myJSON.ddlActive = "";
        var myString = JSON.stringify(myJSON);
        if (GridData) {

            DashBoard.DashBoardEncounterGridLoadOld(GridData, PageNo, rpp);
            var TableControl = "pnlDashboard #pnlEncounterGridOld #OlddgvEncounterGrid";
            var PagingPanelControlID = "pnlDashboard #pnlEncounterGridOld #OlddgvEncounterGrid_paginate";
            var ClassControlName = "DashBoard";
            var PagesToDisplay = 5;
            var iTotalDisplayRecords = GridData.iTotalDisplayRecords;
            setTimeout(CreatePagination(GridData.VisitsCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                DashBoard.DashBoardEncounterSearchOld(PageNumber, ResultPerPage, null);

            }), 10);
        }
        else {

            DashBoard.SearchVisitsOld(PageNo, rpp).done(function (response) {
                if (response.status != false) {
                    DashBoard.DashBoardEncounterGridLoadOld(response, PageNo, rpp);
                    var TableControl = "pnlDashboard #pnlEncounterGridOld #OlddgvEncounterGrid";
                    var PagingPanelControlID = "pnlDashboard #pnlEncounterGridOld #OlddgvEncounterGrid_paginate";
                    var ClassControlName = "DashBoard";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout(CreatePagination(response.VisitsCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        DashBoard.DashBoardEncounterSearchOld(PageNumber, ResultPerPage, null);
                    }), 10);
                    $("#pnlDashboard #pnlEncounterGridOld #OldtxtProvider").val($("#pnlDashboard #OldNoteProviderText").val());
                }
                else {
                    utility.DisplayMessages(response.Message, 3);

                }
            });
            DashBoard.LoadNotesDraftCount().done(function (response) {
                if (response.status != false) {
                    $("#pnlDashboard #OldspnDraftNoteCount").html(response.DraftCount == 0 ? '' : '<button class="btn btn-danger btn-xs tab_space noWordBreak" onclick="DashBoard.SearchDashBoardEncounterDraft();">' + response.DraftCount + ' Draft Note(s)</button>');
                    $("#pnlDashboard #pnlEncounterGridOld #OldtxtProvider").val($("#pnlDashboard #OldNoteProviderText").val());
                }
                else {
                    utility.DisplayMessages(response.Message, 3);

                }
            });
        }

    },
    DashBoardEncounterGridLoadOld: function (response, PageNo, rpp) {
        var gridControlEncounter = $('#pnlDashboard #pnlEncounterGridOld #OldgridControlEncounter').html();
        var dpVisitFromValue = $('#pnlDashboard #pnlEncounterGridOld #OlddpVisitFrom').val();
        var dpVisitToValue = $('#pnlDashboard #pnlEncounterGridOld #OlddpVisitTo').val();
        var ddlNoteStatusValue = $('#pnlDashboard #pnlEncounterGridOld #OldddlNoteStatus').val();
        var providerId = $("#pnlDashboard #pnlEncounterGridOld #OldhfProviderId").val()
        var providerText = $("#pnlDashboard #pnlEncounterGridOld #OldtxtProvider").val();
        var patientId = $("#pnlDashboard #pnlEncounterGridOld #OldhfPatientId").val();
        var patientText = $("#pnlDashboard #pnlEncounterGridOld #OldtxtFullName").val();
        var ddlNoteTypeValue = $('#pnlDashboard #pnlEncounterGridOld #OldddlNoteType').val();

        $('#pnlDashboard #pnlEncounterGridOld #OldgridControlEncounter').remove();

        $("#pnlDashboard #pnlEncounterGridOld #OlddgvEncounterGrid").dataTable().fnDestroy();
        $("#pnlDashboard #pnlEncounterGridOld #OlddgvEncounterGrid tbody").find("tr").remove();

        if (response.VisitsCount == 0) {
            $('#pnlDashboard div.wEncounter .badge').css("display", "none");
            $('#spnNotesCount').css("display", "none");
            // $('#wpanel #widgetpanel .slick-track div').eq(24).find('span:last').hide();
        } else {
            $('#pnlDashboard div.wEncounter .badge').css("display", "inline");
            $('#spnNotesCount').css("display", "inline");
            $('#wpanel .slick-track div').each(function (i) {
                if ($(this).find('span:first').text() == 'Notes') {
                    $(this).find('span:last').text(response.iTotalDisplayRecords);
                    $(this).find('span:last').show();
                }

            });
            //  $('#wpanel #widgetpanel .slick-track div').eq(24).find('span:last').show();
            // $('#wpanel #widgetpanel .slick-track div').eq(24).find('span:last').text(response.iTotalDisplayRecords);
            $('#pnlDashboard div.wEncounter .badge').text(response.iTotalDisplayRecords);
            //$('#spnNotesCount').text(response.iTotalDisplayRecords);

        }

        Clinical_Notes.canSignNote().done(function (canSign) {
            if (response.VisitsCount > 0) {

                var EncounterSearchJSONData = JSON.parse(response.VisitsLoad_JSON);
                $.each(EncounterSearchJSONData, function (i, item) {
                    var $row = $('<tr/>');
                    $row.attr("id", "OldgvDashBoard_Encounter_row" + i + "");

                    var ClassDisabled = item.NoteStatus.toUpperCase() == "SIGNED" ? "disabled" : "";

                    var DemographicsMethod = "utility.PatientDemographics('" + item.PatientId + "', 'mstrTabDashBoard' ,event);";

                    var NoteDeleteMethod = "DashBoard.NotesDelete('" + item.NotesId + "',event);";

                    var NoteStatusUpdateMethod = "DashBoard.NotesStatusUpdate('" + item.NotesId + "', '" + item.PatientId + "', '" + item.ProviderId + "',false,'" + utility.RemoveTimeFromDate(null, item.VisitDate) + "'," + item.BillingInfoId + ",'" + item.AppointmentDate + "'," + item.VisitId + ",'" + utility.RemoveTimeFromDate(null, item.NoteDate) + "','" + item.PatientTypeId + "', '" + item.FacilityId + "', '" + item.POS + "'," + (!item.RefProviderId ? null : +item.RefProviderId) + "," + (item.IsPhoneEncounter == "False" ? false : true) + ',event' + ");";

                    var NotesPreview = "DashBoard.NotesPreview('" + item.NotesId + "','" + item.PatientId + "','" + item.ProviderId + "','" + "" + "',event,'" + utility.RemoveTimeFromDate(null, item.VisitDate) + "','" + item.BillingInfoId + "','" + item.AppointmentDate + "'," + item.VisitId + ",'" + item.NoteDate + "','" + item.PatientTypeId + "', '" + item.FacilityId + "', '" + item.POS + "'," + (!item.RefProviderId ? null : +item.RefProviderId) + "," + (item.IsPhoneEncounter == "False" ? false : true) + ",'" + item.BillingStatus + "');";
                    var isVisible = 'style="display:none;';
                    if (response.HasDeleteRights != "No") {
                        isVisible = "";
                    }
                    var EditProgressNoteMethod = "DashBoard.EditProgressNote('" + item.NotesId + "', '" + item.PatientId + "', event" + ");";
                    if (canSign) {
                        $row.append('<td><a class="btn btn-xs ' + ClassDisabled + '" onclick="' + NoteDeleteMethod + '"  ' + ' ' + isVisible + ' title="Delete Note"> <i class="fa fa-close red"></i></a>&nbsp; <a class="btn btn-xs ' + ClassDisabled + '" onclick="' + EditProgressNoteMethod + '" title="Edit Note"> <i class="fa fa-edit black"></i></a> <a class="btn  btn-xs" onclick="' + NotesPreview + '" title="Note Preview"> <i class="fa fa-credit-card blue"></i></a>&nbsp; <a class="btn btn-xs ' + ClassDisabled + '" onclick="' + NoteStatusUpdateMethod + '" title="Sign Note"> <i class="fa fa-calculator black"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Audit Log" onclick="DashBoard.rowHistory(' + item.NotesId + ',\'' + utility.RemoveTimeFromDate(null, item.VisitDate) + '\',\'' + item.AppReason + '\',\'' + item.NoteType + '\',event);"> <i class="fa fa-history blue"></i></a>  </td><td><a onclick="' + DemographicsMethod + '"  title="View Patient">' + item.AccountNumber + '</a></td><td><a onclick="' + DemographicsMethod + '"  title="View Patient">' + item.PatientName + '</a></td><td>' + item.ProviderName + '</td><td>' + item.FacilityName + '</td><td>' + utility.RemoveTimeFromDate(null, item.VisitDate) + '</td><td>' + item.AppReason + '</td><td>' + item.NoteType + '</td><td>' + item.CC + '</td><td>' + item.NoteStatus + '</td>');
                    }
                    else {
                        $row.append('<td><a class="btn btn-xs ' + ClassDisabled + '" onclick="' + NoteDeleteMethod + '"  ' + ' ' + isVisible + ' title="Delete Note"> <i class="fa fa-close red"></i></a>&nbsp; <a class="btn btn-xs ' + ClassDisabled + '" onclick="' + EditProgressNoteMethod + '" title="Edit Note"> <i class="fa fa-edit black"></i></a> <a class="btn  btn-xs" onclick="' + NotesPreview + '" title="Note Preview"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Audit Log" onclick="DashBoard.rowHistory(' + item.NotesId + ',\'' + utility.RemoveTimeFromDate(null, item.VisitDate) + '\',\'' + item.AppReason + '\',\'' + item.NoteType + '\',event);"> <i class="fa fa-history blue"></i></a>  </td><td><a onclick="' + DemographicsMethod + '"  title="View Patient">' + item.AccountNumber + '</a></td><td><a onclick="' + DemographicsMethod + '"  title="View Patient">' + item.PatientName + '</a></td><td>' + item.ProviderName + '</td><td>' + item.FacilityName + '</td><td>' + utility.RemoveTimeFromDate(null, item.VisitDate) + '</td><td>' + item.AppReason + '</td><td>' + item.NoteType + '</td><td>' + item.CC + '</td><td>' + item.NoteStatus + '</td>');
                    }
                    $row.attr("onClick", item.NoteStatus.toUpperCase() == "SIGNED" ? NotesPreview : EditProgressNoteMethod);

                    $("#pnlDashboard #OlddgvEncounterGrid tbody").last().append($row);
                });
            }
            else {
                //$("#pnlDashboard #divEncounterPaging").css("display", "none");
                $('#pnlDashboard #OlddgvEncounterGrid').DataTable({
                    "language": {
                        "emptyTable": "No Encounter Found"
                    }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }], searching: false
                });
            }
            if ($.fn.dataTable.isDataTable('#pnlDashboard #OlddgvEncounterGrid'))
                ;
            else
                $("#pnlDashboard #OlddgvEncounterGrid").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }], searching: false }); // to remove records per page dropdown
            //.DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
            //  $('.table-responsive').css('min-height', '220px');

            $("#pnlDashboard #OlddgvEncounterGrid th")[9].click();
            // $('#pnlDashboard #dgvEncounterGrid_wrapper .datatables-header div:first').addClass('.col-sm-12 .col-md-8');
            $('#pnlDashboard #OlddgvEncounterGrid_wrapper .datatables-header div:first').html(' <div id="OldgridControlEncounter">' + gridControlEncounter + '</div>');
            $('#pnlDashboard #pnlEncounterGridOld #OldgridControlEncounter #OlddpVisitFrom').val(dpVisitFromValue);
            $('#pnlDashboard #pnlEncounterGridOld #OldgridControlEncounter #OlddpVisitTo').val(dpVisitToValue);
            $('#pnlDashboard #pnlEncounterGridOld #OldgridControlEncounter #OldddlNoteStatus').val(ddlNoteStatusValue);
            $('#pnlDashboard #pnlEncounterGridOld #OldgridControlEncounter #OldddlNoteType').val(ddlNoteTypeValue);

            $("#pnlDashboard #pnlEncounterGridOld #OldhfProviderId").val(providerId)
            $("#pnlDashboard #pnlEncounterGridOld #OldtxtProvider").val(providerText);
            $("#pnlDashboard #pnlEncounterGridOld #OldhfPatientId").val(patientId);
            $("#pnlDashboard #pnlEncounterGridOld #OldtxtFullName").val(patientText);

            if (dpVisitFromValue != '') {
                $('#pnlDashboard #OlddpVisitTo').removeAttr('disabled');
            } else {
                $('#pnlDashboard #OlddpVisitTo').attr('disabled', true); $('#pnlDashboard #OlddpVisitTo').val('');
            }
            DashBoard.documentReady(false);
            $('#pnlDashboard #OlddgvEncounterGrid_wrapper .datatables-header div.dataTables_filter').parent().removeClass('col-sm-12 col-md-6');
            $('#pnlDashboard #OlddgvEncounterGrid_wrapper .datatables-header div:first').removeClass('col-sm-12 col-md-6');


            if (!$('#pnlDashboard #OlddgvEncounterGrid_wrapper .datatables-header div.dataTables_filter').parent().hasClass('col-sm-6 col-md-5')) {
                $('#pnlDashboard #OlddgvEncounterGrid_wrapper .datatables-header div.dataTables_filter').parent().addClass('col-sm-6 col-md-5');
            }
            if (!$('#pnlDashboard #OlddgvEncounterGrid_wrapper .datatables-header div.dataTables_filter').parent().hasClass('mt-md')) {
                $('#pnlDashboard #OlddgvEncounterGrid_wrapper .datatables-header div.dataTables_filter').addClass('mt-md');
            }
            if (!$('#pnlDashboard #OlddgvEncounterGrid_wrapper .datatables-header div:first').hasClass('col-sm-6 col-md-9')) {
                $('#pnlDashboard #OlddgvEncounterGrid_wrapper .datatables-header div:first').addClass('col-sm-12 col-md-12');
            }
            //Start 16-10-2017 Humaira Yousaf IMP-1197
            if ($('#pnlDashboard #OlddgvEncounterGrid_wrapper .datatables-header').hasClass('form-inline')) {
                $('#pnlDashboard #OlddgvEncounterGrid_wrapper .datatables-header').removeClass('form-inline')
            }
            //End 16-10-2017 Humaira Yousaf IMP-1197
        });
    },

    SearchVisitsOld: function (PageNumber, RowsPerPage) {

        var VisitFrom = "";
        var VisitTo = "";
        var Status = "";
        var providerId = "";
        var patientId = "";
        var noteType = "";

        VisitFrom = $("#pnlDashboard #OlddpVisitFrom").val();
        VisitTo = $("#pnlDashboard #OlddpVisitTo").val();
        providerId = $("#pnlDashboard #pnlEncounterGridOld #OldhfProviderId").val() == "" || $("#pnlDashboard #pnlEncounterGridOld #OldhfProviderId").val() == null ? null : $("#pnlDashboard #pnlEncounterGridOld #OldhfProviderId").val();
        patientId = $("#pnlDashboard #pnlEncounterGridOld #OldhfPatientId").val() == "" || $("#pnlDashboard #pnlEncounterGridOld #OldhfPatientId").val() == null ? null : $("#pnlDashboard #pnlEncounterGridOld #OldhfPatientId").val();
        if ($("#pnlDashboard #OldddlNoteStatus option:selected").val() == "") {
            Status = "";
        }
        else {
            Status = $("#pnlDashboard #OldddlNoteStatus option:selected").text();
        }
        //Start 16-10-2017 Humaira Yousaf IMP-1197
        if ($("#pnlDashboard #OldddlNoteType option:selected").val() == "") {
            noteType = "";
        }
        else {
            noteType = $("#pnlDashboard #OldddlNoteType option:selected").text();
        }
        //End 16-10-2017 Humaira Yousaf IMP-1197
        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 15 : RowsPerPage;

        var objData = new JSON.constructor();

        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        objData["VisitFrom"] = VisitFrom;
        objData["VisitTo"] = VisitTo;
        objData["Status"] = Status;
        objData["IsDraftNote"] = false;
        objData["providerId"] = providerId;
        objData["patientId"] = patientId;
        objData["NoteType"] = noteType;

        if ($("#pnlDashboard #pnlEncounterGridOld #OldhfIsDraftNote").val() == "1") {
            objData["IsDraftNote"] = true;
        }

        objData["CommandType"] = "SEARCH_VISITS_NOTE";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "SearchDashBoard");
    },
    //#region Encounter search for ready to sign notes
    DashBoardEncounterSearch: function (PageNo, rpp, GridData) {
        var objDeffered = $.Deferred();
        DashBoard.pageNoReadyToSign = PageNo;
        $("#pnlDashboard #pnlEncounterGrid #dgvEncounterGrid #chkReadytosign").prop("checked", false);
        //Start By Khaleel Ur Rehman to Validate From and To Date , on 28 january 2016.
        utility.ValidateFromToDate('frmDashboard', 'dpVisitFrom', 'dpVisitTo', true);
        //End By Khaleel Ur Rehman to Validate From and To Date , on 28 january 2016.
        DashBoard.EnableDisableControl();
        if ($("#pnlDashboard #pnlEncounterGrid").css("display") == "none")
            $("#pnlDashboard #pnlEncounterGrid").show();
        var myJSON = new Object();

        myJSON.txtProvider = null;
        myJSON.providerId = null;
        myJSON.txtFacility = null;
        myJSON.dtpAppointmentDateFrom = $.datepicker.formatDate('mm/dd/yy', new Date());
        myJSON.dtpAppointmentDateTo = $.datepicker.formatDate('mm/dd/yy', new Date());
        myJSON.txtClaimNumber = "";
        myJSON.ddlActive = "";
        var myString = JSON.stringify(myJSON);
        if (GridData) {
            $.when(DashBoard.DashBoardEncounterGridLoad(GridData, PageNo, rpp)).then(function () {
                objDeffered.resolve("ok");
            });
            var TableControl = "pnlDashboard #pnlEncounterGrid #ctrlPanReadyToSign #dgvEncounterGrid";
            var PagingPanelControlID = "pnlDashboard #pnlEncounterGrid #ctrlPanReadyToSign #dgvEncounterGrid_paginate";
            var ClassControlName = "DashBoard";
            var PagesToDisplay = 5;
            var iTotalDisplayRecords = GridData.iTotalDisplayRecords;
            setTimeout(CreatePagination(GridData.VisitsCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                DashBoard.DashBoardEncounterSearch(PageNumber, ResultPerPage, null);

            }), 10);
        }
        else {
            DashBoard.SearchVisits(PageNo, rpp, 'Draft', 'ctrlPanReadyToSign').done(function (response) {
                if (response.status != false) {
                    $.when(DashBoard.DashBoardEncounterGridLoad(response, PageNo, rpp)).then(function () {
                        objDeffered.resolve("ok");
                    });
                    var TableControl = "pnlDashboard #pnlEncounterGrid #ctrlPanReadyToSign #dgvEncounterGrid";
                    var PagingPanelControlID = "pnlDashboard #pnlEncounterGrid #ctrlPanReadyToSign #dgvEncounterGrid_paginate";
                    var ClassControlName = "DashBoard";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout(CreatePagination(response.VisitsCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        DashBoard.DashBoardEncounterSearch(PageNumber, ResultPerPage, null);
                    }), 10);
                    $("#pnlDashboard #pnlEncounterGrid #ctrlPanReadyToSign #txtProvider").val($("#pnlDashboard #ctrlPanReadyToSign #NoteProviderText").val());
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        return objDeffered;
    },

    //#region Encounter search for Signed  notes
    DashBoardEncounterSearchSigned: function (PageNo, rpp, GridData) {
        //Start By Khaleel Ur Rehman to Validate From and To Date , on 28 january 2016.
        utility.ValidateFromToDate('frmDashboard', 'dpVisitFrom', 'dpVisitTo', true);
        //End By Khaleel Ur Rehman to Validate From and To Date , on 28 january 2016.
        DashBoard.EnableDisableControl();
        if ($("#pnlDashboard #pnlEncounterGrid").css("display") == "none")
            $("#pnlDashboard #pnlEncounterGrid").show();
        var myJSON = new Object();

        myJSON.txtProvider = null;
        myJSON.providerId = null;
        myJSON.txtFacility = null;
        myJSON.dtpAppointmentDateFrom = $.datepicker.formatDate('mm/dd/yy', new Date());
        myJSON.dtpAppointmentDateTo = $.datepicker.formatDate('mm/dd/yy', new Date());
        myJSON.txtClaimNumber = "";
        myJSON.ddlActive = "";
        var myString = JSON.stringify(myJSON);
        if (GridData) {
            DashBoard.DashBoardEncounterGridLoadSigned(GridData, PageNo, rpp);
            var TableControl = "pnlDashboard #pnlEncounterGrid #ctrlPanSigned #dgvEncounterGridSigned";
            var PagingPanelControlID = "pnlDashboard #pnlEncounterGrid #ctrlPanSigned #dgvEncounterGrid_paginateSigned";
            var ClassControlName = "DashBoard";
            var PagesToDisplay = 5;
            var iTotalDisplayRecords = GridData.iTotalDisplayRecords;
            setTimeout(CreatePagination(GridData.VisitsCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                DashBoard.DashBoardEncounterSearchSigned(PageNumber, ResultPerPage, null);
            }), 10);
        }
        else {
            DashBoard.SearchVisits(PageNo, rpp, 'Signed', 'ctrlPanSigned').done(function (response) {
                if (response.status != false) {
                    DashBoard.DashBoardEncounterGridLoadSigned(response, PageNo, rpp);
                    var TableControl = "pnlDashboard #pnlEncounterGrid #ctrlPanSigned #dgvEncounterGridSigned";
                    var PagingPanelControlID = "pnlDashboard #pnlEncounterGrid #ctrlPanSigned #dgvEncounterGrid_paginateSigned";
                    var ClassControlName = "DashBoard";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout(CreatePagination(response.VisitsCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        DashBoard.DashBoardEncounterSearchSigned(PageNumber, ResultPerPage, null);
                    }), 10);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    //#region Encounter search for Missing Info notes
    DashBoardEncounterSearchMissingInfo: function (PageNo, rpp, GridData) {
        $("#pnlDashboard #pnlEncounterGrid #ctrlMissingInfo #chkMissingInfo").prop("checked", false);
        var objDeffered = $.Deferred();
        DashBoard.pageNoMissingInfo = PageNo;
        //Start By Khaleel Ur Rehman to Validate From and To Date , on 28 january 2016.
        utility.ValidateFromToDate('frmDashboard', 'dpVisitFrom', 'dpVisitTo', true);
        //End By Khaleel Ur Rehman to Validate From and To Date , on 28 january 2016.
        DashBoard.EnableDisableControl();
        if ($("#pnlDashboard #pnlEncounterGrid").css("display") == "none")
            $("#pnlDashboard #pnlEncounterGrid").show();
        var myJSON = new Object();

        myJSON.txtProvider = null;
        myJSON.providerId = null;
        myJSON.txtFacility = null;
        myJSON.dtpAppointmentDateFrom = $.datepicker.formatDate('mm/dd/yy', new Date());
        myJSON.dtpAppointmentDateTo = $.datepicker.formatDate('mm/dd/yy', new Date());
        myJSON.txtClaimNumber = "";
        myJSON.ddlActive = "";
        var myString = JSON.stringify(myJSON);
        if (GridData) {
            $.when(DashBoard.DashBoardEncounterGridLoadMissingInfo(GridData, PageNo, rpp)).then(function () {
                objDeffered.resolve("ok");
            });
            var TableControl = "pnlDashboard #pnlEncounterGrid #ctrlMissingInfo #dgvEncounterGridMissingInfo";
            var PagingPanelControlID = "pnlDashboard #pnlEncounterGrid #ctrlMissingInfo #dgvEncounterGrid_paginateMissingInfo";
            var ClassControlName = "DashBoard";
            var PagesToDisplay = 5;
            var iTotalDisplayRecords = GridData.iTotalDisplayRecords;
            setTimeout(CreatePagination(GridData.VisitsCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                DashBoard.DashBoardEncounterSearchMissingInfo(PageNumber, ResultPerPage, null);
            }), 10);
        }
        else {
            DashBoard.SearchVisits(PageNo, rpp, 'Draft', 'ctrlMissingInfo').done(function (response) {
                if (response.status != false) {
                    $.when(DashBoard.DashBoardEncounterGridLoadMissingInfo(response, PageNo, rpp)).then(function () {
                        objDeffered.resolve("ok");
                    });
                    var TableControl = "pnlDashboard #pnlEncounterGrid #ctrlMissingInfo #dgvEncounterGridMissingInfo";
                    var PagingPanelControlID = "pnlDashboard #pnlEncounterGrid #ctrlMissingInfo #dgvEncounterGrid_paginateMissingInfo";
                    var ClassControlName = "DashBoard";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout(CreatePagination(response.VisitsCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        DashBoard.DashBoardEncounterSearchMissingInfo(PageNumber, ResultPerPage, null);
                    }), 10);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        return objDeffered.promise();
    },

    DashBoardEncounterGridLoad: function (response, PageNo, rpp, innerCtrl) {
        var objDeffered = $.Deferred();
        var dpVisitFromValue = $('#pnlDashboard #pnlEncounterGrid #ctrlPanReadyToSign #dpVisitFrom').val();
        var dpVisitToValue = $('#pnlDashboard #pnlEncounterGrid #ctrlPanReadyToSign #dpVisitTo').val();
        var providerId = $("#pnlDashboard #ctrlPanReadyToSign #hfProviderId").val()
        var providerText = $("#pnlDashboard #pnlEncounterGrid #ctrlPanReadyToSign #txtProvider").val();
        var patientId = $("#pnlDashboard #ctrlPanReadyToSign #hfPatientId").val();
        var patientText = $("#pnlDashboard #pnlEncounterGrid #ctrlPanReadyToSign #txtFullName").val();
        var ddlNoteTypeValue = $('#pnlDashboard #pnlEncounterGrid #ctrlPanReadyToSign #ddlNoteType').val();
        $("#pnlDashboard #pnlEncounterGrid #ctrlPanReadyToSign #dgvEncounterGrid").dataTable().fnDestroy();
        $("#pnlDashboard #pnlEncounterGrid #ctrlPanReadyToSign #dgvEncounterGrid tbody").find("tr").remove();
        if (response.VisitsCount == 0) {
            $('#pnlDashboard div.wEncounter .badge').css("display", "none");
            $('#ctrlPanReadyToSign #spnNotesCount').css("display", "none");
        } else {
            $('#pnlDashboard div.wEncounter .badge').css("display", "inline");
            $('#ctrlPanReadyToSign #spnNotesCount').css("display", "inline");
            $('#wpanel .slick-track div').each(function (i) {
                if ($(this).find('span:first').text() == 'Notes') {
                    $(this).find('span:last').text(response.iTotalDisplayRecords);
                    $(this).find('span:last').show();
                }
            });
            $('#pnlDashboard div.wEncounter .badge').text(response.iTotalDisplayRecords);
            $('#ctrlPanReadyToSign #spnNotesCount').text(response.iTotalDisplayRecords);
        }

        Clinical_Notes.canSignNote().done(function (canSign) {
            if (response.VisitsCount > 0) {
                var EncounterSearchJSONData = JSON.parse(response.VisitsLoad_JSON);
                $.each(EncounterSearchJSONData, function (i, item) {
                    var $row = $('<tr/>');
                    $row.attr("id", "gvDashBoard_Encounter_row" + item.NotesId + "");
                    var NoteDeleteMethod = "DashBoard.NotesDelete('" + item.NotesId + "',event,1);";
                    var DemographicsMethod = "utility.PatientDemographics('" + item.PatientId + "', 'mstrTabDashBoard' ,event);";
                    var NotesPreview = "DashBoard.NotesPreview('" + item.NotesId + "','" + item.PatientId + "','" + item.ProviderId + "','" + "" + "',event,'" + utility.RemoveTimeFromDate(null, item.VisitDate) + "','" + item.BillingInfoId + "','" + item.AppointmentDate + "'," + item.VisitId + ",'" + item.NoteDate + "','" + item.PatientTypeId + "', '" + item.FacilityId + "', '" + item.POS + "'," + (!item.RefProviderId ? null : +item.RefProviderId) + "," + (item.IsPhoneEncounter == "False" ? false : true) + ",'" + item.BillingStatus + "');";
                    var isVisible = 'style="display:none;';
                    if (response.HasDeleteRights != "No") {
                        isVisible = "";
                    }
                    var innerCtrl = "ctrlPanReadyToSign";
                    var allCheckBox = "chkReadytosign";
                    var allInnerCheckBox = "SelectCheckBoxReadytoSign";
                    var EditProgressNoteMethod = "DashBoard.EditProgressNote('" + item.NotesId + "', '" + item.PatientId + "', event" + ");";
                    var signNoteCheckbox = '<input type="checkbox" class="input-block" data-notesid="' + item.NotesId + '" data-patientid="' + item.PatientId + '" coltype="checkbox"   name="SelectCheckBoxReadytoSign" onclick="DashBoard.CheckUncheckThisNote(' + item.NotesId + ',event,\'' + innerCtrl + '\',\'' + allCheckBox + '\',\'' + allInnerCheckBox + '\')" />';
                    if (canSign) {
                        $row.append('<td><a class="btn btn-xs" role="button" onclick="DashBoard.OnClickPreventDefault(event);" title="Select note to sign">' + signNoteCheckbox + '</a> &nbsp;<a class="btn btn-xs" href="#" onclick="' + NoteDeleteMethod + '" title="Delete Note"> <i class="fa fa-close red"></i></a>&nbsp; <a class="btn  btn-xs" href="#"  onclick="' + NotesPreview + '" title="Note Preview"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Audit Log" onclick="DashBoard.rowHistory(' + item.NotesId + ',\'' + utility.RemoveTimeFromDate(null, item.VisitDate) + '\',\'' + item.AppReason + '\',\'' + item.NoteType + '\',event);"> <i class="fa fa-history blue"></i></a>  </td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.AccountNumber + '</a></td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.PatientName + '</a></td><td>' + item.ProviderName + '</td><td>' + item.FacilityName + '</td><td>' + utility.RemoveTimeFromDate(null, item.VisitDate) + '</td><td>' + item.AppReason + '</td><td>' + item.NoteType + '</td><td>' + item.NoteStatus + '<span data-toggle="tooltip" data-placement="top" title class="rightListMargin ReadyToSignWarning"></span></td>');
                    }
                    else {
                        $row.append('<td><a class="btn btn-xs" role="button" onclick="' + NoteDeleteMethod + '" title="Delete Note"> <i class="fa fa-close red"></i></a>&nbsp; <a class="btn  btn-xs" href="#"  onclick="' + NotesPreview + '" title="Note Preview"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Audit Log" onclick="DashBoard.rowHistory(' + item.NotesId + ',\'' + utility.RemoveTimeFromDate(null, item.VisitDate) + '\',\'' + item.AppReason + '\',\'' + item.NoteType + '\',event);"> <i class="fa fa-history blue"></i></a>  </td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.AccountNumber + '</a></td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.PatientName + '</a></td><td>' + item.ProviderName + '</td><td>' + item.FacilityName + '</td><td>' + utility.RemoveTimeFromDate(null, item.VisitDate) + '</td><td>' + item.AppReason + '</td><td>' + item.NoteType + '</td><td>' + item.NoteStatus + '<span data-toggle="tooltip" data-placement="top" class="rightListMargin ReadyToSignWarning" title></span></td>');
                    }
                    $row.attr("onClick", item.NoteStatus.toUpperCase() == "SIGNED" ? NotesPreview : EditProgressNoteMethod);

                    $("#pnlDashboard #ctrlPanReadyToSign #dgvEncounterGrid tbody").last().append($row);
                });
                objDeffered.resolve("ok");
            }
            else {
                $('#pnlDashboard #ctrlPanReadyToSign #dgvEncounterGrid').DataTable({
                    "language": {
                        "emptyTable": "No Record Found"
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }], "searching": false
                });
                objDeffered.resolve("ok");
            }
            if ($.fn.dataTable.isDataTable('#pnlDashboard #ctrlPanReadyToSign #dgvEncounterGrid'))
                ;
            else
                $("#pnlDashboard #ctrlPanReadyToSign #dgvEncounterGrid").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }], searching: false }); // to remove records per page dropdown
            $('#pnlDashboard #pnlEncounterGrid #ctrlPanReadyToSign #dpVisitFrom').val(dpVisitFromValue);
            $('#pnlDashboard #pnlEncounterGrid #ctrlPanReadyToSign #dpVisitTo').val(dpVisitToValue);
            $('#pnlDashboard #pnlEncounterGrid #ctrlPanReadyToSign #ddlNoteType').val(ddlNoteTypeValue);
            $("#pnlDashboard #ctrlPanReadyToSign #hfProviderId").val(providerId)
            $("#pnlDashboard #pnlEncounterGrid #ctrlPanReadyToSign #txtProvider").val(providerText);
            $("#pnlDashboard #ctrlPanReadyToSign #hfPatientId").val(patientId);
            $("#pnlDashboard #pnlEncounterGrid #ctrlPanReadyToSign #txtFullName").val(patientText);
            if (dpVisitFromValue != '') {
                $('#pnlDashboard #ctrlPanReadyToSign #dpVisitTo').removeAttr('disabled');
            } else {
                $('#pnlDashboard #ctrlPanReadyToSign #dpVisitTo').attr('disabled', true); $('#pnlDashboard #ctrlPanReadyToSign #dpVisitTo').val('');
            }
            DashBoard.documentReady(false);
            $('#pnlDashboard #ctrlPanReadyToSign #dgvEncounterGrid_wrapper .datatables-header div.dataTables_filter').parent().removeClass('col-sm-12 col-md-6');
            $('#pnlDashboard #ctrlPanReadyToSign #dgvEncounterGrid_wrapper .datatables-header div:first').removeClass('col-sm-12 col-md-6');

            if (!$('#pnlDashboard #ctrlPanReadyToSign #dgvEncounterGrid_wrapper .datatables-header div.dataTables_filter').parent().hasClass('col-sm-6 col-md-5')) {
                $('#pnlDashboard #ctrlPanReadyToSign #dgvEncounterGrid_wrapper .datatables-header div.dataTables_filter').parent().addClass('col-sm-6 col-md-5');
            }
            if (!$('#pnlDashboard #ctrlPanReadyToSign #dgvEncounterGrid_wrapper .datatables-header div.dataTables_filter').parent().hasClass('mt-md')) {
                $('#pnlDashboard #ctrlPanReadyToSign #dgvEncounterGrid_wrapper .datatables-header div.dataTables_filter').addClass('mt-md');
            }
            if (!$('#pnlDashboard #ctrlPanReadyToSign #dgvEncounterGrid_wrapper .datatables-header div:first').hasClass('col-sm-6 col-md-9')) {
                $('#pnlDashboard #ctrlPanReadyToSign #dgvEncounterGrid_wrapper .datatables-header div:first').addClass('col-sm-12 col-md-12');
            }
            //Start 16-10-2017 Humaira Yousaf IMP-1197
            if ($('#pnlDashboard #ctrlPanReadyToSign #dgvEncounterGrid_wrapper .datatables-header').hasClass('form-inline')) {
                $('#pnlDashboard #ctrlPanReadyToSign #dgvEncounterGrid_wrapper .datatables-header').removeClass('form-inline')
            }
            //End 16-10-2017 Humaira Yousaf IMP-1197
        });
        return objDeffered;
    },

    DashBoardEncounterGridLoadSigned: function (response, PageNo, rpp) {
        var dpVisitFromValue = $('#pnlDashboard #pnlEncounterGrid #ctrlPanSigned #dpVisitFrom').val();
        var dpVisitToValue = $('#pnlDashboard #pnlEncounterGrid #ctrlPanSigned #dpVisitTo').val();
        var providerId = $("#pnlDashboard #ctrlPanSigned #hfProviderId").val()
        var providerText = $("#pnlDashboard #pnlEncounterGrid #ctrlPanSigned #txtProvider").val();
        var patientId = $("#pnlDashboard #ctrlPanSigned #hfPatientId").val();
        var patientText = $("#pnlDashboard #pnlEncounterGrid #ctrlPanSigned #txtFullName").val();
        var ddlNoteTypeValue = $('#pnlDashboard #pnlEncounterGrid #ctrlPanSigned #ddlNoteType').val();
        $("#pnlDashboard #pnlEncounterGrid #ctrlPanSigned #dgvEncounterGridSigned").dataTable().fnDestroy();
        $("#pnlDashboard #pnlEncounterGrid #ctrlPanSigned #dgvEncounterGridSigned tbody").find("tr").remove();

        if (response.VisitsCount == 0) {
            $('#pnlDashboard div.wEncounter .badge').css("display", "none");
            $('#ctrlPanSigned #spnNotesCount').css("display", "none");
        } else {
            $('#pnlDashboard #ctrlPanSigned div.wEncounter .badge').css("display", "inline");
            $('#ctrlPanSigned #spnNotesCount').css("display", "inline");
            $('#wpanel .slick-track div').each(function (i) {
                if ($(this).find('span:first').text() == 'Notes') {
                    $(this).find('span:last').text(response.iTotalDisplayRecords);
                    $(this).find('span:last').show();
                }
            });
            $('#pnlDashboard div.wEncounter .badge').text(response.iTotalDisplayRecords);
            $('#ctrlPanSigned #spnNotesCount').text(response.iTotalDisplayRecords);
        }
        Clinical_Notes.canSignNote().done(function (canSign) {
            if (response.VisitsCount > 0) {
                var EncounterSearchJSONData = JSON.parse(response.VisitsLoad_JSON);
                $.each(EncounterSearchJSONData, function (i, item) {
                    var $row = $('<tr/>');
                    $row.attr("id", "gvDashBoard_Encounter_row" + i + "");

                    var ClassDisabled = item.NoteStatus.toUpperCase() == "SIGNED" ? "disabled" : "";

                    var DemographicsMethod = "utility.PatientDemographics('" + item.PatientId + "', 'mstrTabDashBoard' ,event);";

                    var NotesPreview = "DashBoard.NotesPreview('" + item.NotesId + "','" + item.PatientId + "','" + item.ProviderId + "','" + "" + "',event,'" + utility.RemoveTimeFromDate(null, item.VisitDate) + "','" + item.BillingInfoId + "','" + item.AppointmentDate + "'," + item.VisitId + ",'" + item.NoteDate + "','" + item.PatientTypeId + "', '" + item.FacilityId + "', '" + item.POS + "'," + (!item.RefProviderId ? null : +item.RefProviderId) + "," + (item.IsPhoneEncounter == "False" ? false : true) + ",'" + item.BillingStatus + "');";
                    if (canSign) {
                        $row.append('<td><a class="btn  btn-xs" href="#"  onclick="' + NotesPreview + '" title="Note Preview"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Audit Log" onclick="DashBoard.rowHistory(' + item.NotesId + ',\'' + utility.RemoveTimeFromDate(null, item.VisitDate) + '\',\'' + item.AppReason + '\',\'' + item.NoteType + '\',event);"> <i class="fa fa-history blue"></i></a>  </td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.AccountNumber + '</a></td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.PatientName + '</a></td><td>' + item.ProviderName + '</td><td>' + item.FacilityName + '</td><td>' + utility.RemoveTimeFromDate(null, item.VisitDate) + '</td><td>' + item.AppReason + '</td><td>' + item.NoteType + '</td><td>' + item.NoteStatus + '</td>');
                    }
                    else {
                        $row.append('<td><a class="btn  btn-xs" href="#"  onclick="' + NotesPreview + '" title="Note Preview"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Audit Log" onclick="DashBoard.rowHistory(' + item.NotesId + ',\'' + utility.RemoveTimeFromDate(null, item.VisitDate) + '\',\'' + item.AppReason + '\',\'' + item.NoteType + '\',event);"> <i class="fa fa-history blue"></i></a>  </td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.AccountNumber + '</a></td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.PatientName + '</a></td><td>' + item.ProviderName + '</td><td>' + item.FacilityName + '</td><td>' + utility.RemoveTimeFromDate(null, item.VisitDate) + '</td><td>' + item.AppReason + '</td><td>' + item.NoteType + '</td><td>' + item.NoteStatus + '</td>');
                    }
                    $row.attr("onClick", NotesPreview);
                    $("#pnlDashboard #ctrlPanSigned #dgvEncounterGridSigned tbody").last().append($row);
                });
            }
            else {
                $('#pnlDashboard #ctrlPanSigned #dgvEncounterGridSigned').DataTable({
                    "language": {
                        "emptyTable": "No Record Found"
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{
                        "bSortable": false, "aTargets": [0]
                    }], searching: false
                });
            }
            if ($.fn.dataTable.isDataTable('#pnlDashboard #ctrlPanSigned #dgvEncounterGridSigned'))
                ;
            else
                $("#pnlDashboard #ctrlPanSigned #dgvEncounterGridSigned").DataTable({
                    "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{
                        "bSortable": false, "aTargets": [0]
                    }], searching: false
                }); // to remove records per page dropdown
            $('#pnlDashboard #pnlEncounterGrid #ctrlPanSigned #dpVisitFrom').val(dpVisitFromValue);
            $('#pnlDashboard #pnlEncounterGrid #ctrlPanSigned #dpVisitTo').val(dpVisitToValue);
            $('#pnlDashboard #pnlEncounterGrid #ctrlPanSigned #ddlNoteType').val(ddlNoteTypeValue);
            $("#pnlDashboard #ctrlPanSigned #hfProviderId").val(providerId)
            $("#pnlDashboard #pnlEncounterGrid #ctrlPanSigned #txtProvider").val(providerText);
            $("#pnlDashboard #ctrlPanSigned #hfPatientId").val(patientId);
            $("#pnlDashboard #pnlEncounterGrid #ctrlPanSigned #txtFullName").val(patientText);
            if (dpVisitFromValue != '') {
                $('#pnlDashboard #ctrlPanSigned #dpVisitTo').removeAttr('disabled');
            } else {
                $('#pnlDashboard #ctrlPanSigned #dpVisitTo').attr('disabled', true); $('#pnlDashboard #ctrlPanSigned #dpVisitTo').val('');
            }
            DashBoard.documentReady(false);
            $('#pnlDashboard #ctrlPanSigned #dgvEncounterGrid_wrapper .datatables-header div.dataTables_filter').parent().removeClass('col-sm-12 col-md-6');
            $('#pnlDashboard #ctrlPanSigned #dgvEncounterGrid_wrapper .datatables-header div:first').removeClass('col-sm-12 col-md-6');
            if (!$('#pnlDashboard #ctrlPanSigned #dgvEncounterGrid_wrapper .datatables-header div.dataTables_filter').parent().hasClass('col-sm-6 col-md-5')) {
                $('#pnlDashboar #ctrlPanSignedd #dgvEncounterGrid_wrapper .datatables-header div.dataTables_filter').parent().addClass('col-sm-6 col-md-5');
            }
            if (!$('#pnlDashboard #ctrlPanSigned #dgvEncounterGrid_wrapper .datatables-header div.dataTables_filter').parent().hasClass('mt-md')) {
                $('#pnlDashboard #ctrlPanSigned #dgvEncounterGrid_wrapper .datatables-header div.dataTables_filter').addClass('mt-md');
            }
            if (!$('#pnlDashboard #ctrlPanSigned #dgvEncounterGrid_wrapper .datatables-header div:first').hasClass('col-sm-6 col-md-9')) {
                $('#pnlDashboard #ctrlPanSigned #dgvEncounterGrid_wrapper .datatables-header div:first').addClass('col-sm-12 col-md-12');
            }
            //Start 16-10-2017 Humaira Yousaf IMP-1197
            if ($('#pnlDashboard #ctrlPanSigned #dgvEncounterGrid_wrapper .datatables-header').hasClass('form-inline')) {
                $('#pnlDashboard #ctrlPanSigned #dgvEncounterGrid_wrapper .datatables-header').removeClass('form-inline')
            }
            //End 16-10-2017 Humaira Yousaf IMP-1197
        });
    },

    DashBoardEncounterGridLoadMissingInfo: function (response, PageNo, rpp) {
        var objDeffered = $.Deferred();
        var dpVisitFromValue = $('#pnlDashboard #pnlEncounterGrid #ctrlMissingInfo #dpVisitFrom').val();
        var dpVisitToValue = $('#pnlDashboard #pnlEncounterGrid #ctrlMissingInfo #dpVisitTo').val();
        var providerId = $("#pnlDashboard #ctrlMissingInfo #hfProviderId").val()
        var providerText = $("#pnlDashboard #pnlEncounterGrid #ctrlMissingInfo #txtProvider").val();
        var patientId = $("#pnlDashboard #ctrlMissingInfo #hfPatientId").val();
        var patientText = $("#pnlDashboard #pnlEncounterGrid #ctrlMissingInfo #txtFullName").val();
        var ddlNoteTypeValue = $('#pnlDashboard #pnlEncounterGrid #ctrlMissingInfo #ddlNoteType').val();
        $("#pnlDashboard #pnlEncounterGrid #ctrlMissingInfo #dgvEncounterGridMissingInfo").dataTable().fnDestroy();
        $("#pnlDashboard #pnlEncounterGrid #ctrlMissingInfo #dgvEncounterGridMissingInfo tbody").find("tr").remove();

        if (response.VisitsCount == 0) {
            $('#pnlDashboard div.wEncounter .badge').css("display", "none");
            $('#ctrlMissingInfo #spnNotesCount').css("display", "none");
        } else {
            $('#pnlDashboard div.wEncounter .badge').css("display", "inline");
            $('#ctrlMissingInfo #spnNotesCount').css("display", "inline");
            $('#wpanel .slick-track div').each(function (i) {
                if ($(this).find('span:first').text() == 'Notes') {
                    $(this).find('span:last').text(response.iTotalDisplayRecords);
                    $(this).find('span:last').show();
                }
            });
            $('#pnlDashboard div.wEncounter .badge').text(response.iTotalDisplayRecords);
            $('#ctrlMissingInfo #spnNotesCount').text(response.iTotalDisplayRecords);
        }
        Clinical_Notes.canSignNote().done(function (canSign) {
            if (response.VisitsCount > 0) {
                DashBoard.CDSIds = [];
                DashBoard.MissingInfoMarkAsReady = [];
                var EncounterSearchJSONData = JSON.parse(response.VisitsLoad_JSON);
                $.each(EncounterSearchJSONData, function (i, item) {
                    var $row = $('<tr/>');
                    $row.attr("id", "gvDashBoard_Encounter_row" + item.NotesId + "");
                    var NoteDeleteMethod = "DashBoard.NotesDelete('" + item.NotesId + "',event,2);";
                    var DemographicsMethod = "utility.PatientDemographics('" + item.PatientId + "', 'mstrTabDashBoard' ,event);";
                    var NotesPreview = "DashBoard.NotesPreview('" + item.NotesId + "','" + item.PatientId + "','" + item.ProviderId + "','" + "" + "',event,'" + utility.RemoveTimeFromDate(null, item.VisitDate) + "','" + item.BillingInfoId + "','" + item.AppointmentDate + "'," + item.VisitId + ",'" + item.NoteDate + "','" + item.PatientTypeId + "', '" + item.FacilityId + "', '" + item.POS + "'," + (!item.RefProviderId ? null : +item.RefProviderId) + "," + (item.IsPhoneEncounter == "False" ? false : true) + ",'" + item.BillingStatus + "');";
                    var EditProgressNoteMethod = "DashBoard.EditProgressNote('" + item.NotesId + "', '" + item.PatientId + "', event" + ");";
                    var MissingLinks = "";
                    var icdTag = false;
                    var cptTag = false;
                    var objMissing = new Object();
                    objMissing.NotesId = item.NotesId;
                    if (item.MissingInfo.toLowerCase().indexOf("icd") > -1) {
                        MissingLinks = '<a href="#" onclick="DashBoard.EditProgressNoteICDCPT(event,' + item.NotesId + ',' + item.PatientId + ',' + item.VisitId + ');" title="Click to add ICD">ICD</a>';
                        objMissing.ICD = true;
                        icdTag = true;
                    } else {
                        objMissing.ICD = false;
                    }
                    if (item.MissingInfo.toLowerCase().indexOf("cpt") > -1) {
                        if (icdTag) {
                            MissingLinks += ', <a href="#" onclick="DashBoard.EditProgressNoteICDCPT(event,' + item.NotesId + ',' + item.PatientId + ',' + item.VisitId + ');" title="Click to add CPT">CPT</a>';
                        } else {
                            MissingLinks += ' <a href="#" onclick="DashBoard.EditProgressNoteICDCPT(event,' + item.NotesId + ',' + item.PatientId + ',' + item.VisitId + ');" title="Click to add CPT">CPT</a>';
                        }
                        objMissing.CPT = true;
                        cptTag = true;
                    } else {
                        objMissing.CPT = false;
                    }
                    if (item.MissingInfo.toLowerCase().indexOf("cds") > -1) {
                        var cdsids = new Object();
                        cdsids.CDSIds = item.CDSIds;
                        cdsids.NotesId = item.NotesId;
                        cdsids.PatientId = item.PatientId;
                        DashBoard.CDSIds.push(cdsids);
                        if (cptTag || icdTag) {
                            MissingLinks += ', <a href="#" onclick="DashBoard.openCDSAlert(event,' + item.NotesId + ',' + item.PatientId + ');" title="Click to open CDS">CDS Alerts</a>';
                        } else {
                            MissingLinks += ' <a href="#" onclick="DashBoard.openCDSAlert(event,' + item.NotesId + ',' + item.PatientId + ');" title="Click to open CDS">CDS Alerts</a>';
                        }
                        objMissing.CDS = true;
                    } else {
                        objMissing.CDS = false;
                    }
                    DashBoard.MissingInfoMarkAsReady.push(objMissing);
                    var innerCtrl = "ctrlMissingInfo";
                    var allCheckBox = "chkMissingInfo";
                    var allInnerCheckBox = "SelectCheckBoxMissingInfo";
                    var missingInfoCheckbox = '<input type="checkbox" class="input-block" data-notesid="' + item.NotesId + '" data-patientid="' + item.PatientId + '"  coltype="checkbox"   name="SelectCheckBoxMissingInfo" onclick="DashBoard.CheckUncheckThisNote(' + item.NotesId + ',event,\'' + innerCtrl + '\',\'' + allCheckBox + '\',\'' + allInnerCheckBox + '\')" />';
                    if (canSign) {
                        $row.append('<td><a class="btn btn-xs" role="button"  onclick="DashBoard.OnClickPreventDefault(event);" title="Select note to sign">' + missingInfoCheckbox + '</a><a class="btn btn-xs" href="#" onclick="' + NoteDeleteMethod + '" title="Delete Note"> <i class="fa fa-close red"></i></a>&nbsp;<a class="btn  btn-xs" href="#"  onclick="' + NotesPreview + '" title="Note Preview"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Audit Log" onclick="DashBoard.rowHistory(' + item.NotesId + ',\'' + utility.RemoveTimeFromDate(null, item.VisitDate) + '\',\'' + item.AppReason + '\',\'' + item.NoteType + '\',event);"> <i class="fa fa-history blue"></i></a>  </td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.AccountNumber + '</a></td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.PatientName + '</a></td><td>' + item.ProviderName + '</td><td>' + item.FacilityName + '</td><td>' + utility.RemoveTimeFromDate(null, item.VisitDate) + '</td><td>' + item.AppReason + '</td><td>' + item.NoteType + '</td><td>' + MissingLinks + '<span data-toggle="tooltip" data-placement="top" title class="rightListMargin MarkReadyToSignWarning"></span></td>');
                    }
                    else {
                        $row.append('<td><a class="btn btn-xs" role="button"  onclick="' + NoteDeleteMethod + '" title="Delete Note"> <i class="fa fa-close red"></i></a>&nbsp;<a class="btn  btn-xs" href="#"  onclick="' + NotesPreview + '" title="Note Preview"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Audit Log" onclick="DashBoard.rowHistory(' + item.NotesId + ',\'' + utility.RemoveTimeFromDate(null, item.VisitDate) + '\',\'' + item.AppReason + '\',\'' + item.NoteType + '\',event);"> <i class="fa fa-history blue"></i></a>  </td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.AccountNumber + '</a></td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.PatientName + '</a></td><td>' + item.ProviderName + '</td><td>' + item.FacilityName + '</td><td>' + utility.RemoveTimeFromDate(null, item.VisitDate) + '</td><td>' + item.AppReason + '</td><td>' + item.NoteType + '</td><td>' + item.NoteStatus + '<span data-toggle="tooltip" data-placement="top" title class="rightListMargin MarkReadyToSignWarning"></span></td>');
                    }
                    $row.attr("onClick", EditProgressNoteMethod);

                    $("#pnlDashboard #ctrlMissingInfo #dgvEncounterGridMissingInfo tbody").last().append($row);
                });
                objDeffered.resolve("ok");
            }
            else {
                $('#pnlDashboard #ctrlMissingInfo #dgvEncounterGridMissingInfo').DataTable({
                    "language": {
                        "emptyTable": "No Record Found"
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{
                        "bSortable": false, "aTargets": [0]
                    }], searching: false
                });
                objDeffered.resolve("ok");
            }

            if ($.fn.dataTable.isDataTable('#pnlDashboard #ctrlMissingInfo #dgvEncounterGridMissingInfo'))
                ;
            else
                $("#pnlDashboard #ctrlMissingInfo #dgvEncounterGridMissingInfo").DataTable({
                    "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{
                        "bSortable": false, "aTargets": [0]
                    }], searching: false
                }); // to remove records per page dropdown
            $('#pnlDashboard #pnlEncounterGrid #ctrlMissingInfo #dpVisitFrom').val(dpVisitFromValue);
            $('#pnlDashboard #pnlEncounterGrid #ctrlMissingInfo #dpVisitTo').val(dpVisitToValue);
            $('#pnlDashboard #pnlEncounterGrid #ctrlMissingInfo #ddlNoteType').val(ddlNoteTypeValue);
            $("#pnlDashboard #ctrlMissingInfo #hfProviderId").val(providerId)
            $("#pnlDashboard #pnlEncounterGrid #ctrlMissingInfo #txtProvider").val(providerText);
            $("#pnlDashboard #ctrlMissingInfo #hfPatientId").val(patientId);
            $("#pnlDashboard #pnlEncounterGrid #ctrlMissingInfo #txtFullName").val(patientText);
            if (dpVisitFromValue != '') {
                $('#pnlDashboard #ctrlMissingInfo #dpVisitTo').removeAttr('disabled');
            } else {
                $('#pnlDashboard #ctrlMissingInfo #dpVisitTo').attr('disabled', true); $('#pnlDashboard #ctrlMissingInfo #dpVisitTo').val('');
            }
            DashBoard.documentReady(false);
            $('#pnlDashboard #ctrlMissingInfo #dgvEncounterGrid_wrapper .datatables-header div.dataTables_filter').parent().removeClass('col-sm-12 col-md-6');
            $('#pnlDashboard #ctrlMissingInfo #dgvEncounterGrid_wrapper .datatables-header div:first').removeClass('col-sm-12 col-md-6');
            if (!$('#pnlDashboard #ctrlMissingInfo #dgvEncounterGrid_wrapper .datatables-header div.dataTables_filter').parent().hasClass('col-sm-6 col-md-5')) {
                $('#pnlDashboar #ctrlMissingInfo #dgvEncounterGrid_wrapper .datatables-header div.dataTables_filter').parent().addClass('col-sm-6 col-md-5');
            }
            if (!$('#pnlDashboard #ctrlMissingInfo #dgvEncounterGrid_wrapper .datatables-header div.dataTables_filter').parent().hasClass('mt-md')) {
                $('#pnlDashboard #ctrlMissingInfo #dgvEncounterGrid_wrapper .datatables-header div.dataTables_filter').addClass('mt-md');
            }
            if (!$('#pnlDashboard #ctrlMissingInfo #dgvEncounterGrid_wrapper .datatables-header div:first').hasClass('col-sm-6 col-md-9')) {
                $('#pnlDashboard #ctrlMissingInfo #dgvEncounterGrid_wrapper .datatables-header div:first').addClass('col-sm-12 col-md-12');
            }
            //Start 16-10-2017 Humaira Yousaf IMP-1197
            if ($('#pnlDashboard #ctrlMissingInfo #dgvEncounterGrid_wrapper .datatables-header').hasClass('form-inline')) {
                $('#pnlDashboard #ctrlMissingInfo #dgvEncounterGrid_wrapper .datatables-header').removeClass('form-inline')
            }
            //End 16-10-2017 Humaira Yousaf IMP-1197
        });
        return objDeffered.promise();
    },

    openCDSAlert: function (event, NotesId, PatientId) {
        if (event != null) {
            event.preventDefault();
            event.stopPropagation();
        }
        var params = [];
        params["PatientId"] = PatientId;

        params["FromAdmin"] = 0;
        params["isPopup"] = 1;
        var cdsids = DashBoard.CDSIds.filter(function (obj) {
            if (obj.NotesId == NotesId)
                return true;
        });
        params["CDSIds"] = cdsids[0].CDSIds;
        params["TriggerLocation"] = "FromBulkSign";
        LoadActionPan("Clinical_CDSAlert", params);

    },

    OnClickPreventDefault: function (event) {
        if (event != null) {
            event.stopPropagation();
        }

        var ChkCountResults = 0;

        $("#pnlDashboard  #LabResult #dgvLabOrderResult input[name='SelectCheckBoxResult']:checkbox").each(function () {
            if (this.checked) {
                ChkCountResults++;
            }
        });

        if (ChkCountResults == 15)
            $("#pnlDashboard  #LabResult #dgvLabOrderResult thead tr #selectResults").prop('checked', true);
        else
            $("#pnlDashboard  #LabResult #dgvLabOrderResult thead tr #selectResults").prop('checked', false);

        //
        var ChkCountOrders = 0;

        $("#pnlDashboard  #dgvLabOrderGrid #dgvLabOrderDashboard input[name='SelectCheckBoxOrder']:checkbox").each(function () {
            if (this.checked) {
                ChkCountOrders++;
            }
        });

        if (ChkCountOrders == 15)
            $("#pnlDashboard  #dgvLabOrderGrid #dgvLabOrderDashboard thead tr #selectOrders").prop('checked', true);
        else
            $("#pnlDashboard  #dgvLabOrderGrid #dgvLabOrderDashboard thead tr #selectOrders").prop('checked', false);

        //
        var ChkCountUnSolicited = 0;

        $("#pnlDashboard  #LabUnsolicited #dgvLabUnsoOrderResult input[name='SelectCheckBoxResult']:checkbox").each(function () {
            if (this.checked) {
                ChkCountUnSolicited++;
            }
        });

        if (ChkCountUnSolicited == 15)
            $("#pnlDashboard  #LabUnsolicited #dgvLabUnsoOrderResult thead tr #selectResults").prop('checked', true);
        else
            $("#pnlDashboard  #LabUnsolicited #dgvLabUnsoOrderResult thead tr #selectResults").prop('checked', false);

    },

    checkUncheckAllNotes: function (chkBox, innerCtrl, innerChkBox) {
        if ($(chkBox).is(':checked')) {
            $("#pnlDashboard #" + innerCtrl + " [name=" + innerChkBox + "]").prop("checked", true);
        } else {
            $("#pnlDashboard #" + innerCtrl + " [name=" + innerChkBox + "]").prop("checked", false);
        }
    },

    CheckUncheckThisNote: function (NotesId, event, innerCtrl, ctrl, innerChkBox) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        var bChecked = true;
        $("#pnlDashboard #" + innerCtrl + " [name=" + innerChkBox + "]").each(function () {
            if (!this.checked) {
                bChecked = false;
            }
        });
        if (bChecked) {
            $("#pnlDashboard #pnlEncounterGrid #" + innerCtrl + " #" + ctrl).prop("checked", true);
        } else {
            $("#pnlDashboard #pnlEncounterGrid #" + innerCtrl + " #" + ctrl).prop("checked", false);
        }
    },


    // Mark note to be ready for "Ready To Sign Tab"
    MarkReadyToSign: function () {
        var arrNotesIds = new Array();
        $("#pnlDashboard #ctrlMissingInfo [name='SelectCheckBoxMissingInfo']").each(function () {
            if (this.checked) {
                arrNotesIds.push($(this).attr("data-notesid"));
            }
        });
        if (arrNotesIds.length > 0) {
            $.when(Clinical_ProgressNote.readytosignNote_DBCallMultiple(arrNotesIds.join(','), true)).then(function (response) {
                response = JSON.parse(response);
                if (response.status) {
                    var result = response.NoteComponentModel;
                    var resultUnMarked = DashBoard.MissingInfoMarkAsReady;
                    if (result.length > 0) {
                        DashBoard.checkMarkedCount(result);
                        $.when(DashBoard.DashBoardEncounterSearchMissingInfo(DashBoard.pageNoMissingInfo)).then(function () {
                            if (result.length > 0) {
                                var counter = 0;
                                result.forEach(function (entry) {
                                    if ($("#gvDashBoard_Encounter_row" + entry.NotesId + " .MarkReadyToSignWarning").length > 0) {
                                        var img = '<img src="../../Content/images/warning.png" width="14" height="14"/>';
                                        var objMarkAsReady = DashBoard.MissingInfoMarkAsReady.filter(function (obj) {
                                            if (obj.NotesId == entry.NotesId)
                                                return true;
                                        });
                                        if (objMarkAsReady.length > 0) {
                                            var errMsg = "You do not have the privileges to mark as 'Ready to Sign' without a ";
                                            var bICD = false;
                                            var bCPT = false;
                                            var bCDS = false;
                                            var bResult = false;
                                            if (entry.ICD == false && objMarkAsReady[0].ICD == true) {
                                                bICD = true;
                                                bResult = true;
                                            }
                                            if (entry.CPT == false && objMarkAsReady[0].CPT == true) {
                                                bCPT = true;
                                                bResult = true;
                                            }
                                            if (entry.CDSAlerts == false && objMarkAsReady[0].CDS == true) {
                                                bCDS = true;
                                                bResult = true;
                                            }
                                            if (bICD) {
                                                errMsg += "ICD";
                                                if (bCPT && bCDS) {
                                                    errMsg += ", CPT and CDS Alert";
                                                }
                                                else if (bCPT) {
                                                    errMsg += " and CPT";
                                                }
                                                else if (bCDS) {
                                                    errMsg += " and CDS Alert";
                                                }
                                            }
                                            else if (bCPT && bCDS) {
                                                errMsg += "CPT and CDS Alert";
                                            }
                                            else if (bCPT) {
                                                errMsg += "CPT";
                                            }
                                            else if (bCDS) {
                                                errMsg += "CDS Alert";
                                            }
                                            if (bResult) {
                                                $("#gvDashBoard_Encounter_row" + entry.NotesId + " .MarkReadyToSignWarning").prop("title", errMsg);
                                                $("#gvDashBoard_Encounter_row" + entry.NotesId + " .MarkReadyToSignWarning").html(img);
                                            }
                                        }
                                    } else {
                                        counter += 1;
                                    }
                                });
                                if (counter > 0) {
                                    utility.DisplayMessages(counter + " note(s) have been successfully marked as ready to sign.", 1);
                                }
                            }
                        });
                    } else {
                        if (resultUnMarked.length > 0) {
                            resultUnMarked.forEach(function (entry) {
                                if ($("#gvDashBoard_Encounter_row" + entry.NotesId + " .MarkReadyToSignWarning").length > 0) {
                                    var img = '<img src="../../Content/images/warning.png" width="14" height="14"/>';
                                    var errMsg = "You do not have the privileges to mark as 'Ready to Sign' without a ";
                                    var bICD = false;
                                    var bCPT = false;
                                    var bCDS = false;
                                    var bResult = false;
                                    if (entry.ICD == true) {
                                        bICD = true;
                                        bResult = true;
                                    }
                                    if (entry.CPT == true) {
                                        bCPT = true;
                                        bResult = true;
                                    }
                                    if (entry.CDS == true) {
                                        bCDS = true;
                                        bResult = true;
                                    }
                                    if (bICD) {
                                        errMsg += "ICD";
                                        if (bCPT && bCDS) {
                                            errMsg += ", CPT and CDS Alert";
                                        }
                                        else if (bCPT) {
                                            errMsg += " and CPT";
                                        }
                                        else if (bCDS) {
                                            errMsg += " and CDS Alert";
                                        }
                                    }
                                    else if (bCPT && bCDS) {
                                        errMsg += "CPT and CDS Alert";
                                    }
                                    else if (bCPT) {
                                        errMsg += "CPT";
                                    }
                                    else if (bCDS) {
                                        errMsg += "CDS Alert";
                                    }
                                    if (bResult) {
                                        $("#gvDashBoard_Encounter_row" + entry.NotesId + " .MarkReadyToSignWarning").prop("title", errMsg);
                                        $("#gvDashBoard_Encounter_row" + entry.NotesId + " .MarkReadyToSignWarning").html(img);
                                    }
                                }
                            });
                        }
                    }
                } else {
                    utility.DisplayMessages(response.Message, 4);
                }
            });
        } else {
            utility.DisplayMessages("Please select note to mark as ready to sign", 2);
        }
    },

    checkMarkedCount: function (result) {
        var counter = 0;
        result.forEach(function (entry) {
            var objMarkAsReady = DashBoard.MissingInfoMarkAsReady.filter(function (obj) {
                if (obj.NotesId == entry.NotesId)
                    return true;
            });
            if (objMarkAsReady.length > 0) {
                var bResult = false;
                if (entry.ICD == false && objMarkAsReady[0].ICD == true) {
                    bResult = true;
                }
                if (entry.CPT == false && objMarkAsReady[0].CPT == true) {
                    bResult = true;
                }
                if (entry.CDSAlerts == false && objMarkAsReady[0].CDS == true) {
                    bResult = true;
                }
                if (bResult) {
                    counter += 1;
                }
            }
        });
        if (counter == 0) {
            if (DashBoard.pageNoMissingInfo != null && DashBoard.pageNoMissingInfo != "undefined" && DashBoard.pageNoMissingInfo > 1) {
                DashBoard.pageNoMissingInfo = DashBoard.pageNoMissingInfo - 1;
            }
        }
    },

    // Bulk Sign multiple Notes 
    BulkSignNotes: function () {
        var arrNotesIds = new Array();
        var patientIds = [];
        $("#pnlDashboard #ctrlPanReadyToSign [name='SelectCheckBoxReadytoSign']").each(function () {
            if (this.checked) {
                arrNotesIds.push($(this).attr("data-notesid"));
                patientIds.push($(this).attr("data-patientid"));
            }
        });
        if (arrNotesIds.length > 0) {
            var totalSignedCoubt = 0;
            DashBoard.bulkSignNotes = [];

            utility.myConfirm("If you sign the selected notes in bulk, you will not be able to achieve numerator for certain measures of PI,Quality and Improvement Activities. Do you still wish to continue?",
                function () {

                    utility.myConfirm("63", function () {

                        $.when(Clinical_ProgressNote.signNote_DBCallMultiple(arrNotesIds.join(','), true)).then(function (response) {
                            response = JSON.parse(response);
                            if (response.status) {
                                var result = response.NoteComponentModel.filter(function (obj) {
                                    if (obj.ErrorMessage == null || obj.ErrorMessage == "")
                                        return true;
                                }).map(function (obj) {
                                    return obj.NotesId;
                                });
                                var resultUnsigned = response.NoteComponentModel.filter(function (obj) {
                                    if (obj.ErrorMessage != null && obj.ErrorMessage != "")
                                        return true;
                                });
                                totalSignedCoubt = result.length;
                                if (result.length > 0) {
                                    $.when(DashBoard.SavePdfForBulkSign(result)).then(function () {
                                        utility.DisplayMessages(totalSignedCoubt + " note(s) have been successfully signed.", 1);
                                        Clinical_NotesSearch.SetNotesCount();

                                        if ($("#PatientProfile #hfPatientId").val() && patientIds.indexOf($("#PatientProfile #hfPatientId").val()) >= 0) {
                                            //Load MU3 Alerts
                                            utility.LoadMUAlerts($("#PatientProfile #hfPatientId").val(), true);
                                        }

                                        DashBoard.bulkSignNotes = [];
                                        if (totalSignedCoubt == 15) {
                                            if (DashBoard.pageNoReadyToSign != null && DashBoard.pageNoReadyToSign != "undefined" && DashBoard.pageNoReadyToSign > 1) {
                                                DashBoard.pageNoReadyToSign = DashBoard.pageNoReadyToSign - 1;
                                            }
                                        }
                                        $.when(DashBoard.DashBoardEncounterSearch(DashBoard.pageNoReadyToSign)).then(function () {
                                            $("#pnlDashboard #pnlEncounterGrid #ctrlPanReadyToSign #hfIsDraftNote").val("0");
                                            $("#pnlDashboard #pnlEncounterGrid #ctrlPanReadyToSign #chkReadytosign").prop("checked", false);
                                            var errMsg = "The note was not signed due to an unknown error. Please contact support!";
                                            if (resultUnsigned.length > 0) {
                                                resultUnsigned.forEach(function (entry) {
                                                    if ($("#gvDashBoard_Encounter_row" + entry.NotesId + " .ReadyToSignWarning").length > 0) {
                                                        var img = '<img src="../../Content/images/warning.png" width="14" height="14"/>';
                                                        $("#gvDashBoard_Encounter_row" + entry.NotesId + " .ReadyToSignWarning").prop("title", errMsg);
                                                        $("#gvDashBoard_Encounter_row" + entry.NotesId + " .ReadyToSignWarning").html(img);
                                                    }
                                                });
                                            }
                                        });
                                    });
                                } else {
                                    if (resultUnsigned.length > 0) {
                                        var errMsg = "The note was not signed due to an unknown error. Please contact support!";
                                        Clinical_NotesSearch.SetNotesCount();
                                        resultUnsigned.forEach(function (entry) {
                                            if ($("#gvDashBoard_Encounter_row" + entry.NotesId + " .ReadyToSignWarning").length > 0) {
                                                var img = '<img src="../../Content/images/warning.png" width="14" height="14"/>';
                                                $("#gvDashBoard_Encounter_row" + entry.NotesId + " .ReadyToSignWarning").prop("title", errMsg);
                                                $("#gvDashBoard_Encounter_row" + entry.NotesId + " .ReadyToSignWarning").html(img);
                                            }
                                        });
                                    }
                                }
                            } else {
                                utility.DisplayMessages(response.Message, 4);
                            }
                        });

                    }, function () {

                    }, "", "Save", "Cancel");

                }, function () {
                }, "Warning Alert");

        } else {
            utility.DisplayMessages("Please select note to sign", 2);
        }
    },

    SavePdfForBulkSign: function (arrNotesIds, PatientId) {
        var i = 0;
        var IsFromProgressNote = true;
        var IsPhoneEncounter = false;
        var dfdSave = $.Deferred();
        var dfdAll = $.Deferred();
        var VisitId;
        var BillingInfoId;
        var VisitDate;
        var ProviderId;
        var FacilityPOSCode;
        var ProviderFullName;
        var ResourceProviderId;
        var ResourceProviderName;
        var POS;
        var VisitReason;
        var VisitTime;
        var isNonBillable = false;
        if ($("#pnlDashboard #ctrlPanReadyToSign #ddlNoteType option:selected").val() == "") {
            IsPhoneEncounter = false;
        } else if ($("#pnlDashboard #ctrlPanReadyToSign #ddlNoteType option:selected").text() == "Phone Encounter") {
            IsPhoneEncounter = true;
        }
        Clinical_ProgressNote.signNoteget_DBCall(arrNotesIds.join(',')).done(function (response) {
            response = JSON.parse(response);
            if (response.status) {
                $('#BackgroundLoader').show();
                $("#signNotePrint").show();
                $("#signNotePrint").css("display", "inline");
                var dfd1 = $.Deferred();
                var Record1 = DashBoard.GetPdfDataBulkSign(response, arrNotesIds.shift());
                $.when(Clinical_ProgressNote.previewNoteBulkSign(Record1, Record1.NotesLoad_JSON[0].NotesId, Record1.NotesLoad_JSON[0].PatientId, Record1.NotesLoad_JSON[0].ProviderId, Record1.NotesLoad_JSON[0].VisitDate, Record1.NotesLoad_JSON[0].VisitId, Record1.NotesLoad_JSON[0].VisitTime, Record1.NotesLoad_JSON[0].BillingInfoId, "Signed", Record1.NotesLoad_JSON[0].VisitReason, IsPhoneEncounter)).then(function () {
                    dfd1.resolve("ok");
                });
                var dfd2 = $.Deferred();
                $.when(dfd1).then(function () {
                    if (arrNotesIds.length > 0) {
                        $('#BackgroundLoader').show();
                        Record1 = DashBoard.GetPdfDataBulkSign(response, arrNotesIds.shift());
                        $.when(Clinical_ProgressNote.previewNoteBulkSign(Record1, Record1.NotesLoad_JSON[0].NotesId, Record1.NotesLoad_JSON[0].PatientId, Record1.NotesLoad_JSON[0].ProviderId, Record1.NotesLoad_JSON[0].VisitDate, Record1.NotesLoad_JSON[0].VisitId, Record1.NotesLoad_JSON[0].VisitTime, Record1.NotesLoad_JSON[0].BillingInfoId, "Signed", Record1.NotesLoad_JSON[0].VisitReason, IsPhoneEncounter)).then(function () {
                            dfd2.resolve("ok");
                        });
                    } else {
                        dfd2.resolve("ok");
                    }
                });
                var dfd3 = $.Deferred();
                $.when(dfd2).then(function () {
                    if (arrNotesIds.length > 0) {
                        $('#BackgroundLoader').show();
                        Record1 = DashBoard.GetPdfDataBulkSign(response, arrNotesIds.shift());
                        $.when(Clinical_ProgressNote.previewNoteBulkSign(Record1, Record1.NotesLoad_JSON[0].NotesId, Record1.NotesLoad_JSON[0].PatientId, Record1.NotesLoad_JSON[0].ProviderId, Record1.NotesLoad_JSON[0].VisitDate, Record1.NotesLoad_JSON[0].VisitId, Record1.NotesLoad_JSON[0].VisitTime, Record1.NotesLoad_JSON[0].BillingInfoId, "Signed", Record1.NotesLoad_JSON[0].VisitReason, IsPhoneEncounter)).then(function () {
                            dfd3.resolve("ok");
                        });
                    } else {
                        dfd3.resolve("ok");
                    }
                });
                var dfd4 = $.Deferred();
                $.when(dfd3).then(function () {
                    if (arrNotesIds.length > 0) {
                        $('#BackgroundLoader').show();
                        Record1 = DashBoard.GetPdfDataBulkSign(response, arrNotesIds.shift());
                        $.when(Clinical_ProgressNote.previewNoteBulkSign(Record1, Record1.NotesLoad_JSON[0].NotesId, Record1.NotesLoad_JSON[0].PatientId, Record1.NotesLoad_JSON[0].ProviderId, Record1.NotesLoad_JSON[0].VisitDate, Record1.NotesLoad_JSON[0].VisitId, Record1.NotesLoad_JSON[0].VisitTime, Record1.NotesLoad_JSON[0].BillingInfoId, "Signed", Record1.NotesLoad_JSON[0].VisitReason, IsPhoneEncounter)).then(function () {
                            dfd4.resolve("ok");
                        });
                    } else {
                        dfd4.resolve("ok");
                    }
                });
                var dfd5 = $.Deferred();
                $.when(dfd4).then(function () {
                    if (arrNotesIds.length > 0) {
                        $('#BackgroundLoader').show();
                        Record1 = DashBoard.GetPdfDataBulkSign(response, arrNotesIds.shift());
                        $.when(Clinical_ProgressNote.previewNoteBulkSign(Record1, Record1.NotesLoad_JSON[0].NotesId, Record1.NotesLoad_JSON[0].PatientId, Record1.NotesLoad_JSON[0].ProviderId, Record1.NotesLoad_JSON[0].VisitDate, Record1.NotesLoad_JSON[0].VisitId, Record1.NotesLoad_JSON[0].VisitTime, Record1.NotesLoad_JSON[0].BillingInfoId, "Signed", Record1.NotesLoad_JSON[0].VisitReason, IsPhoneEncounter)).then(function () {
                            dfd5.resolve("ok");
                        });
                    } else {
                        dfd5.resolve("ok");
                    }
                });
                var dfd6 = $.Deferred();
                $.when(dfd5).then(function () {
                    if (arrNotesIds.length > 0) {
                        $('#BackgroundLoader').show();
                        Record1 = DashBoard.GetPdfDataBulkSign(response, arrNotesIds.shift());
                        $.when(Clinical_ProgressNote.previewNoteBulkSign(Record1, Record1.NotesLoad_JSON[0].NotesId, Record1.NotesLoad_JSON[0].PatientId, Record1.NotesLoad_JSON[0].ProviderId, Record1.NotesLoad_JSON[0].VisitDate, Record1.NotesLoad_JSON[0].VisitId, Record1.NotesLoad_JSON[0].VisitTime, Record1.NotesLoad_JSON[0].BillingInfoId, "Signed", Record1.NotesLoad_JSON[0].VisitReason, IsPhoneEncounter)).then(function () {
                            dfd6.resolve("ok");
                        });
                    } else {
                        dfd6.resolve("ok");
                    }
                });
                var dfd7 = $.Deferred();
                $.when(dfd6).then(function () {
                    if (arrNotesIds.length > 0) {
                        $('#BackgroundLoader').show();
                        Record1 = DashBoard.GetPdfDataBulkSign(response, arrNotesIds.shift());
                        $.when(Clinical_ProgressNote.previewNoteBulkSign(Record1, Record1.NotesLoad_JSON[0].NotesId, Record1.NotesLoad_JSON[0].PatientId, Record1.NotesLoad_JSON[0].ProviderId, Record1.NotesLoad_JSON[0].VisitDate, Record1.NotesLoad_JSON[0].VisitId, Record1.NotesLoad_JSON[0].VisitTime, Record1.NotesLoad_JSON[0].BillingInfoId, "Signed", Record1.NotesLoad_JSON[0].VisitReason, IsPhoneEncounter)).then(function () {
                            dfd7.resolve("ok");
                        });
                    } else {
                        dfd7.resolve("ok");
                    }
                });
                var dfd8 = $.Deferred();
                $.when(dfd7).then(function () {
                    if (arrNotesIds.length > 0) {
                        $('#BackgroundLoader').show();
                        Record1 = DashBoard.GetPdfDataBulkSign(response, arrNotesIds.shift());
                        $.when(Clinical_ProgressNote.previewNoteBulkSign(Record1, Record1.NotesLoad_JSON[0].NotesId, Record1.NotesLoad_JSON[0].PatientId, Record1.NotesLoad_JSON[0].ProviderId, Record1.NotesLoad_JSON[0].VisitDate, Record1.NotesLoad_JSON[0].VisitId, Record1.NotesLoad_JSON[0].VisitTime, Record1.NotesLoad_JSON[0].BillingInfoId, "Signed", Record1.NotesLoad_JSON[0].VisitReason, IsPhoneEncounter)).then(function () {
                            dfd8.resolve("ok");
                        });
                    } else {
                        dfd8.resolve("ok");
                    }
                });
                var dfd9 = $.Deferred();
                $.when(dfd8).then(function () {
                    if (arrNotesIds.length > 0) {
                        $('#BackgroundLoader').show();
                        Record1 = DashBoard.GetPdfDataBulkSign(response, arrNotesIds.shift());
                        $.when(Clinical_ProgressNote.previewNoteBulkSign(Record1, Record1.NotesLoad_JSON[0].NotesId, Record1.NotesLoad_JSON[0].PatientId, Record1.NotesLoad_JSON[0].ProviderId, Record1.NotesLoad_JSON[0].VisitDate, Record1.NotesLoad_JSON[0].VisitId, Record1.NotesLoad_JSON[0].VisitTime, Record1.NotesLoad_JSON[0].BillingInfoId, "Signed", Record1.NotesLoad_JSON[0].VisitReason, IsPhoneEncounter)).then(function () {
                            dfd9.resolve("ok");
                        });
                    } else {
                        dfd9.resolve("ok");
                    }
                });
                var dfd10 = $.Deferred();
                $.when(dfd9).then(function () {
                    if (arrNotesIds.length > 0) {
                        $('#BackgroundLoader').show();
                        Record1 = DashBoard.GetPdfDataBulkSign(response, arrNotesIds.shift());
                        $.when(Clinical_ProgressNote.previewNoteBulkSign(Record1, Record1.NotesLoad_JSON[0].NotesId, Record1.NotesLoad_JSON[0].PatientId, Record1.NotesLoad_JSON[0].ProviderId, Record1.NotesLoad_JSON[0].VisitDate, Record1.NotesLoad_JSON[0].VisitId, Record1.NotesLoad_JSON[0].VisitTime, Record1.NotesLoad_JSON[0].BillingInfoId, "Signed", Record1.NotesLoad_JSON[0].VisitReason, IsPhoneEncounter)).then(function () {
                            dfd10.resolve("ok");
                        });
                    } else {
                        dfd10.resolve("ok");
                    }
                });
                var dfd11 = $.Deferred();
                $.when(dfd10).then(function () {
                    if (arrNotesIds.length > 0) {
                        $('#BackgroundLoader').show();
                        Record1 = DashBoard.GetPdfDataBulkSign(response, arrNotesIds.shift());
                        $.when(Clinical_ProgressNote.previewNoteBulkSign(Record1, Record1.NotesLoad_JSON[0].NotesId, Record1.NotesLoad_JSON[0].PatientId, Record1.NotesLoad_JSON[0].ProviderId, Record1.NotesLoad_JSON[0].VisitDate, Record1.NotesLoad_JSON[0].VisitId, Record1.NotesLoad_JSON[0].VisitTime, Record1.NotesLoad_JSON[0].BillingInfoId, "Signed", Record1.NotesLoad_JSON[0].VisitReason, IsPhoneEncounter)).then(function () {
                            dfd11.resolve("ok");
                        });
                    } else {
                        dfd11.resolve("ok");
                    }
                });
                var dfd12 = $.Deferred();
                $.when(dfd11).then(function () {
                    if (arrNotesIds.length > 0) {
                        $('#BackgroundLoader').show();
                        Record1 = DashBoard.GetPdfDataBulkSign(response, arrNotesIds.shift());
                        $.when(Clinical_ProgressNote.previewNoteBulkSign(Record1, Record1.NotesLoad_JSON[0].NotesId, Record1.NotesLoad_JSON[0].PatientId, Record1.NotesLoad_JSON[0].ProviderId, Record1.NotesLoad_JSON[0].VisitDate, Record1.NotesLoad_JSON[0].VisitId, Record1.NotesLoad_JSON[0].VisitTime, Record1.NotesLoad_JSON[0].BillingInfoId, "Signed", Record1.NotesLoad_JSON[0].VisitReason, IsPhoneEncounter)).then(function () {
                            dfd12.resolve("ok");
                        });
                    } else {
                        dfd12.resolve("ok");
                    }
                });
                var dfd13 = $.Deferred();
                $.when(dfd12).then(function () {
                    if (arrNotesIds.length > 0) {
                        $('#BackgroundLoader').show();
                        Record1 = DashBoard.GetPdfDataBulkSign(response, arrNotesIds.shift());
                        $.when(Clinical_ProgressNote.previewNoteBulkSign(Record1, Record1.NotesLoad_JSON[0].NotesId, Record1.NotesLoad_JSON[0].PatientId, Record1.NotesLoad_JSON[0].ProviderId, Record1.NotesLoad_JSON[0].VisitDate, Record1.NotesLoad_JSON[0].VisitId, Record1.NotesLoad_JSON[0].VisitTime, Record1.NotesLoad_JSON[0].BillingInfoId, "Signed", Record1.NotesLoad_JSON[0].VisitReason, IsPhoneEncounter)).then(function () {
                            dfd13.resolve("ok");
                        });
                    } else {
                        dfd13.resolve("ok");
                    }
                });
                var dfd14 = $.Deferred();
                $.when(dfd13).then(function () {
                    if (arrNotesIds.length > 0) {
                        $('#BackgroundLoader').show();
                        Record1 = DashBoard.GetPdfDataBulkSign(response, arrNotesIds.shift());
                        $.when(Clinical_ProgressNote.previewNoteBulkSign(Record1, Record1.NotesLoad_JSON[0].NotesId, Record1.NotesLoad_JSON[0].PatientId, Record1.NotesLoad_JSON[0].ProviderId, Record1.NotesLoad_JSON[0].VisitDate, Record1.NotesLoad_JSON[0].VisitId, Record1.NotesLoad_JSON[0].VisitTime, Record1.NotesLoad_JSON[0].BillingInfoId, "Signed", Record1.NotesLoad_JSON[0].VisitReason, IsPhoneEncounter)).then(function () {
                            dfd14.resolve("ok");
                        });
                    } else {
                        dfd14.resolve("ok");
                    }
                });
                var dfd15 = $.Deferred();
                $.when(dfd14).then(function () {
                    if (arrNotesIds.length > 0) {
                        $('#BackgroundLoader').show();
                        Record1 = DashBoard.GetPdfDataBulkSign(response, arrNotesIds.shift());
                        $.when(Clinical_ProgressNote.previewNoteBulkSign(Record1, Record1.NotesLoad_JSON[0].NotesId, Record1.NotesLoad_JSON[0].PatientId, Record1.NotesLoad_JSON[0].ProviderId, Record1.NotesLoad_JSON[0].VisitDate, Record1.NotesLoad_JSON[0].VisitId, Record1.NotesLoad_JSON[0].VisitTime, Record1.NotesLoad_JSON[0].BillingInfoId, "Signed", Record1.NotesLoad_JSON[0].VisitReason, IsPhoneEncounter)).then(function () {
                            dfd15.resolve("ok");
                            dfdAll.resolve("ok");
                        });
                    } else {
                        dfd15.resolve("ok");
                        dfdAll.resolve("ok");
                    }
                });
                $.when(dfdAll).then(function () {
                    $('#BackgroundLoader').hide();
                    $.when(DashBoard.SaveImport()).then(function () {
                        dfdSave.resolve("ok");
                    });
                    $("#signNotePrint").hide();
                });
            }
            else {
                dfdSave.resolve();
                utility.DisplayMessages(response.Message, 4);
            }
        });
        return dfdSave.promise();
    },

    GetPdfDataBulkSign: function (response, NotesId) {
        var objResponse = Object();
        objResponse.ReportHeaderInfo = response.ReportHeaderInfo.filter(function (entry) {
            return entry.NotesId == NotesId;
        });
        objResponse.NotesLoad_JSON = response.NotesLoad_JSON.filter(function (entry) {
            return entry.NotesId == NotesId;
        });
        objResponse.NoteHeaderPatientData = response.NoteHeaderPatientData.filter(function (entry) {
            return entry.PatientId == objResponse.NotesLoad_JSON[0].PatientId;
        });
        objResponse.NoteHeaderProviderData = response.NoteHeaderProviderData.filter(function (entry) {
            return entry.ProviderId == objResponse.NotesLoad_JSON[0].ProviderId;
        });
        objResponse.NoteHeaderPracticeData = response.NoteHeaderPracticeData.filter(function (entry) {
            return entry.PracticeId == 1;
        });
        var temp = $.grep(response.NoteComponentListFill_JSON, function (item_) {
            return (item_.ComponentName != 'Order Sets')
        });
        objResponse.NoteComponentListFill_JSON = temp.filter(function (entry) {
            return entry.NotesId == NotesId;
        });
        return objResponse;
    },

    SaveImport: function () {
        var data = new FormData();
        var myJSON = JSON.stringify(DashBoard.bulkSignNotes);
        data.append("PatientDocumentData", myJSON);
        $("#signNotePrint").hide();
        return MDVisionService.fileService(data, "PATIENT_DOCUMENT", "SAVE_PATIENT_DOCUMENT_BULKSIGN");
    },

    DashBoardModifiedGridLoad: function (response, PageNo, rpp) {
        $("#pnlDashboard #pnlModifiedNoteGrid #dgvModifiedNoteGrid").dataTable().fnDestroy();
        $("#pnlDashboard #pnlModifiedNoteGrid #dgvModifiedNoteGrid tbody").find("tr").remove();
        if (response.VisitsCount > 0) {
            var ModifiedNoteSearchJSONData = JSON.parse(response.VisitsLoad_JSON);
            $.each(ModifiedNoteSearchJSONData, function (i, item) {
                var NotesPreview = "DashBoard.NotesPreview('" + item.NotesId + "', '" + item.PatientId + "', '" + item.ProviderId + "','ModifiedNote',event);"
                var $row = $('<tr onclick="' + NotesPreview + '"/>');
                $row.attr("id", "gvDashBoard_ModifiedNote_row" + i + "");
                var disabled = "";
                if ($("#pnlDashboard #ddlModifiedNoteStatusId option:selected").val() == "1") {
                    disabled = "disabled";
                }
                var CalimOpen = "DashBoard.LoadVisitDetail('" + item.VisitId + "', '" + item.PatientId + "',event);"
                var DemographicsMethod = "utility.PatientDemographics('" + item.PatientId + "', 'mstrTabDashBoard' ,event);";

                var Reviewed = "DashBoard.Reviewed('" + item.NotesId + "')";
                $row.append('<td><a class="btn  btn-xs" href="#"  onclick="' + NotesPreview + '" title="Note Preview"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a ' + disabled + ' class="btn  btn-xs" href="#"  onclick="' + Reviewed + '" title="Reviewed"> <i class="fa fa-check"></i></a>&nbsp;<a class="btn btn-xs " href="javascript:void(0)" title="Record History" onclick="DashBoard.ShowModifiedNoteHistory(' + item.NotesId + ',' + item.PatientId + ');"> <i class="fa fa-history blue"></i></a>  </td><td>' + item.AccountNumber + '</td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.PatientName + '</a></td><td>' + item.ProviderName + '</td><td>' + item.FacilityName + '</td><td>' + utility.RemoveTimeFromDate(null, item.VisitDate) + '</td><td>' + item.AppReason + '</td><td><a class="CalimOpen" href="#" onclick="' + CalimOpen + '"  title="Claim Number">' + item.ClaimNumber + '</a></td>');
                $("#pnlDashboard #dgvModifiedNoteGrid tbody").last().append($row);
            });
        }
        else {
            $('#pnlDashboard #dgvModifiedNoteGrid').DataTable({
                "language": {
                    "emptyTable": "No Modified Note Found"
                }, "searching": false, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#pnlDashboard #dgvModifiedNoteGrid'))
            ;
        else
            $("#pnlDashboard #dgvModifiedNoteGrid").DataTable({ "searching": false, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
        DashBoard.documentReady(false);
        $('#pnlDashboard #dgvModifiedNoteGrid_wrapper .datatables-header div.dataTables_filter').parent().removeClass('col-sm-12 col-md-6');
        $('#pnlDashboard #dgvModifiedNoteGrid_wrapper .datatables-header div:first').removeClass('col-sm-12 col-md-6');


        if (!$('#pnlDashboard #dgvModifiedNoteGrid_wrapper .datatables-header div.dataTables_filter').parent().hasClass('col-sm-6 col-md-5')) {
            $('#pnlDashboard #dgvModifiedNoteGrid_wrapper .datatables-header div.dataTables_filter').parent().addClass('col-sm-6 col-md-5');
        }
        if (!$('#pnlDashboard #dgvModifiedNoteGrid_wrapper .datatables-header div.dataTables_filter').parent().hasClass('mt-md')) {
            $('#pnlDashboard #dgvModifiedNoteGrid_wrapper .datatables-header div.dataTables_filter').addClass('mt-md');
        }
        if (!$('#pnlDashboard #dgvModifiedNoteGrid_wrapper .datatables-header div:first').hasClass('col-sm-6 col-md-9')) {
            $('#pnlDashboard #dgvModifiedNoteGrid_wrapper .datatables-header div:first').addClass('col-sm-6 col-md-7');
        }
    },

    ShowModifiedNoteHistory: function (NotesId, PatientId) {
        EMRUtility.showCurrentItemHistory('pnlDashboard', null, NotesId, "Modified Notes", null, 'mstrTabDashBoard', null, 'pnlDashboard');
    },
    Reviewed: function (NoteId) {
        utility.myConfirm('43', function () {
            DashBoard.ModifiedNoteReviewed_DBCALL(NoteId, 1).done(function (response) {
                if (response.status != false) {
                    DashBoard.DashBoardModifiedNotesSearch();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }, function () {
        },
                    '1'
                    );
    },

    ModifiedNoteReviewed_DBCALL: function (NoteId, IsReviewed) {
        var objData = {
        };
        objData["NotesID"] = NoteId;
        objData["IsReviewed"] = IsReviewed;
        objData["CommandType"] = "MODIFIED_NOTE_REVIEWED";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "UpdateDashBoard");
    },
    /// Author: ZeeshanAK
    /// Purpose:  Call for showing history of current item
    /// Date : April 22, 2016
    ShowHistory: function (NotesId) {
        EMRUtility.showCurrentItemHistory(DashBoard.params.PanelID, null, NotesId, "Notes", DashBoard.params.patientID, DashBoard.params.TabID, null);
    },

    /// Author: ZeeshanAK
    /// Purpose:  Row history
    /// Date : April 22, 2016
    rowHistory: function (NotesId, VisitDate, VisitReasonComments, NoteTempType, event) {
        if (event != null)
            event.stopPropagation();
        var currentNotesId = NotesId != null ? NotesId : -1;
        if (currentNotesId > 0) {
            Clinical_Notes.ShowHistory(currentNotesId, VisitDate, VisitReasonComments, NoteTempType);
        }
    },
    SearchVisits: function (PageNumber, RowsPerPage, status, innerCtrl) {

        var VisitFrom = "";
        var VisitTo = "";
        var Status = "";
        var providerId = "";
        var patientId = "";
        var noteType = "";

        if (innerCtrl == "ctrlMissingInfo") {
            VisitFrom = $("#pnlDashboard #" + innerCtrl + " #dpVisitFromM").val();
            VisitTo = $("#pnlDashboard #" + innerCtrl + " #dpVisitToM").val();
        } else if (innerCtrl == "ctrlPanSigned") {
            VisitFrom = $("#pnlDashboard #" + innerCtrl + " #dpVisitFromS").val();
            VisitTo = $("#pnlDashboard #" + innerCtrl + " #dpVisitToS").val();
        } else if (innerCtrl == "ctrlPanReadyToSign") {
            VisitFrom = $("#pnlDashboard #" + innerCtrl + " #dpVisitFromR").val();
            VisitTo = $("#pnlDashboard #" + innerCtrl + " #dpVisitToR").val();
        }
        providerId = $("#pnlDashboard #" + innerCtrl + " #hfProviderId").val() == "" || $("#pnlDashboard #" + innerCtrl + " #hfProviderId").val() == null ? null : $("#pnlDashboard #" + innerCtrl + " #hfProviderId").val();
        patientId = $("#pnlDashboard #" + innerCtrl + " #hfPatientId").val() == "" || $("#pnlDashboard #" + innerCtrl + " #hfPatientId").val() == null ? null : $("#pnlDashboard #" + innerCtrl + " #hfPatientId").val();
        Status = status;
        if ($("#pnlDashboard #" + innerCtrl + " #ddlNoteType option:selected").val() == "") {
            noteType = "";
        }
        else {
            noteType = $("#pnlDashboard #" + innerCtrl + " #ddlNoteType option:selected").text();
        }
        //End 16-10-2017 Humaira Yousaf IMP-1197
        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 15 : RowsPerPage;

        var objData = new JSON.constructor();

        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        objData["VisitFrom"] = VisitFrom;
        objData["VisitTo"] = VisitTo;
        objData["Status"] = Status;
        objData["IsDraftNote"] = false;
        objData["providerId"] = providerId;
        objData["patientId"] = patientId;
        objData["NoteType"] = noteType;

        if ($("#pnlDashboard #pnlEncounterGrid #" + innerCtrl + " #hfIsDraftNote").val() == "1") {
            objData["IsDraftNote"] = true;
        }
        if (innerCtrl == "ctrlPanReadyToSign") {
            objData["IsReadyOrMissing"] = 1;
        }

        if (innerCtrl == "ctrlPanSigned") {
            objData["CommandType"] = "SEARCH_VISITS_NOTE";
        } else {
            objData["CommandType"] = "search_visits_note_bulksign";
        }
        if ($("#pnlDashboard #" + innerCtrl + " #ddlMissingInfo").length > 0 && $("#pnlDashboard #" + innerCtrl + " #ddlMissingInfo  option:selected").val() != "") {
            objData["MissingInfo"] = $("#pnlDashboard #" + innerCtrl + " #ddlMissingInfo option:selected").text();
        }

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "SearchDashBoard");
    },

    SearchModifiedNote: function (PageNumber, RowsPerPage) {

        var VisitFrom = "";
        var VisitTo = "";
        var Status = "";

        VisitFrom = $("#pnlDashboard #dpModifiedNoteFrom").val();
        VisitTo = $("#pnlDashboard #dpModifiedNoteTo").val();
        ProviderId = $("#pnlDashboard #hfNoteProviderId").val();
        Status = $("#pnlDashboard #ddlModifiedNoteStatusId option:selected").val();

        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 15 : RowsPerPage;

        var objData = new JSON.constructor();

        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        objData["VisitFrom"] = VisitFrom;
        objData["VisitTo"] = VisitTo;
        objData["ProviderId"] = ProviderId;
        objData["Status"] = Status;


        objData["CommandType"] = "SEARCH_MODIFIED_NOTE";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "SearchDashBoard");
    },

    LoadNotesDraftCount: function () {
        var objData = new Object();
        objData["CommandType"] = "load_notes_draft_count";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "LoadDashBoard");
    },

    SearchDashBoardEncounter: function (PageNo, rpp, GridData) {
        if (globalAppdata.IsProviderBulkSign == "True") {
            $("#pnlDashboard #pnlEncounterGrid #ctrlPanReadyToSign #hfIsDraftNote").val("0");
            DashBoard.DashBoardEncounterSearch(PageNo, rpp, GridData);
            $("#pnlDashboard #pnlEncounterGrid #ctrlPanReadyToSign #chkReadytosign").prop("checked", false);
        } else {
            $("#pnlDashboard #pnlEncounterGridOld #OldhfIsDraftNote").val("0");
            DashBoard.DashBoardEncounterSearchOld(PageNo, rpp, GridData);
        }
    },

    SearchDashBoardEncounterDraft: function (PageNo, rpp, GridData) {
        $('#pnlDashboard #pnlEncounterGridOld #OldddlNoteType').val("");
        if (globalAppdata.IsProviderBulkSign != "True") {
            $("#pnlDashboard #pnlEncounterGridOld #OldhfIsDraftNote").val("1");
            $("#pnlDashboard #pnlEncounterGridOld #OlddpVisitFrom").val("");
            $('#pnlDashboard #pnlEncounterGridOld #OlddpVisitTo').attr('disabled', true);
            $('#pnlDashboard #pnlEncounterGridOld #OlddpVisitTo').val('');
            $('#pnlDashboard #pnlEncounterGridOld select[id=OldddlNoteStatus] option:eq(1)').prop('selected', 'selected');
            DashBoard.DashBoardEncounterSearchOld(PageNo, rpp, GridData);
        } else {
            $("#pnlDashboard #pnlEncounterGrid #hfIsDraftNote").val("1");
            $("#pnlDashboard #pnlEncounterGrid #dpVisitFrom").val("");
            $('#pnlDashboard #pnlEncounterGrid #dpVisitTo').attr('disabled', true);
            $('#pnlDashboard #pnlEncounterGrid #dpVisitTo').val('');
            DashBoard.DashBoardEncounterSearch(PageNo, rpp, GridData);
        }
    },

    SearchDashBoardModifiedNotesSearch: function (PageNo, rpp, GridData) {
        DashBoard.DashBoardModifiedNotesSearch(PageNo, rpp, GridData);
    },


    DashBoardModifiedNotesSearch: function (PageNo, rpp, GridData) {
        //Start By Khaleel Ur Rehman to Validate From and To Date , on 28 january 2016.
        utility.ValidateFromToDate('dpModifiedNoteFrom', 'dpVisitFrom', 'dpModifiedNoteTo', true);
        //End By Khaleel Ur Rehman to Validate From and To Date , on 28 january 2016.
        DashBoard.EnableDisableControl();
        if ($("#pnlDashboard #pnlModifiedNoteGrid").css("display") == "none")
            $("#pnlDashboard #pnlModifiedNoteGrid").show();
        var myJSON = new Object();

        myJSON.txtProvider = null;
        myJSON.txtFacility = null;
        myJSON.dtpAppointmentDateFrom = $.datepicker.formatDate('mm/dd/yy', new Date());
        myJSON.dtpAppointmentDateTo = $.datepicker.formatDate('mm/dd/yy', new Date());
        myJSON.txtClaimNumber = "";
        myJSON.ddlActive = "";
        var myString = JSON.stringify(myJSON);
        if (GridData) {

            DashBoard.DashBoardModifiedGridLoad(GridData, PageNo, rpp);
            var TableControl = "pnlDashboard #pnlModifiedNoteGrid #dgvModifiedNoteGrid";
            var PagingPanelControlID = "pnlDashboard #pnlModifiedNoteGrid #dgvModifiedNoteGrid_paginate";
            var ClassControlName = "DashBoard";
            var PagesToDisplay = 5;
            var iTotalDisplayRecords = GridData.iTotalDisplayRecords;
            setTimeout(CreatePagination(GridData.VisitsCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                DashBoard.DashBoardModifiedNotesSearch(PageNumber, ResultPerPage, null);
            }), 10);
        }
        else {
            DashBoard.SearchModifiedNote(PageNo, rpp).done(function (response) {
                if (response.status != false) {
                    DashBoard.DashBoardModifiedGridLoad(response, PageNo, rpp);
                    var TableControl = "pnlDashboard #pnlModifiedNoteGrid #dgvModifiedNoteGrid";
                    var PagingPanelControlID = "pnlDashboard #pnlModifiedNoteGrid #dgvModifiedNoteGrid_paginate";
                    var ClassControlName = "DashBoard";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout(CreatePagination(response.VisitsCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        DashBoard.DashBoardModifiedNotesSearch(PageNumber, ResultPerPage, null);
                    }), 10);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    //#endregion Encounter

    //#region Document
    DashBoardDocumentSearch: function (PageNo, rpp, GridData) {
        DashBoard.EnableDisableControl();
        if ($("#pnlDashboard #pnlPatientDocumentGrid").css("display") == "none")
            $("#pnlDashboard #pnlPatientDocumentGrid").show();
        var myJSON = new Object();
        utility.ValidateFromToDate('frmDashboard', 'dpEntryDateFrom', 'dpEntryDateTo', true);
        utility.ValidateFromToDate('frmDashboard', 'dpDOSDateFrom', 'dpDOSDateTo', true);
        $('#pnlDashboard #pnlPatientDocumentGrid #chkMasterPatDoc').prop('checked', false);
        // myJSON.FromEntry = $('#frmDashboard #gridControlDoc #dpEntryDateFrom').val();
        myJSON.PatientName = $("#pnlDashboard #pnlPatientDocumentGrid #txtFullName").val();
        myJSON.dosFrom = $("#pnlDashboard #pnlPatientDocumentGrid #dpDOSDateFrom").val();
        myJSON.dosTo = $("#pnlDashboard #pnlPatientDocumentGrid #dpDOSDateTo").val();
        myJSON.assignedUser = $("#pnlDashboard #pnlPatientDocumentGrid #ddlDocumentReview").val();
        myJSON.priority = $("#pnlDashboard #pnlPatientDocumentGrid #ddlDocumentPriority").val();
        myJSON.status = $("#pnlDashboard #pnlPatientDocumentGrid #ddlDocumentStatus").val();

        if (myJSON.dosFrom != "" && myJSON.dosFrom != "")
            $('#pnlDashboard #dpDOSDateTo').removeAttr('disabled');

        if (myJSON.status == "-Select-") {
            myJSON.status = null;
        }
        if (myJSON.status == "Reviewed") {

            $('#pnlDashboard #pnlPatientDocumentGrid #btnReviewedSign').addClass('hidden');
        }
        else {

            $('#pnlDashboard #pnlPatientDocumentGrid #btnReviewedSign').removeClass('hidden');
        }
        myJSON.ddlActive = null;


        var myString = JSON.stringify(myJSON);

        if (GridData) {

            DashBoard.DashBoardDocumentGridLoad(GridData, PageNo, rpp);
        }
        else {

            DashBoard.SearchDocument(myString, null, PageNo, rpp, 0).done(function (response) {
                if (response.status != false) {
                    var PendingDocResponse = [];
                    if (response.PendingDocCount > 0) {
                        PendingDocResponse = JSON.parse(response.PendingPatDocument_JSON);
                    }
                    DashBoard.PendingDocList = PendingDocResponse;
                    DashBoard.DashBoardDocumentGridLoad(response, PageNo, rpp);
                    // PMS-4653
                    setTimeout(function () {
                        var currentPageno = DashBoard.params["CurrentPageNo"];
                        // if ($("#pnlDashboard #pagerParent").find('li:contains(' + currentPageno + ')').hasClass('active')) {
                        $("#pnlDashboard #pagerParent li.active").removeClass('active');
                        $("#pnlDashboard #pagerParent").find('li:contains(' + currentPageno + ')').addClass('active')
                        // }
                    }, 200);


                }
                else {
                    // utility.DisplayMessages(response.Message, 3);
                    //$('#pnlDashboard #Documents #spnDocuments').css("display", "none");
                    if (DashBoard.params.isfromQuickDoc) {
                        $('#spnPendingDocumentsCount').hide();
                        DashBoard.params.isfromQuickDoc = false;
                    }

                    $('#pnlDashboard div.wDocuments .badge').css("display", "none");
                    $("#pnlDashboard #divPendingPagingGrid").css("display", "none");
                    $("#pnlDashboard #pnlPatientDocumentGrid #dgvPatientDocumentGrid").dataTable().fnDestroy();
                    $("#pnlDashboard #pnlPatientDocumentGrid #dgvPatientDocumentGrid tbody").find("tr").remove();
                    $('#pnlDashboard #dgvPatientDocumentGrid').DataTable({
                        "language": {
                            "emptyTable": "No Document Found"
                        }, "searching": false, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false
                        //}, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false}]
                    });
                    // PRD-32 remove pagination
                    // $('#pnlDashboard #dgvPatientDocumentGrid_paginate').addClass('hidden');
                }
            });

        }



    },
    DashBoardDocumentGridLoad: function (response, PageNo, rpp) {

        var iTotalDisplayRecords = response.iTotalDisplayRecords
        var response = JSON.parse(response.DocumentLoad_JSON);


        if (response.length > 0) {
            $("#pnlDashboard #divPendingPagingGrid").css("display", "inline");
            //Showing 1 to 15 of 15 Record(s)
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            params: [];
            params["CurrentPage"] = CurrentPage;
            params["RecordsPerPage"] = RecordsPerPage;

            var PagesToShow = Math.ceil(iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("divPendingPagingGrid", iTotalDisplayRecords, 5, "DashBoard", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + iTotalDisplayRecords + " Record(s)";
            $("#pnlDashboard #divPendingPagingGrid #divShowingEntries").text(showingText);
            DashBoard.params["CurrentPageNo"] = CurrentPage;
            // Change Background Color to Black for selected page
            $("#pnlDashboard li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
        }
        else {
            $("#pnlDashboard #divPendingPagingGrid").css("display", "none");
        }

        $("#pnlDashboard #pnlPatientDocumentGrid #dgvPatientDocumentGrid").dataTable().fnDestroy();
        $("#pnlDashboard #pnlPatientDocumentGrid #dgvPatientDocumentGrid tbody").find("tr").remove();
        if (iTotalDisplayRecords == undefined) {
            $('#pnlDashboard div.wDocuments .badge').css("display", "none");

        } else {
            $('#pnlDashboard div.wDocuments .badge').css("display", "inline");

            $('#pnlDashboard div.wDocuments .badge').text(iTotalDisplayRecords);

            if (DashBoard.params.isfromQuickDoc) {
                $('#spnPendingDocumentsCount').show();
                $('#spnPendingDocumentsCount').text(iTotalDisplayRecords)
                DashBoard.params.isfromQuickDoc = false;
            }



        }
        if (response.length > 0) {
            var DocumentSearchJSONData = response;
            $.each(DocumentSearchJSONData, function (i, item) {
                var DocumentStatus = '';
                if (item.isReviewed == "0") {
                    DocumentStatus = "Pending"
                } else {
                    DocumentStatus = "Reviewed"
                }
                var DocPriority = "";
                if (item.DocPriority) {
                    if (item.DocPriority.toLowerCase().trim() == "low") {
                        DocPriority = '<span class=green bold>' + item.DocPriority + '</span>';
                    }
                    else if (item.DocPriority.toLowerCase().trim() == "medium") {
                        DocPriority = '<span class=dark-yellow bold>' + item.DocPriority + '</span>';
                    } else if (item.DocPriority.toLowerCase().trim() == "high") {
                        DocPriority = '<span class=red bold>' + item.DocPriority + '</span>';
                    }
                }
                var $row = $('<tr/>');
                $row.attr("id", "gvDashBoard_Documents_row" + i + "");
                $row.attr("documentid", item.PatDocId);
                $row.attr("Patientid", item.PatientId);
                //$row.attr("onclick", "utility.SelectGridRow($('#gvDashBoard_Documents_row" + i + "'))");
                $row.attr("onclick", "Patient_Document.DocumentEdit(" + item.PatientId + "," + item.PatDocId + ",event,1" + ");utility.SelectGridRow($(this))");
                $row.append(
                        "<td class='text-center'><input type='checkbox' id='chkPatDoc" + item.PatDocId + "' onclick='DashBoard.SelectDocument(event,this)'  /></td></td><td><a href=\"#\" onclick=\"DashBoard.DocumentPatientDemographics(" + item.PatientId + ",event);\"  title=\"View Patient Documents\">" + item.AccountNumber + "</a></td>" + "<td><a href=\"#\" onclick=\"DashBoard.DocumentPatientClinicalSummary(" + item.PatientId + ",event);\"  title=\"View Clinical Summary\">" + item.PatientName + "</a></td>" + '<td>' + item.DocumentName + '</td>><td>' + utility.RemoveTimeFromDate(null, item.DOS) + '</td><td>' + item.FacilityName + '</td><td class="text-center">' + DocPriority + '</td><td>' + item.DocAssignToReview + '</td><td>' + item.CreatedBy + ', ' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '</td><td>' + DocumentStatus + '</td><td>' + item.Comments + '</td>');


                $("#pnlDashboard #dgvPatientDocumentGrid tbody").last().append($row);
            });
        }
        else {
            $("#pnlDashboard #divPendingPagingGrid").css("display", "none");
            //  $('#pnlDashboard div.wDocuments .badge').css("display", "none");
            $('#pnlDashboard #dgvPatientDocumentGrid').DataTable({
                "language": {
                    "emptyTable": "No Document Found"
                }, "searching": false, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "orderable": false, "aTargets": [0, 1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#pnlDashboard #dgvPatientDocumentGrid'))
            ;
        else
            $("#pnlDashboard #dgvPatientDocumentGrid").DataTable({ "searching": false, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }], searching: false }); // to remove records per page dropdown
        //  $('.table-responsive').css('min-height', '220px');       
    },
    //#endregion Document
    DocumentPatientClinicalSummary: function (patientId, event) {

        if (event != null) {
            event.preventDefault();
            event.stopPropagation();
        }
        $.when(DashBoard.DocumentPatientDemographics(patientId, event)).then(function () {

            setTimeout(function () {
                demographicDetail.OpenClinicalSummary();
            }, 510);
        });

    },
    OpenPendingDocuments: function () {
        DashBoard.params.isfromQuickDoc = true;
        if (params.PanelID == 'pnlDashboard') {
            utility.callbackAfterAllDOMLoaded(function () {
                DashBoard.LoadWidgetControl('Documents', null);
            });
        }
        else {
            $.when(SelectTab('mstrTabDashBoard', 'false')).then(function () {
                utility.callbackAfterAllDOMLoaded(function () {
                    DashBoard.LoadWidgetControl('Documents', null);
                });
            });
        }
    },
    //#region Payment
    DashBoardPaymentsSearch: function (PageNo, rpp, GridData) {

        if ($("#pnlDashboard #pnlPaymentsGrid").css("display") == "none") {
            $("#pnlDashboard #pnlPaymentsGrid").show();
        }
        if (GridData) {

            DashBoard.DashBoardPaymentGridLoad(GridData);
        }
        else {

            DashBoard.LoadDashBoardPayment(PageNo, rpp).done(function (response) {
                if (response.status != false) {
                    DashBoard.DashBoardPaymentGridLoad(response, PageNo, rpp);
                }
                else {

                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    DashBoardTCMSearch: function (PageNo, rpp, GridData, Status, isfrom) {
        if (isfrom == null) {

            if ($('#pnlDashboard #pnlTCMGrid #TCMSwitch .ios-switch').hasClass('on')) {
                DashBoard.toggleStatus = 'All';
            } else {
                DashBoard.toggleStatus = 'Draft';
            }
        } else {
            DashBoard.toggleStatus = isfrom;
        }

        if (Status != null) {
            if (!$.trim($('#pnlDashboard #pnlTCMGrid #TCMSwitch').html()).length) {
                var HtmlOfSwitch = '<span class="pr-xs">Draft</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                                        '<input id="switchLabResultAbnormal" isactive="1" type="checkbox" checked="checked" data-plugin-ios-switch="" style="display: none;" onchange="DashBoard.DashBoardTCMSearch(null,null,null,null,\'switch\');">' +
                                         '</div><span class="pl-xs">All</span>';

                $('#pnlDashboard #gridControlTCM #TCMSwitch').html(HtmlOfSwitch);
                if ($("#TCMSwitch .btnWidgetSwitch div:first").hasClass("ios-switch on")) {
                    $("#TCMSwitch .btnWidgetSwitch div:first").removeClass("ios-switch on");
                    $("#TCMSwitch .btnWidgetSwitch div:first").addClass("ios-switch off")
                }

            } else {
                if ($("#TCMSwitch .btnWidgetSwitch div:first").hasClass("ios-switch on")) {
                    $("#TCMSwitch .btnWidgetSwitch div:first").removeClass("ios-switch on");
                    $("#TCMSwitch .btnWidgetSwitch div:first").addClass("ios-switch off")
                }
            }
        }

        PageNo = (PageNo == null || PageNo == "") ? 1 : PageNo;
        rpp = (rpp == null || rpp == "") ? 15 : rpp;
        if ($("#pnlDashboard #pnlTCMGrid").css("display") == "none") {
            $("#pnlDashboard #pnlTCMGrid").show();
        }
        if (GridData) {

            DashBoard.DashBoardTCMGridLoad(GridData);
        }
        else {

            DashBoard.LoadDashBoardTCM(PageNo, rpp, Status, isfrom).done(function (response) {
                if (response.status != false) {
                    DashBoard.DashBoardTCMGridLoad(response, PageNo, rpp);
                    var TableControl = "pnlDashboard #pnlTCMGrid #dgvTCMGrid";
                    var PagingPanelControlID = "pnlDashboard #dgvTCMGrid_paginate";
                    var ClassControlName = "DashBoard";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout(function () {
                        CreatePagination(response.TCMPatientsCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            DashBoard.DashBoardTCMSearch(PageNumber, ResultPerPage);
                        })
                    }, 10);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        DashBoard.SwicthWidgetInializatoin();
    },

    DashBoardPaymentGridLoad: function (response, PageNo, rpp) {


        if (response.PaymentsCount > 0) {
            $("#pnlDashboard #divPaymentsPagingGrid").css("display", "inline");
            //Showing 1 to 15 of 15 Record(s)
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var params = [];
            params["CurrentPage"] = CurrentPage;
            params["RecordsPerPage"] = RecordsPerPage;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("divPaymentsPagingGrid", response.iTotalDisplayRecords, 5, "DashBoard", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#pnlDashboard #divPaymentsPagingGrid #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $("#pnlDashboard li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
        }
        else {
            $("#pnlDashboard #divPaymentsPagingGrid").css("display", "none");
        }

        $("#pnlDashboard #pnlPaymentsGrid #dgvPaymentsGrid").dataTable().fnDestroy();
        $("#pnlDashboard #pnlPaymentsGrid #dgvPaymentsGrid tbody").find("tr").remove();

        if (response.iTotalDisplayRecords == undefined || response.iTotalDisplayRecords == null || response.iTotalDisplayRecords == 0) {
            $('#pnlDashboard div.wPayments .badge').css("display", "none");
        } else {
            $('#pnlDashboard div.wPayments .badge').css("display", "inline");
            $('#pnlDashboard div.wPayments .badge').text(response.iTotalDisplayRecords);
        }

        if (response.iTotalDisplayRecords > 0) {
            var PaymentSearchJSONData = JSON.parse(response.Payments_JSON);
            $.each(PaymentSearchJSONData, function (i, item) {
                var ViewClaim = "DashBoard.LoadVisitDetail('" + item.VisitId + "', '" + item.PatientId + "',event);";
                var $row = $('<tr/>');
                $row.attr("id", "gvDashBoard_Payment_row" + i + "");
                $row.attr("onclick", "utility.SelectGridRow($('#gvDashBoard_Payment_row" + i + "'))");
                $row.append('<td>' + item.PracticeName + '</td>><td>' + item.ProviderName + '</td><td>' + item.FacilityName + '</td><td>' + item.InsurancePlan + '</td><td><a class="CalimOpen" href="#" onclick="' + ViewClaim + '"  title="Claim Number">' + item.ClaimNumber + '</a></td><td class="text-right">' + "$" + parseFloat(item.InsurancePaid).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td  class="text-right">' + "$" + parseFloat(item.PatientPaid).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td  class="text-right">' + "$" + parseFloat(item.CopayPaid).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td>');

                $("#pnlDashboard #dgvPaymentsGrid tbody").last().append($row);
            });
        }
        else {
            $('#pnlDashboard #dgvPaymentsGrid').DataTable({
                "language": {
                    "emptyTable": "No Payment Found"
                }, "searching": false, "autoWidth": false, "bLengthChange": false, "order": [], "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#pnlDashboard #dgvPaymentsGrid'))
            ;
        else
            $("#pnlDashboard #dgvPaymentsGrid").DataTable({
                "searching": false, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [], "aoColumnDefs": [{
                    "bSortable": false, "aTargets": [0]
                }]
            }); // to remove records per page dropdown

        // $('.table-responsive').css('min-height', '220px');
    },

    DashBoardTCMGridLoad: function (response, PageNo, rpp) {
        if (DashBoard.toggleStatus == "Span" || DashBoard.toggleStatus == null) {
            if ($("#TCMSwitch .btnWidgetSwitch div:first").hasClass("ios-switch on")) {
                $("#TCMSwitch .btnWidgetSwitch div:first").removeClass("ios-switch on");
                $("#TCMSwitch .btnWidgetSwitch div:first").addClass("ios-switch off")
            }
        }
        DashBoard.LoadDashBoardTCMNotCreatedCount();
        if ($.fn.dataTable.isDataTable("#pnlDashboard #pnlTCMGrid #dgvTCMGrid")) {
            $("#pnlDashboard #pnlTCMGrid #dgvTCMGrid").dataTable().fnClearTable();
            $("#pnlDashboard #pnlTCMGrid #dgvTCMGrid").dataTable().fnDestroy();
            $("#pnlDashboard #pnlTCMGrid #dgvTCMGrid tbody").find("tr").remove();
        }
        if (response.TCMPatientsCount > 0) {
            $("#pnlDashboard #divTCMPagingGrid").css("display", "inline");
            //Showing 1 to 15 of 15 Record(s)
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var params = [];
            params["CurrentPage"] = CurrentPage;
            params["RecordsPerPage"] = RecordsPerPage;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("divTCMPagingGrid", response.iTotalDisplayRecords, 5, "DashBoard", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#pnlDashboard #divTCMPagingGrid #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $("#pnlDashboard li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
        }
        else {
            $("#pnlDashboard #divTCMPagingGrid").css("display", "none");
            $("#pnlDashboard #dgvTCMGrid_paginate").css("display", "none");
            $("#pnlDashboard #dgvTCMGrid_info").css("display", "none");

        }

        if (response.iTotalDisplayRecords == undefined || response.iTotalDisplayRecords == null || response.iTotalDisplayRecords == 0) {
            $('#pnlDashboard div.wTCM .badge').css("display", "none");
        } else {
            $('#pnlDashboard div.wTCM .badge').css("display", "inline");
            $('#pnlDashboard div.wTCM .badge').text(response.iTotalDisplayRecords);
        }

        if (response.iTotalDisplayRecords > 0) {
            var TCMPatientsJSONData = JSON.parse(response.TCMPatients_JSON);
            $.each(TCMPatientsJSONData, function (i, item) {
                var EditTCMPatient = "";
                var appointmentDate = item.DateOfAppointment.toLowerCase() == "does not apply" ? item.DateOfAppointment : utility.RemoveTimeFromDate(null, item.DateOfAppointment);
                var status = item.Status == null || item.Status == "NotCreated" ? "Not Created" : item.Status;
                var $row = $('<tr/>');
                $row.attr("id", "gvDashBoard_Payment_row" + i + "");
                $row.attr("onclick", "DashBoard.LoadPhoneEncounter(" + item.PatientId + ");");
                $row.append('<td>' + item.AccountNumber + '</td><td>' + item.PatientName + '</td><td>' + utility.RemoveTimeFromDate(null, item.DischargeDate) + '</td><td>' + appointmentDate + '</td><td>' + item.Provider + '</td><td>' + item.Insurance + '</td><td>' + status + '</td>');


                $("#pnlDashboard #dgvTCMGrid tbody").last().append($row);
            });
        }
        else {

            $('#pnlDashboard #dgvTCMGrid').DataTable({
                "destroy": true,
                "language": {
                    "emptyTable": "No Record Found"
                }, "searching": false, "autoWidth": false, "bLengthChange": false, "order": [], "aoColumnDefs": [{
                    "bSortable": false, "aTargets": [6]
                }]
            });
        }
        if ($.fn.dataTable.isDataTable('#pnlDashboard #dgvTCMGrid'))
            ;
        else
            $("#pnlDashboard #dgvTCMGrid").DataTable({
                "aaSortingFixed": [0, 'desc'], "searching": false, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [], "aoColumnDefs": [{
                    "bSortable": false, "aTargets": [6]
                }]
            }); // to remove records per page dropdown

        // $('.table-responsive').css('min-height', '220px');
    },

    DashBoardDataChangeGridLoad: function (response, rpp, PageNo) {
        if (response.CheckInPatientsCount > 0) {
            $("#pnlDashboard #pnlDataChangeRequestGrid").css("display", "inline");
            //Showing 1 to 15 of 15 Record(s)
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var params = [];
            params["CurrentPage"] = CurrentPage;
            params["RecordsPerPage"] = RecordsPerPage;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("dgvDataChangeRequestGrid_paginate", response.iTotalDisplayRecords, 5, "DashBoard", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#pnlDashboard #dgvDataChangeRequestGrid_paginate #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $("#pnlDashboard li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
        }
        else {
            //  $("#pnlDashboard #pnlDataChangeRequestGrid").css("display", "none");
        }

        $("#pnlDashboard #pnlDataChangeRequestGrid #dgvDataChangeRequestGrid").dataTable().fnDestroy();
        $("#pnlDashboard #pnlDataChangeRequestGrid #dgvDataChangeRequestGrid tbody").find("tr").remove();
        if (response.iTotalDisplayRecords == undefined) {
            $('#pnlDashboard #pnlLiveRequests .badge').css("display", "none");
        } else {
            $('#pnlDashboard div.wLiveRequests .badge').css("display", "inline");
            $("#pnldashboard div[class='wLiveRequests'] .badge").css("display", "inline");
            $("#spnLiveRequests").text(response.iTotalDisplayRecords)
            $("#SpnCheckInRequestCount").text(response.iTotalDisplayRecords)
            //$('#pnlDashboard #pnlLiveRequests .badge').css("display", "inline");
            //$('#pnlDashboard #pnlLiveRequests .badge').text(response.iTotalDisplayRecords);
        }
        $('#pnlDashboard #btnCheckInRequestsApprove').addClass('hidden');
        $('#pnlDashboard #btnCheckInRequestsDiscard').addClass('hidden');
        if (response.CheckInPatientsCount > 0) {
            var CheckInPatientSearchJSONData = JSON.parse(response.CheckInPatients_JSON);
            $.each(CheckInPatientSearchJSONData, function (i, item) {
                var TextColor = "";
                var RequestStatus = "";
                if (item.Status.toLowerCase().trim() == "pending") {
                    TextColor = "text-primary";
                    RequestStatus = "Pending";
                } else if (item.Status.toLowerCase().trim() == "approved") {
                    TextColor = "text-success";
                    RequestStatus = "Approved";
                } else {
                    TextColor = "text-danger";
                    RequestStatus = "Discarded";
                }
                var $row = $('<tr/>');
                $row.attr("id", "gvDashBoard_CheckinPatient_row" + i + "");
                $row.attr("onclick", "DashBoard.LoadCheckInPatients('" + item.PatientId + "','" + item.DimmyPatientId + "','" + item.DBTableName + "','" + item.Status + "','" + item.RequestReceivedFor + "');");
                $row.append('<td style="display:none"></td><td>' + item.PatientName + '</td><td>' + item.AppointmentDate + '</td><td>' + item.Provider + '</td><td>' + item.Facility + '</td><td>' + item.RequestReceivedFor + '</td><td class="' + TextColor + '">' + RequestStatus + '</td><td>' + item.RequestReceivedAt + '</td><td>' + item.AppointmentReason + '</td><td style="display: none;"></td>');
                $("#pnlDashboard #dgvDataChangeRequestGrid tbody").last().append($row);
            });
            $("#pnlDashboard #dgvDataChangeRequestGrid thead tr th:last").attr("style", "display: none;");
            $("#pnlDashboard #dgvDataChangeRequestGrid thead tr th:first").attr("style", "display: none;");
        }
        else {
            $('#pnlDashboard #dgvDataChangeRequestGrid').DataTable({
                "language": {
                    "emptyTable": "No Check In Patient Found"
                }, "searching": false, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{
                    "bSortable": true, "aTargets": [4]
                }]
                //}, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false}]
            });
        }
        if ($.fn.dataTable.isDataTable('#pnlDashboard #dgvDataChangeRequestGrid'))
            ;
        else {
            DashBoard.EditableGridOrder = $("#pnlDashboard #dgvDataChangeRequestGrid").DataTable({
                "destroy": true,
                "aaSorting": [],
                "searching": false, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{
                    "bSortable": true, "aTargets": [4]
                }]
            });
        }

        //  $('.table-responsive').css('min-height', '220px');
    },

    DashBoardCheckInRequestGridLoad: function (response, rpp, PageNo) {
        if (response.CheckInPatientsCount > 0) {   
            $("#pnlDashboard #pnlDataChangeRequestGrid").css("display", "inline");
            //Showing 1 to 15 of 15 Record(s)
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var params = [];
            params["CurrentPage"] = CurrentPage;
            params["RecordsPerPage"] = RecordsPerPage;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("dgvDataChangeRequestGrid_paginate", response.iTotalDisplayRecords, 5, "DashBoard", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#pnlDashboard #dgvDataChangeRequestGrid_paginate #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $("#pnlDashboard li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
        }
        else {
            //  $("#pnlDashboard #pnlDataChangeRequestGrid").css("display", "none");
        }

        $("#pnlDashboard #pnlDataChangeRequestGrid #dgvDataChangeRequestGrid").dataTable().fnDestroy();
        $("#pnlDashboard #pnlDataChangeRequestGrid #dgvDataChangeRequestGrid tbody").find("tr").remove();
        if (response.iTotalDisplayRecords == undefined) {
            $('#pnlDashboard #pnlLiveRequests .badge').css("display", "none");
        } else {
            $('#pnlDashboard div.wLiveRequests .badge').css("display", "inline");
            $("#pnldashboard div[class='wLiveRequests'] .badge").css("display", "inline");
            $("#spnLiveRequests").text(response.iTotalDisplayRecords)
            $('#pnlDashboard #pnlLiveRequests .badge').css("display", "inline");
            $('#pnlDashboard #pnlLiveRequests .badge').text(response.iTotalDisplayRecords);
            $("#SpnCheckInRequestCount").text(response.iTotalDisplayRecords)
        }
        $('#pnlDashboard #btnCheckInRequestsApprove').removeClass('hidden');
        $('#pnlDashboard #btnCheckInRequestsDiscard').removeClass('hidden');
        if (response.CheckInPatientsCount > 0) {
            var CheckInPatientSearchJSONData = JSON.parse(response.CheckInPatients_JSON);
            $.each(CheckInPatientSearchJSONData, function (i, item) {
                var TextColor = "";
                var RequestStatus = "";
                if (item.Status.toLowerCase().trim() == "pending"){
                    TextColor = "text-primary";
                    RequestStatus = "Pending";
                } else if (item.Status.toLowerCase().trim() == "approved") {
                    TextColor = "text-success";
                    RequestStatus = "Approved";
                } else {
                    TextColor = "text-danger";
                    RequestStatus = "Discarded";
                }
                var $row = $('<tr/>');
                $row.attr("id", "gvDashBoard_CheckinPatient_row" + i + "");
                var ApproveAppointment = "MobileAppRequest.InsertPatientAppointment(" + item.PatientId + ", " + item.AppointmentId + ", true);";
                var DiscardAppointment = "MobileAppRequest.InsertPatientAppointment(" + item.PatientId + ", " + item.AppointmentId + ", false);";
                //$row.attr("onclick", "DashBoard.LoadCheckInPatients('" + item.PatientId + "','" + item.DimmyPatientId + "','" + item.DBTableName + "','" + item.Status + "','" + item.RequestReceivedFor + "');");
                $row.append('<td><a class="btn btn-xs" href="#" onclick="' + ApproveAppointment + '" title="Approve Record"><i class="fa fa-check-circle green"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="' + DiscardAppointment + '" title="Discard Record"><i class="fa fa-ban red"></i></a>&nbsp;</td><td>' + item.PatientName + '</td><td>' + item.AppointmentDate + ' ' + item.TimeFrom + '</td><td>' + item.Provider + '</td><td>' + item.Facility + '</td><td>' + item.RequestReceivedFor + '</td><td class="' + TextColor + '">' + RequestStatus + '</td><td>' + item.RequestReceivedAt + '</td><td>' + item.AppointmentReason + '</td><td>' + item.RejectionReason + '</td>');
                $("#pnlDashboard #dgvDataChangeRequestGrid tbody").last().append($row);
            });
            $("#pnlDashboard #dgvDataChangeRequestGrid thead tr th:last").removeAttr("style", "display: none;");
            $("#pnlDashboard #dgvDataChangeRequestGrid thead tr th:first").removeAttr("style", "display: none;");
        }
        else {
            $('#pnlDashboard #dgvDataChangeRequestGrid').DataTable({
                "language": {
                    "emptyTable": "No Check In Patient Found"
                }, "searching": false, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{
                    "bSortable": true, "aTargets": [4]
                }]
                , "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false}]
            });
        }
        if ($.fn.dataTable.isDataTable('#pnlDashboard #dgvDataChangeRequestGrid'))
            ;
        else {
            DashBoard.EditableGridOrder = $("#pnlDashboard #dgvDataChangeRequestGrid").DataTable({
                "destroy": true,
                "aaSorting": [],
                "searching": false, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{
                    "bSortable": true, "aTargets": [4]
                }]
            });
        }

         // $('.table-responsive').css('min-height', '220px');
    },

    SearchDashBoardPatientPortalRequests: function (PageNo, rpp, GridData) {

        DashBoard.EnableDisableControl();
        if ($("#pnlDashboard #pnlPatientPortalRequests").css("display") == "none") {
            $("#pnlDashboard #pnlPatientPortalRequests").show();
        }


        DashBoard.LoadPatientPortalSignupRequests(PageNo, rpp).done(function (response) {
            if (response.status != false) {
                DashBoard.DashBoardPatientsSignupReqGridLoad(response, PageNo, rpp);
                var TableControl = "pnlDashboard #pnlPatientPortalRequests #dgvPatientsSignupReq";
                var PagingPanelControlID = "pnlDashboard #dgvPatientsSignupReq_Paging";
                var ClassControlName = "DashBoard";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(function () {
                    CreatePagination(response.PatientsrequestCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        DashBoard.DashBoardPatientPortalRequests(PageNumber, ResultPerPage);
                    })
                },
    10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },


    LoadPatientPortalSignupRequests: function (PageNumber, RowsPerPage) {

        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 15 : RowsPerPage;

        var objData = new JSON.constructor();
        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        // objData["Status"]=   $("#pnlDashboard #pnlDataChangeRequestGrid #ddlDataChangeRequestStatus").text()
        objData["Status"] = $('#pnlDashboard #pnlPatientPortalRequests #patientStatus option:selected').val();
        objData["ProviderId"] = $('#pnlDashboard #pnlPatientPortalRequests #hfProviderId').val();


        objData["CommandType"] = "search_patients_signup_request";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "SearchDashBoard");

    },

    DashBoardPatientsSignupReqGridLoad: function (response, rpp, PageNo) {
        if (response.PatientsrequestCount > 0) {
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var params = [];
            params["CurrentPage"] = CurrentPage;
            params["RecordsPerPage"] = RecordsPerPage;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("dgvPatientsSignupReq_Paging", response.iTotalDisplayRecords, 5, "DashBoard", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#pnlDashboard #dgvPatientsSignupReq_Paging #divShowingEntries").text(showingText);


            $("#pnlDashboard #pnlPatientPortalRequests #dgvPatientsSignupReq").dataTable().fnDestroy();
            $("#pnlDashboard #pnlPatientPortalRequests #dgvPatientsSignupReq tbody").find("tr").remove();


            var Patientsrequest_JSON = JSON.parse(response.Patientsrequest_JSON);
            $.each(Patientsrequest_JSON, function (i, item) {

                var $row = $('<tr/>');
                $row.attr("id", i);
                $row.attr("Patient_id", item.PatientId);
                //  $row.attr("onclick", "DashBoard.LoadCheckInPatients('" + item.PatientId + "','" + item.DimmyPatientId + "','" + item.DBTableName + "','" + item.Status + "','" + item.RequestReceivedFor + "');");
                $row.append('<td class="text-center"><input type="checkbox" id=' + "cskPatient_id_" + item.PatientId + '> </td><td>' + item.AccountNumber + '</td><td>' +
                    item.PatientName + '</td><td>' + item.StatusName + '</td><td>' + item.UpdatedByName + '</td><td>' +
                    item.RequestDateTime + '</td><td>' + item.UpdateDateTime + '</td><td>' + item.ProviderName + '</td>');
                $("#pnlDashboard #dgvPatientsSignupReq tbody").last().append($row);
            });


            if ($.fn.dataTable.isDataTable('#pnlDashboard #dgvPatientsSignupReq'))
                ;
            else {

                DashBoard.EditableGridOrder = $("#pnlDashboard #dgvPatientsSignupReq").DataTable({
                    "destroy": true,
                    "aaSorting": [],
                    "searching": false, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false,
                    "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
                });
            }
        }
        else {
            $("#pnlDashboard #pnlPatientPortalRequests #dgvPatientsSignupReq").dataTable().fnDestroy();
            $("#pnlDashboard #pnlPatientPortalRequests #dgvPatientsSignupReq tbody").find("tr").remove();
            $('#pnlDashboard #dgvPatientsSignupReq').DataTable({
                "language": {
                    "emptyTable": "No patient portal request found"
                }, "searching": false, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{
                    "bSortable": true, "aTargets": [4]
                }]
                //}, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false}]
            });
        }
    },
    PatientsSignupStatusChange: function () {
        var statusValue = $('#pnlDashboard #pnlPatientPortalRequests #patientStatus option:selected').val();
        if (statusValue == "Approved") {
            $('#pnlDashboard #pnlPatientPortalRequests #btnPPRequestsApprove').prop('disabled', true);
        }
        else {
            $('#pnlDashboard #pnlPatientPortalRequests #btnPPRequestsApprove').prop('disabled', false);
        }
    },

    RefreshDashBoardPatientPortalRequests: function () {
        $('#pnlDashboard #pnlPatientPortalRequests #patientStatus').val('Pending');
        $('#pnlDashboard #pnlPatientPortalRequests #hfProviderId').val("");
        $('#pnlDashboard #pnlPatientPortalRequests #txtProvider').val("");
        $('#pnlDashboard #pnlPatientPortalRequests #btnPPRequestsApprove').prop('disabled', false);
        DashBoard.SearchDashBoardPatientPortalRequests(null, null, null);
    },

    PatSignUpcheckUncheckAll: function (obj) {
        if ($(obj).is(":checked")) {
            $("#pnlDashboard #pnlPatientPortalRequests #dgvPatientsSignupReq input[id*='cskPatient_id']").each(function (k, v) {
                $(this).prop('checked', true);
            });
        }
        else {
            $("#pnlDashboard #pnlPatientPortalRequests #dgvPatientsSignupReq input[id*='cskPatient_id']").each(function (k, v) {
                $(this).prop('checked', false);
            });
        }
    },

    HealthPrivilegescheckUncheckAll: function (obj) {
        if ($(obj).is(":checked")) {
            $("#pnlDashboard #pnlPatientPortalRequests #HealthPrivilegesModal input[id*='CHK']").each(function (k, v) {
                $(this).prop('checked', true);
            });
        }
        else {
            $("#pnlDashboard #pnlPatientPortalRequests #HealthPrivilegesModal input[id*='CHK']").each(function (k, v) {
                $(this).prop('checked', false);
            });
        }
    },

    ApproveSignUpRequests: function () {
        var formData = JSON.parse($("#pnlDashboard #pnlPatientPortalRequests #HealthPrivilegesModal #healthRecordForm").getMyJSONByName());

        formData["PatientId"] = $("#pnlDashboard #pnlPatientPortalRequests #HealthPrivilegesModal #patIdsForApproval").val();

        AppPrivileges.GetFormPrivileges("Demographic", "Edit", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                DashBoard.DBCall_SignupRequestApprove(formData).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        DashBoard.SearchDashBoardPatientPortalRequests(null, null, null);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else {

                utility.DisplayMessages(res_message, 3);
            }

        });


    },
    openHealthPrivilegesModel: function () {
        var PatIds = $("#pnlDashboard #pnlPatientPortalRequests #dgvPatientsSignupReq input[id*='cskPatient_id_']:checked").map(function () {
            return this.id.replace("cskPatient_id_", "");
        }).get().join(',');

        if (PatIds == "") {
            utility.DisplayMessages("Please select any patient request to approve.", 4);
            return false;
        }
        else {
            $("#pnlDashboard #pnlPatientPortalRequests #HealthPrivilegesModal #patIdsForApproval").val(PatIds);
            $("#pnlDashboard #pnlPatientPortalRequests #HealthPrivilegesModal").modal();

        }
    },

    DeleteSelectedSignupRequests: function () {
        var PatIds = $("#pnlDashboard #pnlPatientPortalRequests #dgvPatientsSignupReq input[id*='cskPatient_id_']:checked").map(function () {
            return this.id.replace("cskPatient_id_", "");
        }).get().join(',');

        if (PatIds == "") {
            utility.DisplayMessages("Please select any patient request to delete.", 4);
            return false;
        }
        else {
            DashBoard.SignupRequestsDelete(PatIds);

        }
    },
    SignupRequestsDelete: function (PatientId) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Demographic", "Edit", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                DashBoard.DBCall_SignupRequestsDelete(PatientId).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        DashBoard.SearchDashBoardPatientPortalRequests(null, null, null);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else {

                utility.DisplayMessages(res_message, 3);
            }

        });

    },

    DBCall_SignupRequestsDelete: function (PatientId) {
        var objData = new JSON.constructor();

        objData["PatientId"] = PatientId;


        objData["CommandType"] = "patients_signup_delete_request";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "UpdateDashBoard");

    },
    DBCall_SignupRequestApprove: function (objData) {


        objData["CommandType"] = "patients_signup_approve_request";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "UpdateDashBoard");

    },
    LoadPhoneEncounter: function (patientid) {

        if (params["patientID"] != patientid) {
            $.when(setPatientBanner(patientid, "1")).then(function () {
                AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        Patient_Demographic.LoadPhoneEncounter();
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
            });
        }

    },
    //#endregion Payment

    //#region Copay
    DashBoardCopaySearch: function (Pageno, rpp, GridData) {


        if ($("#pnlDashboard #pnlCopayGrid").css("display") == "none") {
            $("#pnlDashboard #pnlCopayGrid").show();
        }

        if (GridData) {

            DashBoard.DashBoardCopayGridLoad(GridData);
        }
        else {

            DashBoard.LoadDashBoardCopay(Pageno, rpp).done(function (response) {
                if (response.status != false) {
                    DashBoard.DashBoardCopayGridLoad(response, rpp, Pageno);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }




    },
    DashBoardCopayGridLoad: function (response, rpp, PageNo) {

        if (response.CopayCount > 0) {
            $("#pnlDashboard #divCopayPagingGrid").css("display", "inline");
            //Showing 1 to 15 of 15 Record(s)
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var params = [];
            params["CurrentPage"] = CurrentPage;
            params["RecordsPerPage"] = RecordsPerPage;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("divCopayPagingGrid", response.iTotalDisplayRecords, 5, "DashBoard", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#pnlDashboard #divCopayPagingGrid #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $("#pnlDashboard li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
        }
        else {
            $("#pnlDashboard #divCopayPagingGrid").css("display", "none");
        }

        $("#pnlDashboard #pnlCopayGrid #dgvCopayGrid").dataTable().fnDestroy();
        $("#pnlDashboard #pnlCopayGrid #dgvCopayGrid tbody").find("tr").remove();
        if (response.iTotalDisplayRecords == undefined) {
            $('#pnlDashboard div.wCopayment .badge').css("display", "none");
        } else {
            $('#pnlDashboard div.wCopayment .badge').css("display", "inline");
            $('#pnlDashboard div.wCopayment .badge').text(response.iTotalDisplayRecords);
        }
        if (response.CopayCount > 0) {
            var CopaySearchJSONData = JSON.parse(response.Copay_JSON);
            $.each(CopaySearchJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", "gvDashBoard_Copay_row" + i + "");
                $row.attr("onclick", "utility.SelectGridRow($('#gvDashBoard_Copay_row" + i + "'))");

                var CopaymentMethod = "DashBoard.LoadCopayment('" + item.AppointmentId + "','" + item.PatientId + "','" + item.VisitId + "','" + item.AppointmentStatus + "','" + item.VisitStatus + "','" + item.ResourceId + "','" + item.FacilityId + "','" + item.ProviderId + "');";
                var EditAppointmentMethod = "DashBoard.AppointmentEdit('" + item.AppointmentId + "','" + item.VisitId + "','" + item.ResourceId + "','" + item.FacilityId + "','" + item.ProviderId + "','" + item.AppointmentStatus + "','" + item.VisitStatus + "');";


                $row.append('<td><a class="btn btn-xs" href="#" onclick="' + CopaymentMethod + '"  title="Copayment"><i class="fa fa-usd"></i></a><a class="btn  btn-xs" href="#" onclick="' + EditAppointmentMethod + '" title="Edit Appointment"><i class="fa fa-edit black"></i></a>&nbsp;</td><td><a href="#" onclick="utility.PatientDemographics(' + item.PatientId + ', ' + "mstrTabDashBoard" + ' ,event);"  title="View Patient Documents">' + item.PatientAccount + '</a></td>><td><a href="#" onclick="utility.PatientDemographics(' + item.PatientId + ', ' + "mstrTabDashBoard" + ' ,event);"  title="View Patient">' + item.PatientName + '</a></td><td>' + item.ProviderName + '</td><td>' + item.FacilityName + '</td><td class="text-right">' + utility.convertToFigure(item.Copay, true) + '</td><td class="text-right">' + utility.convertToFigure(item.CopayPaid, true) + '</td><td class="text-right">' + utility.convertToFigure(item.CopayDiscount, true) + '</td>');

                $("#pnlDashboard #dgvCopayGrid tbody").last().append($row);
            });
        }
        else {
            $('#pnlDashboard #dgvCopayGrid').DataTable({
                "language": {
                    "emptyTable": "No Copay Found"
                }, "searching": false, "autoWidth": false, "bLengthChange": false, "order": [], "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
                //}, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false}]
            });
        }
        if ($.fn.dataTable.isDataTable('#pnlDashboard #dgvCopayGrid'))
            ;
        else
            $("#pnlDashboard #dgvCopayGrid").DataTable({
                "searching": false, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [], "aoColumnDefs": [{
                    "bSortable": false, "aTargets": [0]
                }]
            }); // to remove records per page dropdown
        //  $('.table-responsive').css('min-height', '220px');
    },


    LoadCopayment: function (appid, patientid, patientvisitid, appointmentstatus, visitstatus, resourceid, facilityid, providerid) {

        if ((appointmentstatus.toUpperCase() != 'CONFIRM' || appointmentstatus.toUpperCase() == 'CONFIRM') && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
            visitstatus = "0";

        var params = [];
        params["FromAdmin"] = "0";
        params["ProviderId"] = providerid;
        params["FacilityId"] = facilityid;
        params["ResourceId"] = "";// resourceid;
        params["AppointmentId"] = appid;

        //params["DayDate"] = $('#pnlScheduleCalendar #daydate').text().trim();
        params["PatientId"] = patientid;
        params["PatientVisitId"] = patientvisitid;
        params["PatientVisitName"] = visitstatus;

        params["ParentCtrl"] = 'mstrTabDashBoard';
        LoadActionPan('schcopayment', params);
    },
    LoadCheckInPatients: function (patientid, DimmyPatientId, tabs, Status, RequestReceivedFor) {
        var params = [];;
        params["patientID"] = patientid;
        params["DimmyPatientId"] = DimmyPatientId;
        params["DataSection"] = RequestReceivedFor;
        params["Status"] = Status;
        params["DataSubSection"] = tabs;
        params["ParentCtrl"] = 'mstrTabDashBoard';
        LoadActionPan('MobileAppRequest', params);

    },
    AppointmentEdit: function (appid, patientvisitid, resourceid, facilityid, providerid, appointmentstatus, visitstatus) {

        if (appid) {
            var strMessage = "";
            AppPrivileges.GetFormPrivileges("Appointment", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {

                    var checkin = "";
                    if ((appointmentstatus.toUpperCase() != 'CONFIRM' || appointmentstatus.toUpperCase() == 'CONFIRM') && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                        checkin = "0";
                    if ((appointmentstatus.toUpperCase() == 'CHECK IN' || appointmentstatus.toUpperCase() == 'CHECK OUT') && patientvisitid != "" && appointmentstatus.toUpperCase() != 'CANCEL')
                        checkin = "1";
                    if (appointmentstatus.toUpperCase() == 'CANCEL' || (patientvisitid != "" && appointmentstatus.toUpperCase() != 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL'))
                        checkin = "2";

                    params: [];
                    params["checkin"] = checkin;
                    params["ProviderId"] = providerid;
                    params["ResourceId"] = resourceid;
                    params["FacilityId"] = facilityid;
                    //params["DayDate"] = $('#pnlScheduleCalendar #daydate').text().trim();
                    params["AppointmentId"] = appid;
                    params["PatientVisitId"] = patientvisitid;
                    params["mode"] = "Edit";
                    params["ParentCtrl"] = "mstrTabDashBoard";
                    LoadActionPan('appointmentDetail', params);

                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else {
            utility.DisplayMessages("Appointment is not created.", 2);
        }

    },

    //#endregion Copay

    //#region LabOrder
    DashBoardLabOrderSearch: function (pageno, rpp, gridData) {


        if ($("#pnlDashboard #pnlLabOrderGrid").css("display") == "none" && $("#pnlDashboard #widgetgridpanel > div:not(:hidden)").length == 0) {
            $("#pnlDashboard #pnlLabOrderGrid").show();
        }

        //if (gridData) {

        //    DashBoard.DashBoardLabOrderGridLoad(gridData);
        //}
        //else {
        //    DashBoard.DashBoardLabOrderGridLoad(null, rpp, pageno);
        //}

        //Start Muhammad Arshad 26-8-2016 http://192.168.0.85:8080/browse/EMR-782
        ////set date Picker Values on first load
        $("#pnlDashboard #ctrlPanLabOrder #dpStartDate").datepicker().on('change', function () {
            $('.datepicker').hide();
        });
        $("#pnlDashboard #ctrlPanLabOrder #dpToDate").datepicker().on('change', function () {
            $('.datepicker').hide();
        });

        //$("#pnlDashboard #LabResult #dplabResultStartDate").datepicker("setDate", EMRUtility.getDate("-", null, 1, null));
        //$("#pnlDashboard #LabResult #dplabResultToDate").datepicker("setDate", EMRUtility.getDate());

        //$("#pnlDashboard #LabUnsolicited #dplabUnsoResultStartDate").datepicker("setDate", EMRUtility.getDate("-", null, 1, null));
        //$("#pnlDashboard #LabUnsolicited #dplabUnsoResultToDate").datepicker("setDate", EMRUtility.getDate());
        //End Muhammad Arshad 26-8-2016 http://192.168.0.85:8080/browse/EMR-782


        DashBoard.DashBoardLabOrderGridLoad(null, pageno, rpp);
    },
    InitializeLabOrderGrid: function () {
        $("#dgvLabOrderGrid").kendoGrid({
            sortable: true,
            resizable: true,
            scrollable: false,
            pageable: {
                refresh: true,
                pageSizes: [5, 10, 20, 50, 100],
                buttonCount: 5
            },
            columns: [
        {
            title: "Action", width: "100px", template: '#=DashBoard.ActionLabOrderTemplate(data)#'
        },
        {
            field: "PatientAccount", title: "Account", width: "90px", template: '<a href="javascript:;" onclick="utility.PatientDemographics(\'#=PatientId#\', \'mstrTabDashBoard\', event);" title="View Patient">#=PatientAccount#</a>'
        },
        {
            field: "PatientName", title: "Patient", width: "100px", template: '<a href="javascript:;" onclick="utility.PatientDemographics(\'#=PatientId#\', \'mstrTabDashBoard\', event);" title="View Patient">#=PatientName#</a>'
        },
        {
            field: "FacilityName", title: "Facility", width: "100px"
        },
        {
            field: "ProviderName", title: "Provider", width: "100px"
        },
        {
            field: "LabOrder", title: "Charge", width: "80px", template: "$#= LabOrder#"
        },
        {
            field: "LabOrderPaid", title: "Paid", width: "90px", template: "$#= LabOrderPaid#"
        },
        {
            field: "LabOrderDiscount", title: "Discount", width: "100px", template: "$#= LabOrderDiscount#"
        }
            ],
            noRecords: true,
            messages: {
                noRecords: "No LabOrder Found"
            }
        });
    },

    selectUnselectLabOrderTabs: function (LabOrderTabId) {
        $('#pnlDashboard #pnlLabOrderGrid #pnlLabOrder ul#ulLabOrderResultTabsItems li').each(function (index, item) {
            if ($(item).attr("id") == LabOrderTabId) {
                //$(item).addClass("active");
                $(item).trigger("click");
            }
            //else {
            //    $(item).removeClass("active");
            //}
        });
    },

    DashBoardLabOrderGridLoad: function (response, pageNo, rpp) {




        pageNo = (pageNo == null || pageNo == "") ? 1 : pageNo;
        rpp = (rpp == null || rpp == "") ? 15 : rpp;

        var gridctl = $("#dgvLabOrderGrid").data("kendoGrid");
        var userLabOrderment = {
            data: [], total: 0
        };
        var dataSource;


        DashBoard.LoadDashBoardLabOrder('{}', 0, pageNo, rpp).done(function (response) {

            response = JSON.parse(response)
            if (response.status != false) {
                DashBoard.selectUnselectLabOrderTabs("listLabOrder");
                //Begin Edit by Fahad Malik , Bug # EMR-2152
                //if (response.LabLoad_JSON != undefined) {
                DashBoard.LabGridLoad(response);
                // }


                var TableControl = "pnlDashboard #dgvLabOrderGrid #dgvLabOrderDashboard";
                var PagingPanelControlID = "pnlDashboard #dgvLabOrderDashboard_Paging";
                var ClassControlName = "DashBoard";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(function () {
                    CreatePagination(response.LabOrderCount, pageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        DashBoard.DashBoardLabOrderGridLoad(PrimaryID, PageNumber, ResultPerPage);
                    })
                }, 10);

                //$("#pnlDashboard div[class*='wOrders'] .badge").text(response.iTotalDisplayRecords);
                $("#pnlDashboard #widgetpanel div").each(function () {
                    if ($(this).attr('onclick') == "DashBoard.LoadWidgetControl('Orders&Results',this);") {
                        // $(this).find(".badge").text(response.iTotalDisplayRecords);
                    }
                })
                if (response.LabOrderCount > 0) {
                    $("#pnlDashboard div[class*='wOrders'] .badge").css("display", "inline");
                } else {
                    //End Edit by Fahad Malik , Bug # EMR-2152
                    $("#pnlDashboard div[class*='wOrders'] .badge").css("display", "none");
                }
            }
            else
                utility.DisplayMessages(response.Message, 3);

        });


    },

    DashBoardLabResultGridLoad: function (response, rpp, pageNo, IsAssignedResultsFromQuick) {

        pageNo = (pageNo == null || pageNo == "") ? 1 : pageNo;
        rpp = (rpp == null || rpp == "") ? 15 : rpp;

        if ($("#pnlDashboard #pnlLabOrderGrid").css("display") == "none" && $("#pnlDashboard #widgetgridpanel > div:not(:hidden)").length == 0)
            $("#pnlDashboard #pnlLabOrderGrid").show();
        //var gridctl = $("#LabResult").data("kendoGrid");
        //var userLabOrderment = { data: [], total: 0 };
        //var dataSource;
        DashBoard.LoadDashBoardLabResultAbnormalCount();
        if ($('#pnlDashboard #LabResult #txtProvider').val() == "")
            $('#pnlDashboard #LabResult #hfProvider').val('');

        DashBoard.GetAssignedLabResultsCount();

        DashBoard.LoadDashBoardLabResult('{}', 0, pageNo, rpp, IsAssignedResultsFromQuick).done(function (response) {
            response = JSON.parse(response)
            if (response.status != false) {
                //Begin Edit by Fahad Malik , Bug # EMR-2152
                //if (response.LabResultCount > 0) {
                DashBoard.selectUnselectLabOrderTabs("listLabResult");
                //if (response.LabOrderLoad_JSON != undefined)
                DashBoard.LabResultGridLoad(response);
                // userLabOrderment.data = JSON.parse(response.LabOrderLoad_JSON);

                var TableControl = "pnlDashboard  #LabResult #dgvLabResultGrid #dgvLabResultDashboard_Paging";
                var PagingPanelControlID = "pnlDashboard #dgvLabResultDashboard_Paging";
                var ClassControlName = "DashBoard";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(
                CreatePagination(response.LabResultCount, pageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                    DashBoard.DashBoardLabResultGridLoad(PrimaryID, ResultPerPage, PageNumber);
                }), 10);
                //$("#pnlDashboard div[class*='wOrders'] .badge").text(response.iTotalDisplayRecords);
                $("#pnlDashboard #widgetpanel div").each(function () {
                    if ($(this).attr('onclick') == "DashBoard.LoadWidgetControl('Orders&Results',this);") {
                        //$(this).find(".badge").text(response.iTotalDisplayRecords);
                    }
                })
                //}
                //if (response.LabResultCount > 0) { $("#pnlDashboard div[class*='wOrders'] .badge").css("display", "inline"); }
                //End Edit by Fahad Malik , Bug # EMR-2152
                //else {
                //    $("#pnlDashboard div[class*='wOrders'] .badge").css("display", "none");
                //}
            }
            else
                utility.DisplayMessages(response.Message, 3);

        });

        //} else {
        //    if (response.LabOrderCount > 0) {
        //        if (response.LabOrder_JSON != undefined)
        //            userLabOrderment.data = JSON.parse(response.LabOrder_JSON);
        //        userLabOrderment.total = response.iTotalDisplayRecords;

        //        dataSource = new kendo.data.DataSource({
        //            serverPaging: true,
        //            schema: {
        //                data: "data",
        //                total: "total"
        //            },
        //            pageSize: rpp,
        //            page: pageNo,
        //            data: userLabOrderment
        //        });

        //        $('#pnlDashboard #LabOrderment .badge').css("display", "inline");
        //        $('#pnlDashboard #LabOrderment .badge').text(response.iTotalDisplayRecords);
        //    } else {
        //        $('#pnlDashboard #LabOrderment .badge').css("display", "none");
        //    }
        //}

    },

    OpenAssignedResults: function () {
        DashBoard.params.IsAssignedResultsFromQuick = true;
        if (params.PanelID == 'pnlDashboard') {
            utility.callbackAfterAllDOMLoaded(function () {
                DashBoard.LoadWidgetControl('Orders&Results', null);
            });
        }
        else {
            $.when(SelectTab('mstrTabDashBoard', 'false')).then(function () {
                utility.callbackAfterAllDOMLoaded(function () {
                    DashBoard.params.IsAssignedResultsFromQuick = true;
                    DashBoard.LoadWidgetControl('Orders&Results', null);
                });
            });
        }
    },

    GetAssignedLabResultsCount: function () {
        DashBoard.GetAssignedLabResultsCount_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (DashBoard.params.IsAssignedResultsFromQuick)
                DashBoard.params.IsAssignedResultsFromQuick = false;

            if (response.status != false) {
                if (response.AssignedResultsCount > 0) {
                    $('#spnAssignedResults').show();
                    $('#spnAssignedResults').text(response.AssignedResultsCount);
                }
            }
            else {
                $('#spnAssignedResults').hide();
                $('#spnAssignedResults').text(response.AssignedResultsCount);
            }
        });
    },

    GetAssignedLabResultsCount_DBCall: function () {
        var objData = new Object();
        objData["AssigneeId"] = globalAppdata.AppUserId;
        objData["commandType"] = "get_assigned_labresults_count";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },

    DashBoardLabResultLoad: function (response, rpp, pageNo, isAbnormal) {
        if (!$.trim($('#pnlDashboard #LabResult #divSwitchLabResultAbnormal').html()).length) {
            var HtmlOfSwitch = '<span class="pr-xs">Abnormal</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                                     '<input id="switchLabResultAbnormal" isactive="1" type="checkbox" checked="checked" data-plugin-ios-switch="" style="display: none;" onchange="DashBoard.DashBoardLabResultGridLoad(null, null, null);">' +
                                      '</div><span class="pl-xs">All</span>';

            $('#pnlDashboard #LabResult #divSwitchLabResultAbnormal').html(HtmlOfSwitch);
        }

        if ($("#pnlDashboard #hfIsAbnormalTests").val() == "1") {
            if ($.trim($('#pnlDashboard #LabResult #divSwitchLabResultAbnormal').html()).length) {
                if ($("#divSwitchLabResultAbnormal .btnWidgetSwitch div:first").hasClass("ios-switch on")) {
                    $("#divSwitchLabResultAbnormal .btnWidgetSwitch div:first").removeClass("ios-switch on");
                    $("#divSwitchLabResultAbnormal .btnWidgetSwitch div:first").addClass("ios-switch off");
                }
            }
            $("#pnlDashboard #LabResult #switchLabResultAbnormal").attr("checked", false);
            $("#pnlDashboard #hfIsAbnormalTests").val("0");
        }
        if (DashBoard.params.IsAssignedResultsFromQuick) {
            $('#pnlDashboard #LabResult #ddlAssignedUsers').val(globalAppdata.AppUserId);
            $('#pnlDashboard #LabResult #txtProvider').val('');
            $('#pnlDashboard #LabResult #hfProvider').val('');
        }
        else {
            $('#pnlDashboard #LabResult #ddlAssignedUsers').val('');
            DashBoard.LoadDefaultValueForDashboardWidgetLabResultTab();
        }

        DashBoard.DashBoardLabResultGridLoad(response, rpp, pageNo, DashBoard.params.IsAssignedResultsFromQuick);
    },
    SwicthWidgetInializatoin: function () {
        (function ($) {
            'use strict';
            $(function () {
                $('#pnlDashboard [data-plugin-ios-switch]').each(function () {
                    var $this = $(this);

                    $this.themePluginIOS7Switch();
                });
            });
        }).apply(this, [jQuery]);
    },
    ActionLabOrderTemplate: function (data) {

        var LabOrdermentMethod = "DashBoard.LoadLabOrderment('" + data.AppointmentId + "','" + data.PatientId + "','" + data.VisitId + "','" + data.AppointmentStatus + "','" + data.VisitStatus + "','" + data.ResourceId + "','" + data.FacilityId + "','" + data.ProviderId + "');";
        var editAppointmentMethod = "DashBoard.AppointmentEdit('" + data.AppointmentId + "','" + data.VisitId + "','" + data.ResourceId + "','" + data.FacilityId + "','" + data.ProviderId + "','" + data.AppointmentStatus + "','" + data.VisitStatus + "');";

        return '<a class="btn btn-xs" href="javascript:;" onclick="' + LabOrdermentMethod + '"  title="LabOrderment"><i class="fa fa-usd"></i></a><a class="btn  btn-xs" href="javascript:;" onclick="' + editAppointmentMethod + '" title="Edit Appointment"><i class="fa fa-edit black"></i></a>';
    },

    LoadLabOrderment: function (appid, patientid, patientvisitid, appointmentstatus, visitstatus, resourceid, facilityid, providerid) {

        if ((appointmentstatus.toUpperCase() != 'CONFIRM' || appointmentstatus.toUpperCase() == 'CONFIRM') && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
            visitstatus = "0";

        var params = [];
        params["FromAdmin"] = "0";
        params["ProviderId"] = providerid;
        params["FacilityId"] = facilityid;
        params["ResourceId"] = "";// resourceid;
        params["AppointmentId"] = appid;

        //params["DayDate"] = $('#pnlScheduleCalendar #daydate').text().trim();
        params["PatientId"] = patientid;
        params["PatientVisitId"] = patientvisitid;
        params["PatientVisitName"] = visitstatus;

        params["ParentCtrl"] = 'mstrTabDashBoard';
        LoadActionPan('schLabOrderment', params);
    },

    AppointmentEdit: function (appid, patientvisitid, resourceid, facilityid, providerid, appointmentstatus, visitstatus) {

        if (appid) {
            var strMessage = "";
            AppPrivileges.GetFormPrivileges("Appointment", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {

                    var checkin = "";
                    if ((appointmentstatus.toUpperCase() != 'CONFIRM' || appointmentstatus.toUpperCase() == 'CONFIRM') && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                        checkin = "0";
                    if ((appointmentstatus.toUpperCase() == 'CHECK IN' || appointmentstatus.toUpperCase() == 'CHECK OUT') && patientvisitid != "" && appointmentstatus.toUpperCase() != 'CANCEL')
                        checkin = "1";
                    if (appointmentstatus.toUpperCase() == 'CANCEL' || (patientvisitid != "" && appointmentstatus.toUpperCase() != 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL'))
                        checkin = "2";

                    params: [];
                    params["checkin"] = checkin;
                    params["ProviderId"] = providerid;
                    params["ResourceId"] = resourceid;
                    params["FacilityId"] = facilityid;
                    //params["DayDate"] = $('#pnlScheduleCalendar #daydate').text().trim();
                    params["AppointmentId"] = appid;
                    params["PatientVisitId"] = patientvisitid;
                    params["mode"] = "Edit";
                    params["ParentCtrl"] = "mstrTabDashBoard";
                    LoadActionPan('appointmentDetail', params);

                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else {
            utility.DisplayMessages("Appointment is not created.", 2);
        }

    },

    //#endregion LabOrder

    //  #region Active Accounts
    SearchPatientPortalAccounts: function (PatientId, PageNo, rpp, GridData) {
        if ($("#pnlDashboard #pnlActiveAccountsGrid").css("display") == "none")
            $("#pnlDashboard #pnlActiveAccountsGrid").show();
        if (GridData) {
            DashBoard.ActiveAccountsGridLoad(GridData);
        }
        else {
            DashBoard.LoadActiveAccounts_DBCall(PageNo, rpp).done(function (response) {
                if (response.status != false) {
                    DashBoard.ActiveAccountsGridLoad(response, PageNo, rpp);

                    var TableControl = "pnlDashboard  #pnlActiveAccountsGrid #dgvPatPortalAccountsGrid";
                    var PagingPanelControlID = "pnlDashboard #pnlActiveAccountsGrid #dgvPatPortalAccountsGrid_paginate";
                    var ClassControlName = "DashBoard";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.PatientPortalAccountsCount;
                    setTimeout(
                    CreatePagination(response.PatientPortalAccountsCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        DashBoard.SearchPatientPortalAccounts(PrimaryID, PageNumber, ResultPerPage);
                    }), 10);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    LoadActiveAccounts_DBCall: function (PageNumber, RowsPerPage) {
        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 15 : RowsPerPage;

        var objData = new JSON.constructor();
        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        objData["PatientId"] = $("#pnlDashboard #ctrlPanPatPortalAccounts #hfPatientId").val();
        objData["DOB"] = $("#pnlDashboard #ctrlPanPatPortalAccounts #dpDOB").val();
        objData["CommandType"] = "Search_ActiveAccounts";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "SearchDashBoard");
    },

    ActiveAccountsGridLoad: function (response, PageNo, rpp) {
        if ($.fn.dataTable.isDataTable("#pnlDashboard  #pnlActiveAccountsGrid #dgvPatPortalAccountsGrid")) {
            $("#pnlDashboard  #pnlActiveAccountsGrid #dgvPatPortalAccountsGrid").dataTable().fnClearTable();
            $("#pnlDashboard  #pnlActiveAccountsGrid #dgvPatPortalAccountsGrid").dataTable().fnDestroy();
        }

        $("#pnlDashboard  #pnlActiveAccountsGrid #dgvPatPortalAccountsGrid tbody").find("tr").remove();

        if (response.PatientPortalAccountsCount > 0) {
            var spnActiveAccountsCount = $("#pnlDashboard #spnActiveAccounts");
            spnActiveAccountsCount.html(response.PatientPortalAccountsCount);

            if ($("#pnlDashboard #spnActiveAccounts").css("display") == "none")
                $("#pnlDashboard #spnActiveAccounts").show();

            var PatientPortalJSON_Load = JSON.parse(response.PatientPortalLoad_JSON);

            $.each(PatientPortalJSON_Load, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", "gvPatientPortalAccount_row" + item.PatientId);
                $row.append('<td style="display:none;">' + item.PatientId + '</td><td><a href=\"#\" onclick=\"utility.PatientDemographics(' + item.PatientId + ', mstrTabDashBoard, event);\"  title=\"View Patient Accounts\">' + item.AccountNo + '</a></td><td><a href=\"#\" onclick=\"utility.PatientDemographics(' + item.PatientId + ', mstrTabDashBoard, event);\"  title=\"View Patient Accounts\">' + item.PatientName + '</a></td><td>' + utility.RemoveTimeFromDate(null, item.DOB) + '</td><td>' + item.Insurance + '</td><td>' + item.Provider + '</td>');

                $("#pnlDashboard  #pnlActiveAccountsGrid #dgvPatPortalAccountsGrid tbody").last().append($row);
            });
        }
        else {
            $("#pnlDashboard  #pnlActiveAccountsGrid #dgvPatPortalAccountsGrid").DataTable({
                "bdestroy": true,
                "language": {
                    "emptyTable": "No Active Accounts Found"
                }, "autoWidth": false, "bLengthChange": false, "bInfo": false, "bPaginate": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#pnlDashboard  #pnlActiveAccountsGrid #dgvPatPortalAccountsGrid"))
            ;
        else {
            $("#pnlDashboard  #pnlActiveAccountsGrid #dgvPatPortalAccountsGrid").DataTable({ "destroy": true, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        }

        EMRUtility.fixDataTableDuplication("#pnlDashboard  #pnlActiveAccountsGrid #dgvPatPortalAccountsGrid");
    },

    BindPatNameForActiveAccounts: function () {
        var Ctrl = $("#pnlDashboard #ctrlPanPatPortalAccounts #txtFullName");
        var func = function () {
            return utility.GetPatientArrayByName(Ctrl.val(), 1)
        };
        var hfCtrl = $("#pnlDashboard #ctrlPanPatPortalAccounts #hfPatientId");
        var onSelect = function (e) {
            $("#pnlDashboard #ctrlPanPatPortalAccounts #txtAccountNumber").attr("disabled", "disabled");
            $("#pnlDashboard #ctrlPanPatPortalAccounts #lnkPatientAccount").attr("disabled", "disabled");
            $("#pnlDashboard #ctrlPanPatPortalAccounts #txtAccountNumber").val(e.AccountNumber);
            $("#pnlDashboard #ctrlPanPatPortalAccounts #hfAccountNo").val(e.AccountNumber);
            utility.InsertRecentPatient(e.id);
        };
        var onChange = function (valid) {
            if (!valid) {
                $("#pnlDashboard #ctrlPanPatPortalAccounts #txtAccountNumber").removeAttr("disabled", "disabled");
                $("#pnlDashboard #ctrlPanPatPortalAccounts #lnkPatientAccount").removeAttr("disabled", "disabled");
                $("#pnlDashboard #ctrlPanPatPortalAccounts #txtAccountNumber").val("");
            }
        }
        utility.BindKendoAutoComplete(Ctrl, 3, "FullName", "contains", null, func, hfCtrl, onSelect, onChange);
    },
    BindPatAccountForActiveAccounts: function () {
        var Ctrl = $("#pnlDashboard #ctrlPanPatPortalAccounts #txtAccountNumber");
        var func = function () {
            return utility.GetPatientArray(Ctrl.val(), 1)
        };
        var hfCtrl = $("#pnlDashboard #ctrlPanPatPortalAccounts #hfPatientId");
        var onSelect = function (e) {
            $("#pnlDashboard #ctrlPanPatPortalAccounts #txtFullName").attr("disabled", "disabled");
            $("#pnlDashboard #ctrlPanPatPortalAccounts #lnkPatientName").attr("disabled", "disabled");
            $("#pnlDashboard #ctrlPanPatPortalAccounts #txtFullName").val(e.FullName);
            $("#pnlDashboard #ctrlPanPatPortalAccounts #hfAccountNo").val(e.AccountNumber);
            utility.InsertRecentPatient(e.id);
        };
        var onChange = function (valid) {
            if (!valid) {
                $("#pnlDashboard #ctrlPanPatPortalAccounts #txtFullName").removeAttr("disabled", "disabled");
                $("#pnlDashboard #ctrlPanPatPortalAccounts #lnkPatientName").removeAttr("disabled", "disabled");
                $("#pnlDashboard #ctrlPanPatPortalAccounts #txtFullName").val("");
            }
        }
        utility.BindKendoAutoComplete(Ctrl, 4, "AccountNumber", "contains", null, func, hfCtrl, onSelect, onChange);
    },

    OpenPatAccountFromActiveAccount: function (innerCtrl) {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'mstrTabDashBoard';
        params["IsUnsolitedPatientSearch"] = false;
        params["IsFirstLoadFromDash"] = false;
        params["ActiveWidget"] = "ActiveAccounts";
        if (innerCtrl == 'txtFullName') {
            params["RefCtrl"] = innerCtrl;
        }
        else {
            params["RefCtrl"] = innerCtrl;
        }

        LoadActionPan('Patient_Search', params);
    },

    ResetPatientFields: function (RefCtrlName) {
        if (RefCtrlName == 'txtFullName' && $("#pnlDashboard #ctrlPanPatPortalAccounts #txtFullName").val() == "") {
            $("#pnlDashboard #ctrlPanPatPortalAccounts #txtAccountNumber").removeAttr("disabled", "disabled");
            $("#pnlDashboard #ctrlPanPatPortalAccounts #lnkPatientAccount").removeAttr("disabled", "disabled");
            $("#pnlDashboard #ctrlPanPatPortalAccounts #txtAccountNumber").val("");
            $("#pnlDashboard #ctrlPanPatPortalAccounts #hfPatientId").val("");
        }
        else if (RefCtrlName == 'txtAccountNumber' && $("#pnlDashboard #ctrlPanPatPortalAccounts #txtAccountNumber").val() == "") {
            $("#pnlDashboard #ctrlPanPatPortalAccounts #txtFullName").removeAttr("disabled", "disabled");
            $("#pnlDashboard #ctrlPanPatPortalAccounts #lnkPatientName").removeAttr("disabled", "disabled");
            $("#pnlDashboard #ctrlPanPatPortalAccounts #txtFullName").val("");
            $("#pnlDashboard #ctrlPanPatPortalAccounts #hfPatientId").val("");
        }
    },
    //  #endregion Active Accounts

    //#region Patient Changes

    DashBoardPatientChangesSearch: function (PageNo, rpp, GridData) {
        DashBoard.EnableDisableControl();
        if ($("#pnlDashboard #pnlPatientChangesGrid").css("display") == "none") {
            $("#pnlDashboard #pnlPatientChangesGrid").show();
        }

        if (GridData) {

            DashBoard.DashBoardPatientChangesGridLoad(GridData, PageNo, rpp);
        }
        else {
            var self = $("#pnlDashboard #pnlPatientChangesGrid");
            var PatientData = self.getMyJSONByName();
            DashBoard.LoadDashBoardPatientChanges(PatientData, PageNo, rpp).done(function (response) {
                if (response.status != false) {
                    DashBoard.DashBoardPatientChangesGridLoad(response, PageNo, rpp);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    DashBoardPatientChangesGridLoad: function (response, PageNo, rpp) {
        // var gridPatientChargesControls = $('#pnlDashboard #pnlPatientChangesGrid #gridPatientChargesControls').html();
        var dpMonth = $('#pnlDashboard #pnlPatientChangesGrid #dpMonth').val();
        var ddlChangeEntity = $('#pnlDashboard #pnlPatientChangesGrid #ddlChangeEntity').val();

        //  $('#pnlDashboard #pnlPatientChangesGrid #gridPatientChargesControls').remove();

        //var gridControlPatientChanges = $('#pnlDashboard #pnlPatientChangesGrid #gridControlPatientChanges').html();
        //var dpMonth = $('#pnlDashboard #pnlPatientChangesGrid #dpMonth').val();
        //$('#pnlDashboard #pnlPatientChangesGrid #gridControlPatientChanges').remove();

        if (response.PatientChangesCount > 0) {
            $("#pnlDashboard #divPatientChangesPagingGrid").css("display", "inline");
            //Showing 1 to 15 of 15 Record(s)
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var params = [];
            params["CurrentPage"] = CurrentPage;
            params["RecordsPerPage"] = RecordsPerPage;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("divPatientChangesPagingGrid", response.iTotalDisplayRecords, 5, "DashBoard", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#pnlDashboard #divPatientChangesPagingGrid #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $("#pnlDashboard li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
        }
        else {
            $("#pnlDashboard #divPatientChangesPagingGrid").css("display", "none");
        }


        $("#pnlDashboard #pnlPatientChangesGrid #dgvPatientChangesGrid").dataTable().fnDestroy();
        $("#pnlDashboard #pnlPatientChangesGrid #dgvPatientChangesGrid tbody").find("tr").remove();

        if (response.iTotalDisplayRecords == undefined) {
            $('#pnlDashboard div.wPatientChanges .badge').css("display", "none");
        } else {
            $('#pnlDashboard div.wPatientChanges .badge').css("display", "inline");
            $('#pnlDashboard div.wPatientChanges .badge').text(response.iTotalDisplayRecords);
        }

        if (response.PatientChangesCount > 0) {
            var PatientChangesJSONData = JSON.parse(response.PatientChanges_JSON);
            $.each(PatientChangesJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", "gvDashBoard_Patient_row" + i + "");
                $row.attr("onclick", "utility.SelectGridRow($('#gvDashBoard_Patient_row" + i + "'))");

                var viewPatientAction = '';
                if (item.IsBreakGlassAllow) {
                    if (item.IsBreakGlassAllow == "True") {
                        viewPatientAction = '<a href="javascript:void(0);"  onclick="DashBoard.patientBreaktheGlass(' + item.PatientId.trim() + ',\'' + item.AccountNumber + '\',\'' + item.PatientName + '\',event);"   title="Break The Glass">';
                    } else if (item.IsBreakGlassAllow == "False") {
                        viewPatientAction = '<a  href="javascript:void(0);" onclick="utility.DisplayMessages(\'You are not authorized to view this patient.\',3);"  title="Access Restricted">';
                    }


                } else {
                    viewPatientAction = '<a href="#" onclick="utility.PatientDemographics(' + item.PatientId + ', ' + "mstrTabDashBoard" + ' ,event);"  title="View Patient">';
                }
                if (item.ColumnName == "SSN") {
                    if (globalAppdata.IsFullSSN.toLowerCase() === 'true') {
                    }
                    else {

                        if (item.OldValue != "") {
                            var last4digit = item.OldValue.slice(-4);
                            item.OldValue = "XXX-XX-" + last4digit;
                        }
                        if (item.NewValue != "") {
                            var last4digits = item.NewValue.slice(-4);
                            item.NewValue = "XXX-XX-" + last4digits;
                        }
                    }
                }
                $row.append('<td>' + viewPatientAction + item.AccountNumber + '</a></td><td>' + viewPatientAction + item.PatientName + '</a></td><td>' + item.ProviderName + '</td><td>' + item.ProfileName + '</td><td>' + item.DBAuditAction + '</td><td>' + item.ColumnName + '</td><td>' + item.OldValue + '</td><td>' + item.NewValue + '</td><td>' + item.UserName + '</td><td>' + item.CreatedDate + '</td>');

                $("#pnlDashboard #pnlPatientChangesGrid #dgvPatientChangesGrid tbody").last().append($row);
            });
        }
        else {
            $("#pnlDashboard #divPatientChangesPagingGrid").css("display", "none");
            $('#pnlDashboard #dgvPatientChangesGrid').DataTable({
                "language": {
                    "emptyTable": "No Patient Change Found"
                }, "searching": false, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false
            });
        }

        if ($.fn.dataTable.isDataTable('#pnlDashboard #dgvPatientChangesGrid'))
            ;
        else
            //adnan maqbool, pms-4028 , 18-02-2016
            $("#pnlDashboard #pnlPatientChangesGrid #dgvPatientChangesGrid").DataTable({
                "searching": false, "bInfo": false, "bPaginate": false, "bLengthChange": false, "aaSorting": [], "autoWidth": false
            }); // to remove records per page dropdown
        //end
        //  $('#pnlDashboard #dgvPatientChangesGrid_wrapper .datatables-header div:first').html('<div id="gridPatientChargesControls">' + gridPatientChargesControls + '</div>');
        utility.CreateMonthViewDatePicker("pnlDashboard #frmDashboard #dpMonth", function () {
        }, false, null, null);
        $('#pnlDashboard #pnlPatientChangesGrid #gridPatientChargesControls #dpMonth').val(dpMonth);
        $('#pnlDashboard #pnlPatientChangesGrid #gridPatientChargesControls #ddlChangeEntity option[value="' + ddlChangeEntity + '"]').prop("selected", true);
        DashBoard.documentReady(false);
        $('#pnlDashboard #dgvPatientChangesGrid_wrapper .datatables-header div.dataTables_filter').parent().removeClass('col-sm-12 col-md-6');
        $('#pnlDashboard #dgvPatientChangesGrid_wrapper .datatables-header div:first').removeClass('col-sm-12 col-md-6');


        if (!$('#pnlDashboard #dgvPatientChangesGrid_wrapper .datatables-header div.dataTables_filter').parent().hasClass('col-sm-6 col-md-5')) {
            $('#pnlDashboard #dgvPatientChangesGrid_wrapper .datatables-header div.dataTables_filter').parent().addClass('col-sm-6 col-md-5');
        }
        if (!$('#pnlDashboard #dgvPatientChangesGrid_wrapper .datatables-header div.dataTables_filter').parent().hasClass('mt-md')) {
            $('#pnlDashboard #dgvPatientChangesGrid_wrapper .datatables-header div.dataTables_filter').addClass('mt-md');
        }
        if (!$('#pnlDashboard #dgvPatientChangesGrid_wrapper .datatables-header div:first').hasClass('col-sm-6 col-md-9')) {
            $('#pnlDashboard #dgvPatientChangesGrid_wrapper .datatables-header div:first').addClass('col-sm-6 col-md-7');
        }
        //$('#pnlDashboard #dgvPatientChangesGrid_filter').parent().removeClass('col-sm-12 col-md-6').addClass('col-sm-4 pull-right mt-md');
        //if ($('#pnlDashboard #dgvPatientChangesGrid_filter').parent().siblings().first().hasClass('col-sm-12 col-md-6')) {
        //    $('#pnlDashboard #dgvPatientChangesGrid_filter').parent().siblings().first().remove();
        //}


        //$('#pnlDashboard #dgvPatientChangesGrid_wrapper div:first').append(' <div id="gridControlPatientChanges" class="col-sm-8 pull-left" >' + gridControlPatientChanges + '</div>');
        //utility.CreateMonthViewDatePicker("pnlDashboard #dpMonth", function () { }, true, null, null);
        //$('#pnlDashboard #pnlPatientChangesGrid #gridControlPatientChanges #dpMonth').val(dpMonth);

    },

    //endregion
    patientBreaktheGlass: function (PatientId, AccountNumber, PatientName, event) {
        if (event != null) {
            event.stopPropagation();
        }

        var FirstName = PatientName.split(',')[1];
        var LastName = PatientName.split(',')[0];
        var PanelID = 'pnlDashboard';
        DashBoard.showRestrictUser_PatientConsent("#" + PanelID + " #actionPanDashboard", PatientId, AccountNumber, PatientName, FirstName, LastName);
    },

    saveBreakGlassReason: function () {
        var PanelID = 'pnlDashboard';
        var BreakTheReason = $("#" + PanelID + " #actionPanDashboard #BreakTheReason").val();
        if (BreakTheReason && BreakTheReason.split(' ').join() != '') {
            var PatientId = $("#" + PanelID + " #actionPanDashboard #hfPatientId").val();
            var AccountNumber = $("#" + PanelID + " #actionPanDashboard #hfAccountNumber").val();
            var FullName = $("#" + PanelID + " #actionPanDashboard #hfFullName").val();
            var FirstName = $("#" + PanelID + " #actionPanDashboard #hfFirstName").val();
            var LastName = $("#" + PanelID + " #actionPanDashboard #hfLastName").val();
            Restrict_User.saveBreakGlassReason_DBCall(BreakTheReason, PatientId).done(function () {
                var PanelID = 'pnlDashboard';
                DashBoard.UnLoadRestrictUser_PatientConsent();
                //Begin 28/4/2016  Edit By M Ahmad Imran Bug # EMR-800
                $("#" + PanelID + " #actionPanDashboard").on('hidden.bs.modal', function () {

                    if ($("#" + PanelID + " #actionPanDashboard #pnldemographicDetail").length < 1) {
                        utility.PatientDemographics(PatientId, "mstrTabDashBoard", event);
                    }

                    //eval(MethodMode);
                });
                //End 28/4/2016  Edit By M Ahmad Imran Bug # EMR-800

            });
        } else {
            utility.DisplayMessages('Please enter the Break the Glass Reason', 2);
        }


    },
    DocumentPatientDemographics: function (patientid, event) {
        if (event != null) {
            event.preventDefault();
            event.stopPropagation();
        }
        var params = [];
        params["mode"] = "Edit";
        params["PatBanner"] = true;
        params["patientID"] = patientid;
        params["IsFill"] = false;
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "mstrTabDashBoard";
        LoadActionPan("demographicDetail", params);
    },
    // this function is used by both Notes and Progress Note Form
    showRestrictUser_PatientConsent: function (ActionPanID, PatientId, AccountNumber, FullName, FirstName, LastName) {

        $(ActionPanID).prepend($("#pnlDashboard #pnlRestrictUser_PatientConsent").html());
        $(ActionPanID).modal({
            show: 'true',
            backdrop: 'static',
            keyboard: false

        }).on('hidden.bs.modal', function () {
            $('body').addClass('modal-open');
        }).on('shown.bs.modal', function () {
            var PanelID = 'pnlDashboard';
            $("#" + PanelID + " #actionPanDashboard #lblPatientName").text(FirstName + ' ' + LastName);
            $("#" + PanelID + " #actionPanDashboard #hfPatientId").val(PatientId);
            $("#" + PanelID + " #actionPanDashboard #hfAccountNumber").val(AccountNumber);
            $("#" + PanelID + " #actionPanDashboard #hfFullName").val(FullName);
            $("#" + PanelID + " #actionPanDashboard #hfFirstName").val(FirstName);
            $("#" + PanelID + " #actionPanDashboard #hfLastName").val(LastName);
        });
    },
    // this function is used by both Notes and Progress Note Form
    UnLoadRestrictUser_PatientConsent: function () {
        //Begin 28/4/2016  Edit By M Ahmad Imran Bug # EMR-800
        var PanelID = 'pnlDashboard';
        var objDeffered = $.Deferred();
        $("#" + PanelID + " #actionPanDashboard").html('');
        $("#" + PanelID + " #actionPanDashboard").modal('hide');
        objDeffered.resolve();
        return objDeffered;
        //End 28/4/2016  Edit By M Ahmad Imran Bug # EMR-800
    },
    //#region Server Side Calls
    LoadDashBoard: function (DashBoardData, DBSId, DBSType) {

        var objData = new JSON.constructor();
        if (DashBoardData) {
            objData = JSON.parse(DashBoardData);
            objData["IsDashBoardData"] = true;
        }
        else
            objData["IsDashBoardData"] = false;


        objData["DBSId"] = DBSId;
        objData["DBSType"] = DBSType;
        objData["CommandType"] = "Load_DashBoard";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "LoadDashBoard");

    },
    LoadPatientMessages: function (PageNumber, RowsPerPage) {

        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 15 : RowsPerPage;
        var objData = new Object();


        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        objData["MessageName"] = $("#pnlDashboard #txtPatientMsgsName").val();
        objData["Priority"] = $("#pnlDashboard #ddlPatientPriorityMessage").val();
        objData["MessageDate"] = $("#pnlDashboard #dtpPatientMsgDate").val();
        objData["CommandType"] = "Load_Messages";
        objData["MessageType"] = "Patient";



        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "LoadDashBoard");
    },

    LoadMessages: function (PageNumber, RowsPerPage) {

        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 15 : RowsPerPage;
        var objData = new Object();


        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        objData["MessageName"] = $("#pnlDashboard #txtMsgsName").val();
        objData["Priority"] = $("#pnlDashboard #ddlPriorityMessage").val();
        objData["MessageDate"] = $("#pnlDashboard #dtpMsgDate").val();
        objData["CommandType"] = "Load_Messages";
        objData["MessageType"] = "Practice";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "LoadDashBoard");
    },

    LoadDirectMessages: function (PageNumber, RowsPerPage) {

        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 15 : RowsPerPage;


        var objData = new Object();
        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        objData["DirectAddress"] = $("#pnlDashboard #txtMsgsDirectAddress").val();
        objData["MessageDate"] = $("#pnlDashboard #dtpMsgDateDirect").val();
        objData["MessageType"] = "DIRECT";
        objData["CommandType"] = "Load_Direct_Messages";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "LoadDashBoard");
    },

    LoadLogMessages: function (PageNumber, RowsPerPage) {

        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 15 : RowsPerPage;

        var objData = new Object();
        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        objData["CommandType"] = "load_message_log";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "LoadDashBoard");
    },


    LoadDashBoardPayment: function (PageNumber, RowsPerPage) {

        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 15 : RowsPerPage;

        var objData = new JSON.constructor();
        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        objData["CommandType"] = "Search_Payment";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "SearchDashBoard");

    },

    loaddashboardAppointmentLabelCount: function () {

        var objData = new JSON.constructor();
        objData["CommandType"] = "load_dashboard_appointment_labelCount";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "LoadDashBoard");

    },


    BindProvider: function (Id, linkId, lblId, hfId) {

        if (!Id)
            Id = 'txtProvider';
        if (!linkId)
            linkId = 'lnkProviderEdit';
        if (!lblId)
            lblId = 'lblProvider';
        if (!hfId)
            hfId = 'hfProvider';
        var Ctrl = $("#pnlDashboard #pnlTCMGrid #" + Id);
        var func = function () {
            return utility.GetProviderArray(Ctrl.val())
        };
        var hfCtrl = $("#pnlDashboard #pnlTCMGrid #" + hfId);
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },
    LoadDefaultValueForDashboardWidgetLabResultTab: function () {
        if (globalAppdata['DefaultProviderName'] != "")
            if (globalAppdata['DefaultProviderName'].toLowerCase().indexOf("select") <= 0)
                $('#pnlDashboard #LabResult #txtProvider').val(globalAppdata['DefaultProviderName']);
            else
                $('#pnlDashboard #LabResult #txtProvider').val("");
        if (globalAppdata['DefaultProviderId'] != "")
            $('#pnlDashboard #LabResult #hfProvider').val(globalAppdata['DefaultProviderId']);
        else
            $('#pnlDashboard #LabResult #hfProvider').val("");
        if (globalAppdata['DefaultProviderId'] != "") {
            $('#pnlDashboard #LabResult #lnkProviderEdit').css("display", "inline");
            $('#pnlDashboard #LabResult #lblProvider').css("display", "none");
        }
        var $Ctrl = $("#pnlDashboard #LabResult  #txtProvider");
        var $hfCtrlS = $("#pnlDashboard #LabResult #hfProvider");
        var ProviderName = $("#pnlDashboard #LabResult  #txtProvider").val();
        var ProviderId = $("#pnlDashboard #LabResult #hfProvider").val();
        utility.SetKendoAutoCompleteSourceforValidate($Ctrl, ProviderName, $hfCtrlS, ProviderId);
    },
    BindProviderLabResultSearch: function (Id, linkId, lblId, hfId) {
        if (!Id)
            Id = 'txtProvider';
        if (!linkId)
            linkId = 'lnkProviderEdit';
        if (!lblId)
            lblId = 'lblProvider';
        if (!hfId)
            hfId = 'hfProvider';
        var Ctrl = $("#pnlDashboard #LabResult #" + Id);
        var func = function () {
            return utility.GetProviderArray(Ctrl.val())
        };
        var hfCtrl = $("#pnlDashboard #LabResult #" + hfId);
        var onSelect = function (e) {
            $("#pnlDashboard #LabResult #" + hfId).val(e.id);
            if ($("#pnlDashboard #LabResult #" + linkId).css("display") == "none") {
                $("#pnlDashboard #LabResult #" + linkId).css("display", "inline");
                $("#pnlDashboard #LabResult #" + lblId).css("display", "none");
            }
        };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect);
    },



    BindCCMInsurancePlan: function (Id, hfId, shortName) {
        var Ctrl = $("#pnlDashboard #pnlCCMEnrollmentInfoGrid #txtPrimaryInsurance");
        var func = function () {
            return utility.GetInsurancePlanArray(Ctrl.val())
        };
        var hfCtrl = $("#pnlDashboard #pnlCCMEnrollmentInfoGrid #hfInsurancePlanId");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    BindProviderAutoCompPatientportalSignupReq: function () {
        var Ctrl = $("#pnlDashboard #pnlPatientPortalRequests #txtProvider");
        var func = function () {
            return utility.GetProviderArray(Ctrl.val())
        };
        var hfCtrl = $("#pnlDashboard #pnlPatientPortalRequests #hfProviderId");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    BindReferralInsurancePlan: function () {
        var Ctrl = $("#pnlDashboard #ctrlPanIncomingReferral #txtPrimaryInsurance");
        var func = function () {
            return utility.GetInsurancePlanArray(Ctrl.val())
        };
        var hfCtrl = $("#pnlDashboard #ctrlPanIncomingReferral #hfInsurancePlanId");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    BindPatientChangesProvider: function () {
        var Ctrl = $("#pnlDashboard #pnlPatientChangesGrid #txtProvider");
        var func = function () {
            return utility.GetProviderArray(Ctrl.val())
        };
        var hfCtrl = $("#pnlDashboard #pnlPatientChangesGrid #hfProviderId");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },
    OpenProviderDetail: function (HiddenCtrl, TxtBoxCtrl) {
        var params = [];
        params["ProviderId"] = $('#pnlDashboard #' + HiddenCtrl).val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = TxtBoxCtrl;
        params["ParentCtrl"] = 'mstrTabDashBoard';
        LoadActionPan('providerDetail', params);
    },

    BindNoteProvider: function (innerCtrl, Id, linkId, lblId, hfId, shortName) {

        if (!Id)
            Id = 'txtProvider';
        if (!linkId)
            linkId = 'lnkProviderEdit';
        if (!lblId)
            lblId = 'lblProvider';
        if (!hfId)
            hfId = 'hfProviderId';

        if (!shortName)
            shortName = $("#pnlDashboard #pnlEncounterGrid #" + innerCtrl + " #txtProvider").val();

        $("#pnlDashboard #" + innerCtrl + " #hfProviderId").val("");
        $("#pnlDashboard #" + innerCtrl + " #NoteProviderText").val($("#pnlDashboard #pnlEncounterGrid #" + innerCtrl + " #txtProvider").val());



        utility.GetProviderArray(shortName).done(function (response) {

            $("#pnlDashboard #pnlEncounterGrid #" + innerCtrl + " #" + Id).autocomplete({
                autoFocus: true,
                source: response,
                select: function (event, ui) {
                    setTimeout(function () {

                        $("#pnlDashboard #pnlEncounterGrid #" + innerCtrl + " #" + Id).val(ui.item.value);
                        $("#pnlDashboard #" + innerCtrl + " #NoteProviderText").val(ui.item.value)
                        $("#pnlDashboard #" + innerCtrl + " #" + hfId).val(ui.item.id);
                        if ($("#pnlDashboard #pnlEncounterGrid #" + innerCtrl + " #" + linkId).css("display") == "none") {
                            $("#pnlDashboard #pnlEncounterGrid #" + innerCtrl + " #" + linkId).css("display", "inline");
                            $("#pnlDashboard #pnlEncounterGrid #" + innerCtrl + " #" + lblId).css("display", "none");
                        }

                    }, 100);

                }
            });
            //$("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes #" + Id).autocomplete("search");
        });

    },

    BindNoteProviderOld: function (Id, linkId, lblId, hfId, shortName) {

        if (!Id)
            Id = 'OldtxtProvider';
        if (!linkId)
            linkId = 'OldlnkProviderEdit';
        if (!lblId)
            lblId = 'OldlblProvider';
        if (!hfId)
            hfId = 'OldhfProviderId';

        if (!shortName)
            shortName = $("#pnlDashboard #pnlEncounterGridOld #OldtxtProvider").val();

        $("#pnlDashboard #pnlEncounterGridOld #OldhfProviderId").val("");
        $("#pnlDashboard #OldNoteProviderText").val($("#pnlDashboard #pnlEncounterGridOld #OldtxtProvider").val());



        utility.GetProviderArray(shortName).done(function (response) {

            $("#pnlDashboard #pnlEncounterGridOld #" + Id).autocomplete({
                autoFocus: true,
                source: response,
                select: function (event, ui) {
                    setTimeout(function () {

                        $("#pnlDashboard #pnlEncounterGridOld #" + Id).val(ui.item.value);
                        $("#pnlDashboard #OldNoteProviderText").val(ui.item.value)
                        $("#pnlDashboard #" + hfId).val(ui.item.id);
                        if ($("#pnlDashboard #pnlEncounterGridOld #" + linkId).css("display") == "none") {
                            $("#pnlDashboard #pnlEncounterGridOld #" + linkId).css("display", "inline");
                            $("#pnlDashboard #pnlEncounterGridOld #" + lblId).css("display", "none");
                        }

                    }, 100);

                }
            });
            //$("#" + Clinical_Notes.params.PanelID + " #frmClinicalNotes #" + Id).autocomplete("search");
        });

    },

    BindModifiedNotesProvider: function () {
        var Ctrl = $("#pnlDashboard #gridControlModifiedNote #txtNoteProvider");
        var func = function () {
            return utility.GetProviderArray(Ctrl.val())
        };
        var hfCtrl = $("#pnlDashboard #gridControlModifiedNote #hfNoteProviderId");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    BindCCMProvider: function () {
        var Ctrl = $("#pnlDashboard #pnlCCMEnrollmentInfoGrid #txtProvider");
        var func = function () {
            return utility.GetProviderArray(Ctrl.val())
        };
        var hfCtrl = $("#pnlDashboard #pnlCCMEnrollmentInfoGrid #hfProviderId");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    BindLiveRequestProvider: function () {
        var Ctrl = $("#pnlDashboard #pnlDataChangeRequestGrid #txtProvider");
        var func = function () {
            return utility.GetProviderArray(Ctrl.val())
        };
        var hfCtrl = $("#pnlDashboard #pnlDataChangeRequestGrid #hfProvider");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    BindPatientName: function () {
        var Ctrl = $("#pnlDashboard #pnlTCMGrid #txtFullName");
        var func = function () {
            return utility.GetPatientArrayByName(Ctrl.val(), 1)
        };
        var hfCtrl = $("#pnlDashboard #pnlTCMGrid #hfTCMPatientId");
        var onSelect = function (e) {
            utility.InsertRecentPatient(e.id);
        };
        utility.BindKendoAutoComplete(Ctrl, 3, "FullName", "contains", null, func, hfCtrl, onSelect);
    },

    BindPatientName_LiveRequest: function () {
        var Ctrl = $("#pnlDashboard #pnlDataChangeRequestGrid #txtFullName");
        var func = function () {
            return utility.GetPatientArrayByName(Ctrl.val(), 1)
        };
        var hfCtrl = $("#pnlDashboard #pnlDataChangeRequestGrid #hfPatientId");
        var onSelect = function (e) {
            utility.InsertRecentPatient(e.id);
        };
        utility.BindKendoAutoComplete(Ctrl, 3, "FullName", "contains", null, func, hfCtrl, onSelect);
    },

    BindDocumentPatientName: function () {
        var valid = false;
        var Ctrl = $("#pnlDashboard #pnlPatientDocumentGrid #txtFullName");
        var hfCtrl = $("#pnlDashboard #pnlPatientDocumentGrid #hfPatientId");
        if (Ctrl.data("kendoAutoComplete"))
            Ctrl.data("kendoAutoComplete").destroy();
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
                else {
                    return false;
                }
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
                if ($(Ctrl).val() != "")
                    utility.DisplayMessages($(Ctrl).attr('customName') + " is not Valid", 2);
            }
        };
        var onSelect = function (e) {
            var dataItem = this.dataItem(e.item.index());
            Ctrl.val(dataItem.FullName);
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
                        var AllPatients = utility.GetPatientArrayByName(Ctrl.val(), 1).done(function (response) {
                            $.each(response, function (i, item) {
                                item.value = item.FullName;
                            });
                            e.success(response);
                        });
                    },
                }
            },
        });
    },
    BindOrderPatientName: function () {
        var Ctrl = $("#pnlDashboard #ctrlPanLabOrder #txtOrderFullName");
        var func = function () {
            return utility.GetPatientArrayByName(Ctrl.val(), 1)
        };
        var hfCtrl = $("#pnlDashboard #ctrlPanLabOrder #hfPatientId");
        var onSelect = function (e) {
            utility.InsertRecentPatient(e.id);
        };
        utility.BindKendoAutoComplete(Ctrl, 3, "FullName", "contains", null, func, hfCtrl, onSelect);
    },
    BindResultPatientName: function () {
        var Ctrl = $("#pnlDashboard #LabResult #txtResultFullName");
        var func = function () {
            return utility.GetPatientArrayByName(Ctrl.val(), 1)
        };
        var hfCtrl = $("#pnlDashboard #LabResult #hfPatientId");
        var onSelect = function (e) {
            utility.InsertRecentPatient(e.id);
        };
        utility.BindKendoAutoComplete(Ctrl, 3, "FullName", "contains", null, func, hfCtrl, onSelect);
    },

    LoadDashBoardTCM: function (PageNumber, RowsPerPage, Status, isfrom) {

        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 15 : RowsPerPage;

        var objData = new JSON.constructor();
        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        if (isfrom == "search") {
            objData["Status"] = $('#pnlDashboard #pnlTCMGrid #ddlTCMStatus option:selected').text();
        } else if (isfrom == "switch") {
            if ($('#pnlDashboard #pnlTCMGrid #TCMSwitch .ios-switch').hasClass('on')) {
                objData["Status"] = 'All';
            } else {
                objData["Status"] = 'Draft';
            }
        } else if (isfrom == "Span") {
            objData["Status"] = Status;
        } else {
            if ($('#pnlDashboard #pnlTCMGrid #TCMSwitch .ios-switch').hasClass('on')) {
                objData["Status"] = 'All';
            } else {
                objData["Status"] = 'Draft';
            }
            // objData["Status"] = Status == null ? 'Draft' : Status;
        }
        objData["Status"] = objData["Status"].replace(/\s+/g, '');
        var TCMPatientId = $("#pnlDashboard #pnlTCMGrid #hfTCMPatientId").val() == "" || $("#pnlDashboard #pnlTCMGrid #hfTCMPatientId").val() == null ? null : $("#pnlDashboard #pnlTCMGrid #hfTCMPatientId").val();
        var TCMProviderId = $("#pnlDashboard #pnlTCMGrid #hfProvider").val() == "" || $("#pnlDashboard #pnlTCMGrid #hfProvider").val() == null ? null : $("#pnlDashboard #pnlTCMGrid #hfProvider").val();
        objData["PatientId"] = TCMPatientId;
        objData["ProviderId"] = TCMProviderId;
        objData["CommandType"] = "Search_TCM_Patients";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "SearchDashBoard");

    },
    LoadDashBoardDataChangeRequest: function (PageNumber, RowsPerPage) {

        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 15 : RowsPerPage;

        var objData = new JSON.constructor();
        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        // objData["Status"]=   $("#pnlDashboard #pnlDataChangeRequestGrid #ddlDataChangeRequestStatus").text()
        objData["Status"] = $('#pnlDashboard #pnlDataChangeRequestGrid #ddlDataChangeRequestStatus option:selected').text();



        objData["Status"] = objData["Status"].replace(/\s+/g, '');
        var DataChangeRequestPatientId = $("#pnlDashboard #pnlDataChangeRequestGrid #hfPatientId").val() == "" || $("#pnlDashboard #pnlDataChangeRequestGrid #hfPatientId").val() == null ? null : $("#pnlDashboard #pnlDataChangeRequestGrid #hfPatientId").val();
        var DataChangeRequestProviderId = $("#pnlDashboard #pnlDataChangeRequestGrid #hfProvider").val() == "" || $("#pnlDashboard #pnlDataChangeRequestGrid #hfProvider").val() == null ? null : $("#pnlDashboard #pnlDataChangeRequestGrid #hfProvider").val();
        objData["PatientId"] = DataChangeRequestPatientId;
        objData["ProviderId"] = DataChangeRequestProviderId;
        objData["CommandType"] = "search_checkin_patients";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "SearchDashBoard");

    },

    LoadDashBoardCheckInRequest: function (PageNumber, RowsPerPage) {

        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 15 : RowsPerPage;

        var objData = new JSON.constructor();
        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        // objData["Status"]=   $("#pnlDashboard #pnlDataChangeRequestGrid #ddlDataChangeRequestStatus").text()
        objData["Status"] = $('#pnlDashboard #pnlDataChangeRequestGrid #ddlDataChangeRequestStatus option:selected').text();



        objData["Status"] = objData["Status"].replace(/\s+/g, '');
        var DataChangeRequestPatientId = $("#pnlDashboard #pnlDataChangeRequestGrid #hfPatientId").val() == "" || $("#pnlDashboard #pnlDataChangeRequestGrid #hfPatientId").val() == null ? null : $("#pnlDashboard #pnlDataChangeRequestGrid #hfPatientId").val();
        var DataChangeRequestProviderId = $("#pnlDashboard #pnlDataChangeRequestGrid #hfProvider").val() == "" || $("#pnlDashboard #pnlDataChangeRequestGrid #hfProvider").val() == null ? null : $("#pnlDashboard #pnlDataChangeRequestGrid #hfProvider").val();
        objData["PatientId"] = DataChangeRequestPatientId;
        objData["ProviderId"] = DataChangeRequestProviderId;
        objData["CommandType"] = "search_checkin_patients_Request";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "SearchDashBoard");

    },
    LoadDashBoardCopay: function (PageNumber, RowsPerPage) {

        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 15 : RowsPerPage;

        var objData = new JSON.constructor();
        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        objData["CommandType"] = "Search_Copay";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "SearchDashBoard");
    },

    LoadDashBoardPatientChanges: function (PatientData, PageNumber, RowsPerPage) {

        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 15 : RowsPerPage;

        var objData = new JSON.constructor();
        if (PatientData)
            objData = JSON.parse(PatientData);

        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        objData["CommandType"] = "Search_Patient_Charges";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "SearchDashBoard");

    },


    LoadDashBoardLabOrder: function (LabOrderData, LabOrderId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = {
        };
        if (LabOrderData != null) {
            objData = JSON.parse(LabOrderData);
        }
        objData["LabOrderId"] = LabOrderId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;

        objData["LabId"] = $('#pnlDashboard #ddlLabId').val();
        if (objData["LabId"] == null) {
            objData["LabId"] = '';
        }

        objData["OrderFromDate"] = $('#pnlDashboard #dpStartDate').val();
        objData["Status"] = $('#pnlDashboard #pnlLabOrder #ddlStatus option:selected').val();
        objData["OrderToDate"] = $('#pnlDashboard #dpToDate').val();
        objData["PatientId"] = $('#pnlDashboard #ctrlPanLabOrder #hfPatientId').val();
        //objData["commandType"] = "search_Laborders";
        objData["commandType"] = "search_laborders_dashboard";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabOrder", "LabOrder");
    },

    bindAutoComplete: function (element, refCtrlId) {

        var hiddenCrtl = $('#' + refCtrlId);
        DashBoard.params = [];
        DashBoard.params["PanelID"] = "LabResult";
        //  utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "Clinical_LabOrder", null, true);
        EMRUtility.BindLOINCCodes(hiddenCrtl, "DashBoard");
    },

    validateSpecialCharacters: function (event) {
        var valid = (event.which >= 48 && event.which <= 57) || (event.which >= 65 && event.which <= 90) || (event.which >= 97 && event.which <= 122);
        if (!valid) {
            event.preventDefault();
        }

    },

    LoadDashBoardLabResult: function (LabOrderData, LabOrderId, PageNumber, RowsPerPage, IsAssignedResultsFromQuick) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = {
        };
        if (LabOrderData != null) {
            objData = JSON.parse(LabOrderData);
        }

        objData["LabOrderId"] = LabOrderId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;

        objData["LabId"] = $('#pnlDashboard #ddllabResultLabId').val();

        objData["Test"] = $('#pnlDashboard  #txtlabResultCPTCode').val();
        objData["OrderNo"] = $('#pnlDashboard  #txtlabResultOrderNumber').val();
        objData["OrderFromDate"] = $('#pnlDashboard  #dplabResultStartDate').val();
        objData["OrderToDate"] = $('#pnlDashboard  #dplabResultToDate').val();
        //objData["OrderFromDate"] = $('#' + DashBoard.params.PanelID + ' #frmClinicalLabOrder #dpStartDate').val();
        objData["Status"] = $('#pnlDashboard #ddllabResultStatus option:selected').val();
        //objData["OrderToDate"] = $('#' + DashBoard.params.PanelID + ' #frmClinicalLabOrder #dpToDate').val();

        //  objData["commandType"] = "search_LabResults";
        // added by faizan ameen..
        $("#pnlDashboard  #dplabResultStartDate").datepicker().on('change', function () {
            $('.datepicker').hide();
        });

        $("#pnlDashboard  #dplabResultToDate").datepicker().on('change', function () {
            $('.datepicker').hide();
        });

        if ($("#ddlUnsoResultReviewStatus").val() == "") {
            objData["CountIsReviewed"] = false;
            objData["isReviewedDashBoardUnsolicited"] = true;
        } else if ($("#ddlUnsoResultReviewStatus").val() == "1") {
            objData["CountIsReviewed"] = true;
            objData["isReviewedDashBoardUnsolicited"] = false;
        } else {
            objData["CountIsReviewed"] = false;
            objData["isReviewedDashBoardUnsolicited"] = false;
        }

        //PRD-785 START
        if (DashBoard.params.IsAssignedResultsFromQuick == true) {
            objData["AssigneeId"] = globalAppdata.AppUserId;
            $("#pnlDashboard #LabResult #ddlReviewStatus").val('0');
            objData["IsReviewed"] = false;
            objData["isReviewedFromDashBoard"] = false;
        }
        else {
            objData["AssigneeId"] = $('#pnlDashboard #LabResult #ddlAssignedUsers').val();

            if ($("#ddlReviewStatus").val() == "") {
                objData["IsReviewed"] = $('#pnlDashboard #LabResult #chkIsLabResultReviewed').is(':checked');
                objData["isReviewedFromDashBoard"] = true;
            } else if ($("#ddlReviewStatus").val() == "1") {
                objData["IsReviewed"] = true;
                objData["isReviewedFromDashBoard"] = false;
            } else {
                objData["IsReviewed"] = false;
                objData["isReviewedFromDashBoard"] = false;
            }
        }
        //PRD-785 END
        objData["ProviderId"] = $('#pnlDashboard #LabResult #hfProvider').val();
        objData["PatientPortalStatus"] = $('#pnlDashboard #LabResult #ddlResultPatPortalStatus').val();
        objData["IsAllResult"] = $('#pnlDashboard #LabResult #switchLabResultAbnormal').is(':checked');
        objData["PatientId"] = $('#pnlDashboard #LabResult #hfPatientId').val();
        objData["CountStatus"] = $('#pnlDashboard #ddllabUnsoResultStatus option:selected').val();
        objData["CountLabId"] = $('#pnlDashboard #ddllabUnsoResultLaboratory').val();
        objData["CountOrderFromDate"] = $('#pnlDashboard  #dplabUnsoResultStartDate').val();
        objData["CountOrderToDate"] = $('#pnlDashboard  #dplabUnsoResultToDate').val();
        //objData["CountIsReviewed"] = $('#pnlDashboard #LabUnsolicited #chkIsLabUnsoResultReviewed').is(':checked');
        objData["CountIsAllResult"] = $('#pnlDashboard #LabUnsolicited #switchLabUnsoResultAbnormal').is(':checked');
        if (!$.trim($('#pnlDashboard #LabUnsolicited #divSwitchLabUnsoResultAbnormal').html()).length) {
            objData["CountIsAllResult"] = true;
        }

        objData["commandType"] = "search_labresults_dashboard"
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },

    DashBoardLabUnsolicitedResultGridLoad: function (response, rpp, pageNo) {

        pageNo = (pageNo == null || pageNo == "") ? 1 : pageNo;
        rpp = (rpp == null || rpp == "") ? 15 : rpp;

        var gridctl = $("#LabUnsolicited").data("kendoGrid");
        var userLabOrderment = {
            data: [], total: 0
        };
        var dataSource;
        DashBoard.LoadDashBoardLabResultAbnormalCount();

        DashBoard.LoadDashBoardLabUnsolicitedResult('{}', 0, pageNo, rpp).done(function (response) {
            response = JSON.parse(response)
            if (response.status != false) {
                // if (response.LabResultCount > 0) {
                if (response.LabOrderLoad_JSON != undefined)
                    DashBoard.LabUnsolicitedResultGridLoad(response);
                userLabOrderment.data = JSON.parse(response.LabOrderLoad_JSON);

                var TableControl = "pnlDashboard  #LabUnsolicited #dgvLabUnsoOrderResult";
                var PagingPanelControlID = "pnlDashboard #dgvLabUnsoResultDashboard_Paging";
                var ClassControlName = "DashBoard";
                var PagesToDisplay = 5;

                // added by faizan ameen.
                $("#pnlDashboard  #dplabUnsoResultStartDate").datepicker().on('change', function () {
                    $('.datepicker').hide();
                });

                $("#pnlDashboard  #dplabUnsoResultToDate").datepicker().on('change', function () {
                    $('.datepicker').hide();
                });
                // End of code added by faizan ameen.


                var iTotalDisplayRecords = userLabOrderment.total = response.iTotalDisplayRecords;
                setTimeout(
                    CreatePagination(response.LabResultCount, pageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        DashBoard.DashBoardLabUnsolicitedResultGridLoad(PrimaryID, ResultPerPage, PageNumber);
                    }), 10);
                $('#pnlDashboard #LabUnsolicited .badge').css("display", "none");
                //$('#pnlDashboard #LabUnsolicited .badge').text(response.iTotalDisplayRecords);
                //} else {
                //    //$('#pnlDashboard #LabUnsolicited .badge').css("display", "none");
                //}
            }
            else
                utility.DisplayMessages(response.Message, 3);

        });



    },

    DashBoardLabUnsolicitedResultLoad: function (response, rpp, pageNo) {
        if (!$.trim($('#pnlDashboard #LabUnsolicited #divSwitchLabUnsoResultAbnormal').html()).length) {
            var HtmlOfSwitch = '<span class="pr-xs">Abnormal</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                                     '<input id="switchLabUnsoResultAbnormal" isactive="1" type="checkbox" checked="checked" data-plugin-ios-switch="" style="display: none;" onchange="DashBoard.DashBoardLabUnsolicitedResultGridLoad(null, null, null);">' +
                                      '</div><span class="pl-xs">All</span>';

            $('#pnlDashboard #LabUnsolicited #divSwitchLabUnsoResultAbnormal').html(HtmlOfSwitch);

        }
        DashBoard.DashBoardLabUnsolicitedResultGridLoad(response, rpp, pageNo);
    },

    LoadDashBoardLabUnsolicitedResult: function (LabOrderData, LabOrderId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = {
        };
        if (LabOrderData != null) {
            objData = JSON.parse(LabOrderData);
        }
        objData["LabOrderId"] = LabOrderId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;

        objData["Test"] = $('#pnlDashboard  #txtlabResultCPTCode').val();
        objData["OrderNo"] = $('#pnlDashboard  #txtlabResultOrderNumber').val();
        objData["OrderFromDate"] = $('#pnlDashboard  #dplabUnsoResultStartDate').val();
        objData["OrderToDate"] = $('#pnlDashboard  #dplabUnsoResultToDate').val();
        //objData["OrderFromDate"] = $('#' + DashBoard.params.PanelID + ' #frmClinicalLabOrder #dpStartDate').val();
        objData["Status"] = $('#pnlDashboard #ddllabUnsoResultStatus option:selected').val();
        //objData["OrderToDate"] = $('#' + DashBoard.params.PanelID + ' #frmClinicalLabOrder #dpToDate').val();

        //  objData["commandType"] = "search_LabResults";

        objData["LabId"] = $('#pnlDashboard #ddllabUnsoResultLaboratory').val();
        //objData["IsReviewed"] = $('#pnlDashboard #LabUnsolicited #chkIsLabUnsoResultReviewed').is(':checked');
        objData["IsAllResult"] = $('#pnlDashboard #LabUnsolicited #switchLabUnsoResultAbnormal').is(':checked');

        objData["CountStatus"] = $('#pnlDashboard #ddllabResultStatus option:selected').val();
        objData["CountLabId"] = $('#pnlDashboard #ddllabResultLabId').val();
        objData["CountOrderFromDate"] = $('#pnlDashboard  #dplabResultStartDate').val();
        objData["CountOrderToDate"] = $('#pnlDashboard  #dplabResultToDate').val();
        //objData["CountIsReviewed"] = $('#pnlDashboard #LabResult #chkIsLabResultReviewed').is(':checked');
        objData["CountIsAllResult"] = $('#pnlDashboard #LabResult #switchLabResultAbnormal').is(':checked');
        if (!$.trim($('#pnlDashboard #LabResult #divSwitchLabResultAbnormal').html()).length) {
            objData["CountIsAllResult"] = true;
        }
        if ($("#ddlUnsoResultReviewStatus").val() == "") {
            objData["IsReviewed"] = false;
            objData["isReviewedDashBoardUnsolicited"] = true;
        } else if ($("#ddlUnsoResultReviewStatus").val() == "1") {
            objData["IsReviewed"] = true;
            objData["isReviewedDashBoardUnsolicited"] = false;
        } else {
            objData["IsReviewed"] = false;
            objData["isReviewedDashBoardUnsolicited"] = false;
        }

        if ($("#ddlReviewStatus").val() == "") {
            objData["CountIsReviewed"] = $('#pnlDashboard #LabResult #chkIsLabResultReviewed').is(':checked');
            objData["isReviewedFromDashBoard"] = true;
        } else if ($("#ddlReviewStatus").val() == "1") {
            objData["CountIsReviewed"] = true;
            objData["isReviewedFromDashBoard"] = false;
        } else {
            objData["CountIsReviewed"] = false;
            objData["isReviewedFromDashBoard"] = false;
        }

        objData["commandType"] = "search_labunsolicitedresults_dashboard"
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },
    checkUncheckAllMessages: function (obj) {
        var tableId = $(obj.parentNode.parentNode.parentNode.parentNode).attr("id");
        if ($(obj).is(':checked')) {
            $("#pnlDashboard #pnlUserMessagesGrid #" + tableId + " input[name='SelectCheckBoxOrder']:checkbox").prop('checked', true);
        } else {
            $("#pnlDashboard #pnlUserMessagesGrid #" + tableId + " input[name='SelectCheckBoxOrder']:checkbox").prop('checked', false);
        }


    },
    checkUncheckMessage: function (event) {
        event.stopPropagation();
    },
    LabGridLoad: function (response) {

        //Start//Abid Ali
        DashBoard.labOrderRows = [];
        //End//Abid Ali
        if ($.fn.dataTable.isDataTable("#pnlDashboard  #dgvLabOrderGrid #dgvLabOrderDashboard")) {
            $("#pnlDashboard  #dgvLabOrderGrid #dgvLabOrderDashboard").dataTable().fnClearTable();
            $("#pnlDashboard  #dgvLabOrderGrid #dgvLabOrderDashboard").dataTable().fnDestroy();
        }
        $("#pnlDashboard  #dgvLabOrderGrid #dgvLabOrderDashboard tbody").find("tr").remove();
        if ($("#pnlDashboard  #dgvLabOrderGrid #dgvLabOrderDashboard thead tr #selectOrders").length == 0) {
            $("#pnlDashboard  #dgvLabOrderGrid #dgvLabOrderDashboard thead tr").prepend('<th class="noWordBreak size70 size-min70"><input type="checkbox" onchange="DashBoard.checkUncheckAllOrders(this);" controlname="selectOrders" id="selectOrders" name="chkHeader" class="input-block pull-left ml-xs" coltype="checkbox"/></th>');
        } else {
            $("#pnlDashboard  #dgvLabOrderGrid #dgvLabOrderDashboard thead tr #selectOrders").prop('checked', false);
        }
        if (response.LabOrderCount > 0) {
            var LabLoadJSONData = JSON.parse(response.LabOrderFill_JSON);//JSON.parse(response.LabLoad_JSON); //Parsing array to JSON
            $.each(LabLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", "gvLab_row" + item.LabOrderId);
                $row.attr("LabId", item.LabOrderId);
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
                var SelectionCheckBoxColumn = '<a class="btn btn-xs" role="button" onclick="DashBoard.OnClickPreventDefault(event);" title="Select Lab Order"><input type="checkbox" id=' + item.LabOrderId + '  name="SelectCheckBoxOrder"  class="input-block"/></a>';
                var Checked = "";

                var onclick = "DashBoard.labOrderRowExpand(this,event);";
                var onCellClick = "DashBoard.labOrderRowExpand(this,event,'cell');";
                var expandCollapseIcon = '<a id="lnkExpand" href="#" onclick="' + onclick + '" class="tab_space" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a>';

                //if (DashBoard.params.ParentCtrl == "clinicalTabProgressNote") {
                //    SelectionCheckBoxColumn = '<td><input type="checkbox" onclick="DashBoard.enableAddOrder(this,event);" id="' + item.LabOrderId + '" name="SelectCheckBoxOrder" ' + Checked + ' class="input-block"/></td>';
                //} else {
                //    SelectionCheckBoxColumn = "";
                //}
                var Method = "";
                var Action = "";
                if (item.LabOrderResultLatestNoteModel != null) {
                    Method = 'DashBoard.EditProgressNote(' + item.LabOrderResultLatestNoteModel.NoteId + ', ' + item.PatientId + ');';
                    if (item.LabOrderResultLatestNoteModel.NoteStatus != "Signed") {
                        Action = '<a href="javascript:void(0);" onclick="' + Method + '"  title="Edit Note">Edit Note</a>';
                    }
                    else {
                        Method = "DashBoard.NotesPreview('" + item.LabOrderResultLatestNoteModel.NoteId + "', '" + item.PatientId + "', '" + item.LabOrderResultLatestNoteModel.ProviderId + "');"
                        Action = '<a href="javascript:void(0);" onclick="' + Method + '"  title="Edit Note">Preview Note</a>';
                    }
                }

                var divLoinc = "";
                var totalTests = 1;
                if (item.Test.indexOf('|') > -1) {
                    totalTests = item.Test.split('|').length;
                }

                for (var i = 0; i < totalTests; i++) {
                    if (totalTests > 0) {
                        divLoinc += item.Test.split('|')[i] + "<br />";
                    }
                }
                if (divLoinc != "") {
                    divLoinc = divLoinc.trim();
                    //divLoinc = divLoinc.replace(/,\s*$/, "");
                }

                if (item.Status == "Transmitted") {
                    $row.append('<td style="display:none;">' + item.LabOrderId + '</td><td>' + SelectionCheckBoxColumn + '&nbsp;<a class="btn  btn-xs" href="#" onclick="DashBoard.printLabOrder(\'' + item.LabOrderId + '\',\'' + item.Status + '\',event, \'' + item.PatientId + '\');" title="View Lab Order"> <i class="fa fa-credit-card blue"></i></a></td><td id=\'' + item.PatientId + '\'> <a href="#" onclick="utility.PatientDemographics(\'' + item.PatientId + '\', \'mstrTabDashBoard\', event);">' + item.PatientName + '</a><a class="btn btn-xs hidden" href="#" onclick="DashBoard.EditPatientLab(this,event);" title="Edit Record"><i class="fa fa-edit black"></i></a><div class=""><div id="divtxtCPT709085" class="input-group" title="" data-toggle="tooltip" data-placement="right" ><div class="input-group divChangePatient hidden" ><input class="form-control ui-autocomplete-input" id="txtAccountNo" customname="Patient Name" type="text" name="patientAccount" oninput="DashBoard.BindPatientAccount(this,\'DashBoardLabOrderResults\');" onblur="" data-bv-field="patientAccount" autocomplete="off"><div class="input-group-btn"><button id="lnkPatientAccount" type="button" class="btn btn-primary btn-xs" onclick="DashBoard.OpenPatientAccount(\'Pat_' + item.LabOrderId + '\',this);"><i class="fa fa-search"></i></button></div></div><input type="hidden" value="' + item.PatientId + '" id="Pat_' + item.LabOrderId + '"/></td><td>'
                    + utility.RemoveTimeFromDate(null, item.OrderDate) + ' ' + item.OrderTime + '</td><td onclick="' + onCellClick + '">' + expandCollapseIcon + divLoinc + '</td><td>' + item.LabName + '</td><td>' + item.OrderNo + '</td><td>' + item.Status + '</td><td>' + item.Provider + '</td><td>' + item.AssigneeName + '</td><td>' + Action + '</td>');
                }
                else {

                    $row.append('<td style="display:none;">' + item.LabOrderId + '</td><td>' + SelectionCheckBoxColumn + '<a class="btn btn-xs disableAll" href="#" onclick="Clinical_LabOrder.LabOrderAddEdit(\'' + item.LabOrderId + '\',\'mstrTabDashBoard\',event, \'pnlDashboard\');" title="View Lab Order"> <i class="fa fa-credit-card blue"></i></a></td><td id=\'' + item.PatientId + '\'> <a href="#" onclick="utility.PatientDemographics(\'' + item.PatientId + '\', \'mstrTabDashBoard\', event);">' + item.PatientName + '</a><a class="btn btn-xs hidden" href="#" onclick="DashBoard.EditPatientLab(this,event);" title="Edit Record"><i class="fa fa-edit black"></i></a><div class=""><div id="divtxtCPT709085" class="input-group" title="" data-toggle="tooltip" data-placement="right" ><div class="input-group divChangePatient hidden" ><input class="form-control ui-autocomplete-input" id="txtAccountNo" customname="Patient Name" type="text" name="patientAccount" oninput="DashBoard.BindPatientAccount(this,\'DashBoardLabOrderResults\');" onblur="" data-bv-field="patientAccount" autocomplete="off"><div class="input-group-btn"><button id="lnkPatientAccount" type="button" class="btn btn-primary btn-xs" onclick="DashBoard.OpenPatientAccount(\'Pat_' + item.LabOrderId + '\',this);"><i class="fa fa-search"></i></button></div></div><input type="hidden" value="' + item.PatientId + '" id="Pat_' + item.LabOrderId + '"/></td><td>'
                        + utility.RemoveTimeFromDate(null, item.OrderDate) + ' ' + item.OrderTime + '</td><td onclick="' + onCellClick + '">' + expandCollapseIcon + divLoinc + '</td><td>' + item.LabName + '</td><td>' + item.OrderNo + '</td><td>' + item.Status + '</td><td>' + item.Provider + '</td><td>' + item.AssigneeName + '</td><td>' + Action + '</td>');
                }
                $("#pnlDashboard  #dgvLabOrderGrid #dgvLabOrderDashboard tbody").last().append($row);

                var childRows = DashBoard.buildLabOrderRowChild(item.LabOrderTests, item.LabOrderId);
                DashBoard.labOrderRows.push({
                    row: $row, childs: childRows
                });
                //Bind values
                //childRows.loadDropDowns(true).done(function () {
                //    //Bind Values to the child row
                utility.bindMyJSONByName(true, item, false, childRows).done(function () {
                    childRows.find('#orderNo').text("Order Number: " + item.OrderNo);
                    childRows.find('#laboratory').text("Lab: " + item.LabName);

                });
                //});
                //CacheManager.BindDropDownsByID(childRows.find('#ddlAssigneeId'), 'GetUsers', true, 1).done(function () {
                //    childRows.find('#ddlAssigneeId').find("option:first").text('- Select -');
                //});


            });

        }
        else {

            $("#pnlDashboard  #dgvLabOrderGrid #dgvLabOrderDashboard").DataTable({
                "destroy": true,
                "language": {
                    "emptyTable": "No Lab Order Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{
                    "bSortable": false, "aTargets": [0]
                }]
            });
        }
        if ($.fn.dataTable.isDataTable("#pnlDashboard  #dgvLabOrderGrid #dgvLabOrderDashboard"))
            ;
        else {
            DashBoard.EditableGridOrder = $("#pnlDashboard  #dgvLabOrderGrid #dgvLabOrderDashboard").DataTable({
                "destroy": true,
                "aaSorting": [],
                "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{
                    "bSortable": false, "aTargets": [1], "orderable": false, "aTargets": [0]
                }]
            });
        }
        //Hide Data Table default search
        $("#pnlDashboard  #dgvLabOrderGrid").find(".dataTables_filter").hide();

        // Manually trigger custom search of data table
        $("#pnlDashboard  #pnlLabOrderGrid").find("#dgvLabOrderGrid_Search").on("keyup", function () {
            DashBoard.EditableGridOrder.search(this.value).draw();
        });



        EMRUtility.fixDataTableDuplication("#pnlDashboard  #dgvLabOrderGrid");

        // $("#pnlDashboard  #dgvLabOrderGrid").css('display', "")
        // $("#pnlDashboard #listLabOrderPending").trigger('click');

        $.each(DashBoard.labOrderRows, function (i, item) {

            if (DashBoard.EditableGridOrder != null) {

                var row = DashBoard.EditableGridOrder.row(item.row);
                if (item.childs.length > 0) {
                    row.child(item.childs);
                    if (globalAppdata['IsOrdersExpand'] && globalAppdata['IsOrdersExpand'].toLowerCase() == 'true') {
                        var expandObj = item.row.find("a#lnkExpand");
                        if (expandObj.length > 0) {
                            DashBoard.labOrderRowExpandAll(expandObj, null);
                        }
                    }
                }
                else {
                    //$(item.row).find('td:first').find('a').hide();
                }
            }
        });
    },

    SaveLabResult: function (btnSave, orderNo, patientId, providerId, event) {

        event.preventDefault();

        var ContainerDiv = $(btnSave).closest('div.panel-body');

        var Obj = $(ContainerDiv).getMyJSONByName();
        Obj = JSON.parse(Obj);




        var LabId = $(ContainerDiv).closest('tr').prev().attr('labid');
        var IsAknowledged = $(ContainerDiv).closest('tr').prev().attr('isaknowledged');
        Obj["LabOrderId"] = LabId;
        Obj["callFromGrid"] = "True";
        Obj["IsAknowledged"] = IsAknowledged;
        Obj["OrderNo"] = orderNo;
        Obj["PatientId"] = patientId;
        Obj["ProviderId"] = providerId;


        Obj["IsSentToPortal"] = Obj["SentToPortal"];

        Obj["commandType"] = "SAVE_LABRESULT";
        Obj["Comments"] = $(ContainerDiv).find("#txtComment").html();
        DashBoard.LabResultSave(Obj).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if ($('#pnlDashboard #listLabUnsolicited').hasClass('active')) {
                    DashBoard.DashBoardLabUnsolicitedResultGridLoad();
                } else if ($('#pnlDashboard #listLabResult').hasClass('active')) {
                    DashBoard.DashBoardLabResultGridLoad(null, null, null);
                }
                utility.DisplayMessages(response.message, 1);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });


    },

    LabResultSave: function (LabOrderData) {

        var data = JSON.stringify(LabOrderData);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },
    LabResultAcknowledge: function (LabResultId) {
        var Obj = new Object();
        Obj["commandType"] = "ACKNOWLEDGE_LABRESULT";
        Obj["LabResultId"] = LabResultId;
        var data = JSON.stringify(Obj);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },
    LabResultGridLoad: function (response) {

        //Start//Abid Ali
        DashBoard.labResultRows = [];
        //End//Abid Ali


        if ($.fn.dataTable.isDataTable("#pnlDashboard  #LabResult #dgvLabOrderResult")) {
            $("#pnlDashboard  #LabResult #dgvLabOrderResult").dataTable().fnClearTable();
            $("#pnlDashboard  #LabResult #dgvLabOrderResult").dataTable().fnDestroy();
        }

        $("#pnlDashboard  #LabResult #dgvLabOrderResult tbody").find("tr").remove();
        if ($("#pnlDashboard  #LabResult #dgvLabOrderResult thead tr #selectResults").length == 0) {
            $("#pnlDashboard  #LabResult #dgvLabOrderResult thead tr").prepend('<th class="noWordBreak size100 size-min100"><input type="checkbox" onchange="DashBoard.checkUncheckAllResults(this);" controlname="selectResults" id="selectResults" name="chkHeader" class="input-block pull-left ml-xs" coltype="checkbox"/></th>');
        } else {
            $("#pnlDashboard  #LabResult #dgvLabOrderResult thead tr #selectResults").prop('checked', false);
        }

        if (response.LabResultCount > 0) {
            // Start PRD-423
            $('#pnlDashboard #comments-remarks #spnPatientPortalStatus').show();
            $('#pnlDashboard #comments-remarks #lblPatientPortalStatus').show();
            // End PRD-423
            //DashBoard.LabResultCount = response.iTotalDisplayRecords;
            var TotalCount = response.UnsolicitedPlusSolicited;
            $("#pnlDashboard div[class*='wOrders'] .badge").css("display", "inline");
            $("#pnlDashboard div[class*='wOrders'] .badge").text(TotalCount);
            $('#wpanel .slick-track div').each(function (i) {
                if ($(this).find('span:first').text() == 'Orders & Results') {
                    $(this).find('span:last').text(TotalCount);
                    $(this).find('span:last').show();
                }
            });
            $("#pnlDashboard #spnDashboard_LabOrderCount").text(TotalCount);
            if (JSON.parse(response.LabOrderLoad_JSON)[0] != undefined) {
                $("#pnlDashboard #spnListLabResultCount").text(JSON.parse(response.LabOrderLoad_JSON)[0].RecordCount);
            }




            var LabLoadJSONData = JSON.parse(response.LabOrderResultModel_JSON)//JSON.parse(response.LabOrderLoad_JSON); //Parsing array to JSON

            $("#pnlDashboard  #LabResult #dgvManulayyMappedRecord").addClass("hidden");

            $.each(LabLoadJSONData, function (i, item) {

                var currentDate = new Date();
                var currentTime = currentDate.toLocaleTimeString().replace(/([\d]+:[\d]{2})(:[\d]{2})(.*)/, "$1$3");

                //Convert Date to prper date time format
                if (item.ModifiedOn != null) {
                    currentDate = new Date(item.ModifiedOn).toLocaleDateString();
                    currentTime = new Date(item.ModifiedOn).toLocaleTimeString().replace(/([\d]+:[\d]{2})(:[\d]{2})(.*)/, "$1$3");
                }

                var CollectionDate = "", CollectionTime = "";
                if (item.CollectionDateTime != null) {
                    CollectionDate = new Date(item.CollectionDateTime).toLocaleDateString();
                    CollectionTime = new Date(item.CollectionDateTime).toLocaleTimeString().replace(/([\d]+:[\d]{2})(:[\d]{2})(.*)/, "$1$3");
                }

                var $row = $('<tr/>');
                $row.attr("id", "gvLab_row" + item.LabResultId);
                $row.attr("LabOrderResultId", item.LabResultId);
                $row.attr("labid", item.LabOrderId);
                $row.attr("isaknowledged", item.IsAknowledged);
                var editMode = 'onclick="DashBoard.LabResultAddEdit(' + item.LabResultId + ',' + item.LabOrderId + ',event,' + item.PatientId + ');"';
                var SelectionCheckBoxColumn = '<a class="btn btn-xs" role="button" onclick="DashBoard.OnClickPreventDefault(event);" title="Select Lab Result"><input type="checkbox" id=' + item.LabResultId + ' name="SelectCheckBoxResult" class="input-block chklabResult"/></a>';
                var Checked = "";

                parentControl = 'DashBoard';
                parentControlPanelID = 'pnlDashboard';
                var divLoinc = '';
                try {
                    //divLoinc = item.LabOrderTests.length > 0 ? item.LabOrderTests[0].CPTCode + " " + item.LabOrderTests[0].CPTDescription : "";
                    for (var i = 0; i < item.LabOrderTests.length; i++) {
                        if (item.LabOrderTests.length > 0) {
                            divLoinc += item.LabOrderTests[i].CPTDescription + "<br />";
                        }
                    }
                    if (divLoinc != "")
                        divLoinc = divLoinc.trim();
                } catch (ex) {
                    console.log(ex);
                }


                // currentTime = "";

                var onclick = "DashBoard.labOrderResultRowExpand(this,event);";
                var onCellClick = "DashBoard.labOrderResultRowExpand(this,event, 'cell');";
                var expandCollapseIcon = '<a id="lnkResExpand" href="#" onclick="' + onclick + '" class="tab_space" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a>';
                var Method = "";
                var Action = "";
                if (item.LabOrderResultLatestNoteModel != null) {
                    Method = 'DashBoard.EditProgressNote(' + item.LabOrderResultLatestNoteModel.NoteId + ', ' + item.PatientId + ');';
                    if (item.LabOrderResultLatestNoteModel.NoteStatus != "Signed") {
                        Action = '<a href="javascript:void(0);" onclick="' + Method + '"  title="Edit Note">Edit Note</a>';
                    }
                    else {
                        Method = "DashBoard.NotesPreview('" + item.LabOrderResultLatestNoteModel.NoteId + "', '" + item.PatientId + "', '" + item.LabOrderResultLatestNoteModel.ProviderId + "');"
                        Action = '<a href="javascript:void(0);" onclick="' + Method + '"  title="Edit Note">Preview Note</a>';
                    }
                }
                var comments = "";
                var commentsMethod = "Clinical_LabOrder.AddComments('" + item.LabResultId + "');";
                if (item.Comments == "<div></div>")
                    item.Comments = "";
                var decodedStripedHtml = "";
                var decodedStripedHtml = $("<div/>").html(item.Comments).text();
                comments = `<a href="#" id='comment_${item.LabResultId}' onclick="${commentsMethod}" data-toggle="tooltip" data-placement="right" title='${decodedStripedHtml.trim()}'><i class="fa fa-commenting blue"></i></a>`;
                if (item.IsAknowledged) {
                    $row.append('<td style="display:none;">' + item.LabResultId + '</td><td>' + SelectionCheckBoxColumn + '&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_LabOrder.printLabResult(\'' + item.LabOrderId + '\',\'' + item.LabResultId + '\',\'' + item.Status + '\',event, \'' + item.PatientId + '\');" title="View Lab Result"> <i class="fa fa-credit-card blue"></i></a><a class="btn btn-xs" href="#" onclick="Clinical_LabOrder.openLabTrends(\'' + item.LabResultId + '\',\'' + item.LabOrderId + '\',\'' + item.PatientFullName + ' (' + item.AccountNumber + ') ' + utility.RemoveTimeFromDate(null, item.DOB) + '\' , event);" title="Trend Record"><i class="fa fa-line-chart green"></i></a>&nbsp;' + comments + '</td><td id=\'' + item.PatientId + '\'> <a href="#" onclick="utility.PatientDemographics(\'' + item.PatientId + '\', \'mstrTabDashBoard\', event);">' + item.PatientFullName + '</a><a class="btn btn-xs" href="#" onclick="DashBoard.EditPatientLab(this,event);" title="Edit Record"><i class="fa fa-edit black"></i></a><div class=""><div id="divtxtCPT709085" class="input-group" title="" data-toggle="tooltip" data-placement="right" ><div class="input-group divChangePatient hidden" ><input class="form-control ui-autocomplete-input" id="txtAccountNo" customname="Patient Name" type="text" name="patientAccount" oninput="DashBoard.BindPatientAccount(this,\'DashBoardLabOrderResults\',null,null,null,\'' + item.LabOrderId + '\',\'LabResultUnsolicited\');" onblur="" data-bv-field="patientAccount" autocomplete="off"><div class="input-group-btn"><button id="lnkPatientAccount" type="button" class="btn btn-primary btn-xs" onclick="DashBoard.OpenPatientAccount(\'Pat_' + item.LabResultId + '\',this);"><i class="fa fa-search"></i></button></div></div><input type="hidden" value="' + item.PatientId + '" id="Pat_' + item.LabResultId + '"/></td><td ' + editMode + '>'
                     + CollectionDate + " " + CollectionTime + '</td><td ' + editMode + '>' + currentDate + ' ' + currentTime + '</td><td onclick="' + onCellClick + '">' + expandCollapseIcon + divLoinc + '</td><td ' + editMode + '>' + item.LabName + '</td><td ' + editMode + '>' + item.OrderNo + '</td><td ' + editMode + '>' + item.Status + '</td><td ' + editMode + '>' + item.Provider + '</td><td ' + editMode + '>' + item.AssigneeName + '</td><td>' + Action + '</td><td class="hidden">' + Number(new Date(currentDate + ' ' + currentTime)) + '</td>');
                }
                else {
                    $row.append('<td style="display:none;">' + item.LabResultId + '</td><td>' + SelectionCheckBoxColumn + '&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_LabOrder.printLabResult(\'' + item.LabOrderId + '\',\'' + item.LabResultId + '\',\'' + item.Status + '\',event, \'' + item.PatientId + '\');" title="View Lab Result"> <i class="fa fa-credit-card blue"></i></a><a class="btn btn-xs" href="#" onclick="Clinical_LabOrder.openLabTrends(\'' + item.LabResultId + '\',\'' + item.LabOrderId + '\',\'' + item.PatientFullName + ' (' + item.AccountNumber + ') ' + utility.RemoveTimeFromDate(null, item.DOB) + '\' , event);" title="Trend Record"><i class="fa fa-line-chart green"></i></a>&nbsp;' + comments + '</td><td id=\'' + item.PatientId + '\'> <a href="#" onclick="utility.PatientDemographics(\'' + item.PatientId + '\', \'mstrTabDashBoard\', event);">' + item.PatientFullName + '</a><a class="btn btn-xs" href="#" onclick="DashBoard.EditPatientLab(this,event);" title="Edit Record"><i class="fa fa-edit black"></i></a><div class=""><div id="divtxtCPT709085" class="input-group" title="" data-toggle="tooltip" data-placement="right"><div class="input-group divChangePatient hidden" ><input class="form-control ui-autocomplete-input" id="txtAccountNo" customname="Patient Name" type="text" name="patientAccount" oninput="DashBoard.BindPatientAccount(this,\'DashBoardLabOrderResults\',null,null,null,\'' + item.LabOrderId + '\',\'LabResultUnsolicited\');" onblur="" data-bv-field="patientAccount" autocomplete="off"><div class="input-group-btn"><button id="lnkPatientAccount" type="button" class="btn btn-primary btn-xs" onclick="DashBoard.OpenPatientAccount(\'Pat_' + item.LabResultId + '\',this);"><i class="fa fa-search"></i></button></div></div><input type="hidden" value="' + item.PatientId + '" id="Pat_' + item.LabResultId + '"/></td><td ' + editMode + '>'
                        + CollectionDate + " " + CollectionTime + '</td><td ' + editMode + '>' + currentDate + ' ' + currentTime + '</td><td onclick="' + onCellClick + '">' + expandCollapseIcon + divLoinc + '</td><td ' + editMode + '>' + item.LabName + '</td><td ' + editMode + '>' + item.OrderNo + '</td><td ' + editMode + '>' + item.Status + '</td><td ' + editMode + '>' + item.Provider + '</td><td ' + editMode + '>' + item.AssigneeName + '</td><td>' + Action + '</td><td class="hidden">' + Number(new Date(currentDate + ' ' + currentTime)) + '</td>');
                }
                var hfLabResultComments = $(`<input type="hidden" id='hfComments${item.LabResultId}' name="Comments" value='${decodedStripedHtml.trim()} '>`);
                $row.append(hfLabResultComments);
                if (item.isManually == true) {
                    $("#pnlDashboard  #LabResult #dgvManulayyMappedRecord").removeClass("hidden");
                    $("#pnlDashboard  #LabResult #ddgvManulayyMappedRecord").addClass("dark-yellow");
                    $row.addClass('manually');
                }

                $("#pnlDashboard  #LabResult #dgvLabOrderResult tbody").last().append($row);

                var childRows = DashBoard.buildLabOrderResultRowChild(item.LabOrderTests, item);
                DashBoard.labResultRows.push({
                    row: $row, childs: childRows
                });

                utility.bindMyJSONByName(true, item, false, childRows).done(function () {
                    childRows.find('#txtComment').html(item.Comments);
                    childRows.find('#orderNo').text("Order Number: " + item.OrderNo);
                    childRows.find('#laboratory').text("Lab: " + item.LabName);

                    var currentPatientAccount = item.AccountNumber;
                    var currentPatientId = item.PatientId;
                    var currentPatientName = item.PatientFullName;
                    var currentModuleName = "Lab Result";
                    var ResultId = item.LabResultId;
                    var LabOrderId = item.LabOrderId;
                    var CreatedBy = item.CreatedBy;
                    var ExternalPDFId = item.LabOrderResultExternalPDFId;
                    var btnScanDocument = childRows.find("#anchorDocumentScan").unbind('click').bind("click", function (e) {
                        EMRUtility.documentScan("mstrTabDashBoard", currentModuleName, currentPatientId, currentPatientName, ResultId);
                    });

                    var btnUploadDocument = childRows.find("#anchorDocumentUpload").unbind('click').bind("click", function () {
                        EMRUtility.documentImport("mstrTabDashBoard", currentModuleName, currentPatientId, currentPatientName, ResultId, currentPatientAccount);
                    });

                    var btnViewAttachment = childRows.find("#btnViewAttachment").unbind('click').bind("click", function (e) {
                        EMRUtility.loadAttachments("mstrTabDashBoard", currentModuleName, "load_attachments", currentPatientId, ResultId, "#pnlDashboard #menuViewAttachment");

                    });

                    //-----------

                    var btnViewResultDocumentPDF = childRows.find("#btnViewPDF").unbind('click').bind("click", function () {
                        //  DashBoard.viewPdfLabResult(currentPatientId, LabOrderId, ResultId);
                        if (ExternalPDFId == 0) {  // External PDF doesn't exist
                            Clinical_LabOrder.printLabResult(LabOrderId, ResultId, '', event, currentPatientId);
                        }
                        else {
                            Clinical_LabOrder.printLabResultExternalPDF(ExternalPDFId);
                        }
                    });
                    if (item.IsAknowledged) {
                        childRows.find("#btnAcknowledge").hide();
                    }
                    else {
                        var btnAcknowledged = childRows.find("#btnAcknowledge").unbind('click').bind("click", function () {
                            DashBoard.LabResultAcknowledge(ResultId).done(function (response) {
                                if (response.status) {
                                    DashBoard.DashBoardLabResultLoad(null, null, null);
                                }
                                else {
                                    utility.DisplayMessages("Could not acknowledge result", 2);
                                }

                            });
                        });
                    }

                    //-----------

                    var onClick = "DashBoard.SaveLabResult(this,'" + item.OrderNo + "','" + item.PatientId + "','" + item.ProviderId + "',event)";
                    childRows.find('#btnSaveResult').attr('onclick', onClick);
                    var btnViewHL7 = childRows.find("#btnShowHL7");
                    if (CreatedBy != null && CreatedBy.toLowerCase().indexOf("mirth") > -1) {
                        btnViewHL7.removeClass("disableAll");
                        btnViewHL7.unbind('click').bind("click", function (e) {
                            EMRUtility.viewHL7PDF("mstrTabDashBoard", currentPatientId, LabOrderId, ResultId);

                        });
                    }
                    else {
                        btnViewHL7.addClass("disableAll");
                        btnViewHL7.unbind('click');
                    }
                    if (item.Status == "Final") {

                        childRows.find('#chkSentToPortal').prop('disabled', false);

                        if (item.IsSentToPortal) {
                            childRows.find('#chkSentToPortal').prop('checked', true);
                        }
                        else {
                            childRows.find('#chkSentToPortal').prop('checked', false);
                        }
                    }
                    else {
                        childRows.find('#chkSentToPortal').prop('disabled', true);
                    }

                    if (item.MarkAsReviewed) {
                        childRows.find('#chkMarkAsReviewed').prop('checked', true);
                    }
                    else {
                        childRows.find('#chkMarkAsReviewed').prop('checked', false);
                    }

                });

                var options = $("#pnlDashboard #ddlAssigneeTemplate").find('option').clone();
                childRows.find('#ddlAssigneeId').append(options);
                childRows.find('#ddlAssigneeId').val(item.AssigneeId)

                var options = $("#pnlDashboard #ddlAssigneeTemplate").find('option').clone();
                childRows.find('#ddlReviewedById').append(options);
                if (item.ReviewedById == "") {
                    childRows.find('#ddlReviewedById').val(globalAppdata["AppUserId"]);
                } else {
                    childRows.find('#ddlReviewedById').val(item.ReviewedById);
                }

            });
        }
        else {
            if (JSON.parse(response.LabOrderLoad_JSON)[0] == undefined) {
                $("#pnlDashboard #spnListLabResultCount").text("");
            }
            var TotalCount = response.UnsolicitedPlusSolicited;
            $("#pnlDashboard div[class*='wOrders'] .badge").css("display", "none");
            $("#pnlDashboard div[class*='wOrders'] .badge").text(TotalCount);
            $('#wpanel .slick-track div').each(function (i) {
                if ($(this).find('span:first').text() == 'Orders & Results') {
                    $(this).find('span:last').text(TotalCount);
                    $(this).find('span:last').hide();
                }
            });

            //$("#pnlDashboard #spnDashboard_LabOrderCount").text(TotalCount);

            $("#pnlDashboard  #LabResult #dgvLabOrderResult").DataTable({
                "destroy": true,
                "language": {
                    "emptyTable": "No Lab Result Found"
                }, "autoWidth": false, "bLengthChange": false, "order": [[10, "desc"]]
                  , "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }

        if ($.fn.dataTable.isDataTable("#pnlDashboard #LabResult #dgvLabOrderResult"))
            ;
        else {
            DashBoard.EditableGridOrderResult = $("#pnlDashboard  #LabResult #dgvLabOrderResult").DataTable({
                "destroy": true,
                "aaSorting": [],
                "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false
                , "aoColumnDefs": [{ "bSortable": false, "aTargets": [1], "orderable": false, "aTargets": [0] }]
            });
        }


        //Hide Data Table default search
        $("#pnlDashboard  #dgvLabResultGrid").find(".dataTables_filter").hide();

        // Manually trigger custom search of data table
        $("#pnlDashboard  #LabResult").find("#dgvLabResultGrid_Search").on("keyup", function () {

            DashBoard.EditableGridOrderResult.search(this.value).draw();

        });
        EMRUtility.fixDataTableDuplication("#pnlDashboard  #dgvLabOrderResult");

        $.each(DashBoard.labResultRows, function (i, item) {

            if (DashBoard.EditableGridOrderResult != null) {

                var row = DashBoard.EditableGridOrderResult.row(item.row);
                if (item.childs.length > 0) {
                    row.child(item.childs);

                    if (globalAppdata['IsResultsExpand'] && globalAppdata['IsResultsExpand'].toLowerCase() == 'true') {
                        var expandObj = item.row.find("a#lnkResExpand");
                        if (expandObj.length > 0) {
                            DashBoard.labOrderResultRowExpandAll(expandObj, null);
                        }
                    }
                }
                else {
                    //$(item.row).find('td:first').find('a').hide();
                }
            }
        });
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body', html: true });
        DashBoard.SwicthWidgetInializatoin();

    },

    labOrderUnsolicitedResultRowExpand: function ($row, event, from) {
        if (event != null) {
            event.stopPropagation();
            event.preventDefault();
        }

        var currentRowId = null;
        if (from == "cell") {
            $row = $($row).parent();
        } else {
            $row = $($row).parent().parent();
        }
        currentRowId = $($row).attr('id');

        var $actions,
         values = [];
        var row = DashBoard.EditableGridOrderUnsolicitedResult.row($row);
        //if ($row.hasClass('adding')) {
        //    $row.removeClass('adding');
        //}
        if (row.child.isShown()) {
            // This row is already open - close it
            $row.find("td:eq(4) .fa-minus-square").attr("class", "fa fa-plus-square");
            row.child.hide();
            //tr.removeClass('shown');
        }
        else {
            $row.find("td:eq(4) .fa-plus-square").attr("class", "fa fa-minus-square");
            // Open this row
            row.child.show();
        }
        if (globalAppdata['IsResultsExpand'] && globalAppdata['IsResultsExpand'].toLowerCase() == 'false') {
            $("#pnlDashboard  #LabUnsolicited #dgvLabUnsoOrderResult tbody tr").each(function (i, item) {
                if (currentRowId != $(item).attr('id')) {
                    var allotherrows = DashBoard.EditableGridOrderUnsolicitedResult.row(item);
                    if (allotherrows.child.isShown()) {
                        $(item).find(".fa-minus-square").attr("class", "fa fa-plus-square");
                        allotherrows.child.hide();

                    }
                }
            });
        }
    },
    labOrderUnsolicitedResultRowExpandAll: function ($row, event, from) {
        if (event != null) {
            event.stopPropagation();
            event.preventDefault();
        }

        var currentRowId = null;
        if (from == "cell") {
            $row = $($row).parent();
        } else {
            $row = $($row).parent().parent();
        }
        currentRowId = $($row).attr('id');

        var $actions,
         values = [];
        var row = DashBoard.EditableGridOrderUnsolicitedResult.row($row);
        //if ($row.hasClass('adding')) {
        //    $row.removeClass('adding');
        //}
        if (row.child.isShown()) {
            // This row is already open - close it
            $row.find("td:eq(4) .fa-minus-square").attr("class", "fa fa-plus-square");
            row.child.hide();
            //tr.removeClass('shown');
        }
        else {
            $row.find("td:eq(4) .fa-plus-square").attr("class", "fa fa-minus-square");
            // Open this row
            row.child.show();
        }

        $("#pnlDashboard  #LabUnsolicited #dgvLabUnsoOrderResult tbody tr").each(function (i, item) {

            var allotherrows = DashBoard.EditableGridOrderUnsolicitedResult.row(item);
            $(item).find(".fa-plus-square").attr("class", "fa fa-minus-square");
            allotherrows.child.show();

        });
    },

    LabUnsolicitedResultGridLoad: function (response) {
        DashBoard.labUnsolicitedResultRows = [];

        if ($.fn.dataTable.isDataTable("#pnlDashboard  #LabUnsolicited #dgvLabUnsoOrderResult")) {
            $("#pnlDashboard  #LabUnsolicited #dgvLabUnsoOrderResult").dataTable().fnClearTable();
            $("#pnlDashboard  #LabUnsolicited #dgvLabUnsoOrderResult").dataTable().fnDestroy();
        }
        $("#pnlDashboard  #LabUnsolicited #dgvLabUnsoOrderResult tbody").find("tr").remove();
        if ($("#pnlDashboard  #LabUnsolicited #dgvLabUnsoOrderResult thead tr #selectResults").length == 0) {
            $("#pnlDashboard  #LabUnsolicited #dgvLabUnsoOrderResult thead tr").prepend('<th class="noWordBreak size40 size-min40"><input type="checkbox" onchange="DashBoard.checkUncheckAllUnSolicitedResults(this);" controlname="selectResults" id="selectResults" name="chkHeader" class="input-block" coltype="checkbox"/></th>');
        } else {
            $("#pnlDashboard  #LabUnsolicited #dgvLabUnsoOrderResult thead tr #selectResults").prop('checked', false);
        }
        if (response.LabResultCount > 0) {
            // Start PRD-423
            $('#pnlDashboard #comments-remarks #spnPatientPortalStatus').hide();
            $('#pnlDashboard #comments-remarks #lblPatientPortalStatus').hide();
            // End PRD-423
            if (JSON.parse(response.LabOrderLoad_JSON)[0] != undefined) {
                var TotalCount = response.UnsolicitedPlusSolicited;
                $("#pnlDashboard div[class*='wOrders'] .badge").css("display", "inline");
                $("#pnlDashboard div[class*='wOrders'] .badge").text(TotalCount);
                $('#wpanel .slick-track div').each(function (i) {
                    if ($(this).find('span:first').text() == 'Orders & Results') {
                        $(this).find('span:last').text(TotalCount);
                        $(this).find('span:last').show();
                    }
                });
                $("#pnlDashboard #spnDashboard_LabOrderCount").text(TotalCount);
                $("#pnlDashboard #spnListLabUnsolicitedCount").text(JSON.parse(response.LabOrderLoad_JSON)[0].RecordCount);
            }
            var LabLoadJSONData = JSON.parse(response.LabOrderUnsolicitedResultModel_JSON)//JSON.parse(response.LabOrderLoad_JSON); //Parsing array to JSON
            $.each(LabLoadJSONData, function (i, item) {

                var currentDate = new Date();
                var currentTime = currentDate.toLocaleTimeString().replace(/([\d]+:[\d]{2})(:[\d]{2})(.*)/, "$1$3");

                //Convert Date to prper date time format
                if (item.ModifiedOn != null) {
                    currentDate = new Date(item.ModifiedOn).toLocaleDateString();
                    currentTime = new Date(item.ModifiedOn).toLocaleTimeString().replace(/([\d]+:[\d]{2})(:[\d]{2})(.*)/, "$1$3");
                }
                var $row = $('<tr/>');
                $row.attr("id", "gvLab_row" + item.LabResultId);
                $row.attr("LabOrderResultId", item.LabResultId);
                $row.attr("labid", item.LabOrderId);

                var SelectionCheckBoxColumn = '<a class="btn btn-xs" role="button" onclick="DashBoard.OnClickPreventDefault(event);" title="Select Lab Result"><input type="checkbox" id=' + item.LabResultId + ' name="SelectCheckBoxResult" class="input-block"/></a>';
                var Checked = "";

                parentControl = 'DashBoard';
                parentControlPanelID = 'pnlDashboard';

                var divLoinc = '';
                try {
                    divLoinc = item.LabOrderTests.length > 0 && (item.LabOrderTests[0].LabOrderResultDetails.length > 0 && item.LabOrderTests[0].LabOrderResultDetails[0].LOINC != "") ? item.LabOrderTests[0].LabOrderResultDetails[0].LOINCDescription : item.LabOrderTests[0].CPTDescription;
                } catch (ex) {
                    console.log(ex);
                }
                var onclick = "DashBoard.labOrderUnsolicitedResultRowExpand(this,event);";
                var onCellClick = "DashBoard.labOrderUnsolicitedResultRowExpand(this,event,'cell');";
                var expandCollapseIcon = '<a id="lnkSolExpand" href="#" onclick="' + onclick + '" class="tab_space" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a>';
                
                if (item.Status == "Signed") {
                    $row.append('<td style="display:none;">' + item.LabResultId + '</td><td>' + SelectionCheckBoxColumn + '&nbsp;<a class="btn  btn-xs hidden" href="#" onclick="Clinical_LabOrder.printLabResult(\'' + item.LabOrderId + '\',\'' + item.LabResultId + '\',\'' + item.Status + '\',event, \'' + item.PatientId + '\');" title="View Unsolicited Result"> <i class="fa fa-credit-card blue"></i></a></td><td>'
                   + item.PatientName + '</td><td>' + currentDate + ' ' + currentTime + '</td><td onclick="' + onCellClick + '">' + expandCollapseIcon + divLoinc + '</td><td>' + item.LabName + '</td><td>' + item.OrderNo + '</td><td>' + item.Status + '</td><td>' + item.Provider + '</td><td>' + item.AssigneeName + '</td><td><div class=""><div id="divtxtCPT709085" class="input-group" title="" data-toggle="tooltip" data-placement="right" ><div class="input-group divChangePatient" ><input class="form-control ui-autocomplete-input" id="txtAccountNo" customname="Patient Name" value="' + item.PatientName + '" type="text" name="patientAccount" oninput="DashBoard.BindPatientAccountAndName(this,\'DashBoardLabOrderResults\',null,null,null,\'' + item.LabOrderId + '\',\'LabResultUnsolicited\');" onblur="" data-bv-field="patientAccount" autocomplete="off"><div class="input-group-btn"><button id="lnkPatientAccount" type="button" class="btn btn-primary btn-xs" onclick="DashBoard.OpenPatientAccount(\'Pat_' + item.LabResultId + '\',this);"><i class="fa fa-search"></i></button></div></div><input type="hidden" value="' + item.PatientId + '" id="Pat_' + item.LabResultId + '"/></td>');
                }
                else {

                    $row.append('<td style="display:none;">' + item.LabResultId + '</td><td>' + SelectionCheckBoxColumn + '&nbsp;<a class="btn  btn-xs hidden" href="#" onclick="Clinical_LabOrder.printLabResult(\'' + item.LabOrderId + '\',\'' + item.LabResultId + '\',\'' + item.Status + '\',event, \'' + item.PatientId + '\');" title="View Unsolicited Result"> <i class="fa fa-credit-card blue"></i></a></td><td>'
                       + item.PatientName + '</td><td>' + currentDate + ' ' + currentTime + '</td><td  onclick="' + onCellClick + '">' + expandCollapseIcon + divLoinc + '</td><td>' + item.LabName + '</td><td>' + item.OrderNo + '</td><td>' + item.Status + '</td><td>' + item.Provider + '</td><td>' + item.AssigneeName + '</td><td><div class=""><div id="divtxtCPT709085" class="input-group" title="" data-toggle="tooltip" data-placement="right" ><div class="input-group divChangePatient" ><input class="form-control ui-autocomplete-input" id="txtAccountNo" customname="Patient Name" type="text" value="' + item.PatientName + '" name="patientAccount" oninput="DashBoard.BindPatientAccountAndName(this,\'DashBoardLabOrderResults\',null,null,null,\'' + item.LabOrderId + '\',\'LabResultUnsolicited\');" onblur="" data-bv-field="patientAccount" autocomplete="off"><div class="input-group-btn"><button id="lnkPatientAccount" type="button" class="btn btn-primary btn-xs" onclick="DashBoard.OpenPatientAccount(\'Pat_' + item.LabResultId + '\',this);"><i class="fa fa-search"></i></button></div></div><input type="hidden" value="' + item.PatientId + '" id="Pat_' + item.LabResultId + '"/></td>');
                }
                $("#pnlDashboard  #LabUnsolicited #dgvLabUnsoOrderResult tbody").last().append($row);

                var childRows = DashBoard.buildLabOrderResultRowChild(item.LabOrderTests, item);
                DashBoard.labUnsolicitedResultRows.push({
                    row: $row, childs: childRows
                });
                //Bind values
                // childRows.loadDropDowns(true).done(function () {
                //Bind Values to the child row
                utility.bindMyJSONByName(true, item, false, childRows).done(function () {
                    childRows.find('#txtComment').html(item.Comments);
                    childRows.find('#orderNo').text("Order Number: " + item.OrderNo);
                    childRows.find('#laboratory').text("Lab: " + item.LabName);

                    var currentPatientAccount = item.AccountNumber;
                    var currentPatientId = item.PatientId;
                    var currentPatientName = item.PatientFullName;
                    var currentModuleName = "Lab Result";
                    var ResultId = item.LabResultId;
                    var LabOrderId = item.LabOrderId;
                    var CreatedBy = item.CreatedBy;

                    var onClick = "DashBoard.SaveLabResult(this,'" + item.OrderNo + "','" + item.PatientId + "','" + item.ProviderId + "',event)";
                    childRows.find('#btnSaveResult').attr('onclick', onClick);
                    var btnViewHL7 = childRows.find("#btnShowHL7");
                    if (CreatedBy != null && CreatedBy.toLowerCase().indexOf("mirth") > -1) {
                        btnViewHL7.removeClass("disableAll");
                        btnViewHL7.unbind('click').bind("click", function (e) {
                            EMRUtility.viewHL7PDF("mstrTabDashBoard", currentPatientId, LabOrderId, ResultId);

                        });
                    }
                    else {
                        btnViewHL7.addClass("disableAll");
                        btnViewHL7.unbind('click');
                    }

                    //Hide drpdown attach icon
                    childRows.find('#attchdrpDown').hide();
                    if (item.Status == "Final") {

                        childRows.find('#chkSentToPortal').prop('disabled', false);

                        if (item.IsSentToPortal) {
                            childRows.find('#chkSentToPortal').prop('checked', true);
                        }
                        else {
                            childRows.find('#chkSentToPortal').prop('checked', false);
                        }
                    }
                    else {
                        childRows.find('#chkSentToPortal').prop('disabled', true);
                    }

                });
                // });
                var options = $("#pnlDashboard #ddlAssigneeTemplate").find('option').clone();
                childRows.find('#ddlAssigneeId').append(options);
                childRows.find('#ddlAssigneeId').val(item.AssigneeId)

                var options = $("#pnlDashboard #ddlAssigneeTemplate").find('option').clone();
                childRows.find('#ddlReviewedById').append(options);

                childRows.find('#ddlReviewedById').val(item.ReviewedById)
                //AST - 415
                if (item.ReviewedById || item.MarkAsReviewed) {
                    childRows.find('#chkMarkAsReviewed').prop('checked', true)
                }

                else {
                    childRows.find('#chkMarkAsReviewed').prop('checked', false);
                }



            });
        }
        else {
            if (JSON.parse(response.LabOrderLoad_JSON)[0] == undefined) {
                $("#pnlDashboard #spnListLabUnsolicitedCount").text("");
            }
            var TotalCount = response.UnsolicitedPlusSolicited;
            $("#pnlDashboard div[class*='wOrders'] .badge").css("display", "none");
            //$("#pnlDashboard div[class*='wOrders'] .badge").text(TotalCount);
            $('#wpanel .slick-track div').each(function (i) {
                if ($(this).find('span:first').text() == 'Orders & Results') {
                    $(this).find('span:last').text(TotalCount);
                    $(this).find('span:last').hide();
                }
            });
            //$("#pnlDashboard #spnDashboard_LabOrderCount").text(TotalCount);
            $('#pnlDashboard #LabUnsolicited .badge').css("display", "none");

            $("#pnlDashboard  #LabUnsolicited #dgvLabUnsoOrderResult").DataTable({
                "destroy": true,
                "language": {
                    "emptyTable": "No unsolicited Results Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{
                    "bSortable": false, "aTargets": [1], "orderable": false, "aTargets": [0]
                }]
            });
        }

        if ($.fn.dataTable.isDataTable("#pnlDashboard #LabUnsolicited #dgvLabUnsoOrderResult"))
            ;
        else {
            DashBoard.EditableGridOrderUnsolicitedResult = $("#pnlDashboard  #LabUnsolicited #dgvLabUnsoOrderResult").DataTable({
                "destroy": true,
                "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{
                    "bSortable": false, "aTargets": [1], "orderable": false, "aTargets": [0]
                }]
            });
        }



        //Hide Data Table default search
        $("#pnlDashboard  #dgvLabUnsoResultGrid").find(".dataTables_filter").hide();

        // Manually trigger custom search of data table
        $("#pnlDashboard  #LabUnsolicited").find("#dgvLabUnsoResultGrid_Search").on("keyup", function () {

            DashBoard.EditableGridOrderUnsolicitedResult.search(this.value).draw();

        });
        EMRUtility.fixDataTableDuplication("#pnlDashboard  #dgvLabUnsoOrderResult");

        $.each(DashBoard.labUnsolicitedResultRows, function (i, item) {

            if (DashBoard.EditableGridOrderUnsolicitedResult != null) {

                var row = DashBoard.EditableGridOrderUnsolicitedResult.row(item.row);
                if (item.childs.length > 0) {
                    row.child(item.childs);

                    if (globalAppdata['IsResultsExpand'] && globalAppdata['IsResultsExpand'].toLowerCase() == 'true') {
                        var expandObj = item.row.find("a#lnkSolExpand");
                        if (expandObj.length > 0) {
                            DashBoard.labOrderUnsolicitedResultRowExpandAll(expandObj, null);
                        }
                    }
                }
                else {
                    //$(item.row).find('td:first').find('a').hide();
                }
            }
        });
        DashBoard.SwicthWidgetInializatoin();

    },

    //setControlValue: function (obj) {
    //    if (obj)
    //        $("#hfMapToPatient").val($(obj).val());
    //},

    LoadDashBoardLabResultAbnormalCount: function () {
        DashBoard.LoadDashBoardLabResultCount().done(function (response) {
            response = JSON.parse(response);
            var labCountObj = JSON.parse(response.LABRESULT_COUUNT);
            var abnormalCount = labCountObj[0].AbnormalResultCount;
            $("#pnlDashboard #spnAbnormalResultCount").html(abnormalCount == 0 ? '' : '<button class="btn btn-danger btn-xs tab_space" onclick="DashBoard.LoadAbmormalLabResults();">' + abnormalCount + ' Abnormal Lab Result(s)</button>');
            //if (abnormalCount == 0)
            //$("#pnlDashboard #lnkLabUnsolicited").trigger("click");
        });
    },
    LoadDashBoardTCMNotCreatedCount: function () {
        DashBoard.LoadDashBoardTCMCount().done(function (response) {
            response = JSON.parse(response);
            var CountObj = JSON.parse(response.TCMS_COUUNT);
            var DraftCount = CountObj[0].Draft;
            var NotCreatedCount = CountObj[0].NotCreated;
            $("#pnlDashboard #spnNotCreatedTCMCount").html(NotCreatedCount == 0 ? '' : '<button class="btn btn-danger btn-xs tab_space" onclick="DashBoard.DashBoardTCMSearch(null,null,null,\'NotCreated\',\'Span\');">' + NotCreatedCount + ' Not Created</button>');
            $("#pnlDashboard #spnDraftTCMCount").html(DraftCount == 0 ? '' : '<button class="btn btn-danger btn-xs tab_space" onclick="DashBoard.DashBoardTCMSearch(null,null,null,\'Draft\',\'Span\');">' + DraftCount + ' Draft</button>');
            //if (abnormalCount == 0)
            //$("#pnlDashboard #lnkLabUnsolicited").trigger("click");
        });
    },

    LoadDashBoardLabResultCount: function () {
        var objData = {
        };
        objData["commandType"] = "labresults_count_dashboard"
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },
    LoadDashBoardTCMCount: function () {
        var objData = {
        };
        objData["commandType"] = "tcm_count_dashboard"
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },

    createDataTable: function (panelGridId, gridId, ctrName, data) {

    },

    //Author: Abid Ali
    //Date :  15-04-2016
    //This function will handle Add/Edit of LabResult
    LabResultAddEdit: function (LabResultId, LabOrderId, event, PatientId) {

        event.preventDefault();
        var strMessage = "";
        var permissionState = LabOrderId != null && parseInt(LabOrderId) > 0 ? "EDIT" : "ADD";
        AppPrivileges.GetFormPrivileges("Orders and Results_Lab Result", permissionState, "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                if (LabOrderId != null && parseInt(LabOrderId) > 0) {
                    params["LabResultId"] = LabResultId;
                    params["mode"] = "Edit";
                }
                else {
                    params["LabResultId"] = -1;
                    params["mode"] = "Add";
                }
                params["LabOrderId"] = LabOrderId;
                if (params["ParentCtrl"] == null) {
                    //  params['LabResultId'] = LabOrderId;
                    params['RefModuleName'] = "Lab Results";
                    params['TransitionId'] = LabOrderId;
                }

                params["FromAdmin"] = "0";
                params["LabResultPatientId"] = PatientId;
                params["ParentCtrl"] = 'mstrTabDashBoard';
                LoadActionPan('ClinicalLabResultDetail', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    handleDatatableOnBtnClick: function (obj) {

        var btnId = $(obj).attr("id");
        var hrefId = $(obj).find('a').attr("href");
        var parent = $(obj).parent().find('li').removeClass('active');
        // $(obj).addClass('active');
        $('.tabs-custom a[href="' + hrefId + '"]').tab('show');
        var PanelGrid = "#dgvLabOrderGrid";
        var GridId = "#dgvLabOrderGrid #dgvLabOrderDashboard";

        switch (btnId) {
            case 'listLabOrderPending':
                DashBoard.createDataTable(PanelGrid, GridId, "DashBoard", DashBoard.labOrderRows);
                break;
            case 'listLabResult':
                PanelGrid = "#LabResult";
                GridId = "#LabResult #dgvLabOrderResult";
                DashBoard.createDataTable(PanelGrid, GridId, "DashBoard", DashBoard.labResultRows);
                break;
        }
    },
    //For Lab Order
    buildLabOrderRowChild: function (tests, labOrderId) {

        //  var tests = tests//childItems.Test.split('|');
        var CurrentRowchilds = $();
        var templateHtml = $("#pnlDashboard  #LabOrderTemplate").clone();

        if (tests != null && tests.length > 0) {

            var $tbody = templateHtml.find('#dgvLabOrderTemplate').find('tbody');

            var onClick = "DashBoard.LabResultAddEdit(-1,'" + labOrderId + "',event)";
            $.each(tests, function (i, item) {


                var link = item.LabOrderResultDetails.length > 0 ? "View Result" : "Add Result";
                var i = 1;
                do {
                    var $ChilRowDetail = $('<tr/>').addClass("childRowDetail-bg");
                    if (i == 1) {
                        $ChilRowDetail.append('<td class="bold" colspan="2" >' + item.CPTDescription + '</td> <td></td>');
                    }
                    else {
                        $ChilRowDetail.append('<td colspan="2" >' + item.CPTDescription + '</td> <td ><a onclick="' + onClick + ' " href="#" >' + link + '</a></td>');
                        if (item.AOEs != null || item.AOEs != "") {
                            item.AOEs = item.AOEs.replace(/;/g, '<br>');
                            item.AOEs = item.AOEs.replace(/Q-/g, '<b>');
                            item.AOEs = item.AOEs.replace(/A-/g, '</b>');
                            var $ChilRowDetailAOE = $('<tr/>').addClass("childRowDetail-bg");
                            $ChilRowDetailAOE.append('<td colspan="2" >' + item.AOEs + '</td> <td></td>');
                        }
                    }
                    $tbody.append($ChilRowDetail);
                    if ($ChilRowDetailAOE != undefined) {
                        $tbody.append($ChilRowDetailAOE);
                    }
                    i++;
                } while (i <= 2)
            });
        }

        var $row = $('<tr/>').addClass("childRow-bg");
        var cellpadding = '';

        $row.append('<td class="hidden"></td> <td class="hidden"> <td class="hidden"></td>  <td class="hidden"></td> <td colspan="10">' + templateHtml.html() + '</td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td>');
        return CurrentRowchilds = CurrentRowchilds.add($row);

    },

    labOrderRowExpand: function ($row, event, from) {
        if (event != null) {
            event.stopPropagation();
            event.preventDefault();
        }

        var currentRowId = null;
        if (from == "cell") {
            $row = $($row).parent();
        } else {
            $row = $($row).parent().parent();
        }
        currentRowId = $($row).attr('id');

        var $actions,
         values = [];
        var row = DashBoard.EditableGridOrder.row($row);
        //if ($row.hasClass('adding')) {
        //    $row.removeClass('adding');
        //}
        if (row.child.isShown()) {
            // This row is already open - close it
            $row.find("td:eq(4) .fa-minus-square").attr("class", "fa fa-plus-square");
            row.child.hide();
            //tr.removeClass('shown');
        }
        else {
            $row.find("td:eq(4) .fa-plus-square").attr("class", "fa fa-minus-square");
            // Open this row
            row.child.show();
        }
        if (globalAppdata['IsOrdersExpand'] && globalAppdata['IsOrdersExpand'].toLowerCase() == 'false') {
            $("#pnlDashboard  #dgvLabOrderGrid #dgvLabOrderDashboard tbody tr").each(function (i, item) {
                if (currentRowId != $(item).attr('id')) {
                    var allotherrows = DashBoard.EditableGridOrder.row(item);
                    if (allotherrows.child.isShown()) {
                        $(item).find(".fa-minus-square").attr("class", "fa fa-plus-square");
                        allotherrows.child.hide();

                    }
                }
            });
        }
    },
    labOrderRowExpandAll: function ($row, event, from) {
        if (event != null) {
            event.stopPropagation();
            event.preventDefault();
        }

        var currentRowId = null;
        if (from == "cell") {
            $row = $($row).parent();
        } else {
            $row = $($row).parent().parent();
        }
        currentRowId = $($row).attr('id');

        var $actions,
         values = [];
        var row = DashBoard.EditableGridOrder.row($row);
        //if ($row.hasClass('adding')) {
        //    $row.removeClass('adding');
        //}
        if (row.child.isShown()) {
            // This row is already open - close it
            $row.find("td:eq(4) .fa-minus-square").attr("class", "fa fa-plus-square");
            row.child.hide();
            //tr.removeClass('shown');
        }
        else {
            $row.find("td:eq(4) .fa-plus-square").attr("class", "fa fa-minus-square");
            // Open this row
            row.child.show();
        }
        $("#pnlDashboard  #dgvLabOrderGrid #dgvLabOrderDashboard tbody tr").each(function (i, item) {

            var allotherrows = DashBoard.EditableGridOrder.row(item);

            $(item).find(".fa-plus-square").attr("class", "fa fa-minus-square");
            allotherrows.child.show();

        });
    },

    //For Lab Result
    buildLabOrderResultRowChild: function (tests, object) {

        //  var tests = tests//childItems.Test.split('|');
        var CurrentRowchilds = $();
        var templateHtml = $("#pnlDashboard  #LabResultTemplate").clone();

        if (tests != null && tests.length > 0) {

            var $tbody = templateHtml.find('#dgvLabResultTemplate').find('tbody');

            $.each(tests, function (i, item) {

                var $ChilRowTestDetail = $('<tr/>').addClass("childRowTest-bg");

                $ChilRowTestDetail.append('<td class="bold" colspan="6" >' + item.CPTDescription + '</td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td>  <td class="hidden"></td> <td class="hidden"></td>');
                $tbody.append($ChilRowTestDetail);
                var NTEText = "";
                $.each(item.LabOrderResultDetails, function (i, detailItem) {

                    //-------------------------

                    var color = "";

                    if (detailItem.Flag == "Abnormally High" || detailItem.Flag == "High" || detailItem.Flag == "Abnormal") {
                        color = 'style = "color:red;font-weight:bold"'
                    }
                    if (detailItem.Flag == "Abnormally Low" || detailItem.Flag == "Low") {
                        color = 'style = "color:orange;font-weight:bold"'
                    }
                    if (detailItem.Flag == "Normal") {
                        color = 'style = "color:green;font-weight:bold"'
                    }

                    //-------------------------

                    $ChilRowDetail = $('<tr/>').addClass("childRowDetail-bg");
                    $ChilRowDetail.append('<td  >' + detailItem.LOINCDescription + '</td> <td ' + color + '>' + detailItem.Result + '</td> <td>' + detailItem.UoM + '</td> <td>' + detailItem.Range + '</td>  <td ' + color + '>' + detailItem.Flag + '</td> <td>' + object.Status + '</td>');
                    if (detailItem.NTEText != "") {
                        NTEText = NTEText + detailItem.NTEText;
                    }

                    $tbody.append($ChilRowDetail);
                });

                if (NTEText != "") {
                    NTEText = NTEText.replace(/\~/g, '<br>');
                    $ChilRowDetail = $('<tr/>').addClass("childRowDetail-bg");
                    $ChilRowDetail.append('<td colspan="6">' + NTEText + '</td> <td style="display:none;"></td> <td style="display:none;"></td> <td style="display:none;"></td>  <td style="display:none;"></td> <td style="display:none;"></td>');
                    $tbody.append($ChilRowDetail);
                }

            });
        }
        var $row = $('<tr/>').addClass("childRow-bg");
        var cellpadding = '';

        $row.append('<td class="hidden"></td> <td class="hidden"> <td class="hidden"></td>  <td class="hidden"></td> <td colspan="10">' + templateHtml.html() + '</td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td>');
        return CurrentRowchilds = CurrentRowchilds.add($row);

    },



    labOrderResultRowExpand: function ($row, event, from) {
        if (event != null) {
            event.stopPropagation();
            event.preventDefault();
        }

        var currentRowId = null;
        if (from == "cell") {
            $row = $($row).parent();
        } else {
            $row = $($row).parent().parent();
        }
        currentRowId = $($row).attr('id');

        var $actions,
         values = [];
        var row = DashBoard.EditableGridOrderResult.row($row);
        //if ($row.hasClass('adding')) {
        //    $row.removeClass('adding');
        //}
        if (row.child.isShown()) {
            // This row is already open - close it
            $row.find("td:eq(5) .fa-minus-square").attr("class", "fa fa-plus-square");
            row.child.hide();
            //tr.removeClass('shown');
        }
        else {
            $row.find("td:eq(5) .fa-plus-square").attr("class", "fa fa-minus-square");
            // Open this row
            row.child.show();
            $('#pnlDashboard #dgvLabOrderResult .childRow-bg').find('td[colspan=10]').attr('colspan', 11)
        }
        if (globalAppdata['IsResultsExpand'] && globalAppdata['IsResultsExpand'].toLowerCase() == 'false') {
            $("#pnlDashboard  #LabResult #dgvLabOrderResult tbody tr").each(function (i, item) {
                if (currentRowId != $(item).attr('id')) {
                    var allotherrows = DashBoard.EditableGridOrderResult.row(item);
                    if (allotherrows.child.isShown()) {
                        $(item).find(".fa-minus-square").attr("class", "fa fa-plus-square");
                        allotherrows.child.hide();

                    }
                }
            });
        }
    },
    labOrderResultRowExpandAll: function ($row, event, from) {
        if (event != null) {
            event.stopPropagation();
            event.preventDefault();
        }

        var currentRowId = null;
        if (from == "cell") {
            $row = $($row).parent();
        } else {
            $row = $($row).parent().parent();
        }
        currentRowId = $($row).attr('id');

        var $actions,
             values = [];
        var row = DashBoard.EditableGridOrderResult.row($row);
        //if ($row.hasClass('adding')) {
        //    $row.removeClass('adding');
        //}
        if (row.child.isShown()) {
            // This row is already open - close it
            $row.find("td:eq(5) .fa-minus-square").attr("class", "fa fa-plus-square");
            row.child.hide();
            //tr.removeClass('shown');
        }
        else {
            $row.find("td:eq(5) .fa-plus-square").attr("class", "fa fa-minus-square");
            // Open this row
            row.child.show();
            $('#pnlDashboard #dgvLabOrderResult .childRow-bg').find('td[colspan=10]').attr('colspan', 11)
        }

        $("#pnlDashboard  #LabResult #dgvLabOrderResult tbody tr").each(function (i, item) {

            var allotherrows = DashBoard.EditableGridOrderResult.row(item);

            $(item).find(".fa-plus-square").attr("class", "fa fa-minus-square");
            allotherrows.child.show();


        });
    },
    OpenPatientAccountTCM: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'mstrTabDashBoard';
        params["IsUnsolitedPatientSearch"] = false;
        params["IsFirstLoadFromDash"] = false;
        params["ActiveWidget"] = "TCM";
        LoadActionPan('Patient_Search', params);

    },
    OpenPatientAccountDataChangeRequest: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'mstrTabDashBoard';
        params["IsUnsolitedPatientSearch"] = false;
        params["IsFirstLoadFromDash"] = false;
        params["ActiveWidget"] = "DataChangeRequest";
        LoadActionPan('Patient_Search', params);

    },
    OpenDocumentPatientAccount: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'mstrTabDashBoard';
        params["IsUnsolitedPatientSearch"] = false;
        params["IsFirstLoadFromDash"] = false;
        params["ActiveWidget"] = "Documents";
        LoadActionPan('Patient_Search', params);

    },
    OpenPatientFromLabOrder: function (ActiveWidget) {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'mstrTabDashBoard';
        params["IsUnsolitedPatientSearch"] = false;
        params["IsFirstLoadFromDash"] = false;
        params["ActiveWidget"] = ActiveWidget
        LoadActionPan('Patient_Search', params);
    },
    OpenPatientAccountNote: function (innerCtrl) {
        var params = [];
        DashBoard.InnerCtrl = innerCtrl;
        if (globalAppdata.IsProviderBulkSign == "True") {
            if (innerCtrl && innerCtrl != "") {
                params["RefCtrl"] = innerCtrl + " #txtFullName";
                params["RefCtrlHidden"] = innerCtrl + " #hfPatientId";
            } else {
                params["RefCtrl"] = "txtFullName";
                params["RefCtrlHidden"] = "hfPatientId";
            }
        }
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'mstrTabDashBoard';
        params["IsUnsolitedPatientSearch"] = false;
        params["IsFirstLoadFromDash"] = false;
        params["ActiveWidget"] = "Encounter";
        LoadActionPan('Patient_Search', params);

    },

    OpenPatientAccountDocument: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'mstrTabDashBoard';
        params["IsUnsolitedPatientSearch"] = false;
        params["IsFirstLoadFromDash"] = false;
        params["ActiveWidget"] = "Documents";
        LoadActionPan('Patient_Search', params);

    },

    OpenPatientAccount: function (hiddenId, button) {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'mstrTabDashBoard';
        params["DashboardLabOrder"] = '1';

        var tableID = $(button).closest('table').attr('id');

        hiddenId = tableID + ' #' + hiddenId;
        //params["Type"] = "order"
        params["HiddenControl"] = hiddenId;

        //if ($("#hfMapToPatient").val())
        //    params["MapToPatient"] = $("#hfMapToPatient").val();

        var elem = $(button).parent().parent();
        params["PatientName"] = $(elem).children('input').val();
        params["IsUnsolitedPatientSearch"] = true;
        params["IsFirstLoadFromDash"] = true;
        LoadActionPan('Patient_Search', params);
    },


    UpdatePatientOfLabOrder: function (patientID, AccountNumber, FullName, event, LabOrderId, ResultType) {
        try {
            if (event != null) {
                event.stopPropagation();
            }
            if (Patient_Search != null && Patient_Search.params != null && Patient_Search.params.HiddenControl != null) {

                $('#pnlDashboard #' + Patient_Search.params.HiddenControl).val(patientID);
                var Currenttd = $('#pnlDashboard #' + Patient_Search.params.HiddenControl).closest('td');

                var objData = new Object();
                objData["PatientId"] = patientID;
                objData["LabOrderId"] = Currenttd.closest('tr').attr('labid');
                objData["commandType"] = "LABORDER_CHANGE_PATIENT";
                var data = JSON.stringify(objData);

                MDVisionService.APIService(data, "LabOrder", "LabOrder").done(function (response) {

                    if (typeof response == 'string') {
                        response = JSON.parse(response);
                    }
                    if (response.status == true) {
                        Currenttd.find("#txtAccountNo").val(FullName);
                        Currenttd.attr('id', patientID);

                        Currenttd.find('a:first').attr('onclick', "utility.PatientDemographics('" + patientID + "', 'mstrTabDashBoard', event)");
                        Currenttd.find('a:first').text(FullName);
                        Currenttd.find('a').removeClass('hidden');
                        Currenttd.find('.divChangePatient').addClass('hidden');

                        $('#pnlDashboard #' + Patient_Search.params.HiddenControl).val(patientID);
                        //$("#pnlDashboard #btnLabSearch").trigger("click");
                        $("#pnlDashboard #btnLabUnsoResultSearch").trigger("click");
                        $("#pnlDashboard #btnLabResultSearch").trigger("click");

                        if (Currenttd.closest('table').attr('id') == 'dgvLabUnsoOrderResult') {
                            Currenttd.closest('tr').remove();
                            // DashBoard.DashBoardLabOrderSearch(null, null, null);
                            DashBoard.selectDashBoradOrderResultSubTabs("LabResultUnsolicited");

                        }
                        else if (Currenttd.closest('table').attr('id') == 'dgvLabOrderResult') {
                            DashBoard.selectDashBoradOrderResultSubTabs("LabResult");
                        }
                        else {
                            DashBoard.selectDashBoradOrderResultSubTabs("LabOrder");
                        }
                    }


                });

                UnloadActionPan(Patient_Search.params.ParentCtrl);
            }
            else if (LabOrderId != null) {
                var objData = new Object();
                objData["PatientId"] = patientID;
                objData["LabOrderId"] = LabOrderId;
                objData["commandType"] = "LABORDER_CHANGE_PATIENT";
                var data = JSON.stringify(objData);

                MDVisionService.APIService(data, "LabOrder", "LabOrder").done(function (response) {

                    if (typeof response == 'string') {
                        response = JSON.parse(response);
                    }
                    if (response.status == true) {
                        //$("#pnlDashboard #btnLabSearch").trigger("click");
                        $("#pnlDashboard #btnLabUnsoResultSearch").trigger("click");
                        $("#pnlDashboard #btnLabResultSearch").trigger("click");

                        DashBoard.selectDashBoradOrderResultSubTabs(ResultType);

                        $('#pnlDashboard #' + Patient_Search.params.HiddenControl).val(patientID);
                    }


                });
            }
        } catch (ex) {
            console.log(ex);
        }
    },

    selectDashBoradOrderResultSubTabs: function (ResultType) {
        var DashBoardOrderResultType = "#listLabOrder";
        if (ResultType == "LabResult") {
            DashBoardOrderResultType = "#listLabResult";
        }
        else if (ResultType == "LabResultUnsolicited") {
            DashBoardOrderResultType = "#listLabUnsolicited";
        }

        $("#pnlDashboard #ulLabOrderResultTabsItems " + DashBoardOrderResultType).trigger("click");
    },

    FillReferralNumber: function () {
        // Start 29/01/2016 Muhammad Irfan for Bug # 3738
        var AccountNo = $('#pnlDashboard #frmappointmentDetail #txtAccountNo').val();
        if (AccountNo == "") {
            $('#pnlDashboard #frmappointmentDetail #txtFullName,#dtpDOB,#txtPatientBalance,#txtInsuranceBalance,#txtAdvanceBalance,#txtAdvanceBalance,#txtRefProvider,#txtPriority,#txtReferralNo,#CoPayment').val('');
            if ($("#pnlDashboard #lnkRefProviderEdit").css("display") == "inline") {
                $("#pnlDashboard #lnkRefProviderEdit").css("display", "none");
                $("#pnlDashboard #lblRefProvider").css("display", "inline");
            }
        }
        // End 29/01/2016 Muhammad Irfan for Bug # 3738
        var ParentCtrl = "pnlDashboard";
        var self = $("#pnlDashboard #pnlReferralsGrid");

        var patientID = self.find("#hfpatientid").val();
        var selectedInsurancePlan = self.find("#hfInsurancePlanId").val() == "" ? '0' : self.find("#hfInsurancePlanId").val();
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

    BindPatientAccount: function (obj, fromCtrlType, currentPatientId, currentPatientAccountNo, currentPatientFullName, LabOrderId, ResultType) {
        var AccountNo = obj != null ? $(obj).val() : $('#pnlDashboard #txtAccountNo').val();
        var ctrlAccountNo = obj != null ? $(obj) : $('#pnlDashboard #txtAccountNo');
        currentPatientId != null ? currentPatientId : "";
        currentPatientAccountNo != null ? currentPatientAccountNo : "";
        currentPatientFullName != null ? currentPatientFullName : "";
        // Start 08/03/2016 Muhammad Irfan for bug # PMS-4361
        utility.Keyupdelay(function () {
            var AllPatients = utility.GetPatientArray(AccountNo, 1).done(function (response) {

                // Start 29/01/2016 Muhammad Irfan for Bug # 3738
                if (AccountNo == "") {
                    $('#pnlDashboard #frmappointmentDetail #txtReferralNo').val('');
                    if ($("#pnlDashboard #lnkRefProviderEdit").css("display") == "inline") {
                        $("#pnlDashboard #lnkRefProviderEdit").css("display", "none");
                        $("#pnlDashboard #lblRefProvider").css("display", "inline");
                    }
                    if ($("#pnlDashboard #lnkReferralNumberEdit").css("display") == "inline") {
                        $("#pnlDashboard #lnkReferralNumberEdit").css("display", "none");
                        $("#pnlDashboard #lblReferralNumber").css("display", "inline");
                    }
                }
                // End 29/01/2016 Muhammad Irfan for Bug # 3738

                ctrlAccountNo.autocomplete({
                    //source: AllPatients, // pass an array (without a comma)
                    autoFocus: true,
                    source: response,
                    open: function (event, ui) {
                        disable = true
                    },
                    close: function (event, ui) {
                        disable = false; $(this).focus();
                    },
                    select: function (event, ui) {
                        setTimeout(function () {
                            ctrlAccountNo.val(ui.item.AccountNumber);
                            currentPatientId = ui.item.id;
                            currentPatientAccountNo = ui.item.AccountNumber;
                            currentPatientFullName = ui.item.FullName;
                            utility.InsertRecentPatient(ui.item.id);
                        }, 100);
                    }
                }).blur(function () {

                    //Start 10/02/2016 Muhammad Irfan for bug PMS-3873
                    setTimeout(function () {
                        utility.ValidateAutoComplete(ctrlAccountNo, "pnlDashboard #hfpatientid", false, 1, null, null);
                        if (fromCtrlType == "DashBoardLabOrderResults") {
                            DashBoard.UpdatePatientOfLabOrder(currentPatientId, currentPatientAccountNo, currentPatientFullName, event, LabOrderId, ResultType);
                        }
                    }, 200);
                    //End 10/02/2016 Muhammad Irfan for bug PMS-3873
                });


                ctrlAccountNo.autocomplete("search");
                $('#pnlDashboard #txtAccountNo').focus();
            });
        });
        // End 08/03/2016 Muhammad Irfan for bug # PMS-4361

    },


    EditPatientLab: function (obj, event) {
        $(obj).parent().find('.divChangePatient').removeClass("hidden");
        $(obj).parent().find('a').addClass("hidden");
        $(obj).parent().find("#txtAccountNo").val($(obj).parent().find('a:first').text());
        event.preventDefault();
    },

    //#endregion Server Side Calls
    SelectedPageClick: function (PageNo, objPage) {
        var command = $('#ActiveWidget').val();

        switch (command) {
            case 'Payments':
                $("#pnlPaymentsGrid li").each(function () {
                    if ($(this).text() == PageNo) {
                        $(this).attr("class", "active");
                    }
                    else
                        $(this).removeAttr("class");
                });
                DashBoard.DashBoardPaymentsSearch(PageNo, 15);
                break;
            case 'Appointments':
                $("#pnlSchAppointmentGrid li").each(function () {
                    if ($(this).text() == PageNo) {
                        $(this).attr("class", "active");
                    }
                    else
                        $(this).removeAttr("class");
                });
                DashBoard.DashBoardAppointmentSearch(PageNo, 15, null, null);

                break;
            case 'Messages':

                var activeTab;
                $('#pnlUserMessagesGrid .tabs-custom li').each(function (index, element) {
                    if ($(element).hasClass('active')) {
                        activeTab = $(element);
                    }
                });

                if (activeTab.attr('id') == "liMsgsDirect") {
                    DashBoard.DashBoardDirectMessagesSearch(PageNo, 15);
                    break;
                }
                else if (activeTab.attr('id') == "liMsgsPractice") {
                    DashBoard.DashBoardMessagesSearch(PageNo, 15);
                    break;
                }
                else if (activeTab.attr('id') == "liMsgsPatient") {
                    DashBoard.DashBoardPatientMessagesSearch(PageNo, 15);
                    break;
                } else if (activeTab.attr('id') == "liMsgsLog") {
                    DashBoard.DashBoardLogMessagesSearch(PageNo, 15);
                    break;
                } else if (activeTab.attr('id') == "liMsgsDirectOutgoing") {
                    DashBoard.DashBoardOutgoingDirectMessagesSearch(PageNo, 15);
                    break;
                }
            case 'Tasks':
                $("#pnlUserTaskGrid li").each(function () {
                    if ($(this).text() == PageNo) {
                        $(this).attr("class", "active");
                    }
                    else
                        $(this).removeAttr("class");
                });
                DashBoard.DashBoardTasksSearch(PageNo, 15);
                break;
            case 'Encounter':
                $("#pnlEncounterGrid li").each(function () {
                    if ($(this).text() == PageNo) {
                        $(this).attr("class", "active");
                    }
                    else
                        $(this).removeAttr("class");
                });
                DashBoard.DashBoardEncounterSearch(PageNo, 15);
                break;

            case 'Documents':
                $("#pnlPatientDocumentGrid li").each(function () {
                    if ($(this).text() == PageNo) {
                        $(this).attr("class", "active");
                    }
                    else
                        $(this).removeAttr("class");
                });
                DashBoard.DashBoardDocumentSearch(PageNo, 15);

                break;
            case 'PatientChanges':
                $("#pnlPatientChangesGrid li").each(function () {
                    if ($(this).text() == PageNo) {
                        $(this).attr("class", "active");
                    }
                    else
                        $(this).removeAttr("class");
                });
                DashBoard.DashBoardPatientChangesSearch(PageNo, 15);

                break;
            case 'Copayment':
                $("#pnlCopayGrid li").each(function () {
                    if ($(this).text() == PageNo) {
                        $(this).attr("class", "active");
                    }
                    else
                        $(this).removeAttr("class");
                });
                DashBoard.DashBoardCopaySearch(PageNo, 15);
                break;
            case 'CCM':
                $("#CCMGrids li").each(function () {
                    if ($(this).text() == PageNo) {
                        $(this).attr("class", "active");
                    }
                    else
                        $(this).removeAttr("class");
                });
                DashBoard.DashBoardCCMEnrollmentInfoSearch(PageNo);
                break;
            case 'Data Change Request':
                $("#pnlDataChangeRequestGrid li").each(function () {
                    if ($(this).text() == PageNo) {
                        $(this).attr("class", "active");
                    }
                    else
                        $(this).removeAttr("class");
                });
                DashBoard.SearchDashBoardDataChangeRequest(PageNo);
                break;
        }
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var command = $('#ActiveWidget').val();

        switch (command) {
            case 'Payments':
                var currentPageNo = "";
                $("#pnlPaymentsGrid li").each(function () {
                    if ($(this).attr("class") == "active") {
                        $(this).removeAttr("class");
                        currentPageNo = parseInt($(this).text());
                    }

                });
                currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

                if (currentPageNo != "" && currentPageNo > 0) {
                    DashBoard.DashBoardPaymentsSearch(currentPageNo, 15);

                }
                break;
            case 'Appointments':
                var currentPageNo = "";
                $("#pnlSchAppointmentGrid li").each(function () {
                    if ($(this).attr("class") == "active") {
                        $(this).removeAttr("class");
                        currentPageNo = parseInt($(this).text());
                    }

                });
                currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

                if (currentPageNo != "" && currentPageNo > 0) {
                    DashBoard.DashBoardAppointmentSearch(currentPageNo, 15, null, null);

                }

                break;
            case 'Messages':
                var currentPageNo = "";
                var activeTab;
                $("#pnlUserMessagesGrid li").each(function () {
                    if ($(this).attr("class") == "active") {
                        // $(this).removeAttr("class");
                        currentPageNo = parseInt($(this).text());
                    }

                });


                $('#pnlUserMessagesGrid .tabs-custom li').each(function (index, element) {
                    if ($(element).hasClass('active')) {
                        activeTab = $(element);
                    }
                });
                currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

                if (currentPageNo != "" && currentPageNo > 0) {
                    if (activeTab.attr('id') == "liMsgsDirect") {
                        DashBoard.DashBoardDirectMessagesSearch(currentPageNo, 15);
                        break;
                    }
                    else if (activeTab.attr('id') == "liMsgsPractice") {
                        DashBoard.DashBoardMessagesSearch(currentPageNo, 15);
                        break;
                    }
                    else if (activeTab.attr('id') == "liMsgsPatient") {
                        DashBoard.DashBoardPatientMessagesSearch(currentPageNo, 15);
                        break;
                    }
                    else if (activeTab.attr('id') == "liMsgsLog") {
                        DashBoard.DashBoardLogMessagesSearch(currentPageNo, 15);
                        break;
                    } else if (activeTab.attr('id') == "liMsgsDirectOutgoing") {
                        DashBoard.DashBoardOutgoingDirectMessagesSearch(currentPageNo, 15);
                        break;
                    }


                }
                break;

            case 'Tasks':
                var currentPageNo = "";
                $("#pnlUserTaskGrid li").each(function () {
                    if ($(this).attr("class") == "active") {
                        $(this).removeAttr("class");
                        currentPageNo = parseInt($(this).text());
                    }

                });
                currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

                if (currentPageNo != "" && currentPageNo > 0) {
                    DashBoard.DashBoardTasksSearch(currentPageNo, 15);

                }
                break;
            case 'Encounter':
                var currentPageNo = "";
                $("#pnlEncounterGrid li").each(function () {
                    if ($(this).attr("class") == "active") {
                        $(this).removeAttr("class");
                        currentPageNo = parseInt($(this).text());
                    }

                });
                currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

                if (currentPageNo != "" && currentPageNo > 0) {
                    DashBoard.DashBoardEncounterSearch(currentPageNo, 15);

                }
                break;
            case 'Documents':
                var currentPageNo = "";
                $("#pnlPatientDocumentGrid li").each(function () {
                    if ($(this).attr("class") == "active") {
                        $(this).removeAttr("class");
                        currentPageNo = parseInt($(this).text());
                    }

                });
                currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

                if (currentPageNo != "" && currentPageNo > 0) {
                    DashBoard.DashBoardDocumentSearch(currentPageNo, 15);

                }
                break;
            case 'PatientChanges':
                var currentPageNo = "";
                $("#pnlPatientChangesGrid li").each(function () {
                    if ($(this).attr("class") == "active") {
                        $(this).removeAttr("class");
                        currentPageNo = parseInt($(this).text());
                    }

                });
                currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

                if (currentPageNo != "" && currentPageNo > 0) {
                    DashBoard.DashBoardPatientChangesSearch(currentPageNo, 15);

                }
                break;

            case 'Copayment':
                var currentPageNo = "";
                $("#pnlCopayGrid li").each(function () {
                    if ($(this).attr("class") == "active") {
                        $(this).removeAttr("class");
                        currentPageNo = parseInt($(this).text());
                    }

                });
                currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

                if (currentPageNo != "" && currentPageNo > 0) {
                    DashBoard.DashBoardCopaySearch(currentPageNo, 15);

                }
                break;
            case 'CCM':
                var currentPageNo = "";
                $("#CCMGrids li").each(function () {
                    if ($(this).attr("class") == "active") {
                        $(this).removeAttr("class");
                        currentPageNo = parseInt($(this).text());
                    }

                });
                currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

                if (currentPageNo != "" && currentPageNo > 0) {
                    DashBoard.DashBoardCCMEnrollmentInfoSearch(currentPageNo);

                }
                break;

        }

    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var command = $('#ActiveWidget').val();

        switch (command) {
            case 'Payments':
                var currentPageNo = "";
                $("#pnlPaymentsGrid li").each(function () {
                    if ($(this).attr("class") == "active") {
                        $(this).removeAttr("class");
                        currentPageNo = parseInt($(this).text());
                    }
                });

                currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
                if (currentPageNo != "" && currentPageNo <= TotalPages) {
                    DashBoard.DashBoardPaymentsSearch(currentPageNo, 15);
                }

                break;
            case 'Appointments':
                var currentPageNo = "";
                $("#pnlSchAppointmentGrid li").each(function () {
                    if ($(this).attr("class") == "active") {
                        $(this).removeAttr("class");
                        currentPageNo = parseInt($(this).text());
                    }
                });

                currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
                if (currentPageNo != "" && currentPageNo <= TotalPages) {
                    DashBoard.DashBoardAppointmentSearch(currentPageNo, 15, null, null);
                }

                break;
            case 'Messages':
                var currentPageNo = "";
                var activeTab = "";
                $("#pnlUserMessagesGrid li").each(function () {
                    if ($(this).attr("class") == "active") {
                        // $(this).removeAttr("class");
                        currentPageNo = parseInt($(this).text());
                    }
                });
                $('#pnlUserMessagesGrid .tabs-custom li').each(function (index, element) {
                    if ($(element).hasClass('active')) {
                        activeTab = $(element);
                    }
                });
                currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
                if (currentPageNo != "" && currentPageNo <= TotalPages) {
                    if (activeTab.attr('id') == "liMsgsDirect") {
                        DashBoard.DashBoardDirectMessagesSearch(currentPageNo, 15);
                        break;
                    }
                    else if (activeTab.attr('id') == "liMsgsPractice") {
                        DashBoard.DashBoardMessagesSearch(currentPageNo, 15);
                        break;
                    }
                    else if (activeTab.attr('id') == "liMsgsPatient") {
                        DashBoard.DashBoardPatientMessagesSearch(currentPageNo, 15);
                        break;
                    }
                    else if (activeTab.attr('id') == "liMsgsLog") {
                        DashBoard.DashBoardLogMessagesSearch(currentPageNo, 15);
                        break;
                    } else if (activeTab.attr('id') == "liMsgsDirectOutgoing") {
                        DashBoard.DashBoardOutgoingDirectMessagesSearch(currentPageNo, 15);
                        break;
                    }

                }
                break;

            case 'Tasks':
                var currentPageNo = "";
                $("#pnlUserTaskGrid li").each(function () {
                    if ($(this).attr("class") == "active") {
                        $(this).removeAttr("class");
                        currentPageNo = parseInt($(this).text());
                    }
                });

                currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
                if (currentPageNo != "" && currentPageNo <= TotalPages) {
                    DashBoard.DashBoardTasksSearch(currentPageNo, 15);
                }
                break;
            case 'Encounter':
                var currentPageNo = "";
                $("#pnlEncounterGrid li").each(function () {
                    if ($(this).attr("class") == "active") {
                        $(this).removeAttr("class");
                        currentPageNo = parseInt($(this).text());
                    }
                });

                currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
                if (currentPageNo != "" && currentPageNo <= TotalPages) {
                    DashBoard.DashBoardEncounterSearch(currentPageNo, 15);
                }
                break;
            case 'Documents':
                var currentPageNo = "";
                $("#pnlPatientDocumentGrid li").each(function () {
                    if ($(this).attr("class") == "active") {
                        $(this).removeAttr("class");
                        currentPageNo = parseInt($(this).text());
                    }
                });

                currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
                if (currentPageNo != "" && currentPageNo <= TotalPages) {
                    DashBoard.DashBoardDocumentSearch(currentPageNo, 15);
                }
                break;
            case 'PatientChanges':
                var currentPageNo = "";
                $("#pnlPatientChangesGrid li").each(function () {
                    if ($(this).attr("class") == "active") {
                        $(this).removeAttr("class");
                        currentPageNo = parseInt($(this).text());
                    }
                });

                currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
                if (currentPageNo != "" && currentPageNo <= TotalPages) {
                    DashBoard.DashBoardPatientChangesSearch(currentPageNo, 15);
                }
                break;
            case 'Copayment':
                var currentPageNo = "";
                $("#pnlCopayGrid li").each(function () {
                    if ($(this).attr("class") == "active") {
                        $(this).removeAttr("class");
                        currentPageNo = parseInt($(this).text());
                    }
                });

                currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
                if (currentPageNo != "" && currentPageNo <= TotalPages) {
                    DashBoard.DashBoardCopaySearch(currentPageNo, 15);
                }
                break;

            case 'CCM':
                var currentPageNo = "";
                $("#CCMGrids li").each(function () {
                    if ($(this).attr("class") == "active") {
                        $(this).removeAttr("class");
                        currentPageNo = parseInt($(this).text());
                    }
                });

                currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
                if (currentPageNo != "" && currentPageNo <= TotalPages) {
                    DashBoard.DashBoardCCMEnrollmentInfoSearch(currentPageNo);
                }
                break;

        }

    },


    // Functions for EMR (IrFan)
    CreateNote: function (PatientId, AppointmentId, ProviderId, ProviderName, AppointmentTime, VisitId, VisitDate, Reason, FacilityName, FacilityId, Room, NotesId) {
        if (PatientId != $('#PatientProfile #hfPatientId').val()) {
            Patient_Demographic.isFinanicialAlert = true;
        }
        $.when(setPatientBanner(PatientId, "1")).then(function () {
            if (CheckPatientDemoMissingDetails() == false) {
                DashBoard.NoteCreation(PatientId, AppointmentId, ProviderId, ProviderName, AppointmentTime, VisitId, VisitDate, Reason, FacilityName, FacilityId, Room, NotesId);
            }
            else {
                params["mode"] = "Edit";
                params["PatBanner"] = true;
                params["IsFill"] = true;
                params["FormsTitle"] = "Please complete missing demographics to continue!";
                params["GrandParent"] = "DashboardAppointment";
                params["PatientId"] = PatientId;
                params["AppointmentId"] = AppointmentId;
                params["ProviderId"] = ProviderId;
                params["ProviderName"] = ProviderName;
                params["AppointmentTime"] = AppointmentTime;
                params["VisitId"] = VisitId;
                params["VisitDate"] = VisitDate;
                params["Reason"] = Reason;
                params["FacilityName"] = FacilityName;
                params["FacilityId"] = FacilityId;
                params["Room"] = Room;
                params["NotesId"] = NotesId;
                LoadActionPan('demographicDetail', params);
            }
        });

    },

    NoteCreation: function (PatientId, AppointmentId, ProviderId, ProviderName, AppointmentTime, VisitId, VisitDate, Reason, FacilityName, FacilityId, Room, NotesId) {
        params["mode"] = "Add";
        params["NotesProviderId"] = ProviderId;
        params["PatientId"] = PatientId;
        params["AppointmentId"] = AppointmentId;
        params["NotesVisitTime"] = AppointmentTime;
        params["NotesVisitId"] = VisitId;
        params["ParentCntrlLoadid"] = "Dashboard";
        params["NotesProviderId"] = ProviderId;
        params["NotesProviderName"] = ProviderName;
        params["NotesVisitDate"] = VisitDate;
        params['ScheduleReason'] = Reason;
        params['NotesRoom'] = Room;
        params['NotesFacilityName'] = FacilityName;
        params['NotesFacilityId'] = FacilityId;
        params["RefProviderName"] = null;
        params["RefProviderId"] = null;
        params['ForProgressNote'] = false;

        if (NotesId != null && NotesId != '' && NotesId > 0) {
            params['NotesId'] = NotesId;
        }
        AppPrivileges.GetFormPrivileges("Notes_Notes", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {

            if (strMessage == "") {

                $("ul li[id*=mstrMenu]").hide();

                if ($("html").hasClass("sidebar-left-collapsed")) {
                    $("html").removeClass("sidebar-left-collapsed");
                }

                $("#anchorMainMenu").show();
                $("div[id*=mstrDiv]").hide();

                $.when(ClinicalMenuSettings.ClinicalMenuSettingsSearch(null)).then(function () {

                    $('#mstrTabClinical').siblings().removeClass('active');
                    $('#mstrTabClinical').addClass('active');
                    $('#ClinicalUL li').removeClass('nav-expanded nav-active');
                    $('#ClinicalUL li#clinicalMenuNotes').addClass('nav-expanded nav-active');
                    $('#ctrlPanDashBoard').hide();

                    /// Farooq - > Other way of select Tab
                    for (var i = 0; i < TabsArray.length; i++) {

                        if (TabsArray[i].TabID == "mstrTabDashBoard") TabsArray[i].Selected = false;
                        if (TabsArray[i].TabID == "mstrTabPatient") TabsArray[i].Selected = false;
                        if (TabsArray[i].TabID == "mstrTabClinical") TabsArray[i].Selected = true;
                    }
                    if (params.PanelID == "pnlDemographic") {
                        params.DemographicAutoUpdateActiveTab = "";
                    }
                    SelectTab("clinicalTabNotes", "false");

                    try {
                        $('#ctrlPanPatient').css('display', 'none');
                        document.getElementById("ctrlPanPatient").style.display = "none !important";
                        $('#ctrlPanPatient').css('display', 'none !important');
                    } catch (ex) {
                        console.log(ex);
                    }
                    Patient_Demographic.FillPatientAlertsCount();
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    CreateNoteSuperbill: function (PatientId, AppointmentId, ProviderId, ProviderName, AppointmentTime, VisitId, VisitDate, Reason, FacilityName, FacilityId, Room, NotesId, patientType, NoteDate, billInfoId) {

        var params = [];
        NotesId = NotesId == null ? 0 : NotesId;
        ProviderId = ProviderId != null ? ProviderId : "";
        refProviderId = ProviderId;
        FacilityId = FacilityId != null ? FacilityId : "";

        if (billInfoId == 0) {
            billInfoId = -1;
        }


        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'mstrTabDashBoard';
        params["ProviderId"] = ProviderId;
        params["PatientId"] = PatientId;
        params["NotesId"] = parseInt(NotesId);
        params["VisitId"] = VisitId;
        params["VisitDate"] = VisitDate;
        params["BillingInfoId"] = parseInt(billInfoId);
        params["BtnClicked"] = null;
        params["appId"] = AppointmentId;
        params["AppointmentDate"] = utility.formatDate(Date($('#pnlDashboard #appDate').text()));
        params["PatientType"] = null;
        params["NoteDate"] = utility.RemoveTimeFromDate(null, NoteDate);
        LoadActionPan('BillingInformation', params);
    },

    EditNote: function (PatientId, AppointmentId, ProviderId, ProviderName, AppointmentTime, VisitId, VisitDate, Reason, FacilityName, FacilityId, Room, NotesId) {
        AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                params["QuickAddPatient"] = true;
                var ParentCntrlLoadid = "Dashboard";
                DashBoard.EditProgressNote(NotesId, PatientId);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    CreateSuperBill: function (patientid) {

    },

    IsCheckedIn: function (objThis) {

        var visitStatus = $(objThis).attr('VisitStatus');

        if (visitStatus == '1') {
            $(objThis).attr('VisitStatus', '0');
        }
        else if (visitStatus == '0') {
            $(objThis).attr('VisitStatus', '1');
        }

        DashBoard.DashBoardAppointmentSearch(null, null, null, null);
    },

    NextDate: function () {
        var criteria = $('#pnlDashboard #appDate').text();

        var d = new Date(DashBoard.FormatDate(criteria));
        var newdate = DashBoard.AddSubDays(d, 1);
        var curdte = $.datepicker.formatDate('mm/dd/yy', newdate);

        $('#pnlDashboard #appDate').text(curdte);
        DashBoard.DashBoardAppointmentSearch(null, null, null, $.datepicker.formatDate('mm/dd/yy', newdate));
    },

    BackDate: function () {

        var criteria = $('#pnlDashboard #appDate').text();

        var d = new Date(DashBoard.FormatDate(criteria));
        var newdate = DashBoard.AddSubDays(d, -1);
        var curdte = $.datepicker.formatDate('mm/dd/yy', newdate);

        $('#pnlDashboard #appDate').text(curdte);
        DashBoard.DashBoardAppointmentSearch(null, null, null, $.datepicker.formatDate('mm/dd/yy', newdate));
    },

    AddSubDays: function (theDate, days) {

        return new Date(theDate.getTime() + days * 24 * 60 * 60 * 1000);

    },

    NotesDelete: function (NotesId, event, operationId) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        //Start//29/12/2015//Ahmad Raza//Privileges logic implemented
        AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = NotesId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Clinical_Notes.NotesDeleted(selectedValue).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                if (globalAppdata.IsProviderBulkSign == "True") {
                                    // operationId = 1 for ready to sign notes and operationId = 2 for Missing info notes
                                    if (operationId == 1) {
                                        DashBoard.DashBoardEncounterSearch();
                                    } else {
                                        DashBoard.DashBoardEncounterSearchMissingInfo()
                                    }
                                } else {
                                    //Clinical_Notes.NotesSearch(null);
                                    DashBoard.DashBoardEncounterSearchOld($("#pnlDashboard #OlddgvEncounterGrid_paginate li.active").text(), 15);
                                }
                                utility.DisplayMessages(response.Message, 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () {
                },
                    '1'
                        );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        //End//29/12/2015//Ahmad Raza//Privileges logic implemented
    },

    VBPWithReasoningLoad: function (NotesId, PatientId, ProviderId, isComponentSelect, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter) {
        var objDeffered = $.Deferred();
        Clinical_ProgressNote.loadVBPWithReasoning(VisitDate, VisitDate, PatientId, ProviderId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                DashBoard.params["VBPResponse"] = response;
                if (response.AllMeasuresReasoningDetailCount > 0) {
                    var arrNonCompliantPatients = $.grep(JSON.parse(response.AllMeasuresReasoningDetailLoad_JSON), function (a) {
                        return a.Patientid == PatientId && a.NoteId == NotesId;
                    });

                    if (arrNonCompliantPatients.length > 0) {
                        DashBoard.arrVBPReasoning[PatientId] = JSON.stringify(arrNonCompliantPatients);
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
                            $.when(DashBoard.openMissingAlert_VBP(null, null, null, 'mstrTabDashBoard', NotesId, PatientId, ProviderId, isComponentSelect, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter, PHQ2Missing, PHQ9Missing)).then(function () {
                                DashBoard.params.isVBPExists = 1;
                                objDeffered.resolve();
                            });
                        }, function () {
                            $.when(DashBoard.CQMWithReasoningLoad(NotesId, PatientId, ProviderId, isComponentSelect, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter)).then(function () {
                                objDeffered.resolve();
                            });
                        },
                               '<b>2017 Value Based Program Missing Data Alert</b>', "Yes, I do", "No, not this time"
                          );
                    }
                    else {
                        $.when(DashBoard.CQMWithReasoningLoad(NotesId, PatientId, ProviderId, isComponentSelect, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter)).then(function () {
                            objDeffered.resolve();
                        });
                    }
                }
                else {
                    $.when(DashBoard.CQMWithReasoningLoad(NotesId, PatientId, ProviderId, isComponentSelect, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter)).then(function () {
                        objDeffered.resolve();
                    });
                }
            }
            else {
                $.when(DashBoard.CQMWithReasoningLoad(NotesId, PatientId, ProviderId, isComponentSelect, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter)).then(function () {
                    objDeffered.resolve();
                });
                utility.DisplayMessages(response.Message, 3);
            }
        });
        return objDeffered;
    },

    openMissingAlert_VBP: function (BillingInformation, Obj, customSigMsg, prntctrl, NotesId, PatientId, ProviderId, isComponentSelect, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter, PHQ2Missing, PHQ9Missing) {
        var params = [];
        params["FromAdmin"] = "0";
        params["BillingInformation"] = BillingInformation;
        params["Obj"] = Obj;
        params["customSigMsg"] = customSigMsg;
        params["ParentCtrl"] = prntctrl;
        params["NoteId"] = NotesId;
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
        params["IsPhoneEncounter"] = IsPhoneEncounter;
        params["PHQ2Missing"] = PHQ2Missing;
        params["PHQ9Missing"] = PHQ9Missing;

        LoadActionPan('VBP_MissingDataAlert', params);
    },
    NotesStatusUpdate: function (NotesId, PatientId, ProviderId, isComponentSelect, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter, event) {
        if (event != null)
            event.stopPropagation();

        var objDeffered = $.Deferred();
        var CDSAlertCount = parseInt($(" #mainForm  li#CDSAlert span").text());
        if (!IsPhoneEncounter && CDSAlertCount > 0) {
            utility.DisplayMessages("Please change the Status of all the CDS Alerts before signing the Note.", 3);
        }
        else {
            DashBoard.VBPWithReasoningLoad(NotesId, PatientId, ProviderId, isComponentSelect, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter);

        }
    },


    CQMWithReasoningLoad: function (NotesId, PatientId, ProviderId, isComponentSelect, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter) {
        var objDeffered = $.Deferred();
        Clinical_ProgressNote.loadCQMWithReasoning(VisitDate, VisitDate, PatientId, ProviderId, NotesId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                DashBoard.params["CQMResponse"] = response;
                if (response.AllMeasuresReasoningDetailCount > 0) {
                    var arrNonCompliantPatients = $.grep(JSON.parse(response.AllMeasuresReasoningDetailLoad_JSON), function (a) {
                        return a.Patientid == PatientId;
                    });

                    DashBoard.arrCQMReasoning[PatientId] = JSON.stringify(arrNonCompliantPatients);
                    var CQMFoundMsg = "Our System found some <span class='red'>missing data</span> related to this patient."
                                    + " In order to qualify for the <b>2016 CQM incentives,</b> you must enter those <span class='red'>missing data values</span>"
                                    + " against the CQM measures that you have planned to report this year. Do you want to enter the data here before signing off this note?";

                    utility.myConfirm(CQMFoundMsg, function () {
                        objDeffered.resolve();
                        Clinical_Notes.openPatientList(PatientId, isComponentSelect, ProviderId, VisitDate, NotesId, null, "mstrTabDashBoard", BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId);
                    }, function () {
                        $.when(DashBoard.NotesStatusUpdateAfterCQM(NotesId, PatientId, ProviderId, isComponentSelect, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter)).then(function () {
                            objDeffered.resolve();
                        });
                    },
                          '<b>2016 CQM Missing Data Alert</b>', "Yes, I do", "No, not this time"
                      );

                }
                else {
                    $.when(DashBoard.NotesStatusUpdateAfterCQM(NotesId, PatientId, ProviderId, isComponentSelect, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter)).then(function () {
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


    NotesStatusUpdateAfterCQM: function (NotesId, PatientId, ProviderId, isComponentSelect, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter) {
        //Start//02-05-2016//Ahmad Raza//logic for CDS Alert
        var CDSAlertCount = parseInt($(" #mainForm  li#CDSAlert span").text());
        if (!IsPhoneEncounter && CDSAlertCount > 0) {
            utility.DisplayMessages("Please change the Status of all the CDS Alerts before signing the Note.", 3);
        }
        else {
            //Start//29/12/2015//Ahmad Raza//Privileges logic implemented
            AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    utility.myConfirm('Do you want to Sign this record?', function () {
                        $.when(Clinical_ProgressNote.signNote(NotesId, PatientId, false, IsPhoneEncounter, null, false)).done(function () {
                            var triggerLocation = 'Notes';
                            if (!IsPhoneEncounter) {
                                ClinicalCDSDetail.showCDSAlert(triggerLocation, 0);
                            }
                            $(" #mainForm #hfTriggerLocation").val('Notes');
                            Clinical_NotesSearch.SetNotesCount();
                            if (globalAppdata.IsProviderBulkSign == "True") {
                                DashBoard.DashBoardEncounterSearch(null, null, null);
                            } else {
                                DashBoard.DashBoardEncounterSearchOld(null, null, null);
                            }

                        });
                    }, function () {
                    },
                            'Confirm Sign'
                        );
                }
                else {
                    utility.DisplayMessages(strMessage, 2);
                }
            });
            //End//29/12/2015//Ahmad Raza//Privileges logic implemented
        }
        //End//02-05-2016//Ahmad Raza//logic for CDS Alert
    },

    CreateCharges: function (Obj, NotesId, PatientId, ProviderId, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID) {
        BillingInformation.params = Clinical_NotesSearch.initializeBillingInfoParams(NotesId, PatientId, ProviderId, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID);
        DashBoard.CreateObjectForBilling(POS);
    },

    CreateObjectForBilling: function (POSCode) {
        var facPOS = POSCode;
        BillingInformation.BillingInformationLoad().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                response.BillingInfoCPTFill_JSON = JSON.parse(response.BillingInfoCPTFill_JSON);
                response.BillingInfoICDFill_JSON = JSON.parse(response.BillingInfoICDFill_JSON);

                response.BillingInfoFill_JSON = JSON.parse(response.BillingInfoFill_JSON);

                var Obj = {
                };
                var ICDs = []

                for (var index in response.BillingInfoICDFill_JSON) {
                    var item = response.BillingInfoICDFill_JSON[index];
                    var currentICD = {
                    };
                    if (item.ICDType == "10") {
                        currentICD.ICDCode9 = '';
                        currentICD.ICDDescription9 = item.ICDCodeDescription.replace(/"/g, "'");;

                        currentICD.ICDCode10 = item.ICDCode;
                        currentICD.ICDDescription10 = item.ICDCodeDescription.replace(/"/g, "'");;

                        currentICD.SNOMEDCode = item.SNOMEDID;
                        currentICD.SNOMEDDescription = item.SNOMEDDescription;
                    }
                    else {
                        currentICD.ICDCode9 = item.ICDCode;
                        currentICD.ICDDescription9 = item.ICDCodeDescription.replace(/"/g, "'");;

                        currentICD.ICDCode10 = '';
                        currentICD.ICDDescription10 = item.ICDCodeDescription.replace(/"/g, "'");;
                        currentICD.SNOMEDCode = item.SNOMEDID;
                        currentICD.SNOMEDDescription = item.SNOMEDDescription;
                    }
                    ICDs.push(currentICD);
                }


                var CPTs = []

                for (var index in response.BillingInfoCPTFill_JSON) {
                    var item = response.BillingInfoCPTFill_JSON[index];
                    var currentCPT = {
                    };
                    currentCPT.CPTCode = item.CPTCode;
                    currentCPT.CPTDescription = item.CPTDescription.replace(/"/g, "'").replace(/&#39;/g, "");
                    currentCPT.Modifier1 = item.Modifier1;
                    currentCPT.Modifier2 = item.Modifier2;
                    currentCPT.Modifier3 = item.Modifier3;
                    currentCPT.Modifier4 = item.Modifier4;
                    currentCPT.DxPointer1 = item.ICDPointer1;
                    currentCPT.DxPointer2 = item.ICDPointer2;
                    currentCPT.DxPointer3 = item.ICDPointer3;
                    currentCPT.DxPointer4 = item.ICDPointer4;
                    currentCPT.UnitsId = item.Units;
                    currentCPT.POS = facPOS;
                    currentCPT.DOSFrom = item.DOSFrom;
                    currentCPT.DOSTo = item.DOSTo;

                    CPTs.push(currentCPT);

                }

                Obj.CPTs = CPTs;
                Obj.ICDs = ICDs;
                Obj.POS = facPOS;
                BillingInformation.LoadAttachecdICDsAndCPTs().done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {

                        response.ProblemListFill_JSON = JSON.parse(response.ProblemListFill_JSON);
                        response.ProcedureListFill_JSON = JSON.parse(response.ProcedureListFill_JSON);

                        response.ProblemListFill_JSON = $.grep(response.ProblemListFill_JSON, function (a) {
                            return a.IsNoteLinked == "True";
                        });
                        response.ProcedureListFill_JSON = $.grep(response.ProcedureListFill_JSON, function (a) {
                            return a.IsNoteLinked == "1" || (parseInt(a.VaccineHxId) > 0 || parseInt(a.ImmTherInjectionId) > 0);
                        });


                        var counter = 0;
                        var objData = {
                        };
                        if (BillingInformation.params.BillingInfoId > 0) {
                            objData["BillingInfoId"] = BillingInformation.params.BillingInfoId;
                        }
                        else {
                            objData["BillingInfoId"] = '-1'
                        }
                        objData["POS"] = facPOS;
                        objData["commandType"] = "BILLING_INFORMATION_SAVE";
                        objData["NotesId"] = BillingInformation.params.NotesId;
                        objData["PatientId"] = BillingInformation.params.PatientId;
                        objData["ProviderId"] = BillingInformation.params["ProviderId"];
                        objData["VisitId"] = BillingInformation.params.VisitId;
                        objData["Status"] = 'Draft';
                        objData["VisitDate"] = BillingInformation.params.VisitDate;
                        objData.ICDs = [];
                        objData.CPTs = [];

                        for (var i in response.ProblemListFill_JSON) {
                            item = response.ProblemListFill_JSON[i];
                            var ICD = {
                            };
                            ICD.ICDCode9 = item.ICD9;
                            ICD.ICDCode10 = item.ICD10;
                            ICD.ICDDescription9 = item.ICD9_Description;
                            ICD.ICDDescription10 = item.ICD10_Description;
                            ICD.SNOMEDCode = item.SNOMEDID;
                            ICD.SNOMEDDescription = item.SNOMED_DESCRIPTION
                            objData.ICDs.push(ICD);
                        }


                        for (var i in response.ProcedureListFill_JSON) {
                            item = response.ProcedureListFill_JSON[i];
                            var currentCPT = {
                            };
                            currentCPT.CPTCode = item.CPTCode;
                            currentCPT.CPTDescription = item.CPT_DESCRIPTION.replace(/"/g, "'").replace(/&#39;/g, "").replace(/&amp;/g, '&');
                            currentCPT.Modifier1 = item.Modifier;
                            currentCPT.Modifier2 = "";
                            currentCPT.Modifier3 = "";
                            currentCPT.Modifier4 = "";

                            var icd_cods = item.ICDCodes.split(',');
                            var pinter1 = icd_cods.length > 0 && icd_cods[0] != '' ? Clinical_ProgressNote.Getpointers(icd_cods[0], objData.ICDs) : '1';
                            var pinter2 = icd_cods.length > 1 && icd_cods[1] != '' ? Clinical_ProgressNote.Getpointers(icd_cods[1], objData.ICDs) : '';
                            var pinter3 = icd_cods.length > 2 && icd_cods[2] != '' ? Clinical_ProgressNote.Getpointers(icd_cods[2], objData.ICDs) : '';
                            var pinter4 = icd_cods.length > 3 && icd_cods[3] != '' ? Clinical_ProgressNote.Getpointers(icd_cods[3], objData.ICDs) : '';
                            currentCPT.DxPointer1 = pinter1;
                            currentCPT.DxPointer2 = pinter2;
                            currentCPT.DxPointer3 = pinter3;
                            currentCPT.DxPointer4 = pinter4;
                            currentCPT.UnitsId = item.Unit;
                            currentCPT.Unit = item.Unit;
                            currentCPT.DOSFrom = item.StartDate;
                            currentCPT.DOSTo = item.EndDate;
                            currentCPT.CPTSNOMEDCodeId = item.SNOMEDID;
                            currentCPT.CPTSNOMEDDescription = item.SNOMED_DESCRIPTION;
                            currentCPT.txtFEE = item.Fee;
                            currentCPT.Fee = item.Fee;
                            if (i == 0) {
                                currentCPT.Copay = item.Copay;
                            } else {
                                currentCPT.Copay = "";
                            }
                            if (item.ExpectedFee && item.ExpectedFee != "0") {
                                currentCPT.hfExpectedFee = item.ExpectedFee;
                                currentCPT.Expectedfee = item.ExpectedFee;
                            }
                            else {
                                currentCPT.hfExpectedFee = "0.00";
                                currentCPT.Expectedfee = "0.00";
                            }
                            currentCPT.PatCharges = item.PatCharges;
                            currentCPT.Inscharges = item.Inscharges;
                            currentCPT.POS = facPOS;
                            objData.CPTs.push(currentCPT);
                        }
                        BillingInformation.BillingObj = objData;
                        BillingInformation.AttachtedCPTData = objData.CPTs;
                        BillingInformation.AttachtedCPTData.reverse();
                        if (BillingInformation.params.BillingInfoId > 0) {
                            BillingInformation.CreateCharge(BillingInformation.BillingObj, true);
                        }
                        else {
                            BillingInformation.BillingInfoSave(objData).done(function (InnerResponse) {
                                InnerResponse = JSON.parse(InnerResponse);
                                if (InnerResponse.status != false) {
                                    BillingInformation.BillingObj.BillingInfoId = BillingInformation.params.BillingInfoId = InnerResponse.BillingInfoId;
                                    BillingInformation.CreateCharge(BillingInformation.BillingObj, true);
                                }
                            });
                        }
                    }
                });
                findInDiv.hide(true);
            }
        });
    },
    LoadAttachecdICDsAndCPTs: function (NotesId, patientID) {
        var objData = new Object();
        objData["NotesId"] = NotesId;
        objData["PatientId"] = patientID;
        objData["commandType"] = "LoadProceduresAndProblemsByNoteAndPatientId";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "BillingInformation", "BillingInformation");
    },
    // By: Khaleel Ur Rehman
    // Date : 16-March-2016
    // Purpose : To Handle Co-Sign when view a Note
    NotesUpdateForCosignORAmendment: function (NotesID, radioval, jsondata, coSignedProviderId) {

        /*var objData = new JSON.constructor();
        if (jsondata)
            objData = JSON.stringify(jsondata);*/
        var objData = {
        };
        jsondata = JSON.parse(jsondata);
        objData["NotesID"] = NotesID;
        objData["CommentsCosign"] = unescape(jsondata.txtCommentsCosign);
        objData["Radioval"] = radioval;
        objData["ProviderId"] = jsondata.ProviderId;
        objData["CommandType"] = "UPDATE_NOTES_COSIGN";
        objData["CoSignedProviderId"] = coSignedProviderId;

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "UpdateDashBoard");
    },
    NotesUpdateCosign: function (NotesId, radioval, jsonData, coSignedProviderId, Parentctrl) {
        DashBoard.NotesUpdateForCosignORAmendment(NotesId, radioval, jsonData, coSignedProviderId).done(function (response) {
            if (response.status != false) {
                utility.DisplayMessages("Successfully Co-Signed!", 1);
                Clinical_NotesCoSign.UnLoadTab();
                //Start || 15 August, 2016 || ZeeshanAK || Fix for EMR-14
                // Clinical_NotesView.NotesPreview(NotesId, Clinical_ProgressNote.params.patientID, Clinical_ProgressNote.params.CurrentNotesProviderId);
                //End   || 15 August, 2016 || ZeeshanAK || Fix for EMR-14

                if (Parentctrl == "clinicalTabProgressNote") {

                    var AmendfromProgress = "cosignfromProgress";
                    //Clinical_NotesView.NotesPreview(NotesId, Clinical_ProgressNote.params.patientID, Clinical_ProgressNote.params.CurrentNotesProviderId, AmendfromProgress, null, null);
                    Clinical_NotesView.PrintReports(NotesId, Clinical_ProgressNote.params.patientID, Clinical_ProgressNote.params.CurrentNotesProviderId, AmendfromProgress, 'disable')

                } else {
                    Clinical_NotesView.PrintReports(NotesId, null, Clinical_ProgressNote.params.CurrentNotesProviderId, null, 'disable')
                }


                /*setTimeout(function () {
                    Clinical_NotesView.UnLoad();
                }, 200);*/
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    // By: Khaleel Ur Rehman
    // Date : 16-March-2016
    // Purpose : To Handle Amendment when view a Note
    NotesUpdateForAmendment: function (NotesID, action, jsondata, IsAmendmentForBilling) {


        /*var objData = new JSON.constructor();
        if (jsondata)
            objData = JSON.stringify(jsondata);*/
        jsondata = JSON.parse(jsondata);
        var objData = {
        };
        objData["NotesID"] = NotesID;
        objData["Action"] = action;
        var secLookupId;
        var compLookupId;
        /*objData["CommentsAmendment"] = objData.txtCommentsCosign;
        objData["Comments"] = objData.AmendmentComments;
        objData["RequestedBy"] = objData.ddlAmendment;*/

        //objData["CommentsAmendment"] = jsondata.txtCommentsCosign;
        NoteComponents.filter(function (itm, indx) {
            if (itm.value == 'Amendment')
                compLookupId = itm.id;
        });
        objData["IsAmendmentForBilling"] = IsAmendmentForBilling;
        objData["NoteSectionsLookupId"] = null;
        objData["NoteComponentsLookupId"] = compLookupId;
        objData["Comments"] = unescape(jsondata.AmendmentComments);
        objData["RequestedBy"] = jsondata.ddlAmendment;

        objData["CommandType"] = "UPDATE_NOTES_AMENDMENT";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "UpdateDashBoard");
    },
    NotesUpdateAmendment: function (NotesId, action, jsonData, Parentctrl, IsAmendmentForBilling) {
        if (IsAmendmentForBilling && IsAmendmentForBilling == "1") {
            IsAmendmentForBilling = true;
        }
        else {
            IsAmendmentForBilling = false;
        }
        DashBoard.NotesUpdateForAmendment(NotesId, action, jsonData, IsAmendmentForBilling).done(function (response) {
            if (response.status != false) {
                utility.DisplayMessages("Successfully Updated!", 1);
                Clinical_NotesAmendment.UnLoadTab();

                //Start || 15 August, 2016 || ZeeshanAK || Fix for EMR-15
                //End   || 15 August, 2016 || ZeeshanAK || Fix for EMR-15
                if (Parentctrl == "clinicalTabProgressNote") {
                    SelectTab("clinicalTabProgressNote", "false"); ClinicalMenuClick(event, null, null, null, 'clinicalTabProgressNote', 'button');
                    var AmendfromProgress = "AmendfromProgress";
                    //Clinical_NotesView.NotesPreview(NotesId, Clinical_ProgressNote.params.patientID, Clinical_ProgressNote.params.CurrentNotesProviderId, AmendfromProgress, null, null);
                    Clinical_NotesView.PrintReports(NotesId, Clinical_ProgressNote.params.patientID, Clinical_ProgressNote.params.CurrentNotesProviderId, AmendfromProgress, 'disable')

                } else {

                    Clinical_NotesView.PrintReports(NotesId, null, Clinical_ProgressNote.params.CurrentNotesProviderId, null, 'disable');
                }
                /*setTimeout(function () {
                    Clinical_NotesView.UnLoad();
                }, 200);*/
            }

            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    // End By Khaleel Ur Rehman.
    //-----------------------------

    NotesPreview: function (NotesId, PatientId, ProviderId, Grid, event, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, IsPhoneEncounter, BillingInfoStatus) {
        if (event != null) {
            if ($(event.target).is('i[class*="fa fa-check"]') || $(event.target).is('i[class*="fa fa-history"]') || $(event.target).is('a[class*="CalimOpen"]')) {
                return;
            }
            event.stopPropagation();
        }
        var params = [];
        params["FromAdmin"] = "0";
        params["NotesId"] = NotesId;
        params["Grid"] = Grid;
        params["PatientId"] = PatientId;
        params["ProviderId"] = ProviderId;
        params["ParentCtrl"] = 'mstrTabDashBoard';

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
        params["BillingInfoStatus"] = BillingInfoStatus;

        LoadActionPan('Clinical_NotesView', params);
    },


    NotesUpdate: function (NotesID) {


        var objData = new JSON.constructor();
        objData["NotesID"] = NotesID;
        objData["CommandType"] = "UPDATE_NOTES_STATUS";
        var secLookupId;
        var compLookupId;
        NoteComponents.filter(function (itm, indx) {
            if (itm.value == 'Signature')
                compLookupId = itm.id;
        });
        NoteSections.filter(function (itm, indx) {
            if (itm.value == 'Progress')
                secLookupId = itm.id;
        });
        objData["NoteSectionsLookupId"] = secLookupId;
        objData["NoteComponentsLookupId"] = compLookupId;
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "UpdateDashBoard");
    },

    NotesDeleted: function (NotesId) {
        var data = "NotesID=" + NotesId;
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "CLINICAL_NOTES", "DELETE_CLINICAL_NOTES");
    },

    EditProgressNote: function (NotesId, PatientId, event) {
        if (event != null)
            event.stopPropagation();

        //Start//29/12/2015//Ahmad Raza//Privileges logic implemented
        AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                params["QuickAddPatient"] = true;

                EMRUtility.CreateNote("Edit", PatientId, null, null, null, null, null, null, null, null,
                    null, null, NotesId, false, 'Schedular', null, null);

                //Start//15-03-2016//Ahmad Raza//showing CDS Alert icon on patient selection
                $(" #mainForm  li#CDSAlert").show();
                if (globalAppdata.IsImmunizationAlert != "False") {
                    //$(" #mainForm  li#ImmunizationAlert").show();
                }
                $(" #mainForm #hfTriggerLocation").val('FaceSheet');
                //End//15-03-2016//Ahmad Raza//showing CDS Alert icon on patient selection
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        //End//29/12/2015//Ahmad Raza//Privileges logic implemented

    },

    // Open ESuperBill on redirect to notes screen on edit note
    EditProgressNoteICDCPT: function (event, NotesId, PatientId, VisitId) {
        if (event != null)
            event.stopPropagation();

        //Start//29/12/2015//Ahmad Raza//Privileges logic implemented
        AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                params["QuickAddPatient"] = true;

                $.when(EMRUtility.CreateNote("Edit", PatientId, null, null, null, null, VisitId, null, null, null,
                    null, null, NotesId, false, 'Schedular', null, null)).then(function () {
                        if (CheckPatientDemoMissingDetails() == false) {
                            setTimeout(function () {
                                DashBoard.OpenESuperBill();
                            }, 300);
                        }
                    });

                //Start//15-03-2016//Ahmad Raza//showing CDS Alert icon on patient selection
                $(" #mainForm  li#CDSAlert").show();
                if (globalAppdata.IsImmunizationAlert != "False") {
                }
                $(" #mainForm #hfTriggerLocation").val('FaceSheet');
                //End//15-03-2016//Ahmad Raza//showing CDS Alert icon on patient selection
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        //End//29/12/2015//Ahmad Raza//Privileges logic implemented

    },

    OpenESuperBill: function () {
        if ($("#frmClinicalProgressNote #btnBillingInfo").length > 0) {
            Clinical_ProgressNote.BillingInfo(true);
        } else {
            setTimeout(DashBoard.OpenESuperBill, 200);
        }
    },


    // Start 02/12/2015 Muhammad Irfan Bug # 64
    FormatDate: function (date) {
        var obj = {
            January: 1, February: 2, March: 3, April: 4, May: 5, June: 6, July: 7, August: 8, September: 9, October: 10, November: 11, December: 12
        };
        var temp = date.split('/');
        return temp[2] + "/" + temp[0] + "/" + temp[1];

        //var obj = {
        //    January: 1, February: 2, March: 3, April: 4, May: 5, June: 6, July: 7, August: 8, September: 9, October: 10, November: 11, December: 12
        //};
        //var temp = date.split('/');
        //return temp[2] + "/" + obj[temp[1]] + "/" + temp[0];
    },
    // End 02/12/2015 Muhammad Irfan Bug # 64


    /* Start 12/04/2016 Muhammad Irfan Messages Functions for DashBoard */
    ComposeMessage: function () {
        AppPrivileges.GetFormPrivilegesByModule("Messages", "ADD", "DashBoard", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE_BY_MODULE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = 'mstrTabDashBoard';
                params["mode"] = 'Add';
                params["MessageType"] = 'Practice';
                LoadActionPan('Patient_MessageCreate', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);

        });


    },

    ComposePatientMessage: function () {
        AppPrivileges.GetFormPrivilegesByModule("Messages", "ADD", "DashBoard", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE_BY_MODULE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = 'mstrTabDashBoard';
                params["mode"] = 'Add';
                params["MessageType"] = 'Patient';
                LoadActionPan('Patient_MessageCreate', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);

        });

    },

    DeleteUserMessage: function (UserMessageIds, event, type) {
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivilegesByModule("Messages", "DELETE", "DashBoard", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE_BY_MODULE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    DashBoard.DeleteUserMessage_DBCall(UserMessageIds).done(function (response) {
                        if (response.status != false) {

                            var activeTab;
                            $('#pnlUserMessagesGrid .tabs-custom li').each(function (index, element) {
                                if ($(element).hasClass('active')) {
                                    activeTab = $(element);
                                }
                            });

                            if (activeTab.attr('id') == "liMsgsDirect") {
                                var table1 = $('#dgvDirectMessagesGrid').DataTable();
                                table1.row('.active').remove().draw(false);
                                DashBoard.DashBoardDirectMessagesSearch();
                                utility.DisplayMessages(response.Message, 1);
                            }
                            else if (activeTab.attr('id') == "liMsgsPractice") {
                                var table1 = $('#dgvUserMessagesGrid').DataTable();
                                table1.row('.active').remove().draw(false);
                                DashBoard.DashBoardMessagesSearch();
                            }
                            else if (activeTab.attr('id') == "liMsgsPatient") {
                                var table1 = $('#dgvPatientMessagesGrid').DataTable();
                                table1.row('.active').remove().draw(false);
                                DashBoard.DashBoardPatientMessagesSearch();
                            }
                        } else {
                            utility.DisplayMessages(response.Message, 3);
                        }

                    });
                }, function () {
                },
                   '1'
                       );
            }
            else
                utility.DisplayMessages(strMessage, 2);

        });




    },
    DeleteSelectedUserMessages: function (UserMessageIds) {

        DashBoard.DeleteUserMessage_DBCall(UserMessageIds).done(function (response) {
            if (response.status != false) {



                utility.DisplayMessages(response.Message, 1);

            } else {
                utility.DisplayMessages(response.Message, 3);
            }
            var activeTab;
            $('#pnlUserMessagesGrid .tabs-custom li').each(function (index, element) {
                if ($(element).hasClass('active')) {
                    activeTab = $(element);
                }
            });

            if (activeTab.attr('id') == "liMsgsDirect") {
                var table1 = $('#dgvDirectMessagesGrid').DataTable();
                table1.row('.active').remove().draw(false);
                DashBoard.DashBoardDirectMessagesSearch();

            }
            else if (activeTab.attr('id') == "liMsgsPractice") {
                var table1 = $('#dgvUserMessagesGrid').DataTable();
                table1.row('.active').remove().draw(false);
                DashBoard.DashBoardMessagesSearch();

            }
            else if (activeTab.attr('id') == "liMsgsPatient") {
                var table1 = $('#dgvPatientMessagesGrid').DataTable();
                table1.row('.active').remove().draw(false);
                DashBoard.DashBoardPatientMessagesSearch();

            }
            $("#pnlDashboard #pnlUserMessagesGrid #dgvUserMessagesGrid thead tr #selectMessages").prop('checked', false);
            $("#pnlDashboard #pnlUserMessagesGrid #dgvPatientMessagesGrid thead tr #selectMessages").prop('checked', false);
        });

    },

    DeleteUserMessage_DBCall: function (UserMessageIds) {

        var objData = new Object();
        objData["CommandType"] = "delete_message";
        objData["UserMessagesIds"] = UserMessageIds;
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Messages", "Messages");


    },

    SelectAllUserMessagesChkBx: function (obj, event) {

        if (event != null) {
            event.stopPropagation();
        }
        var objCheck = $(obj);

        if ((objCheck).is(':checked')) {

            $('#dgvUserMessagesGrid input[type="checkbox"]').prop('checked', 'checked');
        }

        else {
            $('#dgvUserMessagesGrid input[type=checkbox]').each(function () {

                $(this).attr('checked', false);
            });
        }

    },

    SelectAllDirectUserMessagesChkBx: function (obj, event) {

        if (event != null) {
            event.stopPropagation();
        }
        var objCheck = $(obj);

        if ((objCheck).is(':checked')) {

            $('#dgvDirectMessagesGrid input[type="checkbox"]').prop('checked', 'checked');
        }

        else {
            $('#dgvDirectMessagesGrid input[type=checkbox]').each(function () {

                $(this).attr('checked', false);
            });
        }

    },

    SelectAllPatientUserMessagesChkBx: function (obj, event) {

        if (event != null) {
            event.stopPropagation();
        }
        var objCheck = $(obj);

        if ((objCheck).is(':checked')) {

            $('#dgvPatientMessagesGrid input[type="checkbox"]').prop('checked', 'checked');
        }

        else {
            $('#dgvPatientMessagesGrid input[type=checkbox]').each(function () {

                $(this).attr('checked', false);
            });
        }

    },
    DeleteMultiplePatientMessages: function () {


        var selected = [];
        var msgIds;
        $('#dgvPatientMessagesGrid > tbody > tr').each(function () {
            if ($(this).find('input:checked').is(':checked')) {
                selected.push($(this).attr('id'));
                //selected.push($(this).attr('id').replace('checkbox', ''));
            }
        });

        for (var w = 0; w <= selected.length; w++) {
            msgIds += selected[w] + ',';
        }
        msgIds = msgIds.replace('undefined,', '');
        msgIds = msgIds.replace('undefined', '');
        if (msgIds != "") {

            DashBoard.DeleteMultiplePatientMessages_DBCall(msgIds);

        } else {
            utility.DisplayMessages("Please Select message.", 3);
        }

    },
    DeleteMultipleUserMessages: function () {


        var selected = [];
        var msgIds;
        $('#dgvUserMessagesGrid > tbody > tr').each(function () {
            if ($(this).find('input:checked').is(':checked')) {
                selected.push($(this).attr('id'));
                //selected.push($(this).attr('id').replace('checkbox', ''));
            }
        });

        for (var w = 0; w <= selected.length; w++) {
            msgIds += selected[w] + ',';
        }
        msgIds = msgIds.replace('undefined,', '');
        msgIds = msgIds.replace('undefined', '');
        if (msgIds != "") {

            DashBoard.DeleteMultipleUserMessages_DBCall(msgIds);

        } else {
            utility.DisplayMessages("Please Select message.", 3);
        }

    },
    DeleteMultiplePatientMessages_DBCall: function (UserMessageIds) {

        utility.myConfirm('40', function () {
            DashBoard.DeleteUserMessage_DBCall(UserMessageIds).done(function (response) {
                if (response.status != false) {
                    var table1 = $('#dgvPatientMessagesGrid').DataTable();
                    table1.row('.active').remove().draw(false);
                    DashBoard.DashBoardPatientMessagesSearch();
                    utility.DisplayMessages(response.Message, 1);
                    //if ($('#dgvUserMessagesGrid > tbody > tr').length < 1) {
                    $('#dgvPatientMessagesGrid #chkSelectAllMessages').attr('checked', false);
                    //}
                } else {
                    utility.DisplayMessages(response.Message, 3);
                }

            });
        }, function () {
        },
                    'Confirm Delete'
    );



    },
    DeleteMultipleUserMessages_DBCall: function (UserMessageIds) {

        utility.myConfirm('40', function () {
            DashBoard.DeleteUserMessage_DBCall(UserMessageIds).done(function (response) {
                if (response.status != false) {
                    var table1 = $('#dgvUserMessagesGrid').DataTable();
                    table1.row('.active').remove().draw(false);
                    DashBoard.DashBoardMessagesSearch();
                    utility.DisplayMessages(response.Message, 1);
                    //if ($('#dgvUserMessagesGrid > tbody > tr').length < 1) {
                    $('#dgvUserMessagesGrid #chkSelectAllMessages').attr('checked', false);
                    //}
                } else {
                    utility.DisplayMessages(response.Message, 3);
                }

            });
        }, function () {
        },
                    'Confirm Delete'
    );



    },

    ViewUserMessage: function (UserMessageId, event, Type) {

        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'mstrTabDashBoard';
        params["mode"] = 'Edit';
        params["Isopentask"] = '1';
        params["MessageType"] = Type;
        params["UserMessageId"] = UserMessageId;
        LoadActionPan('Patient_MessageCreate', params);

    },

    ViewDirectMessage: function (UserMessageId, event, Type) {

        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'mstrTabDashBoard';
        params["mode"] = 'Edit';
        params["Isopentask"] = '1';
        params["MessageType"] = Type;
        params["UserMessageId"] = UserMessageId;
        LoadActionPan('Patient_MessageCompose', params);

    },



    ViewUserTask: function (UserMessageId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'mstrTabDashBoard';
        //params["mode"] = 'Edit';
        params["UserMessageId"] = UserMessageId;
        LoadActionPan('Patient_TaskDetail', params);

    },

    /* Start 12/04/2016 Muhammad Irfan Messages Functions for DashBoard */


    //FormatDate: function (date) {
    //    var obj = {
    //        January: 1, February: 2, March: 3, April: 4, May: 5, June: 6, July: 7, August: 8, September: 9, October: 10, November: 11, December: 12
    //    };
    //    var temp = date.split('/');
    //    return temp[2] + "/" + obj[temp[1]] + "/" + temp[0];
    //},

    SearchDocument: function (JASONData, patientId, PageNumber, RowsPerPage) {
        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 15 : RowsPerPage;

        var objData = new JSON.constructor();

        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        objData["DOSFrom"] = $('#frmDashboard #gridControlDoc #dpDOSDateFrom').val();
        objData["DOSTo"] = $('#frmDashboard #gridControlDoc #dpDOSDateTo').val();
        if ($("#pnlDashboard #pnlPatientDocumentGrid #txtFullName").val() != "")
            objData["PatientId"] = $('#frmDashboard #pnlPatientDocumentGrid #hfPatientId').val();
        objData["DocAssignToReview"] = $('#frmDashboard #gridControlDoc #ddlDocumentReview').val();
        objData["DocPriority"] = $('#frmDashboard #gridControlDoc #ddlDocumentPriority').val();
        objData["DocStatus"] = $('#frmDashboard #gridControlDoc #ddlDocumentStatus option:selected').text();
        if (objData["DocStatus"] == "-Select-") {
            objData["DocStatus"] = null;
        }
        objData["CommandType"] = "Search_Documents";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "SearchDashBoard");
    },
    UserTaskSearch: function (MessageId, AssignedToId, MsgType, MsgStatusId, PageNumber, RowsPerPage) {

        if (MessageId == null) {
            MessageId = 0;
        }
        if (AssignedToId == null) {
            AssignedToId = 0;
        }
        if (MsgType == null) {
            MsgType = "Task";
        }
        ////PMS-863
        //if (MsgStatusId == null) {
        //    MsgStatusId = 2;// 2 stands for Task
        //}
        //
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }

        var objData = new JSON.constructor();
        objData["MessageId"] = MessageId;
        objData["AssignedToId"] = AssignedToId;
        objData["MessageType"] = MsgType;
        objData["MsgStatusId"] = MsgStatusId;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        objData["CommandType"] = "Search_Tasks";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "SearchDashBoard");
    },

    //Start Added by Humaira Yousaf for referral widget
    DashBoardReferralSearch: function (pageno, rpp, gridData) {

        if ($("#pnlDashboard #pnlReferralsGrid").css("display") == "none") {
            $("#pnlDashboard #pnlReferralsGrid").show();
        }
        DashBoard.fillRefferal();
        DashBoard.DashBoardIncomingReferralGridLoad(null, pageno, rpp);
        //DashBoard.DashBoardOutgoingReferralGridLoad(null, pageno, rpp);
    },
    DashBoardIncomingReferralGridLoad: function (response, pageNo, rpp) {
        var PnlResult = "";
        var dgvDivId = "";
        var pagingDivId = "";
        PnlResult = "pnlInComingReferalDS_Result";
        dgvDivId = "dgvInComingReferralDS";
        pagingDivId = "dgvInComingReferralDS_Paging";

        DashBoard.LoadIncomingReferral('{}', 0, pageNo, rpp).done(function (response) {
            response = JSON.parse(response)
            if (response.status != false) {
                DashBoard.selectUnselectReferralTabs("listIncomingReferrals");
                DashBoard.ReferralGridLoadNew('Incoming', response);

                var TableControl = 'pnlDashboard' + " #pnlInComingReferalDS_Result" + " #" + pagingDivId;
                var PagingPanelControlID = 'pnlDashboard' + " #pnlInComingReferalDS_Result" + " #" + pagingDivId;
                var ClassControlName = "DashBoard";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.ReferralListCount, pageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                    DashBoard.DashBoardIncomingReferralGridLoad(PrimaryID, PageNumber, ResultPerPage);
                }), 10);
            }
            else
                utility.DisplayMessages(response.Message, 3);
        });
    },

    LoadIncomingReferral: function (ReferralData, ReferralId, PageNumber, RowsPerPage) {

        SearchFormDivId = "IncomingReferralSearch";

        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var self = $('#pnlDashboard' + ' #' + SearchFormDivId);
        var objData = self != null ? self.getMyJSONByName() : "{}";
        objData = JSON.parse(objData);
        objData["PatientId"] = $('#pnlDashboard #ctrlPanIncomingReferral #txtFullName').val() == "" ? "" : $('#pnlDashboard #ctrlPanIncomingReferral #hfPatientId').val();
        objData["IsActive"] = "1";
        objData["ReferralId"] = ReferralId;

        objData["PatientInsurance"] = $('#pnlDashboard #IncomingReferralSearch #hfInsurancePlanId').val() == "" ? '0' : $('#pnlDashboard #IncomingReferralSearch #hfInsurancePlanId').val();

        objData["ProviderId"] = $('#pnlDashboard' + ' #IncomingReferralSearch #ddlReferralTo').val();
        objData["RefProviderId"] = $('#pnlDashboard' + ' #IncomingReferralSearch #ddlReferralFrom').val();

        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["LoadFor"] = "Grid";
        objData["Type"] = 'Incoming';
        objData["commandType"] = "SEARCH_REFERRAL";
        objData["NoteId"] = 0;

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");
    },
    checkUncheckAllReferrals: function (type, obj) {
        var tableId = '';
        if (type == 'Incoming')
            tableId = ' #ctrlPanIncomingReferral #dgvInComingReferralDS';
        else
            tableId = ' #ctrlPanOutgoingReferral #dgvOutgoingReferralDS';

        $('#pnlDashboard' + tableId + ' input[id*="chk' + type + 'Referrals"]').prop("checked", $(obj).prop("checked"));
        DashBoard.EnableDisableReferralCheckboxes(type);
    },
    EnableDisableReferralCheckboxes: function (type) {
        var tableId = '';
        if (type == 'Incoming')
            tableId = ' #ctrlPanIncomingReferral #dgvInComingReferralDS';
        else
            tableId = ' #ctrlPanOutgoingReferral #dgvOutgoingReferralDS';

        if ($('#pnlDashboard' + tableId + ' input[id*="chk' + type + 'Referrals"]').length == $('#pnlDashboard' + tableId + ' input[id*="chk' + type + 'Referrals"]:checked').length)
            $('#pnlDashboard' + tableId + ' input[id="select' + type + 'Referrals"]').prop("checked", true);
        else if ($('#pnlDashboard' + tableId + ' input[id*="chk' + type + 'Referrals"]:checked').length < $('#pnlDashboard' + tableId + ' input[id*="chk' + type + 'Referrals"]').length && $('#pnlDashboard' + tableId + ' input[id*="chk' + type + 'Referrals"]:checked').length > 0)
            $('#pnlDashboard' + tableId + ' input[id="select' + type + 'Referrals"]').prop("checked", false);
    },
    ReferralGridLoadNew: function (Type, response) {
        var PnlResult = "";
        var dgvDivId = "";
        var pagingDivId = "";
        if (Type == "Incoming") {
            PnlResult = "pnlInComingReferalDS_Result";
            dgvDivId = "dgvInComingReferralDS";
            pagingDivId = "dgvInComingReferralDS_Paging";
        }
        else {
            PnlResult = "pnlOutgoingReferalDS_Result";
            dgvDivId = "dgvOutgoingReferralDS";
            pagingDivId = "dgvOutgoingReferralDS_Paging";
        }

        var actions = "";
        $("#pnlDashboard" + " #" + dgvDivId + " tr th").each(function () {
            if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                var arrActionType = [];
                if ($(this).attr("ActionType") != null) {
                    arrActionType = $(this).attr("ActionType").split(',');
                    actions = EMREditableGrid.GetActions(arrActionType, "#pnlDashboard" + " #" + PnlResult);
                }
            }
        });

        if ($.fn.dataTable.isDataTable("#pnlDashboard" + " #" + PnlResult + " #" + dgvDivId)) {
            $("#pnlDashboard" + " #" + PnlResult + " #" + dgvDivId).dataTable().fnClearTable();
            $("#pnlDashboard" + " #" + PnlResult + " #" + dgvDivId).dataTable().fnDestroy();
            $("#pnlDashboard" + " #" + PnlResult + " #" + dgvDivId + " tbody").find("tr").remove();
        }

        if ($("#pnlDashboard" + " #" + dgvDivId + " thead tr #select" + Type + "Referrals").length == 0)
            $("#pnlDashboard" + " #" + dgvDivId + " thead tr").prepend('<th class="noWordBreak size70 size-min80"><input type="checkbox" onchange="DashBoard.checkUncheckAllReferrals(\'' + Type + '\',this);" controlname="selectReferrals" id="select' + Type + 'Referrals" name="select' + Type + 'Referrals" class="input-block pull-left ml-xs" coltype="checkbox"/></th>');
        else
            $("#pnlDashboard" + " #" + dgvDivId + " thead tr #select" + Type + "Referrals").prop('checked', false);

        if (response.ReferralListCount > 0) {
            var ReferralLoadJSONData = JSON.parse(response.ReferralListLoad_JSON);

            if ($.fn.dataTable.isDataTable("#pnlDashboard" + " #" + PnlResult + " #" + dgvDivId)) {
                $("#pnlDashboard" + " #" + PnlResult + " #" + dgvDivId).dataTable().fnDestroy();
            }

            var arraTemp = [];

            $.each(ReferralLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", item.ReferralId);
                $row.attr("ProblemListNotesId", item.NoteId);
                var SelectionCheckBoxColumn = '<a class="btn btn-xs" role="button" onclick="DashBoard.EnableDisableReferralCheckboxes(\'' + Type + '\');" title="Select Referral"><input type="checkbox" id=chk' + Type + 'Referrals' + item.ReferralId + ' name="SelectCheckBoxReferral" class="input-block"/></a>&nbsp;<a title="View Referral" class="btn  btn-xs" href="#" onclick="DashBoard.printReferral(\'' + item.ReferralId + '\',\'' + item.PatientId + '\');"> <i class="fa fa-credit-card blue"></i></a>';
                var BellIcon = '&nbsp;<a title="Remind" class="btn  btn-xs" href="#" onclick="DashBoard.composeReferralMessage(\'' + item.AssigneeId + '\',\'' + item.AssigneeName + '\',\'' + item.PatientId + '\',\'' + item.Date + '\',\'' + item.Time + '\',\'' + $($("#pnlDashboard" + " #ddlStatusReferral > option")[item.Status]).text() + '\',\'' + $($("#pnlDashboard" + " #ddlVisitType > option")[item.Visits]).text() + '\',\'' + item.Reason + '\',\'' + item.ProviderName + '\',\'' + item.RefProviderName + '\',\'' + item.PatientInsuranceName + '\',\'' + item.Type + '\',\'' + item.PatientName + '\');"> <i class="fa fa-bell-o"></i></a>';
                var editMode = "onclick=DashBoard.ReferralEdit(\"" + item.ReferralId + "\",\"" + item.Type + "\",\"" + item.PatientId + "\");";
                var source = "MD Vision";
                var status = "Sent Successfully";

                var proceduresHtml = "";
                var procedures = item.Procedures.split(',');
                for (var p = 0; p < procedures.length; p++) {
                    proceduresHtml = proceduresHtml + procedures[p] + "<br>";
                }

                if (item.MedTextAppointmentId)
                    source = "MedText";
                
                if (item.IsDraft.toLowerCase() == "true") {
                    status = "Saved as Draft";
                    btnview = '';
                    if (source == "MedText" && response.MedTextURL)
                    {
                        var url = response.MedTextURL.replace("{MedTextReferralId}", item.MedTextAppointmentId);
                        editMode = "onclick=DashBoard.OpenMedText(\"" + url + "\",event);utility.SelectGridRow($(this))";
                    }
                }
                else {
                    editMode = '';
                }

                if (item.Type == 'Outgoing') {
                    $row.append('<td>' + SelectionCheckBoxColumn + '</td><td style="display:none;"' + editMode + '>' + source + '</td><td><a href=\"#\" onclick=\"utility.PatientDemographics(' + item.PatientId + ', mstrTabDashBoard, event);\"  title=\"View Patient Details\">' + item.AccountNumber + '</a></td><td><a href=\"#\" onclick=\"utility.PatientDemographics(' + item.PatientId + ', mstrTabDashBoard, event);\"  title=\"View Patient Details\">' + item.PatientName + '</a></td><td ' + editMode + '>' + utility.RemoveTimeFromDate(null, item.Date) + ' ' + item.Time + '</td><td ' + editMode + '>' + item.ProviderName + '</td><td ' + editMode + '>' + item.RefProviderName + '</td><td ' + editMode + '>' + item.FacilityToName + '</td><td ' + editMode + '>' + item.ToSpecialtyName + '</td><td ' + editMode + '>' + $($("#pnlDashboard" + " #ddlVisitType > option")[item.Visits]).text() + '</td><td ' + editMode + '>' + item.AssigneeName + '</td><td style="display:none;"' + editMode + '>' + status + '</td>');
                }
                else {
                    $row.append('<td>' + SelectionCheckBoxColumn + '</td><td style="display:none;"' + editMode + '>' + source + '</td><td><a href=\"#\" onclick=\"utility.PatientDemographics(' + item.PatientId + ', mstrTabDashBoard, event);\"  title=\"View Patient Details\">' + item.AccountNumber + '</a></td><td><a href=\"#\" onclick=\"utility.PatientDemographics(' + item.PatientId + ', mstrTabDashBoard, event);\"  title=\"View Patient Details\">' + item.PatientName + '</a></td><td ' + editMode + '>' + utility.RemoveTimeFromDate(null, item.Date) + ' ' + item.Time + '</td><td ' + editMode + '>' + item.RefProviderName + '</td><td ' + editMode + '>' + item.ProviderName + '</td><td ' + editMode + '>' + item.PatientInsuranceName + '</td><td ' + editMode + '>' + $($("#pnlDashboard" + " #ddlVisitType > option")[item.Visits]).text() + '</td><td ' + editMode + '>' + (item.AssigneeName == "" ? item.AssigneeName : item.AssigneeName + ' ' + BellIcon) + '</td><td ' + editMode + '>' + $($("#pnlDashboard #ctrlPanIncomingReferral  #ddlReferralStatus > option")[item.Status]).text() + '</td><td style="display:none;"' + editMode + '>' + item.StatusReasons + '</td>');
                }

                if (item.IsActive == "True") {
                    $($row).find('a.edit-row').removeClass('disableAll')
                }
                else {
                    $($row).find('a.edit-row').addClass('disableAll')

                }

                $("#pnlDashboard" + " #" + dgvDivId + " tbody").last().append($row);

                var CurrentRowchilds = $();
                if (CurrentRowchilds.length > 0) {

                }

                arraTemp.push({
                    row: $row, childs: CurrentRowchilds
                });
            });

            ////Inalize grid
            var PanelGrid = "#pnlDashboard" + " #" + PnlResult;
            var GridId = "#pnlDashboard" + " #" + dgvDivId;

            ////Start By Babur on 2/16/2016 - Below line comment out inorder to remove duplicate grid search
            if (DashBoard.myGrid != null) {

                if ($.fn.dataTable.isDataTable(DashBoard.myGrid)) {
                    DashBoard.myGrid.$table.dataTable().fnDestroy();
                } else {
                    DashBoard.myGrid = null;
                }

                if ($.fn.dataTable.isDataTable("#pnlDashboard" + " #" + PnlResult + " #" + dgvDivId)) {
                    $("#pnlDashboard" + " #" + PnlResult + " #" + dgvDivId).dataTable().fnDestroy();
                }
            }

            DashBoard.myGrid = EMRUtility.MakeEditableGrid(PanelGrid, GridId, DashBoard, 0, false, false, false, true, false, null);

            $.each(arraTemp, function (i, item) {
                if (DashBoard.myGrid != null) {
                    var row = DashBoard.myGrid.datatable.row(item.row);
                    if (item.childs.length > 0) {
                        row.child(item.childs);
                    }
                }
            });

            //Start//04//01//2015//Ahmad Raza//Sorting removed from first column of grid
            $('#pnlDashboard' + ' #' + dgvDivId).dataTable().fnSettings().aoColumns[0].bSortable = false;
            //End//04//01//2015//Ahmad Raza//Sorting removed from first column of grid
        }
        else {
            if ($("#pnlDashboard" + ' div#divShowHistory').hasClass("hidden") == false) {
                $("#pnlDashboard" + ' div#divShowHistory').addClass("hidden");
            }
            var NotFoundMessage = "";
            if (Type == "Incoming") {
                NotFoundMessage = "No Incoming Referral Found.";
            }
            else {
                NotFoundMessage = "No Outgoing Referral Found.";
            }

            $("#pnlDashboard" + ' #' + PnlResult + ' #' + dgvDivId).DataTable({
                "language": {
                    "emptyTable": NotFoundMessage
                }, "searching": false, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{"bSortable": false, "aTargets": [0]}], "order": [], "bDestroy": true
            });
        }
        
        $("#pnlDashboard" + ' #' + dgvDivId + ' thead tr th:first-child').removeClass('sorting_asc');
    },
    BindPatientReferral: function () {
        //incoming referral
        var Ctrl = $("#pnlDashboard #ctrlPanIncomingReferral #txtFullName");
        var func = function () { return utility.GetPatientArrayByName(Ctrl.val(), 1) };
        var hfCtrl = $("#pnlDashboard #ctrlPanIncomingReferral #hfPatientId");
        var onSelect = function (e) { utility.InsertRecentPatient(e.id); };
        var onChange = function (valid) {
            if (!valid)
                $("#pnlDashboard #ctrlPanIncomingReferral #txtFullName").val("");
        }
        utility.BindKendoAutoComplete(Ctrl, 3, "FullName", "contains", null, func, hfCtrl, onSelect, onChange);

        //outgoing referral
        var outgoingCtrl = $("#pnlDashboard #ctrlPanOutgoingReferral #txtFullName");
        var outgoingFunc = function () { return utility.GetPatientArrayByName(outgoingCtrl.val(), 1) };
        var outgoingHfCtrl = $("#pnlDashboard #ctrlPanOutgoingReferral #hfPatientId");
        var outgoingOnSelect = function (e) { utility.InsertRecentPatient(e.id); };
        var outgoingOnChange = function (valid) {
            if (!valid)
                $("#pnlDashboard #ctrlPanOutgoingReferral #txtFullName").val("");
        }
        utility.BindKendoAutoComplete(outgoingCtrl, 3, "FullName", "contains", null, outgoingFunc, outgoingHfCtrl, outgoingOnSelect, outgoingOnChange);
    },
    OpenPatientReferral:function(type){
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'mstrTabDashBoard';
        params["IsUnsolitedPatientSearch"] = false;
        params["IsFirstLoadFromDash"] = false;
        params["ActiveWidget"] = "Referrals";
        params["RefCtrl"] = 'txtFullName';
        params['Type'] = type;

        LoadActionPan('Patient_Search', params);
    },

    selectUnselectReferralTabs: function (referralTabId) {
        $('#pnlDashboard #pnlReferralsGrid #pnlReferrals ul#ulReferralsTabsItems li').each(function (index, item) {
            if ($(item).attr("id") == referralTabId) {
                $(item).addClass('active');
                $(item).trigger("click");
            }
            else {
                $(item).removeClass('active');
            }
        });
    },
    DashBoardOutgoingReferralGridLoad: function (response, pageNo, rpp) {
        //DashBoard.fillRefferal();
        //pageNo = (pageNo == null || pageNo == "") ? 1 : pageNo;
        //rpp = (rpp == null || rpp == "") ? 15 : rpp;

        //var gridctl = $("#pnlDashboard #pnlOutgoingReferal_Result").data("kendoGrid");
        //var userLabOrderment = { data: [], total: 0 };
        //var dataSource;


        var PnlResult = "";
        var dgvDivId = "";
        var pagingDivId = "";
        PnlResult = "pnlOutgoingReferalDS_Result";
        dgvDivId = "dgvOutgoingReferralDS";
        pagingDivId = "dgvOutgoingReferralDS_Paging";

        DashBoard.LoadOutgoingReferral('{}', 0, pageNo, rpp).done(function (response) {
            response = JSON.parse(response)
            if (response.status != false) {
                DashBoard.selectUnselectReferralTabs("listOutgoingReferrals");
                DashBoard.ReferralGridLoadNew('Outgoing', response);

                var TableControl = 'pnlDashboard' + " #pnlOutgoingReferalDS_Result" + " #" + pagingDivId;
                var PagingPanelControlID = 'pnlDashboard' + " #pnlOutgoingReferalDS_Result" + " #" + pagingDivId;
                var ClassControlName = "DashBoard";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.ReferralListCount, pageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                    DashBoard.DashBoardOutgoingReferralGridLoad(PrimaryID, PageNumber, ResultPerPage);
                }), 10);
            }
            else
                utility.DisplayMessages(response.Message, 3);
        });
    },

    LoadOutgoingReferral: function (ReferralData, ReferralId, PageNumber, RowsPerPage) {

        SearchFormDivId = "OutgoingReferralSearch";

        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var self = $('#pnlDashboard' + ' #' + SearchFormDivId);
        var objData = self != null ? self.getMyJSONByName() : "{}";
        objData = JSON.parse(objData);
        objData["PatientId"] = $('#pnlDashboard #ctrlPanOutgoingReferral #txtFullName').val() == "" ? "" : $('#pnlDashboard #ctrlPanOutgoingReferral #hfPatientId').val();
        objData["IsActive"] = "1";
        objData["ReferralId"] = ReferralId;

        objData["CPTCodeDescription"] = objData["CPTCode"];

        objData["ProviderId"] = $('#pnlDashboard' + ' #OutgoingReferralSearch #ddlReferralFrom').val();
        objData["RefProviderId"] = $('#pnlDashboard' + ' #OutgoingReferralSearch #ddlReferralTo').val();

        objData["FacilityFrom"] = $('#pnlDashboard' + ' #OutgoingReferralSearch #ddlFacilityFrom').val();
        objData["FacilityTo"] = $('#pnlDashboard' + ' #OutgoingReferralSearch #ddlFacilityTo').val();

        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["LoadFor"] = "Grid";
        objData["Type"] = 'Outgoing';
        objData["commandType"] = "SEARCH_REFERRAL";
        objData["NoteId"] = 0;

        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");
    },

    fillRefferal: function () {
        DashBoard.fillRefferalFromDBCall().done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                //var referringProviders = JSON.parse(response.ReferralFrom);
                //var providers = JSON.parse(response.ReferralTo);

                var $ddlReferralFromIncoming = $('#pnlDashboard' + ' #IncomingReferralSearch #ddlReferralFrom');
                var $ddlReferralToIncoming = $('#pnlDashboard' + ' #IncomingReferralSearch #ddlReferralTo');

                var $ddlReferralFromOutgoing = $('#pnlDashboard' + ' #OutgoingReferralSearch #ddlReferralFrom');
                var $ddlReferralToOutgoing = $('#pnlDashboard' + ' #OutgoingReferralSearch #ddlReferralTo');

                $ddlReferralFromIncoming.empty();
                $ddlReferralToOutgoing.empty();

                $ddlReferralToIncoming.empty();
                $ddlReferralFromOutgoing.empty();

                var incomingReferralFrom = JSON.parse(response.IncomingReferralFrom);
                var incomingReferralTo = JSON.parse(response.IncomingReferralTo);
                var outgoingReferralFrom = JSON.parse(response.OutgoingReferralFrom);
                var outgoingReferralTo = JSON.parse(response.OutgoingReferralTo);

                $.each(incomingReferralFrom, function (i, item) {
                    $ddlReferralFromIncoming.append(
                        $('<option/>', {
                            value: item.Value,
                            html: item.Name,
                        })
                );
                });

                $.each(incomingReferralTo, function (i, item) {
                    $ddlReferralToIncoming.append(
                        $('<option/>', {
                            value: item.Value,
                            html: item.Name,
                        })
                    );
                });

                $.each(outgoingReferralFrom, function (i, item) {
                    $ddlReferralFromOutgoing.append(
                        $('<option/>', {
                            value: item.Value,
                            html: item.Name,
                        })
                    );
                });

                $.each(outgoingReferralTo, function (i, item) {
                    $ddlReferralToOutgoing.append(
                        $('<option/>', {
                            value: item.Value,
                            html: item.Name,
                        })
                    );
                });

                var $ddlFacilityTo = $('#pnlDashboard' + ' #OutgoingReferralSearch #ddlFacilityTo');
                var $ddlFacilityFrom = $('#pnlDashboard' + ' #OutgoingReferralSearch #ddlFacilityFrom');

                $ddlFacilityTo.empty();
                $ddlFacilityFrom.empty();

                var facilityTo = JSON.parse(response.FacilityList);
                var facilityFrom = JSON.parse(response.FacilityList);

                $.each(facilityTo, function (i, item) {
                    $ddlFacilityTo.append(
                        $('<option/>', {
                            value: item.Value,
                            html: item.Name,
                        })
                    );
                });

                $.each(facilityFrom, function (i, item) {
                    $ddlFacilityFrom.append(
                        $('<option/>', {
                            value: item.Value,
                            html: item.Name,
                        })
                    );
                });
            }

        });
    },

    fillRefferalFromDBCall: function () {
        var objData = new Object();
        objData["commandType"] = "getreferringfromprovider";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");

    },

    OpenMedText : function (Url){

        var params = [];
        params["FromAdmin"] = "0";
        params["MedTextUrl"] = Url;
        params["ParentCtrl"] = "mstrTabDashBoard";
        LoadActionPan('Patient_MedText_Referrals', params);

    },

    ReferralEdit: function (ReferralId, Type, PatientId) {

        var params = [];
        var PanelID = "";
        params["ParentCtrl"] = 'mstrTabDashBoard';
        PanelID = 'pnlDashboard';
        params["FromAdmin"] = 0;
        params["ReferralId"] = ReferralId;
        params["mode"] = "Edit";
        params["PatientId"] = PatientId; //$('#PatientProfile #hfPatientId').val();

        if (Type == "Incoming") {
            LoadActionPan("Patient_Referrals_Incoming_Detail", params, PanelID);
        }
        else {
            LoadActionPan("Patient_Referrals_Outgoing_Detail", params, PanelID);
        }
    },

    printReferral: function (ReferralId, patientId) {
        //var params = [];
        //params["FromAdmin"] = "0";
        //params["UserId"] = globalAppdata['AppUserId'];
        //params["PatientId"] = patientId;
        //params["ParentCtrl"] = "mstrTabDashBoard";
        //params["ReferralId"] = ReferralId;
        //LoadActionPan('Patient_ReferralsView', params);

        Patient_ReferralsView.ReferralPreview(patientId, globalAppdata['AppUserId'], ReferralId);
    },

    DeleteReferral: function (type, event) {
        var strMessage = "";
        var ReferralIds = "";
        var tableId = '';
        var chkId = '';

        if (event != null) {
            event.stopPropagation();
        }
        
        if (type == 'Incoming') {
            tableId = ' #ctrlPanIncomingReferral #dgvInComingReferralDS';
            chkId = /chkIncomingReferrals/gi;
        }
        else {
            tableId = ' #ctrlPanOutgoingReferral #dgvOutgoingReferralDS';
            chkId = /chkOutgoingReferrals/gi;
        }


        if ($("#pnlDashboard" + tableId + " tbody tr input:checked").length == 0) {
            utility.DisplayMessages('Please select any referral to delete', 4);
            return;
        }
        else {
            ReferralIds = $("#pnlDashboard" + tableId + " tbody tr input:checked").map(function () {
                return this.id.replace(chkId, '');
            }).get().join(',');
        }

        AppPrivileges.GetFormPrivilegesByModule("Referrals", "DELETE", "DashBoard", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE_BY_MODULE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    DashBoard.DeleteReferral_DBCall(ReferralIds).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            if (type == 'Incoming') {
                                var grid = $('#dgvInComingReferral').DataTable();
                                grid.row('.active').remove().draw(false);
                                DashBoard.DashBoardIncomingReferralGridLoad(null, null, null);
                            }
                            else {
                                var grid = $('#dgvOutgoingReferral').DataTable();
                                grid.row('.active').remove().draw(false);
                                DashBoard.DashBoardOutgoingReferralGridLoad(null, null, null);
                            }
                            utility.DisplayMessages(response.Message, 1);
                        }
                        else
                            utility.DisplayMessages(response.Message, 3);
                    });
                }, function () {}, '1');
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    DeleteReferral_DBCall: function (ReferralIds) {

        var objData = new Object();
        objData["ReferralId"] = ReferralIds;
        objData["commandType"] = "DELETE_REFERRAL";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");
    },

    composeReferralMessage: function (AssigneeId, AssigneeName, PatientId, Date, Time, status, visitType, reason, referralTo, referralBy, insurancePlan, type, patientName) {

        var message = "Hi,&#13;&#10;&#13;&#10;A referral needs your attention. The details are given below:&#13;&#10;&#13;&#10;" +
            "Date: " + utility.RemoveTimeFromDate(null, Date) + ' ' + Time + "&#13;&#10;" +
            "Category :" + type + "&#13;&#10;" +
            "Status: " + status + "&#13;&#10;" +
            "Visit Type: " + visitType + "&#13;&#10;" +
            "Reason: " + reason + "&#13;&#10;" +
            "Referral To :" + referralTo + "&#13;&#10;" +
            "Referral By :" + referralBy + "&#13;&#10;" +
            "Insurance Plan :" + (insurancePlan == 'undefined' ? '' : insurancePlan) + "&#13;&#10;" +
            "Assigned To :" + AssigneeName + "&#13;&#10;&#13;&#10;" +
            "Thank you," + '' + "&#13;&#10;&#13;&#10;" +
           DashBoard.changeNameFormat(globalAppdata.AppUserNameFullName);

        var subject = "Referral Reminder: " + DashBoard.changeNameFormat(patientName);;

        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'mstrTabDashBoard';
        params["Caller"] = "Referrals";
        params["AssignedToId"] = AssigneeId;
        params["AssignedName"] = AssigneeName;
        params["PatientId"] = PatientId;
        params["Message"] = message;
        params["MsgSubject"] = subject;
        params["MessageType"] = 'Practice';

        params["mode"] = 'Add';
        LoadActionPan('Patient_MessageCreate', params);



    },


    //End Added by Humaira Yousaf for referral widget

    /* Portal Appointment Request Functions Start */

    IsPendingOrConfirm: function (objThis) {

        var appstatus = $(objThis).attr('appstatus');

        if (appstatus == '1') {
            $(objThis).attr('appstatus', '0');
        }
        else if (appstatus == '0') {
            $(objThis).attr('appstatus', '1');
        }

        //DashBoard.DashBoardAppRequestSearch();

    },

    PortalNextDate: function () {

        var criteria = $('#pnlDashboard #appRequestDate').text();

        var d = new Date(DashBoard.FormatDate(criteria));
        var newdate = DashBoard.AddSubDays(d, 1);
        var curdte = $.datepicker.formatDate('mm/dd/yy', newdate);

        $('#pnlDashboard #appRequestDate').text(curdte);
        //DashBoard.DashBoardAppRequestSearch();

    },

    PortalBackDate: function () {

        var criteria = $('#pnlDashboard #appRequestDate').text();

        var d = new Date(DashBoard.FormatDate(criteria));
        var newdate = DashBoard.AddSubDays(d, -1);
        var curdte = $.datepicker.formatDate('mm/dd/yy', newdate);

        $('#pnlDashboard #appRequestDate').text(curdte);
        //DashBoard.DashBoardAppRequestSearch();

    },

    //DashBoardAppRequestSearch: function (GridData) {
    //    DashBoard.EnableDisableControl();
    //    if ($("#pnlDashboard #pnlAppRequestGrid").css("display") == "none")
    //        $("#pnlDashboard #pnlAppRequestGrid").show();

    //    if (GridData) {

    //        DashBoard.DashBoardAppRequestGridLoad(GridData);
    //    }
    //    else {

    //        DashBoard.DashBoardAppRequest_DBCall().done(function (response) {
    //            if (response.status != false) {

    //                DashBoard.DashBoardAppRequestGridLoad(response);
    //            }
    //            else {
    //                utility.DisplayMessages(response.Message, 3);

    //            }
    //        });

    //    }
    //},

    DashBoardAppRequestGridLoad: function (response) {

        $("#dgvAppRequestGrid").dataTable().fnDestroy();
        $("#dgvAppRequestGrid tbody").find("tr").remove();
        if (response.PortalRequestCount > 0) {
            var PortalRequestJSONData = JSON.parse(response.PortalRequest_JSON);
            $.each(PortalRequestJSONData, function (i, item) {
                var $row = $('<tr/>');

                if (item.AppointmentId != "") {
                    $row.attr("onclick", "DashBoard.PortalRequestAppEdit('" + item.AppointmentId + "',event);");
                }

                $row.attr("id", item.PortalAppRequestId);
                $row.attr("PortalAppRequestId", item.PortalAppRequestId);

                var actionBar = "";
                var minutes = item.Duration == "" ? "" : item.Duration + "(min)";
                //var ClassDisabled = item.RequestStatus.toLowerCase() == "pending" ? "disableAll" : "";

                var _status = item.RequestStatus;
                var chkBox = "";
                if (item.RequestStatus.toLowerCase() == "pending") {
                    actionBar = '<td><a disabled class="btn  btn-xs"  href="#" onclick="" title="Schedule appointment"><i class="fa fa-calendar-check-o black"></i></a>&nbsp;<a class="btn btn-xs"  href="#" onclick="DashBoard.AcceptAppRequest(' + item.PortalAppRequestId + ',' + item.PatientId + ',\'' + item.PracticeId + '\',\'' + item.FacilityId + '\',\'' + item.ProviderName + '\',event, \'' + item.PatientName + '\');"  title="Confirm request"><i class="fa fa-check black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="DashBoard.CancelAppRequest(' + item.PortalAppRequestId + ',' + item.PatientId + ',\'' + item.PracticeId + '\',\'' + item.FacilityId + '\',\'' + item.ProviderName + '\',event, \'' + item.PatientName + '\');" title="Cancel request"><i class="fa fa-times black"></i></a></td>';
                    chkBox = '<input type="checkbox" id="' + item.PortalAppRequestId + '"></input>';
                } else if (item.RequestStatus.toLowerCase() == "cancel" && item.AppointmentId == "") {
                    actionBar = '<td><a disabled class="btn  btn-xs"  href="#" onclick="" title="Schedule appointment"><i class="fa fa-calendar-check-o black"></i></a>&nbsp;<a class="btn btn-xs"  href="#" onclick="DashBoard.AcceptAppRequest(' + item.PortalAppRequestId + ',' + item.PatientId + ',\'' + item.PracticeId + '\',\'' + item.FacilityId + '\',\'' + item.ProviderName + '\',event, \'' + item.PatientName + '\');"  title="Confirm request"><i class="fa fa-check black"></i></a>&nbsp;<a disabled class="btn  btn-xs" href="#" onclick="" title="Cancel request"><i class="fa fa-times black"></i></a></td>';
                    chkBox = '<input type="checkbox" id="' + item.PortalAppRequestId + '"></input>';
                } else if (item.RequestStatus.toLowerCase() == "cancel" && item.AppointmentId != "") {
                    actionBar = '<td><a disabled class="btn  btn-xs"  href="#" onclick="" title="Schedule appointment"><i class="fa fa-calendar-check-o black"></i></a>&nbsp;<a disabled class="btn btn-xs"  href="#" onclick=""  title="Confirm request"><i class="fa fa-check black"></i></a>&nbsp;<a disabled class="btn  btn-xs" href="#" onclick="" title="Cancel request"><i class="fa fa-times black"></i></a></td>';
                    _status = item.AppointmentStatus;
                    chkBox = "";
                } else if (item.RequestStatus.toLowerCase() == "confirm") {
                    actionBar = '<td><a class="btn  btn-xs"  href="#" onclick="DashBoard.PortalRequestAppAdd(\'' + item.TimeSlotId + '\', \'' + item.TimeSlotDtlId + '\', \'' + item.ProviderId + '\', \'' + item.ProviderName + '\', \'' + item.FacilityId + '\', \'' + item.FacilityName + '\', \'' + item.SchReasonId + '\', \'' + item.FromTime + '\', \'' + item.ToTime + '\', \'' + item.IsSpecialist + '\', \'' + item.Duration + '\',\'' + item.SchReasonName + '\',\'' + item.PatientId + '\',\'' + item.AccountNumber + '\',\'' + item.PortalAppRequestId + '\', \'' + item.PracticeId + '\', event, \'' + item.PatientName + '\', \'' + item.SchReasonId + '\', \'' + item.SchReasonName + '\', \'' + item.AppDate + '\');" title="Schedule appointment"><i class="fa fa-calendar-check-o black"></i></a>&nbsp;<a disabled class="btn btn-xs"  href="#" onclick=""  title="Confirm request"><i class="fa fa-check black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="DashBoard.CancelAppRequest(' + item.PortalAppRequestId + ',' + item.PatientId + ',\'' + item.PracticeId + '\',\'' + item.FacilityId + '\',\'' + item.ProviderName + '\',event, \'' + item.PatientName + '\');" title="Cancel request"><i class="fa fa-times black"></i></a></td>';
                    chkBox = '<input type="checkbox" id="' + item.PortalAppRequestId + '"></input>';
                } else if (item.RequestStatus.toLowerCase() == "booked") {
                    actionBar = '<td><a disabled class="btn  btn-xs"  href="#" onclick="" title="Schedule appointment"><i class="fa fa-calendar-check-o black"></i></a>&nbsp;<a disabled class="btn btn-xs"  href="#" onclick=""  title="Confirm request"><i class="fa fa-check black"></i></a>&nbsp;<a disabled class="btn  btn-xs" href="#" onclick="" title="Cancel request"><i class="fa fa-times black"></i></a></td>';
                    _status = item.AppointmentStatus;
                    chkBox = "";
                }
                $row.append(actionBar + '<td>' + item.AccountNumber + '</td><td>' + item.PatientName + '</td><td>' + item.ProviderName + '</td><td>' + item.FacilityName + '</td><td>' + item.PatientType + '</td><td>' + item.PatientVisitType + '</td><td>' + item.FromTime + '</td><td>' + minutes + '</td><td>' + _status + '</td>');
                $("#dgvAppRequestGrid tbody").last().append($row);
            });
        }
        else {
            $('#dgvAppRequestGrid').DataTable({
                "language": {
                    "emptyTable": "No Appointment Request Found."
                }, "searching": false, "autoWidth": false, "bLengthChange": false, "order": [], "bPaginate": false, "bInfo": false, "aoColumnDefs": [{
                    "bSortable": false, "aTargets": [0]
                }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvAppRequestGrid'))
            ;
        else
            $("#dgvAppRequestGrid").DataTable({
                "searching": false, "bSortable": false, "bSort": false, "bLengthChange": false, "autoWidth": false, "order": [], "aoColumnDefs": [{
                    "bSortable": false, "aTargets": [0]
                }]
            }); // to remove records per page dropdown
    },

    DashBoardAppRequest_DBCall: function () {

        var calDate = $('#pnlDashboard #appRequestDate').text();
        var newDatee = DashBoard.FormatDate(calDate);
        var AppDate = newDatee;
        var IsPendingOrConfirm = null;

        IsPendingOrConfirm = $('#pnlDashboard #pnlAppRequestGrid #divSwitch #switchAppRequest').attr('AppStatus');

        var objData = new Object();
        objData["PortalAppDate"] = AppDate;
        objData["PortalAppStatus"] = IsPendingOrConfirm;
        objData["CommandType"] = "LoadPortalAppRequest";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "LoadDashBoard");


    },

    CancelAppRequest: function (PortalAppRequestId, PatientId, pracId, facId, ProvName, event, patName) {
        if (event != null) {
            event.stopPropagation();
        }

        utility.myConfirm('Are you sure want to cancel appointment request?', function () {
            DashBoard.CancelAppRequest_DBCall(PortalAppRequestId, PatientId, pracId, facId, ProvName, patName).done(function (response) {
                if (response.status != false) {
                    utility.DisplayMessages("Appointment cancelled successfully!", 1);
                    //DashBoard.DashBoardAppRequestSearch(null);

                } else {
                    utility.DisplayMessages(response.Message, 4);
                }

            });
        }, function () {
        },
                    'Cancel appointment request'
                            );
    },

    CancelAppRequest_DBCall: function (PortalAppRequestId, PatientId, pracId, facId, ProvName, patName) {

        var arr = patName.split(",");
        var name = arr[1] + " " + arr[0];

        var objData = new Object();
        objData["PortalAppRequestId"] = PortalAppRequestId;
        objData["PatientId"] = PatientId;
        objData["PracticeId"] = pracId;
        objData["FacilityId"] = facId;
        objData["ProviderName"] = ProvName;
        objData["PatientName"] = name;
        objData["PortalAppDate"] = $("#appRequestDate").text();
        objData["CommandType"] = "CancelAppRequest";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "LoadDashBoard");

    },

    AcceptAppRequest: function (PortalAppRequestId, PatientId, pracId, facId, ProvName, event, patName) {
        if (event != null) {
            event.stopPropagation();
        }

        utility.myConfirm('Are you sure want to confirm appointment request?', function () {
            DashBoard.AcceptAppRequest_DBCall(PortalAppRequestId, PatientId, pracId, facId, ProvName, patName).done(function (response) {
                if (response.status != false) {
                    utility.DisplayMessages("Appointment confirmed successfully!", 1);
                    //DashBoard.DashBoardAppRequestSearch(null);
                } else {
                    utility.DisplayMessages(response.Message, 4);
                }

            });
        }, function () {
        },
            'Confirm appointment request'
                    );
    },

    AcceptAppRequest_DBCall: function (PortalAppRequestId, PatientId, pracId, facId, ProvName, patName) {
        var arr = patName.split(",");
        var name = arr[1] + " " + arr[0];
        var objData = new Object();
        objData["PortalAppRequestId"] = PortalAppRequestId;
        objData["PatientId"] = PatientId;
        objData["PracticeId"] = pracId;
        objData["FacilityId"] = facId;
        objData["ProviderName"] = ProvName;
        objData["PatientName"] = name;
        objData["PortalAppDate"] = $("#appRequestDate").text();
        objData["CommandType"] = "ConfirmAppRequest";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "LoadDashBoard");

    },

    PortalRequestAppAdd: function (Slotid, TimeSlotDtlId, ProviderId, ProviderName, FacilityId, FacilityName, ScheduleReasonId, Time, ToTime, IsSpecialist, SlotMinutes, ScheduleReason, PatientId, AccountNumber, PortalAppRequestId, PracticeId, event, patName, reasonId, reasonName, AppDate) {
        var arr = patName.split(",");
        var name = arr[1] + " " + arr[0];

        var params = [];
        params["SlotId"] = Slotid;
        params["SlotdetailId"] = TimeSlotDtlId;
        params["PracticeId"] = PracticeId;
        params["ProviderId"] = ProviderId;
        params["ProviderName"] = ProviderName;
        params["FacilityId"] = FacilityId;
        params["FacilityName"] = FacilityName;
        params["ScheduleReasonId"] = ScheduleReasonId;
        params["mode"] = "Add";
        params["DayDate"] = $('#pnlScheduleCalendar #daydate').text().trim();
        params["Time"] = Time;
        params["ToTime"] = ToTime;
        params["SlotMinutes"] = SlotMinutes;
        params["IsSpecialist"] = IsSpecialist;
        params["ScheduleReason"] = ScheduleReason;
        params["PatientId"] = PatientId;
        params["AccountNumber"] = AccountNumber;
        params["PortalAppRequestId"] = PortalAppRequestId;
        params["ScheduleDate"] = AppDate;
        params["PortalSchDate"] = $("#appRequestDate").text();
        params["PatientName"] = name;
        params["CommentBox"] = "Appointment request received from Patient Portal";
        params["ParentCtrl"] = "mstrTabDashBoard";
        LoadActionPan('appointmentDetail', params);
    },

    PortalRequestAppEdit: function (appID, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Appointment", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["checkin"] = 1;
                params["AppointmentId"] = appID;
                params["mode"] = "Edit";
                params["ParentCtrl"] = "mstrTabDashBoard";
                LoadActionPan('appointmentDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    SelectAllPortalRequests: function (obj, event) {

        if (event != null) {
            event.stopPropagation();
        }
        var objCheck = $(obj);

        if ((objCheck).is(':checked')) {

            $('#dgvAppRequestGrid input[type="checkbox"]').prop('checked', 'checked');
        }

        else {
            $('#dgvAppRequestGrid input[type=checkbox]').each(function () {

                $(this).attr('checked', false);
            });
        }

    },

    AcceptAppRequest_CheckBox: function () {
        var selected = [];
        var reqIds;
        $('#dgvAppRequestGrid > tbody > tr').each(function () {
            if ($(this).find('input:checked').is(':checked')) {
                selected.push($(this).attr('id'));
                //selected.push($(this).attr('id').replace('checkbox', ''));
            }
        });

        for (var w = 0; w <= selected.length; w++) {
            reqIds += selected[w] + ',';
        }
        reqIds = reqIds.replace('undefined,', '');
        reqIds = reqIds.replace('undefined', '');
        if (reqIds != "") {

            DashBoard.AcceptAppRequest_CheckBox_DBCall(reqIds).done(function (response) {
                if (response.status != false) {
                    $('#pnlDashboard #chkSelectAllMessages').prop('checked', false);
                    //DashBoard.DashBoardAppRequestSearch(null);
                    utility.DisplayMessages("Request(s) accepted successfully!", 1);
                } else {
                    utility.DisplayMessages(response.Message, 4);
                }

            });

        } else {
            utility.DisplayMessages("Please Select request.", 3);
        }
    },

    AcceptAppRequest_CheckBox_DBCall: function (reqIds) {
        var objData = new Object();
        objData["PortalAppRequestId"] = reqIds;
        objData["PortalAppStatus"] = "Confirm";
        //objData["PatientId"] = PatientId;
        //objData["PracticeId"] = pracId;
        //objData["FacilityId"] = facId;
        //objData["ProviderName"] = ProvName;
        //objData["PortalAppDate"] = $("#appRequestDate").text();
        objData["CommandType"] = "CONFIRM_CANCEL_MULTIPLE_APPREQUEST";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "LoadDashBoard");
    },

    CancelAppRequest_CheckBox: function () {
        var selected = [];
        var reqIds;
        $('#dgvAppRequestGrid > tbody > tr').each(function () {
            if ($(this).find('input:checked').is(':checked')) {
                selected.push($(this).attr('id'));
                //selected.push($(this).attr('id').replace('checkbox', ''));
            }
        });

        for (var w = 0; w <= selected.length; w++) {
            reqIds += selected[w] + ',';
        }
        reqIds = reqIds.replace('undefined,', '');
        reqIds = reqIds.replace('undefined', '');
        if (reqIds != "") {

            DashBoard.CancelAppRequest_CheckBox_DBCall(reqIds).done(function (response) {
                if (response.status != false) {
                    $('#pnlDashboard #chkSelectAllMessages').prop('checked', false);
                    //DashBoard.DashBoardAppRequestSearch(null);
                    utility.DisplayMessages("Request(s) cancelled successfully!", 1);
                } else {
                    utility.DisplayMessages(response.Message, 4);
                }

            });

        } else {
            utility.DisplayMessages("Please Select request.", 3);
        }
    },

    CancelAppRequest_CheckBox_DBCall: function (reqIds) {
        var objData = new Object();
        objData["PortalAppRequestId"] = reqIds;
        objData["PortalAppStatus"] = "Cancel";
        //objData["PatientId"] = PatientId;
        //objData["PracticeId"] = pracId;
        //objData["FacilityId"] = facId;
        //objData["ProviderName"] = ProvName;
        //objData["PortalAppDate"] = $("#appRequestDate").text();
        objData["CommandType"] = "CONFIRM_CANCEL_MULTIPLE_APPREQUEST";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "LoadDashBoard");
    },
    /* Portal Appointment Request Functions End */

    changeNameFormat: function (fullname) {

        var index = fullname.indexOf(',');
        var firstName = fullname.substring(index + 1, fullname.length);
        var lastName = fullname.substring(0, index);

        return firstName.trim() + " " + lastName.trim();
    },

    /******* CCM SECTION **********/
    DashBoardCCMEnrollmentInfoSearch: function (PageNo, rpp, GridData, Status) {

        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({
            container: 'body', html: true
        });

        DashBoard.EnableDisableControl();

        if (Status == null || Status == "") {
            Status = $('#pnlDashboard #pnlCCMEnrollmentInfoGrid ul.tabs-custom li.active').text().toLowerCase();
            if (Status == null || Status == "") {
                $('#pnlDashboard #pnlCCMEnrollmentInfoGrid ul.tabs-custom li').removeClass('active');
                $('#pnlDashboard #pnlCCMEnrollmentInfoGrid ul.tabs-custom li.first').addClass('active');
                DashBoard.DashBoardCCMEnrollmentInfoSearch(null, null, null, 'pending');
                return false;
            }
        }

        $('#pnlDashboard #pnlCCMEnrollmentInfoGrid ul.tabs-custom li').each(function (index, item) {
            if ($(item).text().toLowerCase() == Status) {
                $(item).addClass('active');
                $('#pnlDashboard #pnlCCMEnrollmentInfoGrid .tabs-custom-body .tab-pane').eq(index).addClass('active');
            }
            else {
                $(item).removeClass('active');
                $('#pnlDashboard #pnlCCMEnrollmentInfoGrid .tabs-custom-body .tab-pane').eq(index).removeClass('active');
            }
        });

        if ($("#pnlDashboard #pnlCCMEnrollmentInfoGrid").css("display") == "none")
            $("#pnlDashboard #pnlCCMEnrollmentInfoGrid").show();

        if (GridData) {
            DashBoard.DashBoardPendingCCMEnrollmentInfoGridLoad(GridData, PageNo, rpp);
        }
        else {
            DashBoard.LoadCCMEnrollmentInfo(PageNo, rpp, Status).done(function (response) {
                if (response.status != false) {
                    if (Status == "pending") {
                        DashBoard.DashBoardPendingCCMEnrollmentInfoGridLoad(response, PageNo, rpp);
                    }
                    else if (Status == "accepted") {
                        DashBoard.DashBoardAcceptedCCMEnrollmentInfoGridLoad(response, PageNo, rpp);
                    }
                    else if (Status == "declined") {
                        DashBoard.DashBoardDeclinedCCMEnrollmentInfoGridLoad(response, PageNo, rpp);
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);

                }
            });
        }


    },

    LoadCCMEnrollmentInfo: function (PageNumber, RowsPerPage, Status) {
        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 15 : RowsPerPage;
        var objData = new Object();

        if ($("#pnlDashboard #pnlCCMEnrollmentInfoGrid #txtPatientName").val() == '') {
            $("#pnlDashboard #pnlCCMEnrollmentInfoGrid #hfPatientId").val('');
        }

        switch (Status) {
            case "pending":
                Status = 1;
                break;
            case "accepted":
                Status = 2;
                break;
            case "declined":
                Status = 3;
                break;
            case "terminated":
                Status = 4;
                break;
        }

        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        objData["PatientId"] = $("#pnlDashboard #pnlCCMEnrollmentInfoGrid #hfPatientId").val();
        objData["ProviderId"] = $("#pnlDashboard #pnlCCMEnrollmentInfoGrid #hfProviderId").val();
        objData["InsurancePlanId"] = $("#pnlDashboard #pnlCCMEnrollmentInfoGrid #hfInsurancePlanId").val();
        objData["CommandType"] = "Load_CCM_Enrollment_Info";
        objData["Status"] = Status;

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "LoadDashBoard");
    },



    DashBoardPendingCCMEnrollmentInfoGridLoad: function (response, PageNo, rpp) {

        if (response.CCMEnrollmentInfoCount > 0) {

            $("#pnlDashboard div[id='CCM'] .badge").css("display", "inline");
            $("#spnCCM").text(response.CCMEnrollmentInfoCount);

            $("#pnlDashboard #divPendingCCMEnrollmentInfoPaging").css("display", "inline");
            //Showing 1 to 15 of 15 Record(s)
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            // fixMe
            if (CurrentPage == 1) {
                if (RecordsPerPage == 1000) {
                    RecordsPerPage = 15;
                }
            }
            var params = [];
            params["CurrentPage"] = CurrentPage;
            params["RecordsPerPage"] = RecordsPerPage;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("divPendingCCMEnrollmentInfoPaging", response.iTotalDisplayRecords, 5, "DashBoard", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#pnlDashboard #divPendingCCMEnrollmentInfoPaging #divShowingEntries").text(showingText);

            $("#pnlDashboard #divPendingCCMEnrollmentInfoPaging li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });

        }
        else {

            $("#pnlDashboard div[id='CCM'] .badge").css("display", "none");
            $("#pnlDashboard #divPendingCCMEnrollmentInfoPaging").css("display", "none");
        }

        $("#pnlDashboard #pnlCCMEnrollmentInfoGrid #dgvPendingCCMEnrollmentInfoGrid").dataTable().fnDestroy();
        $("#pnlDashboard #pnlCCMEnrollmentInfoGrid #dgvPendingCCMEnrollmentInfoGrid tbody").find("tr").remove();
        if (response.CCMEnrollmentInfoCount > 0) {
            var CCMEnrollmentInfoJSONData = response.CCMEnrollmentInfo_JSON;
            $.each(CCMEnrollmentInfoJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", '' + item.PatientId + '');
                $row.attr("onclick", "utility.SelectGridRow($('#gvDashBoard_PendingCCMEnrollmentInfoGrid_row" + i + "'))");

                if (item.IsActive == "True") {
                    isactive = 0;
                    tglclass = "fa fa-toggle-on green";
                } else {
                    isactive = 1;
                    tglclass = "fa fa-toggle-on red";
                }


                var AcceptMethod = '<a class="btn btn-success btn-xs mr-xs" onclick="DashBoard.EnrollForCCM(\'' + item.PatientId + '\',\'' + item.PatientName + '\',\'' + item.ProviderName + '\');"> Accept</a>'
                var DeclineMethod = '<a class="btn btn-danger btn-xs mr-xs" onclick="DashBoard.DeclineForCCM(\'' + item.PatientId + '\');">Decline</a>'
                var parameters = item.PatientId + ",'mstrTabDashBoard', event";
                $row.append('<td><a href="#" onclick="utility.PatientDemographics(' + parameters + ');">' + item.AccountNumber + '</a></td><td><a href="#" onclick="utility.PatientDemographics(' + parameters + ');">' + item.PatientName + '</a></td><td>' + item.ProviderName + '</td><td data-toggle="tooltip" title="" data-original-title="' + item.Problems + '">' + item.NoOfProblems + '</td><td>' + item.InsuranceName + '</td><td>' + item.CreatedOn + '</td><td>' + AcceptMethod + '&nbsp;' + DeclineMethod + '</td>');
                $("#hfpatientid").val(item.PatientId);
                $("#pnlDashboard #dgvPendingCCMEnrollmentInfoGrid tbody").last().append($row);
            });
        }
        else {
            $("#pnlDashboard #divPendingCCMEnrollmentInfoPaging").css("display", "none");
            $('#pnlDashboard #pnlCCMEnrollmentInfoGrid #dgvPendingCCMEnrollmentInfoGrid').DataTable({
                "language": {
                    "emptyTable": "No Data Found"
                }, "searching": false, "autoWidth": false, "pageLength": 5, "bLengthChange": false, "order": [], "bPaginate": false, "bInfo": false, "aoColumnDefs": [{
                    "bSortable": false, "aTargets": []
                }]
            });
        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({
            container: 'body'
        });
        if ($.fn.dataTable.isDataTable('#pnlDashboard #dgvPendingCCMEnrollmentInfoGrid'))
            ;
        else
            $("#pnlDashboard #pnlCCMEnrollmentInfoGrid #dgvPendingCCMEnrollmentInfoGrid").DataTable({
                "searching": false, "bInfo": false, "bPaginate": false, "pageLength": 5, "bLengthChange": false, "autoWidth": false, "order": [], "aoColumnDefs": [{
                    "bSortable": false, "aTargets": [6]
                }]
            }); // to remove records per page dropdown
    },
    DashBoardAcceptedCCMEnrollmentInfoGridLoad: function (response, PageNo, rpp) {

        if (response.CCMEnrollmentInfoCount > 0) {

            $("#pnlDashboard #divAcceptedCCMEnrollmentInfoPaging").css("display", "inline");
            //Showing 1 to 15 of 15 Record(s)
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            // fixMe
            if (CurrentPage == 1) {
                if (RecordsPerPage == 1000) {
                    RecordsPerPage = 15;
                }
            }

            var params = [];
            params["CurrentPage"] = CurrentPage;
            params["RecordsPerPage"] = RecordsPerPage;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("divAcceptedCCMEnrollmentInfoPaging", response.iTotalDisplayRecords, 5, "DashBoard", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#pnlDashboard #divAcceptedCCMEnrollmentInfoPaging #divShowingEntries").text(showingText);

            $("#pnlDashboard #divAcceptedCCMEnrollmentInfoPaging li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });

        }
        else {
            $("#pnlDashboard #divAcceptedCCMEnrollmentInfoPaging").css("display", "none");
        }

        $("#pnlDashboard #pnlCCMEnrollmentInfoGrid #dgvAcceptedCCMEnrollmentInfoGrid").dataTable().fnDestroy();
        $("#pnlDashboard #pnlCCMEnrollmentInfoGrid #dgvAcceptedCCMEnrollmentInfoGrid tbody").find("tr").remove();
        if (response.CCMEnrollmentInfoCount > 0) {
            var CCMEnrollmentInfoJSONData = response.CCMEnrollmentInfo_JSON;
            $.each(CCMEnrollmentInfoJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", '' + item.PatientId + '');
                $row.attr("onclick", "utility.SelectGridRow($('#gvDashBoard_AcceptedCCMEnrollmentInfoGrid_row" + i + "'))");

                if (item.IsActive == "True") {
                    isactive = 0;
                    tglclass = "fa fa-toggle-on green";
                } else {
                    isactive = 1;
                    tglclass = "fa fa-toggle-on red";
                }


                var AddCCMTimeMethod = '<a class="btn btn-xs" onclick="DashBoard.AddCCMTaskTime(\'' + item.EnrollmentInfoId + '\', \'' + item.PatientId + '\');"  title="Add Time"><i class="fa fa-plus-square"></i></a>'
                var EditMethod = '<a class="btn btn-xs" onclick="DashBoard.EditEnrollmentInfo(\'' + item.EnrollmentInfoId + '\',\'' + item.PatientId + '\');"  title="Edit Record"><i class="fa fa-edit black"></i></a>'
                var CCMHubMethod = '<a class="btn btn-warning btn-xs mr-xs" onclick="DashBoard.OpenCCMHub(\'' + item.EnrollmentInfoId + '\', \'' + item.PatientId + '\');">Hub</a>'

                $("#hfpatientid").val(item.PatientId);
                var d = new Date();
                var month = new Array();
                month[0] = "January";
                month[1] = "February";
                month[2] = "March";
                month[3] = "April";
                month[4] = "May";
                month[5] = "June";
                month[6] = "July";
                month[7] = "August";
                month[8] = "September";
                month[9] = "October";
                month[10] = "November";
                month[11] = "December";
                var n = month[d.getMonth()];
                var parameters = item.PatientId + ",'mstrTabDashBoard', event";
                $row.append('<td>' + EditMethod + '</td><td><a href="#" onclick="utility.PatientDemographics(' + parameters + ');">' + item.AccountNumber + '</a></td><td><a href="#" onclick="utility.PatientDemographics(' + parameters + ');">' + item.PatientName + '</a></td><td>' + item.ProviderName + '</td><td data-toggle="tooltip" title="" data-original-title="' + item.Problems + '">' + item.NoOfProblems + '</td><td>' + item.Program + '</td><td>' + item.InsuranceName + '</td><td>' + item.Duration + '</td><td>' + item.TimeCompleted + AddCCMTimeMethod + '</td><td>' + item.NoteStatus + '</td><td>' + item.Consent + '</td><td>' + n + '</td><td>' + CCMHubMethod + '</td>');

                $("#pnlDashboard #dgvAcceptedCCMEnrollmentInfoGrid tbody").last().append($row);
            });
        }
        else {
            $("#pnlDashboard #divAcceptedCCMEnrollmentInfoPaging").css("display", "none");
            $('#pnlDashboard #pnlCCMEnrollmentInfoGrid #dgvAcceptedCCMEnrollmentInfoGrid').DataTable({
                "language": {
                    "emptyTable": "No Data Found"
                }, "searching": false, "autoWidth": false, "pageLength": 5, "bLengthChange": false, "order": [], "bPaginate": false, "bInfo": false, "aoColumnDefs": [{
                    "bSortable": false, "aTargets": [0]
                }]
            });
        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({
            container: 'body'
        });
        if ($.fn.dataTable.isDataTable('#pnlDashboard #dgvAcceptedCCMEnrollmentInfoGrid'))
            ;
        else
            $("#pnlDashboard #pnlCCMEnrollmentInfoGrid #dgvAcceptedCCMEnrollmentInfoGrid").DataTable({
                "searching": false, "bInfo": false, "bPaginate": false, "pageLength": 5, "bLengthChange": false, "autoWidth": false, "order": [], "aoColumnDefs": [{
                    "bSortable": false, "aTargets": [0, 12]
                }]
            }); // to remove records per page dropdown
    },

    DashBoardDeclinedCCMEnrollmentInfoGridLoad: function (response, PageNo, rpp) {

        if (response.CCMEnrollmentInfoCount > 0) {

            $("#pnlDashboard #divDeclinedCCMEnrollmentInfoPaging").css("display", "inline");
            //Showing 1 to 15 of 15 Record(s)
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            // fixMe
            if (CurrentPage == 1) {
                if (RecordsPerPage == 1000) {
                    RecordsPerPage = 15;
                }
            }

            var params = [];
            params["CurrentPage"] = CurrentPage;
            params["RecordsPerPage"] = RecordsPerPage;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("divDeclinedCCMEnrollmentInfoPaging", response.iTotalDisplayRecords, 5, "DashBoard", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#pnlDashboard #divDeclinedCCMEnrollmentInfoPaging #divShowingEntries").text(showingText);

            $("#pnlDashboard #divDeclinedCCMEnrollmentInfoPaging li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });

        }
        else {
            $("#pnlDashboard #divDeclinedCCMEnrollmentInfoPaging").css("display", "none");
        }

        $("#pnlDashboard #pnlCCMEnrollmentInfoGrid #dgvDeclinedCCMEnrollmentInfoGrid").dataTable().fnDestroy();
        $("#pnlDashboard #pnlCCMEnrollmentInfoGrid #dgvDeclinedCCMEnrollmentInfoGrid tbody").find("tr").remove();
        if (response.CCMEnrollmentInfoCount > 0) {
            var CCMEnrollmentInfoJSONData = response.CCMEnrollmentInfo_JSON;
            $.each(CCMEnrollmentInfoJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", '' + item.PatientId + '');
                $row.attr("onclick", "utility.SelectGridRow($('#gvDashBoard_DeclinedCCMEnrollmentInfoGrid_row" + i + "'))");

                if (item.IsActive == "True") {
                    isactive = 0;
                    tglclass = "fa fa-toggle-on green";
                } else {
                    isactive = 1;
                    tglclass = "fa fa-toggle-on red";
                }


                var ResumeMethod = "";

                if (item.Status == "Declined") {
                    ResumeMethod = '<a class="btn btn-success btn-xs mr-xs" onclick="DashBoard.EnrollForCCM(\'' + item.PatientId + '\',\'' + item.PatientName + '\',\'' + item.ProviderName + '\');"> Accept</a>'
                }
                else {
                    ResumeMethod = '<a class="btn btn-success btn-xs mr-xs" onclick="DashBoard.ResumeForCCM(\'' + item.EnrollmentInfoId + '\');">Resume</a>'
                }


                $("#hfpatientid").val(item.PatientId);
                var parameters = item.PatientId + ",'mstrTabDashBoard', event";
                $row.append('<td><a href="#" onclick="utility.PatientDemographics(' + parameters + ');">' + item.AccountNumber + '</a></td><td><a href="#" onclick="utility.PatientDemographics(' + parameters + ');">' + item.PatientName + '</a></td><td>' + item.ProviderName + '</td><td>' + item.Problems + '</td><td>' + item.InsuranceName + '</td><td>' + item.CreatedOn + '</td><td>' + item.Status + '</td><td>' + item.Comments + '</td><td>' + ResumeMethod + '</td>');

                $("#pnlDashboard #dgvDeclinedCCMEnrollmentInfoGrid tbody").last().append($row);
            });
        }
        else {
            $("#pnlDashboard #divDeclinedCCMEnrollmentInfoPaging").css("display", "none");
            $('#pnlDashboard #pnlCCMEnrollmentInfoGrid #dgvDeclinedCCMEnrollmentInfoGrid').DataTable({
                "language": {
                    "emptyTable": "No Data Found"
                }, "searching": false, "autoWidth": false, "pageLength": 5, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{
                    "bSortable": false, "aTargets": [0]
                }]
            });
        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({
            container: 'body'
        });
        if ($.fn.dataTable.isDataTable('#pnlDashboard #dgvDeclinedCCMEnrollmentInfoGrid'))
            ;
        else
            $("#pnlDashboard #pnlCCMEnrollmentInfoGrid #dgvDeclinedCCMEnrollmentInfoGrid").DataTable({
                "searching": false, "bInfo": false, "bPaginate": false, "pageLength": 5, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{
                    "bSortable": false, "aTargets": [8]
                }]
            }); // to remove records per page dropdown
    },


    EnrollForCCM: function (PatientId, PatientName, ProviderName) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Chronic Care Management", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ParentCtrl"] = 'mstrTabDashBoard';
                params["PatientId"] = PatientId;
                params["PatientName"] = PatientName;
                params["ProviderName"] = ProviderName;
                params["FromAdmin"] = "0";
                params["mode"] = "add";

                LoadActionPan('CCMEnrollmentInfo', params);
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    ResumeForCCM: function (EnrollmentInfoId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Chronic Care Management", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('Are you sure you want to Resume CCM Program?', function () {

                    DashBoard.CCMEnrollmentResume(EnrollmentInfoId).done(function (response) {
                        if (response.status != false) {
                            DashBoard.DashBoardCCMEnrollmentInfoSearch();
                            setPatientBanner($("#hfPatientId").val(), "1");
                            utility.DisplayMessages(response.Message, 1);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                },
        function () {
            /* cancel resume */
        },
                'Confirm Resume');
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    CCMEnrollmentResume: function (EnrollmentInfoId) {

        var objData = new Object();

        objData["EnrollmentInfoId"] = EnrollmentInfoId
        objData["StatusId"] = "2";

        var data = JSON.stringify(objData);
        return MDVisionService.CCMAPIService(data, "CCMEnrollmentInfo", "ResumeCCMEnrollmentInfo");
    },

    EditEnrollmentInfo: function (EnrollmentInfoId, PatientId) {
        var params = [];
        params["ParentCtrl"] = 'mstrTabDashBoard';
        params["EnrollmentInfoId"] = EnrollmentInfoId;
        params["PatientId"] = PatientId;
        params["FromAdmin"] = "0";
        params["mode"] = "edit";
        LoadActionPan('CCMEnrollmentInfo', params);
    },

    AddCCMTaskTime: function (EnrollmentInfoId, PatientId) {
        var params = [];
        params["ParentCtrl"] = 'mstrTabDashBoard';
        params["EnrollmentInfoId"] = EnrollmentInfoId;
        params["PatientId"] = PatientId;
        params["FromAdmin"] = "0";
        params["mode"] = "add";
        LoadActionPan('CCMTaskTimerDetail', params);
    },



    DeclineForCCM: function (PatientId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Chronic Care Management", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ParentCtrl"] = 'mstrTabDashBoard';
                params["PatientId"] = PatientId;
                params["mode"] = "add";
                params["FromAdmin"] = "0";

                LoadActionPan('CCMEnrollmentDecline', params);
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    OpenCCMHub: function (EnrollmentInfoId, PatientId) {
        params["EnrollmentInfoId"] = EnrollmentInfoId;
        params["PatientId"] = PatientId;
        LoadActionPan('CCM_Patient_Hub', params);
    },



    BindPatientAccount: function () {
        var valid = false;
        var self = $("#pnlDashboard #pnlCCMEnrollmentInfoGrid");
        var Ctrl = self.find("#txtPatientName");
        var hfCtrl = self.find("#hfPatientId");
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
                else {
                    return false;
                }
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
        };
        var onSelect = function (e) {
            var dataItem = this.dataItem(e.item.index());
            self.find("#txtPatientName").val(dataItem.AccountNumber);
            self.find("#txtPatientName").attr("data-PatientId", dataItem.id);
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

    OpenPatientSearch: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'mstrTabDashBoard';

        LoadActionPan('Patient_Search', params);
    },
    FillPatientInfoFromSearch: function (PatientId, AccountNumber, patFullName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var self = null;
        if (Patient_Search.params.ActiveWidget == "TCM") {
            self = $("#pnlDashboard #pnlTCMGrid");
            self.find("#hfTCMPatientId").val(PatientId);
            self.find("#txtFullName").val(patFullName);
            $Ctrl = $("#pnlDashboard #pnlTCMGrid #txtFullName");
            $hfCtrl = $("#pnlDashboard #pnlTCMGrid #hfTCMPatientId");
            //Patient
            utility.SetKendoAutoCompleteSourceforValidate($Ctrl, patFullName, $hfCtrl, PatientId, "FullName");
        }
        else if (Patient_Search.params.ActiveWidget == "Encounter") {
            if (globalAppdata.IsProviderBulkSign == "True") {
                self = $("#pnlDashboard #pnlEncounterGrid #" + DashBoard.InnerCtrl);
                self.find("#hfPatientId").val(PatientId);
                self.find("#txtFullName").val(patFullName);
                $Ctrl = $("#pnlDashboard #pnlEncounterGrid #" + DashBoard.InnerCtrl + " #txtFullName");
                $hfCtrl = $("#pnlDashboard #pnlEncounterGrid #" + DashBoard.InnerCtrl + " #hfPatientId");
                //Patient
                utility.SetAutoCompleteSource($Ctrl, $hfCtrl);
            } else {
                self = $("#pnlDashboard #pnlEncounterGridOld");
                self.find("#OldhfPatientId").val(PatientId);
                self.find("#OldtxtFullName").val(patFullName);
                $Ctrl = $("#pnlDashboard #pnlEncounterGridOld #OldtxtFullName");
                $hfCtrl = $("#pnlDashboard #pnlEncounterGridOld #OldhfPatientId");
                //Patient
                utility.SetAutoCompleteSource($Ctrl, $hfCtrl);
            }
        }
        else if (Patient_Search.params.ActiveWidget == "Documents") {
            self = $("#pnlDashboard #pnlPatientDocumentGrid");
            self.find("#hfPatientId").val(PatientId);
            self.find("#txtFullName").val(patFullName);
            $Ctrl = $("#pnlDashboard #pnlPatientDocumentGrid #txtFullName");
            $hfCtrl = $("#pnlDashboard #pnlPatientDocumentGrid #hfPatientId");
            //Patient
            utility.SetKendoAutoCompleteSourceforValidate($Ctrl, patFullName, $hfCtrl, PatientId, "FullName");
        }
        else if (Patient_Search.params.ActiveWidget == "DataChangeRequest") {
            self = $("#pnlDashboard #pnlDataChangeRequestGrid");
            self.find("#hfPatientId").val(PatientId);
            self.find("#txtFullName").val(patFullName);
            utility.SetKendoAutoCompleteSourceforValidate(self.find("#txtFullName"), patFullName, self.find("#hfPatientId"), PatientId, "FullName");
        }
        else if (Patient_Search.params.ActiveWidget == "LabOrder") {
            self = $("#pnlDashboard #ctrlPanLabOrder");
            self.find("#hfPatientId").val(PatientId);
            self.find("#txtOrderFullName").val(patFullName);
            $Ctrl = $("#pnlDashboard #ctrlPanLabOrder #txtOrderFullName");
            $hfCtrl = $("#pnlDashboard #ctrlPanLabOrder #hfPatientId");
            //Patient
            utility.SetKendoAutoCompleteSourceforValidate($Ctrl, patFullName, $hfCtrl, PatientId, "FullName");
            //EMR 6266
            $Ctrl.focus();

        }
        else if (Patient_Search.params.ActiveWidget == "LabResult") {
            self = $("#pnlDashboard #LabResult");
            self.find("#hfPatientId").val(PatientId);
            self.find("#txtResultFullName").val(patFullName);
            $Ctrl = $("#pnlDashboard #LabResult #txtResultFullName");
            $hfCtrl = $("#pnlDashboard #LabResult #hfPatientId");
            //Patient
            utility.SetKendoAutoCompleteSourceforValidate($Ctrl, patFullName, $hfCtrl, PatientId, "FullName");
            //EMR 6266
            $Ctrl.focus();
        }
        else if (Patient_Search.params.ActiveWidget == "ActiveAccounts") {
            self = $("#pnlDashboard #ctrlPanPatPortalAccounts");
            self.find("#hfPatientId").val(PatientId);
            self.find("#hfAccountNo").val(AccountNumber);
            self.find("#txtFullName").val(patFullName);
            self.find("#txtAccountNumber").val(AccountNumber);
            $("#pnlDashboard #ctrlPanPatPortalAccounts #txtFullName").removeAttr("disabled", "disabled");
            $("#pnlDashboard #ctrlPanPatPortalAccounts #lnkPatientName").removeAttr("disabled", "disabled");
            $("#pnlDashboard #ctrlPanPatPortalAccounts #txtAccountNumber").removeAttr("disabled", "disabled");
            $("#pnlDashboard #ctrlPanPatPortalAccounts #lnkPatientAccount").removeAttr("disabled", "disabled");
            if (Patient_Search.params.RefCtrl == 'txtFullName') {
                $("#pnlDashboard #ctrlPanPatPortalAccounts #txtAccountNumber").attr("disabled", "disabled");
                $("#pnlDashboard #ctrlPanPatPortalAccounts #lnkPatientAccount").attr("disabled", "disabled");
                $Ctrl = $("#pnlDashboard #ctrlPanPatPortalAccounts #txtFullName");
                $hfCtrl = $("#pnlDashboard #ctrlPanPatPortalAccounts #hfPatientId");
                utility.SetKendoAutoCompleteSourceforValidate($Ctrl, patFullName, $hfCtrl, PatientId, "FullName");
            }
            else {
                $("#pnlDashboard #ctrlPanPatPortalAccounts #txtFullName").attr("disabled", "disabled");
                $("#pnlDashboard #ctrlPanPatPortalAccounts #lnkPatientName").attr("disabled", "disabled");
                $Ctrl = $("#pnlDashboard #ctrlPanPatPortalAccounts #txtAccountNumber");
                $hfCtrl = $("#pnlDashboard #ctrlPanPatPortalAccounts #hfPatientId");
                utility.SetKendoAutoCompleteSourceforValidate($Ctrl, AccountNumber, $hfCtrl, PatientId, "AccountNumber");
            }
        }
        else if (Patient_Search.params.ActiveWidget == "Referrals") {
            if (Patient_Search.params.Type == 'incoming') {
                self = $("#pnlDashboard #ctrlPanIncomingReferral");
                self.find("#hfPatientId").val(PatientId);
                self.find("#txtFullName").val(patFullName);

                $Ctrl = $("#pnlDashboard #ctrlPanIncomingReferral #txtFullName");
                $hfCtrl = $("#pnlDashboard #ctrlPanIncomingReferral #hfPatientId");
                utility.SetKendoAutoCompleteSourceforValidate($Ctrl, patFullName, $hfCtrl, PatientId, "FullName");
            }
            else {
                self = $("#pnlDashboard #ctrlPanOutgoingReferral");
                self.find("#hfPatientId").val(PatientId);
                self.find("#txtFullName").val(patFullName);

                $Ctrl = $("#pnlDashboard #ctrlPanOutgoingReferral #txtFullName");
                $hfCtrl = $("#pnlDashboard #ctrlPanOutgoingReferral #hfPatientId");
                utility.SetKendoAutoCompleteSourceforValidate($Ctrl, patFullName, $hfCtrl, PatientId, "FullName");
            }
        }
        else {
            self = $("#pnlDashboard #pnlCCMEnrollmentInfoGrid");

            self.find("#hfPatientId").val(PatientId);
            self.find("#txtPatientName").val(AccountNumber);
            self.find("#txtPatientName").attr("data-PatientId", PatientId);
            utility.SetKendoAutoCompleteSourceforValidate(self.find("#txtPatientName"), AccountNumber, self.find("#hfPatientId"), PatientId, "AccountNumber");
        }



        UnloadActionPan("mstrTabDashBoard");
        utility.InsertRecentPatient(PatientId);
    },

    OpenProvider: function (ParentRefCtrl, innerCtrl) {
        var params = [];
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        if (innerCtrl && innerCtrl != "" && globalAppdata.IsProviderBulkSign == "True") {
            params["RefCtrl"] = innerCtrl + " #txtProvider";
            params["RefCtrlHidden"] = innerCtrl + " #hfProviderId";
        } else {
            params["RefCtrl"] = "OldtxtProvider";
            params["RefCtrlHidden"] = "OldhfProviderId";
        }
        params["ParentCtrl"] = "mstrTabDashBoard";
        params["ParentRefCtrl"] = ParentRefCtrl;
        LoadActionPan('Admin_Provider', params);
    },

    OpenNoteProvider: function (ParentRefCtrl) {
        var params = [];
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtNoteProvider";
        params["RefCtrlHidden"] = "hfNoteProviderId";
        params["ParentCtrl"] = "mstrTabDashBoard";
        params["ParentRefCtrl"] = ParentRefCtrl;
        LoadActionPan('Admin_Provider', params);
    },

    OpenInsurancePlan: function (pnlInsurancePlan) {
        DashBoard.params["pnlInsurancePlan"] = pnlInsurancePlan;
        var params = [];
        params["ParentCtrl"] = 'mstrTabDashBoard';
        params["FromAdmin"] = "0";
        LoadActionPan('Admin_InsurancePlan', params);
    },

    FillInsurancePlanName: function (InsurancePlanId, InsurancePlanName, SearchPattern) {
        var self = null;
        self = $("#pnlDashboard #" + DashBoard.params["pnlInsurancePlan"]);
        self.find("#txtPrimaryInsurance").val(InsurancePlanName);
        self.find("#hfInsurancePlanId").val(InsurancePlanId);
        utility.SetKendoAutoCompleteSourceforValidate(self.find("#txtPrimaryInsurance"), InsurancePlanName, self.find("#hfInsurancePlanId"), InsurancePlanId);
        UnloadActionPan("mstrTabDashBoard");
    },

    /****************/

    LoadAbmormalLabResults: function () {

        $("#pnlDashboard #hfIsAbnormalTests").val("1");
        $("#pnlDashboard #listLabResult #results").trigger("click");
        if (!$.trim($('#pnlDashboard #LabResult #divSwitchLabResultAbnormal').html()).length) {
            if ($("#divSwitchLabResultAbnormal .btnWidgetSwitch div:first").hasClass("ios-switch on")) {
                $("#divSwitchLabResultAbnormal .btnWidgetSwitch div:first").removeClass("ios-switch on");
                $("#divSwitchLabResultAbnormal .btnWidgetSwitch div:first").removeClass("ios-switch off");
            }
        }
        //console.log("called");

    },
    viewPdfLabResult: function (currentPatientId, LabOrderId, ResultId) {
        var params = [];
        params["FromAdmin"] = "0";
        params["UserId"] = globalAppdata['AppUserId'];
        params["PatientId"] = currentPatientId;
        params["ParentCtrl"] = "mstrTabDashBoard";
        params["LabOrderId"] = LabOrderId;
        params["LabResultId"] = ResultId;
        params["Caller"] = "viewpdf";
        LoadActionPan('Clinical_LabResultView', params);

    },

    //-------------------
    BindPatientAccountAndName: function (obj, fromCtrlType, currentPatientId, currentPatientAccountNo, currentPatientFullName, LabOrderId, ResultType) {
        var AccountNo = obj != null ? $(obj).val() : $('#pnlDashboard #txtAccountNo').val();
        var ctrlAccountNo = obj != null ? $(obj) : $('#pnlDashboard #txtAccountNo');
        currentPatientId != null ? currentPatientId : "";
        currentPatientAccountNo != null ? currentPatientAccountNo : "";
        currentPatientFullName != null ? currentPatientFullName : "";
        utility.Keyupdelay(function () {
            var AllPatients = utility.GetPatientArrayByName(AccountNo, 1).done(function (response) {
                if (AccountNo == "") {
                    $('#pnlDashboard #frmappointmentDetail #txtReferralNo').val('');
                    if ($("#pnlDashboard #lnkRefProviderEdit").css("display") == "inline") {
                        $("#pnlDashboard #lnkRefProviderEdit").css("display", "none");
                        $("#pnlDashboard #lblRefProvider").css("display", "inline");
                    }
                    if ($("#pnlDashboard #lnkReferralNumberEdit").css("display") == "inline") {
                        $("#pnlDashboard #lnkReferralNumberEdit").css("display", "none");
                        $("#pnlDashboard #lblReferralNumber").css("display", "inline");
                    }
                }
                ctrlAccountNo.autocomplete({
                    autoFocus: true,
                    source: response,
                    open: function (event, ui) {
                        disable = true
                    },
                    close: function (event, ui) {
                        disable = false; $(this).focus();
                    },
                    select: function (event, ui) {
                        setTimeout(function () {
                            ctrlAccountNo.val(ui.item.AccountNumber);
                            currentPatientId = ui.item.id;
                            currentPatientAccountNo = ui.item.AccountNumber;
                            currentPatientFullName = ui.item.FullName;
                            utility.InsertRecentPatient(ui.item.id);
                        }, 100);
                    }
                }).blur(function () {
                    setTimeout(function () {
                        utility.ValidateAutoComplete(ctrlAccountNo, "pnlDashboard #hfpatientid", false, 1, null, null);
                        if (fromCtrlType == "DashBoardLabOrderResults") {
                            DashBoard.UpdatePatientOfLabOrder(currentPatientId, currentPatientAccountNo, currentPatientFullName, event, LabOrderId, ResultType);
                        }
                    }, 200);
                });
                ctrlAccountNo.autocomplete("search");
                $('#pnlDashboard #txtAccountNo').focus();
            });
        });
    },

    printLabOrder: function (LabId, status, event, PatientId) {
        if (event != null) {
            event.stopPropagation();
        }
        if (status == 'Signed' || status == "Transmitted") {
            var params = [];
            params["FromAdmin"] = "0";
            params["UserId"] = globalAppdata['AppUserId'];
            var currentPatientId = PatientId != null && parseInt(PatientId) > 0 ? PatientId : $('#PatientProfile #hfPatientId').val();
            params["PatientId"] = currentPatientId;
            // params["ParentCtrl"] = Clinical_LabOrder.params.TabID;
            params["ParentCtrl"] = (Clinical_LabOrder.params.ParentCtrl != "clinicalTabProgressNote" && Clinical_LabOrder.params.ParentCtrl != "clinicalTabFaceSheet" && Clinical_LabOrder.params.ParentCtrl != null) ? Clinical_LabOrder.params.TabID : "Clinical_LabOrder";
            if ($('#mstrTabDashBoard').hasClass('active')) {
                params["ParentCtrl"] = undefined;
                Clinical_LabOrder.params["ParentCtrl"] = undefined;
            }
            params["LabOrderId"] = LabId;

            utility.myConfirm('Would you like to print the Specimen Label for this order?', function () {
                if (Clinical_LabOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
                    params["ParentCtrlPanelID"] = Clinical_LabOrder.params.PanelID;
                    params["BarCodeHtml"] = 'true';
                    params["IsSpecimen"] = true;
                    DashBoard.printLabOrderDir(LabId, true, event, currentPatientId);

                } else {
                    params["BarCodeHtml"] = 'true';
                    params["IsSpecimen"] = true;
                    DashBoard.printLabOrderDir(LabId, true, event, currentPatientId);
                }



            }, function () {
                if (Clinical_LabOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
                    params["BarCodeHtml"] = 'true'; // IMP-481 // MK
                    params["ParentCtrlPanelID"] = Clinical_LabOrder.params.PanelID;
                    DashBoard.printLabOrderDir(LabId, false, event, currentPatientId);
                } else {
                    params["BarCodeHtml"] = 'true'; // IMP-481 // MK
                    DashBoard.printLabOrderDir(LabId, false, event, currentPatientId);
                }

            },
                           'Specimen Label Printing');
        }
    },

    printLabOrderDir: function (LabId, status, event, PatientId) {
        Clinical_LabOrderView.previewLabOrder(PatientId, globalAppdata['AppUserId'], LabId).done(function (response) {
            response = JSON.parse(response);
            if (response.status) {
                var params = [];
                if (status) {
                    ClinicalLabOrderDetail.generateBarcode();

                    params["BarCodeHtml"] = 'true';
                    params["IsSpecimen"] = true;
                    $('#' + ClinicalLabOrderDetail.params.PanelID + " #barcodeTarget").hide();
                    //Clinical_LabOrder.printSpecimen(LabId, 'Signed', null, 'mstrTabDashBoard');


                    params["PatientID"] = Clinical_LabOrder.params.patientID;
                    params["LabOrderId"] = LabId;
                    params["providerid"] = $('#pnlClinicalLabOrder #hfprovider').val();
                    params["ParentCtrl"] = 'mstrTabDashBoard';
                    LoadActionPan('ClinicalBarCodeView', params);



                    Clinical_LabOrderView.params["IsSpecimen"] = false;

                } else {
                    params["BarCodeHtml"] = 'true';
                    params["IsSpecimen"] = false;
                }
                utility.documentPrint(response.LabOrderHTML);
            }
            else {
                utility.DisplayMessages(response.Message, 2);
            }
        });

    },
    LoadCDSAlert: function () {

        if ($("div#PatientProfile #hfPatientId").val() != "") {
            $("#mainForm  li#CDSAlert").show();
            var triggerLocation = 'Dashboard';

            IsBackgroundLoaderShow = false;
            ClinicalCDSDetail.showCDSAlert(triggerLocation, 0);
            IsBackgroundLoaderShow = true;
        }

    },

    SetDefsultProviderValues: function () {
        $("#pnlDashboard #pnlEncounterGrid #txtProvider").val($("#pnlDashboard #NoteProviderText").val());
    },

    BindPatientNameAtNoteTab: function (innerCtrl) {
        var AccountNo = $("#pnlDashboard #pnlEncounterGrid #" + innerCtrl + " #txtFullName").val();
        $("#pnlDashboard #" + innerCtrl + " #hfPatientId").val("");
        // Start 08/03/2016 Muhammad Irfan for bug # PMS-4361
        utility.Keyupdelay(function () {
            var AllPatients = utility.GetPatientArrayByName(AccountNo, 1).done(function (response) {
                $.each(response, function (i, item) {
                    item.value = item.FullName;
                });
                $("#pnlDashboard #pnlEncounterGrid #" + innerCtrl + " #txtFullName").autocomplete({
                    //source: AllPatients, // pass an array (without a comma)
                    autoFocus: true,
                    source: response,
                    open: function (event, ui) {
                        disable = true
                    },
                    close: function (event, ui) {
                        disable = false; $(this).focus();
                    },
                    select: function (event, ui) {
                        setTimeout(function () {
                            $("#pnlDashboard #pnlEncounterGrid #" + innerCtrl + " #txtFullName").val(ui.item.FullName);
                            $("#pnlDashboard #" + innerCtrl + " #hfPatientId").val(ui.item.id);
                            //  appointmentDetail.FillPatientAccount(ui.item.id);
                            utility.InsertRecentPatient(ui.item.id);
                        }, 100);
                    }
                }).blur(function () {
                    //Start 10/02/2016 Muhammad Irfan for bug PMS-3873
                    setTimeout(function () {
                        // utility.ValidateAutoCompletePatientName($("#pnlDashboard #pnlEncounterGrid #txtFullName"), "pnlEncounterGrid #hfTCMPatientId", false, 1, null, null);
                    }, 200);
                    //End 10/02/2016 Muhammad Irfan for bug PMS-3873
                });


                $("#pnlDashboard #pnlEncounterGrid #" + innerCtrl + " #txtFullName").autocomplete("search");
                $("#pnlDashboard #pnlEncounterGrid #" + innerCtrl + " #txtFullName").focus();
            });
        });
        // End 08/03/2016 Muhammad Irfan for bug # PMS-4361

    },

    BindPatientNameAtNoteTabOld: function () {
        var AccountNo = $("#pnlDashboard #pnlEncounterGridOld #OldtxtFullName").val();
        $("#pnlDashboard #pnlEncounterGridOld #OldhfPatientId").val("");
        // Start 08/03/2016 Muhammad Irfan for bug # PMS-4361
        utility.Keyupdelay(function () {
            var AllPatients = utility.GetPatientArrayByName(AccountNo, 1).done(function (response) {
                $.each(response, function (i, item) {
                    item.value = item.FullName;
                });
                $("#pnlDashboard #pnlEncounterGridOld #OldtxtFullName").autocomplete({
                    //source: AllPatients, // pass an array (without a comma)
                    autoFocus: true,
                    source: response,
                    open: function (event, ui) {
                        disable = true
                    },
                    close: function (event, ui) {
                        disable = false; $(this).focus();
                    },
                    select: function (event, ui) {
                        setTimeout(function () {
                            $("#pnlDashboard #pnlEncounterGridOld #OldtxtFullName").val(ui.item.FullName);
                            $("#pnlDashboard #pnlEncounterGridOld #OldhfPatientId").val(ui.item.id);
                            //  appointmentDetail.FillPatientAccount(ui.item.id);
                            utility.InsertRecentPatient(ui.item.id);
                        }, 100);
                    }
                }).blur(function () {
                    //Start 10/02/2016 Muhammad Irfan for bug PMS-3873
                    setTimeout(function () {
                        // utility.ValidateAutoCompletePatientName($("#pnlDashboard #pnlEncounterGrid #txtFullName"), "pnlEncounterGrid #hfTCMPatientId", false, 1, null, null);
                    }, 200);
                    //End 10/02/2016 Muhammad Irfan for bug PMS-3873
                });


                $("#pnlDashboard #pnlEncounterGridOld #OldtxtFullName").autocomplete("search");
                $("#pnlDashboard #pnlEncounterGridOld #OldtxtFullName").focus();
            });
        });
        // End 08/03/2016 Muhammad Irfan for bug # PMS-4361

    },

    SearchDashBoardDataChangeRequest: function (PageNo, rpp, GridData) {
        DashBoard.DashBoardDataChangeRequestSearch(PageNo, rpp, GridData);
    },


    DashBoardDataChangeRequestSearch: function (PageNo, rpp, GridData) {
        //Start By Khaleel Ur Rehman to Validate From and To Date , on 28 january 2016.
        //   utility.ValidateFromToDate('dpModifiedNoteFrom', 'dpVisitFrom', 'dpModifiedNoteTo', true);
        //End By Khaleel Ur Rehman to Validate From and To Date , on 28 january 2016.
        DashBoard.EnableDisableControl();
        $("#pnlDashboard #pnlDataChangeRequestGrid #btnCheckInAppRequests").attr("onclick", "DashBoard.SearchDashBoardDataChangeRequest(null, null, null);");
        $("#pnlDashboard #pnlDataChangeRequestGrid #btnRefreshChangeRequest").attr("onclick", "DashBoard.SearchDashBoardDataChangeRequest(null, null, null);");

        PageNo = (PageNo == null || PageNo == "") ? 1 : PageNo;
        rpp = (rpp == null || rpp == "") ? 15 : rpp;
        if ($("#pnlDashboard #pnlDataChangeRequestGrid").css("display") == "none") {
            $("#pnlDashboard #pnlDataChangeRequestGrid").show();
        }
        if (GridData) {

            DashBoard.DashBoardDataChangeGridLoad(GridData);
        }
        else {

            DashBoard.LoadDashBoardDataChangeRequest(PageNo, rpp).done(function (response) {
                if (response.status != false) {
                    DashBoard.DashBoardDataChangeGridLoad(response, PageNo, rpp);
                    var TableControl = "pnlDashboard #pnlDataChangeRequestGrid #dgvDataChangeRequestGrid";
                    var PagingPanelControlID = "pnlDashboard #dgvDataChangeRequestGrid_paginate";
                    var ClassControlName = "DashBoard";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout(function () {
                        CreatePagination(response.CheckInPatientsCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            DashBoard.DashBoardDataChangeRequestSearch(PageNumber, ResultPerPage);
                        })
                    }, 10);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    DashBoardCheckInRequestSearch: function (PageNo, rpp, GridData) {
        //Start By Khaleel Ur Rehman to Validate From and To Date , on 28 january 2016.
        //   utility.ValidateFromToDate('dpModifiedNoteFrom', 'dpVisitFrom', 'dpModifiedNoteTo', true);
        //End By Khaleel Ur Rehman to Validate From and To Date , on 28 january 2016.
        DashBoard.EnableDisableControl();
        $("#pnlDashboard #pnlDataChangeRequestGrid #btnCheckInAppRequests").attr("onclick", "DashBoard.DashBoardCheckInRequestSearch(null, null, null);");
        $("#pnlDashboard #pnlDataChangeRequestGrid #btnRefreshChangeRequest").attr("onclick", "DashBoard.DashBoardCheckInRequestSearch(null, null, null);");

        PageNo = (PageNo == null || PageNo == "") ? 1 : PageNo;
        rpp = (rpp == null || rpp == "") ? 15 : rpp;
        if ($("#pnlDashboard #pnlDataChangeRequestGrid").css("display") == "none") {
            $("#pnlDashboard #pnlDataChangeRequestGrid").show();
        }
        if (GridData) {

            DashBoard.DashBoardCheckInRequestGridLoad(GridData);
        }
        else {

            DashBoard.LoadDashBoardCheckInRequest(PageNo, rpp).done(function (response) {
                if (response.status != false) {
                    DashBoard.DashBoardCheckInRequestGridLoad(response, PageNo, rpp);
                    var TableControl = "pnlDashboard #pnlDataChangeRequestGrid #dgvDataChangeRequestGrid";
                    var PagingPanelControlID = "pnlDashboard #dgvDataChangeRequestGrid_paginate";
                    var ClassControlName = "DashBoard";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout(function () {
                        CreatePagination(response.CheckInPatientsCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            DashBoard.DashBoardDataChangeRequestSearch(PageNumber, ResultPerPage);
                        })
                    }, 10);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    //Start 12-10-2017 Humaira Yousaf IMP-1195
    BindAppointmentsProvider: function (Id, linkId, lblId, hfId) {

        if (!Id)
            Id = 'txtProvider';
        if (!linkId)
            linkId = 'lnkProvider';
        if (!lblId)
            lblId = 'lblProvider';
        if (!hfId)
            hfId = 'hfProviderId';
        var Ctrl = $("#pnlDashboard #pnlSchAppointmentGrid #" + Id);
        var func = function () {
            return utility.GetProviderArray(Ctrl.val())
        };
        var hfCtrl = $("#pnlDashboard #pnlSchAppointmentGrid #" + hfId);
        var onSelect = function (e) {
            if ($("#pnlDashboard #pnlSchAppointmentGrid #" + linkId).css("display") == "none") {
                $("#pnlDashboard #pnlSchAppointmentGrid #" + linkId).css("display", "inline");
                $("#pnlDashboard #pnlSchAppointmentGrid #" + lblId).css("display", "none");
            }
        };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect);
    },

    InputAppointmentsProvider: function () {
        $("#pnlDashboard #pnlSchAppointmentGrid #hfProviderId").val("");
    },

    BindAppointmentsFacility: function (Id, linkId, lblId, hfId) {

        if (!Id)
            Id = 'txtFacility';
        if (!linkId)
            linkId = 'lnkFacility';
        if (!lblId)
            lblId = 'lblFacility';
        if (!hfId)
            hfId = 'hfFacilityId';
        var Ctrl = $("#pnlDashboard #pnlSchAppointmentGrid #" + Id);
        var func = function () {
            return utility.GetFacilityArray(Ctrl.val())
        };
        var hfCtrl = $("#pnlDashboard #pnlSchAppointmentGrid #" + hfId);
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },


    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmDashboard";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "mstrTabDashBoard";
        LoadActionPan('Admin_Facility', params);
    },
    //End 12-10-2017 Humaira Yousaf IMP-1195


    ComposeDirectMessage: function () {
        AppPrivileges.GetFormPrivilegesByModule("Messages", "ADD", "Dash Board", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE_BY_MODULE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = Patient_UserMessages.params["TabID"];
                params["mode"] = 'Add';
                params["FromPatModule"] = '1';
                params["MessageType"] = 'Direct';
                LoadActionPan('Direct_MessageCreate', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);

        });

    },
    DashBoardOutgoingDirectMessagesSearch: function (PageNo, rpp, GridData) {
        // utility.CreateDatePicker("pnlDashboard #frmDashboard #dtpMsgDateDirect", function () { }, false, null, null);

        if ($("#pnlDashboard #pnlUserMessagesGrid").css("display") == "none")
            $("#pnlDashboard #pnlUserMessagesGrid").show();

        $("#pnlDashboard #pnlUserMessagesGrid #msgsPractice").hide();
        $("#pnlDashboard #pnlUserMessagesGrid #msgsPatient").hide();
        $("#pnlDashboard #pnlUserMessagesGrid #msgsLog").hide();
        if (GridData) {
            DashBoard.DashBoardOutgoingDirectMessagesGridLoad(GridData, PageNo, rpp);
        }
        else {
            DashBoard.LoadOutgoinDirectMessages(PageNo, rpp).done(function (response) {
                if (response.status != false) {
                    DashBoard.DashBoardOutgoingDirectMessagesGridLoad(response, PageNo, rpp);
                    $('#pnlDashboard .tabs-custom').eq(0).find('li').eq(3).attr('class', 'active');
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

        }
        DashBoard.MessageCount();
    },

    DashBoardDirectMessagesChangeStatus: function (status) {
        // var id = event.target.name.split('-')[1];
        // var status =
        DashBoard.UpdateOutgoinDirectMessagesStatus(0, status);
        {
            if (VarArrayDelivered.length > 0) {
                utility.DisplayMessages("Successfully Updated!", 1);
                DashBoard.DashBoardOutgoingDirectMessagesSearch();
                VarArrayDelivered = [];
            }
            // else {
            //utility.DisplayMessages(response.Message, 3);
            // }
        }

        //{

        //    if (status) {
        //        utility.DisplayMessages("Successfully Updated!", 1);
        //        DashBoard.DashBoardOutgoingDirectMessagesSearch();
        //    }
        //    else {
        //        utility.DisplayMessages(response.Message, 3);
        //    }
        //}

    },

    UpdateOutgoinDirectMessagesStatus: function (ID, isdelivered) {

        var objData = new Object();
        objData["IDs"] = VarArrayDelivered.toString();
        objData["isdelivered"] = isdelivered;
        objData["CommandType"] = "Update_Outgoing_Direct_Messages";

        var data = JSON.stringify(objData);
        return MDVisionService.APIServiceSync(data, "DashBoard", "UpdateDashBoard");
    },

    DashBoardOutgoingDirectMessagesGridLoad: function (response, PageNo, rpp) {
        if (response.MessageCount > 0) {

            $("#pnlDashboard #divDirectMessagesOutPaging").css("display", "inline");
            //Showing 1 to 15 of 15 Record(s)
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var params = [];
            params["CurrentPage"] = CurrentPage;
            params["RecordsPerPage"] = RecordsPerPage;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("divDirectMessagesOutPaging", response.iTotalDisplayRecords, 5, "DashBoard", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#pnlDashboard #divDirectMessagesOutPaging #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $("#pnlDashboard li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
        }
        else {
            $("#pnlDashboard #divDirectMessagesOutPaging").css("display", "none");
        }

        $("#pnlDashboard #pnlUserMessagesGrid #dgvDirectMessagesOutGrid").dataTable().fnDestroy();
        $("#pnlDashboard #pnlUserMessagesGrid #dgvDirectMessagesOutGrid tbody").find("tr").remove();
        if (response.MessageCount > 0) {
            var MessageSearchJSONData = JSON.parse(response.DirectMessageLoad_JSON);
            $.each(MessageSearchJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", item.ID);
                if (item.isXml == "True" && item.isHtml == "True") {
                    var xmlOrHtml = "HTML, XML";
                } else if (item.isXml == "False" && item.isHtml == "True") {
                    var xmlOrHtml = "HTML";
                } else if (item.isXml == "True" && item.isHtml == "False") {
                    var xmlOrHtml = "XML";
                } else {
                    var xmlOrHtml = "";
                }
                var isdelivered = '';
                if (item.isdelivered === "True") {
                    isdelivered = 'checked="checked" ';
                }
                $row.append('<td><input type="checkbox" name="dchkbx-' + item.ID + '" id="dchkbx-' + item.ID + '" title="" ' + isdelivered + '/></td><td>' + item.EmailFrom + '</td><td>' + item.EmailTo + '</td><td>' + item.DateTime + '</td><td>' + item.MessageStatus + '</td><td>' + item.AccountNumber + '</td><td>' + item.DocType + '</td><td>' + xmlOrHtml + '</td><td>' + item.DOS + '</td>');

                $("#pnlDashboard #dgvDirectMessagesOutGrid tbody").last().append($row);
            });



            $("#pnlDashboard #dgvDirectMessagesOutGrid tbody input[type=checkbox][name^=dchkbx]").click(function (event) {

                if (event.target.checked) {
                    VarArrayDelivered.push(event.target.name.split('-')[1]);
                }

                else {
                    var i = VarArrayDelivered.indexOf(event.target.name.split('-')[1]);
                    if (i != -1) {
                        VarArrayDelivered.splice(i, 1);

                    }

                }

                //DashBoard.DashBoardDirectMessagesChangeStatus(event);
            });
        }
        else {
            $("#pnlDashboard #divDirectMessagesOutPaging").css("display", "none");
            $('#pnlDashboard #dgvDirectMessagesOutGrid').DataTable({
                "language": {
                    "emptyTable": "No Message Found"
                    //}, "autoWidth": false, "pageLength": 5, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false }]
                }, "searching": false, "autoWidth": false, "pageLength": 5, "bLengthChange": false, "order": [], "bPaginate": false, "bInfo": false, "aoColumnDefs": [{
                    "bSortable": false, "aTargets": [0]
                }]
            });
        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({
            container: 'body'
        });
        if ($.fn.dataTable.isDataTable('#pnlDashboard #dgvDirectMessagesOutGrid'))
            ;
        else
            //$("#pnlDashboard #dgvUserMessagesGrid").DataTable({ "bInfo": false, "bPaginate": false, "pageLength": 5, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
            $("#pnlDashboard #dgvDirectMessagesOutGrid").DataTable({
                "searching": false, "bInfo": false, "bPaginate": false, "pageLength": 5, "bLengthChange": false, "autoWidth": false, "order": [], "aoColumnDefs": [{
                    "bSortable": false, "aTargets": [0]
                }]
            }); // to remove records per page dropdown
        //  $('.table-responsive').css('min-height', '220px');
    },



    LoadOutgoinDirectMessages: function (PageNumber, RowsPerPage) {

        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 15 : RowsPerPage;


        var objData = new Object();
        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        objData["CommandType"] = "Load_Outgoing_Direct_Messages";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "LoadDashBoard");
    },
    DashBoardAppRequestReferesh: function (event) {
        if (event != null) {
            event.stopPropagation();
        }
        DashBoard.DashBoardAppRequest_DBCall().done(function (response) {
            if (response.status != false) {

                DashBoard.DashBoardAppRequestGridLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);

            }
        });
    },

    BindPatientNameAutocomplete: function () {
        var AccountNo = $("#pnlDashboard #pnlPatientDocumentGrid #txtFullName").val();
        utility.Keyupdelay(function () {
            var AllPatients = utility.GetPatientArrayByName(AccountNo, 1).done(function (response) {
                $.each(response, function (i, item) {
                    item.value = item.FullName;
                });

                $("#pnlDashboard #pnlPatientDocumentGrid #txtFullName").autocomplete({
                    //source: AllPatients, // pass an array (without a comma)
                    autoFocus: true,
                    source: response,
                    open: function (event, ui) {
                        disable = true
                    },
                    close: function (event, ui) {
                        disable = false; $(this).focus();
                    },
                    select: function (event, ui) {
                        setTimeout(function () {
                            $("#pnlDashboard #pnlPatientDocumentGrid #txtFullName").val(ui.item.FullName);
                            $("#pnlDashboard #pnlPatientDocumentGrid #hfPatDocPatientId").val(ui.item.id);
                            //  appointmentDetail.FillPatientAccount(ui.item.id);
                            utility.InsertRecentPatient(ui.item.id);
                        }, 100);
                    }
                }).blur(function () {

                    setTimeout(function () {
                        utility.ValidateAutoCompletePatientName($("#pnlDashboard #pnlPatientDocumentGrid #txtFullName"), "pnlPatientDocumentGrid #hfPatDocPatientId", false, 0, null, null);
                    }, 200);

                });


                $("#pnlDashboard #pnlPatientDocumentGrid #txtFullName").autocomplete("search");
                $("#pnlDashboard #pnlPatientDocumentGrid #txtFullName").focus();

            });
        });


    },
    SelectDocument: function (event, obj) {
        if (event != null) {
            event.stopPropagation();
        }
    },
    checkUncheckAll: function (obj) {
        if ($(obj).is(":checked")) {
            $("#pnlDashboard #pnlPatientDocumentGrid #dgvPatientDocumentGrid input[id*='chkPatDoc']").each(function (k, v) {
                $(this).prop('checked', true);
            });
        }
        else {
            $("#pnlDashboard #pnlPatientDocumentGrid #dgvPatientDocumentGrid input[id*='chkPatDoc']").each(function (k, v) {
                $(this).prop('checked', false);
            });
        }
    },
    DownloadSelectedDocument: function () {

        var PatDocIds = $("#pnlDashboard #pnlPatientDocumentGrid #dgvPatientDocumentGrid input[id*='chkPatDoc']:checked").map(function () {
            return this.id.replace("chkPatDoc", "");
        }).get().join(',');

        if (PatDocIds == "") {
            utility.DisplayMessages("Please select any document to download.", 4);
            return false;
        }
        else {
            DashBoard.ExportDocuments(PatDocIds);

        }
    },
    ExportDocuments: function (PatDocIds) {
        Document_Export.DownloadDocuments(null, PatDocIds).done(function (response) {

            if (response.status) {
                var files = JSON.parse(response.Documents);
                var zip = new JSZip();
                for (var fileIndex = 0; fileIndex < files.length; fileIndex++) {
                    if (files[fileIndex].Base64FileStream && files[fileIndex].Base64FileStream != "") {
                        if (!zip.files[files[fileIndex].FilePath])
                            zip.file(files[fileIndex].FilePath, files[fileIndex].Base64FileStream, {
                                base64: true
                            });
                        else
                            zip.file("(" + fileIndex + ")" + files[fileIndex].FilePath, files[fileIndex].Base64FileStream, {
                                base64: true
                            });

                    }

                }
                setTimeout(function () {
                    var content = zip.generateAsync({ type: "blob" })
                                         .then(function (content) {
                                             saveAs(content, "DashboardDocuments" + ".zip");

                                         });
                }, 20);

                utility.DisplayMessages("Downloaded Successfully", 1);
            } else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    DeleteSelectedDocument: function () {
        var PatDocIds = $("#pnlDashboard #pnlPatientDocumentGrid #dgvPatientDocumentGrid input[id*='chkPatDoc']:checked").map(function () {
            return this.id.replace("chkPatDoc", "");
        }).get().join(',');

        if (PatDocIds == "") {
            utility.DisplayMessages("Please select any document to delete.", 4);
            return false;
        }
        else {
            DashBoard.DocumentDelete(PatDocIds);

        }
    },
    DocumentDelete: function (DocumentId, event, IsFromAttach, ChildPatientDocId) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Documents", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = DocumentId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {

                        var $def = $.Deferred();

                        if (IsFromAttach == true) {

                            Patient_Document.DetachDocumentToNote_DBCall(selectedValue).done(function (result) {
                                if (result.status != false) {
                                    $def.resolve("ok");
                                }
                                else {
                                    $def.resolve(result.Message);
                                }
                            });
                        }
                        else {
                            $def.resolve("ok");
                        }

                        $def.then(function (res_message) {
                            if (res_message == "ok") {
                                Patient_Document.DeleteDocument(selectedValue, ChildPatientDocId).done(function (response) {
                                    if (response.status != false) {
                                        // Quick link Document count set                                       
                                        Document_AssignedTo.PendingCountSearchDoc();
                                        utility.DisplayMessages(response.Message, 1);
                                        var pageNo = $("#pnlDashboard #divPendingPagingGrid li.active a").text();
                                        DashBoard.DashBoardDocumentSearch(pageNo, 15);
                                    }
                                    else {
                                        utility.DisplayMessages(response.Message, 3);
                                    }
                                });
                            }
                            else {

                                utility.DisplayMessages(res_message, 3);
                            }

                        });
                    }
                }, function () {
                },
                    '1'
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    SignAndReview: function () {
        var PatDocIds = $("#pnlDashboard #pnlPatientDocumentGrid #dgvPatientDocumentGrid input[id*='chkPatDoc']:checked").map(function () {
            return this.id.replace("chkPatDoc", "");
        }).get().join(',');

        if (PatDocIds == "") {
            utility.DisplayMessages("Please select any document to Review & Sign.", 4);
            return false;
        }
        else {
            AppPrivileges.GetFormPrivileges("Documents", "REVIEW AND SIGN", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Patient_Document.ReviewedAndSignedDocument(PatDocIds, 0, null).done(function (response) {
                        if (response.status != false) {
                            // Quick link Document count set                           
                            Document_AssignedTo.PendingCountSearchDoc();
                            utility.DisplayMessages(response.message, 1);
                            var pageNo = $("#pnlDashboard #divPendingPagingGrid li.active a").text();
                            DashBoard.DashBoardDocumentSearch(pageNo, 15);
                        }
                        else {
                            utility.DisplayMessages(response.message, 2);
                        }
                    });
                } else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
    },
    printDocument: function () {
        var PatDocIds = $("#pnlDashboard #pnlPatientDocumentGrid #dgvPatientDocumentGrid input[id*='chkPatDoc']:checked").map(function () {
            return this.id.replace("chkPatDoc", "");
        }).get().join(',');

        if (PatDocIds == "") {
            utility.DisplayMessages(" Please select any document to print.", 4);
            return false;
        }

        Patient_Document.FillDocumentsForFax(null, "", PatDocIds).done(function (response) {
            if (response.status != false) {
                var params = [];
                params["PDFBase64"] = response.MergedContent;
                var blobitem = DashBoard.getblob(response.MergedContent, 'application/pdf');
                var pdfObjectUrl = URL.createObjectURL(blobitem);
                var ifrm = document.createElement("iframe");
                ifrm.className = "DashboardFrame";
                ifrm.setAttribute("src", pdfObjectUrl);
                ifrm.style.minWidth = "640px";
                ifrm.style.minHeight = "480px";
                ifrm.width = "100%";

                document.body.appendChild(ifrm);
                ifrm.contentWindow.print();
                if ($(".DashboardFrame").length > 1) {
                    $(".DashboardFrame:not(:last)").each(function (k, v) {
                        $(this).remove();
                    });
                }
                $(ifrm).hide();


            }
            else {
                utility.DisplayMessages(response.Message, 4);
            }
        });
    },
    IntializeMultiSelectDropDown: function () {
        var checkInValue = "";
        if (globalAppdata.PreferredAppointmentStatus == "1") {
            checkInValue = $('#pnlDashboard #pnlSchAppointmentGrid #ddlAppointmentStatus option:contains(Check In)').attr("value");
        }
        else {
            checkInValue = $('#pnlDashboard #ddlAppointmentStatus option').map(function () {
                return this.value;
            }).get().join(',');
        }

        $('#pnlDashboard #pnlSchAppointmentGrid #ddlAppointmentStatus').val(checkInValue.split(','));
        $('#pnlDashboard #pnlSchAppointmentGrid #ddlAppointmentStatus').multiselect('destroy');
        // PRD-14
        //$('#pnlDashboard #pnlSchAppointmentGrid #ddlAppointmentStatus').multiselect("refresh");
        $('#pnlDashboard #pnlSchAppointmentGrid #ddlAppointmentStatus').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247

        });
        $('#pnlDashboard #pnlSchAppointmentGrid #ddlAppointmentStatus').multiselect('rebuild');
        //for positioning
        $('#pnlDashboard #pnlSchAppointmentGrid #ddlAppointmentStatus').parent().find('button.dropdown-toggle').parent().addClass('position-r');
        $('#pnlDashboard #pnlSchAppointmentGrid #ddlAppointmentStatus').parent().find('button.dropdown-toggle').parent().find('ul').addClass('size100per')

    },
    getblob: function (b64Data, contentType) {
        var byteCharacters = atob(b64Data)

        var byteArrays = []

        for (var offset = 0; offset < byteCharacters.length; offset += 512) {
            var slice = byteCharacters.slice(offset, offset + 512),
                byteNumbers = new Array(slice.length)
            for (var i = 0; i < slice.length; i++) {
                byteNumbers[i] = slice.charCodeAt(i)
            }
            var byteArray = new Uint8Array(byteNumbers)

            byteArrays.push(byteArray)
        }

        var blob = new Blob(byteArrays, {
            type: contentType
        })
        return blob
    },
    DeleteWidget: function (DBSId) {
        var allSettting = new Array();
        var objSettings = new Object();
        objSettings.DBSId = DBSId;
        objSettings.isActive = 0;
        allSettting.push(objSettings);
        var strJSON = JSON.stringify(allSettting);

        DashBoardSetting.UpdateAllDashBoardSetting(strJSON).done(function (response) {
            if (response != false) {
                if (!utility.CheckPageLoaded("mdvisionadmin.aspx")) {
                    $('.slide-ul').unslick();
                    $('#pnlDashboard #kpipanel').empty();
                    $('#pnlDashboard #widgetpanel').empty();
                    DashBoard.LoadDashBoardSetting(0);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    ResetLiveRequestDiv: function (obj, FromTabs, FromButtons) {
        if (FromTabs == true) {
            $("#LiveReuestActiveDiv").empty();
            if ($(obj).parent().prop('id') == "liPatientPortal") {
                $("#liPatientPortal").addClass("active");
                $("#liCheckInPortal").removeClass("active");
                $("#liCheckInApp").removeClass("active");
                $("#divPatientPortalRequestGrid").show();
                $("#divCheckInAppRequestGrid").hide();
            } else if ($(obj).parent().prop('id') == "liCheckInPortal") {
                $("#liPatientPortal").removeClass("active");
                $("#liCheckInPortal").addClass("active");
                $("#liCheckInApp").removeClass("active");
            } else if ($(obj).parent().prop('id') == "liCheckInApp") {
                $("#liPatientPortal").removeClass("active");
                $("#liCheckInPortal").removeClass("active");
                $("#liCheckInApp").addClass("active");
                $("#divPatientPortalRequestGrid").hide();
                $("#divCheckInAppRequestGrid").show();
            }
        }
        if (FromButtons) {
            $("#LiveReuestActiveDiv").empty();
        }
    },
    ToFromDateChange: function (obj, CtrlFromDate, CtrlToDate) {
        if ($(CtrlFromDate).val() != '' && $(CtrlToDate).val() != '') {
            var fromDate = new Date($(CtrlFromDate).val());
            var toDate = new Date($(CtrlToDate).val());

            if (fromDate <= toDate && fromDate != '') {
                $(CtrlToDate).val($(CtrlToDate).val()).datepicker('update');
            } else {
                $(obj).val('');
                utility.DisplayMessages("From date is greater than To date", 3);
            }
        }

        $(CtrlToDate).prop('disabled', false);
    },
}

