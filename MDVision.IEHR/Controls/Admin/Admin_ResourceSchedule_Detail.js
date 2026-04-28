resourcescheduleDetail = {
    params: [],
    Load: function (params) {
        resourcescheduleDetail.params = params;
        
        var self = $('#resourcescheduleDetail');
        self.loadDropDowns(true).done(function () {

            resourcescheduleDetail.LoadResourceScheduleDetail();
            resourcescheduleDetail.BindBlockReasons();

        });

    },

    LoadResourceScheduleDetail: function () {
        if (resourcescheduleDetail.params.mode == "Add") {

            resourcescheduleDetail.SetFieldsDefault();

            //serialize Data.
            $('#frmresourcescheduleDetail').data('serialize', $('#frmresourcescheduleDetail').serialize());
            resourcescheduleDetail.ValidateresourcescheduleDetail();
        }
        else if (resourcescheduleDetail.params.mode == "Edit") {
            $("#resourcescheduleDetail #btnsave").hide();
            resourcescheduleDetail.FillResourceSchedule(resourcescheduleDetail.params.ResScheduleId).done(function (response) {
                if (response.status != false) {
                    var resourceSchedule_detail = JSON.parse(response.ResourceScheduleFill_JSON);
                    var self = $("#resourcescheduleDetail");
                    utility.bindMyJSON(true, resourceSchedule_detail, false, self).done(function () {

                        $("#resourcescheduleDetail #frmresourcescheduleDetail :input").prop("disabled", true);
                        $("#resourcescheduleDetail #frmresourcescheduleDetail #btnHistoryResSch").prop("disabled", false);

                        if (resourceSchedule_detail.chkActive == 'True')
                            $("#resourcescheduleDetail #chkActive").prop("checked", true);
                        else
                            $("#resourcescheduleDetail #chkActive").prop("checked", false);

                        if (resourceSchedule_detail.schfor == "Daily") {
                            $('.nav-tabs a[href="#Daily"]').tab('show');
                            if (resourceSchedule_detail.patternevery == "True" && resourceSchedule_detail.patterdays == "0") {
                                $('#resourcescheduleDetail #Every').prop('checked', true);
                                $('#resourcescheduleDetail #txtevrydays').val(resourceSchedule_detail.value);
                            }
                            if (resourceSchedule_detail.patternevery == "True" && resourceSchedule_detail.patterdays != "0") {
                                $('#resourcescheduleDetail #Every').prop('checked', true);
                                $('#resourcescheduleDetail #txtevrydays').val(resourceSchedule_detail.value);
                                $('#resourcescheduleDetail #chkweekendsonly').prop('checked', true);
                            }
                            if (resourceSchedule_detail.patternevery == "False") {

                                $('#resourcescheduleDetail #rdevryweekend').prop('checked', true);
                            }
                        }
                        if (resourceSchedule_detail.schfor == "Weekly") {

                            $('.nav-tabs a[href="#Weekly"]').tab('show');

                            if (resourceSchedule_detail.patternevery == "False") {

                                $('#resourcescheduleDetail #rdEveryWeekdayOn').prop('checked', true);
                                var s = resourceSchedule_detail.patterdays;
                                var match = s.split(',')
                                var x = 0;
                                for (var a in match) {
                                    var variable = match[a];

                                    if (x == 0 && variable == "1") {
                                        $('#resourcescheduleDetail #chkwekSunday').prop('checked', true);
                                    }
                                    if (x == 0 && variable == "0") {
                                        $('#resourcescheduleDetail #chkwekSunday').prop('checked', false);
                                    }
                                    if (x == 1 && variable == "1") {
                                        $('#resourcescheduleDetail #chkwekMonday').prop('checked', true);
                                    }
                                    if (x == 1 && variable == "0") {
                                        $('#resourcescheduleDetail #chkwekMonday').prop('checked', false);
                                    }
                                    if (x == 2 && variable == "1") {
                                        $('#resourcescheduleDetail #chkwekTuesday').prop('checked', true);
                                    }
                                    if (x == 2 && variable == "0") {
                                        $('#resourcescheduleDetail #chkwekTuesday').prop('checked', false);
                                    }
                                    if (x == 3 && variable == "1") {
                                        $('#resourcescheduleDetail #chkwekWednesday').prop('checked', true);
                                    }
                                    if (x == 3 && variable == "0") {
                                        $('#resourcescheduleDetail #chkwekWednesday').prop('checked', false);
                                    }
                                    if (x == 4 && variable == "1") {
                                        $('#resourcescheduleDetail #chkwekThursday').prop('checked', true);
                                    }
                                    if (x == 4 && variable == "0") {
                                        $('#resourcescheduleDetail #chkwekThursday').prop('checked', false);
                                    }
                                    if (x == 5 && variable == "1") {
                                        $('#resourcescheduleDetail #chkwekFriday').prop('checked', true);
                                    }
                                    if (x == 5 && variable == "0") {
                                        $('#resourcescheduleDetail #chkwekFriday').prop('checked', false);
                                    }
                                    if (x == 6 && variable == "1") {
                                        $('#resourcescheduleDetail #chkwekSaturday').prop('checked', true);
                                    }
                                    if (x == 6 && variable == "0") {
                                        $('#resourcescheduleDetail #chkwekSaturday').prop('checked', false);
                                    }
                                    x++;
                                }
                            }

                            if (resourceSchedule_detail.patternevery == "True") {
                                $('#resourcescheduleDetail #rdEveryweekely').prop('checked', true);
                                $('#resourcescheduleDetail #txtactiveweek').val(resourceSchedule_detail.value);
                                var s = resourceSchedule_detail.patterdays;
                                var match = s.split(',');
                                var x = 0;
                                for (var a in match) {
                                    var variable = match[a];

                                    if (x == 0 && variable == "1") {
                                        $('#resourcescheduleDetail #chkwekSunday').prop('checked', true);
                                    }
                                    if (x == 0 && variable == "0") {
                                        $('#resourcescheduleDetail #chkwekSunday').prop('checked', false);
                                    }
                                    if (x == 1 && variable == "1") {
                                        $('#resourcescheduleDetail #chkwekMonday').prop('checked', true);
                                    }
                                    if (x == 1 && variable == "0") {
                                        $('#resourcescheduleDetail #chkwekMonday').prop('checked', false);
                                    }
                                    if (x == 2 && variable == "1") {
                                        $('#resourcescheduleDetail #chkwekTuesday').prop('checked', true);
                                    }
                                    if (x == 2 && variable == "0") {
                                        $('#resourcescheduleDetail #chkwekTuesday').prop('checked', false);
                                    }
                                    if (x == 3 && variable == "1") {
                                        $('#resourcescheduleDetail #chkwekWednesday').prop('checked', true);
                                    }
                                    if (x == 3 && variable == "0") {
                                        $('#resourcescheduleDetail #chkwekWednesday').prop('checked', false);
                                    }
                                    if (x == 4 && variable == "1") {
                                        $('#resourcescheduleDetail #chkwekThursday').prop('checked', true);
                                    }
                                    if (x == 4 && variable == "0") {
                                        $('#resourcescheduleDetail #chkwekThursday').prop('checked', false);
                                    }
                                    if (x == 5 && variable == "1") {
                                        $('#resourcescheduleDetail #chkwekFriday').prop('checked', true);
                                    }
                                    if (x == 5 && variable == "0") {
                                        $('#resourcescheduleDetail #chkwekFriday').prop('checked', false);
                                    }
                                    if (x == 6 && variable == "1") {
                                        $('#resourcescheduleDetail #chkwekSaturday').prop('checked', true);
                                    }
                                    if (x == 6 && variable == "0") {
                                        $('#resourcescheduleDetail #chkwekSaturday').prop('checked', false);
                                    }
                                    x++;
                                }
                            }
                        }
                        if (resourceSchedule_detail.schfor == "Monthly") {

                            $('.nav-tabs a[href="#Monthly"]').tab('show');


                            if (resourceSchedule_detail.patternevery == "False") {
                                $('#resourcescheduleDetail #rdmntdaay').prop('checked', true);
                                $('#resourcescheduleDetail #txtmntActive').val(resourceSchedule_detail.value);
                                $('#resourcescheduleDetail #txtmntofevry').val(resourceSchedule_detail.pattermonths);
                            }
                            if (resourceSchedule_detail.patternevery == "True") {

                                $('#resourcescheduleDetail #rdmntthe').prop('checked', true);
                                $('#resourcescheduleDetail #txtmnttheofevery').val(resourceSchedule_detail.pattermonths);
                                var s = resourceSchedule_detail.patterdays;
                                var match = s.split(',');
                                var x = 0;
                                for (var a in match) {
                                    var variable = match[a];

                                    if (x == 0 && variable == "1") {
                                        $('#resourcescheduleDetail #chkmntSunday').prop('checked', true);
                                    }
                                    if (x == 0 && variable == "0") {
                                        $('#resourcescheduleDetail #chkmntSunday').prop('checked', false);
                                    }
                                    if (x == 1 && variable == "1") {
                                        $('#resourcescheduleDetail #chkmntMonday').prop('checked', true);
                                    }
                                    if (x == 1 && variable == "0") {
                                        $('#resourcescheduleDetail #chkmntMonday').prop('checked', false);
                                    }
                                    if (x == 2 && variable == "1") {
                                        $('#resourcescheduleDetail #chkmntTuesday').prop('checked', true);
                                    }
                                    if (x == 2 && variable == "0") {
                                        $('#resourcescheduleDetail #chkmntTuesday').prop('checked', false);
                                    }
                                    if (x == 3 && variable == "1") {
                                        $('#resourcescheduleDetail #chkmntWednesday').prop('checked', true);
                                    }
                                    if (x == 3 && variable == "0") {
                                        $('#resourcescheduleDetail #chkmntWednesday').prop('checked', false);
                                    }
                                    if (x == 4 && variable == "1") {
                                        $('#resourcescheduleDetail #chkmntThursday').prop('checked', true);
                                    }
                                    if (x == 4 && variable == "0") {
                                        $('#resourcescheduleDetail #chkmntThursday').prop('checked', false);
                                    }
                                    if (x == 5 && variable == "1") {
                                        $('#resourcescheduleDetail #chkmntFriday').prop('checked', true);
                                    }
                                    if (x == 5 && variable == "0") {
                                        $('#resourcescheduleDetail #chkmntFriday').prop('checked', false);
                                    }
                                    if (x == 6 && variable == "1") {
                                        $('#resourcescheduleDetail #chkmntSaturday').prop('checked', true);
                                    }
                                    if (x == 6 && variable == "0") {
                                        $('#resourcescheduleDetail #chkmntSaturday').prop('checked', false);
                                    }
                                    x++;
                                }
                                var s1 = resourceSchedule_detail.patterweeks;
                                var match1 = s1.split(',');
                                var x1 = 0;
                                for (var a1 in match1) {
                                    var variable1 = match1[a1];

                                    if (x1 == 0 && variable1 == "1") {
                                        $('#resourcescheduleDetail #chkmntFirst').prop('checked', true);
                                    }
                                    if (x1 == 0 && variable1 == "0") {
                                        $('#resourcescheduleDetail #chkmntFirst').prop('checked', false);
                                    }
                                    if (x1 == 1 && variable1 == "1") {
                                        $('#resourcescheduleDetail #chkmntSecond').prop('checked', true);
                                    }
                                    if (x1 == 1 && variable1 == "0") {
                                        $('#resourcescheduleDetail #chkmntSecond').prop('checked', false);
                                    }
                                    if (x1 == 2 && variable1 == "1") {
                                        $('#resourcescheduleDetail #chkmntThird').prop('checked', true);
                                    }
                                    if (x1 == 2 && variable1 == "0") {
                                        $('#resourcescheduleDetail #chkmntThird').prop('checked', false);
                                    }
                                    if (x1 == 3 && variable1 == "1") {
                                        $('#resourcescheduleDetail #chkmntFourth').prop('checked', true);
                                    }
                                    if (x1 == 3 && variable1 == "0") {
                                        $('#resourcescheduleDetail #chkmntFourth').prop('checked', false);
                                    }
                                    if (x1 == 4 && variable1 == "1") {
                                        $('#resourcescheduleDetail #chkmntLast').prop('checked', true);
                                    }
                                    if (x1 == 4 && variable1 == "0") {
                                        $('#resourcescheduleDetail #chkmntLast').prop('checked', false);
                                    }

                                    x1++;
                                }
                            }
                        }
                        if (resourceSchedule_detail.schfor == "Yearly") {

                            $('.nav-tabs a[href="#Yearly"]').tab('show');

                            if (resourceSchedule_detail.patternevery == "False") {
                                $('#resourcescheduleDetail #rdyerEvery').prop('checked', true);
                                $('#resourcescheduleDetail #monthDays').val(resourceSchedule_detail.value);
                                if (resourceSchedule_detail.pattermonths == "1")
                                    $('#resourcescheduleDetail #yearMonth').val("January");
                                else if (resourceSchedule_detail.pattermonths == "2")
                                    $('#resourcescheduleDetail #yearMonth').val("February");
                                else if (resourceSchedule_detail.pattermonths == "3")
                                    $('#resourcescheduleDetail #yearMonth').val("March");
                                else if (resourceSchedule_detail.pattermonths == "4")
                                    $('#resourcescheduleDetail #yearMonth').val("April");
                                else if (resourceSchedule_detail.pattermonths == "5")
                                    $('#resourcescheduleDetail #yearMonth').val("May");
                                else if (resourceSchedule_detail.pattermonths == "6")
                                    $('#resourcescheduleDetail #yearMonth').val("June");
                                else if (resourceSchedule_detail.pattermonths == "7")
                                    $('#resourcescheduleDetail #yearMonth').val("July");
                                else if (resourceSchedule_detail.pattermonths == "8")
                                    $('#resourcescheduleDetail #yearMonth').val("August");
                                else if (resourceSchedule_detail.pattermonths == "9")
                                    $('#resourcescheduleDetail #yearMonth').val("September");
                                else if (resourceSchedule_detail.pattermonths == "10")
                                    $('#resourcescheduleDetail #yearMonth').val("October");
                                else if (resourceSchedule_detail.pattermonths == "11")
                                    $('#resourcescheduleDetail #yearMonth').val("November");
                                else if (resourceSchedule_detail.pattermonths == "12")
                                    $('#resourcescheduleDetail #yearMonth').val("Decmber");;

                            }
                            if (resourceSchedule_detail.patternevery == "True") {
                                $('#resourcescheduleDetail #rdyerthe').prop('checked', true);
                                var s = resourceSchedule_detail.patterdays;
                                var match = s.split(',');
                                var x = 0;
                                for (var a in match) {
                                    var variable = match[a];

                                    if (x == 0 && variable == "1") {
                                        $('#resourcescheduleDetail #chkyerSunday').prop('checked', true);
                                    }
                                    if (x == 0 && variable == "0") {
                                        $('#resourcescheduleDetail #chkyerSunday').prop('checked', false);
                                    }
                                    if (x == 1 && variable == "1") {
                                        $('#resourcescheduleDetail #chkyerMonday').prop('checked', true);
                                    }
                                    if (x == 1 && variable == "0") {
                                        $('#resourcescheduleDetail #chkyerMonday').prop('checked', false);
                                    }
                                    if (x == 2 && variable == "1") {
                                        $('#resourcescheduleDetail #chkyerTuesday').prop('checked', true);
                                    }
                                    if (x == 2 && variable == "0") {
                                        $('#resourcescheduleDetail #chkyerTuesday').prop('checked', false);
                                    }
                                    if (x == 3 && variable == "1") {
                                        $('#resourcescheduleDetail #chkyerWednesday').prop('checked', true);
                                    }
                                    if (x == 3 && variable == "0") {
                                        $('#resourcescheduleDetail #chkyerWednesday').prop('checked', false);
                                    }
                                    if (x == 4 && variable == "1") {
                                        $('#resourcescheduleDetail #chkyerThursday').prop('checked', true);
                                    }
                                    if (x == 4 && variable == "0") {
                                        $('#resourcescheduleDetail #chkyerThursday').prop('checked', false);
                                    }
                                    if (x == 5 && variable == "1") {
                                        $('#resourcescheduleDetail #chkyerFriday').prop('checked', true);
                                    }
                                    if (x == 5 && variable == "0") {
                                        $('#resourcescheduleDetail #chkyerFriday').prop('checked', false);
                                    }
                                    if (x == 6 && variable == "1") {
                                        $('#resourcescheduleDetail #chkyerSaturday').prop('checked', true);
                                    }
                                    if (x == 6 && variable == "0") {
                                        $('#resourcescheduleDetail #chkyerSaturday').prop('checked', false);
                                    }
                                    x++;
                                }
                                var s1 = resourceSchedule_detail.patterweeks;
                                var match1 = s1.split(',');
                                var x1 = 0;
                                for (var a1 in match1) {
                                    var variable1 = match1[a1];

                                    if (x1 == 0 && variable1 == "1") {
                                        $('#resourcescheduleDetail #chkyerFirst').prop('checked', true);
                                    }
                                    if (x1 == 0 && variable1 == "0") {
                                        $('#resourcescheduleDetail #chkyerFirst').prop('checked', false);
                                    }
                                    if (x1 == 1 && variable1 == "1") {
                                        $('#resourcescheduleDetail #chkyerSecond').prop('checked', true);
                                    }
                                    if (x1 == 1 && variable1 == "0") {
                                        $('#resourcescheduleDetail #chkyerSecond').prop('checked', false);
                                    }
                                    if (x1 == 2 && variable1 == "1") {
                                        $('#resourcescheduleDetail #chkyerThird').prop('checked', true);
                                    }
                                    if (x1 == 2 && variable1 == "0") {
                                        $('#resourcescheduleDetail #chkyerThird').prop('checked', false);
                                    }
                                    if (x1 == 3 && variable1 == "1") {
                                        $('#resourcescheduleDetail #chkyerFourth').prop('checked', true);
                                    }
                                    if (x1 == 3 && variable1 == "0") {
                                        $('#resourcescheduleDetail #chkyerFourth').prop('checked', false);
                                    }
                                    if (x1 == 4 && variable1 == "1") {
                                        $('#resourcescheduleDetail #chkyerLast').prop('checked', true);
                                    }
                                    if (x1 == 4 && variable1 == "0") {
                                        $('#resourcescheduleDetail #chkyerLast').prop('checked', false);
                                    }

                                    x1++;
                                }
                                var s2 = resourceSchedule_detail.pattermonths;
                                var match2 = s2.split(',');
                                var x2 = 0;
                                for (var a2 in match2) {
                                    var variable2 = match2[a2];

                                    if (x2 == 0 && variable2 == "1") {
                                        $('#resourcescheduleDetail #chkyerJanuary').prop('checked', true);
                                    }
                                    if (x2 == 0 && variable2 == "0") {
                                        $('#resourcescheduleDetail #chkyerJanuary').prop('checked', false);
                                    }
                                    if (x2 == 1 && variable2 == "1") {
                                        $('#resourcescheduleDetail #chkyerFebruary').prop('checked', true);
                                    }
                                    if (x2 == 1 && variable2 == "0") {
                                        $('#resourcescheduleDetail #chkyerFebruary').prop('checked', false);
                                    }
                                    if (x2 == 2 && variable2 == "1") {
                                        $('#resourcescheduleDetail #chkyerMarch').prop('checked', true);
                                    }
                                    if (x2 == 2 && variable2 == "0") {
                                        $('#resourcescheduleDetail #chkyerMarch').prop('checked', false);
                                    }
                                    if (x2 == 3 && variable2 == "1") {
                                        $('#resourcescheduleDetail #chkyerApril').prop('checked', true);
                                    }
                                    if (x2 == 3 && variable2 == "0") {
                                        $('#resourcescheduleDetail #chkyerApril').prop('checked', false);
                                    }
                                    if (x2 == 4 && variable2 == "1") {
                                        $('#resourcescheduleDetail #chkyerMay').prop('checked', true);
                                    }
                                    if (x2 == 4 && variable2 == "0") {
                                        $('#resourcescheduleDetail #chkyerMay').prop('checked', false);
                                    }
                                    if (x2 == 5 && variable2 == "1") {
                                        $('#resourcescheduleDetail #chkyerJune').prop('checked', true);
                                    }
                                    if (x2 == 5 && variable2 == "0") {
                                        $('#resourcescheduleDetail #chkyerJune').prop('checked', false);
                                    }
                                    if (x2 == 6 && variable2 == "1") {
                                        $('#resourcescheduleDetail #chkyerJuly').prop('checked', true);
                                    }
                                    if (x2 == 6 && variable2 == "0") {
                                        $('#resourcescheduleDetail #chkyerJuly').prop('checked', false);
                                    }
                                    if (x2 == 7 && variable2 == "1") {
                                        $('#resourcescheduleDetail #chkyerAugust').prop('checked', true);
                                    }
                                    if (x2 == 7 && variable2 == "0") {
                                        $('#resourcescheduleDetail #chkyerAugust').prop('checked', false);
                                    }
                                    if (x2 == 8 && variable2 == "1") {
                                        $('#resourcescheduleDetail #chkyerSeptember').prop('checked', true);
                                    }
                                    if (x2 == 8 && variable2 == "0") {
                                        $('#resourcescheduleDetail #chkyerSeptember').prop('checked', false);
                                    }
                                    if (x2 == 9 && variable2 == "1") {
                                        $('#resourcescheduleDetail #chkyerOctober').prop('checked', true);
                                    }
                                    if (x2 == 9 && variable2 == "0") {
                                        $('#resourcescheduleDetail #chkyerOctober').prop('checked', false);
                                    }
                                    if (x2 == 10 && variable2 == "1") {
                                        $('#resourcescheduleDetail #chkyerNovember').prop('checked', true);
                                    }
                                    if (x2 == 10 && variable2 == "0") {
                                        $('#resourcescheduleDetail #chkyerNovember').prop('checked', false);
                                    }
                                    if (x2 == 11 && variable2 == "1") {
                                        $('#resourcescheduleDetail #chkyerDecember').prop('checked', true);
                                    }
                                    if (x2 == 11 && variable2 == "0") {
                                        $('#resourcescheduleDetail #chkyerDecember').prop('checked', false);
                                    }


                                    x2++;
                                }
                            }
                        }
                        
                        //serialize Data.
                        $('#frmresourcescheduleDetail').data('serialize', $('#frmresourcescheduleDetail').serialize());
                        resourcescheduleDetail.ValidateresourcescheduleDetail();

                    });
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    Saveresourceschedules: function () {
        var criteria;
        var criteria1 = 0;
        var start = blockHoursDetail.ConvertTimeformat("24", $("#resourcescheduleDetail #frmtime").val());
        var end = blockHoursDetail.ConvertTimeformat("24", $("#resourcescheduleDetail #totime").val());

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
        if ($("#frmblckhrstime").val() != "")
        {

            var start = blockHoursDetail.ConvertTimeformat("24", $("#resourcescheduleDetail #frmblckhrstime").val());
            var end = blockHoursDetail.ConvertTimeformat("24", $("#resourcescheduleDetail #toblckhrstime").val());

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
                if (criteria1 == 'true')
                {
                    var strMessage = "";
                    var self = $("#resourcescheduleDetail");
                    var myJSON = self.getMyJSON();
                    var patternJSON;
                    var $tab = $('#resourcescheduleDetail #myTabContent'), $active = $tab.find('.tab-pane.active'), text = $active.find('p:hidden').text();
                    //alert(text)
                    if (text == 'Daily') {
                        var self = $("#resourcescheduleDetail #Daily");
                        patternJSON = self.getMyJSON();
                        var json2 = { "pattern": "Daily" };
                        patternJSON = JSON.stringify($.extend(false, {}, JSON.parse(patternJSON), json2));
                    }
                    else if (text == 'Weekly') {
                        var self = $("#resourcescheduleDetail #Weekly");
                        patternJSON = self.getMyJSON();
                        var json2 = { "pattern": "Weekly" };
                        patternJSON = JSON.stringify($.extend(false, {}, JSON.parse(patternJSON), json2));
                    }
                    else if (text == 'Monthly') {
                        var self = $("#resourcescheduleDetail #Monthly");
                        patternJSON = self.getMyJSON();
                        var json2 = { "pattern": "Monthly" };
                        patternJSON = JSON.stringify($.extend(false, {}, JSON.parse(patternJSON), json2));
                    }
                    else if (text == 'Yearly') {
                        var self = $("#resourcescheduleDetail #Yearly");
                        patternJSON = self.getMyJSON();
                        var json2 = { "pattern": "Yearly" };
                        patternJSON = JSON.stringify($.extend(false, {}, JSON.parse(patternJSON), json2));
                    }
                    patternJSON = patternJSON

                    var j = { "name": "Daily" };
                    JSON.stringify(j);
                    if (resourcescheduleDetail.params.mode == "Add") {
                        AppPrivileges.GetFormPrivileges("Resource Schedule", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                            if (strMessage == "") {
                                resourcescheduleDetail.Saveresourceschedule(myJSON, patternJSON).done(function (response) {
                                    if (response.status != false) {
                                        Admin_ResourceSchedule.ResourceScheduleSearch(response.ScheduleId);
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
                    else if (resourcescheduleDetail.params.mode == "Edit") {
                        AppPrivileges.GetFormPrivileges("Resource Schedule", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                            if (strMessage == "") {
                                resourcescheduleDetail.UpdateResourceSchedule(myJSON, resourcescheduleDetail.params.ResScheduleId).done(function (response) {
                                    if (response.status != false) {
                                        Admin_ResourceSchedule.ResourceScheduleSearch(resourcescheduleDetail.params.ResScheduleId);
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
                var self = $("#resourcescheduleDetail");
                var myJSON = self.getMyJSON();
                var patternJSON;
                var $tab = $('#resourcescheduleDetail #myTabContent'), $active = $tab.find('.tab-pane.active'), text = $active.find('p:hidden').text();
                //alert(text)
                if (text == 'Daily') {
                    var self = $("#resourcescheduleDetail #Daily");
                    patternJSON = self.getMyJSON();
                    var json2 = { "pattern": "Daily" };
                    patternJSON = JSON.stringify($.extend(false, {}, JSON.parse(patternJSON), json2));
                }
                else if (text == 'Weekly') {
                    var self = $("#resourcescheduleDetail #Weekly");
                    patternJSON = self.getMyJSON();
                    var json2 = { "pattern": "Weekly" };
                    patternJSON = JSON.stringify($.extend(false, {}, JSON.parse(patternJSON), json2));
                }
                else if (text == 'Monthly') {
                    var self = $("#resourcescheduleDetail #Monthly");
                    patternJSON = self.getMyJSON();
                    var json2 = { "pattern": "Monthly" };
                    patternJSON = JSON.stringify($.extend(false, {}, JSON.parse(patternJSON), json2));
                }
                else if (text == 'Yearly') {
                    var self = $("#resourcescheduleDetail #Yearly");
                    patternJSON = self.getMyJSON();
                    var json2 = { "pattern": "Yearly" };
                    patternJSON = JSON.stringify($.extend(false, {}, JSON.parse(patternJSON), json2));
                }
                patternJSON = patternJSON

                var j = { "name": "Daily" };
                JSON.stringify(j);
                if (resourcescheduleDetail.params.mode == "Add") {
                    AppPrivileges.GetFormPrivileges("Resource Schedule", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            resourcescheduleDetail.Saveresourceschedule(myJSON, patternJSON).done(function (response) {
                                if (response.status != false) {
                                    Admin_ResourceSchedule.ResourceScheduleSearch(response.ScheduleId);
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
                else if (resourcescheduleDetail.params.mode == "Edit") {
                    AppPrivileges.GetFormPrivileges("Resource Schedule", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            resourcescheduleDetail.UpdateResourceSchedule(myJSON, resourcescheduleDetail.params.ResScheduleId).done(function (response) {
                                if (response.status != false) {
                                    Admin_ResourceSchedule.ResourceScheduleSearch(resourcescheduleDetail.params.ResScheduleId);
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

    ValidateresourcescheduleDetail: function () {
        $('#frmresourcescheduleDetail')
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
                       group: '.col-md-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   //facility: {
                   //    group: '.col-md-3',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //},

                   // irfan

                   //schreason: {
                   //    group: '.col-md-3',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //},

                   fromdate: {
                       group: '.col-md-2',
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
                       group: '.col-md-2',
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
                       group: '.col-md-2',
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
                       group: '.col-md-2',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   //PatientAllowed: {
                   //    group: '.col-md-2',
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
                   txtactiveweek: {
                       group: '.col-md-8',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   txtmntofevry: {
                       group: '.col-md-2',
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
                   DailyEveryDays: {
                       group: '.col-sm-6',
                       enabled: true,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   txtmntofevryM: {
                       group: '.size35',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   txtmntActive: {
                       group: '.size35',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   todateY: {
                       group: '.size100',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },

                   todateYDay: {
                       group: '.size120',
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
            resourcescheduleDetail.Saveresourceschedules();
        });
    },

    EntityBaseData: function (control) {

        var entityID = $(control).find(":selected").attr('refvalue') || "";
        if (entityID != "") {
            CacheManager.BindDropDownsByEntityID('#resourcescheduleDetail #ddlfacility', 'GetFacility', true, entityID);
            //CacheManager.BindDropDownsByEntityID('#resourcescheduleDetail #ddlschreason', 'GetBlockReasons', false, entityID);
            //CacheManager.BindDropDownsByEntityID('#resourcescheduleDetail #blockreason', 'GetBlockReasons', false, entityID);
            $("#resourcescheduleDetail #ddlfacility").removeAttr('disabled');
            //$("#resourcescheduleDetail #ddlschreason").removeAttr('disabled');
            //$("#resourcescheduleDetail #blockreason").removeAttr('disabled');

        } else {
            resourcescheduleDetail.SetFieldsDefault();
        }
    },

    SetFieldsDefault: function () {

        $("#resourcescheduleDetail #ddlfacility").prop('disabled', 'disabled');
        //$("#resourcescheduleDetail #blockreason").prop('disabled', 'disabled');
        $("#resourcescheduleDetail #ddlfacility").val('');
        //$("#resourcescheduleDetail #blockreason").val('');
    },

    FillDuration: function (control) {

        var ScheduleReasonID = $(control).val();

        resourcescheduleDetail.FillScheduleReasonDuration(ScheduleReasonID).done(function (response) {
            if (response.status != false) {
                var reasonduration = JSON.parse(response.ProviderScheduleFill_JSON);
                $('#resourcescheduleDetail #slotminutes').val(reasonduration.slotminutes);
                $('#frmresourcescheduleDetail').bootstrapValidator('revalidateField', 'slotminutes');

            }
            else {

            }
        });
    },

    Saveresourceschedule: function (ResourceScheduleData, ResourceSchedulePatternData) {
        var data = "ResourceScheduleData=" + ResourceScheduleData + "&ResourceSchedulePatternData=" + ResourceSchedulePatternData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_RESOURCESCHEDULE_DETAIL", "SAVE_RESOURCESCHEDULE");
    },

    UpdateResourceSchedule: function (ResourceScheduleData, ResScheduleID) {
        var data = "ResourceScheduleData=" + ResourceScheduleData + "&ResScheduleID=" + ResScheduleID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_RESOURCESCHEDULE_DETAIL", "UPDATE_RESOURCESCHEDULE");
    },

    FillResourceSchedule: function (ResScheduleID) {
        var data = "ResScheduleID=" + ResScheduleID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_RESOURCESCHEDULE_DETAIL", "FILL_RESOURCESCHEDULE");
    },

    UpdateResourceScheduleActiveInactive: function (ResScheduleId, IsActive) {
        var data = "ResScheduleId=" + ResScheduleId + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_RESOURCESCHEDULE_DETAIL", "UPDATE_RESOURCESCHEDULE_ACTIVE_INACTIVE");
    },

    FillScheduleReasonDuration: function (ScheduleReasonID) {
        var data = "ScheduleReasonID=" + ScheduleReasonID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_RESOURCESCHEDULE_DETAIL", "FILL_SCHEDULEREASON_DURATION");
    },

    UnLoad: function () {

        utility.UnLoadDialog("frmresourcescheduleDetail", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });

    },

    //***** Validation *****\\

    DailyChecks: function () {

        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('DailyEveryDays', true);

        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('weekdays[]', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('txtactiveweek', false);

        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('txtmntofevryM', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('txtmntActive', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('txtmntofevry', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('themonths[]', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('theweeksM[]', false);

        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('theQuartersY[]', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('themonthsY[]', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('theDaysY[]', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('todateY', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('todateYDay', false);
    },

    WeeklyChecks: function () {

        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('DailyEveryDays', false);

        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('weekdays[]', true);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('txtactiveweek', false);

        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('txtmntofevryM', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('txtmntActive', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('txtmntofevry', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('themonths[]', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('theweeksM[]', false);

        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('theQuartersY[]', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('themonthsY[]', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('theDaysY[]', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('todateY', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('todateYDay', false);
    },

    MonthlyChecks: function () {

        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('DailyEveryDays', false);

        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('weekdays[]', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('txtactiveweek', false);

        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('txtmntofevryM', true);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('txtmntActive', true);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('txtmntofevry', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('themonths[]', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('theweeksM[]', false);

        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('theQuartersY[]', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('themonthsY[]', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('theDaysY[]', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('todateY', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('todateYDay', false);

    },

    YearlyChecks: function () {

        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('DailyEveryDays', false);

        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('weekdays[]', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('txtactiveweek', false);

        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('txtmntofevryM', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('txtmntActive', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('txtmntofevry', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('themonths[]', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('theweeksM[]', false);

        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('theQuartersY[]', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('themonthsY[]', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('theDaysY[]', false);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('todateY', true);
        $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('todateYDay', true);


    },

    //------------------------------------------

    FillScheduleReason: function (ScheduleReasonId, ShortName, Duration, event) {

        if (event != null) {
            event.stopPropagation();
        }

        UnloadActionPan("resourcescheduleDetail");

        $('#resourcescheduleDetail #txtSchReason').val(ShortName);
        $('#resourcescheduleDetail #hfSchReasonId').val(ScheduleReasonId);
        $('#resourcescheduleDetail #slotminutes').val(Duration);

        if ($('#frmresourcescheduleDetail').data('bootstrapValidator') != null && typeof $('#frmresourcescheduleDetail').data('bootstrapValidator') != 'undefined') {
            $('#frmresourcescheduleDetail').bootstrapValidator('revalidateField', 'slotminutes');
            
        }

        resourcescheduleDetail.BindScheduleReasons();
    },

    BindScheduleReasons: function () {
        var SchReason = $('#resourcescheduleDetail #txtSchReason').val();
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

            $('#resourcescheduleDetail #txtSchReason').autocomplete({
                autoFocus: true,
                source: AllSchReasons,
                open: function (event, ui) { disable = true },
                close: function (event, ui) {
                    disable = false; $(this).focus();
                },
                select: function (event, ui) {
                    setTimeout(function () {
                        $('#resourcescheduleDetail #txtSchReason').val(ui.item.value);
                        $('#resourcescheduleDetail #hfSchReasonId').val(ui.item.id);
                        $('#resourcescheduleDetail #slotminutes').val(ui.item.Duration);
                        $('#frmresourcescheduleDetail').bootstrapValidator('revalidateField', 'slotminutes');
                    }, 100);

                }
            }).blur(function () {
                setTimeout(function () {
                    utility.ValidateAutoComplete($('#resourcescheduleDetail #txtSchReason'), 'resourcescheduleDetail #hfSchReasonId', false, null, null, null);
                }, 200);
            });
           // $('#resourcescheduleDetail #txtSchReason').autocomplete("search");

        });

        //--------------------
    },

    OpenScheduleReason: function () {

        var params = [];
        params["ScheduleReasonId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "resourcescheduleDetail";
        LoadActionPan('Admin_ScheduleReason', params);

    },

    //-----------------------
    FillBlockReason: function (ScheduleReasonId, ShortName, Duration, event) {
        if (event != null) {
            event.stopPropagation();
        }

        UnloadActionPan("resourcescheduleDetail");

        if ($('#resourcescheduleDetail #txtBlckReason').data('kendoAutoComplete'))
            utility.SetKendoAutoCompleteSourceforValidate($('#resourcescheduleDetail #txtBlckReason'), ShortName, $('#resourcescheduleDetail #hfBlckReasonId'), ScheduleReasonId);
    },

    BindBlockReasons: function () {
        var Ctrl = $('#resourcescheduleDetail #txtBlckReason');
        var func = function () { return resourcescheduleDetail.GetScheduleReasonsArray(Ctrl.val()) };
        var hfCtrl = $('#resourcescheduleDetail #hfBlckReasonId');
        var onSelect = function (e) {
            $('#appointmentDetail #txtBlckReason').val(e.value);
            $('#appointmentDetail #hfBlckReasonId').val(e.id);
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
        params["ParentCtrl"] = "resourcescheduleDetail";
        params["IsBlockReason"] = true;
        LoadActionPan('Admin_ScheduleReason', params);

    },


    BlockReasonOnblur: function () {
        setTimeout(function () {
            if ($("#frmresourcescheduleDetail #hfBlckReasonId").val() != "") {
                $('#frmresourcescheduleDetail #frmblckhrstime').timepicker('setTime', $('#frmresourcescheduleDetail #frmtime').data("timepicker").getTime());
                $('#frmresourcescheduleDetail #toblckhrstime').timepicker('setTime', $('#frmresourcescheduleDetail #totime').data("timepicker").getTime());
                $('#frmresourcescheduleDetail #frmblckhrstime').attr('disabled', false);
                $('#frmresourcescheduleDetail #toblckhrstime').attr('disabled', false);
                if ($('#frmresourcescheduleDetail').data('bootstrapValidator') != null && typeof $('#frmresourcescheduleDetail').data('bootstrapValidator') != 'undefined') {
                    $('#frmresourcescheduleDetail').bootstrapValidator('revalidateField', 'blckhrsfromtime');
                    $('#frmresourcescheduleDetail').bootstrapValidator('revalidateField', 'blckhrstotime');
                }
            }
            else {

                $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('blckhrsfromtime', false);
                $('#frmresourcescheduleDetail').data('bootstrapValidator').enableFieldValidators('blckhrstotime', false);
                
                $('#frmresourcescheduleDetail #frmblckhrstime').timepicker('setTime', '');
                $('#frmresourcescheduleDetail #toblckhrstime').timepicker('setTime', '');
                $('#frmresourcescheduleDetail #frmblckhrstime').attr('disabled', true);
                $('#frmresourcescheduleDetail #toblckhrstime').attr('disabled', true);

            }
        }, 300);
    },


    ShowHistory: function () {
        var PanelID = 'resourcescheduleDetail';
        var ParentCtrl = 'resourcescheduleDetail';
        var ProfileName = 'Resource Schedule';
        var DBTableName = 'ResourceSchedule';
        var ColumnKeyId = resourcescheduleDetail.params.ResScheduleId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);

    },
}