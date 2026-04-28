ClinicalRadiologyOrderDetail = {
    params: [],
    bIsFirstLoad: true,
    EditableGrid: null,
    FavListName: "RadiologyOrderDetail",
    checkedProblems: [],
    loadProviderFromAppData: false,
    loadFacilityFromAppData: false,
    ProviderOnDemandLoad: false,
    FacilityOnDemandLoaded: false,
    isProviderLoaded: false,
    isFacilityLoaded: false,
    AddResult: false,
    AddedRadiologyOrderResultId: 0,
    Load: function (params) {
        BackgroundLoaderShow(true);

        ClinicalRadiologyOrderDetail.params = params;
        ClinicalRadiologyOrderDetail.AddResult = false;
        ClinicalRadiologyOrderDetail.AddedRadiologyOrderResultId = 0;
        ClinicalRadiologyOrderDetail.ddlSpecimen = [];
        ClinicalRadiologyOrderDetail.ddlAlternativeSpecimen = [];

        if (ClinicalRadiologyOrderDetail.params.ParentCtrlPanelID) {
            ClinicalRadiologyOrderDetail.params.PanelID = ClinicalRadiologyOrderDetail.params.ParentCtrlPanelID;
        } else {
            if (ClinicalRadiologyOrderDetail.params.PanelID != 'pnlClinicalRadiologyOrderDetail') {
                ClinicalRadiologyOrderDetail.params.PanelID = ClinicalRadiologyOrderDetail.params.PanelID + ' #pnlClinicalRadiologyOrderDetail';
            } else {
                ClinicalRadiologyOrderDetail.params.PanelID = 'pnlClinicalRadiologyOrderDetail';
            }
        }
        if (ClinicalRadiologyOrderDetail.params.ParentCtrl == "Clinical_Treatment") {
            $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #btnSignPrintOrder").addClass("hidden");
            $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #btnSignOrder").addClass("hidden");
        }
        //set patient id in hidden field
        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfPatientId").val($("div#PatientProfile #hfPatientId").val());
        if (Clinical_RadiologyOrder.params["ParentCtrl"] == "Clinical_FaceSheet") {
            $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfPatientId").val(Clinical_FaceSheet.params.patientID);
        }
        var self = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #divRadiologyOrderInformation");
        var resetButton = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #btnResetRadiologyOrder");

        if (parseInt(ClinicalRadiologyOrderDetail.params.RadiologyOrderId) > -1) {
            resetButton.hide();
        }
        else {
            resetButton.show();
        }
        var PanelRadiologyGrid = "#" + ClinicalRadiologyOrderDetail.params.PanelID + " #pnlRadiology_Result";
        var RadiologyGridId = "#" + ClinicalRadiologyOrderDetail.params.PanelID + " #dgvRadiology";
        $(RadiologyGridId + " tbody tr").remove();
        ClinicalRadiologyOrderDetail.EditableGrid = EMRUtility.MakeEditableGrid(PanelRadiologyGrid, RadiologyGridId, ClinicalRadiologyOrderDetail, "0", false, false, false, false);

        //Start//18-03-2016//Abid Ali //Load All Patient Insurances

        utility.CreateDatePicker(ClinicalRadiologyOrderDetail.params.PanelID + ' #frmClinicalRadiologyOrderDetail input[id="txtOrderDate"]', function () {
            //on-change callback method
        }, true);

        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #frmClinicalRadiologyOrderDetail input[id="txtOrderTime"]').timepicker({
            defaultTime: new Date()
        });

        //Start//18-03-2016//Abid Ali //Load All Patient Insurances
        if (Clinical_RadiologyOrder.params.ParentCtrl == 'clinicalTabProgressNote') {

        } else {
            //ClinicalRadiologyOrderDetail.loadAllAutocomplete();
        }
        //Start//23-03-2016//Ahmad Raza//Logic to select Guarantor and Relation
        ClinicalRadiologyOrderDetail.loadPatientGuarantor($('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfPatientId").val()).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false && response.GuarantorFill_JSON != "") {
                var obj = JSON.parse(response.GuarantorFill_JSON);

                ClinicalRadiologyOrderDetail.params.GuarantorName = obj.GuarantorName;
                ClinicalRadiologyOrderDetail.params.GuarantorId = obj.GuarantorId;

                ClinicalRadiologyOrderDetail.params.RelationName = response.relationName;
                ClinicalRadiologyOrderDetail.params.RelationId = response.relationshipId;

                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtGuarantor").val(obj.GuarantorName);
                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #frmClinicalRadiologyOrderDetail #txtRadiologyRelationShipId').val(response.relationName);
                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #frmClinicalRadiologyOrderDetail #ddlRelationShipId option[value="' + response.relationshipId + '"]').attr("selected", "selected");
                //   $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlRelationShipId').val(response.relationshipId);
                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #frmClinicalRadiologyOrderDetail #hfGuarantorId').val(obj.GuarantorId);

                if (response.relationshipId > 0)
                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #frmClinicalRadiologyOrderDetail #hfRalationId').val(response.relationshipId);
                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").data('serialize', $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").serialize());
            }
            //var isProblemAdded = false;
            //if (ClinicalRadiologyOrderDetail.params.isProblemAdded) {
            //    isProblemAdded = true;
            //} else {
            //    isProblemAdded = false;
            //}
            ////------------

            //ClinicalRadiologyOrderDetail.loadProblemList(isProblemAdded);
        });
        //End//23-03-2016//Ahmad Raza//Logic to select Guarantor and Relation

        ClinicalRadiologyOrderDetail.searchPatientInsurance($('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfPatientId").val()).done(function (response) {
            if (response.status != false) {
                var obj = JSON.parse(response.PatientInsuranceLoad_JSON);
                //Start//22-03-2016//Ahmad Raza//Logic to bind Insurance dropdowns on load
                var $ddlPrimary = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlPrimaryInsuraceId');
                var $ddlSecondary = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlSecondaryInsuraceId');
                var $ddlTertiary = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlTertiaryInsuraceId');

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
                    $ddlPrimary.append(
                        $('<option/>', {
                            value: item.InsuranceId,
                            html: item.InsurancePlanName,
                            Priority: item.PlanPriority
                        })
                    );
                });

                $.each(obj, function (i, item) {
                    $ddlSecondary.append(
                        $('<option/>', {
                            value: item.InsuranceId,
                            html: item.InsurancePlanName,
                            Priority: item.PlanPriority
                        })
                    );
                });

                $.each(obj, function (i, item) {
                    $ddlTertiary.append(
                        $('<option/>', {
                            value: item.InsuranceId,
                            html: item.InsurancePlanName,
                            Priority: item.PlanPriority
                        })
                    );
                });

                //------------------ Start insurance checks
                if (obj.length > 0) {
                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlBillingTypeId').val(3);
                }

                if ($('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlBillingTypeId').val() == 3) {

                    ClinicalRadiologyOrderDetail.enableDisableInsurances(false);
                    // ClinicalRadiologyOrderDetail.billingtype = 3;
                    var selectedVal1 = '';
                    var selectedVal2 = '';
                    var selectedVal3 = '';
                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlPrimaryInsuraceId option').each(function () {
                        if ($(this).attr('Priority') == "1") {
                            selectedVal1 = $(this).text();
                            $("#hfRadPrimaryInsId").val($(this).val());
                        }

                    });
                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlPrimaryInsuraceId').val(selectedVal1);

                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlSecondaryInsuraceId option').each(function () {
                        if ($(this).attr('Priority') == "2") {
                            selectedVal2 = $(this).text();
                            $("#hfRadSecondaryInsId").val($(this).val());
                        }
                    });
                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlSecondaryInsuraceId').val(selectedVal2);

                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlTertiaryInsuraceId option').each(function () {
                        if ($(this).attr('Priority') == "3") {
                            selectedVal3 = $(this).text();
                            $("#hfRadTertiaryInsId").val($(this).val());
                        }
                    });
                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlTertiaryInsuraceId').val(selectedVal3);
                }
                else {

                    ClinicalRadiologyOrderDetail.enableDisableInsurances(false);

                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlPrimaryInsuraceId').val('');
                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlSecondaryInsuraceId').val('');
                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlTertiaryInsuraceId').val('');

                }

                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").data('serialize', $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").serialize());


                //------------------ End insurance checks

            }
            else {

                utility.DisplayMessages(response.Message, 3);
            }
        });
        //End//18-03-2016//Abid Ali //Load All Patient Insurances


        // Set Title Explicitly if it's passed as Parameter
        if (ClinicalRadiologyOrderDetail.params.Title != null)
            $("#" + ClinicalRadiologyOrderDetail.params["PanelID"] + " #headingTitle").text(ClinicalRadiologyOrderDetail.params.Title);



        //var loadDropDowns = function () {

        //    var dfd = $.Deferred();
        //ClinicalRadiologyOrderDetail.favoriteListSearch();

        //Start 02-03-2016 Humaira Yousaf to load dropdowns


        if (ClinicalRadiologyOrderDetail.bIsFirstLoad == true) {


            ClinicalRadiologyOrderDetail.validateRadiologyOrderDetail();
            ClinicalRadiologyOrderDetail.bIsFirstLoad = false;

            //self.loadDropDowns(true).done(function () {
            //    dfd.resolve();
            //});
        }
        //else {
        //    dfd.resolve();
        //}
        //End 02-03-2016 Humaira Yousaf to load dropdowns

        //CacheManager.BindDropDownsByID($('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #frmClinicalRadiologyOrderDetail #ddlAssigneeId'), 'GetUsersFullName', true, 1).done(function () {
        //    $("#" + ClinicalRadiologyOrderDetail.params.PanelID + " #ddlAssigneeId option:first").text('- Select -');
        //});

        //    dfd.promise();
        //}
        //var loadOrder = function () {
        //Start 13-04-2016 Abid Ali to load Laboratories


        Clinical_LabOrder.LoadLabs('ddlLabId', ClinicalRadiologyOrderDetail.params.PanelID).done(function () {

            if (ClinicalRadiologyOrderDetail.params.mode == "Add") {
                if ($("#" + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlLabId').val() != "") {
                    $("#" + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlLabId').trigger('change');
                }
            }

            //------------
            var isProblemAdded = false;
            if (ClinicalRadiologyOrderDetail.params.isProblemAdded) {
                isProblemAdded = true;
            } else {
                isProblemAdded = false;
            }
            //------------
            self.loadDropDowns(true).done(function () {
                ClinicalRadiologyOrderDetail.loadProblemList(isProblemAdded, true);
                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").data('serialize', $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").serialize());
            });


        });


        //End 13-04-2016 Abid Ali to load Laboratories
        //}
        //$.when(loadDropDowns()).done(loadOrder());

        ClinicalRadiologyOrderDetail.Init();
        //Check buttons
        ClinicalRadiologyOrderDetail.enableDisableRadiologyOrderButtons();
        ClinicalRadiologyOrderDetail.BindFacility();
        ClinicalRadiologyOrderDetail.BindProvider();
        ClinicalRadiologyOrderDetail.BindFacilityTo();
        ClinicalRadiologyOrderDetail.BindAssignee();
        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #txtProvider").focus(function () {

            if (!ClinicalRadiologyOrderDetail.isProviderLoaded)
                ClinicalRadiologyOrderDetail.ProviderOnDemandLoad = true;
            else
                ClinicalRadiologyOrderDetail.ProviderOnDemandLoad = false;

            ClinicalRadiologyOrderDetail.loadAllAutocomplete();
        });
        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #txtFacility").focus(function () {
            if (!ClinicalRadiologyOrderDetail.isFacilityLoaded)
                ClinicalRadiologyOrderDetail.FacilityOnDemandLoad = true;
            else
                ClinicalRadiologyOrderDetail.FacilityOnDemandLoad = false;

            ClinicalRadiologyOrderDetail.loadAllAutocomplete();
        });

        $('#frmClinicalRadiologyOrderDetail #txtProvider').blur(function () {
            if ($('#frmClinicalRadiologyOrderDetail #txtProvider').val().length > 0) {
                ClinicalRadiologyOrderDetail.EnableDisableTestSearch();
            }
        });
        ClinicalRadiologyOrderDetail.documentReady();
    },
    documentReady: function () {
        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #txtComment').on("click", function (e) {
            if (!$('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #MacroDescDetails').is(":hidden")) {
                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #MacroDescDetails').hide();
            }
        });
        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #txtComment').on("keyup", function (e) {

            if (e.keyCode == 190 || e.keyCode == 110) // dot key is pressed
            {
                e.preventDefault();
                if ($('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #txtComment').find("#marker").length > 0) {
                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #txtComment').find("#marker").remove();
                }
                EMRUtility.pasteHtmlAtCaret('<span id=marker></span>');
                if (EMRUtility.callAutopopulationOrNot(ClinicalRadiologyOrderDetail.params.PanelID, "txtComment")) {
                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #txtComment').focus();
                    EMRUtility.AutoKeyWordPopulateForDiv(ClinicalRadiologyOrderDetail.params.PanelID, "txtComment", "Diagnostic Imaging Order", 0);
                }
                else {
                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #txtComment').find("#marker").remove();
                }

            }
        });
    },
    BindAssignee: function () {
        var Ctrl = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #ddlAssigneeId");
        var hfCtrl = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfAssignee");
        var func = function () { return ClinicalRadiologyOrderDetail.GetUserArray(Ctrl.val()) };
        var onSelect = function (dataItem) { $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #ddlAssigneeId").attr("Assignee", dataItem.id); }
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect);
    },
    LoadUserNameDBCall: function (userName) {

        var objData = new Object();
        objData["UserName"] = userName;
        objData["commandType"] = "get_users";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyOrder", "RadiologyOrder");
    },
    GetUserArray: function (userName) {

        var AllUsers = [];
        var IsValid = false;

        if (userName != null && userName.length > 1)
            IsValid = true;
        else
            IsValid = false;

        var dfd = new $.Deferred();
        if (IsValid) {

            ClinicalRadiologyOrderDetail.LoadUserNameDBCall(userName).done(function (responseData) {
                responseData = JSON.parse(responseData);
                if (responseData.status != false) {
                    if (responseData.UsersCount > 0) {
                        var UsersLoadJSONData = JSON.parse(responseData.UsersLoad_JSON);
                        $.each(UsersLoadJSONData, function (i, item) {

                            AllUsers.push({ id: item.UserId, value: item.UserName });

                        });
                    }
                }
                dfd.resolve(AllUsers);
            });
        }
        else {
            dfd.resolve(AllUsers);
        }

        return dfd.promise();

    },
    BindFacility: function () {
        var Ctrl = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtFacility");
        var hfCtrl = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfFacility");
        var func = function () { return utility.GetFacilityArray(Ctrl.val()) };
        var onSelect = function (dataItem) { $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtFacility").attr("Facility", dataItem.id); }
        var onChange = function (valid) { ClinicalRadiologyOrderDetail.EnableDisableTestSearch(); }
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect, onChange);
    },
    BindProvider: function () {
        var Ctrl = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtProvider");
        var hfCtrl = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfProvider");
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        var onSelect = function (dataItem) {
            $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtProvider").attr("Provider", dataItem.id);
            $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtFacilityTo").val("");
            $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfFacilityTo").val("");
        }
        var onChange = function (valid) { ClinicalRadiologyOrderDetail.EnableDisableTestSearch(); }
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect, onChange);
    },
    ValidateProvider: function (obj) {
        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtFacilityTo").val("");
        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfFacilityTo").val("");
        utility.ValidateAutoComplete(obj, 'frmClinicalNotes #hfProvider', true);
    },
    //Author: Abid Ali
    //Date :  07-03-2016
    //Reason: Clear/Set gurantor/relation values
    setGurantorRelationValues: function (setValues) {

        var guarantorName = ClinicalRadiologyOrderDetail.params.GuarantorName;
        var guarantorId = ClinicalRadiologyOrderDetail.params.GuarantorId;
        var relationName = ClinicalRadiologyOrderDetail.params.RelationName;
        var relationId = ClinicalRadiologyOrderDetail.params.RelationId;

        var self = $('#' + ClinicalRadiologyOrderDetail.params.PanelID);

        if (setValues) {
            self.find("#txtGuarantor").val(guarantorName);
            self.find('#hfGuarantorId').val(guarantorId);
            if ($('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlBillingTypeId').val() == 3) {
                self.find('#txtRadiologyRelationShipId').val(relationName);
                if (relationId > 0)
                    self.find('#hfRalationId').val(relationId);
                else
                    self.find('#hfRalationId').val("");
            }
            else {
                self.find('#txtRadiologyRelationShipId').val("");
                self.find('#hfRalationId').val("");
            }
        }
        else {
            self.find("#txtGuarantor").val("");
            self.find('#hfGuarantorId').val("");
            self.find('#txtRadiologyRelationShipId').val("");
            self.find('#hfRalationId').val("");
        }
    },

    //Author: Muhammad Arshad
    //Date :  21-04-2016
    //This function will handle Add/Edit of RadiologyResult
    RadiologyResultAddEdit: function (RadiologyResultId, RadiologyOrderId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Orders and Results_Diagnostic Imaging Order", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                if (RadiologyResultId != null && parseInt(RadiologyResultId) > 0) {
                    params["RadiologyResultId"] = RadiologyResultId;
                    params["mode"] = "Edit";
                }
                else {
                    params["RadiologyResultId"] = -1;
                    params["mode"] = "Add";
                }
                params["RadiologyOrderId"] = ClinicalRadiologyOrderDetail.params["RadiologyOrderId"];
                params["FromAdmin"] = ClinicalRadiologyOrderDetail.params["FromAdmin"];
                params["ParentCtrl"] = 'ClinicalRadiologyOrderDetail';
                LoadActionPan('ClinicalRadiologyResultDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    //Author: Abid Ali
    //Date :  06-05-2016
    //Reason: Reset The Entire form
    radiologyOrderDetailReset: function () {

        utility.myConfirm('22', function () {

            //selectors
            var form = "#" + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail";
            var $problems = $(form + " #divRadiologyAssociatedProblems");
            // var $testInformation = $(form + ' #divTestInformation');
            var $billingInformation = $(form + " #divRadiologyBillingInformation");
            var $orderInfomation = $(form + " #divRadiologyOrderInformation");

            $problems.resetAllControls(null);
            //  $testInformation.resetAllControls(null);
            $billingInformation.resetAllControls(null);
            $orderInfomation.resetAllControls(null);

            //Clear and draw the data table
            ClinicalRadiologyOrderDetail.EditableGrid.datatable.clear().draw();

            //Disable Radiology Order Detail button.
            ClinicalRadiologyOrderDetail.enableDisableRadiologyOrderButtons();

            //revalidate the required fields
            $(form).bootstrapValidator('revalidateField', 'LabId').bootstrapValidator('revalidateField', 'Provider').bootstrapValidator('revalidateField', 'Facility');

        }, function () { },
            '22'
        );
    },


    //Author: Muhammad Arshad
    //Date :  25-03-2016
    //Reason: Function to disable Radiology Order Controls
    disableRadiologyOrderControls: function (IsSigned) {
        var detailsDivs = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #divRadiologyOrderInformation,#divRadiologyBillingInformation,#divTestInformation,#divRadiologyAssociatedProblems,#divRadiologyComments");
        var detailsButtons = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #btnSaveOrder,#btnSignPrintOrder,#btnSignOrder,#btnResetRadiologyOrder");
        if (ClinicalRadiologyOrderDetail.params.ParentCtrl == "Clinical_Treatment") {
            detailsButtons = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #btnSaveOrder,#btnResetRadiologyOrder");
        }
        var printRequisitionButton = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #btnPrintOrder");
        var addResultButton = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #btnAddRadiologyResult");
        if (IsSigned == true) {
            detailsDivs.addClass("disableAll");
            detailsButtons.addClass("hidden");
            printRequisitionButton.removeClass("hidden");
            addResultButton.removeClass("hidden");
        }
        else {
            detailsDivs.removeClass("disableAll");
            detailsButtons.removeClass("hidden");
            printRequisitionButton.addClass("hidden");
            addResultButton.addClass("hidden");
        }
    },

    enableDisableRadiologyOrderButtons: function () {

        var isDisable = $("#" + ClinicalRadiologyOrderDetail.params.PanelID + " #dgvRadiology tbody tr").length > 0 && $("#" + ClinicalRadiologyOrderDetail.params.PanelID + " #dgvRadiology tbody tr").find('.dataTables_empty').length == 0 ? false : true;

        var detailsButtons = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #btnSaveOrder,#btnSignPrintOrder,#btnSignOrder,#btnResetRadiologyOrder");
        if (isDisable) {
            detailsButtons.addClass("disableAll");
        }
        else {
            detailsButtons.removeClass("disableAll");
        }
    },

    //Author: Muhammad Arshad
    //Date :  24-03-2016
    //Reason: Function to load procedure Order Fav List
    favoriteListSearch: function () {
        ClinicalRadiologyOrderDetail.searchFavoriteList_DBCall("RadiologyOrder", null, 1, 5000,true).done(function (response) {

            response = JSON.parse(response);

            var $ddl = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlFavoriteListRadiologyOrder');
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
                          }));
                    }
                });
                if (favouriteProcedures.length > 0) {
                    EMRUtility.getFavListValue(ClinicalRadiologyOrderDetail.FavListName).done(function (response1) {
                        response1 = JSON.parse(response1);
                        if (response1.status != false) {
                            if (response1.favListVal != "" && response1.favListVal != "-1") {
                                if ($("#" + ClinicalRadiologyOrderDetail.params.PanelID + " #ddlFavoriteListRadiologyOrder option[value='" + response1.favListVal + "']").length > 0) {
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
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                } else {
                    $ddl.trigger("onchange");
                }
            }
        });

    },

    searchFavoriteList_DBCall: function (ListType, FavoriteListId, PageNumber, RowsPerPage, IsSelectForLookUp) {
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
        objData["LabId"] = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlLabId option:selected').val();
        objData["ProviderId"] = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #hfProvider').val();

        if (Favorite_RadiologyOrder.Switch == 1) {
            objData["IsActive"] = true
        }
        else {
            objData["IsActive"] = false;
        }
        objData["IsSelectForLookUp"] = IsSelectForLookUp;

        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;

        objData["commandType"] = "load_favoritelist";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },

    //Author: Muhammad Arshad
    //Date :  25-03-2016
    //Reason: Function to load procedure Order Fav List Test while selection is done
    selectAllFavoriteListContent: function () {

        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ulFavoriteListRadiologyOrderContent li').each(function (i, item) {
            var cptCode = $(item).attr('CPTCode')
            var procDesc = $(item).attr('procedureDescription');
            var cptDescription = $(item).attr('cptDescription');

            var isTestAlreadySelected = ClinicalRadiologyOrderDetail.isTestExists(procDesc);

            if (isTestAlreadySelected != true) {
                ClinicalRadiologyOrderDetail.AddNewRadiologyRow(null, null, null, cptCode, procDesc, cptDescription);
            }
            else {
                utility.DisplayMessages("Test is already selected", 2);
                // return false;
            }
        });

    },
    AutoSearchFavRadiologyOrder: function () {
        utility.Keyupdelay(function () {
            ClinicalRadiologyOrderDetail.loadfavoriteListContent();
        });
    },
    //Author: Muhammad Arshad
    //Date :  24-03-2016
    //Reason: Function to load procedure Order Fav List Test while selection is done
    loadfavoriteListContent: function (obj, favListIds) {
        if ((typeof favListIds == typeof undefined || favListIds == null) && (typeof obj == typeof undefined || obj == null)) {
            obj = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlFavoriteListRadiologyOrder');
        }
        var SearchData = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #FavSearchBox').val();
        var $uL = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ulFavoriteListRadiologyOrderContent');
        if (obj != null) {
            var selectedOption = $(obj).find("option:selected");
            var selectedOptionValue = selectedOption.attr("id");
            //Start 12-07-2016 Muhammad Arshad Bug#EMR-1486 Lab order -> Favorite list -> "Select ALL " functionality is not working on 46 server ,please see attached video and resolve the issue
            var divSelectAllFavorites = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #divSelectAllRadiologyOrderFavorites');
            if (selectedOptionValue > 0) {
                divSelectAllFavorites.removeClass("disableAll");

                ClinicalRadiologyOrderDetail.favoriteList_CPTSearch(selectedOptionValue, null, SearchData);
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
                // var $uL = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ulFavoriteListRadiologyOrderContent');
                $uL.empty();

                $.each(favListIds, function (index, item) {
                    //load all favList
                    ClinicalRadiologyOrderDetail.favoriteList_CPTSearch(item, false);
                });
            }
        }
    },

    //Author: Muhammad Arshad
    //Date :  24-03-2016
    //Reason: Function to load procedure Order Fav List
    favoriteList_CPTSearch: function (FavoriteListId, isEmptyUl, SearchData) {
        var $uL = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ulFavoriteListRadiologyOrderContent');
        Favorite_ProcedureOrder.searchFavoriteList_CPT_DBCall(null, FavoriteListId, 1, 5000, SearchData).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                var objData = JSON.parse(response.FavoriteListCPTJSON);

                isEmptyUl = isEmptyUl == null ? true : false;
                if (isEmptyUl) {
                    $uL.empty();
                }
                $.each(objData, function (i, item) {
                    if (item.CPTCodeDescription != "") {
                        var onclick = 'ClinicalRadiologyOrderDetail.BindProcedureGridItem(\'' + item.CPTCode + '\',\'' + String(item.CPTCodeDescription) + '\',\'' + String(item.CPTCodeDescription) + '\')';
                        var $li = $(document.createElement('li'));
                        $li.attr('onclick', onclick);
                        $li.attr('CPTCode', item.CPTCode);
                        $li.attr('procedureDescription', String(item.CPTCodeDescription));
                        $li.attr('cptDescription', String(item.CPTCodeDescription));
                        $li.append(item.CPTCode + " " + item.CPTCodeDescription);
                        $uL.append($li);
                    }
                });


                //Favorite_ProcedureOrder.FavoriteListCPTGridLoad(response);
            }
            else {
                isEmptyUl = isEmptyUl == null ? true : false;
                if (isEmptyUl) {
                    $uL.empty();
                }
            }
            //else {
            //    utility.DisplayMessages(response.Message, 3);
            //}
        });
    },

    //Author: Abid Ali
    //Date :  17-03-2016
    //Reason: Bind Guaranter
    bindGuarantor: function () {
        var shortName = $('#pnlClinicalRadiologyOrderDetail #txtGuarantor').val();
        utility.GetGuarontorArray(shortName).done(function (response) {

            $('#pnlClinicalRadiologyOrderDetail #txtGuarantor').autocomplete({
                //source: AllPatients, // pass an array (without a comma)
                autoFocus: true,
                source: response,
                select: function (event, ui) {

                    setTimeout(function () {

                        $("#pnlClinicalRadiologyOrderDetail #hfRadiologyGuarantorId").val(ui.item.id); // add the selected id
                        if ($("#pnlClinicalRadiologyOrderDetail #lnkGuarantorEdit").css("display") == "none") {
                            $("#pnlClinicalRadiologyOrderDetail #lnkGuarantorEdit").css("display", "inline");
                            $("#pnlClinicalRadiologyOrderDetail #lblGuarantor").css("display", "none");
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
        params["ParentCtrl"] = "ClinicalRadiologyOrderDetail";
        params["RefCtrl"] = "txtGuarantor";
        LoadActionPan('Patient_Guarantor', params);
    },

    //Author: Abid Ali
    //Date :  17-03-2016
    //Reason: open Guaranter
    openGuarantorDetail: function () {
        //Patient_Guarantor.GuarantorEdit($('#pnlDemographic #hfGuarantor').val(), 'patTabDemographic');
        var params = [];
        params["GuarantorId"] = $('#pnlClinicalRadiologyOrderDetail #hfRadiologyGuarantorId').val();
        params["mode"] = "Edit";
        params["RefCtrl"] = "txtGuarantor";
        params["ParentCtrl"] = 'ClinicalRadiologyOrderDetail';
        LoadActionPan('guarantorDetail', params);
    },

    //Author: Abid Ali
    //Date :  17-03-2016
    //Reason: Load auto complete for provider and Facility
    loadAllAutocomplete: function () {

        if (ClinicalRadiologyOrderDetail.loadProviderFromAppData && ClinicalRadiologyOrderDetail.ProviderOnDemandLoad) {
            if (globalAppdata.DefaultProviderName != "" && globalAppdata.DefaultProviderName != "- Select -") {

                CacheManager.BindCodes('GetProvider', false).done(function (result) {
                    var Ctrl = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtProvider");
                    var hfCtrl = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfProvider");
                    var onSelect = function (dataItem) { $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtProvider").attr("Provider", dataItem.id); }
                    var onChange = function () { ClinicalRadiologyOrderDetail.EnableDisableTestSearch(); }
                    utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, onSelect, onChange);

                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").data('serialize', $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").serialize());
                });
                ClinicalRadiologyOrderDetail.loadProviderFromAppData = true;
                ClinicalRadiologyOrderDetail.ProviderOnDemandLoad = false;
                ClinicalRadiologyOrderDetail.isProviderLoaded = true;
            }
        }

        if (ClinicalRadiologyOrderDetail.loadFacilityFromAppData && ClinicalRadiologyOrderDetail.FacilityOnDemandLoad) {
            if (globalAppdata.DefaultFacilityName != "" && globalAppdata.DefaultFacilityName != "- Select -") {

                CacheManager.BindCodes('GetFacility', false).done(function (result) {
                    var Ctrl = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtFacility");
                    var hfCtrl = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfFacility");
                    var onSelect = function (dataItem) { $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtFacility").attr("Facility", dataItem.id); }
                    utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl, onSelect);

                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").data('serialize', $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").serialize());
                });
                ClinicalRadiologyOrderDetail.loadFacilityFromAppData = true;
                ClinicalRadiologyOrderDetail.FacilityOnDemandLoad = false;
                ClinicalRadiologyOrderDetail.isFacilityLoaded = true;
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
            ClinicalRadiologyOrderDetail.enableDisableDropdownList(ddlIds, true);
        else {
            ddlIds.push('ddlPrimaryInsuraceId');
            ddlIds.push('ddlSecondaryInsuraceId');
            ddlIds.push('ddlTertiaryInsuraceId');
            ClinicalRadiologyOrderDetail.enableDisableDropdownList(ddlIds, false);
        }
    },

    //Author: Abid Ali
    //Date :  17-03-2016
    //Reason: Enable disable dropdownlists
    enableDisableDropdownList: function (listOfDdlIds, isHide) {

        $.each(listOfDdlIds, function (index, item) {
            if (isHide) {
                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #' + item).prop('disabled', true);
            }
            else {
                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #' + item).removeClass('disabled', false);
            }
        });
    },

    //Author: Abid Ali
    //Date :  17-03-2016
    //Reason: Populate patient insurances in ddl
    populateInsuranceDropDownList: function (ddlTypeId, options) {
        var $ddl = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #' + ddlTypeId);

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

    //Author: Farooq Ahmad
    //Date :  17-03-2016
    //Reason: Function to load problem list
    loadProblemList: function (fromProblemListSave, IsRadiologyOrderLoad) {
        $("#" + ClinicalRadiologyOrderDetail.params.PanelID + ' #ulProblemLists input[type=checkbox]:checked').each(function (i, item) {
            if ($(item).attr("id") != "chkSelectAll") {
                ClinicalRadiologyOrderDetail.pushProblems(item);
            }
        });
        ClinicalRadiologyOrderDetail.SearchProblemList().done(function (response) {
            var obj = JSON.parse(response);
            if (obj.status != false) {
                var loadProblems = function () {
                    if (obj.ProblemListCount > 0) {
                        var ProblemListLoadJSONData = JSON.parse(obj.ProblemListLoad_JSON);
                        var ProblemListHistoryLoadJSONData = JSON.parse(obj.ProblemListHistoryLoad_JSON);
                        var finalTr = '';
                        var counter = 2;
                        $("#" + ClinicalRadiologyOrderDetail.params.PanelID + " #ulProblemLists tbody tr").remove();
                        $.each(ProblemListLoadJSONData, function (i, item) {
                            if (item.Description.split(" - ")[2] != undefined) {
                                item.Description = item.Description.substring(item.Description.indexOf("-") + 2);
                            }
                            if (counter % 2 == 0) {
                                finalTr = finalTr + '<tr>';
                            }
                            if (counter == 2) {
                                finalTr = finalTr + '<td><div class="col-xs-12 p-xs"><div class="checkbox-custom">';
                                finalTr = finalTr + '<input type="checkbox" onclick ="ClinicalRadiologyOrderDetail.selectAllProblems(this);"  id="chkSelectAll">';

                                finalTr = finalTr + '<label for="chkSelectAll" class="control-label">Select All</label></div></div></td>';
                                counter++;
                            }


                            finalTr = finalTr + '<td><div class="p-xs col-xs-12"><div class="checkbox-custom">';

                            //$(finalTr).find('input[id*="chk"]').trigger('click');

                            //Start//Abid Ali//For bug# EMR-1590
                            var checked = ClinicalRadiologyOrderDetail.isCheckedProblem('chk' + item.ProblemListId + '') ? "checked" : "";
                            //if (checked == "") {
                            //    if (ClinicalRadiologyOrderDetail.params.mode == "Add") {
                            //        var Notes = item.NoteId.split(',');
                            //        if ($.inArray('' + ClinicalRadiologyOrderDetail.params.FromNoteId + '', Notes) != -1) {
                            //            checked = "checked";
                            //            fromProblemListSave = false;
                            //        }
                            //    }
                            //}

                            finalTr = finalTr + '<input ' + checked + ' type="checkbox" onclick ="ClinicalRadiologyOrderDetail.pushProblems(this); ClinicalRadiologyOrderDetail.checkOrUncheckSelectAll();" name="' + item.Code + '" value="' + item.ProblemListId + '"  id="chk' + item.ProblemListId + '" class="chkProblem">';
                            //End//Abid Ali//For bug# EMR-1590

                            finalTr = finalTr + '   <label for="chk' + item.ProblemListId + '" class="control-label">' + item.Description + '</label></div></div></td>';

                            if (counter % 2 == 1) {
                                finalTr = finalTr + '</tr>';
                            }
                            counter++;
                            var color = "";
                        });
                        $("#" + ClinicalRadiologyOrderDetail.params.PanelID + " #ulProblemLists tbody").append(finalTr);
                        //-------------
                        //if (ClinicalRadiologyOrderDetail.params.mode == "Edit") {
                        //    fromProblemListSave = false;
                        //}
                        if (Clinical_ProgressNote.params.newlyAddedProblemLists && Clinical_ProgressNote.params.newlyAddedProblemLists.length > 0) {
                            $.each(Clinical_ProgressNote.params.newlyAddedProblemLists, function (i, item) {
                                $("#" + ClinicalRadiologyOrderDetail.params.PanelID + ' #ulProblemLists input[type=checkbox]').each(function () {
                                    if (item == $(this).attr('value') && !($(this).attr('checked'))) {
                                        $("#" + ClinicalRadiologyOrderDetail.params.PanelID + ' #ulProblemLists' + ' #chk' + item).trigger('click');
                                    }
                                    //console.log(item);
                                    //console.log($(this).attr('value'));
                                });
                            });
                        } else {

                            if (fromProblemListSave) {
                                var _val = 0;
                                var _id = "";
                                $("#" + ClinicalRadiologyOrderDetail.params.PanelID + ' #ulProblemLists input[type=checkbox]').each(function () {
                                    if (parseInt($(this).attr('value')) > _val) {
                                        _val = $(this).attr('value');
                                        _id = $(this).attr('id');
                                    }
                                });
                                $("#" + ClinicalRadiologyOrderDetail.params.PanelID + ' #ulProblemLists' + ' #' + _id).trigger('click');
                            }
                        }
                        //-----------
                    }
                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").data('serialize', $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").serialize());
                }
                var loadOrder = function () {

                    if (IsRadiologyOrderLoad) {
                        var radiologyOrderId = ClinicalRadiologyOrderDetail.params.RadiologyOrderId;
                        radiologyOrderId = typeof radiologyOrderId != 'undefined' && radiologyOrderId > 0 ? radiologyOrderId : -1
                        ClinicalRadiologyOrderDetail.loadRadiologyOrder(radiologyOrderId);
                        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfRadiologyOrderId").val(radiologyOrderId);
                    }
                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").data('serialize', $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").serialize());
                }
                $.when(loadProblems()).then(loadOrder());
                ClinicalRadiologyOrderDetail.checkOrUncheckSelectAll();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    checkOrUncheckSelectAll: function () {
        var anyUncheck = false
        $.each($('#' + ClinicalRadiologyOrderDetail.params.PanelID).find(".chkProblem"), function (i, item) {
            if (!$(item).prop('checked')) {
                anyUncheck = true;
            }
        })
        if (anyUncheck) {
            $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #chkSelectAll").prop('checked', false);
        }
        else {
            $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #chkSelectAll").prop('checked', true);
        }
    },

    selectAllProblems: function (obj) {
        if ($(obj).prop('checked')) {
            $.each($('#' + ClinicalRadiologyOrderDetail.params.PanelID).find(".chkProblem"), function (i, item) {
                $(item).prop('checked', true)
                var id = $(item).attr('id');
                if (!($.inArray(id, ClinicalRadiologyOrderDetail.checkedProblems) != -1)) {
                    ClinicalRadiologyOrderDetail.checkedProblems.push(id);
                }

            });
        }
        else {

            $.each($('#' + ClinicalRadiologyOrderDetail.params.PanelID).find(".chkProblem"), function (i, item) {
                $(item).prop('checked', false)
                var id = $(item).attr('id');
                ClinicalRadiologyOrderDetail.checkedProblems.splice($.inArray(id, ClinicalRadiologyOrderDetail.checkedProblems), 1);
            });
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
            if (!($.inArray(id, ClinicalRadiologyOrderDetail.checkedProblems) != -1)) {
                ClinicalRadiologyOrderDetail.checkedProblems.push(id);
            }
        }
        else {
            //Remove from checked problems
            ClinicalRadiologyOrderDetail.checkedProblems.splice($.inArray(id, ClinicalRadiologyOrderDetail.checkedProblems), 1);
        }

    },
    //Author: Abid Ali
    //Date :  15-07-2016
    //Reason: function to check problem in array
    isCheckedProblem: function (problemId) {

        return $.inArray(problemId, ClinicalRadiologyOrderDetail.checkedProblems) == -1 ? false : this;

    },
    //Author: Abid Ali
    //Date :  17-03-2016
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
            patientID = Clinical_RadiologyOrder.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        }
        var objData = new Object();
        objData["PatientId"] = patientID;

        objData["commandType"] = "SEARCH_Guarantor";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyOrder", "RadiologyOrder");
    },

    //Author: Abid Ali
    //Date :  21-03-2016
    //Reason: Edit radiologyOrder
    radiologyEdit: function (radiologyOrderId, event) {
        //if icon is clicked then  popup the window

        ClinicalRadiologyOrderDetail.loadProviderFromAppData = true;
        ClinicalRadiologyOrderDetail.loadFacilityFromAppData = true;

        Clinical_RadiologyOrder.RadiologyOrderAddEdit(radiologyOrderId);
        event.stopPropagation();

    },

    //Author: Farooq Ahmad
    //Date :  17-03-2016
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
        objData["PatientId"] = Clinical_RadiologyOrder.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
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
    //Date :  17-03-2016
    //Reason: open provider form
    OpenProvider: function () {
        //if ($('#pnlClinicalRadiologyOrderDetail #txtProvider').hasClass('ui-autocomplete-input')) {
        //    $('#pnlClinicalRadiologyOrderDetail #txtProvider').autocomplete("destroy");
        //}
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmClinicalRadiologyOrderDetail";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ClinicalRadiologyOrderDetail";
        LoadActionPan('Admin_Provider', params);
    },

    //Author: Abid Ali
    //Date :  17-03-2016
    //Reason: open provider detail
    OpenProviderDetail: function () {
        //Admin_Provider.ProviderEdit($('#pnlClinicalNotes #hfProvider').val(),'clinicalTabNotes');
        var params = [];
        params["ProviderId"] = $('#pnlClinicalRadiologyOrderDetail #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'ClinicalRadiologyOrderDetail';
        LoadActionPan('providerDetail', params);
    },

    //Author: Farooq Ahmad
    //Date :  17-03-2016
    //Reason: open facility form
    OpenFacility: function () {
        if ($('#pnlClinicalRadiologyOrderDetail #txtFacility').hasClass('ui-autocomplete-input')) {
            $('#pnlClinicalRadiologyOrderDetail #txtFacility').autocomplete("destroy");
        }
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmClinicalRadiologyOrderDetail";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ClinicalRadiologyOrderDetail";
        LoadActionPan('Admin_Facility', params);
    },

    //Author: Farooq Ahmad
    //Date :  17-03-2016
    //Reason: open facility detail form
    OpenFacilityDetail: function () {
        //Admin_Facility.FacilityEdit($('#' + demographicDetail.params.PanelID + ' #hfFacility').val(), "demographicDetail");
        var params = [];
        params["FacilityId"] = $('#pnlClinicalRadiologyOrderDetail #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacility";
        params["ParentCtrl"] = 'ClinicalRadiologyOrderDetail';
        LoadActionPan('facilityDetail', params);
    },

    HideProviderLink: function () {
        $('#pnlClinicalRadiologyOrderDetail #txtProvider').attr("ProviderId", "-1");
        $('#pnlClinicalRadiologyOrderDetail #hfProvider').val("-1");
        $("#pnlClinicalRadiologyOrderDetail #lnkProviderEdit").css("display", "none");
        $("#pnlClinicalRadiologyOrderDetail #lblProvider").css("display", "inline");
    },
    // -------------- End Provider -----------------

    //Author: Ahmad Raza
    //Date :  04-03-2016
    //Reason: Function to apply bootstrap validations
    validateRadiologyOrderDetail: function () {
        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail")
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

                var $form = $(e.target);
                var $button = $form.data('bootstrapValidator').getSubmitButton();
                switch ($button.attr('id')) {
                    //Start 28-11-2016 Humaira Yousaf for associated problems validation

                    // MK: http://192.168.0.85:8080/browse/IMP-482
                    // Problems won't be mandatory

                    case 'btnSaveOrder':
                        var isProblemAdded = ClinicalRadiologyOrderDetail.problemAdded();
                        if (isProblemAdded == true) {
                            ClinicalRadiologyOrderDetail.RadiologyOrderSave('save');
                        }
                        else {
                            utility.kendoConfirm('Confirm Save', 'No ICD code is associated with this order. Do you wish to continue?', function () {
                                if ($("#donotaskedagain:checked").length > 0) {
                                    utility.AddKey2LocalStorageForDoNotAskedAgain();
                                }
                                ClinicalRadiologyOrderDetail.RadiologyOrderSave('save');
                            });
                        }
                        //else {
                        //    utility.DisplayMessages("Associate at least one Problem with the order.", 3);
                        //}
                        break;
                    case 'btnSignOrder':
                        var isProblemAdded = ClinicalRadiologyOrderDetail.problemAdded();
                        if (isProblemAdded == true) {
                            ClinicalRadiologyOrderDetail.RadiologyOrderSave('signorder');
                            ClinicalRadiologyOrderDetail.disableRadiologyOrderControls(true);
                        } else {
                            utility.kendoConfirm('Confirm Sign', 'No ICD code is associated with this order. Do you wish to continue?', function () {
                                if ($("#donotaskedagain:checked").length > 0) {
                                    utility.AddKey2LocalStorageForDoNotAskedAgain();
                                }
                                ClinicalRadiologyOrderDetail.RadiologyOrderSave('signorder');
                                ClinicalRadiologyOrderDetail.disableRadiologyOrderControls(true);
                            });
                        }
                        //else {
                        //    utility.DisplayMessages("Associate at least one Problem with the order.", 3);
                        //}
                        break;
                    case 'btnPrintOrder':
                        var mode = ClinicalRadiologyOrderDetail.params.mode;
                        var isIncludeComments = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #chkIncludeComments").prop("checked");
                        ClinicalRadiologyOrderDetail.radiologyOrderPreview((Clinical_RadiologyOrder.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val()), globalAppdata['AppUserId'], $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfRadiologyOrderId").val(), mode, isIncludeComments);
                        //ClinicalRadiologyOrderDetail.printRadiologyOrder();
                        //ClinicalRadiologyOrderDetail.disableRadiologyOrderControls(true);
                        break;
                    case 'btnSignPrintOrder':
                        var isProblemAdded = ClinicalRadiologyOrderDetail.problemAdded();
                        if (isProblemAdded == true) {
                            ClinicalRadiologyOrderDetail.RadiologyOrderSave('signprintorder');
                            ClinicalRadiologyOrderDetail.disableRadiologyOrderControls(true);
                        } else {
                            utility.kendoConfirm('Confirm Sign', 'No ICD code is associated with this order. Do you wish to continue?', function () {
                                if ($("#donotaskedagain:checked").length > 0) {
                                    utility.AddKey2LocalStorageForDoNotAskedAgain();
                                }
                                ClinicalRadiologyOrderDetail.RadiologyOrderSave('signprintorder');
                                ClinicalRadiologyOrderDetail.disableRadiologyOrderControls(true);
                            });
                        }
                        //else {
                        //utility.DisplayMessages("Associate at least one Problem with the order.", 3);
                        //}
                        break;
                    case 'btnAddRadiologyResult':
                        Clinical_RadiologyOrder.radiologyResultAddEdit(-1, ClinicalRadiologyOrderDetail.params.RadiologyOrderId, true, "AddResult");
                        break;
                    case 'btnEditLabResult':
                        break;
                    default:
                        if ($("#" + ClinicalRadiologyOrderDetail.params.PanelID + " #dgvRadiology tbody tr").length > 0 && $("#" + ClinicalRadiologyOrderDetail.params.PanelID + " #dgvRadiology tbody tr").text() != "No data available in table") {
                            var isProblemAdded = ClinicalRadiologyOrderDetail.problemAdded();
                            if (isProblemAdded == true) {
                                ClinicalRadiologyOrderDetail.RadiologyOrderSave('save');
                            } else {
                                utility.kendoConfirm('Confirm Sign', 'No ICD code is associated with this order. Do you wish to continue?', function () {
                                    if ($("#donotaskedagain:checked").length > 0) {
                                        utility.AddKey2LocalStorageForDoNotAskedAgain();
                                    }
                                    ClinicalRadiologyOrderDetail.RadiologyOrderSave('save');
                                });
                            }
                        }
                        //else {
                        //utility.DisplayMessages("Associate at least one Problem with the order.", 3);
                        //}
                        break;

                        //End 28-11-2016 Humaira Yousaf for associated problems validation
                }

            }
            e.type = "";
        });

    },

    //Author: Ahmad Raza
    //Date :  04-03-2016
    //Reason: Binding numpad with height, weight, systolic and diastolic fields
    Init: function () {

        //Start//22-03-2016//Ahmad Raza//Logic to select insurance dropdowns on priority base
        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlBillingTypeId').on('change', function () {

            if ($(this).val() == 3) {

                //$('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlPrimaryInsuraceId').attr('disabled', false);
                //$('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlSecondaryInsuraceId').attr('disabled', false);
                //$('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlTertiaryInsuraceId').attr('disabled', false);
                var selectedVal1 = '';
                var selectedVal2 = '';
                var selectedVal3 = '';
                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlPrimaryInsuraceId option').each(function () {
                    if ($(this).attr('Priority') == "1") {
                        selectedVal1 = $(this).text();
                        $("#hfRadPrimaryInsId").val($(this).val());
                    }

                });
                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlPrimaryInsuraceId').val(selectedVal1);

                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlSecondaryInsuraceId option').each(function () {
                    if ($(this).attr('Priority') == "2") {
                        selectedVal2 = $(this).text();
                        $("#hfRadSecondaryInsId").val($(this).val());
                    }
                });
                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlSecondaryInsuraceId').val(selectedVal2);

                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlTertiaryInsuraceId option').each(function () {
                    if ($(this).attr('Priority') == "3") {
                        selectedVal3 = $(this).text();
                        $("#hfRadTertiaryInsId").val($(this).val());
                    }
                });
                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlTertiaryInsuraceId').val(selectedVal3);

                //Start//07-03-2016//Abid Ali//set gurantor and relation vlaues
                ClinicalRadiologyOrderDetail.setGurantorRelationValues(true);
                //End//07-03-2016//Abid Ali//set gurantor and relation vlaues
            }
            else {

                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlPrimaryInsuraceId').attr('disabled', true);
                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlSecondaryInsuraceId').attr('disabled', true);
                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlTertiaryInsuraceId').attr('disabled', true);


                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlPrimaryInsuraceId').val('');
                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlSecondaryInsuraceId').val('');
                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlTertiaryInsuraceId').val('');
                $("#hfRadPrimaryInsId").val('');
                $("#hfRadSecondaryInsId").val('');
                $("#hfRadTertiaryInsId").val('');
                //Start//07-03-2016//Abid Ali//Clear gurantor and relation vlaues
                ClinicalRadiologyOrderDetail.setGurantorRelationValues(false);
                //End//07-03-2016//Abid Ali//Clear gurantor and relation vlaues
            }

        });
        //End//22-03-2016//Ahmad Raza//Logic to select insurance dropdowns on priority base

        $('.toggleHorSmallLeft section').click(function (e) {
            $(this).parent().toggleClass("toggled");
            ClinicalRadiologyOrderDetail.toggleHorSmallLeftIcon($(this));

        });

        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #frmClinicalRadiologyOrderDetail [data-plugin-keyboard-numpad]').keyboard({
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

    radiologyOrderPreview: function (patientID, userID, radiologyOrderId, mode, isIncludeComments) {
        Clinical_RadiologyOrderView.previewRadiologyOrder(patientID, userID, radiologyOrderId, mode, isIncludeComments).done(function (response) {
            response = JSON.parse(response);
            Clinical_RadiologyOrderView.pdf = response.RadiologyOrderHTML;
            // var raw = atob(response.RadiologyOrderHTML);
            utility.documentPrint(response.RadiologyOrderHTML);
        });
    },

    //Author: Ahmad Raza
    //Date :  03-03-2016
    //Reason: Loading ICD Codes for Problem List AutoComplete
    bindICD9AutoComplete: function (element) {
        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "ClinicalRadiologyOrderDetail", null, false);
    },


    UnLoad: function (caller) {
        ClinicalRadiologyOrderDetail.checkedProblems = [];
        $('#' + Clinical_RadiologyOrder.params.PanelID + ' #frmClinicalRadiologyResult #ddlLaboratory').val('');
        $('#' + Clinical_RadiologyOrder.params.PanelID + ' #frmClinicalRadiologyResult #txtProvider').val('');
        $('#' + Clinical_RadiologyOrder.params.PanelID + ' #frmClinicalRadiologyResult #txtOrderNumber').val('');
        $('#' + Clinical_RadiologyOrder.params.PanelID + ' #frmClinicalRadiologyResult #ddlStatus').val('');
        $('#' + Clinical_RadiologyOrder.params.PanelID + ' #pnlRadiologyOrder_Search #frmClinicalRadiologyOrder #txtOrderNumber').val('')
        var saveButtonisHidden = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #btnSaveOrder").hasClass("hidden");
        var form = '#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail";
        if (caller == 'saveExit' || saveButtonisHidden == true) {
            if (ClinicalRadiologyOrderDetail.params["ParentCtrl"] == "Clinical_RadiologyOrder") {
                UnloadActionPan(ClinicalRadiologyOrderDetail.params["ParentCtrl"], "ClinicalRadiologyOrderDetail", null, ClinicalRadiologyOrderDetail.params["ParentCtrlPanelID"]);
                if (ClinicalRadiologyOrderDetail.AddResult && Clinical_RadiologyOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
                    $('#' + Clinical_RadiologyOrder.params.PanelID).find('[id*="listRadiologyOrder"]').removeClass('active');
                    $('#' + Clinical_RadiologyOrder.params.PanelID).find('[id*="RadiologyOrder1"]').removeClass('active');
                    $('#' + Clinical_RadiologyOrder.params.PanelID + ' #listRadiologyResult').addClass('active');
                    $('#' + Clinical_RadiologyOrder.params.PanelID + ' #RadiologyResult1').addClass('active');

                    var def = $.Deferred();
                    var resultDiv = " #dgvRadiologyResult";
                    var type = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlLabId option:selected').text();
                    if (type == "External Facility") {
                        def = Clinical_RadiologyOrder.externalRadiologyResultsSearch("", 1, 15, null, true);
                        $('#' + Clinical_RadiologyOrder.params.PanelID + ' #ulRadilogyResultTypeTabsItems a[href="#RadiologyExternalResult"]').trigger('click');
                        resultDiv = " #dgvExternalRadiologyResult";
                    }
                    else {
                        def = Clinical_RadiologyOrder.radiologyResultsSearch("", 1, 15, null, true);
                        $('#' + Clinical_RadiologyOrder.params.PanelID + ' #ulRadilogyResultTypeTabsItems a[href="#RadiologyInternalResult"]').trigger('click');
                    }

                    $.when(def).then(function () {
                        $("#pnlClinicalProgressNote #pnlClinicalRadiologyOrder" + resultDiv + " input#" + ClinicalRadiologyOrderDetail.AddedRadiologyOrderResultId).prop("checked", true);

                    });

                    //$.when(Clinical_RadiologyOrder.radiologyResultsSearch("", 1, 15, null, true)).then(function () {
                    //    $("#pnlClinicalProgressNote #pnlClinicalRadiologyOrder #dgvRadiologyResult input#" + ClinicalRadiologyOrderDetail.AddedRadiologyOrderResultId).prop("checked", true);
                    //});
                }

            }
            else {
                if (ClinicalRadiologyOrderDetail.params.ParentCtrl == "Clinical_Treatment") {
                    Clinical_Treatment.radiologyOrderSearch();
                }
                UnloadActionPan(ClinicalRadiologyOrderDetail.params["ParentCtrl"], "ClinicalRadiologyOrderDetail");

            }
        }
        else {
            utility.UnLoadDialog(form, function () {
                if (ClinicalRadiologyOrderDetail.params["ParentCtrl"] == "Clinical_RadiologyOrder") {
                    $('#' + Clinical_RadiologyOrder.params.PanelID + ' #frmClinicalRadiologyOrder #ddlLaboratory').val('');
                    $('#' + Clinical_RadiologyOrder.params.PanelID + ' #frmClinicalRadiologyOrder #txtProvider').val('');
                    $('#' + Clinical_RadiologyOrder.params.PanelID + ' #frmClinicalRadiologyOrder #txtOrderNumber').val('');
                    $('#' + Clinical_RadiologyOrder.params.PanelID + ' #frmClinicalRadiologyOrder #ddlStatus').val('');
                    $('#' + Clinical_RadiologyOrder.params.PanelID + ' #pnlRadiologyOrder_Search #frmClinicalRadiologyOrder #txtOrderNumber').val('')
                    UnloadActionPan(ClinicalRadiologyOrderDetail.params["ParentCtrl"], "ClinicalRadiologyOrderDetail", null, ClinicalRadiologyOrderDetail.params["ParentCtrlPanelID"]);
                }
                else {
                    UnloadActionPan(ClinicalRadiologyOrderDetail.params["ParentCtrl"], "ClinicalRadiologyOrderDetail");
                    //Start 11-08-2016 Edit By Humaira Yousaf Bug# EMR-1927
                    Clinical_RadiologyOrder.radiologyOrderSearch(null, null, null, "RadiologyOrderDetail");
                    //End 11-08-2016 Edit By Humaira Yousaf Bug# EMR-1927
                }
            }, function () {
                if (ClinicalRadiologyOrderDetail.params["ParentCtrl"] == "Clinical_RadiologyOrder") {
                    UnloadActionPan(ClinicalRadiologyOrderDetail.params["ParentCtrl"], "ClinicalRadiologyOrderDetail", null, ClinicalRadiologyOrderDetail.params["ParentCtrlPanelID"]);
                }
                else {
                    UnloadActionPan(ClinicalRadiologyOrderDetail.params["ParentCtrl"], "ClinicalRadiologyOrderDetail");

                }
            });
        }
        $('#FaceSheetPager .slick-track').attr('style', 'opacity: 1; transform: translate3d(-57px, 0px, 0px); width: 1193px;');

        ClinicalRadiologyOrderDetail.loadProviderFromAppData = false;
        ClinicalRadiologyOrderDetail.loadFacilityFromAppData = false;
        ClinicalRadiologyOrderDetail.isProviderLoaded = false;
        ClinicalRadiologyOrderDetail.isFacilityLoaded = false;
    },

    //Function Name: RadiologyOrderSave
    //Author Name: Humaira Yousaf
    //Created Date: 02-03-2016
    //Description: Saves RadiologyOrder
    RadiologyOrderSave: function (sender) {
        var FavVal = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlFavoriteListRadiologyOrder').val();
        var RadiologyOrderId = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfRadiologyOrderId").val() != "" ? $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfRadiologyOrderId").val() : "-1";
        if (parseInt(RadiologyOrderId) > 0) {
            ClinicalRadiologyOrderDetail.params.mode = "Edit";
        }
        else {
            ClinicalRadiologyOrderDetail.params.mode = "Add";
        }

        var self = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail");
        //Start 03-03-2016 Humaira Yousaf to validate title
        var title = self.find("#txtRadiologyOrderTitle").val();
        if (self.find("#txtComment").text()) {
            self.find("#txtComments").val(self.find("#txtComment").text()); // add comments in  existing field
        }
        else {
            self.find("#txtComments").val("");
        }
        if (true) {
            //End 03-03-2016 Humaira Yousaf to validate title
            var mainErrorMessage = "";


            var myJSON = self != null ? self.getMyJSONByName() : "{}";
            var objData = JSON.parse(myJSON);
            objData["Comments"] = $("#" + ClinicalRadiologyOrderDetail.params.PanelID + " #txtComment").html();
            var RadiologyOrderProblemList = [];
            $(self).find("#ulProblemLists td").each(function (index, item) {
                if ($(this).find("input:checkbox").prop("checked") && $(this).find("input:checkbox").attr("id") != "chkSelectAll") {
                    var objProblem = {
                        ProblemId: $(this).find("input:checkbox").val(),
                        Description: $(this).text()
                    };
                    objProblem.Description = (objProblem.Description).replace(/%/g, "%25");
                    RadiologyOrderProblemList.push(objProblem);
                }
            });
            objData["RadiologyOrderProblemList"] = RadiologyOrderProblemList;
            //Start 22-03-2016 Humaira Yousaf for status
            if (sender == 'signorder' || sender == 'signprintorder')
                objData["Status"] = 'Signed';
            else if (sender == 'save')
                objData["Status"] = 'Pending';
            //End 22-03-2016 Humaira Yousaf for status
            //objData["CPTSNOMEDCodeId"] = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #hfCPTSNOMEDCode').val();
            //objData["CPTSNOMEDDescription"] = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #hfCPTSNOMEDDescription').val();

            myJSON = JSON.stringify(objData);

            //------------------------------------------------------------
            var RadiologyTestIds = $("#" + ClinicalRadiologyOrderDetail.params.PanelID + " #dgvRadiology tbody tr:not([id*=Child]").map(function () {
                return this.id.replace("id", "");
            }).get().join(',');
            $("#" + ClinicalRadiologyOrderDetail.params.PanelID + " #pnlRadiology_Result #hfRadiologyTestIds").val(RadiologyTestIds);

            var selfRadiologyTest = $("#" + ClinicalRadiologyOrderDetail.params.PanelID + " #pnlRadiology_Result");
            var JSONToSave = selfRadiologyTest.getMyJSON();
            var objRad = new Object();

            objRad["RadiologyTestIds"] = RadiologyTestIds;
            // JSONToSave = utility.MergeJSON(myJSON, myJSONRadiologyTest);
            var data = JSON.stringify(objRad);

            try {
                JSONToSave = decodeURIComponent(JSONToSave);
            }
            catch (ex) {
                console.log(ex);
            }

            JSONToSave = utility.MergeJSON(myJSON, JSONToSave);
            JSONToSave = utility.MergeJSON(data, JSONToSave);
            //--------------------------------------------------------------

            ClinicalRadiologyOrderDetail.cacheRadiologyOrderData();

            if (ClinicalRadiologyOrderDetail.params.mode == "Add") {

                AppPrivileges.GetFormPrivileges("Orders and Results_Diagnostic Imaging Order", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        ClinicalRadiologyOrderDetail.saveRadiologyOrder(JSONToSave).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {

                                ClinicalRadiologyOrderDetail.params.mode = "Edit";
                                //Start 10-06-2016 Abid Ali for favorite list setting for all favLists
                                var isFavListOpened = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #favSectionDiv").hasClass("toggled");
                                $.when(EMRUtility.insertUpdateFavListStatus(ClinicalRadiologyOrderDetail.FavListName, !isFavListOpened)).then(function () {
                                    ClinicalRadiologyOrderDetail.SaveFavListVal(FavVal);
                                });
                                //End 31-05-2016 Abid Ali for favorite list setting for all favLists

                                if (ClinicalRadiologyOrderDetail.params.ParentCtrl == "clinicalTabProgressNote") {
                                    ClinicalRadiologyOrderDetail.getLatestRadiologyOrderByPatientId(true);
                                }
                                //Start 07-03-2016 Humaira Yousaf to save RadiologyOrder Id
                                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfRadiologyOrderId").val(response.radiologicalOrderId);
                                //End 07-03-2016 Humaira Yousaf to save RadiologyOrder Id


                                utility.DisplayMessages(response.message, 1);


                                //Clinical_RadiologyOrder.radiologyResultsSearch(null, null, null, "RadiologyResultDetail");
                                var def = $.Deferred();
                                var orderDiv = " #dgvRadiologyOrder";
                                var type = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlLabId option:selected').text();
                                if (type == "External Facility") {
                                    def = Clinical_RadiologyOrder.externalRadiologyOrderSearch(null, null, null, "RadiologyOrderDetail");
                                    Clinical_RadiologyOrder.activeTab('External')
                                    orderDiv = " #dgvExternalRadiologyOrder";
                                }
                                else {
                                    def = Clinical_RadiologyOrder.radiologyOrderSearch(null, null, null, "RadiologyOrderDetail");
                                    Clinical_RadiologyOrder.activeTab('Internal');
                                }
                                $.when(def).then(function () {
                                    if (ClinicalRadiologyOrderDetail.params.ParentCtrl == "Clinical_RadiologyOrder" && Clinical_RadiologyOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                                        $("#pnlClinicalProgressNote #pnlClinicalRadiologyOrder" + orderDiv + " input#" + response.radiologicalOrderId).prop("checked", true);
                                    }
                                });

                                //Start 23-03-2016 Humaira Yousaf to view PDF
                                if (sender == 'signprintorder') {
                                    ClinicalRadiologyOrderDetail.params.RadiologyOrderId = response.radiologicalOrderId;
                                    ClinicalRadiologyOrderDetail.printRadiologyOrder();
                                }
                                else {
                                    //Start 03-03-2016 Humaira Yousaf to unload form on save
                                    ClinicalRadiologyOrderDetail.UnLoad('saveExit');
                                    $('#FaceSheetPager .slick-track').attr('style', 'opacity: 1; transform: translate3d(-57px, 0px, 0px); width: 1193px;');
                                    //End 03-03-2016 Humaira Yousaf to unload form on save
                                }
                                //End 23-03-2016 Humaira Yousaf to view PDF
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
            else if (ClinicalRadiologyOrderDetail.params.mode == "Edit") {

                AppPrivileges.GetFormPrivileges("Orders and Results_Diagnostic Imaging Order", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        ClinicalRadiologyOrderDetail.updateRadiologyOrder(JSONToSave, RadiologyOrderId).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {

                                //Start 10-06-2016 Abid Ali for favorite list setting for all favLists

                                var isFavListOpened = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #favSectionDiv").hasClass("toggled");
                                $.when(EMRUtility.insertUpdateFavListStatus(ClinicalRadiologyOrderDetail.FavListName, !isFavListOpened)).then(function () {
                                    ClinicalRadiologyOrderDetail.SaveFavListVal(FavVal);
                                });

                                //End 31-05-2016 Abid Ali for favorite list setting for all favLists

                                if (ClinicalRadiologyOrderDetail.params.ParentCtrl == "clinicalTabProgressNote") {
                                    ClinicalRadiologyOrderDetail.getLatestRadiologyOrderByPatientId();
                                }
                                //Start 07-03-2016 Humaira Yousaf to save RadiologyOrder Id
                                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfRadiologyOrderId").val(response.radiologicalOrderId);
                                //End 07-03-2016 Humaira Yousaf to save RadiologyOrder Id
                                utility.DisplayMessages(response.message, 1);
                                $('#' + Clinical_RadiologyOrder.params.PanelID + ' #frmClinicalRadiologyOrder #ddlLaboratory').val('');
                                $('#' + Clinical_RadiologyOrder.params.PanelID + ' #frmClinicalRadiologyOrder #txtProvider').val('');
                                $('#' + Clinical_RadiologyOrder.params.PanelID + ' #frmClinicalRadiologyOrder #txtOrderNumber').val('');
                                $('#' + Clinical_RadiologyOrder.params.PanelID + ' #frmClinicalRadiologyOrder #ddlStatus').val('');
                                Clinical_RadiologyOrder.radiologyResultsSearch(null, null, null, "RadiologyResultDetail");

                                var def = $.Deferred();
                                var orderDiv = " #dgvRadiologyOrder";
                                var type = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlLabId option:selected').text();
                                if (type == "External Facility") {
                                    def = Clinical_RadiologyOrder.externalRadiologyOrderSearch(null, null, null, "RadiologyOrderDetail");
                                    $('#' + Clinical_RadiologyOrder.params.PanelID + ' #ulRadilogyOrderTypeTabsItems a[href="#RadiologyExternalOrder"]').trigger('click');
                                    orderDiv = " #dgvExternalRadiologyOrder";
                                }
                                else {
                                    def = Clinical_RadiologyOrder.radiologyOrderSearch(null, null, null, "RadiologyOrderDetail");
                                    $('#' + Clinical_RadiologyOrder.params.PanelID + ' #ulRadilogyOrderTypeTabsItems a[href="#RadiologyInternalOrder"]').trigger('click');
                                }
                                $.when(def).then(function () {
                                    if (ClinicalRadiologyOrderDetail.params.ParentCtrl == "Clinical_RadiologyOrder" && Clinical_RadiologyOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                                        $("#pnlClinicalProgressNote #pnlClinicalRadiologyOrder " + orderDiv + " input#" + response.radiologicalOrderId).prop("checked", true);
                                    }
                                });

                                //Start 23-03-2016 Humaira Yousaf to view PDF
                                if (sender == 'signprintorder') {
                                    ClinicalRadiologyOrderDetail.printRadiologyOrder();
                                }
                                else {
                                    //Start 03-03-2016 Humaira Yousaf to unload form on save
                                    ClinicalRadiologyOrderDetail.UnLoad('saveExit');
                                    $('#FaceSheetPager .slick-track').attr('style', 'opacity: 1; transform: translate3d(-57px, 0px, 0px); width: 1193px;');
                                    //End 03-03-2016 Humaira Yousaf to unload form on save
                                }
                                //End 23-03-2016 Humaira Yousaf to view PDF

                            }
                            else {
                                utility.DisplayMessages(response.message, 3);
                            }
                        });
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });

            }

        }
        else {
            //      utility.DisplayMessages("Title is required.", 3);
        }
    },

    SaveFavListVal: function (FavListVal) {
        EMRUtility.insertUpdateFavListVal(ClinicalRadiologyOrderDetail.FavListName, FavListVal);
    },

    //Author Name: Abid Ali
    //Created Date: 13-06-2016
    //Description: Saves Radiology order detail data
    cacheRadiologyOrderData: function () {
        Clinical_RadiologyOrder.params["ProviderName"] = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #txtProvider").val();
        Clinical_RadiologyOrder.params["ProviderId"] = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #hfProvider").val();

        Clinical_RadiologyOrder.params["AssigneeName"] = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #ddlAssigneeId").val();
        Clinical_RadiologyOrder.params["AssigneeId"] = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #hfAssignee").val();

        Clinical_RadiologyOrder.params["Problems"] = ClinicalRadiologyOrderDetail.RadiologyOrderProblems;

        Clinical_RadiologyOrder.params["LabId"] = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #ddlLabId").val();
        Clinical_RadiologyOrder.params["BillingTypeId"] = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #ddlBillingTypeId").val();

        Clinical_RadiologyOrder.params["FacilityName"] = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #txtFacility").val();
        Clinical_RadiologyOrder.params["FacilityId"] = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #hfFacility").val();


        Clinical_RadiologyOrder.params["CurrentPatientId"] = Clinical_RadiologyOrder.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        Clinical_RadiologyOrder.params["hasData"] = true;
    },


    //Function Name: checkRadiologyOrderExists
    //Author Name: Ahmad Raza
    //Created Date: 24-03-2016
    //Description: To check whether Radiology order exists or not
    checkRadiologyOrderExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_radiologyorder').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #PlanNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="RadiologyOrderComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_radiologyorder title="Radiology Order"  id="clinicalMenu_Orders_Radiology" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'Radiology\',\'clinicalMenu_Orders_Radiology\',' + Clinical_ProgressNote.params.NotesId + ');" title="Diagnostic Imaging Order">Diagnostic Imaging Order</a> ' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'Radiology Order\',\'clinicalMenu_Orders_Radiology\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_radiologyorder> </header></li>');
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
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_radiologyorder').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
    },

    //Function Name: RadiologyOrderSave
    //Author Name: Humaira Yousaf
    //Created Date: 02-03-2016
    //Description: Saves RadiologyOrder
    //Params: RadiologyOrderData
    saveRadiologyOrder: function (RadiologyOrderData, NotUpdateTestValues) {
        var objData = JSON.parse(RadiologyOrderData);
        //if (objData.Comments) {
        //    var commentlength = objData.Comments.length;
        //    if (commentlength > 2000) {
        //        objData.Comments = objData.Comments.substring(0, 2000);
        //    }
        //}
        // objData["PatientId"] = hfPatientId
        objData["commandType"] = "save_RadiologyOrder";

        objData["PrimaryInsuraceId"] = $("#hfRadPrimaryInsId").val();
        objData["SecondaryInsuraceId"] = $("#hfRadSecondaryInsId").val();
        objData["TertiaryInsuraceId"] = $("#hfRadTertiaryInsId").val();

        if (ClinicalRadiologyOrderDetail.params.ParentCtrlPanelID == "pnlClinicalProgressNote #pnlClinicalRadiologyOrder") {
            objData["NoteId"] = $("#pnlClinicalProgressNote #hfNoteId").val();
        } else {
            objData["NoteId"] = "";
        }
        if (NotUpdateTestValues) {
            objData["UpdateTestValues"] = "0";
        }
        else {
            objData["UpdateTestValues"] = "1";
        }
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyOrder", "RadiologyOrder");
    },

    //Function Name: loadRadiologyOrder
    //Author Name: Humaira Yousaf
    //Created Date: 04-03-2016
    //Description: Loads RadiologyOrder data
    loadRadiologyOrder: function (radiologyOrderId) {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Orders and Results_Diagnostic Imaging Order", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (ClinicalRadiologyOrderDetail.params.mode == "Edit") {
                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfRadiologyOrderId").val(radiologyOrderId);
                    var self = $('#' + ClinicalRadiologyOrderDetail.params.PanelID);

                    ClinicalRadiologyOrderDetail.fillRadiologyOrder(radiologyOrderId).done(function (response) {
                        if (response != "") {
                            var data = JSON.parse(response);
                            if (data.status != false) {
                                var detailJSON = JSON.parse(data.radiologyFill_JSON);
                                if (detailJSON.IsIncludeComments == "True") {
                                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #chkIncludeComments").prop("checked", true);
                                }
                                if (detailJSON.bResultAcknowledged == "True") {
                                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #btnAddRadiologyResult").text("View Result");
                                }
                                else if (detailJSON.bResultExists == "True") {
                                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #btnAddRadiologyResult").text("Edit Result");
                                }
                                else {
                                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #btnAddRadiologyResult").text("Add Result");
                                }

                                var radiologyOrderDetail = JSON.parse(data.radiologyOrderFill_JSON);

                                radiologyOrderDetail = radiologyOrderDetail[0];

                                if (radiologyOrderDetail != null && radiologyOrderDetail.OrderDate != null && radiologyOrderDetail.OrderDate != '') {
                                    function pad(s) { return (s < 10) ? '0' + s : s; }
                                    var d = new Date(radiologyOrderDetail.OrderDate);
                                    radiologyOrderDetail.OrderDate = [pad(d.getMonth() + 1), pad(d.getDate()), d.getFullYear()].join('/');
                                }
                                //Start//25-03-2016//Abid Ali//For bug# EMR-552
                                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtRadiologyOrderNo").val(radiologyOrderDetail.OrderNo);
                                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtRadiologyOrderNo").parent().css('display', 'block');
                                //End//25-03-2016//Abid Ali//For bug# EMR-552

                                if (radiologyOrderDetail.RelationShipId == "") {
                                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID).find('#txtRadiologyRelationShipId').val("");
                                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID).find('#hfRalationId').val("");
                                }
                                utility.bindMyJSONByName(true, radiologyOrderDetail, false, $(self.selector + " #frmClinicalRadiologyOrderDetail")).done(function () {

                                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #ddlAssigneeId").val(radiologyOrderDetail.AssigneeName);

                                    ClinicalRadiologyOrderDetail.EnableDisableTestSearch($('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #ddlLabId"));

                                    var billingType = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlBillingTypeId option:selected').text();

                                    if (billingType.indexOf('Insurance') >= 0) {
                                        ClinicalRadiologyOrderDetail.enableDisableInsurances(false);
                                    }
                                    else {
                                        ClinicalRadiologyOrderDetail.enableDisableInsurances(false);
                                        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlPrimaryInsuraceId').val('');
                                        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlSecondaryInsuraceId').val('');
                                        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlTertiaryInsuraceId').val('');
                                    }
                                    //$('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtComment").text(radiologyOrderDetail.Comments);
                                    ClinicalRadiologyOrderDetail.RadiologyOrderTestGridLoad(response);
                                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").data('serialize', $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").serialize());

                                    var comm = '<div>' + radiologyOrderDetail.Comments + '</div>';
                                    var obj = $(comm);
                                    comm = '<div>' + $(obj).text() + '</div>';
                                    obj = $(comm);
                                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #txtComment').html(obj);


                                });

                                if (data.radiologyOrderProblems_JSON != null) {

                                    data.radiologyOrderProblems_JSON = JSON.parse(data.radiologyOrderProblems_JSON);

                                    for (var index in data.radiologyOrderProblems_JSON) {
                                        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #ulProblemLists td #chk" + data.radiologyOrderProblems_JSON[index].ProblemId).attr("checked", "checked");
                                    }
                                    ClinicalRadiologyOrderDetail.checkOrUncheckSelectAll();
                                }

                                if (ClinicalRadiologyOrderDetail.params.mode == "Edit") {
                                    if (radiologyOrderDetail.Status.toLowerCase() == "signed") {
                                        ClinicalRadiologyOrderDetail.disableRadiologyOrderControls(true);
                                    }
                                    else {
                                        ClinicalRadiologyOrderDetail.disableRadiologyOrderControls(false);
                                    }
                                }
                                else {
                                    ClinicalRadiologyOrderDetail.disableRadiologyOrderControls(false);
                                }

                            }
                        }
                        if (ClinicalRadiologyOrderDetail.params.mode == "Add") {
                            ClinicalRadiologyOrderDetail.disableRadiologyOrderControls(false);
                        }
                        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").data('serialize', $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").serialize());
                    });
                }
                else {
                    //populate prev saved/updated order details
                    ClinicalRadiologyOrderDetail.populateRadiologyOrderSavedData();
                }


                //Start 10-06-2016 Abid Ali for favorite list setting for all favLists
                if (EMRUtility.getFavListStatus(ClinicalRadiologyOrderDetail.FavListName))
                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #favSectionDiv").removeClass("toggled");
                else
                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #favSectionDiv").addClass("toggled");
                //End 10-06-2016 Abid Ali for favorite list setting for all favLists
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    //Author Name: Abid Ali
    //Created Date: 13-06-2016
    //Description: Gets Radiology order detail data
    populateRadiologyOrderSavedData: function () {

        var $form = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail");

        if (Clinical_RadiologyOrder.params["hasData"] == true) {

            $form.find("#txtProvider").val(Clinical_RadiologyOrder.params["ProviderName"]);
            $form.find("#hfProvider").val(Clinical_RadiologyOrder.params["ProviderId"]);
            //$form.find("#ddlAssigneeId").val(Clinical_RadiologyOrder.params["AssigneeName"]);according to EMR-4363 (QA Mehboob sahab)
            $form.find("#hfAssignee").val(Clinical_RadiologyOrder.params["AssigneeId"]);
            $form.find("#ddlLabId").val(Clinical_RadiologyOrder.params["LabId"]);
            $form.find("#ddlLabId").trigger("onchange");
            $form.find("#ddlBillingTypeId").val(Clinical_RadiologyOrder.params["BillingTypeId"]);

            //Start//Abid Ali/ For bug# EMR-751
            var billingType = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + '  #frmClinicalRadiologyOrderDetail #ddlBillingTypeId option:selected').text();
            if (billingType.indexOf('Insurance') >= 0) {
                ClinicalRadiologyOrderDetail.enableDisableInsurances(false);
            }
            else {
                ClinicalRadiologyOrderDetail.enableDisableInsurances(false);
                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlPrimaryInsuraceId').val('');
                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlSecondaryInsuraceId').val('');
                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlTertiaryInsuraceId').val('');
            }
            //End//Abid Ali/ For bug# EMR-751

            $form.find("#txtFacility").val(Clinical_RadiologyOrder.params["FacilityName"]);
            $form.find("#hfFacility").val(Clinical_RadiologyOrder.params["FacilityId"]);

        }


        ClinicalRadiologyOrderDetail.loadProviderFromAppData = true;
        ClinicalRadiologyOrderDetail.loadFacilityFromAppData = true;

        if (Clinical_RadiologyOrder.params.ParentCtrl == 'clinicalTabProgressNote') {

            //if (ClinicalRadiologyOrderDetail.loadProviderFromAppData && ClinicalRadiologyOrderDetail.ProviderOnDemandLoad)
            //{
            CacheManager.BindCodes('GetProvider', false).done(function (result) {
                var Ctrl = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtProvider");
                var onSelect = function (dataItem) {
                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtProvider").attr("Provider", Clinical_ProgressNote.params["CurrentNotesProviderId"]);
                    //$('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfProvider").val(Clinical_ProgressNote.params["CurrentNotesProviderId"]);
                    // Set selected Provider Id AST - 365
                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfProvider").val(dataItem.id);
                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtFacilityTo").val("");
                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfFacilityTo").val("");
                }
                utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, null, onSelect);

                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtProvider").val(Clinical_ProgressNote.params["CurrentNotesProviderText"]);
                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtProvider").attr("Provider", Clinical_ProgressNote.params["CurrentNotesProviderId"]);
                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfProvider").val(Clinical_ProgressNote.params["CurrentNotesProviderId"]);
                utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtProvider"), Clinical_ProgressNote.params.CurrentNotesProviderText, $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfProvider"), Clinical_ProgressNote.params.CurrentNotesProviderId);
                ClinicalRadiologyOrderDetail.setDefaultFacilityTO();
                ClinicalRadiologyOrderDetail.EnableDisableTestSearch();
                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").data('serialize', $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").serialize());
            });
            //    ClinicalRadiologyOrderDetail.loadProviderFromAppData = true;
            //    ClinicalRadiologyOrderDetail.ProviderOnDemandLoad = false;
            //}

            //if (ClinicalRadiologyOrderDetail.loadFacilityFromAppData && ClinicalRadiologyOrderDetail.FacilityOnDemandLoaded) {

            CacheManager.BindCodes('GetFacility', false).done(function (result) {
                var Ctrl = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtFacility");
                var onSelect = function (dataItem) {
                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtFacility").attr("Facility", Clinical_ProgressNote.params["NotesFacilityIDForFollowUp"]);
                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfFacility").val(Clinical_ProgressNote.params["NotesFacilityIDForFollowUp"]);
                }
                utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, null, onSelect);

                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtFacility").val(Clinical_ProgressNote.params["NotesFacilityName"]);
                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtFacility").attr("Facility", Clinical_ProgressNote.params["NotesFacilityIDForFollowUp"]);
                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfFacility").val(Clinical_ProgressNote.params["NotesFacilityIDForFollowUp"]);
                utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtFacility"), Clinical_ProgressNote.params.NotesFacilityName, $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfFacility"), Clinical_ProgressNote.params.NotesFacilityIDForFollowUp);
                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").data('serialize', $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").serialize());
            });
            //    ClinicalRadiologyOrderDetail.loadProviderFromAppData = true;
            //    ClinicalRadiologyOrderDetail.ProviderOnDemandLoad = false;
            //}
        } else {
            if ($('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtProvider").val() == "") {
                if (globalAppdata.DefaultProviderName != "- Select -")
                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtProvider").val(globalAppdata.DefaultProviderName);
                else
                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtProvider").val("");
                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtProvider").attr("Provider", globalAppdata.DefaultProviderId);
                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfProvider").val(globalAppdata.DefaultProviderId);
                utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtProvider"), globalAppdata.DefaultProviderName, $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfProvider"), globalAppdata.DefaultProviderId);

            }
            if ($('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtFacility").val() == "") {
                if (globalAppdata.DefaultFacilityName != "- Select -")
                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtFacility").val(globalAppdata.DefaultFacilityName);
                else
                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtFacility").val("");

                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtFacility").attr("Facility", globalAppdata.DefaultFacilityId);
                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfFacility").val(globalAppdata.DefaultFacilityId);
                utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtFacility"), globalAppdata.DefaultFacilityName, $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfFacility"), globalAppdata.DefaultFacilityId);
            }
            if (ClinicalRadiologyOrderDetail.loadProviderFromAppData && ClinicalRadiologyOrderDetail.ProviderOnDemandLoad) {
                if (globalAppdata.DefaultProviderName != "" && globalAppdata.DefaultProviderName != "- Select -") {
                    CacheManager.BindCodes('GetProvider', false).done(function (result) {
                        var Ctrl = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtProvider");
                        var hfCtrl = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfProvider");
                        var onSelect = function (dataItem) { $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtProvider").attr("Provider", dataItem.id); }
                        utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, onSelect);

                        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtProvider").val(globalAppdata.DefaultProviderName);
                        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtProvider").attr("Provider", globalAppdata.DefaultProviderId);
                        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfProvider").val(globalAppdata.DefaultProviderId);
                        utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtProvider"), globalAppdata.DefaultProviderName, $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfProvider"), globalAppdata.DefaultProviderId);
                        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").data('serialize', $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").serialize());
                    });
                    ClinicalRadiologyOrderDetail.loadProviderFromAppData = true;
                    ClinicalRadiologyOrderDetail.ProviderOnDemandLoad = false;
                } else {
                    CacheManager.BindCodes('GetProvider', false).done(function (result) {
                        var Ctrl = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtProvider");
                        var hfCtrl = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfProvider");
                        var onSelect = function (dataItem) { $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtProvider").attr("Provider", dataItem.id); }
                        utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, onSelect);

                        if ($('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtProvider").val() == "") {
                            $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtProvider").val("");
                            $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtProvider").attr("Provider", "");
                            $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfProvider").val("");
                        }
                        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").data('serialize', $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").serialize());
                    });
                    ClinicalRadiologyOrderDetail.loadProviderFromAppData = true;
                    ClinicalRadiologyOrderDetail.ProviderOnDemandLoad = false;
                }
            }

            if (ClinicalRadiologyOrderDetail.loadFacilityFromAppData && ClinicalRadiologyOrderDetail.FacilityOnDemandLoad) {
                if (globalAppdata.DefaultFacilityName != "" && globalAppdata.DefaultFacilityName != "- Select -") {

                    CacheManager.BindCodes('GetFacility', false).done(function (result) {
                        var Ctrl = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtFacility");
                        var hfCtrl = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfFacility");
                        var onSelect = function (dataItem) { $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtFacility").attr("Facility", dataItem.id); }
                        utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl, onSelect);

                        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtFacility").val(globalAppdata.DefaultFacilityName);
                        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtFacility").attr("Facility", globalAppdata.DefaultFacilityId);
                        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfFacility").val(globalAppdata.DefaultFacilityId);
                        utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtFacility"), globalAppdata.DefaultFacilityName, $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfFacility"), globalAppdata.DefaultFacilityId);
                        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").data('serialize', $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").serialize());
                    });
                    ClinicalRadiologyOrderDetail.loadFacilityFromAppData = true;
                    ClinicalRadiologyOrderDetail.FacilityOnDemandLoad = false;
                }
                else {
                    CacheManager.BindCodes('GetFacility', false).done(function (result) {
                        var Ctrl = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtFacility");
                        var onSelect = function (dataItem) {
                            $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtFacility").attr("Facility", "");
                            $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfFacility").val("");
                        }
                        utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, null, onSelect);

                        if ($('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtFacility").val() == "") {
                            $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtFacility").val("");
                            $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtFacility").attr("Facility", "");
                            $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfFacility").val("");
                        }
                        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").data('serialize', $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").serialize());
                    });
                    ClinicalRadiologyOrderDetail.loadFacilityFromAppData = true;
                    ClinicalRadiologyOrderDetail.FacilityOnDemandLoad = false;
                }
            }
        }
        ClinicalRadiologyOrderDetail.setDefaultFacilityTO();
        ClinicalRadiologyOrderDetail.EnableDisableTestSearch();
    },
    setDefaultFacilityTO: function () {
        var ProviderId = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #txtProvider").val() != "" && parseInt($('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfProvider").val()) > 0 ? $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfProvider").val() : "-1";
        if (parseInt(ProviderId) > 0) {
            utility.GetProvidersDiagnosticImagingFacilityArray("", ProviderId).done(function (response) {
                if (response.length > 0) {
                    var CurrentFacility = $.grep(response, function (a) {
                        return a.value == "SHI";
                    });
                    if (CurrentFacility.length > 0) {
                        $("#" + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #txtFacilityTo").val(CurrentFacility[0].value);
                        $("#" + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #hfFacilityTo").val(CurrentFacility[0].id);
                        utility.SetKendoAutoCompleteSourceforValidate($("#" + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #txtFacilityTo"), CurrentFacility[0].value, $("#" + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #hfFacilityTo"), CurrentFacility[0].id);
                    }
                }
            });
        }
    },
    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: To Disable inurances
    enableDisableInsurances: function (enable) {
        if (enable) {
            $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlPrimaryInsuraceId').attr('disabled', false);
            $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlSecondaryInsuraceId').attr('disabled', false);
            $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlTertiaryInsuraceId').attr('disabled', false);

            //Start//Abid Ali//For bug# EMR-1253
            ClinicalRadiologyOrderDetail.setGurantorRelationValues(true);
            //End//Abid Ali//For bug# EMR-1253
        }
        else {
            $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlPrimaryInsuraceId').attr('disabled', true);
            $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlSecondaryInsuraceId').attr('disabled', true);
            $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlTertiaryInsuraceId').attr('disabled', true);

            //Start//Abid Ali//For bug# EMR-1253
            ClinicalRadiologyOrderDetail.setGurantorRelationValues(true);
            //End//Abid Ali//For bug# EMR-1253
        }
    },

    //Function Name: fillRadiologyOrder
    //Author Name: Humaira Yousaf
    //Created Date: 04-03-2016
    //Description: Fills RadiologyOrder
    //Params: RadiologyOrderId
    fillRadiologyOrder: function (radiologyOrderId) {

        var objData = {};
        objData["commandType"] = "fill_radiologyorder";
        objData["RadiologyOrderId"] = radiologyOrderId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyOrder", "RadiologyOrder");
    },

    //Function Name: updateRadiologyOrder
    //Author Name: Humaira Yousaf5
    //Created Date: 07-03-2016
    //Description: Updates RadiologyOrder
    //Params: RadiologyOrderData, RadiologyOrderId
    updateRadiologyOrder: function (RadiologyOrderData, RadiologyOrderId) {

        var objData = JSON.parse(RadiologyOrderData);
        // AST - 405
        if (!objData.Assignee)
            objData.AssigneeId = "";
        objData["RadiologyOrderId"] = RadiologyOrderId;
        objData["commandType"] = "save_RadiologyOrder";
        objData["UpdateTestValues"] = "1";

        objData["PrimaryInsuraceId"] = $("#hfRadPrimaryInsId").val();
        objData["SecondaryInsuraceId"] = $("#hfRadSecondaryInsId").val();
        objData["TertiaryInsuraceId"] = $("#hfRadTertiaryInsId").val();

        if (ClinicalRadiologyOrderDetail.params.ParentCtrlPanelID == "pnlClinicalProgressNote #pnlClinicalRadiologyOrder") {
            objData["NoteId"] = $("#pnlClinicalProgressNote #hfNoteId").val();
        } else {
            objData["NoteId"] = "";
        }

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyOrder", "RadiologyOrder");

    },

    //Author: Ahmad Raza
    //Date :  09-03-2016
    //Reason: to show RadiologyOrder Alert on Dashboard
    showRadiologyOrderAlert: function (triggerLocation) {

        ClinicalRadiologyOrderDetail.showRadiologyOrderAlertDBCall(triggerLocation).done(function (response) {
            response = JSON.parse(response);
            //Start//09-03-2016//Ahmad Raza//setting hiddenField values
            $(" #mainForm  li#RadiologyOrderAlert input").val('');
            $(" #mainForm  li#RadiologyOrderAlert input").val(function (i, val) {
                return val + (val ? ', ' : '') + response.RadiologyOrderIDs;
            });
            //End//09-03-2016//Ahmad Raza//setting hiddenField values
            if (response.status != false) {

                if (response.alertCount > 0) {
                    $(" #mainForm  li#RadiologyOrderAlert span").text(response.alertCount);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    //Author: Ahmad Raza
    //Date :  09-03-2016
    //Reason: DBCall to show RadiologyOrder Alert on Dashboard
    showRadiologyOrderAlertDBCall: function (triggerLocation) {
        var objData = new Object();
        if (triggerLocation == 'FaceSheet') {
            objData["RadiologyOrderTriggerLocation"] = '1';
        }
        else if (triggerLocation == 'Notes') {
            objData["RadiologyOrderTriggerLocation"] = '2';
        }
        objData["PatientId"] = Clinical_RadiologyOrder.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        objData["commandType"] = "show_RadiologyOrder_alert";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyOrder", "RadiologyOrder");
    },

    bindAutoComplete: function (element) {
        var hiddenCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "ClinicalRadiologyOrderDetail", null, true);

    },

    openCPTCode: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ClinicalRadiologyOrderDetail";
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtCPTCode";
        params["ParentCtrlPanelID"] = ClinicalRadiologyOrderDetail.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, ClinicalRadiologyOrderDetail.params.PanelID);
    },

    isTestExists: function (procDesc) {
        var currentRow = $("#" + ClinicalRadiologyOrderDetail.params.PanelID + " #dgvRadiology tbody tr");
        var isTestAlreadySelected = false;
        $.each(currentRow, function (i, item) {

            //Start//08-06-2016//Modified by Abid Ali
            var test = $(item).find("input[id*='Procedure']").val();
            var testCPT = $(item).find("input[id*='CPTCode']").val();
            var testDescription = $(item).find("input[id*='CPTDescription']").val();

            if (testDescription != null) {
                if (procDesc.toLowerCase().toLowerCase().replace(/\-/gi, '').trim() == testDescription.toLowerCase().replace(/\-/gi, '').trim()) {
                    isTestAlreadySelected = true;
                    return false;
                }
            }

        });
        return isTestAlreadySelected;
    },

    BindProcedureGridItem: function (cptCode, procedureDescription, cptDescription, SNOMEDId, SNOMEDDescription) {

        var cptCode = cptCode;
        var procDesc = utility.decodeHtml(procedureDescription);
        var isTestAlreadySelected = ClinicalRadiologyOrderDetail.isTestExists(procDesc);

        if (isTestAlreadySelected != true) {
            ClinicalRadiologyOrderDetail.AddNewRadiologyRow(null, null, null, cptCode, procDesc, cptDescription, SNOMEDId, SNOMEDDescription);
            setTimeout(function () {
                $("#" + ClinicalRadiologyOrderDetail.params.PanelID + " #txtRadiologyCPTCode").val('');
            }, 200);
        }
        else {
            utility.DisplayMessages("Test is already selected", 2);
        }
    },

    buildRowChild: function (obj, ParentRowId) {
        if (!ParentRowId) {
            ParentRowId = "";
        }
        var ChildHTML = $("<div></div>");

        var ddlSpecimen = "<div class='col-xs-2'><label class='control-label'>Specimen</label><select id='Specimen" + ParentRowId + "' name='Specimen' class='form-control' ></select></div>";
        var PatientInstructions = "<div class='col-xs-3'><label class='control-label'>Patient Instructions</label><input type='text' maxlength='500' class='form-control' onkeypress='ClinicalRadiologyOrderDetail.validateSpecialCharacters(event)'  id='PatientInstructions" + ParentRowId + "'  name='PatientInstructions'></input></div>";
        var Volume = "<div class='col-xs-1'><label class='control-label'>Volume</label><input type='number' onkeydown='ClinicalRadiologyOrderDetail.validateVolume(event,this)' class='form-control' id='VolumeText" + ParentRowId + "' name='VolumeText'></input></div>";
        var ddlVolume = "<div class='col-xs-2'><label class='control-label'></label><select id='VolumeDDL" + ParentRowId + "' name='VolumeDDL' class='form-control' ddlist='GetVolume'></select></div>";
        var FillerInstructions = "<div class='col-xs-3'><label class='control-label'>Filler Instructions</label><input type='text' maxlength='500' class='form-control' onkeypress='ClinicalRadiologyOrderDetail.validateSpecialCharacters(event)' id='FillerInstructions" + ParentRowId + "' name='FillerInstructions'></input></div>";


        var spacer = '<div class="spacer5"></div>';
        ChildHTML.append(ddlSpecimen, Volume, ddlVolume, PatientInstructions, FillerInstructions);
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
    //Function Name: validateVolume
    //Description: This function will validate the volume input
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
        AppPrivileges.GetFormPrivileges("Orders and Results_Diagnostic Imaging Order", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ChargeCapId"] = ChargeCapId;
                params["mode"] = "Edit";
                //Edited by Azeem Raza Tayyab on 16-Feb-2016 to fix bug#: PMS-3871
                if (ClinicalRadiologyOrderDetail.params.TabID == 'chargeBatchDetail' || ClinicalRadiologyOrderDetail.params.TabID == 'billTabUnClaimedAppointment' || ClinicalRadiologyOrderDetail.params.TabID == 'Bill_ChargeSearch' || ClinicalRadiologyOrderDetail.params.TabID == 'Patient_Case_Detail' || ClinicalRadiologyOrderDetail.params.TabID == 'schTabCalendar' || ClinicalRadiologyOrderDetail.params.TabID == 'batchTabEncounter' || ClinicalRadiologyOrderDetail.params.TabID == 'Bill_FollowUpPatientAR_Detail' || ClinicalRadiologyOrderDetail.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || ClinicalRadiologyOrderDetail.params.TabID == "billTabClaimSubmission" || ClinicalRadiologyOrderDetail.params.TabID == "Bill_PaymentPosting" || ClinicalRadiologyOrderDetail.params.TabID == "EDIClaimViewDetail")
                    params["ParentCtrl"] = 'ClinicalRadiologyOrderDetail';
                else
                    params["ParentCtrl"] = ClinicalRadiologyOrderDetail.params["TabID"];
                LoadActionPan('chargeSearchDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    EnableDisableTestSearch: function (ddlLab) {

        var LabId = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #ddlLabId option:selected').val();
        var ProviderId = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #hfProvider').val();
        if (ProviderId != '' && LabId != '') {
            $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #txtRadiologyCPTCode').removeClass('disableAll');
            $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #favSectionDiv').removeClass('disableAll');
            ClinicalRadiologyOrderDetail.favoriteListSearch();
        }
        else {
            $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #txtRadiologyCPTCode').addClass('disableAll');
            $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #favSectionDiv').addClass('disableAll');
        }

    },

    rowRemove: function ($row, obj) {
        utility.myConfirm('1', function () {


            if ($row.hasClass('adding')) {
            }
            //var _self = obj;
            //_self.datatable.row($row.get(0)).remove().draw();
            if (parseInt($row.attr("id")) > 0) {
                ClinicalRadiologyOrderDetail.DeleteRadiologyOrderTest($row.attr("id"), $row, obj);
            }
            else {
                var _self = obj;
                _self.datatable.row($row.get(0)).remove().draw();
                utility.DisplayMessages("Successfully Deleted", 1);
                ClinicalRadiologyOrderDetail.enableDisableRadiologyOrderButtons();
            }



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

    AddNewRadiologyRow: function (RowId, mode, CurrRef, cptCode, procDesc, cptDescription, SNOMEDId, SNOMEDDescription) {

        var dfdObject = $.Deferred();
        var RadiologyGridId = "#" + ClinicalRadiologyOrderDetail.params.PanelID + " #dgvRadiology";

        var CurrentRow = null;
        if (RowId && RowId > 0) {
            if (ClinicalRadiologyOrderDetail.params.ParentCtrl != null) {
                CurrentRow = ClinicalRadiologyOrderDetail.EditableGrid.rowAdd(RowId, "");
            }
            else {
                CurrentRow = ClinicalRadiologyOrderDetail.EditableGrid.rowAdd(RowId, ClinicalRadiologyOrderDetail.params.RadiologyId);
            }

        }
        else {
            var TemplateRow = $("#" + ClinicalRadiologyOrderDetail.params.PanelID + " #dgvRadiology tbody tr[id*='-']").last();
            var TemplateRowId = 0;
            if (TemplateRow.length > 0) {
                if (TemplateRow.attr("id").indexOf("Child") != "-1")
                    TemplateRowId = parseInt(TemplateRow.prev().attr("id"));
                else
                    TemplateRowId = parseInt(TemplateRow.attr("id"));
            }
            CurrentRow = ClinicalRadiologyOrderDetail.EditableGrid.rowAdd(TemplateRowId - 1, "");
        }
        var rowId = CurrentRow.attr("id");

        var cptCodeHtml = '<input type="hidden" id="CPTCode' + rowId + '"  name="CPTCode" value="' + cptCode + '" />';
        var cptDescHtml = '<input type="hidden" id="CPTDescription' + rowId + '" name="CPTDescription"  value="' + procDesc + '"  />';
        $(CurrentRow).find('td:first').append(cptCodeHtml + cptDescHtml);

        var snomedCodeHtml = '<input type="hidden" id="CPTSNOMEDCodeId' + rowId + '"  name="CPTSNOMEDCodeId" value="' + SNOMEDId + '" />';
        var snomedDescHtml = '<input type="hidden" id="CPTSNOMEDDescription' + rowId + '" name="CPTSNOMEDDescription"  value="' + SNOMEDDescription + '"  />';
        $(CurrentRow).find('td:first').append(snomedCodeHtml + snomedDescHtml);

        var row = ClinicalRadiologyOrderDetail.EditableGrid.datatable.row(CurrentRow);
        row.child(ClinicalRadiologyOrderDetail.buildRowChild(row.data(), CurrentRow.attr("id"))).show();
        row.child().attr("id", "Child" + CurrentRow.attr("id"));

        row.child.hide();
        //, cptCode, procDesc, cptDescription
        if (cptCode != null && cptCode != "") {
            $(CurrentRow).find('input[id*="RadiologyProcedure"]').val(cptCode + " " + procDesc);
        }
        else if (procDesc != null) {
            $(CurrentRow).find('input[id*="RadiologyProcedure"]').val(procDesc.trim());
        }

        ClinicalRadiologyOrderDetail.enableRemoveRow($(CurrentRow));
        ClinicalRadiologyOrderDetail.enableDisableRadiologyOrderButtons();

        //Start Farooq Ahmad 07/14/2016 /EMR-588
        var dgvRadiology = $("#" + ClinicalRadiologyOrderDetail.params.PanelID + " #dgvRadiology");

        $(dgvRadiology).find('input[id*="dtpRadiologyDate"]').removeClass('size80');
        $(dgvRadiology).find('input[id*="dtpRadiologyDate"]').closest('div').removeClass('size80');

        $(dgvRadiology).find('input[id*="tpRadiologyTime"]').removeClass('size80');
        $(dgvRadiology).find('input[id*="tpRadiologyTime"]').closest('div').removeClass('size80');

        $(dgvRadiology).find('input[id*="RadiologyProcedure"]').addClass('size-min300');
        //End Farooq Ahmad 07/14/2016 /EMR-588

        $(CurrentRow).loadDropDowns(true).done(function () {

            if (row.child().length > 0) {

                row.child().loadDropDowns(true).done(function () {

                    row.child().find('select,datalist').each(function () {
                        var data = null;
                        if ($(this).attr('id').indexOf("Specimen") == 0) {
                            data = {
                                'StrID': cptCode, 'ID': -1, 'StrID2': 'Prefer', 'StrID3': $(this).attr('id')
                            };
                            ddlSpecimen = this;
                            ClinicalRadiologyOrderDetail.ddlSpecimen.push(this);

                            return MDVisionService.lookups('GetSpecimen', true, data).done(function (results) {
                                results = JSON.parse(results["GetSpecimen"]);
                                if (results) {
                                    var l = null;
                                    for (var count in ClinicalRadiologyOrderDetail.ddlSpecimen) {
                                        if ($(ClinicalRadiologyOrderDetail.ddlSpecimen[count]).attr('id') == results.DropDownId) {
                                            l = $(ClinicalRadiologyOrderDetail.ddlSpecimen[count]);
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
                                dfdObject.resolve(CurrentRow);
                            });
                        }
                        else {
                            return true;
                        }
                    });
                });
            }
            else {
                dfdObject.resolve(CurrentRow);
            }
        });

        return dfdObject.promise();
    },

    //Author: Farooq Ahmad
    //Date: 28-03-2016
    //Reason:to add more problem is Associated Problem list
    addProblem: function () {
        var params = [];
        params["CurrentNotesProviderId"] = ClinicalRadiologyOrderDetail.params["CurrentNotesProviderId"];
        params["IsFromNote"] = ClinicalRadiologyOrderDetail.params["IsFromNote"]
        params["RefForm"] = "frmClinicalRadiologyOrderDetail";
        params["FromOrderDetail"] = "1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ClinicalRadiologyOrderDetail";
        LoadActionPan('Clinical_ProblemLists', params);
    },

    GetRadiologyRowsJSON: function () {

        var RadiologyTestIds = $("#" + ClinicalRadiologyOrderDetail.params.PanelID + " #dgvRadiology tbody tr:not([id*=Child]").map(function () {
            return this.id.replace("id", "");
        }).get().join(',');

        $("#" + ClinicalRadiologyOrderDetail.params.PanelID + " #pnlRadiology_Result #hfRadiologyTestIds").val(RadiologyTestIds);

        var self = $("#" + ClinicalRadiologyOrderDetail.params.PanelID + " #pnlRadiology_Result");
        var myJSON = self.getMyJSONByName();

        ClinicalRadiologyOrderDetail.SaveRadiologyOrderTest(myJSON);

    },

    SaveRadiologyOrderTest: function (objRadiology) {
        ClinicalRadiologyOrderDetail.RadiologyOrderTestSave_DBCall(objRadiology).done(function (response) {

            var response = JSON.parse(response);
            if (response.status != false) {

            } else {

            }


        });
    },

    RadiologyOrderTestSave_DBCall: function (RadiologyOrderData) {

        if (ClinicalRadiologyOrderDetail.params.mode.toLowerCase() == "add") {

            var objData = JSON.parse(RadiologyOrderData);
            objData["commandType"] = "save_radiology_order_test";
            var data = JSON.stringify(objData);
            return MDVisionService.APIService(data, "RadiologyOrder", "RadiologyOrder");

        } else if (ClinicalRadiologyOrderDetail.params.mode.toLowerCase() == "edit") {
            var objData = JSON.parse(RadiologyOrderData);
            objData["commandType"] = "save_radiology_order_test";
            objData["RadiologyOrderId"] = $("#" + ClinicalRadiologyOrderDetail.params.PanelID + ' #hfRadiologyOrderId').val();
            var data = JSON.stringify(objData);
            return MDVisionService.APIService(data, "RadiologyOrder", "RadiologyOrder");
        }





    },

    RadiologyOrderTestGridLoad: function (response) {
        var response = JSON.parse(response);
        var PanelRadiologyGrid = "#" + ClinicalRadiologyOrderDetail.params.PanelID + " #pnlRadiology_Result";
        var RadiologyGridId = "#" + ClinicalRadiologyOrderDetail.params.PanelID + " #dgvRadiology";
        $(RadiologyGridId + " tbody tr").remove();


        if ($.fn.dataTable.isDataTable(RadiologyGridId)) {
            $(RadiologyGridId).dataTable().fnClearTable();
            $(RadiologyGridId).dataTable().fnDestroy();
        }

        ClinicalRadiologyOrderDetail.EditableGrid.datatable.clear().draw();
        ClinicalRadiologyOrderDetail.ddlSpecimen = [];
        ClinicalRadiologyOrderDetail.ddlAlternativeSpecimen = [];
        var RadiologyOrderTestLoadJSONData = JSON.parse(response.radiologyOrderTest_JSON);

        $.each(RadiologyOrderTestLoadJSONData, function (i, item) {

            var dfd = $.Deferred();

            var CurrentRow = null;
            var newChildRow = null;

            var createCurrentRow = function (i, item, CurrentRow, newChildRow) {

                var RadiologyOrderTestId = item.RadiologyOrderTestId;

                ClinicalRadiologyOrderDetail.AddNewRadiologyRow(RadiologyOrderTestId, null, null, item.CPTCode, item.CPTDescription, null, null, null).done(function (parentRow) {

                    CurrentRow = parentRow;
                    var row = ClinicalRadiologyOrderDetail.EditableGrid.datatable.row(CurrentRow);
                    item.CurrentRow = CurrentRow;
                    newChildRow = row.child();
                    item.newChildRow = newChildRow;

                    dfd.resolve(CurrentRow, newChildRow, item);
                    $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").data('serialize', $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail").serialize());
                });
                return dfd.promise();
            }

            //Start Farooq Ahmad 03/28/2016 bind values to the table

            var BindFunction = function (CurrentRow, newChildRow, item) {
                if (CurrentRow == null) {
                    CurrentRow = item.CurrentRow;
                }
                if (newChildRow == null) {
                    newChildRow = item.newChildRow;
                }
                utility.bindMyJSONByName(true, item, false, $(CurrentRow)).done(function () {
                    utility.bindMyJSONByName(true, item, false, $(newChildRow));
                });
                //show button controls
                ClinicalRadiologyOrderDetail.enableDisableRadiologyOrderButtons();
            }

            //End Farooq Ahmad 03/28/2016 bind values to the table
            $.when(createCurrentRow(i, item, CurrentRow, newChildRow)).done(function (CurrentRow, newChildRow, item) {
                BindFunction(CurrentRow, newChildRow, item);
            });
        });
    },

    DeleteRadiologyOrderTest: function (RadiologyTestId, $row, obj) {

        ClinicalRadiologyOrderDetail.RadiologyOrderTest_DBCall(RadiologyTestId).done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {
                utility.DisplayMessages(response.Message, 1);
                var _self = obj;
                _self.datatable.row($row.get(0)).remove().draw();
            } else {
                utility.DisplayMessages(response.Message, 3);
            }
            ClinicalRadiologyOrderDetail.enableDisableRadiologyOrderButtons();
        });

    },

    RadiologyOrderTest_DBCall: function (RadiologyTestId) {

        var objData = new Object();
        objData["RadiologyOrderTestId"] = RadiologyTestId;
        objData["commandType"] = "DELETE_RADIOLOGYORDER_TEST";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyOrder", "RadiologyOrder");
    },


    //Function Name: getLatestRadiologyOrderByPatientId
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: get Latest Radiology Order By PatientId
    getLatestRadiologyOrderByPatientId: function (hideAlertMessage) {
        var strMessage = '';
        //   AppPrivileges.GetFormPrivileges("Notes_Notes", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            ClinicalRadiologyOrderDetail.getLatestRadiologyOrderByPatientIdDBCall().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    ClinicalRadiologyOrderDetail.createRadiologyOrderBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage);
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

    //Function Name: createRadiologyOrderBodyHTML
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: To create Radiology Order's Body HTML
    createRadiologyOrderBodyHTML: function (response, NoteHTMLCtrl, UnloadRadiologyOrder, hideAlertMessage) {
        ClinicalRadiologyOrderDetail.checkRadiologyOrderExists();
        if (response.RadiologyOrderFill_JSON != null && response.RadiologyOrderFill_JSON != '') {
            var RadiologyOrderFill_Obj = JSON.parse(response.RadiologyOrderFill_JSON);
            var $mainDivRadiologyOrder = $(document.createElement('div'));

            var RadiologyOrderId = RadiologyOrderFill_Obj.RadiologyOrderId;
            if (RadiologyOrderId > 0) {
                var $SectionBodyRadiologyOrder = $(document.createElement('section'));
                $SectionBodyRadiologyOrder.attr('id', "Cli_RadiologyOrderDetail_Main" + RadiologyOrderId);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_RadiologyOrderDetail_" + RadiologyOrderId);
                var $ListRadiologyOrder = $(document.createElement('ul'));

                $ListRadiologyOrder.attr('class', 'list-unstyled')

                $SectionBodyRadiologyOrder.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_RadiologyOrderDetail_" + RadiologyOrderId + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_RadiologyOrderDetail_Main" + RadiologyOrderId + '"  ><i class="fa fa-times"></i></a></div> ');


                $ListRadiologyOrder.append("<li>" + RadiologyOrderFill_Obj.SoapText + "</li>");
                $DetailsDiv.append($ListRadiologyOrder);
                $SectionBodyRadiologyOrder.append($DetailsDiv);
                if ($(NoteHTMLCtrl + ' clinical_radiologyorder').parent().parent().find('#Cli_RadiologyOrderDetail_Main' + RadiologyOrderId).length == 0) {
                    $mainDivRadiologyOrder.append($SectionBodyRadiologyOrder);
                    ClinicalRadiologyOrderDetail.updateRadiologyOrderHtml($mainDivRadiologyOrder.html(), RadiologyOrderId, NoteHTMLCtrl, hideAlertMessage);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' clinical_radiologyorder').parent().parent().find('#Cli_RadiologyOrderDetail_Main' + RadiologyOrderId + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' clinical_radiology').parent().parent().find('#Cli_RadiologyOrder_Main' + RadiologyOrderId + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' clinical_radiologyorder').parent().parent().find('#Cli_RadiologyOrderDetail_Main' + RadiologyOrderId).html($SectionBodyRadiologyOrder.html());
                    $(NoteHTMLCtrl + ' clinical_radiologyorder').parent().parent().find('#Cli_RadiologyOrderDetail_Main' + RadiologyOrderId + ' ul').append(CommentHTML);
                    Clinical_ProgressNote.saveComponentSOAPText("Diagnostic Imaging Order", hideAlertMessage);
                    ClinicalRadiologyOrderDetail.updateRadiologyOrderHtml("", RadiologyOrderId, NoteHTMLCtrl, hideAlertMessage);

                }

                if (UnloadRadiologyOrder == true) {
                    ClinicalRadiologyOrderDetail.Unload(ClinicalRadiologyOrderDetail.bNextPrev);
                }
            }
        }
    },

    //Function Name: detach_ComponentsRadiologyOrder
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: To detach Components Radiology Order
    detach_ComponentsRadiologyOrder: function (ComponentName, IsUpdate, RadiologyOrderComponentRemove) {
        var radiologyOrderIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_radiologyorder').parent().parent().find('section[id*="Cli_RadiologyOrderDetail_Main"]').map(function () {
            return this.id.replace("Cli_RadiologyOrderDetail_Main", "");
        }).get().join(',');

        var docIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_radiologyorder').parent().parent().find('section[id*="Cli_RadiologyOrderDetail_Main"]').map(function () {
            return $(this).attr('patdocid');
        }).get().join(',');

        var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .RadiologyOrdersComponent,.DiagnosticImagingOrderComponent').attr('NoteComponentId');
        if (!NoteComponentId)
            NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .RadiologyOrdersComponent,.DiagnosticImagingOrderComponent').attr('NoteComponentId');
        if (RadiologyOrderComponentRemove) {
            if (NoteComponentId && NoteComponentId != "NCDummyId") {
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_radiologyorder').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Diagnostic Imaging Order', true))
                }
                else
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                $.when.apply($, promise).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_radiologyorder').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
            else {
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Radiology Order']").remove();
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_radiologyorder').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Diagnostic Imaging Order', true))
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
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_radiologyorder').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_radiologyorder').parent().parent().find('section[id*="Cli_RadiologyOrderDetail_Main"]').remove();
        }

        if (radiologyOrderIds == "" || radiologyOrderIds == "undefined") {
            $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Radiology Order']").remove();
            var promise = [];
            if (Clinical_ProgressNote.params["TemplateName"]) {
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_radiologyorder').parent().parent().addClass('hidden');
                promise.push(Clinical_ProgressNote.saveComponentSOAPText('Diagnostic Imaging Order', true))
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
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_radiologyorder').parent().parent().remove();
                Clinical_ProgressNote.ShowHideComponetsHeaders();
                utility.DisplayMessages('Successfully Deleted', 1);
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
            });
        }
        else {

            ClinicalRadiologyOrderDetail.detachRadiologyOrderFromNotesDBCall(radiologyOrderIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (docIds.length > 0) {
                        Clinical_ProgressNote.detachImagesComponentFromNotes_DBCall(docIds).done(function (responseDoc) {
                            if (responseDoc.status != false) {
                                Patient_Document.DeleteDocument(docIds);
                                if (IsUpdate) {
                                    Clinical_ProgressNote.saveComponentSOAPText("Diagnostic Imaging Order", true);
                                    Clinical_ProgressNote.HideShowBillingInfo();
                                }
                                utility.DisplayMessages(response.Message, 1);
                                Clinical_ProgressNote.updateAttachDocumentButtonImg();
                                setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                            }
                            else {
                                utility.DisplayMessages(responseDoc.Message, 3);
                            }
                        });
                    }
                    else {
                        if (IsUpdate) {
                            Clinical_ProgressNote.saveComponentSOAPText("Diagnostic Imaging Order", true);
                            //Start 01-12-2016 Humaira Yousaf to hide show eSuperbill link
                            Clinical_ProgressNote.HideShowBillingInfo();
                            //End 01-12-2016 Humaira Yousaf to hide show eSuperbill link
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
    },

    //Function Name: detachRadiologyOrderFromNotes
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: To detach Radiology Order from Notes
    detachRadiologyOrderFromNotes: function (RadiologyOrderId) {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('1', function () {
                EMRUtility.scrollToPNcomponent('clinical_radiologyorder');
                var selectedValue = RadiologyOrderId.replace('Cli_RadiologyOrderDetail_Main', '');
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    ClinicalRadiologyOrderDetail.detachRadiologyOrderFromNotes_DBCall(selectedValue).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            var docId = $("#" + RadiologyOrderId).attr('patdocid');
                            if (docId > 0) {
                                Clinical_ProgressNote.DetachImageFromNotes_DBCall(docId).done(function (responseDoc) {
                                    if (responseDoc.status != false) {
                                        $('#' + RadiologyOrderId).remove();
                                        Patient_Document.DeleteDocument(docId);
                                        Clinical_ProgressNote.updateAttachDocumentButtonImg();
                                        Clinical_ProgressNote.saveComponentSOAPText("Diagnostic Imaging Order");
                                        setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                                    }
                                    else {
                                        utility.DisplayMessages(responseDoc.Message, 3);
                                    }
                                });
                            }
                            else {
                                $('#' + RadiologyOrderId).remove();
                                Clinical_ProgressNote.saveComponentSOAPText("Diagnostic Imaging Order");
                                setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                            }
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

    //Function Name: detachRadiologyOrderFromNotes_DBCall
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: DB Call to detach Radiology Order from Notes
    detachRadiologyOrderFromNotes_DBCall: function (RadiologyOrderId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["RadiologyOrderId"] = RadiologyOrderId;
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
        objData["commandType"] = "detach_radiologyorder_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "RadiologyOrder", "RadiologyOrder");
    },

    //Function Name: detachRadiologyOrderFromNotesDBCall
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: DB Call to detach Radiology Order From Notes
    detachRadiologyOrderFromNotesDBCall: function (RadiologyId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["RadiologyOrderId"] = RadiologyId;
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
        objData["commandType"] = "detach_RadiologyOrder_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "RadiologyOrder", "RadiologyOrder");
    },

    //Function Name: updateRadiologyOrderHtml
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: To update Radiology Order Html
    updateRadiologyOrderHtml: function (RadiologyOrderHtml, RadiologyOrderId, NoteHTMLCtrl, hideAlertMessage) {
        $(NoteHTMLCtrl + ' clinical_radiologyorder').parent().parent().addClass('initialVisitBody');
        if (RadiologyOrderHtml != '') {
            $(NoteHTMLCtrl + ' clinical_radiologyorder').parent().parent().append(RadiologyOrderHtml);
        }
        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (RadiologyOrderHtml != '') {
            ClinicalRadiologyOrderDetail.attachRadiologyOrderWithNotes(RadiologyOrderId, hideAlertMessage);
        }

    },

    //Function Name: attachRadiologyOrderWithNotes
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: To attach Radiology Order With Notes
    attachRadiologyOrderWithNotes: function (RadiologyOrderId, hideAlertMessage) {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {

            var selectedValue = RadiologyOrderId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                ClinicalRadiologyOrderDetail.attachRadiologyOrderWithNotesDBCall(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        //If Attached MedicalHx Made new inseration to MedicalHx Table than good ids should be attached to HTML
                        Clinical_ProgressNote.saveComponentSOAPText("Diagnostic Imaging Order", hideAlertMessage);
                        $('#' + RadiologyOrderId).remove();
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

    //Function Name: attachRadiologyOrderWithNotesDBCall
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: DB Call to attach Radiology Order With Notes
    attachRadiologyOrderWithNotesDBCall: function (RadiologyOrderId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["RadiologyOrderId"] = RadiologyOrderId;
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
        objData["commandType"] = "attach_RadiologyOrder_with_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "RadiologyOrder", "RadiologyOrder");
    },

    //Function Name: getLatestRadiologyOrderByPatientIdDBCall
    //Author Name: Ahmad Raza
    //Created Date: 18-03-2016
    //Description: DB Call to get Latest Radiology Order By PatientId
    getLatestRadiologyOrderByPatientIdDBCall: function () {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["commandType"] = "getlatest_Radiologyorderby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyOrder", "RadiologyOrder");
    },

    getRadiologyOrderInfo: function (radiologyOrderId) {
        ClinicalRadiologyOrderDetail.fillRadiologyOrder(radiologyOrderId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    ClinicalRadiologyOrderDetail.createRadiologyOrderBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML');

                }
                else {
                    utility.DisplayMessages(strMessage, 3);
                }
            }
        });
    },
    //Function Name: printRadiologyOrder
    //Author Name: Humaira Yousaf
    //Created Date: 23-03-2016
    //Description: Creates PDF to view Radiology Order
    printRadiologyOrder: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["UserId"] = globalAppdata['AppUserId'];
        params["PatientId"] = Clinical_RadiologyOrder.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        params["ParentCtrl"] = "ClinicalRadiologyOrderDetail";
        params["RadiologyOrderId"] = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfRadiologyOrderId").val();
        Clinical_RadiologyOrderView.radiologyOrderPreview(params.PatientId, params.UserId, params.RadiologyOrderId);
        //LoadActionPan('Clinical_RadiologyOrderView', params);
    },

    //Author: Humaira Yousaf
    //Date :  28-11-2016
    //Description: Checks if any associated problems is selected on saving order
    problemAdded: function () {

        var problemsSelected = false;
        if ($('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #ulProblemLists input:checked").length > 0) {
            problemsSelected = true;
        }

        return problemsSelected;
    },

    BindFacilityTo: function () {
        var Ctrl = $("#" + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #txtFacilityTo");
        var hfCtrl = $("#" + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #hfFacilityTo");
        var func = function () { return utility.GetProvidersDiagnosticImagingFacilityArray(Ctrl.val(), $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #hfProvider').val()) };
        var onChange = function (valid) { ClinicalRadiologyOrderDetail.removeFacility($("#" + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #txtFacilityTo")) }
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, null, onChange);
    },

    OnInputFacilityTo: function () {
        if ($('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #hfProvider').val() == "") {
            utility.DisplayMessages("Please select provider first.", 2);
            $("#" + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #txtFacilityTo").val("");
        }
    },

    GetFacilityArrayByName: function (ShortName, IsAccountWithFullName, IsGetAll) {
        var AllPatients = [];
        var IsValid = false;
        if (IsGetAll === undefined) {
            if (ShortName != null && ShortName.length > 2)
                IsValid = true;
            else
                IsValid = false;
        } else {
            IsValid = true;
        }

        var dfd = new $.Deferred();
        if (IsValid) {
            // serach parameter , class name, command name of class
            Patient_Referrals_Outgoing_Detail.LoadActiveFacilitiesByName(ShortName).done(function (responseData) {
                responseData = JSON.parse(responseData);
                if (responseData.status != false) {
                    if (responseData.PatientCount > 0) {
                        var PatientLoadJSONData = JSON.parse(responseData.PatientLoad_JSON);
                        $.each(PatientLoadJSONData, function (i, item) {

                            AllPatients.push({ id: item.FacilityId, value: item.FacilityId, ShortName: item.Description });


                        });
                    }
                }

                dfd.resolve(AllPatients);
            });
        }
        else {
            dfd.resolve(AllPatients);
        }

        return dfd.promise();

    },
    LoadActiveFacilitiesByName: function (Searchstring) {
        var objData = new Object();
        objData["ShortName"] = Searchstring;
        objData["commandType"] = "SEARCH_FACILITY_BY_SHORTNAME";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");

    },
    removeFacility: function (ctrl) {
        if ($(ctrl).val() == "") {
            $("#" + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #hfFacilityTo").val("");
        }

    },
    OpenFacilityTo: function () {
        if ($("#" + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #txtFacilityTo").hasClass('ui-autocomplete-input')) {
            $("#" + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #txtFacilityTo").autocomplete("destroy");
        }

        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmClinicalRadiologyOrderDetail";
        params["FacilityTo"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacilityTo";
        params["RefHiddenIdCtrl"] = "hfFacilityTo";
        params["ProviderId"] = $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfProvider").val() ? $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #hfProvider").val() : "-1";
        params["LoadAllFacility"] = "True";
        params["ParentCtrl"] = "ClinicalRadiologyOrderDetail";
        LoadActionPan('Admin_Facility', params);
    },
}