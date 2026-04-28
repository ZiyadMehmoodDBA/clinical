/*
Author: Muhammad Irfan
Date: 21/12/2015
Overview: This file is created to show appointments in facesheet
*/

clinicalFaceSheetAppointments = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        clinicalFaceSheetAppointments.params = params;

        if (clinicalFaceSheetAppointments.params.PanelID != 'clinicalFaceSheetAppointments') {
            clinicalFaceSheetAppointments.params.PanelID = clinicalFaceSheetAppointments.params.PanelID + ' #clinicalFaceSheetAppointments';
        } else {
            clinicalFaceSheetAppointments.params.PanelID = 'clinicalFaceSheetAppointments';
        }
        if (clinicalFaceSheetAppointments.params.ParentCtrl == "clinicalTabFaceSheet") {
            EMRUtility.MakeFaceSheetPager('#' + clinicalFaceSheetAppointments.params.PanelID + " div#FaceSheetPager", clinicalFaceSheetAppointments.params.FaceSheetComponents, 'appointments');
        } else if (clinicalFaceSheetAppointments.params.ParentCtrl == "Clinical_FaceSheet") {
            EMRUtility.MakeFaceSheetPager('#pnlClinicalFaceSheet #clinicalFaceSheetAppointments' + " div#FaceSheetPager", clinicalFaceSheetAppointments.params.FaceSheetComponents, 'appointments');
        }
        clinicalFaceSheetAppointments.appointmentSearch();

        utility.callbackAfterAllDOMLoaded(function () {

            var faceSheetpager = $('#FaceSheetPager');
            if (faceSheetpager.length > 0) {
                //show/hide button controls acording to resoltion
                EMRUtility.HideShowFaceSheetPagerBtnControls(faceSheetpager);
                $("#FaceSheetPager").find("div.slick-track").css("width", "1356px");
            }
        });
    },

    /*
    Author: Muhammad Irfan
    Date: 21/12/2015
    Overview: This function is created to search appointment
    */
    appointmentSearch: function (PageNo, rpp) {

        clinicalFaceSheetAppointments.searchAppointments(PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                clinicalFaceSheetAppointments.appointmentGridLoad(response);

                // Start 26/11/2015 Muhammad Irfan for Bug # EMR-25 

                var TableControl = "clinicalFaceSheetAppointments #dgvClinicalFaceSheetAppointments";
                var PagingPanelControlID = "clinicalFaceSheetAppointments #divFaceSheetAppointmentPaging";
                var ClassControlName = "clinicalFaceSheetAppointments";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.AppointmentsCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (myJSON, PageNumber, ResultPerPage) {
                    clinicalFaceSheetAppointments.appointmentSearch(PageNumber, ResultPerPage);
                }), 10);

                // End 26/11/2015 Muhammad Irfan for Bug # EMR-25 

            }
            else {
                utility.DisplayMessages(response.Message, 3);

            }
        });
    },

    /*
    Author: Muhammad Irfan
    Date: 21/12/2015
    Overview: This function is created to load grid of appointments
    */
    appointmentGridLoad: function (response) {
        $("#" + clinicalFaceSheetAppointments.params.PanelID + " #dgvClinicalFaceSheetAppointments").dataTable().fnDestroy();
        $("#" + clinicalFaceSheetAppointments.params.PanelID + " #dgvClinicalFaceSheetAppointments tbody").find("tr").remove();
        if (response.AppointmentsCount > 0) {
            var AppointmentSearchJSONData = JSON.parse(response.AppointmentsLoad_JSON);
            $.each(AppointmentSearchJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.append('<td>' + utility.RemoveTimeFromDate(null, item.ScheduleDate) + '</td><td>' + item.SchReason + '</td><td>' + item.ProviderName + '</td><td>' + item.Time + '</td><td>' + item.Duration + '</td><td>' + item.AppointmentStatus + '</td>');

                $("#" + clinicalFaceSheetAppointments.params.PanelID + " #dgvClinicalFaceSheetAppointments tbody").last().append($row);
            });
        }
        else {
            $("#" + clinicalFaceSheetAppointments.params.PanelID + " #divFaceSheetAppointmentPaging").css("display", "none");
            $("#" + clinicalFaceSheetAppointments.params.PanelID + " #dgvClinicalFaceSheetAppointments").DataTable({
                "language": {
                    "emptyTable": "No Schedule Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }

        if ($.fn.dataTable.isDataTable("#" + clinicalFaceSheetAppointments.params.PanelID + " #dgvClinicalFaceSheetAppointments"))
            ;
        else
            $("#" + clinicalFaceSheetAppointments.params.PanelID + " #dgvClinicalFaceSheetAppointments").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        // $('.table-responsive').css('min-height', '220px');
    },
    /*
    Author: Muhammad Irfan
    Date: 21/12/2015
    Overview: This function is created to call handler for data
    */
    searchAppointments: function (PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = new Object();
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PatientId"] = clinicalFaceSheetAppointments.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $("div#PatientProfile #hfPatientId").val();
        objData["commandType"] = "Search_FaceSheet_Appointments";
        return MDVisionService.APIService(objData, "FaceSheet", "FaceSheet");

    },

    /*
    Author: Muhammad Irfan
    Date: 21/12/2015
    Overview: This function is created to Unload screen
    */
    UnLoad: function () {
        var parentPanelId = null;
        var objDeffered = $.Deferred();
        if (clinicalFaceSheetAppointments.params.ParentCtrl == "Clinical_FaceSheet") {
            parentPanelId = GetTab(Clinical_FaceSheet.params["ParentCtrl"]).PanelID;
            Clinical_FaceSheet.params.ChildPanelID = null;
            UnloadActionPan(clinicalFaceSheetAppointments.params.ParentCtrl, 'clinicalFaceSheetAppointments', null, parentPanelId);
        } else {
            UnloadActionPan(clinicalFaceSheetAppointments.params.ParentCtrl, 'clinicalFaceSheetAppointments');
        }
        objDeffered.resolve();
        return objDeffered;
        if (clinicalFaceSheetAppointments.params.ParentCtrl == "clinicalTabFaceSheet" || clinicalFaceSheetAppointments.params.ParentCtrl == "Clinical_FaceSheet") {
            Clinical_FaceSheet.loadFaceSheet();
        }
    },
}