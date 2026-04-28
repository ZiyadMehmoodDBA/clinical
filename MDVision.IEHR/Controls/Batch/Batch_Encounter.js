Batch_Encounter = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Batch_Encounter.params = params;
        if (Batch_Encounter.bIsFirstLoad) {
            Batch_Encounter.bIsFirstLoad = false;
            if (Batch_Encounter.params.PanelID != null && Batch_Encounter.params.PanelID != 'pnlBatchEncounter')
                Batch_Encounter.params.PanelID = Batch_Encounter.params.PanelID + ' #pnlBatchEncounter';
            else
                Batch_Encounter.params.PanelID = "pnlBatchEncounter"

            var self = $('#' + Batch_Encounter.params.PanelID);
            self.loadDropDowns(true).done(function () {

            });
            Batch_Encounter.removeDialogClasses();
        }
        Batch_Encounter.LoadAllAutocomplete();
        Batch_Encounter.VisitSearch(null, 15);
    },
    LoadAllAutocomplete: function () {
        Batch_Encounter.BindClaimNumber();
        Batch_Encounter.BindPatientAccount();

        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            var Ctrl = $('#' + Batch_Encounter.params.PanelID + " input#txtProvider");
            var hfCtrl = $("#" + Batch_Encounter.params.PanelID + " #hfProvider");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl);
        });

        CacheManager.BindCodes('GetFacility', false).done(function (result) {
            var Ctrl = $('#' + Batch_Encounter.params.PanelID + " input#txtFacility");
            var hfCtrl = $("#" + Batch_Encounter.params.PanelID + " #hfFacility");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl);
        });

        utility.ValidateFromToDate(Batch_Encounter.params.PanelID + ' #frmBatchEncounter', 'dtpAppointmentDateFrom', 'dtpAppointmentDateTo', true);
    },

    BindClaimNumber: function () {
        var Ctrl = $('#' + Batch_Encounter.params.PanelID + ' #txtClaimNumber');
        var hfCtrl = $("#" + Batch_Encounter.params.PanelID + " #hfVisitId");
        var func = function () { return Batch_Encounter.GetClaimNumberArray(Ctrl.val()); };
        var onSelect = function (e) { $("#" + Batch_Encounter.params.PanelID + " #dpDOSfrm").val(e.DOSFrom); };
        utility.BindKendoAutoComplete(Ctrl, 3, "ClaimNumber", "contains", null, func, hfCtrl, onSelect);
    },

    GetClaimNumberArray: function (name) {
        var AllClaimsVisits = [];
        var dfd = new $.Deferred();
        Batch_Encounter.LoadClaimNumers(name).done(function (responseData) {
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

    LoadClaimNumers: function (claimNumber) {
        var data = "ClaimNumber=" + claimNumber;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "BILLING_BATCHCHARGE_DETAIL", "SEARCH_VISIT_CLAIM");
    },

    FillClaimNumberFromSearch: function (ClaimNumber, AccountNumber, PatientName, PatientId, DOSFrom, VisitId) {
        $("#" + Batch_Encounter.params.PanelID + " #frmBatchEncounter #txtClaimNumber").val(ClaimNumber);
        $("#" + Batch_Encounter.params.PanelID + " #frmBatchEncounter #hfVisitId").val(VisitId);
        if ($("#" + Batch_Encounter.params.PanelID + " #frmBatchEncounter #txtClaimNumber").data('kendoAutoComplete'))
            utility.SetKendoAutoCompleteSourceforValidate($("#" + Batch_Encounter.params.PanelID + " #frmBatchEncounter #txtClaimNumber"), ClaimNumber, $("#" + Batch_Encounter.params.PanelID + " #frmBatchEncounter #hfVisitId"), VisitId, "ClaimNumber");
        Encounter_Visits.UnLoad();
    },

    OpenEncounter: function () {
        var params = [];
        params["FromAdmin"] = 0;
        params["ParentCtrl"] = 'batchTabEncounter';
        params["patientID"] = 0;

        if ($("#" + Batch_Encounter.params.PanelID + " #txtAccountNumber").val().trim() == "")
            params["patientID"] = 0;
        else
            params["patientID"] = Number($('#' + Batch_Encounter.params.PanelID + ' #hfPatientId').val());

        LoadActionPan('Encounter_Visits', params);

    },

    OpenProvider: function () {
        var params = [];
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "batchTabEncounter";
        LoadActionPan('Admin_Provider', params);
    },
    OpenProviderDetail: function () {
        //Admin_Provider.ProviderEdit($('#' + demographicDetail.params.PanelID + ' #hfProvider').val(), "demographicDetail");
        var params = [];
        params["ProviderId"] = $('#' + Batch_Encounter.params.PanelID + ' #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'batchTabEncounter';
        LoadActionPan('providerDetail', params);
    },
    OpenFacility: function () {
        var params = [];
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "batchTabEncounter";
        LoadActionPan('Admin_Facility', params);
    },
    OpenFacilityDetail: function () {
        //Admin_Facility.FacilityEdit($('#' + demographicDetail.params.PanelID + ' #hfFacility').val(), "demographicDetail");
        var params = [];
        params["FacilityId"] = $('#' + Batch_Encounter.params.PanelID + ' #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacility";
        params["ParentCtrl"] = 'batchTabEncounter';
        LoadActionPan('facilityDetail', params);
    },
    BindPatientAccount: function () {
        var Ctrl = $('#' + Batch_Encounter.params.PanelID + ' #txtAccountNumber');
        var func = function () { return utility.GetPatientArray(Ctrl.val(), 0) };
        var hfCtrl = $("#" + Batch_Encounter.params["PanelID"] + " #hfPatientId");
        var onSelect = function (e) { utility.InsertRecentPatient(e.id); };
        utility.BindKendoAutoComplete(Ctrl, 4, "value", "contains", null, func, hfCtrl, onSelect);
    },
    OpenPatientSearch: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "batchTabEncounter";
        LoadActionPan('Patient_Search', params);
    },
    FillPatientInfoFromSearch: function (PatientId, patFullName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        $("#" + Batch_Encounter.params["PanelID"] + " #hfPatientId").val(PatientId);
        $("#" + Batch_Encounter.params["PanelID"] + " #txtAccountNumber").val(patFullName.split(" - ")[0]);
        UnloadActionPan("batchTabEncounter");
        utility.InsertRecentPatient(PatientId);
    },
    removeDialogClasses: function () {
        $('#' + Batch_Encounter.params.PanelID + ' .modal-header').hide();
        $('#' + Batch_Encounter.params.PanelID + ' #modalbody').removeClass('panel-body');
        $('#' + Batch_Encounter.params.PanelID + ' .modal-content').removeClass('modal-content');
        $('#' + Batch_Encounter.params.PanelID + ' .modal-dialog-full').removeAttr('class');
    },

    VisitSearch: function (PageNo, rpp) {
        var self = $("#" + Batch_Encounter.params.PanelID);
        var myJSON = self.getMyJSON();
        var PatientID = null;
        if ($('#' + Batch_Encounter.params.PanelID + ' #txtAccountNumber').val() != null)
            PatientID = Number($('#' + Batch_Encounter.params.PanelID + ' #hfPatientId').val());

        var VisitStatus = $('#' + Batch_Encounter.params.PanelID + ' #ddlVisitType').val();
        //  var PageNo = 1;
        //  var rpp = 15;
        Batch_Encounter.SearchVisitsBatch(myJSON, PatientID, null, PageNo, rpp, VisitStatus).done(function (response) {
            if (response.status != false) {
                if ($("#" + Batch_Encounter.params.PanelID + " #pnlVisits_Result").css("display") == "none") {
                    $("#" + Batch_Encounter.params.PanelID + " #pnlVisits_Result").show();
                }
                if (response.VisitsLoad_JSON != "") {
                    Batch_Encounter.VisitGridLoad(response, PageNo, rpp);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    VisitGridLoad: function (response, PageNo, rpp) {

        $("#" + Batch_Encounter.params.PanelID + " #BatchVisits #dgvVisitsDetail").dataTable().fnDestroy();
        $("#" + Batch_Encounter.params.PanelID + " #BatchVisits #dgvVisitsDetail tbody").find("tr").remove();

        if (response.VisitsCount > 0) {
            var VisitsLoadJSONData = JSON.parse(response.VisitsLoad_JSON);
            $.each(VisitsLoadJSONData, function (i, item) {
                if (item.Status.toLowerCase() != "checkout")
                    gridRowId = "gvOpenVisitsDetail_row";
                var $row = $('<tr/>');
                $row.attr("onclick", "Batch_Encounter.OpenVisitDetail('" + item.VisitId + "','" + item.PatientId + "',event)");
                $row.attr("id", item.VisitId);
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
                var EditMethod = "Batch_Encounter.OpenVisitDetail('" + item.VisitId + "','" + item.PatientId + "',event)";

                var ActiveInacvtiveMethod = "Batch_Encounter.VisitActiveInactive(" + item.VisitId.trim() + "," + isactive + ",event);";
                actions = '<td><a class="btn  btn-xs" href="#" onclick="Batch_Encounter.VisitDelete(' + item.VisitId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="' + EditMethod + '" title="Edit Record"><i class="fa fa-edit black"></i></a><a class="btn  btn-xs" href="#" onclick="' + ActiveInacvtiveMethod + '" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>&nbsp;</td>';
                $row.append('<td style="display:none;">' + item.VisitId + '</td>' + actions + '<td>' + item.ClaimNumber + '</td><td>' + utility.RemoveTimeFromDate(null, item.AppointmentDate) + '</td><td>' + item.FacilityName + '</td><td>' + item.ProviderName + '</td><td>' + item.Status + '</td><td>' + item.CheckInDateTime + '</td><td>' + item.CheckInByFullName + '</td>');

                //if (item.Status.toLowerCase() != "checkout")
                $("#" + Batch_Encounter.params.PanelID + " div#BatchVisits #dgvVisitsDetail tbody").last().append($row);
            });

            var openVisitRows = $("#" + Batch_Encounter.params.PanelID + " div#BatchVisits #dgvVisitsDetail tbody").find("tr");

            if (openVisitRows.length < 1) {
                $("#" + Batch_Encounter.params.PanelID + " div#BatchVisits #dgvVisitsDetail").DataTable({
                    "language": {
                        "emptyTable": "No Open Visit Found"
                    }, "autoWidth": false, "bInfo": false, "bPaginate": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
                $("#" + Batch_Encounter.params.PanelID + " #divVisitsPaging").css("display", "none");
            }

            //------------Pagination Open Visits-----------
            $("#" + Batch_Encounter.params.PanelID + " #divVisitsPaging").css("display", "inline");
            //Showing 1 to 15 of 15 entries
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("" + Batch_Encounter.params.PanelID + " #divVisitsPaging", response.iTotalDisplayRecords, 5, "Batch_Encounter", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#" + Batch_Encounter.params.PanelID + " #divVisitsPaging #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $("#" + Batch_Encounter.params.PanelID + " #BatchVisits #divVisitsPaging li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
            //------------End Pagination-------

        }
        else {
            $("#" + Batch_Encounter.params.PanelID + " div#BatchVisits #dgvVisitsDetail").DataTable({
                "language": {
                    "emptyTable": "No Visits Found"
                }, "autoWidth": false, "bInfo": false, "bPaginate": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
            $("#" + Batch_Encounter.params.PanelID + " #divVisitsPaging").css("display", "none");
        }
        if ($.fn.dataTable.isDataTable("#" + Batch_Encounter.params.PanelID + " div#BatchVisits #dgvVisitsDetail"))
            ;
        else
            $("#" + Batch_Encounter.params.PanelID + " div#BatchVisits #dgvVisitsDetail").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "bFilter": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    //--------------Pagination functions-----

    SelectedPageClick: function (PageNo) {

        Batch_Encounter.VisitSearch(PageNo, 15);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#" + Batch_Encounter.params["PanelID"] + " #BatchVisits li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            Batch_Encounter.VisitSearch(currentPageNo, 15);
        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#" + Batch_Encounter.params["PanelID"] + " #BatchVisits li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });
        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo > 0) {
            Batch_Encounter.VisitSearch(currentPageNo, 15);
        }
    },

    //---------------Open Pagination-------

    VisitActiveInactive: function (VisitId, IsActive, event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.myConfirm('3', function () {
            var selectedValue = VisitId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Encounter_Visits.UpdateActiveInactiveVisits(selectedValue, IsActive).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        Batch_Encounter.VisitSearch(null, 15);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { }, '3', null, null, null, IsActive);
    },
    VisitDelete: function (VisitId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.myConfirm('1', function () {
            var selectedValue = VisitId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Encounter_Visits.DeleteVisit(selectedValue).done(function (response) {
                    if (response.status != false) {
                        Batch_Encounter.VisitSearch(null, 15);
                        utility.DisplayMessages(response.Message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { }, '1');
    },
    OpenVisitDetail: function (VisitId, PatientId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var params = [];
        params["ParentCtrl"] = 'batchTabEncounter';
        params["VisitId"] = VisitId;
        params["patientID"] = PatientId;
        LoadActionPan('EncounterChargeCapture', params);

    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },

    SearchVisitsBatch: function (VisitData, PatientID, VisitId, PageNumber, RowsPerPage, VisitStatus) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "VisitData=" + VisitData + "&VisitID=" + VisitId + "&PatientID=" + PatientID + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage + "&VisitStatus=" + VisitStatus;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_ENCOUNTER_VISITS", "SEARCH_VISITS_BATCH");
    },

}