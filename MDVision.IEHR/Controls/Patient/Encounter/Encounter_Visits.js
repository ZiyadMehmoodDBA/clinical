Encounter_Visits = {
    bIsFirstLoad: true,
    params: [],
    Load: function (parameters) {
        Encounter_Visits.params = parameters;
        if (Encounter_Visits.bIsFirstLoad) {
            Encounter_Visits.bIsFirstLoad = false;
            if (Encounter_Visits.params.PanelID != null && Encounter_Visits.params.PanelID != 'pnlEncounter')
                Encounter_Visits.params.PanelID = Encounter_Visits.params.PanelID + ' #pnlEncounter';
            else
                Encounter_Visits.params["PanelID"] = "pnlEncounter";

            var self = $('#' + Encounter_Visits.params.PanelID);
            self.loadDropDowns(true).done(function () {

                utility.ValidateFromToDate('' + Encounter_Visits.params.PanelID + ' #frmVisits', 'dtpAppointmentDateFrom', 'dtpAppointmentDateTo', true);
                utility.AddDaysFromToDate(Encounter_Visits.params.PanelID + ' #frmVisits', 'dtpAppointmentDateFrom', 'dtpAppointmentDateTo', 0, 0);
            });

        }
        Encounter_Visits.BindProvider();
        Encounter_Visits.BindFacility();
        if (Encounter_Visits.params.ParentCtrl == "ChargeBatch_Viewer") {
            $('#' + Encounter_Visits.params.PanelID + ' #txtProvider').val(globalAppdata.DefaultProviderName);
            $('#' + Encounter_Visits.params.PanelID + ' #hfProvider').val(globalAppdata.DefaultProviderId);

            $('#' + Encounter_Visits.params.PanelID + ' #txtFacility').val(globalAppdata.DefaultFacilityName);
            $('#' + Encounter_Visits.params.PanelID + ' #hfFacility').val(globalAppdata.DefaultFacilityId);

            $Ctrl_p = $('#' + Encounter_Visits.params.PanelID + ' #txtProvider');
            $hfCtrl_p = $('#' + Encounter_Visits.params.PanelID + ' #hfProvider');
            //Provider To
            utility.SetKendoAutoCompleteSourceforValidate($Ctrl_p, globalAppdata.DefaultProviderName, $hfCtrl_p, globalAppdata.DefaultProviderId);

            $Ctrl_f = $('#' + Encounter_Visits.params.PanelID + ' #txtFacility');
            $hfCtrl_f = $('#' + Encounter_Visits.params.PanelID + ' #hfFacility');
            //Facility
            utility.SetKendoAutoCompleteSourceforValidate($Ctrl_f, globalAppdata.DefaultFacilityName, $hfCtrl_f, globalAppdata.DefaultFacilityId);
        }
        // Begin 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3018
        if (Encounter_Visits.params.ParentCtrl != "ChargeBatch_Viewer" && Encounter_Visits.params.ParentCtrl != "batchTabEncounter" && Encounter_Visits.params.ParentCtrl != "billTabUnClaimedAppointment" && Encounter_Visits.params.ParentCtrl != "billTabPaymentPosting" && Encounter_Visits.params.ParentCtrl != "Bill_PaymentPosting" && Encounter_Visits.params.ParentCtrl != "Bill_ChargeSearch" && Encounter_Visits.params.ParentCtrl != "ERA_ChargeSearch" && Encounter_Visits.params.ParentCtrl != "Document_Import" && Encounter_Visits.params.ParentCtrl != "Document_Viewer" && Encounter_Visits.params.ParentCtrl != "billTabFollowUpPatientAR" && Encounter_Visits.params.ParentCtrl != "Bill_FollowUpPatientAR_Detail" && Encounter_Visits.params.ParentCtrl != "billTabFollowUpInsuranceAR" && Encounter_Visits.params.ParentCtrl != "Bill_FollowUpInsuranceAR_Detail" && Encounter_Visits.params.ParentCtrl != "billTabClaimSubmission" && Encounter_Visits.params.ParentCtrl != "Patient_Search" && Encounter_Visits.params.ParentCtrl != "Patient_Case" && Encounter_Visits.params.ParentCtrl != "EncounterChargeCapture") {
            Encounter_Visits.removeDialogClasses();
        }
        // End 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3018
        // Patient_Demographic.FillPatientInfo(Patient_Demographic.params);
        Encounter_Visits.VisitSearch(0);
        if (params.PreviousTab.TabID == "patTabDemographic" && Encounter_Visits.params["ParentCtrl"] != "Patient_Search") {
            Patient_Demographic.isChangeInDemographic(Patient_Demographic.params.mode);
        }
        else if (params.PreviousTab.TabID == "patTabInsurance" && Encounter_Visits.params["ParentCtrl"] != "Patient_Search") {
            Patient_Insurance.isChangeInInsurance(Patient_Insurance.params.mode);
        }
        // End 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3018
    },

    BindProvider: function () {
        var Ctrl = $("#" + Encounter_Visits.params.PanelID + " #frmVisits #txtProvider");
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        var hfCtrl = $("#" + Encounter_Visits.params.PanelID + " #frmVisits #hfProvider");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    BindFacility: function () {
        var Ctrl = $("#" + Encounter_Visits.params.PanelID + " #frmVisits #txtFacility");
        var func = function () { return utility.GetFacilityArray(Ctrl.val()) };
        var hfCtrl = $("#" + Encounter_Visits.params.PanelID + " #frmVisits #hfFacility");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    OpenProvider: function () {
        var params = [];
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Encounter_Visits";
        if (Encounter_Visits.params.PanelID != 'pnlEncounter')
            LoadActionPan('Admin_Provider', params, Encounter_Visits.params.PanelID);
        else
            LoadActionPan('Admin_Provider', params);
    },

    OpenProviderDetail: function () {
        //Admin_Provider.ProviderEdit($('#' + demographicDetail.params.PanelID + ' #hfProvider').val(), "demographicDetail");
        var params = [];
        params["ProviderId"] = $('#' + Encounter_Visits.params["PanelID"] + ' #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'Encounter_Visits';
        LoadActionPan('providerDetail', params);
    },

    OpenFacility: function () {
        var params = [];
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Encounter_Visits";
        if (Encounter_Visits.params.PanelID != 'pnlEncounter')
            LoadActionPan('Admin_Facility', params, Encounter_Visits.params.PanelID);
        else
            LoadActionPan('Admin_Facility', params);
    },

    OpenFacilityDetail: function () {
        //Admin_Facility.FacilityEdit($('#' + demographicDetail.params.PanelID + ' #hfFacility').val(), "demographicDetail");
        var params = [];
        params["FacilityId"] = $('#' + Encounter_Visits.params.PanelID + ' #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacility";
        params["ParentCtrl"] = 'Encounter_Visits';
        LoadActionPan('facilityDetail', params);
    },

    VisitSearch: function (VisitId, PageNo, rpp, VisitStatus) {
        utility.ValidateSearchCriteria(Encounter_Visits.params["PanelID"] + " #frmVisits", function () {
            var self = $("#" + Encounter_Visits.params["PanelID"]);
            var myJSON = self.getMyJSON();

            Encounter_Visits.SearchVisits(myJSON, Encounter_Visits.params.patientID, VisitId, PageNo, rpp, VisitStatus).done(function (response) {
                if (response.status != false) {
                    if ($("#" + Encounter_Visits.params["PanelID"] + " #pnlVisits_Result").css("display") == "none") {
                        $("#" + Encounter_Visits.params["PanelID"] + " #pnlVisits_Result").show();
                    }
                    if (response.VisitsCount > 0) { }
                    else {
                        $("#" + Encounter_Visits.params["PanelID"] + " #divOpenVisitsPaging").css("display", "none");
                        $("#" + Encounter_Visits.params["PanelID"] + " #divCloseVisitsPaging").css("display", "none");
                    }
                    if (response.CloseVisitsLoad_JSON != "") {
                        Encounter_Visits.CloseVisitGridLoad(response, PageNo, rpp);
                    }
                    if (response.VisitsLoad_JSON != "") {
                        Encounter_Visits.OpenVisitGridLoad(response, PageNo, rpp);
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        });
    },

    VisitEdit: function (VisitId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Encounter", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = VisitId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["PatientId"] = Encounter_Visits.params.patientID;
                    params["VisitId"] = selectedValue;
                    params["mode"] = "Edit";
                    LoadActionPan('basicFeeScheduleDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    OpenVisitGridLoad: function (response, PageNo, rpp) {
        if ($.fn.dataTable.isDataTable("#" + Encounter_Visits.params["PanelID"] + " #pnlVisits_Result #dgvOpenVisitsDetail"))
            $("#" + Encounter_Visits.params["PanelID"] + " #pnlVisits_Result #dgvOpenVisitsDetail").dataTable().fnDestroy();
        //$("#" + Encounter_Visits.params["PanelID"] + " #pnlVisits_Result div#CloseVisits #dgvCloseVisitsDetail").dataTable().fnDestroy();
        $("#" + Encounter_Visits.params["PanelID"] + " #pnlVisits_Result #dgvOpenVisitsDetail tbody").find("tr").remove();
        //$("#" + Encounter_Visits.params["PanelID"] + " #pnlVisits_Result div#CloseVisits #dgvCloseVisitsDetail tbody").find("tr").remove();
        //var OpenVisitsDivHTML = $("#" + Encounter_Visits.params["PanelID"] + " #pnlVisits_Result div#OpenVisits").html();
        //var objDivClosedVisits = $("#" + Encounter_Visits.params["PanelID"] + " #pnlVisits_Result div#CloseVisits");
        //objDivClosedVisits.empty().append(OpenVisitsDivHTML);
        //objDivClosedVisits.find("#Encounter_Open_Visits_SelectedDataKeys").attr("id", "Encounter_Closed_Visits_SelectedDataKeys");
        //objDivClosedVisits.find("#dgvOpenVisitsDetail").attr("id", "dgvCloseVisitsDetail");
        //objDivClosedVisits.find("#divOpenVisitsPaging").attr("id", "divCloseVisitsPaging");
        if (response.OpenVisitsCount > 0) {
            var VisitsLoadJSONData = JSON.parse(response.VisitsLoad_JSON);
            $.each(VisitsLoadJSONData, function (i, item) {
                var gridId = "";
                if (item.Status.toLowerCase() != "checkout")
                    gridRowId = "gvOpenVisitsDetail_row";
                var $row = $('<tr/>');

                $row.attr("id", gridId + item.VisitId);
                $row.attr("VisitId", item.VisitId);

                if (item.IsActive == 1) {
                    isactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }
                var EditMethod = "SelectTab('EncounterTabVisit_Detail', false, true, '" + item.VisitId + "','" + utility.RemoveTimeFromDate(null, item.CreatedOn) + "',event)";
                var MethodMode = "";
                var ActiveInacvtiveMethod = "Encounter_Visits.VisitActiveInactive(" + item.VisitId.trim() + "," + isactive + ",event);";
                if (Encounter_Visits.params.ParentCtrl == "ChargeBatch_Viewer") {
                    MethodMode = "ChargeBatch_Viewer.FillClaimNumberFromSearch('" + item.ClaimNumber + "','" + item.AccountNumber + "' , '" + item.PatientName + "' , '" + item.PatientId + "' , '" + item.DOSFrom + "' , '" + item.VisitId + "' );"
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="' + MethodMode + '" title="Select Record"><i class="fa fa-check black"></i></a></td>';
                }
                else if (Encounter_Visits.params.ParentCtrl == "ERA_ChargeSearch") {
                    MethodMode = "ERA_ChargeSearch.FillClaimNumberFromSearch('" + item.ClaimNumber + "','" + item.AccountNumber + "' , '" + item.PatientName + "' , '" + item.PatientId + "' , '" + item.DOSFrom + "' , '" + item.VisitId + "' );"
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="' + MethodMode + '" title="Select Record"><i class="fa fa-check black"></i></a></td>';
                }
                else if (Encounter_Visits.params.ParentCtrl == "billTabPaymentPosting" || Encounter_Visits.params.ParentCtrl == "Bill_PaymentPosting") {
                    MethodMode = "Bill_PaymentPosting.FillClaimNumberFromSearch('" + item.ClaimNumber + "','" + item.AccountNumber + "' , '" + item.PatientName + "' , '" + item.PatientId + "' , '" + item.DOSFrom + "' , '" + item.VisitId + "' );"
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="' + MethodMode + '" title="Select Record"><i class="fa fa-check black"></i></a></td>';
                }
                else if (Encounter_Visits.params.ParentCtrl == "Patient_Case") {
                    MethodMode = "Patient_Case.FillClaimNumberFromSearch('" + item.ClaimNumber + "','" + item.AccountNumber + "' , '" + item.PatientName + "' , '" + item.PatientId + "' , '" + item.DOSFrom + "' , '" + item.VisitId + "' );"
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="' + MethodMode + '" title="Select Record"><i class="fa fa-check black"></i></a></td>';
                }
                else if (Encounter_Visits.params.ParentCtrl == "Bill_ChargeSearch") {
                    MethodMode = "Bill_ChargeSearch.FillClaimNumberFromSearch('" + item.ClaimNumber + "','" + item.AccountNumber + "' , '" + item.PatientName + "' , '" + item.PatientId + "' , '" + item.DOSFrom + "' , '" + item.VisitId + "' );"
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="' + MethodMode + '" title="Select Record"><i class="fa fa-check black"></i></a></td>';
                }
                else if (Encounter_Visits.params.ParentCtrl == "billTabUnClaimedAppointment") {
                    MethodMode = "Bill_UnClaimedAppointment.FillClaimNumberFromSearch('" + item.ClaimNumber + "','" + item.AccountNumber + "' , '" + item.PatientName + "' , '" + item.PatientId + "' , '" + item.DOSFrom + "' , '" + item.VisitId + "' );"
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="' + MethodMode + '" title="Select Record"><i class="fa fa-check black"></i></a></td>';
                }
                else if (Encounter_Visits.params.ParentCtrl == "billTabFollowUpPatientAR") {
                    MethodMode = "Bill_FollowUpPatientAR.FillClaimNumberFromSearch('" + item.ClaimNumber + "','" + item.AccountNumber + "' , '" + item.PatientName + "' , '" + item.PatientId + "' , '" + item.DOSFrom + "' , '" + item.VisitId + "' );"
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="' + MethodMode + '" title="Select Record"><i class="fa fa-check black"></i></a></td>';
                }
                else if (Encounter_Visits.params.ParentCtrl == "Document_Import") {
                    MethodMode = "Document_Import.FillClaimNumberFromSearch('" + item.ClaimNumber + "','" + item.AccountNumber + "' , '" + item.PatientName + "' , '" + item.PatientId + "' , '" + item.DOSFrom + "' , '" + item.VisitId + "' );"
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="' + MethodMode + '" title="Select Record"><i class="fa fa-check black"></i></a></td>';
                }
                else if (Encounter_Visits.params.ParentCtrl == "Document_Viewer") {
                    MethodMode = "Document_Viewer.FillClaimNumberFromSearch('" + item.ClaimNumber + "','" + item.AccountNumber + "' , '" + item.PatientName + "' , '" + item.PatientId + "' , '" + item.DOSFrom + "' , '" + item.VisitId + "' );"
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="' + MethodMode + '" title="Select Record"><i class="fa fa-check black"></i></a></td>';
                }
                else if (Encounter_Visits.params.ParentCtrl == "Bill_FollowUpPatientAR_Detail") {
                    MethodMode = "Bill_FollowUpPatientAR_Detail.FillClaimNumberFromSearch('" + item.ClaimNumber + "','" + item.AccountNumber + "' , '" + item.PatientName + "' , '" + item.PatientId + "' , '" + item.DOSFrom + "' , '" + item.VisitId + "' );"
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="' + MethodMode + '" title="Select Record"><i class="fa fa-check black"></i></a></td>';
                }

                else if (Encounter_Visits.params.ParentCtrl == "billTabFollowUpInsuranceAR") {
                    MethodMode = "Bill_FollowUpInsuranceAR.FillClaimNumberFromSearch('" + item.ClaimNumber + "','" + item.AccountNumber + "' , '" + item.PatientName + "' , '" + item.PatientId + "' , '" + item.DOSFrom + "' , '" + item.VisitId + "' );"
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="' + MethodMode + '" title="Select Record"><i class="fa fa-check black"></i></a></td>';
                }

                else if (Encounter_Visits.params.ParentCtrl == "Bill_FollowUpInsuranceAR_Detail") {
                    MethodMode = "Bill_FollowUpInsuranceAR_Detail.FillClaimNumberFromSearch('" + item.ClaimNumber + "','" + item.AccountNumber + "' , '" + item.PatientName + "' , '" + item.PatientId + "' , '" + item.DOSFrom + "' , '" + item.VisitId + "' );"
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="' + MethodMode + '" title="Select Record"><i class="fa fa-check black"></i></a></td>';
                }
                else if (Encounter_Visits.params.ParentCtrl == "billTabClaimSubmission") {
                    MethodMode = "Bill_ClaimSubmission.FillClaimNumberFromSearch('" + item.ClaimNumber + "','" + item.AccountNumber + "' , '" + item.PatientName + "' , '" + item.PatientId + "' , '" + item.DOSFrom + "' , '" + item.VisitId + "' );"
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="' + MethodMode + '" title="Select Record"><i class="fa fa-check black"></i></a></td>';
                }
                else if (Encounter_Visits.params.ParentCtrl == "batchTabEncounter") {
                    MethodMode = "Batch_Encounter.FillClaimNumberFromSearch('" + item.ClaimNumber + "','" + item.AccountNumber + "' , '" + item.PatientName + "' , '" + item.PatientId + "' , '" + item.DOSFrom + "' , '" + item.VisitId + "' );"
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="' + MethodMode + '" title="Select Record"><i class="fa fa-check black"></i></a></td>';
                }


                    // Begin 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3018
                else if (Encounter_Visits.params.ParentCtrl == "Patient_Search") {
                    MethodMode = "Patient_Search.FillClaimNumberFromSearch('" + item.ClaimNumber + "','" + item.AccountNumber + "' , '" + item.PatientName + "' , '" + item.PatientId + "' , '" + item.DOSFrom + "' , '" + item.VisitId + "' );"
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="' + MethodMode + '" title="Select Record"><i class="fa fa-check black"></i></a></td>';
                }
                    // End 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3018
                else if (Encounter_Visits.params.ParentCtrl == "EncounterChargeCapture") {
                    EditMethod = '';
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="Encounter_Visits.VisitDelete(' + item.VisitId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a><a class="btn  btn-xs" href="#" onclick="' + ActiveInacvtiveMethod + '" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>&nbsp;</td>';
                    $row.attr("onclick", "utility.SelectGridRow($('#" + gridId + item.VisitId + "'))");
                }

                else {
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="Encounter_Visits.VisitDelete(' + item.VisitId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="' + EditMethod + '" title="Edit Record"><i class="fa fa-edit black"></i></a><a class="btn  btn-xs" href="#" onclick="' + ActiveInacvtiveMethod + '" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>&nbsp;</td>';
                }
                if (MethodMode != "") {
                    $row.attr("onclick", MethodMode);
                }
                else {
                    $row.attr("onclick", EditMethod);
                }

                $row.append('<td style="display:none;">' + item.VisitId + '</td>' + actions + '<td>' + item.ClaimNumber + '</td><td>' + utility.RemoveTimeFromDate(null, item.AppointmentDate) + '</td><td>' + item.FacilityName + '</td><td>' + item.ProviderName + '</td><td>' + item.Status + '</td><td>' + item.CheckInDateTime + '</td><td>' + item.CheckInByFullName + '</td>');

                //if (item.Status == "CheckOut")
                //    $("#" + Encounter_Visits.params["PanelID"] + " div#pnlVisits_Result div#CloseVisits #dgvCloseVisitsDetail tbody").last().append($row);
                // else
                if (item.Status.toLowerCase() != "checkout")
                    $("#" + Encounter_Visits.params["PanelID"] + " div#pnlVisits_Result #dgvOpenVisitsDetail tbody").last().append($row);

            });
            var openVisitRows = $("#" + Encounter_Visits.params["PanelID"] + " div#pnlVisits_Result #dgvOpenVisitsDetail tbody").find("tr");
            //var closedVisitRows = $("#" + Encounter_Visits.params["PanelID"] + " div#pnlVisits_Result div#CloseVisits #dgvCloseVisitsDetail tbody").find("tr");


            if (openVisitRows.length < 1) {
                $("#" + Encounter_Visits.params["PanelID"] + " div#pnlVisits_Result #dgvOpenVisitsDetail").DataTable({
                    "language": {
                        "emptyTable": "No Open Visit Found"
                    }, "autoWidth": false, "bInfo": false, "bPaginate": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
                $("#" + Encounter_Visits.params["PanelID"] + " #divOpenVisitsPaging").css("display", "none");
            }
            //if (closedVisitRows.length < 1) {
            //    //$("#" + Encounter_Visits.params["PanelID"] + " #divOpenVisitsPaging").css("display", "none");
            //    $("#" + Encounter_Visits.params["PanelID"] + " div#pnlVisits_Result div#CloseVisits #dgvCloseVisitsDetail").DataTable({
            //        "language": {
            //            "emptyTable": "No Closed Visit Found"
            //        }, "autoWidth": false, "bInfo": false, "bPaginate": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            //    });
            //}
            //------------Pagination Open Visits-----------
            $("#" + Encounter_Visits.params["PanelID"] + " #divOpenVisitsPaging").css("display", "inline");
            //Showing 1 to 15 of 15 entries
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var PagesToShow = Math.ceil(response.OpenVisitsCount / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("" + Encounter_Visits.params["PanelID"] + " #divOpenVisitsPaging", response.OpenVisitsCount, 5, "Encounter_Visits", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.OpenVisitsCount ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.OpenVisitsCount;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.OpenVisitsCount + " Record(s)";
            $("#" + Encounter_Visits.params["PanelID"] + " #divOpenVisitsPaging #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $("#" + Encounter_Visits.params["PanelID"] + " #pnlVisits_Result #divOpenVisitsPaging li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
            //------------End Pagination-------

        }
        else {
            if (!$("#" + Encounter_Visits.params["PanelID"] + " div#pnlVisits_Result #dgvOpenVisitsDetail").parent().parent().hasClass("dataTables_wrapper")) {
                $("#" + Encounter_Visits.params["PanelID"] + " div#pnlVisits_Result #dgvOpenVisitsDetail").DataTable({
                    "language": {
                        "emptyTable": "No Open Visit Found"
                    }, "autoWidth": false, "bInfo": false, "bPaginate": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
                $("#" + Encounter_Visits.params["PanelID"] + " #divOpenVisitsPaging").css("display", "none");
            }
            else {
                var $row = $('<tr/>');
                $row.append('<td colspan="8" class="center" >No Open Visit Found</td>');
                $("#" + Encounter_Visits.params["PanelID"] + " div#pnlVisits_Result #dgvOpenVisitsDetail tbody").last().append($row);
            }
            //$("#" + Encounter_Visits.params["PanelID"] + " div#pnlVisits_Result div#CloseVisits #dgvCloseVisitsDetail").DataTable({

            //    "language": {
            //        "emptyTable": "No Closed Visit Found"
            //    }, "autoWidth": false, "bInfo": false, "bPaginate": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            //});
        }
        if ($.fn.dataTable.isDataTable("#" + Encounter_Visits.params["PanelID"] + " div#pnlVisits_Result #dgvOpenVisitsDetail") || $("#" + Encounter_Visits.params["PanelID"] + " div#pnlVisits_Result #dgvOpenVisitsDetail").parent().parent().hasClass("dataTables_wrapper"))
            ;
        else
            $("#" + Encounter_Visits.params["PanelID"] + " div#pnlVisits_Result #dgvOpenVisitsDetail").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "bFilter": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        //if ($.fn.dataTable.isDataTable("#" + Encounter_Visits.params["PanelID"] + " div#pnlVisits_Result div#CloseVisits #dgvCloseVisitsDetail"))
        //    ;
        //else
        //    $("#" + Encounter_Visits.params["PanelID"] + " div#pnlVisits_Result div#CloseVisits #dgvCloseVisitsDetail").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "bFilter": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    CloseVisitGridLoad: function (response, PageNo, rpp) {
        if ($.fn.dataTable.isDataTable("#" + Encounter_Visits.params["PanelID"] + " #pnlVisits_Result #dgvCloseVisitsDetail"))
            $("#" + Encounter_Visits.params["PanelID"] + " #pnlVisits_Result #dgvCloseVisitsDetail").dataTable().fnDestroy();
        $("#" + Encounter_Visits.params["PanelID"] + " #pnlVisits_Result #dgvCloseVisitsDetail tbody").find("tr").remove();
        if (response.CloseVisitsCount > 0) {
            var CloseVisitsLoadJSONData = JSON.parse(response.CloseVisitsLoad_JSON);
            $.each(CloseVisitsLoadJSONData, function (i, item) {
                var gridId = "";
                if (item.Status.toLowerCase() == "checkout")
                    gridRowId = "gvCloseVisitsDetail_row";
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#" + gridId + item.VisitId + "'))");
                $row.attr("id", gridId + item.VisitId);
                $row.attr("VisitId", item.VisitId);

                if (item.IsActive == 1) {
                    isactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }
                var EditMethod = "SelectTab('EncounterTabVisit_Detail', false, true, '" + item.VisitId + "','" + utility.RemoveTimeFromDate(null, item.CreatedOn) + "')";

                var ActiveInacvtiveMethod = "Encounter_Visits.VisitActiveInactive(" + item.VisitId.trim() + "," + isactive + ");";
                if (Encounter_Visits.params.ParentCtrl == "ChargeBatch_Viewer") {
                    MethodMode = "ChargeBatch_Viewer.FillClaimNumberFromSearch('" + item.ClaimNumber + "','" + item.AccountNumber + "' , '" + item.PatientName + "' , '" + item.PatientId + "' , '" + item.DOSFrom + "' , '" + item.VisitId + "' );"
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="' + MethodMode + '" title="Select Record"><i class="fa fa-check black"></i></a></td>';
                }
                else if (Encounter_Visits.params.ParentCtrl == "billTabPaymentPosting" || Encounter_Visits.params.ParentCtrl == "Bill_PaymentPosting") {
                    MethodMode = "Bill_PaymentPosting.FillClaimNumberFromSearch('" + item.ClaimNumber + "','" + item.AccountNumber + "' , '" + item.PatientName + "' , '" + item.PatientId + "' , '" + item.DOSFrom + "' , '" + item.VisitId + "' );"
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="' + MethodMode + '" title="Select Record"><i class="fa fa-check black"></i></a></td>';
                }
                else if (Encounter_Visits.params.ParentCtrl == "Bill_ChargeSearch") {
                    MethodMode = "Bill_ChargeSearch.FillClaimNumberFromSearch('" + item.ClaimNumber + "','" + item.AccountNumber + "' , '" + item.PatientName + "' , '" + item.PatientId + "' , '" + item.DOSFrom + "' , '" + item.VisitId + "' );"
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="' + MethodMode + '" title="Select Record"><i class="fa fa-check black"></i></a></td>';
                }
                else if (Encounter_Visits.params.ParentCtrl == "billTabUnClaimedAppointment") {
                    MethodMode = "Bill_UnClaimedAppointment.FillClaimNumberFromSearch('" + item.ClaimNumber + "','" + item.AccountNumber + "' , '" + item.PatientName + "' , '" + item.PatientId + "' , '" + item.DOSFrom + "' , '" + item.VisitId + "' );"
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="' + MethodMode + '" title="Select Record"><i class="fa fa-check black"></i></a></td>';
                }
                else if (Encounter_Visits.params.ParentCtrl == "billTabFollowUpPatientAR") {
                    MethodMode = "Bill_FollowUpPatientAR.FillClaimNumberFromSearch('" + item.ClaimNumber + "','" + item.AccountNumber + "' , '" + item.PatientName + "' , '" + item.PatientId + "' , '" + item.DOSFrom + "' , '" + item.VisitId + "' );"
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="' + MethodMode + '" title="Select Record"><i class="fa fa-check black"></i></a></td>';
                }

                else if (Encounter_Visits.params.ParentCtrl == "billTabFollowUpInsuranceAR") {
                    MethodMode = "Bill_FollowUpInsuranceAR.FillClaimNumberFromSearch('" + item.ClaimNumber + "','" + item.AccountNumber + "' , '" + item.PatientName + "' , '" + item.PatientId + "' , '" + item.DOSFrom + "' , '" + item.VisitId + "' );"
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="' + MethodMode + '" title="Select Record"><i class="fa fa-check black"></i></a></td>';
                }
                else if (Encounter_Visits.params.ParentCtrl == "Document_Import") {
                    MethodMode = "Document_Import.FillClaimNumberFromSearch('" + item.ClaimNumber + "','" + item.AccountNumber + "' , '" + item.PatientName + "' , '" + item.PatientId + "' , '" + item.DOSFrom + "' , '" + item.VisitId + "' );"
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="' + MethodMode + '" title="Select Record"><i class="fa fa-check black"></i></a></td>';
                }
                else if (Encounter_Visits.params.ParentCtrl == "Document_Viewer") {
                    MethodMode = "Document_Viewer.FillClaimNumberFromSearch('" + item.ClaimNumber + "','" + item.AccountNumber + "' , '" + item.PatientName + "' , '" + item.PatientId + "' , '" + item.DOSFrom + "' , '" + item.VisitId + "' );"
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="' + MethodMode + '" title="Select Record"><i class="fa fa-check black"></i></a></td>';
                }
                else if (Encounter_Visits.params.ParentCtrl == "Bill_FollowUpPatientAR_Detail") {
                    MethodMode = "Bill_FollowUpPatientAR_Detail.FillClaimNumberFromSearch('" + item.ClaimNumber + "','" + item.AccountNumber + "' , '" + item.PatientName + "' , '" + item.PatientId + "' , '" + item.DOSFrom + "' , '" + item.VisitId + "' );"
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="' + MethodMode + '" title="Select Record"><i class="fa fa-check black"></i></a></td>';
                }
                else if (Encounter_Visits.params.ParentCtrl == "billTabClaimSubmission") {
                    MethodMode = "Bill_ClaimSubmission.FillClaimNumberFromSearch('" + item.ClaimNumber + "','" + item.AccountNumber + "' , '" + item.PatientName + "' , '" + item.PatientId + "' , '" + item.DOSFrom + "' , '" + item.VisitId + "' );"
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="' + MethodMode + '" title="Select Record"><i class="fa fa-check black"></i></a></td>';
                }
                else if (Encounter_Visits.params.ParentCtrl == "batchTabEncounter") {
                    MethodMode = "Batch_Encounter.FillClaimNumberFromSearch('" + item.ClaimNumber + "','" + item.AccountNumber + "' , '" + item.PatientName + "' , '" + item.PatientId + "' , '" + item.DOSFrom + "' , '" + item.VisitId + "' );"
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="' + MethodMode + '" title="Select Record"><i class="fa fa-check black"></i></a></td>';
                }
                    // Begin 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3018
                else if (Encounter_Visits.params.ParentCtrl == "Patient_Search") {
                    MethodMode = "Patient_Search.FillClaimNumberFromSearch('" + item.ClaimNumber + "','" + item.AccountNumber + "' , '" + item.PatientName + "' , '" + item.PatientId + "' , '" + item.DOSFrom + "' , '" + item.VisitId + "' );"
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="' + MethodMode + '" title="Select Record"><i class="fa fa-check black"></i></a></td>';
                }
                    // End 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3018
                else if (Encounter_Visits.params.ParentCtrl == "EncounterChargeCapture") {
                    EditMethod = '';
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="Encounter_Visits.VisitDelete(' + item.VisitId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="' + ActiveInacvtiveMethod + '" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>&nbsp;</td>';
                }
                else {
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="Encounter_Visits.VisitDelete(' + item.VisitId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="' + EditMethod + '" title="Edit Record"><i class="fa fa-edit black"></i></a><a class="btn  btn-xs" href="#" onclick="' + ActiveInacvtiveMethod + '" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>&nbsp;</td>';
                }
                $row.append('<td style="display:none;">' + item.VisitId + '</td>' + actions + '<td>' + item.ClaimNumber + '</td><td>' + utility.RemoveTimeFromDate(null, item.AppointmentDate) + '</td><td>' + item.FacilityName + '</td><td>' + item.ProviderName + '</td><td>' + item.Status + '</td><td>' + item.CheckOutDatetime + '</td><td>' + item.CheckInByFullName + '</td>');

                $("#" + Encounter_Visits.params["PanelID"] + " div#pnlVisits_Result #dgvCloseVisitsDetail tbody").last().append($row);

            });
            var closedVisitRows = $("#" + Encounter_Visits.params["PanelID"] + " div#pnlVisits_Result #dgvCloseVisitsDetail tbody").find("tr");


            if (closedVisitRows.length < 1) {
                $("#" + Encounter_Visits.params["PanelID"] + " div#pnlVisits_Result #dgvCloseVisitsDetail").DataTable({
                    "language": {
                        "emptyTable": "No Close Visit Found"
                    }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
                $("#" + Encounter_Visits.params["PanelID"] + " #divCloseVisitsPaging").css("display", "none");
            }

            //------------Pagination Close Visits-----------
            $("#" + Encounter_Visits.params["PanelID"] + " #divCloseVisitsPaging").css("display", "inline");
            //Showing 1 to 15 of 15 Record(s)
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var PagesToShow = Math.ceil(response.CloseVisitsCount / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("" + Encounter_Visits.params["PanelID"] + " #divCloseVisitsPaging", response.CloseVisitsCount, 5, "Encounter_Visits", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.CloseVisitsCount ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.CloseVisitsCount;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.CloseVisitsCount + " Record(s)";
            $("#" + Encounter_Visits.params["PanelID"] + " #divCloseVisitsPaging #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $("#" + Encounter_Visits.params["PanelID"] + " #pnlVisits_Result #divCloseVisitsPaging li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
            //------------End Pagination-------
        }
        else {
            if (!$("#" + Encounter_Visits.params["PanelID"] + " div#pnlVisits_Result #dgvCloseVisitsDetail").parent().parent().hasClass("dataTables_wrapper")) {
                $("#" + Encounter_Visits.params["PanelID"] + " div#pnlVisits_Result #dgvCloseVisitsDetail").DataTable({
                    "language": {
                        "emptyTable": "No Close Visit Found"
                    }, "autoWidth": false, "bInfo": false, "bPaginate": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
            }
            else {
                var $row = $('<tr/>');
                $row.append('<td colspan="8" class="center" >No Close Visit Found</td>');
                $("#" + Encounter_Visits.params["PanelID"] + " div#pnlVisits_Result #dgvCloseVisitsDetail tbody").last().append($row);
            }
        }
        if ($.fn.dataTable.isDataTable("#" + Encounter_Visits.params["PanelID"] + " div#pnlVisits_Result #dgvCloseVisitsDetail") || $("#" + Encounter_Visits.params["PanelID"] + " div#pnlVisits_Result #dgvCloseVisitsDetail").parent().parent().hasClass("dataTables_wrapper"))
            ;
        else
            $("#" + Encounter_Visits.params["PanelID"] + " div#pnlVisits_Result #dgvCloseVisitsDetail").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "bFilter": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

    },

    VisitActiveInactive: function (VisitId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Encounter", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = VisitId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Encounter_Visits.UpdateActiveInactiveVisits(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                //Resolved By Mohsin Nasir Bug # PMS-2876
                                utility.DisplayMessages(response.Message, 1);
                                // END Resolved By Mohsin Nasir Bug # PMS-2876
                                Encounter_Visits.VisitSearch();
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { }, '3', null, null, null, IsActive);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    VisitDelete: function (VisitId, event, formId, status) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        var deletionMsg = "Do you want to delete this record ?";
        if (status != undefined && status != null && status.toLowerCase() == "submitted") {
            deletionMsg = "Claim is submitted, Are you sure you want to delete this record?";
        }
        AppPrivileges.GetFormPrivileges("Encounter", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm(deletionMsg, function () {
                    var selectedValue = VisitId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Encounter_Visits.DeleteVisit(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $("#" + Encounter_Visits.params["PanelID"] + " div#pnlVisits_Result div#OpenVisits #dgvOpenVisitsDetail").DataTable();
                                var table2 = $("#" + Encounter_Visits.params["PanelID"] + " div#pnlVisits_Result div#CloseVisits #dgvCloseVisitsDetail").DataTable();
                                table1.row('.active').remove().draw(false);
                                table2.row('.active').remove().draw(false);
                                //Modified by Azeem Raza Tayyab on 11-Apr-2016 to Fix bug#:PMS-4834 added new perameter(formId) to handel deletion from Bill_ChargeSearch.
                                if (formId == "ChargeSearch") {
                                    Bill_ChargeSearch.bIsFirstLoad = true;
                                    Bill_ChargeSearch.Load(Bill_ChargeSearch.params);
                                }
                                else {
                                    Encounter_Visits.VisitSearch();
                                }
                                utility.DisplayMessages(response.Message, 1);
                            }
                            else {
                                if (response.Message.indexOf('successfully') > -1) {
                                    utility.DisplayMessages(response.Message, 1);
                                    if (Encounter_Visits.params["PanelID"]) {
                                        Encounter_Visits.VisitSearch();
                                    }
                                    else {
                                        Bill_ChargeSearch.ValidateSearchCriteria();
                                    }
                                } else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            }
                        });
                    }
                }, function () { }, 'Confirm Delete');
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    SearchVisits: function (VisitData, PatientID, VisitId, PageNumber, RowsPerPage, VisitStatus) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "VisitData=" + VisitData + "&VisitID=" + VisitId + "&PatientID=" + PatientID + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage + "&VisitStatus=" + VisitStatus;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_ENCOUNTER_VISITS", "SEARCH_VISITS");
    },

    SaveVisit: function (VisitData, PatientID) {
        var data = "VisitData=" + VisitData + "&PatientID=" + PatientID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_ENCOUNTER_VISITS", "SAVE_VISIT");
    },

    UpdateVisit: function (VisitData, VisitID, PatientID) {
        var data = "VisitData=" + VisitData + "&VisitID=" + VisitID + "&PatientID=" + PatientID;//Encounter_Visits.params.patientID
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_ENCOUNTER_VISITS", "UPDATE_VISIT");
    },

    UpdateActiveInactiveVisits: function (VisitID, IsActive) {
        var data = "VisitID=" + VisitID + "&PatientID=" + Encounter_Visits.params.patientID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_ENCOUNTER_VISITS", "UPDATE_VISIT_ACTIVE_INACTIVE");
    },

    DeleteVisit: function (VisitID) {
        var data = "VisitID=" + VisitID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_ENCOUNTER_VISITS", "DELETE_VISITS");
    },

    removeDialogClasses: function () {
        $('#' + Encounter_Visits.params.PanelID + ' .modal-header').hide();
        $('#' + Encounter_Visits.params.PanelID + ' #modalbody').removeClass('panel-body');
        $('#' + Encounter_Visits.params.PanelID + ' .modal-content').removeClass('modal-content');
        $('#' + Encounter_Visits.params.PanelID + ' .modal-dialog-full').removeAttr('class');
    },

    UnLoad: function () {
        if (Encounter_Visits.params != null && Encounter_Visits.params.ParentCtrl != null) {
            UnloadActionPan(Encounter_Visits.params.ParentCtrl, 'Encounter_Visits', null, Encounter_Visits.params.PanelID.split(' ')[0]);
            //if (bVisitFirst) {
            //    $($('body #OpenVisits1')[0]).attr('id', 'OpenVisits')
            //    $($('body #CloseVisits1')[0]).attr('id', 'CloseVisits');
            //}
        }
        else
            UnloadActionPan(null, 'Encounter_Visits');
    },

    //setTabs: function (event, activetab, inactivetab) {

    //    if (event != null) {
    //        event.stopPropagation();
    //    }

    //    $("body").find("." + activetab).each(function () {
    //        $(this).addClass("active");

    //    });
    //    $("body").find("." + inactivetab).each(function () {
    //        $(this).removeClass("active");
    //    });
    //},
    //--------------Pagination functions-----

    SelectedPageClick: function (PageNo, objPage, TotalRecords, rpp, pagingDivId) {
        // Change Background Color to Black for selected page

        //if (pagingDivId == "divCloseVisitsPaging") {
        //    var current_li = $(objPage).closest("li");
        //    $("#pnlVisits_Result #divCloseVisitsPaging li").each(function () {
        //        if ($(objPage).closest("li") == current_li) {
        //            current_li.addClass('active');
        //        }
        //        else
        //            $(this).removeAttr("class");

        //    });
        //}
        //var current_li = $(objPage).closest("li");
        //current_li.addClass('active');

        if (pagingDivId == Encounter_Visits.params["PanelID"] + " #divCloseVisitsPaging") {
            Encounter_Visits.VisitSearch(0, PageNo, 15, '1');
        }
        else if (pagingDivId == Encounter_Visits.params["PanelID"] + " #divOpenVisitsPaging") {
            Encounter_Visits.VisitSearch(0, PageNo, 15, '0');
        }

    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#" + Encounter_Visits.params["PanelID"] + " #pnlVisits_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            if (pagingDivId == Encounter_Visits.params["PanelID"] + " #divCloseVisitsPaging") {
                Encounter_Visits.VisitSearch(0, currentPageNo, 15, '1');
            }
            else if (pagingDivId == Encounter_Visits.params["PanelID"] + " #divOpenVisitsPaging") {
                Encounter_Visits.VisitSearch(0, currentPageNo, 15, '0');
            }
        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#" + Encounter_Visits.params["PanelID"] + " #pnlVisits_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });
        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo > 0) {
            if (pagingDivId == Encounter_Visits.params["PanelID"] + " #divCloseVisitsPaging") {
                Encounter_Visits.VisitSearch(0, currentPageNo, 15, '1');
            }
            else if (pagingDivId == Encounter_Visits.params["PanelID"] + " #divOpenVisitsPaging") {
                Encounter_Visits.VisitSearch(0, currentPageNo, 15, '0');
            }
        }
    },

    //---------------Open Pagination-------

    VisitReset: function () {
        $('#' + Encounter_Visits.params["PanelID"] + ' #frmVisits').resetAllControls();
        $('#' + Encounter_Visits.params["PanelID"] + ' #frmVisits #dtpAppointmentDateTo').attr('disabled', true);
    },
}