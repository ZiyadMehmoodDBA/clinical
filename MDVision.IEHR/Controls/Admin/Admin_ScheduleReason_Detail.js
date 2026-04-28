scheduleReasonDetail = {
    params: [],
    Load: function (params) {
        scheduleReasonDetail.params = params;
        var self = $('#scheduleReasonDetail');
        if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
            self.find("#ddlEntity").attr('disabled', 'disabled');
        }
        self.loadDropDowns(true).done(function () {

            //if (globalAppdata['IsAdmin'] != "True") {
            //    $("#scheduleReasonDetail #divReason_Entity").css("display", "none");
            //    $("#scheduleReasonDetail #ddlEntity").val(globalAppdata["SeletedEntityId"]);
            //}
          
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
               self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
            }
            scheduleReasonDetail.LoadScheduleReason();

        });
              
    },

    LoadScheduleReason: function () {
        
        if (scheduleReasonDetail.params.mode == "Add") {
            $('#scheduleReasonDetail #txtShortName').attr("enabled", "enabled");
            
            scheduleReasonDetail.ValidateScheduleReason();
            //serialize Data after all controls loaded.
            $('#frmScheduleReasonDetail').data('serialize', $('#frmScheduleReasonDetail').serialize());
           
        }
        else if (scheduleReasonDetail.params.mode == "Edit") {
            $('#scheduleReasonDetail #txtShortName').attr("disabled", "disabled");
            scheduleReasonDetail.FillScheduleReason(scheduleReasonDetail.params.ScheduleReasonId).done(function (response) {
                if (response.status != false) {
                    var scheduleReason_detail = JSON.parse(response.ScheduleReasonFill_JSON);
                    var self = $("#scheduleReasonDetail");
                    utility.bindMyJSON(true, scheduleReason_detail, false, self).done(function () {


                        if (scheduleReason_detail.chkActive == 'True')
                            $("#scheduleReasonDetail #chkActive").attr("checked", true);
                        else
                            $("#scheduleReasonDetail #chkActive").attr("checked", false);

                        scheduleReasonDetail.ValidateScheduleReason();
                        //serialize Data after all controls loaded.
                        $('#frmScheduleReasonDetail').data('serialize', $('#frmScheduleReasonDetail').serialize());

                    });
                   
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    ValidateScheduleReason: function () {
        $('#frmScheduleReasonDetail')
           .bootstrapValidator({
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   ShortName: {
                       group: '.col-xs-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Duration: {
                       group: '.col-xs-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Entity: {
                       group: '.col-xs-5',
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
            scheduleReasonDetail.ScheduleReasonSave();
        });
    },

    ScheduleReasonSave: function () {
        var strMessage = "";
        var self = $("#scheduleReasonDetail");
        var myJSON = self.getMyJSON();
        if (scheduleReasonDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Schedule Reason", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    scheduleReasonDetail.SaveScheduleReason(myJSON).done(function (response) {
                        if (response.status != false) {
                            Admin_ScheduleReason.ScheduleReasonSearch(response.ScheduleReasonId);
                            utility.DisplayMessages(response.message, 1);
                            scheduleReasonDetail.UnLoad();
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
        else if (scheduleReasonDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Schedule Reason", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    scheduleReasonDetail.UpdateScheduleReason(myJSON, scheduleReasonDetail.params.ScheduleReasonId).done(function (response) {
                        if (response.status != false) {
                            Admin_ScheduleReason.ScheduleReasonSearch(scheduleReasonDetail.params.ScheduleReasonId);
                            utility.DisplayMessages(response.message, 1);
                            scheduleReasonDetail.UnLoad();
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

    SaveScheduleReason: function (ScheduleReasonData) {
        var data = "ScheduleReasonData=" + ScheduleReasonData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SCHEDULE_REASON_DETAIL", "SAVE_SCHEDULE_REASON");
    },

    UpdateScheduleReason: function (ScheduleReasonData, ScheduleReasonID, IsActive) {
        var data = "ScheduleReasonData=" + ScheduleReasonData + "&ScheduleReasonID=" + ScheduleReasonID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SCHEDULE_REASON_DETAIL", "UPDATE_SCHEDULE_REASON");
    },

    FillScheduleReason: function (ScheduleReasonID) {
        var data = "ScheduleReasonID=" + ScheduleReasonID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SCHEDULE_REASON_DETAIL", "FILL_SCHEDULE_REASON");
    },

    UpdateScheduleReasonActiveInactive: function (ScheduleReasonID, IsActive) {
        var data = "ScheduleReasonID=" + ScheduleReasonID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SCHEDULE_REASON_DETAIL", "UPDATE_SCHEDULE_REASON_ACTIVE_INACTIVE");
    },

    UnLoad: function () {

        //utility.UnLoadDialog("frmScheduleReasonDetail", function () {
        //    UnloadActionPan();
        //}, function () {
        //    UnloadActionPan();
        //});

        if (scheduleReasonDetail.params != null && scheduleReasonDetail.params.ParentCtrl != null) {
            UnloadActionPan(scheduleReasonDetail.params.ParentCtrl, 'scheduleReasonDetail', null, scheduleReasonDetail.params.PanelID);
        }
        else {
            utility.UnLoadDialog("frmScheduleReasonDetail", function () {
                UnloadActionPan(scheduleReasonDetail.params["ParentCtrl"], "scheduleReasonDetail");
            }, function () {
                UnloadActionPan(scheduleReasonDetail.params["ParentCtrl"], "scheduleReasonDetail");
            });
        }

    },
    ShowHistory: function () {
        var PanelID = 'scheduleReasonDetail';
        var ParentCtrl = 'scheduleReasonDetail';
        var ProfileName = 'Schedule Reason';
        var DBTableName = 'ScheduleReasons';
        var ColumnKeyId = scheduleReasonDetail.params.ScheduleReasonId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);

    },
}