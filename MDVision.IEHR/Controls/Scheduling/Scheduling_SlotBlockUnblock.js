blckreasonDetail = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        blckreasonDetail.params = params;

        var self = $('#blckreasonDetail');
        self.loadDropDowns(true).done(function () {
            blckreasonDetail.LoadSlotBlockUnblock();

            //serialize Data.
            $('#frmblckreason').data('serialize', $('#frmblckreason').serialize());
        });
    },

    LoadSlotBlockUnblock: function () {
        if (blckreasonDetail.params.Status == "Blocked") {

            blckreasonDetail.ValidateBlockUnBlockReason(blckreasonDetail.params.Status, blckreasonDetail.params.slotids);
            $('#blckreasonDetail #titleBlockReason').html('Block');

        }
        else if (blckreasonDetail.params.Status == "UnBlocked") {

            blckreasonDetail.ValidateBlockUnBlockReason(blckreasonDetail.params.Status, blckreasonDetail.params.slotids);
            $('#blckreasonDetail #titleBlockReason').html('Unblock');

        }
    },

    ValidateBlockUnBlockReason: function (status, slotids) {
        $('#frmblckreason')
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
                   //schreason: {
                   //    group: '.col-sm-6',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //}
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            blckreasonDetail.BlockSlotSave(status, slotids);
        });
    },

    BlockSlotSave: function (status, slotids) {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Slot", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var blockreason;
                var comments;
                comments = $('#blckreasonDetail #txtComments').val();
                blockreason = $('#blckreasonDetail #hfSchReasonId').val();
                //if (blockreason != "") {
                blckreasonDetail.UpdateSlotStatus(slotids, blockreason, status, comments).done(function (response) {
                    if (response.status != false) {

                        utility.DisplayMessages(response.message, 1);

                        UnloadActionPan(blckreasonDetail.params["ParentCtrl"], "actionPanSlotBlockUnBlock");

                        var dayrgx = /(\d{2})\/(\d{2})\/(\d{4})/;
                        //expression for week range
                        var weekrg = /(\d{2})\/(\d{2})\/(\d{4}) - (\d{2})\/(\d{2})\/(\d{4})/;
                        //Month Regular Expression
                        var monthreg = /(\January|February|March|April|May|June|July|August|September|October|November|December)\/(\d{4})/;

                        //var numAlpha = /((\<Alpha>[a-zA-Z]*)(\<Numeric>[0-9]*))/;ParentCtrl

                        if (blckreasonDetail.params.PanelID == "pnlScheduleSearch" || blckreasonDetail.params.ScheduleSearch == "1") {

                            Scheduling_Search.ScheduleSearch(blckreasonDetail.params.CurrentPage, blckreasonDetail.params.RecordsPerPage);
                            $('#dgvScheduleSearch input[type=checkbox]').each(function () {
                                if ($(this).is(":checked")) {
                                    $(this).attr('checked', false);
                                }
                            });

                        }

                        else if (blckreasonDetail.params.MultipleView == "1") {

                            Scheduling_MuliView.BackDate(blckreasonDetail.params.DateId, blckreasonDetail.params.ProviderId, blckreasonDetail.params.ResourceId, blckreasonDetail.params.FacilityId, 0, null);

                        }

                        else if (blckreasonDetail.params.DayDate.match(weekrg) && blckreasonDetail.params.DayDate.length > 15) {
                            var a = $('#pnlScheduleCalendar #fromdate').datepicker('getDate');
                            Scheduling_Calendar.ClearTable();

                            var date1 = $.datepicker.formatDate('yy/mm/dd', new Date(a));
                            var week = Scheduling_Calendar.GetWeek(date1);
                            $('#pnlScheduleCalendar #daydate span').html(week);
                            //$('#pnlScheduleCalendar #fromdate').datepicker("setDate", date1);
                            var date2 = $.datepicker.formatDate('mm/dd/yy', new Date(a));
                            $('#pnlScheduleCalendar #fromdate').datepicker("setDate", date2);
                        }
                        else if (blckreasonDetail.params.DayDate.match(dayrgx) && blckreasonDetail.params.DayDate.length < 15) {
                            var statusslots = Scheduling_Calendar.FilterCriteria();
                            if ($("#pnlScheduleCalendar input:radio[name='RadioProvider']").is(":checked"))
                                Scheduling_Calendar.DayCalendar(blckreasonDetail.params.ProviderId, 0, blckreasonDetail.params.FacilityId, blckreasonDetail.params.DayDate, statusslots);
                            else if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked"))
                                Scheduling_Calendar.DayCalendar(0, blckreasonDetail.params.ResourceId, blckreasonDetail.params.FacilityId, blckreasonDetail.params.DayDate, statusslots);
                            //--

                            if (blckreasonDetail.params.ProviderId != "")
                                $("#pnlScheduleCalendar #Provider option[value=" + blckreasonDetail.params.ProviderId + "]").attr('selected', 'selected');
                            if (blckreasonDetail.params.ResourceId != "")
                                $("#pnlScheduleCalendar #Resource option[value=" + blckreasonDetail.params.ResourceId + "]").attr('selected', 'selected');
                            if (blckreasonDetail.params.FacilityId != "")
                                $("#pnlScheduleCalendar #Facility option[value=" + blckreasonDetail.params.FacilityId + "]").attr('selected', 'selected');
                            if (blckreasonDetail.params.DayDate != "")
                                $('#pnlScheduleCalendar #daydate span').html(blckreasonDetail.params.DayDate);

                            //--
                        }




                        if ($('#schEditSlot #btnBlockUnblock').html() == 'Unblock') {
                            $('#schEditSlot #btnBlockUnblock').html('Block');
                        }
                        else if ($('#schEditSlot #btnBlockUnblock').html() == 'Block') {
                            $('#schEditSlot #btnBlockUnblock').html('Unblock');
                        }

                    }
                    else {
                        UnloadActionPan(blckreasonDetail.params["ParentCtrl"], "actionPanSlotBlockUnBlock");
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
                //} else {
                //    utility.DisplayMessages("Reason not valid", 2);
                //    if ($('#frmblckreason').data('bootstrapValidator') != null && typeof $('#frmblckreason').data('bootstrapValidator') != 'undefined') {
                //        $('#frmblckreason').bootstrapValidator('revalidateField', 'schreason');
                //    }
                //}

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    UpdateSlotStatus: function (SlotId, BlockReasonId, BlockStatus, Comments) {
        var data = "SlotId=" + SlotId + "&BlockReasonId=" + BlockReasonId + "&BlockStatus=" + BlockStatus + "&Comments=" + Comments;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_BLOCK_UNBLOCK", "UPDATE_SLOT_STATUS");



    },

    UnLoad: function () {

        utility.UnLoadDialog('frmblckreason', function () {
            UnloadActionPan(blckreasonDetail.params["ParentCtrl"], "actionPanSlotBlockUnBlock");
        }, function () {
            UnloadActionPan(blckreasonDetail.params["ParentCtrl"], "actionPanSlotBlockUnBlock");
        });

    },

    BindScheduleReasons: function () {
        var SchReason = $('#blckreasonDetail #txtSchReason').val();
        var AllSchReasons = [];
        var dfd = new $.Deferred();
        if (SchReason.length > 2) {
            utility.Keyupdelay(function () {
                blckreasonDetail.LoadScheduleReasons(SchReason).done(function (responseData) {
                    if (responseData.status != false) {
                        if (responseData.SchReasonCount > 0) {
                            var ScheduleReasons = JSON.parse(responseData.ScheduleReasonsLoad_JSON);

                            $.each(ScheduleReasons, function (i, item) {
                                AllSchReasons.push({ id: item.ScheduleReasonId, value: item.ShortName, Duration: item.Duration });
                            });

                        }
                    }
                    dfd.resolve(AllSchReasons);
                });
            });
        } else {
            dfd.resolve(AllSchReasons);
        }

        //---------------
        dfd.then(function () {

            $('#blckreasonDetail #txtSchReason').autocomplete({
                autoFocus: true,
                source: AllSchReasons,
                open: function (event, ui) { disable = true },
                close: function (event, ui) {
                    disable = false; $(this).focus();
                },
                select: function (event, ui) {
                    setTimeout(function () {
                        $('#blckreasonDetail #txtSchReason').val(ui.item.value);
                        $('#blckreasonDetail #hfSchReasonId').val(ui.item.id);
                    }, 100);

                }
            }).blur(function () {
                setTimeout(function () {
                    utility.ValidateAutoComplete($('#blckreasonDetail #txtSchReason'), 'blckreasonDetail #hfSchReasonId', false, null, null, null);
                }, 200);
            });
            $('#blckreasonDetail #txtSchReason').autocomplete("search");

        });

        //--------------------
    },

    LoadScheduleReasons: function (SchReason) {

        var data = "SchReason=" + SchReason;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_BLOCK_UNBLOCK", "GET_SCHEDULE_REASONS");

    },

    OpenScheduleReason: function () {

        var params = [];
        params["ScheduleReasonId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "blckreasonDetail";
        LoadActionPan('Admin_ScheduleReason', params);

    },

    FillScheduleReason: function (ScheduleReasonId, ShortName, Duration, event) {

        if (event != null) {
            event.stopPropagation();
        }

        UnloadActionPan("blckreasonDetail");

        $('#blckreasonDetail #txtSchReason').val(ShortName);
        $('#blckreasonDetail #hfSchReasonId').val(ScheduleReasonId);

        if ($('#frmblckreason').data('bootstrapValidator') != null && typeof $('#frmblckreason').data('bootstrapValidator') != 'undefined') {
            $('#frmblckreason').bootstrapValidator('revalidateField', 'schreason');
        }


    },
}