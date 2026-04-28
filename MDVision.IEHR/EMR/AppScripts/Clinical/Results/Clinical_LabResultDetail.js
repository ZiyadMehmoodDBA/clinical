
ClinicalLabResultDetail = {
    params: [],
    bIsFirstLoad: true,
    EditableGrid: null,
    myGrid: null,
    LabResultNewParnetRows: [],
    rowsParentChild: [],
    deletedChildRows: [],
    isError: false,
    isAttribute: null,
    explicitResultOrderId: [],
    loadProviderFromAppData: false,
    loadFacilityFromAppData: false,
    ProviderOnDemandLoad: false,
    FacilityOnDemandLoaded: false,
    isProviderLoaded: false,
    isFacilityLoaded: false,
    Organisms: [],
    Antimicrobials: [],
    isEnterococcus: false,
    FavListName: "LabResultDetail",
    Load: function (params) {

        BackgroundLoaderShow(true);
        ClinicalLabResultDetail.Organisms = [];

        ClinicalLabResultDetail.params = params;

        //Start//Abid Ali // For bug# EMR-896
        $('#' + ClinicalLabOrderDetail.params.PanelID + ' #divLabOrderInformation').hide();
        $('#' + ClinicalLabOrderDetail.params.PanelID + ' #divLabBillingInformation').hide();
        //End//Abid Ali // For bug# EMR-896

        if (ClinicalLabResultDetail.params.ParentCtrl == "ClinicalLabOrderDetail") {
            var $printButton = $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #btnPrintResult");
            $printButton.addClass("disableAll");
        }

        if (ClinicalLabResultDetail.params.FromLabDetail) {
            var $printButton = $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #btnPrintResult");
            $printButton.addClass("disableAll");
        }

        if (ClinicalLabResultDetail.params.PanelID != 'pnlClinicalLabResultDetail') {
            ClinicalLabResultDetail.params.PanelID = ClinicalLabResultDetail.params.PanelID + ' #pnlClinicalLabResultDetail';
        } else {
            ClinicalLabResultDetail.params.PanelID = 'pnlClinicalLabResultDetail';
        }
        var labOrderId = ClinicalLabResultDetail.params.LabOrderId;
        if (labOrderId != null && labOrderId > 0) {
            $('#' + ClinicalLabResultDetail.params.PanelID + " #btnScanResult,#btnViewAttachment,#btnViewPDF").removeClass("hidden");
        }
        //For opening result detail page in edit mode
        if (ClinicalLabResultDetail.params.mode == "Edit") {

            // Loads Lab results
            var labResultId = ClinicalLabResultDetail.params.LabResultId;
            var labOrderId = ClinicalLabResultDetail.params.LabOrderId;
            ClinicalLabResultDetail.showHideEditableControls(true);

            ClinicalLabResultDetail.fillLabResult(labResultId, labOrderId);

            $('#' + ClinicalLabResultDetail.params.PanelID + " #hfLabResultId").val(labResultId);
            ClinicalLabResultDetail.validateLabResultDetail();
        }
        $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #hfPatientId").val($("div#PatientProfile #hfPatientId").val());
        if (Clinical_LabOrder.params.ParentCtrl == "Clinical_FaceSheet") {
            $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #hfPatientId").val(Clinical_FaceSheet.params.patientID);
        }
        $("#" + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #btnSaveOrder,#btnSignPrintOrder,#btnSignOrder,#btnResetLabResult").addClass('disableAll');

        var self = $('#' + ClinicalLabResultDetail.params.PanelID);
        self.find('#btnViewAttachment').dropdown();

        if (ClinicalLabResultDetail.bIsFirstLoad == true) {

            ClinicalLabResultDetail.bIsFirstLoad = false;

            //  if (ClinicalLabResultDetail.params.mode == "Add") {
            // Populate dropdowns
            //for Favorites toggle
            EMRUtility.setFavoriteSectionStyle(ClinicalLabResultDetail.params.PanelID);
            self.loadDropDowns(true).done(function () {
                $('#' + ClinicalLabResultDetail.params.PanelID + " #ddlStatus").val("Final");
                $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail").data('serialize', $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail").serialize());
            });
        }

        // For Creating Order and Result both at the same time
        if (ClinicalLabResultDetail.params.mode == "Add") {

            $('#' + ClinicalLabResultDetail.params.PanelID + " #btnViewPDF").addClass("disableAll");

            //Enable disable Print button
            ClinicalLabResultDetail.enableDisableOrderResultButton('btnPrintResult', 'hfLabOrderResultId');
            ClinicalLabResultDetail.enableDisableOrderResultButtons();

            //Initialize data table
            var PanelLabResultGrid = "#" + ClinicalLabResultDetail.params.PanelID + " #pnlLab_ResultDetail";
            var LabResultGridId = "#" + ClinicalLabResultDetail.params.PanelID + " #dgvLabResultDetail";

            if ($.fn.dataTable.isDataTable(LabResultGridId)) {
                $(LabResultGridId).dataTable().fnClearTable();
                $(LabResultGridId).dataTable().fnDestroy();
            }
            ClinicalLabResultDetail.EditableGrid = EMRUtility.MakeEditableGrid(PanelLabResultGrid, LabResultGridId, ClinicalLabResultDetail, 0, false, true, false, false, false, null);
            $(LabResultGridId + " tbody tr").remove();

            ClinicalLabResultDetail.params.LabResultId = -1;
            var labOrderId = ClinicalLabResultDetail.params.LabOrderId;
            $('#' + ClinicalLabResultDetail.params.PanelID + " #hfLabResultId").val(-1);

            ClinicalLabResultDetail.showHideEditableControls(false);
            //if (Clinical_LabOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
            //    CacheManager.BindCodes('GetProvider', false).done(function (result) {
            //        $("#frmClinicalLabResultDetail #txtProvider").autocomplete({
            //            autoFocus: true,
            //            source: Providers,
            //            select: function (event, ui) {

            //                setTimeout(function () {
            //                    $('#' + ClinicalLabResultDetail.params.PanelID + " #txtProvider").attr("Provider", ui.item.id);
            //                    $('#' + ClinicalLabResultDetail.params.PanelID + " #hfProvider").val(ui.item.id);
            //                }, 100);
            //            }
            //        });
            //        $('#' + ClinicalLabResultDetail.params.PanelID + " #txtProvider").val(Clinical_ProgressNote.params["CurrentNotesProviderText"]);
            //        $('#' + ClinicalLabResultDetail.params.PanelID + " #txtProvider").attr("Provider", Clinical_ProgressNote.params["CurrentNotesProviderId"]);
            //        $('#' + ClinicalLabResultDetail.params.PanelID + " #hfProvider").val(Clinical_ProgressNote.params["CurrentNotesProviderId"]);
            //    });

            //    CacheManager.BindCodes('GetFacility', false).done(function (result) {
            //        $("#frmClinicalLabResultDetail #txtFacility").autocomplete({
            //            autoFocus: true,
            //            source: Facilities,
            //            select: function (event, ui) {

            //                setTimeout(function () {
            //                    $('#' + ClinicalLabResultDetail.params.PanelID + " #txtFacility").attr("Facility", ui.item.id);
            //                    $('#' + ClinicalLabResultDetail.params.PanelID + " #hfFacility").val(ui.item.id);
            //                }, 100);
            //            }
            //        });
            //        $('#' + ClinicalLabResultDetail.params.PanelID + " #txtFacility").val(Clinical_ProgressNote.params["NotesFacilityName"]);
            //        $('#' + ClinicalLabResultDetail.params.PanelID + " #txtFacility").attr("Facility", Clinical_ProgressNote.params["NotesFacilityIDForFollowUp"]);
            //        $('#' + ClinicalLabResultDetail.params.PanelID + " #hfFacility").val(Clinical_ProgressNote.params["NotesFacilityIDForFollowUp"]);
            //    });

            //} else {
            //    //ClinicalLabResultDetail.loadAllAutocomplete();
            //}


            //Load laboratries
            Clinical_LabOrder.LoadLabs('ddlLabId', ClinicalLabResultDetail.params.PanelID).done(function () {
                if (ClinicalLabResultDetail.params.mode == "Add") {
                    // -- Below code has been commented for the story: PRD-25
                    //if (ClinicalLabResultDetail.params["LastLabResultName"]) {
                    //    var theText = ClinicalLabResultDetail.params["LastLabResultName"];
                    //    if (theText != "") {
                    //        if (ClinicalLabResultDetail.params.ParentCtrlPanelID) {
                    //            $("#" + ClinicalLabResultDetail.params.ParentCtrlPanelID + ' #frmClinicalLabResultDetail #ddlLabId option:contains(' + theText + ')').attr('selected', 'selected');
                    //        } else {
                    //            $("#" + ClinicalLabResultDetail.params.PanelID + ' #frmClinicalLabResultDetail #ddlLabId option:contains(' + theText + ')').attr('selected', 'selected');
                    //        }

                    //    }
                    //} else {
                    //    $("#" + ClinicalLabResultDetail.params.PanelID + ' #ddlLabId').val("");
                    //}
                    $("#" + ClinicalLabResultDetail.params.PanelID + ' #frmClinicalLabResultDetail #ddlLabId option:contains("Point of Care")').attr('selected', 'selected');
                    ClinicalLabResultDetail.SetFavSearch();
                }
                ClinicalLabResultDetail.validateLabResultDetail(true);
                $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail").data('serialize', $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail").serialize());
            });

            // Create Date and time picker
            utility.CreateDatePicker(ClinicalLabResultDetail.params.PanelID + ' #frmClinicalLabResultDetail input[id="txtOrderDate"]', function () {
                //on-change callback method
            }, true);

            $('#' + ClinicalLabResultDetail.params.PanelID + ' #frmClinicalLabResultDetail input[id="txtOrderTime"]').timepicker({
                defaultTime: new Date()
            });

            ClinicalLabResultDetail.loadProviderFromAppData = true;
            ClinicalLabResultDetail.ProviderOnDemandLoad = true;

            if ($('#' + ClinicalLabResultDetail.params.PanelID + " #txtProvider").val() == "") {
                if (Clinical_LabOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
                    $('#' + ClinicalLabResultDetail.params.PanelID + " #txtProvider").val(Clinical_ProgressNote.params["CurrentNotesProviderText"]);
                    $('#' + ClinicalLabResultDetail.params.PanelID + " #txtProvider").attr("Provider", Clinical_ProgressNote.params["CurrentNotesProviderId"]);
                    $('#' + ClinicalLabResultDetail.params.PanelID + " #hfProvider").val(Clinical_ProgressNote.params["CurrentNotesProviderId"]);
                    utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalLabResultDetail.params.PanelID + " #txtProvider"), Clinical_ProgressNote.params.CurrentNotesProviderText, $('#' + ClinicalLabResultDetail.params.PanelID + " #hfProvider"), Clinical_ProgressNote.params.CurrentNotesProviderId);
                    ClinicalLabResultDetail.loadProviderFromAppData = false;
                } else {
                    if (globalAppdata.DefaultProviderName != "" && globalAppdata.DefaultProviderName != "- Select -") {
                        $('#' + ClinicalLabResultDetail.params.PanelID + " #txtProvider").val(globalAppdata.DefaultProviderName);
                        $('#' + ClinicalLabResultDetail.params.PanelID + " #txtProvider").attr("Provider", globalAppdata.DefaultProviderId);
                        $('#' + ClinicalLabResultDetail.params.PanelID + " #hfProvider").val(globalAppdata.DefaultProviderId);
                        utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalLabResultDetail.params.PanelID + " #txtProvider"), globalAppdata.DefaultProviderName, $('#' + ClinicalLabResultDetail.params.PanelID + " #hfProvider"), globalAppdata.DefaultProviderId);
                        ClinicalLabResultDetail.loadProviderFromAppData = true;
                    }
                }
            }
            if ($('#' + ClinicalLabResultDetail.params.PanelID + " #txtFacility").val() == "") {
                //globalAppdata.DefaultProviderName != "" && globalAppdata.DefaultProviderName != "- Select -"
                if (Clinical_LabOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
                    $('#' + ClinicalLabResultDetail.params.PanelID + " #txtFacility").val(Clinical_ProgressNote.params["NotesFacilityName"]);
                    $('#' + ClinicalLabResultDetail.params.PanelID + " #txtFacility").attr("Facility", Clinical_ProgressNote.params["NotesFacilityIDForFollowUp"]);
                    $('#' + ClinicalLabResultDetail.params.PanelID + " #hfFacility").val(Clinical_ProgressNote.params["NotesFacilityIDForFollowUp"]);
                    utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalLabResultDetail.params.PanelID + " #txtFacility"), Clinical_ProgressNote.params.NotesFacilityName, $('#' + ClinicalLabResultDetail.params.PanelID + " #hfFacility"), Clinical_ProgressNote.params.NotesFacilityIDForFollowUp);
                    ClinicalLabResultDetail.loadFacilityFromAppData = false;
                } else {
                    if (globalAppdata.DefaultFacilityName != "" && globalAppdata.DefaultFacilityName != "- Select -") {
                        $('#' + ClinicalLabResultDetail.params.PanelID + " #txtFacility").val(globalAppdata.DefaultFacilityName);
                        $('#' + ClinicalLabResultDetail.params.PanelID + " #txtFacility").attr("Facility", globalAppdata.DefaultFacilityId);
                        $('#' + ClinicalLabResultDetail.params.PanelID + " #hfFacility").val(globalAppdata.DefaultFacilityId);
                        utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalLabResultDetail.params.PanelID + " #txtFacility"), globalAppdata.DefaultFacilityName, $('#' + ClinicalLabResultDetail.params.PanelID + " #hfFacility"), globalAppdata.DefaultFacilityId);
                        ClinicalLabResultDetail.loadFacilityFromAppData = true;
                    }
                }
            }

        }
        ClinicalLabResultDetail.domReadyFunction();
        setTimeout(function () {
            if ($("#dgvLabResultDetail").parent().hasClass('Of-a')) $("#dgvLabResultDetail").parent().removeClass('Of-a');
            $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail").data('serialize', $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail").serialize());
        }, 1000);
        ClinicalLabResultDetail.bindProvider();
        ClinicalLabResultDetail.bindFacility();
        ClinicalLabResultDetail.openAssignee();
        $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #txtProvider").focus(function () {

            if (!ClinicalLabResultDetail.isProviderLoaded)
                ClinicalLabResultDetail.ProviderOnDemandLoad = true;
            else
                ClinicalLabResultDetail.ProviderOnDemandLoad = false;

            ClinicalLabResultDetail.loadAllAutocomplete();
        });
        $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #txtFacility").focus(function () {
            if (!ClinicalLabResultDetail.isFacilityLoaded)
                ClinicalLabResultDetail.FacilityOnDemandLoad = true;
            else
                ClinicalLabResultDetail.FacilityOnDemandLoad = false;

            ClinicalLabResultDetail.loadAllAutocomplete();
        });
        if (ClinicalLabResultDetail.params.LabResultId <= 0) {
            $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #btnScanResult").addClass("disabled");
            $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #btnViewAttachment").addClass("disabled");
        }
        if (globalAppdata["isTransPubHealthAgAntimicobialUse"] && globalAppdata["isTransPubHealthAgAntimicobialUse"].toLowerCase() == "false")
            $("#" + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #dvQuestionnaire").addClass('hidden');
        ClinicalLabResultDetail.documentReady();
        if (EMRUtility.getFavListStatus(ClinicalLabResultDetail.FavListName)) {
            $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #favSectionDiv").removeClass("toggled");
            // $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #FormDiv").addClass("toggleHorContainer");
        }
        else {
            $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #favSectionDiv").addClass("toggled");
            // $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #FormDiv").removeClass("toggleHorContainer");
        }
    },
    documentReady: function () {
        $('#' + ClinicalLabResultDetail.params.PanelID + ' #txtComment').on("click", function (e) {
            if (!$('#' + ClinicalLabResultDetail.params.PanelID + ' #MacroDescDetails').is(":hidden")) {
                $('#' + ClinicalLabResultDetail.params.PanelID + ' #MacroDescDetails').hide();
            }
        });
        $('#' + ClinicalLabResultDetail.params.PanelID + ' #txtComment').on("keyup", function (e) {

            if (e.keyCode == 190 || e.keyCode == 110) // dot key is pressed
            {
                e.preventDefault();
                if ($('#' + ClinicalLabResultDetail.params.PanelID + ' #txtComment').find("#marker").length > 0) {
                    $('#' + ClinicalLabResultDetail.params.PanelID + ' #txtComment').find("#marker").remove();
                }
                EMRUtility.pasteHtmlAtCaret('<span id=marker></span>');
                if (EMRUtility.callAutopopulationOrNot(ClinicalLabResultDetail.params.PanelID, "txtComment")) {
                    $('#' + ClinicalLabResultDetail.params.PanelID + ' #txtComment').focus();
                    EMRUtility.AutoKeyWordPopulateForDiv(ClinicalLabResultDetail.params.PanelID, "txtComment", "Lab Results", 0);
                }
                else {
                    $('#' + ClinicalLabResultDetail.params.PanelID + ' #txtComment').find("#marker").remove();
                }
            }
        });
    },

    SetFavSearch: function () {

        var dfd = new $.Deferred();
        var LabId = "";
        //if ($(ddlLab).val() != '') {
        if (ClinicalLabResultDetail.params.mode.toLowerCase() == "add") {
            LabId = $('#' + ClinicalLabResultDetail.params.PanelID + ' #ddlLabId option:selected').val();
        }
        else {
            LabId = $('#' + ClinicalLabResultDetail.params.PanelID + ' #hfLabId').val();
        }
        var ProviderId = $('#' + ClinicalLabResultDetail.params.PanelID + ' #hfProvider').val();
        if (ProviderId != '' && LabId != '') {


            ClinicalLabResultDetail.favoriteListSearch().done(function () {
                dfd.resolve();
            });

        }
        else {
            //$('#' + ClinicalLabOrderDetail.params.PanelID + ' #txtLabCPTCode').addClass('disableAll');

            var $ddl = $('#' + ClinicalLabResultDetail.params.PanelID + ' #ddlFavLabResult');
            $ddl.empty();
            $ddl.append($('<option/>', {
                value: "",
                html: "- Select -"
            }));
            $ddl.val("");
            $ddl.trigger("onchange");

            dfd.resolve();


            // $('#' + ClinicalLabOrderDetail.params.PanelID + ' #favSectionDiv').addClass('disableAll');
        }

        return dfd.promise();
    },
    loadfavoriteListContent: function (obj) {
        if (typeof obj == typeof undefined || obj == null) {
            obj = $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ddlFavLabResult');
        }
        var SearchData = $('#' + ClinicalLabResultDetail.params.PanelID + ' #FavSearchBox').val();
        var $uL = $('#' + ClinicalLabResultDetail.params.PanelID + ' #ulFavLabResult');
        if (obj != null) {
            var selectedOption = $(obj).find("option:selected");
            var selectedOptionValue = selectedOption.attr("id");
            //Start 12-07-2016 Muhammad Arshad Bug#EMR-1486 Lab order -> Favorite list -> "Select ALL " functionality is not working on 46 server ,please see attached video and resolve the issue
            if (selectedOptionValue > 0) {
                ClinicalLabResultDetail.favoriteList_CPTSearch(selectedOptionValue, null, SearchData);
            }
            else {
                $uL.empty();
            }
        }
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Function to load procedure Order Fav List
    favoriteList_CPTSearch: function (FavoriteListId, isEmptyUl, SearchData) {
        var $uL = $('#' + ClinicalLabOrderDetail.params.PanelID + ' #ulFavLabResult');
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

                            var onclick = "ClinicalLabOrderDetail.BindLabOrderGridItem(\"" + item.CPTCode + "\",\"" + String(item.CPTCodeDescription) + "\",\"" + item.CPTCodeDescription + "\",null);";


                            if (item.LabName != null && item.LabName.length > 0) {
                                $uL.append("<li onclick='" + onclick + "'>" + item.CPTCode + " " + String(item.CPTCodeDescription) + " (<strong>" + item.LabName + "</strong>)" + mod + "</li>");
                            } else {
                                $uL.append("<li onclick='" + onclick + "'>" + item.CPTCode + " " + String(item.CPTCodeDescription) + mod + "</li>");
                            }
                        }
                        else {
                            var onclick = 'ClinicalLabOrderDetail.BindLabOrderGridItem(\'' + item.CPTCode + '\',\'' + item.CPTCodeDescription + '\',\'' + item.CPTCodeDescription + '\',null)';
                            if (item.LabName != null && item.LabName.length > 0) {
                                $uL.append('<li onclick="' + onclick + '">' + item.CPTCode + " " + item.CPTCodeDescription + ' (<strong>' + item.LabName + '</strong>)' + mod + '</li>');
                            } else {
                                $uL.append('<li onclick="' + onclick + '">' + item.CPTCode + " " + item.CPTCodeDescription + mod + '</li>');
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
    showHideEditableControls: function (isHide) {

        var $self = $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail");

        if (!isHide) {

            //Hide Labels
            $self.find('div.Order-label-control').addClass("hidden");
            // Show Editable controls
            $self.find('div.editable-control').removeClass("hidden");


        }
        else {
            //Show Labels
            $self.find('div.Order-label-control').removeClass("hidden");
            // Hide Editable controls
            $self.find('div.editable-control').addClass("hidden");
        }
    },

    enableDisableOrderResultButtons: function (isDisable) {

        if (isDisable == null)
            var isDisable = ($("#" + ClinicalLabResultDetail.params.PanelID + " #dgvLabResultDetail tbody tr[class*=childRow-bg]").length > 0 && $("#" + ClinicalLabResultDetail.params.PanelID + " #dgvLabResultDetail tbody tr").find('.dataTables_empty').length == 0) ? false : true;

        var resultDetailsButtons = $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #btnSaveResult,#btnSendToPortal");
        if (isDisable) {
            resultDetailsButtons.addClass("disableAll");
        }
        else {
            resultDetailsButtons.removeClass("disableAll");
        }
        return isDisable;
    },

    enableDisableOrderResultButton: function (buttonId, hiddenFieldId) {

        var controlBasedId = $('#' + ClinicalLabResultDetail.params.PanelID + " #" + hiddenFieldId).val();
        var isDisable = controlBasedId > "0" ? false : true;

        var $button = $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #" + buttonId);
        if (isDisable) {
            $button.addClass("disableAll");
        }
        else {
            $button.removeClass("disableAll");
        }
    },

    enableDisableOrderResultPrintButton: function () {

        var resultId = $('#' + ClinicalLabResultDetail.params.PanelID + " #hfLabResultId").val();
        var isDisable = resultId > "0" ? false : true;
        var $printButton = $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #btnPrintResult");
        if (isDisable) {
            $printButton.addClass("disableAll");
        }
        else {
            $printButton.removeClass("disableAll");
        }
    },
    //Author: Ahmad Raza
    //Date :  21-06-2016
    //Function Name: validateSpecialCharacters
    //Description: This function will validate the special characters
    validateSpecialCharacters: function (event) {
        var valid = (event.which >= 48 && event.which <= 57) || (event.which >= 65 && event.which <= 90) || (event.which >= 97 && event.which <= 122) || (event.which >= 32 || event.which <= 46);
        if (!valid) {
            event.preventDefault();
        }

    },

    //Author Name: Abid Ali
    //Created Date: 16-04-2016
    //Description: get parent child row Json
    getParentChildJson: function (parentGridRow) {

        var $parentRow = $(parentGridRow);
        var LabOrderResultDetailModel = {};

        var parentRowId = $parentRow.attr('id');
        var cptCode = $parentRow.attr('CPTCode');
        var cptDescription = $parentRow.attr('CPTDescription');

        LabOrderResultDetailModel['LabOrderResultDetailId'] = $parentRow.attr('id');
        LabOrderResultDetailModel['CPTCode'] = cptCode;
        LabOrderResultDetailModel['CPTCodeDescription'] = cptDescription;
        LabOrderResultDetailModel["dtpLabDate"] = $parentRow.find('td input[id^=dtpDate]').val();
        LabOrderResultDetailModel["tpLabTime"] = $parentRow.find('td input[id^=tpTime]').val();


        var dtbParentRow = ClinicalLabResultDetail.EditableGrid.datatable.row(parentGridRow);

        //check for child rows
        if (dtbParentRow.child() != null) {

            if (dtbParentRow.child().length > 0) {

                var ChildRows = [];

                pushChilds = function () {

                    var childRows = dtbParentRow.child();
                    $(childRows).each(function () {

                        var $this = $(this);
                        var chidlId = $this.attr('child-id');
                        if ($this.is('[gchild-id]') == false) {
                            var childJSONData = $this.getMyJSONByName();
                            //Child row object
                            var ChildResultDetailModel = JSON.parse(childJSONData);

                            gchild = ClinicalLabResultDetail.getGrandChildRow($("#" + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #dgvLabResultDetail"), $this);
                            if (gchild.length > 0) {
                                ChildResultDetailModel["ReferenceRangeInterpration"] = $(gchild).find("[name=ReferenceRangeInterpration]").val();
                                ChildResultDetailModel["ReferenceRangeDescription"] = $(gchild).find("[name=ReferenceRangeDescription]").val();
                                ChildResultDetailModel["TestAntimicrobial"] = $(gchild).find("[name=TestAntimicrobial]").val();
                            }

                            ChildResultDetailModel["LabOrderResultDetailId"] = chidlId;
                            ChildResultDetailModel["LOINC"] = $this.attr("LOINICCODE");
                            ChildResultDetailModel["LOINCDescription"] = $this.attr("LOINICDescription");
                            ChildResultDetailModel["ObservationDate"] = ($this.find('td input[id*=Child_dtpDate]').val() + " " + $this.find('td input[id*=Child_tpTime]').val());
                            ChildResultDetailModel['IsAttribute'] = $this.attr('IsAttribute');
                            ChildResultDetailModel['LabTestId'] = $this.attr('LabTestId');
                            ChildResultDetailModel['LabTestAttributeId'] = $this.attr('LabTestAttributeId');


                            ChildRows.push(ChildResultDetailModel);
                        }

                    });
                }
                $.when(pushChilds()).then(function () {

                    LabOrderResultDetailModel['ChildRows'] = ChildRows;
                });
            }
        }
        return LabOrderResultDetailModel;
    },

    getGrandChildRow: function ($self, $row) {
        return $self.find("tr[gparent-id='" + $row.attr("child-id") + "']");
    },
    // Pop to open for Assignee
    //Auto Complete for Assignee txt field
    openAssignee: function () {

        CacheManager.BindCodes('GetUsersFullName', true).done(function (result) {
            var Ctrl = $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #txtAssignee");
            var hfCtrl = $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail  #hfAssigneeId");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", GetUsersFullName, null, hfCtrl);
        });

    },

    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmClinicalLabResultDetail";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ClinicalLabResultDetail";
        LoadActionPan('Admin_Provider', params);
    },

    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmClinicalLabResultDetail";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ClinicalLabResultDetail";
        LoadActionPan('Admin_Facility', params);
    },
    OpenAssignee: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmClinicalLabResultDetail";
        params["AssigneeId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ClinicalLabResultDetail";
        params["RefCtrl"] = "txtAssignee";
        params["RefCtrlHidden"] = "hfAssigneeId";
        params["RefCtrlLink"] = "lnkAssignee";
        LoadActionPan('Admin_Provider', params);
    },
    bindProvider: function () {
        var Ctrl = $('#' + ClinicalLabResultDetail.params.PanelID + " #txtProvider");
        var hfCtrl = $('#' + ClinicalLabResultDetail.params.PanelID + " #hfProvider");
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        var onSelect = function (dataItem) { $('#' + ClinicalLabResultDetail.params.PanelID + " #txtProvider").attr("Provider", dataItem.id) }
        var onChange = function (valid) { ClinicalLabResultDetail.SetFavSearch(); }
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect, onChange);
    },
    bindFacility: function () {
        var Ctrl = $('#' + ClinicalLabResultDetail.params.PanelID + " #txtFacility");
        //EMR-6535 by:MAHMAD
        var hfCtrl = $('#' + ClinicalLabResultDetail.params.PanelID + " #hfFacility");
        //EMR-6535 by:MAHMAD
        var func = function () { return utility.GetFacilityArray(Ctrl.val()) };
        var onSelect = function (dataItem) {
            $('#' + ClinicalLabResultDetail.params.PanelID + " #txtFacility").attr("Facility", dataItem.id);
            $('#' + ClinicalLabResultDetail.params.PanelID + " #txtLocation").val(dataItem.Location);
        }
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect);
    },
    //Author Name: Muhammad Arshad
    //Created Date: 19-04-2016
    //Description: open LOINC search screen
    openLOINCSearch: function (row, $row) {
        var params = [];
        params["GridRow"] = row;
        params["Grid$Row"] = $row;
        params["FromAdmin"] = ClinicalLabResultDetail.params["FromAdmin"];
        params["ParentCtrl"] = 'ClinicalLabResultDetail';

        LoadActionPan('Clinical_LOINC', params);
    },

    //Author Name: Muhammad Arshad
    //Created Date: 25-04-2016
    //Description: open Document Import Screen
    documentImport: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Documents", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];

                var AccountNo = $("#PatientProfile #hfAccountNo").val();
                var PatientFullName = $("#PatientProfile #hfPatientFullName").val();
                var PatientId = Clinical_LabOrder.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $("#PatientProfile #hfPatientId").val();
                var PatientName = "";
                if (PatientFullName.length > 0) {
                    var Firstname = PatientFullName.split(" ")[1];
                    var Lastname = PatientFullName.split(" ")[0];
                    var MiddleInitial = PatientFullName.split(" ")[2];
                    PatientName = Lastname + " " + Firstname + " " + MiddleInitial;
                }
                params["patientId"] = Clinical_LabOrder.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
                params["RefCtrl"] = "LabResult";
                params['LabResultId'] = ClinicalLabResultDetail.params.LabResultId;
                params['RefModuleName'] = "Lab Results";
                params['TransitionId'] = ClinicalLabResultDetail.params.LabResultId;
                params["FromAdmin"] = "0";
                params["mode"] = "Add";


                params["ParentCtrl"] = 'ClinicalLabResultDetail';
                params["PatientName"] = PatientName;//ClinicalLabResultDetail.params.PatientName;
                LoadActionPan('Document_Import', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });


    },

    //Author Name: Muhammad Arshad
    //Created Date: 25-04-2016
    //Description: open Document Scan Screen
    documentScan: function () {
        AppPrivileges.GetFormPrivileges("Documents", "SCAN", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var param = [];
                param["mode"] = "Scan";
                param["RefCtrl"] = "LabResult";
                param['LabResultId'] = ClinicalLabResultDetail.params.LabResultId;
                param['RefModuleName'] = "Lab Results";
                param['TransitionId'] = ClinicalLabResultDetail.params.LabResultId;
                param['patientID'] = Clinical_LabOrder.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
                param["ParentCtrl"] = 'ClinicalLabResultDetail';
                LoadActionPan('Document_Scan', param);
            }
            else
                utility.DisplayMessages(strMessage, 2);

        });
    },

    //Author Name: Abid Ali
    //Created Date: 16-04-2016
    //Description: Binds LabResults with UI controls.
    fillLabResult: function (labResultId, labOrderId) {

        ClinicalLabResultDetail.loadLabResults_DbCall(labResultId, labOrderId).done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {

                var $self = $('#' + ClinicalLabResultDetail.params.PanelID + ' #frmClinicalLabResultDetail');

                //sectionInfo details
                var sectionInfoDetails = JSON.parse(response.LabResultOrderInfoFill_JSON);
                //Array Of LabOrderTests
                var labResultTests = JSON.parse(response.LabResultOrderTestFill_JSON);
                var tests = JSON.parse(response.LabResultTestsFill_JSON);

                $self.find('#hfCodeSystemTypeId').val(sectionInfoDetails.CodeSystemId);
                ClinicalLabResultDetail.isEnterococcus = true;
                $.each(tests, function (i, obj) {

                    if (obj.Organism != "Enterococcus") {
                        ClinicalLabResultDetail.isEnterococcus = false;

                    }

                    if (obj.AntimicrobialIds) {
                        $.each(obj.AntimicrobialIds.split(","), function (i, AntimicrobialId) {
                            //Antimicrobials ali awan
                            // var temp = {};
                            // temp["AntimicrobialId"] = AntimicrobialId;
                            // temp["Antimicrobial"] = obj.Antimicrobials.split(",")[i];
                            //check if exists then push

                            if ($self.find("#ddlTestAntimicrobial option[value='" + AntimicrobialId + "']").length <= 0) {
                                $self.find("#ddlTestAntimicrobial").append("<option value='" + AntimicrobialId + "'>" + obj.Antimicrobials.split(",")[i] + "</option>")
                            }
                        });

                    }

                });


                //Binds sectionInfo Details

                utility.bindMyJSONByName(true, sectionInfoDetails, false, $self).done(function () {

                    $self.find('#hfOrderNo').val(sectionInfoDetails.OrderNo);
                    if (sectionInfoDetails.Status == "") {
                        $self.find('#ddlStatus').val("Final");
                    }

                    if (sectionInfoDetails && sectionInfoDetails.FinalInterpretation) {
                        $.each(sectionInfoDetails.FinalInterpretation.split(","), function (i, item) {

                            ClinicalLabResultDetail.AddNewFinalInterpretation(item)
                        });
                    }
                    $('#' + ClinicalLabResultDetail.params.PanelID + ' #hfLabId').val(sectionInfoDetails.LaboratoryId);
                    var ctrl_pr = $('#' + ClinicalLabResultDetail.params.PanelID + " #txtProvider");
                    var hfCtrl_pr = $('#' + ClinicalLabResultDetail.params.PanelID + " #hfProvider");
                    utility.SetKendoAutoCompleteSourceforValidate(ctrl_pr, ctrl_pr.val(), hfCtrl_pr, hfCtrl_pr.val());
                    var ctrl_fc = $('#' + ClinicalLabResultDetail.params.PanelID + " #txtFacility");
                    var hfCtrl_fc = $('#' + ClinicalLabResultDetail.params.PanelID + " #hfFacility");
                    utility.SetKendoAutoCompleteSourceforValidate(ctrl_fc, ctrl_fc.val(), hfCtrl_fc, hfCtrl_fc.val());
                    //$self.find("#txtComment").text(sectionInfoDetails.Comments);


                    let comm = '<div>' + sectionInfoDetails.Comments + '</div>';
                    let obj = $(comm);
                    $self.find("#txtComment").html(obj);

                });



                $('#' + ClinicalLabResultDetail.params.PanelID + " #hfLabOrderResultId").val(sectionInfoDetails.LabOrderResultId);
                ClinicalRadiologyResultDetail.params.RadiologyResultId = sectionInfoDetails.LabOrderResultId;
                //Start 30-05-2016 Humaira Yousaf to enable/disable view pdf link
                if (response.ResultCount <= 0) {
                    if (response.RejectionReasonCount <= 0) {
                        $('#' + ClinicalLabResultDetail.params.PanelID + " #btnViewPDF").addClass("disableAll");
                    }
                    else {
                        $('#' + ClinicalLabResultDetail.params.PanelID + " #btnViewPDF").removeClass("disableAll");
                    }
                }
                else {
                    $('#' + ClinicalLabResultDetail.params.PanelID + " #btnViewPDF").removeClass("disableAll");
                }

                if (ClinicalLabResultDetail.params.FromLabDetail) {

                    var LOINCCOde = "";
                    var LOINCDescription = "";
                    var LaboratoryId = sectionInfoDetails.LaboratoryId;
                    var PanelLabResultGrid = "#" + ClinicalLabResultDetail.params.PanelID + " #pnlLab_ResultDetail";
                    var LabResultGridId = "#" + ClinicalLabResultDetail.params.PanelID + " #dgvLabResultDetail";

                    if ($.fn.dataTable.isDataTable(LabResultGridId)) {
                        $(LabResultGridId).dataTable().fnClearTable();
                        $(LabResultGridId).dataTable().fnDestroy();
                    }
                    ClinicalLabResultDetail.EditableGrid = EMRUtility.MakeEditableGrid(PanelLabResultGrid, LabResultGridId, ClinicalLabResultDetail, 0, false, true, false, false, false, null);
                    $(LabResultGridId + " tbody tr").remove();

                    for (var i = 0; i < tests.length; i++) {

                        LOINCCOde = tests[i].CPTCode;
                        LOINCDescription = tests[i].CPTCodeDescription;

                        ClinicalLabResultDetail.loadLabTest_DBCall(LaboratoryId, LOINCCOde, LOINCDescription).done(function (response) {

                            var data = JSON.parse(response);
                            if (data.status != false) {
                                var LabTestJSON = JSON.parse(data.ClinicalLabTest_JSON);
                                if (LabTestJSON.length > 0) {
                                    //
                                    if (LabTestJSON[0].IsActive != "False") {
                                        if (LabTestJSON[0].IsTemplate == "False") {
                                            ClinicalLabResultDetail.BindLabOrderResultGridItem(null, tests[i], LOINCCOde, LOINCDescription, LOINCDescription, true, data);
                                            ClinicalLabResultDetail.enableDisableOrderResultButtons();
                                            ClinicalLabResultDetail.isAttribute = true;
                                        } else {
                                            ClinicalLabResultDetail.BindLabOrderResultGridItem(null, tests[i], LOINCCOde, LOINCDescription, LOINCDescription, true, data);
                                            ClinicalLabResultDetail.enableDisableOrderResultButtons();
                                            ClinicalLabResultDetail.isAttribute = false;
                                        }
                                    } else {
                                        ClinicalLabResultDetail.BindLabOrderResultGridItem(null, tests[i], LOINCCOde, LOINCDescription, LOINCDescription);
                                        ClinicalLabResultDetail.enableDisableOrderResultButtons();
                                    }
                                    //
                                } else {
                                    ClinicalLabResultDetail.BindLabOrderResultGridItem(null, tests[i], LOINCCOde, LOINCDescription, LOINCDescription);
                                    ClinicalLabResultDetail.enableDisableOrderResultButtons();
                                }

                            } else {
                                ClinicalLabResultDetail.BindLabOrderResultGridItem(null, tests[i], LOINCCOde, LOINCDescription, LOINCDescription);
                                ClinicalLabResultDetail.enableDisableOrderResultButtons();
                            }
                        });
                    }
                    // $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #lblOrganisms").text(ClinicalLabResultDetail.Organisms.join(", "))
                } else {
                    //End 30-05-2016 Humaira Yousaf to enable/disable view pdf link
                    //Load the grid and show/hide buttons
                    $.when(ClinicalLabResultDetail.labResultGridLoad(response, labResultTests, tests)).then(
                            function () {

                                var isDisableCtrls = (sectionInfoDetails.IsAknowledged == "True");
                                ClinicalLabResultDetail.disableLabResultControls(isDisableCtrls);
                                // ClinicalLabResultDetail.enableDisableOrderResultButton('btnPrintResult', 'hfLabOrderResultId');
                                ClinicalLabResultDetail.enableDisableOrderResultButtons((isDisableCtrls == true) ? isDisableCtrls : null);
                                $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #btnPrintResult").removeClass('disableAll');
                                //   $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #lblOrganisms").text(ClinicalLabResultDetail.Organisms.join(", "))
                            });
                }

                ClinicalLabResultDetail.SetFavSearch();
            }
            else {
                ClinicalLabResultDetail.enableDisableOrderResultButtons();
                utility.DisplayMessages(response.Message, 3);
            }
            $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail").data('serialize', $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail").serialize());
        });

    },

    //Author Name: Abid Ali
    //Created Date: 19-04-2016
    //Description: Loads lab results grid
    labResultGridLoad: function (response, oltests, tests) {

        //Start/ Grid Panel and ID
        var PanelLabResultGrid = "#" + ClinicalLabResultDetail.params.PanelID + " #pnlLab_ResultDetail";
        var LabResultGridId = "#" + ClinicalLabResultDetail.params.PanelID + " #dgvLabResultDetail";
        $(LabResultGridId + " tbody").find("tr").remove();

        if ($.fn.dataTable.isDataTable(LabResultGridId)) {
            $(LabResultGridId).dataTable().fnClearTable();
            $(LabResultGridId).dataTable().fnDestroy();
        }
        //End/ Grid Panel and ID
        var parentRowLabResultTests = JSON.parse(response.LabResultOrderTestFill_JSON);
        if (parentRowLabResultTests.LabOrderResultDetailModels.length > 0) {
            //Draw the Grid
            ClinicalLabResultDetail.drawChildGrid(parentRowLabResultTests, tests);

        }
        else {
            $("#" + ClinicalLabResultDetail.params.PanelID + " #pnlLab_ResultDetail #divLabResultPaging").css("display", "none");
            $("#" + ClinicalLabResultDetail.params.PanelID + ' #dgvLabResultDetail').DataTable({
                "language": {
                    "emptyTable": "No Lab orders Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bDestroy": true, "bSort": false
            });
        }

    },

    drawChildGrid: function (parentRowLabResultTests, tests) {
        var labResults = [];
        buildTable = function (labTests, parrent) {

            $.each(parrent, function (i, item) {

                var labResultTestId = item.LabOrderTestId;

                var currentRow = ClinicalLabResultDetail.addLabResultRow(item.Modifier, item);

                //Build Child Rows (Agruments (current Row/ Child Items Json)
                //  var childRows = null;
                //   $.each(parentRowLabResultTests.LabOrderResultDetailModels, function (i, items) {
                var childRows = ClinicalLabResultDetail.buildRowChild(currentRow, parentRowLabResultTests.LabOrderResultDetailModels[i]);
                //  });

                labResults.push({ row: currentRow, childs: childRows });

                //Push parent child rows
                //  rowsParentChild.push({ row: currentRow, childs: childRows });

            });
        }

        //Draw Table
        drawTable = function () {

            var PanelLabResultGrid = "#" + ClinicalLabResultDetail.params.PanelID + " #pnlLab_ResultDetail";
            var LabResultGridId = "#" + ClinicalLabResultDetail.params.PanelID + " #dgvLabResultDetail";

            ClinicalLabResultDetail.EditableGrid = EMRUtility.MakeEditableGrid(PanelLabResultGrid, LabResultGridId, ClinicalLabResultDetail, 0, false, true, false, false, false, null);
            ClinicalLabResultDetail.EditableGrid.datatable.draw();

            //push parent/childs rows in the datatable
            $.each(labResults, function (i, item) {

                if (ClinicalLabResultDetail.EditableGrid != null) {

                    var row = ClinicalLabResultDetail.EditableGrid.datatable.row(item.row);
                    if (item.childs.length > 0) {

                        row.child(item.childs);
                        $(item.row).find('td:first').find('a').show();
                        $(item.row).find("td:first .fa-plus-square").attr("class", "fa fa-minus-square");
                        // Open this row
                        row.child.show();
                    }
                    else {
                        $(item.row).find('td:first').find('a').hide();
                    }
                }
            });
            ClinicalLabResultDetail.SetDateTimeControl($("#" + ClinicalLabResultDetail.params.PanelID + " #dgvLabResultDetail tbody"), true);
            $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body', html: true });
            //$("#" + ClinicalLabResultDetail.params.PanelID).find('.tooltip-inner').css("max-width", "500px !important");

            // $("#" + ClinicalLabResultDetail.params.PanelID + " #dgvLabResultDetail").dataTable().fnPageChange('first');
        }
        //builds and draw the Grid
        $.when(buildTable(parentRowLabResultTests.LabOrderResultDetailModels, tests)).then(drawTable());
    },

    selectProvider: function (providerId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Orders and Results_Lab Result", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
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
                                    $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalLabResultDetail #txtProvider").val(decodeHtmlEntity(item.LastName + ', ' + item.FirstName + ' ' + item.MiddleInitial));
                                    $('#' + ClinicalProcedureOrderDetail.params.PanelID + " #frmClinicalLabResultDetail #hfProvider").val(decodeHtmlEntity(item.ProviderId));
                                }
                            });
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

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Function to disable Lab Order Controls
    disableLabResultControls: function (IsSigned) {
        var detailsDivs = $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #divResultInformation,#comments-remarks,#divLabResultInformation");
        var detailsButtons = $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #btnSaveResult,#btnSignPrintOrder,#btnResetLabResult");
        var printRequisitionButton = $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #btnPrintResult");
        if (IsSigned == true) {
            detailsDivs.addClass("disableAll");
            detailsButtons.addClass("disableAll");
            printRequisitionButton.removeClass("disableAll");
        }
        else {
            detailsDivs.removeClass("disableAll");
            detailsButtons.removeClass("disableAll");
            printRequisitionButton.addClass("disableAll");
        }
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Function to load Lab Order Fav List
    favoriteListSearch: function () {
        var dfd = new $.Deferred();
        ClinicalLabResultDetail.searchFavoriteList_DBCall("LabOrder", null, 1, 5000).done(function (response) {
            response = utility.decodeHtml(response);
            response = JSON.parse(response);
            var $ddl = $('#' + ClinicalLabResultDetail.params.PanelID + ' #ddlFavLabResult');
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
                    EMRUtility.getFavListValue(ClinicalLabResultDetail.FavListName).done(function (response1) {
                        response1 = JSON.parse(response1);
                        if (response1.status != false) {
                            if (response1.favListVal != "" && response1.favListVal != "-1") {
                                if ($("#" + ClinicalLabResultDetail.params.PanelID + " #ddlFavLabResult option[value='" + response1.favListVal + "']").length > 0) {
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
                //    $ddl.trigger("onchange");
                dfd.resolve();
            }
            else {
                dfd.resolve();
            }

        });
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
        if (ClinicalLabResultDetail.params.mode.toLowerCase() == "add") {
            objData["LabId"] = $('#' + ClinicalLabResultDetail.params.PanelID + ' #ddlLabId option:selected').val();
        }
        else {
            objData["LabId"] = $('#' + ClinicalLabResultDetail.params.PanelID + ' #hfLabId').val();
        }
        objData["ProviderId"] = $('#' + ClinicalLabResultDetail.params.PanelID + ' #hfProvider').val();
        objData["IsActive"] = true
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

        $('#' + ClinicalLabResultDetail.params.PanelID + ' #ulFavoriteListLabResultContent li').each(function (i, item) {
            $(item).trigger("click");
        });


    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Function to load procedure Order Fav List Test while selection is done
    loadfavoriteListContent: function (obj) {
        if (typeof obj == typeof undefined || obj == null) {
            obj = $('#' + ClinicalLabResultDetail.params.PanelID + ' #ddlFavLabResult');
        }
        if (obj != null) {
            var selectedOption = $(obj).find("option:selected");
            var SearchData = $('#' + ClinicalLabResultDetail.params.PanelID + ' #FavSearchBox').val();
            ClinicalLabResultDetail.favoriteList_CPTSearch(selectedOption.attr("id"), SearchData);
        }
    },
    AutoSearchFavLabresultDetail: function (obj) {
        utility.Keyupdelay(function () {
            ClinicalLabResultDetail.loadfavoriteListContent();
        });
    },
    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Function to load procedure Order Fav List
    favoriteList_CPTSearch: function (FavoriteListId, SearchData) {
        Favorite_ProcedureOrder.searchFavoriteList_CPT_DBCall(null, FavoriteListId, 1, 5000, SearchData).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var $UL = $('#' + ClinicalLabResultDetail.params.PanelID + ' #ulFavLabResult');
                var objData = JSON.parse(response.FavoriteListCPTJSON);
                $UL.empty();

                $.each(objData, function (i, item) {
                    var mod = item.Modifier != "" ? "<strong> - Mod : " + item.Modifier + "</strong>" : "";
                    if (item.CPTCodeDescription != "") {
                        var onclick = 'ClinicalLabResultDetail.bindLabTestResultAttributes(\'' + item.Modifier + '\',\'' + item.CPTCode + '\',\'' + String(item.CPTCodeDescription) + '\')';
                        $UL.append('<li onclick="' + onclick + '">' + item.CPTCode + " " + item.CPTCodeDescription + mod + '</li>');
                    }
                });
            }
            $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail").data('serialize', $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail").serialize());
            //else {
            //    utility.DisplayMessages(response.Message, 3);
            //}
        });
    },


    //Author: Abid Ali
    //Date :  17-03-2016
    //Reason: Bind Guaranter
    bindGuarantor: function () {
        var shortName = $('#pnlClinicalLabResultDetail #txtGuarantor').val();
        utility.GetGuarontorArray(shortName).done(function (response) {

            $('#pnlClinicalLabResultDetail #txtGuarantor').autocomplete({
                //source: AllPatients, // pass an array (without a comma)
                autoFocus: true,
                source: response,
                select: function (event, ui) {

                    setTimeout(function () {

                        $("#pnlClinicalLabResultDetail #hfLabGuarantorId").val(ui.item.id); // add the selected id
                        if ($("#pnlClinicalLabResultDetail #lnkGuarantorEdit").css("display") == "none") {
                            $("#pnlClinicalLabResultDetail #lnkGuarantorEdit").css("display", "inline");
                            $("#pnlClinicalLabResultDetail #lblGuarantor").css("display", "none");
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
        params["ParentCtrl"] = "ClinicalLabResultDetail";
        params["RefCtrl"] = "txtGuarantor";
        LoadActionPan('Patient_Guarantor', params);
    },


    //Author: Abid Ali
    //Date :  17-03-2016
    //Reason: open Guaranter
    openGuarantorDetail: function () {
        //Patient_Guarantor.GuarantorEdit($('#pnlDemographic #hfGuarantor').val(), 'patTabDemographic');
        var params = [];
        params["GuarantorId"] = $('#pnlClinicalLabResultDetail #hfLabGuarantorId').val();
        params["mode"] = "Edit";
        params["RefCtrl"] = "txtGuarantor";
        params["ParentCtrl"] = 'ClinicalLabResultDetail';
        LoadActionPan('guarantorDetail', params);
    },

    //Author: Abid Ali
    //Date :  17-03-2016
    //Reason: Load auto complete for provider and Facility
    loadAllAutocomplete: function () {
        if (ClinicalLabResultDetail.loadProviderFromAppData && ClinicalLabResultDetail.ProviderOnDemandLoad) {
            if (Clinical_LabOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
                CacheManager.BindCodes('GetProvider', false).done(function (result) {
                    var Ctrl = $('#' + ClinicalLabResultDetail.params.PanelID + " #txtProvider");
                    var hfCtrl = $('#' + ClinicalLabResultDetail.params.PanelID + " #hfProvider");
                    var onSelect = function (dataItem) { $('#' + ClinicalLabResultDetail.params.PanelID + " #txtProvider").attr("Provider", dataItem.id); }
                    utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, onSelect);

                    $('#' + ClinicalLabResultDetail.params.PanelID + " #txtProvider").val(Clinical_ProgressNote.params["CurrentNotesProviderText"]);
                    $('#' + ClinicalLabResultDetail.params.PanelID + " #txtProvider").attr("Provider", Clinical_ProgressNote.params["CurrentNotesProviderId"]);
                    $('#' + ClinicalLabResultDetail.params.PanelID + " #hfProvider").val(Clinical_ProgressNote.params["CurrentNotesProviderId"]);
                    utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalLabResultDetail.params.PanelID + " #txtProvider"), Clinical_ProgressNote.params.CurrentNotesProviderText, $('#' + ClinicalLabResultDetail.params.PanelID + " #hfProvider"), Clinical_ProgressNote.params.CurrentNotesProviderId);
                });
                ClinicalLabResultDetail.loadProviderFromAppData = true;
                ClinicalLabResultDetail.ProviderOnDemandLoad = false;

            }
            else {
                if (globalAppdata.DefaultProviderName != "" && globalAppdata.DefaultProviderName != "- Select -") {
                    CacheManager.BindCodes('GetProvider', false).done(function (result) {
                        var Ctrl = $('#' + ClinicalLabResultDetail.params.PanelID + " #txtProvider");
                        var hfCtrl = $('#' + ClinicalLabResultDetail.params.PanelID + " #hfProvider");
                        var onSelect = function (dataItem) { $('#' + ClinicalLabResultDetail.params.PanelID + " #txtProvider").attr("Provider", dataItem.id); }
                        utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, onSelect);

                        $('#' + ClinicalLabResultDetail.params.PanelID + " #txtProvider").val(globalAppdata.DefaultProviderName);
                        $('#' + ClinicalLabResultDetail.params.PanelID + " #txtProvider").attr("Provider", globalAppdata.DefaultProviderId);
                        $('#' + ClinicalLabResultDetail.params.PanelID + " #hfProvider").val(globalAppdata.DefaultProviderId);
                        utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalLabResultDetail.params.PanelID + " #txtProvider"), globalAppdata.DefaultProviderName, $('#' + ClinicalLabResultDetail.params.PanelID + " #hfProvider"), globalAppdata.DefaultProviderId);
                    });
                    ClinicalLabResultDetail.loadProviderFromAppData = true;
                    ClinicalLabResultDetail.ProviderOnDemandLoad = false;
                    ClinicalLabResultDetail.isProviderLoaded = true;

                } else {
                    CacheManager.BindCodes('GetProvider', false).done(function (result) {
                        var Ctrl = $('#' + ClinicalLabResultDetail.params.PanelID + " #txtProvider");
                        var hfCtrl = $('#' + ClinicalLabResultDetail.params.PanelID + " #hfProvider");
                        var onSelect = function (dataItem) { $('#' + ClinicalLabResultDetail.params.PanelID + " #txtProvider").attr("Provider", dataItem.id); }
                        utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, onSelect);

                        if ($('#' + ClinicalLabResultDetail.params.PanelID + " #txtProvider").val() == "") {
                            $('#' + ClinicalLabResultDetail.params.PanelID + " #txtProvider").val("");
                            $('#' + ClinicalLabResultDetail.params.PanelID + " #txtProvider").attr("Provider", "");
                            $('#' + ClinicalLabResultDetail.params.PanelID + " #hfProvider").val("");
                        }
                    });
                    ClinicalLabResultDetail.loadProviderFromAppData = true;
                    ClinicalLabResultDetail.ProviderOnDemandLoad = false;
                    ClinicalLabResultDetail.isProviderLoaded = true;
                }
            }
        }

        if (ClinicalLabResultDetail.loadFacilityFromAppData && ClinicalLabResultDetail.FacilityOnDemandLoad) {
            if (Clinical_LabOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
                CacheManager.BindCodes('GetFacility', false).done(function (result) {
                    var Ctrl = $('#' + ClinicalLabResultDetail.params.PanelID + " #txtFacility");
                    var hfCtrl = $('#' + ClinicalLabResultDetail.params.PanelID + " #hfFacility");
                    var onSelect = function (dataItem) { $('#' + ClinicalLabResultDetail.params.PanelID + " #txtFacility").attr("Facility", dataItem.id); }
                    utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl, onSelect);

                    $('#' + ClinicalLabResultDetail.params.PanelID + " #txtFacility").val(Clinical_ProgressNote.params["NotesFacilityName"]);
                    $('#' + ClinicalLabResultDetail.params.PanelID + " #txtFacility").attr("Facility", Clinical_ProgressNote.params["NotesFacilityIDForFollowUp"]);
                    $('#' + ClinicalLabResultDetail.params.PanelID + " #hfFacility").val(Clinical_ProgressNote.params["NotesFacilityIDForFollowUp"]);
                    utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalLabResultDetail.params.PanelID + " #txtFacility"), Clinical_ProgressNote.params.NotesFacilityName, $('#' + ClinicalLabResultDetail.params.PanelID + " #hfFacility"), Clinical_ProgressNote.params.NotesFacilityIDForFollowUp);
                });
                ClinicalLabResultDetail.loadFacilityFromAppData = true;
                ClinicalLabResultDetail.FacilityOnDemandLoad = false;
                ClinicalLabResultDetail.isFacilityLoaded = true;

            }
            else {

                if (globalAppdata.DefaultFacilityName != "" && globalAppdata.DefaultFacilityName != "- Select -") {
                    CacheManager.BindCodes('GetFacility', false).done(function (result) {
                        var Ctrl = $('#' + ClinicalLabResultDetail.params.PanelID + " #txtFacility");
                        var hfCtrl = $('#' + ClinicalLabResultDetail.params.PanelID + " #hfFacility");
                        var onSelect = function (dataItem) { $('#' + ClinicalLabResultDetail.params.PanelID + " #txtFacility").attr("Facility", dataItem.id); }
                        utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl, onSelect);

                        $('#' + ClinicalLabResultDetail.params.PanelID + " #txtFacility").val(globalAppdata.DefaultFacilityName);
                        $('#' + ClinicalLabResultDetail.params.PanelID + " #txtFacility").attr("Facility", globalAppdata.DefaultFacilityId);
                        $('#' + ClinicalLabResultDetail.params.PanelID + " #hfFacility").val(globalAppdata.DefaultFacilityId);
                        utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalLabResultDetail.params.PanelID + " #txtFacility"), globalAppdata.DefaultFacilityName, $('#' + ClinicalLabResultDetail.params.PanelID + " #hfFacility"), globalAppdata.DefaultFacilityId);
                    });
                    ClinicalLabResultDetail.loadFacilityFromAppData = true;
                    ClinicalLabResultDetail.FacilityOnDemandLoad = false;
                    ClinicalLabResultDetail.isFacilityLoaded = true;
                } else {
                    CacheManager.BindCodes('GetFacility', false).done(function (result) {
                        var Ctrl = $('#' + ClinicalLabResultDetail.params.PanelID + " #txtFacility");
                        var hfCtrl = $('#' + ClinicalLabResultDetail.params.PanelID + " #hfFacility");
                        var onSelect = function (dataItem) { $('#' + ClinicalLabResultDetail.params.PanelID + " #txtFacility").attr("Facility", dataItem.id); }
                        utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl, onSelect);

                        if ($('#' + ClinicalLabResultDetail.params.PanelID + " #txtFacility").val() == "") {
                            $('#' + ClinicalLabResultDetail.params.PanelID + " #txtFacility").val("");
                            $('#' + ClinicalLabResultDetail.params.PanelID + " #txtFacility").attr("Facility", "");
                            $('#' + ClinicalLabResultDetail.params.PanelID + " #hfFacility").val("");
                        }
                    });
                    ClinicalLabResultDetail.loadFacilityFromAppData = true;
                    ClinicalLabResultDetail.FacilityOnDemandLoad = false;
                    ClinicalLabResultDetail.isFacilityLoaded = true;
                }
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
            ClinicalLabResultDetail.enableDisableDropdownList(ddlIds, true);
        else {
            ddlIds.push('ddlPrimaryInsuraceId');
            ddlIds.push('ddlSecondaryInsuraceId');
            ddlIds.push('ddlTertiaryInsuraceId');
            ClinicalLabResultDetail.enableDisableDropdownList(ddlIds, false);
        }
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Enable disable dropdownlists
    enableDisableDropdownList: function (listOfDdlIds, isHide) {

        $.each(listOfDdlIds, function (index, item) {
            if (isHide) {
                $('#' + ClinicalLabResultDetail.params.PanelID + ' #' + item).prop('disabled', true);
            }
            else {
                $('#' + ClinicalLabResultDetail.params.PanelID + ' #' + item).removeClass('disabled', false);
            }
        });
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Populate patient insurances in ddl
    populateInsuranceDropDownList: function (ddlTypeId, options) {
        var $ddl = $('#' + ClinicalLabResultDetail.params.PanelID + ' #' + ddlTypeId);

        $ddl.empty();
        $ddl.append($('<option/>', {
            value: "",
            html: "- SELECT -"
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
    loadProblemList: function () {
        ClinicalLabResultDetail.SearchProblemList().done(function (response) {
            var obj = JSON.parse(response);
            if (obj.status != false) {
                if (obj.ProblemListCount > 0) {
                    var ProblemListLoadJSONData = JSON.parse(obj.ProblemListLoad_JSON);
                    var ProblemListHistoryLoadJSONData = JSON.parse(obj.ProblemListHistoryLoad_JSON);
                    var finalTr = '';
                    var counter = 2;
                    $("#" + ClinicalLabResultDetail.params.PanelID + " #ulProblemLists tbody tr").remove();
                    $.each(ProblemListLoadJSONData, function (i, item) {
                        if (counter % 2 == 0) {
                            finalTr = finalTr + '<tr>';
                        }
                        finalTr = finalTr + '<td><div class="p-xs pl-none size85 pull-left"><div class="checkbox-custom">';

                        finalTr = finalTr + '<input type="checkbox" name="' + item.Code + '" value="' + item.ProblemListId + '"  id="chk' + item.ProblemListId + '">';
                        finalTr = finalTr + '   <label class="control-label"></label></div></div> <div class="pull-left size-min200 pt-xs">' + item.Description + '</div></td>';

                        if (counter % 2 == 1) {
                            finalTr = finalTr + '</tr>';
                        }
                        counter++;
                        var color = "";
                    });
                    $("#" + ClinicalLabResultDetail.params.PanelID + " #ulProblemLists tbody").append(finalTr);
                }

                var LabResultId = ClinicalLabResultDetail.params.LabResultId;
                LabResultId = typeof LabResultId != 'undefined' && LabResultId > 0 ? LabResultId : -1
                ClinicalLabResultDetail.loadLabResult(LabResultId);
                $('#' + ClinicalLabResultDetail.params.PanelID + " #hfLabResultId").val(LabResultId);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
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
        return MDVisionService.defaultService(data, "PATIENT_INSURANCE", "SEARCH_PATIENT_INSURANCE");
    },

    loadPatientGuarantor: function (patientID) {
        if (patientID == null) {
            patientID = Clinical_LabOrder.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        }
        var objData = new Object();
        objData["PatientId"] = patientID;

        objData["commandType"] = "SEARCH_Guarantor";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Edit LabResult
    LabEdit: function (LabResultId, event) {
        //if icon is clicked then  popup the window

        Clinical_LabOrder.LabResultAddEdit(LabResultId);
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
        objData["PatientId"] = Clinical_LabOrder.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
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
        params["RefForm"] = "frmClinicalLabResultDetail";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ClinicalLabResultDetail";
        LoadActionPan('Admin_Provider', params);
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: open provider detail
    OpenProviderDetail: function () {
        //Admin_Provider.ProviderEdit($('#pnlClinicalNotes #hfProvider').val(),'clinicalTabNotes');
        var params = [];
        params["ProviderId"] = $('#pnlClinicalLabResultDetail #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'ClinicalLabResultDetail';
        LoadActionPan('providerDetail', params);
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: open facility form
    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmClinicalLabResultDetail";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ClinicalLabResultDetail";
        LoadActionPan('Admin_Facility', params);
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: open facility detail form
    OpenFacilityDetail: function () {
        //Admin_Facility.FacilityEdit($('#' + demographicDetail.params.PanelID + ' #hfFacility').val(), "demographicDetail");
        var params = [];
        params["FacilityId"] = $('#pnlClinicalLabResultDetail #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacility";
        params["ParentCtrl"] = 'ClinicalLabResultDetail';
        LoadActionPan('facilityDetail', params);
    },

    HideProviderLink: function () {
        $('#pnlClinicalLabResultDetail #txtProvider').attr("ProviderId", "-1");
        $('#pnlClinicalLabResultDetail #hfProvider').val("-1");
        $("#pnlClinicalLabResultDetail #lnkProviderEdit").css("display", "none");
        $("#pnlClinicalLabResultDetail #lblProvider").css("display", "inline");
    },

    // -------------- End Provider -----------------

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Function to apply bootstrap validations
    validateLabResultDetail: function (isEditableControls) {
        var $self = $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail");
        var fields = {};
        // Required for both cases
        fields["Status"] = {
            group: '.col-sm-3',
            validators: {
                notEmpty: {
                    message: ''
                }
            }
        };
        fields["Facility"] = {
            group: '.col-sm-4',
            validators: {
                notEmpty: {
                    message: ''
                }
            }
        };
        if (isEditableControls) {

            fields["LabId"] = {
                group: '.col-sm-3',
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            }; fields["Provider"] = {
                group: '.col-md-3',
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            }
        }
        $self
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: fields,

           })
        .on('success.form.bv', function (e) {
            if (e.type == "success") {
                setTimeout(function () {
                    e.preventDefault();
                    var $form = $(e.target);
                    var $button = $form.data('bootstrapValidator').getSubmitButton();
                    switch ($button.attr('id')) {

                        case 'btnSendToPortal':
                            if (!ClinicalLabResultDetail.isError) {
                                ClinicalLabResultDetail.LabResultSave('SendToPortal');
                            }
                            break;
                        case 'btnPrintResult':
                            ClinicalLabResultDetail.printLabResult();
                            ClinicalLabResultDetail.disableLabResultControls(true);
                            break;
                        case 'btnAcknowledge':
                            if (!ClinicalLabResultDetail.isError) {
                                utility.myConfirm('36', function () {
                                    ClinicalLabResultDetail.LabResultSave('Acknowledge');
                                }, function () {

                                },
                                    '36');
                            }
                            break;
                        default:
                            if (!ClinicalLabResultDetail.isError) {
                                ClinicalLabResultDetail.LabResultSave('save');
                            }
                            else {

                            }
                            break;
                    }
                }, 200);

            }
            e.type = "";
        });

    },

    bindAutoComplete: function (element) {
        var CodeSystemType = null;
        var labId = null;
        if (ClinicalLabResultDetail.params.mode.toLowerCase() == "add") {
            labId = $('#pnlClinicalLabResultDetail #ddlLabId').val();
            CodeSystemType = $('#pnlClinicalLabResultDetail #ddlLabId option:selected').attr('CodeSystem');
        }
        else {
            labId = $('#pnlClinicalLabResultDetail #hfLabId').val();
            CodeSystemType = $('#pnlClinicalLabResultDetail #hfCodeSystemTypeId').val();
        }
        EMRUtility.BindLOINCCodes(element, "ClinicalLabResultDetail", labId, '', CodeSystemType);
    },
    bindAutoCompleteOrganisms: function (element) {

        EMRUtility.BindOrganismCodes(element, "ClinicalLabResultDetail");
    },

    openCPTCode: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ClinicalLabResultDetail";
        //  params["RefHiddenCtrl"] = "hfCPTCode";
        //   params["RefCtrl"] = "txtCPTCode";
        params["ParentCtrlPanelID"] = ClinicalLabResultDetail.params.PanelID;
        //  LoadActionPan('Admin_IMOCPT', params, ClinicalLabResultDetail.params.PanelID);

        //  var params = [];
        //params["FromAdmin"] = "0";
        //params["ParentCtrl"] = "'";
        //params["RefHiddenCtrl"] = "hfCPTCode";
        //params["RefCtrl"] = "txtCPTCode";
        //   params["ParentCtrlPanelID"] = ClinicalLabOrderDetail.params.PanelID;
        //LoadActionPan('Admin_IMOCPT', params, ClinicalLabOrderDetail.params.PanelID);
        //   params["FromAdmin"] = ClinicalLabOrderDetail.params["FromAdmin"];
        //  params["ParentCtrl"] = 'ClinicalLabOrderDetail';
        LoadActionPan('Clinical_LOINC', params, ClinicalLabResultDetail.params.PanelID);
    },

    //Called from LOINIC Control to pass code and description as Json obj
    pushLOINCAsCpt: function (JsonObj) {

        var observation = JsonObj["Observation"];
        var LOINCCOde = JsonObj['LOINICCODE'];
        var LOINCDescription = JsonObj['LOINICDescription'];

        //ClinicalLabResultDetail.BindLabOrderResultGridItem(LOINCCOde, LOINCDescription, LOINCDescription);
        ClinicalLabResultDetail.bindLabTestResultAttributes("", LOINCCOde, LOINCDescription);

    },

    pushOrganisms: function (JsonObj, elem) {

        var observation = JsonObj["Observation"];
        var LOINCCOde = JsonObj['LOINICCODE'];
        var LOINCDescription = JsonObj['LOINICDescription'];

        var code = '<input type="hidden" name="OrganismCode" value="' + JsonObj["Code"] + '"/>';
        var description = '<input type="hidden" name="OrganismDescription" value="' + JsonObj["Description"] + '"/>';

        $(elem).after(description);
        $(elem).after(code);


        //$(code).insertAfter('#pnlClinicalLabResultDetail #txtResult');
        //$(description).insertAfter('#pnlClinicalLabResultDetail #txtResult');



    },

    bindLabTestResultAttributes: function (modifier, LOINCCOde, LOINCDescription) {
        var _selectedLabId = $('#' + ClinicalLabResultDetail.params.PanelID + " #ddlLabId").val();

        if (_selectedLabId != "") {
            ClinicalLabResultDetail.loadLabTest_DBCall(_selectedLabId, LOINCCOde, LOINCDescription).done(function (response) {

                var data = JSON.parse(response);
                if (data.status != false) {
                    var LabTestJSON = JSON.parse(data.ClinicalLabTest_JSON);
                    if (LabTestJSON.length > 0) {
                        //
                        if (LabTestJSON[0].IsActive != "False") {
                            if (LabTestJSON[0].IsTemplate == "False") {
                                ClinicalLabResultDetail.BindLabOrderResultGridItem(modifier, null, LOINCCOde, LOINCDescription, LOINCDescription, true, data);
                                ClinicalLabResultDetail.isAttribute = true;
                            } else {
                                ClinicalLabResultDetail.BindLabOrderResultGridItem(modifier, null, LOINCCOde, LOINCDescription, LOINCDescription, true, data);
                                ClinicalLabResultDetail.isAttribute = false;
                            }
                        } else {
                            ClinicalLabResultDetail.BindLabOrderResultGridItem(modifier, null, LOINCCOde, LOINCDescription, LOINCDescription);
                        }
                        //
                    } else {
                        ClinicalLabResultDetail.BindLabOrderResultGridItem(modifier, null, LOINCCOde, LOINCDescription, LOINCDescription);
                    }

                } else {
                    ClinicalLabResultDetail.BindLabOrderResultGridItem(modifier, null, LOINCCOde, LOINCDescription, LOINCDescription);
                }
            });
        } else {
            ClinicalLabResultDetail.BindLabOrderResultGridItem(modifier, null, LOINCCOde, LOINCDescription, LOINCDescription);
        }
    },

    BindLabOrderResultGridItem: function (modifier, item_, cptCode, procedureDescription, cptDescription, isAttribute, attributeResponse) {

        var currentRow = $("#" + ClinicalLabResultDetail.params.PanelID + " #dgvLabResultDetail tbody tr:not([class*='child']");
        var isTestAlreadySelected = false;

        $.each(currentRow, function (i, item) {

            var currentRowCPTDescription = $(item).attr('CPTDescription').trim();

            if (cptDescription.toLowerCase().trim() == currentRowCPTDescription.toLowerCase()) {
                isTestAlreadySelected = true;
                return;
            }

        });

        if (isTestAlreadySelected != true) {

            var row = ClinicalLabResultDetail.addLabResultRow(modifier, item_, cptCode, cptDescription, isAttribute, attributeResponse);
            var $row = ClinicalLabResultDetail.EditableGrid.datatable.row.add(row);


            //===========================

            if (isAttribute) {
                var labTestJSON = JSON.parse(attributeResponse.ClinicalLabTest_JSON);
                if (labTestJSON[0].IsTemplate == "False") {
                    var attributeJSON = JSON.parse(attributeResponse.ClinicalLabTestAttribute_JSON);
                    var attributeResultJSON = JSON.parse(attributeResponse.ClinicalLabTestAttributeResult_JSON);
                    $.each(attributeJSON, function (i, item) {
                        if (item.AttributeName != "") {
                            var childRow = ClinicalLabResultDetail.addNewResultChildRow($row, null, true, item.AttributeName, null, null, item.UoM, item.Range, item.LabTestId, item.LabTestAttributeId, null, attributeResultJSON);
                            var parentId = $(childRow).attr("child-id");
                            var gRow = null;
                            if (ClinicalLabResultDetail.params.ParentCtrl == "Clinical_LabOrder" || ClinicalLabResultDetail.params.ParentCtrl == "ClinicalLabOrderDetail" || ClinicalLabResultDetail.params.ParentCtrl == "clinicalTabLabOrder")
                                gRow = ClinicalLabResultDetail.addNewResultGrandChildRow(parentId, row, null, item);
                        }
                        if (childRow != null) {

                            if (gRow != null)
                                childRow = childRow.add(gRow);

                            childRow = childRow.add($row.child());
                            $row.child(childRow);

                            $(row).find("td:first").find('a').show();
                            $(row).find("td:first .fa-plus-square").attr("class", "fa fa-minus-square");
                            $row.child.show();
                            ClinicalLabResultDetail.SetDateTimeControl(childRow, true);
                            ClinicalLabResultDetail.enableDisableOrderResultButtons();
                        }
                        else {
                            utility.DisplayMessages("Result is already selected", 2);
                        }
                    });
                } else {
                    var attributeJSON = JSON.parse(attributeResponse.ClinicalLabTestAttribute_JSON);
                    $.each(attributeJSON, function (i, item) {
                        var childRow = ClinicalLabResultDetail.addNewResultChildRow($row, null, true, null, true, item.Description);
                        var parentId = $(childRow).attr("child-id");
                        var gRow = null;
                        if (ClinicalLabResultDetail.params.ParentCtrl == "Clinical_LabOrder" || ClinicalLabResultDetail.params.ParentCtrl == "ClinicalLabOrderDetail" || ClinicalLabResultDetail.params.ParentCtrl == "clinicalTabLabOrder")
                            gRow = ClinicalLabResultDetail.addNewResultGrandChildRow(parentId, $row, null, item);
                        if (childRow != null) {

                            if (gRow != null)
                                childRow = childRow.add(gRow);

                            childRow = childRow.add($row.child());
                            $row.child(childRow);

                            $(row).find("td:first").find('a').show();
                            $(row).find("td:first .fa-plus-square").attr("class", "fa fa-minus-square");
                            $row.child.show();
                            ClinicalLabResultDetail.SetDateTimeControl(childRow, true);
                            ClinicalLabResultDetail.enableDisableOrderResultButtons();
                        }
                        else {
                            utility.DisplayMessages("Result is already selected", 2);
                        }
                    });
                }
            }

            //===========================

            setTimeout(function () {
                $("#" + ClinicalLabResultDetail.params.PanelID + " #txtLabCPTCode").val('');
            }, 200);

        }
        else {
            utility.DisplayMessages("Test is already selected", 2);
        }

        //Start 05-04-2016 Humaira Yousaf to enable/disable action buttons
        if ($("#" + ClinicalLabResultDetail.params.PanelID + " #dgvLabResultDetail tbody tr").length > 0 && $("#" + ClinicalLabResultDetail.params.PanelID + " #dgvLabResultDetail tbody tr").find('.dataTables_empty').length == 0) {
            $("#" + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabOrderDetail #btnSaveOrder,#btnSignPrintOrder,#btnSignOrder,#btnResetLabOrder").removeClass('disableAll');
        }
        else {
            $("#" + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabOrderDetail #btnSaveOrder,#btnSignPrintOrder,#btnSignOrder,#btnResetLabOrder").addClass('disableAll');
        }
        if (globalAppdata["isTransPubHealthAgAntimicobialUse"] && globalAppdata["isTransPubHealthAgAntimicobialUse"].toLowerCase() == "false")
            $("#" + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #dvQuestionnaire").addClass('hidden');
        //End 05-04-2016 Humaira Yousaf to enable/disable action buttons
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Binding numpad with height, weight, systolic and diastolic fields
    domReadyFunction: function () {
        $(document).ready(function () {

            $('#' + ClinicalLabResultDetail.params.PanelID + ' #ddlBillingTypeId').on('change', function () {

                if ($(this).val() == 3) {

                    ClinicalLabResultDetail.enableDisableInsurances(true);

                    var selectedVal1 = '';
                    var selectedVal2 = '';
                    var selectedVal3 = '';
                    $('#' + ClinicalLabResultDetail.params.PanelID + ' #ddlPrimaryInsuraceId option').each(function () {
                        if ($(this).attr('Priority') == "1") {
                            selectedVal1 = $(this).val();
                        }

                    });
                    $('#' + ClinicalLabResultDetail.params.PanelID + ' #ddlPrimaryInsuraceId').val(selectedVal1);

                    $('#' + ClinicalLabResultDetail.params.PanelID + ' #ddlSecondaryInsuraceId option').each(function () {
                        if ($(this).attr('Priority') == "2") {
                            selectedVal2 = $(this).val();
                        }
                    });
                    $('#' + ClinicalLabResultDetail.params.PanelID + ' #ddlSecondaryInsuraceId').val(selectedVal2);

                    $('#' + ClinicalLabResultDetail.params.PanelID + ' #ddlTertiaryInsuraceId option').each(function () {
                        if ($(this).attr('Priority') == "3") {
                            selectedVal3 = $(this).val();
                        }
                    });
                    $('#' + ClinicalLabResultDetail.params.PanelID + ' #ddlTertiaryInsuraceId').val(selectedVal3);
                }
                else {

                    ClinicalLabResultDetail.enableDisableInsurances(false);

                    $('#' + ClinicalLabResultDetail.params.PanelID + ' #ddlPrimaryInsuraceId').val('0');
                    $('#' + ClinicalLabResultDetail.params.PanelID + ' #ddlSecondaryInsuraceId').val('0');
                    $('#' + ClinicalLabResultDetail.params.PanelID + ' #ddlTertiaryInsuraceId').val('0');

                }

            });



            $('#' + ClinicalLabResultDetail.params.PanelID + ' .toggleHorSmallLeft section').unbind('click').bind("click", function (e) {
                $(this).parent().toggleClass("toggled");
                ClinicalConsultationOrderDetail.toggleHorSmallLeftIcon($(this));

            });
            //$('.toggleHorSmallLeft section').click(function (e) {
            //    $(this).parent().toggleClass("toggled");
            //    ClinicalLabResultDetail.toggleHorSmallLeftIcon($(this));

            //});
        });
        $(function () {
            $('#' + ClinicalLabResultDetail.params.PanelID + ' #frmClinicalLabResultDetail [data-plugin-keyboard-numpad]').keyboard({
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

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: To Disable inurances
    enableDisableInsurances: function (enable) {
        if (enable) {
            $('#' + ClinicalLabResultDetail.params.PanelID + ' #ddlPrimaryInsuraceId').attr('disabled', false);
            $('#' + ClinicalLabResultDetail.params.PanelID + ' #ddlSecondaryInsuraceId').attr('disabled', false);
            $('#' + ClinicalLabResultDetail.params.PanelID + ' #ddlTertiaryInsuraceId').attr('disabled', false);
        }
        else {
            $('#' + ClinicalLabResultDetail.params.PanelID + ' #ddlPrimaryInsuraceId').attr('disabled', true);
            $('#' + ClinicalLabResultDetail.params.PanelID + ' #ddlSecondaryInsuraceId').attr('disabled', true);
            $('#' + ClinicalLabResultDetail.params.PanelID + ' #ddlTertiaryInsuraceId').attr('disabled', true);
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
            WeightInLbs = $("#" + ClinicalLabResultDetail.params.PanelID + " #txtWeight").val();
        }

        var HeightInFeet = "";
        if (objHeightInFeet != null) {
            HeightInFeet = $(objHeightInFeet).val();
        }
        else {
            HeightInFeet = $("#" + ClinicalLabResultDetail.params.PanelID + " #txtHeight").val();
        }

        var CtrlName = "";
        if (TargetCtrl != null) {
            CtrlName = $(TargetCtrl);
        }
        else {
            CtrlName = $("#" + ClinicalLabResultDetail.params.PanelID + " #txtBSA");
        }

        if (WeightInLbs == "" || WeightInLbs == ".")
            var weightInLbs = 0;
        else
            var weightInLbs = parseFloat(WeightInLbs);
        if (HeightInFeet == "" || HeightInFeet == ".")
            heightInFeet = 0;
        else
            var heightInFeet = parseFloat(HeightInFeet);

        var weightInKG = ClinicalLabResultDetail.convertWeight(weightInLbs);
        var heightIn_cm = ClinicalLabResultDetail.convertHeightTo_cm(heightInFeet);
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
            WeightInLbs = $("#" + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #txtWeight").val();
        }

        var HeightInFeet = "";
        if (objHeightInFeet != null) {
            HeightInFeet = $(objHeightInFeet).val();
        }
        else {
            HeightInFeet = $("#" + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #txtHeight").val();
        }

        var CtrlName = "";
        if (TargetCtrl != null) {
            CtrlName = $(TargetCtrl);
        }
        else {
            CtrlName = $("#" + ClinicalLabResultDetail.params.PanelID + " #txtBMI");
        }

        if (WeightInLbs == "" || WeightInLbs == ".")
            var weightInLbs = 0;
        else
            var weightInLbs = parseFloat(WeightInLbs);
        if (HeightInFeet == "" || HeightInFeet == ".")
            heightInFeet = 0;
        else
            var heightInFeet = parseFloat(HeightInFeet);

        var heightInInches = ClinicalLabResultDetail.convertHeightInches(heightInFeet);

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
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "ClinicalLabResultDetail", null, false);
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
        if (ClinicalLabResultDetail.params.TabID == 'clinicalTabProgressNote') {
            params['FromProgressNote'] = 'pnlClinicalProgressNote';
            params["ParentCtrl"] = 'ClinicalLabResultDetail';
        }

        else {
            params["ParentCtrl"] = ClinicalLabResultDetail.params["TabID"];

        }
        params["PanelID"] = ClinicalLabResultDetail.params["PanelID"];

        params["ActionPanContainer"] = ClinicalLabResultDetail.params["ActionPanContainer"];
        if (Ctrl != null) {
            params["RefCtrl"] = Ctrl;
        }
        if (HiddenCtrl != null) {
            params["RefHiddenCtrl"] = HiddenCtrl;
        }
        if (controlToLoad != "") {
            if (ClinicalLabResultDetail.params.TabID == 'clinicalTabProgressNote' && SearchType == "ICD")
                LoadActionPan(controlToLoad, params, 'pnlClinicalProgressNote');
            else
                LoadActionPan(controlToLoad, params, ClinicalLabResultDetail.params.PanelID);
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
        ClinicalLabResultDetail.deletedChildRows = [];
        var form = '#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail";
        //Start//Abid Ali // For bug# EMR-896
        $('#' + ClinicalLabOrderDetail.params.PanelID + ' #divLabOrderInformation').show();
        $('#' + ClinicalLabOrderDetail.params.PanelID + ' #divLabBillingInformation').show();
        //End//Abid Ali // For bug# EMR-896

        var saveButtonisHidden = $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #btnSaveOrder").hasClass("hidden");

        if (caller == 'saveExit' || saveButtonisHidden == true) {
            if (ClinicalLabResultDetail.params["ParentCtrl"] == "mstrTabDashBoard") {
                if (DashBoard != null && DashBoard.DashBoardLabOrderGridLoad != null) {
                    var PageNo = 1;
                    var tab_ = $('#pnlDashboard #pnlLabOrderGrid #pnlLabOrder ul#ulLabOrderResultTabsItems li.active').attr("id")
                    if (tab_ == "listLabOrder") {
                        PageNo = $("#pnlDashboard #dgvLabOrderDashboard_Paging ul li.active a").html();
                        DashBoard.DashBoardLabOrderSearch(PageNo, null, null);
                    }
                    else {
                        PageNo = $("#pnlDashboard #dgvLabResultDashboard_Paging ul li.active a").html();
                        DashBoard.DashBoardLabResultLoad(null, null, PageNo);
                    }
                }

                UnloadActionPan(ClinicalLabResultDetail.params["ParentCtrl"], "ClinicalLabResultDetail");
            }
            else if (ClinicalLabResultDetail.params["ParentCtrl"] == "Clinical_LabOrder") {
                UnloadActionPan(ClinicalLabResultDetail.params["ParentCtrl"], "ClinicalLabResultDetail", null, ClinicalLabResultDetail.params["ParentCtrlPanelID"]);
            }
            else {
                UnloadActionPan(ClinicalLabResultDetail.params["ParentCtrl"], "ClinicalLabResultDetail");

            }
        }
        else {
            utility.UnLoadDialog(form, function () {
                if (ClinicalLabResultDetail.params["ParentCtrl"] == "mstrTabDashBoard") {
                    if (DashBoard != null && DashBoard.DashBoardLabOrderGridLoad != null) {
                        var PageNo = 1;
                        var tab_ = $('#pnlDashboard #pnlLabOrderGrid #pnlLabOrder ul#ulLabOrderResultTabsItems li.active').attr("id")
                        if (tab_ == "listLabOrder") {
                            PageNo = $("#pnlDashboard #dgvLabOrderDashboard_Paging ul li.active a").html();
                            DashBoard.DashBoardLabOrderSearch(PageNo, null, null);
                        }
                        else {
                            PageNo = $("#pnlDashboard #dgvLabResultDashboard_Paging ul li.active a").html();
                            DashBoard.DashBoardLabResultLoad(null, null, PageNo);
                        }
                    }
                    UnloadActionPan(ClinicalLabResultDetail.params["ParentCtrl"], "ClinicalLabResultDetail");
                }
                else if (ClinicalLabResultDetail.params["ParentCtrl"] == "Clinical_LabOrder") {
                    UnloadActionPan(ClinicalLabResultDetail.params["ParentCtrl"], "ClinicalLabResultDetail", null, ClinicalLabResultDetail.params["ParentCtrlPanelID"]);
                }
                else {
                    UnloadActionPan(ClinicalLabResultDetail.params["ParentCtrl"], "ClinicalLabResultDetail");

                }
            }, function () {
                if (ClinicalLabResultDetail.params["ParentCtrl"] == "mstrTabDashBoard") {
                    if (DashBoard != null && DashBoard.DashBoardLabOrderGridLoad != null) {
                        var PageNo = 1;
                        var tab_ = $('#pnlDashboard #pnlLabOrderGrid #pnlLabOrder ul#ulLabOrderResultTabsItems li.active').attr("id")
                        if (tab_ == "listLabOrder") {
                            PageNo = $("#pnlDashboard #dgvLabOrderDashboard_Paging ul li.active a").html();
                            DashBoard.DashBoardLabOrderSearch(PageNo, null, null);
                        }
                        else {
                            PageNo = $("#pnlDashboard #dgvLabResultDashboard_Paging ul li.active a").html();
                            DashBoard.DashBoardLabResultLoad(null, null, PageNo);
                        }
                    }

                    UnloadActionPan(ClinicalLabResultDetail.params["ParentCtrl"], "ClinicalLabResultDetail");
                }
                else if (ClinicalLabResultDetail.params["ParentCtrl"] == "Clinical_LabOrder") {
                    UnloadActionPan(ClinicalLabResultDetail.params["ParentCtrl"], "ClinicalLabResultDetail", null, ClinicalLabResultDetail.params["ParentCtrlPanelID"]);
                }
                else {
                    UnloadActionPan(ClinicalLabResultDetail.params["ParentCtrl"], "ClinicalLabResultDetail");

                }
            });
        }
        $('#FaceSheetPager .slick-track').attr('style', 'opacity: 1; transform: translate3d(-57px, 0px, 0px); width: 1193px;');

        ClinicalLabResultDetail.loadProviderFromAppData = false;
        ClinicalLabResultDetail.loadFacilityFromAppData = false;
        ClinicalLabResultDetail.isProviderLoaded = false;
        ClinicalLabResultDetail.isFacilityLoaded = false;
    },

    LabOrderSave: function (JSONToSave, fromResult, NotUpdateTestValues) {
        var deffered = $.Deferred();
        JSONToSave = JSON.parse(JSONToSave);
        JSONToSave["Status"] = "Transmitted";

        JSONToSave = JSON.stringify(JSONToSave);

        //AppPrivileges.GetFormPrivileges("Orders and Results_Lab Result", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //if (strMessage == "") {

        ClinicalLabOrderDetail.saveLabOrder(JSONToSave, NotUpdateTestValues).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                $('#' + ClinicalLabResultDetail.params.PanelID + " #hfOrderNo").val(response.orderNo);
                if (fromResult == true) {
                    ClinicalLabResultDetail.explicitResultOrderId.push(response.orderNo);
                }
                $('#' + ClinicalLabResultDetail.params.PanelID + " #hfLabOrderId").val(response.radiologicalOrderId);
                ClinicalLabResultDetail.params.LabOrderId = response.radiologicalOrderId;
                Clinical_LabOrder.LabOrderSearch(null, null, null, null);
            }
            else {
                // utility.DisplayMessages(response.Message, 3);
            }
            deffered.resolve();

        });
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
        return deffered;
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Saves LabResult
    LabResultSave: function (sender) {

        if (!$('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #btnSaveResult").hasClass('disableAll')) {
            if ($('#' + ClinicalLabResultDetail.params.PanelID + " #txtFacility").val() == '-Select -' || $('#' + ClinicalLabResultDetail.params.PanelID + " #txtFacility").val() == "" || $('#' + ClinicalLabResultDetail.params.PanelID + " #hfFacility").val() < 1) {
                utility.DisplayMessages("Please select a valid facility.", 2);
                return;
            }
            var IsValid = true;
            if (ClinicalLabResultDetail.params.LabOrderId < 1) {
                IsValid = false;
                var labOrderJson = ClinicalLabResultDetail.getLabRowsJSON();


                $.when(ClinicalLabResultDetail.LabOrderSave(labOrderJson, true, 1)).done(function () {
                    ClinicalLabResultDetail.LabResultInsertUpdate(sender);
                });
            }
            else {
                var labOrderJson = ClinicalLabResultDetail.getLabRowsJSON();
                $.when(ClinicalLabResultDetail.LabOrderSave(labOrderJson, null, 1)).done(function () {
                    ClinicalLabResultDetail.LabResultInsertUpdate(sender);
                });
            }
        }
    },


    SaveFavToggelStatus: function (FavListVal) {
        var isFavListOpened = $('#' + ClinicalLabResultDetail.params.PanelID + " #favSectionDiv").hasClass("toggled");
        $.when(EMRUtility.insertUpdateFavListStatus(ClinicalLabResultDetail.FavListName, isFavListOpened)).then(function () {
            EMRUtility.insertUpdateFavListVal(ClinicalLabResultDetail.FavListName, FavListVal);
        });
    },

    LabResultInsertUpdate: function (sender) {
        var PreFavVal = $('#' + ClinicalLabResultDetail.params.PanelID + ' #ddlFavLabResult').val();
        var LabResultId = $('#' + ClinicalLabResultDetail.params.PanelID + " #hfLabResultId").val() != "" ? $('#' + ClinicalLabResultDetail.params.PanelID + " #hfLabResultId").val() : "-1";

        var self = $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail");
        if (self.find("#txtComment")) {
            self.find("#txtComments").val(self.find("#txtComment").text())
        }
        else {
            self.find("#txtComments").val("");
        }
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objData = JSON.parse(myJSON);

        objData["Comments"] = $("#" + ClinicalLabResultDetail.params.PanelID + " #txtComment").html();

        objData["FinalInterpretation"] = ClinicalLabResultDetail.getFinalInterpretationjson()

        //Push ParentChild Rows in LabOrderResultDetailModels
        var LabOrderResultDetailModels = [];

        $("#" + ClinicalLabResultDetail.params.PanelID + " #dgvLabResultDetail tbody tr:not([class*=child]").each(function () {

            var rowJSON = ClinicalLabResultDetail.getParentChildJson(this);
            LabOrderResultDetailModels.push(rowJSON);
        });
        objData["LabOrderResultDetailModels"] = LabOrderResultDetailModels;

        objData["DeletedResultDetailIds"] = ClinicalLabResultDetail.deletedChildRows;

        objData["OrderNo"] = self.find('#hfOrderNo').val();
        objData["LabOrderId"] = ClinicalLabResultDetail.params.LabOrderId;


        if ($('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #ddlStatus").val().toLowerCase() == "partial") {
            objData["IsSentToPortal"] = false;
        } else {
            objData["IsSentToPortal"] = true;
        }

        //For send to Portal
        //  if (sender == "SendToPortal")
        //objData["IsSentToPortal"] = true;
        // else {
        //    objData["IsSentToPortal"] = false;
        //   }

        if (sender == "Acknowledge")
            objData["IsAknowledged"] = true;
        else {
            objData["IsAknowledged"] = false;
        }

        if ($("#chkMarkAsReviewed").is(":checked"))
            objData["IsElectronicResult"] = true;
        else
            objData["IsElectronicResult"] = false;

        myJSON = JSON.stringify(objData);


        if (ClinicalLabResultDetail.params.mode == "Add") {

            AppPrivileges.GetFormPrivileges("Orders and Results_Lab Result", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    ClinicalLabResultDetail.saveLabResult(myJSON).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            ClinicalLabResultDetail.SaveFavToggelStatus(PreFavVal);
                            DashBoard.GetAssignedLabResultsCount();
                            if (ClinicalLabResultDetail.params.ParentCtrl == "ClinicalLabOrderDetail") {
                                ClinicalLabOrderDetail.AddResult = true;
                                ClinicalLabOrderDetail.AddedLabOrderResultId = response.labOrderResultId;
                            }
                            if (ClinicalLabResultDetail.params.ParentCtrl == "clinicalTabProgressNote") {
                                ClinicalLabResultDetail.getLatestLabResultByPatientId();
                                EMRUtility.scrollToPNcomponent('clinical_labresults');
                            } else {
                                utility.DisplayMessages(response.message, 1);
                            }
                            $('#' + ClinicalLabResultDetail.params.PanelID + " #hfLabResultId").val(response.radiologicalOrderId);


                            //Clinical_LabOrder.LabOrderSearch(null, null, null, "LabOrderDetail");
                            if ($("#" + Clinical_LabOrder.params.PanelID + " #PatientlistLabOrderSent").hasClass('active')) {
                                Clinical_LabOrder.LabOrderSearch('0', '', '', null, 'Signed');
                            }
                            $("#mainForm  li#CDSAlert").show();
                            $.when(ClinicalCDSDetail.showCDSAlert("", $('#PatientProfile #hfPatientId').val())).then(function () {
                                if (ClinicalLabResultDetail.params.ParentCtrl == "Clinical_LabOrder" && Clinical_LabOrder.params.ParentCtrl == "clinicalTabProgressNote")
                                    Clinical_ProgressNote.LoadCDSAlerts();
                                EMRUtility.scrollToPNcomponent('clinical_labresults');
                            });
                            $.when(Clinical_LabOrder.LabResultsSearch(null, null, null, "LabResultDetail")).then(function () {
                                if (ClinicalLabResultDetail.params.ParentCtrl == "Clinical_LabOrder" && Clinical_LabOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                                    //$("#pnlClinicalProgressNote #pnlClinicalLabOrder #dgvLabResult input#" + response.labOrderResultId).prop("checked", true);
                                    $("#pnlClinicalProgressNote #pnlClinicalLabOrder #dgvLabResult input#" + response.labOrderResultId).trigger('click');
                                    EMRUtility.scrollToPNcomponent('clinical_labresults');
                                }
                            });


                            if (sender == "Acknowledge") {
                                $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #btnAddLabResult").text("View Result");
                            }
                            else {
                                $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #btnAddLabResult").text("Edit Result");
                            }

                            if (sender == 'signprintorder') {
                                ClinicalLabResultDetail.printLabResult();
                            }
                            else if (sender == '') {

                            }
                            else {
                                ClinicalLabResultDetail.UnLoad('saveExit');
                                $('#FaceSheetPager .slick-track').attr('style', 'opacity: 1; transform: translate3d(-57px, 0px, 0px); width: 1193px;');
                            }

                            //Commenting this after discussion with Ali Raza
                            //   ClinicalCDSDetail.showCDSAlert('', 0);
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
        else if (ClinicalLabResultDetail.params.mode == "Edit") {

            AppPrivileges.GetFormPrivileges("Orders and Results_Lab Result", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    ClinicalLabResultDetail.saveLabResult(myJSON, LabResultId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            ClinicalLabResultDetail.SaveFavToggelStatus(PreFavVal);
                            DashBoard.GetAssignedLabResultsCount();
                            if (ClinicalLabResultDetail.params.ParentCtrl == "ClinicalLabOrderDetail") {
                                ClinicalLabOrderDetail.AddResult = true;
                                ClinicalLabOrderDetail.AddedLabOrderResultId = response.labOrderResultId;
                            }
                            if (ClinicalLabResultDetail.params.ParentCtrl == "clinicalTabProgressNote") {
                                ClinicalLabResultDetail.getLatestLabResultByPatientId();
                                EMRUtility.scrollToPNcomponent('clinical_labresults');
                            } else {
                                utility.DisplayMessages(response.message, 1);
                            }

                            if (ClinicalLabResultDetail.params["ParentCtrl"] == "mstrTabDashBoard") {
                                ClinicalLabResultDetail.UnLoad('saveExit');
                                return;
                            }

                            $('#' + ClinicalLabResultDetail.params.PanelID + " #hfLabResultId").val(response.radiologicalOrderId);


                            //Clinical_LabOrder.LabOrderSearch(null, null, null, "LabOrderDetail");
                            if ($("#" + Clinical_LabOrder.params.PanelID + " #PatientlistLabOrderSent").hasClass('active')) {
                                Clinical_LabOrder.LabOrderSearch('0', '', '', null, 'Signed');
                            }
                            $("#mainForm  li#CDSAlert").show();
                            $.when(ClinicalCDSDetail.showCDSAlert("", $('#PatientProfile #hfPatientId').val())).then(function () {
                                if (ClinicalLabResultDetail.params.ParentCtrl == "Clinical_LabOrder" && Clinical_LabOrder.params.ParentCtrl == "clinicalTabProgressNote")
                                    Clinical_ProgressNote.LoadCDSAlerts();
                                EMRUtility.scrollToPNcomponent('clinical_labresults');
                            });
                            $.when(Clinical_LabOrder.LabResultsSearch(null, null, null, "LabResultDetail")).then(function () {
                                if (ClinicalLabResultDetail.params.ParentCtrl == "Clinical_LabOrder" && Clinical_LabOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                                    //$("#pnlClinicalProgressNote #pnlClinicalLabOrder #dgvLabResult input#" + response.labOrderResultId).prop("checked", true);
                                    $("#pnlClinicalProgressNote #pnlClinicalLabOrder #dgvLabResult input#" + response.labOrderResultId).trigger('click');
                                    EMRUtility.scrollToPNcomponent('clinical_labresults');
                                }
                            });

                            if (sender == "Acknowledge") {
                                $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #btnAddLabResult").text("View Result");
                            }
                            else {
                                $('#' + ClinicalLabOrderDetail.params.PanelID + " #frmClinicalLabOrderDetail #btnAddLabResult").text("Edit Result");
                            }

                            if (sender == 'signprintorder') {
                                ClinicalLabResultDetail.printLabResult();
                            }
                            else if (sender == '') {

                            }
                            else {
                                ClinicalLabResultDetail.UnLoad('saveExit');
                            }
                            //for EMR-2776 to remove exception
                            //  ClinicalCDSDetail.showCDSAlert('', 0);

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
    },

    //Function Name: cacheProcedureOrderData
    //Author Name: Humaira Yousaf
    //Created Date: 06-04-2016
    //Description: Saves procedure order detail data
    cacheLabResultData: function () {
        Clinical_LabOrder.params["ProviderName"] = $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #txtProvider").val();
        Clinical_LabOrder.params["ProviderId"] = $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #hfProvider").val();

        Clinical_LabOrder.params["AssigneeName"] = $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #ddlAssigneeId").val();
        Clinical_LabOrder.params["AssigneeId"] = $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #hfAssignee").val();

        Clinical_LabOrder.params["Problems"] = ClinicalLabResultDetail.LabResultProblems;

        Clinical_LabOrder.params["LabId"] = $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #ddlLabId").val();
        Clinical_LabOrder.params["BillingTypeId"] = $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #ddlBillingTypeId").val();

        Clinical_LabOrder.params["FacilityName"] = $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #txtFacility").val();
        Clinical_LabOrder.params["FacilityId"] = $('#' + ClinicalLabResultDetail.params.PanelID + " #frmClinicalLabResultDetail #hfFacility").val();


        Clinical_LabOrder.params["CurrentPatientId"] = Clinical_LabOrder.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        Clinical_LabOrder.params["hasData"] = true;
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: To check whether Lab order exists or not
    checkLabResultExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_LabResults').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ObjectiveNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="LabResultComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_LabResults title="Lab Order"  id="clinicalMenu_Orders_Lab" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'Lab\',\'clinicalMenu_Orders_Lab\',' + Clinical_ProgressNote.params.NotesId + ');" title="Lab Order">Lab Order</a> ' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'Lab Order\',\'clinicalMenu_Orders_Lab\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_LabResults> </header></li>');
            Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
                $(this).find('.closeBtn').removeClass('hidden');
                $(this).css('background', '#EAF1F8');
            });

            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
                $(this).find('.closeBtn').addClass('hidden');
                $(this).css('background', '#fff');
            });
        }
    },


    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: To show LabResult Search in Popup
    openLabResultAlert: function () {

        if ($(" #mainForm  li#LabResultAlert span").text() != '' && $('#PatientProfile #hfPatientId').val() != '') {
            BackgroundLoaderShow(true);
            var params = [];


            params["FromAdmin"] = 0;
            //   params["StartupScreen"] = "message";
            LoadActionPan("Clinical_LabResult", params);
        }
    },

    getLabRowsJSON: function () {

        var selfLabOrder = $("#" + ClinicalLabResultDetail.params.PanelID + " #divLabResultInformation");
        var myJSONLabOrder = selfLabOrder.getMyJSONByName();
        var LabTestIds = $("#" + ClinicalLabResultDetail.params.PanelID + " #dgvLabResultDetail tbody tr:not([id*=Child]").map(function () {
            return this.id.replace("id", "");
        }).get().join(',');

        $("#" + ClinicalLabResultDetail.params.PanelID + " #dgvLabResultDetail tbody tr:not([id*=child]").each(function (i, item) {
            var rowObject = new Object();
            rowObject["LabOrderTestId" + $(item).attr("id")] = $(item).attr("id");
            rowObject["dtpLabDate" + $(item).attr("id")] = $(item).find('td input[id^=dtpDate]').val();
            rowObject["tpLabTime" + $(item).attr("id")] = $(item).find('td input[id^=tpTime]').val();
            //   rowObject["LabProcedure" + $(item).attr("id")] = $(item).attr("CPTCode") + " - " + $(item).attr("CPTDescription");
            //modified for search
            rowObject["LabProcedure" + $(item).attr("id")] = $(item).attr("CPTCode") + " " + $(item).attr("CPTDescription");
            rowObject["Modifier" + $(item).attr("id")] = $(item).attr("Modifier");
            rowObject["CPTCode" + $(item).attr("id")] = $(item).attr("CPTCode");
            rowObject["CPTDescription" + $(item).attr("id")] = $(item).attr("CPTDescription");

            rowObject["Specimen" + $(item).attr("id")] = $(item).attr("SpecimenId");
            rowObject["SpecimenSource" + $(item).attr("id")] = $(item).attr("SpecimenSourceId");
            rowObject["Organism" + $(item).attr("id")] = $(item).attr("OrganismId");
            rowObject["Antimicrobials" + $(item).attr("id")] = $(item).attr("AntimicrobialIds");
            rowObject["TestTypeId" + $(item).attr("id")] = $(item).attr("TestTypeId");



            var currentRowJSON = JSON.stringify(rowObject);
            myJSONLabOrder = utility.MergeJSON(myJSONLabOrder, currentRowJSON);
        });
        //Hard coded for Lab Result Order Creation
        var objRad = new Object();

        if (ClinicalLabResultDetail.params.LabOrderId && ClinicalLabResultDetail.params.LabOrderId > 0) {
            objRad["LabOrderId"] = ClinicalLabResultDetail.params.LabOrderId;
        } else {
            objRad["LabOrderId"] = '-1';
        }
        if (ClinicalLabResultDetail.params.ParentCtrl != "mstrTabDashBoard") {
            objRad["PatientId"] = Clinical_LabOrder.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        } else if (ClinicalLabResultDetail.params.ParentCtrl == "mstrTabDashBoard") {
            objRad["PatientId"] = $("#pnlClinicalLabResultDetail #frmClinicalLabResultDetail #hfPatientId").val();
        }

        objRad["LabTestIds"] = LabTestIds;
        objRad["Status"] = 'Signed';
        var myJSON = JSON.stringify(objRad);
        myJSONLabOrder = utility.MergeJSON(myJSON, myJSONLabOrder);
        return myJSONLabOrder;
    },


    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: get Latest Lab Order By PatientId
    getLatestLabResultByPatientId: function () {
        var strMessage = '';
        //   AppPrivileges.GetFormPrivileges("Notes_Notes", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            ClinicalLabResultDetail.getLatestLabResultByPatientIdDBCall().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    ClinicalLabResultDetail.createLabResultBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML');
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
    createLabResultBodyHTML: function (response, NoteHTMLCtrl, UnloadLabResult) {
        ClinicalLabResultDetail.checkLabResultExists();
        if (response.LabResultFill_JSON != null && response.LabResultFill_JSON != '') {
            var LabResultFill_Obj = JSON.parse(response.LabResultFill_JSON);
            var $mainDivLabResult = $(document.createElement('div'));

            var LabResultId = LabResultFill_Obj.LabResultId;
            if (LabResultId > 0) {
                var $SectionBodyLabResult = $(document.createElement('section'));
                $SectionBodyLabResult.attr('id', "Cli_LabResultDetail_Main" + LabResultId);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_LabResultDetail_" + LabResultId);
                var $ListLabResult = $(document.createElement('ul'));

                $ListLabResult.attr('class', 'list-unstyled')

                $SectionBodyLabResult.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_LabResultDetail_" + LabResultId + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_LabResultDetail_Main" + LabResultId + '"  ><i class="fa fa-times"></i></a></div> ');


                $ListLabResult.append("<li>" + LabResultFill_Obj.SoapText + "</li>");
                $DetailsDiv.append($ListLabResult);
                $SectionBodyLabResult.append($DetailsDiv);
                if ($(NoteHTMLCtrl + ' clinical_LabResults').parent().parent().find('#Cli_LabResultDetail_Main' + LabResultId).length == 0) {
                    $mainDivLabResult.append($SectionBodyLabResult);
                    ClinicalLabResultDetail.updateLabResultHtml($mainDivLabResult.html(), LabResultId, NoteHTMLCtrl);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' clinical_LabResults').parent().parent().find('#Cli_LabResultDetail_Main' + LabResultId + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' clinical_LabResults').parent().parent().find('#Cli_LabResult_Main' + LabResultId + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' clinical_LabResults').parent().parent().find('#Cli_LabResultDetail_Main' + LabResultId).html($SectionBodyLabResult.html());
                    $(NoteHTMLCtrl + ' clinical_LabResults').parent().parent().find('#Cli_LabResultDetail_Main' + LabResultId + ' ul').append(CommentHTML);
                    Clinical_ProgressNote.saveComponentSOAPText('Lab Result');
                    ClinicalLabResultDetail.updateLabResultHtml("", LabResultId, NoteHTMLCtrl);

                }

                if (UnloadLabResult == true) {
                    ClinicalLabResultDetail.Unload(ClinicalLabResultDetail.bNextPrev);
                }
            }
        }
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: To detach Components Lab Order
    detach_ComponentsLabResult: function (ComponentName, IsUpdate, LabResultComponentRemove) {
        var LabResultIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_LabResults').parent().parent().find('section[id*="Cli_LabResultDetail_Main"]').map(function () {
            return this.id.replace("Cli_LabResultDetail_Main", "");
        }).get().join(',');
        var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .LabResultComponent').attr('NoteComponentId');
        if (LabResultIds == "" || LabResultIds == "undefined") {
            $.when(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId)).then(function () {
                Clinical_ProgressNote.ShowHideComponetsHeaders();
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
            });
        }
        else {
            ClinicalLabResultDetail.detachLabResultFromNotesDBCall(LabResultIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (IsUpdate) {

                        Clinical_ProgressNote.saveComponentSOAPText('Lab Results', true);
                    }
                    utility.DisplayMessages(response.Message, 1);
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        if (LabResultComponentRemove) {
            if (NoteComponentId && NoteComponentId != "NCDummyId") {
                $.when(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId)).then(function () {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Lab Orders']").remove();
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_LabResults').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
            else {
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Lab Orders']").remove();
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_LabResults').parent().parent().remove();
            }
        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_LabResults').parent().parent().find('section[id*="Cli_LabResultDetail_Main"]').remove();
        }
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: To detach Lab Order from Notes
    detachLabResultFromNotes: function (LabResultId) {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('1', function () {
                var selectedValue = LabResultId.replace('Cli_LabResultDetail_Main', '');
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    ClinicalLabResultDetail.detachLabResultFromNotes_DBCall(selectedValue).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            $('#' + LabResultId).remove();

                            Clinical_ProgressNote.saveComponentSOAPText('Lab Result');
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
    detachLabResultFromNotes_DBCall: function (LabResultId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["LabResultId"] = LabResultId;
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
        objData["commandType"] = "detach_LabResult_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: DB Call to detach Lab Order From Notes
    detachLabResultFromNotesDBCall: function (LabId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["LabResultId"] = LabId;
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
        objData["commandType"] = "detach_LabResult_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: To update Lab Order Html
    updateLabResultHtml: function (LabResultHtml, LabResultId, NoteHTMLCtrl) {
        $(NoteHTMLCtrl + ' clinical_LabResults').parent().parent().addClass('initialVisitBody');
        if (LabResultHtml != '') {
            $(NoteHTMLCtrl + ' clinical_LabResults').parent().parent().append(LabResultHtml);
        }
        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (LabResultHtml != '') {
            ClinicalLabResultDetail.attachLabResultWithNotes(LabResultId);
        }

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: To attach Lab Order With Notes
    attachLabResultWithNotes: function (LabResultId) {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {

            var selectedValue = LabResultId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                ClinicalLabResultDetail.attachLabResultWithNotesDBCall(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        //If Attached MedicalHx Made new inseration to MedicalHx Table than good ids should be attached to HTML
                        Clinical_ProgressNote.saveComponentSOAPText('Lab Result');
                        $('#' + LabResultId).remove();
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
    attachLabResultWithNotesDBCall: function (LabResultId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["LabResultId"] = LabResultId;
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
        objData["commandType"] = "attach_LabResult_with_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: DB Call to get Latest Lab Order By PatientId
    getLatestLabResultByPatientIdDBCall: function () {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["commandType"] = "getlatest_LabResultby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },

    getLabResultInfo: function (LabResultId) {
        ClinicalLabResultDetail.fillLabResult(LabResultId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    ClinicalLabResultDetail.createLabResultBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML');

                }
                else {
                    utility.DisplayMessages(strMessage, 3);
                }
            }
        });
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Creates PDF to view Lab Order
    printLabResult: function () {

        //utility.myConfirm('Would you like to print the Specimen Label for this result?', function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["UserId"] = globalAppdata['AppUserId'];
        if (ClinicalLabResultDetail.params.ParentCtrl == "mstrTabDashBoard") {
            params["PatientId"] = ClinicalLabResultDetail.params.LabResultPatientId;
        }
        else {
            params["PatientId"] = Clinical_LabOrder.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        }

        params["ParentCtrl"] = "ClinicalLabResultDetail";
        var LabResultId = $('#' + ClinicalLabResultDetail.params.PanelID + " #hfLabResultId").val();
        var LabOrderId = $('#' + ClinicalLabResultDetail.params.PanelID + " #hfLabOrderId").val();
        if (!(LabResultId != null && parseInt(LabResultId) > 0)) {
            LabResultId = ClinicalLabResultDetail.params.LabResultId;
        }
        if (!(LabOrderId != null && parseInt(LabOrderId) > 0)) {
            LabOrderId = ClinicalLabResultDetail.params.LabOrderId;
        }
        params["BarCodeHtml"] = 'true';
        params["LabResultId"] = LabResultId;
        params["LabOrderId"] = LabOrderId;
        LoadActionPan('Clinical_LabResultView', params);
        //}, function () {
        //    var params = [];
        //    params["FromAdmin"] = "0";
        //    params["UserId"] = globalAppdata['AppUserId'];
        //    params["PatientId"] = $('#PatientProfile #hfPatientId').val();
        //    params["ParentCtrl"] = "ClinicalLabResultDetail";
        //    var LabResultId = $('#' + ClinicalLabResultDetail.params.PanelID + " #hfLabResultId").val();
        //    var LabOrderId = $('#' + ClinicalLabResultDetail.params.PanelID + " #hfLabOrderId").val();
        //    if (!(LabResultId != null && parseInt(LabResultId) > 0)) {
        //        LabResultId = ClinicalLabResultDetail.params.LabResultId;
        //    }
        //    if (!(LabOrderId != null && parseInt(LabOrderId) > 0)) {
        //        LabOrderId = ClinicalLabResultDetail.params.LabOrderId;
        //    }
        //    params["BarCodeHtml"] = 'false';
        //    params["LabResultId"] = LabResultId;
        //    params["LabOrderId"] = LabOrderId;
        //    LoadActionPan('Clinical_LabResultView', params);
        //},
        //             'Specimen Label Printing');


    },


    getRandomNumber: function (isNegative, upperLimit) {

        isNegative = isNegative == null ? false : isNegative;
        upperLimit = upperLimit == null ? 1000 : upperLimit;

        var randomNumber = (1 + Math.floor(Math.random() * upperLimit));

        if (isNegative)
            randomNumber = randomNumber * -1;

        return randomNumber;
    },


    //Author: Abid Ali
    //Date :  07-06-2016
    //Description: checks radiology result in parent row passed
    isResultExists: function (currentRow, objJson) {

        var childExists = false;
        var currentLOINICCODE = "";
        var currentLOINCDescription = "";
        if (objJson != null) {
            currentLOINICCODE = objJson['LOINICCODE'];
            currentLOINCDescription = objJson['LOINICDescription'].trim();
        }
        ////Concat if both has values
        //if (currentLOINICCODE != null && currentLOINICCODE != "")
        //    currentLOINCDescription = currentLOINICCODE + " - " + currentLOINCDescription;


        if (currentRow.child() != null) {

            $.each(currentRow.child(), function (index, item) {

                var childRow = $(item);
                var LOINICCODE = childRow.attr('LOINICCODE');
                var LOINICDescription = childRow.attr('LOINICDescription');

                if (currentLOINICCODE == LOINICCODE && currentLOINCDescription == LOINICDescription) {
                    childExists = true;
                    return false;
                }
            });
        }
        return childExists;
    },



    //Start//---------- Grid Data -----------

    //For Adding/loading Parent row
    addLabResultRow: function (modifier, item, cptCode, cptDescription, isAttribute, attributeResponse) {
        var currentDate = new Date();
        var currentTime = currentDate.toLocaleTimeString().replace(/([\d]+:[\d]{2})(:[\d]{2})(.*)/, "$1$3");
        currentDate = currentDate.toLocaleDateString().replace(/[^A-Za-z 0-9 \.,\?""!@#\$%\^&\*\(\)-_=\+;:<>\/\\\|\}\{\[\]`~]*/g, '');

        var $row = $('<tr/>');
        if (item != null) {
            $row.attr("id", item.LabOrderTestId).addClass('parent-row');
            item.CPTCodeDescription = utility.decodeHtml(item.CPTCodeDescription);
            $row.attr("Modifier", item.Modifier);
            $row.attr("CPTCode", item.CPTCode);
            $row.attr("CPTDescription", item.CPTCodeDescription);

            $row.attr("SpecimenId", item.SpecimenId);
            $row.attr("SpecimenSourceId", item.SpecimenSourceId);
            $row.attr("OrganismId", item.OrganismId);
            $row.attr("Organism", item.Organism);
            $row.attr("AntimicrobialIds", item.AntimicrobialIds);
            $row.attr("Antimicrobials", item.Antimicrobials);
            $row.attr("TestTypeId", item.TestTypeId);

            if (item.Organism) {
                ClinicalLabResultDetail.Organisms.push(item.Organism);
            }
            if (item.ObservationDate != null) {
                currentDate = new Date(item.ObservationDate).toLocaleDateString().replace(/[^A-Za-z 0-9 \.,\?""!@#\$%\^&\*\(\)-_=\+;:<>\/\\\|\}\{\[\]`~]*/g, '');
                currentTime = new Date(item.ObservationDate).toLocaleTimeString().replace(/([\d]+:[\d]{2})(:[\d]{2})(.*)/, "$1$3");
            }
            else {
                currentDate = new Date().toLocaleDateString().replace(/[^A-Za-z 0-9 \.,\?""!@#\$%\^&\*\(\)-_=\+;:<>\/\\\|\}\{\[\]`~]*/g, '');
                currentTime = new Date().toLocaleTimeString().replace(/([\d]+:[\d]{2})(:[\d]{2})(.*)/, "$1$3");
            }
            //currentDate = currentDate + " " + currentTime;
            var expandCollapseIcon = '<a href="#" class="on-editing expand-row" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a>';
            if (ClinicalLabResultDetail.params.FromLabDetail || ClinicalLabResultDetail.params.mode == "Edit") {
                var removeIcon = '<a class="disableAll" href="#" onclick="ClinicalLabResultDetail.removelabResultParent(this);"  title="Delete Record"><i class="fa fa-close red"></i></a>';
            }
            else {
                var removeIcon = '<a href="#" onclick="ClinicalLabResultDetail.removelabResultParent(this);"  title="Delete Record"><i class="fa fa-close red"></i></a>';
            }
            var addRowIcon = '<a href="#" class="on-child-add add-child-row" title="Add Child Record">Add Observation</a>';

            //For result specimen if rejected
            var resultSpecimenRejectedToolTip = "";
            if (item.ResultSpecimens != null && item.ResultSpecimens.length > 0) {

                var title = ClinicalLabResultDetail.getResultRejectSpecimenTitle(item.ResultSpecimens);
                if (title != "") {
                    resultSpecimenRejectedToolTip = '<i class="fa fa-exclamation ellip100" style="color: red;padding-left:10px;" data-toggle="tooltip" data-placement="right" title="' + title + '"></i>';
                }
            }




            var controlwidth = "size65";
            var Timecontrolwidth = "size45";
            var controldisabledClass = "disabled";
            var calendars = '<div class="input-group ' + controlwidth + '"><span class="input-group-addon pl-tiny pr-tiny  xxs-font"><i class="fa fa-calendar"></i></span><input id="dtpDate' + ClinicalLabResultDetail.getRandomNumber(true, 1000) + '" name="Date" ' + controldisabledClass + ' type="text" class="form-control ' + controlwidth + ' p-tiny" value="' + currentDate + '" /></div>';
            var Time = '<div class="input-group size50"><span class="input-group-addon pl-tiny pr-tiny  xxs-font"><i class="fa fa-clock-o"></i></span><input id="tpTime' + ClinicalLabResultDetail.getRandomNumber(true, 1000) + '" name="Time" ' + controldisabledClass + ' type="text" class="form-control size60 p-tiny" value="' + currentTime + '" data-plugin-timepicker /></div>';

            $row.append('<td>' + expandCollapseIcon + " " + removeIcon + '</td><td>' + calendars + '</td><td>' + Time + '</td><td colspan="2">' + item.CPTCode + " " + item.CPTCodeDescription + resultSpecimenRejectedToolTip + '</td><td  colspan="4">' + addRowIcon + '</td><td style="display: none;"></td><td style="display: none;"></td><td style="display: none;"></td><td style="display: none;"></td>');
            $("#" + ClinicalLabResultDetail.params.PanelID + " #dgvLabResultDetail tbody").last().append($row);
            ClinicalLabResultDetail.SetDateTimeControl($row, false);
        }
        else {
            //Add random negative Id to parent row
            $row.attr("id", ClinicalLabResultDetail.getRandomNumber(true, 1000)).addClass('parent-row');
            $row.attr("CPTCode", cptCode);
            $row.attr("CPTDescription", cptDescription);
            $row.attr("Modifier", modifier);
            currentDate = new Date().toLocaleDateString().replace(/[^A-Za-z 0-9 \.,\?""!@#\$%\^&\*\(\)-_=\+;:<>\/\\\|\}\{\[\]`~]*/g, '');
            currentTime = new Date().toLocaleTimeString().replace(/([\d]+:[\d]{2})(:[\d]{2})(.*)/, "$1$3");

            //currentDate = currentDate + " " + currentTime;
            var expandCollapseIcon = '<a style="display:none" href="#" class="on-editing expand-row" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a>';
            var addRowIcon = '<a href="#" class="on-child-add add-child-row" title="Add Child Record">Add Observation</a>';
            if (ClinicalLabResultDetail.params.FromLabDetail) {
                var removeIcon = '<a href="#" class="disableAll" onclick="ClinicalLabResultDetail.removelabResultParent(this);"  title="Delete Record"><i class="fa fa-close red"></i></a>';
            }
            else {
                var removeIcon = '<a href="#" onclick="ClinicalLabResultDetail.removelabResultParent(this);"  title="Delete Record"><i class="fa fa-close red"></i></a>';
            }
            var controlwidth = "size65";
            var Timecontrolwidth = "size45";
            var controldisabledClass = "";
            var calendars = '<div class="input-group ' + controlwidth + '"><span class="input-group-addon pl-tiny pr-tiny  xxs-font"><i class="fa fa-calendar"></i></span><input id="dtpDate" name="Date' + ClinicalLabResultDetail.getRandomNumber(true, 1000) + '" ' + controldisabledClass + ' type="text" class="form-control ' + controlwidth + ' p-tiny" value="' + currentDate + '" /></div>';
            var Time = '<div class="input-group size50"><span class="input-group-addon pl-tiny pr-tiny  xxs-font"><i class="fa fa-clock-o"></i></span><input id="tpTime" name="Time' + ClinicalLabResultDetail.getRandomNumber(true, 1000) + '" ' + controldisabledClass + ' type="text" class="form-control size60 p-tiny" value="' + currentTime + '" data-plugin-timepicker /></div>';
            $row.append('<td>' + expandCollapseIcon + " " + removeIcon + '</td><td>' + calendars + '</td><td>' + Time + '</td><td colspan="2">' + cptCode + " " + cptDescription + '</td><td  colspan="4">' + addRowIcon + '</td><td style="display: none;"></td><td style="display: none;"></td><td style="display: none;"></td><td style="display: none;"></td>');

            $("#" + ClinicalLabResultDetail.params.PanelID + " #dgvLabResultDetail tbody").last().append($row);
            ClinicalLabResultDetail.SetDateTimeControl($row, false);

        }
        return $row;
    },
    removelabResultParent: function (obj) {

        var labOrderTestId = $(obj).parent().parent().attr('id');
        utility.myConfirm('1', function () {
            if (labOrderTestId > 0) {


                ClinicalLabResultDetail.deleteLabResults_DbCall(labOrderTestId).done(function (response) {

                    response = JSON.parse(response);
                    if (response.status != false) {
                        var tr = $(obj).closest('tr');
                        var table = $(" #dgvLabResultDetail").DataTable();
                        var row = table.row(tr);
                        row.child() != null ? row.child().remove() : true;
                        tr.remove();
                        utility.DisplayMessages("Successfully Deleted", 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            } else {
                var tr = $(obj).closest('tr');
                var table = $(" #dgvLabResultDetail").DataTable();
                var row = table.row(tr);
                row.child() != null ? row.child().remove() : true;
                tr.remove();
                utility.DisplayMessages("Successfully Deleted", 1);
            }
        }, function () {

        }, '1');

    },
    deleteLabResults_DbCall: function (labOrderTestId) {


        var objData = {};

        objData["LabOrderTestId"] = labOrderTestId;

        objData["commandType"] = "delete_labtest";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },
    SetDateTimeControl: function ($row, ForChild) {
        if (ForChild) {
            $row.find("input[id^=Child_dtp]").each(function () {

                var date_format = 'dd/mm/yyyy';
                //set default Date Formate
                if (globalAppdata['DateFormat'])
                    date_format = globalAppdata['DateFormat'];



                $(this).datepicker({
                    todayHighlight: true,
                    format: date_format,
                }).on('changeDate', function (e) {

                    $(this).datepicker('hide');
                    $(this).val(e.target.value);

                });
                //Begin 4/27/16  Edit By M Ahmad Imran
                if ($(this).val() == "" || $(this).val() == null) {
                    $(this).datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), new Date()));
                }
                //End 4/27/16  Edit By M Ahmad Imran

            });
            $row.find("input[id^=Child_tp]").each(function () {
                $(this).timepicker().on('changeTime.timepicker', function (e) {
                    disableFocus: false
                    $(this).val(e.target.value);
                });
                if ($(this).val() == "" || $(this).val() == null) {
                    $(this).timepicker('setTime', new Date());
                }
            });
        }
        else {
            $row.find("input[id^=dtp]").each(function () {

                var date_format = 'dd/mm/yyyy';
                //set default Date Formate
                if (globalAppdata['DateFormat'])
                    date_format = globalAppdata['DateFormat'];



                $(this).datepicker({
                    todayHighlight: true,
                    format: date_format,
                }).on('changeDate', function (e) {

                    $(this).datepicker('hide');
                    $(this).val(e.target.value);

                });
                //Begin 4/27/16  Edit By M Ahmad Imran
                if ($(this).val() == "" || $(this).val() == null) {
                    $(this).datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), new Date()));
                }
                //End 4/27/16  Edit By M Ahmad Imran

            });
            $row.find("input[id^=tp]").each(function () {
                $(this).timepicker().on('changeTime.timepicker', function (e) {
                    disableFocus: false
                    $(this).val(e.target.value);
                });
                if ($(this).val() == "" || $(this).val() == null) {
                    $(this).timepicker('setTime', new Date());
                }
            });
        }

    },
    //For loading child rows
    buildRowChild: function (CurrentRow, ChildItems, secondStepChild) {

        var currentDate = new Date();
        var currentTime = currentDate.toLocaleTimeString().replace(/([\d]+:[\d]{2})(:[\d]{2})(.*)/, "$1$3");
        currentDate = currentDate.toLocaleDateString().replace(/[^A-Za-z 0-9 \.,\?""!@#\$%\^&\*\(\)-_=\+;:<>\/\\\|\}\{\[\]`~]*/g, '');

        var CurrentRowchilds = $();

        if (ChildItems != null && ChildItems.ChildRows.length > 0) {

            //   for (var i = 0; i < ChildItems.length; i++) {
            $.each(ChildItems.ChildRows, function (i, item) {

                //Push values as attributes of row
                var $row = $('<tr/>').addClass("childRow-bg");
                $row.attr("child-id", item.LabOrderResultDetailId);
                $row.attr('LOINICCODE', item.LOINC)
                $row.attr('LOINICDescription', item.LOINCDescription);
                $row.attr("LabOrderResultId", item.LabOrderResultId);
                $row.attr("IsAttribute", item.IsAttribute);

                $row.attr("LabTestId", item.LabTestId);
                $row.attr("LabTestAttributeId", item.LabTestAttributeId);

                item.Observation = item.LOINC + " " + item.LOINCDescription;

                //Convert Date to prper date time format
                if (item.ObservationDate != null) {
                    currentDate = new Date(item.ObservationDate).toLocaleDateString().replace(/[^A-Za-z 0-9 \.,\?""!@#\$%\^&\*\(\)-_=\+;:<>\/\\\|\}\{\[\]`~]*/g, '');
                    currentTime = new Date(item.ObservationDate).toLocaleTimeString().replace(/([\d]+:[\d]{2})(:[\d]{2})(.*)/, "$1$3");
                }
                //currentDate = currentDate + " " + currentTime;
                var flagOptions = "<option value='Normal'>Normal</option>" +
                                   "<option value='Negative'>Negative</option>" +
                                   "<option value='Positive'>Positive</option>" +
                                   "<option value='High'>High</option>" +
                                   "<option value='Abnormal'>Abnormal</option>" +
                                   "<option value='Abnormally High'>Abnormally High</option>" +
                                   "<option value='Low'>Low</option>" +
                                   //"<option value='Susceptible. Indicates for microbiology susceptibilities only'>Susceptible. Indicates for microbiology susceptibilities only</option>" +
                                   "<option value='Abnormally Low'>Abnormally Low</option>";
                //  if ($(CurrentRow).attr("Organism") == "Enterococcus" && $(CurrentRow).attr("Antimicrobials") && ($(CurrentRow).attr("Antimicrobials").indexOf("Gentamicin") || $(CurrentRow).attr("Antimicrobials").indexOf("Streptomycin") >= 0)) {
                globalAppdata["isTransPubHealthAgAntimicobialUse"] && globalAppdata["isTransPubHealthAgAntimicobialUse"].toLowerCase() == "true" ?
                flagOptions +=
                    "<option value='S= Susceptible/Synergistic'>S= Susceptible/Synergistic</option>" +
                    "<option value='R= Resistant/Non Synergistic'>R= Resistant/Non Synergistic</option>" +
                    "<option value='S=Susceptible'>S=Susceptible</option>" +
                    "<option value='S-DD=Susceptible-Dose Dependent'>S-DD=Susceptible-Dose Dependent</option>" +
                    "<option value='I=Intermediate'>I=Intermediate</option>" +
                    "<option value='R=Resistant'>R=Resistant</option>" +
                    "<option value='NS=Non-Susceptible'>NS=Non-Susceptible</option>" +
                    "<option value='N=Not Tested'>N=Not Tested</option>" : "";

                var conditionStatementOptions = "<option value='>'>Greater Than</option>" +
                                  "<option value='>='>Greater Than or equal to</option>" +
                                  "<option value='<'>Less Than</option>" +
                                  "<option value='<='>Less Than or equal to</option>" +
                                  "<option value='='>Exactly Equal to</option>";


                var rowRemove = '<a href="#" class="btn-xs on-default remove-row mr-none btn" title="Delete Record"><i class="fa fa-close red"></i></a>';
                //Start 11-05-2016 Edit By Humaira Yousaf Bug# EMR-1021
                var ddlFlag = "<select onchange='ClinicalLabResultDetail.highlightResultValue(this);' id='ddlFlag" + item.LabResultTestId + "' name='Flag' class='form-control'>" + flagOptions + "</select>";
                var ddlConditionStatement = "<select id='ddlConditionStatement" + item.ConditionStatement + "' name='ConditionStatement' class='form-control'>" + conditionStatementOptions + "</select>";
                (globalAppdata["isTransPubHealthAgAntimicobialUse"] && globalAppdata["isTransPubHealthAgAntimicobialUse"].toLowerCase() == "false") ? ddlConditionStatement = "" : "";
                //End 11-05-2016 Edit By Humaira Yousaf Bug# EMR-1021

                (globalAppdata["isTransPubHealthAgCaseReporting"] && globalAppdata["isTransPubHealthAgCaseReporting"].toLowerCase() == "false") ? result = "" : "";
                var ValueClass = "";
                var OrganismClass = "";
                if (item.Organism.toLowerCase() == "true") {
                    ValueClass = "";
                    OrganismClass = 'checked="checked"';
                }
                else {
                    ValueClass = 'checked="checked"';
                    OrganismClass = "";
                }
                var rand = ClinicalLabResultDetail.getRandomNumber(true, 1000);
                var result = '';
                if (globalAppdata["isTransPubHealthAgCaseReporting"] && globalAppdata["isTransPubHealthAgCaseReporting"].toLowerCase() == "false") {
                    var options = '';
                    $.each(item.childResultAttribueModel, function (j, arr) {
                        options += `<option value="${arr.ResultName}">`;
                    });
                    var placeholder = "placeholder='- Select -'";
                    result = '<input ' + (options ? placeholder : '') + '  autocomplete="off" type="text" list="browsers' + rand + '" name="Result" id="txtResult' + item.LabOrderResultDetailId + '" value="' + item.Result + '"><datalist id="browsers' + rand + '">' + options + '</datalist>';
                }
                else {
                    result = "<input type='text' class='form-control' onblur='ClinicalLabResultDetail.validateResult(this,event)'  id='txtResult" + item.LabOrderResultDetailId + "'  name='Result'></input>";
                    result += ' <div class="col-sm-5"><input type="radio" ' + ValueClass + ' name="ResultType' + rand + '" id="ResultValue' + ClinicalLabResultDetail.getRandomNumber(true, 1000) + '" value="Value" onchange="ClinicalLabResultDetail.SelectResultRadio(1,this);"> <label class="control-label" style="font-size:11px;">Value</label> </div>' + ' <div class="col-sm-7" ><input type="radio" ' + OrganismClass + ' name="ResultType' + rand + '" id="ResultOrganism' + ClinicalLabResultDetail.getRandomNumber(true, 1000) + '" value="Organism" onchange="ClinicalLabResultDetail.SelectResultRadio(2,this);"> <label class="control-label"  style="font-size:11px;">Organism</label> </div>';
                }
                var UOM = "<input type='text' class='form-control' onkeypress='ClinicalLabResultDetail.validateUOM(event)' id='txtUoM" + item.LabOrderResultDetailId + "' name='UoM'></input>";
                var range = "<input type='text' class='form-control' onblur='ClinicalLabResultDetail.validateRange(this,event)' id='txtRange" + item.LabOrderResultDetailId + "' name='Range'></input>";
                var expandCollapseIcon = "";
                var gexpandCollapseIcon = '<a href="#" onclick="ClinicalLabResultDetail.GrandClidCollapseExpand(this)" title="Expand/Collapse Record"><i class="fa fa-minus-square red"></i></a>';

                if (item.ChildRows != null && item.ChildRows.length > 0) {
                    var expandCollapseIcon = '<a style="display:none" href="#" class="on-editing expand-row" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a>';
                }
                var cellpadding = '';
                if (secondStepChild != null && secondStepChild) {
                    cellpadding = ' style="padding-left:10px !important;background-color: #def!important;"';
                }

                var attrDescriptionText = '<textarea id="txtAttributeDescription"' + i + '"" class="form-control" spellcheck="true" name="Observation" onblur="ClinicalLabResultDetail.changeAttributeDescription(this, event);"></textarea>';

                var controlwidth = "size65";
                var Timecontrolwidth = "size45";
                var controldisabledClass = "";
                var calendars = '<div class="input-group ' + controlwidth + '"><span class="input-group-addon pl-tiny pr-tiny  xxs-font"><i class="fa fa-calendar"></i></span><input id="Child_dtpDateChild' + ClinicalLabResultDetail.getRandomNumber(true, 1000) + '" name="Date" ' + controldisabledClass + ' type="text" class="form-control ' + controlwidth + ' p-tiny" value="' + currentDate + '" /></div>';
                var Time = '<div class="input-group size50"><span class="input-group-addon pl-tiny pr-tiny  xxs-font"><i class="fa fa-clock-o"></i></span><input id="Child_tpTime' + ClinicalLabResultDetail.getRandomNumber(true, 1000) + '" name="Time" ' + controldisabledClass + ' type="text" class="form-control size60 p-tiny" value="' + currentTime + '" data-plugin-timepicker /></div>';

                var ResultNTETextTooltip = "";
                //For Result NTE if HL7 is received
                if (item.NTEText != null && item.NTEText != "") {
                    ResultNTETextTooltip = ' <i class="fa fa-exclamation ellip100" style="color: blue;padding-left:10px;" data-toggle="tooltip" data-placement="right" title="' + item.NTEText + '"></i> ';
                }


                if (item.IsAttribute == "True") {
                    $row.append('<td' + cellpadding + '>' + gexpandCollapseIcon + rowRemove + '&nbsp;' + expandCollapseIcon + '</td><td>' + calendars + '</td><td>' + Time + '</td><td' + cellpadding + '>' + item.Observation + '</td><td' + cellpadding + '>' + ddlConditionStatement + '</td><td' + cellpadding + '>' + result + '</td><td' + cellpadding + '>' + UOM + '</td><td' + cellpadding + '>' + ddlFlag + '</td><td' + cellpadding + '>' + range + '</td>');
                } else if (item.IsAttribute == "False") {
                    $row.append('<td' + cellpadding + '>' + gexpandCollapseIcon + rowRemove + '&nbsp;' + expandCollapseIcon + '</td><td>' + calendars + '</td><td>' + Time + '</td><td colspan="3" ' + cellpadding + '>' + attrDescriptionText + '</td><td' + cellpadding + '>' + ddlFlag + '</td><td' + cellpadding + '></td>');
                    setTimeout(function () { $('#txtAttributeDescription' + i).val(item.Observation); }, 300);
                } else {
                    $row.append('<td' + cellpadding + '>' + gexpandCollapseIcon + rowRemove + '&nbsp;' + expandCollapseIcon + '</td><td>' + calendars + '</td><td>' + Time + '</td><td' + cellpadding + '>' + item.Observation + Clinical_InfoButtonView.GenerateInfoLink(item.LOINC, "ClinicalLabResultDetail", 3) + ResultNTETextTooltip + '</td><td' + cellpadding + '>' + ddlConditionStatement + '</td><td' + cellpadding + '>' + result + '</td><td' + cellpadding + '>' + UOM + '</td><td' + cellpadding + '>' + ddlFlag + '</td><td' + cellpadding + '>' + range + '</td>');
                }

                //Bind JSON to the child item
                utility.bindMyJSONByName(true, item, false, $row).done(function () {
                    //Start 11-05-2016 Edit By Humaira Yousaf Bug# EMR-1021
                    $row.find('select').trigger('onchange');
                    //End 11-05-2016 Edit By Humaira Yousaf Bug# EMR-1021
                });

                // For Electronic Results disable the following controls

                if (item.IsElectronicResult == "True") {
                    $("#dgvLabResultDetail tbody tr").addClass('disableAll');
                    $row.addClass('disableAll');
                    //$("#chkMarkAsReviewed").attr('disabled', true);
                }



                if (item.ChildRows != null && item.ChildRows.length > 0) {


                    var childRows = ClinicalLabResultDetail.buildRowChild($row, item.ChildRows, true);
                    var gchildRow = ClinicalLabResultDetail.buildResultGrandChildRow(item.LabOrderResultDetailId, item, CurrentRow);
                    CurrentRowchilds = CurrentRowchilds.add($row);
                    if (gchildRow != null)
                        CurrentRowchilds = CurrentRowchilds.add(gchildRow);
                    CurrentRowchilds = CurrentRowchilds.add(childRows);
                } else {

                    CurrentRowchilds = CurrentRowchilds.add($row);
                    var gchildRow = ClinicalLabResultDetail.buildResultGrandChildRow(item.LabOrderResultDetailId, item, CurrentRow);
                    if (gchildRow != null)
                        CurrentRowchilds = CurrentRowchilds.add(gchildRow);
                }

            });
            //   }
        }
        return CurrentRowchilds;
    },
    SelectResultRadio: function (type, obj) {
        if (type == 2) {

            $(obj).parent().parent().find('input[id*="txtResult"]').attr('oninput', 'ClinicalLabResultDetail.bindAutoCompleteOrganisms(this)')
        }
        else {
            $(obj).parent().parent().find('input[id*="txtResult"]').autocomplete("disable");
        }
    },
    //For adding new chid rows
    addNewResultChildRow: function (currentRow, objJson, isAttribute, attributeValue, isDescription, descriptionValue, UoM, Range, LabTestId, LabTestAttributeId, TestRow, attributeResultJSON) {

        if (!ClinicalLabResultDetail.isResultExists(currentRow, objJson)) {
            var currentDate = new Date();
            var observationVal = "";
            if (objJson != null)
                observationVal = objJson["Observation"];
            if (isAttribute) {
                if (isDescription) {
                    observationVal = descriptionValue;
                } else {
                    observationVal = attributeValue;
                }
            }
            var currentTime = currentDate.toLocaleTimeString().replace(/([\d]+:[\d]{2})(:[\d]{2})(.*)/, "$1$3");
            currentDate = currentDate.toLocaleDateString().replace(/[^A-Za-z 0-9 \.,\?""!@#\$%\^&\*\(\)-_=\+;:<>\/\\\|\}\{\[\]`~]*/g, '');



            var flagOptions = "<option value='Abnormally High'>Abnormally High</option>" +
                                "<option value='High'>High</option>" +
                                "<option value='Negative'>Negative</option>" +
                                "<option value='Positive'>Positive</option>" +
                                "<option value='Normal' selected>Normal</option>" +
                                "<option value='Low'>Low</option>" +
                                "<option value='Abnormally Low'>Abnormally Low</option>";
            // if ($(TestRow).attr("Organism") == "Enterococcus" && $(TestRow).attr("Antimicrobials") && ($(TestRow).attr("Antimicrobials").indexOf("Gentamicin") >= 0 || $(TestRow).attr("Antimicrobials").indexOf("Streptomycin") >= 0)) {
            globalAppdata["isTransPubHealthAgAntimicobialUse"] && globalAppdata["isTransPubHealthAgAntimicobialUse"].toLowerCase() == "true" ?
            flagOptions +=
                "<option value='S= Susceptible/Synergistic'>S= Susceptible/Synergistic</option>" +
                "<option value='R= Resistant/Non Synergistic'>R= Resistant/Non Synergistic</option>" +
                "<option value='S=Susceptible'>S=Susceptible</option>" +
                "<option value='S-DD=Susceptible-Dose Dependent'>S-DD=Susceptible-Dose Dependent</option>" +
                "<option value='I=Intermediate'>I=Intermediate</option>" +
                "<option value='R=Resistant'>R=Resistant</option>" +
                "<option value='NS=Non-Susceptible'>NS=Non-Susceptible</option>" +
                "<option value='N=Not Tested'>N=Not Tested</option>" : "";



            var conditionStatementOptions = "<option value='>'>Greater Than</option>" +
                                  "<option value='>='>Greater Than or equal to</option>" +
                                  "<option value='<'>Less Than</option>" +
                                  "<option value='<='>Less Than or equal to</option>" +
                                  "<option value='='>Exactly Equal to</option>";

            var currentRowchild = $();

            var LOINICCODE = "";
            var LOINCDescription = "";
            if (objJson != null) {
                LOINICCODE = objJson['LOINICCODE'];
                LOINCDescription = objJson['LOINICDescription'];
            } else {
                if (isDescription) {
                    LOINICCODE = "";
                    LOINCDescription = descriptionValue;
                } else {
                    LOINICCODE = "";
                    LOINCDescription = attributeValue;
                }
            }

            var $row = $('<tr/>').addClass("childRow-bg").attr('child-id', ClinicalLabResultDetail.getRandomNumber(true, 1000));
            //push value as attribute into child row
            $row.attr('LOINICCODE', LOINICCODE);
            $row.attr('LOINICDescription', LOINCDescription);
            $row.attr("LabOrderResultId", ClinicalLabResultDetail.getRandomNumber(true, 1000));
            //Start 11-05-2016 Edit By Humaira Yousaf Bug# EMR-1021
            var ddlFlag = "<select onchange='ClinicalLabResultDetail.highlightResultValue(this);' id='ddlFlag' name='Flag' class='form-control'>" + flagOptions + "</select>";
            var ddlConditionStatement = "<select id='ddlConditionStatement" + ClinicalLabResultDetail.getRandomNumber(true, 1000) + "' name='ConditionStatement' class='form-control'>" + conditionStatementOptions + "</select>";
            //End 11-05-2016 Edit By Humaira Yousaf Bug# EMR-1021
            (globalAppdata["isTransPubHealthAgAntimicobialUse"] && globalAppdata["isTransPubHealthAgAntimicobialUse"].toLowerCase() == "false") ? ddlConditionStatement = "" : "";
            if (isAttribute) {

                var range = "<input type='text' value='" + Range + "' class='form-control' onblur='ClinicalLabResultDetail.validateRange(this,event)'id='txtRange' name='Range'></input>";
                var UOM = "<input type='text' value='" + UoM + "' class='form-control' onkeypress='ClinicalLabResultDetail.validateUOM(event)' id='txtUoM' name='UoM'></input>";
            } else {
                var range = "<input type='text' class='form-control' onblur='ClinicalLabResultDetail.validateRange(this,event)'id='txtRange' name='Range'></input>";
                var UOM = "<input type='text' class='form-control' onkeypress='ClinicalLabResultDetail.validateUOM(event)' id='txtUoM' name='UoM'></input>";
            }
            var result = '';
            var rand = ClinicalLabResultDetail.getRandomNumber(true, 1000);
            if (globalAppdata["isTransPubHealthAgCaseReporting"] && globalAppdata["isTransPubHealthAgCaseReporting"].toLowerCase() == "false") {
                var options = '';
                if (isAttribute) {
                    if (LabTestAttributeId && attributeResultJSON.length > 0) {
                        var returnedArray = $.grep(attributeResultJSON, function (item) {
                            return item.LabTestAttributeId == LabTestAttributeId;
                        });
                        if (returnedArray.length > 0) {
                            $.each(returnedArray, function (j, arr) {
                                options += "<option value='" + arr.ResultName + "'>";
                            });
                        }
                    }
                }
                var placeholder = "placeholder='- Select -'";
                result = '<input ' + (options ? placeholder : '') + '  autocomplete="off" type="text" list="browsers' + rand + '" name="Result" id="txtResult"><datalist id="browsers' + rand + '">' + options + '</datalist>';
            }
            else {
                result = "<input type='text' class='form-control' onblur='ClinicalLabResultDetail.validateResult(this,event)' id='txtResult'  name='Result'></input>";
                result += ' <div class="col-sm-5"><input type="radio" checked="checked" name="ResultType' + rand + '" id="ResultValue' + ClinicalLabResultDetail.getRandomNumber(true, 1000) + '" value="Value" onchange="ClinicalLabResultDetail.SelectResultRadio(1,this);"> <label class="control-label" style="font-size:11px;">Value</label> </div>' + '<div class="col-sm-7" ><input type="radio" name="ResultType' + rand + '" id="ResultOrganism' + ClinicalLabResultDetail.getRandomNumber(true, 1000) + '" value="Organism" onchange="ClinicalLabResultDetail.SelectResultRadio(2,this);"> <label class="control-label"  style="font-size:11px;">Organism</label> </div>';
            }


            var Observation = "<label class='control-label' name='Observation'>" + observationVal + "</label>";
            var rowRemove = '<a href="#" class="btn-xs on-default remove-row mr-none btn" title="Delete Record"><i class="fa fa-close red"></i></a>';
            var expandCollapseIcon = '<a href="#" onclick="ClinicalLabResultDetail.GrandClidCollapseExpand(this)" title="Expand/Collapse Record"><i class="fa fa-minus-square red"></i></a>';


            //var attrDescriptionText = '<input type="text" id="" value="' + descriptionValue + '" class="form-control"   name="Observation"></input>';
            var attrDescriptionText = '<textarea id="txtAttributeDescription" spellcheck="true" class="form-control" name="Observation" onblur="ClinicalLabResultDetail.changeAttributeDescription(this, event);"></textarea>';

            var controlwidth = "size65";
            var Timecontrolwidth = "size45";
            var controldisabledClass = "";
            var Id = ClinicalLabResultDetail.getRandomNumber(true, 1000);
            var calendars = '<div class="input-group ' + controlwidth + '"><span class="input-group-addon pl-tiny pr-tiny  xxs-font"><i class="fa fa-calendar"></i></span><input id="Child_dtpDateChild' + Id + '" name="Date" ' + controldisabledClass + ' type="text" class="form-control ' + controlwidth + ' p-tiny" value="' + currentDate + '" /></div>';
            var Time = '<div class="input-group size50"><span class="input-group-addon pl-tiny pr-tiny  xxs-font"><i class="fa fa-clock-o"></i></span><input id="Child_tpTime' + Id + '" name="Time" ' + controldisabledClass + ' type="text" class="form-control size60 p-tiny" value="' + currentTime + '" data-plugin-timepicker /></div>';


            if (isAttribute) {
                if (isDescription) {
                    $row.append('<td>' + expandCollapseIcon + rowRemove + '</td><td>' + calendars + '</td><td>' + Time + '</td><td colspan="3">' + attrDescriptionText + '</td><td>' + ddlFlag + '</td><td></td>');
                    setTimeout(function () { $('#txtAttributeDescription').val(descriptionValue); }, 300);
                    $row.attr('IsAttribute', 'False');
                    $row.attr('LabTestId', '');
                    $row.attr('LabTestAttributeId', '');

                } else {
                    $row.append('<td>' + expandCollapseIcon + rowRemove + '</td><td>' + calendars + '</td><td>' + Time + '</td><td>' + Observation + '</td><td>' + ddlConditionStatement + '</td><td>' + result + '</td><td>' + UOM + '</td><td>' + ddlFlag + '</td><td>' + range + '</td>');
                    $row.attr('IsAttribute', 'True');
                    $row.attr('LabTestId', LabTestId);
                    $row.attr('LabTestAttributeId', LabTestAttributeId);
                }
            } else {
                $row.append('<td>' + expandCollapseIcon + rowRemove + '</td><td>' + calendars + '</td><td>' + Time + '</td><td>' + Observation + Clinical_InfoButtonView.GenerateInfoLink(LOINICCODE, "ClinicalLabResultDetail", 3) + '</td><td>' + ddlConditionStatement + '</td><td>' + result + '</td><td>' + UOM + '</td><td>' + ddlFlag + '</td><td>' + range + '</td>');
                $row.attr('IsAttribute', '');
                $row.attr('LabTestId', '');
                $row.attr('LabTestAttributeId', '');
            }

            $row.find('select').trigger('onchange');
            //End 11-05-2016 Edit By Humaira Yousaf Bug# EMR-1021
            return currentRowchild = currentRowchild.add($row);
        }
        return null;
    },

    GrandClidCollapseExpand: function (obj) {

        if ($(obj).find("i").hasClass("fa-plus-square")) {
            $(obj).find("i").removeClass("fa-plus-square green");
            $(obj).find("i").addClass("fa-minus-square red");
            $(obj).parent().parent().next("tr").show();
        }
        else {
            $(obj).find("i").removeClass("fa-minus-square red");
            $(obj).find("i").addClass("fa-plus-square green");
            $(obj).parent().parent().next("tr").hide();
        }
    },

    buildResultGrandChildRow: function (parentId, item, $row) {

        //Grand child row
        var gchild_id = ClinicalLabResultDetail.getRandomNumber(true, 1000);
        var $grow = $('<tr/>').addClass("childRow-bg").attr('gchild-id', gchild_id);
        $grow.attr("gparent-id", parentId);
        var RefrenceRangeOptions = "<option value=''>- Select -</option>" +
                            "<option value='Normal'>Normal</option>" +
                            "<option value='Negative'>Negative</option>" +
                            "<option value='Positive'>Positive</option>" +
                            "<option value='High'>High</option>" +
                            "<option value='Abnormal'>Abnormal</option>" +
                            "<option value='Abnormally High'>Abnormally High</option>" +
                            "<option value='Low'>Low</option>" +
                            "<option value='Susceptible.Indicates for microbiology susceptibilities only'>Susceptible.Indicates for microbiology susceptibilities only</option>" +
                            "<option value='Abnormally Low'>Abnormally Low</option>";

        var RefrenceRangeDescription = '<textarea id="txtReferenceRangeDescription" spellcheck="true" class="form-control" name="ReferenceRangeDescription"></textarea>';
        var ddlReferenceRange = "<select onchange='ClinicalLabResultDetail.highlightResultValue(this);' id='ddlReferenceRangeInterpration' name='ReferenceRangeInterpration' class='form-control'>" + RefrenceRangeOptions + "</select>";
        var TestAntimicrobialOptions = "<option value=''>- Select -</option>";
        if ($($row).attr("AntimicrobialIds")) {
            $.each($($row).attr("AntimicrobialIds").split(","), function (i, item) {

                TestAntimicrobialOptions += "<option value='" + item + "'>" + $($row).attr("Antimicrobials").split(",")[i] + "</option>";
            });


        }
        var ddlTestAntimicrobial = "<select id='ddlTestAntimicrobial' name='TestAntimicrobial' class='form-control'>" + TestAntimicrobialOptions + "</select>";


        $grow.append('<td colspan="4" style="padding-left: 50px !important;padding-right: 80px !important;>' + (
                        (globalAppdata["isTransPubHealthAgCaseReporting"] && globalAppdata["isTransPubHealthAgCaseReporting"].toLowerCase() == "false") ? "" :
                        '<label class="control-label"> Reference Range Interpretation</label>' +
                         ddlReferenceRange) +
                        ((globalAppdata["isTransPubHealthAgAntimicobialUse"] && globalAppdata["isTransPubHealthAgAntimicobialUse"].toLowerCase() == "false") ? "" :
                        '<label class="control-label"> Antimicrobial</label>' +
                         ddlTestAntimicrobial) +
                        '</td>' +
                        ((globalAppdata["isTransPubHealthAgCaseReporting"] && globalAppdata["isTransPubHealthAgCaseReporting"].toLowerCase() == "false") ? "" :
                        '<td colspan="5" style="padding-left: 30px !important;padding-right: 30px !important;"><label class="control-label"> Reference Range Description</label>' +
                         RefrenceRangeDescription + '</td>'));

        if (item) {
            if (item.ReferenceRangeDescription)
                $grow.find("#txtReferenceRangeDescription").val(item.ReferenceRangeDescription);
            if (item.ReferenceRangeInterpration)
                $grow.find("#ddlReferenceRangeInterpration").val(item.ReferenceRangeInterpration);
            if (item.TestAntimicrobial)
                $grow.find("#ddlTestAntimicrobial").val(item.TestAntimicrobial);
        }
        return $grow;
    },

    addNewResultGrandChildRow: function (parentId, currentRow, objJson, item, $row) {


        var currentRowchild = $();
        //Grand child row
        var $grow = ClinicalLabResultDetail.buildResultGrandChildRow(parentId, item, $row);
        //End 11-05-2016 Edit By Humaira Yousaf Bug# EMR-1021
        return currentRowchild = currentRowchild.add($grow);
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

    rowAddChild: function ($row, obj) {

        var _self = obj;
        var row = _self.datatable.row($row);
        var childRow = ClinicalLabResultDetail.openLOINCSearch(row, $row);
    },

    rowRemove: function ($row, obj) {
        utility.myConfirm('1', function () {

            var _self = obj;
            //Logic For removing child row from datatable
            if ($row.hasClass('childRow-bg')) {

                var childId = $row.attr("child-id");
                ClinicalLabResultDetail.deletedChildRows.push(childId);
                //Find parent row of the current child
                var rows = $row.prevAll();
                var parentHtmlRow;
                $(rows).each(function () {

                    if ($(this).hasClass('parent-row')) {

                        parentHtmlRow = $(this);
                        return false;
                    }

                });

                var dtbParentRow = _self.datatable.row(parentHtmlRow.get(0));
                var htmlChilds = dtbParentRow.child();

                var childs = $();
                var child_ids = [];
                $(htmlChilds).each(function () {
                    if ($(this).attr('gchild-id') && child_ids.indexOf("" + $(this).attr('gparent-id')) >= 0) {
                        childs = childs.add($(this));
                    }
                    else if ($(this).attr('child-id') && $(this).attr('child-id') != $row.attr('child-id')) {
                        childs = childs.add($(this));
                        child_ids.push($(this).attr('child-id'));
                    }


                });

                //Push filtered childs
                if (childs.length > 0)
                    dtbParentRow.child(childs);
                else {
                    //Remove All childs and convert the 'minus' icon to 'plus'
                    $(parentHtmlRow).find("td:first .fa-minus-square").attr("class", "fa fa-plus-square");
                    $(parentHtmlRow).find("td:first").find('a').hide();
                    //Push Empty Childs context
                    dtbParentRow.child(childs);
                }

            }
            ClinicalLabResultDetail.enableDisableOrderResultButtons();
            ClinicalLabResultDetail.deleteLabResultsDetail_DbCall(childId).done(function (response) {

                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages("Successfully Deleted", 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

            ClinicalLabResultDetail.SetDateTimeControl($("#" + ClinicalLabResultDetail.params.PanelID + " #dgvLabResultDetail tbody"), true);
        }, function () {
        },
                    '1'
    );

    },

    enableRemoveRow: function (CurrentRow) {
        CurrentRow.find("td.actions .remove-row").removeClass("hidden");

    },

    //End//---------- Grid Data -----------

    getResultRejectSpecimenTitle: function (resultSpecimens) {

        var details = "";
        var newLineCharCode = '<br/>';
        var colon = ":";
        var space = " ";
        $.each(resultSpecimens, function (index, resultSpecimen) {

            //For reject Result Reject Reasons
            if (resultSpecimen.ResultSpecimenRejectReasons.length > 0) {

                details += "Specimen Type" + colon + space + resultSpecimen.SpecimenType + newLineCharCode;

                $.each(resultSpecimen.ResultSpecimenRejectReasons, function (index, resultSpecimenRejectReason) {

                    details += "Identifier" + colon + space + resultSpecimenRejectReason.Identifer + newLineCharCode;
                    details += "Text" + colon + space + resultSpecimenRejectReason.Text + newLineCharCode;
                    details += "Name Of Coding System" + colon + space + resultSpecimenRejectReason.NameOfCodingSystem + newLineCharCode;
                    details += "Alternate Identifier" + colon + space + resultSpecimenRejectReason.AlternateIdentifier + newLineCharCode;
                    details += "Alternate Text" + colon + space + resultSpecimenRejectReason.AlternateText + newLineCharCode;
                    details += "Name Of Aletrnate Coding System" + colon + space + resultSpecimenRejectReason.NameOfAletrnateCodingSystem + newLineCharCode;
                    details += "Original Text" + colon + space + resultSpecimenRejectReason.OriginalText + newLineCharCode;

                    //End Of Items
                    if (index == resultSpecimen.ResultSpecimenRejectReasons.length - 1) {
                        details += newLineCharCode + newLineCharCode;
                    }

                });
            }
        });
        return details;
    },

    //----------Db Calls--------------

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Db Call for loading LabResults
    loadLabResults_DbCall: function (labResultId, labOrderId) {

        if (labResultId < 0) {
            labResultId = 0;
        }
        if (labOrderId < 0) {

            labOrderId = 0;
        }
        var objData = {};

        objData["LabResultId"] = labResultId;
        objData["LabOrderId"] = labOrderId;

        // objData["PatientId"] = Clinical_LabOrder.patientId;

        objData["commandType"] = "fill_labresult";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },

    deleteLabResultsDetail_DbCall: function (labResultDetailId) {

        if (labResultDetailId < 0) {
            labResultDetailId = 0;
        }

        var objData = {};

        objData["LabOrderResultDetailId"] = labResultDetailId;

        objData["commandType"] = "delete_labresultdetail";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Saves LabResult
    //Params: LabResultData
    saveLabResult: function (LabResultData) {
        var objData = JSON.parse(LabResultData);
        //if (objData.Comments) {
        //    var commentlength = objData.Comments.length;
        //    if (commentlength > 2000) {
        //        objData.Comments = objData.Comments.substring(0, 2000);
        //    }
        //}
        if (objData["Assignee"] == "")
            objData["AssigneeId"] = "";
        objData["commandType"] = "save_LabResult";
        if (ClinicalLabResultDetail.params.ParentCtrlPanelID == "pnlClinicalProgressNote #pnlClinicalLabOrder") {
            objData["NoteId"] = $("#pnlClinicalProgressNote #hfNoteId").val();
        } else {
            objData["NoteId"] = "";
        }
        objData["PatientFullName"] = $('#PatientProfile #hfPatientFullNameOnly').val();
        objData["PracticeId"] = $("#PatientProfile #hfPatientPracticeId").val();
        objData["PatientFacilityId"] = $('#PatientProfile #hfPatientFacilityId').val();

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },

    //----------Db Calls--------------


    //Function Name: loadAttachments
    //Author: Humaira Yousaf
    //Date :  27-04-2016
    //Description: Loads Attachments
    loadAttachments: function () {

        ClinicalLabResultDetail.loadAttachments_DbCall().done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {

                var ulAttachment = $('#' + ClinicalLabResultDetail.params.PanelID + ' #menuViewAttachment');
                $(ulAttachment).empty();
                if (response.OrderResultAttachmentCount > 0) {
                    var attachments = JSON.parse(response.OrderResultAttachmentLoad_JSON);

                    $(attachments).each(function (index, item) {
                        $(ulAttachment).append('<li><a href="#" id="' + item.PatDocId + '" onclick="ClinicalLabResultDetail.showAttachment(\'' + item.PatDocId + '\',event)">' + item.ModifiedOn + '</a></li>');
                    });

                }
                else {
                    $(ulAttachment).append('<li><a href="#">No Attachment Found</a></li>');
                }

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    //Function Name: loadAttachments_DbCall
    //Author: Humaira Yousaf
    //Date :  27-04-2016
    //Description: DbCall to Loads Attachments
    loadAttachments_DbCall: function () {

        var objData = {};
        var labResultId = $('#' + ClinicalLabResultDetail.params.PanelID + " #hfLabOrderId").val();
        // objData["TranisitionId"] = labResultId == null || labResultId == "" ? -1 : labResultId;
        objData["TranisitionId"] = ClinicalLabResultDetail.params.LabResultId == null || ClinicalLabResultDetail.params.LabResultId == "" ? -1 : ClinicalLabResultDetail.params.LabResultId
        objData["RefModuleName"] = "Lab Result";
        objData["PatientId"] = Clinical_LabOrder.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $("div#PatientProfile #hfPatientId").val();

        objData["commandType"] = "load_attachments";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },

    //Function Name: showAttachment
    //Author: Humaira Yousaf
    //Date :  28-04-2016
    //Description: shows Attachments
    showAttachment: function (PatDocID, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Documents", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["PatientID"] = Clinical_LabOrder.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
                params["PatDocID"] = PatDocID;
                params["mode"] = "Edit";
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = "ClinicalLabResultDetail";
                params["ParentCtrlPanelID"] = "pnlClinicalLabResultDetail";
                LoadActionPan('Document_Viewer', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    //Function Name: viewPdfLabResult
    //Author: Humaira Yousaf
    //Date :  06-05-2016
    //Description: shows Attachments
    viewPdfLabResult: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["UserId"] = globalAppdata['AppUserId'];
        params["PatientId"] = Clinical_LabOrder.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        //Start 11-05-2016 Edit By Humaira Yousaf Bug# EMR-1023
        params["ParentCtrl"] = "ClinicalLabResultDetail";
        //End 11-05-2016 Edit By Humaira Yousaf Bug# EMR-1023
        params["LabOrderId"] = ClinicalLabResultDetail.params.LabOrderId;
        params["LabResultId"] = $('#' + ClinicalLabResultDetail.params.PanelID + " #hfLabResultId").val();
        params["Caller"] = "viewpdf";
        LoadActionPan('Clinical_LabResultView', params);

    },

    //Author: Ahmad Raza
    //Date :  21-06-2016
    //Function Name: validateUOM
    //Description: This function will validate the UOM Field
    validateUOM: function (event) {

        if (navigator.userAgent.search("Firefox") > -1) {
            if (event.keyCode == 8 || event.keyCode == 46 || event.keyCode == 37 || event.keyCode == 39 || event.key == "Tab") return true;
        }

        var code = event.keyCode || event.charCode;
        var test = /^[a-zA-Z!”@#$%&’()*\+,\/;\[\\\]\^_`{|}~]+$/;
        var value = String.fromCharCode(code);
        if (value.match(test)) {
            return true;
        }
        else {
            event.preventDefault();
            return false;

        }
    },

    validateRange: function (obj, event) {
        //  Regex changed by Abid Ali
        var test = /^[0-9]+\-[0-9]+$/;
        var value = $(obj).val(); //String.fromCharCode(event.keyCode);
        if (value != "") {

            if (value.match(test)) {
                var rangeValues = value.split('-'); //split on hypen char
                var lowerRange = parseFloat(rangeValues[0].trim());
                var upperRange = parseFloat(rangeValues[1].trim());

                if (lowerRange >= upperRange) {
                    //event.preventDefault();
                    $(obj).val(""); //emtpy range value if does not match the pattern
                    setTimeout(function () {
                        utility.DisplayMessages("Please Enter Range in proper format Like (Lower bound - upper bound)", 3);
                    }, 100)
                    $(obj).css('border-color', 'red');
                    ClinicalLabResultDetail.isError = true;
                    return false;
                }
                else {
                    var $row = $(obj).parent().parent();
                    $row.find('input[id*="txtResult"]').trigger('blur'); // dynamically validate result
                    ClinicalLabResultDetail.isError = false;
                    $(obj).css({ 'border-color': '#ccc' });
                    return true;
                }
            }
            else {
                ////event.preventDefault();
                //$(obj).val(""); //emtpy range value if does not match the pattern
                //setTimeout(function () {
                //    utility.DisplayMessages("Please Enter Range in proper format Like (0-9)", 3);
                //}, 100);
                //$(obj).css('border-color', 'red');
                //ClinicalLabResultDetail.isError = true;
                //// return false;
                ClinicalLabResultDetail.isError = false;
                $(obj).css({ 'border-color': '#ccc' });
            }
        }
        else {
            ClinicalLabResultDetail.isError = false;
            $(obj).css({ 'border-color': '#ccc' });
        }

        return true;
    },

    //Edited by Abid Ali
    //Date :  07-12-2016
    //Description: This function will allow only digits and hypen in text box
    checkRange: function (event) {
        if (navigator.userAgent.search("Firefox") > -1 || event.charCode == 46) {
            if (event.keyCode == 8 || event.keyCode == 46 || event.keyCode == 37 || event.keyCode == 39 || event.key == "Tab" || event.charCode == 46) return true;
        }
        var code = event.keyCode || event.charCode;
        var test = /^[0-9]|\-$/;
        var value = String.fromCharCode(code);
        if (value != "" && value.match(test)) {
            return true;
        }
        else {
            event.preventDefault();
            // $(obj).val(""); //emtpy range value if does not match the pattern
            return false;
        }
    },

    //Author: Abid Ali
    //Date :  21-06-2016
    //Function Name: validateResult
    //Description: This function will validate the result Field
    validateResult: function (obj, event) {

        var $row = $(obj).parent().parent();
        var rangeValue = "";
        var resultValue = $(obj).val().trim();

        if (resultValue != "") {
            if ($row != null) {
                rangeValue = $row.find('input[id*="txtRange"]').val();
                if ($.isNumeric(resultValue) == true) {
                    resultValue = parseFloat(resultValue);  // if numeric valued result is entered - validate
                    if (rangeValue != "") {
                        var rangeValues = rangeValue.split('-'); //split on hypen char

                        if (($.isNumeric(rangeValues[0].trim()) == true && $.isNumeric(rangeValues[1].trim()) == true) && (rangeValues.length == 2)) {
                            var lowerRange = parseFloat(rangeValues[0].trim());
                            var upperRange = parseFloat(rangeValues[1].trim());
                            if (resultValue < lowerRange || resultValue > upperRange) {

                                // event.preventDefault();
                                $(obj).val(""); // empty result value
                                setTimeout(function () {
                                    utility.DisplayMessages("The Result Value falls outside the Range", 3);
                                }, 100);
                                ClinicalLabResultDetail.isError = true;
                                $(obj).css({ 'border-color': 'red' });
                                // return false;
                            }
                            else {
                                ClinicalLabResultDetail.isError = false;
                                $(obj).css({ 'border-color': '#ccc' });
                            }
                        }
                    }
                }
                else {
                    ClinicalLabResultDetail.isError = false;
                    $(obj).css({ 'border-color': '#ccc' });
                }
            }
        }
        return true;
    },

    //Function Name: highlightResultValue
    //Author: Humaira Yousaf
    //Date :  11-05-2016
    //Description: Highlights result values
    highlightResultValue: function (obj) {

        var $tr = $(obj).parent().parent();
        flagValue = $tr.find('select[id *= "ddlFlag"]').val().toLowerCase();
        var $result = $tr.find('input[id *= "txtResult"]');

        switch (flagValue) {

            case 'normal':
                $result.css("color", "green");
                break;
            case 'abnormally high':
                $result.css("color", "red");
                break;
            case 'high':
                $result.css("color", "red");
                break;
            case 'abnormally low':
                $result.css("color", "orange");
                break;
            case 'low':
                $result.css("color", "orange");
                break;
            default:
                $result.css("color", "black");
        }
    },

    allowNumbersOnly: function (event) {

        if (navigator.userAgent.search("Firefox") > -1 || event.charCode == 46) {
            if (event.keyCode == 8 || event.keyCode == 46 || event.keyCode == 37 || event.keyCode == 39 || event.key == "Tab" || event.charCode == 46) return true;
        }

        var code = event.keyCode || event.charCode;
        var test = /^[0-9]$/;
        var value = String.fromCharCode(code);
        if (value != "" && value.match(test)) {
            return true;
        }
        else {
            event.preventDefault();
            return false;
        }
    },

    // Start Result Attributes functions


    loadLabTest_DBCall: function (LabId, LOINICCODE, LOINCDescription) {
        var objData = new Object();
        objData["LabId"] = LabId;
        objData["LOINICCODE"] = LOINICCODE;
        objData["LOINICDescription"] = LOINCDescription;
        objData["commandType"] = "load_clinical_lab_test_and_attributes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIServiceSync(data, "ClinicalLab", "ClinicalLabTest");
    },


    changeAttributeDescription: function (obj, event) {

        var $row = $(obj).parent().parent();
        var rangeValue = "";
        var resultValue = $(obj).val().trim();

        $($row).attr("LOINICDescription", $(obj).val().trim());
    },

    // End Result Attributes functions

    AddNewFinalInterpretation: function (FinalInterpretation) {

        var self = $('#' + ClinicalLabResultDetail.params.PanelID + ' #divResultInformation');
        var row = self.find('#divFinalInterpretationTemplate').find("tbody").clone();
        var table = self.find('#dgvFinalInterpretation');

        table.append(row);

        if (FinalInterpretation) {
            row.find("#ddlTestAntimicrobial").val(FinalInterpretation.split("|")[0]);

            ClinicalLabResultDetail.onAntimicrobialChange(row.find("#ddlTestAntimicrobial"));


            row.find("#ddlFinalInterpretation").val(FinalInterpretation.split("|")[1]);
        } else {
            //validate duplication only if it is being added manually
            ClinicalLabResultDetail.onAntimicrobialChange(row.find("#ddlTestAntimicrobial"));
        }
    },
    onAntimicrobialChange: function (obj) {

        var currentAntimicrobialValue = $(obj).val();

        var self = $('#' + ClinicalLabResultDetail.params.PanelID + ' #divResultInformation');
        var table = self.find('#dgvFinalInterpretation');


        if (ClinicalLabResultDetail.isEnterococcus && ($(obj).find("option:selected").text() == "Gentamicin" || $(obj).find("option:selected").text() == "Streptomycin")) {
            $(obj).closest("tr").find("#ddlFinalInterpretation").empty().append("<option value='S= Susceptible/Synergistic'>S= Susceptible/Synergistic</option>" +
                     "<option value='R= Resistant/Non Synergistic'>R= Resistant/Non Synergistic</option>");
        }
        else {

            $(obj).closest("tr").find("#ddlFinalInterpretation").empty().append("<option value='S=Susceptible'>S=Susceptible</option>" +
                      "<option value='S-DD=Susceptible-Dose Dependent'>S-DD=Susceptible-Dose Dependent</option>" +
                      "<option value='I=Intermediate'>I=Intermediate</option>" +
                      "<option value='R=Resistant'>R=Resistant</option>" +
                      "<option value='NS=Non-Susceptible'>NS=Non-Susceptible</option>" +
                      "<option value='N=Not Tested'>N=Not Tested</option>");
        }




        $(table).find("tr").not($(obj).closest("tr")).each(function (i, row) {
            if ($(row).find("#ddlTestAntimicrobial").val() == currentAntimicrobialValue) {

                utility.DisplayMessages("Antimicrobial already Exists", 3);
                $(obj).val("");
                return false;
            }
        });


    },
    deleteFinalInterpretation: function (obj, ev) {
        ev.stopPropagation();
        //   var questionnaireId = $(obj).parent().attr('id');
        // if ($(obj).parent().hasClass('fromDB')) {
        utility.myConfirm('1', function () {
            $(obj).closest("tr").remove();
        }, function () {

        },
             '1'
         );

        //} else {
        //    $(obj).parent().remove();



        //}

    },

    getFinalInterpretationjson: function () {

        var arr = [];

        var self = $('#' + ClinicalLabResultDetail.params.PanelID + ' #divResultInformation');
        var table = self.find('#dgvFinalInterpretation');

        $(table).find("tr").each(function (i, row) {
            if ($(row).find("#ddlTestAntimicrobial").val() && $(row).find("#ddlFinalInterpretation").val())
                arr.push($(row).find("#ddlTestAntimicrobial").val() + "|" + $(row).find("#ddlFinalInterpretation").val())
        });
        return arr.toString();
    },

}
