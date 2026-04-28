holidaysDetail = {
    params: [],
    Load: function (params) {
        holidaysDetail.params = params;
        var self = null;
        if (holidaysDetail.params.PanelID == 'holidaysDetail')
            self = $('#holidaysDetail');
        else
            self = $('#' + holidaysDetail.params.PanelID + ' #holidaysDetail');

        if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
            self.find("#ddlEntity").attr('disabled', 'disabled');
        }

        self.loadDropDowns(true).done(function () {

            //if (globalAppdata['IsAdmin'] != "True") {
            //    $("#holidaysDetail #divHoliday_Entity").css("display", "none");
            //    $("#holidaysDetail #ddlEntity").val(globalAppdata["SeletedEntityId"]);
            //}
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
            }

            holidaysDetail.LoadHolidays();
        });
         
    },

    LoadHolidays: function () {
        if (holidaysDetail.params.mode == "Add") {

            holidaysDetail.ValidateHolidays();
            //serialize Data after all controls loaded.
            $('#frmHolidaysDetail').data('serialize', $('#frmHolidaysDetail').serialize());
            
        }
        else if (holidaysDetail.params.mode == "Edit") {
            holidaysDetail.FillHolidays(holidaysDetail.params.HolidaysId).done(function (response) {
                if (response.status != false) {
                    var holidays_detail = JSON.parse(response.HolidaysFill_JSON);
                    var self = $("#holidaysDetail");
                    utility.bindMyJSON(true, holidays_detail, false, self).done(function () {

                        if (holidays_detail.chkActive == 'True') {
                            $("#holidaysDetail #chkActive").attr("checked", true);
                        }
                        else {
                            $("#holidaysDetail #chkActive").attr("checked", false);
                        }
   

                        holidaysDetail.ValidateHolidays();
                        //serialize Data after all controls loaded.
                        $('#frmHolidaysDetail').data('serialize', $('#frmHolidaysDetail').serialize());

                    });
                   
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    ValidateHolidays: function () {
        $('#frmHolidaysDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   Holidaydate: {
                       group: '.col-xs-5',
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                           date: {
                               format: date_format.toUpperCase(),
                               message: ' '
                           }
                       }
                   },
                   Entity: {
                       group: '.col-xs-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Holiday: {
                       group: '.col-xs-9',
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
            holidaysDetail.HolidaysSave();
        });
    },

    HolidaysSave: function () {
        var strMessage = "";
        var self = $("#holidaysDetail");
        var myJSON = self.getMyJSON();
        if (holidaysDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Holidays", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    holidaysDetail.SaveHolidays(myJSON).done(function (response) {
                        if (response.status != false) {
                            Admin_Holidays.HolidaysSearch(response.HolidaysId);
                            utility.DisplayMessages(response.message, 1);
                            UnloadActionPan();
                            //MDVisionService.reloadLookups = true;
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
        else if (holidaysDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Holidays", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    holidaysDetail.UpdateHolidays(myJSON, holidaysDetail.params.HolidaysId).done(function (response) {
                        if (response.status != false) {
                            Admin_Holidays.HolidaysSearch(holidaysDetail.params.HolidaysId);
                            utility.DisplayMessages(response.message, 1);
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

    SaveHolidays: function (HolidaysData) {
        var data = "HolidaysData=" + HolidaysData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_HOLIDAYS_DETAIL", "SAVE_HOLIDAYS");
    },

    UpdateHolidays: function (HolidaysData, HolidaysID) {
        var data = "HolidaysData=" + HolidaysData + "&HolidaysID=" + HolidaysID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_HOLIDAYS_DETAIL", "UPDATE_HOLIDAYS");
    },

    FillHolidays: function (HolidaysID) {
        var data = "HolidaysID=" + HolidaysID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_HOLIDAYS_DETAIL", "FILL_HOLIDAYS");
    },

    UpdateScheduleHolidaysActiveInactive: function (HolidaysID, IsActive) {
        var data = "HolidaysID=" + HolidaysID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_HOLIDAYS_DETAIL", "UPDATE_HOLIDAYS_ACTIVE_INACTIVE");
    },

    UnLoad: function () {

        utility.UnLoadDialog("frmHolidaysDetail", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });
    },

    ShowHistory: function () {
        var PanelID = 'holidaysDetail';
        var ParentCtrl = 'holidaysDetail';
        var ProfileName = 'Holidays';
        var DBTableName = 'ScheduleHolidays';
        var ColumnKeyId = holidaysDetail.params.HolidaysId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);

    },
}