Bill_ERA_277CA = {
    params: [],

    Load: function (params) {
        Bill_ERA_277CA.params = params;
        //Bill_ERA_277CA.LoadReport_ERA_277CA();
    },

    //LoadReport_ERA_277CA: function () {

    //    Bill_ERA_277CA.Load_ReportERA277CA().done(function (response) {

    //        if (response.status != false) {

    //            var ArrayHeader = response.Report277CA_JSON.EDI277Header;
    //            var ArrayName = response.Report277CA_JSON.EDI277Names;
    //            var ArrayServiceLine = response.Report277CA_JSON.EDI277ServiceLine
    //            var ArrayStatus = response.Report277CA_JSON.EDI277Status;

    //            var HeaderRow = ArrayHeader[0];
    //            var TotalAcceptedCharges = 0.0;
    //            var TotalRejectedCharges = 0.0;
    //            var TotalCharges = TotalAcceptedCharges + TotalRejectedCharges;
    //            var TotalAcceptedCount = 0;
    //            var TotalRejectedCount = 0;
    //            var TotalCount = TotalAcceptedCount + TotalRejectedCount;
    //            var Group_NPI = [];

    //            //Names
    //            $.each(ArrayName, function (i, item) {

    //                //Billing Provider
    //                if (item.NM101_QUL === "85") {

    //                    var ProviderAcceptedCount = 0;
    //                    var ProviderRejectedCount = 0;
    //                    var ProviderTotalCount = ProviderAcceptedCount + ProviderRejectedCount;
    //                    var ProviderAcceptedCharges = 0.0;
    //                    var ProviderRejectedCharges = 0.0;
    //                    var ProviderTotalCharges = ProviderAcceptedCharges + ProviderRejectedCharges;

    //                    var ProviderName = item.NM103;
    //                    var NPI = item.NM109;
    //                    var AcceptedSTCRows = [];
    //                    var RejectedSTCRows = [];
    //                    var ServiceLineSTCRows = [];

    //                    //Find Patient Names against this Provider from same hash table with Parent Id = this ProviderId
    //                    $.each(ArrayName, function (i, Patient_item) {

    //                        if (Patient_item.ParentNameId == item.EDI277NameId) {
    //                            //Select Patient STC rows
    //                            $.each(ArrayStatus, function (i, Status_item) {

    //                                if (Status_item.EDI277NameId == Patient_item.EDI277NameId) {

    //                                    if (Status_item.STC03.toLowerCase() === "reject") {
    //                                        ProviderRejectedCount++;
    //                                        RejectedSTCRows.push({ ClaimNumber: Patient_item.TRN02, StatuItems: Status_item });
    //                                        ProviderRejectedCharges += Number(Status_item.STC04);

    //                                    }
    //                                    else if (Status_item.STC03.toLowerCase() === "accept") {
    //                                        ProviderAcceptedCount++;
    //                                        AcceptedSTCRows.push({ ClaimNumber: Patient_item.TRN02, StatuItems: Status_item });
    //                                        ProviderAcceptedCharges += Number(Status_item.STC04);
    //                                    }
    //                                }

    //                            });


    //                            //Select ServiceLine for STC rows
    //                            //$.each(ArrayServiceLine, function (i, Service_item) {
    //                            //});

    //                        }

    //                    });

    //                    //Provider Charges and Count
    //                    ProviderTotalCharges = ProviderAcceptedCharges + ProviderRejectedCharges;
    //                    ProviderTotalCount = ProviderAcceptedCount + ProviderRejectedCount;

    //                    //Report Total Count
    //                    TotalAcceptedCount += ProviderAcceptedCount;
    //                    TotalRejectedCount += ProviderRejectedCount;

    //                    //Report Total Charges
    //                    TotalAcceptedCharges += ProviderAcceptedCharges;
    //                    TotalRejectedCharges += ProviderRejectedCharges;

    //                    var IsAdded = false;
    //                    $.each(Group_NPI, function (i, NPI_item) {

    //                        if (NPI_item.NPI == NPI && IsAdded == false) {
    //                            var temp_ = NPI_item.Data;

    //                            temp_.push({
    //                                ProviderAcceptedCount: ProviderAcceptedCount,
    //                                ProviderRejectedCount: ProviderRejectedCount,
    //                                ProviderTotalCount: ProviderTotalCount,
    //                                ProviderAcceptedCharges: ProviderAcceptedCharges,
    //                                ProviderRejectedCharges: ProviderRejectedCharges,
    //                                ProviderTotalCharges: ProviderTotalCharges,
    //                                AcceptedSTCRows: AcceptedSTCRows,
    //                                RejectedSTCRows: RejectedSTCRows,
    //                            });

    //                            Group_NPI[NPI_item] = ({ ProviderName: NPI_item.ProviderName, NPI: NPI_item.NPI, Data: temp_ });
    //                            IsAdded = true;
    //                        }
    //                    });

    //                    if (!IsAdded) {
    //                        var tt_ = [];
    //                        tt_.push({
    //                            ProviderAcceptedCount: ProviderAcceptedCount,
    //                            ProviderRejectedCount: ProviderRejectedCount,
    //                            ProviderTotalCount: ProviderTotalCount,
    //                            ProviderAcceptedCharges: ProviderAcceptedCharges,
    //                            ProviderRejectedCharges: ProviderRejectedCharges,
    //                            ProviderTotalCharges: ProviderTotalCharges,
    //                            AcceptedSTCRows: AcceptedSTCRows,
    //                            RejectedSTCRows: RejectedSTCRows,
    //                        });

    //                        Group_NPI.push({ ProviderName: ProviderName, NPI: NPI, Data: tt_ });
    //                    }
    //                }


    //            });

    //            $.each(Group_NPI, function (i, NPI_item) {

    //                //Providers Data
    //                var ProviderTemplate = $($("#pnlReport_ERA_277CA ReportCA").html());
    //                $(ProviderTemplate).removeClass("hidden");

    //                var ProviderAcceptedCount = 0;
    //                var ProviderRejectedCount = 0;
    //                var ProviderTotalCount = 0;
    //                var ProviderAcceptedCharges = 0.0;
    //                var ProviderRejectedCharges = 0.0;
    //                var ProviderTotalCharges = 0.0;

    //                $.each(NPI_item.Data, function (i, NPI_Data_item) {

    //                    ProviderAcceptedCount += NPI_Data_item.ProviderAcceptedCount;
    //                    ProviderRejectedCount += NPI_Data_item.ProviderRejectedCount;
    //                    ProviderAcceptedCharges += NPI_Data_item.ProviderAcceptedCharges;
    //                    ProviderRejectedCharges += NPI_Data_item.ProviderRejectedCharges;

    //                    //Fill Accepted Grid
    //                    Bill_ERA_277CA.LoadSTCGrod(NPI_Data_item.AcceptedSTCRows, $(ProviderTemplate).find(".tblAcceptedSTC"));
    //                    //Fill Rejected Grid
    //                    Bill_ERA_277CA.LoadSTCGrod(NPI_Data_item.RejectedSTCRows, $(ProviderTemplate).find(".tblRejectedSTC"));

    //                });

    //                ProviderTotalCharges = ProviderAcceptedCharges + ProviderRejectedCharges;
    //                ProviderTotalCount = ProviderAcceptedCount + ProviderRejectedCount;

    //                //Fill Privider Detail
    //                $(ProviderTemplate).find("#tdAcceptedCount").html(ProviderAcceptedCount);
    //                $(ProviderTemplate).find("#tdRejectedCount").html(ProviderRejectedCount);
    //                $(ProviderTemplate).find("#tdTotalCount").html(ProviderTotalCount);

    //                $(ProviderTemplate).find("#tdAcceptedCharges").html(utility.convertToFigure(ProviderAcceptedCharges));
    //                $(ProviderTemplate).find("#tdRejectedCharges").html(utility.convertToFigure(ProviderRejectedCharges));
    //                $(ProviderTemplate).find("#tdTotalCharges").html(utility.convertToFigure(ProviderTotalCharges));

    //                $(ProviderTemplate).find("#spNPI").html(NPI_item.NPI);
    //                $(ProviderTemplate).find("#spProviderName").html(NPI_item.ProviderName);

    //                //append into report body
    //                $("#pnlReport_ERA_277CA #rpt277body").append(ProviderTemplate);


    //            });


    //            //Total Charges of all providers
    //            TotalCharges = TotalAcceptedCharges + TotalRejectedCharges;
    //            TotalCount = TotalAcceptedCount + TotalRejectedCount;

    //            var ReportHeader = $("#pnlReport_ERA_277CA #rpt277header");
    //            //Fill Report Header
    //            $(ReportHeader).find("#tdReportAcceptedCount").html(TotalAcceptedCount);
    //            $(ReportHeader).find("#tdReportRejectedCount").html(TotalRejectedCount);
    //            $(ReportHeader).find("#tdReportTotalCount").html(TotalCount);

    //            $(ReportHeader).find("#tdReportAcceptedCharges").html(utility.convertToFigure(TotalAcceptedCharges));
    //            $(ReportHeader).find("#tdReportRejectedCharges").html(utility.convertToFigure(TotalRejectedCharges));
    //            $(ReportHeader).find("#tdReportTotalCharges").html(utility.convertToFigure(TotalCharges));



    //            $(ReportHeader).find("#spSubmitterId").html(HeaderRow.SubmitterID);
    //            $(ReportHeader).find("#spSubmitterName").html(HeaderRow.SubmitterName);
    //            $(ReportHeader).find("#spFileControlNumber").html(HeaderRow.ControlNumber);
    //            $(ReportHeader).find("#spReportDate").html(HeaderRow.ReportDate);
    //            $(ReportHeader).find("#spTestPro").html(HeaderRow.TorP);
    //            $(ReportHeader).find("#spReceiverName").html(HeaderRow.ReceiverName);

    //            $("#pnlReport_ERA_277CA ReportCA").html("");
    //            $('[data-plugin-toggle]').each(function () {
    //                var $this = $(this),
    //                    opts = {};

    //                var pluginOptions = $this.data('plugin-options');
    //                if (pluginOptions)
    //                    opts = pluginOptions;

    //                $this.themePluginToggle(opts);
    //            });
    //        }
    //        else
    //            utility.DisplayMessages(response.Message, 2);

    //    });

    //},

    //LoadSTCGrod: function (JsonData, Control) {


    //    //if (JsonData.length > 0) {

    //    var lastClaimnumber = "";
    //    $.each(JsonData, function (i, item) {
    //        var $row = $('<tr/>');
    //        $row.attr("onclick", "utility.SelectGridRow($(this))");

    //        var $row = $('<tr/>');
    //        $row.attr("onclick", "utility.SelectGridRow($(this))");
    //        var ClaimNumber = item.ClaimNumber;
    //        if (item.StatuItems.TRN02)
    //            ClaimNumber = item.StatuItems.TRN02

    //        //if (ClaimNumber == lastClaimnumber) {
    //        //    $lastrow = $(Control).find("tbody tr").last();
    //        //    $($lastrow).find("td").eq(1).append(utility.convertToFigure(item.StatuItems.STC04, true));
    //        //    $($lastrow).find("td").eq(2).append(utility.convertToFigure(item.StatuItems.STC05, true));
    //        //    $($lastrow).find("td").eq(3).append(item.StatuItems.STC01_3_QUL + ": " + item.StatuItems.STC01_3);
    //        //    $($lastrow).find("td").eq(4).append(item.StatuItems.STC01_1_QUL + ": " + item.StatuItems.STC01_1);
    //        //    $($lastrow).find("td").eq(5).append(item.StatuItems.STC01_2_QUL + ": " + item.StatuItems.STC01_2);
    //        //}
    //        //else {
    //            lastClaimnumber = ClaimNumber
    //            $row.append(
    //              '<td>' + ClaimNumber + '</td>'
    //            + '<td>' + utility.convertToFigure(item.StatuItems.STC04, true) + '</td>' //Claim Charge Amount
    //            + '<td>' + utility.convertToFigure(item.StatuItems.STC05, true) + '</td>' //Claim Payment Amount
    //            + '<td>' + item.StatuItems.STC01_3_QUL + ": " + item.StatuItems.STC01_3 + '</td>'
    //            + '<td>' + item.StatuItems.STC01_1_QUL + ": " + item.StatuItems.STC01_1 + '</td>'
    //            + '<td>' + item.StatuItems.STC01_2_QUL + ": " + item.StatuItems.STC01_2 + '</td>'
    //            );

    //            $(Control).find("tbody").last().append($row);
    //       // }



    //    });

    //    //}
    //    //else {

    //    //    var $row = $('<tr/>');
    //    //    $row.append('<td colspan="6" style="text-align: center;" >No Record Found.</td>');
    //    //    $(Control).find("tbody").last().append($row);
    //    //}



    //},

    //UnLoad: function () {

    //    if (Bill_ERA_277CA.params != null && Bill_ERA_277CA.params.ParentCtrl) {
    //        UnloadActionPan(Bill_ERA_277CA.params.ParentCtrl);
    //    }
    //    else
    //        UnloadActionPan();

    //},


    ////------------Server  Calls----------------\\

    //Load_ReportERA277CA: function () {

    //    var data = "ReportId";
    //    // serach parameter , class name, command name of class 
    //    return MDVisionService.defaultService(data, "BILLING_ERA", "LOAD_RERPORT_ERA_277CA");

    //}



};