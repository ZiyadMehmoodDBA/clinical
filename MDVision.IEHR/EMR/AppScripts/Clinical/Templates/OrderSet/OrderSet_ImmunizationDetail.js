OrderSet_ImmunizationDetail = {
    bIsFirstLoad: true,
    params: [],

    AdministerText: "ADMINISTER",
    DocumentHxDoseText: "DOCUMENTHX",
    REFUSALText: "REFUSAL",

    LookupVaccineMethod: "GetAdministerVaccine_Vaccine",
    LookupVaccineManufacturerMethod: "GetVaccineManufacturer",
    LookupVaccineLotNumber: "GetVaccineLotNumber",

    ddlAdministerVaccine_LotNumber: "ddlAdministerVaccine_LotNumber",

    ddlAdministerVaccine_Category: "ddlAdministerVaccine_Category",
    ddlDocumentHxDose_Category: "ddlDocumentHxDose_Category",
    ddlRecordRefusal_Category: "ddlRecordRefusal_Category",

    ddlAdministerVaccine_Vaccine: "ddlAdministerVaccine_Vaccine",
    ddlDocumentHxDose_Vaccine: "ddlDocumentHxDose_Vaccine",

    ddlAdministerVaccine_Manufacturer: "ddlAdministerVaccine_Manufacturer",
    ddlDocumentHxDose_Manufacturer: "ddlDocumentHxDose_Manufacturer",

    dpAdministerVaccine_VISDate: "dpAdministerVaccine_VISDate",

    LookupFunctionForGetLotNumbers: "GetVaccineLotNumber",
    ddlAdministerVaccine_LotNumber: "ddlAdministerVaccine_LotNumber",



    Load: function (params) {
        OrderSet_ImmunizationDetail.params = params;
        if (OrderSet_ImmunizationDetail.params.PanelID != 'pnlOrderSetImmunizationDetail') {
            OrderSet_ImmunizationDetail.params.PanelID = OrderSet_ImmunizationDetail.params.PanelID + ' #pnlOrderSetImmunizationDetail';
        } else {
            OrderSet_ImmunizationDetail.params.PanelID = 'pnlOrderSetImmunizationDetail';
        }
        var self = $('#' + OrderSet_ImmunizationDetail.params.PanelID);
        self.loadDropDowns(true).done(function () {
            OrderSet_ImmunizationDetail.CreatingDatePickers();
            OrderSet_ImmunizationDetail.CreatingTimePickers();
            if (OrderSet_ImmunizationDetail.params.mode == "EDIT") {
                $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #ddlAgeGroup').attr("disabled", true);
                $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #ddlSchedule').attr("disabled", true);
                $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #ddlCategory').attr("disabled", true);
                $.when(responseGetAgeLimScheCategAgainstVaccShedId = OrderSet_ImmunizationDetail.GetAgeLimScheCategAgainstVaccShedId(OrderSet_ImmunizationDetail.params.VaccineScheduleId)).then(function () {
                    if (responseGetAgeLimScheCategAgainstVaccShedId.response != 0) {
                        $.when(OrderSet_ImmunizationDetail.IntializeAllTabs()).then(function () {
                            var vaccineHxId = OrderSet_ImmunizationDetail.params.VaccineHxId;
                            if ($("#" + OrderSet_ImmunizationDetail.params.PanelID + " #VaccineTabsDiv").hasClass("disableAll")) {
                                $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #VaccineTabsDiv").removeClass("disableAll")
                            }



                            if (vaccineHxId > 0) {
                                OrderSet_ImmunizationDetail.ActiveOneTab();
                                OrderSet_ImmunizationDetail.searchVaccineHx(vaccineHxId).done(function (response) {
                                    OrderSet_ImmunizationDetail.bindVaccineHxDetails(response);
                                });
                            }
                        });

                    }

                });

            }
        });
        if (OrderSet_ImmunizationDetail.bIsFirstLoad) {
            OrderSet_ImmunizationDetail.ValidateDoc();
            OrderSet_ImmunizationDetail.ValidateAdmini();
            OrderSet_ImmunizationDetail.ValidateRefusalForm();
            OrderSet_ImmunizationDetail.bIsFirstLoad = false;

        }
    },

    bindVaccineHxDetails: function (response) {

        var selfAdminForm = $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail");
        var selfDocumentForm = $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail");
        var selfRefusalForm = $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail");
        response = JSON.parse(response);
        if (response.status != false) {

            // Bind with Admin tab

            if (OrderSet_ImmunizationDetail.params.Type.trim() == "ADMINISTER") {

                var details = JSON.parse(response.AdminVaccineHxLoad_JSON)[0];
                //OrderSet_ImmunizationDetail.SelectMainFields(details);
                //$('#' + OrderSet_ImmunizationDetail.params.PanelID + " #hfVaccineHx").val(details.VaccineHxId);
                utility.bindMyJSONByName(true, details, false, selfAdminForm).done(function () {

                    OrderSet_ImmunizationDetail.CreatingDatePickers();
                    OrderSet_ImmunizationDetail.CreatingTimePickers();
                    $.when(OrderSet_ImmunizationDetail.PopulateVISDate_VISURL_and_ManufacturerDropDown(details.AdministerVaccine_Vaccine, 'ADMINISTER', true)).then(function () {
                        $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #txtAdministerVaccine_Dose").val(details.AdministerVaccine_Dose);
                        $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_Amount").val(details.AdministerVaccine_Amount);
                        $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_Manufacturer").val(details.AdministerVaccine_Manufacturer);
                        if ($('#' + OrderSet_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_LotNumber option[value='" + details.AdministerVaccine_LotNumber + "']").length == 0) {
                            $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #ddlAdministerVaccine_LotNumber').append('<option selected value="' + details.AdministerVaccine_LotNumber + '">' + details.LotText + '</option>');
                        }
                        $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_LotNumber").val(details.AdministerVaccine_LotNumber);
                        $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail").bootstrapValidator('revalidateField', 'AdministerVaccine_LotNumber');
                        setTimeout(function () {
                            $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_Route").val(details.AdministerVaccine_Route);
                        }, 600);
                        $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail").bootstrapValidator('revalidateField', 'AdministerVaccine_Dose');
                        $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail").bootstrapValidator('revalidateField', 'AdministerVaccine_Amount');
                    });
                    $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #dpAdministerVaccine_ExpiryDate').datepicker('setDate', $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #dpAdministerVaccine_ExpiryDate').val());

                    $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_Vaccine").attr("disabled", true);
                    $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail #ddlDocumentHxDose_Vaccine").attr("disabled", true);
                    $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail #ddlRecordRefusal_Vaccine").attr("disabled", true);
                    $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_Vaccine").val(details.AdministerVaccine_Vaccine);
                    $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail #ddlDocumentHxDose_Vaccine").val(details.AdministerVaccine_Vaccine);
                    $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail #ddlRecordRefusal_Vaccine").val(details.AdministerVaccine_Vaccine);
                   
                });

            }
            else if (OrderSet_ImmunizationDetail.params.Type.trim() == "DOCUMENTHX") {

                var details = JSON.parse(response.DocVaccineHxLoad_JSON)[0];
                //OrderSet_ImmunizationDetail.SelectMainFields(details);
                $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #hfVaccineHx").val(details.VaccineHxId);
                utility.bindMyJSONByName(true, details, false, selfDocumentForm).done(function () {
                    OrderSet_ImmunizationDetail.CreatingDatePickers();
                    OrderSet_ImmunizationDetail.CreatingTimePickers();
                    $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_Vaccine").attr("disabled", true);
                    $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail #ddlDocumentHxDose_Vaccine").attr("disabled", true);
                    $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail #ddlRecordRefusal_Vaccine").attr("disabled", true);

                    setTimeout(function () {
                        $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #ddlDocumentHxDose_Route").val(details.DocumentHxDose_Route);
                    }, 500);

                    $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_Vaccine").val(details.DocumentHxDose_Vaccine);
                    $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail #ddlDocumentHxDose_Vaccine").val(details.DocumentHxDose_Vaccine);
                    $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail #ddlRecordRefusal_Vaccine").val(details.DocumentHxDose_Vaccine);
                });
            }
            else if (OrderSet_ImmunizationDetail.params.Type.trim() == "REFUSAL") {

                var details = JSON.parse(response.RefusalVaccineLoad_JSON)[0];
                //OrderSet_ImmunizationDetail.SelectMainFields(details);
                $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #hfVaccineHx").val(details.VaccineHxId);
                utility.bindMyJSONByName(true, details, false, selfRefusalForm).done(function () {
                    OrderSet_ImmunizationDetail.CreatingDatePickers();
                    OrderSet_ImmunizationDetail.CreatingTimePickers();
                    $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_Vaccine").attr("disabled", true);
                    $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail #ddlDocumentHxDose_Vaccine").attr("disabled", true);
                    $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail #ddlRecordRefusal_Vaccine").attr("disabled", true);

                    $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_Vaccine").val(details.RecordRefusal_Vaccine);
                    $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail #ddlDocumentHxDose_Vaccine").val(details.RecordRefusal_Vaccine);
                    $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail #ddlRecordRefusal_Vaccine").val(details.RecordRefusal_Vaccine);
                });
            }
        }
    },
    SelectMainFields: function (Detail) {

    },
    ActiveOneTab: function () {
        if (OrderSet_ImmunizationDetail.params.Type == "ADMINISTER") {
            $('#' + OrderSet_ImmunizationDetail.params.PanelID).find('[id*="listAdministerVaccine"]').addClass('active');
            $('#' + OrderSet_ImmunizationDetail.params.PanelID).find('[id*="tabAdministerVaccine"]').addClass('active');
            $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #listRecordRefusal').removeClass('active');
            $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #tabRecordRefusal').removeClass('active');
            $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #listDocumentHxDose').removeClass('active');
            $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #tabDocumentHxDose').removeClass('active');
        }
        else if (OrderSet_ImmunizationDetail.params.Type == "DOCUMENTHX") {
            $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #listAdministerVaccine").removeClass('active');
            $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #tabAdministerVaccine").removeClass('active');
            $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #listRecordRefusal').removeClass('active');
            $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #tabRecordRefusal').removeClass('active');
            $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #listDocumentHxDose').addClass('active');
            $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #tabDocumentHxDose').addClass('active');
        }
        else if (OrderSet_ImmunizationDetail.params.Type == "REFUSAL") {
            $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #listAdministerVaccine").removeClass('active');
            $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #tabAdministerVaccine").removeClass('active');
            $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #listRecordRefusal').addClass('active');
            $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #tabRecordRefusal').addClass('active');
            $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #listDocumentHxDose').removeClass('active');
            $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #tabDocumentHxDose').removeClass('active');
        }
    },

    searchVaccineHx: function (vaccineHxId) {
        var objData = new Object();
        objData["commandType"] = "Search_VacinehxForEdit";
        objData["VaccineHxId"] = vaccineHxId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "Immunization");
    },
    GetAgeLimScheCategAgainstVaccShedId: function (VaccineScheduleId) {
        var dfd = $.Deferred();
        OrderSet_ImmunizationDetail.GetAgeLimScheCategAgainstVaccShedId_DBCALL(VaccineScheduleId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.ScheduleDetailCount > 0) {
                    var ScheduleDetailLoad_JSON = JSON.parse(response.ScheduleDetailLoad_JSON);
                    $.each(ScheduleDetailLoad_JSON, function (i, item) {
                        $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #ddlAgeGroup').val(item.MainAgeGroup);
                        $.when(OrderSet_ImmunizationDetail.BindSchedule($('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #ddlAgeGroup'), 'ddlSchedule', item.MainSchedule)).then(function () {
                            $.when(OrderSet_ImmunizationDetail.BindCategory($('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #ddlSchedule'), 'ddlCategory', item.MainCategory)).then(function () {
                                dfd.response = response.ScheduleDetailCount;
                                dfd.resolve();
                            });
                        });
                    });

                }
                else {
                    dfd.response = response.ScheduleDetailCount;
                    dfd.resolve();
                }
            }
            else {
                dfd.response = 0;
                dfd.resolve();
                utility.DisplayMessages(response.message, 3);
            }
        });
        return dfd;
    },
    GetAgeLimScheCategAgainstVaccShedId_DBCALL: function (VaccineScheduleId) {
        var objData = new Object();
        objData["VaccineScheduleId"] = VaccineScheduleId;
        objData["commandType"] = "Get_AgeLim_Sche_Categ_Against_VaccShedId";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "Immunization");

    },
    CreatingDatePickers: function () {

        var datePickerArray = [
            "  #dpAdministerVaccine_VisitDate", "  #dpAdministerVaccine_AdministrationDate", " #dpAdministerVaccine_ExpiryDate",
            "  #dpAdministerVaccine_VISDate", "  #dpDocumentHxDose_AdministrationDate", "  #dpDocumentHxDose_ExpiryDate", " #dpRecordRefusalVaccine_ExpiryDate",

           //Start || 20 April, 2016 || ZeeshanAK || Changes for new fields
            " #dpAdministerVaccine_PIEffectiveDate", " #dpAdministerVaccine_PublicityExpiryDate", " #dpAdministerVaccine_IRSEffectiveDate", " #dpAdministerVaccine_PIEffectiveDate",
            " #dpDocumentHxDose_PublicityExpiryDate", " #dpDocumentHxDose_IRSEffectiveDate", " #dpDocumentHxDose_PIEffectiveDate"
            //End   || 20 April, 2016 || ZeeshanAK || Changes for new fields

        ];

        for (var i = 0; i < datePickerArray.length; i++) {
            if (datePickerArray[i] == " #dpAdministerVaccine_PublicityExpiryDate" || datePickerArray[i] == " #dpAdministerVaccine_PIEffectiveDate" || datePickerArray[i] == " #dpAdministerVaccine_IRSEffectiveDate" || datePickerArray[i] == " #dpAdministerVaccine_ExpiryDate" || datePickerArray[i] == " #dpDocumentHxDose_ExpiryDate" || datePickerArray[i] == " #dpRecordRefusalVaccine_ExpiryDate") {
                utility.CreateDatePicker(OrderSet_ImmunizationDetail.params.PanelID + datePickerArray[i], function () { }, false);
            } else {
                utility.CreateDatePicker(OrderSet_ImmunizationDetail.params.PanelID + datePickerArray[i], function () { }, true);
            }

        }
        //$("#date").datepicker('setDate', '01/26/2014');
    },


    CreatingTimePickers: function () {
        $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #tpAdministerVaccine_AdministrationTime').timepicker('setTime', new Date().toLocaleTimeString());
        $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #tpDocumentHxDose_AdministrationTime').timepicker('setTime', new Date().toLocaleTimeString());
    },

    ValidateAdmini: function () {
        $(' #pnlOrderSetImmunizationDetail  #frmVaccineHxAdministerTabDetail')
              .bootstrapValidator({
                  live: 'disabled',
                  message: 'This value is not valid',
                  feedbackIcons: {
                      valid: 'glyphicon glyphicon-ok',
                      invalid: 'glyphicon glyphicon-remove',
                      validating: 'glyphicon glyphicon-refresh'
                  },
                  fields: {

                      AdministerVaccine_Category: {
                          group: '.col-sm-4',
                          validators: {
                              notEmpty: {
                                  message: ''
                              }
                          }
                      },

                      AdministerVaccine_Provider: {
                          group: '.col-sm-4',
                          validators: {
                              notEmpty: {
                                  message: ''
                              }
                          }
                      },

                      AdministerVaccine_Vaccine: {
                          group: '.col-sm-4',
                          validators: {
                              notEmpty: {
                                  message: ''
                              }
                          }
                      },
                      //AdministerVaccine_AdministrationDate: {
                      //    group: '.col-sm-4',
                      //    validators: {
                      //        notEmpty: {
                      //            message: ''
                      //        }
                      //    }
                      //},
                      AdministerVaccine_Dose: {
                          group: '.col-sm-4',
                          validators: {
                              notEmpty: {
                                  message: ''
                              },
                              numeric: {
                                  message: 'Dose value should be numeric',
                                  thousandsSeparator: '',
                                  decimalSeparator: '.'
                              }
                          }

                      },
                      AdministerVaccine_Amount: {
                          group: '.col-sm-4',
                          validators: {
                              notEmpty: {
                                  message: ''
                              }
                          }
                      },
                      AdministerVaccine_Comments: {
                          group: '.col-sm-8',
                          validators: {
                              notEmpty: {
                                  enabled: false,
                                  message: ''
                              }
                          }
                      },
                      AdministerVaccine_LotNumber: {
                          group: '.col-sm-4',
                          validators: {
                              notEmpty: {
                                  message: ''
                              }
                          }
                      },

                  }
              }).on('click', '#chkAdministerVoidDose', function (e) {
                  var formValidation = $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #frmVaccineHxAdministerTabDetail').data("bootstrapValidator");
                  if (formValidation) {
                      var Control = $("#" + OrderSet_ImmunizationDetail.params["PanelID"] + ' #frmVaccineHxAdministerTabDetail #chkAdministerVoidDose').prop("checked");
                      if (Control) {
                          formValidation.enableFieldValidators('AdministerVaccine_Comments', true);
                      }
                      else {
                          formValidation.enableFieldValidators('AdministerVaccine_Comments', false);
                      }
                  }
              }).on('success.form.bv', function (e) {
                  if (OrderSet_ImmunizationDetail.getACtiveTabLidID() === "listAdministerVaccine") {
                      e.preventDefault();
                      OrderSet_ImmunizationDetail.VaccineSave('ADMINISTER');
                  }
              });



    },
    ValidateDoc: function () {
        $('#pnlOrderSetImmunizationDetail  #frmVaccineHxDocumentHxDoseTabDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   DocumentHxDose_Category: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   DocumentHxDose_Vaccine: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   //DocumentHxDose_Dose: {
                   //    group: '.col-sm-4',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        },
                   //        numeric: {
                   //            message: 'Dose is not a number or decimal',
                   //            thousandsSeparator: '',
                   //            decimalSeparator: '.'
                   //        }
                   //    }
                   //},
                   DocumentHxDose_Comments: {
                       group: '.col-sm-8',
                       validators: {
                           notEmpty: {
                               enabled: false,
                               message: ''
                           }
                       }
                   },


               }
           }).on('click', '#chkDocumentHxVoidDose', function (e) {
               var formValidation = $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #frmVaccineHxDocumentHxDoseTabDetail').data("bootstrapValidator");
               if (formValidation) {
                   var Control = $("#" + OrderSet_ImmunizationDetail.params["PanelID"] + ' #frmVaccineHxDocumentHxDoseTabDetail #chkDocumentHxVoidDose').prop("checked");
                   if (Control) {
                       formValidation.enableFieldValidators('DocumentHxDose_Comments', true);
                   }
                   else {
                       formValidation.enableFieldValidators('DocumentHxDose_Comments', false);
                   }
               }
           })
                   .on('success.form.bv', function (e) {
                       if (OrderSet_ImmunizationDetail.getACtiveTabLidID() === "listDocumentHxDose") {
                           e.preventDefault();
                           OrderSet_ImmunizationDetail.VaccineSave('DOCUMENTHX');
                       }
                   });



    },
    ValidateRefusalForm: function () {
        $('#pnlOrderSetImmunizationDetail  #frmVaccineRecordRefusalTabDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   RecordRefusal_Category: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   RecordRefusal_Provider: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   RecordRefusal_Vaccine: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   RecordRefusalReason: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   RecordRefusal_Comments: {
                       group: '.col-sm-8',
                       validators: {
                           notEmpty: {
                               enabled: false,
                               message: ''
                           }
                       }
                   },


               }
           }).on('click', '#chkRecordRefusalVoidDose', function (e) {
               var formValidation = $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #frmVaccineRecordRefusalTabDetail').data("bootstrapValidator");
               if (formValidation) {
                   var Control = $("#" + OrderSet_ImmunizationDetail.params["PanelID"] + ' #frmVaccineRecordRefusalTabDetail #chkRecordRefusalVoidDose').prop("checked");
                   if (Control) {
                       formValidation.enableFieldValidators('RecordRefusal_Comments', true);
                   }
                   else {
                       formValidation.enableFieldValidators('RecordRefusal_Comments', false);
                   }
               }
           })
                   .on('success.form.bv', function (e) {
                       if (OrderSet_ImmunizationDetail.getACtiveTabLidID() === "listRecordRefusal") {
                           e.preventDefault();
                           OrderSet_ImmunizationDetail.VaccineSave('REFUSAL');
                       }
                   });
    },



    getACtiveTabLidID: function () {
        var activeTabliId = $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #ulmmunizationDetailTabsItems ").find("li.active").attr('id');
        return activeTabliId;
    },
    VaccineSave: function (forModule) {
        if (forModule === OrderSet_ImmunizationDetail.AdministerText) {

            $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail").bootstrapValidator('revalidateField', 'AdministerVaccine_Category');
            $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail").bootstrapValidator('revalidateField', 'AdministerVaccine_Provider');
            $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail").bootstrapValidator('revalidateField', 'AdministerVaccine_Vaccine');
            $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail").bootstrapValidator('revalidateField', 'AdministerVaccine_Dose');
            $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail").bootstrapValidator('revalidateField', 'AdministerVaccine_LotNumber');
            var formValidation = $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #frmVaccineHxAdministerTabDetail').data("bootstrapValidator");
            var Control = $("#" + OrderSet_ImmunizationDetail.params["PanelID"] + ' #frmVaccineHxAdministerTabDetail #chkAdministerVoidDose').prop("checked");
            if (Control) {
                formValidation.enableFieldValidators('AdministerVaccine_Comments', true);
                $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail").bootstrapValidator('revalidateField', 'AdministerVaccine_Comments');
            }
            else {
                formValidation.enableFieldValidators('AdministerVaccine_Comments', false);
            }

            if (OrderSet_ImmunizationDetail.params.mode == "EDIT") {

                OrderSet_ImmunizationDetail.SaveAdministerVaccine().done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        $.when(Clinical_OrderSetDetails.ImmunizationSearch()).then(function () {
                            OrderSet_ImmunizationDetail.UnLoad();
                        });

                    }
                    else {
                        utility.DisplayMessages(response.message, 3);
                    }
                });

            }
            else {

                OrderSet_ImmunizationDetail.SaveAdministerVaccine().done(function (response) {

                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        $.when(Clinical_OrderSetDetails.ImmunizationSearch()).then(function () {
                            OrderSet_ImmunizationDetail.UnLoad();
                        });
                    }
                    else {

                        utility.DisplayMessages(response.Message, 3);
                    }
                });


            }
        }
        else if (forModule === OrderSet_ImmunizationDetail.DocumentHxDoseText) {

            $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail").bootstrapValidator('revalidateField', 'DocumentHxDose_Category');
            $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail").bootstrapValidator('revalidateField', 'DocumentHxDose_Vaccine');

            var formValidation = $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #frmVaccineHxDocumentHxDoseTabDetail').data("bootstrapValidator");
            var Control = $("#" + OrderSet_ImmunizationDetail.params["PanelID"] + ' #frmVaccineHxDocumentHxDoseTabDetail #chkDocumentHxVoidDose').prop("checked");
            if (Control) {
                formValidation.enableFieldValidators('DocumentHxDose_Comments', true);
                $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail").bootstrapValidator('revalidateField', 'DocumentHxDose_Comments');
            }
            else {
                formValidation.enableFieldValidators('DocumentHxDose_Comments', false);
            }

            if (OrderSet_ImmunizationDetail.params.mode == "EDIT") {

                OrderSet_ImmunizationDetail.SaveVaccineHxDose().done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        $.when(Clinical_OrderSetDetails.ImmunizationSearch()).then(function () {
                            OrderSet_ImmunizationDetail.UnLoad();
                        });

                    }
                    else {
                        utility.DisplayMessages(response.message, 3);
                    }
                });

            }
            else {

                OrderSet_ImmunizationDetail.SaveVaccineHxDose().done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        $.when(Clinical_OrderSetDetails.ImmunizationSearch()).then(function () {
                            OrderSet_ImmunizationDetail.UnLoad();
                        });
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });

            }


        }
        else if (forModule === OrderSet_ImmunizationDetail.REFUSALText) {

            $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail").bootstrapValidator('revalidateField', 'RecordRefusal_Category');
            $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail").bootstrapValidator('revalidateField', 'RecordRefusal_Provider');
            $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail").bootstrapValidator('revalidateField', 'RecordRefusal_Vaccine');
            $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail").bootstrapValidator('revalidateField', 'RecordRefusalReason');

            var formValidation = $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #frmVaccineRecordRefusalTabDetail').data("bootstrapValidator");
            var Control = $("#" + OrderSet_ImmunizationDetail.params["PanelID"] + ' #frmVaccineRecordRefusalTabDetail #chkRecordRefusalVoidDose').prop("checked");
            if (Control) {
                formValidation.enableFieldValidators('RecordRefusal_Comments', true);
                $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail").bootstrapValidator('revalidateField', 'RecordRefusal_Comments');
            }
            else {
                formValidation.enableFieldValidators('RecordRefusal_Comments', false);
            }

            if (OrderSet_ImmunizationDetail.params.mode == "EDIT") {


                OrderSet_ImmunizationDetail.SaveVaccineRefusalRecord().done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        $.when(Clinical_OrderSetDetails.ImmunizationSearch()).then(function () {
                            OrderSet_ImmunizationDetail.UnLoad();
                        });
                    }
                    else {
                        utility.DisplayMessages(response.message, 3);
                    }
                });

            }
            else {

                OrderSet_ImmunizationDetail.SaveVaccineRefusalRecord().done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        $.when(Clinical_OrderSetDetails.ImmunizationSearch()).then(function () {
                            OrderSet_ImmunizationDetail.UnLoad();
                        });
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });

            }
        }


    },
    ResetPage: function () {
        $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #ddlAgeGroup').val("");
        $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #ddlSchedule').val("");
        $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #ddlCategory').val("");

        if (!$("#" + OrderSet_ImmunizationDetail.params.PanelID + " #VaccineTabsDiv").hasClass("disableAll")) {
            $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #VaccineTabsDiv").addClass("disableAll")
        }
        var tabId = OrderSet_ImmunizationDetail.getACtiveTabLidID();
        if (tabId === "listAdministerVaccine") {

            $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #tabAdministerVaccine").find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select,[type=hidden]').each(function () {
                OrderSet_ImmunizationDetail.resetControlValue($(this));
            });
            OrderSet_ImmunizationDetail.Disable_and_RemoveDropDownItems("ddlAdministerVaccine_Manufacturer");
            OrderSet_ImmunizationDetail.Disable_and_RemoveDropDownItems(OrderSet_ImmunizationDetail.ddlAdministerVaccine_LotNumber);
            $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #lnklblAdministerVaccine_VISURL").attr('disabled', true);
            $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #lnklblAdministerVaccine_VISURL").attr("href", "#");
            $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #dpAdministerVaccine_AdministrationDate').datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), new Date()));
            OrderSet_ImmunizationDetail.CreatingTimePickers();
        }
        else if (tabId === "listDocumentHxDose") {
            $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #tabDocumentHxDose").find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select,[type=hidden]').each(function () {
                OrderSet_ImmunizationDetail.resetControlValue($(this));
            });

            $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #dpDocumentHxDose_AdministrationDate').datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), new Date()));
            OrderSet_ImmunizationDetail.CreatingTimePickers();
        }
        else if (tabId === "listRecordRefusal") {
            $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #tabRecordRefusal").find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select,[type=hidden]').each(function () {
                OrderSet_ImmunizationDetail.resetControlValue($(this));
            });
        }
    },
    resetControlValue: function (obj) {
        var currentElementTagName = obj.tagName != null ? obj.tagName : obj.prop("tagName");
        if ($(obj).attr('type') == 'text' || currentElementTagName.toLowerCase() == 'textarea' || $(obj).attr('type') == 'hidden')
            $(obj).val('');
        if ($(obj).attr('type') == 'checkbox' || $(obj).attr('type') == 'radio') {

            if ($(obj).attr('type') == 'radio') {
                obj.checked = false;
            }
            else if ($(obj).attr('type') == 'checkbox') {
                $(obj).attr('checked', false);
            }
        }

        if (currentElementTagName.toLowerCase() == 'select') {
            $(obj).find('option:selected').removeAttr('selected');
            $(obj).find('option:eq(0)').attr('selected', 'selected');
        }
        if (currentElementTagName.toLowerCase() == 'ul') {
            $(obj).find('li.active').removeClass('active');
        }
    },
    SaveAdministerVaccine: function () {

        var objData = {};
        var self = $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #frmVaccineHxAdministerTabDetail');
        favoriteListId = self.find('#hfFavoriteListId').val();

        var self = $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #frmVaccineHxAdministerTabDetail'); // $('#pnlFavoriteProcedureOrderDetail');
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objDetail = JSON.parse(myJSON);

        objData["CategoryID"] = objDetail["AdministerVaccine_Category"];
        objData["VisitDate"] = objDetail["AdministerVaccine_VisitDate_text"];
        objData["VisitDateId"] = objDetail["AdministerVaccine_VisitDate"];
        objData["ProviderId"] = objDetail["AdministerVaccine_Provider"];
        objData["VaccineID"] = objDetail["AdministerVaccine_Vaccine"];
        objData["AdministrationDate"] = objDetail["AdministerVaccine_AdministrationDate"];
        objData["Time"] = objDetail["AdministerVaccine_AdministrationTime"];
        objData["Dose"] = objDetail["AdministerVaccine_Dose"];
        objData["Amount"] = objDetail["AdministerVaccine_Amount"];
        objData["LotNo"] = objDetail["AdministerVaccine_LotNumber"];
        objData["ManufacturerId"] = objDetail["AdministerVaccine_Manufacturer"];
        objData["RouteId"] = objDetail["AdministerVaccine_Route"];
        objData["SiteId"] = objDetail["AdministerVaccine_Site"];
        objData["ExpiryDate"] = objDetail["AdministerVaccine_ExpiryDate"];
        objData["VfcId"] = objDetail["AdministerVaccine_VFC"];
        objData["VisDateId"] = $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #VisDateId").val();
        objData["Reaction"] = objDetail["AdministerReaction"];//Test In Model
        objData["VoidDose"] = objDetail["AdministerVoidDose"];//Test In Model
        objData["Comments"] = objDetail["AdministerVaccine_Comments"];
        objData["PublicityCode"] = objDetail["AdministerVaccine_PublicityCode"];
        objData["PublicityCodeExpiryDate"] = objDetail["AdministerVaccine_PublicityExpiryDate"];
        objData["ImmunizationRegistryStatusCode"] = objDetail["AdministerVaccine_IRS"];
        objData["IRSEffectiveDate"] = objDetail["AdministerVaccine_IRSEffectiveDate"];
        objData["ProtectionIndicator"] = objDetail["AdministerVaccine_ProtectionIndicator"];
        objData["PIEffectiveDate"] = objDetail["AdministerVaccine_PIEffectiveDate"];
        objData["IsActive"] = objDetail["AdministerVaccine_IsActive"] == true ? 1 : 0;
        //objData["VaccineScheduleId"] = OrderSet_ImmunizationDetail.params["VaccineScheduleId"] === 0 ? "" : OrderSet_ImmunizationDetail.params["VaccineScheduleId"];
        objData["Type"] = OrderSet_ImmunizationDetail.AdministerText;
        objData["OverrideRule"] = objDetail["AdministerOverrideRule"];
        objData["OrderSetId"] = OrderSet_ImmunizationDetail.params.OrderSetId;
        objData["MainAgeGroup"] = $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #ddlAgeGroup').val();
        objData["MainSchedule"] = $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #ddlSchedule').val();
        objData["MainCategory"] = $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #ddlCategory').val();



        //if (typeof GetSelectedPatientID() === "undefined") {
        //    objData["PatientId"] = OrderSet_ImmunizationDetail.params.patientID;
        //}
        //else {
        //    objData["PatientId"] = GetSelectedPatientID();//163; //
        //}





        if (OrderSet_ImmunizationDetail.params.mode == "Add")
            objData["commandType"] = "Save_AdministerVaccine";
        else {
            objData["VaccineHxId"] = OrderSet_ImmunizationDetail.params.VaccineHxId;
            objData["commandType"] = "update_VacinehxDose";
        }

        //objData["PatientAge"] = PatientAge;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "Immunization");


    },
    SaveVaccineHxDose: function () {
        //FavoriteListIcd
        var objData = {};
        var self = $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #frmVaccineHxDocumentHxDoseTabDetail');
        favoriteListId = self.find('#hfFavoriteListId').val();

        var self = $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #frmVaccineHxDocumentHxDoseTabDetail'); // $('#pnlFavoriteProcedureOrderDetail');
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objDetail = JSON.parse(myJSON);

        objData["CategoryID"] = objDetail["DocumentHxDose_Category"];
        objData["SourceOfHxId"] = objDetail["DocumentHxDose_SourceOfHx"];
        objData["ProviderId"] = objDetail["DocumentHxDose_Provider"];
        objData["VaccineID"] = objDetail["DocumentHxDose_Vaccine"];
        objData["Dose"] = objDetail["DocumentHxDose_Dose"];
        objData["Amount"] = objDetail["DocumentHxDose_Amount"];
        objData["RouteId"] = objDetail["DocumentHxDose_Route"];
        objData["SiteId"] = objDetail["DocumentHxDose_Site"];
        objData["VoidDose"] = objDetail["DocumentHxVoidDose"];
        objData["Comments"] = objDetail["DocumentHxDose_Comments"];
        objData["Type"] = OrderSet_ImmunizationDetail.DocumentHxDoseText;


        objData["IsActive"] = objDetail["DocumentHxDose_IsActive"] == true ? 1 : 0;
        if (OrderSet_ImmunizationDetail.params.mode == "Add")
            objData["commandType"] = "Save_VacineHxDose";
        else {

            objData["VaccineHxId"] = OrderSet_ImmunizationDetail.params.VaccineHxId;
            objData["commandType"] = "update_VacinehxDose";
        }
        objData["OrderSetId"] = OrderSet_ImmunizationDetail.params.OrderSetId;
        objData["MainAgeGroup"] = $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #ddlAgeGroup').val();
        objData["MainSchedule"] = $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #ddlSchedule').val();
        objData["MainCategory"] = $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #ddlCategory').val();

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "Immunization");

    },
    SaveVaccineRefusalRecord: function () {
        //FavoriteListIcd
        var objData = {};
        var self = $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #frmVaccineHxDocumentHxDoseTabDetail');
        favoriteListId = self.find('#hfFavoriteListId').val();

        var self = $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #frmVaccineRecordRefusalTabDetail'); // $('#pnlFavoriteProcedureOrderDetail');
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objDetail = JSON.parse(myJSON);

        objData["CategoryID"] = objDetail["RecordRefusal_Category"];
        objData["ProviderId"] = objDetail["RecordRefusal_Provider"];
        objData["VaccineID"] = objDetail["RecordRefusal_Vaccine"];
        objData["RefusalReasonID"] = objDetail["RecordRefusalReason"];
        objData["ExpiryDate"] = objDetail["RecordRefusalVaccine_ExpiryDate"];
        objData["VoidDose"] = objDetail["RecordRefusalVoidDose"];
        objData["Comments"] = objDetail["RecordRefusal_Comments"];
        objData["Type"] = OrderSet_ImmunizationDetail.REFUSALText;
        objData["OrderSetId"] = OrderSet_ImmunizationDetail.params.OrderSetId;



        objData["IsActive"] = objDetail["RecordRefusal_IsActive"] == true ? 1 : 0;

        if (OrderSet_ImmunizationDetail.params.mode == "Add")
            objData["commandType"] = "save_vacinerefusalrecord";
        else {

            objData["VaccineHxId"] = OrderSet_ImmunizationDetail.params.VaccineHxId;
            objData["commandType"] = "update_vacinerefusalrecord";
        }


        objData["MainAgeGroup"] = $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #ddlAgeGroup').val();
        objData["MainSchedule"] = $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #ddlSchedule').val();
        objData["MainCategory"] = $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #ddlCategory').val();

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "Immunization");

    },
    IntializeAllTabs: function () {
        var dfd = $.Deferred();
        $.when(OrderSet_ImmunizationDetail.PopulateVaccineGroupCategory(true, "#ddlAdministerVaccine_Category", "GetAdministerVaccine_Category", null, $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #ddlCategory option:selected").text(),$('#' + OrderSet_ImmunizationDetail.params.PanelID + " #ddlCategory option:selected").val())).then(function () {
            $.when(OrderSet_ImmunizationDetail.PopulateVaccineGroupCategory(true, "#ddlDocumentHxDose_Category", "GetAdministerVaccine_Category", null, $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #ddlCategory option:selected").text(), $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #ddlCategory option:selected").val())).then(function () {
                $.when(OrderSet_ImmunizationDetail.PopulateVaccineGroupCategory(true, "#ddlRecordRefusal_Category", "GetAdministerVaccine_Category", null, $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #ddlCategory option:selected").text(), $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #ddlCategory option:selected").val())).then(function () {
                    dfd.resolve();
                });
            });
        });
        return dfd;
    },
    CategoryChange: function (obj) {
        if ($(obj).val() != '') {
            if ($("#" + OrderSet_ImmunizationDetail.params.PanelID + " #VaccineTabsDiv").hasClass("disableAll")) {
                $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #VaccineTabsDiv").removeClass("disableAll")
            }
            OrderSet_ImmunizationDetail.IntializeAllTabs();
        }
        else {
            if (!$("#" + OrderSet_ImmunizationDetail.params.PanelID + " #VaccineTabsDiv").hasClass("disableAll")) {
                $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #VaccineTabsDiv").addClass("disableAll")
            }
        }

    },
    PopulateVaccineGroupCategory: function (isLoad, controlId, methodName, data, CategoryId,CategoryVal) {

        var dfd = new $.Deferred();
        var forModule = "";
        if (controlId === '#ddlAdministerVaccine_Category') {

            OrderSet_ImmunizationDetail.Disable_and_RemoveDropDownItems('ddlAdministerVaccine_Vaccine');
            OrderSet_ImmunizationDetail.Disable_and_RemoveDropDownItems('ddlAdministerVaccine_Manufacturer');
            data = "StrID=1";
            forModule = "ADMINISTER";
        }
        else if (controlId === '#ddlDocumentHxDose_Category') {
            data = "StrID=0";
            OrderSet_ImmunizationDetail.Disable_and_RemoveDropDownItems('ddlDocumentHxDose_Vaccine');
            OrderSet_ImmunizationDetail.Disable_and_RemoveDropDownItems('ddlDocumentHxDose_Manufacturer');
            forModule = "DOCUMENTHX";
        }
        else if (controlId === '#ddlRecordRefusal_Category') {
            data = "StrID=0";
            OrderSet_ImmunizationDetail.Disable_and_RemoveDropDownItems('ddlRecordRefusal_Vaccine');
            forModule = "REFUSAL";
        }

        $.when(OrderSet_ImmunizationDetail.LoadingDropDowns(isLoad, controlId, methodName, data)).then(function () {
            if (!$('#' + OrderSet_ImmunizationDetail.params.PanelID + " " + controlId).hasClass("disableAll")) {
                $('#' + OrderSet_ImmunizationDetail.params.PanelID + " " + controlId).addClass("disableAll");
            }
            $("#" + OrderSet_ImmunizationDetail.params.PanelID + " " + controlId + ' option').filter(function () {
                return $.trim($(this).text().toLowerCase()) == CategoryId.toLowerCase() && $(this).val() == CategoryVal
            }).prop('selected', true);
            $.when(OrderSet_ImmunizationDetail.PopulateVaccineDropDown($("#" + OrderSet_ImmunizationDetail.params.PanelID + " " + controlId).val(), forModule)).then(function () {
                dfd.resolve();
            });
        });
        return dfd.promise();
    },
    PopulateVaccineDropDown: function (vaccineGroupCategoryId, forModule, selectedVaccineId) {
        var dfd = new $.Deferred();
        var controlId = "";
        if (forModule === OrderSet_ImmunizationDetail.AdministerText) {
            var controlId = "ddlAdministerVaccine_Vaccine";
        }
        else if (forModule === OrderSet_ImmunizationDetail.DocumentHxDoseText) {
            var controlId = "ddlDocumentHxDose_Vaccine";
        }
        else if (forModule === OrderSet_ImmunizationDetail.REFUSALText) {
            var controlId = "ddlRecordRefusal_Vaccine";
        }

        if ((forModule === OrderSet_ImmunizationDetail.AdministerText || forModule === OrderSet_ImmunizationDetail.DocumentHxDoseText || forModule === OrderSet_ImmunizationDetail.REFUSALText) && vaccineGroupCategoryId !== "") {

            // --------------------------- Actions -----------------------------
            $('#' + controlId).attr('disabled', false);
            if (forModule === OrderSet_ImmunizationDetail.AdministerText) {
                OrderSet_ImmunizationDetail.Disable_Manufacturer_VISDate_VISurl(forModule);
                OrderSet_ImmunizationDetail.Disable_and_RemoveDropDownItems(OrderSet_ImmunizationDetail.ddlAdministerVaccine_LotNumber);
            }
            // $("ddlAdministerVaccine_Vaccine").val("");
            // -------------------Get Paramters and Load Dropdown -------------
            if (OrderSet_ImmunizationDetail.params.TabId == "Hx") {
                var data = "StrID=" + vaccineGroupCategoryId + "&StrID2=" + forModule;
            }
            else if (OrderSet_ImmunizationDetail.params.TabId == "Alert") {
                var data = "StrID=" + vaccineGroupCategoryId + "&StrID2=" + forModule;
            }
            else {
                var data = "StrID=" + vaccineGroupCategoryId + "&StrID2=" + forModule;
            }

            OrderSet_ImmunizationDetail.LoadingDropDowns(true, " #" + controlId, OrderSet_ImmunizationDetail.LookupVaccineMethod, data).done(function () {

                if (selectedVaccineId != null && vaccineGroupCategoryId != "") {
                    if (forModule === OrderSet_ImmunizationDetail.DocumentHxDoseText) {
                        $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #ddlDocumentHxDose_Vaccine").val(selectedVaccineId);
                        dfd.resolve();
                    }
                    else if (forModule === OrderSet_ImmunizationDetail.AdministerText) {
                        $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_Vaccine").val(selectedVaccineId);
                        $.when(OrderSet_ImmunizationDetail.PopulateVISDate_VISURL_and_ManufacturerDropDown(selectedVaccineId, forModule)).then(function () {
                            dfd.resolve();
                        });
                    }
                    else if (forModule === OrderSet_ImmunizationDetail.REFUSALText) {
                        $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #ddlRecordRefusal_Vaccine").val(selectedVaccineId);
                        dfd.resolve();
                    }
                    else {
                        dfd.resolve();
                    }
                }
                if (selectedVaccineId != null && vaccineGroupCategoryId == 0) {
                    if (forModule === OrderSet_ImmunizationDetail.DocumentHxDoseText) {
                        $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #ddlDocumentHxDose_Vaccine").val(selectedVaccineId);
                        dfd.resolve();
                    }
                    else if (forModule === OrderSet_ImmunizationDetail.AdministerText) {
                        $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_Vaccine").val(selectedVaccineId);
                        dfd.resolve();
                    }
                    else if (forModule === OrderSet_ImmunizationDetail.REFUSALText) {
                        $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #ddlRecordRefusal_Vaccine").val(selectedVaccineId);
                        dfd.resolve();
                    }
                    else {
                        dfd.resolve();
                    }
                }
                else {
                    if (forModule === OrderSet_ImmunizationDetail.AdministerText) {
                        $.when(OrderSet_ImmunizationDetail.PopulateVISDate_VISURL_and_ManufacturerDropDown($('#' + OrderSet_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_Vaccine").val(), forModule)).then(function () {
                            dfd.resolve();
                        });
                    }
                    else {
                        if (forModule === OrderSet_ImmunizationDetail.DocumentHxDoseText) {
                            if ($('#' + OrderSet_ImmunizationDetail.params.PanelID + " #ddlDocumentHxDose_Vaccine").val() != "") {
                                Immunization_ImmunizationAddImmInj.GetVaccineInformation('immunization', $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #ddlDocumentHxDose_Vaccine").val()).done(function (response) {
                                    $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #txtDocumentHxDose_Dose').val(response.Dose);
                                    $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #ddlDocumentHxDose_Amount').val(response.Amount);
                                    dfd.resolve();
                                });
                            }
                            else {
                                dfd.resolve();
                            }
                        }
                        else {
                            dfd.resolve();
                        }
                    }
                }

            });
        } else {
            OrderSet_ImmunizationDetail.Disable_Manufacturer_VISDate_VISurl(forModule);
            OrderSet_ImmunizationDetail.Disable_and_RemoveDropDownItems(controlId);
            dfd.resolve();
            //  $('#' + controlId).attr('disabled', true);
        }
        return dfd;
    },
    PopulateVISDate_VISURL_and_ManufacturerDropDown: function (vaccineId, forModule, NotCallWhyLotIsNotAva) {
        var dfd = new $.Deferred();
        var controlId = forModule === OrderSet_ImmunizationDetail.AdministerText ? "ddlAdministerVaccine_Vaccine" : "ddlDocumentHxDose_Vaccine";
        if ($(" #" + controlId).val() !== "") {
            $.when(OrderSet_ImmunizationDetail.PopulateVaccineManufacturerDropDown(vaccineId, forModule)).then(function () {
                if (forModule === "ADMINISTER") {
                    $.when(OrderSet_ImmunizationDetail.PopulateLotNumber(vaccineId, NotCallWhyLotIsNotAva)).then(function () {
                        $.when(OrderSet_ImmunizationDetail.SetLotManufanucture($("#" + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_LotNumber").val())).then(function () {
                            OrderSet_ImmunizationDetail.PopulateVISDate(vaccineId, forModule);
                            OrderSet_ImmunizationDetail.PopulateVISURL(vaccineId, forModule);
                            if (vaccineId != "") {
                                Immunization_ImmunizationAddImmInj.GetVaccineInformation('immunization', vaccineId).done(function (response) {
                                    $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #txtAdministerVaccine_Dose').val(response.Dose);
                                    $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #ddlAdministerVaccine_Amount').val(response.Amount);
                                    $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail").bootstrapValidator('revalidateField', 'AdministerVaccine_Dose');
                                    $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail").bootstrapValidator('revalidateField', 'AdministerVaccine_Amount');
                                    dfd.resolve();
                                });
                            }
                            else {
                                dfd.resolve();
                            }
                        });
                    });
                }
            });


            if (forModule === "DOCUMENTHX") {
                if ($('#' + OrderSet_ImmunizationDetail.params.PanelID + " #ddlDocumentHxDose_Vaccine").val() != "") {
                    Immunization_ImmunizationAddImmInj.GetVaccineInformation('immunization', $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #ddlDocumentHxDose_Vaccine").val()).done(function (response) {
                        $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #txtDocumentHxDose_Dose').val(response.Dose);
                        $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #ddlDocumentHxDose_Amount').val(response.Amount);
                        dfd.resolve();
                    });
                }
                else {
                    dfd.resolve();
                }
            }

        } else {
            OrderSet_ImmunizationDetail.Disable_Manufacturer_VISDate_VISurl(forModule);
            dfd.resolve();
        }
        return dfd;
    },

    SetLotManufanucture: function (LotId) {
        var dfd = $.Deferred();
        if (LotId != "" && LotId != null && LotId > 0) {
            OrderSet_ImmunizationDetail.GetLotManufanucture_DB_CALL(LotId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    //$("#frmVaccineHxAdministerTabDetail #hfAdministerTabManufacturer").val(response.ManufactureId);
                    //$("#frmVaccineHxAdministerTabDetail #txtAdministerTabManufacturer").val(response.ManufacturerName);
                    $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_Manufacturer").val(response.ManufactureId);
                    dfd.resolve();
                }
                else {
                    dfd.resolve();
                    utility.DisplayMessages(response.message, 3);
                }
            });
        }
        else {
            $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_Manufacturer").val("");
            dfd.resolve();
        }
        return dfd;
    },

    GetLotManufanucture_DB_CALL: function (LotId) {
        var objData = new Object();
        objData["LotId"] = LotId;
        objData["commandType"] = "Get_Lot_Manufanucture";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");
    },
    PopulateVISDate: function (vaccineId, forModule) {
        var objData = new Object();
        objData["VaccineID"] = vaccineId;
        objData["commandType"] = "Get_VISDate";
        var data = JSON.stringify(objData);
        MDVisionService.APIService(data, "MEDICAL", "Immunization").done(function (response) {
            response = JSON.parse(response);
            if (response != null && response.status === true) {
                $(' #dpAdministerVaccine_VISDate').val(response.VISDate);
                $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #VisDateId").val(response.VISId);
            }
            else {
                $(' #dpAdministerVaccine_VISDate').val("");
                $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #VisDateId").val(0);
            }
        });
    },
    PopulateVaccineManufacturerDropDown: function (vaccineId, forModule) {
        var dfd = new $.Deferred();
        var controlId = forModule === OrderSet_ImmunizationDetail.AdministerText ? "ddlAdministerVaccine_Manufacturer" : "ddlDocumentHxDose_Manufacturer";
        if ((forModule === OrderSet_ImmunizationDetail.AdministerText || forModule === OrderSet_ImmunizationDetail.DocumentHxDoseText) && vaccineId !== "") {
            // --------------------------- Actions -----------------------------
            $(' #' + controlId).prop('disabled', false);
            // -------------------Get Paramters and Load Dropdown -------------
            var data = "StrID=" + vaccineId + "&StrID2=" + forModule;
            $.when(OrderSet_ImmunizationDetail.LoadingDropDowns(true, " #" + controlId, OrderSet_ImmunizationDetail.LookupVaccineManufacturerMethod, data)).then(function () {
                dfd.resolve();
            });
        } else {
            OrderSet_ImmunizationDetail.Disable_and_RemoveDropDownItems(controlId);
            //  $(' #' + controlId).prop('disabled', true);
        }
        return dfd;
    },

    PopulateVISURL: function (vaccineId, forModule) {
        if (forModule === OrderSet_ImmunizationDetail.AdministerText) {
            var objData = new Object();
            objData["VaccineID"] = vaccineId;
            objData["commandType"] = "get_visurl";

            var data = JSON.stringify(objData);
            MDVisionService.APIService(data, "MEDICAL", "Immunization").done(function (response) {
                response = JSON.parse(response);
                if (response != null && response.VIS_url !== "") {
                    $(" #pnlOrderSetImmunizationDetail #lnklblAdministerVaccine_VISURL").attr("href", response.VIS_url);
                    if ($(" #pnlOrderSetImmunizationDetail #lnklblAdministerVaccine_VISURL").attr("href") !== "") {
                        $(" #pnlOrderSetImmunizationDetail #lnklblAdministerVaccine_VISURL").attr('disabled', false);
                    }
                }
                else {
                    $(" #pnlOrderSetImmunizationDetail #lnklblAdministerVaccine_VISURL").attr('disabled', true);
                }
            });
        }
    },

    PopulateLotNumber: function (vaccineId, ComeFormLoadFuntion) {
        var dfd = new $.Deferred();
        //$(" #pnlOrderSetImmunizationDetail #lnklblAdministerVaccine_LotNo").attr('disabled', false);
        $(" #pnlOrderSetImmunizationDetail #" + OrderSet_ImmunizationDetail.ddlAdministerVaccine_LotNumber).attr('disabled', false);
        if (vaccineId > 0 && $(" #pnlOrderSetImmunizationDetail #ddlAdministerVaccine_Provider").val() > 0) {
            var data = "ID=" + vaccineId + "&ID2=" + $(" #pnlOrderSetImmunizationDetail #ddlAdministerVaccine_Provider").val();
            $.when(OrderSet_ImmunizationDetail.LoadingDropDowns(true, "#" + OrderSet_ImmunizationDetail.ddlAdministerVaccine_LotNumber, OrderSet_ImmunizationDetail.LookupFunctionForGetLotNumbers, data)).then(function () {
                if (typeof ComeFormLoadFuntion == typeof undefined || ComeFormLoadFuntion == null) {
                    if ($(" #pnlOrderSetImmunizationDetail #" + OrderSet_ImmunizationDetail.ddlAdministerVaccine_LotNumber).find("option").length == 1) {
                        var checkEmpty = false;
                        if ($(" #pnlOrderSetImmunizationDetail #" + OrderSet_ImmunizationDetail.ddlAdministerVaccine_LotNumber).find("option:first").text() == "- Select -") {
                            checkEmpty = true;
                        }
                        //if()
                        if (checkEmpty) {
                            $.when(Clinical_ImmunizationDetail.WhyLotIsNotPopulateInDropDown(vaccineId, $(" #pnlOrderSetImmunizationDetail #ddlAdministerVaccine_Provider").val(), "Vaccine")).then(function () {
                                dfd.resolve();
                            });
                        }
                        else {
                            dfd.resolve();
                        }

                    }
                    else {
                        dfd.resolve();
                    }
                }
                else {
                    dfd.resolve();
                }
                $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail").bootstrapValidator('revalidateField', 'AdministerVaccine_LotNumber');
            });
        }
        else {
            $("#pnlOrderSetImmunizationDetail #" + OrderSet_ImmunizationDetail.ddlAdministerVaccine_LotNumber).empty();
            $("#pnlOrderSetImmunizationDetail #" + OrderSet_ImmunizationDetail.ddlAdministerVaccine_LotNumber).append('<option value="">- Select -</option>');
            dfd.resolve();
        }
        return dfd;
    },
    LoadingDropDowns: function (isLoad, controlId, methodName, data) {

        var dfd = new $.Deferred();

        var contrainerid = "pnlOrderSetImmunizationDetail";  //this[0].id;
        if (data == null || data == "undefined") {
            data = "";
        }

        var ddl = " " + controlId + " ";
        var getDataMethod = methodName;
        if (true) {
            return MDVisionService.lookups(getDataMethod, isLoad, data).done(function (results) {
                results = JSON.parse(results[getDataMethod]);
                //if (isLoad && results) {
                if (results) {

                    var l = $(ddl);
                    //action pan required contrainer id
                    //  if ($('#' + contrainerid + ' #' + $(ddl)[0].id).length > 0)
                    //  if ($('#' + contrainerid + ' #' + $(ddl)).length > 0)


                    //l = $('#' + contrainerid + " " + ddl); //   l = $('#' + contrainerid + ' #' + $(ddl)[0].id);

                    if (l.id == "LedgerAccount") {
                        console.log(results);
                    }
                    l.empty();
                    $.each(results, function (j, result) {

                        //if ($(ddls).prop("tagName").toLowerCase() == "datalist") {
                        //    l.append($("<option />").val(result.Name).attr("RefValue", result.RefValue).attr("RefName", result.RefName));
                        //}
                        //else {
                        if (result.Name == "" && (methodName == "GetPatientVisits" || methodName == "GetVaccineManufacturer")) {

                        }
                        else {
                            if (j == 0 && results.length == 1 && methodName == "GetVaccineLotNumber") {
                                l.append($("<option checked/>").val(result.Value).text(result.Name).attr("RefValue", result.RefValue).attr("RefName", result.RefName));

                                OrderSet_ImmunizationDetail.LotNumberChange(result.Value);
                            }
                            else if (methodName == "GetAdministerVaccine_Category") {
                                l.append($("<option checked />").val(result.Value).text(result.Name).attr("RefValue", result.RefValue).attr("RefName", result.RefName));
                            }
                            else {
                                l.append($("<option />").val(result.Value).text(result.Name).attr("RefValue", result.RefValue).attr("RefName", result.RefName));
                            }

                        }
                        //}
                        //l.append($("<option />").attr("RefValue", result.RefValue));
                    });
                    BackgroundLoaderShow(false);
                    dfd.resolve();
                }
            });
        }

        return dfd.promise();
    },
    LotNumberChange: function (LotId) {
        var dfd = $.Deferred();
        if (LotId != "") {
            OrderSet_ImmunizationDetail.get_LotDataForAutoPopulate(LotId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.LotNumberCount > 0) {
                        var LotNumberLoad_JSON = JSON.parse(response.LotNumberLoad_JSON)[0];
                        OrderSet_ImmunizationDetail.SetExpiryDateAndRoute(LotNumberLoad_JSON.ExpiryDate, LotNumberLoad_JSON.RouteId);
                        dfd.resolve();
                    }
                    else {
                        if ($('#' + OrderSet_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_LotNumber").length == 1 && $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_LotNumber").val() != "") {

                        }
                        else {
                            $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_Route").val("");
                            $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #dpAdministerVaccine_ExpiryDate').val("");
                        }
                        dfd.resolve();
                    }
                }
                else {
                    dfd.resolve();
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else {
            $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_Route").val("");
            $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #dpAdministerVaccine_ExpiryDate').val("");
            dfd.resolve();
        }
        return dfd
    },
    SetExpiryDateAndRoute: function (ExpiryDate, RouteId) {
        if (RouteId != "" && RouteId != "0") {
            $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_Route").val(RouteId);
        }
        else {
            $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_Route").val("");
        }
        if (ExpiryDate != "") {
            ExpiryDate = $.datepicker.formatDate('mm dd yy ', new Date(ExpiryDate));
            $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #dpAdministerVaccine_ExpiryDate').val(ExpiryDate);
            $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #dpAdministerVaccine_ExpiryDate').datepicker('setDate', $('#' + OrderSet_ImmunizationDetail.params.PanelID + ' #dpAdministerVaccine_ExpiryDate').val());
        }

    },
    get_LotDataForAutoPopulate: function (LotId) {

        var objData = new Object();
        PageNumber = 1;
        RowsPerPage = 5000;
        objData["VaccineLotNoId"] = LotId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["Checkprivilegas"] = "yes";
        objData["commandType"] = "get_lotnumber_by_id";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ImmunizationLotNumber");
    },
    toggleMe: function (obj) {
        $(obj).parent().parent().find('.toggleMe').toggleClass('hidden');
        if ($(obj).parent().hasClass("col-sm-12")) {
            $(obj).parent().removeClass("col-sm-12")
            $(obj).parent().addClass("col-sm-4")
            $(obj).parent().removeClass("text-center")
            $(obj).parent().addClass("align-left")
        }
        else if ($(obj).parent().hasClass("col-sm-4")) {
            $(obj).parent().removeClass("col-sm-4")
            $(obj).parent().addClass("col-sm-12")
            $(obj).parent().addClass("text-center")
            $(obj).parent().removeClass("align-left")
        }

    },
    BindSchedule: function ($obj, ddl, Value) {
        var dfd = $.Deferred();
        if ($($obj).val() != '') {
            var ScheduleTypeid = $($obj).val();
            $.when(CacheManager.BindDropDownsByID("#" + OrderSet_ImmunizationDetail.params["PanelID"] + ' #' + ddl, 'GetImmunizationSchedule', true, ScheduleTypeid)).then(function () {
                if (Value != null && typeof Value != typeof undefined) {
                    $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #ddlSchedule").val(Value);
                    dfd.resolve();
                }
                else {
                    dfd.resolve();
                }
            });
        }
        else {
            $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #ddlSchedule").html("<option value=''>- Select -</option>");
            $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #ddlCategory").html("<option value=''>- Select -</option>");
            if (!$("#" + OrderSet_ImmunizationDetail.params.PanelID + " #VaccineTabsDiv").hasClass("disableAll")) {
                $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #VaccineTabsDiv").addClass("disableAll")
            }
            dfd.resolve();
        }
        return dfd;
    },
    BindCategory: function ($obj, ddl, Value) {
        var dfd = $.Deferred();
        if ($($obj).val() != '') {
            var Scheduleid = $($obj).val();
            var ScheduleTypeid = $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #ddlAgeGroup").val();

            var self = $('#' + OrderSet_ImmunizationDetail.params.PanelID);
            self.find('#ddlCategory').attr('ddlist', 'GetCategoryAgaintsSchAndSchtype');
            var data = "IsActive=&ID=" + ScheduleTypeid + "&ID2=" + Scheduleid;
            self.find('#' + ddl).parent().loadDropDowns(true, data).done(function () {
                if (Value != null && typeof Value != typeof undefined) {
                    $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #ddlCategory").val(Value);
                    dfd.resolve();
                }
                else {
                    dfd.resolve();
                }
            });

        }
        else {
            $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #ddlCategory").html("<option value=''>- Select -</option>");
            if (!$("#" + OrderSet_ImmunizationDetail.params.PanelID + " #VaccineTabsDiv").hasClass("disableAll")) {
                $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #VaccineTabsDiv").addClass("disableAll")
            }
            dfd.resolve();
        }
        return dfd;
    },
    //Author: Talha Tanweer
    Disable_and_RemoveDropDownItems: function (ddlId) {
        $(" #" + ddlId).children().remove();
        $(" #" + ddlId).attr('disabled', true);
    },
    Disable_Manufacturer_VISDate_VISurl: function (forModule) {
        var manufacturerddlId = forModule === this.AdministerText ? this.ddlAdministerVaccine_Manufacturer : this.ddlDocumentHxDose_Manufacturer;
        OrderSet_ImmunizationDetail.Disable_and_RemoveDropDownItems(manufacturerddlId);
        OrderSet_ImmunizationDetail.Disable_and_RemoveDropDownItems(OrderSet_ImmunizationDetail.ddlAdministerVaccine_LotNumber);

        if (forModule === this.AdministerText) {
            $(" #pnlOrderSetImmunizationDetail #lnklblAdministerVaccine_VISURL").attr('disabled', true);
            $(" #pnlOrderSetImmunizationDetail #lnklblAdministerVaccine_VISURL").attr("href", "#");
            $(" #dpAdministerVaccine_VISDate").val("");

            //$(" #pnlOrderSetImmunizationDetail #lnklblAdministerVaccine_LotNo").attr('disabled', true);

        }
    },
    SetDate: function (obj, Id) {
        if (Id != "dpAdministerVaccine_PIEffectiveDate") {
            if ($(obj).val() != "") {
                $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #" + Id).datepicker("setDate", $.datepicker.formatDate(date_format.replace('MM/dd/yy', ''), new Date()));
            }
            else {
                $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #" + Id).val("");
            }
        }
        else {
            if ($(obj).prop("checked")) {
                $('#' + OrderSet_ImmunizationDetail.params.PanelID + " #" + Id).datepicker("setDate", $.datepicker.formatDate(date_format.replace('MM/dd/yy', ''), new Date()));
            }
            else {
                $("#" + OrderSet_ImmunizationDetail.params.PanelID + " #" + Id).val("");
            }
        }
    },

    ProviderChange: function (ComeFormLoadFuntion) {
        $.when(OrderSet_ImmunizationDetail.PopulateLotNumber($("#pnlOrderSetImmunizationDetail #ddlAdministerVaccine_Vaccine").val())).then(function () {
            OrderSet_ImmunizationDetail.SetLotManufanucture($("#" + OrderSet_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_LotNumber").val());
        });
    },
    IsVaccineHxInValidAge: function (OS_VaccineHxId, PatientId) {

        var dfd = $.Deferred();
        OrderSet_ImmunizationDetail.IsVaccineHxInValidAge_DB_CALL(OS_VaccineHxId, PatientId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                dfd.resolve(response.IsVaccineHxInValidAge);
            }
            else {
                dfd.resolve(null);
                utility.DisplayMessages(response.message, 3);
            }
        });

        return dfd.then(function (result) {
            return result;
        });
    },

    IsVaccineHxInValidAge_DB_CALL: function (OS_VaccineHxId, PatientId) {
        var objData = new Object();
        objData["OS_VaccineHxId"] = OS_VaccineHxId;
        objData["PatientId"] = PatientId
        objData["commandType"] = "Is_VaccineHx_In_Valid_Age";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "Immunization");
    },


    IsVaccineHxLotIssue: function (OS_VaccineHxId,Type,ImmunizationIds) {

        var dfd = $.Deferred();
        OrderSet_ImmunizationDetail.IsVaccineHxLotIssue_DB_CALL(OS_VaccineHxId, Type, ImmunizationIds).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                dfd.resolve(response.IsVaccineHxLotIssue);
            }
            else {
                dfd.resolve(null);
                utility.DisplayMessages(response.message, 3);
            }
        });

        return dfd.then(function (result) {
            return result;
        });
    },

    IsVaccineHxLotIssue_DB_CALL: function (OS_VaccineHxId, Type,ImmunizationIds) {
        var objData = new Object();
        objData["OS_VaccineHxId"] = OS_VaccineHxId;
        if (typeof Type != typeof undefined && Type != null && Type=="Therapeutic") {
            objData["Type"] = Type;
        }
        else {
            objData["Type"] = "Immunization";
        }
        if (typeof ImmunizationIds != typeof undefined && ImmunizationIds != null && ImmunizationIds.length>0) {
            objData["ImmunizationIds"] = ImmunizationIds.join(',');
        }
        objData["commandType"] = "Is_VaccineHx_Lot_Issue";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "Immunization");
    },
    UnLoad: function () {

        if (OrderSet_ImmunizationDetail.params != null && OrderSet_ImmunizationDetail.params.ParentCtrl && OrderSet_ImmunizationDetail.params.ParentCtrlPanelID) {
            UnloadActionPan(OrderSet_ImmunizationDetail.params.ParentCtrl, "OrderSet_ImmunizationDetail", null, OrderSet_ImmunizationDetail.params.ParentCtrlPanelID);
        }
        else if (OrderSet_ImmunizationDetail.params != null && OrderSet_ImmunizationDetail.params.ParentCtrl) {
            UnloadActionPan(OrderSet_ImmunizationDetail.params.ParentCtrl, "OrderSet_ImmunizationDetail");
        }
        else {
            UnloadActionPan(null, "OrderSet_ImmunizationDetail");
        }
    },
}