//Author : Zia Mehmood
//Date : 12/08/2017

Batch_ExportCCDA = {
    bIsFirstLoad: true,
    params: [],
    PatientId: 0,
    XMLContent: "",
    NoteCompnentsCheckedIds: [],
    NoteCompnentsIds: '',
    YearsCheckedIds: [],
    IsUpdate: false,
    IsCreated: false,
    IsActive: '1',
    SchedulerId: "",
    YearsIds: '',
    MonthsCheckedIds: [],
    MonthsIds: '',
    WeeksCheckedIds: [],
    WeeksIds: '',
    ProviderCheckedIds: [],
    ProviderIds: '',
    PatientsCheckedIds: [],
    PatientsIds: '',
    RefProviderSimple: [],
    DaysCheckedIds: [],
    objfiledownload: null,
    IsdownloadCDDA:false,
    defLeng: 0,
    PatientCount: 0,
    visitLength:0,
    visitCount:0,
    DaysIds: '',
    asdwe: 0,
    zip: null,
    PatientCounter: 0,
    PatientLength: 0,
    VisitsCounter:0,
    VisitsLength: 0,
    counter:0,

    Load: function (params) {
        Batch_ExportCCDA.params.PanelID = "pnlBatchExportCCDA";
        var self = $("#" + Batch_ExportCCDA.params["PanelID"] + " #divRoleType");
        if (Batch_ExportCCDA.bIsFirstLoad == true) {


            Batch_ExportCCDA.IsCreated = false;
            self.loadDropDowns(true);
            self = null;
            self = $("#" + Batch_ExportCCDA.params["PanelID"] + " #divYears");
            self.loadDropDowns(true);
            Batch_ExportCCDA.loadEntityProvider(globalAppdata["SeletedEntityId"]);
            Batch_ExportCCDA.IntializeMultiSelectDropDownMonth();
            Batch_ExportCCDA.IntializeMultiSelectDropDownWeeks();

            Batch_ExportCCDA.IntializeMultiSelectDropDownDays();
            Batch_ExportCCDA.LoadNoteComponentsLookup();

            Batch_ExportCCDA.IntializeMultiSelectDropDownYears();

        }
        Batch_ExportCCDA.LoadAllAutocomplete();
        Batch_ExportCCDA.LoadCCDASchedule();

        $("#ddlRoleType").change(function () {
            Batch_ExportCCDA.HideShowOnceOrReoccuring();
        })


        // Batch_ExportCCDA.readyFunction();
        if (Batch_ExportCCDA.bIsFirstLoad == true) {
            Batch_ExportCCDA.bIsFirstLoad = false;
            Batch_ExportCCDA.ValidateBatch_ExportCCDA();
            if (Batch_ExportCCDA.IsCreated == false) {
                $('#pnlBatchExportCCDA    #txtpatientMulti').trigger('oninput');
                Batch_ExportCCDA.IsCreated = true;
            }
            setTimeout(function () {
                $('#' + Batch_ExportCCDA.params.PanelID + ' #divNoteComponents ul li:contains("Demographic Data Element")').find('a').addClass('disableAll');
                $('#' + Batch_ExportCCDA.params.PanelID + ' #divNoteComponents ul li:contains("Provider Data Element")').find('a').addClass('disableAll');
            }, 500);
        }
    },



    readyFunction: function () {


    },

    ValidateBatch_ExportCCDA: function () {
        $('#pnlBatchExportCCDA')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  //DateTo: {
                  //    group: '.col-sm-2',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        },
                  //        date: {
                  //            format: date_format.toUpperCase(),
                  //            message: ' '
                  //        }
                  //    }
                  //},
                  //DateFrom: {
                  //    group: '.col-sm-2',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        },
                  //        date: {
                  //            format: date_format.toUpperCase(),
                  //            message: ' '
                  //        }
                  //    }
                  //},
                  //patientMulti: {
                  //    group: '.col-sm-3',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        },
                  //    }
                  //},
                  RoleType: {
                      group: '.col-sm-2',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  DateExport: {
                      group: '.col-sm-2',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  //Years: {
                  //    group: '.col-sm-2',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        },
                  //    }
                  //},
                  Time: {
                      group: '.col-sm-2',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  MultiTime: {
                      group: '.col-sm-2',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  FilePath: {
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
           if (Batch_ExportCCDA.IsCreated == false) {
               $('#pnlBatchExportCCDA    #txtpatientMulti').trigger('oninput');
               Batch_ExportCCDA.IsCreated = true;
           }

           //e.stopPropagation();
           //$('#pnlBatchExportCCDA ').bootstrapValidator('revalidateField', 'DateFrom');
           //$('#pnlBatchExportCCDA ').bootstrapValidator('revalidateField', 'DateTo');
           //$('#pnlBatchExportCCDA ').bootstrapValidator('revalidateField', 'DateExport');
           var selectedPatient = null;
           if ($('#pnlBatchExportCCDA   #ddlProvider').val() == "0") {
               selectedPatient = $('#pnlBatchExportCCDA    #txtpatientMulti').data("kendoMultiSelect").value();
           } else {
               selectedPatient = $('#pnlBatchExportCCDA   #ddlPatients').val();
           }
           if (selectedPatient == null) {
           selectedPatient=[];
           }
           if (selectedPatient.length > 0 && selectedPatient[0] != "") {
               if ($('#pnlBatchExportCCDA   #ddlProvider').val() == "0") {
                   Batch_ExportCCDA.validatePatientMulti(2);
               } else {
                   Batch_ExportCCDA.validatePatients(2);
           }
               e.preventDefault();
               Batch_ExportCCDA.SaveCCDASchedule();
           } else {
               if ($('#pnlBatchExportCCDA   #ddlProvider').val() == "0") {
                   Batch_ExportCCDA.validatePatientMulti(1);
               } else {
                   Batch_ExportCCDA.validatePatients(1);
               }
               return false;
           }


       });
    },

    // Validation of Multiselect
    CheckMonthsValidation: function () {
        var self = $('#pnlBatchExportCCDA');
        var monthsIds = self.find('#ddlMonths option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        var strMonthsIds = monthsIds;
        Batch_ExportCCDA.MonthsIds = strMonthsIds;
        if (Batch_ExportCCDA.MonthsIds != '') {
            Batch_ExportCCDA.validateMonths(2);
        } else {
            Batch_ExportCCDA.validateMonths(1);
        }
    },
    validateMonths: function (operationid) {
        $("#pnlBatchExportCCDA  #divMonths label").find("i").remove();
        if (operationid == 1) {
            $("#pnlBatchExportCCDA  #divMonths .multiselect").css("border-color", "#cc2724");
            $("#pnlBatchExportCCDA  #divMonths").find(".control-label").css("color", "#cc2724");
            $("#pnlBatchExportCCDA  #divMonths").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-remove" data-bv-icon-for="Race" style="display: block;"></i>');
        } else if (operationid == 2) {
            $("#pnlBatchExportCCDA  #divMonths .multiselect").css("border-color", "#3c763d");
            $("#pnlBatchExportCCDA  #divMonths").find(".control-label").css("color", "#3c763d");
            $("#pnlBatchExportCCDA  #divMonths").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-ok" data-bv-icon-for="Race" style="display: block;"></i>');
        } else {
            $("#pnlBatchExportCCDA  #divMonths .multiselect").css("border-color", "#ccc");
            $("#pnlBatchExportCCDA  #divMonths").find(".control-label").css("color", "#000000");
        }
    },
    validatePatients: function (operationid) {
        $("#pnlBatchExportCCDA  #divPatients label").find("i").remove();
        if (operationid == 1) {
            $("#pnlBatchExportCCDA  #divPatients .multiselect").css("border-color", "#cc2724");
            $("#pnlBatchExportCCDA  #divPatients").find(".control-label").css("color", "#cc2724");
            $("#pnlBatchExportCCDA  #divPatients").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-remove" data-bv-icon-for="Race" style="display: block;"></i>');
        } else if (operationid == 2) {
            $("#pnlBatchExportCCDA  #divPatients .multiselect").css("border-color", "#3c763d");
            $("#pnlBatchExportCCDA  #divPatients").find(".control-label").css("color", "#3c763d");
            $("#pnlBatchExportCCDA  #divPatients").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-ok" data-bv-icon-for="Race" style="display: block;"></i>');
        } else {
            $("#pnlBatchExportCCDA  #divPatients .multiselect").css("border-color", "#ccc");
            $("#pnlBatchExportCCDA  #divPatients").find(".control-label").css("color", "#000000");
        }
    },
    //validateWeeks: function (operationid) {
    //    $("#pnlBatchExportCCDA  #divWeeks label").find("i").remove();
    //    if (operationid == 1) {
    //        $("#pnlBatchExportCCDA  #divWeeks .multiselect").css("border-color", "#cc2724");
    //        $("#pnlBatchExportCCDA  #divWeeks").find(".control-label").css("color", "#cc2724");
    //        $("#pnlBatchExportCCDA  #divWeeks").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-remove" data-bv-icon-for="Race" style="display: block;"></i>');
    //    } else if (operationid == 2) {
    //        $("#pnlBatchExportCCDA  #divWeeks .multiselect").css("border-color", "#3c763d");
    //        $("#pnlBatchExportCCDA  #divWeeks").find(".control-label").css("color", "#3c763d");
    //        $("#pnlBatchExportCCDA  #divWeeks").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-ok" data-bv-icon-for="Race" style="display: block;"></i>');
    //    } else {
    //        $("#pnlBatchExportCCDA  #divWeeks .multiselect").css("border-color", "#ccc");
    //        $("#pnlBatchExportCCDA  #divWeeks").find(".control-label").css("color", "#000000");
    //    }
    //},
    validateDays: function (operationid) {
        $("#pnlBatchExportCCDA  #divDays label").find("i").remove();
        if (operationid == 1) {
            $("#pnlBatchExportCCDA  #divDays .multiselect").css("border-color", "#cc2724");
            $("#pnlBatchExportCCDA  #divDays").find(".control-label").css("color", "#cc2724");
            $("#pnlBatchExportCCDA  #divDays").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-remove" data-bv-icon-for="Race" style="display: block;"></i>');
        } else if (operationid == 2) {
            $("#pnlBatchExportCCDA  #divDays .multiselect").css("border-color", "#3c763d");
            $("#pnlBatchExportCCDA  #divDays").find(".control-label").css("color", "#3c763d");
            $("#pnlBatchExportCCDA  #divDays").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-ok" data-bv-icon-for="Race" style="display: block;"></i>');
        } else {
            $("#pnlBatchExportCCDA  #divDays .multiselect").css("border-color", "#ccc");
            $("#pnlBatchExportCCDA  #divDays").find(".control-label").css("color", "#000000");
        }
    },
    validateYears: function (operationid) {
        $("#pnlBatchExportCCDA  #divYears label").find("i").remove();
        if (operationid == 1) {
            $("#pnlBatchExportCCDA  #divYears .multiselect").css("border-color", "#cc2724");
            $("#pnlBatchExportCCDA  #divYears").find(".control-label").css("color", "#cc2724");
            $("#pnlBatchExportCCDA  #divYears").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-remove" data-bv-icon-for="Race" style="display: block;"></i>');
        } else if (operationid == 2) {
            $("#pnlBatchExportCCDA  #divYears .multiselect").css("border-color", "#3c763d");
            $("#pnlBatchExportCCDA  #divYears").find(".control-label").css("color", "#3c763d");
            $("#pnlBatchExportCCDA  #divYears").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-ok" data-bv-icon-for="Race" style="display: block;"></i>');
        } else {
            $("#pnlBatchExportCCDA  #divYears .multiselect").css("border-color", "#ccc");
            $("#pnlBatchExportCCDA  #divYears").find(".control-label").css("color", "#000000");
        }
    },
    
    validatePatientMulti: function (operationid) {
        $("#pnlBatchExportCCDA  #divpatientMulti label").find("i").remove();
        if (operationid == 1) {
            $("#pnlBatchExportCCDA  #divpatientMulti .multiselect").css("border-color", "#cc2724");
            $("#pnlBatchExportCCDA  #divpatientMulti").find(".control-label").css("color", "#cc2724");
            $("#pnlBatchExportCCDA  #divpatientMulti").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-remove" data-bv-icon-for="Race" style="display: block;"></i>');
        } else if (operationid == 2) {
            $("#pnlBatchExportCCDA  #divpatientMulti .multiselect").css("border-color", "#3c763d");
            $("#pnlBatchExportCCDA  #divpatientMulti").find(".control-label").css("color", "#3c763d");
            $("#pnlBatchExportCCDA  #divpatientMulti").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-ok" data-bv-icon-for="Race" style="display: block;"></i>');
        } else {
            $("#pnlBatchExportCCDA  #divpatientMulti .multiselect").css("border-color", "#ccc");
            $("#pnlBatchExportCCDA  #divpatientMulti").find(".control-label").css("color", "#000000");
        }
    },
    // End Validation of Multiselect



    HideShowOnceOrReoccuring: function () {
        if ($('#ddlRoleType option:selected').text() == "Recurring") {
            $('#divReOcurring').removeClass('hidden');
            $('#divOnetime').addClass('hidden');
            // $('#pnlBatchExportCCDA').data('bootstrapValidator').enableFieldValidators('Years', true);
            $('#pnlBatchExportCCDA').data('bootstrapValidator').enableFieldValidators('Time', false);
            $('#pnlBatchExportCCDA').data('bootstrapValidator').enableFieldValidators('DateExport', false);
            $('#pnlBatchExportCCDA #btnDownloadCCDA').addClass('disableAll');
            $('#pnlBatchExportCCDA #btnDownloadCCDA2').addClass('disableAll');
        }
        else {
            $('#divReOcurring').addClass('hidden');
            $('#divOnetime').removeClass('hidden');
            // $('#pnlBatchExportCCDA').data('bootstrapValidator').enableFieldValidators('Years', false);
            $('#pnlBatchExportCCDA').data('bootstrapValidator').enableFieldValidators('Time', true);
            $('#pnlBatchExportCCDA').data('bootstrapValidator').enableFieldValidators('DateExport', true);
            if (Batch_ExportCCDA.IsCreated == false) {
                $('#pnlBatchExportCCDA    #txtpatientMulti').trigger('oninput');
                Batch_ExportCCDA.IsCreated = true;
            }
            var selectval = $('#pnlBatchExportCCDA    #txtpatientMulti').data("kendoMultiSelect").value();

            if ( selectval[0] != 40000)
            {
                $('#pnlBatchExportCCDA #btnDownloadCCDA').removeClass('disableAll');
                $('#pnlBatchExportCCDA #btnDownloadCCDA2').removeClass('disableAll');
        }

        }
    },


    getfolder: function (e) {
        //var files = e.target.files;
        var path = e.files[0].mozFullPath;
        var Folder = path.split("/");
        alert(Folder[0]);
    },
    // added by Zia Mehmood For Patient Mulitselect
    LoadAllAutocomplete: function () {
        //on date change, validating time
        $('#pnlBatchExportCCDA  #Time').timepicker().on('changeTime.timepicker', function (e) {
            disableFocus: false;
            if ($('#pnlBatchExportCCDA ').data("bootstrapValidator") != null && typeof $('#pnlBatchExportCCDA').data('bootstrapValidator') != 'undefined') {
                $('#pnlBatchExportCCDA ').bootstrapValidator('revalidateField', 'Time');
            }
        });

        $('#pnlBatchExportCCDA  #MultiTime').timepicker({
            defaultTime: 'now',
            minuteStep: 5//,
        });

        $('#pnlBatchExportCCDA  #Time').timepicker('setTime', new Date());


        $('#pnlBatchExportCCDA  #txtMultiTime').timepicker().on('changeTime.timepicker', function (e) {
            disableFocus: false;
            if ($('#pnlBatchExportCCDA ').data("bootstrapValidator") != null && typeof $('#pnlBatchExportCCDA').data('bootstrapValidator') != 'undefined') {
                $('#pnlBatchExportCCDA ').bootstrapValidator('revalidateField', 'MultiTime');
            }
        });

        $('#pnlBatchExportCCDA  #txtMultiTime').timepicker({
            defaultTime: 'now',
            minuteStep: 5//,
        });

        $('#pnlBatchExportCCDA  #txtMultiTime').timepicker('setTime', new Date());

        $('#pnlBatchExportCCDA  #txtMultiTime2').timepicker().on('changeTime.timepicker', function (e) {
            disableFocus: false;
            if ($('#pnlBatchExportCCDA ').data("bootstrapValidator") != null && typeof $('#pnlBatchExportCCDA').data('bootstrapValidator') != 'undefined') {
                // $('#pnlBatchExportCCDA ').bootstrapValidator('revalidateField', 'MultiTime2');
            }
        });

        $('#pnlBatchExportCCDA  #txtMultiTime2').timepicker({
            defaultTime: 'now',
            minuteStep: 5//,
        });

        $('#pnlBatchExportCCDA  #txtMultiTime2').timepicker('setTime', new Date());

        //$('#pnlBatchExportCCDA  #txtMultiTime2').timepicker().on('changeTime.timepicker', function (e) {
        //   // disableFocus: false;
        //   // format: 'hh:mm';
        //   // pickDate: false;
        //   // pickSeconds: false;
        //   // pick12HourFormat: false;
        //   // //defaultTime: 'now',
        //   // timeFormat: 'G:i';
        //   // show2400: true;
        //    //// use24hours: true;
        //    use24hours: true
        //    if ($('#pnlBatchExportCCDA ').data("bootstrapValidator") != null && typeof $('#pnlBatchExportCCDA').data('bootstrapValidator') != 'undefined') {
        //       // $('#pnlBatchExportCCDA ').bootstrapValidator('revalidateField', 'txtMultiTime2');
        //    }
        //});

        //$('#pnlBatchExportCCDA  #txtMultiTime2').datetimepicker({
        //    format: 'HH:mm',
        //    //pickDate: false,
        //    //pickSeconds: false,
        //    //pickHourFormat: false,
        //    ////defaultTime: 'now',
        //    //timeFormat: 'G:i',
        //    //show2400: true,
        //    //use24hours: true,
        //    //minuteStep: 5//,
        //});

        /// $('#pnlBatchExportCCDA  #txtMultiTime2').timepicker('setTime', new Date());

        //$("#txtMultiTime2").kendoTimePicker();

        utility.CreateDatePicker("pnlBatchExportCCDA #dtpDateExport", function () {

            if ($('#pnlBatchExportCCDA').data("bootstrapValidator") != null) {
                $('#pnlBatchExportCCDA').bootstrapValidator('revalidateField', 'DateExport');
            }
        }, true);


        utility.CreateDatePicker("pnlBatchExportCCDA #dtpDateTo",
      function (ev) {

          if ($('#pnlBatchExportCCDA').data("bootstrapValidator") != null) {
             // $('#pnlBatchExportCCDA').bootstrapValidator('revalidateField', 'DateTo');
          }

      }, true);
        $('#pnlBatchExportCCDA  #dtpDateTo').datepicker('setEndDate', new Date());

        utility.CreateDatePicker("pnlBatchExportCCDA  #dtpDateFrom",
    function (ev) {

        if ($('#pnlBatchExportCCDA ').data("bootstrapValidator") != null) {
           // $('#pnlBatchExportCCDA ').bootstrapValidator('revalidateField', 'DateFrom');

        }

        //on-change callback method
    }, true);
        $('#pnlBatchExportCCDA #dtpDateFrom').datepicker('setEndDate', new Date());
        utility.ValidateFromToDate('pnlBatchExportCCDA', 'dtpDateFrom', 'dtpDateTo', true, null, null, true);

    },
    LoadPatientMultiSelect: function (obj) {
        Batch_ExportCCDA.IsCreated = true;
        //CacheManager.BindCodes('GetRefProviders', false).done(function (result) {

        $('#pnlBatchExportCCDA    #txtpatientMulti').kendoMultiSelect({
            dataValueField: "id",
            dataTextField: "value",
            dataSource: new kendo.data.DataSource({
                schema: {
                    data: "data"
                },
                serverFiltering: true,
                transport: {
                    read: function (e) {
                        var objData = {};
                        objData.PatientName = $('#pnlBatchExportCCDA #txtpatientMulti').data("kendoMultiSelect")._prev;
                        objData.commandType = "FILL_PATIENT_LOOKUP";
                        var data = JSON.stringify(objData);
                        MDVisionService.APIService(data, "Batch_ExportCCDA", "ExportCCDA").done(function (response) {
                            if (response != "") {
                                var CPTData = { data: [] };
                                var temp_res = jQuery.parseJSON(response);
                                if (temp_res.ExportCCDACount > 0) {
                                    result = jQuery.parseJSON(temp_res.ExportCCDA_JSON);
                                    $.each(result, function (j, item) {
                                        CPTData.data.push({ id: item.PatientId, value: item.PatientFullName });
                                    });
                                }
                                e.success(CPTData);
                            }
                        })
                    }
                }
            }),
            separator: ", ",
            select: Batch_ExportCCDA.onSelectPatient,
            change: Batch_ExportCCDA.onChangePatient,
            open: function (e) {
                if (this.input.val().length == 0) {
                    e.preventDefault();
                }
            }
        });
        // });
    },

    onSelectPatient: function (e) {
        var selectval = $('#pnlBatchExportCCDA    #txtpatientMulti').data("kendoMultiSelect").value();

        var dataItem = this.dataSource.view()[e.item.index()];
        if (selectval.length > 0 && selectval[0] == "-1") {
            $('#pnlBatchExportCCDA    #txtpatientMulti').data("kendoMultiSelect").value(dataItem.value);

        }
        if (dataItem.id == 40000) {
            Batch_ExportCCDA.asdwe = dataItem.id;
            $('#pnlBatchExportCCDA #hfIspatientAll').val('1');
            $('#pnlBatchExportCCDA    #txtpatientMulti').data("kendoMultiSelect").value([dataItem.id]);
            //  $('#pnlBatchExportCCDA    #txtpatientMulti').data("kendoMultiSelect").trigger('change');

        } else if (dataItem.id != 40000 && $('#pnlBatchExportCCDA #hfIspatientAll').val() == "1") {
            //var items = $.grep(selectval, function (item,i) {
            //    return item = "-1";
            Batch_ExportCCDA.asdwe = dataItem.id;
            //});
            //if (items.length > 0)
            //{
            $('#pnlBatchExportCCDA    #txtpatientMulti').data("kendoMultiSelect").value([dataItem.id]);
            // $('#pnlBatchExportCCDA    #txtpatientMulti').data("kendoMultiSelect").trigger('change');

            //}
            $('#pnlBatchExportCCDA #hfIspatientAll').val('0');
        }

        if (($('#pnlBatchExportCCDA #hfIspatientAll').val() == "1" ) || $('#ddlRoleType').val() != "Onetime") {
            // $('#pnlBatchExportCCDA #btnDisplayCCDA').addClass('disableAll');
            $('#pnlBatchExportCCDA #btnDownloadCCDA').addClass('disableAll');
        } else {
            // $('#pnlBatchExportCCDA #btnDisplayCCDA').removeClass('disableAll');
            $('#pnlBatchExportCCDA #btnDownloadCCDA').removeClass('disableAll');
        }
        if (selectval.length > 0 || dataItem.id) {
            $('#pnlBatchExportCCDA #btnDownloadCCDA').removeClass('disableAll');
        } else {
            $('#pnlBatchExportCCDA #btnDownloadCCDA').addClass('disableAll');
        }

        RefProviderSimple.push(dataItem.id);
        $('#pnlBatchExportCCDA #hfPatientMulti').text("");
        for (i = 0; i < RefProviderSimple.length; i++) {
            $('#pnlBatchExportCCDA #hfPatientMulti').append(RefProviderSimple[i] + ',');
        }

    },

    onChangePatient: function (e) {
        RefProviderSimple = e.sender._old;
        $('#pnlBatchExportCCDA #hfPatientMulti').text("");
        for (i = 0; i < RefProviderSimple.length; i++) {
            $('#pnlBatchExportCCDA #hfPatientMulti').append(RefProviderSimple[i] + ',');
        }

        var selectval = $('#pnlBatchExportCCDA    #txtpatientMulti').data("kendoMultiSelect").value();
        if (((selectval.length == 1 && selectval[0] == 40000) || (selectval.length == 0) )&&( $('#ddlRoleType').val() != "Onetime")) {
            $('#pnlBatchExportCCDA #btnDisplayCCDA').addClass('disableAll');
            $('#pnlBatchExportCCDA #btnDownloadCCDA').addClass('disableAll');
        } else {
            $('#pnlBatchExportCCDA #btnDisplayCCDA').removeClass('disableAll');
            $('#pnlBatchExportCCDA #btnDownloadCCDA').removeClass('disableAll');
        }
        if (Batch_ExportCCDA.asdwe > 0) {
            $('#pnlBatchExportCCDA    #txtpatientMulti').data("kendoMultiSelect").value([Batch_ExportCCDA.asdwe]);
            Batch_ExportCCDA.asdwe = 0
        }
        if (selectval.length > 0) {
            $('#pnlBatchExportCCDA #btnDownloadCCDA').removeClass('disableAll');
        } else {
            $('#pnlBatchExportCCDA #btnDownloadCCDA').addClass('disableAll');
        }
    },
    ClearHiddenField: function (Cntrl, HiddenCntrlId) {

        if ($(Cntrl).val() == "") {
            $("#pnlBatchExportCCDA #" + HiddenCntrlId).val("");
        }
    },
    // End added by Zia Mehmood

    // For Notes Component Lookup
    LoadNoteComponentsLookup: function (obj, controlId) {



        Batch_ExportCCDA.LoadNoteComponentsLookup_DBCall().done(function (response) {

            response = JSON.parse(response);
            //utility.DisplayMessages(response.Message, 1);
            if (response.status != false) {

                var ExportCCDA_JSON = JSON.parse(response.ExportCCDA_JSON);
                $('#pnlBatchExportCCDA #ddlNoteComponents').empty();

                $.each(ExportCCDA_JSON, function (i, item) {
                    var ComponentName="";
                    if (item.ComponentName == "DemographicDataElement") { ComponentName = "Demographic Data Element"; }
                    if (item.ComponentName == "ProviderDataElement") { ComponentName = "Provider Data Element"; }
                    if (item.ComponentName == "SocialHx") { ComponentName = "Social Hx"; }
                    if (item.ComponentName == "Assessment") { ComponentName = "Assessment";}
                    if (item.ComponentName == "PlanofTreatment") { ComponentName = "Plan of Treatment"; }
                    if (item.ComponentName == "PlanOfCare") { ComponentName = "Plan Of Care"; }
                    if (item.ComponentName == "CareTeamMembers") { ComponentName = "Care Team Members"; }
                    if (item.ComponentName == "VisitReason") { ComponentName = "Visit Reason"; }
                    if (item.ComponentName == "HealthConcerns") { ComponentName = "Health Concerns"; }
                    if (item.ComponentName == "Vitals") { ComponentName = "Vitals"; }
                    if (item.ComponentName == "ProblemLists") { ComponentName = "Problem Lists";}
                    if (item.ComponentName == "Medications") { ComponentName = "Medications"; }
                    if (item.ComponentName == "Allergies") { ComponentName = "Allergies";}
                    if (item.ComponentName == "Immunization") { ComponentName = "Immunization"; }
                    if (item.ComponentName == "LabResult") { ComponentName = "Lab Result";}
                    if (item.ComponentName == "Procedures") { ComponentName = "Procedures"; }
                    if (item.ComponentName == "MedicalEquipment") { ComponentName = "Medical Equipment"; }
                    if (item.ComponentName == "Refferral") { ComponentName = "Referral" ;}
                    if (item.ComponentName == "FunctionalStatus") { ComponentName = "Functional Status"; }
                    if (item.ComponentName == "EncounterDiagnostic") { ComponentName = "Encounter Diagnostic";}
                    if (item.ComponentName == "PayersSection") { ComponentName = "Payers Section";}
                    if (item.ComponentName == "MentalStatus") { ComponentName = "Mental Status"; }
                    
                    $('#pnlBatchExportCCDA #ddlNoteComponents').append(
                        $('<option/>', {
                            value: item.NoteComponentsLookupId,
                            html: ComponentName,
                            RefVal: item.ComponentName
                        })
                    );
                });
            }
        }).then(function () {
            Batch_ExportCCDA.IntializeMultiSelectDropDownNoteCompoentes();
        });



    },
    IntializeMultiSelectDropDownNoteCompoentes: function () {

        $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlNoteComponents').multiselect('destroy');
        $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlNoteComponents option').prop('selected', true);
        
        $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlNoteComponents').multiselect({
            includeSelectAllOption: false,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: true,
            onChange: function (option, checked, select) {
                //$('#divNoteComponents ul.multiselect-container li:contains("Demographic Data Element")').find('input[type=checkbox]').prop('checked', true);
                //$('#divNoteComponents ul.multiselect-container li:contains("Provider Data Element")').find('input[type=checkbox]').prop('checked', true);
                //var self = $('#pnlBatchExportCCDA');
                //$('#' + Batch_ExportCCDA.params.PanelID + " #ddlYears").multiselect('updateButtonText');
                //var Ids = self.find('#divNoteComponents ul.multiselect-container li input[type=checkbox]:checked').map(function () {
                //    return this.value;
                //}).get().join(',');
                //Batch_ExportCCDA.NoteCompnentsIds = Ids;
                //if (Batch_ExportCCDA.NoteCompnentsIds != '')
                //    Batch_ExportCCDA.validate(2);
                //else
                //    Batch_ExportCCDA.validateYears(1)

            },
            onDropdownHide: function (event) {
                $.when(
                   // Batch_ExportCCDA.filterProvidersByNoteCompnentsIds()
                ).then(function () {
                    //if (Batch_ExportCCDA.ProviderIds != '') {
                    //    var Providers = Batch_ExportCCDA.ProviderIds.split(",");

                    //    if (Providers != '' && typeof Providers != 'undefined') {

                    //        $.each(Providers, function (index, item) {
                    //            Batch_ExportCCDA.providerCheckedIds = Batch_ExportCCDA.removeFromArray(Batch_ExportCCDA.providerCheckedIds, item);
                    //            Batch_ExportCCDA.providerCheckedIds.push(item);
                    //        });
                    //    }
                    //}
                    //$('#' + Batch_ExportCCDA.params.PanelID + ' #ddlPhysicalExamTemplateProvider').val(Batch_ExportCCDA.providerCheckedIds);
                    //Batch_ExportCCDA.IntializeMultiSelectDropDownProviders();
                });
            },

            onDropdownShow: function (event) {
                //make items selected and initialize dropdownlist
                if (Batch_ExportCCDA.NoteCompnentsIds != '') {
                    var spacialties = Batch_ExportCCDA.NoteCompnentsIds.split(",");

                    if (spacialties != '' && typeof spacialties != 'undefined') {

                        $.each(spacialties, function (index, item) {
                            Batch_ExportCCDA.NoteCompnentsCheckedIds = Batch_ExportCCDA.removeFromArray(Batch_ExportCCDA.NoteCompnentsCheckedIds, item);
                            Batch_ExportCCDA.NoteCompnentsCheckedIds.push(item);
                        });
                    }
                }
                // Batch_ExportCCDA.setSpacialtiesByselectedProviderIds();
                $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlNoteComponents').multiselect('select', Batch_ExportCCDA.NoteCompnentsCheckedIds);
            },
        });
    },
    LoadNoteComponentsLookup_DBCall: function () {

        var objData = new Object();


        objData["commandType"] = "FILL_NOTECOMPONENT_LOOKPUP";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Batch_ExportCCDA", "ExportCCDA");
    },
    Get_Scheduled_PatientVisits: function (obj, controlId) {



        Batch_ExportCCDA.Get_Scheduled_PatientVisits_DBCall().done(function (response) {

            response = JSON.parse(response);
            //utility.DisplayMessages(response.Message, 1);
            if (response.status != false) {

                var ExportCCDA_JSON = JSON.parse(response.ExportCCDA_JSON);

            }
        })



    },
    Get_Scheduled_PatientVisits_DBCall: function () {

        var objData = new Object();


        objData["commandType"] = "GET_SCHEDULED_PATIENTVISITS";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Batch_ExportCCDA", "ExportCCDA");
    },
    IntializeMultiSelectDropDownYears: function () {
        GetYears = '[{ "Name": "2018", "Value": "2018"},' +
                    '{ "Name": "2019", "Value": "2019"},' +
                    '{ "Name": "2020", "Value": "2020"},' +
                    '{ "Name": "2021", "Value": "2021"},' +
                    '{ "Name": "2022", "Value": "2022"},' +
                    '{ "Name": "2023", "Value": "2023"},' +
                    '{ "Name": "2024", "Value": "2024"},' +
                    '{ "Name": "2025", "Value": "2025"},' +
                    '{ "Name": "2026", "Value": "2026"},' +
                    '{ "Name": "2027", "Value":"2027"},' +
                    '{ "Name": "2028", "Value":"2028"},' +
                    '{ "Name": "2029", "Value":"2029"},' +
                    '{ "Name": "2030", "Value": "2030"},' +
                    '{ "Name": "2031", "Value": "2031"},' +
                    '{ "Name": "2032", "Value": "2032"},' +
                    '{ "Name": "2033", "Value": "2033"},' +
                    '{ "Name": "2034", "Value": "2034"},' +
                    '{ "Name": "2035", "Value": "2035"},' +
                    '{ "Name": "2036", "Value": "2036"},' +
                    '{ "Name": "2037", "Value": "2037"},' +
                    '{ "Name": "2038", "Value": "2038"},' +
                    '{ "Name": "2039", "Value":"2039"},' +
                    '{ "Name": "2040", "Value":"2040"}]';
        var Years = JSON.parse(GetYears);
        $.when($.each(Years, function (i, item) {
            $('#pnlBatchExportCCDA #ddlYears').append(
                $('<option/>', {
                    value: item.Value,
                    html: item.Name
                })
            );
        })).then(function () {
            $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlYears').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                maxHeight: 247,
                nonSelectedText: 'Select',
                selectAll: false,
                onChange: function (option, checked, select) {
                    var self = $('#pnlBatchExportCCDA');
                    $('#' + Batch_ExportCCDA.params.PanelID + " #ddlYears").multiselect('updateButtonText');
                    var Ids = self.find('#divYears ul.multiselect-container li input[type=checkbox]:checked').map(function () {
                        return this.value;
                    }).get().join(',');
                    Batch_ExportCCDA.YearsIds = Ids;
                    if (Batch_ExportCCDA.YearsIds != '')
                        Batch_ExportCCDA.validateYears(2);
                    else
                        Batch_ExportCCDA.validateYears(1)


                },
                onDropdownHide: function (event) {
                    $.when(

                    ).then(function () {

                    });
                },

                onDropdownShow: function (event) {
                    //make items selected and initialize dropdownlist
                    if (Batch_ExportCCDA.YearsIds != '') {
                        var spacialties = Batch_ExportCCDA.YearsIds.split(",");

                        if (spacialties != '' && typeof spacialties != 'undefined') {

                            $.each(spacialties, function (index, item) {
                                Batch_ExportCCDA.YearsCheckedIds = Batch_ExportCCDA.removeFromArray(Batch_ExportCCDA.YearsCheckedIds, item);
                                Batch_ExportCCDA.YearsCheckedIds.push(item);
                            });
                        }
                    }

                    // $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlTime').multiselect('select', Batch_ExportCCDA.YearsCheckedIds);
                },
            });
        });

        // $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlTime').multiselect('destroy');

    },
    IntializeMultiSelectDropDownMonth: function () {
        GetMonths = '[{ "Name": "January",  "Value": "01"},' +
                    '{ "Name": "February", "Value": "02"},' +
                    '{ "Name": "March", "Value": "03"},' +
                    '{ "Name": "April", "Value": "04"},' +
                    '{ "Name": "May", "Value": "05"},' +
                    '{ "Name": "June", "Value": "06"},' +
                    '{ "Name": "July", "Value": "07"},' +
                    '{ "Name": "August", "Value": "08"},' +
                    '{ "Name": "September", "Value": "09"},' +
                    '{ "Name": "October", "Value": "10"},' +
                    '{ "Name": "November", "Value": "11"},' +
                    '{ "Name": "December", "Value": "12"}]';

        var months = JSON.parse(GetMonths);
        $.when($.each(months, function (i, item) {
            $('#pnlBatchExportCCDA #ddlMonths').append(
                $('<option/>', {
                    value: item.Value,
                    html: item.Name
                })
            );
        })).then(function () {
            $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlMonths').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                maxHeight: 247,
                nonSelectedText: 'Select',
                selectAll: false,
                onChange: function (option, checked, select) {
                    var self = $('#pnlBatchExportCCDA');
                    $('#' + Batch_ExportCCDA.params.PanelID + " #ddlMonths").multiselect('updateButtonText');
                    var MonthsIds = self.find('#divMonths ul.multiselect-container li input[type=checkbox]:checked').map(function () {
                        return this.value;
                    }).get().join(',');
                    Batch_ExportCCDA.MonthsIds = MonthsIds;
                    if (Batch_ExportCCDA.MonthsIds != '')
                        Batch_ExportCCDA.validateMonths(2);
                    else
                        Batch_ExportCCDA.validateMonths(1)

                },
                onDropdownHide: function (event) {
                    $.when(

                    ).then(function () {

                    });
                },

                onDropdownShow: function (event) {
                    //make items selected and initialize dropdownlist
                    if (Batch_ExportCCDA.MonthsIds != '') {
                        var month = Batch_ExportCCDA.MonthsIds.split(",");

                        if (month != '' && typeof month != 'undefined') {

                            $.each(month, function (index, item) {
                                Batch_ExportCCDA.MonthsCheckedIds = Batch_ExportCCDA.removeFromArray(Batch_ExportCCDA.MonthsCheckedIds, item);
                                Batch_ExportCCDA.MonthsCheckedIds.push(item);
                            });
                        }
                    }

                    //   $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlMonths').multiselect('select', Batch_ExportCCDA.MonthsCheckedIds);
                },
            });
        });

        // $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlTime').multiselect('destroy');

    },
    loadEntityProvider: function (entityId) {

        var data = "entityID=" + entityId;
        if (entityId != null && entityId > 0) {

            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlProvider');
              //  var $providerHiddenDdl = $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlProvider');

                //Empty both the providers ddls.
                $providerDdl.empty();
               // $providerHiddenDdl.empty();
                $providerDdl.append(
                                           $('<option/>', {
                                               value: '0',
                                               html: 'Select',
                                               refname: '0',
                                               refvalue: '0'

                                           })
                                       );
                //Loop through all providers loaded from the server
                $.each(options, function (i, item) {
                    if (item.Value != "" && typeof item.Value != 'undefined') {

                        // User will see these providers in multiSelect dropdownlist
                        $providerDdl.append(
                            $('<option/>', {
                                value: item.Value,
                                html: item.Name,
                                refname: item.RefName,
                                refvalue: item.RefValue

                            })
                        );
                        // Populate hidden ddl provider
                        //A Hack to load all the providers in hidden dropdownlist
                        //$providerHiddenDdl.append(
                        //     $('<option/>', {
                        //         value: item.Value,
                        //         html: item.Name,
                        //         refname: item.RefName,
                        //         refvalue: item.RefValue

                        //     })
                        //);
                    }
                });
                // Assigned server side providers to providerCheckedIds array and made selected
                if (Batch_ExportCCDA.ProviderIds != '') {
                    var Providers = Batch_ExportCCDA.ProviderIds.split(",");
                    Batch_ExportCCDA.providerCheckedIds = Providers;
                    $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlProvider').val(Providers);
                }

            }).then(function () {
                // A hack to trigger the onDropDownHide event of Spacialty multiselect      
                $('#' + Batch_ExportCCDA.params.PanelID + ' #divProvider').find('.dropdown-toggle').dropdown('toggle').dropdown('toggle');
                //Intialized in onhidden spacialty ddl.  
                Batch_ExportCCDA.IntializeMultiSelectDropDownProviders();
               
            });
            //enable multiselect
            //Batch_ExportCCDA.enableDisableDropDowLists('ddlPhysicalExamTemplateProvider', false);
        }
        else {
            //disable multiselect
           // Batch_ExportCCDA.enableDisableDropDownLists('ddlProvider', true);
        }
    },
    IntializeMultiSelectDropDownProviders: function () {
        $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlProvider').multiselect('destroy');
        $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'All',
            selectAll: false,
            onChange: function (option, checked, select) {

                
                
                if ( $('#pnlBatchExportCCDA   #ddlProvider').val() != '0') {
                    Batch_ExportCCDA.loadEntityPatients(option.val());
                    $('#pnlBatchExportCCDA   #divpatientMulti').addClass('hidden');
                    $('#pnlBatchExportCCDA   #divPatients').removeClass('hidden');
                    $('#pnlBatchExportCCDA   #btnDownloadCCDA2').removeClass('hidden');
                    $('#pnlBatchExportCCDA   #btnDownloadCCDA').addClass('hidden');
                    Batch_ExportCCDA.validatePatients();
                    //$('#pnlBatchExportCCDA   #btnSave').addClass('disableAll');
                } else {
                    Batch_ExportCCDA.validatePatientMulti();
                    $('#pnlBatchExportCCDA   #divpatientMulti').removeClass('hidden');
                    $('#pnlBatchExportCCDA   #divPatients').addClass('hidden');
                    $('#pnlBatchExportCCDA   #btnDownloadCCDA2').addClass('hidden');
                    $('#pnlBatchExportCCDA   #btnDownloadCCDA').removeClass('hidden');
                    // $('#pnlBatchExportCCDA   #btnSave').removeClass('disableAll');
                    $('#pnlBatchExportCCDA    #txtpatientMulti').data("kendoMultiSelect").value([]);

                }

            },
            onDropdownHide: function (event) {
                // Batch_ExportCCDA.specialitiesByProviderIds();
                //Refresh multiselect
                //  $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect('refresh');
            },


        });
    },
    loadEntityPatients: function (ProviderId, IsDeferred,defferObj) {
        var objData = {};
        objData["ProviderId"] = ProviderId;
        objData["commandType"] = "FILL_PATIENT_LOOKUP";
        var data = JSON.stringify(objData);    
        if (ProviderId != null && ProviderId > 0) {
               
             MDVisionService.APIService(data, "Batch_ExportCCDA", "ExportCCDA").done(function (result) {
                 result = JSON.parse(result);
                 var options = [];
                 if (result.ExportCCDACount > 0) {
                      options = JSON.parse(result.ExportCCDA_JSON);
                      }
                     
                     var $PatientsDdl = $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlPatients');
                     //  var $providerHiddenDdl = $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlProvider');

                     //Empty both the providers ddls.
                     $PatientsDdl.empty();
                     // $providerHiddenDdl.empty();

                     //Loop through all providers loaded from the server     item.PatientId, value: item.PatientFullName
                     $.each(options, function (i, item) {
                         if (item.PatientId != "" && typeof item.PatientId != 'undefined') {

                             // User will see these providers in multiSelect dropdownlist
                             $PatientsDdl.append(
                                 $('<option/>', {
                                     value: item.PatientId,
                                     html: item.PatientFullName,
                                     refname: item.RefName,
                                     refvalue: item.RefValue

                                 })
                             );
                             // Populate hidden ddl provider
                             //A Hack to load all the providers in hidden dropdownlist
                             //$providerHiddenDdl.append(
                             //     $('<option/>', {
                             //         value: item.Value,
                             //         html: item.Name,
                             //         refname: item.RefName,
                             //         refvalue: item.RefValue

                             //     })
                             //);
                         }
                     });
                     // Assigned server side providers to providerCheckedIds array and made selected
                     if (Batch_ExportCCDA.ProviderIds != '') {
                         var Providers = Batch_ExportCCDA.ProviderIds.split(",");
                         Batch_ExportCCDA.providerCheckedIds = Providers;
                         $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlPatients').val(Providers);
                     }
                
            }).then(function () {
                // A hack to trigger the onDropDownHide event of Spacialty multiselect      
                $('#' + Batch_ExportCCDA.params.PanelID + ' #divPatients').find('.dropdown-toggle').dropdown('toggle').dropdown('toggle');
                //Intialized in onhidden spacialty ddl.  
                Batch_ExportCCDA.IntializeMultiSelectDropDownPatients(IsDeferred,defferObj);

            });
            //enable multiselect
            //Batch_ExportCCDA.enableDisableDropDowLists('ddlPhysicalExamTemplateProvider', false);
        }
        else {
            //disable multiselect
            // Batch_ExportCCDA.enableDisableDropDownLists('ddlProvider', true);
        }
    },
    IntializeMultiSelectDropDownPatients: function (IsDeferred, defferObj) {
        $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlPatients').multiselect('destroy');
        $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlPatients').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'All',
            selectAll: false,
            onChange: function (option, checked, select) {
                if ($('#pnlBatchExportCCDA   #ddlPatients').val())
                {
                    Batch_ExportCCDA.validatePatients(2);
                }
                else {
                    Batch_ExportCCDA.validatePatients(1);
                }
            },
            onDropdownHide: function (event) {
                // Batch_ExportCCDA.specialitiesByProviderIds();
                //Refresh multiselect
                //  $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect('refresh');
            },


        });
        if (IsDeferred == true)
        {
            defferObj.resolve();
        }
    },

    removeFromArray: function (array, removeItem) {

        var resultantArray = jQuery.grep(array, function (item) {
            return item != removeItem;
        });
        return resultantArray;
    },
    IntializeMultiSelectDropDownWeeks: function () {
        GetWeeks = '[{ "Name": "1",  "Value": "1"},' +
                    '{ "Name": "2", "Value": "2"},' +
                    '{ "Name": "3", "Value": "3"},' +
                    '{ "Name": "4", "Value": "4"},' +
                    '{ "Name": "5", "Value": "5"}]';


        var Weeks = JSON.parse(GetWeeks);
        $.when($.each(Weeks, function (i, item) {
            $('#pnlBatchExportCCDA #ddlWeeks').append(
                $('<option/>', {
                    value: item.Value,
                    html: item.Name
                })
            );
        })).then(function () {
            $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlWeeks').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                maxHeight: 247,
                nonSelectedText: 'Select',
                selectAll: false,
                onChange: function (option, checked, select) {
                    $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlDays').multiselect('clearSelection', true);
                    $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlDays').multiselect('updateButtonText');
                    Batch_ExportCCDA.IntializeMultiSelectDropDownDays();


                    // $('#' + Batch_ExportCCDA.params.PanelID + " #ddlDays").val(scheduler_Jason[0].Days.split(','));
                    $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlDays').multiselect("refresh");
                    //var self = $('#pnlBatchExportCCDA');
                    //$('#' + Batch_ExportCCDA.params.PanelID + " #ddlWeeks").multiselect('updateButtonText');
                    //var Ids = self.find('#divWeeks ul.multiselect-container li input[type=checkbox]:checked').map(function () {
                    //    return this.value;
                    //}).get().join(',');
                    //Batch_ExportCCDA.WeeksIds = Ids;
                    //if (Batch_ExportCCDA.WeeksIds != '')
                    //    Batch_ExportCCDA.validateWeeks(2);
                    //else
                    //    Batch_ExportCCDA.validateWeeks(1)
                },
                onDropdownHide: function (event) {
                    $.when(

                    ).then(function () {

                    });
                },

                onDropdownShow: function (event) {
                    //make items selected and initialize dropdownlist
                    if (Batch_ExportCCDA.WeeksIds != '') {
                        var month = Batch_ExportCCDA.WeeksIds.split(",");

                        if (month != '' && typeof month != 'undefined') {

                            $.each(month, function (index, item) {
                                Batch_ExportCCDA.WeeksCheckedIds = Batch_ExportCCDA.removeFromArray(Batch_ExportCCDA.WeeksCheckedIds, item);
                                Batch_ExportCCDA.WeeksCheckedIds.push(item);
                            });
                        }
                    }

                    //  $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlWeeks').multiselect('select', Batch_ExportCCDA.WeeksCheckedIds);
                },
            });
        });

        // $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlTime').multiselect('destroy');

    },
    IntializeMultiSelectDropDownDays: function () {
        $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlDays ').multiselect('destroy');
        $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlDays option').remove();
        var GetDays;
        if ($('#' + Batch_ExportCCDA.params.PanelID + ' #ddlWeeks').val() == null) {

            GetDays = '[{ "Name": "1",  "Value": "01"},' +
                    '{ "Name": "2", "Value": "02"},' +
                    '{ "Name": "3", "Value": "03"},' +
                    '{ "Name": "4", "Value": "04"},' +
                    '{ "Name": "5", "Value": "05"},' +
                    '{ "Name": "6", "Value": "06"},' +
                    '{ "Name": "7", "Value": "07"},' +
                    '{ "Name": "8", "Value": "08"},' +
                    '{ "Name": "9", "Value": "09"},' +
                    '{ "Name": "10", "Value": "10"},' +
                    '{ "Name": "11", "Value": "11"},' +
                    '{ "Name": "12", "Value": "12"},' +
                    '{ "Name": "13", "Value": "13"},' +
                    '{ "Name": "14", "Value": "14"},' +
                    '{ "Name": "15", "Value": "15"},' +
                    '{ "Name": "16", "Value": "16"},' +
                    '{ "Name": "17", "Value": "17"},' +
                    '{ "Name": "18", "Value": "18"},' +
                    '{ "Name": "19", "Value": "19"},' +
                    '{ "Name": "20", "Value": "20"},' +
                    '{ "Name": "21", "Value": "21"},' +
                    '{ "Name": "22", "Value": "22"},' +
                    '{ "Name": "23", "Value": "23"},' +
                    '{ "Name": "24", "Value": "24"},' +
                    '{ "Name": "25", "Value": "25"},' +
                    '{ "Name": "26", "Value": "26"},' +
                    '{ "Name": "27", "Value": "27"},' +
                    '{ "Name": "28", "Value": "28"},' +
                    '{ "Name": "29", "Value": "29"},' +
                    '{ "Name": "30", "Value": "30"},' +
                    '{ "Name": "31", "Value": "31"}]';

        } else {

            GetDays = '[{ "Name": "1",  "Value": "1"},' +
                        '{ "Name": "2", "Value": "2"},' +
                        '{ "Name": "3", "Value": "3"},' +
                        '{ "Name": "4", "Value": "4"},' +
                        '{ "Name": "5", "Value": "5"},' +
                        '{ "Name": "6", "Value": "6"},' +
                        '{ "Name": "7", "Value": "7"}]';
        }





        var Days = JSON.parse(GetDays);
        $.when($.each(Days, function (i, item) {
            $('#pnlBatchExportCCDA #ddlDays').append(
                $('<option/>', {
                    value: item.Value,
                    html: item.Name
                })
            );
        })).then(function () {
            $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlDays').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                maxHeight: 247,
                nonSelectedText: 'Select',
                selectAll: false,
                onChange: function (option, checked, select) {

                    var self = $('#pnlBatchExportCCDA');
                    $('#' + Batch_ExportCCDA.params.PanelID + " #ddlDays").multiselect('updateButtonText');
                    var Ids = self.find('#divDays ul.multiselect-container li input[type=checkbox]:checked').map(function () {
                        return this.value;
                    }).get().join(',');
                    Batch_ExportCCDA.DaysIds = Ids;
                    if (Batch_ExportCCDA.DaysIds != '')
                        Batch_ExportCCDA.validateDays(2);
                    else
                        Batch_ExportCCDA.validateDays(1)

                },
                onDropdownHide: function (event) {
                    $.when(

                    ).then(function () {

                    });
                },

                onDropdownShow: function (event) {
                    //make items selected and initialize dropdownlist
                    if (Batch_ExportCCDA.DaysIds != '') {
                        var month = Batch_ExportCCDA.DaysIds.split(",");

                        if (month != '' && typeof month != 'undefined') {

                            $.each(month, function (index, item) {
                                Batch_ExportCCDA.DaysCheckedIds = Batch_ExportCCDA.removeFromArray(Batch_ExportCCDA.DaysCheckedIds, item);
                                Batch_ExportCCDA.DaysCheckedIds.push(item);
                            });
                        }
                    }

                    // $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlDays').multiselect('select', Batch_ExportCCDA.DaysCheckedIds);
                },
            });
        });

        // $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlTime').multiselect('destroy');

    },
    // for save CCDA Schedule
    SaveCCDASchedule: function () {
        var self = null;
        self = $('#' + Batch_ExportCCDA.params.PanelID);
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objData = JSON.parse(myJSON);
        if ((Batch_ExportCCDA.MonthsIds != '' && Batch_ExportCCDA.DaysIds != '' && Batch_ExportCCDA.YearsIds != '') || ($('#' + Batch_ExportCCDA.params.PanelID + ' #ddlRoleType ').val() == "Onetime")) {
            Batch_ExportCCDA.validateYears(2);
            Batch_ExportCCDA.validateMonths(2);
            Batch_ExportCCDA.validateDays(2);
            Batch_ExportCCDA.SaveCCDASchedule_DBCall(objData).done(function (response) {
                if (response != null && response != "") {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        Batch_ExportCCDA.LoadCCDASchedule();
                        Batch_ExportCCDA.ClearValues();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else {
            if (Batch_ExportCCDA.MonthsIds == '') {
                Batch_ExportCCDA.validateMonths(1);
            }
            else if (Batch_ExportCCDA.DaysIds == '') {
                Batch_ExportCCDA.validateDays(1);
            }
            else if (Batch_ExportCCDA.YearsIds == '') {
                Batch_ExportCCDA.validateYears(1);
            }
        }
    },
    SaveCCDASchedule_DBCall: function (objData) {
        if (objData.RuleType == "Onetime"){
            var time = $("#Time").val();
            var hours = Number(time.match(/^(\d+)/)[1]);
            var minutes = Number(time.match(/:(\d+)/)[1]);
            var AMPM = time.match(/\s(.*)$/)[1];
            if (AMPM == "PM" && hours < 12) hours = hours + 12;
            if (AMPM == "AM" && hours == 12) hours = hours - 12;
            var sHours = hours.toString();
            var sMinutes = minutes.toString();
            if (hours < 10) sHours = "0" + sHours;
            if (minutes < 10) sMinutes = "0" + sMinutes;

            objData["SchedulerHour"] = sHours;
        }
        else {
            var time = $("#txtMultiTime").val();
            var hours = Number(time.match(/^(\d+)/)[1]);
            var minutes = Number(time.match(/:(\d+)/)[1]);
            var AMPM = time.match(/\s(.*)$/)[1];
            if (AMPM == "PM" && hours < 12) hours = hours + 12;
            if (AMPM == "AM" && hours == 12) hours = hours - 12;
            var sHours = hours.toString();
            var sMinutes = minutes.toString();
            if (hours < 10) sHours = "0" + sHours;
            if (minutes < 10) sMinutes = "0" + sMinutes;
            // alert(sHours + ":" + sMinutes);

            var time2 = $("#txtMultiTime2").val();
            var hours2 = Number(time2.match(/^(\d+)/)[1]);
            var minutes2 = Number(time2.match(/:(\d+)/)[1]);
            var AMPM2 = time2.match(/\s(.*)$/)[1];
            if (AMPM2 == "PM" && hours2 < 12) hours2 = hours2 + 12;
            if (AMPM2 == "AM" && hours2 == 12) hours2 = hours2 - 12;
            var sHours2 = hours2.toString();
            var sMinutes2 = minutes2.toString();
            if (hours2 < 10) sHours2 = "0" + sHours2;
            if (minutes2 < 10) sMinutes2 = "0" + sMinutes2;
            // alert(sHours2 + ":" + sMinutes2);
            if ($("#demo").hasClass('in')) {
                objData.MultiTime = $("#txtMultiTime").val() + ',' + $("#txtMultiTime2").val();
                objData["SchedulerHour"] = sHours + ',' + sHours2;
            } else {

                objData.MultiTime = $("#txtMultiTime").val();
                objData["SchedulerHour"] = sHours;
            }
        }

        //var arry = objData["MultiTime"].split(',');
        //$.each(arry, function (j, item) {
        //    var time = {
        //        ExportTime:item,
        //    }
        //    Batch_ExportCCDA.TimeCheckedIds.push(time);
        //})

        //var arry = objData["Months"].split(',');
        //$.each(arry, function (j, item) {
        //    var Month = {
        //        Months: item,
        //    }
        //    Batch_ExportCCDA.MonthsCheckedIds.push(Month);
        //})

        //var arry = objData["Weeks"].split(',');
        //$.each(arry, function (j, item) {
        //    var Week = {
        //        Weeks: item,
        //    }
        //    Batch_ExportCCDA.WeeksCheckedIds.push(Week);
        //})

        //var arry = objData["Days"].split(',');
        //$.each(arry, function (j, item) {
        //    var Day = {
        //        Days: item,
        //    }
        //    Batch_ExportCCDA.DaysCheckedIds.push(Day);
        //})
        //var arry = objData["NoteComponents"].split(',');
        //$.each(arry, function (j, item) {
        //    var NoteComponent = {
        //        NoteComponents: item,
        //    }
        //    Batch_ExportCCDA.NoteCompnentsCheckedIds.push(NoteComponent);
        //})
        if ($('#' + Batch_ExportCCDA.params.PanelID + ' #ddlNoteComponents ').val() == null) {
            objData["IsComponentsAll"] = "1";
        }
        if (Batch_ExportCCDA.SchedulerId != "" && Batch_ExportCCDA.IsUpdate == true) {
            objData.SchedulerId = Batch_ExportCCDA.SchedulerId;
        }
        if (objData.RuleType == "Onetime") {
            objData.Years = objData.DateExport.slice(6, 10);
            objData.Months = objData.DateExport.slice(0, 2);
            objData.Weeks = "";
            objData.Days = objData.DateExport.slice(3, 5);
            objData.MultiTime = objData.Time;
            //objData.DateExport = "";
        } else {
            objData.Time = "";
            objData.DateExport = "";
        }

        //var patientIds = $('#patientMulti').text().slice($('#patientMulti').text().lastIndexOf('delete') + 6);
        //var arrypatientIds = patientIds.split(',');
        //arrypatientIds.splice(arrypatientIds.length - 1,1);
        var Patients = "";
        var arrypatientIds = null;
        if ($('#pnlBatchExportCCDA   #ddlProvider').val() == "0") {
             arrypatientIds = $('#pnlBatchExportCCDA    #txtpatientMulti').data("kendoMultiSelect").value();
        } else {
             arrypatientIds = $('#pnlBatchExportCCDA   #ddlPatients').val();
            objData["ProviderId"]=$('#pnlBatchExportCCDA   #ddlProvider').val();
        }
        $.each(arrypatientIds, function (j, item) {
            if (j == 0) {
                Patients = item;
            } else {
                Patients = Patients + ',' + item;
            }
            //  Batch_ExportCCDA.Patients.push(Patients);
        })
        //objData["Patients"] = Batch_ExportCCDA.Patients;
        //objData["TimeCheckedIds"] = Batch_ExportCCDA.TimeCheckedIds;
        //objData["MonthsCheckedIds"] = Batch_ExportCCDA.MonthsCheckedIds;
        //objData["WeeksCheckedIds"] = Batch_ExportCCDA.WeeksCheckedIds;
        //objData["DaysCheckedIds"] = Batch_ExportCCDA.DaysCheckedIds;
        //objData["NoteComponents"] = Batch_ExportCCDA.NoteCompnentsCheckedIds;
        if ($('#pnlBatchExportCCDA #hfIspatientAll').val() == "1") {
            Patients = "";
        }
        objData["IsPatientsAll"] = $('#pnlBatchExportCCDA #hfIspatientAll').val();
        objData["IsActive"] = 1;
        objData["SecPatient"] = Patients;
        objData["commandType"] = "SAVE_CCDASCHEDULE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Batch_ExportCCDA", "ExportCCDA");
    },

    DeleteCCDASchedule_DBCall: function (SchedulerId) {
        var objData = {};
        objData["SchedulerId"] = SchedulerId;
        objData["commandType"] = "DELETE_CCDA_SCHEDULE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Batch_ExportCCDA", "ExportCCDA");
    },
    CCDASchedule_ActiveInactive_DBCall: function (SchedulerId, IsActive) {
        var objData = {};
        objData["IsActive"] = IsActive;
        objData["SchedulerId"] = SchedulerId;
        objData["commandType"] = "ACTIVE_INACTIVE_CCDA_SCHEDULE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Batch_ExportCCDA", "ExportCCDA");
    },
    LoadCCDASchedule: function (SchedulerId, pageNumber, rowsPerPage, IsActive) {
        Batch_ExportCCDA.LoadCCDASchedule_DBCall(SchedulerId, pageNumber, rowsPerPage, IsActive).done(function (response) {
            if (response != null && response != "") {
                response = JSON.parse(response);
                if (response.status != false) {

                    Batch_ExportCCDA.CCDAScheduleGridLoad(response);
                    //server side pagination
                    var TableControl = Batch_ExportCCDA.params.PanelID + " #dgvAuditReport";
                    var PagingPanelControlID = Batch_ExportCCDA.params.PanelID + " #dgvCCDAExport_Paging";
                    var ClassControlName = "Batch_ExportCCDA";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout(CreatePagination(response.ExportCCDACount, pageNumber, 15, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, pageNumber, resultPerPage) {
                        Batch_ExportCCDA.LoadCCDASchedule(PrimaryID, pageNumber, resultPerPage);
                    }), 10);

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    LoadCCDASchedule_DBCall: function (SchedulerId, pageNumber, rowsPerPage, IsActive) {
        var objData = {};
        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 15;
        }
        if (IsActive == null) {
            IsActive = "1";
        }
        objData["IsActive"] = IsActive;
        objData["PageNumber"] = pageNumber;
        objData["RowsPerPage"] = rowsPerPage;
        objData["commandType"] = "LOAD_CCDA_SCHEDULE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Batch_ExportCCDA", "ExportCCDA");


    },
    SelectCCDASchedule: function (SchedulerId) {
        Batch_ExportCCDA.ClearValues();
        Batch_ExportCCDA.SelectCCDASchedule_DBCall(SchedulerId).done(function (response) {
            if (response != null && response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    var scheduler_Jason = JSON.parse(response.ExportCCDA_JSON);
                    // Batch_ExportCCDA.ClearValues();
                    // get the value of the multiselect.
                    //var value = multiselect.value();
                    if (scheduler_Jason[0].SecPatient != "") {
                        // $('#pnlBatchExportCCDA    #txtpatientMulti').kendoMultiSelect();
                        if (Batch_ExportCCDA.IsCreated == false) {
                            $('#pnlBatchExportCCDA    #txtpatientMulti').trigger('oninput');
                            Batch_ExportCCDA.IsCreated = true;
                        }
                        //var multiselect = $('#pnlBatchExportCCDA    #txtpatientMulti').data("kendoMultiSelect");
                        var patient = scheduler_Jason[0].SecPatient.split(',');
                        // var pateintobj = { data: [] };
                        var selectedval = "";
                        $.each(patient, function (i, item) {
                            // pateintobj.data.push({ id: item.slice(1, item.lastIndexOf(':')), value: item.slice(item.lastIndexOf(':') + 1, -1) });
                            $('#pnlBatchExportCCDA    #txtpatientMulti').data("kendoMultiSelect").dataSource.insert(0, {
                                id: item.slice(0, item.lastIndexOf(':')), value: item.slice(item.lastIndexOf(':') + 1, -1)
                            })
                            // $('#pnlBatchExportCCDA    #txtpatientMulti').data("kendoMultiSelect").value(item.slice(1, item.lastIndexOf(':')));//.insert(0, { id: item.slice(1, item.lastIndexOf(':')), value: item.slice(item.lastIndexOf(':') + 1, -1) })
                            if (i == 0) {
                                selectedval = item.slice(0, item.lastIndexOf(':'));
                            } else {

                                selectedval = selectedval + ',' + item.slice(0, item.lastIndexOf(':'));
                            }

                            //selectedval.push(item.slice(1, item.lastIndexOf(':')));
                        });
                        var temp = selectedval.split(",");
                        if (scheduler_Jason[0].ProviderId != "") {
                            $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlProvider').multiselect('clearSelection', false);
                            $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlProvider').multiselect('updateButtonText');
                            $('#' + Batch_ExportCCDA.params.PanelID + " #ddlProvider").val(scheduler_Jason[0].ProviderId);
                            $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlProvider').multiselect("refresh");
                            defferObj = $.Deferred();
                            Batch_ExportCCDA.loadEntityPatients(scheduler_Jason[0].ProviderId,true,defferObj);
                            $.when(defferObj).then(function () {
                                $('#pnlBatchExportCCDA   #divpatientMulti').addClass('hidden');
                                $('#pnlBatchExportCCDA   #divPatients').removeClass('hidden');
                                $('#pnlBatchExportCCDA   #btnDownloadCCDA2').removeClass('hidden');
                                $('#pnlBatchExportCCDA   #btnDownloadCCDA').addClass('hidden');
                                Batch_ExportCCDA.validatePatients();
                                $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlPatients').multiselect('clearSelection', false);
                                $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlPatients').multiselect('updateButtonText');
                                $('#' + Batch_ExportCCDA.params.PanelID + " #ddlPatients").val(temp);
                                $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlPatients').multiselect("refresh");
                                Batch_ExportCCDA.validatePatients(2);
                            });

                           

                        } else {

                            // setTimeout(function(){$('#pnlBatchExportCCDA    #txtpatientMulti').data("kendoMultiSelect").value([temp])},3000)
                            // $('#pnlBatchExportCCDA    #txtpatientMulti').data("kendoMultiSelect").refresh();
                            $('#pnlBatchExportCCDA    #txtpatientMulti').data("kendoMultiSelect").value([]);
                            //setTimeout(function(){

                            $('#pnlBatchExportCCDA    #txtpatientMulti').data("kendoMultiSelect").value(temp);
                            //},1000);

                            //$('#pnlBatchExportCCDA    #txtpatientMulti').data("kendoMultiSelect").trigger('change');
                            //var newsoure = new kendo.data.DataSource(dataSource);
                            // multiselect.dataSource.data(pateintobj);
                            // multiselect.dataSource.insert(0, pateintobj)
                            // $('#pnlBatchExportCCDA    #txtpatientMulti').trigger('oninput');
                            //  multiselect.value(pateintobj);
                        }
                    } else if (scheduler_Jason[0].IsPatientsAll) {
                        if (Batch_ExportCCDA.IsCreated == false) {
                            $('#pnlBatchExportCCDA    #txtpatientMulti').trigger('oninput');
                            Batch_ExportCCDA.IsCreated = true;
                        }
                        $('#pnlBatchExportCCDA    #txtpatientMulti').data("kendoMultiSelect").value([]);
                        $('#pnlBatchExportCCDA    #txtpatientMulti').data("kendoMultiSelect").value(40000);
                    }
                    //$("#txtpatientMulti").kendoMultiSelect({
                    //    dataSource: ["Item1", "Item2", "Item3", "Item4"],
                    //    value: ["Item2", "Item3"]
                    //});
                    Batch_ExportCCDA.NoteCompnentsIds = scheduler_Jason[0].NoteComponents;
                    Batch_ExportCCDA.MonthsIds = scheduler_Jason[0].Months;
                    Batch_ExportCCDA.WeeksIds = scheduler_Jason[0].Weeks;
                    Batch_ExportCCDA.DaysIds = scheduler_Jason[0].Days;
                    Batch_ExportCCDA.YearsIds = scheduler_Jason[0].Years;
                    $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlRoleType').val(scheduler_Jason[0].RuleType);
                    //$('#' + Batch_ExportCCDA.params.PanelID + ' #ddlYears').val(scheduler_Jason[0].Years);
                    Batch_ExportCCDA.HideShowOnceOrReoccuring();
                    $('#' + Batch_ExportCCDA.params.PanelID + ' #dtpDateTo').val("");
                    $('#' + Batch_ExportCCDA.params.PanelID + ' #txtFilePath').val(scheduler_Jason[0].FilePath);
                    $('#' + Batch_ExportCCDA.params.PanelID + ' #dtpDateExport').datepicker("setDate", scheduler_Jason[0].DateExport.replace(" 12:00:00 AM", ""));
                    $('#' + Batch_ExportCCDA.params.PanelID + ' #dtpDateFrom').datepicker("setDate", scheduler_Jason[0].DateFrom.replace(" 12:00:00 AM", "").replace("1/1/1900", ""));
                    $('#' + Batch_ExportCCDA.params.PanelID + ' #dtpDateTo').datepicker("setDate", scheduler_Jason[0].DateTo.replace(" 12:00:00 AM", "").replace("1/1/1900", ""));
                    $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlMonths').multiselect('clearSelection', false);
                    $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlMonths').multiselect('updateButtonText');
                    $('#' + Batch_ExportCCDA.params.PanelID + " #ddlMonths").val(scheduler_Jason[0].Months.split(','));
                    $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlMonths').multiselect("refresh");
                    $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlWeeks').multiselect('clearSelection', false);
                    $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlWeeks').multiselect('updateButtonText');
                    $('#' + Batch_ExportCCDA.params.PanelID + " #ddlWeeks").val(scheduler_Jason[0].Weeks.split(','));
                    $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlWeeks').multiselect("refresh");
                    Batch_ExportCCDA.IntializeMultiSelectDropDownDays();
                    $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlDays').multiselect('clearSelection', false);
                    $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlDays').multiselect('updateButtonText');
                    $('#' + Batch_ExportCCDA.params.PanelID + " #ddlDays").val(scheduler_Jason[0].Days.split(','));
                    $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlDays').multiselect("refresh");
                    $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlNoteComponents').multiselect('clearSelection', false);
                    $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlNoteComponents').multiselect('updateButtonText');
                    $('#' + Batch_ExportCCDA.params.PanelID + " #ddlNoteComponents").val(scheduler_Jason[0].NoteComponents.split(','));
                    $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlNoteComponents').multiselect("refresh");

                    $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlYears').multiselect('clearSelection', false);
                    $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlYears').multiselect('updateButtonText');
                    $('#' + Batch_ExportCCDA.params.PanelID + " #ddlYears").val(scheduler_Jason[0].Years.split(','));
                    $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlYears').multiselect("refresh");

                    Batch_ExportCCDA.SchedulerId = SchedulerId;
                    Batch_ExportCCDA.IsUpdate = true;
                    Batch_ExportCCDA.validateMonths();
                    Batch_ExportCCDA.validateDays();
                    // Batch_ExportCCDA.validateWeeks();
                    Batch_ExportCCDA.validateYears();
                    //Batch_ExportCCDA.validateNoteComponents();
                    if (scheduler_Jason[0].RuleType == "Onetime") {
                        $('#' + Batch_ExportCCDA.params.PanelID + ' #Time').timepicker("setTime", scheduler_Jason[0].MultiTime);
                        $('#pnlBatchExportCCDA #btnDownloadCCDA').removeClass('disableAll');
                    }
                    else {
                        if (scheduler_Jason[0].MultiTime != "") {
                            var multime = scheduler_Jason[0].MultiTime.split(',');
                            if (multime.length == 1) {
                                $("#hrfToggle i").addClass("fa-plus-square");
                                $("#hrfToggle i").removeClass("fa-minus-square");
                                $("#demo").removeClass('in');
                                $('#' + Batch_ExportCCDA.params.PanelID + ' #txtMultiTime').timepicker("setTime", multime[0]);
                            } else if (multime.length == 2) {
                                $("#hrfToggle i").removeClass("fa-plus-square");
                                $("#hrfToggle i").addClass("fa-minus-square");
                                $("#demo").addClass('in');
                                $('#' + Batch_ExportCCDA.params.PanelID + ' #txtMultiTime').timepicker("setTime", multime[0]);
                                $('#' + Batch_ExportCCDA.params.PanelID + ' #txtMultiTime2').timepicker("setTime", multime[1]);
                            }
                        }
                    }
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    TooglePlusMinus: function () {
        if (!$("#demo").hasClass('in')) {

            $("#hrfToggle i").removeClass("fa-plus-square");
            $("#hrfToggle i").addClass("fa-minus-square");
        } else {
            $("#hrfToggle i").removeClass("fa-minus-square");
            $("#hrfToggle i").addClass("fa-plus-square");

        }
    },
    SelectCCDASchedule_DBCall: function (SchedulerId) {
        var objData = {};
        objData["PageNumber"] = 1;
        objData["RowsPerPage"] = 15;
        objData["SchedulerId"] = SchedulerId;
        objData["commandType"] = "LOAD_CCDA_SCHEDULE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Batch_ExportCCDA", "ExportCCDA");
    },
    ClearValues: function () {
       // $('#pnlBatchExportCCDA #btnDownloadCCDA').addClass('disableAll');
        Batch_ExportCCDA.loadEntityProvider(globalAppdata["SeletedEntityId"]);
        $('#pnlBatchExportCCDA   #divpatientMulti').removeClass('hidden');
        $('#pnlBatchExportCCDA   #divPatients').addClass('hidden');
        $('#pnlBatchExportCCDA   #btnDownloadCCDA2').addClass('hidden');
        $('#pnlBatchExportCCDA   #btnDownloadCCDA').removeClass('hidden');
        $('#pnlBatchExportCCDA   #btnSave').removeClass('disableAll');
        $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlRoleType').val('Onetime');
        $('#pnlBatchExportCCDA     #divYears i ').removeClass('glyphicon-ok');
        $('#pnlBatchExportCCDA     #divYears').removeClass('has-success');
        $('#pnlBatchExportCCDA    #divOnetime #divdateexport i ').removeClass('glyphicon-ok');
        $('#pnlBatchExportCCDA    #divOnetime #divdateexport').removeClass('has-success');
        $('#pnlBatchExportCCDA    #divOnetime #divTimeOneTime i ').removeClass('glyphicon-ok');
        $('#pnlBatchExportCCDA    #divOnetime #divTimeOneTime').removeClass('has-success');
        $('#pnlBatchExportCCDA     #divpatientMulti i ').removeClass('glyphicon-ok');
        $('#pnlBatchExportCCDA     #divpatientMulti  ').removeClass('has-success');
        $('#pnlBatchExportCCDA     #divFilePath i ').removeClass('glyphicon-ok');
        $('#pnlBatchExportCCDA     #divFilePath  ').removeClass('has-success');
        $('#pnlBatchExportCCDA     #divFilePath i ').removeClass('glyphicon-remove');
        $('#pnlBatchExportCCDA     #divFilePath  ').removeClass('has-error');
        Batch_ExportCCDA.validatePatients();
        Batch_ExportCCDA.validatePatientMulti();
        $('#pnlBatchExportCCDA  #dtpDateTo').datepicker('setDate', new Date(), 'mm/dd/yyyy');
        $('#pnlBatchExportCCDA  #dtpDateFrom').datepicker('setDate', new Date(), 'mm/dd/yyyy');
        $('#pnlBatchExportCCDA #hfIspatientAll').val('0');
        $('#pnlBatchExportCCDA #btnDownloadCCDA').addClass('disableAll');
        Batch_ExportCCDA.NoteCompnentsCheckedIds = [];
        Batch_ExportCCDA.TimeCheckedIds = [];
        Batch_ExportCCDA.asdwe = 0;
        Batch_ExportCCDA.MonthsCheckedIds = [];

        Batch_ExportCCDA.WeeksCheckedIds = [];
        Batch_ExportCCDA.DaysCheckedIds = [];

        Batch_ExportCCDA.NoteCompnentsIds = '';
        Batch_ExportCCDA.MonthsIds = '';
        Batch_ExportCCDA.WeeksIds = '';
        Batch_ExportCCDA.DaysIds = '';
        Batch_ExportCCDA.TimeIds = '';
        Batch_ExportCCDA.SchedulerId = '';
        $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlMonths').multiselect('updateButtonText');
        $('#' + Batch_ExportCCDA.params.PanelID + " #ddlMonths").val(null);
        $('#' + Batch_ExportCCDA.params.PanelID + " #txtFilePath").val('CCDA_ScheduledFiles');
        $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlMonths').multiselect("refresh");
        $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlWeeks').multiselect('clearSelection', false);
        $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlWeeks').multiselect('updateButtonText');
        $('#' + Batch_ExportCCDA.params.PanelID + " #ddlWeeks").val(null);
        $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlWeeks').multiselect("refresh");
        Batch_ExportCCDA.IntializeMultiSelectDropDownDays();
        $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlDays').multiselect('clearSelection', false);
        $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlDays').multiselect('updateButtonText');
        $('#' + Batch_ExportCCDA.params.PanelID + " #ddlDays").val(null);
        $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlDays').multiselect("refresh");
        $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlYears').multiselect('clearSelection', false);
        $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlYears').multiselect('updateButtonText');
        $('#' + Batch_ExportCCDA.params.PanelID + " #ddlYears").val(null);
        $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlYears').multiselect("refresh");
        $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlNoteComponents').multiselect('clearSelection', false);
        $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlNoteComponents').multiselect('updateButtonText');
        $('#' + Batch_ExportCCDA.params.PanelID + " #ddlNoteComponents").val("");
        $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlNoteComponents').multiselect("refresh");
        Batch_ExportCCDA.validateYears();
        Batch_ExportCCDA.validateMonths();
        Batch_ExportCCDA.validateDays();
        $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlYears').val("");
        $('#pnlBatchExportCCDA  #Time').timepicker('setTime', new Date());
        //$('#' + Batch_ExportCCDA.params.PanelID + ' #dtpDateExport').val('');
        //$('#' + Batch_ExportCCDA.params.PanelID + ' #dtpDateExport').datepicker('setDate', new Date(), 'mm/dd/yyyy');

        utility.CreateDatePicker("pnlBatchExportCCDA #dtpDateExport", function () {

            if ($('#pnlBatchExportCCDA').data("bootstrapValidator") != null) {
                $('#pnlBatchExportCCDA').bootstrapValidator('revalidateField', 'DateExport');
            }
        }, true);

        //$('#pnlBatchExportCCDA ').bootstrapValidator('revalidateField', 'DateExport');
        //$('#pnlBatchExportCCDA ').bootstrapValidator('revalidateField', 'Years');
        //$('#pnlBatchExportCCDA ').bootstrapValidator('revalidateField', 'DateFrom');
        //$('#pnlBatchExportCCDA ').bootstrapValidator('revalidateField', 'DateTo');
        //$('#pnlBatchExportCCDA ').bootstrapValidator('revalidateField', 'Time');
        ////$("#pnlBatchExportCCDA  #divdatefrom .multiselect").css("border-color", "#ccc");
        //$("#pnlBatchExportCCDA  #divdatefrom").find(".control-label").css("color", "#000000");

        //$("#pnlBatchExportCCDA  #divdateto .multiselect").css("border-color", "#ccc");
        //$("#pnlBatchExportCCDA  #divdateto").find(".control-label").css("color", "#000000");

        //$("#pnlBatchExportCCDA  #divYears .multiselect").css("border-color", "#ccc");
        //$("#pnlBatchExportCCDA  #divYears").find(".control-label").css("color", "#000000");

        //$("#pnlBatchExportCCDA  #divdateexport .multiselect").css("border-color", "#ccc");
        //$("#pnlBatchExportCCDA  #divdateexport").find(".control-label").css("color", "#000000");

        //$("#pnlBatchExportCCDA  #divTimeOneTime .multiselect").css("border-color", "#ccc");
        //$("#pnlBatchExportCCDA  #divTimeOneTime").find(".control-label").css("color", "#000000");

        if (Batch_ExportCCDA.IsCreated == false) {
            $('#pnlBatchExportCCDA    #txtpatientMulti').trigger('oninput');
            Batch_ExportCCDA.IsCreated = true;
        }
        Batch_ExportCCDA.LoadNoteComponentsLookup();
        setTimeout(function () {
            $('#' + Batch_ExportCCDA.params.PanelID + ' #divNoteComponents ul li:contains("Demographic Data Element")').find('a').addClass('disableAll');
            $('#' + Batch_ExportCCDA.params.PanelID + ' #divNoteComponents ul li:contains("Provider Data Element")').find('a').addClass('disableAll');
        }, 500);
        $('#pnlBatchExportCCDA    #txtpatientMulti').data("kendoMultiSelect").value([]);
        $('#' + Batch_ExportCCDA.params.PanelID + ' #ddlRoleType').trigger('change');
    },

    CCDAScheduleGridLoad: function (response) {
        $("#pnlBatchExportCCDA #dgvCCDAExport tbody").empty();
        $("#pnlBatchExportCCDA #dgvCCDAExport").dataTable().fnDestroy();
        $("#pnlBatchExportCCDA #dgvCCDAExport tbody").find("tr").remove();
        if (response.ExportCCDACount > 0) {
            var ExportCCDA_JSON = JSON.parse(response.ExportCCDA_JSON); //Parsing array to JSON
            $.each(ExportCCDA_JSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Batch_ExportCCDA.Batch_ExportCCDA_Edit('" + item.SchedulerId + "',event);");
                $row.attr("id", "dgvCCDAExport_Row" + item.SchedulerId);
                $row.attr("SchedulerId", item.SchedulerId);

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


                var ClassDisabled = item.IsActive == "True" ? "" : "disabled";

                var patientName = "";
                var selectMethod = "Admin_Specialty.FillSpecialtyName('" + item.SchedulerId + "',event);"
                if (item.SecPatient != "") {
                    SecPatient = item.SecPatient.split(',');

                    if (SecPatient.length > 1) {
                        patientName = "(Multiple Patients)"
                    }
                    else if (SecPatient.length == 1) {
                        patientName = item.SecPatient.slice(item.SecPatient.lastIndexOf(':') + 1, item.SecPatient.length).replace(' ',', ');
                    } else {
                        patientName = "";
                    }

                } else if (item.IsPatientsAll) {
                    patientName = "(All Patients)"
                }
                var RuleType = item.RuleType;
                if (item.RuleType == "Onetime") {
                    RuleType = "One Time";
                }

                if (ClassDisabled != "disabled")
                    $row.attr("onclick", selectMethod);

                //$row.attr("onclick", "Batch_ExportCCDA.SelectCCDASchedule('" + item.SchedulerId + "',event);");


                $row.append('<td style="display:none;">' + item.SchedulerId + '</td><td><a class="btn btn-xs" href="#" onclick="Batch_ExportCCDA.DeleteCCDASchedule(\'' + item.SchedulerId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Batch_ExportCCDA.SelectCCDASchedule(\'' + item.SchedulerId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Batch_ExportCCDA.CCDASchedule_ActiveInactive(\'' + item.SchedulerId + '\', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + patientName + '</td><td>' + RuleType + '</td><td>' + item.CreatedBy + '</td><td>' + item.CreatedOn + '</td>');

                $("#pnlBatchExportCCDA #dgvCCDAExport tbody").last().append($row);
            });

        }
        else {
            $('#pnlBatchExportCCDA #dgvCCDAExport').DataTable({
                "language": {
                    "emptyTable": "No Schedule Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        //Creating Data Table Instance
        if ($.fn.dataTable.isDataTable('#pnlBatchExportCCDA #dgvCCDAExport'))
            ;
        else {
            $("#pnlBatchExportCCDA #dgvCCDAExport").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "bFilter": true, "order": [[5, "desc"]] });

        }
        var checked = '';
        var isactive = Batch_ExportCCDA.IsActive;
        if (isactive == "0" || isactive == 0) {
            isactive = "0";
        } else if (isactive == null || isactive == '') {
            isactive = "1";
            checked = 'checked="checked"';
        } else {
            isactive = "1";
            checked = 'checked="checked"';
        }
        var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                              '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="Batch_ExportCCDA.activeLoadCCDASchedule(this);">' +
                               '</div><span class="pl-xs">Active</span>';

        $("#" + Batch_ExportCCDA.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        EMRUtility.SwicthWidgetInializatoin();


    },
    activeLoadCCDASchedule: function (objThis) {
        var isactive = $(objThis).attr('isactive');
        if (isactive == '1') {
            $(objThis).attr('isactive', '0');
            isactive = 0;
        }
        else if (isactive == '0') {
            $(objThis).attr('isactive', '1');
            isactive = 1;
        }
        Batch_ExportCCDA.IsActive = isactive;
        Batch_ExportCCDA.LoadCCDASchedule(null, null, null, isactive);
    },
    DeleteCCDASchedule: function (SchedulerId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#dgvCCDAExport_Row' + SchedulerId));
        utility.myConfirm('1', function () {
            var SchedulerValue = SchedulerId;
            if (SchedulerValue == "" || SchedulerValue == "undefined") {
            }
            else {
                Batch_ExportCCDA.DeleteCCDASchedule_DBCall(SchedulerValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        //var table1 = $('#dgvCCDAExport').DataTable();
                        //table1.row('.active').remove().draw(false);
                        utility.DisplayMessages(response.Message, 1);
                        //CacheManager.BindCodes('GetSpecialty', true);
                        Batch_ExportCCDA.ClearValues();
                        Batch_ExportCCDA.LoadCCDASchedule();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { }, '1');
    },
    CCDASchedule_ActiveInactive: function (SchedulerId, IsActive, event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.myConfirm('3', function () {
            var selectedValue = SchedulerId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Batch_ExportCCDA.CCDASchedule_ActiveInactive_DBCall(selectedValue, IsActive).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        Batch_ExportCCDA.LoadCCDASchedule(null, null, null, $('#switchActive').attr('isactive'));
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { }, '3', null, null, null, IsActive);
    },
    CCDAGenerate: function (isFrom,from) {
      //  if ($('#pnlBatchExportCCDA #dtpDateFrom').val() != "" && $('#pnlBatchExportCCDA #dtpDateTo').val() != "") {
        Batch_ExportCCDA.IsdownloadCDDA == false;
            var param = new Object();
            if (Batch_ExportCCDA.IsCreated == false) {
                $('#pnlBatchExportCCDA    #txtpatientMulti').trigger('oninput');
                Batch_ExportCCDA.IsCreated = true;
            }
            var selectedPatients;
            if (from == 'ddl') {
                if ($('#pnlBatchExportCCDA   #ddlPatients').val()) {
                    selectedPatients = $('#pnlBatchExportCCDA   #ddlPatients').val();
                    Batch_ExportCCDA.validatePatients(2);
                } else {
                    Batch_ExportCCDA.validatePatients(1);
                    return false;
                }
              
            } else {
                selectedPatients = $('#pnlBatchExportCCDA    #txtpatientMulti').data("kendoMultiSelect").value();

                
                if (selectedPatients.length < 1) {
                    Batch_ExportCCDA.validatePatientMulti(1);
                    return false;

                } else {
                    Batch_ExportCCDA.validatePatientMulti(2);
                }
            }

            
            //var PatientId = selectedPatients[0];
           // Batch_ExportCCDA.PatientCount = selectedPatients.length;
            var self = null;
            self = $('#' + Batch_ExportCCDA.params.PanelID);
            var myJSON = self != null ? self.getMyJSONByName() : "{}";
            var objData = JSON.parse(myJSON);
            var notecomponentsarry = [];
            if (objData.NoteComponents_text != "") {
                notecomponentsarry = objData.NoteComponents_text.split(',');

                var noteCompObject = [];
                $.each(notecomponentsarry, function (i, item) {
                    var ComponentName = "";
                    if (item == "Demographic Data Element") { ComponentName = "DemographicDataElement"; }
                    if (item == "Provider Data Element") { ComponentName = "ProviderDataElement"; }
                    if (item == "Social Hx") { ComponentName = "SocialHx"; }
                    if (item == "Assessment") { ComponentName = "Assessment"; }
                    if (item == "Plan of Treatment") { ComponentName = "PlanofTreatment"; }
                    if (item == "Plan Of Care") { ComponentName = "PlanOfCare"; }
                    if (item == "Care Team Members") { ComponentName = "CareTeamMembers"; }
                    if (item == "Visit Reason") { ComponentName = "VisitReason"; }
                    if (item == "Health Concerns") { ComponentName = "HealthConcerns"; }
                    if (item == "Vitals") { ComponentName = "Vitals"; }
                    if (item == "Problem Lists") { ComponentName = "ProblemLists"; }
                    if (item == "Medications") { ComponentName = "Medications"; }
                    if (item == "Allergies") { ComponentName = "Allergies"; }
                    if (item == "Immunization") { ComponentName = "Immunization"; }
                    if (item == "Lab Result") { ComponentName = "LabResult"; }
                    if (item == "Procedures") { ComponentName = "Procedures"; }
                    if (item == "Medical Equipment") { ComponentName = "MedicalEquipment"; }
                    if (item == "Referral") { ComponentName = "Refferral"; }
                    if (item == "Functional Status") { ComponentName = "Cognitive"; }
                    if (item == "Encounter Diagnostic") { ComponentName = "EncounterDiagnostic"; }
                    if (item == "Payers Section") { ComponentName = "PayersSection"; }
                    if (item == "Mental Status") { ComponentName = "MentalStatus"; }
                    var obj = {

                        componentId: i * (-1),
                        componentName: ComponentName

                    }
                    noteCompObject.push(obj);
                });
                Batch_ExportCCDA.zip = new JSZip();
                var counter = 0;
                Batch_ExportCCDA.objfiledownload = $.Deferred();
                Batch_ExportCCDA.PatientCounter = 0;
                Batch_ExportCCDA.PatientLength = 0;
                Batch_ExportCCDA.VisitsCounter = 0;
                Batch_ExportCCDA.VisitsLength = 0
                Batch_ExportCCDA.counter = 0;
                Batch_ExportCCDA.PatientLength = selectedPatients.length;
                $.each(selectedPatients, function (k, itemPatients) {
                  //  Batch_ExportCCDA.defLeng = k;
                    param["PatientId"] = Number(itemPatients);
                    param["commandType"] = "GET_SCHEDULED_PATIENTVISITS";
                    param["DateFrom"] = $('#pnlBatchExportCCDA #dtpDateFrom').val();
                    param["DateTo"] = $('#pnlBatchExportCCDA #dtpDateTo').val();
                    data = JSON.stringify(param);
                    MDVisionService.APIService(data, "Batch_ExportCCDA", "ExportCCDA").done(function (response) {
                        if (response != null && response != "") {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                if (response.ExportCCDACount > 0) {
                                    var JSON_Data = JSON.parse(response.ExportCCDA_JSON);
                                    Batch_ExportCCDA.PatientCounter = Batch_ExportCCDA.PatientCounter + 1;
                                    Batch_ExportCCDA.VisitsLength = Batch_ExportCCDA.VisitsLength + JSON_Data.length;
                                    var xmlMainFolder = Batch_ExportCCDA.zip.folder("XML " + JSON_Data[0].PatientFullName + " " + Number(itemPatients));
                                    var counter = 0;
                                    $.each(JSON_Data, function (i, item) {
                                        if (item.Id != null && item.ProviderId != null) {
                                            //Batch_ExportCCDA.generateCCDA(isFrom, item.Id, item.ProviderId, Number(itemPatients), noteCompObject, i, JSON_Data.length, k, selectedPatients.length);
                                            var param1 = new Object();
                                            if ($('#' + Clinical_ContinuityofCareDocument.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {

                                                // End Geting Patient Information
                                                param1["NoteId"] = item.Id;
                                                param1["PatientId"] = Number(itemPatients);
                                                Clinical_ProgressNote.FillNotes(null, param1["NoteId"]).done(function (response) {
                                                    response = JSON.parse(response);
                                                    if (response.status != false || 1 == 1) {
                                                        var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);
                                                        param1["ProviderId"] = item.ProviderId;
                                                        param1["commandType"] = "XMLContinuityofCareDocument";
                                                        param1["Components"] = noteCompObject;
                                                        param1["IsConfidential"] = "false";
                                                        data = JSON.stringify(param1);
                                                        MDVisionService.APIService(data, "CLINICALSUMMARY", "ClinicalSummary").done(function (response) {
                                                            var responseDetail = response = JSON.parse(response);
                                                            if (response.status != false) {
                                                                response.data = JSON.parse(response.data);
                                                                if (response.data.status != false) {
                                                                    $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #hfXMLData").val(response.data.xmlData); //Base64 string in hidden field
                                                                    if (isFrom == Clinical_ContinuityofCareDocument.CommandType.ViewHtml) {
                                                                        param1["XMLData"] = response.data.xmlData; //base64 String
                                                                        param1["ParentCtrl"] = "Batch_ExportCCDA";
                                                                        param1["ActionPanContainer"] = "actionPanBatchExportCCDA";
                                                                        var componentActionPan = "Clinical_ClinicalSummaryHTML";
                                                                        LoadActionPan(componentActionPan, param1);
                                                                    }
                                                                    else if (isFrom == Clinical_ContinuityofCareDocument.CommandType.Download) {

                                                                        if (!Clinical_ContinuityofCareDocument.checkEncryption())
                                                                            return;
                                                                        param1["XMLData"] = response.data.xmlData;
                                                                        param1["Encryption"] = "false";
                                                                        param1["IncludeHashCode"] = "false";
                                                                        param1["Password"] = "";
                                                                        param1["commandType"] = "DOWNLOAD";
                                                                        param1["SummaryType"] = "1"; // 1 for clinical Summary
                                                                        data = JSON.stringify(param1);
                                                                        MDVisionService.APIService(data, "CLINICALSUMMARY", "DownloadFile").done(function (response) {
                                                                            response = JSON.parse(response);
                                                                            counter = counter + 1;
                                                                            Batch_ExportCCDA.counter = Batch_ExportCCDA.counter + 1;
                                                                            Batch_ExportCCDA.VisitsCounter = Batch_ExportCCDA.VisitsCounter + 1;
                                                                            if (response.status != false) {
                                                                                if ($("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #chkIncludeHashCode").prop("checked"))
                                                                                    Batch_ExportCCDA.zip.file("HashCode" + counter + ".txt", response.HashCode);
                                                                                var xml = xmlMainFolder.folder("XML " + counter);

                                                                                var xmlSub = xml.folder("XML");
                                                                                xmlSub.file("XMLData.xml", response.XMLByte, { base64: true });

                                                                                var html = xml.folder("HTML");
                                                                                html.file("htmlData.html", response.HTMLByte, { base64: true });
                                                                                Batch_ExportCCDA.IsdownloadCDDA = true;
                                                                                if ((Batch_ExportCCDA.PatientCounter == Batch_ExportCCDA.PatientLength) && (Batch_ExportCCDA.VisitsCounter == Batch_ExportCCDA.VisitsLength)) {
                                                                                    Batch_ExportCCDA.objfiledownload.resolve();
                                                                                }

                                                                            }
                                                                            else {
                                                                                utility.DisplayMessages(response.Message, 3);
                                                                                if ((Batch_ExportCCDA.PatientCounter == Batch_ExportCCDA.PatientLength) && (Batch_ExportCCDA.VisitsCounter == Batch_ExportCCDA.VisitsLength)) {
                                                                                    Batch_ExportCCDA.objfiledownload.resolve();
                                                                                }
                                                                            }
                                                                        });
                                                                    }
                                                                }
                                                                else {

                                                                    if (Batch_ExportCCDA.objfiledownload.state() == "pending" && Batch_ExportCCDA.IsdownloadCDDA == true && Batch_ExportCCDA.PatientCounter == Batch_ExportCCDA.PatientLength) {
                                                                        Batch_ExportCCDA.objfiledownload.resolve();
                                                                    }

                                                                }
                                                            }

                                                            else {
                                                                utility.DisplayMessages(response.Message, 3);
                                                                //return dfd.promise();
                                                            }
                                                        });
                                                    }
                                                    else {
                                                        utility.DisplayMessages(response.Message, 3);
                                                    }
                                                });

                                            }
                                            else {
                                                utility.DisplayMessages("please select the clinical visit.", 3);
                                            }
                                        }
                                        

                                    });
                                   
                                }
                                else {
                                    try
                                    {
                                        if (Batch_ExportCCDA.IsdownloadCDDA == false && k == selectedPatients.length -1) {
                                            utility.DisplayMessages("No clinical visit found.", 3);
                                            Batch_ExportCCDA.objfiledownload = null;
                                        }
                                        else if (Batch_ExportCCDA.objfiledownload.state() == "pending" && Batch_ExportCCDA.IsdownloadCDDA == true && k == selectedPatients.length - 1)
                                        {
                                            //Batch_ExportCCDA.objfiledownload.resolve();
                                        }
                                    }
                                    catch(e)
                                    {}

                                }
                            }

                        }
                        else {


                        }
                    });



                });
            

                $.when(Batch_ExportCCDA.objfiledownload).then(function () {
                    Batch_ExportCCDA.zip.generateAsync({ type: "blob" })
                                           .then(function (content) {
                                               saveAs(content, "Exported_CCDA.zip");
                                           });
                    utility.DisplayMessages("CCDA Downloaded Successfully.", 1);
                    Batch_ExportCCDA.IsdownloadCDDA = false;
                });
                   
                } else {

                utility.DisplayMessages("CCDA Components not Attached.", 1);
                   
                }
               






        //} else {
        //    $('#pnlBatchExportCCDA ').bootstrapValidator('revalidateField', 'DateFrom');
        //    $('#pnlBatchExportCCDA ').bootstrapValidator('revalidateField', 'DateTo');
        //}
    },
    generateCCDA: function (isFrom, NotesId, ProviderId, PatientId, NoteComponents, VisitsCounter, VisitsLength, PatientCounter, PatientLength) {
        var param = new Object();
        if ($('#' + Clinical_ContinuityofCareDocument.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {

            // End Geting Patient Information
            param["NoteId"] = NotesId;// $('#' + Clinical_ContinuityofCareDocument.params.PanelID + ' #ddlClinicalVisit option:selected').val();
            //Clinical_ProgressNote.params.patientID = "334562";// param["PatientId"] = Clinical_ContinuityofCareDocument.params.PatientId;
            param["PatientId"] = PatientId;
            Clinical_ProgressNote.FillNotes(null, param["NoteId"]).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false || 1 == 1) {
                    //var componentsObjects = [];
                    //var obj1 = {
                    //    componentId: "-1",
                    //    componentName: "DemographicDataElement"
                    //};
                    //var obj2 = {
                    //    componentId: "-2",
                    //    componentName: "ProviderDataElement"
                    //};
                    //componentsObjects.push(obj1);
                    //componentsObjects.push(obj2);

                    var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);
                    param["ProviderId"] = ProviderId;
                    param["commandType"] = "XMLContinuityofCareDocument";
                    param["Components"] = NoteComponents;  // Clinical_ContinuityofCareDocument.getSelectedComponentJSONArray();
                    param["IsConfidential"] = "false"//$("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #chkConfidential").prop("checked");
                    data = JSON.stringify(param); //false comments
                    // data = '{"NoteId":"131990","PatientId":"334562","ProviderId":"209","commandType":"XMLContinuityofCareDocument","Components":[{"componentId":-1,"componentName":"DemographicDataElement"},{"componentId":-2,"componentName":"ProviderDataElement"}]}';

                    MDVisionService.APIService(data, "CLINICALSUMMARY", "ClinicalSummary").done(function (response) {
                        var responseDetail = response = JSON.parse(response);
                        if (response.status != false) {
                            response.data = JSON.parse(response.data);
                            if (response.data.status != false) {
                                $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #hfXMLData").val(response.data.xmlData); //Base64 string in hidden field
                                if (isFrom == Clinical_ContinuityofCareDocument.CommandType.ViewHtml) {
                                    param["XMLData"] = response.data.xmlData; //base64 String
                                    param["ParentCtrl"] = "Batch_ExportCCDA";
                                    param["ActionPanContainer"] = "actionPanBatchExportCCDA";
                                    var componentActionPan = "Clinical_ClinicalSummaryHTML";
                                    LoadActionPan(componentActionPan, param);
                                }
                                else if (isFrom == Clinical_ContinuityofCareDocument.CommandType.Download) {

                                    if (!Clinical_ContinuityofCareDocument.checkEncryption())
                                        return;
                                    param["XMLData"] = response.data.xmlData;
                                    param["Encryption"] = "false";// $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #chkEncryption").prop("checked");
                                    param["IncludeHashCode"] = "false";//$("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #chkIncludeHashCode").prop("checked");
                                    param["Password"] = "";//$("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #txtPassword").val();
                                    param["commandType"] = "DOWNLOAD";
                                    param["SummaryType"] = "1"; // 1 for clinical Summary
                                    data = JSON.stringify(param);
                                    MDVisionService.APIService(data, "CLINICALSUMMARY", "DownloadFile").done(function (response) {
                                        response = JSON.parse(response);
                                        if (response.status != false) {
                                            var zip = new JSZip();
                                            if ($("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #chkIncludeHashCode").prop("checked"))
                                                zip.file("HashCode.txt", response.HashCode);
                                            var xml = zip.folder("XML");

                                            xml.file("XMLData.xml", response.XMLByte, { base64: true });

                                            var html = zip.folder("HTML");
                                            html.file("htmlData.html", response.HTMLByte, { base64: true });
                                            zip.generateAsync({ type: "blob" })
                                            .then(function (content) {
                                                saveAs(content, "CCDA.zip");
                                            });
                                            Batch_ExportCCDA.IsdownloadCDDA = true;
                                            if (((PatientCounter == PatientLength - 1) && (VisitsCounter == VisitsLength - 1))) {
                                                Batch_ExportCCDA.objfiledownload.resolve();
                                            }

                                        }
                                        else {
                                            utility.DisplayMessages(response.Message, 3);
                                        }
                                    });
                                }
                            }
                            else {

                                if (Batch_ExportCCDA.objfiledownload.state() == "pending" && Batch_ExportCCDA.IsdownloadCDDA == true && PatientCounter == PatientLength - 1) {
                                    Batch_ExportCCDA.objfiledownload.resolve();
                                }

                            }
                        }
                       
                        else {
                            utility.DisplayMessages(response.Message, 3);
                            return dfd.promise();
                        }
                    });
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

        }
        else {
            utility.DisplayMessages("please select the clinical visit.", 3);
        }
    },
    UnLoad: function () {
        Batch_ExportCCDA.ClearValues();
        UnloadActionPan(null, 'Batch_ExportCCDA');
        Batch_ExportCCDA.IsCreated = false;
        //if (Batch_ExportCCDA.params != null && Batch_ExportCCDA.params.ParentCtrl) {
        //    UnloadActionPan(Batch_ExportCCDA.params.ParentCtrl);
        //    // UnloadActionPan(Batch_ExportCCDA.params["ParentCtrl"], "Batch_ExportCCDA", null, Batch_ExportCCDA.params["ParentCtrlPanelID"]);
        //    Batch_ExportCCDA.params = null;
        //}
        //else {
        //    UnloadActionPan();
        //    var CurrentMasterTab = GetCurrentMasterTab();
        //    if (CurrentMasterTab.TabID == "mstrTabPatient" && PatientArray.length <= 0) {
        //        ClosePatientNew();
        //        $('.modal-backdrop.fade.in').remove();
        //    }
        //}
    },

    UnLoadTab: function () {
        Batch_ExportCCDA.ClearValues();
        Batch_ExportCCDA.IsCreated = false;
        RemoveAdminTab();
    },

    SelectTab: function (Type) {
        Batch_ExportCCDA.Type = Type;
        //$(Patient_Referrals.params.PanelID + " #headingTitle").html("Search " + Type + " Referrals");
        if (Batch_ExportCCDA.Type == "ImportCCDA") {
            //Batch_ExportCCDA.ValidateIncomingTab();
            if (!$("#" + Batch_ExportCCDA.params.PanelID + " #ImportCCDA").hasClass("active")) {
                $("#" + Batch_ExportCCDA.params.PanelID + " #ImportCCDA").addClass("active");
            }
            $("#" + Batch_ExportCCDA.params.PanelID + " #ImportCypress").removeClass("active");
        }
        else {
            //Batch_ExportCCDA.ValidateOutcomingTab();
            $("#" + Batch_ExportCCDA.params.PanelID + " #ImportCCDA").removeClass("active");
            if (!$("#" + Batch_ExportCCDA.params.PanelID + " #ImportCypress").hasClass("active")) {
                $("#" + Batch_ExportCCDA.params.PanelID + " #ImportCypress").addClass("active");
            }
        }
        //Patient_Referrals.ReferralSearch();
        return true;
    },

}