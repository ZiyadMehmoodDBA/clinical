Clinical_ImplantableDetail = {
    bIsFirstLoad: true,
    IsGMDNPNameList: false,
    IsDIList: false,
    EditableGrid: null,
    EditableGridUNK: null,
    params: [],
    IsEditableGrid: true,
    IsEditableGridUNK: true,
    Load: function (params) {
        Clinical_ImplantableDetail.params = params;
        var ImplantableDevicesPKId = Clinical_ImplantableDetail.params.ImplantableDevicesPKId;
        Clinical_ImplantableDetail.IsGMDNPNameList = false;
        Clinical_ImplantableDetail.IsDIList = false;
        Clinical_ImplantableDetail.IsEditableGrid = true;
        Clinical_ImplantableDetail.IsEditableGridUNK = true;

        if (Clinical_ImplantableDetail.params.PanelID != 'pnlClinicalImplantableDetail') {
            Clinical_ImplantableDetail.params.PanelID = Clinical_ImplantableDetail.params.PanelID + ' #pnlClinicalImplantableDetail';
        } else {
            Clinical_ImplantableDetail.params.PanelID = 'pnlClinicalImplantableDetail';
        }

        Clinical_ImplantableDetail.domReadyFunction();

        utility.CreateDatePicker(Clinical_ImplantableDetail.params.PanelID + ' #frmClinicalImplantableDetail #ImplantDate', function (ev) { }, true);
        utility.CreateDatePicker(Clinical_ImplantableDetail.params.PanelID + ' #frmClinicalImplantableDetail #ImplantDateUnknown', function (ev) { }, true);
        $('#UDI').attr('title', 'A Unique Device Identifier (UDI) is composed of two parts: Device Identifier (DI) and Production Identifier (PI). Entering a UDI will populate data from the Global UDI Database.');

        var self = $('#' + Clinical_ImplantableDetail.params.PanelID);
        self.loadDropDowns(true).done(function () {
            Clinical_ImplantableDetail.ValidateUDI();
            if (Clinical_ImplantableDetail.params.mode == "Edit") {
                Clinical_ImplantableDetail.FillDeviceForm(ImplantableDevicesPKId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status == true) {
                        Clinical_ImplantableDetail.bindMyJson(response);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else {
                $('#' + Clinical_ImplantableDetail.params.PanelID + " #divDialogLength").removeClass();
                $('#' + Clinical_ImplantableDetail.params.PanelID + " #divDialogLength").addClass('modal-dialog modal-dialog-sm');
                $('#' + Clinical_ImplantableDetail.params.PanelID + " #divUdiUnknownSpace").removeClass();
                $('#' + Clinical_ImplantableDetail.params.PanelID + " #divVerifySpace").removeClass();
                $('#' + Clinical_ImplantableDetail.params.PanelID + " #divDeviceDetailSpace").removeClass();
                $('#' + Clinical_ImplantableDetail.params.PanelID + " #divImplantationDetailsSpace").removeClass();
                $('#' + Clinical_ImplantableDetail.params.PanelID + " #divProcedureSpace").removeClass();
                $('#' + Clinical_ImplantableDetail.params.PanelID + " #divProcedureSpaceUNK").removeClass();

                $('#' + Clinical_ImplantableDetail.params.PanelID + " #divProcedures").hide();
                $('#' + Clinical_ImplantableDetail.params.PanelID + " #divProceduresUNK").hide();
                $('#' + Clinical_ImplantableDetail.params.PanelID + " #divVerify").hide();
                $('#' + Clinical_ImplantableDetail.params.PanelID + " #divImplantationDetails").hide();
                $('#' + Clinical_ImplantableDetail.params.PanelID + " #divUdiUnknown").hide();
                $('#' + Clinical_ImplantableDetail.params.PanelID + " #SaveButton").hide();
            }
        });
    },

    ValidateUDI: function () {
        $('#frmClinicalImplantableDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   UDI: {
                       group: '.col-xs-12',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   DeviceName: {
                       group: '.col-xs-4',
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
            var checkBtnPressed = $(e.target).data('bootstrapValidator').getSubmitButton();
            if ($(checkBtnPressed).attr("id") == "VerifyButton") {
                Clinical_ImplantableDetail.VerifyUDI();
            }
            else if ($(checkBtnPressed).attr("id") == "SaveButton")
                Clinical_ImplantableDetail.ImplantableDeviceSave();
        });
    },

    MakeEditableGridKnown: function () {
        var PanelProcedureGrid = "#" + Clinical_ImplantableDetail.params.PanelID + " #pnlImplantableDeviceProcedure_Result";
        var ProcedureGridId = "#" + Clinical_ImplantableDetail.params.PanelID + " #dgvProcedureImplantableDevice";
        $(ProcedureGridId + " tbody tr").remove();
        Clinical_ImplantableDetail.EditableGrid = EMRUtility.MakeEditableGrid(PanelProcedureGrid, ProcedureGridId, Clinical_ImplantableDetail, "0", false, false, false, false);
    },

    MakeEditableGridUnknown: function (){
        var PanelProcedureGridUNK = "#" + Clinical_ImplantableDetail.params.PanelID + " #pnlImplantableDeviceProcedureUNK_Result";
        var ProcedureGridIdUNK = "#" + Clinical_ImplantableDetail.params.PanelID + " #dgvProcedureImplantableDeviceUNK";
        $(ProcedureGridIdUNK + " tbody tr").remove();
        Clinical_ImplantableDetail.EditableGridUNK = EMRUtility.MakeEditableGrid(PanelProcedureGridUNK, ProcedureGridIdUNK, Clinical_ImplantableDetail, "0", false, false, false, false);
    },

    UdiKnown: function () {
        if (Clinical_ImplantableDetail.IsEditableGrid == true || $("#" + Clinical_ImplantableDetail.params.PanelID + " #dgvProcedureImplantableDevice tbody tr").find(".dataTables_empty").length > 0) {
            Clinical_ImplantableDetail.IsEditableGrid = false;
            Clinical_ImplantableDetail.MakeEditableGridKnown();
        }
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #divDialogLength").removeClass();
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #divDialogLength").addClass('modal-dialog modal-dialog-lg');
        
        if (Clinical_ImplantableDetail.params.mode == "Edit") { 
            Clinical_ImplantableDetail.EditableGrid = EMRUtility.MakeEditableGrid("#" + Clinical_ImplantableDetail.params.PanelID + " #pnlImplantableDeviceProcedure_Result", "#" + Clinical_ImplantableDetail.params.PanelID + " #dgvProcedureImplantableDevice", Clinical_ImplantableDetail, "0", false, false, false, false);
            $('#' + Clinical_ImplantableDetail.params.PanelID + " #divUdiUnknownSpace").removeClass();
            $('#' + Clinical_ImplantableDetail.params.PanelID + " #divUdiUnknown").hide();
            $('#' + Clinical_ImplantableDetail.params.PanelID + " #divVerifySpace").addClass('spacer15');
            $('#' + Clinical_ImplantableDetail.params.PanelID + " #divVerify").show();
            Clinical_ImplantableDetail.DeviceInactiveInEdit();
            Clinical_ImplantableDetail.DivsHideShowInVerification();
        }
        else {
            Clinical_ImplantableDetail.EditableGrid = EMRUtility.MakeEditableGrid("#" + Clinical_ImplantableDetail.params.PanelID + " #pnlImplantableDeviceProcedure_Result", "#" + Clinical_ImplantableDetail.params.PanelID + " #dgvProcedureImplantableDevice", Clinical_ImplantableDetail, "0", false, false, false, false);
            $('#' + Clinical_ImplantableDetail.params.PanelID + " #divUdiUnknownSpace").removeClass();
            $('#' + Clinical_ImplantableDetail.params.PanelID + " #divDeviceDetailSpace").removeClass();
            $('#' + Clinical_ImplantableDetail.params.PanelID + " #divImplantationDetailsSpace").removeClass();
            $('#' + Clinical_ImplantableDetail.params.PanelID + " #divProcedureSpace").removeClass();
            $('#' + Clinical_ImplantableDetail.params.PanelID + " #divProcedureSpaceUNK").removeClass();
            $('#' + Clinical_ImplantableDetail.params.PanelID + " #divUdiUnknown").hide();
            $('#' + Clinical_ImplantableDetail.params.PanelID + " #SaveButton").hide();
            $('#' + Clinical_ImplantableDetail.params.PanelID + " #divProcedures").hide();
            $('#' + Clinical_ImplantableDetail.params.PanelID + " #divProceduresUNK").hide();
            $('#' + Clinical_ImplantableDetail.params.PanelID + " #divVerifySpace").addClass('spacer15');
            $('#' + Clinical_ImplantableDetail.params.PanelID + " #divVerify").show();
        }
    },

    UdiUnknown: function () {
        if (Clinical_ImplantableDetail.IsEditableGridUNK == true || $("#" + Clinical_ImplantableDetail.params.PanelID + " #dgvProcedureImplantableDeviceUNK tbody tr").find(".dataTables_empty").length > 0) {
            Clinical_ImplantableDetail.IsEditableGridUNK = false;
            Clinical_ImplantableDetail.MakeEditableGridUnknown();
        }

        if (Clinical_ImplantableDetail.params.mode == "Edit") {
            Clinical_ImplantableDetail.EditableGridUNK = EMRUtility.MakeEditableGrid("#" + Clinical_ImplantableDetail.params.PanelID + " #pnlImplantableDeviceProcedureUNK_Result", "#" + Clinical_ImplantableDetail.params.PanelID + " #dgvProcedureImplantableDeviceUNK", Clinical_ImplantableDetail, "0", false, false, false, false);
            Clinical_ImplantableDetail.DeviceInactiveInEdit();
        }
        else
            Clinical_ImplantableDetail.EditableGridUNK = EMRUtility.MakeEditableGrid("#" + Clinical_ImplantableDetail.params.PanelID + " #pnlImplantableDeviceProcedureUNK_Result", "#" + Clinical_ImplantableDetail.params.PanelID + " #dgvProcedureImplantableDeviceUNK", Clinical_ImplantableDetail, "0", false, false, false, false);

        $('#' + Clinical_ImplantableDetail.params.PanelID + " #divDialogLength").removeClass();
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #divDialogLength").addClass('modal-dialog modal-dialog-lg');
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #divVerifySpace").removeClass();
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #divDeviceDetailSpace").removeClass();
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #divImplantationDetailsSpace").removeClass();
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #divProcedureSpace").removeClass();
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #divProcedures").hide();
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #divVerify").hide();
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #divImplantationDetails").hide();
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #pnlDeviceDetail").hide();
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #divUdiUnknownSpace").addClass('spacer15');
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #divProcedureSpaceUNK").addClass('spacer15');
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #divProceduresUNK").show();
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #divUdiUnknown").show();
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #SaveButton").show();
    },

    ShowDetailsInToolTip: function (implantableDeviceJSONData) {
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #GMDPTNameDetail").attr('title', implantableDeviceJSONData.GMDNPName);
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #DeviceDescriptionDetail").attr('title', implantableDeviceJSONData.DeviceDescription);
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #CompanyNameDetail").attr('title', implantableDeviceJSONData.CompanyName);
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #MRIInfoDetail").attr('title', implantableDeviceJSONData.MRISafetyStatus);
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #BrandNameDetail").attr('title', implantableDeviceJSONData.BrandName);
    },

    DivsHideShowInVerification: function () {
        if ($("#" + Clinical_ImplantableDetail.params.PanelID + " #pnlDeviceDetail").css("display") === "none")
            $("#" + Clinical_ImplantableDetail.params.PanelID + " #pnlDeviceDetail").show();

        $('#' + Clinical_ImplantableDetail.params.PanelID + " #divVerifySpace").addClass('spacer15');
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #divDeviceDetailSpace").addClass('spacer15');
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #divImplantationDetailsSpace").addClass('spacer15');
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #divProcedureSpace").addClass('spacer15');
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #SaveButton").show();
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #divProcedures").show();
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #divImplantationDetails").show();
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #ddlGMDNPName").remove();
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #ddlDI").remove();
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #divProcedureSpaceUNK").removeClass();
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #divProceduresUNK").hide();
    },

    ClearFields: function () {
        if (Clinical_ImplantableDetail.params.mode == "Edit") {
            if ($("#" + Clinical_ImplantableDetail.params.PanelID + " #rdUdiKnown").is(":checked")) {
                $("#" + Clinical_ImplantableDetail.params.PanelID + " #divImplantationDetails").resetAllControls();
                Clinical_ImplantableDetail.MakeEditableGridKnown();
            }
        }
    },

    DeviceInactiveInEdit: function () {
        if ($("#" + Clinical_ImplantableDetail.params.PanelID + " #IsActive").is(":checked") == false)
            $("#" + Clinical_ImplantableDetail.params.PanelID + " #IsActiveUnknown").prop("checked", false);
        else if ($("#" + Clinical_ImplantableDetail.params.PanelID + " #IsActiveUnknown").is(":checked") == false)
            $("#" + Clinical_ImplantableDetail.params.PanelID + " #IsActive").prop("checked", false);
    },

    HideAllDivs: function () {
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #divUdiUnknownSpace").removeClass();
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #divDeviceDetailSpace").removeClass();
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #divImplantationDetailsSpace").removeClass();
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #divProcedureSpace").removeClass();

        $('#' + Clinical_ImplantableDetail.params.PanelID + " #divImplantationDetails").hide();
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #divProcedures").hide();
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #pnlDeviceDetail").hide();
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #divUdiUnknown").hide();
        $('#' + Clinical_ImplantableDetail.params.PanelID + " #SaveButton").hide();
    },

    domReadyFunction: function () {
        $(function () {
            $('#' + Clinical_ImplantableDetail.params.PanelID + ' [data-plugin-toggle]').each(function () {
                var $this = $(this),
                    opts = {};

                var pluginOptions = $this.data('plugin-options');
                if (pluginOptions)
                    opts = pluginOptions;

                $this.themePluginToggle(opts);
            });
        });
    },

    bindMyJson: function (ImplantableDeviceJSONData) {
        var implantableDeviceJSONData = JSON.parse(ImplantableDeviceJSONData.implantableDevicesList);
        var IDJsonData = implantableDeviceJSONData[0];
        if (IDJsonData != null) {
            if (IDJsonData.Status == "Known") {
                if ($("#" + Clinical_ImplantableDetail.params.PanelID + " #pnlDeviceDetail").css("display") === "none")
                    $("#" + Clinical_ImplantableDetail.params.PanelID + " #pnlDeviceDetail").show();
                Clinical_ImplantableDetail.UdiKnown();
                if (ImplantableDeviceJSONData.implantableProceduresList != null) {
                    Clinical_ImplantableDetail.ProcedureGridLoad(ImplantableDeviceJSONData, "dgvProcedureImplantableDevice", "pnlImplantableDeviceProcedure_Result");
                }
                Clinical_ImplantableDetail.ShowDetailsInToolTip(IDJsonData);
            }
            else {
                Clinical_ImplantableDetail.UdiUnknown();
                if (ImplantableDeviceJSONData.implantableProceduresList != null) {
                    Clinical_ImplantableDetail.ProcedureGridLoad(ImplantableDeviceJSONData, "dgvProcedureImplantableDeviceUNK", "pnlImplantableDeviceProcedureUNK_Result");
                }
            }

            var self = $('#' + Clinical_ImplantableDetail.params.PanelID);
            utility.bindMyJSONByName(true, IDJsonData, false, self).done(function () {
                if (IDJsonData.Status == "Known") {
                    $("#" + Clinical_ImplantableDetail.params.PanelID + " #rdUdiKnown").prop("checked", IDJsonData.Status == "Known");
                    $("#" + Clinical_ImplantableDetail.params.PanelID + " #Issuing_agencyGS1").prop("checked", IDJsonData.Issuing_agency == "GS1");
                    $("#" + Clinical_ImplantableDetail.params.PanelID + " #Issuing_agencyHIBCC").prop("checked", IDJsonData.Issuing_agency == "HIBCC");
                    $("#" + Clinical_ImplantableDetail.params.PanelID + " #Issuing_agencyICCBBA").prop("checked", IDJsonData.Issuing_agency == "ICCBBA");
                    $("#" + Clinical_ImplantableDetail.params.PanelID + " #GMDPTNameDetail").text(IDJsonData.GMDNPName);
                    $("#" + Clinical_ImplantableDetail.params.PanelID + " #ImplantDate").val(utility.RemoveTimeFromDate(null, IDJsonData.ImplantDate));
                }
                else {
                    $("#" + Clinical_ImplantableDetail.params.PanelID + " #rdUdiUnknown").prop("checked", IDJsonData.Status == "Unknown");
                    $("#" + Clinical_ImplantableDetail.params.PanelID + " #IsActiveUnknown").prop("checked", implantableDeviceJSONData[0].IsActive == "True");
                    $("#" + Clinical_ImplantableDetail.params.PanelID + " #DeviceName").val(IDJsonData.GMDNPName);
                    $("#" + Clinical_ImplantableDetail.params.PanelID + " #UdiUnknown").val(IDJsonData.UDI != "NULL" ? IDJsonData.UDI : '');
                    $("#" + Clinical_ImplantableDetail.params.PanelID + " #ImplantDateUnknown").val(utility.RemoveTimeFromDate(null, IDJsonData.ImplantDate));
                    $("#" + Clinical_ImplantableDetail.params.PanelID + " #txtCommentsUdiUnknown").val(IDJsonData.Comments);
                }
            });
        }
        /*if (ImplantableDeviceJSONData.implantableProceduresList != null) {
            Clinical_ImplantableDetail.ProcedureGridLoad(ImplantableDeviceJSONData, "dgvProcedureImplantableDevice", "pnlImplantableDeviceProcedure_Result");
        }
    
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #IsActive").prop("checked", implantableDeviceJSONData[0].IsActive == "True");
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #rdUdiKnown").prop("checked", implantableDeviceJSONData[0].Status == "Known");
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #Issuing_agencyGS1").prop("checked", implantableDeviceJSONData[0].Issuing_agency == "GS1");
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #Issuing_agencyHIBCC").prop("checked", implantableDeviceJSONData[0].Issuing_agency == "HIBCC");
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #Issuing_agencyICCBBA").prop("checked", implantableDeviceJSONData[0].Issuing_agency == "ICCBBA");
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #LotNoDetail").text(implantableDeviceJSONData[0].Lot_Number);
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #SerialNoDetail").text(implantableDeviceJSONData[0].Serial_Number);
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #DeviceDescriptionDetail").text(implantableDeviceJSONData[0].DeviceDescription);
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #ExpiryDateDetail").text(utility.RemoveTimeFromDate(null, implantableDeviceJSONData[0].Expiration_Date));
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #ManufacturingDateDetail").text(utility.RemoveTimeFromDate(null, implantableDeviceJSONData[0].Manufacturing_Date));
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #BrandNameDetail").text(implantableDeviceJSONData[0].BrandName);
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #ModelNoDetail").text(implantableDeviceJSONData[0].VersionModelNumber);
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #CompanyNameDetail").text(implantableDeviceJSONData[0].CompanyName);
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #MRIInfoDetail").text(implantableDeviceJSONData[0].MRISafetyStatus);
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #LabelingInfoDetail").text(implantableDeviceJSONData[0].LabeledContainsNRL);
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #GMDPTNameDetail").text(implantableDeviceJSONData[0].GMDNPName);
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #DistinctIdentificationCodeDetail").text(implantableDeviceJSONData[0].DI);
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #ImplantDate").val(utility.RemoveTimeFromDate(null, implantableDeviceJSONData[0].ImplantDate));
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #UDI").val(implantableDeviceJSONData[0].UDI != "NULL" ? implantableDeviceJSONData[0].UDI : '');
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #txtCommentsUdiKnown").val(implantableDeviceJSONData[0].Comments);
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #Area").val(implantableDeviceJSONData[0].Area);
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #ddlLaterality").val(implantableDeviceJSONData[0].LateralityId);
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #txtTargetSite").val(implantableDeviceJSONData[0].TargetSite);
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #hfTargetSite").val(implantableDeviceJSONData[0].TargetSiteId);
        Clinical_ImplantableDetail.ShowDetailsInToolTip(implantableDeviceJSONData[0]);
    }
    else if (implantableDeviceJSONData[0].Status == "Unknown") {
        Clinical_ImplantableDetail.UdiUnknown();
        if (ImplantableDeviceJSONData.implantableProceduresList != null) {
            Clinical_ImplantableDetail.ProcedureGridLoad(ImplantableDeviceJSONData, "dgvProcedureImplantableDeviceUNK", "pnlImplantableDeviceProcedureUNK_Result");
        }
    
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #rdUdiUnknown").prop("checked", implantableDeviceJSONData[0].Status == "Unknown");
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #IsActiveUnknown").prop("checked", implantableDeviceJSONData[0].IsActive == "True");
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #DeviceName").val(implantableDeviceJSONData[0].GMDNPName);
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #UdiUnknown").val(implantableDeviceJSONData[0].UDI != "NULL" ? implantableDeviceJSONData[0].UDI : '');
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #ddlInformant").val(implantableDeviceJSONData[0].InformantId);
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #ImplantDateUnknown").val(utility.RemoveTimeFromDate(null, implantableDeviceJSONData[0].ImplantDate));
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #txtCommentsUdiUnknown").val(implantableDeviceJSONData[0].Comments);
    }*/
    },

    VerifyUDI: function () {
        Clinical_ImplantableDetail.VerifyImplantableDevice().done(function (response) {
            response = JSON.parse(response);
            if (response.status == true) {
                Clinical_ImplantableDetail.DivsHideShowInVerification();
                Clinical_ImplantableDetail.IsGMDNPNameList = false;
                Clinical_ImplantableDetail.IsDIList = false;

                var DIJSONData = response.DIList;
                var GMDNPJSONData = response.GMDNPNamesList;
                var implantableDeviceJSONData = response.implantableDevicesList[0];

                if (implantableDeviceJSONData != null) {
                    if (GMDNPJSONData.length != 0) {
                        Clinical_ImplantableDetail.IsGMDNPNameList = true;
                        $("#" + Clinical_ImplantableDetail.params.PanelID + " #GMDPTName").removeClass();
                        $("#" + Clinical_ImplantableDetail.params.PanelID + " #GMDPTNameDetail").hide();
                        var combo = $('<select id="ddlGMDNPName" style="height: 25px;width: 318px;padding-top: 3px;padding-bottom: 3px;padding-left: 8px;padding-right: 8px;"></select>');
                        $.each(GMDNPJSONData, function (i, item) {
                            combo.append("<option id=" + (i + 1) + ">" + item.GMDNPName + "</option>");
                            $("#" + Clinical_ImplantableDetail.params.PanelID + " #GMDPTName").append(combo);
                        });
                    }
                    else {
                        $("#" + Clinical_ImplantableDetail.params.PanelID + " #GMDPTName").addClass("control-label text-underline-full");
                        $("#" + Clinical_ImplantableDetail.params.PanelID + " #GMDPTNameDetail").show();
                        $("#" + Clinical_ImplantableDetail.params.PanelID + " #GMDPTNameDetail").text(implantableDeviceJSONData.GMDNPName);
                    }

                    if (DIJSONData.length != 0) {
                        Clinical_ImplantableDetail.IsDIList = true;
                        $("#" + Clinical_ImplantableDetail.params.PanelID + " #DistinctIdentificationCode").removeClass();
                        $("#" + Clinical_ImplantableDetail.params.PanelID + " #DistinctIdentificationCodeDetail").hide();
                        var combo = $('<select id="ddlDI" style="height: 25px;width: 248px;padding-top: 3px;padding-bottom: 3px;padding-left: 8px;padding-right: 8px;"></select>');
                        $.each(DIJSONData, function (i, item) {
                            combo.append("<option id=" + (i + 1) + " value=" + item.DeviceId + ">" + item.DeviceId + " - " + item.DeviceIdType + " - " + item.DeviceIdIssuingAgency + "</option>");
                            $("#" + Clinical_ImplantableDetail.params.PanelID + " #DistinctIdentificationCode").append(combo);
                        });
                    }
                    else {
                        $("#" + Clinical_ImplantableDetail.params.PanelID + " #DistinctIdentificationCode").addClass("control-label text-underline-full");
                        $("#" + Clinical_ImplantableDetail.params.PanelID + " #DistinctIdentificationCodeDetail").show();
                        $("#" + Clinical_ImplantableDetail.params.PanelID + " #DistinctIdentificationCodeDetail").text(implantableDeviceJSONData.DI);
                    }

                    $("#" + Clinical_ImplantableDetail.params.PanelID + " #Issuing_agencyGS1").prop("checked", implantableDeviceJSONData.Issuing_agency == "GS1");
                    $("#" + Clinical_ImplantableDetail.params.PanelID + " #Issuing_agencyHIBCC").prop("checked", implantableDeviceJSONData.Issuing_agency == "HIBCC");
                    $("#" + Clinical_ImplantableDetail.params.PanelID + " #Issuing_agencyICCBBA").prop("checked", implantableDeviceJSONData.Issuing_agency == "ICCBBA");
                    $("#" + Clinical_ImplantableDetail.params.PanelID + " #LotNoDetail").text(implantableDeviceJSONData.Lot_Number);
                    $("#" + Clinical_ImplantableDetail.params.PanelID + " #SerialNoDetail").text(implantableDeviceJSONData.Serial_Number);
                    $("#" + Clinical_ImplantableDetail.params.PanelID + " #DeviceDescriptionDetail").text(implantableDeviceJSONData.DeviceDescription);
                    $("#" + Clinical_ImplantableDetail.params.PanelID + " #ExpiryDateDetail").text(utility.RemoveTimeFromDate(null, implantableDeviceJSONData.Expiration_Date));
                    $("#" + Clinical_ImplantableDetail.params.PanelID + " #ManufacturingDateDetail").text(utility.RemoveTimeFromDate(null, implantableDeviceJSONData.Manufacturing_Date));
                    $("#" + Clinical_ImplantableDetail.params.PanelID + " #BrandNameDetail").text(implantableDeviceJSONData.BrandName);
                    $("#" + Clinical_ImplantableDetail.params.PanelID + " #ModelNoDetail").text(implantableDeviceJSONData.VersionModelNumber);
                    $("#" + Clinical_ImplantableDetail.params.PanelID + " #CompanyNameDetail").text(implantableDeviceJSONData.CompanyName);
                    $("#" + Clinical_ImplantableDetail.params.PanelID + " #MRIInfoDetail").text(implantableDeviceJSONData.MRISafetyStatus);
                    $("#" + Clinical_ImplantableDetail.params.PanelID + " #LabelingInfoDetail").text(implantableDeviceJSONData.LabeledContainsNRL);

                    Clinical_ImplantableDetail.ShowDetailsInToolTip(implantableDeviceJSONData);
                }
            }
            else {
                response.message = JSON.parse(response.message);
                utility.DisplayMessages(response.message.error, 3);
            }
        });
    },

    ImplantableDeviceSave: function () {
        var params = {};
        params["ImplantableDevicesPKId"] = Clinical_ImplantableDetail.params.ImplantableDevicesPKId;
        params["PatientId"] = Clinical_ImplantableDetail.params.PatientID;

        if ($("#" + Clinical_ImplantableDetail.params.PanelID + " #rdUdiKnown").is(":checked")) {
            $("#" + Clinical_ImplantableDetail.params.PanelID + " #Issuing_agencyGS1").is(':checked') ? params["Issuing_agency"] = "GS1" : ($("#" + Clinical_ImplantableDetail.params.PanelID + " #Issuing_agencyHIBCC").is(':checked') ? params["Issuing_agency"] = "HIBCC" : ($("#" + Clinical_ImplantableDetail.params.PanelID + " #Issuing_agencyICCBBA").is(':checked') ? params["Issuing_agency"] = "ICCBBA" : null));
            params["UDI"] = $("#" + Clinical_ImplantableDetail.params.PanelID + " #UDI").val();
            params["ImplantDate"] = $("#" + Clinical_ImplantableDetail.params.PanelID + " #ImplantDate").val();
            $("#" + Clinical_ImplantableDetail.params.PanelID + " #IsActive").is(':checked') ? params["IsActive"] = true : params["IsActive"] = false;
            params["BrandName"] = $("#" + Clinical_ImplantableDetail.params.PanelID + " #BrandNameDetail").text();
            params["CompanyName"] = $("#" + Clinical_ImplantableDetail.params.PanelID + " #CompanyNameDetail").text();
            params["Expiration_Date"] = $("#" + Clinical_ImplantableDetail.params.PanelID + " #ExpiryDateDetail").text();
            params["Manufacturing_Date"] = $("#" + Clinical_ImplantableDetail.params.PanelID + " #ManufacturingDateDetail").text();
            params["LabeledContainsNRL"] = $("#" + Clinical_ImplantableDetail.params.PanelID + " #LabelingInfoDetail").text();
            params["MRISafetyStatus"] = $("#" + Clinical_ImplantableDetail.params.PanelID + " #MRIInfoDetail").text();
            params["Lot_Number"] = $("#" + Clinical_ImplantableDetail.params.PanelID + " #LotNoDetail").text();
            params["Serial_Number"] = $("#" + Clinical_ImplantableDetail.params.PanelID + " #SerialNoDetail").text();
            params["VersionModelNumber"] = $("#" + Clinical_ImplantableDetail.params.PanelID + " #ModelNoDetail").text();
            params["DeviceDescription"] = $("#" + Clinical_ImplantableDetail.params.PanelID + " #DeviceDescriptionDetail").text();
            params["Status"] = "Known";
            params["Comments"] = $("#" + Clinical_ImplantableDetail.params.PanelID + " #txtCommentsUdiKnown").val();
            params["Area"] = $("#" + Clinical_ImplantableDetail.params.PanelID + " #Area").val();
            params["LateralityId"] = $("#" + Clinical_ImplantableDetail.params.PanelID + " #ddlLaterality").val();
            params["TargetSiteId"] = $("#" + Clinical_ImplantableDetail.params.PanelID + " #frmClinicalImplantableDetail #hfTargetSite").val();
            
            var count = 0;
            var ImplantableDeviceProcedure = [];
            if ($("#" + Clinical_ImplantableDetail.params.PanelID + " #dgvProcedureImplantableDevice tbody tr").length >= 1 && $("#" + Clinical_ImplantableDetail.params.PanelID + " #dgvProcedureImplantableDevice tbody tr").attr("id") != undefined) {
                $("#" + Clinical_ImplantableDetail.params.PanelID + " #dgvProcedureImplantableDevice tbody tr").each(function () {
                    ProcedureId = $(this).attr("id");
                    var ProcedureRow = {};
                    var ParsedProcedureJson = JSON.parse($(this).getMyJSONByName());
                    ProcedureRow["ImplantableDeviceProcedureId"] = ProcedureId;
                    ProcedureRow["Procedure"] = ParsedProcedureJson.Procedure;
                    ProcedureRow["CPTCode"] = $($("#" + Clinical_ImplantableDetail.params.PanelID + " #dgvProcedureImplantableDevice tbody tr")[count]).attr('cptcode');
                    ProcedureRow["CPTCodeDescription"] = $($("#" + Clinical_ImplantableDetail.params.PanelID + " #dgvProcedureImplantableDevice tbody tr")[count]).attr('cptdescription');
                    ProcedureRow["SnomedID"] = $($("#" + Clinical_ImplantableDetail.params.PanelID + " #dgvProcedureImplantableDevice tbody tr")[count]).attr('snomedid');
                    ProcedureRow["SnomedDescription"] = $($("#" + Clinical_ImplantableDetail.params.PanelID + " #dgvProcedureImplantableDevice tbody tr")[count]).attr('snomeddescription');
                    ImplantableDeviceProcedure.push(ProcedureRow);
                    count++;
                });
            }
            params["ImplantableDeviceProcedure"] = ImplantableDeviceProcedure;

            if (Clinical_ImplantableDetail.IsGMDNPNameList == true)
                params["GMDNPName"] = $("#" + Clinical_ImplantableDetail.params.PanelID + " #ddlGMDNPName option:selected").text();
            else
                params["GMDNPName"] = $("#" + Clinical_ImplantableDetail.params.PanelID + " #GMDPTNameDetail").text();

            if (Clinical_ImplantableDetail.IsDIList == true)
                params["DI"] = $("#" + Clinical_ImplantableDetail.params.PanelID + " #ddlDI option:selected").val();
            else
                params["DI"] = $("#" + Clinical_ImplantableDetail.params.PanelID + " #DistinctIdentificationCodeDetail").text();

        }
        else {
            params["UDI"] = $("#" + Clinical_ImplantableDetail.params.PanelID + " #UdiUnknown").val();
            params["InformantId"] = $("#" + Clinical_ImplantableDetail.params.PanelID + " #ddlInformant option:selected").val();
            params["Comments"] = $("#" + Clinical_ImplantableDetail.params.PanelID + " #txtCommentsUdiUnknown").val();
            params["GMDNPName"] = $("#" + Clinical_ImplantableDetail.params.PanelID + " #DeviceName").val();
            $("#" + Clinical_ImplantableDetail.params.PanelID + " #IsActiveUnknown").is(':checked') ? params["IsActive"] = true : params["IsActive"] = false;
            params["ImplantDate"] = $("#" + Clinical_ImplantableDetail.params.PanelID + " #ImplantDateUnknown").val();
            params["Status"] = "Unknown";

            var count = 0;
            var ImplantableDeviceProcedure = [];
            if ($("#" + Clinical_ImplantableDetail.params.PanelID + " #dgvProcedureImplantableDeviceUNK tbody tr").length >= 1 && $("#" + Clinical_ImplantableDetail.params.PanelID + " #dgvProcedureImplantableDeviceUNK tbody tr").attr("id") != undefined) {
                $("#" + Clinical_ImplantableDetail.params.PanelID + " #dgvProcedureImplantableDeviceUNK tbody tr").each(function () {
                    ProcedureId = $(this).attr("id");
                    var ProcedureRow = {};
                    var ParsedProcedureJson = JSON.parse($(this).getMyJSONByName());
                    ProcedureRow["ImplantableDeviceProcedureId"] = ProcedureId;
                    ProcedureRow["Procedure"] = ParsedProcedureJson.Procedure;
                    ProcedureRow["CPTCode"] = $($("#" + Clinical_ImplantableDetail.params.PanelID + " #dgvProcedureImplantableDeviceUNK tbody tr")[count]).attr('cptcode');
                    ProcedureRow["CPTCodeDescription"] = $($("#" + Clinical_ImplantableDetail.params.PanelID + " #dgvProcedureImplantableDeviceUNK tbody tr")[count]).attr('cptdescription');
                    ProcedureRow["SnomedID"] = $($("#" + Clinical_ImplantableDetail.params.PanelID + " #dgvProcedureImplantableDeviceUNK tbody tr")[count]).attr('snomedid');
                    ProcedureRow["SnomedDescription"] = $($("#" + Clinical_ImplantableDetail.params.PanelID + " #dgvProcedureImplantableDeviceUNK tbody tr")[count]).attr('snomeddescription');
                    ImplantableDeviceProcedure.push(ProcedureRow);
                    count++;
                });
            }
            params["ImplantableDeviceProcedure"] = ImplantableDeviceProcedure;
        }
        
        if (Clinical_ImplantableDetail.params.mode == "Add") {
            params["commandType"] = "save_ImplantableDevices";
            Clinical_ImplantableDetail.SaveImplantableDevice(params).done(function (response) {
                response = JSON.parse(response);
                if (response.status == true) {
                    Clinical_ImplantableDetail.UnLoadTab();
                    Clinical_Implantable.ImplantableSearch();
                    utility.DisplayMessages(response.message, 1);
                }
                else {
                    utility.DisplayMessages(response.message, 3);
                }
            });
        }
        else if (Clinical_ImplantableDetail.params.mode == "Edit") {
            params["commandType"] = "update_ImplantableDevices";
            Clinical_ImplantableDetail.UpdateImplantableDevice(params).done(function (response) {
                response = JSON.parse(response);
                if (response.status == true) {
                    Clinical_ImplantableDetail.UnLoadTab();
                    Clinical_Implantable.ImplantableSearch();
                    utility.DisplayMessages(response.message, 1);
                }
                else {
                    utility.DisplayMessages(response.message, 3);
                }
            });
        }
    },

    UpdateImplantableDevice: function (ImplantableDeviceJson) {
        var data = JSON.stringify(ImplantableDeviceJson);
        return MDVisionService.APIService(data, "Implantable", "ImplantableDetail");
    },

    SaveImplantableDevice: function (ImplantableDeviceJson) {
        var data = JSON.stringify(ImplantableDeviceJson);
        return MDVisionService.APIService(data, "Implantable", "ImplantableDetail");
    },

    VerifyImplantableDevice: function () {
        var params = {};
        params["ImplantableDevicesPKId"] = Clinical_ImplantableDetail.params.ImplantableDevicesPKId;
        params["PatientId"] = Clinical_ImplantableDetail.params.PatientID;
        $("#" + Clinical_ImplantableDetail.params.PanelID + " #Issuing_agencyGS1").is(':checked') ? params["Issuing_agency"] = "GS1" : ($("#" + Clinical_ImplantableDetail.params.PanelID + " #Issuing_agencyHIBCC").is(':checked') ? params["Issuing_agency"] = "HIBCC" : ($("#" + Clinical_ImplantableDetail.params.PanelID + " #Issuing_agencyICCBBA").is(':checked') ? params["Issuing_agency"] = "ICCBBA" : params["Issuing_agency"] = "false"));
        params["UDI"] = $("#" + Clinical_ImplantableDetail.params.PanelID + " #UDI").val();
        params["ImplantDate"] = $("#" + Clinical_ImplantableDetail.params.PanelID + " #ImplantDate").val();
        params["IsActive"] = $("#" + Clinical_ImplantableDetail.params.PanelID + " #IsActive").val() == "" ? "" : $("#" + Clinical_ImplantableDetail.params.PanelID + " #IsActive").val();
        params["commandType"] = "verify_UDI";
        var data = JSON.stringify(params);
        return MDVisionService.APIService(data, "Implantable", "ImplantableDetail");
    },

    FillDeviceForm: function (ImplantableDevicesPKId, pageNumber, rowsPerPage) {
        if (pageNumber == null)
            pageNumber = 1;
        if (rowsPerPage == null)
            rowsPerPage = 15;
        var params = {};
        params["ImplantableDevicesPKId"] = ImplantableDevicesPKId;
        params["PatientId"] = Clinical_ImplantableDetail.params.PatientID;
        params["PageNumber"] = pageNumber;
        params["RowsPerPage"] = rowsPerPage;
        params["commandType"] = "fill_ImplantableDevice";
        var data = JSON.stringify(params);
        return MDVisionService.APIService(data, "Implantable", "ImplantableDetail");
    },

    UnLoadTab: function (caller) {
        if (Clinical_ImplantableDetail.params["FromAdmin"] == "0") {
            if (Clinical_ImplantableDetail.params != null && Clinical_ImplantableDetail.params.ParentCtrl != null) {
                if (Clinical_ImplantableDetail.params.ParentCtrl == 'clinicalTabImplantable')
                    UnloadActionPan(Clinical_ImplantableDetail.params["ParentCtrl"], "Clinical_ImplantableDetail");
                else {
                    Clinical_ImplantableDetail.params.PanelID = Clinical_ImplantableDetail.params.PanelID.replace(" #pnlClinicalImplantableDetail", "");
                    UnloadActionPan(Clinical_ImplantableDetail.params.ParentCtrl, 'Clinical_ImplantableDetail', null, Clinical_ImplantableDetail.params.PanelID);
                }
            }
            else
                UnloadActionPan(null, 'Clinical_ImplantableDetail');
        }
        else
            RemoveAdminTab();
    },

        //for Target Site
    BindTargetSite: function () {
        var TargetSite = $("#" + Clinical_ImplantableDetail.params.PanelID + " #frmClinicalImplantableDetail #txtTargetSite").val();
        utility.GetTargetSiteArray(TargetSite).done(function (response) {
            $("#" + Clinical_ImplantableDetail.params.PanelID + " #frmClinicalImplantableDetail #txtTargetSite").autocomplete({
                autoFocus: true,
                source: response,
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#" + Clinical_ImplantableDetail.params.PanelID + " #frmClinicalImplantableDetail #txtTargetSite").val(ui.item.value);
                        $("#" + Clinical_ImplantableDetail.params.PanelID + " #frmClinicalImplantableDetail #hfTargetSite").val(ui.item.id);
                    }, 100);
                }
            });
        });
    },

    LoadTargetSiteDBCall: function (TargetSite) {
        var objData = {};
        objData["TargetSite"] = TargetSite;
        objData["commandType"] = "TargetSite_Lookup";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Implantable", "ImplantableDetail");
    },

         //for associated procedures
    bindAutoComplete: function (element) {
        var hiddenCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "Clinical_ImplantableDetail", null, true);
    },

    openCPTCode: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Clinical_ImplantableDetail";
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtCPTCode";
        params["ParentCtrlPanelID"] = Clinical_ImplantableDetail.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, Clinical_ImplantableDetail.params.PanelID);
    },

    BindProcedureGridItem: function (cptCode, procedureDescription, cptDescription, SnomedID, SnomedDescription) {
        var cptCode = cptCode;
        var procDesc = procedureDescription;
        var cptDesc = cptDescription;
        var tableId = "";
        var txtId = "";
        
        if ($("#" + Clinical_ImplantableDetail.params.PanelID + " #rdUdiKnown").is(":checked")) {
            tableId = "dgvProcedureImplantableDevice";
            txtId = "txtImplantableDeviceCPTCode";
        }
        else {
            tableId = "dgvProcedureImplantableDeviceUNK";
            txtId = "txtImplantableDeviceCPTCodeUNK";
        }

        var currentRow = $("#" + Clinical_ImplantableDetail.params.PanelID + " #" + tableId + " tbody tr");
        var isTestAlreadySelected = false;
        $.each(currentRow, function (i, item) {
            var currentRowCPTDescription = $(item).find("input[id*='Procedure']").val() != null ? $(item).find("input[id*='Procedure']").val().replace(cptCode, "").trim() : "";
            if (cptDescription.toLowerCase() == currentRowCPTDescription.toLowerCase()) {
                isTestAlreadySelected = true;
                return;
            }
        });

        if (isTestAlreadySelected != true) {
            Clinical_ImplantableDetail.AddNewProcedureRow(null, null, cptCode, procDesc, cptDescription, SnomedID, SnomedDescription, tableId);
            setTimeout(function () {
                $("#" + Clinical_ImplantableDetail.params.PanelID + " #"+txtId).val('');
            }, 200);
        }
        else {
            utility.DisplayMessages("This code already exists in the implantable device.", 2);
        }
    },

    AddNewProcedureRow: function (Status, RowId, cptCode, procDesc, cptDescription, SnomedId, SnomedDescription, tableId) {
        var ProcedureGridId = "#" + Clinical_ImplantableDetail.params.PanelID + " #"+tableId;
        var CurrentRow = null;
        if (RowId && RowId > 0) {
            if (Clinical_ImplantableDetail.params.ParentCtrl != null){
                if (Status == "Known" || $("#" + Clinical_ImplantableDetail.params.PanelID + " #rdUdiKnown").is(":checked"))
                    CurrentRow = Clinical_ImplantableDetail.EditableGrid.rowAdd(RowId, "");
                else
                    CurrentRow = Clinical_ImplantableDetail.EditableGridUNK.rowAdd(RowId, "");
            }
            else {
                if (Status == "Known" || $("#" + Clinical_ImplantableDetail.params.PanelID + " #rdUdiKnown").is(":checked"))
                    CurrentRow = Clinical_ImplantableDetail.EditableGrid.rowAdd(RowId, Clinical_ImplantableDetail.params.ImplantableDevicesPKId);
                else
                    CurrentRow = Clinical_ImplantableDetail.EditableGridUNK.rowAdd(RowId, Clinical_ImplantableDetail.params.ImplantableDevicesPKId);
            }
        }
        else {
            if (Clinical_ImplantableDetail.params.ParentCtrlPanelID != undefined)
                var TemplateRow = $("#" + Clinical_ImplantableDetail.params.ParentCtrlPanelID + " #"+ tableId + " tbody tr[id*=-]").last();
            else
                var TemplateRow = $("#" + Clinical_ImplantableDetail.params.PanelID + " #" + tableId + " tbody tr[id*=-]").last();
            var TemplateRowId = 0;
            if (TemplateRow.length > 0) {
                if (TemplateRow.attr("id").indexOf("Child") != "-1")
                    TemplateRowId = parseInt(TemplateRow.prev().attr("id"));
                else
                    TemplateRowId = parseInt(TemplateRow.attr("id"));
            }
            if (Status == "Known" || $("#" + Clinical_ImplantableDetail.params.PanelID + " #rdUdiKnown").is(":checked"))
                CurrentRow = Clinical_ImplantableDetail.EditableGrid.rowAdd(TemplateRowId - 1, "");
            else
                CurrentRow = Clinical_ImplantableDetail.EditableGridUNK.rowAdd(TemplateRowId - 1, "");
        }
        $(CurrentRow).attr("CptCode", cptCode);
        $(CurrentRow).attr("CptDescription", cptDescription);
        $(CurrentRow).attr("SnomedId", SnomedId);
        $(CurrentRow).attr("SnomedDescription", SnomedDescription);
        $(CurrentRow).find('input[id*="Procedure"]').val(cptCode != "" ? cptCode + " " + cptDescription : cptDescription);

        Clinical_ImplantableDetail.enableRemoveRow($(CurrentRow));

        return CurrentRow;
    },

    enableRemoveRow: function (CurrentRow) {
        CurrentRow.find("td.actions .remove-row").removeClass("hidden");
    },

    rowRemove: function ($row, obj) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_Implantable Devices", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    if ($row.hasClass('adding')) {
                    }
                    if (parseInt($row.attr("id")) > 0) {
                        Clinical_ImplantableDetail.DeleteImplantableDeviceProcedure($row.attr("id"), $row, obj);
                    }
                    else {
                        var _self = obj;
                        _self.datatable.row($row.get(0)).remove().draw();
                        utility.DisplayMessages("Successfully Deleted", 1);
                    }
                }, function () {
                },
                            '1'
            );
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    DeleteImplantableDeviceProcedure: function (ImplantableDeviceProcedureId, $row, obj) {
        Clinical_ImplantableDetail.DeleteImplantableDeviceProcedure_DBCall(ImplantableDeviceProcedureId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                utility.DisplayMessages(response.message, 1);
                var _self = obj;
                _self.datatable.row($row.get(0)).remove().draw();
            } else {
                utility.DisplayMessages(response.message, 3);
            }
        });
    },

    DeleteImplantableDeviceProcedure_DBCall: function (ImplantableDeviceProcedureId) {
        var objData = {};
        objData["ProcedureId"] = ImplantableDeviceProcedureId;
        objData["commandType"] = "DELETE_IMPLANTABLE_DEVICE_PROCEDURE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Implantable", "ImplantableDetail");
    },

    ProcedureGridLoad: function (response, tableId, tablePanelId) {
        var ImplantableDeviceProcedureLoadJSONData = JSON.parse(response.implantableProceduresList);
        var implantableDeviceLoadJsonData = JSON.parse(response.implantableDevicesList);
        var TotalProcedures = ImplantableDeviceProcedureLoadJSONData.length;
        var procedureNo = 0;
        var dfd = $.Deferred();
        var PanelProcedureGrid = "#" + Clinical_ImplantableDetail.params.PanelID + " #" + tablePanelId;
        var ImplantableDeviceGridId = "#" + Clinical_ImplantableDetail.params.PanelID + " #" + tableId;

        $(ImplantableDeviceGridId + " tbody tr").remove();
        if ($.fn.dataTable.isDataTable(ImplantableDeviceGridId)) {
            $(ImplantableDeviceGridId).dataTable().fnClearTable();
            $(ImplantableDeviceGridId).dataTable().fnDestroy();
        }
        if (implantableDeviceLoadJsonData[0].Status == "Known" || $("#" + Clinical_ImplantableDetail.params.PanelID + " #rdUdiKnown").is(":checked"))
            Clinical_ImplantableDetail.EditableGrid.datatable.clear().draw();
        else
            Clinical_ImplantableDetail.EditableGridUNK.datatable.clear().draw();

        $.each(ImplantableDeviceProcedureLoadJSONData, function (i, item) {
            var ImplantableDeviceProcedureId = item.ImplantableDeviceProcedureId;
            var CurrentRow = Clinical_ImplantableDetail.AddNewProcedureRow(implantableDeviceLoadJsonData[0].Status, ImplantableDeviceProcedureId,  item.CPTCode, item.Procedure, item.CPTCodeDescription, item.SnomedID, item.SnomedDescription, tableId);
            var self = $("#" + Clinical_ImplantableDetail.params.PanelID + " tr#" + ImplantableDeviceProcedureId);
            var row = "";
            if (implantableDeviceLoadJsonData[0].Status == "Known" || $("#" + Clinical_ImplantableDetail.params.PanelID + " #rdUdiKnown").is(":checked"))
                row = Clinical_ImplantableDetail.EditableGrid.datatable.row(CurrentRow);
            else
                row = Clinical_ImplantableDetail.EditableGridUNK.datatable.row(CurrentRow);

            var ImplantableDevicePocedureTable = $("#" + Clinical_ImplantableDetail.params.PanelID + " #" + tableId);
            var counter = 0;
            var BindFunction = function (counter, item, CurrentRow, TotalProcedures, procedureNo) {
                procedureNo++;
                if (counter++ < 5 && $(CurrentRow).find('select option').length <= 1)
                    setTimeout(BindFunction, 500, counter, item, CurrentRow, TotalProcedures, procedureNo);
                else {
                    utility.bindMyJSONByName(true, item, false, $(CurrentRow)).done(function () {
                        dfd.resolve();
                    });
                }
            }
            BindFunction(counter, item, CurrentRow, TotalProcedures, procedureNo);
        });
        if (TotalProcedures == 0) {
            dfd.resolve();
        }
        return dfd.promise();
    },
}