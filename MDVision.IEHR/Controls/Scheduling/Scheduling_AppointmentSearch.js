Scheduling_AppointmentSearch = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Scheduling_AppointmentSearch.params = params;
        if (Scheduling_AppointmentSearch.params != null && Scheduling_AppointmentSearch.params.PanelID != "Scheduling_AppointmentSearch") {
            Scheduling_AppointmentSearch.params["PanelID"] = Scheduling_AppointmentSearch.params["PanelID"] + ' #Scheduling_AppointmentSearch';
        }
        else {
            Scheduling_AppointmentSearch.params = [];
            Scheduling_AppointmentSearch.params["PanelID"] = "Scheduling_AppointmentSearch"
        }

        if (Scheduling_AppointmentSearch.bIsFirstLoad) {
            Scheduling_AppointmentSearch.bIsFirstLoad = false;
            var self = $('#Scheduling_AppointmentSearch');
            self.loadDropDowns(true).done(function () {
                Scheduling_AppointmentSearch.IntializeMultiSelectDropDown();
                Scheduling_AppointmentSearch.AppointmentSearch();
            });
        }
        //setTimeout(function () {
        //    Scheduling_AppointmentSearch.AppendVisitStatuses();
        //}, 2000);
        if (Scheduling_AppointmentSearch.params.ParentCtrl == "clinicalTabFaceSheet") {
            EMRUtility.MakeFaceSheetPager('#' + Scheduling_AppointmentSearch.params.PanelID + " div#FaceSheetPager", Scheduling_AppointmentSearch.params.FaceSheetComponents, 'appointments');
        }
        Scheduling_AppointmentSearch.documentReady();
    },

    // Grid Load And Search Functions

    AppointmentSearch: function (myJSON, PageNo, rpp) {
        if ($("#Scheduling_AppointmentSearch #pnlAppointment_Result").css("display") == "none")
            $("#Scheduling_AppointmentSearch #pnlAppointment_Result").show();

        var self = $("#Scheduling_AppointmentSearch");
        var myJSON = self.getMyJSON();

        Scheduling_AppointmentSearch.SearchAppointments(myJSON, PageNo, rpp).done(function (response) {
            if (response.status != false) {
                Scheduling_AppointmentSearch.AppointmentGridLoad(response);

                // Start 26/11/2015 Muhammad Irfan for Bug # EMR-25 

                var TableControl = "Scheduling_AppointmentSearch #dgvAppointmentSearch";
                var PagingPanelControlID = "Scheduling_AppointmentSearch #divAppointmentSearchPaging";
                var ClassControlName = "Scheduling_AppointmentSearch";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.SchAppStatusCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (myJSON, PageNumber, ResultPerPage) {
                    Scheduling_AppointmentSearch.AppointmentSearch(myJSON, PageNumber, ResultPerPage);
                }), 10);

                // End 26/11/2015 Muhammad Irfan for Bug # EMR-25 

            }
            else {
                utility.DisplayMessages(response.Message, 3);

            }
        });
    },

    AppointmentGridLoad: function (response) {
        var gridControl = $('#Scheduling_AppointmentSearch #pnlAppointment_Result #gridControl').html();
        var appDate = $('#Scheduling_AppointmentSearch #pnlAppointment_Result #appDate').text();
        $('#Scheduling_AppointmentSearch #pnlAppointment_Result #gridControl').remove();

        $("#Scheduling_AppointmentSearch #dgvAppointmentSearch").dataTable().fnDestroy();
        $("#Scheduling_AppointmentSearch #dgvAppointmentSearch tbody").find("tr").remove();
        if (response.SchAppStatusCount > 0) {
           // $('#Appointments #spnAppCount').text(response.iTotalDisplayRecords);
            var AppointmentSearchJSONData = JSON.parse(response.SchAppStatus_JSON);
            $.each(AppointmentSearchJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", "gvDashBoard_Appointments_row" + i + "");
                $row.attr("onclick", "utility.SelectGridRow($('#gvDashBoard_Appointments_row" + i + "'))");

                //--- Start Action Column Checks on Visit Status base


                var DemographicsMethod = "Scheduling_AppointmentSearch.PatientDemographics('" + item.PatientId + "');";

                var Action = "";
                var Room = "";

                //var Wait = "";

                //if (item.minsWait != "")
                //    Wait = item.minsWait + '(min)';


                //if (item.AppointmentStatus.toUpperCase() == "CHECKIN" && (item.NotesId == null || item.NotesId == '' || item.NotesId == '0' || item.NotesId == 0)) {
                //    //<a href="#" onclick="' + DemographicsMethod + '"  title="View Patient"> </a>
                //    var Method = "Scheduling_AppointmentSearch.CreateNote('" + item.PatientId + "',\'" + item.AppointmentId + "\',\'" + item.ProviderId + "\',\'" + item.ProviderName + "\',\'" + item.AppointmentTime + "\',\'" + item.VisitId + "\',\'" + utility.RemoveTimeFromDate(null, item.VisitDate) + "\',\'" + item.Reason + "\',\'" + item.FacilityName + "\',\'" + item.FacilityId + "\',\'" + item.Room + "\',\'" + item.NotesId + "\');";
                //    Action = '<a href="#" onclick="' + Method + '"  title="Create Note">Create Note</a>';
                //    Room = "Lobby";
                //}
                //else if ((item.NotesId != null || item.NotesId != '' || item.NotesId > 0) && item.NotesId != '0' && item.NoteStatus != 'Signed' && item.AppointmentStatus.toUpperCase() != "CHECKOUT" && (item.BillingInfoId == null || item.BillingInfoId == '0')) {
                //    //<a href="#" onclick="' + DemographicsMethod + '"  title="View Patient"> </a>
                //    var Method = "Scheduling_AppointmentSearch.EditNote('" + item.PatientId + "',\'" + item.AppointmentId + "\',\'" + item.ProviderId + "\',\'" + item.ProviderName + "\',\'" + item.AppointmentTime + "\',\'" + item.VisitId + "\',\'" + utility.RemoveTimeFromDate(null, item.VisitDate) + "\',\'" + item.Reason + "\',\'" + item.FacilityName + "\',\'" + item.FacilityId + "\',\'" + item.Room + "\',\'" + item.NotesId + "\');";
                //    Action = '<a href="#" onclick="' + Method + '"  title="Edit Note">Edit Note</a>';
                //} else if ((item.NoteStatus == 'Signed' || parseInt(item.BillingInfoId) > 0) && item.BillingStatus != 'Signed') {
                //    var Method = "return false;";//DashBoard.EditNote('" + item.PatientId + "',\'" + item.AppointmentId + "\',\'" + item.ProviderId + "\',\'" + item.ProviderName + "\',\'" + item.AppointmentTime + "\',\'" + item.VisitId + "\',\'" + utility.RemoveTimeFromDate(null, item.VisitDate) + "\',\'" + item.Reason + "\',\'" + item.FacilityName + "\',\'" + item.FacilityId + "\',\'" + item.Room + "\',\'" + item.NotesId + "\');";
                //    //Action = '<a href="javascript:void(0);" onclick="' + Method + '"  title="Create eSuperbill">Create eSuperbill</a>';

                //    var Method = "Scheduling_AppointmentSearch.CreateNoteSuperbill('" + item.PatientId + "',\'" + item.AppointmentId + "\',\'" + item.ProviderId + "\',\'" + item.ProviderName + "\',\'" + item.AppointmentTime + "\',\'" + item.VisitId + "\',\'" + utility.RemoveTimeFromDate(null, item.VisitDate) + "\',\'" + item.Reason + "\',\'" + item.FacilityName + "\',\'" + item.FacilityId + "\',\'" + item.Room + "\',\'" + item.NotesId + "\',\'" + item.PatientType + "\',\'" + item.NoteDate + "\',\'" + item.BillingInfoId + "\');";
                //    var superbillTitle = "Create eSuperbill";
                //    if (item.BillingInfoId != 0) {
                //        superbillTitle = "Edit eSuperbill";
                //    }
                //    Action = '<a href="javascript:void(0);" onclick="' + Method + '"  title="' + superbillTitle + '">' + superbillTitle + '</a>';

                //}
                //else if (item.BillingStatus == 'Signed') {

                //    var Method = "return false;";//DashBoard.EditNote('" + item.PatientId + "',\'" + item.AppointmentId + "\',\'" + item.ProviderId + "\',\'" + item.ProviderName + "\',\'" + item.AppointmentTime + "\',\'" + item.VisitId + "\',\'" + utility.RemoveTimeFromDate(null, item.VisitDate) + "\',\'" + item.Reason + "\',\'" + item.FacilityName + "\',\'" + item.FacilityId + "\',\'" + item.Room + "\',\'" + item.NotesId + "\');";
                //    var Method = "Scheduling_AppointmentSearch.LoadVisitDetail(\'" + item.VisitId + "\','" + item.PatientId + "');";
                //    var superbillTitle = "View Charges";

                //    Action = '<a href="javascript:void(0);" onclick="' + Method + '"  title="' + superbillTitle + '">' + superbillTitle + '</a>';
                //}

                if (Room == "") {
                    Room = item.Room;
                }
                $row.append('<td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.PatientName + '</a></td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.AccountNumber + '</a></td><td>' + item.ProviderName + '</td><td>' + item.PatientType + '</td><td>' + item.VisitType + '</td><td>' + item.Reason + '</td><td>' + item.AppointmentTime + '</td><td class="size-max120 ellipses" data-toggle="tooltip" data-placement="left" title="' + item.Duration + '(min)">' + item.Duration + '(min)</td><td>' + item.VisitTime + '</td><td>' + item.AppointmentStatus + '</td>');

                $("#Scheduling_AppointmentSearch #dgvAppointmentSearch tbody").last().append($row);
            });
        }
        else {
            $("#Scheduling_AppointmentSearch #divAppointmentSearchPaging").css("display", "none");
            $('#Scheduling_AppointmentSearch #dgvAppointmentSearch').DataTable({
                "language": {
                    "emptyTable": "No Schedule Found"
                }, "searching": false, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [9] }]
            });
        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });

        if ($.fn.dataTable.isDataTable('#Scheduling_AppointmentSearch #dgvAppointmentSearch'))
            ;
        else
            $("#Scheduling_AppointmentSearch #dgvAppointmentSearch").DataTable({ "searching": false, "bInfo": false, "bPaginate": false, "aaSorting": [], "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [9] }] }); // to remove records per page dropdown
        // $('.table-responsive').css('min-height', '220px');

        $('#Scheduling_AppointmentSearch #dgvAppointmentSearch_wrapper .datatables-header div.dataTables_filter').parent().removeClass('col-sm-12 col-md-6');
        if (!$('#Scheduling_AppointmentSearch #dgvAppointmentSearch_wrapper .datatables-header div.dataTables_filter').parent().hasClass('col-sm-6 col-md-6 pull-right')) {
            $('#Scheduling_AppointmentSearch #dgvAppointmentSearch_wrapper .datatables-header div.dataTables_filter').parent().addClass('col-sm-6 col-md-6 pull-right');
        }
        //  $('#Scheduling_AppointmentSearch #dgvAppointmentSearch_wrapper  .datatables-header div:first').remove();

        $('#Scheduling_AppointmentSearch #dgvAppointmentSearch_wrapper .datatables-header div:first').html(' <div id="gridControl">' + gridControl + '</div>');
        $('#Scheduling_AppointmentSearch #dgvAppointmentSearch_wrapper .datatables-header div:first').removeClass('col-sm-12 col-md-6');
        if (!$('#Scheduling_AppointmentSearch #dgvAppointmentSearch_wrapper .datatables-header div:first').hasClass('col-sm-6 col-md-6 text-right')) {
            $('#Scheduling_AppointmentSearch #dgvAppointmentSearch_wrapper .datatables-header div:first').addClass('col-sm-6 col-md-6 text-right');
        }

        $('#Scheduling_AppointmentSearch #pnlAppointment_Result #appDate').text(appDate);
        Scheduling_AppointmentSearch.documentReady();
        //$('#Scheduling_AppointmentSearch #pnlAppointment_Result #appDate').datepicker('setDate', new Date($('#Scheduling_AppointmentSearch #pnlAppointment_Result #appDate').text()))

    },

    NotesPreview: function (NotesId, PatientId, ProviderId) {
        var params = [];
        params["FromAdmin"] = "0";
        params["NotesId"] = NotesId;
        params["PatientId"] = PatientId;
        params["ProviderId"] = ProviderId;
        params["ParentCtrl"] = 'Scheduling_AppointmentSearch';
        LoadActionPan('Clinical_NotesView', params);
    },


    documentReady: function () {
        var today = new Date();
        if ($('#Scheduling_AppointmentSearch #pnlAppointment_Result #appDate').text() == "") {
            var cur = $.datepicker.formatDate('mm/dd/yy', new Date(today));
            $('#Scheduling_AppointmentSearch #pnlAppointment_Result #appDate').text(cur);
        }
        $('#Scheduling_AppointmentSearch #pnlAppointment_Result #appDate').text(cur);
        $('#Scheduling_AppointmentSearch #pnlAppointment_Result #appDate').datepicker({
            format: "mm/dd/yy",
            todayHighlight: true
        }).on('changeDate', function (e) {
            $(this).datepicker('hide');
            var newDate = $.datepicker.formatDate('mm/dd/yy', new Date(e.date));
            if (newDate != $('#Scheduling_AppointmentSearch #pnlAppointment_Result #appDate').text()) {
                $('#Scheduling_AppointmentSearch #pnlAppointment_Result #appDate').text(newDate);
                Scheduling_AppointmentSearch.AppointmentSearch();
            }
            //alert('called')
        });
    },
    SearchAppointments: function (AppointmentData, PageNumber, RowsPerPage) {

        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var AppointmentStatusIds = $('#' + Scheduling_AppointmentSearch.params.PanelID + ' #ddlAppointmentStatus option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        var calDate = $('#Scheduling_AppointmentSearch #pnlAppointment_Result #appDate').text();
        // Start 02/12/2015 Muhammad Irfan Bug # 64
        var newDatee = Scheduling_AppointmentSearch.FormatDate(calDate);
        //var cur1 = $.datepicker.formatDate('yy/mm/dd', new Date(calDate));
        var AppDate = newDatee;

        var data = "AppointmentData=" + AppointmentData + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage + "&AppDate=" + AppDate + "&AppointmentStatusIds=" + AppointmentStatusIds;;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "SCHEDULING_APPOINTMENT_SEARCH", "SEARCH_APPOINTMENTS");
    },
    //-------------------------------

    UnLoad: function () {
        var objDeffered = $.Deferred();
        /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        if (Scheduling_AppointmentSearch.params.ParentCtrl == "clinicalTabFaceSheet") {
            if (Scheduling_AppointmentSearch.params["FromAdmin"] == "0") {
                if (Scheduling_AppointmentSearch.params != null && Scheduling_AppointmentSearch.params.ParentCtrl != null) {
                    UnloadActionPan(Scheduling_AppointmentSearch.params.ParentCtrl, 'Scheduling_AppointmentSearch');
                }
                else
                    UnloadActionPan(null, 'Scheduling_AppointmentSearch');
            }
            else {
                RemoveAdminTab();
            }
            objDeffered.resolve();
        }
            /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        else {
            if (Scheduling_AppointmentSearch.params != null && Scheduling_AppointmentSearch.params.ParentCtrl) {
                UnloadActionPan(Scheduling_AppointmentSearch.params.ParentCtrl);
                Scheduling_AppointmentSearch.params = null;
            }
            else {
                UnloadActionPan();
            }
            objDeffered.resolve();
        }

        return objDeffered;
    },

    NextDate: function () {
        var criteria = $('#Scheduling_AppointmentSearch #appDate').text();
        // Start 02/12/2015 Muhammad Irfan Bug # 64
        var d = new Date(Scheduling_AppointmentSearch.FormatDate(criteria));
        var newdate = Scheduling_AppointmentSearch.AddSubDays(d, 1);
        var curdte = $.datepicker.formatDate('mm/dd/yy', newdate);

        $('#Scheduling_AppointmentSearch #appDate').text(curdte);
        Scheduling_AppointmentSearch.AppointmentSearch();
    },

    BackDate: function () {

        var criteria = $('#Scheduling_AppointmentSearch #appDate').text();
        // Start 02/12/2015 Muhammad Irfan Bug # 64
        var d = new Date(Scheduling_AppointmentSearch.FormatDate(criteria));
        var newdate = Scheduling_AppointmentSearch.AddSubDays(d, -1);
        var curdte = $.datepicker.formatDate('mm/dd/yy', newdate);

        $('#Scheduling_AppointmentSearch #appDate').text(curdte);
        Scheduling_AppointmentSearch.AppointmentSearch();
    },

    AddSubDays: function (theDate, days) {

        return new Date(theDate.getTime() + days * 24 * 60 * 60 * 1000);

    },

    CreateNote: function (PatientId, AppointmentId, ProviderId, ProviderName, AppointmentTime, VisitId, VisitDate, Reason, FacilityName, FacilityId, Room, NotesId) {
        AppPrivileges.GetFormPrivileges("Notes_Notes", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Scheduling_AppointmentSearch.UnLoad();
                params["QuickAddPatient"] = true;
                var ParentCntrlLoadid = "Dashboard";
                var ForProgressNote = false;
                var RefProviderId = '';
                var RefProviderName = '';

                EMRUtility.CreateNote("Add", PatientId, AppointmentId, ProviderId, ProviderName, AppointmentTime, VisitId, VisitDate, Reason, FacilityName,
                        FacilityId, Room, NotesId, ForProgressNote, ParentCntrlLoadid, RefProviderName, RefProviderId);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    EditNote: function (PatientId, AppointmentId, ProviderId, ProviderName, AppointmentTime, VisitId, VisitDate, Reason, FacilityName, FacilityId, Room, NotesId) {
        AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Scheduling_AppointmentSearch.UnLoad();
                params["QuickAddPatient"] = true;

                var ParentCntrlLoadid = "Dashboard";
                var ForProgressNote = false;
                var RefProviderName = '';
                var RefProviderId = '-1';
                DashBoard.EditProgressNote(NotesId, PatientId);
                //EMRUtility.CreateNote("Add", PatientId, AppointmentId, ProviderId, ProviderName, AppointmentTime, VisitId, VisitDate, Reason, FacilityName,
                //        FacilityId, Room, NotesId, ForProgressNote, ParentCntrlLoadid, RefProviderName, RefProviderId, ParentCntrlLoadid);
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    PatientDemographics: function (patientid) {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Demographic", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                var params = [];
                params["mode"] = 'Edit';
                params["PatBanner"] = true;
                params["patientID"] = patientid;
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = "Scheduling_AppointmentSearch";
                LoadActionPan('demographicDetail', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    // Start 02/12/2015 Muhammad Irfan Bug # 64
    FormatDate: function (date) {
        var obj = { January: 1, February: 2, March: 3, April: 4, May: 5, June: 6, July: 7, August: 8, September: 9, October: 10, November: 11, December: 12 };
        var temp = date.split('/');
        return temp[2] + "/" + temp[0] + "/" + temp[1];
    },
    // End 02/12/2015 Muhammad Irfan Bug # 64

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
        params["ParentCtrl"] = 'Scheduling_AppointmentSearch';
        params["ProviderId"] = ProviderId;
        params["PatientId"] = PatientId;
        params["NotesId"] = parseInt(NotesId);
        params["VisitId"] = VisitId;
        params["VisitDate"] = VisitDate;
        params["BillingInfoId"] = parseInt(billInfoId);
        params["BtnClicked"] = null;
        params["appId"] = AppointmentId;
        params["AppointmentDate"] = utility.formatDate(Date($('#Scheduling_AppointmentSearch #pnlAppointment_Result #appDate').text()));
        params["PatientType"] = null;
        params["NoteDate"] = utility.RemoveTimeFromDate(null, NoteDate);
        LoadActionPan('BillingInformation', params);
    },

    LoadVisitDetail: function (VisitId, PatientId) {

        AppPrivileges.GetFormPrivileges("Charges", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FromAdmin"] = 0;
                params["ParentCtrl"] = 'Scheduling_AppointmentSearch';

                params["VisitId"] = VisitId;
                params["patientID"] = PatientId;

                LoadActionPan('EncounterChargeCapture', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    IntializeMultiSelectDropDown: function () {
        var checkInValue = "";
        if (globalAppdata.PreferredAppointmentStatus == "1") {
            checkInValue = $('#' + Scheduling_AppointmentSearch.params.PanelID + ' #ddlAppointmentStatus option:contains(Check In)').attr("value");
        }
        else {
            checkInValue = $('#' + Scheduling_AppointmentSearch.params.PanelID + ' #ddlAppointmentStatus option').map(function () {
                return this.value;
            }).get().join(',');
        }

        $('#' + Scheduling_AppointmentSearch.params.PanelID + ' #ddlAppointmentStatus').val(checkInValue.split(','));
        $('#' + Scheduling_AppointmentSearch.params.PanelID + ' #ddlAppointmentStatus').multiselect('destroy');
        // PRD-14
        //$('#pnlDashboard #pnlSchAppointmentGrid #ddlAppointmentStatus').multiselect("refresh");
        $('#' + Scheduling_AppointmentSearch.params.PanelID + ' #ddlAppointmentStatus').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247

        });
        $('#' + Scheduling_AppointmentSearch.params.PanelID + ' #ddlAppointmentStatus').multiselect('rebuild');
        //for positioning
        $('#' + Scheduling_AppointmentSearch.params.PanelID + ' #ddlAppointmentStatus').parent().find('button.dropdown-toggle').parent().addClass('position-r');
        $('#' + Scheduling_AppointmentSearch.params.PanelID + ' #ddlAppointmentStatus').parent().find('button.dropdown-toggle').parent().find('ul').addClass('size100per')

    }
}
