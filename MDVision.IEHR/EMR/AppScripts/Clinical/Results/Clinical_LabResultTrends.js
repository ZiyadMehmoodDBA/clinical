ClinicalLabResultTrends = {
    params: [],
    bIsFirstLoad: true,
    Trends: null,
    SelectedRows: [],
    PatPracDetails: null,
    previousHighlightedRow: null,
    LOINCs: [],
    Load: function (params) {
        ClinicalLabResultTrends.params = params;
        BackgroundLoaderShow(true);

        ClinicalLabResultTrends.previousHighlightedRow = null;
        EMRUtility.ValidateFromToDate('pnlClinicalLabResultTrends', 'dptStartDate', 'dptToDate', true, function () { }, function () { }, "To Date should be greater than From Date");
        EMRUtility.ValidateFromToDate('pnlClinicalLabResultTrends', 'dptStartDate', 'dptToDate', true, function () { }, function () { }, "To Date should be greater than From Date");

        $('#pnlClinicalLabResultTrends input[name=radgroup]').change(function () {
            if ($('#pnlClinicalLabResultTrends input[name=radgroup]:checked').val() == "DOL") {
                $('#pnlClinicalLabResultTrends #printLabLetter').addClass("disableAll");
            }
            else {
                $('#pnlClinicalLabResultTrends #printLabLetter').removeClass("disableAll");
            }
        });

        if (ClinicalLabResultTrends.params.ParentCtrl == "mstrTabDashBoard") {
            $('#pnlClinicalLabResultTrends #PatientDetails').append(ClinicalLabResultTrends.params.PatientInfo);
        }
        else {
            $('#pnlClinicalLabResultTrends #PatientDetails').append($('#banner_PatientName').parent().text() + ' ' + $('#banner_PatientDOB').text());
        }

        if (ClinicalLabResultTrends.params.PanelID != 'pnlClinicalLabResultTrends') {
            ClinicalLabResultTrends.params.PanelID = ClinicalLabResultTrends.params.PanelID + ' #pnlClinicalLabResultTrends';
        } else {
            ClinicalLabResultTrends.params.PanelID = 'pnlClinicalLabResultTrends';
        }
        ClinicalLabResultTrends.fetch_LabTrends().done(function (response) {
            response = JSON.parse(response);
            ClinicalLabResultTrends.Trends = response.Trends;
            ClinicalLabResultTrends.PatPracDetails = response.PatPracticeDetails;
            ClinicalLabResultTrends.fetched_trendsGridLoad(response);
            ClinicalLabResultTrends.BindSelector(response);
        });

        $('#dgvLabTrends').on("click", "tr.k-master-row", function (e) {
            var $link = $(this).find("td.k-hierarchy-cell .k-icon");
            $link.click();
        });
    },
    BindSelector: function (response) {
        $('#pnlClinicalLabResultTrends #ddlFilterId').append('<option value=""> - Select - </option>');
        $.each(response.Trends, function (i, item) {
            $('#pnlClinicalLabResultTrends #ddlFilterId').append('<option value="' + item.TestDescription + '">' + item.TestDescription + '</option>');
        });
        $('#' + ClinicalLabResultTrends.params.PanelID + ' #ddlFilterId').val(response.Trends[0].TestDescription);
    },
    fetched_trendsGridLoad: function (object) {
        ClinicalLabResultTrends.LOINCs = [];
        var DetailTemplate = '  <div id="DetailTemp" class="table-responsive"> '
                   + ' <input type="hidden" id="Lab_SelectedDataKeys" /> '
                   + ' <table class="table table-bordered table-striped table-condensed table-hover mb-none tablePaddingAdjst" id="dgvTrends"> '
                   + '     <thead><tr></tr>  </thead><tbody></tbody></table></div>';
        setTimeout(function () {
        kendo.fx($("#dgvLabTrends").kendoGrid({
            dataSource: object.Trends,
            resizable: true,
            scrollable: false,
            noRecords: true,
            messages: {
                noRecords: "No Trends Fetched."
            },
            detailTemplate: kendo.template(DetailTemplate),
            detailInit: detailInit,
            detailExpand: function (e) {
                this.collapseRow(this.tbody.find(' > tr.k-grid-edit-row').not(e.masterRow));
                var detailRow = e.detailRow;
                setTimeout(function () {
                    kendo.fx(detailRow).fade("in").play();
                }, 0);

            },
            columns: [
            //{ title: "Action", width: "100px", template: '#=Admin_CCMCareTeam.ActionCCMCareTeams(data)#' },
            { title: "Test Name", field: "TestDescription", width: "97.5%" },

            ],
        })).expand("vertical").play();


        var expandedRow;

        function detailInit(e) {
            ClinicalLabResultTrends.LOINCs = [];
            // Only one open at a time
            if (expandedRow != null && expandedRow[0] != e.masterRow[0]) {

                var grid = $('#dgvLabTrends').data('kendoGrid');
                grid.collapseRow(expandedRow);
                if (expandedRow[0].nextElementSibling != null) {

                    expandedRow[0].nextElementSibling.remove();
                }
            }
            expandedRow = e.masterRow;

            var detailRow = e.detailRow;
            var x = 0;
            var $tbody = detailRow.find('#dgvTrends').find('tbody');
            var $thead = detailRow.find('#dgvTrends').find('thead');

            var $ChildRowTestHead = $('<tr/>');

            if ($('#pnlClinicalLabResultTrends input[name=radgroup]:checked').val() == "DOT") {
                $ChildRowTestHead.append('<th id="SelectRecord" class="size10 center" coltype="checkbox"> <input type="checkbox" id="chkHeaderObservations" onchange="ClinicalLabResultTrends.checkUncheckAllObservations(this);" class="pull-left" coltype="checkbox"/></th><th></th><th>Observation</th>');
                $.each(e.data.ResultDates, function (i, item) {
                    $ChildRowTestHead.append('<th>' + item + '</th>');
                });
                $thead.append($ChildRowTestHead);

                $.each(e.data.TestTrends, function (i, item) {
                    var $ChildRowTestBody = $('<tr onclick="ClinicalLabResultTrends.rowHighlight(this)"/>').addClass("childRowTest-bg");
                    $ChildRowTestBody.attr('id', item.LOINC);
                    $ChildRowTestBody.append('<td><input type="checkbox" id="' + item.LOINC + '" onclick="ClinicalLabResultTrends.checkRow(\'' + item.LOINC + '\',\'' + item.LOINCDescription + '\',\'' + e.data.TestCode + '\',\'' + e.data.TestDescription + '\', this);" name="SelectCheckBoxOrder" class="input-block"/></td><td><a class="btn btn-xs" href="#" onclick="ClinicalLabResultTrends.open_Graphs(\'' + item.LOINC + '\',\'' + item.LOINCDescription + '\',\'' + e.data.TestCode + '\',\'' + e.data.TestDescription + '\');" title="See Record"><i class="fa fa-line-chart green"></i></a></td><td style="color:dodgerblue;">' + item.LOINCDescription + '' + (item.ResultsValues[0].Unit != "" ? ' - ' + item.ResultsValues[0].Unit : "") + '</td>');
                    if (item.ResultsValues.length > 0) {
                        $.each(item.ResultsValues, function (i, innerItem) {
                            var obj = ClinicalLabResultTrends.EvaluateResultRange(innerItem.Value, innerItem.ReferenceRange, innerItem.ReferenceRangeInterpration, innerItem.ReferenceRangeDescription, innerItem.Unit, innerItem.Flag);
                            var comments = ClinicalLabResultTrends.checkCommentsLength(innerItem.Date, item.LOINC, item.LOINCDescription, innerItem.Comments);
                            var columntooltip = '';
                            var oncolumnClick = 'onclick="ClinicalLabResultTrends.open_LabTestInterpretation(\'' + innerItem.Date + '\',\'' + item.LOINC + '\',\'' + item.LOINCDescription + '\',\'' + utility.decodeHtml(innerItem.Comments) + '\');"';
                            if (innerItem.Comments != "") {
                                if (innerItem.Comments.length > 40) {
                                    columntooltip = 'data-toggle="tooltip" data-placement="bottom" data-html="true" title="<b>NOTES: </b> ' + utility.decodeHtml(innerItem.Comments.substring(0, 50)) + '....."';
                                } else {
                                    columntooltip = 'data-toggle="tooltip" data-placement="bottom" data-html="true" title="<b>NOTES: </b> ' + utility.decodeHtml(innerItem.Comments) + '"';
                                }
                            }
                            if (innerItem.Comments.length > 40) {
                                $ChildRowTestBody.append('<td ' + oncolumnClick + ' ' + columntooltip + '><span style="color:' + obj["Color"] + ';" >' + innerItem.Value + ' </span> ' + obj["icon"] + ' ' + comments + '</td>');
                            } else {
                                $ChildRowTestBody.append('<td  ' + columntooltip + '><span style="color:' + obj["Color"] + ';" >' + innerItem.Value + ' </span> ' + obj["icon"] + ' ' + comments + '</td>');
                            }
                            $tbody.append($ChildRowTestBody);
                        });
                    }
                    else {
                        $ChildRowTestBody.append('<td></td>');
                        $tbody.append($ChildRowTestBody);
                    }
                });
            }
            else {
                $ChildRowTestHead.append('<th id="SelectRecord" class="size10 center" coltype="checkbox"> <input type="checkbox" id="chkHeaderObservations" onchange="ClinicalLabResultTrends.checkUncheckAllObservations(this);" class="pull-left" coltype="checkbox"/></th><th></th><th>Date</th>');
                $.each(e.data.TestTrends, function (i, item) {
                    $ChildRowTestHead.append('<th>' + item.LOINCDescription + '' + (item.ResultsValues[0].Unit != "" ? ' ' + item.ResultsValues[0].Unit : "") + '</th>');
                });
                $thead.append($ChildRowTestHead);
                if (e.data.ResultDates.length > 0) {
                    $.each(e.data.ResultDates, function (i, innerItem) {
                        var $ChildRowTestBody = $('<tr onclick="ClinicalLabResultTrends.rowHighlight(this)"/>').addClass("childRowTest-bg");
                        $ChildRowTestBody.append('<td><input type="checkbox" onclick="ClinicalLabResultTrends.checkRow(\'\',\'\',\'' + e.data.TestCode + '\',\'' + e.data.TestDescription + '\', this);" id="' + innerItem + '" name="SelectCheckBoxOrder" class="input-block"/></td><td><a class="btn btn-xs" disabled href="#" title="See Record"><i class="fa fa-line-chart green"></i></a></td><td style="color:dodgerblue;">' + innerItem + '</td>');
                        $.each(e.data.TestTrends, function (i, item) {
                            $.each(item.ResultsValues, function (i, j) {
                                if (j.Date == innerItem) {
                                    var obj = ClinicalLabResultTrends.EvaluateResultRange(j.Value, j.ReferenceRange, j.ReferenceRangeInterpration, j.ReferenceRangeDescription, j.Unit, j.Flag);
                                    var comments = ClinicalLabResultTrends.checkCommentsLength(innerItem, item.LOINC, item.LOINCDescription, j.Comments);
                                    var columntooltip = '';
                                    var oncolumnClick = 'onclick="ClinicalLabResultTrends.open_LabTestInterpretation(\'' + innerItem.Date + '\',\'' + item.LOINC + '\',\'' + item.LOINCDescription + '\',\'' + utility.decodeHtml(innerItem.Comments) + '\');"';
                                    if (innerItem.Comments != "") {
                                        if (innerItem.Comments.length > 40) {
                                            columntooltip = 'data-toggle="tooltip" data-placement="bottom" data-html="true" title="<b>NOTES: </b> ' + utility.decodeHtml(innerItem.Comments.substring(0, 50)) + '....."';
                                        } else {
                                            columntooltip = 'data-toggle="tooltip" data-placement="bottom" data-html="true" title="<b>NOTES: </b> ' + utility.decodeHtml(innerItem.Comments) + '"';
                                        }
                                    }
                                    if (innerItem.Comments.length > 40) {
                                        $ChildRowTestBody.append('<td ' + oncolumnClick + ' ' + columntooltip + '><span style="color:' + obj["Color"] + ';" >' + j.Value + '</span>' + obj["icon"] + ' ' + comments + '</td>');
                                    } else {
                                        $ChildRowTestBody.append('<td  ' + columntooltip + '><span style="color:' + obj["Color"] + ';" >' + j.Value + '</span>' + obj["icon"] + ' ' + comments + '</td>');
                                    }
                                }
                            });
                        });
                        $tbody.append($ChildRowTestBody);
                    });
                }
                else {
                    $ChildRowTestBody.append('<td></td>');
                    $tbody.append($ChildRowTestBody);
                }

            }
            $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });


            ClinicalLabResultTrends.BindHeader();
            var Grid = $('#dgvTrends').clone();
            $($(Grid).find('thead tr')[0]).remove();
            $($($(Grid).find('thead tr')[0]).children()[0]).remove();
            $($($(Grid).find('thead tr')[0]).children()[0]).remove();
            $(Grid).find('tbody').html("");
            $(Grid).attr('id', 'dgvLabTrendsPrint');

            var TestHeading = '<h4>' + e.data.TestDescription + '</h4>';
            var DateDescription = 'From ' + e.data.ResultDates[0] + ' to ' + e.data.ResultDates[e.data.ResultDates.length-1];
            var row = '<div class="row"> <div class="col-md-6">' + TestHeading + ' </div> <div class="col-md-5"> <p class="pull-right">' + DateDescription + ' </p></div> </div>';

            $('#PrintPanel #TrendsPrint').html("");
            $('#PrintPanel #dgvTrendsGraphsPrint').html("");
            $('#PrintPanel #TrendsPrint').append(row);
            $('#PrintPanel #TrendsPrint').append(Grid);
            $('#PrintPanel #dgvTrendsGraphsPrint').append(row);
        }

        $('#dgvLabTrends').data('kendoGrid').expandRow($('#dgvLabTrends tbody>tr:first'));
        }, 250);
    },
    checkUncheckAllObservations: function (chkBox) {
        if ($(chkBox).is(':checked')) {
            $("#pnlClinicalLabResultTrends [name='SelectCheckBoxOrder']").prop("checked", true);
        } else {
            $("#pnlClinicalLabResultTrends [name='SelectCheckBoxOrder']").prop("checked", false);
        }
        $("#pnlClinicalLabResultTrends #dgvTrends").find('input:checkbox').each(function () {
            $(this).trigger('onclick');
        });
    },
    printLetter: function (type, Base64) {
        if ($('#dgvTrends tbody input[type=checkbox]:checked').length > 0) {
            var LOINCs = "";
            for (i = 0; i < ClinicalLabResultTrends.LOINCs.length; i++) {
                LOINCs += ClinicalLabResultTrends.LOINCs[i] + ',';
            }
            var params = [];
            params["LabResultId"] = ClinicalLabResultTrends.params.LabResultId;
            params["ParentCtrl"] = "ClinicalLabResultTrends";
            params["LOINC"] = LOINCs;
            params["header"] = $('#pnlClinicalLabResultTrends #PanelHead').clone();
            params["PatientId"] = ClinicalLabResultTrends.PatPracDetails.PatientId;
            if (type != null) {
                params["Type"] = type;
                params["LabOrderId"] = ClinicalLabResultTrends.params.LabOrderId;
                params["Base64"] = Base64;
            }
            LoadActionPan("Clinical_LabResultTrendsLetter", params);
        }
        else {
            utility.DisplayMessages("Please select an Observation.", 2);
        }
    },
    checkCommentsLength: function (Date, LOINC, LOINCDescription, Comments) {
        var Notes = "";
        if (Comments != "") {
            if (Comments.length > 40) {
                Notes = '<span <i class="fa blue fa-sticky-note" aria-hidden="true" ></i></span>';
            }
            else {
                Notes = '<span <i class="fa blue fa-sticky-note" aria-hidden="true"></i></span>';
            }
            //Notes = '<div class="d-inline-block position-rl p-none"><a href="#" class="tooltip-custom-v btn btn-link btn-xs" onmouseenter="ClinicalLabResultTrends.domreadyFunctions(this,event);"><i class="fa blue fa-sticky-note"></i></a><div class="v-ellipses-tooltip"><div class="tooltip-container"><p><b>NOTES: </b>' + utility.decodeHtml(Comments) + '</p><a href="#" class="btn btn-default btn-xs tooltip-read-more white hidden" onclick="ClinicalLabResultTrends.open_LabTestInterpretation(\'' + Date + '\',\'' + LOINC + '\',\'' + LOINCDescription + '\',\'' + utility.decodeHtml(Comments) + '\');">read more.</a></div></div></div>';
        }
        else {
            Notes = "";
        }
        return Notes;
    },
    rowHighlight: function (obj) {
        if (ClinicalLabResultTrends.previousHighlightedRow != null) {
            ClinicalLabResultTrends.previousHighlightedRow.children("td").removeClass("custom-highlight");
        }
        $(obj).children("td").addClass("custom-highlight");
        ClinicalLabResultTrends.previousHighlightedRow = $(obj);
    },
    BindHeader: function () {
        var details = ClinicalLabResultTrends.PatPracDetails;
        var PracticeDetails = '<div id="PracticePrintDetails">' + details.PracticeName + ' <br> '
                                + details.PracticeAddress + ' <br> ' + details.PracticeCity + ', ' + details.PracticeState + ', ' + details.PracticeZIP;
        var PatientDetails = '<div id="PatientPrintDetails">' + details.LastName + ', ' + details.FirstName + ' <br> ' + details.AccountNumber +
                               '<br>' + details.Gender + ', ' + details.PatientDOB + ' <br> '
                                + details.PatientAddress + ', ' + details.PatientCity + ', ' + details.PatientState
                                + ', ' + details.PatientZipCode;
        var ProviderDetails = '<div id="ProviderPrintDetails">' + ' <br> ' + details.ProviderName + ' <br> '
                                       + details.ProviderNPI + ' <br> ' + details.ProviderSpecialty;
        $('#PrintPanel #ProviderPrintDetails').replaceWith(ProviderDetails);
        $('#PrintPanel #PracticePrintDetails').replaceWith(PracticeDetails);
        $('#PrintPanel #PatientPrintDetails').replaceWith(PatientDetails);
    },
    checkRow: function (LOINC, LOINCDescription, TestCode, TestDescription, $object) {
        if ($($object).prop('checked')) {
            var row = $($object).parent().parent().clone();
            $(row).find('.fa-sticky-note').removeClass("fa-sticky-note");
            $(row).find('.fa-caret-up').removeClass("fa-caret-up");
            $(row).find('.fa-caret-down').removeClass("fa-caret-down");
            $(row).find('.custom-highlight').removeClass("custom-highlight");
            $(row.children()[0]).remove();
            $(row.children()[0]).remove();

            $('#PrintPanel #dgvLabTrendsPrint').find('tbody').append(row);
            if (LOINC != null && LOINC != "") {
                if ($('#pnlClinicalLabResultTrends input[name=radgroup]:checked').val() != "DOL") {
                    $("#pnlClinicalLabResultTrends #printLabLetter").removeClass("disableAll");
                }
                
                ClinicalLabResultTrends.BindPrintingGraphs(LOINC, LOINCDescription, TestCode, TestDescription);
                ClinicalLabResultTrends.MarkLOINC(LOINC);
            }
        }
        else {
            $.each($('#PrintPanel #dgvLabTrendsPrint tbody tr'), function (i, item) {
                if ($($object).parent().parent().attr('id') == $(item).attr('id')) {
                    item.remove();
                }

                if (LOINC != null && LOINC != "") {
                    $('#PrintPanel #' + LOINC).remove();
                    ClinicalLabResultTrends.DemarkLOINC(LOINC);
                }
            });
        }
    },
    MarkLOINC: function (LOINC) {
        ClinicalLabResultTrends.LOINCs.push(LOINC);
    },
    DemarkLOINC: function (LOINC) {
        for (i = 0; i < ClinicalLabResultTrends.LOINCs.length; i++) {
            if (ClinicalLabResultTrends.LOINCs[i] == LOINC) {
                ClinicalLabResultTrends.LOINCs.splice(i, 1);
            }
        }
    },
    BindPrintingGraphs: function (LOINC, LOINCDescription, TestCode, TestDescription) {
        $.each(ClinicalLabResultTrends.Trends, function (i, item) {

            if (item.TestDescription == TestDescription) {

                $.each(item.TestTrends, function (i, innerItem) {
                    if (innerItem.LOINC == LOINC) {


                        var Graph = $('<div id="' + LOINC + '"></div>')
                        Graph.kendoChart({
                            dataSource: {
                                data: innerItem.ResultsValues
                            },
                            title: {
                                text: LOINCDescription
                            },
                            chartArea: {
                                width: 700,
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
                            }

                        })

                        $('#PrintPanel #dgvTrendsGraphsPrint').append(Graph);

                        return false;
                    }
                });
            }

        });

    },
    printTrends: function (type) {
        if ($('#dgvTrends tbody input[type=checkbox]:checked').length > 0) {
            var msg = "";
            var labResultBase64 = "";
            if (type != "Graph") {
                msg = 'Do you want to print ' + type.toLowerCase() + ' with Lab Letter?';
            }
            else{
                msg = 'Do you want to print ' + type.toLowerCase() + '(s) with Lab Letter?';
            }
            utility.myConfirmDetail(msg, function () {
                if (type == "Result") {
                    Clinical_LabResultTrendsView.previewLabResult(ClinicalLabResultTrends.PatPracDetails.PatientId, ClinicalLabResultTrends.params.LabResultId, ClinicalLabResultTrends.params.LabOrderId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            labResultBase64 = response.LabResultHTML;
                            ClinicalLabResultTrends.printLetter(type, labResultBase64);
                        }
                        else
                            utility.DisplayMessages(response.Message, 2);
                    });
                }
                else if (type == "Graph") {
                    ClinicalLabResultTrends.printGraphsWLetter();
                }
                else
                    ClinicalLabResultTrends.printLetter(type, labResultBase64);
            }
            , function () {
                ClinicalLabResultTrends.printTGRWithoutLabLetter(type);
            },
            'Confirm Print', type, 'The print preview would only contain latest date result for selected test and its observation(s).');
        }
        else {
            utility.DisplayMessages("Please select an observation.", 2);
        }
    },
    printGraphsWLetter: function () {
        setTimeout(function () {
            $('#PrintPanel').show();
            $('#TrendsPrint').hide();

            kendo.drawing.drawDOM('#PrintPanel', {
                landscape: false,
                scale: 0.7,
                paperSize: "Legal",
                margin: {
                    left: "10mm",
                    top: "10mm",
                    right: "10mm",
                    bottom: "30mm"
                }
            }).then(function (group) {
                kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                    var PrintPDFDataURL = dataURL.split('data:application/pdf;base64,').join('');
                    ClinicalLabResultTrends.printLetter("Graph", PrintPDFDataURL);
                    $('#TrendsPrint').show();
                    $('#dgvTrendsGraphsPrint').show();
                    $('#PrintPanel').hide();
                });
            });
        }, 250);
    },
    printTGRWithoutLabLetter: function (type) {
        var params = [];
        params["ParentCtrl"] = "ClinicalLabResultTrends";
        LoadActionPan('Clinical_LabResultTrendsView', params);
        setTimeout(function () {
            $('#PrintPanel').show();
            if (type == "Trend") {
                $('#dgvTrendsGraphsPrint').hide();
            }
            if (type == "Graph") {
                $('#TrendsPrint').hide();
            }
            kendo.drawing.drawDOM('#PrintPanel', {
                landscape: false,
                scale: 0.7,
                paperSize: "Legal",
                // margin: "2cm 3cm ",
                margin: {
                    left: "10mm",
                    top: "10mm",
                    right: "10mm",
                    bottom: "30mm"
                }//,
                // template: $('#' + CCM_CarePlanDetail.params["PanelID"] + " #page-template").html()
            }).then(function (group) {

                kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                    if (type != "Result") {
                        var PrintPDFDataURL = dataURL.split('data:application/pdf;base64,').join('');
                        Clinical_LabResultTrendsView.faxPDF = PrintPDFDataURL;
                        Clinical_LabResultTrendsView.printScreen(dataURL);
                    }
                    else {
                        Clinical_LabResultTrendsView.LabResultPreview(ClinicalLabResultTrends.PatPracDetails.PatientId, ClinicalLabResultTrends.params.LabResultId, ClinicalLabResultTrends.params.LabOrderId);
                    }
                    $('#TrendsPrint').show();
                    $('#dgvTrendsGraphsPrint').show();
                    $('#PrintPanel').hide();
                });

            });
        }, 250);
    },
    EvaluateResultRange: function (resultValue, resultRange, refRangeInterpration, refRangeDesc, resultUnit, flag) {
        var obj = {};
        obj["Color"] = "";
        obj["Notes"] = "";
        obj["icon"] = "";
        obj["FlagColor"] = "";
        if (flag != "Normal") {
            obj["FlagColor"] = "#DC143C";
        }
        else {
            obj["FlagColor"] = "";
        }
        if (resultRange && resultRange.indexOf("<") > -1) {
            var lowValue = resultRange.split("<")[0];
            var highValue = resultRange.split("<")[1];
            if (lowValue == "") {
                if (parseInt(resultValue) > parseInt(highValue)) {
                    obj["Color"] = "#DC143C";
                    obj["Notes"] = refRangeDesc == "" ? "Result is higher than the normal value." : refRangeDesc;
                    obj["icon"] = '<span data-toggle="tooltip" data-placement="bottom" data-html="true" title="<b>REFERENCE RANGE:</b> ' + resultRange + ' ' + resultUnit + ' <br><b>NOTES:</b> ' + obj["Notes"] + '"  <i class="fa red fas fa-caret-up" aria-hidden="true"></i></span>';
                }
            }
        }
        else if (resultRange && resultRange.indexOf(">") > -1) {
            var lowValue = resultRange.split(">")[0];
            var highValue = resultRange.split(">")[1];
            if (lowValue == "") {
                if (parseInt(resultValue) < parseInt(highValue)) {
                    obj["Color"] = "#DC143C";
                    obj["Notes"] = refRangeDesc == "" ? "Result is lower than the normal value." : refRangeDesc;
                    obj["icon"] = '<span data-toggle="tooltip" data-placement="bottom" data-html="true"  title="<b>REFERENCE RANGE:</b> ' + resultRange + ' ' + resultUnit + ' <br> <b>NOTES:</b> ' + obj["Notes"] + '"  <i class="fa red fas fa-caret-down" aria-hidden="true"></i></span>';
                }
            }
        }

        else if (resultRange && resultRange.indexOf("-") > -1) {
            var lowValue = resultRange.split("-")[0];
            var highValue = resultRange.split("-")[1];
            if ((parseInt(resultValue) > parseInt(highValue)) && (parseInt(resultValue) < parseInt(lowValue))) {
                obj["Color"] = "";
            }
            else if ((parseInt(resultValue) > parseInt(highValue))) {
                obj["Color"] = "#DC143C";
                obj["Notes"] = refRangeDesc == "" ? "Result is higher than the normal value." : refRangeDesc;
                obj["icon"] = '<span data-toggle="tooltip" data-placement="bottom" data-html="true"  title="<b>REFERENCE RANGE:</b> ' + resultRange + ' ' + resultUnit + ' <br> <b>NOTES:</b> ' + obj["Notes"] + '"  <i class="fa red fa-caret-up" aria-hidden="true"></i></span>';
            }
            else if (parseInt(resultValue) < parseInt(lowValue)) {
                obj["Color"] = "#DC143C";
                obj["Notes"] = refRangeDesc == "" ? "Result is lower than the normal value." : refRangeDesc;
                obj["icon"] = '<span data-toggle="tooltip" data-placement="bottom" data-html="true" title="<b>REFERENCE RANGE:</b> ' + resultRange + ' ' + resultUnit + ' <br> <b>NOTES:</b> ' + obj["Notes"] + '"  <i class="fa red fa-caret-down" aria-hidden="true"></i></span>';
            }
        }

        return obj;
    },
    fetch_LabTrends: function (FilterSearch, DateFrom, DateTo) {
        var param = {};
        param["LabResultId"] = ClinicalLabResultTrends.params.LabResultId;
        param["FilterSearch"] = FilterSearch;
        param["DateFrom"] = DateFrom;
        param["DateTo"] = DateTo;
        param["commandType"] = "fetch_labresult_trends";
        var data = JSON.stringify(param);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },
    open_Graphs: function(LOINC, LOINCDescription, MainTestCode, MainTestDescription) {
        var params = [];
        params["LOINC"] = LOINC;
        params["LOINCDescription"] = LOINCDescription;
        params["MainTestCode"] = MainTestCode;
        params["MainTestDescription"] = MainTestDescription;
        params["ParentCtrl"] = "ClinicalLabResultTrends";
        LoadActionPan('ClinicalLabResultTrendsGraphs', params);
    },
    open_LabTestInterpretation: function (Date, LOINC, LOINCDescription, Comments) {
        var params = [];
        params["Date"] = Date;
        params["LOINC"] = LOINC;
        params["LOINCDescription"] = LOINCDescription;
        params["Comments"] = Comments;
        params["ParentCtrl"] = "ClinicalLabResultTrends";
        LoadActionPan('Clinical_LabResultTrendsNotes', params);
    },
    SearchTrends: function () {
        ClinicalLabResultTrends.fetch_LabTrends($('#pnlClinicalLabResultTrends #ddlFilterId option:selected').val().trim()
            , $('#pnlClinicalLabResultTrends #dptStartDate').val(), $('#pnlClinicalLabResultTrends #dptToDate').val())
                .done(function (response) {
                    ClinicalLabResultTrends.LOINCs = [];
                    response = JSON.parse(response);
                    ClinicalLabResultTrends.Trends = response.Trends;
                    ClinicalLabResultTrends.PatPracDetails = response.PatPracticeDetails;
                    var Grid = $('#pnlClinicalLabResultTrends #dgvLabTrends').data("kendoGrid");
                    var dataSource = new kendo.data.DataSource({
                        data: response.Trends
                    });
                    Grid.setDataSource(dataSource); // Kendo YOURE AWESOME!!
                    Grid.expandRow($('#dgvLabTrends tbody>tr:first'));
                });
    },
    UnLoad: function () {
        if (ClinicalLabResultTrends.params != null && ClinicalLabResultTrends.params.ParentCtrl != null) {
            if (ClinicalLabResultTrends.params.ParentCtrl == 'clinicalTabLabOrder') {
                UnloadActionPan(ClinicalLabResultTrends.params["ParentCtrl"], "ClinicalLabResultTrends");
            } else {
                ClinicalLabResultTrends.params.PanelID = ClinicalLabResultTrends.params.PanelID.replace(" #ClinicalLabResultTrends", "");
                UnloadActionPan(ClinicalLabResultTrends.params.ParentCtrl, 'ClinicalLabResultTrends', null, ClinicalLabResultTrends.params.PrPanelID);
            }
        }
        else
            UnloadActionPan(null, 'ClinicalLabResultTrends');
    }
}