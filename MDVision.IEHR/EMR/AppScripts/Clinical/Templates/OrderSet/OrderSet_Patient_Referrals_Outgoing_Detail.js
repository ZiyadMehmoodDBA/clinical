OrderSet_Patient_Referrals_Outgoing_Detail = {
    bIsFirstLoad: true,
    params: [],
    EditableGrid: null,
    IsAnyChange: false,
    Load: function (params) {
        OrderSet_Patient_Referrals_Outgoing_Detail.params = params;
        OrderSet_Patient_Referrals_Outgoing_Detail.IsAnyChange = false;


        if (OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID != 'pnlOrderSetPatientReferralsOutgoingDetail') {
            OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID = OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + ' #pnlOrderSetPatientReferralsOutgoingDetail';
        } else {
            OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID = 'pnlOrderSetPatientReferralsOutgoingDetail';
        }

        utility.CreateDatePicker(OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + ' #dtDate', function () {
            //on-change callback method 
        }, true);
        utility.CreateDatePicker(OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + ' #dtDateFrom', function () {
            //on-change callback method 
        }, true);
        utility.CreateDatePicker(OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + ' #dtDateTo', function () {
            //on-change callback method 
        }, true);
        utility.ValidateFromToDate('frmPatientReferralsOutgoingDetail', "dtDateFrom", "dtDateTo", true);
        $('#' + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + ' #tmTime').timepicker({
            defaultTime: new Date()
        });


        var PanelReferralGrid = "#" + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #pnlReferralProcedure_Result";
        var ReferralGridId = "#" + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral";
        //$(ChargeGridId).dataTable().fnDestroy();
        $(ReferralGridId + " tbody tr").remove();
        OrderSet_Patient_Referrals_Outgoing_Detail.EditableGrid = EMRUtility.MakeEditableGrid(PanelReferralGrid, ReferralGridId, OrderSet_Patient_Referrals_Outgoing_Detail, "0", false, false, false, false);

        OrderSet_Patient_Referrals_Outgoing_Detail.OpenAssignee();
        OrderSet_Patient_Referrals_Outgoing_Detail.LoadAllAutocomplete();
        OrderSet_Patient_Referrals_Outgoing_Detail.BindAutoSpecialityReferralcomplete();
        var self = $('#' + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID);

        if (OrderSet_Patient_Referrals_Outgoing_Detail.bIsFirstLoad == true) {
            self.loadDropDowns(true).done(function () {
                var $options = $(self).find("#ddlFacilityTo > option").clone();
                $(self).find('#ddlFacilityFrom').append($options);

                if (OrderSet_Patient_Referrals_Outgoing_Detail.params.mode == "Edit") {
                    //$("#" + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #PrintButton").prop("disabled", false);
                    $('#' + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail").data('serialize', $('#' + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail").serialize());
                }
                else {
                    $('#' + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail").data('serialize', $('#' + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail").serialize());
                }
                OrderSet_Patient_Referrals_Outgoing_Detail.ValidateOutGoingReferrals();
            });



        }

        if (OrderSet_Patient_Referrals_Outgoing_Detail.params.mode == "Add") {

            $('#' + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #menuAttach").remove();
            $('#' + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #attachmentDiv").html('<a id="btnViewAttachment" href="#" class="dropdown-toggle btn btn-link btn-xs p-none" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false" onclick="">View Attachment</a>');
            $('#' + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #btnScanResult,#btnViewAttachment").addClass("disableAll");
        }
        else if (OrderSet_Patient_Referrals_Outgoing_Detail.params.mode == "Edit") {
            OrderSet_Patient_Referrals_Outgoing_Detail.EditModeSettings();
            OrderSet_Patient_Referrals_Outgoing_Detail.FillOrderSetReferrals(OrderSet_Patient_Referrals_Outgoing_Detail.params.OrderSetReferralId, OrderSet_Patient_Referrals_Outgoing_Detail.params.OrderSetId);

        }
    },

    EditModeSettings: function () {

        $('#' + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #attachDiv").append('<ul id="menuAttach" class="dropdown-menu" aria-labelledby="btnScanResult">' +
             '<li><a href="#" onclick="OrderSet_Patient_Referrals_Outgoing_Detail.documentScan()">Scan</a></li><li><a href="#" onclick="OrderSet_Patient_Referrals_Outgoing_Detail.documentImport()">Upload</a></li></ul>');

        $('#' + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #attachmentDiv").html('<a id="btnViewAttachment" href="#" class="dropdown-toggle btn btn-link btn-xs p-none" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false" onclick="OrderSet_Patient_Referrals_Outgoing_Detail.loadAttachments()">View Attachment</a><ul id="menuViewAttachment" class="dropdown-menu" aria-labelledby="btnViewAttachment"></ul>');
        $('#' + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #btnScanResult,#btnViewAttachment").removeClass("disableAll");
    },
    ValidateOutGoingReferrals: function () {

        var $self = $("#" + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail");
        var fields = {};
        $self.bootstrapValidator({
            live: 'disabled',
            message: 'This value is not valid',
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {}

        })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            OrderSet_Patient_Referrals_Outgoing_Detail.OutGoingReferralSave();
        });
    },

    removeFacility: function (ctrl) {
        if ($(ctrl).val() == "") {
            $("#" + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #txtPractice").val("");
            $("#" + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #hfPractice").val("");

        }

    },

    UnLoadTab: function () {

        if (OrderSet_Patient_Referrals_Outgoing_Detail.IsAnyChange == true && OrderSet_Patient_Referrals_Outgoing_Detail.params["ParentCtrl"] == "Clinical_OrderSetDetails") {
            Clinical_OrderSetDetails.LoadOrderSetReferrals();
        }

        if (OrderSet_Patient_Referrals_Outgoing_Detail.params != null && OrderSet_Patient_Referrals_Outgoing_Detail.params.ParentCtrl != null) {
            UnloadActionPan(OrderSet_Patient_Referrals_Outgoing_Detail.params.ParentCtrl, 'OrderSet_Patient_Referrals_Outgoing_Detail');
        }
        else
            UnloadActionPan(null, 'OrderSet_Patient_Referrals_Outgoing_Detail');


    },

    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmPatientReferralsOutgoingDetail";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["RefCtrlHidden"] = "hfProvider";
        params["ParentCtrl"] = "OrderSet_Patient_Referrals_Outgoing_Detail";
        LoadActionPan('Admin_Provider', params);
    },

    OpenRefProvider: function () {
        var params = [];
        params["ReferringProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefForm"] = "frmPatientReferralsOutgoingDetail";
        params["IsOptional"] = true;
        params["RefCtrl"] = "txtRefProvider";
        params["RefCtrlHidden"] = "hfRefProvider";
        params["ParentCtrl"] = "OrderSet_Patient_Referrals_Outgoing_Detail";
        LoadActionPan('Admin_ReferringProvider', params);
    },

    OpenAssignee: function () {
        CacheManager.BindCodes('GetUsersFullName', true).done(function (result) {
            var Ctrl = $('#' + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #txtAssignee");
            var hfCtrl = $('#' + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #hfAssignee");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", GetUsersFullName, null, hfCtrl);
        });
    },

    LoadAllAutocomplete: function () {
        //CacheManager.BindCodes('GetRefProviders', false).done(function (result) {
        //    $("input#txtRefProvider").autocomplete({
        //        autoFocus: true,
        //        source: RefProviders, // pass an array
        //        select: function (event, ui) {
        //            setTimeout(function () {
        //                $("#pnlOrderSetPatientReferralsOutgoingDetail #txtRefProvider").attr("RefProviderId", ui.item.id); // add the selected id
        //                $("#pnlOrderSetPatientReferralsOutgoingDetail #hfRefProvider").val(ui.item.id); // add the selected id
        //                if ($("#pnlOrderSetPatientReferralsOutgoingDetail #lnkProviderEdit").css("display") == "none") {
        //                    $("#pnlOrderSetPatientReferralsOutgoingDetail #lnkProviderEdit").css("display", "inline");
        //                    $("#pnlOrderSetPatientReferralsOutgoingDetail #lblRefProvider").css("display", "none");
        //                }
        //            }, 100);
        //        }
        //    });
        //});

        CacheManager.BindCodes('GetAllProviders', false).done(function (result) {
            var Ctrl = $('#' + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail input#txtProvider");
            var hfCtrl = $("#pnlOrderSetPatientReferralsOutgoingDetail #hfProvider");
            var onSelect = function (dataItem) { $("#pnlOrderSetPatientReferralsOutgoingDetail #txtProvider").attr("ProviderId", dataItem.id); }
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", AllProviders, null, hfCtrl, onSelect);
        });


        //CacheManager.BindCodes('GetRefProviders', false).done(function (result) {
        //    $("input#txtRefProvider").autocomplete({
        //        autoFocus: true,
        //        minLength: 3,  //added min length, Abdur Rehman Latif - PMS 2986
        //        source: RefProviders, // pass an array
        //        select: function (event, ui) {
        //            setTimeout(function () {
        //                $("#pnlOrderSetPatientReferralsOutgoingDetail #hfRefProvider").val(ui.item.id);
        //                if ($("#pnlOrderSetPatientReferralsOutgoingDetail #lnkRefProviderEdit").css("display") == "none") {
        //                    $("#pnlOrderSetPatientReferralsOutgoingDetail #lnkRefProviderEdit").css("display", "inline");
        //                    $("#pnlOrderSetPatientReferralsOutgoingDetail #lblRefProvider").css("display", "none");
        //                }
        //            }, 100);

        //        }
        //    });
        //});



        var Ctrl = $('#' + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail input#txtRefProvider");
        var hfCtrl = $('#' + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #hfRefProvider")
        var func = function () { return utility.GetRefProviderArray(Ctrl.val()) };
        utility.BindKendoAutoComplete(Ctrl, 3, "value", "contains", null, func, hfCtrl);




        //Begin Edit by Fahad Malik 19-Dec-2016, Bug# EMR-2277
        CacheManager.BindCodes('GetFacilityOutgoingReferral', false).done(function (result) {
            var Ctrl = $("#pnlOrderSetPatientReferralsOutgoingDetail input#txtFacilityFrom");
            var hfCtrl = $("#pnlOrderSetPatientReferralsOutgoingDetail #hfFacilityFrom");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", FacilitiesOutgoingReferral, null, hfCtrl);
        });
        CacheManager.BindCodes('GetFacilityOutgoingReferral', false).done(function (result) {
            var Ctrl = $("#pnlOrderSetPatientReferralsOutgoingDetail input#txtFacilityTo");
            var hfCtrl = $("#pnlOrderSetPatientReferralsOutgoingDetail #hfFacilityTo");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", FacilitiesOutgoingReferral, null, hfCtrl);
        });

        //End Edit by Fahad Malik 19-Dec-2016, Bug# EMR-2277


        //CacheManager.BindCodes('GetUsersFullName', false).done(function (result) {
        //    $("#pnlOrderSetPatientReferralsOutgoingDetail #txtAssignee").autocomplete({
        //        autoFocus: true,
        //        source: GetUsersFullName, // pass an array
        //        select: function (event, ui) {

        //            setTimeout(function () {
        //                $("#pnlOrderSetPatientReferralsOutgoingDetail #txtAssignee").attr("ProviderId", ui.item.id); // add the selected id
        //                $("#pnlOrderSetPatientReferralsOutgoingDetail #hfAssignee").val(ui.item.id);
        //            }, 100);
        //        }
        //    });


        //});


        CacheManager.BindCodes('GetSpecialtiesAllEntitiesReferals', false).done(function (result) {

            $("input#txtSpecialtyFrom").autocomplete({
                autoFocus: true,
                source: Specialities, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#pnlOrderSetPatientReferralsOutgoingDetail #hfSpecialtyFrom").val(ui.item.id); // add the selected id

                    }, 100);
                }
            });
        });

    },

    OutGoingReferralSave: function () {
        if (EMRUtility.compareFormDataWithSerialized(OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + ' #frmPatientReferralsOutgoingDetail')) {
            var OrderSetReferralId = $('#' + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #hfOrderSetReferralId").val() != "" ? $('#' + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #hfOrderSetReferralId").val() : "-1";

            if (parseInt(OrderSetReferralId) > 0) {
                OrderSet_Patient_Referrals_Outgoing_Detail.params.mode = "Edit";
                OrderSet_Patient_Referrals_Outgoing_Detail.params.OrderSetReferralId = OrderSetReferralId;
            }
            else {
                OrderSet_Patient_Referrals_Outgoing_Detail.params.mode = "Add";
            }

            if ($('#' + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #txtRefProvider").val() != '' || $('#' + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #txtSpecialtyFrom").val() != '') {
                var self = $('#' + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail");





                var ProcedureIds = $("#" + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral tbody tr").map(function () {
                    return this.id.replace("id", "");
                }).get().join(',');
                $("#" + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #pnlReferralProcedure_Result #hfReferralProcedureIds").val(ProcedureIds);

                var ReferralProcedure = [];
                if ($("#" + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral tbody tr").length == 1 && $($("#" + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral tbody tr")[0]).find("td").length == 1) {
                    ProcedureIds = "";
                    ReferralProcedure = [];
                }
                else {

                    $("#" + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral tbody tr").each(function (index) {
                        ProcedureId = $(this).attr("id");
                        var ProcedureRow = {};
                        var ParsedProcedureJson = JSON.parse($(this).getMyJSONByName());
                        ProcedureRow["ReferralProcedureId"] = ProcedureId;
                        ProcedureRow["Urgency"] = ParsedProcedureJson.Urgency;
                        ProcedureRow["Procedure"] = ParsedProcedureJson.Procedure;
                        ProcedureRow["CPTCode"] = $($("#" + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral tbody tr")[index]).attr('cptcode');
                        ProcedureRow["CPTCodeDescription"] = $($("#" + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral tbody tr")[index]).attr('cptdescription');
                        ReferralProcedure.push(ProcedureRow);
                    });
                }

                var myJSON = self != null ? self.getMyJSONByName() : "{}";
                var objData = JSON.parse(myJSON);
                objData["ReferralProcedure"] = ReferralProcedure
                objData["Type"] = "Outgoing";
                objData["OrderSetId"] = OrderSet_Patient_Referrals_Outgoing_Detail.params['OrderSetId'];
                myJSON = JSON.stringify(objData);

                //--------------------------------------------------------------

                if (OrderSet_Patient_Referrals_Outgoing_Detail.params.mode == "Add") {
                    var strMessage = "";
                    AppPrivileges.GetFormPrivileges("Referrals", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            OrderSet_Patient_Referrals_Outgoing_Detail.SaveReferral(myJSON).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    utility.DisplayMessages(response.Message, 1);
                                    $('#' + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #hfOrderSetReferralId").val(response.OrderSetReferralId);
                                    OrderSet_Patient_Referrals_Outgoing_Detail.params.OrderSetReferralId = response.OrderSetReferralId;
                                    OrderSet_Patient_Referrals_Outgoing_Detail.params.mode = "Edit";
                                    OrderSet_Patient_Referrals_Outgoing_Detail.EditModeSettings();
                                    OrderSet_Patient_Referrals_Outgoing_Detail.IsAnyChange = true;
                                    OrderSet_Patient_Referrals_Outgoing_Detail.UnLoadTab();
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                        else {
                            utility.DisplayMessages(strMessage, 2);
                        }
                    });
                }
                else if (OrderSet_Patient_Referrals_Outgoing_Detail.params.mode == "Edit") {
                    var strMessage = "";
                    AppPrivileges.GetFormPrivileges("Referrals", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            OrderSet_Patient_Referrals_Outgoing_Detail.updateReferral(myJSON, OrderSetReferralId).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {

                                    utility.DisplayMessages(response.Message, 1);
                                    OrderSet_Patient_Referrals_Outgoing_Detail.IsAnyChange = true;
                                    OrderSet_Patient_Referrals_Outgoing_Detail.UnLoadTab();
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                        else {
                            utility.DisplayMessages(strMessage, 2);
                        }
                    });

                }
            }
            else {
                utility.DisplayMessages("'Either enter 'Referral To' or 'Referral To Specialty' to proceed.", 3);
            }
        } else {
            utility.DisplayMessages("Please make any changes to save/update Outgoing Referral", 3);
            setTimeout(function () {
                $('#' + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + ' #frmPatientReferralsOutgoingDetail').data('serialize', $('#' + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + ' #frmPatientReferralsOutgoingDetail').serialize());
            }, 100);
        }
    },

    FillOrderSetReferrals: function (OrderSetReferralId, OrderSetId) {

        OrderSet_Patient_Referrals_Outgoing_Detail.fillReferral(OrderSetReferralId, OrderSetId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var referralsJSON = response.OrderSetReferralsJSON[0];
                $self = $('#' + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID)
                utility.bindMyJSONByName(true, referralsJSON, false, $self);
                OrderSet_Patient_Referrals_Outgoing_Detail.ProcedureGridLoad(response);
            }
            else
                utility.DisplayMessages(response.Message, 3);

        });

    },

    SaveReferral: function (ReferralData) {
        var objData = JSON.parse(ReferralData);
        objData["commandType"] = "save_OrderSetReferral";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "OrderSetPatientReferralsOutgoingDetail");
    },

    updateReferral: function (ReferralData, OrderSetReferralId) {

        var objData = JSON.parse(ReferralData);
        objData["OrderSetReferralId"] = OrderSetReferralId;
        objData["commandType"] = "update_OrderSetReferral";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "OrderSetPatientReferralsOutgoingDetail");

    },

    fillReferral: function (OrderSetReferralId, OrderSetId) {

        var objData = new Object();
        objData["OrderSetReferralId"] = OrderSetReferralId;
        objData["OrderSetId"] = OrderSetId;
        objData["commandType"] = "fill_OrderSetReferral";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "OrderSetPatientReferralsOutgoingDetail");

    },


    DeleteReferralProcedure_DBCall: function (ReferralProcedureId) {

        var objData = new Object();
        objData["OrderSetReferralProcedureId"] = ReferralProcedureId;
        objData["commandType"] = "DELETE_REFERRAL_PROCEDURE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "OrderSetPatientReferralsOutgoingDetail");

    },

    printReferral: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["UserId"] = globalAppdata['AppUserId'];
        params["ParentCtrl"] = "OrderSet_Patient_Referrals_Outgoing_Detail";
        params["OrderSetReferralId"] = OrderSet_Patient_Referrals_Outgoing_Detail.params.OrderSetReferralId;
        LoadActionPan('Patient_ReferralsView', params);
    },

    OpenFacilityTo: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmPatientReferralsOutgoingDetail";
        params["FacilityTo"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacilityTo";
        params["RefHiddenIdCtrl"] = "hfFacilityTo";
        params["LoadAllFacility"] = "True";
        params["ParentCtrl"] = "OrderSet_Patient_Referrals_Outgoing_Detail";
        LoadActionPan('Admin_Facility', params);
    },

    OpenFacilityFrom: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmPatientReferralsOutgoingDetail";
        params["FacilityFrom"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacilityFrom";
        params["RefHiddenIdCtrl"] = "hfFacilityFrom";
        params["ParentCtrl"] = "OrderSet_Patient_Referrals_Outgoing_Detail";
        LoadActionPan('Admin_Facility', params);
    },

    documentImport: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Documents", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];

                params["RefCtrl"] = "OrderSetOutgoingReferral";
                params['OrderSetReferralId'] = OrderSet_Patient_Referrals_Outgoing_Detail.params.OrderSetReferralId;
                params['RefModuleName'] = "Outgoing Referral";
                params['TransitionId'] = 0;
                params["FromAdmin"] = "0";
                params["mode"] = "Add";
                params["ParentCtrl"] = 'OrderSet_Patient_Referrals_Outgoing_Detail';
                params["PatientName"] = "";
                params["AccountNo"] = "";
                params["PatientId"] = 0;
                LoadActionPan('Document_Import', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });


    },

    documentScan: function () {
        AppPrivileges.GetFormPrivileges("Documents", "SCAN", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var param = [];
                param["mode"] = "Scan";
                param["RefCtrl"] = "OrderSetOutgoingReferral";
                param['OrderSetReferralId'] = OrderSet_Patient_Referrals_Outgoing_Detail.params.OrderSetReferralId;
                param['RefModuleName'] = "Outgoing Referral";
                param['TransitionId'] = 0;
                params['PatientID'] = 0;
                param["ParentCtrl"] = 'OrderSet_Patient_Referrals_Outgoing_Detail';
                LoadActionPan('Document_Scan', param);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    loadAttachments: function (controlName, OrderSetReferralId, resultId, tableId) {

        OrderSet_Patient_Referrals_Outgoing_Detail.loadAttachments_DbCall(OrderSetReferralId).done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {

                var ulAttachment = null;

                if (controlName == null)
                    ulAttachment = $('#' + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + ' #menuViewAttachment');
                else {
                    var control = eval(controlName.trim());
                    if (tableId != null) {

                        ulAttachment = $('#' + control.params.PanelID + " #" + tableId.trim() + " tr td").find('div.dropdown').find("#menuViewAttachment" + resultId);
                        if ($('#' + control.params.PanelID + " #" + tableId.trim()).parent() != null) {
                            $('#' + control.params.PanelID + " #" + tableId.trim()).parent().removeClass('Of-a');
                        }
                    }
                }
                $(ulAttachment).empty();
                if (response.AttachmentCount > 0) {
                    var attachments = JSON.parse(response.AttachmentLoad_JSON);

                    $(attachments).each(function (index, item) {
                        if (controlName == null)
                            $(ulAttachment).append('<li><a href="#" id="' + item.PatDocId + '" onclick="OrderSet_Patient_Referrals_Outgoing_Detail.showAttachment(\'' + item.PatDocId + '\',event)">' + item.ModifiedOn + '</a></li>');
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

    loadAttachments_DbCall: function (OrderSetReferralId) {

        var objData = {};
        objData["OrderSetReferralId"] = OrderSet_Patient_Referrals_Outgoing_Detail.params.OrderSetReferralId;
        objData["RefModuleName"] = "Outgoing Referral";
        //objData["PatientId"] = $("div#PatientProfile #hfPatientId").val();

        objData["commandType"] = "load_attachments";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");
    },

    showAttachment: function (PatDocID, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Documents", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                //params["PatientID"] = $('#PatientProfile #hfPatientId').val();
                params["PatDocID"] = PatDocID;
                params["mode"] = "Edit";
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = "OrderSet_Patient_Referrals_Outgoing_Detail";

                LoadActionPan('Document_Viewer', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    referralReset: function () {
        utility.myConfirm('22', function () {

            //selectors
            var form = "#" + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail";

            var $orderInfomation = $(form + " #divReferralInfo");
            var $problems = $(form + " #divProblems");

            $orderInfomation.resetAllControls(null);
            $problems.resetAllControls(null);

            utility.CreateDatePicker(OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + ' #dtDate', function () {
                //on-change callback method 
            }, true);
            utility.CreateDatePicker(OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + ' #dtDateFrom', function () {
                //on-change callback method 
            }, true);
            utility.CreateDatePicker(OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + ' #dtDateTo', function () {
                //on-change callback method 
            }, true);
            utility.ValidateFromToDate('frmPatientReferralsOutgoingDetail', "dtDateFrom", "dtDateTo", true);
            $('#' + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + ' #tmTime').timepicker({
                defaultTime: new Date()
            });

            //revalidate the required fields
            $(form).bootstrapValidator('revalidateField', 'Status').bootstrapValidator('revalidateField', 'RefProvider');

        }, function () { },
            '22'
        );
    },

    OpenSpecialtyFrom: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmPatientReferralsOutgoingDetail";
        params["FacilityFrom"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtSpecialtyFrom";
        params["RefHiddenIdCtrl"] = "hfSpecialtyFrom";
        params["ParentCtrl"] = "OrderSet_Patient_Referrals_Outgoing_Detail";
        LoadActionPan('Admin_Specialty', params);
    },


    BindAutoSpecialityReferralcomplete: function () {
        var Ctrl = $("#" + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + ' #txtSpecialtyFrom');
        var func = function () { return OrderSet_Patient_Referrals_Outgoing_Detail.GetSpecialityArray(Ctrl.val()) };
        var hfCtrl = $('#' + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + ' #hfSpecialtyFrom');
        utility.BindKendoAutoComplete(Ctrl, 3, "value", "contains", null, func, hfCtrl);
    },
    GetSpecialityArray: function (Speciality) {
        var AllSpecialities = [];
        var IsValid = false;

        if (Speciality != null && Speciality.length > 2) {
            IsValid = true;
        }

        else {
            IsValid = false;
        }



        var dfd = new $.Deferred();
        if (IsValid) {
            // serach parameter , class name, command name of class
            OrderSet_Patient_Referrals_Outgoing_Detail.GetSpecialityArray_DBCALL(Speciality).done(function (responseData) {
                if (responseData.status != false) {
                    if (responseData.SpecialtyCount > 0) {
                        var SpecialtyLoadJSONData = JSON.parse(responseData.SpecialtyLoad_JSON);
                        $.each(SpecialtyLoadJSONData, function (i, item) {
                            AllSpecialities.push({ id: item.SpecialtyId, value: item.Description });
                        });
                    }
                }
                dfd.resolve(AllSpecialities);
            });
        }
        else {
            dfd.resolve(AllSpecialities);
        }

        return dfd.promise();

    },
    GetSpecialityArray_DBCALL: function (Speciality) {
        var objData = new Object();
        var data = 'SpecialtyData={"txtShortName":"","txtDescription":"' + Speciality + '","ddlEntity":"100","ddlEntity_text":"SHS","chkActive":"1","chkActive_text":"Active"}&SpecialtyID=undefined&PageNumber=1&RowsPerPage=15'
        return MDVisionService.defaultService(data, "ADMIN_SPECIALTY", "SEARCH_SPECIALTY");
    },
    bindAutoComplete: function (element) {
        var hiddenCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "OrderSet_Patient_Referrals_Outgoing_Detail", null, true);
    },
    openCPTCode: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "OrderSet_Patient_Referrals_Outgoing_Detail";
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtCPTCode";
        params["ParentCtrlPanelID"] = OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID);
    },
    BindProcedureGridItem: function (cptCode, procedureDescription, cptDescription, SnomedID, SnomedDescription) {

        var cptCode = cptCode;
        var procDesc = procedureDescription;
        var cptDesc = cptDescription;

        var currentRow = $("#" + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral tbody tr");
        var isTestAlreadySelected = false;
        $.each(currentRow, function (i, item) {
            var currentRowCPTDescription = $(item).find("input[id*='Procedure']").val() != null ? $(item).find("input[id*='Procedure']").val().replace(cptCode, "").trim() : "";
            var currentRowCPTDescription = currentRowCPTDescription.split('(')[0].trim();
            if (cptDescription.toLowerCase() == currentRowCPTDescription.toLowerCase()) {
                isTestAlreadySelected = true;
                return;
            }
        });

        if (isTestAlreadySelected != true) {

            OrderSet_Patient_Referrals_Outgoing_Detail.AddNewProcedureRow(null, null, null, cptCode, procDesc, cptDescription, SnomedID, SnomedDescription, null, true);
            setTimeout(function () {
                $("#" + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #txtReferralCPTCode").val('');
            }, 200);
        }
        else {
            utility.DisplayMessages("This code already exists in the referral.", 2);
        }
    },
    AddNewProcedureRow: function (RowId, mode, CurrRef, cptCode, procDesc, cptDescription, SnomedId, SnomedDescription, UrgencyId, fromInput) {

        var ConsultationGridId = "#" + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral";

        var CurrentRow = null;
        if (RowId && RowId > 0) {
            if (OrderSet_Patient_Referrals_Outgoing_Detail.params.ParentCtrl != null) {
                CurrentRow = OrderSet_Patient_Referrals_Outgoing_Detail.EditableGrid.rowAdd(RowId, "");
            }
            else {
                CurrentRow = OrderSet_Patient_Referrals_Outgoing_Detail.EditableGrid.rowAdd(RowId, OrderSet_Patient_Referrals_Outgoing_Detail.params.ConsultationId);
            }
        }
        else {
            if (OrderSet_Patient_Referrals_Outgoing_Detail.params.ParentCtrlPanelID != undefined)
                var TemplateRow = $("#" + OrderSet_Patient_Referrals_Outgoing_Detail.params.ParentCtrlPanelID + " #dgvProcedureReferral tbody tr[id*=-]").last();
            else
                var TemplateRow = $("#" + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral tbody tr[id*=-]").last();
            var TemplateRowId = 0;
            if (TemplateRow.length > 0) {
                if (TemplateRow.attr("id").indexOf("Child") != "-1")
                    TemplateRowId = parseInt(TemplateRow.prev().attr("id"));
                else
                    TemplateRowId = parseInt(TemplateRow.attr("id"));
            }
            CurrentRow = OrderSet_Patient_Referrals_Outgoing_Detail.EditableGrid.rowAdd(TemplateRowId - 1, "");
        }

        $(CurrentRow).attr("CptCode", cptCode);
        $(CurrentRow).attr("CptDescription", cptDescription);
        $(CurrentRow).attr("SnomedId", SnomedId);
        $(CurrentRow).attr("SnomedDescription", SnomedDescription);
        $(CurrentRow).find('input[id*="Procedure"]').val(cptCode != "" ? cptCode + " " + cptDescription : cptDescription);
        if (fromInput) {
            utility.callbackAfterAllDOMLoaded(function () {
                $(CurrentRow).find('select[id*="Urgency"] option').map(function () {
                    if ($(this).text() == "Normal") return this;
                }).attr('selected', 'selected');
            });
        }
        else
            $(CurrentRow).attr("UrgencyId", UrgencyId);
        OrderSet_Patient_Referrals_Outgoing_Detail.enableRemoveRow($(CurrentRow));

        return CurrentRow;
    },
    enableRemoveRow: function (CurrentRow) {
        CurrentRow.find("td.actions .remove-row").removeClass("hidden");
        //    .each(function () {
        //    $(this).removeclass('hidden')
        //});
    },
    rowRemove: function ($row, obj) {
        //var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Referrals", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        utility.myConfirm('1', function () {
            if ($row.hasClass('adding')) {
            }
            //var _self = obj;
            //_self.datatable.row($row.get(0)).remove().draw();
            if (parseInt($row.attr("id")) > 0) {
                OrderSet_Patient_Referrals_Outgoing_Detail.DeleteReferralProcedure($row.attr("id"), $row, obj);
                //OrderSet_Patient_Referrals_Outgoing_Detail.loadReferralData();
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
        //    }
        //    else {
        //        utility.DisplayMessages(strMessage, 2);
        //    }
        //});

    },
    DeleteReferralProcedure: function (ReferralProcedureId, $row, obj) {

        OrderSet_Patient_Referrals_Outgoing_Detail.DeleteReferralProcedure_DBCall(ReferralProcedureId).done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {
                utility.DisplayMessages(response.Message, 1);
                var _self = obj;
                _self.datatable.row($row.get(0)).remove().draw();
            } else {
                utility.DisplayMessages(response.Message, 3);
            }

        });

    },



    ProcedureGridLoad: function (response) {
        var dfd = $.Deferred();
        var PanelReferralGrid = "#" + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #pnlReferralProcedure_Result";
        var ReferralGridId = "#" + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral";
        $(ReferralGridId + " tbody tr").remove();
        var arraTemp = [];
        if ($.fn.dataTable.isDataTable(ReferralGridId)) {
            $(ReferralGridId).dataTable().fnClearTable();
            $(ReferralGridId).dataTable().fnDestroy();
        }
        if (response.OrderSetReferralProcedureCount > 0) {
            OrderSet_Patient_Referrals_Outgoing_Detail.EditableGrid.datatable.clear().draw();
            var ReferralProcedureLoadJSONData = response.OrderSetReferralProcedureJSON;
            $.each(ReferralProcedureLoadJSONData, function (i, item) {
                var _dfd = $.Deferred();
                var CurrentRow = OrderSet_Patient_Referrals_Outgoing_Detail.AddNewProcedureRow(item.OrderSetReferralProcedureId, null, null, item.CPTCode, null, item.CPTCodeDescription, null, null, item.Urgency, false);
                var self = $("#" + OrderSet_Patient_Referrals_Outgoing_Detail.params.PanelID + " tr#" + item.ReferralProcedureId);
                //$(self).loadDropDowns(true).done(function () {
                utility.bindMyJSONByName(true, item, false, $(CurrentRow)).done(function () {
                    _dfd.resolve();
                });
                //});
                arraTemp.push(_dfd);
            });
            $.when.apply($, arraTemp).then(function () {
                dfd.resolve();
            });
        }
        else {
            $(ReferralGridId).DataTable({
                "language": {
                    "emptyTable": "No Procedures Found."
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bDestroy": true, "bSort": false
            });
            dfd.resolve();
        }
        return dfd;
    },
}