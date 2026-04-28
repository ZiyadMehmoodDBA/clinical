iTrack_Dashboard = {
    params: [],
    bIsFirstLoad: true,
    RequireSubmit: true,
    ActualVisitID: null,
    ActualChargeCaptureID: null,
    IsChargeCapture: null,
    isFullWidth: true,
    Load: function (params) {

        iTrack_Dashboard.params = params;
        if (iTrack_Dashboard.bIsFirstLoad) {
            iTrack_Dashboard.bIsFirstLoad = false;

            //iTrack_Dashboard.readyFunction();
            setTimeout(function () {
                //$('#pnliTrackDashboard #ReportingType input:checkbox[name="ScoreType"]').on('change', function () {
                //    $('#pnliTrackDashboard #ReportingType input:checkbox[name="ScoreType"]').not(this).prop('checked', false);
                //});
                iTrack_Dashboard.readyFunction();
                if (globalAppdata["DefaultProviderId"] != "") {
                    if (globalAppdata["iTrackDashboardIds"].indexOf('1,2') > -1) {
                        iTrack_Dashboard.isFullWidth = false;
                    }
                    $('#noDefaultProvider').addClass('hidden');
                    if (globalAppdata["iTrackDashboardIds"].indexOf('1') > -1) {
                        if (iTrack_Dashboard.isFullWidth) {
                            $('#widgetMU').removeClass('col-md-6').addClass('col-md-offset-2 col-md-8');
                        }
                        $('#widgetMU').removeClass('hidden');
                        $('#txtmuprovider').append(globalAppdata["DefaultProviderName"]);
                    }
                    if (globalAppdata["iTrackDashboardIds"].indexOf('2') > -1) {
                        if (iTrack_Dashboard.isFullWidth) {
                            $('#widgetMIPS').removeClass('col-md-6').addClass('col-md-offset-2 col-md-8');
                        }
                        $('#widgetMIPS').removeClass('hidden');
                        $('#txtMIPSProvider').append(globalAppdata["DefaultProviderName"]);
                    }
                    if (globalAppdata["iTrackDashboardIds"] == '1') {
                        $('#widgetMU').children('div.panel-body').addClass('itrack-graph');
                    } else if (globalAppdata["iTrackDashboardIds"] == '2') {
                        $('#widgetMIPS').children('div.panel-body').addClass('itrack-graph');
                    }

                }
            }, 100);
            if (globalAppdata["DefaultProviderId"] != "") {
                if (globalAppdata["iTrackDashboardIds"]) {
                    if (globalAppdata["iTrackDashboardIds"].indexOf('1') > -1) {
                        iTrack_Dashboard.searchMUScore();
                    }
                    if (globalAppdata["iTrackDashboardIds"].indexOf('2') > -1) {
                        iTrack_Dashboard.calculateMIPScore(null);
                    }
                }
            }
        }
    },
    readyFunction: function () {

        (function ($) {
            'use strict';
            $(function () {
                $('[data-plugin-ios-switch]').each(function () {
                    var $this = $(this);

                    $this.themePluginIOS7Switch();
                });
            });
        }).apply(this, [jQuery]);
    },
    updateiTrackReportingType: function (id) {
        iTrack_Dashboard.updateiTrackReportingType_DBCall(id).done(function (response) {
            response = JSON.parse(response);

            if (response.status != false) {
                utility.DisplayMessages(response.Message, 1);
            }
        });
    },
    updateiTrackReportingType_DBCall: function (id, obj) {

        var iTrackReportingType = "";
        if (obj.checked == false) {
            iTrackReportingType = "Individual";
        } else if (obj.checked == true) {
            iTrackReportingType = "Group";
        }
        var objData = new Object();
        objData["ObjectId"] = id;
        objData["iTrackReportingType"] = iTrackReportingType;
        objData["commandType"] = "updateitrackreportingtype";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "iTrack", "iTrackAdmin");

    },
    calculateMIPScore: function (from, obj) {

        iTrack_Dashboard.Search_MIPSIndiviualPreferences_DBCall().done(function (response) {
            response = JSON.parse(response);

            if (response.status != false && response.IndividualProCount > 0) {

                var list = JSON.parse(response.IndividualProCountLoad_JSON);
                if (from == "update") {
                    //iTrack_Dashboard.updateiTrackReportingType(list[0].ObjectId);
                    iTrack_Dashboard.updateiTrackReportingType_DBCall(list[0].ObjectId, obj).done(function (response) {
                        response = JSON.parse(response);

                        if (response.status != false) {

                            iTrack_Dashboard.Search_MIPSIndiviualPreferences_DBCall().done(function (response) {
                                response = JSON.parse(response);

                                if (response.status != false && response.IndividualProCount > 0) {

                                    var list1 = JSON.parse(response.IndividualProCountLoad_JSON);

                                    if (list1[0].ReportingType == "Group") {
                                        $('#pnliTrackDashboard #ReportingType').removeClass('hidden');
                                        if (list1[0].iTrackReportingType == "Group") {
                                            iTrack_Dashboard.params.GroupId = list1[0].GroupId;
                                            iTrack_Dashboard.params.GroupName = list1[0].GroupName;
                                            iTrack_Dashboard.params.GroupTIN = list1[0].GroupTIN;
                                            iTrack_Dashboard.searchGroupMIPScore(list1[0].GroupTIN, list1[0].GroupId);
                                        } else {
                                            iTrack_Dashboard.params.NPI = list1[0].NPI;
                                            iTrack_Dashboard.params.GroupId = null;
                                            iTrack_Dashboard.params.GroupName = null;
                                            iTrack_Dashboard.params.GroupTIN = null;
                                            iTrack_Dashboard.searchMIPScore();
                                        }

                                    } else {
                                        iTrack_Dashboard.params.NPI = list1[0].NPI;
                                        iTrack_Dashboard.params.GroupId = null;
                                        iTrack_Dashboard.params.GroupName = null;
                                        iTrack_Dashboard.params.GroupTIN = null;
                                        iTrack_Dashboard.searchMIPScore();
                                    }

                                }
                            });
                        }
                    });
                } else {
                    if (list[0].ReportingType == "Group") {
                        $('#pnliTrackDashboard #ReportingType').removeClass('hidden');
                        if (list[0].iTrackReportingType == "Group") {
                            iTrack_Dashboard.params.GroupId = list[0].GroupId;
                            iTrack_Dashboard.params.GroupName = list[0].GroupName;
                            iTrack_Dashboard.params.GroupTIN = list[0].GroupTIN;
                            $('#switchReporting').prev('div').removeClass('off');
                            $('#switchReporting').prev('div').addClass('on');
                            //$('#pnliTrackDashboard #chkGroupScore').prop('checked', true);
                            iTrack_Dashboard.searchGroupMIPScore(list[0].GroupTIN, list[0].GroupId);
                        } else {
                            iTrack_Dashboard.params.NPI = list[0].NPI;
                            $('#switchReporting').prev('div').removeClass('on');
                            $('#switchReporting').prev('div').addClass('off');
                            //$('#pnliTrackDashboard #chkIndividualScore').prop('checked', true);
                            iTrack_Dashboard.searchMIPScore();
                        }

                    } else {
                        if (list[0].IsReporting == "No") {
                            iTrack_Dashboard.params.NotReportingReason = list[0].NotReportingReason;
                            iTrack_Dashboard.params.NPI = list[0].NPI;
                        }
                        iTrack_Dashboard.searchMIPScore();
                    }
                }
            }
        });
    },
    searchGroupMIPScoreDB_Call: function (TIN, groupId) {

        var objData = new Object();

        objData["ProviderId"] = "0";
        objData["GroupId"] = groupId;
        objData["DateFrom"] = "01/01/2018";
        objData["DateTo"] = "12/31/2018";
        objData["TIN"] = TIN;

        objData["commandType"] = "loadmipskpis";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "iTrack", "iTrackDashBoard");
    },
    openMUReport: function () {

        if (!$('#iTrackMenuMeaningfulUse').hasClass('nav-expanded')) {
            $('#iTrackMenuMeaningfulUse').addClass('nav-expanded');
        }
        $('#pnliTrack_MUReport').css('display', 'block');
        params.FromDashBoard = true;
        SelectTab('iTrackTabMUReport', 'true');
    },
    openMIPSummary: function () {
        if (!$('#iTrackMenuMIPS').hasClass('nav-expanded')) {
            $('#iTrackMenuMIPS').addClass('nav-expanded');
        }
        $('#pnliTrackMIPSummary').css('display', 'block');
        params.FromDashBoard = true;
        params.NPI = iTrack_Dashboard.params.NPI;
        params.GroupId = iTrack_Dashboard.params.GroupId;
        params.GroupName = iTrack_Dashboard.params.GroupName;
        params.GroupTIN = iTrack_Dashboard.params.GroupTIN;
        params.NotReportingReason = iTrack_Dashboard.params.NotReportingReason;
        SelectTab('iTrackTabMIPSummary', 'true');
    },

    searchMIPScore: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("iTrackDashBoard", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                iTrack_Dashboard.searchMIPScoreDB_Call().done(function (response) {
                   
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var categoryValues = JSON.parse(response.MIPSKPIsLoad_JSON);
                        iTrack_Dashboard.params.totalScore = 0;
                        $.each(categoryValues, function (i, item) {
                            if (item.Category == "Quality") {

                                iTrack_Dashboard.params.qualityAchievedPoints = parseInt(item.value);
                            } else if (item.Category == "PI") {

                                iTrack_Dashboard.params.PIAchievedPoints = parseInt(item.value);
                            } else if (item.Category == "IA") {

                                iTrack_Dashboard.params.ImprovementActivitiesAchievedPoints = parseInt(item.value);
                            } else if (item.Category == "Cost") {
                                item.value = 10;
                                iTrack_Dashboard.params.CostAchievedPoints = parseInt(item.value);
                            }
                            iTrack_Dashboard.params.totalScore += parseInt(item.value);
                        });
                        var data = [{
                            category: "Quality",
                            value: 50,
                            color: "#418fc3",
                            stitle: "Quality <br/> " + iTrack_Dashboard.params.qualityAchievedPoints + " Pts"
                        }, {
                            category: "Promoting Interoperability",
                            value: 25,
                            color: "#88d612",
                            stitle: "Promoting Interoperability <br/> " + iTrack_Dashboard.params.PIAchievedPoints + " Pts"
                        }, {
                            category: "IA",
                            value: 15,
                            color: "#068c35",
                            stitle: "IA <br/> " + iTrack_Dashboard.params.ImprovementActivitiesAchievedPoints + " Pts"
                        }, {
                            category: "Cost",
                            value: 10,
                            color: "#033939",
                            stitle: "Cost <br/> " + iTrack_Dashboard.params.CostAchievedPoints + " Pts"
                        }];

                        iTrack_Dashboard.createChartMIPS(data);

                    }
                });
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    searchGroupMIPScore: function (TIN, groupId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("iTrackDashBoard", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                iTrack_Dashboard.searchGroupMIPScoreDB_Call(TIN, groupId).done(function (response) {
                    if (response.status != false) {
                        response = JSON.parse(response);
                        var categoryValues = JSON.parse(response.MIPSKPIsLoad_JSON);
                        iTrack_Dashboard.params.totalScore = 0;
                        $.each(categoryValues, function (i, item) {
                            if (item.Category == "Quality") {

                                iTrack_Dashboard.params.qualityAchievedPoints = parseInt(item.value);
                            } else if (item.Category == "PI") {

                                iTrack_Dashboard.params.PIAchievedPoints = parseInt(item.value);
                            } else if (item.Category == "IA") {

                                iTrack_Dashboard.params.ImprovementActivitiesAchievedPoints = parseInt(item.value);
                            } else if (item.Category == "Cost") {
                                item.value = 10;
                                iTrack_Dashboard.params.CostAchievedPoints = parseInt(item.value);
                            }
                            iTrack_Dashboard.params.totalScore += parseInt(item.value);
                        });
                        var data = [{
                            category: "Quality",
                            value: 50,
                            color: "#418fc3",
                            stitle: "Quality <br/> " + iTrack_Dashboard.params.qualityAchievedPoints + " Pts"
                        }, {
                            category: "Promoting Interoperability",
                            value: 25,
                            color: "#88d612",
                            stitle: "Promoting Interoperability <br/> " + iTrack_Dashboard.params.PIAchievedPoints + " Pts"
                        }, {
                            category: "IA",
                            value: 15,
                            color: "#068c35",
                            stitle: "IA <br/> " + iTrack_Dashboard.params.ImprovementActivitiesAchievedPoints + " Pts"
                        }, {
                            category: "Cost",
                            value: 10,
                            color: "#033939",
                            stitle: "Cost <br/> " + iTrack_Dashboard.params.CostAchievedPoints + " Pts"
                        }];

                        iTrack_Dashboard.createChartMIPS(data);

                    }
                });
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    getProviderPreference: function () {

        iTrack_Dashboard.Search_MIPSIndiviualPreferences_DBCall().done(function (response) {
            response = JSON.parse(response);

            if (response.status != false && response.IndividualProCount > 0) {

                var list = JSON.parse(response.IndividualProCountLoad_JSON);
                if (list[0].ReportingType == "Group") {
                    $('#pnliTrackDashboard #ReportingType').removeClass('hidden');
                }
            }
        });
    },
    Search_MIPSIndiviualPreferences_DBCall: function () {

        var objData = new Object();
        objData["PageNumber"] = 1;
        objData["RowsPerPage"] = 15;

        objData["ProviderId"] = globalAppdata["DefaultProviderId"];
        objData["Specialty"] = null;
        objData["NPI"] = null;
        objData["EntityId"] = "100";
        objData["PracticeType"] = null;
        objData["MIPSEligibilityStatus"] = null;
        objData["InEligibileReason"] = null;
        objData["ReportingType"] = null;
        objData["ReportingMethod"] = null;
        objData["IsReporting"] = null;
        objData["IsActive"] = true;
        objData["commandType"] = "loadindividualprovider";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "iTrack", "iTrackAdmin");

    },
    generateMuStageReport_DBCall: function () {
        iTrack_MUReport.calculateFromDateToDate();
        var objData = {};
        objData["Provider"] = globalAppdata["DefaultProviderId"];
        objData["FromDate"] = '01/01/2018';
        objData["ToDate"] = '12/31/2018';
        objData["ReportName"] = "MU Stage 3 Report";
        objData["commandType"] = "SEARCH_MUStageReport1";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Report", "MUReport");
    },
    searchMUScore: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("iTrackDashBoard", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                iTrack_Dashboard.generateMuStageReport_DBCall().done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var measures = JSON.parse(response.MU_JSON);
                        var values = [];
                        if (measures.length > 0) {
                            var passedCount = 0;
                            $.each(measures, function (i, item) {
                                if (item.Measure != "") {

                                    if (item.IsObjectCompleted == "True") {
                                        passedCount++;
                                    }
                                    if (item.Measure == "CPOE Labs") {
                                        item.Measure = "CPOE Laboratory";
                                    }
                                    if (item.Measure == "Secure Message") {
                                        item.Measure = "Secure Messaging";
                                    }
                                    var category = '';
                                    var color = '';
                                    var stitle = '';
                                    var performanceRate = '';

                                    if (parseInt(item.PerfromanceRate1) <= 0) {
                                        color = '#ff5a5a';
                                        category = item.Measure;
                                        stitle = item.Measure;
                                        performanceRate = item.PerfromanceRate1;
                                    } else if (parseInt(item.PerfromanceRate1) > 0 && parseInt(item.PerfromanceRate1) < parseInt(item.RequiredTarget)) {
                                        color = '#F5F573';
                                        category = item.Measure;
                                        stitle = item.Measure;
                                        performanceRate = item.PerfromanceRate1;
                                    } else if (parseInt(item.PerfromanceRate1) >= parseInt(item.RequiredTarget)) {
                                        color = '#78ff78';
                                        category = item.Measure;
                                        stitle = item.Measure;
                                        performanceRate = item.PerfromanceRate1;
                                    }

                                    values.push({

                                        category: category,
                                        value: 10,
                                        color: color,
                                        stitle: stitle,
                                        performanceRate: parseInt(item.PerfromanceRate1).toString()
                                    });
                                }
                            });
                        }
                        var data = [(values)];
                        iTrack_Dashboard.createChartforMU(data[0], passedCount, measures.length);
                    }
                });
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    searchMIPScoreDB_Call: function () {

        var objData = new Object();
        objData["ProviderId"] = globalAppdata["DefaultProviderId"];
        objData["commandType"] = "loadmipskpis";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "iTrack", "iTrackDashBoard");
    },
    createChartMIPS: function (dataSource) {
        var center;
        var radius;
        //var sum = 0;
        //$.each(dataSource, function (i, item) {
        //    sum = sum + item.value;
        //})
        $("#MIPSchart").kendoChart({

            legend: {
                visible: false
            },
            title: {
                position: "bottom",
                text: "Submission required by 31st March 2019",
                font: "background-color:#418fc3",
                color: "black"
            },
            chartArea: {
                background: "",
                width: 490,
                height: 400
            },
            seriesDefaults: {
                type: "donut",
                overlay: { "gradient": "none" },
                padding: 0
            },
            series: [{
                type: "donut",
                startAngle: 150,
                visual: function (e) {
                    // Obtain parameters for the segments
                    // Will run many times, but that's not an issue
                    center = e.center;
                    radius = e.radius;

                    // Create default visual
                    return e.createVisual();
                },
                data: dataSource,
            }],
            tooltip: {
                visible: true,
                template: "#=dataItem.stitle#"
            },
            render: function (e) {
                var draw = kendo.drawing;
                var geom = kendo.geometry;
                var chart = e.sender;


                var circleGeometry = new geom.Circle(center, radius);
                var bbox = circleGeometry.bbox();


                //var text = new draw.Text("12111111111111ssasa", [0, 0], {
                //    font: "18px Verdana,Arial,sans-serif"
                //});


                //draw.align([text], bbox, "center");
                //draw.vAlign([text], bbox, "center");
                var textTarget = new draw.Text("Target", [bbox.origin.x + bbox.size.width - 198, bbox.origin.y + bbox.size.height / 3, ], {
                    font: "12px background-color:#418fc3",
                    fill: { color: "black", rotation: 90 }
                    //fill:{rotation:90}
                });

                var textAchieved = new draw.Text("Achieved", [bbox.origin.x + bbox.size.width - 203, bbox.origin.y + bbox.size.height / 2.5], {
                    font: "12px background-color:#418fc3",
                });
                var textVal = new draw.Text(iTrack_Dashboard.params.totalScore, [bbox.origin.x + bbox.size.width - 192, bbox.origin.y + bbox.size.height / 2.15], {
                    font: "bold 24px sans-serif",
                });
                var textPoints = new draw.Text("Points", [bbox.origin.x + bbox.size.width - 196, bbox.origin.y + bbox.size.height / 1.75], {
                    font: "bold 13px sans-serif",
                });
                var textQuality = new draw.Text("Weightage 50%", [bbox.origin.x + bbox.size.width - 188, bbox.origin.y + bbox.size.height / 1.25], {
                    font: "10px background-color:#418fc3",
                    fill: { color: "white", rotation: 45 },
                });
                var textACI = new draw.Text("Weightage 25%", [bbox.origin.x + bbox.size.width - 340, bbox.origin.y + bbox.size.height / 2.5], {
                    font: "10px background-color:#418fc3",
                    fill: { color: "white", rotation: 45 },
                });
                var textIA = new draw.Text("Weightage 15%", [bbox.origin.x + bbox.size.width - 225, bbox.origin.y + bbox.size.height / 10], {
                    font: "10px background-color:#418fc3",
                    fill: { color: "white", rotation: 45 },
                });
                var textCost = new draw.Text("Weightage 10%", [bbox.origin.x + bbox.size.width - 125, bbox.origin.y + bbox.size.height / 5.5], {
                    font: "10px background-color:#418fc3",
                    //color: "#068c35",
                    fill: { color: "white", rotation: 45 },

                });
                // Draw it on the Chart drawing surface
                //e.sender.surface.draw(text);
                e.sender.surface.draw(textTarget);
                e.sender.surface.draw(textAchieved);
                e.sender.surface.draw(textVal);
                e.sender.surface.draw(textPoints);
                e.sender.surface.draw(textQuality);
                e.sender.surface.draw(textACI);
                e.sender.surface.draw(textIA);
                e.sender.surface.draw(textCost);
            }
        });
        $('#pnliTrackDashboard #lblQuality').text(iTrack_Dashboard.params.qualityAchievedPoints);
        $('#pnliTrackDashboard #lblPI').text(iTrack_Dashboard.params.PIAchievedPoints);
        $('#pnliTrackDashboard #lblIA').text(iTrack_Dashboard.params.ImprovementActivitiesAchievedPoints);
        $('#pnliTrackDashboard #lblCost').text(iTrack_Dashboard.params.CostAchievedPoints);
        $('#pnliTrackDashboard #lblTotalScore').text(iTrack_Dashboard.params.totalScore);
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
    },
    createChartforMU: function (datasource, passedMeasures, totalMeasures) {
        var center;
        var radius;
        var submissionStatus = "Performance Not Met for All Measures, Not ready for submission";
        if (parseInt(passedMeasures) == parseInt(totalMeasures)) {
            submissionStatus = "Performance Met for All Measures, ready for submission";
        }
        $("#MUchart").kendoChart({
            legend: {
                visible: false
            },
            title: {
                position: "bottom",
                text: "Passed " + passedMeasures + " out of " + totalMeasures + " Objectives \n \n" + submissionStatus + "",
                font: "background-color:#418fc3",
                color: "black"
            },
            chartArea: {
                background: "",
                width: 490,
                height: 400
            },
            seriesDefaults: {
                type: "donut",

                labels: {
                    visible: true,
                    font: "12px sans-serif",
                    background: "transparent"
                },
                overlay: { "gradient": "none" },
                padding: 0
            },
            series: [{
                type: "donut",
                startAngle: 150,
                visual: function (e) {

                    center = e.center;
                    radius = e.radius;
                    return e.createVisual();
                },
                data: datasource,
                labels: {
                    visible: true,
                    background: "transparent",
                    template: "#= dataItem.performanceRate #"
                }
            }],
            tooltip: {
                visible: true,
                template: "#=dataItem.stitle#"
            },
            render: function (e) {
                var draw = kendo.drawing;
                var geom = kendo.geometry;
                var chart = e.sender;

                var circleGeometry = new geom.Circle(center, radius);
                var bbox = circleGeometry.bbox();

                var text = new draw.Text("", [187.39999999999998, 159.53333333333336], {
                    font: "bold 16px Verdana,Arial,sans-serif"
                });

                draw.align([text], bbox, "center");
                draw.vAlign([text], bbox, "center");

                var textTarget = new draw.Text("Performance", [bbox.origin.x + bbox.size.width - 202, bbox.origin.y + bbox.size.height / 2.8, ], {
                    font: " 12px sans-serif"
                });
                var textAchieved = new draw.Text("Met", [bbox.origin.x + bbox.size.width - 180, bbox.origin.y + bbox.size.height / 2.4], {
                    font: " 12px sans-serif"
                });
                var measuresPassed = new draw.Text(passedMeasures, [bbox.origin.x + bbox.size.width -176, bbox.origin.y + bbox.size.height / 2.1], {
                    font: "bold 24px sans-serif"
                });
                var subject = new draw.Text("Measures", [bbox.origin.x + bbox.size.width - 196, bbox.origin.y + bbox.size.height / 1.8], {
                    font: "bold 13px sans-serif"
                });
                e.sender.surface.draw(text);
                e.sender.surface.draw(textTarget);
                e.sender.surface.draw(textAchieved);
                e.sender.surface.draw(measuresPassed);
                e.sender.surface.draw(subject);
            }
        });
    },
    createCharteforCQMs: function (datasource) {
        var center;
        var radius;

        $("#eCQMschart").kendoChart({
            title: {
                position: "bottom",
                text: " "
            },
            legend: {
                visible: true,
                position: "bottom",
            },
            chartArea: {
                background: ""
            },
            series: [{
                type: "donut",
                startAngle: 150,
                visual: function (e) {
                    // Obtain parameters for the segments
                    // Will run many times, but that's not an issue
                    center = e.center;
                    radius = e.radius;

                    // Create default visual
                    return e.createVisual();
                },
                data: datasource
            }],
            tooltip: {
                visible: true,
                format: "{0}%"
            },
            render: function (e) {
                var draw = kendo.drawing;
                var geom = kendo.geometry;
                var chart = e.sender;


                var circleGeometry = new geom.Circle(center, radius);
                var bbox = circleGeometry.bbox();


                var text = new draw.Text("", [0, 0], {
                    font: "18px Verdana,Arial,sans-serif"
                });


                draw.align([text], bbox, "center");
                draw.vAlign([text], bbox, "center");
                var textTarget = new draw.Text("Target", [bbox.origin.x + bbox.size.width - 175, bbox.origin.y + bbox.size.height / 3], {
                    font: "12px Verdana,Arial,sans-serif"
                });
                var textAchieved = new draw.Text("Achieved", [bbox.origin.x + bbox.size.width - 155, bbox.origin.y + bbox.size.height / 2.5], {
                    font: "12px Verdana,Arial,sans-serif"
                });
                var textVal = new draw.Text("100", [bbox.origin.x + bbox.size.width - 160, bbox.origin.y + bbox.size.height / 2], {
                    font: "24px Verdana,Arial,sans-serif",
                    color: "red"
                });
                var textPer = new draw.Text("%", [bbox.origin.x + bbox.size.width - 110, bbox.origin.y + bbox.size.height / 2], {
                    font: "24px Verdana,Arial,sans-serif",
                    color: "red"
                });
                // Draw it on the Chart drawing surface
                e.sender.surface.draw(text);
                e.sender.surface.draw(textTarget);
                e.sender.surface.draw(textAchieved);
                e.sender.surface.draw(textVal);
                e.sender.surface.draw(textPer);
                // Draw it on the Chart drawing surface
                e.sender.surface.draw(text);
            }
        });
    },

    // Zia Mehmood
    //Validation function
    ValidateActivityLogs: function () {
        $('#pnlAuditbleEventsActivityLog #frmAuditbleEventsActivityLog')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {


                  DateTo: {
                      group: '.col-sm-2',
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
                  DateFrom: {
                      group: '.col-sm-2',
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



              }
          })

       .on('success.form.bv', function (e) {

           e.preventDefault();
           $('#pnlAuditbleEventsActivityLog #frmAuditbleEventsActivityLog').bootstrapValidator('revalidateField', 'DateFrom');
           $('#pnlAuditbleEventsActivityLog #frmAuditbleEventsActivityLog').bootstrapValidator('revalidateField', 'DateTo');
           iTrack_Dashboard.Search_ActivityLog();

       });
    },
    // End validation function
    // Start LoadAllFunction
    LoadAllAutocomplete: function () {
        utility.CreateDatePicker("pnlAuditbleEventsActivityLog #frmAuditbleEventsActivityLog #dtpDOB", function () {
        }, false);


        utility.CreateDatePicker("pnlAuditbleEventsActivityLog #frmAuditbleEventsActivityLog #dtpDateTo",
      function (ev) {

          if ($('#pnlAuditbleEventsActivityLog #frmAuditbleEventsActivityLog').data("bootstrapValidator") != null) {
              $('#pnlAuditbleEventsActivityLog #frmAuditbleEventsActivityLog').bootstrapValidator('revalidateField', 'DateTo');
          }

      }, true);
        $('#pnlAuditbleEventsActivityLog #frmAuditbleEventsActivityLog #dtpDateTo').datepicker('setEndDate', new Date());

        utility.CreateDatePicker("pnlAuditbleEventsActivityLog #frmAuditbleEventsActivityLog #dtpDateFrom",
    function (ev) {

        if ($('#pnlAuditbleEventsActivityLog #frmAuditbleEventsActivityLog').data("bootstrapValidator") != null) {
            $('#pnlAuditbleEventsActivityLog #frmAuditbleEventsActivityLog').bootstrapValidator('revalidateField', 'DateFrom');

        }

        //on-change callback method
    }, true);
        $('#pnlAuditbleEventsActivityLog #frmAuditbleEventsActivityLog #dtpDateFrom').datepicker('setEndDate', new Date());
        utility.ValidateFromToDate('frmAuditbleEventsActivityLog', 'dtpDateFrom', 'dtpDateTo', true, null, null, true);

    },
    // End LoadAllFunctiln

    //start  Reset SearchLoadAuditbleEvents_ActivityLogs
    ClearSearch_ActivityLog: function () {

        $(' #pnlAuditbleEventsActivityLog').resetAllControls();
        $(' #pnlAuditbleEventsActivityLog #ddlActive').find('option:selected').removeAttr('selected');
        $(' #pnlAuditbleEventsActivityLog #ddlActive').val('1');
        $('#pnlAuditbleEventsActivityLog #frmAuditbleEventsActivityLog').bootstrapValidator('revalidateField', 'DateFrom');
        $('#pnlAuditbleEventsActivityLog #frmAuditbleEventsActivityLog').bootstrapValidator('revalidateField', 'DateTo');

    },
    //End Reset Search
    // Start-- search Activity Log
    Search_ActivityLog: function (pageNumber, rowsPerPage) {

        iTrack_Dashboard.searchActivityLog_DBCall(pageNumber, rowsPerPage).done(function (response) {
            response = JSON.parse(response);

            if (response.status != false) {


                var ActivityLogUser_JSON = JSON.parse(response.ActivityLogUser_JSON);
                iTrack_Dashboard.GridLoad(ActivityLogUser_JSON, response.ActivityLogUserCount, "NewEntry", " #pnlAuditbleEventsActivityLog #dgvNewEntry");
                var TableControl = iTrack_Dashboard.params.PanelID + " #dgvNewEntry";
                var PagingPanelControlID = iTrack_Dashboard.params.PanelID + " #dgvActivityLogUser_Paging";
                var ClassControlName = "AuditbleEvents_ActivityLog";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.ActivityLogUserCount, pageNumber, 15, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, pageNumber, resultPerPage) {
                    iTrack_Dashboard.Search_ActivityLog(pageNumber, resultPerPage);
                }), 10);
                if (ActivityLogUser_JSON.length > 0) {
                    $('#pnlAuditbleEventsActivityLog #dgvNewEntry tbody tr:first').click();
                }
                else {
                    iTrack_Dashboard.GridLoad(ActivityLogUser_JSON, response.ActivityLogCompCount, "User", " #pnlAuditbleEventsActivityLog #dgvUser");
                    iTrack_Dashboard.GridLoad(ActivityLogUser_JSON, response.ActivityLogCompCount, "Changes", " #pnlAuditbleEventsActivityLog #dgvChanges");
                }


            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });

    },
    searchActivityLog_DBCall: function (pageNumber, rowsPerPage) {
        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 15;
        }
        var objData = new Object();
        //objData["AuditReportId"] = AuditReportId;
        objData["PageNumber"] = pageNumber;
        objData["RowsPerPage"] = rowsPerPage;

        //objData["ProviderId"] = Clinical_ProgressNote.params.CurrentNotesProviderId
        objData["AccountNo"] = $(' #pnlAuditbleEventsActivityLog #txtAccountNo').val() == "" ? null : $(' #pnlAuditbleEventsActivityLog #txtAccountNo').val();
        objData["LastName"] = $(' #pnlAuditbleEventsActivityLog #txtLastName').val() == "" ? null : $(' #pnlAuditbleEventsActivityLog #txtLastName').val();
        objData["FirstName"] = $(' #pnlAuditbleEventsActivityLog #txtFirstName').val() == "" ? null : $(' #pnlAuditbleEventsActivityLog #txtFirstName').val();
        objData["DOB"] = $(' #pnlAuditbleEventsActivityLog #dtpDOB').val() == "" ? null : $(' #pnlAuditbleEventsActivityLog #dtpDOB').val();
        objData["SSN"] = $(' #pnlAuditbleEventsActivityLog #txtSSN').val() == "" ? null : $(' #pnlAuditbleEventsActivityLog #txtSSN').val();
        objData["Status"] = $(' #pnlAuditbleEventsActivityLog #ddlPatientStatus').val();
        objData["DateTo"] = $(' #pnlAuditbleEventsActivityLog #dtpDateTo').val() == "" ? null : $(' #pnlAuditbleEventsActivityLog #dtpDateTo').val();
        objData["DateFrom"] = $(' #pnlAuditbleEventsActivityLog #dtpDateFrom').val() == "" ? null : $(' #pnlAuditbleEventsActivityLog #dtpDateFrom').val();
        objData["EmergencyAccess"] = $(' #pnlAuditbleEventsActivityLog #chkEnergencyAccess').prop('checked') == true ? true : false;
        objData["commandType"] = "auditbleeventsactivitylog";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "AuditbleEventsActivityLog", "AuditbleEventsActivityLog");

    },
    // End -- search Activity Log

    // Start -- Component Load
    ActivityLogsComponent: function (ProfileName, PatientId, DateAndTime, UserId, pageNumber, rowsPerPage) {
        iTrack_Dashboard.searchActivityLogComponents_DBCall(ProfileName, PatientId, DateAndTime, UserId, pageNumber, rowsPerPage).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {


                var ActivityLogUser_JSON = JSON.parse(response.ActivityLogCompLoad_JSON);
                iTrack_Dashboard.GridLoad(ActivityLogUser_JSON, response.ActivityLogCompCount, "User", " #pnlAuditbleEventsActivityLog #dgvUser");
                var TableControl = iTrack_Dashboard.params.PanelID + " #dgvUser";
                var PagingPanelControlID = iTrack_Dashboard.params.PanelID + " #dgvActivityLogComp_Paging";
                var ClassControlName = "AuditbleEvents_ActivityLog";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.ActivityLogCompCount, pageNumber, 15, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, pageNumber, resultPerPage) {
                    iTrack_Dashboard.ActivityLogsComponent($('#pnlAuditbleEventsActivityLog #dgvNewEntry tr.active #rowprofilename').text(), $('#pnlAuditbleEventsActivityLog #dgvNewEntry tr.active #rowpatid').text(), $('#pnlAuditbleEventsActivityLog #dgvNewEntry tr.active #rowdateid').text(), $('#pnlAuditbleEventsActivityLog #dgvNewEntry tr.active #rowuserid').text(), pageNumber, resultPerPage);
                }), 10);
                if (ActivityLogUser_JSON.length > 0) {
                    $('#pnlAuditbleEventsActivityLog #dgvUser tbody tr:first').click();
                }
                else {
                    iTrack_Dashboard.GridLoad(ActivityLogUser_JSON, response.ActivityLogCompCount, "Changes", " #pnlAuditbleEventsActivityLog #dgvChanges");
                }

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });
    },
    searchActivityLogComponents_DBCall: function (ProfileName, PatientId, DateAndTime, UserId, pageNumber, rowsPerPage) {
        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 15;
        }
        var objData = new Object();
        //objData["AuditReportId"] = AuditReportId;
        objData["PageNumber"] = pageNumber;
        objData["RowsPerPage"] = rowsPerPage;
        objData["ProfileName"] = ProfileName;
        objData["PatientId"] = PatientId;
        objData["DateAndTime"] = DateAndTime;
        objData["UserId"] = UserId;
        objData["commandType"] = "auditbleeventsactivitylogcomponents";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "AuditbleEventsActivityLog", "AuditbleEventsActivityLog");
    },
    // End -- Components load
    // Start -- ActivityLog Changes
    ActivityLogsChanges: function (ColumnKeyId, ProfileName, DateAndTime, pageNumber, rowsPerPage) {
        iTrack_Dashboard.searchActivityLogChanges_DBCall(ColumnKeyId, ProfileName, DateAndTime, pageNumber, rowsPerPage).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {


                var ActivityLogUser_JSON = JSON.parse(response.ActivityLogChangesLoad_JSON);
                iTrack_Dashboard.GridLoad(ActivityLogUser_JSON, response.ActivityLogChangesCount, "Changes", " #pnlAuditbleEventsActivityLog #dgvChanges");
                var TableControl = iTrack_Dashboard.params.PanelID + " #dgvChanges";
                var PagingPanelControlID = iTrack_Dashboard.params.PanelID + " #dgvActivityLogChanges_Paging";
                var ClassControlName = "AuditbleEvents_ActivityLog";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.ActivityLogChangesCount, pageNumber, 15, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, pageNumber, resultPerPage) {
                    iTrack_Dashboard.ActivityLogsChanges($('#pnlAuditbleEventsActivityLog #dgvUser tr.active #colkey').text(), $('#pnlAuditbleEventsActivityLog #dgvUser tr.active #profName').text(), $('#pnlAuditbleEventsActivityLog #dgvUser tr.active #datetime').text(), pageNumber, resultPerPage);
                }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });
    },
    searchActivityLogChanges_DBCall: function (ColumnKeyId, ProfileName, DateAndTime, pageNumber, rowsPerPage) {
        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 15;
        }
        var objData = new Object();
        //objData["AuditReportId"] = AuditReportId;
        objData["PageNumber"] = pageNumber;
        objData["RowsPerPage"] = rowsPerPage;
        objData["ColumnKeyId"] = ColumnKeyId;
        objData["DateAndTime"] = DateAndTime;
        objData["ProfileName"] = ProfileName;
        objData["commandType"] = "auditbleeventsactivitylogChanges";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "AuditbleEventsActivityLog", "AuditbleEventsActivityLog");

    },
    // ENd -- ActivityLog Changes
    GridLoad: function (ActivityLogUser_JSON, count, gridType, gridId) {
        $(gridId + " tbody").empty();
        $(gridId).dataTable().fnDestroy();
        $(gridId + " tbody").find("tr").remove();

        var emptyTableMsg = "No New Entry Found";

        if (count != null && count > 0) {
            var firstRowId = "";

            $.each(ActivityLogUser_JSON, function (i, item) {
                var $row = $('<tr/>');
                var _rowId = "";
                var emptyTableMsg = "";
                if (gridType == "NewEntry" || gridType == "NewEntryUser") {
                    _rowId = "dgvNewEntry_row" + i;
                    if (item.ColumnKeyId == null || item.ColumnKeyId == "") {
                        temp_colkey = null;
                    }
                    else {
                        temp_colkey = item.ColumnKeyId;
                    }
                    $row.append('<td id= "rowprofilename" style="display:none;">' + item.ModuleName + '</td><td id= "rowpatid" style="display:none;">' + item.PatientId + '</td><td id= "rowcolid" style="display:none;">' + temp_colkey + '</td><td id= "rowdateid" style="display:none;">' + item.DateAndTime + '</td><td id= "rowuserid" style="display:none;">' + item.UserId + '</td><td class="ellipses size-max90" title="' + item.Patient + '">' + item.Patient + '</td><td class="ellipses size-max90" title="' + item.AccountNo + '">' + item.AccountNo + '</td><td class="ellipses size-max90" title="' + item.User + '">' + item.User + '</td>><td class="ellipses size-max90" title="' + item.ModuleName + '">' + item.ModuleName + '</td><td>' + item.DateAndTime + '</td>');
                    //$row.append('<td style="display:none;">' + item.AuditbleEvents_ActivityLogId + '</td><td>' + item.ProfileName + '</td><td>' + item.User + '</td><td>' + item.EntryDate + '</td><td>' + item.Field + '</td><td>' + item.OriginalValue + '</td><td>' + item.CurrentValue + '</td>');


                    $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); iTrack_Dashboard.ActivityLogsComponent('" + item.ModuleName + "'," + item.PatientId + ",'" + item.DateAndTime + "'," + item.UserId + ");");


                }
                else if (gridType == "User") {
                    _rowId = "dgvUser_row" + i;
                    var temp_colkey
                    if (item.ColumnKeyId == null || item.ColumnKeyId == "") {
                        temp_colkey = null;
                    }
                    else {
                        temp_colkey = item.ColumnKeyId;
                    }
                    $row.append('<td id="colkey" style="display:none;">' + temp_colkey + '</td><td id="profName" style="display:none;">' + item.ProfileName + '</td><td id="datetime"style="display:none;">' + item.CreatedDateTime + '</td><td >' + item.ProfileName + '</td><td style="display:none;">' + item.SubProfileName + '</td><td>' + item.DBAuditAction + '</td><td>' + item.DateAndTime + '</td>');
                    $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); iTrack_Dashboard.ActivityLogsChanges(" + temp_colkey + ",'" + item.ProfileName + "','" + item.CreatedDateTime + "');");



                } else if (gridType == "Changes") {
                    //

                    _rowId = "dgvDeletedCharges_row" + i;
                    var OriginalValue = item.PreviousValue;
                    var CurrentValue = item.CurrentValue;
                    if (item.Field == 'Note Text') {
                        CurrentValue = CurrentValue.replace(/&quot;/g, '"');
                        CurrentValue = CurrentValue.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
                        CurrentValue = CurrentValue.replace(/&nbsp;/g, '');
                        OriginalValue = OriginalValue.replace(/&quot;/g, '"');
                        OriginalValue = OriginalValue.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
                        OriginalValue = OriginalValue.replace(/&nbsp;/g, '');
                    }
                    if (item.Field == 'Patient Image') {

                        CurrentValue = CurrentValue != "" ? '<img id="imgCurrent" class="img-responsive img-center" src="' + CurrentValue + '" alt="Current Patient Image"/>' : "";
                        OriginalValue = OriginalValue != "" ? '<img id="imgCurrent" class="img-responsive img-center" src="' + OriginalValue + '" alt="Original Patient Image"/>' : "";

                    }
                    if (item.Field == 'SSN') {
                        if (globalAppdata.IsFullSSN.toLowerCase() === 'true') {
                        }
                        else {

                            if (CurrentValue != "") {
                                var last4digit = CurrentValue.slice(-4);
                                CurrentValue = "XXX-XX-" + last4digit;
                            }
                            if (OriginalValue != "") {
                                var last4digits = OriginalValue.slice(-4);
                                OriginalValue = "XXX-XX-" + last4digits;
                            }
                        }
                    }

                    if (item.Field.indexOf("Date") >= 0 || item.Field.indexOf("DOB") >= 0 || item.Field == 'Started On' || item.Field == 'Stopped On') {
                        CurrentValue = CurrentValue != "" ? utility.RemoveTimeFromDate(null, CurrentValue) : "";
                        OriginalValue = OriginalValue != "" ? utility.RemoveTimeFromDate(null, OriginalValue) : "";

                    }
                    if ((item.Field == 'DOSTo')
                        || (item.Field == 'DOSFrom')) {
                        if (CurrentValue != "") {
                            CurrentValue = CurrentValue.replace(' 12:00AM', '');
                            CurrentValue = new Date(CurrentValue);
                            CurrentValue = $.datepicker.formatDate('mm/dd/yy', CurrentValue);
                        }
                        if (OriginalValue != "") {
                            OriginalValue = OriginalValue.replace(' 12:00AM', '');
                            OriginalValue = new Date(OriginalValue);
                            OriginalValue = $.datepicker.formatDate('mm/dd/yy', OriginalValue);

                        }

                    }
                    // $row.append('<td style="display:none;">' + item.ActivityLogId + '</td><td>' + item.Field + '</td><td>' + OriginalValue + '</td><td>' + CurrentValue + '</td>');
                    // disabled the anchor click, because it's only for user to view
                    if (item.Field == 'Note Text') { $row.find('a').addClass('disableAll'); }
                    $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "'));");


                    $row.append('<td style="display:none;">' + item.AuditbleEvents_ActivityLogId + '</td><td>' + item.Field + '</td><td>' + OriginalValue + '</td><td>' + CurrentValue + '</td>');


                }

                if (gridType == "User") {
                    if (firstRowId == "") {
                        firstRowId = _rowId;
                    }
                    $row.attr("id", _rowId);
                }
                else {
                    if (firstRowId == "") {
                        firstRowId = _rowId;
                    }
                    $row.attr("id", _rowId);
                }

                if (iTrack_Dashboard.params.ParentCtrl == "userDetail" && $(gridId).attr("id") == "dgvUser")
                { $(gridId + " thead").find("th").eq(1).css("display", "none"); $(gridId + " tbody").last().append($row); }
                else
                    $(gridId + " tbody").last().append($row);
            });



        }
        else {


            $(gridId).DataTable({
                "language": {
                    "emptyTable": emptyTableMsg
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
            if ($(gridId + "_wrapper").find(".datatables-footer").length == 1 && gridType == "User") {
                $("#dgvActivityLogComp_Paging").children().remove();
            }
            if ($(gridId + "_wrapper").find(".datatables-footer").length == 1 && gridType == "Changes") {
                $("#dgvActivityLogChanges_Paging").children().remove();
            }
        }


        if ($.fn.dataTable.isDataTable(gridId))
            ;
        else {
            var orderIndex;
            if (gridType == "NewEntry" || gridType == "NewEntryUser") {
                orderIndex = 9;
            } else if (gridType == "User") {
                orderIndex = 6;
            } else {

                orderIndex = 1;
            }
            $(gridId).DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "bFilter": false, "order": [[orderIndex, "desc"]] }); // to remove records per page dropdown
        }

        if ($(gridId + "_wrapper").find(".datatables-footer").length > 1) {
            $(gridId + "_wrapper").find(".datatables-footer").last().remove();
            $(gridId + "_wrapper").find(".Of-a").first().removeClass("Of-a");
        }

    },







}
