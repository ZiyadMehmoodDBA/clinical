var inactiveProvider = [];
iTrack_MUReport = {
    FromDate: '',
    ToDate: '',
    bIsFirstLoad: true,
    params: [],
    MUdata_Patient: [],
    totalResultResponse: {},
    ReportId: "",
    SelectedReportID: "",
    ReportName: "",
    arrReasoning: [],
    IsHeaderFareez: true,
    findHeader: null,
    getHeaderRow: null,
    headingRow: null,
    RowCount: 0,
    Locations: [],

    Load: function (params) {

        iTrack_MUReport.params = params;
        if (params.FromDashBoard == true) {
            iTrack_MUReport.bIsFirstLoad = true;
        }
        if (iTrack_MUReport.bIsFirstLoad) {
            iTrack_MUReport.bIsFirstLoad = false;
            var self = "";
            if (iTrack_MUReport.params["PanelID"] != "pnliTrack_MUReport") {
                self = $('#' + iTrack_MUReport.params["PanelID"] + " #pnliTrack_MUReport")
            }
            else
                self = $('#' + iTrack_MUReport.params["PanelID"]);
            utility.CreateDatePicker("pnliTrack_MUReport #dtpFromDate, #dtpToDate", function () { }, false);
            utility.CreateDatePicker("pnliTrack_MUReport #dtpFromDate, #dtpToDate", function () { }, false);
            utility.ValidateFromToDate('frmiTrack_MUReport', 'dtpFromDate', 'dtpToDate', true);

            if (params.FromDashBoard == true) {
                $("#pnliTrack_MUReport #frmiTrack_MUReport #txtProvider").val('');
                params.FromDashBoard = false;
                iTrack_MUReport.BindProvider(true);
                $Ctrl_gr = $("#pnliTrack_MUReport #txtProvider");
                $hfCtrl_gr = $("#pnliTrack_MUReport #hfProviderId");
                var providerName = globalAppdata["DefaultProviderName"];
                var providerId = globalAppdata["DefaultProviderId"];
                utility.SetKendoAutoCompleteSourceforValidate($Ctrl_gr, providerName, $hfCtrl_gr, providerId);
                $('#pnliTrack_MUReport #dtpFromDate').val('01/01/2018');
                $('#pnliTrack_MUReport #dtpToDate').val('12/31/2018');

                iTrack_MUReport.generateMuStageReport(true);

            } else {
                iTrack_MUReport.BindProvider(false);
            }
        }
    },
    calculateFromDateToDate: function () {
        var from = '';
        var to = '';
        var year = $('#pnliTrack_MUReport #ddlYear').val();
        if ($('#pnliTrack_MUReport #ddlQuarter').val() == 1) {
            from = '01/01/' + year;
            to = '03/31/' + year;
        } else if ($('#pnliTrack_MUReport #ddlQuarter').val() == 2) {
            from = '04/01/' + year;
            to = '06/30/' + year;
        } else if ($('#pnliTrack_MUReport #ddlQuarter').val() == 3) {
            from = '07/01/' + year;
            to = '09/30/' + year;
        } else if ($('#pnliTrack_MUReport #ddlQuarter').val() == 4) {
            from = '10/01/' + year;
            to = '12/31/' + year;
        }

        iTrack_MUReport.FromDate = from;
        iTrack_MUReport.ToDate = to;
    },
    generateMuStageReport_DBCall: function (FromDashBoard) {

        iTrack_MUReport.calculateFromDateToDate();
        var objData = {};

        if (FromDashBoard == true) {
            objData["Provider"] = globalAppdata["DefaultProviderId"];
            objData["FromDate"] = '01/01/2018';
            objData["ToDate"] = '12/31/2018';
        } else {
            objData["Provider"] = $('#pnliTrack_MUReport #hfProviderId').val();
            objData["FromDate"] = $('#pnliTrack_MUReport #QuarterlyReport').is(':checked') ? iTrack_MUReport.FromDate : $('#pnliTrack_MUReport #dtpFromDate').val();
            objData["ToDate"] = $('#pnliTrack_MUReport #QuarterlyReport').is(':checked') ? iTrack_MUReport.ToDate : $('#pnliTrack_MUReport #dtpToDate').val();
        }

        objData["ReportName"] = "MU Stage 3 Report";
        objData["commandType"] = "SEARCH_MUStageReport1";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Report", "MUReport");
    },
  
    BindProvider: function (FromDashBoard) {
        var Ctrl = $("#pnliTrack_MUReport #frmiTrack_MUReport #txtProvider");
        var searchedName = Ctrl.val();
        if (FromDashBoard) {
            Ctrl.val(globalAppdata["DefaultProviderName"]);
        }
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        var hfCtrl = $("#pnliTrack_MUReport #hfProviderId");
        var onSelect = function (e) {
            $("#pnliTrack_MUReport #hfProviderId").val(e.id);
            // iTrack_Quality.setProviderName(); utility.FillProviderNPI('#pnliTrack_MUReport #frmiTrack_MUReport', '#hfProviderId', '#NPItxt');
        };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect);


    },

    checkQuarterly: function (chkBox) {
        if ($(chkBox).is(':checked')) {
            $('#pnliTrack_MUReport #FromDate').addClass('disableAll');
            $('#pnliTrack_MUReport #FromDate').val('');
            $('#pnliTrack_MUReport #ToDate').val('');
            $('#pnliTrack_MUReport #Quarter').removeClass('disableAll');
            $('#pnliTrack_MUReport #Year').removeClass('disableAll');

        } else {
            $('#pnliTrack_MUReport #FromDate').removeClass('disableAll');
            // $('#pnliTrack_MUReport #ddlQuarter').addClass('disableAll');
            // $('#pnliTrack_MUReport #ddlYear').addClass('disableAll');
            $('#pnliTrack_MUReport #Quarter').addClass('disableAll');
            $('#pnliTrack_MUReport #Year').addClass('disableAll');
        }
    },
    PrintReport: function () {
        //var providerData = "";
        //if (iTrack_MUReport.params.ProviderData != 'undefined' && iTrack_MUReport.params.ProviderData != null)
        //    providerData = iTrack_MUReport.params.ProviderData.split('<br/>');
        //var newProviderText = '';
        //for (var i = 0; i < providerData.length; i++) {
        //    if ($.trim(providerData[i]) != '') {
        //        newProviderText += '<li class="text-left">' + providerData[i] + '</li>';
        //    }
        //}
        //$('#' + iTrack_MUReport.params.PanelID + " #printcall #ProviderList").append(newProviderText);

        //var practiceData = "";
        //if (iTrack_MUReport.params.PracticeData != 'undefined' && iTrack_MUReport.params.PracticeData != null)
        //    practiceData = iTrack_MUReport.params.PracticeData.split('<br/>');
        //var newPracticeText = '';
        //for (var i = 0; i < practiceData.length; i++) {
        //    if ($.trim(practiceData[i]) != '') {
        //        newPracticeText += '<li class="text-right">' + practiceData[i] + '</li>';
        //    }
        //}
        //$('#' + iTrack_MUReport.params.PanelID + " #printcall #PracticeList").append(newPracticeText);

        //var dt = new Date();
        //var time = dt.getHours() + ":" + dt.getMinutes() + ":" + dt.getSeconds();
        //var day = dt.getMonth() + "/" + dt.getDate() + "/" + dt.getYear();
        //var datetime = day + " " + time;
        //$('#' + iTrack_MUReport.params.PanelID + " #printcall #liCurrentDate").text(datetime);
        params["UlContent"] = $("#pnliTrack_MUReport #MuStageReportView")[0].outerHTML;
        $("#pnliTrack_MUReport #printcall #ulContent").append(params["UlContent"]);




        $('#pnliTrack_MUReport #printcall').removeClass('hidden');
        kendo.drawing.drawDOM("#pnliTrack_MUReport #printcall", {
            landscape: false,
            scale: 0.6,
            paperSize: "A4",
            margin: {
                left: "10mm",
                top: "7mm",
                right: "10mm",
                bottom: "15mm"
            },
            template: kendo.template($("#pnliTrack_MUReport #page-templateLegacy").html())
        }).then(function (group) {

            kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                var params = [];
                var PrintPDFDataURL = dataURL.split('data:application/pdf;base64,').join('');
                params["PrintPDFDataURL"] = PrintPDFDataURL;
                params["PreviewPdf"] = true;
                utility.PDFViewer(params["PrintPDFDataURL"], true, null, null, true);
                $("#pnliTrack_MUReport #printcall").addClass('hidden');
                $("#pnliTrack_MUReport #printcall #ulContent").html("");
            });

        });

    },
    validateFieldsBeforeRunReport: function () {
        var retVal = true;
        if (($('#pnliTrack_MUReport #dtpFromDate').val() == '' || $('#pnliTrack_MUReport #txtProvider').val() == ''
            || $('#pnliTrack_MUReport #dtpToDate').val() == '')
            && $('#pnliTrack_MUReport #QuarterlyReport').is(':checked') == false) {
            retVal = false;
        }
        return retVal;
    },
    generateMuStageReport: function (FromDashBoard) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("MUStage3", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (iTrack_MUReport.validateFieldsBeforeRunReport()) {
                    $('#MuStageReportView').show();
                    $('#pnliTrack_MUReport #MUStagePassedProgressBar').html('').show();
                    iTrack_MUReport.generateMuStageReport_DBCall(FromDashBoard).done(function (response) {
                        response = JSON.parse(response);
                        iTrack_MUReport.totalResultResponse = response;
                        if (response.status != false) {
                            var data = JSON.parse(response.MU_JSON);
                            iTrack_MUReport.MUdata_Patient = JSON.parse(response.MU_JSON_PatientWise);

                            if (data.length > 0) {
                                var passedCount = 0;
                                $('#pnliTrack_MUReport #tblMUData > tbody').html("");
                                $.each(data, function (i, item) {
                                    if (item.Measure == "") {
                                        return;
                                    }
                                    var $row = $('<tr/>');
                                    $row.attr("id", "gvMU2_row" + item.ID);
                                    $row.attr("onclick", "utility.SelectGridRow($('#gvMU2_row" + item.ID + "'))");
                                    var TargetRequired = Number(item.RequiredTarget);
                                    var Targetmet = Number(item.PerfromanceRate1);
                                    var ProgressClass = "";
                                    var labelClass = "";
                                    var circleClass = "";
                                    if (TargetRequired <= Targetmet) {
                                        success = "progress-bar-success";
                                    } else if (TargetRequired > Targetmet) {
                                        ProgressClass = "progress-bar-danger";
                                    }
                                    if (TargetRequired <= Targetmet) {
                                        circleClass = "success";
                                        labelClass = "green";
                                    } else if (TargetRequired > Targetmet) {
                                        circleClass = "danger";
                                        labelClass = "red";
                                    }
                                    var PatientCount = 0;
                                    if (iTrack_MUReport.MUdata_Patient != null && iTrack_MUReport.MUdata_Patient.length > 0) {
                                        var ListPatient = [];
                                        $.each(iTrack_MUReport.MUdata_Patient, function (i, dataItem) {
                                            //if (dataItem.ID == item.ID && dataItem.Numerator == "0") {
                                            if (dataItem.ID == item.ID) {
                                                ListPatient.push(dataItem);
                                            }
                                        });
                                        PatientCount = ListPatient.length;

                                    }
                                    var disableButtonClass = "";
                                    if (PatientCount == 0) {
                                        disableButtonClass = " disableAll";
                                    }
                                    if (item.Measure == "CPOE Labs") {
                                        item.Measure = "CPOE Laboratory";
                                    }
                                    if (item.Measure == "Secure Message") {
                                        item.Measure = "Secure Messaging";
                                    }
                                    $row.append('<td style=display:none>' + item.ID + '</td>' + '<td style="min-width:345px; max-width:345px;width:345px;">' + item.Measure + '</td>'
                                       + '<td style="min-width:80px; max-width:80px;width:80px;">' + item.Numerator + '/' + item.Denominator + '</td>'
                                        + '<td style="min-width:46px; max-width:46px;width:46px;">' + Math.round(item.ReportingRate2) + '</td>' //RequiredTarget
                                   + '<td style="min-width:83px; max-width:83px;width:83px;">' + item.RequiredTarget + ' %</td>'
                                  + '<td style="min-width:128px; max-width:128px;width:128px;">' + '<div class="progress-bar-circle ' + circleClass + '" data-percent="' + Math.round(item.PerfromanceRate1) + '" data-line="2" data-size="50" ></div><label id="performanceRateCount" style="display:none" class="' + labelClass + '">' + Math.round(item.PerfromanceRate1) + ' %</label></td>'
                                  + '<td  style="min-width:128px; max-width:128px;width:128px;">' + '<button type="button" onclick="iTrack_MUReport.OpenMUStage1(' + item.ID + ',\'' + item.Measure + '\');" class="btn btn-sm ' + disableButtonClass + '">PATIENTS (' + PatientCount + ')</button>' + '<label id="patCount" style="display:none">' + PatientCount + '</label></td>');


                                    $('#pnliTrack_MUReport #tblMUData > tbody').last().append($row);

                                    if (item.IsObjectCompleted == "True") {
                                        passedCount++;
                                    }
                                });
                                var dataPercent = (passedCount / data.length) * 100;
                                $('#pnliTrack_MUReport #MUStagePassedProgressBar').html('<h4> Passed ' + passedCount + ' of ' + data.length + ' Objectives</h4><div class="progress mb-none mt-xs mb-xs">' +
                                                                    '<div class="progress-bar progress-bar-success" role="progressbar" aria-valuenow="' + passedCount + '" aria-valuemin="0" aria-valuemax="' + data.length + '" style="width: ' + dataPercent + '%;min-width:0px !important;"></div>' +
                                                                '</div>');
                                iTrack_MUReport.progressCircle('#pnliTrack_MUReport #MUStagePassedProgressBar');
                                iTrack_MUReport.progressCircle('#pnliTrack_MUReport #tblMUData ');
                            }
                        }
                    });
                } else {
                    $('#MuStageReportView').hide();
                    $('#pnliTrack_MUReport #MUStagePassedProgressBar').html('').hide();
                    utility.DisplayMessages('Please select Provider, From date, To date or select a quarter.', 3);
                }
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    OpenMUStage1: function (ID, Measure) {
        var params = [];
        params["ParentCtrl"] = "iTrack_MUReport";
        params["FromAdmin"] = 0;
        params["ID"] = ID;
        params["Measure"] = Measure;
        params["rptName"] = "MU Stage 3 Report";
        params["resultSet"] = iTrack_MUReport.MUdata_Patient;
        LoadActionPan("iTrack_MUStage3", params);
    },
    progressCircle: function (id) {
        var el = id + ' .progress-bar-circle'; //document.getElementsByClassName('progress-bar-circle'); // get canvas
        $(el).each(function (count) {
            var options = {
                percent: this.getAttribute('data-percent') || 0,
                size: this.getAttribute('data-size') || 100,
                lineWidth: this.getAttribute('data-line') || 100,
                rotate: this.getAttribute('data-rotate') || 0,
                primaryClr: "#0088cc",
                dangerClr: '#d2322d',
                successClr: '#47a447',
                defaultClr: '#555555',
                remainCircleClor: '#efefef'
            }
            $(this).html('<canvas id="' + count + 'MUReport"></canvas>');
            var canvasID = $(this).children('canvas').attr("id");
            var canvas = document.getElementById(canvasID);
            var span = document.createElement('span');
            span.textContent = options.percent + '%';
            if (typeof (G_vmlCanvasManager) !== 'undefined') {
                G_vmlCanvasManager.initElement(canvas);
            }

            var ctx = canvas.getContext('2d');
            canvas.width = canvas.height = options.size;

            this.appendChild(span);
            this.appendChild(canvas);

            //settings
            $(this).height(options.size);
            $(this).width(options.size);
            //settings for span
            $(this).children("span").css("line-height", options.size + "px");
            $(this).children("span").width(options.size);

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
            drawCircle(options.remainCircleClor, options.lineWidth, 100 / 100);
            //circle themes color
            if (options.percent === "0") {
                return;
            }
            else if ($(this).hasClass("success") == true) {
                drawCircle(options.successClr, options.lineWidth, options.percent / 100);
            }
            else if ($(this).hasClass("danger") == true) {
                drawCircle(options.dangerClr, options.lineWidth, options.percent / 100);
            }
            else if ($(this).hasClass("primary") == true) {
                drawCircle(options.primaryClr, options.lineWidth, options.percent / 100);
            }
            else {
                drawCircle(options.defaultClr, options.lineWidth, options.percent / 100);
            }

        });//each function
    },
}