providerscheduleDetail = {
    params: [],
    Load: function (params) {
        providerscheduleDetail.params = params;

        var self = $('#providerscheduleDetail');
        self.loadDropDowns(true).done(function () {
            self.find('#blockreason').html(self.find('#ddlschreason').html());

            providerscheduleDetail.LoadProviderScheduleDetail();
            providerscheduleDetail.LoadAllAutocomplete();
            providerscheduleDetail.BindBlockReasons();

            $('#providerscheduleDetail #txtevrydays').on("keyup input", function (event) {
                var days = parseInt($('#providerscheduleDetail #txtevrydays').val());
                if (days > 31) {
                    $('#providerscheduleDetail #txtevrydays').val('');
                    $('#frmproviderscheduleDetail').bootstrapValidator('revalidateField', 'DailyEveryDays');
                }
            });

            $('#providerscheduleDetail #txtmntActive').on("keyup input", function (event) {
                var days = parseInt($('#providerscheduleDetail #txtmntActive').val());
                if (days > 31) {
                    $('#providerscheduleDetail #txtmntActive').val('');
                    $('#frmproviderscheduleDetail').bootstrapValidator('revalidateField', 'txtmntActive');
                }
            });
        });
    },

    LoadProviderScheduleDetail: function () {
        if (providerscheduleDetail.params.mode == "Add") {
            providerscheduleDetail.SetFieldsDefault();

            //serialize Data after all controls loaded.
            $('#frmproviderscheduleDetail').data('serialize', $('#frmproviderscheduleDetail').serialize());
            providerscheduleDetail.ValidateproviderscheduleDetail();
        }
        else if (providerscheduleDetail.params.mode == "Edit") {
            $("#providerscheduleDetail #btnsaveProviderSch").hide();
            providerscheduleDetail.FillProviderSchedule(providerscheduleDetail.params.ScheduleId).done(function (response) {
                if (response.status != false) {
                    var providerSchedule_detail = JSON.parse(response.ProviderScheduleFill_JSON);
                    var self = $("#providerscheduleDetail");
                    utility.bindMyJSON(true, providerSchedule_detail, false, self).done(function () {


                        $("#providerscheduleDetail #frmproviderscheduleDetail :input").prop("disabled", true);
                        $("#providerscheduleDetail #frmproviderscheduleDetail #btnHistoryProSch").prop("disabled", false);

                        if (providerSchedule_detail.chkActive == 'True')
                            $("#providerscheduleDetail #chkActive").prop("checked", true);
                        else
                            $("#providerscheduleDetail #chkActive").prop("checked", false);

                        if (providerSchedule_detail.schfor == "Daily") {
                            $('.nav-tabs a[href="#Daily"]').tab('show');
                            if (providerSchedule_detail.patternevery == "True" && providerSchedule_detail.patterdays == "0") {
                                $('#providerscheduleDetail #Every').prop('checked', true);
                                $('#providerscheduleDetail #txtevrydays').val(providerSchedule_detail.value);
                            }
                            if (providerSchedule_detail.patternevery == "True" && providerSchedule_detail.patterdays != "0") {
                                $('#providerscheduleDetail #Every').prop('checked', true);
                                $('#providerscheduleDetail #txtevrydays').val(providerSchedule_detail.value);
                                $('#providerscheduleDetail #chkweekendsonly').prop('checked', true);
                            }
                            if (providerSchedule_detail.patternevery == "False") {

                                $('#providerscheduleDetail #rdevryweekend').prop('checked', true);
                            }
                        }
                        if (providerSchedule_detail.schfor == "Weekly") {

                            $('.nav-tabs a[href="#Weekly"]').tab('show');

                            if (providerSchedule_detail.patternevery == "False") {

                                $('#providerscheduleDetail #rdEveryWeekdayOn').prop('checked', true);
                                var s = providerSchedule_detail.patterdays;
                                var match = s.split(',')
                                var x = 0;
                                for (var a in match) {
                                    var variable = match[a];

                                    if (x == 0 && variable == "1") {
                                        $('#providerscheduleDetail #chkwekSunday').prop('checked', true);
                                    }
                                    if (x == 0 && variable == "0") {
                                        $('#providerscheduleDetail #chkwekSunday').prop('checked', false);
                                    }
                                    if (x == 1 && variable == "1") {
                                        $('#providerscheduleDetail #chkwekMonday').prop('checked', true);
                                    }
                                    if (x == 1 && variable == "0") {
                                        $('#providerscheduleDetail #chkwekMonday').prop('checked', false);
                                    }
                                    if (x == 2 && variable == "1") {
                                        $('#providerscheduleDetail #chkwekTuesday').prop('checked', true);
                                    }
                                    if (x == 2 && variable == "0") {
                                        $('#providerscheduleDetail #chkwekTuesday').prop('checked', false);
                                    }
                                    if (x == 3 && variable == "1") {
                                        $('#providerscheduleDetail #chkwekWednesday').prop('checked', true);
                                    }
                                    if (x == 3 && variable == "0") {
                                        $('#providerscheduleDetail #chkwekWednesday').prop('checked', false);
                                    }
                                    if (x == 4 && variable == "1") {
                                        $('#providerscheduleDetail #chkwekThursday').prop('checked', true);
                                    }
                                    if (x == 4 && variable == "0") {
                                        $('#providerscheduleDetail #chkwekThursday').prop('checked', false);
                                    }
                                    if (x == 5 && variable == "1") {
                                        $('#providerscheduleDetail #chkwekFriday').prop('checked', true);
                                    }
                                    if (x == 5 && variable == "0") {
                                        $('#providerscheduleDetail #chkwekFriday').prop('checked', false);
                                    }
                                    if (x == 6 && variable == "1") {
                                        $('#providerscheduleDetail #chkwekSaturday').prop('checked', true);
                                    }
                                    if (x == 6 && variable == "0") {
                                        $('#providerscheduleDetail #chkwekSaturday').prop('checked', false);
                                    }
                                    x++;
                                }
                            }

                            if (providerSchedule_detail.patternevery == "True") {
                                $('#providerscheduleDetail #rdEveryweekely').prop('checked', true);
                                $('#providerscheduleDetail #txtactiveweek').val(providerSchedule_detail.value);
                                var s = providerSchedule_detail.patterdays;
                                var match = s.split(',');
                                var x = 0;
                                for (var a in match) {
                                    var variable = match[a];

                                    if (x == 0 && variable == "1") {
                                        $('#providerscheduleDetail #chkwekSunday').prop('checked', true);
                                    }
                                    if (x == 0 && variable == "0") {
                                        $('#providerscheduleDetail #chkwekSunday').prop('checked', false);
                                    }
                                    if (x == 1 && variable == "1") {
                                        $('#providerscheduleDetail #chkwekMonday').prop('checked', true);
                                    }
                                    if (x == 1 && variable == "0") {
                                        $('#providerscheduleDetail #chkwekMonday').prop('checked', false);
                                    }
                                    if (x == 2 && variable == "1") {
                                        $('#providerscheduleDetail #chkwekTuesday').prop('checked', true);
                                    }
                                    if (x == 2 && variable == "0") {
                                        $('#providerscheduleDetail #chkwekTuesday').prop('checked', false);
                                    }
                                    if (x == 3 && variable == "1") {
                                        $('#providerscheduleDetail #chkwekWednesday').prop('checked', true);
                                    }
                                    if (x == 3 && variable == "0") {
                                        $('#providerscheduleDetail #chkwekWednesday').prop('checked', false);
                                    }
                                    if (x == 4 && variable == "1") {
                                        $('#providerscheduleDetail #chkwekThursday').prop('checked', true);
                                    }
                                    if (x == 4 && variable == "0") {
                                        $('#providerscheduleDetail #chkwekThursday').prop('checked', false);
                                    }
                                    if (x == 5 && variable == "1") {
                                        $('#providerscheduleDetail #chkwekFriday').prop('checked', true);
                                    }
                                    if (x == 5 && variable == "0") {
                                        $('#providerscheduleDetail #chkwekFriday').prop('checked', false);
                                    }
                                    if (x == 6 && variable == "1") {
                                        $('#providerscheduleDetail #chkwekSaturday').prop('checked', true);
                                    }
                                    if (x == 6 && variable == "0") {
                                        $('#providerscheduleDetail #chkwekSaturday').prop('checked', false);
                                    }
                                    x++;
                                }
                            }
                        }
                        if (providerSchedule_detail.schfor == "Monthly") {

                            $('.nav-tabs a[href="#Monthly"]').tab('show');


                            if (providerSchedule_detail.patternevery == "False") {
                                $('#providerscheduleDetail #rdmntdaay').prop('checked', true);
                                $('#providerscheduleDetail #txtmntActive').val(providerSchedule_detail.value);
                                $('#providerscheduleDetail #txtmntofevry').val(providerSchedule_detail.pattermonths);
                            }
                            if (providerSchedule_detail.patternevery == "True") {

                                $('#providerscheduleDetail #rdmntthe').prop('checked', true);
                                $('#providerscheduleDetail #txtmnttheofevery').val(providerSchedule_detail.pattermonths);
                                var s = providerSchedule_detail.patterdays;
                                var match = s.split(',');
                                var x = 0;
                                for (var a in match) {
                                    var variable = match[a];

                                    if (x == 0 && variable == "1") {
                                        $('#providerscheduleDetail #chkmntSunday').prop('checked', true);
                                    }
                                    if (x == 0 && variable == "0") {
                                        $('#providerscheduleDetail #chkmntSunday').prop('checked', false);
                                    }
                                    if (x == 1 && variable == "1") {
                                        $('#providerscheduleDetail #chkmntMonday').prop('checked', true);
                                    }
                                    if (x == 1 && variable == "0") {
                                        $('#providerscheduleDetail #chkmntMonday').prop('checked', false);
                                    }
                                    if (x == 2 && variable == "1") {
                                        $('#providerscheduleDetail #chkmntTuesday').prop('checked', true);
                                    }
                                    if (x == 2 && variable == "0") {
                                        $('#providerscheduleDetail #chkmntTuesday').prop('checked', false);
                                    }
                                    if (x == 3 && variable == "1") {
                                        $('#providerscheduleDetail #chkmntWednesday').prop('checked', true);
                                    }
                                    if (x == 3 && variable == "0") {
                                        $('#providerscheduleDetail #chkmntWednesday').prop('checked', false);
                                    }
                                    if (x == 4 && variable == "1") {
                                        $('#providerscheduleDetail #chkmntThursday').prop('checked', true);
                                    }
                                    if (x == 4 && variable == "0") {
                                        $('#providerscheduleDetail #chkmntThursday').prop('checked', false);
                                    }
                                    if (x == 5 && variable == "1") {
                                        $('#providerscheduleDetail #chkmntFriday').prop('checked', true);
                                    }
                                    if (x == 5 && variable == "0") {
                                        $('#providerscheduleDetail #chkmntFriday').prop('checked', false);
                                    }
                                    if (x == 6 && variable == "1") {
                                        $('#providerscheduleDetail #chkmntSaturday').prop('checked', true);
                                    }
                                    if (x == 6 && variable == "0") {
                                        $('#providerscheduleDetail #chkmntSaturday').prop('checked', false);
                                    }
                                    x++;
                                }
                                var s1 = providerSchedule_detail.patterweeks;
                                var match1 = s1.split(',');
                                var x1 = 0;
                                for (var a1 in match1) {
                                    var variable1 = match1[a1];

                                    if (x1 == 0 && variable1 == "1") {
                                        $('#providerscheduleDetail #chkmntFirst').prop('checked', true);
                                    }
                                    if (x1 == 0 && variable1 == "0") {
                                        $('#providerscheduleDetail #chkmntFirst').prop('checked', false);
                                    }
                                    if (x1 == 1 && variable1 == "1") {
                                        $('#providerscheduleDetail #chkmntSecond').prop('checked', true);
                                    }
                                    if (x1 == 1 && variable1 == "0") {
                                        $('#providerscheduleDetail #chkmntSecond').prop('checked', false);
                                    }
                                    if (x1 == 2 && variable1 == "1") {
                                        $('#providerscheduleDetail #chkmntThird').prop('checked', true);
                                    }
                                    if (x1 == 2 && variable1 == "0") {
                                        $('#providerscheduleDetail #chkmntThird').prop('checked', false);
                                    }
                                    if (x1 == 3 && variable1 == "1") {
                                        $('#providerscheduleDetail #chkmntFourth').prop('checked', true);
                                    }
                                    if (x1 == 3 && variable1 == "0") {
                                        $('#providerscheduleDetail #chkmntFourth').prop('checked', false);
                                    }
                                    if (x1 == 4 && variable1 == "1") {
                                        $('#providerscheduleDetail #chkmntLast').prop('checked', true);
                                    }
                                    if (x1 == 4 && variable1 == "0") {
                                        $('#providerscheduleDetail #chkmntLast').prop('checked', false);
                                    }

                                    x1++;
                                }
                            }
                        }
                        if (providerSchedule_detail.schfor == "Yearly") {

                            $('.nav-tabs a[href="#Yearly"]').tab('show');

                            if (providerSchedule_detail.patternevery == "False") {
                                $('#providerscheduleDetail #rdyerEvery').prop('checked', true);
                                $('#providerscheduleDetail #monthDays').val(providerSchedule_detail.value);
                                if (providerSchedule_detail.pattermonths == "1")
                                    $('#providerscheduleDetail #yearMonth').val("January");
                                else if (providerSchedule_detail.pattermonths == "2")
                                    $('#providerscheduleDetail #yearMonth').val("February");
                                else if (providerSchedule_detail.pattermonths == "3")
                                    $('#providerscheduleDetail #yearMonth').val("March");
                                else if (providerSchedule_detail.pattermonths == "4")
                                    $('#providerscheduleDetail #yearMonth').val("April");
                                else if (providerSchedule_detail.pattermonths == "5")
                                    $('#providerscheduleDetail #yearMonth').val("May");
                                else if (providerSchedule_detail.pattermonths == "6")
                                    $('#providerscheduleDetail #yearMonth').val("June");
                                else if (providerSchedule_detail.pattermonths == "7")
                                    $('#providerscheduleDetail #yearMonth').val("July");
                                else if (providerSchedule_detail.pattermonths == "8")
                                    $('#providerscheduleDetail #yearMonth').val("August");
                                else if (providerSchedule_detail.pattermonths == "9")
                                    $('#providerscheduleDetail #yearMonth').val("September");
                                else if (providerSchedule_detail.pattermonths == "10")
                                    $('#providerscheduleDetail #yearMonth').val("October");
                                else if (providerSchedule_detail.pattermonths == "11")
                                    $('#providerscheduleDetail #yearMonth').val("November");
                                else if (providerSchedule_detail.pattermonths == "12")
                                    $('#providerscheduleDetail #yearMonth').val("Decmber");
                            }
                            if (providerSchedule_detail.patternevery == "True") {
                                $('#providerscheduleDetail #rdyerthe').prop('checked', true);
                                var s = providerSchedule_detail.patterdays;
                                var match = s.split(',');
                                var x = 0;
                                for (var a in match) {
                                    var variable = match[a];

                                    if (x == 0 && variable == "1") {
                                        $('#providerscheduleDetail #chkyerSunday').prop('checked', true);
                                    }
                                    if (x == 0 && variable == "0") {
                                        $('#providerscheduleDetail #chkyerSunday').prop('checked', false);
                                    }
                                    if (x == 1 && variable == "1") {
                                        $('#providerscheduleDetail #chkyerMonday').prop('checked', true);
                                    }
                                    if (x == 1 && variable == "0") {
                                        $('#providerscheduleDetail #chkyerMonday').prop('checked', false);
                                    }
                                    if (x == 2 && variable == "1") {
                                        $('#providerscheduleDetail #chkyerTuesday').prop('checked', true);
                                    }
                                    if (x == 2 && variable == "0") {
                                        $('#providerscheduleDetail #chkyerTuesday').prop('checked', false);
                                    }
                                    if (x == 3 && variable == "1") {
                                        $('#providerscheduleDetail #chkyerWednesday').prop('checked', true);
                                    }
                                    if (x == 3 && variable == "0") {
                                        $('#providerscheduleDetail #chkyerWednesday').prop('checked', false);
                                    }
                                    if (x == 4 && variable == "1") {
                                        $('#providerscheduleDetail #chkyerThursday').prop('checked', true);
                                    }
                                    if (x == 4 && variable == "0") {
                                        $('#providerscheduleDetail #chkyerThursday').prop('checked', false);
                                    }
                                    if (x == 5 && variable == "1") {
                                        $('#providerscheduleDetail #chkyerFriday').prop('checked', true);
                                    }
                                    if (x == 5 && variable == "0") {
                                        $('#providerscheduleDetail #chkyerFriday').prop('checked', false);
                                    }
                                    if (x == 6 && variable == "1") {
                                        $('#providerscheduleDetail #chkyerSaturday').prop('checked', true);
                                    }
                                    if (x == 6 && variable == "0") {
                                        $('#providerscheduleDetail #chkyerSaturday').prop('checked', false);
                                    }
                                    x++;
                                }
                                var s1 = providerSchedule_detail.patterweeks;
                                var match1 = s1.split(',');
                                var x1 = 0;
                                for (var a1 in match1) {
                                    var variable1 = match1[a1];

                                    if (x1 == 0 && variable1 == "1") {
                                        $('#providerscheduleDetail #chkyerFirst').prop('checked', true);
                                    }
                                    if (x1 == 0 && variable1 == "0") {
                                        $('#providerscheduleDetail #chkyerFirst').prop('checked', false);
                                    }
                                    if (x1 == 1 && variable1 == "1") {
                                        $('#providerscheduleDetail #chkyerSecond').prop('checked', true);
                                    }
                                    if (x1 == 1 && variable1 == "0") {
                                        $('#providerscheduleDetail #chkyerSecond').prop('checked', false);
                                    }
                                    if (x1 == 2 && variable1 == "1") {
                                        $('#providerscheduleDetail #chkyerThird').prop('checked', true);
                                    }
                                    if (x1 == 2 && variable1 == "0") {
                                        $('#providerscheduleDetail #chkyerThird').prop('checked', false);
                                    }
                                    if (x1 == 3 && variable1 == "1") {
                                        $('#providerscheduleDetail #chkyerFourth').prop('checked', true);
                                    }
                                    if (x1 == 3 && variable1 == "0") {
                                        $('#providerscheduleDetail #chkyerFourth').prop('checked', false);
                                    }
                                    if (x1 == 4 && variable1 == "1") {
                                        $('#providerscheduleDetail #chkyerLast').prop('checked', true);
                                    }
                                    if (x1 == 4 && variable1 == "0") {
                                        $('#providerscheduleDetail #chkyerLast').prop('checked', false);
                                    }

                                    x1++;
                                }
                                var s2 = providerSchedule_detail.pattermonths;
                                var match2 = s2.split(',');
                                var x2 = 0;
                                for (var a2 in match2) {
                                    var variable2 = match2[a2];

                                    if (x2 == 0 && variable2 == "1") {
                                        $('#providerscheduleDetail #chkyerJanuary').prop('checked', true);
                                    }
                                    if (x2 == 0 && variable2 == "0") {
                                        $('#providerscheduleDetail #chkyerJanuary').prop('checked', false);
                                    }
                                    if (x2 == 1 && variable2 == "1") {
                                        $('#providerscheduleDetail #chkyerFebruary').prop('checked', true);
                                    }
                                    if (x2 == 1 && variable2 == "0") {
                                        $('#providerscheduleDetail #chkyerFebruary').prop('checked', false);
                                    }
                                    if (x2 == 2 && variable2 == "1") {
                                        $('#providerscheduleDetail #chkyerMarch').prop('checked', true);
                                    }
                                    if (x2 == 2 && variable2 == "0") {
                                        $('#providerscheduleDetail #chkyerMarch').prop('checked', false);
                                    }
                                    if (x2 == 3 && variable2 == "1") {
                                        $('#providerscheduleDetail #chkyerApril').prop('checked', true);
                                    }
                                    if (x2 == 3 && variable2 == "0") {
                                        $('#providerscheduleDetail #chkyerApril').prop('checked', false);
                                    }
                                    if (x2 == 4 && variable2 == "1") {
                                        $('#providerscheduleDetail #chkyerMay').prop('checked', true);
                                    }
                                    if (x2 == 4 && variable2 == "0") {
                                        $('#providerscheduleDetail #chkyerMay').prop('checked', false);
                                    }
                                    if (x2 == 5 && variable2 == "1") {
                                        $('#providerscheduleDetail #chkyerJune').prop('checked', true);
                                    }
                                    if (x2 == 5 && variable2 == "0") {
                                        $('#providerscheduleDetail #chkyerJune').prop('checked', false);
                                    }
                                    if (x2 == 6 && variable2 == "1") {
                                        $('#providerscheduleDetail #chkyerJuly').prop('checked', true);
                                    }
                                    if (x2 == 6 && variable2 == "0") {
                                        $('#providerscheduleDetail #chkyerJuly').prop('checked', false);
                                    }
                                    if (x2 == 7 && variable2 == "1") {
                                        $('#providerscheduleDetail #chkyerAugust').prop('checked', true);
                                    }
                                    if (x2 == 7 && variable2 == "0") {
                                        $('#providerscheduleDetail #chkyerAugust').prop('checked', false);
                                    }
                                    if (x2 == 8 && variable2 == "1") {
                                        $('#providerscheduleDetail #chkyerSeptember').prop('checked', true);
                                    }
                                    if (x2 == 8 && variable2 == "0") {
                                        $('#providerscheduleDetail #chkyerSeptember').prop('checked', false);
                                    }
                                    if (x2 == 9 && variable2 == "1") {
                                        $('#providerscheduleDetail #chkyerOctober').prop('checked', true);
                                    }
                                    if (x2 == 9 && variable2 == "0") {
                                        $('#providerscheduleDetail #chkyerOctober').prop('checked', false);
                                    }
                                    if (x2 == 10 && variable2 == "1") {
                                        $('#providerscheduleDetail #chkyerNovember').prop('checked', true);
                                    }
                                    if (x2 == 10 && variable2 == "0") {
                                        $('#providerscheduleDetail #chkyerNovember').prop('checked', false);
                                    }
                                    if (x2 == 11 && variable2 == "1") {
                                        $('#providerscheduleDetail #chkyerDecember').prop('checked', true);
                                    }
                                    if (x2 == 11 && variable2 == "0") {
                                        $('#providerscheduleDetail #chkyerDecember').prop('checked', false);
                                    }


                                    x2++;
                                }
                            }
                        }

                        //serialize Data after all controls loaded.
                        $('#frmproviderscheduleDetail').data('serialize', $('#frmproviderscheduleDetail').serialize());

                    });

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    Saveproviderschedules: function () {

        var criteria;
        var criteria1 = 0;
        var start = blockHoursDetail.ConvertTimeformat("24", $("#providerscheduleDetail #frmtime").val());
        var end = blockHoursDetail.ConvertTimeformat("24", $("#providerscheduleDetail #totime").val());

        var array = start.split(":");
        var array1 = end.split(":");
        var x = array[0];
        var y = array1[0];

        if (parseInt(x) < parseInt(y))
            criteria = 'true';
        else if (parseInt(x) == parseInt(y)) {
            var w = array[1];
            var n = array1[1];
            if (parseInt(w) == parseInt(n))
                criteria = 'false';
            if (parseInt(w) < parseInt(n))
                criteria = 'true';
            if (parseInt(w) > parseInt(n))
                criteria = 'false';

        }
        else if (parseInt(x) > parseInt(y))
            criteria = 'false';
        // block hours
        var blktime = $("#providerscheduleDetail #frmblckhrstime").val();
        if ($.trim(blktime) != "") {

            var start = blockHoursDetail.ConvertTimeformat("24", $("#providerscheduleDetail #frmblckhrstime").val());
            var end = blockHoursDetail.ConvertTimeformat("24", $("#providerscheduleDetail #toblckhrstime").val());

            var array = start.split(":");
            var array1 = end.split(":");
            var x = array[0];
            var y = array1[0];

            if (parseInt(x) < parseInt(y))
                criteria1 = 'true';
            else if (parseInt(x) == parseInt(y)) {
                var w = array[1];
                var n = array1[1];
                if (parseInt(w) == parseInt(n))
                    criteria1 = 'false';
                if (parseInt(w) < parseInt(n))
                    criteria1 = 'true';
                if (parseInt(w) > parseInt(n))
                    criteria1 = 'false';

            }
            else if (parseInt(x) > parseInt(y))
                criteria1 = 'false';
        }

        ////

        if (criteria == 'true') {
            if (criteria1 != 0) {
                if (criteria1 == 'true') {
                    var strMessage = "";
                    var self = $("#providerscheduleDetail");
                    var myJSON = self.getMyJSON();
                    var patternJSON;
                    var $tab = $('#providerscheduleDetail #myTabContent'), $active = $tab.find('.tab-pane.active'), text = $active.find('p:hidden').text();
                    //alert(text)
                    if (text == 'Daily') {
                        var self = $('#providerscheduleDetail #Daily');
                        patternJSON = self.getMyJSON();
                        var json2 = { "pattern": "Daily" };
                        patternJSON = JSON.stringify($.extend(false, {}, JSON.parse(patternJSON), json2));
                    }
                    else if (text == 'Weekly') {
                        var self = $('#providerscheduleDetail #Weekly');
                        patternJSON = self.getMyJSON();
                        var json2 = { "pattern": "Weekly" };
                        patternJSON = JSON.stringify($.extend(false, {}, JSON.parse(patternJSON), json2));
                        providerscheduleDetail.ValidateproviderscheduleDetail();
                    }
                    else if (text == 'Monthly') {
                        var self = $('#providerscheduleDetail #Monthly');
                        patternJSON = self.getMyJSON();
                        var json2 = { "pattern": "Monthly" };
                        patternJSON = JSON.stringify($.extend(false, {}, JSON.parse(patternJSON), json2));
                    }
                    else if (text == 'Yearly') {
                        var self = $('#providerscheduleDetail #Yearly');
                        patternJSON = self.getMyJSON();
                        var json2 = { "pattern": "Yearly" };
                        patternJSON = JSON.stringify($.extend(false, {}, JSON.parse(patternJSON), json2));
                    }
                    patternJSON = patternJSON

                    var j = { "name": "Daily" };
                    JSON.stringify(j);
                    if (providerscheduleDetail.params.mode == "Add") {
                        AppPrivileges.GetFormPrivileges("Provider Schedule", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                            if (strMessage == "") {
                                providerscheduleDetail.Saveproviderschedule(myJSON, patternJSON).done(function (response) {
                                    if (response.status != false) {
                                        Admin_ProviderSchedule.ProviderScheduleSearch(response.ScheduleId);
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
                    else if (providerscheduleDetail.params.mode == "Edit") {
                        AppPrivileges.GetFormPrivileges("Provider Schedule", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                            if (strMessage == "") {
                                providerscheduleDetail.UpdateProviderSchedule(myJSON, providerscheduleDetail.params.ScheduleId).done(function (response) {
                                    if (response.status != false) {
                                        Admin_ProviderSchedule.ProviderScheduleSearch(providerscheduleDetail.params.ScheduleId);
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
                }
                else {
                    utility.DisplayMessages('Block Hours From Time Criteria must be greater then To Time Criteria.', 3);
                }
            }
            else {
                var strMessage = "";
                var self = $("#providerscheduleDetail");
                var myJSON = self.getMyJSON();
                var patternJSON;
                var $tab = $('#providerscheduleDetail #myTabContent'), $active = $tab.find('.tab-pane.active'), text = $active.find('p:hidden').text();
                //alert(text)
                if (text == 'Daily') {
                    var self = $('#providerscheduleDetail #Daily');
                    patternJSON = self.getMyJSON();
                    var json2 = { "pattern": "Daily" };
                    patternJSON = JSON.stringify($.extend(false, {}, JSON.parse(patternJSON), json2));
                }
                else if (text == 'Weekly') {
                    var self = $('#providerscheduleDetail #Weekly');
                    patternJSON = self.getMyJSON();
                    var json2 = { "pattern": "Weekly" };
                    patternJSON = JSON.stringify($.extend(false, {}, JSON.parse(patternJSON), json2));
                    providerscheduleDetail.ValidateproviderscheduleDetail();
                }
                else if (text == 'Monthly') {
                    var self = $('#providerscheduleDetail #Monthly');
                    patternJSON = self.getMyJSON();
                    var json2 = { "pattern": "Monthly" };
                    patternJSON = JSON.stringify($.extend(false, {}, JSON.parse(patternJSON), json2));
                }
                else if (text == 'Yearly') {
                    var self = $('#providerscheduleDetail #Yearly');
                    patternJSON = self.getMyJSON();
                    var json2 = { "pattern": "Yearly" };
                    patternJSON = JSON.stringify($.extend(false, {}, JSON.parse(patternJSON), json2));
                }
                patternJSON = patternJSON

                var j = { "name": "Daily" };
                JSON.stringify(j);
                if (providerscheduleDetail.params.mode == "Add") {
                    AppPrivileges.GetFormPrivileges("Provider Schedule", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            providerscheduleDetail.Saveproviderschedule(myJSON, patternJSON).done(function (response) {
                                if (response.status != false) {
                                    Admin_ProviderSchedule.ProviderScheduleSearch(response.ScheduleId);
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
                else if (providerscheduleDetail.params.mode == "Edit") {
                    AppPrivileges.GetFormPrivileges("Provider Schedule", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            providerscheduleDetail.UpdateProviderSchedule(myJSON, providerscheduleDetail.params.ScheduleId).done(function (response) {
                                if (response.status != false) {
                                    Admin_ProviderSchedule.ProviderScheduleSearch(providerscheduleDetail.params.ScheduleId);
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
            }
        }
        else
            utility.DisplayMessages('From Time Criteria must be greater then To Time Criteria.', 3);



    },

    ValidateproviderscheduleDetail: function () {
        $('#frmproviderscheduleDetail')
           .bootstrapValidator({
               //live: 'disabled',
               live: 'enabled',
               //excluded: [':disabled'],
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   provider: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   //facility: {
                   //    group: '.col-sm-4',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //},
                   fromdate: {
                       group: '.col-sm-4',
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
                   frmtime: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   //slotminutes: {
                   //    group: '.col-md-2',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //},
                   todate: {
                       group: '.col-sm-4',
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
                   totime: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   //PatientAllowed: {
                   //    group: '.col-sm-4',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //},
                   //blockreason: {
                   //    group: '.col-md-3',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //},
                   blckhrsfromtime: {
                       group: '.col-md-2',

                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   blckhrstotime: {
                       group: '.col-md-2',

                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   DailyEveryDays: {
                       group: '.col-md-5',
                       enabled: true,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   'weekdays[]': {
                       feedbackIcons: 'false',
                       validators: {
                           choice: {
                               min: 1,
                               message: 'Please choose 2 - 4 programming languages you are good at'
                           }
                       }
                   },
                   txtactiveweek: {
                       group: '.col-md-8',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   todateY: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },

                   todateYDay: {
                       group: '.col-sm-2',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },

                   //schreason: {
                   //    group: '.col-md-3',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //},

                   txtmntofevry: {
                       group: '.col-md-2',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },

                   'theQuartersY[]': {
                       group: '.theQuartersYVD',
                       feedbackIcons: 'false',
                       validators: {
                           choice: {
                               min: 1,
                               message: 'Please atleast choose 1.'
                           }
                       }
                   },


                   'themonthsY[]': {
                       group: '.themonthsYVD',
                       feedbackIcons: 'false',
                       validators: {
                           choice: {
                               min: 1,
                               message: 'Please atleast choose 1.'
                           }
                       }
                   },

                   'theDaysY[]': {
                       group: '.theDaysYVD',
                       feedbackIcons: 'false',
                       validators: {
                           choice: {
                               min: 1,
                               message: 'Please atleast choose 1.'
                           }
                       }
                   },


                   'themonths[]': {
                       group: '.themonthsVD',
                       feedbackIcons: 'false',
                       validators: {
                           choice: {
                               min: 1,
                               message: 'Please atleast choose 1.'
                           }
                       }
                   },

                   'theweeksM[]': {
                       group: '.theweeksmVD',
                       feedbackIcons: 'false',
                       validators: {
                           choice: {
                               min: 1,
                               message: 'Please atleast choose 1.'
                           }
                       }
                   },
                   txtmntofevryM: {
                       group: '.col-md-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   txtmntActive: {
                       group: '.col-md-2',
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
            providerscheduleDetail.Saveproviderschedules();
        });
    },

    LoadAllAutocomplete: function () {

        CacheManager.BindCodes('GetFacility', false).done(function (result) {
            $("input#txtFacility").autocomplete({
                autoFocus: true,
                source: Facilities, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#providerscheduleDetail #hfFacility").val(ui.item.id); // add the selected id
                    }, 100);
                }
            });
        });

    },

    FillDuration: function (control) {

        var ScheduleReasonID = $(control).val();

        providerscheduleDetail.FillScheduleReasonDuration(ScheduleReasonID).done(function (response) {
            if (response.status != false) {
                var reasonduration = JSON.parse(response.ProviderScheduleFill_JSON);
                $('#providerscheduleDetail #slotminutes').val(reasonduration.slotminutes);
                $('#frmproviderscheduleDetail').bootstrapValidator('revalidateField', 'slotminutes');

            }
            else {

            }
        });
    },

    EntityBaseData: function (control) {

        var entityID = $(control).find(":selected").attr('refvalue') || "";
        if (entityID != "") {
            CacheManager.BindDropDownsByEntityID('#providerscheduleDetail #ddlfacility', 'GetFacility', true, entityID);
            //CacheManager.BindDropDownsByEntityID('#providerscheduleDetail #ddlschreason', 'GetBlockReasons', true, entityID).done(function () {
            //    $('#providerscheduleDetail #blockreason').html($('#providerscheduleDetail #ddlschreason').html());
            //});
            //CacheManager.BindDropDownsByEntityID('#providerscheduleDetail #blockreason', 'GetBlockReasons', false, entityID);
            $("#providerscheduleDetail #ddlfacility").removeAttr('disabled');
            //$("#providerscheduleDetail #ddlschreason").removeAttr('disabled');
            //$("#providerscheduleDetail #blockreason").removeAttr('disabled');
            if ($('#frmproviderscheduleDetail').data('bootstrapValidator') != null && typeof $('#frmproviderscheduleDetail').data('bootstrapValidator') != 'undefined') {
                $('#providerscheduleDetail #ddlfacility').val('');
                $('#frmproviderscheduleDetail').bootstrapValidator('revalidateField', 'facility');
            }
        } else {
            providerscheduleDetail.SetFieldsDefault();
        }
    },

    SetFieldsDefault: function () {

        $("#providerscheduleDetail #ddlfacility").prop('disabled', 'disabled');
        // $("#providerscheduleDetail #ddlschreason").prop('disabled', 'disabled');
        //$("#providerscheduleDetail #blockreason").prop('disabled', 'disabled');
        $("#providerscheduleDetail #ddlfacility").val('');
        //$("#providerscheduleDetail #ddlschreason").val('');
        //$("#providerscheduleDetail #blockreason").val('');
    },

    Saveproviderschedule: function (ProviderScheduleData, ProviderSchedulePatternData) {
        var data = "ProviderScheduleData=" + ProviderScheduleData + "&ProviderSchedulePatternData=" + ProviderSchedulePatternData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROVIDERSCHEDULE_DETAIL", "SAVE_PROVIDERSCHEDULE");
    },

    UpdateProviderSchedule: function (ProviderScheduleData, ScheduleID) {
        var data = "ProviderScheduleData=" + ProviderScheduleData + "&ScheduleID=" + ScheduleID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROVIDERSCHEDULE_DETAIL", "UPDATE_PROVIDERSCHEDULE");
    },

    FillProviderSchedule: function (ScheduleID) {
        var data = "ScheduleID=" + ScheduleID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROVIDERSCHEDULE_DETAIL", "FILL_PROVIDERSCHEDULE");
    },

    UpdateProviderScheduleActiveInactive: function (ScheduleId, IsActive) {
        var data = "ScheduleId=" + ScheduleId + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROVIDERSCHEDULE_DETAIL", "UPDATE_PROVIDERSCHEDULE_ACTIVE_INACTIVE");
    },

    FillScheduleReasonDuration: function (ScheduleReasonID) {
        var data = "ScheduleReasonID=" + ScheduleReasonID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROVIDERSCHEDULE_DETAIL", "FILL_SCHEDULEREASON_DURATION");
    },

    UnLoad: function () {
        //if (providerscheduleDetail.params.mode == "Add") {
        if ($('#frmproviderscheduleDetail').serialize() != $('#frmproviderscheduleDetail').data('serialize')) {
            utility.myConfirm('2', function () {
                UnloadActionPan();
            }, function () { },
                    '2'
                );
        }
        else {
            UnloadActionPan();
        }
        //}
        //else if (providerscheduleDetail.params.mode == "Edit") {
        //    UnloadActionPan();
        //}

    },

    // -------------- Facility ---------------------

    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmproviderscheduleDetail";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "providerscheduleDetail";
        LoadActionPan('Admin_Facility', params);
    },

    // -------------- Facility ---------------------

    //***** Validation *****\\

    DailyChecks: function () {

        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('DailyEveryDays', true);

        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('weekdays[]', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('txtactiveweek', false);

        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('txtmntofevryM', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('txtmntActive', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('txtmntofevry', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('themonths[]', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('theweeksM[]', false);

        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('theQuartersY[]', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('themonthsY[]', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('theDaysY[]', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('todateY', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('todateYDay', false);
    },

    WeeklyChecks: function () {

        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('DailyEveryDays', false);

        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('weekdays[]', true);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('txtactiveweek', false);

        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('txtmntofevryM', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('txtmntActive', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('txtmntofevry', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('themonths[]', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('theweeksM[]', false);

        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('theQuartersY[]', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('themonthsY[]', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('theDaysY[]', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('todateY', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('todateYDay', false);
    },

    MonthlyChecks: function () {

        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('DailyEveryDays', false);

        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('weekdays[]', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('txtactiveweek', false);

        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('txtmntofevryM', true);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('txtmntActive', true);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('txtmntofevry', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('themonths[]', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('theweeksM[]', false);

        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('theQuartersY[]', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('themonthsY[]', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('theDaysY[]', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('todateY', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('todateYDay', false);

    },

    YearlyChecks: function () {

        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('DailyEveryDays', false);

        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('weekdays[]', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('txtactiveweek', false);

        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('txtmntofevryM', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('txtmntActive', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('txtmntofevry', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('themonths[]', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('theweeksM[]', false);

        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('theQuartersY[]', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('themonthsY[]', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('theDaysY[]', false);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('todateY', true);
        $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('todateYDay', true);


    },


    //------------------------------------------

    FillScheduleReason: function (ScheduleReasonId, ShortName, Duration, event) {

        if (event != null) {
            event.stopPropagation();
        }

        UnloadActionPan("providerscheduleDetail");

        $('#providerscheduleDetail #txtSchReason').val(ShortName);
        $('#providerscheduleDetail #hfSchReasonId').val(ScheduleReasonId);
        $('#providerscheduleDetail #slotminutes').val(Duration);
     
        if ($('#frmproviderscheduleDetail').data('bootstrapValidator') != null && typeof $('#frmproviderscheduleDetail').data('bootstrapValidator') != 'undefined') {
            $('#frmproviderscheduleDetail').bootstrapValidator('revalidateField', 'slotminutes');
            
        }
        providerscheduleDetail.BindScheduleReasons();

    },

    BindScheduleReasons: function () {
        var SchReason = $('#providerscheduleDetail #txtSchReason').val();
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

            $('#providerscheduleDetail #txtSchReason').autocomplete({
                autoFocus: true,
                source: AllSchReasons,
                open: function (event, ui) { disable = true },
                close: function (event, ui) {
                    disable = false; $(this).focus();
                },
                select: function (event, ui) {
                    setTimeout(function () {
                        $('#providerscheduleDetail #txtSchReason').val(ui.item.value);
                        $('#providerscheduleDetail #hfSchReasonId').val(ui.item.id);
                        $('#providerscheduleDetail #slotminutes').val(ui.item.Duration)
                        $('#frmproviderscheduleDetail').bootstrapValidator('revalidateField', 'slotminutes');
                    }, 100);

                }
            }).blur(function () {
                setTimeout(function () {
                    utility.ValidateAutoComplete($('#providerscheduleDetail #txtSchReason'), 'providerscheduleDetail #hfSchReasonId', false, null, null, null);
                }, 200);
            });
          //  $('#providerscheduleDetail #txtSchReason').autocomplete("search");

        });

        //--------------------
    },

    OpenScheduleReason: function () {

        var params = [];
        params["ScheduleReasonId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "providerscheduleDetail";
        params["IsBlockReason"] = false;
        LoadActionPan('Admin_ScheduleReason', params);

    },


    //-----------------------
    FillBlockReason: function (ScheduleReasonId, ShortName, Duration, event) {
        if (event != null) {
            event.stopPropagation();
        }

        UnloadActionPan("providerscheduleDetail");

        if ($('#providerscheduleDetail #txtBlckReason').data('kendoAutoComplete'))
            utility.SetKendoAutoCompleteSourceforValidate($('#providerscheduleDetail #txtBlckReason'), ShortName, $('#providerscheduleDetail #hfBlckReasonId'), ScheduleReasonId);
        providerscheduleDetail.BlockReasonOnblur();
    },

    BindBlockReasons: function () {
        var Ctrl = $('#providerscheduleDetail #txtBlckReason');
        var func = function () { return providerscheduleDetail.GetScheduleReasonsArray(Ctrl.val()) };
        var hfCtrl = $('#providerscheduleDetail #hfBlckReasonId');
        var onSelect = function (e) {
            $('#providerscheduleDetail #txtBlckReason').val(e.value);
            $('#providerscheduleDetail #hfBlckReasonId').val(e.id);
            providerscheduleDetail.BlockReasonOnblur();
        };
        utility.BindKendoAutoComplete(Ctrl, 3, "value", "contains", null, func, hfCtrl, onSelect);
    },

    GetScheduleReasonsArray: function (name) {
        var AllSchReasons = [];
        var dfd = new $.Deferred();
        blckreasonDetail.LoadScheduleReasons(name).done(function (responseData) {
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
        return dfd.promise();
    },

    OpenBlockReason: function () {

        var params = [];
        params["ScheduleReasonId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "providerscheduleDetail";
        params["IsBlockReason"] = true;
        LoadActionPan('Admin_ScheduleReason', params);

    },


    BlockReasonOnblur: function () {
        //setTimeout(function () {
            if ($("#frmproviderscheduleDetail #hfBlckReasonId").val() != "") {
                $('#frmproviderscheduleDetail #frmblckhrstime').timepicker('setTime', $('#frmproviderscheduleDetail #frmtime').data("timepicker").getTime());
                $('#frmproviderscheduleDetail #toblckhrstime').timepicker('setTime', $('#frmproviderscheduleDetail #totime').data("timepicker").getTime());
                $('#frmproviderscheduleDetail #frmblckhrstime').attr('disabled', false);
                $('#frmproviderscheduleDetail #toblckhrstime').attr('disabled', false);
                if ($('#frmproviderscheduleDetail').data('bootstrapValidator') != null && typeof $('#frmproviderscheduleDetail').data('bootstrapValidator') != 'undefined') {
                    $('#frmproviderscheduleDetail').bootstrapValidator('revalidateField', 'blckhrsfromtime');
                    $('#frmproviderscheduleDetail').bootstrapValidator('revalidateField', 'blckhrstotime');
                }
            }
            else {

                $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('blckhrsfromtime', false);
                $('#frmproviderscheduleDetail').data('bootstrapValidator').enableFieldValidators('blckhrstotime', false);

                $('#frmproviderscheduleDetail #frmblckhrstime').timepicker('setTime', '');
                $('#frmproviderscheduleDetail #toblckhrstime').timepicker('setTime', '');
                $('#frmproviderscheduleDetail #frmblckhrstime').attr('disabled', true);
                $('#frmproviderscheduleDetail #toblckhrstime').attr('disabled', true);

            }
        //}, 300);
    },

    ShowHistory: function () {
        var PanelID = 'providerscheduleDetail';
        var ParentCtrl = 'providerscheduleDetail';
        var ProfileName = 'Provider Schedule';
        var DBTableName = 'ProviderSchedule';
        var ColumnKeyId = providerscheduleDetail.params.ScheduleId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);

    },
}