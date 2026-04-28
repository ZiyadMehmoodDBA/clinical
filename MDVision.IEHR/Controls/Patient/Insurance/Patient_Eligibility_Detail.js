Patient_Eligibility_Detail = {
    params: [],

    Load: function (params) {
        Patient_Eligibility_Detail.params = params;

        if (Patient_Eligibility_Detail.params.EDIEligibilityData && Patient_Eligibility_Detail.params.EDIEligibilityId == -1) {
            Patient_Eligibility_Detail.BindEligibilityReport(Patient_Eligibility_Detail.params.EDIEligibilityData);
        }
        else {
            Patient_Eligibility_Detail.LoadEDIEligibilityDetail();
        }

    },

    LoadEDIEligibilityDetail: function () {
        Patient_Eligibility_Detail.Load_EDIEligibilityDetail().done(function (response) {
            if (response.status != false) {
                Patient_Eligibility_Detail.BindEligibilityReport(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    BindEligibilityReport: function (JsonData) {

        //Lists
        var EligibilityBatch_list = JSON.parse(JsonData.EligibilityBatch_JSON);
        var Header_list = JSON.parse(JsonData.EligibilityHeader_JSON);
        var Names_list = JSON.parse(JsonData.EligibilityNames_JSON);
        var Benifits_list = JSON.parse(JsonData.EligibilityBenifits_JSON);
        var Benifits_arraylist = JSON.parse(JsonData.EligibilityBenifitsArray_JSON);
        var BenifitsDetail_list = JSON.parse(JsonData.EligibilityBenifitsDetail_JSON);
        var ServiceTypeCode = JsonData.ServiceTypeCode;


        //Rows
        var Patientrow = "";
        var Batchrow = "";
        var Headerrow = "";
        var Payerrow = "";
        var Providerrow = "";
        var PCProw = "";
        var Subscriberrow = "";
        var Dependentrow = "";

        //Variables
        var Payerstatus = "";
        var Providerstatus = "";
        var Subscriberstatus = "";
        var Dependentstatus = "";
        var ErrorsCount = 0;


        //-----------------------Initialize rows and variables starts-----------------------------

        //Provider: 1P
        //Payer: PR
        //PCP: P3
        //Subscriber: IL
        //Dependent: 03

        //Get Rows
        $.each(Names_list, function (i, item) {

            if (item.NM101Code == "P3") {
                PCProw = item;
            }
            else if (item.NM101Code == "PR") {
                Payerrow = item;
            }
            else if (item.NM101Code == "1P") {
                Providerrow = item;
            }
            else if (item.NM101Code == "IL") {
                Subscriberrow = item;
            }
            else if (item.NM101Code == "03") {
                Dependentrow = item;
            }

        });

        //Batch Row
        $.each(EligibilityBatch_list, function (i, item) {
            Batchrow = item;
        });

        //Header Row
        $.each(Header_list, function (i, item) {
            Headerrow = item;
        });

        //Patient Row
        if (Dependentrow != "")
            Patientrow = Dependentrow;
        else if (Subscriberrow != "")
            Patientrow = Subscriberrow;

        if (Payerrow != "") {
            //Payer Status
            if (Payerrow.AAA01.toLowerCase() == "y")
                Payerstatus = "Yes";
            else if (Payerrow.AAA01.toLowerCase() == "n")
                Payerstatus = "No";

            if (Payerstatus == "No") {
                ErrorsCount++;
                Patient_Eligibility_Detail.AddErrorSummary("Payer", Payerstatus, Payerrow.AAA03, Payerrow.AAA04);
            }

        }

        if (Providerrow != "") {

            //Provider Status
            if (Providerrow.AAA01.toLowerCase() == "y")
                Providerstatus = "Yes";
            else if (Providerrow.AAA01.toLowerCase() == "n")
                Providerstatus = "No";

            if (Providerstatus == "No") {
                ErrorsCount++;
                Patient_Eligibility_Detail.AddErrorSummary("Receiver", Providerstatus, Providerrow.AAA03, Providerrow.AAA04);
            }

        }

        if (Subscriberrow != "") {
            //Subscriber Status
            if (Subscriberrow.AAA01.toLowerCase() == "y")
                Subscriberstatus = "Valid";
            else if (Subscriberrow.AAA01.toLowerCase() == "n")
                Subscriberstatus = "Invalid";

            if (Subscriberstatus == "Invalid") {
                ErrorsCount++;
                Patient_Eligibility_Detail.AddErrorSummary("Subscriber", Subscriberstatus, Subscriberrow.AAA03, Subscriberrow.AAA04);
            }

        }


        if (Dependentrow != "") {
            //Dependent Status
            if (Dependentrow.AAA01.toLowerCase() == "y")
                Dependentstatus = "Valid";
            else if (Dependentrow.AAA01.toLowerCase() == "n")
                Dependentstatus = "Invalid";

            if (Dependentstatus == "Invalid") {
                ErrorsCount++;
                Patient_Eligibility_Detail.AddErrorSummary("Dependent", Dependentstatus, Dependentrow.AAA03, Dependentrow.AAA04);
            }

        }

        if ($("#pnlPatientEligibilityDetail #lblErrorSummaryList div").length) {
            $("#pnlPatientEligibilityDetail #ErrorSummaryHeading").html("Error Summary (Errors=" + ErrorsCount + ")");
            $("#pnlPatientEligibilityDetail #ErrorSummary").css('display', 'block');
        }
        else
            $("#pnlPatientEligibilityDetail #ErrorSummary").css('display', 'none');

        //Error Status
        if (ErrorsCount > 0) {
            $("#pnlPatientEligibilityDetail #lblStatus").addClass("text-danger");
            $("#pnlPatientEligibilityDetail #lblStatus").html("Rejected");
        }

        else {
            $("#pnlPatientEligibilityDetail #lblStatus").addClass("text-success");
            $("#pnlPatientEligibilityDetail #lblStatus").html("Accepted");
        }


        //-----------------------Initialize rows and variables ends-----------------------------

        //-----------------------Bind report header data starts---------------------------------

        //Bind Patient info
        var dob = "";
        dob = Patientrow.DMG02;
        if (dob) {
            //Calculate age
            Patient_Demographic.FillAge(dob).done(function (response) {
                if (response.status != false) {
                    $("#pnlPatientEligibilityDetail #lblDOBAge").html(dob + " - " + response.ActualAge);
                }
            });
        }

        $("#pnlPatientEligibilityDetail #lblname").html(Patientrow.NM103 + ", " + Patientrow.NM104);
        $("#pnlPatientEligibilityDetail #lblGender").html(Patientrow.DMG03);
        $("#pnlPatientEligibilityDetail #lblDOBAge").html(dob);


        if (Patientrow.N301 != "")
            $("#pnlPatientEligibilityDetail #lblAddress").html(Patientrow.N301);

        if (Patientrow.N401 != "")
            $("#pnlPatientEligibilityDetail #lblAddress2").html(Patientrow.N401);

        var str = "";
        if (Patientrow.REF01 != "" && Patientrow.REF02 != "") {
            var ref01 = Patientrow.REF01.split(':');
            var ref02 = Patientrow.REF02.split(':');
            for (var i = 0; i < ref01.length; i++) {
                if (ref01[i] != "" && ref02[i]) {
                    str += '<span class="text-bold">' + ref01[i] + ': </span><span>' + ref02[i] + '</span><br />';
                }
            }

        }

        //if (Patientrow.NM108 != "" && Patientrow.NM109 != "") {
        //    str += '<span class="text-bold">' + Patientrow.NM108 + ': </span><span>' + Patientrow.NM109 + '</span><br />';
        //}
        if (str != "") {
            $("#pnlPatientEligibilityDetail #spREFContainer").html(str);
        }


        if (Headerrow != "") {
            if (Headerrow.IsEligible == "Active")
                $("#pnlPatientEligibilityDetail #lblEligibilityStatus").html("<span class='text-success text-bold' > Eligible </span> for the Service Type Code (" + Headerrow.ServiceTypeName + ")");
            else
                $("#pnlPatientEligibilityDetail #lblEligibilityStatus").html("<span class='text-danger text-bold' > Not Eligible </span> for the Service Type Code (" + Headerrow.ServiceTypeName + ")");
        }

        if (Batchrow != "") {
            $("#pnlPatientEligibilityDetail #lblEligibilityCopay").html(globalAppdata.DefaultCurrency + Number(Batchrow.Copay).toFixed(Number(globalAppdata.DecimalPlaces)));
            $("#pnlPatientEligibilityDetail #lblEligibilityDeductible").html(globalAppdata.DefaultCurrency + Number(Batchrow.Deductible).toFixed(Number(globalAppdata.DecimalPlaces)));
        }


        //Bind Plan and Subscriber info
        if (Batchrow != "") {
            if (Batchrow.InsurancePlanName)
                $("#pnlPatientEligibilityDetail #lblPlan").html(Batchrow.InsurancePlanName);
            if (Batchrow.SubmitterGroupId)
                $("#pnlPatientEligibilityDetail #lblGroup").html(Batchrow.SubmitterGroupId);
            if (Batchrow.PlanSubmitterId)
                $("#pnlPatientEligibilityDetail #lblSubscriberId").html(Batchrow.PlanSubmitterId);

            //Some Patient info
            $("#pnlPatientEligibilityDetail #lblRelationshipWithInsured").html(Batchrow.Relationship);
            $("#pnlPatientEligibilityDetail #lblDOS").html(Batchrow.DOS);
        }

        //Bind Subscriber info
        if (Subscriberrow != "") {
            var SubscriberName = Subscriberrow.NM103 + "" + Subscriberrow.NM104;
            if (SubscriberName)
               $("#pnlPatientEligibilityDetail #lblSubscriberName").html(Subscriberrow.NM103 + " " + Subscriberrow.NM104);
        }

        //Bind Provider info
        if (Providerrow != "") {
            var ProviderName = Providerrow.NM103 + "" + Providerrow.NM104;
            if (ProviderName)
               $("#pnlPatientEligibilityDetail #lblProviderName").html(Providerrow.NM103 + " " + Providerrow.NM104);
            if (Providerrow.NM109)
              $("#pnlPatientEligibilityDetail #lblProviderNPI").html(Providerrow.NM109);
            if (Providerrow.N301)
                $("#pnlPatientEligibilityDetail #lblProviderContact").html(Providerrow.N301);
        }

        //Bind Payer info
        if (Payerrow != "") {
            var PayerName = Payerrow.NM103 + "" + Payerrow.NM104;
            if (PayerName)
                $("#pnlPatientEligibilityDetail #lblPayerName").html(Payerrow.NM103 + " " + Payerrow.NM104);
            if (Payerrow.NM109)
               $("#pnlPatientEligibilityDetail #lblPayerId").html(Payerrow.NM109);
        }

        //Bind Primary care provider info
        if (PCProw != "") {
            var PCP = PCProw.NM103 + "" + PCProw.NM104;
            if (PCP)
                $("#pnlPatientEligibilityDetail #lblPCP").html(PCProw.NM103 + " " + PCProw.NM104);
            if (PCProw.NM109)
                $("#pnlPatientEligibilityDetail #lblNPI").html(PCProw.NM109);
            if (PCProw.N301)
                $("#pnlPatientEligibilityDetail #lblContact").html(PCProw.N301);
        }

        //----------------------------Bind report header data ends-----------------------------------



        //----------------------------Bind report grids data starts----------------------------------

        //Create Benefits tables
        $.each(Benifits_arraylist, function (i, item) {

            if (item.EB01) {
                Patient_Eligibility_Detail.AddBenifitTable(item.EB01);
            }

        });

        var DeductibleList = "";
        var NonCoveredList = "";
        var ActiveCoverageList = "";

        //Bind Benefits Data
        $.each(Benifits_list, function (i, item) {

            if (item.EB01) {

                var TableObj = "";
                var code = item.ServiceTypeCode.split('^');
                if (code.indexOf(ServiceTypeCode) >= 0)
                    TableObj = "#Benifits_div #dgvEligibility_" + item.EB01.replace(/[\s()-/]+/gi, '');
                else
                    TableObj = "#AllBenifits_div #dgvEligibility_" + item.EB01.replace(/[\s()-/]+/gi, '');

                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($(this))");

                $row.append(
                      '<td>' + item.EB02 + '</td>'
                    + '<td>' + item.EB03 + '</td>'
                    + '<td>' + item.EB04 + '</td>'
                    + '<td>' + item.EB05 + '</td>'
                    + '<td>' + item.EB07 + '</td>'
                    + '<td>' + item.EB12 + '</td>'
                    + '<td>' + item.EB13_1 + '</td>'
                    + '<td>' + item.DTP01 + " " + item.DTP03 + '</td>');

                $(TableObj + " tbody").last().append($row);
            }

            if (item.EB01.toLowerCase() == "active coverage" || item.EB01.toLowerCase() == "activecoverage")
                ActiveCoverageList += "<li class='bs-callout bs-callout-info'>" + item.EB03 + "</li>";
            else if (item.EB01.toLowerCase() == "non-covered")
                NonCoveredList += "<li class='bs-callout bs-callout-info'>" + item.EB03 + "</li>";
            else if (item.EB01.toLowerCase() == "deductible")
                DeductibleList += "<li class='bs-callout bs-callout-info'>" + item.EB03 + " $" + item.EB07 + " per " + item.EB06 + "</li>";

        });

        if (ActiveCoverageList == "")
            ActiveCoverageList = "<li>No Active Coverages</li>";
        if (NonCoveredList == "")
            NonCoveredList = "<li>No Non-Covered</li>";
        if (DeductibleList == "")
            DeductibleList = "<li>No Deductibles</li>";

        //bind filed lists
        $("#pnlPatientEligibilityDetail #lblActiveCoverageList").html(ActiveCoverageList);
        $("#pnlPatientEligibilityDetail #lblNonCoveredList").html(NonCoveredList);
        $("#pnlPatientEligibilityDetail #lblDeductibleList").html(DeductibleList);

        //----------------------------Bind report grids data ends----------------------------------

        //Internalize Toggle Plugin
        $('[data-plugin-toggle]').each(function () {
            var $this = $(this),
                opts = {};

            var pluginOptions = $this.data('plugin-options');
            if (pluginOptions)
                opts = pluginOptions;

            $this.themePluginToggle(opts);
        });

        //Hide Benefits tables that have no records.
        $.each(Benifits_arraylist, function (i, item) {

            if (item.EB01) {

                var tablename = "dgvEligibility_" + item.EB01.replace(/[\s()-/]+/gi, '');
                $("#" + tablename + " tbody").each(function (i, item) {
                    if ($(item).children('tr').length <= 0)
                        $(item).closest("#eligibility_" + tablename).css("display", "none");
                });
            }

        });

    },

    AddBenifitTable: function (BenifitName) {

        var tablename = "dgvEligibility_" + BenifitName.replace(/[\s()-/]+/gi, '');

        var table = '<div id="eligibility_' + tablename + '" class="toggle" data-plugin-toggle="">'
                        + '<section id="section_' + tablename + '" class="toggle">'
                            + '<label>' + BenifitName + '</label>'
                            + '<div class="toggle-content panel-body pt-sm NoRadiusT">'
                                + '<div id="pnl' + tablename + '_Result" style="display: inline;">'
                                    + '<table class="table table-bordered table-striped table-condensed table-hover mb-none" id="' + tablename + '">'
                                        + '<thead>'
                                            + '<tr class="bg-gray">'
                                                + '<th>Coverage Level</th>'
                                                + '<th>Service Type</th>'
                                                + '<th>Insurance Type</th>'
                                                + '<th>Description</th>'
                                                + '<th>Amount/Quantity</th>'
                                                + '<th>In Network / Out of Network</th>'
                                                + '<th>Procedure Code</th>'
                                                + '<th>Eligibility/Benefit Date</th>'
                                            + '</tr>'
                                        + '</thead>'
                                        + '<tbody></tbody>'
                                    + '</table>'
                                + '</div>'
                            + '</div>'
                        + '</section>'
                    + '</div>';

        if ($("#pnlPatientEligibilityDetail #Benifits_div table").length > 0)
            $("#pnlPatientEligibilityDetail #Benifits_div").append(table);
        else
            $("#pnlPatientEligibilityDetail #Benifits_div").html(table);

        if ($("#pnlPatientEligibilityDetail #AllBenifits_div table").length > 0)
            $("#pnlPatientEligibilityDetail #AllBenifits_div").append(table);
        else
            $("#pnlPatientEligibilityDetail #AllBenifits_div").html(table);

        return tablename;
    },

    AddErrorSummary: function (SectionName, IsEligible, Reason, Action) {
        var errorSummary = '<div class="splitter text-center" style=" margin:0px">' + SectionName + ' Request Validation Information</div>'
                                + '<div class="mt-xs">'
                                + '<span class="col-sm-3">Is Eligibility Request Valid:</span> <span  class="col-sm-3 text-justify">' + IsEligible + '</span>'
                                + '<span class="col-sm-3">Reject Reason Code:</span> <span  class="col-sm-3 text-justify">' + Reason + '</span>'
                                + '<span class="col-sm-3">Follow Up Action Code:</span> <span class="col-sm-3 text-justify">' + Action + '</span>'
                                + '<div class="spacer15"></div></div>';

        $("#pnlPatientEligibilityDetail #lblErrorSummaryList").append(errorSummary);
    },

    Load_EDIEligibilityDetail: function () {

        var data = "EDIEligibilityId=" + Patient_Eligibility_Detail.params.EDIEligibilityId;
        return MDVisionService.defaultService(data, "PATIENT_ELIGIBILITY_DETAIL", "LOAD_ELIGIBILITY_DETAIL");
    },

    UnLoad: function () {

        if (Patient_Eligibility_Detail.params != null && Patient_Eligibility_Detail.params.ParentCtrl) {
            UnloadActionPan(Patient_Eligibility_Detail.params.ParentCtrl);
        }
        else
            UnloadActionPan();

        //UnloadActionPan("Patient_Eligibility");
    },
}