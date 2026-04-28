iTrack_PromotingInteroperabilityGraph = {
    params: [],

    Load: function (params) {
        iTrack_PromotingInteroperabilityGraph.params = params;
        iTrack_PromotingInteroperabilityGraph.params.GraphObj = params.GraphObj;
        $('#frmiTrackPromotingInteroperabilityGraph #kpipanel').append(' <div class="DashBoardKPI_Drop" id="1">'
                       + '<div class="panel panel-featured">'
                       + '<header id="KPIHeader_1" class="panel-heading">'
                       + '<div class="panel-actions"> </div>'
                       + '<h2 class="panel-title pull-left">Composite Score</h2>'
                       + '</header>'
                       + '<div class=" panel-body"><div class="text-vertical-kpi">Performance Rate %</div><div id="chart_1" style="min-height: 300px; height:0px !important; padding:5px 10px 35px 10px" class=" panel-body"></div>'
                       + '</div>'
                       + '</div>'
                       + '<div id="legend" class="font-xs bold text-center">Promoting Interoperability(PI) Measures</div><div class="pr-default text-center">'
           + ' <label class="tab_space"><span style="background-color:#1569C7" class="chip-notifi"></span> Base Measure</label>'
           + ' <label class="tab_space"><span style="background-color:#5bc0de" class="chip-notifi"></span> Performance Measure</label>'
           + ' <label class="tab_space"><span style="background-color:#47a447" class="bg-success chip-notifi"></span> Bonus Measure</label>'
          + '</div>');

        $('#frmiTrackPromotingInteroperabilityGraph #divGraphInfo').html(iTrack_PromotingInteroperabilityGraph.params.label);
        var myJSON = [];

        $.each(iTrack_PromotingInteroperabilityGraph.params.GraphObj, function (i, item) {

            var jsonArray = {
                measure: item.name, b: (item.PerformanceRate ? item.PerformanceRate.toString() : "0.00"), c: item.MeasureName, MeasureType
            : item.MeasureType
            };
            var temp = jsonArray;
            myJSON.push(temp);

        });
        var baseMesures = 'PI_PPHI_1,PI_EP_1,PI_PEA_1,PI_HIE_2,PI_HIE_1';
        var perMesures = 'PI_HIE_3,PI_PEA_2,PI_CCTPE_2,PI_CCTPE_1,PI_CCTPE_3';
        var weeklyPatientVisitsChart = Morris.Bar({
            xLabelMargin: 30,
            padding: 30,
            resize: true,
            element: 'chart_1',
            data: myJSON,
            xkey: 'measure',
            ykeys: ['b'],
            labels: ['Performance Rate', 'MeasureType'],
            colors: ['#1569C7', 'red', 'pink'],
            xLabelAngle: '70',
            barColors: function (row, series, type) {

                if (baseMesures.indexOf(row.label) > -1) return "#1569C7";
                else if (perMesures.indexOf(row.label) > -1) return "#5bc0de";
                else return "#47a447";
            },
            hoverCallback: function (index, options, content) {
                var data = options.data[index];
                var color = '#1569C7';
                if (baseMesures.indexOf(data.c) > -1) color = "#1569C7";
                else if (perMesures.indexOf(data.c) > -1) color = "#5bc0de";
                else color = "#47a447";
                return '</div><div class="morris-hover-point" style="color: ' + color + '">'
                 + data.c
                 + '</div><div class="morris-hover-point" style="color: #0000FF">'
                 + 'Performance Rate:'
                 + data.b + '%'
                 + '</div>';
            },
        });
        $('#chart_1').resize(function () {
            weeklyPatientVisitsChart.redraw();
        });
    },

    UnLoad: function () {
        UnloadActionPan(iTrack_PromotingInteroperabilityGraph.params.ParentCtrl, 'iTrack_PromotingInteroperabilityGraph');

    },
}