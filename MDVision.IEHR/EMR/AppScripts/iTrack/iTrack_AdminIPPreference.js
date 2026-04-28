iTrack_AdminIPPreference = {
    params: [],
    bIsFirstLoad: true,
    RequireSubmit: true,
    ActualVisitID: null,
    specialityCheckedIds: [],
    SpecialtyIds: '',
    ActualChargeCaptureID: null,
    IsChargeCapture: null,
    Load: function (params) {

        iTrack_AdminIPPreference.params = params;
        EMRUtility.SwicthWidgetInializatoin();
        iTrack_AdminIPPreference.LoadAllAutocomplete();
        ////  iTrack_AdminIPPreference.BindProvider();
        iTrack_AdminIPPreference.displayCheckBox();
        if (iTrack_AdminIPPreference.bIsFirstLoad) {
            iTrack_AdminIPPreference.LoadGroupLookup();
            $('#switchActiveACI').prev('div').removeClass('off');
            $('#switchActiveACI').prev('div').addClass('on');
            $('#switchActiveIA').prev('div').removeClass('off');
            $('#switchActiveIA').prev('div').addClass('on');
            iTrack_AdminIPPreference.bIsFirstLoad = false;
            iTrack_AdminIPPreference.disableControls();
            var self = $('#pnlIndividualProvider');
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#IstEntityId").attr('disabled', 'disabled');

            }
            $('#pnlIndividualProvider #divIneligibile input:checkbox[name="IneligibleReason"]').on('change', function () {
                $('#pnlIndividualProvider #divIneligibile input:checkbox[name="IneligibleReason"]').not(this).prop('checked', false);
                iTrack_AdminIPPreference.displayCheckBox();
            });
            EMRUtility.CreateYearViewDatePicker(iTrack_AdminIPPreference.params["PanelID"] + ' #dtpPerformanceYear',null, true);
            self.loadDropDowns(true).done(function () {
                $("#" + iTrack_AdminIPPreference.params.PanelID + ' #dtpPerformanceYear').datepicker('setDate', new Date())
                if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {

                    iTrack_AdminIPPreference.ValidateMeasureGroup();

                    $('#pnlIndividualProvider #lstEntityIdGroup').val(globalAppdata["SeletedEntityId"]);
                    $('#pnlIndividualProvider #frmIndividualProvider').data('bootstrapValidator').enableFieldValidators('DateFromQuality', false);
                    $('#pnlIndividualProvider #frmIndividualProvider').data('bootstrapValidator').enableFieldValidators('DateToQuality', false);
                    //$('#pnlIndividualProvider #frmIndividualProvider').data('bootstrapValidator').enableFieldValidators('DateFromACI', false);
                    //$('#pnlIndividualProvider #frmIndividualProvider').data('bootstrapValidator').enableFieldValidators('DateToACI', false);
                    //$('#pnlIndividualProvider #frmIndividualProvider').data('bootstrapValidator').enableFieldValidators('DateFromIA', false);
                    //$('#pnlIndividualProvider #frmIndividualProvider').data('bootstrapValidator').enableFieldValidators('DateToIA', false);
                    $('#pnlIndividualProvider #switchActiveIA').prop('checked', true);
                    $('#pnlIndividualProvider #switchActiveACI').prop('checked', true);
                    $('#pnlIndividualProvider #frmIndividualProvider').data('bootstrapValidator').enableFieldValidators('DateFromCost', false);
                    $('#pnlIndividualProvider #frmIndividualProvider').data('bootstrapValidator').enableFieldValidators('DateToCost', false);
                    $('#pnlIndividualProvider #frmIndividualProvider').data('bootstrapValidator').enableFieldValidators('JoiningReason', false);
                    $('#pnlIndividualProvider #frmIndividualProvider').data('bootstrapValidator').enableFieldValidators('JoiningDate', false);
                    $('#pnlIndividualProvider #frmIndividualProvider').data('bootstrapValidator').enableFieldValidators('GroupName', false);
                    $('#pnlIndividualProvider #ddlMemberProvider').multiselect({
                        includeSelectAllOption: true,
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                    });

                    iTrack_AdminIPPreference.LoadPracticLookup();
                    //iTrack_AdminIPPreference.LoadMIPSProviders();            //iTrack_AdminIPPreference.Search_IndividualProvider();
                    iTrack_AdminIPPreference.LoadGroupCatLookup();

                    iTrack_AdminIPPreference.LoadIneligibleReasonLookup();
                    iTrack_AdminIPPreference.LoadParticipatingReasonLookup();
                    iTrack_AdminIPPreference.loadIndividualPreferences(iTrack_AdminIPPreference.params.PreferenceId, iTrack_AdminIPPreference.params.ProviderId);
                    //self.find("*#lstEntityIdGroup").val(globalAppdata["SeletedEntityId"]);
                }
                //iTrack_AdminIPPreference.UserSearch('0');
            });

        }


    },
    displayCheckBox: function () {
        if ($('#chkIneligibileReason3').prop('checked')) {
            $('#txtIneligibileReason').removeClass('hidden');
        }
        else {
            $('#txtIneligibileReason').addClass('hidden');
        }
    },
    disableControls: function () {


    },
    loadIndividualPreferences: function (preferenceId, providerId) {

        iTrack_AdminIPPreference.loadIndividualPreferences_DBCall(preferenceId, providerId).done(function (response) {
            response = JSON.parse(response);

            if (response.status != false && response.IndividualProCount > 0) {

                var jsn = JSON.parse(response.IndividualProCountLoad_JSON);
                var detailjsn = JSON.parse(response.IndividualProDetailLoad_JSON);
                if (detailjsn != null && detailjsn.length > 0) {
                    $.each(detailjsn, function (i, item) {

                        if (item.IsFullYear == "True" && item.CategoryName == "Quality") { ///////change to category names and also save year also in case of full year
                            $('#switchActiveQuality').prev('.ios-switch').removeClass('on');
                            var date = new Date(item.StartDate);
                            $('#pnlIndividualProvider  #ddlQualityYeaR').val(date.getFullYear());

                        } else if (item.IsFullYear == "False" && item.CategoryName == "Quality") {
                            $('#switchActiveQuality').prev('.ios-switch').removeClass('off');
                            $('#switchActiveQuality').prev('.ios-switch').addClass('on');
                            $('#pnlIndividualProvider #divPeriodQuality').removeClass('hidden');
                            $('#pnlIndividualProvider #divYearQuality').addClass('hidden');
                            $('#pnlIndividualProvider #dtpDateFromQuality').datepicker('setDate', new Date(item.StartDate));
                            $('#pnlIndividualProvider #dtpDateToQuality').datepicker('setDate', new Date(item.EndDate));
                            $('#pnlIndividualProvider #switchActiveQuality').prop('checked', true);
                        }

                        if (item.IsFullYear == "True" && item.CategoryName == "Promoting Interoperability (Formally ACI)") {
                            $('#switchActiveACI').prev('.ios-switch').removeClass('on');
                            var date = new Date(item.StartDate);
                            $('#pnlIndividualProvider  #ddlACIYeaR').val(date.getFullYear());
                            $('#pnlIndividualProvider #divPeriodACI').addClass('hidden');
                            $('#pnlIndividualProvider #divYearACI').removeClass('hidden');

                        } else if (item.IsFullYear == "False" && item.CategoryName == "Promoting Interoperability (Formally ACI)") {
                            $('#switchActiveACI').prev('.ios-switch').removeClass('off');
                            $('#switchActiveACI').prev('.ios-switch').addClass('on');
                            $('#pnlIndividualProvider #divPeriodACI').removeClass('hidden');
                            $('#pnlIndividualProvider #divYearACI').addClass('hidden');
                            $('#pnlIndividualProvider #dtpDateFromACI').datepicker('setDate', new Date(item.StartDate));
                            $('#pnlIndividualProvider #dtpDateToACI').datepicker('setDate', new Date(item.EndDate));
                            $('#pnlIndividualProvider #switchActiveACI').prop('checked', true);
                        }

                        if (item.IsFullYear == "True" && item.CategoryName == "IA (Improvement Activities)") {
                            $('#switchActiveIA').prev('.ios-switch').removeClass('on');
                            var date = new Date(item.StartDate);
                            $('#pnlIndividualProvider #ddlIAYeaR').val(date.getFullYear());
                            $('#pnlIndividualProvider #divPeriodIA').addClass('hidden');
                            $('#pnlIndividualProvider #divYearIA').removeClass('hidden');
                        } else if (item.IsFullYear == "False" && item.CategoryName == "IA (Improvement Activities)") {
                            $('#switchActiveIA').prev('.ios-switch').removeClass('off');
                            $('#switchActiveIA').prev('.ios-switch').addClass('on');
                            $('#pnlIndividualProvider #divPeriodIA').removeClass('hidden');
                            $('#pnlIndividualProvider #divYearIA').addClass('hidden');
                            $('#pnlIndividualProvider #dtpDateFromIA').datepicker('setDate', new Date(item.StartDate));
                            $('#pnlIndividualProvider #dtpDateToIA').datepicker('setDate', new Date(item.EndDate));
                            $('#pnlIndividualProvider #switchActiveIA').prop('checked', true);
                        }

                        if (item.IsFullYear == "True" && item.CategoryName == "Cost") {
                            $('#switchActiveCost').prev('.ios-switch').removeClass('on');
                            var date = new Date(item.StartDate);
                            $('#pnlIndividualProvider #ddlCostYeaR').val(date.getFullYear());

                        } else if (item.IsFullYear == "False" && item.CategoryName == "Cost") {
                            $('#switchActiveCost').prev('.ios-switch').removeClass('off');
                            $('#switchActiveCost').prev('.ios-switch').addClass('on');
                            $('#pnlIndividualProvider #divPeriodCost').removeClass('hidden');
                            $('#pnlIndividualProvider #divYearCost').addClass('hidden');
                            $('#pnlIndividualProvider #dtpDateFromCost').datepicker('setDate', new Date(item.StartDate));
                            $('#pnlIndividualProvider #dtpDateToCost').datepicker('setDate', new Date(item.EndDate));
                            $('#pnlIndividualProvider #switchActiveCost').prop('checked', true);
                        }


                    });

                } else {
                    $('#pnlIndividualProvider #dtpDateFromQuality').datepicker('setDate', '');
                    $('#pnlIndividualProvider #dtpDateToQuality').datepicker('setDate', '');
                    $('#pnlIndividualProvider #dtpDateFromACI').datepicker('setDate', '');
                    $('#pnlIndividualProvider #dtpDateToACI').datepicker('setDate', '');
                    $('#pnlIndividualProvider #dtpDateFromIA').datepicker('setDate', '');
                    $('#pnlIndividualProvider #dtpDateToIA').datepicker('setDate', '');
                    $('#pnlIndividualProvider #dtpDateFromCost').datepicker('setDate', '');
                    $('#pnlIndividualProvider #dtpDateToCost').datepicker('setDate', '');
                }
                var ineligiblereasons = [];
                $.each(jsn, function (i, item) {

                    ineligiblereasons.push(item.InEligibileReason);

                });
                var self = $('#pnlIndividualProvider');
                var providerName = jsn[0].ProviderName;
                var otherineligiblereason = jsn[0].OtherComments;

                var firstName = providerName.split(' ')[1];
                var lastName = providerName.split(' ')[0];
                var specialty = jsn[0].Specialty;
                var practice = jsn[0].PracticeName;
                var NPI = jsn[0].NPI;
                var TIN = jsn[0].TIN;
                var mipseligibility = jsn[0].MIPSEligibilityStatus;
                var isReporting = jsn[0].IsReporting;
                var inEligible = jsn[0].IsEligibile;
                var reportingType = jsn[0].ReportingType;
                var reportingMethod = jsn[0].ReportingMethod;
                var reportingYear = jsn[0].ReportingYear;
                var practicetype = jsn[0].PracticeType;
                var IsActive = jsn[0].IsActive;
                var notReportingReason = jsn[0].NotReportingReason;
                var groupName = jsn[0].GroupName;
                var participatingReason = jsn[0].ParticipatingReason;
                var groupTIN = jsn[0].GroupTIN;
                var groupComments = jsn[0].GroupComments;
                var joiningDate = jsn[0].JoiningDate.replace(" 12:00:00 AM", "");
                var leavingDate = jsn[0].LeavingDate.replace(" 12:00:00 AM", "");

                $("#" + iTrack_AdminIPPreference.params.PanelID + ' #dtpPerformanceYear').datepicker('setDate', reportingYear);
                $('#pnlIndividualProvider #txtIneligibileReason').val(otherineligiblereason)
                if (joiningDate != "" && joiningDate != "Invalid Date") {
                    $('#pnlIndividualProvider #dtpJoiningDate').val(moment(joiningDate).format("MM/DD/YYYY"));
                }
                if (leavingDate != "" && leavingDate != "Invalid Date") {
                    $('#pnlIndividualProvider #dtpLeavingDate').val(moment(leavingDate).format("MM/DD/YYYY"));
                }
                $('#pnlIndividualProvider #txtLastName').val(lastName.replace(/,/g, ""));
                $('#pnlIndividualProvider #txtFirstName').val(firstName);
                $('#pnlIndividualProvider #actionPanIndividualProvider #txtNPI').val(NPI);
                $('#pnlIndividualProvider #txtProviderTIN').val(TIN);
                $('#pnlIndividualProvider #txtTIN').val(groupTIN);
                $('#pnlIndividualProvider #txtReasonComments').val(groupComments);
                if (mipseligibility == "Eligible") {
                    $('#pnlIndividualProvider #rdoEligibile').prop('checked', true);
                    $('#pnlIndividualProvider #frmIndividualProvider #divIneligibile').addClass('hidden');
                    //iTrack_AdminIPPreference.CheckEligibility('rdoEligibile', this);
                } else {
                    $('#pnlIndividualProvider #rdoIneligibile').prop('checked', true);
                    $('#pnlIndividualProvider #frmIndividualProvider #divIneligibile').removeClass('hidden');
                    //iTrack_AdminIPPreference.CheckEligibility('Ineligibile', this);
                }
                if (isReporting == "Yes") {
                    $("#switchReporting").prev('.ios-switch').removeClass('on');
                    $("#switchReporting").prev('.ios-switch').addClass('of');
                    $('#pnlIndividualProvider #switchReporting').prop('checked', false);
                } else {
                    $("#switchReporting").prev('.ios-switch').removeClass('of');
                    $("#switchReporting").prev('.ios-switch').addClass('on');
                    $('#pnlIndividualProvider #NotReporting').removeClass('hidden');
                    $('#pnlIndividualProvider #Reporting').addClass('hidden');
                    $('#pnlIndividualProvider #NotReporting #txtReason').val(notReportingReason);
                    $('#pnlIndividualProvider #switchReporting').prop('checked', true);
                }
                if (reportingType != "" && isReporting != "No") {

                    if (reportingType == "Individual") {
                        $('#pnlIndividualProvider #rdoIndividual').prop('checked', true);
                        $('#pnlIndividualProvider #divReportingForGroup').addClass('hidden');
                        $('#pnlIndividualProvider #divReportingForInd').removeClass('hidden');
                    } else {
                        $('#pnlIndividualProvider #rdoGroup').prop('checked', true);
                        $('#pnlIndividualProvider #divReportingForGroup').removeClass('hidden');
                        $('#pnlIndividualProvider #divReportingForInd').addClass('hidden');

                    }
                }
                if (reportingMethod == "MD Vision EHR") {
                    $('#pnlIndividualProvider #rdoMDVision').prop('checked', true);
                } else if (reportingMethod == "Sovereign Health Registry") {
                    $('#pnlIndividualProvider #rdoSHS').prop('checked', true);
                } else if (reportingMethod == "Sovereign QCDR") {
                    $('#pnlIndividualProvider #rdoQCDR').prop('checked', true);
                } else if (reportingMethod == "Other") {
                    $('#pnlIndividualProvider #rdoOther').prop('checked', true);
                }
                if (practicetype == "Rural") {
                    $('#pnlIndividualProvider #rdoRural').prop('checked', true);
                } else if (practicetype == "Small") {
                    $('#pnlIndividualProvider #rdoSmall').prop('checked', true);
                } else if (practicetype == "Large") {
                    $('#pnlIndividualProvider #rdoLarge').prop('checked', true);
                }
                if (IsActive == "True") {
                    $('#pnlIndividualProvider #chkIsActive').prop('checked', true);
                } else {
                    $('#pnlIndividualProvider #chkIsActive').prop('checked', false);
                }

                $('#pnlIndividualProvider #ddlSpecialty option').each(function () {

                    if ($(this).text() == specialty) {
                        $(this).attr('selected', 'selected');
                    }
                });
                $('#pnlIndividualProvider #lstPractice option').each(function () {

                    if ($(this).text() == practice) {
                        $(this).attr('selected', 'selected');
                    }
                });

                setTimeout(function () {
                    $('#pnlIndividualProvider #ddlJoiningReason option').each(function () {

                        if ($(this).text() == participatingReason) {
                            $(this).attr('selected', 'selected');
                        }
                    });
                }, 500)
                $('#pnlIndividualProvider #ddlGroupName option').each(function () {

                    if ($(this).text() == groupName) {
                        $(this).attr('selected', 'selected');
                        iTrack_AdminIPPreference.loadGroupData();
                    }
                });
                if (mipseligibility != "Eligible") {

                    $.each(ineligiblereasons, function (i, val) {

                        if (val == "Does Not meet low-volume threshold for 2018") {
                            $('#pnlIndividualProvider #chkIneligibileReason1').prop('checked', true);
                        }
                        if (val == "Newly enrolled in Medicare") {
                            $('#pnlIndividualProvider #chkIneligibileReason5').prop('checked', true);
                        }
                        if (val == "Partially Qualifying APM Participant (Partial QP)") {
                            $('#pnlIndividualProvider #chkIneligibileReason6').prop('checked', true);
                        }
                        if (val == "Qualifying APM Participant (QP)") {
                            $('#pnlIndividualProvider #chkIneligibileReason2').prop('checked', true);
                        }
                        if (val == "Other") {
                            $('#pnlIndividualProvider #chkIneligibileReason3').prop('checked', true);
                        }
                    });
                }
            }
            if (iTrack_AdminIPPreference.params['From'] != null && iTrack_AdminIPPreference.params['From'] == "GroupDetail") {
                $('#pnlIndividualProvider #tblproviderDetail').addClass('disableAll');
            }
        });

    },
    loadGroupData: function () {

        iTrack_AdminIPPreference.loadGroupData_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $('#pnlIndividualProvider #ddlMemberProvider').multiselect("rebuild");
                var dta = JSON.parse(response.IndividualProCountLoad_JSON)
                if (dta.Groups && dta.Groups.length > 0) {

                    var tin = dta.Groups[0].TIN;
                    var comments = dta.Groups[0].JoiningComments;
                    var joiningDate = dta.Groups[0].JoiningDate;
                    //joiningDate = new Date(joiningDate);
                    var leavingDate = dta.Groups[0].LeavingDate;
                    //leavingDate = new Date(leavingDate);
                    // $('#pnlIndividualProvider #ddlGroupName option').val(tin);
                    $('#pnlIndividualProvider #txtTIN').val(tin);
                    if (joiningDate != "" && joiningDate != "Invalid Date") {
                        //  $('#pnlIndividualProvider #dtpJoiningDate').val(joiningDate);
                    }
                    if (leavingDate != "" && leavingDate != "Invalid Date") {
                        // $('#pnlIndividualProvider #dtpLeavingDate').val(leavingDate);
                    }
                    // $('#pnlIndividualProvider #txtReasonComments').val(comments);

                }
                var members = "";
                if (dta.GroupDetail && dta.GroupDetail.length > 0) {
                    $.each(dta.GroupDetail, function (i, item) {

                        members += "," + item.ProviderId;

                    });
                    var memberslist = members.split(',');
                    $('#pnlIndividualProvider #ddlMemberProvider').val(memberslist);
                    $('#pnlIndividualProvider #ddlMemberProvider').multiselect("refresh");
                }
                $("ul.multiselect-container li").each(function (i, item) {

                    if (!$(this).hasClass('active')) {
                        $(this).remove();
                    }


                });


            }
        });
    },
    SelectGroupPreferences: function () {
        iTrack_AdminIPPreference.SelectGroupPreferences_DBCall().done(function (response) {
            response = JSON.parse(response);

            if (response.status != false) {
                var list = JSON.parse(response.IndividualProCountLoad_JSON);
                $('#pnlIndividualProvider #txtTIN').val(list.Groups[0].TIN);
                $('#pnlIndividualProvider #txtGroupName').val(list.Groups[0].GroupName);
                $('#pnlIndividualProvider #frmIndividualProvider #dtpPerformanceYear').datepicker('setDate', list.Groups[0].ReportingYear);
                $('#pnlIndividualProvider #frmIndividualProvider #ddlPractice').val(list.Groups[0].PracticeId);
                if (list.Groups[0].PracticeType == "Rural Practice") {
                    $("#rdoRural").prop('checked', true)
                } else if (list.Groups[0].PracticeType == "Small Practice(15 or fewer clinicians)") {
                    $("#rdoSmall").prop('checked', true)
                } else if (list.Groups[0].PracticeType == "Large Practice") {
                    $("#rdoLarge").prop('checked', true)
                }
                if (list.Groups[0].IsActive == "True") {
                    $("#chkIsActive").prop('checked', true)
                } else {
                    $("#chkIsActive").prop('checked', false)
                }
                if (list.Groups[0].IsReporting == "True") {
                    $.each(list.GroupDetail, function (i, item) {

                        if (item.ReportingCat == "Quality") {
                            if (item.IsFullYear == "False") {
                                $("#switchActiveQuality").prev('.ios-switch').addClass('on');
                                $("#switchActiveQuality").prev('.ios-switch').removeClass('of');
                                $("#divPeriodQuality").removeClass('hidden');
                                $("#divYearQuality").addClass('hidden');
                                $('#pnlIndividualProvider  #dtpDateFromQuality').datepicker('setDate', item.DateFrom);
                                $('#pnlIndividualProvider  #dtpDateToQuality').datepicker('setDate', item.DateTo);
                                $('#pnlIndividualProvider #switchActiveQuality').prop('checked', true);
                            } else {
                                var date = new Date(item.DateFrom);
                                $('#pnlIndividualProvider  #ddlQualityYeaR').val(date.getFullYear());
                            }
                        } else if (item.ReportingCat == "Promoting Interoperability (Formally ACI)") {
                            if (item.IsFullYear == "False") {
                                $("#switchActiveACI").prev('.ios-switch').addClass('on');
                                $("#switchActiveACI").prev('.ios-switch').removeClass('of');
                                $("#divPeriodACI").removeClass('hidden');
                                $("#divYearACI").addClass('hidden');
                                $('#pnlIndividualProvider  #dtpDateFromACI').datepicker('setDate', item.DateFrom);
                                $('#pnlIndividualProvider  #dtpDateToACI').datepicker('setDate', item.DateTo);
                                $('#pnlIndividualProvider #switchActiveACI').prop('checked', true);
                            } else {
                                var date = new Date(item.DateFrom);
                                $('#pnlIndividualProvider  #ddlACIYeaR').val(date.getFullYear());
                            }
                        } else if (item.ReportingCat == "IA (Improvement Activities)") {
                            if (item.IsFullYear == "False") {
                                $("#switchActiveIA").prev('.ios-switch').addClass('on');
                                $("#switchActiveIA").prev('.ios-switch').removeClass('of');
                                $("#divPeriodIA").removeClass('hidden');
                                $("#divYearIA").addClass('hidden');
                                $('#pnlIndividualProvider  #dtpDateFromIA').datepicker('setDate', item.DateFrom);
                                $('#pnlIndividualProvider  #dtpDateToIA').datepicker('setDate', item.DateTo);
                                $('#pnlIndividualProvider #switchActiveIA').prop('checked', true);
                            } else {
                                var date = new Date(item.DateFrom);
                                $('#pnlIndividualProvider  #ddlIAYeaR').val(date.getFullYear());
                            }
                        } else if (item.ReportingCat == "Cost") {
                            if (item.IsFullYear == "False") {
                                $("#switchActiveCost").prev('.ios-switch').addClass('on');
                                $("#switchActiveCost").prev('.ios-switch').removeClass('of');
                                $("#divPeriodCost").removeClass('hidden');
                                $("#divYearCost").addClass('hidden');
                                $('#pnlIndividualProvider  #dtpDateFromCost').datepicker('setDate', item.DateFrom);
                                $('#pnlIndividualProvider  #dtpDateToCost').datepicker('setDate', item.DateTo);
                                $('#pnlIndividualProvider #switchActiveCost').prop('checked', true);
                            } else {
                                var date = new Date(item.DateFrom);
                                $('#pnlIndividualProvider  #ddlCostYeaR').val(date.getFullYear());
                            }
                        }

                    });


                } else {
                    $("#switchReporting").prev('.ios-switch').addClass('on');
                    $("#switchReporting").prev('.ios-switch').removeClass('of');
                    $("#NotReporting").removeClass('hidden');
                    $("#Reporting").addClass('hidden');
                    $('#pnlIndividualProvider #txtReason').val(list.Groups[0].ReportingReason);
                    $('#pnlIndividualProvider #switchReporting').prop('checked', true);

                }

            }
            else {

            }

        });

    },
    SelectGroupPreferences_DBCall: function () {
        var objData = new Object();
        objData["GroupId"] = iTrack_AdminIPPreference.params.GroupId;
        objData["commandType"] = "selectmipsgrouppreferences";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "iTrack", "iTrackAdmin");

    },
    loadGroupData_DBCall: function () {
        var objData = new Object();
        objData["GroupName"] = $('#pnlIndividualProvider #ddlGroupName option:selected').text();
        objData["EntityId"] = "100";
        objData["PageNumber"] = 1;
        objData["RowsPerPage"] = 15;
        objData["commandType"] = "searchmimpsgrouppreferences";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "iTrack", "iTrackAdmin");

    },
    loadIndividualPreferences_DBCall: function (preferenceId, providerId) {

        var objData = new Object();
        objData["ObjectId"] = preferenceId;
        objData["ProviderId"] = providerId;
        objData["commandType"] = "selectindividualpreferences";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "iTrack", "iTrackAdmin");

    },
    GridTabChange: function (NoteStatus) {
        iTrack_AdminIPPreference.LoadMIPSProviders(NoteStatus);
    },
    LoadPracticLookup: function () {
        iTrack_AdminIPPreference.LoadPracticLookup_DBCall().done(function (response) {
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
        iTrack_AdminIPPreference.LoadGroupCatLookup_DBCall().done(function (response) {
            response = JSON.parse(response);

            if (response.status != false) {

                iTrack_AdminIPPreference['GroupCatLookUp'] = JSON.parse(response.IndividualProCountLoad_JSON);
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
    LoadIneligibleReasonLookup: function () {
        iTrack_AdminIPPreference.LoadIneligibleReasonLookup_DBCall().done(function (response) {
            response = JSON.parse(response);

            if (response.status != false) {

                iTrack_AdminIPPreference['IneligibleReasonLookUp'] = JSON.parse(response.IndividualProCountLoad_JSON);
            }
            else {

            }

        });

    },
    LoadParticipatingReasonLookup: function () {
        iTrack_AdminIPPreference.LoadParticipatingReasonLookup_DBCall().done(function (response) {
            response = JSON.parse(response);

            if (response.status != false) {

                //iTrack_AdminIPPreference['ParticipatingReasonLookUp'] = JSON.parse(response.IndividualProCountLoad_JSON);
                var list = JSON.parse(response.IndividualProCountLoad_JSON);
                $.each(list, function (i, item) {
                    if ($('#frmIndividualProvider #ddlJoiningReason option[value=' + item.LookupId + ']').length <= 0) {
                        $('#frmIndividualProvider #ddlJoiningReason').append('<option value=' + item.LookupId + ' >' + item.Name + '</option>');
                    }
                    if (item.Name == "Other") {
                        $('#frmIndividualProvider #ddlJoiningReason option[value=' + item.LookupId + ']').attr('selected', 'selected');
                    }
                });
            }
            else {

            }

        });

    },
    LoadIneligibleReasonLookup_DBCall: function () {
        var objData = new Object();
        objData["commandType"] = "loadineligiblereasonlookup";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "iTrack", "iTrackAdmin");

    },
    LoadParticipatingReasonLookup_DBCall: function () {
        var objData = new Object();
        objData["commandType"] = "loadparticipatingreasonlookup";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "iTrack", "iTrackAdmin");

    },
    LoadMIPSProviders: function (tab, pageNumber, resultPerPage) {
        iTrack_AdminIPPreference.LoadMIPSProviders_DBCall(tab, pageNumber, resultPerPage).done(function (response) {
            response = JSON.parse(response);

            if (response.status != false && response.IndividualProCount > 0) {

                var list = JSON.parse(response.IndividualProCountLoad_JSON);
                iTrack_AdminIPPreference.GridLoad(list);
                var TableControl = iTrack_AdminIPPreference.params.PanelID + " #dgvMIPSAdminProvider";
                var PagingPanelControlID = iTrack_AdminIPPreference.params.PanelID + " #divdgvMIPSAdminProviderPaging";
                var ClassControlName = "MIPS_AdminPreferenceGroupDetail";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.IndividualProCount, pageNumber, 15, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, pageNumber, resultPerPage) {
                    iTrack_AdminIPPreference.LoadMIPSProviders(null, pageNumber, resultPerPage);
                }), 10);
            }
            else {
                iTrack_AdminIPPreference.GridLoad();
            }

        });

    },
    LoadMIPSProviders_DBCall: function (tab, pageNumber, resultPerPage) {
        if (!tab || tab == "All Providers") {
            tab = null;
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
            $('#pnlIndividualProvider #frmIndividualProvider #divIneligibile').removeClass('hidden');
            $('#pnlIndividualProvider #rdoIndividual').addClass('disableAll');
            $('#pnlIndividualProvider #rdoIndividual').prop('checked', false);
            $('#pnlIndividualProvider #rdoGroup').trigger('click');
        }
        else {
            $('#pnlIndividualProvider #frmIndividualProvider #divIneligibile').addClass('hidden');
            $('#pnlIndividualProvider #rdoIndividual').removeClass('disableAll');

        }
    },
    SelectPerriod: function (obj, div) {
        if (obj.checked == true) {
            $('#pnlIndividualProvider #frmIndividualProvider #divYear' + div).addClass('hidden');
            $('#pnlIndividualProvider #frmIndividualProvider #divPeriod' + div).removeClass('hidden');
            $('#pnlIndividualProvider #frmIndividualProvider').data('bootstrapValidator').enableFieldValidators('DateFrom' + div, true);
            $('#pnlIndividualProvider #frmIndividualProvider').data('bootstrapValidator').enableFieldValidators('DateTo' + div, true);
            $('#pnlIndividualProvider #frmIndividualProvider').data('bootstrapValidator').enableFieldValidators(div + 'YeaR', false);
        } else {
            $('#pnlIndividualProvider #frmIndividualProvider #divYear' + div).removeClass('hidden');
            $('#pnlIndividualProvider #frmIndividualProvider #divPeriod' + div).addClass('hidden');
            $('#pnlIndividualProvider #frmIndividualProvider').data('bootstrapValidator').enableFieldValidators('DateFrom' + div, false);
            $('#pnlIndividualProvider #frmIndividualProvider').data('bootstrapValidator').enableFieldValidators('DateTo' + div, false);
            $('#pnlIndividualProvider #frmIndividualProvider').data('bootstrapValidator').enableFieldValidators(div + 'YeaR', true);
        }
        iTrack_AdminIPPreference.endabelDisableValidation(div);
    },
    endabelDisableValidation: function (div) {


    },
    SelectReportingDiv: function (obj) {
        if (obj.checked == true) {
            $('#pnlIndividualProvider #frmIndividualProvider #Reporting').addClass('hidden');
            $('#pnlIndividualProvider #frmIndividualProvider #NotReporting').removeClass('hidden');
            $('#pnlIndividualProvider #frmIndividualProvider').data('bootstrapValidator').enableFieldValidators('Reason', true);
        } else {
           
            $('#pnlIndividualProvider #frmIndividualProvider #Reporting').removeClass('hidden');
            $('#pnlIndividualProvider #frmIndividualProvider #NotReporting').addClass('hidden');
            $('#pnlIndividualProvider #rdoIndividual').prop('checked', true);
            $('#pnlIndividualProvider #frmIndividualProvider').data('bootstrapValidator').enableFieldValidators('Reason', false);
            if ($('#pnlIndividualProvider #rdoIndividual').hasClass('disableAll')) {
                $('#pnlIndividualProvider #rdoIndividual').prop('checked', false);
                $('#pnlIndividualProvider #rdoGroup').trigger('click');
            }
        }

    },
    LoadAllAutocomplete: function () {
        utility.CreateDatePicker("pnlIndividualProvider #frmIndividualProvider #dtpJoiningDate",
  function (ev) {

      if ($('#pnlIndividualProvider #frmIndividualProvider').data("bootstrapValidator") != null) {
          $('#pnlIndividualProvider #frmIndividualProvider').bootstrapValidator('revalidateField', 'JoiningDate');
      }

  }, true);
        //$('#pnliTrackAdminIPPreference #frmIndividualProvider #dtpDateToQuality').datepicker('setEndDate', new Date());

        utility.CreateDatePicker("pnlIndividualProvider #frmIndividualProvider #dtpLeavingDate",
    function (ev) {

        if ($('#pnlIndividualProvider #frmIndividualProvider').data("bootstrapValidator") != null) {
            $('#pnlIndividualProvider #frmIndividualProvider').bootstrapValidator('revalidateField', 'LeavingDate');

        }

        //on-change callback method
    }, false);
        //$('#pnliTrackAdminIPPreference #frmAuditbleEventsActivityLog #dtpDateFrom').datepicker('setEndDate', new Date());
        utility.ValidateFromToDate('pnlIndividualProvider', 'dtpJoiningDate', 'dtpLeavingDate', true, null, null, true);

        // For Quality
        utility.CreateDatePicker("pnlIndividualProvider #frmIndividualProvider #dtpDateToQuality",
      function (ev) {

          if ($('#pnlIndividualProvider #frmIndividualProvider').data("bootstrapValidator") != null) {
              $('#pnlIndividualProvider #frmIndividualProvider').bootstrapValidator('revalidateField', 'DateToQuality');
          }

      }, true);
        //$('#pnliTrackAdminIPPreference #frmIndividualProvider #dtpDateToQuality').datepicker('setEndDate', new Date());

        utility.CreateDatePicker("pnlIndividualProvider #frmIndividualProvider #dtpDateFromQuality",
    function (ev) {

        if ($('#pnlIndividualProvider #frmIndividualProvider').data("bootstrapValidator") != null) {
            $('#pnlIndividualProvider #frmIndividualProvider').bootstrapValidator('revalidateField', 'DateFromQuality');

        }

        //on-change callback method
    }, true);
        //$('#pnliTrackAdminIPPreference #frmAuditbleEventsActivityLog #dtpDateFrom').datepicker('setEndDate', new Date());
        utility.ValidateFromToDate('pnlIndividualProvider', 'dtpDateFromQuality', 'dtpDateToQuality', true, null, null, true);
        // For ACI
        utility.CreateDatePicker("pnlIndividualProvider #frmIndividualProvider #dtpDateToACI",
    function (ev) {

        if ($('#pnlIndividualProvider #frmIndividualProvider').data("bootstrapValidator") != null) {
            $('#pnlIndividualProvider #frmIndividualProvider').bootstrapValidator('revalidateField', 'DateToACI');
        }

    }, true);
        //$('#pnliTrackAdminIPPreference #frmIndividualProvider #dtpDateToQuality').datepicker('setEndDate', new Date());

        utility.CreateDatePicker("pnlIndividualProvider #frmIndividualProvider #dtpDateFromACI",
    function (ev) {

        if ($('#pnlIndividualProvider #frmIndividualProvider').data("bootstrapValidator") != null) {
            $('#pnlIndividualProvider #frmIndividualProvider').bootstrapValidator('revalidateField', 'DateFromACI');

        }

        //on-change callback method
    }, true);
        //$('#pnliTrackAdminIPPreference #frmAuditbleEventsActivityLog #dtpDateFrom').datepicker('setEndDate', new Date());
        utility.ValidateFromToDate('pnlIndividualProvider', 'dtpDateFromACI', 'dtpDateToACI', true, null, null, true);
        // For IA
        utility.CreateDatePicker("pnlIndividualProvider #frmIndividualProvider #dtpDateToIA",
    function (ev) {

        if ($('#pnlIndividualProvider #frmIndividualProvider').data("bootstrapValidator") != null) {
            $('#pnlIndividualProvider #frmIndividualProvider').bootstrapValidator('revalidateField', 'DateToIA');
        }

    }, true);
        //$('#pnliTrackAdminIPPreference #frmIndividualProvider #dtpDateToQuality').datepicker('setEndDate', new Date());

        utility.CreateDatePicker("pnlIndividualProvider #frmIndividualProvider #dtpDateFromIA",
    function (ev) {

        if ($('#pnlIndividualProvider #frmIndividualProvider').data("bootstrapValidator") != null) {
            $('#pnlIndividualProvider #frmIndividualProvider').bootstrapValidator('revalidateField', 'DateFromIA');

        }

        //on-change callback method
    }, true);
        //$('#pnliTrackAdminIPPreference #frmAuditbleEventsActivityLog #dtpDateFrom').datepicker('setEndDate', new Date());
        utility.ValidateFromToDate('pnlIndividualProvider', 'dtpDateFromIA', 'dtpDateToIA', true, null, null, true);
        // For Cost
        utility.CreateDatePicker("pnlIndividualProvider #frmIndividualProvider #dtpDateToCost",
    function (ev) {

        if ($('#pnlIndividualProvider #frmIndividualProvider').data("bootstrapValidator") != null) {
            $('#pnlIndividualProvider #frmIndividualProvider').bootstrapValidator('revalidateField', 'DateToCost');
        }

    }, true);
        //$('#pnliTrackAdminIPPreference #frmIndividualProvider #dtpDateToQuality').datepicker('setEndDate', new Date());

        utility.CreateDatePicker("pnlIndividualProvider #frmIndividualProvider #dtpDateFromCost",
    function (ev) {

        if ($('#pnlIndividualProvider #frmIndividualProvider').data("bootstrapValidator") != null) {
            $('#pnlIndividualProvider #frmIndividualProvider').bootstrapValidator('revalidateField', 'DateFromCost');

        }

        //on-change callback method
    }, true);
        //$('#pnliTrackAdminIPPreference #frmAuditbleEventsActivityLog #dtpDateFrom').datepicker('setEndDate', new Date());
        utility.ValidateFromToDate('frmIndividualProvider', 'dtpDateFromCost', 'dtpDateToCost', true, null, null, true);
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
        var Ctrl = $("#pnlIndividualProvider #frmIndividualProvider #txtProvider");
        var hfCtrl = $("#pnlIndividualProvider #frmIndividualProvider #hfProvider");
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, null);
    },
    SelectIndiualOrGroup: function (obj, id) {
        if (id == "Group" && obj.checked == true) {
            $('#pnlIndividualProvider #divReportingForGroup').removeClass('hidden');
            $('#pnlIndividualProvider #divReportingForInd').addClass('hidden');
            $('#pnlIndividualProvider #frmIndividualProvider').data('bootstrapValidator').enableFieldValidators('JoiningReason', true);
            $('#pnlIndividualProvider #frmIndividualProvider').data('bootstrapValidator').enableFieldValidators('JoiningDate', true);
            $('#pnlIndividualProvider #frmIndividualProvider').data('bootstrapValidator').enableFieldValidators('GroupName', true);
        }
        else {
            $('#pnlIndividualProvider #divReportingForGroup').addClass('hidden');
            $('#pnlIndividualProvider #divReportingForInd').removeClass('hidden');
            $('#pnlIndividualProvider #frmIndividualProvider').data('bootstrapValidator').enableFieldValidators('JoiningReason', false);
            $('#pnlIndividualProvider #frmIndividualProvider').data('bootstrapValidator').enableFieldValidators('JoiningDate', false);
            $('#pnlIndividualProvider #frmIndividualProvider').data('bootstrapValidator').enableFieldValidators('GroupName', false);
        }
    },
    LoadGroupLookup: function () {
        iTrack_AdminIPPreference.LoadGroupLookup_DBCall().done(function (response) {
            response = JSON.parse(response);

            if (response.status != false) {
                var list = JSON.parse(response.IndividualProCountLoad_JSON);
                $.each(list, function (i, item) {
                    $('#frmIndividualProvider #ddlGroupName').append('<option value=' + item.GroupId + ' >' + item.GroupName + '</option>')
                });




            }
            else {

            }

        });

    },
    LoadGroupLookup_DBCall: function () {
        var objData = new Object();
        objData["EntityId"] = globalAppdata.SeletedEntityId;
        objData["commandType"] = "loadgroupnamelookup";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "iTrack", "iTrackAdmin");

    },
    // Zia Mehmood
    //Validation function
    ValidateMeasureGroup: function () {
        $('#pnlIndividualProvider #frmIndividualProvider').bootstrapValidator('destroy');
        $('#pnlIndividualProvider #frmIndividualProvider')
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
                  GroupName: {
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
                  JoiningDate: {
                      group: '.col-sm-2',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  JoiningReason: {
                      group: '.col-sm-2',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },

              }
          }).on('error.form.bv', function (e) {
              // Prevent form submission
              e.preventDefault();

          })
       .on('success.form.bv', function (e) {


           e.preventDefault();
           iTrack_AdminIPPreference.saveMIPSPreferencesGroup();
       });
    },
    // End validation function
    saveMIPSPreferencesGroup: function () {
        iTrack_AdminIPPreference.saveMIPSPreferencesGroup_DBCall().done(function (response) {
            response = JSON.parse(response);

            if (response.status != false) {
                utility.DisplayMessages(response.Message, 1);
                MIPSPreference_IndividualProvider.Search_MIPSIndiviualPreferences()
                iTrack_AdminIPPreference.UnLoad();


            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });

    },
    saveMIPSPreferencesGroup_DBCall: function () {

        var objData = new Object();
        var threshold = null;
        var Medicare = null;
        var Qualifying = null;
        var PartiallyQualifying = null;
        var other = null;
        if (iTrack_AdminIPPreference['IneligibleReasonLookUp']) {
            $.each(iTrack_AdminIPPreference['IneligibleReasonLookUp'], function (i, item) {
                if (item.Name == "Does Not meet low-volume threshold for 2018") {

                    threshold = item.LookupId;
                } else if (item.Name == "Newly enrolled in Medicare") {

                    Medicare = item.LookupId;
                } else if (item.Name == "Qualifying APM Participant (QP)") {

                    Qualifying = item.LookupId;
                } else if (item.Name == "Partially Qualifying APM Participant (Partial QP)") {

                    PartiallyQualifying = item.LookupId;
                } else if (item.Name == "Other") {

                    other = item.LookupId;
                }

            });
        }


        var IneligibleReason = [];
        if ($('#pnlIndividualProvider #rdoEligibile').prop('checked')) {
            objData["IsEligibile"] = true;
        } else {
            objData["IsEligibile"] = false;
            if ($('#pnlIndividualProvider #chkIneligibileReason1').prop('checked')) {
                data = { Reason: threshold, IsActive: true }
                IneligibleReason.push(data)
            } else {
                data = { Reason: threshold, IsActive: false }
                IneligibleReason.push(data)
            }
            if ($('#pnlIndividualProvider #chkIneligibileReason5').prop('checked')) {
                data = { Reason: Medicare, IsActive: true }
                IneligibleReason.push(data)
            } else {
                data = { Reason: Medicare, IsActive: false }
                IneligibleReason.push(data)
            }
            if ($('#pnlIndividualProvider #chkIneligibileReason2').prop('checked')) {
                data = { Reason: Qualifying, IsActive: true }
                IneligibleReason.push(data)
            } else {
                data = { Reason: Qualifying, IsActive: false }
                IneligibleReason.push(data)
            }
            if ($('#pnlIndividualProvider #chkIneligibileReason6').prop('checked')) {
                data = { Reason: PartiallyQualifying, IsActive: true }
                IneligibleReason.push(data)
            } else {
                data = { Reason: PartiallyQualifying, IsActive: false }
                IneligibleReason.push(data)
            }
            if ($('#pnlIndividualProvider #chkIneligibileReason3').prop('checked')) {
                data = { Reason: other, IsActive: true }
                IneligibleReason.push(data)
            } else {
                data = { Reason: other, IsActive: false }
                IneligibleReason.push(data)
            }
            if ($('#pnlIndividualProvider #txtIneligibileReason').val()) {

            }
            objData["InEligibileReason"] = $('#pnlIndividualProvider #txtIneligibileReason').val();
            objData["OtherComments"] = $('#pnlIndividualProvider #txtIneligibileReason').val();
            objData["InEligibileReasonIds"] = IneligibleReason;
        }
        var QulityId = null;
        var ACIId = null;
        var IAId = null;
        var CostId = null;
        if (iTrack_AdminIPPreference['GroupCatLookUp']) {
            $.each(iTrack_AdminIPPreference['GroupCatLookUp'], function (i, item) {
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
        var IsReporting = true;
        if ($('#pnlIndividualProvider #switchReporting').prop('checked')) {
            IsReporting = false;
            objData["ReportingReason"] = $('#pnlIndividualProvider #txtReason').val();

        } else {
            var PracticeId = $('#pnlIndividualProvider input[name=rdoPractice]:checked').val();
            var Practice = 'Large';
            if (PracticeId == 1) {
                Practice = 'Large';
            } else if (PracticeId == 2) {
                Practice = 'Small';
            } else if (PracticeId == 3) {
                Practice = 'Rural';
            }
            objData["PracticeType"] = Practice;
            var ReportingType = $('#pnlIndividualProvider input[name=rdoReportingType]:checked').val();
            var reproting = "Individual";
            if (ReportingType == 1) {
                reproting = "Individual";

                var MIPSCatData = [];
                if ($('#pnlIndividualProvider #chkQuality').prop('checked')) {
                    if ($('#pnlIndividualProvider #switchActiveQuality').prop('checked')) {
                        data = {
                            ReportingCat: QulityId,
                            DateFrom: $('#pnlIndividualProvider #dtpDateFromQuality').val(),
                            DateTo: $('#pnlIndividualProvider #dtpDateToQuality').val(),
                            GroupId: MIPSGroupId,
                            IsFullYear: false
                        }
                    }
                    else {
                        data = {
                            ReportingCat: QulityId,
                            DateFrom: "01/01/" + $('#pnlIndividualProvider #ddlQualityYeaR :selected').text(),
                            DateTo: "12/31/" + $('#pnlIndividualProvider #ddlQualityYeaR :selected').text(),
                            GroupId: MIPSGroupId,
                            IsFullYear: true,
                        }

                    }
                    MIPSCatData.push(data);
                }
                if ($('#pnlIndividualProvider #chkACI').prop('checked')) {
                    if ($('#pnlIndividualProvider #switchActiveACI').prop('checked')) {
                        data = {
                            ReportingCat: ACIId,
                            DateFrom: $('#pnlIndividualProvider #dtpDateFromACI').val(),
                            DateTo: $('#pnlIndividualProvider #dtpDateToACI').val(),
                            GroupId: MIPSGroupId,
                            IsFullYear: false,
                        }
                    }
                    else {
                        data = {
                            ReportingCat: ACIId,
                            DateFrom: "01/01/" + $('#pnlIndividualProvider #ddlACIYeaR :selected').text(),
                            DateTo: "12/31/" + $('#pnlIndividualProvider #ddlACIYeaR :selected').text(),
                            GroupId: MIPSGroupId,
                            IsFullYear: true,
                        }

                    }
                    MIPSCatData.push(data);
                }
                if ($('#pnlIndividualProvider #chkIA').prop('checked')) {
                    if ($('#pnlIndividualProvider #switchActiveIA').prop('checked')) {
                        data = {
                            ReportingCat: IAId,
                            DateFrom: $('#pnlIndividualProvider #dtpDateFromIA').val(),
                            DateTo: $('#pnlIndividualProvider #dtpDateToIA').val(),
                            GroupId: MIPSGroupId,
                            IsFullYear: false,
                        }
                    }
                    else {
                        data = {
                            ReportingCat: IAId,
                            DateFrom: "01/01/" + $('#pnlIndividualProvider #ddlIAYeaR :selected').text(),
                            DateTo: "12/31/" + $('#pnlIndividualProvider #ddlIAYeaR :selected').text(),
                            GroupId: MIPSGroupId,
                            IsFullYear: true,
                        }

                    }
                    MIPSCatData.push(data);
                }
                if ($('#pnlIndividualProvider #chkCost').prop('checked')) {
                    if ($('#pnlIndividualProvider #switchActiveCost').prop('checked')) {
                        data = {
                            ReportingCat: CostId,
                            DateFrom: $('#pnlIndividualProvider #dtpDateFromCost').val(),
                            DateTo: $('#pnlIndividualProvider #dtpDateToCost').val(),
                            GroupId: MIPSGroupId,
                            IsFullYear: false,
                        }
                    }
                    else {
                        data = {
                            ReportingCat: CostId,
                            DateFrom: "01/01/" + $('#pnlIndividualProvider #ddlCostYeaR :selected').text(),
                            DateTo: "12/31/" + $('#pnlIndividualProvider #ddlCostYeaR :selected').text(),
                            GroupId: MIPSGroupId,
                            IsFullYear: true,
                        }

                    }
                    MIPSCatData.push(data);
                }
                objData["MIPSCatData"] = MIPSCatData;



            } else if (ReportingType == 2) {
                reproting = "Group";
                objData["JoiningDate"] = $('#pnlIndividualProvider #dtpJoiningDate').val();
                objData["LeavingDate"] = $('#pnlIndividualProvider #dtpLeavingDate').val();
                objData["JoiningReason"] = $('#pnlIndividualProvider #ddlJoiningReason').val();
                objData["JoiningComments"] = $('#pnlIndividualProvider #txtReasonComments').val();
                objData["ParticipatingId"] = $('#pnlIndividualProvider #ddlJoiningReason :selected').val();
                objData["GroupId"] = $('#pnlIndividualProvider #ddlGroupName :selected').val();
            }
            objData["ReportingType"] = reproting;
            var PracticeMethId = $('#pnlIndividualProvider input[name=reportingmethod]:checked').val();
            var ReportingMethod = 'MD Vision EHR';
            if (PracticeMethId == 1) {
                ReportingMethod = 'MD Vision EHR';
            } else if (PracticeMethId == 2) {
                ReportingMethod = 'Sovereign Health Registry';
            } else if (PracticeMethId == 3) {
                ReportingMethod = 'Sovereign QCDR';
            } else if (PracticeMethId == 4) {
                ReportingMethod = 'Other';
            }

            objData["ReportingMethod"] = ReportingMethod;

        }
        var MIPSGroupId = iTrack_AdminIPPreference.params.ProviderId;
        if (iTrack_AdminIPPreference.params.PreferenceId != null && iTrack_AdminIPPreference.params.PreferenceId != "" && iTrack_AdminIPPreference.params.PreferenceId != "undefined") {
            objData["ObjectId"] = iTrack_AdminIPPreference.params.PreferenceId;
            objData["Id"] = iTrack_AdminIPPreference.params.PreferenceId;
            objData["commandType"] = "updatemipspreferencesindvidual";
        } else {
            objData["ObjectId"] = "-1";
            objData["Id"] = "-1";
            objData["commandType"] = "savemipspreferencesindvidual";
        }
        objData["IsActive"] = "True";
        objData["PerformanceYear"] = $('#pnlIndividualProvider #dtpPerformanceYear').val() == "" ? "2018" : $('#pnlIndividualProvider #dtpPerformanceYear').val();
        objData["ProviderId"] = MIPSGroupId;
        objData["IsReporting"] = IsReporting;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "iTrack", "iTrackAdmin");

    },
    // Start-- search Activity Log
    Search_IndividualProvider: function (pageNumber, rowsPerPage) {

        iTrack_AdminIPPreference.Search_IndividualProvider_DBCall(pageNumber, rowsPerPage).done(function (response) {
            response = JSON.parse(response);

            if (response.status != false) {


                // var ActivityLogUser_JSON = JSON.parse(response.ActivityLogUser_JSON);
                iTrack_AdminIPPreference.GridLoad(ActivityLogUser_JSON, response.ActivityLogUserCount, "NewEntry", " #pnlAuditbleEventsActivityLog #dgvNewEntry");
                var TableControl = iTrack_AdminIPPreference.params.PanelID + " #dgvNewEntry";
                var PagingPanelControlID = iTrack_AdminIPPreference.params.PanelID + " #dgvActivityLogUser_Paging";
                var ClassControlName = "AuditbleEvents_ActivityLog";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.ActivityLogUserCount, pageNumber, 15, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, pageNumber, resultPerPage) {
                    iTrack_AdminIPPreference.Search_ActivityLog(pageNumber, resultPerPage);
                }), 10);
                if (ActivityLogUser_JSON.length > 0) {
                    $('#pnlAuditbleEventsActivityLog #dgvNewEntry tbody tr:first').click();
                }
                else {
                    iTrack_AdminIPPreference.GridLoad(ActivityLogUser_JSON, response.ActivityLogCompCount, "User", " #pnlAuditbleEventsActivityLog #dgvUser");
                    iTrack_AdminIPPreference.GridLoad(ActivityLogUser_JSON, response.ActivityLogCompCount, "Changes", " #pnlAuditbleEventsActivityLog #dgvChanges");
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
        if ($("#rdoRural").prop('checked')) {
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
        iTrack_AdminIPPreference.searchActivityLogComponents_DBCall(ProfileName, PatientId, DateAndTime, UserId, pageNumber, rowsPerPage).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {


                var ActivityLogUser_JSON = JSON.parse(response.ActivityLogCompLoad_JSON);
                iTrack_AdminIPPreference.GridLoad(ActivityLogUser_JSON, response.ActivityLogCompCount, "User", " #pnlAuditbleEventsActivityLog #dgvUser");
                var TableControl = iTrack_AdminIPPreference.params.PanelID + " #dgvUser";
                var PagingPanelControlID = iTrack_AdminIPPreference.params.PanelID + " #dgvActivityLogComp_Paging";
                var ClassControlName = "AuditbleEvents_ActivityLog";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.ActivityLogCompCount, pageNumber, 15, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, pageNumber, resultPerPage) {
                    iTrack_AdminIPPreference.ActivityLogsComponent($('#pnlAuditbleEventsActivityLog #dgvNewEntry tr.active #rowprofilename').text(), $('#pnlAuditbleEventsActivityLog #dgvNewEntry tr.active #rowpatid').text(), $('#pnlAuditbleEventsActivityLog #dgvNewEntry tr.active #rowdateid').text(), $('#pnlAuditbleEventsActivityLog #dgvNewEntry tr.active #rowuserid').text(), pageNumber, resultPerPage);
                }), 10);
                if (ActivityLogUser_JSON.length > 0) {
                    $('#pnlAuditbleEventsActivityLog #dgvUser tbody tr:first').click();
                }
                else {
                    iTrack_AdminIPPreference.GridLoad(ActivityLogUser_JSON, response.ActivityLogCompCount, "Changes", " #pnlAuditbleEventsActivityLog #dgvChanges");
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
        iTrack_AdminIPPreference.searchActivityLogChanges_DBCall(ColumnKeyId, ProfileName, DateAndTime, pageNumber, rowsPerPage).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {


                var ActivityLogUser_JSON = JSON.parse(response.ActivityLogChangesLoad_JSON);
                iTrack_AdminIPPreference.GridLoad(ActivityLogUser_JSON, response.ActivityLogChangesCount, "Changes", " #pnlAuditbleEventsActivityLog #dgvChanges");
                var TableControl = iTrack_AdminIPPreference.params.PanelID + " #dgvChanges";
                var PagingPanelControlID = iTrack_AdminIPPreference.params.PanelID + " #dgvActivityLogChanges_Paging";
                var ClassControlName = "AuditbleEvents_ActivityLog";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.ActivityLogChangesCount, pageNumber, 15, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, pageNumber, resultPerPage) {
                    iTrack_AdminIPPreference.ActivityLogsChanges($('#pnlAuditbleEventsActivityLog #dgvUser tr.active #colkey').text(), $('#pnlAuditbleEventsActivityLog #dgvUser tr.active #profName').text(), $('#pnlAuditbleEventsActivityLog #dgvUser tr.active #datetime').text(), pageNumber, resultPerPage);
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


                $row.append('<td id= "MIPSProviderId" style="display:none;">' + item.MIPSProviderId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_Provider.ProviderDelete(\'' + item.MIPSProviderId + '\',event);" title="Delete Record"><i class="fa fa-eye blue"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_Provider.ProviderEdit(\'' + item.MIPSProviderId + '\',event,\'' + item.EntityId + '\');"  title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_Provider.ProviderActiveInactive(\'' + item.MIPSProviderId + '\', ' + item.MIPSProviderId + ',event);" title="' + item.MIPSProviderId + '"><i class="' + item.MIPSProviderId + '"></i></a>' + item.MIPSProviderId + '</td><td>' + item.LastName + '</td><td>' + item.FirstName + '</td><td>' + item.Specialty + '</td><td>' + item.NPI + '</td><td>' + item.PracticeName + '</td><td>' + item.Facility + '</td><td>' + item.MIPSEligibilityStatus + '</td><td>' + item.JoiningDate + '</td><td>' + item.LeavingDate + '</td>');
                $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); iTrack_AdminIPPreference.ActivityLogsComponent('" + item.ModuleName + "'," + item.PatientId + ",'" + item.DateAndTime + "'," + item.UserId + ");");

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
        if ($.fn.dataTable.isDataTable('#' + iTrack_AdminIPPreference.params["PanelID"] + ' #dgvMIPSAdminProvider'))
            ;
        else {
            $('#' + iTrack_AdminIPPreference.params["PanelID"] + ' #dgvMIPSAdminProvider').DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "searching": true, "bFilter": false, "order": [[3, "desc"]] }); // to remove records per page dropdown
        }

    },
    UnLoad: function () {
        UnloadActionPan(iTrack_AdminIPPreference.params["ParentCtrl"], "MIPS_AdminPreferenceGroup");

    },
}
