schChangeFacility = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        schChangeFacility.params = params;

        var self = $('#schChangeFacility');
        self.loadDropDowns(true).done(function () {
            schChangeFacility.LoadChangeSchFacility();

            //serialize Data.
            $('#frmChangeFacility').data('serialize', $('#frmChangeFacility').serialize());
        });
    },

    LoadChangeSchFacility: function () {
        

        schChangeFacility.ValidateChangeSchFacility(schChangeFacility.params.SlotIDs);
       
    },

    ValidateChangeSchFacility: function (SlotIDs) {
        $('#frmChangeFacility')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               excluded: ':disabled',
               fields: {

                   facility: {
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
            schChangeFacility.ChangeFacilitySave(SlotIDs);
        });
    },

    ChangeFacilitySave: function (SlotIDs) {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Slot", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                var newfacilityId;

                newfacilityId = $('#schChangeFacility #ddlfacility').val();
                schChangeFacility.ChangeScheduleFacility(SlotIDs, newfacilityId).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        UnloadActionPan(schChangeFacility.params["ParentCtrl"], "actionPanSchChangeFacility");
                        Scheduling_Search.ScheduleSearch(schChangeFacility.params.CurrentPage, schChangeFacility.params.RecordsPerPage);
                        $('#dgvScheduleSearch input[type=checkbox]').each(function () {
                            if ($(this).is(":checked")) {
                                $(this).attr('checked', false);
                            }
                        });
                    }
                    else {
                        UnloadActionPan(schChangeFacility.params["ParentCtrl"], "actionPanSchChangeFacility");
                        utility.DisplayMessages(response.Message, 3);
                    }
                });

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    ChangeScheduleFacility: function (SlotDtlIds, MoveFacilityId) {

        var data = "SlotDtlIds=" + SlotDtlIds + "&MoveFacilityId=" + MoveFacilityId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "SCHEDULING_CHANGE_FACILITY_DETAIL", "CHANGE_SCH_FACILITY");

    },

    UnLoad: function () {

        utility.UnLoadDialog('frmChangeFacility', function () {
            UnloadActionPan(schChangeFacility.params["ParentCtrl"], "actionPanSchChangeFacility");
        }, function () {
            UnloadActionPan(schChangeFacility.params["ParentCtrl"], "actionPanSchChangeFacility");
        });

       
    },
}