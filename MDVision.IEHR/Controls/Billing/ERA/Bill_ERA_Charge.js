ERA_ChargeSearch = {
    params: [],
    bIsFirstLoad: true,
    //bVisitFirst: true,
    self: null,
    Load: function (params) {
        ERA_ChargeSearch.params = params;
        if (ERA_ChargeSearch.bIsFirstLoad) {
            ERA_ChargeSearch.LoadAllControls();
            ERA_ChargeSearch.bIsFirstLoad = false;
        }
        if (ERA_ChargeSearch.params.PanelID != "pnlBillERAChargeSearch") {
            ERA_ChargeSearch.self = $('#' + ERA_ChargeSearch.params.PanelID + ' #pnlBillERAChargeSearch');
            ERA_ChargeSearch.params.PanelID = ERA_ChargeSearch.params.PanelID + ' #pnlBillERAChargeSearch';
        }
        else
            ERA_ChargeSearch.self = $('#' + ERA_ChargeSearch.params.PanelID);

        if (ERA_ChargeSearch.params["data"]) {
            ERA_ChargeSearch.self.find('#txtClaimNumber').val(ERA_ChargeSearch.params.data["ClaimNumber"]);
            ERA_ChargeSearch.self.find('#txtCPT').val(ERA_ChargeSearch.params.data["CPT"]);
            ERA_ChargeSearch.self.find('#txtPatientName').val(ERA_ChargeSearch.params.data["AccountNo"]);
            ERA_ChargeSearch.self.find('#txtLastName').val(ERA_ChargeSearch.params.data["LastName"]);
            ERA_ChargeSearch.self.find('#txtFirstName').val(ERA_ChargeSearch.params.data["FirstName"]);
            ERA_ChargeSearch.self.find('#dpDOSfrm').val(ERA_ChargeSearch.params.data["DOSFrom"]);
            ERA_ChargeSearch.self.find('#dpDOSto').val(ERA_ChargeSearch.params.data["DOSTo"]);
        }

        ERA_ChargeSearch.Bill_ERAChargeSearch();
        //$('#' + ERA_ChargeSearch.params.PanelID + ' #frmERAChargeSearch').data('serialize');
    },

    LoadAllControls: function () {
        CacheManager.BindCodes('GetFacility', false).done(function (result) {
            var Ctrl = $("#frmERAChargeSearch #txtFacility");
            var hfCtrl = $("#" + ERA_ChargeSearch.params.PanelID + " #hfFacility");
            var onSelect = function (e) {
                $("#" + ERA_ChargeSearch.params.PanelID + " #txtPractice").val(e.Practice);
                $("#" + ERA_ChargeSearch.params.PanelID + " #hfPractice").val(e.PracticeId);
            };
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl, onSelect);
        });
        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            var Ctrl = $("#frmERAChargeSearch #txtProvider");
            var hfCtrl = $("#" + ERA_ChargeSearch.params.PanelID + " #hfProvider");
            var onSelect = function (e) { $("#" + ERA_ChargeSearch.params.PanelID + " #txtProvider").attr("ProviderId", e.id); };
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, onSelect);
        });
        CacheManager.BindCodes('GetInsurancePlan', false).done(function (result) {
            var Ctrl = $("#" + ERA_ChargeSearch.params.PanelID + " input#txtInsurancePlan");
            var hfCtrl = $("#" + ERA_ChargeSearch.params.PanelID + " #hfInsurancePlan");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", InsurancePlans, null, hfCtrl);
        });
        utility.ValidateFromToDate('frmERAChargeSearch', 'dpDOSfrm', 'dpDOSto', true);
        ERA_ChargeSearch.BindClaimNumber();
        ERA_ChargeSearch.BindPatientAccount();
    },

    BindClaimNumber: function () {
        var Ctrl = $("#" + ERA_ChargeSearch.params.PanelID + " #txtClaimNumber");
        var hfCtrl = $("#" + ERA_ChargeSearch.params.PanelID + " #hfVisitId");
        var func = function () { return ERA_ChargeSearch.GetClaimNumberArray(Ctrl.val()); };
        utility.BindKendoAutoComplete(Ctrl, 3, "ClaimNumber", "contains", null, func, hfCtrl);
    },

    GetClaimNumberArray: function (name) {
        var AllClaimsVisits = [];
        var dfd = new $.Deferred();
        Patient_Search.LoadClaimNumers(name).done(function (responseData) {
            if (responseData.status != false) {
                if (responseData.ClaimsCount > 0) {
                    var Claims = JSON.parse(responseData.ClaimsLoad_JSON);
                    $.each(Claims, function (i, item) {
                        AllClaimsVisits.push({ id: item.VisitId, value: item.ClaimNumber + ' - ( ' + item.AccountNumber + ' - ' + item.PatientName + ' )', PatientId: item.PatientId, DOSFrom: item.DOSFrom, PatientName: item.AccountNumber + ' - ' + item.PatientName, ClaimNumber: item.ClaimNumber });
                    });
                }
            }
            dfd.resolve(AllClaimsVisits);
        });
        return dfd.promise();
    },

    OpenEncounter: function (event) {
        if (event != null) {
            event.stopPropagation();
        }
        var params = [];
        params["FromAdmin"] = 0;
        params["ParentCtrl"] = 'ERA_ChargeSearch';
        if ($("#" + ERA_ChargeSearch.params.PanelID + " #txtPatientName").val().trim() == "")
            params["patientID"] = 0;
        else
            params["patientID"] = Number($('#' + ERA_ChargeSearch.params.PanelID + ' #hfPatientId').val());

        LoadActionPan('Bill_ChargeSearch', params);
        //$('#CloseVisits').remove();
        // $('#OpenVisits').remove();
        //if (ERA_ChargeSearch.bVisitFirst) {
        //    $($('body #OpenVisits')[0]).attr('id', 'OpenVisits1')
        //    $($('body #CloseVisits')[0]).attr('id', 'CloseVisits1');
        //    ERA_ChargeSearch.bVisitFirst = false;
        //}

    },
    FillClaimNumberFromSearch: function (ClaimNumber, AccountNumber, PatientName, PatientId, DOSFrom, VisitId) {
        $("#" + ERA_ChargeSearch.params.PanelID + " #txtClaimNumber").val(ClaimNumber);
        //$("#" + ERA_ChargeSearch.params.PanelID + " #dpDOSfrm").val(DOSFrom);
        //$("#" + ERA_ChargeSearch.params.PanelID + " #hfPatientId").val(PatientId);
        //$("#" + ERA_ChargeSearch.params.PanelID + " #txtPatientName").val(AccountNumber + ' - ' + PatientName);
        $("#" + ERA_ChargeSearch.params.PanelID + " #hfVisitId").val(VisitId);

        // UnloadActionPan("ERA_ChargeSearch");
        Encounter_Visits.UnLoad();
    },
    OpenPatientSearch: function (event) {
        if (event != null) {
            event.stopPropagation();
        }
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'ERA_ChargeSearch';
        LoadActionPan('Patient_Search', params);
    },
    FillPatientInfoFromSearch: function (PatientId, patFullName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        $("#" + ERA_ChargeSearch.params.PanelID + " #hfPatientId").val(PatientId);
        $("#" + ERA_ChargeSearch.params.PanelID + " #txtPatientName").val(patFullName);
        utility.SetKendoAutoCompleteSourceforValidate($("#" + ERA_ChargeSearch.params.PanelID + " #txtPatientName"), patFullName, $("#" + ERA_ChargeSearch.params.PanelID + " #hfPatientId"), PatientId);
        UnloadActionPan("ERA_ChargeSearch");
        utility.InsertRecentPatient(PatientId);
    },
    BindPatientAccount: function () {
        var Ctrl = $("#" + ERA_ChargeSearch.params.PanelID + " #txtPatientName");
        var hfCtrl = $("#" + ERA_ChargeSearch.params.PanelID + " #hfPatientId");
        var func = function () { return ERA_ChargeSearch.GetActivePatientsArray(Ctrl.val()); };
        var onSelect = function (e) { utility.InsertRecentPatient(e.id); };
        utility.BindKendoAutoComplete(Ctrl, 3, "value", "contains", null, func, hfCtrl, onSelect);
    },

    GetActivePatientsArray: function (name) {
        var AllPatients = [];
        var dfd = new $.Deferred();
        appointmentDetail.LoadActivePatients(name).done(function (responseData) {
            if (responseData.status != false) {
                if (responseData.PatientCount > 0) {
                    var PatientLoadJSONData = JSON.parse(responseData.PatientLoad_JSON);
                    $.each(PatientLoadJSONData, function (i, item) {
                        AllPatients.push({ id: item.PatientId, value: item.AccountNumber, FullAlias: item.AccountNumber + " - " + item.FullName });
                    });
                }
            }
            dfd.resolve(AllPatients);
        });
        return dfd.promise();
    },

    BindAutoComplete: function (element) {

        var hiddenCrtl = $("#frmERAChargeSearch #hfCPT");
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "ERA_ChargeSearch", null, true);

    },

    ERAChargeReset: function () {

        $('#' + ERA_ChargeSearch.params.PanelID + ' #frmERAChargeSearch').find('[data-plugin-datepicker]').each(function () {
            $(this).val("");
        });

        $('#' + ERA_ChargeSearch.params.PanelID + ' #frmERAChargeSearch .ERAChargeformControls_div').resetAllControls();

    },

    OpenInsurancePlan: function (event) {
        if (event != null) {
            event.stopPropagation();
        }
        var params = [];
        params["InsurancePlanId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'ERA_ChargeSearch';
        LoadActionPan('Admin_InsurancePlan', params);
    },
    OpenInsurancePlanDetail: function () {
        var params = [];
        params["InsurancePlanId"] = $("#" + ERA_ChargeSearch.params.PanelID + " #hfInsurancePlan").val();
        params["mode"] = "Edit";
        Admin_InsurancePlan.params["FromAdmin"] == "0";
        params["ParentCtrl"] = 'ERA_ChargeSearch';
        LoadActionPan('insurancePlanDetail', params);
    },
    FillInsurancePlanName: function (InsurancePlanId, InsurancePlanName) {
        $("#" + ERA_ChargeSearch.params.PanelID + " #txtInsurancePlan").val(InsurancePlanName);
        $("#" + ERA_ChargeSearch.params.PanelID + " #hfInsurancePlan").val(InsurancePlanId);
        $("#" + ERA_ChargeSearch.params.PanelID + " #lnkInsurancePlanDetail").css("display", "inline");
        $("#" + ERA_ChargeSearch.params.PanelID + " #lblInsurancePlan").css("display", "none");
        UnloadActionPan(Admin_InsurancePlan.params["ParentCtrl"]);

    },
    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmERAChargeSearch";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ERA_ChargeSearch";
        LoadActionPan('Admin_Facility', params);
    },
    OpenFacilityDetail: function () {
        var params = [];
        params["FacilityId"] = $('#' + ERA_ChargeSearch.params.PanelID + ' #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'ERA_ChargeSearch';
        params["RefCtrl"] = "txtFacility";
        LoadActionPan('facilityDetail', params);
    },
    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmERAChargeSearch";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ERA_ChargeSearch";
        LoadActionPan('Admin_Provider', params);
    },
    OpenProviderDetail: function () {
        var params = [];
        params["ProviderId"] = $('#' + ERA_ChargeSearch.params.PanelID + ' #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'ERA_ChargeSearch';
        LoadActionPan('providerDetail', params);
    },

    Bill_ERAChargeSearch: function (ChargeCapId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Charges", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#" + ERA_ChargeSearch.params["PanelID"] + " #pnlERACharge_Result").css("display") == "none") {
                    $("#" + ERA_ChargeSearch.params["PanelID"] + " #pnlERACharge_Result").show();
                }
                var self = $('#' + ERA_ChargeSearch.params["PanelID"]);

                var myJSON = self.getMyJSONByName();

                ERA_ChargeSearch.SearchBillCharge(myJSON, ChargeCapId, null, null).done(function (response) {
                    if (response.status != false) {
                        ERA_ChargeSearch.BillERAChargeGridLoad(response);
                        //$('#' + ERA_ChargeSearch.params.PanelID + ' #frmERAChargeSearch').data('serialize');
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    SearchBillCharge: function (BillChargeData, ChargeCapId, PageNumber, RowsPerPage) {

        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 1000 : RowsPerPage;

        var objData = JSON.parse(BillChargeData);

        objData["ChargeCapId"] = ChargeCapId;
        objData["PageNumber"] = PageNumber;
        objData["CommandType"] = "search_bill_charge";
        objData["RowsPerPage"] = RowsPerPage;

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Charges", "Charge");
    },

    BillERAChargeGridLoad: function (response) {
        $("#" + ERA_ChargeSearch.params["PanelID"] + " #dgvERACharge").dataTable().fnDestroy();
        $("#" + ERA_ChargeSearch.params["PanelID"] + " #pnlERACharge_Result #dgvERACharge tbody").find("tr").remove();
        if (response.BillChargeCount > 0) {
            var planPriorityClass = "";
            var claimTitle = "";

            var BillChargeLoadJSONData = JSON.parse(response.BillChargeLoad_JSON);
            $.each(BillChargeLoadJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", "gvBillCharge_row" + item.ChargeCapId);
                $row.attr("ChargeCapId", item.ChargeCapId);

                var selectBillCharge = "";
                var selectMethod = "";
                if (ERA_ChargeSearch.params["ParentCtrl"] == "ERADetail") {
                    selectMethod = "ERADetail.LinkERA('', '" + item.ChargeCapId + "', 'true' ,event,'" + item.PatientId + "');"
                    selectBillCharge = '&nbsp;<a class="btn  btn-xs" href="#" onclick="' + selectMethod + '" title="Select Record"><i class="fa fa-check"></i></a>';
                }
                else {
                    selectMethod = "Bill_ERACharge_Detail.LinkERACharge('" + item.ChargeCapId + "' ,'true' ,event,'" + item.PatientId + "');"
                    selectBillCharge = '&nbsp;<a class="btn  btn-xs" href="#" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check"></i></a>';
                }

                if (selectMethod != "") {
                    $row.attr("onclick", selectMethod + "utility.SelectGridRow($('#gvBillCharge_row" + item.ChargeCapId + "'));");
                }
                else {
                    $row.attr("onclick", "utility.SelectGridRow($('#gvBillCharge_row" + item.ChargeCapId + "'))");
                }

                /********************/
                if (item.IsPrimary == "False") {
                    planPriorityClass = "bg-info";
                    claimTitle = "Primary Claim";
                }

                else {
                    planPriorityClass = "";
                    claimTitle = "Non Primary Claim";
                }
                $row.addClass(planPriorityClass);
                /*******************/

                $row.append('<td style="display:none;">' + item.ChargeCapId + '</td><td>' + selectBillCharge + '</td> <td>' + item.AccountNumber + '</td> <td>' + item.PatientName + '</td>  <td>' + item.InsurancePlanName + '</td>  <td>' + item.CPTCode + '</td><td>' + utility.convertToFigure(item.InsBalance, true) + '</td><td title="' + claimTitle + '">' + item.ClaimNumber + '</td><td>' + utility.RemoveTimeFromDate(null, item.SubmittedDate) + '</td>');

                //if (item.IsVNC.toLowerCase() == "true") {
                //    $($row).removeClass("active").removeClass("bg-info");
                //    $($row).css("background", "#FF6A6A");
                //}
                if (item.IsVoided.toLowerCase() == "1") {
                    $($row).removeClass("active").removeClass("bg-info");
                    $($row).css("background", "#FFFFE0");
                }
                else if (item.IsVNC.toLowerCase() == "false") {
                    $($row).removeClass("active").removeClass("bg-info");
                    $($row).css("background", "#FF6A6A");
                }
                //else {
                //    $($row).removeClass("active").removeClass("bg-info");
                //    $($row).css("background", "#FF6A6A");
                //}

                $("#" + ERA_ChargeSearch.params["PanelID"] + " #pnlERACharge_Result #dgvERACharge tbody").last().append($row);
            });
        }
        else {
            $("#" + ERA_ChargeSearch.params["PanelID"] + " #dgvERACharge").DataTable({
                "language": {
                    "emptyTable": "No Charge Found"
                }, "autoWidth": false, "bLengthChange": false, "bFilter": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable("#" + ERA_ChargeSearch.params["PanelID"] + " #dgvERACharge"))
            ;
        else
            $("#" + ERA_ChargeSearch.params["PanelID"] + " #pnlERACharge_Result #dgvERACharge").DataTable({ "bLengthChange": false, "bFilter": false, "bSort": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        $("#pnlERACharge_Result div.table-responsive").css("overflow", "auto");
    },

    UnLoad: function () {
        //if ($('#frmERAChargeSearch').serialize() != $('#frmERAChargeSearch').data('serialize')) {
        //    utility.myConfirm('2', function () {
        //        if (ERA_ChargeSearch.params != null && ERA_ChargeSearch.params.ParentCtrl != null) {
        //            UnloadActionPan(ERA_ChargeSearch.params.ParentCtrl, "ERA_ChargeSearch");
        //        }
        //    }, function () { },
        //            '2'
        //    );
        //}
        //else {
        UnloadActionPan(ERA_ChargeSearch.params.ParentCtrl, "ERA_ChargeSearch");
        //}
    },

}