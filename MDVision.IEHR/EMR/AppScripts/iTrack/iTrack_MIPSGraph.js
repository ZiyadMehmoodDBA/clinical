iTrack_MIPSGraph = {
    params: [],

    Load: function (params) {
        iTrack_MIPSGraph.params = params;
        $('#frmiTrackMipsGraph #spnProvider').text(iTrack_MIPSGraph.params.ProviderText);
        $('#frmiTrackMipsGraph #spnYear').text(iTrack_MIPSGraph.params.PerformanceYear);
        $('#frmiTrackMipsGraph #isIndividual').text(iTrack_MIPSGraph.params.IsIndividual);

        $('#frmiTrackMipsGraph #kpipanel').append(' <div class="DashBoardKPI_Drop" id="1">'
                       + '<div class="panel panel-featured">'
                       + '<header id="KPIHeader_1" class="panel-heading">'
                       + '<div class="panel-actions"> </div>'
                       + '<h2 class="panel-title">Composite Score</h2>'
                       + '</header>'
                       + '<div class=" panel-body"><div class="text-vertical-kpi">Performance Rate %</div><div id="chart_1" style="min-height: 300px; height:auto;"></div>'
                       +'<div id="chartTitle" class="bold" style="text-align:center;font-size:large"> MIPS Categories</div>'
                       + '</div></div>'
                       + '</div>');

        var myJSON = [];
        var qualityAchievedPoints = iTrack_MIPSGraph.params.qualityAchievedPoints;
        var PIAchievedPoints = iTrack_MIPSGraph.params.PIAchievedPoints;
        var ImprovementActivitiesAchievedPoints = 15;//iTrack_MIPSGraph.params.ImprovementActivitiesAchievedPoints;
        var CostAchievedPoints = 10;//iTrack_MIPSGraph.params.CostAchievedPoints;
        var TotalAchievedPoints = qualityAchievedPoints + PIAchievedPoints + ImprovementActivitiesAchievedPoints + CostAchievedPoints;

        var qualityWeightagePoints = 50 - qualityAchievedPoints;
        var PIWeightagePoints = 25 - PIAchievedPoints;
        var ImprovementActivitiesWeightagePoints = 15 - ImprovementActivitiesAchievedPoints;
        var CostWeightagePoints = 10 - CostAchievedPoints;
        var TotalWeightagePoints = 100 - TotalAchievedPoints;

        var qualityWeightageLabel = 50 ;
        var PIWeightageLabel = 25;
        var ImprovementActivitiesWeightageLabel = 15;
        var CostWeightageLabel = 10;
        var TotalWeightageLabel = 100;

        var data = {
           
            "Measures": [
            { "name": "Quality", "weightage": qualityWeightagePoints.toString(), "achieved": qualityAchievedPoints.toString(), "weight": qualityWeightageLabel },
            { "name": "PI", "weightage": PIWeightagePoints.toString(), "achieved": PIAchievedPoints.toString(), "weight": PIWeightageLabel },
            { "name": "IA", "weightage": ImprovementActivitiesWeightagePoints.toString(), "achieved": ImprovementActivitiesAchievedPoints.toString(), "weight": ImprovementActivitiesWeightageLabel },
            { "name": "Cost", "weightage": CostWeightagePoints.toString(), "achieved": CostAchievedPoints.toString(), "weight": CostWeightageLabel },
            { "name": "Total", "weightage": TotalWeightagePoints.toString(), "achieved": TotalAchievedPoints.toString(), "weight": TotalWeightageLabel }
            ],
        };

        $.each(data.Measures, function (i, item) {

            var jsonArray = { measure: item.name, a: item.weightage, b: item.achieved, c: item.weight };
            var temp = jsonArray;
            myJSON.push(temp);

        });

        var mipsChart = Morris.Bar({
            //xLabelMargin: 40,
            xLabelAngle: 0,
            ymax: 100,
            resize: true,
            element: 'chart_1',
            data: myJSON,
            hoverCallback: function (index, options, content) {
                return (content);
            },
            padding: 40,
            xkey: 'measure',
            ykeys: ['b', 'a'],
            stacked: true,
            labels: ['Points Achieved', 'Weight age'],
            barColors: function (row, series, type) {

                if (series.label == "Points Achieved") return "#AD1D28";
                if (series.label == "Weight age") return "#DEBB27";

            },
            hoverCallback: function (index, options, content) {
                
                var data = options.data[index];
                data.measure = data.measure == "PI" ? "Promoting Interoperability" : data.measure;
                data.measure = data.measure == "IA" ? "Improvement Activities" : data.measure;

                return '<div class="morris-hover-point" style="color: #DEBB27">'
                 + ''+ data.measure +' Weight age:'
                 + data.c + " Pts"
                 + '</div><div class="morris-hover-row-label"></div><div class="morris-hover-point" style="color: #AD1D28">'
                 + 'Points Achieved:'
                 + data.b + " Pts"
                 + '</div>';
            },
        });
        $('#chart_1').resize(function () {
            //if ($("#ctrlPanDashBoard").css("display") != "none") {
            mipsChart.redraw();
            //}
        });
    },

    UnLoad: function () {
        UnloadActionPan(iTrack_MIPSGraph.params.ParentCtrl, 'iTrack_MIPSGraph');

    },
}