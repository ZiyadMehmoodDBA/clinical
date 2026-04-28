
//$(document).on('change', '#ddlAdministerVaccine_Category', function() {

//    Clinical_ImmunizationDetail.PopulateVaccineDropDown("5", "ADMINISTER");
//});


Clinical_ImmunizationDetail = {
    bIsFirstLoad: true,
    params: [],
    EditableGrid: null,
    myGrid: null,
    FavListName: 'ClinicalImmunizationDetail',
    AdministerText: "ADMINISTER",
    DocumentHxDoseText: "DOCUMENTHX",
    REFUSALText: "REFUSAL",
    CommSeparatedFavListsStatus: "",
    DeleteProcedureIds: "",
    LookupVaccineMethod: "GetAdministerVaccine_Vaccine",
    LookupVaccineManufacturerMethod: "GetVaccineManufacturer",
    LookupVaccineLotNumber: "GetVaccineLotNumber",

    ddlAdministerVaccine_LotNumber: "ddlAdministerVaccine_LotNumber",

    ddlAdministerVaccine_Category: "ddlAdministerVaccine_Category",
    ddlDocumentHxDose_Category: "ddlDocumentHxDose_Category",
    ddlRecordRefusal_Category: "ddlRecordRefusal_Category",

    ddlAdministerVaccine_Vaccine: "ddlAdministerVaccine_Vaccine",
    ddlDocumentHxDose_Vaccine: "ddlDocumentHxDose_Vaccine",

    txtAdministerTabManufacturer: "txtAdministerTabManufacturer",
    ddlDocumentHxDose_Manufacturer: "ddlDocumentHxDose_Manufacturer",

    dpAdministerVaccine_VISDate: "dpAdministerVaccine_VISDate",

    LookupFunctionForGetLotNumbers: "GetVaccineLotNumber",
    ddlAdministerVaccine_LotNumber: "ddlAdministerVaccine_LotNumber",
    previousVoid: false,
    //Start//Talha Tanweer//22/03/2016//This function will be called once tab is clicked, it expects parameters to be used for Immunization
    Load: function (params) {
        Clinical_ImmunizationDetail.params = params;
        //Clinical_ImmunizationDetail.params.mode = "Add";

        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#pnlClinicalImmunization #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        Clinical_ImmunizationDetail.params.PATIENTID = $('#PatientProfile #hfPatientId').val();
        if (Clinical_ImmunizationDetail.params.TabId == "Batch") {
            Clinical_ImmunizationDetail.params.PATIENTID = Clinical_ImmunizationDetail.params.patientID;
        }


        Clinical_ImmunizationDetail.params = params;


        if (Clinical_ImmunizationDetail.params.from == "Clinical_Treatment" && Clinical_ImmunizationDetail.params.mode == "Add") {
            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #ClinicalVaccineTabsDiv').addClass('disableAll');
        }
        else {
            $("#VaccineSchedulerDiv").remove();
        }


        var self = $('#pnlClinicalImmunization');

        if (Clinical_ImmunizationDetail.params != null && Clinical_ImmunizationDetail.params.ParentPanelID != "pnlClinicalImmunizationDetail") {
            Clinical_ImmunizationDetail.params.PanelID = Clinical_ImmunizationDetail.params.ParentPanelID + ' #pnlClinicalImmunizationDetail';
            self = $('#' + Clinical_ImmunizationDetail.params.PanelID);
        }
        else {

            Clinical_ImmunizationDetail.params["PanelID"] = "pnlClinicalImmunizationDetail"
            self = $('#' + Clinical_ImmunizationDetail.params.PanelID);
        }


        // ----------------------------------------- Creating DatePickers -------------------------------------

        Clinical_ImmunizationDetail.CreatingDatePickers();
        Clinical_ImmunizationDetail.CreatingTimePickers();

        if (Clinical_ImmunizationDetail.bIsFirstLoad) {
            if (Clinical_ImmunizationDetail.params.TabId == "HistoryDose") {
                Clinical_ImmunizationDetail.ValidateDoc();
            }
            else {
                Clinical_ImmunizationDetail.ValidateAdmini();
                Clinical_ImmunizationDetail.ValidateDoc();
                Clinical_ImmunizationDetail.ValidateRefusalForm();
            }
            Clinical_ImmunizationDetail.BindFacility('frmVaccineHxAdministerTabDetail');
            Clinical_ImmunizationDetail.BindFacility('frmVaccineRecordRefusalTabDetail');
            Clinical_ImmunizationDetail.bIsFirstLoad = false;
        }



        var data = null;
        self.loadDropDowns(true, data).done(function () {
            EMRUtility.setFavoriteSectionStyle(Clinical_ImmunizationDetail.params.PanelID);

            // -------------------------------  Clinical_ImmunizationDetail.validateImmunization();

            if ((Clinical_ImmunizationDetail.params["VaccineScheduleId"] == null || Clinical_ImmunizationDetail.params["VaccineScheduleId"] == "") && Clinical_ImmunizationDetail.params["OrderSetId"] == "" && Clinical_ImmunizationDetail.params["TabId"]
                != "HistoryDose" && Clinical_ImmunizationDetail.params["TabId"]
                != "Clinical_Treatment") {

                if (Clinical_ImmunizationDetail.params.mode == "Edit") {
                    Clinical_ImmunizationDetail.searchVaccineHx(Clinical_ImmunizationDetail.params.VaccineHxId).done(function (response) {

                        var selfAdminForm = $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail");
                        var selfDocumentForm = $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail");
                        var selfRefusalForm = $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail");
                        response = JSON.parse(response);
                        if (response.status != false) {

                            // Bind with Admin tab

                            if (Clinical_ImmunizationDetail.params.Type.trim() == "ADMINISTER") {
                                var details = JSON.parse(response.AdminVaccineHxLoad_JSON)[0];
                                if (details.AdministerVaccine_Category != "" || details.AdministerVaccine_Category != null) {
                                    $.when(Clinical_ImmunizationDetail.PopulateVaccineGroupCategory(true, "#ddlAdministerVaccine_Category", "GetAdministerVaccine_Category", null, Clinical_ImmunizationDetail.params.CategoryId)).then(function () {
                                        $('#' + Clinical_ImmunizationDetail.params.PanelID + " #hfVaccineHx").val(details.VaccineHxId);
                                        utility.bindMyJSONByName(true, details, false, selfAdminForm).done(function () {

                                            $.when(Clinical_ImmunizationDetail.PopulateVaccineDropDown(0, Clinical_ImmunizationDetail.params.Type.trim(), details.AdministerVaccine_Vaccine)).then(function () {
                                                $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #dpAdministerVaccine_AdministrationDate').datepicker('setDate', $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #dpAdministerVaccine_AdministrationDate').val());
                                                $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tpAdministerVaccine_AdministrationTime').timepicker('setTime', $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tpAdministerVaccine_AdministrationTime').val());
                                                $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #ddlAdministerVaccine_LotNumber').append("<option selected>" + details.LotText + "</option>");
                                                $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #pnlSection_Search').addClass('disableAll');
                                            });

                                        });
                                    });
                                }
                                else {
                                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #hfVaccineHx").val(details.VaccineHxId);
                                    utility.bindMyJSONByName(true, details, false, selfAdminForm).done(function () {

                                        $.when(Clinical_ImmunizationDetail.PopulateVaccineDropDown(0, Clinical_ImmunizationDetail.params.Type.trim(), details.AdministerVaccine_Vaccine)).then(function () {
                                            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #dpAdministerVaccine_AdministrationDate').datepicker('setDate', $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #dpAdministerVaccine_AdministrationDate').val());
                                            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tpAdministerVaccine_AdministrationTime').timepicker('setTime', $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tpAdministerVaccine_AdministrationTime').val());
                                            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #ddlAdministerVaccine_LotNumber').append("<option selected>" + details.LotText + "</option>");
                                            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #pnlSection_Search').addClass('disableAll');
                                        });

                                    });
                                }
                            }
                            else if (Clinical_ImmunizationDetail.params.Type.trim() == "DOCUMENTHX") {
                                var details = JSON.parse(response.DocVaccineHxLoad_JSON)[0];
                                $('#' + Clinical_ImmunizationDetail.params.PanelID + " #hfVaccineHx").val(details.VaccineHxId);
                                if (details.DocumentHxDose_Category != "" || details.DocumentHxDose_Category != null) {
                                    $.when(Clinical_ImmunizationDetail.PopulateVaccineGroupCategory(true, "#ddlDocumentHxDose_Category", "GetAdministerVaccine_Category", null, Clinical_ImmunizationDetail.params.CategoryId)).then(function () {
                                        utility.bindMyJSONByName(true, details, false, selfDocumentForm).done(function () {
                                            $.when(Clinical_ImmunizationDetail.PopulateVaccineDropDown(0, Clinical_ImmunizationDetail.params.Type.trim(), details.DocumentHxDose_Vaccine)).then(function () {
                                                $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #dpDocumentHxDose_AdministrationDate').datepicker('setDate', $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #dpDocumentHxDose_AdministrationDate').val());
                                                $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tpDocumentHxDose_AdministrationTime').timepicker('setTime', $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tpDocumentHxDose_AdministrationTime').val());
                                                $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #pnlSection_Search').addClass('disableAll');
                                            });

                                        });
                                    });
                                }
                                else {
                                    utility.bindMyJSONByName(true, details, false, selfDocumentForm).done(function () {
                                        $.when(Clinical_ImmunizationDetail.PopulateVaccineDropDown(0, Clinical_ImmunizationDetail.params.Type.trim(), details.DocumentHxDose_Vaccine)).then(function () {
                                            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #dpDocumentHxDose_AdministrationDate').datepicker('setDate', $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #dpDocumentHxDose_AdministrationDate').val());
                                            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tpDocumentHxDose_AdministrationTime').timepicker('setTime', $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tpDocumentHxDose_AdministrationTime').val());
                                            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #pnlSection_Search').addClass('disableAll');
                                        });

                                    });
                                }

                                $('#' + Clinical_ImmunizationDetail.params.PanelID + " #listAdministerVaccine").removeClass('active');
                                $('#' + Clinical_ImmunizationDetail.params.PanelID + " #tabAdministerVaccine").removeClass('active');
                                $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #listRecordRefusal').removeClass('active');
                                $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tabRecordRefusal').removeClass('active');
                                $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #listDocumentHxDose').addClass('active');
                                $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tabDocumentHxDose').addClass('active');
                            }
                            else if (Clinical_ImmunizationDetail.params.Type.trim() == "REFUSAL") {
                                var details = JSON.parse(response.RefusalVaccineLoad_JSON)[0];
                                $('#' + Clinical_ImmunizationDetail.params.PanelID + " #hfVaccineHx").val(details.VaccineHxId);
                                if (details.RecordRefusal_Category != "" || details.RecordRefusal_Category != null) {
                                    $.when(Clinical_ImmunizationDetail.PopulateVaccineGroupCategory(true, "#ddlRecordRefusal_Category", "GetAdministerVaccine_Category", null, Clinical_ImmunizationDetail.params.CategoryId)).then(function () {
                                        utility.bindMyJSONByName(true, details, false, selfRefusalForm).done(function () {

                                            $.when(Clinical_ImmunizationDetail.PopulateVaccineDropDown(0, Clinical_ImmunizationDetail.params.Type.trim(), details.RecordRefusal_Vaccine)).then(function () {
                                                $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #pnlSection_Search').addClass('disableAll');
                                            });

                                        });
                                    });
                                }
                                else {
                                    utility.bindMyJSONByName(true, details, false, selfRefusalForm).done(function () {
                                        $.when(Clinical_ImmunizationDetail.PopulateVaccineDropDown(0, Clinical_ImmunizationDetail.params.Type.trim(), details.RecordRefusal_Vaccine)).then(function () {
                                            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #pnlSection_Search').addClass('disableAll');
                                        });

                                    });
                                }

                                $('#' + Clinical_ImmunizationDetail.params.PanelID + " #listAdministerVaccine").removeClass('active');
                                $('#' + Clinical_ImmunizationDetail.params.PanelID + " #tabAdministerVaccine").removeClass('active');
                                $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #listRecordRefusal').addClass('active');
                                $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tabRecordRefusal').addClass('active');
                                $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #listDocumentHxDose').removeClass('active');
                                $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tabDocumentHxDose').removeClass('active');
                            }
                        }
                    });

                }









                //// Start//7/04/2016//Talha Tanweer//Serializing form
                //$('#' + Clinical_ImmunizationDetail.params.PanelID + ' #frmClinicalImmunization').data('serialize', $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #frmClinicalImmunization').serialize());
                ////  End//7/04/2016//Talha Tanweer//Serializing form


            }
            else if (Clinical_ImmunizationDetail.params.TabId == "HistoryDose") {
                $('#' + Clinical_ImmunizationDetail.params.PanelID + " #listAdministerVaccine").removeClass('active');
                $('#' + Clinical_ImmunizationDetail.params.PanelID + " #tabAdministerVaccine").removeClass('active');
                $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #listRecordRefusal').removeClass('active');
                $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tabRecordRefusal').removeClass('active');
                $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #listDocumentHxDose').addClass('active');
                $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tabDocumentHxDose').addClass('active');
                $('#' + Clinical_ImmunizationDetail.params.PanelID + " #listAdministerVaccine").addClass('hidden');
                $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #listRecordRefusal').addClass('hidden');
                $.when(Clinical_ImmunizationDetail.setPatientProvider()).then(function () {
                    if (Clinical_ImmunizationDetail.params.mode == "Add") {

                    }
                    else {
                        $.when(Clinical_ImmunizationDetail.PopulateVaccineGroupCategory(true, "#ddlDocumentHxDose_Category", "GetAdministerVaccine_Category", null, Clinical_ImmunizationDetail.params.CategoryId, false)).then(function () {
                            $("#" + Clinical_ImmunizationDetail.params.PanelID + " #favSectionDiv").hide();
                            var vaccineId = Clinical_ImmunizationDetail.params.VaccineHxId;
                            $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #chkAdministerVoidDose").attr("disabled", false);
                            $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail #chkDocumentHxVoidDose").attr("disabled", false);
                            $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail #chkRecordRefusalVoidDose").attr("disabled", false);
                            if (vaccineId > 0) {

                                Clinical_ImmunizationDetail.params.TabType;
                                Clinical_ImmunizationDetail.params.mode = "Edit";
                                Clinical_ImmunizationDetail.searchVaccineHx(vaccineId).done(function (response) {
                                    Clinical_ImmunizationDetail.bindVaccineHxDetails(response);
                                });
                            }

                        });
                    }
                });
            }
            else {
                if (!(Clinical_ImmunizationDetail.params.from == "Clinical_Treatment" && Clinical_ImmunizationDetail.params.mode == "Add")) {
                    Clinical_ImmunizationDetail.IntializeAllTabs();
                }
            }
        });

        if (EMRUtility.getFavListStatus(Clinical_ImmunizationDetail.FavListName)) {
            $('#' + Clinical_ImmunizationDetail.params.PanelID + " #favSectionDiv").addClass("toggledHor");
            $('#' + Clinical_ImmunizationDetail.params.PanelID + " #FormDiv").addClass("toggleHorContainer");
        }
        else {
            $('#' + Clinical_ImmunizationDetail.params.PanelID + " #favSectionDiv").removeClass("toggledHor");
            $('#' + Clinical_ImmunizationDetail.params.PanelID + " #FormDiv").removeClass("toggleHorContainer");
        }
    },

    BindFacility: function (formId) {
        var Ctrl = $("#" + Clinical_ImmunizationDetail.params.PanelID + " #" + formId + " #txtFacility");
        var hfCtrl = $("#" + Clinical_ImmunizationDetail.params.PanelID + " #" + formId + " #hfFacility");
        var func = function () { return utility.GetFacilityArray(Ctrl.val()) };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },
    OpenFacility: function (formId) {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = formId;
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Clinical_ImmunizationDetail";
        LoadActionPan('Admin_Facility', params);
    },
    IsAdministrationPeriodOver: function (VaccineScheduleId) {
        var dfd = $.Deferred();
        Clinical_ImmunizationDetail.IsAdministrationPeriodOver_DB_CALL(VaccineScheduleId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                dfd.response = response.IsOver;
                dfd.resolve();
            }
            else {
                dfd.resolve();
                utility.DisplayMessages(response.message, 3);
            }
        });
        return dfd;
    },

    IsAdministrationPeriodOver_DB_CALL: function (VaccineScheduleId) {
        var objData = new Object();
        objData["VaccineScheduleId"] = VaccineScheduleId;
        if (Clinical_ImmunizationDetail.params.TabId == "Batch") {
            objData["PatientId"] = Clinical_ImmunizationDetail.params.patientID;
        }
        else {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        }
        objData["commandType"] = "Is_Administration_Period_Over";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");
    },
    toggleMeAdministered: function(obj) {
        $("#" +Clinical_ImmunizationDetail.params.PanelID).find('.toggleMe').toggleClass('hidden');
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
        if ($($(obj).parent().parent()).attr("id") == "RefusalHiddenField") {
            $(obj).parent().parent().addClass("col-xs-12")
        }

    },

    searchVaccineHx: function (vaccineId) {

        var objData = new Object();
        if (Clinical_ImmunizationDetail.params.TabId == "Batch") {
            objData["PatientId"] = Clinical_ImmunizationDetail.params.patientID;
        }
        else {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        }

        objData["commandType"] = "Search_VacinehxDose";
        objData["VaccineHxId"] = vaccineId;

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");

    },

    getACtiveTabLidID: function () {
        var activeTabliId = $("#ulmmunizationDetailTabsItems ").find("li.active").attr('id');
        return activeTabliId;
    },

    ValidateAdmini: function () {
        $(' #pnlClinicalImmunizationDetail  #frmVaccineHxAdministerTabDetail')
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
                      AdministerVaccine_AdministrationDate: {
                          group: '.col-sm-4',
                          validators: {
                              notEmpty: {
                                  message: ''
                              }
                          }
                      },
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
                      AdministerVaccine_LotNumber: {
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


                  }
              }).on('click', '#chkAdministerVoidDose', function (e) {
                  var formValidation = $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #frmVaccineHxAdministerTabDetail').data("bootstrapValidator");
                  if (formValidation) {
                      var Control = $("#" + Clinical_ImmunizationDetail.params["PanelID"] + ' #frmVaccineHxAdministerTabDetail #chkAdministerVoidDose').prop("checked");
                      if (Control) {
                          formValidation.enableFieldValidators('AdministerVaccine_Comments', true);
                      }
                      else {
                          formValidation.enableFieldValidators('AdministerVaccine_Comments', false);
                      }
                  }
              }).on('success.form.bv', function (e) {
                  if (Clinical_ImmunizationDetail.getACtiveTabLidID() === "listAdministerVaccine") {
                      e.preventDefault();
                      //Clinical_ImmunizationDetail.VaccineSave('ADMINISTER');
                      Clinical_ImmunizationDetail.VaccineInsertUpdate('ADMINISTER');
                  }
              });



    },

    ValidateDoc: function () {
        $(' #pnlClinicalImmunizationDetail  #frmVaccineHxDocumentHxDoseTabDetail')
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
               var formValidation = $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #frmVaccineHxDocumentHxDoseTabDetail').data("bootstrapValidator");
               if (formValidation) {
                   var Control = $("#" + Clinical_ImmunizationDetail.params["PanelID"] + ' #frmVaccineHxDocumentHxDoseTabDetail #chkDocumentHxVoidDose').prop("checked");
                   if (Control) {
                       formValidation.enableFieldValidators('DocumentHxDose_Comments', true);
                   }
                   else {
                       formValidation.enableFieldValidators('DocumentHxDose_Comments', false);
                   }
               }
           })
                   .on('success.form.bv', function (e) {
                       if (Clinical_ImmunizationDetail.getACtiveTabLidID() === "listDocumentHxDose") {
                           e.preventDefault();
                           //Clinical_ImmunizationDetail.VaccineSave('DOCUMENTHX');
                           Clinical_ImmunizationDetail.VaccineInsertUpdate('DOCUMENTHX');
                       }
                   });



    },
    SetFavListVal: function ($ddl) {

        var FavOptionLength = $("#" + Clinical_ImmunizationDetail.params.PanelID + " #ddlFavorites option").length;

        if (FavOptionLength > 1) {

            EMRUtility.getFavListValue(Clinical_ImmunizationDetail.FavListName).done(function (response1) {

                response1 = JSON.parse(response1);

                if (response1.status != false) {

                    if (response1.favListVal != "") {

                        if ($("#" + Clinical_ImmunizationDetail.params.PanelID + " #ddlFavorites option[value='" + response1.favListVal + "']").length > 0) {
                            $ddl.val(response1.favListVal);
                            $ddl.trigger("onchange");
                        }
                        else {
                            if (FavOptionLength == 2) {
                                $ddl.val($("#" + Clinical_ImmunizationDetail.params.PanelID + " #ddlFavorites option:nth-child(2)").val());
                                $ddl.trigger("onchange");
                            }
                            else if (FavOptionLength > 2) {
                                $ddl.trigger("onchange");
                            }
                            else {
                                $ddl.trigger("onchange");
                            }
                        }
                    }
                    else {
                        if (FavOptionLength == 2) {
                            $ddl.val($("#" + Clinical_ImmunizationDetail.params.PanelID + " #ddlFavorites option:nth-child(2)").val());
                            $ddl.trigger("onchange");
                        }
                        else if (FavOptionLength > 2) {
                            $ddl.trigger("onchange");
                        }
                        else {
                            $ddl.trigger("onchange");
                        }
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    ValidateRefusalForm: function () {
        $('#pnlClinicalImmunizationDetail  #frmVaccineRecordRefusalTabDetail')
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
               var formValidation = $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #frmVaccineRecordRefusalTabDetail').data("bootstrapValidator");
               if (formValidation) {
                   var Control = $("#" + Clinical_ImmunizationDetail.params["PanelID"] + ' #frmVaccineRecordRefusalTabDetail #chkRecordRefusalVoidDose').prop("checked");
                   if (Control) {
                       formValidation.enableFieldValidators('RecordRefusal_Comments', true);
                   }
                   else {
                       formValidation.enableFieldValidators('RecordRefusal_Comments', false);
                   }
               }
           })
                   .on('success.form.bv', function (e) {
                       if (Clinical_ImmunizationDetail.getACtiveTabLidID() === "listRecordRefusal") {
                           e.preventDefault();
                           //Clinical_ImmunizationDetail.VaccineSave('REFUSAL');
                           Clinical_ImmunizationDetail.VaccineInsertUpdate('REFUSAL');
                       }
                   });




    },

    //Author: Talha Tanweer
    Include_DDL_Events_Explicitly: function () {
        $(document).on('change', this.ddlAdministerVaccine_Category, function () {
            this.PopulateVaccineDropDown($(this.ddlAdministerVaccine_Category).val(), this.AdministerText);
        });

        $(document).on('change', this.ddlDocumentHxDose_Category, function () {
            this.PopulateVaccineDropDown($(this.ddlDocumentHxDose_Category).val(), this.DocumentHxDoseText);
        });

        $(document).on('change', this.ddlAdministerVaccine_Vaccine, function () {
            this.PopulateVaccineManufacturerDropDown($(this.ddlAdministerVaccine_Vaccine).val(), this.AdministerText);
        });

        $(document).on('change', this.ddlDocumentHxDose_Vaccine, function () {
            this.PopulateVaccineManufacturerDropDown($(this.ddlDocumentHxDose_Vaccine).val(), this.DocumentHxDoseText);
        });


        //$(document).on('change', this.ddlAdministerVaccine_Vaccine, function() {
        //    $(this.ddlAdministerVaccine_Vaccine).val(), this.AdministerText;
        //});

        //$(document).on('change', this.ddlDocumentHxDose_Vaccine, $(this.ddlDocumentHxDose_Vaccine).val(), this.DocumentHxDoseText);

        //$(document).on('change', this.ddlDocumentHxDose_Category, $(this.ddlDocumentHxDose_Category).val(), this.DocumentHxDoseText);
    },



    getSelectedTabText: function () {
        var selectedId = $("#ulmmunizationDetailTabsItems ").find("li.active").attr('id');
        if (selectedId === "listAdministerVaccine") {
            return Clinical_ImmunizationDetail.AdministerText;
        } else if (selectedId === "listDocumentHxDose") {
            return Clinical_ImmunizationDetail.DocumentHxDoseText;
        }
    },

    //Author: Talha Tanweer
    LoadingDropDowns: function (isLoad, controlId, methodName, data) {

        var dfd = new $.Deferred();

        var contrainerid = "pnlClinicalImmunizationDetail";  //this[0].id;
        if (data == null || data == "undefined") {
            data = "";
        }

        var ddl = " " + controlId + " ";
        var getDataMethod = methodName;
        if (true) {
            return MDVisionService.lookups(getDataMethod, isLoad, data).done(function (results) {
                results = JSON.parse(results[getDataMethod]);
                if (results) {

                    var l = $(ddl);
                    if (l.id == "LedgerAccount") {
                        console.log(results);
                    }
                    l.empty();
                    $.each(results, function (j, result) {
                        if (result.Name == "" && (methodName == "GetPatientVisits" || methodName == "GetVaccineManufacturer")) {

                        }
                        else {
                            if (j == 0 && results.length == 1 && methodName == "GetVaccineLotNumber") {
                                l.append($("<option checked/>").val(result.Value).text(result.Name).attr("RefValue", result.RefValue).attr("RefName", result.RefName));

                                Clinical_ImmunizationDetail.LotNumberChange(result.Value);
                            }
                            else if (methodName == "GetAdministerVaccine_Category") {
                                l.append($("<option checked />").val(result.Value).text(result.Name).attr("RefValue", result.RefValue).attr("RefName", result.RefName));
                            }
                            else {
                                l.append($("<option />").val(result.Value).text(result.Name).attr("RefValue", result.RefValue).attr("RefName", result.RefName));
                            }

                        }
                    });
                    BackgroundLoaderShow(false);
                    dfd.resolve();
                }
            });
        }

        return dfd.promise();
    },

    PopulateVisitDate: function () {
        Clinical_ImmunizationDetail.NotesLoad().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.ClinicalNotesCount > 0) {

                    var NotesArray = response.NotesLoad_JSON;
                    var ddlVisitDate = $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #ddlAdministerVaccine_VisitDate');
                    $(ddlVisitDate).find('option').remove();
                    $(ddlVisitDate).append($('<option>', {
                        value: '',
                        text: '- Select -'
                    }));
                    for (var Note in NotesArray) {
                        var date = new Date(NotesArray[Note].VisitDate);
                        var displayValue = (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear();
                        displayValue = displayValue + ' ' + NotesArray[Note].VisitTime;


                        $(ddlVisitDate).append($('<option>', {
                            value: NotesArray[Note].NotesId,
                            text: displayValue
                        }))
                    }
                    if (Clinical_ImmunizationDetail.params.from == 'clinicalTabProgressNote' || Clinical_ImmunizationDetail.params.from == 'Clinical_Treatment') {
                        $(ddlVisitDate).val(Clinical_ProgressNote.params.NotesId);
                        $(ddlVisitDate).attr('disabled', true);
                    }

                }
                else {
                    var ddlVisitDate = $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #ddlAdministerVaccine_VisitDate');
                    $(ddlVisitDate).find('option').remove();
                    $(ddlVisitDate).append($('<option>', {
                        value: '',
                        text: '- Select -'
                    }));
                }

            }
            else {
                utility.DisplayMessages(strMessage, 3);
            }
        });
    },

    NotesLoad: function () {
        var objData = new Object();
        if (Clinical_ImmunizationDetail.params.patientID != null) {
            objData["PatientId"] = Clinical_ImmunizationDetail.params.patientID;
        }


        objData["commandType"] = "LOAD_CLINICAL_NOTES_DATES";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    //Author: Talha Tanweer
    PopulateVaccineGroupCategory: function (isLoad, controlId, methodName, data, CategoryId, NotCallWhyLotIsNotAva) {

        var dfd = new $.Deferred();
        var forModule = "";
        if (controlId === '#ddlAdministerVaccine_Category') {

            Clinical_ImmunizationDetail.Disable_and_RemoveDropDownItems('ddlAdministerVaccine_Vaccine');
            Clinical_ImmunizationDetail.Disable_and_RemoveDropDownItems('txtAdministerTabManufacturer');

            //if (Clinical_ImmunizationDetail.params.TabId == "Hx") {
            //    data = "StrID=1";
            //}
            //else if (Clinical_ImmunizationDetail.params.TabId == "Alert") {
            //    data = "StrID=1";
            //}
            //else if (Clinical_ImmunizationDetail.params.TabId == "Chart") {
            //    data = "StrID=1";
            //}
            //else {
            //    data = "StrID=1";
            //}


            forModule = "ADMINISTER";

            //data = "TabId=" + CategoryId;
            $('#ddlAdministerVaccine_Vaccine').children().remove();
            $('#ddlAdministerVaccine_Manufacturer').children().remove();
            $('#ddlAdministerVaccine_Vaccine').prop('disabled', true);
            $('#ddlAdministerVaccine_Manufacturer').prop('disabled', true);
        }
        else if (controlId === '#ddlDocumentHxDose_Category') {

            //if (Clinical_ImmunizationDetail.params.TabId == "Hx") {
            //    data = "StrID=0";
            //}
            //else if (Clinical_ImmunizationDetail.params.TabId == "Alert") {
            //    data = "StrID=0";
            //}
            //else if (Clinical_ImmunizationDetail.params.TabId == "Chart") {
            //    data = "StrID=0";
            //}
            //else {
            //    data = "StrID=0";
            //}

            Clinical_ImmunizationDetail.Disable_and_RemoveDropDownItems('ddlDocumentHxDose_Vaccine');
            Clinical_ImmunizationDetail.Disable_and_RemoveDropDownItems('ddlDocumentHxDose_Manufacturer');
            forModule = "DOCUMENTHX";

            $('#ddlDocumentHxDose_Vaccine').children().remove();
            $('#ddlDocumentHxDose_Manufacturer').children().remove();
            $('#ddlDocumentHxDose_Vaccine').prop('disabled', true);
            $('#ddlDocumentHxDose_Manufacturer').prop('disabled', true);
        }
        else if (controlId === '#ddlRecordRefusal_Category') {

            //if (Clinical_ImmunizationDetail.params.TabId == "Hx") {
            //    data = "StrID=0";
            //}
            //else if (Clinical_ImmunizationDetail.params.TabId == "Alert") {
            //    data = "StrID=0";
            //}
            //else if (Clinical_ImmunizationDetail.params.TabId == "Chart") {
            //    data = "StrID=0";
            //}
            //else {
            //    data = "StrID=0";
            //}

            Clinical_ImmunizationDetail.Disable_and_RemoveDropDownItems('ddlRecordRefusal_Vaccine');
            forModule = "REFUSAL";
        }

        //$.when(Clinical_ImmunizationDetail.LoadingDropDowns(isLoad, controlId, methodName, data)).then(function () {
        if (Clinical_ImmunizationDetail.params.TabId == "Hx" && Clinical_ImmunizationDetail.params.mode == "Add") {
            if ($('#' + Clinical_ImmunizationDetail.params.PanelID + " " + controlId).hasClass("disableAll")) {
                $('#' + Clinical_ImmunizationDetail.params.PanelID + " " + controlId).removeClass("disableAll");
            }
            Clinical_ImmunizationDetail.Disable_Manufacturer_VISDate_VISurl(forModule);
            dfd.resolve();
        }
        else {
            if (!$('#' + Clinical_ImmunizationDetail.params.PanelID + " " + controlId).hasClass("disableAll")) {
                $('#' + Clinical_ImmunizationDetail.params.PanelID + " " + controlId).addClass("disableAll");
            }
            if ((Clinical_ImmunizationDetail.params.TabId == "Hx" || Clinical_ImmunizationDetail.params.TabId == "HistoryDose" || Clinical_ImmunizationDetail.params.TabId == "Clinical_Treatment") && Clinical_ImmunizationDetail.params.mode == "Edit") {
                $("#" + Clinical_ImmunizationDetail.params.PanelID + " " + controlId).val(CategoryId);
            }
            else if (Clinical_ImmunizationDetail.params.TabId == "Alert" || Clinical_ImmunizationDetail.params.TabId == "Chart") {
                $("#" + Clinical_ImmunizationDetail.params.PanelID + " " + controlId).val(CategoryId);
            }
            else {
                if (Clinical_ImmunizationDetail.params.Category) {
                    $("#" + Clinical_ImmunizationDetail.params.PanelID + " " + controlId + ' option').filter(function () {
                        return $.trim($(this).text().toLowerCase()) == CategoryId.toLowerCase() && $(this).val() == Clinical_ImmunizationDetail.params.Category
                    }).prop('selected', true);
                }
                else {
                    $("#" + Clinical_ImmunizationDetail.params.PanelID + " " + controlId + ' option').filter(function () {
                        return $.trim($(this).text().toLowerCase()) == CategoryId.toLowerCase()
                    }).prop('selected', true);
                }

            }
            $.when(Clinical_ImmunizationDetail.PopulateVaccineDropDown($("#" + Clinical_ImmunizationDetail.params.PanelID + " " + controlId).val(), forModule, undefined, NotCallWhyLotIsNotAva)).then(function () {
                dfd.resolve();
            });
        }


        // });


        return dfd.promise();




    },



    //Author: Talha Tanweer
    PopulateVaccineDropDown: function (vaccineGroupCategoryId, forModule, selectedVaccineId, NotCallWhyLotIsNotAva) {
        if (Clinical_ImmunizationDetail.params.mode.toLowerCase() == "add" && Clinical_ImmunizationDetail.params.TabId == "HistoryDose") {
            Clinical_ImmunizationDetail.LoadFavVaccine(true);
        }
        var dfd = new $.Deferred();
        var controlId = "";
        if (forModule === Clinical_ImmunizationDetail.AdministerText) {
            var controlId = "ddlAdministerVaccine_Vaccine";
        }
        else if (forModule === Clinical_ImmunizationDetail.DocumentHxDoseText) {
            var controlId = "ddlDocumentHxDose_Vaccine";
        }
        else if (forModule === Clinical_ImmunizationDetail.REFUSALText) {
            var controlId = "ddlRecordRefusal_Vaccine";
        }

        if ((forModule === Clinical_ImmunizationDetail.AdministerText || forModule === Clinical_ImmunizationDetail.DocumentHxDoseText || forModule === Clinical_ImmunizationDetail.REFUSALText) && vaccineGroupCategoryId !== "") {

            // --------------------------- Actions -----------------------------
            $('#' + controlId).attr('disabled', false);
            if (forModule === Clinical_ImmunizationDetail.AdministerText) {
                Clinical_ImmunizationDetail.Disable_Manufacturer_VISDate_VISurl(forModule);
                Clinical_ImmunizationDetail.Disable_and_RemoveDropDownItems(Clinical_ImmunizationDetail.ddlAdministerVaccine_LotNumber);
            }
            // $("ddlAdministerVaccine_Vaccine").val("");
            // -------------------Get Paramters and Load Dropdown -------------
            if (Clinical_ImmunizationDetail.params.TabId == "Hx") {
                var data = "StrID=" + vaccineGroupCategoryId + "&StrID2=" + forModule;
            }
            else if (Clinical_ImmunizationDetail.params.TabId == "Alert") {
                var data = "StrID=" + vaccineGroupCategoryId + "&StrID2=" + forModule;
            }
            else {
                var data = "StrID=" + vaccineGroupCategoryId + "&StrID2=" + forModule;
            }

            Clinical_ImmunizationDetail.LoadingDropDowns(true, " #" + controlId, Clinical_ImmunizationDetail.LookupVaccineMethod, data).done(function () {

                if (selectedVaccineId != null && vaccineGroupCategoryId != "") {
                    if (forModule === Clinical_ImmunizationDetail.DocumentHxDoseText) {
                        $('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlDocumentHxDose_Vaccine").val(selectedVaccineId);
                        dfd.resolve();
                    }
                    else if (forModule === Clinical_ImmunizationDetail.AdministerText) {
                        $('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_Vaccine").val(selectedVaccineId);
                        $.when(Clinical_ImmunizationDetail.PopulateVISDate_VISURL_and_ManufacturerDropDown(selectedVaccineId, forModule, NotCallWhyLotIsNotAva)).then(function () {
                            dfd.resolve();
                        });
                    }
                    else if (forModule === Clinical_ImmunizationDetail.REFUSALText) {
                        $('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlRecordRefusal_Vaccine").val(selectedVaccineId);
                        dfd.resolve();
                    }
                    else {
                        dfd.resolve();
                    }
                }
                if (selectedVaccineId != null && vaccineGroupCategoryId == 0) {
                    if (forModule === Clinical_ImmunizationDetail.DocumentHxDoseText) {
                        $('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlDocumentHxDose_Vaccine").val(selectedVaccineId);
                        dfd.resolve();
                    }
                    else if (forModule === Clinical_ImmunizationDetail.AdministerText) {
                        $('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_Vaccine").val(selectedVaccineId);
                        dfd.resolve();
                    }
                    else if (forModule === Clinical_ImmunizationDetail.REFUSALText) {
                        $('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlRecordRefusal_Vaccine").val(selectedVaccineId);
                        dfd.resolve();
                    }
                    else {
                        dfd.resolve();
                    }
                }
                else {
                    if (forModule === Clinical_ImmunizationDetail.AdministerText) {
                        $.when(Clinical_ImmunizationDetail.PopulateVISDate_VISURL_and_ManufacturerDropDown($('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_Vaccine").val(), forModule, NotCallWhyLotIsNotAva)).then(function () {
                            dfd.resolve();
                        });
                    }
                    else {
                        if (forModule === "DOCUMENTHX") {
                            if ($('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlDocumentHxDose_Vaccine").val() != "") {
                                Immunization_ImmunizationAddImmInj.GetVaccineInformation('immunization', $('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlDocumentHxDose_Vaccine").val()).done(function (response) {
                                    $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #txtDocumentHxDose_Dose').val(response.Dose);
                                    $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #ddlDocumentHxDose_Amount').val(response.Amount);
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
            Clinical_ImmunizationDetail.Disable_Manufacturer_VISDate_VISurl(forModule);
            Clinical_ImmunizationDetail.Disable_and_RemoveDropDownItems(controlId);
            //  $('#' + controlId).attr('disabled', true);
            dfd.resolve();
        }
        return dfd;
    },

    //Author: Talha Tanweer
    Disable_Manufacturer_VISDate_VISurl: function (forModule) {
        //  $(forModule === this.AdministerText ? " #" + this.ddlAdministerVaccine_Vaccine : this.ddlDocumentHxDose_Vaccine).attr('disabled', true);
        var manufacturerddlId = forModule === this.AdministerText ? this.txtAdministerTabManufacturer : this.ddlDocumentHxDose_Manufacturer;
        Clinical_ImmunizationDetail.Disable_and_RemoveDropDownItems(manufacturerddlId);
        Clinical_ImmunizationDetail.Disable_and_RemoveDropDownItems(Clinical_ImmunizationDetail.ddlAdministerVaccine_LotNumber);

        //   $(forModule === this.AdministerText ? " #" + this.ddlAdministerVaccine_Manufacturer : this.ddlDocumentHxDose_Manufacturer).attr('disabled', true);
        if (forModule === this.AdministerText) {
            $(" #pnlClinicalImmunizationDetail #lnklblAdministerVaccine_VISURL").attr('disabled', true);
            $(" #pnlClinicalImmunizationDetail #lnklblAdministerVaccine_VISURL").attr("href", "#");
            $(" #dpAdministerVaccine_VISDate").val("");

            $(" #pnlClinicalImmunizationDetail #lnklblAdministerVaccine_LotNo").attr('disabled', true);

        }



    },

    //Author: Talha Tanweer
    PopulateVISDate_VISURL_and_ManufacturerDropDown: function (vaccineId, forModule, NotCallWhyLotIsNotAva) {
        var dfd = new $.Deferred();
        var controlId = forModule === Clinical_ImmunizationDetail.AdministerText ? "ddlAdministerVaccine_Vaccine" : "ddlDocumentHxDose_Vaccine";
        if ($(" #" + controlId).val() !== "") {
            $.when(Clinical_ImmunizationDetail.PopulateVaccineManufacturerDropDown(vaccineId, forModule)).then(function () {
                if (forModule === "ADMINISTER") {
                    $.when(Clinical_ImmunizationDetail.PopulateLotNumber(vaccineId, NotCallWhyLotIsNotAva)).then(function () {
                        $.when(Clinical_ImmunizationDetail.SetLotManufanucture($("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_LotNumber").val(), NotCallWhyLotIsNotAva)).then(function () {
                            Clinical_ImmunizationDetail.PopulateVISDateAndURL(vaccineId, forModule);
                            if (vaccineId != "") {
                                Immunization_ImmunizationAddImmInj.GetVaccineInformation('immunization', vaccineId).done(function (response) {
                                    $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #txtAdministerVaccine_Dose').val(response.Dose);
                                    $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #ddlAdministerVaccine_Amount').val(response.Amount);
                                    dfd.resolve();
                                });
                            }
                            else {
                                dfd.resolve();
                            }
                        });

                    });

                }
                else {
                    if (forModule === "DOCUMENTHX") {
                        if ($('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlDocumentHxDose_Vaccine").val()) {
                            Immunization_ImmunizationAddImmInj.GetVaccineInformation('immunization', $('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlDocumentHxDose_Vaccine").val()).done(function (response) {
                                $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #txtDocumentHxDose_Dose').val(response.Dose);
                                $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #ddlDocumentHxDose_Amount').val(response.Amount);
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
            });


        } else {
            Clinical_ImmunizationDetail.Disable_Manufacturer_VISDate_VISurl(forModule);
            dfd.resolve();
        }


        return dfd;
    },
    SetLotManufanucture: function (LotId) {
        var dfd = $.Deferred();
        if (LotId != "" && LotId != null) {
            Clinical_ImmunizationDetail.GetLotManufanucture_DB_CALL(LotId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    $("#frmVaccineHxAdministerTabDetail #hfAdministerTabManufacturer").val(response.ManufactureId);
                    $("#frmVaccineHxAdministerTabDetail #txtAdministerTabManufacturer").val(response.ManufacturerName);
                    dfd.resolve();
                }
                else {
                    dfd.resolve();
                    utility.DisplayMessages(response.message, 3);
                }
            });
        }
        else {
            $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_LotNumber").val("");
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

    PopulateLotNumber: function (vaccineId, ComeFormLoadFuntion) {
        var dfd = new $.Deferred();
        if ($(" #pnlClinicalImmunizationDetail #ddlAdministerVaccine_Provider").val() != "" && vaccineId != "") {
            $(" #pnlClinicalImmunizationDetail #lnklblAdministerVaccine_LotNo").attr('disabled', false);
            $(" #pnlClinicalImmunizationDetail #" + Clinical_ImmunizationDetail.ddlAdministerVaccine_LotNumber).attr('disabled', false);

            var data = "ID=" + vaccineId + "&ID2=" + $(" #pnlClinicalImmunizationDetail #ddlAdministerVaccine_Provider").val();
            $.when(Clinical_ImmunizationDetail.LoadingDropDowns(true, "#" + Clinical_ImmunizationDetail.ddlAdministerVaccine_LotNumber, Clinical_ImmunizationDetail.LookupFunctionForGetLotNumbers, data)).then(function () {
                if (typeof ComeFormLoadFuntion == typeof undefined || ComeFormLoadFuntion == null || ComeFormLoadFuntion == false) {
                    if ($(" #pnlClinicalImmunizationDetail #" + Clinical_ImmunizationDetail.ddlAdministerVaccine_LotNumber).find("option").length == 1) {
                        var checkEmpty = false;
                        if ($(" #pnlClinicalImmunizationDetail #" + Clinical_ImmunizationDetail.ddlAdministerVaccine_LotNumber).find("option:first").text() == "- Select -") {
                            checkEmpty = true;
                        }
                        //if()
                        if (checkEmpty) {
                            $.when(Clinical_ImmunizationDetail.WhyLotIsNotPopulateInDropDown(vaccineId, $(" #pnlClinicalImmunizationDetail #ddlAdministerVaccine_Provider").val(), "Vaccine")).then(function () {
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
                $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail").bootstrapValidator('revalidateField', 'AdministerVaccine_LotNumber');

            })
        }
        else {
            dfd.resolve();
        }
        return dfd;
    },

    //Author: Talha Tanweer
    PopulateVaccineManufacturerDropDown: function (vaccineId, forModule) {
        var dfd = new $.Deferred();
        var controlId = forModule === Clinical_ImmunizationDetail.AdministerText ? "txtAdministerTabManufacturer" : "ddlDocumentHxDose_Manufacturer";
        if ((forModule === Clinical_ImmunizationDetail.AdministerText || forModule === Clinical_ImmunizationDetail.DocumentHxDoseText) && vaccineId !== "") {
            //    // --------------------------- Actions -----------------------------
            $(' #' + controlId).prop('disabled', false);
            //    // -------------------Get Paramters and Load Dropdown -------------
            //    var data = "StrID=" + vaccineId + "&StrID2=" + forModule;
            //    $.when(Clinical_ImmunizationDetail.LoadingDropDowns(true, " #" + controlId, Clinical_ImmunizationDetail.LookupVaccineManufacturerMethod, data)).then(function () {
            dfd.resolve();
            //    });
            //} else {
            //Clinical_ImmunizationDetail.Disable_and_RemoveDropDownItems(controlId);
            //$(' #' + controlId).prop('disabled', true);
        }
        return dfd;
    },

    //Author: Talha Tanweer
    PopulateVISDate: function (vaccineId, forModule) {
        var objData = new Object();
        //objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        //objData["IsActive"] = IsActive;

        //objData["pageNumber"] = pageNumber;
        //objData["rowsPerPage"] = rowsPerPage;
        //objData["CvxCode"] = Clinical_Immunization.params.NotesId == null ? 0 : Clinical_Immunization.params.NotesId;
        //   vaccineId = "146";
        objData["VaccineID"] = vaccineId;
        objData["commandType"] = "Get_VISDate";

        var data = JSON.stringify(objData);
        MDVisionService.APIService(data, "MEDICAL", "Immunization").done(function (response) {
            response = JSON.parse(response);
            if (response != null && response.status === true) {
                $(' #dpAdministerVaccine_VISDate').val(response.VISDate);
                $("#" + Clinical_ImmunizationDetail.params.PanelID + " #VisDateId").val(response.VISId);
            }
            else {
                $(' #dpAdministerVaccine_VISDate').val("");
                $("#" + Clinical_ImmunizationDetail.params.PanelID + " #VisDateId").val(0);
            }
        });
        // $(" # " + Clinical_Immunization.dpAdministerVaccine_VISDate).val(visDate);

    },
    PopulateVISDateAndURL: function (vaccineId, forModule) {
        var objData = new Object();
        objData["VaccineID"] = vaccineId;
        objData["commandType"] = "get_visdate_and_visurl";
        var data = JSON.stringify(objData);
        MDVisionService.APIService(data, "MEDICAL", "Immunization").done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.VaccineVISCount > 0) {

                    var VaccineVIS_JSON = response.VaccineVIS_JSON[0];
                    $(' #dpAdministerVaccine_VISDate').val($.datepicker.formatDate('mm/dd/yy ', new Date(VaccineVIS_JSON.VISDate)));
                    $("#" + Clinical_ImmunizationDetail.params.PanelID + " #VisDateId").val(VaccineVIS_JSON.VaccineVISId);
                    $(" #pnlClinicalImmunizationDetail #lnklblAdministerVaccine_VISURL").attr("href", VaccineVIS_JSON.VISDocumentLink);
                    if ($(" #pnlClinicalImmunizationDetail #lnklblAdministerVaccine_VISURL").attr("href") !== "") {
                        $(" #pnlClinicalImmunizationDetail #lnklblAdministerVaccine_VISURL").attr('disabled', false);
                    }
                }
                else {
                    $(" #pnlClinicalImmunizationDetail #lnklblAdministerVaccine_VISURL").attr('disabled', true);
                    $(' #dpAdministerVaccine_VISDate').val("");
                    $("#" + Clinical_ImmunizationDetail.params.PanelID + " #VisDateId").val(0);
                }
            }
            else {
                $(" #pnlClinicalImmunizationDetail #lnklblAdministerVaccine_VISURL").attr('disabled', true);
                utility.DisplayMessages(response.Message, 3);
                $(' #dpAdministerVaccine_VISDate').val("");
                $("#" + Clinical_ImmunizationDetail.params.PanelID + " #VisDateId").val(0);
            }
        });
    },

    SetExpiryDateAndRoute: function (ExpiryDate, RouteId) {
        if (RouteId != "" && RouteId != "0") {
            $("#" + Clinical_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_Route").val(RouteId);
        }
        else {
            $("#" + Clinical_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_Route").val("");
        }
        if (ExpiryDate != "") {
            ExpiryDate = $.datepicker.formatDate('mm dd yy ', new Date(ExpiryDate));
            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #dpAdministerVaccine_ExpiryDate').val(ExpiryDate);
            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #dpAdministerVaccine_ExpiryDate').datepicker('setDate', $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #dpAdministerVaccine_ExpiryDate').val());
        }

    },

    //Author: Talha Tanweer
    PopulateVISURL: function (vaccineId, forModule) {
        if (forModule === Clinical_ImmunizationDetail.AdministerText) {
            var objData = new Object();
            objData["VaccineID"] = vaccineId;
            objData["commandType"] = "get_visurl";

            var data = JSON.stringify(objData);
            MDVisionService.APIService(data, "MEDICAL", "Immunization").done(function (response) {
                response = JSON.parse(response);
                if (response != null && response.VIS_url !== "") {
                    $(" #pnlClinicalImmunizationDetail #lnklblAdministerVaccine_VISURL").attr("href", response.VIS_url);
                    if ($(" #pnlClinicalImmunizationDetail #lnklblAdministerVaccine_VISURL").attr("href") !== "") {
                        $(" #pnlClinicalImmunizationDetail #lnklblAdministerVaccine_VISURL").attr('disabled', false);
                    }
                }
                else {
                    $(" #pnlClinicalImmunizationDetail #lnklblAdministerVaccine_VISURL").attr('disabled', true);
                }
            });
        }
    },

    //Author: Talha Tanweer
    PopulateUserME: function (ddlGivenById) {

        var objData = new Object();
        objData["commandType"] = "get_userme";

        var data = JSON.stringify(objData);
        MDVisionService.APIService(data, "MEDICAL", "Immunization").done(function (response) {
            response = JSON.parse(response);
            if (response != null) {
                $(' #pnlClinicalImmunizationDetail #' + ddlGivenById).val(response.userId);
            }
        });
    },

    //Author: Talha Tanweer
    CreatingDatePickers: function () {

        var datePickerArray = [
            "  #dpAdministerVaccine_VisitDate", "  #dpAdministerVaccine_AdministrationDate", " #dpAdministerVaccine_ExpiryDate",
            "  #dpAdministerVaccine_VISDate", "  #dpDocumentHxDose_AdministrationDate", "  #dpDocumentHxDose_ExpiryDate", " #dpRecordRefusalVaccine_ExpiryDate",

           //Start || 20 April, 2016 || ZeeshanAK || Changes for new fields
            " #dpAdministerVaccine_PIEffectiveDate", " #dpAdministerVaccine_PublicityExpiryDate", " #dpAdministerVaccine_IRSEffectiveDate", " #dpAdministerVaccine_PIEffectiveDate",
            " #dpDocumentHxDose_PublicityExpiryDate", " #dpDocumentHxDose_IRSEffectiveDate", " #dpDocumentHxDose_PIEffectiveDate"
            //End   || 20 April, 2016 || ZeeshanAK || Changes for new fields
            , " #dpDocumentVaccine_PublicityExpiryDate"
            , " #dpDocumentVaccine_IRSEffectiveDate"
            , " #dpDocumentVaccine_PIEffectiveDate"
            , " #dpRefusalVaccine_PublicityExpiryDate"
            , " #dpRefusalVaccine_IRSEffectiveDate"
            , " #dpRefusalVaccine_PIEffectiveDate"
        ];

        for (var i = 0; i < datePickerArray.length; i++) {
            if (datePickerArray[i] == " #dpAdministerVaccine_PublicityExpiryDate" || datePickerArray[i] == " #dpAdministerVaccine_PIEffectiveDate" || datePickerArray[i] == " #dpAdministerVaccine_IRSEffectiveDate" || datePickerArray[i] == " #dpAdministerVaccine_ExpiryDate" || datePickerArray[i] == " #dpDocumentHxDose_ExpiryDate" || datePickerArray[i] == " #dpRecordRefusalVaccine_ExpiryDate"
                || datePickerArray[i] == " #dpDocumentVaccine_PublicityExpiryDate"
                || datePickerArray[i] == " #dpDocumentVaccine_IRSEffectiveDate"
                || datePickerArray[i] == " #dpDocumentVaccine_PIEffectiveDate"
                || datePickerArray[i] == " #dpRefusalVaccine_PublicityExpiryDate"
                || datePickerArray[i] == " #dpRefusalVaccine_IRSEffectiveDate"
                || datePickerArray[i] == " #dpRefusalVaccine_PIEffectiveDate") {
                utility.CreateDatePicker(Clinical_ImmunizationDetail.params.PanelID + datePickerArray[i], function () { }, false);
            } else {
                utility.CreateDatePicker(Clinical_ImmunizationDetail.params.PanelID + datePickerArray[i], function () { }, true);
            }

        }
        //$("#date").datepicker('setDate', '01/26/2014');
    },

    //Author: Talha Tanweer
    CreatingTimePickers: function () {
        $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tpAdministerVaccine_AdministrationTime').timepicker('setTime', new Date().toLocaleTimeString());
        $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tpDocumentHxDose_AdministrationTime').timepicker('setTime', new Date().toLocaleTimeString());


    },

    //Author: Talha Tanweer
    Disable_and_RemoveDropDownItems: function (ddlId) {
        $(" #" + ddlId).children().remove();
        $(" #" + ddlId).attr('disabled', true);
    },

    UnLoad: function (caller) {

        if (Clinical_ImmunizationDetail.params["FromAdmin"] == "0") {
            if (Clinical_ImmunizationDetail.params != null && Clinical_ImmunizationDetail.params.ParentCtrl != null) {

                if (Clinical_ImmunizationDetail.params.ParentCtrl == 'clinicalTabImmunization') {
                    UnloadActionPan(Clinical_ImmunizationDetail.params["ParentCtrl"], "Clinical_ImmunizationDetail");
                } else {
                    Clinical_ImmunizationDetail.params.PanelID = Clinical_ImmunizationDetail.params.PanelID.replace(" #pnlClinicalImmunizationDetail", "");
                    UnloadActionPan(Clinical_ImmunizationDetail.params.ParentCtrl, 'Clinical_ImmunizationDetail', null, Clinical_ImmunizationDetail.params.PanelID);
                }

            }
            else
                UnloadActionPan(null, 'Clinical_ImmunizationDetail');
        }
        else {
            RemoveAdminTab();
        }

        //Clinical_ImmunizationDetail.params.TabType = "";
        //Clinical_ImmunizationDetail.params.VaccineHxId = "";
        ////var saveButtonisHidden = $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmClinicalImmunizationDetail #btnSaveOrder").hasClass("hidden");
        ////Start 31-03-2016 Talha Tanweer to close form after save
        ////   if (caller == 'saveExit' || saveButtonisHidden == true) {
        //UnloadActionPan(Clinical_ImmunizationDetail.params["ParentCtrl"], "Clinical_ImmunizationDetail");
        //     }
        //End 31-03-2016 Talha Tanweer to close form after save
        //else {
        //    utility.UnLoadDialog("frmClinicalImmunizationDetail", function () {
        //        UnloadActionPan(Clinical_ImmunizationDetail.params["ParentCtrl"], "Clinical_ImmunizationDetail", null, Clinical_ImmunizationDetail.params["PanelID"]);
        //    }, function () {
        //        UnloadActionPan(Clinical_ImmunizationDetail.params["ParentCtrl"], "Clinical_ImmunizationDetail");
        //    });
        //}
    },

    //Author: Talha Tanweer
    //Date  : 05/04/2016
    //Reason: Function will reset the Immunization detail form
    resetImmunizationDetail: function (tabId) {
        utility.myConfirm('Are you sure to reset all controls?', function () {


            if (tabId === "tabAdministerVaccine") {

                VaccineId = $('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_Vaccine").val();
                $('#' + Clinical_ImmunizationDetail.params.PanelID + " #" + tabId).find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select,[type=hidden]').each(function () {
                    if ($(this).attr("id") != "ddlAdministerVaccine_Category") {
                        Clinical_ImmunizationDetail.resetControlValue($(this));
                    }

                });
                $("#pnlClinicalImmunizationDetail #hfAdministerTabManufacturer").val("");
                Clinical_ImmunizationDetail.Disable_and_RemoveDropDownItems("txtAdministerTabManufacturer");
                Clinical_ImmunizationDetail.Disable_and_RemoveDropDownItems(Clinical_ImmunizationDetail.ddlAdministerVaccine_LotNumber);
                $(" #pnlClinicalImmunizationDetail #lnklblAdministerVaccine_VISURL").attr('disabled', true);
                $(" #pnlClinicalImmunizationDetail #lnklblAdministerVaccine_VISURL").attr("href", "#");
                $(" #pnlClinicalImmunizationDetail #lnklblAdministerVaccine_LotNo").attr('disabled', true);
                $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #dpAdministerVaccine_AdministrationDate').datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), new Date()));

                Clinical_ImmunizationDetail.CreatingTimePickers();

            }
            else if (tabId === "tabDocumentHxDose") {


                $('#' + Clinical_ImmunizationDetail.params.PanelID + " #" + tabId).find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select,[type=hidden]').each(function () {
                    if ($(this).attr("id") != "ddlDocumentHxDose_Category") {
                        Clinical_ImmunizationDetail.resetControlValue($(this));
                    }
                });

                $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #dpDocumentHxDose_AdministrationDate').datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), new Date()));
                Clinical_ImmunizationDetail.CreatingTimePickers();
            }
            else if (tabId === "tabRecordRefusal") {
                $('#' + Clinical_ImmunizationDetail.params.PanelID + " #" + tabId).find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select,[type=hidden]').each(function () {
                    if ($(this).attr("id") != "ddlRecordRefusal_Category") {
                        Clinical_ImmunizationDetail.resetControlValue($(this));
                    }
                });
            }
        }, function () { }, 'Confirm Reset');
    },

    //Author: Talha Tanweer
    //Date  : 05/04/2016
    //
    resetControlValue: function (obj) {
        var currentElementTagName = obj.tagName != null ? obj.tagName : obj.prop("tagName");
        if ($(obj).attr('type') == 'text' || currentElementTagName.toLowerCase() == 'textarea' || $(obj).attr('type') == 'hidden')
            $(obj).val('');
        if ($(obj).attr('type') == 'checkbox' || $(obj).attr('type') == 'radio') {

            if ($(obj).attr('type') == 'radio') {
                obj.checked = false;

                //var groupRadBtn = $("input[name='" + $(obj).attr('name') + "']");
                //if (groupRadBtn.length > 1) {
                //    $.each(groupRadBtn, function (i, item) {
                //        if ($(item).attr("id").toLowerCase().indexOf("no") > -1) {
                //            $(item).trigger("click");
                //        }
                //    });
                //}

            }
                //Start//28-03-2016//Talha Tanweer//fixed bug# EMR-573
            else if ($(obj).attr('type') == 'checkbox') {
                $(obj).attr('checked', false);
            }
            //End//28-03-2016//Talha Tanweer//fixed bug# EMR-573
        }

        if (currentElementTagName.toLowerCase() == 'select') {
            $(obj).find('option:selected').removeAttr('selected');
            //$(this).attr('selectedIndex', '-1');
            $(obj).find('option:eq(0)').attr('selected', 'selected');
        }
        if (currentElementTagName.toLowerCase() == 'ul') {
            $(obj).find('li.active').removeClass('active');
        }
    },

    SetDate: function (obj, Id) {
        if (Id != "dpAdministerVaccine_PIEffectiveDate" && Id != "dpDocumentVaccine_PIEffectiveDate" && Id != "dpRefusalVaccine_PIEffectiveDate") {
            if ($(obj).val() != "") {
                $('#' + Clinical_ImmunizationDetail.params.PanelID + " #" + Id).datepicker("setDate", $.datepicker.formatDate(date_format.replace('MM/dd/yy', ''), new Date()));

                //utility.CreateDatePicker(Clinical_ImmunizationDetail.params.PanelID + " #" + Id, function () { }, true);
            }
            else {
                $("#" + Clinical_ImmunizationDetail.params.PanelID + " #" + Id).val("");
            }
        }
        else {
            if ($(obj).prop("checked")) {
                $('#' + Clinical_ImmunizationDetail.params.PanelID + " #" + Id).datepicker("setDate", $.datepicker.formatDate(date_format.replace('MM/dd/yy', ''), new Date()));
                //utility.CreateDatePicker(Clinical_ImmunizationDetail.params.PanelID + " #" + Id, function () { }, true);
            }
            else {
                $("#" + Clinical_ImmunizationDetail.params.PanelID + " #" + Id).val("");
            }
        }
    },

    //---------------------------------------------------------------------------------------------------------------
    //-------------------------------------------- Start Save Region ------------------------------------------------
    //---------------------------------------------------------------------------------------------------------------

    //VaccineSave: function (forModule) {
    //    var PreFavVal = $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #ddlFavorites').val();
    //    //if (globalAppdata['AppUserName'] == DefaultUser) {
    //    //    $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmFavoriteComplaintsDetail #lstEntityId").enable = true;
    //    //    $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmFavoriteComplaintsDetail").bootstrapValidator('revalidateField', 'EntityId');
    //    //}

    //    //  if (Clinical_ImmunizationDetail.FavComplaints.length > 0) {
    //    //$("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmClinicalImmunization").bootstrapValidator('revalidateField', 'AdministerVaccine_AdministrationDate');


    //    if (globalAppdata.IsImmunizationAlert != "False") {
    //        //$(" #mainForm  li#ImmunizationAlert").show();
    //    }
    //    if (forModule === Clinical_ImmunizationDetail.AdministerText) {

    //        $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail").bootstrapValidator('revalidateField', 'AdministerVaccine_Category');
    //        $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail").bootstrapValidator('revalidateField', 'AdministerVaccine_Provider');
    //        $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail").bootstrapValidator('revalidateField', 'AdministerVaccine_Vaccine');
    //        $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail").bootstrapValidator('revalidateField', 'AdministerVaccine_AdministrationDate');
    //        $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail").bootstrapValidator('revalidateField', 'AdministerVaccine_Dose');
    //        $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail").bootstrapValidator('revalidateField', 'AdministerVaccine_LotNumber');
    //        var formValidation = $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #frmVaccineHxAdministerTabDetail').data("bootstrapValidator");
    //        var Control = $("#" + Clinical_ImmunizationDetail.params["PanelID"] + ' #frmVaccineHxAdministerTabDetail #chkAdministerVoidDose').prop("checked");
    //        if (Control) {
    //            formValidation.enableFieldValidators('AdministerVaccine_Comments', true);
    //            $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail").bootstrapValidator('revalidateField', 'AdministerVaccine_Comments');
    //        }
    //        else {
    //            formValidation.enableFieldValidators('AdministerVaccine_Comments', false);
    //        }

    //        if (Clinical_ImmunizationDetail.params.mode == "Edit") {
    //            $.when(UpdateProcedureData = Clinical_ImmunizationDetail.UpdateVaccineInProcedure($("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #chkAdministerVoidDose").prop("checked"), Clinical_ImmunizationDetail.params.VaccineHxId)).then(function () {

    //                if (UpdateProcedureData.response == 1) {
    //                    $.when(result = Clinical_ImmunizationDetail.GetAgeOfPatient()).then(function () {
    //                        Clinical_ImmunizationDetail.SaveAdministerVaccine(result.response).done(function (response) {

    //                            response = JSON.parse(response);
    //                            if (response.status != false) {
    //                                Clinical_ImmunizationDetail.SaveFavToggelStatus(PreFavVal);
    //                                if (Clinical_ImmunizationDetail.params.mode == "Add") {
    //                                    Clinical_ImmunizationDetail.InsertVaccineInProcedure($("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_Vaccine").val(), response.VaccineHxIdColumn);
    //                                }
    //                                else if ((Clinical_ImmunizationDetail.params.mode == "Edit") && (Clinical_ImmunizationDetail.previousVoid == true && $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #chkAdministerVoidDose").prop("checked") == false)) {
    //                                    Clinical_ImmunizationDetail.InsertVaccineInProcedure($("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_Vaccine").val(), response.VaccineHxIdColumn);
    //                                }
    //                                $.when(Immunization_AlertConfiguration.SetImmunizationAlertCount($('#PatientProfile #hfPatientId').val(), true)).then(function () {
    //                                    if (Clinical_ImmunizationDetail.params.TabId == "Alert") {
    //                                        if (Clinical_ImmunizationDetail.params.from == "Immunization_AlertPreview") {
    //                                            $.when(Immunization_AlertPreview.SearchAlerts()).then(function () {
    //                                                Clinical_ImmunizationDetail.UnLoad();
    //                                            });

    //                                        }
    //                                    }
    //                                    else if (Clinical_ImmunizationDetail.params.TabId == "Chart") {
    //                                        if (Clinical_ImmunizationDetail.params.from == "Clinical_SchedulerView") {
    //                                            $.when(Clinical_SchedulerView.SchedulerPreview()).then(function () {
    //                                                Clinical_ImmunizationDetail.UnLoad();
    //                                            });

    //                                        }
    //                                    }
    //                                    else if (Clinical_ImmunizationDetail.params.TabId != "Hx") {
    //                                        if (Clinical_ImmunizationDetail.params.from == "clinicalTabProgressNote") {
    //                                            Clinical_ImmunizationDetail.GetVaccineHxIds(response.VaccineHxIdColumn).done(function (resultVaccineHxIds) {
    //                                                $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(resultVaccineHxIds, "Administer", Clinical_ImmunizationDetail.params.patientID, false, false)).then(function () {
    //                                                    $.when(Clinical_Immunization.LoadSchedlerData(Clinical_ImmunizationDetail.params.TabId)).then(function () {

    //                                                        Clinical_ImmunizationDetail.UnLoad();
    //                                                    });
    //                                                });
    //                                            });
    //                                        }
    //                                        else {
    //                                            utility.DisplayMessages(response.message, 1);
    //                                            $.when(Clinical_Immunization.LoadSchedlerData(Clinical_ImmunizationDetail.params.TabId)).then(function () {

    //                                                Clinical_ImmunizationDetail.UnLoad();
    //                                            });
    //                                        }
    //                                    }

    //                                    else {
    //                                        if (Clinical_ImmunizationDetail.params.from == "clinicalTabProgressNote") {
    //                                            Clinical_ImmunizationDetail.GetVaccineHxIds(response.VaccineHxIdColumn).done(function (resultVaccineHxIds) {
    //                                                $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(resultVaccineHxIds, "Administer", Clinical_ImmunizationDetail.params.patientID, false, false)).then(function () {
    //                                                    $.when(Clinical_Immunization.ImmunizationSearch('', 1, 15)).then(function () {
    //                                                        Clinical_ImmunizationDetail.UnLoad();
    //                                                    });
    //                                                });
    //                                            });
    //                                        }
    //                                        else {
    //                                            utility.DisplayMessages(response.message, 1);
    //                                            $.when(Clinical_Immunization.ImmunizationSearch('', 1, 15)).then(function () {
    //                                                Clinical_ImmunizationDetail.UnLoad();
    //                                            });
    //                                        }

    //                                    }

    //                                });
    //                                //if (Clinical_ImmunizationDetail.params.ParentCtrl == "Clinical_Immunization") {
    //                                //    Clinical_ImmunizationDetail.getAdministerVaccineInfo(response.VaccineHxIdColumn, "Administer", Clinical_ImmunizationDetail.params.patientID, true);

    //                                //}
    //                                //else {
    //                                //    Clinical_Immunization.ImmunizationSearch();
    //                                //}
    //                                //Clinical_ImmunizationDetail.UnLoad('saveExit');
    //                            }
    //                            else {

    //                                utility.DisplayMessages(response.message, 3);
    //                            }
    //                        });
    //                    });
    //                }
    //                else {

    //                }

    //            });
    //        }
    //        else {
    //            $.when(result = Clinical_ImmunizationDetail.GetAgeOfPatient()).then(function () {
    //                Clinical_ImmunizationDetail.SaveAdministerVaccine(result.response).done(function (response) {

    //                    response = JSON.parse(response);
    //                    if (response.status != false) {
    //                        Clinical_ImmunizationDetail.SaveFavToggelStatus(PreFavVal);
    //                        if (Clinical_ImmunizationDetail.params.mode == "Add") {
    //                            Clinical_ImmunizationDetail.InsertVaccineInProcedure($("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_Vaccine").val(), response.VaccineHxIdColumn);
    //                        }
    //                        $.when(Immunization_AlertConfiguration.SetImmunizationAlertCount($('#PatientProfile #hfPatientId').val(), true)).then(function () {
    //                            if (Clinical_ImmunizationDetail.params.TabId == "Alert") {
    //                                if (Clinical_ImmunizationDetail.params.from == "Immunization_AlertPreview") {
    //                                    $.when(Immunization_AlertPreview.SearchAlerts()).then(function () {
    //                                        Clinical_ImmunizationDetail.UnLoad();
    //                                    });

    //                                }
    //                            }
    //                            else if (Clinical_ImmunizationDetail.params.TabId == "Chart") {
    //                                if (Clinical_ImmunizationDetail.params.from == "Clinical_SchedulerView") {
    //                                    $.when(Clinical_SchedulerView.SchedulerPreview()).then(function () {
    //                                        Clinical_ImmunizationDetail.UnLoad();
    //                                    });

    //                                }
    //                            }
    //                            else if (Clinical_ImmunizationDetail.params.TabId != "Hx") {
    //                                if (Clinical_ImmunizationDetail.params.from == "clinicalTabProgressNote") {
    //                                    Clinical_ImmunizationDetail.GetVaccineHxIds(response.VaccineHxIdColumn).done(function (resultVaccineHxIds) {
    //                                        $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(resultVaccineHxIds, "Administer", Clinical_ImmunizationDetail.params.patientID, false, false)).then(function () {
    //                                            $.when(Clinical_Immunization.LoadSchedlerData(Clinical_ImmunizationDetail.params.TabId)).then(function () {

    //                                                Clinical_ImmunizationDetail.UnLoad();
    //                                            });
    //                                        });
    //                                    });
    //                                }
    //                                else {
    //                                    utility.DisplayMessages(response.message, 1);
    //                                    $.when(Clinical_Immunization.LoadSchedlerData(Clinical_ImmunizationDetail.params.TabId)).then(function () {

    //                                        Clinical_ImmunizationDetail.UnLoad();
    //                                    });
    //                                }
    //                            }

    //                            else {
    //                                if (Clinical_ImmunizationDetail.params.from == "clinicalTabProgressNote") {
    //                                    Clinical_ImmunizationDetail.GetVaccineHxIds(response.VaccineHxIdColumn).done(function (resultVaccineHxIds) {
    //                                        $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(resultVaccineHxIds, "Administer", Clinical_ImmunizationDetail.params.patientID, false, false)).then(function () {
    //                                            $.when(Clinical_Immunization.ImmunizationSearch('', 1, 15)).then(function () {
    //                                                Clinical_ImmunizationDetail.UnLoad();
    //                                            });
    //                                        });
    //                                    });
    //                                }
    //                                else {
    //                                    utility.DisplayMessages(response.message, 1);
    //                                    $.when(Clinical_Immunization.ImmunizationSearch('', 1, 15)).then(function () {
    //                                        Clinical_ImmunizationDetail.UnLoad();
    //                                    });
    //                                }

    //                            }

    //                        });
    //                        //if (Clinical_ImmunizationDetail.params.ParentCtrl == "Clinical_Immunization") {
    //                        //    Clinical_ImmunizationDetail.getAdministerVaccineInfo(response.VaccineHxIdColumn, "Administer", Clinical_ImmunizationDetail.params.patientID, true);

    //                        //}
    //                        //else {
    //                        //    Clinical_Immunization.ImmunizationSearch();
    //                        //}
    //                        //Clinical_ImmunizationDetail.UnLoad('saveExit');
    //                    }
    //                    else {

    //                        utility.DisplayMessages(response.message, 3);
    //                    }
    //                });
    //            });

    //        }
    //    }
    //    else if (forModule === Clinical_ImmunizationDetail.DocumentHxDoseText) {

    //        $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail").bootstrapValidator('revalidateField', 'DocumentHxDose_Category');
    //        $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail").bootstrapValidator('revalidateField', 'DocumentHxDose_Vaccine');

    //        var formValidation = $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #frmVaccineHxDocumentHxDoseTabDetail').data("bootstrapValidator");
    //        var Control = $("#" + Clinical_ImmunizationDetail.params["PanelID"] + ' #frmVaccineHxDocumentHxDoseTabDetail #chkDocumentHxVoidDose').prop("checked");
    //        if (Control) {
    //            formValidation.enableFieldValidators('DocumentHxDose_Comments', true);
    //            $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail").bootstrapValidator('revalidateField', 'DocumentHxDose_Comments');
    //        }
    //        else {
    //            formValidation.enableFieldValidators('DocumentHxDose_Comments', false);
    //        }

    //        if (Clinical_ImmunizationDetail.params.mode == "Edit") {
    //            $.when(UpdateProcedureData = Clinical_ImmunizationDetail.UpdateVaccineInProcedure($("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail #chkDocumentHxVoidDose").prop("checked"), Clinical_ImmunizationDetail.params.VaccineHxId)).then(function () {

    //                if (UpdateProcedureData.response == 1) {
    //                    $.when(result = Clinical_ImmunizationDetail.GetAgeOfPatient()).then(function () {
    //                        Clinical_ImmunizationDetail.SaveVaccineHxDose(result.response).done(function (response) {
    //                            response = JSON.parse(response);
    //                            if (response.status != false) {
    //                                Clinical_ImmunizationDetail.SaveFavToggelStatus(PreFavVal);
    //                                if (Clinical_ImmunizationDetail.params.mode == "Add") {
    //                                    //Clinical_ImmunizationDetail.InsertVaccineInProcedure($("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail #ddlDocumentHxDose_Vaccine").val(), response.VaccineHxIdColumn);
    //                                }
    //                                else if ((Clinical_ImmunizationDetail.params.mode == "Edit") && (Clinical_ImmunizationDetail.previousVoid == true && $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail #chkDocumentHxVoidDose").prop("checked") == false)) {
    //                                    //Clinical_ImmunizationDetail.InsertVaccineInProcedure($("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail #ddlDocumentHxDose_Vaccine").val(), response.VaccineHxIdColumn);
    //                                }
    //                                $.when(Immunization_AlertConfiguration.SetImmunizationAlertCount($('#PatientProfile #hfPatientId').val(), true)).then(function () {
    //                                    if (Clinical_ImmunizationDetail.params.TabId == "Alert") {
    //                                        if (Clinical_ImmunizationDetail.params.from == "Immunization_AlertPreview") {


    //                                            $.when(Immunization_AlertPreview.SearchAlerts()).then(function () {
    //                                                Clinical_ImmunizationDetail.UnLoad();
    //                                            });

    //                                        }
    //                                    }
    //                                    else if (Clinical_ImmunizationDetail.params.TabId == "Chart") {
    //                                        if (Clinical_ImmunizationDetail.params.from == "Clinical_SchedulerView") {
    //                                            $.when(Clinical_SchedulerView.SchedulerPreview()).then(function () {
    //                                                Clinical_ImmunizationDetail.UnLoad();
    //                                            });

    //                                        }
    //                                    }
    //                                    else if (Clinical_ImmunizationDetail.params.TabId != "Hx") {
    //                                        if (Clinical_ImmunizationDetail.params.from == "clinicalTabProgressNote") {
    //                                            Clinical_ImmunizationDetail.GetVaccineHxIds(response.VaccineHxIdColumn).done(function (resultVaccineHxIds) {
    //                                                $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(resultVaccineHxIds, "DOCUMENTHX", Clinical_ImmunizationDetail.params.patientID, false, false)).then(function () {
    //                                                    $.when(Clinical_Immunization.LoadSchedlerData(Clinical_ImmunizationDetail.params.TabId)).then(function () {
    //                                                        Clinical_ImmunizationDetail.UnLoad();
    //                                                    });
    //                                                });
    //                                            });
    //                                        }
    //                                        else {
    //                                            utility.DisplayMessages(response.message, 1);
    //                                            $.when(Clinical_Immunization.LoadSchedlerData(Clinical_ImmunizationDetail.params.TabId)).then(function () {
    //                                                Clinical_ImmunizationDetail.UnLoad();
    //                                            });
    //                                        }
    //                                    }


    //                                    else {
    //                                        if (Clinical_ImmunizationDetail.params.from == "clinicalTabProgressNote") {
    //                                            Clinical_ImmunizationDetail.GetVaccineHxIds(response.VaccineHxIdColumn).done(function (resultVaccineHxIds) {
    //                                                $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(resultVaccineHxIds, "DOCUMENTHX", Clinical_ImmunizationDetail.params.patientID, false, false)).then(function () {
    //                                                    $.when(Clinical_Immunization.ImmunizationSearch('', 1, 15)).then(function () {
    //                                                        Clinical_ImmunizationDetail.UnLoad();
    //                                                    });
    //                                                });
    //                                            });
    //                                        }
    //                                        else {
    //                                            utility.DisplayMessages(response.message, 1);
    //                                            $.when(Clinical_Immunization.ImmunizationSearch('', 1, 15)).then(function () {
    //                                                Clinical_ImmunizationDetail.UnLoad();
    //                                            });
    //                                        }

    //                                    }
    //                                });
    //                                //if (Clinical_ImmunizationDetail.params.ParentCtrl == "Clinical_Immunization") {
    //                                //    Clinical_ImmunizationDetail.getAdministerVaccineInfo(response.VaccineHxIdColumn, "DOCUMENTHX", Clinical_ImmunizationDetail.params.patientID, true);

    //                                //}
    //                                //else {
    //                                //    Clinical_Immunization.ImmunizationSearch();
    //                                //}
    //                                //Clinical_ImmunizationDetail.UnLoad('saveExit');

    //                            }
    //                            else {
    //                                utility.DisplayMessages(response.message, 3);
    //                            }
    //                        });
    //                    });
    //                }
    //                else {

    //                }

    //            });
    //        }
    //        else {
    //            $.when(result = Clinical_ImmunizationDetail.GetAgeOfPatient()).then(function () {
    //                Clinical_ImmunizationDetail.SaveVaccineHxDose(result.response).done(function (response) {
    //                    response = JSON.parse(response);
    //                    if (response.status != false) {
    //                        Clinical_ImmunizationDetail.SaveFavToggelStatus(PreFavVal);
    //                        if (Clinical_ImmunizationDetail.params.mode == "Add") {
    //                            //Clinical_ImmunizationDetail.InsertVaccineInProcedure($("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail #ddlDocumentHxDose_Vaccine").val(), response.VaccineHxIdColumn);
    //                        }
    //                        $.when(Immunization_AlertConfiguration.SetImmunizationAlertCount($('#PatientProfile #hfPatientId').val(), true)).then(function () {
    //                            if (Clinical_ImmunizationDetail.params.TabId == "Alert") {
    //                                if (Clinical_ImmunizationDetail.params.from == "Immunization_AlertPreview") {


    //                                    $.when(Immunization_AlertPreview.SearchAlerts()).then(function () {
    //                                        Clinical_ImmunizationDetail.UnLoad();
    //                                    });

    //                                }
    //                            }
    //                            else if (Clinical_ImmunizationDetail.params.TabId == "Chart") {
    //                                if (Clinical_ImmunizationDetail.params.from == "Clinical_SchedulerView") {
    //                                    $.when(Clinical_SchedulerView.SchedulerPreview()).then(function () {
    //                                        Clinical_ImmunizationDetail.UnLoad();
    //                                    });

    //                                }
    //                            }
    //                            else if (Clinical_ImmunizationDetail.params.TabId != "Hx") {
    //                                if (Clinical_ImmunizationDetail.params.from == "clinicalTabProgressNote") {
    //                                    Clinical_ImmunizationDetail.GetVaccineHxIds(response.VaccineHxIdColumn).done(function (resultVaccineHxIds) {
    //                                        $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(resultVaccineHxIds, "DOCUMENTHX", Clinical_ImmunizationDetail.params.patientID, false, false)).then(function () {
    //                                            $.when(Clinical_Immunization.LoadSchedlerData(Clinical_ImmunizationDetail.params.TabId)).then(function () {
    //                                                Clinical_ImmunizationDetail.UnLoad();
    //                                            });
    //                                        });
    //                                    });
    //                                }
    //                                else {
    //                                    utility.DisplayMessages(response.message, 1);
    //                                    $.when(Clinical_Immunization.LoadSchedlerData(Clinical_ImmunizationDetail.params.TabId)).then(function () {
    //                                        Clinical_ImmunizationDetail.UnLoad();
    //                                    });
    //                                }
    //                            }


    //                            else {
    //                                if (Clinical_ImmunizationDetail.params.from == "clinicalTabProgressNote") {
    //                                    Clinical_ImmunizationDetail.GetVaccineHxIds(response.VaccineHxIdColumn).done(function (resultVaccineHxIds) {
    //                                        $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(resultVaccineHxIds, "DOCUMENTHX", Clinical_ImmunizationDetail.params.patientID, false, false)).then(function () {
    //                                            $.when(Clinical_Immunization.ImmunizationSearch('', 1, 15)).then(function () {
    //                                                Clinical_ImmunizationDetail.UnLoad();
    //                                            });
    //                                        });
    //                                    });
    //                                }
    //                                else {
    //                                    utility.DisplayMessages(response.message, 1);
    //                                    $.when(Clinical_Immunization.ImmunizationSearch('', 1, 15)).then(function () {
    //                                        Clinical_ImmunizationDetail.UnLoad();
    //                                    });
    //                                }

    //                            }
    //                        });
    //                        //if (Clinical_ImmunizationDetail.params.ParentCtrl == "Clinical_Immunization") {
    //                        //    Clinical_ImmunizationDetail.getAdministerVaccineInfo(response.VaccineHxIdColumn, "DOCUMENTHX", Clinical_ImmunizationDetail.params.patientID, true);

    //                        //}
    //                        //else {
    //                        //    Clinical_Immunization.ImmunizationSearch();
    //                        //}
    //                        //Clinical_ImmunizationDetail.UnLoad('saveExit');

    //                    }
    //                    else {
    //                        utility.DisplayMessages(response.message, 3);
    //                    }
    //                });
    //            });
    //        }


    //    }
    //    else if (forModule === Clinical_ImmunizationDetail.REFUSALText) {

    //        $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail").bootstrapValidator('revalidateField', 'RecordRefusal_Category');
    //        $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail").bootstrapValidator('revalidateField', 'RecordRefusal_Provider');
    //        $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail").bootstrapValidator('revalidateField', 'RecordRefusal_Vaccine');
    //        $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail").bootstrapValidator('revalidateField', 'RecordRefusalReason');

    //        var formValidation = $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #frmVaccineRecordRefusalTabDetail').data("bootstrapValidator");
    //        var Control = $("#" + Clinical_ImmunizationDetail.params["PanelID"] + ' #frmVaccineRecordRefusalTabDetail #chkRecordRefusalVoidDose').prop("checked");
    //        if (Control) {
    //            formValidation.enableFieldValidators('RecordRefusal_Comments', true);
    //            $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail").bootstrapValidator('revalidateField', 'RecordRefusal_Comments');
    //        }
    //        else {
    //            formValidation.enableFieldValidators('RecordRefusal_Comments', false);
    //        }

    //        if (Clinical_ImmunizationDetail.params.mode == "Edit") {
    //            $.when(UpdateProcedureData = Clinical_ImmunizationDetail.UpdateVaccineInProcedure($("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail #chkRecordRefusalVoidDose").prop("checked"), Clinical_ImmunizationDetail.params.VaccineHxId)).then(function () {

    //                if (UpdateProcedureData.response == 1) {
    //                    $.when(result = Clinical_ImmunizationDetail.GetAgeOfPatient()).then(function () {
    //                        Clinical_ImmunizationDetail.SaveVaccineRefusalRecord(result.response).done(function (response) {
    //                            response = JSON.parse(response);
    //                            if (response.status != false) {
    //                                Clinical_ImmunizationDetail.SaveFavToggelStatus(PreFavVal);
    //                                if (Clinical_ImmunizationDetail.params.mode == "Add") {
    //                                    //Clinical_ImmunizationDetail.InsertVaccineInProcedure($("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail #ddlRecordRefusal_Vaccine").val(), response.VaccineHxIdColumn);
    //                                }
    //                                else if ((Clinical_ImmunizationDetail.params.mode == "Edit") && (Clinical_ImmunizationDetail.previousVoid == true && $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail #chkRecordRefusalVoidDose").prop("checked") == false)) {
    //                                    //Clinical_ImmunizationDetail.InsertVaccineInProcedure($("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail #ddlRecordRefusal_Vaccine").val(), response.VaccineHxIdColumn);
    //                                }
    //                                $.when(Immunization_AlertConfiguration.SetImmunizationAlertCount($('#PatientProfile #hfPatientId').val(), true)).then(function () {
    //                                    if (Clinical_ImmunizationDetail.params.TabId == "Alert") {
    //                                        if (Clinical_ImmunizationDetail.params.from == "Immunization_AlertPreview") {
    //                                            $.when(Immunization_AlertPreview.SearchAlerts()).then(function () {
    //                                                Clinical_ImmunizationDetail.UnLoad();
    //                                            });

    //                                        }
    //                                    }
    //                                    else if (Clinical_ImmunizationDetail.params.TabId == "Chart") {
    //                                        if (Clinical_ImmunizationDetail.params.from == "Clinical_SchedulerView") {
    //                                            $.when(Clinical_SchedulerView.SchedulerPreview()).then(function () {
    //                                                Clinical_ImmunizationDetail.UnLoad();
    //                                            });

    //                                        }
    //                                    }
    //                                    else if (Clinical_ImmunizationDetail.params.TabId != "Hx") {
    //                                        if (Clinical_ImmunizationDetail.params.from == "clinicalTabProgressNote") {
    //                                            Clinical_ImmunizationDetail.GetVaccineHxIds(response.VaccineHxIdColumn).done(function (resultVaccineHxIds) {
    //                                                $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(resultVaccineHxIds, "Administer", Clinical_ImmunizationDetail.params.patientID, false, false)).then(function () {
    //                                                    $.when(Clinical_Immunization.LoadSchedlerData(Clinical_ImmunizationDetail.params.TabId)).then(function () {
    //                                                        Clinical_ImmunizationDetail.UnLoad();
    //                                                    });
    //                                                });
    //                                            });
    //                                        }
    //                                        else {
    //                                            utility.DisplayMessages(response.message, 1);
    //                                            $.when(Clinical_Immunization.LoadSchedlerData(Clinical_ImmunizationDetail.params.TabId)).then(function () {
    //                                                Clinical_ImmunizationDetail.UnLoad();
    //                                            });
    //                                        }
    //                                    }

    //                                    else {
    //                                        if (Clinical_ImmunizationDetail.params.from == "clinicalTabProgressNote") {
    //                                            Clinical_ImmunizationDetail.GetVaccineHxIds(response.VaccineHxIdColumn).done(function (resultVaccineHxIds) {
    //                                                $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(resultVaccineHxIds, "REFUSAL", Clinical_ImmunizationDetail.params.patientID, false, false)).then(function () {
    //                                                    $.when(Clinical_Immunization.ImmunizationSearch('', 1, 15)).then(function () {
    //                                                        Clinical_ImmunizationDetail.UnLoad();
    //                                                    });
    //                                                });
    //                                            });
    //                                        }
    //                                        else {
    //                                            utility.DisplayMessages(response.message, 1);
    //                                            $.when(Clinical_Immunization.ImmunizationSearch('', 1, 15)).then(function () {
    //                                                Clinical_ImmunizationDetail.UnLoad();
    //                                            });
    //                                        }

    //                                    }
    //                                });
    //                                //if (Clinical_ImmunizationDetail.params.ParentCtrl == "Clinical_Immunization") {
    //                                //    Clinical_ImmunizationDetail.getAdministerVaccineInfo(response.VaccineHxIdColumn, "DOCUMENTHX", Clinical_ImmunizationDetail.params.patientID, true);

    //                                //}
    //                                //else {
    //                                //    Clinical_Immunization.ImmunizationSearch();
    //                                //}
    //                                //Clinical_ImmunizationDetail.UnLoad('saveExit');

    //                            }
    //                            else {
    //                                utility.DisplayMessages(response.message, 3);
    //                            }
    //                        });
    //                    });
    //                }
    //                else {

    //                }

    //            });
    //        }
    //        else {
    //            $.when(result = Clinical_ImmunizationDetail.GetAgeOfPatient()).then(function () {
    //                Clinical_ImmunizationDetail.SaveVaccineRefusalRecord(result.response).done(function (response) {
    //                    response = JSON.parse(response);
    //                    if (response.status != false) {
    //                        Clinical_ImmunizationDetail.SaveFavToggelStatus(PreFavVal);
    //                        if (Clinical_ImmunizationDetail.params.mode == "Add") {
    //                            //Clinical_ImmunizationDetail.InsertVaccineInProcedure($("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail #ddlRecordRefusal_Vaccine").val(), response.VaccineHxIdColumn);
    //                        }
    //                        $.when(Immunization_AlertConfiguration.SetImmunizationAlertCount($('#PatientProfile #hfPatientId').val(), true)).then(function () {
    //                            if (Clinical_ImmunizationDetail.params.TabId == "Alert") {
    //                                if (Clinical_ImmunizationDetail.params.from == "Immunization_AlertPreview") {
    //                                    $.when(Immunization_AlertPreview.SearchAlerts()).then(function () {
    //                                        Clinical_ImmunizationDetail.UnLoad();
    //                                    });

    //                                }
    //                            }
    //                            else if (Clinical_ImmunizationDetail.params.TabId == "Chart") {
    //                                if (Clinical_ImmunizationDetail.params.from == "Clinical_SchedulerView") {
    //                                    $.when(Clinical_SchedulerView.SchedulerPreview()).then(function () {
    //                                        Clinical_ImmunizationDetail.UnLoad();
    //                                    });

    //                                }
    //                            }
    //                            else if (Clinical_ImmunizationDetail.params.TabId != "Hx") {
    //                                if (Clinical_ImmunizationDetail.params.from == "clinicalTabProgressNote") {
    //                                    Clinical_ImmunizationDetail.GetVaccineHxIds(response.VaccineHxIdColumn).done(function (resultVaccineHxIds) {
    //                                        $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(resultVaccineHxIds, "Administer", Clinical_ImmunizationDetail.params.patientID, false, false)).then(function () {
    //                                            $.when(Clinical_Immunization.LoadSchedlerData(Clinical_ImmunizationDetail.params.TabId)).then(function () {
    //                                                Clinical_ImmunizationDetail.UnLoad();
    //                                            });
    //                                        });
    //                                    });
    //                                }
    //                                else {
    //                                    utility.DisplayMessages(response.message, 1);
    //                                    $.when(Clinical_Immunization.LoadSchedlerData(Clinical_ImmunizationDetail.params.TabId)).then(function () {
    //                                        Clinical_ImmunizationDetail.UnLoad();
    //                                    });
    //                                }
    //                            }

    //                            else {
    //                                if (Clinical_ImmunizationDetail.params.from == "clinicalTabProgressNote") {
    //                                    Clinical_ImmunizationDetail.GetVaccineHxIds(response.VaccineHxIdColumn).done(function (resultVaccineHxIds) {
    //                                        $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(resultVaccineHxIds, "REFUSAL", Clinical_ImmunizationDetail.params.patientID, false, false)).then(function () {
    //                                            $.when(Clinical_Immunization.ImmunizationSearch('', 1, 15)).then(function () {
    //                                                Clinical_ImmunizationDetail.UnLoad();
    //                                            });
    //                                        });
    //                                    });
    //                                }
    //                                else {
    //                                    utility.DisplayMessages(response.message, 1);
    //                                    $.when(Clinical_Immunization.ImmunizationSearch('', 1, 15)).then(function () {
    //                                        Clinical_ImmunizationDetail.UnLoad();
    //                                    });
    //                                }

    //                            }
    //                        });
    //                        //if (Clinical_ImmunizationDetail.params.ParentCtrl == "Clinical_Immunization") {
    //                        //    Clinical_ImmunizationDetail.getAdministerVaccineInfo(response.VaccineHxIdColumn, "DOCUMENTHX", Clinical_ImmunizationDetail.params.patientID, true);

    //                        //}
    //                        //else {
    //                        //    Clinical_Immunization.ImmunizationSearch();
    //                        //}
    //                        //Clinical_ImmunizationDetail.UnLoad('saveExit');

    //                    }
    //                    else {
    //                        utility.DisplayMessages(response.message, 3);
    //                    }
    //                });
    //            });
    //        }
    //    }


    //},

    SaveFavToggelStatus: function (FavListVal) {
        var isFavListOpened = $('#' + Clinical_ImmunizationDetail.params.PanelID + " #favSectionDiv").hasClass("toggledHor");
        $.when(EMRUtility.insertUpdateFavListStatus(Clinical_ImmunizationDetail.FavListName, isFavListOpened)).then(function () {
            EMRUtility.insertUpdateFavListVal(Clinical_ImmunizationDetail.FavListName, FavListVal);
        });
    },

    //this function will get Administer Vaccine Soap Text and attach that to Progress note
    getAdministerVaccineInfo: function (AdministerVaccineId, type, PatientId, isGridUpdate, MakeProcedureSoapText) {
        var dfd = $.Deferred();
        if (AdministerVaccineId == null || AdministerVaccineId == '') {
            return false;
        }

        var splitAdministerVaccineId = AdministerVaccineId.split(",");
        var AdministerVaccineIdForImmunization = [];
        var AdministerVaccineIdForTherapeuticInjection = [];
        $.each(AdministerVaccineId.split(","), function (i, item) {
            if (item.indexOf("thera") > -1) {
                AdministerVaccineIdForTherapeuticInjection.push(item.replace("thera", ""));
            }
            else {
                AdministerVaccineIdForImmunization.push(item);
            }
        });

        Clinical_ImmunizationDetail.get_AdministerVaccine_ForSOAP(AdministerVaccineIdForImmunization.join(), AdministerVaccineIdForTherapeuticInjection.join(), PatientId).done(function (response) {
            response = JSON.parse(response);
            if (response.status == true) {

                if (MakeProcedureSoapText) {
                    var Vaccinesoap_JSON = response.Vaccines;
                    var TheraeuticInjectionLoad_JSON = response.Injections;
                    var VaccineHxIds = [];
                    var ImmTherInjectionIds = [];
                    $.each(Vaccinesoap_JSON, function (i, item) {
                        VaccineHxIds.push(item.VaccineHxId);
                    });
                    $.each(TheraeuticInjectionLoad_JSON, function (i, item) {
                        ImmTherInjectionIds.push(item.ImmTherInjectionId);
                    });
                    if (VaccineHxIds != "" || ImmTherInjectionIds != "") {
                        var ResponseprocedureIds;
                        $.when(ResponseprocedureIds = Clinical_ImmunizationDetail.MakeSoaptextOfProcedures(VaccineHxIds.join(), ImmTherInjectionIds.join())).then(function () {
                            if (ResponseprocedureIds.response != 0) {
                                Clinical_Procedures.getProceduresInfo(ResponseprocedureIds.response, true);
                            }
                            $.when(Clinical_ImmunizationDetail.createImmunizationBodyHTMLForSoap(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', AdministerVaccineId, true)).then(function () {
                                if (Clinical_Immunization.params != null && Clinical_Immunization.params.PanelID.indexOf('pnlClinicalImmunization') != -1 && isGridUpdate) {
                                    $.when(Clinical_Immunization.ImmunizationSearch()).then(function () {
                                        $.when(Clinical_Immunization.ImmunizationTherapeuticInjectionSearch()).then(function () {
                                            dfd.resolve();
                                        });
                                    });
                                }
                                else {
                                    dfd.resolve();
                                }
                            });
                        });
                    }
                }
                else {
                    $.when(Clinical_ImmunizationDetail.createImmunizationBodyHTMLForSoap(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', AdministerVaccineId, true)).then(function () {
                        if (Clinical_Immunization.params != null && Clinical_Immunization.params.PanelID.indexOf('pnlClinicalImmunization') != -1 && isGridUpdate) {
                            $.when(Clinical_Immunization.ImmunizationSearch()).then(function () {
                                $.when(Clinical_Immunization.ImmunizationTherapeuticInjectionSearch()).then(function () {
                                    dfd.resolve();
                                });
                            });
                        }
                        else {
                            dfd.resolve();
                        }
                    });
                }

                //End//04//01//2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
            }
            else {
                dfd.resolve();
                utility.DisplayMessages(response.Message, 3);
            }
        });
        return dfd.promise();
    },

    //This Function will check, if Problem Lists Soap is already attached in Progress note, if Problem Lists is not attached than it will create main divs to attach allergy
    checkImmunizationExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML Clinical_Immunization').length == 0) {

            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #PlanNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';


            $(CompnentSelector).first().append(' <li class="ImmunizationComponent" NoteComponentId="NCDummyId"> <header>' +
                '<Clinical_Immunization title="Immunization"  id="' + this.id + '" class="NotesComponent">' +
                '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'Immunization\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Immunization">Immunization</a><a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'Immunization\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a><a onclick="Clinical_ProgressNote.RemoveComponentTab(\'Immunization\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a></Clinical_Immunization> </header></li>');
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
                $(this).find('.closeBtn').removeClass('hidden');
                $(this).css('background', '#EAF1F8');
            });

            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
                $(this).find('.closeBtn').addClass('hidden');
                $(this).css('background', '#fff');
            });
        }
        else
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML Clinical_Immunization').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($('#' + Clinical_ProgressNote.params["PanelID"] + ' #hfNotesId').val());
    },

    //This Function is used to create SOAP html and append it to  Progress note
    createImmunizationBodyHTML: function (response, NoteHTMLCtrl, ImmunizationId, hideAlertMessage) {
        var dfd = $.Deferred();
        Clinical_ImmunizationDetail.checkImmunizationExists();
        var Vaccinesoap_JSON = JSON.parse(response.VaccineLoad_JSON);
        var TheraeuticInjectionLoad_JSON = [];
        if (response.TheraeuticInjectionCount > 0) {
            TheraeuticInjectionLoad_JSON = JSON.parse(response.TheraeuticInjectionLoad_JSON);
        }

        var $mainDivVital = $(document.createElement('div'));
        var $mainDivAdminister = $(document.createElement('div'));
        $mainDivAdminister.attr('id', "Section_VaccineAdminister");
        $mainDivAdminister.append('<h6 class="text-bold">Administration Vaccine</h6>' + "<div id='AllAdministerVaccine'></div>");
        var $mainDivDocumentHx = $(document.createElement('div'));
        $mainDivDocumentHx.attr('id', "Section_VaccineDocumentHx");
        $mainDivDocumentHx.append('<h6 class="text-bold">Document Hx Vaccine</h6>' + "<div id='AllDocumentHxVaccine'></div>");

        var $mainDivRefusal = $(document.createElement('div'));
        $mainDivRefusal.attr('id', "Section_VaccineRefusal");
        $mainDivRefusal.append('<h6 class="text-bold">Refusal Vaccine</h6>' + "<div id='AllRefusalVaccine'></div>");

        var $mainDivVoidDose = $(document.createElement('div'));
        $mainDivVoidDose.attr('id', "Section_VaccineVoidDose");
        $mainDivVoidDose.append('<h6 class="text-bold">Void Dose</h6>' + "<div id='AllVoidDoseVaccine'></div>");


        var $mainDivTherapeuticInjection = $(document.createElement('div'));
        $mainDivTherapeuticInjection.attr('id', "Section_TherapeuticInjection");
        $mainDivTherapeuticInjection.append('<h6 class="text-bold">Therapeutic Injection</h6>' + "<div id='AllTherapeuticInjection'></div>");

        var $mainDivTherapeuticInjectionHistory = $(document.createElement('div'));
        $mainDivTherapeuticInjectionHistory.attr('id', "Section_TherapeuticInjectionHistory");
        $mainDivTherapeuticInjectionHistory.append('<h6 class="text-bold">Therapeutic Injection History</h6>' + "<div id='AllTherapeuticInjectionHistory'></div>");


        var $DivNewTherapeuticInjection = $(document.createElement('div'));
        var $DivNewTherapeuticInjectionHistory = $(document.createElement('div'));
        var $DivNewAdminister = $(document.createElement('div'));
        var $DivNewDocumentHx = $(document.createElement('div'));
        var $DivNewRefusal = $(document.createElement('div'));
        var $DivNewVoidDose = $(document.createElement('div'));


        if ((Vaccinesoap_JSON == null || Vaccinesoap_JSON.length == 0) && (TheraeuticInjectionLoad_JSON == null || TheraeuticInjectionLoad_JSON.length == 0)) {
            $.when(Clinical_ProgressNote.saveComponentSOAPText("Immunization", hideAlertMessage)).then(function () {
                dfd.resolve();
            });
            return dfd.promise();
        }
        if (response.VaccineCount > 0 || response.TheraeuticInjectionCount > 0) {
            var PListId = [];
            var def = [];

            $.each(Vaccinesoap_JSON, function (index, element) {
                var color = "";


                var PLid = element.VaccineHxId;
                var $SectionBodyVital = $(document.createElement('section'));
                $SectionBodyVital.attr('id', "Cli_Immunization_Main" + PLid);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_Immunization_" + PLid);
                var $ListVital = $(document.createElement('ul'));

                $ListVital.attr('class', 'list-unstyled')

                $SectionBodyVital.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Immunization_" + PLid + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Immunization_Main" + PLid + '"  ><i class="fa fa-times"></i></a></div> ');

                'Patient underwent <ProcedureCode ProcedureName> based on the following assessment: <Diagnosis> from <From> to <To>. Comments: <Comments>'
                if (element.Type.toLowerCase() == "administer" && element.VoidDose == "False") {
                    $ListVital.append("<li>" +
                        (element.VaccineName == '' ? "" : "<b>" + element.VaccineName + "</b>") +
                       (element.Dose == '' ? "" : ", Dose: " + element.Dose + " " + element.Amount) +
                         (!(element.RouteDescription) ? "" : ", Route: " + element.RouteDescription) +
                         (!(element.SiteDescription) ? "" : ", Site: " + element.SiteDescription) +
                          (!(element.LotNumber) ? "" : ", Lot Number: " + element.LotNumber) +
                          (!(element.ManufacturerName) ? "" : ", Manufacturer: " + element.ManufacturerName) +
                        (element.ProviderName == '' ? "" : ", administrated by " + element.ProviderName) +
                        (element.AdministrationDate == '' ? "" : " on " + (element.AdministrationDate).split(" ")[0])
                        );
                }
                else if (element.Type.toLowerCase() == "documenthx" && element.VoidDose == "False") {
                    $ListVital.append("<li>" +
                        (element.VaccineName == '' ? "" : "<b>" + element.VaccineName + "</b>") +
                        (element.Dose == '' ? "" : ", Dose: " + element.Dose + " " + element.Amount) +
                         (!(element.RouteDescription) ? "" : ", Route: " + element.RouteDescription) +
                         (!(element.SiteDescription) ? "" : ", Site: " + element.SiteDescription) +
                          (!(element.LotNumber) ? "" : ", Lot Number: " + element.LotNumber) +
                          (!(element.ManufacturerName) ? "" : ", Manufacturer: " + element.ManufacturerName) +
                        (element.ProviderName == '' ? "" : ", administrated by " + element.ProviderName) +
                        (element.AdministrationDate == '' ? "" : " on " + (moment(element.AdministrationDate).format("MM/DD/YYYY, hh:mm:ss A")))
                        );
                }
                else if (element.Type.toLowerCase() == "refusal" && element.VoidDose == "False") {
                    $ListVital.append("<li>" +
                        (element.VaccineName == '' ? "" : "<b>" + element.VaccineName + "</b>") +
                        (element.RefusalReason == '' ? "" : ", not given to the patient due to " + element.RefusalReason)
                        );
                }
                else if (element.VoidDose == "True") {
                    $ListVital.append("<li>" +
                        (element.VaccineName == '' ? "" : "<b>" + element.VaccineName + "</b>") +
                        (element.Dose == '' ? "" : ", Dose: " + element.Dose + " " + element.Amount) +
                         (!(element.RouteDescription) ? "" : ", Route: " + element.RouteDescription) +
                         (!(element.SiteDescription) ? "" : ", Site: " + element.SiteDescription) +
                          (!(element.LotNumber) ? "" : ", Lot Number: " + element.LotNumber) +
                          (!(element.ManufacturerName) ? "" : ", Manufacturer: " + element.ManufacturerName) +
                        (element.ProviderName == '' ? "" : ", administrated by " + element.ProviderName) +
                        (element.AdministrationDate == '' ? "" : " on " + (moment(element.AdministrationDate).format("MM/DD/YYYY, hh:mm:ss A"))) +
                        (element.Comments == '' ? "" : "</br> Reason: " + element.Comments)
                        );
                }


                $DetailsDiv.append($ListVital);
                $SectionBodyVital.append($DetailsDiv);

                var MainDivId = "";
                if (element.Type.toLowerCase() == "administer" && element.VoidDose == "False") {
                    MainDivId = "AllAdministerVaccine";
                }
                else if (element.Type.toLowerCase() == "documenthx" && element.VoidDose == "False") {
                    MainDivId = "AllDocumentHxVaccine";
                }
                else if (element.Type.toLowerCase() == "refusal" && element.VoidDose == "False") {
                    MainDivId = "AllRefusalVaccine";
                }
                else if (element.VoidDose == "True") {
                    MainDivId = "AllVoidDoseVaccine";
                }

                if ($(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#' + MainDivId + ' #Cli_Immunization_Main' + PLid).length == 0) {
                    PListId.push(PLid);
                    if (element.Type.toLowerCase() == "administer" && element.VoidDose == "False") {
                        def.push(
                        $.when(Clinical_ImmunizationDetail.RemoveIfExistINAnyOtherVaccineType(NoteHTMLCtrl, PLid)).then(function () {
                            $DivNewAdminister.append($SectionBodyVital);
                        })
                        )

                    }
                    else if (element.Type.toLowerCase() == "documenthx" && element.VoidDose == "False") {

                        def.push($.when(Clinical_ImmunizationDetail.RemoveIfExistINAnyOtherVaccineType(NoteHTMLCtrl, PLid)).then(function () {
                            $DivNewDocumentHx.append($SectionBodyVital);
                        }))
                    }
                    else if (element.Type.toLowerCase() == "refusal" && element.VoidDose == "False") {
                        def.push($.when(Clinical_ImmunizationDetail.RemoveIfExistINAnyOtherVaccineType(NoteHTMLCtrl, PLid)).then(function () {
                            $DivNewRefusal.append($SectionBodyVital);
                        }))
                    }
                    else if (element.VoidDose == "True") {
                        def.push($.when(Clinical_ImmunizationDetail.RemoveIfExistINAnyOtherVaccineType(NoteHTMLCtrl, PLid)).then(function () {
                            $DivNewVoidDose.append($SectionBodyVital);
                        }))
                    }


                } else {
                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + PLid + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + PLid + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + PLid).html($SectionBodyVital.html());
                    $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + PLid + ' ul').append(CommentHTML);
                }


                if (Vaccinesoap_JSON.length == index + 1) {

                }

            });

            $.when.apply($, def).done(function ($n) {
                if (response.TheraeuticInjectionCount > 0) {

                    $.each(TheraeuticInjectionLoad_JSON, function (index, element1) {
                        var color = "";

                        var TheraInjectionid = element1.ImmTherInjectionId + "thera";
                        var $SectionBodyVital = $(document.createElement('section'));
                        $SectionBodyVital.attr('id', "Cli_Immunization_Main" + TheraInjectionid);
                        var $DetailsDiv = $(document.createElement('div'));
                        $DetailsDiv.attr('id', "Cli_Immunization_" + TheraInjectionid);
                        var $ListVital = $(document.createElement('ul'));

                        $ListVital.attr('class', 'list-unstyled')

                        $SectionBodyVital.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Immunization_" + TheraInjectionid + '"><i class="fa fa-edit"></i></a>' +
                            '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Immunization_Main" + TheraInjectionid + '"  ><i class="fa fa-times"></i></a></div> ');
                        var _InjecNameAndCode = (element1.CPTCode == '' ? (element1.TherapeuticInjection == '' ? "" : element1.TherapeuticInjection) : element1.CPTCode + " - " + element1.TherapeuticInjection);

                        'Patient underwent <ProcedureCode ProcedureName> based on the following assessment: <Diagnosis> from <From> to <To>. Comments: <Comments>'
                        $ListVital.append("<li>" + "<b>" + _InjecNameAndCode + "</b>" +
                       (element1.Dose == '' ? "" : ", Dose: " + element1.Dose + " " + element1.Amount) +
                         (!(element1.RouteDescription) ? "" : ", Route: " + element1.RouteDescription) +
                         (!(element1.SiteDescription) ? "" : ", Site: " + element1.SiteDescription) +
                          (!(element1.LotNumber) ? "" : ", Lot Number: " + element1.LotNumber) +
                          (!(element1.ManufacturerName) ? "" : ", Manufacturer: " + element1.ManufacturerName) +
                        (element1.ProviderName == '' ? "" : ", administrated by " + element1.ProviderName) +
                         (element1.AdministrationDate == '' ? "" : " on " + (moment(element1.AdministrationDate).format("MM/DD/YYYY, hh:mm:ss A")))
                       );



                        $DetailsDiv.append($ListVital);
                        $SectionBodyVital.append($DetailsDiv);

                        var MainDivId = "";
                        if (element1.Type == "Administered") {
                            MainDivId = "AllTherapeuticInjection";
                            if ($(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#' + MainDivId + ' #Cli_Immunization_Main' + TheraInjectionid).length == 0) {
                                PListId.push(TheraInjectionid);
                                $DivNewTherapeuticInjection.append($SectionBodyVital);
                            } else {
                                var CommentHTML = "";
                                var CommentsID = $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + TheraInjectionid + ' ul li:Last').attr('id');
                                if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                                    CommentHTML = $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + TheraInjectionid + ' ul li:Last').get(0).outerHTML;
                                }
                                $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + TheraInjectionid).html($SectionBodyVital.html());
                                $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + TheraInjectionid + ' ul').append(CommentHTML);
                            }

                        }
                        else {
                            MainDivId = "AllTherapeuticInjectionHistory";
                            if ($(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#' + MainDivId + ' #Cli_Immunization_Main' + TheraInjectionid).length == 0) {
                                PListId.push(TheraInjectionid);
                                $DivNewTherapeuticInjectionHistory.append($SectionBodyVital);
                            } else {
                                var CommentHTML = "";
                                var CommentsID = $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + TheraInjectionid + ' ul li:Last').attr('id');
                                if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                                    CommentHTML = $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + TheraInjectionid + ' ul li:Last').get(0).outerHTML;
                                }
                                $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + TheraInjectionid).html($SectionBodyVital.html());
                                $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + TheraInjectionid + ' ul').append(CommentHTML);
                            }
                        }

                    });

                    //Start//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
                    if (PListId.join(",") != "") {
                        ImmunizationId = PListId.join(",");
                    }
                    //End//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
                    if ($DivNewTherapeuticInjectionHistory.html() != '' || $DivNewTherapeuticInjection.html() != '' || $DivNewAdminister.html() != '' || $DivNewDocumentHx.html() != '' || $DivNewRefusal.html() != '' || $DivNewVoidDose.html() != '') {

                        if ($DivNewAdminister.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineAdminister').length == 0) {
                                $mainDivVital.append($mainDivAdminister);
                                $mainDivVital.find('#AllAdministerVaccine').append($DivNewAdminister);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineAdminister')[0].outerHTML);
                                $mainDivVital.find('#AllAdministerVaccine').append($DivNewAdminister);
                            }
                        }
                        else {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineAdminister').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineAdminister')[0].outerHTML);
                            }
                        }
                        if ($DivNewDocumentHx.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineDocumentHx').length == 0) {
                                $mainDivVital.append($mainDivDocumentHx);
                                $mainDivVital.find('#AllDocumentHxVaccine').append($DivNewDocumentHx);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineDocumentHx')[0].outerHTML);
                                $mainDivVital.find('#AllDocumentHxVaccine').append($DivNewDocumentHx);
                            }
                        } else {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineDocumentHx').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineDocumentHx')[0].outerHTML);
                            }
                        }

                        if ($DivNewRefusal.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineRefusal').length == 0) {
                                $mainDivVital.append($mainDivRefusal);
                                $mainDivVital.find('#AllRefusalVaccine').append($DivNewRefusal);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineRefusal')[0].outerHTML);
                                $mainDivVital.find('#AllRefusalVaccine').append($DivNewRefusal);
                            }
                        } else {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineRefusal').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineRefusal')[0].outerHTML);
                            }
                        }

                        if ($DivNewVoidDose.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineVoidDose').length == 0) {
                                $mainDivVital.append($mainDivVoidDose);
                                $mainDivVital.find('#AllVoidDoseVaccine').append($DivNewVoidDose);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineVoidDose')[0].outerHTML);
                                $mainDivVital.find('#AllVoidDoseVaccine').append($DivNewVoidDose);
                            }
                        } else {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineVoidDose').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineVoidDose')[0].outerHTML);
                            }
                        }
                        if ($DivNewTherapeuticInjection.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_TherapeuticInjection').length == 0) {
                                $mainDivVital.append($mainDivTherapeuticInjection);
                                $mainDivVital.find('#AllTherapeuticInjection').append($DivNewTherapeuticInjection);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_TherapeuticInjection')[0].outerHTML);
                                $mainDivVital.find('#AllTherapeuticInjection').append($DivNewTherapeuticInjection);
                            }
                        }
                        else {
                            if ($(NoteHTMLCtrl).find('#Section_TherapeuticInjection').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_TherapeuticInjection')[0].outerHTML);
                            }
                        }
                        if ($DivNewTherapeuticInjectionHistory.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_TherapeuticInjectionHistory').length == 0) {
                                $mainDivVital.append($mainDivTherapeuticInjectionHistory);
                                $mainDivVital.find('#AllTherapeuticInjectionHistory').append($DivNewTherapeuticInjectionHistory);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_TherapeuticInjectionHistory')[0].outerHTML);
                                $mainDivVital.find('#AllTherapeuticInjectionHistory').append($DivNewTherapeuticInjectionHistory);
                            }
                        }
                        else {
                            if ($(NoteHTMLCtrl).find('#Section_TherapeuticInjectionHistory').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_TherapeuticInjectionHistory')[0].outerHTML);
                            }
                        }
                        //Start//04//01//2015//Ahmad Raza//
                        $.when(Clinical_ImmunizationDetail.updateImmunizationHtml($mainDivVital.html(), ImmunizationId, NoteHTMLCtrl, hideAlertMessage)).then(function () {
                            dfd.resolve();
                        });
                    } else {
                        $.when(Clinical_ImmunizationDetail.updateImmunizationHtml('', ImmunizationId, NoteHTMLCtrl, hideAlertMessage)).then(function () {
                            dfd.resolve();
                        });
                    }


                }
                else {

                    //Start//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
                    if (PListId.join(",") != "") {
                        ImmunizationId = PListId.join(",");
                    }
                    //End//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
                    if ($DivNewAdminister.html() != '' || $DivNewDocumentHx.html() != '' || $DivNewRefusal.html() != '' || $DivNewVoidDose.html() != '') {
                        if ($DivNewAdminister.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineAdminister').length == 0) {
                                $mainDivVital.append($mainDivAdminister);
                                $mainDivVital.find('#AllAdministerVaccine').append($DivNewAdminister);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineAdminister')[0].outerHTML);
                                $mainDivVital.find('#AllAdministerVaccine').append($DivNewAdminister);
                            }
                        }
                        else {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineAdminister').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineAdminister')[0].outerHTML);
                            }
                        }
                        if ($DivNewDocumentHx.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineDocumentHx').length == 0) {
                                $mainDivVital.append($mainDivDocumentHx);
                                $mainDivVital.find('#AllDocumentHxVaccine').append($DivNewDocumentHx);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineDocumentHx')[0].outerHTML);
                                $mainDivVital.find('#AllDocumentHxVaccine').append($DivNewDocumentHx);
                            }
                        } else {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineDocumentHx').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineDocumentHx')[0].outerHTML);
                            }
                        }

                        if ($DivNewRefusal.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineRefusal').length == 0) {
                                $mainDivVital.append($mainDivRefusal);
                                $mainDivVital.find('#AllRefusalVaccine').append($DivNewRefusal);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineRefusal')[0].outerHTML);
                                $mainDivVital.find('#AllRefusalVaccine').append($DivNewRefusal);
                            }
                        } else {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineRefusal').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineRefusal')[0].outerHTML);
                            }
                        }

                        if ($DivNewVoidDose.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineVoidDose').length == 0) {
                                $mainDivVital.append($mainDivVoidDose);
                                $mainDivVital.find('#AllVoidDoseVaccine').append($DivNewVoidDose);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineVoidDose')[0].outerHTML);
                                $mainDivVital.find('#AllVoidDoseVaccine').append($DivNewVoidDose);
                            }
                        } else {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineVoidDose').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineVoidDose')[0].outerHTML);
                            }
                        }
                        if ($DivNewTherapeuticInjection.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_TherapeuticInjection').length == 0) {
                                $mainDivVital.append($mainDivTherapeuticInjection);
                                $mainDivVital.find('#AllTherapeuticInjection').append($DivNewTherapeuticInjection);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_TherapeuticInjection')[0].outerHTML);
                                $mainDivVital.find('#AllTherapeuticInjection').append($DivNewTherapeuticInjection);
                            }
                        }
                        else {
                            if ($(NoteHTMLCtrl).find('#Section_TherapeuticInjection').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_TherapeuticInjection')[0].outerHTML);
                            }
                        }
                        if ($DivNewTherapeuticInjectionHistory.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_TherapeuticInjectionHistory').length == 0) {
                                $mainDivVital.append($mainDivTherapeuticInjectionHistory);
                                $mainDivVital.find('#AllTherapeuticInjectionHistory').append($DivNewTherapeuticInjectionHistory);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_TherapeuticInjectionHistory')[0].outerHTML);
                                $mainDivVital.find('#AllTherapeuticInjectionHistory').append($DivNewTherapeuticInjectionHistory);
                            }
                        }
                        else {
                            if ($(NoteHTMLCtrl).find('#Section_TherapeuticInjectionHistory').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_TherapeuticInjectionHistory')[0].outerHTML);
                            }
                        }
                        //Start//04//01//2015//Ahmad Raza//
                        $.when(Clinical_ImmunizationDetail.updateImmunizationHtml($mainDivVital.html(), ImmunizationId, NoteHTMLCtrl, hideAlertMessage)).then(function () {
                            dfd.resolve();

                        });
                    } else {
                        $.when(Clinical_ImmunizationDetail.updateImmunizationHtml('', ImmunizationId, NoteHTMLCtrl, hideAlertMessage)).then(function () {
                            dfd.resolve();
                        });
                        Clinical_ProgressNote.saveComponentSOAPText("Immunization", hideAlertMessage);
                    }


                }
            });

        }
        else {
            dfd.resolve();
        }
        return dfd.promise();
    },

    createImmunizationBodyHTMLForSoap: function (response, NoteHTMLCtrl, ImmunizationId, hideAlertMessage, bNotSaveCompt, comeFromProgressNote) {
        var dfd = $.Deferred();
        if (typeof comeFromProgressNote != typeof undefined && comeFromProgressNote != null && comeFromProgressNote) {
            var tbl = JSON.parse(response.Vaccines);
            var ther = JSON.parse(response.Injections);
            if (tbl.length > 0 || ther.length > 0) {
                Clinical_ImmunizationDetail.checkImmunizationExists();
            }
        }
        else {
            Clinical_ImmunizationDetail.checkImmunizationExists();
        }

        if (response.FromOrderSet == true) {
            var Vaccinesoap_JSON = JSON.parse(response.Vaccines);
        }
        else {
            var Vaccinesoap_JSON = response.Vaccines;
        }

        var TheraeuticInjectionLoad_JSON = [];
        if (typeof comeFromProgressNote != typeof undefined && comeFromProgressNote != null && comeFromProgressNote) {
            TheraeuticInjectionLoad_JSON = JSON.parse(response.Injections);
        }
        else {
            if (response.FromOrderSet == true) {
                TheraeuticInjectionLoad_JSON = JSON.parse(response.Injections);
            }
            else {
                if (response.Injections.length > 0) {
                    TheraeuticInjectionLoad_JSON = response.Injections;
                }
            }

        }


        var $mainDivVital = $(document.createElement('div'));
        var $mainDivAdminister = $(document.createElement('div'));
        $mainDivAdminister.attr('id', "Section_VaccineAdminister");
        $mainDivAdminister.append('<h6 class="text-bold">Administration Vaccine</h6>' + "<div id='AllAdministerVaccine'></div>");
        var $mainDivDocumentHx = $(document.createElement('div'));
        $mainDivDocumentHx.attr('id', "Section_VaccineDocumentHx");
        $mainDivDocumentHx.append('<h6 class="text-bold">Document Hx Vaccine</h6>' + "<div id='AllDocumentHxVaccine'></div>");

        var $mainDivRefusal = $(document.createElement('div'));
        $mainDivRefusal.attr('id', "Section_VaccineRefusal");
        $mainDivRefusal.append('<h6 class="text-bold">Refusal Vaccine</h6>' + "<div id='AllRefusalVaccine'></div>");

        var $mainDivVoidDose = $(document.createElement('div'));
        $mainDivVoidDose.attr('id', "Section_VaccineVoidDose");
        $mainDivVoidDose.append('<h6 class="text-bold">Void Dose</h6>' + "<div id='AllVoidDoseVaccine'></div>");


        var $mainDivTherapeuticInjection = $(document.createElement('div'));
        $mainDivTherapeuticInjection.attr('id', "Section_TherapeuticInjection");
        $mainDivTherapeuticInjection.append('<h6 class="text-bold">Therapeutic Injection</h6>' + "<div id='AllTherapeuticInjection'></div>");

        var $mainDivTherapeuticInjectionHistory = $(document.createElement('div'));
        $mainDivTherapeuticInjectionHistory.attr('id', "Section_TherapeuticInjectionHistory");
        $mainDivTherapeuticInjectionHistory.append('<h6 class="text-bold">Therapeutic Injection History</h6>' + "<div id='AllTherapeuticInjectionHistory'></div>");


        var $DivNewTherapeuticInjection = $(document.createElement('div'));
        var $DivNewTherapeuticInjectionHistory = $(document.createElement('div'));
        var $DivNewAdminister = $(document.createElement('div'));
        var $DivNewDocumentHx = $(document.createElement('div'));
        var $DivNewRefusal = $(document.createElement('div'));
        var $DivNewVoidDose = $(document.createElement('div'));


        if ((Vaccinesoap_JSON == null || Vaccinesoap_JSON.length == 0) && ((TheraeuticInjectionLoad_JSON == null || TheraeuticInjectionLoad_JSON.length == 0))) {
            if (!bNotSaveCompt) {
                $.when(Clinical_ProgressNote.saveComponentSOAPText("Immunization", hideAlertMessage)).then(function () {
                    dfd.resolve();
                });
            }
            else {
                dfd.resolve();
            }

            return dfd.promise();
        }
        if (Vaccinesoap_JSON.length > 0 || ((TheraeuticInjectionLoad_JSON.length > 0))) {
            var PListId = [];
            var def = [];

            $.each(Vaccinesoap_JSON, function (index, element) {
                var color = "";
                var PLid = element.VaccineHxId;
                var $SectionBodyVital = $(document.createElement('section'));
                $SectionBodyVital.attr('id', "Cli_Immunization_Main" + PLid);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_Immunization_" + PLid);
                var $ListVital = $(document.createElement('ul'));

                $ListVital.attr('class', 'list-unstyled')

                $SectionBodyVital.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Immunization_" + PLid + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Immunization_Main" + PLid + '"  ><i class="fa fa-times"></i></a></div> ');

                'Patient underwent <ProcedureCode ProcedureName> based on the following assessment: <Diagnosis> from <From> to <To>. Comments: <Comments>'
                if (element.Type.toLowerCase() == "administer" && (element.VoidDose == "False" || element.VoidDose == false)) {
                    $ListVital.append("<li>" +
                        (element.VaccineName == '' ? "" : "<b>" + element.VaccineName + "</b>") +
                        ((element.CPT == '' || element.CPT == null) ? "" : " <b>(" + element.CPT.substring(0, element.CPT.length - 1) + ") </b>") +
                        (element.Dose == '' ? "" : ", Dose: " + element.Dose + " " + element.Amount) +
                         (!(element.RouteDescription) ? "" : ", Route: " + element.RouteDescription) +
                         (!(element.SiteDescription) ? "" : ", Site: " + element.SiteDescription) +
                          (!(element.LotNumber) ? "" : ", Lot Number: " + element.LotNumber) +
                        (!(element.ManufacturerName) ? "" : ", Manufacturer: " + element.ManufacturerName) +
                        (element.ProviderName == '' ? "" : ", administrated by " + element.ProviderName) +
                        (element.AdministrationDate == '' ? "" : " on " + (moment(element.AdministrationDate).format("MM/DD/YYYY, hh:mm:ss A")))

                        );
                }
                else if (element.Type.toLowerCase() == "documenthx" && (element.VoidDose == "False" || element.VoidDose == false)) {
                    $ListVital.append("<li>" +
                        (element.VaccineName == '' ? "" : "<b>" + element.VaccineName + "</b>") +
                      (element.Dose == '' ? "" : ", Dose: " + element.Dose + " " + element.Amount) +
                         (!(element.RouteDescription) ? "" : ", Route: " + element.RouteDescription) +
                         (!(element.SiteDescription) ? "" : ", Site: " + element.SiteDescription) +
                          (!(element.LotNumber) ? "" : ", Lot Number: " + element.LotNumber) +
                          (!(element.ManufacturerName) ? "" : ", Manufacturer: " + element.ManufacturerName) +
                        (element.ProviderName == '' ? "" : ", administrated by " + element.ProviderName) +
                        (element.AdministrationDate == '' ? "": " on " +(moment(element.AdministrationDate).format("MM/DD/YYYY, hh:mm:ss A")))
                        );
                }
                else if (element.Type.toLowerCase() == "refusal" && (element.VoidDose == "False" || element.VoidDose == false)) {
                    $ListVital.append("<li>" +
                        (element.VaccineName == '' ? "" : "<b>" + element.VaccineName + "</b>") +
                        (element.RefusalReason == '' ? "" : ", not given to the patient due to " + element.RefusalReason)
                        );
                }
                else if (element.VoidDose == "True" || element.VoidDose == true) {
                    $ListVital.append("<li>" +
                        (element.VaccineName == '' ? "" : "<b>" + element.VaccineName + "</b>") +
                        (element.Dose == '' ? "" : ", Dose: " + element.Dose + " " + element.Amount) +
                         (!(element.RouteDescription) ? "" : ", Route: " + element.RouteDescription) +
                         (!(element.SiteDescription) ? "" : ", Site: " + element.SiteDescription) +
                          (!(element.LotNumber) ? "" : ", Lot Number: " + element.LotNumber) +
                          (!(element.ManufacturerName) ? "" : ", Manufacturer: " + element.ManufacturerName) +
                        (element.ProviderName == '' ? "" : ", administrated by " + element.ProviderName) +
                        (element.AdministrationDate == '' ? "" : " on " + (moment(element.AdministrationDate).format("MM/DD/YYYY, hh:mm:ss A"))) +
                        (element.Comments == '' ? "" : "</br> Reason: " + element.Comments)
                        );
                }


                $DetailsDiv.append($ListVital);
                $SectionBodyVital.append($DetailsDiv);

                var MainDivId = "";
                if (element.Type.toLowerCase() == "administer" && (element.VoidDose == "False" || element.VoidDose == false)) {
                    MainDivId = "AllAdministerVaccine";
                }
                else if (element.Type.toLowerCase() == "documenthx" && (element.VoidDose == "False" || element.VoidDose == false)) {
                    MainDivId = "AllDocumentHxVaccine";
                }
                else if (element.Type.toLowerCase() == "refusal" && (element.VoidDose == "False" || element.VoidDose == false)) {
                    MainDivId = "AllRefusalVaccine";
                }
                else if (element.VoidDose == "True" || element.VoidDose == true) {
                    MainDivId = "AllVoidDoseVaccine";
                }
                if (PLid != "") {

                    if ($(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#' + MainDivId + ' #Cli_Immunization_Main' + PLid).length == 0) {
                        PListId.push(PLid);
                        if (element.Type.toLowerCase() == "administer" && (element.VoidDose == "False" || element.VoidDose == false)) {
                            def.push(
                            $.when(Clinical_ImmunizationDetail.RemoveIfExistINAnyOtherVaccineType(NoteHTMLCtrl, PLid)).then(function () {
                                $DivNewAdminister.append($SectionBodyVital);
                            })
                            )

                        }
                        else if (element.Type.toLowerCase() == "documenthx" && (element.VoidDose == "False" || element.VoidDose == false)) {

                            def.push($.when(Clinical_ImmunizationDetail.RemoveIfExistINAnyOtherVaccineType(NoteHTMLCtrl, PLid)).then(function () {
                                $DivNewDocumentHx.append($SectionBodyVital);
                            }))
                        }
                        else if (element.Type.toLowerCase() == "refusal" && (element.VoidDose == "False" || element.VoidDose == false)) {
                            def.push($.when(Clinical_ImmunizationDetail.RemoveIfExistINAnyOtherVaccineType(NoteHTMLCtrl, PLid)).then(function () {
                                $DivNewRefusal.append($SectionBodyVital);
                            }))
                        }
                        else if (element.VoidDose == "True" || element.VoidDose == true) {
                            def.push($.when(Clinical_ImmunizationDetail.RemoveIfExistINAnyOtherVaccineType(NoteHTMLCtrl, PLid)).then(function () {
                                $DivNewVoidDose.append($SectionBodyVital);
                            }))
                        }


                    } else {
                        var CommentHTML = "";
                        var CommentsID = $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + PLid + ' ul li:Last').attr('id');
                        if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                            CommentHTML = $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + PLid + ' ul li:Last').get(0).outerHTML;
                        }
                        $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + PLid).html($SectionBodyVital.html());
                        $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + PLid + ' ul').append(CommentHTML);
                    }
                }


                if (Vaccinesoap_JSON.length == index + 1) {

                }

            });






            $.when.apply($, def).done(function ($n) {
                if (TheraeuticInjectionLoad_JSON.length > 0) {

                    $.each(TheraeuticInjectionLoad_JSON, function (index, element1) {
                        var color = "";


                        var TheraInjectionid = element1.ImmTherInjectionId + "thera";
                        var $SectionBodyVital = $(document.createElement('section'));
                        $SectionBodyVital.attr('id', "Cli_Immunization_Main" + TheraInjectionid);
                        var $DetailsDiv = $(document.createElement('div'));
                        $DetailsDiv.attr('id', "Cli_Immunization_" + TheraInjectionid);
                        var $ListVital = $(document.createElement('ul'));

                        $ListVital.attr('class', 'list-unstyled')

                        $SectionBodyVital.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Immunization_" + TheraInjectionid + '"><i class="fa fa-edit"></i></a>' +
                            '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Immunization_Main" + TheraInjectionid + '"  ><i class="fa fa-times"></i></a></div> ');
                        var _InjecNameAndCode = (element1.CPTCode == '' ? (element1.TherapeuticInjection == '' ? "" : element1.TherapeuticInjection) : element1.CPTCode + " - " + element1.TherapeuticInjection);
                        'Patient underwent <ProcedureCode ProcedureName> based on the following assessment: <Diagnosis> from <From> to <To>. Comments: <Comments>'
                        $ListVital.append("<li>" + "<b>" + _InjecNameAndCode + "</b>" +
                       (element1.Dose == '' ? "" : ", Dose: " + element1.Dose + " " + element1.Amount) +
                         (!(element1.RouteDescription) ? "" : ", Route: " + element1.RouteDescription) +
                         (!(element1.SiteDescription) ? "" : ", Site: " + element1.SiteDescription) +
                          (!(element1.LotNumber) ? "" : ", Lot Number: " + element1.LotNumber) +
                          (!(element1.ManufacturerName) ? "" : ", Manufacturer: " + element1.ManufacturerName) +
                        (element1.ProviderName == '' ? "" : ", administrated by " + element1.ProviderName) +
                         (element1.AdministrationDate == '' ? "" : " on " + (moment(element1.AdministrationDate).format("MM/DD/YYYY, hh:mm:ss A")))
                       );



                        $DetailsDiv.append($ListVital);
                        $SectionBodyVital.append($DetailsDiv);

                        var MainDivId = "";
                        if (element1.Type == "Administered") {
                            MainDivId = "AllTherapeuticInjection";
                            if ($(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#' + MainDivId + ' #Cli_Immunization_Main' + TheraInjectionid).length == 0) {
                                PListId.push(TheraInjectionid);
                                $DivNewTherapeuticInjection.append($SectionBodyVital);
                            } else {
                                var CommentHTML = "";
                                var CommentsID = $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + TheraInjectionid + ' ul li:Last').attr('id');
                                if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                                    CommentHTML = $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + TheraInjectionid + ' ul li:Last').get(0).outerHTML;
                                }
                                $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + TheraInjectionid).html($SectionBodyVital.html());
                                $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + TheraInjectionid + ' ul').append(CommentHTML);
                            }

                        }
                        else {
                            MainDivId = "AllTherapeuticInjectionHistory";
                            if ($(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#' + MainDivId + ' #Cli_Immunization_Main' + TheraInjectionid).length == 0) {
                                PListId.push(TheraInjectionid);
                                $DivNewTherapeuticInjectionHistory.append($SectionBodyVital);
                            } else {
                                var CommentHTML = "";
                                var CommentsID = $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + TheraInjectionid + ' ul li:Last').attr('id');
                                if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                                    CommentHTML = $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + TheraInjectionid + ' ul li:Last').get(0).outerHTML;
                                }
                                $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + TheraInjectionid).html($SectionBodyVital.html());
                                $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + TheraInjectionid + ' ul').append(CommentHTML);
                            }
                        }

                    });

                    //Start//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
                    if (PListId.join(",") != "") {
                        ImmunizationId = PListId.join(",");
                    }
                    //End//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
                    if ($DivNewTherapeuticInjectionHistory.html() != '' || $DivNewTherapeuticInjection.html() != '' || $DivNewAdminister.html() != '' || $DivNewDocumentHx.html() != '' || $DivNewRefusal.html() != '' || $DivNewVoidDose.html() != '') {

                        if ($DivNewAdminister.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineAdminister').length == 0) {
                                $mainDivVital.append($mainDivAdminister);
                                $mainDivVital.find('#AllAdministerVaccine').append($DivNewAdminister);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineAdminister')[0].outerHTML);
                                $mainDivVital.find('#AllAdministerVaccine').append($DivNewAdminister);
                            }
                        }
                        else {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineAdminister').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineAdminister')[0].outerHTML);
                            }
                        }
                        if ($DivNewDocumentHx.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineDocumentHx').length == 0) {
                                $mainDivVital.append($mainDivDocumentHx);
                                $mainDivVital.find('#AllDocumentHxVaccine').append($DivNewDocumentHx);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineDocumentHx')[0].outerHTML);
                                $mainDivVital.find('#AllDocumentHxVaccine').append($DivNewDocumentHx);
                            }
                        } else {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineDocumentHx').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineDocumentHx')[0].outerHTML);
                            }
                        }

                        if ($DivNewRefusal.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineRefusal').length == 0) {
                                $mainDivVital.append($mainDivRefusal);
                                $mainDivVital.find('#AllRefusalVaccine').append($DivNewRefusal);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineRefusal')[0].outerHTML);
                                $mainDivVital.find('#AllRefusalVaccine').append($DivNewRefusal);
                            }
                        } else {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineRefusal').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineRefusal')[0].outerHTML);
                            }
                        }

                        if ($DivNewVoidDose.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineVoidDose').length == 0) {
                                $mainDivVital.append($mainDivVoidDose);
                                $mainDivVital.find('#AllVoidDoseVaccine').append($DivNewVoidDose);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineVoidDose')[0].outerHTML);
                                $mainDivVital.find('#AllVoidDoseVaccine').append($DivNewVoidDose);
                            }
                        } else {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineVoidDose').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineVoidDose')[0].outerHTML);
                            }
                        }
                        if ($DivNewTherapeuticInjection.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_TherapeuticInjection').length == 0) {
                                $mainDivVital.append($mainDivTherapeuticInjection);
                                $mainDivVital.find('#AllTherapeuticInjection').append($DivNewTherapeuticInjection);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_TherapeuticInjection')[0].outerHTML);
                                $mainDivVital.find('#AllTherapeuticInjection').append($DivNewTherapeuticInjection);
                            }
                        }
                        else {
                            if ($(NoteHTMLCtrl).find('#Section_TherapeuticInjection').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_TherapeuticInjection')[0].outerHTML);
                            }
                        }
                        if ($DivNewTherapeuticInjectionHistory.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_TherapeuticInjectionHistory').length == 0) {
                                $mainDivVital.append($mainDivTherapeuticInjectionHistory);
                                $mainDivVital.find('#AllTherapeuticInjectionHistory').append($DivNewTherapeuticInjectionHistory);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_TherapeuticInjectionHistory')[0].outerHTML);
                                $mainDivVital.find('#AllTherapeuticInjectionHistory').append($DivNewTherapeuticInjectionHistory);
                            }
                        }
                        else {
                            if ($(NoteHTMLCtrl).find('#Section_TherapeuticInjectionHistory').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_TherapeuticInjectionHistory')[0].outerHTML);
                            }
                        }
                        //Start//04//01//2015//Ahmad Raza//
                        $.when(Clinical_ImmunizationDetail.updateImmunizationHtml($mainDivVital.html(), ImmunizationId, NoteHTMLCtrl, hideAlertMessage, bNotSaveCompt)).then(function () {
                            dfd.resolve();
                        });
                        if (typeof comeFromProgressNote != typeof undefined && comeFromProgressNote != null && comeFromProgressNote) {
                            Clinical_ProgressNote.saveComponentSOAPText("Immunization", hideAlertMessage);
                        }
                    } else {
                        $.when(Clinical_ImmunizationDetail.updateImmunizationHtml('', ImmunizationId, NoteHTMLCtrl, hideAlertMessage, bNotSaveCompt)).then(function () {
                            dfd.resolve();
                        });
                    }


                }
                else {

                    //Start//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
                    if (PListId.join(",") != "") {
                        ImmunizationId = PListId.join(",");
                    }
                    //End//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
                    if ($DivNewAdminister.html() != '' || $DivNewDocumentHx.html() != '' || $DivNewRefusal.html() != '' || $DivNewVoidDose.html() != '') {
                        if ($DivNewAdminister.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineAdminister').length == 0) {
                                $mainDivVital.append($mainDivAdminister);
                                $mainDivVital.find('#AllAdministerVaccine').append($DivNewAdminister);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineAdminister')[0].outerHTML);
                                $mainDivVital.find('#AllAdministerVaccine').append($DivNewAdminister);
                            }
                        }
                        else {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineAdminister').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineAdminister')[0].outerHTML);
                            }
                        }
                        if ($DivNewDocumentHx.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineDocumentHx').length == 0) {
                                $mainDivVital.append($mainDivDocumentHx);
                                $mainDivVital.find('#AllDocumentHxVaccine').append($DivNewDocumentHx);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineDocumentHx')[0].outerHTML);
                                $mainDivVital.find('#AllDocumentHxVaccine').append($DivNewDocumentHx);
                            }
                        } else {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineDocumentHx').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineDocumentHx')[0].outerHTML);
                            }
                        }

                        if ($DivNewRefusal.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineRefusal').length == 0) {
                                $mainDivVital.append($mainDivRefusal);
                                $mainDivVital.find('#AllRefusalVaccine').append($DivNewRefusal);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineRefusal')[0].outerHTML);
                                $mainDivVital.find('#AllRefusalVaccine').append($DivNewRefusal);
                            }
                        } else {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineRefusal').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineRefusal')[0].outerHTML);
                            }
                        }

                        if ($DivNewVoidDose.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineVoidDose').length == 0) {
                                $mainDivVital.append($mainDivVoidDose);
                                $mainDivVital.find('#AllVoidDoseVaccine').append($DivNewVoidDose);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineVoidDose')[0].outerHTML);
                                $mainDivVital.find('#AllVoidDoseVaccine').append($DivNewVoidDose);
                            }
                        } else {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineVoidDose').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineVoidDose')[0].outerHTML);
                            }
                        }
                        if ($DivNewTherapeuticInjection.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_TherapeuticInjection').length == 0) {
                                $mainDivVital.append($mainDivTherapeuticInjection);
                                $mainDivVital.find('#AllTherapeuticInjection').append($DivNewTherapeuticInjection);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_TherapeuticInjection')[0].outerHTML);
                                $mainDivVital.find('#AllTherapeuticInjection').append($DivNewTherapeuticInjection);
                            }
                        }
                        else {
                            if ($(NoteHTMLCtrl).find('#Section_TherapeuticInjection').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_TherapeuticInjection')[0].outerHTML);
                            }
                        }
                        if ($DivNewTherapeuticInjectionHistory.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_TherapeuticInjectionHistory').length == 0) {
                                $mainDivVital.append($mainDivTherapeuticInjectionHistory);
                                $mainDivVital.find('#AllTherapeuticInjectionHistory').append($DivNewTherapeuticInjectionHistory);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_TherapeuticInjectionHistory')[0].outerHTML);
                                $mainDivVital.find('#AllTherapeuticInjectionHistory').append($DivNewTherapeuticInjectionHistory);
                            }
                        }
                        else {
                            if ($(NoteHTMLCtrl).find('#Section_TherapeuticInjectionHistory').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_TherapeuticInjectionHistory')[0].outerHTML);
                            }
                        }
                        //Start//04//01//2015//Ahmad Raza//
                        $.when(Clinical_ImmunizationDetail.updateImmunizationHtml($mainDivVital.html(), ImmunizationId, NoteHTMLCtrl, hideAlertMessage, bNotSaveCompt)).then(function () {
                            dfd.resolve();

                        });
                        if (typeof comeFromProgressNote != typeof undefined && comeFromProgressNote != null && comeFromProgressNote) {
                            Clinical_ProgressNote.saveComponentSOAPText("Immunization", hideAlertMessage);
                        }
                    } else {
                        $.when(Clinical_ImmunizationDetail.updateImmunizationHtml('', ImmunizationId, NoteHTMLCtrl, hideAlertMessage, bNotSaveCompt)).then(function () {
                            dfd.resolve();
                        });
                        Clinical_ProgressNote.saveComponentSOAPText("Immunization", hideAlertMessage);
                    }


                }
            });

        }
        else {
            dfd.resolve();
        }
        return dfd.promise();
    },

    getLatestImmunizationsByPatientId: function () {
        Clinical_ImmunizationDetail.getLatestClinical_ImmunizationByPatientId_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status == true) {
                Clinical_ImmunizationDetail.checkImmunizationExists();
                if (response) {
                    Clinical_ImmunizationDetail.createImmunizationBodyHTMLForSoap(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, true);
                }
                //else {
                //    Clinical_ProgressNote.saveComponentSOAPText(null, null, hideAlertMessage);
                //    //Clinical_Medications.noActiveMedicationSoapText(hideAlertMessage);
                //}
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });
    },

    RemoveIfExistINAnyOtherVaccineType: function (NoteHTMLCtrl, PLid) {
        var dfd = $.Deferred();
        var MainDivArray = ["AllAdministerVaccine", "AllDocumentHxVaccine", "AllRefusalVaccine", "AllVoidDoseVaccine"];
        var found = false;
        $.each(MainDivArray, function (index, item) {
            if ($(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#' + item + ' #Cli_Immunization_Main' + PLid).length != 0) {
                found = true;
                return;
            }
        });
        if (found) {
            $.when(Clinical_ImmunizationDetail.detachImmunization(PLid)).then(function () {
                dfd.resolve();
            });
        }
        else {
            dfd.resolve();
        }
        return dfd.promise();
    },

    // This Function is called by Progress Notes (Fill Vitals Func, CopyAllNotesCategories)
    updateImmunizationHtml: function (ImmunizationHtml, ProcedureId, NoteHTMLCtrl, hideAlertMessage, bNotSaveCompt) {
        var dfd = $.Deferred();
        $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().addClass('initialVisitBody');
        if (ImmunizationHtml != '') {
            //var divImmunization = $(NoteHTMLCtrl + ' Clinical_Immunization a:nth-child(1)')[0].outerHTML + $(NoteHTMLCtrl + ' Clinical_Immunization a:nth-child(2)')[0].outerHTML + $(NoteHTMLCtrl + ' Clinical_Immunization a:nth-child(3)')[0].outerHTML;
            var divImmunization = ""
            if ($(NoteHTMLCtrl + ' Clinical_Immunization a:nth-child(1)').length > 0)
                divImmunization += $(NoteHTMLCtrl + ' Clinical_Immunization a:nth-child(1)')[0].outerHTML;
            if ($(NoteHTMLCtrl + ' Clinical_Immunization a:nth-child(2)').length > 0)
                divImmunization += $(NoteHTMLCtrl + ' Clinical_Immunization a:nth-child(2)')[0].outerHTML;
            if ($(NoteHTMLCtrl + ' Clinical_Immunization a:nth-child(3)').length > 0)
                divImmunization += $(NoteHTMLCtrl + ' Clinical_Immunization a:nth-child(3)')[0].outerHTML;
            $(NoteHTMLCtrl + ' Clinical_Immunization').html(divImmunization);
            //$(NoteHTMLCtrl + ' Clinical_Immunization').in.append(ImmunizationHtml);
            $(NoteHTMLCtrl).find('#Section_VaccineAdminister').remove();
            $(NoteHTMLCtrl).find('#Section_VaccineDocumentHx').remove();
            $(NoteHTMLCtrl).find('#Section_VaccineRefusal').remove();
            $(NoteHTMLCtrl).find('#Section_VaccineVoidDose').remove();
            $(NoteHTMLCtrl).find('#Section_TherapeuticInjection').remove();
            $(NoteHTMLCtrl).find('#Section_TherapeuticInjectionHistory').remove();
            $(ImmunizationHtml).insertAfter($(NoteHTMLCtrl + ' Clinical_Immunization').parent());
        }

        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (ImmunizationHtml != '') {
            $.when(Clinical_ImmunizationDetail.attachImmunizationignFromNotes(ProcedureId, hideAlertMessage, bNotSaveCompt)).then(function () {
                dfd.resolve();
            });
        }
        else {
            dfd.resolve();
        }
        return dfd.promise();
    },

    //This Functions ask for Detaching Vital sign from Progress Note for current Patient Selected
    detachImmunizationFromNotes: function (ImmunizationId) {
        //Start//29/12/2015//Ahmad Raza//Privileges logic implemented
        utility.myConfirm('1', function () {
            EMRUtility.scrollToPNcomponent('clinical_immunization');
            var selectedValue = ImmunizationId.replace('Cli_Immunization_Main', '');
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                if (selectedValue.indexOf("thera") > -1) {
                    Clinical_ImmunizationDetail.detachTherapeuticInjectionFromNotes_DBCall(selectedValue.replace("thera", "")).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {

                            //start Remove heading
                            var HeadingNotRemoved = true;
                            if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + selectedValue).parent().parent().attr("id") == "AllAdministerVaccine") {
                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllAdministerVaccine').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_VaccineAdminister').remove();
                                    HeadingNotRemoved = false;
                                }
                            }
                            if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + selectedValue).parent().parent().attr("id") == "AllTherapeuticInjection") {
                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllTherapeuticInjection').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_TherapeuticInjection').remove();
                                    HeadingNotRemoved = false;
                                }
                            }
                            if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + selectedValue).parent().parent().attr("id") == "AllTherapeuticInjectionHistory") {
                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllTherapeuticInjectionHistory').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_TherapeuticInjectionHistory').remove();
                                    HeadingNotRemoved = false;
                                }
                            }
                            if (HeadingNotRemoved) {
                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + selectedValue).parent().parent().attr("id") == "AllDocumentHxVaccine") {
                                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllDocumentHxVaccine').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_VaccineDocumentHx').remove();
                                        HeadingNotRemoved = false;
                                    }
                                }
                            }
                            if (HeadingNotRemoved) {
                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + selectedValue).parent().parent().attr("id") == "AllRefusalVaccine") {
                                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllRefusalVaccine').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_VaccineRefusal').remove();
                                        HeadingNotRemoved = false;
                                    }
                                }
                            }
                            if (HeadingNotRemoved) {
                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + selectedValue).parent().parent().attr("id") == "AllVoidDoseVaccine") {
                                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllVoidDoseVaccine').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_VaccineVoidDose').remove();
                                        HeadingNotRemoved = false;
                                    }
                                }
                            }
                            if (HeadingNotRemoved) {
                                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + selectedValue).remove();
                            }
                            //End remove heading

                            Clinical_ProgressNote.saveComponentSOAPText("Immunization");
                            setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);

                            //   utility.DisplayMessages(response.Message, 1);
                        }
                        else {

                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else {
                    Clinical_ImmunizationDetail.detachImmunizationFromNotes_DBCall(selectedValue).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {

                            //start Remove heading
                            var HeadingNotRemoved = true;
                            if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + selectedValue).parent().parent().attr("id") == "AllAdministerVaccine") {
                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllAdministerVaccine').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_VaccineAdminister').remove();
                                    HeadingNotRemoved = false;
                                }
                            }
                            if (HeadingNotRemoved) {
                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + selectedValue).parent().parent().attr("id") == "AllDocumentHxVaccine") {
                                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllDocumentHxVaccine').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_VaccineDocumentHx').remove();
                                        HeadingNotRemoved = false;
                                    }
                                }
                            }
                            if (HeadingNotRemoved) {
                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + selectedValue).parent().parent().attr("id") == "AllRefusalVaccine") {
                                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllRefusalVaccine').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_VaccineRefusal').remove();
                                        HeadingNotRemoved = false;
                                    }
                                }
                            }
                            if (HeadingNotRemoved) {
                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + selectedValue).parent().parent().attr("id") == "AllVoidDoseVaccine") {
                                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllVoidDoseVaccine').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_VaccineVoidDose').remove();
                                        HeadingNotRemoved = false;
                                    }
                                }
                            }
                            if (HeadingNotRemoved) {
                                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + selectedValue).remove();
                            }
                            //End remove heading

                            Clinical_ProgressNote.saveComponentSOAPText("Immunization");
                            setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);

                            //   utility.DisplayMessages(response.Message, 1);
                        }
                        else {

                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            }
        }, function () { }, '1');
        //End//29/12/2015//Ahmad Raza//Privileges logic implemented
    },

    detachImmunization: function (ImmunizationId) {
        var dfd = $.Deferred();
        var selectedValue = ImmunizationId.toString().replace('Cli_Immunization_Main', '');
        if (selectedValue == "" || selectedValue == "undefined") {
            dfd.resolve();
        }
        else {
            Clinical_ImmunizationDetail.detachImmunizationFromNotes_DBCall(selectedValue).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {

                    //start Remove heading
                    var HeadingNotRemoved = true;
                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + selectedValue).parent().parent().attr("id") == "AllAdministerVaccine") {
                        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllAdministerVaccine').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_VaccineAdminister').remove();
                            HeadingNotRemoved = false;
                        }
                    }
                    if (HeadingNotRemoved) {
                        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + selectedValue).parent().parent().attr("id") == "AllDocumentHxVaccine") {
                            if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllDocumentHxVaccine').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_VaccineDocumentHx').remove();
                                HeadingNotRemoved = false;
                            }
                        }
                    }
                    if (HeadingNotRemoved) {
                        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + selectedValue).parent().parent().attr("id") == "AllTherapeuticInjection") {
                            if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllTherapeuticInjection').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_TherapeuticInjection').remove();
                                HeadingNotRemoved = false;
                            }
                        }
                    }
                    if (HeadingNotRemoved) {
                        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + selectedValue).parent().parent().attr("id") == "AllTherapeuticInjectionHistory") {
                            if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllTherapeuticInjectionHistory').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_TherapeuticInjectionHistory').remove();
                                HeadingNotRemoved = false;
                            }
                        }
                    }
                    if (HeadingNotRemoved) {
                        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + selectedValue).parent().parent().attr("id") == "AllRefusalVaccine") {
                            if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllRefusalVaccine').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_VaccineRefusal').remove();
                                HeadingNotRemoved = false;
                            }
                        }
                    }
                    if (HeadingNotRemoved) {
                        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + selectedValue).parent().parent().attr("id") == "AllVoidDoseVaccine") {
                            if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllVoidDoseVaccine').find('section[id*="Cli_Immunization_Main"]').length == 1) {
                                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_VaccineVoidDose').remove();
                                HeadingNotRemoved = false;
                            }
                        }
                    }
                    if (HeadingNotRemoved) {
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Immunization_Main' + selectedValue).remove();
                    }
                    ////End remove heading

                    //Clinical_ProgressNote.saveComponentSOAPText();
                    //setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                    dfd.resolve();
                    //   utility.DisplayMessages(response.Message, 1);
                }
                else {
                    dfd.resolve();
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        return dfd.promise();
    },

    //This Function detach Problem list From progress note
    detach_ComponentsImmunization: function (ComponentName, IsUpdate, ImmunizationComponentRemove) {

        var Clinical_ProcedureIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML Clinical_Immunization').parent().parent().find('section[id*="Cli_Immunization_Main"]').map(function () {
            return this.id.replace("Cli_Immunization_Main", "");
        }).get().join(',');
        var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .ImmunizationComponent').attr('NoteComponentId');
        if (ImmunizationComponentRemove) {
            if (NoteComponentId && NoteComponentId != "NCDummyId") {
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' Clinical_Immunization').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Immunization', true))
                }
                else
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                $.when.apply($, promise).done(function () {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Immunization']").remove();
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' Clinical_Immunization').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
            else {
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Immunization']").remove();
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' Clinical_Immunization').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Immunization', true))
                }
                else {
                    if (NoteComponentId && NoteComponentId != "NCDummyId")
                        promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId));
                    else {
                        var def = $.Deferred();
                        promise.push(def);
                        def.resolve();
                    }
                }
                $.when.apply($, promise).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' Clinical_Immunization').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML Clinical_Immunization').parent().parent().find('section[id*="Cli_Immunization_Main"]').remove();
        }

        if (Clinical_ProcedureIds == "" || Clinical_ProcedureIds == "undefined") {
            var promise = [];
            if (Clinical_ProgressNote.params["TemplateName"]) {
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' Clinical_Immunization').parent().parent().addClass('hidden');
                promise.push(Clinical_ProgressNote.saveComponentSOAPText('Immunization', true))
            }
            else
            {
                if (NoteComponentId && NoteComponentId != "NCDummyId")
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId));
                else {
                    var def = $.Deferred();
                    promise.push(def);
                    def.resolve();
                }
            }
            $.when.apply($, promise).done(function () {
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Immunization']").remove();
                if (Clinical_ProgressNote.params["TemplateName"] == "")
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' Clinical_Immunization').parent().parent().remove();
                Clinical_ProgressNote.ShowHideComponetsHeaders();
                utility.DisplayMessages('Successfully Deleted', 1);
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
            });
        }
        else {
            var splitAdministerVaccineId = Clinical_ProcedureIds.split(",");
            var AdministerVaccineIdForImmunization = [];
            var AdministerVaccineIdForTherapeuticInjection = [];

            $.each(Clinical_ProcedureIds.split(","), function (i, item) {
                if (item.indexOf("thera") > -1) {
                    AdministerVaccineIdForTherapeuticInjection.push(item.replace("thera", ""));
                }
                else {
                    AdministerVaccineIdForImmunization.push(item);
                }
            });

            if (AdministerVaccineIdForImmunization != "") {
                Clinical_ImmunizationDetail.detachImmunizationFromNotes_DBCall(AdministerVaccineIdForImmunization.join()).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML Clinical_Immunization').parent().parent().find('div[id="Section_VaccineAdminister"]').remove();
                        if (AdministerVaccineIdForTherapeuticInjection != "") {
                            Clinical_ImmunizationDetail.detachTherapeuticInjectionFromNotes_DBCall(AdministerVaccineIdForTherapeuticInjection.join()).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML Clinical_Immunization').parent().parent().find('div[id="Section_TherapeuticInjection"]').remove();
                                    if (IsUpdate) {
                                        Clinical_ProgressNote.saveComponentSOAPText("Immunization", true);
                                    }
                                    utility.DisplayMessages(response.Message, 1);
                                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                        else {
                            if (IsUpdate) {
                                Clinical_ProgressNote.saveComponentSOAPText("Immunization", true);
                            }
                            utility.DisplayMessages(response.Message, 1);
                            setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                        }
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else if (AdministerVaccineIdForTherapeuticInjection != "") {
                Clinical_ImmunizationDetail.detachTherapeuticInjectionFromNotes_DBCall(AdministerVaccineIdForTherapeuticInjection.join()).done(function (response) {

                    response = JSON.parse(response);
                    if (response.status != false) {
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML Clinical_Immunization').parent().parent().find('div[id="Section_TherapeuticInjection"]').remove();
                        if (IsUpdate) {
                            Clinical_ProgressNote.saveComponentSOAPText("Immunization", true);
                        }
                        utility.DisplayMessages(response.Message, 1);
                        setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }
    },

    //This Functions attached Vital sign to Progress Note for current Patient Selected
    attachImmunizationignFromNotes: function (ImmunizationId, hideAlertMessage, bNotSaveCompt) {
        var dfd = $.Deferred();
        if (ImmunizationId == "" || ImmunizationId == "undefined") {
            dfd.resolve();
        }
        else {
            var splitAdministerVaccineId = ImmunizationId.split(",");
            var AdministerVaccineIdForImmunization = [];
            var AdministerVaccineIdForTherapeuticInjection = [];
            $.each(ImmunizationId.split(","), function (i, item) {
                if (item.indexOf("thera") > -1) {
                    AdministerVaccineIdForTherapeuticInjection.push(item.replace("thera", ""));
                }
                else {
                    AdministerVaccineIdForImmunization.push(item);
                }
            });
            if (AdministerVaccineIdForImmunization != "") {
                Clinical_ImmunizationDetail.attachImmunizationWithNotes_DBCall(AdministerVaccineIdForImmunization.join()).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {

                        if (AdministerVaccineIdForTherapeuticInjection != "") {
                            Clinical_ImmunizationDetail.attachTherapeuticInjectionWithNotes_DBCall(AdministerVaccineIdForTherapeuticInjection.join()).done(function (response1) {
                                response1 = JSON.parse(response1);
                                if (response1.status != false) {

                                    if (!bNotSaveCompt)
                                        Clinical_ProgressNote.saveComponentSOAPText("Immunization", hideAlertMessage);
                                    var copyAdministerVaccineIdForTherapeuticInjection = [];
                                    $.each(AdministerVaccineIdForTherapeuticInjection, function (i, item) {
                                        copyAdministerVaccineIdForTherapeuticInjection.push(item + "thera");

                                    });
                                    $('#' + copyAdministerVaccineIdForTherapeuticInjection.join()).remove();
                                    $('#' + AdministerVaccineIdForImmunization.join()).remove();
                                    dfd.resolve();
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                    dfd.resolve();
                                }
                            });
                        }
                        else {
                            if (!bNotSaveCompt)
                                Clinical_ProgressNote.saveComponentSOAPText("Immunization", hideAlertMessage);
                            $('#' + AdministerVaccineIdForImmunization.join()).remove();
                            dfd.resolve();

                        }
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                        dfd.resolve();
                    }
                });
            }
            else if (AdministerVaccineIdForTherapeuticInjection != "") {
                Clinical_ImmunizationDetail.attachTherapeuticInjectionWithNotes_DBCall(AdministerVaccineIdForTherapeuticInjection.join()).done(function (response1) {
                    response1 = JSON.parse(response1);
                    if (response1.status != false) {

                        $.when(Clinical_ProgressNote.saveComponentSOAPText("Immunization", hideAlertMessage)).then(function () {
                            var copyAdministerVaccineIdForTherapeuticInjection = [];
                            $.each(AdministerVaccineIdForTherapeuticInjection, function (i, item) {
                                copyAdministerVaccineIdForTherapeuticInjection.push(item + "thera");

                            });
                            $('#' + copyAdministerVaccineIdForTherapeuticInjection.join()).remove();
                            dfd.resolve();
                        });

                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                        dfd.resolve();
                    }
                });
            }
        }
        return dfd.promise();
    },

    /* If BirthHx Component which is dropeed in Progress note has no Immunization attached, than it will call for Latest Immunization for this patient
    Author: Muhammad Ahmad Imran */
    getLatestImmunizationByPatientId: function (Vaccines, Injections, Attached_Vaccines, Attached_Injections, noteHTMLCtrl, unloadBirthhx, hideAlertMessage, MakeProcedureSoapText, bNotSaveCompt) {

        if (Vaccines && Injections) {

            if (MakeProcedureSoapText) {

                var VaccineHxIds = [];
                var ImmTherInjectionIds = [];

                $.each(Vaccines, function (i, item) {
                    VaccineHxIds.push(item.VaccineHxId);
                });

                $.each(Injections, function (i, item) {
                    ImmTherInjectionIds.push(item.ImmTherInjectionId);
                });

                if (VaccineHxIds != "" || ImmTherInjectionIds != "") {

                    $.when(Clinical_ImmunizationDetail.createImmunizationSoapText(Vaccines, Injections, Attached_Vaccines, Attached_Injections, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, true, bNotSaveCompt)).then(function () {
                        utility.DisplayMessages("Successfully Updated", 1);
                    });
                }
            }
            else {
                $.when(Clinical_ImmunizationDetail.createImmunizationSoapText(Vaccines, Injections, Attached_Vaccines, Attached_Injections, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, true, bNotSaveCompt)).then(function () {
                    utility.DisplayMessages("Successfully Updated", 1);
                });
            }
        }
    },

    createImmunizationSoapText: function (Vaccines, Injections, Attached_Vaccines, Attached_Injections, NoteHTMLCtrl, ImmunizationId, hideAlertMessage, bNotSaveCompt, comeFromProgressNote) {

        var dfd = $.Deferred();

        Clinical_ImmunizationDetail.checkImmunizationExists();

        var Vaccinesoap_JSON = Vaccines;
        var TheraeuticInjectionLoad_JSON = Injections;


        var $mainDivVital = $(document.createElement('div'));
        var $mainDivAdminister = $(document.createElement('div'));
        $mainDivAdminister.attr('id', "Section_VaccineAdminister");
        $mainDivAdminister.append('<h6 class="text-bold">Administration Vaccine</h6>' + "<div id='AllAdministerVaccine'></div>");
        var $mainDivDocumentHx = $(document.createElement('div'));
        $mainDivDocumentHx.attr('id', "Section_VaccineDocumentHx");
        $mainDivDocumentHx.append('<h6 class="text-bold">Document Hx Vaccine</h6>' + "<div id='AllDocumentHxVaccine'></div>");

        var $mainDivRefusal = $(document.createElement('div'));
        $mainDivRefusal.attr('id', "Section_VaccineRefusal");
        $mainDivRefusal.append('<h6 class="text-bold">Refusal Vaccine</h6>' + "<div id='AllRefusalVaccine'></div>");

        var $mainDivVoidDose = $(document.createElement('div'));
        $mainDivVoidDose.attr('id', "Section_VaccineVoidDose");
        $mainDivVoidDose.append('<h6 class="text-bold">Void Dose</h6>' + "<div id='AllVoidDoseVaccine'></div>");


        var $mainDivTherapeuticInjection = $(document.createElement('div'));
        $mainDivTherapeuticInjection.attr('id', "Section_TherapeuticInjection");
        $mainDivTherapeuticInjection.append('<h6 class="text-bold">Therapeutic Injection</h6>' + "<div id='AllTherapeuticInjection'></div>");

        var $mainDivTherapeuticInjectionHistory = $(document.createElement('div'));
        $mainDivTherapeuticInjectionHistory.attr('id', "Section_TherapeuticInjectionHistory");
        $mainDivTherapeuticInjectionHistory.append('<h6 class="text-bold">Therapeutic Injection History</h6>' + "<div id='AllTherapeuticInjectionHistory'></div>");


        var $DivNewTherapeuticInjection = $(document.createElement('div'));
        var $DivNewTherapeuticInjectionHistory = $(document.createElement('div'));
        var $DivNewAdminister = $(document.createElement('div'));
        var $DivNewDocumentHx = $(document.createElement('div'));
        var $DivNewRefusal = $(document.createElement('div'));
        var $DivNewVoidDose = $(document.createElement('div'));


        if ((Vaccinesoap_JSON == null || Vaccinesoap_JSON.length == 0) && ((TheraeuticInjectionLoad_JSON == null || TheraeuticInjectionLoad_JSON.length == 0))) {
            if (!bNotSaveCompt) {
                $.when(Clinical_ProgressNote.saveComponentSOAPText("Immunization", hideAlertMessage)).then(function () {
                    dfd.resolve();
                });
            }
            else {
                dfd.resolve();
            }

            return dfd.promise();
        }

        if (Vaccinesoap_JSON.length > 0 || ((TheraeuticInjectionLoad_JSON.length > 0))) {
            var PListId = [];
            var def = [];

            $.each(Vaccinesoap_JSON, function (index, element) {
                var color = "";
                var PLid = element.VaccineHxId;
                var $SectionBodyVital = $(document.createElement('section'));
                $SectionBodyVital.attr('id', "Cli_Immunization_Main" + PLid);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_Immunization_" + PLid);
                var $ListVital = $(document.createElement('ul'));

                $ListVital.attr('class', 'list-unstyled')

                $SectionBodyVital.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Immunization_" + PLid + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Immunization_Main" + PLid + '"  ><i class="fa fa-times"></i></a></div> ');

                'Patient underwent <ProcedureCode ProcedureName> based on the following assessment: <Diagnosis> from <From> to <To>. Comments: <Comments>'
                if (element.Type.toLowerCase() == "administer" && (element.VoidDose == "False" || element.VoidDose == false)) {
                    $ListVital.append("<li>" +
                        (element.VaccineName == '' ? "" : "<b>" + element.VaccineName + "</b>") +
                        ((element.CPT == '' || element.CPT == null) ? "" : " <b>(" + element.CPT.substring(0, element.CPT.length - 1) + ") </b>") +
                        (element.Dose == '' ? "" : ", Dose: " + element.Dose + " " + element.Amount) +
                         (!(element.RouteDescription) ? "" : ", Route: " + element.RouteDescription) +
                         (!(element.SiteDescription) ? "" : ", Site: " + element.SiteDescription) +
                          (!(element.LotNumber) ? "" : ", Lot Number: " + element.LotNumber) +
                          (!(element.ManufacturerName) ? "" : ", Manufacturer: " + element.ManufacturerName) +
                        (element.ProviderName == '' ? "" : ", administrated by " + element.ProviderName) +
                        (element.AdministrationDate == '' ? "" : " on " + (moment(element.AdministrationDate).format("MM/DD/YYYY, hh:mm:ss A")))

                        );
                }
                else if (element.Type.toLowerCase() == "documenthx" && (element.VoidDose == "False" || element.VoidDose == false)) {
                    $ListVital.append("<li>" +
                        (element.VaccineName == '' ? "" : "<b>" + element.VaccineName + "</b>") +
                        (element.Dose == '' ? "" : ", Dose: " + element.Dose + " " + element.Amount) +
                         (!(element.RouteDescription) ? "" : ", Route: " + element.RouteDescription) +
                         (!(element.SiteDescription) ? "" : ", Site: " + element.SiteDescription) +
                          (!(element.LotNumber) ? "" : ", Lot Number: " + element.LotNumber) +
                          (!(element.ManufacturerName) ? "" : ", Manufacturer: " + element.ManufacturerName) +
                        (element.ProviderName == '' ? "" : ", administrated by " + element.ProviderName) + 
                        (element.AdministrationDate == '' ? "" : " on " + (moment(element.AdministrationDate).format("MM/DD/YYYY, hh:mm:ss A")))
                        );
                }
                else if (element.Type.toLowerCase() == "refusal" && (element.VoidDose == "False" || element.VoidDose == false)) {
                    $ListVital.append("<li>" +
                        (element.VaccineName == '' ? "" : "<b>" + element.VaccineName + "</b>") +
                        (element.RefusalReason == '' ? "" : ", not given to the patient due to " + element.RefusalReason)
                        );
                }
                else if (element.VoidDose == "True" || element.VoidDose == true) {
                    $ListVital.append("<li>" +
                       (element.VaccineName == '' ? "" : "<b>" + element.VaccineName + "</b>") +
                        (element.Dose == '' ? "" : ", Dose: " + element.Dose + " " + element.Amount) +
                         (!(element.RouteDescription) ? "" : ", Route: " + element.RouteDescription) +
                         (!(element.SiteDescription) ? "" : ", Site: " + element.SiteDescription) +
                          (!(element.LotNumber) ? "" : ", Lot Number: " + element.LotNumber) +
                          (!(element.ManufacturerName) ? "" : ", Manufacturer: " + element.ManufacturerName) +
                        (element.ProviderName == '' ? "" : ", administrated by " + element.ProviderName) +
                        (element.AdministrationDate == '' ? "" : " on " + (moment(element.AdministrationDate).format("MM/DD/YYYY, hh:mm:ss A"))) +
                        (element.Comments == '' ? "" : "</br> Reason: " + element.Comments)
                        );
                }


                $DetailsDiv.append($ListVital);
                $SectionBodyVital.append($DetailsDiv);

                var MainDivId = "";
                if (element.Type.toLowerCase() == "administer" && (element.VoidDose == "False" || element.VoidDose == false)) {
                    MainDivId = "AllAdministerVaccine";
                }
                else if (element.Type.toLowerCase() == "documenthx" && (element.VoidDose == "False" || element.VoidDose == false)) {
                    MainDivId = "AllDocumentHxVaccine";
                }
                else if (element.Type.toLowerCase() == "refusal" && (element.VoidDose == "False" || element.VoidDose == false)) {
                    MainDivId = "AllRefusalVaccine";
                }
                else if (element.VoidDose == "True" || element.VoidDose == true) {
                    MainDivId = "AllVoidDoseVaccine";
                }
                if (PLid != "") {
                    PListId.push(PLid);
                    if ($(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#' + MainDivId + ' #Cli_Immunization_Main' + PLid).length == 0) {
                        if (element.Type.toLowerCase() == "administer" && (element.VoidDose == "False" || element.VoidDose == false)) {
                            $DivNewAdminister.append($SectionBodyVital);
                        }
                        else if (element.Type.toLowerCase() == "documenthx" && (element.VoidDose == "False" || element.VoidDose == false)) {
                            $DivNewDocumentHx.append($SectionBodyVital);
                        }
                        else if (element.Type.toLowerCase() == "refusal" && (element.VoidDose == "False" || element.VoidDose == false)) {
                            $DivNewRefusal.append($SectionBodyVital);
                        }
                        else if (element.VoidDose == "True" || element.VoidDose == true) {
                            $DivNewVoidDose.append($SectionBodyVital);
                        }

                    } else {
                        var CommentHTML = "";
                        var CommentsID = $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + PLid + ' ul li:Last').attr('id');
                        if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                            CommentHTML = $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + PLid + ' ul li:Last').get(0).outerHTML;
                        }
                        $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + PLid).html($SectionBodyVital.html());
                        $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + PLid + ' ul').append(CommentHTML);
                    }
                }
            });

            $.when.apply($, def).done(function ($n) {

                if (TheraeuticInjectionLoad_JSON.length > 0) {
                    $.each(TheraeuticInjectionLoad_JSON, function (index, element1) {

                        var color = "";
                        var TheraInjectionid = element1.ImmTherInjectionId + "thera";
                        PListId.push(TheraInjectionid);
                        var $SectionBodyVital = $(document.createElement('section'));
                        $SectionBodyVital.attr('id', "Cli_Immunization_Main" + TheraInjectionid);
                        var $DetailsDiv = $(document.createElement('div'));
                        $DetailsDiv.attr('id', "Cli_Immunization_" + TheraInjectionid);
                        var $ListVital = $(document.createElement('ul'));

                        $ListVital.attr('class', 'list-unstyled')

                        $SectionBodyVital.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Immunization_" + TheraInjectionid + '"><i class="fa fa-edit"></i></a>' +
                            '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Immunization_Main" + TheraInjectionid + '"  ><i class="fa fa-times"></i></a></div> ');
                        var _InjecNameAndCode = (element1.CPTCode == '' ? (element1.TherapeuticInjection == '' ? "" : element1.TherapeuticInjection) : element1.CPTCode + " - " + element1.TherapeuticInjection);
                        'Patient underwent <ProcedureCode ProcedureName> based on the following assessment: <Diagnosis> from <From> to <To>. Comments: <Comments>'
                        $ListVital.append("<li>" + "<b>" + _InjecNameAndCode + "</b>" +
                       (element1.Dose == '' ? "" : ", Dose: " + element1.Dose + " " + element1.Amount) +
                         (!(element1.RouteDescription) ? "" : ", Route: " + element1.RouteDescription) +
                         (!(element1.SiteDescription) ? "" : ", Site: " + element1.SiteDescription) +
                          (!(element1.LotNumber) ? "" : ", Lot Number: " + element1.LotNumber) +
                          (!(element1.ManufacturerName) ? "" : ", Manufacturer: " + element1.ManufacturerName) +
                        (element1.ProviderName == '' ? "" : ", administrated by " + element1.ProviderName) + 
                         (element1.AdministrationDate == '' ? "" : " on " + (moment(element1.AdministrationDate).format("MM/DD/YYYY, hh:mm:ss A")))
                       );

                        $DetailsDiv.append($ListVital);
                        $SectionBodyVital.append($DetailsDiv);

                        var MainDivId = "";
                        if (element1.Type == "Administered") {
                            MainDivId = "AllTherapeuticInjection";
                            if ($(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#' + MainDivId + ' #Cli_Immunization_Main' + TheraInjectionid).length == 0) {
                                $DivNewTherapeuticInjection.append($SectionBodyVital);
                            } else {
                                var CommentHTML = "";
                                var CommentsID = $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + TheraInjectionid + ' ul li:Last').attr('id');
                                if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                                    CommentHTML = $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + TheraInjectionid + ' ul li:Last').get(0).outerHTML;
                                }
                                $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + TheraInjectionid).html($SectionBodyVital.html());
                                $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + TheraInjectionid + ' ul').append(CommentHTML);
                            }
                        }
                        else {
                            MainDivId = "AllTherapeuticInjectionHistory";
                            if ($(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#' + MainDivId + ' #Cli_Immunization_Main' + TheraInjectionid).length == 0) {
                                $DivNewTherapeuticInjectionHistory.append($SectionBodyVital);
                            } else {
                                var CommentHTML = "";
                                var CommentsID = $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + TheraInjectionid + ' ul li:Last').attr('id');
                                if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                                    CommentHTML = $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + TheraInjectionid + ' ul li:Last').get(0).outerHTML;
                                }
                                $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + TheraInjectionid).html($SectionBodyVital.html());
                                $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().find('#Cli_Immunization_Main' + TheraInjectionid + ' ul').append(CommentHTML);
                            }
                        }
                    });

                    //Start//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
                    if (PListId.join(",") != "") {
                        ImmunizationId = PListId.join(",");
                    }
                    //End//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
                    if ($DivNewTherapeuticInjectionHistory.html() != '' || $DivNewTherapeuticInjection.html() != '' || $DivNewAdminister.html() != '' || $DivNewDocumentHx.html() != '' || $DivNewRefusal.html() != '' || $DivNewVoidDose.html() != '') {

                        if ($DivNewAdminister.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineAdminister').length == 0) {
                                $mainDivVital.append($mainDivAdminister);
                                $mainDivVital.find('#AllAdministerVaccine').append($DivNewAdminister);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineAdminister')[0].outerHTML);
                                $mainDivVital.find('#AllAdministerVaccine').append($DivNewAdminister);
                            }
                        }
                        else {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineAdminister').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineAdminister')[0].outerHTML);
                            }
                        }
                        if ($DivNewDocumentHx.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineDocumentHx').length == 0) {
                                $mainDivVital.append($mainDivDocumentHx);
                                $mainDivVital.find('#AllDocumentHxVaccine').append($DivNewDocumentHx);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineDocumentHx')[0].outerHTML);
                                $mainDivVital.find('#AllDocumentHxVaccine').append($DivNewDocumentHx);
                            }
                        } else {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineDocumentHx').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineDocumentHx')[0].outerHTML);
                            }
                        }

                        if ($DivNewRefusal.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineRefusal').length == 0) {
                                $mainDivVital.append($mainDivRefusal);
                                $mainDivVital.find('#AllRefusalVaccine').append($DivNewRefusal);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineRefusal')[0].outerHTML);
                                $mainDivVital.find('#AllRefusalVaccine').append($DivNewRefusal);
                            }
                        } else {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineRefusal').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineRefusal')[0].outerHTML);
                            }
                        }

                        if ($DivNewVoidDose.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineVoidDose').length == 0) {
                                $mainDivVital.append($mainDivVoidDose);
                                $mainDivVital.find('#AllVoidDoseVaccine').append($DivNewVoidDose);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineVoidDose')[0].outerHTML);
                                $mainDivVital.find('#AllVoidDoseVaccine').append($DivNewVoidDose);
                            }
                        } else {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineVoidDose').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineVoidDose')[0].outerHTML);
                            }
                        }
                        if ($DivNewTherapeuticInjection.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_TherapeuticInjection').length == 0) {
                                $mainDivVital.append($mainDivTherapeuticInjection);
                                $mainDivVital.find('#AllTherapeuticInjection').append($DivNewTherapeuticInjection);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_TherapeuticInjection')[0].outerHTML);
                                $mainDivVital.find('#AllTherapeuticInjection').append($DivNewTherapeuticInjection);
                            }
                        }
                        else {
                            if ($(NoteHTMLCtrl).find('#Section_TherapeuticInjection').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_TherapeuticInjection')[0].outerHTML);
                            }
                        }
                        if ($DivNewTherapeuticInjectionHistory.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_TherapeuticInjectionHistory').length == 0) {
                                $mainDivVital.append($mainDivTherapeuticInjectionHistory);
                                $mainDivVital.find('#AllTherapeuticInjectionHistory').append($DivNewTherapeuticInjectionHistory);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_TherapeuticInjectionHistory')[0].outerHTML);
                                $mainDivVital.find('#AllTherapeuticInjectionHistory').append($DivNewTherapeuticInjectionHistory);
                            }
                        }
                        else {
                            if ($(NoteHTMLCtrl).find('#Section_TherapeuticInjectionHistory').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_TherapeuticInjectionHistory')[0].outerHTML);
                            }
                        }
                        //Start//04//01//2015//Ahmad Raza//
                        $.when(Clinical_ImmunizationDetail.setImmunizationHtml($mainDivVital.html(), ImmunizationId, Attached_Vaccines, Attached_Injections, NoteHTMLCtrl, hideAlertMessage, bNotSaveCompt)).then(function () {
                            dfd.resolve();
                        });
                        if (typeof comeFromProgressNote != typeof undefined && comeFromProgressNote != null && comeFromProgressNote) {
                            Clinical_ProgressNote.saveComponentSOAPText("Immunization", hideAlertMessage);
                        }
                    } else {
                        $.when(Clinical_ImmunizationDetail.setImmunizationHtml('', ImmunizationId, Attached_Vaccines, Attached_Injections, NoteHTMLCtrl, hideAlertMessage, bNotSaveCompt)).then(function () {
                            dfd.resolve();
                        });
                    }


                }
                else {

                    //Start//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
                    if (PListId.join(",") != "") {
                        ImmunizationId = PListId.join(",");
                    }
                    //End//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
                    if ($DivNewAdminister.html() != '' || $DivNewDocumentHx.html() != '' || $DivNewRefusal.html() != '' || $DivNewVoidDose.html() != '') {
                        if ($DivNewAdminister.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineAdminister').length == 0) {
                                $mainDivVital.append($mainDivAdminister);
                                $mainDivVital.find('#AllAdministerVaccine').append($DivNewAdminister);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineAdminister')[0].outerHTML);
                                $mainDivVital.find('#AllAdministerVaccine').append($DivNewAdminister);
                            }
                        }
                        else {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineAdminister').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineAdminister')[0].outerHTML);
                            }
                        }
                        if ($DivNewDocumentHx.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineDocumentHx').length == 0) {
                                $mainDivVital.append($mainDivDocumentHx);
                                $mainDivVital.find('#AllDocumentHxVaccine').append($DivNewDocumentHx);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineDocumentHx')[0].outerHTML);
                                $mainDivVital.find('#AllDocumentHxVaccine').append($DivNewDocumentHx);
                            }
                        } else {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineDocumentHx').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineDocumentHx')[0].outerHTML);
                            }
                        }

                        if ($DivNewRefusal.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineRefusal').length == 0) {
                                $mainDivVital.append($mainDivRefusal);
                                $mainDivVital.find('#AllRefusalVaccine').append($DivNewRefusal);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineRefusal')[0].outerHTML);
                                $mainDivVital.find('#AllRefusalVaccine').append($DivNewRefusal);
                            }
                        } else {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineRefusal').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineRefusal')[0].outerHTML);
                            }
                        }

                        if ($DivNewVoidDose.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineVoidDose').length == 0) {
                                $mainDivVital.append($mainDivVoidDose);
                                $mainDivVital.find('#AllVoidDoseVaccine').append($DivNewVoidDose);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineVoidDose')[0].outerHTML);
                                $mainDivVital.find('#AllVoidDoseVaccine').append($DivNewVoidDose);
                            }
                        } else {
                            if ($(NoteHTMLCtrl).find('#Section_VaccineVoidDose').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_VaccineVoidDose')[0].outerHTML);
                            }
                        }
                        if ($DivNewTherapeuticInjection.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_TherapeuticInjection').length == 0) {
                                $mainDivVital.append($mainDivTherapeuticInjection);
                                $mainDivVital.find('#AllTherapeuticInjection').append($DivNewTherapeuticInjection);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_TherapeuticInjection')[0].outerHTML);
                                $mainDivVital.find('#AllTherapeuticInjection').append($DivNewTherapeuticInjection);
                            }
                        }
                        else {
                            if ($(NoteHTMLCtrl).find('#Section_TherapeuticInjection').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_TherapeuticInjection')[0].outerHTML);
                            }
                        }
                        if ($DivNewTherapeuticInjectionHistory.html() != '') {
                            if ($(NoteHTMLCtrl).find('#Section_TherapeuticInjectionHistory').length == 0) {
                                $mainDivVital.append($mainDivTherapeuticInjectionHistory);
                                $mainDivVital.find('#AllTherapeuticInjectionHistory').append($DivNewTherapeuticInjectionHistory);
                            }
                            else {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_TherapeuticInjectionHistory')[0].outerHTML);
                                $mainDivVital.find('#AllTherapeuticInjectionHistory').append($DivNewTherapeuticInjectionHistory);
                            }
                        }
                        else {
                            if ($(NoteHTMLCtrl).find('#Section_TherapeuticInjectionHistory').length != 0) {
                                $mainDivVital.append($(NoteHTMLCtrl).find('#Section_TherapeuticInjectionHistory')[0].outerHTML);
                            }
                        }
                        //Start//04//01//2015//Ahmad Raza//
                        $.when(Clinical_ImmunizationDetail.setImmunizationHtml($mainDivVital.html(), ImmunizationId, Attached_Vaccines, Attached_Injections, NoteHTMLCtrl, hideAlertMessage, bNotSaveCompt)).then(function () {
                            dfd.resolve();

                        });
                        if (typeof comeFromProgressNote != typeof undefined && comeFromProgressNote != null && comeFromProgressNote) {
                            Clinical_ProgressNote.saveComponentSOAPText("Immunization", hideAlertMessage);
                        }
                    } else {
                        $.when(Clinical_ImmunizationDetail.setImmunizationHtml('', ImmunizationId, Attached_Vaccines, Attached_Injections, NoteHTMLCtrl, hideAlertMessage, bNotSaveCompt)).then(function () {
                            dfd.resolve();
                        });
                        Clinical_ProgressNote.saveComponentSOAPText("Immunization", hideAlertMessage);
                    }


                }
            });

        }
        else {
            dfd.resolve();
        }
        return dfd.promise();
    },

    setImmunizationHtml: function (ImmunizationHtml, ImmunizationId, Attached_Vaccines, Attached_Injections, NoteHTMLCtrl, hideAlertMessage, bNotSaveCompt) {
        var dfd = $.Deferred();
        $(NoteHTMLCtrl + ' Clinical_Immunization').parent().parent().addClass('initialVisitBody');
        if (ImmunizationHtml != '') {
            //var divImmunization = $(NoteHTMLCtrl + ' Clinical_Immunization a:nth-child(1)')[0].outerHTML + $(NoteHTMLCtrl + ' Clinical_Immunization a:nth-child(2)')[0].outerHTML + $(NoteHTMLCtrl + ' Clinical_Immunization a:nth-child(3)')[0].outerHTML;
            var divImmunization = ""
            if ($(NoteHTMLCtrl + ' Clinical_Immunization a:nth-child(1)').length > 0)
                divImmunization += $(NoteHTMLCtrl + ' Clinical_Immunization a:nth-child(1)')[0].outerHTML;
            if ($(NoteHTMLCtrl + ' Clinical_Immunization a:nth-child(2)').length > 0)
                divImmunization += $(NoteHTMLCtrl + ' Clinical_Immunization a:nth-child(2)')[0].outerHTML;
            if ($(NoteHTMLCtrl + ' Clinical_Immunization a:nth-child(3)').length > 0)
                divImmunization += $(NoteHTMLCtrl + ' Clinical_Immunization a:nth-child(3)')[0].outerHTML;
            $(NoteHTMLCtrl + ' Clinical_Immunization').html(divImmunization);
            //$(NoteHTMLCtrl + ' Clinical_Immunization').in.append(ImmunizationHtml);
            $(NoteHTMLCtrl).find('#Section_VaccineAdminister').remove();
            $(NoteHTMLCtrl).find('#Section_VaccineDocumentHx').remove();
            $(NoteHTMLCtrl).find('#Section_VaccineRefusal').remove();
            $(NoteHTMLCtrl).find('#Section_VaccineVoidDose').remove();
            $(NoteHTMLCtrl).find('#Section_TherapeuticInjection').remove();
            $(NoteHTMLCtrl).find('#Section_TherapeuticInjectionHistory').remove();
            $(ImmunizationHtml).insertAfter($(NoteHTMLCtrl + ' Clinical_Immunization').parent());
        }

        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (ImmunizationHtml != '') {

            var splitAdministerVaccineId = ImmunizationId.split(",");
            var AdministerVaccineIdForImmunization = [];
            var AdministerVaccineIdForTherapeuticInjection = [];
            $.each(ImmunizationId.split(","), function (i, item) {
                if (item.indexOf("thera") > -1) {
                    AdministerVaccineIdForTherapeuticInjection.push(item.replace("thera", ""));
                }
                else {
                    AdministerVaccineIdForImmunization.push(item);
                }
            });

            if (Attached_Vaccines.length > 0) {

                if (!bNotSaveCompt)
                    Clinical_ProgressNote.saveComponentSOAPText("Immunization", hideAlertMessage);

                $('#' + AdministerVaccineIdForImmunization.join()).remove();
            }

            if (Attached_Injections.length > 0) {

                if (!bNotSaveCompt)
                    Clinical_ProgressNote.saveComponentSOAPText("Immunization", hideAlertMessage);

                var copyAdministerVaccineIdForTherapeuticInjection = [];
                $.each(AdministerVaccineIdForTherapeuticInjection, function (i, item) {
                    copyAdministerVaccineIdForTherapeuticInjection.push(item + "thera");
                });

                $('#' + copyAdministerVaccineIdForTherapeuticInjection.join()).remove();

            }
        }

        dfd.resolve();
        return dfd.promise();
    },

    /* Retrieves latest Immunization data against the PatientId
    Author: Muhammad Ahmad Imran */
    getLatestClinical_ImmunizationByPatientId_DBCall: function () {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["UserId"] = globalAppdata["AppUserId"];
        objData["commandType"] = "getlatest_Immunizationby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");
    },

    attachImmunizationWithNotes_DBCall: function (ProceduresId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["VaccineHxIds"] = ProceduresId;
        objData["commandType"] = "attach_Vaccine_with_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");
    },

    attachTherapeuticInjectionWithNotes_DBCall: function (ImmTherInjectionId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["ImmTherInjectionId"] = ImmTherInjectionId;
        objData["commandType"] = "attach_Thera_Injection_with_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "IMMUNIZATIONTHERAPEUTICINJECTION");
    },

    detachImmunizationFromNotes_DBCall: function (ProceduresId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["VaccineHxIds"] = ProceduresId;
        objData["commandType"] = "detach_Vaccine_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");
    },

    detachTherapeuticInjectionFromNotes_DBCall: function (ImmTherInjectionId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["ImmTherInjectionId"] = ImmTherInjectionId;
        objData["commandType"] = "detach_Thera_Injection_with_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "IMMUNIZATIONTHERAPEUTICINJECTION");
    },

    //Start//22/03/2016//Talha Tanweer//Implimented Call to Controller for Vaccine Administer Save  Detail
    //SaveAdministerVaccine: function (PatientAge) {

    //    var objData = {};
    //    var self = $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #frmVaccineHxAdministerTabDetail');
    //    favoriteListId = self.find('#hfFavoriteListId').val();

    //    var self = $(' #frmVaccineHxAdministerTabDetail'); // $('#pnlFavoriteProcedureOrderDetail');
    //    var myJSON = self != null ? self.getMyJSONByName() : "{}";
    //    var objDetail = JSON.parse(myJSON);

    //    objData["CategoryID"] = objDetail["AdministerVaccine_Category"];
    //    objData["VisitDate"] = objDetail["AdministerVaccine_VisitDate_text"];
    //    objData["VisitDateId"] = objDetail["AdministerVaccine_VisitDate"];
    //    objData["ProviderId"] = objDetail["AdministerVaccine_Provider"];
    //    objData["VaccineID"] = objDetail["AdministerVaccine_Vaccine"];
    //    objData["AdministrationDate"] = objDetail["AdministerVaccine_AdministrationDate"];
    //    objData["Time"] = objDetail["AdministerVaccine_AdministrationTime"];
    //    objData["Dose"] = objDetail["AdministerVaccine_Dose"];
    //    objData["Amount"] = objDetail["AdministerVaccine_Amount"];
    //    objData["LotNo"] = objDetail["AdministerVaccine_LotNumber"];
    //    objData["ManufacturerId"] = $("#frmVaccineHxAdministerTabDetail #hfAdministerTabManufacturer").val();
    //    objData["RouteId"] = objDetail["AdministerVaccine_Route"];
    //    objData["SiteId"] = objDetail["AdministerVaccine_Site"];
    //    objData["ExpiryDate"] = objDetail["AdministerVaccine_ExpiryDate"];
    //    objData["VfcId"] = objDetail["AdministerVaccine_VFC"];
    //    objData["VisDateId"] = $("#" + Clinical_ImmunizationDetail.params.PanelID + " #VisDateId").val();
    //    objData["Reaction"] = objDetail["AdministerReaction"];//Test In Model
    //    objData["VoidDose"] = objDetail["AdministerVoidDose"];//Test In Model
    //    objData["Comments"] = objDetail["AdministerVaccine_Comments"];
    //    objData["PublicityCode"] = objDetail["AdministerVaccine_PublicityCode"];
    //    objData["PublicityCodeExpiryDate"] = objDetail["AdministerVaccine_PublicityExpiryDate"];
    //    objData["ImmunizationRegistryStatusCode"] = objDetail["AdministerVaccine_IRS"];
    //    objData["IRSEffectiveDate"] = objDetail["AdministerVaccine_IRSEffectiveDate"];
    //    objData["ProtectionIndicator"] = objDetail["AdministerVaccine_ProtectionIndicator"];
    //    objData["PIEffectiveDate"] = objDetail["AdministerVaccine_PIEffectiveDate"];
    //    objData["IsActive"] = objDetail["AdministerVaccine_IsActive"] == true ? 1 : 0;
    //    if ((typeof Clinical_ImmunizationDetail.params["VaccineScheduleId"] == typeof undefined || Clinical_ImmunizationDetail.params["VaccineScheduleId"] == null || Clinical_ImmunizationDetail.params["VaccineScheduleId"] == "") && (typeof Clinical_ImmunizationDetail.params.OrderSetId != typeof undefined && Clinical_ImmunizationDetail.params.OrderSetId != null)) {
    //        objData["VaccineScheduleId"] = "";
    //    }
    //    else {
    //        objData["VaccineScheduleId"] = Clinical_ImmunizationDetail.params["VaccineScheduleId"] === 0 ? "" : Clinical_ImmunizationDetail.params["VaccineScheduleId"];
    //    }

    //    objData["Type"] = Clinical_ImmunizationDetail.AdministerText;
    //    objData["OverrideRule"] = objDetail["AdministerOverrideRule"];


    //    if (typeof GetSelectedPatientID() === "undefined") {
    //        objData["PatientId"] = Clinical_ImmunizationDetail.params.patientID;
    //    }
    //    else {
    //        objData["PatientId"] = GetSelectedPatientID();//163; //
    //    }





    //    if (Clinical_ImmunizationDetail.params.mode == "Add")
    //        objData["commandType"] = "Save_AdministerVaccine";
    //    else {
    //        objData["VaccineHxId"] = Clinical_ImmunizationDetail.params.VaccineHxId;
    //        if (typeof Clinical_ImmunizationDetail.params.OrderSetId != typeof undefined && Clinical_ImmunizationDetail.params.OrderSetId != null) {
    //            objData["OrdersetId"] = Clinical_ImmunizationDetail.params.OrderSetId;
    //        }
    //        else {
    //            objData["OrdersetId"] = "";
    //        }
    //        objData["commandType"] = "update_VacinehxDose";
    //    }

    //    objData["PatientAge"] = PatientAge;
    //    var data = JSON.stringify(objData);
    //    return MDVisionService.APIService(data, "MEDICAL", "Immunization");


    //},
    //End Talha Tanweer 22/03/2016

    //Start//22/03/2016//Talha Tanweer//Implimented Call to Controller for Vaccine Administer Save  Detail
    SaveVaccineHxDose: function (PatientAge) {
        //FavoriteListIcd
        var objData = {};
        var self = $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #frmVaccineHxDocumentHxDoseTabDetail');
        favoriteListId = self.find('#hfFavoriteListId').val();

        var self = $(' #frmVaccineHxDocumentHxDoseTabDetail'); // $('#pnlFavoriteProcedureOrderDetail');
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objDetail = JSON.parse(myJSON);

        objData["CategoryID"] = objDetail["DocumentHxDose_Category"];
        objData["SourceOfHxId"] = objDetail["DocumentHxDose_SourceOfHx"];
        objData["ProviderId"] = objDetail["DocumentHxDose_Provider"];
        objData["VaccineID"] = objDetail["DocumentHxDose_Vaccine"];
        objData["AdministrationDate"] = objDetail["DocumentHxDose_AdministrationDate"];
        objData["Time"] = objDetail["DocumentHxDose_AdministrationTime"];
        objData["Dose"] = objDetail["DocumentHxDose_Dose"];
        objData["Amount"] = objDetail["DocumentHxDose_Amount"];
        objData["RouteId"] = objDetail["DocumentHxDose_Route"];
        objData["SiteId"] = objDetail["DocumentHxDose_Site"];
        objData["VoidDose"] = objDetail["DocumentHxVoidDose"];
        objData["Comments"] = objDetail["DocumentHxDose_Comments"];
        objData["VaccineScheduleId"] = Clinical_ImmunizationDetail.params["VaccineScheduleId"] === 0 ? "" : Clinical_ImmunizationDetail.params["VaccineScheduleId"];
        objData["Type"] = Clinical_ImmunizationDetail.DocumentHxDoseText;
        if (typeof GetSelectedPatientID() === "undefined") {
            objData["PatientId"] = Clinical_ImmunizationDetail.params.patientID;
        }
        else {
            objData["PatientId"] = GetSelectedPatientID();//163; //
        }
        if (Clinical_ImmunizationDetail.params.TabId == "Batch") {
            objData["PatientId"] = Clinical_ImmunizationDetail.params.patientID;
        }

        objData["IsActive"] = objDetail["DocumentHxDose_IsActive"] == true ? 1 : 0;
        if (Clinical_ImmunizationDetail.params.mode == "Add")
            objData["commandType"] = "Save_VacineHxDose";
        else {

            objData["VaccineHxId"] = Clinical_ImmunizationDetail.params.VaccineHxId;
            objData["commandType"] = "update_VacinehxDose";
        }



        objData["PatientAge"] = PatientAge;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");

    },
    //End Talha Tanweer 22/03/2016

    SaveVaccineRefusalRecord: function (PatientAge) {
        //FavoriteListIcd
        var objData = {};
        var self = $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #frmVaccineHxDocumentHxDoseTabDetail');
        favoriteListId = self.find('#hfFavoriteListId').val();

        var self = $(' #frmVaccineRecordRefusalTabDetail'); // $('#pnlFavoriteProcedureOrderDetail');
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objDetail = JSON.parse(myJSON);

        objData["CategoryID"] = objDetail["RecordRefusal_Category"];
        objData["ProviderId"] = objDetail["RecordRefusal_Provider"];
        objData["VaccineID"] = objDetail["RecordRefusal_Vaccine"];
        objData["RefusalReasonID"] = objDetail["RecordRefusalReason"];
        objData["ExpiryDate"] = objDetail["RecordRefusalVaccine_ExpiryDate"];
        objData["VoidDose"] = objDetail["RecordRefusalVoidDose"];
        objData["Comments"] = objDetail["RecordRefusal_Comments"];
        objData["VaccineScheduleId"] = Clinical_ImmunizationDetail.params["VaccineScheduleId"] === 0 ? "" : Clinical_ImmunizationDetail.params["VaccineScheduleId"];
        objData["Type"] = Clinical_ImmunizationDetail.REFUSALText;

        if (typeof GetSelectedPatientID() === "undefined") {
            objData["PatientId"] = Clinical_ImmunizationDetail.params.patientID;
        }
        else {
            objData["PatientId"] = GetSelectedPatientID();//163; //
        }
        if (Clinical_ImmunizationDetail.params.TabId == "Batch") {
            objData["PatientId"] = Clinical_ImmunizationDetail.params.patientID;
        }

        objData["IsActive"] = objDetail["RecordRefusal_IsActive"] == true ? 1 : 0;

        if (Clinical_ImmunizationDetail.params.mode == "Add")
            objData["commandType"] = "save_vacinerefusalrecord";
        else {

            objData["VaccineHxId"] = Clinical_ImmunizationDetail.params.VaccineHxId;
            objData["commandType"] = "update_vacinerefusalrecord";
        }



        objData["PatientAge"] = PatientAge;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");

    },

    // Abid Ali
    bindVaccineHxDetails: function (response) {

        var selfAdminForm = $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail");
        var selfDocumentForm = $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail");
        var selfRefusalForm = $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail");
        response = JSON.parse(response);
        if (response.status != false) {

            // Bind with Admin tab

            if (Clinical_ImmunizationDetail.params.Type.trim() == "ADMINISTER") {

                var details = JSON.parse(response.AdminVaccineHxLoad_JSON)[0];
                $('#' + Clinical_ImmunizationDetail.params.PanelID + " #hfVaccineHx").val(details.VaccineHxId);
                utility.bindMyJSONByName(true, details, false, selfAdminForm).done(function () {
                    $.when(Clinical_ImmunizationDetail.PopulateVISDate_VISURL_and_ManufacturerDropDown(details.AdministerVaccine_Vaccine, 'ADMINISTER', true)).then(function () {
                        $('#' + Clinical_ImmunizationDetail.params.PanelID + " #txtAdministerVaccine_Dose").val(details.AdministerVaccine_Dose);
                        $('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_Amount").val(details.AdministerVaccine_Amount);

                        $('#' + Clinical_ImmunizationDetail.params.PanelID + " #hfAdministerTabManufacturer").val(details.AdministerVaccine_Manufacturer);
                        $('#' + Clinical_ImmunizationDetail.params.PanelID + " #AdministerTabManufacturer").val(details.AdministerTabManufacturer);
                        if ($('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_LotNumber option[value='" + details.AdministerVaccine_LotNumber + "']").length == 0) {
                            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #ddlAdministerVaccine_LotNumber').append('<option selected value="' + details.AdministerVaccine_LotNumber + '">' + details.LotText + '</option>');
                        }
                        $('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_LotNumber").val(details.AdministerVaccine_LotNumber);
                        $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail").bootstrapValidator('revalidateField', 'AdministerVaccine_LotNumber');
                        setTimeout(function () {
                            $('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_Route").val(details.AdministerVaccine_Route);
                        }, 500);




                    });

                    $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #dpAdministerVaccine_AdministrationDate').datepicker('setDate', $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #dpAdministerVaccine_AdministrationDate').val());
                    $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #dpAdministerVaccine_ExpiryDate').datepicker('setDate', $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #dpAdministerVaccine_ExpiryDate').val());
                    $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tpAdministerVaccine_AdministrationTime').timepicker('setTime', $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tpAdministerVaccine_AdministrationTime').val());


                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_Vaccine").attr("disabled", true);
                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail #ddlDocumentHxDose_Vaccine").attr("disabled", true);
                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail #ddlRecordRefusal_Vaccine").attr("disabled", true);

                    if (details.AdministerVoidDose == "0") {
                        Clinical_ImmunizationDetail.previousVoid = false;
                    }
                    else if (details.AdministerVoidDose == "1") {
                        Clinical_ImmunizationDetail.previousVoid = true;
                    }

                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_Vaccine").val(details.AdministerVaccine_Vaccine);
                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail #ddlDocumentHxDose_Vaccine").val(details.AdministerVaccine_Vaccine);
                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail #ddlRecordRefusal_Vaccine").val(details.AdministerVaccine_Vaccine);




                });

            }
            else if (Clinical_ImmunizationDetail.params.Type.trim() == "DOCUMENTHX") {

                var details = JSON.parse(response.DocVaccineHxLoad_JSON)[0];
                $('#' + Clinical_ImmunizationDetail.params.PanelID + " #hfVaccineHx").val(details.VaccineHxId);
                utility.bindMyJSONByName(true, details, false, selfDocumentForm).done(function () {
                    $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #dpDocumentHxDose_AdministrationDate').datepicker('setDate', $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #dpDocumentHxDose_AdministrationDate').val());
                    $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tpDocumentHxDose_AdministrationTime').timepicker('setTime', $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tpDocumentHxDose_AdministrationTime').val());

                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_Vaccine").attr("disabled", true);
                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail #ddlDocumentHxDose_Vaccine").attr("disabled", true);
                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail #ddlRecordRefusal_Vaccine").attr("disabled", true);

                    setTimeout(function () {
                        $('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlDocumentHxDose_Route").val(details.DocumentHxDose_Route);
                    }, 500);




                    if (details.DocumentHxVoidDose == "0") {
                        Clinical_ImmunizationDetail.previousVoid = false;
                    }
                    else if (details.DocumentHxVoidDose == "1") {
                        Clinical_ImmunizationDetail.previousVoid = true;
                    }
                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_Vaccine").val(details.DocumentHxDose_Vaccine);
                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail #ddlDocumentHxDose_Vaccine").val(details.DocumentHxDose_Vaccine);
                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail #ddlRecordRefusal_Vaccine").val(details.DocumentHxDose_Vaccine);

                });
            }
            else if (Clinical_ImmunizationDetail.params.Type.trim() == "REFUSAL") {

                var details = JSON.parse(response.RefusalVaccineLoad_JSON)[0];
                $('#' + Clinical_ImmunizationDetail.params.PanelID + " #hfVaccineHx").val(details.VaccineHxId);
                utility.bindMyJSONByName(true, details, false, selfRefusalForm).done(function () {

                    $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #dpRecordRefusalVaccine_ExpiryDate').datepicker('setDate', $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #dpRecordRefusalVaccine_ExpiryDate').val());

                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_Vaccine").attr("disabled", true);
                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail #ddlDocumentHxDose_Vaccine").attr("disabled", true);
                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail #ddlRecordRefusal_Vaccine").attr("disabled", true);


                    if (details.RecordRefusalVoidDose == "0") {
                        Clinical_ImmunizationDetail.previousVoid = false;
                    }
                    else if (details.RecordRefusalVoidDose == "1") {
                        Clinical_ImmunizationDetail.previousVoid = true;
                    }

                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_Vaccine").val(details.RecordRefusal_Vaccine);
                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail #ddlDocumentHxDose_Vaccine").val(details.RecordRefusal_Vaccine);
                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail #ddlRecordRefusal_Vaccine").val(details.RecordRefusal_Vaccine);
                });
            }
        }
    },

    get_AdministerVaccine_ForSOAP: function (AdministerVaccineId, TherapeuticInjectionIds, PatientId) {

        var objData = new Object();
        objData["VaccineHxIds"] = AdministerVaccineId;
        objData["ImmTherInjectionIds"] = TherapeuticInjectionIds;
        objData["commandType"] = "Load_Vaccine";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");
    },

    OpenLotNumber: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["UserId"] = globalAppdata['AppUserId'];
        if (Clinical_ImmunizationDetail.params.TabId == "Batch") {
            objData["PatientId"] = Clinical_ImmunizationDetail.params.patientID;
        }
        else {
            params["PatientId"] = $('#PatientProfile #hfPatientId').val();
        }
        params["ParentCtrl"] = 'Clinical_ImmunizationDetail';

        if (Clinical_ImmunizationDetail.getACtiveTabLidID() == "listAdministerVaccine") {
            params["VaccineId"] = $('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_Vaccine").val();
            params["VaccineText"] = $('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_Vaccine option:selected").text();

        }
        else if (Clinical_ImmunizationDetail.getACtiveTabLidID() == "listDocumentHxDose") {
            params["VaccineId"] = $('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlDocumentHxDose_Vaccine").val();
            params["VaccineText"] = $('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlDocumentHxDose_Vaccine option:selected").text();

        }
        else if (Clinical_ImmunizationDetail.getACtiveTabLidID() == "listRecordRefusal") {
            params["VaccineId"] = $('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlRecordRefusal_Vaccine").val();
            params["VaccineText"] = $('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlRecordRefusal_Vaccine option:selected").text();
        }
        if ($('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_Provider").val() != "") {
            params["ProviderId"] = $('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_Provider").val();
            LoadActionPan('Immunization_LotNumber', params);
        }
        else {
            utility.DisplayMessages("Select Provider", 2);
        }
    },

    GetAgeOfPatient: function () {
        var dfd = $.Deferred();

        Patient_Demographic.FillAge($("#lblPatientData span").find('.addressRow').text().split(',')[0].replace("DOB:", "").trim()).done(function (response) {
            if (response.status != false) {
                dfd.response = response.ActualAge;
                dfd.resolve();
            } else {
                dfd.resolve();
                utility.DisplayMessages(response.Message, 3);
            }
        });
        return dfd;
    },

    PopulateLotNumberFromLotNumberList: function (Vaccineid, VaccineLotId) {
        var dfd = $.Deferred();
        $.when(Clinical_ImmunizationDetail.PopulateLotNumber(Vaccineid)).then(function () {
            if (VaccineLotId != "") {
                if ($('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_LotNumber option[value='" + VaccineLotId + "']").length > 0) {
                    $(" #pnlClinicalImmunizationDetail #" + Clinical_ImmunizationDetail.ddlAdministerVaccine_LotNumber).val(VaccineLotId);
                    $(' #pnlClinicalImmunizationDetail #' + Clinical_ImmunizationDetail.ddlAdministerVaccine_LotNumber + ' option[value=' + VaccineLotId + ']').prop("selected", "selected");//
                    $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail").bootstrapValidator('revalidateField', 'AdministerVaccine_LotNumber');
                    dfd.resolve();
                }
                else {
                    dfd.resolve();
                }
            }
            else {
                dfd.resolve();
            }
        });
        return dfd;
    },

    LotNumberChange: function (LotId) {
        var dfd = $.Deferred();
        if (LotId != "") {


            Clinical_ImmunizationDetail.get_LotDataForAutoPopulate(LotId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.LotNumberCount > 0) {
                        var LotNumberLoad_JSON = JSON.parse(response.LotNumberLoad_JSON)[0];
                        Clinical_ImmunizationDetail.SetExpiryDateAndRoute(LotNumberLoad_JSON.ExpiryDate, LotNumberLoad_JSON.RouteId);
                        $.when(Clinical_ImmunizationDetail.SetLotManufanucture($("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_LotNumber").val())).then(function () {
                            dfd.resolve();
                        });
                    }
                    else {
                        if ($('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_LotNumber").length == 1 && $('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_LotNumber").val() != "") {

                        }
                        else {
                            $("#" + Clinical_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_Route").val("");
                            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #dpAdministerVaccine_ExpiryDate').val("");
                        }
                        $.when(Clinical_ImmunizationDetail.SetLotManufanucture($("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_LotNumber").val())).then(function () {
                            dfd.resolve();
                        });
                    }
                }
                else {
                    dfd.resolve();
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else {
            $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_LotNumber").val("");
            $("#" + Clinical_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_Route").val("");
            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #dpAdministerVaccine_ExpiryDate').val("");
            dfd.resolve();
        }
        return dfd
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

    RefreshLotDetail: function () {
        var dfd = $.Deferred();
        var oldLotId = $('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_LotNumber").val();
        //Clinical_ImmunizationDetail.PopulateLotNumber($('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_Vaccine").val());
        $.when(Clinical_ImmunizationDetail.PopulateLotNumberFromLotNumberList($('#' + Clinical_ImmunizationDetail.params.PanelID + " #ddlAdministerVaccine_Vaccine").val(), oldLotId)).then(function () {
            if (oldLotId != "") {
                $.when(Clinical_ImmunizationDetail.LotNumberChange(oldLotId)).then(function () {
                    dfd.resolve();
                });
            }
            else {
                dfd.resolve();
            }
        });

        return dfd;
    },

    setPatientProvider: function () {
        var dfd = $.Deferred();
        if (Clinical_ImmunizationDetail.params.from == "clinicalTabProgressNote" || Clinical_ImmunizationDetail.params.from == 'Clinical_Treatment') {
            Clinical_ImmunizationDetail.getPatientProvider().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var found = false;
                    $.each($("#frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_Provider option"), function (i, item) {
                        if ($(item).attr("value") == response.ProviderId) {
                            found = true;
                        }
                    });

                    if (found) {
                        if (Clinical_ImmunizationDetail.params.TabId == "HistoryDose") {
                            $("#frmVaccineHxDocumentHxDoseTabDetail #ddlDocumentHxDose_Provider").val(response.ProviderId);
                        }
                        else {
                            $("#frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_Provider").val(response.ProviderId);
                            $("#frmVaccineRecordRefusalTabDetail #ddlRecordRefusal_Provider").val(response.ProviderId);
                        }
                    }
                    else {
                        $("#frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_Provider").val("");
                        $("#frmVaccineRecordRefusalTabDetail #ddlRecordRefusal_Provider").val("");
                        $("#frmVaccineHxDocumentHxDoseTabDetail #ddlDocumentHxDose_Provider").val("");
                    }

                    dfd.resolve();
                }
                else {
                    dfd.resolve();
                    utility.DisplayMessages(response.message, 3);
                }
            });
        }
        else {
            if (globalAppdata.DefaultProviderId != "" && globalAppdata.DefaultProviderId != null) {
                if (Clinical_ImmunizationDetail.params.TabId == "HistoryDose") {
                    $("#frmVaccineHxDocumentHxDoseTabDetail #ddlDocumentHxDose_Provider").val(globalAppdata.DefaultProviderId);
                }
                else {
                    $("#frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_Provider").val(globalAppdata.DefaultProviderId);
                    $("#frmVaccineRecordRefusalTabDetail #ddlRecordRefusal_Provider").val(globalAppdata.DefaultProviderId);
                    $("#frmVaccineHxDocumentHxDoseTabDetail #ddlDocumentHxDose_Provider").val(globalAppdata.DefaultProviderId);
                }
            }
            else {
                $("#frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_Provider").val("");
                $("#frmVaccineRecordRefusalTabDetail #ddlRecordRefusal_Provider").val("");
                $("#frmVaccineHxDocumentHxDoseTabDetail #ddlDocumentHxDose_Provider").val("");
            }
            dfd.resolve();
        }

        return dfd;
    },
    LoadFavVaccine: function (ComeFormLoadFuntion) {
        $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #ulFavVaccine li').remove();
        var Tab = Clinical_ImmunizationDetail.AdministerText;
        var CtrlId = "#frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_Provider";
        if (Clinical_ImmunizationDetail.getACtiveTabLidID() == "listAdministerVaccine") {
            Tab = Clinical_ImmunizationDetail.AdministerText;
            CtrlId = "#frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_Provider";
        }
        else if (Clinical_ImmunizationDetail.getACtiveTabLidID() == "listDocumentHxDose") {
            Tab = Clinical_ImmunizationDetail.DocumentHxDoseText;
            CtrlId = "#frmVaccineHxDocumentHxDoseTabDetail #ddlDocumentHxDose_Provider";
        }
        else if (Clinical_ImmunizationDetail.getACtiveTabLidID() == "listRecordRefusal") {
            Tab = Clinical_ImmunizationDetail.REFUSALText;
            CtrlId = "#frmVaccineRecordRefusalTabDetail #ddlRecordRefusal_Provider";
        }


        var dfd = $.Deferred();
        if (Clinical_ImmunizationDetail.params.TabId == "HistoryDose") {
            var cateValue = $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail #ddlDocumentHxDose_Category").val();
            if (cateValue != "") {
                Clinical_ImmunizationDetail.params.Category = cateValue;
            }
        }
        if (Clinical_ImmunizationDetail.params.Category) {
            var self = $('#' + Clinical_ImmunizationDetail.params.PanelID);
            self.find('.Favorites > select').attr('ddlist', 'GetFavVaccine');
            var data = "IsActive=&StrID=" + $(CtrlId).val() + "&StrID2=" + Tab + "&StrID3=vaccine&ID=" + Clinical_ImmunizationDetail.params.Category;
            self.find('.Favorites').loadDropDowns(true, data).done(function () {
                if (typeof ComeFormLoadFuntion != typeof undefined && ComeFormLoadFuntion != null && ComeFormLoadFuntion == true) {
                    Clinical_ImmunizationDetail.SetFavListVal($("#" + Clinical_ImmunizationDetail.params.PanelID + " #ddlFavorites"));
                }

                if (Clinical_ImmunizationDetail.getACtiveTabLidID() == "listAdministerVaccine") {
                    $.when(Clinical_ImmunizationDetail.PopulateLotNumber($("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_Vaccine").val(), ComeFormLoadFuntion)).then(function () {
                        if (typeof ComeFormLoadFuntion != typeof undefined && ComeFormLoadFuntion != null && ComeFormLoadFuntion == true) {
                            dfd.resolve();
                        }
                        else {
                            $.when(Clinical_ImmunizationDetail.SetLotManufanucture($("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_LotNumber").val())).then(function () {
                                dfd.resolve();
                            });
                        }
                    });
                }
                else {
                    dfd.resolve();
                }
            });
        }
        else {
            dfd.resolve();
        }
        return dfd;
    },
    getPatientProvider: function () {
        var objData = new Object();
        objData["NotesId"] = $("#pnlClinicalProgressNote #hfNoteId").val();
        objData["commandType"] = "GET_PROVIDER_ID";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");
    },

    CheckPatientInsuranceIsMedicare: function () {
        var dfd = $.Deferred();
        Clinical_ImmunizationDetail.CheckPatientInsuranceIsMedicare_DB_CALL().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.IsMedicare == "1") {
                    $("#frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_VFC").attr("disabled", false);
                }
                else {
                    $("#frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_VFC").attr("disabled", true);
                }
                dfd.resolve();
            }
            else {
                dfd.resolve();
                utility.DisplayMessages(response.message, 3);
            }
        });
        return dfd;
    },

    CheckPatientInsuranceIsMedicare_DB_CALL: function () {
        var objData = new Object();
        if (Clinical_ImmunizationDetail.params.TabId == "Batch") {
            objData["PatientId"] = Clinical_ImmunizationDetail.params.patientID;
        }
        else {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        }
        objData["commandType"] = "CHECK_PATIENT_INSURANCE_IS_MEDICARE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");
    },

    //check this vaccine is last administered dose,
    IsLastAdministeredDoes: function (vaccineHxId, VaccineScheduleId, VoidDose, VaccineId) {
        var dfd = $.Deferred();
        Clinical_ImmunizationDetail.IsLastAdministeredDoes_DB_CALL(vaccineHxId, VaccineScheduleId, VoidDose, VaccineId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.IsLastAdministeredDoes == "1") {
                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #chkAdministerVoidDose").attr("disabled", false);
                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail #chkDocumentHxVoidDose").attr("disabled", false);
                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail #chkRecordRefusalVoidDose").attr("disabled", false);
                }
                else {
                    if (response.IsLastAdministeredDoes == "-1") {
                        utility.DisplayMessages("The dose you are trying to void is a part of common vaccinations which is not the latest dose. Please first void the latest dose and then try again to void this common vaccination.", 2);
                    }
                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #chkAdministerVoidDose").attr("disabled", true);
                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail #chkDocumentHxVoidDose").attr("disabled", true);
                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail #chkRecordRefusalVoidDose").attr("disabled", true);

                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #chkAdministerVoidDose").prop("checked", false);
                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail #chkDocumentHxVoidDose").prop("checked", false);
                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail #chkRecordRefusalVoidDose").prop("checked", false);
                }
                dfd.resolve();
            }
            else {
                dfd.resolve();
                utility.DisplayMessages(response.message, 3);
            }
        });
        return dfd;
    },

    //check this vaccine is last administered dose DB Call
    IsLastAdministeredDoes_DB_CALL: function (vaccineHxId, VaccineScheduleId, VoidDose, VaccineId) {
        var objData = new Object();
        objData["VaccineHxId"] = vaccineHxId;
        objData["VoidDose"] = VoidDose;
        objData["VaccineScheduleId"] = VaccineScheduleId;
        objData["VaccineID"] = VaccineId;
        if (Clinical_ImmunizationDetail.params.TabId == "Batch") {
            objData["PatientId"] = Clinical_ImmunizationDetail.params.patientID;
        }
        else {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        }
        objData["commandType"] = "Is_Last_Administered_Does";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");
    },

    ChangeVoidDoes: function (obj, VaccineId) {
        if ($(obj).prop("checked") == true) {
            utility.myConfirm('39', function () {
                if (Clinical_ImmunizationDetail.params["VaccineScheduleId"] != 0 && Clinical_ImmunizationDetail.params["VaccineScheduleId"] != "" && Clinical_ImmunizationDetail.params["VaccineScheduleId"] != null) {
                    Clinical_ImmunizationDetail.IsLastAdministeredDoes(Clinical_ImmunizationDetail.params.VaccineHxId, Clinical_ImmunizationDetail.params["VaccineScheduleId"], $(obj).prop("checked"), $('#' + Clinical_ImmunizationDetail.params.PanelID + " #" + VaccineId).val());

                }

            }, function () {
                $(obj).attr('checked', false)

            },
                    '1'
                );
        }

    },

    //InsertVaccineInProcedure: function (VaccineId, VaccineHxId) {
    //    $.when(result = Clinical_ImmunizationDetail.GetCptOfVaccine(VaccineId)).then(function () {
    //        if (result.response != null && result.response != "") {
    //            Clinical_ImmunizationDetail.GetVaccineHxIds(VaccineHxId).done(function (HxResponse) {
    //                if (result.response != null && result.response != "") {
    //                    var CptList = result.response.split(',');
    //                    var ProceduresDetail = [];
    //                    $(CptList).each(function (jj, cpt) {

    //                        var objDetail = {};
    //                        objDetail["CPTId"] = cpt;
    //                        objDetail["VaccineHxId"] = HxResponse;
    //                        if (typeof GetSelectedPatientID() === "undefined") {
    //                            objDetail["PatientId"] = Clinical_ImmunizationDetail.params.patientID;
    //                        }
    //                        else {
    //                            objDetail["PatientId"] = GetSelectedPatientID();//163; //
    //                        }

    //                        if (Clinical_ImmunizationDetail.params.PanelID.indexOf("pnlClinicalProgressNote") > 0) {
    //                            objDetail["NotesId"] = $("#pnlClinicalProgressNote #hfNotesId").val();
    //                        }
    //                        else {
    //                            objDetail["NotesId"] = -1;
    //                        }
    //                        objDetail["Modifier"] = "";
    //                        objDetail["Unit"] = 1;
    //                        ProceduresDetail.push(objDetail);
    //                    });

    //                    if (ProceduresDetail.length > 0) {
    //                        Clinical_ImmunizationDetail.SaveProceduresForVaccine(ProceduresDetail).done(function (response) {
    //                            response = JSON.parse(response);
    //                            if (response.status != false) {
    //                                //if (response.ProcedureCount > 0) {
    //                                //    if (Clinical_ImmunizationDetail.params.from == "clinicalTabProgressNote") {
    //                                //        var ProceduresLoadJSONData = JSON.parse(response.ProcedureLoad_JSON);
    //                                //        $.each(ProceduresLoadJSONData, function (i, item) {
    //                                //            //$('#' + Clinical_ImmunizationDetail.params.PanelID + ' #ddlAdministerVaccine_VisitDate')
    //                                //            Clinical_Procedures.getProceduresInfo(item.ProcedureId, true);
    //                                //        });
    //                                //    }
    //                                //}
    //                            }
    //                            else {
    //                                utility.DisplayMessages(response.message, 3);
    //                            }
    //                        });
    //                    }
    //                }
    //            });



    //        }
    //    });
    //},

    SaveProceduresForVaccine: function (ProceduresDetail) {
        var objData = {};
        objData["procedureDetailModel"] = ProceduresDetail;
        objData["commandType"] = "save_procedures";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Procedure");
    },

    GetCptDetail_DB_CALL: function (cptCode) {
        var data = "text=" + cptCode + "&entityID=" + globalAppdata["SeletedEntityId"] + "&iscode=CPT" + "&isMDVision=true";
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "COMMON_IMO_CODE", "GET_IMO_CPTCODE");
    },

    GetCptOfVaccine: function (VaccineId) {

        var dfd = $.Deferred();
        Clinical_ImmunizationDetail.GetCptOfVaccine_DB_CALL(VaccineId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                dfd.response = response.Cpt;
                dfd.resolve();
            }
            else {
                dfd.resolve();
                utility.DisplayMessages(response.message, 3);
            }
        });
        return dfd;
    },

    GetCptOfVaccine_DB_CALL: function (VaccineId) {
        var objData = new Object();
        objData["VaccineId"] = VaccineId;
        objData["commandType"] = "Get_Cpt_Of_Vaccine";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");
    },

    GetVaccineHxIds: function (VaccineHxId) {

        var dfd = $.Deferred();
        Clinical_ImmunizationDetail.GetVaccineHxIds_DB_CALL(VaccineHxId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                dfd.resolve(response.VaccineHxIds);
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

    GetVaccineHxIds_DB_CALL: function (VaccineHxId) {
        var objData = new Object();
        objData["VaccineHxId"] = VaccineHxId;
        objData["commandType"] = "Get_VaccineHxIds";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");
    },

    //UpdateVaccineInProcedure: function (VoidDoes, VaccineHxId) {
    //    var dfd = $.Deferred();
    //    if (Clinical_ImmunizationDetail.previousVoid != VoidDoes) {
    //        Clinical_ImmunizationDetail.GetVaccineHxIds(VaccineHxId).done(function (resultVaccineHxIds) {
    //            if (resultVaccineHxIds) {
    //                var VaccineHxIdList = resultVaccineHxIds.split(',');

    //                var DeleteProceduresDetail = [];
    //                var DeleteProceduresIds = "";
    //                $(VaccineHxIdList).each(function (jj, VaccineHxId) {
    //                    if (Clinical_ImmunizationDetail.previousVoid == false && VoidDoes == true) {
    //                        if (jj == 0) {
    //                            DeleteProceduresIds += VaccineHxId;
    //                        }
    //                        else {
    //                            DeleteProceduresIds += "," + VaccineHxId;
    //                        }
    //                    }
    //                });

    //                if (DeleteProceduresIds != "") {
    //                    Clinical_ImmunizationDetail.DeleteProcedure(DeleteProceduresIds).done(function (response) {
    //                        response = JSON.parse(response);
    //                        if (response.status != false) {
    //                            dfd.response = 1;
    //                            dfd.resolve();

    //                            //if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Procedures_Main' + selectedValue).length != 0) {
    //                            //    Clinical_Procedures.detachProceduresFromNotes_DBCall(selectedValue).done(function (response) {
    //                            //        response = JSON.parse(response);
    //                            //        if (response.status != false) {

    //                            //            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Procedures_Main' + selectedValue).remove();
    //                            //            Clinical_ProgressNote.saveComponentSOAPText();
    //                            //            setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
    //                            //        }
    //                            //        else {
    //                            //            utility.DisplayMessages(response.Message, 3);
    //                            //        }
    //                            //    });
    //                            //}
    //                        }
    //                        else {
    //                            dfd.response = 0;
    //                            dfd.resolve();
    //                            utility.DisplayMessages("Record Associated with Notes so it Cannot be Void the dose", 3);
    //                        }
    //                    });
    //                }
    //                else {
    //                    dfd.response = 1;
    //                    dfd.resolve();
    //                }

    //            }
    //            else {
    //                dfd.response = 1;
    //                dfd.resolve();
    //            }
    //        });
    //    }
    //    else {
    //        dfd.response = 1;
    //        dfd.resolve();
    //    }
    //    return dfd;
    //},

    DeleteProcedure: function (VaccineHxIds) {

        var objData = new Object();
        objData["VaccineHxId"] = VaccineHxIds;
        objData["commandType"] = "DELETE_Procedure";
        objData["ProcedureId"] = VaccineHxIds//for a time

        if (typeof GetSelectedPatientID() === "undefined") {
            objData["PatientId"] = Clinical_ImmunizationDetail.params.patientID;
        }
        else {
            objData["PatientId"] = GetSelectedPatientID();//163; //
        }
        if (Clinical_ImmunizationDetail.params.TabId == "Batch") {
            objData["PatientId"] = Clinical_ImmunizationDetail.params.patientID;
        }


        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Procedure");

    },

    MakeSoaptextOfProcedures: function (VaccineHxIds, ImmTheraInjecIds) {
        var dfd = $.Deferred();
        Clinical_ImmunizationDetail.GetProcedureIdsAgainstVaccAndImm(VaccineHxIds, ImmTheraInjecIds).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                dfd.response = response.ProcedureIds;
                dfd.resolve();
            }
            else {
                dfd.response = 0;
                dfd.resolve();
                utility.DisplayMessages(response.message, 3);
            }
        });
        return dfd;
    },

    GetProcedureIdsAgainstVaccAndImm: function (VaccineHxIds, ImmTheraInjecIds) {
        var objData = new Object();
        objData["VaccineHxIds"] = VaccineHxIds;
        objData["ImmTherInjectionIds"] = ImmTheraInjecIds;
        objData["commandType"] = "Get_ProcedureIds_Against_VaccAndImm";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");

    },

    AutoSearchFavVaccine: function () {
        utility.Keyupdelay(function () {
            Clinical_ImmunizationDetail.FavVaccineChange(null);
        });
    },
    FavVaccineChange: function (obj) {

        if (typeof obj == typeof undefined || obj == null) {
            obj = $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #ddlFavorites');
        }
        $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #ulFavVaccine li').remove();
        if ($(obj).val() != "") {
            Clinical_ImmunizationDetail.LoadFavImmunization_Vaccine_DBCALL($(obj).val(), $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #FavSearchBox').val()).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.FavImmunizationCount > 0) {
                        $.each(response.FavImmunization_JSON, function (i, item) {
                            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #ulFavVaccine').append('<li id=' + item.VaccineID + ' onclick="Clinical_ImmunizationDetail.SelectFavVaccine(this)">' + item.VaccineName + '</li>');
                        });
                    }
                }
            });
        }
    },
    SelectFavVaccine: function (obj) {
        var tab = Clinical_ImmunizationDetail.AdministerText;
        var controlId = "#ddlAdministerVaccine_Vaccine";
        if (Clinical_ImmunizationDetail.getACtiveTabLidID() == "listAdministerVaccine") {
            controlId = "#ddlAdministerVaccine_Vaccine";
            tab = Clinical_ImmunizationDetail.AdministerText;
        }
        else if (Clinical_ImmunizationDetail.getACtiveTabLidID() == "listDocumentHxDose") {
            controlId = "#ddlDocumentHxDose_Vaccine";
            tab = Clinical_ImmunizationDetail.DocumentHxDoseText;
        }
        else if (Clinical_ImmunizationDetail.getACtiveTabLidID() == "listRecordRefusal") {
            controlId = "#ddlRecordRefusal_Vaccine";
            tab = "RecordRefusal";
        }
        $("#" + Clinical_ImmunizationDetail.params.PanelID + " " + controlId + ' option').filter(function () {
            return $.trim($(this).text().toLowerCase().trim()) == $(obj).text().toLowerCase().trim();
        }).prop('selected', true);
        Clinical_ImmunizationDetail.PopulateVISDate_VISURL_and_ManufacturerDropDown($("#" + Clinical_ImmunizationDetail.params.PanelID + " " + controlId).val(), tab);
    },
    LoadFavImmunization_Vaccine_DBCALL: function (FavId, SearchData) {
        var Tab = Clinical_ImmunizationDetail.AdministerText;

        if (Clinical_ImmunizationDetail.getACtiveTabLidID() == "listAdministerVaccine") {
            Tab = Clinical_ImmunizationDetail.AdministerText;
        }
        else if (Clinical_ImmunizationDetail.getACtiveTabLidID() == "listDocumentHxDose") {
            Tab = Clinical_ImmunizationDetail.DocumentHxDoseText;
        }
        else if (Clinical_ImmunizationDetail.getACtiveTabLidID() == "listRecordRefusal") {
            Tab = Clinical_ImmunizationDetail.REFUSALText;
        }


        var objData = new Object();
        objData["commandType"] = "Load_Fav_Immunization_Vaccine_Detail";
        objData["FavoritiesListId"] = FavId;
        objData["SearchData"] = SearchData;
        objData["Tab"] = Tab;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "Immunization");
    },
    ChangeTab: function (Obj) {
        if ($(Obj).attr("id") == "listAdministerVaccine") {
            $('#' + Clinical_ImmunizationDetail.params.PanelID).find('[id*="listAdministerVaccine"]').addClass('active');
            $('#' + Clinical_ImmunizationDetail.params.PanelID).find('[id*="tabAdministerVaccine"]').addClass('active');
            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #listRecordRefusal').removeClass('active');
            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tabRecordRefusal').removeClass('active');
            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #listDocumentHxDose').removeClass('active');
            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tabDocumentHxDose').removeClass('active');
        }
        else if ($(Obj).attr("id") == "listDocumentHxDose") {
            $('#' + Clinical_ImmunizationDetail.params.PanelID + " #listAdministerVaccine").removeClass('active');
            $('#' + Clinical_ImmunizationDetail.params.PanelID + " #tabAdministerVaccine").removeClass('active');
            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #listRecordRefusal').removeClass('active');
            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tabRecordRefusal').removeClass('active');
            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #listDocumentHxDose').addClass('active');
            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tabDocumentHxDose').addClass('active');
        }
        else if ($(Obj).attr("id") == "listRecordRefusal") {
            $('#' + Clinical_ImmunizationDetail.params.PanelID + " #listAdministerVaccine").removeClass('active');
            $('#' + Clinical_ImmunizationDetail.params.PanelID + " #tabAdministerVaccine").removeClass('active');
            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #listRecordRefusal').addClass('active');
            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tabRecordRefusal').addClass('active');
            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #listDocumentHxDose').removeClass('active');
            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tabDocumentHxDose').removeClass('active');
        }
        Clinical_ImmunizationDetail.FavVaccineChange($('#' + Clinical_ImmunizationDetail.params.PanelID + ' #ddlFavorites'));
    },
    WhyLotIsNotPopulateInDropDown: function (VaccineId, ProviderId, Type) {
        var dfd = $.Deferred();
        if (VaccineId != "" && VaccineId > 0, ProviderId != "" && ProviderId > 0) {

            Clinical_ImmunizationDetail.WhyLotIsNotPopulateInDropDown_CALL(VaccineId, ProviderId, Type).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    dfd.response = response.WhyLotIsNotAvailable;


                    if (response.WhyLotIsNotAvailable == "1") {
                        utility.DisplayMessages(Type + " lot is expired!", 2);
                    }
                    else if (response.WhyLotIsNotAvailable == "2") {
                        utility.DisplayMessages(Type + " Lot is out-of-stock!", 2);
                    }
                    else if (response.WhyLotIsNotAvailable == "3") {
                        utility.DisplayMessages("Either " + Type + " lot is expired or out-of-stock!", 2);
                    }
                    dfd.resolve();
                }
                else {
                    dfd.resolve();
                    utility.DisplayMessages(response.message, 3);
                }
            });

        }
        else {
            dfd.resolve();
        }
        return dfd;
    },

    WhyLotIsNotPopulateInDropDown_CALL: function (VaccineId, ProviderId, Type) {
        var objData = new Object();
        objData["Type"] = Type;
        objData["VaccineID"] = VaccineId;
        objData["ProviderId"] = ProviderId;
        objData["commandType"] = "Why_Lot_Is_Not_Available";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");
    },
    BindAutocomplete: function () {
        var VaccineId = $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #frmVaccineHxAdministerTabDetail #ddlAdministerVaccine_Vaccine').val();
        var ManufacturerName = $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #frmVaccineHxAdministerTabDetail #txtAdministerTabManufacturer').val();
        if (VaccineId > 0) {
            utility.Keyupdelay(function () {
                Clinical_ImmunizationDetail.GetManufacturerArray(VaccineId, ManufacturerName).done(function (response) {
                    $("#frmVaccineHxAdministerTabDetail #txtAdministerTabManufacturer").autocomplete({
                        autoFocus: true,
                        source: response,
                        select: function (event, ui) {
                            setTimeout(function () {
                                $("#frmVaccineHxAdministerTabDetail #hfAdministerTabManufacturer").val(ui.item.id);
                                $("#frmVaccineHxAdministerTabDetail #txAdministerTabtManufacturer").val(ui.item.value);
                            }, 100);
                        }
                    });
                    $("#frmVaccineHxAdministerTabDetail #txtAdministerTabManufacturer").autocomplete("search");
                });
            });
        }
        else {
            utility.DisplayMessages("Select Vaccine", 2);
        }
    },
    GetManufacturerArray: function (VaccineId, ManufacturerName) {
        var AllManufacturer = [];
        var IsValid = false;

        if (ManufacturerName != null && ManufacturerName.length > 2) {
            IsValid = true;
        }

        else {
            IsValid = false;
        }
        var dfd = new $.Deferred();
        if (IsValid) {
            // serach parameter , class name, command name of class
            Clinical_ImmunizationDetail.GetManufacturerArray_DBCALL(VaccineId, ManufacturerName).done(function (responseData) {
                if (responseData.status != false) {
                    responseData = JSON.parse(responseData)
                    if (responseData.ManufacturerCount > 0) {
                        var Manufacturer_JSON = responseData.Manufacturer_JSON;
                        $.each(Manufacturer_JSON, function (i, item) {
                            AllManufacturer.push({ id: item.ManufacturerId, value: item.ManufacturerName });
                        });
                    }
                }
                dfd.resolve(AllManufacturer);
            });
        }
        else {
            dfd.resolve(AllManufacturer);
        }

        return dfd.promise();

    },
    GetManufacturerArray_DBCALL: function (VaccineId, ManufacturerName) {

        var objData = new Object();
        objData["VaccineId"] = VaccineId;
        objData["ManufacturerName"] = ManufacturerName;
        objData["commandType"] = "Get_Manufacturer_Array";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Manufacturer");
    },

    //#region Vaccine Hx Transaction

    VaccineInsertUpdate: function (forModule) {
        var PreFavVal = $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #ddlFavorites').val();
        Clinical_ImmunizationDetail.FavListStatusNames(PreFavVal);
        Clinical_ImmunizationDetail.FavListVal = PreFavVal;
        if (forModule === Clinical_ImmunizationDetail.AdministerText) {

            $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail").bootstrapValidator('revalidateField', 'AdministerVaccine_Category');
            $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail").bootstrapValidator('revalidateField', 'AdministerVaccine_Provider');
            $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail").bootstrapValidator('revalidateField', 'AdministerVaccine_Vaccine');
            $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail").bootstrapValidator('revalidateField', 'AdministerVaccine_AdministrationDate');
            $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail").bootstrapValidator('revalidateField', 'AdministerVaccine_Dose');
            $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail").bootstrapValidator('revalidateField', 'AdministerVaccine_LotNumber');
            var formValidation = $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #frmVaccineHxAdministerTabDetail').data("bootstrapValidator");
            var Control = $("#" + Clinical_ImmunizationDetail.params["PanelID"] + ' #frmVaccineHxAdministerTabDetail #chkAdministerVoidDose').prop("checked");
            if (Control) {
                formValidation.enableFieldValidators('AdministerVaccine_Comments', true);
                $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail").bootstrapValidator('revalidateField', 'AdministerVaccine_Comments');
            }
            else {
                formValidation.enableFieldValidators('AdministerVaccine_Comments', false);
            }

            if (Clinical_ImmunizationDetail.params.mode == "Edit") {
                Clinical_ImmunizationDetail.DeleteProcedureIds = "";
                $.when(UpdateProcedureData = Clinical_ImmunizationDetail.UpdateVaccineInProcedureVoidDoes($("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #chkAdministerVoidDose").prop("checked"), Clinical_ImmunizationDetail.params.VaccineHxId)).then(function () {

                    if (UpdateProcedureData.response == 1) {
                        $.when(result = Clinical_ImmunizationDetail.GetAgeOfPatient()).then(function () {
                            Clinical_ImmunizationDetail.InsertUpdateAdministerVaccine(result.response).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {


                                    $.when(Immunization_AlertConfiguration.OnlySetImmunizationAlertCount(Clinical_ImmunizationDetail.params.PATIENTID)).then(function () {
                                        if (Clinical_ImmunizationDetail.params.TabId == "Alert") {
                                            if (Clinical_ImmunizationDetail.params.from == "Immunization_AlertPreview") {
                                                $.when(Immunization_AlertPreview.SearchAlerts()).then(function () {
                                                    Clinical_ImmunizationDetail.UnLoad();
                                                });
                                            }
                                        }
                                        else if (Clinical_ImmunizationDetail.params.TabId == "Chart") {
                                            if (Clinical_ImmunizationDetail.params.from == "Clinical_SchedulerView") {
                                                $.when(Clinical_SchedulerView.SchedulerPreview()).then(function () {
                                                    Clinical_ImmunizationDetail.UnLoad();
                                                });
                                            }
                                        }
                                        else if (Clinical_ImmunizationDetail.params.TabId == "Batch") {
                                            if (Clinical_ImmunizationDetail.params.from == "Batch_ImportHL7ImmunizationBatch") {
                                                $.when(Batch_ImportHL7ImmunizationBatch.QueueSearch()).then(function () {
                                                    Clinical_ImmunizationDetail.UnLoad();
                                                });
                                            }
                                        }
                                        else if (Clinical_ImmunizationDetail.params.TabId != "Hx") {
                                            if (Clinical_ImmunizationDetail.params.from == "clinicalTabProgressNote") {
                                                Clinical_ImmunizationDetail.GetVaccineHxIds(response.VaccineHxIdColumn).done(function (resultVaccineHxIds) {
                                                    $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(resultVaccineHxIds, "Administer", Clinical_ImmunizationDetail.params.patientID, false, false)).then(function () {
                                                        $.when(Clinical_Immunization.LoadSchedlerData(Clinical_ImmunizationDetail.params.TabId)).then(function () {

                                                            Clinical_ImmunizationDetail.UnLoad();
                                                        });
                                                    });
                                                });
                                            }
                                            else {
                                                utility.DisplayMessages(response.message, 1);
                                                if (Clinical_ImmunizationDetail.params.from == 'Clinical_Treatment') {
                                                    Clinical_Treatment.ImmunizationSearch('', 1, 15);
                                                    Clinical_ImmunizationDetail.UnLoad();
                                                }
                                                else {
                                                    $.when(Clinical_Immunization.LoadSchedlerData(Clinical_ImmunizationDetail.params.TabId)).then(function () {
                                                        Clinical_ImmunizationDetail.UnLoad();
                                                    });
                                                }
                                            }
                                        }
                                        else {
                                            if (Clinical_ImmunizationDetail.params.from == "clinicalTabProgressNote") {
                                                Clinical_ImmunizationDetail.GetVaccineHxIds(response.VaccineHxIdColumn).done(function (resultVaccineHxIds) {
                                                    $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(resultVaccineHxIds, "Administer", Clinical_ImmunizationDetail.params.patientID, false, false)).then(function () {
                                                        $.when(Clinical_Immunization.ImmunizationSearch('', 1, 15)).then(function () {
                                                            Clinical_ImmunizationDetail.UnLoad();
                                                        });
                                                    });
                                                });
                                            }
                                            else {
                                                utility.DisplayMessages(response.message, 1);
                                                $.when(Clinical_Immunization.ImmunizationSearch('', 1, 15)).then(function () {
                                                    Clinical_ImmunizationDetail.UnLoad();
                                                });
                                            }
                                        }
                                    });
                                }
                                else {
                                    utility.DisplayMessages(response.message, 3);
                                }
                            });
                        });
                    }
                    else {
                    }
                });
            }
            else {
                $.when(result = Clinical_ImmunizationDetail.GetAgeOfPatient()).then(function () {
                    Clinical_ImmunizationDetail.InsertUpdateAdministerVaccine(result.response).done(function (response) {

                        response = JSON.parse(response);
                        if (response.status != false) {
                            $.when(Immunization_AlertConfiguration.OnlySetImmunizationAlertCount(Clinical_ImmunizationDetail.params.PATIENTID)).then(function () {
                                if (Clinical_ImmunizationDetail.params.TabId == "Alert") {
                                    if (Clinical_ImmunizationDetail.params.from == "Immunization_AlertPreview") {
                                        $.when(Immunization_AlertPreview.SearchAlerts()).then(function () {
                                            Clinical_ImmunizationDetail.UnLoad();
                                        });
                                    }
                                }
                                else if (Clinical_ImmunizationDetail.params.TabId == "Chart") {
                                    if (Clinical_ImmunizationDetail.params.from == "Clinical_SchedulerView") {
                                        $.when(Clinical_SchedulerView.SchedulerPreview()).then(function () {
                                            Clinical_ImmunizationDetail.UnLoad();
                                        });
                                    }
                                }
                                else if (Clinical_ImmunizationDetail.params.TabId != "Hx") {
                                    if (Clinical_ImmunizationDetail.params.from == "clinicalTabProgressNote") {
                                        Clinical_ImmunizationDetail.GetVaccineHxIds(response.VaccineHxIdColumn).done(function (resultVaccineHxIds) {
                                            $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(resultVaccineHxIds, "Administer", Clinical_ImmunizationDetail.params.patientID, false, false)).then(function () {
                                                $.when(Clinical_Immunization.LoadSchedlerData(Clinical_ImmunizationDetail.params.TabId)).then(function () {
                                                    Clinical_ImmunizationDetail.UnLoad();
                                                });
                                            });
                                        });
                                    }
                                    else {
                                        utility.DisplayMessages(response.message, 1);
                                        if (Clinical_ImmunizationDetail.params.from == 'Clinical_Treatment') {
                                            Clinical_Treatment.ImmunizationSearch('', 1, 15);
                                            Clinical_ImmunizationDetail.UnLoad();
                                        }
                                        else {
                                            $.when(Clinical_Immunization.LoadSchedlerData(Clinical_ImmunizationDetail.params.TabId)).then(function () {
                                                Clinical_ImmunizationDetail.UnLoad();
                                            });
                                        }
                                    }
                                }
                                else {
                                    if (Clinical_ImmunizationDetail.params.from == "clinicalTabProgressNote") {
                                        Clinical_ImmunizationDetail.GetVaccineHxIds(response.VaccineHxIdColumn).done(function (resultVaccineHxIds) {
                                            $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(resultVaccineHxIds, "Administer", Clinical_ImmunizationDetail.params.patientID, false, false)).then(function () {
                                                $.when(Clinical_Immunization.ImmunizationSearch('', 1, 15)).then(function () {
                                                    Clinical_ImmunizationDetail.UnLoad();
                                                });
                                            });
                                        });
                                    }
                                    else {
                                        utility.DisplayMessages(response.message, 1);
                                        $.when(Clinical_Immunization.ImmunizationSearch('', 1, 15)).then(function () {
                                            Clinical_ImmunizationDetail.UnLoad();
                                        });
                                    }
                                }
                            });
                        }
                        else {
                            utility.DisplayMessages(response.message, 3);
                        }
                    });
                });
            }
        }
        else if (forModule === Clinical_ImmunizationDetail.DocumentHxDoseText) {
            $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail").bootstrapValidator('revalidateField', 'DocumentHxDose_Category');
            $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail").bootstrapValidator('revalidateField', 'DocumentHxDose_Vaccine');
            var formValidation = $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #frmVaccineHxDocumentHxDoseTabDetail').data("bootstrapValidator");
            var Control = $("#" + Clinical_ImmunizationDetail.params["PanelID"] + ' #frmVaccineHxDocumentHxDoseTabDetail #chkDocumentHxVoidDose').prop("checked");
            if (Control) {
                formValidation.enableFieldValidators('DocumentHxDose_Comments', true);
                $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail").bootstrapValidator('revalidateField', 'DocumentHxDose_Comments');
            }
            else {
                formValidation.enableFieldValidators('DocumentHxDose_Comments', false);
            }

            if (Clinical_ImmunizationDetail.params.mode == "Edit") {
                $.when(UpdateProcedureData = Clinical_ImmunizationDetail.UpdateVaccineInProcedureVoidDoes($("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail #chkDocumentHxVoidDose").prop("checked"), Clinical_ImmunizationDetail.params.VaccineHxId)).then(function () {

                    if (UpdateProcedureData.response == 1) {
                        $.when(result = Clinical_ImmunizationDetail.GetAgeOfPatient()).then(function () {
                            Clinical_ImmunizationDetail.InsertUpdateVaccineHxDose(result.response).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    if (Clinical_ImmunizationDetail.params.TabId == "HistoryDose") {
                                        if (Clinical_ImmunizationDetail.params.from == "clinicalTabProgressNote") {
                                            Clinical_ImmunizationDetail.GetVaccineHxIds(response.VaccineHxIdColumn).done(function (resultVaccineHxIds) {
                                                $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(resultVaccineHxIds, "DOCUMENTHX", Clinical_ImmunizationDetail.params.patientID, false, false)).then(function () {
                                                    $.when(Clinical_Immunization.ImmunizationSearch('', 1, 15)).then(function () {
                                                        Clinical_ImmunizationDetail.UnLoad();
                                                    });
                                                });
                                            });
                                        }
                                        else {
                                            utility.DisplayMessages(response.message, 1);
                                            $.when(Clinical_Immunization.ImmunizationSearch('', 1, 15)).then(function () {
                                                Clinical_ImmunizationDetail.UnLoad();
                                            });
                                        }
                                    }
                                    else {
                                        $.when(Immunization_AlertConfiguration.OnlySetImmunizationAlertCount(Clinical_ImmunizationDetail.params.PATIENTID)).then(function () {
                                            if (Clinical_ImmunizationDetail.params.TabId == "Alert") {
                                                if (Clinical_ImmunizationDetail.params.from == "Immunization_AlertPreview") {
                                                    $.when(Immunization_AlertPreview.SearchAlerts()).then(function () {
                                                        Clinical_ImmunizationDetail.UnLoad();
                                                    });
                                                }
                                            }
                                            else if (Clinical_ImmunizationDetail.params.TabId == "Chart") {
                                                if (Clinical_ImmunizationDetail.params.from == "Clinical_SchedulerView") {
                                                    $.when(Clinical_SchedulerView.SchedulerPreview()).then(function () {
                                                        Clinical_ImmunizationDetail.UnLoad();
                                                    });
                                                }
                                            }
                                            else if (Clinical_ImmunizationDetail.params.TabId == "Batch") {
                                                if (Clinical_ImmunizationDetail.params.from == "Batch_ImportHL7ImmunizationBatch") {
                                                    $.when(Batch_ImportHL7ImmunizationBatch.QueueSearch()).then(function () {
                                                        Clinical_ImmunizationDetail.UnLoad();
                                                    });
                                                }
                                            }
                                            else if (Clinical_ImmunizationDetail.params.TabId != "Hx") {
                                                if (Clinical_ImmunizationDetail.params.from == "clinicalTabProgressNote") {
                                                    Clinical_ImmunizationDetail.GetVaccineHxIds(response.VaccineHxIdColumn).done(function (resultVaccineHxIds) {
                                                        $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(resultVaccineHxIds, "DOCUMENTHX", Clinical_ImmunizationDetail.params.patientID, false, false)).then(function () {
                                                            $.when(Clinical_Immunization.LoadSchedlerData(Clinical_ImmunizationDetail.params.TabId)).then(function () {
                                                                Clinical_ImmunizationDetail.UnLoad();
                                                            });
                                                        });
                                                    });
                                                }
                                                else {
                                                    utility.DisplayMessages(response.message, 1);
                                                    if (Clinical_ImmunizationDetail.params.from == 'Clinical_Treatment') {
                                                        Clinical_Treatment.ImmunizationSearch('', 1, 15);
                                                        Clinical_ImmunizationDetail.UnLoad();
                                                    }
                                                    else {
                                                        $.when(Clinical_Immunization.LoadSchedlerData(Clinical_ImmunizationDetail.params.TabId)).then(function () {
                                                            Clinical_ImmunizationDetail.UnLoad();
                                                        });
                                                    }
                                                }
                                            }
                                            else {
                                                if (Clinical_ImmunizationDetail.params.from == "clinicalTabProgressNote") {
                                                    Clinical_ImmunizationDetail.GetVaccineHxIds(response.VaccineHxIdColumn).done(function (resultVaccineHxIds) {
                                                        $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(resultVaccineHxIds, "DOCUMENTHX", Clinical_ImmunizationDetail.params.patientID, false, false)).then(function () {
                                                            $.when(Clinical_Immunization.ImmunizationSearch('', 1, 15)).then(function () {
                                                                Clinical_ImmunizationDetail.UnLoad();
                                                            });
                                                        });
                                                    });
                                                }
                                                else {
                                                    utility.DisplayMessages(response.message, 1);
                                                    $.when(Clinical_Immunization.ImmunizationSearch('', 1, 15)).then(function () {
                                                        Clinical_ImmunizationDetail.UnLoad();
                                                    });
                                                }
                                            }
                                        });
                                    }

                                }
                                else {
                                    utility.DisplayMessages(response.message, 3);
                                }
                            });
                        });
                    }
                    else {

                    }
                });
            }
            else {
                $.when(result = Clinical_ImmunizationDetail.GetAgeOfPatient()).then(function () {
                    Clinical_ImmunizationDetail.InsertUpdateVaccineHxDose(result.response).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            if (Clinical_ImmunizationDetail.params.TabId == "HistoryDose") {
                                if (Clinical_ImmunizationDetail.params.from == "clinicalTabProgressNote") {
                                    Clinical_ImmunizationDetail.GetVaccineHxIds(response.VaccineHxIdColumn).done(function (resultVaccineHxIds) {
                                        $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(resultVaccineHxIds, "DOCUMENTHX", Clinical_ImmunizationDetail.params.patientID, false, false)).then(function () {
                                            $.when(Clinical_Immunization.ImmunizationSearch('', 1, 15)).then(function () {
                                                Clinical_ImmunizationDetail.UnLoad();
                                            });
                                        });
                                    });
                                }
                                else {
                                    utility.DisplayMessages(response.message, 1);
                                    $.when(Clinical_Immunization.ImmunizationSearch('', 1, 15)).then(function () {
                                        Clinical_ImmunizationDetail.UnLoad();
                                    });
                                }
                            }
                            else {
                                $.when(Immunization_AlertConfiguration.OnlySetImmunizationAlertCount(Clinical_ImmunizationDetail.params.PATIENTID)).then(function () {
                                    if (Clinical_ImmunizationDetail.params.TabId == "Alert") {
                                        if (Clinical_ImmunizationDetail.params.from == "Immunization_AlertPreview") {
                                            $.when(Immunization_AlertPreview.SearchAlerts()).then(function () {
                                                Clinical_ImmunizationDetail.UnLoad();
                                            });
                                        }
                                    }
                                    else if (Clinical_ImmunizationDetail.params.TabId == "Chart") {
                                        if (Clinical_ImmunizationDetail.params.from == "Clinical_SchedulerView") {
                                            $.when(Clinical_SchedulerView.SchedulerPreview()).then(function () {
                                                Clinical_ImmunizationDetail.UnLoad();
                                            });
                                        }
                                    }
                                    else if (Clinical_ImmunizationDetail.params.TabId != "Hx") {
                                        if (Clinical_ImmunizationDetail.params.from == "clinicalTabProgressNote") {
                                            Clinical_ImmunizationDetail.GetVaccineHxIds(response.VaccineHxIdColumn).done(function (resultVaccineHxIds) {
                                                $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(resultVaccineHxIds, "DOCUMENTHX", Clinical_ImmunizationDetail.params.patientID, false, false)).then(function () {
                                                    $.when(Clinical_Immunization.LoadSchedlerData(Clinical_ImmunizationDetail.params.TabId)).then(function () {
                                                        Clinical_ImmunizationDetail.UnLoad();
                                                    });
                                                });
                                            });
                                        }
                                        else {
                                            utility.DisplayMessages(response.message, 1);
                                            if (Clinical_ImmunizationDetail.params.from == 'Clinical_Treatment') {
                                                Clinical_Treatment.ImmunizationSearch('', 1, 15);
                                                Clinical_ImmunizationDetail.UnLoad();
                                            }
                                            else {
                                                $.when(Clinical_Immunization.LoadSchedlerData(Clinical_ImmunizationDetail.params.TabId)).then(function () {
                                                    Clinical_ImmunizationDetail.UnLoad();
                                                });
                                            }
                                        }
                                    }
                                    else {
                                        if (Clinical_ImmunizationDetail.params.from == "clinicalTabProgressNote") {
                                            Clinical_ImmunizationDetail.GetVaccineHxIds(response.VaccineHxIdColumn).done(function (resultVaccineHxIds) {
                                                $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(resultVaccineHxIds, "DOCUMENTHX", Clinical_ImmunizationDetail.params.patientID, false, false)).then(function () {
                                                    $.when(Clinical_Immunization.ImmunizationSearch('', 1, 15)).then(function () {
                                                        Clinical_ImmunizationDetail.UnLoad();
                                                    });
                                                });
                                            });
                                        }
                                        else {
                                            utility.DisplayMessages(response.message, 1);
                                            $.when(Clinical_Immunization.ImmunizationSearch('', 1, 15)).then(function () {
                                                Clinical_ImmunizationDetail.UnLoad();
                                            });
                                        }

                                    }
                                });
                            }
                        }
                        else {
                            utility.DisplayMessages(response.message, 3);
                        }
                    });
                });
            }
        }
        else if (forModule === Clinical_ImmunizationDetail.REFUSALText) {
            $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail").bootstrapValidator('revalidateField', 'RecordRefusal_Category');
            $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail").bootstrapValidator('revalidateField', 'RecordRefusal_Provider');
            $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail").bootstrapValidator('revalidateField', 'RecordRefusal_Vaccine');
            $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail").bootstrapValidator('revalidateField', 'RecordRefusalReason');

            var formValidation = $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #frmVaccineRecordRefusalTabDetail').data("bootstrapValidator");
            var Control = $("#" + Clinical_ImmunizationDetail.params["PanelID"] + ' #frmVaccineRecordRefusalTabDetail #chkRecordRefusalVoidDose').prop("checked");
            if (Control) {
                formValidation.enableFieldValidators('RecordRefusal_Comments', true);
                $("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail").bootstrapValidator('revalidateField', 'RecordRefusal_Comments');
            }
            else {
                formValidation.enableFieldValidators('RecordRefusal_Comments', false);
            }

            if (Clinical_ImmunizationDetail.params.mode == "Edit") {
                $.when(UpdateProcedureData = Clinical_ImmunizationDetail.UpdateVaccineInProcedureVoidDoes($("#" + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail #chkRecordRefusalVoidDose").prop("checked"), Clinical_ImmunizationDetail.params.VaccineHxId)).then(function () {

                    if (UpdateProcedureData.response == 1) {
                        $.when(result = Clinical_ImmunizationDetail.GetAgeOfPatient()).then(function () {
                            Clinical_ImmunizationDetail.InsertUpdateSaveVaccineRefusalRecord(result.response).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    $.when(Immunization_AlertConfiguration.OnlySetImmunizationAlertCount(Clinical_ImmunizationDetail.params.PATIENTID)).then(function () {
                                        if (Clinical_ImmunizationDetail.params.TabId == "Alert") {
                                            if (Clinical_ImmunizationDetail.params.from == "Immunization_AlertPreview") {
                                                $.when(Immunization_AlertPreview.SearchAlerts()).then(function () {
                                                    Clinical_ImmunizationDetail.UnLoad();
                                                });
                                            }
                                        }
                                        else if (Clinical_ImmunizationDetail.params.TabId == "Chart") {
                                            if (Clinical_ImmunizationDetail.params.from == "Clinical_SchedulerView") {
                                                $.when(Clinical_SchedulerView.SchedulerPreview()).then(function () {
                                                    Clinical_ImmunizationDetail.UnLoad();
                                                });
                                            }
                                        }
                                        else if (Clinical_ImmunizationDetail.params.TabId == "Batch") {
                                            if (Clinical_ImmunizationDetail.params.from == "Batch_ImportHL7ImmunizationBatch") {
                                                $.when(Batch_ImportHL7ImmunizationBatch.QueueSearch()).then(function () {
                                                    Clinical_ImmunizationDetail.UnLoad();
                                                });
                                            }
                                        }
                                        else if (Clinical_ImmunizationDetail.params.TabId != "Hx") {
                                            if (Clinical_ImmunizationDetail.params.from == "clinicalTabProgressNote") {
                                                Clinical_ImmunizationDetail.GetVaccineHxIds(response.VaccineHxIdColumn).done(function (resultVaccineHxIds) {
                                                    $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(resultVaccineHxIds, "Administer", Clinical_ImmunizationDetail.params.patientID, false, false)).then(function () {
                                                        $.when(Clinical_Immunization.LoadSchedlerData(Clinical_ImmunizationDetail.params.TabId)).then(function () {
                                                            Clinical_ImmunizationDetail.UnLoad();
                                                        });
                                                    });
                                                });
                                            }
                                            else {
                                                utility.DisplayMessages(response.message, 1);
                                                if (Clinical_ImmunizationDetail.params.from == 'Clinical_Treatment') {
                                                    Clinical_Treatment.ImmunizationSearch('', 1, 15);
                                                    Clinical_ImmunizationDetail.UnLoad();
                                                }
                                                else {
                                                    $.when(Clinical_Immunization.LoadSchedlerData(Clinical_ImmunizationDetail.params.TabId)).then(function () {
                                                        Clinical_ImmunizationDetail.UnLoad();
                                                    });
                                                }
                                            }
                                        }
                                        else {
                                            if (Clinical_ImmunizationDetail.params.from == "clinicalTabProgressNote") {
                                                Clinical_ImmunizationDetail.GetVaccineHxIds(response.VaccineHxIdColumn).done(function (resultVaccineHxIds) {
                                                    $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(resultVaccineHxIds, "REFUSAL", Clinical_ImmunizationDetail.params.patientID, false, false)).then(function () {
                                                        $.when(Clinical_Immunization.ImmunizationSearch('', 1, 15)).then(function () {
                                                            Clinical_ImmunizationDetail.UnLoad();
                                                        });
                                                    });
                                                });
                                            }
                                            else {
                                                utility.DisplayMessages(response.message, 1);
                                                $.when(Clinical_Immunization.ImmunizationSearch('', 1, 15)).then(function () {
                                                    Clinical_ImmunizationDetail.UnLoad();
                                                });
                                            }
                                        }
                                    });
                                }
                                else {
                                    utility.DisplayMessages(response.message, 3);
                                }
                            });
                        });
                    }
                    else {
                    }
                });
            }
            else {
                $.when(result = Clinical_ImmunizationDetail.GetAgeOfPatient()).then(function () {
                    Clinical_ImmunizationDetail.InsertUpdateSaveVaccineRefusalRecord(result.response).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            $.when(Immunization_AlertConfiguration.OnlySetImmunizationAlertCount(Clinical_ImmunizationDetail.params.PATIENTID)).then(function () {
                                if (Clinical_ImmunizationDetail.params.TabId == "Alert") {
                                    if (Clinical_ImmunizationDetail.params.from == "Immunization_AlertPreview") {
                                        $.when(Immunization_AlertPreview.SearchAlerts()).then(function () {
                                            Clinical_ImmunizationDetail.UnLoad();
                                        });
                                    }
                                }
                                else if (Clinical_ImmunizationDetail.params.TabId == "Chart") {
                                    if (Clinical_ImmunizationDetail.params.from == "Clinical_SchedulerView") {
                                        $.when(Clinical_SchedulerView.SchedulerPreview()).then(function () {
                                            Clinical_ImmunizationDetail.UnLoad();
                                        });
                                    }
                                }
                                else if (Clinical_ImmunizationDetail.params.TabId != "Hx") {
                                    if (Clinical_ImmunizationDetail.params.from == "clinicalTabProgressNote") {
                                        Clinical_ImmunizationDetail.GetVaccineHxIds(response.VaccineHxIdColumn).done(function (resultVaccineHxIds) {
                                            $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(resultVaccineHxIds, "Administer", Clinical_ImmunizationDetail.params.patientID, false, false)).then(function () {
                                                $.when(Clinical_Immunization.LoadSchedlerData(Clinical_ImmunizationDetail.params.TabId)).then(function () {
                                                    Clinical_ImmunizationDetail.UnLoad();
                                                });
                                            });
                                        });
                                    }
                                    else {
                                        utility.DisplayMessages(response.message, 1);
                                        if (Clinical_ImmunizationDetail.params.from == 'Clinical_Treatment') {
                                            Clinical_Treatment.ImmunizationSearch('', 1, 15);
                                            Clinical_ImmunizationDetail.UnLoad();
                                        }
                                        else {
                                            $.when(Clinical_Immunization.LoadSchedlerData(Clinical_ImmunizationDetail.params.TabId)).then(function () {
                                                Clinical_ImmunizationDetail.UnLoad();
                                            });
                                        }
                                    }
                                }
                                else {
                                    if (Clinical_ImmunizationDetail.params.from == "clinicalTabProgressNote") {
                                        Clinical_ImmunizationDetail.GetVaccineHxIds(response.VaccineHxIdColumn).done(function (resultVaccineHxIds) {
                                            $.when(Clinical_ImmunizationDetail.getAdministerVaccineInfo(resultVaccineHxIds, "REFUSAL", Clinical_ImmunizationDetail.params.patientID, false, false)).then(function () {
                                                $.when(Clinical_Immunization.ImmunizationSearch('', 1, 15)).then(function () {
                                                    Clinical_ImmunizationDetail.UnLoad();
                                                });
                                            });
                                        });
                                    }
                                    else {
                                        utility.DisplayMessages(response.message, 1);
                                        $.when(Clinical_Immunization.ImmunizationSearch('', 1, 15)).then(function () {
                                            Clinical_ImmunizationDetail.UnLoad();
                                        });
                                    }
                                }
                            });
                        }
                        else {
                            utility.DisplayMessages(response.message, 3);
                        }
                    });
                });
            }
        }
    },

    InsertUpdateAdministerVaccine: function (PatientAge) {

        var objData = {};
        var self = $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #frmVaccineHxAdministerTabDetail');
        favoriteListId = self.find('#hfFavoriteListId').val();

        var self = $(' #frmVaccineHxAdministerTabDetail');
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
        objData["ManufacturerId"] = $("#frmVaccineHxAdministerTabDetail #hfAdministerTabManufacturer").val();
        objData["RouteId"] = objDetail["AdministerVaccine_Route"];
        objData["SiteId"] = objDetail["AdministerVaccine_Site"];
        objData["ExpiryDate"] = objDetail["AdministerVaccine_ExpiryDate"];
        objData["VfcId"] = objDetail["AdministerVaccine_VFC"];
        objData["VisDateId"] = $("#" + Clinical_ImmunizationDetail.params.PanelID + " #VisDateId").val();
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
        if ((typeof Clinical_ImmunizationDetail.params["VaccineScheduleId"] == typeof undefined || Clinical_ImmunizationDetail.params["VaccineScheduleId"] == null || Clinical_ImmunizationDetail.params["VaccineScheduleId"] == "") && (typeof Clinical_ImmunizationDetail.params.OrderSetId != typeof undefined && Clinical_ImmunizationDetail.params.OrderSetId != null)) {
            objData["VaccineScheduleId"] = "";
        }
        else {
            objData["VaccineScheduleId"] = Clinical_ImmunizationDetail.params["VaccineScheduleId"] === 0 ? "" : Clinical_ImmunizationDetail.params["VaccineScheduleId"];
        }

        objData["Type"] = Clinical_ImmunizationDetail.AdministerText;
        objData["OverrideRule"] = objDetail["AdministerOverrideRule"];


        if (typeof GetSelectedPatientID() === "undefined") {
            objData["PatientId"] = Clinical_ImmunizationDetail.params.patientID;
        }
        else {
            objData["PatientId"] = GetSelectedPatientID();//163; //
        }
        if (Clinical_ImmunizationDetail.params.TabId == "Batch") {
            objData["PatientId"] = Clinical_ImmunizationDetail.params.patientID;
        }
        if (Clinical_ImmunizationDetail.params.mode == "Add")
            objData["commandType"] = "insert_administervaccine";
        else {
            objData["VaccineHxId"] = Clinical_ImmunizationDetail.params.VaccineHxId;
            if (typeof Clinical_ImmunizationDetail.params.OrderSetId != typeof undefined && Clinical_ImmunizationDetail.params.OrderSetId != null) {
                objData["OrdersetId"] = Clinical_ImmunizationDetail.params.OrderSetId;
            }
            else {
                objData["OrdersetId"] = "";
            }
            objData["commandType"] = "update_administer_vaccine";
            objData["DeleteProcedureIds"] = Clinical_ImmunizationDetail.DeleteProcedureIds
        }
        objData["PatientAge"] = PatientAge;

        //objData["FavListNames"] = Clinical_ImmunizationDetail.CommSeparatedFavListsStatus;
        //objData["FavListVal"] = $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #ddlFavorites').val();
        //if (Clinical_ImmunizationDetail.params.PanelID.indexOf("pnlClinicalProgressNote") > 0) {
        //    objData["NotesId"] = $("#pnlClinicalProgressNote #hfNotesId").val();
        //}
        //else {
        //    objData["NotesId"] = -1;
        //}

        objData["RegisteryId"] = objDetail["AdministerRegistery"];
        objData["FacilityId"] = objDetail["FacilityId"];
        objData["UserId"] = objDetail["AdministerEnteredBy"];
        objData["PreviousVoid"] = Clinical_ImmunizationDetail.previousVoid
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");
    },

    InsertUpdateVaccineHxDose: function (PatientAge) {
        //FavoriteListIcd
        var objData = {};
        var self = $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #frmVaccineHxDocumentHxDoseTabDetail');
        favoriteListId = self.find('#hfFavoriteListId').val();

        var self = $(' #frmVaccineHxDocumentHxDoseTabDetail'); // $('#pnlFavoriteProcedureOrderDetail');
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objDetail = JSON.parse(myJSON);

        objData["CategoryID"] = objDetail["DocumentHxDose_Category"];
        objData["SourceOfHxId"] = objDetail["DocumentHxDose_SourceOfHx"];
        objData["ProviderId"] = objDetail["DocumentHxDose_Provider"];
        objData["VaccineID"] = objDetail["DocumentHxDose_Vaccine"];
        objData["AdministrationDate"] = objDetail["DocumentHxDose_AdministrationDate"];
        objData["Time"] = objDetail["DocumentHxDose_AdministrationTime"];
        objData["Dose"] = objDetail["DocumentHxDose_Dose"];
        objData["Amount"] = objDetail["DocumentHxDose_Amount"];
        objData["RouteId"] = objDetail["DocumentHxDose_Route"];
        objData["SiteId"] = objDetail["DocumentHxDose_Site"];
        objData["VoidDose"] = objDetail["DocumentHxVoidDose"];
        objData["Comments"] = objDetail["DocumentHxDose_Comments"];
        if (Clinical_ImmunizationDetail.params.TabId && Clinical_ImmunizationDetail.params.TabId == "HistoryDose") {
            objData["VaccineScheduleId"] = -2;//only for HistroyDose.(check in sp_InsertVaccineHx)
            objData["IsHistoryDose"] = true;
        }
        else {
            objData["VaccineScheduleId"] = Clinical_ImmunizationDetail.params["VaccineScheduleId"] === 0 ? "" : Clinical_ImmunizationDetail.params["VaccineScheduleId"];
            objData["IsHistoryDose"] = false;
        }
        objData["Type"] = Clinical_ImmunizationDetail.DocumentHxDoseText;

        objData["PublicityCode"] = objDetail["DocumentVaccine_PublicityCode"];
        objData["PublicityCodeExpiryDate"] = objDetail["DocumentVaccine_PublicityExpiryDate"];
        objData["ImmunizationRegistryStatusCode"] = objDetail["DocumentVaccine_IRS"];
        objData["IRSEffectiveDate"] = objDetail["DocumentVaccine_IRSEffectiveDate"];
        objData["ProtectionIndicator"] = objDetail["DocumentVaccine_ProtectionIndicator"];
        objData["PIEffectiveDate"] = objDetail["DocumentVaccine_PIEffectiveDate"];

        if (typeof GetSelectedPatientID() === "undefined") {
            objData["PatientId"] = Clinical_ImmunizationDetail.params.patientID;
        }
        else {
            objData["PatientId"] = GetSelectedPatientID();//163; //
        }
        if (Clinical_ImmunizationDetail.params.TabId == "Batch") {
            objData["PatientId"] = Clinical_ImmunizationDetail.params.patientID;
        }
        objData["IsActive"] = objDetail["DocumentHxDose_IsActive"] == true ? 1 : 0;
        if (Clinical_ImmunizationDetail.params.mode == "Add")
            objData["commandType"] = "insert_administervaccine";
        else {

            objData["VaccineHxId"] = Clinical_ImmunizationDetail.params.VaccineHxId;
            objData["commandType"] = "update_administer_vaccine";
            objData["DeleteProcedureIds"] = Clinical_ImmunizationDetail.DeleteProcedureIds
        }
        //objData["FavListNames"] = Clinical_ImmunizationDetail.CommSeparatedFavListsStatus;
        //objData["FavListVal"] = $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #ddlFavorites').val();
        //if (Clinical_ImmunizationDetail.params.PanelID.indexOf("pnlClinicalProgressNote") > 0) {
        //    objData["NotesId"] = $("#pnlClinicalProgressNote #hfNotesId").val();
        //}
        //else {
        //    objData["NotesId"] = -1;
        //}

        objData["PatientAge"] = PatientAge;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");
    },

    InsertUpdateSaveVaccineRefusalRecord: function (PatientAge) {
        //FavoriteListIcd
        var objData = {};
        var self = $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #frmVaccineHxDocumentHxDoseTabDetail');
        favoriteListId = self.find('#hfFavoriteListId').val();

        var self = $(' #frmVaccineRecordRefusalTabDetail'); // $('#pnlFavoriteProcedureOrderDetail');
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objDetail = JSON.parse(myJSON);

        objData["CategoryID"] = objDetail["RecordRefusal_Category"];
        objData["ProviderId"] = objDetail["RecordRefusal_Provider"];
        objData["VaccineID"] = objDetail["RecordRefusal_Vaccine"];
        objData["RefusalReasonID"] = objDetail["RecordRefusalReason"];
        objData["ExpiryDate"] = objDetail["RecordRefusalVaccine_ExpiryDate"];
        objData["VoidDose"] = objDetail["RecordRefusalVoidDose"];
        objData["Comments"] = objDetail["RecordRefusal_Comments"];
        objData["VaccineScheduleId"] = Clinical_ImmunizationDetail.params["VaccineScheduleId"] === 0 ? "" : Clinical_ImmunizationDetail.params["VaccineScheduleId"];
        objData["Type"] = Clinical_ImmunizationDetail.REFUSALText;
        objData["PublicityCode"] = objDetail["RefusalVaccine_PublicityCode"];
        objData["PublicityCodeExpiryDate"] = objDetail["RefusalVaccine_PublicityExpiryDate"];
        objData["ImmunizationRegistryStatusCode"] = objDetail["RefusalVaccine_IRS"];
        objData["IRSEffectiveDate"] = objDetail["RefusalVaccine_IRSEffectiveDate"];
        objData["ProtectionIndicator"] = objDetail["RefusalVaccine_ProtectionIndicator"];
        objData["PIEffectiveDate"] = objDetail["RefusalVaccine_PIEffectiveDate"];
        if (typeof GetSelectedPatientID() === "undefined") {
            objData["PatientId"] = Clinical_ImmunizationDetail.params.patientID;
        }
        else {
            objData["PatientId"] = GetSelectedPatientID();//163; //
        }
        if (Clinical_ImmunizationDetail.params.TabId == "Batch") {
            objData["PatientId"] = Clinical_ImmunizationDetail.params.patientID;
        }
        objData["IsActive"] = objDetail["RecordRefusal_IsActive"] == true ? 1 : 0;

        if (Clinical_ImmunizationDetail.params.mode == "Add")
            objData["commandType"] = "insert_administervaccine";
        else {

            objData["VaccineHxId"] = Clinical_ImmunizationDetail.params.VaccineHxId;
            objData["commandType"] = "update_administer_vaccine";
            objData["DeleteProcedureIds"] = Clinical_ImmunizationDetail.DeleteProcedureIds
        }

        objData["RegisteryId"] = objDetail["RecordRefusalRegistery"];
        objData["FacilityId"] = objDetail["FacilityId"];
        objData["UserId"] = objDetail["RecordRefusalEnteredBy"];
        //objData["FavListNames"] = Clinical_ImmunizationDetail.CommSeparatedFavListsStatus;
        //objData["FavListVal"] = $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #ddlFavorites').val();
        //if (Clinical_ImmunizationDetail.params.PanelID.indexOf("pnlClinicalProgressNote") > 0) {
        //    objData["NotesId"] = $("#pnlClinicalProgressNote #hfNotesId").val();
        //}
        //else {
        //    objData["NotesId"] = -1;
        //}

        objData["PatientAge"] = PatientAge;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");

    },

    FavListStatusNames: function (favListName) {
        Clinical_ImmunizationDetail.CommSeparatedFavListsStatus = "";
        var isFavListOpened = $('#' + Clinical_ImmunizationDetail.params.PanelID + " #favSectionDiv").hasClass("toggledHor");
        Clinical_ImmunizationDetail.CommSeparatedFavListsStatus = "";
        if (isFavListOpened == true) {
            Clinical_ImmunizationDetail.CommSeparatedFavListsStatus = EMRUtility.setFavListStatus(favListName, true);
        }
        else {
            Clinical_ImmunizationDetail.CommSeparatedFavListsStatus = EMRUtility.setFavListStatus(favListName, false);
        }
    },

    UpdateVaccineInProcedureVoidDoes: function (VoidDoes, VaccineHxId) {
        Clinical_ImmunizationDetail.DeleteProcedureIds = "";
        var dfd = $.Deferred();
        if (Clinical_ImmunizationDetail.previousVoid != VoidDoes) {
            Clinical_ImmunizationDetail.GetVaccineHxIds(VaccineHxId).done(function (resultVaccineHxIds) {
                if (resultVaccineHxIds) {
                    var VaccineHxIdList = resultVaccineHxIds.split(',');

                    var DeleteProceduresDetail = [];
                    $(VaccineHxIdList).each(function (jj, VaccineHxId) {
                        if (Clinical_ImmunizationDetail.previousVoid == false && VoidDoes == true) {
                            if (jj == 0) {
                                Clinical_ImmunizationDetail.DeleteProcedureIds += VaccineHxId;
                            }
                            else {
                                Clinical_ImmunizationDetail.DeleteProcedureIds += "," + VaccineHxId;
                            }
                        }
                    });
                    if (Clinical_ImmunizationDetail.DeleteProcedureIds != "") {
                        dfd.response = 1;
                        dfd.resolve();
                    }
                    else {
                        dfd.response = 1;
                        dfd.resolve();
                    }
                }
                else {
                    dfd.response = 1;
                    dfd.resolve();
                }
            });
            dfd.response = 1;
            dfd.resolve();
        }
        else {
            dfd.response = 1;
            dfd.resolve();
        }
        return dfd;
    },

    //#endregion Vaccine Hx Transaction
    BindSchedule: function ($obj, ddl, Value) {
        var dfd = $.Deferred();
        if ($($obj).val() != '') {
            var ScheduleTypeid = $($obj).val();
            $.when(CacheManager.BindDropDownsByID("#" + Clinical_ImmunizationDetail.params["PanelID"] + ' #' + ddl, 'GetImmunizationSchedule', true, ScheduleTypeid)).then(function () {
                if (Value != null && typeof Value != typeof undefined) {
                    $("#" + Clinical_ImmunizationDetail.params.PanelID + " #ddlSchedule").val(Value);
                    dfd.resolve();
                }
                else {
                    dfd.resolve();
                }
            });
        }
        else {
            $("#" + Clinical_ImmunizationDetail.params.PanelID + " #ddlSchedule").html("<option value=''>- Select -</option>");
            $("#" + Clinical_ImmunizationDetail.params.PanelID + " #ddlCategory").html("<option value=''>- Select -</option>");
            if (!$("#" + Clinical_ImmunizationDetail.params.PanelID + " #ClinicalVaccineTabsDiv").hasClass("disableAll")) {
                $("#" + Clinical_ImmunizationDetail.params.PanelID + " #ClinicalVaccineTabsDiv").addClass("disableAll")
            }
            dfd.resolve();
        }
        return dfd;
    },
    BindCategory: function ($obj, ddl, Value) {
        var dfd = $.Deferred();
        if ($($obj).val() != '') {
            var Scheduleid = $($obj).val();
            var ScheduleTypeid = $("#" + Clinical_ImmunizationDetail.params.PanelID + " #ddlAgeGroup").val();

            var self = $('#' + Clinical_ImmunizationDetail.params.PanelID);
            self.find('#ddlCategory').attr('ddlist', 'GetCategoryAgaintsSchAndSchtype');
            var data = "IsActive=&ID=" + ScheduleTypeid + "&ID2=" + Scheduleid;
            self.find('#' + ddl).parent().loadDropDowns(true, data).done(function () {
                if (Value != null && typeof Value != typeof undefined) {
                    $("#" + Clinical_ImmunizationDetail.params.PanelID + " #ddlCategory").val(Value);
                    dfd.resolve();
                }
                else {
                    dfd.resolve();
                }
            });

        }
        else {
            $("#" + Clinical_ImmunizationDetail.params.PanelID + " #ddlCategory").html("<option value=''>- Select -</option>");
            if (!$("#" + Clinical_ImmunizationDetail.params.PanelID + " #ClinicalVaccineTabsDiv").hasClass("disableAll")) {
                $("#" + Clinical_ImmunizationDetail.params.PanelID + " #ClinicalVaccineTabsDiv").addClass("disableAll")
            }
            dfd.resolve();
        }
        return dfd;
    },
    CategoryChange: function (obj) {
        if ($(obj).val() != '') {
            if ($("#" + Clinical_ImmunizationDetail.params.PanelID + " #ClinicalVaccineTabsDiv").hasClass("disableAll")) {
                $("#" + Clinical_ImmunizationDetail.params.PanelID + " #ClinicalVaccineTabsDiv").removeClass("disableAll")
            }
            $.when(Clinical_ImmunizationDetail.GetAgeLimScheCategAgainstVaccShedId()).then(function () {
                Clinical_ImmunizationDetail.IntializeAllTabs();
            });
        }
        else {
            if (!$("#" + Clinical_ImmunizationDetail.params.PanelID + " #ClinicalVaccineTabsDiv").hasClass("disableAll")) {
                $("#" + Clinical_ImmunizationDetail.params.PanelID + " #ClinicalVaccineTabsDiv").addClass("disableAll")
            }
        }

    },
    IntializeAllTabs: function () {
        Clinical_ImmunizationDetail.bIsFirstLoad = false;

        Clinical_ImmunizationDetail.CheckPatientInsuranceIsMedicare();
        Clinical_ImmunizationDetail.PopulateVisitDate();
        if (Clinical_ImmunizationDetail.params.mode == "Add" && Clinical_ImmunizationDetail.params["VaccineScheduleId"] != null && Clinical_ImmunizationDetail.params["VaccineScheduleId"] != 0) {
            if (typeof Clinical_ImmunizationDetail.params["VaccineScheduleId"] != typeof undefined && Clinical_ImmunizationDetail.params["VaccineScheduleId"] != null && Clinical_ImmunizationDetail.params["VaccineScheduleId"] != "") {
                $.when(IsOver = Clinical_ImmunizationDetail.IsAdministrationPeriodOver(Clinical_ImmunizationDetail.params["VaccineScheduleId"])).then(function () {
                    if (IsOver.response == "1") {
                        $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail #chkAdministerOverrideRule").prop("checked", true);
                        // Save Vaccine Automaticallay
                    }
                });
            }
        }
        $.when(Clinical_ImmunizationDetail.setPatientProvider()).then(function () {
            var NotCallWhyLotIsNotAva = false;
            if (Clinical_ImmunizationDetail.params.mode == "Edit" && Clinical_ImmunizationDetail.params.Type == "ADMINISTER") {
                NotCallWhyLotIsNotAva = true;
            }
            var def = [];
            def.push($.when(Clinical_ImmunizationDetail.PopulateVaccineGroupCategory(true, "#ddlAdministerVaccine_Category", "GetAdministerVaccine_Category", null, Clinical_ImmunizationDetail.params.CategoryId, NotCallWhyLotIsNotAva)).then(function () {

            }))
            def.push($.when(Clinical_ImmunizationDetail.PopulateVaccineGroupCategory(true, "#ddlDocumentHxDose_Category", "GetAdministerVaccine_Category", null, Clinical_ImmunizationDetail.params.CategoryId, NotCallWhyLotIsNotAva)).then(function () {

            }))
            def.push($.when(Clinical_ImmunizationDetail.PopulateVaccineGroupCategory(true, "#ddlRecordRefusal_Category", "GetAdministerVaccine_Category", null, Clinical_ImmunizationDetail.params.CategoryId, NotCallWhyLotIsNotAva)).then(function () {

            }))
            $.when.apply($, def).done(function ($n) {
                $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #dpAdministerVaccine_AdministrationDate').datepicker('setDate', $('#dpAdministerVaccine_AdministrationDate').val());

                if (Clinical_ImmunizationDetail.params.mode == "Add") {

                    Clinical_ImmunizationDetail.LoadFavVaccine(true);
                }
                else if (Clinical_ImmunizationDetail.params.mode == "Edit") {
                    $("#" + Clinical_ImmunizationDetail.params.PanelID + " #favSectionDiv").hide();
                    var vaccineId = Clinical_ImmunizationDetail.params.VaccineHxId;

                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxAdministerTabDetail #chkAdministerVoidDose").attr("disabled", false);
                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineHxDocumentHxDoseTabDetail #chkDocumentHxVoidDose").attr("disabled", false);
                    $('#' + Clinical_ImmunizationDetail.params.PanelID + " #frmVaccineRecordRefusalTabDetail #chkRecordRefusalVoidDose").attr("disabled", false);
                    if (vaccineId > 0) {

                        Clinical_ImmunizationDetail.params.TabType;
                        Clinical_ImmunizationDetail.params.mode = "Edit";
                        if (Clinical_ImmunizationDetail.params.Type == "ADMINISTER") {
                            $('#' + Clinical_ImmunizationDetail.params.PanelID).find('[id*="listAdministerVaccine"]').addClass('active');
                            $('#' + Clinical_ImmunizationDetail.params.PanelID).find('[id*="tabAdministerVaccine"]').addClass('active');
                            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #listRecordRefusal').removeClass('active');
                            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tabRecordRefusal').removeClass('active');
                            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #listDocumentHxDose').removeClass('active');
                            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tabDocumentHxDose').removeClass('active');
                        }
                        else if (Clinical_ImmunizationDetail.params.Type == "DOCUMENTHX") {
                            $('#' + Clinical_ImmunizationDetail.params.PanelID + " #listAdministerVaccine").removeClass('active');
                            $('#' + Clinical_ImmunizationDetail.params.PanelID + " #tabAdministerVaccine").removeClass('active');
                            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #listRecordRefusal').removeClass('active');
                            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tabRecordRefusal').removeClass('active');
                            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #listDocumentHxDose').addClass('active');
                            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tabDocumentHxDose').addClass('active');
                        }
                        else if (Clinical_ImmunizationDetail.params.Type == "REFUSAL") {
                            $('#' + Clinical_ImmunizationDetail.params.PanelID + " #listAdministerVaccine").removeClass('active');
                            $('#' + Clinical_ImmunizationDetail.params.PanelID + " #tabAdministerVaccine").removeClass('active');
                            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #listRecordRefusal').addClass('active');
                            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tabRecordRefusal').addClass('active');
                            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #listDocumentHxDose').removeClass('active');
                            $('#' + Clinical_ImmunizationDetail.params.PanelID + ' #tabDocumentHxDose').removeClass('active');
                        }
                        $.when(Clinical_ImmunizationDetail.LoadFavVaccine(true)).then(function () {
                            Clinical_ImmunizationDetail.searchVaccineHx(vaccineId).done(function (response) {
                                Clinical_ImmunizationDetail.bindVaccineHxDetails(response);
                            });
                        });
                    }

                }
                else {

                }
                var comesFromScheduleChart = Clinical_ImmunizationDetail.params["ScheduleChartType"] === undefined ? false : true;

                if (comesFromScheduleChart) {

                    var vaccineCategoryArray = [Clinical_ImmunizationDetail.ddlAdministerVaccine_Category, Clinical_ImmunizationDetail.ddlDocumentHxDose_Category];
                    var vaccineTabTypeArray = [Clinical_ImmunizationDetail.AdministerText, Clinical_ImmunizationDetail.DocumentHxDoseText];


                    for (var i = 0; i < vaccineCategoryArray.length; i++) {
                        var controlId = " #pnlClinicalImmunizationDetail #" + vaccineCategoryArray[i];
                        $(controlId).val(Clinical_ImmunizationDetail.params["VaccineGroupId"]);
                        Clinical_ImmunizationDetail.PopulateVaccineDropDown(Clinical_ImmunizationDetail.params["VaccineGroupId"], vaccineTabTypeArray[i]);
                        $(controlId).attr("disabled", true);
                    }
                }
            });
        });
    },
    GetAgeLimScheCategAgainstVaccShedId: function () {
        var dfd = $.Deferred();
        Clinical_ImmunizationDetail.GetVaccShedIdAgainstAgeLimScheCateg_DBCALL().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.VaccineScheduleId > 0) {
                    Clinical_ImmunizationDetail.params.VaccineScheduleId = response.VaccineScheduleId;
                    Clinical_ImmunizationDetail.params.CategoryId = $("#" + Clinical_ImmunizationDetail.params.PanelID + " #ddlCategory option:selected").text();
                    Clinical_ImmunizationDetail.params.Category = $("#" + Clinical_ImmunizationDetail.params.PanelID + " #ddlCategory").val();
                    dfd.resolve();
                }
                else {
                    dfd.resolve();
                    utility.DisplayMessages("Vaccine Shcedule is not found.", 3);
                }
            }
            else {
                dfd.resolve();
                utility.DisplayMessages(response.message, 3);
            }
        });
        return dfd;
    },
    GetVaccShedIdAgainstAgeLimScheCateg_DBCALL: function () {
        var AgeLimitId = $("#" + Clinical_ImmunizationDetail.params.PanelID + " #ddlAgeGroup").val();
        var ScheduleId = $("#" + Clinical_ImmunizationDetail.params.PanelID + " #ddlSchedule").val();
        var CategoryId = $("#" + Clinical_ImmunizationDetail.params.PanelID + " #ddlCategory").val();
        var objData = new Object();
        objData["MainAgeGroup"] = AgeLimitId;
        objData["MainSchedule"] = ScheduleId;
        objData["MainCategory"] = CategoryId;
        objData["commandType"] = "get_vaccshedid_against_agelim_sche_categ";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Immunization");
    },
}