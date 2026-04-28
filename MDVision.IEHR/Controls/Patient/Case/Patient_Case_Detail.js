Patient_Case_Detail = {
    params: [],
    Load: function (params) {
        Patient_Case_Detail.params = params;

        var self = null;
        if (Patient_Case_Detail.params.PanelID != "pnlPatientCaseDetail")
            self = $('#' + Patient_Case_Detail.params["PanelID"] + " #pnlPatientCaseDetail");
        else
            self = $('#pnlPatientCaseDetail');
        self.loadDropDowns(true).done(function () {

            if (Patient_Case_Detail.params.PatientId == "") {
                Patient_Case_Detail.params.PatientId = "-1";
            }
            else {
                // Begin 13-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3083
                CacheManager.BindDropDownsByID("#" + Patient_Case_Detail.params["PanelID"] + ' #ddlPatientInsuranceId', 'GetPatientInsurance', true, Patient_Case_Detail.params.PatientId);
                // Begin 13-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3083
            }
            Patient_Case_Detail.LoadCase();
            if (Patient_Case_Detail.params.mode == "Add") {
                Patient_Case_Detail.ValidateAddCase();
            }
            else if (Patient_Case_Detail.params.mode == "Edit") {
                // var liUBDetails = $('#' + Patient_Case_Detail.params["PanelID"] + " ul li#liUBDetails");
                //if (liUBDetails.attr("class") == "disableAll") {
                //    liUBDetails.removeAttr("class");
                //}
                //if ($('#' + Patient_Case_Detail.params["PanelID"] + " ul li#liWCDetails").hasClass("disableAll")) {
                //    $('#' + Patient_Case_Detail.params["PanelID"] + " ul li#liWCDetails").removeClass("disableAll")
                //}
                Patient_Case_Detail.ValidateEditCase();

            }
            //Patient_Case_Detail.ValidateCase();
            Patient_Case_Detail.LoadAllAutocomplete();
            $('#frmPatientCaseDetail').data('serialize', $('#frmPatientCaseDetail').serialize());
        });
        $("#" + Patient_Case_Detail.params["PanelID"] + " #liWCDetails").on("click", function (e) {
            if (Patient_Case_Detail.params.mode == "Edit") {
                Patient_Case_Detail.FillWCNFDetail(Patient_Case_Detail.params.CaseId).done(function (response) {
                    if (response.status != false) {
                        // $("#" + Patient_Case.params.PanelID + " #dvControls").removeClass("disableAll");
                        utility.CreateDatePicker(Patient_Case.params.PanelID + ' #frmPatientCaseDetail #dtpOccurrenceDate1,#dtpOccurrenceDate2,#dtpOccurrenceDate3,#dtpOccurrenceDate4,#dtpInjuryDate,#dtpHCFAField16DateFrom,#dtpHCFAField16DateTo,#dtpHCFAField18DateFrom,#dtpHCFAField18DateTo', function (ev) { }, false, "frmPatientCaseDetail", false);

                        var case_detail = JSON.parse(response.CaseWCNF_Fill_JSON);
                        var self = $("#frmPatientCaseDetail");
                        utility.bindMyJSON(true, case_detail, false, self).done(function () {
                            //if (case_detail.chkActive == 'True')
                            //    $("#frmPatientCaseDetail #chkActive").attr("checked", true);
                            //else
                            //    $("#frmPatientCaseDetail #chkActive").attr("checked", false);

                            //serialize data
                            $('#frmPatientCaseDetail').data('serialize', $('#frmPatientCaseDetail').serialize());
                        });

                    }
                });
            }
            if ($("#" + Patient_Case_Detail.params["PanelID"] + " #claimDetail").is(":visible")) {
                $("#" + Patient_Case_Detail.params["PanelID"] + " #claimDetail").hide();
            }
        });
        $("#" + Patient_Case_Detail.params["PanelID"] + " #liCaseDetails, #liUBDetails").on("click", function (e) {
            if (!$("#" + Patient_Case_Detail.params["PanelID"] + " #claimDetail").is(":visible")) {
                $("#" + Patient_Case_Detail.params["PanelID"] + " #claimDetail").show();
            }
        });
        $("#" + Patient_Case_Detail.params["PanelID"] + " #frmPatientCaseDetail #ddlCaseType").on("change", function (e) {
            if ($("#" + Patient_Case_Detail.params["PanelID"] + " #frmPatientCaseDetail #ddlCaseType option:selected").text().toLowerCase() == "wc/nf") {
                if (!$('#' + Patient_Case_Detail.params["PanelID"] + " #frmPatientCaseDetail #txtHospitalCaseNo").prop("disabled")) {
                    $('#' + Patient_Case_Detail.params["PanelID"] + " #frmPatientCaseDetail #txtHospitalCaseNo").prop("disabled", true);
                }
                if (!$('#' + Patient_Case_Detail.params["PanelID"] + " #frmPatientCaseDetail #txtProvider").prop("disabled")) {
                    $('#' + Patient_Case_Detail.params["PanelID"] + " #frmPatientCaseDetail #txtProvider").prop("disabled", true);
                }
            }
            else {
                if ($('#' + Patient_Case_Detail.params["PanelID"] + " #frmPatientCaseDetail #txtHospitalCaseNo").prop("disabled")) {
                    $('#' + Patient_Case_Detail.params["PanelID"] + " #frmPatientCaseDetail #txtHospitalCaseNo").prop("disabled", false);
                }
                if ($('#' + Patient_Case_Detail.params["PanelID"] + " #frmPatientCaseDetail #txtProvider").prop("disabled")) {
                    $('#' + Patient_Case_Detail.params["PanelID"] + " #frmPatientCaseDetail #txtProvider").prop("disabled", false);
                }
            }

        });
    },

    ShowHideEditLinks: function () {
        var self = null;
        if (Patient_Case_Detail.params.PanelID != 'pnlPatientCaseDetail')
            self = $('#' + Patient_Case_Detail.params.PanelID + ' #pnlPatientCaseDetail');
        else
            self = $('#' + Patient_Case_Detail.params.PanelID);

        // Facility
        if (self.find("#hfFacility").val() != "") {
            self.find("#lnkFacilityEdit").css("display", "inline");
            self.find("#lblFacility").css("display", "none");
        }
        else {
            self.find("#lnkFacilityEdit").css("display", "none");
            self.find("#lblFacility").css("display", "inline");
        }

        //Provider
        if (self.find("#hfProvider").val() != "") {
            self.find("#lnkProviderEdit").css("display", "inline");
            self.find("#lblProvider").css("display", "none");
        }
        else {
            self.find("#lnkProviderEdit").css("display", "none");
            self.find("#lblProvider").css("display", "inline");
        }

        // Refering Provider

        if (self.find("#hfRefProvider").val() != "") {
            self.find("#lnkRefProviderEdit").css("display", "inline");
            self.find("#lblRefProvider").css("display", "none");
        }
        else {
            self.find("#lnkRefProviderEdit").css("display", "none");
            self.find("#lblRefProvider").css("display", "inline");
        }
    },

    LoadAllAutocomplete: function () {
        var self = null;
        if (Patient_Case_Detail.params.PanelID != 'pnlPatientCaseDetail')
            self = $('#' + Patient_Case_Detail.params.PanelID + ' #pnlPatientCaseDetail');
        else
            self = $('#' + Patient_Case_Detail.params.PanelID);

        CacheManager.BindCodes('GetInsurancePlan', false).done(function (result) {
            self.find("input#txtInsurancePlan").autocomplete({
                autoFocus: true,
                source: InsurancePlans, // pass an array (without a comma)
                select: function (event, ui) {
                    setTimeout(function () {
                        self.find("#hfInsurancePlan").val(ui.item.id); // add the selected id
                        if (self.find("#lnkInsurancePlanDetail").css("display") == "none") {
                            self.find("#lnkInsurancePlanDetail").css("display", "inline");
                            self.find("#lblInsurancePlan").css("display", "none");
                        }
                    }, 100);
                }
            });
            $('#frmPatientCaseDetail').data('serialize', $('#frmPatientCaseDetail').serialize());
        });

        CacheManager.BindCodes('GetCaseAdjuster', false).done(function (result) {
            var Ctrl = self.find("input#txtCaseAdjuster");
            var hfCtrl = $("#" + Patient_Case.params["PanelID"] + " #hfCaseAdjusterId");
            var onSelect = function (e) {
                Patient_CaseAdjuster.params["IsFromParentCtrl"] = "1";
                Patient_CaseAdjuster.params["ParentPanelId"] = "pnlPatientCaseDetail"
                Patient_CaseAdjuster.params["ParentFormId"] = "frmPatientCaseDetail";
                Patient_CaseAdjuster.FillCaseAdjusterInfo(e.id);
            }
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", CaseAdjuster, null, hfCtrl, onSelect);
        });
        Patient_Case_Detail.BindFacility();
        Patient_Case_Detail.BindRefProvider();
        Patient_Case_Detail.BindProvider();
    },


    BindFacility: function () {
        var Ctrl = $("#" + Patient_Case_Detail.params.PanelID + " #frmPatientCaseDetail #txtFacility");
        var func = function () { return utility.GetFacilityArray(Ctrl.val()) };
        var hfCtrl = $("#" + Patient_Case_Detail.params.PanelID + " #frmPatientCaseDetail #hfFacility");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    BindProvider: function () {
        var Ctrl = $("#" + Patient_Case_Detail.params.PanelID + " #frmPatientCaseDetail #txtProvider");
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        var hfCtrl = $("#" + Patient_Case_Detail.params.PanelID + " #frmPatientCaseDetail #hfProvider");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    BindRefProvider: function () {
        var Ctrl = $('#pnlPatientCaseDetail #txtRefProvider');
        var hfCtrl = $("#pnlPatientCaseDetail #hfRefProvider");
        var func = function () { return utility.GetRefProviderArray(Ctrl.val()) };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    OpenInsurancePlan: function () {
        var params = [];
        params["InsurancePlanId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Patient_Case_Detail";
        LoadActionPan('Admin_InsurancePlan', params);
    },

    OpenInsurancePlanDetail: function () {
        var params = [];
        params["InsurancePlanId"] = $('#' + Patient_Case_Detail.params["PanelID"] + " #hfInsurancePlan").val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Patient_Case_Detail";
        LoadActionPan('insurancePlanDetail', params);
    },

    FillInsurancePlanName: function (InsurancePlanId, InsurancePlanName) {
        $('#' + Patient_Case_Detail.params["PanelID"] + " #txtInsurancePlan").val(InsurancePlanName);
        $('#' + Patient_Case_Detail.params["PanelID"] + " #hfInsurancePlan").val(InsurancePlanId);
        $('#' + Patient_Case_Detail.params["PanelID"] + " #lnkInsurancePlanDetail").css("display", "inline");
        $('#' + Patient_Case_Detail.params["PanelID"] + " #lblInsurancePlan").css("display", "none");
        UnloadActionPan(Admin_InsurancePlan.params["ParentCtrl"]);
        //Patient_Insurance.FillInsurancePlanAddress(InsurancePlanId);
    },

    OpenFacility: function () {
        var params = [];
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Patient_Case_Detail";
        LoadActionPan('Admin_Facility', params);
    },

    OpenFacilityDetail: function () {
        //Admin_Facility.FacilityEdit($('#' + demographicDetail.params.PanelID + ' #hfFacility').val(), "demographicDetail");
        var params = [];
        params["FacilityId"] = $('#' + Patient_Case.params.PanelID + ' #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacility";
        params["ParentCtrl"] = 'Patient_Case_Detail';
        LoadActionPan('facilityDetail', params);
    },

    OpenRefProvider: function () {
        var params = [];
        params["ReferringProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtRefProvider";
        params["ParentCtrl"] = "Patient_Case_Detail";
        LoadActionPan('Admin_ReferringProvider', params);
    },

    OpenRefProviderDetail: function () {
        //Admin_ReferringProvider.ReferringProviderEdit($('#pnlDemographic #hfRefProvider').val(), "patTabDemographic", "txtRefProvider");
        var params = [];
        params["ReferringProviderId"] = $('#' + Patient_Case.params.PanelID + ' #hfRefProvider').val();
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtRefProvider";
        params["mode"] = "Edit";
        params["ParentCtrl"] = "Patient_Case_Detail";
        LoadActionPan('referringproviderDetail', params);
    },

    OpenProvider: function () {
        var params = [];
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Patient_Case_Detail";
        LoadActionPan('Admin_Provider', params);
    },

    OpenProviderDetail: function () {
        //Admin_Provider.ProviderEdit($('#' + demographicDetail.params.PanelID + ' #hfProvider').val(), "demographicDetail");
        var params = [];
        params["ProviderId"] = $('#' + Patient_Case.params["PanelID"] + ' #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'Patient_Case_Detail';
        LoadActionPan('providerDetail', params);
    },

    OpenPatientCharges: function () {

        var params = [];
        params["FromAdmin"] = 0;
        params["PatientAccountNo"] = Patient_Demographic.params.PatientAccountNo;
        params["PatientID"] = Patient_Demographic.params.patientID;
        params["ParentCtrl"] = 'Patient_Case_Detail';
        params["CaseId"] = Patient_Case_Detail.params.CaseId;
        params["CaseNo"] = $("#" + Patient_Case.params.PanelID + " #hfCaseNumber").val();
        LoadActionPan('Bill_ChargeSearch', params);

        //var params = [];
        //params["FromAdmin"] = 0;
        //params["ParentCtrl"] = 'Patient_Case_Detail';
        //params["PatientAccountNo"] = Patient_Demographic.params.PatientAccountNo;
        //params["patientID"] = Patient_Demographic.params.patientID;

        //LoadActionPan('Encounter_Visits', params);
    },

    OpenChargeCapture: function () {


        var params = [];
        params["FromAdmin"] = 0;
        params["CaseId"] = Patient_Case_Detail.params.CaseId;
        params["CaseNo"] = $("#" + Patient_Case.params.PanelID + " #hfCaseNumber").val();
        params["ParentCtrl"] = 'Patient_Case_Detail';
        params['mode'] = "Edit";
        LoadActionPan('EncounterChargeCapture', params);

    },

    FillVisitFromSearch: function (PatientId, VisitId, event) {

        if (event != null) {
            event.stopPropagation();
        }

        AppPrivileges.GetFormPrivileges("Demographic", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Patient_Case_Detail.UpdatePatientVisit(VisitId).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        Patient_Case_Detail.LoadClaims(Patient_Case_Detail.params.CaseId);
                        Bill_ChargeSearch.UnLoad();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else {
                utility.DisplayMessages(strMessage, 3);
            }
        });
    },
    FillWCNFDetail: function (CaseID) {
        if (CaseID == null) {
            CaseID = 0;
        }
        var data = "CaseID=" + CaseID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_CASE", "FILL_WCNF_DETAIL");
    },
    LoadCase: function () {
        AppPrivileges.GetFormPrivileges("Demographic", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (Patient_Case_Detail.params.mode == "Add") {
                    $('#' + Patient_Case_Detail.params["PanelID"] + " #btnAddDocument").attr("disabled", true);
                    $('#' + Patient_Case_Detail.params["PanelID"] + " #btnViewDocument").attr("disabled", true);
                    $('#frmPatientCaseDetail').data('serialize', $('#frmPatientCaseDetail').serialize());
                    utility.CreateDatePicker(Patient_Case.params.PanelID + ' #frmPatientCaseDetail #dtpOccurrenceDate1,#dtpOccurrenceDate2,#dtpOccurrenceDate3,#dtpOccurrenceDate4,#dtpInjuryDate,#dtpHCFAField16DateFrom,#dtpHCFAField16DateTo,#dtpHCFAField18DateFrom,#dtpHCFAField18DateTo', function (ev) { }, false, "frmPatientCaseDetail", false);
                    utility.ValidateFromToDate('frmPatientCaseDetail', 'dtpHCFAField16DateFrom', 'dtpHCFAField16DateTo', true);
                    utility.ValidateFromToDate('frmPatientCaseDetail', 'dtpHCFAField18DateFrom', 'dtpHCFAField18DateTo', true);
                }
                else if (Patient_Case_Detail.params.mode == "Edit") {
                    Patient_Case_Detail.FillCase(Patient_Case_Detail.params.CaseId).done(function (response) {
                        if (response.status != false) {
                            $("#" + Patient_Case.params.PanelID + " #dvControls").removeClass("disableAll");
                            Patient_Case_Detail.LoadClaims(Patient_Case_Detail.params.CaseId);
                            utility.CreateDatePicker(Patient_Case.params.PanelID + ' #frmPatientCaseDetail #dtpOccurrenceDate1,#dtpOccurrenceDate2,#dtpOccurrenceDate3,#dtpOccurrenceDate4,#dtpInjuryDate,#dtpHCFAField16DateFrom,#dtpHCFAField16DateTo,#dtpHCFAField18DateFrom,#dtpHCFAField18DateTo', function (ev) { }, false, "frmPatientCaseDetail", false);
                            utility.ValidateFromToDate('frmPatientCaseDetail', 'dtpHCFAField16DateFrom', 'dtpHCFAField16DateTo', true);
                            utility.ValidateFromToDate('frmPatientCaseDetail', 'dtpHCFAField18DateFrom', 'dtpHCFAField18DateTo', true);
                            var case_detail = JSON.parse(response.CaseFill_JSON);
                            var wcnf_detail = JSON.parse(response.CaseFill_WCNF);
                            var self = $("#frmPatientCaseDetail");
                            utility.bindMyJSON(true, case_detail, false, self).done(function () {

                                Patient_Case_Detail.ShowHideEditLinks();
                                if (case_detail.chkActive == 'True')
                                    $("#frmPatientCaseDetail #chkActive").attr("checked", true);
                                else
                                    $("#frmPatientCaseDetail #chkActive").attr("checked", false);

                                if (wcnf_detail != null) {
                                    utility.bindMyJSON(true, wcnf_detail, false, self).done(function () {
                                        //serialize data
                                        $('#frmPatientCaseDetail #WCDetails').data('serialize', $('#frmPatientCaseDetail #WCDetails').serialize());
                                    });
                                }
                                var liUBDetails = $('#' + Patient_Case_Detail.params["PanelID"] + " ul li#liUBDetails");
                                if ($('#' + Patient_Case_Detail.params["PanelID"] + " #frmPatientCaseDetail #ddlCaseType option:selected").text().toLowerCase() == "wc/nf") {
                                    if ($('#' + Patient_Case_Detail.params["PanelID"] + " ul li#liWCDetails").hasClass("disableAll")) {
                                        $('#' + Patient_Case_Detail.params["PanelID"] + " ul li#liWCDetails").removeClass("disableAll");
                                    }
                                    //
                                    if (!$('#' + Patient_Case_Detail.params["PanelID"] + " #frmPatientCaseDetail #txtHospitalCaseNo").prop("disabled")) {
                                        $('#' + Patient_Case_Detail.params["PanelID"] + " #frmPatientCaseDetail #txtHospitalCaseNo").prop("disabled", true);
                                    }
                                    if (!$('#' + Patient_Case_Detail.params["PanelID"] + " #frmPatientCaseDetail #txtProvider").prop("disabled")) {
                                        $('#' + Patient_Case_Detail.params["PanelID"] + " #frmPatientCaseDetail #txtProvider").prop("disabled", true);
                                    }

                                }
                                else {
                                    if (liUBDetails.attr("class") == "disableAll") {
                                        liUBDetails.removeAttr("class");

                                    }
                                    if ($('#' + Patient_Case_Detail.params["PanelID"] + " #frmPatientCaseDetail #txtHospitalCaseNo").prop("disabled")) {
                                        $('#' + Patient_Case_Detail.params["PanelID"] + " #frmPatientCaseDetail #txtHospitalCaseNo").prop("disabled", false);
                                    }
                                    if ($('#' + Patient_Case_Detail.params["PanelID"] + " #frmPatientCaseDetail #txtProvider").prop("disabled")) {
                                        $('#' + Patient_Case_Detail.params["PanelID"] + " #frmPatientCaseDetail #txtProvider").prop("disabled", false);
                                    }

                                }
                                if (parseInt(response.DocumentCount) == 0)
                                { $('#' + Patient_Case_Detail.params["PanelID"] + " #btnViewDocument").attr("disabled", true); }
                                else {
                                    $('#' + Patient_Case_Detail.params["PanelID"] + " #btnViewDocument").attr("disabled", false);
                                }

                                $('#' + Patient_Case_Detail.params["PanelID"] + " #btnAddDocument").attr("disabled", false);
                                Patient_Case_Detail.params["CaseNo"] = case_detail.hfCaseNumber;
                                //serialize data
                                $('#frmPatientCaseDetail').data('serialize', $('#frmPatientCaseDetail').serialize());
                            });
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }

                    });
                }
            }
            else {
                utility.DisplayMessages(strMessage, 3);
            }
        });
    },

    LoadClaims: function (caseId) {
        Patient_Case_Detail.ClaimsLoad(caseId).done(function (response) {
            if (response.status != false) {
                if ($("#" + Patient_Case_Detail.params["PanelID"] + " #pnlPatientCaseDetail_Result").css("display") == "none") {
                    $("#" + Patient_Case_Detail.params["PanelID"] + " #pnlPatientCaseDetail_Result").show();
                }
                //var isnurance_detail = JSON.parse(response.VisitsLoad_JSON);
                Patient_Case_Detail.ClaimGridLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    ClaimGridLoad: function (response) {
        if ($.fn.dataTable.isDataTable("#" + Patient_Case_Detail.params["PanelID"] + " #pnlPatientCaseDetail_Result #dgvPatientCaseDetail"))
            $("#" + Patient_Case_Detail.params["PanelID"] + " #pnlPatientCaseDetail_Result #dgvPatientCaseDetail").dataTable().fnDestroy();
        $("#" + Patient_Case_Detail.params["PanelID"] + " #pnlPatientCaseDetail_Result #dgvPatientCaseDetail tbody").find("tr").remove();
        if (response.VisitsCount > 0) {
            var CaseLoadJSONData = JSON.parse(response.VisitsLoad_JSON);
            $.each(CaseLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#dgvPatientCaseDetail_row" + item.VisitId + "'))");
                $row.attr("id", "dgvPatientCaseDetail_row" + item.VisitId);
                $row.attr("VisitId", item.VisitId);

                if (item.IsActive == "True") {
                    isactive = 0;
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    tglclass = "fa fa-toggle-on red";
                }

                var EditMethod = ""; //"Patient_Case.CaseAddEdit(" + item.VisitId.trim() + ",'Edit');";
                var ActiveInacvtiveMethod = "";// "Patient_Case.ActiveInactiveCase(" + item.VisitId.trim() + "," + isactive + ");";
                var submitted = "No";
                if (item.SubmittedDate != "") {
                    submitted = "Yes";
                }
                $row.append('<td style="display:none;">' + item.VisitId + '</td><td style="display:none;"><a class="btn  btn-xs" href="#" onclick="Patient_Case.DeletePatientCase(' + item.VisitId + ');" title="Delete Record"><i class="' + tglclass + '"></i></a><a class="btn btn-xs" href="#" onclick="' + EditMethod + '"  title="Edit Record"><i class="fa fa-edit black"></i></a><a class="btn  btn-xs" href="#" onclick="' + ActiveInacvtiveMethod + '" title="Inactive Record"><i class="' + tglclass + '"></i></a>&nbsp;</td><td>' + item.ClaimNumber + '</td><td>' + submitted + '</td><td>' + utility.convertToFigure(item.Charges, true) + '</td><td>' + utility.convertToFigure(item.ClaimBalance, true) + '</td>');

                $("#" + Patient_Case_Detail.params["PanelID"] + " #pnlPatientCaseDetail_Result #dgvPatientCaseDetail tbody").last().append($row);
            });
        }
        else {
            if (!$("#" + Patient_Case_Detail.params["PanelID"] + " #pnlPatientCaseDetail_Result #dgvPatientCaseDetail").parent().parent().hasClass("dataTables_wrapper")) {
                $("#" + Patient_Case_Detail.params["PanelID"] + " #pnlPatientCaseDetail_Result #dgvPatientCaseDetail").DataTable({
                    "language": {
                        "emptyTable": "No Claim Found"
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
            }
            else {
                var $row = $('<tr/>');
                $row.append('<td colspan="6" class="center" >No Claim Found</td>');
                $(gridId + " tbody").last().append($row);
            }
        }
        if ($.fn.dataTable.isDataTable("#" + Patient_Case_Detail.params["PanelID"] + " #pnlPatientCaseDetail_Result #dgvPatientCaseDetail") || $("#" + Patient_Case_Detail.params["PanelID"] + " #pnlPatientCaseDetail_Result #dgvPatientCaseDetail").parent().parent().hasClass("dataTables_wrapper"))
            ;
        else
            $("#" + Patient_Case_Detail.params["PanelID"] + " #pnlPatientCaseDetail_Result #dgvPatientCaseDetail").DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    CaseSave: function () {
        var strMessage = "";
        var self = $("#frmPatientCaseDetail");
        var myJSON = self.getMyJSON();
        if (Patient_Case_Detail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Demographic", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Patient_Case_Detail.SaveCase(myJSON).done(function (response) {
                        if (response.status != false) {
                            Patient_Case.SearchPatientCase();
                            //Patient_Case.CaseSearch(response.CaseId);
                            Patient_Case_Detail.params.CaseId = response.CaseId;
                            Patient_Case_Detail.LoadClaims(Patient_Case_Detail.params.CaseId);
                            utility.DisplayMessages(response.message, 1);
                            Patient_Case_Detail.params.mode = "Edit";
                            Patient_Case_Detail.LoadCase();
                            //     var liUBDetails = $('#' + Patient_Case_Detail.params["PanelID"] + " ul li#liUBDetails");
                            //$('#pnlPatientCase #hfProvider').val("");
                            //$('#pnlPatientCase #hfFacility').val("");
                            //$('#pnlPatientCase #hfInsurancePlan').val("");
                            var liUBDetails = $('#' + Patient_Case_Detail.params["PanelID"] + " ul li#liUBDetails");
                            if ($('#' + Patient_Case_Detail.params["PanelID"] + " #frmPatientCaseDetail #ddlCaseType option:selected").text().toLowerCase() == "wc/nf") {
                                if ($('#' + Patient_Case_Detail.params["PanelID"] + " ul li#liWCDetails").hasClass("disableAll")) {
                                    $('#' + Patient_Case_Detail.params["PanelID"] + " ul li#liWCDetails").removeClass("disableAll");
                                    $('#' + Patient_Case_Detail.params["PanelID"] + " ul li").each(function (i, item) {
                                        $(this).removeClass("active");
                                    });
                                    $('#' + Patient_Case_Detail.params["PanelID"] + ' #ulTabs a[href="#WCDetails"]').tab('show');
                                    if ($("#" + Patient_Case_Detail.params["PanelID"] + " #claimDetail").is(":visible")) {
                                        $("#" + Patient_Case_Detail.params["PanelID"] + " #claimDetail").hide();
                                    }
                                }
                            }
                            else {
                                if (liUBDetails.attr("class") == "disableAll") {
                                    liUBDetails.removeAttr("class");
                                    $('#' + Patient_Case_Detail.params["PanelID"] + ' #ulTabs a[href="#UBDetails"]').tab('show');
                                }
                            }

                            Patient_Case_Detail.ValidateEditCase();
                            //UnloadActionPan(Patient_Case_Detail.params["ParentCtrl"]);
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
        else if (Patient_Case_Detail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Demographic", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Patient_Case_Detail.UpdateCase(myJSON, Patient_Case_Detail.params.CaseId).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            if (Patient_Case_Detail.params.RefCtrl != null && Patient_Case_Detail.params.RefCtrl != "") {
                                UnloadActionPan(Patient_Case_Detail.params["ParentCtrl"]);
                            }
                            else {
                                Patient_Case.SearchPatientCase(Patient_Case_Detail.params.CaseId);
                                UnloadActionPan(Patient_Case_Detail.params["ParentCtrl"]);
                            }
                            if (Patient_Case_Detail.params["ParentCtrl"] == "EncounterChargeCapture") {
                                EncounterChargeCapture.FillCaseDetail(Patient_Case_Detail.params.CaseNo);
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
        $("#pnlPatientCase #hfProvider,#hfRefProvider,#hfFacility,#hfInsurancePlan").val("");
    },

    ValidateAddCase: function () {
        $('#frmPatientCaseDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   CaseType: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   // Begin 18-Jan-2016  Added By Azeem Raza Tayyab Bug # PMS-3397
                   OccurrenceDate1: {
                       group: '.col-xs-7',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   OccurrenceDate2: {
                       group: '.col-xs-7',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   PatientStatus: {
                       group: '.col-xs-7',
                       enabled: true,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },

                   FrequencyCode: {
                       group: '.col-xs-7',
                       enabled: true,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   OccurrenceDate3: {
                       group: '.col-xs-7',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   OccurrenceDate4: {
                       group: '.col-xs-7',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   ConditionCode1: {
                       group: '.col-xs-7',
                       enabled: true,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },

                   ValueCode1Amount: {
                       group: '.col-xs-7',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   ValueCode2Amount: {
                       group: '.col-xs-7',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   ValueCode3Amount: {
                       group: '.col-xs-7',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   ValueCode4Amount: {
                       group: '.col-xs-7',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   InjuryDate: {
                       group: '.col-sm-6',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   HCFAField16DateFrom: {
                       group: '.col-sm-9',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   HCFAField16DateTo: {
                       group: '.col-sm-9',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   HCFAField18DateFrom: {
                       group: '.col-sm-9',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   HCFAField18DateTo: {
                       group: '.col-sm-9',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
               }
           }).on('keyup', 'input#txtOccurrenceCode1,input#txtOccurrenceCode2,input#txtOccurrenceCode3,input#txtOccurrenceCode4,input#txtValueCode1,input#txtValueCode2,input#txtValueCode3,input#txtValueCode4', function (e) {
               var formValidation = $("#frmPatientCaseDetail").data("bootstrapValidator");
               switch ($(this).attr("name")) {
                   case 'OccurrenceCode1':
                       var OccurenceCod1Val = $("input#txtOccurrenceCode1").val();
                       if (OccurenceCod1Val != "") {
                           //formValidation.enableFieldValidators('OccurrenceDate1', true);//.revalidateField('OccurrenceDate1');
                           formValidation.enableFieldValidators('OccurrenceDate1', true);
                       }
                       else
                           formValidation.enableFieldValidators('OccurrenceDate1', false);
                       break;
                   case 'OccurrenceCode2':
                       var OccurenceCod2Val = $("input#txtOccurrenceCode2").val();
                       if (OccurenceCod2Val != "") {
                           formValidation.enableFieldValidators('OccurrenceDate2', true);
                       }
                       else
                           formValidation.enableFieldValidators('OccurrenceDate2', false);
                       break;
                   case 'OccurrenceCode3':
                       var OccurenceCod3Val = $("input#txtOccurrenceCode3").val();
                       if (OccurenceCod3Val != "") {
                           formValidation.enableFieldValidators('OccurrenceDate3', true);
                       }
                       else
                           formValidation.enableFieldValidators('OccurrenceDate3', false);
                       break;
                   case 'OccurrenceCode4':
                       var OccurenceCod4Val = $("input#txtOccurrenceCode4").val();
                       if (OccurenceCod4Val != "") {
                           formValidation.enableFieldValidators('OccurrenceDate4', true);
                       }
                       else
                           formValidation.enableFieldValidators('OccurrenceDate4', false);
                       break;
                   case 'ValueCode1':
                       var ValueCod1Val = $("input#txtValueCode1").val();
                       if (ValueCod1Val != "") {
                           formValidation.enableFieldValidators('ValueCode1Amount', true);
                       }
                       else
                           formValidation.enableFieldValidators('ValueCode1Amount', false);
                       break;
                   case 'ValueCode2':
                       var ValueCod2Val = $("input#txtValueCode2").val();
                       if (ValueCod2Val != "") {
                           formValidation.enableFieldValidators('ValueCode2Amount', true);
                       }
                       else
                           formValidation.enableFieldValidators('ValueCode2Amount', false);
                       break;
                   case 'ValueCode3':
                       var ValueCod3Val = $("input#txtValueCode3").val();
                       if (ValueCod3Val != "") {
                           formValidation.enableFieldValidators('ValueCode3Amount', true);
                       }
                       else
                           formValidation.enableFieldValidators('ValueCode3Amount', false);
                       break;
                   case 'ValueCode4':
                       var ValueCod4Val = $("input#txtValueCode4").val();
                       if (ValueCod4Val != "") {
                           formValidation.enableFieldValidators('ValueCode4Amount', true);
                       }
                       else
                           formValidation.enableFieldValidators('ValueCode4Amount', false);
                       break;
                   default:
                       break;
                       // End 18-Jan-2016  Added By Azeem Raza Tayyab Bug # PMS-3397
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            if (Patient_Case_Detail.params.mode != "Edit") {
                Patient_Case_Detail.CaseSave();
            }

        });
    },

    ValidateEditCase: function () {
        $('#frmPatientCaseDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   CaseType: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   OccurrenceDate1: {
                       group: '.col-xs-7',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   OccurrenceDate2: {
                       group: '.col-xs-7',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   PatientStatus: {
                       group: '.col-xs-7',
                       enabled: true,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },

                   FrequencyCode: {
                       group: '.col-xs-7',
                       enabled: true,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   OccurrenceDate3: {
                       group: '.col-xs-7',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   OccurrenceDate4: {
                       group: '.col-xs-7',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   ConditionCode1: {
                       group: '.col-xs-7',
                       enabled: true,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },

                   ValueCode1Amount: {
                       group: '.col-xs-7',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   ValueCode2Amount: {
                       group: '.col-xs-7',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   ValueCode3Amount: {
                       group: '.col-xs-7',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   ValueCode4Amount: {
                       group: '.col-xs-7',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   InjuryDate: {
                       group: '.col-sm-6',
                       //enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   HCFAField16DateFrom: {
                       group: '.col-sm-9',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   HCFAField16DateTo: {
                       group: '.col-sm-9',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   HCFAField18DateFrom: {
                       group: '.col-sm-9',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   HCFAField18DateTo: {
                       group: '.col-sm-9',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
               }
           })
        .on('keyup', 'input#txtOccurrenceCode1,input#txtOccurrenceCode2,input#txtOccurrenceCode3,input#txtOccurrenceCode4,input#txtValueCode1,input#txtValueCode2,input#txtValueCode3,input#txtValueCode4', function (e) {
            var formValidation = $("#frmPatientCaseDetail").data("bootstrapValidator");
            switch ($(this).attr("name")) {
                case 'OccurrenceCode1':
                    var OccurenceCod1Val = $("input#txtOccurrenceCode1").val();
                    if (OccurenceCod1Val != "") {
                        //formValidation.enableFieldValidators('OccurrenceDate1', true);//.revalidateField('OccurrenceDate1');
                        formValidation.enableFieldValidators('OccurrenceDate1', true);
                    }
                    else
                        formValidation.enableFieldValidators('OccurrenceDate1', false);
                    break;
                case 'OccurrenceCode2':
                    var OccurenceCod2Val = $("input#txtOccurrenceCode2").val();
                    if (OccurenceCod2Val != "") {
                        formValidation.enableFieldValidators('OccurrenceDate2', true);
                    }
                    else
                        formValidation.enableFieldValidators('OccurrenceDate2', false);
                    break;
                case 'OccurrenceCode3':
                    var OccurenceCod3Val = $("input#txtOccurrenceCode3").val();
                    if (OccurenceCod3Val != "") {
                        formValidation.enableFieldValidators('OccurrenceDate3', true);
                    }
                    else
                        formValidation.enableFieldValidators('OccurrenceDate3', false);
                    break;
                case 'OccurrenceCode4':
                    var OccurenceCod4Val = $("input#txtOccurrenceCode4").val();
                    if (OccurenceCod4Val != "") {
                        formValidation.enableFieldValidators('OccurrenceDate4', true);
                    }
                    else
                        formValidation.enableFieldValidators('OccurrenceDate4', false);
                    break;
                case 'ValueCode1':
                    var ValueCod1Val = $("input#txtValueCode1").val();
                    if (ValueCod1Val != "") {
                        formValidation.enableFieldValidators('ValueCode1Amount', true);
                    }
                    else
                        formValidation.enableFieldValidators('ValueCode1Amount', false);
                    break;
                case 'ValueCode2':
                    var ValueCod2Val = $("input#txtValueCode2").val();
                    if (ValueCod2Val != "") {
                        formValidation.enableFieldValidators('ValueCode2Amount', true);
                    }
                    else
                        formValidation.enableFieldValidators('ValueCode2Amount', false);
                    break;
                case 'ValueCode3':
                    var ValueCod3Val = $("input#txtValueCode3").val();
                    if (ValueCod3Val != "") {
                        formValidation.enableFieldValidators('ValueCode3Amount', true);
                    }
                    else
                        formValidation.enableFieldValidators('ValueCode3Amount', false);
                    break;
                case 'ValueCode4':
                    var ValueCod4Val = $("input#txtValueCode4").val();
                    if (ValueCod4Val != "") {
                        formValidation.enableFieldValidators('ValueCode4Amount', true);
                    }
                    else
                        formValidation.enableFieldValidators('ValueCode4Amount', false);
                    break;
                default:
                    break;

            }
        }).on('success.form.bv', function (e) {
            e.preventDefault();
            Patient_Case_Detail.CaseSave();
        });
    },

    SaveCase: function (CaseData) {
        var data = "CaseData=" + CaseData + "&PatientID=" + Patient_Case_Detail.params.PatientId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_CASE", "SAVE_CASE");
    },

    UpdateCase: function (CaseData, CaseID) {
        var data = "CaseData=" + CaseData + "&PatientID=" + Patient_Case_Detail.params.PatientId + "&CaseID=" + CaseID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_CASE", "UPDATE_CASE");
    },
    UpdatePatientVisit: function (VisitID) {
        var data = "VisitID=" + VisitID + "&CaseID=" + Patient_Case_Detail.params.CaseId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_CASE", "UPDATE_PATIENT_VISIT");
    },

    FillCase: function (CaseID) {
        if (CaseID == null) {
            CaseID = 0;
        }
        var data = "PatientID=" + Patient_Case_Detail.params.PatientId + "&CaseID=" + CaseID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_CASE", "FILL_CASE");
    },

    ClaimsLoad: function (CaseID) {
        var data = "CaseID=" + CaseID;//Encounter_Visits.params.patientID
        //var data = "PatientID=" + Patient_Case_Detail.params.PatientId + "&CaseID=" + CaseID;//Encounter_Visits.params.patientID
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_ENCOUNTER_VISITS", "SEARCH_CASE_VISITS");
    },
    UpdateCaseActiveInactive: function (CaseID, IsActive) {
        var data = "PatientID=" + Patient_Case_Detail.params.PatientId + "&CaseID=" + CaseID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_CASE", "UPDATE_CASE_ACTIVE_INACTIVE");
    },
    changeYesNoSelection: function (currentctrl, refCtrl1, refCtrl2) {
        var checkState = $(currentctrl).is(":checked");
        $(currentctrl).prop("checked", checkState);
        if (checkState == true) {
            $('#pnlPatientCaseDetail #frmPatientCaseDetail').find("#" + refCtrl1).prop("checked", false);
            $('#pnlPatientCaseDetail #frmPatientCaseDetail').find("#" + refCtrl2).prop("checked", false);
        }
    },
    OpenCaseAdjusterPanel: function () {
        var params = [];
        params["CaseAdjusterId"] = "-1";
        params["FromAdmin"] = "0";
        params["IsFromParentCtrl"] = "0";
        params["RefCtrl"] = "txtCaseAdjuster";
        params["ParentCtrl"] = "Patient_Case_Detail";
        params["ParentPanelId"] = "pnlPatientCaseDetail";
        params["ParentFormId"] = "frmPatientCaseDetail";
        LoadActionPan('Patient_CaseAdjuster', params);
    },
    AddCaseDocument: function () {
        var params = [];
        //params["PanelID"] = demographicDetail.params["PanelID"];
        params["PatientId"] = Patient_Case_Detail.params.PatientId;
        params["PatBanner"] = true;
        params["CaseId"] = Patient_Case_Detail.params["CaseId"];
        params["CaseNo"] = Patient_Case_Detail.params["CaseNo"];
        params["GridPatientDocument"] = "dgvPatientDocument";
        params["GridRevDocument"] = "dgvPatRevDocument";
        params["FromAdmin"] = "0";
        params["UserId"] = globalAppdata['AppUserId'];
        params["RefCtrl"] = "frmPatientCaseDetail";
        params["ParentCtrl"] = 'Patient_Case_Detail';
        LoadActionPan('Patient_Document', params);
    },
    OpenDocumentDetail: function () {
        var params = [];
        //params["PanelID"] = demographicDetail.params["PanelID"];
        params["PatientId"] = Patient_Case_Detail.params.PatientId;
        params["CaseId"] = Patient_Case_Detail.params.CaseId;
        params["PanelID"] = "pnlCaseDocument";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Patient_Case_Detail';
        LoadActionPan('Patient_Case_Document', params);
    },
    UnLoad: function () {

        utility.UnLoadDialog('frmPatientCaseDetail', function () {

            if (Patient_Case_Detail.params != null && Patient_Case_Detail.params.ParentCtrl != null) {
                //UnloadActionPan(Patient_Case_Detail.params.ParentCtrl, 'Patient_Case_Detail');
                UnloadActionPan(Patient_Case_Detail.params["ParentCtrl"], 'Patient_Case_Detail', null, Patient_Case_Detail.params.PanelID);
                $("#pnlPatientCase #hfProvider,#hfRefProvider,#hfFacility,#hfInsurancePlan").val("");
            }
            else {
                UnloadActionPan(null, 'Patient_Case_Detail');
                $("#pnlPatientCase #hfProvider,#hfFacility,#hfInsurancePlan").val("");
            }

        }, function () {

            if (Patient_Case_Detail.params != null && Patient_Case_Detail.params.ParentCtrl != null) {
                //UnloadActionPan(Patient_Case_Detail.params.ParentCtrl, 'Patient_Case_Detail');
                UnloadActionPan(Patient_Case_Detail.params["ParentCtrl"], 'Patient_Case_Detail', null, Patient_Case_Detail.params.PanelID);
                $("#pnlPatientCaseDetail #hfProvider,#hfRefProvider,#hfFacility,#hfInsurancePlan").val("");
            }
            else {
                UnloadActionPan(null, 'Patient_Case_Detail');
                $("#pnlPatientCase #hfProvider,#hfRefProvider,#hfFacility,#hfInsurancePlan").val("");
            }

        });


    },
}