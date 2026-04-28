ClinicalRadiologyResultDetail = {
    params: [],
    bIsFirstLoad: true,
    EditableGrid: null,
    myGrid: null,
    RadiologyResultNewParnetRows: [],
    rowsParentChild: [],
    deletedChildRows: [],
    isSaved: false,
    isError: false,
    bNextPrev: false,
    controlToInvoke: null,
    loadProviderFromAppData: false,
    loadFacilityFromAppData: false,
    ProviderOnDemandLoad: false,
    FacilityOnDemandLoaded: false,
    isProviderLoaded: false,
    isFacilityLoaded: false,
    FavListName: "RadiologyResultDetail",
    Load: function (params) {

        BackgroundLoaderShow(true);

        ClinicalRadiologyResultDetail.params = params;

        if (ClinicalRadiologyResultDetail.params.PanelID != 'pnlClinicalRadiologyResultDetail') {
            ClinicalRadiologyResultDetail.params.PanelID = ClinicalRadiologyResultDetail.params.PanelID + ' #pnlClinicalRadiologyResultDetail';
        } else {
            ClinicalRadiologyResultDetail.params.PanelID = 'pnlClinicalRadiologyResultDetail';
        }
        var RadiologyOrderId = ClinicalRadiologyResultDetail.params.RadiologyOrderId;
        if (RadiologyOrderId != null && RadiologyOrderId > 0) {
            //$('#' + ClinicalRadiologyResultDetail.params.PanelID + " #btnScanResult,#btnViewAttachment,#btnViewPDF").removeClass("hidden");
        }
        if (ClinicalRadiologyResultDetail.params.FromRadiologyDetail || ClinicalRadiologyResultDetail.params.mode == "Edit") {
            var $TestField = $('#pnlClinicalRadiologyResultDetail #txtLabCPTCode');
            $TestField.addClass('disableAll');
        }
        //Start//Abid Ali // For bug# EMR-896
        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #divRadiologyOrderInformation').hide();
        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #divRadiologyBillingInformation').hide();
        //End//Abid Ali // For bug# EMR-896


        var self = $('#' + ClinicalRadiologyResultDetail.params.PanelID);


        if (EMRUtility.getFavListStatus(ClinicalRadiologyResultDetail.FavListName))
            $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail #favSectionDiv").removeClass("toggled");
        else
            $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail #favSectionDiv").addClass("toggled");

        //For opening result detail page in edit mode
        if (ClinicalRadiologyResultDetail.params.mode == "Edit") {

            var RadiologyResultId = ClinicalRadiologyResultDetail.params.RadiologyResultId;
            var RadiologyOrderId = ClinicalRadiologyResultDetail.params.RadiologyOrderId;

            ClinicalRadiologyResultDetail.showHideEditableControls(true);

            ClinicalRadiologyResultDetail.fillRadiologyResult(RadiologyResultId, RadiologyOrderId);

            $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #hfRadiologyResultId").val(ClinicalRadiologyResultDetail.params.RadiologyResultId);
            ClinicalRadiologyResultDetail.validateRadiologyResultDetail();
        }


        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail #hfPatientId").val($("div#PatientProfile #hfPatientId").val());
        // $("#" + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail #btnSaveOrder,#btnSignPrintOrder,#btnSignOrder,#btnResetRadiologyResult").addClass('disableAll');


        self.find('#btnViewAttachment').dropdown();

        if (ClinicalRadiologyResultDetail.bIsFirstLoad == true) {

            ClinicalRadiologyResultDetail.bIsFirstLoad = false;

            if (ClinicalRadiologyResultDetail.params.mode == "Add") {
                // Populate dropdowns
                self.loadDropDowns(true).done(function () {
                    $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #ddlStatus").val("Open");
                    $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #ddlType").val("Final");
                    $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").data('serialize', $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").serialize());
                });

            }
        }
        // For Creating Order and Result both at the same time
        if (ClinicalRadiologyResultDetail.params.mode == "Add") {

            $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #btnUploadResult,#btnViewAttachment,#btnViewPDF").addClass("disableAll");

            //Enable disable Print button
            ClinicalRadiologyResultDetail.enableDisableOrderResultButton('btnPrintResult', 'hfRadiologyResultId');
            ClinicalRadiologyResultDetail.enableDisableOrderResultButtons();


            //Initialize data table
            var PanelRadiologyResultGrid = "#" + ClinicalRadiologyResultDetail.params.PanelID + " #pnlLab_ResultDetail";
            var RadiologyResultGridId = "#" + ClinicalRadiologyResultDetail.params.PanelID + " #dgvRadiologyResultDetail";

            if ($.fn.dataTable.isDataTable(RadiologyResultGridId)) {
                $(RadiologyResultGridId).dataTable().fnClearTable();
                $(RadiologyResultGridId).dataTable().fnDestroy();
            }
            ClinicalRadiologyResultDetail.EditableGrid = EMRUtility.MakeEditableGrid(PanelRadiologyResultGrid, RadiologyResultGridId, ClinicalRadiologyResultDetail, 0, false, true, false, false, false, null);
            $(RadiologyResultGridId + " tbody tr").remove();

            ClinicalRadiologyResultDetail.params.RadiologyResultId = -1;
            ClinicalRadiologyResultDetail.params.RadiologyOrderId = -1;
            var RadiologyOrderId = ClinicalRadiologyResultDetail.params.RadiologyOrderId;
            $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #hfRadiologyResultId").val(-1);

            ClinicalRadiologyResultDetail.showHideEditableControls(false);
            var $form = $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail");
            //if (Clinical_RadiologyOrder.params.ParentCtrl == 'clinicalTabProgressNote') {

            //    CacheManager.BindCodes('GetProvider', false).done(function (result) {
            //        $("#frmClinicalRadiologyResultDetail #txtProvider").autocomplete({
            //            autoFocus: true,
            //            source: Providers,
            //            select: function (event, ui) {

            //                setTimeout(function () {
            //                    $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #txtProvider").attr("Provider", Clinical_ProgressNote.params["CurrentNotesProviderId"]);
            //                    $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #hfProvider").val(Clinical_ProgressNote.params["CurrentNotesProviderId"]);
            //                }, 100);
            //            }
            //        });
            //        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail #txtProvider").val(Clinical_ProgressNote.params["CurrentNotesProviderText"]);
            //        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail #hfProvider").val(Clinical_ProgressNote.params["CurrentNotesProviderId"]);
            //    });
            //    CacheManager.BindCodes('GetFacility', false).done(function (result) {
            //        $("#frmClinicalRadiologyResultDetail #txtFacility").autocomplete({
            //            autoFocus: true,
            //            source: Facilities,
            //            select: function (event, ui) {

            //                setTimeout(function () {
            //                    $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #txtFacility").attr("Facility", Clinical_ProgressNote.params["NotesFacilityIDForFollowUp"]);
            //                    $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #hfFacility").val(Clinical_ProgressNote.params["NotesFacilityIDForFollowUp"]);
            //                }, 100);
            //            }
            //        });
            //        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #txtFacility").val(Clinical_ProgressNote.params["NotesFacilityName"]);
            //        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #txtFacility").attr("Facility", Clinical_ProgressNote.params["NotesFacilityIDForFollowUp"]);
            //        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #hfFacility").val(Clinical_ProgressNote.params["NotesFacilityIDForFollowUp"]);
            //    });
            //} else {

            //    //ClinicalRadiologyResultDetail.loadAllAutocomplete();
            //}


            //Load laboratries
            Clinical_LabOrder.LoadLabs('ddlLabId', ClinicalRadiologyResultDetail.params.PanelID).done(function () {

                ClinicalRadiologyResultDetail.validateRadiologyResultDetail(true);

                if (ClinicalRadiologyResultDetail.params.mode == "Add") {
                    ClinicalRadiologyResultDetail.setFavSearch();
                    //if (ClinicalRadiologyResultDetail.params["LastRadiologyResultLabName"]) {
                    //    var theText = ClinicalRadiologyResultDetail.params["LastRadiologyResultLabName"];
                    //    if (theText != "") {
                    //        $("#" + ClinicalRadiologyResultDetail.params.PanelID + ' #ddlLabId option:contains(' + theText + ')').attr('selected', 'selected');
                    //    }
                    //} else {
                    //    $("#" + ClinicalRadiologyResultDetail.params.PanelID + ' #ddlLabId').val("");
                    //}
                }
                $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").data('serialize', $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").serialize());
            });

            // Create Date and time picker
            utility.CreateDatePicker(ClinicalRadiologyResultDetail.params.PanelID + ' #frmClinicalRadiologyResultDetail input[id="txtOrderDate"]', function () {
                //on-change callback method
            }, true);

            $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #frmClinicalRadiologyResultDetail input[id="txtOrderTime"]').timepicker({
                defaultTime: new Date()
            });

            ClinicalRadiologyResultDetail.loadProviderFromAppData = true;
            ClinicalRadiologyResultDetail.ProviderOnDemandLoad = true;

            if ($('#' + ClinicalRadiologyResultDetail.params.PanelID + " #txtProvider").val() == "") {
                $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #txtProvider").val(globalAppdata.DefaultProviderName);
                $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #txtProvider").attr("Provider", globalAppdata.DefaultProviderId);
                $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #hfProvider").val(globalAppdata.DefaultProviderId);
                ClinicalRadiologyResultDetail.loadProviderFromAppData = true;
            }
            if ($('#' + ClinicalRadiologyResultDetail.params.PanelID + " #txtFacility").val() == "") {
                $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #txtFacility").val(globalAppdata.DefaultFacilityName);
                $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #txtFacility").attr("Facility", globalAppdata.DefaultFacilityId);
                $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #hfFacility").val(globalAppdata.DefaultFacilityId);
                ClinicalRadiologyResultDetail.loadFacilityFromAppData = true;
            }

        }
        if (ClinicalRadiologyResultDetail.params.ParentCtrl == "Clinical_RadiologyOrder") {
            EMRUtility.appendPrevNext_NotesComponent_Btns(ClinicalRadiologyResultDetail.params.PanelID, 'Orders', 'Radiology', 'ClinicalRadiologyResultDetail.unLoad(null,null,true);', null, true);
        }
        ClinicalRadiologyResultDetail.domReadyFunction();

        setTimeout(function () {
            if ($("#dgvRadiologyResultDetail").parent().hasClass('Of-a')) $("#dgvRadiologyResultDetail").parent().removeClass('Of-a');
            $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").data('serialize', $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").serialize());
        }, 1000);


        ClinicalRadiologyResultDetail.ProviderOnDemandLoad = true;
        ClinicalRadiologyResultDetail.FacilityOnDemandLoad = true;
        ClinicalRadiologyResultDetail.loadAllAutocomplete();
        ClinicalRadiologyResultDetail.openAssignee();


        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail #txtProvider").focus(function () {

            if (!ClinicalRadiologyResultDetail.isProviderLoaded)
                ClinicalRadiologyResultDetail.ProviderOnDemandLoad = true;
            else
                ClinicalRadiologyResultDetail.ProviderOnDemandLoad = false;

            ClinicalRadiologyResultDetail.loadAllAutocomplete();
        });
        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail #txtFacility").focus(function () {
            if (!ClinicalRadiologyResultDetail.isFacilityLoaded)
                ClinicalRadiologyResultDetail.FacilityOnDemandLoad = true;
            else
                ClinicalRadiologyResultDetail.FacilityOnDemandLoad = false;

            ClinicalRadiologyResultDetail.loadAllAutocomplete();
        });
        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").data('serialize', $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").serialize());
        ClinicalRadiologyResultDetail.documentReady();
    },
    documentReady: function () {
        $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #txtComment').on("click", function (e) {
            if (!$('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #MacroDescDetails').is(":hidden")) {
                $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #MacroDescDetails').hide();
            }
        });
        $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #txtComment').on("keyup", function (e) {

            if (e.keyCode == 190 || e.keyCode == 110) // dot key is pressed
            {
                e.preventDefault();
                if ($('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #txtComment').find("#marker").length > 0) {
                    $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #txtComment').find("#marker").remove();
                }
                EMRUtility.pasteHtmlAtCaret('<span id=marker></span>');
                if (EMRUtility.callAutopopulationOrNot(ClinicalRadiologyResultDetail.params.PanelID, "txtComment")) {
                    $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #txtComment').focus();
                    EMRUtility.AutoKeyWordPopulateForDiv(ClinicalRadiologyResultDetail.params.PanelID, "txtComment", "Diagnostic Imaging Order", 0);
                }
                else {
                    $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #txtComment').find("#marker").remove();
                }

            }
        });
    },
    // Pop to open for Assignee
    //Auto Complete for Assignee txt field
    openAssignee: function () {
        CacheManager.BindCodes('GetUsersFullName', true).done(function (result) {
            var Ctrl = $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail #txtAssignee");
            var hfCtrl = $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail  #hfAssigneeId");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", GetUsersFullName, null, hfCtrl);
        });

    },

    OpenAssignee: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmClinicalRadiologyResultDetail";
        params["AssigneeId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ClinicalRadiologyResultDetail";
        params["RefCtrl"] = "txtAssignee";
        params["RefCtrlHidden"] = "hfAssigneeId";
        params["RefCtrlLink"] = "lnkAssignee";
        LoadActionPan('Admin_Provider', params);
    },

    enableDisableOrderResultButtons: function (isDisable) {

        if (isDisable == null)
            var isDisable = ($("#" + ClinicalRadiologyResultDetail.params.PanelID + " #dgvRadiologyResultDetail tbody tr[class*=childRow-bg]").length > 0 && $("#" + ClinicalRadiologyResultDetail.params.PanelID + " #dgvRadiologyResultDetail tbody tr").find('.dataTables_empty').length == 0) ? false : true;

        var resultDetailsButtons = $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail #btnSaveResult,#btnSendToPortal");
        if (isDisable) {
            resultDetailsButtons.addClass("disableAll");
        }
        else {
            resultDetailsButtons.removeClass("disableAll");
        }
        return isDisable;
    },

    enableDisableOrderResultButton: function (buttonId, hiddenFieldId) {

        var controlBasedId = $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #" + hiddenFieldId).val();
        var isDisable = controlBasedId > "0" ? false : true;

        var $button = $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail #" + buttonId);
        if (isDisable) {
            $button.addClass("disableAll");
        }
        else {
            $button.removeClass("disableAll");
        }
    },

    showHideEditableControls: function (isHide) {

        var $self = $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail");

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

    //Author Name: Abid Ali
    //Created Date: 16-04-2016
    //Description: get parent child row Json
    getParentChildJson: function (parentGridRow) {

        var $parentRow = $(parentGridRow);
        var RadiologyOrderResultDetailModel = {};

        //Start//Push Parent Row json

        var parentRowId = $parentRow.attr('id');
        var cptCode = $parentRow.attr('CPTCode');
        var cptDescription = $parentRow.attr('CPTDescription');

        var snmdCode = $parentRow.attr('CPTSNOMEDCodeId');
        var snmdDescription = $parentRow.attr('CPTSNOMEDDescription');

        RadiologyOrderResultDetailModel['RadiologyOrderResultDetailId'] = $parentRow.attr('id');
        RadiologyOrderResultDetailModel['CPTCode'] = cptCode;
        RadiologyOrderResultDetailModel['CPTCodeDescription'] = cptDescription;
        RadiologyOrderResultDetailModel['CPTSNOMEDCodeId'] = snmdCode;
        RadiologyOrderResultDetailModel['CPTSNOMEDDescription'] = snmdDescription;
        RadiologyOrderResultDetailModel["dtpRadiologyDate"] = $parentRow.find('td input[id^=dtpDate]').val();
        RadiologyOrderResultDetailModel["tpRadiologyTime"] = $parentRow.find('td input[id^=tpTime]').val();

        //End//Push Parent Row json

        var dtbParentRow = ClinicalRadiologyResultDetail.EditableGrid.datatable.row(parentGridRow);

        //check for child rows
        if (dtbParentRow.child() != null) {

            if (dtbParentRow.child().length > 0) {

                var ChildRows = [];

                //Start//Push child Row json
                pushChilds = function () {

                    var childRows = dtbParentRow.child();
                    $(childRows).each(function () {

                        var $this = $(this);
                        var chidlId = $this.attr('child-id');

                        var childJSONData = $this.getMyJSONByName();
                        //Child row object
                        var ChildResultDetailModel = JSON.parse(childJSONData);

                        ChildResultDetailModel["RadiologyOrderResultDetailId"] = chidlId;
                        ChildResultDetailModel["LOINC"] = $this.attr("LOINICCODE");
                        ChildResultDetailModel["LOINCDescription"] = $this.attr("LOINICDescription");

                        ChildResultDetailModel["CPTSNOMEDCodeId"] = $this.attr("CPTSNOMEDCodeId");
                        ChildResultDetailModel["CPTSNOMEDDescription"] = $this.attr("CPTSNOMEDDescription");

                        ChildResultDetailModel["ObservationDate"] = $this.find('td input[id*=Child_dtpDate]').val() + " " + $this.find('td input[id*=Child_tpTime]').val();
                        ChildRows.push(ChildResultDetailModel);

                    });
                }
                //End//Push child Row json

                $.when(pushChilds()).then(function () {

                    RadiologyOrderResultDetailModel['ChildRows'] = ChildRows;
                });
            }
        }
        return RadiologyOrderResultDetailModel;
    },

    //Author Name: Muhammad Arshad
    //Created Date: 19-04-2016
    //Description: open LOINC search screen
    openLOINCSearch: function (row, $row) {
        var params = [];
        params["GridRow"] = row;
        params["Grid$Row"] = $row;
        params["FromAdmin"] = ClinicalRadiologyResultDetail.params["FromAdmin"];
        params["ParentCtrl"] = 'ClinicalRadiologyResultDetail';
        params["displayTestControl"] = true;
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
                var PatientId = $("#PatientProfile #hfPatientId").val();
                var PatientName = "";
                if (PatientFullName.length > 0) {
                    var Firstname = PatientFullName.split(" ")[1];
                    var Lastname = PatientFullName.split(" ")[0];
                    var MiddleInitial = PatientFullName.split(" ")[2];
                    PatientName = Lastname + " " + Firstname + " " + MiddleInitial;
                }
                params["patientId"] = $('#PatientProfile #hfPatientId').val();
                params["RefCtrl"] = "RadiologyResult";
                params['RadiologyResultId'] = ClinicalRadiologyResultDetail.params.RadiologyResultId;
                params['RefModuleName'] = "Radiology Result";
                //Start 12-05-2016 Edit By Humaira Yousaf Bug# EMR-1036
                //RadiologyResultId is replaced with RadiologyOrderId to also handle the case when result is not added against order but attachment is uploaded
                params['TransitionId'] = ClinicalRadiologyResultDetail.params.RadiologyOrderId; //ClinicalRadiologyResultDetail.params.RadiologyResultId;
                //End 12-05-2016 Edit By Humaira Yousaf Bug# EMR-1036
                params["FromAdmin"] = "0";
                params["mode"] = "Add";
                params["ParentCtrl"] = 'ClinicalRadiologyResultDetail';
                params["PatientName"] = PatientName;//ClinicalRadiologyResultDetail.params.PatientName;
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
                param["RefCtrl"] = "RadiologyResult";
                param['RadiologyResultId'] = ClinicalRadiologyResultDetail.params.RadiologyResultId;
                param['RefModuleName'] = "Radiology Result";
                param['TransitionId'] = ClinicalRadiologyResultDetail.params.RadiologyResultId;
                param['patientID'] = $('#PatientProfile #hfPatientId').val();
                param["ParentCtrl"] = 'ClinicalRadiologyResultDetail';
                LoadActionPan('Document_Scan', param);
            }
            else
                utility.DisplayMessages(strMessage, 2);

        });
    },

    //Author Name: Abid Ali
    //Created Date: 16-04-2016
    //Description: Binds RadiologyResults with UI controls.
    fillRadiologyResult: function (RadiologyResultId, RadiologyOrderId) {

        ClinicalRadiologyResultDetail.loadRadiologyResults_DbCall(RadiologyResultId, RadiologyOrderId).done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {

                var $self = $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #frmClinicalRadiologyResultDetail');

                //sectionInfo details
                var sectionInfoDetails = JSON.parse(response.RadiologyResultOrderInfoFill_JSON);
                //Array Of RadiologyOrderTests
                var RadiologyResultTests = JSON.parse(response.RadiologyResultOrderTestFill_JSON);

                $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #hfRadiologyResultId").val(sectionInfoDetails.RadiologyOrderResultId);


                ClinicalRadiologyResultDetail.params.RadiologyResultId = sectionInfoDetails.RadiologyOrderResultId;
                //Binds sectionInfo Details
                utility.bindMyJSONByName(true, sectionInfoDetails, false, $self).done(function () {
                    $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #hfLabId').val(sectionInfoDetails.LaboratoryId);
                    $self.find('#hfOrderNo').val(sectionInfoDetails.OrderNo);
                    if (sectionInfoDetails.Status == "") {
                        $self.find('#ddlStatus').val("Final");
                    }
                    $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #txtComment').text(sectionInfoDetails.Comments);
                   
                    $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").data('serialize', $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").serialize());


                    var comm = '<div>' + sectionInfoDetails.Comments + '</div>';
                    var obj = $(comm);
                    $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #txtComment').html(obj);
                });

                //Load the grid and show hide buttons
                $.when(ClinicalRadiologyResultDetail.RadiologyResultGridLoad(response, RadiologyResultTests)).then(
                    function () {

                        var isDisableCtrls = (sectionInfoDetails.IsAknowledged == "True");

                        //Enable/Disable input controls
                        ClinicalRadiologyResultDetail.disableRadiologyResultControls(isDisableCtrls);

                        //Enable/Disable button controls
                        ClinicalRadiologyResultDetail.enableDisableOrderResultButton('btnPrintResult', 'hfRadiologyResultId');
                        ClinicalRadiologyResultDetail.enableDisableOrderResultButtons((isDisableCtrls == true) ? isDisableCtrls : null);
                        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").data('serialize', $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").serialize());
                    });
                ClinicalRadiologyResultDetail.setFavSearch();
                $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").data('serialize', $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").serialize());
            }
            else {
                ClinicalRadiologyResultDetail.enableDisableOrderResultButtons();
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Function to disable Radiology Order Result Controls
    disableRadiologyResultControls: function (IsSigned) {

        var detailsDivs = $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail #divResultInformation,#comments-remarks,#divRadiologyResultInformation");
        var detailsButtons = $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail #btnSaveResult,#btnResetRadiologyResult");
        var printRequisitionButton = $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail #btnPrintResult");

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

    //Author Name: Abid Ali
    //Created Date: 19-04-2016
    //Description: Loads Radiology results grid
    RadiologyResultGridLoad: function (response) {

        //Start/ Grid Panel and ID
        var PanelRadiologyResultGrid = "#" + ClinicalRadiologyResultDetail.params.PanelID + " #pnlLab_ResultDetail";
        var RadiologyResultGridId = "#" + ClinicalRadiologyResultDetail.params.PanelID + " #dgvRadiologyResultDetail";
        $(RadiologyResultGridId + " tbody").find("tr").remove();

        if ($.fn.dataTable.isDataTable(RadiologyResultGridId)) {
            $(RadiologyResultGridId).dataTable().fnClearTable();
            $(RadiologyResultGridId).dataTable().fnDestroy();
        }
        //End/ Grid Panel and ID
        var RadiologyResults = [];
        var parentRowRadiologyResultTests = JSON.parse(response.RadiologyResultOrderTestFill_JSON);
        if (parentRowRadiologyResultTests.RadiologyOrderResultDetailModels.length > 0) {

            //Draw the Grid
            buildTable = function (labTests, parrent) {

                $.each(labTests, function (i, item) {

                    var RadiologyResultTestId = item.RadiologyOrderResultDetailId;

                    var currentRow = ClinicalRadiologyResultDetail.addRadiologyResultRow(item);

                    //Build Child Rows (Agruments (current Row/ Child Items Json)
                    var childRows = ClinicalRadiologyResultDetail.buildRowChild(currentRow, item.ChildRows);

                    RadiologyResults.push({ row: currentRow, childs: childRows });

                    //Push parent child rows
                    //  rowsParentChild.push({ row: currentRow, childs: childRows });

                });
            }

            //Draw Table
            drawTable = function () {

                var PanelRadiologyResultGrid = "#" + ClinicalRadiologyResultDetail.params.PanelID + " #pnlLab_ResultDetail";
                var RadiologyResultGridId = "#" + ClinicalRadiologyResultDetail.params.PanelID + " #dgvRadiologyResultDetail";

                ClinicalRadiologyResultDetail.EditableGrid = EMRUtility.MakeEditableGrid(PanelRadiologyResultGrid, RadiologyResultGridId, ClinicalRadiologyResultDetail, 0, false, true, false, false, false, null);
                ClinicalRadiologyResultDetail.EditableGrid.datatable.draw();

                //push parent/childs rows in the datatable
                $.each(RadiologyResults, function (i, item) {

                    if (ClinicalRadiologyResultDetail.EditableGrid != null) {

                        var row = ClinicalRadiologyResultDetail.EditableGrid.datatable.row(item.row);
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
                ClinicalRadiologyResultDetail.SetDateTimeControl($("#" + ClinicalRadiologyResultDetail.params.PanelID + ' #dgvRadiologyResultDetail tbody'), true);
                // $("#" + ClinicalRadiologyResultDetail.params.PanelID + " #dgvRadiologyResultDetail").dataTable().fnPageChange('first');
            }
            //builds and draw the Grid
            $.when(buildTable(parentRowRadiologyResultTests.RadiologyOrderResultDetailModels)).then(drawTable());

        }
        else {
            $("#" + ClinicalRadiologyResultDetail.params.PanelID + " #pnlLab_ResultDetail #divRadiologyResultPaging").css("display", "none");
            $("#" + ClinicalRadiologyResultDetail.params.PanelID + ' #dgvRadiologyResultDetail').DataTable({
                "language": {
                    "emptyTable": "No Radiology orders Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bDestroy": true, "bSort": false
            });
        }
        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").data('serialize', $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").serialize());
    },


    selectProvider: function (providerId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Orders and Results_Diagnostic Imaging Result", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
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
                                    $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail #txtProvider").val(decodeHtmlEntity(item.LastName + ', ' + item.FirstName + ' ' + item.MiddleInitial));
                                    $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail #hfProvider").val(decodeHtmlEntity(item.ProviderId));
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
    //Date :  17-03-2016
    //Reason: Bind Guaranter
    bindGuarantor: function () {
        var shortName = $('#pnlClinicalRadiologyResultDetail #txtGuarantor').val();
        utility.GetGuarontorArray(shortName).done(function (response) {

            $('#pnlClinicalRadiologyResultDetail #txtGuarantor').autocomplete({
                //source: AllPatients, // pass an array (without a comma)
                autoFocus: true,
                source: response,
                select: function (event, ui) {

                    setTimeout(function () {

                        $("#pnlClinicalRadiologyResultDetail #hfLabGuarantorId").val(ui.item.id); // add the selected id
                        if ($("#pnlClinicalRadiologyResultDetail #lnkGuarantorEdit").css("display") == "none") {
                            $("#pnlClinicalRadiologyResultDetail #lnkGuarantorEdit").css("display", "inline");
                            $("#pnlClinicalRadiologyResultDetail #lblGuarantor").css("display", "none");
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
        params["ParentCtrl"] = "ClinicalRadiologyResultDetail";
        params["RefCtrl"] = "txtGuarantor";
        LoadActionPan('Patient_Guarantor', params);
    },


    //Author: Abid Ali
    //Date :  17-03-2016
    //Reason: open Guaranter
    openGuarantorDetail: function () {
        //Patient_Guarantor.GuarantorEdit($('#pnlDemographic #hfGuarantor').val(), 'patTabDemographic');
        var params = [];
        params["GuarantorId"] = $('#pnlClinicalRadiologyResultDetail #hfLabGuarantorId').val();
        params["mode"] = "Edit";
        params["RefCtrl"] = "txtGuarantor";
        params["ParentCtrl"] = 'ClinicalRadiologyResultDetail';
        LoadActionPan('guarantorDetail', params);
    },

    loadAllAutocomplete: function () {
        var $form = $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail");

        if (ClinicalRadiologyResultDetail.loadProviderFromAppData && ClinicalRadiologyResultDetail.ProviderOnDemandLoad) {

            if (Clinical_RadiologyOrder.params.ParentCtrl == 'clinicalTabProgressNote') {

                CacheManager.BindCodes('GetProvider', false).done(function (result) {
                    var Ctrl = $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #txtProvider");
                    var onSelect = function (dataItem) {
                        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #txtProvider").attr("Provider", Clinical_ProgressNote.params["CurrentNotesProviderId"]);
                        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #hfProvider").val(Clinical_ProgressNote.params["CurrentNotesProviderId"]);
                        ClinicalRadiologyResultDetail.setFavSearch();
                    }
                    utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, null, onSelect);

                    $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail #txtProvider").val(Clinical_ProgressNote.params["CurrentNotesProviderText"]);
                    $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail #hfProvider").val(Clinical_ProgressNote.params["CurrentNotesProviderId"]);
                    utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail #txtProvider"), Clinical_ProgressNote.params.CurrentNotesProviderText, $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail #hfProvider"), Clinical_ProgressNote.params.CurrentNotesProviderId);
                    $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").data('serialize', $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").serialize());
                });
                ClinicalRadiologyResultDetail.loadProviderFromAppData = true;
                ClinicalRadiologyResultDetail.ProviderOnDemandLoad = false;
                ClinicalRadiologyResultDetail.isProviderLoaded = true;

            }
            else {
                if (globalAppdata.DefaultProviderName != "" && globalAppdata.DefaultProviderName != "- Select -") {
                    CacheManager.BindCodes('GetProvider', false).done(function (result) {
                        var Ctrl = $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #txtProvider");
                        var hfCtrl = $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #hfProvider");
                        var onSelect = function (dataItem) {
                            $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #txtProvider").attr("Provider", dataItem.id);
                            ClinicalRadiologyResultDetail.setFavSearch();
                        }
                        utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, onSelect);
                        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail #txtProvider").val(globalAppdata.DefaultProviderName);
                        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail #hfProvider").val(globalAppdata.DefaultProviderId);
                        utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail #txtProvider"), globalAppdata.DefaultProviderName, $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail #hfProvider"), globalAppdata.DefaultProviderId);
                        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").data('serialize', $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").serialize());
                    });
                    ClinicalRadiologyResultDetail.loadProviderFromAppData = true;
                    ClinicalRadiologyResultDetail.ProviderOnDemandLoad = false;
                    ClinicalRadiologyResultDetail.isProviderLoaded = true;
                }
                else {
                    CacheManager.BindCodes('GetProvider', false).done(function (result) {
                        var Ctrl = $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #txtProvider");
                        var hfCtrl = $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #hfProvider");
                        var onSelect = function (dataItem) {
                            $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #txtProvider").attr("Provider", dataItem.id);
                            ClinicalRadiologyResultDetail.setFavSearch();
                        }
                        utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, onSelect);
                        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail #txtProvider").val("");
                        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail #hfProvider").val("");
                        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").data('serialize', $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").serialize());
                    });
                    ClinicalRadiologyResultDetail.loadProviderFromAppData = true;
                    ClinicalRadiologyResultDetail.ProviderOnDemandLoad = false;
                    ClinicalRadiologyResultDetail.isProviderLoaded = true;
                }
            }
        }

        if (ClinicalRadiologyResultDetail.loadFacilityFromAppData && ClinicalRadiologyResultDetail.FacilityOnDemandLoad) {
            if (Clinical_RadiologyOrder.params.ParentCtrl == 'clinicalTabProgressNote') {
                CacheManager.BindCodes('GetFacility', false).done(function (result) {
                    var Ctrl = $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #txtFacility");
                    var onSelect = function (dataItem) {
                        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #txtFacility").attr("Facility", Clinical_ProgressNote.params["NotesFacilityIDForFollowUp"]);
                        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #hfFacility").val(Clinical_ProgressNote.params["NotesFacilityIDForFollowUp"]);
                    }
                    utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, null, onSelect);

                    $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #txtFacility").val(Clinical_ProgressNote.params["NotesFacilityName"]);
                    $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #txtFacility").attr("Facility", Clinical_ProgressNote.params["NotesFacilityIDForFollowUp"]);
                    $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #hfFacility").val(Clinical_ProgressNote.params["NotesFacilityIDForFollowUp"]);
                    utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalRadiologyResultDetail.params.PanelID + " #txtFacility"), Clinical_ProgressNote.params.NotesFacilityName, $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #hfFacility"), Clinical_ProgressNote.params.NotesFacilityIDForFollowUp);
                    $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").data('serialize', $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").serialize());
                });
                ClinicalRadiologyResultDetail.loadFacilityFromAppData = true;
                ClinicalRadiologyResultDetail.FacilityOnDemandLoad = false;
                ClinicalRadiologyResultDetail.isFacilityLoaded = true;
            }
            else {
                if (globalAppdata.DefaultFacilityName != "" && globalAppdata.DefaultFacilityName != "- Select -") {
                    CacheManager.BindCodes('GetFacility', false).done(function (result) {
                        var Ctrl = $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #txtFacility");
                        var hfCtrl = $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #hfFacility");
                        var onSelect = function (dataItem) { $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #txtFacility").attr("Facility", dataItem.id); }
                        utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl, onSelect);

                        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #txtFacility").val(globalAppdata.DefaultFacilityName);
                        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #txtFacility").attr("Facility", globalAppdata.DefaultFacilityId);
                        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #hfFacility").val(globalAppdata.DefaultFacilityId);
                        utility.SetKendoAutoCompleteSourceforValidate($('#' + ClinicalRadiologyResultDetail.params.PanelID + " #txtFacility"), globalAppdata.DefaultFacilityName, $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #hfFacility"), globalAppdata.DefaultFacilityId);
                        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").data('serialize', $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").serialize());
                    });
                    ClinicalRadiologyResultDetail.loadFacilityFromAppData = true;
                    ClinicalRadiologyResultDetail.FacilityOnDemandLoad = false;
                    ClinicalRadiologyResultDetail.isFacilityLoaded = true;
                } else {
                    CacheManager.BindCodes('GetFacility', false).done(function (result) {
                        var Ctrl = $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #txtFacility");
                        var hfCtrl = $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #hfFacility");
                        var onSelect = function (dataItem) { $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #txtFacility").attr("Facility", dataItem.id); }
                        utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl, onSelect);

                        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #txtFacility").val("");
                        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #txtFacility").attr("Facility", "");
                        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #hfFacility").val("");
                        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").data('serialize', $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").serialize());
                    });
                    ClinicalRadiologyResultDetail.loadFacilityFromAppData = true;
                    ClinicalRadiologyResultDetail.FacilityOnDemandLoad = false;
                    ClinicalRadiologyResultDetail.isFacilityLoaded = true;
                }
            }
        }
    },


    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Enable disable dropdownlists
    enableDisableDropdownList: function (listOfDdlIds, isHide) {

        $.each(listOfDdlIds, function (index, item) {
            if (isHide) {
                $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #' + item).prop('disabled', true);
            }
            else {
                $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #' + item).removeClass('disabled', false);
            }
        });
    },



    // -------------- Provider ---------------------
    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: open provider form
    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmClinicalRadiologyResultDetail";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ClinicalRadiologyResultDetail";
        LoadActionPan('Admin_Provider', params);
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: open provider detail
    OpenProviderDetail: function () {
        //Admin_Provider.ProviderEdit($('#pnlClinicalNotes #hfProvider').val(),'clinicalTabNotes');
        var params = [];
        params["ProviderId"] = $('#pnlClinicalRadiologyResultDetail #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'ClinicalRadiologyResultDetail';
        LoadActionPan('providerDetail', params);
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: open facility form
    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmClinicalRadiologyResultDetail";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ClinicalRadiologyResultDetail";
        LoadActionPan('Admin_Facility', params);
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: open facility detail form
    OpenFacilityDetail: function () {
        //Admin_Facility.FacilityEdit($('#' + demographicDetail.params.PanelID + ' #hfFacility').val(), "demographicDetail");
        var params = [];
        params["FacilityId"] = $('#pnlClinicalRadiologyResultDetail #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacility";
        params["ParentCtrl"] = 'ClinicalRadiologyResultDetail';
        LoadActionPan('facilityDetail', params);
    },

    HideProviderLink: function () {
        $('#pnlClinicalRadiologyResultDetail #txtProvider').attr("ProviderId", "-1");
        $('#pnlClinicalRadiologyResultDetail #hfProvider').val("-1");
        $("#pnlClinicalRadiologyResultDetail #lnkProviderEdit").css("display", "none");
        $("#pnlClinicalRadiologyResultDetail #lblProvider").css("display", "inline");
    },

    // -------------- End Provider -----------------

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Function to apply bootstrap validations
    validateRadiologyResultDetail: function (isEditableControls) {
        var $self = $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail");
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
        if (isEditableControls) {

            fields["LabId"] = {
                group: '.col-sm-3',
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            }; fields["Facility"] = {
                group: '.col-sm-3',
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            }; fields["Provider"] = {
                group: '.col-sm-3',
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
            e.preventDefault();
            if (e.type == "success") {

                var $form = $(e.target);
                var $button = $form.data('bootstrapValidator').getSubmitButton();
                switch ($button.attr('id')) {

                    case 'btnAcknowledge':
                        if (!ClinicalRadiologyResultDetail.isError) {

                            utility.myConfirm('36', function () {
                                ClinicalRadiologyResultDetail.RadiologyResultSave('Acknowledge');
                            }, function () {

                            },
                              '36');
                        }
                        break;

                    case 'btnPrintResult':
                        ClinicalRadiologyResultDetail.printRadiologyResult();
                        break;

                    case 'btnSendToPortal':
                        if (!ClinicalRadiologyResultDetail.isError) {
                            ClinicalRadiologyResultDetail.RadiologyResultSave('SendToPortal');
                        }
                        break;

                    default:
                        if (!ClinicalRadiologyResultDetail.isError) {
                            ClinicalRadiologyResultDetail.RadiologyResultSave('save');
                        }

                        break;
                }
            }
            e.type = "";
        });

    },

    bindAutoComplete: function (element) {
        var hiddenCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "ClinicalRadiologyResultDetail", null, true);

    },

    openCPTCode: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ClinicalRadiologyResultDetail";
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtCPTCode";
        params["ParentCtrlPanelID"] = ClinicalRadiologyResultDetail.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, ClinicalRadiologyResultDetail.params.PanelID);
    },

    BindRadiologyOrderResultGridItem: function (cptCode, procedureDescription, cptDescription) {

        var cptCod = cptCode;
        var procDesc = procedureDescription;
        var cptDesc = cptDescription;

        isTestAlreadySelected = ClinicalRadiologyResultDetail.validateTest(cptCod, procDesc);


        if (isTestAlreadySelected != true) {

            ClinicalRadiologyResultDetail.EditableGrid.datatable.row.add(ClinicalRadiologyResultDetail.addRadiologyResultRow(null, cptCode, procDesc));

            setTimeout(function () {
                $("#" + ClinicalRadiologyResultDetail.params.PanelID + " #txtLabCPTCode").val('');
            }, 200);

        }
        else {
            utility.DisplayMessages("Test is already selected", 2);
        }

        ClinicalRadiologyResultDetail.enableDisableOrderResultButtons();

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Reason: Binding numpad with height, weight, systolic and diastolic fields
    domReadyFunction: function () {
        $(document).ready(function () {
            $('.toggleHorSmallLeft section').click(function (e) {
                $(this).parent().toggleClass("toggled");
                ClinicalRadiologyResultDetail.toggleHorSmallLeftIcon($(this));

            });
        });
        $(function () {
            $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #frmClinicalRadiologyResultDetail [data-plugin-keyboard-numpad]').keyboard({
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
    //Reason: Loading ICD Codes for Problem List AutoComplete
    bindICD9AutoComplete: function (element) {
        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "ClinicalRadiologyResultDetail", null, false);
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
        if (ClinicalRadiologyResultDetail.params.TabID == 'clinicalTabProgressNote') {
            params['FromProgressNote'] = 'pnlClinicalProgressNote';
            params["ParentCtrl"] = 'ClinicalRadiologyResultDetail';
        }

        else {
            params["ParentCtrl"] = ClinicalRadiologyResultDetail.params["TabID"];

        }
        params["PanelID"] = ClinicalRadiologyResultDetail.params["PanelID"];

        params["ActionPanContainer"] = ClinicalRadiologyResultDetail.params["ActionPanContainer"];
        if (Ctrl != null) {
            params["RefCtrl"] = Ctrl;
        }
        if (HiddenCtrl != null) {
            params["RefHiddenCtrl"] = HiddenCtrl;
        }
        if (controlToLoad != "") {
            if (ClinicalRadiologyResultDetail.params.TabID == 'clinicalTabProgressNote' && SearchType == "ICD")
                LoadActionPan(controlToLoad, params, 'pnlClinicalProgressNote');
            else
                LoadActionPan(controlToLoad, params, ClinicalRadiologyResultDetail.params.PanelID);
        }

    },





    UnLoad: function (NextOrPre, controlToInvoke, caller) {
        ClinicalRadiologyResultDetail.deletedChildRows = [];

        //Start//Abid Ali // For bug# EMR-896
        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #divRadiologyOrderInformation').show();
        $('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #divRadiologyBillingInformation').show();
        //End//Abid Ali // For bug# EMR-896

        var form = '#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail";
        var saveButtonisHidden = $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail #btnSaveOrder").hasClass("hidden");


        if (controlToInvoke != null) {
            ClinicalRadiologyResultDetail.controlToInvoke = controlToInvoke;
            if (NextOrPre == true) {
                ClinicalRadiologyResultDetail.UnloadRadiologyOrderDetail(NextOrPre);
            }
            else {
                ClinicalRadiologyResultDetail.bNextPrev = true;
            }
        }
        else {

            if (caller == 'saveExit' || saveButtonisHidden == true) {
                if (ClinicalRadiologyResultDetail.params["ParentCtrl"] == "Clinical_RadiologyOrder") {
                    UnloadActionPan(ClinicalRadiologyResultDetail.params["ParentCtrl"], "ClinicalRadiologyResultDetail", null, ClinicalRadiologyResultDetail.params["ParentCtrlPanelID"]);
                }
                else {
                    UnloadActionPan(ClinicalRadiologyResultDetail.params["ParentCtrl"], "ClinicalRadiologyResultDetail");

                }
            }
                //Start 16-05-2016 Edit By Humaira Yousaf Bug# EMR-1097
                //Start || 15 July, 2016 || ZeeshanAK || Fix for EMR-1609
            else if ((ClinicalRadiologyResultDetail.params["ParentCtrl"] == "ClinicalRadiologyOrderDetail" || ClinicalRadiologyResultDetail.params["ParentCtrl"] == "clinicalTabRadiologyOrder") && ClinicalRadiologyResultDetail.isSaved == true || ClinicalRadiologyResultDetail.params["ParentCtrl"] == "Clinical_RadiologyOrder" && ClinicalRadiologyResultDetail.params.ParentCtrlPanelID != "pnlClinicalProgressNote #pnlClinicalRadiologyOrder") {
                //End   || 15 July, 2016 || ZeeshanAK || Fix for EMR-1609

                utility.myConfirm("2", function () {
                    ClinicalRadiologyResultDetail.isSaved = false;
                    //ClinicalRadiologyResultDetail.
                    UnloadActionPan(ClinicalRadiologyResultDetail.params["ParentCtrl"], "ClinicalRadiologyResultDetail", null, ClinicalRadiologyResultDetail.params["ParentCtrlPanelID"]);

                }, function () { },
                '2'
            );


            }
                //End 16-05-2016 Edit By Humaira Yousaf Bug# EMR-1097
            else {
                utility.UnLoadDialog(form, function () {
                    if (ClinicalRadiologyResultDetail.params["ParentCtrl"] == "Clinical_RadiologyOrder") {
                        UnloadActionPan(ClinicalRadiologyResultDetail.params["ParentCtrl"], "ClinicalRadiologyResultDetail", null, ClinicalRadiologyResultDetail.params["ParentCtrlPanelID"]);
                    }
                    else {
                        UnloadActionPan(ClinicalRadiologyResultDetail.params["ParentCtrl"], "ClinicalRadiologyResultDetail");

                    }


                    Clinical_RadiologyOrder.radiologyResultsSearch(null, null, null, "RadiologyResultDetail");
                    Clinical_RadiologyOrder.radiologyOrderSearch(null, null, null, "RadiologyOrderDetail");

                }, function () {
                    if (ClinicalRadiologyResultDetail.params["ParentCtrl"] == "Clinical_RadiologyOrder") {
                        UnloadActionPan(ClinicalRadiologyResultDetail.params["ParentCtrl"], "ClinicalRadiologyResultDetail", null, ClinicalRadiologyResultDetail.params["ParentCtrlPanelID"]);
                    }
                    else {
                        UnloadActionPan(ClinicalRadiologyResultDetail.params["ParentCtrl"], "ClinicalRadiologyResultDetail");

                    }
                });
            }
        }
        $('#FaceSheetPager .slick-track').attr('style', 'opacity: 1; transform: translate3d(-57px, 0px, 0px); width: 1193px;');

        ClinicalRadiologyResultDetail.loadProviderFromAppData = false;
        ClinicalRadiologyResultDetail.loadFacilityFromAppData = false;
        ClinicalRadiologyResultDetail.isProviderLoaded = false;
        ClinicalRadiologyResultDetail.isFacilityLoaded = false;
    },


    UnloadRadiologyOrderDetail: function (NextOrPre) {
        if (ClinicalRadiologyResultDetail.params["FromAdmin"] == "0") {
            if (ClinicalRadiologyResultDetail.params != null && ClinicalRadiologyResultDetail.params.ParentCtrl != null) {
                if (ClinicalRadiologyResultDetail.params.ParentCtrl == "Clinical_RadiologyOrder" && NextOrPre == true) {
                    UnloadActionPan(ClinicalRadiologyResultDetail.params.ParentCtrl, 'ClinicalRadiologyResultDetail');

                    UnloadActionPan('clinicalTabProgressNote', 'Clinical_RadiologyOrder');
                    if (ClinicalRadiologyResultDetail.controlToInvoke != null) {
                        setTimeout(function () {
                            if (ClinicalRadiologyResultDetail.controlToInvoke.indexOf('Consultation') > -1) {
                                ClinicalRadiologyResultDetail.controlToInvoke = "ConsultationOrder";
                            }
                            if (ClinicalRadiologyResultDetail.controlToInvoke.indexOf('Procedure') > -1) {
                                ClinicalRadiologyResultDetail.controlToInvoke = "ProcedureOrder";
                            }
                            Clinical_ProgressNote.SelectNotesComponentTab(ClinicalRadiologyResultDetail.controlToInvoke);
                            ClinicalRadiologyResultDetail.controlToInvoke = null;
                        }, 400);
                    }
                }
                else {
                    UnloadActionPan(ClinicalRadiologyResultDetail.params.ParentCtrl, 'ClinicalRadiologyResultDetail');
                    if (ClinicalRadiologyResultDetail.controlToInvoke != null)
                        setTimeout(function () {
                            if (ClinicalRadiologyResultDetail.controlToInvoke.indexOf('Consultation') > -1) {
                                ClinicalRadiologyResultDetail.controlToInvoke = "ConsultationOrder";
                            }
                            Clinical_ProgressNote.SelectNotesComponentTab(ClinicalRadiologyResultDetail.controlToInvoke);
                            ClinicalRadiologyResultDetail.controlToInvoke = null;
                        }, 400);
                }
            }
            else
                UnloadActionPan(null, 'ClinicalRadiologyResultDetail');
        }
        else {
            $("#pnlClinicalRadiologyOrder").remove();
            $(".modal-backdrop").remove();
        }
        $("#pnlClinicalRadiologyOrder").remove();
        $(".modal-backdrop").remove();

        EMRUtility.scrollToPNcomponent('ClinicalRadiologyResultDetail');
    },



    RadiologyOrderSave: function (JSONToSave, NotUpdateTestValues) {
        var deffered = $.Deferred();
        JSONToSave = JSON.parse(JSONToSave);
        JSONToSave["Status"] = "Signed";
        JSONToSave = JSON.stringify(JSONToSave);
        AppPrivileges.GetFormPrivileges("Orders and Results_Diagnostic Imaging Result", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                ClinicalRadiologyOrderDetail.saveRadiologyOrder(JSONToSave, NotUpdateTestValues).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {

                        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #hfOrderNo").val(response.orderNo);
                        $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #hfRadiologyOrderId").val(response.radiologicalOrderId);
                        ClinicalRadiologyResultDetail.params.RadiologyOrderId = response.radiologicalOrderId;
                    }
                    else {
                        // utility.DisplayMessages(response.Message, 3);
                    }
                    deffered.resolve();

                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        return deffered;
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Saves RadiologyResult
    RadiologyResultSave: function (sender) {
        var IsValid = true;
        if (ClinicalRadiologyResultDetail.params.RadiologyOrderId < 1) {
            IsValid = false;
            var RadiologyOrderJson = ClinicalRadiologyResultDetail.getRadiologyRowsJSON();
            $.when(ClinicalRadiologyResultDetail.RadiologyOrderSave(RadiologyOrderJson, 1)).done(function () {
                ClinicalRadiologyResultDetail.RadiologyResultInsertUpdate(sender);
                $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #btnUploadResult,#btnViewAttachment,#btnViewPDF").removeClass("disableAll");
            });
        }
        else {
            ClinicalRadiologyResultDetail.RadiologyResultInsertUpdate(sender);
            //var RadiologyOrderJson = ClinicalRadiologyResultDetail.getRadiologyRowsJSON();
            //$.when(ClinicalRadiologyResultDetail.RadiologyOrderSave(RadiologyOrderJson,1)).done(function () {
            //    ClinicalRadiologyResultDetail.RadiologyResultInsertUpdate(sender);
            //});
        }
    },

    RadiologyResultInsertUpdate: function (sender) {
        var FavVal = $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #ddlFavoriteListRadiologyResult').val();
        var RadiologyResultId = $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #hfRadiologyResultId").val() != "" ? $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #hfRadiologyResultId").val() : "-1";

        var self = $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail");
        if (self.find("#txtComment").text()) {
            self.find("#txtComments").val(self.find("#txtComment").text());
        }
        else {
            self.find("#txtComments").val("");
        }
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objData = JSON.parse(myJSON);

        //Push ParentChild Rows in RadiologyOrderResultDetailModels
        var RadiologyOrderResultDetailModels = [];

        $("#" + ClinicalRadiologyResultDetail.params.PanelID + " #dgvRadiologyResultDetail tbody tr:not([class*=child]").each(function () {

            var rowJSON = ClinicalRadiologyResultDetail.getParentChildJson(this);
            RadiologyOrderResultDetailModels.push(rowJSON);
        });
        objData["RadiologyOrderResultDetailModels"] = RadiologyOrderResultDetailModels;

        objData["DeletedResultDetailIds"] = ClinicalRadiologyResultDetail.deletedChildRows;

        objData["OrderNo"] = self.find('#hfOrderNo').val();
        objData["RadiologyOrderId"] = ClinicalRadiologyResultDetail.params.RadiologyOrderId;


        objData["IsSentToPortal"] = true;
        objData["Comments"] = $("#" + ClinicalRadiologyResultDetail.params.PanelID + " #txtComment").html();


        objData["IsAknowledged"] = (sender == "Acknowledge") ? true : false;

        //objData["CPTSNOMEDCodeId"] = $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #hfCPTSNOMEDCode').val();
        //objData["CPTSNOMEDDescription"] = $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #hfCPTSNOMEDDescription').val();

        myJSON = JSON.stringify(objData);



        if (ClinicalRadiologyResultDetail.params.mode == "Add") {

            AppPrivileges.GetFormPrivileges("Orders and Results_Diagnostic Imaging Result", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    ClinicalRadiologyResultDetail.saveRadiologyResult(myJSON).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            var isFavListOpened = $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail #favSectionDiv").hasClass("toggled");
                            $.when(EMRUtility.insertUpdateFavListStatus(ClinicalRadiologyResultDetail.FavListName, !isFavListOpened)).then(function () {
                                ClinicalRadiologyResultDetail.SaveFavListVal(FavVal);
                            });

                            if (ClinicalRadiologyResultDetail.params.ParentCtrl == "ClinicalRadiologyOrderDetail") {
                                ClinicalRadiologyOrderDetail.AddResult = true;
                                ClinicalRadiologyOrderDetail.AddedRadiologyOrderResultId = response.RadiologyOrderResultId;
                            }
                            if (ClinicalRadiologyResultDetail.params.ParentCtrl == "clinicalTabProgressNote") {
                                ClinicalRadiologyResultDetail.getLatestRadiologyResultByPatientId();
                            } else {
                                utility.DisplayMessages(response.message, 1);
                            }


                            //disable sent to portal button
                            //if (sender == "SendToPortal") {

                            //    $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #hfIsSentToPortal").val("-1");
                            //    ClinicalRadiologyResultDetail.enableDisableOrderResultButton('btnSendToPortal', 'hfIsSentToPortal');
                            //}


                            $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #hfRadiologyResultId").val(response.RadiologyOrderResultId);
                            ClinicalRadiologyResultDetail.enableDisableOrderResultButton('btnPrintResult', 'hfRadiologyResultId');

                            //Start 16-05-2016 Edit By Humaira Yousaf Bug# EMR-1097
                            ClinicalRadiologyResultDetail.isSaved = true;
                            //End 16-05-2016 Edit By Humaira Yousaf Bug# EMR-1097

                            var def = $.Deferred();
                            var resultDiv = " #dgvRadiologyResult";
                            var type = $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #ddlLabId option:selected').text();
                            if (type == "External Facility") {
                                def = Clinical_RadiologyOrder.externalRadiologyResultsSearch(null, null, null, "RadiologyResultDetail");
                                $('#' + Clinical_RadiologyOrder.params.PanelID + ' #ulRadilogyResultTypeTabsItems a[href="#RadiologyExternalResult"]').trigger('click');
                                resultDiv = " #dgvExternalRadiologyResult";
                                Clinical_RadiologyOrder.externalRadiologyOrderSearch(null, null, null, "RadiologyOrderDetail");
                            }
                            else {
                                def = Clinical_RadiologyOrder.radiologyResultsSearch(null, null, null, "RadiologyResultDetail");
                                $('#' + Clinical_RadiologyOrder.params.PanelID + ' #ulRadilogyResultTypeTabsItems a[href="#RadiologyInternalResult"]').trigger('click');
                                Clinical_RadiologyOrder.radiologyOrderSearch(null, null, null, "RadiologyOrderDetail");
                            }

                            $.when(def).then(function () {
                                if (ClinicalRadiologyResultDetail.params.ParentCtrl == "Clinical_RadiologyOrder" && Clinical_RadiologyOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                                    $("#pnlClinicalProgressNote #pnlClinicalRadiologyOrder" + resultDiv + " input#" + response.RadiologyOrderResultId).prop("checked", true);
                                }
                            });
                            // Clinical_RadiologyOrder.radiologyOrderSearch(null, null, null, "RadiologyOrderDetail");

                            if (sender == 'signprintorder') {
                                ClinicalRadiologyResultDetail.printRadiologyResult();
                            }
                            else {
                                ClinicalRadiologyResultDetail.UnLoad(null, null, 'saveExit');
                            }

                            //Unload Detail Page.
                            if (sender == "Acknowledge") {
                                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #btnAddRadiologyResult").text("View Result");
                            }
                            else {
                                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #btnAddRadiologyResult").text("Edit Result");
                            }

                            //   ClinicalRadiologyResultDetail.UnLoad();
                            $('#FaceSheetPager .slick-track').attr('style', 'opacity: 1; transform: translate3d(-57px, 0px, 0px); width: 1193px;');
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                            //Start 16-05-2016 Edit By Humaira Yousaf Bug# EMR-1097
                            ClinicalRadiologyResultDetail.isSaved = false;
                            //End 16-05-2016 Edit By Humaira Yousaf Bug# EMR-1097
                        }
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else if (ClinicalRadiologyResultDetail.params.mode == "Edit") {

            AppPrivileges.GetFormPrivileges("Orders and Results_Diagnostic Imaging Result", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    ClinicalRadiologyResultDetail.saveRadiologyResult(myJSON, RadiologyResultId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            var isFavListOpened = $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail #favSectionDiv").hasClass("toggled");
                            $.when(EMRUtility.insertUpdateFavListStatus(ClinicalRadiologyResultDetail.FavListName, !isFavListOpened)).then(function () {
                                ClinicalRadiologyResultDetail.SaveFavListVal(FavVal);
                            });

                            if (ClinicalRadiologyResultDetail.params.ParentCtrl == "ClinicalRadiologyOrderDetail") {
                                ClinicalRadiologyOrderDetail.AddResult = true;
                                ClinicalRadiologyOrderDetail.AddedRadiologyOrderResultId = response.RadiologyOrderResultId;
                            }
                            if (ClinicalRadiologyResultDetail.params.ParentCtrl == "clinicalTabProgressNote") {
                                ClinicalRadiologyResultDetail.getLatestRadiologyResultByPatientId();
                            } else {
                                utility.DisplayMessages(response.message, 1);
                            }


                            //disable sent to portal button
                            //if (sender == "SendToPortal") {

                            //    $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #hfIsSentToPortal").val("-1");
                            //    ClinicalRadiologyResultDetail.enableDisableOrderResultButton('btnSendToPortal', 'hfIsSentToPortal');
                            //}

                            $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #hfRadiologyResultId").val(response.RadiologyOrderResultId);
                            ClinicalRadiologyResultDetail.enableDisableOrderResultButton('btnPrintResult', 'hfRadiologyResultId');

                            //Start 16-05-2016 Edit By Humaira Yousaf Bug# EMR-1097
                            ClinicalRadiologyResultDetail.isSaved = true;
                            //End 16-05-2016 Edit By Humaira Yousaf Bug# EMR-1097

                            //Referesh both grids for order and results
                            var def = $.Deferred();
                            var resultDiv = " #dgvRadiologyResult";
                            var type = $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #lblLaboratory').text();
                            if (type == "External Facility") {
                                def = Clinical_RadiologyOrder.externalRadiologyResultsSearch(null, null, null, "RadiologyResultDetail");
                                $('#' + Clinical_RadiologyOrder.params.PanelID + ' #ulRadilogyResultTypeTabsItems a[href="#RadiologyExternalResult"]').trigger('click');
                                resultDiv = " #dgvExternalRadiologyResult";
                                Clinical_RadiologyOrder.externalRadiologyOrderSearch(null, null, null, "RadiologyOrderDetail");
                            }
                            else {
                                def = Clinical_RadiologyOrder.radiologyResultsSearch(null, null, null, "RadiologyResultDetail");
                                $('#' + Clinical_RadiologyOrder.params.PanelID + ' #ulRadilogyResultTypeTabsItems a[href="#RadiologyInternalResult"]').trigger('click');
                                Clinical_RadiologyOrder.radiologyOrderSearch(null, null, null, "RadiologyOrderDetail");
                            }

                            $.when(def).then(function () {
                                if (ClinicalRadiologyResultDetail.params.ParentCtrl == "Clinical_RadiologyOrder" && Clinical_RadiologyOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                                    $("#pnlClinicalProgressNote #pnlClinicalRadiologyOrder" + resultDiv + " input#" + response.RadiologyOrderResultId).prop("checked", true);
                                }
                            });
                            // Clinical_RadiologyOrder.radiologyOrderSearch(null, null, null, "RadiologyOrderDetail");

                            if (sender == "Acknowledge") {
                                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #btnAddRadiologyResult").text("View Result");
                            }
                            else {
                                $('#' + ClinicalRadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrderDetail #btnAddRadiologyResult").text("Edit Result");
                            }

                            if (sender == 'signprintorder') {
                                ClinicalRadiologyResultDetail.printRadiologyResult();
                            }
                            else {
                                ClinicalRadiologyResultDetail.UnLoad(null, null, 'saveExit');
                                $('#FaceSheetPager .slick-track').attr('style', 'opacity: 1; transform: translate3d(-57px, 0px, 0px); width: 1193px;');
                            }


                        }
                        else {
                            utility.DisplayMessages(response.message, 3);
                            //Start 16-05-2016 Edit By Humaira Yousaf Bug# EMR-1097
                            ClinicalRadiologyResultDetail.isSaved = false;
                            //End 16-05-2016 Edit By Humaira Yousaf Bug# EMR-1097
                        }
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });

        }
    },

    SaveFavListVal: function (FavListVal) {
        EMRUtility.insertUpdateFavListVal(ClinicalRadiologyResultDetail.FavListName, FavListVal);
    },

    //Author: Abid Ali
    //Date :  06-05-2016
    //Reason: Reset The Entire form
    radiologyOrderResultReset: function () {

        //Start 13-05-2016 Edit By Humaira Yousaf Bug# EMR-1072
        utility.myConfirm('22', function () {

            var self = '#' + ClinicalRadiologyResultDetail.params.PanelID + ' #frmClinicalRadiologyResultDetail';
            var $self = $(self);

            var $testInformation = $(self + ' #dgvRadiologyResultDetail');
            $(self + ' #divRadiologyResultInformation').resetAllControls(null);

            $(self + ' #ddlStatus').val("");
            $(self + ' #txtRemarks').val("");
            $(self + ' #txtComments').val("");

            $testInformation.resetAllControls(null);
            $(self + ' input[name=Result]').css("color", "green");

            utility.CreateDatePicker(ClinicalRadiologyResultDetail.params.PanelID + ' #frmClinicalRadiologyResultDetail input[id="txtOrderDate"]', function () {
            }, true);

            $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #frmClinicalRadiologyResultDetail input[id="txtOrderTime"]').timepicker({
                defaultTime: new Date()
            });

            if (ClinicalRadiologyResultDetail.params.mode == "Edit") {
                $self.bootstrapValidator('revalidateField', 'Status');

            }
            else {
                //revalidate the required fields
                $self.bootstrapValidator('revalidateField', 'LabId');
                $self.bootstrapValidator('revalidateField', 'Status');
                $self.bootstrapValidator('revalidateField', 'Provider');
                $self.bootstrapValidator('revalidateField', 'Facility');
            }
            //Disable form Buttons
            ClinicalRadiologyResultDetail.enableDisableOrderResultButtons();
            //End 13-05-2016 Edit By Humaira Yousaf Bug# EMR-1072
        }, function () { },
            '22'
        );
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: To check whether Radiology order exists
    checkRadiologyResultExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_RadiologyResults').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ObjectiveNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="RadiologyResultComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_RadiologyResults title="Radiology Order"  id="clinicalMenu_Orders_Lab" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'Radiology\',\'clinicalMenu_Orders_Lab\',' + Clinical_ProgressNote.params.NotesId + ');" title="Diagnostic Imaging Order">Diagnostic Imaging Order</a> ' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'Radiology Order\',\'clinicalMenu_Orders_Lab\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_RadiologyResults> </header></li>');
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
    //Description: To check whether Radiology order Test exists
    validateTest: function (cptCode, cptDiscription) {
        var isExists = false;
        $("#" + ClinicalRadiologyResultDetail.params.PanelID + " #dgvRadiologyResultDetail tbody tr:not([class*='child]'").each(function () {

            var rowJSON = ClinicalRadiologyResultDetail.getParentChildJson(this);
            //var testCPT = rowJSON['CPTCode'];
            var currentRowCPTDescription = rowJSON['CPTCodeDescription'];
            if (currentRowCPTDescription) {
                if (cptDiscription.toLowerCase().trim() == currentRowCPTDescription.toLowerCase()) {
                    isExists = true;
                    return false;
                }
            }
        });
        return isExists;
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: To show RadiologyResult Search in Popup
    openRadiologyResultAlert: function () {

        if ($(" #mainForm  li#RadiologyResultAlert span").text() != '' && $('#PatientProfile #hfPatientId').val() != '') {
            BackgroundLoaderShow(true);
            var params = [];


            params["FromAdmin"] = 0;
            //   params["StartupScreen"] = "message";
            LoadActionPan("Clinical_RadiologyResult", params);
        }
    },

    getRadiologyRowsJSON: function () {

        var selfRadiologyOrder = $("#" + ClinicalRadiologyResultDetail.params.PanelID + " #divRadiologyResultInformation");
        if (selfRadiologyOrder.find("#txtComment").text()) {
            selfRadiologyOrder.find("#txtComments").val(selfRadiologyOrder.find("#txtComment").text());
        }
        else {
            selfRadiologyOrder.find("#txtComments").val("");
        }
        var myJSONRadiologyOrder = selfRadiologyOrder.getMyJSONByName();
        var RadiologyTestIds = $("#" + ClinicalRadiologyResultDetail.params.PanelID + " #dgvRadiologyResultDetail tbody tr:not([id*=Child]").map(function () {
            return this.id.replace("id", "");
        }).get().join(',');

        $("#" + ClinicalRadiologyResultDetail.params.PanelID + " #dgvRadiologyResultDetail tbody tr:not([id*=child]").each(function (i, item) {
            var rowObject = new Object();


            rowObject["RadiologyOrderTestId" + $(item).attr("id")] = $(item).attr("id");
            rowObject["CPTCode" + $(item).attr("id")] = $(item).attr("CPTCode");
            rowObject["CPTDescription" + $(item).attr("id")] = $(item).attr("CPTDescription");
            rowObject["dtpRadiologyDate" + $(item).attr("id")] = $(item).find('td input[id^=dtpDate]').val();
            rowObject["tpRadiologyTime" + $(item).attr("id")] = $(item).find('td input[id^=tpTime]').val();
            rowObject["RadiologyProcedure" + $(item).attr("id")] = $(item).attr("CPTDescription");
            var currentRowJSON = JSON.stringify(rowObject);
            myJSONRadiologyOrder = utility.MergeJSON(myJSONRadiologyOrder, currentRowJSON);
        });
        //Hard coded for Radiology Result Order Creation
        var objRad = new Object();
        if (ClinicalRadiologyResultDetail.params.RadiologyOrderId && ClinicalRadiologyResultDetail.params.RadiologyOrderId > 0) {
            objRad["RadiologyOrderId"] = ClinicalRadiologyResultDetail.params.RadiologyOrderId;
        } else {
            objRad["RadiologyOrderId"] = '-1';
        }
        objRad["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objRad["RadiologyTestIds"] = RadiologyTestIds;
        objRad["Status"] = 'Signed';
        var myJSON = JSON.stringify(objRad);
        myJSONRadiologyOrder = utility.MergeJSON(myJSON, myJSONRadiologyOrder);
        return myJSONRadiologyOrder;
    },


    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: get Latest Radiology Order By PatientId
    getLatestRadiologyResultByPatientId: function () {
        var strMessage = '';
        //   AppPrivileges.GetFormPrivileges("Notes_Notes", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            ClinicalRadiologyResultDetail.getLatestRadiologyResultByPatientIdDBCall().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    ClinicalRadiologyResultDetail.createRadiologyResultBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML');
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
    //Description: To create Radiology Order's Body HTML
    createRadiologyResultBodyHTML: function (response, NoteHTMLCtrl, UnloadRadiologyResult) {
        ClinicalRadiologyResultDetail.checkRadiologyResultExists();
        if (response.RadiologyResultFill_JSON != null && response.RadiologyResultFill_JSON != '') {
            var RadiologyResultFill_Obj = JSON.parse(response.RadiologyResultFill_JSON);
            var $mainDivRadiologyResult = $(document.createElement('div'));

            var RadiologyResultId = RadiologyResultFill_Obj.RadiologyResultId;
            if (RadiologyResultId > 0) {
                var $SectionBodyRadiologyResult = $(document.createElement('section'));
                $SectionBodyRadiologyResult.attr('id', "Cli_RadiologyResultDetail_Main" + RadiologyResultId);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_RadiologyResultDetail_" + RadiologyResultId);
                var $ListRadiologyResult = $(document.createElement('ul'));

                $ListRadiologyResult.attr('class', 'list-unstyled')

                $SectionBodyRadiologyResult.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_RadiologyResultDetail_" + RadiologyResultId + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_RadiologyResultDetail_Main" + RadiologyResultId + '"  ><i class="fa fa-times"></i></a></div> ');


                $ListRadiologyResult.append("<li>" + RadiologyResultFill_Obj.SoapText + "</li>");
                $DetailsDiv.append($ListRadiologyResult);
                $SectionBodyRadiologyResult.append($DetailsDiv);
                if ($(NoteHTMLCtrl + ' clinical_RadiologyResults').parent().parent().find('#Cli_RadiologyResultDetail_Main' + RadiologyResultId).length == 0) {
                    $mainDivRadiologyResult.append($SectionBodyRadiologyResult);
                    ClinicalRadiologyResultDetail.updateRadiologyResultHtml($mainDivRadiologyResult.html(), RadiologyResultId, NoteHTMLCtrl);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' clinical_RadiologyResults').parent().parent().find('#Cli_RadiologyResultDetail_Main' + RadiologyResultId + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' clinical_RadiologyResults').parent().parent().find('#Cli_RadiologyResult_Main' + RadiologyResultId + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' clinical_RadiologyResults').parent().parent().find('#Cli_RadiologyResultDetail_Main' + RadiologyResultId).html($SectionBodyRadiologyResult.html());
                    $(NoteHTMLCtrl + ' clinical_RadiologyResults').parent().parent().find('#Cli_RadiologyResultDetail_Main' + RadiologyResultId + ' ul').append(CommentHTML);
                    Clinical_ProgressNote.saveComponentSOAPText('Radiology Result');
                    ClinicalRadiologyResultDetail.updateRadiologyResultHtml("", RadiologyResultId, NoteHTMLCtrl);

                }

                if (UnloadRadiologyResult == true) {
                    ClinicalRadiologyResultDetail.Unload(ClinicalRadiologyResultDetail.bNextPrev);
                }
            }
        }
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: To detach Components Radiology Order
    detach_ComponentsRadiologyResult: function (ComponentName, IsUpdate, RadiologyResultComponentRemove) {
        var RadiologyResultIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_RadiologyResults').parent().parent().find('section[id*="Cli_RadiologyResultDetail_Main"]').map(function () {
            return this.id.replace("Cli_RadiologyResultDetail_Main", "");
        }).get().join(',');
        var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .RadiologyResultComponent').attr('NoteComponentId');

        if (RadiologyResultIds == "" || RadiologyResultIds == "undefined") {
            $.when(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId)).then(function () {
                Clinical_ProgressNote.ShowHideComponetsHeaders();
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
            });
        }
        else {
            ClinicalRadiologyResultDetail.detachRadiologyResultFromNotesDBCall(RadiologyResultIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (IsUpdate) {

                        Clinical_ProgressNote.saveComponentSOAPText('Radiology Result');
                    }
                    utility.DisplayMessages(response.Message, 1);
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        if (RadiologyResultComponentRemove) {
            if (NoteComponentId && NoteComponentId != "NCDummyId") {
                $.when(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId)).then(function () {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Radiology Orders']").remove();
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_RadiologyResults').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
            else {
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Radiology Orders']").remove();
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_RadiologyResults').parent().parent().remove();
            }
        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_RadiologyResults').parent().parent().find('section[id*="Cli_RadiologyResultDetail_Main"]').remove();
        }
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: To detach Radiology Order from Notes
    detachRadiologyResultFromNotes: function (RadiologyResultId) {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('1', function () {
                var selectedValue = RadiologyResultId.replace('Cli_RadiologyResultDetail_Main', '');
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    ClinicalRadiologyResultDetail.detachRadiologyResultFromNotes_DBCall(selectedValue).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            $('#' + RadiologyResultId).remove();

                            Clinical_ProgressNote.saveComponentSOAPText('Radiology Result');
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
    //Description: DB Call to detach Radiology Order from Notes
    detachRadiologyResultFromNotes_DBCall: function (RadiologyResultId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["RadiologyResultId"] = RadiologyResultId;
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
        objData["commandType"] = "detach_RadiologyResult_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "RadiologyResult", "RadiologyResult");
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: DB Call to detach Radiology Order From Notes
    detachRadiologyResultFromNotesDBCall: function (RadiologyId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["RadiologyResultId"] = RadiologyId;
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
        objData["commandType"] = "detach_RadiologyResult_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "RadiologyResult", "RadiologyResult");
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: To update Radiology Order Html
    updateRadiologyResultHtml: function (RadiologyResultHtml, RadiologyResultId, NoteHTMLCtrl) {
        $(NoteHTMLCtrl + ' clinical_RadiologyResults').parent().parent().addClass('initialVisitBody');
        if (RadiologyResultHtml != '') {
            $(NoteHTMLCtrl + ' clinical_RadiologyResults').parent().parent().append(RadiologyResultHtml);
        }
        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (RadiologyResultHtml != '') {
            ClinicalRadiologyResultDetail.attachRadiologyResultWithNotes(RadiologyResultId);
        }

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: To attach Radiology Order With Notes
    attachRadiologyResultWithNotes: function (RadiologyResultId) {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {

            var selectedValue = RadiologyResultId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                ClinicalRadiologyResultDetail.attachRadiologyResultWithNotesDBCall(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        //If Attached MedicalHx Made new inseration to MedicalHx Table than good ids should be attached to HTML
                        Clinical_ProgressNote.saveComponentSOAPText('Radiology Result');
                        $('#' + RadiologyResultId).remove();
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
    //Description: DB Call to attach Radiology Order With Notes
    attachRadiologyResultWithNotesDBCall: function (RadiologyResultId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["RadiologyResultId"] = RadiologyResultId;
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
        objData["commandType"] = "attach_RadiologyResult_with_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "RadiologyResult", "RadiologyResult");
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: DB Call to get Latest Radiology Order By PatientId
    getLatestRadiologyResultByPatientIdDBCall: function () {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["commandType"] = "getlatest_RadiologyResultby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyResult", "RadiologyResult");
    },


    //Author: Humaira Yousaf
    //Date :  02-05-2016
    //Description: Creates PDF to view Radiology Order
    printRadiologyResult: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["UserId"] = globalAppdata['AppUserId'];
        params["PatientId"] = $('#PatientProfile #hfPatientId').val();
        params["ParentCtrl"] = "ClinicalRadiologyResultDetail";
        params["RadiologyResultId"] = $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #hfRadiologyResultId").val();
        params["RadiologyOrderId"] = $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #hfRadiologyOrderId").val();
        LoadActionPan('Clinical_RadiologyResultView', params);
    },


    getRandomNumber: function (isNegative, upperLimit) {

        isNegative = isNegative == null ? false : isNegative;
        upperLimit = upperLimit == null ? 1000 : upperLimit;

        var randomNumber = (1 + Math.floor(Math.random() * upperLimit));

        if (isNegative)
            randomNumber = randomNumber * -1;

        return randomNumber;
    },

    //For Adding color to result value in grid
    highLightResultValue: function (obj) {

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
            case 'low':
                $result.css("color", "orange");
                break;
            case 'abnormally low':
                $result.css("color", "orange");
                break;
            default:
                $result.css("color", "black");
        }
    },


    //Author: Abid Ali
    //Date :  07-06-2016
    //Description: checks radiology result in parent row passed
    isRadiologyResultExists: function (currentRow, objJson) {

        var childExists = false;
        var currentLOINICCODE = objJson['LOINICCODE'];
        var currentLOINCDescription = objJson['LOINICDescription'].trim();

        //Concat if both has values
        if (currentLOINICCODE != null && currentLOINICCODE != "")
            currentLOINCDescription = currentLOINICCODE + " - " + currentLOINCDescription;


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
    addRadiologyResultRow: function (item, cptCode, cptDescription) {

        var $row = $('<tr/>');
        if (item != null) {
            $row.attr("id", item.RadiologyOrderResultDetailId).addClass('parent-row');

            $row.attr("CPTCode", item.CPTCode);
            $row.attr("CPTDescription", item.CPTCodeDescription);

            if (item.ObservationDate != null) {
                currentDate = new Date(item.ObservationDate).toLocaleDateString();
                currentTime = new Date(item.ObservationDate).toLocaleTimeString().replace(/([\d]+:[\d]{2})(:[\d]{2})(.*)/, "$1$3");
            }
            else {
                currentDate = new Date().toLocaleDateString();
                currentTime = new Date().toLocaleTimeString().replace(/([\d]+:[\d]{2})(:[\d]{2})(.*)/, "$1$3");
            }

            //currentDate = currentDate + " " + currentTime;
            var expandCollapseIcon = '<a href="#" class="on-editing expand-row" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a>';
            var addRowIcon = '<a href="#" class="on-child-add add-child-row" title="Add Child Record">Add Observation</a>';
            if (ClinicalRadiologyResultDetail.params.FromRadiologyDetail || ClinicalRadiologyResultDetail.params.mode == "Edit") {
                var removeIcon = '<a href="#" onclick="ClinicalRadiologyResultDetail.removeRadiologyResultParent(this);"  title="Delete Record" class="disableAll"><i class="fa fa-close red"></i></a>';
            }
            else {
                var removeIcon = '<a href="#" onclick="ClinicalRadiologyResultDetail.removeRadiologyResultParent(this);"  title="Delete Record"><i class="fa fa-close red"></i></a>';
            }
            var controlwidth = "size65";

            var controldisabledClass = "disabled";
            var calendars = '<div class="input-group ' + controlwidth + '"><span class="input-group-addon pl-tiny pr-tiny  xxs-font"><i class="fa fa-calendar"></i></span><input id="dtpDate' + ClinicalLabResultDetail.getRandomNumber(true, 1000) + '" name="Date" ' + controldisabledClass + ' type="text" class="form-control ' + controlwidth + ' p-tiny" value="' + currentDate + '" /></div>';
            var Time = '<div class="input-group size50"><span class="input-group-addon pl-tiny pr-tiny  xxs-font"><i class="fa fa-clock-o"></i></span><input id="tpTime' + ClinicalLabResultDetail.getRandomNumber(true, 1000) + '" name="Time" ' + controldisabledClass + ' type="text" class="form-control size60 p-tiny" value="' + currentTime + '" data-plugin-timepicker /></div>';



            $row.append('<td>' + expandCollapseIcon + " " + removeIcon + '</td><td>' + calendars + '</td><td>' + Time + '</td><td colspan="2" >' + item.CPTCode + " " + item.CPTCodeDescription + '</td><td colspan="3">' + addRowIcon + '</td></td><td style="display: none;"></td><td style="display: none;"></td>');
            $("#" + ClinicalRadiologyResultDetail.params.PanelID + " #dgvRadiologyResultDetail tbody").last().append($row);
            ClinicalRadiologyResultDetail.SetDateTimeControl($row, false);
        }
        else {
            //Add random negative Id to parent row


            $row.attr("id", ClinicalRadiologyResultDetail.getRandomNumber(true, 1000)).addClass('parent-row');

            $row.attr("CPTCode", cptCode);

            //Save Tests as CPTDescription
            // if (cptCode != null && cptCode != "") {
            //    $row.attr("CPTDescription",cptDescription);
            // }
            // else {
            $row.attr("CPTDescription", cptDescription.trim());
            // }

            currentDate = new Date().toLocaleDateString();
            currentTime = new Date().toLocaleTimeString().replace(/([\d]+:[\d]{2})(:[\d]{2})(.*)/, "$1$3");

            var expandCollapseIcon = '<a style="display:none" href="#" class="on-editing expand-row" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a>';
            var addRowIcon = '<a href="#" class="on-child-add add-child-row" title="Add Child Record">Add Observation</a>';

            if (ClinicalRadiologyResultDetail.params.FromRadiologyDetail) {
                var removeIcon = '<a href="#" onclick="ClinicalRadiologyResultDetail.removeRadiologyResultParent(this);"  title="Delete Record" class="disableAll"><i class="fa fa-close red"></i></a>';
            }
            else {
                var removeIcon = '<a href="#" onclick="ClinicalRadiologyResultDetail.removeRadiologyResultParent(this);"  title="Delete Record"><i class="fa fa-close red"></i></a>';
            }
            var controlwidth = "size65";

            var controldisabledClass = "";
            var calendars = '<div class="input-group ' + controlwidth + '"><span class="input-group-addon pl-tiny pr-tiny  xxs-font"><i class="fa fa-calendar"></i></span><input id="dtpDate' + ClinicalLabResultDetail.getRandomNumber(true, 1000) + '" name="Date" ' + controldisabledClass + ' type="text" class="form-control ' + controlwidth + ' p-tiny" value="' + currentDate + '" /></div>';
            var Time = '<div class="input-group size50"><span class="input-group-addon pl-tiny pr-tiny  xxs-font"><i class="fa fa-clock-o"></i></span><input id="tpTime' + ClinicalLabResultDetail.getRandomNumber(true, 1000) + '" name="Time" ' + controldisabledClass + ' type="text" class="form-control size60 p-tiny" value="' + currentTime + '" data-plugin-timepicker /></div>';


            $row.append('<td>' + expandCollapseIcon + " " + removeIcon + '</td><td>' + calendars + '</td><td>' + Time + '</td><td colspan="2">' + cptCode + " " + $row.attr("CPTDescription") + '</td><td colspan="3">' + addRowIcon + '</td><td style="display: none;"></td><td style="display: none;"></td><td style="display: none;"></td>');
            $("#" + ClinicalRadiologyResultDetail.params.PanelID + " #dgvRadiologyResultDetail tbody").last().append($row);
            ClinicalRadiologyResultDetail.SetDateTimeControl($row, false);
        }

        return $row;
    },

    deleteRadiologyResultsDetail_DbCall: function (RadiologyResultDetailId) {

        if (RadiologyResultDetailId < 0) {
            RadiologyResultDetailId = 0;
        }

        var objData = {};

        objData["RadiologyOrderResultDetailId"] = RadiologyResultDetailId;

        objData["commandType"] = "delete_Radiologyresultdetail";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyResult", "RadiologyResult");
    },
    removeRadiologyResultParent: function (obj) {

        var labOrderTestId = $(obj).parent().parent().attr('id');
        utility.myConfirm('1', function () {
            if (labOrderTestId > 0) {


                ClinicalRadiologyResultDetail.deleteRadiologyResults_DbCall(labOrderTestId).done(function (response) {

                    response = JSON.parse(response);
                    if (response.status != false) {
                        var tr = $(obj).closest('tr');
                        var table = $(" #dgvRadiologyResultDetail").DataTable();
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
                var table = $(" #dgvRadiologyResultDetail").DataTable();
                var row = table.row(tr);
                row.child() != null ? row.child().remove() : true;
                tr.remove();
                utility.DisplayMessages("Successfully Deleted", 1);
            }
        }, function () {

        }, '1');

    },

    deleteRadiologyResults_DbCall: function (RadiologyOrderTestId) {


        var objData = {};

        objData["RadiologyOrderTestId"] = RadiologyOrderTestId;

        objData["commandType"] = "delete_radiologytest";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyResult", "RadiologyResult");
    },

    //For loading child rows
    buildRowChild: function (CurrentRow, ChildItems) {

        var currentDate = new Date();
        var currentTime = currentDate.toLocaleTimeString().replace(/([\d]+:[\d]{2})(:[\d]{2})(.*)/, "$1$3");
        currentDate = currentDate.toLocaleDateString();

        var CurrentRowchilds = $();

        if (ChildItems != null && ChildItems.length > 0) {


            $.each(ChildItems, function (i, item) {

                //Push values as attributes of row
                var $row = $('<tr/>').addClass("childRow-bg");
                $row.attr("child-id", item.RadiologyOrderResultDetailId);
                $row.attr('LOINICCODE', item.LOINC);
                $row.attr('LOINICDescription', item.LOINCDescription);

                $row.attr('CPTSNOMEDCodeId', item.CPTSNOMEDCodeId);
                $row.attr('CPTSNOMEDDescription', item.CPTSNOMEDDescription);

                $row.attr("RadiologyOrderResultId", item.RadiologyOrderResultId);

                // item.Observation = item.LOINC + " " + item.LOINCDescription;

                item.Observation = item.LOINCDescription;

                //Convert Date to prper date time format
                if (item.ObservationDate != null) {
                    currentDate = new Date(item.ObservationDate).toLocaleDateString();
                    currentTime = new Date(item.ObservationDate).toLocaleTimeString().replace(/([\d]+:[\d]{2})(:[\d]{2})(.*)/, "$1$3");
                }
                //currentDate = currentDate + " " + currentTime;
                var flagOptions = "<option value='Abnormal'>Abnormal</option>" +
                             "<option value='Critical High'>Critical High</option>" +
                               "<option value='Critical Low'>Critical Low</option>" +
                                      "<option value='High'>High</option>" +
                                       "<option value='Low'>Low</option>" +
                                         "<option value='Negative'>Negative</option>" +
                                         "<option value='Osteoarthritis'>Osteoarthritis</option>" +
                                         "<option value='Positive'>Positive</option>" +
                                         "<option value='Rheumatoid Arthritis'>Rheumatoid Arthritis</option>" +
                                         "<option value='Stable'>Stable</option>";

                var rowRemove = '<a href="#" class="btn-xs on-default remove-row mr-none btn" title="Delete Record"><i class="fa fa-close red"></i></a>';
                var ddlFlag = "<select onchange='ClinicalRadiologyResultDetail.highLightResultValue(this);' id='ddlFlag" + item.RadiologyOrderResultDetailId + "' name='Flag' class='form-control'>" + flagOptions + "</select>";
                //var result = "<input type='number' min='0' class='form-control' onblur='ClinicalRadiologyResultDetail.validateResult(this,event)'  id='txtResult" + item.RadiologyOrderResultDetailId + "'  name='Result'></input>";
                var UOM = "<input type='text' class='form-control' id='txtUoM" + item.RadiologyOrderResultDetailId + "' name='UoM'></input>";
                //var range = "<input type='text' class='form-control' onkeypress = 'ClinicalRadiologyResultDetail.checkRange(event)' onblur='ClinicalRadiologyResultDetail.validateRange(this,event)' id='txtRange" + item.RadiologyOrderResultDetailId + "' name='Range'></input>";
                var Remarks = "<input maxlength='500' type='text' class='form-control' id='txtRemarks" + item.RadiologyOrderResultDetailId + "' name='Remarks'></input>";
                var controlwidth = "size65";
                var Timecontrolwidth = "size45";
                var controldisabledClass = "";
                var calendars = '<div class="input-group ' + controlwidth + '"><span class="input-group-addon pl-tiny pr-tiny  xxs-font"><i class="fa fa-calendar"></i></span><input id="Child_dtpDateChild' + ClinicalLabResultDetail.getRandomNumber(true, 1000) + '" name="Date" ' + controldisabledClass + ' type="text" class="form-control ' + controlwidth + ' p-tiny" value="' + currentDate + '" /></div>';
                var Time = '<div class="input-group size50"><span class="input-group-addon pl-tiny pr-tiny  xxs-font"><i class="fa fa-clock-o"></i></span><input id="Child_tpTime' + ClinicalLabResultDetail.getRandomNumber(true, 1000) + '" name="Time" ' + controldisabledClass + ' type="text" class="form-control size60 p-tiny" value="' + currentTime + '" data-plugin-timepicker /></div>';

                $row.append('<td>' + rowRemove + '</td><td>' + calendars + '</td><td>' + Time + '</td><td>' + item.Observation + Clinical_InfoButtonView.GenerateInfoLink(item.LOINC, "ClinicalRadiologyResultDetail", 2) + '</td><td>' + UOM + '</td><td>' + ddlFlag + '</td><td>' + Remarks + '</td>');

                //Bind JSON to the child item
                utility.bindMyJSONByName(true, item, false, $row).done(function () {

                    $row.find('select').trigger('onchange');
                    $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").data('serialize', $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").serialize());
                });

                CurrentRowchilds = CurrentRowchilds.add($row);

            });
        }
        return CurrentRowchilds;
    },

    //For adding new chid rows
    addNewResultChildRow: function (currentRow, objJson) {

        if (!ClinicalRadiologyResultDetail.isRadiologyResultExists(currentRow, objJson)) {
            var currentDate = new Date();
            var observationVal = "";
            if (objJson != null) {

                if (objJson['LOINICCODE'] != "" && objJson['LOINICCODE'] != null) {

                    observationVal = objJson['LOINICCODE'] + " - ";
                }
                observationVal = observationVal + objJson["Observation"];
            }

            var currentTime = currentDate.toLocaleTimeString().replace(/([\d]+:[\d]{2})(:[\d]{2})(.*)/, "$1$3");
            currentDate = currentDate.toLocaleDateString().replace('///g', '-');

            var flagOptions = "<option value='Abnormal'>Abnormal</option>" +
                             "<option value='Critical High'>Critical High</option>" +
                               "<option value='Critical Low'>Critical Low</option>" +
                                      "<option value='High'>High</option>" +
                                       "<option value='Low'>Low</option>" +
                                         "<option value='Negative'>Negative</option>" +
                                         "<option value='Osteoarthritis'>Osteoarthritis</option>" +
                                         "<option value='Positive'>Positive</option>" +
                                         "<option value='Rheumatoid Arthritis'>Rheumatoid Arthritis</option>" +
                                         "<option value='Stable'>Stable</option>";

            var currentRowchild = $();
            var LOINICCODE = objJson['LOINICCODE'];
            var LOINCDescription = objJson['LOINICDescription'];
            var snomedCode = objJson['CPTSNOMEDCodeId'];
            var snomedDesc = objJson['CPTSNOMEDDescription'];

            var randomNumber = ClinicalRadiologyResultDetail.getRandomNumber(true, 1000);
            var $row = $('<tr/>').addClass("childRow-bg").attr('child-id', randomNumber);
            //push value as attribute into child row
            $row.attr('LOINICCODE', LOINICCODE)

            $row.attr('CPTSNOMEDCodeId', snomedCode)
            $row.attr('CPTSNOMEDDescription', snomedDesc)

            if (LOINICCODE != null && LOINICCODE != "")
                $row.attr('LOINICDescription', LOINICCODE + " - " + LOINCDescription);
            else
                $row.attr('LOINICDescription', LOINCDescription.trim());

            $row.attr("RadiologyOrderResultId", randomNumber);

            var ddlFlag = "<select  onchange='ClinicalRadiologyResultDetail.highLightResultValue(this);' id='ddlFlag" + randomNumber + "' name='Flag' class='form-control'>" + flagOptions + "</select>";
            //var result = "<input type='number' min='0' class='form-control' onblur='ClinicalRadiologyResultDetail.validateResult(this,event)'  id='txtResult" + randomNumber + "'  name='Result'></input>";
            var UOM = "<input type='text' class='form-control' id='txtUoM" + randomNumber + "' name='UoM'></input>";
            var Remarks = "<input maxlength='500' type='text' class='form-control' id='txtRemarks" + randomNumber + "' name='Remarks'></input>";
            //var range = "<input type='text' class='form-control'  onkeypress = 'ClinicalRadiologyResultDetail.checkRange(event)' onblur='ClinicalRadiologyResultDetail.validateRange(this,event)' id='txtRange" + randomNumber + "' name='Range'></input>";
            var Observation = "<label class='control-label' name='Observation'>" + observationVal + "</label>";

            var rowRemove = '<a href="#" class="btn-xs on-default remove-row mr-none btn" title="Delete Record"><i class="fa fa-close red"></i></a>';

            var controlwidth = "size65";
            var controldisabledClass = "";
            var Id = ClinicalLabResultDetail.getRandomNumber(true, 1000);
            var calendars = '<div class="input-group ' + controlwidth + '"><span class="input-group-addon pl-tiny pr-tiny  xxs-font"><i class="fa fa-calendar"></i></span><input id="Child_dtpDateChild' + Id + '" name="Date" ' + controldisabledClass + ' type="text" class="form-control ' + controlwidth + ' p-tiny" value="' + currentDate + '" /></div>';
            var Time = '<div class="input-group size50"><span class="input-group-addon pl-tiny pr-tiny  xxs-font"><i class="fa fa-clock-o"></i></span><input id="Child_tpTime' + Id + '" name="Time" ' + controldisabledClass + ' type="text" class="form-control size60 p-tiny" value="' + currentTime + '" data-plugin-timepicker /></div>';

            $row.append('<td>' + rowRemove + '</td><td>' + calendars + '</td><td>' + Time + '</td><td>' + Observation + Clinical_InfoButtonView.GenerateInfoLink(LOINICCODE, "ClinicalRadiologyResultDetail", 2) + '</td><td>' + UOM + '</td><td>' + ddlFlag + '</td><td>' + Remarks + '</td>');

            $row.find('select').trigger('onchange');

            return currentRowchild = currentRowchild.add($row);
        }
        return null;

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
        var childRow = ClinicalRadiologyResultDetail.openLOINCSearch(row, $row);
    },

    rowRemove: function ($row, obj) {
        utility.myConfirm('1', function () {

            var _self = obj;
            //Logic For removing child row from datatable
            if ($row.hasClass('childRow-bg')) {

                var childId = $row.attr("child-id");
                ClinicalRadiologyResultDetail.deletedChildRows.push(childId);
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
                $(htmlChilds).each(function () {
                    if ($(this).attr('child-id') != $row.attr('child-id')) {
                        childs = childs.add($(this));
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
            ClinicalRadiologyResultDetail.enableDisableOrderResultButtons();
            ClinicalRadiologyResultDetail.deleteRadiologyResultsDetail_DbCall(childId).done(function (response) {
                response = JSON.parse(response);
                if (response.status) {
                    utility.DisplayMessages(response.Message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 2);
                }
            });

            ClinicalRadiologyResultDetail.SetDateTimeControl($("#" + ClinicalRadiologyResultDetail.params.PanelID + ' #dgvRadiologyResultDetail tbody'), true);
        }, function () {
        },
                    '1'
    );

    },

    enableRemoveRow: function (CurrentRow) {
        CurrentRow.find("td.actions .remove-row").removeClass("hidden");

    },

    //End//---------- Grid Data -----------


    //----------Db Calls--------------

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Db Call for loading RadiologyResults
    loadRadiologyResults_DbCall: function (RadiologyResultId, RadiologyOrderId) {

        if (RadiologyResultId < 0) {
            RadiologyResultId = 0;
        }
        if (RadiologyOrderId < 0) {

            RadiologyOrderId = 0;
        }
        var objData = {};

        objData["RadiologyResultId"] = RadiologyResultId;
        objData["RadiologyOrderId"] = RadiologyOrderId;

        // objData["PatientId"] = Clinical_RadiologyOrder.patientId;

        objData["commandType"] = "fill_RadiologyResult";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyResult", "RadiologyResult");
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Saves RadiologyResult
    //Params: RadiologyResultData
    saveRadiologyResult: function (RadiologyResultData) {
        var objData = JSON.parse(RadiologyResultData);
        //if (objData.Comments) {
        //    var commentlength = objData.Comments.length;
        //    if (commentlength > 2000) {
        //        objData.Comments = objData.Comments.substring(0, 2000);
        //    }
        //}
        if (objData["Assignee"] == "")
            objData["AssigneeId"] = "";

        objData["commandType"] = "save_RadiologyResult";
        if (ClinicalRadiologyResultDetail.params.ParentCtrlPanelID == "pnlClinicalProgressNote #pnlClinicalRadiologyOrder") {
            objData["NoteId"] = $("#pnlClinicalProgressNote #hfNoteId").val();
        } else {
            objData["NoteId"] = "";
        }
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyResult", "RadiologyResult");
    },

    //----------Db Calls--------------


    //Function Name: loadAttachments
    //Author: Humaira Yousaf
    //Date :  02-05-2016
    //Description: Loads Attachments // modified by Abid
    loadAttachments: function (controlName, radiologyOrderId, resultId, tableId) {

        ClinicalRadiologyResultDetail.loadAttachments_DbCall(radiologyOrderId).done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {

                var ulAttachment = null;

                if (controlName == null)
                    ulAttachment = $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #menuViewAttachment');
                else {
                    var control = eval(controlName.trim());
                    if (tableId != null) {

                        ulAttachment = $('#' + control.params.PanelID + " #" + tableId.trim() + " tr td").find('div.dropdown').find("#menuViewAttachment" + resultId);
                        if ($('#' + control.params.PanelID + " #" + tableId.trim()).parent() != null) {

                            //show dropdown top on row
                            $('#' + control.params.PanelID + " #" + tableId.trim()).parent().removeClass('Of-a');
                        }
                    }
                }
                $(ulAttachment).empty();
                if (response.OrderResultAttachmentCount > 0) {
                    var attachments = JSON.parse(response.OrderResultAttachmentLoad_JSON);

                    $(attachments).each(function (index, item) {
                        if (controlName == null)
                            $(ulAttachment).append('<li><a href="#" id="' + item.PatDocId + '" onclick="ClinicalRadiologyResultDetail.showAttachment(\'' + item.PatDocId + '\',event)">' + item.ModifiedOn + '</a></li>');
                        else {
                            var onClick = controlName.trim() + ".showAttachment";
                            $(ulAttachment).append('<li><a href="#" id="' + item.PatDocId + '" onclick="' + onClick + '(\'' + item.PatDocId + '\',event,this);">' + item.ModifiedOn + '</a></li>');
                        }
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
    //Date :  02-05-2016
    //Description: DbCall to Loads Attachments
    loadAttachments_DbCall: function (radiologyOrderId) {

        var objData = {};
        //Start 12-05-2016 Humaira Yousaf to load attachments against order
        //Attachments are now saved against RadiologyOrderId and not agaisnt RadiologyResultId
        if (radiologyOrderId == null) {

            radiologyOrderId = ClinicalRadiologyResultDetail.params.RadiologyOrderId;
            objData["TransitionId"] = radiologyOrderId == null || radiologyOrderId == "" ? -1 : radiologyOrderId;
        }
        else {
            objData["TransitionId"] = radiologyOrderId;
        }
        //End 12-05-2016 Humaira Yousaf to load attachments against order
        objData["RefModuleName"] = "Radiology Result";
        objData["PatientId"] = $("div#PatientProfile #hfPatientId").val();

        objData["commandType"] = "load_attachments";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyResult", "RadiologyResult");
    },

    //Function Name: showAttachment
    //Author: Humaira Yousaf
    //Date :  02-05-2016
    //Description: shows Attachments
    showAttachment: function (PatDocID, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Documents", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["PatientID"] = $('#PatientProfile #hfPatientId').val();
                params["PatDocID"] = PatDocID;
                params["mode"] = "Edit";
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = "ClinicalRadiologyResultDetail";
                params["ParentCtrlPanelID"] = "pnlClinicalRadiologyResultDetail";

                LoadActionPan('Document_Viewer', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    //Function Name: viewPdfRadiologyResult
    //Author: Humaira Yousaf
    //Date :  09-05-2016
    //Description: Views pdf
    viewPdfRadiologyResult: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["UserId"] = globalAppdata['AppUserId'];
        params["PatientId"] = $('#PatientProfile #hfPatientId').val();
        params["ParentCtrl"] = "ClinicalRadiologyResultDetail";
        params["RadiologyOrderId"] = ClinicalRadiologyResultDetail.params.RadiologyOrderId;
        params["RadiologyResultId"] = $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #hfRadiologyResultId").val();
        params["Caller"] = "viewpdf";
        LoadActionPan('Clinical_RadiologyResultView', params);
    },


    //Author: by Abid Ali
    //Date :  21-06-2016
    //Function Name: validateRange
    //Description: This function will validate the Range Field
    validateRange: function (obj, event) {
        //Regex changed by Abid Ali
        var test = /^[0-9]+\-[0-9]+$/;
        var value = $(obj).val(); //String.fromCharCode(event.keyCode);
        if (value != "") {

            if (value.match(test)) {
                var rangeValues = value.split('-'); //split on hypen char
                var lowerRange = parseFloat(rangeValues[0].trim());
                var upperRange = parseFloat(rangeValues[1].trim());

                if (lowerRange >= upperRange) {
                    event.preventDefault();
                    $(obj).val(""); //emtpy range value if does not match the pattern
                    setTimeout(function () {
                        utility.DisplayMessages("Please Enter Range in proper format Like (Lower bound - upper bound)", 3);
                    }, 100);
                    $(obj).css('border-color', 'red');
                    ClinicalRadiologyResultDetail.isError = true;
                    return false;
                }
                else {
                    var $row = $(obj).parent().parent();
                    $row.find('input[id*="txtResult"]').trigger('blur'); // dynamically validate result
                    ClinicalRadiologyResultDetail.isError = false;
                    $(obj).css({ 'border-color': '#ccc' });
                    return true;
                }
            }
            else {
                event.preventDefault();
                $(obj).val(""); //emtpy range value if does not match the pattern
                setTimeout(function () {
                    utility.DisplayMessages("Please Enter Range in proper format Like (0-9)", 3);
                }, 100);
                $(obj).css('border-color', 'red');
                ClinicalRadiologyResultDetail.isError = true;
                return false;
            }
        }
        else {
            event.preventDefault();
            $(obj).val(""); //emtpy range value if does not match the pattern
            utility.DisplayMessages("Please Enter Range in proper format Like (0-9)", 3);
            return false;
        }
    },
    //Edited by Abid Ali
    //Date :  07-12-2016
    //Description: This function will allow only digits and hypen in text box
    checkRange: function (event) {
        var test = /^[0-9]|\-$/;
        var code = event.keyCode || event.charCode || event.which;
        if (code === 8) {
            return;
        }
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
                resultValue = parseFloat(resultValue);
                if (rangeValue != "") {
                    var rangeValues = rangeValue.split('-'); //split on hypen char
                    var lowerRange = parseFloat(rangeValues[0].trim());
                    var upperRange = parseFloat(rangeValues[1].trim());
                    if (resultValue < lowerRange || resultValue > upperRange) {

                        event.preventDefault();
                        $(obj).val(""); // empty result value
                        setTimeout(function () {
                            utility.DisplayMessages("The Result Value falls outside the Range", 3);
                        }, 100);
                        $(obj).css('border-color', 'red');
                        ClinicalRadiologyResultDetail.isError = true;
                        return false;
                    }
                    else {
                        ClinicalRadiologyResultDetail.isError = false;
                        $(obj).css({ 'border-color': '#ccc' });
                    }
                }
            }
        }
    },

    favoriteListSearch: function () {
        ClinicalRadiologyResultDetail.searchFavoriteList_DBCall("RadiologyOrder", null, 1, 5000, true).done(function (response) {

            response = JSON.parse(response);

            var $ddl = $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #ddlFavoriteListRadiologyResult');
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
                    EMRUtility.getFavListValue(ClinicalRadiologyResultDetail.FavListName).done(function (response1) {
                        response1 = JSON.parse(response1);
                        if (response1.status != false) {
                            if (response1.favListVal != "" && response1.favListVal != "-1") {
                                if ($("#" + ClinicalRadiologyResultDetail.params.PanelID + " #ddlFavoriteListRadiologyResult option[value='" + response1.favListVal + "']").length > 0) {
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
                            $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").data('serialize', $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").serialize());
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                } else {
                    $ddl.trigger("onchange");
                }
                $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").data('serialize', $('#' + ClinicalRadiologyResultDetail.params.PanelID + " #frmClinicalRadiologyResultDetail").serialize());
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
        if (ClinicalRadiologyResultDetail.params.mode.toLowerCase() == "add") {
            objData["LabId"] = $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #ddlLabId option:selected').val();
        }
        else {
            objData["LabId"] = $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #hfLabId').val();
        }
        objData["ProviderId"] = $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #hfProvider').val();

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
    AutoSearchFavRadiologyResult: function () {
        utility.Keyupdelay(function () {
            ClinicalRadiologyResultDetail.loadfavoriteListContent();
        });
    },
    loadfavoriteListContent: function (obj, favListIds) {
        if ((typeof favListIds == typeof undefined || favListIds == null) && (typeof obj == typeof undefined || obj == null)) {
            obj = $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #ddlFavoriteListRadiologyResult');
        }
        var SearchData = $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #FavSearchBox').val();
        var $uL = $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #ulFavoriteListRadiologyResultContent');
        if (obj != null) {
            var selectedOption = $(obj).find("option:selected");
            var selectedOptionValue = selectedOption.attr("id");
            var divSelectAllFavorites = $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #divSelectAllRadiologyResultFavorites');
            if (selectedOptionValue > 0) {
                divSelectAllFavorites.removeClass("disableAll");

                ClinicalRadiologyResultDetail.favoriteList_CPTSearch(selectedOptionValue, null, SearchData);
            }
            else {
                $uL.empty();
                if (divSelectAllFavorites.hasClass("disableAll") == false) {
                    divSelectAllFavorites.addClass("disableAll");
                }
            }
        }
        else {
            if (favListIds != null) {
                $uL.empty();
                $.each(favListIds, function (index, item) {
                    ClinicalRadiologyResultDetail.favoriteList_CPTSearch(item, false);
                });
            }
        }
    },


    favoriteList_CPTSearch: function (FavoriteListId, isEmptyUl, SearchData) {
        var $uL = $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #ulFavoriteListRadiologyResultContent');
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
                        var onclick = 'ClinicalRadiologyResultDetail.BindRadiologyOrderResultGridItem(\'' + item.CPTCode + '\',\'' + String(item.CPTCodeDescription) + '\',\'' + String(item.CPTCodeDescription) + '\')';
                        var $li = $(document.createElement('li'));
                        $li.attr('onclick', onclick);
                        $li.attr('CPTCode', item.CPTCode);
                        $li.attr('procedureDescription', String(item.CPTCodeDescription));
                        $li.attr('cptDescription', String(item.CPTCodeDescription));
                        $li.append(item.CPTCode + " " + item.CPTCodeDescription);
                        $uL.append($li);
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
    selectAllFavoriteListContent: function () {
        var allTestAlreadySelected = false;
        $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #ulFavoriteListRadiologyResultContent li').each(function (i, item) {
            var cptCode = $(item).attr('CPTCode')
            var procDesc = $(item).attr('procedureDescription');
            var cptDescription = $(item).attr('cptDescription');
            var isTestAlreadySelected = ClinicalRadiologyResultDetail.isTestExists(procDesc);
            if (isTestAlreadySelected != true) {
                ClinicalRadiologyResultDetail.BindRadiologyOrderResultGridItem(cptCode, procDesc, cptDescription);
                allTestAlreadySelected = true;
            }
            else {
                //utility.DisplayMessages("Test is already selected", 2);
                // return false;
            }
        });
        if (!allTestAlreadySelected) {
            utility.DisplayMessages("All test are already selected", 2);
        }
    },
    setFavSearch: function () {
        var LabId = "";
        if (ClinicalRadiologyResultDetail.params.mode.toLowerCase() == "add") {
            LabId = $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #ddlLabId option:selected').val();
        }
        else {
            LabId = $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #hfLabId').val();
        }
        var ProviderId = $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #hfProvider').val();
        if (ProviderId != '' && LabId != '') {
            //$('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #txtRadiologyCPTCode').removeClass('disableAll');
            //$('#' + ClinicalRadiologyOrderDetail.params.PanelID + ' #favSectionDiv').removeClass('disableAll');
            ClinicalRadiologyResultDetail.favoriteListSearch();
        }
        else {
            var $ddl = $('#' + ClinicalRadiologyResultDetail.params.PanelID + ' #ddlFavoriteListRadiologyResult');
            $ddl.empty();
            $ddl.append($('<option/>', {
                value: "",
                html: "- Select -"
            }));
            $ddl.val("");
            $ddl.trigger("onchange");
        }
    },
    isTestExists: function (procDesc) {
        var currentRow = $("#" + ClinicalRadiologyResultDetail.params.PanelID + " #dgvRadiologyResultDetail tbody tr");
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
}