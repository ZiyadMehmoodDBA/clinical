Bill_ClaimSubmission = {
    bIsFirstLoad: true,
    params: [],
    ClearingHouseId: null,
    ClaimHistoryisFirstload: true,
    SubmittedbatchIsFirstload: true,
    Load: function (params) {

        Bill_ClaimSubmission.params = params;

        if (Bill_ClaimSubmission.bIsFirstLoad) {
            Bill_ClaimSubmission.bIsFirstLoad = false;
            Bill_ClaimSubmission.LoadAllAutocomplete();

            var self = $('#pnlBillClaimSubmission');
            self.loadDropDowns(true).done(function () {

                Bill_ClaimSubmission.ValidateSearch();
                //Bill_ClaimSubmission.SetDefaults();
                Bill_ClaimSubmission.ValidateBatchSearch();
                Bill_ClaimSubmission.SetSubmissionActions('ElectronicClaim');
                //Bill_ClaimSubmission.SubmitedBatch_Search();
                // Load Claim Submission History
                //Bill_ClaimSubmission.ClaimSubmitHistory_Search();
            });
        }


        utility.ValidateFromToDate('pnlBillClaimSubmission #frmClaimSubmission', 'dpDOSfrm', 'dpDOSto', true);
        utility.CreateDatePicker('pnlBillClaimSubmission #dpSubmit,#dpSubmited', function () { });

        $("#pnlBillClaimSubmission #chkAllVisits").change(function () {

            if ($(this).prop('checked'))
                $(this).attr('title', 'Unselect all');
            else
                $(this).attr('title', 'Select all');

            $("#pnlBillClaimSubmission #dgvSubmission").find("input[type='checkbox']")
                .prop('checked', this.checked);
        });

    },
    SubmitBatchReset: function () {
        $('#' + Bill_ClaimSubmission.params["PanelID"] + ' #frmSubmittedBatch').find('[data-plugin-datepicker]').each(function () { $(this).datepicker('setDate', new Date()); });
        $('#' + Bill_ClaimSubmission.params["PanelID"] + ' #frmSubmittedBatch').resetAllControls();
        $('#' + Bill_ClaimSubmission.params["PanelID"] + ' #frmSubmittedBatch [type="text"][onblur]').each(function () {
            $(this).trigger("blur");
        });
    },
    ClaimSubmitHistoryReset: function () {
        $('#' + Bill_ClaimSubmission.params["PanelID"] + ' #frmClaimSubmitHistory').find('[data-plugin-datepicker]').each(function () { $(this).datepicker('setDate', new Date()); });
        $('#' + Bill_ClaimSubmission.params["PanelID"] + ' #frmClaimSubmitHistory').resetAllControls();
        $('#' + Bill_ClaimSubmission.params["PanelID"] + ' #frmClaimSubmitHistory [type="text"][onblur]').each(function () {
            $(this).trigger("blur");
        });
    },

    SetDefaults: function () {

        //Set Patient If Selected
        var AccountNo = $("#PatientProfile #hfAccount").val();
        if (AccountNo.length > 0) {
            //Submission Claim
            $('#frmClaimSubmission #txtAccountNo').val(AccountNo);
            $("#frmClaimSubmission #hfAccount").val(AccountNo);
        }
        else {
            //Submission Claim
            $('#frmClaimSubmission #txtAccountNo').val('');
            $("#frmClaimSubmission #hfAccount").val('');
        }

        //Set Default Provider
        $('#frmClaimSubmission #txtProvider').val(globalAppdata['DefaultProviderName']);
        $('#frmClaimSubmission #hfProvider').val(globalAppdata['DefaultProviderId']);
        //Set Default Entity
        $('#frmClaimSubmission #txtFacility').val(globalAppdata['DefaultFacilityName']);
        $('#frmClaimSubmission #hfFacility').val(globalAppdata['DefaultFacilityId']);
        //Set Default Provide
        $('#frmClaimSubmission #txtProvider').val(globalAppdata['DefaultProviderName']);
        $('#frmClaimSubmission #hfProvider').val(globalAppdata['DefaultProviderId']);
        //Set Default Practice
        $('#frmClaimSubmission #txtPractice').val(globalAppdata['DefaultPracticeName']);
        $('#frmClaimSubmission #hfPractice').val(globalAppdata['DefaultPracticeId']);
    },

    ValidateSearch: function () {
        $('#pnlBillClaimSubmission #frmClaimSubmission')
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                    ClearingHouse: {
                        group: '.col-md-2',
                        enabled: false,
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                }
            })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            Bill_ClaimSubmission.Claim_Search();
            //e.stopImmediatePropagation();
        });
    },

    LoadAllAutocomplete: function () {

        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            var Ctrl = $("#frmClaimSubmission #txtProvider");
            var hfCtrl = $("#frmClaimSubmission #hfProvider");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl);
        });

        CacheManager.BindCodes('GetChargeBatches', false).done(function (result) {
            var Ctrl = $("#frmClaimSubmission #txtChargeBatchNumber");
            var hfCtrl = $("#frmClaimSubmission #hfChargeBatchNumber");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Batches, null, hfCtrl);
        });
        CacheManager.BindCodes('GetFacility', false).done(function (result) {
            var Ctrl = $("#frmClaimSubmission #txtFacility");
            var hfCtrl = $("#frmClaimSubmission #hfFacility");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl);
        });

        CacheManager.BindCodes('GetInsurancePlan', false).done(function (result) {
            var Ctrl = $("#frmClaimSubmission #txtInsurancePlan");
            var hfCtrl = $("#frmClaimSubmission #hfInsurancePlan");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", InsurancePlans, null, hfCtrl);
        });

        CacheManager.BindCodes('GetPractice', false).done(function (result) {
            var Ctrl = $("#frmClaimSubmission #txtPractice");
            var hfCtrl = $("#frmClaimSubmission #hfPractice");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Practices, null, hfCtrl);
        });
        Bill_ClaimSubmission.BindClaimNumber();
        Bill_ClaimSubmission.BindSubmittedClaimNumber();
        Bill_ClaimSubmission.BindAutocomplete();
        Bill_ClaimSubmission.BindAccountAutocomplete();
        //Submission Claim
        //CacheManager.BindCodes('GetClearingHouse', false).done(function (result) {

        //    $("#frmClaimSubmission #txtClearingHouse").autocomplete({
        //        source: ClearingHouse, // pass an array (without a comma)
        //        select: function (event, ui) {
        //            setTimeout(function () {
        //                $("#frmClaimSubmission #hfClearingHouse").val(ui.item.id);
        //            }, 100);
        //        }
        //    });
        //});

    },

    BindAutocomplete: function () {
        var Ctrl = $("#frmClaimSubmission #txtAccountNo");
        var hfCtrl = $("#frmClaimSubmission #hfAccount");
        var func = function () { return utility.GetPatientArray(Ctrl.val(), 0) };
        var onSelect = function (e) { utility.InsertRecentPatient(e.id); };
        var onChange = function (valid) { hfCtrl.val(Ctrl.val()); };
        utility.BindKendoAutoComplete(Ctrl, 4, "AccountNumber", "contains", null, func, null, onSelect, onChange);
    },


    BindAccountAutocomplete: function () {
        var Ctrl = $("#frmClaimSubmitHistory #txtAccountNumber");
        var hfCtrl = $("#frmClaimSubmitHistory #hfAccountNo");
        var func = function () { return utility.GetPatientArray(Ctrl.val(), 0) };
        var onSelect = function (e) { utility.InsertRecentPatient(e.id); };
        var onChange = function (valid) { hfCtrl.val(Ctrl.val()); };
        utility.BindKendoAutoComplete(Ctrl, 4, "AccountNumber", "contains", null, func, null, onSelect, onChange);
    },
    OpenChargeBatchNumber: function () {


        var params = [];
        params["FromAdmin"] = "0";
        params["RefCtrl"] = Bill_ClaimSubmission.GetCurrentFormId() + " #txtChargeBatchNumber";
        params["RefHiddenCtrl"] = Bill_ClaimSubmission.GetCurrentFormId() + " #hfChargeBatchNumber";
        LoadActionPan('Bill_ChargeBatchSearch', params);

    },

    OpenInsurancePlan: function () {


        var params = [];
        params["InsurancePlanId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = Bill_ClaimSubmission.GetCurrentFormId() + " #txtInsurancePlan";
        params["RefHiddenIdCtrl"] = Bill_ClaimSubmission.GetCurrentFormId() + " #hfInsurancePlan";

        LoadActionPan('Admin_InsurancePlan', params);
    },

    OpenPractice: function () {
        var params = [];
        params["PracticeId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = Bill_ClaimSubmission.GetCurrentFormId() + " #txtPractice";
        params["RefHiddenIdCtrl"] = Bill_ClaimSubmission.GetCurrentFormId() + " #hfPractice";
        LoadActionPan('Admin_Practice', params);
    },

    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmClaimSubmission";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = Bill_ClaimSubmission.GetCurrentFormId() + " #txtFacility";
        params["RefHiddenIdCtrl"] = Bill_ClaimSubmission.GetCurrentFormId() + " #hfFacility";
        LoadActionPan('Admin_Facility', params);
    },

    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmClaimSubmission";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrlHidden"] = Bill_ClaimSubmission.GetCurrentFormId() + " #hfProvider";
        params["RefCtrl"] = Bill_ClaimSubmission.GetCurrentFormId() + " #txtProvider";
        LoadActionPan('Admin_Provider', params);
    },

    OpenPatientAccount: function (IsFill) {

        Bill_ClaimSubmission.params["IsFill"] = IsFill;
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'billTabClaimSubmission';
        LoadActionPan('Patient_Search', params);
    },

    ResetBatchHiddenValue: function () {

        $('#' + Bill_ClaimSubmission.GetCurrentFormId() + ' #hfChargeBatchNumber').val("");
    },

    FillPatientInfoFromSearch: function (AccountNo, PatientId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var Ctrl;
        if (Bill_ClaimSubmission.params["IsFill"] == true) {
            $('#' + Bill_ClaimSubmission.GetCurrentFormId() + ' #hfAccount').val(AccountNo);
            Ctrl = $('#' + Bill_ClaimSubmission.GetCurrentFormId() + ' #txtAccountNo');
        }
        else {
            $('#' + Bill_ClaimSubmission.GetCurrentFormId() + ' #hfAccountNo').val(AccountNo);
            Ctrl = $('#' + Bill_ClaimSubmission.GetCurrentFormId() + ' #txtAccountNumber');
        }
        if (Ctrl.data('kendoAutoComplete'))
            utility.SetKendoAutoCompleteSourceforValidate(Ctrl, AccountNo, null, PatientId, "AccountNumber");
        UnloadActionPan("billTabClaimSubmission");
        utility.InsertRecentPatient(PatientId);
    },

    Claim_Search: function (PageNo, rpp) {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Claim Submission", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                var self = $("#pnlBillClaimSubmission #frmClaimSubmission");
                var myJSON = self.getMyJSONByName();
                Bill_ClaimSubmission.ClearingHouseId = $("#frmClaimSubmission #ddlClearinghouse").val();
                Bill_ClaimSubmission.SearchClaim(myJSON, PageNo, rpp).done(function (response) {

                    if (response.status != false) {
                        Bill_ClaimSubmission.LoadClaimGrid(response, PageNo, rpp);
                    }
                    else {
                        //utility.DisplayMessages(response.Message, 3);
                    }

                });

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    // Code for Submitted Batch
    View_Claims: function (_837BatchId) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }

        var params = [];
        params["_837BatchId"] = _837BatchId;
        params["ParentCtrl"] = 'billTabClaimSubmission';
        LoadActionPan('EDIClaimViewDetail', params);
    },

    View_EDIFile: function (_837BatchId) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }

        var self = $("#frmViewEDIFile");
        var myJSON = self.getMyJSON();


        $("#viewEDIFileData #txtTextView").val('');
        Bill_ClaimSubmission.ViewEDIFile(_837BatchId).done(function (response) {
            if (response.status != false) {
                var edi_fileData = JSON.parse(response.EDI_FileDataJSON);
                if (edi_fileData.txtTextView != "") {
                    utility.bindMyJSON(true, edi_fileData, false, self).done(function () {
                        //  $("#viewEDIFileData #txtTextView").val(edi_fileData.txtTextView);
                        // Open EDI File Viewer in Popup //Bill_ClaimSubmission
                        $("#pnlBillClaimSubmission #actionPanBillClaimSubmission").prepend($("#viewEDIFileData").html());
                        $("#pnlBillClaimSubmission #actionPanBillClaimSubmission").modal({
                            show: 'true',
                            backdrop: 'static',
                            keyboard: false

                        });
                    });
                }
                else {
                    utility.DisplayMessages('No data is found', 2);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    View_BatchDetail: function (_837BatchId, BatchControlNo, EntityId) {
        var params = [];
        params["_837BatchId"] = _837BatchId;
        params["BatchControlNo"] = BatchControlNo;
        params["EntityId"] = EntityId;
        params["ParentCtrl"] = "billTabClaimSubmission";
        LoadActionPan('EDIBatchDetail', params);
    },


    Create_EDIFile: function () {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Claim Submission", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                var visits = Bill_ClaimSubmission.GetSelectedVisits();
                if (visits.length > 0) {

                    Bill_ClaimSubmission.CreateEDIFile(visits).done(function (response) {

                        if (response.status != false) {

                            $("#pnlBillClaimSubmission #actionPanBillClaimSubmission").prepend($("#viewEDIFileData").html());
                            $("#pnlBillClaimSubmission #actionPanBillClaimSubmission").modal({
                                show: 'true',
                                backdrop: 'static',
                                keyboard: false
                            });

                            $("#pnlBillClaimSubmission #txtTextView").val(response.EDI_JSON);

                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }

                    });
                }
                else {
                    utility.DisplayMessages("Please select claim.", 2);
                }

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    Scrub_Claims: function () {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Claim Submission", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                var visits = Bill_ClaimSubmission.GetSelectedVisits();
                if (visits.length > 0) {

                    Bill_ClaimSubmission.ScrubClaims(visits).done(function (response) {

                        if (response.status != false) {

                            if (response.ErrorsCount > 0)
                                utility.DisplayMessages(response.Message, 4);
                            else
                                utility.DisplayMessages(response.Message, 1);

                            var ScrubClaimJSONData = JSON.parse(response.ScrubClaim_JSON);
                            $.each(ScrubClaimJSONData.Claims, function (i, item) {

                                var $tr = $("#pnlBillClaimSubmission #pnlSubmission_Result #dgvSubmission tbody tr[visitid='" + item.VisitId + "']");
                                if ($tr && item.HasErrors == true) {
                                    $($tr).removeClass("active").removeClass("bg-info");
                                    $($tr).css("background", "#ff9999");
                                }
                                else if ($tr && item.HasErrors == false) {
                                    $($tr).css("background", "");
                                }
                            });
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }

                    });
                }
                else {
                    utility.DisplayMessages("Please select claim.", 2);
                }

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },


    ValidateBatchSearch: function () {
        $('#frmSubmittedBatch')
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                    Entity: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                }
            })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            Bill_ClaimSubmission.SubmitedBatch_Search(true);
            //e.stopImmediatePropagation();
        });
    },

    SubmitedBatch_Search: function (SubmittedIsFirstload, PageNo, rpp) {

        if (Bill_ClaimSubmission.SubmittedbatchIsFirstload == true || SubmittedIsFirstload == true) {
            Bill_ClaimSubmission.SubmittedbatchIsFirstload = false;
            var self = $("#pnlBillClaimSubmission #frmSubmittedBatch");
            var myJSON = self.getMyJSONByName();
            Bill_ClaimSubmission.SearchSubmittedBatch(myJSON, PageNo, rpp).done(function (response) {

                if (response.status != false) {
                    Bill_ClaimSubmission.LoadSubmitedBatchGrid(response, PageNo, rpp);
                }
                else {
                    //utility.DisplayMessages(response.Message, 3);
                }

            });
        }
    },

    ClaimSubmitHistory_Search: function (ClaimHistoryFirstload, PageNo, rpp) {

        if (Bill_ClaimSubmission.ClaimHistoryisFirstload == true || ClaimHistoryFirstload == true) {
            Bill_ClaimSubmission.ClaimHistoryisFirstload = false;

            var strMessage = "";
            AppPrivileges.GetFormPrivileges("Claim Submission", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {

                    var self = $("#pnlBillClaimSubmission #frmClaimSubmitHistory");
                    var myJSON = self.getMyJSONByName();

                    Bill_ClaimSubmission.SearchClaimSubmitHistory(myJSON, PageNo, rpp).done(function (response) {

                        if (response.status != false) {
                            Bill_ClaimSubmission.LoadClaimSubmitHistoryGrid(response, PageNo, rpp);
                        }
                        else {
                            //utility.DisplayMessages(response.Message, 3);
                        }
                    });

                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
    },

    SearchClaim: function (ClaimData, PageNumber, RowsPerPage) {


        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 100 : RowsPerPage;

        var objData = JSON.parse(ClaimData);

        objData["PageNo"] = PageNumber;
        objData["CommandType"] = "Search";
        objData["rpp"] = RowsPerPage;

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Billing", "SearchPatientClaim");
    },

    PrintClaim: function (Visits, isSubmit, MarkSubmitted) {

        var objData = new Object();

        objData["Visits"] = Visits.toString();
        objData["isSubmit"] = isSubmit;
        objData["MarkSubmitted"] = MarkSubmitted;
        objData["ClearingHouse"] = Bill_ClaimSubmission.ClearingHouseId;
        objData["UserBrowser"] = utility.UserBrowser().toLowerCase();

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Billing", "PrintPatientClaim");
    },

    TransmitClaim: function (Visits, MarkSubmitted) {
        var objData = new Object();

        objData["Visits"] = Visits.toString();
        objData["MarkSubmitted"] = MarkSubmitted;
        objData["ClearingHouse"] = Bill_ClaimSubmission.ClearingHouseId;

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Billing", "TransmitPatientClaim");
    },

    LoadClaimGrid: function (response, PageNo, rpp) {


        //Set Custom Paging
        if (response.ClaimCount > 0) {
            $("#pnlBillClaimSubmission #divSubmissionPaging").css("display", "inline");
            //Showing 1 to 15 of 15 entries
            var RecordsPerPage = rpp != null ? rpp : 100;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("divSubmissionPaging", response.iTotalDisplayRecords, 5, "Bill_ClaimSubmission", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#pnlBillClaimSubmission #divSubmissionPaging #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $("#pnlBillClaimSubmission #pnlSubmission_Result li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                } else
                    $(this).removeAttr("class");
            });
        } else {
            $("#pnlBillClaimSubmission #divSubmissionPaging").css("display", "none");
            var showingText = "Showing 0 Record(s)";
            $("#pnlBillClaimSubmission #divSubmissionPaging #divShowingEntries").text(showingText);
        }

        //Bind Data in Table
        $("#pnlBillClaimSubmission #dgvSubmission").dataTable().fnDestroy();
        $("#pnlBillClaimSubmission #pnlSubmission_Result #dgvSubmission tbody").find("tr").remove();

        if (response.ClaimCount > 0) {

            $("#pnlBillClaimSubmission #pnlSubmission_Result").css("display", "");

            var claimTypeClass = "";
            var claimTitle = "";


            var ClaimLoadJSONData = JSON.parse(response.ClaimLoad_JSON);
            $.each(ClaimLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($(this));Bill_ClaimSubmission.SelectClaim(this,event)");
                $row.attr("VisitId", item.VisitId);
                $row.attr("ClaimNumber", item.ClaimNumber);
                $row.attr("PatientId", item.PatientId);


                if (item.IsPrimary == "False") {
                    claimTypeClass = "bg-info";
                    claimTitle = "Primary Claim";
                }
                else {
                    claimTypeClass = "";
                    claimTitle = "Non Primary Claim";
                }
                $row.addClass(claimTypeClass);

                var billType = "";
                if (item.ClaimType.toLowerCase() == "professional medical")
                    billType = "P";
                else if (item.ClaimType.toLowerCase() == "institutional")
                    billType = "I"
                else if (item.ClaimType.toLowerCase() == "dental")
                    billType = "D";
                else if (item.ClaimType.toLowerCase() == "professional anesthesia")
                    billType = "A"

                var bgcolor = "";
                claimTitle = 'title="' + claimTitle + '"';
                if (item.HasSubmissionError.toLowerCase() == 'true')
                {
                    bgcolor = '#5f1b34';
                    claimTitle = 'title="' + item.SubmissionError + '" data-toggle="tooltip"';
                }


                billType = item.ClaimType;
                //bug #PMS-2485,add batch number
                $row.append('<td style="display:none;">' + item.VisitId + '</td><td><input type="checkbox" onclick="Bill_ClaimSubmission.CheckedVisit(this,event)" name="checkbox" id="' + item.VisitId + '"></td><td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td><td ' + claimTitle + ' style="background-color:' + bgcolor + '" ><a href="#" onclick="Bill_ClaimSubmission.LoadVisitDetail(' + item.VisitId + ',' + item.PatientId + ',event);"  title="View Claim Detail">' + item.ClaimNumber + '</a></td><td><a href="#" onclick="Bill_ClaimSubmission.PatientDemographics(' + item.PatientId + ',event);"  title="View Patient">' + item.AccountNumber + '</a></td><td><a href="#" onclick="Bill_ClaimSubmission.PatientDemographics(' + item.PatientId + ',event);"  title="View Patient">' + item.PatientName + '</a></td><td>' + item.PlanName + '</td><td>' + item.SubscriberId + '</td><td>' + item.RefProvider + '</td><td>' + item.ProviderName + '</td><td>' + item.FacilityName + '</td><td class="text-right">' + utility.convertToFigure(item.Fee, true) + '</td><td class="text-right">' + utility.convertToFigure(item.InsCharges, true) + '</td><td class="text-right">' + utility.convertToFigure(item.Balance, true) + '</td><td data-toggle="tooltip" title="' + item.ClaimType + '" >' + billType + '</td><td>' + utility.RemoveTimeFromDate(null, item.ReadyToSubmitOn) + '</td><td>' + item.BatchNumber + '</td>');
                //bug #PMS-2485,add batch number
                //$row.append('<td style="display:none;">' + item.VisitId + '</td><td><input type="checkbox" name="checkbox" id="' + item.VisitId + '"></td><td>' + item.DOSFrom + '</td><td>' + item.AccountNumber + '</td><td>' + item.PatientName + '</td><td>' + item.PlanName + '</td><td>' + item.SubscriberId + '</td><td>' + item.ProviderName + '</td><td>' + item.FacilityName + '</td><td>' + item.Fee + '</td><td>' + item.InsCharges + '</td><td>' + item.Balance + '</td>');

                if (item.HasError.toLowerCase() == "true") {
                    $($row).removeClass("active").removeClass("bg-info");
                    $($row).css("background", "#FF6A6A");
                }

                $("#pnlBillClaimSubmission #dgvSubmission tbody").last().append($row);
            });
        }
        else {
            $("#pnlBillClaimSubmission #dgvSubmission").dataTable().fnDestroy();
            $("#pnlBillClaimSubmission #dgvSubmission").DataTable({
                "language": {
                    "emptyTable": "No Patient Claim  Found."
                }, "autoWidth": false, "bLengthChange": false, "bFilter": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bPaginate": false, "bInfo": false
            });
        }
        if ($.fn.dataTable.isDataTable("#pnlBillClaimSubmission #dgvSubmission"))
            ;
        else
            $("#pnlBillClaimSubmission #dgvSubmission").DataTable({ "bLengthChange": false, "autoWidth": false, "bFilter": false, "aaSorting": [[1, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bPaginate": false, "bInfo": false });

        //$("#pnlBillClaimSubmission #pnlSubmission_Count").html(response.iTotalDisplayRecords);
        if ($("#pnlClaimSubmission_Search #divSubmissionPaging #divShowingEntries").html())
            $("#pnlBillClaimSubmission #pnlSubmission_Count").html($("#pnlClaimSubmission_Search #divSubmissionPaging #divShowingEntries").html().replace('Record(s)', 'Patient Claims'));
        //$("#pnlBillClaimSubmission #divSubmissionPaging").addClass("hidden");

        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
    },

    SelectClaim: function (obj, event) {
        $(obj).find("[type=checkbox]").click();
    },

    SearchSubmittedBatch: function (SubmitBatchData, PageNumber, RowsPerPage) {

        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 15 : RowsPerPage;

        var objData = JSON.parse(SubmitBatchData);

        objData["PageNo"] = PageNumber;
        objData["CommandType"] = "Search";
        objData["rpp"] = RowsPerPage;

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Billing", "SearchSubmittedBatch");
    },

    SearchClaimSubmitHistory: function (ClaimData, PageNumber, RowsPerPage) {

        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 15 : RowsPerPage;

        var objData = JSON.parse(ClaimData);

        objData["PageNo"] = PageNumber;
        objData["CommandType"] = "Search";
        objData["rpp"] = RowsPerPage;

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Billing", "SearchClaimSubmitionHistory");
    },

    LoadSubmitedBatchGrid: function (response, PageNo, rpp) {

        //Set Custom Paging
        if (response.SubmittedBatchCount > 0) {
            $("#pnlBillClaimSubmission #divSubmittedBatchPaging").css("display", "inline");
            //Showing 1 to 15 of 15 entries
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("divSubmittedBatchPaging", response.iTotalDisplayRecords, 5, "Bill_ClaimSubmission", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#pnlBillClaimSubmission #divSubmittedBatchPaging #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $("#pnlBillClaimSubmission #pnlSubmittedBatch_Result li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                } else
                    $(this).removeAttr("class");
            });
        } else {
            $("#pnlBillClaimSubmission #divSubmittedBatchPaging").css("display", "none");
        }


        //Bind Data in Table
        $("#pnlBillClaimSubmission #dgvSubmittedBatch").dataTable().fnDestroy();
        $("#pnlBillClaimSubmission #dgvSubmittedBatch tbody").find("tr").remove();
        if (response.SubmittedBatchCount > 0) {
            $("#pnlBillClaimSubmission #pnlSubmittedBatch_Result").css("display", "");

            var SubmittedBatchLoadJSONData = JSON.parse(response.SubmittedBatchLoad_JSON);
            $.each(SubmittedBatchLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", 'utility.SelectGridRow($(this));Bill_ClaimSubmission.RememberSelectedRow($(this)); Bill_ClaimSubmission.View_BatchDetail(' + item.BatchId + ',"' + item.BatchControlNo + '",' + item.EntityId + ');');
                $row.attr("VisitId", item.BatchId);

                var IsCompleted = "";
                if (item.IsCompleted == "True")
                    IsCompleted = "Yes";
                else
                    IsCompleted = "No";

                var mark_as_submitted = "";
                if (item.SubmitTypeStatus.toLowerCase() == "paper" && item.MarkSubmitted.toLowerCase() == "true")
                    mark_as_submitted = "/Mark as Submitted";
                else if (item.SubmitTypeStatus.toLowerCase() == "electronic" && item.MarkSubmitted.toLowerCase() == "true")
                    mark_as_submitted = "/Mark as Transmitted";

                //$row.append('<td style="display:none;">' + item.BatchId + '</td><td><a class="btn  btn-xs" href="#" onclick="Bill_ClaimSubmission.View_Claims(' + item.BatchId + ');" title="View Claim"><i class="fa fa-calculator black"></i></a>&nbsp;<a class="btn btn-xs" href="#" id=' + item.BatchId + ' onclick="Bill_ClaimSubmission.View_EDIFile(' + item.BatchId + ');"  title="View EDI File"><i class="fa fa-file blue"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Bill_ClaimSubmission.View_BatchDetail(' + item.BatchId + ',' + item.BatchControlNo + ',' + item.EntityId + ');" title="View Batch Detail"><i class="fa fa-edit black"></i></a></td><td>' + item.BatchControlNo + '</td><td>' + item.SubmitDate + '</td><td>' + item.CreatedBy + '</td><td>' + item.SubmitTypeStatus + '</td><td>' + item.ClearingHouseName + '</td><td>' + item.TotalClaims + '</td><td>' + IsCompleted + '</td><td>' + item.BatchStatus + '</td>');
                $row.append('<td style="display:none;">' + item.BatchId + '</td><td><a class="btn  btn-xs" href="#" onclick="Bill_ClaimSubmission.View_Claims(' + item.BatchId + ');" title="View Claim"><i class="fa fa-calculator black"></i></a>&nbsp;<a class="btn btn-xs" href="#" id=' + item.BatchId + ' onclick="Bill_ClaimSubmission.View_EDIFile(' + item.BatchId + ');"  title="View EDI File"><i class="fa fa-file blue"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Bill_ClaimSubmission.View_BatchDetail(' + item.BatchId + ',' + item.BatchControlNo + ',' + item.EntityId + ');" title="View Batch Detail"><i class="fa fa-edit black"></i></a></td><td>' + item.BatchControlNo + '</td><td>' + /*utility.RemoveTimeFromDate(null,*/item.SubmitDate/*)*/ + '</td><td>' + item.CreatedBy + '</td><td>' + item.SubmitTypeStatus + mark_as_submitted + '</td><td>' + item.ClearingHouseName + '</td><td>' + item.TotalClaims + '</td><td>' + item.BatchStatus + '</td>');

                $("#pnlBillClaimSubmission #dgvSubmittedBatch tbody").last().append($row);
            });
        }
        else {

            $("#pnlBillClaimSubmission #pnlSubmittedBatch_Result").css("display", "");
            $("#pnlBillClaimSubmission #dgvSubmittedBatch").dataTable().fnDestroy();
            $("#pnlBillClaimSubmission #dgvSubmittedBatch").DataTable({
                "language": {
                    "emptyTable": "No Submit Batch  Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bPaginate": false, "bInfo": false
            });
        }

        //selected row remain selected even grid refreshed.
        if (Bill_ClaimSubmission.params["SelectedVisitId"] != "" && Bill_ClaimSubmission.params["SelectedVisitId"] != undefined)
            $("#pnlBillClaimSubmission #dgvSubmittedBatch").find("tr[visitid=" + Bill_ClaimSubmission.params["SelectedVisitId"] + "]").addClass("active");

        if ($.fn.dataTable.isDataTable("#pnlBillClaimSubmission #dgvSubmittedBatch"))
            ;
        else
            $("#pnlBillClaimSubmission #dgvSubmittedBatch").DataTable({ "bLengthChange": false, "autoWidth": false, "aaSorting": [[1, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bPaginate": false, "bInfo": false });
    },

    LoadClaimSubmitHistoryGrid: function (response, PageNo, rpp) {

        //Set Custom Paging
        if (response.ClaimSubmitHistoryCount > 0) {
            $("#pnlBillClaimSubmission #divClaimSubmitHistoryPaging").css("display", "inline");
            //Showing 1 to 15 of 15 entries
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("divClaimSubmitHistoryPaging", response.iTotalDisplayRecords, 5, "Bill_ClaimSubmission", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#pnlBillClaimSubmission #divClaimSubmitHistoryPaging #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $("#pnlBillClaimSubmission #pnlClaimSubmitHistory_Result li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                } else
                    $(this).removeAttr("class");
            });
        } else {
            $("#pnlBillClaimSubmission #divClaimSubmitHistoryPaging").css("display", "none");
        }


        //Bind Data in Table
        $("#pnlBillClaimSubmission #dgvClaimSubmitHistory").dataTable().fnDestroy();
        $("#pnlBillClaimSubmission #dgvClaimSubmitHistory tbody").find("tr").remove();
        if (response.ClaimSubmitHistoryCount > 0) {
            $("#pnlBillClaimSubmission #pnlClaimSubmitHistory_Result").css("display", "");

            var ClaimSubmitHistoryLoadJSONData = JSON.parse(response.ClaimSubmitHistoryLoad_JSON);
            $.each(ClaimSubmitHistoryLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($(this));");

                var SubmitBatch = "Bill_ClaimSubmission.View_BatchDetail(" + item.BatchId + ", " + item.BatchNumber + ", " + item.EntityId + ");"

                var submittype = "";
                var mark_as_submitted = "";
                if (item.SubmitType.toLowerCase() == "true")
                    submittype = "Electronic";
                else if (item.SubmitType.toLowerCase() == "false")
                    submittype = "Paper";

                if (item.SubmitType.toLowerCase() == "false" && item.IsMarkSubmitted.toLowerCase() == "true")
                    mark_as_submitted = "/Mark as Submitted";
                else if (item.SubmitType.toLowerCase() == "true" && item.IsMarkSubmitted.toLowerCase() == "true")
                    mark_as_submitted = "/Mark as Transmitted";


                $row.append(
                        '<td><a href="#" onclick="Bill_ClaimSubmission.PatientDemographics(' + item.PatientId + ',event);"  title="View Patient">' + item.AccountNumber + '</a></td>' +
                        '<td><a href="#" onclick="Bill_ClaimSubmission.PatientDemographics(' + item.PatientId + ',event);"  title="View Patient">' + item.PatientName + '</a></td>' +
                        '<td>' + item.InsuranceName + '</td>' +
                        '<td><a href="#" onclick="Bill_ClaimSubmission.LoadVisitDetail(' + item.VisitId + ',' + item.PatientId + ',event);"  title="View Claim Detail">' + item.ClaimNumber + '</a></td>' +
                        '<td><a href="#" onclick="' + SubmitBatch + '" title="View Batch Detail">' + item.BatchNumber + '</td>' +
                        '<td>' + submittype + mark_as_submitted + '</td>' +
                        '<td>' + item.SubmittedBy + '</td>' +
                        '<td>' + item.SubmittedTo + '</td>' +
                        '<td>' + item.SubmittedOn + '</td>');

                $("#pnlBillClaimSubmission #dgvClaimSubmitHistory tbody").last().append($row);
            });
        }
        else {

            $("#pnlBillClaimSubmission #pnlClaimSubmitHistory_Result").css("display", "");
            $("#pnlBillClaimSubmission #dgvClaimSubmitHistory").dataTable().fnDestroy();
            $("#pnlBillClaimSubmission #dgvClaimSubmitHistory").DataTable({
                "language": {
                    "emptyTable": "No Record  Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bPaginate": false, "bInfo": false
            });
        }

        if ($.fn.dataTable.isDataTable("#pnlBillClaimSubmission #dgvClaimSubmitHistory"))
            ;
        else
            $("#pnlBillClaimSubmission #dgvClaimSubmitHistory").DataTable({ "bLengthChange": false, "autoWidth": false, "aaSorting": [[8, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bPaginate": false, "bInfo": false });

    },

    RememberSelectedRow: function (obj) {
        Bill_ClaimSubmission.params["SelectedVisitId"] = obj.attr("visitid");
    },

    ViewEDIFile: function (_837BatchId) {

        var objData = new Object();
        objData["_837BatchId"] = _837BatchId;

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Billing", "ViewEDIFile");
    },

    CreateEDIFile: function (Visits) {
        var objData = new Object();
        objData["Visits"] = Visits.toString();
        objData["ClearingHouse"] = Bill_ClaimSubmission.ClearingHouseId;

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Billing", "CreateEDIFile");
    },

    ScrubClaims: function (Visits) {

        var objData = new Object();
        objData["Visits"] = Visits.toString();

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Billing", "ScrubClaims");
    },

    CheckedSubmittedBatches: function (obj) {
        if (!$(obj).prop('checked')) {
            $("#pnlBillClaimSubmission #chkAllBatch").prop('checked', false);
            $("#pnlBillClaimSubmission #chkAllBatch").attr('title', 'Select all');
        }

    },

    GetCurrentFormId: function () {

        var current_form = $("#pnlBillClaimSubmission #tab_Container").children('.tab-pane.active').find('form');
        return formId = $(current_form).attr('id');
    },

    GetSelectedVisits: function () {

        var Visits = [];
        $("#pnlBillClaimSubmission #dgvSubmission").find("input[type='checkbox']").each(function (index) {
            if (index > 0 && $(this).prop('checked'))
                Visits.push($(this).attr("id"));
        });
        return Visits;
    },

    SetSubmissionActions: function (action_control) {

        var objClaimSubmission = $('#pnlBillClaimSubmission #frmClaimSubmission');
        var formValidation = objClaimSubmission.data("bootstrapValidator");

        if (action_control == "ElectronicClaim") {

            //enable clearing house required
            formValidation.enableFieldValidators('ClearingHouse', true);
            $("#pnlBillClaimSubmission #sp_required").css("display", "inline-block");

            $("#pnlPaperClaim_Action").fadeOut(function () {
                $("#pnlElectronicClaim_Action").fadeIn();
                $("#pnlBillClaimSubmission #ddlSubmitionMod").val('Electronic');

                var selected_clearningHouse = $("#pnlBillClaimSubmission #pnlClaim_Search #ddlClearinghouse").val();
                var clearning_house = $("#pnlBillClaimSubmission #pnlClaim_Search #ddlClearinghouse option:eq(0)").val();
                if (selected_clearningHouse == clearning_house)
                    $("#pnlBillClaimSubmission #pnlClaim_Search #ddlClearinghouse").val($("#pnlBillClaimSubmission #pnlClaim_Search #ddlClearinghouse option:eq(1)").val());

                $("#pnlBillClaimSubmission #ddlSubmitionMod").prop('disabled', true);
                $("#pnlBillClaimSubmission #pnlSubmission_Result #chkAllVisits").prop('disabled', false);
                $("#pnlBillClaimSubmission #pnlSubmission_Result #chkAllVisits").prop('checked', false);
                $("#pnlBillClaimSubmission #dgvSubmission").find("input[type='checkbox']").prop('checked', false);
                $("#pnlBillClaimSubmission #dgvSubmission").find("input[type='checkbox']").prop('disabled', false);

                //search claims
                Bill_ClaimSubmission.Claim_Search();
            });

        }
        else {

            //disable clearing house required
            formValidation.enableFieldValidators('ClearingHouse', false);
            $("#pnlBillClaimSubmission #sp_required").css("display", "none");

            $("#pnlElectronicClaim_Action").fadeOut(function () {
                $("#pnlPaperClaim_Action").fadeIn();
                $("#pnlBillClaimSubmission #ddlSubmitionMod").val('Paper');
                $("#pnlBillClaimSubmission #ddlSubmitionMod").prop('disabled', false);
                $("#pnlBillClaimSubmission #ddlClearinghouse").val($("#pnlBillClaimSubmission #ddlClearinghouse option:eq(0)").val());
                // $("#pnlBillClaimSubmission #pnlSubmission_Result #chkAllVisits").prop('disabled', true);
                $("#pnlBillClaimSubmission #pnlSubmission_Result #chkAllVisits").prop('checked', false);
                $("#pnlBillClaimSubmission #dgvSubmission").find("input[type='checkbox']").prop('checked', false);

                //search claims
                Bill_ClaimSubmission.Claim_Search();
            });
        }



    },

    CheckedVisit: function (obj, event) {

        if (event != null) {
            event.stopPropagation();
        }


        if (!$(obj).prop('checked')) {
            $("#pnlBillClaimSubmission #chkAllVisits").prop('checked', false);
            $("#pnlBillClaimSubmission #chkAllVisits").attr('title', 'Select all');
            //$("#pnlBillClaimSubmission #dgvSubmission").find("input[type='checkbox']").prop('disabled', false);
        }
        else {

            var selected = [];
            $("#pnlBillClaimSubmission #dgvSubmission tbody").find("input[type='checkbox']").each(function () {

                if (!$(this).is(":checked")) {
                    selected.push(this);
                }
            });

            if (selected.length > 0) {
                $("#pnlBillClaimSubmission #chkAllVisits").prop('checked', false);
                $("#pnlBillClaimSubmission #chkAllVisits").attr('title', 'Select all');

            }
            else {
                $("#pnlBillClaimSubmission #chkAllVisits").prop('checked', true);
                $("#pnlBillClaimSubmission #chkAllVisits").attr('title', 'Unselect all');
            }
        }
    },

    Claim_Transmit: function (MarkSubmitted) {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Claim Submission", "PRINT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                var visits = Bill_ClaimSubmission.GetSelectedVisits();
                var Claims_Array = [];
                $.each($("#pnlBillClaimSubmission #dgvSubmission").find('tr'), function (i, item) {
                    if (visits.indexOf($(item).attr('visitid')) >= 0)
                    {
                        var Claim_Detail = { visitid: $(item).attr('visitid'), claimnumber: $(item).attr('claimnumber'), patientid: $(item).attr('patientid') };
                        Claims_Array.push(Claim_Detail);
                    }

                });

                if (visits.length > 0) {

                    Bill_ClaimSubmission.TransmitClaim(visits, MarkSubmitted).done(function (response) {

                        if (response.status != false) {

                            var ClaimsSubmissionJSON = JSON.parse(response.ClaimsSubmissionJSON);
                            if (ClaimsSubmissionJSON.Status == true)
                                utility.DisplayMessages(ClaimsSubmissionJSON.Message, 1);
                            else {
                                Bill_ClaimSubmission.Open_ClaimSubmissionErrorReport(response, Claims_Array);
                            }

                            $('#pnlBillClaimSubmission #chkAllVisits').prop('checked', false);
                            Bill_ClaimSubmission.Claim_Search();
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else {
                    utility.DisplayMessages("Please select claim.", 2);
                }

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

        Bill_ClaimSubmission.GetSelectedVisits();
    },

    Open_ClaimSubmissionErrorReport: function (response, Claims_Array) {

        var params = [];
        params["mode"] = "Open";
        params["ClaimsSubmissionResponse"] = response;
        params["Claims_Array"] = Claims_Array;
        LoadActionPan('Bill_ClaimSubmissionErrorReport', params);

    },

    Claim_MarkSubmitted: function () {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Claim Submission", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                var visits = Bill_ClaimSubmission.GetSelectedVisits();

                if (visits.length > 0) {
                    Bill_ClaimSubmission.PrintClaim(visits, true, true).done(function (response) {

                        if (response.status != false) {

                            utility.DisplayMessages(response.Message, 1);

                            //Search Claims
                            Bill_ClaimSubmission.Claim_Search();
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }

                    });
                }
                else {
                    utility.DisplayMessages("Please select a visit.", 2);
                }

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },


    Claim_Print: function (isSubmit, MarkSubmitted) {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Claim Submission", "PRINT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                var visits = Bill_ClaimSubmission.GetSelectedVisits();
                if (visits.length > 0) {

                    var params = [];
                    params["mode"] = "Open";
                    params["Visits"] = visits;
                    params["IsSubmit"] = isSubmit;
                    params["MarkSubmitted"] = MarkSubmitted;
                    params["ClearningHouseId"] = Bill_ClaimSubmission.ClearingHouseId;
                    LoadActionPan('Bill_ClaimHcfaForm', params);
                    $('#pnlBillClaimSubmission #chkAllVisits').prop('checked', false);
                }
                else {
                    utility.DisplayMessages("Please select a visit.", 2);
                }

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    UnLoadTab: function () {

        RemoveAdminTab();
    },

    UnLoadEdiFileViewer: function () {

        $("#pnlBillClaimSubmission #actionPanBillClaimSubmission").modal('hide');

        setTimeout(function () {
            $("#pnlBillClaimSubmission #actionPanBillClaimSubmission").find('div').first().remove();
        }, 300);

    },

    //Custom Paging Overloaded Methods

    SelectedPageClick: function (PageNo, objPage, frm, to, pagingDivId) {

        var selecter = "#pnlSubmittedBatch_Result li";
        if (pagingDivId == "divSubmissionPaging")
            selecter = "#pnlSubmission_Result li";
        else if (pagingDivId == "divClaimSubmitHistoryPaging")
            selecter = "#pnlClaimSubmitHistory_Result li";

        // Change Background Color to Black for selected page
        $(selecter).each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });

        if (pagingDivId == "divSubmissionPaging")
            Bill_ClaimSubmission.Claim_Search(PageNo, 100);
        else if (pagingDivId == "divSubmittedBatchPaging")
            Bill_ClaimSubmission.SubmitedBatch_Search(true, PageNo, 15);
        else if (pagingDivId == "divClaimSubmitHistoryPaging")
            Bill_ClaimSubmission.ClaimSubmitHistory_Search(true, PageNo, 15);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {

        var selecter = "#pnlSubmittedBatch_Result li";
        if (pagingDivId == "divSubmissionPaging")
            selecter = "#pnlSubmission_Result li";
        else if (pagingDivId == "divClaimSubmitHistoryPaging")
            selecter = "#pnlClaimSubmitHistory_Result li";

        var currentPageNo = "";
        $(selecter).each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {

            if (pagingDivId == "divSubmissionPaging")
                Bill_ClaimSubmission.Claim_Search(currentPageNo, 100);
            else if (pagingDivId == "divSubmittedBatchPaging")
                Bill_ClaimSubmission.SubmitedBatch_Search(true, currentPageNo, 15);
            else if (pagingDivId == "divClaimSubmitHistoryPaging")
                Bill_ClaimSubmission.ClaimSubmitHistory_Search(true, currentPageNo, 15);


        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {


        var selecter = "#pnlSubmittedBatch_Result li";
        if (pagingDivId == "divSubmissionPaging")
            selecter = "#pnlSubmission_Result li";
        else if (pagingDivId == "divClaimSubmitHistoryPaging")
            selecter = "#pnlClaimSubmitHistory_Result li";

        var currentPageNo = "";
        $(selecter).each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });

        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo <= TotalPages) {

            if (pagingDivId == "divSubmissionPaging")
                Bill_ClaimSubmission.Claim_Search(currentPageNo, 100);
            else if (pagingDivId == "divSubmittedBatchPaging")
                Bill_ClaimSubmission.SubmitedBatch_Search(true, currentPageNo, 15);
            else if (pagingDivId == "divClaimSubmitHistoryPaging")
                Bill_ClaimSubmission.ClaimSubmitHistory_Search(true, currentPageNo, 15);
        }
    },

    BillClaimSubmissionReset: function () {

        $('#' + Bill_ClaimSubmission.params["PanelID"] + ' #frmClaimSubmission').find('[data-plugin-datepicker]').each(function () {
            $(this).val("");
        });

        var Clearinghouseval = $('#' + Bill_ClaimSubmission.params["PanelID"] + ' #frmClaimSubmission #ddlClearinghouse option:selected').val();
        $('#' + Bill_ClaimSubmission.params["PanelID"] + ' #frmClaimSubmission .ClaimformControls_div').resetAllControls();
        //adnan maqbool, PMS-3983 , 16-02-2016
        if ($("#" + Bill_ClaimSubmission.params["PanelID"] + " #ElectronicClaim").prop("checked")) {
            $("#" + Bill_ClaimSubmission.params["PanelID"] + " #ddlSubmitionMod").val('Electronic');
            $('#' + Bill_ClaimSubmission.params["PanelID"] + ' #frmClaimSubmission #ddlClearinghouse').val(Clearinghouseval);
            $('#' + Bill_ClaimSubmission.params["PanelID"] + ' #frmClaimSubmission').bootstrapValidator('revalidateField', 'ClearingHouse');
        }
        // end

        $('#' + Bill_ClaimSubmission.params["PanelID"] + ' #frmClaimSubmission [type="text"][onblur]').each(function () {
            $(this).trigger("blur");
        });
    },

    BindClaimNumber: function () {
        var Ctrl = $("#" + Bill_ClaimSubmission.params.PanelID + " #txtClaimNumber");
        var hfCtrl = $("#" + Bill_ClaimSubmission.params.PanelID + " #hfVisitId");
        var func = function () { return Bill_ClaimSubmission.GetClaimNumberArray(Ctrl.val()); };
        utility.BindKendoAutoComplete(Ctrl, 3, "ClaimNumber", "contains", null, func, hfCtrl);
    },

    GetClaimNumberArray: function (name) {
        var AllClaimsVisits = [];
        var dfd = new $.Deferred();
        Bill_ClaimSubmission.LoadClaimNumers(name).done(function (responseData) {
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

    BindSubmittedClaimNumber: function () {
        var Ctrl = $("#" + Bill_ClaimSubmission.params.PanelID + " #txtClaimNo");
        var hfCtrl = $("#" + Bill_ClaimSubmission.params.PanelID + " #hfVisitsId");
        var func = function () { return Bill_ClaimSubmission.GetClaimNumberArray(Ctrl.val()); };
        utility.BindKendoAutoComplete(Ctrl, 3, "ClaimNumber", "contains", null, func, hfCtrl);
    },

    OpenEncounter: function (IsClaimFill) {

        Bill_ClaimSubmission.params["IsClaimFill"] = IsClaimFill;
        var params = [];
        params["FromAdmin"] = 0;
        params["ParentCtrl"] = 'billTabClaimSubmission';
        params["patientID"] = 0;
        LoadActionPan('Encounter_Visits', params);

    },

    PatientDemographics: function (patientid, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Demographic", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                var params = [];
                params["mode"] = 'Edit';
                params["PatBanner"] = true;
                params["patientID"] = patientid;
                params["IsFill"] = false;
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = "billTabClaimSubmission";
                LoadActionPan('demographicDetail', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    LoadVisitDetail: function (VisitId, PatientId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Charges", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FromAdmin"] = 0;
                params["ParentCtrl"] = 'billTabClaimSubmission';

                params["VisitId"] = VisitId;
                params["patientID"] = PatientId;

                LoadActionPan('EncounterChargeCapture', params, Bill_ClaimSubmission.params.PanelID);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    LoadClaimNumers: function (claimNumber) {
        var data = "ClaimNumber=" + claimNumber;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "BILLING_BATCHCHARGE_DETAIL", "SEARCH_VISIT_CLAIM");
    },


    FillClaimNumberFromSearch: function (ClaimNumber, AccountNumber, PatientName, PatientId, DOSFrom, VisitId) {

        if (Bill_ClaimSubmission.params["IsClaimFill"] == true) {
            $("#" + Bill_ClaimSubmission.params.PanelID + " #txtClaimNumber").val(ClaimNumber);
            //$("#" + Bill_ClaimSubmission.params.PanelID + " #dpDOSfrm").val(DOSFrom);
            $("#" + Bill_ClaimSubmission.params.PanelID + " #hfVisitId").val(VisitId);
        }
        else {
            $("#" + Bill_ClaimSubmission.params.PanelID + " #txtClaimNo").val(ClaimNumber);
            $("#" + Bill_ClaimSubmission.params.PanelID + " #hfVisitsId").val(VisitId);
        }

        Encounter_Visits.UnLoad();
    },

    ClaimSubmitHistory_Print: function (ClaimHistoryFirstload, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Claim Submission", "PRINT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                var self = $("#pnlBillClaimSubmission #frmClaimSubmitHistory");
                var myJSON = self.getMyJSONByName();

                params["JSON"] = myJSON;
                params["ParentCtrl"] = "billTabClaimSubmission";
                LoadActionPan('BillClaimSubmitHistoryPrint', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

};