Bill_FollowUpPatientAR = {
    bIsFirstLoad: true,
    params: [],
   // bVisitFirst: true,
    Load: function (params) {

        Bill_FollowUpPatientAR.params = params;
        if (Bill_FollowUpPatientAR.params.PanelID != "pnlBillFollowUpPatientAR") {
            Bill_FollowUpPatientAR.params.PanelID = Bill_FollowUpPatientAR.params.PanelID + ' #pnlBillFollowUpPatientAR';
        }
        var self = $('#' + Bill_FollowUpPatientAR.params.PanelID);
        if (Bill_FollowUpPatientAR.bIsFirstLoad) {
            Bill_FollowUpPatientAR.bIsFirstLoad = false;

            self.loadDropDowns(true).done(function () {

                Bill_FollowUpPatientAR.LoadAllControls();
                Bill_FollowUpPatientAR.ARSearch("Patient");

            });

        }
    },

    BindProvider: function () {
        var Ctrl = $("#" + Bill_FollowUpPatientAR.params.PanelID + " #frmFollowUpPatientAR #txtProvider");
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        var hfCtrl = $("#" + Bill_FollowUpPatientAR.params.PanelID + " #frmFollowUpPatientAR #hfProvider");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },
    BindFacility: function () {
        var Ctrl = $("#" + Bill_FollowUpPatientAR.params.PanelID + " #frmFollowUpPatientAR #txtFacility");
        var func = function () { return utility.GetFacilityArray(Ctrl.val()) };
        var hfCtrl = $("#" + Bill_FollowUpPatientAR.params.PanelID + " #frmFollowUpPatientAR #txtFacility");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },
    BindInsuranceAutoComplete: function () {
        var Ctrl = $("#" + Bill_FollowUpPatientAR.params.PanelID + " #frmFollowUpPatientAR #txtInsurancePlan");
        var func = function () { return utility.GetInsurancePlanArray(Ctrl.val()) };
        var hfCtrl = $("#" + Bill_FollowUpPatientAR.params.PanelID + " #frmFollowUpPatientAR #hfInsurancePlan");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    LoadAllControls: function () {
        
        utility.ValidateFromToDate('frmFollowUpPatientAR', 'dpDOSFrom', 'dpDOSTo', true);
        utility.CreateDatePicker('frmFollowUpPatientAR #dpLastModified', function () {
            //on-change callback method 
        }, false);
        Bill_FollowUpPatientAR.BindInsuranceAutoComplete();
        Bill_FollowUpPatientAR.BindFacility();
        Bill_FollowUpPatientAR.BindProvider();
        Bill_FollowUpPatientAR.BindClaimNumber();
        Bill_FollowUpPatientAR.BindPatientAccount();
    },
    FillInsurancePlanName: function (InsurancePlanId, InsurancePlanName) {
        if ($('#' + Bill_FollowUpPatientAR.params["PanelID"] + " #txtInsurancePlan").data('kendoAutoComplete'))
            utility.SetKendoAutoCompleteSourceforValidate($('#' + Bill_FollowUpPatientAR.params["PanelID"] + " #txtInsurancePlan"), InsurancePlanName, $('#' + Bill_FollowUpPatientAR.params["PanelID"] + " #hfInsurancePlan"), InsurancePlanId);
        UnloadActionPan(Bill_FollowUpPatientAR.params["ParentCtrl"]);
        //Patient_Insurance.FillInsurancePlanAddress(InsurancePlanId);
    },

    OpenInsurancePlanDetail: function () {
        //Admin_InsurancePlan.InsurancePlanEdit($("#pnlPatientInsurance #hfInsurancePlan").val());
        var params = [];
        var PanelID = null;
        params["ParentCtrl"] = 'billTabFollowUpPatientAR';
        params["InsurancePlanId"] = $("#" + Bill_FollowUpPatientAR.params.PanelID + " #hfInsurancePlan").val();
       // params["PlanAddressId"] = $("#pnlPatientInsurance #ddlPlanAddress").last().val();
        params["mode"] = "Edit";
         params["FromAdmin"] == "0";
        //params["ParentCtrl"] = 'patTabInsurance';
        LoadActionPan('insurancePlanDetail', params, PanelID);
    },

   

    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmFollowUpPatientAR";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "billTabFollowUpPatientAR";
        LoadActionPan('Admin_Facility', params);
    },
    OpenFacilityDetail: function () {
        var params = [];
        params["FacilityId"] = $('#' + Bill_FollowUpPatientAR.params.PanelID + ' #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'billTabFollowUpPatientAR';
        params["RefCtrl"] = "txtFacility";
        LoadActionPan('facilityDetail', params);
    },
    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmFollowUpPatientAR";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "billTabFollowUpPatientAR";
        LoadActionPan('Admin_Provider', params);
    },
    OpenProviderDetail: function () {
        var params = [];
        params["ProviderId"] = $('#' + Bill_FollowUpPatientAR.params.PanelID + ' #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'billTabFollowUpPatientAR';
        LoadActionPan('providerDetail', params);
    },
    OpenPatientSearch: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'billTabFollowUpPatientAR';
        LoadActionPan('Patient_Search', params);
    },
    FillPatientInfoFromSearch: function (PatientId, AccountNumber, patFullName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        if ($("#" + Bill_FollowUpPatientAR.params.PanelID + " #txtPatientName").data('kendoAutoComplete'))
            utility.SetKendoAutoCompleteSourceforValidate($("#" + Bill_FollowUpPatientAR.params.PanelID + " #txtPatientName"), AccountNumber, $("#" + Bill_FollowUpPatientAR.params.PanelID + " #hfPatientId"), PatientId, "AccountNumber");
        UnloadActionPan("billTabFollowUpPatientAR");
        utility.InsertRecentPatient(PatientId);
    },

    OpenInsurancePlan: function () {
        var params = [];
        params["InsurancePlanId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "billTabFollowUpPatientAR";
        LoadActionPan('Admin_InsurancePlan', params);
    },
    BindPatientAccount: function () {
        var Ctrl = $("#" + Bill_FollowUpPatientAR.params.PanelID + " #txtPatientName");
        var hfCtrl = $("#" + Bill_FollowUpPatientAR.params.PanelID + " #hfPatientId");
        var func = function () { return utility.GetPatientArray(Ctrl.val(), 0) };
        var onSelect = function (e) { utility.InsertRecentPatient(e.id); };
        utility.BindKendoAutoComplete(Ctrl, 4, "AccountNumber", "contains", null, func, null, onSelect);
    },
    BindClaimNumber: function () {
        var Ctrl = $('#' + Bill_FollowUpPatientAR.params.PanelID + ' #txtClaimno');
        var hfCtrl = $("#" + Bill_FollowUpPatientAR.params.PanelID + " #hfVisitId");
        var func = function () { return Bill_FollowUpPatientAR.GetClaimNumberArray(Ctrl.val()); };
        utility.BindKendoAutoComplete(Ctrl, 3, "ClaimNumber", "contains", null, func, hfCtrl);
    },
    GetClaimNumberArray: function (name) {
        var AllClaimsVisits = [];
        var dfd = new $.Deferred();
        Bill_FollowUpPatientAR.LoadClaimNumers(name).done(function (responseData) {
            if (responseData.status != false) {
                if (responseData.ClaimsCount > 0) {
                    var Claims = JSON.parse(responseData.ClaimsLoad_JSON);
                    $.each(Claims, function (i, item) {
                        AllClaimsVisits.push({ id: item.VisitId, value: item.ClaimNumber, PatientId: item.PatientId, DOSFrom: item.DOSFrom, PatientName: item.AccountNumber + ' - ' + item.PatientName, ClaimNumber: item.ClaimNumber });
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
    BillFollowUpPatientRReset: function () {
        $('#' + Bill_FollowUpPatientAR.params.PanelID + ' #frmFollowUpPatientAR').resetAllControls();
        $('#' + Bill_FollowUpPatientAR.params.PanelID + ' #frmFollowUpPatientAR #dpDOSTo').attr('disabled', true);

        if ($('#' + Bill_FollowUpPatientAR.params.PanelID + ' #frmFollowUpPatientAR #lnkInsurancePlanDetail').is(":visible")) {
            $('#' + Bill_FollowUpPatientAR.params.PanelID + ' #frmFollowUpPatientAR #lnkInsurancePlanDetail').hide();
            $('#' + Bill_FollowUpPatientAR.params.PanelID + ' #frmFollowUpPatientAR #lblInsurancePlan').show();
        }
        if ($('#' + Bill_FollowUpPatientAR.params.PanelID + ' #frmFollowUpPatientAR #lnkFacilityEdit').is(":visible")) {
            $('#' + Bill_FollowUpPatientAR.params.PanelID + ' #frmFollowUpPatientAR #lnkFacilityEdit').hide();
            $('#' + Bill_FollowUpPatientAR.params.PanelID + ' #frmFollowUpPatientAR #lblFacility').show();
        }
        if ($('#' + Bill_FollowUpPatientAR.params.PanelID + ' #frmFollowUpPatientAR #lnkProviderEdit').is(":visible")) {
            $('#' + Bill_FollowUpPatientAR.params.PanelID + ' #frmFollowUpPatientAR #lnkProviderEdit').hide();
            $('#' + Bill_FollowUpPatientAR.params.PanelID + ' #frmFollowUpPatientAR #lblProvider').show();
        }
    },
    OpenEncounter: function () {
        var params = [];
        params["FromAdmin"] = 0;
        params["ParentCtrl"] = 'billTabFollowUpPatientAR';
        params["patientID"] = 0;
        LoadActionPan('Encounter_Visits', params);
       

    },
    FillClaimNumberFromSearch: function (ClaimNumber, AccountNumber, PatientName, PatientId, DOSFrom, VisitId) {
        $("#" + Bill_FollowUpPatientAR.params.PanelID + " #txtClaimno").val(ClaimNumber);
        if ($("#" + Bill_FollowUpPatientAR.params.PanelID + " #txtClaimno").data('kendoAutoComplete'))
            utility.SetKendoAutoCompleteSourceforValidate($("#" + Bill_FollowUpPatientAR.params.PanelID + " #txtClaimno"), ClaimNumber, null, VisitId, "ClaimNumber");
        Encounter_Visits.UnLoad();
    },

    //____________ Open Screens______________________//
    OpenPatientARDetail: function (VisitId, FollowUpPatientARID, PatientId, AccountNumber, LastName, FirstName, InsurancePlanId, ProviderName, ProviderId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var params = [];
        params["ParentCtrl"] = "billTabFollowUpPatientAR";
        params["mode"] = "edit";
        params["VisitId"] = VisitId;

        // information to be passed to eligibility
        params["patientID"] = PatientId;
        params["patientAccount"] = AccountNumber;
        params["patientLastName"] = LastName;
        params["patientFirstName"] = FirstName;
        params["patientInsurancePlanId"] = InsurancePlanId;
        params["Provider"] = ProviderName;
        params["ProviderId"] = ProviderId;

        params["followUpPatientARID"] = FollowUpPatientARID;
        LoadActionPan('Bill_FollowUpPatientAR_Detail', params);

    },
    //____________ End Open Screens___________________//


    ARSearch: function (ARType, PageNo, rpp) {
        if ($("#pnlBillFollowUpPatientAR_Result").css("display") == "none") {
            $("#pnlBillFollowUpPatientAR_Result").show();
        }
        if ($("#" + Bill_FollowUpPatientAR.params.PanelID + " #pnlBillFollowUpPatientAR_Result").css("display") == "none") {
            $("#" + Bill_FollowUpPatientAR.params.PanelID + " #pnlBillFollowUpPatientAR_Result").show();
        }

        var self = "";
        self = $('#' + Bill_FollowUpPatientAR.params.PanelID);
        var myJSON = self.getMyJSONByName();

        Bill_FollowUpPatientAR.SearchAR(myJSON, ARType, PageNo, rpp).done(function (response) {
            if (response.status != false) {
                //-----------------Pagination------------
                if (response.ARVisitCount > 0) {
                    $('#' + Bill_FollowUpPatientAR.params.PanelID + " #divFollowUpARPatientPaging").css("display", "inline");
                    //Showing 1 to 15 of 15 entries
                    var RecordsPerPage = rpp != null ? rpp : 15;
                    var CurrentPage = PageNo != null ? PageNo : 1;

                    var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                    var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                    if (PageNo == null) {
                        utility.GetCustomPaging("divFollowUpARPatientPaging", response.iTotalDisplayRecords, 5, "Bill_FollowUpPatientAR", CurrentPage, RecordsPerPage);
                    }
                    var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                    var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                    $('#' + Bill_FollowUpPatientAR.params.PanelID + " #divFollowUpARPatientPaging #divShowingEntries").text(showingText);
                    // Change Background Color to Black for selected page
                    $('#' + Bill_FollowUpPatientAR.params.PanelID + " li").each(function () {
                        if ($(this).text() == CurrentPage) {
                            $(this).attr("class", "active");
                        }
                        else
                            $(this).removeAttr("class");
                    });
                }
                else {
                    $('#' + Bill_FollowUpPatientAR.params.PanelID + " #divFollowUpARPatientPaging").css("display", "none");
                }

                //--------------------End Pagination-------------------

                Bill_FollowUpPatientAR.FollowUpPatientARGridLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },
    FollowUpPatientARGridLoad: function (response) {
        $("#" + Bill_FollowUpPatientAR.params.PanelID + " #pnlBillFollowUpPatientAR_Result #dgvFollowUpPatientAR").dataTable().fnDestroy();
        $("#" + Bill_FollowUpPatientAR.params.PanelID + " #pnlBillFollowUpPatientAR_Result #dgvFollowUpPatientAR tbody").find("tr").remove();
        if (response.ARVisitCount > 0) {
            var PatientARJSONData = JSON.parse(response.ARVisitLoad_JSON);
            $.each(PatientARJSONData, function (i, item) {
                var DemographicsMethod = "utility.PatientDemographics('" + item.PatientId + "', 'billTabFollowUpPatientAR', event);";
                var VisitDetail = "utility.LoadVisitDetail('" + item.VisitId + "', '" + item.PatientId + "', 'billTabFollowUpPatientAR', event);";
                var $row = $('<tr/>');

                $row.attr("id", "gvPatientAR_row" + item.FolUpARDtlId);
                $row.attr("PatARDetailId", item.FolUpARDtlId);
                if (item.IsActive == "True") {
                    isactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }

                var deleteActionMethod = '';

                var editMethod = "Bill_FollowUpPatientAR.OpenPatientARDetail('" + item.VisitId + "','" + item.FolUpARDtlId + "','" + item.PatientId + "','" + item.AccountNumber + "','" + item.LastName + "','" + item.FirstName + "','" + item.InsurancePlanId + "','" + item.ProviderName + "','" + item.ProviderId + "',event)";
                var editActionMarkup = '<a class="btn btn-xs" href="#" onclick="' + editMethod + '" title="AR Detail"><i class="fa fa-book"></i></a>';
                $row.attr("onclick", editMethod);
                //                var editActionMethod = '<a class="btn btn-xs" href="#" onclick="Bill_FollowUpPatientAR.OpenPatientARDetail(' + item.VisitId + ',' + item.FolUpARDtlId + ');" title="AR Detail"><i class="fa fa-book"></i></a>';
                var selectActionMethod = '';

                $row.append('<td style="display:none;">' + item.FolUpARDtlId + '</td><td>' + deleteActionMethod + '&nbsp;' + editActionMarkup + selectActionMethod + '</td> <td>' + item.PracticeName + '</td> <td>' + item.FacilityName + '</td> <td>' + item.ProviderName + '</td><td>' + item.InsurancePlanName + '</td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.AccountNumber + '</a></td><td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td><td><a href="#" onclick="' + VisitDetail + '"  title="View Claim">' + item.ClaimNumber + '</a></td><td class="text-center">' + item.LastName + '</td><td class="text-center">' + item.FirstName + '</td><td>' + item.ARGroupName + '</td><td>' + item.FollowupActionName + '</td><td>' + item.FollowupReasonName + '</td><td>' + item.Suspended + '</td><td>' + item.Age + '</td><td class="text-right">' + utility.convertToFigure(item.PatCharges, true) + '</td><td class="text-right">' + utility.convertToFigure(item.PatBalance, true) + '</td><td>' + utility.RemoveTimeFromDate(null, item.FirstStatementDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.StatementDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.ModifiedOn) + '</td><td class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '">' + item.Comments + '</td>');

                $("#" + Bill_FollowUpPatientAR.params.PanelID + " #pnlBillFollowUpPatientAR_Result #dgvFollowUpPatientAR tbody").last().append($row);
            });
        }
        else {
            $('#' + Bill_FollowUpPatientAR.params.PanelID + " #divFollowUpARPatientPaging").css("display", "none");
            $("#" + Bill_FollowUpPatientAR.params.PanelID + " #pnlBillFollowUpPatientAR_Result #dgvFollowUpPatientAR").DataTable({
                "language": {
                    "emptyTable": "No FollowUp Patient AR Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable("#" + Bill_FollowUpPatientAR.params.PanelID + " #pnlBillFollowUpPatientAR_Result #dgvFollowUpPatientAR"))
            ;
        else
            $("#" + Bill_FollowUpPatientAR.params.PanelID + " #pnlBillFollowUpPatientAR_Result #dgvFollowUpPatientAR").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

    },


    SelectedPageClick: function (PageNo, objPage, TotalRecords, rpp, pagingDivId) {

        $("#pnlBillFollowUpPatientAR_Result li").each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });

        Bill_FollowUpPatientAR.ARSearch("patient", PageNo, 15);
    },
    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#pnlBillFollowUpPatientAR_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {

            Bill_FollowUpPatientAR.ARSearch("patient", currentPageNo, 15);
        }
    },
    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#pnlBillFollowUpPatientAR_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });
        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo > 0) {
            Bill_FollowUpPatientAR.ARSearch("patient", currentPageNo, 15);
        }
    },


    // _____________________DB Calls___________________________________//
    SearchAR: function (searchData, ARType, pageNo, recordPerPage) {
        if (pageNo == null) {
            pageNo = 1;
        }
        if (recordPerPage == null) {
            recordPerPage = 15;
        }

        var objData = JSON.parse(searchData);
        objData["ARType"] = ARType;
        objData["RowsPerPage"] = recordPerPage;
        objData["PageNumber"] = pageNo;
        objData["CommandType"] = "search";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "FollowUps", "FollowUpPatientAR");
    },
    // ______________________End DB Calls__________________________//



    // _____________________UNLOADING___________________________________//
    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}