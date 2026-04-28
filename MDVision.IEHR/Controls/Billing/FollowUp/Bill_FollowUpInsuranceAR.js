Bill_FollowUpInsuranceAR = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {

        Bill_FollowUpInsuranceAR.params = params;
        if (Bill_FollowUpInsuranceAR.params.PanelID != "pnlBillFollowUpInsuranceAR") {
            Bill_FollowUpInsuranceAR.params.PanelID = Bill_FollowUpInsuranceAR.params.PanelID + ' #pnlBillFollowUpInsuranceAR';
        }
        var self = $('#' + Bill_FollowUpInsuranceAR.params.PanelID);
        if (Bill_FollowUpInsuranceAR.bIsFirstLoad) {

            Bill_FollowUpInsuranceAR.bIsFirstLoad = false;

            self.loadDropDowns(true).done(function () {

                Bill_FollowUpInsuranceAR.LoadAllControls();
                Bill_FollowUpInsuranceAR.ARSearch("Insurance");

            });
        }
    },


    BindProvider: function (isFullName, shortName) {
        var Ctrl = $("#" + Bill_FollowUpInsuranceAR.params.PanelID + " #frmFollowUpInsuranceAR #txtProvider");
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        var hfCtrl = $("#" + Bill_FollowUpInsuranceAR.params.PanelID + " #frmFollowUpInsuranceAR #hfProvider");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },
    BindFacility: function () {
        var Ctrl = $("#" + Bill_FollowUpInsuranceAR.params.PanelID + " #frmFollowUpInsuranceAR #txtFacility");
        var func = function () { return utility.GetFacilityArray(Ctrl.val()) };
        var hfCtrl = $("#" + Bill_FollowUpInsuranceAR.params.PanelID + " #frmFollowUpInsuranceAR #hfFacility");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },
    BindInsuranceAutoComplete: function () {
        var Ctrl = $("#" + Bill_FollowUpInsuranceAR.params.PanelID + " #frmFollowUpInsuranceAR #txtInsurancePlan");
        var func = function () { return utility.GetInsurancePlanArray(Ctrl.val()) };
        var hfCtrl = $("#" + Bill_FollowUpInsuranceAR.params.PanelID + " #frmFollowUpInsuranceAR #hfInsurancePlan");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    LoadAllControls: function () {



        utility.ValidateFromToDate('frmFollowUpInsuranceAR', 'dpDOSFrom', 'dpDOSTo', true);
        utility.CreateDatePicker('frmFollowUpInsuranceAR #dpLastModified', function () {
            //on-change callback method 
        }, false);
        Bill_FollowUpInsuranceAR.BindFacility();
        Bill_FollowUpInsuranceAR.BindProvider();
        Bill_FollowUpInsuranceAR.BindClaimNumber();
        Bill_FollowUpInsuranceAR.BindPatientAccount();
        Bill_FollowUpInsuranceAR.BindInsuranceAutoComplete();
    },
    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmFollowUpInsuranceAR";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "billTabFollowUpInsuranceAR";
        LoadActionPan('Admin_Facility', params);
    },
    OpenFacilityDetail: function () {
        var params = [];
        params["FacilityId"] = $('#' + Bill_FollowUpInsuranceAR.params.PanelID + ' #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'billTabFollowUpInsuranceAR';
        params["RefCtrl"] = "txtFacility";
        LoadActionPan('facilityDetail', params);
    },
    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmFollowUpInsuranceAR";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "billTabFollowUpInsuranceAR";
        LoadActionPan('Admin_Provider', params);
    },
    OpenProviderDetail: function () {
        var params = [];
        params["ProviderId"] = $('#' + Bill_FollowUpInsuranceAR.params.PanelID + ' #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'billTabFollowUpInsuranceAR';
        LoadActionPan('providerDetail', params);
    },
    OpenPatientSearch: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'billTabFollowUpInsuranceAR';
        LoadActionPan('Patient_Search', params);
    },
    FillPatientInfoFromSearch: function (PatientId, AccountNumber, patFullName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        $("#" + Bill_FollowUpInsuranceAR.params.PanelID + " #hfPatientId").val(PatientId);
        $("#" + Bill_FollowUpInsuranceAR.params.PanelID + " #txtPatientName").val(AccountNumber);
        utility.SetKendoAutoCompleteSourceforValidate($("#" + Bill_FollowUpInsuranceAR.params.PanelID + " #txtPatientName"), AccountNumber, $("#" + Bill_FollowUpInsuranceAR.params.PanelID + " #hfPatientId"), PatientId, "AccountNumber");
        UnloadActionPan("billTabFollowUpInsuranceAR");
        utility.InsertRecentPatient(PatientId);
    },
    FillInsurancePlanName: function (InsurancePlanId, InsurancePlanName) {
        utility.SetKendoAutoCompleteSourceforValidate($('#' + Bill_FollowUpInsuranceAR.params["PanelID"] + " #txtInsurancePlan"), InsurancePlanName, $('#' + Bill_FollowUpInsuranceAR.params["PanelID"] + " #hfInsurancePlan"), InsurancePlanId);
        UnloadActionPan(Bill_FollowUpInsuranceAR.params["ParentCtrl"]);
        //Patient_Insurance.FillInsurancePlanAddress(InsurancePlanId);
    },

    OpenInsurancePlanDetail: function () {
        //Admin_InsurancePlan.InsurancePlanEdit($("#pnlPatientInsurance #hfInsurancePlan").val());
        var params = [];
        var PanelID = null;
        params["ParentCtrl"] = 'billTabFollowUpInsuranceAR';
        params["InsurancePlanId"] = $("#" + Bill_FollowUpInsuranceAR.params.PanelID + " #hfInsurancePlan").val();
        // params["PlanAddressId"] = $("#pnlPatientInsurance #ddlPlanAddress").last().val();
        params["mode"] = "Edit";
        params["FromAdmin"] == "0";
        //params["ParentCtrl"] = 'patTabInsurance';
        LoadActionPan('insurancePlanDetail', params, PanelID);
    },

    OpenInsurancePlan: function () {
        var params = [];
        params["InsurancePlanId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "billTabFollowUpInsuranceAR";
        LoadActionPan('Admin_InsurancePlan', params);
    },
    BindPatientAccount: function () {
        var Ctrl = $("#" + Bill_FollowUpInsuranceAR.params.PanelID + " #txtPatientName");
        var hfCtrl = $("#" + Bill_FollowUpInsuranceAR.params.PanelID + " #hfPatientId");
        var func = function () { return utility.GetPatientArray(Ctrl.val(), 0) };
        var onSelect = function (e) { utility.InsertRecentPatient(e.id); };
        utility.BindKendoAutoComplete(Ctrl, 4, "AccountNumber", "contains", null, func, null, onSelect);
    },
    BindClaimNumber: function () {
        var Ctrl = $("#" + Bill_FollowUpInsuranceAR.params.PanelID + " #txtClaimno");
        var hfCtrl = $("#" + Bill_FollowUpInsuranceAR.params.PanelID + " #hfVisitId");
        var func = function () { return Bill_FollowUpInsuranceAR.GetClaimNumberArray(Ctrl.val()); };
        utility.BindKendoAutoComplete(Ctrl, 3, "ClaimNumber", "contains", null, func, hfCtrl);
    },
    GetClaimNumberArray: function (name) {
        var AllClaimsVisits = [];
        var dfd = new $.Deferred();
        Bill_FollowUpInsuranceAR.LoadClaimNumers(name).done(function (responseData) {
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
    BillFollowUpInsuranceARReset: function () {
        $('#' + Bill_FollowUpInsuranceAR.params.PanelID + ' #frmFollowUpInsuranceAR').resetAllControls();
        $('#' + Bill_FollowUpInsuranceAR.params.PanelID + ' #frmFollowUpInsuranceAR #txtInsBalGreater').val('');
        $('#' + Bill_FollowUpInsuranceAR.params.PanelID + ' #frmFollowUpInsuranceAR #txtInsBalLess').val('');
        $('#' + Bill_FollowUpInsuranceAR.params.PanelID + ' #frmFollowUpInsuranceAR #dpDOSTo').attr('disabled', true);
        if ($('#' + Bill_FollowUpInsuranceAR.params.PanelID + ' #frmFollowUpInsuranceAR #lnkInsurancePlanDetail').is(":visible")) {
            $('#' + Bill_FollowUpInsuranceAR.params.PanelID + ' #frmFollowUpInsuranceAR #lnkInsurancePlanDetail').hide();
            $('#' + Bill_FollowUpInsuranceAR.params.PanelID + ' #frmFollowUpInsuranceAR #lblInsurancePlan').show();
        }
        if ($('#' + Bill_FollowUpInsuranceAR.params.PanelID + ' #frmFollowUpInsuranceAR #lnkFacilityEdit').is(":visible")) {
            $('#' + Bill_FollowUpInsuranceAR.params.PanelID + ' #frmFollowUpInsuranceAR #lnkFacilityEdit').hide();
            $('#' + Bill_FollowUpInsuranceAR.params.PanelID + ' #frmFollowUpInsuranceAR #lblFacility').show();
        }
        if ($('#' + Bill_FollowUpInsuranceAR.params.PanelID + ' #frmFollowUpInsuranceAR #lnkProviderEdit').is(":visible")) {
            $('#' + Bill_FollowUpInsuranceAR.params.PanelID + ' #frmFollowUpInsuranceAR #lnkProviderEdit').hide();
            $('#' + Bill_FollowUpInsuranceAR.params.PanelID + ' #frmFollowUpInsuranceAR #lblProvider').show();
        }
    },
    LoadClaimNumers: function (claimNumber) {
        var data = "ClaimNumber=" + claimNumber;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "BILLING_BATCHCHARGE_DETAIL", "SEARCH_VISIT_CLAIM");
    },
    OpenEncounter: function () {
        var params = [];
        params["FromAdmin"] = 0;
        params["ParentCtrl"] = 'billTabFollowUpInsuranceAR';
        params["patientID"] = 0;
        LoadActionPan('Encounter_Visits', params);
        //if (Bill_FollowUpInsuranceAR.bVisitFirst) {
        //    $($('body #OpenVisits')[0]).attr('id', 'OpenVisits1')
        //    $($('body #CloseVisits')[0]).attr('id', 'CloseVisits1');
        //    Bill_FollowUpInsuranceAR.bVisitFirst = false;
        //}

    },
    FillClaimNumberFromSearch: function (ClaimNumber, AccountNumber, PatientName, PatientId, DOSFrom, VisitId) {
        $("#" + Bill_FollowUpInsuranceAR.params.PanelID + " #txtClaimno").val(ClaimNumber);
        utility.SetKendoAutoCompleteSourceforValidate($("#" + Bill_FollowUpInsuranceAR.params.PanelID + " #txtClaimno"), ClaimNumber, null, VisitId, "ClaimNumber");
        Encounter_Visits.UnLoad();
    },

    //____________ Open Screens______________________//
    OpenInsuranceARDetail: function (VisitId, FollowUpInsuranceARID, PatientId, AccountNumber, LastName, FirstName, InsurancePlanId, ProviderName, ProviderId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var params = [];
        params["ParentCtrl"] = "billTabFollowUpInsuranceAR";
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

        params["FollowUpInsuranceARID"] = FollowUpInsuranceARID;


        LoadActionPan('Bill_FollowUpInsuranceAR_Detail', params);

    },
    //____________ End Open Screens___________________//


    ARSearch: function (ARType, PageNo, rpp) {
        if ($("#pnlBillFollowUpInsuranceAR_Result").css("display") == "none") {
            $("#pnlBillFollowUpInsuranceAR_Result").show();
        }
        if ($("#" + Bill_FollowUpInsuranceAR.params.PanelID + " #pnlBillFollowUpInsuranceAR_Result").css("display") == "none") {
            $("#" + Bill_FollowUpInsuranceAR.params.PanelID + " #pnlBillFollowUpInsuranceAR_Result").show();
        }

        var self = "";
        self = $('#' + Bill_FollowUpInsuranceAR.params.PanelID);
        if (!$('#' + Bill_FollowUpInsuranceAR.params.PanelID + ' #txtInsurancePlan').val()) {
            $('#' + Bill_FollowUpInsuranceAR.params.PanelID + ' #hfInsurancePlan').val('');
        }
        var myJSON = self.getMyJSONByName();
        var ParsedJson = JSON.parse(myJSON);

        if (ParsedJson.NameInitialFrom && ParsedJson.NameInitialTo && ParsedJson.NameInitialFrom.length >= 1 && ParsedJson.NameInitialTo.length >= 1) {
            var lastNameInitailTo = $.trim(ParsedJson.NameInitialTo.toLowerCase()).charCodeAt(0);
            var lastNameInitialFrom = $.trim(ParsedJson.NameInitialFrom.toLowerCase()).charCodeAt(0);
            if (lastNameInitailTo < lastNameInitialFrom) {
                utility.DisplayMessages("Please Select Last Name Initial From Greater Than  Last Name Initial To", 2);
                return;
            }

        }
        Bill_FollowUpInsuranceAR.SearchAR(myJSON, ARType, PageNo, rpp).done(function (response) {
            if (response.status != false) {
                //-----------------Pagination------------
                if (response.ARVisitCount > 0) {
                    $('#' + Bill_FollowUpInsuranceAR.params.PanelID + " #divFollowUpARInsurancePaging").css("display", "inline");
                    //Showing 1 to 15 of 15 entries
                    var RecordsPerPage = rpp != null ? rpp : 15;
                    var CurrentPage = PageNo != null ? PageNo : 1;

                    var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                    var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                    if (PageNo == null) {
                        utility.GetCustomPaging("divFollowUpARInsurancePaging", response.iTotalDisplayRecords, 5, "Bill_FollowUpInsuranceAR", CurrentPage, RecordsPerPage);
                    }
                    var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                    var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                    $('#' + Bill_FollowUpInsuranceAR.params.PanelID + " #divFollowUpARInsurancePaging #divShowingEntries").text(showingText);
                    // Change Background Color to Black for selected page
                    $('#' + Bill_FollowUpInsuranceAR.params.PanelID + " li").each(function () {
                        if ($(this).text() == CurrentPage) {
                            $(this).attr("class", "active");
                        }
                        else
                            $(this).removeAttr("class");
                    });
                }
                else {
                    $('#' + Bill_FollowUpInsuranceAR.params.PanelID + " #divFollowUpARInsurancePaging").css("display", "none");
                }

                //--------------------End Pagination-------------------

                Bill_FollowUpInsuranceAR.FollowUpInsuranceARGridLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },
    FollowUpInsuranceARGridLoad: function (response) {
        $("#" + Bill_FollowUpInsuranceAR.params.PanelID + " #pnlBillFollowUpInsuranceAR_Result #dgvFollowUpInsuranceAR").dataTable().fnDestroy();
        $("#" + Bill_FollowUpInsuranceAR.params.PanelID + " #pnlBillFollowUpInsuranceAR_Result #dgvFollowUpInsuranceAR tbody").find("tr").remove();
        if (response.ARVisitCount > 0) {
            var InsuranceARJSONData = JSON.parse(response.ARVisitLoad_JSON);
            $.each(InsuranceARJSONData, function (i, item) {

                var $row = $('<tr/>');
                $row.attr("id", "gvInsuranceAR_row" + item.FolUpARDtlId);
                //PMS-4694
                var DemographicsMethod = "utility.SelectGridRow($('#gvInsuranceAR_row" + item.FolUpARDtlId + "'));utility.PatientDemographics('" + item.PatientId + "', 'billTabFollowUpInsuranceAR', event);";
                var VisitDetail = "utility.SelectGridRow($('#gvInsuranceAR_row" + item.FolUpARDtlId + "'));utility.LoadVisitDetail('" + item.VisitId + "', '" + item.PatientId + "', 'billTabFollowUpInsuranceAR', event);";
               
                $row.attr("InsARDetailId", item.FolUpARDtlId);
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

                var deleteActionMethod = '';//<a class="btn  btn-xs" href="#" onclick="Bill_FollowUpInsuranceAR.FollowActionDelete(' + item.InsARDetailId + ');" title="Delete FollowUp Action"><i class="fa fa-close red"></i></a>';

                var editMethod = "utility.SelectGridRow($('#gvInsuranceAR_row" + item.FolUpARDtlId + "'));Bill_FollowUpInsuranceAR.OpenInsuranceARDetail('" + item.VisitId + "','" + item.FolUpARDtlId + "','" + item.PatientId + "','" + item.AccountNumber + "','" + item.LastName + "','" + item.FirstName + "','" + item.InsurancePlanId + "','" + item.ProviderName + "','" + item.ProviderId + "',event)";
                var editActionMarkup = '<a class="btn btn-xs" href="#" onclick="' + editMethod + '" title="AR Detail"><i class="fa fa-book"></i></a>';
                var selectActionMethod = '';
              
                $row.attr("onclick",editMethod);
          
           
                //start syed zia, bug #PMS-2363
                $row.append('<td style="display:none;">' + item.FolUpARDtlId + '</td><td>' + deleteActionMethod + '&nbsp;' + editActionMarkup + selectActionMethod + '</td> <td>' + item.PracticeName + '</td> <td>' + item.FacilityName + '</td> <td>' + item.ProviderName + '</td><td>' + item.InsurancePlanName + '</td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.AccountNumber + '</a></td><td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td><td class="text-center">' + item.AgingDaysDos + '</td><td><a href="#" onclick="' + VisitDetail + '"  title="View Claim">' + item.ClaimNumber + '</a></td><td class="text-center"><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient"  >' + item.LastName + '</a></td><td class="text-center"><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.FirstName + '</a></td><td>' + item.ARGroupName + '</td><td>' + item.FollowupActionName + '</td><td>' + item.FollowupReasonName + '</td><td>' + item.Suspended + '</td><td class="text-center">' + item.Age + '</td><td class="text-right">' + utility.convertToFigure(item.InsCharges, true) + '</td><td class="text-right">' + utility.convertToFigure(item.InsBalance, true) + '</td><td>' + utility.RemoveTimeFromDate(null, item.FirstSubmittedDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.SubmittedDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.ModifiedOn) + '</td><td class="ellip200" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '">' + item.Comments + '</td>');
              //  $row.append('<td style="display:none;">' + item.FolUpARDtlId + '</td><td>' + deleteActionMethod + '&nbsp;' + editActionMarkup + selectActionMethod + '</td> <td>' + item.PracticeName + '</td> <td>' + item.FacilityName + '</td> <td>' + item.ProviderName + '</td><td>' + item.InsurancePlanName + '</td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.AccountNumber + '</a></td><td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td><td class="text-center">' + item.AgingDaysDos + '</td><td><a href="#" onclick="' "utility.SelectGridRow($('#gvInsuranceAR_row" + item.FolUpARDtlId + "'));" + VisitDetail + '"  title="View Claim">' + item.ClaimNumber + '</a></td><td class="text-center"><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient"  >' + item.LastName + '</a></td><td class="text-center"><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.FirstName + '</a></td><td>' + item.ARGroupName + '</td><td>' + item.FollowupActionName + '</td><td>' + item.FollowupReasonName + '</td><td>' + item.Suspended + '</td><td class="text-center">' + item.Age + '</td><td class="text-right">' + utility.convertToFigure(item.InsCharges, true) + '</td><td class="text-right">' + utility.convertToFigure(item.InsBalance, true) + '</td><td>' + utility.RemoveTimeFromDate(null, item.FirstSubmittedDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.SubmittedDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.ModifiedOn) + '</td><td class="ellip200" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '">' + item.Comments + '</td>');
                //end syed zia, bug #PMS-2363
                $("#" + Bill_FollowUpInsuranceAR.params.PanelID + " #pnlBillFollowUpInsuranceAR_Result #dgvFollowUpInsuranceAR tbody").last().append($row);
            });
        }
        else {
            $('#' + Bill_FollowUpInsuranceAR.params.PanelID + " #divFollowUpARInsurancePaging").css("display", "none");
            $("#" + Bill_FollowUpInsuranceAR.params.PanelID + " #pnlBillFollowUpInsuranceAR_Result #dgvFollowUpInsuranceAR").DataTable({
                "language": {
                    "emptyTable": "No FollowUp Insurance AR Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable("#" + Bill_FollowUpInsuranceAR.params.PanelID + " #pnlBillFollowUpInsuranceAR_Result #dgvFollowUpInsuranceAR"))
            ;
        else
            $("#" + Bill_FollowUpInsuranceAR.params.PanelID + " #pnlBillFollowUpInsuranceAR_Result #dgvFollowUpInsuranceAR").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

    },


    SelectedPageClick: function (PageNo, objPage, TotalRecords, rpp, pagingDivId) {

        $("#pnlBillFollowUpInsuranceAR_Result li").each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });

        Bill_FollowUpInsuranceAR.ARSearch("patient", PageNo, 15);
    },
    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#pnlBillFollowUpInsuranceAR_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {

            Bill_FollowUpInsuranceAR.ARSearch("patient", currentPageNo, 15);
        }
    },
    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#pnlBillFollowUpInsuranceAR_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });
        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo > 0) {
            Bill_FollowUpInsuranceAR.ARSearch("patient", currentPageNo, 15);
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
        return MDVisionService.PMSAPIService(data, "FollowUps", "FollowUpInsuranceAR");
    },
    // ______________________End DB Calls__________________________//



    // _____________________UNLOADING___________________________________//
    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}