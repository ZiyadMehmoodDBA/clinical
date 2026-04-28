
chargeSearchDetail = {
    params: [],
    bIsFirstLoad: true,
    objChargeDetail: null,
    IsLocked: false,
    ChargeStatus:"",
    Load: function (params) {
        chargeSearchDetail.params = params;


        if (chargeSearchDetail.bIsFirstLoad) {

            chargeSearchDetail.bIsFirstLoad = false;
            if (chargeSearchDetail.params.PanelID != "chargeSearchDetail") {
                chargeSearchDetail.params.PanelID += " #chargeSearchDetail";
            }

            //chargeSearchDetail.LoadAllAutocomplete();

            var self = $('#' + chargeSearchDetail.params.PanelID);
            self.loadDropDowns(true).done(function () {
                chargeSearchDetail.LoadChargeDetail(true);
                //adnan maqbool, PMS-4042, 18-02-2016
                chargeSearchDetail.ValidateChargeDetail();
                //end
            });
        }
        
        
        
    },


    LoadChargeDetail: function (isFromLoad) {


        if (chargeSearchDetail.params.mode == "Edit") {
            //AppPrivileges.GetFormPrivileges("Appointment Status", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            //    if (strMessage == "") {
            chargeSearchDetail.FillChargeDetail(chargeSearchDetail.params.ChargeCapId).done(function (response) {
                if (response.status != false) {


                    var charge = JSON.parse(response.ChargesFill_JSON);
                    var charge_detail = JSON.parse(response.ChargesDetail_JSON);
                    chargeSearchDetail.objChargeDetail = charge_detail[0];

                    chargeSearchDetail.IsLocked = charge_detail[0].IsLocked == "True" ? true : false;
                    chargeSearchDetail.ChargeStatus = charge_detail[0].Status;


                    var self = $("#chargeSearchDetail");
                    self.find('#aEOB').removeClass('disabled');

                    utility.bindMyJSON(true, charge, false, self).done(function () {

                        BackgroundLoaderShow(false);
                        //chargeSearchDetail.FillChargePOS("hfFacility", "txtPOS");
                        //serialize Data.
                        //Begin Edited by Azeem Raza Tayyab on 04-Feb-2016 to fix Bug #PMS-3675
                        if (charge_detail[0].IsPrimary == "False") {
                            self.find('#chkPrimary').val("True");
                        }
                        else if (charge_detail[0].IsPrimary == "True") {
                            self.find('#chkPrimary').val("False");
                        }
                        //End Edited by Azeem Raza Tayyab on 04-Feb-2016 to fix Bug #PMS-3675
                        $('#frmChargeSearchDetail').data('serialize', $('#frmChargeSearchDetail').serialize());
                        utility.ValidateFromToDate('frmChargeSearchDetail', 'dtpDOSFrom', 'dtpDOSTo', true);
                      
                    });
                    if (charge_detail[0].Status.toLowerCase() == "regular" || charge_detail[0].Status.toLowerCase() == "resubmit") {
                        chargeSearchDetail.ActiveControls(true);
                        self.find('#btnResubmit').addClass('disabled');
                        self.find("#btnSave").show();
                        //chargeSearchDetail.ValidateChargeDetail();
                     //   if(isFromLoad)
                     //       chargeSearchDetail.ValidateChargeDetail();
                    }
                    else if (charge_detail[0].Status.toLowerCase() == "submitted") {
                        chargeSearchDetail.ActiveControls(false);
                        self.find('#btnResubmit').removeClass('disabled');
                        self.find("#btnSave").hide();
                    }

                    //PMS-483, adnan maqbool
                    if (charge_detail[0].Status.toLowerCase() == "regular") {
                    
                        self.find('#txtSubmittedBy').val("");
                        self.find("#txtSubmitDate").val("");

                    }
                    //
                    
                    if (charge_detail[0].InsurancePlanName == "")
                        $('#' + chargeSearchDetail.params.PanelID + ' #txtINSCharges').prop('disabled', true);

                    //var TotalBalance = Number($('#' + chargeSearchDetail.params.PanelID + ' #txtTotalBalance').val());
                    //if(TotalBalance != 0 )
                    //{
                    //    $('#' + chargeSearchDetail.params.PanelID + ' #txtCPT').prop('disabled', true);
                    //}
                    chargeSearchDetail.TransferedChargesSearch();

                    chargeSearchDetail.LockCharge();


                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
                $('#frmChargeSearchDetail').data('serialize', $('#frmChargeSearchDetail').serialize());
            });
            chargeSearchDetail.ChargeLedgerBalance();
            // }
            //    else
            //        utility.DisplayMessages(strMessage, 2);
            //});
        }

    },

    FillChargePOS: function (facilityHiddenCtrl, POSHiddenCtrl) {
        var facilityId = $('#' + chargeSearchDetail.params["PanelID"] + ' #frmChargeSearchDetail #' + facilityHiddenCtrl).val();
        if (facilityId > 0) {
            Admin_Facility.SearchFacility(null, facilityId).done(function (response) {
                if (response.status != false) {

                    if (response.FacilityCount > 0) {
                        var FacilityLoadJSONData = JSON.parse(response.FacilityLoad_JSON)[0];

                        if (POSHiddenCtrl) {
                            $('#' + chargeSearchDetail.params["PanelID"] + ' #frmChargeSearchDetail #' + POSHiddenCtrl).val(FacilityLoadJSONData.POS);
                        }
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    OpenElectronicEOB: function () {

        var params = [];
        params["ChargeId"] = chargeSearchDetail.params.ChargeCapId;
        params["PanelID"] = chargeSearchDetail.params.PanelID;
        params["ParentCtrl"] = 'chargeSearchDetail';
        LoadActionPan('Bill_ERA_ElectronicEOB', params);

    },

    AutoCompleteCPT: function (obj) {
        //if (globalAppdata['IMO_ID'] == "") {
        //    //CacheManager.BindAutoCompleteText(obj, 'GetCPTCode', true, '#' + chargeSearchDetail.params["PanelID"] + ' #frmChargeSearchDetail #txtCPT', '');
        //}
        //else {
            //utility.BindAutoCompleteText(obj, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', '#' + chargeSearchDetail.params["PanelID"] + ' #frmChargeSearchDetail #txtCPT', '');
            utility.BindIMOAutoCompleteText(obj, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', '#' + chargeSearchDetail.params["PanelID"] + ' #frmChargeSearchDetail #txtCPT', null, true, -1, "CPT", true, "chargeSearchDetail", null, true);
        //}
    },

    AutoCompleteICD: function (obj) {
        //if (globalAppdata['IMO_ID'] == "") {
        //    //CacheManager.BindAutoCompleteText(obj, 'GetCPTCode', true, '#' + chargeSearchDetail.params["PanelID"] + ' #frmChargeSearchDetail #txtCPT', '');
        //}
        //else {
            //utility.BindAutoCompleteText(obj, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', '#' + chargeSearchDetail.params["PanelID"] + ' #frmChargeSearchDetail #txtCPT', '');
            utility.BindIMOAutoCompleteText(obj, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', '#' + chargeSearchDetail.params["PanelID"] + ' #frmChargeSearchDetail' + obj.id , null, true, -1, "ICD", false, "chargeSearchDetail", null, true);
        //}
    },

    FillCPTData: function (obj) {
        var CPTCode = $(obj).val();
        if (CPTCode != "") {
            //var j = { "txtShortName": "", "txtCPTCode": String(CPTCode), "txtDiscription": "", "lstTOSId": "", "lstSpeciality": "", "chkDiscontinued": true, "ddlEntity": "" };
            var j = { "txtShortName": "", "txtCPTCode": String(CPTCode), "txtDiscription": "", "lstTOSId": "", "lstSpeciality": "", "ddlEntity": "" };
            var myJSON = JSON.stringify(j);
            Admin_CPTCode.SearchCPTCode(myJSON, 0, 1, 15).done(function (response) {
                if (response.status != false) {
                    if (response.CPTCount > 0) {
                        var CPTDetails = JSON.parse(response.CPTLoad_JSON)[0];
                        $('#' + chargeSearchDetail.params["PanelID"] + ' #frmChargeSearchDetail input[id*="txtTOS"]').val(CPTDetails.TypeOfServiceCode);
                        $('#' + chargeSearchDetail.params["PanelID"] + ' #frmChargeSearchDetail input[id*="txtUnits"]').val(CPTDetails.BasicUnits);
                        $('#' + chargeSearchDetail.params["PanelID"] + ' #frmChargeSearchDetail input[id*="txtNDC"]').val(CPTDetails.NDC);
                        $('#' + chargeSearchDetail.params["PanelID"] + ' #frmChargeSearchDetail input[id*="txtNDCUnit"]').val(CPTDetails.NDCUnit);
                        $('#' + chargeSearchDetail.params["PanelID"] + ' #frmChargeSearchDetail input[id*="txtNDCUnitPrice"]').val(CPTDetails.NDCUnitPrice);
                        $('#' + chargeSearchDetail.params["PanelID"] + ' #frmChargeSearchDetail select[id*="ddlNDCMeasurement"]').val(CPTDetails.NDCMeasurementId).attr("selected", "selected");
                        $('#' + chargeSearchDetail.params["PanelID"] + ' #frmChargeSearchDetail input[id*="txtExpectedFee"]').val(CPTDetails.ExpectedFee);
                        $('#' + chargeSearchDetail.params["PanelID"] + ' #frmChargeSearchDetail input[id*="txtINSCharges"]').val('0.00');
                        $('#' + chargeSearchDetail.params["PanelID"] + ' #frmChargeSearchDetail input[id*="txtPATCharges"]').val('0.00');
                        
                        chargeSearchDetail.objChargeDetail.Fee = CPTDetails.Fee;
                        chargeSearchDetail.objChargeDetail.Units = CPTDetails.BasicUnits;
                        $('#' + chargeSearchDetail.params["PanelID"] + ' #frmChargeSearchDetail #txtFEE').val((Number(CPTDetails.Fee) * Number(CPTDetails.BasicUnits)).toFixed());
                        chargeSearchDetail.CalculateUnitFee($('#' + chargeSearchDetail.params["PanelID"] + ' #frmChargeSearchDetail #txtUnits'));
                    }
                    else {
                        //cptcount is less than zero
                        utility.DisplayMessages("Invalid CPT Code", 3);
                    }

                }
                else {
                    if (response.CPTCount == 0)
                        utility.DisplayMessages("Invalid CPT Code", 3);
                    else
                        utility.DisplayMessages(response.Message, 3);
                }
            });
        }

    },
    ValidateTOS: function (ev) {
        Admin_TypeOfService.SearchTypeOfService(null, $(ev).val()).done(function (response) {
            if (response.TypeOfServiceCount == 0) {
                utility.DisplayMessages("Invalid TOS", 3);
                $(ev).val('');
            }

        });
    },

    //This function is not working
    ValidatePOS: function (ev) {
        Admin_PlaceOfService.SearchPlaceOfService(null, $(ev).val()).done(function (response) {
            if (response.PlaceOfServiceCount == 0) {
                utility.DisplayMessages("Invalid POS", 3);
                $(ev).val('');
            }

        });
    },
    //

    LoadAllAutocomplete: function () {

        CacheManager.BindCodes('GetInsurancePlan', false).done(function (result) {

            $("#" + chargeSearchDetail.params.PanelID + " #frmChargeSearchDetail input#txtInsurancePlan").autocomplete({
                autoFocus: true,
                source: InsurancePlans, // pass an array (without a comma)
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#" + chargeSearchDetail.params.PanelID + " #chargeSearchDetail #hfInsurancePlan").val(ui.item.id); // add the selected id
                        if ($("#" + chargeSearchDetail.params.PanelID + " #chargeSearchDetail #lnkInsurancePlanDetail").css("display") == "none") {
                            $("#" + chargeSearchDetail.params.PanelID + " #chargeSearchDetail #lnkInsurancePlanDetail").css("display", "inline");
                            $("#" + chargeSearchDetail.params.PanelID + " #chargeSearchDetail #lblInsurancePlan").css("display", "none");
                        }
                    }, 100);
                }
            });
        });

        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            $("#" + chargeSearchDetail.params.PanelID + " #frmChargeSearchDetail #txtProvider").autocomplete({
                autoFocus: true,
                source: Providers, // pass an array
                select: function (event, ui) {

                    setTimeout(function () {
                        if ($("#" + chargeSearchDetail.params.PanelID + " #chargeSearchDetail #lnkProviderEdit").css("display") == "none") {
                            $("#" + chargeSearchDetail.params.PanelID + " #chargeSearchDetail #lnkProviderEdit").css("display", "");
                            $("#" + chargeSearchDetail.params.PanelID + " #chargeSearchDetail #lblProvider").css("display", "none");
                        }
                        $("#" + chargeSearchDetail.params.PanelID + " #chargeSearchDetail #txtProvider").attr("ProviderId", ui.item.id); // add the selected id
                        $("#" + chargeSearchDetail.params.PanelID + " #chargeSearchDetail #hfProvider").val(ui.item.id);
                    }, 100);
                }
            });
        });

        CacheManager.BindCodes('GetFacility', false).done(function (result) {
            $("#" + chargeSearchDetail.params.PanelID + " #frmChargeSearchDetail  input#txtFacility").autocomplete({
                autoFocus: true,
                source: Facilities, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#" + chargeSearchDetail.params.PanelID + " #chargeSearchDetail #hfFacility").val(ui.item.id); // add the selected id                        
                        if ($("#" + chargeSearchDetail.params.PanelID + " #chargeSearchDetail #lnkFacilityEdit").css("display") == "none") {
                            $("#" + chargeSearchDetail.params.PanelID + " #chargeSearchDetail #lnkFacilityEdit").css("display", "inline");
                            $("#" + chargeSearchDetail.params.PanelID + " #chargeSearchDetail #lblFacility").css("display", "none");
                        }
                    }, 100);
                }
            });
        });

        CacheManager.BindCodes('GetModifier', false).done(function (result) {
            $("#" + chargeSearchDetail.params.PanelID + " #frmChargeSearchDetail input#txtModifier").autocomplete({
                autoFocus: true,
                source: Modifiers, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#" + chargeSearchDetail.params.PanelID + " #chargeSearchDetail #hfModifier").val(ui.item.id); // add the selected id                        
                        if ($("#" + chargeSearchDetail.params.PanelID + " #chargeSearchDetail #lnkModifierEdit").css("display") == "none") {
                            $("#" + chargeSearchDetail.params.PanelID + " #chargeSearchDetail #lnkModifierEdit").css("display", "inline");
                            $("#" + chargeSearchDetail.params.PanelID + " #chargeSearchDetail #lblModifier").css("display", "none");
                        }
                    }, 100);
                }
            });
        });

    },


    LockCharge: function () {

        var isLocked = chargeSearchDetail.IsLocked;
        var ChargeStatus = chargeSearchDetail.ChargeStatus;

        if ((isLocked == null || isLocked == undefined || isLocked == "") && chargeSearchDetail.params["IsLocked"] != "")
            isLocked = chargeSearchDetail.params.IsLocked;

        if (isLocked != null)
        {
            if (isLocked == false && ChargeStatus.toLowerCase() == 'submitted')
                $("#" + chargeSearchDetail.params.PanelID + " #divAllControls").find("input:text,input:checkbox,input:radio,select,button,textarea").prop('disabled', true);
            else
            $("#" + chargeSearchDetail.params.PanelID + " #divAllControls").find("input:text,input:checkbox,input:radio,select,button,textarea").prop('disabled', isLocked);
        }
           

        if (isLocked == true) {
            $("#" + chargeSearchDetail.params.PanelID + " #divAllControls #btnSave").hide();
        }

        //disable some fields that are disabled by default
        if (isLocked == false) {
            $("#" + chargeSearchDetail.params.PanelID + " #divAllControls").find(
                "#txtStatus,#txtChargeNumber,#txtFacility,#txtProvider,#txtInsurancePlan,#dtpEOD,#txtCPT,#txtPrimaryFEE,#txtTotalFEE" +
                ",#txtInsBalance,#txtPatBalance,#txtCopayBalance,#txtInsPaid,#txtWriteoff,#txtPatientPaid,#txtPatientDiscount,#txtCopayPaid"+
                ",#txtCopayDiscount,#txtTotalBalance,#chkPrimary,#txtBatchNo,#txtSubmitDate,#txtSubmittedBy,#txtEntryDate,#txtEnteredBy,#txtICD1,#txtICD2,#txtICD3,#txtICD4"
                ).prop('disabled', true);
        }

        $("#" + chargeSearchDetail.params.PanelID + " #divAllControls").find("#txtTotalUnits").prop("disabled", true);

    },


    // -------------- Modifier ---------------------

    OpenModiferDetail: function () {
        var params = [];
        params["ModifierId"] = $("#" + chargeSearchDetail.params.PanelID + ' #chargeSearchDetail #hfModifier').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'chargeSearchDetail';
        params["RefCtrl"] = "txtModifier";
        LoadActionPan('modifierDetail', params);

    },

    HideModiferLink: function () {
        $("#" + chargeSearchDetail.params.PanelID + ' #chargeSearchDetail #txtModifer').attr("ModiferId", "-1");
        $("#" + chargeSearchDetail.params.PanelID + ' #chargeSearchDetail #hfModifer').val("-1");
        $("#" + chargeSearchDetail.params.PanelID + ' #chargeSearchDetail #lnkModifierEdit').css("display", "none");
        $("#" + chargeSearchDetail.params.PanelID + ' #chargeSearchDetail #lblModifier').css("display", "inline");
    },
    // --------------End Modifier ---------------------

    // -------------- Facility ---------------------

    OpenFacility: function () {
        var params = [];
        //params["IsOptional"] = false;
        // params["RefForm"] = "frmChargeSearchDetail";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "chargeSearchDetail";
        LoadActionPan('Admin_Facility', params);
    },

    OpenFacilityDetail: function () {
        //Admin_Facility.FacilityEdit($('#chargeSearchDetail #hfFacility').val(), 'patTabDemographic');
        var params = [];
        params["FacilityId"] = $("#" + chargeSearchDetail.params.PanelID + ' #frmChargeSearchDetail #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'chargeSearchDetail';
        params["RefCtrl"] = "txtFacility";
        LoadActionPan('facilityDetail', params);
    },

    HideFacilityLink: function () {
        $("#" + chargeSearchDetail.params.PanelID + ' #chargeSearchDetail #txtFacility').attr("FacilityId", "-1");
        $("#" + chargeSearchDetail.params.PanelID + ' #chargeSearchDetail #hfFacility').val("-1");
        $("#" + chargeSearchDetail.params.PanelID + ' #chargeSearchDetail #lnkFacilityEdit').css("display", "none");
        $("#" + chargeSearchDetail.params.PanelID + ' #chargeSearchDetail #lblFacility').css("display", "inline");
    },
    // -------------- End Facility -----------------
    // -------------- Provider ---------------------

    OpenProvider: function () {
        var params = [];
        //params["IsOptional"] = false;
        //params["RefForm"] = "frmChargeSearchDetail";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "chargeSearchDetail";
        LoadActionPan('Admin_Provider', params);
    },

    OpenProviderDetail: function () {
        //Admin_Provider.ProviderEdit($('#chargeSearchDetail #hfProvider').val(),'patTabDemographic');
        var params = [];
        params["ProviderId"] = $("#" + chargeSearchDetail.params.PanelID + ' #frmChargeSearchDetail #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'chargeSearchDetail';
        LoadActionPan('providerDetail', params);
    },

    HideProviderLink: function () {
        $("#" + chargeSearchDetail.params.PanelID + ' #chargeSearchDetail #txtProvider').attr("ProviderId", "-1");
        $("#" + chargeSearchDetail.params.PanelID + ' #chargeSearchDetail #hfProvider').val("-1");
        $("#" + chargeSearchDetail.params.PanelID + " #chargeSearchDetail #lnkProviderEdit").css("display", "none");
        $("#" + chargeSearchDetail.params.PanelID + " #chargeSearchDetail #lblProvider").css("display", "inline");
    },
    // -------------- End Provider -----------------

    // -------------- Insurance Plan ---------------------
    OpenInsurancePlanDetail: function () {
        var params = [];
        params["ProviderId"] = $("#" + chargeSearchDetail.params.PanelID + '  #chargeSearchDetail #hfInsurancePlan').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtInsurancePlan";
        params["ParentCtrl"] = 'chargeSearchDetail';
        LoadActionPan('insurancePlanDetail', params);
        //Admin_InsurancePlan.InsurancePlanEdit($("#chargeSearchDetail #hfInsurancePlan").val());
    },

    FillInsurancePlanName: function (InsurancePlanId, InsurancePlanName) {
        $("#" + chargeSearchDetail.params.PanelID + " #chargeSearchDetail #txtInsurancePlan").val(InsurancePlanName);
        $("#" + chargeSearchDetail.params.PanelID + " #chargeSearchDetail #hfInsurancePlan").val(InsurancePlanId);
        $("#" + chargeSearchDetail.params.PanelID + " #chargeSearchDetail #lnkInsurancePlanDetail").css("display", "inline");
        $("#" + chargeSearchDetail.params.PanelID + " #chargeSearchDetail #lblInsurancePlan").css("display", "none");
        UnloadActionPan(Admin_InsurancePlan.params["ParentCtrl"]);
    },

    HideInsurancePlanLink: function () {
        $("#" + chargeSearchDetail.params.PanelID + " #chargeSearchDetail #hfInsurancePlan").val("-1");
        $("#" + chargeSearchDetail.params.PanelID + " #chargeSearchDetail #lnkInsurancePlanDetail").css("display", "none");
        $("#" + chargeSearchDetail.params.PanelID + " #chargeSearchDetail #lblInsurancePlan").css("display", "inline");
        //$('#chargeSearchDetail #ddlPlanAddress').empty();
        //$('#chargeSearchDetail #ddlPlanAddress').append($("<option />").val("").text("- SELECT -"));
    },

    OpenInsurancePlan: function () {
        var params = [];
        //params["IsOptional"] = false;
        //params["RefForm"] = "frmChargeSearchDetail";
        params["InsurancePlanId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "chargeSearchDetail";
        LoadActionPan('Admin_InsurancePlan', params);
    },

    // --------------End Insurance Plan ---------------------


    FillChargeDetail: function (ChargeCapId) {
        var data = "ChargeCapId=" + ChargeCapId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "BILLING_CHARGEDETAIL", "FILL_CHARGE_DETAIL");
    },
    ChargeSave: function () {
        var self = $('#' + chargeSearchDetail.params.PanelID);
        var myJSON = self.getMyJSON();
        tempObj = JSON.parse(myJSON);
        //tempObj.txtFEE = (Number(tempObj.txtFEE) / Number(tempObj.txtUnits)).toString();
        myJSON = JSON.stringify(tempObj);
        //Update Charge.
        AppPrivileges.GetFormPrivileges("Charges", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                chargeSearchDetail.UpdateCharge(myJSON, chargeSearchDetail.params.ChargeCapId).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        chargeSearchDetail.LoadChargeDetail();
                        //$('#frmChargeSearchDetail').data('serialize', $('#frmChargeSearchDetail').serialize());
                        Bill_ChargeSearch.BillChargeSearch();
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
    UpdateCharge: function (ChargeData, ChargeId) {
        var data = "ChargeData=" + ChargeData + "&ChargeId=" + ChargeId + "&RefCtrl=Charge Detail";
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_CHARGE_CAPTURE", "UPDATE_CHARGE_CAPTURE");
    },
    ActiveControls: function (active) {
        if (!active) {
            $('#' + chargeSearchDetail.params.PanelID + ' #frmChargeSearchDetail').find('#dtpDOSFrom, #dtpDOSTo, #txtCPT, #txtUnits ,#txtModifier1 , #txtModifier2,#txtModifier3, #txtModifier4,#txtICD1,#txtICD2,#txtICD3,#txtICD4,#txtPOS,#txtFEE,#txtINSCharges,#txtPATCharges,#txtCOPAY,#chkEMG,#txtNDC,#txtNDCUnit,#txtNDCUnitPrice,#ddlNDCMeasurement,#txtHoldDays,#chkHold,#txtComments,#txtTOS,#txtServiceDescription').each(function () {
                $(this).prop('disabled', true);
            });
        }
        else {
            $('#' + chargeSearchDetail.params.PanelID + ' #frmChargeSearchDetail').find('#dtpDOSFrom, #dtpDOSTo, #txtCPT, #txtUnits ,#txtModifier1 , #txtModifier2,#txtModifier3, #txtModifier4,#txtICD1,#txtICD2,#txtICD3,#txtICD4,#txtPOS,#txtFEE,#txtINSCharges,#txtPATCharges,#txtCOPAY,#chkEMG,#txtNDC,#txtNDCUnit,#txtNDCUnitPrice,#ddlNDCMeasurement,#txtHoldDays,#chkHold,#txtComments,#txtTOS,#txtServiceDescription').each(function () {
                $(this).prop('disabled', false);
            });
        }
    },
    ResubmitCharge: function () {
        chargeSearchDetail.ChargeResubmit(chargeSearchDetail.objChargeDetail.VisitId, chargeSearchDetail.params.ChargeCapId).done(function (response) {
            if (response.status != false) {
                utility.DisplayMessages(response.Message, 1);
                //Edited by Azeem Raza Tayyab on 3-Mar-2016 to fix Bug:PMS-4247
                EncounterChargeCapture.params["IsReload"] = true;
                eval('EncounterChargeCapture.Load')(EncounterChargeCapture.params);
                chargeSearchDetail.ActiveControls(true);
                var self = $("#" + chargeSearchDetail.params.PanelID);
                self.find('#btnResubmit').addClass('disabled');
                self.find("#btnSave").show();
                //    chargeSearchDetail.ValidateChargeDetail();
                chargeSearchDetail.LoadChargeDetail(true);
                $('#frmChargeSearchDetail').data('serialize', $('#frmChargeSearchDetail').serialize());
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    ChargeResubmit: function ( VisitID, ChargeId) {
        var data = "VisitID=" + VisitID + "&ChargeId=" + ChargeId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "BILLING_CHARGEDETAIL", "RESUBMIT_CHARGE");
    },
    CalculateUnitFee: function (ev) {
        if (chargeSearchDetail.objChargeDetail != null) {
            // In this Screen PanelID is already attach with ParentCtrl Panel
            var self = $('#' + chargeSearchDetail.params.PanelID);
            //var feePerUnit = Number(chargeSearchDetail.objChargeDetail.Fee) / Number(chargeSearchDetail.objChargeDetail.Units);
            //var feePerUnit = Number(chargeSearchDetail.objChargeDetail.Fee);
            var feePerUnit = Number(self.find('#txtFEE').val());

            self.find('#txtTotalFEE').val((feePerUnit * Number($(ev).val())).toFixed(2));

            if (self.find('#txtInsurancePlan').val() == "") {
                var patientFee = (Number(self.find('#txtTotalFEE').val())).toFixed(2);
                self.find('#txtPATCharges').val(patientFee)
            }
            else {
                var assignBenifits = chargeSearchDetail.objChargeDetail.AssignBenefits.toLowerCase();
                var TotalBal = (Number(self.find('#txtTotalFEE').val())).toFixed(2);
                if (assignBenifits == "true") {
                    var PatientBal = Number(chargeSearchDetail.objChargeDetail.PatCharges);
                    var insuranceBal = TotalBal - PatientBal;
                    if(insuranceBal < 0)
                    {
                        self.find('#txtINSCharges').val(TotalBal);
                        self.find('#txtPATCharges').val('0.00');
                    }
                    else
                        self.find('#txtINSCharges').val(insuranceBal);
                }
                else {
                    self.find('#txtPATCharges').val(TotalBal);

                }
            }


        }
    },
    ValidateChargeDetail: function () {
        $('#' + chargeSearchDetail.params.PanelID + ' #frmChargeSearchDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   NDCUnit: {
                       group: '.col-sm-3',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   NDCUnitPrice: {
                       group: '.col-sm-3',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   
               }
           }).on('success.form.bv', function (e) {
                e.preventDefault();
                chargeSearchDetail.ChargeSave();
            });


    },
    NDCvalidation: function (ev) {
        var formValidation = $('#' + chargeSearchDetail.params.PanelID + ' #frmChargeSearchDetail').data("bootstrapValidator");
        if ($(ev).val() != "") {
            formValidation.enableFieldValidators('NDCUnit', true);
            formValidation.enableFieldValidators('NDCUnitPrice', true);
        }
        else
        {
            formValidation.enableFieldValidators('NDCUnit', false);
            formValidation.enableFieldValidators('NDCUnitPrice', false);
        }
        $('#' + chargeSearchDetail.params.PanelID + ' #frmChargeSearchDetail').bootstrapValidator('revalidateField', 'NDCUnit');
        $('#' + chargeSearchDetail.params.PanelID + ' #frmChargeSearchDetail').bootstrapValidator('revalidateField', 'NDCUnitPrice');
    },
    ChargeLedgerBalance: function () {
        chargeSearchDetail.GetLedgerBalanceOfCharge(chargeSearchDetail.params.ChargeCapId, 0, 0, 0).done(function (response) {
            if (response.status != false)
            {
                if (Number(response.Balance) != 0)
                {
                    $('#' + chargeSearchDetail.params.PanelID + ' #txtCPT').prop('disabled', true);
                }
            }
            else {

            }


        });
    },
    GetLedgerBalanceOfCharge: function (ChargeId, VisitId, AppointmentId, PaymentId) {
        var data = "ChargeId=" + ChargeId + "&VisitId=" + VisitId + "&AppointmentId=" + AppointmentId + "&PaymentId=" + PaymentId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "BILLING_CHARGEDETAIL", "GET_LEDGER_BALANCE");
    },

    TransferedChargesSearch: function () {
        chargeSearchDetail.SearchTransferedCharges(chargeSearchDetail.objChargeDetail.VisitId).done(function (response) {
            if (response.status != false) {
                chargeSearchDetail.TransferChargesGridLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    SearchTransferedCharges: function (VisitID) {
        var data = "VisitId=" + VisitID;
        return MDVisionService.defaultService(data, "BILLING_CHARGEDETAIL", "LOAD_TRANSFER_CHARGES");
    },
    TransferChargesGridLoad: function (response) {
        $('#' + chargeSearchDetail.params.PanelID + ' #dgvTransferCharges').dataTable().fnDestroy();
        $('#' + chargeSearchDetail.params.PanelID + ' #dgvTransferCharges tbody').find("tr").remove();

        if (response.TransferedChargesCount > 0) {
            var TransferChargesLoad_JSONData = JSON.parse(response.TransferedChargesLoad_JSON);
            $.each(TransferChargesLoad_JSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#dgvTransferCharges" + i + "'))");
                $row.attr("id", "dgvTransferCharges" + i);
                $row.attr("ChargeID", item.ClaimNumber);
                $row.append('<td>' + item.ClaimNumber + '</td><td>' + item.InsurancePlanName + '</td><td>' + utility.convertToFigure(item.Fee, true) + '</td><td >' + utility.convertToFigure(item.InsCharges, true) + '</td><td>' + utility.convertToFigure(item.InsBalance, true) + '</td><td>' + utility.convertToFigure(item.PatCharges, true) + '</td><td>' + utility.convertToFigure(item.PatBalance, true) + '</td><td>' + utility.convertToFigure(item.Copay, true) + '</td><td>' + utility.convertToFigure(item.TotalBal, true) + '</td><td>' + utility.RemoveTimeFromDate(null, item.SubmittedDate) + '</td>');
                $('#' + chargeSearchDetail.params.PanelID + ' #dgvTransferCharges tbody').last().append($row);

            });


        }



        else {
            $('#' + chargeSearchDetail.params.PanelID + ' #dgvTransferCharges').DataTable({
                "language": {
                    "emptyTable": "No Transfer Charges Found "
                }, "autoWidth": false, "bLengthChange": false, "bInfo": false, "bPaginate": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });


        }

        if ($.fn.dataTable.isDataTable('#' + chargeSearchDetail.params.PanelID + ' #dgvTransferCharges'));
        else
            $('#'+ chargeSearchDetail.params.PanelID +' #dgvTransferCharges').DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

    },

    ShowHistory: function (ParentControlId) {
        //var strMessage = "";
        //AppPrivileges.GetFormPrivileges("ERA Adjustment Codes", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        // if (strMessage == "") {
        var params = [];
        params["PanelID"] = chargeSearchDetail.params.PanelID;
        params["ChargeCapId"] = chargeSearchDetail.params["ChargeCapId"];
        params["PatientId"] = chargeSearchDetail.params["PatientId"];
        params["ParentCtrl"] = 'chargeSearchDetail';
        LoadActionPan('Activity_Log', params, ParentControlId);
        // }
        // else
        //    utility.DisplayMessages(strMessage, 2);
        // });
    },
    UnLoad: function () {

        
        if (chargeSearchDetail.params != null && chargeSearchDetail.params.ParentCtrl != null) {
            UnloadActionPan(chargeSearchDetail.params.ParentCtrl, 'chargeSearchDetail');
        }
        //else if (chargeSearchDetail.params.ParentCtrl == null) {
        //    UnloadActionPan("EncounterChargeCapture", 'chargeSearchDetail');
        //}
        else
            UnloadActionPan("EncounterChargeCapture", "chargeSearchDetail");
        //utility.UnLoadDialog(chargeSearchDetail.params.PanelID + ' #frmChargeSearchDetail', function () {
        //    if (chargeSearchDetail.params != null && chargeSearchDetail.params.ParentCtrl != null) {
        //        UnloadActionPan(chargeSearchDetail.params.ParentCtrl, 'chargeSearchDetail');
        //    }
        //    else
        //        UnloadActionPan();
        //}, function () {
        //    UnloadActionPan();
        //});

    },

}