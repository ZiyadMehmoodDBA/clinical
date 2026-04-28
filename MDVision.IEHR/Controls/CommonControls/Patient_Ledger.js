Patient_Ledger = {
    bIsFirstLoad: true,
    params: [],
    claimTypeClass: "",
    Load: function (params) {
        Patient_Ledger.params = params;
        if (Patient_Ledger.bIsFirstLoad) {
            Patient_Ledger.bIsFirstLoad = false;

            if (Patient_Ledger.params.PanelID != null && Patient_Ledger.params.PanelID != 'pnlPatientLedger' && Patient_Ledger.params.PanelID != 'mstrDivBilling')
                Patient_Ledger.params.PanelID = Patient_Ledger.params.PanelID + ' #pnlPatientLedger';
            else
                Patient_Ledger.params.PanelID = "pnlPatientLedger";

            var self = $('#' + Patient_Ledger.params.PanelID);
            Patient_Ledger.LoadDefaultSettinngs();

            //fix PMS-2421
            utility.ValidateFromToDate('frmPatientLedger', 'dpDOSFrom', 'dpDOSTo', true);

            Patient_Ledger.LoadAllAutocomplete();


            Patient_Ledger.fillPatientInfo();
            Patient_Ledger.LoadCharges();



        }
    },

    LoadAllAutocomplete: function () {

        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            var Ctrl = $('#' + Patient_Ledger.params["PanelID"] + " input#txtProvider");
            var hfCtrl = $("#" + Patient_Ledger.params["PanelID"] + " #hfProviderId");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl);
        });

        CacheManager.BindCodes('GetFacility', false).done(function (result) {
            var Ctrl = $('#' + Patient_Ledger.params["PanelID"] + " input#txtFacility");
            var hfCtrl = $("#" + Patient_Ledger.params["PanelID"] + " #hfFacilityId");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl);
        });

        CacheManager.BindCodes('GetInsurancePlan', false).done(function (result) {
            var Ctrl = $("#" + Patient_Ledger.params["PanelID"] + " input#txtInsurancePlan");
            var hfCtrl = $("#" + Patient_Ledger.params["PanelID"] + " #hfInsurancePlan");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", InsurancePlans, null, hfCtrl);
        });
    },

    LoadDefaultSettinngs: function () {
        if (Patient_Ledger.params.IsFromCollection == true) {
            $('#chkOtherBalance').prop('checked', false);
            $('#chkInculdeCollectionBal').prop('checked', true);
            $("#divIncudeCollection").hide();
            $("#divOtherBalance").show();
        }
        else {
            $('#chkOtherBalance').prop('checked', true);
            $('#chkInculdeCollectionBal').prop('checked', false);
            $("#divIncudeCollection").show();
            $("#divOtherBalance").hide();
        }
    },
    fillPatientInfo: function () {

        var self = $("#" + Patient_Ledger.params["PanelID"]);

        self.find("#txtPatientName").val(Patient_Ledger.params.PatientAccountNo);
        self.find("#txtPatLastName").val(Patient_Ledger.params.PatientLastName);
        self.find("#txtPatFirstName").val(Patient_Ledger.params.PatientFirstName);
        self.find("#dtpDOB").val(Patient_Ledger.params.PatientDOB);
        self.find("#txtAge").val(Patient_Ledger.params.patientAge);
        if (Patient_Ledger.params.PatientSex && Patient_Ledger.params.PatientSex.toLowerCase() == "male")
        {
            self.find("#txtSex").val("M");
        }
        else if (Patient_Ledger.params.PatientSex && Patient_Ledger.params.PatientSex.toLowerCase() == "female")
        {
            self.find("#txtSex").val("F");
        }
        else if (Patient_Ledger.params.PatientSex && Patient_Ledger.params.PatientSex.toLowerCase() == "unknown") {
            self.find("#txtSex").val("U");
        }
        else {
            self.find("#txtSex").val(Patient_Ledger.params.PatientSex);
        }
       
        self.find("#txtHomeTel").val(Patient_Ledger.params.PatientHomeTel);
    },
    OpenPatientPayment: function () {
        var params = [];
        params["PatientId"] = Patient_Ledger.params["patientID"];
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'Patient_Ledger';
        LoadActionPan('Bill_PatientPayments', params);
    },
    OpenPatientDemographics: function () {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Demographic", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                var params = [];
                params["mode"] = 'Edit';
                params["patientID"] = Patient_Ledger.params.patientID;
                params["FromAdmin"] = "0";
                if (Patient_Ledger.params.PatBanner)
                    params["FromBanner"] = Patient_Ledger.params.PatBanner;
                params["ParentCtrl"] = "Patient_Ledger";
                LoadActionPan('demographicDetail', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    OpenProvider: function () {
        var params = [];
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Patient_Ledger";
        if (Patient_Ledger.params.PanelID != 'pnlEncounter')
            LoadActionPan('Admin_Provider', params, Patient_Ledger.params.PanelID);
        else
            LoadActionPan('Admin_Provider', params);
    },

    OpenProviderDetail: function () {
        //Admin_Provider.ProviderEdit($('#' + demographicDetail.params.PanelID + ' #hfProvider').val(), "demographicDetail");
        var params = [];
        params["ProviderId"] = $('#' + Patient_Ledger.params["PanelID"] + ' #hfProviderId').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'Patient_Ledger';
        LoadActionPan('providerDetail', params);
    },

    OpenInsurancePlan: function () {
        var params = [];
        params["InsurancePlanId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Patient_Ledger";
        LoadActionPan('Admin_InsurancePlan', params);
    },

    FillInsurancePlanName: function (InsurancePlanId, InsurancePlanName) {

        $('#' + Patient_Ledger.params["PanelID"] + " #txtInsurancePlan").val(InsurancePlanName);
        $('#' + Patient_Ledger.params["PanelID"] + " #hfInsurancePlan").val(InsurancePlanId);
        //$('#' + Patient_Case_Detail.params["PanelID"] + " #lnkInsurancePlanDetail").css("display", "inline");
        //$('#' + Patient_Ledger.params["PanelID"] + " #lblInsurancePlan").css("display", "none");
        UnloadActionPan("Patient_Ledger");
        //Patient_Insurance.FillInsurancePlanAddress(InsurancePlanId);
    },


    OpenFacility: function () {
        var params = [];
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Patient_Ledger";
        if (Patient_Ledger.params.PanelID != 'pnlPatientLedger')
            LoadActionPan('Admin_Facility', params, Patient_Ledger.params.PanelID);
        else
            LoadActionPan('Admin_Facility', params);
    },

    OpenFacilityDetail: function () {
        var params = [];
        params["FacilityId"] = $('#' + Patient_Ledger.params.PanelID + ' #hfFacilityId').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacility";
        params["ParentCtrl"] = 'Patient_Ledger';
        LoadActionPan('facilityDetail', params);
    },



    LoadCharges: function (PageNo, rpp,isLedgerSearch) {
        var strMessage = "";
       
        if ($("#" + Patient_Ledger.params["PanelID"] + " #chkDetails").prop("checked")) {
            $("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientLedger_Result").hide();
            $("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientLedger_Detail_Result").show();
        }
        else {
            $("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientLedger_Detail_Result").hide();
            $("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientLedger_Result").show();
        }


        if (Patient_Ledger.params.patientID != null || Patient_Ledger.params.patientID != "") {

            var PatientId = Patient_Ledger.params.patientID;

            AppPrivileges.GetFormPrivileges("Payment Posting", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {

                    //if ($('#' + Patient_Ledger.params.PanelID + " #pnlPatientLedger_Result").css("display") == "none") {
                    //    $('#' + Patient_Ledger.params.PanelID + " #pnlPatientLedger_Result").show();
                    //}

                    var self = $("#" + Patient_Ledger.params["PanelID"] + ' #frmPatientLedger');
                    var myJSON = self.getMyJSON();



                    Patient_Ledger.SearchCharges(myJSON, PatientId, 0, PageNo, rpp).done(function (response) {
                        if (response.status != false) {
                            if (response.ChargeCount > 0) {

                                var chargeJSON = JSON.parse(response.ChargeLoad_JSON);
                                var patientJSON = JSON.parse(response.PatientsLoad_JSON);
                                if (Patient_Ledger.params.ParentCtrl == "EncounterChargeCapture") {
                                    var self = $("#" + Patient_Ledger.params["PanelID"]);

                                    self.find("#txtPatientName").val(patientJSON[0].AccountNumber);
                                    self.find("#txtPatLastName").val(patientJSON[0].LastName);
                                    self.find("#txtPatFirstName").val(patientJSON[0].FirstName);
                                    self.find("#dtpDOB").val(utility.RemoveTimeFromDate(null,patientJSON[0].DOB));
                                    self.find("#txtAge").val(patientJSON[0].PatientAge);
                                    self.find("#txtSex").val(patientJSON[0].Gender);
                                    self.find("#txtHomeTel").val(patientJSON[0].HomePhoneNo);
                                }
                                if (patientJSON[0].AccountNoteComments.trim() != "") {
                                    $("#pnlPatientLedger #txtAccountCommentsLoad").val("Last Updated By: " + patientJSON[0].ModifiedBy + " " + patientJSON[0].ModifiedOn + " Notes: " + patientJSON[0].AccountNoteComments);

                                }

                                $("#pnlPatientLedger #hfNameDateAccountNotes").val("Last Updated By: " + patientJSON[0].CreatedBy + " " + patientJSON[0].CreatedOn);
                                //$('#' + Patient_Ledger.params.PanelID + " #frmPatientLedger #txtInsuranceTransfer").trigger("keyup");
                                //$('#' + Patient_Ledger.params.PanelID + " #frmPatientLedger #txtPatientTransfer").trigger("keyup");

                                //------------Pagination-----------
                                //$("#" + Patient_Ledger.params["PanelID"] + " #divPatientLedgerPaging").css("display", "inline");
                                //Showing 1 to 15 of 15 entries
                                var RecordsPerPage = rpp != null ? rpp : 5;
                                var CurrentPage = PageNo != null ? PageNo : 1;

                                //params["myJSON"] = myJSON;

                                // var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                                //  var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                                //  if (PageNo == null) {
                                //utility.GetCustomPaging("divPatientLedgerPaging", response.iTotalDisplayRecords, 5, "Patient_Ledger", CurrentPage, RecordsPerPage);
                                //   }
                                //  var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                                // var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                                //$("#" + Patient_Ledger.params["PanelID"] + " #divPatientLedgerPaging #divShowingEntries").text(showingText);
                                // Change Background Color to Black for selected page
                                $("#" + Patient_Ledger.params["PanelID"] + " li").each(function () {
                                    if ($(this).text() == CurrentPage) {
                                        $(this).attr("class", "active");
                                    }
                                    else

                                        $(this).removeAttr("class");
                                });
                                //------------End Pagination-------
                            }
                            else {
                                //$("#" + Patient_Ledger.params["PanelID"] + " #divPatientLedgerPaging").css("display", "none");
                            }
                            if ($("#" + Patient_Ledger.params["PanelID"] + " #chkDetails").prop("checked")) {
                                Patient_Ledger.ChargesDetailGridLoad(response);
                            }
                            else {
                                Patient_Ledger.ChargesGridLoad(response);
                            }
                            if (!isLedgerSearch) {
                                Patient_Ledger.OutstandingGridLoad(response);
                            }
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });


                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }

        else {
            utility.DisplayMessages("Please Select a Patient", 2);
        }
    },

    ChargesGridLoad: function (response) {
        utility.ClearFormValidation('#' + Patient_Ledger.params.PanelID + ' #frmPatientLedger');
        $("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientLedger_Result #dgvPatientLedger tfoot").html("");

        var $FooterRow = $('<tr class="bold black bg-default" style="text-align: right" />');
        // $("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientLedger_Result #dgvPatientLedger").dataTable().fnDestroy();
        $("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientLedger_Result #dgvPatientLedger tbody").find("tr").remove();
        var PreviousClaimNo = "";
        var PreviousDOS = "";
        if (response.ChargeCount > 0) {//response.ChargeCount
            var ChargeLoadJSONData = JSON.parse(response.ChargeLoad_JSON);
            var ttlCharges = 0;
            var ttlpayment = 0;
            var ttlAdjustment = 0;
            var ttlOutstandingbal = 0;
            $.each(ChargeLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                var VisitDetailText = "";
                $row.attr("onclick", "utility.SelectGridRow($('#dgvPatientLedger_row" + item.chargeid + "'));");
                $row.attr("id", "dgvPatientLedger_row" + item.chargeid);
                $row.attr("ChargeId", item.chargeid);
               
             
                if (PreviousClaimNo != item.ClaimNumber) {
                    if (!EMRUtility.CheckPnlOpen("pnlEncounterChargeCapture")) {
                        VisitDetail = "utility.LoadVisitDetail('" + item.VisitId + "', '" + item.PatientId + "', 'Patient_Ledger', event);";
                        VisitDetailText = '<a href="#" onclick="' + VisitDetail + '"  title="View Claim Detail">' + item.ClaimNumber + '</a>';
                    } else {
                        VisitDetailText = item.ClaimNumber;
                    }
                }
                else {
                    VisitDetailText = "";
                    if (PreviousDOS == item.DOSFrom) {
                        item.DOSFrom = "";
                    }
                }
                ttlCharges = parseFloat(ttlCharges) + parseFloat(item.TotalCharges);
                ttlpayment = parseFloat(ttlpayment) + parseFloat(item.TotalPayments);
                ttlAdjustment = parseFloat(ttlAdjustment) + parseFloat(item.TotalAdjustments);
                ttlOutstandingbal = parseFloat(ttlOutstandingbal) + parseFloat(item.OutStandingBalance);

                if (VisitDetailText && item.IsCollection.toLowerCase() == "true" && Patient_Ledger.params.IsFromCollection != true) {
                    $row.append('<td style="display:none;">' + item.chargeid + '</td><td style="background-color:#ff8080">' + VisitDetailText + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td><td>' + utility.RemoveTimeFromDate(null, item.TransactionDate) + '</td><td >' + item.Description + '</td><td>' + utility.convertToFigure(item.TotalCharges, true) + '</td><td>' + utility.convertToFigure(item.TotalPayments, true) + '</td><td>' + utility.convertToFigure(item.TotalAdjustments, true) + '</td><td>' + utility.convertToFigure(item.OutStandingBalance, true) + '</td>');
                }
                else if (VisitDetailText && item.IsCollection.toLowerCase() == "true" && Patient_Ledger.params.IsFromCollection == true && $("#" + Patient_Ledger.params["PanelID"] + " #chkOtherBalance").prop('checked') == true) {
                    $row.append('<td style="display:none;">' + item.chargeid + '</td><td style="background-color:#ff8080">' + VisitDetailText + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td><td>' + utility.RemoveTimeFromDate(null, item.TransactionDate) + '</td><td >' + item.Description + '</td><td>' + utility.convertToFigure(item.TotalCharges, true) + '</td><td>' + utility.convertToFigure(item.TotalPayments, true) + '</td><td>' + utility.convertToFigure(item.TotalAdjustments, true) + '</td><td>' + utility.convertToFigure(item.OutStandingBalance, true) + '</td>');
                }
                else {
                    $row.append('<td style="display:none;">' + item.chargeid + '</td><td>' + VisitDetailText + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td><td>' + utility.RemoveTimeFromDate(null, item.TransactionDate) + '</td><td >' + item.Description + '</td><td>' + utility.convertToFigure(item.TotalCharges, true) + '</td><td>' + utility.convertToFigure(item.TotalPayments, true) + '</td><td>' + utility.convertToFigure(item.TotalAdjustments, true) + '</td><td>' + utility.convertToFigure(item.OutStandingBalance, true) + '</td>');
                }
                $("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientLedger_Result #dgvPatientLedger tbody").last().append($row);
                PreviousClaimNo = item.ClaimNumber;
                PreviousDOS = item.DOSFrom;
            });

            $FooterRow.html('<td colspan="3"></td><td style="Text-align:left">Total</td>' +
               '<td style="text-align:left;">' + utility.convertToFigure(ttlCharges, true) + '</td>' +
               '<td style="text-align:left;">' + utility.convertToFigure(ttlpayment, true) + '</td>' +
               '<td style="text-align:left;">' + utility.convertToFigure(ttlAdjustment, true) + '</td>' +
               '<td style="text-align:left;">' + utility.convertToFigure(ttlOutstandingbal, true) + '</td>');

            $("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientLedger_Result #dgvPatientLedger tfoot").html($FooterRow);
         }
        else {
            if ($.fn.dataTable.isDataTable("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientLedger_Result #dgvPatientLedger")) {
                $("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientLedger_Result #dgvPatientLedger tbody").last().append("<tr><td colspan='10' style='text-align: center'>No Charges Found</td></tr>");
            }
            else {
                $("#" + Patient_Ledger.params["PanelID"] + " #divPatientLedgerPaging").css("display", "none");
                $("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientLedger_Result #dgvPatientLedger").DataTable({
                    "language": { "emptyTable": "No Charges Found" }, "autoWidth": false, "bInfo": false, "bLengthChange": false, "bSort": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
            }
        }
        if ($.fn.dataTable.isDataTable("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientLedger_Result #dgvPatientLedger"))
            ;
        else
            //  $("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientLedger_Result #dgvPatientLedger").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "iDisplayLength": 5, "bFilter": true, "bSort": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown


        //    Patient_Ledger.TogglingVoidedClaims();
        //Patient_Ledger.TogglingCPTPayments();
        //Patient_Ledger.TogglingSubTotalCPTPayments();
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });

    },

    ChargesDetailGridLoad: function (response) {
      
        utility.ClearFormValidation('#' + Patient_Ledger.params.PanelID + ' #frmPatientLedger');
        var $FooterRow = $('<tr class="bold black bg-default" style="text-align: right" />');
        // $("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientLedger_Result #dgvPatientLedger").dataTable().fnDestroy();
        $("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientLedger_Detail_Result #dgvPatientLedger_Detail tbody").find("tr").remove();
        var PreviousClaimNo = "";
        var PreviousDOS = "";
        if (response.ChargeCount > 0) {//response.ChargeCount
            var ChargeLoadJSONData = JSON.parse(response.ChargeLoad_JSON);
            //var ttlCharges = 0;
            //var ttlpayment = 0;
            //var ttlAdjustment = 0;
            //var ttlOutstandingbal = 0;
            $.each(ChargeLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                var VisitDetailText = "";
                $row.attr("onclick", "utility.SelectGridRow($('#dgvPatientLedger_Detail_row" + item.ChargeCapId + "'));");
                $row.attr("id", "dgvPatientLedger_Detail_row" + item.ChargeCapId);
                $row.attr("ChargeId", item.ChargeCapId);

                if (PreviousClaimNo != item.ClaimNumber) {
                    if (!EMRUtility.CheckPnlOpen("pnlEncounterChargeCapture")) {
                        VisitDetail = "utility.LoadVisitDetail('" + item.VisitId + "', '" + item.PatientId + "', 'Patient_Ledger', event);";
                        VisitDetailText = '<a href="#" onclick="' + VisitDetail + '"  title="View Claim Detail">' + item.ClaimNumber + '</a>';
                    } else {
                        VisitDetailText = item.ClaimNumber;
                    }
                }
                else {
                    VisitDetailText = "";
                    
                }
                //ttlCharges = parseFloat(ttlCharges) + parseFloat(item.TotalCharges);
                //ttlpayment = parseFloat(ttlpayment) + parseFloat(item.TotalPayments);
                //ttlAdjustment = parseFloat(ttlAdjustment) + parseFloat(item.TotalAdjustments);
                //ttlOutstandingbal = parseFloat(ttlOutstandingbal) + parseFloat(item.OutStandingBalance);
                if (Number(item.PrimaryIns) < 0) {
                    item.PrimaryIns = utility.convertToFigure(Math.abs(item.PrimaryIns), true, true)
                }
                else {
                    item.PrimaryIns = utility.convertToFigure(item.PrimaryIns, true)
                }

                if (Number(item.SecondaryIns) < 0) {
                    item.SecondaryIns = utility.convertToFigure(Math.abs(item.SecondaryIns), true, true)
                }
                else {
                    item.SecondaryIns = utility.convertToFigure(item.SecondaryIns, true)
                }

                if (Number(item.TertiaryIns) < 0) {
                    item.TertiaryIns = utility.convertToFigure(Math.abs(item.TertiaryIns), true, true)
                }
                else {
                    item.TertiaryIns = utility.convertToFigure(item.TertiaryIns, true)
                }

                if (Number(item.Patient) < 0) {
                    item.Patient = utility.convertToFigure(Math.abs(item.Patient), true, true)
                }
                else {
                    item.Patient = utility.convertToFigure(item.Patient, true)
                }
                if (Number(item.OutstandingBalance) < 0) {
                    item.OutstandingBalance = utility.convertToFigure(Math.abs(item.OutstandingBalance), true, true)
                }
                else {
                    item.OutstandingBalance = utility.convertToFigure(item.OutstandingBalance, true)
                }

                if (item.TransactionType == "Charge") {
                    PreviousDOS = item.DOSFrom;
                }
                else {
                    item.DOSFrom = "";
                    item.OutstandingBalance = "";
                }

                if (VisitDetailText && item.IsCollection.toLowerCase() == "true" && Patient_Ledger.params.IsFromCollection != true) {
                    $row.append('<td style="display:none;">' + item.ChargeCapId + '</td><td style="background-color:#ff8080">' + VisitDetailText + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td><td>' + utility.RemoveTimeFromDate(null, item.TransactionDate) + '</td><td>' + item.TransactionType + '</td><td >' + item.Description + '</td><td>' + item.PrimaryIns + '</td><td>' + item.SecondaryIns + '</td><td>' + item.TertiaryIns + '</td><td>' + item.Patient + '</td><td>' + item.OutstandingBalance + '</td>');
                }
                else if (VisitDetailText && item.IsCollection.toLowerCase() == "true" && Patient_Ledger.params.IsFromCollection == true && $("#" + Patient_Ledger.params["PanelID"] + " #chkOtherBalance").prop('checked') == true) {
                    $row.append('<td style="display:none;">' + item.ChargeCapId + '</td><td style="background-color:#ff8080">' + VisitDetailText + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td><td>' + utility.RemoveTimeFromDate(null, item.TransactionDate) + '</td><td>' + item.TransactionType + '</td><td >' + item.Description + '</td><td>' + item.PrimaryIns + '</td><td>' + item.SecondaryIns + '</td><td>' + item.TertiaryIns + '</td><td>' + item.Patient + '</td><td>' + item.OutstandingBalance + '</td>');

                }
                else {
                    $row.append('<td style="display:none;">' + item.ChargeCapId + '</td><td>' + VisitDetailText + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td><td>' + utility.RemoveTimeFromDate(null, item.TransactionDate) + '</td><td>' + item.TransactionType + '</td><td >' + item.Description + '</td><td>' + item.PrimaryIns + '</td><td>' + item.SecondaryIns + '</td><td>' + item.TertiaryIns + '</td><td>' + item.Patient + '</td><td>' + item.OutstandingBalance + '</td>');
                }
                $("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientLedger_Detail_Result #dgvPatientLedger_Detail tbody").last().append($row);
                PreviousClaimNo = item.ClaimNumber;
               
            });

            //$FooterRow.html('<td colspan="3"></td><td style="Text-align:left">Total</td>' +
            //   '<td style="text-align:left;">' + utility.convertToFigure(ttlCharges, true) + '</td>' +
            //   '<td style="text-align:left;">' + utility.convertToFigure(ttlpayment, true) + '</td>' +
            //   '<td style="text-align:left;">' + utility.convertToFigure(ttlAdjustment, true) + '</td>' +
            //   '<td style="text-align:left;">' + utility.convertToFigure(ttlOutstandingbal, true) + '</td>');

            //$("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientLedger_Result #dgvPatientLedger_Detail tfoot").html($FooterRow);
        }
        else {
            if ($.fn.dataTable.isDataTable("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientLedger_Detail_Result #dgvPatientLedger_Detail")) {
                $("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientLedger_Detail_Result #dgvPatientLedger_Detail tbody").last().append("<tr><td colspan='10' style='text-align: center'>No Charges Found</td></tr>");
            }
            else {
                $("#" + Patient_Ledger.params["PanelID"] + " #divPatientLedgerPaging").css("display", "none");
                $("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientLedger_Detail_Result #dgvPatientLedger_Detail").DataTable({
                    "language": { "emptyTable": "No Charges Found" }, "autoWidth": false, "bInfo": false, "bLengthChange": false, "bSort": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
            }
        }
        if ($.fn.dataTable.isDataTable("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientLedger_Detail_Result #dgvPatientLedger_Detail"))
            ;
        else
            //  $("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientLedger_Result #dgvPatientLedger").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "iDisplayLength": 5, "bFilter": true, "bSort": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown


            //    Patient_Ledger.TogglingVoidedClaims();
            //Patient_Ledger.TogglingCPTPayments();
            //Patient_Ledger.TogglingSubTotalCPTPayments();
            //Set ToolTip for Comments.
            $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });

    },
    PatientLedgerReset: function () {
        $('#' + Patient_Ledger.params.PanelID + ' #divSearchCharges').resetAllControls();
        $("#" + Patient_Ledger.params["PanelID"] + " #hfInsurancePlan").val("");
        $("#" + Patient_Ledger.params["PanelID"] + " #hfFacilityId").val("");
        $("#" + Patient_Ledger.params["PanelID"] + " #hfProviderId").val("");
        Patient_Ledger.LoadDefaultSettinngs();
        $('#' + Patient_Ledger.params["PanelID"] + ' #frmPatientLedger [type="text"][onblur]').each(function () {
            $(this).trigger("blur");
        });
        /// Start PRD-76
        if ($("#" + Patient_Ledger.params["PanelID"] + " #lnkProviderEditLedger").is(":visible"))
        { $("#" + Patient_Ledger.params["PanelID"] + " #lnkProviderEditLedger").hide(); }
        if (!$("#" + Patient_Ledger.params["PanelID"] + " #lblProviderLedger").is(":visible"))
        { $("#" + Patient_Ledger.params["PanelID"] + " #lblProviderLedger").show(); }
        // reset to default state
        if ($("#" + Patient_Ledger.params["PanelID"] + " #lnkFacilityEditLedger").is(":visible"))
        { $("#" + Patient_Ledger.params["PanelID"] + " #lnkFacilityEditLedger").hide(); }
        if (!$("#" + Patient_Ledger.params["PanelID"] + " #lblFacilityLedger").is(":visible"))
        { $("#" + Patient_Ledger.params["PanelID"] + " #lblFacilityLedger").show(); }

        
        /// End PRD-76
    },

    ClaimTypeOnChange: function () {
        if ($("#" + Patient_Ledger.params["PanelID"] + " #ddlLedgerClaimType").val() == "1") {

            $("#" + Patient_Ledger.params["PanelID"] + " #frmPatientLedger #dgvPatientLedger tr").each(function (i, row) {
                if ($(this).find("td").eq(7).text() == "$0.00" && $(this).find("td").eq(8).text() == "$0.00" && $(this).attr("class").includes("CPTPaymentTotal")) {
                    var strClaimTypeClass = $(this).attr("name");
                    $("." + strClaimTypeClass).hide();
                }
            });
        }
        else {

            // $("." + Patient_Ledger.claimTypeClass).show();
            $("#" + Patient_Ledger.params["PanelID"] + " #frmPatientLedger #dgvPatientLedger tr").each(function (i, row) {
                var strClaimTypeClass = $(this).attr("name");
                $("." + strClaimTypeClass).show();
            });
            //Patient_Ledger.TogglingVoidedClaims();
            //Patient_Ledger.TogglingCPTPayments();
            //Patient_Ledger.TogglingSubTotalCPTPayments();
        }
    },
    PrintReport: function () {
        $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientOutstanding tr td").css("text-align", "center");
        if (!$("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientLedger_Detail_Result").css('visibility') === 'hidden') {
            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger_Detail tr td").css("text-align", "center");
        }
        else {
            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger tr td").css("text-align", "center");
        }
       
        $("#" + Patient_Ledger.params["PanelID"] + " #lblPatientName").text(": " + $("#" + Patient_Ledger.params["PanelID"] + " #txtPatientName").val());
        $("#" + Patient_Ledger.params["PanelID"] + " #lblPatLastName").text(": " + $("#" + Patient_Ledger.params["PanelID"] + " #txtPatLastName").val());
        $("#" + Patient_Ledger.params["PanelID"] + " #lblPatFirstName").text(": " + $("#" + Patient_Ledger.params["PanelID"] + " #txtPatFirstName").val());
        $("#" + Patient_Ledger.params["PanelID"] + " #lblDOB").text(": " + $("#" + Patient_Ledger.params["PanelID"] + " #dtpDOB").val());

        $("#" + Patient_Ledger.params["PanelID"] + " #lblAge").text(": " + $("#" + Patient_Ledger.params["PanelID"] + " #txtAge").val());
        $("#" + Patient_Ledger.params["PanelID"] + " #lblSex").text(": " + $("#" + Patient_Ledger.params["PanelID"] + " #txtSex").val());
        $("#" + Patient_Ledger.params["PanelID"] + " #lblHomeTel").text(": " + $("#" + Patient_Ledger.params["PanelID"] + " #txtHomeTel").val());
        //$("#" + Bill_ERA_ElectronicEOB.params["PanelID"] + " #reportTable").removeClass("Of-a");
        $("#" + Patient_Ledger.params["PanelID"] + " #PrintReport").hide();
        $("#" + Patient_Ledger.params["PanelID"] + " #divSearchCharges").hide();

        $("#" + Patient_Ledger.params["PanelID"] + " #txtAccountComments").next().hide();
        $("#" + Patient_Ledger.params["PanelID"] + " #txtAccountComments").hide();
        $("#" + Patient_Ledger.params["PanelID"] + " #txtAccountCommentsLoad").prev().hide();
        $("#" + Patient_Ledger.params["PanelID"] + " #AccountNote").css("width", "100%");
        $("#" + Patient_Ledger.params["PanelID"] + " #txtAccountCommentsLoad").css({ "width": "100%", "margin-left": "1%", "margin-right": "1%" });
        $("#" + Patient_Ledger.params["PanelID"] + " .txtPatientInfo").hide();
        $("#" + Patient_Ledger.params["PanelID"] + " .lblPatientInfo").show();
        // add comment on print screen.
        $("#" + Patient_Ledger.params["PanelID"] + " #txtAccountCommentsLoad").text($("#" + Patient_Ledger.params["PanelID"] + " #txtAccountCommentsLoad").val());
        //  $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientOutstanding_filter").hide();

        var ua = window.navigator.userAgent;
        var msie = ua.indexOf("MSIE ");
        if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {
            var strcontents = $("#" + Patient_Ledger.params["PanelID"] + " #mainScrollPan").html();

            var ReportName = "Patient Ledger";
            var windowUrl = 'Patient Ledger';
            var uniqueName = new Date();
            var windowName = 'Print' + uniqueName.getTime();
            var printWindow = window.open('', 'printwindow');
            printWindow.document.write('<html><head><title>' + ReportName + ' | PrintReport</title>');
            printWindow.document.write('</head><body>');
            //Append the external CSS file.
            printWindow.document.write('<link rel="stylesheet" media="print" href="Content/Blue/bootstrap.css" /> <link rel="stylesheet" media="print" href="Content/Blue/theme.css" /><link rel="stylesheet" media="print" href="Content/Blue/theme-custom.css" /><link rel="stylesheet" media="print" href="Content/Blue/default.css" />');
            //Append the DIV contents.
            printWindow.document.write(strcontents);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            printWindow.close();
            setTimeout(function () {
                $("#" + Patient_Ledger.params["PanelID"] + " #PrintReport").show();
                $("#" + Patient_Ledger.params["PanelID"] + " #divSearchCharges").show();
                $("#" + Patient_Ledger.params["PanelID"] + " .txtPatientInfo").show();
                $("#" + Patient_Ledger.params["PanelID"] + " .lblPatientInfo").hide();
                $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientOutstanding tr td").css("text-align", "");

                if (!$("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientLedger_Detail_Result").css('visibility') === 'hidden') {
                    $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger_Detail tr td").css("text-align", "");
                }
                else {
                    $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger tr td").css("text-align", "");
                    $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger tfoot tr td").css("text-align", "left");
                    
                }
              
            }, 200);
        }
        else {
            var ReportName = "Patient Ledger";
            setTimeout(function () {
                var contents = $("#" + Patient_Ledger.params["PanelID"] + " #mainScrollPan").html();
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
                    window.frames[ReportName.toLowerCase().trim()].focus();
                    window.frames[ReportName.toLowerCase().trim()].print();
                    frame1.remove();
                    //$("#" + Bill_ERA_ElectronicEOB.params["PanelID"] + " #reportTable").addClass("Of-a");
                    $("#" + Patient_Ledger.params["PanelID"] + " #PrintReport").show();
                    $("#" + Patient_Ledger.params["PanelID"] + " #divSearchCharges").show();
                    $("#" + Patient_Ledger.params["PanelID"] + " .txtPatientInfo").show();
                    $("#" + Patient_Ledger.params["PanelID"] + " .lblPatientInfo").hide();
                    $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientOutstanding tr td").css("text-align", "");
                    if (!$("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientLedger_Detail_Result").css('visibility') === 'hidden') {
                        $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger_Detail tr td").css("text-align", "");
                    }
                    else {
                        $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger tr td").css("text-align", "");
                        $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger tfoot tr td").css("text-align", "left");
                    }
                    //$("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger tr td").css("text-align", "");
                    // $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientOutstanding_filter").show();
                    $("#" + Patient_Ledger.params["PanelID"] + " #txtAccountComments").next().show();
                    $("#" + Patient_Ledger.params["PanelID"] + " #txtAccountComments").show();
                    $("#" + Patient_Ledger.params["PanelID"] + " #txtAccountCommentsLoad").prev().show();
                    $("#" + Patient_Ledger.params["PanelID"] + " #AccountNote").css("width", "100%");
                    $("#" + Patient_Ledger.params["PanelID"] + " #txtAccountCommentsLoad").css({ "margin-left": "0%", "margin-right": "0%" });
                }, 200);

            }, 100);
        }
    },
    AddSubTotalRow: function (SubTotalArray) {
        var TotalPayment = 0;
        var totalAdjustment = 0;
        var Charges = 0;
        var VisitId;
        for (var i = 0; i < SubTotalArray.length; i++) {
            // var subArray = 


            //$.each(SubTotalArray, function (i, item) {
            //    if (i == 0) {
            //        VisitId = item.VisitId; 
            //    }

            //    if (VisitId = item.VisitId) {
            //        TotalPayment = Numer(TotalPayment) + Number(item.TotalCPTPayments);
            //    }

            //});
        }

    },
    ShowClaimDetail: function()
    {
        var PatientId = Patient_Ledger.params.patientID;
        var self = $("#" + Patient_Ledger.params["PanelID"] + ' #frmPatientLedger');
        var myJSON = self.getMyJSON();
        Patient_Ledger.SearchCharges(myJSON, PatientId, 0, 1, 5000).done(function (response) {
            if (response.status != false) {
                if ($("#" + Patient_Ledger.params["PanelID"] + " #chkDetails").prop("checked")) {
                    $("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientLedger_Result").hide();
                    $("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientLedger_Detail_Result").show();
                    Patient_Ledger.ChargesDetailGridLoad(response);
                }
                else {
                    $("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientLedger_Detail_Result").hide();
                    $("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientLedger_Result").show();
                    Patient_Ledger.ChargesGridLoad(response);
                }
            }
        }
);
       
    },
    //TogglingCPTPayments: function () {
    //    if ($("#" + Patient_Ledger.params["PanelID"] + " #chkCPTPayments").prop('checked') == true) {

    //        if ($("#" + Patient_Ledger.params["PanelID"] + " #chkVoidedClaims").prop('checked') == true) {
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .CPTPaymentTotal").show();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .VoidedAndRecreateClaims").show();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .VandRClaimsForSubtotalLabel").hide();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .CPTPaymentSubTotal").hide();
    //        }
    //        else {
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .CPTPaymentTotal").show();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .VoidedAndRecreateClaims").hide();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .VandRClaimsForSubtotalLabel").hide();
    //            //  $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .CPTPaymentSubTotal").hide();
    //        }
    //    }
    //    else {
    //        $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .CPTPaymentTotal").hide();
    //    }

    //},

    //TogglingVoidedClaims: function () {
    //    if ($("#" + Patient_Ledger.params["PanelID"] + " #chkVoidedClaims").prop('checked') == true) {

    //        if ($("#" + Patient_Ledger.params["PanelID"] + " #chkCPTPayments").prop('checked') == true && $("#" + Patient_Ledger.params["PanelID"] + " #chkPatPaymentTotal").prop('checked') != true) {
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .VoidedAndRecreateClaims").show();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .CPTPaymentTotal").show();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .CPTPaymentSubTotal").hide();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .VandRClaimsForSubtotalLabel").hide();
    //        }
    //        else if ($("#" + Patient_Ledger.params["PanelID"] + " #chkCPTPayments").prop('checked') != true && $("#" + Patient_Ledger.params["PanelID"] + " #chkPatPaymentTotal").prop('checked') == true) {
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .VoidedAndRecreateClaims").show();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .CPTPaymentSubTotal").show();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .VandRClaimsForSubtotalLabel").show();
    //        }
    //        else if ($("#" + Patient_Ledger.params["PanelID"] + " #chkCPTPayments").prop('checked') == true && $("#" + Patient_Ledger.params["PanelID"] + " #chkPatPaymentTotal").prop('checked') == true) {
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .VoidedAndRecreateClaims").show();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .CPTPaymentTotal").show();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .CPTPaymentSubTotal").show();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .VandRClaimsForSubtotalLabel").show();
    //        }
    //        else {
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .VoidedAndRecreateClaims").show();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .CPTPaymentTotal").hide();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .CPTPaymentSubTotal").hide();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .VandRClaimsForSubtotalLabel").hide();
    //        }
    //    }
    //    else {
    //        //  $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .VoidedAndRecreateClaims").hide();
    //        if ($("#" + Patient_Ledger.params["PanelID"] + " #chkCPTPayments").prop('checked') == true && $("#" + Patient_Ledger.params["PanelID"] + " #chkPatPaymentTotal").prop('checked') != true) {
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .CPTPaymentTotal").show();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .VoidedAndRecreateClaims").hide();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .VandRClaimsForSubtotalLabel").hide();
    //        }
    //        else if ($("#" + Patient_Ledger.params["PanelID"] + " #chkCPTPayments").prop('checked') != true && $("#" + Patient_Ledger.params["PanelID"] + " #chkPatPaymentTotal").prop('checked') == true) {
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .CPTPaymentSubTotal").show();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .VoidedAndRecreateClaims").hide();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .VandRClaimsForSubtotalLabel").hide();
    //        }
    //        else if ($("#" + Patient_Ledger.params["PanelID"] + " #chkCPTPayments").prop('checked') == true && $("#" + Patient_Ledger.params["PanelID"] + " #chkPatPaymentTotal").prop('checked') == true) {

    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .CPTPaymentTotal").show();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .CPTPaymentSubTotal").show();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .VoidedAndRecreateClaims").hide();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .VandRClaimsForSubtotalLabel").hide();
    //        }
    //        else {
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .VoidedAndRecreateClaims").hide();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .CPTPaymentTotal").hide();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .CPTPaymentSubTotal").hide();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .VandRClaimsForSubtotalLabel").hide();
    //        }
    //    }


    //},

    //TogglingSubTotalCPTPayments: function () {
    //    if ($("#" + Patient_Ledger.params["PanelID"] + " #chkPatPaymentTotal").prop('checked') == true) {
    //        if ($("#" + Patient_Ledger.params["PanelID"] + " #chkVoidedClaims").prop('checked') == true) {
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .CPTPaymentSubTotal").show();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .VoidedAndRecreateClaims").show();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .VandRClaimsForSubtotalLabel").show();
    //        }
    //        else {
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .CPTPaymentSubTotal").show();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .VoidedAndRecreateClaims").hide();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .VandRClaimsForSubtotalLabel").hide();
    //        }
    //    }
    //    else {
    //        $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .CPTPaymentSubTotal").hide();
    //    }
    //},


    //TogglingClaimBalance: function () {

    //    var BilledToSelectedValue = $("#" + Patient_Ledger.params["PanelID"] + " #ddlBilledTo").val();

    //    if ($("#" + Patient_Ledger.params["PanelID"] + " #ddlClaimType").val() == "") {
    //        if (BilledToSelectedValue == 0)
    //        {
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .BilledToPatient").show();
    //        }
    //        else if (BilledToSelectedValue == 1) {
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .BilledToInsurance").show();
    //        }
    //        else {
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .Primary").show();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .Secondary").show();
    //        }
    //    }
    //    else if ($("#" + Patient_Ledger.params["PanelID"] + " #ddlClaimType").val() == 0) {
    //        if (BilledToSelectedValue == 0) {
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .Primary .BilledToPatient").show();
    //        }
    //        else if (BilledToSelectedValue == 1) {
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .Primary .BilledToInsurance").show();
    //        }
    //        else {
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .Primary").show();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .Secondary").hide();
    //        }
    //    }
    //    else if ($("#" + Patient_Ledger.params["PanelID"] + " #ddlClaimType").val() == 1) {
    //        if (BilledToSelectedValue == 0) {
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .Secondary .BilledToPatient").show();
    //        }
    //        else if (BilledToSelectedValue == 1) {
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .Secondary .BilledToInsurance").show();
    //        }
    //        else {
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .Secondary").show();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .Primary").hide();
    //        }
    //    }

    //},
    //TogglingBilledTo: function () {
    //    var ClaimBalanceSelectedValue = $("#" + Patient_Ledger.params["PanelID"] + " #ddlClaimType").val();

    //    if ($("#" + Patient_Ledger.params["PanelID"] + " #ddlBilledTo").val() == "") {
    //        if (ClaimBalanceSelectedValue == 0) {
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .Primary").show();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .Secondary").hide();
    //        }
    //        else if (ClaimBalanceSelectedValue == 1) {
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .Secondary").show();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .Primary").hide();
    //        }
    //        else {
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .BilledToPatient").show();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .BilledToInsurance").show();
    //        }
    //    }
    //    else if ($("#" + Patient_Ledger.params["PanelID"] + " #ddlBilledTo").val() == 0) {
    //        if (ClaimBalanceSelectedValue == 0) {
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .Primary .BilledToPatient").show();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .Secondary .BilledToPatient").hide();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .BilledToInsurance").hide();
    //        }
    //        else if (ClaimBalanceSelectedValue == 1) {
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .Secondary .BilledToPatient").show();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .Primary .BilledToPatient").hide();
    //        }
    //        else {
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .BilledToPatient").show();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .BilledToInsurance").hide();
    //        }
    //    }
    //    else if ($("#" + Patient_Ledger.params["PanelID"] + " #ddlBilledTo").val() == 1) {
    //        if (ClaimBalanceSelectedValue == 0) {
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .Primary .BilledToInsurance").show();
    //        }
    //        else if (ClaimBalanceSelectedValue == 1) {
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .Secondary .BilledToInsurance").show();
    //        }
    //        else {
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .BilledToInsurance").show();
    //            $("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger .BilledToPatient").hide();
    //        }
    //    }

    //},

    SearchCharges: function (ChargeData, PatientId, ChargeId, PageNumber, RowsPerPage) {

        if (PatientId == null) {
            PatientId = 0;
        }

        if (ChargeId == null) {
            ChargeId = 0;
        }
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 5;
        }


        Patient_Ledger.params.CurrentPageNo = PageNumber;
        var data = "ChargeData=" + ChargeData + "&PatientId=" + PatientId + "&ChargeId=" + ChargeId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_LEDGER", "SEARCH_CHARGE");
    },




    OutstandingGridLoad: function (response) {

        $("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientOutstanding_Result #dgvPatientOutstanding").dataTable().fnDestroy();
        $("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientOutstanding_Result #dgvPatientOutstanding tbody").find("tr").remove();
        if (response.OutstandingBalanceCount > 0) {
            var OutstandingJSONData = JSON.parse(response.OutstandingLoad_JSON);
            $.each(OutstandingJSONData, function (i, item) {
                var $row = $('<tr/>');
                // $row.attr("onclick", "utility.SelectGridRow($('#gvPatientOutstanding_row" + item.PatientId + "'));");
                $row.attr("id", "gvPatientOutstanding_row" + item.PatientId);
                $row.attr("ChargeId", item.PatientId);

                $row.append('<td style="display:none;">' + item.PatientId + '</td><th class="rowHeading">' + item.Title + '</th><td>' + utility.convertToFigure(item.TotalBalance, true) + '</td><td>' + utility.convertToFigure(item.Age0to30, true) + '</td><td>' + utility.convertToFigure(item.Age31to60, true) + '</td><td>' + utility.convertToFigure(item.Age61to90, true) + '</td><td>' + utility.convertToFigure(item.Age91to120, true) + '</td><td>' + utility.convertToFigure(item.Age120onward, true) + '</td>');
                $("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientOutstanding_Result #dgvPatientOutstanding tbody").last().append($row);
            });


        }
        else {
            //$("#" + Patient_Ledger.params["PanelID"] + " #divPatientOutstandingPaging").css("display", "none");
            $("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientOutstanding_Result #dgvPatientOutstanding").DataTable({
                "language": {
                    "emptyTable": "No Record Found"
                }, "autoWidth": false, "bLengthChange": false, "bInfo": false,"bPaginate": false, "bFilter": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientOutstanding_Result #dgvPatientOutstanding"))
            ;
        else
            $("#" + Patient_Ledger.params["PanelID"] + " #pnlPatientOutstanding_Result #dgvPatientOutstanding").DataTable({

                "bInfo": false,
                "bPaginate": false,
                "bLengthChange": false,
                "autoWidth": false,
                "iDisplayLength": 5,
                "bFilter": false,
                "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            }
            ); // to remove records per page dropdown






    },


    UnLoad: function () {
        if (Patient_Ledger.params != null && Patient_Ledger.params.ParentCtrl != null) {
            UnloadActionPan(Patient_Ledger.params.ParentCtrl, 'Patient_Ledger');
        }
        else
            UnloadActionPan(null, 'Patient_Ledger');
    },

    //--------------Pagination functions-----

    SelectedPageClick: function (PageNo, objPage) {
        // Change Background Color to Black for selected page
        $("#pnlPatientLedger_Result li").each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });

        Patient_Ledger.LoadCharges(PageNo, 5);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#pnlPatientLedger_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {


            Patient_Ledger.LoadCharges(currentPageNo, 5);


        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#pnlPatientLedger_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });
        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo > 0) {



            Patient_Ledger.LoadCharges(currentPageNo, 5);

        }
    },


    //---------------Open Pagination-------

    SaveAccountNote: function () {
        var AccountNote = "";
        AccountNote = $("#pnlPatientLedger #txtAccountComments").val();
        if (AccountNote.trim() != "") {
            Patient_Ledger.SaveAccountNote_DBCall().done(function (response) {
                if (response.status != false) {
                    $("#pnlPatientLedger #txtAccountCommentsLoad").val(response.AccountNoteComments);
                    $("#pnlPatientLedger #txtAccountComments").val("");
                    utility.DisplayMessages(response.Message, 1);
                } else {
                    utility.DisplayMessages(response.Message, 2);
                }


            });
        }
    },

    SaveAccountNote_DBCall: function () {
        var AccountNote = "";
        var PatientId = "";
        PatientId = Patient_Ledger.params.patientID
        AccountNote = $("#pnlPatientLedger #txtAccountComments").val();
        var data = "AccountNote=" + AccountNote + "&PatientId=" + PatientId;
        return MDVisionService.defaultService(data, "PATIENT_LEDGER", "SAVE_ACCOUNT_NOTE");
    },

    OpenAccountNotesLog: function () {

        var params = [];
        params["ParentCtrl"] = "Patient_Ledger";
        params["patientID"] = Patient_Ledger.params.patientID;
        LoadActionPan('Activity_LogNotes', params);

    },
}