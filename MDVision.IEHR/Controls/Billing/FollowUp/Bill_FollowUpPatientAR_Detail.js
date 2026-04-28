Bill_FollowUpPatientAR_Detail = {
    bIsFirstLoad: true,
    params: [],
    //bVisitFirst: true,
    Load: function (params) {
        Bill_FollowUpPatientAR_Detail.params = params;
        if (Bill_FollowUpPatientAR_Detail.params.PanelID != "pnlBillFollowUpPatientARDetail")
            Bill_FollowUpPatientAR_Detail.params.PanelID = Bill_FollowUpPatientAR_Detail.params.PanelID + ' #pnlBillFollowUpPatientARDetail';

        if (Bill_FollowUpPatientAR_Detail.bIsFirstLoad) {
            Bill_FollowUpPatientAR_Detail.bIsFirstLoad = false;

            $('#' + Bill_FollowUpPatientAR_Detail.params.PanelID).loadDropDowns(true).done(function () {
                Bill_FollowUpPatientAR_Detail.LoadAllControls();

                Bill_FollowUpPatientAR_Detail.LoadFollowUpPatientARDetail();
                Bill_FollowUpPatientAR_Detail.LoadARCharges(Bill_FollowUpPatientAR_Detail.params.VisitId);
                $('#' + Bill_FollowUpPatientAR_Detail.params.PanelID + ' #frmFollowUpPatientARDetail').data('serialize', $('#' + Bill_FollowUpPatientAR_Detail.params.PanelID + ' #frmFollowUpPatientARDetail').serialize());
            });

            Bill_FollowUpPatientAR_Detail.ValidateFollowUpPatientAR();
        }
    },


    ValidateFollowUpPatientAR: function () {
        $('#frmFollowUpPatientARDetail')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {

                  //GroupID: {
                  //    group: '.col-sm-3',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        },
                  //    }
                  //},


                  ActionID: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          }
                      }
                  },


                  ////Reason: {
                  ////    group: '.col-sm-3',
                  ////    validators: {
                  ////        notEmpty: {
                  ////            message: ''
                  ////        }
                  ////    }
                  ////},

                  //RemitanceCode: {
                  //    group: '.col-sm-3',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        }
                  //    }
                  //},


              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           Bill_FollowUpPatientAR_Detail.saveFollowUpPatientAR();
       });
    },



    saveFollowUpPatientAR: function (showMessage) {

        var strMessage = "";
        var self = $('#' + Bill_FollowUpPatientAR_Detail.params.PanelID);
        var myJSON = self.getMyJSONByName();
        if (Bill_FollowUpPatientAR_Detail.params.mode.toLowerCase() == "add") {
            AppPrivileges.GetFormPrivileges("Advance Payment", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    //
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else if (Bill_FollowUpPatientAR_Detail.params.mode.toLowerCase() == "edit") {
            AppPrivileges.GetFormPrivileges("Advance Payment", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Bill_FollowUpPatientAR_Detail.followUpPatientARUpdate(myJSON, Bill_FollowUpPatientAR_Detail.params.followUpPatientARID).done(function (response) {
                        if (response.status != false) {

                            if (showMessage != null && showMessage == false) {
                                ;
                            } else {

                                utility.DisplayMessages(response.message, 1);
                            }
                            //Bill_FollowUpPatientAR_Detail.SearchAdvancePayment();
                            // UnloadActionPan(Bill_FollowUpPatientAR_Detail.params["ParentCtrl"]);
                            Bill_FollowUpPatientAR.ARSearch("Patient");
                            $('#frmFollowUpPatientARDetail').data('serialize', $('#frmFollowUpPatientARDetail').serialize());
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
    },

    followUpPatientARUpdate: function (followUpPatientARData) {

        var objData = JSON.parse(followUpPatientARData);
        objData["FollowUpPatientARID"] = Bill_FollowUpPatientAR_Detail.params.followUpPatientARID;
        objData["GroupName"] = $("#" + Bill_FollowUpPatientAR_Detail.params["PanelID"] + " #ddlGroup option:selected").text();
        objData["ActionName"] = $("#" + Bill_FollowUpPatientAR_Detail.params["PanelID"] + " #ddlAction option:selected").text();
        objData["ReasonName"] = $("#" + Bill_FollowUpPatientAR_Detail.params["PanelID"] + " #ddlReason option:selected").text();
        objData["RemitanceCodeText"] = $("#" + Bill_FollowUpPatientAR_Detail.params["PanelID"] + " #ddlRemitanceCode option:selected").text();
        objData["CommandType"] = "update_followup_patient_ar";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "FollowUps", "FollowUpPatientARDetail");

    },

    BindClaimNumber: function () {
        var ClaimNumber = $('#' + Bill_FollowUpPatientAR_Detail.params.PanelID + ' #txtClaimno').val();
        var AllClaimsVisits = [];
        if (ClaimNumber.length > 2) {
            Bill_FollowUpPatientAR_Detail.LoadClaimNumers(ClaimNumber).done(function (responseData) {
                if (responseData.status != false) {
                    if (responseData.ClaimsCount > 0) {
                        var Claims = JSON.parse(responseData.ClaimsLoad_JSON);

                        $.each(Claims, function (i, item) {
                            //AllClaimsVisits.push({ id: item.VisitId, value: item.ClaimNumber + ' - ( ' + item.AccountNumber + ' - ' + item.PatientName + ' )', PatientId: item.PatientId, DOSFrom: item.DOSFrom, PatientName: item.AccountNumber + ' - ' + item.PatientName, ClaimNumber: item.ClaimNumber });
                            AllClaimsVisits.push({ id: item.VisitId, value: item.ClaimNumber, PatientId: item.PatientId, DOSFrom: item.DOSFrom, PatientName: item.AccountNumber + ' - ' + item.PatientName, ClaimNumber: item.ClaimNumber });
                        });
                        response(AllClaimsVisits);
                    }
                }
            });
        }
        $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #txtClaimno").autocomplete({
            autoFocus: true,
            source: AllClaimsVisits,
            select: function (event, ui) {
                setTimeout(function () {
                    $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #txtClaimno").val(ui.item.ClaimNumber);
                }, 100);

                //$("#hfpatientid").val(ui.item.id);
            }
        });
    },
    LoadClaimNumers: function (claimNumber) {
        var data = "ClaimNumber=" + claimNumber;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "BILLING_BATCHCHARGE_DETAIL", "SEARCH_VISIT_CLAIM");
    },

    PrintPatientLetter: function () {

        var params = [];
        params["FromAdmin"] = "0";
        params["ARDetailId"] = "-1";
        params["VisitId"] = Bill_FollowUpPatientAR_Detail.params.VisitId;
        params["ParentCtrl"] = 'Bill_FollowUpPatientAR_Detail';
        LoadActionPan('designLetterPrinting', params);
    },

    LoadPaymentPosting: function (ChargeCapId, VisitId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Payment Posting", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FromAdmin"] = 0;
                params["ParentCtrl"] = 'Bill_FollowUpPatientAR_Detail';
                params["ARDetailId"] = "-1";
                params["VisitId"] = Bill_FollowUpPatientAR_Detail.params.VisitId;
                params["ChargeId"] = ChargeCapId;
                params["PaymentRef"] = "pnlBillFollowUpPatientARDetail";

                LoadActionPan('Bill_PaymentPosting', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    OpenPatientARCall: function () {
        var params = [];
        params["ParentCtrl"] = "Bill_FollowUpPatientAR_Detail";
        params["FollowUpARID"] = Bill_FollowUpPatientAR_Detail.params.followUpPatientARID;
        params["FollowUpARType"] = "patient"

        params["VisitId"] = Bill_FollowUpPatientAR_Detail.params.VisitId;
        LoadActionPan('Bill_FollowUpARCall', params);

    },

    OpenPatientARHistory: function () {
        var params = [];
        params["ParentCtrl"] = "Bill_FollowUpPatientAR_Detail";
        params["VisitId"] = Bill_FollowUpPatientAR_Detail.params.VisitId;

        params["FollowUpARType"] = "patient"
        params["FollowUpARID"] = Bill_FollowUpPatientAR_Detail.params.followUpPatientARID;

        LoadActionPan('Bill_FollowUpARHistory', params);
    },

    OpenPatientARSplitClaim: function () {
        var params = [];
        params["ParentCtrl"] = "Bill_FollowUpPatientAR_Detail";
        params["ARDetailId"] = "-1";
        params["VisitId"] = Bill_FollowUpPatientAR_Detail.params.VisitId;
        LoadActionPan('Bill_FollowUpClaimSplit', params);

    },

    OpenPatientEligibility: function () {
        var params = [];
        params["FromAdmin"] = "0";

        params["patientID"] = Bill_FollowUpPatientAR_Detail.params.patientID;
        params["patientAccount"] = Bill_FollowUpPatientAR_Detail.params.patientAccount;
        params["patientLastName"] = Bill_FollowUpPatientAR_Detail.params.patientLastName;
        params["patientFirstName"] = Bill_FollowUpPatientAR_Detail.params.patientFirstName;
        params["patientInsurancePlanId"] = Bill_FollowUpPatientAR_Detail.params.patientInsurancePlanId;
        params["Provider"] = Bill_FollowUpPatientAR_Detail.params.Provider;
        params["ProviderId"] = Bill_FollowUpPatientAR_Detail.params.ProviderId;

        params["ParentCtrl"] = "Bill_FollowUpPatientAR_Detail";
        params["ARDetailId"] = "-1";
        params["VisitId"] = Bill_FollowUpPatientAR_Detail.params.VisitId;
        LoadActionPan('Patient_Eligibility', params);

    },
    OpenEncounter: function () {
        var params = [];
        params["FromAdmin"] = 0;
        params["ParentCtrl"] = 'Bill_FollowUpPatientAR_Detail';

        params["patientID"] = 0;

        LoadActionPan('Encounter_Visits', params);

        //if (Bill_FollowUpPatientAR_Detail.bVisitFirst) {
        //    $($('body #OpenVisits')[0]).attr('id', 'OpenVisits1')
        //    $($('body #CloseVisits')[0]).attr('id', 'CloseVisits1');
        //    Bill_FollowUpPatientAR_Detail.bVisitFirst = false;
        //}

    },
    FillClaimNumberFromSearch: function (ClaimNumber, AccountNumber, PatientName, PatientId, DOSFrom, VisitId) {
        $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #txtClaimno").val(ClaimNumber);
        //UnloadActionPan("billTabChargeSearch");
        Bill_FollowUpPatientAR_Detail.BindClaimNumber();
        Encounter_Visits.UnLoad();
    },

    OpenPatientSearch: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Bill_FollowUpPatientAR_Detail';
        LoadActionPan('Patient_Search', params);
    },
    FillPatientInfoFromSearch: function (PatientId, AccountNumber, patFullName) {
        $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #hfPatientId").val(PatientId);
        $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #txtPatientName").val(AccountNumber);
        UnloadActionPan("Bill_FollowUpPatientAR_Detail");
        utility.InsertRecentPatient(PatientId);
    },
    BindPatientAccount: function () {
        var AccountNo = $('#' + Bill_FollowUpPatientAR_Detail.params.PanelID + ' #txtPatientName').val();
        utility.GetPatientArray(AccountNo, 0).done(function (response) {

            $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #txtPatientName").autocomplete({
                autoFocus: true,
                source: response, // pass an array (without a comma)
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #txtPatientName").val(ui.item.AccountNumber);
                        $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #hfPatientId").val(ui.item.id);
                        utility.InsertRecentPatient(ui.item.id);
                    }, 100);


                }
            });
            $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #txtPatientName").autocomplete("search");
            $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #txtPatientName").focus();
        });

    },

    LoadAllControls: function () {
        CacheManager.BindCodes('GetFacility', false).done(function (result) {
            $("#frmFollowUpPatientARDetail #txtFacility").autocomplete({
                autoFocus: true,
                source: Facilities, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #hfFacility").val(ui.item.id); // add the selected id
                        $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #txtPractice").val(ui.item.Practice);
                        $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #hfPractice").val(ui.item.PracticeId);
                        if ($("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #lnkFacilityEdit").css("display") == "none") {
                            $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #lnkFacilityEdit").css("display", "inline");
                            $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #lblFacility").css("display", "none");
                        }
                    }, 100);
                }
            });
        });
        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            $("#frmFollowUpPatientARDetail #txtProvider").autocomplete({
                autoFocus: true,
                source: Providers, // pass an array
                select: function (event, ui) {

                    setTimeout(function () {
                        if ($("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #lnkProviderEdit").css("display") == "none") {
                            $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #lnkProviderEdit").css("display", "");
                            $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #lblProvider").css("display", "none");
                        }
                        $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #txtProvider").attr("ProviderId", ui.item.id); // add the selected id
                        $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #hfProvider").val(ui.item.id);
                    }, 100);
                }
            });
        });
        CacheManager.BindCodes('GetInsurancePlan', false).done(function (result) {

            $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " input#txtInsurancePlan").autocomplete({
                autoFocus: true,
                source: InsurancePlans, // pass an array (without a comma)
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #hfInsurancePlan").val(ui.item.id); // add the selected id
                        if ($("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #lnkInsurancePlanDetail").css("display") == "none") {
                            $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #lnkInsurancePlanDetail").css("display", "inline");
                            $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #lblInsurancePlan").css("display", "none");
                        }
                    }, 100);
                }
            });
        });
        CacheManager.BindCodes('GetPractice', false).done(function (result) {
            $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #txtPractice").autocomplete({
                autoFocus: true,
                source: Practices, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #hfPractice").val(ui.item.id); // add the selected id
                        if ($("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #lnkPracticeEdit").css("display") == "none") {
                            $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #lnkPracticeEdit").css("display", "inline");
                            $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #lblPractice").css("display", "none");
                        }
                    }, 100);
                }
            });
        });
        utility.CreateDatePicker(Bill_FollowUpPatientAR_Detail.params.PanelID + ' #frmFollowUpPatientARDetail #dtpSuspendedDate,#dtpDOB,#dtpEligibilityDate,#dtpFirstStatementDate,#dtpStatementDate',
            function (ev) { }, true);
    },

    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmFollowUpPatientARDetail";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Bill_FollowUpPatientAR_Detail";
        LoadActionPan('Admin_Facility', params);
    },
    OpenFacilityDetail: function () {
        var params = [];
        params["FacilityId"] = $('#' + Bill_FollowUpPatientAR_Detail.params.PanelID + ' #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Bill_FollowUpPatientAR_Detail';
        params["RefCtrl"] = "txtFacility";
        LoadActionPan('facilityDetail', params);
    },

    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmFollowUpPatientARDetail";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Bill_FollowUpPatientAR_Detail";
        LoadActionPan('Admin_Provider', params);
    },
    OpenProviderDetail: function () {
        var params = [];
        params["ProviderId"] = $('#' + Bill_FollowUpPatientAR_Detail.params.PanelID + ' #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'Bill_FollowUpPatientAR_Detail';
        LoadActionPan('providerDetail', params);
    },


    LoadVisitDetail: function () {
        var strMessage = "";

        AppPrivileges.GetFormPrivileges("Charges", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FromAdmin"] = 0;
                params["ParentCtrl"] = 'Bill_FollowUpPatientAR_Detail';

                params["VisitId"] = Bill_FollowUpPatientAR_Detail.params.VisitId;
                params["patientID"] = Bill_FollowUpPatientAR_Detail.params.patientID;

                LoadActionPan('EncounterChargeCapture', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },


    OpenInsurancePlan: function () {
        var params = [];
        params["InsurancePlanId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Bill_FollowUpPatientAR_Detail';
        LoadActionPan('Admin_InsurancePlan', params);
    },
    OpenInsurancePlanDetail: function () {

        var params = [];
        params["InsurancePlanId"] = $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #hfInsurancePlan").val();
        params["mode"] = "Edit";
        Admin_InsurancePlan.params["FromAdmin"] == "0";
        params["ParentCtrl"] = 'Bill_FollowUpPatientAR_Detail';
        LoadActionPan('insurancePlanDetail', params);
    },
    FillInsurancePlanName: function (InsurancePlanId, InsurancePlanName) {
        $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #txtInsurancePlan").val(InsurancePlanName);
        $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #hfInsurancePlan").val(InsurancePlanId);
        $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #lnkInsurancePlanDetail").css("display", "inline");
        $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #lblInsurancePlan").css("display", "none");
        UnloadActionPan(Admin_InsurancePlan.params["ParentCtrl"]);

    },


    OpenPractice: function () {
        var params = [];
        params["PracticeId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Bill_FollowUpPatientAR_Detail";
        LoadActionPan('Admin_Practice', params);
    },
    OpenPracticeDetail: function () {
        var params = [];
        params["PracticeId"] = $('#' + Bill_FollowUpPatientAR_Detail.params.PanelID + ' #hfPractice').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtPractice";
        params["ParentCtrl"] = 'Bill_FollowUpPatientAR_Detail';
        LoadActionPan('practiceDetail', params);
    },

    OpenPatientDemographics: function () {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Demographic", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                var params = [];
                params["mode"] = 'Edit';
                params["patientID"] = $('#' + Bill_FollowUpPatientAR_Detail.params.PanelID + ' #hfPatientId').val();
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = "Bill_FollowUpPatientAR_Detail";
                params["PatBanner"] = true;
                params["IsFill"] = false;
                LoadActionPan('demographicDetail', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },




    UnLoad: function () {

        utility.UnLoadDialog(Bill_FollowUpPatientAR_Detail.params.PanelID + ' #frmFollowUpPatientARDetail', function () {
            UnloadActionPan(Bill_FollowUpPatientAR_Detail.params["ParentCtrl"], "Bill_FollowUpPatientAR_Detail");
        }, function () {
            UnloadActionPan(Bill_FollowUpPatientAR_Detail.params["ParentCtrl"], "Bill_FollowUpPatientAR_Detail");
        });

    },



    /*****AR DETAILS******/

    LoadFollowUpPatientARDetail: function () {


        if (Bill_FollowUpPatientAR_Detail.params.mode.toLowerCase() == "add") {

            //serialize Data.
            $('#frmFollowUpPatientARDetail').data('serialize', $('#frmFollowUpPatientARDetail').serialize());

        }
        else if (Bill_FollowUpPatientAR_Detail.params.mode.toLowerCase() == "edit") {



            Bill_FollowUpPatientAR_Detail.FillFollowUpPatientARDetail(Bill_FollowUpPatientAR_Detail.params.followUpPatientARID).done(function (response) {

                if (response.status != false) {

                    var self = $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID);
                    var FollowUpPatientARDetailFill_JSON = JSON.parse(response.FollowUpPatientARDetailFill_JSON);
                    utility.bindMyJSONByName(true, FollowUpPatientARDetailFill_JSON, false, self).done(function () {



                        //serialize Data.
                        $('#frmFollowUpPatientARDetail').data('serialize', $('#frmFollowUpPatientARDetail').serialize());

                    });

                }
                else {
                    UnloadActionPan();
                    utility.DisplayMessages(response.Message, 3);
                }



                Bill_FollowUpPatientAR_Detail.enableEditLinks();

            });

        }
    },

    FillFollowUpPatientARDetail: function (FollowUpPatientARID) {

        var objData = new Object();
        objData["FollowUpPatientARID"] = FollowUpPatientARID;
        objData["CommandType"] = "fill_followup_patient_ar";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "FollowUps", "FollowUpPatientARDetail");

    },

    /*****AR DETAILS END******/

    /*****CHARGES******/

    LoadARCharges: function (VisitId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Encounter", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($('#' + Bill_FollowUpPatientAR_Detail.params.PanelID + " #pnlBillFollowUpPatientARDetail_Result").css("display") == "none") {
                    $('#' + Bill_FollowUpPatientAR_Detail.params.PanelID + " #pnlBillFollowUpPatientARDetail_Result").show();
                }

                //var self = $("#" + Bill_PaymentPosting.params["PanelID"]);
                //var myJSON = self.getMyJSON();

                Bill_FollowUpPatientAR_Detail.SearchCharges("", VisitId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        if (response.ChargeCount > 0) {

                            //------------Pagination-----------
                            $("#" + Bill_FollowUpPatientAR_Detail.params["PanelID"] + " #divBillFollowUpPatientARDetailPaging").css("display", "inline");
                            //Showing 1 to 15 of 15 entries
                            var RecordsPerPage = rpp != null ? rpp : 5;
                            var CurrentPage = PageNo != null ? PageNo : 1;

                            //params["myJSON"] = myJSON;


                            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                            //if (PageNo == null) {
                            utility.GetCustomPaging("divBillFollowUpPatientARDetailPaging", response.iTotalDisplayRecords, 5, "Bill_FollowUpPatientAR_Detail", CurrentPage, RecordsPerPage);
                            //}
                            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                            $("#" + Bill_FollowUpPatientAR_Detail.params["PanelID"] + " #divBillFollowUpPatientARDetailPaging #divShowingEntries").text(showingText);
                            // Change Background Color to Black for selected page
                            $("#" + Bill_FollowUpPatientAR_Detail.params["PanelID"] + " li").each(function () {
                                if ($(this).text() == CurrentPage) {
                                    $(this).attr("class", "active");
                                }
                                else

                                    $(this).removeAttr("class");
                            });
                            //------------End Pagination-------
                        }
                        else {
                            $("#" + Bill_FollowUpPatientAR_Detail.params["PanelID"] + " #divBillFollowUpPatientARDetailPaging").css("display", "none");
                        }
                        Bill_FollowUpPatientAR_Detail.ChargesGridLoad(response);
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

    ChargesGridLoad: function (response) {
       
        $("#" + Bill_FollowUpPatientAR_Detail.params["PanelID"] + " #pnlBillFollowUpPatientARDetail_Result #dgvBillFollowUpPatientARDetail").dataTable().fnDestroy();
        $("#" + Bill_FollowUpPatientAR_Detail.params["PanelID"] + " #pnlBillFollowUpPatientARDetail_Result #dgvBillFollowUpPatientARDetail tbody").find("tr").remove();
        if (response.ChargeCount > 0) {//response.ChargeCount
            var ChargeLoadJSONData = JSON.parse(response.ChargeLoad_JSON);
            $.each(ChargeLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvBillFollowUpPatientARDetail_row" + item.ChargeCapId + "'));");
                $row.attr("id", "gvBillFollowUpPatientARDetail_row" + item.ChargeCapId);
                $row.attr("ChargeId", item.ChargeCapId);

                //if (item.IsActive == "True") {
                //    isactive = 0;
                //    activeRecord = "Active Record";
                //    tglclass = "fa fa-toggle-on green";
                //}
                //else {
                //    isactive = 1;
                //    activeRecord = "Inactive Record";
                //    tglclass = "fa fa-toggle-on red";
                //}

                //var MethodMode = "";
                //var ActionBit = false;

                //var EditMethod = "Bill_PaymentPosting.ChargePaymentAdd(" + item.ChargeCapId.trim() + ",'Add','" + item.PatientId + "','" + item.PatientPatientId + "','" + item.PatientPlanName + "','" + item.VisitId + "','" + item.FacilityId + "','" + item.FacilityName + "','" + item.ProviderId + "','" + item.ProviderName + "','" + item.TotalBal + "','" + item.InsCharges + "','" + item.PatCharges + "','" + item.Copay + "','" + item.InsBalance + "','" + item.PatBalance + "','" + item.CopayBalance + "','" + item.Fee + "');";

                //var ActiveInacvtiveMethod = "";//"Patient_Search.ActiveInactivePatient(" + item.PatientId.trim() + "," + isactive + ");";
                //var strAction = "";
                //strAction = '<a class="btn btn-xs" href="#sectionChargeDetail" onclick="' + EditMethod + '"  title="Edit Record"><i class="fa fa-edit black"></i></a><a class="btn  btn-xs mr-xs" href="#sectionChargeDetail" onclick="' + ActiveInacvtiveMethod + '" title="' + activeRecord + '"><i class="' + tglclass + '"></i></a>';

                // $row.append('<td style="display:none;">' + item.ChargeCapId + '</td><td>' + strAction + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td><td>' + item.ClaimNumber + '</td><td>' + item.CPTCode + '</td><td>' + item.PatientPlanName + '</td><td>' + item.Fee + '</td><td>' + item.TotalBal + '</td><td>0</td><td>0</td><td>0</td><td>' + item.PatChargeAmt + '</td><td>0</td><td>0</td><td>0</td><td>' + item.Copay + '</td><td>0</td><td>0</td><td>0</td><td></td>');
                //start syed zia, bug #PMS-1741
                $row.append('<td style="display:none;">' + item.ChargeCapId + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td>' + '<td><a href="#" onclick="Bill_FollowUpPatientAR_Detail.LoadVisitDetail();"  title="View Claim Detail">' + item.ClaimNumber + '</a></td>' + '<td title="' + item.CPTDescription + '" data-toggle="tooltip" data-placement="right">' + item.CPTCode + '</td><td>' + item.InsurancePlanName + '</td><td class="text-right">' + utility.convertToFigure(item.Fee, true) + '</td><td>' + utility.convertToFigure(item.Units) + '</td><td class="text-right">' + utility.convertToFigure(parseFloat(parseFloat(item.Units) * parseFloat(item.Fee)), true) + '</td><td class="text-right">' + utility.convertToFigure(item.TotalBal, true) + '</td><td class="text-right">' + utility.convertToFigure(item.InsCharges, true) + '</td><td class="text-right">' + utility.convertToFigure(item.InsPaid, true) + '</td><td class="text-right">' + utility.convertToFigure(item.InsWriteOff, true) + '</td><td class="text-right">' + utility.convertToFigure(item.InsBalance, true) + '</td><td class="text-right">' + utility.convertToFigure(item.PatCharges, true) + '</td><td class="text-right">' + utility.convertToFigure(item.PatPaid, true) + '</td><td class="text-right">' + utility.convertToFigure(item.PatDiscount, true) + '</td><td class="text-right">' + utility.convertToFigure(item.PatBalance, true) + '</td><td class="text-right">' + utility.convertToFigure(item.Copay, true) + '</td><td class="text-right">' + utility.convertToFigure(item.CopayPaid, true) + '</td><td class="text-right">' + utility.convertToFigure(item.CopayDiscount, true) + '</td><td class="text-right">' + utility.convertToFigure(item.CopayBalance, true) + '</td>');
                $("#" + Bill_FollowUpPatientAR_Detail.params["PanelID"] + " #pnlBillFollowUpPatientARDetail_Result #dgvBillFollowUpPatientARDetail tbody").last().append($row);
            });

            $('[data-toggle=tooltip],[rel=tooltip]').tooltip({
            container: 'body'
        });

             //end syed zia, bug #PMS-1741
        }
        else {
            $("#" + Bill_FollowUpPatientAR_Detail.params["PanelID"] + " #divBillFollowUpPatientARDetailPaging").css("display", "none");
            $("#" + Bill_FollowUpPatientAR_Detail.params["PanelID"] + " #pnlBillFollowUpPatientARDetail_Result #dgvBillFollowUpPatientARDetail").DataTable({
                "language": {
                    "emptyTable": "No Charges Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Bill_FollowUpPatientAR_Detail.params["PanelID"] + " #pnlBillFollowUpPatientARDetail_Result #dgvBillFollowUpPatientARDetail"))
            ;
        else
            $("#" + Bill_FollowUpPatientAR_Detail.params["PanelID"] + " #pnlBillFollowUpPatientARDetail_Result #dgvBillFollowUpPatientARDetail").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "iDisplayLength": 5, "bFilter": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

        //if (Bill_FollowUpPatientAR_Detail.params.ChargeId != null) {
        //    $("#" + Bill_FollowUpPatientAR_Detail.params["PanelID"] + " #pnlBillFollowUpPatientARDetail_Result #dgvBillFollowUpPatientARDetail #gvBillFollowUpPatientARDetail_row" + Bill_FollowUpPatientAR_Detail.params.ChargeId).trigger("click");
        //}
        //else if (RowId != null) {
        //    $("#" + Bill_FollowUpPatientAR_Detail.params["PanelID"] + " #pnlBillFollowUpPatientARDetail_Result #dgvBillFollowUpPatientARDetail tbody tr#" + RowId).trigger("click");
        //}
        //else
        //    $("#" + Bill_FollowUpPatientAR_Detail.params["PanelID"] + " #pnlBillFollowUpPatientARDetail_Result #dgvBillFollowUpPatientARDetail tbody tr:eq(0) ").click();




    },

    SearchCharges: function (ChargeData, VisitId, PageNumber, RowsPerPage) {

        var objData = new JSON.constructor();
        if (ChargeData) {
           objData = JSON.parse(ChargeData);
        }
        if (VisitId == null) {
            VisitId = 0;
        }

        var ChargeId = 0;
        //if (ChargeId == null) {
        //    ChargeId = 0;
        //}
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 5;
        }


        Bill_FollowUpPatientAR_Detail.params.CurrentPageNo = PageNumber;

        objData["ChargeId"] = ChargeId;
        objData["VisitId"] = VisitId;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        objData["CommandType"] = "search";
            var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Payments", "PaymentPosting");
    },



    //---------------Pagination functions-----
    SelectedPageClick: function (PageNo, objPage) {
        // Change Background Color to Black for selected page
        $("#pnlBillFollowUpPatientARDetail_Result li").each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });

        if (Bill_FollowUpPatientAR_Detail.params.VisitId != null)
            Bill_FollowUpPatientAR_Detail.LoadARCharges(Bill_FollowUpPatientAR_Detail.params.VisitId, PageNo, 5);
        else
            Bill_FollowUpPatientAR_Detail.LoadARCharges(0, PageNo, 5);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#pnlBillFollowUpPatientARDetail_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {

            if (Bill_FollowUpPatientAR_Detail.params.VisitId != null)
                Bill_FollowUpPatientAR_Detail.LoadARCharges(Bill_FollowUpPatientAR_Detail.params.VisitId, currentPageNo, 5);
            else
                Bill_FollowUpPatientAR_Detail.LoadARCharges(0, currentPageNo, 5);


        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#pnlBillFollowUpPatientARDetail_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });
        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo > 0) {


            if (Bill_FollowUpPatientAR_Detail.params.VisitId != null)
                Bill_FollowUpPatientAR_Detail.LoadARCharges(Bill_FollowUpPatientAR_Detail.params.VisitId, currentPageNo, 5);
            else
                Bill_FollowUpPatientAR_Detail.LoadARCharges(0, currentPageNo, 5);

        }
    },


    /*****CHARGES END******/



    appendCallComments: function (callComments) {


        var objComments = $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #frmFollowUpPatientARDetail #txtComments");

        var seperator = "";

        if (objComments.val().trim() != "") {

            seperator = " | "
        }

        var callData = seperator + "Call: " + globalAppdata.AppUserName + " " + $.datepicker.formatDate(globalAppdata['DateFormat'].replace('yy', ''), new Date()) + "  " + callComments;

        objComments.val(objComments.val() + callData);

        Bill_FollowUpPatientAR_Detail.saveFollowUpPatientAR(false);

    },


    enableEditLinks: function () {


        var self = $("#" + Bill_FollowUpPatientAR_Detail.params.PanelID + " #frmFollowUpPatientARDetail");


        if (self.find("#hfProvider").val() != "") {
            self.find("#lnkProviderEdit").css("display", "inline");
            self.find("#lblProvider").css("display", "none");
        }

        if (self.find("#hfFacility").val() != "") {
            self.find("#lnkFacilityEdit").css("display", "inline");
            self.find("#lblFacility").css("display", "none");
        }


        if (self.find("#hfPractice").val() != "") {
            self.find("#lnkPracticeEdit").css("display", "inline");
            self.find("#lblPractice").css("display", "none");
        }

        if (self.find("#hfPatientId").val() != "") {
            self.find("#lnkPatientEdit").css("display", "inline");
            self.find("#lblPatient").css("display", "none");
        }


        if (self.find("#hfVisitId").val() != "") {
            self.find("#lnkVisitEdit").css("display", "inline");
            self.find("#lblVisit").css("display", "none");
        }



    },

}