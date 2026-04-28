ReportsSSRSPrintView = {
    params: [],
    bIsFirstLoad: true,

    Load: function (params) {
        ReportsSSRSPrintView.params = params;

        //if (ReportsSSRSPrintView.bIsFirstLoad) {
        //    ReportsSSRSPrintView.bIsFirstLoad = false;
        var self = $("#" + ReportsSSRSPrintView.params["PanelID"] + " #ReportsSSRSPrintView");
        if (ReportsSSRSPrintView.params["PanelID"] != "ReportsSSRSPrintView") {
            self = $("#" + ReportsSSRSPrintView.params["PanelID"] + " #ReportsSSRSPrintView");
        }
        $("#" + ReportsSSRSPrintView.params["PanelID"] + " #ReportsSSRSPrintView #reportPrintTitle").html(ReportsSSRSPrintView.params.ReportName);
        //Check for, to preview report as pdf or preview report as HTML
        if (ReportsSSRSPrintView.params.PreviewPdf == true) {
            ReportsSSRSPrintView.PrintPdfReport();
            $("#" + ReportsSSRSPrintView.params["PanelID"] + " #PrintPdfControlDiv").show();
        } else {
            $("#" + ReportsSSRSPrintView.params["PanelID"] + " #PrintPdfControlDiv").hide();
            ReportsSSRSPrintView.LoadReportsSSRSPrintView();
        }


        //}
    },
    LoadReportsSSRSPrintView: function () {
        if (ReportsSSRSPrintView.params.ReportName == "Financial Analysis At CPT Level") {
            ReportsDetailsHTML = ReportsSSRSPrintView.CreateReportPrintFinancial();
            $("#" + ReportsSSRSPrintView.params["PanelID"] + " #ReportsSSRSPrintView #reportPrintTitle").html(ReportsSSRSPrintView.params.ReportName);
            $("#" + ReportsSSRSPrintView.params["PanelID"] + " #ReportsSSRSPrintView #printMe").attr('disabled', false);
            $("#" + ReportsSSRSPrintView.params["PanelID"] + " #ReportsSSRSPrintView #ReportPrintView").html($(ReportsDetailsHTML).find('table[cols=26]')[0].outerHTML);
            //$('#ReportPrintView div:first').addClass("table-responsive Of-a");
            //$('#ReportPrintView div:first').attr('id', 'reportPrintTable')
            $("#" + ReportsSSRSPrintView.params["PanelID"] + ' #ReportPrintView table[cols=26]').wrap("<div class='table-responsive Of-a' id='reportPrintTable'>");
            $("#" + ReportsSSRSPrintView.params["PanelID"] + ' #ReportPrintView table[cols=26]').addClass('table table-bordered table-striped table-condensed table-hover');

            $("#" + ReportsSSRSPrintView.params["PanelID"] + ' #ReportPrintView table[cols=26]').prepend('<thead>' + $('#ReportPrintView table[cols=26] tbody').find('tr:nth-child(2)').html().replace(/td/gi, 'th') + '</thead>');
            $("#" + ReportsSSRSPrintView.params["PanelID"] + ' #ReportPrintView table[cols=26] tbody').find('tr:nth-child(2)').remove();
            $("#" + ReportsSSRSPrintView.params["PanelID"] + ' #ReportPrintView table[cols=26]').find('thead').addClass('printHeading');
            $("#" + ReportsSSRSPrintView.params["PanelID"] + ' #ReportPrintView table[cols=26]').find('thead').find('th').addClass('noWordBreak');
            $("#" + ReportsSSRSPrintView.params["PanelID"] + ' #ReportPrintView table[cols=26] tbody').find('tr').each(function () {
                if ($(this).text() == "") {
                    $(this).remove();
                }
            });
            $("#" + ReportsSSRSPrintView.params["PanelID"] + ' #ReportPrintView table[cols=26] tbody').find('tr').each(function () {
                if ($(this).find('td:first').text() == "") {
                    $(this).find('td:first').remove();
                }
            });
            $("#" + ReportsSSRSPrintView.params["PanelID"] + ' #ReportPrintView table[cols=26] thead').find('th').each(function () {
                if ($(this).text() == "") {
                    $(this).remove();
                }
            });
            $("#" + ReportsSSRSPrintView.params["PanelID"] + ' #ReportPrintView table[cols=26] tbody tr:last').css('background', 'grey');
        } else if (ReportsSSRSDashboard.ReportName == "MU Stage 1 Report" || ReportsSSRSDashboard.ReportName == "MU Stage 2 Report" || ReportsSSRSDashboard.ReportName == "MU Stage 2 Report Latest" || ReportsSSRSDashboard.ReportName == "MU Stage 3 Report") {
            $("#" + ReportsSSRSPrintView.params["PanelID"] + " #ReportsSSRSPrintView #reportPrintTitle").html(ReportsSSRSDashboard.ReportName);

            if (ReportsSSRSPrintView.params["ReportPrintHTML"] != "") {

                $("#" + ReportsSSRSPrintView.params["PanelID"] + " #ReportsSSRSPrintView #printMe").attr('disabled', false);
                $("#" + ReportsSSRSPrintView.params["PanelID"] + " #ReportsSSRSPrintView #ReportPrintView").html(ReportsSSRSPrintView.params["ReportPrintHTML"]);
                $("#" + ReportsSSRSPrintView.params["PanelID"] + ' #ReportsSSRSPrintView #ReportPrintView tr .progress-bar-circle').hide();
                $("#" + ReportsSSRSPrintView.params["PanelID"] + ' #ReportsSSRSPrintView #ReportPrintView tr .progress').hide();
                $("#" + ReportsSSRSPrintView.params["PanelID"] + ' #ReportsSSRSPrintView #ReportPrintView tr button').hide();
                $("#" + ReportsSSRSPrintView.params["PanelID"] + " #ReportsSSRSPrintView #ReportPrintView tr label").show();

            } else {

                $("#" + ReportsSSRSPrintView.params["PanelID"] + " #ReportsSSRSPrintView #printMe").attr('disabled', true);
                $("#" + ReportsSSRSPrintView.params["PanelID"] + " #ReportsSSRSPrintView #ReportPrintView").html("No Data Found!");
            }

        } else {
            ReportsSSRSPrintView.GetReportBody().done(function (response) {
                $("#" + ReportsSSRSPrintView.params["PanelID"] + " #ReportsSSRSPrintView #reportPrintTitle").html(ReportsSSRSPrintView.params.ReportName);

                var d = new Date($('#userCurrentTime').text());

                if (d == "Invalid Date")
                    d = new Date(Date($('#userCurrentTime').text()))
                var t = d.toLocaleTimeString();
                //var date = d.toDateString();

                $("#" + ReportsSSRSPrintView.params["PanelID"] + " #ReportsSSRSPrintView #reportPrintTime").html(utility.getFullDate(d) + " at " + t);
                if (response.status == true) {
                    ReportsSSRSPrintView.PrintParameters();
                    if (response.ReportsDetailsHTML != "") {
                        $("#" + ReportsSSRSPrintView.params["PanelID"] + " #ReportsSSRSPrintView #printMe").attr('disabled', false);
                        $("#" + ReportsSSRSPrintView.params["PanelID"] + " #ReportsSSRSPrintView #ReportPrintView").html(response.ReportsDetailsHTML);
                    } else {
                        if (response.Message != "" && response.Message != undefined) {
                            utility.DisplayMessages(response.Message, 2);
                        }
                        $("#" + ReportsSSRSPrintView.params["PanelID"] + " #ReportsSSRSPrintView #printMe").attr('disabled', true);
                        $("#" + ReportsSSRSPrintView.params["PanelID"] + " #ReportsSSRSPrintView #ReportPrintView").html("No Data Found!");
                    }
                } else {
                    if (response.Message != "") {
                        utility.DisplayMessages(response.Message, 2);
                    }
                }
            });
        }
    },

    /* Author: Muhammad Azhar Shahzad
    Date: 23/12/2015
    Report: Financial Report*/
    CreateReportPrintFinancial: function () {
        //get the ReportViewer Id

        var rv1 = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').contents().find('#RptViewer_ReportViewer');
        var iDoc = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').contents().find('#RptViewer_ReportViewer').parents('html');
        // Reading the report styles
        var styles = iDoc.find("head style[id$='ReportControl_styles']").html();
        if ((styles == undefined) || (styles == '')) {
            iDoc.find('head script').each(function () {
                var cnt = $(this).html();
                var p1 = cnt.indexOf('ReportStyles":"');
                if (p1 > 0) {
                    p1 += 15;
                    var p2 = cnt.indexOf('"', p1);
                    styles = cnt.substr(p1, p2 - p1);
                }
            });
        }
        if (styles == '') { alert("Cannot generate styles, Displaying without styles.."); }
        styles = '<style type="text/css">' + styles + "</style>";
        // Reading the report html
        var table = rv1.find("div[id$='_oReportDiv']");
        if (table == undefined) {
            alert("Report source not found.");
            return;
        }
        return table.parent().html();
        // Generating a copy of the report in a new window
        //var docType = '<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/loose.dtd">';
        //var docCnt = styles + table.parent().html();
        //if (docCnt != '<style type="text/css">undefined</style>undefined') {


        //    var docHead = '<head><style>body{margin:5;padding:0;}</style></head>';
        //    var winAttr = "location=yes, statusbar=no, directories=no, menubar=no, titlebar=no, toolbar=no, dependent=no, width=720, height=600, resizable=yes, screenX=200, screenY=200, personalbar=no, scrollbars=yes";;
        //    var newWin = window.open("", "_blank", winAttr);
        //    writeDoc = newWin.document;
        //    writeDoc.open();
        //    writeDoc.write(docType + '<html>' + docHead + '<body onload="window.print();">' + docCnt + '</body></html>');
        //    writeDoc.close();
        //    newWin.focus();
        //    // uncomment to autoclose the preview window when printing is confirmed or canceled.
        //    newWin.close();
        //}
    },

    PrintReport: function () {
        if (ReportsSSRSPrintView.params.PreviewPdf == true) {
            //if (EMRUtility.detectIE() == false) {
            $("#" + ReportsSSRSPrintView.params["PanelID"] + " #PreviewReportPrint")[0].contentWindow.focus();
            $("#" + ReportsSSRSPrintView.params["PanelID"] + " #PreviewReportPrint")[0].contentWindow.print();
            // } else {
            //       $("#" + ReportsSSRSPrintView.params["PanelID"] + " #PreviewReportPrint")[0].contentWindow.focus(); $('#PreviewReportPrint')[0].contentWindow.print();
            // }

        } else {
            $("#" + ReportsSSRSPrintView.params["PanelID"] + " #ReportsSSRSPrintView  #reportTable").removeClass("Of-a");
            $("#" + ReportsSSRSPrintView.params["PanelID"] + " #ReportsSSRSPrintView #printMe").hide();
            setTimeout(function () {
                var contents = $("#" + ReportsSSRSPrintView.params["PanelID"] + " #ReportsSSRSPrintView #ReportDetails")[0].outerHTML;

                var $s = $(contents);
                $s.find("#PrintPdfControlDiv").remove()
                contents = $s.html();
                var frame1 = $('<iframe />');
                frame1[0].name = ReportsSSRSDashboard.ReportName.toLowerCase().trim();
                frame1.attr("scrolling", "no");
                frame1.css({ "position": "absolute", "top": "-1000000px", "overflow": "hidden" });
                $("body").append(frame1);
                var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
                frameDoc.document.open();
                //Create a new HTML document.
                frameDoc.document.write('<html><head><title>' + ReportsSSRSDashboard.ReportName + ' | PrintReport</title>');
                frameDoc.document.write('</head><body>');
                //Append the external CSS file.
                frameDoc.document.write('<link rel="stylesheet" media="print" href="Content/Default/bootstrap.css" /><link rel="stylesheet" media="print" href="Content/Blue/theme.css" /><link rel="stylesheet" media="print" href="Content/Blue/theme-custom.css" /><link rel="stylesheet" media="print" href="Content/Blue/default.css" /><link rel="stylesheet" media="print" href="Content/Default/print-media.css" />');
                //Append the DIV contents.
                frameDoc.document.write(contents);
                frameDoc.document.write('</body></html>');
                frameDoc.document.close();
                // this code creating issue while printing.give different view on each time click.
                var ua = window.navigator.userAgent;
                var msie = ua.indexOf("MSIE ");
                if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {
                    var result = frameDoc.document.execCommand('print', false, null);
                    setTimeout(function () {
                        if (!result) {
                            window.frames[ReportsSSRSDashboard.ReportName.toLowerCase().trim()].focus();
                            window.frames[ReportsSSRSDashboard.ReportName.toLowerCase().trim()].print();
                            frame1.remove();
                            $("#" + ReportsSSRSPrintView.params["PanelID"] + " #ReportsSSRSPrintView  #reportTable").addClass("Of-a");
                            $("#" + ReportsSSRSPrintView.params["PanelID"] + " #ReportsSSRSPrintView #printMe").show();
                        } else {
                            frame1.remove();
                            $("#" + ReportsSSRSPrintView.params["PanelID"] + " #ReportsSSRSPrintView  #reportTable").addClass("Of-a");
                            $("#" + ReportsSSRSPrintView.params["PanelID"] + " #ReportsSSRSPrintView #printMe").show();
                        }
                    }, 200);
                } else {
                    setTimeout(function () {
                        window.frames[ReportsSSRSDashboard.ReportName.toLowerCase().trim()].focus();
                        window.frames[ReportsSSRSDashboard.ReportName.toLowerCase().trim()].print();
                        frame1.remove();
                        $("#" + ReportsSSRSPrintView.params["PanelID"] + " #ReportsSSRSPrintView  #reportTable").addClass("Of-a");
                        $("#" + ReportsSSRSPrintView.params["PanelID"] + " #ReportsSSRSPrintView #printMe").show();
                    }, 200);
                }
            }, 100);
        }
    },

    PrintParameters: function () {
        $("#dgvPrintParameter").dataTable().fnDestroy();
        $("#ReportsSSRSPrintView #PrintReportParameter #dgvPrintParameter tbody").find("tr").remove();
        //var QueryString = ReportsSSRSDashboard.CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters');
        var self;
        if (ReportsSSRSPrintView.params.ReportName.toLowerCase().trim() == "Orders".toLowerCase().trim()) {
            url = 'Controls/Reports/ReportViewer.aspx?reportpath=' + ReportsSSRSDashboard.OrdersReportURL(ReportsSSRSDashboard.ReportId);
            if ($('#ReportParamaters #ulOrderTabsItems').find('.active').attr('id') == "listLabOrder") {
                self = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #LabOrderDiv');
            } else if ($('#ReportParamaters #ulOrderTabsItems').find('.active').attr('id') == "listRadiologyOrder") {
                self = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #RadiologyOrder');
            } else if ($('#ReportParamaters #ulOrderTabsItems').find('.active').attr('id') == "listProcedureOrder") {
                self = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #ProcedureOrderDiv');
            } else if ($('#ReportParamaters #ulOrderTabsItems').find('.active').attr('id') == "listConsultationOrder") {
                self = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #ConsultationOrderDiv');
            } else if ($('#ReportParamaters #ulOrderTabsItems').find('.active').attr('id') == "listPrescriptionOrder") {
                self = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #PrescriptionOrderDiv');
            }
        }
        else if (ReportsSSRSPrintView.params.ReportName.toLowerCase().trim() == "Results".toLowerCase().trim()) {
            if ($('#ReportParamaters #ulRersultTabsItems').find('.active').attr('id') == "listLabResult") {
                self = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #LabResultDiv');
            } else if ($('#ReportParamaters #ulRersultTabsItems').find('.active').attr('id') == "listRadiologyResult") {
                self = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #RadiologyResult');
            } else if ($('#ReportParamaters #ulRersultTabsItems').find('.active').attr('id') == "listConsultationResult") {
                self = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #ConsultationResultDiv');
            }
        } else {
            self = $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters');
        }

        var ContolsKeys = "CPTCode,CPTCode,ICDCode,PlanType,ApplyToId,SystemCategoryId,LedgerAccountId,LedgerTypeId,CPTCodeFrom,CPTCodeTo,DOSStart,DOSEnd,AgingBucketsToDisplay,ReferalToId,ReferalFormId,RefProviderId,StartDate,EndDate,ProbDateFrom,ProbDateTo,ProbGivenDateFrom,ProbGivenDateTo,Is Administrated,Amended Note";
        var ContolsValues = "txtCPTCode,hfCPTCodeMulti,txtICD,ddlPlanCategory,ApplyTo,SystemCategory,LedgerAccount,LedgerType,CPTCodeFrom,CPTCodeTo,DOSStartDays,DOSEndDays,AgingBucketsToDisplay,hfReferalToId,hfReferalFromId,hfRefProvider,StartDate,EndDate,ProbDateFrom,ProbDateTo,ProbGivenDateFrom,ProbGivenDateTo,IsAdministatered,IsAmendedNote,txtCPTCodeProcedure,hfICD10Description,hfICD10Code";
        var ControlsDiv = "txtCPTCode,txtCPTCodeMulti,txtICD,ddlPlanCategory,ApplyTo,SystemCategory,LedgerAccount,LedgerType,CPTCodeFrom,CPTCodeTo,DOSStartDays,DOSEndDays,AgingBucketsToDisplay,txtReferalTo,txtReferalForm,txtRefProvider,StartDate,EndDate,ProbDateFrom,ProbDateTo,ProbGivenDateFrom,ProbGivenDateTo,IsAdministatered,chkAmendedNote,txtCPTCodeProcedure,hfICD10Description,hfICD10Code";
        var NameKeys = "CPT Code,CPT Code,ICD Code,Plan Type,Apply To,System Category,Ledger Account,Ledger Type,CPT Code From,CPT Code To,DOS Start,DOS End,Aging Buckets To Display,Referal To,Referal Form,Ref Provider,Start Date,End Date,Start Date,End Date,Problems From,Problems To,IsAdministatered,IsAmendedNote,Procedure,ICD10Description,ICD10Code";

        var kvpairs = [];
        var isMultiselected = true;
        var isValidDate = true;
        self.find('[type=hidden],[type=text],[type=number], textarea').each(function () {
            var CurrentID = this.id;

            if (CurrentID != "" && $.data($(this).get(0), 'datepicker') != null && $(this).val().length < 7 && $(this).val() != '') {
                if (($(this).hasClass("datepickerMonthViewEnd") || $(this).hasClass("datepickerMonthViewStart")) && $(this).val().length < 7) {
                    isValidDate = false;
                } else if ($(this).val().length < 10) {
                    isValidDate = false;
                }
            } else if (CurrentID != "") {
                var date_format = 'dd/mm/yyyy';
                //set default Date Formate
                if (globalAppdata['DateFormat'])
                    date_format = globalAppdata['DateFormat'];
                date_format = date_format.replace('yyyy', 'yy');
                var SelectedId = "";
                for (var i = 0; i < ControlsDiv.split(',').length; i++) {
                    if (ControlsDiv.split(',')[i] === CurrentID) {
                        SelectedId = NameKeys.split(',')[i]
                        break;
                    }
                }
                if (SelectedId != "") {
                    if ($.data($(this).get(0), 'datepicker') != null && $('#' + ReportsSSRSDashboard.params.PanelID + ' #' + ContolsValues.split(',')[i]).val() != '') {
                        //For datepicker up to month view, We need the last day of to date field so that we can good date
                        var selectedDate = $('#' + ReportsSSRSDashboard.params.PanelID + ' #' + ContolsValues.split(',')[i]).datepicker('getDate');
                        if ($(this).hasClass("datepickerMonthViewEnd")) {
                            var date = selectedDate, y = selectedDate.getFullYear(), m = selectedDate.getMonth();
                            selectedDate = new Date(y, m + 1, 0);
                        }
                        kvpairs.push((SelectedId) + "=" + ($.datepicker.formatDate(date_format, selectedDate)));
                    } else {
                        if (ContolsValues.split(',')[i].indexOf('hf') > -1) {
                            if (ContolsValues.split(',')[i] == 'hfReferalToId' || ContolsValues.split(',')[i] == 'hfReferalFromId' || ContolsValues.split(',')[i] == 'hfCPTCodeMulti' || ContolsValues.split(',')[i] == 'hfRefProvider') {

                                if (ContolsValues.split(',')[i] == 'hfRefProvider') {
                                    kvpairs.push(("Ref Provider") + "=" + ($('#' + ReportsSSRSDashboard.params.PanelID + ' #hfRefProviderName').text()));
                                }
                                else {
                                    kvpairs.push((SelectedId) + "=" + ($('#' + ReportsSSRSDashboard.params.PanelID + ' #' + ContolsValues.split(',')[i]).text()));
                                }
                            }
                            else {
                                kvpairs.push((SelectedId) + "=" + ($('#' + ReportsSSRSDashboard.params.PanelID + ' #' + ContolsValues.split(',')[i]).val()));
                            }
                        } else
                            kvpairs.push((SelectedId) + "=" + ($('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + ContolsValues.split(',')[i]).val()));
                    }

                } else {
                    if ($.data($(this).get(0), 'datepicker') != null && this.value != '') {
                        //For datepicker up to month view, We need the last day of to date field so that we can good date
                        var selectedDate = $(this).datepicker('getDate');
                        if ($(this).hasClass("datepickerMonthViewEnd")) {
                            var date = selectedDate, y = selectedDate.getFullYear(), m = selectedDate.getMonth();
                            selectedDate = new Date(y, m + 1, 0);
                        }
                        kvpairs.push(($(this).attr('customid')) + "=" + ($.datepicker.formatDate(date_format, selectedDate)));
                    } else {
                        //fix PMS-2265
                        //if (CurrentID == "ClaimDateFrom" && this.value == "" && ReportsSSRSPrintView.params.ReportName.toLowerCase().trim() != "beginning ar ending ar" && ReportsSSRSPrintView.params.ReportName.toLowerCase().trim() != "Claim Over Paid By Insurance".toLowerCase().trim() && ReportsSSRSPrintView.params.ReportName.toLowerCase().trim() != "Claim Under Paid By Insurance".toLowerCase().trim()) {
                        //    setTimeout(function () {
                        //        utility.DisplayMessages("Claim Date From is not selected", 2);
                        //    }, 500);
                        //    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').attr('src', null);
                        //    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').html(CurrentID + " is not selected");
                        //}
                        //fix PMS-2265
                        //if (CurrentID == "ClaimDateTo" && this.value == "" && ReportsSSRSPrintView.params.ReportName.toLowerCase().trim() != "beginning ar ending ar" && ReportsSSRSPrintView.params.ReportName.toLowerCase().trim() != "Claim Over Paid By Insurance".toLowerCase().trim() && ReportsSSRSPrintView.params.ReportName.toLowerCase().trim() != "Claim Under Paid By Insurance".toLowerCase().trim()) {
                        //    setTimeout(function () {
                        //        utility.DisplayMessages("Claim Date To is not selected", 2);
                        //    }, 500);
                        //    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').attr('src', null);
                        //    $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').html(CurrentID + " is not selected");
                        //}
                        if (CurrentID != "hdnProblemName" && CurrentID != "hfProviderOrder" && CurrentID != "hfClinicalProvider" && CurrentID != "hfFacilityTo"
                            && CurrentID != "LabTest" && CurrentID != "hfProviderResult" && CurrentID != "hfAssigneeProviderOrder" && CurrentID != "hfProcedureOrder" && CurrentID != "hfPharmacy") {
                            if (CurrentID == "txtPatBalFrom" || CurrentID == "txtPatBalTo") {
                                kvpairs.push(($(this).attr('customid')) + "=$" + (this.value));
                            } else {
                                kvpairs.push(($(this).attr('customid')) + "=" + (this.value));
                            }
                        }
                    }
                }
            }
        });
        self.find('[type=checkbox][id], [type=radio][id]').each(function () {
            var CurrentID = this.id;
            if (CurrentID != "" && CurrentID.indexOf("chkInclude") < 0 && CurrentID.indexOf("chkFields") < 0 && CurrentID.indexOf("IncludeInactive") < 0) {
                var SelectedId = "";
                for (var i = 0; i < ControlsDiv.split(',').length; i++) {
                    if (ControlsDiv.split(',')[i] === CurrentID) {
                        SelectedId = ContolsKeys.split(',')[i]
                        break;
                    }
                }
                if (SelectedId != "") {
                    kvpairs.push((SelectedId) + "=" + ((this.checked) ? true : false));
                } else {
                    if ($("#" + CurrentID).is(':visible')) {
                        kvpairs.push((this.id) + "=" + ((this.checked) ? true : false));
                    }
                }
            }
        });
        self.find('select').each(function (index, element) {

            var CurrentID = this.id;
            if (CurrentID != "") {
                var SelectedId = "";
                var SelectedNames = [];
                SelectedId = $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID).attr('customname');
                var SelectedClass = $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID).attr('customid');
                if (SelectedId != "" && SelectedId != undefined) {

                    var Selectedvalues;
                    //if ($('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters .' + SelectedClass + ' .multiselect-all').hasClass('active')) {
                    //    Selectedvalues = "all";
                    //}
                    //else {
                    //if (SelectedId == "ProviderId" || SelectedId == "AppointmentProviderId" || SelectedId == "ResourceProviderId" || SelectedId == "RenderingProviderId" || SelectedId == "FacilityId" || SelectedId == "PracticeId" || SelectedId == "RefProviderId" || SelectedId == "InsurancePlanId" || SelectedId == "ResourceId" || SelectedId == "SpecialtyId" || SelectedId == "PlanCategoryId" || SelectedId == "ProcedureCategoryId" || SelectedId == "PaymentTypeId" || SelectedId == "UserId" || SelectedId == "SecurityRoleId" || SelectedId == "EnteredBy") {
                    //    Selectedvalues = $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID + ' option:selected').map(function (a, item) { return item.value; }).get().join();
                    // }
                    // else {
                    if (SelectedId == "IsActive" || SelectedId == "FilterChargesBy") {
                        Selectedvalues = ($('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID).val() == 'undefined' || $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID).val() == "") ? "" : ($('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID).val() == "1" ? true : false);
                        if (SelectedId == "IsActive") {
                            SelectedNames = ($('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID).val() == 'undefined' || $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID).val() == "") ? "All" : ($('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID).val() == "1" ? "Active" : "In Active");
                        } else {
                            SelectedNames = ($('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID).val() == 'undefined' || $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID).val() == "") ? "All" : ($('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID).val() == "1" ? "Service Date" : "Claim Date");
                        }
                    }
                    else {
                        if (CurrentID == 'txtInsurancePlan') {
                            Selectedvalues = $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID + ' option[value!=self]:selected').map(function (a, item) { return item.value; }).get().join();
                        } else {
                            Selectedvalues = $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID).val();//$('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + SelectedId + ' option:selected').map(function (a, item) { return item.value; }).get().join();
                        }

                        if ($('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID).find('option:selected').length == $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID).find('option').length) {
                            SelectedNames = 'All';
                        } else {
                            $('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #' + CurrentID).find('option:selected').each(function () {
                                //if (SelectedId == "ProviderId" || SelectedId == "RenderingProviderId" || SelectedId == "ResourceProviderId" || SelectedId == "AppointmentProviderId") {
                                //SelectedNames.push("'" + $(this).text() + "'");
                                //} else {
                                SelectedNames.push(" " + $(this).text());
                                //}
                            })
                        }
                        if (Selectedvalues == undefined) {
                            Selectedvalues = "";
                        }
                        if (SelectedNames == "- Select -" || SelectedNames == " - Select - " || SelectedNames == " - Select -" || SelectedNames == "- Select - ") {
                            SelectedNames = "";
                        }
                        Selectedvalues = $.isArray(Selectedvalues) ? Selectedvalues.join() : Selectedvalues;
                        SelectedNames = $.isArray(SelectedNames) ? SelectedNames.join() : SelectedNames;
                    }
                    //}

                    if (Selectedvalues == "" && (SelectedId == "PracticeId" || SelectedId == "FacilityId") && $(this).hasClass("multiselect")) {
                        setTimeout(function () { utility.DisplayMessages(SelectedId.slice(0, -2) + " is not selected", 2); }, 500);
                        isMultiselected = false;
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').attr('src', null);
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').html(SelectedId.slice(0, -2) + " is not selected");
                    }
                    if (Selectedvalues == "" && (SelectedId == "ProviderId") && $(this).hasClass("multiselect") && (ReportsSSRSDashboard.ReportName != "Immunization")) {
                        setTimeout(function () { utility.DisplayMessages(SelectedId.slice(0, -2) + " is not selected", 2); }, 500);
                        isMultiselected = false;
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').attr('src', null);
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').html(SelectedId.slice(0, -2) + " is not selected");
                    } else if ($('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #AccountNumber').val() == "" && (Selectedvalues == "" && (SelectedId == "ProviderId") && $(this).hasClass("multiselect"))) {
                        setTimeout(function () {
                            utility.DisplayMessages("Provider or Account Number is not selected", 2);
                        }, 500);
                        isMultiselected = false;
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').attr('src', null);
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').html("Provider or Account Number is not selected");
                    }
                    if (Selectedvalues == "" && (SelectedId == "AgingBucketsToDisplay") && $(this).hasClass("multiselect")) {
                        setTimeout(function () { utility.DisplayMessages(SelectedId + " is not selected", 2); }, 500);
                        isMultiselected = false;
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').attr('src', null);
                        $('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportViewIframe').html(SelectedId + " is not selected");
                    } else {
                        //kvpairs.push((SelectedId) + "=" + (Selectedvalues));
                        if (SelectedNames.toLowerCase().trim() != "none" && Selectedvalues != "") {
                            if (SelectedId != "Quarter" && SelectedId != "Year") {
                                kvpairs.push((SelectedClass) + "=" + (SelectedNames));
                            } else if ($('#' + ReportsSSRSDashboard.params.PanelID + ' #ReportParamaters #QuarterlyReport').prop("checked") == true) {
                                kvpairs.push((SelectedClass) + "=" + (SelectedNames));
                            }                       
                        }
                    }
                }
            }
        });
        //var Parameters = QueryString.split('&');
        for (var i = 0; i < kvpairs.length; i++) {

            var EachPara = kvpairs[i].split('=');
            var $row = $('<tr/>');
            EachPara[1].replace("%2C", ",");
            if (EachPara[1] != "") {
                $row.append('<td VALIGN=TOP style="width: 120px"><b>' + EachPara[0] + '</b></td><td>' + EachPara[1] + '</td>');
                $("#ReportsSSRSPrintView #PrintReportParameter #dgvPrintParameter tbody").last().append($row);
            }
            //    console.log(Name);
            //    console.log(values);
        }
        if (ReportsSSRSPrintView.params.ReportName == "Incorrect Balance by Voided Claims") {
            $("#ReportsSSRSPrintView #PrintReportParameter #dgvPrintParameter").last().append("<td colspan='2'><b>Desc: Claims where the balances are off between Original and Voided Claims.</b></td>");
        }
        if ($("#ReportsSSRSPrintView #PrintReportParameter #dgvPrintParameter tbody tr").length == 0) {
            $("#ReportsSSRSPrintView #PrintReportParameter").hide();
        }
        else {
            if ($.fn.dataTable.isDataTable('#dgvAppointmentStatus'))
                ;
            else
                $("#ReportsSSRSPrintView #PrintReportParameter #dgvPrintParameter").DataTable({ "searching": false, "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": true, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] });
        }


    },

    GetReportBody: function (FormName) {
        if ((ReportsSSRSPrintView.params.ReportName) == "Aging Detail Analysis") {
            FormName = "Aging Detail Analysis";
            QueryString = ReportsViewer_Detail.params.QueryString;
        } else if (ReportsSSRSPrintView.params.ReportName == "Claim Status Dashboard") {
            FormName = "Claim Status Dashboard";
            QueryString = ReportsViewer_Detail.params.QueryString;
        }
        else if (ReportsSSRSPrintView.params.ReportName == "AR Reconciliation Report") {
            if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters  #ReportType').val() == "1") {
                FormName = ReportsSSRSDashboard.ReportName + " Detial";
            } else {
                FormName = ReportsSSRSDashboard.ReportName;
            }
            var QueryString = ReportsSSRSDashboard.CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters');
        }
        else if (ReportsSSRSPrintView.params.ReportName == "Appointments Vs Claim Detail") {
            FormName = ReportsSSRSPrintView.params.ReportName;
            var QueryString = ReportsSSRSDashboard.CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters');
        }
        else if (ReportsSSRSPrintView.params.ReportName == "Orders") {
            if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #listLabOrder').hasClass('active')) {
                FormName = 'Orders_Lab';
                QueryString = ReportsSSRSDashboard.CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #LabOrderDiv');
            } else if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #listRadiologyOrder').hasClass('active')) {
                FormName = 'Orders_Radiology';
                QueryString = ReportsSSRSDashboard.CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #RadiologyOrder');
            } else if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #listProcedureOrder').hasClass('active')) {
                FormName = 'Orders_Procedure';
                QueryString = ReportsSSRSDashboard.CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #ProcedureOrderDiv');
            } else if ($('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #listConsultationOrder').hasClass('active')) {
                FormName = 'Orders_Consultation';
                QueryString = ReportsSSRSDashboard.CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #ConsultationOrderDiv');
            }
            else {
                FormName = 'Orders_Prescription';
                QueryString = ReportsSSRSDashboard.CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #PrescriptionOrderDiv');
            }
        }
        else if (ReportsSSRSPrintView.params.ReportName == "Results") {
            if ($('#ReportParamaters #ulRersultTabsItems').find('.active').attr('id') == "listLabResult") {
                FormName = 'Results_Lab';
                QueryString = ReportsSSRSDashboard.CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #LabResultDiv');
            } else if ($('#ReportParamaters #ulRersultTabsItems').find('.active').attr('id') == "listRadiologyResult") {
                FormName = 'Results_Radiology';
                QueryString = ReportsSSRSDashboard.CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #RadiologyResult');
            } else if ($('#ReportParamaters #ulRersultTabsItems').find('.active').attr('id') == "listConsultationResult") {
                FormName = 'Results_Consultation';
                QueryString = ReportsSSRSDashboard.CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters #ConsultationResultDiv');
            }
        }
        else {
            if (FormName == null || FormName == "" || FormName == "undefined" || typeof FormName == "undefined") {
                FormName = ReportsSSRSDashboard.ReportName;
            }
            var QueryString = ReportsSSRSDashboard.CreateQuery('#' + ReportsSSRSDashboard.params["PanelID"] + ' #ReportParamaters');
        }

        if (QueryString) {
            var data = "ReportName=" + FormName.trim() + "&" + QueryString;
            // serach parameter , class name, command name of class 
            return MDVisionService.defaultService(data, "REPORTS_DASHBOARD", "GET_REPORTS_DETAILSHTML");
        }
    },

    /* Author: Muhammad Azhar Shahzad
   Date: 30/12/2015
   Report: Pdf Financial Report Preview*/
    PrintPdfReport: function (FormName) {
        if (ReportsSSRSPrintView.params.PrintPDFDataURL != null) {
            utility.PDFViewer(ReportsSSRSPrintView.params.PrintPDFDataURL, false, 'ReportsSSRSPrintView #PreviewReportPrint', true);
        } else {
            ReportsSSRSPrintView.PreviewPdfPrintReport_DbCall(FormName).done(function (response) {
                utility.PDFViewer(response.ReportPreview, false, 'ReportsSSRSPrintView #PreviewReportPrint', true);
            });
        }

    },
    /* Author: Muhammad Azhar Shahzad
   Date: 30/12/2015
   Report: Pdf Financial Report Preview*/
    PreviewPdfPrintReport_DbCall: function () {

        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(null, "REPORTS_DASHBOARD", "PREVIEW_REPORTS");
    },
    UnLoad: function (Tab) {
        UnloadActionPan(ReportsSSRSPrintView.params["ParentCtrl"]);
    },
};
