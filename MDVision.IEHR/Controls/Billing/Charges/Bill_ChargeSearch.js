Bill_ChargeSearch = {
    bIsFirstLoad: true,
    params: [],
    bVisitFirst: true,
    okFunc: null,
    girdId: 'dgvBillCharge',
    PaggingId: 'divBillChargeSearch',
    cancelFunc: null,
    Load: function (params) {
        if (params.RefCtrl)
            params.FromAdmin = '0';
        else
            params.FromAdmin = '1';
        Bill_ChargeSearch.params = params;
        Bill_ChargeSearch.okFunc = null;
        Bill_ChargeSearch.cancelFunc = null;

        if (Bill_ChargeSearch.bIsFirstLoad) {
            Bill_ChargeSearch.bIsFirstLoad = false;

            var self = "";//$('#pnlBillChargeSearch');
            if (Bill_ChargeSearch.params["PanelID"] != 'pnlBillChargeSearch')
                self = $('#' + Bill_ChargeSearch.params["PanelID"] + ' #pnlBillChargeSearch');
            else
                self = $('#pnlBillChargeSearch');

            if (Bill_ChargeSearch.params["ParentCtrl"] == "Patient_Case_Detail") {
                if (Bill_ChargeSearch.params["PatientAccountNo"]) {
                    self.find("#txtPatientName").val(Bill_ChargeSearch.params["PatientAccountNo"]);
                    self.find("#txtPatientName").parent().addClass("disableAll");

                    if (Bill_ChargeSearch.params["PatientID"])
                        self.find("#hfPatientId").val(Bill_ChargeSearch.params["PatientID"]);

                    self.find("#txtPatientName").focus()
                }
            }

            Bill_ChargeSearch.LoadAllControls();

            if (Bill_ChargeSearch.params.PanelID == "pnlDocumentScan") {

                Bill_ChargeSearch.girdId = "dgvBillChargeDoc";
                Bill_ChargeSearch.PaggingId = "divBillChargeSearchDoc";
                $("#" + Bill_ChargeSearch.params["PanelID"] + " #pnlBillCharge_Result #dgvBillCharge ").attr('id', 'dgvBillChargeDoc');
                $("#" + Bill_ChargeSearch.params["PanelID"] + " #pnlBillCharge_Result #divBillChargeSearch ").attr('id', 'divBillChargeSearchDoc')

            }

            var data = "IsActive=";
            self.find('.LoadZero').loadDropDowns(true, data).done(function () {
                var dataAllUsers = "IsActive=&ID=1";
                self.find('.User').loadDropDowns(true, dataAllUsers).done(function () {
                    if (Bill_ChargeSearch.params["ParentCtrl"] == "Document_Scan") {
                        $('#' + Bill_ChargeSearch.params.PanelID + ' #frmChargeSearch #hfPatientId').val(localStorage.SelectedPatientId);
                        $('#' + Bill_ChargeSearch.params.PanelID + ' #frmChargeSearch #txtPatientName').val(localStorage.SelectedAccountNumber);
                    }
                    Bill_ChargeSearch.BillChargeSearch();
                });
            });

            if (Bill_ChargeSearch.params.ParentCtrl != 'Patient_Case_Detail' && Bill_ChargeSearch.params.ParentCtrl != 'billTabClaimSubmission' && Bill_ChargeSearch.params.ParentCtrl != 'billTabFollowUpPatientAR' && Bill_ChargeSearch.params.ParentCtrl != 'billTabFollowUpInsuranceAR' && Bill_ChargeSearch.params.ParentCtrl != 'Document_Scan' && Bill_ChargeSearch.params.ParentCtrl != 'Patient_Search' && Bill_ChargeSearch.params.ParentCtrl != 'Document_Import' && Bill_ChargeSearch.params.ParentCtrl != 'Patient_Information_Import' && Bill_ChargeSearch.params.ParentCtrl != "ERA_ChargeSearch" && Bill_ChargeSearch.params.ParentCtrl != "Bill_Insurance_Payment_Detail" && Bill_ChargeSearch.params.ParentCtrl != "Bill_Insurance_PaymentPosting_Detail") {
                Bill_ChargeSearch.removeDialogClasses();
            }
            //Begin Added by Azeem Raza Tayyab on 29-Apr-2016 for the changes related to Bug#:PMS-4685
            self.find("#chkAllVisits").change(function () {

                if ($(this).prop('checked'))
                    $(this).attr('title', 'Unselect all');
                else
                    $(this).attr('title', 'Select all');

                $("#" + Bill_ChargeSearch.params.PanelID + " #" + Bill_ChargeSearch.girdId).find("input[type='checkbox']")
                    .prop('checked', this.checked);
            });
            //End Added by Azeem Raza Tayyab on 29-Apr-2016 for the changes related to Bug#:PMS-4685
            self.loadDropDowns(true).done(function () {
                Bill_ChargeSearch.BillChargeSearch();
            });
        }

    },
    removeDialogClasses: function () {
        $('#' + Bill_ChargeSearch.params.PanelID + ' .modal-header').hide();
        $('#' + Bill_ChargeSearch.params.PanelID + ' .modal-content').removeClass('modal-content');
        $('#' + Bill_ChargeSearch.params.PanelID + ' .modal-dialog').removeAttr('class');
        $('#' + Bill_ChargeSearch.params.PanelID + ' #containerModalDialog').removeClass('modal-dialog');
    },

    ChargeCaptureAdd: function () {

        var params = [];
        params["FromAdmin"] = 0;
        params["ParentCtrl"] = 'Bill_ChargeSearch';
        params['mode'] = "Add";
        LoadActionPan('Encounter_CreateClaim', params);
    },

    BillChargeSearch: function (ChargeCapId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Charges", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#" + Bill_ChargeSearch.params["PanelID"] + " #pnlBillCharge_Result").css("display") == "none") {
                    $("#" + Bill_ChargeSearch.params["PanelID"] + " #pnlBillCharge_Result").show();
                }
                var DOSTo = $('#' + Bill_ChargeSearch.params.PanelID + ' #frmChargeSearch #dpDOSto').val();
                var DOSFrom = $('#' + Bill_ChargeSearch.params.PanelID + ' #frmChargeSearch #dpDOSfrm').val();
                if (DOSTo == "" && DOSFrom != "") {
                    utility.CreateDatePicker(Bill_ChargeSearch.params.PanelID + ' #frmChargeSearch #dpDOSto', function (ev) { }, true);
                }


                var self = "";//$('#pnlBillChargeSearch');
                if (Bill_ChargeSearch.params["PanelID"] != 'pnlBillChargeSearch')
                    self = $('#' + Bill_ChargeSearch.params["PanelID"] + ' #pnlBillChargeSearch');
                else
                    self = $('#pnlBillChargeSearch');

                var myJSON = self.getMyJSONByName();

                Bill_ChargeSearch.SearchCharge(myJSON, ChargeCapId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {

                        //-----------------Pagination------------

                        if (response.BillChargeCount > 0) {
                            $('#' + Bill_ChargeSearch.params["PanelID"] + " #" + Bill_ChargeSearch.PaggingId).css("display", "inline");
                            //Showing 1 to 15 of 15 entries
                            var RecordsPerPage = rpp != null ? rpp : 15;
                            var CurrentPage = PageNo != null ? PageNo : 1;

                            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                            if (PageNo == null) {
                                utility.GetCustomPaging(Bill_ChargeSearch.params["PanelID"] + " #" + Bill_ChargeSearch.PaggingId, response.iTotalDisplayRecords, 5, "Bill_ChargeSearch", CurrentPage, RecordsPerPage);
                            }
                            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                            $('#' + Bill_ChargeSearch.params["PanelID"] + " #" + Bill_ChargeSearch.PaggingId + " #divShowingEntries").text(showingText);
                            // Change Background Color to Black for selected page
                            //Edited by Azeem Raza Tayyab on 8-Mar-2016 to fix bug#:PMS-4375
                            $('#' + Bill_ChargeSearch.params["PanelID"] + " #pnlBillCharge_Result li").each(function () {
                                if ($(this).text() == CurrentPage) {
                                    $(this).attr("class", "active");
                                }
                                else
                                    $(this).removeAttr("class");
                            });
                        }
                        else {
                            $('#' + Bill_ChargeSearch.params["PanelID"] + " #" + Bill_ChargeSearch.PaggingId).css("display", "none");
                        }

                        //--------------------End Pagination-------------------

                        Bill_ChargeSearch.BillChargeGridLoad(response);
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

    BillChargeReset: function () {
        //Begin Edited by Azeem Raza Tayyab on 22-Apr-2016 to fix Bug#:PMS-4962
        //$('#' + Bill_ChargeSearch.params["PanelID"] + ' #frmChargeSearch').find('[data-plugin-datepicker]').each(function () { $(this).datepicker('setDate', new Date()); });
        $('#' + Bill_ChargeSearch.params["PanelID"] + ' #frmChargeSearch').resetAllControls();
        $('#' + Bill_ChargeSearch.params["PanelID"] + ' #frmChargeSearch [type="text"][onblur]').each(function () {
            $(this).trigger("blur");
        });
        $('#' + Bill_ChargeSearch.params["PanelID"] + ' #frmChargeSearch #dpDOSto').attr('disabled', true);
        $('#' + Bill_ChargeSearch.params["PanelID"] + ' #frmChargeSearch #dtpClaimCreatedTo').prop('disabled', true);
        $('#' + Bill_ChargeSearch.params["PanelID"] + ' #frmChargeSearch #dtpImportedDateTo').prop('disabled', true);
        //End Edited by Azeem Raza Tayyab on 22-Apr-2016 to fix Bug#:PMS-4962
        if (Bill_ChargeSearch.params["PanelID"] == 'pnlBillChargeSearch') {
            $("#" + Bill_ChargeSearch.params["PanelID"] + " #" + Bill_ChargeSearch.girdId).css("pointer-events", "");
        }
        /// Start PRD-91
        // reset provider link to default state
        if ($("#" + Bill_ChargeSearch.params["PanelID"] + " #frmChargeSearch #lnkProviderEdit").is(":visible"))
        { $("#" + Bill_ChargeSearch.params["PanelID"] + " #frmChargeSearch #lnkProviderEdit").hide(); }
        if (!$("#" + Bill_ChargeSearch.params["PanelID"] + " #frmChargeSearch #lblProvider").is(":visible"))
        { $("#" + Bill_ChargeSearch.params["PanelID"] + " #frmChargeSearch #lblProvider").show(); }
        // reset facility to default state
        if ($("#" + Bill_ChargeSearch.params["PanelID"] + " #frmChargeSearch #lnkFacilityEdit").is(":visible"))
        { $("#" + Bill_ChargeSearch.params["PanelID"] + " #frmChargeSearch #lnkFacilityEdit").hide(); }
        if (!$("#" + Bill_ChargeSearch.params["PanelID"] + " #frmChargeSearch #lblFacility").is(":visible"))
        { $("#" + Bill_ChargeSearch.params["PanelID"] + " #frmChargeSearch #lblFacility").show(); }
        /// END PRD-91
        if ($("#" + Bill_ChargeSearch.params["PanelID"] + " #frmChargeSearch #lnkInsurancePlanDetail").is(":visible"))
        {
            $("#" + Bill_ChargeSearch.params["PanelID"] + " #frmChargeSearch #lnkInsurancePlanDetail").hide();
            $("#" + Bill_ChargeSearch.params["PanelID"] + " #frmChargeSearch #lblInsurancePlan").show();
        }
        if ($("#" + Bill_ChargeSearch.params["PanelID"] + " #frmChargeSearch #lnkResourceProviderEdit").is(":visible")) {
            $("#" + Bill_ChargeSearch.params["PanelID"] + " #frmChargeSearch #lnkResourceProviderEdit").hide();
            $("#" + Bill_ChargeSearch.params["PanelID"] + " #frmChargeSearch #lblResourceProvider").show();
        }

    },

    BillChargeGridLoad: function (response) {
        $("#" + Bill_ChargeSearch.params["PanelID"] + " #" + Bill_ChargeSearch.girdId).dataTable().fnDestroy();
        $("#" + Bill_ChargeSearch.params["PanelID"] + " #pnlBillCharge_Result #" + Bill_ChargeSearch.girdId + " tbody").find("tr").remove();
        if (response.BillChargeCount > 0) {

            var planPriorityClass = "";
            var claimTitle = "";

            var BillChargeLoadJSONData = JSON.parse(response.BillChargeLoad_JSON);
            $.each(BillChargeLoadJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", "gvBillCharge_row" + item.VisitId);
                $row.attr("VisitId", item.VisitId);
                $row.attr("ClaimStatus", item.Status);
                if (item.SubmittedDate != "") {
                    $row.attr("Submitted", true);
                } else {
                    $row.attr("Submitted", false);
                }

                if (item.IsActive == "True") {
                    isactive = 0;
                    activeTitle = "Payment Posting";
                    tglclass = "fa fa-dollar green";
                }
                else {
                    isactive = 1;
                    activeTitle = "Payment Posting";
                    tglclass = "fa fa-dollar green";
                }
                /********************/
                planPriorityClass = "bg-info";
                if (item.IsPrimary == "False") {
                    
                    claimTitle = "Non Primary Claim";
                }

                else {
                 //   planPriorityClass = "";
                    claimTitle = "Primary Claim";
                }
                $row.addClass(planPriorityClass);
                var attachmentIcon = "";
                if (item.IsDocAttach == 1) {
                    attachmentIcon = " <i class='fa fa-paperclip' title='Document(s) Attached'></i>"
                }
                /*******************/
                var lockIcon = "";
                if (item.NotesId) {
                    var ShowNotes = "Bill_ChargeSearch.LoadNotesTab('" + item.NotesId.trim() + "','" + item.PatientId.trim() + "','" + item.ResourceProviderId.trim() + "',event);";
                    if (item.NoteStatus.toLowerCase() == 'signed')
                        lockIcon = '&nbsp;<a class="btn  btn-xs" href="#" onclick="' + ShowNotes + '" title="Signed Note"><i class="glyphicon glyphicon-lock black"></i></a>';
                }
                var selectBillCharge = "";
                if (Bill_ChargeSearch.params["FromAdmin"] == "0") {
                    var selectMethod = "Bill_ChargeSearch.FillBillCharge('" + item.ChargeCapId + "','" + item.PatientName + "');"
                    selectBillCharge = '&nbsp;<a class="btn  btn-xs" href="#" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check"></i></a>' + lockIcon;
                    $row.attr("onclick", selectMethod);
                }
                else if (Bill_ChargeSearch.params["ParentCtrl"] == "Patient_Case_Detail") {
                    var selectMethod = "Patient_Case_Detail.FillVisitFromSearch('" + item.PatientId + "' , '" + item.VisitId + "',event );"
                    var selectBillCharge = '&nbsp;<a class="btn  btn-xs" href="#" onclick="' + selectMethod + '" title="Select Record"><i class="fa fa-check black"></i></a>' + lockIcon;
                    $row.attr("onclick", selectMethod);
                }
                else if (Bill_ChargeSearch.params["ParentCtrl"] == "billTabFollowUpPatientAR") {
                    var selectMethod = "utility.FillVisitFromSearch('" + item.ClaimNumber + "' , '#frmFollowUpPatientAR #txtClaimno',event );"
                    var selectBillCharge = '&nbsp;<a class="btn  btn-xs" href="#" onclick="' + selectMethod + '" title="Select Record"><i class="fa fa-check black"></i></a>' + lockIcon;
                    $row.attr("onclick", selectMethod);
                }
                else if (Bill_ChargeSearch.params["ParentCtrl"] == "billTabFollowUpInsuranceAR") {
                    var selectMethod = "utility.FillVisitFromSearch('" + item.ClaimNumber + "' , '#frmFollowUpInsuranceAR #txtClaimno',event );"
                    var selectBillCharge = '&nbsp;<a class="btn  btn-xs" href="#" onclick="' + selectMethod + '" title="Select Record"><i class="fa fa-check black"></i></a>' + lockIcon;
                    $row.attr("onclick", selectMethod);
                }
                else if (Bill_ChargeSearch.params["ParentCtrl"] == "Patient_Search") {
                    var selectMethod = "utility.FillVisitFromSearch('" + item.ClaimNumber + "' , '#frmPatientSearch #txtClaimno',event );"
                    var selectBillCharge = '&nbsp;<a class="btn  btn-xs" href="#" onclick="' + selectMethod + '" title="Select Record"><i class="fa fa-check black"></i></a>' + lockIcon;
                    $row.attr("onclick", selectMethod);
                }
                else if (Bill_ChargeSearch.params["ParentCtrl"] == "ERA_ChargeSearch") {
                    var selectMethod = "utility.FillVisitFromSearch('" + item.ClaimNumber + "' , '#frmERAChargeSearch #txtClaimNumber',event );"
                    var selectBillCharge = '&nbsp;<a class="btn  btn-xs" href="#" onclick="' + selectMethod + '" title="Select Record"><i class="fa fa-check black"></i></a>' + lockIcon;
                    $row.attr("onclick", selectMethod);
                }
                else if (Bill_ChargeSearch.params["ParentCtrl"] == "Document_Import") {
                    var selectMethod = "utility.FillVisitFromSearch('" + item.ClaimNumber + "' , '#frmDocumentImport #txtClaimNumber','" + item.VisitId + "','#frmDocumentImport #hfVisitId',event );"
                    var selectBillCharge = '&nbsp;<a class="btn  btn-xs" href="#" onclick="' + selectMethod + '" title="Select Record"><i class="fa fa-check black"></i></a>' + lockIcon;
                    $row.attr("onclick", selectMethod);
                }
                else if (Bill_ChargeSearch.params["ParentCtrl"] == "Patient_Information_Import") {
                    var selectMethod = "utility.FillVisitFromSearch('" + item.ClaimNumber + "' , '#frmDocumentImport #txtClaimNumber','" + item.VisitId + "','#frmDocumentImport #hfVisitId',event );"
                    var selectBillCharge = '&nbsp;<a class="btn  btn-xs" href="#" onclick="' + selectMethod + '" title="Select Record"><i class="fa fa-check black"></i></a>' + lockIcon;
                    $row.attr("onclick", selectMethod);
                }
                else if (Bill_ChargeSearch.params["ParentCtrl"] == "Document_Scan") {

                    // var selectMethod = "utility.FillVisitFromSearch('" + item.ClaimNumber + "' , '#frmDocumentScan #ddlClaim',event );"
                    var selectMethod = "Document_Scan.FillVisitFromSearch('" + item.AccountNumber + "' ,'" + item.PatientId + "' ,'" + item.ClaimNumber + "' , '" + item.VisitId + "',event );"
                    var selectBillCharge = '&nbsp;<a class="btn  btn-xs" href="#" onclick="' + selectMethod + '" title="Select Record"><i class="fa fa-check black"></i></a>' + lockIcon;
                    $row.attr("onclick", selectMethod);
                }
                else if (Bill_ChargeSearch.params["ParentCtrl"] == "billTabClaimSubmission") {
                    if (Bill_ChargeSearch.params["Field1"] == true) {
                        var selectMethod = "utility.FillVisitFromSearch('" + item.ClaimNumber + "' , '#frmClaimSubmission #txtClaimNumber','" + item.VisitId + "',null ,event );"
                    } else {
                        var selectMethod = "utility.FillVisitFromSearch('" + item.ClaimNumber + "' , '#frmClaimSubmitHistory #txtClaimNo',event );"
                    }
                    var selectBillCharge = '&nbsp;<a class="btn  btn-xs" href="#" onclick="' + selectMethod + '" title="Select Record"><i class="fa fa-check black"></i></a>' + lockIcon;
                    $row.attr("onclick", selectMethod);
                }
                else if (Bill_ChargeSearch.params["ParentCtrl"] == "Bill_Insurance_Payment_Detail" || Bill_ChargeSearch.params["ParentCtrl"] == "Bill_Insurance_PaymentPosting_Detail") {
                    var selectMethod = Bill_ChargeSearch.params["ParentCtrl"] +".FillVisitFromSearch('" + item.ClaimNumber + "'," + item.VisitId + ",'" + item.PatientName + "'," + item.PatientId + ",'" + utility.RemoveTimeFromDate(null, item.DOSFrom) + "',event );"
                    var selectBillCharge = '&nbsp;<a class="btn  btn-xs" href="#" onclick="' + selectMethod + '" title="Select Record"><i class="fa fa-check black"></i></a>' + lockIcon;
                    $row.attr("onclick", selectMethod);
                }
                
                else {
                    $row.attr("onclick", "utility.SelectGridRow($('#gvBillCharge_row" + item.VisitId + "'));Bill_ChargeSearch.LoadVisitDetail('" + item.VisitId.trim() + "','" + item.PatientId.trim() + "',event);");
                }

                if (selectBillCharge == "")
                    selectBillCharge = lockIcon;
                var PaymentPostingAction = "";
                var ShowPaymentPosting = "Bill_ChargeSearch.LoadPaymentPosting(null,'" + item.VisitId.trim() + "','" + item.IsERAttach.trim() + "','" + item.PatientId.trim() + "',event);";

                var actions = "";
                if (Bill_ChargeSearch.params["ParentCtrl"] == "Patient_Case_Detail") {
                    actions = selectBillCharge;
                }
                else {
                    //Modified by Azeem Raza Tayyab on 11-Apr-2016 to Fix bug#:PMS-4834 added new perameter(,\'ChargeSearch\') to handel deletion. and Item.Status for customized delet alret msg for submitted claim
                    PaymentPostingAction = '<a class="btn  btn-xs" href="#" onclick="' + ShowPaymentPosting + '" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>';
                    //if (parseInt(item.ChargeAmount) && item.IsLocked.toLowerCase() == 'true' && parseInt(item.ChargeAmount) < 0) {
                    //    PaymentPostingAction = '';
                    //}
                    actions = '<a class="btn  btn-xs" href="#" onclick="Encounter_Visits.VisitDelete(\'' + item.VisitId + '\',event,\'ChargeSearch\',\'' + item.Status + '\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Bill_ChargeSearch.LoadVisitDetail(\'' + item.VisitId.trim() + '\',\'' + item.PatientId.trim() + '\',event);" title="Visit Detail"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Bill_ChargeSearch.LoadClaimSummary(\'' + item.VisitId.trim() + '\',\'' + item.PatientId.trim() + '\',event);" title="Claim Summary"><i class="fa fa-file-text" aria-hidden="true"></i></a>' + PaymentPostingAction + selectBillCharge +attachmentIcon;
                }
                //Edited by Azeem Raza Tayyab on 29-Apr-2016 for the changes related to Bug#:PMS-4685
                if (item.SubmitStatus.toLowerCase() == "patient") {
                    item.InsurancePlanName = "Patient";
                }

                var patcharge = Number(item.PatCharges) + Number(item.Copay);
                //  $row.append('<td style="display:none;">' + item.VisitId + '</td><td><input type="checkbox" onclick="Bill_ChargeSearch.CheckedVisit(this,event)" name="checkbox" id="' + item.VisitId + '"></td><td>' + actions + '</td> <td><a href="#" onclick="Bill_ChargeSearch.PatientDemographics(' + item.PatientId + ',event);"  title="View Patient">' + item.AccountNumber + '</a></td> <td><a href="#" onclick="Bill_ChargeSearch.PatientDemographics(' + item.PatientId + ',event);"  title="View Patient">' + item.PatientName + '</a></td> <td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td><td title="' + claimTitle + '">' + item.ClaimNumber + '</td><td>' + item.InsurancePlanName + '</td><td>' + utility.convertToFigure(item.TotalClaimFee, true) + '</td><td>' + utility.convertToFigure(item.ExpectedFee, true) + '</td> <td>' + utility.convertToFigure(item.InsCharges, true) + '</td> <td>' + utility.convertToFigure(item.InsBalance, true) + '</td> <td>' + utility.convertToFigure(item.PatCharges, true) + '</td><td>' + utility.convertToFigure(item.Copay, true) + '</td><td>' + utility.convertToFigure(item.PatBalance, true) + '</td><td class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.FacilityName + '">' + item.FacilityName + '</td><td data-toggle="tooltip" data-placement="right" title="' + item.ProviderName + '">' + item.ProviderName + '</td><td data-toggle="tooltip" data-placement="right" title="' + item.ResourceProviderName + '">' + item.ResourceProviderName + '</td><td>' + item.Status + '</td><td>' + item.SubmitStatus + '</td><td>' + item.EnteredBy + '</td><td>' + item.ClaimCreatedDate + '</td><td>' + item.EncounterSignOffDate + '</td><td>' + item.ImportedOn + '</td><td>' + item.SubmittedDate + '</td>');
                $row.append('<td style="display:none;">' + item.VisitId + '</td><td><input type="checkbox" onclick="Bill_ChargeSearch.CheckedVisit(this,event)" name="checkbox" id="' + item.VisitId + '"></td><td>' + actions + '</td> <td><a href="#" onclick="Bill_ChargeSearch.PatientDemographics(' + item.PatientId + ',event);"  title="View Patient">' + item.AccountNumber + '</a></td> <td><a href="#" onclick="Bill_ChargeSearch.PatientDemographics(' + item.PatientId + ',event);"  title="View Patient">' + item.PatientName + '</a></td> <td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td><td title="' + claimTitle + '">' + item.ClaimNumber + '</td><td>' + item.InsurancePlanName + '</td><td class="text-right">' + utility.convertToFigure(item.TotalClaimFee, true) + '</td><td  class="text-right">' + utility.convertToFigure(item.ExpectedFee, true) + '</td> <td  class="text-right">' + utility.convertToFigure(item.InsCharges, true) + '</td> <td  class="text-right">' + utility.convertToFigure(item.InsBalance, true) + '</td> <td  class="text-right">' + utility.convertToFigure(patcharge, true) + '</td><td  class="text-right">' + utility.convertToFigure(item.PatBalance, true) + '</td><td class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.FacilityName + '">' + item.FacilityName + '</td><td data-toggle="tooltip" data-placement="right" title="' + item.ProviderName + '">' + item.ProviderName + '</td><td data-toggle="tooltip" data-placement="right" title="' + item.ResourceProviderName + '">' + item.ResourceProviderName + '</td><td>' + item.Status + '</td><td>' + item.SubmitStatus + '</td><td>' + item.EnteredBy + '</td><td>' + item.ClaimCreatedDate + '</td><td>' + item.EncounterSignOffDate + '</td><td>' + item.ImportedOn + '</td><td>' + item.SubmittedDate + '</td><td>' + item.EncounterTypeName + '</td>');

                if (item.HasError.toLowerCase() == "true") {
                    $($row).removeClass("active").removeClass("bg-info");
                    $($row).css("background", "#FF6A6A");
                }

                $("#" + Bill_ChargeSearch.params["PanelID"] + " #pnlBillCharge_Result #" + Bill_ChargeSearch.girdId + " tbody").last().append($row);
            });
        }
        else {
            $('#' + Bill_ChargeSearch.params["PanelID"] + " #" + Bill_ChargeSearch.PaggingId).css("display", "none");
            $("#" + Bill_ChargeSearch.params["PanelID"] + " #" + Bill_ChargeSearch.girdId).DataTable({
                "language": {
                    "emptyTable": "No Charge Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable("#" + Bill_ChargeSearch.params["PanelID"] + " #" + Bill_ChargeSearch.girdId))
            ;
        else
            $("#" + Bill_ChargeSearch.params["PanelID"] + " #pnlBillCharge_Result #" + Bill_ChargeSearch.girdId).DataTable({ "bInfo": false, "scrollY": 300, "scrollX": true, "bPaginate": false, "bLengthChange": false, "order": [[0, "desc"]], "autoWidth": false, "bFilter": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        $($("#" + Bill_ChargeSearch.params["PanelID"] + " #pnlBillCharge_Result #" + Bill_ChargeSearch.girdId).parent()).css({ "height": "initial", "max-height": "300px" });
        $("#pnlBillCharge_Result div.table-responsive").css("overflow", "initial");
    },

    FillBillCharge: function (ChargeCapId, PatientName) {

        var RefCtrl = "txtChargeBatchNumber";
        var RefHiddenIdCtrl = "hfChargeBatchNumber";

        if (Bill_ChargeSearch.params.RefCtrl != null)
            RefCtrl = Bill_ChargeSearch.params.RefCtrl;

        if (Bill_ChargeSearch.params.RefHiddenIdCtrl != null)
            RefHiddenIdCtrl = Bill_ChargeSearch.params.RefHiddenIdCtrl;

        $('#' + Bill_ChargeSearch.params.PanelID + ' #' + RefCtrl).val(PatientName);
        $('#' + Bill_ChargeSearch.params.PanelID + ' #' + RefHiddenIdCtrl).val(ChargeCapId);

        UnloadActionPan(Bill_ChargeSearch.params["ParentCtrl"], "Bill_ChargeSearch");

    },

    LoadChargeDetail: function (ChargeCapId, event) {

        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Charges", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ChargeCapId"] = ChargeCapId;
                params["mode"] = "Edit";
                LoadActionPan('chargeSearchDetail', params);
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
                if (Bill_ChargeSearch.params["PanelID"] == 'pnlBillChargeSearch') {
                    $("#" + Bill_ChargeSearch.params["PanelID"] + " #" + Bill_ChargeSearch.girdId).css("pointer-events", "none");
                }
                var params = [];
                params["FromAdmin"] = 0;
                params["ParentCtrl"] = 'Bill_ChargeSearch';

                params["VisitId"] = VisitId;
                params["patientID"] = PatientId;

                LoadActionPan('EncounterChargeCapture', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    LoadPaymentPosting: function (ChargeCapId, VisitId,IsERAttach,patientId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Payment Posting", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FromAdmin"] = 0;
                params["ParentCtrl"] = 'Bill_ChargeSearch';
                params["VisitId"] = VisitId;
                params["ChargeId"] = ChargeCapId;
                params["PaymentRef"] = "pnlBillChargeSearch";
                params["IsERAAttached"] = IsERAttach;
                params["patientID"] = patientId;
                
                LoadActionPan('Bill_PaymentPosting', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    LoadNotesTab: function (notesId, patientId, providerId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Notes_Notes", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (notesId != null && notesId != "") {
                    var params = [];
                    params["FromAdmin"] = "0";
                    params["NotesId"] = notesId;
                    params["PatientId"] = patientId;
                    params["RefSearch"] = "DraftSearch";
                    params["ProviderId"] = providerId;
                    params["ParentCtrl"] = "Bill_ChargeSearch";
                    LoadActionPan('Clinical_NotesView', params);
                } else {
                    utility.DisplayMessages("There is no clinical note attached", 3);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    SearchCharge: function (BillChargeData, ChargeCapId, PageNumber, RowsPerPage) {

        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 15 : RowsPerPage;

        var objData = JSON.parse(BillChargeData);

        objData["ChargeCapId"] = ChargeCapId;
        objData["PageNumber"] = PageNumber;
        objData["CommandType"] = "Search";
        objData["CreatedBy_Text"] = $('#ddlCreatedBy option:selected').text();
        objData["Paid_Text"] = $('#ddlPaid option:selected').text();
        objData["EncounterTypeId"] = $('#pnlBillCharge_Search #ddlEncounterType option:selected').val();
        objData["RowsPerPage"] = RowsPerPage;

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Charges", "Charge");

    },

    LoadAllControls: function () {

        CacheManager.BindCodes('GetInsurancePlan', false).done(function (result) {
            var Ctrl = $("#" + Bill_ChargeSearch.params["PanelID"] + " input#txtInsurancePlan");
            var hfCtrl = $("#" + Bill_ChargeSearch.params["PanelID"] + " #hfInsurancePlan");
            var onSelect = function (e) { $("#" + Bill_ChargeSearch.params["PanelID"] + " #hfInsurancePlansearchPattern").val(e.searchPattern); };
            var onChange = function (valid) { Patient_Insurance.ValidateSearchPatternForSelectedInsurancePlan(); };
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", InsurancePlans, null, hfCtrl, onSelect, onChange);
        });

        CacheManager.BindCodes('GetFacility', false).done(function (result) {
            var Ctrl = $("#frmChargeSearch #txtFacility");
            var hfCtrl = $("#" + Bill_ChargeSearch.params.PanelID + " #hfFacility");
            var onSelect = function (e) {
                $("#" + Bill_ChargeSearch.params.PanelID + " #txtPractice").val(e.Practice);
                $("#" + Bill_ChargeSearch.params.PanelID + " #hfPractice").val(e.PracticeId);
            };
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl, onSelect);
        });
        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            var Ctrl = $("#frmChargeSearch #txtProvider");
            var hfCtrl = $("#" + Bill_ChargeSearch.params.PanelID + " #hfProvider");
            var onSelect = function (e) {
                $("#" + Bill_ChargeSearch.params.PanelID + " #txtProvider").attr("ProviderId", e.id);
                $('#' + Bill_ChargeSearch.params.PanelID + ' #frmChargeSearch #hfProvider').val(e.id);
            };
            
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, onSelect);
        });
        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            var Ctrl = $("#frmChargeSearch #txtResourceProvider");
            var hfCtrl = $("#" + Bill_ChargeSearch.params.PanelID + " #hfResourceProvider");
            var onSelect = function (e) { $("#" + Bill_ChargeSearch.params.PanelID + " #txtResourceProvider").attr("ProviderId", e.id) };
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, onSelect);
        });
        //utility.CreateDatePicker(Bill_ChargeSearch.params.PanelID + ' #frmChargeSearch #dpDOSfrm,#dpDOSto', function (ev) { }, false);
        utility.ValidateFromToDate('frmChargeSearch', 'dpDOSfrm', 'dpDOSto', true);
        //utility.CreateDatePicker(Bill_ChargeSearch.params.PanelID + ' #frmChargeSearch #dtpClaimCreatedFrom', function (ev) { });
        utility.ValidateFromToDate('frmChargeSearch', 'dtpClaimCreatedFrom', 'dtpClaimCreatedTo', true);
        utility.ValidateFromToDate('frmChargeSearch', 'dtpImportedDateFrom', 'dtpImportedDateTo', true);
        utility.ValidateFromToDate('frmChargeSearch', 'dtpSubmittedDateFrom', 'dtpSubmittedDateTo', true);
        utility.CreateDatePicker(Bill_ChargeSearch.params.PanelID + ' #frmChargeSearch #dtpEncounterSignOffDate', function (ev) { });
        CacheManager.BindDropDownsByID('#' + Bill_ChargeSearch.params.PanelID + ' #frmChargeSearch #ddlSubmitStatus', 'GetAllSubmitStatus', false, null, null, false);
        utility.AddDaysFromToDate('frmChargeSearch', 'dpDOSfrm', 'dpDOSto', 0, 0);
        //Added by Azeem Raza Tayyab on 29-Apr-2016 for the changes related to Bug#:PMS-4685
        CacheManager.BindDropDownsByID('#' + Bill_ChargeSearch.params.PanelID + ' #frmChargeSearch #ddlSubmitStatusPopUp', 'GetSubmitStatus', false, null, null, false);
        Bill_ChargeSearch.BindPatientAccount();
    },

    ValidateSearchCriteria: function () {

        utility.ValidateSearchCriteria(Bill_ChargeSearch.params.PanelID + " #frmChargeSearch", function () {
            Bill_ChargeSearch.BillChargeSearch();
            if (Bill_ChargeSearch.params["PanelID"] == 'pnlBillChargeSearch') {
                $("#" + Bill_ChargeSearch.params["PanelID"] + " #" + Bill_ChargeSearch.girdId).css("pointer-events", "");
            }
        });
    },

    Scrub_Claims: function () {
        AppPrivileges.GetFormPrivileges("Encounter", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var SelectedVisits = [];
                var visits = Bill_ChargeSearch.GetSelectedVisits();

                // fixation of PMS-1063
                $.each(visits, function (i, item) {
                    SelectedVisits.push(item.VisitId);
                });

                var ConfirmDiv = $("#" + Bill_ChargeSearch.params.PanelID + " #divSubmitStatus");
                if (SelectedVisits.length > 0) {

                    Bill_ClaimSubmission.ScrubClaims(SelectedVisits).done(function (response) {

                        if (response.status != false) {

                            if (response.ErrorsCount > 0)
                                utility.DisplayMessages(response.Message, 4);
                            else
                                utility.DisplayMessages(response.Message, 1);

                            var ScrubClaimJSONData = JSON.parse(response.ScrubClaim_JSON);
                            $.each(ScrubClaimJSONData.Claims, function (i, item) {

                                var $tr = $("#" + Bill_ChargeSearch.params.PanelID + " #" + Bill_ChargeSearch.girdId + " tbody tr[visitid='" + item.VisitId + "']");
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
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    LoadDefaultData: function () {
        $("#" + Bill_ChargeSearch.params["PanelID"] + ' #txtProvider').val(globalAppdata['DefaultProviderName']);
        $("#" + Bill_ChargeSearch.params["PanelID"] + ' #hfProvider').val(globalAppdata['DefaultProviderId']);
        $("#" + Bill_ChargeSearch.params["PanelID"] + ' #lnkProviderEdit').css("display", "inline");
        $("#" + Bill_ChargeSearch.params["PanelID"] + ' #lblProvider').css("display", "none");
        $("#" + Bill_ChargeSearch.params["PanelID"] + ' #txtFacility').val(globalAppdata['DefaultFacilityName']);
        $("#" + Bill_ChargeSearch.params["PanelID"] + ' #hfFacility').val(globalAppdata['DefaultFacilityId']);
        $("#" + Bill_ChargeSearch.params["PanelID"] + ' #lnkFacilityEdit').css("display", "inline");
        $("#" + Bill_ChargeSearch.params["PanelID"] + ' #lblFacility').css("display", "none");
        utility.CreateDatePicker(Bill_ChargeSearch.params.PanelID + ' #frmChargeSearch #dpDOSfrm,#dpDOSto,#dtpEncounterSignOffDate,#dtpImportedDateFrom,#dtpImportedDateTo,#dtpSubmittedDateFrom,#dtpSubmittedDateTo', function (ev) { }, true);
    },
    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmChargeSearch";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Bill_ChargeSearch";
        LoadActionPan('Admin_Facility', params);
    },
    OpenFacilityDetail: function () {
        var params = [];
        params["FacilityId"] = $('#' + Bill_ChargeSearch.params.PanelID + ' #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Bill_ChargeSearch';
        params["RefCtrl"] = "txtFacility";
        LoadActionPan('facilityDetail', params);
    },
    OpenProvider: function (Title, RefCtrl, RefCtrlHidden, RefCtrlLabel, RefCtrlLink) {
        //var params = [];
        //params["IsOptional"] = true;
        //params["RefForm"] = "frmChargeSearch";
        //params["ProviderId"] = "-1";
        //params["FromAdmin"] = "0";
        //params["ParentCtrl"] = "Bill_ChargeSearch";
        //LoadActionPan('Admin_Provider', params);
        var params = [];
        //adnan maqbool, PMS-4047 , 18-02-2016
        //var PanelID = EncounterChargeCapture.params["PanelID"];
        //end
        if (RefCtrl == 'txtProvider')
            params["IsOptional"] = false;
        else
            params["IsOptional"] = true;
        params["RefForm"] = 'Bill_ChargeSearch';
        params["Title"] = Title;
        params["RefCtrl"] = RefCtrl;
        params["RefCtrlHidden"] = RefCtrlHidden;
        params["RefCtrlLabel"] = RefCtrlLabel;
        params["RefCtrlLink"] = RefCtrlLink;
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Bill_ChargeSearch';

        LoadActionPan('Admin_Provider', params);
    },
    OpenProviderDetail: function (HiddenCtrl, TxtBoxCtrl) {
        var params = [];
        params["ProviderId"] = $('#' + Bill_ChargeSearch.params["PanelID"] + ' #' + HiddenCtrl).val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = TxtBoxCtrl;
        params["ParentCtrl"] = 'Bill_ChargeSearch';
        LoadActionPan('providerDetail', params);
    },
    ChargeDelete: function (chargeID, event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.myConfirm('1', function () {
            var selectedValue = chargeID;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                AppPrivileges.GetFormPrivileges("Charges", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        Bill_ChargeSearch.DeleteCharge(chargeID).done(function (response) {
                            if (response.status == true) {
                                Bill_ChargeSearch.BillChargeSearch();
                                utility.DisplayMessages(response.Message, 1);
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
        }, function () { });

    },
    DeleteCharge: function (chargeID) {


        var objData = new JSON.constructor();
        objData["ChargeCapId"] = chargeID;
        objData["CommandType"] = "delete_bill_charge";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Charges", "Charge");
    },
    BindClaimNumber: function () {
        var ClaimNumber = $('#' + Bill_ChargeSearch.params.PanelID + ' #txtClaimno').val();
        var AllClaimsVisits = [];
        if (ClaimNumber.length > 2) {
            utility.Keyupdelay(function () {
                Bill_ChargeSearch.LoadClaimNumers(ClaimNumber).done(function (responseData) {
                    if (responseData.status != false) {
                        if (responseData.ClaimsCount > 0) {
                            var Claims = JSON.parse(responseData.ClaimsLoad_JSON);

                            $.each(Claims, function (i, item) {
                                //AllClaimsVisits.push({ id: item.VisitId, value: item.ClaimNumber + ' - ( ' + item.AccountNumber + ' - ' + item.PatientName + ' )', PatientId: item.PatientId, DOSFrom: item.DOSFrom, PatientName: item.AccountNumber + ' - ' + item.PatientName, ClaimNumber: item.ClaimNumber });
                                AllClaimsVisits.push({ id: item.VisitId, value: item.ClaimNumber, PatientId: item.PatientId, DOSFrom: item.DOSFrom, PatientName: item.AccountNumber + ' - ' + item.PatientName, ClaimNumber: item.ClaimNumber });
                            });
                            //response(AllClaimsVisits);


                            $("#" + Bill_ChargeSearch.params.PanelID + " #txtClaimno").autocomplete({
                                autoFocus: true,
                                source: AllClaimsVisits,
                                select: function (event, ui) {
                                    setTimeout(function () {
                                        $("#" + Bill_ChargeSearch.params.PanelID + " #txtClaimno").val(ui.item.ClaimNumber);
                                    }, 100);

                                    //$("#hfpatientid").val(ui.item.id);
                                }
                            });
                        }
                    }
                    $("#" + Bill_ChargeSearch.params.PanelID + " #txtClaimno").autocomplete("search");
                });
            });

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
        params["ParentCtrl"] = 'Bill_ChargeSearch';

        params["patientID"] = 0;

        LoadActionPan('Encounter_Visits', params);

        //if (Bill_ChargeSearch.bVisitFirst) {
        //    $($('body #OpenVisits')[0]).attr('id', 'OpenVisits1')
        //    $($('body #CloseVisits')[0]).attr('id', 'CloseVisits1');
        //    Bill_ChargeSearch.bVisitFirst = false;
        //}

    },
    FillClaimNumberFromSearch: function (ClaimNumber, AccountNumber, PatientName, PatientId, DOSFrom, VisitId) {
        $("#" + Bill_ChargeSearch.params.PanelID + " #txtClaimno").val(ClaimNumber);
        //UnloadActionPan("Bill_ChargeSearch");
        Bill_ChargeSearch.BindClaimNumber();
        Encounter_Visits.UnLoad();
    },
    OpenPatientSearch: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Bill_ChargeSearch';
        LoadActionPan('Patient_Search', params);
    },
    FillPatientInfoFromSearch: function (PatientId, AccountNumber, patFullName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        if ($("#" + Bill_ChargeSearch.params.PanelID + " #frmChargeSearch #txtPatientName").data('kendoAutoComplete'))
            utility.SetKendoAutoCompleteSourceforValidate($("#" + Bill_ChargeSearch.params.PanelID + " #txtPatientName"), AccountNumber, $("#" + Bill_ChargeSearch.params.PanelID + " #hfPatientId"), PatientId, "AccountNumber");
        UnloadActionPan("Bill_ChargeSearch");
        utility.InsertRecentPatient(PatientId);
    },
    BindPatientAccount: function () {
        var Ctrl = $("#" + Bill_ChargeSearch.params.PanelID + " #txtPatientName");
        var func = function () { return utility.GetPatientArray(Ctrl.val(), 0) };
        var hfCtrl = $("#" + Bill_ChargeSearch.params.PanelID + " #hfPatientId");
        var onSelect = function (e) { utility.InsertRecentPatient(e.id); };
        utility.BindKendoAutoComplete(Ctrl, 4, "AccountNumber", "contains", null, func, hfCtrl, onSelect);
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
                params["ParentCtrl"] = "Bill_ChargeSearch";
                LoadActionPan('demographicDetail', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    UnLoadTab: function (Tab) {

        RemoveAdminTab(Tab);

    },

    UnLoad: function () {
        Bill_ChargeSearch.girdId = 'dgvBillCharge'
        Bill_ChargeSearch.PaggingId = 'divBillChargeSearch';
        if (Bill_ChargeSearch.params != null && Bill_ChargeSearch.params.ParentCtrl != null) {
            UnloadActionPan(Bill_ChargeSearch.params.ParentCtrl, 'Bill_ChargeSearch');
        }
        else
            UnloadActionPan('Bill_ChargeSearch');

    },


    /***insurance plan autocomplete thing START****/

    OpenInsurancePlanDetail: function () {
        //Admin_InsurancePlan.InsurancePlanEdit($("#pnlPatientInsurance #hfInsurancePlan").val());
        var params = [];
        var PanelID = null;
        if (Bill_ChargeSearch.params["PanelID"] != "pnlPatientInsurance")
            PanelID = Bill_ChargeSearch.params["PanelID"];
        //if (Bill_ChargeSearch.params.ParentCtrl == "demographicDetail")
        //    params["ParentCtrl"] = 'Bill_ChargeSearch';
        //else
        params["ParentCtrl"] = 'Bill_ChargeSearch';
        params["InsurancePlanId"] = $("#pnlBillChargeSearch #hfInsurancePlan").val();
        params["mode"] = "Edit";
        Admin_InsurancePlan.params["FromAdmin"] == "0";
        //params["ParentCtrl"] = 'patTabInsurance';
        LoadActionPan('insurancePlanDetail', params, PanelID);
    },

    OpenInsurancePlan: function (PlanProvider) {
        var params = [];
        var PanelID = null;
        if (Bill_ChargeSearch.params["PanelID"] != "pnlBillChargeSearch")
            PanelID = Bill_ChargeSearch.params["PanelID"];
        //if (Bill_ChargeSearch.params.ParentCtrl == "demographicDetail")
        //    params["ParentCtrl"] = 'Bill_ChargeSearch';
        //else
        params["ParentCtrl"] = 'Bill_ChargeSearch';
        params["InsurancePlanId"] = "-1";
        params["FromAdmin"] = "0";
        if (PlanProvider != null)
            params["RefCtrl"] = PlanProvider;
        LoadActionPan('Admin_InsurancePlan', params, PanelID);
    },

    FillInsurancePlanName: function (InsurancePlanId, InsurancePlanName) {
        $("#pnlBillChargeSearch #txtInsurancePlan").val(InsurancePlanName);
        $("#pnlBillChargeSearch #hfInsurancePlan").val(InsurancePlanId);
        //$("#pnlBillChargeSearch #hfInsurancePlansearchPattern").val(SearchPattern);
        $("#pnlBillChargeSearch #lnkInsurancePlanDetail").css("display", "inline");
        $("#pnlBillChargeSearch #lblInsurancePlan").css("display", "none");
        UnloadActionPan(Admin_InsurancePlan.params["ParentCtrl"]);

        //if ($('#pnlPatientInsurance #frmPatientInsurance').data("bootstrapValidator") != null) {
        //    $('#pnlPatientInsurance #frmPatientInsurance').bootstrapValidator('revalidateField', 'InsurancePlan');
        //}
    },

    /***insurance plan autocomplete thing END****/

    //-------Pagination Functions----------
    SelectedPageClick: function (PageNo, objPage) {
        // Change Background Color to Black for selected page
        $("#pnlBillCharge_Result li").each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });
        Bill_ChargeSearch.BillChargeSearch(0, PageNo, 15);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#pnlBillCharge_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            Bill_ChargeSearch.BillChargeSearch(0, currentPageNo, 15);

        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {

        var currentPageNo = "";
        $("#pnlBillCharge_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });

        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo <= TotalPages) {
            Bill_ChargeSearch.BillChargeSearch(0, currentPageNo, 15);
        }
    },

    //OpenPatientSearchForClaim: function () {
    //    if (!Patient_Demographic.params.patientID) {
    //        var params = [];
    //        params["FromAdmin"] = "0";
    //        params["ForNewClaim"] = "1";
    //        params["ParentCtrl"] = 'Bill_ChargeSearch';

    //        LoadActionPan('Patient_Search', params);
    //    }
    //    else {
    //        var params = [];
    //        params["FromAdmin"] = 0;
    //        params["ParentCtrl"] = 'Bill_ChargeSearch';
    //        params['mode'] = "Add";
    //        params["PatientId"] = Patient_Demographic.params.patientID;
    //        params["patFullName"] = Patient_Demographic.params.patFullName;
    //        params["RefProviderId"] = Patient_Demographic.params.RefProviderId;
    //        params["RefProviderName"] = Patient_Demographic.params.RefProviderName;
    //        params["ProviderId"] = Patient_Demographic.params.PatientProviderId;
    //        params["ProviderName"] = Patient_Demographic.params.PatientProvider;
    //        params["FacilityId"] = Patient_Demographic.params.PatientFacilityId;
    //        params["FaciltyName"] = Patient_Demographic.params.PatientFacility;
    //        params["SelfPay"] = Patient_Demographic.params.SelfPay;
    //        LoadActionPan('Encounter_CreateClaim', params);
    //    }
    //},
    OpenPatientSearchForClaim: function () {
        AppPrivileges.GetFormPrivileges("Encounter", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (localStorage.getItem("SelectedPatientId") != null && localStorage.getItem("SelectedPatientId") != "" && localStorage.getItem("SelectedPatientId") != "-1") {
                    Patient_Demographic.FillDemographic(localStorage.getItem("SelectedPatientId")).done(function (response) {
                        if (response.status != false) {
                            var params = [];
                            params["FromAdmin"] = "0";
                            params['mode'] = "Add";
                            if (Bill_ChargeSearch.params["CaseId"]) {
                                params["CaseId"] = Bill_ChargeSearch.params["CaseId"];
                                params["CaseNo"] = Bill_ChargeSearch.params["CaseNo"];
                            }
                            params["ParentCtrl"] = 'Bill_ChargeSearch';
                            var demographic_detail = JSON.parse(response.DemographicFill_JSON);
                            params["PatientId"] = localStorage.getItem("SelectedPatientId");
                            params["patFullName"] = demographic_detail.AccountNo + " - " + demographic_detail.FullName;
                            params["RefProviderId"] = demographic_detail.RefProviderID;
                            params["RefProviderName"] = demographic_detail.RefProvider;
                            params["ProviderId"] = demographic_detail.ProviderID;
                            params["ProviderName"] = demographic_detail.Provider;
                            params["FacilityId"] = demographic_detail.FacilityID;
                            params["FaciltyName"] = demographic_detail.Facility;
                            params["SelfPay"] = demographic_detail.SelfPay;
                            LoadActionPan('Encounter_CreateClaim', params);
                        }
                    });
                } else {
                    var params = [];
                    params["FromAdmin"] = "0";
                    params["ForNewClaim"] = "1";
                    if (Bill_ChargeSearch.params["CaseId"]) {
                        params["CaseId"] = Bill_ChargeSearch.params["CaseId"];
                        params["CaseNo"] = Bill_ChargeSearch.params["CaseNo"];
                    }
                    params["ParentCtrl"] = 'Bill_ChargeSearch';
                    LoadActionPan('Patient_Search', params);
                }
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },


    //Begin Added by Azeem Raza Tayyab on 29-Apr-2016 for the changes related to Bug#:PMS-4685
    CheckedVisit: function (obj, event) {

        if (event != null) {
            event.stopPropagation();
        }


        if (!$(obj).prop('checked')) {
            $("#" + Bill_ChargeSearch.params.PanelID + " #chkAllVisits").prop('checked', false);
            $("#" + Bill_ChargeSearch.params.PanelID + " #chkAllVisits").attr('title', 'Select all');
            //$("#pnlBillClaimSubmission #dgvSubmission").find("input[type='checkbox']").prop('disabled', false);
        }
        else {

            var selected = [];
            $("#" + Bill_ChargeSearch.params.PanelID + " #" + Bill_ChargeSearch.girdId + " tbody").find("input[type='checkbox']").each(function () {

                if (!$(this).is(":checked")) {
                    selected.push(this);
                }
            });

            if (selected.length > 0) {
                $("#" + Bill_ChargeSearch.params.PanelID + " #chkAllVisits").prop('checked', false);
                $("#" + Bill_ChargeSearch.params.PanelID + " #chkAllVisits").attr('title', 'Select all');

            }
            else {
                $("#" + Bill_ChargeSearch.params.PanelID + " #chkAllVisits").prop('checked', true);
                $("#" + Bill_ChargeSearch.params.PanelID + " #chkAllVisits").attr('title', 'Unselect all');
            }
        }
    },

    OpenSubmitStatusWindow: function () {
        AppPrivileges.GetFormPrivileges("Encounter", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var SelectedVisits = [];
                var visits = Bill_ChargeSearch.GetSelectedVisits();
                var ConfirmDiv = $("#" + Bill_ChargeSearch.params.PanelID + " #divSubmitStatus");
                if (visits.length > 0) {
                    var AllSubmitted = Bill_ChargeSearch.StatusUpdateConfirmation(visits);
                    $.each(visits, function (i, item) {
                        SelectedVisits.push(item.VisitId);
                    });
                    Bill_ChargeSearch.myConfirm(function () {
                        var $option = $("#popDiv").find("#ddlSubmitStatusPopUp option:selected");
                        var $oldOption = $("#" + Bill_ChargeSearch.params["PanelID"] + ' #frmChargeSearch').find("#ddlSubmitStatus option:selected");
                        var ddl = $("#popDiv").find("#ddlSubmitStatusPopUp");
                        if ($option.val() == "0" || $option.val() == "") {
                            ddl.focus();
                            ddl.css("border", "1px solid red");
                            utility.DisplayMessages('Please select submit status to update', 2);
                        }
                        else {
                            if ($option.val() != $oldOption.val()) {
                                ddl.css("border", "");
                                if ($option.text() == "Submitted" && AllSubmitted == false) {
                                    utility.myConfirm("Claim(s) not Submitted/Resubmitted yet. Are you sure you want to change claim status to \“Submitted\”?", function () {
                                        Bill_ChargeSearch.ChangeSubmitStatus(SelectedVisits, $option.val()).done(function (response) {
                                            if (response.status == true) {
                                                
                                                $("#modal-from-dom-ChargeSearch").modal('hide');
                                                var submittedClaimCount = visits.filter(function (obj) {
                                                    return obj.ClaimStatus === "Submitted";
                                                });
                                                if (visits.length > 1 && submittedClaimCount.length>0)
                                                { Bill_ChargeSearch.VisitStatusUpdateInfo(); }
                                                Bill_ChargeSearch.BillChargeSearch();
                                                utility.DisplayMessages(response.Message, 1);
                                            }
                                            else {
                                                utility.DisplayMessages(response.Message, 3);
                                            }

                                        });
                                    }, function () {
                                        return false;
                                    }, 'Confirmation');
                                } else {
                                    Bill_ChargeSearch.ChangeSubmitStatus(SelectedVisits, $option.val()).done(function (response) {
                                        if (response.status == true) {
                                           
                                            $("#modal-from-dom-ChargeSearch").modal('hide');
                                            
                                            var submittedClaimCount = visits.filter(function (obj) {
                                                return obj.ClaimStatus === "Submitted";
                                            });
                                            if (visits.length > 0 && submittedClaimCount.length > 0)
                                            { Bill_ChargeSearch.VisitStatusUpdateInfo(); }
                                            Bill_ChargeSearch.BillChargeSearch();
                                            utility.DisplayMessages(response.Message, 1);
                                        }
                                        else {
                                            utility.DisplayMessages(response.Message, 3);
                                        }

                                    });
                                }
                            }
                            else {
                                utility.DisplayMessages("Submit status must be different.", 2);
                            }
                        }
                    }, function () {
                        if ($("#modal-from-dom-ChargeSearch")) {
                            $("#modal-from-dom-ChargeSearch").remove();
                        }
                        return false;
                    });
                }
                else {
                    utility.DisplayMessages("Please select claim.", 2);
                }
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    OpenAddNoteCommentsWindow: function () {
        AppPrivileges.GetFormPrivileges("Encounter", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var visits = Bill_ChargeSearch.GetSelectedVisits();
                if (visits.length > 0) {
                    var VisitIds = $(visits).map(function () {
                        return this.VisitId;
                    }).get().join(',');

                    var params = [];
                    params["FromAdmin"] = 0;
                    params["ParentCtrl"] = 'Bill_ChargeSearch';
                    params["VisitIds"] = VisitIds;

                    LoadActionPan('Bill_ChargeSearch_AddNote', params);
                }
                else {
                    utility.DisplayMessages("Please select claim.", 2);
                }
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    GetSelectedVisits: function () {

        var Visits = [];
        $("#" + Bill_ChargeSearch.params.PanelID + " #" + Bill_ChargeSearch.girdId).find("input[type='checkbox']").each(function (index) {
            if (index > 0 && $(this).prop('checked'))
                Visits.push({ 'VisitId': $(this).attr("id"), 'Submitted': $(this).parent().parent().attr('submitted'), 'ClaimStatus': $(this).parent().parent().attr('ClaimStatus') });
        });
        return Visits;
    },

    StatusUpdateConfirmation: function (Visits) {
        var AllSubmitted = true;
        $.each(Visits, function (i, item) {
            if (item.Submitted.toLowerCase() == "false") {
                AllSubmitted = false;
            }
        });
        return AllSubmitted
    },

    ChangeSubmitStatus: function (VisitsIds, SubmitStatusId) {
        var data = "VisitsIds=" + VisitsIds + "&SubmitStatusId=" + SubmitStatusId;
        return MDVisionService.defaultService(data, "BILLING_CHARGE", "CHANGE_VISITS_STATUS");
    },

    myConfirm: function (okFunc, cancelFunc) {
        if ($("#modal-from-dom-ChargeSearch")) {
            $("#modal-from-dom-ChargeSearch").remove();
        }
        var ConfirmDiv = $("#" + Bill_ChargeSearch.params.PanelID + " #divSubmitStatus");
        var dialogText = "Please select submit status to change.";
        var dialogTitle = "Change Submit Status";

        okBtnTitle = "Update";
        cancelBtnTitle = "Cancel";

        Bill_ChargeSearch.okFunc = okFunc;
        Bill_ChargeSearch.cancelFunc = cancelFunc;

        //Begin Edited by Azeem Raza Tayyab on 11-May-2016 to fix Bug#:PMS-5159
        var markUp = '';
        markUp = '<div id="modal-from-dom-ChargeSearch" class="modal fade "> <div class="modal-dialog modal-dialog-smd modal-top-adjust"><div class="modal-content">' +
                 '<div class="modal-header">' +
                 '<button type="button" onclick="Bill_ChargeSearch.cancelConfirmDialog();"  class="close" ">' +
                 '<span class=" red">&nbsp;</span><span class="sr-only">Close</span></button>' +
                         '<h4 class="modal-title">' + dialogTitle + ' </h4></div><div class="modal-body bg-white" id="popDiv"><p>' + dialogText + '</p>' + ConfirmDiv.html() + '<div classs=clearfix></div></div>' +
                      '<div class="modal-footer">' +
                         '<a href="#" id="btnNo" onclick="Bill_ChargeSearch.cancelConfirmDialog();" data-dismiss="modal" class="btn btn-primary btn-sm" >' + cancelBtnTitle + '</a>' +
                         '<a href="#" id="btnYes"  onclick="Bill_ChargeSearch.saveConfirmDialog();" class="btn btn-primary btn-sm">' + okBtnTitle + '</a>' +
                      '</div>' +
                  '</div> <div></div>';
        $(markUp).modal({
            show: 'true',
            backdrop: 'static',
            keyboard: false
        }).on('shown.bs.modal', function () {
            // $('#btnNo', this).focus();


        }).on('hidden.bs.modal', function () {
            //$('body').css('overflow', 'auto !important');
            if ($('body').find('.modal-backdrop').length > 0) {
                if ($('body').css('overflow').toLowerCase() != "scroll") {
                    $('body').addClass('modal-open');
                }
                else {
                    $('body').addClass('modal-open');
                }

            }
        });
        //End Edited by Azeem Raza Tayyab on 11-May-2016 to fix Bug#:PMS-5159
    },
    //Begin Edited by Azeem Raza Tayyab on 11-May-2016 to fix Bug#:PMS-5159
    cancelConfirmDialog: function () {
        $("#modal-from-dom-ChargeSearch").modal('hide');
    },
    saveConfirmDialog: function () {
        if (typeof (Bill_ChargeSearch.okFunc) == 'function') {
            setTimeout(Bill_ChargeSearch.okFunc, 50);
        }
        $('body').addClass('modal-open');
    },
    //End Edited by Azeem Raza Tayyab on 11-May-2016 to fix Bug#:PMS-5159
    //End Added by Azeem Raza Tayyab on 29-Apr-2016 for the changes related to Bug#:PMS-4685
    LoadClaimSummary: function (VisitId, PatientId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Charges", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FromAdmin"] = 0;
                params["ParentCtrl"] = 'Bill_ChargeSearch';

                params["VisitId"] = VisitId;
                params["PatientId"] = PatientId;

                LoadActionPan('EncounterClaimSummary', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    VisitStatusUpdateInfo: function () {
        var rendomKey = utility.makeRendomKey();
        utility.makeFuncArray(rendomKey, null, null);
        if ($("#modal-ChargeSearchStatusUpdate")) {
            $("#modal-ChargeSearchStatusUpdate").remove();
        }
        var visits = Bill_ChargeSearch.GetSelectedVisits();
        var submittedClaimCount = visits.filter(function (obj) {
            return obj.ClaimStatus === "Submitted";
        });
        var Recordbeingupdate = visits.length - submittedClaimCount.length;
        var dialogText = "Status of " + Recordbeingupdate + " claims has been updated.";
        var dialogTextNotUpdated="Status of "+submittedClaimCount.length+" claims cannot be Changed as the claims were already submitted."
        var dialogTitle = "Alert! Submitted Claim Status";
        okBtnTitle = "OK";
        var markUp = '';
        markUp = '<div id="modal-ChargeSearchStatusUpdate" class="modal fade "> <div class="modal-dialog modal-dialog-smd modal-top-adjust"><div class="modal-content">' +
                 '<div class="modal-header">' +
                         '<h4 class="modal-title">' + dialogTitle + ' </h4></div><div class="modal-body bg-white" id="popDivMsg"><p>' + dialogText + '</p><p>' + dialogTextNotUpdated + '</p><div classs=clearfix></div></div>' +
                      '<div class="modal-footer">' +            
                         '<a href="#" id="btnUpdateStatusOK"  data-dismiss="modal" class="btn btn-success btn-sm">' + okBtnTitle + '</a>' +
                      '</div>' +
                  '</div> <div></div>';
        $(markUp).modal({
            show: 'true',
            backdrop: 'static',
            keyboard: false
        }).on('shown.bs.modal', function () {
            $('#btnUpdateStatusOK', this).focus();


        }).on('hidden.bs.modal', function () {
            //$('body').css('overflow', 'auto !important');
            if ($('body').find('.modal-backdrop').length > 0) {
                if ($('body').css('overflow').toLowerCase() != "scroll") {
                    $('body').addClass('modal-open');
                }
                else {
                    $('body').addClass('modal-open');
                }

            }
        });
       
    },
}
