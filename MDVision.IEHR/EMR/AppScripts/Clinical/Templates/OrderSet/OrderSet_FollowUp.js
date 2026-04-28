OrderSet_FollowUp = {
    bIsFirstLoad: true,
    params: [],
    IsAnyChange: false,
    //appid: "",
    Load: function (params) {
        OrderSet_FollowUp.params = params;
        OrderSet_FollowUp.IsAnyChange = false;

        var self = $('#pnlOrderSetFollowUpAppointment');
        self.loadDropDowns(true).done(function () {
            $('#pnlOrderSetFollowUpAppointment #txtFromTime').timepicker("setTime", "12:00 am");
            $('#pnlOrderSetFollowUpAppointment #txtToTime').timepicker("setTime", "12:15 am");
           
            if (OrderSet_FollowUp.params.mode == "Edit") {
                OrderSet_FollowUp.FillOrderSetFollowUp();
            }
        });

        utility.CreateDatePicker('pnlOrderSetFollowUpAppointment #fromDate', function () {

        }, true);

        $("#frmOrderSetFollowUpAppointment #btnSave").click(function (e) {
            OrderSet_FollowUp.FollowUpAppointmentSave();
        });

        utility.callbackAfterAllDOMLoaded(function () {

            //Serialization
            $('#' + OrderSet_FollowUp.params.PanelID + ' #frmOrderSetFollowUpAppointment').data('serialize', $('#' + OrderSet_FollowUp.params.PanelID + ' #frmOrderSetFollowUpAppointment').serialize());

        });

        OrderSet_FollowUp.SetUpFollowUpAppointmentEvents();
    },

    SetUpFollowUpAppointmentEvents: function () {

        $("#pnlOrderSetFollowUpAppointment #pnlFollowupAppointments input[type='radio']").on("click", function () {

            var soaptextValue = "";
            var cval = $(this).attr("cval");
            if (cval == "prn") {
                soaptextValue = "As per need.";
                OrderSet_FollowUp.params.ctype = "prn";
                OrderSet_FollowUp.params.cval = "";
                $("#pnlOrderSetFollowUpAppointment #chkCreateAppointment").attr("disabled", "disabled");
                $("#pnlOrderSetFollowUpAppointment #chkCreateAppointment").prop('checked', false);
            }
            else {
                $("#pnlOrderSetFollowUpAppointment #chkCreateAppointment").attr("disabled", false);
                var ctype = $(this).attr("ctype");
                var addDays = 0;
                var myDate = new Date();// new Date($("#pnlOrderSetFollowUpAppointment #fromDate").val());


                switch (ctype) {

                    case "Ds":
                        ctype = "Days";
                        addDays = parseInt(cval);
                        myDate.setDate(myDate.getDate() + addDays);
                        $("#pnlOrderSetFollowUpAppointment #fromDate").val($.datepicker.formatDate('mm/dd/yy', myDate));
                        break;
                    case "W":
                        parseInt(cval) > 1 ? ctype = "Weeks" : ctype = "Week";
                        addDays = parseInt(cval) * 7;
                        myDate.setDate(myDate.getDate() + addDays);
                        $("#pnlOrderSetFollowUpAppointment #fromDate").val($.datepicker.formatDate('mm/dd/yy', myDate));
                        break;
                    case "M":
                        parseInt(cval) > 1 ? ctype = "Months" : ctype = "Month";

                        myDate.setMonth(myDate.getMonth() + parseInt(cval));
                        $("#pnlOrderSetFollowUpAppointment #fromDate").val($.datepicker.formatDate('mm/dd/yy', myDate));
                        break;
                    case "Y":
                        parseInt(cval) > 1 ? ctype = "Years" : ctype = "Year";
                        myDate.setFullYear(myDate.getFullYear() + parseInt(cval));
                        $("#pnlOrderSetFollowUpAppointment #fromDate").val($.datepicker.formatDate('mm/dd/yy', myDate));
                        break;
                }

                soaptextValue = "Patient needs to be seen again in " + cval + " " + ctype;
                OrderSet_FollowUp.params.ctype = ctype;
                OrderSet_FollowUp.params.cval = cval;
            }
            if (soaptextValue != "") {
                $("#pnlOrderSetFollowUpAppointment #followUpText").html(soaptextValue);
                $("#pnlOrderSetFollowUpAppointment #txtComments").attr("rows", "3");
            }
            else {
                $("#pnlOrderSetFollowUpAppointment #followUpText").html("");
                $("#pnlOrderSetFollowUpAppointment #txtComments").attr("rows", "4");
                OrderSet_FollowUp.params.ctype = "";
                OrderSet_FollowUp.params.cval = "";
            }
        });

    },
    AddTime: function (obj) {

        var fromTimeObj = $("#txtFromTime");
        var toTimeObj = $("#txtToTime")

        var timeFrom = new Date("01/01/2007 " + $('#txtFromTime').val());
        timeFrom.setMinutes(timeFrom.getMinutes());
        $("#txtToTime").timepicker({ ampm: true });
        $("#txtToTime").timepicker("setTime", timeFrom);
        $("#txtToTime").val(OrderSet_FollowUp.formatAMPM(new Date("01/01/2007 " + $('#txtToTime').val())));
        //var timeEnd = new Date("01/01/2007 " + $('#txtToTime').val()).getMinutes();

    },
    formatAMPM: function (date) {
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var ampm = hours >= 12 ? 'PM' : 'AM';
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'
        minutes = minutes < 10 ? '0' + minutes : minutes;
        var strTime = hours + ':' + minutes + ' ' + ampm;
        return strTime;
    },
    FillOrderSetFollowUp: function () {
        OrderSet_FollowUp.FillOrderSetFollowUp_DBCall(OrderSet_FollowUp.params.FollowUpId,
            OrderSet_FollowUp.params.OrderSetId).done(function (response) {

                response = JSON.parse(response);
                if (response.status != false) {
                    var fillData = response.OrderSetFollowUpJSON[0];
                    $self = $('#' + OrderSet_FollowUp.params.PanelID)
                    utility.bindMyJSONByName(true, fillData, false, $self);
                    if (fillData.FollowUpText != "") {
                        //$("#pnlOrderSetFollowUpAppointment #followUpText").html(fillData.FollowUpText);
                        //$("#pnlOrderSetFollowUpAppointment #txtComments").attr("rows", "3");
                        try {
                            var temp = fillData.FollowUpText.split(" in");
                            if (temp.length > 1) {
                                var count = temp[1].trim().split(" ")[0];
                                var type = temp[1].trim().toLowerCase();
                                if (type.indexOf('day') >= 0) {
                                    $("#pnlOrderSetFollowUpAppointment #r"+count).click();
                                }
                                else if (type.indexOf('week') >= 0) {
                                    OrderSet_FollowUp.setFollowUpText(count, "W");
                                }
                                else if (type.indexOf('month') >= 0) {
                                    OrderSet_FollowUp.setFollowUpText(count, "M");
                                }
                                else if (type.indexOf('year') >= 0) {
                                    OrderSet_FollowUp.setFollowUpText(count, "Y");
                                }
                            } else if (temp[0] == "As per need.") {
                                $("#pnlOrderSetFollowUpAppointment #r12").click();
                            }
                        } catch (e) {
                            console.log(e.message);
                        }
                        
                    }
                    else {
                        $("#pnlOrderSetFollowUpAppointment #followUpText").html("");
                        $("#pnlOrderSetFollowUpAppointment #txtComments").attr("rows", "4");
                    }
                    
                }
                else
                    utility.DisplayMessages(response.Message, 3);

            })

    },

    setFollowUpText: function (value,type) {

        $("#pnlOrderSetFollowUpAppointment #pnlFollowupAppointments input[type='radio']").each(function (index, item) {
            if ($(this).attr("cval") == value && $(this).attr("ctype") == type) {
                $(this).click();
                return;
            }
        });
    },

    FillOrderSetFollowUp_DBCall: function (FollowUpId, OrderSetId, pageNo, rpp) {

        var objData = {};
        objData["PageNumber"] = pageNo == null ? 1 : pageNo;
        objData["RowspPage"] = rpp == null ? 1000 : rpp;
        objData["FollowUpId"] = FollowUpId;
        objData["OrderSetId"] = OrderSetId;
        objData["commandType"] = "fill_ordersetfollowup";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "OrderSetFollowUp");

    },

    FollowUpAppointmentSave: function () {
        var dfd = $.Deferred();
        var soapText = $("#pnlOrderSetFollowUpAppointment #followUpText").html();
        var comments = $("#pnlOrderSetFollowUpAppointment #txtComments").val();
        var valuestart = $('#pnlOrderSetFollowUpAppointment #txtFromTime').val();
        var valuestop = $('#pnlOrderSetFollowUpAppointment #txtToTime').val()
        var hourStart = new Date("01/01/2007 " + valuestart).getHours();
        var hourEnd = new Date("01/01/2007 " + valuestop).getHours();
        var minuteStart = new Date("01/01/2007 " + valuestart).getMinutes();
        var minuteEnd = new Date("01/01/2007 " + valuestop).getMinutes();
        var hourDiff = hourEnd - hourStart;
        hourDiff = hourDiff * 60;                                                                
        var minuteDiff = minuteEnd - minuteStart;
        minuteDiff = parseInt(hourDiff) > 0 ? parseInt(hourDiff) + parseInt(minuteDiff) : parseInt(minuteDiff);
        var duration = parseInt(minuteDiff);

      
        var objData = new Object();
        objData["Duration"] = duration.toString();
        objData["ProviderId"] = $('#pnlOrderSetFollowUpAppointment #Provider').val();
        objData["FacilityId"] = $('#pnlOrderSetFollowUpAppointment #Facility').val();
        objData["Date"] = $('#pnlOrderSetFollowUpAppointment #fromDate').val();
        objData["FromTime"] = $('#pnlOrderSetFollowUpAppointment #txtFromTime').val();
        objData["ToTime"] = $('#pnlOrderSetFollowUpAppointment #txtToTime').val();
        objData["Comments"] = $('#pnlOrderSetFollowUpAppointment #txtComments').val();
        objData["CreateAppointment"] = $("#pnlOrderSetFollowUpAppointment #chkCreateAppointment").is(':checked');

        var data = JSON.stringify(objData);

        if (soapText || comments) {
            var self = $("#pnlOrderSetFollowUpAppointment #frmOrderSetFollowUpAppointment");
            var myJSON = self.getMyJSONByName();
            OrderSet_FollowUp.FollowUpAppointmentSave_DBCall(data).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {

                    if (response.FollowUpId) {
                        OrderSet_FollowUp.params.FollowUpId = response.FollowUpId;
                    }

                    OrderSet_FollowUp.IsAnyChange = true;

                    utility.DisplayMessages(response.Message, 1);
                    OrderSet_FollowUp.UnLoad('saved');
                    dfd.resolve();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                    dfd.resolve();
                }
            });
        }
        else {
            dfd.resolve();
            utility.DisplayMessages("Please Select Follow Up Appointment.", 2);
        }
        return dfd;
    },

    FollowUpAppointmentSave_DBCall: function (followUpData) {

        var objData = JSON.parse(followUpData);
        if (OrderSet_FollowUp.params.mode == "Edit") {

            objData["commandType"] = "update_ordersetfollowup";
            objData["FollowUpId"] = OrderSet_FollowUp.params.FollowUpId;
            objData["OrderSetId"] = OrderSet_FollowUp.params.OrderSetId;
        } else {
            objData["OrderSetId"] = OrderSet_FollowUp.params.OrderSetId;
            objData["commandType"] = "save_ordersetfollowup";
        }
        objData["FollowUpText"] = $("#frmOrderSetFollowUpAppointment #followUpText").html();

        switch (OrderSet_FollowUp.params.ctype) {

            case "Days":
                
                objData["FollowUpDays"] = parseInt(OrderSet_FollowUp.params.cval);
                
                break;
            case "Weeks":
            case "Week":
                addDays = parseInt(OrderSet_FollowUp.params.cval) * 7;
                objData["FollowUpDays"] = addDays;
               
                break;
            case "Months":
            case "Month":
                objData["FollowUpMonths"] = parseInt(OrderSet_FollowUp.params.cval);
              
                break;
            case "Years":
            case "Year":
               
                objData["FollowUpYears"] = parseInt(OrderSet_FollowUp.params.cval);
               
                break;
        }

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "OrderSetFollowUp");

    },

    FollowUpAppointmentDelete_DBCall: function (FollowUpId) {

        var objData = new Object();
        objData["commandType"] = "delete_ordersetfollowup";
        objData["FollowUpId"] = FollowUpId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "OrderSetFollowUp");

    },

    UnLoad: function (status) {

        if (OrderSet_FollowUp.IsAnyChange == true && OrderSet_FollowUp.params["ParentCtrl"] == "Clinical_OrderSetDetails") {
            Clinical_OrderSetDetails.LoadOrderSetFollowUp();
        }

        UnloadActionPan(OrderSet_FollowUp.params["ParentCtrl"], "pnlOrderSetFollowUpAppointment");
    },
}