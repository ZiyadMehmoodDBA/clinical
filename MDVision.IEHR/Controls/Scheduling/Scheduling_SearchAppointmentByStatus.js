schAppointmentStatus = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        schAppointmentStatus.params = params;       
        var self = $('#schAppointmentStatus');
        self.loadDropDowns(true);

  
        schAppointmentStatus.SearchAppointmentByStatus(schAppointmentStatus.params.ProviderId, schAppointmentStatus.params.FacilityId, schAppointmentStatus.params.SlotDate, schAppointmentStatus.params.color, schAppointmentStatus.params.ResourceId);

    },

    SearchAppointmentByStatus: function (ProviderId, FacilityId, SlotDate, color, ResourceId) {
        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Appointment", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
                if ($("#schAppointmentStatus #pnlSchAppointmentStatus_Result").css("display") == "none") {
                    $("#schAppointmentStatus #pnlSchAppointmentStatus_Result").show();
                }

                
                schAppointmentStatus.SearchAppByStatus(ProviderId, FacilityId, SlotDate, color, ResourceId).done(function (response) {
                    if (response.status != false) {
                        schAppointmentStatus.AppStatusSearchGridLoad(response);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
    },

    AppStatusSearchGridLoad: function (response) {
        $("#dgvSchAppointmentStatus").dataTable().fnDestroy();
        //$("#pnlSchAppointmentStatus_Result #dgvScheduleSearch tbody").find("tr").remove();
        if (response.SchAppStatusCount > 0) {
            var SchAppStatusJSONData = JSON.parse(response.SchAppStatus_JSON);
            $.each(SchAppStatusJSONData, function (i, item) {
                var $row = $('<tr style="cursor: pointer;" />');
                //$row.attr("onclick", "utility.SelectGridRow($('#gvAppointmentStatus_row" + item.AppointmentId + "'))");
                //$row.attr("id", "gvAppointmentStatus_row" + item.AppointmentId);
                //$row.attr("AppointmentId", item.AppointmentId);

                var ProvResoName = "";
                if (item.Provider != "") {
                    ProvResoName = item.Provider;
                } else if(item.Resource != "") {
                    ProvResoName = item.Resource;
                }
                $row.append('<td>' + item.Status + '</td><td>' + item.Date.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + item.Time + '</td><td>' + item.Facility + '</td><td>' + ProvResoName + '</td><td>' + item.Name + '</td><td>' + item.Phone + '</td><td>' + item.Reason + '</td><td>' + item.DOB.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td data-toggle="tooltip" data-placement="right" title="' + item.Comments + '">' + item.Comments.split(/\s+/).slice(0, 3).join(" ") + '</td><td>' + item.PatientType + '</td><td>' + item.VisitType + '</td>');


                $("#pnlSchAppointmentStatus_Result #dgvSchAppointmentStatus tbody").last().append($row);
            });
            
        }
        else {
            $('#dgvSchAppointmentStatus').DataTable({
                "language": {
                    "emptyTable": "No Appointment Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvSchAppointmentStatus'))
            ;
        else
            $("#pnlSchAppointmentStatus_Result #dgvSchAppointmentStatus").DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
    },

    SearchAppByStatus: function (ProviderId, FacilityId, SlotDate, color, ResourceId) {
        var data = "ProviderId=" + ProviderId + "&FacilityId=" + FacilityId + "&SlotDate=" + SlotDate + "&color=" + color + "&ResourceId=" + ResourceId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "SCHEDULING_SEARCH_APPSTATUS", "SEARCH_APP_BY_STATUS");
    },

    UnLoad: function () {

        UnloadActionPan();

        //if ($('#schAppointmentStatus').serialize() != $('#schAppointmentStatus').data('serialize')) {
        //    utility.myConfirm('2', function () {
        //        UnloadActionPan();
        //    }, function () { },
        //            '2'
        //    );
        //}
        //else {
        //    UnloadActionPan();
        //}
    },
}