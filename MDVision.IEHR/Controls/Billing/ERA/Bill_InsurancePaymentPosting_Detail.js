Bill_Insurance_PaymentPosting_Detail = {
    params: [],
    bIsFirstLoad: true,
    AllClaimsVisitsList: [],
    EditableGrid: null,
    Load: function (params) {
        Bill_Insurance_PaymentPosting_Detail.params = params;
        if (Bill_Insurance_PaymentPosting_Detail.bIsFirstLoad == true) {
            Bill_Insurance_PaymentPosting_Detail.bIsFirstLoad == false;
            if (Bill_Insurance_PaymentPosting_Detail.params != null && Bill_Insurance_PaymentPosting_Detail.params.PanelID != "pnlBillInsurancePaymentPostingDetail") {
                Bill_Insurance_PaymentPosting_Detail.params["PanelID"] = Bill_Insurance_PaymentPosting_Detail.params["PanelID"] + ' #pnlBillInsurancePaymentPostingDetail';
            }
            else {
                Bill_Insurance_PaymentPosting_Detail.params["PanelID"] = "pnlBillInsurancePaymentPostingDetail";
            }
            Bill_Insurance_PaymentPosting_Detail.BindClaimNumber();
            Bill_Insurance_PaymentPosting_Detail.FillDefaultValues();
            Bill_Insurance_PaymentPosting_Detail.InitilizeToggle();
            Bill_Insurance_PaymentPosting_Detail.VisitChargeLoad();
            utility.CreateDatePicker(Bill_Insurance_PaymentPosting_Detail.params.PanelID + ' #frmInsurancePaymentPostSearch #dpCheckDate', function (ev) { }, true);
        }
    },
    InitilizeToggle: function () {
        var self;
        if (Bill_Insurance_PaymentPosting_Detail.params.PanelID.indexOf("pnlBillInsurancePaymentPostingDetail") < 0)
            self = $('#' + Bill_Insurance_PaymentPosting_Detail.params.PanelID + " #pnlBillInsurancePaymentPostingDetail");
        else
            self = $('#' + Bill_Insurance_PaymentPosting_Detail.params.PanelID);
        $(self.find($('[data-plugin-toggle]'))).each(function () {
            var $this = $(this),
                opts = {};
            var pluginOptions = $this.data('plugin-options');
            if (pluginOptions)
                opts = pluginOptions;
            $this.themePluginToggle(opts);
        });
        var Addsection = self.find('#InsurancePaymentPostingDetailInfoDiv section:first');
        Addsection.addClass('active')
        Addsection.find('.toggle-content:first').css('display', 'block');
    },
    FillDefaultValues:function(){
        $("#" + Bill_Insurance_PaymentPosting_Detail.params["PanelID"] + " #txtPatientName").val(Bill_Insurance_PaymentPosting_Detail.params["PatientName"]);
        $("#" + Bill_Insurance_PaymentPosting_Detail.params["PanelID"] + " #hfPatientId").val(Bill_Insurance_PaymentPosting_Detail.params["PatientId"]);
        $("#" + Bill_Insurance_PaymentPosting_Detail.params["PanelID"] + " #txtClaimNumber").val(Bill_Insurance_PaymentPosting_Detail.params["ClaimNumber"]);
        $("#" + Bill_Insurance_PaymentPosting_Detail.params["PanelID"] + " #hfVisitId").val(Bill_Insurance_PaymentPosting_Detail.params["VisitId"]);
        $("#" + Bill_Insurance_PaymentPosting_Detail.params["PanelID"] + " #txtInsurance").val(Bill_Insurance_PaymentPosting_Detail.params["InsurancePlan"]);
        $("#" + Bill_Insurance_PaymentPosting_Detail.params["PanelID"] + " #hfInsuranceId").val(Bill_Insurance_PaymentPosting_Detail.params["InsurancePlanId"]);
        $("#" + Bill_Insurance_PaymentPosting_Detail.params["PanelID"] + " #txtCheckNumber").val(Bill_Insurance_PaymentPosting_Detail.params["CheckNumber"]);
        $("#" + Bill_Insurance_PaymentPosting_Detail.params["PanelID"] + " #dtpVisitDate").val(Bill_Insurance_PaymentPosting_Detail.params["VisitDate"] );
        $("#" + Bill_Insurance_PaymentPosting_Detail.params["PanelID"] + " #txtCheckAmount").val(Bill_Insurance_PaymentPosting_Detail.params["CheckAmount"]);
        $("#" + Bill_Insurance_PaymentPosting_Detail.params["PanelID"] + " #dpCheckDate").val(Bill_Insurance_PaymentPosting_Detail.params["CheckDate"]);
        $("#" + Bill_Insurance_PaymentPosting_Detail.params["PanelID"] + " #hfEobLinked").val(false);
        var $ctrl = $("#" + Bill_Insurance_PaymentPosting_Detail.params.PanelID + " #frmInsurancePaymentPostSearch #txtClaimNumber");
        var $hfctrl = $("#" + Bill_Insurance_PaymentPosting_Detail.params.PanelID + " #frmInsurancePaymentPostSearch #hfPatientId" );
        utility.SetKendoAutoCompleteSourceforValidate($ctrl, $ctrl.val(), $hfctrl, $hfctrl.val());
    },
    ResetSearchFilter:function(){
        $('#' + Bill_Insurance_PaymentPosting_Detail.params["PanelID"] + ' #frmInsurancePaymentPostSearch').resetAllControls();
        $("#" + Bill_Insurance_PaymentPosting_Detail.params.PanelID + " #dgvPaymentPostDetailAdd tbody tr").each(function () { console.log($(this).remove()) });
        $("#" + Bill_Insurance_PaymentPosting_Detail.params.PanelID + " #btnApplyPayment").addClass("disableAll");
    },
    BindClaimNumber: function () {
        var Ctrl = $("#" + Bill_Insurance_PaymentPosting_Detail.params.PanelID + " #txtClaimNumber");
        var hfCtrl = $("#" + Bill_Insurance_PaymentPosting_Detail.params.PanelID + " #hfVisitId");
        var func = function () { return Bill_Insurance_PaymentPosting_Detail.GetClaimNumberArray(Ctrl.val()); };
        var onSelect = function (e) {

            setTimeout(function () {
                var matchobj = Bill_Insurance_PaymentPosting_Detail.AllClaimsVisitsList.filter(function (obj) {
                    return obj.ClaimNumber == $("#" + Bill_Insurance_PaymentPosting_Detail.params.PanelID + " #txtClaimNumber").val().trim();
                });
                if (matchobj.length) {

                    $("#" + Bill_Insurance_PaymentPosting_Detail.params.PanelID + " #hfPatientId").val(matchobj[0].PatientId);
                    $("#" + Bill_Insurance_PaymentPosting_Detail.params.PanelID + " #hfPatientName").val(matchobj[0].PatientName);
                    $("#" + Bill_Insurance_PaymentPosting_Detail.params.PanelID + " #dtpVisitDate").val(matchobj[0].DOSFrom);
                }

                Bill_Insurance_PaymentPosting_Detail.AllClaimsVisitsList = null;
            }, 500);
        };
        utility.BindKendoAutoComplete(Ctrl, 3, "ClaimNumber", "contains", null, func, hfCtrl, onSelect);


    },
    GetClaimNumberArray: function (name) {
        var AllClaimsVisits = [];
        var dfd = new $.Deferred();
        Bill_Insurance_PaymentPosting_Detail.LoadClaimNumbers(name).done(function (responseData) {
            if (responseData.status != false) {
                if (responseData.ClaimsCount > 0) {
                    var Claims = JSON.parse(responseData.ClaimsLoad_JSON);
                    $.each(Claims, function (i, item) {
                        AllClaimsVisits.push({ id: item.VisitId, value: item.ClaimNumber + ' - ( ' + item.AccountNumber + ' - ' + item.PatientName + ' )', PatientId: item.PatientId, DOSFrom: item.DOSFrom, PatientName: item.AccountNumber + ' - ' + item.PatientName, ClaimNumber: item.ClaimNumber });
                    });
                    Bill_Insurance_PaymentPosting_Detail.AllClaimsVisitsList = AllClaimsVisits;
                }
            }
            dfd.resolve(AllClaimsVisits);
        });
        return dfd.promise();
    },
    LoadClaimNumbers: function (claimNumber) {
        var data = "ClaimNumber=" + claimNumber;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "BILLING_BATCHCHARGE_DETAIL", "SEARCH_VISIT_CLAIM");
    },
    FillVisitFromSearch: function (ClaimNumber, VisitId, PatientName, PatientId, VisitDate, event) {
        if (event != null) {
            event.stopPropagation();
        }
        $("#" + Bill_Insurance_PaymentPosting_Detail.params.PanelID + ' #frmInsurancePaymentPostSearch #dtpVisitDate').val(VisitDate);
        var ClaimField = $("#" + Bill_Insurance_PaymentPosting_Detail.params.PanelID + ' #frmInsurancePaymentPostSearch #txtClaimNumber');
        $(ClaimField).val(ClaimNumber);
        var VisitField = $("#" + Bill_Insurance_PaymentPosting_Detail.params.PanelID + ' #frmInsurancePaymentPostSearch #hfVisitId')
        $(VisitField).val(VisitId);
        var PatName = $("#" + Bill_Insurance_PaymentPosting_Detail.params.PanelID + ' #frmInsurancePaymentPostSearch #hfPatientName');
        $(PatName).val(PatientName);
        var PatId = $("#" + Bill_Insurance_PaymentPosting_Detail.params.PanelID + ' #frmInsurancePaymentPostSearch #hfPatientId');
        $(PatId).val(PatientId);
        if ($(ClaimField).data('kendoAutoComplete'))
            utility.SetKendoAutoCompleteSourceforValidate($(ClaimField), ClaimNumber, $(VisitField), VisitId, "ClaimNumber");
        Bill_ChargeSearch.UnLoad();
    },
    VisitChargeLoad: function () {
        var VisitId = $("#" + Bill_Insurance_PaymentPosting_Detail.params["PanelID"] + " #hfVisitId").val();
        Bill_Insurance_PaymentPosting_Detail.LoadVisitCharge(VisitId);
        $("#" + Bill_Insurance_PaymentPosting_Detail.params["PanelID"] + " #hfEobLinked").val(false);
    },
    LoadVisitCharge:function(VisitId){
        Bill_Insurance_PaymentPosting_Detail.LoadClaimVisit(VisitId).done(function (response) {
            if (response.status != false) {
                if (response.ChargesCount > 0) {
                    $("#" + Bill_Insurance_PaymentPosting_Detail.params.PanelID + " #btnApplyPayment").removeClass("disableAll");
                   
                }
                var chargeList = JSON.parse(response.ChargeLoad_JSON);
                Bill_Insurance_PaymentPosting_Detail.ChargeGridLoad(chargeList);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
                $("#" + Bill_Insurance_PaymentPosting_Detail.params.PanelID + " #btnApplyPayment").addClass("disableAll");
            }
        });
    },
    ChargeGridLoad:function(chargeData){
        var PanelInsuranceGrid = "#" + Bill_Insurance_PaymentPosting_Detail.params.PanelID + " #pnlPaymentPost_DetailtAdd";
        var InsuranceGridId = "#" + Bill_Insurance_PaymentPosting_Detail.params.PanelID + " #dgvPaymentPostDetailAdd";
        $(InsuranceGridId + " tbody").find("tr").remove();
        if ($.fn.dataTable.isDataTable(InsuranceGridId)) {
            $(InsuranceGridId).dataTable().fnClearTable();
            $(InsuranceGridId).dataTable().fnDestroy();
        }
        Bill_Insurance_PaymentPosting_Detail.EditableGrid = EMRUtility.MakeEditableGrid(PanelInsuranceGrid, InsuranceGridId, Bill_Insurance_PaymentPosting_Detail, "0", false, false, false, false);
       
        var htmlInsurance = '<option value="">Select Insurance</option>';
        var VisitId = $("#" + Bill_Insurance_PaymentPosting_Detail.params["PanelID"] + " #hfVisitId").val();
      
            Bill_Insurance_PaymentPosting_Detail.SearchVisitInsurance(VisitId).done(function (response) {
           
            if (response.status != false) {
                
                if (response.PatientInsuranceCount > 0)
                {
                    var patIns = JSON.parse(response.PatientInsuranceLoad_JSON);
                    $.each(patIns, function (i, item) {
                        htmlInsurance = htmlInsurance+
                            "<option value=" + item.VisitInsuranceId + " PlanPriority=" + item.PlanPriority + " MaxPlanPriority=" + response.PatientInsuranceCount + ">" + item.InsurancePlanName + "</option>";
                    });
                }
            }
            });
           
            utility.callbackAfterAllDOMLoaded(function () {
                $.each(chargeData, function (i, item) {
                    var CurrentRow = Bill_Insurance_PaymentPosting_Detail.AddNewChargeRow(item.VisitId, item.ChargeCapId, item.DOSFrom, item.CPTCode, item.Billed, item.InsCharges, item.AllowedAmt, item.Fee, item.CPTDescription, item.PatCharges, item.Copay, htmlInsurance, item.VisitInsuranceId, item.PatientId);
                    Bill_Insurance_PaymentPosting_Detail.EditableGrid.datatable.row(CurrentRow);
                });
            });
    },
    
    AddNewChargeRow: function (VisitId, ChargeCapId, DOSFrom, CPTCode, Billed, InsCharges, AllowedAmt, Fee, CPTDescription, PatCharges, Copay, Insurancehtml, VisitInsuranceId, PatientId)
    {
        var currentRow = null;
       
        var TemplateRow = $("#" + Bill_Insurance_PaymentPosting_Detail.params.PanelID + " #pnlPaymentPost_DetailtAdd #dgvPaymentPostDetailAdd tbody tr[id*=-]").last();
        var TemplateRowId = 0;
            currentRow = Bill_Insurance_PaymentPosting_Detail.EditableGrid.rowAdd(TemplateRowId - 1, VisitId);
            //coltype="dropdown"  ddlist="GetCRemittanceCode"
            $(currentRow).attr("ChargeCapId", ChargeCapId);
            $(currentRow).find("td:eq(0)").append("<input type='hidden' name='PatientId' value='" + PatientId + "' />");
            $(currentRow).find("td:eq(0)").append("<input type='hidden' name='VisitId' value='" + VisitId +"' />");
            $(currentRow).find("td:eq(0)").append("<input type='hidden' name='ChargeCapId' value='" + ChargeCapId + "' />");
            
            $(currentRow).find("td:eq(0)").append("<input type='hidden' name='VisitInsuranceId' value='" + VisitInsuranceId + "' />");
            $(currentRow).find("td:eq(0)").append("<input type='hidden' name='EOBManualPostingId' value='" +Bill_Insurance_PaymentPosting_Detail.params.EOBManualPostingId + "' />");
            $(currentRow).find("td:eq(0)").append("<input type='hidden' name='DateOfService' value='" + utility.RemoveTimeFromDate(null, DOSFrom) + "' />");
            $(currentRow).find("td:eq(0)").attr("VisitDate", utility.RemoveTimeFromDate(null,DOSFrom));
            $(currentRow).find("td:eq(0)").append("<label>" + utility.RemoveTimeFromDate(null, DOSFrom) + "</label>");
            $(currentRow).find("td:eq(1)").append("<input type='hidden' name='CPTCode' value='" + CPTCode + "' />");
            $(currentRow).find("td:eq(1)").attr("CPTCode", CPTCode);
            $(currentRow).find("td:eq(1)").append("<label>" + CPTCode + "</label>");
            $(currentRow).find("td:eq(2)").append("<input type='hidden' name='BilledAmount' value='" + Billed + "' />");
            $(currentRow).find("td:eq(2)").attr("Billed", Billed);
            $(currentRow).find("td:eq(2)").append("<label>" + utility.convertToFigure(Billed, true) + "</label>");
            $(currentRow).find("td:eq(3)").append("<input type='hidden' name='ExpectedAmount' value='" + Fee + "' />");
            $(currentRow).find("td:eq(3)").attr("Fee", Fee);
            $(currentRow).find("td:eq(3)").append("<label>" + utility.convertToFigure(Fee, true) + "</label>");
            $(currentRow).find("td:eq(4)").append("<input type='hidden' name='InsCharges' value='" + InsCharges + "' />");
            $(currentRow).find("td:eq(4)").attr("InsCharges", InsCharges);
            $(currentRow).find("td:eq(4)").append("<label>" + utility.convertToFigure(InsCharges, true) + "</label>");
            $(currentRow).find("td:eq(11)").append("<input type='hidden' name='ChargeCopay' value='" + Copay + "' />");
            $(currentRow).find("td:eq(11)").attr("Copay", Copay);
            $(currentRow).find("td:eq(11)").append("<label>" + utility.convertToFigure(Copay, true) + "</label>");
            var htmlNextRes = '<select id=ddlNextRes-' + TemplateRowId + ' name="NextResponsibilityId" class="form-control">'
            + '<option value=""> Select Resp. </option><option value="1">Insurance</option><option value="0">Patient</option></select>';
            $(currentRow).find("td:eq(13)").append(htmlNextRes);
            var htmlInsurance = '<select id="ddlInsurance-' + TemplateRowId + '" name="InsurancePlanId"  class="form-control">'
               + Insurancehtml + '</select>';
            $(currentRow).find("td:eq(14)").append(htmlInsurance);
           
            if (VisitInsuranceId) {
                $(currentRow).find("select[id*='ddlInsurance']").val(VisitInsuranceId);
                if ($(currentRow).find("select[id*='ddlInsurance'] option:selected").next().is("option"))
                {
                    $(currentRow).find("select[id*='ddlInsurance'] option:selected").next().attr("selected", "selected");
                    $(currentRow).find("select[id*='ddlNextRes']").val(1);
                }
                else {
                    //$(currentRow).find("select[id*='ddlInsurance'] option:selected").prev().attr("selected", "selected");
                    $(currentRow).find("select[id*='ddlInsurance']").val("");
                    $(currentRow).find("select[id*='ddlInsurance']").attr("disabled", "disabled");
                    $(currentRow).find("select[id*='ddlNextRes']").val(0);

                }
                //var planPriority=$('option:selected', $(currentRow).find("select[id*='ddlInsurance']")).attr("planpriority");
                //var maxPlanPriority=$('option:selected', $(currentRow).find("select[id*='ddlInsurance']")).attr("maxplanpriority");
                //if (maxPlanPriority == planPriority || !planPriority) {
                //    $(currentRow).find("select[id*='ddlNextRes']").val(0);
                //}
                //else {
                //    $(currentRow).find("select[id*='ddlNextRes']").val(1);
                //}
            }
            else {
                $(currentRow).find("select[id*='ddlNextRes']").val(0);
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
           var htmlGroupCode = '<select id="ddlGroupCode-' + TemplateRowId + '"  class="form-control" name="AdjustmentGroupCodeId"  ddlist="GetAdjustmentGroupCode">'
                       + '<option value="">--Select--</option></select>';
            $(currentRow).find("td:eq(15)").append(htmlGroupCode);
            var self = $(currentRow).find("td:eq(15)");
            self.loadDropDowns(true).done(function () { })
            var htmlReasonCode = '<select id="ddlReasonCode-' + TemplateRowId + '"  class="form-control" name="AdjustmentReasonCodeId" ddlist="GetAdjustmentReasonCode">'
                           + '<option value="">--Select--</option></select>';
            $(currentRow).find("td:eq(16)").append(htmlReasonCode);
             self = $(currentRow).find("td:eq(16)");
            self.loadDropDowns(true).done(function () { })
            var htmlRemarkCode = '<select id="ddlRemarkCode-' + TemplateRowId + '"  class="form-control" name="RemarkCode" ddlist="GetCRemittanceCode">'
                   + '<option value="">--Select--</option></select>';
            $(currentRow).find("td:eq(17)").append(htmlRemarkCode);
             self = $(currentRow).find("td:eq(17)");
            self.loadDropDowns(true).done(function () { });
            //var htmlPosted = '<select id=ddlPosted-' + TemplateRowId + ' name="Posted" class="form-control">'
            //   + '<option value="">--Select--</option><option value="1">Yes</option><option value="0">No</option></select>';
            $(currentRow).find("td:eq(18)").append("No <input type='hidden' name='Posted' value='0' />");
            //var htmlCrossOver = '<select id=ddlCrossOver-' + TemplateRowId + ' name="CrossOver" class="form-control">'
            //       + '<option value="">--Select--</option><option value="1">Yes</option><option value="0">No</option></select>';
            $(currentRow).find("td:eq(19)").append("Yes   <input type='hidden' name='CrossOver' value='1' />");
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
    SavePaymentInsurance: function () {
        var PaymentArray = [];
        $("#" + Bill_Insurance_PaymentPosting_Detail.params.PanelID + " #dgvPaymentPostDetailAdd tbody tr").each(function () {
            var self = $(this);
            var myJSON = self.getMyJSONByName();
            PaymentArray.push(myJSON);

        });
        Bill_Insurance_PaymentPosting_Detail.SavePaymentDetail(PaymentArray).done(function (response) {

            if (response.status != "false") {
                utility.DisplayMessages(response.Message, 1);
                $("#" + Bill_Insurance_PaymentPosting_Detail.params["PanelID"] + " #hfEobLinked").val(true);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    SavePaymentDetail: function (payments) {
        payments = "[" + payments + "]";
        var data = "Paymentdata=" + payments;
      
        return MDVisionService.defaultService(data, "BILLING_EOB_Manual_Posting", "SAVE_PAYMENTS_DETAIL");
    },
    SearchVisitInsurance: function (visitId, patientId) {
        var data = "VisitId=" + visitId + "&PatientId=" + patientId;
        return MDVisionService.defaultService(data, "PATIENT_CHARGE_CAPTURE", "SEARCH_VISIT_INSURANCE");
    },
    
    UnLoad: function () {
        UnloadActionPan(Bill_Insurance_PaymentPosting_Detail.params["ParentCtrl"]);
        Bill_Insurance_Payment_Detail.LoadEOBDetail(0, Bill_Insurance_PaymentPosting_Detail.params.EOBManualPostingId,0);

    },
}