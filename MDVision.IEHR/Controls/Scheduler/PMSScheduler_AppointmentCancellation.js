PMSScheduler_AppointmentCancellation = {
    params: [],
    Load: function (params) {
        PMSScheduler_AppointmentCancellation.params = params;
        $("#pnlPMSScheduler_AppointmentCancellation #CancellationReason").keypress(function (event) {
            var character = String.fromCharCode(event.keyCode);
            return isValid(character);
        });
        setTimeout(function () { $("#pnlPMSScheduler_AppointmentCancellation #CancellationReason").focus(); }, 1000);
        function isValid(str) {
            return !/[&]/g.test(str);
        }

        if (PMSScheduler_AppointmentCancellation.params.ParentCtrl == "appointmentDetail" && PMSScheduler_AppointmentCancellation.params.IsEdit == true) {
            $('#pnlPMSScheduler_AppointmentCancellation #CancellationReason').val($('#appointmentDetail #txtCancellationReason').val());
        }
        PMSScheduler_AppointmentCancellation.ValidateAppointmentCancellation();
    },

    ValidateAppointmentCancellation: function () {
        $('#pnlPMSScheduler_AppointmentCancellation #frmAppointmentCancellation').bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  Description: {
                      group: '.col-sm-12',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
              }
          }).on('success.form.bv', function (e) {
           e.preventDefault();
           PMSScheduler_AppointmentCancellation.Save();
       });
    },

    UpdateAppointmentCancellationReason: function (AppointmentID, CancelReason) {
        var object = new Object();
        object["CancelReason"] = CancelReason;
        var stream = JSON.stringify(object);
        var data = "AppointmentID=" + AppointmentID + "&AppointmentData=" + stream;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_APPOINTMENT_DETAIL", "UPDATE_APPOINTMENT_CANCELLATION_REASON");
    },
    Save: function() {
        var CancellationReason = $('#pnlPMSScheduler_AppointmentCancellation #CancellationReason').val();
        if (PMSScheduler_AppointmentCancellation.params.ParentCtrl == "appointmentDetail" && PMSScheduler_AppointmentCancellation.params.IsEdit != true) {
            if (CancellationReason != null && CancellationReason != "") {            
                 appointmentDetail.CancellationReason = CancellationReason;
                 PMSScheduler_AppointmentCancellation.UnLoad();
                 appointmentDetail.SavePatientAppointmentDetail();          //MA-693
            }
            else {
                utility.DisplayMessages("Please enter a cancellation reason.", 2);
              //  return;
            }
        }
        else if (PMSScheduler_AppointmentCancellation.params.ParentCtrl == "appointmentDetail" && PMSScheduler_AppointmentCancellation.params.IsEdit == true) {
            PMSScheduler_AppointmentCancellation.UpdateAppointmentCancellationReason(PMSScheduler_AppointmentCancellation.params.AppointmentId, CancellationReason).done(function (response) {
                if (response.status != false) {
                    var myJson = $("#appointmentpanel #frmappointmentDetail").getMyJSON();                   
                    utility.DisplayMessages(response.Message, 1);
                    appointmentDetail.LoadCurrentAppointmentInScheduler("Add", "", "", myJson);
                    $('#appointmentDetail #txtCancellationReason').val($('#pnlPMSScheduler_AppointmentCancellation #CancellationReason').val());
                }
                else {
                    utility.DisplayMessages(response.Message, 2);
                }
                //Start MA-693 Tahreem Malik 
                $.when(PMSScheduler_AppointmentCancellation.UnLoad()).done(function () {
                    utility.callbackAfterAllDOMLoaded(function () {
                        UnloadActionPan(appointmentDetail.params["ParentCtrl"], "appointmentDetail");
                    });
                });
                //End MA-693 Tahreem Malik
            });
        }
        else {
            if (CancellationReason != null && CancellationReason != "") {
                Scheduling_Calendar.UpdateAppointmentStatus(PMSScheduler_AppointmentCancellation.params.AppointmentId, PMSScheduler_AppointmentCancellation.params.StatusId, CancellationReason).done(function (response) {
                    if (response.status != false) {
                        PMSScheduler.CanScheduler.dataSource.read();
                        //var dataSourceApp = PMSScheduler.CanScheduler.dataSource;
                        //var ap = dataSourceApp._data.filter(f => f.AppointmentId == appid)[0];
                        ////ap.VisitId = visitId;
                        //ap.AppointmentStatus = AppointmentStatus;
                        //ap.SchStatusId = statusid;
                        //ap.StatusColor = color;
                        //PMSScheduler.CanScheduler.dataSource.pushUpdate(ap);
                        utility.DisplayMessages(response.Message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 2);
                    }
                    PMSScheduler_AppointmentCancellation.UnLoad();
                });
            }
            else {
                utility.DisplayMessages("Please enter a cancellation reason.", 2);
                //  return;
            }
        }
    },
    UnLoad: function (caller) {

        if (PMSScheduler_AppointmentCancellation.params.ParentCtrl != "") {
            UnloadActionPan(PMSScheduler_AppointmentCancellation.params.ParentCtrl);
        }
        else {
            UnloadActionPan();
        }


    },
}