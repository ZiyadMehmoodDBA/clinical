Patient_Search = {
    bIsFirstLoad: true,
    params: [],
    ActionBit: false,
    PatientAddbit: true,
    //bVisitFirst: true,
    currentPanelID: "",
    IsRecentPatientSelected: "",
    NotFoundPatientDetails: {},
    Load: function (params) {
        Patient_Search.params = params;

        if (Patient_Search.params != null && Patient_Search.params.PanelID != "Patient_Search") {
            Patient_Search.params["PanelID"] = Patient_Search.params["PanelID"] + ' #Patient_Search';
        }
        else {
            Patient_Search.params = [];
            Patient_Search.params["PanelID"] = "Patient_Search"
        }
        // Edit by Mohsin Bug# PMS-3216
        // Now appointment of inActive Patient cannot be created
        if (Patient_Search.params.ParentCtrl == 'appointmentDetail') {
            $('#' + Patient_Search.params.PanelID + ' #ddlActive').attr('disabled', 'disabled');
        }
        //Begin Edit By Azeem Raza Tayyab to Fix Bug#
        $('html').click(function () {
            $('.tooltip').remove();
        });
        //End
        //END Edit by Mohsin Bug# PMS-3216

        //Start Farooq Ahmad 26/04/2016 Select All Patients

        $('#pnlPatient_Result #selectAllPatients').on('click', function () {
            $('#pnlPatient_Result input[type=checkbox].patient').prop('checked', $(this).prop('checked'));
        });

        $('#pnlPatient_Result .patient').on('click', function (event) {
            event.stopPropagation();
        });

        //End Farooq Ahmad 26/4/2016 Select All Patients
        utility.ValidateDOB('pnlPatient_Search #frmPatientSearch', Patient_Search.params.PanelID + ' #frmPatientSearch #dtpLastSeenDate', new Date('1800-01-01'), new Date(), false);
        if (Patient_Search.bIsFirstLoad) {
            Patient_Search.bIsFirstLoad = false;
            var self = $('#' + Patient_Search.params["PanelID"]);
            $('#' + Patient_Search.params.PanelID + ' #txtSSN').val('');
            self.loadDropDowns(true).done(function () {
                //Insert all option in ddlSearchSex
                try {
                    $($(self.find("#ddlSearchSex option"))[0]).html("All");
                } catch (ex) {
                    console.log(ex);
                }

                // LoadActionPan('Patient_Search', params);
                $("#" + Patient_Search.params["PanelID"] + " #txtRowspPageHF").val(15);
                $("#" + Patient_Search.params["PanelID"] + " #txtPageNoHF").val(1);
                //For Change the size of search Dialog
                $('#' + Patient_Search.params.PanelID + ' #modaldialog').removeClass('modal-dialog-lg');
                $('#' + Patient_Search.params.PanelID + ' #modaldialog').addClass('size100per');
                $("#" + Patient_Search.params["PanelID"] + " #hfIsRecentPatientCalled").val("1");

                AppPrivileges.GetFormPrivileges("Demographic", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        Patient_Search.PatientSearch(null, null, null, 1);
                        Patient_Search.IsRecentPatientSelected = 1
                    }
                });

                //Patient_Search.LoadAllAutocomplete();
                Patient_Search.BindInsuranceAutoComplete();
                Patient_Search.BindClaimNumber();
                Patient_Search.BindProviderAutoComplete();
                Patient_Search.BindFaciltyAutoComplete();
                Patient_Search.BindPracticeAutoComplete();
                Patient_Search.BindGuarantor();
                Patient_Search.SelectActionMode();
                $("#" + Patient_Search.params["PanelID"] + " #txtPrimaryInsurance").attr("autocomplete", "nope");
                $("#" + Patient_Search.params["PanelID"] + " #txtGuarantor").attr("autocomplete", "nope");
                Patient_Search.SearchPatientByPressingEnter();  // PRD-422
            });
        }

        $('.Patient_Search:last').parent().draggable({
            handle: ".Patient_Search:last .modal-content"
        });
    },

    LoadAllAutocomplete: function () {
        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            $("input#txtSearchProvider").autocomplete({
                autoFocus: true,
                source: Providers, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#" + Patient_Search.params["PanelID"] + " #hfSearchProvider").val(ui.item.id); // add the selected id
                        if ($("#" + Patient_Search.params["PanelID"] + " #lnkSearchProviderEdit").css("display") == "none") {
                            $("#" + Patient_Search.params["PanelID"] + " #lnkSearchProviderEdit").css("display", "inline");
                            $("#" + Patient_Search.params["PanelID"] + " #lblSearchProvider").css("display", "none");
                        }
                    }, 100);
                }
            });
        });

        CacheManager.BindCodes('GetFacility', false).done(function (result) {
            $("input#txtSearchFacility").autocomplete({
                autoFocus: true,
                source: Facilities, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#" + Patient_Search.params["PanelID"] + " #hfSearchFacility").val(ui.item.id); // add the selected id
                        if ($("#" + Patient_Search.params["PanelID"] + " #lnkSearchFacilityEdit").css("display") == "none") {
                            $("#" + Patient_Search.params["PanelID"] + " #lnkSearchFacilityEdit").css("display", "inline");
                            $("#" + Patient_Search.params["PanelID"] + " #lblSearchFacility").css("display", "none");
                        }
                    }, 100);
                }
            });
        });
        CacheManager.BindCodes('GetInsurancePlan', false).done(function (result) {

            $("#" + Patient_Search.params["PanelID"] + " input#txtPrimaryInsurance").autocomplete({
                autoFocus: true,
                source: InsurancePlans, // pass an array (without a comma)
                select: function (event, ui) {

                    setTimeout(function () {
                        $("#" + Patient_Search.params["PanelID"] + " #hfInsurancePlan").val(ui.item.id); // add the selected id
                        //$("#" + Patient_Search.params["PanelID"] + " #hfInsurancePlansearchPattern").val(ui.item.searchPattern);
                        if ($("#" + Patient_Search.params["PanelID"] + " #lnkPrimaryInsurancePlanDetail").css("display") == "none") {
                            $("#" + Patient_Search.params["PanelID"] + " #lnkPrimaryInsurancePlanDetail").css("display", "inline");
                            $("#" + Patient_Search.params["PanelID"] + " #lblPrimaryInsurance").css("display", "none");
                        }
                        // Patient_Search.FillInsurancePlanAddress(ui.item.id);
                        //CacheManager.BindDropDownsByID('#pnlPatientInsurance #ddlPlanAddress', 'GetInsurancePlanAddress', true, ui.item.id);
                    }, 100);
                }
            });
        });

        CacheManager.BindCodes('GetPractice', false).done(function (result) {
            $("input#txtSearchPractice").autocomplete({
                autoFocus: true,
                source: Practices, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#" + Patient_Search.params["PanelID"] + " #hfSearchPractice").val(ui.item.id); // add the selected id
                        if ($("#" + Patient_Search.params["PanelID"] + " #lnkPracticeEdit").css("display") == "none") {
                            $("#" + Patient_Search.params["PanelID"] + " #lnkPracticeEdit").css("display", "inline");
                            $("#" + Patient_Search.params["PanelID"] + " #lblPractice").css("display", "none");
                        }
                    }, 100);
                }
            });
        });
    },

    OpenProvider: function () {
        var params = [];
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtSearchProvider";
        params["RefCtrlHidden"] = "hfSearchProvider";
        params["RefCtrlLabel"] = "lblSearchProvider";
        params["RefCtrlLink"] = "lnkSearchProviderEdit";
        params["ParentCtrl"] = "Patient_Search";
        LoadActionPan('Admin_Provider', params);
    },
    FillInsurancePlanName: function (InsurancePlanId, InsurancePlanName, SearchPattern) {
        $("#" + Patient_Search.params.PanelID + " #txtPrimaryInsurance").val(InsurancePlanName);
        $("#" + Patient_Search.params.PanelID + " #hfInsurancePlan").val(InsurancePlanId);
        $("#" + Patient_Search.params.PanelID + " #lnkPrimaryInsurancePlanDetail").css("display", "inline");
        $("#" + Patient_Search.params.PanelID + " #lblPrimaryInsurance").css("display", "none");
        utility.SetKendoAutoCompleteSourceforValidate($("#" + Patient_Search.params.PanelID + " #txtPrimaryInsurance"), InsurancePlanName, $("#" + Patient_Search.params.PanelID + " #hfInsurancePlan"), InsurancePlanId);
        UnloadActionPan(Admin_InsurancePlan.params["ParentCtrl"]);
        //Patient_Insurance.FillInsurancePlanAddress(InsurancePlanId);

    },
    OpenInsurancePlanDetail: function () {
        //Admin_InsurancePlan.InsurancePlanEdit($("#pnlPatientInsurance #hfInsurancePlan").val());
        var params = [];
        var PanelID = null;

        PanelID = Patient_Search.params["PanelID"];

        params["ParentCtrl"] = 'Patient_Search';

        params["InsurancePlanId"] = $("#" + Patient_Search.params.PanelID + " #hfInsurancePlan").val();
        // params["PlanAddressId"] = $("#pnlPatientInsurance #ddlPlanAddress").last().val();
        params["mode"] = "Edit";
        Admin_InsurancePlan.params["FromAdmin"] == "0";
        //params["ParentCtrl"] = 'patTabInsurance';
        LoadActionPan('insurancePlanDetail', params, PanelID);
    },

    OpenInsurancePlan: function (PlanProvider) {
        var params = [];
        var PanelID = null;
        // if (Patient_Search.params["PanelID"] != "pnlPatientInsurance")
        PanelID = Patient_Search.params["PanelID"];
        //  if (Patient_Search.params.ParentCtrl == "Admin_InsurancePlan" || Patient_Search.params.ParentCtrl == "demographicDetail")
        params["ParentCtrl"] = 'Patient_Search';


        params["InsurancePlanId"] = "-1";
        params["FromAdmin"] = "0";
        if (PlanProvider != null)
            params["RefCtrl"] = PlanProvider;
        LoadActionPan('Admin_InsurancePlan', params, PanelID);
    },

    OpenProviderDetail: function () {
        //Admin_Provider.ProviderEdit($('#' + demographicDetail.params.PanelID + ' #hfSearchProvider').val(), "demographicDetail");
        var params = [];
        params["ProviderId"] = $('#' + Patient_Search.params["PanelID"] + ' #hfSearchProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtSearchProvider";
        params["ParentCtrl"] = 'Patient_Search';
        LoadActionPan('providerDetail', params);
    },

    OpenFacility: function () {
        var params = [];
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtSearchFacility";
        params["RefCtrlHidden"] = "hfSearchFacility";
        params["RefCtrlLabel"] = "lblSearchFacility";
        params["RefCtrlLink"] = "lnkSearchFacilityEdit";
        params["ParentCtrl"] = "Patient_Search";
        LoadActionPan('Admin_Facility', params);
    },

    OpenFacilityDetail: function () {
        //Admin_Facility.FacilityEdit($('#' + demographicDetail.params.PanelID + ' #hfSearchFacility').val(), "demographicDetail");
        var params = [];
        params["FacilityId"] = $('#' + Patient_Search.params.PanelID + ' #hfSearchFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtSearchFacility";
        params["ParentCtrl"] = 'Patient_Search';
        LoadActionPan('facilityDetail', params);
    },

    OpenPractice: function () {
        var params = [];
        params["PracticeId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtSearchPractice";
        params["RefCtrlHidden"] = "hfSearchPractice";
        params["RefCtrlLabel"] = "lblPractice";
        params["RefCtrlLink"] = "lnkPracticeEdit";
        params["ParentCtrl"] = "Patient_Search";
        LoadActionPan('Admin_Practice', params);
    },

    OpenPracticeDetail: function () {
        //Admin_Facility.FacilityEdit($('#' + demographicDetail.params.PanelID + ' #hfSearchFacility').val(), "demographicDetail");
        var params = [];
        params["PracticeId"] = $('#' + Patient_Search.params.PanelID + ' #hfSearchPractice').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtSearchPractice";
        params["ParentCtrl"] = 'Patient_Search';
        LoadActionPan('practiceDetail', params);
    },

    SelectActionMode: function () {
        $('#' + Patient_Search.params["PanelID"] + ' #pnlPatient_Result #ColumnAction').addClass("size95");

        Patient_Search.ActionBit = false;
        if (Patient_Search.params != null) {
            if (Patient_Search.params.ParentCtrl != null) {
                if (Patient_Search.params.ParentCtrl == "emergencyContactDetail") {
                    Patient_Search.ActionBit = true;

                }
                else if (Patient_Search.params.ParentCtrl == "Patient_Family_Detail") {
                    Patient_Search.ActionBit = true;

                }
                else if (Patient_Search.params.ParentCtrl == "appointmentDetail") {
                    Patient_Search.ActionBit = true;
                    Patient_Search.PatientAddbit = false;
                }
                else if (Patient_Search.params.ParentCtrl == "Scheduling_Force_Booking") {
                    Patient_Search.ActionBit = true;
                    Patient_Search.PatientAddbit = false;
                }
                else if (Patient_Search.params.ParentCtrl == "Patient_MessageAdd") {
                    Patient_Search.ActionBit = true;

                }
                else if (Patient_Search.params.ParentCtrl == "Patient_MessageEdit") {
                    Patient_Search.ActionBit = true;

                }
                else if (Patient_Search.params.ParentCtrl == "Document_Viewer") {
                    Patient_Search.ActionBit = true;

                }
                else if (Patient_Search.params.ParentCtrl == "advancePaymentDetail") {
                    Patient_Search.ActionBit = true;

                }
                else if (Patient_Search.params.ParentCtrl == "Patient_AdvancePayment") {
                    Patient_Search.ActionBit = true;

                }
                else if (Patient_Search.params.ParentCtrl == "ChargeBatch_Viewer") {
                    Patient_Search.ActionBit = true;
                }
                else if (Patient_Search.params.ParentCtrl == "ERA_ChargeSearch") {
                    Patient_Search.ActionBit = true;
                }
                else if (Patient_Search.params.ParentCtrl == "billTabChargeSearch" || Patient_Search.params.ParentCtrl == "Bill_ChargeSearch" || Patient_Search.params.ParentCtrl == "Patient_Case_Detail") {
                    Patient_Search.ActionBit = true;
                }
                else if (Patient_Search.params.ParentCtrl == "billTabUnClaimedAppointment" || Patient_Search.params.ParentCtrl == "Bill_ChargeSearch" || Patient_Search.params.ParentCtrl == "Patient_Case_Detail") {
                    Patient_Search.ActionBit = true;
                }
                else if (Patient_Search.params.ParentCtrl == "Encounter_CreateClaim" || Patient_Search.params.ParentCtrl == "billTabCopayReceipt") {
                    Patient_Search.ActionBit = true;
                }
                else if (Patient_Search.params.ParentCtrl != null && Patient_Search.params.ParentCtrl.indexOf("Encounter") != -1) {
                    Patient_Search.ActionBit = true;
                }
                else if (Patient_Search.params.ParentCtrl == "billTabClaimSubmission" || Patient_Search.params.ParentCtrl == "batchTabDocuments" || Patient_Search.params.ParentCtrl == "Document_Scan" || Patient_Search.params.ParentCtrl == "batchTabMessages" || Patient_Search.params.ParentCtrl == "patTabDocuments") {
                    Patient_Search.ActionBit = true;

                }
                else if (Patient_Search.params.ParentCtrl == "schwaitlistdetail") {
                    Patient_Search.ActionBit = true;

                }
                else if (Patient_Search.params.ParentCtrl == "schTabWaitList") {
                    Patient_Search.ActionBit = true;

                }
                else if (Patient_Search.params.ParentCtrl == "billTabPaymentPosting" || Patient_Search.params.ParentCtrl == "Bill_PaymentPosting") {
                    Patient_Search.ActionBit = true;

                }
                else if (Patient_Search.params.ParentCtrl == "billTabPatientStatement" || Patient_Search.params.ParentCtrl == "Bill_PatientStatement") {
                    Patient_Search.ActionBit = true;
                }
                else if (Patient_Search.params.ParentCtrl == "schTabSearch") {
                    Patient_Search.ActionBit = true;

                }
                else if (Patient_Search.params.ParentCtrl == "Document_Import") {
                    Patient_Search.ActionBit = true;
                }
                else if (Patient_Search.params.ParentCtrl == "Document_Export") {
                    Patient_Search.ActionBit = true;
                }
                else if (Patient_Search.params.ParentCtrl == "billTabFollowUpPatientAR") {
                    Patient_Search.ActionBit = true;
                }
                else if (Patient_Search.params.ParentCtrl == "billTabFollowUpInsuranceAR") {
                    Patient_Search.ActionBit = true;
                }
                else if (Patient_Search.params.ParentCtrl == "batchTabEncounter") {
                    Patient_Search.ActionBit = true;
                }
                else if (Patient_Search.params.ParentCtrl == "Batch_ImportHL7ImmunizationBatch") {
                    Patient_Search.ActionBit = true;
                }
                else if (Patient_Search.params.ParentCtrl == "batchTabPatientEligibility") {
                    Patient_Search.ActionBit = true;
                } else if (Patient_Search.params.ParentCtrl == "batchTabPatientEligibility") {
                    Patient_Search.ActionBit = true;
                }
                else if (Patient_Search.params.ParentCtrl == "Patient_AccountManager") {
                    Patient_Search.ActionBit = true;
                }
                else if (Patient_Search.params.ParentCtrl == "Patient_MessageCompose") {
                    Patient_Search.ActionBit = true;
                }
                else if (Patient_Search.params.ParentCtrl == "Patient_MessageCreate") {
                    Patient_Search.ActionBit = true;
                }
                else if (Patient_Search.params.ParentCtrl == "mstrTabDashBoard") {
                    Patient_Search.ActionBit = true;
                }

            }

        }
        if (Patient_Search.ActionBit == true) {

            $('#' + Patient_Search.params["PanelID"] + ' #pnlPatient_Result #ColumnAction').removeClass("size95").addClass("size15");
            if (Patient_Search.PatientAddbit != false) {
                $('#' + Patient_Search.params["PanelID"] + ' #btnPatientAdd').css("display", "none");
                $('#' + Patient_Search.params["PanelID"] + ' #btnPatientQuickSearch').css("display", "none");
            }
        }
    },

    ShowSearchFiels: function (obj) {
        obj = $(obj);
        if (obj.text() == "Advance Search") {
            obj.text('Basic Search');
            $('#' + Patient_Search.params["PanelID"] + ' div[divtype="AdvancedSearch"]').each(function () {
                $(this).css("display", "inline");
            });
            //$('#' + Patient_Search.params["PanelID"] + ' div#pnlPatient_Search').attr("").css("display", "none");
        }
        else if (obj.text() == "Basic Search") {
            obj.text('Advance Search');
            $('#' + Patient_Search.params["PanelID"] + ' div[divtype="AdvancedSearch"]').each(function () {

                $(this).find('[type=hidden],[type=text], textarea, ul').each(function () {
                    $(this).val('');
                });

                $(this).find('[type=checkbox], [type=radio]').each(function () {
                    this.checked = false;
                });

                $(this).find('select').each(function () {
                    $(this).find('option:selected').removeAttr('selected');
                    $(this).find('option[value=""]').attr('selected', 'selected');
                });

                $(this).css("display", "none");
            });
            $('#' + Patient_Search.params["PanelID"] + ' select#ddlActive').val('1');
        }
    },
    OpenGuarantorDetail: function () {
        //Patient_Guarantor.GuarantorEdit($('#' + Patient_Search.params["PanelID"] + ' #hfGuarantor').val(), "demographicDetail");
        var params = [];
        params["GuarantorId"] = $('#' + Patient_Search.params["PanelID"] + ' #hfGuarantor').val();
        params["mode"] = "Edit";
        params["RefCtrl"] = "txtGuarantor";
        params["ParentCtrl"] = 'Patient_Search';
        params["Address1"] = $('#' + Patient_Search.params["PanelID"] + ' #txtAddress1').val();
        params["Zip"] = $('#' + Patient_Search.params["PanelID"] + ' #txtZip').val();
        params["City"] = $('#' + Patient_Search.params["PanelID"] + ' #txtCity').val();
        params["State"] = $('#' + Patient_Search.params["PanelID"] + ' #txtState').val();
        LoadActionPan('guarantorDetail', params);
    },
    OpenGuarantor: function () {
        var params = [];
        params["GuarantorId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Patient_Search";
        params["Address1"] = $('#' + Patient_Search.params["PanelID"] + ' #txtAddress1').val();
        params["Zip"] = $('#' + Patient_Search.params["PanelID"] + ' #txtZip').val();
        params["City"] = $('#' + Patient_Search.params["PanelID"] + ' #txtCity').val();
        params["State"] = $('#' + Patient_Search.params["PanelID"] + ' #txtState').val();
        params["RefCtrl"] = "txtGuarantor";
        LoadActionPan('Patient_Guarantor', params);
    },
    ValidateSearchCriteria: function () {

        utility.ValidateSearchCriteria(Patient_Search.params["PanelID"] + " #frmPatientSearch", function () {

            //check if all fields are empty and only active filter value is active then alert to enter seach crireria.
            var isok = false;
            var active_ = $("#" + Patient_Search.params["PanelID"] + " #frmPatientSearch #ddlActive").val();
            if (active_ == "1") {
                $("#" + Patient_Search.params["PanelID"] + " #frmPatientSearch").find('select,[type=text],[type=hidden],[type=checkbox]').each(function () {
                    if ((($(this).attr('id') != "ddlActive") && $(this).hasClass('ValidateCriteria') == true && $(this).val() != "") ||
                        ($(this).attr('id') == "chkIncompleteDemographics" && $(this).prop('checked') == true) || ($(this).attr('id') == "chkBadAddress" && $(this).prop('checked') == true)) {
                        isok = true;
                        return false;
                    }
                });
            }
            else
                isok = true;

            if (isok == true) {
                Patient_Search.PatientSearch(null, null, null, 0);
                Patient_Search.IsRecentPatientSelected = 0;
            }

            else
                utility.DisplayMessages('Too many records! <br> Please Specify filter criteria ', 2);


        }, function () {
            utility.DisplayMessages('Too many records! <br> Please Specify filter criteria ', 2);
        });
    },

    PatientSearch: function (PatientId, PageNo, rpp, isRecentPatients, badAddressCheck) {

        if (isRecentPatients == 1) {
            $("#" + Patient_Search.params["PanelID"] + " #hfIsRecentPatientCalled").val("1");
            Patient_Search.IsRecentPatientSelected = 1;
        } else {
            $("#" + Patient_Search.params["PanelID"] + " #hfIsRecentPatientCalled").val("0");
            Patient_Search.IsRecentPatientSelected = 0;
        }
        if (badAddressCheck == true) {
            $("#" + Patient_Search.params["PanelID"] + " #chkBadAddress").prop('checked', true);
        }
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Demographic", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#" + Patient_Search.params["PanelID"] + " #pnlPatient_Result").css("display") == "none") {
                    $("#" + Patient_Search.params["PanelID"] + " #pnlPatient_Result").show();
                }
                //Patient_Search.PatientGridLoad1("");
                var self = $("#" + Patient_Search.params["PanelID"]);
                var myJSON = self.getMyJSONByName();

                if (Patient_Search.params.IsUnsolitedPatientSearch == true && Patient_Search.params.IsFirstLoadFromDash == true) {       // If user comes from dashboard Unsolicited Results to search this patient
                    var name = Patient_Search.params["PatientName"].split(',');
                    var LastName = name[0];
                    var FirstName = name[1];
                    $('#Patient_Search #txtSearchFirstName').val(FirstName);
                    $('#Patient_Search #txtSearchLastName').val(LastName);
                    myJSON = JSON.parse(myJSON);
                    myJSON.FirstName = FirstName.trim();
                    myJSON.LastName = LastName.trim();
                    myJSON = JSON.stringify(myJSON);
                    isRecentPatients = 0;
                    Patient_Search.params.IsFirstLoadFromDash = false;
                }
                Patient_Search.SearchPatient(myJSON, PatientId, PageNo, rpp, isRecentPatients).done(function (response) {
                    if (response.status != false) {
                        if (response.PatientCount > 0) {
                            $("#" + Patient_Search.params["PanelID"] + " #divPatientPaging").css("display", "inline");
                            //Showing 1 to 15 of 15 entries
                            var RecordsPerPage = rpp != null ? rpp : 15;
                            var CurrentPage = PageNo != null ? PageNo : 1;
                            if (isRecentPatients == 1) {
                                var RecordsPerPage1 = response.PatientCount;
                            } else {
                                var RecordsPerPage1 = RecordsPerPage;
                            }



                            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage1);
                            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                            if (PageNo == null) {
                                utility.GetCustomPaging(Patient_Search.params["PanelID"] + " #divPatientPaging", response.iTotalDisplayRecords, 5, "Patient_Search", CurrentPage, RecordsPerPage1);
                            }

                            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage1)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage1)) : response.iTotalDisplayRecords;
                            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage1) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                            $("#" + Patient_Search.params["PanelID"] + " #divPatientPaging #divShowingEntries").text(showingText);
                            // Change Background Color to Black for selected page
                            $("#" + Patient_Search.params["PanelID"] + " li").each(function () {
                                if ($(this).text() == CurrentPage) {
                                    $(this).attr("class", "active");
                                }
                                else
                                    $(this).removeAttr("class");
                            });

                            Patient_Search.NotFoundPatientDetails = {};
                        }
                        else {
                            $("#" + Patient_Search.params["PanelID"] + " #divPatientPaging").css("display", "none");
                            Patient_Search.NotFoundPatientDetails["FirstName"] = $('#Patient_Search #txtSearchFirstName').val();
                            Patient_Search.NotFoundPatientDetails["LastName"] = $('#Patient_Search #txtSearchLastName').val();
                            Patient_Search.NotFoundPatientDetails["DateOfBirth"] = $('#Patient_Search #dtpSearchDOB').val();
                        }

                        Patient_Search.PatientGridLoad(response);


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

    PatientGridLoad: function (response) {
        $("#" + Patient_Search.params["PanelID"] + " #pnlPatient_Result #dgvPatient #selectAllPatients").prop("checked", false);
        $("#" + Patient_Search.params["PanelID"] + " #pnlPatient_Result #dgvPatient").dataTable().fnDestroy();
        $("#" + Patient_Search.params["PanelID"] + " #pnlPatient_Result #dgvPatient tbody").find("tr").remove();
        //if ($('#Patient_Search #txtGuarantor').val() == "") {
        //    $('#dgvPatient thead').find('#DynamicTH').hide();
        // }
        if (response.PatientCount > 0) {
            var PatientLoadJSONData = JSON.parse(response.PatientLoad_JSON);
            $.each(PatientLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                //if ($('#Patient_Search #txtGuarantor').val() != null && $('#Patient_Search #txtGuarantor').val() != "") {
                //    if (i == 0) {
                //        $('#dgvPatient thead').find('#DynamicTH').show();
                //}
                //}
                //else {
                //    $('#dgvPatient thead').find('#DynamicTH').hide();
                //}
                $row.attr("id", "gvPatient_row" + item.PatientId);
                $row.attr("PatientId", item.PatientId);
                if (item.IsActive == 1) {
                    isactive = 0;
                    activeRecord = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else if (item.IsActive == 2) {
                    isactive = 1;
                    activeRecord = "Deceased Record";
                    tglclass = "fa fa-toggle-on red";
                }
                else {
                    isactive = 1;
                    activeRecord = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }
                if (globalAppdata.IsFullSSN.toLowerCase() === 'false') {
                    if (item.SSN) {
                        var last4digit = item.SSN.slice(-4);
                        item.SSN = "XXX-XX-" +last4digit;
                }
            }
                var MethodMode = Patient_Search.getPatientSelectMethodName(item.PatientId, item.AccountNumber, item.FullName, item.FirstName, item.LastName, item.ReferringProviderId, item.ReferringProviderName, item.ProviderId, item.ProviderName, item.FacilityId, item.FacilityName, item.SelfPay, item.DOB, item.ConfidentialityCode, item.Gender);
                var ActionBit = false;

                var disableActions = false;
                var selectAction = '';
                var EditMethod = "Patient_Search.PatientAddEdit(" + item.PatientId.trim() + ",'Edit',event);";
                var ActiveInacvtiveMethod = "Patient_Search.ActiveInactivePatient(" + item.PatientId.trim() + "," + isactive + ",event);";
                var incompleteDemographicsSign = ''; //"&nbsp;<span class='yellow'>!</span>";

                if (item.IncompleteDemographics == "1") {
                    incompleteDemographicsSign = "&nbsp;<i class='fa fa-exclamation red' aria-hidden='true' title='Incomplete Demographics'></i>";
                }

                if (item.IsBreakGlassAllow) {
                    //As Current User is restricted to do any actions for Patient(Break the glass Business logic)
                    disableActions = true;
                    if (item.IsBreakGlassAllow == "True") {
                        selectAction = '<a class="btn btn-xs" href="javascript:void(0);"  onclick="Patient_Search.patientBreaktheGlass(' + item.PatientId.trim() + ',\'' + item.AccountNumber + '\',\'' + item.FullName + '\',\'' + item.FirstName + '\',\'' + item.LastName + '\',\'' + item.ReferringProviderId + '\',\'' + item.ReferringProviderName + '\',\'' + item.ProviderId + '\',\'' + item.ProviderName + '\',\'' + item.FacilityId + '\',\'' + item.FacilityName + '\',\'' + item.SelfPay + '\',event);"   title="Break The Glass"><i class="fa fa-gavel black"></i></a>&nbsp;';
                    } else if (item.IsBreakGlassAllow == "False") {
                        selectAction = '<a class="btn btn-xs" href="javascript:void(0);" onclick="utility.DisplayMessages(\'You are not authorized to view this patient.\',3);"  title="Access Restricted"><i class="fa fa-lock black"></i></a>&nbsp;';
                    }
                } else {
                    if (Patient_Search.ActionBit == false) {
                        var link = $('<a class="btn btn-xs"  title="Select Patient"><i class="fa fa-check black"></i></a>');
                        link.attr("onclick", MethodMode);
                        selectAction = link[0].outerHTML + '&nbsp;';
                    } else {
                        var link = $('<a class="btn btn-xs"  title="Select Patient"><i class="fa fa-check black"></i></a>');
                        link.attr("onclick", MethodMode);
                        selectAction = link[0].outerHTML + '&nbsp;';
                    }
                }
                //As Current User is restricted to do any actions for Patient(Break the glass Business logic)
                if (!disableActions) {
                    if (MethodMode != "") {
                        $row.attr("onclick", MethodMode);
                    }
                    else {
                        $row.attr("onclick", "utility.SelectGridRow($('#gvPatient_row" + item.PatientId + "'))");

                    }
                }
                var strAction = "";
                if (Patient_Search.ActionBit == false) {
                    if (disableActions) {
                        strAction = '<a class="btn btn-xs" href="#" onclick="' + EditMethod + '" disabled="' + disableActions + '" title="Edit Patient Info"><i class="fa fa-edit black"></i></a><a class="btn  btn-xs" href="#" disabled="' + disableActions + '" onclick="' + ActiveInacvtiveMethod + '" title="' + activeRecord + '"><i class="' + tglclass + '"></i></a>&nbsp;' + selectAction + incompleteDemographicsSign;
                    } else {
                        strAction = '<a class="btn btn-xs" href="#" onclick="' + EditMethod + '" title="Edit Patient Info"><i class="fa fa-edit black"></i></a><a class="btn  btn-xs" href="#" onclick="' + ActiveInacvtiveMethod + '" title="' + activeRecord + '"><i class="' + tglclass + '"></i></a>&nbsp;' + selectAction + incompleteDemographicsSign;
                    }

                }
                else {
                    strAction = selectAction + incompleteDemographicsSign;
                }

                //Start Farooq Ahmad 26/4/2016 Implementation of Data Portability in patient search
                var CheckBox = '<input type="checkbox" name="' + item.PatientId + '" value="' + item.ProviderId + '" id="' + item.PatientId + '" class="patient" />';
                //End Farooq Ahmad 26/4/2016 Implementation of Data Portability in patient search
                if (Patient_Search.params.ParentCtrl == "Patient_Family_Detail") {
                    if ($('#PatientProfile #hfPatientId').val() != item.PatientId) {
                        $row.append('<td style="display:none;">' + item.PatientId + '</td><td>' + CheckBox + '</td><td class="noWordBreak">' + strAction + '</td><td>' + item.AccountNumber + '</td><td>' + item.LastName + '</td><td>' + item.FirstName + '</td><td class="noWordBreak">' + item.SSN + '</td><td>' + item.Gender + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOB) + '</td><td class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.Address1 + '">' + item.Address1 + '</td><td>' + item.ProviderName + '</td><td>' + item.FacilityName + '</td><td>' + utility.RemoveTimeFromDate(null, item.AppointmentDate) + '</td><td>' + item.PrimaryInsurance + '</td><td>' + item.CoverageType + '</td>' + '<td>' + item.GuarantorName + '</td>');
                        $("#" + Patient_Search.params["PanelID"] + " #pnlPatient_Result #dgvPatient tbody").last().append($row);
                    }
                }
                else {
                    $row.append('<td style="display:none;">' + item.PatientId + '</td><td>' + CheckBox + '</td><td class="noWordBreak">' + strAction + '</td><td>' + item.AccountNumber + '</td><td>' + item.LastName + '</td><td>' + item.FirstName + '</td><td class="noWordBreak">' + item.SSN + '</td><td>' + item.Gender + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOB) + '</td><td class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.Address1 + '">' + item.Address1 + '</td><td>' + item.ProviderName + '</td><td>' + item.FacilityName + '</td><td>' + utility.RemoveTimeFromDate(null, item.AppointmentDate) + '</td><td>' + item.PrimaryInsurance + '</td><td>' + item.CoverageType + '</td>' + '<td>' + item.GuarantorName + '</td>');
                    $("#" + Patient_Search.params["PanelID"] + " #pnlPatient_Result #dgvPatient tbody").last().append($row);
                }

                //Begin 05-Jan-2015  Edited By Azeem Raza Tayyab Bug # PMS-3136            
            });
        }
        else {
            $('#' + Patient_Search.params["PanelID"] + ' #divPatientPaging').css("display", "none");
            $("#" + Patient_Search.params["PanelID"] + " #pnlPatient_Result #dgvPatient").DataTable({
                "language": {
                    "emptyTable": "No Patient Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Patient_Search.params["PanelID"] + " #pnlPatient_Result #dgvPatient"))
            ;
        else
            $("#" + Patient_Search.params["PanelID"] + " #pnlPatient_Result #dgvPatient").DataTable({
                "bInfo": false, "ordering": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "bFilter": false, "aoColumnDefs": [{
                    "bSortable": false, "aTargets": [1]
                }]
            }); // to remove records per page dropdown


        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({
            container: 'body'
        });
    },

    getPatientSelectMethodName: function (PatientId, AccountNumber, FullName, FirstName, LastName, ReferringProviderId, ReferringProviderName, ProviderId, ProviderName, FacilityId, FacilityName, SelfPay, DOB, ConfidentialityCode, Gender) {
        var MethodMode = "";
        if (Patient_Search.params != null) {
            if (Patient_Search.params.ParentCtrl != null) {
                if (Patient_Search.params.ParentCtrl == "emergencyContactDetail")
                    MethodMode = "emergencyContactDetail.FillPatientInfoFromSearch(" + PatientId.trim() + ",event);";

                else if (Patient_Search.params.ParentCtrl == "Patient_Family_Detail")
                    MethodMode = "Patient_Family_Detail.FillPatientInfoFromSearch(" + PatientId.trim() + ",event);";

                else if (Patient_Search.params.ParentCtrl == "appointmentDetail")
                    MethodMode = "appointmentDetail.FillPatientInfoFromSearch(" + PatientId.trim() + ",event);"
                else if (Patient_Search.params.ParentCtrl == "Scheduling_Force_Booking")
                    MethodMode = "Scheduling_Force_Booking.FillPatientInfoFromSearch(" + PatientId.trim() + ",event);"
                else if (Patient_Search.params.ParentCtrl == "billTabClaimSubmission")
                    MethodMode = "Bill_ClaimSubmission.FillPatientInfoFromSearch('" + AccountNumber.trim() + "','" + PatientId.trim() + "',event);"

                else if (Patient_Search.params.ParentCtrl == "billTabCopayReceipt")
                    MethodMode = "Scheduling_UnallocatedCopayment_Search.FillPatientInfoFromSearch('" + AccountNumber.trim() + "','" + PatientId.trim() + "',event);"

                else if (Patient_Search.params.ParentCtrl == "batchTabDocuments")
                    MethodMode = "Patient_Document.FillPatientInfoFromSearch(" + PatientId.trim() + ",'" + AccountNumber + "', '" + FirstName + "', '" + LastName + "','" + Patient_Search.params.RefCtrl + "',event);"

                else if (Patient_Search.params.ParentCtrl == "Document_Scan")
                    MethodMode = "Document_Scan.FillPatientInfoFromSearch(" + PatientId.trim() + ",'" + AccountNumber + "', '" + FirstName + "', '" + LastName + "',event);"

                else if (Patient_Search.params.ParentCtrl == "patTabDocuments" || Patient_Search.params.ParentCtrl == "Patient_Document")
                    MethodMode = "Patient_Document.FillPatientInfoFromSearch(" + PatientId.trim() + ",'" + AccountNumber + "', '" + FirstName + "', '" + LastName + "','" + Patient_Search.params.RefCtrl + "',event);"
                else if (Patient_Search.params.ParentCtrl == "Batch_ImportHL7ImmunizationBatch")
                    MethodMode = "Batch_ImportHL7ImmunizationBatch.FillPatientInfoFromSearch('" + AccountNumber.trim() + "','" + PatientId.trim() + "',event);"

                else if (Patient_Search.params.ParentCtrl == "batchTabMessages")
                    MethodMode = "Patient_Message.FillPatientInfoFromSearch('" + AccountNumber.trim() + "','" + PatientId.trim() + "',event);"

                else if (Patient_Search.params.ParentCtrl == "schTabSearch")
                    MethodMode = "Scheduling_Search.FillPatientInfoFromSearch(" + PatientId.trim() + ",'" + AccountNumber + "', '" + FirstName + "', '" + LastName + "',event);"

                else if (Patient_Search.params.ParentCtrl == "Patient_MessageAdd")
                    MethodMode = "Patient_MessageAdd.FillPatientInfoFromSearch(" + PatientId.trim() + ",\"" + AccountNumber + " - " + FullName + "\",event);"

                else if (Patient_Search.params.ParentCtrl == "Patient_MessageEdit")
                    MethodMode = "Patient_MessageEdit.FillPatientInfoFromSearch(" + PatientId.trim() + ",\"" + AccountNumber + " - " + FullName + "\",event);"

                else if (Patient_Search.params.ParentCtrl == "Document_Viewer")
                    MethodMode = "Document_Viewer.FillPatientInfoFromSearch(" + PatientId.trim() + ",\"" + AccountNumber + " - " + FullName + "\",event);"

                else if (Patient_Search.params.ParentCtrl == "ChargeBatch_Viewer")
                    MethodMode = "ChargeBatch_Viewer.FillPatientInfoFromSearch(" + PatientId.trim() + ",\"" + AccountNumber + " - " + FullName + "\",event);"
                else if (Patient_Search.params.ParentCtrl == "Document_Import")
                    MethodMode = "Document_Import.FillPatientInfoFromSearch(" + PatientId.trim() + ",\"" + AccountNumber + " - " + FullName + "\",event);"
                else if (Patient_Search.params.ParentCtrl == "Patient_Document_Search")
                    MethodMode = "Patient_Document_Search.FillPatientInfoFromSearch(" + PatientId.trim() + ",\"" + AccountNumber + " - " + FullName + "\",event);"

                else if (Patient_Search.params.ParentCtrl == "Document_Export")
                    MethodMode = "Document_Export.FillPatientInfoFromSearch(" + PatientId.trim() + ",\"" + AccountNumber + " - " + LastName + ", " + FirstName + "\",event);"

                else if (Patient_Search.params.ParentCtrl == "ERA_ChargeSearch")
                    MethodMode = "ERA_ChargeSearch.FillPatientInfoFromSearch(" + PatientId.trim() + ",'" + AccountNumber + "',event);"

                else if (Patient_Search.params.ParentCtrl == "billTabChargeSearch" || Patient_Search.params.ParentCtrl == "Bill_ChargeSearch" || Patient_Search.params.ParentCtrl == "Patient_Case_Detail") {
                    if (Patient_Search.params["ForNewClaim"] != null) {
                        MethodMode = "Patient_Search.FillPatientInfoForNewClaim(" + PatientId.trim() + ",\"" + AccountNumber + " - " + FullName + "\",'" + ReferringProviderId + "','" + ReferringProviderName + "','" + ProviderId + "','" + ProviderName + "','" + FacilityId + "','" + FacilityName + "','" + SelfPay + "','" + DOB + "','" + Gender + "','" + Patient_Search.params.ParentCtrl + "',event);";
                    } else {
                        MethodMode = "Bill_ChargeSearch.FillPatientInfoFromSearch(" + PatientId.trim() + ",'" + AccountNumber + "',\"" + FullName + "\",event);"
                    }
                }

                else if (Patient_Search.params.ParentCtrl == "billTabUnClaimedAppointment")
                    if (Patient_Search.params["ForNewClaim"] != null) {
                        MethodMode = "Patient_Search.FillPatientInfoForNewClaim(" + PatientId.trim() + ",\"" + AccountNumber + " - " + FullName + "\",'" + ReferringProviderId + "','" + ReferringProviderName + "','" + ProviderId + "','" + ProviderName + "','" + FacilityId + "','" + FacilityName + "','" + SelfPay + "','" + DOB + "','" + Gender + "','" + Patient_Search.params.ParentCtrl + "',event);"
                    } else {
                        MethodMode = "Bill_UnClaimedAppointment.FillPatientInfoFromSearch(" + PatientId.trim() + ",'" + AccountNumber + "',\"" + FullName + "\",event);"
                    }

                else if (Patient_Search.params.ParentCtrl == "billTabFollowUpPatientAR")
                    MethodMode = "Bill_FollowUpPatientAR.FillPatientInfoFromSearch(" + PatientId.trim() + ",'" + AccountNumber + "',\"" + FullName + "\",event);"

                else if (Patient_Search.params.ParentCtrl == "billTabFollowUpInsuranceAR")
                    MethodMode = "Bill_FollowUpInsuranceAR.FillPatientInfoFromSearch(" + PatientId.trim() + ",'" + AccountNumber + "',\"" + FullName + "\",event);"

                else if (Patient_Search.params.ParentCtrl == "batchTabEncounter")
                    MethodMode = "Batch_Encounter.FillPatientInfoFromSearch(" + PatientId.trim() + ",\"" + AccountNumber + " - " + FullName + "\",event);"
                else if (Patient_Search.params.ParentCtrl == "Encounter_CreateClaim")
                    MethodMode = "Encounter_CreateClaim.FillPatientInfoFromSearch(" + PatientId.trim() + ",\"" + AccountNumber + " - " + FullName + "\",'" + ReferringProviderId + "','" + ReferringProviderName + "','" + ProviderId + "','" + ProviderName + "','" + FacilityId + "','" + FacilityName + "','" + SelfPay + "',event,'" + DOB + "','" + Gender + "');"
                else if (Patient_Search.params.ParentCtrl != null && Patient_Search.params.ParentCtrl.indexOf("Encounter") != -1)
                    MethodMode = "EncounterChargeCapture.FillPatientInfoFromSearch(" + PatientId.trim() + ",\"" + AccountNumber + " - " + FullName + "\",'" + ReferringProviderId + "','" + ReferringProviderName + "','" + ProviderId + "','" + ProviderName + "','" + FacilityId + "','" + FacilityName + "','" + SelfPay + "',event);"

                else if (Patient_Search.params.ParentCtrl == "Patient_AdvancePayment")
                    MethodMode = "Patient_AdvancePayment.FillPatientInfoFromSearch(" + PatientId.trim() + ",\"" + AccountNumber + " - " + FullName + "\",event);"

                else if (Patient_Search.params.ParentCtrl == "advancePaymentDetail")
                    MethodMode = "advancePaymentDetail.FillPatientInfoFromSearch(" + PatientId.trim() + ",\"" + AccountNumber + " - " + FullName + "\",event);"

                else if (Patient_Search.params.ParentCtrl == "schTabWaitList")
                    MethodMode = "Scheduling_WaitList.FillPatientInfoFromSearch(" + PatientId.trim() + ",event);"
                else if (Patient_Search.params.ParentCtrl == "schwaitlistdetail")
                    MethodMode = "schwaitlistdetail.FillPatientInfoFromSearch(" + PatientId.trim() + ",event);"
                else if (Patient_Search.params.ParentCtrl == "billTabPaymentPosting" || Patient_Search.params.ParentCtrl == "Bill_PaymentPosting")
                    MethodMode = "Bill_PaymentPosting.FillPatientInfoFromSearch(" + PatientId.trim() + ",\"" + AccountNumber + " - " + FullName + "\",event);"
                else if (Patient_Search.params.ParentCtrl == "Patient_Case")
                    MethodMode = "Patient_Case.FillPatientInfoFromSearch(" + PatientId.trim() + ",\"" + AccountNumber + "\",event);"
                else if (Patient_Search.params.ParentCtrl == "billTabPatientStatement" || Patient_Search.params.ParentCtrl == "Bill_PatientStatement")
                    MethodMode = "Bill_PatientStatement.FillPatientInfoFromSearch(" + PatientId.trim() + ",\"" + AccountNumber + " - " + FullName + "\",event);"
                else if (Patient_Search.params.ParentCtrl == "batchTabPatientEligibility")
                    MethodMode = "Patient_Eligibility.FillPatientInfoFromSearch('" + PatientId.trim() + "','" + AccountNumber + "' ,\"" + FirstName + "\",\"" + LastName + "\",event);"

                else if (Patient_Search.params.ParentCtrl == "Patient_AccountManager")
                    MethodMode = "Patient_AccountManager.FillPatientInfoFromSearch(" + PatientId.trim() + ",'" + AccountNumber + "',\"" + FirstName + "\",\"" + LastName + "\",event);"
                else if (Patient_Search.params.ParentCtrl == "adminTabAuditReport")
                    MethodMode = "Clinical_AuditReport.FillPatientInfoFromSearch(" + PatientId.trim() + ",'" + AccountNumber + "',\"" + FirstName + "\",\"" + LastName + "\",event);"
                else if (Patient_Search.params.ParentCtrl == "Patient_MessageCompose")
                    MethodMode = "Patient_MessageCompose.FillPatientInfoFromSearch(" + PatientId.trim() + ",'" + AccountNumber + "',\"" + FullName + "\",event);"
                else if (Patient_Search.params.ParentCtrl == "Patient_MessageCreate")
                    MethodMode = "Patient_MessageCreate.FillPatientInfoFromSearch(" + PatientId.trim() + ",'" + AccountNumber + "',\"" + FullName + "\",event);"
                else if (Patient_Search.params.ParentCtrl == "Batch_ClinicalQualityMeasureDetail")
                    MethodMode = "Batch_ClinicalQualityMeasureDetail.FillPatientInfoFromSearch(" + PatientId.trim() + ",'" + AccountNumber + "',\"" + FullName + "\",event);"
                else if (Patient_Search.params.ParentCtrl == "iTrack_QualityMeasureDetail")
                    MethodMode = "iTrack_QualityMeasureDetail.FillPatientInfoFromSearch(" + PatientId.trim() + ",'" + AccountNumber + "',\"" + FullName + "\",event);"
                else if (Patient_Search.params.ParentCtrl == "iTrack_PromotingInteroperabilityDetail")
                    MethodMode = "iTrack_PromotingInteroperabilityDetail.FillPatientInfoFromSearch(" + PatientId.trim() + ",'" + AccountNumber + "', '" + FirstName + "', '" + LastName + "','" + Patient_Search.params.RefCtrl + "',event);"
                else if (Patient_Search.params.ParentCtrl == "mstrTabDashBoard" && Patient_Search.params.DashboardLabOrder == "1")
                    MethodMode = "DashBoard.UpdatePatientOfLabOrder(" + PatientId.trim() + ",'" + AccountNumber + "',\"" + FullName + "\",event);"
                else if (Patient_Search.params.ParentCtrl == "mstrTabReports")
                    MethodMode = "ReportsSSRSDashboard.FillPatientInfoFromSearch(" + PatientId.trim() + ",'" + AccountNumber + "',\"" + FullName + "\",\"" + FirstName + "\",\"" + LastName + "\",event);"

                else if (Patient_Search.params.ParentCtrl == "mstrTabDashBoard") {

                    MethodMode = "DashBoard.FillPatientInfoFromSearch(" + PatientId.trim() + ",'" + AccountNumber + "',\"" + LastName + ", " + FirstName + "\",event);"

                }
                else if (Patient_Search.params.ParentCtrl == "Batch_ClinicalQualityMeasure")
                    MethodMode = "Batch_ClinicalQualityMeasure.FillPatientInfoFromSearch(" + PatientId.trim() + ",'" + AccountNumber + "',\"" + LastName + ", " + FirstName + "\",event);"
                else if (Patient_Search.params.ParentCtrl == "iTrackeCQMs")
                    MethodMode = "iTrack_eCQMs.FillPatientInfoFromSearch(" + PatientId.trim() + ",'" + AccountNumber + "',\"" + LastName + ", " + FirstName + "\",event);"
                else if (Patient_Search.params.ParentCtrl == "iTrack_eCQMsDetail")
                    MethodMode = "iTrack_eCQMsDetail.FillPatientInfoFromSearch(" + PatientId.trim() + ",'" + AccountNumber + "',\"" + LastName + ", " + FirstName + "\",event);"

                else {
                    //Begin 28/4/2016  Edit By M Ahmad Imran Bug # EMR-800
                    MethodMode = "Patient_Search.SelectPatient(" + PatientId.trim() + ",'" + AccountNumber + "','" + ConfidentialityCode + "',event);";//billTabPaymentPosting
                    //End 28/4/2016  Edit By M Ahmad Imran Bug # EMR-800
                }
            }
            else {
                //Begin 28/4/2016  Edit By M Ahmad Imran Bug # EMR-800
                MethodMode = "Patient_Search.SelectPatient(" + PatientId.trim() + ",'" + AccountNumber + "','" + ConfidentialityCode + "',event);";
                //End 28/4/2016  Edit By M Ahmad Imran Bug # EMR-800
            }
        }
        else {
            //Begin 28/4/2016  Edit By M Ahmad Imran Bug # EMR-800
            MethodMode = "Patient_Search.SelectPatient(" + PatientId.trim() + ",'" + AccountNumber + "','" + ConfidentialityCode + "',event);";
            //End 28/4/2016  Edit By M Ahmad Imran Bug # EMR-800
        }

        return MethodMode;
    },

    patientBreaktheGlass: function (PatientId, AccountNumber, FullName, FirstName, LastName, ReferringProviderId, ReferringProviderName, ProviderId, ProviderName, FacilityId, FacilityName, SelfPay, event) {
        if (event != null) {
            event.stopPropagation();
        }
        if (params["patientID"] != PatientId) {
            Patient_Search.showRestrictUser_PatientConsent("#" + Patient_Search.params["PanelID"] + " #actionPanPatientSearch", PatientId, AccountNumber, FullName, FirstName, LastName);
        } else {

            var MethodMode = Patient_Search.getPatientSelectMethodName(PatientId.toString(), AccountNumber, FullName, FirstName, LastName, ReferringProviderId, ReferringProviderName, ProviderId, ProviderName, FacilityId, FacilityName, SelfPay);
            eval(MethodMode);
        }

        // setInterval(funcAfterBreakTheGlass, 5);
    },
    saveBreakGlassReason: function () {
        var BreakTheReason = $("#" + Patient_Search.params["PanelID"] + " #actionPanPatientSearch #BreakTheReason").val();
        if (BreakTheReason && BreakTheReason.split(' ').join() != '') {
            var PatientId = $("#" + Patient_Search.params["PanelID"] + " #actionPanPatientSearch #hfPatientId").val();
            var AccountNumber = $("#" + Patient_Search.params["PanelID"] + " #actionPanPatientSearch #hfAccountNumber").val();
            var FullName = $("#" + Patient_Search.params["PanelID"] + " #actionPanPatientSearch #hfFullName").val();
            var FirstName = $("#" + Patient_Search.params["PanelID"] + " #actionPanPatientSearch #hfFirstName").val();
            var LastName = $("#" + Patient_Search.params["PanelID"] + " #actionPanPatientSearch #hfLastName").val();
            Restrict_User.saveBreakGlassReason_DBCall(BreakTheReason, PatientId).done(function () {
                var MethodMode = Patient_Search.getPatientSelectMethodName(PatientId, AccountNumber, FullName, FirstName, LastName);
                Patient_Search.UnLoadRestrictUser_PatientConsent();
                //Begin 28/4/2016  Edit By M Ahmad Imran Bug # EMR-800
                $("#" + Patient_Search.params["PanelID"] + " #actionPanPatientSearch").on('hidden.bs.modal', function () {

                    var tempTag = document.createElement('a');
                    tempTag.setAttribute('onclick', MethodMode);
                    $(tempTag).trigger('click');
                    $(tempTag).remove();
                    //eval(MethodMode);
                });
                //End 28/4/2016  Edit By M Ahmad Imran Bug # EMR-800

            });
        } else {
            utility.DisplayMessages('Please enter the Break the Glass Reason', 2);
        }


    },
    //***************************************************

    // this function is used by both Notes and Progress Note Form
    showRestrictUser_PatientConsent: function (ActionPanID, PatientId, AccountNumber, FullName, FirstName, LastName) {
        $(ActionPanID).prepend($("#Patient_Search #pnlRestrictUser_PatientConsent").html());
        $(ActionPanID).modal({
            show: 'true',
            backdrop: 'static',
            keyboard: false

        }).on('hidden.bs.modal', function () {
            $('body').addClass('modal-open');
        }).on('shown.bs.modal', function () {
            $("#" + Patient_Search.params["PanelID"] + " #actionPanPatientSearch #lblPatientName").text(FirstName + ' ' + LastName);
            $("#" + Patient_Search.params["PanelID"] + " #actionPanPatientSearch #hfPatientId").val(PatientId);
            $("#" + Patient_Search.params["PanelID"] + " #actionPanPatientSearch #hfAccountNumber").val(AccountNumber);
            $("#" + Patient_Search.params["PanelID"] + " #actionPanPatientSearch #hfFullName").val(FullName);
            $("#" + Patient_Search.params["PanelID"] + " #actionPanPatientSearch #hfFirstName").val(FirstName);
            $("#" + Patient_Search.params["PanelID"] + " #actionPanPatientSearch #hfLastName").val(LastName);
        });
    },

    // this function is used by both Notes and Progress Note Form
    UnLoadRestrictUser_PatientConsent: function () {
        //Begin 28/4/2016  Edit By M Ahmad Imran Bug # EMR-800
        var objDeffered = $.Deferred();
        $("#" + Patient_Search.params["PanelID"] + " #actionPanPatientSearch").html('');
        $("#" + Patient_Search.params["PanelID"] + " #actionPanPatientSearch").modal('hide');
        objDeffered.resolve();
        return objDeffered;
        //End 28/4/2016  Edit By M Ahmad Imran Bug # EMR-800
    },

    //***************************************************

    PatientGridLoad1: function (response) {
        var self = $("#" + Patient_Search.params["PanelID"]);
        var myJSON = self.getMyJSON();
        var data = "PatientData=" + myJSON; //+ "&PatientID=" + PatientId;

        $("#" + Patient_Search.params["PanelID"] + " #pnlPatient_Result #dgvPatient").dataTable({
            "processing": true,
            "serverSide": true,
            "ajax": MDVisionService.defaultService1(data, 'PATIENT_SEARCH', 'SEARCH_PATIENT'),
            "columns": [
                { "jsondata": "PatientId" },
                { "jsondata": "PatientId" },
                { "jsondata": "AccountNumber" },
                { "jsondata": "FirstName" },
                { "jsondata": "LastName" },
                { "jsondata": "Gender" },
                { "jsondata": "DOB" },
                { "jsondata": "FirstName" }
            ]
        });

    },
    ActiveInactivePatient: function (patientId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($("#gvPatient_row" + patientId));
        AppPrivileges.GetFormPrivileges("Demographic", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = patientId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Patient_Search.PatientUpdateActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                if ($("#" + Patient_Search.params["PanelID"] + " #hfIsRecentPatientCalled").val() == "1") {
                                    Patient_Search.PatientSearch(null, null, null, 1);
                                } else {
                                    Patient_Search.PatientSearch(null, null, null, 0);
                                }
                                //Patient_Search.PatientSearch();
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                    '3', null, null, null, IsActive
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    SearchPatient: function (PatientData, PatientId, PageNo, rpp, isRecentPatients) {

        PageNo = PageNo == null || PageNo == "" ? 1 : PageNo;
        rpp = rpp == null || rpp == "" ? 15 : rpp;

        var objData = JSON.parse(PatientData);

        if (isRecentPatients) {
            objData["AppointmentDate"] = null;
        }

        objData["PatientID"] = PatientId;
        objData["PageNo"] = PageNo;
        objData["CommandType"] = "Search";
        objData["rpp"] = rpp;
        objData["IsRecentPatients"] = isRecentPatients;

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Patient", "Patient");
    },

    BindGuarantor: function () {
        var Ctrl = $('#Patient_Search #txtGuarantor');
        var func = function () { return utility.GetGuarontorArray(Ctrl.val()) };
        var hfCtrl = $("#Patient_Search #hfGuarantor");
        var onChange = function () { Patient_Demographic.CommunicationValidation(Ctrl); };
        utility.BindKendoAutoComplete(Ctrl, 3, "value", "contains", null, func, hfCtrl, null, onChange);
    },
    ClearPatientSearch: function () {

        $('#' + Patient_Search.params["PanelID"] + ' #pnlPatient_Search').resetAllControls();
        $('#' + Patient_Search.params["PanelID"] + ' #pnlPatient_Search #ddlActive').find('option:selected').removeAttr('selected');
        //$('#' + Patient_Search.params["PanelID"] + ' #pnlPatient_Search #ddlActive option[value=1]').attr('selected', 'selected');
        $('#' + Patient_Search.params["PanelID"] + ' #pnlPatient_Search #ddlActive').val('1');
        $('#' + Patient_Search.params["PanelID"] + ' #pnlPatient_Search #txtSearchProvider,#txtSearchFacility,#txtSearchPractice').each(function () {
            $(this).trigger("blur");
        });
    },
    UnLoad: function () {
        if (Patient_Search.params && Patient_Search.params.ParentCtrl && Patient_Search.params.ParentCtrl == "appointmentDetail") {
            if ($('#appointmentDetail #SchedulingFormDiv').hasClass("disableAll")) {
                $('#appointmentDetail #SchedulingFormDiv').removeClass("disableAll")
            }
        }
        Patient_Search.DisposeDraggable();
        if (Patient_Search.params != null && Patient_Search.params.ParentCtrl) {
            if (Patient_Search.params != null && Patient_Search.params.ParentPanelID)
                UnloadActionPan(Patient_Search.params.ParentCtrl, 'Patient_Search', null, Patient_Search.params.ParentPanelID);
            else
                UnloadActionPan(Patient_Search.params.ParentCtrl);

            Patient_Search.params = null;
        }
        else {
            UnloadActionPan();
            //Patient_Search.params = null;
            var CurrentMasterTab = GetCurrentMasterTab();
            if (CurrentMasterTab.TabID == "mstrTabPatient" && PatientArray.length <= 0) {
                //SelectTab("mstrTabDashBoard");
                // $("#actionPanPatient").find('div').first().hide('blind', 500, function () { $(this).remove() });
                $('#btnDemographicLabel').hide();
                ClosePatientNew();
                $('.modal-backdrop.fade.in').remove();
                //$('#ctrlPanPatient').empty();

            }
            if ($('.modal-backdrop').length > 0) {
                $('.modal-backdrop').remove();
            }
        }
        // if active tab update request is pending then reset global variable
        if (params.IsDemographicInfoGlobalyUpdated) {
            params.IsDemographicInfoGlobalyUpdated = false;
        }
    },
    DisposeDraggable: function () {
        if ($('.Patient_Search:last').parent().data('ui-draggable'))
            $('.Patient_Search:last').parent().draggable("destroy");
        $('.Patient_Search:last').parent().attr("style", "display: block;");
        $(".Patient_Search:last #modaldialog").attr("class", "modal-dialog size100per");
    },
    PatientAddEdit: function (patientId, mode, event) {

        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        if (patientId > 0) {
            utility.SelectGridRow($("#gvPatient_row" + patientId));
        }
        AppPrivileges.GetFormPrivileges("Demographic", mode.toUpperCase(), "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["mode"] = mode;
                if (!(Patient_Search.params.ParentCtrl)) {
                    params["IsNotFromParent"] = true;
                } else {
                    params["IsNotFromParent"] = false;
                }
                params["patientID"] = patientId;
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = "Patient_Search";
                params["NotFoundPatientDetails"] = Patient_Search.NotFoundPatientDetails;
                if (Patient_Search.params.ParentCtrl == "appointmentDetail") {
                    params["PatBanner"] = true;
                    params["GrandParentCtrl"] = Patient_Search.params.ParentCtrl;
                }
                LoadActionPan('demographicDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);

        });
    },
    IsDataPrivacyCheck: function () {
        var data = "";
        // serach parameter , class name, command name of class

        return MDVisionService.defaultService(data, "ADMIN_USER_DETAIL", "CHECK_IS_DATA_PRIVACY");
    },
    SelectPatient: function (PatientId, AccountNumber, confidenilityCode, event) {
        localStorage.removeItem("PatientSelectedScreen");
        localStorage.removeItem("BatchSelectedScreen");
        if (Patient_Search.params && Patient_Search.params.ParentCtrl && Patient_Search.params.ParentCtrl == "appointmentDetail") {
            if ($('#appointmentDetail #SchedulingFormDiv').hasClass("disableAll")) {
                $('#appointmentDetail #SchedulingFormDiv').removeClass("disableAll")
            }
        }
        if (event != null) {
            event.stopPropagation();
            if (event.target.type == "checkbox") {
                $(':checkbox', this).trigger('click');
                return;
            }
            else {

                Patient_Search.DisposeDraggable();
            }
        }
        var objDef = $.Deferred();
        //MU3-16 Faizan Ameen...
        if (confidenilityCode != undefined && confidenilityCode == "R") {
            Patient_Search.IsDataPrivacyCheck().done(function (response) {
                if (response.status != false) {
                    if (response.IsDataPrivacy == "True") {
                        objDef.resolve();
                    }
                    else {
                        utility.DisplayMessages("You are not authorized to access the data", 3);
                        return;
                    }
                }

            });
        }
        else {
            objDef.resolve();
        }
        $.when(objDef).then(function () {
            var defPatientSelect = $.Deferred();
            if (Patient_Search.params != null && Patient_Search.params.ParentCtrl) {
                UnloadActionPan(Patient_Search.params.ParentCtrl);
            }
            else
                UnloadActionPan();
            //SelectTab('mstrTabPatient', 'false');
            if (Patient_Search.params.FormName) {
                SelectPatient(PatientId, Patient_Search.params.FormName).done(function () {
                    defPatientSelect.resolve('ok');
                });

            }
            else
                SelectPatient(PatientId, AccountNumber, confidenilityCode, "").done(function () {
                    defPatientSelect.resolve('ok');
                });
            $.when(defPatientSelect).then(function () {
                localStorage.setItem("SelectedPatientId", PatientId);
                localStorage.setItem("SelectedAccountNumber", AccountNumber);
                localStorage.setItem("SelectPatientEntityId", globalAppdata.SeletedEntityId);
                $("#pnlPatientDocument #folderPanel #DocTreeSearch").val("");

                utility.InsertRecentPatient(PatientId);
                //Start//15-03-2016//Ahmad Raza//showing CDS ALert icon on patient selection
                //var strMessage = "";
                //AppPrivileges.GetFormPrivileges("ClinicalDecisionSupport_Clinical Decision Support", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                //    if (strMessage == "") {
                if (typeof Immunization_AlertConfiguration != "undefined") {
                    IsBackgroundLoaderShow = false;
                    $.when(Immunization_AlertConfiguration.SetImmunizationAlertCount(PatientId, false)).then(function () {
                        IsBackgroundLoaderShow = true;
                        if (globalAppdata.IsImmunizationAlert != "False") {
                            //$(" #mainForm  li#ImmunizationAlert").show();
                        }
                    });
                }
            });



            //    }
            //    else {

            //    }
            //});
            //End//15-03-2016//Ahmad Raza//showing CDS ALert icon on patient selection
            //SelectTab('patTabDemographic');
        });

    },
    PatientUpdateActiveInactive: function (patientID, IsActive) {
        var data = "PatientID=" + patientID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_SEARCH", "UPDATE_PATIENT_ACTIVE_INACTIVE");
    },

    SelectedPageClick: function (PageNo, objPage) {
        Patient_Search.PatientSearch(null, PageNo, 15, Patient_Search.IsRecentPatientSelected);
    },
    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#" + Patient_Search.params["PanelID"] + " li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";
        if (currentPageNo != "" && currentPageNo > 0) {
            Patient_Search.PatientSearch(null, currentPageNo, 15, Patient_Search.IsRecentPatientSelected);
        }
    },
    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#" + Patient_Search.params["PanelID"] + " li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });

        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo <= TotalPages) {
            Patient_Search.PatientSearch(null, currentPageNo, 15, Patient_Search.IsRecentPatientSelected);
        }
    },
    // Begin 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3018
    BindClaimNumber: function () {
        var Ctrl = $("#" + Patient_Search.params.PanelID + " #txtClaimno");
        var func = function () { return Patient_Search.GetClaimNumberArray(Ctrl.val()); };
        utility.BindKendoAutoComplete(Ctrl, 3, "ClaimNumber", "contains", null, func);
    },

    GetClaimNumberArray: function (name) {
        var AllClaimsVisits = [];
        var dfd = new $.Deferred();
        Patient_Search.LoadClaimNumers(name).done(function (responseData) {
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

    LoadClaimNumers: function (claimNumber) {
        var data = "ClaimNumber=" + claimNumber;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "BILLING_BATCHCHARGE_DETAIL", "SEARCH_VISIT_CLAIM");
    },
    OpenEncounter: function () {
        var params = [];
        params["FromAdmin"] = 0;
        params["ParentCtrl"] = 'Patient_Search';
        params["PreviousTab"] = null;
        params["patientID"] = 0;

        LoadActionPan('Encounter_Visits', params);

        //if (Patient_Search.bVisitFirst) {
        //    $($('body #OpenVisits')[0]).attr('id', 'OpenVisits1')
        //    $($('body #CloseVisits')[0]).attr('id', 'CloseVisits1');
        //    Patient_Search.bVisitFirst = false;
        //}

    },
    FillClaimNumberFromSearch: function (ClaimNumber, AccountNumber, PatientName, PatientId, DOSFrom, VisitId) {
        $("#" + Patient_Search.params.PanelID + " #txtClaimno").val(ClaimNumber);
        //UnloadActionPan("Bill_ChargeSearch");
        Encounter_Visits.UnLoad();
    },
    // End 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3018



    //Start Farooq Ahmad 26/4/2016 Implementation of Data Portability in patient search

    ExportCCDASummary: function () {
        PatientInfo = [];
        $('#pnlPatient_Result input[type=checkbox].patient').each(function () {
            if ($(this).prop('checked')) {
                obj = {
                    PatientId: $(this).attr('id'),
                    ProviderId: $(this).val()
                }
                PatientInfo.push(obj);

            }
        });
        if (PatientInfo.length == 0) {
            utility.DisplayMessages("Please select the patient", 2);
        }
        else {
            Patient_Search.GetExportCCDASummary(PatientInfo).done(function (response) {

                response = JSON.parse(response);
                if (response.status != false) {
                    var zip = new JSZip();
                    var data = JSON.parse(response.data);

                    for (var d in data) {
                        var FolderName = data[d].PatientName + "_" + data[d].PatientId;
                        var xml = zip.folder(FolderName);
                        var html = xml.folder("HTML");
                        html.file(FolderName + ".html", data[d].HTMLData, { base64: true });
                        var xmlFolder = xml.folder("XML");
                        xmlFolder.file(FolderName + ".xml", data[d].Data, { base64: true });

                    }
                    zip.generateAsync({ type: "blob" })
                    .then(function (content) {
                        saveAs(content, "CCDA.zip");
                    });
                }
            });
        }
        //}

    },


    GetExportCCDASummary: function (obj) {

        var objData = new Object();
        objData["PatientInfo"] = obj;
        objData["commandType"] = "ExportCCDA";
        objData["FromPatientSearch"] = "true";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALSUMMARY", "ClinicalSummary");

    },

    ImportCCDASummary: function () {
        var params = [];
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtSearchProvider";
        params["RefCtrlHidden"] = "hfSearchProvider";
        params["RefCtrlLabel"] = "lblSearchProvider";
        params["RefCtrlLink"] = "lnkSearchProviderEdit";
        params["ParentCtrl"] = "Patient_Search";
        params["ParentCtrlPanelID"] = "pnlBatchImportCCDA";
        params["OpenFromPopup"] = "1";
        LoadActionPan("Batch_ImportCCDA", params);
    },

    //End Farooq Ahmad 26/4/2016 Implementation of Data Portability in patient search


    FillPatientInfoForNewClaim: function (PatientId, patFullName, RefProviderId, RefProviderName, ProviderId, ProviderName,
        FacilityId, FaciltyName, SelfPay, DOB,Gender, ParentCtrl, event) {
        if (Patient_Search.params && Patient_Search.params.ParentCtrl && Patient_Search.params.ParentCtrl == "appointmentDetail") {
            if ($('#appointmentDetail #SchedulingFormDiv').hasClass("disableAll")) {
                $('#appointmentDetail #SchedulingFormDiv').removeClass("disableAll")
            }
        }
        Patient_Search.DisposeDraggable();
        if (event != null) {
            event.stopPropagation();
        }
        UnloadActionPan(Patient_Search.params.ParentCtrl, "Patient_Search");
        setTimeout(function () {
            var params = [];
            params["FromAdmin"] = 0;
            params["ParentCtrl"] = ParentCtrl;
            params['mode'] = "Add";
            params["PatientId"] = PatientId;
            params["patFullName"] = patFullName;
            params["RefProviderId"] = RefProviderId;
            params["RefProviderName"] = RefProviderName;
            params["ProviderId"] = ProviderId;
            params["ProviderName"] = ProviderName;
            params["FacilityId"] = FacilityId;
            params["FaciltyName"] = FaciltyName;
            params["SelfPay"] = SelfPay;
            params["DOB"] = DOB;
            params["Gender"] = Gender;

            if (Patient_Search.params["CaseId"]) {
                params["CaseId"] = Patient_Search.params["CaseId"];
                params["CaseNo"] = Patient_Search.params["CaseNo"];
            }
            LoadActionPan('Encounter_CreateClaim', params);
        }, 510);


    },

    BindProviderAutoComplete: function () {
        var Ctrl = $("#" + Patient_Search.params["PanelID"] + " #frmPatientSearch #txtSearchProvider");
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        var hfCtrl = $("#" + Patient_Search.params["PanelID"] + " #frmPatientSearch #hfSearchProvider");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    BindFaciltyAutoComplete: function () {
        var Ctrl = $("#" + Patient_Search.params["PanelID"] + " #frmPatientSearch #txtSearchFacility");
        var func = function () { return utility.GetFacilityArray(Ctrl.val()) };
        var hfCtrl = $("#" + Patient_Search.params["PanelID"] + " #frmPatientSearch #hfSearchFacility");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    BindInsuranceAutoComplete: function () {
        var Ctrl = $("#" + Patient_Search.params["PanelID"] + " #frmPatientSearch #txtPrimaryInsurance");
        var func = function () { return utility.GetInsurancePlanArray(Ctrl.val()) };
        var hfCtrl = $("#" + Patient_Search.params["PanelID"] + " #frmPatientSearch #hfInsurancePlan");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    BindPracticeAutoComplete: function () {
        var Ctrl = $("#" + Patient_Search.params["PanelID"] + " #frmPatientSearch #txtSearchPractice");
        var func = function () { return utility.GetPracticeArray(Ctrl.val()) };
        var hfCtrl = $("#" + Patient_Search.params["PanelID"] + " #frmPatientSearch #hfSearchPractice");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },
    // PRD-14 
    OpenQuickAddPatient: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["GrandParentCtrl"] = Patient_Search.params.ParentCtrl;
        params["ParentCtrl"] = "Patient_Search";
        LoadActionPan('Patient_DemographicQuick', params)
    },

    //  PRD-422
    SearchPatientByPressingEnter: function () {
        $("#" + Patient_Search.params["PanelID"] + " #frmPatientSearch").find('select,[type=text],[type=checkbox]').each(function () {
            var element = document.getElementById(this.id);
            element.addEventListener('keypress', function (event) {
                if (event.keyCode == 13) {
                    event.preventDefault();
                    Patient_Search.ValidateSearchCriteria();
                }
            });
        });

        $('#Patient_Search #txtPrimaryInsurance').keypress(function (event) {
            if (event.keyCode == 13) {
                event.preventDefault();
                Patient_Search.ValidateSearchCriteria();
            }
        });
    },
}