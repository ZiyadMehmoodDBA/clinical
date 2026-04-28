EDIReviewReport = {
    params: [],
    bIsFirstLoad: true,
    Load: function (params) {
        EDIReviewReport.params = params;
        if (EDIReviewReport.bIsFirstLoad) {
            EDIReviewReport.bIsFirstLoad = false;
            var self = $('#EDIReviewReport');
            self.loadDropDowns(true).done(function () {

                if (EDIReviewReport.params["PanelID"] == 'EDIBatchDetail')
                    EDIReviewReport.Fill_EDIBatchReport();
                else
                    EDIReviewReport.Fill_EDIReport();

                $("#EDIReviewReport #txtBatchNo").prop('disabled', true);
                $("#EDIReviewReport #ddlClearinghouse").prop('disabled', true);
            });
            EDIReviewReport.ValidationReviewReport();
        }

    },



    Fill_EDIReport: function () {

        $("#EDIReviewReport #batchNumber_div").css("display", "none");

        var self = $("#EDIReviewReport #frmEDIReviewReport");
        var myJSON = self.getMyJSONByName();


        EDIReviewReport.FillEDIReport().done(function (response) {
            if (response.status != false) {

                var edi_fileData = JSON.parse(response.EDI_FileDataJSON);

                if (edi_fileData.ReportType == "277") {

                    utility.bindMyJSONByName(true, edi_fileData, false, self).done(function () {
                    });
                    EDIReviewReport.LoadReport_ERA_277CA(response, $(self.find("#txtHTMLView").parent().first()));

                }
                else {
                    //set EDI in HTML View
                    self.find("#txtHTMLView").html(edi_fileData.HtmlView);
                    self.find("#btnPrintClaim").removeClass("disableAll");
                }


                //HighLightText
                var Sourcetext = $("#frmBillEDIReport #EDIText").val();
                if (Sourcetext != "") {
                    utility.HighLightText("frmEDIReviewReport #txtHTMLView", Sourcetext);
                }
                utility.bindMyJSONByName(true, edi_fileData, false, self).done(function () {
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });




    },

    Fill_EDIBatchReport: function () {

        var self = $("#EDIReviewReport #frmEDIReviewReport");
        var myJSON = self.getMyJSONByName();
        EDIReviewReport.FillEDIBatchReport().done(function (response) {
            if (response.status != false) {
                var edi_fileData = JSON.parse(response.EDI_FileDataJSON);
                //set EDI in HTML View
                self.find("#txtHTMLView").html(edi_fileData.HtmlView);
                self.find("#btnPrintClaim").removeClass("disableAll");
                utility.bindMyJSONByName(true, edi_fileData, false, self).done(function () {

                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });

    },

    ValidationReviewReport: function () {
        $('#frmEDIReviewReport')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  Comments: {
                      group: '.col-sm-6',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           EDIReviewReport.Update_Report();
       });
    },


    LoadReport_ERA_277CA: function (response, reportContainer) {


        var ajax_get = $.get("./Controls/Billing/ERA/Bill_ERA_277CA.html", {}, function (content) {
            var $html_control = $(content);

            var ArrayHeader = response.Report277CA_JSON.EDI277Header;
            var ArrayName = response.Report277CA_JSON.EDI277Names;
            var ArrayServiceLine = response.Report277CA_JSON.EDI277ServiceLine
            var ArrayStatus = response.Report277CA_JSON.EDI277Status;
            var ArrayStatics = response.Report277CA_JSON.EDI277Statics;

            var ReportStaticsRow = null;
            $.each(ArrayStatics, function (i, Statics_item) {
                if (Statics_item.Name == "41")
                    ReportStaticsRow = Statics_item;
            });

            var HeaderRow = ArrayHeader[0];
            var Group_NPI = [];

            //Names
            $.each(ArrayName, function (i, item) {

                //Billing Provider
                if (item.NM101_QUL === "85") {
                    var ProviderName = item.NM103;
                    var NPI = item.NM109;
                    var AcceptedSTCRows = [];
                    var RejectedSTCRows = [];
                    var ServiceLineSTCRows = [];

                    //Find Patient Names against this Provider from same hash table with Parent Id = this ProviderId
                    $.each(ArrayName, function (i, Patient_item) {

                        if (Patient_item.ParentNameId == item.EDI277NameId) {
                            //Select Patient STC rows
                            $.each(ArrayStatus, function (i, Status_item) {

                                if (Status_item.EDI277NameId == Patient_item.EDI277NameId) {

                                    if (Status_item.STC03.toLowerCase() === "reject") {
                                        RejectedSTCRows.push({ ClaimNumber: Patient_item.TRN02, StatuItems: Status_item });

                                    }
                                    else if (Status_item.STC03.toLowerCase() === "accept") {
                                        AcceptedSTCRows.push({ ClaimNumber: Patient_item.TRN02, StatuItems: Status_item });
                                    }
                                }

                            });


                        }

                    });

                    var IsAdded = false;
                    $.each(Group_NPI, function (i, NPI_item) {

                        if (NPI_item.NPI == NPI && IsAdded == false) {
                            var temp_ = NPI_item.Data;

                            temp_.push({
                                AcceptedSTCRows: AcceptedSTCRows,
                                RejectedSTCRows: RejectedSTCRows,
                            });

                            Group_NPI[NPI_item] = ({ ProviderName: NPI_item.ProviderName, NPI: NPI_item.NPI, Data: temp_ });
                            IsAdded = true;
                        }
                    });

                    if (!IsAdded) {
                        var tt_ = [];
                        tt_.push({
                            AcceptedSTCRows: AcceptedSTCRows,
                            RejectedSTCRows: RejectedSTCRows,
                        });

                        Group_NPI.push({ ProviderName: ProviderName, NPI: NPI, Data: tt_ });
                    }
                }


            });

            $.each(Group_NPI, function (i, NPI_item) {

                //Providers Data
                var ProviderTemplate = $($html_control.find("ReportCA").html());
                $(ProviderTemplate).removeClass("hidden");
                $.each(NPI_item.Data, function (i, NPI_Data_item) {

                    //Fill Accepted Grid
                    EDIReviewReport.LoadSTCGrod(NPI_Data_item.AcceptedSTCRows, $(ProviderTemplate).find(".tblAcceptedSTC"));
                    //Fill Rejected Grid
                    EDIReviewReport.LoadSTCGrod(NPI_Data_item.RejectedSTCRows, $(ProviderTemplate).find(".tblRejectedSTC"));

                });

                var ProviderStaticsRow = null;
                $.each(ArrayStatics, function (i, Statics_item) {
                    if (Statics_item.Name == "85" && Statics_item.NPI == NPI_item.NPI)
                        ProviderStaticsRow = Statics_item;
                });

                if (ProviderStaticsRow != null) {
                    //Fill Privider Detail
                    $(ProviderTemplate).find("#tdAcceptedCount").html(Number(ProviderStaticsRow.AcceptedCount));
                    $(ProviderTemplate).find("#tdRejectedCount").html(Number(ProviderStaticsRow.RejectedCount));
                    $(ProviderTemplate).find("#tdTotalCount").html(Number(ProviderStaticsRow.AcceptedCount + ProviderStaticsRow.RejectedCount));

                    $(ProviderTemplate).find("#tdAcceptedCharges").html(utility.convertToFigure(ProviderStaticsRow.AcceptedCharges, true));
                    $(ProviderTemplate).find("#tdRejectedCharges").html(utility.convertToFigure(ProviderStaticsRow.RejectedCharges, true));
                    $(ProviderTemplate).find("#tdTotalCharges").html(utility.convertToFigure(ProviderStaticsRow.AcceptedCharges + ProviderStaticsRow.RejectedCharges, true));

                    $(ProviderTemplate).find("#spNPI").html(NPI_item.NPI);
                    $(ProviderTemplate).find("#spProviderName").html(NPI_item.ProviderName);
                }

                //append into report body
                $($html_control).find("#rpt277body").append(ProviderTemplate);


            });


            var ReportHeader = $($html_control).find("#rpt277header");

            if (ReportStaticsRow != null) {
                //Fill Report Header
                $(ReportHeader).find("#tdReportAcceptedCount").html(Number(ReportStaticsRow.AcceptedCount));
                $(ReportHeader).find("#tdReportRejectedCount").html(Number(ReportStaticsRow.RejectedCount));
                $(ReportHeader).find("#tdReportTotalCount").html(Number(ReportStaticsRow.AcceptedCount + ReportStaticsRow.RejectedCount));

                $(ReportHeader).find("#tdReportAcceptedCharges").html(utility.convertToFigure(ReportStaticsRow.AcceptedCharges, true));
                $(ReportHeader).find("#tdReportRejectedCharges").html(utility.convertToFigure(ReportStaticsRow.RejectedCharges, true));
                $(ReportHeader).find("#tdReportTotalCharges").html(utility.convertToFigure(ReportStaticsRow.AcceptedCharges + ReportStaticsRow.RejectedCharges, true));
            }




            $(ReportHeader).find("#spSubmitterId").html(HeaderRow.SubmitterID);
            $(ReportHeader).find("#spSubmitterName").html(HeaderRow.SubmitterName);
            $(ReportHeader).find("#spFileControlNumber").html("<a href='#' onclick=EDIReviewReport.View_BatchDetail('" + HeaderRow.ControlNumber + "')>" + HeaderRow.ControlNumber + "</a>");
            $(ReportHeader).find("#spReportDate").html(HeaderRow.ReportDate);
            $(ReportHeader).find("#spTestPro").html(HeaderRow.TorP);
            $(ReportHeader).find("#spReceiverName").html(HeaderRow.ReceiverName);

            //$("#pnlReport_ERA_277CA ReportCA").html("");
            setTimeout(function () {

                $('[data-plugin-toggle]').each(function () {
                    var $this = $(this),
                        opts = {};

                    var pluginOptions = $this.data('plugin-options');
                    if (pluginOptions)
                        opts = pluginOptions;

                    $this.themePluginToggle(opts);
                });

            }, 500);

            $(reportContainer).html($html_control.find("form"));

        }, "html");


    },

    LoadSTCGrod: function (JsonData, Control) {
        //if (JsonData.length > 0) {
        var lastClaimnumber = "";
        $.each(JsonData, function (i, item) {
            var $row = $('<tr/>');
            $row.attr("onclick", "utility.SelectGridRow($(this))");
            if (item.ClaimNumber == null)
                item.ClaimNumber = "";

            var ClaimNumber = item.ClaimNumber;
            if (item.StatuItems.TRN02 != null)
                ClaimNumber = item.StatuItems.TRN02
            var Entity = "";
            if (item.StatuItems.STC01_3_QUL != null && item.StatuItems.STC01_3 != null)
                Entity = item.StatuItems.STC01_3_QUL + ": " + item.StatuItems.STC01_3;

            //Start PRD-652 TahreeMalik     get rejection reaason from A3, A4, A6, A7, A8 segments
            var rejectionReason = "";
            if (item.StatuItems.STC01_1_QUL == 'A3' || item.StatuItems.STC01_1_QUL == 'A4' || item.StatuItems.STC01_1_QUL == 'A6' || item.StatuItems.STC01_1_QUL == 'A7' || item.StatuItems.STC01_1_QUL == 'A8') {
                rejectionReason = '<td>' + (utility.IsNullOrEmptyString(item.StatuItems.STC12) ? "" : item.StatuItems.STC12) + '</td>';
            }
            else
                rejectionReason = "<td></td>";
            //End PRD-652 TahreeMalik

            $row.append(
              '<td> <a href="#" onclick=EDIReviewReport.LoadPatientVisit("'+ClaimNumber+'")>' + ClaimNumber + '</a></td>'
            + '<td>' + utility.convertToFigure(item.StatuItems.STC04, true) + '</td>' //Claim Charge Amount
            + '<td>' + utility.convertToFigure(item.StatuItems.STC05, true) + '</td>' //Claim Payment Amount
            + '<td>' + Entity + '</td>'
            + '<td>' + item.StatuItems.STC01_1_QUL + ": " + item.StatuItems.STC01_1 + '</td>'
            + '<td>' + item.StatuItems.STC01_2_QUL + ": " + item.StatuItems.STC01_2 + '</td>'
            + ($(Control).hasClass('tblRejectedSTC') ? rejectionReason : "")
            );

            $(Control).find("tbody").last().append($row);
        });

        //}
        //else {

        //    var $row = $('<tr/>');
        //    $row.append('<td colspan="6" style="text-align: center;" >No Record Found.</td>');
        //    $(Control).find("tbody").last().append($row);
        //}
    },

    ERA_Print: function () {

        var self = $("#EDIReviewReport #frmEDIReviewReport");
        //self.find("#btnPrintClaim").hide();
        var ReportName = "ERA Report";
        setTimeout(function () {
            var contents = self.find("#tbHtmlView").html();
            var frame1 = $('<iframe />');
            frame1[0].name = ReportName.toLowerCase().trim();
            frame1.attr("scrolling", "no");
            frame1.css({ "position": "absolute", "top": "-1000000px", "overflow": "hidden" });
            $("body").append(frame1);
            var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
            frameDoc.document.open();
            //Create a new HTML document.
            frameDoc.document.write('<html><head><title>' + ReportName + ' | PrintReport</title>');
            frameDoc.document.write('</head><body>');
            //Append the external CSS file.
            frameDoc.document.write('<link rel="stylesheet" media="print" href="Content/Blue/bootstrap.css" /> <link rel="stylesheet" media="print" href="Content/Blue/theme.css" /><link rel="stylesheet" media="print" href="Content/Blue/theme-custom.css" /><link rel="stylesheet" media="print" href="Content/Blue/default.css" />');
            //Append the DIV contents.
            frameDoc.document.write(contents);
            frameDoc.document.write('</body></html>');
            frameDoc.document.close();

            setTimeout(function () {
                if (utility.UserBrowser() == "IE") {
                    var $temp = $(window.frames[ReportName.toLowerCase().trim()].document.getElementsByTagName('pre')).get(0);
                    $($temp).css("padding-left", "25px");
                    window.frames[ReportName.toLowerCase().trim()].document.execCommand('print', false, null);
                    frame1.remove();
                }
                else {
                    window.frames[ReportName.toLowerCase().trim()].focus();
                    window.frames[ReportName.toLowerCase().trim()].print();
                    frame1.remove();
                }
            }, 200);

        }, 100);
    },


    Update_Report: function () {
        var strMessage = "";
        var self = $("#EDIReviewReport");
        var myJSON = self.getMyJSONByName();
        EDIReviewReport.UpdateReport(EDIReviewReport.params.EDIReportId, myJSON).done(function (response) {
            if (response.status != false) {
                utility.DisplayMessages(response.message, 1);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });
    },

    FillEDIReport: function () {
        var CheckNo = "";
        if (EDIReviewReport.params.CheckNo)
            CheckNo = EDIReviewReport.params.CheckNo;

        //var data = "EDIReportId=" + EDIReviewReport.params.EDIReportId + "&CheckNo=" + CheckNo;
        //// serach parameter , class name, command name of class 
        //return MDVisionService.defaultService(data, "BILLING_EDI_REVIEW_DETAIL", "FILL_EDI_REPORT");
        var objData = new JSON.constructor();
        objData["CheckNumber"] = CheckNo;
        objData["EDIReportId"] = EDIReviewReport.params.EDIReportId;
        objData["CommandType"] = "fill_edi_report";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "EDIReviewReport", "EDIReviewReport");
    },


    FillEDIBatchReport: function () {
        //var data = "EDIReportId=" + EDIReviewReport.params.EDIReportId + "&EDIBatchNumber=" + EDIReviewReport.params.EDIBatchNumber;
        //// serach parameter , class name, command name of class 
        //return MDVisionService.defaultService(data, "BILLING_EDI_REVIEW_DETAIL", "FILL_EDI_BATCH_REPORT");
        var objData = new JSON.constructor();
        objData["EDIBatchNumber"] = EDIReviewReport.params.EDIBatchNumber;
        objData["EDIReportID"] = EDIReviewReport.params.EDIReportId;
        objData["CommandType"] = "fill_edi_batch_report";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "EDIReviewReport", "EDIReviewReport");
    },

    UpdateReport: function (EDIReportId, Jdata) {
        //var data = "EDIReportId=" + EDIReportId + "&ReportReviewData=" + data;
        //// serach parameter , class name, command name of class 
        //return MDVisionService.defaultService(data, "BILLING_EDI_REVIEW_DETAIL", "UPDATE_REPORT");

        var objData = new JSON.constructor();
        if (Jdata)
            objData = JSON.parse(Jdata);

        objData["EDIReportID"] = EDIReportId;
        objData["CommandType"] = "update_report";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "EDIReviewReport", "EDIReviewReport");
    },

    UnLoad: function () {
        if (EDIReviewReport.params != null && EDIReviewReport.params.ParentCtrl != null) {
            UnloadActionPan(EDIReviewReport.params.ParentCtrl, "EDIReviewReport");
        }
        else
            UnloadActionPan(null, 'EDIReviewReport');
    },
    LoadPatientVisit: function (claimNumber) {
        console.log(claimNumber);
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Charges", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                EDIReviewReport.SearchCharge(claimNumber, 1, 15).done(function (response) {
                    if (response.BillChargeCount > 0) {
                        var bill_Charge_Load = JSON.parse(response.BillChargeLoad_JSON);
                        var params = [];
                        params["FromAdmin"] = 0;
                        params["ParentCtrl"] = 'EDIReviewReport';
                        params["VisitId"] = bill_Charge_Load[0].VisitId;
                        params["patientID"] = bill_Charge_Load[0].PatientId;
                        params["PanelID"] = EDIReviewReport.params.PanelID;
                        LoadActionPan('EncounterChargeCapture', params);
                    }
                });
              
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    SearchCharge: function (claimNumber, PageNumber, RowsPerPage) {

        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 15 : RowsPerPage;

        var objData = {
            PageNumber: PageNumber,
            CommandType: "Search",
            ClaimNumber: claimNumber,
            RowsPerPage: RowsPerPage
        };
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Charges", "Charge");

    },
    
    View_BatchDetail(BatchNo) {
        EDIReviewReport.GetBatchControlDetail(BatchNo).done(function (response) {
            if (response.status == true) {
                var myJSON = JSON.parse(response.EDIReport_JSON)[0];
                var params = [];
                params["_837BatchId"] = myJSON.BatchId;
                params["BatchControlNo"] = myJSON.BatchControlNo;
                params["EntityId"] = myJSON.EntityId;
                params["ParentCtrl"] = "EDIReviewReport";
                LoadActionPan('EDIBatchDetail', params);
            }
            else {
                utility.DisplayMessages(response.Message, 2);
            }
        });
    },
    GetBatchControlDetail: function (BatchNo) {
        var objData = new JSON.constructor();
        
        objData["BatchNumber"] = BatchNo;
        objData["CommandType"] = "batch_control_detail";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "EDIReviewReport", "EDIReviewReport");
    },
}