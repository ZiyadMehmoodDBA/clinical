iTrack_QualityGraph = {
    params: [],

    Load: function (params) {
        iTrack_QualityGraph.params = params;
        $('#frmiTrackQualityGraph #spnProvider').text(iTrack_QualityGraph.params.ProviderText);
        $('#frmiTrackQualityGraph #spnNPI').text(iTrack_QualityGraph.params.ProviderNPI);
        $('#frmiTrackQualityGraph #spnYear').text(iTrack_QualityGraph.params.PerformanceYear);
        iTrack_QualityGraph.params.GraphObj = params.GraphObj;
        $('#frmiTrackQualityGraph #kpipanel').append(' <div class="DashBoardKPI_Drop" id="1">'
                       + '<div class="panel panel-featured">'
                       + '<header id="KPIHeader_1" class="panel-heading">'
                       + '<div class="panel-actions"> </div>'
                       + '<h2 class="panel-title pull-left">Composite Score</h2>'
                       + '</header>'
                       + '<div id="chart_1" style="min-height: 300px; height:0px;" class=" panel-body"></div>'
                       + '</div>'
                       + '</div>');

        var myJSON = [];
        var data = {

            "Measures": [
            { "name": "Measure 1", "range": "70 - 80", "PerformanceRate": "10" },
            { "name": "Measure 2", "range": "80 - 98", "PerformanceRate": "50" },
            { "name": "Measure 3", "range": "50 - 55", "PerformanceRate": "60" },
            { "name": "Measure 4", "range": "60 - 70", "PerformanceRate": "70" },
            { "name": "Measure 5", "range": "90 - 95", "PerformanceRate": "80" },
            { "name": "Measure 6", "range": "98 - 100", "PerformanceRate": "100" }
            
            ],
        };

        $.each(iTrack_QualityGraph.params.GraphObj, function (i, item) {

            var jsonArray = { MeasureNumber: item.MeasureNumber, measure: item.name, a: item.range, b: item.PerformanceRate };
            var temp = jsonArray;
            myJSON.push(temp);

        });

        var qualityChart = Morris.Bar({
            xLabelMargin: 40,
            padding: 40,
            resize: true,
            element: 'chart_1',
            data: myJSON,
            xkey: 'MeasureNumber',
            ykeys: ['b'],
            xLabelAngle: '70',
            labels: ['Performance Rate'],

            barColors: function (row, series, type) {

                if (series.label == "Range") return "#5E7D7E";
                if (series.label == "Performance Rate") return "#1569C7" ;

            },
            hoverCallback: function (index, options, content) {
                var data = options.data[index];
                return '<div class="morris-hover-row-label"></div><div class="morris-hover-point" style="color: #000000">'
                 + 'Range:'
                 + data.a
                 + '</div><div class="morris-hover-point" style="color: #0000FF">'
                 + 'Performance Rate:'
                 + data.b + '%'
                 + '</div><div class="morris-hover-point" style="color: #FF0400">'
                 + 'Measure Name: '
                 + data.measure
                 + '</div>';
            },
        });
        $('#chart_1').resize(function () {
           // if ($("#ctrlPanDashBoard").css("display") != "none") {
            qualityChart.redraw();
           // }
        });
    },

    UnLoad: function () {
        UnloadActionPan(iTrack_QualityGraph.params.ParentCtrl, 'iTrack_QualityGraph');

    },
}