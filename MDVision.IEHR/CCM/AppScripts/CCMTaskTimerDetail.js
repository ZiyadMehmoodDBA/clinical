CCMTaskTimerDetail = {
    bIsFirstLoad: true,
    ticker: {},
    ConsentFileStream: {},
    params: [],
    Load: function (params)
    {
        CCMTaskTimerDetail.params = params;

        if (CCMTaskTimerDetail.params != null && CCMTaskTimerDetail.params.PanelID != "pnlCCMTaskTimerDetail") {
            CCMTaskTimerDetail.params["PanelID"] = CCMTaskTimerDetail.params["PanelID"] + ' #pnlCCMTaskTimerDetail';
        }
        else {
            CCMTaskTimerDetail.params["PanelID"] = "pnlCCMTaskTimerDetail";
        }
        
        var self = $('#' + CCMTaskTimerDetail.params.PanelID);
        if (CCMTaskTimerDetail.bIsFirstLoad)
        {
            self.loadDropDowns(true).done(function ()
            {
            CCMTaskTimerDetail.bIsFirstLoad = false;
            //CCMTaskTimerDetail.LoadAllControls();
            CCMTaskTimerDetail.LoadCCMTaskTimerDetail();
            CCMTaskTimerDetail.InitializeControls();
            $("#txtAddedBy").val(globalAppdata.AppUserName);
            //$("#txtTaskReason").val('CCM Program');
            });
        }

        setTimeout(function () {

            jQuery("#ddlTaskReason").find("option:contains('CCM Program')").each(function () {
                if (jQuery(this).text() == 'CCM Program') {
                    jQuery(this).attr("selected", "selected");
                }
            });
        }, 150);
        //    $("#" + CCMTaskTimerDetail.params["PanelID"] + " #divDetails_ #txtAddedBy").val(globalAppdata.AppUserName);
        
    },

    LoadCCMTaskTimerDetail: function () {
        PageNo = null;
        rpp = null;
        CCMTaskTimerDetail.ValidateCCMTaskTimerDetail();

            //Serialize data
            $('#frmCCMTaskTimerDetail').data('serialize', $('#frmCCMTaskTimerDetail').serialize());
       
      
    },

    InitializeControls: function ()
    {
        utility.CreateDatePicker(CCMTaskTimerDetail.params.PanelID + ' #divDetails_ #dtpCallDate', function (ev) { });
        $("#" + CCMTaskTimerDetail.params.PanelID + ' #divDetails_ #txtCallTime').timepicker({
            defaultTime: '12:00 PM',
            minuteStep: 1,
        });

        $("#" + CCMTaskTimerDetail.params.PanelID + ' #divDetails_ #txtCallTime').timepicker().on('changeTime.timepicker', function (e) {
            disableFocus: false
        });


        $("#" + CCMTaskTimerDetail.params.PanelID + ' #divDetails_ #dtpCallDate').datepicker('setDate', new Date());
        $("#" + CCMTaskTimerDetail.params.PanelID + ' #divDetails_ #txtCallTime').timepicker('setTime', new Date().toLocaleTimeString());

        $("#hfPatientId").val(CCMTaskTimerDetail.params.PatientId)

    },

    //TIMER FUNCTIONS START
    RecordTaskTime: function () {
        var self = $("#" + CCMTaskTimerDetail.params["PanelID"] + " #modaldialog");
        self.find("#btnRecord").addClass("disableAll")
        self.find("#btnLogTime").addClass("disableAll")
        self.find("#btnReset").addClass("disableAll")

        var hoursControl = self.find("#txtTaskHours");
        var minutesControl = self.find("#txtTaskMinutes");
        var secondsControl = self.find("#txtTaskSeconds");
        var miliSecondsControl = self.find("#txtTaskMiliSeconds");



        hoursControl.addClass("disableAll");
        minutesControl.addClass("disableAll");
        secondsControl.addClass("disableAll");
        //  miliSecondsControl.addClass("disableAll");

        var hours = hoursControl.val();
        var minutes = minutesControl.val();
        var seconds = secondsControl.val();
        // var miliSeconds = miliSecondsControl.val();

        CCMTaskTimerDetail.ticker = setInterval(function () {


            //if (miliSeconds == 100) {
            //    miliSeconds = 0;
            //    seconds++;
            //}
            if (seconds == 60) {
                seconds = 0;
                minutes++;
            }
            if (minutes == 60) {
                minutes = 0;
                hours++;

            }

            seconds++;
            //miliSeconds++;

            hoursControl.val(hours);
            minutesControl.val(minutes);
            secondsControl.val(seconds);
            // miliSecondsControl.val(miliSeconds);

        }, 1000);
    },
    StopTaskTime: function () {
        var self = $("#" + CCMTaskTimerDetail.params["PanelID"] + " #modaldialog");
        clearInterval(CCMTaskTimerDetail.ticker);

        self.find("#btnRecord").removeClass("disableAll")
        self.find("#btnLogTime").removeClass("disableAll")
        self.find("#btnReset").removeClass("disableAll")
        self.find("#txtTaskHours").removeClass("disableAll");
        self.find("#txtTaskMinutes").removeClass("disableAll");
        self.find("#txtTaskSeconds").removeClass("disableAll");


    },
    ResetTaskTime: function () {
        var self = $("#" + CCMTaskTimerDetail.params["PanelID"] + " #modaldialog");

        self.find("#txtTaskHours").val(0);
        self.find("#txtTaskMinutes").val(0);
        self.find("#txtTaskSeconds").val(0);

    },
    //TIMER FUNCTIONS END


    LoadAllControls: function () {
        utility.ValidateFromToDate(CCMTaskTimerDetail.params.PanelID + ' #frmCCMTaskTimerDetail', 'dtpStartingFrom', 'dtpEndingOn', true);

        utility.CreateDatePicker(CCMTaskTimerDetail.params.PanelID + ' #frmCCMTaskTimerDetail #dtpConsentDate', function (ev) { });
    },

    ValidateCCMTaskTimerDetail: function () {
        $('#frmCCMTaskTimerDetail')
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {

                    TaskReason: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                }
            })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            CCMTaskTimerDetail.saveCCMTaskTimerDetail();
        });
    },

    saveCCMTaskTimerDetail: function () {

        var self = $("#" + CCMTaskTimerDetail.params["PanelID"]);
        var myJSON = self.getMyJSONByName();
        var strMessage = "";
      
            //fixme add priviliges
            //  AppPrivileges.GetFormPrivileges("Chronic Care Management", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                CCMTaskTimerDetail.CCMTaskTimerDetailSave(myJSON).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);

                        if (CCMTaskTimerDetail.params != null && CCMTaskTimerDetail.params.ParentCtrl != null) {
                            UnloadActionPan(CCMTaskTimerDetail.params.ParentCtrl, 'CCMTaskTimerDetail');
                        }
                        else
                            UnloadActionPan(null, 'CCMTaskTimerDetail');

                        DashBoard.DashBoardCCMEnrollmentInfoSearch(null, null, null, null);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
            // });
    },

    

    CCMTaskTimerDetailSave: function (CCMEnrollmentData) {
        var objData = JSON.parse(CCMEnrollmentData);

        objData["ConsentFileStream"] = "";// CCMTaskTimerDetail.ConsentFileStream;
        objData["PatientId"] = CCMTaskTimerDetail.params.PatientId;
        objData["StatusId"] = "2";
        objData["EnrollmentInfoId"] = CCMTaskTimerDetail.params.EnrollmentInfoId;

        objData["TaskDate"] = $("#dtpCallDate").val();
        objData["TaskTime"] = $("#txtCallTime").val();

        var data = JSON.stringify(objData);
        return MDVisionService.CCMAPIService(data, "CCMProgramUpdate", "SaveCCMTaskTimeFromDashBoard");
    },

    SaveAndAddMore: function ()
    {
        var self = $("#" + CCMTaskTimerDetail.params["PanelID"]);
        var myJSON = self.getMyJSONByName();
        var strMessage = "";

        //fixme add priviliges
        //  AppPrivileges.GetFormPrivileges("Chronic Care Management", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            CCMTaskTimerDetail.CCMTaskTimerDetailSave(myJSON).done(function (response) {
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    $("#txtTaskDuration").val('');
                    //$("#dtpCallDate").val('');
                    //$("#txtTaskReason").val('CCM Program');
                    $("#txtComments").val('');
                    //$("#txtCallTime").val('12:00 PM');
                    $("#ddlTaskDurationUnit").val('minutes');
                    $("#" + CCMTaskTimerDetail.params.PanelID + ' #divDetails_ #dtpCallDate').datepicker('setDate', new Date());
                    $("#" + CCMTaskTimerDetail.params.PanelID + ' #divDetails_ #txtCallTime').timepicker('setTime', new Date().toLocaleTimeString());
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }

                DashBoard.DashBoardCCMEnrollmentInfoSearch(null, null, null, null);
            });
        }
        else
            utility.DisplayMessages(strMessage, 2);
    },

    UnLoad: function () {

        if (CCMTaskTimerDetail.params != null && CCMTaskTimerDetail.params.ParentCtrl != null && CCMTaskTimerDetail.params.PanelID != 'pnlCCMTaskTimerDetail') {
            UnloadActionPan(CCMTaskTimerDetail.params.ParentCtrl, 'pnlCCMTaskTimerDetail', null, CCMTaskTimerDetail.params.PanelID);
        }

        else if (CCMTaskTimerDetail.params != null && CCMTaskTimerDetail.params.ParentCtrl != null) {
            UnloadActionPan(CCMTaskTimerDetail.params.ParentCtrl, 'CCMTaskTimerDetail');
        }
    },
}