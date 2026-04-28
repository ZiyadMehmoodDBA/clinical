Bill_Insurance_Payment_Detail = {
    params: [],
    bIsFirstLoad: true,
    AllClaimsVisitsList: [],
    EditableGrid: null,
    Load: function (params) {
        Bill_Insurance_Payment_Detail.params = params;
        if (Bill_Insurance_Payment_Detail.bIsFirstLoad) {
            Bill_Insurance_Payment_Detail.bIsFirstLoad = false;
            if (Bill_Insurance_Payment_Detail.params != null && Bill_Insurance_Payment_Detail.params.PanelID != "pnlBillInsurancePaymentDetail") {
                Bill_Insurance_Payment_Detail.params["PanelID"] = Bill_Insurance_Payment_Detail.params["PanelID"] + ' #pnlBillInsurancePaymentDetail';
            }
            else {
                Bill_Insurance_Payment_Detail.params["PanelID"] = "pnlBillInsurancePaymentDetail"
            }

            var self = $('#' + Bill_Insurance_Payment_Detail.params["PanelID"]);
            self.loadDropDowns(true).done(function () {
            });

            CacheManager.BindCodes('GetInsurancePlan', false).done(function (result) {
                var Ctrl = $("#" + Bill_Insurance_Payment_Detail.params["PanelID"] + " input#txtInsurancePlan");
                var hfCtrl = $("#" + Bill_Insurance_Payment_Detail.params["PanelID"] + " #hfInsurancePlan");
                var onSelect = function (e) { $("#" + Bill_Insurance_Payment_Detail.params["PanelID"] + " #hfInsurancePlansearchPattern").val(e.searchPattern); };
                var onChange = function (valid) { Patient_Insurance.ValidateSearchPatternForSelectedInsurancePlan(); };
                utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", InsurancePlans, null, hfCtrl, onSelect, onChange);
            });
            Bill_Insurance_Payment_Detail.ValidateInsurancePaymentDetail();
            Bill_Insurance_Payment_Detail.BindClaimNumber();
            if (Bill_Insurance_Payment_Detail.params.mode && Bill_Insurance_Payment_Detail.params.mode == "Add") {
                $("#" + Bill_Insurance_Payment_Detail.params["PanelID"] + " #btnPostAllCharge").addClass("disableAll");
                $("#" + Bill_Insurance_Payment_Detail.params["PanelID"] + " #btnPostDoc").addClass("disableAll");
            }
            if (Bill_Insurance_Payment_Detail.params.EOBId) {
                Bill_Insurance_Payment_Detail.FillEOBManualPosting(Bill_Insurance_Payment_Detail.params.EOBId);
               
            }
        }

        utility.CreateDatePicker(Bill_Insurance_Payment_Detail.params.PanelID + ' #frmBillInsurancePaymentDetail #dpCheckDate', function (ev) { }, true);
        utility.CreateDatePicker(Bill_Insurance_Payment_Detail.params.PanelID + ' #frmBillInsurancePaymentDetail #dpEntryDate', function (ev) { }, true);
        utility.CreateDatePicker(Bill_Insurance_Payment_Detail.params.PanelID + ' #frmBillInsurancePaymentDetail #dpCheckDepositDate', function (ev) { }, false);
        $("#" + Bill_Insurance_Payment_Detail.params.PanelID + ' #frmBillInsurancePaymentDetail #txtUser').val(globalAppdata.AppUserLastName + ', ' + globalAppdata.AppUserFirstName);
    },
    ValidateInsurancePaymentDetail: function () {
        $('#pnlBillInsurancePaymentDetail #frmBillInsurancePaymentDetail')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  InsurancePlan: {
                      group: '.col-sm-2',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  CheckNumber: {
                      group: '.col-sm-2',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Status: {
                      group: '.col-sm-2',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  CheckAmount: {
                      group: '.col-sm-2',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  CheckDate: {
                      group: '.col-sm-2',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           // confirm races and ethenticity are not empty
           Bill_Insurance_Payment_Detail.SaveInsurancePayment()
       });
    },
    OpenInsurancePlan: function (PlanProvider) {
        var params = [];
        var PanelID = null;
        if (Bill_Insurance_Payment_Detail.params["PanelID"] != "pnlBillInsurancePaymentDetail")
            PanelID = Bill_Insurance_Payment_Detail.params["PanelID"];

        params["ParentCtrl"] = 'Bill_Insurance_Payment_Detail';
        params["InsurancePlanId"] = "-1";
        params["FromAdmin"] = "0";
        if (PlanProvider != null)
            params["RefCtrl"] = PlanProvider;
        LoadActionPan('Admin_InsurancePlan', params, PanelID);
    },
    OpenInsurancePlanDetail: function () {

        var params = [];
        var PanelID = null;
        if (Bill_Insurance_Payment_Detail.params["PanelID"] != "pnlBillInsurancePaymentDetail")
            PanelID = Bill_Insurance_Payment_Detail.params["PanelID"];

        params["ParentCtrl"] = 'Bill_Insurance_Payment_Detail';
        params["InsurancePlanId"] = $("#pnlBillInsurancePaymentDetail #hfInsurancePlan").val();
        params["mode"] = "Edit";
        Admin_InsurancePlan.params["FromAdmin"] == "0";
        LoadActionPan('insurancePlanDetail', params, PanelID);
    },
    FillInsurancePlanName: function (InsurancePlanId, Description) {
        $("#" + Bill_Insurance_Payment_Detail.params.PanelID + ' #frmBillInsurancePaymentDetail #txtInsurancePlan').val(Description);
        $("#" + Bill_Insurance_Payment_Detail.params.PanelID + ' #frmBillInsurancePaymentDetail #hfInsurancePlan').val(InsurancePlanId);
        UnloadActionPan(Admin_InsurancePlan.params.ParentCtrl);
    },
    SaveInsurancePayment: function () {
        var self = $("#" + Bill_Insurance_Payment_Detail.params["PanelID"]);
        var myJSON = self.getMyJSON();
                Bill_Insurance_Payment_Detail.InsurancePaymentSave(myJSON).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        $("#pnlBillManualPost_Detail #hfEOBManualPostingId").val(response.EOBManualPostingId);
                        $("#pnlBillManualPost_Detail").removeClass("disableAll");
                        $("#" + Bill_Insurance_Payment_Detail.params["PanelID"] + " #btnPostDoc").addClass("disableAll");
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
    },
    InsurancePaymentSave: function (insurancePayments) {
        var data = "insurancePayments=" + insurancePayments;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "BILLING_EOB_Manual_Posting", "SAVE_EOB_MANUAL_POSTING");
    },
    BindClaimNumber: function () {
        var Ctrl = $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #txtClaimNumber");
        var hfCtrl = $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #hfVisitId");
        var func = function () { return Bill_Insurance_Payment_Detail.GetClaimNumberArray(Ctrl.val()); };
        var onSelect = function (e) {

            setTimeout(function () {
                var matchobj = Bill_Insurance_Payment_Detail.AllClaimsVisitsList.filter(function (obj) {
                    return obj.ClaimNumber == $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #txtClaimNumber").val().trim();
                });
                if (matchobj.length) {
                    Bill_Insurance_Payment_Detail.InsurancePaymentDetailPresent(matchobj[0].id, $("#pnlBillManualPost_Detail #hfEOBManualPostingId").val());
                    $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #hfPatientId").val(matchobj[0].PatientId);
                    $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #hfPatientName").val(matchobj[0].PatientName);
                    $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #hfDOS").val(utility.RemoveTimeFromDate(null, matchobj[0].DOSFrom));
                }

                Bill_Insurance_Payment_Detail.AllClaimsVisitsList = null;
            }, 500)
        };
        utility.BindKendoAutoComplete(Ctrl, 3, "ClaimNumber", "contains", null, func, hfCtrl, onSelect);


    },
    GetClaimNumberArray: function (name) {
        var AllClaimsVisits = [];
        var dfd = new $.Deferred();
        Bill_Insurance_Payment_Detail.LoadClaimNumers(name).done(function (responseData) {
            if (responseData.status != false) {
                if (responseData.ClaimsCount > 0) {
                    var Claims = JSON.parse(responseData.ClaimsLoad_JSON);
                    $.each(Claims, function (i, item) {
                        AllClaimsVisits.push({ id: item.VisitId, value: item.ClaimNumber + ' - ( ' + item.AccountNumber + ' - ' + item.PatientName + ' )', PatientId: item.PatientId, DOSFrom: item.DOSFrom, PatientName: item.AccountNumber + ' - ' + item.PatientName, ClaimNumber: item.ClaimNumber });
                    });
                    Bill_Insurance_Payment_Detail.AllClaimsVisitsList = AllClaimsVisits;
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
    FillVisitFromSearch: function (ClaimNumber, VisitId, PatientName, PatientId, VisitDate, event) {
        if (event != null) {
            event.stopPropagation();
        }
        $("#" + Bill_Insurance_Payment_Detail.params.PanelID + ' #pnlBillManualPost_Detail #hfDOS').val(VisitDate);
        var ClaimField = $("#" + Bill_Insurance_Payment_Detail.params.PanelID + ' #pnlBillManualPost_Detail #txtClaimNumber');
        $(ClaimField).val(ClaimNumber);
        var VisitField = $("#" + Bill_Insurance_Payment_Detail.params.PanelID + ' #pnlBillManualPost_Detail #hfVisitId')
        $(VisitField).val(VisitId);
        var PatName = $("#" + Bill_Insurance_Payment_Detail.params.PanelID + ' #pnlBillManualPost_Detail #hfPatientName');
        $(PatName).val(PatientName);
        var PatId = $("#" + Bill_Insurance_Payment_Detail.params.PanelID + ' #pnlBillManualPost_Detail #hfPatientId');
        $(PatId).val(PatientId);
        if ($(ClaimField).data('kendoAutoComplete'))
            utility.SetKendoAutoCompleteSourceforValidate($(ClaimField), ClaimNumber, $(VisitField), VisitId, "ClaimNumber");
        Bill_ChargeSearch.UnLoad();
        Bill_Insurance_Payment_Detail.InsurancePaymentDetailPresent(VisitId, $("#pnlBillManualPost_Detail #hfEOBManualPostingId").val());

    },
    InsurancePaymentDetailPresent: function (VisitId, InsurancePaymentDetailId) {
        Bill_Insurance_Payment_Detail.InsurancePaymentDetailExist(VisitId, InsurancePaymentDetailId).done(function (response) {
            if (response.status != false) {
                $("#pnlBillManualPost_Detail #hfIsEobDetailExists").val(response.IsEobDetailExists);
            }

        });
    },
    InsurancePaymentDetailExist: function (VisitId, InsurancePaymentDetailId) {
        var data = "VisitId=" + VisitId + "&InsurancePaymentDetailId=" + InsurancePaymentDetailId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "BILLING_EOB_Manual_Posting", "CHECK_EOB_MANUAL_POSTING_EXIST");
    },
    LoadInsurancePaymentPostingDetail: function () {
        var VisitId = $("#" + Bill_Insurance_Payment_Detail.params.PanelID + ' #pnlBillManualPost_Detail #hfVisitId').val();
        if (VisitId) {
            var EobDetailExist = $("#" + Bill_Insurance_Payment_Detail.params.PanelID + ' #pnlBillManualPost_Detail #hfIsEobDetailExists').val();
            if (EobDetailExist=="1")
            { Bill_Insurance_Payment_Detail.EOBDetailLoad(); }
            else {
                Bill_Insurance_Payment_Detail.LoadPaymentPostingDetailScreen();
            }
        }
        else {
            utility.DisplayMessages("Please Select Claim Number", 3);
        }
    },
    LoadPaymentPostingDetailScreen: function () {
        var params = [];
        var PanelID = null;
        if (Bill_Insurance_Payment_Detail.params["PanelID"] != "pnlBillInsurancePaymentDetail")
        { PanelID = Bill_Insurance_Payment_Detail.params["PanelID"]; }


        params["PatientId"] = $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #hfPatientId").val();
        params["PatientName"] = $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #hfPatientName").val();
        params["EOBManualPostingId"] = $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #hfEOBManualPostingId").val();
        params["VisitId"] = $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #hfVisitId").val();
        params["ClaimNumber"] = $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #txtClaimNumber").val().trim();
        params["InsurancePlan"] = $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #txtInsurancePlan").val().trim();
        params["InsurancePlanId"] = $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #hfInsurancePlan").val().trim();
        params["VisitDate"] = $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #hfDOS").val().trim();
        params["CheckNumber"] = $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #txtCheckNumber").val().trim();
        params["CheckAmount"] = $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #txtCheckAmount").val().trim();
        params["CheckDate"] = $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #dpCheckDate").val().trim();
        params["ParentCtrl"] = 'Bill_Insurance_Payment_Detail';
        Bill_Insurance_Payment_Detail.params["FromAdmin"] == "0";
        LoadActionPan('Bill_Insurance_PaymentPosting_Detail', params, PanelID);
    },
    EOBDetailLoad: function () {
        var VisitId = $("#" + Bill_Insurance_Payment_Detail.params["PanelID"] + " #hfVisitId").val();
        Bill_Insurance_Payment_Detail.LoadEOBDetail(VisitId,0,0);
    },
    LoadEOBDetail: function (VisitId, EOBPostingId,EOBDetlId) {
        Bill_Insurance_Payment_Detail.LoadEObPaymentDetail(VisitId, EOBPostingId, EOBDetlId).done(function (response) {
            if (response.status != false) {
                Bill_Insurance_Payment_Detail.PaymentPostGridLoadNew(response);
                if (response.ChargesCount > 0) {
                    $("#" + Bill_Insurance_Payment_Detail.params["PanelID"] + " #btnPostAllCharge").removeClass("disableAll");
                }
                else {
                    $("#" + Bill_Insurance_Payment_Detail.params["PanelID"] + " #btnPostAllCharge").addClass("disableAll");
                }
                $("#" + Bill_Insurance_Payment_Detail.params["PanelID"] + " #btnPostDoc").removeClass("disableAll");
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    ChargeGridLoad: function (chargeData) {
        var PanelInsuranceGrid = "#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail";
        var InsuranceGridId = "#" + Bill_Insurance_Payment_Detail.params.PanelID + " #dgvPaymentPostDetail";
        $(InsuranceGridId + " tbody").find("tr").remove();
        if ($.fn.dataTable.isDataTable(InsuranceGridId)) {
            $(InsuranceGridId).dataTable().fnClearTable();
            $(InsuranceGridId).dataTable().fnDestroy();
        }
        Bill_Insurance_Payment_Detail.EditableGrid = EMRUtility.MakeEditableGrid(PanelInsuranceGrid, InsuranceGridId, Bill_Insurance_Payment_Detail, "0", false, false, false, false);
        var htmlInsurance = '<option value="">--Select Insurance--</option>';
        var VisitId = $("#" + Bill_Insurance_Payment_Detail.params["PanelID"] + " #hfVisitId").val();

        Bill_Insurance_PaymentPosting_Detail.SearchVisitInsurance(VisitId).done(function (response) {

            if (response.status != false) {

                if (response.PatientInsuranceCount > 0) {
                    var patIns = JSON.parse(response.PatientInsuranceLoad_JSON);
                    $.each(patIns, function (i, item) {
                        htmlInsurance = htmlInsurance +
                            "<option value=" + item.InsuranceId + " PlanPriority=" + item.PlanPriority + " MaxPlanPriority=" + response.PatientInsuranceCount + ">" + item.InsurancePlanName + "</option>";
                    });
                }
            }
        });

        utility.callbackAfterAllDOMLoaded(function () {
            //$.each(chargeData, function (i, item) {
            //    var CurrentRow = Bill_Insurance_Payment_Detail.PaymentPostGridLoadNew(item.VisitId, item.ChargeCapId, item.DOSFrom, item.CPTCode, item.Billed, item.InsCharges, item.AllowedAmt, item.Fee, item.CPTDescription, item.PatCharges, item.Copay, htmlInsurance, item.PatientInsuranceId, item.PatientId);
            //    Bill_Insurance_Payment_Detail.EditableGrid.datatable.row(CurrentRow);
            //});

        });
    },
    AddNewChargeRow: function (VisitId, ChargeCapId, DOSFrom, CPTCode, Billed, InsCharges, AllowedAmt, Fee, CPTDescription, PatCharges, Copay, Insurancehtml, PatientInsuranceId, PatientId) {
        Bill_Insurance_Payment_Detail.EditableGrid.rowAdd(ChargeCapId, VisitId);
    },
    PaymentPostGridLoadNew: function (response) {
        // get Actions
        var actions = "";
        $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #dgvPaymentPostDetail tr th").each(function () {
            if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                var arrActionType = [];
                if ($(this).attr("ActionType") != null) {
                    arrActionType = $(this).attr("ActionType").split(',');
                    actions = EMREditableGrid.GetActions(arrActionType, " #pnlPaymentPost_Detail");
                }
            }
        });
        if ($.fn.dataTable.isDataTable("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail #dgvPaymentPostDetail")) {
            $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail #dgvPaymentPostDetail").dataTable().fnClearTable();
            $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail #dgvPaymentPostDetail").dataTable().fnDestroy();
            $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail #dgvPaymentPostDetail tbody").find("tr").remove();
        }

        $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail #dgvPaymentPostDetail tbody").find("tr").remove();
        if (response.ChargesCount > 0) {
            var PaymentDetailLoad = JSON.parse(response.PaymentDetailLoad_JSON);
            var arraTemp = [];

            $.each(PaymentDetailLoad, function (i, item) {

               

                var $row = $('<tr/>');
                $row.attr("id", item.Id);
                $row.attr("VisitId", item.VisitId);
                var AccountLnk = "<a href='#' onclick=utility.PatientDemographics(" + item.PatientId + ',' + "'Bill_Insurance_Payment_Detail'" + ",event);>" + item.AccountNumber + " </a>";
                var PatientNameLnk = "<a href='#' onclick=utility.PatientDemographics(" + item.PatientId + ',' + "'Bill_Insurance_Payment_Detail'" + ",event);>" + item.PatientName + " </a>";
                var ClaimLnk = "<a href='#' onclick=utility.LoadVisitDetail(" + item.VisitId + "," + item.PatientId + ",'" + 'Bill_Insurance_Payment_Detail' + "'); >" + item.ClaimNumber + " </a>";
                $row.append('<td class="actions" actionid="' + item.Id + '" >' + actions + '</td><td>' + item.InsurancePlanId_text + '</td><td>' + AccountLnk + '</td><td>' + PatientNameLnk + '</td><td>' + ClaimLnk + '</td><td>' + utility.RemoveTimeFromDate(null, item.DateOfService) + '</td><td>' + item.CPTCode + '</td><td>' + item.BilledAmount + '</td><td>' + item.ExpectedAmount + '</td><td>' + item.InsCharges + '</td><td>' + item.AllowedAmount + '</td><td>' + item.PaidAmount + '</td><td>' + item.WriteOffAmount + '</td><td>' + item.Deducables + '</td><td>' + item.CoInsAmount + '</td><td>' + item.EOBCopay + '</td><td>' + item.ChargeCopay + '</td><td>' + item.PatientResp + '</td><td>' + item.NextResponsibilityId_text + '</td><td>' + item.NextResponsibilityId + '</td><td>' + item.AdjustmentGroupCodeId_text + '</td><td>' + item.AdjustmentReasonCodeId_text + '</td><td>' + item.RemarkCode_text + '</td><td>' + item.Posted + '</td><td>' + item.CrossOver + '</td><td>' + item.Comments + '</td>');

                $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail #dgvPaymentPostDetail tbody").last().append($row);
                $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail #dgvPaymentPostDetail tbody #" + item.Id + " td:eq(0)").append("<input type='hidden' value='" + item.Id + "' name='Id' />");
                $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail #dgvPaymentPostDetail tbody #" + item.Id + " td:eq(0)").append("<input type='hidden' value='" + item.PatientId + "' name='PatientId' />");
                $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail #dgvPaymentPostDetail tbody #" + item.Id + " td:eq(0)").append("<input type='hidden' value='" + item.VisitId + "' name='VisitId' />");
                
                $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail #dgvPaymentPostDetail tbody #" + item.Id + " td:eq(0)").append("<input type='hidden' value='" + item.ChargeCapId + "' name='ChargeCapId' />");
                $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail #dgvPaymentPostDetail tbody #" + item.Id + " td:eq(0)").append("<input type='hidden' value='" + item.EOBManualPostingId + "' name='EOBManualPostingId' />");
                
                //$("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail #dgvPaymentPostDetail tbody #" + item.Id + " td:eq(1)").append("<input type='hidden' value='" + item.InsurancePlanId_text + "' name='InsurancePlanId_text' />");
                $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail #dgvPaymentPostDetail tbody #" + item.Id + " td:eq(1)").append("<input type='hidden' value='" + item.VisitInsuranceId + "' name='VisitInsuranceId' />");
                $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail #dgvPaymentPostDetail tbody #" + item.Id + " td:eq(2)").append("<input type='hidden' value='" + item.AccountNumber + "' name='AccountNumber' />");
                $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail #dgvPaymentPostDetail tbody #" + item.Id + " td:eq(3)").append("<input type='hidden' value='" + item.PatientName + "' name='PatientName' />");
                $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail #dgvPaymentPostDetail tbody #" + item.Id + " td:eq(4)").append("<input type='hidden' value='" + item.ClaimNumber + "' name='ClaimNumber' />");
                $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail #dgvPaymentPostDetail tbody #" + item.Id + " td:eq(4)").append("<input type='hidden' value='" + item.CheckNumber + "' name='CheckNumber' />");
                $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail #dgvPaymentPostDetail tbody #" + item.Id + " td:eq(5)").append("<input type='hidden' value='" + utility.RemoveTimeFromDate(null, item.DateOfService) + "' name='DateOfService' />");
                $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail #dgvPaymentPostDetail tbody #" + item.Id + " td:eq(6)").append("<input type='hidden' value='" + item.CPTCode + "' name='CPTCode' />");
                $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail #dgvPaymentPostDetail tbody #" + item.Id + " td:eq(7)").append("<input type='hidden' value='" + item.BilledAmount + "' name='BilledAmount' />");
                $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail #dgvPaymentPostDetail tbody #" + item.Id + " td:eq(8)").append("<input type='hidden' value='" + item.ExpectedAmount + "' name='ExpectedAmount' />");
                $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail #dgvPaymentPostDetail tbody #" + item.Id + " td:eq(9)").append("<input type='hidden' value='" + item.InsCharges + "' name='InsCharges' />");
                $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail #dgvPaymentPostDetail tbody #" + item.Id + " td:eq(16)").append("<input type='hidden' value='" + item.ChargeCopay + "' name='ChargeCopay' />");
                $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail #dgvPaymentPostDetail tbody #" + item.Id + " td:eq(23)").append("<input type='hidden' value='" + item.Posted + "' name='Posted' />");
                $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail #dgvPaymentPostDetail tbody #" + item.Id + " td:eq(24)").append("<input type='hidden' value='" + item.CrossOver + "' name='CrossOver' />");
                $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail #dgvPaymentPostDetail tbody #" + item.Id + " td:eq(25)").append("<input type='hidden' value='" + item.Comments + "' name='Comments' />");
                arraTemp.push({ row: $row });
            });

            //Inalize grid
            var PanelGrid = "#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail";
            var GridId = "#" + Bill_Insurance_Payment_Detail.params.PanelID + " #dgvPaymentPostDetail";


            if (Bill_Insurance_Payment_Detail.EditableGrid != null) {
                $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail #dgvPaymentPostDetail").dataTable().fnDestroy();

            }

            Bill_Insurance_Payment_Detail.EditableGrid = EMRUtility.MakeEditableGrid(PanelGrid, GridId, Bill_Insurance_Payment_Detail, 0, false, true, false, true, false, null);
            
        }
        else {
            $('#' + Bill_Insurance_Payment_Detail.params.PanelID + ' #pnlPaymentPost_Detail #dgvPaymentPostDetail').DataTable({
                "language": {
                    "emptyTable": "No Payment Detail Found."
                }, "autoWidth": false, "bInfo":false,"bPaginate":false,"bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bDestroy": true
            });
        }
        $("#pnlPaymentPost_Detail #dgvPaymentPostDetail_wrapper").find(".datatables-header").remove();
        //EMRUtility.SwicthWidgetInializatoin();
        //$('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body', html: true });
        //EMRUtility.fixDataTableDuplication("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail");
        $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail .table-responsive").removeClass("Of-a");

    },
    EditNewChargeRow: function (VisitId, ChargeCapId, DOSFrom, CPTCode, Billed, InsCharges, AllowedAmt, Fee, CPTDescription, PatCharges, Copay, Insurancehtml, PatientInsuranceId, PatientId) {
        var currentRow = null;

        var TemplateRow = $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_DetailAdd #dgvPaymentPostDetailAdd tbody tr[id*=-]").last();
        var TemplateRowId = 0;
        currentRow = Bill_Insurance_Payment_Detail.EditableGrid.rowAdd(TemplateRowId - 1, VisitId);
        //coltype="dropdown"  ddlist="GetCRemittanceCode"
        $(currentRow).attr("ChargeCapId", ChargeCapId);
        $(currentRow).find("td:eq(0)").append("<input type='hidden' name='PatientId' value=" + PatientId + "/>");
        $(currentRow).find("td:eq(0)").append("<input type='hidden' name='VisitId' value=" + VisitId + "/>");
        $(currentRow).find("td:eq(0)").append("<input type='hidden' name='ChargeCapId' value=" + ChargeCapId + "/>");
        $(currentRow).find("td:eq(0)").append("<input type='hidden' name='DateOfService' value=" + utility.RemoveTimeFromDate(null, DOSFrom) + "/>");
        $(currentRow).find("td:eq(0)").attr("VisitDate", utility.RemoveTimeFromDate(null, DOSFrom));
        $(currentRow).find("td:eq(0)").append("<label>" + utility.RemoveTimeFromDate(null, DOSFrom) + "</label>");
        $(currentRow).find("td:eq(1)").append("<input type='hidden' name='CPTCode' value=" + CPTCode + "/>");
        $(currentRow).find("td:eq(1)").attr("CPTCode", CPTCode);
        $(currentRow).find("td:eq(1)").append("<label>" + CPTCode + "</label>");
        $(currentRow).find("td:eq(2)").append("<input type='hidden' name='BilledAmount' value=" + Billed + "/>");
        $(currentRow).find("td:eq(2)").attr("Billed", Billed);
        $(currentRow).find("td:eq(2)").append("<label>" + utility.convertToFigure(Billed, true) + "</label>");
        $(currentRow).find("td:eq(3)").append("<input type='hidden' name='ExpectedAmount' value=" + Fee + "/>");
        $(currentRow).find("td:eq(3)").attr("Fee", Fee);
        $(currentRow).find("td:eq(3)").append("<label>" + utility.convertToFigure(Fee, true) + "</label>");
        $(currentRow).find("td:eq(4)").append("<input type='hidden' name='InsCharges' value=" + InsCharges + "/>");
        $(currentRow).find("td:eq(4)").attr("InsCharges", InsCharges);
        $(currentRow).find("td:eq(4)").append("<label>" + utility.convertToFigure(InsCharges, true) + "</label>");
        $(currentRow).find("td:eq(11)").append("<input type='hidden' name='Copay' value=" + Copay + "/>");
        $(currentRow).find("td:eq(11)").attr("Copay", Copay);
        $(currentRow).find("td:eq(11)").append("<label>" + utility.convertToFigure(Copay, true) + "</label>");
        var htmlNextRes = '<select id=ddlNextRes-' + TemplateRowId + ' name="NextResponsibility" class="form-control">'
        + '<option value=""> Select Resp. </option><option value="1">Insurance</option><option value="0">Patient</option></select>';
        $(currentRow).find("td:eq(13)").append(htmlNextRes);
        var htmlInsurance = '<select id="ddlInsurance-' + TemplateRowId + '" name="InsurancePlan"  class="form-control">'
           + Insurancehtml + '</select>';
        $(currentRow).find("td:eq(14)").append(htmlInsurance);

        if (PatientInsuranceId) {
            $(currentRow).find("select[id*='ddlInsurance']").val(PatientInsuranceId);
            if ($(currentRow).find("select[id*='ddlInsurance'] option:selected").next().is("option"))
            { $(currentRow).find("select[id*='ddlInsurance'] option:selected").next().attr("selected", "selected"); }
            var planPriority = $('option:selected', $(currentRow).find("select[id*='ddlInsurance']")).attr("planpriority");
            var maxPlanPriority = $('option:selected', $(currentRow).find("select[id*='ddlInsurance']")).attr("maxplanpriority");
            if (maxPlanPriority == planPriority) {
                $(currentRow).find("select[id*='ddlNextRes']").val(0);
            }
            else {
                $(currentRow).find("select[id*='ddlNextRes']").val(1);
            }
        }
        $(currentRow).find("select[id*='ddlNextRes']").on("change", function () {
            if ($(this).val() == 0) // if Patient is selected
            {
                $(this).parent().parent().find("select[id*='ddlInsurance']").val("");
                $(this).parent().parent().find("select[id*='ddlInsurance']").attr("disabled", "disabled");
            }
            else {
                $(this).parent().parent().find("select[id*='ddlInsurance']").removeAttr("disabled", "disabled");
            }

        });
        var htmlGroupCode = '<select id="ddlGroupCode-' + TemplateRowId + '"  class="form-control" name="AdjustmentGroupCode"  ddlist="GetAdjustmentGroupCode">'
                    + '<option value="">--Select--</option></select>';
        $(currentRow).find("td:eq(15)").append(htmlGroupCode);
        var self = $(currentRow).find("td:eq(15)");
        self.loadDropDowns(true).done(function () { })
        var htmlReasonCode = '<select id="ddlReasonCode-' + TemplateRowId + '"  class="form-control" name="AdjustmentReasonCode" ddlist="GetAdjustmentReasonCode">'
                       + '<option value="">--Select--</option></select>';
        $(currentRow).find("td:eq(16)").append(htmlReasonCode);
        self = $(currentRow).find("td:eq(16)");
        self.loadDropDowns(true).done(function () { })
        var htmlRemarkCode = '<select id="ddlRemarkCode-' + TemplateRowId + '"  class="form-control" name="RemarkCode" ddlist="GetCRemittanceCode">'
               + '<option value="">--Select--</option></select>';
        $(currentRow).find("td:eq(17)").append(htmlRemarkCode);
        self = $(currentRow).find("td:eq(17)");
        self.loadDropDowns(true).done(function () { });
        var htmlPosted = '<select id=ddlPosted-' + TemplateRowId + ' name="Posted" class="form-control">'
           + '<option value="">--Select--</option><option value="1">Yes</option><option value="0">No</option></select>';
        $(currentRow).find("td:eq(18)").append(htmlPosted);
        var htmlCrossOver = '<select id=ddlCrossOver-' + TemplateRowId + ' name="CrossOver" class="form-control">'
               + '<option value="">--Select--</option><option value="1">Yes</option><option value="0">No</option></select>';
        $(currentRow).find("td:eq(19)").append(htmlCrossOver);
        var AllowedAmount = $(currentRow).find('input[name*="AllowedAmount"]');
        AllowedAmount.attr("data-plugin-keyboard-numpad", "data-plugin-keyboard-numpad");
        var PaidAmount = $(currentRow).find('input[name*="PaidAmount"]');
        PaidAmount.attr("data-plugin-keyboard-numpad", "data-plugin-keyboard-numpad");
        var WriteOffAmount = $(currentRow).find('input[name*="WriteOffAmount"]');
        WriteOffAmount.attr("data-plugin-keyboard-numpad", "data-plugin-keyboard-numpad");
        var DeductionAmount = $(currentRow).find('input[name*="Deducables"]');
        DeductionAmount.attr("data-plugin-keyboard-numpad", "data-plugin-keyboard-numpad");
        var CoInsAmount = $(currentRow).find('input[name*="CoInsAmount"]');
        CoInsAmount.attr("data-plugin-keyboard-numpad", "data-plugin-keyboard-numpad");
        var EOBCopay = $(currentRow).find('input[name*="EOBCopay"]');
        EOBCopay.attr("data-plugin-keyboard-numpad", "data-plugin-keyboard-numpad");
        var PatResp = $(currentRow).find('input[name*="PatientResp"]');
        PatResp.attr("data-plugin-keyboard-numpad", "data-plugin-keyboard-numpad");
        $(currentRow).find('[data-plugin-keyboard-numpad]').keyboard({
            customLayout: {
                'default': [
                    '7 8 9 {b}',
                    '4 5 6 {clear}',
                    '1 2 3 {t}',
                    '0   .  {a} {c} '
                ]
            },
            change: function (e, keyboard, el) {
                if (keyboard.$preview.attr('maxlength') != null && !keyboard.$preview.keyboard().getkeyboard().options.maxLength) {
                    keyboard.$preview.keyboard().getkeyboard().options.maxLength = keyboard.$preview.attr('maxlength');
                }
                if (keyboard.$preview.attr('oninput') != null) {
                    keyboard.$preview.trigger('oninput');
                } else if (keyboard.$preview.attr('onkeyup') != null) {
                    keyboard.$preview.trigger('onkeyup');
                } else if (keyboard.$preview.attr('onkeydown') != null) {
                    keyboard.$preview.trigger('onkeydown');
                }
            },
            layout: 'custom',
            appendLocally: this,
            restrictInput: true, // Prevent keys not in the displayed keyboard from being typed in
            preventPaste: true,  // prevent ctrl-v and right click
            usePreview: false,
            autoAccept: true,
            tabNavigation: true
        })
                .addTyping()
            .on('focus', function (ev) {
                var offset = $(this).position();
                $(this).parent().find('.ui-keyboard').attr('style', 'top: ' + (offset.top + 22) + 'px !important;');
            });
        return currentRow;
        
    },
    LoadClaimVisit: function (VisitId) {
        var data = "VisitId=" + VisitId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "BILLING_EOB_Manual_Posting", "LOAD_CHARGE_DETAIL");
    },
    LoadEObPaymentDetail: function (VisitId, EOBPostingId, EOBDtlId) {
        var data = "VisitId=" + VisitId + "&EOBPostingId=" + EOBPostingId + "&EOBDtlId=" + EOBDtlId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "BILLING_EOB_Manual_Posting", "LOAD_EOB_PAYMENT_DETAIL");
    },
    rowSave: function ($row) {
        console.log("save detail");
        var PaymentArray = [];
        //$("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlBillManualPost_Detail #dgvPaymentPostDetail tbody tr").each(function () {
        //    var self = $(this);
        //    var myJSON = self.getMyJSONByName();
        //    PaymentArray.push(myJSON);

        //});
        var myJSON = $row.getMyJSONByName();
        PaymentArray.push(myJSON);
        var EOBPostingId = $($row).find("input[name*=EOBManualPostingId]").val();
        Bill_Insurance_Payment_Detail.UpdatePaymentDetail(PaymentArray, EOBPostingId);

    },
    rowEdit: function ($row, obj) {
        var Insurancehtml = '<option value="">Select Insurance</option>';
        var VisitId = $($row).attr("VisitId"); //$("#" + Bill_Insurance_Payment_Detail.params["PanelID"] + " #hfVisitId").val();
        var EOBDetailID = $($row).attr("id");
        var rowDetail;
        Bill_Insurance_PaymentPosting_Detail.SearchVisitInsurance(VisitId).done(function (response) {

            if (response.status != false) {

                if (response.PatientInsuranceCount > 0) {
                    var patIns = JSON.parse(response.PatientInsuranceLoad_JSON);
                    $.each(patIns, function (i, item) {
                        Insurancehtml = Insurancehtml +
                            "<option value=" + item.VisitInsuranceId + " PlanPriority=" + item.PlanPriority + " MaxPlanPriority=" + response.PatientInsuranceCount + ">" + item.InsurancePlanName + "</option>";
                    });
                }
            }
        });

        
        utility.callbackAfterAllDOMLoaded(function () {

            //edit row
            var RowId = $($row).attr("id");
            var VisitId = $($row).attr("visitid");
            $(Bill_Insurance_Payment_Detail.EditableGrid.options.table + " tr th").each(function (i) {
                var $this = $row.find('td:nth-child(' + (i + 1) + ')');
                var controlId = $(this).attr("controlid");
                if (!controlId) {
                    controlId = "";
                }
                var controlName = $(this).attr("controlname");
                if (!controlName) {
                    controlName = "";
                }
                var coltype = $(this).attr("coltype");
                if (coltype && coltype.toLowerCase() == "action") {

                    $row.find('.on-editing').removeClass('hidden');
                    $row.find('.on-default').addClass('hidden');
                    $this.addClass('actions');
                }
            });
            $($row).find("td:eq(10)").empty();
            $($row).find("td:eq(10)").append("<input type='text' name='AllowedAmount' class='form-control' /> ");
            $($row).find("td:eq(11)").empty();
            $($row).find("td:eq(11)").append("<input type='text' name='PaidAmount' class='form-control' /> ");
            $($row).find("td:eq(12)").empty();
            $($row).find("td:eq(12)").append("<input type='text' name='WriteOffAmount' class='form-control' /> ");
            $($row).find("td:eq(13)").empty();
            $($row).find("td:eq(13)").append("<input type='text' name='Deducables' class='form-control' /> ");
            $($row).find("td:eq(14)").empty();
            $($row).find("td:eq(14)").append("<input type='text' name='CoInsAmount' class='form-control' /> ");
            $($row).find("td:eq(15)").empty();
            $($row).find("td:eq(15)").append("<input type='text' name='EOBCopay' class='form-control' /> ");
            $($row).find("td:eq(17)").empty();
            $($row).find("td:eq(17)").append("<input type='text' name='PatientResp' class='form-control' /> ");
            var AllowedAmount = $($row).find('input[name*="AllowedAmount"]');
            AllowedAmount.attr("data-plugin-keyboard-numpad", "data-plugin-keyboard-numpad");
            var PaidAmount = $($row).find('input[name*="PaidAmount"]');
            PaidAmount.attr("data-plugin-keyboard-numpad", "data-plugin-keyboard-numpad");
            var WriteOffAmount = $($row).find('input[name*="WriteOffAmount"]');
            WriteOffAmount.attr("data-plugin-keyboard-numpad", "data-plugin-keyboard-numpad");
            var DeductionAmount = $($row).find('input[name*="Deducables"]');
            DeductionAmount.attr("data-plugin-keyboard-numpad", "data-plugin-keyboard-numpad");
            var CoInsAmount = $($row).find('input[name*="CoInsAmount"]');
            CoInsAmount.attr("data-plugin-keyboard-numpad", "data-plugin-keyboard-numpad");
            var EOBCopay = $($row).find('input[name*="EOBCopay"]');
            EOBCopay.attr("data-plugin-keyboard-numpad", "data-plugin-keyboard-numpad");
            var PatResp = $($row).find('input[name*="PatientResp"]');
            PatResp.attr("data-plugin-keyboard-numpad", "data-plugin-keyboard-numpad");
            $($row).find('[data-plugin-keyboard-numpad]').keyboard({
                customLayout: {
                    'default': [
                        '7 8 9 {b}',
                        '4 5 6 {clear}',
                        '1 2 3 {t}',
                        '0   .  {a} {c} '
                    ]
                },
                change: function (e, keyboard, el) {
                    if (keyboard.$preview.attr('maxlength') != null && !keyboard.$preview.keyboard().getkeyboard().options.maxLength) {
                        keyboard.$preview.keyboard().getkeyboard().options.maxLength = keyboard.$preview.attr('maxlength');
                    }
                    if (keyboard.$preview.attr('oninput') != null) {
                        keyboard.$preview.trigger('oninput');
                    } else if (keyboard.$preview.attr('onkeyup') != null) {
                        keyboard.$preview.trigger('onkeyup');
                    } else if (keyboard.$preview.attr('onkeydown') != null) {
                        keyboard.$preview.trigger('onkeydown');
                    }
                },
                layout: 'custom',
                appendLocally: this,
                restrictInput: true, // Prevent keys not in the displayed keyboard from being typed in
                preventPaste: true,  // prevent ctrl-v and right click
                usePreview: false,
                autoAccept: true,
                tabNavigation: true
            })
                    .addTyping()
                .on('focus', function (ev) {
                    var offset = $(this).position();
                    $(this).parent().find('.ui-keyboard').attr('style', 'top: ' + (offset.top + 22) + 'px !important;');
                });

            $($row).find("td:eq(18)").empty();
            var htmlNextRes = '<select id=ddlNextRes-' + "-1" + ' name="NextResponsibilityId" class="form-control">'
           + '<option value=""> Select Resp. </option><option value="1">Insurance</option><option value="0">Patient</option></select>';
            $($row).find("td:eq(18)").append(htmlNextRes);
            var htmlInsurance = '<select id="ddlInsurance-' + "-1" + '" name="InsurancePlanId"  class="form-control">'
               + Insurancehtml + '</select>';
            $($row).find("td:eq(19)").empty();
            $($row).find("td:eq(19)").append(htmlInsurance);
            $($row).find("select[id*='ddlNextRes']").on("change", function () {
                if ($(this).val() == 0) // if Patient is selected
                {
                    $(this).parent().parent().find("select[id*='ddlInsurance']").val("");
                    $(this).parent().parent().find("select[id*='ddlInsurance']").attr("disabled", "disabled");
                }
                else {
                    $(this).parent().parent().find("select[id*='ddlInsurance']").removeAttr("disabled", "disabled");
                }

            });
            var htmlGroupCode = '<select id="ddlGroupCode-' + "-1" + '"  class="form-control" name="AdjustmentGroupCodeId"  ddlist="GetAdjustmentGroupCode">'
                        + '<option value="">--Select--</option></select>';
            $($row).find("td:eq(20)").empty();
            $($row).find("td:eq(20)").append(htmlGroupCode);
            var self = $($row).find("td:eq(20)");
            self.loadDropDowns(true).done(function () { })
            var htmlReasonCode = '<select id="ddlReasonCode-' + "-1" + '"  class="form-control" name="AdjustmentReasonCodeId" ddlist="GetAdjustmentReasonCode">'
                           + '<option value="">--Select--</option></select>';
            $($row).find("td:eq(21)").empty();
            $($row).find("td:eq(21)").append(htmlReasonCode);
            self = $($row).find("td:eq(21)");
            self.loadDropDowns(true).done(function () { })
            var htmlRemarkCode = '<select id="ddlRemarkCode-' + "-1" + '"  class="form-control" name="RemarkCode" ddlist="GetCRemittanceCode">'
                   + '<option value="">--Select--</option></select>';
            $($row).find("td:eq(22)").empty();
            $($row).find("td:eq(22)").append(htmlRemarkCode);
            self = $($row).find("td:eq(22)");
            self.loadDropDowns(true).done(function () { });
            // bind row data
            Bill_Insurance_Payment_Detail.LoadEObPaymentDetail(VisitId,0 ,EOBDetailID).done(function (response) {
                if (response.status != false) {
                    if (response.ChargesCount > 0) {
                        var PaymentDetailLoad = JSON.parse(response.PaymentDetailLoad_JSON);
                        var arraTemp = [];

                        $.each(PaymentDetailLoad, function (i, item) {
                            
                             $($row).find('input[name*="AllowedAmount"]').val(item.AllowedAmount);
                             $($row).find('input[name*="PaidAmount"]').val(item.PaidAmount);
                             $($row).find('input[name*="WriteOffAmount"]').val(item.WriteOffAmount);
                             $($row).find('input[name*="Deducables"]').val(item.Deducables);
                             $($row).find('input[name*="CoInsAmount"]').val(item.CoInsAmount);
                             $($row).find('input[name*="EOBCopay"]').val(item.EOBCopay);
                             $($row).find('input[name*="PatientResp"]').val(item.PatientResp);
                             setTimeout(function () {
                                 if (item.NextResponsibilityId_text.toLowerCase() == "insurance") {
                                     $($row).find('select[name*="NextResponsibility"]').val(1);
                                     $($row).find('select[name*="InsurancePlanId"]').val(item.NextResponsibilityId);
                                 }
                                 else if (item.NextResponsibilityId_text.toLowerCase() == "patient") {
                                     $($row).find('select[name*="NextResponsibility"]').val(0);
                                     $($row).find('select[name*="InsurancePlanId"]').attr("disabled","disabled");
                                 }
                             $($row).find('select[name*="AdjustmentGroupCode"]').val(item.AdjustmentGroupCodeId);
                             $($row).find('select[name*="AdjustmentReasonCode"]').val(item.AdjustmentReasonCodeId);
                             $($row).find('select[name*="RemarkCode"]').val(item.RemarkCode);
                             }, 1000);
                        });
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
                $("#" + Bill_Insurance_Payment_Detail.params.PanelID + " #pnlPaymentPost_Detail #dgvPaymentPostDetail ").css("border", "0px");
            });

        });
    },
    rowCancel: function ($row, obj) {
        var _self = obj,
            $actions,
            i,
            data;

        if ($row.hasClass('adding')) {
            _self.datatable.row($row.get(0)).remove().draw();

        } else {

            data = _self.datatable.row($row.get(0)).data();
            _self.datatable.row($row.get(0)).data(data);
            _self.datatable.draw();
        }
    },
    rowRemove: function ($row, obj) {
        var EOBPostingId = $($row).find("input[name*=EOBManualPostingId]").val();
        utility.myConfirm("Do you want to remove the payments/adjustments posted for claim "+"<b>"+$($row).find("input[name*=ClaimNumber]").val()+"</b> for Check <b>" + $($row).find("input[name*=CheckNumber]").val()+"</b> ?", function () {
            var detailId = $($row).attr("id");
            Bill_Insurance_Payment_Detail.DeleteEOBManualPostDetail(detailId).done(function (response) {

                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    Bill_Insurance_Payment_Detail.LoadEOBDetail(0, EOBPostingId, 0);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }, function () { },
                'Confirm Delete'
            );
    },
    rowPaymentDetail: function ($row, obj) {
        Bill_Insurance_Payment_Detail.OpenPaymentPosting(($row).find("input[name*='VisitId']").val(), ($row).find("input[name*='PatientId']").val())
    },
    UpdatePaymentDetail: function (payment,EOBPostingId) {
        Bill_Insurance_Payment_Detail.PaymentDetailUpdate(payment).done(function (response) {
            if (response.status != false) {
                utility.DisplayMessages(response.message, 1);
                Bill_Insurance_Payment_Detail.LoadEOBDetail(0,EOBPostingId,0);
            }
            else {
                utility.DisplayMessages(response.message, 1);
            }
        });
    },
    PaymentDetailUpdate: function (payments) {
        payments = payments;
        var data = "Paymentdata=" + payments;

        return MDVisionService.defaultService(data, "BILLING_EOB_Manual_Posting", "UPDATE_PAYMENTS_DETAIL");
    },
    FillEOBManualPosting: function (EOBPostingId) {

        Bill_Insurance_Payment_Detail.FillEOBManualPostingData(EOBPostingId).done(function (response) {
            if (response.status != false) {
                var EOBManualPosting_detail = JSON.parse(response.EOBManualPostingLoad_JSON);
                var self =$("#"+ Bill_Insurance_Payment_Detail.params["PanelID"]);
                utility.bindMyJSON(true, EOBManualPosting_detail, false, self).done(function () {  
                });
                Bill_Insurance_Payment_Detail.LoadEOBDetail(0, EOBPostingId,0);
                $("#pnlBillManualPost_Detail").removeClass("disableAll");
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    FillEOBManualPostingData: function (EOBId) {       
        var data = "EOBId=" + EOBId;
        return MDVisionService.defaultService(data, "BILLING_EOB_Manual_Posting", "FILL_EOB_MANUAL_POSTING");
    },
    PostAllCharge: function () {
        var EOBId = $("#pnlBillManualPost_Detail #hfEOBManualPostingId").val();
        Bill_Insurance_Payment_Detail.PostEOBManualPaymentData(EOBId, 0).done(function (response) {
            if (response.status != 'false') {
                utility.DisplayMessages(response.message, 1);
            }
        });
    },
    rowPayment:function($row,obj){
        var detailId = $($row).attr("id");
        var EObId = ($row).find("input[name*='EOBManualPostingId']").val();
        Bill_Insurance_Payment_Detail.PostEOBManualPaymentData(EObId, detailId).done(function (response) {
            if (response.status != 'false') {
                utility.DisplayMessages(response.message, 1);
                Bill_Insurance_Payment_Detail.LoadEOBDetail(0, EObId, 0);
            }
        });
    },
    PostEOBManualPaymentData: function (EOBId,EOBDetlId) {       
        var data = "EOBId=" + EOBId + "&EOBDetlId=" + EOBDetlId;
        return MDVisionService.defaultService(data, "BILLING_EOB_Manual_Posting", "POST_MANUAL_PAYMENT");
    },
    DeleteEOBManualPostDetail: function (EOBDetlId) {
        var data ="EOBDetlId=" + EOBDetlId;
        return MDVisionService.defaultService(data, "BILLING_EOB_Manual_Posting", "DELETE_MANUAL_PAYMENT_DETAIL");
    },
    LoadEOBManualPostDocument: function () {
        var EOBPostingId = $("#pnlBillManualPost_Detail #hfEOBManualPostingId").val();
        Bill_Insurance_Payment_Detail.EOBManualPostDocument(EOBPostingId).done(function (response) {
            if (response.status != 'false') {
                if (response.PatDocId == 0) {
                    Bill_Insurance_Payment_Detail.OpenDocumentManager();
                }
                else {
                    Bill_Insurance_Payment_Detail.OpenDocumentViewer(response.PatDocId);
                }
            }
        });
    },
    EOBManualPostDocument: function (EOBId) {
        var data = "EOBId=" + EOBId;
        return MDVisionService.defaultService(data, "BILLING_EOB_Manual_Posting", "LOAD_MANUAL_POSTING_DOCUMENT");
    },
    OpenDocumentManager: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Documents", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["EOBPostingId"] = $("#pnlBillManualPost_Detail #hfEOBManualPostingId").val();
                params["FromAdmin"] = "0";
                params["UserId"] = globalAppdata['AppUserId'];
                params["ParentCtrl"] = "Bill_Insurance_Payment_Detail";
                params["RefCtrl"] = "ImportDoc";
                params["mode"] = "Add";
                LoadActionPan('Document_Import', params);
             
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    OpenDocumentViewer(PatDocId){
        AppPrivileges.GetFormPrivileges("Documents", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["PatDocID"] = PatDocId;
                params["mode"] = "Edit";
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = "Bill_Insurance_Payment_Detail";
                params["EOBPostingId"] = $("#pnlBillManualPost_Detail #hfEOBManualPostingId").val();
                LoadActionPan('Document_Viewer', params);
            }
            else { utility.DisplayMessages(strMessage, 2); }
        });
    },
    OpenPaymentPosting: function (VisitId, PatientId) {
        AppPrivileges.GetFormPrivileges("Payment Posting", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FromAdmin"] = 0;
                params["ParentCtrl"] = 'Bill_Insurance_Payment_Detail';
                params["VisitId"] = VisitId;
                params["PaymentRef"] = params["ParentCtrl"];
                params["IsFromCollectCopay"] = false ;
                params["patientID"] = PatientId;
                LoadActionPan('Bill_PaymentPosting', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    UnLoadTab: function () {


        UnloadActionPan(Bill_Insurance_Payment_Detail.params["TabID"]);
    },
}