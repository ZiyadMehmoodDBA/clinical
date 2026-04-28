appointmentStatusDetail = {
    params: [],
    Load: function (params) {
        appointmentStatusDetail.params = params;
        appointmentStatusDetail.LoadAppointmentStatus();
    },

    LoadAppointmentStatus: function () {
        if (appointmentStatusDetail.params.mode == "Add") {
            $('#appointmentStatusDetail #txtShortName').attr("enabled", "enabled");
            
            appointmentStatusDetail.ValidateAppointmentStatus();
            //serialize Data after all controls loaded.
            $('#frmAppointmentStatusDetail').data('serialize', $('#frmAppointmentStatusDetail').serialize());
        }
        else if (appointmentStatusDetail.params.mode == "Edit") {

            $('#appointmentStatusDetail #txtShortName').attr("disabled", "disabled");
            appointmentStatusDetail.FillAppointmentStatus(appointmentStatusDetail.params.AppointmentId).done(function (response) {
                if (response.status != false) {
                    var appointmentStatus_detail = JSON.parse(response.AppointmentStatusFill_JSON);
                    var self = $("#appointmentStatusDetail");
                    utility.bindMyJSON(true, appointmentStatus_detail, false, self).done(function () {

                        if (appointmentStatus_detail.chkActive == 'True')
                            $("#appointmentStatusDetail #chkActive").attr("checked", true);
                        else
                            $("#appointmentStatusDetail #chkActive").attr("checked", false);
                        
                        
                        $('.demo2').colorpicker('setValue', appointmentStatus_detail.txtColor);
                        
                        //serialize Data after all controls loaded.
                        $('#frmAppointmentStatusDetail').data('serialize', $('#frmAppointmentStatusDetail').serialize());
                        
                    });

                   
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
            appointmentStatusDetail.ValidateAppointmentStatus();
        }
    },

    ValidateAppointmentStatus: function () {
        $('#frmAppointmentStatusDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   name: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Color: {
                       group: '.col-sm-4',
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
            appointmentStatusDetail.AppointmentStatusSave();
        });
    },

    AppointmentStatusSave: function () {
        var strMessage = "";
        var self = $("#appointmentStatusDetail");
        var myJSON = self.getMyJSON();
        if (appointmentStatusDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Appointment Status", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    appointmentStatusDetail.SaveAppointmentStatus(myJSON).done(function (response) {
                        if (response.status != false) {
                            Admin_AppointmentStatus.AppointmentStatusSearch(response.AppointmentId);
                            utility.DisplayMessages(response.message, 1);
                            CacheManager.BindCodes('GetAppointmentStatus', true);
                            MDVisionService.lookups("GetAppointmentStatus", true);

                            UnloadActionPan();
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else if (appointmentStatusDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Appointment Status", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    appointmentStatusDetail.UpdateAppointmentStatus(myJSON, appointmentStatusDetail.params.AppointmentId).done(function (response) {
                        if (response.status != false) {
                            Admin_AppointmentStatus.AppointmentStatusSearch(appointmentStatusDetail.params.AppointmentId);
                            utility.DisplayMessages(response.message, 1);
                            CacheManager.BindCodes('GetAppointmentStatus', true);
                            MDVisionService.lookups("GetAppointmentStatus", true);

                            UnloadActionPan();
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
    },

    SaveAppointmentStatus: function (AppointmentStatusData) {
        var data = "AppointmentStatusData=" + AppointmentStatusData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_APPOINTMENT_STATUS_DETAIL", "SAVE_APPOINTMENT_STATUS");
    },

    UpdateAppointmentStatus: function (AppointmentStatusData, AppointmentID) {
        var data = "AppointmentStatusData=" + AppointmentStatusData + "&AppointmentID=" + AppointmentID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_APPOINTMENT_STATUS_DETAIL", "UPDATE_APPOINTMENT_STATUS");
    },

    FillAppointmentStatus: function (AppointmentID) {
        var data = "AppointmentID=" + AppointmentID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_APPOINTMENT_STATUS_DETAIL", "FILL_APPOINTMENT_STATUS");
    },

    UpdateScheduleAppointmentStatusActiveInactive: function (AppointmentID, IsActive) {
        var data = "AppointmentID=" + AppointmentID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_APPOINTMENT_STATUS_DETAIL", "UPDATE_APPOINTMENT_STATUS_ACTIVE_INACTIVE");
    },

    UnLoad: function () {

        utility.UnLoadDialog("frmAppointmentStatusDetail", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });
    },

    ShowHistory: function () {
        var PanelID = 'appointmentStatusDetail';
        var ParentCtrl = 'appointmentStatusDetail';
        var ProfileName = 'Appointment Status';
        var DBTableName = 'AppointmentStatus';
        var ColumnKeyId = appointmentStatusDetail.params.AppointmentId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);

    },
}