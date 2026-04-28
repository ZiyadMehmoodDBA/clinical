Scheduling_CheckInReason = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Scheduling_CheckInReason.params = params;
        Scheduling_CheckInReason.ValidatePatientCheckOut();
    },

    ValidatePatientCheckOut: function () {
        $('#frmScheduling_CheckInReason')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   CheckInReason: {
                       group: '.col-sm-12',
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
            Scheduling_CheckInReason.CheckInAppointmentwithReason();
        });
    },
    CheckInAppointmentwithReason: function ()
    {
        var checkinReason = $("#frmScheduling_CheckInReason #txtCheckInReason").val();
        Scheduling_CheckInReason.UnLoad();
        schcheckin.CheckinAppointment(checkinReason);
    },
    UnLoad: function () {
      UnloadActionPan(Scheduling_CheckInReason.params["ParentCtrl"], "actionPanScheduling_CheckInReason");
    },

}