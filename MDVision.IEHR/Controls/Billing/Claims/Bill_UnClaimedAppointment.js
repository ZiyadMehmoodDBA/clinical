Bill_UnClaimedAppointment = {
    bIsFirstLoad: true,
    params:[],
    Load: function (params) {
        Bill_UnClaimedAppointment.params = params;
        if (Bill_UnClaimedAppointment.bIsFirstLoad) {
            Bill_UnClaimedAppointment.bIsFirstLoad = false;
            var self = $('#' + Bill_UnClaimedAppointment.params["PanelID"]);
            self.loadDropDowns(true);
            Bill_UnClaimedAppointment.LoadAllControls();
            //Bill_UnClaimedAppointment.LoadDefaultData();
            Bill_UnClaimedAppointment.UnClaimedAppSearch();
        }
    },

    ChargeCaptureAdd: function () {
        var params = [];
        params["FromAdmin"] = 0;
        params["ParentCtrl"] = 'billTabUnClaimedAppointment';
        params['mode'] = "Edit";
        LoadActionPan('EncounterChargeCapture', params);
    },
    OpenEncounter: function () {
        var params = [];
        params["FromAdmin"] = 0;
        params["ParentCtrl"] = 'billTabUnClaimedAppointment';

        params["patientID"] = 0;

        LoadActionPan('Encounter_Visits', params);

        //if (Bill_ChargeSearch.bVisitFirst) {
        //    $($('body #OpenVisits')[0]).attr('id', 'OpenVisits1')
        //    $($('body #CloseVisits')[0]).attr('id', 'CloseVisits1');
        //    Bill_ChargeSearch.bVisitFirst = false;
        //}

    },
    FillClaimNumberFromSearch: function (ClaimNumber, AccountNumber, PatientName, PatientId, DOSFrom, VisitId) {
        if ($("#" + Bill_UnClaimedAppointment.params.PanelID + " #txtClaimno").data('kendoAutoComplete'))
            utility.SetKendoAutoCompleteSourceforValidate($("#" + Bill_UnClaimedAppointment.params.PanelID + " #txtClaimno"), ClaimNumber, null, VisitId, "ClaimNumber");
        Encounter_Visits.UnLoad();
    },
    BindClaimNumber: function () {
        var Ctrl = $('#' + Bill_UnClaimedAppointment.params.PanelID + ' #txtClaimno');
        var func = function () { return Bill_UnClaimedAppointment.GetClaimNumberArray(Ctrl.val()); };
        utility.BindKendoAutoComplete(Ctrl, 3, "ClaimNumber", "contains", null, func);
    },
    GetClaimNumberArray: function (ClaimNumber) {
        var AllClaimsVisits = [];
        var dfd = new $.Deferred();
        Bill_ClaimSubmission.LoadClaimNumers(ClaimNumber).done(function (responseData) {
            if (responseData.status != false) {
                if (responseData.ClaimsCount > 0) {
                    var Claims = JSON.parse(responseData.ClaimsLoad_JSON);
                    $.each(Claims, function (i, item) {
                        AllClaimsVisits.push({ id: item.VisitId, value: item.ClaimNumber, PatientId: item.PatientId, DOSFrom: item.DOSFrom, PatientName: item.AccountNumber + ' - ' + item.PatientName, ClaimNumber: item.ClaimNumber });
                    });
                }
            }
            dfd.resolve(AllClaimsVisits);
        });
        return dfd.promise();
    },
    UnClaimedAppSearch: function (PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("UnClaimed Appointments", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlBillUnClaimedApp_Result").css("display") == "none") {
                    $("#pnlBillUnClaimedApp_Result").show();
                }
                var DOSTo = $('#' + Bill_UnClaimedAppointment.params.PanelID + ' #frmUnClaimedAppointment #dpUnclaimedDOSTo').val();
                var DOSFrom = $('#' + Bill_UnClaimedAppointment.params.PanelID + ' #frmUnClaimedAppointment #dpUnclaimedDOSFrom').val();
                if (DOSTo == "" && DOSFrom !="")
                {
                    utility.CreateDatePicker(Bill_UnClaimedAppointment.params.PanelID + ' #frmUnClaimedAppointment #dpUnclaimedDOSTo', function (ev) { }, true);
                }



                var self = $("#pnlBillUnClaimedAppSearch");
                var myJSON = self.getMyJSON();

                Bill_UnClaimedAppointment.SearchUnClaimedApp(myJSON, PageNo, rpp).done(function (response) {
                    if (response.status != false) {

                        //-----------------Start Pagination--------------

                        if (response.UnClaimedAppCount > 0) {
                            $("#" + Bill_UnClaimedAppointment.params.PanelID + " #divUnClaimAppPaging").css("display", "inline");
                            //Showing 1 to 15 of 15 entries
                            var RecordsPerPage = rpp != null ? rpp : 15;
                            var CurrentPage = PageNo != null ? PageNo : 1;

                            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                            if (PageNo == null) {
                                utility.GetCustomPaging("divUnClaimAppPaging", response.iTotalDisplayRecords, 5, "Bill_UnClaimedAppointment", CurrentPage, RecordsPerPage);
                            }
                            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                            $("#" + Bill_UnClaimedAppointment.params.PanelID + " #divUnClaimAppPaging #divShowingEntries").text(showingText);
                            // Change Background Color to Black for selected page
                            $("#" + Bill_UnClaimedAppointment.params.PanelID + " li").each(function () {
                                if ($(this).text() == CurrentPage) {
                                    $(this).attr("class", "active");
                                }
                                else
                                    $(this).removeAttr("class");
                            });
                        }
                        else {
                            $("#" + Bill_UnClaimedAppointment.params.PanelID + " #divUnClaimAppPaging").css("display", "none");
                        }


                        //--------------------End Pagination------------
                      
                        Bill_UnClaimedAppointment.UnClaimedAppGridLoad(response);
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
    UnClaimedAppReset: function () {
        //$('#' + Bill_UnClaimedAppointment.params["PanelID"] + ' #frmUnClaimedAppointment').find('[data-plugin-datepicker]').each(function () { $(this).datepicker('setDate', new Date()); });
        $('#' + Bill_UnClaimedAppointment.params["PanelID"] + ' #frmUnClaimedAppointment').resetAllControls();
        //$('#' + Bill_UnClaimedAppointment.params["PanelID"] + ' #frmUnClaimedAppointment [type="text"][onblur]').each(function () {
        //    $(this).trigger("blur");
        //});
        $('#' + Bill_UnClaimedAppointment.params["PanelID"] + ' #frmUnClaimedAppointment #dpUnclaimedDOSTo').attr('disabled', true);
        //utility.AddDaysFromToDate('frmUnClaimedAppointment', 'dpUnclaimedDOSFrom', 'dpUnclaimedDOSTo', 0, 0);
        //AST-165 Start
        // reset Facility Link
        if ($('#' + Bill_UnClaimedAppointment.params["PanelID"] + ' #frmUnClaimedAppointment #lnkFacilityEdit').is(":visible")) {
            $('#' + Bill_UnClaimedAppointment.params["PanelID"] + ' #frmUnClaimedAppointment #lnkFacilityEdit').css("display", "none");
            $('#' + Bill_UnClaimedAppointment.params["PanelID"] + ' #frmUnClaimedAppointment #lblFacility').css("display", "block");
        }
        // reset Provider Link
        if ($('#' + Bill_UnClaimedAppointment.params["PanelID"] + ' #frmUnClaimedAppointment #lnkProviderEdit').is(":visible")) {
            $('#' + Bill_UnClaimedAppointment.params["PanelID"] + ' #frmUnClaimedAppointment #lnkProviderEdit').css("display", "none");
            $('#' + Bill_UnClaimedAppointment.params["PanelID"] + ' #frmUnClaimedAppointment #lblProvider').css("display", "block");
        }
        // reset Insurance Link
        if ($('#' + Bill_UnClaimedAppointment.params["PanelID"] + ' #frmUnClaimedAppointment #lnkInsurancePlanDetail').is(":visible")) {
            $('#' + Bill_UnClaimedAppointment.params["PanelID"] + ' #frmUnClaimedAppointment #lnkInsurancePlanDetail').css("display", "none");
            $('#' + Bill_UnClaimedAppointment.params["PanelID"] + ' #frmUnClaimedAppointment #lblInsurancePlan').css("display", "block");
        }
        //AST-165 End
    },
    ValidateSearchCriteria: function () {

        utility.ValidateSearchCriteria(Bill_UnClaimedAppointment.params.PanelID + " #frmUnClaimedAppointment", function () {
            Bill_UnClaimedAppointment.UnClaimedAppSearch();
        });
    },
    UnClaimedAppGridLoad: function (response) {
        $("#dgvUnClaimedApp").dataTable().fnDestroy();
        $("#pnlBillUnClaimedApp_Result #dgvUnClaimedApp tbody").find("tr").remove();
        if (response.UnClaimedAppCount > 0) {
            var UnClaimedAppLoadJSONData = JSON.parse(response.UnClaimedAppLoad_JSON);
            $.each(UnClaimedAppLoadJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", "gvUnClaimedApp_row" + i);


                if (UnClaimedAppLoadJSONData[i].Claimed.toLowerCase() == "yes") {

                    $row.append('<td style="display:none;">' + item.AppointmentId + '</td> <td><a class="btn  btn-xs" href="#" onclick="Bill_UnClaimedAppointment.OpenVisitDetail(' + item.VisitId + ',' + item.PatientId + ',event);" title="Visit Detail"><i class="fa fa-edit black"></i></a></td> <td data-toggle="tooltip" class="ellip150" data-placement="right" title="' + item.Patient + '"  ><a href="#" onclick="Bill_UnClaimedAppointment.PatientDemographics(' + item.PatientId + ',event);"  title="View Patient">' + item.Patient + '</a></td> <td><a href="#" onclick="Bill_UnClaimedAppointment.PatientDemographics(' + item.PatientId + ',event);"  title="View Patient">' + item.AccountNumber + '</a></td><td>' + item.ClaimNumber + '</td> <td>' + item.DOSFrom.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td> <td>' + item.DOSTo.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td> <td>' + item.Status + '</td> <td>' + item.FacilityName + '</td> <td>' + item.ProviderName + '</td> <td data-toggle="tooltip" class="ellip150" data-placement="left" title="' + item.InsurancePlanName + '"  >' + item.InsurancePlanName + '</td><td>' + item.Claimed + '</td><td>' + item.SubmitStatus + '</td><td>' + item.ClaimStatus + '</td>');
                    $row.attr("onclick", "Bill_UnClaimedAppointment.OpenVisitDetail('" + item.VisitId + "','" + item.PatientId + "',event);");
                }
                else if (UnClaimedAppLoadJSONData[i].Claimed.toLowerCase() == "no") {

                    $row.append('<td style="display:none;">' + item.AppointmentId + '</td> <td><a class="btn  btn-xs" href="#" onclick="Bill_UnClaimedAppointment.OpenCheckIn(\'' + item.AppointmentId + '\',\'' + item.PatientId + '\',\'' + item.ProviderId + '\',\'' + item.FacilityId + '\',event);" title="CheckIn"><i class="fa fa-check-square-o black"></i></a></td> <td data-toggle="tooltip" class="ellip150" data-placement="right" title="' + item.Patient + '"><a href="#" onclick="Bill_UnClaimedAppointment.PatientDemographics(' + item.PatientId + ',event);"  title="View Patient">' + item.Patient + '</a></td> <td><a href="#" onclick="Bill_UnClaimedAppointment.PatientDemographics(' + item.PatientId + ',event);"  title="View Patient">' + item.AccountNumber + '</a></td><td>' + item.ClaimNumber + '</td> <td>' + item.DOSFrom.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td> <td>' + item.DOSTo.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td> <td>' + item.Status + '</td> <td>' + item.FacilityName + '</td> <td>' + item.ProviderName + '</td> <td>' + item.InsurancePlanName + '</td><td>' + item.Claimed + '</td><td>' + item.SubmitStatus + '</td><td>' + item.ClaimStatus + '</td>');
                    $row.attr("onclick", "Bill_UnClaimedAppointment.OpenCheckIn('" + item.AppointmentId + "','" + item.PatientId + "','" + item.ProviderId + "','" + item.FacilityId + "',event);");
                } else if (UnClaimedAppLoadJSONData[i].Claimed.toLowerCase() == "") {
                    $row.append('<td style="display:none;">' + item.AppointmentId + '</td> <td><a class="btn  btn-xs" href="#" onclick="Bill_UnClaimedAppointment.OpenCheckIn(\'' + item.AppointmentId + '\',\'' + item.PatientId + '\',\'' + item.ProviderId + '\',\'' + item.FacilityId + '\',event);" title="CheckIn"><i class="fa fa-check-square-o black"></i></a></td> <td data-toggle="tooltip" class="ellip150" data-placement="right" title="' + item.Patient + '"><a href="#" onclick="Bill_UnClaimedAppointment.PatientDemographics(' + item.PatientId + ',event);"  title="View Patient">' + item.Patient + '</a></td> <td><a href="#" onclick="Bill_UnClaimedAppointment.PatientDemographics(' + item.PatientId + ',event);"  title="View Patient">' + item.AccountNumber + '</a></td><td>' + item.ClaimNumber + '</td> <td>' + item.DOSFrom.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td> <td>' + item.DOSTo.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td> <td>' + item.Status + '</td> <td>' + item.FacilityName + '</td> <td>' + item.ProviderName + '</td> <td>' + item.InsurancePlanName + '</td><td>' + item.Claimed + '</td><td>' + item.SubmitStatus + '</td><td>' + item.ClaimStatus + '</td>');
                    $row.attr("onclick", "Bill_UnClaimedAppointment.OpenCheckIn('" + item.AppointmentId + "','" + item.PatientId + "','" + item.ProviderId + "','" + item.FacilityId + "',event);");
                }

                $("#pnlBillUnClaimedApp_Result #dgvUnClaimedApp tbody").last().append($row);
            });
        }
        else {
            $("#" + Bill_UnClaimedAppointment.params.PanelID + " #divUnClaimAppPaging").css("display", "none");
            $('#dgvUnClaimedApp').DataTable({
                "language": {
                    "emptyTable": "No Appointment Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvUnClaimedApp'))
            ;
        else
            $("#pnlBillUnClaimedApp_Result #dgvUnClaimedApp").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
    },
    OpenCheckIn: function (AppointmentId, PatientId, ProviderId, FacilityId,event) {
        if (event != null) {
            event.stopPropagation();
        }
        var params = [];
        params["AppointmentId"] = AppointmentId;
        params["PatientId"] = PatientId;
        params["ProviderId"] = ProviderId;
        params["FacilityId"] = FacilityId;
        params["ParentCtrl"] = 'billTabUnClaimedAppointment';
        LoadActionPan('schcheckin', params);

    },

    PatientDemographics: function (patientid, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Demographic", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                var params = [];
                params["mode"] = 'Edit';
                params["PatBanner"] = true;
                params["patientID"] = patientid;
                params["IsFill"] = false;
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = "billTabUnClaimedAppointment";
                LoadActionPan('demographicDetail', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    LoadAllControls: function () {
        CacheManager.BindCodes('GetFacility', false).done(function (result) {
            var Ctrl = $("#frmUnClaimedAppointment #txtUnclaimedFacility");
            var hfCtrl = $("#" + Bill_UnClaimedAppointment.params.PanelID + " #hfFacility");
            var onSelect = function (e) {
                $("#" + Bill_UnClaimedAppointment.params.PanelID + " #txtPractice").val(e.Practice);
                $("#" + Bill_UnClaimedAppointment.params.PanelID + " #hfPractice").val(e.PracticeId);
            };
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl, onSelect);
        });
        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            var Ctrl = $("#frmUnClaimedAppointment #txtUnClaimedProvider");
            var hfCtrl = $("#" + Bill_UnClaimedAppointment.params.PanelID + " #hfProvider");
            var onSelect = function (e) { Ctrl.attr("ProviderId", e.id); };
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, onSelect);
        });
        CacheManager.BindCodes('GetInsurancePlan', false).done(function (result) {
            var Ctrl = $("#" + Bill_UnClaimedAppointment.params.PanelID + " input#txtInsurancePlan");
            var hfCtrl = $("#" + Bill_UnClaimedAppointment.params.PanelID + " #hfInsurancePlan");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", InsurancePlans, null, hfCtrl);
        });
        Bill_UnClaimedAppointment.BindPatientAccount();
        Bill_UnClaimedAppointment.BindClaimNumber();
        utility.ValidateFromToDate('frmUnClaimedAppointment', 'dpUnclaimedDOSFrom', 'dpUnclaimedDOSTo', true);
        utility.AddDaysFromToDate('frmUnClaimedAppointment', 'dpUnclaimedDOSFrom', 'dpUnclaimedDOSTo', 0, 0);
    },
   

    LoadDefaultData: function () {
        $("#" + Bill_UnClaimedAppointment.params["PanelID"] + ' #txtUnClaimedProvider').val(globalAppdata['DefaultProviderName']);
        $("#" + Bill_UnClaimedAppointment.params["PanelID"] + ' #hfProvider').val(globalAppdata['DefaultProviderId']);
        $("#" + Bill_UnClaimedAppointment.params["PanelID"] + ' #lnkProviderEdit').css("display", "inline");
        $("#" + Bill_UnClaimedAppointment.params["PanelID"] + ' #lblProvider').css("display", "none");
        $("#" + Bill_UnClaimedAppointment.params["PanelID"] + ' #txtUnclaimedFacility').val(globalAppdata['DefaultFacilityName']);
        $("#" + Bill_UnClaimedAppointment.params["PanelID"] + ' #hfFacility').val(globalAppdata['DefaultFacilityId']);
        $("#" + Bill_UnClaimedAppointment.params["PanelID"] + ' #lnkFacilityEdit').css("display", "inline");
        $("#" + Bill_UnClaimedAppointment.params["PanelID"] + ' #lblFacility').css("display", "none");
        utility.CreateDatePicker(Bill_UnClaimedAppointment.params.PanelID + ' #frmUnClaimedAppointment #dpUnclaimedDOSFrom,#dpUnclaimedDOSTo', function (ev) { }, true);
    },
    OpenVisitDetail: function (VisitId, PatientId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var params = [];
        params["ParentCtrl"] = 'billTabUnClaimedAppointment';
        params["VisitId"] = VisitId;
        params["patientID"] =PatientId;
        LoadActionPan('EncounterChargeCapture', params);

    },
    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmUnClaimedAppointment";
        params["FacilityId"] = "-1";
        params["RefCtrl"] = "txtUnclaimedFacility";
        params["RefHiddenIdCtrl"] = "hfFacility";
        
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "billTabUnClaimedAppointment";
        LoadActionPan('Admin_Facility', params);
    },
    OpenFacilityDetail: function () {
        var params = [];
        params["FacilityId"] = $('#' + Bill_UnClaimedAppointment.params.PanelID + ' #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'billTabUnClaimedAppointment';
        params["RefCtrl"] = "txtUnclaimedFacility";
        LoadActionPan('facilityDetail', params);
    },
    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmUnClaimedAppointment";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtUnClaimedProvider";
        params["RefHiddenIdCtrl"] = "hfProvider";
        params["ParentCtrl"] = "billTabUnClaimedAppointment";
        LoadActionPan('Admin_Provider', params);
    },
    OpenProviderDetail: function () {
        var params = [];
        params["ProviderId"] = $('#'+Bill_UnClaimedAppointment.params.PanelID+' #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtUnClaimedProvider";
        params["ParentCtrl"] = 'billTabUnClaimedAppointment';
        LoadActionPan('providerDetail', params);
    },
    OpenInsurancePlan: function () {
        var params = [];
        params["InsurancePlanId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'billTabUnClaimedAppointment';
        LoadActionPan('Admin_InsurancePlan', params);
    },
    OpenInsurancePlanDetail: function () {
        //Admin_InsurancePlan.InsurancePlanEdit($("#"+ Bill_UnClaimedAppointment.params.PanelID +" #hfInsurancePlan").val());
       var params = [];
        params["InsurancePlanId"] = $("#" + Bill_UnClaimedAppointment.params.PanelID + " #hfInsurancePlan").val();
        params["mode"] = "Edit";
        Admin_InsurancePlan.params["FromAdmin"] == "0";
        params["ParentCtrl"] = 'billTabUnClaimedAppointment';
        LoadActionPan('insurancePlanDetail', params);
    },
    FillInsurancePlanName: function (InsurancePlanId, InsurancePlanName) {
        $("#"+Bill_UnClaimedAppointment.params.PanelID+" #txtInsurancePlan").val(InsurancePlanName);
        $("#" + Bill_UnClaimedAppointment.params.PanelID + " #hfInsurancePlan").val(InsurancePlanId);
        $("#" + Bill_UnClaimedAppointment.params.PanelID + " #lnkInsurancePlanDetail").css("display", "inline");
        $("#" + Bill_UnClaimedAppointment.params.PanelID + " #lblInsurancePlan").css("display", "none");
        UnloadActionPan(Admin_InsurancePlan.params["ParentCtrl"]);

    },
    SearchUnClaimedApp: function (UnClaimedAppData, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }

        var data = "UnClaimedAppData=" + UnClaimedAppData + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "BILLING_UNCLAIMED_APP", "SEARCH_UNCLAIMED_APP");
    },
    OpenPatientSearch: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'billTabUnClaimedAppointment';
        LoadActionPan('Patient_Search', params);
    },
    FillPatientInfoFromSearch: function (PatientId, AccountNumber, patFullName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        $Ctrl_reft = $("#" + Bill_UnClaimedAppointment.params.PanelID + " #txtPatientName");
        $hfCtrl_reft = $("#" + Bill_UnClaimedAppointment.params.PanelID + " #hfPatientId");
        if ($Ctrl_reft.data('kendoAutoComplete'))
            utility.SetKendoAutoCompleteSourceforValidate($Ctrl_reft, AccountNumber, $hfCtrl_reft, PatientId, "AccountNumber");
        UnloadActionPan("billTabUnClaimedAppointment");
        utility.InsertRecentPatient(PatientId);
    },
    BindPatientAccount: function () {
        var Ctrl = $('#' + Bill_UnClaimedAppointment.params.PanelID + ' #txtPatientName');
        var func = function () { return utility.GetPatientArray(Ctrl.val(), 0) };
        var hfCtrl = $("#" + Bill_UnClaimedAppointment.params.PanelID + " #hfPatientId");
        var onSelect = function (e) { utility.InsertRecentPatient(e.id); };
        utility.BindKendoAutoComplete(Ctrl, 4, "AccountNumber", "contains", null, func, hfCtrl, onSelect);
    },
    UnLoadTab: function (Tab) {

        RemoveAdminTab(Tab);

    },
    //--------Pagination Functions-------

    SelectedPageClick: function (PageNo, objPage) {
        // Change Background Color to Black for selected page
        $("#pnlBillUnClaimedApp_Result li").each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });
        Bill_UnClaimedAppointment.UnClaimedAppSearch(PageNo, 15);
    },
    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#pnlBillUnClaimedApp_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            Bill_UnClaimedAppointment.UnClaimedAppSearch(currentPageNo, 15);

        }
    },
    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {

        var currentPageNo = "";
        $("#pnlBillUnClaimedApp_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });

        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo <= TotalPages) {
            Bill_UnClaimedAppointment.UnClaimedAppSearch(currentPageNo, 15);
        }
    },
    OpenPatientSearchForClaim: function () {
        if (!Patient_Demographic.params.patientID) {
            var params = [];
            params["FromAdmin"] = "0";
            params["ForNewClaim"] = "1";
            params["ParentCtrl"] = 'billTabUnClaimedAppointment';

            LoadActionPan('Patient_Search', params);
        }
        else {
            var params = [];
            params["FromAdmin"] = 0;
            params["ParentCtrl"] = 'billTabUnClaimedAppointment';
            params['mode'] = "Add";
            params["PatientId"] = Patient_Demographic.params.patientID;
            params["patFullName"] = Patient_Demographic.params.patFullName;
            params["RefProviderId"] = Patient_Demographic.params.RefProviderId;
            params["RefProviderName"] = Patient_Demographic.params.RefProviderName;
            params["ProviderId"] = Patient_Demographic.params.PatientProviderId;
            params["ProviderName"] = Patient_Demographic.params.PatientProvider;
            params["FacilityId"] = Patient_Demographic.params.PatientFacilityId;
            params["FaciltyName"] = Patient_Demographic.params.PatientFacility;
            params["SelfPay"] = Patient_Demographic.params.SelfPay;
            LoadActionPan('Encounter_CreateClaim', params);
        }
    },   
}
