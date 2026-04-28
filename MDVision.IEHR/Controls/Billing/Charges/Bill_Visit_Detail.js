visitDetail = {
    params: [],
    Load: function (params) {
        visitDetail.params = params;
        //appointmentStatusDetail.LoadAppointmentStatus();
    },


    UnLoad: function () {

        utility.UnLoadDialog('frmVisitDetail', function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });
    },
}