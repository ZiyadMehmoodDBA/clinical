Patient_Appointments = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Patient_Appointments.params = params;
        if (Patient_Appointments.params.PanelID != 'pnlPatientAppointments')
            Patient_Appointments.params.PanelID = Patient_Appointments.params.PanelID + ' #pnlPatientAppointments';
        else
            Patient_Appointments.params.PanelID = 'pnlPatientAppointments';
        if ($('#PatientProfile #hfPatientId').val() != "")
            $("#" + Patient_Appointments.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        var self = $('#' + Patient_Appointments.params.PanelID);
        self.loadDropDowns(true).done(function () {
            Patient_Appointments.IntializeAppStatusDDL();
            Patient_Appointments.PatientAppoitmentsSearch();
        });
        Patient_Appointments.BindProvider();
        Patient_Appointments.BindFacility();  
    },
    IntializeAppStatusDDL: function () {
        try {
            $('#' + Patient_Appointments.params.PanelID + ' #ddlAppointmentStatus').multiselect('destroy');
            $('#' + Patient_Appointments.params.PanelID + ' #ddlAppointmentStatus').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                maxHeight: 247,
            });
            $('#' + Patient_Appointments.params.PanelID + " #ddlAppointmentStatus").val("");
        }
        catch (ex) {
            console.log(ex);
        }
    },
    PatientAppoitmentsSearch: function (PrimaryID, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Patient Appointments", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Patient_Appointments.PatientAppointment_DBCall(PrimaryID, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Patient_Appointments.PatientAppointmentsGridLoad(response);
                        var TableControl = Patient_Appointments.params.PanelID + " #pnlPatientAppoitments_Result #dgvPatientAppoitments";
                        var PagingPanelControlID = Patient_Appointments.params.PanelID + " #divPatientAppoitmentsPaging";
                        var ClassControlName = "Patient_Appointments";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(
                            CreatePagination(response.SchAppStatusCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                                Patient_Appointments.PatientAppoitmentsSearch(PrimaryID, PageNumber, ResultPerPage);
                            }), 10);
                    }
                    else
                        utility.DisplayMessages(response.Message, 3);
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    PatientAppointmentsGridLoad: function (response) {
        $("#" + Patient_Appointments.params.PanelID + " #pnlPatientAppoitments_Result #dgvPatientAppoitments").dataTable().fnDestroy();
        $("#" + Patient_Appointments.params.PanelID + " #pnlPatientAppoitments_Result #dgvPatientAppoitments tbody").find("tr").remove();
        if (response.SchAppStatusCount > 0) {
            var SchAppStatusLoadJSONData = JSON.parse(response.SchAppStatus_JSON);
            $.each(SchAppStatusLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", "gvAppointment_row" + item.AppointmentId);
                $row.append('<td style="display:none;">' + item.AppointmentId + '</td><td>' + item.AppointmentDate + '</td><td>' + item.AppointmentTime + '</td><td>' + item.Reason + '</td><td>' + item.ProviderName + '</td><td>' + item.FacilityName + '</td><td>' + item.Duration + ' (min)</td><td>' + item.AppointmentStatus + '</td>');
                $("#" + Patient_Appointments.params.PanelID + " #pnlPatientAppoitments_Result #dgvPatientAppoitments tbody").last().append($row);
            });
        }
        else {
            $("#" + Patient_Appointments.params.PanelID + ' #pnlPatientAppoitments_Result #dgvPatientAppoitments').DataTable({
                "language": {
                    "emptyTable": "No Appoitment Found."
                }, "autoWidth": false, "ordering": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Patient_Appointments.params.PanelID + ' #pnlPatientAppoitments_Result #dgvPatientAppoitments'))
            ;
        else
            $("#" + Patient_Appointments.params.PanelID + ' #pnlPatientAppoitments_Result #dgvPatientAppoitments').DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false }] });
    },
    PatientAppointment_DBCall: function (id, PageNumber, RowsPerPage) {
        var AppointmentStatusIds = $("#" + Patient_Appointments.params.PanelID + ' #ddlAppointmentStatus option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        var objData = {};
        objData["RowsPerPage"] = RowsPerPage ? RowsPerPage : 15;  
        objData["PageNumber"] = PageNumber ? PageNumber : 1;
        objData["AppointmentStatusIds"] = AppointmentStatusIds;
        objData["ProviderId"] = $("#" + Patient_Appointments.params.PanelID + " #frmPatientAppointments #hfProvider").val();
        objData["FacilityId"] = $("#" + Patient_Appointments.params.PanelID + " #frmPatientAppointments #hfFacility").val();
        objData["PatientId"] = $("#" + Patient_Appointments.params.PanelID + " #hfPatientId").val();
        objData["CommandType"] = "load_patient_appoitments";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "DashBoard", "LoadDashBoard");
    },

    BindFacility: function () {
        var Ctrl = $("#" + Patient_Appointments.params.PanelID + " #frmPatientAppointments #txtFacility");
        var func = function () { return utility.GetFacilityDescriptionArray(Ctrl.val()) };
        var hfCtrl = $("#" + Patient_Appointments.params.PanelID + " #frmPatientAppointments #hfFacility");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },
    BindProvider: function (isFullName, shortName) {
        var Ctrl = $("#" + Patient_Appointments.params.PanelID + " #frmPatientAppointments #txtProvider");
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        var hfCtrl = $("#" + Patient_Appointments.params.PanelID + " #frmPatientAppointments #hfProvider");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },
    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmPatientAppointments";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "patTabAppointments";
        LoadActionPan('Admin_Facility', params);
    },
    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmPatientAppointments";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "patTabAppointments";
        LoadActionPan('Admin_Provider', params);
    },

    UnLoadTab: function () {
        var objDeffered = $.Deferred();
        if (Patient_Appointments.params["FromAdmin"] == "0") {
            if (Patient_Appointments.params != null && Patient_Appointments.params.ParentCtrl != null)
                UnloadActionPan(Patient_Appointments.params.ParentCtrl, 'Patient_Appointments');
            else
                UnloadActionPan(null, 'Patient_Appointments');
        }
        else
            RemoveAdminTab();
        objDeffered.resolve();
        return objDeffered;
    },


}
