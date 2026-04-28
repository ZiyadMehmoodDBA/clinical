ClinicalLabResultTrendsGraphs = {
    params: [],
    bIsFirstLoad: true,
    ThreeDChart: null,
    Load: function (params) {
        $('#pnlClinicalLabResultTrendsGraphs hr').css({ "margin-top": "10px" , "margin-bottom":"10px" });
        ClinicalLabResultTrendsGraphs.params = params;
        $('#pnlClinicalLabResultTrendsGraphs #PatientDetails').append($('#banner_PatientName').parent().text() + ' ' + $('#banner_PatientDOB').text())

        ClinicalLabResultTrendsGraphs.Set_GraphsCurrentTrend(ClinicalLabResultTrendsGraphs.params.LOINC, ClinicalLabResultTrendsGraphs.params.LOINCDescription);

  
        $("#pnlClinicalLabResultTrendsGraphs input[name=radgroupGraph]").on("change", function () {
            if ($('input[name=radgroupGraph]:checked').val() == "3D") {
                $('#TwoD').hide();
                $('#ThreeD').show();
                ClinicalLabResultTrendsGraphs.buildRadioBtnGraph("3D");
            }
            else {
                $('#ThreeD').hide();
                $('#dgvLabGraphs').html("");
                $('#TwoD').show();
                ClinicalLabResultTrendsGraphs.buildRadioBtnGraph("2D");
            }
        });
        
    },
    buildRadioBtnGraph: function (type) {
        if ($('#pnlClinicalLabResultTrendsGraphs #ddlFilterObservation option:selected').length == 0) {
            var LOINC = ClinicalLabResultTrendsGraphs.params.LOINC;
            var LOINCDescription = ClinicalLabResultTrendsGraphs.params.LOINCDescription;
        }
        else {
            var LOINC = $('#pnlClinicalLabResultTrendsGraphs #ddlFilterObservation option:selected').attr('LOINC');
            var LOINCDescription = $('#pnlClinicalLabResultTrendsGraphs #ddlFilterObservation option:selected').attr('LOINCDescription');
        }

        if (type == "3D") {
            ClinicalLabResultTrendsGraphs.build3DGraph(LOINC, LOINCDescription);
        }
        else {
            ClinicalLabResultTrendsGraphs.Set_GraphsCurrentTrend(LOINC, LOINCDescription);
        }
    },
    Set_GraphsCurrentTrend: function(LOINC, LOINCDescription) {
        $.each(ClinicalLabResultTrends.Trends, function (i, item) {

            if (item.TestDescription == ClinicalLabResultTrendsGraphs.params.MainTestDescription) {
                if ($('#pnlClinicalLabResultTrendsGraphs #ddlFilterObservation option').length == 0) {
                    ClinicalLabResultTrendsGraphs.BindSelector(item);
                }
                $.each(item.TestTrends, function (i, innerItem) {
                    if (innerItem.LOINC == LOINC) {

                        setTimeout(function () {
                            kendo.fx($("#dgvLabGraphs").kendoChart({
                                dataSource: {
                                    data: innerItem.ResultsValues
                                },
                                title: {
                                    text: LOINCDescription
                                },
                                chartArea: {
                                    width: 900,
                                },
                                legend: {
                                    position: "bottom"
                                },
                                seriesDefaults: {
                                    type: "line"
                                },
                                series: [{
                                    name: "Result values",
                                    field: "Value",
                                    categoryField: "Date",
                                    color: "#42C2F4",
                                    notes: {
                                        label: {
                                            position: "outside"
                                        },
                                        position: "bottom"
                                    },
                                    highlight: {
                                        visual: function (e) {
                                            var origin = e.rect.origin;
                                            var bottomRight = e.rect.bottomRight();
                                            var topRight = e.rect.topRight();
                                            var topLeft = e.rect.topLeft();
                                            var path = new kendo.drawing.Path({
                                                fill: {
                                                    color: "white",
                                                    opacity: 1,
                                                },
                                                stroke: {
                                                    color: "red",
                                                    opacity: 0.7,
                                                    width: 2,
                                                }
                                            })
                                            .moveTo(origin.x, bottomRight.y)
                                            .lineTo(bottomRight.x, bottomRight.y)
                                            .lineTo(topRight.x, topRight.y)
                                            .lineTo(topLeft.x, topLeft.y)
                                            .close();
                                            return path;
                                        }
                                    }
                                }],
                                valueAxis: {
                                    line: {
                                        visible: false
                                    }
                                },
                                categoryAxis: {
                                    majorGridLines: {
                                        visible: false
                                    }
                                },
                                tooltip: {
                                    visible: true,
                                    template: "#=ClinicalLabResultTrendsGraphs.KendoToolTipGraph(dataItem)#"
                                }
                            })).fade("in").play();
                        }, 100);
                        return false;
                    }
                });
            }

        });

    },
    build3DGraph: function (LOINC, LOINCDescription) {
        $.each(ClinicalLabResultTrends.Trends, function (i, item) {

            if (item.TestDescription == ClinicalLabResultTrendsGraphs.params.MainTestDescription) {
                if ($('#pnlClinicalLabResultTrendsGraphs #ddlFilterObservation option').length == 0) {
                    ClinicalLabResultTrendsGraphs.BindSelector(item);
                }
                $.each(item.TestTrends, function (i, innerItem) {
                    if (innerItem.LOINC == LOINC) {

                        var Dates = [];
                        var ResultValues = [];
                        var toolTipColor = [];
                    //    var toolTipColor = jQuery.extend(true, {}, innerItem.ResultsValues);
                        toolTipColor = innerItem.ResultsValues;
                        $.each(innerItem.ResultsValues, function (i, x) {
                            Dates.push(x.Date);
                            ResultValues.push(parseFloat(x.Value));
                            toolTipColor[i]["y"]  = parseFloat(x.Value);
                            //toolTipColor[i]["color"] = "#42C2F4";
                        });

                        ClinicalLabResultTrendsGraphs.ThreeDChart = new Highcharts.Chart({
                            chart: {
                                renderTo: 'Graph3d',
                                type: 'column',
                                options3d: {
                                    enabled: true,
                                    alpha: 18,
                                    beta: 6,
                                    depth: 64,
                                    viewDistance: 25
                                }
                            },
                            title: {
                                text: LOINCDescription
                            },
                            plotOptions: {
                                column: {
                                    depth: 25
                                }
                            },
                            xAxis: {
                                name: "Result Dates",
                                categories: Dates
                            },
                            tooltip: {
                                formatter: function () {
                                    var data = this.series;
                                    var eval = ClinicalLabResultTrends.EvaluateResultRange(this.point.Value, this.point.ReferenceRange, this.point.ReferenceRangeInterpration, this.point.ReferenceRangeDescription, this.point.Unit, this.point.Flag);
                                    return '<div id="GraphPointDetails">  <b>Order#:</b> ' + this.point.OrderNumber + ' <br> <b>Result: </b> <span style="color:' + (eval["Color"] == "#DC143C" ? "red" : "") + ';">' + this.point.Value + ' </span> <br> <b>Units:</b> ' + this.point.Unit + ' <br> <b>Range:</b> ' + this.point.ReferenceRange + ' <br> <b>Flag: </b><span style="color:' + (eval["FlagColor"] == "#DC143C" ? "red" : "") + ';">' + this.point.Flag + ' </span> <br> <b>Date: </b>' + this.point.Date + '</div> ';
                                }
                            },
                            series: [{
                                name: "Result Values",
                                data: toolTipColor
                            }]
                        });
                    }
                });
            }
        });
 

        function showValues() {
            $('#alpha-value').html(ClinicalLabResultTrendsGraphs.ThreeDChart.options.chart.options3d.alpha);
            $('#beta-value').html(ClinicalLabResultTrendsGraphs.ThreeDChart.options.chart.options3d.beta);
            $('#depth-value').html(ClinicalLabResultTrendsGraphs.ThreeDChart.options.chart.options3d.depth);
        }

        // Activate the sliders
        $('#sliders input').on('input change', function () {
            ClinicalLabResultTrendsGraphs.ThreeDChart.options.chart.options3d[this.id] = parseFloat(this.value);
            showValues();
            ClinicalLabResultTrendsGraphs.ThreeDChart.redraw(false);
        });

        showValues();
    },
    KendoToolTipGraph: function (data) {
        var eval = ClinicalLabResultTrends.EvaluateResultRange(data.Value, data.ReferenceRange, data.ReferenceRangeInterpration, data.ReferenceRangeDescription, data.Unit, data.Flag);
        return '<div id="GraphPointDetails" class="col-sm-2 col-md-2">  <b>Order#:</b> ' + data.OrderNumber + ' <br> <b>Result: </b> <span style="color:' + (eval["Color"] == "#DC143C" ? "red" : "") + ';">' + data.Value + ' </span> <br> <b>Units:</b> ' + data.Unit + ' <br> <b>Range:</b> ' + data.ReferenceRange + ' <br> <b>Flag: </b> <span style="color:' + (eval["FlagColor"] == "#DC143C" ? "red" : "") + ';" > ' + data.Flag + ' </span> <br> <b>Date: </b>' + data.Date + '</div> ';
    },
    onChangeFilter: function () {
        if ($("#pnlClinicalLabResultTrendsGraphs input[name=radgroupGraph]:checked").val() == "2D") {
            ClinicalLabResultTrendsGraphs.Set_GraphsCurrentTrend($('#pnlClinicalLabResultTrendsGraphs #ddlFilterObservation option:selected').attr('LOINC'), $('#pnlClinicalLabResultTrendsGraphs #ddlFilterObservation option:selected').attr('LOINCDescription'));
        }
        else if ($("#pnlClinicalLabResultTrendsGraphs input[name=radgroupGraph]:checked").val() == "3D") {
            ClinicalLabResultTrendsGraphs.build3DGraph($('#pnlClinicalLabResultTrendsGraphs #ddlFilterObservation option:selected').attr('LOINC'), $('#pnlClinicalLabResultTrendsGraphs #ddlFilterObservation option:selected').attr('LOINCDescription'));
        }
    },
    BindSelector: function (response) {
        
        $.each(response.TestTrends, function (i, item) {
            var checked = "";
            if (item.LOINC == ClinicalLabResultTrendsGraphs.params.LOINC) {
                checked = "selected";
            }
            $('#pnlClinicalLabResultTrendsGraphs #ddlFilterObservation').append('<option value="' + item.LOINC + '" LOINC="' + item.LOINC + '" LOINCDescription="' + item.LOINCDescription + '" ' + checked + '> ' + item.LOINCDescription + '</option>');
        });
        
    },
   
    get_Graph: function(LOINC, LOINCDescription) {

    }

    ,
    UnLoad: function () {
        if (ClinicalLabResultTrendsGraphs.params != null && ClinicalLabResultTrendsGraphs.params.ParentCtrl != null) {
            if (ClinicalLabResultTrendsGraphs.params.ParentCtrl == 'clinicalTabLabOrder') {
                UnloadActionPan(ClinicalLabResultTrendsGraphs.params["ParentCtrl"], "ClinicalLabResultTrendsGraphs");
            } else {
                ClinicalLabResultTrendsGraphs.params.PanelID = ClinicalLabResultTrendsGraphs.params.PanelID.replace(" #ClinicalLabResultTrendsGraphs", "");
                UnloadActionPan(ClinicalLabResultTrendsGraphs.params.ParentCtrl, 'ClinicalLabResultTrendsGraphs', null, ClinicalLabResultTrendsGraphs.params.PrPanelID);
            }
        }
        else
            UnloadActionPan(null, 'ClinicalLabResultTrendsGraphs');
    }
}