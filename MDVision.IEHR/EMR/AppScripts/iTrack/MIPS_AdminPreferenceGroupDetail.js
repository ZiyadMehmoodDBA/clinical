MIPS_AdminPreferenceGroupDetail = {
    params: [],
    bIsFirstLoad: true,
    RequireSubmit: true,
    ActualVisitID: null,
    specialityCheckedIds: [],
    SpecialtyIds: '',
    ActualChargeCaptureID: null,
    IsChargeCapture: null,
    Load: function (params) {

        MIPS_AdminPreferenceGroupDetail.params = params;
        EMRUtility.SwicthWidgetInializatoin();
        MIPS_AdminPreferenceGroupDetail.LoadAllAutocomplete();
        ////  MIPS_AdminPreferenceGroupDetail.BindProvider();

        if (MIPS_AdminPreferenceGroupDetail.bIsFirstLoad) {

            $('#switchActiveACI').prev('div').removeClass('off');
            $('#switchActiveACI').prev('div').addClass('on');
            $('#switchActiveIA').prev('div').removeClass('off');
            $('#switchActiveIA').prev('div').addClass('on');
            MIPS_AdminPreferenceGroupDetail.bIsFirstLoad = false;
            
            var self = $('#pnlMIPSAdminPreferenceGroupDetail');
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#EntityId").attr('disabled', 'disabled');

            }
            self.loadDropDowns(true).done(function () {
                if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                    self.find("#EntityId").val(globalAppdata["SeletedEntityId"]);
                    MIPS_AdminPreferenceGroupDetail.ValidateMeasureGroup();
                    $('#pnlMIPSAdminPreferenceGroupDetail #switchActiveIA').prop('checked', true);
                    $('#pnlMIPSAdminPreferenceGroupDetail #switchActiveACI').prop('checked', true);
                    $('#pnlMIPSAdminPreferenceGroupDetail').data('bootstrapValidator').enableFieldValidators('DateFromQuality', false);
                    $('#pnlMIPSAdminPreferenceGroupDetail').data('bootstrapValidator').enableFieldValidators('DateToQuality', false);
                    //$('#pnlMIPSAdminPreferenceGroupDetail').data('bootstrapValidator').enableFieldValidators('DateFromACI', false);
                    //$('#pnlMIPSAdminPreferenceGroupDetail').data('bootstrapValidator').enableFieldValidators('DateToACI', false);
                    //$('#pnlMIPSAdminPreferenceGroupDetail').data('bootstrapValidator').enableFieldValidators('DateFromIA', false);
                    //$('#pnlMIPSAdminPreferenceGroupDetail').data('bootstrapValidator').enableFieldValidators('DateToIA', false);
                    $('#pnlMIPSAdminPreferenceGroupDetail').data('bootstrapValidator').enableFieldValidators('DateFromCost', false);
                    $('#pnlMIPSAdminPreferenceGroupDetail').data('bootstrapValidator').enableFieldValidators('DateToCost', false);
                    MIPS_AdminPreferenceGroupDetail.LoadPracticLookup();
                    MIPS_AdminPreferenceGroupDetail.LoadMIPSProviders();            //MIPS_AdminPreferenceGroupDetail.Search_IndividualProvider();
                    MIPS_AdminPreferenceGroupDetail.LoadGroupCatLookup();
                    //self.find("*#lstEntityIdGroup").val(globalAppdata["SeletedEntityId"]);
                }
                //MIPS_AdminPreferenceGroupDetail.UserSearch('0');
            });

        }
        if (MIPS_AdminPreferenceGroupDetail.params.mode == "Edit")
        {
            MIPS_AdminPreferenceGroupDetail.SelectGroupPreferences();

        }

    },
    SelectGroupPreferences: function () {
        MIPS_AdminPreferenceGroupDetail.SelectGroupPreferences_DBCall().done(function (response) {
            response = JSON.parse(response);

            if (response.status != false) {
                var list = JSON.parse(response.IndividualProCountLoad_JSON);
                $('#pnlMIPSAdminPreferenceGroupDetail #txtTIN').val(list.Groups[0].TIN);
                $('#pnlMIPSAdminPreferenceGroupDetail #txtGroupName').val(list.Groups[0].GroupName);
                $('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail #dtpSubmissionYear').datepicker('setDate', list.Groups[0].PerformanceYear);
                $('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail #ddlPractice').val(list.Groups[0].PracticeId);
                if (list.Groups[0].PracticeType == "Rural") {
                    $("#rdoRural").prop('checked', true)
                } else if (list.Groups[0].PracticeType == "Small") {
                    $("#rdoSmall").prop('checked', true)
                } else if (list.Groups[0].PracticeType == "Large") {
                    $("#rdoLarge").prop('checked', true)
                }
                if (list.Groups[0].IsActive == "True") {
                    $("#chkIsActive").prop('checked', true)
                } else {
                    $("#chkIsActive").prop('checked', false)
                }
                if (list.Groups[0].IsReporting == "True") {
                    $.each(list.GroupDetail, function (i, item) {
                       
                        if (item.ReportingCat == "Quality")
                        {
                            if (item.IsFullYear == "False") {
                                $("#switchActiveQuality").prev('.ios-switch').addClass('on');
                                $("#switchActiveQuality").prev('.ios-switch').removeClass('of');
                                $("#divPeriodQuality").removeClass('hidden');
                                $("#divYearQuality").addClass('hidden');
                                $('#pnlMIPSAdminPreferenceGroupDetail  #dtpDateFromQuality').datepicker('setDate', new Date(item.DateFrom));
                                $('#pnlMIPSAdminPreferenceGroupDetail  #dtpDateToQuality').datepicker('setDate', new Date(item.DateTo));
                                $('#pnlMIPSAdminPreferenceGroupDetail #switchActiveQuality').prop('checked', true);
                            } else {
                                var date = new Date(item.DateFrom);
                                $('#pnlMIPSAdminPreferenceGroupDetail  #ddlQualityYeaR').val(date.getFullYear());
                            }
                        } else if (item.ReportingCat == "Promoting Interoperability (Formally ACI)") {
                            if (item.IsFullYear == "False") {
                                $("#switchActiveACI").prev('.ios-switch').addClass('on');
                                $("#switchActiveACI").prev('.ios-switch').removeClass('of');
                            $("#divPeriodACI").removeClass('hidden');
                            $("#divYearACI").addClass('hidden');
                            $('#pnlMIPSAdminPreferenceGroupDetail  #dtpDateFromACI').datepicker('setDate', new Date(item.DateFrom));
                            $('#pnlMIPSAdminPreferenceGroupDetail  #dtpDateToACI').datepicker('setDate', new Date(item.DateTo));
                            $('#pnlMIPSAdminPreferenceGroupDetail #switchActiveACI').prop('checked', true);
                            } else {
                                var date = new Date(item.DateFrom);
                                $('#pnlMIPSAdminPreferenceGroupDetail  #ddlACIYeaR').val(date.getFullYear());
                            }
                        } else if (item.ReportingCat == "IA (Improvement Activities)") {
                            if (item.IsFullYear == "False") {
                                $("#switchActiveIA").prev('.ios-switch').addClass('on');
                                $("#switchActiveIA").prev('.ios-switch').removeClass('of');
                            $("#divPeriodIA").removeClass('hidden');
                            $("#divYearIA").addClass('hidden');
                            $('#pnlMIPSAdminPreferenceGroupDetail  #dtpDateFromIA').datepicker('setDate', new Date(item.DateFrom));
                            $('#pnlMIPSAdminPreferenceGroupDetail  #dtpDateToIA').datepicker('setDate', new Date(item.DateTo));
                            $('#pnlMIPSAdminPreferenceGroupDetail #switchActiveIA').prop('checked', true);
                            } else {
                                var date = new Date(item.DateFrom);
                                $('#pnlMIPSAdminPreferenceGroupDetail  #ddlIAYeaR').val(date.getFullYear());
                            }
                        } else if (item.ReportingCat == "Cost") {
                            if (item.IsFullYear == "False") {
                                $("#switchActiveCost").prev('.ios-switch').addClass('on');
                                $("#switchActiveCost").prev('.ios-switch').removeClass('of');
                            $("#divPeriodCost").removeClass('hidden');
                            $("#divYearCost").addClass('hidden');
                            $('#pnlMIPSAdminPreferenceGroupDetail  #dtpDateFromCost').datepicker('setDate', new Date(item.DateFrom));
                            $('#pnlMIPSAdminPreferenceGroupDetail  #dtpDateToCost').datepicker('setDate', new Date(item.DateTo));
                            $('#pnlMIPSAdminPreferenceGroupDetail #switchActiveCost').prop('checked', true);
                            } else {
                                var date = new Date(item.DateFrom);
                                $('#pnlMIPSAdminPreferenceGroupDetail  #ddlCostYeaR').val(date.getFullYear());
                            }
                        }

                    });


                } else {
                    $("#switchReporting").prev('.ios-switch').addClass('on');
                    $("#switchReporting").prev('.ios-switch').removeClass('of');
                    $("#NotReporting").removeClass('hidden');
                    $("#Reporting").addClass('hidden');
                    $('#pnlMIPSAdminPreferenceGroupDetail #txtReason').val(list.Groups[0].ReportingReason);
                    $('#pnlMIPSAdminPreferenceGroupDetail #switchReporting').prop('checked', true);
                    
                }

            }
            else {

            }

        });

    },
    SelectGroupPreferences_DBCall: function () {
        var objData = new Object();
        objData["GroupId"] = MIPS_AdminPreferenceGroupDetail.params.GroupId;
        objData["commandType"] = "selectmipsgrouppreferences";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "iTrack", "iTrackAdmin");

    },
    GridTabChange: function (NoteStatus) {
        MIPS_AdminPreferenceGroupDetail.LoadMIPSProviders(NoteStatus);
    },
    LoadPracticLookup: function () {
        MIPS_AdminPreferenceGroupDetail.LoadPracticLookup_DBCall().done(function (response) {
            response = JSON.parse(response);

            if (response.status != false) {
                var list = JSON.parse(response.IndividualProCountLoad_JSON);
                $.each(list, function (i, item) {
                    $('#ddlPractice').append('<option value=' + item.PracticeId + ' >' + item.PracticeName + '</option>')
                });

            }
            else {

            }

        });

    },
    LoadPracticLookup_DBCall: function () {
        var objData = new Object();
        objData["EntityId"] = globalAppdata.SeletedEntityId;
        objData["commandType"] = "loadpracticlookup";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "iTrack", "iTrackAdmin");

    },
    LoadGroupCatLookup: function () {
        MIPS_AdminPreferenceGroupDetail.LoadGroupCatLookup_DBCall().done(function (response) {
            response = JSON.parse(response);

            if (response.status != false) {
                if (response.IndividualProCountLoad_JSON != null ) {
                    MIPS_AdminPreferenceGroupDetail['GroupCatLookUp'] = JSON.parse(response.IndividualProCountLoad_JSON);
                }
            }
            else {

            }

        });

    },
    LoadGroupCatLookup_DBCall: function () {
        var objData = new Object();
        objData["commandType"] = "loadgroupcatlookup";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "iTrack", "iTrackAdmin");

    },
    LoadMIPSProviders: function (tab, pageNumber, resultPerPage) {
        if (tab == 0) {

            if ($('#pnlMIPSAdminPreferenceGroupDetail #liMDVision').hasClass('active')) {
                tab = "MD Vision EHR";
            } else if ($('#pnlMIPSAdminPreferenceGroupDetail #liAllProviders').hasClass('active')) {
                tab = "All Providers";
            } else if ($('#pnlMIPSAdminPreferenceGroupDetail #liSovereignHealthRegistry').hasClass('active')) {
                tab = "Sovereign Health Registry";
            } else if ($('#pnlMIPSAdminPreferenceGroupDetail #liSovereignQCDRs').hasClass('active')) {
                tab = "Sovereign QCDR";
            } else if ($('#pnlMIPSAdminPreferenceGroupDetail #liOther').hasClass('active')) {
                tab = "Other";
            }
        }
        MIPS_AdminPreferenceGroupDetail.LoadMIPSProviders_DBCall(tab, pageNumber, resultPerPage).done(function (response) {
            response = JSON.parse(response);

            if (response.status != false && response.IndividualProCount > 0) {

                var list = JSON.parse(response.IndividualProCountLoad_JSON);
                MIPS_AdminPreferenceGroupDetail.GridLoad(list);
                var TableControl = MIPS_AdminPreferenceGroupDetail.params.PanelID + " #dgvMIPSAdminProvider";
                var PagingPanelControlID = MIPS_AdminPreferenceGroupDetail.params.PanelID + " #divdgvMIPSAdminProviderPaging";
                var ClassControlName = "MIPS_AdminPreferenceGroupDetail";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.IndividualProCount, pageNumber, 15, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (tab, pageNumber, resultPerPage) {
                    MIPS_AdminPreferenceGroupDetail.LoadMIPSProviders(tab, pageNumber, resultPerPage);
                }), 10);
            }
            else {
                MIPS_AdminPreferenceGroupDetail.GridLoad();
            }

        });

    },
    LoadMIPSProviders_DBCall: function (tab, pageNumber, resultPerPage) {
        if (!tab || tab == "All Providers") {
            tab = null;
        }
        if (tab == "MD Vision") {
            tab = "MD Vision EHR";
        }
       
        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (resultPerPage == null) {
            resultPerPage = 15;
        }
        var objData = new Object();
        objData["EntityId"] = globalAppdata.SeletedEntityId;
        objData["pageNumber"] = pageNumber;
        objData["RowsPerPage"] = resultPerPage;
        objData["ReportingMethod"] = tab;
        if(tab !=null)
            objData["GroupId"] = MIPS_AdminPreferenceGroupDetail.params.GroupId;

        objData["commandType"] = "loadmipsprovider";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "iTrack", "iTrackAdmin");

    },
    multiselect_Validator: function (cntrl) {
        if ($(cntrl).val() == null || $(cntrl).val() == "") {
            $(cntrl).closest('div').find('label.control-label').addClass('has-error');
            $(cntrl).closest('div').find('label.control-label').css('color', '#cc2724');
        } else {
            $(cntrl).closest('div').find('label.control-label').removeClass('has-error');
            $(cntrl).closest('div').find('label.control-label').css('color', '');
        }
    },
    CheckEligibility: function (id, obj) {
        if (id == "Ineligibile" && obj.checked == true) {
            $('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail #divIneligibile').removeClass('hidden');
        }
        else {
            $('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail #divIneligibile').addClass('hidden');
        }
    },
    SelectPerriod: function (obj, div) {
        if (obj.checked == true) {
            $('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail #divYear' + div).addClass('hidden');
            $('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail #divPeriod' + div).removeClass('hidden');
            $('#pnlMIPSAdminPreferenceGroupDetail').data('bootstrapValidator').enableFieldValidators('DateFrom' + div, true);
            $('#pnlMIPSAdminPreferenceGroupDetail').data('bootstrapValidator').enableFieldValidators('DateTo' + div, true);
            $('#pnlMIPSAdminPreferenceGroupDetail').data('bootstrapValidator').enableFieldValidators(div + 'YeaR', false);
        } else {
            $('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail #divYear' + div).removeClass('hidden');
            $('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail #divPeriod' + div).addClass('hidden');
            $('#pnlMIPSAdminPreferenceGroupDetail').data('bootstrapValidator').enableFieldValidators('DateFrom' + div, false);
            $('#pnlMIPSAdminPreferenceGroupDetail').data('bootstrapValidator').enableFieldValidators('DateTo' + div, false);
            $('#pnlMIPSAdminPreferenceGroupDetail').data('bootstrapValidator').enableFieldValidators(div + 'YeaR', true);
        }
        MIPS_AdminPreferenceGroupDetail.endabelDisableValidation(div);
    },
    endabelDisableValidation: function (div) {


    },
    SelectReportingDiv: function (obj) {
        if (obj.checked == true) {
            $('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail #Reporting').addClass('hidden');
            $('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail #NotReporting').removeClass('hidden');
            $('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail').data('bootstrapValidator').enableFieldValidators('Reason', true);
        } else {
            $('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail #Reporting').removeClass('hidden');
            $('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail #NotReporting').addClass('hidden');
            $('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail').data('bootstrapValidator').enableFieldValidators('Reason', false);
        }

    },
    LoadAllAutocomplete: function () {
        EMRUtility.CreateYearViewDatePicker('pnlMIPSAdminPreferenceGroupDetail #dtpSubmissionYear',

               function (ev) {
                   if ($('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail').data("bootstrapValidator") != null) {
                       $('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail').bootstrapValidator('revalidateField', 'SubmissionYear');
                   }
               }, true);

        // For Quality
        utility.CreateDatePicker("pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail #dtpDateToQuality",
      function (ev) {

          if ($('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail').data("bootstrapValidator") != null) {
              $('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail').bootstrapValidator('revalidateField', 'DateToQuality');
          }

      }, true);
        //$('#pnliTrackAdminIPPreference #frmMIPSAdminPreferenceGroupDetail #dtpDateToQuality').datepicker('setEndDate', new Date());

        utility.CreateDatePicker("pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail #dtpDateFromQuality",
    function (ev) {

        if ($('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail').data("bootstrapValidator") != null) {
            $('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail').bootstrapValidator('revalidateField', 'DateFromQuality');

        }

        //on-change callback method
    }, true);
        //$('#pnliTrackAdminIPPreference #frmAuditbleEventsActivityLog #dtpDateFrom').datepicker('setEndDate', new Date());
        utility.ValidateFromToDate('frmMIPSAdminPreferenceGroupDetail', 'dtpDateFromQuality', 'dtpDateToQuality', true, null, null, true);
        // For ACI
        utility.CreateDatePicker("pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail #dtpDateToACI",
    function (ev) {

        if ($('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail').data("bootstrapValidator") != null) {
            $('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail').bootstrapValidator('revalidateField', 'DateToACI');
        }

    }, true);
        //$('#pnliTrackAdminIPPreference #frmMIPSAdminPreferenceGroupDetail #dtpDateToQuality').datepicker('setEndDate', new Date());

        utility.CreateDatePicker("pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail #dtpDateFromACI",
    function (ev) {

        if ($('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail').data("bootstrapValidator") != null) {
            $('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail').bootstrapValidator('revalidateField', 'DateFromACI');

        }

        //on-change callback method
    }, true);
        //$('#pnliTrackAdminIPPreference #frmAuditbleEventsActivityLog #dtpDateFrom').datepicker('setEndDate', new Date());
        utility.ValidateFromToDate('frmMIPSAdminPreferenceGroupDetail', 'dtpDateFromACI', 'dtpDateToACI', true, null, null, true);
        // For IA
        utility.CreateDatePicker("pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail #dtpDateToIA",
    function (ev) {

        if ($('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail').data("bootstrapValidator") != null) {
            $('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail').bootstrapValidator('revalidateField', 'DateToIA');
        }

    }, true);
        //$('#pnliTrackAdminIPPreference #frmMIPSAdminPreferenceGroupDetail #dtpDateToQuality').datepicker('setEndDate', new Date());

        utility.CreateDatePicker("pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail #dtpDateFromIA",
    function (ev) {

        if ($('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail').data("bootstrapValidator") != null) {
            $('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail').bootstrapValidator('revalidateField', 'DateFromIA');

        }

        //on-change callback method
    }, true);
        //$('#pnliTrackAdminIPPreference #frmAuditbleEventsActivityLog #dtpDateFrom').datepicker('setEndDate', new Date());
        utility.ValidateFromToDate('pnlMIPSAdminPreferenceGroupDetail', 'dtpDateFromIA', 'dtpDateToIA', true, null, null, true);
        // For Cost
        utility.CreateDatePicker("pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail #dtpDateToCost",
    function (ev) {

        if ($('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail').data("bootstrapValidator") != null) {
            $('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail').bootstrapValidator('revalidateField', 'DateToCost');
        }

    }, true);
        //$('#pnliTrackAdminIPPreference #frmMIPSAdminPreferenceGroupDetail #dtpDateToQuality').datepicker('setEndDate', new Date());

        utility.CreateDatePicker("pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail #dtpDateFromCost",
    function (ev) {

        if ($('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail').data("bootstrapValidator") != null) {
            $('#pnlMIPSAdminPreferenceGroupDetail #frmMIPSAdminPreferenceGroupDetail').bootstrapValidator('revalidateField', 'DateFromCost');

        }

        //on-change callback method
    }, true);
        //$('#pnliTrackAdminIPPreference #frmAuditbleEventsActivityLog #dtpDateFrom').datepicker('setEndDate', new Date());
        utility.ValidateFromToDate('frmMIPSAdminPreferenceGroupDetail', 'dtpDateFromCost', 'dtpDateToCost', true, null, null, true);
    },
    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmMIPSlIndividualProvider";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "MIPSPreference_IndividualProvider";
        LoadActionPan('Admin_Provider', params);
    },
    HideProviderLink: function (value) {
        if (value == "") {
            $('#pnlIndividualProvider #txtProvider').attr("ProviderId", "-1");
            $('#pnlIndividualProvider #hfProvider').val("-1");
            $("#pnlIndividualProvider #lnkProviderEdit").css("display", "none");
            $("#pnlIndividualProvider #lblProvider").css("display", "inline");
        }
    },
    BindProvider: function () {
        var Ctrl = $("#pnlIndividualProvider #frmMIPSAdminPreferenceGroupDetail #txtProvider");
        var hfCtrl = $("#pnlIndividualProvider #frmMIPSAdminPreferenceGroupDetail #hfProvider");
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, null);
    },
    SelectIndiualOrGroup: function (obj, id) {
        if (id == "Group" && obj.checked == true) {
            $('#pnliTrackAdminIPPreference #frmMIPSAdminPreferenceGroupDetail #divReportingForGroup').removeClass('hidden');
            $('#pnliTrackAdminIPPreference #frmMIPSAdminPreferenceGroupDetail #divReportingForInd').addClass('hidden');
        }
        else {
            $('#pnliTrackAdminIPPreference #frmMIPSAdminPreferenceGroupDetail #divReportingForGroup').addClass('hidden');
            $('#pnliTrackAdminIPPreference #frmMIPSAdminPreferenceGroupDetail #divReportingForInd').removeClass('hidden');
        }
    },
    // Zia Mehmood
    //Validation function
    ValidateMeasureGroup: function () {
        $('#pnlMIPSAdminPreferenceGroupDetail ').bootstrapValidator('destroy');
        $('#pnlMIPSAdminPreferenceGroupDetail ')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  DateFromQuality: {
                      group: '.col-xs-6',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  DateToQuality: {
                      group: '.col-xs-6',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  DateFromACI: {
                      group: '.col-xs-6',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  DateToACI: {
                      group: '.col-xs-6',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  DateFromIA: {
                      group: '.col-xs-6',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  DateToIA: {
                      group: '.col-xs-6',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  DateFromCost: {
                      group: '.col-xs-6',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  DateToCost: {
                      group: '.col-xs-6',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  QualityYeaR: {
                      group: '.col-xs-6',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  ACIYeaR: {
                      group: '.col-xs-6',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  IAYeaR: {
                      group: '.col-xs-6',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  CostYeaR: {
                      group: '.col-xs-6',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  SubmissionYear: {
                      group: '.col-sm-2',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Practice: {
                      group: '.col-sm-2',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Entity: {
                      group: '.col-sm-2',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  TIN: {
                      group: '.col-sm-2',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Reason: {
                      group: '.col-sm-12',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  GroupName: {
                      group: '.col-sm-2',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  }

              }
          }).on('error.form.bv', function (e) {
              // Prevent form submission
              e.preventDefault();

          })
       .on('success.form.bv', function (e) {


           e.preventDefault();
           MIPS_AdminPreferenceGroupDetail.saveMIPSPreferencesGroup();
       });
    },
    // End validation function
    saveMIPSPreferencesGroup: function () {
        MIPS_AdminPreferenceGroupDetail.saveMIPSPreferencesGroup_DBCall().done(function (response) {
            response = JSON.parse(response);

            if (response.status != false) {
                utility.DisplayMessages(response.Message, 2);
                MIPS_AdminPreferenceGroup.Search_MIPSGroupPreferences();
                MIPS_AdminPreferenceGroupDetail.UnLoad();


            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });

    },
    saveMIPSPreferencesGroup_DBCall: function () {
       
        var objData = new Object();
        var IsReporting = 1;
        if ($('#pnlMIPSAdminPreferenceGroupDetail #switchReporting').prop('checked')) {
            IsReporting = 0;
            objData["ReportingReason"] = $('#pnlMIPSAdminPreferenceGroupDetail #txtReason').val();

        } else {
            var PracticeId = $('#pnlMIPSAdminPreferenceGroupDetail input[name=rdoPractice]:checked').val();
            var Practice = 'Large';
            if (PracticeId == 1) {
                Practice = 'Large';
            } else if (PracticeId == 2) {
                Practice = 'Small';
            } else if (PracticeId == 3) {
                Practice = 'Rural';
            }
            var QulityId=null;
            var ACIId = null;
            var IAId = null;
            var CostId = null;
            if (MIPS_AdminPreferenceGroupDetail['GroupCatLookUp'])
            {
                $.each(MIPS_AdminPreferenceGroupDetail['GroupCatLookUp'], function (i,item) {
                    if (item.Name == "Quality") {
                        
                        QulityId = item.LookupId;
                    }
                    if (item.Name == "Promoting Interoperability (Formally ACI)") {

                        ACIId = item.LookupId;
                    }
                    if (item.Name == "IA (Improvement Activities)") {

                        IAId = item.LookupId;
                    }
                    if (item.Name == "Cost") {

                        CostId = item.LookupId;
                    }

                });

            }
            objData["PracticeType"] = Practice;
            var MIPSCatData = [];
            if ($('#pnlMIPSAdminPreferenceGroupDetail #chkQuality').prop('checked')) {
                if ($('#pnlMIPSAdminPreferenceGroupDetail #switchActiveQuality').prop('checked')) {
                    data = {
                        ReportingCat: QulityId,
                        DateFrom: $('#pnlMIPSAdminPreferenceGroupDetail #dtpDateFromQuality').val(),
                        DateTo: $('#pnlMIPSAdminPreferenceGroupDetail #dtpDateToQuality').val(),
                        GroupId: MIPSGroupId,
                        IsFullYear:false
                    }
                }
                else {
                    data = {
                        ReportingCat: QulityId,
                        DateFrom: "01/01/" + $('#pnlMIPSAdminPreferenceGroupDetail #ddlQualityYeaR :selected').text(),
                        DateTo: "12/31/" + $('#pnlMIPSAdminPreferenceGroupDetail #ddlQualityYeaR :selected').text(),
                        GroupId: MIPSGroupId,
                        IsFullYear: true,
                    }
                    
                }
                MIPSCatData.push(data);
            }
            if ($('#pnlMIPSAdminPreferenceGroupDetail #chkACI').prop('checked')) {
                if ($('#pnlMIPSAdminPreferenceGroupDetail #switchActiveACI').prop('checked')) {
                    data = {
                        ReportingCat: ACIId,
                        DateFrom: $('#pnlMIPSAdminPreferenceGroupDetail #dtpDateFromACI').val(),
                        DateTo: $('#pnlMIPSAdminPreferenceGroupDetail #dtpDateToACI').val(),
                        GroupId: MIPSGroupId,
                        IsFullYear: false,
                    }
                }
                else {
                    data = {
                        ReportingCat: ACIId,
                        DateFrom: "01/01/" + $('#pnlMIPSAdminPreferenceGroupDetail #ddlACIYeaR :selected').text(),
                        DateTo: "12/31/" + $('#pnlMIPSAdminPreferenceGroupDetail #ddlACIYeaR :selected').text(),
                        GroupId: MIPSGroupId,
                        IsFullYear: true,
                    }
                   
                }
                MIPSCatData.push(data);
            }
            if ($('#pnlMIPSAdminPreferenceGroupDetail #chkIA').prop('checked')) {
                if ($('#pnlMIPSAdminPreferenceGroupDetail #switchActiveIA').prop('checked')) {
                    data = {
                        ReportingCat: IAId,
                        DateFrom: $('#pnlMIPSAdminPreferenceGroupDetail #dtpDateFromIA').val(),
                        DateTo: $('#pnlMIPSAdminPreferenceGroupDetail #dtpDateToIA').val(),
                        GroupId: MIPSGroupId,
                        IsFullYear: false,
                    }
                }
                else {
                    data = {
                        ReportingCat: IAId,
                        DateFrom: "01/01/" + $('#pnlMIPSAdminPreferenceGroupDetail #ddlIAYeaR :selected').text(),
                        DateTo: "12/31/" + $('#pnlMIPSAdminPreferenceGroupDetail #ddlIAYeaR :selected').text(),
                        GroupId: MIPSGroupId,
                        IsFullYear: true,
                    }
                    
                }
                MIPSCatData.push(data);
            }
            if ($('#pnlMIPSAdminPreferenceGroupDetail #chkCost').prop('checked')) {
                if ($('#pnlMIPSAdminPreferenceGroupDetail #switchActiveCost').prop('checked')) {
                    data = {
                        ReportingCat: CostId,
                        DateFrom: $('#pnlMIPSAdminPreferenceGroupDetail #dtpDateFromCost').val(),
                        DateTo: $('#pnlMIPSAdminPreferenceGroupDetail #dtpDateToCost').val(),
                        GroupId: MIPSGroupId,
                        IsFullYear: false,
                    }
                }
                else {
                    data = {
                        ReportingCat: CostId,
                        DateFrom: "01/01/" + $('#pnlMIPSAdminPreferenceGroupDetail #ddlCostYeaR :selected').text(),
                        DateTo: "12/31/" + $('#pnlMIPSAdminPreferenceGroupDetail #ddlCostYeaR :selected').text(),
                        GroupId: MIPSGroupId,
                        IsFullYear: true,
                    }
                   
                }
                MIPSCatData.push(data);
            }
            objData["MIPSCatData"] = MIPSCatData;

        }
        var MIPSGroupId = null;
        if (MIPS_AdminPreferenceGroupDetail.params.mode == "Edit") {
            MIPSGroupId = MIPS_AdminPreferenceGroupDetail.params.GroupId;
            objData["commandType"] = "updatemipspreferencesgroup";
        } else {
            objData["commandType"] = "savemipspreferencesgroup";
        }
        var isactive = $('#chkIsActive').prop('checked')
        objData["IsActive"] = $('#chkIsActive').prop('checked');
        objData["GroupId"] = MIPSGroupId;
        objData["IsReporting"] = IsReporting;
        objData["EntityId"] = $('#pnlMIPSAdminPreferenceGroupDetail #EntityId :selected').val();
        objData["SubmissionYear"] = $('#pnlMIPSAdminPreferenceGroupDetail #dtpSubmissionYear').val();
        objData["PracticeId"] = $('#pnlMIPSAdminPreferenceGroupDetail #ddlPractice :selected').val();
        objData["TIN"] = $('#pnlMIPSAdminPreferenceGroupDetail #txtTIN').val();
        objData["GroupName"] = $('#pnlMIPSAdminPreferenceGroupDetail #txtGroupName').val();
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "iTrack", "iTrackAdmin");

    },
    // Start-- search Activity Log
    Search_IndividualProvider: function (pageNumber, rowsPerPage) {

        MIPS_AdminPreferenceGroupDetail.Search_IndividualProvider_DBCall(pageNumber, rowsPerPage).done(function (response) {
            response = JSON.parse(response);

            if (response.status != false) {


                var ActivityLogUser_JSON = JSON.parse(response.ActivityLogUser_JSON);
                MIPS_AdminPreferenceGroupDetail.GridLoad(ActivityLogUser_JSON, response.ActivityLogUserCount, "NewEntry", " #pnlAuditbleEventsActivityLog #dgvNewEntry");
                var TableControl = MIPS_AdminPreferenceGroupDetail.params.PanelID + " #dgvNewEntry";
                var PagingPanelControlID = MIPS_AdminPreferenceGroupDetail.params.PanelID + " #dgvActivityLogUser_Paging";
                var ClassControlName = "AuditbleEvents_ActivityLog";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.ActivityLogUserCount, pageNumber, 15, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, pageNumber, resultPerPage) {
                    MIPS_AdminPreferenceGroupDetail.Search_ActivityLog(pageNumber, resultPerPage);
                }), 10);
                if (ActivityLogUser_JSON.length > 0) {
                    $('#pnlAuditbleEventsActivityLog #dgvNewEntry tbody tr:first').click();
                }
                else {
                    MIPS_AdminPreferenceGroupDetail.GridLoad(ActivityLogUser_JSON, response.ActivityLogCompCount, "User", " #pnlAuditbleEventsActivityLog #dgvUser");
                    MIPS_AdminPreferenceGroupDetail.GridLoad(ActivityLogUser_JSON, response.ActivityLogCompCount, "Changes", " #pnlAuditbleEventsActivityLog #dgvChanges");
                }


            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });

    },
    Search_IndividualProvider_DBCall: function (pageNumber, rowsPerPage) {
        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 15;
        }
        var PracticeTYpe = null;
        if ($("#rdoRural").prop('checked'))
        {
            PracticeTYpe = "Rural Practice";
        } else if ($("#rdoSmall").prop('checked')) {
            PracticeTYpe = "Small Practice(15 or fewer clinicians)";
        } else if ($("#rdoLarge").prop('checked')) {
            PracticeTYpe = "Large Practice";
        }
        var objData = new Object();
        //objData["AuditReportId"] = AuditReportId;
        objData["PageNumber"] = pageNumber;
        objData["RowsPerPage"] = rowsPerPage;

        //objData["ProviderId"] = Clinical_ProgressNote.params.CurrentNotesProviderId
        objData["ProviderName"] = $("#NoteProviderText").val();
        objData["Specialty"] = $("#ddlSpeciality :selected").text();
        objData["NPI"] = $("#txtNPI").val();
        objData["Entity"] = $("#ddlSpeciality :selected").text();
        objData["PracticeType"] = PracticeTYpe;
        objData["MIPSEligibility"] = $("#ddlMIPSEligibility :selected").text();
        objData["Ineligible"] = $("#ddlIneligible :selected").text();
        objData["ReportingType"] = $("#ddlReportingType :selected").text();
        objData["ReportingMethod"] = $("#ddlReportingMethod :selected").text();
        objData["ReportingMIPS"] = $("#ddlReportingMIPS :selected").text();
        objData["IsActive"] = 1;
        objData["commandType"] = "auditbleeventsactivitylog";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "AuditbleEventsActivityLog", "AuditbleEventsActivityLog");

    },
    // End -- search Activity Log

    // Start -- Component Load
    ActivityLogsComponent: function (ProfileName, PatientId, DateAndTime, UserId, pageNumber, rowsPerPage) {
        MIPS_AdminPreferenceGroupDetail.searchActivityLogComponents_DBCall(ProfileName, PatientId, DateAndTime, UserId, pageNumber, rowsPerPage).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {


                var ActivityLogUser_JSON = JSON.parse(response.ActivityLogCompLoad_JSON);
                MIPS_AdminPreferenceGroupDetail.GridLoad(ActivityLogUser_JSON, response.ActivityLogCompCount, "User", " #pnlAuditbleEventsActivityLog #dgvUser");
                var TableControl = MIPS_AdminPreferenceGroupDetail.params.PanelID + " #dgvUser";
                var PagingPanelControlID = MIPS_AdminPreferenceGroupDetail.params.PanelID + " #dgvActivityLogComp_Paging";
                var ClassControlName = "AuditbleEvents_ActivityLog";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.ActivityLogCompCount, pageNumber, 15, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, pageNumber, resultPerPage) {
                    MIPS_AdminPreferenceGroupDetail.ActivityLogsComponent($('#pnlAuditbleEventsActivityLog #dgvNewEntry tr.active #rowprofilename').text(), $('#pnlAuditbleEventsActivityLog #dgvNewEntry tr.active #rowpatid').text(), $('#pnlAuditbleEventsActivityLog #dgvNewEntry tr.active #rowdateid').text(), $('#pnlAuditbleEventsActivityLog #dgvNewEntry tr.active #rowuserid').text(), pageNumber, resultPerPage);
                }), 10);
                if (ActivityLogUser_JSON.length > 0) {
                    $('#pnlAuditbleEventsActivityLog #dgvUser tbody tr:first').click();
                }
                else {
                    MIPS_AdminPreferenceGroupDetail.GridLoad(ActivityLogUser_JSON, response.ActivityLogCompCount, "Changes", " #pnlAuditbleEventsActivityLog #dgvChanges");
                }

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });
    },
    searchActivityLogComponents_DBCall: function (ProfileName, PatientId, DateAndTime, UserId, pageNumber, rowsPerPage) {
        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 15;
        }
        var objData = new Object();
        //objData["AuditReportId"] = AuditReportId;
        objData["PageNumber"] = pageNumber;
        objData["RowsPerPage"] = rowsPerPage;
        objData["ProfileName"] = ProfileName;
        objData["PatientId"] = PatientId;
        objData["DateAndTime"] = DateAndTime;
        objData["UserId"] = UserId;
        objData["commandType"] = "auditbleeventsactivitylogcomponents";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "AuditbleEventsActivityLog", "AuditbleEventsActivityLog");
    },
    // End -- Components load
    // Start -- ActivityLog Changes
    ActivityLogsChanges: function (ColumnKeyId, ProfileName, DateAndTime, pageNumber, rowsPerPage) {
        MIPS_AdminPreferenceGroupDetail.searchActivityLogChanges_DBCall(ColumnKeyId, ProfileName, DateAndTime, pageNumber, rowsPerPage).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {


                var ActivityLogUser_JSON = JSON.parse(response.ActivityLogChangesLoad_JSON);
                MIPS_AdminPreferenceGroupDetail.GridLoad(ActivityLogUser_JSON, response.ActivityLogChangesCount, "Changes", " #pnlAuditbleEventsActivityLog #dgvChanges");
                var TableControl = MIPS_AdminPreferenceGroupDetail.params.PanelID + " #dgvChanges";
                var PagingPanelControlID = MIPS_AdminPreferenceGroupDetail.params.PanelID + " #dgvActivityLogChanges_Paging";
                var ClassControlName = "AuditbleEvents_ActivityLog";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.ActivityLogChangesCount, pageNumber, 15, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, pageNumber, resultPerPage) {
                    MIPS_AdminPreferenceGroupDetail.ActivityLogsChanges($('#pnlAuditbleEventsActivityLog #dgvUser tr.active #colkey').text(), $('#pnlAuditbleEventsActivityLog #dgvUser tr.active #profName').text(), $('#pnlAuditbleEventsActivityLog #dgvUser tr.active #datetime').text(), pageNumber, resultPerPage);
                }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });
    },
    searchActivityLogChanges_DBCall: function (ColumnKeyId, ProfileName, DateAndTime, pageNumber, rowsPerPage) {
        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 15;
        }
        var objData = new Object();
        //objData["AuditReportId"] = AuditReportId;
        objData["PageNumber"] = pageNumber;
        objData["RowsPerPage"] = rowsPerPage;
        objData["ColumnKeyId"] = ColumnKeyId;
        objData["DateAndTime"] = DateAndTime;
        objData["ProfileName"] = ProfileName;
        objData["commandType"] = "auditbleeventsactivitylogChanges";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "AuditbleEventsActivityLog", "AuditbleEventsActivityLog");

    },
    // ENd -- ActivityLog Changes
    GridLoad: function (list) {
        var gridId = "#dgvMIPSAdminProvider";
        $(gridId + " tbody").empty();
        $(gridId).dataTable().fnDestroy();
        $(gridId + " tbody").find("tr").remove();

        var emptyTableMsg = "No Record Found";

        if (list != null && list.length > 0) {
            var firstRowId = "";

            $.each(list, function (i, item) {

                var $row = $('<tr/>');
                var _rowId = "";
                var emptyTableMsg = "";

                _rowId = "dgvMIPSAdminProvider_row" + i;
                if (item.ColumnKeyId == null || item.ColumnKeyId == "") {
                    temp_colkey = null;
                }
                else {
                    temp_colkey = item.ColumnKeyId;
                }
                $row.append('<td id= "MIPSProviderId" style="display:none;">' + item.MIPSProviderId + '</td><td>' + item.LastName + '</td><td>' + item.FirstName + '</td><td>' + item.Specialty + '</td><td>' + item.NPI + '</td><td>' + item.PracticeName + '</td><td>' + item.MIPSEligibilityStatus + '</td><td>' + item.JoiningDate + '</td><td>' + item.LeavingDate + '</td>');
                $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); MIPS_AdminPreferenceGroupDetail.previewProviderPreferences('" + item.MIPSProviderId + "');");

                $(gridId + " tbody").last().append($row);
            });
        }
        else {
            $('#divdgvMIPSAdminProviderPaging').css("display", "none");
            $(gridId).DataTable({
                "language": {
                    "emptyTable": emptyTableMsg
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [2] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#' + MIPS_AdminPreferenceGroupDetail.params["PanelID"] + ' #dgvMIPSAdminProvider'))
            ;
        else {
            $('#' + MIPS_AdminPreferenceGroupDetail.params["PanelID"] + ' #dgvMIPSAdminProvider').DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "searching": true, "bFilter": false, "order": [[3, "desc"]] }); // to remove records per page dropdown
        }

    },
    previewProviderPreferences: function(providerId){

        var params = [];
        params["PreferenceId"] = null;
        params["ProviderId"] = providerId;
        params["mode"] = "Edit";
        params["FromAdmin"] = Admin_Provider.params["FromAdmin"];
        params["ParentCtrl"] = 'MIPS_AdminPreferenceGroupDetail';
        params["From"] = "GroupDetail";
        LoadActionPan('iTrack_AdminIPPreference', params);


    },
    UnLoad: function () {
        UnloadActionPan(MIPS_AdminPreferenceGroupDetail.params["ParentCtrl"], "MIPS_AdminPreferenceGroup");

    },
}