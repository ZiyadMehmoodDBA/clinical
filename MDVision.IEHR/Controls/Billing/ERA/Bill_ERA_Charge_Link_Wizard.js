Bill_ERA_Charge_Link_Wizard = {
    params: [],
    bIsFirstLoad: true,
    Load: function (params) {

        Bill_ERA_Charge_Link_Wizard.params = params;
        if (Bill_ERA_Charge_Link_Wizard.bIsFirstLoad) {
            Bill_ERA_Charge_Link_Wizard.bIsFirstLoad = false;
            Bill_ERA_Charge_Link_Wizard.ValidatePaymentPosting();

            Patient_Insurance.SearchPatientInsurance(Bill_ERA_Charge_Link_Wizard.params.PatientID).done(function (response) {

                if (response.status != false) {
                    var PatientInsuranceJSONData = JSON.parse(response.PatientInsuranceLoad_JSON);
                    $.each(PatientInsuranceJSONData, function (i, item) {

                        var $Option = $('<option />', {
                            value: item.InsuranceId,
                            html: item.InsurancePlanName,
                            priority: item.PlanPriority,
                            insurancePlanId: item.InsurancePlanId
                        });

                        if (item.IsActive != "True") {
                            $Option.css("color", "red");
                        }
                        $("#frmERAChargeLinkWizard #ddlPaymentInsurance").append($Option);

                    });
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

            //CacheManager.BindPatientData('GetPatientInsurance', true, Bill_ERA_Charge_Link_Wizard.params.PatientID, 1).done(function (result) {

            //    $("#frmERAChargeLinkWizard #ddlPaymentInsurance").empty();
            //    $("#frmERAChargeLinkWizard #ddlPaymentInsurance").append($('<option />', { html: "- Select -", value: "" }));

            //    $.each(result, function (i, item) {
            //        if (item.RefValue != "") {
            //            $("#frmERAChargeLinkWizard #ddlPaymentInsurance").append(
            //            $('<option />', {
            //                value: item.Value,
            //                html: item.Name,
            //                priority: item.RefValue,
            //                insurancePlanId: item.ExValue
            //            }));
            //        }
            //    });
            //});
        }

    },

    ValidatePaymentPosting: function () {

        $('#' + Bill_ERA_Charge_Link_Wizard.params.PanelID + ' #frmERAChargeLinkWizard')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {

                   PaymentInsurance: {
                       group: '.col-sm-6',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },

               }
           }).on('success.form.bv', function (e) {
               e.preventDefault();
               Bill_ERA_Charge_Link_Wizard.LinkERACharge();
           });
    },

    LinkERACharge: function () {

        AppPrivileges.GetFormPrivileges("ERA", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                var PaymentInsuranceID = $("#frmERAChargeLinkWizard #ddlPaymentInsurance").val();

                ERADetail.ERALink(Bill_ERA_Charge_Link_Wizard.params.ERADtlID,
                                  Bill_ERA_Charge_Link_Wizard.params.ChargeID,
                                  Bill_ERA_Charge_Link_Wizard.params.IsLink, PaymentInsuranceID).done(function (response) {

                                      if (response.status != false) {

                                          utility.DisplayMessages(response.Message, 1);
                                          Bill_ERA_Charge_Link_Wizard.UnLoad();
                                          ERA_ChargeSearch.UnLoad();

                                          if (Bill_ERA_Charge_Link_Wizard.params.ScreenName == "ERAChargeDetail") {
                                              Bill_ERACharge_Detail.ERACHargeDetailLoad();  
                                          }
                                          else if (Bill_ERA_Charge_Link_Wizard.params.ScreenName == "ERADetail") {
                                              //Load Grid again.
                                              Bill_ERA.ERASearch();
                                              ERADetail.LoadERADetail();
                                          }

                                         
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

    UnLoad: function () {
        UnloadActionPan(Bill_ERA_Charge_Link_Wizard.params["ParentCtrl"], "Bill_ERA_Charge_Link_Wizard");
    },
}