ClinicalLabOrderDetail = {
    params: [],
    bIsFirstLoad: true,
    EditableGrid: null,
    LabOrderProblems: [],
    FavListName: "LabOrderDetail",
    checkedProblems: [],
    CPTCodeQA: [],
    ArrayValidation: [],
    selectedTestCode: null,
    selectedTestDescription: null,
    OrderedTestsCount: 0,
    AOEsExists: false,
    loadProviderFromAppData: false,
    loadFacilityFromAppData: false,
    ProviderOnDemandLoad: false,
    FacilityOnDemandLoaded: false,
    isProviderLoaded: false,
    isFacilityLoaded: false,
    IsFavoriteGroupSelected: false,
    SaveModeSet: null,
    FavoriteGroupTestLabs: [],
    SavelabOrderId: 0,
    IsVitalAxisCPT: false,
    AddResult: false,
    LabsData: [],
    AddedLabOrderResultId: 0,
    Load: function (params) {
        BackgroundLoaderShow(true);
        //ClinicalLabOrderDetail.params["TabID"] = 'ClinicalLabOrderDetail';
        ClinicalLabOrderDetail.AddResult = false;
        ClinicalLabOrderDetail.AddedLabOrderResultId = 0;
        ClinicalLabOrderDetail.AOEsExists = false;
        ClinicalLabOrderDetail.SaveModeSet = false;
        ClinicalLabOrderDetail.params = params;
        ClinicalLabOrderDetail.ddlSpecimen = [];
        ClinicalLabOrderDetail.ddlAlternativeSpecimen = [];
        ClinicalLabOrderDetail.ArrayValidation = [];
        ClinicalLabOrderDetail.IsFavoriteGroupSelected = false;
        ClinicalLabOrderDetail.FavoriteGroupTestLabs = [];
        ClinicalLabOrderDetail.SavelabOrderId = 0;
        ClinicalLabOrderDetail.IsVitalAxisCPT = false;

        if (ClinicalLabOrderDetail.params.ParentCtrlPanelID) {
            ClinicalLabOrderDetail.params.PanelID = ClinicalLabOrderDetail.params.ParentCtrlPanelID;
        } else {
            if (ClinicalLabOrderDetail.params.PanelID != 'pnlClinicalLabOrderDetail') {
                ClinicalLabOrderDetail.params.PanelID = ClinicalLabOrderDetail.params.PanelID + ' #pnlClinicalLabOrderDetail';
            } else {
                ClinicalLabOrderDetail.params.PanelID = 'pnlClinicalLabOrderDetail';
            }
        }
        if (ClinicalLabOrderDetail.params.ParentCtrl == "Clinical_Treatment") {
            $('#' + ClinicalLabOrderDetail.params.PanelID + " #btnSignPrintOrder,#btnPrintLabOrder").addClass("hidden");
        }
        $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #hfPatientId").val($("div#PatientProfile #hfPatientId").val());
        $("#" + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #btnSaveOrder,#btnSignPrintOrder,#btnSignOrder,#btnPrintLabOrder, #btnResetLabOrder").addClass('disableAll');

        if (Clinical_LabOrder.params["ParentCtrl"] == "Clinical_FaceSheet") {
            $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #hfPatientId").val(Clinical_FaceSheet.params.patientID);
            $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfPatientId").val(Clinical_FaceSheet.params.patientID);
        }
        var self = $('#' + ClinicalLabOrderDetail.params.PanelID);

        //For Data Grid
        var PanelLabGrid = "#" + ClinicalLabOrderDetail.params.PanelID + " #pnlLab_ResultOrderDetail";
        var LabGridId = "#" + ClinicalLabOrderDetail.params.PanelID + " #dgvLabOrderDetail";
        $(LabGridId + " tbody tr").remove();

        ClinicalLabOrderDetail.EditableGrid = EMRUtility.MakeEditableGrid(PanelLabGrid, LabGridId, ClinicalLabOrderDetail, "0", false, false, false, false);

        utility.CreateDatePicker(ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail input[id="txtOrderDate"]', function () {
            //on-change callback method
        }, true);

        $('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail input[id="txtOrderTime"]').timepicker({
            defaultTime: new Date()
        }).on('changeTime.timepicker', function (e) {
            var h = e.time.hours;
            var m = e.time.minutes;
            var mer = e.time.meridian;
            //convert hours into minutes
            m += h * 60;
            //10:15 = 10h*60m + 15m = 615 min
            if (m < 0)
                $(this).timepicker('setTime', '09:15 AM');
        });


        $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtLabOrderNo").prop('disabled', true);

        if (ClinicalLabOrderDetail.params.mode == "Edit") {
            $('#pnlClinicalLabOrderDetail #ddlLabId').prop('disabled', true);
        }
        if (Clinical_LabOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
            $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider").val(Clinical_ProgressNote.params["CurrentNotesProviderText"]);
            $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider").attr("Provider", Clinical_ProgressNote.params["CurrentNotesProviderId"]);
            $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfProvider").val(Clinical_ProgressNote.params["CurrentNotesProviderId"]);
            utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider"), Clinical_ProgressNote.params.CurrentNotesProviderText, $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfProvider"), Clinical_ProgressNote.params.CurrentNotesProviderId);

            $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility").val(Clinical_ProgressNote.params["NotesFacilityName"]);
            $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility").attr("Facility", Clinical_ProgressNote.params["NotesFacilityIDForFollowUp"]);
            $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfFacility").val(Clinical_ProgressNote.params["NotesFacilityIDForFollowUp"]);
            ClinicalLabOrderDetail.PoupulateLoction();
            utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility"), Clinical_ProgressNote.params.NotesFacilityName, $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfFacility"), Clinical_ProgressNote.params.NotesFacilityIDForFollowUp);

        } else {








            //ClinicalLabOrderDetail.loadAllAutocomplete();
        }

        // Set Title Explicitly if it's passed as Parameter
        if (ClinicalLabOrderDetail.params.Title != null)
            $("#" + ClinicalLabOrderDetail.params["PanelID"] + " #headingTitle").text(ClinicalLabOrderDetail.params.Title);


        if (ClinicalLabOrderDetail.bIsFirstLoad == true) {
            ClinicalLabOrderDetail.bIsFirstLoad = true;

            Clinical_LabOrder.LoadLabs('ddlLabId', ClinicalLabOrderDetail.params.PanelID).done(function (response) {
                response = JSON.parse(response);
                ClinicalLabOrderDetail.LabsData = JSON.parse(response.ClinicalLab_JSON);
                self.loadDropDowns(true).done(function () {

                    //Start//31-03-2016//Abid Ali//Logic to select Guarantor and Relation
                    ClinicalLabOrderDetail.loadPatientGuarantor($('#' + ClinicalLabOrderDetail.params.PanelID + " #hfPatientId").val()).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false && response.GuarantorFill_JSON != "") {
                            var obj = JSON.parse(response.GuarantorFill_JSON);

                            ClinicalLabOrderDetail.params.GuarantorName = obj.GuarantorName;
                            ClinicalLabOrderDetail.params.GuarantorId = obj.GuarantorId;

                            ClinicalLabOrderDetail.params.RelationName = response.relationName;
                            ClinicalLabOrderDetail.params.RelationId = response.relationshipId;


                            $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtGuarantor").val(obj.GuarantorName);
                            $('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail #txtLabRelationShipId').val(response.relationName);
                            $('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail #ddlRelationShipId option[value="' + response.relationshipId + '"]').attr("selected", "selected");
                            //   $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlRelationShipId').val(response.relationshipId);
                            $('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail #hfGuarantorId').val(obj.GuarantorId);
                            if (response.relationshipId > 0)
                                $('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail #hfRalationId').val(response.relationshipId);
                            //Start//18-03-2016//Abid Ali//Logic to select Guarantor and Relation

                            //Start//31-03-2016//Abid Ali //Load All Patient Insurances
                            if (Clinical_LabOrder.params["ParentCtrl"] == "Clinical_FaceSheet") {
                                $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #hfPatientId").val(Clinical_FaceSheet.params.patientID);
                                $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfPatientId").val(Clinical_FaceSheet.params.patientID);
                            }
                            ClinicalLabOrderDetail.searchPatientInsurance($('#' + ClinicalLabOrderDetail.params.PanelID + " #hfPatientId").val()).done(function (response) {
                                if (response.status != false) {
                                    var obj = JSON.parse(response.PatientInsuranceLoad_JSON);

                                    var $ddlPrimary = $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlPrimaryInsuraceId');
                                    var $ddlSecondary = $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlSecondaryInsuraceId');
                                    var $ddlTertiary = $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlTertiaryInsuraceId');

                                    $ddlPrimary.append($('<option/>', {
                                        value: "",
                                        html: "- Select -"
                                    }));
                                    $ddlSecondary.append($('<option/>', {
                                        value: "",
                                        html: "- Select -"
                                    }));
                                    $ddlTertiary.append($('<option/>', {
                                        value: "",
                                        html: "- Select -"
                                    }));

                                    $.each(obj, function (i, item) {
                                        if (item.IsActive == "True") {
                                            $ddlPrimary.append(
                                                $('<option/>', {
                                                    value: item.InsuranceId,
                                                    html: item.InsurancePlanName,
                                                    Priority: item.PlanPriority
                                                })
                                            );
                                        }
                                    });

                                    $.each(obj, function (i, item) {
                                        if (item.IsActive == "True") {
                                            $ddlSecondary.append(
                                                $('<option/>', {
                                                    value: item.InsuranceId,
                                                    html: item.InsurancePlanName,
                                                    Priority: item.PlanPriority
                                                })
                                            );
                                        }
                                    });

                                    $.each(obj, function (i, item) {
                                        if (item.IsActive == "True") {
                                            $ddlTertiary.append(
                                                $('<option/>', {
                                                    value: item.InsuranceId,
                                                    html: item.InsurancePlanName,
                                                    Priority: item.PlanPriority
                                                })
                                            );
                                        }
                                    });
                                    //Start//20-06-2016//Ahmad Raza//logic to select insurance dropdowns on load
                                    if (obj.length > 0) {
                                        $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlBillingTypeId').val(3);
                                    }

                                    if ($('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlBillingTypeId').val() == 3) {

                                        ClinicalLabOrderDetail.enableDisableInsurances(false);
                                        // ClinicalLabOrderDetail.billingtype = 3;
                                        var selectedVal1 = '';
                                        var selectedVal2 = '';
                                        var selectedVal3 = '';
                                        $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlPrimaryInsuraceId option').each(function () {
                                            if ($(this).attr('Priority') == "1") {
                                                selectedVal1 = $(this).text(); //$(this).val();
                                                $("#hfPrimaryInsId").val($(this).val());
                                            }

                                        });
                                        $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlPrimaryInsuraceId').val(selectedVal1);

                                        $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlSecondaryInsuraceId option').each(function () {
                                            if ($(this).attr('Priority') == "2") {
                                                selectedVal2 = $(this).text();  // $(this).val();
                                                $("#hfSecondaryInsId").val($(this).val());
                                            }
                                        });
                                        $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlSecondaryInsuraceId').val(selectedVal2);

                                        $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlTertiaryInsuraceId option').each(function () {
                                            if ($(this).attr('Priority') == "3") {
                                                selectedVal3 = $(this).text();  // $(this).val();
                                                $("#hfTertiaryInsId").val($(this).val());
                                            }
                                        });
                                        $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlTertiaryInsuraceId').val(selectedVal3);
                                    }
                                    else {

                                        ClinicalLabOrderDetail.enableDisableInsurances(false);

                                        $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlPrimaryInsuraceId').val('');
                                        $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlSecondaryInsuraceId').val('');
                                        $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlTertiaryInsuraceId').val('');
                                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtGuarantor").val("");
                                        $('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail #txtLabRelationShipId').val("");

                                    }
                                    //Start//20-06-2016//Ahmad Raza//logic to select insurance dropdowns on load
                                    $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").data('serialize', $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").serialize());


                                }
                                else {

                                    utility.DisplayMessages(response.essage, 3);
                                }
                            });
                            //End//31-03-2016//Abid Ali//Load All Patient Insurances
                            $.when(CacheManager.BindDropDownsByID($('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail #ddlAssigneeId'), 'GetUsersFullName', true, 1)).then(function () {
                                $("#" + ClinicalLabOrderDetail.params.PanelID + " #ddlAssigneeId option:first").text('- Select -');
                                //------------
                                var isProblemAdded = false;
                                if (ClinicalLabOrderDetail.params.isProblemAdded) {
                                    isProblemAdded = true;
                                } else {
                                    isProblemAdded = false;
                                }
                                //------------
                                $.when(ClinicalLabOrderDetail.loadProblemList(isProblemAdded)).then(function () {
                                    if (isProblemAdded) {
                                        ClinicalLabOrderDetail.loadLabOrder(ClinicalLabOrderDetail.params.LabOrderId);
                                    }
                                    ClinicalLabOrderDetail.validateLabOrderDetail();
                                });
                                $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").data('serialize', $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").serialize());
                            });
                        }
                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").data('serialize', $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").serialize());
                    });

                    if (ClinicalLabOrderDetail.params.mode == "Add") {
                        if (ClinicalLabOrderDetail.params["LastLabName"]) {
                            var theText = ClinicalLabOrderDetail.params["LastLabName"];
                            if (theText != "") {
                                if (ClinicalLabOrderDetail.params.ParentCtrlPanelID) {
                                    $("#" + ClinicalLabOrderDetail.params.ParentCtrlPanelID + ' #frmClinicalLabOrderDetail #ddlLabId option:contains(' + theText + ')').attr('selected', 'selected');
                                    $("#" + ClinicalLabOrderDetail.params.ParentCtrlPanelID + ' #frmClinicalLabOrderDetail #ddlLabId').trigger('change');
                                } else {
                                    $("#" + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail #ddlLabId option:contains(' + theText + ')').attr('selected', 'selected');
                                    $("#" + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail #ddlLabId').trigger('change');
                                }

                            }
                        } else {
                            $("#" + ClinicalLabOrderDetail.params.PanelID + ' #ddlLabId').val("");
                            $("#" + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail #ddlLabId').trigger('change');
                        }
                    }
                    else {
                        $("#" + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail #ddlLabId').trigger('change');
                    }
                    if (Clinical_LabOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider").val(Clinical_ProgressNote.params["CurrentNotesProviderText"]);
                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider").attr("Provider", Clinical_ProgressNote.params["CurrentNotesProviderId"]);
                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfProvider").val(Clinical_ProgressNote.params["CurrentNotesProviderId"]);
                        utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider"), Clinical_ProgressNote.params.CurrentNotesProviderText, $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfProvider"), Clinical_ProgressNote.params.CurrentNotesProviderId);
                    } else {
                        //ClinicalLabOrderDetail.loadAllAutocomplete();

                        if (globalAppdata.DefaultProviderName != "" && globalAppdata.DefaultProviderId != "") {
                            $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider").val(globalAppdata.DefaultProviderName);
                            $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider").attr("Provider", globalAppdata.DefaultProviderId);
                            $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfProvider").val(globalAppdata.DefaultProviderId);
                            utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider"), globalAppdata.DefaultProviderName, $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfProvider"), globalAppdata.DefaultProviderId);
                            ClinicalLabOrderDetail.EnableDisableTestSearch();
                        }
                        if (globalAppdata.DefaultFacilityName != "" && globalAppdata.DefaultFacilityId != "") {
                            $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility").val(globalAppdata.DefaultFacilityName);
                            $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility").attr("Facility", globalAppdata.DefaultFacilityId);
                            $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfFacility").val(globalAppdata.DefaultFacilityId);
                            utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility"), globalAppdata.DefaultFacilityName, $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfFacility"), globalAppdata.DefaultFacilityId);
                            ClinicalLabOrderDetail.PoupulateLoction();
                        }
                    }
                    $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").data('serialize', $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").serialize());
                });

            });


        }

        //Start 06-04-2016 Humaira Yousaf to select provider
        if (ClinicalLabOrderDetail.params.mode == "Add") {
            // fill provider if logged in user and provider has same names
            //ClinicalLabOrderDetail.selectProvider();
            // fill data if order is placed before
            $("#" + ClinicalLabOrderDetail.params.PanelID + ' #headingTitle').text('Add Lab Order');

        } else {
            $("#" + ClinicalLabOrderDetail.params.PanelID + ' #headingTitle').text('Edit Lab Order');
        }
        ClinicalLabOrderDetail.domReadyFunction();
        setTimeout(function () {
            $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").data('serialize', $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").serialize());

        }, 2500);
        ClinicalLabOrderDetail.bindProvider();
        ClinicalLabOrderDetail.bindFacility();

        $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #txtProvider").focus(function () {

            if (!ClinicalLabOrderDetail.isProviderLoaded)
                ClinicalLabOrderDetail.ProviderOnDemandLoad = true;
            else
                ClinicalLabOrderDetail.ProviderOnDemandLoad = false;

            ClinicalLabOrderDetail.loadAllAutocomplete();
        });
        $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #txtFacility").focus(function () {
            if (!ClinicalLabOrderDetail.isFacilityLoaded)
                ClinicalLabOrderDetail.FacilityOnDemandLoad = true;
            else
                ClinicalLabOrderDetail.FacilityOnDemandLoad = false;

            ClinicalLabOrderDetail.loadAllAutocomplete();
        });

        if (ClinicalLabOrderDetail.params.mode == "Edit") {
            $('#' + ClinicalLabOrderDetail.params.PanelID + ' #LabOrderGroupFavList').addClass('disableAll');
            // Focus out field in edit mode
            //utility.callbackAfterAllDOMLoaded(function () {
            //    $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #txtFacility").focusout();
            //    $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #dgvLabOrderDetail").focus();
            //   // $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #txtProvider").focusout();
            //});
        }

        if (EMRUtility.getFavListStatus(ClinicalLabOrderDetail.FavListName))
            $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #favSectionDiv").removeClass("toggled");
        else
            $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #favSectionDiv").addClass("toggled");


    },

    //Author: Abid Ali
    //Date :  07-03-2016
    //Reason: Clear/Set gurantor/relation values
    setGurantorRelationValues: function (setValues) {

        var guarantorName = ClinicalLabOrderDetail.params.GuarantorName;
        var guarantorId = ClinicalLabOrderDetail.params.GuarantorId;
        var relationName = ClinicalLabOrderDetail.params.RelationName;
        var relationId = ClinicalLabOrderDetail.params.RelationId;
        var billingType = $('#' + ClinicalLabOrderDetail.params.PanelID + '  #frmClinicalLabOrderDetail #ddlBillingTypeId option:selected').text();
        if (setValues) {


            $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtGuarantor").val(guarantorName);
            $('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail #hfGuarantorId').val(guarantorId);
            $('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail #txtLabRelationShipId').val(relationName);

            if (relationId > 0)
                $('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail #hfRalationId').val(relationId);

            //$('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail #ddlRelationShipId option[value="' + response.relationshipId + '"]').attr("selected", "selected");
            if (billingType.indexOf('Insurance') < 0) {
                $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlPrimaryInsuraceId').val('');
                $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlSecondaryInsuraceId').val('');
                $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlTertiaryInsuraceId').val('');
                $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtGuarantor").val("");
                $('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail #txtLabRelationShipId').val("");

            }
        }
        else {
            $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtGuarantor").val("");
            $('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail #hfGuarantorId').val("");
            $('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail #txtLabRelationShipId').val("");
            $('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail #hfRalationId').val("");
        }
    },

    //Function Name: selectProvider
    //Author Name: Humaira Yousaf
    //Created Date: 06-04-2016
    //Description: Auto selects provider if same as logged in user
    selectProvider: function (providerId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Orders and Results_Lab Order", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                ClinicalProcedureOrderDetail.searchProvider(providerId).done(function (response) {
                    if (response.status != false) {
                        if (response.ProviderCount > 0) {
                            var ProviderLoadJSONData = JSON.parse(response.ProviderLoad_JSON);
                            $.each(ProviderLoadJSONData, function (i, item) {
                                var name = new Array();
                                name = globalAppdata.AppUserNameFullName.split(',');
                                if (item.LastName == name[0] && item.FirstName == $.trim(name[1])) {
                                    var decodeHtmlEntity = function (str) {
                                        return str.replace(/&#(\d+);/g, function (match, dec) {
                                            return String.fromCharCode(dec);
                                        });
                                    };
                                    utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider"), decodeHtmlEntity(item.LastName + ', ' + item.FirstName + ' ' + item.MiddleInitial), $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfProvider"), decodeHtmlEntity(item.ProviderId));
                                }
                            });
                            utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider"), Clinical_ProgressNote.params.CurrentNotesProviderText, $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfProvider"), Clinical_ProgressNote.params.CurrentNotesProviderId);
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


    bindProvider: function () {
        var Ctrl = $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider");
        var hfCtrl = $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfProvider");
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        var onSelect = function (dataItem) { $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider").attr("Provider", dataItem.id) }
        var onChange = function (valid) { ClinicalLabOrderDetail.EnableDisableTestSearch(null, true); }
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect, onChange);
    },
    bindFacility: function () {
        var Ctrl = $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility");
        var hfCtrl = $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfFacility");
        var func = function () { return utility.GetFacilityArray(Ctrl.val()) };
        var onSelect = function (dataItem) {
            $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility").attr("Facility", dataItem.id);
            $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtLocation").val(dataItem.Location);
        }
        var onChange = function (valid) { ClinicalLabOrderDetail.EnableDisableTestSearch(null, true); }
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect, onChange);
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Function to disable Lab Order Controls
    disableLabOrderControls: function (IsSigned) {
        var detailsDivs = $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #divLabOrderInformation,#divLabBillingInformation,#divTestInformation,#divLabAssociatedProblems");
        var detailsButtons = $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #btnSaveOrder,#btnSignPrintOrder,#btnSignOrder, #btnPrintLabOrder,#btnResetLabOrder");
        if (ClinicalLabOrderDetail.params.ParentCtrl == "Clinical_Treatment") {
            var detailsButtons = $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #btnSaveOrder,#btnSignOrder,#btnResetLabOrder");
        }


        var printRequisitionButton = $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #btnPrintOrder");

        var addLabResult = $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #btnAddLabResult");
        var editLabResult = $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #btnEditLabResult");
        var printABN = $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #btnPrintABN");


        if (IsSigned == true) {
            detailsDivs.addClass("disableAll");
            detailsButtons.addClass("hidden");
            // enable only if insurance is selected (ABN)
            var ins = $('#ddlBillingTypeId option').filter(':selected').text();
            if (ins == "Insurance") {
                printABN.removeClass("hidden");

            }

            printRequisitionButton.removeClass("hidden");
            //Check if labResult already exists then show edit result instead of addd result button
            addLabResult.removeClass("hidden");
            editLabResult.addClass("hidden");
        }
        else {
            detailsDivs.removeClass("disableAll");
            detailsButtons.removeClass("hidden");
            printRequisitionButton.addClass("hidden");
            //Hide if Order is not signed yet
            printABN.addClass("hidden");
            addLabResult.addClass("hidden");
            editLabResult.addClass("hidden");
        }
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Function to load Lab Order Fav List
    favoriteListSearch: function (ListType, IsFromProviderFacility) {

        var dfd = new $.Deferred();

        if (ListType != null && IsFromProviderFacility != true && ClinicalLabOrderDetail.params.mode == "Add") {
            $('#' + ClinicalLabOrderDetail.params.PanelID + ' #dgvLabOrderDetail').dataTable.ext.errMode = 'none';
            $('#' + ClinicalLabOrderDetail.params.PanelID + ' #dgvLabOrderDetail').dataTable().fnClearTable();
        }

        if (ListType == null) {
            ListType = "LabOrder";
        }

        if (ListType == "LabOrderGroup") {
            $('#' + ClinicalLabOrderDetail.params.PanelID + ' #txtLabCPTCode').addClass('disableAll');
            $('#' + ClinicalLabOrderDetail.params.PanelID + ' #btnPrintLabOrder').addClass('disableAll');
            $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlLabId option:selected').removeAttr("selected");
            $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlLabId').prop('disabled', true);
        }
        else {
            ClinicalLabOrderDetail.FavoriteGroupTestLabs = [];
            ClinicalLabOrderDetail.IsFavoriteGroupSelected = false;
            if ($('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlLabId option:selected').val() == "") {
                $('#' + ClinicalLabOrderDetail.params.PanelID + ' #txtLabCPTCode').addClass('disableAll');
            }
            else {
                $('#' + ClinicalLabOrderDetail.params.PanelID + ' #txtLabCPTCode').removeClass('disableAll');
            }
            $('#' + ClinicalLabOrderDetail.params.PanelID + ' #btnPrintLabOrder').removeClass('disableAll');
            if (ClinicalLabOrderDetail.params.mode != "Edit" && IsFromProviderFacility != true) {
                $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlLabId').removeClass('disableAll');
                $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlLabId').prop('disabled', false);
            }
        }

        ClinicalLabOrderDetail.searchFavoriteList_DBCall(ListType, null, 1, 5000).done(function (response) {

            response = utility.decodeHtml(response);
            response = JSON.parse(response);
            var $ddl = $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlFavoriteListLabOrder');
            $ddl.empty();
            $ddl.append($('<option/>', {
                value: "",
                html: "- Select -"
            }));

            if (response.status != false) {

                var favouriteProcedures = JSON.parse(response.FavoriteListJSON);

                $.each(favouriteProcedures, function (i, item) {
                    if (item.Name != "") {
                        $ddl.append(
                          $('<option/>', {
                              id: item.FavoriteListId,
                              value: item.FavoriteListId,
                              html: item.Name,
                          })
                        );
                    }

                });
                if (favouriteProcedures.length > 0) {
                    EMRUtility.getFavListValue(ClinicalLabOrderDetail.FavListName).done(function (response1) {
                        response1 = JSON.parse(response1);
                        if (response1.status != false) {
                            if (response1.favListVal != "" && response1.favListVal != "-1") {
                                if ($("#" + ClinicalLabOrderDetail.params.PanelID + " #ddlFavoriteListLabOrder option[value='" + response1.favListVal + "']").length > 0) {
                                    $ddl.val(response1.favListVal);
                                    $ddl.trigger("onchange");
                                }
                                else {
                                    if (favouriteProcedures.length == 1) {
                                        $ddl.val(favouriteProcedures[0].FavoriteListId);
                                        $ddl.trigger("onchange");
                                    }
                                    else if (favouriteProcedures.length > 1) {
                                        $ddl.trigger("onchange");
                                    }
                                }
                            }
                            else {
                                if (favouriteProcedures.length == 1) {
                                    $ddl.val(favouriteProcedures[0].FavoriteListId);
                                    $ddl.trigger("onchange");
                                }
                                else if (favouriteProcedures.length > 1) {
                                    $ddl.trigger("onchange");
                                }
                            }
                            $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").data('serialize', $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").serialize());
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                } else {
                    $ddl.trigger("onchange");
                }
                //    $ddl.trigger("onchange");

            }
            //else {
            //    utility.DisplayMessages(response.Message, 3);
            //}
            $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").data('serialize', $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").serialize());
            dfd.resolve();
        });
        if (ClinicalLabOrderDetail.params.mode == "Add") {
            ClinicalLabOrderDetail.SaveFreeTextStatus().done(function (res) {
                $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").data('serialize', $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").serialize());
                dfd.resolve();
            });
        }

        return dfd.promise();
    },

    searchFavoriteList_DBCall: function (ListType, FavoriteListId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = -1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = -1;
        }
        var objData = {};
        objData["FavoriteListId"] = FavoriteListId == null ? 0 : FavoriteListId;
        objData["ListType"] = ListType == null ? 'LabOrder' : ListType;
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["LabId"] = ListType == 'LabOrderGroup' ? null : $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlLabId option:selected').val();
        objData["ProviderId"] = $('#' + ClinicalLabOrderDetail.params.PanelID + ' #hfProvider').val();

        if (Favorite_LabOrder.Switch == 1) {
            objData["IsActive"] = true
        }
        else {
            objData["IsActive"] = false;
        }

        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;

        objData["commandType"] = "load_favoritelist";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Function to load procedure Order Fav List Test while selection is done
    selectAllFavoriteListContent: function () {

        $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ulFavoriteListLabOrderContent li').each(function (i, item) {
            $(item).trigger("click");
        });
        //var favListIds = [];
        //$('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlFavoriteListLabOrder option').each(function (i, item) {
        //    var value = $(item).attr('value');
        //    if (value != "") {
        //        favListIds.push(value);
        //    }
        //});
        //ClinicalLabOrderDetail.loadfavoriteListContent(null, favListIds);
    },
    AutoSearchFavLabOrder: function () {
        utility.Keyupdelay(function () {
            ClinicalLabOrderDetail.loadfavoriteListContent();
        });
    },
    //Author: Abid Ali
    //Date :  07-03-2016
    //Reason: Function to load procedure Order Fav List Test while selection is done
    loadfavoriteListContent: function (obj, favListIds) {
        if ((typeof favListIds == typeof undefined || favListIds == null) && (typeof obj == typeof undefined || obj == null)) {
            obj = $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlFavoriteListLabOrder');
        }
        var SearchData = $('#' + ClinicalLabOrderDetail.params.PanelID + ' #FavSearchBox').val();
        var $uL = $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ulFavoriteListLabOrderContent');
        if (obj != null) {
            var selectedOption = $(obj).find("option:selected");
            var selectedOptionValue = selectedOption.attr("id");
            //Start 12-07-2016 Muhammad Arshad Bug#EMR-1486 Lab order -> Favorite list -> "Select ALL " functionality is not working on 46 server ,please see attached video and resolve the issue
            var divSelectAllFavorites = $('#' + ClinicalLabOrderDetail.params.PanelID + ' #divSelectAllLabOrderFavorites');
            if (selectedOptionValue > 0) {
                divSelectAllFavorites.removeClass("disableAll");
                ClinicalLabOrderDetail.favoriteList_CPTSearch(selectedOptionValue, null, SearchData);
            }
            else {
                $uL.empty();
                if (divSelectAllFavorites.hasClass("disableAll") == false) {
                    divSelectAllFavorites.addClass("disableAll");
                }
            }
            //End 12-07-2016 Muhammad Arshad Bug#EMR-1486 Lab order -> Favorite list -> "Select ALL " functionality is not working on 46 server ,please see attached video and resolve the issue

        }
        else {
            if (favListIds != null) {
                //clear the list

                $uL.empty();

                $.each(favListIds, function (index, item) {
                    //load all favList
                    ClinicalLabOrderDetail.favoriteList_CPTSearch(item, false);
                });
            }
        }
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Function to load procedure Order Fav List
    favoriteList_CPTSearch: function (FavoriteListId, isEmptyUl, SearchData) {
        var $uL = $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ulFavoriteListLabOrderContent');
        Favorite_ProcedureOrder.searchFavoriteList_CPT_DBCall(null, FavoriteListId, 1, 5000, SearchData).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                var objData = JSON.parse(response.FavoriteListCPTJSON);

                isEmptyUl = isEmptyUl == null ? true : false;
                if (isEmptyUl) {
                    $uL.empty();
                }
                $.each(objData, function (i, item) {
                    var mod = item.Modifier != "" ? "<strong> - Mod : " + item.Modifier + "</strong>" : "";
                    if (item.CPTCodeDescription != "") {
                        if (item.CPTCodeDescription.indexOf("&#39;") > 0) {  // If double qoutes are present
                            item.CPTCodeDescription = item.CPTCodeDescription.replace(/''/g, "&#39;");
                            if ($('#' + ClinicalLabOrderDetail.params.PanelID + ' #LabOrderGroupFavList').is(':checked')) {
                                var onclick = "ClinicalLabOrderDetail.BindLabOrderGridItem(\"" + item.Modifier + "\",\"" + item.CPTCode + "\",\"" + String(item.CPTCodeDescription) + "\",\"" + item.CPTCodeDescription + "\",null,\"" + String(item.LabId) + "\",true)";
                            }
                            else {
                                var onclick = "ClinicalLabOrderDetail.BindLabOrderGridItem(\"" + item.Modifier + "\",\"" + item.CPTCode + "\",\"" + String(item.CPTCodeDescription) + "\",\"" + item.CPTCodeDescription + "\",null);";
                            }
                           
                            if (item.LabName != null && item.LabName.length > 0) {
                                $uL.append("<li onclick='" + onclick + "'>" + item.CPTCode + " " + String(item.CPTCodeDescription) + " (<strong>" + item.LabName + "</strong>)" + mod + "</li>");
                            } else {
                                $uL.append("<li onclick='" + onclick + "'>" + item.CPTCode + " " + String(item.CPTCodeDescription) + mod + "</li>");
                            }
                        }
                        else {

                            if ($('#' + ClinicalLabOrderDetail.params.PanelID + ' #LabOrderGroupFavList').is(':checked')) {
                                var onclick = 'ClinicalLabOrderDetail.BindLabOrderGridItem(\'' + item.Modifier + '\',\'' + item.CPTCode + '\',\'' + item.CPTCodeDescription + '\',\'' + item.CPTCodeDescription + '\',null,\'' + String(item.LabId) + '\',true)';
                            }
                            else {
                                var onclick = 'ClinicalLabOrderDetail.BindLabOrderGridItem(\'' + item.Modifier + '\',\'' + item.CPTCode + '\',\'' + item.CPTCodeDescription + '\',\'' + item.CPTCodeDescription + '\',null)';
                            }

                            if (item.LabName != null && item.LabName.length > 0) {
                                $uL.append('<li onclick="' + onclick + '">' + item.CPTCode + " " + item.CPTCodeDescription + ' (<strong>' + item.LabName + '</strong>)' + mod + '</li>');
                            } else {
                                $uL.append('<li onclick="' + onclick + '">' + item.CPTCode + " " + item.CPTCodeDescription + mod +'</li>');
                            }
                        }
                    }
                });
            }
            else {
                isEmptyUl = isEmptyUl == null ? true : false;
                if (isEmptyUl) {
                    $uL.empty();
                }
            }

        });
    },


    //Author: Abid Ali
    //Date :  17-03-2016
    //Reason: Bind Guaranter
    bindGuarantor: function () {
        var shortName = $('#pnlClinicalLabOrderDetail #txtGuarantor').val();
        utility.GetGuarontorArray(shortName).done(function (response) {

            $('#pnlClinicalLabOrderDetail #txtGuarantor').autocomplete({
                //source: AllPatients, // pass an array (without a comma)
                autoFocus: true,
                source: response,
                select: function (event, ui) {

                    setTimeout(function () {

                        $("#pnlClinicalLabOrderDetail #hfLabGuarantorId").val(ui.item.id); // add the selected id
                        if ($("#pnlClinicalLabOrderDetail #lnkGuarantorEdit").css("display") == "none") {
                            $("#pnlClinicalLabOrderDetail #lnkGuarantorEdit").css("display", "inline");
                            $("#pnlClinicalLabOrderDetail #lblGuarantor").css("display", "none");
                        }
                    }, 100);

                }
            });

        });

    },


    //Author: Abid Ali
    //Date :  17-03-2016
    //Reason: Open Guaranter
    openGuarantor: function () {
        var params = [];
        params["GuarantorId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ClinicalLabOrderDetail";
        params["RefCtrl"] = "txtGuarantor";
        LoadActionPan('Patient_Guarantor', params);
    },


    //Author: Abid Ali
    //Date :  17-03-2016
    //Reason: open Guaranter
    openGuarantorDetail: function () {
        //Patient_Guarantor.GuarantorEdit($('#pnlDemographic #hfGuarantor').val(), 'patTabDemographic');
        var params = [];
        params["GuarantorId"] = $('#pnlClinicalLabOrderDetail #hfLabGuarantorId').val();
        params["mode"] = "Edit";
        params["RefCtrl"] = "txtGuarantor";
        params["ParentCtrl"] = 'ClinicalLabOrderDetail';
        LoadActionPan('guarantorDetail', params);
    },

    //Author: Abid Ali
    //Date :  17-03-2016
    //Reason: Load auto complete for provider and Facility
    loadAllAutocomplete: function () {
        if (ClinicalLabOrderDetail.loadProviderFromAppData && ClinicalLabOrderDetail.ProviderOnDemandLoad) {
            if (globalAppdata.DefaultProviderName != "" && globalAppdata.DefaultProviderName != "- Select -") {
                CacheManager.BindCodes('GetProvider', false).done(function (result) {
                    var Ctrl = $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider");
                    var hfCtrl = $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfProvider");
                    var onSelect = function (dataItem) { $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider").attr("Provider", dataItem.id) }
                    var onChange = function (valid) { ClinicalLabOrderDetail.EnableDisableTestSearch(); }
                    utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, onSelect, onChange);

                    if (ClinicalLabOrderDetail.loadProviderFromAppData && ClinicalLabOrderDetail.ProviderOnDemandLoad) {
                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider").val(globalAppdata.DefaultProviderName);
                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider").attr("Provider", globalAppdata.DefaultProviderId);
                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfProvider").val(globalAppdata.DefaultProviderId);
                        utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider"), globalAppdata.DefaultProviderName, $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfProvider"), globalAppdata.DefaultProviderId);
                    }
                    $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").data('serialize', $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").serialize());
                });
                ClinicalLabOrderDetail.loadProviderFromAppData = true;
                ClinicalLabOrderDetail.ProviderOnDemandLoad = false;
                ClinicalLabOrderDetail.isProviderLoaded = true;
            }
            else {
                CacheManager.BindCodes('GetProvider', false).done(function (result) {
                    var Ctrl = $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider");
                    var hfCtrl = $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfProvider");
                    var onSelect = function (dataItem) { $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider").attr("Provider", dataItem.id) }
                    var onChange = function (valid) { ClinicalLabOrderDetail.EnableDisableTestSearch(); }
                    utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, onSelect, onChange);

                    //$('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider").val("");
                    //$('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider").attr("Provider", "");
                    //$('#' + ClinicalLabOrderDetail.params.PanelID + " #hfProvider").val("");
                    ClinicalLabOrderDetail.EnableDisableTestSearch();
                    $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").data('serialize', $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").serialize());
                });
                ClinicalLabOrderDetail.loadProviderFromAppData = true;
                ClinicalLabOrderDetail.ProviderOnDemandLoad = false;
                ClinicalLabOrderDetail.isProviderLoaded = true;
            }
        }

        if (ClinicalLabOrderDetail.loadFacilityFromAppData && ClinicalLabOrderDetail.FacilityOnDemandLoad) {

            if (globalAppdata.DefaultFacilityName != "" && globalAppdata.DefaultFacilityName != "- Select -") {
                CacheManager.BindCodes('GetFacility', false).done(function (result) {
                    var Ctrl = $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility");
                    var hfCtrl = $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfFacility");
                    var onSelect = function (dataItem) { $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility").attr("Facility", dataItem.id); }
                    var onChange = function (valid) { if (valid) ClinicalLabOrderDetail.PoupulateLoction(); }
                    utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl, onSelect, onChange);

                    if (ClinicalLabOrderDetail.loadFacilityFromAppData && ClinicalLabOrderDetail.FacilityOnDemandLoad) {
                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility").val(globalAppdata.DefaultFacilityName);
                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility").attr("Facility", globalAppdata.DefaultFacilityId);
                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfFacility").val(globalAppdata.DefaultFacilityId);
                        ClinicalLabOrderDetail.PoupulateLoction();
                        utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility"), globalAppdata.DefaultFacilityName, $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfFacility"), globalAppdata.DefaultFacilityId);
                    }
                    $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").data('serialize', $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").serialize());
                });
                ClinicalLabOrderDetail.loadFacilityFromAppData = true;
                ClinicalLabOrderDetail.FacilityOnDemandLoad = false;
                ClinicalLabOrderDetail.isFacilityLoaded = true;
            }
            else {
                CacheManager.BindCodes('GetFacility', false).done(function (result) {
                    var Ctrl = $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility");
                    var hfCtrl = $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfFacility");
                    var onSelect = function (dataItem) { $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility").attr("Facility", dataItem.id); }
                    var onChange = function (valid) { if (valid) ClinicalLabOrderDetail.PoupulateLoction(); }
                    utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl, onSelect, onChange);

                    //$('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility").val("");
                    //$('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility").attr("Facility", "");
                    //$('#' + ClinicalLabOrderDetail.params.PanelID + " #hfFacility").val("");
                    ClinicalLabOrderDetail.EnableDisableTestSearch();
                    $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").data('serialize', $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").serialize());
                });
                ClinicalLabOrderDetail.loadFacilityFromAppData = true;
                ClinicalLabOrderDetail.FacilityOnDemandLoad = false;
                ClinicalLabOrderDetail.isFacilityLoaded = true;
            }
        }
    },

    //Author: Abid Ali
    //Date :  17-03-2016
    //Reason: Enable disable Patient Insurances
    showHidePatientInsurances: function (primaryInsurance, secondaryInsurance, tertiaryInsurance) {
        //Disable DropdownLists if Insurance not exists
        var ddlIds = [];
        if (primaryInsurance.length <= 0)
            ddlIds.push('ddlPrimaryInsuraceId');
        if (secondaryInsurance.length <= 0)
            ddlIds.push('ddlSecondaryInsuraceId');
        if (tertiaryInsurance.length <= 0)
            ddlIds.push('ddlTertiaryInsuraceId');

        //Hide/Show Patient Insurance dropDownLists
        if (ddlIds.length > 0)
            ClinicalLabOrderDetail.enableDisableDropdownList(ddlIds, true);
        else {
            ddlIds.push('ddlPrimaryInsuraceId');
            ddlIds.push('ddlSecondaryInsuraceId');
            ddlIds.push('ddlTertiaryInsuraceId');
            ClinicalLabOrderDetail.enableDisableDropdownList(ddlIds, false);
        }
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Enable disable dropdownlists
    enableDisableDropdownList: function (listOfDdlIds, isHide) {

        $.each(listOfDdlIds, function (index, item) {
            if (isHide) {
                $('#' + ClinicalLabOrderDetail.params.PanelID + ' #' + item).prop('disabled', true);
            }
            else {
                $('#' + ClinicalLabOrderDetail.params.PanelID + ' #' + item).removeClass('disabled', false);
            }
        });
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Populate patient insurances in ddl
    populateInsuranceDropDownList: function (ddlTypeId, options) {
        var $ddl = $('#' + ClinicalLabOrderDetail.params.PanelID + ' #' + ddlTypeId);

        $ddl.empty();
        $ddl.append($('<option/>', {
            value: "",
            html: "- select -"
        }));
        $.each(options, function (i, item) {
            $ddl.append(
                $('<option/>', {
                    value: item.InsuranceId,
                    html: item.InsurancePlanName
                })
            );
        });
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Function to load problem list
    loadProblemList: function (fromProblemListSave) {

        $("#" + ClinicalLabOrderDetail.params.PanelID + ' #ulProblemLists input[type=checkbox]:checked').each(function (i, item) {
            if ($(item).attr("id") != "chkSelectAll") {
                ClinicalLabOrderDetail.pushProblems(item);
            }
        });


        var dfd = new $.Deferred();
        ClinicalLabOrderDetail.SearchProblemList().done(function (response) {
            var obj = JSON.parse(response);
            if (obj.status != false) {
                if (obj.ProblemListCount > 0) {
                    var ProblemListLoadJSONData = JSON.parse(obj.ProblemListLoad_JSON);
                    var ProblemListHistoryLoadJSONData = JSON.parse(obj.ProblemListHistoryLoad_JSON);
                    var finalTr = '';
                    var counter = 2;
                    $("#" + ClinicalLabOrderDetail.params.PanelID + " #ulProblemLists tbody tr").remove();
                    $.each(ProblemListLoadJSONData, function (i, item) {
                        if (counter % 2 == 0) {
                            finalTr = finalTr + '<tr>';
                        }
                        if (counter == 2) {
                            finalTr = finalTr + '<td><div class="col-xs-12 p-xs"><div class="checkbox-custom">';
                            finalTr = finalTr + '<input type="checkbox" onclick ="ClinicalLabOrderDetail.selectAllProblems(this);"  id="chkSelectAll">';

                            finalTr = finalTr + '<label for="chkSelectAll" class="control-label">Select All</label></div></div></td>';
                            counter++;
                        }



                        try {
                            if (item.Description == null || item.Description == '') {
                                item.Description = '';
                                //if (item.ICD9 != null && item.ICD9 != '') {
                                //    item.Description = item.ICD9 + ' - ';
                                //}
                                if (item.ICD10 != null && item.ICD10 != '') {
                                    item.Description = item.Description + item.ICD10 + ' - ';
                                }
                                if (item.SNOMEDID != null && item.SNOMEDID != '') {
                                    item.Description = item.Description + item.SNOMEDID + ' - ';
                                }
                                if (item.ICD10_Description != null && item.ICD10_Description != '') {
                                    item.Description = item.Description + item.ICD10_Description;
                                }
                                    //else if (item.ICD9_Description != null && item.ICD9_Description != '') {
                                    //    item.Description = item.Description + item.ICD9_Description;
                                    //}
                                else if (item.ProblemName != null && item.ProblemName != '') {
                                    item.Description = item.Description + item.ProblemName;
                                }

                            } else if (item.Description.split(" - ")[2] != undefined) {
                                item.Description = item.Description.substring(item.Description.indexOf("-") + 2);
                            }
                        } catch (ex) {
                            console.log(ex);
                        }


                        finalTr = finalTr + '<td><div class="col-xs-12 p-xs"><div class="checkbox-custom">';

                        var checked = ClinicalLabOrderDetail.isCheckedProblem('chk' + item.ProblemListId + '') ? "checked" : "";
                        //if (checked == "") {
                        //    if (ClinicalLabOrderDetail.params.mode == "Add") {
                        //        var Notes = item.NoteId.split(',');
                        //        //if ($.inArray('' + ClinicalLabOrderDetail.params.FromNoteId + '', Notes) != -1) {
                        //        //    checked = "checked";
                        //        //    fromProblemListSave = false;
                        //        //}
                        //    }
                        //}
                        finalTr = finalTr + '<input ' + checked + ' type="checkbox" onclick ="ClinicalLabOrderDetail.pushProblems(this); ClinicalLabOrderDetail.checkOrUncheckSelectAll();" name="' + item.Code + '" value="' + item.ProblemListId + '"  id="chk' + item.ProblemListId + '" class="chkProblem">';

                        finalTr = finalTr + '   <label for="chk' + item.ProblemListId + '" class="control-label">' + item.Description + '</label></div></div></td>';


                        if (counter % 2 == 1) {
                            finalTr = finalTr + '</tr>';
                        }
                        counter++;
                        var color = "";
                    });
                    $("#" + ClinicalLabOrderDetail.params.PanelID + " #ulProblemLists tbody").append(finalTr);
                    //-------------
                    //if (ClinicalLabOrderDetail.params.mode == "Edit") {
                    //    fromProblemListSave = false;
                    //}

                    if (Clinical_ProgressNote.params.newlyAddedProblemLists && Clinical_ProgressNote.params.newlyAddedProblemLists.length > 0) {
                        $.each(Clinical_ProgressNote.params.newlyAddedProblemLists, function (i, item) {
                            $("#" + ClinicalLabOrderDetail.params.PanelID + ' #ulProblemLists input[type=checkbox]').each(function () {
                                if (item == $(this).attr('value') && !($(this).attr('checked'))) {
                                    $("#" + ClinicalLabOrderDetail.params.PanelID + ' #ulProblemLists' + ' #chk' + item).trigger('click');
                                }
                                //console.log(item);
                                //console.log($(this).attr('value'));
                            });
                        });
                    } else {
                        if (fromProblemListSave) {
                            var _val = 0;
                            var _id = "";
                            $("#" + ClinicalLabOrderDetail.params.PanelID + ' #ulProblemLists input[type=checkbox]').each(function () {
                                if (parseInt($(this).attr('value')) > _val) {
                                    _val = $(this).attr('value');
                                    _id = $(this).attr('id');
                                }
                            });
                            $("#" + ClinicalLabOrderDetail.params.PanelID + ' #ulProblemLists' + ' #' + _id).trigger('click');
                        }
                    }
                    //-----------
                }

                var LabOrderId = ClinicalLabOrderDetail.params.LabOrderId;
                LabOrderId = typeof LabOrderId != 'undefined' && LabOrderId > 0 ? LabOrderId : -1;

                if (!fromProblemListSave && ClinicalLabOrderDetail.params.mode == "Edit") {
                    ClinicalLabOrderDetail.loadLabOrder(LabOrderId);
                }
                $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfLabOrderId").val(LabOrderId);
                $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").data('serialize', $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").serialize());

                ClinicalLabOrderDetail.checkOrUncheckSelectAll();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
            dfd.resolve();
        });
        return dfd.promise();
    },
    checkOrUncheckSelectAll: function () {
        var anyUncheck = false
        $.each($('#' + ClinicalLabOrderDetail.params.PanelID).find(".chkProblem"), function (i, item) {
            if (!$(item).prop('checked')) {
                anyUncheck = true;
            }
        })
        if (anyUncheck) {
            $('#' + ClinicalLabOrderDetail.params.PanelID + " #chkSelectAll").prop('checked', false);
        }
        else {
            $('#' + ClinicalLabOrderDetail.params.PanelID + " #chkSelectAll").prop('checked', true);
        }
    },
    //Author: Abid Ali
    //Date :  15-07-2016
    //Reason: function to push checked problems in array
    pushProblems: function (obj) {

        var $obj = $(obj);
        var isChecked = $obj.prop('checked');
        var id = $obj.attr('id');
        if (isChecked) {
            //Push in checked problems
            if (!($.inArray(id, ClinicalLabOrderDetail.checkedProblems) != -1)) {
                ClinicalLabOrderDetail.checkedProblems.push(id);
            }
        }
        else {
            //Remove from checked problems
            ClinicalLabOrderDetail.checkedProblems.splice($.inArray(id, ClinicalLabOrderDetail.checkedProblems), 1);
        }

    },

    selectAllProblems: function (obj) {
        if ($(obj).prop('checked')) {
            $.each($('#' + ClinicalLabOrderDetail.params.PanelID).find(".chkProblem"), function (i, item) {
                $(item).prop('checked', true)
                var id = $(item).attr('id');
                if (!($.inArray(id, ClinicalLabOrderDetail.checkedProblems) != -1)) {
                    ClinicalLabOrderDetail.checkedProblems.push(id);
                }

            });
        }
        else {

            $.each($('#' + ClinicalLabOrderDetail.params.PanelID).find(".chkProblem"), function (i, item) {
                $(item).prop('checked', false)
                var id = $(item).attr('id');
                ClinicalLabOrderDetail.checkedProblems.splice($.inArray(id, ClinicalLabOrderDetail.checkedProblems), 1);
            });
        }
    },
    //Author: Abid Ali
    //Date :  15-07-2016
    //Reason: function to check problem in array
    isCheckedProblem: function (problemId) {

        return $.inArray(problemId, ClinicalLabOrderDetail.checkedProblems) == -1 ? false : true;

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Db_Call for search patient's Insurances
    searchPatientInsurance: function (patientID, PatientInsuranceId) {
        if (patientID == null) {
            patientID = Patient_Insurance.params.patientID;
        }
        if (PatientInsuranceId == null || PatientInsuranceId == "" || PatientInsuranceId <= 0) {
            PatientInsuranceId = "";
        }

        var data = "PatientID=" + patientID + "&PatientInsuranceId=" + PatientInsuranceId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_INSURANCE", "SEARCH_PATIENT_INSURANCE_FROMLAB");
    },

    loadPatientGuarantor: function (patientID) {
        if (patientID == null) {
            patientID = Clinical_LabOrder.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        }
        var objData = new Object();
        objData["PatientId"] = patientID;

        objData["commandType"] = "SEARCH_Guarantor";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabOrder", "LabOrder");
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Edit LabOrder
    LabEdit: function (LabOrderId, event) {
        //if icon is clicked then  popup the window

        ClinicalLabOrderDetail.loadProviderFromAppData = true;
        ClinicalLabOrderDetail.loadFacilityFromAppData = true;

        Clinical_LabOrder.LabOrderAddEdit(LabOrderId);
        event.stopPropagation();

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Function to load problem list
    SearchProblemList: function () {

        var IsCheckedIn = null;
        IsCheckedIn = $('#' + Clinical_ProblemLists.params.PanelID + ' #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive');
        if (IsCheckedIn == null) {
            IsCheckedIn = "1";
        }

        var PageNumber = 1;
        var RowsPerPage = 2000;

        var objData = new Object();
        objData["PatientId"] = Clinical_LabOrder.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        objData["IsActive"] = '1';
        // objData["ProblemListId"] = ProblemListId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "SEARCH_PROBLEMLIST";
        //objData["NoteId"] = Clinical_ProblemLists.params.NotesId == null ? 0 : Clinical_ProblemLists.params.NotesId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");

    },

    // -------------- Provider ---------------------
    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: open provider form
    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmClinicalLabOrderDetail";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ClinicalLabOrderDetail";
        LoadActionPan('Admin_Provider', params);
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: open provider detail
    OpenProviderDetail: function () {
        //Admin_Provider.ProviderEdit($('#pnlClinicalNotes #hfProvider').val(),'clinicalTabNotes');
        var params = [];
        params["ProviderId"] = $('#pnlClinicalLabOrderDetail #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'ClinicalLabOrderDetail';
        LoadActionPan('providerDetail', params);
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: open facility form
    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmClinicalLabOrderDetail";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ClinicalLabOrderDetail";
        LoadActionPan('Admin_Facility', params);
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: open facility detail form
    OpenFacilityDetail: function () {
        //Admin_Facility.FacilityEdit($('#' + demographicDetail.params.PanelID + ' #hfFacility').val(), "demographicDetail");
        var params = [];
        params["FacilityId"] = $('#pnlClinicalLabOrderDetail #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacility";
        params["ParentCtrl"] = 'ClinicalLabOrderDetail';
        LoadActionPan('facilityDetail', params);
    },

    HideProviderLink: function () {
        $('#pnlClinicalLabOrderDetail #txtProvider').attr("ProviderId", "-1");
        $('#pnlClinicalLabOrderDetail #hfProvider').val("-1");
        $("#pnlClinicalLabOrderDetail #lnkProviderEdit").css("display", "none");
        $("#pnlClinicalLabOrderDetail #lblProvider").css("display", "inline");
    },
    // -------------- End Provider -----------------

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Function to apply bootstrap validations
    validateLabOrderDetail: function () {
        $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail")
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   LabId: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Provider: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Facility: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            if (e.type == "success") {
                //Start 22-03-2016 Humaira Yousaf for multiple submit buttons
                var $form = $(e.target);
                var tridderedText = 'btnSaveOrder';

                var $button = $('button[issubmit=true].submitter');

                var Modestatus = "";

                if ($button) {
                    tridderedText = $button.attr('id');
                }

                if ($button && $button.text() == 'Add Result') {

                    Modestatus = 'Add Result';
                }
                switch (tridderedText) {
                    //Start 28-11-2016 Humaira Yousaf for associated problems validation

                    // MK: http://192.168.0.85:8080/browse/IMP-482
                    // Problems won't be mandatory

                    case 'btnSaveOrder':
                        var isProblemAdded = ClinicalLabOrderDetail.problemAdded();
                        if (isProblemAdded == true) {
                            ClinicalLabOrderDetail.LabOrderSave('save');
                        } else {
                            utility.kendoConfirm('Confirm Save', 'No ICD code is associated with this order. Do you wish to continue?', function () {
                                ClinicalLabOrderDetail.LabOrderSave('save');
                            });

                        }
                        //else {
                        //    utility.DisplayMessages("Associate at least one Problem with the order.", 3);
                        //}
                        break;
                        //case 'btnSignOrder':
                        //    var isProblemAdded = ClinicalLabOrderDetail.problemAdded();
                        //    if (isProblemAdded == true) {
                        //        ClinicalLabOrderDetail.LabOrderSave('signprintorder');
                        //        ClinicalLabOrderDetail.disableLabOrderControls(true);
                        //    } else {
                        //        utility.kendoConfirm('Confirm Sign', 'No ICD code is associated with this order. Do you wish to continue?', function () {
                        //            ClinicalLabOrderDetail.LabOrderSave('signprintorder');
                        //            ClinicalLabOrderDetail.disableLabOrderControls(true);
                        //        });

                        //    }
                        //    //else {
                        //    //    utility.DisplayMessages("Associate at least one Problem with the order.", 3);
                        //    //}

                        //    break;

                    case 'btnPrintLabOrder':
                        var isProblemAdded = ClinicalLabOrderDetail.problemAdded();
                        if (isProblemAdded == true) {
                            ClinicalLabOrderDetail.LabOrderSave('saveprintorder', true, false);

                        } else {
                            utility.kendoConfirm('Confirm Sign', 'No ICD code is associated with this order. Do you wish to continue?', function () {
                                ClinicalLabOrderDetail.LabOrderSave('saveprintorder', true, false);
                            });

                        }
                        //else {
                        //    utility.DisplayMessages("Associate at least one Problem with the order.", 3);
                        //}

                        break;
                    case 'btnPrintOrder':
                        utility.kendoConfirm('Specimen Label Printing', 'Would you like to print the Specimen Label for this order?',
                            function () {//success
                                ClinicalLabOrderDetail.printLabOrder(true);
                                ClinicalLabOrderDetail.disableLabOrderControls(true);
                            }, function () {//Cancel
                                ClinicalLabOrderDetail.printLabOrder(false);
                                ClinicalLabOrderDetail.disableLabOrderControls(true);
                            }
                        );
                        break;

                    case 'btnPrintABN':

                        ClinicalLabOrderDetail.printLabOrderABN();


                        break;

                    case 'btnSignPrintOrder':

                        var isProblemAdded = ClinicalLabOrderDetail.problemAdded();

                        var IsContinue = true;

                        if (!isProblemAdded) {
                            utility.kendoConfirm('Confirmation', 'No ICD code is associated with this order. Do you wish to continue?',
                               function () {//success
                                   //if (!ClinicalLabOrderDetail.IsFavoriteGroupSelected) {
                                   if ($("#donotaskedagain:checked").length > 0) {
                                       utility.AddKey2LocalStorageForDoNotAskedAgain();
                                   }
                                   utility.kendoConfirm('Specimen Label Printing', 'Would you like to print the Specimen Label for this order?',
                                   function () {//success
                                       ClinicalLabOrderDetail.LabOrderSave('signprintorder', true);

                                   }, function () {//Cancel
                                       ClinicalLabOrderDetail.LabOrderSave('signprintorder', false);

                                   });
                                   //}
                                   //else {
                                   //    ClinicalLabOrderDetail.LabOrderSave('signprintorder', false);
                                   //}
                               }
                            );

                        }
                        else {
                            //if (!ClinicalLabOrderDetail.IsFavoriteGroupSelected) {
                            utility.kendoConfirm('Specimen Label Printing', 'Would you like to print the Specimen Label for this order?',
                            function () {//success
                                ClinicalLabOrderDetail.LabOrderSave('signprintorder', true);

                            }, function () {//Cancel
                                ClinicalLabOrderDetail.LabOrderSave('signprintorder', false);

                            });
                            //}
                            //else {
                            //    ClinicalLabOrderDetail.LabOrderSave('signprintorder', false);
                            //}
                        }

                        break;
                    case 'btnAddLabResult':
                        Clinical_LabOrder.LabResultAddEdit(-1, ClinicalLabOrderDetail.params.LabOrderId, true, "AddResult", Modestatus)
                        break;
                    case 'btnEditLabResult':
                        break;
                    default:
                        var isProblemAdded = ClinicalLabOrderDetail.problemAdded();
                        if ($("#" + ClinicalLabOrderDetail.params.PanelID + " #dgvLabOrderDetail tbody tr").length > 0 && $("#" + ClinicalLabOrderDetail.params.PanelID + " #dgvLabOrderDetail tbody tr").text() != "No data available in table") {
                            if (isProblemAdded == true) {
                                ClinicalLabOrderDetail.LabOrderSave('save');
                            } else {
                                utility.kendoConfirm('Confirm Sign', 'No ICD code is associated with this order. Do you wish to continue?', function () {
                                    ClinicalLabOrderDetail.LabOrderSave('save');
                                });

                            }
                        }
                        //else {
                        //    utility.DisplayMessages("Associate at least one Problem with the order.", 3);
                        //}
                        break;
                        //End 28-11-2016 Humaira Yousaf for associated problems validation
                }
                //End 22-03-2016 Humaira Yousaf for multiple submit buttons
            }
            e.type = "";
        });

    },


    EnableDisableTestSearch: function (ddlLab, IsFromProviderFacility) {

        var dfd = new $.Deferred();

        //if ($(ddlLab).val() != '') {
        var LabId = $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlLabId option:selected').val();
        var ProviderId = $('#' + ClinicalLabOrderDetail.params.PanelID + ' #hfProvider').val();
        if (ProviderId != '' && LabId != '') {
            $('#' + ClinicalLabOrderDetail.params.PanelID + ' #txtLabCPTCode').removeClass('disableAll');
            //$('#' + ClinicalLabOrderDetail.params.PanelID + ' #favSectionDiv').removeClass('disableAll');
            if (EMRUtility.getFreeTextStatus(ClinicalLabOrderDetail.FavListName)) {

                if (ClinicalLabOrderDetail.params.mode == "Add") {
                    $("#pnlClinicalLabOrderDetail #LabOrderGroupFavList").prop("checked", true);
                    ClinicalLabOrderDetail.favoriteListSearch('LabOrderGroup', IsFromProviderFacility).done(function () {
                        dfd.resolve();
                    });

                } else {
                    $("#pnlClinicalLabOrderDetail #LabOrderFavList").prop("checked", true);
                    ClinicalLabOrderDetail.favoriteListSearch('LabOrder', IsFromProviderFacility).done(function () {
                        dfd.resolve();
                    });
                }
            }
            else {
                //if (ClinicalLabOrderDetail.params["LastLabName"] != "") {
                $("#pnlClinicalLabOrderDetail #LabOrderFavList").prop("checked", true);
                ClinicalLabOrderDetail.favoriteListSearch('LabOrder', IsFromProviderFacility).done(function () {
                    dfd.resolve();
                });
                //}
            }
        }
        else {
            $('#' + ClinicalLabOrderDetail.params.PanelID + ' #txtLabCPTCode').addClass('disableAll');
            if (EMRUtility.getFreeTextStatus(ClinicalLabOrderDetail.FavListName)) {

                if (ClinicalLabOrderDetail.params.mode == "Add") {
                    $("#pnlClinicalLabOrderDetail #LabOrderGroupFavList").prop("checked", true);
                    ClinicalLabOrderDetail.favoriteListSearch('LabOrderGroup', IsFromProviderFacility).done(function () {
                        dfd.resolve();
                    });
                }
                else {
                    var $ddl = $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlFavoriteListLabOrder');
                    $ddl.empty();
                    $ddl.append($('<option/>', {
                        value: "",
                        html: "- Select -"
                    }));
                    $ddl.val("");
                    $ddl.trigger("onchange");

                    dfd.resolve();
                }
            }
            else {
                var $ddl = $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlFavoriteListLabOrder');
                $ddl.empty();
                $ddl.append($('<option/>', {
                    value: "",
                    html: "- Select -"
                }));
                $ddl.val("");
                $ddl.trigger("onchange");

                dfd.resolve();
            }

            // $('#' + ClinicalLabOrderDetail.params.PanelID + ' #favSectionDiv').addClass('disableAll');
        }

        return dfd.promise();
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Binding numpad with height, weight, systolic and diastolic fields
    domReadyFunction: function () {


        $(document).ready(function () {

            $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlBillingTypeId').on('change', function () {

                if ($(this).val() == 3) {
                    //   ClinicalLabOrderDetail.billingtype = 3;
                    ClinicalLabOrderDetail.enableDisableInsurances(false);

                    var selectedVal1 = '';
                    var selectedVal2 = '';
                    var selectedVal3 = '';
                    $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlPrimaryInsuraceId option').each(function () {
                        if ($(this).attr('Priority') == "1") {
                            selectedVal1 = $(this).text();
                            $("#hfPrimaryInsId").val($(this).val());

                        }

                    });
                    $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlPrimaryInsuraceId').val(selectedVal1);

                    $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlSecondaryInsuraceId option').each(function () {
                        if ($(this).attr('Priority') == "2") {
                            selectedVal2 = $(this).text();
                            $("#hfSecondaryInsId").val($(this).val());
                        }
                    });
                    $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlSecondaryInsuraceId').val(selectedVal2);

                    $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlTertiaryInsuraceId option').each(function () {
                        if ($(this).attr('Priority') == "3") {
                            selectedVal3 = $(this).text();
                            $("#hfTertiaryInsId").val($(this).val());
                        }
                    });
                    $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlTertiaryInsuraceId').val(selectedVal3);

                    //Start//07-03-2016//Abid Ali//set gurantor and relation vlaues
                    ClinicalLabOrderDetail.setGurantorRelationValues(true);
                    //End//07-03-2016//Abid Ali//set gurantor and relation vlaues
                }
                else {

                    ClinicalLabOrderDetail.enableDisableInsurances(false);

                    $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlPrimaryInsuraceId').val('');
                    $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlSecondaryInsuraceId').val('');
                    $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlTertiaryInsuraceId').val('');

                    //Start//07-03-2016//Abid Ali//set gurantor and relation vlaues
                    ClinicalLabOrderDetail.setGurantorRelationValues(false);
                    //End//07-03-2016//Abid Ali//set gurantor and relation vlaues
                }

            });




            $('.toggleHorSmallLeft section').click(function (e) {
                $(this).parent().toggleClass("toggled");
                ClinicalLabOrderDetail.toggleHorSmallLeftIcon($(this));

            });

            $('#frmClinicalLabOrderDetail .submitter').click(function (e) {
                $('.submitter').attr('isSubmit', false);
                $(this).attr('isSubmit', true);

            });
        });
        $(function () {
            $('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail [data-plugin-keyboard-numpad]').keyboard({
                customLayout: {
                    'default': [
                        '7 8 9 {b}',
                        '4 5 6 {clear}',
                        '1 2 3 {t}',
                        '0   .  {a} {c} '
                    ]
                },
                change: function (e, keyboard, el) {
                    if (keyboard.$preview.attr('maxlength') != null && !keyboard.$preview.keyboard().getkeyboard().options.maxLength) {
                        keyboard.$preview.keyboard().getkeyboard().options.maxLength = keyboard.$preview.attr('maxlength');
                    }
                    if (keyboard.$preview.attr('oninput') != null) {
                        keyboard.$preview.trigger('oninput');
                    }
                    // Fix # EMR-96
                    if (keyboard.$preview.attr('name') == 'Height') {
                        if (keyboard.$preview.attr('onkeyup') != null) {
                            keyboard.$preview.trigger('onkeyup');
                            EMRUtility.ValidateHeight(e, keyboard.$preview);
                        }
                    } else if (keyboard.$preview.attr('onkeyup') != null) {
                        keyboard.$preview.trigger('onkeyup');
                    }

                },
                layout: 'custom',
                reposition: true,
                appendLocally: this,
                restrictInput: true,
                preventPaste: true,
                usePreview: false,
                autoAccept: true,
                tabNavigation: true
            })
                    .addTyping();
        });
        $("#pnlClinicalLabOrderDetail").on("keypress", 'form', function (e) {
            var code = e.keyCode || e.which;
            if (code == 13) {
                e.preventDefault();
                return false;
            }
        });
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: To Disable inurances
    enableDisableInsurances: function (enable) {
        if (enable) {
            $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlPrimaryInsuraceId').attr('disabled', false);
            $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlSecondaryInsuraceId').attr('disabled', false);
            $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlTertiaryInsuraceId').attr('disabled', false);

            //Start//Abid Ali//For bug# EMR-1253
            ClinicalLabOrderDetail.setGurantorRelationValues(true);
            //End//Abid Ali//For bug# EMR-1253
        }
        else {
            $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlPrimaryInsuraceId').attr('disabled', true);
            $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlSecondaryInsuraceId').attr('disabled', true);
            $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlTertiaryInsuraceId').attr('disabled', true);

            //Start//Abid Ali//For bug# EMR-1253
            ClinicalLabOrderDetail.setGurantorRelationValues(true);
            //End//Abid Ali//For bug# EMR-1253
        }
    },

    toggleHorSmallLeftIcon: function (e) {
        if (e === undefined) {
            var icon = $('.toggleHorSmallLeft');
            icon.each(function (i) {
                var $this = $(this).children("section").children();
                if ($(this).hasClass("toggled")) {
                    $this.append('<i class="fa fa-chevron-down"></i>');
                }
                else {
                    $this.append('<i class="fa fa-chevron-up"></i>');
                }
            });
        }
        else if (e != undefined) {
            var icon = $(e.children().children());
            if (icon.hasClass("fa-chevron-up")) {
                icon.toggleClass("fa-chevron-down fa-chevron-up")
            }
            else {
                icon.toggleClass("fa-chevron-up fa-chevron-down")
            }
        }
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: To validate blood pressure
    ValidateBP: function (objSystolic, objDiastolic) {
        var systolicVal = 0;
        var diastolicVal = 0;
        if (objSystolic != null) {
            systolicVal = $(objSystolic).val();
        }
        else if (objDiastolic != null) {
            objSystolic = $($(objDiastolic).parent().parent().prevAll()[0]).find("input[id*='txtSystolic']");
            systolicVal = $(objSystolic).val();

        }
        if (objDiastolic != null) {
            diastolicVal = $(objDiastolic).val();
        }
        else if (objSystolic != null) {
            objDiastolic = $($(objSystolic).parent().parent().nextAll()[0]).find("input[id*='txtDiastolic']");
            diastolicVal = $(objDiastolic).val();
        }
        if ((diastolicVal != "" && systolicVal != "") && (parseInt(diastolicVal) >= parseInt(systolicVal))) {
            $(objDiastolic).css("border", "1px solid red");
            utility.DisplayMessages("Diastolic should be less than Systolic", 3);
        }
        else {
            if (systolicVal != "") {
                $(objSystolic).css("border", "1px solid #ccc");
                if (diastolicVal == "") {
                    $(objDiastolic).css("border", "1px solid red");
                }
                else {
                    $(objDiastolic).css("border", "1px solid #ccc");
                }

            }
            else if (diastolicVal != "") {
                $(objDiastolic).css("border", "1px solid #ccc");
                if (systolicVal == "") {
                    $(objSystolic).css("border", "1px solid red");
                }
                else {
                    $(objSystolic).css("border", "1px solid #ccc");
                }
            }
            else {
                $(objDiastolic).css("border", "1px solid #ccc");
                $(objSystolic).css("border", "1px solid #ccc");
            }
        }

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: To validate BSA
    calculateBSA: function (objWeight, objHeightInFeet, TargetCtrl) {

        var WeightInLbs = "";
        if (objWeight != null) {
            WeightInLbs = $(objWeight).val();
        }
        else {
            WeightInLbs = $("#" + ClinicalLabOrderDetail.params.PanelID + " #txtWeight").val();
        }

        var HeightInFeet = "";
        if (objHeightInFeet != null) {
            HeightInFeet = $(objHeightInFeet).val();
        }
        else {
            HeightInFeet = $("#" + ClinicalLabOrderDetail.params.PanelID + " #txtHeight").val();
        }

        var CtrlName = "";
        if (TargetCtrl != null) {
            CtrlName = $(TargetCtrl);
        }
        else {
            CtrlName = $("#" + ClinicalLabOrderDetail.params.PanelID + " #txtBSA");
        }

        if (WeightInLbs == "" || WeightInLbs == ".")
            var weightInLbs = 0;
        else
            var weightInLbs = parseFloat(WeightInLbs);
        if (HeightInFeet == "" || HeightInFeet == ".")
            heightInFeet = 0;
        else
            var heightInFeet = parseFloat(HeightInFeet);

        var weightInKG = ClinicalLabOrderDetail.convertWeight(weightInLbs);
        var heightIn_cm = ClinicalLabOrderDetail.convertHeightTo_cm(heightInFeet);
        var result = 0.007184 * Math.pow(heightIn_cm, 0.725) * Math.pow(weightInKG, 0.425);
        var BSA = result.toFixed(2)
        if (WeightInLbs != "" && HeightInFeet != "")
            CtrlName.val(BSA);
        else
            CtrlName.val('');
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: To validate Weight
    convertWeight: function (pounds) {
        return pounds / 2.20462262185;
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: To validate height
    convertHeightTo_cm: function (feet) {
        return feet * 12 * 2.54;
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: To calculate BMI on the basis of height and weight
    calculateBMI: function (objWeight, objHeightInFeet, TargetCtrl) {

        var WeightInLbs = "";
        if (objWeight != null) {
            WeightInLbs = $(objWeight).val();
        }
        else {
            WeightInLbs = $("#" + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #txtWeight").val();
        }

        var HeightInFeet = "";
        if (objHeightInFeet != null) {
            HeightInFeet = $(objHeightInFeet).val();
        }
        else {
            HeightInFeet = $("#" + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #txtHeight").val();
        }

        var CtrlName = "";
        if (TargetCtrl != null) {
            CtrlName = $(TargetCtrl);
        }
        else {
            CtrlName = $("#" + ClinicalLabOrderDetail.params.PanelID + " #txtBMI");
        }

        if (WeightInLbs == "" || WeightInLbs == ".")
            var weightInLbs = 0;
        else
            var weightInLbs = parseFloat(WeightInLbs);
        if (HeightInFeet == "" || HeightInFeet == ".")
            heightInFeet = 0;
        else
            var heightInFeet = parseFloat(HeightInFeet);

        var heightInInches = ClinicalLabOrderDetail.convertHeightInches(heightInFeet);

        var result = (weightInLbs / (heightInInches * heightInInches)) * 703;
        var BMI = result.toFixed(2);
        if (WeightInLbs != "" && HeightInFeet != "" && BMI != "Infinity")
            CtrlName.val(BMI);
        else
            CtrlName.val('');
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: To convert height to inches
    convertHeightInches: function (feet) {
        var newFeet = feet.toString();
        var a = newFeet.split(".");
        var fee = parseInt(a[0]);
        var inch = parseInt(a[1]);
        if (isNaN(inch))
            return (fee * 12);
        else
            return (fee * 12) + inch;
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Loading ICD Codes for Problem List AutoComplete
    bindICD9AutoComplete: function (element) {
        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "ClinicalLabOrderDetail", null, false);
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Loading ICD Codes for Popup
    OpenSearchPopup: function (SearchType, Ctrl, HiddenCtrl) {
        var controlToLoad = "";
        if (SearchType == "ICD") {

            controlToLoad = "Admin_IMOICD";
        }
        else if (SearchType == "CPT") {
            controlToLoad = "Admin_IMOCPT";
        }
        else if (SearchType == "Modifier") {
            controlToLoad = "Admin_Modifier";
        }
        var params = [];
        params["FromAdmin"] = "0";
        if (ClinicalLabOrderDetail.params.TabID == 'clinicalTabProgressNote') {
            params['FromProgressNote'] = 'pnlClinicalProgressNote';
            params["ParentCtrl"] = 'ClinicalLabOrderDetail';
        }

        else {
            params["ParentCtrl"] = ClinicalLabOrderDetail.params["TabID"];

        }
        params["PanelID"] = ClinicalLabOrderDetail.params["PanelID"];

        params["ActionPanContainer"] = ClinicalLabOrderDetail.params["ActionPanContainer"];
        if (Ctrl != null) {
            params["RefCtrl"] = Ctrl;
        }
        if (HiddenCtrl != null) {
            params["RefHiddenCtrl"] = HiddenCtrl;
        }
        if (controlToLoad != "") {
            if (ClinicalLabOrderDetail.params.TabID == 'clinicalTabProgressNote' && SearchType == "ICD")
                LoadActionPan(controlToLoad, params, 'pnlClinicalProgressNote');
            else
                LoadActionPan(controlToLoad, params, ClinicalLabOrderDetail.params.PanelID);
        }

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: deleting Lis of Problem list
    deleteProblemList: function (obj, ev) {
        ev.stopPropagation();
        var problemListId = $(obj).attr('id');
        if (problemListId < 0) {
            $(obj).remove();
        }

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: deleting Lis of Allergies list
    deleteAllergy: function (obj, ev) {
        ev.stopPropagation();
        var allergyId = $(obj).attr('id');
        if (allergyId < 0) {
            $(obj).remove();
        }

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: deleting Lis of Medications list
    deleteMedication: function (obj, ev) {
        ev.stopPropagation();
        var medicationId = $(obj).attr('id');
        if (medicationId < 0) {
            $(obj).remove();
        }

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: to show delete icon on hover
    showIcon: function (obj) {

        $(obj).find('div').css('display', '');

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: to hide delete icon on mouse leave
    hideIcon: function (obj) {

        if ($(obj).hasClass("active") == false) {
            $(obj).find('div').css('display', 'none');
        }

    },

    UnLoad: function (caller) {
        var form = '#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail";
        var saveButtonisHidden = $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #btnSaveOrder").hasClass("hidden");
        var savelabOrderId = ClinicalLabOrderDetail.SavelabOrderId;
        if (caller == 'saveExit' || saveButtonisHidden == true || ClinicalLabOrderDetail.SaveModeSet == true) {
            if (ClinicalLabOrderDetail.params["ParentCtrl"] == "Clinical_LabOrder") {
                ClinicalLabOrderDetail.clearData();
                UnloadActionPan(ClinicalLabOrderDetail.params["ParentCtrl"], "ClinicalLabOrderDetail", null, ClinicalLabOrderDetail.params["ParentCtrlPanelID"]);

            }
            else {
                ClinicalLabOrderDetail.clearData();
                UnloadActionPan(ClinicalLabOrderDetail.params["ParentCtrl"], "ClinicalLabOrderDetail");

            }
            if (ClinicalLabOrderDetail.SaveModeSet) {
                $('#pnlClinicalLabOrder #ulSocialHxTabsItems a[href="#PatientLabOrderPending_ProgressNote"]').tab('show');
                //$('#pnlClinicalLabOrder #ordersPending').trigger('click');
                if (ClinicalLabOrderDetail.params.ParentCtrl == "Clinical_Treatment") {
                    Clinical_Treatment.LabOrderSearch(null, null, null, null, "Pending");
                }
                else {
                    $.when(Clinical_LabOrder.LabOrderSearch('0', null, null, null, 'Pending')).then(function () {
                        if (ClinicalLabOrderDetail.params.ParentCtrl == "Clinical_LabOrder" && Clinical_LabOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                            if (savelabOrderId.toString().indexOf(',') > -1) {
                                $.each(savelabOrderId.split(','), function (i, item) {
                                    if (item != "") {
                                        //$("#pnlClinicalProgressNote #pnlClinicalLabOrder #dgvLabOrder input#" + item).prop("checked", true);
                                        $("#pnlClinicalProgressNote #pnlClinicalLabOrder #dgvLabOrder input#" + item).trigger('click');
                                    }
                                });
                            }
                            else {
                                //$("#pnlClinicalProgressNote #pnlClinicalLabOrder #dgvLabOrder input#" + savelabOrderId).prop("checked", true);
                                $("#pnlClinicalProgressNote #pnlClinicalLabOrder #dgvLabOrder input#" + savelabOrderId).trigger('click');
                            }

                        }
                    });
                }

            }
            else {
                if (ClinicalLabOrderDetail.AddResult && Clinical_LabOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
                    $('#pnlClinicalLabOrder #ulSocialHxTabsItems a[href="#PatientLabResult_ProgressNote"]').tab('show');
                    $.when(Clinical_LabOrder.LabResultsSearch(null, null, null, null)).then(function () {
                        //$("#pnlClinicalProgressNote #pnlClinicalLabOrder #dgvLabResult input#" + ClinicalLabOrderDetail.AddedLabOrderResultId).prop("checked", true);
                        $("#pnlClinicalProgressNote #pnlClinicalLabOrder #dgvLabResult input#" + ClinicalLabOrderDetail.AddedLabOrderResultId).trigger('click');
                    });
                }
                else {
                    $('#pnlClinicalLabOrder #ulSocialHxTabsItems a[href="#PatientLabOrderSent_ProgressNote"]').tab('show');
                    $('#pnlClinicalLabOrder #ulSocialHxTabsItems a[href="#PatientLabOrderSent"]').tab('show');
                    //$('#pnlClinicalLabOrder #ordersSent').trigger('click');
                    $.when(Clinical_LabOrder.LabOrderSearch('0', null, null, null, 'Transmitted')).then(function () {
                        if (ClinicalLabOrderDetail.params.ParentCtrl == "Clinical_LabOrder" && Clinical_LabOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                            if (savelabOrderId.toString().indexOf(',') > -1) {
                                $.each(savelabOrderId.split(','), function (i, item) {
                                    if (item != "") {
                                        $("#pnlClinicalProgressNote #pnlClinicalLabOrder #dgvLabOrdertSent input#" + item).prop("checked", true);
                                        //$("#pnlClinicalProgressNote #pnlClinicalLabOrder #dgvLabOrdertSent input#" + item).trigger('click');
                                        Clinical_LabOrder.enableAddOrder($("#pnlClinicalProgressNote #pnlClinicalLabOrder #dgvLabOrdertSent input#" + item).get(0), null);

                                    }
                                });
                            }
                            else {
                                $("#pnlClinicalProgressNote #pnlClinicalLabOrder #dgvLabOrdertSent input#" + savelabOrderId).prop("checked", true);
                                //$("#pnlClinicalProgressNote #pnlClinicalLabOrder #dgvLabOrdertSent input#" + savelabOrderId).trigger('click');
                                Clinical_LabOrder.enableAddOrder($("#pnlClinicalProgressNote #pnlClinicalLabOrder #dgvLabOrdertSent input#" + savelabOrderId).get(0), null);
                            }

                        }
                    });
                }

            }

        }
        else {

            utility.UnLoadDialog(form, function () {
                if (ClinicalLabOrderDetail.params["ParentCtrl"] == "Clinical_LabOrder") {
                    $('#' + Clinical_LabOrder.params.PanelID + ' #txtOrderNumber').val('');
                    $('#' + Clinical_LabOrder.params.PanelID + ' #ddlLaboratory').val('');
                    $('#' + Clinical_LabOrder.params.PanelID + ' #ddlProvider').val('');
                    ClinicalLabOrderDetail.clearData();
                    UnloadActionPan(ClinicalLabOrderDetail.params["ParentCtrl"], "ClinicalLabOrderDetail", null, ClinicalLabOrderDetail.params["ParentCtrlPanelID"]);
                }
                else {
                    ClinicalLabOrderDetail.clearData();
                    UnloadActionPan(ClinicalLabOrderDetail.params["ParentCtrl"], "ClinicalLabOrderDetail");
                    Clinical_LabOrder.LabOrderSearch(null, null, null, null, "Pending");
                }
                if (ClinicalLabOrderDetail.params.ParentCtrl == "Clinical_Treatment") {
                    Clinical_Treatment.LabOrderSearch(null, null, null, null, "Pending");
                }
                else {
                    Clinical_LabOrder.LabOrderSearch(null, null, null, "LabOrderDetail");
                    Clinical_LabOrder.LabResultsSearch(null, null, null, "LabResultDetail");
                }
            }, function () {
                if (ClinicalLabOrderDetail.params["ParentCtrl"] == "Clinical_LabOrder") {
                    $('#' + Clinical_LabOrder.params.PanelID + ' #txtOrderNumber').val('');
                    $('#' + Clinical_LabOrder.params.PanelID + ' #ddlLaboratory').val('');
                    $('#' + Clinical_LabOrder.params.PanelID + ' #ddlProvider').val('');
                    ClinicalLabOrderDetail.clearData();
                    UnloadActionPan(ClinicalLabOrderDetail.params["ParentCtrl"], "ClinicalLabOrderDetail", null, ClinicalLabOrderDetail.params["ParentCtrlPanelID"]);
                }
                else {
                    ClinicalLabOrderDetail.clearData();
                    UnloadActionPan(ClinicalLabOrderDetail.params["ParentCtrl"], "ClinicalLabOrderDetail");
                }
                if (ClinicalLabOrderDetail.params.ParentCtrl == "Clinical_Treatment") {
                    Clinical_Treatment.LabOrderSearch(null, null, null, null, "Pending");
                }
                else {
                    Clinical_LabOrder.LabOrderSearch(null, null, null, "LabOrderDetail");
                    Clinical_LabOrder.LabResultsSearch(null, null, null, "LabResultDetail");
                }
            });
        }


    },


    clearData: function () {

        $('#FaceSheetPager .slick-track').attr('style', 'opacity: 1; transform: translate3d(-57px, 0px, 0px); width: 1193px;');
        ClinicalLabOrderDetail.loadProviderFromAppData = false;
        ClinicalLabOrderDetail.loadFacilityFromAppData = false;
        ClinicalLabOrderDetail.isProviderLoaded = false;
        ClinicalLabOrderDetail.isFacilityLoaded = false;
        ClinicalLabOrderDetail.OrderedTestsCount = 0;
        ClinicalLabOrderDetail.checkedProblems = [];
        ClinicalLabOrderDetail.CPTCodeQA = [];

    },


    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Saves LabOrder
    LabOrderSave: function (sender, includeSpecimen, UnLoadOrNot) {
        //if (ClinicalLabOrderDetail.SaveModeSet == null) {
        if ($("#donotaskedagain:checked").length > 0) {
            utility.AddKey2LocalStorageForDoNotAskedAgain();
        }
        ClinicalLabOrderDetail.SaveModeSet = false;
        //}
        var FavVal = $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlFavoriteListLabOrder').val();
        var dfd = $.Deferred();
        var breakout = false;
        var missingAnswers = false;
        if (ClinicalLabOrderDetail.ArrayValidation.length > 0) {

            var Test = ClinicalLabOrderDetail.ArrayValidation[0];
            utility.DisplayMessages("AOE (Asked Order Entry) information is missing " + Test.Test, 3);
            return;
        }
        else if (ClinicalLabOrderDetailAOE.UnAnsweredSelections.length > 0) { // Multiselect unanswered
            utility.DisplayMessages("AOE (Asked Order Entry) information is missing ", 3);
            return;
        }
        else {
            var totalAOEs = $('#' + ClinicalLabOrderDetail.params.PanelID + ' #dgvLabOrderDetail tbody tr td .fa-comments').length;
            var answeredAOEs = 0;

            $.each($('#' + ClinicalLabOrderDetail.params.PanelID + ' #dgvLabOrderDetail tbody tr'), function (i, item) {
                var actionstd = $(item).find('td')[1];
                if ($(actionstd).find('a').length > 1 && $($(actionstd).find('a')[1]).find('i').hasClass('fa-comments') == true) {

                    if ($(item).attr('IsAnswered') == "true") {
                        missingAnswers = false;
                        answeredAOEs += 1;
                    }
                }
            });

            if (answeredAOEs == totalAOEs) {
                missingAnswers = false;
            } else {
                missingAnswers = true;
            }
        }
        if (missingAnswers) {
            utility.DisplayMessages("AOE (Asked Order Entry) information is missing ", 3);
            return;
        }
        if ($('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider").val() == '-Select -' || $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider").val() == "" || $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfProvider").val() < 1) {
            utility.DisplayMessages("Please select a Provider.", 2);
            return;
        }
        if ($('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility").val() == '-Select -' || $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility").val() == "" || $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfFacility").val() < 1) {
            utility.DisplayMessages("Please select a facility.", 2);
            return;
        }

        if (ClinicalLabOrderDetail.OrderedTestsCount == 0) {
            utility.DisplayMessages("Please add some tests first", 2);
            return;
        }
        if (!ClinicalLabOrderDetail.IsFavoriteGroupSelected) {
            if ($('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #ddlLabId option:selected").val() == ""
                || $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #ddlLabId option:selected").length == 0) {
                utility.DisplayMessages("Please select a lab to continue.", 2);
                return;
            }
        }
        var LabOrderId = $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfLabOrderId").val() != "" ? $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfLabOrderId").val() : "-1";
        if (parseInt(LabOrderId) > 0) {
            ClinicalLabOrderDetail.params.mode = "Edit";
        }
        else {
            ClinicalLabOrderDetail.params.mode = "Add";
        }

        var self = $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail");

        var title = self.find("#txtLabOrderTitle").val();

        var mainErrorMessage = "";

        if ($('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail #ddlLabOrderAgeCondition').val() != "") {
            var ageConditionMsg = ClinicalLabOrderDetail.isAgeConditionValid();
            if (ageConditionMsg != "") {
                mainErrorMessage = ageConditionMsg;
            }
        }

        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objData = JSON.parse(myJSON);

        var LabOrderProblemList = [];
        $(self).find("#ulProblemLists td").each(function (index, item) {
            if ($(this).find("input:checkbox").prop("checked") && $(this).find("input:checkbox").attr("id") != "chkSelectAll") {
                var objProblem = {
                    ProblemId: $(this).find("input:checkbox").val(),
                    Description: $(this).text()
                };
                objProblem.Description = (objProblem.Description).replace(/%/g, "%25");
                LabOrderProblemList.push(objProblem);
            }
        });
        objData["LabOrderProblemList"] = LabOrderProblemList;

        //Start 06-04-2016 Humaira Yousaf for problems list
        ClinicalLabOrderDetail.LabOrderProblems = LabOrderProblemList;
        //End 06-04-2016 Humaira Yousaf for problems list

        if (sender == 'signorder' || sender == 'signprintorder')
            objData["Status"] = 'Transmitted';
        else if (sender == 'save')
            objData["Status"] = 'Pending';
        else if (sender == 'saveprintorder')
            objData["Status"] = 'Pending';

        if (ClinicalLabOrderDetail.AOEsExists) {
            if (ClinicalLabOrderDetail.CPTCodeQA.length == 0 && ClinicalLabOrderDetail.params.mode != "Edit") {
                utility.DisplayMessages("AOE (Asked Order Entry) information is missing ", 3);
                return;
            }
        }
        if (ClinicalLabOrderDetail.CPTCodeQA.length > 0) {
            objData['CPTCodeQuestionsAnswers'] = ClinicalLabOrderDetail.CPTCodeQA;
        }


        myJSON = JSON.stringify(objData);

        //------------------------------------------------------------
        var LabTestIds = $("#" + ClinicalLabOrderDetail.params.PanelID + " #dgvLabOrderDetail tbody tr:not([id*=Child]").map(function () {
            return this.id.replace("id", "");
        }).get().join(',');
        $("#" + ClinicalLabOrderDetail.params.PanelID + " #pnlLab_ResultOrderDetail #hfLabTestIds").val(LabTestIds);
        var selfLabTest = $("#" + ClinicalLabOrderDetail.params.PanelID + " #pnlLab_ResultOrderDetail");
        var myJSONLabTest = selfLabTest.getMyJSON(null, true);

        var objRad = new Object();
        objRad["LabTestIds"] = LabTestIds;
        JSONToSave = myJSONLabTest;//utility.MergeJSON(myJSON, myJSONLabTest);

        //Start//01-04-2016//Abid Ali//To save child Rows Data in JSON//
        $("#" + ClinicalLabOrderDetail.params.PanelID + " #dgvLabOrderDetail tbody tr:not([id*=Child]").each(function (index, item) {
            var childExists = ClinicalLabOrderDetail.EditableGrid.datatable.row(index).child();

            if (typeof childExists != 'undefined') {
                var childRow = ClinicalLabOrderDetail.EditableGrid.datatable.row(index).child().get();
                if ($(childRow).length > 0) {

                    var childRowDataJson = $(childRow).getMyJSON(null, true);
                    JSONToSave = utility.MergeJSON(JSONToSave, childRowDataJson)
                }
            }
        });
        //End//01-04-2016//Abid Ali//To save child Rows Data in JSON//


        var data = JSON.stringify(objRad);
        //JSONToSave = utility.MergeJSON(data, JSONToSave);
        try {
            JSONToSave = decodeURIComponent(JSONToSave);
        }
        catch (ex) {
            console.log(ex);
        }

        JSONToSave = utility.MergeJSON(myJSON, JSONToSave);
        JSONToSave = utility.MergeJSON(data, JSONToSave);
        //--------------------------------------------------------------

        ClinicalLabOrderDetail.cacheLabOrderData();
        // var isSavedSuccessfully = false;
        if (ClinicalLabOrderDetail.params.mode == "Add") {
            // Favorite Group checks
            JSONToSave = JSON.parse(JSONToSave);
            JSONToSave["IsFavoriteGroupTypeSave"] = ClinicalLabOrderDetail.IsFavoriteGroupSelected;
            JSONToSave["LabIds"] = ClinicalLabOrderDetail.FavoriteGroupTestLabs.join(",");
            JSONToSave = JSON.stringify(JSONToSave);
            var tests = JSON.parse(JSONToSave);
            var isOk = false;
            var sampleStorages = [];
            for (i = 1; i <= ClinicalLabOrderDetail.OrderedTestsCount; i++) {
                if (tests["ddlSampleStorage-" + i] != "") {
                    sampleStorages.push(tests["ddlSampleStorage-" + i]);
                }
            }
            for (var i = 1; i < sampleStorages.length; i++) {
                if (sampleStorages[i] !== sampleStorages[0]) {
                    utility.DisplayMessages('Tests with different specimen storage temprature cannot be placed together in one order', 2);
                    return false;
                }

            }
            AppPrivileges.GetFormPrivileges("Orders and Results_Lab Order", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    ClinicalLabOrderDetail.saveLabOrder(JSONToSave).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {

                            //Save/update favList Status
                            var isFavListOpened = $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #favSectionDiv").hasClass("toggled");
                            $.when(EMRUtility.insertUpdateFavListStatus(ClinicalLabOrderDetail.FavListName, !isFavListOpened)).then(function () {
                                ClinicalLabOrderDetail.SaveFavListVal(FavVal);
                            });

                            ClinicalLabOrderDetail.SavelabOrderId = response.radiologicalOrderId;
                            if (typeof response.radiologicalOrderId == typeof undefined) {
                                ClinicalLabOrderDetail.SavelabOrderId = response.LabOrderIDs.trim();
                            }
                            if (response.InsertType != undefined && response.InsertType == "FavoriteGroup") { // Close the windows if multiple orders were inserted using favorite groups
                                if (sender == 'save') {
                                    ClinicalLabOrderDetail.SaveModeSet = true;
                                    utility.DisplayMessages(response.message, 1);
                                    $('#' + ClinicalLabOrderDetail.params.PanelID + ' #btnPrintLabOrder').removeClass('disableAll');
                                }
                                if (sender == 'signprintorder' || sender == 'saveprintorder' || sender == 'signorder') {
                                    var LabOrderIDs = response.LabOrderIDs;

                                    ClinicalLabOrderDetail.printLabOrder(includeSpecimen, LabOrderIDs);
                                }
                                ClinicalLabOrderDetail.UnLoad('saveExit');
                            }
                            else {

                                ClinicalLabOrderDetail.params.mode = "Edit";
                                if (sender == 'save') {
                                    ClinicalLabOrderDetail.SaveModeSet = true;
                                }


                                if (ClinicalLabOrderDetail.params.ParentCtrl == "clinicalTabProgressNote") {
                                    ClinicalLabOrderDetail.getLatestLabOrderByPatientId(true);
                                }

                                $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfLabOrderId").val(response.radiologicalOrderId);
                                ClinicalLabOrderDetail.params.LabOrderId = response.radiologicalOrderId;
                                $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfOrderNumber").val(response.orderNo);

                                utility.DisplayMessages(response.message, 1);

                                /* optimize */
                                //Clinical_LabOrder.LabOrderSearch(null, null, null, "LabOrderDetail");
                                //Clinical_LabOrder.LabResultsSearch(null, null, null, "LabResultDetail");
                                /* optimize  */
                                // Download HL7 Lab Order Message
                                if (sender == 'signprintorder' || sender == 'signorder') {
                                    var uri = '';//'data:application/vnd.ms-excel;base64,';
                                    if (response.orderNo != "") {
                                        //download(uri + response.HL7MessageContent, response.orderNo + "LabOrder.txt", "application/octet-stream");
                                    } else {
                                        //download(uri + response.HL7MessageContent, response.radiologicalOrderId + "LabOrder.txt", "application/octet-stream");
                                    }
                                }
                                if (sender == 'signprintorder') {
                                    ClinicalLabOrderDetail.params.LabOrderId = response.radiologicalOrderId;

                                    //includeSecimenLabel
                                    ClinicalLabOrderDetail.printLabOrder(includeSpecimen);
                                }
                                if (sender == 'saveprintorder') {
                                    utility.kendoConfirm('Specimen Label Printing', 'Would you like to print the Specimen Label for this order?',
                                    function () {//success
                                        ClinicalLabOrderDetail.printLabOrder(true);

                                    }, function () {//Cancel
                                        ClinicalLabOrderDetail.printLabOrder(false);

                                    }
                                 );
                                }
                                else {
                                    //if (UnLoadOrNot != false) {
                                    //    ClinicalLabOrderDetail.UnLoad('saveExit');
                                    //}

                                    $('#FaceSheetPager .slick-track').attr('style', 'opacity: 1; transform: translate3d(-57px, 0px, 0px); width: 1193px;');
                                }
                                dfd.resolve();
                                if (sender == 'signprintorder') {
                                    ClinicalLabOrderDetail.disableLabOrderControls(true);
                                }

                                if (!includeSpecimen) {
                                    ClinicalLabOrderDetail.loadLabOrder(ClinicalLabOrderDetail.params.LabOrderId);
                                    //  ClinicalLabOrderDetail.UnLoad('saveExit');
                                }

                            }
                            // Clinical_LabOrder.LabOrderSearch(null, null, null, null, "Pending");
                            ClinicalLabOrderDetail.loadLabOrder($('#' + ClinicalLabOrderDetail.params.PanelID + " #hfLabOrderId").val());
                        }
                        else {
                            utility.DisplayMessages(response.message, 3);
                        }
                        setTimeout(function () {
                            $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").data('serialize', $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").serialize());
                        }, 500);
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else if (ClinicalLabOrderDetail.params.mode == "Edit") {

            AppPrivileges.GetFormPrivileges("Orders and Results_Lab Order", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    ClinicalLabOrderDetail.updateLabOrder(JSONToSave, LabOrderId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            if (sender == 'save') {
                                ClinicalLabOrderDetail.SaveModeSet = true;
                            }
                            //Save/update favList Status
                            ClinicalLabOrderDetail.SavelabOrderId = response.radiologicalOrderId;
                            if (typeof response.radiologicalOrderId == typeof undefined) {
                                ClinicalLabOrderDetail.SavelabOrderId = response.LabOrderIDs.trim();
                            }
                            //var isFavListOpened = $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #favSectionDiv").hasClass("toggled");
                            //$.when(EMRUtility.insertUpdateFavListStatus(ClinicalLabOrderDetail.FavListName, !isFavListOpened)).then(function () {
                            //    ClinicalLabOrderDetail.SaveFavListVal(FavVal);
                            //});

                            if (ClinicalLabOrderDetail.params.ParentCtrl == "clinicalTabProgressNote") {
                                ClinicalLabOrderDetail.getLatestLabOrderByPatientId();
                            }

                            $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfLabOrderId").val(response.radiologicalOrderId);
                            $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfOrderNumber").val(response.orderNo);
                            utility.DisplayMessages(response.message, 1);
                            $('#' + Clinical_LabOrder.params.PanelID + ' #txtOrderNumber').val('');
                            $('#' + Clinical_LabOrder.params.PanelID + ' #ddlLaboratory').val('');
                            $('#' + Clinical_LabOrder.params.PanelID + ' #ddlProvider').val('');

                            if ($("#" + Clinical_LabOrder.params.PanelID + " #PatientlistLabOrderPending").hasClass('active')) {

                                $.when(Clinical_LabOrder.LabOrderSearch(null, null, null, "LabOrderDetail")).then(function () {
                                    if (ClinicalLabOrderDetail.params.ParentCtrl == "Clinical_LabOrder" && Clinical_LabOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                                        //$("#pnlClinicalProgressNote #pnlClinicalLabOrder #dgvLabOrdertSent input#" + response.radiologicalOrderId).prop("checked", true);
                                        $("#pnlClinicalProgressNote #pnlClinicalLabOrder #dgvLabOrdertSent input#" + response.radiologicalOrderId).trigger("click");
                                    }
                                });
                            }
                            /* optimize */
                            //Clinical_LabOrder.LabOrderSearch(null, null, null, "LabOrderDetail");
                            //Clinical_LabOrder.LabResultsSearch(null, null, null, "LabResultDetail");
                            /* optimize */

                            if (sender == 'signprintorder') {
                                UnLoadOrNot = false;
                                ClinicalLabOrderDetail.printLabOrder(includeSpecimen);
                                //includeSecimenLabel
                            }
                            if (sender == 'signprintorder') {
                                ClinicalLabOrderDetail.disableLabOrderControls(true);
                            }
                            if (sender == 'saveprintorder') {
                                utility.kendoConfirm('Specimen Label Printing', 'Would you like to print the Specimen Label for this order?',
                                function () {//success
                                    ClinicalLabOrderDetail.printLabOrder(true);

                                }, function () {//Cancel
                                    ClinicalLabOrderDetail.printLabOrder(false);

                                }
                             );
                            }
                            else {
                                //if (UnLoadOrNot != false) {
                                //    ClinicalLabOrderDetail.UnLoad('saveExit');
                                //}
                                $('#FaceSheetPager .slick-track').attr('style', 'opacity: 1; transform: translate3d(-57px, 0px, 0px); width: 1193px;');
                            }
                            ////Start 31-05-2016 Humaira Yousaf for favorite list toggle
                            //if (response.labFavList == true)
                            //    SetGlobalAppData('LabFavListOpened', 'True');
                            //else
                            //    SetGlobalAppData('LabFavListOpened', 'False');
                            ////End 11-04-2016 Humaira Yousaf for favorite list toggle

                            ClinicalLabOrderDetail.loadLabOrder($('#' + ClinicalLabOrderDetail.params.PanelID + " #hfLabOrderId").val());

                            dfd.resolve();
                            //  Clinical_LabOrder.LabOrderSearch(null, null, null, null, "Pending");
                        }
                        else {
                            utility.DisplayMessages(response.message, 3);
                        }
                        setTimeout(function () {
                            $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").data('serialize', $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").serialize());
                        }, 500);
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });

        }

    },
    SaveFavListVal: function (FavListVal) {
        EMRUtility.insertUpdateFavListVal(ClinicalLabOrderDetail.FavListName, FavListVal);
    },
    //Function Name: cacheProcedureOrderData
    //Author Name: Humaira Yousaf
    //Created Date: 06-04-2016
    //Description: Saves procedure order detail data
    cacheLabOrderData: function () {
        Clinical_LabOrder.params["ProviderName"] = $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #txtProvider").val();
        Clinical_LabOrder.params["ProviderId"] = $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #hfProvider").val();

        Clinical_LabOrder.params["AssigneeName"] = $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #ddlAssigneeId").val();
        Clinical_LabOrder.params["AssigneeId"] = $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #hfAssignee").val();

        Clinical_LabOrder.params["Problems"] = ClinicalLabOrderDetail.LabOrderProblems;

        Clinical_LabOrder.params["LabId"] = $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #ddlLabId").val();
        Clinical_LabOrder.params["BillingTypeId"] = $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #ddlBillingTypeId").val();

        Clinical_LabOrder.params["FacilityName"] = $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #txtFacility").val();
        Clinical_LabOrder.params["FacilityId"] = $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #hfFacility").val();


        Clinical_LabOrder.params["CurrentPatientId"] = Clinical_LabOrder.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        Clinical_LabOrder.params["hasData"] = true;
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: To check whether Lab order exists or not
    checkLabOrderExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_laborders').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #PlanNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="LabOrdersComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_laborders title="Lab Orders"  id="clinicalMenu_Orders_Lab" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'Lab\',\'clinicalMenu_Orders_Lab\',' + Clinical_ProgressNote.params.NotesId + ');" title="Lab Orders">Lab Orders</a> ' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'Lab Orders\',\'clinicalMenu_Orders_Lab\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_laborders> </header></li>');
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
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_laborders').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Saves LabOrder
    //Params: LabOrderData
    saveLabOrder: function (LabOrderData, NotUpdateTestValues) {
        var objData = JSON.parse(LabOrderData);
        // objData["PatientId"] = hfPatientId
        objData["commandType"] = "save_LabOrder";

        objData["PrimaryInsuraceId"] = $("#hfPrimaryInsId").val();
        objData["SecondaryInsuraceId"] = $("#hfSecondaryInsId").val();
        objData["TertiaryInsuraceId"] = $("#hfTertiaryInsId").val();

        if (NotUpdateTestValues) {
            objData["UpdateTestValues"] = "0";
        }
        else {
            objData["UpdateTestValues"] = "1";
        }



        if (ClinicalLabOrderDetail.params.ParentCtrlPanelID == "pnlClinicalProgressNote #pnlClinicalLabOrder") {
            objData["NoteId"] = $("#pnlClinicalProgressNote #hfNoteId").val();
        } else {
            objData["NoteId"] = "";
        }
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabOrder", "LabOrder");
    },
    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: To show LabOrder Search in Popup
    openLabOrderAlert: function () {

        if ($(" #mainForm  li#LabOrderAlert span").text() != '' && $('#PatientProfile #hfPatientId').val() != '') {
            BackgroundLoaderShow(true);
            var params = [];


            params["FromAdmin"] = 0;
            //   params["StartupScreen"] = "message";
            LoadActionPan("Clinical_LabOrder", params);
        }
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Loads LabOrder data
    loadLabOrder: function (LabOrderId) {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Orders and Results_Lab Order", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (ClinicalLabOrderDetail.params.mode == "Edit") {
                    $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfLabOrderId").val(LabOrderId);
                    var self = $('#' + ClinicalLabOrderDetail.params.PanelID);

                    ClinicalLabOrderDetail.fillLabOrder(LabOrderId).done(function (response) {
                        if (response != "") {
                            var data = JSON.parse(response);
                            if (data.status != false) {

                                var detailJSON = JSON.parse(data.LabFill_JSON);
                                if (detailJSON.bResultAcknowledged == "True") {
                                    $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #btnAddLabResult").text("View Result");
                                }
                                else if (detailJSON.bResultExists == "True") {
                                    $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #btnAddLabResult").text("Edit Result");
                                }
                                else {
                                    $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #btnAddLabResult").text("Add Result");
                                }

                                var LabOrderDetail = JSON.parse(data.LabOderFill_JSON);

                                LabOrderDetail = LabOrderDetail[0];
                                if (LabOrderDetail != null && LabOrderDetail.OrderDate != null && LabOrderDetail.OrderDate != '') {
                                    function pad(s) { return (s < 10) ? '0' + s : s; }
                                    var d = new Date(LabOrderDetail.OrderDate);
                                    LabOrderDetail.OrderDate = [pad(d.getMonth() + 1), pad(d.getDate()), d.getFullYear()].join('/');
                                }

                                var LabOrderTestDetail = JSON.parse(data.LabOrderTest_JSON);
                                LabOrderTestDetail = LabOrderTestDetail[0];
                                LabOrderDetail.Specimen = LabOrderTestDetail.Specimen;
                                LabOrderDetail.SpecimenSource = LabOrderTestDetail.SpecimenSource;

                                setTimeout(function () {
                                    utility.bindMyJSONByName(true, LabOrderDetail, false, self).done(function () {

                                        $('#pnlClinicalLabOrder #frmClinicalLabOrder #txtOrderNumber').val("");
                                        $('#pnlClinicalLabOrder #frmClinicalLabOrder #ddlLaboratory').val("");
                                        $('#pnlClinicalLabOrder #frmClinicalLabOrder #ddlProvider').val("");

                                        $('#pnlClinicalLabOrder #frmClinicalLabResult #txtOrderNumber').val("");
                                        $('#pnlClinicalLabOrder #frmClinicalLabResult #ddlLaboratory').val("");
                                        $('#pnlClinicalLabOrder #frmClinicalLabResult #ddlProvider').val("");


                                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtLabOrderNo").val(LabOrderDetail.OrderNo);
                                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfOrderNumber").val(LabOrderDetail.OrderNo);
                                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #ddlLabId").val(LabOrderDetail.LabId);

                                        /*********/
                                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider").val(LabOrderDetail.Provider);
                                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider").attr("Provider", LabOrderDetail.ProviderId);
                                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfProvider").val(LabOrderDetail.ProviderId);

                                        utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider"), LabOrderDetail.Provider, $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfProvider"), LabOrderDetail.ProviderId);

                                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility").val(LabOrderDetail.Facility);
                                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility").attr("Facility", Clinical_ProgressNote.params["NotesFacilityIDForFollowUp"]);
                                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfFacility").val(LabOrderDetail.FacilityId);

                                        utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility"), LabOrderDetail.Facility, $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfFacility"), LabOrderDetail.FacilityId);
                                        /*********/
                                        ClinicalLabOrderDetail.EnableDisableTestSearch($('#' + ClinicalLabOrderDetail.params.PanelID + " #ddlLabId")).done(function () {

                                            var billingType = $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlBillingTypeId option:selected').text();
                                            if (billingType.indexOf('Insurance') >= 0) {
                                                ClinicalLabOrderDetail.enableDisableInsurances(false);

                                            }
                                            else {
                                                ClinicalLabOrderDetail.enableDisableInsurances(false);
                                            }
                                            setTimeout(function () {
                                                ClinicalLabOrderDetail.LabOrderTestGridLoad(response);
                                            }, 500);

                                            $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").data('serialize', $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").serialize());
                                        });
                                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").data('serialize', $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").serialize());
                                    });
                                }, 1000);


                                //Load Problems
                                if (data.LabOrderProblems_JSON != null) {

                                    data.LabOrderProblems_JSON = JSON.parse(data.LabOrderProblems_JSON);

                                    for (var index in data.LabOrderProblems_JSON) {
                                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #ulProblemLists td #chk" + data.LabOrderProblems_JSON[index].ProblemId).attr("checked", "checked");
                                    }
                                    ClinicalLabOrderDetail.checkOrUncheckSelectAll();
                                }

                                if (ClinicalLabOrderDetail.params.mode == "Edit") {
                                    if (LabOrderDetail.Status.toLowerCase() == "transmitted") {
                                        ClinicalLabOrderDetail.disableLabOrderControls(true);
                                    }
                                    else {
                                        ClinicalLabOrderDetail.disableLabOrderControls(false);
                                    }
                                }
                                else {
                                    ClinicalLabOrderDetail.disableLabOrderControls(false);
                                }

                            }
                        }
                        if (ClinicalLabOrderDetail.params.mode == "Add") {
                            ClinicalLabOrderDetail.disableLabOrderControls(false);
                        }
                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").data('serialize', $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").serialize());
                    });
                }
                else {
                    ClinicalLabOrderDetail.populateLabOrderSavedData();
                }


            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    //Function Name: populateLabOrderSavedData
    //Author Name: Humaira Yousaf
    //Created Date: 06-04-2016
    //Description: Gets Lab order detail data
    populateLabOrderSavedData: function () {
        if (Clinical_LabOrder.params["hasData"] == true) {
            if (Clinical_LabOrder.params["ParentCtrl"] != "clinicalTabProgressNote") {
                utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider"), globalAppdata.DefaultProviderName, $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfProvider"), globalAppdata.DefaultProviderId);
            }


            $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #ddlAssigneeId").val(Clinical_LabOrder.params["AssigneeName"]);
            $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #hfAssignee").val(Clinical_LabOrder.params["AssigneeId"]);

            //if (Clinical_LabOrder.params["Problems"] != null) {
            //    $(Clinical_LabOrder.params["Problems"]).each(function (index, item) {
            //        $('#' + ClinicalLabOrderDetail.params.PanelID + " #ulProblemLists td #chk" + item.ProblemId).attr("checked", "checked");
            //    });
            //}

            $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #ddlLabId").val(Clinical_LabOrder.params["LabId"]);
            ClinicalLabOrderDetail.EnableDisableTestSearch($('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #ddlLabId"));

            $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #ddlBillingTypeId").val(Clinical_LabOrder.params["BillingTypeId"]);

            //Start//Abid Ali/ For bug# EMR-751
            var billingType = $('#' + ClinicalLabOrderDetail.params.PanelID + '  #frmClinicalLabOrderDetail #ddlBillingTypeId option:selected').text();
            if (billingType.indexOf('Insurance') >= 0) {
                ClinicalLabOrderDetail.enableDisableInsurances(false);
            }
            else {
                $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlPrimaryInsuraceId').val('');
                $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlSecondaryInsuraceId').val('');
                $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlTertiaryInsuraceId').val('');
                $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtGuarantor").val("");
                $('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail #txtLabRelationShipId').val("");

                ClinicalLabOrderDetail.enableDisableInsurances(false);
            }
            //End//Abid Ali/ For bug# EMR-751
            if (Clinical_LabOrder.params["ParentCtrl"] != "clinicalTabProgressNote") {
                utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #txtFacility"), globalAppdata.DefaultFacilityName, $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #hfFacility"), globalAppdata.DefaultFacilityId);
                ClinicalLabOrderDetail.PoupulateLoction();
            }


        }



        if (Clinical_LabOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
            $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility").attr("Facility", Clinical_ProgressNote.params["NotesFacilityIDForFollowUp"]);
            utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility"), Clinical_ProgressNote.params.NotesFacilityName, $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfFacility"), Clinical_ProgressNote.params.NotesFacilityIDForFollowUp);
            //if (ClinicalLabOrderDetail.loadProviderFromAppData && ClinicalLabOrderDetail.ProviderOnDemandLoad) {
            CacheManager.BindCodes('GetProvider', false).done(function (result) {
                var Ctrl = $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider");
                var hfCtrl = $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfProvider");
                var onSelect = function (dataItem) { $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider").attr("Provider", dataItem.id) }
                var onChange = function (valid) { ClinicalLabOrderDetail.EnableDisableTestSearch(); }
                utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, onSelect, onChange);

                $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider").val(Clinical_ProgressNote.params["CurrentNotesProviderText"]);
                $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider").attr("Provider", Clinical_ProgressNote.params["CurrentNotesProviderId"]);
                $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfProvider").val(Clinical_ProgressNote.params["CurrentNotesProviderId"]);
                utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider"), Clinical_ProgressNote.params.CurrentNotesProviderText, $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfProvider"), Clinical_ProgressNote.params.CurrentNotesProviderId);
            });
            //ClinicalLabOrderDetail.loadProviderFromAppData = true;
            //ClinicalLabOrderDetail.ProviderOnDemandLoad = false;
            //}
            //if (ClinicalLabOrderDetail.loadFacilityFromAppData && ClinicalLabOrderDetail.FacilityOnDemandLoaded) {
            CacheManager.BindCodes('GetFacility', false).done(function (result) {
                var Ctrl = $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility");
                var hfCtrl = $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfFacility");
                var onSelect = function (dataItem) { $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility").attr("Facility", dataItem.id); }
                var onChange = function (valid) { if (valid) ClinicalLabOrderDetail.PoupulateLoction(); }
                utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl, onSelect, onChange);

                $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility").val(Clinical_ProgressNote.params["NotesFacilityName"]);
                $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility").attr("Facility", Clinical_ProgressNote.params["NotesFacilityIDForFollowUp"]);
                $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfFacility").val(Clinical_ProgressNote.params["NotesFacilityIDForFollowUp"]);
                ClinicalLabOrderDetail.PoupulateLoction();
                utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility"), Clinical_ProgressNote.params.NotesFacilityName, $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfFacility"), Clinical_ProgressNote.params.NotesFacilityIDForFollowUp);
            });
            //ClinicalLabOrderDetail.loadProviderFromAppData = true;
            //ClinicalLabOrderDetail.ProviderOnDemandLoad = false;
            //}
        }
        else {
            if ($('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider").val() == "") {
                $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider").val(globalAppdata.DefaultProviderName);
                $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider").attr("Provider", globalAppdata.DefaultProviderId);
                $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfProvider").val(globalAppdata.DefaultProviderId);
                utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider"), globalAppdata.DefaultProviderName, $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfProvider"), globalAppdata.DefaultProviderId);
                ClinicalLabOrderDetail.loadProviderFromAppData = true;
            }
            if ($('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility").val() == "") {
                $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility").val(globalAppdata.DefaultFacilityName);
                $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility").attr("Facility", globalAppdata.DefaultFacilityId);
                $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfFacility").val(globalAppdata.DefaultFacilityId);
                utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility"), globalAppdata.DefaultFacilityName, $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfFacility"), globalAppdata.DefaultFacilityId);
                ClinicalLabOrderDetail.loadFacilityFromAppData = true;
                ClinicalLabOrderDetail.PoupulateLoction();
            }
            if (ClinicalLabOrderDetail.loadProviderFromAppData && ClinicalLabOrderDetail.ProviderOnDemandLoad) {
                if (globalAppdata.DefaultProviderName != "" && globalAppdata.DefaultProviderName != "- Select -") {
                    CacheManager.BindCodes('GetProvider', false).done(function (result) {
                        var Ctrl = $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider");
                        var hfCtrl = $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfProvider");
                        var onSelect = function (dataItem) { $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider").attr("Provider", dataItem.id) }
                        var onChange = function (valid) { ClinicalLabOrderDetail.EnableDisableTestSearch(); }
                        utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, onSelect, onChange);

                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider").val(globalAppdata.DefaultProviderName);
                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider").attr("Provider", globalAppdata.DefaultProviderId);
                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfProvider").val(globalAppdata.DefaultProviderId);
                        utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider"), globalAppdata.DefaultProviderName, $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfProvider"), globalAppdata.DefaultProviderId);
                    });
                    ClinicalLabOrderDetail.loadProviderFromAppData = true;
                    ClinicalLabOrderDetail.ProviderOnDemandLoad = false;
                }
                else {
                    CacheManager.BindCodes('GetProvider', false).done(function (Providers) {
                        $("#frmClinicalLabOrderDetail #txtProvider").autocomplete({
                            autoFocus: true,
                            source: Providers,
                            select: function (event, ui) {

                                setTimeout(function () {
                                    $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider").attr("Provider", "");
                                    $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfProvider").val("");
                                }, 100);
                            },
                            change: function (event, ui) {
                                ClinicalLabOrderDetail.EnableDisableTestSearch();
                            }
                        });
                        if ($('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider").val() == "") {
                            $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider").val("");
                            $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtProvider").attr("Provider", "");
                            $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfProvider").val("");
                        }
                    });
                    ClinicalLabOrderDetail.loadProviderFromAppData = true;
                    ClinicalLabOrderDetail.ProviderOnDemandLoad = false;
                }
            }

            if (ClinicalLabOrderDetail.loadFacilityFromAppData && ClinicalLabOrderDetail.FacilityOnDemandLoad) {
                if (globalAppdata.DefaultFacilityName != "" && globalAppdata.DefaultFacilityName != "- Select -") {
                    CacheManager.BindCodes('GetFacility', false).done(function (result) {
                        var Ctrl = $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility");
                        var hfCtrl = $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfFacility");
                        var onSelect = function (dataItem) { $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility").attr("Facility", dataItem.id); }
                        utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl, onSelect);

                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility").val(globalAppdata.DefaultFacilityName);
                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility").attr("Facility", globalAppdata.DefaultFacilityId);
                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfFacility").val(globalAppdata.DefaultFacilityId);
                        utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility"), globalAppdata.DefaultFacilityName, $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfFacility"), globalAppdata.DefaultFacilityId);
                    });
                    ClinicalLabOrderDetail.loadFacilityFromAppData = true;
                    ClinicalLabOrderDetail.FacilityOnDemandLoad = false;
                    ClinicalLabOrderDetail.PoupulateLoction();
                } else {
                    CacheManager.BindCodes('GetFacility', false).done(function (result) {
                        $("#frmClinicalLabOrderDetail #txtFacility").autocomplete({
                            autoFocus: true,
                            source: Facilities,
                            select: function (event, ui) {

                                setTimeout(function () {
                                    $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility").attr("Facility", ui.item.id);
                                    $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfFacility").val(ui.item.id);
                                    ClinicalLabOrderDetail.PoupulateLoction();
                                }, 100);
                            }

                        });
                        if ($('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility").val() == "") {
                            $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility").val("");
                            $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtFacility").attr("Facility", "");
                            $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfFacility").val("");
                            ClinicalLabOrderDetail.PoupulateLoction();
                        }
                    });
                    ClinicalLabOrderDetail.loadFacilityFromAppData = true;
                    ClinicalLabOrderDetail.FacilityOnDemandLoad = false;

                }
            }
        }
        ClinicalLabOrderDetail.EnableDisableTestSearch();
    },
    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Fills LabOrder
    //Params: LabOrderId
    fillLabOrder: function (LabOrderId) {

        var objData = {};
        objData["commandType"] = "fill_LabOrder";
        objData["LabOrderId"] = LabOrderId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabOrder", "LabOrder");
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Updates LabOrder
    //Params: LabOrderData, LabOrderId
    updateLabOrder: function (LabOrderData, LabOrderId) {

        var objData = JSON.parse(LabOrderData);
        objData["LabOrderId"] = LabOrderId;
        objData["commandType"] = "save_LabOrder";
        objData["UpdateTestValues"] = "1";
        objData["PrimaryInsuraceId"] = $("#hfPrimaryInsId").val();
        objData["SecondaryInsuraceId"] = $("#hfSecondaryInsId").val();
        objData["TertiaryInsuraceId"] = $("#hfTertiaryInsId").val();

        if (ClinicalLabOrderDetail.params.ParentCtrlPanelID == "pnlClinicalProgressNote #pnlClinicalLabOrder") {
            objData["NoteId"] = $("#pnlClinicalProgressNote #hfNoteId").val();
        } else {
            objData["NoteId"] = "";
        }

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabOrder", "LabOrder");

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Shows respective cotrols based on selected age condition
    addAgeConditionValues: function () {
        var ageCondition = $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #ddlLabOrderAgeCondition").val();
        //Start 1-03-2016 Humaira Yousaf
        if (ageCondition == "") {
            $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #ageConditionRange").addClass("hidden");
            $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #ageConditionValue").addClass("hidden");
            var ageToValue = $('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail input[id*=txtLabOrderAgeValue]').val('');
            var ageToValue = $('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail input[id*=txtLabOrderAgeFrom]').val('');
            var ageToValue = $('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail input[id*=txtLabOrderAgeTo]').val('');
        }
            //End 1-03-2016 Humaira Yousaf
        else if (ageCondition == "6") {
            $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #ageConditionRange").removeClass("hidden");
            $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #ageConditionValue").addClass("hidden");
        }
        else {
            $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #ageConditionValue").removeClass("hidden");
            $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #ageConditionRange").addClass("hidden");
        }
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Validates reminder
    isReminderValid: function () {
        var Message = "";

        var reminder = $('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail input[id*=txtLabOrderReminderLength]');
        var stayLength = $(reminder).val();
        var ddlVal = $('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail #ddlLabOrderReminderPeriod').val();
        if (stayLength != null && stayLength != '') {
            if (ddlVal == null || ddlVal == '') {
                $('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail #ddlLabOrderReminderPeriod').focus();
                Message = "Please select Reminder Period.";
            }
        }

        if (ddlVal != null && ddlVal != '') {
            if (stayLength == null || stayLength == '') {
                $(reminder).focus();
                Message = "Please enter Reminder Period.";
            }
        }

        return Message;
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: HideShow Vitals,problems list etc divs on editing
    hideShowDataDivs: function () {
        var selectedValue = $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #ddlLabOrderRuleType option:selected");
        var selected = [];
        $(selectedValue).each(function (index, val) {
            selected.push($(this).text());
        });
        var unSelect = '';
        $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail .comList").addClass('hidden');
        $(selected).each(function (i, item) {
            var sectionName = item;
            unSelect += sectionName + ',';
            $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #dv" + item.replace(/\s/g, '')).removeClass('hidden');
        });
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: to show LabOrder Alert on Dashboard
    showLabOrderAlert: function (triggerLocation) {

        ClinicalLabOrderDetail.showLabOrderAlertDBCall(triggerLocation).done(function (response) {
            response = JSON.parse(response);
            //Start//09-03-2016//Ahmad Raza//setting hiddenField values
            $(" #mainForm  li#LabOrderAlert input").val('');
            $(" #mainForm  li#LabOrderAlert input").val(function (i, val) {
                return val + (val ? ', ' : '') + response.LabOrderIDs;
            });
            //End//09-03-2016//Ahmad Raza//setting hiddenField values
            if (response.status != false) {

                if (response.alertCount > 0) {
                    $(" #mainForm  li#LabOrderAlert span").text(response.alertCount);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: DBCall to show LabOrder Alert on Dashboard
    showLabOrderAlertDBCall: function (triggerLocation) {
        var objData = new Object();
        if (triggerLocation == 'FaceSheet') {
            objData["LabOrderTriggerLocation"] = '1';
        }
        else if (triggerLocation == 'Notes') {
            objData["LabOrderTriggerLocation"] = '2';
        }
        objData["PatientId"] = Clinical_LabOrder.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        objData["commandType"] = "show_LabOrder_alert";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabOrder", "LabOrder");
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Loads LabOrder Medications
    //Params: LabOrderMedications
    loadLabOrderMedications: function (LabOrderMedications) {
        $(LabOrderMedications).each(function (index, item) {

            var li = '';

            if (index == 0) {
                li = "<li id=" + item.drugId + " onclick='' \"><div><a href='#'>" + item.medicationName + "<span class='removeIconListHover' onclick='ClinicalLabOrderDetail.deleteMedication($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }
            else {
                li = "<li id=" + item.drugId + " onclick='' \"><div><select id='ddlMedications" + item.drugId + "' name = 'LabOrderMedications" + item.drugId + "' class='form-control'><option value='AND'>AND</option><option value='OR'>OR</option></select></div><div><a href='#'>" + item.medicationName + "<span class='removeIconListHover' onclick='ClinicalLabOrderDetail.deleteMedication($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }

            $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #ulLabOrderMedications").append(li);

            if (index != 0) {
                if (item.medicationOperator == 'AND') {
                    $($('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #ulLabOrderMedications li#" + item.drugId).find("#ddlMedications" + item.drugId + " option")[0]).attr('selected', true);
                }
                else {
                    $($('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #ulLabOrderMedications li#" + item.drugId).find("#ddlMedications" + item.drugId + " option")[1]).attr('selected', true);
                }
            }
            $('.modal-backdrop').removeClass('in');
            $('.modal-backdrop').addClass('out');
            $('.modal-backdrop').hide();
            $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #txtLabOrderMedications").val('');
        });
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Loads LabOrder Allergies
    //Params: LabOrderAllergies
    loadLabOrderAllergies: function (LabOrderAllergies) {
        $(LabOrderAllergies).each(function (index, item) {

            var li = '';

            if (index == 0) {
                li = "<li id=" + item.AllergyId + " onclick='' \"><div ><a href='#'>" + item.Allergen + "<span class='removeIconListHover' onclick='ClinicalLabOrderDetail.deleteAllergy($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }
            else {
                li = "<li id=" + item.AllergyId + " onclick='' \"><div><select id='ddlAllergies" + item.AllergyId + "' name = 'LabOrderMedications" + item.AllergyId + "' class='form-control'><option value='AND'>AND</option><option value='OR'>OR</option></select></div><div><a href='#'>" + item.Allergen + "<span class='removeIconListHover' onclick='ClinicalLabOrderDetail.deleteAllergy($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }

            $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #ulLabOrderAllergies").append(li);

            if (index != 0) {
                if (item.AllergyOperator == 'AND') {
                    $($('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #ulLabOrderAllergies li#" + item.AllergyId).find("#ddlAllergies" + item.AllergyId + " option")[0]).attr('selected', true);
                }
                else {
                    $($('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #ulLabOrderAllergies li#" + item.AllergyId).find("#ddlAllergies" + item.AllergyId + " option")[1]).attr('selected', true);
                }
            }
            $('.modal-backdrop').removeClass('in');
            $('.modal-backdrop').addClass('out');
            $('.modal-backdrop').hide();
            $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #txtLabOrderAllergies").val('');
        });
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Params: LabOrderProblems
    loadLabOrderProblems: function (LabOrderProblems) {
        $(LabOrderProblems).each(function (index, item) {

            var li = '';

            if (index == 0) {
                li = "<li name='" + item.Problem + "' onclick='' \"><div ><a href='#'>" + item.Problem + "<span class='removeIconListHover' onclick='ClinicalLabOrderDetail.deleteProblemList($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }
            else {
                li = "<li name='" + item.Problem + "' onclick='' \"><div><select id='ddlProblemList" + item.Problem + "' name = 'LabOrderProblemList" + item.ProblemId + "' class='form-control'><option value='AND'>AND</option><option value='OR'>OR</option></select></div><div><a href='#'>" + item.Problem + "<span class='removeIconListHover' onclick='ClinicalLabOrderDetail.deleteProblemList($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }

            $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #ulLabOrderProblemList").append(li);

            if (index != 0) {
                if (item.ProblemOperator == 'AND') {
                    $($('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #ulLabOrderProblemList li#" + item.ProblemId).find("#ddlProblemList" + item.ProblemId + " option")[0]).attr('selected', true);
                }
                else {
                    $($('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #ulLabOrderProblemList li#" + item.ProblemId).find("#ddlProblemList" + item.ProblemId + " option")[1]).attr('selected', true);
                }
            }
            $('.modal-backdrop').removeClass('in');
            $('.modal-backdrop').addClass('out');
            $('.modal-backdrop').hide();
            $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #txtLabOrderProblemList").val('');
        });
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Validates Age Condition
    isAgeConditionValid: function () {
        var Message = "";
        var ddlAgeConditionVal = $('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail #ddlLabOrderAgeCondition').val();

        if (ddlAgeConditionVal == '6') {

            var ageFromValue = $('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail input[id*=txtLabOrderAgeFrom]').val();
            var ageToValue = $('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail input[id*=txtLabOrderAgeTo]').val();

            if ((ageFromValue == null || ageFromValue == '') && (ageToValue == null || ageToValue == '')) {
                $('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail input[id*=txtLabOrderAgeFrom]').focus();
                $('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail input[id*=txtLabOrderAgeTo]').focus();
                Message = "Please enter From Age and To Age.";

            }
            else {
                if (ageFromValue != null || ageFromValue != '') {
                    if (ageToValue == null || ageToValue == '') {
                        $('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail input[id*=txtLabOrderAgeTo]').focus();
                        Message = "Please enter To Age.";
                    }
                }

                if (ageToValue != null || ageToValue != '') {
                    if (ageFromValue == null || ageFromValue == '') {
                        $('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail input[id*=txtLabOrderAgeFrom]').focus();
                        Message = "Please enter From Age.";
                    }
                }
            }

        }
        else {
            var ageValue = $('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail input[id*=txtLabOrderAgeValue]').val();
            if (ageValue == null || ageValue == '') {
                $('#' + ClinicalLabOrderDetail.params.PanelID + ' #frmClinicalLabOrderDetail input[id*=txtLabOrderAgeValue]').focus();
                Message = "Please enter Age.";
            }
        }

        return Message;
    },



    bindAutoComplete: function (element) {
        //var hiddenCrtl = $(element);
        // utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "ClinicalLabOrderDetail", null, true);

        var CodeSystemType = $('#pnlClinicalLabOrderDetail #ddlLabId option:selected').attr('CodeSystem');
        var labId = $('#pnlClinicalLabOrderDetail #ddlLabId').val();
        EMRUtility.BindLOINCCodes(element, "ClinicalLabOrderDetail", labId, '', CodeSystemType);

    },

    openCPTCode: function () {
        var params = [];
        //params["FromAdmin"] = "0";
        //params["ParentCtrl"] = "ClinicalLabOrderDetail";
        //params["RefHiddenCtrl"] = "hfCPTCode";
        //params["RefCtrl"] = "txtCPTCode";
        params["ParentCtrlPanelID"] = ClinicalLabOrderDetail.params.PanelID;
        //LoadActionPan('Admin_IMOCPT', params, ClinicalLabOrderDetail.params.PanelID);
        params["FromAdmin"] = ClinicalLabOrderDetail.params["FromAdmin"];
        params["ParentCtrl"] = 'ClinicalLabOrderDetail';

        LoadActionPan('Clinical_LOINC', params, ClinicalLabOrderDetail.params.PanelID);
    },


    //Called from LOINIC Control to pass code and description as Json obj
    pushLOINCAsCpt: function (JsonObj) {


        var observation = JsonObj["Observation"];
        var LOINCCOde = JsonObj['LOINICCODE'];
        var LOINCDescription = JsonObj['LOINICDescription'];
        var SampleStorage = JsonObj['SampleStorage'];

        if (LOINCDescription.trim().indexOf('-') == 0) {
            LOINCDescription = LOINCDescription.trim().substr(1).trim();
        }

        ClinicalLabOrderDetail.BindLabOrderGridItem(null,LOINCCOde, LOINCDescription, LOINCDescription, SampleStorage);


    },

    BindLabOrderGridItem: function (modifier, cptCode, procedureDescription, cptDescription, SampleStorage, LabId, IsFavoriteGroup) {
        var labName = $('#pnlClinicalLabOrderDetail #ddlLabId option:selected').text();

        if (labName == "VitalAxis") {
            if (ClinicalLabOrderDetail.OrderedTestsCount >= 1) {// VitalAxis only accepts a single test in order

                if ((cptCode != '84154' || cptCode != '84403' || cptCode != '84153' || cptCode != '84439' || cptCode != '84443') && (ClinicalLabOrderDetail.IsVitalAxisCPT == false)) {
                    utility.DisplayMessages("The lab selected currently processes only one Test per Order.", 2);
                    return;
                }
            }
            if ((cptCode == '84154' || cptCode == '84403' || cptCode == '84153' || cptCode == '84439' || cptCode == '84443')) {
                ClinicalLabOrderDetail.IsVitalAxisCPT = true;
            }
            else {
                ClinicalLabOrderDetail.IsVitalAxisCPT = false;
            }
            if (!ClinicalLabOrderDetail.IsVitalAxisCPT && ClinicalLabOrderDetail.OrderedTestsCount >= 1) {
                utility.DisplayMessages("This combination of Tests cannot be ordered to VitalAxis.", 2);
                return;
            }
        }

        var currentRow = $("#" + ClinicalLabOrderDetail.params.PanelID + " #dgvLabOrderDetail tbody tr");
        var isTestAlreadySelected = false;
        $.each(currentRow, function (i, item) {
            var currentRowCPTDescription = $(item).find("input[id*='Procedure']").val() != null ? $(item).find("input[id*='Procedure']").val().replace(cptCode, "").trim() : "";
            if (cptDescription.toLowerCase().replace(/\-/gi, '').trim() == currentRowCPTDescription.toLowerCase().replace(/\-/gi, '').trim()) {
                isTestAlreadySelected = true;
                return;
            }
        });

        if (isTestAlreadySelected != true) {
            if (IsFavoriteGroup) {
                ClinicalLabOrderDetail.AddNewLabRow(modifier,null, null, null, cptCode, procedureDescription, cptDescription, null, SampleStorage, LabId);
            }
            else {
                ClinicalLabOrderDetail.AddNewLabRow(modifier,null, null, null, cptCode, procedureDescription, cptDescription, null, SampleStorage);
            }

            setTimeout(function () {
                $("#" + ClinicalLabOrderDetail.params.PanelID + " #txtLabCPTCode").val('');
            }, 200);
            if (IsFavoriteGroup) {
                ClinicalLabOrderDetail.IsFavoriteGroupSelected = true;
                if ($.inArray(LabId, ClinicalLabOrderDetail.FavoriteGroupTestLabs) == -1) {
                    ClinicalLabOrderDetail.FavoriteGroupTestLabs.push(LabId);
                }
            }
            else {
                ClinicalLabOrderDetail.IsFavoriteGroupSelected = false;
                $('#' + ClinicalLabOrderDetail.params.PanelID + ' #btnPrintLabOrder').removeClass('disableAll');
            }
        }
        else {
            utility.DisplayMessages("Test is already selected", 2);
        }

        //Start 05-04-2016 Humaira Yousaf to enable/disable action buttons
        if ($("#" + ClinicalLabOrderDetail.params.PanelID + " #dgvLabOrderDetail tbody tr").length > 0 && $("#" + ClinicalLabOrderDetail.params.PanelID + " #dgvLabOrderDetail tbody tr").find('.dataTables_empty').length == 0) {
            $("#" + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #btnSaveOrder,#btnSignPrintOrder,#btnSignOrder,#btnPrintLabOrder, #btnResetLabOrder").removeClass('disableAll');
        }
        else {
            $("#" + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #btnSaveOrder,#btnSignPrintOrder,#btnSignOrder,#btnPrintLabOrder, #btnResetLabOrder").addClass('disableAll');
        }
        //End 05-04-2016 Humaira Yousaf to enable/disable action buttons
        ClinicalLabOrderDetail.OrderedTestsCount++;
        if (ClinicalLabOrderDetail.OrderedTestsCount > 0) {
            $('#pnlClinicalLabOrderDetail #ddlLabId').prop('disabled', true);
        }
        if (IsFavoriteGroup) {
            $('#' + ClinicalLabOrderDetail.params.PanelID + ' #btnPrintLabOrder').addClass('disableAll');
        }

    },

    buildRowChild: function (obj, ParentRowId, SampleStorage) {
        if (!ParentRowId) {
            ParentRowId = "";
        }
        var ChildHTML = $("<div class='positionInitial col-xs-12 p-none pb-default pt-default' style='position:initial !important;'></div>");

        var ddlSpecimen = "<div class='col-sm-4'><label class='control-label'>Prefer Specimen</label><select id='Specimen" + ParentRowId + "' name='Specimen' onchange='ClinicalLabOrderDetail.populateSpecimenSource(this);' class='form-control' ></select></div>";

        var ddlSpecimenSource = "<div class='col-sm-4'><label class='control-label'>Specimen Source</label><select id='ddlSpecimenSource" + ParentRowId + "' name='SpecimenSource' class='form-control' ></select></div>";

        var txtSampleStorage = '';

        if (SampleStorage != null)
            txtSampleStorage = "<div class='col-sm-4'><label class='control-label'>Sample Storage</label><select id='ddlSampleStorage" + ParentRowId + "' name='SampleStorage' class='form-control'><option value='' >- Select -</option><option value='" + SampleStorage + "'>" + SampleStorage + "</option></select></div>";
        else
            txtSampleStorage = "<div class='col-sm-4'><label class='control-label'>Sample Storage</label><select id='ddlSampleStorage" + ParentRowId + "' name='SampleStorage' class='form-control'><option value='' >- Select -</option></select></div>";

        var ddlOrganism = "<div class='col-sm-4'><label class='control-label'>Organism</label><select id='Organism" + ParentRowId + "' name='Organism' ddlist='GetOrganism' onchange='ClinicalLabOrderDetail.populateAntimicrobial(this);' class='form-control' ></select></div>";
        var ddlAntimicrobials = "<div class='col-sm-4'><label class='control-label'>Antimicrobials</label><select id='Antimicrobials" + ParentRowId + "' multiple='multiple' " + "' name='Antimicrobials'  class='form-control' ></select></div>";

        var ddlAlternativeSpecimen = "<div class='col-sm-4'><label class='control-label'>Alternative Specimen</label><select id='AlternativeSpecimen" + ParentRowId + "' name='AlternativeSpecimen' onchange='ClinicalLabOrderDetail.populateSpecimenSource(this);' class='form-control'></select></div>";
        var ddlAlternativeSpecimenSource = "<div class='col-sm-4'><label class='control-label'>Alternative Specimen Source</label><select id='AlternativeSpecimenSource" + ParentRowId + "' name='AlternativeSpecimenSource' class='form-control' ></select></div>";
        var Volume = "<div class='col-sm-4'><label class='control-label'>Volume</label><input type='number'  class='form-control' onkeydown='ClinicalLabOrderDetail.validateVolume(event,this)' id='VolumeText" + ParentRowId + "' name='VolumeText'></input></div>";

        //var Volume = "<div class='col-sm-8 p-none'><label class='control-label col-xs-12'>Volume</label><div class='col-sm-6'><input type='number'  class='form-control' onkeydown='ClinicalLabOrderDetail.validateVolume(event,this)' id='VolumeText" + ParentRowId + "' name='VolumeText'></input></div>";

        //var ddlVolume = "<div class='col-sm-6'><input type='text'  class='form-control'  id='VolumeDDL" + ParentRowId + "'' name='VolumeDDL'></input></div></div>";
        var ddlVolume = "<div class='col-sm-4'><label class='control-label' maxlength='20' >Unit</label><input type='text'  class='form-control' maxlength = '20'  id='VolumeDDL" + ParentRowId + "'' name='VolumeDDL'></input</div>";

        Volume += ddlVolume;
        // Faizan ameen.

        //
        var PatientInstructions = "<div class='col-sm-4'><label class='control-label'>Patient Instructions</label><input type='text' onkeypress='ClinicalLabOrderDetail.validateSpecialCharacters(event)' class='form-control' maxlength = '500'   id='PatientInstructions" + ParentRowId + "'  name='PatientInstructions'></input></div>";

        var FillerInstructions = "<div class='col-sm-4'><label class='control-label'>Filler Instructions</label><input type='text' onkeypress='ClinicalLabOrderDetail.validateSpecialCharacters(event)' class='form-control'  maxlength = '500' id='FillerInstructions" + ParentRowId + "' name='FillerInstructions'></input></div>";

        var ddlTestType = "<div class='col-sm-4'><label class='control-label'>Test Type</label><select id='TestTypeId" + ParentRowId + "' name='TestTypeId' class='form-control' ></select></div>";


        if (globalAppdata["isTransPubHealthAgAntimicobialUse"] && globalAppdata["isTransPubHealthAgAntimicobialUse"].toLowerCase() == "false")
        { ddlSpecimenSource = ""; ddlOrganism = ""; ddlAlternativeSpecimenSource = ""; ddlAntimicrobials = ""; }

        ChildHTML.append(ddlSpecimen, ddlSpecimenSource, ddlAlternativeSpecimen, ddlAlternativeSpecimenSource, txtSampleStorage, ddlOrganism, ddlAntimicrobials, Volume, PatientInstructions, FillerInstructions, ddlTestType);

        return ChildHTML;

    },
    //Author: Ahmad Raza
    //Date :  21-06-2016
    //Function Name: validateSpecialCharacters
    //Description: This function will validate the special characters
    validateSpecialCharacters: function (event) {
        var valid = (event.which == 8 || event.which == 9 || event.which == 32) || (event.which >= 48 && event.which <= 57) || (event.which >= 65 && event.which <= 90) || (event.which >= 97 && event.which <= 122);
        if (!valid) {
            event.preventDefault();
        }
    },

    //Author: Ahmad Raza
    //Date :  21-06-2016
    //Function Name: validateSpecialCharacters
    //Description: This function will validate the special characters
    validateVolume: function (event, obj) {

        if ($(obj).val().length === 3) {
            event.preventDefault();
        }
    },

    rowExpand: function ($row, obj) {


        var _self = obj,
            $actions,
            values = [];
        var row = _self.datatable.row($row);
        //if ($row.hasClass('adding')) {
        //    $row.removeClass('adding');
        //}
        if (row.child.isShown()) {
            // This row is already open - close it
            $row.find("td:first .fa-minus-square").attr("class", "fa fa-plus-square");
            row.child.hide();
            //tr.removeClass('shown');
        }
        else {
            $row.find("td:first .fa-plus-square").attr("class", "fa fa-minus-square");
            // Open this row
            row.child.show();
        }

    },

    rowDetail: function ($row, obj) {
        var ChargeCapId = Number($row.attr("id"));
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Orders and Results_Lab Order", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ChargeCapId"] = ChargeCapId;
                params["mode"] = "Edit";
                //Edited by Azeem Raza Tayyab on 16-Feb-2016 to fix bug#: PMS-3871
                if (ClinicalLabOrderDetail.params.TabID == 'chargeBatchDetail' || ClinicalLabOrderDetail.params.TabID == 'billTabUnClaimedAppointment' || ClinicalLabOrderDetail.params.TabID == 'Bill_ChargeSearch' || ClinicalLabOrderDetail.params.TabID == 'Patient_Case_Detail' || ClinicalLabOrderDetail.params.TabID == 'schTabCalendar' || ClinicalLabOrderDetail.params.TabID == 'batchTabEncounter' || ClinicalLabOrderDetail.params.TabID == 'Bill_FollowUpPatientAR_Detail' || ClinicalLabOrderDetail.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || ClinicalLabOrderDetail.params.TabID == "billTabClaimSubmission" || ClinicalLabOrderDetail.params.TabID == "Bill_PaymentPosting" || ClinicalLabOrderDetail.params.TabID == "EDIClaimViewDetail")
                    params["ParentCtrl"] = 'ClinicalLabOrderDetail';
                else
                    params["ParentCtrl"] = ClinicalLabOrderDetail.params["TabID"];
                LoadActionPan('chargeSearchDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    rowRemove: function ($row, obj) {
        utility.myConfirm('1', function () {
            if ($row.hasClass('adding')) {
            }
            //var _self = obj;
            //_self.datatable.row($row.get(0)).remove().draw();
            if (parseInt($row.attr("id")) > 0) {
                ClinicalLabOrderDetail.DeleteLabOrderTest($row.attr("id"), $row, obj);
            }
            else {
                var _self = obj;
                _self.datatable.row($row.get(0)).remove().draw();
                utility.DisplayMessages("Successfully Deleted", 1);

                //Start 05-04-2016 Humaira Yousaf to enable/disable action buttons
                if ($("#" + ClinicalLabOrderDetail.params.PanelID + " #dgvLabOrderDetail tbody tr").length > 0 && $("#" + ClinicalLabOrderDetail.params.PanelID + " #dgvLabOrderDetail tbody tr").find('.dataTables_empty').length == 0) {
                    $("#" + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #btnSaveOrder,#btnSignPrintOrder,#btnSignOrder, #btnPrintLabOrder,#btnResetLabOrder").removeClass('disableAll');
                }
                else {
                    $("#" + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #btnSaveOrder,#btnSignPrintOrder,#btnSignOrder, #btnPrintLabOrder,#btnResetLabOrder").addClass('disableAll');
                }
                //End 05-04-2016 Humaira Yousaf to enable/disable action buttons
            }
            ClinicalLabOrderDetail.OrderedTestsCount--;
            ClinicalLabOrderDetailAOE.UnAnsweredSelections = [];
            ClinicalLabOrderDetail.AOEsExists = false;
            if (ClinicalLabOrderDetail.OrderedTestsCount == 0) {
                $('#pnlClinicalLabOrderDetail #ddlLabId').prop('disabled', false);
            }
            ClinicalLabOrderDetail.ArrayValidation = [];
        }, function () {
        },
                    '1'
    );

    },

    enableRemoveRow: function (CurrentRow) {
        CurrentRow.find("td.actions .remove-row").removeClass("hidden");
        //    .each(function () {
        //    $(this).removeclass('hidden')
        //});
    },

    AddNewLabRow: function (modifier, RowId, mode, CurrRef, cptCode, procDesc, cptDescription, response, SampleStorage, FavoriteGroupTestLabId) {

        var LabGridId = "#" + ClinicalLabOrderDetail.params.PanelID + " #dgvLabOrderDetail";

        var CurrentRow = null;
        if (RowId && RowId > 0) {
            if (ClinicalLabOrderDetail.params.ParentCtrl != null) {
                CurrentRow = ClinicalLabOrderDetail.EditableGrid.rowAdd(RowId, "");
            }
            else {
                CurrentRow = ClinicalLabOrderDetail.EditableGrid.rowAdd(RowId, ClinicalLabOrderDetail.params.LabId);
            }
        }
        else {
            var TemplateRow = $("#" + ClinicalLabOrderDetail.params.PanelID + " #dgvLabOrderDetail tbody tr[id*='-']").last();
            var TemplateRowId = 0;
            if (TemplateRow.length > 0) {
                if (TemplateRow.attr("id").indexOf("Child") != "-1")
                    TemplateRowId = parseInt(TemplateRow.prev().attr("id"));
                else
                    TemplateRowId = parseInt(TemplateRow.attr("id"));
            }

            CurrentRow = ClinicalLabOrderDetail.EditableGrid.rowAdd(TemplateRowId - 1, "", 'ClinicalLabOrderDetail');
        }

        //add cptCode and description to the test row
        var rowId = CurrentRow.attr("id");

        var cptCodeHtml = '<input type="hidden" id="CPTCode' + rowId + '"  name="CPTCode" value="' + cptCode + '" />';
        var LabId = '<input type="hidden" id="FavoriteGroupTestLabId' + rowId + '" name = "FavoriteGroupTestLabId" value = "' + FavoriteGroupTestLabId + '" />';
        var cptDescHtml = '<input type="hidden" id="CPTDescription' + rowId + '" name="CPTDescription"  value="' + procDesc + '"  />';
        var modifierHtml = '<input type="hidden" id="Modifier' + rowId + '"  name="Modifier" value="' + modifier + '" />';

        var IsSeperateTestOrder = '<input type="hidden" id="IsSeperateTestOrder' + rowId + '" name = "IsSeperateTestOrder" value = "' + ClinicalLabOrderDetail.getSeperateTestOrdersBit(FavoriteGroupTestLabId, cptCode) + '" />';



        if (LabId != undefined || LabId != null) {
            $(CurrentRow).find('td:first').append(cptCodeHtml + cptDescHtml + LabId + IsSeperateTestOrder + modifierHtml);
        }
        else {
            $(CurrentRow).find('td:first').append(cptCodeHtml + cptDescHtml + modifierHtml);
        }

        //Start//For Questions and Answer Of Loinc Code
        var isAnswerExists = false;
        var isQuestionExists = false;
        if (response != null && RowId > 0) {

            isQuestionExists = response.AOEExists == "1" ? true : false;
            isAnswerExists = response.AnswerExists == "1" ? true : false;
            if (isAnswerExists) {
                isQuestionExists = true;
            }
            //var object = {
            //    Answer: "Hello Response From Database",
            //    CPTCode: response.CPTCode,
            //    //FirstLoad:true
            //};
            //ClinicalLabOrderDetail.pusCPTCodeQA(object,true);

            ClinicalLabOrderDetail.setQuestionAndAnswerIcon(isQuestionExists, isAnswerExists, $(CurrentRow), response.CPTCode, response.LabOrderTestId, false, response.CPTDescription);
        }
        else {

            ClinicalLabOrderDetail.getLabOrderQuestionAnswer(cptCode).done(function (quesAnsObj) {
                if (quesAnsObj != null) {

                    isQuestionExists = quesAnsObj.IsQuestionExists;

                }
                ClinicalLabOrderDetail.setQuestionAndAnswerIcon(isQuestionExists, isAnswerExists, $(CurrentRow), cptCode, null, true, procDesc);
            });
        }

        //End//For Questions and Answer Of Loinc Code


        //add cptCode and description to the test row
        var row = ClinicalLabOrderDetail.EditableGrid.datatable.row(CurrentRow);
        row.child(ClinicalLabOrderDetail.buildRowChild(row.data(), CurrentRow.attr("id"), SampleStorage)).show();
        row.child().attr("id", "Child" + CurrentRow.attr("id"));

        row.child().loadDropDowns(true).done(function () {

            $(CurrentRow).find('select[id*="Urgency"]').val('1');
        });

        row.child().find('select,datalist').each(function () {
            var data = null;
            if ($(this).attr('id').indexOf("Specimen") == 0) {
                data = {
                    'StrID': cptCode, 'ID': $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #ddlLabId").val(), 'StrID2': 'Prefer', 'StrID3': $(this).attr('id')
                };

                ddlSpecimen = this;
                ClinicalLabOrderDetail.ddlSpecimen.push(this);

                return MDVisionService.lookups('GetSpecimen', true, data).done(function (results) {
                    if (results["GetSpecimen"] != null) {
                        results = JSON.parse(results["GetSpecimen"]);
                    }
                    if (results) {
                        var l = null;
                        for (var count in ClinicalLabOrderDetail.ddlSpecimen) {
                            if ($(ClinicalLabOrderDetail.ddlSpecimen[count]).attr('id') == results.DropDownId) {
                                l = $(ClinicalLabOrderDetail.ddlSpecimen[count]);
                            }
                        }
                        if (l != null) {
                            results = JSON.parse(results.data);
                            l.empty();
                            $.each(results, function (j, result) {
                                if (result.Name == "CSF" || result.Name == "LRI") {
                                    if (globalAppdata["isTransPubHealthAgAntimicobialUse"] && globalAppdata["isTransPubHealthAgAntimicobialUse"].toLowerCase() == "true")
                                        l.append($("<option />").val(result.Value).text(result.Name).attr("RefValue", result.RefValue).attr("RefName", result.RefName));
                                }
                                else
                                    l.append($("<option />").val(result.Value).text(result.Name).attr("RefValue", result.RefValue).attr("RefName", result.RefName));
                            });
                        }
                        //ali awan
                    }
                });
            }
            else if ($(this).attr('id').indexOf("AlternativeSpecimen") == 0) {
                data = {
                    'StrID': cptCode, 'ID': $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #ddlLabId").val(), 'StrID2': 'Alternative', 'StrID3': $(this).attr('id')
                };
                ddlAlternativeSpecimen = this;
                ClinicalLabOrderDetail.ddlAlternativeSpecimen.push(this);
                return MDVisionService.lookups('GetSpecimen', true, data).done(function (results) {
                    if (results["GetSpecimen"] != null) {
                        results = JSON.parse(results["GetSpecimen"]);
                    }

                    if (results) {
                        var l = null;
                        for (var count in ClinicalLabOrderDetail.ddlAlternativeSpecimen) {
                            if ($(ClinicalLabOrderDetail.ddlAlternativeSpecimen[count]).attr('id') == results.DropDownId) {
                                l = $(ClinicalLabOrderDetail.ddlAlternativeSpecimen[count]);
                            }
                        }

                        if (l != null) {
                            results = JSON.parse(results.data);
                            l.empty();
                            $.each(results, function (j, result) {
                                l.append($("<option />").val(result.Value).text(result.Name).attr("RefValue", result.RefValue).attr("RefName", result.RefName));
                            });
                        }
                    }
                });
            }
            else if ($(this).attr('id').indexOf("TestType") == 0) {

                var ddlTestType = $(this);
                //ClinicalLabOrderDetail.ddlTestType.push(this);

                return MDVisionService.lookups('GetTestType', true, null).done(function (results) {
                    if (results["GetTestType"] != null) {
                        results = JSON.parse(results["GetTestType"]);
                    }
                    if (results) {
                        ddlTestType.empty();
                        $.each(results, function (j, result) {
                            ddlTestType.append($("<option />").val(result.Value).text(result.Name).attr("RefValue", result.RefValue).attr("RefName", result.RefName));
                        });
                    }
                });
            }
            else {
                return true;
            }
        });

        row.child.hide();
        //, cptCode, procDesc, cptDescription
        $(CurrentRow).find('input[id*="LabProcedure"]').val(cptCode + " " + procDesc);

        // $(CurrentRow).find('input[id*="LabProcedure"]').val(procDesc);

        ClinicalLabOrderDetail.enableRemoveRow($(CurrentRow));

        //Start 05-04-2016 Humaira Yousaf to enable/disable action buttons
        if ($("#" + ClinicalLabOrderDetail.params.PanelID + " #dgvLabOrderDetail tbody tr").length > 0 && $("#" + ClinicalLabOrderDetail.params.PanelID + " #dgvLabOrderDetail tbody tr").find('.dataTables_empty').length == 0) {
            $("#" + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #btnSaveOrder,#btnSignPrintOrder,#btnSignOrder,#btnPrintLabOrder,#btnResetLabOrder").removeClass('disableAll');
        }
        else {
            $("#" + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #btnSaveOrder,#btnSignPrintOrder,#btnSignOrder,#btnPrintLabOrder,#btnResetLabOrder").addClass('disableAll');
        }
        //End 05-04-2016 Humaira Yousaf to enable/disable action buttons
        //Start Farooq Ahmad 07/14/2016 /EMR-588
        var dgvLabOrderDetail = $("#" + ClinicalLabOrderDetail.params.PanelID + " #dgvLabOrderDetail");

        $(dgvLabOrderDetail).find('input[id*="dtpLabDate"]').removeClass('size80');
        $(dgvLabOrderDetail).find('input[id*="dtpLabDate"]').closest('div').removeClass('size80');

        $(dgvLabOrderDetail).find('input[id*="tpLabTime"]').removeClass('size80');
        $(dgvLabOrderDetail).find('input[id*="tpLabTime"]').closest('div').removeClass('size80');

        $(dgvLabOrderDetail).find('input[id*="LabProcedure"]').addClass('size-min300 mb-tiny');
        //End Farooq Ahmad 07/14/2016 /EMR-588

        return CurrentRow;
    },

    getSeperateTestOrdersBit: function (LabId, cptCode) {
        var IsVitalAxis = false;
        var IsSeperateTestOrder = false;
        for (i = 0; i < ClinicalLabOrderDetail.LabsData.length; i++) {
            if (ClinicalLabOrderDetail.LabsData[i].LabId == LabId && ClinicalLabOrderDetail.LabsData[i].Name == "VitalAxis") {
                IsVitalAxis = true;
                break;
            }
        }
        if (IsVitalAxis) {
            if (cptCode != '84154' && cptCode != '84403' && cptCode != '84153' && cptCode != '84439' && cptCode != '84443') {
                IsSeperateTestOrder = true;
            }
        }
        return IsSeperateTestOrder;
    },
    setQuestionAndAnswerIcon: function (isQuestionExists, isAnswerExists, $CurrentRow, cptCode, labOrderTestId, fromNewRow, CPTDescription) {
        if (isQuestionExists) {

            var quesAnsIcon = "";
            var commentIconColor = 'blue';
            $CurrentRow.attr('IsAnswered', 'false');
            if (isAnswerExists) {
                commentIconColor = 'green';
                $CurrentRow.attr('IsAnswered', 'true');
            }

            labOrderTestId = labOrderTestId == null ? 0 : labOrderTestId;
            var onClick = "ClinicalLabOrderDetail.loadQuestionAndAnswers(this,'" + cptCode + "','" + labOrderTestId + "','" + CPTDescription + "');";
            quesAnsIcon = '<a href="#" onClick ="' + onClick + '" class="btn-xs mr-none btn" title="View Questions And Answers"><i class="fa fa-comments ' + commentIconColor + '" aria-hidden="true"></i></a>';
            $CurrentRow.find('td[class*="actions"]').append(quesAnsIcon);
            if (fromNewRow == true) {
                ClinicalLabOrderDetail.loadQuestionAndAnswers(this, cptCode, labOrderTestId, CPTDescription);
            }
        }
    },

    getLabOrderQuestionAnswer: function (CPTCode) {

        var quesAnsObj = new Object();
        var deffered = new $.Deferred();
        quesAnsObj.IsQuestionExists = false;
        if (CPTCode != null) {

            ClinicalLabOrderDetail.getLabOrderQuestionAnswerDbCall(CPTCode).done(function (response) {

                if (response != "") {
                    response = JSON.parse(response);
                    if (response.LabOrderAOE_Count > 0) {
                        quesAnsObj.IsQuestionExists = true;
                    }
                }
                deffered.resolve(quesAnsObj);
            });
        }
        return deffered.promise();
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason:to add more problem is Associated Problem list
    addProblem: function () {
        var params = [];

        params["RefForm"] = "frmClinicalLabOrderDetail";
        params["FromOrderDetail"] = "1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ClinicalLabOrderDetail";
        params["CurrentNotesProviderId"] = ClinicalLabOrderDetail.params["CurrentNotesProviderId"];
        params["IsFromNote"] = ClinicalLabOrderDetail.params["IsFromNote"];
        LoadActionPan('Clinical_ProblemLists', params);
    },

    GetLabRowsJSON: function () {

        //var myArray = [];
        //var objLab = new Object();
        //$.each($("#" + ClinicalLabOrderDetail.params.PanelID + " #dgvLabOrderDetail tbody tr:not([id*=Child]"), function (i, item) {

        //    var $row = $(this).closest("tr");
        //    var rowId = $row.attr("id");
        //    var self = $row;
        //    var myJSON = self.getMyJSONByName();
        //    var childRow = ClinicalLabOrderDetail.EditableGrid.datatable.row($row).child();
        //    var childRowsJSON = childRow.getMyJSONByName();
        //    JSONToSave = utility.MergeJSON(myJSON, childRowsJSON);
        //    //objLab["LabOrderId" + rowId] = JSONToSave;
        //    objLab["LabOrderId"] = JSONToSave;
        //    //myArray.push(myJSON);
        //});
        //objLab["commandType"] = "save_Lab_order_test";

        var LabTestIds = $("#" + ClinicalLabOrderDetail.params.PanelID + " #dgvLabOrderDetail tbody tr:not([id*=Child]").map(function () {
            return this.id.replace("id", "");
        }).get().join(',');

        $("#" + ClinicalLabOrderDetail.params.PanelID + " #pnlLab_ResultOrderDetail #hfLabTestIds").val(LabTestIds);

        var self = $("#" + ClinicalLabOrderDetail.params.PanelID + " #pnlLab_ResultOrderDetail");
        var myJSON = self.getMyJSONByName();


        ClinicalLabOrderDetail.SaveLabOrderTest(myJSON);
        //console.log(objLab);
    },

    SaveLabOrderTest: function (objLab) {
        ClinicalLabOrderDetail.LabOrderTestSave_DBCall(objLab).done(function (response) {

            var response = JSON.parse(response);
            if (response.status != false) {

            } else {

            }


        });
    },

    LabOrderTestSave_DBCall: function (LabOrderData) {

        if (ClinicalLabOrderDetail.params.mode.toLowerCase() == "add") {

            var objData = JSON.parse(LabOrderData);
            objData["commandType"] = "save_Lab_order_test";
            var data = JSON.stringify(objData);
            return MDVisionService.APIService(data, "LabOrder", "LabOrder");

        } else if (ClinicalLabOrderDetail.params.mode.toLowerCase() == "edit") {
            var objData = JSON.parse(LabOrderData);
            objData["commandType"] = "save_Lab_order_test";
            objData["LabOrderId"] = $("#" + ClinicalLabOrderDetail.params.PanelID + ' #hfLabOrderId').val();
            var data = JSON.stringify(objData);
            return MDVisionService.APIService(data, "LabOrder", "LabOrder");
        }
    },

    LabOrderTestGridLoad: function (response) {
        var response = JSON.parse(response);
        var PanelLabGrid = "#" + ClinicalLabOrderDetail.params.PanelID + " #pnlLab_ResultOrderDetail";
        var LabGridId = "#" + ClinicalLabOrderDetail.params.PanelID + " #dgvLabOrderDetail";
        $(LabGridId + " tbody tr").remove();


        if ($.fn.dataTable.isDataTable(LabGridId)) {
            $(LabGridId).dataTable().fnClearTable();
            $(LabGridId).dataTable().fnDestroy();
        }

        ClinicalLabOrderDetail.EditableGrid.datatable.clear().draw();

        var LabOrderTestLoadJSONData = JSON.parse(response.LabOrderTest_JSON);
        ClinicalLabOrderDetail.ddlSpecimen = [];
        ClinicalLabOrderDetail.ddlAlternativeSpecimen = [];
        $.each(LabOrderTestLoadJSONData, function (i, item) {

            var CurrentRow = null;
            var newChildRow = null;
            var createCurrentRow = function (i, item, CurrentRow, newChildRow) {
                var LabOrderTestId = item.LabOrderTestId;
                item.CPTDescription = utility.decodeHtml(item.CPTDescription);
                var arrChildLab = [];
                arrChildLab.push(item);
                CurrentRow = ClinicalLabOrderDetail.AddNewLabRow(item.Modifier, LabOrderTestId, null, null, item.CPTCode, item.CPTDescription, null, item, item.SampleStorage);
                ClinicalLabOrderDetail.buildRowChild(CurrentRow, CurrentRow.attr("id"), null, null, arrChildLab);
                var self = $("#" + ClinicalLabOrderDetail.params.PanelID + " tr#" + LabOrderTestId);
                var row = ClinicalLabOrderDetail.EditableGrid.datatable.row(CurrentRow);
                item.CurrentRow = CurrentRow;

                newChildRow = row.child();
                item.newChildRow = newChildRow;
                var LabTestTable = $("#" + ClinicalLabOrderDetail.params.PanelID + " #dgvLabOrderDetail");
            }

            //Start Farooq Ahmad 03/28/2016 bind values to the table
            var counter = 0;
            var BindFunction = function (counter, item, CurrentRow, newChildRow) {
                if (CurrentRow == null) {
                    CurrentRow = item.CurrentRow;
                }
                if (newChildRow == null) {
                    newChildRow = item.newChildRow;
                }
                //Start 08-11-2016 Edit By Humaira Yousaf Bug# EMR-1925
                //if (counter++ < 5 && $(CurrentRow).find('select option').length <= 1) {
                //    // setTimeout(BindFunction, 1000, counter, item, CurrentRow, newChildRow);
                //}
                //else {
                utility.bindMyJSONByName(true, item, false, $(CurrentRow)).done(function () {
                    utility.bindMyJSONByName(true, item, false, $(newChildRow)).done(function () {


                        if (($(newChildRow).find('select[id^="Specimen"]').val() && $(newChildRow).find('select[id^="Specimen"]').val() != item.Specimen) || ($(newChildRow).find('select[id*="Organism"]').val() != item.Organism)) {
                            // setTimeout(BindFunction, 1000, counter, item, CurrentRow, newChildRow);
                        } else {
                            $(newChildRow).find('select[id*="ddlSpecimenSource"]').attr("specimensource", item.SpecimenSource);
                            $(newChildRow).find('select[id*="AlternativeSpecimen"]').attr("altspecimensource", item.AlternativeSpecimenSource);
                            $(newChildRow).find('select[id*="Antimicrobials"]').attr("antimicrobials", item.Antimicrobials);

                            $(newChildRow).find('select[id^="Specimen"]').trigger("change");
                        }
                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").data('serialize', $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").serialize());
                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #txtFacility").blur();
                    });
                });
                //}

                //End 08-11-2016 Edit By Humaira Yousaf Bug# EMR-1925
            }
            createCurrentRow(i, item, CurrentRow, newChildRow);
            utility.callbackAfterAllDOMLoaded(function () {
                BindFunction(counter, item, CurrentRow, newChildRow);
            });
            //$.when(createCurrentRow(i, item, CurrentRow, newChildRow)).then(BindFunction(counter, item, CurrentRow, newChildRow));
            ClinicalLabOrderDetail.OrderedTestsCount++;
        });

    },

    DeleteLabOrderTest: function (LabTestId, $row, obj) {

        ClinicalLabOrderDetail.LabOrderTest_DBCall(LabTestId).done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {
                utility.DisplayMessages(response.Message, 1);
                var _self = obj;
                _self.datatable.row($row.get(0)).remove().draw();

                //Start 05-04-2016 Humaira Yousaf to enable/disable action buttons
                if ($("#" + ClinicalLabOrderDetail.params.PanelID + " #dgvLabOrderDetail tbody tr").length > 0 && $("#" + ClinicalLabOrderDetail.params.PanelID + " #dgvLabOrderDetail tbody tr").find('.dataTables_empty').length == 0) {
                    $("#" + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #btnSaveOrder,#btnSignPrintOrder,#btnSignOrder,#btnPrintLabOrder,#btnResetLabOrder").removeClass('disableAll');
                }
                else {
                    $("#" + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #btnSaveOrder,#btnSignPrintOrder,#btnSignOrder,#btnPrintLabOrder,#btnResetLabOrder").addClass('disableAll');
                }
                //End 05-04-2016 Humaira Yousaf to enable/disable action buttons

            } else {
                utility.DisplayMessages(response.Message, 3);
            }

        });

    },

    LabOrderTest_DBCall: function (LabTestId) {

        var objData = new Object();
        objData["LabOrderTestId"] = LabTestId;
        objData["PatientId"] = Clinical_LabOrder.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        objData["commandType"] = "DELETE_LabORDER_TEST";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabOrder", "LabOrder");
    },


    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: get Latest Lab Order By PatientId
    getLatestLabOrderByPatientId: function (hideAlertMessage) {
        var strMessage = '';
        //   AppPrivileges.GetFormPrivileges("Notes_Notes", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            ClinicalLabOrderDetail.getLatestLabOrderByPatientIdDBCall().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    ClinicalLabOrderDetail.createLabOrderBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage);
                }
                else {
                    utility.DisplayMessages(strMessage, 3);
                }

            });
        }
        else {
            utility.DisplayMessages(strMessage, 3);
        }
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: To create Lab Order's Body HTML
    createLabOrderBodyHTML: function (response, NoteHTMLCtrl, UnloadLabOrder, hideAlertMessage) {
        ClinicalLabOrderDetail.checkLabOrderExists();
        if (response.LabOrderFill_JSON != null && response.LabOrderFill_JSON != '') {
            var LabOrderFill_Obj = JSON.parse(response.LabOrderFill_JSON);
            var $mainDivLabOrder = $(document.createElement('div'));

            var LabOrderId = LabOrderFill_Obj.LabOrderId;
            if (LabOrderId > 0) {
                var $SectionBodyLabOrder = $(document.createElement('section'));
                $SectionBodyLabOrder.attr('id', "Cli_LabOrderDetail_Main" + LabOrderId);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_LabOrderDetail_" + LabOrderId);
                var $ListLabOrder = $(document.createElement('ul'));

                $ListLabOrder.attr('class', 'list-unstyled')

                $SectionBodyLabOrder.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_LabOrderDetail_" + LabOrderId + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_LabOrderDetail_Main" + LabOrderId + '"  ><i class="fa fa-times"></i></a></div> ');


                $ListLabOrder.append("<li>" + LabOrderFill_Obj.SoapText + "</li>");
                $DetailsDiv.append($ListLabOrder);
                $SectionBodyLabOrder.append($DetailsDiv);
                if ($(NoteHTMLCtrl + ' clinical_laborders').parent().parent().find('#Cli_LabOrderDetail_Main' + LabOrderId).length == 0) {
                    $mainDivLabOrder.append($SectionBodyLabOrder);
                    ClinicalLabOrderDetail.updateLabOrderHtml($mainDivLabOrder.html(), LabOrderId, NoteHTMLCtrl, hideAlertMessage);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' clinical_laborders').parent().parent().find('#Cli_LabOrderDetail_Main' + LabOrderId + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' clinical_laborders').parent().parent().find('#Cli_LabOrder_Main' + LabOrderId + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' clinical_laborders').parent().parent().find('#Cli_LabOrderDetail_Main' + LabOrderId).html($SectionBodyLabOrder.html());
                    $(NoteHTMLCtrl + ' clinical_laborders').parent().parent().find('#Cli_LabOrderDetail_Main' + LabOrderId + ' ul').append(CommentHTML);
                    Clinical_ProgressNote.saveComponentSOAPText('Lab Orders', hideAlertMessage);
                    ClinicalLabOrderDetail.updateLabOrderHtml("", LabOrderId, NoteHTMLCtrl, hideAlertMessage);

                }

                if (UnloadLabOrder == true) {
                    ClinicalLabOrderDetail.Unload(ClinicalLabOrderDetail.bNextPrev);
                }
            }
        }
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: To detach Components Lab Order
    detach_ComponentsLabOrder: function (ComponentName, IsUpdate, LabOrdersComponentRemove) {
        var LabOrderIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_laborders').parent().parent().find('section[id*="Cli_LabOrderDetail_Main"]').map(function () {
            return this.id.replace("Cli_LabOrderDetail_Main", "");
        }).get().join(',');
        var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .LabOrdersComponent').attr('NoteComponentId');
        if (!NoteComponentId)
            NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .LabOrderComponent').attr('NoteComponentId');

        if (LabOrdersComponentRemove) {
            if (NoteComponentId && NoteComponentId != "NCDummyId") {
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_laborders').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Lab Orders', true))
                }
                else
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                $.when.apply($, promise).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_laborders').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                    $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Lab Orders']").remove();
                });
            }
            else {
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Lab Orders']").remove();
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_laborders').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Lab Orders', true))
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
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_laborders').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                    $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Lab Orders']").remove();
                });
            }
        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_laborders').parent().parent().find('section[id*="Cli_LabOrderDetail_Main"]').remove();
        }

        if (LabOrderIds == "" || LabOrderIds == "undefined") {
            var promise = [];
            if (Clinical_ProgressNote.params["TemplateName"]) {
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_laborders').parent().parent().addClass('hidden');
                promise.push(Clinical_ProgressNote.saveComponentSOAPText('Lab Orders', true))
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
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_laborders').parent().parent().remove();
                Clinical_ProgressNote.ShowHideComponetsHeaders();
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Lab Orders']").remove();
                utility.DisplayMessages('Successfully Deleted', 1);
            });
        }
        else {
            ClinicalLabOrderDetail.detachLabOrderFromNotesDBCall(LabOrderIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_laborders').parent().parent().find('section').remove();
                    if (IsUpdate) {
                        Clinical_ProgressNote.saveComponentSOAPText('Lab Orders');
                        //Start 01-12-2016 Humaira Yousaf to hide show eSuperbill link
                        Clinical_ProgressNote.HideShowBillingInfo();
                        //End 01-12-2016 Humaira Yousaf to hide show eSuperbill link
                    }
                    utility.DisplayMessages(response.Message, 1);
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: To detach Lab Order from Notes
    detachLabOrderFromNotes: function (LabOrderId) {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('1', function () {
                EMRUtility.scrollToPNcomponent('clinical_laborder');
                EMRUtility.scrollToPNcomponent('clinical_laborders');
                var selectedValue = LabOrderId.replace('Cli_LabOrderDetail_Main', '');
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    ClinicalLabOrderDetail.detachLabOrderFromNotes_DBCall(selectedValue).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            $('#' + LabOrderId).remove();
                            if ($(".LabOrdersComponent section[data-status=Pending]").length <= 0) {
                                $("#pendingordertext").remove();
                            }
                            if ($(".LabOrdersComponent section[data-status=Transmitted]").length <= 0) {
                                $("#sentordertext").remove();
                            }
                            Clinical_ProgressNote.saveComponentSOAPText('Lab Orders');
                            setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);

                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            }, function () { },
                '1'
            );
        }
        else
            utility.DisplayMessages(strMessage, 2);
        // });
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: DB Call to detach Lab Order from Notes
    detachLabOrderFromNotes_DBCall: function (LabOrderId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["LabOrderId"] = LabOrderId;
        if (Clinical_ProgressNote.params.NotesVisitId == "" || Clinical_ProgressNote.params.NotesVisitId == "undefined") {
            objData["VisitId"] = 0;
        } else {
            if (Clinical_ProgressNote.params.NotesVisitId < 1) {
                objData["VisitId"] = $('#pnlClinicalProgressNote #hfVisitId').val();
            } else {
                objData["VisitId"] = Clinical_ProgressNote.params.NotesVisitId;
            }

        }
        if (Clinical_ProgressNote.params.patientID == "" || Clinical_ProgressNote.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_ProgressNote.params.patientID;
        }
        objData["commandType"] = "detach_Laborder_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "LabOrder", "LabOrder");
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: DB Call to detach Lab Order From Notes
    detachLabOrderFromNotesDBCall: function (LabId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["LabOrderId"] = LabId;
        if (Clinical_ProgressNote.params.NotesVisitId == "" || Clinical_ProgressNote.params.NotesVisitId == "undefined") {
            objData["VisitId"] = 0;
        } else {
            if (Clinical_ProgressNote.params.NotesVisitId < 1) {
                objData["VisitId"] = $('#pnlClinicalProgressNote #hfVisitId').val();
            } else {
                objData["VisitId"] = Clinical_ProgressNote.params.NotesVisitId;
            }

        }
        if (Clinical_ProgressNote.params.patientID == "" || Clinical_ProgressNote.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_ProgressNote.params.patientID;
        }
        objData["commandType"] = "detach_LabOrder_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "LabOrder", "LabOrder");
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: To update Lab Order Html
    updateLabOrderHtml: function (LabOrderHtml, LabOrderId, NoteHTMLCtrl, hideAlertMessage) {
        $(NoteHTMLCtrl + ' clinical_laborders').parent().parent().addClass('initialVisitBody');
        if (LabOrderHtml != '') {
            $(NoteHTMLCtrl + ' clinical_laborders').parent().parent().append(LabOrderHtml);
        }
        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (LabOrderHtml != '') {
            ClinicalLabOrderDetail.attachLabOrderWithNotes(LabOrderId, hideAlertMessage);
        }

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: To attach Lab Order With Notes
    attachLabOrderWithNotes: function (LabOrderId, hideAlertMessage) {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {

            var selectedValue = LabOrderId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                ClinicalLabOrderDetail.attachLabOrderWithNotesDBCall(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        //If Attached MedicalHx Made new inseration to MedicalHx Table than good ids should be attached to HTML
                        Clinical_ProgressNote.saveComponentSOAPText('Lab Orders', hideAlertMessage);
                        $('#' + LabOrderId).remove();

                        // utility.DisplayMessages(response.Message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }


        }
        else
            utility.DisplayMessages(strMessage, 2);
        // });
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: DB Call to attach Lab Order With Notes
    attachLabOrderWithNotesDBCall: function (LabOrderId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["LabOrderId"] = LabOrderId;
        if (Clinical_ProgressNote.params.NotesVisitId == "" || Clinical_ProgressNote.params.NotesVisitId == "undefined") {
            objData["VisitId"] = 0;
        } else {
            if (Clinical_ProgressNote.params.NotesVisitId < 1) {
                objData["VisitId"] = $('#pnlClinicalProgressNote #hfVisitId').val();
            } else {
                objData["VisitId"] = Clinical_ProgressNote.params.NotesVisitId;
            }

        }
        if (Clinical_ProgressNote.params.patientID == "" || Clinical_ProgressNote.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_ProgressNote.params.patientID;
        }
        objData["commandType"] = "attach_LabOrder_with_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "LabOrder", "LabOrder");
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: DB Call to get Latest Lab Order By PatientId
    getLatestLabOrderByPatientIdDBCall: function () {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["NoteId"] = Clinical_Notes.params.NotesId;
        objData["commandType"] = "getlatest_Laborderby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabOrder", "LabOrder");
    },

    getLabOrderInfo: function (labOrderId) {
        ClinicalLabOrderDetail.fillLabOrder(labOrderId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {

                    ClinicalLabOrderDetail.createLabOrderBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML');

                }
                else {
                    utility.DisplayMessages(strMessage, 3);
                }
            }
        });
    },


    generateBarcode: function () {

        var value = $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfOrderNumber").val();

        var btype = 'code39';
        var renderer = 'css';

        var quietZone = false;


        var settings = {
            output: renderer,
            bgColor: '#FFFFFF',
            color: '#000000',
            barWidth: '1',
            barHeight: '50',
            moduleSize: '5',
            posX: '10',
            posY: '20',
            addQuietZone: '1'
        };



        $('#' + ClinicalLabOrderDetail.params.PanelID + " #barcodeTarget").html("").show().barcode(value, btype, settings);

    },

    //Author: Humaira Yousaf
    //Date :  25-04-2016
    //Description: Creates PDF to view Lab Order Result
    printLabOrder: function (includeBarCode, orderId) {
        var LabOrderId = $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfLabOrderId").val();
        if (orderId != null && orderId != "") {
            LabOrderId = orderId;
        }
        Clinical_LabOrderView.previewLabOrder((Clinical_LabOrder.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val()), globalAppdata['AppUserId'], LabOrderId).done(function (response) {
            response = JSON.parse(response);
            if (response.status) {
                if (includeBarCode) {
                    //ClinicalLabOrderDetail.generateBarcode();
                    params["BarCodeHtml"] = 'true';
                    params["IsSpecimen"] = true;
                    $('#' + ClinicalLabOrderDetail.params.PanelID + " #barcodeTarget").hide();
                    var LabId = LabOrderId;
                    if (ClinicalLabOrderDetail.params.ParentCtrl == "clinicalTabLabOrder") {
                        if (ClinicalLabOrderDetail.IsFavoriteGroupSelected) {
                            ClinicalLabOrderDetail.FavGroupPrintSpecimen(LabId);
                        } else {
                            Clinical_LabOrder.printSpecimen(LabId, 'Signed', null, 'ClinicalLabOrderDetail');
                        }
                    }
                    else if (ClinicalLabOrderDetail.params.PanelID.indexOf('pnlClinicalProgressNote ') > -1) {
                        if (ClinicalLabOrderDetail.IsFavoriteGroupSelected) {
                            ClinicalLabOrderDetail.FavGroupPrintSpecimen(LabId);
                        } else {
                            Clinical_LabOrder.printSpecimen(LabId, 'Signed', null, 'ClinicalLabOrderDetail');
                        }
                    } else {
                        Clinical_LabOrder.printSpecimen(LabId, 'Signed', null, 'pnlClinicalLabOrder');
                    }
                    Clinical_LabOrderView.params["IsSpecimen"] = false;
                } else {
                    params["BarCodeHtml"] = 'true';
                    params["IsSpecimen"] = false;
                }
                utility.documentPrint(response.LabOrderHTML);
            }
            else {
                utility.DisplayMessages(response.Message, 2);
            }
        });
    },

    printLabOrderABN: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["UserId"] = globalAppdata['AppUserId'];
        params["PatientId"] = Clinical_LabOrder.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();


        params["ParentCtrl"] = "ClinicalLabOrderDetail";
        params["LabOrderId"] = $('#' + ClinicalLabOrderDetail.params.PanelID + " #hfLabOrderId").val();
        LoadActionPan('Clinical_LabOrderABNDetail', params);
    },
    getLabOrderQuestionAnswerDbCall: function (CPTCode, LabOrderTestId) {

        var objData = new Object();
        objData["CPTCode"] = CPTCode;
        objData["LabOrderTestId"] = LabOrderTestId;
        objData["commandType"] = "get_question_answer_by_cptcode";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabOrder", "LabOrder");
    },

    loadQuestionAndAnswers: function (obj, cptCode, LabOrderTestId, CPTDescription) {
        var strMessage = "";

        // AppPrivileges.GetFormPrivileges("Orders and Results_Lab Result", permissionState, "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //  if (strMessage == "") {
        var params = [];
        var $currentRow = $(obj).parent().parent();
        params["CPTCode"] = cptCode;
        params["CPTDescription"] = CPTDescription;
        params["LabOrderTestId"] = LabOrderTestId != null && LabOrderTestId != '0' ? LabOrderTestId : $currentRow.attr("id");//LabOrderTestId;

        params["FromAdmin"] = ClinicalLabOrderDetail.params["FromAdmin"];

        params["ParentCtrl"] = 'ClinicalLabOrderDetail';
        params["ParentCtrlPanelID"] = ClinicalLabOrderDetail.params.PanelID;
        LoadActionPan('ClinicalLabOrderDetailAOE', params, ClinicalLabOrderDetail.params.PanelID);

    },
    getCPTCodeQA: function (CPTCode) {

        CPTCodesQA = [];
        $.each(ClinicalLabOrderDetail.CPTCodeQA, function (index, item) {
            if (item.CPTCode == CPTCode) {
                CPTCodesQA.push(ClinicalLabOrderDetail.CPTCodeQA[index]);
            }
        });
        return CPTCodeQA;
    },
    pusCPTCodeQA: function (CPTCodeQA, onloadByPass) {


        if (onloadByPass != null) {

            ClinicalLabOrderDetail.CPTCodeQA.push(CPTCodeQA);
        }
        else {
            if (ClinicalLabOrderDetail.CPTCodeQA.length > 0) {

                isCPTCodeQAExists = false;
                $.each(ClinicalLabOrderDetail.CPTCodeQA, function (index, item) {
                    if (item.CPTCode == CPTCodeQA['CPTCode'] && item.Question == CPTCodeQA['Question']) {
                        isCPTCodeQAExists = true;
                        ClinicalLabOrderDetail.CPTCodeQA[index] = CPTCodeQA;
                    }
                });
                if (!isCPTCodeQAExists) {
                    ClinicalLabOrderDetail.CPTCodeQA.push(CPTCodeQA);
                }
            }
            else {
                ClinicalLabOrderDetail.CPTCodeQA.push(CPTCodeQA);
            }
        }

    },

    colorCPTCodeAOEIcon: function () {

        var LabGridId = "#" + ClinicalLabOrderDetail.params.PanelID + " #dgvLabOrderDetail tbody";


        var answerIndex = -1;
        $(LabGridId).find('tr').each(function (index, gridItem) {

            var isAnswerExists = false;

            var gridCPTCode = $(gridItem).find('td:first').find("input[id*='CPTCode']").val();


            $.each(ClinicalLabOrderDetail.CPTCodeQA, function (index, item) {

                if (item.CPTCode == gridCPTCode && item.Answer != "") {
                    isAnswerExists = true;

                    answerIndex = index;
                }
            });
            if (isAnswerExists) {

                $(gridItem).find('td[class*="actions"]').find("a").find("i.fa-comments").removeClass('blue').addClass('green');
                $(gridItem).attr('IsAnswered', 'true');
            }
            else {
                //$(gridItem).find('td[class*="actions"]').find("a").find("i.fa-comments").removeClass('green').addClass('blue');
                $(gridItem).attr('IsAnswered', 'false');
            }

        });

    },

    //Author: Humaira Yousaf
    //Date :  28-11-2016
    //Description: Checks if any associated problems is selected on saving order
    problemAdded: function () {

        var problemsSelected = false;
        if ($('#' + ClinicalLabOrderDetail.params.PanelID + " #ulProblemLists input:checked").length > 0) {
            problemsSelected = true;
        }

        return problemsSelected;
    },

    TestAdded: function () {
        var TestSelected = false;
        if ($('#' + ClinicalLabOrderDetail.params.PanelID + " #dgvLabOrderDetail tbody tr").length == 1) {
            if ($('#' + ClinicalLabOrderDetail.params.PanelID + " #dgvLabOrderDetail tbody tr td").length > 1) {
                TestSelected = true;
            }
        }
        else if ($('#' + ClinicalLabOrderDetail.params.PanelID + " #dgvLabOrderDetail tbody tr").length > 1) {
            TestSelected = true;
        }
        return TestSelected;
    },
    FavGroupPrintSpecimen: function (LabOrderId, ParentCtrl) {
        var params = [];
        params["LabOrderIDs"] = LabOrderId;
        params["ParentCtrl"] = 'Clinical_LabOrder';
        params["ParentCtrlPanelID"] = ClinicalLabOrderDetail.params.PanelID;
        LoadActionPan('ClincalFavGroupBarCodeView', params);
    },
    SaveFreeTextStatus: function () {
        if (Clinical_LabOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
            panel = "#pnlClinicalProgressNote #pnlClinicalLabOrderDetail";
        }
        else {
            panel = "#pnlClinicalLabOrderDetail";
        }
        var IsFreeText = false;
        $('input[name=FavoriteListType]:checked', panel).val() == "LabOrderGroup" ? IsFreeText = true : IsFreeText = false;
        return EMRUtility.insertUpdateFreeTextStatus(ClinicalLabOrderDetail.FavListName, IsFreeText);
    },
    populateSpecimenSource: function (Obj) {
        var CurrentRow = $(Obj).closest("tr");
        var ddl = null;

        if ($(Obj).attr("id").indexOf("Specimen") == 0) {
            ddl = CurrentRow.find('select[id*="ddlSpecimenSource"]');
            ClinicalLabOrderDetail.populateAntimicrobial(Obj);
        }
        else if ($(Obj).attr("id").indexOf("AlternativeSpecimen") == 0) {
            ddl = CurrentRow.find('select[id*="AlternativeSpecimenSource"]');
        }

        if ($(Obj).val() == "") {
            ddl.empty();
            ddl.append($("<option />").val("").text("- Select -"));
            return;
        }
        data = { 'ID': $(Obj).val() };

        MDVisionService.lookups('GetSpecimenSource', true, data).done(function (results) {
            if (results["GetSpecimenSource"] != null) {
                results = JSON.parse(results["GetSpecimenSource"]);
            }
            if (results) {

                if (ddl != null) {

                    ddl.empty();
                    $.each(results, function (j, result) {
                        ddl.append($("<option />").val(result.Value).text(result.Name).attr("RefValue", result.RefValue).attr("RefName", result.RefName));
                    });

                    ddl.val(ddl.attr("specimensource"));
                    ddl.attr("specimensource", "")
                }
            }
        });
    },
    populateAntimicrobial: function (Obj) {
        var CurrentRow = $(Obj).closest("tr");

        var ddlSpecimen = CurrentRow.find('select[id^="Specimen"]');
        var ddlOrganism = CurrentRow.find('select[id*="Organism"]');

        var ddlAntimicrobials = CurrentRow.find('select[id*="Antimicrobials"]');

        if ($(ddlSpecimen).val() == "" || $(ddlOrganism).val() == "") {
            ddlAntimicrobials.empty();
            ddlAntimicrobials.append($("<option />").val("").text("- Select -"));

            return;
        }


        //ddlAntimicrobials.multiselect('destroy');
        //ddlAntimicrobials.multiselect({
        //    includeSelectAllOption: true,
        //    enableFiltering: true,
        //    enableCaseInsensitiveFiltering: true,
        //    maxHeight: 247,
        //    onChange: function (option, checked) {
        //        alert('hello');
        //    },


        //})

        data = { 'ID': $(ddlSpecimen).val(), 'ID2': $(ddlOrganism).val() };


        MDVisionService.lookups('GetAntimicrobialBySpecimentAndOrganism', true, data).done(function (results) {
            if (results["GetAntimicrobialBySpecimentAndOrganism"] != null) {
                results = JSON.parse(results["GetAntimicrobialBySpecimentAndOrganism"]);
            }
            if (results) {

                if (ddlAntimicrobials != null) {

                    ddlAntimicrobials.empty();
                    $.each(results, function (j, result) {
                        ddlAntimicrobials.append($("<option />").val(result.Value).text(result.Name).attr("RefValue", result.RefValue).attr("RefName", result.RefName));
                    });
                    if (ddlAntimicrobials.attr("antimicrobials")) {
                        ddlAntimicrobials.val(ddlAntimicrobials.attr("antimicrobials").split(","));
                    }
                    ddlAntimicrobials.attr("antimicrobials", "");
                    ddlAntimicrobials.multiselect("refresh");
                    ddlAntimicrobials.multiselect("rebuild");
                }
            }
        });
        /*
        MDVisionService.lookups('GetAntimicrobialBySpecimentAndOrganism', true, data).done(function (results) {
            if (results["GetAntimicrobialBySpecimentAndOrganism"] != null) {
                results = JSON.parse(results["GetAntimicrobialBySpecimentAndOrganism"]);
            }
            if (results) {

                if (ddlAntimicrobials != null) {

                    ddlAntimicrobials.empty();
                    $.each(results, function (j, result) {
                        ddlAntimicrobials.append($("<option />").val(result.Value).text(result.Name).attr("RefValue", result.RefValue).attr("RefName", result.RefName));
                    });
                    ClinicalLabOrderDetail.InitializeMultiSelectDropDown(ddlAntimicrobials);
                    // ddlAntimicrobials.val(ddlAntimicrobials.attr("antimicrobials"));
                    // ddlAntimicrobials.attr("antimicrobials", "");
                }
            }
        });
        */
    },

    PoupulateLoction: function () {
        var shortName = $('#pnlClinicalLabOrderDetail #txtFacility').val();
        var facilityId = "";
        if (typeof $('#pnlClinicalLabOrderDetail #txtFacility').attr("facility") != typeof undefined) {
            facilityId = $('#pnlClinicalLabOrderDetail #txtFacility').attr("facility");
        }
        if (shortName != "" && facilityId > 0) {
            utility.GetFacilityArray(shortName).done(function (response) {
                $.each(response, function (i, item) {
                    if (item.value == shortName && item.id == facilityId) {
                        $('#' + ClinicalLabOrderDetail.params.PanelID + " #txtLocation").val(item.Location);
                    }
                });
                $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").data('serialize', $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail").serialize());
            });
        }
    },
}