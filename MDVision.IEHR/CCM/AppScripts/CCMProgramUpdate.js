CCMProgramUpdate = {
    bIsFirstLoad: true,
    ticker: {},
    ProgramUpdateFields: [],
    params: [],
    ValidateComments: false,
    VisitReasons: [],
    VisitReason: "",
    VisitComments: "",

    Load: function (params, fromNote) {
        CCMProgramUpdate.params = params;

        if (CCMProgramUpdate.bIsFirstLoad) {
            // CCMProgramUpdate.bIsFirstLoad = false;
            var self = $("#" + CCMProgramUpdate.params.PanelID + " #divProgramUpdates");

            var data = "IsActive=&ID=" + CCMProgramUpdate.params.EnrollmentInfoId;
            self.loadDropDownsWithTitle(true, data).done(function () {
                self.loadDropDowns(true);

                if (fromNote == 1)
                    CCMProgramUpdate.LoadCCMProgramUpdate(true);
                else
                    CCMProgramUpdate.LoadCCMProgramUpdate(false);

                //CCMProgramUpdate.LoadCCMProgramUpdate(false);

                var objData = new Object();
                objData["EnrollmentInfoId"] = CCMProgramUpdate.params.EnrollmentInfoId;
                objData["PatientId"] = CCMProgramUpdate.params.PatientId;
                objData["ProviderId"] = "0";

                CCM_Patient_Hub.PatientHubStaticLoad(objData).done(function (response) {
                    if (response.status != false) {
                        CCM_Patient_Hub.BindPatientHubStaticData(response, "PatientHub");
                        var response1 = JSON.parse(response.PHList_JSON);
                        $("#" + CCMProgramUpdate.params.PanelID + " #txtReceiverName").val(response1[0].PatientName);

                        $("#pnlCCM_Patient_Hub #divProgramUpdates").attr("style", " ");
                    } else {
                        utility.DisplayMessages("Issue encountered while opening CCM Hub.", 3);
                        CCM_Patient_Hub.UnLoad();
                    }
                });

                CCMProgramUpdate.LoadAllControls();
                CCMProgramUpdate.ValidateTaskTimer();
                //CCMProgramUpdate.ValidateCallDetails();
                CCMProgramUpdate.ValidateChangeOnProgramUpdate();


                if (CCMProgramUpdate.params["ParentCtrl"] == 'clinicalTabProgressNote') {
                    if (CCMProgramUpdate.params.IsFromNote)
                        CCMProgramUpdate.isFromNote();
                }

                $("#ReasonComments").addClass('hidden');

                // CCM-328
                $("#ddlTaskReason").change(function () {
                    if ($("#ddlTaskReason option:selected").text() == "Other") {
                        CCMProgramUpdate.ValidateComments = true;
                        $("#ReasonComments").removeClass('hidden');
                    }
                    else {
                        CCMProgramUpdate.ValidateComments = false;
                        $("#ReasonComments").addClass('hidden');
                    }

                });

                setTimeout(function () {

                    jQuery("#ddlTaskReason").find("option:contains('CCM Program')").each(function () {
                        if (jQuery(this).text() == 'CCM Program') {
                            jQuery(this).attr("selected", "selected");
                        }
                    });
                    jQuery("#ddlCallReason").find("option:contains('CCM Program')").each(function () {
                        if (jQuery(this).text() == 'CCM Program') {
                            jQuery(this).attr("selected", "selected");
                        }
                    });
                }, 200);

                //CacheManager.BindDropDownsByEntityID('#ddlCaller', 'GetCallDetails', true, CCMProgramUpdate.params.EnrollmentInfoId);
            });
        }
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
    },

    LoadAllControls: function () {

        var self = $("#" + CCMProgramUpdate.params["PanelID"] + " #divProgramUpdates");
        self.find("#txtAddedBy").val(globalAppdata.AppUserNameFullName);

        CCMProgramUpdate.initializeDateControl();
    },

    LoadCCMProgramUpdate: function (isReset) {
        PageNo = null;
        rpp = null;

        if (isReset) {
            for (var i = 5; i <= 8; i++)
                $("[data-ProgressCategoryId='" + i + "']").val('');
        }

        CCMProgramUpdate.FillCCMProgramUpdate().done(function (response) {
            if (response.status != false) {
                if (isReset) {
                    CCMProgramUpdate.BindProgramUpdate(response).done(function () {
                        //successfully binded
                    });
                }
            }
            else {
                CCMProgramUpdate.LoadCCMTaskTime();

                if (isReset) {
                    for (var i = 5; i <= 8; i++)
                        $("[data-ProgressCategoryId='" + i + "']").val('');
                }

                //utility.DisplayMessages(response.Message, 3);
            }
        });
        //$("#txtTaskReason").val("CCM Program");

        CCMProgramUpdate.retainFixedCallerValues();
    },

    retainFixedCallerValues: function () {
        //$("#txtCallReason").val("CCM Program");
        $("#" + CCMProgramUpdate.params.PanelID + " #txtReceiverName").val(CCM_Patient_Hub.params.PatientName);

        for (var i = 0; i < $("#ddlCaller option").length; i++)
            if ($($("#ddlCaller option")[i]).attr('title') == "Provider")
                $("#ddlCaller option")[i].selected = true;
    },

    BindProgramUpdate: function (response) {

        var dfd = new $.Deferred();

        var TotalDuration = 0;
        var self = $("#" + CCMProgramUpdate.params["PanelID"] + " #divProgramUpdates");

        var CCMProgramUpdate_detail = response.CCMProgressUpdateFill_JSON;

        $.each(CCMProgramUpdate_detail, function (i, item) {
            CCMProgramUpdate.ProgramUpdateFields[item.ProgressCategoryId] = item.Value;
            self.find("[data-ProgressCategoryId='" + item.ProgressCategoryId + "']").val(item.Value);

            self.find("[data-ProgressCategoryId='" + item.ProgressCategoryId + "']").next().text("Last updated at " + item.CreatedOn + " by " + item.CreatedByName);

            TotalDuration = item.TotalDuration;
            dfd.resolve(CCMProgramUpdate_detail);
        });

        CCMProgramUpdate.LoadCCMTaskTime();
        return dfd.promise();
    },

    BindLastVisit: function (response) {

        var dfd = new $.Deferred();

        var self = $("#" + CCMProgramUpdate.params["PanelID"] + " #divProgramUpdates #divLastMonthProgress");

        var CCMProgramUpdate_detail = response.CCMProgressUpdateFill_JSON;

        $.each(CCMProgramUpdate_detail, function (i, item) {

            if (item.ProgressCategoryId == 5) {
                self.find("#txtGoalsAchieved").val(item.Value);
                self.find("#txtGoalsAchieved").next().text("Last updated at " + item.CreatedOn + " by " + item.CreatedBy);
            }
            else if (item.ProgressCategoryId == 6) {
                self.find("#txtProgressReducingBarrriers").val(item.Value);
                self.find("#txtProgressReducingBarrriers").next().text("Last updated at " + item.CreatedOn + " by " + item.CreatedBy);
            }
            else if (item.ProgressCategoryId == 7) {
                self.find("#txtProgressTowardsExpectedOutcomes").val(item.Value);
                self.find("#txtProgressTowardsExpectedOutcomes").next().text("Last updated at " + item.CreatedOn + " by " + item.CreatedBy);
            }
            else if (item.ProgressCategoryId == 8) {
                self.find("#txtOtherInformation").val(item.Value);
                self.find("#txtOtherInformation").next().text("Last updated at " + item.CreatedOn + " by " + item.CreatedBy);
            }

            dfd.resolve(CCMProgramUpdate_detail);
        });
        return dfd.promise();
    },

    ValidateTaskTimer: function () {
        $("#" + CCMProgramUpdate.params["PanelID"] + ' #frmTaskTimer').bootstrapValidator('destroy');

        $("#" + CCMProgramUpdate.params["PanelID"] + ' #frmTaskTimer')
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
                    group: '#divTaskReason',
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
                AddedBy: {
                    group: '#divAddedBy',
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
            CCMProgramUpdate.SaveTaskTime();
        });
    },

    ValidateCallDetails: function () {
        $('#frmTaskTimer').bootstrapValidator('destroy');
        $('#frmTaskTimer')
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                }
            })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            CCMProgramUpdate.SaveCallDetails();
        });
    },

    ValidateChangeOnProgramUpdate: function () {
        CCM_Patient_Hub.IshubChanged = false;

        var oldVal = "";
        $("#" + CCMProgramUpdate.params["PanelID"] + " #frmTaskTimer input").on("change keyup paste", function () {
            var currentVal = $(this).val();
            if (currentVal == oldVal) return;
            oldVal = currentVal;
            CCM_Patient_Hub.IshubChanged = true;
        });

        var oldVal1 = "";
        $("#" + CCMProgramUpdate.params["PanelID"] + " #frmTaskTimer input").on("change keyup paste", function () {
            var currentVal = $(this).val();
            if (currentVal == oldVal1) return;
            oldVal1 = currentVal;
            CCM_Patient_Hub.IshubChanged = true;
        });

        var oldVal2 = "";
        $("#" + CCMProgramUpdate.params["PanelID"] + " #frmTaskTimer select").on("change keyup paste", function () {
            var currentVal = $(this).val();
            if (currentVal == oldVal2) return;
            oldVal2 = currentVal;
            CCM_Patient_Hub.IshubChanged = true;
        });

        var oldVal3 = "";
        $("#" + CCMProgramUpdate.params["PanelID"] + " #programUpdatePrintDraw textarea").on("change keyup paste", function () {
            var currentVal = $(this).val();
            if (currentVal == oldVal3) return;
            oldVal3 = currentVal;
            CCM_Patient_Hub.IshubChanged = true;
        });
    },

    // Date Functions (start)
    initializeDateControl: function () {

        utility.CreateDatePicker(CCMProgramUpdate.params.PanelID + ' #dtpCallDate', function (ev) { });
        $('#frmTaskTimer #dtpCallDate').datepicker('setDate', new Date());
        $('#frmTaskTimer #txtCallTime').timepicker('setTime', new Date().toLocaleTimeString());

        $("#" + CCMProgramUpdate.params.PanelID + ' #txtCallTime').timepicker({
            defaultTime: '12:00 PM',
            minuteStep: 1,
        });

        $("#" + CCMProgramUpdate.params.PanelID + ' #txtCallTime').timepicker().on('changeTime.timepicker', function (e) {
            disableFocus: false
        });

        var self = $("#" + CCMProgramUpdate.params.PanelID + " #divProgramUpdates");
        var dateControl = self.find("#dtpMonthlyProgressUpdate");

        var today = new Date();
        if ($('#dtpMonthlyProgressUpdate').text() == "") {
            var cur = $.datepicker.formatDate('MM/yy', new Date(today));
            $('#dtpMonthlyProgressUpdate').text(cur);
        }

        $('#dtpMonthlyProgressUpdate').text(cur);
        $('#dtpMonthlyProgressUpdate').datepicker({
            format: 'MM',
            viewMode: 'months',
            minViewMode: 'months',
        }).on('changeDate', function (e) {
            $(this).datepicker('hide');
            var newDate = $.datepicker.formatDate('MM/yy', new Date(e.date));
            if (newDate != $('#dtpMonthlyProgressUpdate').text()) {
                $('#dtpMonthlyProgressUpdate').text(newDate);
                CCMProgramUpdate.LoadCCMProgramUpdate(false);
            }
        });
        jQuery("#ddlTaskReason").find("option:contains('CCM Program')").each(function () {
            if (jQuery(this).text() == 'CCM Program') {
                jQuery(this).attr("selected", "selected");
            }
        });
    },

    BackDate: function () {

        var criteria = $('#dtpMonthlyProgressUpdate').text();
        var d = new Date(CCMProgramUpdate.FormatDate(criteria));
        var newdate = CCMProgramUpdate.AddSubMonth(d, -1);
        var curdte = $.datepicker.formatDate('MM/yy', newdate);
        $('#dtpMonthlyProgressUpdate').text(curdte);
        CCMProgramUpdate.LoadCCMProgramUpdate(true);
    },

    FormatDate: function (date) {
        var obj = { January: 1, February: 2, March: 3, April: 4, May: 5, June: 6, July: 7, August: 8, September: 9, October: 10, November: 11, December: 12 };
        var temp = date.split('/');
        return temp[1] + "/" + temp[0];
    },

    NextDate: function () {

        var criteria = $('#dtpMonthlyProgressUpdate').text();
        var d = new Date(CCMProgramUpdate.FormatDate(criteria));
        var newdate = CCMProgramUpdate.AddSubMonth(d, 1);
        var curdte = $.datepicker.formatDate('MM/yy', newdate);
        $('#dtpMonthlyProgressUpdate').text(curdte);
        CCMProgramUpdate.LoadCCMProgramUpdate(true);
    },

    AddSubMonth: function (theDate, month) {

        return new Date(theDate.setMonth(theDate.getMonth() + month));

    },
    // Date Functions (end)

    FillCCMProgramUpdate: function (IsLastVisit) {
        var objData = new Object();
        var self = $("#" + CCMProgramUpdate.params.PanelID + " #divProgramUpdates");
        if (IsLastVisit) {

            var date = new Date();
            var ProgressMonth = date.getMonth();
            if (ProgressMonth == 0) {
                ProgressMonth = 12;
            }
            var ProgressYear = date.getFullYear();
            if (ProgressMonth == 12) {
                ProgressYear--;
            }
        }
        else {

            //var date = new Date(self.find("#dtpMonthlyProgressUpdate").text());
            //var ProgressMonth = date.getMonth() + 1;
            //var ProgressYear = date.getFullYear();
            var monthsForFilter = [
                       'January', 'February', 'March', 'April', 'May',
                       'June', 'July', 'August', 'September',
                       'October', 'November', 'December'
            ];
            var FilteredMonth = "";
            var DDateToFilter = self.find("#dtpMonthlyProgressUpdate").text().split("/");
            if (DDateToFilter.length > 0)
                 FilteredMonth = monthsForFilter.indexOf(DDateToFilter[0]) ? monthsForFilter.indexOf(DDateToFilter[0]) + 1 : 0;


            var ProgressMonth = FilteredMonth;
            var ProgressYear = DDateToFilter.length >= 1 ? DDateToFilter[1] : 0;
        }

        if (ProgressMonth == "NaN" || ProgressMonth == NaN || ProgressMonth == null || ProgressMonth == "") {
            var date_ = new Date();
            ProgressMonth = date_.getMonth() + 1;
            ProgressYear = date_.getFullYear();
        }


        objData["EnrollmentInfoId"] = CCMProgramUpdate.params.EnrollmentInfoId;
        objData["PatientId"] = CCMProgramUpdate.params.PatientId;
        objData["ProgressMonth"] = ProgressMonth;
        objData["ProgressYear"] = ProgressYear;


        var data = JSON.stringify(objData);
        return MDVisionService.CCMAPIService(data, "CCMProgramUpdate", "FillCCMProgramUpdate");
    },

    OpenLastVisit: function () {

        var EnrollmentInfoId = CCMProgramUpdate.params.EnrollmentInfoId;

        var self = $("#" + CCMProgramUpdate.params.PanelID + " #divProgramUpdates");
        self.find("#divInitialPlanDetails").hide();
        self.find("#divLastMonthProgress").show();

        CCMProgramUpdate.FillCCMProgramUpdate(true).done(function (response) {
            if (response.status != false) {
                CCMProgramUpdate.BindLastVisit(response).done(function () {
                    //successfully bind
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });



    },

    OpenInitialPlanDetails: function () {

        var EnrollmentInfoId = CCMProgramUpdate.params.EnrollmentInfoId;

        var self = $("#" + CCMProgramUpdate.params.PanelID + " #divProgramUpdates");
        self.find("#divInitialPlanDetails").show();
        self.find("#divLastMonthProgress").hide();


    },

    searchLoadTimer: function (TaskTimer) {
        var DurationTime = 0;
        CCMCallDetailsHistory.SearchCCMCallDetailsHistory(PageNo, rpp).done(function (response) {
            if (response.status != false) {
                if (response.CCMCallDetailsCount > 0) {
                    var TaskHours = 0, TaskMinutes = 0, TaskSeconds = 0;
                    var CCMCallDetailsHistoryJSONData = response.CCMTaskTimer_JSON;
                    $.each(CCMCallDetailsHistoryJSONData, function (i, item) {
                        if (item.Duration != null) {
                            if (item.DurationUnit == "seconds") {
                                TaskSeconds += Number(item.Duration);
                            } else if (item.DurationUnit == "minutes") {
                                TaskMinutes += Number(item.Duration);
                            } else if (item.DurationUnit == "hours") {
                                TaskHours += Number(item.Duration);
                            }
                        }
                    });
                    DurationTime = (TaskHours * 60) + (TaskMinutes) + (TaskSeconds / 60);
                }
            }
            TaskTimer += DurationTime;
            CCMProgramUpdate.progressCircle(TaskTimer);
        });
    },

    LoadCCMTaskTime: function () {
        var TaskTimer = 0;
        CCMProgramUpdate.LoadCCMTaskTime_DbCall().done(function (response) {
            if (response.status != false) {
                if (response.CCMTaskTimerCount > 0) {
                    var CCMTaskTimerHistoryJSONData = response.CCMTaskTimer_JSON;
                    $.each(CCMTaskTimerHistoryJSONData, function (i, item) {
                        TaskTimer += Number(Number(item.TaskDuration).toFixed(globalAppdata.DecimalPlaces));
                    });
                }
                CCMProgramUpdate.searchLoadTimer(TaskTimer);
            } else {
                CCMProgramUpdate.searchLoadTimer(TaskTimer);
            }
        });
    },

    LoadCCMTaskTime_DbCall: function () {
        return CCMTaskTimerHistory.SearchCCMTaskTimerHistory();
    },

    validateComments: function () {
        $("#" + CCMProgramUpdate.params["PanelID"] + ' #frmTaskTimer').bootstrapValidator('destroy');

        $("#" + CCMProgramUpdate.params["PanelID"] + ' #frmTaskTimer')
        .bootstrapValidator({
            live: 'disabled',
            message: 'This value is not valid',
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {

                Comments: {
                    group: '#divTTComments',
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
            CCMProgramUpdate.SaveTaskTime();
        });
    },

    SaveTaskTime: function () {
        var self = $("#" + CCMProgramUpdate.params["PanelID"] + " #divProgramUpdates");
        var strMessage = "";
        var myJSON = self.getMyJSONByName();
        var objData = JSON.parse(myJSON);

        // CCM-328
        if (CCMProgramUpdate.ValidateComments) {
            if ($("#TaskTimerComment").val() == "") {
                utility.DisplayMessages("Please add comment.", 3);
                return;
            }
        }

        //fixme add priviliges
        //  AppPrivileges.GetFormPrivileges("Chronic Care Management", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            CCMProgramUpdate.TaskTimeSave(myJSON).done(function (response) {
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    CCMProgramUpdate.LoadCCMProgramUpdate(false);
                    utility.ClearFormValidation("#" + CCMProgramUpdate.params["PanelID"] + ' #frmTaskTimer', true);
                    $("#" + CCMProgramUpdate.params["PanelID"] + ' #frmTaskTimer')[0].reset();
                    CCMProgramUpdate.ResetTaskTime();
                    var self = $("#" + CCMProgramUpdate.params["PanelID"] + " #divProgramUpdates");
                    self.find("#txtAddedBy").val(globalAppdata.AppUserNameFullName);
                    //$("#txtTaskReason").val("CCM Program");
                    CCM_Patient_Hub.IshubChanged = false;
                    $('#frmTaskTimer #dtpCallDate').datepicker('setDate', new Date());
                    $('#frmTaskTimer #txtCallTime').timepicker('setTime', new Date().toLocaleTimeString());
                    $("#ReasonComments").addClass('hidden');
                    CCMProgramUpdate.LoadCCMTaskTime();
                    CCMProgramUpdate.retainFixedCallerValues();
                    CCMProgramUpdate.ValidateComments = false;
                }
                else
                    utility.DisplayMessages(response.Message, 3);
            });
        }
        else
            utility.DisplayMessages(strMessage, 2);
        // });
    },

    TaskTimeSave: function (TaskTimeData) {
        var objData = JSON.parse(TaskTimeData);
        objData["EnrollmentInfoId"] = CCMProgramUpdate.params.EnrollmentInfoId;
        objData["PatientId"] = CCMProgramUpdate.params.PatientId;
        objData["TaskReason"] = objData["TaskReason_text"];
        objData["Comments"] = $("#TaskTimerComment").val();
        objData["TaskTime"] = objData.CallTime;
        objData["TaskDate"] = objData.CallDate;
        objData["CallerType"] = $("#ddlCaller option:selected").attr('refname');
        objData["ReasonId"] = $("#ddlTaskReason option:selected").val();
        objData["Caller"] = $("#ddlCaller option:selected").val();

        if (CCM_Patient_Hub.params.ProviderName == "" || CCM_Patient_Hub.params.ProviderName == undefined || CCM_Patient_Hub.params.ProviderName == 'undefined' || CCM_Patient_Hub.params.ProviderName == null)
            CCM_Patient_Hub.params.ProviderName = $("#ddlCaller option:selected").text();


        var data = JSON.stringify(objData);
        return MDVisionService.CCMAPIService(data, "CCMProgramUpdate", "SaveCCMTaskTime");
    },

    //TIMER FUNCTIONS START
    RecordTaskTime: function () {
        var self = $("#" + CCMProgramUpdate.params["PanelID"] + " #divProgramUpdates");
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

        CCMProgramUpdate.ticker = setInterval(function () {

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
        CCM_Patient_Hub.IshubChanged = true;
    },

    StopTaskTime: function () {
        var self = $("#" + CCMProgramUpdate.params["PanelID"] + " #divProgramUpdates");
        clearInterval(CCMProgramUpdate.ticker);
        self.find("#btnRecord").removeClass("disableAll")
        self.find("#btnLogTime").removeClass("disableAll")
        self.find("#btnReset").removeClass("disableAll")
        self.find("#txtTaskHours").removeClass("disableAll");
        self.find("#txtTaskMinutes").removeClass("disableAll");
        self.find("#txtTaskSeconds").removeClass("disableAll");
        CCM_Patient_Hub.IshubChanged = true;
    },

    ResetTaskTime: function () {
        var self = $("#" + CCMProgramUpdate.params["PanelID"] + " #divProgramUpdates");
        self.find("#txtTaskHours").val(0);
        self.find("#txtTaskMinutes").val(0);
        self.find("#txtTaskSeconds").val(0);
        CCM_Patient_Hub.IshubChanged = true;
    },
    //TIMER FUNCTIONS END

    SaveCallDetails: function () {
        var self = $("#" + CCMProgramUpdate.params["PanelID"] + " #divProgramUpdates");
        var strMessage = "";
        var myJSON = self.getMyJSONByName();
        //fixme add priviliges
        //  AppPrivileges.GetFormPrivileges("Chronic Care Management", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            CCMProgramUpdate.CallDetailsSave(myJSON).done(function (response) {
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    utility.ClearFormValidation("#" + CCMProgramUpdate.params["PanelID"] + ' #frmTaskTimer', true);
                    $("#" + CCMProgramUpdate.params["PanelID"] + ' #frmTaskTimer')[0].reset();
                    CCMProgramUpdate.LoadCCMTaskTime();
                    CCM_Patient_Hub.IshubChanged = false;
                    $('#frmTaskTimer #dtpCallDate').datepicker('setDate', new Date());
                    $('#frmTaskTimer #txtCallTime').timepicker('setTime', new Date().toLocaleTimeString());
                    CCMProgramUpdate.retainFixedCallerValues();
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

    CallDetailsSave: function (CallDetailsData) {
        var objData = JSON.parse(CallDetailsData);
        objData["EnrollmentInfoId"] = CCMProgramUpdate.params.EnrollmentInfoId;
        objData["PatientId"] = CCMProgramUpdate.params.PatientId;
        objData["CallerType"] = $("#ddlCaller option:selected").attr('refname');
        objData["CallReason"] = objData["CallReason_text"];
        var data = JSON.stringify(objData);
        return MDVisionService.CCMAPIService(data, "CCMProgramUpdate", "SaveCCMCallDetails");
    },

    OpenTaskTimerHistory: function () {

        var params = [];
        params["ParentCtrl"] = 'CCM_Patient_Hub';
        params["EnrollmentInfoId"] = CCMProgramUpdate.params.EnrollmentInfoId;;
        params["PatientId"] = CCMProgramUpdate.params.PatientId;;
        params["FromAdmin"] = "0";

        LoadActionPan('CCMTaskTimerHistory', params);
    },

    OpenCallHistory: function () {
        var EnrollmentInfoId = CCMProgramUpdate.params.EnrollmentInfoId;

        var params = [];
        params["ParentCtrl"] = 'CCM_Patient_Hub';
        params["EnrollmentInfoId"] = EnrollmentInfoId;
        params["FromAdmin"] = "0";
        params["PatientId"] = CCMProgramUpdate.params.PatientId;

        LoadActionPan('CCMCallDetailsHistory', params);
    },

    OpenProgressUpdateHistory: function (ProgressCategoryId) {
        var params = [];
        params["ParentCtrl"] = 'CCM_Patient_Hub';
        params["EnrollmentInfoId"] = CCMProgramUpdate.params.EnrollmentInfoId;;
        params["PatientId"] = CCMProgramUpdate.params.PatientId;
        params["ProgressCategoryId"] = ProgressCategoryId;
        params["FromAdmin"] = "0";
        LoadActionPan('CCMProgressUpdateHistory', params);

    },

    SaveProgramUpdate: function () {

        var selfInitialPlanDetails = $("#" + CCMProgramUpdate.params["PanelID"] + " #divProgramUpdates #divInitialPlanDetails");
        var selfMonthlyProgress = $("#" + CCMProgramUpdate.params["PanelID"] + " #divProgramUpdates #divMonthlyProgressUpdate");

        var strMessage = "";
        var InitialPlanDetailsJSON = selfInitialPlanDetails.getMyJSONByName();
        var MonthlyProgressJSON = selfMonthlyProgress.getMyJSONByName();
        var MergedJSON = utility.MergeJSON(InitialPlanDetailsJSON, MonthlyProgressJSON);

        //fixme add priviliges
        //  AppPrivileges.GetFormPrivileges("Chronic Care Management", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            CCMProgramUpdate.ProgramUpdateSave(MergedJSON).done(function (response) {
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    CCMProgramUpdate.LoadCCMProgramUpdate(false);
                    CCM_Patient_Hub.IshubChanged = false;

                    if (CCMProgramUpdate.params["ParentCtrl"] == 'clinicalTabProgressNote' || CCMProgramUpdate.params.IsFromNote) {
                        CCMProgramUpdate.AddToNote();
                    }
                }
                else
                    utility.DisplayMessages(response.Message, 3);
            });
        }
        else
            utility.DisplayMessages(strMessage, 2);
        // });
    },

    ProgramUpdateSave: function () {

        var dfd = $.Deferred();
        var self = $("#" + CCMProgramUpdate.params["PanelID"] + " #divProgramUpdates");
        var objData = new Object();
        var isChanged = false;

        $.each(self.find("[data-ProgressCategoryId]"), function (i, item) {
            if (i < 4) {
                if ($(item).val() != CCMProgramUpdate.ProgramUpdateFields[$(item).attr("data-ProgressCategoryId")]) {
                    objData[$(item).attr("name")] = $(item).val();
                    isChanged = true;
                }
                else {
                    objData[$(item).attr("name")] = $(item).val();
                    isChanged = true;
                }
            }
            else {
                if ($(item).val() != CCMProgramUpdate.ProgramUpdateFields[$(item).attr("data-ProgressCategoryId")]) {
                    objData[$(item).attr("name")] = $(item).val();
                    isChanged = true;
                }
            }
        });

        objData["EnrollmentInfoId"] = CCMProgramUpdate.params.EnrollmentInfoId;
        objData["PatientId"] = CCMProgramUpdate.params.PatientId;

        //var date = new Date(self.find("#dtpMonthlyProgressUpdate").text());
        //var ProgressMonth = date.getMonth() + 1;
        //var ProgressYear = date.getFullYear();
        //var date = new Date(self.find("#dtpMonthlyProgressUpdate").text());

        var monthsForFilter = [
                       'January', 'February', 'March', 'April', 'May',
                       'June', 'July', 'August', 'September',
                       'October', 'November', 'December'
        ];
        var FilteredMonth = "";
        var DDateToFilter = self.find("#dtpMonthlyProgressUpdate").text().split("/");
        if (DDateToFilter.length > 0)
            FilteredMonth = monthsForFilter.indexOf(DDateToFilter[0]) ? monthsForFilter.indexOf(DDateToFilter[0]) + 1 : 0;


        var ProgressMonth = FilteredMonth;
        var ProgressYear = DDateToFilter.length >= 1 ? DDateToFilter[1] : 0;

        if (ProgressMonth == "NaN" || ProgressMonth == NaN || ProgressMonth == null || ProgressMonth == "") {
            var date_ = new Date();
            ProgressMonth = date_.getMonth() + 1;
            ProgressYear = date_.getFullYear();
            
        }

        objData["ProgressMonth"] = ProgressMonth == "" ? 0 : ProgressMonth;
        objData["ProgressYear"] = ProgressYear == "" ? 0 : ProgressYear;


        var data = JSON.stringify(objData);

        if (!isChanged) {

            var t = { status: false, Message: 'Nothing to save' };
            dfd.resolve(t);

            return dfd;
        }

        return MDVisionService.CCMAPIService(data, "CCMProgramUpdate", "SaveProgramUpdate");
    },
    //openProgramUpdateFromNote: function (EnrollmentInfoId, PatientId, NoteId) {
    openProgramUpdateFromNote: function (EnrollmentInfoId, PatientId) {
        var IsFromNote = true;
        Patient_Demographic.OpenCCMHub(EnrollmentInfoId, PatientId, IsFromNote);
    },

    isFromNote: function () {
        $("#divTaskTimer").addClass('disableAll');
        $("#divInitialPlanDetails textarea").attr('disabled', true);
        $(".ccmControls").addClass('disableAll');
        $("#PatientHubExport").addClass('disableAll');
        $(".ccmControls").addClass('disableAll');
        $($("#programUpdateFooter button")[1]).attr('disabled', true);
        $($("#programUpdateFooter button")[2]).attr('disabled', true);
        $($("#programUpdateFooter button")[3]).attr('disabled', true);
    },

    returnheader: function (headerTitle, id) {
        var IsFreakinfPatch = '00000';
        return '<header>' +
       '<CCMPrgresssUpdate title="' + headerTitle + '"  id="' + id + '" class="CCM_NotesComponent">' +
       '<a class="btn btn-link btn-xs" onclick="CCMProgramUpdate.openProgramUpdateFromNote(' + CCMProgramUpdate.params.EnrollmentInfoId + ',' + CCMProgramUpdate.params.PatientId + ', ' + IsFreakinfPatch + ');" title="' + headerTitle + '">' + headerTitle + '</a> ' +
          '</CCMPrgresssUpdate> </header>';
    },

    createProgramUpdateHTML: function (response, NoteHTMLCtrl, ProblemListsId, hideAlertMessage) {
        var ProgramUpdateFields = CCMProgramUpdate.ProgramUpdateFields;

        var $mainDivVital = $(document.createElement('div'));

        $.each(ProgramUpdateFields, function (index, item) {
            if (index > 4) {
                var CategoryName = CCMProgramUpdate.getProgressCategoryNameById(index);

                var $SectionBodyVital = $(document.createElement('section'));
                $SectionBodyVital.attr('id', "Cli_Goals_Main" + index);
                var $List = $(document.createElement('ul'));
                var $ListItem = $(document.createElement('li'));
                $List.attr('class', 'list-unstyled CCMGoalsComponent')
                $List.attr('name', 'CCM').attr('NoteComponentId', 'NCDummyId');
                $ListItem.append(CCMProgramUpdate.returnheader(CategoryName, CCMProgramUpdate.params.EnrollmentInfoId));
                $SectionBodyVital.append(item == null ? "" : item);
                $ListItem.append($SectionBodyVital);
                $List.append($ListItem);
                $mainDivVital.append($List);
            }
        });

        return $mainDivVital.html();
    },
    updateProgramUpdateHTML: function (response, NoteHTMLCtrl, ProblemListsId, hideAlertMessage) {
        var ProgramUpdateFields = CCMProgramUpdate.ProgramUpdateFields;

        var $mainDivVital = $(document.createElement('div'));
        var NoteHTMLCtrl = '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML'
        $.each(ProgramUpdateFields, function (index, item) {
            if (index > 4) {
                var CategoryName = CCMProgramUpdate.getProgressCategoryNameById(index);

                var $SectionBodyVital = $(document.createElement('section'));
                $SectionBodyVital.attr('id', "Cli_Goals_Main" + index);
                var $List = $(document.createElement('ul'));
                var $ListItem = $(document.createElement('li'));
                $List.attr('class', 'list-unstyled')
                $List.attr('name', 'CCM');
                $ListItem.append(CCMProgramUpdate.returnheader(CategoryName, CCMProgramUpdate.params.EnrollmentInfoId));
                $SectionBodyVital.append(item == null ? "" : item);
                $ListItem.append($SectionBodyVital);
                $List.append($ListItem);

                if ($(NoteHTMLCtrl).find('#Cli_Goals_Main' + index).length == 0) {
                    // $mainDivVital.append($List);
                    $(NoteHTMLCtrl).append($List);
                } else {
                    $(NoteHTMLCtrl).find('#Cli_Goals_Main' + index).html($SectionBodyVital.html());
                }
            }
        });

        return $mainDivVital.html();
    },

    GetVisitReasons: function (dfd) {
        var arrayReason = [];
        var arrayComments = [];
        //sp_TaskTimerDetailsSelect
        CCMTaskTimerHistory.SearchCCMTaskTimerHistory(1, 1000).done(function (response) {
            if (response.status != false) {
                var reasonComm = "";
                var CCMTaskTimerHistoryJSONData = response.CCMTaskTimer_JSON;
                $.each(CCMTaskTimerHistoryJSONData, function (i, item) {
                    arrayReason.push(item.TaskReason);
                    arrayComments.push(item.Comments);
                });
                CCMProgramUpdate.VisitReason = arrayReason.join(", ");
                CCMProgramUpdate.VisitComments = arrayComments.join(", ");
                if (dfd) dfd.resolve();
            }
        });
        CCMCallDetailsHistory.SearchCCMCallDetailsHistory(1, 1000).done(function (response) {
            if (response.status != false) {
                var reasonComm = "";
                var CCMCallDetailsHistoryJSONData = response.CCMTaskTimer_JSON;
                $.each(CCMCallDetailsHistoryJSONData, function (i, item) {
                    arrayReason.push(item.CallReason);
                });
                CCMProgramUpdate.VisitReason = arrayReason.join(", ");
                CCMProgramUpdate.VisitComments = arrayComments.join(", ");
                if (dfd) dfd.resolve();
            }
            else {
                if (dfd) dfd.resolve();
            }
        });
    },

    PhoneEncounterLoad: function () {

        var dfd = new $.Deferred();
        CCMProgramUpdate.GetVisitReasons(dfd);
        dfd.done(function (n) {
            CCMProgramUpdate.LoadPhoneEncounter();
        });
    },

    LoadPhoneEncounter: function () {

        // CCMProgramUpdate.GetVisitReasons();

        if (CCMProgramUpdate.params.IsFromNote) {
            var Duration = $('#' + CCMProgramUpdate.params.PanelID + ' #MUStagePassedProgressBar span').text().split('Total Time').join('');
            Clinical_PhoneEncounter.setDurationTimeOnNote(Duration, Clinical_ProgressNote.params["PanelID"]);
            Clinical_Procedures.ProceduresSaveCCM(Clinical_ProgressNote.params.Program);

            CCMProgramUpdate.updateProgramUpdateHTML();
            CCM_Patient_Hub.UnLoad();
            Clinical_ProgressNote.saveCCMProgramComponents(true);
        } else {
            //successfully binded
            var EnrollmentInfoId = CCMProgramUpdate.params.EnrollmentInfoId;
            var PatientId = CCMProgramUpdate.params.PatientId;
            // var ProgramUpdateFields = CCMProgramUpdate.ProgramUpdateFields;
            var ProgramUpdateHTML = CCMProgramUpdate.createProgramUpdateHTML();

            var Duration = $('#' + CCMProgramUpdate.params.PanelID + ' #MUStagePassedProgressBar span').text().split('Total Time').join('');//+ ":" + $('#' + CCMProgramUpdate.params.PanelID + ' #txtTaskMinutes').val() + ":" + $('#' + CCMProgramUpdate.params.PanelID + ' #txtTaskSeconds').val();

            //fixme add promise
            CCM_Patient_Hub.UnLoad();
            $.when(setPatientBanner(PatientId, "1")).then(function () {

                if (CCMProgramUpdate.VisitReason != "")
                    CCMProgramUpdate.VisitReason = CCMProgramUpdate.VisitReason.replace(/ ,+/g, '');


                if (CCMProgramUpdate.VisitReason != "")
                    CCMProgramUpdate.VisitReason = CCMProgramUpdate.VisitReason.replace(/^,/, '');

                if (CCMProgramUpdate.VisitComments != "")
                    CCMProgramUpdate.VisitComments = CCMProgramUpdate.VisitComments.replace(/^,/, '');

                params["mode"] = "Add";
                params["PatientId"] = PatientId;
                params["ParentCntrlLoadid"] = "Dashboard";
                params['ForProgressNote'] = false;
                params['EnrollmentInfoId'] = EnrollmentInfoId;
                params['ProgramUpdateHTML'] = ProgramUpdateHTML;
                params['IsPhoneEncounter'] = true;
                params['FromCCM'] = true;
                params['CCMDuration'] = Duration;
                params['VisitReason'] = $("#ddlTaskReason option:selected").text();
                params['Caller'] = CCM_Patient_Hub.params.ProviderName;
                params['Receiver'] = CCM_Patient_Hub.params.PatientName;
                params['Program'] = CCM_Patient_Hub.params.Program;
                params['TaskComments'] = CCMProgramUpdate.VisitComments;
                params['TaskVisitReason'] = CCMProgramUpdate.VisitReason;


                //if (CCMProgramUpdate.params.NotesId != null & CCMProgramUpdate.params.NotesId > 0) {
                //    params['NoteId'] = CCMProgramUpdate.params.NotesId;
                //    params['ForProgressNote'] = true;
                //}

                var strMessage = "";
                AppPrivileges.GetFormPrivileges("Notes_Notes", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {

                        $("ul li[id*=mstrMenu]").hide();

                        if ($("html").hasClass("sidebar-left-collapsed")) {
                            $("html").removeClass("sidebar-left-collapsed");
                        }

                        $("#anchorMainMenu").show();
                        $("div[id*=mstrDiv]").hide();

                        $.when(ClinicalMenuSettings.ClinicalMenuSettingsSearch(null)).then(function () {

                            $('#mstrTabClinical').siblings().removeClass('active');
                            $('#mstrTabClinical').addClass('active');
                            $('#ClinicalUL li').removeClass('nav-expanded nav-active');
                            $('#ClinicalUL li#clinicalMenuNotes').addClass('nav-expanded nav-active');
                            $('#ctrlPanDashBoard').hide();

                            EMRUtility.unSelectOtherTabs('mstrTabClinical', 'false');


                            javascript: ClinicalMenuClick(event, function () {
                                $.when(ClinicalMenuSettings.TopButtons('clinicalMenuNotes')).then(function () {
                                    ClinicalMenuSettings.selectClinicalMenu('clinicalMenuNotes');
                                    SelectTab("clinicalTabPhoneEncounter", "false");
                                });
                            }, 0, this, 'clinicalMenuNotes', 'li');
                            try {
                                $('#ctrlPanPatient').css('display', 'none');
                                document.getElementById("ctrlPanPatient").style.display = "none !important";
                                $('#ctrlPanPatient').css('display', 'none !important');
                            } catch (ex) {
                                console.log(ex);
                            }
                        });
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
            });
        }
    },

    AddToNote: function () {

        if (CCM_Patient_Hub.params.ProviderName == "" || CCM_Patient_Hub.params.ProviderName == undefined || CCM_Patient_Hub.params.ProviderName == 'undefined' || CCM_Patient_Hub.params.ProviderName == null)
            CCM_Patient_Hub.params.ProviderName = $("#ddlCaller option:selected").text();

        CCM_Patient_Hub.IshubChanged = false;

        var selfInitialPlanDetails = $("#" + CCMProgramUpdate.params["PanelID"] + " #divProgramUpdates #divInitialPlanDetails");
        var selfMonthlyProgress = $("#" + CCMProgramUpdate.params["PanelID"] + " #divProgramUpdates #divMonthlyProgressUpdate");

        var InitialPlanDetailsJSON = selfInitialPlanDetails.getMyJSONByName();
        var MonthlyProgressJSON = selfMonthlyProgress.getMyJSONByName();

        var MergedJSON = utility.MergeJSON(InitialPlanDetailsJSON, MonthlyProgressJSON);
        CCMProgramUpdate.ProgramUpdateSave(MergedJSON).done(function (response) {
            if (response.status != false) {
                CCMProgramUpdate.FillCCMProgramUpdate().done(function (response) {
                    if (response.status != false) {
                        CCMProgramUpdate.BindProgramUpdate(response).done(function () {
                            CCMProgramUpdate.PhoneEncounterLoad();
                        });
                    }
                    else {
                        CCMProgramUpdate.LoadCCMTaskTime();
                        for (var i = 1; i <= 8; i++)
                            $("[data-ProgressCategoryId='" + i + "']").val('');
                    }
                });

            } else {
                if (response.Message == "Nothing to save") {
                    CCMProgramUpdate.PhoneEncounterLoad();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }

            }
        });

    },

    Export: function (isPrint) {

        //$('#' + CCMProgramUpdate.params["PanelID"] + " #btnCancel").hide();
        //$('#' + CCMProgramUpdate.params["PanelID"] + " #btnIAgree").hide();
        //$('#' + .params["PanelID"] + " #btnSign").hide();

        var dom = "";
        if (isPrint)
            dom = "programUpdatePrintDraw";
        else
            dom = "divProgramUpdates";

        kendo.drawing.drawDOM("#" + CCMProgramUpdate.params["PanelID"] + " #" + dom, {
            landscape: false,
            scale: 0.6,
            paperSize: "A4",
            margin: {
                left: "5mm",
                top: "5mm",
                right: "5mm",
                bottom: "20mm"
            },
        }).then(function (group) {

            if (isPrint) {
                kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                    var params = [];
                    params["PrintPDFDataURL"] = dataURL.split('data:application/pdf;base64,').join('');
                    params["PreviewPdf"] = true;
                    utility.PDFViewer(params["PrintPDFDataURL"], true, null, false, true);
                    //controls to hide
                });
            } else {

                kendo.drawing.pdf.saveAs(group, "CCMProgramUpdate.pdf");

            }
        });
    },

    progressCircle: function (TotalDuration) {
        if (CCM_Patient_Hub.params.Program == "Non-Complex") {
            if (TotalDuration != null && $.isNumeric(TotalDuration) && Number(TotalDuration) >= 20)
                $("#" + CCMProgramUpdate.params["PanelID"] + ' #btn-addToNote').removeClass('disableAll');
            else
                $("#" + CCMProgramUpdate.params["PanelID"] + ' #btn-addToNote').addClass('disableAll');

            var percentageCompleted = (TotalDuration * 100) / 20;
        }
        else if (CCM_Patient_Hub.params.Program == "Complex  (at least 60 minutes required)") {
            if (TotalDuration != null && $.isNumeric(TotalDuration) && Number(TotalDuration) >= 60)
                $("#" + CCMProgramUpdate.params["PanelID"] + ' #btn-addToNote').removeClass('disableAll');
            else
                $("#" + CCMProgramUpdate.params["PanelID"] + ' #btn-addToNote').addClass('disableAll');

            var percentageCompleted = (TotalDuration * 100) / 60;
        }
        else {
            if (TotalDuration != null && $.isNumeric(TotalDuration) && Number(TotalDuration) >= 20)
                $("#" + CCMProgramUpdate.params["PanelID"] + ' #btn-addToNote').removeClass('disableAll');
            else
                $("#" + CCMProgramUpdate.params["PanelID"] + ' #btn-addToNote').addClass('disableAll');

            var percentageCompleted = (TotalDuration * 100) / 20;
        }

        var minutes = Math.floor(TotalDuration)
        var fractionalPart = TotalDuration - Math.floor(TotalDuration)
        var seconds = Math.round(fractionalPart * 60);
        if (seconds == 60) {
            minutes = minutes + 1;
            seconds = 0;
        }

        var el = $("#" + CCMProgramUpdate.params["PanelID"] + " #divProgramUpdates").find(".progress-bar-circle");

        if (percentageCompleted >= 0 && percentageCompleted <= 100) {
            el.addClass("firstCycle");
            el.removeClass("secondCycle");
            el.removeClass("thirdCycle");
            el.attr("data-percent", percentageCompleted);
        }
        else if (percentageCompleted > 100 && percentageCompleted <= 200) {
            el.addClass("secondCycle");
            el.removeClass("firstCycle");
            el.removeClass("thirdCycle");
            el.attr("data-percent", percentageCompleted - 100);
        }
        else if (percentageCompleted > 200) {
            el.addClass("thirdCycle");
            el.removeClass("secondCycle");
            el.removeClass("firstCycle");
            el.attr("data-percent", percentageCompleted - 200);
        }

        $(el).each(function (count) {
            var options = {
                percent: this.getAttribute('data-percent') || 0,
                size: this.getAttribute('data-size') || 100,
                lineWidth: this.getAttribute('data-line') || 100,
                rotate: this.getAttribute('data-rotate') || 0,
                firstCycle: "#0088cc",
                secondCycle: '#ffa500',
                thirdCycle: '#0000ff',
                defaultClr: '#555555',
                remainCircleClor: '#efefef'
            }
            $(this).html('<canvas id="' + count + 'MUReport"></canvas>');
            var canvasID = $(this).children('canvas').attr("id");
            var canvas = document.getElementById(canvasID);
            var span = document.createElement('span');
            span.textContent = (minutes > 9 ? minutes : '0' + minutes) + ":" + (seconds > 9 ? seconds : '0' + seconds);
            if (typeof (G_vmlCanvasManager) !== 'undefined') {
                G_vmlCanvasManager.initElement(canvas);
            }

            var ctx = canvas.getContext('2d');
            canvas.width = canvas.height = options.size;

            var label = document.createElement('label');
            label.textContent = 'Total Time';
            var LINEBREAK = document.createElement('br');
            span.appendChild(LINEBREAK);
            span.appendChild(label);

            this.appendChild(span);
            this.appendChild(canvas);

            //settings
            $(this).height(options.size);
            $(this).width(options.size);
            //settings for span
            $(this).children("span").css("margin", "0px");
            $(this).children("span").css("margin-top", "40px");

            ctx.translate(options.size / 2, options.size / 2); // change center
            ctx.rotate((-1 / 2 + options.rotate / 180) * Math.PI); // rotate -90 deg
            //imd = ctx.getImageData(0, 0, 240, 240);
            var radius = (options.size - options.lineWidth) / 2;
            radius = radius < 0 ? 1 : radius;
            var drawCircle = function (color, lineWidth, percent) {
                percent = Math.min(Math.max(0, percent || 1), 1);
                ctx.beginPath();
                ctx.arc(0, 0, radius, 0, Math.PI * 2 * percent, false);
                ctx.strokeStyle = color;
                ctx.lineCap = 'square'; // butt, round or square
                ctx.lineWidth = lineWidth
                ctx.stroke();
            };


            //circle themes color
            if (options.percent === "0") {
                drawCircle(options.remainCircleClor, options.lineWidth, 100 / 100);
                return;
            }
            else if ($(this).hasClass("firstCycle")) {
                drawCircle(options.remainCircleClor, options.lineWidth, 100 / 100);
                drawCircle(options.firstCycle, options.lineWidth, options.percent / 100);
            }
            else if ($(this).hasClass("secondCycle")) {
                drawCircle(options.firstCycle, options.lineWidth, 100 / 100);
                drawCircle(options.secondCycle, options.lineWidth, options.percent / 100);

            }
            else if ($(this).hasClass("thirdCycle")) {
                drawCircle(options.secondCycle, options.lineWidth, 100 / 100);
                drawCircle(options.thirdCycle, options.lineWidth, options.percent / 100);
            }
            else {
                drawCircle(options.defaultClr, options.lineWidth, options.percent / 100);
            }

        });//each function
    },

    getProgressCategoryNameById: function (ProgressCategoryId) {
        var ProgressCategoryName = "";

        switch (ProgressCategoryId) {
            case 5:
                ProgressCategoryName = "Goals/Targets Achieved";
                break;
            case 6:
                ProgressCategoryName = "Progress reducing barriers";
                break;
            case 7:
                ProgressCategoryName = "Progress towards expected outcomes";
                break;
            case 8:
                ProgressCategoryName = "Other Information";
                break;
            default:
                ProgressCategoryName = "Progress Update History";
                break;
        }
        return ProgressCategoryName;

    },

    UnLoad: function () {

        if (CCMProgramUpdate.params != null && CCMProgramUpdate.params.ParentCtrl != null && CCMProgramUpdate.params.PanelID != 'divProgramUpdates') {
            UnloadActionPan(CCMProgramUpdate.params.ParentCtrl, 'divProgramUpdates', null, CCMProgramUpdate.params.PanelID);
        }

        else if (CCMProgramUpdate.params != null && CCMProgramUpdate.params.ParentCtrl != null) {
            UnloadActionPan(CCMProgramUpdate.params.ParentCtrl, 'CCMProgramUpdate');
        }

    },
}