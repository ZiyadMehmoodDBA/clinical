Patient_Referrals_Outgoing_Detail = {
    bIsFirstLoad: true,
    params: [],
    EditableGrid: null,
    ReferralProblems: [],
    ReferralProcedureChange: false,
    ReferralProblemsChange: false,
    SaveorSavePrintButtonClicked: false,
    Load: function (params) {
        Patient_Referrals_Outgoing_Detail.params = params;

        if (Patient_Referrals_Outgoing_Detail.params.PanelID != 'pnlPatientReferralsOutgoingDetail') {
            Patient_Referrals_Outgoing_Detail.params.PanelID = Patient_Referrals_Outgoing_Detail.params.PanelID + ' #pnlPatientReferralsOutgoingDetail';
        } else {
            Patient_Referrals_Outgoing_Detail.params.PanelID = 'pnlPatientReferralsOutgoingDetail';
        }
        if (Patient_Referrals_Outgoing_Detail.params.ParentCtrl == 'Clinical_Treatment') {
            $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #SavePrintButton").addClass("hidden");
            $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #faxButton").addClass("hidden");
        }
        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        if (Patient_Referrals.params["ParentCtrl"] == "Clinical_FaceSheet") {
            $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #hfPatientId").val(Clinical_FaceSheet.params.patientID);
        }
        utility.CreateDatePicker(Patient_Referrals_Outgoing_Detail.params.PanelID + ' #dtDate', function () {
            //on-change callback method
        }, true);
        utility.CreateDatePicker(Patient_Referrals_Outgoing_Detail.params.PanelID + ' #dtDateFrom', function () {
            //on-change callback method
        }, true);
        utility.CreateDatePicker(Patient_Referrals_Outgoing_Detail.params.PanelID + ' #dtDateTo', function () {
            //on-change callback method
        }, true);
        utility.ValidateFromToDate('frmPatientReferralsOutgoingDetail', "dtDateFrom", "dtDateTo", true);
        $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + ' #tmTime').timepicker({
            defaultTime: new Date()
        });


        if (Patient_Referrals_Outgoing_Detail.params.ParentCtrl == "schcheckout") {
            $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #hfPatientId").val(Patient_Referrals_Outgoing_Detail.params.PatientId);
            //$("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #txtProvider").val(Patient_Referrals_Outgoing_Detail.params.ReferralTo);
            //$("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #hfProvider").val(Patient_Referrals_Outgoing_Detail.params.ReferralToId);
            //$("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #txtReason").val(Patient_Referrals_Outgoing_Detail.params.Reason);

            //$("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #ddlReferralStatus").val(1);
        }

        var PanelReferralGrid = "#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #pnlConsultation_Result";
        var ReferralGridId = "#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral";
        //$(ChargeGridId).dataTable().fnDestroy();
        $(ReferralGridId + " tbody tr").remove();
        Patient_Referrals_Outgoing_Detail.EditableGrid = EMRUtility.MakeEditableGrid(PanelReferralGrid, ReferralGridId, Patient_Referrals_Outgoing_Detail, "0", false, false, false, false);
        //        Patient_Referrals_Outgoing_Detail.LoadAllAutocomplete();
        //CacheManager.BindDropDownsByID($('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + ' #frmPatientReferralsOutgoingDetail #ddlAssigneeId'), 'GetUsers', true, 1).done(function () {
        //    $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #ddlAssigneeId option:first").text('- select -');

        //});

        $($("#dgvProcedureReferral")[0].parentNode).removeClass('Of-a');

        Patient_Referrals_Outgoing_Detail.ReferralProblems = [];
        Patient_Referrals_Outgoing_Detail.DomeReady();
        var self = $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID);
        if (Patient_Referrals_Outgoing_Detail.bIsFirstLoad == true) {
            self.loadDropDowns(true).done(function () {
                var $options = $(self).find("#ddlFacilityTo > option").clone();
                $(self).find('#ddlFacilityFrom').append($options);

                if (Patient_Referrals_Outgoing_Detail.params.ParentCtrl == "mstrTabDashBoard") {
                    $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #hfPatientId").val(Patient_Referrals_Outgoing_Detail.params.PatientId);
                }

                $.when(Patient_Referrals_Outgoing_Detail.loadProblemList()).then(function () {
                    if (Patient_Referrals_Outgoing_Detail.params.mode == "Edit") {
                        $.when(Patient_Referrals_Outgoing_Detail.loadReferralData()).then(function () {
                            setTimeout(function () {
                                $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #PrintButton").prop("disabled", false);
                                $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail").data('serialize', $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail").serialize());
                            }, 200)

                        });
                    }
                    else {
                        $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail").data('serialize', $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail").serialize());
                    }

                });
                Patient_Referrals_Outgoing_Detail.ValidateOutGoingReferrals();
                $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail").data('serialize', $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail").serialize());
            });
            Patient_Referrals_Outgoing_Detail.BindFacilityFrom();
            Patient_Referrals_Outgoing_Detail.BindFacilityTo();
            Patient_Referrals_Outgoing_Detail.BindReferralFrom();
            Patient_Referrals_Outgoing_Detail.BindReferralTo();
            Patient_Referrals_Outgoing_Detail.BindSpecialty();
            Patient_Referrals_Outgoing_Detail.OpenAssignee();
        }

        if (Patient_Referrals_Outgoing_Detail.params.mode == "Add") {

            $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #menuAttach").remove();
            $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #attachmentDiv").html('<a id="btnViewAttachment" href="#" class="dropdown-toggle btn btn-link btn-xs p-none" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false" onclick="">View Attachment</a>');
            $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #btnScanResult,#btnViewAttachment").addClass("disableAll");
            if (Patient_Referrals.params.ParentCtrl == "clinicalTabProgressNote") {
                $("#" + Patient_Referrals.params.PanelID + " #frmPatientReferralsOutgoingDetail #txtFacilityFrom").val(Clinical_ProgressNote.params["CurrentNotesFacilityDescription"]);
                $("#" + Patient_Referrals.params.PanelID + " #frmPatientReferralsOutgoingDetail #txtProvider").val(Clinical_ProgressNote.params["CurrentNotesProviderText"]);
                $("#" + Patient_Referrals.params.PanelID + " #frmPatientReferralsOutgoingDetail #hfFacilityFrom").val($("#pnlClinicalProgressNote #hfFacilityId").val());
                $("#" + Patient_Referrals.params.PanelID + " #frmPatientReferralsOutgoingDetail #hfProvider").val(Clinical_ProgressNote.params["CurrentNotesProviderId"]);
            }
            else {
                $("#" + Patient_Referrals.params.PanelID + " #frmPatientReferralsOutgoingDetail #txtFacilityFrom").val(globalAppdata["DefaultFacilityDescription"]);
                if (globalAppdata['DefaultProviderName'] != "- Select -" && globalAppdata['DefaultProviderName'] != "") {
                    $("#" + Patient_Referrals.params.PanelID + " #frmPatientReferralsOutgoingDetail #txtProvider").val(globalAppdata["DefaultProviderName"]);
                }
                $("#" + Patient_Referrals.params.PanelID + " #frmPatientReferralsOutgoingDetail #hfFacilityFrom").val(globalAppdata["DefaultFacilityId"]);
                $("#" + Patient_Referrals.params.PanelID + " #frmPatientReferralsOutgoingDetail #hfProvider").val(globalAppdata["DefaultProviderId"]);
            }
            var $ctr = $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #txtFacilityFrom");
            var $hfctr = $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #hfFacilityFrom");
            utility.SetKendoAutoCompleteSourceforValidate($ctr, $ctr.val(), $hfctr, $hfctr.val());

            $ctr = $("#" + Patient_Referrals.params.PanelID + " #frmPatientReferralsOutgoingDetail #txtProvider");
            $hfctr = $("#" + Patient_Referrals.params.PanelID + " #frmPatientReferralsOutgoingDetail #hfProvider");
            utility.SetKendoAutoCompleteSourceforValidate($ctr, $ctr.val(), $hfctr, $hfctr.val());
        }
        else if (Patient_Referrals_Outgoing_Detail.params.mode == "Edit") {
            $("#faxButton").removeClass('disabled');

            $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #attachDiv").append('<ul id="menuAttach" class="dropdown-menu" aria-labelledby="btnScanResult">' +
                '<li><a href="#" onclick="Patient_Referrals_Outgoing_Detail.documentScan()">Scan</a></li><li><a href="#" onclick="Patient_Referrals_Outgoing_Detail.documentImport()">Upload</a></li></ul>');

            $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #attachmentDiv").html('<a id="btnViewAttachment" href="#" class="dropdown-toggle btn btn-link btn-xs p-none" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false" onclick="Patient_Referrals_Outgoing_Detail.loadAttachments()">View Attachment</a><ul id="menuViewAttachment" class="dropdown-menu" aria-labelledby="btnViewAttachment"></ul>');
            $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #btnScanResult,#btnViewAttachment").removeClass("disableAll");
        }
        $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail").data('serialize', $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail").serialize());

        if (globalAppdata.IsMedText == "True") {
            $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #divbtnSendWithOptions").removeClass("hidden");
        }
        else {
            $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #btnSend").removeClass("hidden");
        }

        if (Patient_Referrals_Outgoing_Detail.params["IsViewOnly"]
           && Patient_Referrals_Outgoing_Detail.params["IsViewOnly"] == true) {
            $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail .canDisable").addClass("disableAll");
            $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #divProcedures").addClass("disableAll");
            $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #div_prob").addClass("disableAll");

        }
    },
    DomeReady: function () {
        $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral").change(function () {
            Patient_Referrals_Outgoing_Detail.ReferralProcedureChange = true;

        });

        $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #ulProblemLists").change(function () {
            Patient_Referrals_Outgoing_Detail.ReferralProblemsChange = true;
        });
    },
    ValidateOutGoingReferrals: function () {

        var $self = $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail");
        var fields = {};

        //fields["RefProvider"] = {
        //    group: '.col-sm-3',
        //    validators: {
        //        notEmpty: {
        //            message: ''
        //        }
        //    }
        //},
        $self.bootstrapValidator({
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
            Patient_Referrals_Outgoing_Detail.OutGoingReferralSave();
        });
    },
    removeFacility: function (ctrl) {
        if ($(ctrl).val() == "") {
            $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #txtPractice").val("");
            $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #hfPractice").val("");

        }

    },

    SendReferral: function (via) {
        $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #IsDraft").val("0");
        $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #SendVia").val(via);
        $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #btnSendRef").trigger("click");
    },


    loadReferralData: function () {
        var dfd = $.Deferred();
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Referrals", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Patient_Referrals_Outgoing_Detail.fillReferral(Patient_Referrals_Outgoing_Detail.params.ReferralId).done(function (response) {
                    if (response != "") {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            var ReferralDetail = JSON.parse(response.ReferralListLoad_JSON);

                            $.when(Patient_Referrals_Outgoing_Detail.ProcedureGridLoad(response)).then(function () {
                                var self = $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail");
                                utility.bindMyJSONByName(true, ReferralDetail, false, self).done(function () {

                                    var $ctr = $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #txtFacilityFrom");
                                    var $hfctr = $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #hfFacilityFrom");
                                    utility.SetKendoAutoCompleteSourceforValidate($ctr, $ctr.val(), $hfctr, $hfctr.val());

                                    $ctr = $("#" + Patient_Referrals.params.PanelID + " #frmPatientReferralsOutgoingDetail #txtProvider");
                                    $hfctr = $("#" + Patient_Referrals.params.PanelID + " #frmPatientReferralsOutgoingDetail #hfProvider");
                                    utility.SetKendoAutoCompleteSourceforValidate($ctr, $ctr.val(), $hfctr, $hfctr.val());

                                    $ctr = $("#" + Patient_Referrals.params.PanelID + " #frmPatientReferralsOutgoingDetail #txtFacilityTo");
                                    $hfctr = $("#" + Patient_Referrals.params.PanelID + " #frmPatientReferralsOutgoingDetail #hfFacilityTo");
                                    utility.SetKendoAutoCompleteSourceforValidate($ctr, $ctr.val(), $hfctr, $hfctr.val());

                                    $ctr = $("#" + Patient_Referrals.params.PanelID + " #frmPatientReferralsOutgoingDetail #txtRefProvider");
                                    $hfctr = $("#" + Patient_Referrals.params.PanelID + " #frmPatientReferralsOutgoingDetail #hfRefProvider");
                                    utility.SetKendoAutoCompleteSourceforValidate($ctr, $ctr.val(), $hfctr, $hfctr.val());

                                    $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + ' #dtDateFrom').datepicker('setDate', ReferralDetail.DateFrom);
                                    $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + ' #dtDateTo').datepicker('setDate', ReferralDetail.DateTo);
                                    if (response.ReferralProblemListLoad_JSON != null) {
                                        response.ReferralProblemListLoad_JSON = JSON.parse(response.ReferralProblemListLoad_JSON);
                                        for (var index in response.ReferralProblemListLoad_JSON) {
                                            $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #ulProblemLists td #chk" + response.ReferralProblemListLoad_JSON[index].ProblemId).attr("checked", "checked");
                                        }
                                    }
                                    $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #hfReferralID").val(ReferralDetail.ReferralId);
                                    $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail").data('serialize', $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail").serialize());
                                    dfd.resolve();
                                });
                            });
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                            dfd.resolve();
                        }
                    }
                });
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
        return dfd.promise();
    },

    //Function Name: fillConsultationOrder
    //Author Name: Humaira Yousaf
    //Created Date: 18-03-2016
    //Description: Fills ConsultationOrder
    //Params: ConsultationOrderId
    fillReferral: function (ReferralId) {
        var objData = {};
        objData["ReferralId"] = ReferralId;
        objData["LoadFor"] = "Edit";
        objData["Type"] = "Outgoing";
        objData["commandType"] = "SEARCH_REFERRAL";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");
    },


    //End Edit Work


    UnLoadTab: function () {

        // call the method OutGoingReferralSave() on close button if it will validate then save the record on close button
        // Faizan Ameen
        // IMP-584
        // Dated: 03-March-2017.

        // Patient_Referrals_Outgoing_Detail.OutGoingReferralSave();
        //if (!Patient_Referrals_Outgoing_Detail.SaveorSavePrintButtonClicked) {
        //    Patient_Referrals_Outgoing_Detail.OutGoingReferralSaveOnCloseButton();
        //}

        if (Patient_Referrals_Outgoing_Detail.params.ParentCtrl == 'patTabReferrals') {
            utility.UnLoadDialog("frmPatientReferralsOutgoingDetail", function () {
                UnloadActionPan(Patient_Referrals_Outgoing_Detail.params["ParentCtrl"], "Patient_Referrals_Outgoing_Detail");
            }, function () {
                UnloadActionPan(Patient_Referrals_Outgoing_Detail.params["ParentCtrl"], "Patient_Referrals_Outgoing_Detail");
            });
        } else if (Patient_Referrals_Outgoing_Detail.params.ParentCtrl == 'Patient_Referrals') {

            if (Patient_Referrals != null && Patient_Referrals.params.ParentCtrl != null && Patient_Referrals.params.ParentCtrl == "clinicalTabProgressNote") {
                utility.UnLoadDialog("frmPatientReferralsOutgoingDetail", function () {
                    UnloadActionPan(Patient_Referrals_Outgoing_Detail.params["ParentCtrl"], "Patient_Referrals_Outgoing_Detail", null, "pnlClinicalProgressNote #pnlPatientReferrals");
                }, function () {
                    UnloadActionPan(Patient_Referrals_Outgoing_Detail.params["ParentCtrl"], "Patient_Referrals_Outgoing_Detail", null, "pnlClinicalProgressNote #pnlPatientReferrals");
                });
            }
            else {
                UnloadActionPan(Patient_Referrals_Outgoing_Detail.params["ParentCtrl"], "Patient_Referrals_Outgoing_Detail", null, "pnlClinicalFaceSheet #pnlPatientReferrals");
            }


        }
        else if (Patient_Referrals_Outgoing_Detail.params.ParentCtrl == 'mstrTabDashBoard') {
            utility.UnLoadDialog("frmPatientReferralsOutgoingDetail", function () {
                UnloadActionPan(Patient_Referrals_Outgoing_Detail.params.ParentCtrl, 'Patient_Referrals_Outgoing_Detail', null, "pnlDashboard");
            }, function () {
                UnloadActionPan(Patient_Referrals_Outgoing_Detail.params.ParentCtrl, 'Patient_Referrals_Outgoing_Detail', null, "pnlDashboard");
            });
        } else if (Patient_Referrals_Outgoing_Detail.params.ParentCtrl == 'schcheckout') {
            UnloadActionPan(Patient_Referrals_Outgoing_Detail.params.ParentCtrl, 'Patient_Referrals_Outgoing_Detail', null, "schcheckout");
        }
        else if (Patient_Referrals_Outgoing_Detail.params.ParentCtrl == 'Clinical_Treatment') {
            Clinical_Treatment.LoadReferrals(null, 1, 15);
            UnloadActionPan(Patient_Referrals_Outgoing_Detail.params.ParentCtrl, 'Patient_Referrals_Outgoing_Detail', null, "pnlClinicalProgressNote #pnlClinicalTreatment");
        }
        else {
            UnloadActionPan(Patient_Referrals_Outgoing_Detail.params.ParentCtrl, 'Patient_Referrals_Outgoing_Detail', null, "pnlClinicalProgressNote #pnlPatientReferrals");
        }
        Patient_Referrals_Outgoing_Detail.SaveorSavePrintButtonClicked = false;
    },
    //Author: M Ahmad Imran
    //Date :  10-05-2016
    //Reason: open provider form
    //OpenRefProvider: function () {
    //    var params = [];
    //    params["IsOptional"] = false;
    //    params["RefForm"] = "frmPatientReferralsOutgoingDetail";
    //    params["ProviderId"] = "-1";
    //    params["FromAdmin"] = "0";
    //    params["RefCtrl"] = "txtRefProvider";
    //    //params["RefCtrlHidden"] = "hfRefProvider";
    //    params["ParentCtrl"] = "Patient_Referrals_Outgoing_Detail";
    //    LoadActionPan('Admin_ReferringProvider', params);
    //},
    //OpenProvider: function () {
    //    var params = [];
    //    params["ReferringProviderId"] = "-1";
    //    params["FromAdmin"] = "0";
    //    params["RefForm"] = "frmPatientReferralsOutgoingDetail";
    //    params["IsOptional"] = false;
    //    params["RefCtrl"] = "txtProvider";
    //    //params["RefCtrlHidden"] = "hfProvider";
    //    params["ParentCtrl"] = "Patient_Referrals_Outgoing_Detail";
    //    LoadActionPan('Admin_Provider', params);
    //},

    //OpenProvider: function () {
    //    var params = [];
    //    params["IsOptional"] = true;
    //    params["RefForm"] = "frmPatientReferralsOutgoingDetail";
    //    params["ProviderId"] = "-1";
    //    params["FromAdmin"] = "0";
    //    params["RefCtrl"] = "txtProvider";
    //    params["ParentCtrl"] = "Patient_Referrals_Outgoing_Detail";
    //    LoadActionPan('Admin_Provider', params);
    //},


    //OpenRefProvider: function () {
    //    var params = [];
    //    params["RefProviderId"] = "-1";
    //    params["FromAdmin"] = "0";
    //    params["RefForm"] = "frmPatientReferralsOutgoingDetail";
    //    params["IsOptional"] = false;
    //    params["RefCtrl"] = "txtRefProvider";
    //    params["ParentCtrl"] = "Patient_Referrals_Outgoing_Detail";
    //    LoadActionPan('Admin_ReferringProvider', params);
    //},

    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmPatientReferralsOutgoingDetail";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["RefCtrlHidden"] = "hfProvider";
        params["ProviderNPI"] = "hfProviderNPI";
        params["ParentCtrl"] = "Patient_Referrals_Outgoing_Detail";
        LoadActionPan('Admin_Provider', params);
    },
    OpenRefProvider: function () {
        var params = [];
        params["ReferringProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefForm"] = "frmPatientReferralsOutgoingDetail";
        params["IsOptional"] = true;
        params["RefCtrl"] = "txtRefProvider";
        params["RefProviderNPI"] = "hfRefProviderNPI";
        params["RefCtrlHidden"] = "hfRefProvider";
        params["ParentCtrl"] = "Patient_Referrals_Outgoing_Detail";
        LoadActionPan('Admin_ReferringProvider', params);
    },

    //Function Name: OpenAssignee
    //Author: M Ahmad Imran
    //Date :  10-05-2016
    //Description: auto complete for assignee
    OpenAssignee: function () {
        CacheManager.BindCodes('GetUsersFullName', true).done(function (result) {
            var Ctrl = $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #txtAssignee");
            var hfCtrl = $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail  #hfAssignee");
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
        //                $("#pnlPatientReferralsOutgoingDetail #txtRefProvider").attr("RefProviderId", ui.item.id); // add the selected id
        //                $("#pnlPatientReferralsOutgoingDetail #hfRefProvider").val(ui.item.id); // add the selected id
        //                if ($("#pnlPatientReferralsOutgoingDetail #lnkProviderEdit").css("display") == "none") {
        //                    $("#pnlPatientReferralsOutgoingDetail #lnkProviderEdit").css("display", "inline");
        //                    $("#pnlPatientReferralsOutgoingDetail #lblRefProvider").css("display", "none");
        //                }
        //            }, 100);
        //        }
        //    });
        //});

        CacheManager.BindCodes('GetAllProviders', true).done(function (result) {
            $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail input#txtProvider").autocomplete({
                autoFocus: true,
                source: AllProviders, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#pnlPatientReferralsOutgoingDetail #txtProvider").attr("ProviderId", ui.item.id); // add the selected id
                        $("#pnlPatientReferralsOutgoingDetail #hfProvider").val(ui.item.id); // add the selected id
                        if ($("#pnlPatientReferralsOutgoingDetail #lnkProviderEdit").css("display") == "none") {
                            $("#pnlPatientReferralsOutgoingDetail #lnkProviderEdit").css("display", "inline");
                            $("#pnlPatientReferralsOutgoingDetail #lblProvider").css("display", "none");
                        }
                    }, 100);
                }
            });
        });


        //CacheManager.BindCodes('GetRefProviders', false).done(function (result) {
        //    $("input#txtRefProvider").autocomplete({
        //        autoFocus: true,
        //        minLength: 3,  //added min length, Abdur Rehman Latif - PMS 2986
        //        source: RefProviders, // pass an array
        //        select: function (event, ui) {
        //            setTimeout(function () {
        //                $("#pnlPatientReferralsOutgoingDetail #hfRefProvider").val(ui.item.id);
        //                if ($("#pnlPatientReferralsOutgoingDetail #lnkRefProviderEdit").css("display") == "none") {
        //                    $("#pnlPatientReferralsOutgoingDetail #lnkRefProviderEdit").css("display", "inline");
        //                    $("#pnlPatientReferralsOutgoingDetail #lblRefProvider").css("display", "none");
        //                }
        //            }, 100);

        //        }
        //    });
        //});



        CacheManager.BindCodes('GetRefProvidersOutgoingReferral', true).done(function (result) {
            $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail input#txtRefProvider").autocomplete({
                autoFocus: true,
                minLength: 3,  //added min length, Abdur Rehman Latif - PMS 2986
                source: RefProvidersOutgoing, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #hfRefProvider").val(ui.item.id);
                        if ($("#pnlPatientReferralsOutgoingDetail #lnkRefProviderEdit").css("display") == "none") {
                            $("#pnlPatientReferralsOutgoingDetail #lnkRefProviderEdit").css("display", "inline");
                            $("#pnlPatientReferralsOutgoingDetail #lblRefProvider").css("display", "none");
                        }
                    }, 100);

                }
            });
        });




        //Begin Edit by Fahad Malik 19-Dec-2016, Bug# EMR-2277
        CacheManager.BindCodes('GetFacilityOutgoingReferral', true).done(function (result) {

            $("input#txtFacilityFrom").autocomplete({
                autoFocus: true,
                source: FacilitiesOutgoingReferral, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#pnlPatientReferralsOutgoingDetail #hfFacilityFrom").val(ui.item.id); // add the selected id

                    }, 100);
                }
            });

            $("input#txtFacilityTo").autocomplete({
                autoFocus: true,
                source: FacilitiesOutgoingReferral, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#pnlPatientReferralsOutgoingDetail #hfFacilityTo").val(ui.item.id); // add the selected id

                    }, 100);
                }
            });


        });
        //CacheManager.BindCodes('GetFacilityOutgoingReferral', true).done(function (result) {
        //    $("input#txtFacilityTo").autocomplete({
        //        autoFocus: true,
        //        source: FacilitiesOutgoingReferral, // pass an array
        //        select: function (event, ui) {
        //            setTimeout(function () {
        //                $("#pnlPatientReferralsOutgoingDetail #hfFacilityTo").val(ui.item.id); // add the selected id

        //            }, 100);
        //        }
        //    });
        //});

        //End Edit by Fahad Malik 19-Dec-2016, Bug# EMR-2277


        CacheManager.BindCodes('GetUsersFullName', true).done(function (result) {
            $("#pnlPatientReferralsOutgoingDetail #txtAssignee").autocomplete({
                autoFocus: true,
                source: GetUsersFullName, // pass an array
                select: function (event, ui) {

                    setTimeout(function () {
                        $("#pnlPatientReferralsOutgoingDetail #txtAssignee").attr("ProviderId", ui.item.id); // add the selected id
                        $("#pnlPatientReferralsOutgoingDetail #hfAssignee").val(ui.item.id);
                    }, 100);
                }
            });


        });


        CacheManager.BindCodes('GetSpecialtyDescription', true).done(function (result) {

            $("input#txtSpecialtyFrom").autocomplete({
                autoFocus: true,
                source: SpecialitiesDescription, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#pnlPatientReferralsOutgoingDetail #hfSpecialtyFrom").val(ui.item.id); // add the selected id

                    }, 100);
                }
            });
        });

    },

    //Function Name: IncomingReferralSave
    //Author Name: M Ahmad Imran
    //Created Date: 12-05-2016
    //Description: Saves CIncoming Referral
    OutGoingReferralSave: function (issaveandprint) {


        if (EMRUtility.compareFormDataWithSerialized(Patient_Referrals_Outgoing_Detail.params.PanelID + ' #frmPatientReferralsOutgoingDetail') || Patient_Referrals_Outgoing_Detail.ReferralProcedureChange || Patient_Referrals_Outgoing_Detail.ReferralProblemsChange) {
            var ReferralId = $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #hfReferralID").val() != "" ? $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #hfReferralID").val() : "-1";
            if (parseInt(ReferralId) > 0) {
                Patient_Referrals_Outgoing_Detail.params.mode = "Edit";
            }
            else {
                Patient_Referrals_Outgoing_Detail.params.mode = "Add";
            }

            if ($('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #txtRefProvider").val() != '' || $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #txtSpecialtyFrom").val() != '') {
                var self = $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail");

                var myJSON = self != null ? self.getMyJSONByName() : "{}";
                var objData = JSON.parse(myJSON);

                if (Patient_Referrals_Outgoing_Detail.params.ParentCtrl == 'mstrTabDashBoard') {
                    objData["PatientId"] = Patient_Referrals_Outgoing_Detail.params.PatientId;
                }
                else {
                    objData["PatientId"] = $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #hfPatientId").val();//  $('#PatientProfile #hfPatientId').val();
                }

                var ReferralProblemList = [];
                $(self).find("#ulProblemLists td").each(function (index, item) {
                    if ($(this).find("input:checkbox").prop("checked")) {
                        var objProblem = {
                            ProblemId: $(this).find("input:checkbox").val(),
                            Description: $(this).text().trim(),
                            ICD10: $(this).find("input:checkbox").attr("icd10"),
                            ProblemName: $(this).find("input:checkbox").attr("ProblemName")
                        }
                        ReferralProblemList.push(objProblem);
                    }
                });

                objData["ReferralProblemList"] = ReferralProblemList;
                objData["Type"] = 'Outgoing';

                //"{"ConsultationProcedure-1":"86885%20Weak%20D%20antigen%20detection%20on%20red%20blood%20cells","ProcedureId-1":"86885%20Weak%20D%20antigen%20detection%20on%20red%20blood%20cells","Urgency-1":"1","Urgency-1_text":"Normal"}"



                ////Start 22-03-2016 Humaira Yousaf for status
                //if (sender == 'signorder' || sender == 'signprintorder')
                //    objData["Status"] = 'Signed';
                //else if (sender == 'save')
                //    objData["Status"] = 'Draft';
                ////End 22-03-2016 Humaira Yousaf for status

                myJSON = JSON.stringify(objData);

                //------------------------------------------------------------
                var ProcedureIds = $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral tbody tr").map(function () {
                    return this.id.replace("id", "");
                }).get().join(',');
                $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #pnlReferralProcedure_Result #hfReferralProcedureIds").val(ProcedureIds);
                $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + ' #frmPatientReferralsOutgoingDetail').data('serialize', $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + ' #frmPatientReferralsOutgoingDetail').serialize());
                var ReferralProcedure = [];
                $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral tbody tr").each(function (index) {
                    ProcedureId = $(this).attr("id");
                    var ProcedureRow = {};
                    var ParsedProcedureJson = JSON.parse($(this).getMyJSONByName());
                    ProcedureRow["ReferralProcedureId"] = ProcedureId;
                    ProcedureRow["Urgency"] = ParsedProcedureJson.Urgency;
                    ProcedureRow["Urgency_text"] = ParsedProcedureJson.Urgency_text;
                    ProcedureRow["Procedure"] = ParsedProcedureJson.Procedure;
                    ProcedureRow["CPTCode"] = $($("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral tbody tr")[index]).attr('cptcode');
                    ProcedureRow["CPTCodeDescription"] = $($("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral tbody tr")[index]).attr('cptdescription');
                    ReferralProcedure.push(ProcedureRow);
                });
                var objRad = new Object();
                objRad["ProcedureIds"] = ProcedureIds;
                objRad["Type"] = "Outgoing";
                objRad["ReferralProcedure"] = ReferralProcedure

                //JSONToSave = utility.MergeJSON(myJSON, myJSONReferralProcedure);
                var data = JSON.stringify(objRad);

                JSONToSave = utility.MergeJSON(data, myJSON);
                //  JSONToSave = decodeURIComponent(JSONToSave);
                JSONToSave = JSONToSave;

                //--------------------------------------------------------------

                if (Patient_Referrals_Outgoing_Detail.params.mode == "Add") {
                    var strMessage = "";
                    AppPrivileges.GetFormPrivileges("Referrals", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            Patient_Referrals_Outgoing_Detail.SaveReferral(JSONToSave).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {

                                    var IsMedText = false;
                                    if (globalAppdata.IsMedText == "True" && response.MedTextUrl) {
                                        IsMedText = true;
                                        Patient_Referrals_Outgoing_Detail.OpenMedText(response.MedTextUrl);
                                    }

                                    Patient_Referrals_Outgoing_Detail.params.ReferralId = response.ReferralId;
                                    utility.DisplayMessages(response.message, 1);
                                    $("#faxButton").removeClass('disabled');

                                    if (Patient_Referrals.params.ParentCtrl == "clinicalTabProgressNote" && Patient_Referrals_Outgoing_Detail.params.ParentCtrl != 'Clinical_Treatment') {
                                        if (response.ReferralId != null && response.ReferralId != '') {

                                            Patient_Referrals.getReferralInfo(response.ReferralId);

                                        }
                                    }
                                    if (Patient_Referrals_Outgoing_Detail.params.ParentCtrl == 'schcheckout') {
                                        $('#schcheckout #btnAddReferral').text('View Referral');
                                        $('#schcheckout #hfReferralId').val(response.ReferralId);
                                        schcheckout.SavePatientReferral();
                                        if (issaveandprint == undefined && issaveandprint != '1') {
                                            Patient_Referrals_Outgoing_Detail.SaveorSavePrintButtonClicked = true;
                                            if (!IsMedText)
                                                Patient_Referrals_Outgoing_Detail.UnLoadTab();
                                        }
                                        else {

                                            var PanelReferralGrid = "#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #pnlConsultation_Result";
                                            var ReferralGridId = "#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral";
                                            //$(ChargeGridId).dataTable().fnDestroy();
                                            // $(ReferralGridId + " tbody tr").remove();
                                            Patient_Referrals_Outgoing_Detail.EditableGrid = EMRUtility.MakeEditableGrid(PanelReferralGrid, ReferralGridId, Patient_Referrals_Outgoing_Detail, "0", false, false, false, false);
                                            $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #hfReferralID").val(response.ReferralId);
                                            Patient_Referrals_Outgoing_Detail.SaveorSavePrintButtonClicked = false;
                                            Patient_Referrals_Outgoing_Detail.printReferral();

                                        }

                                    }
                                    else {
                                        Patient_Referrals.Type = "Outgoing";
                                        Patient_Referrals.ReferralSearch().done(function () {

                                            Patient_Referrals.fillRefferal();
                                            if (issaveandprint == undefined && issaveandprint != '1') {
                                                Patient_Referrals_Outgoing_Detail.SaveorSavePrintButtonClicked = true;
                                                if (!IsMedText)
                                                    Patient_Referrals_Outgoing_Detail.UnLoadTab();
                                            }
                                            else {

                                                var PanelReferralGrid = "#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #pnlConsultation_Result";
                                                var ReferralGridId = "#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral";
                                                //$(ChargeGridId).dataTable().fnDestroy();
                                                // $(ReferralGridId + " tbody tr").remove();
                                                Patient_Referrals_Outgoing_Detail.EditableGrid = EMRUtility.MakeEditableGrid(PanelReferralGrid, ReferralGridId, Patient_Referrals_Outgoing_Detail, "0", false, false, false, false);
                                                $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #hfReferralID").val(response.ReferralId);
                                                Patient_Referrals_Outgoing_Detail.loadReferralData();
                                                Patient_Referrals_Outgoing_Detail.SaveorSavePrintButtonClicked = false;
                                                Patient_Referrals_Outgoing_Detail.printReferral();
                                            }


                                        });
                                    }
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
                else if (Patient_Referrals_Outgoing_Detail.params.mode == "Edit") {
                    var strMessage = "";
                    AppPrivileges.GetFormPrivileges("Referrals", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            Patient_Referrals_Outgoing_Detail.updateReferral(JSONToSave, ReferralId).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    Patient_Referrals_Outgoing_Detail.params.ReferralId = ReferralId;
                                    utility.DisplayMessages(response.message, 1);

                                    var IsMedText = false;
                                    if (globalAppdata.IsMedText == "True" && response.MedTextUrl) {
                                        IsMedText = true;
                                        Patient_Referrals_Outgoing_Detail.OpenMedText(response.MedTextUrl);
                                    }

                                    if (Patient_Referrals.params.ParentCtrl == "clinicalTabProgressNote" && Patient_Referrals_Outgoing_Detail.params.ParentCtrl != 'Clinical_Treatment') {
                                        if (response.ReferralId != null && response.ReferralId != '') {

                                            Patient_Referrals.getReferralInfo(response.ReferralId);

                                        }
                                    }
                                    if (Patient_Referrals_Outgoing_Detail.params.ParentCtrl == 'mstrTabDashBoard') {
                                        if (issaveandprint == undefined && issaveandprint != '1') {
                                            Patient_Referrals_Outgoing_Detail.SaveorSavePrintButtonClicked = true;
                                            if (!IsMedText)
                                                Patient_Referrals_Outgoing_Detail.UnLoadTab();
                                        }
                                        else {
                                            if (ReferralProcedure[0].Procedure != undefined) {
                                                var PanelReferralGrid = "#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #pnlConsultation_Result";
                                                var ReferralGridId = "#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral";
                                                //$(ChargeGridId).dataTable().fnDestroy();
                                                // $(ReferralGridId + " tbody tr").remove();
                                                Patient_Referrals_Outgoing_Detail.EditableGrid = EMRUtility.MakeEditableGrid(PanelReferralGrid, ReferralGridId, Patient_Referrals_Outgoing_Detail, "0", false, false, false, false);
                                            }
                                            else {
                                                var PanelReferralGrid = "#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #pnlConsultation_Result";
                                                var ReferralGridId = "#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral";
                                                $(ReferralGridId + " tbody tr").remove();
                                                //if ($.fn.dataTable.isDataTable(ReferralGridId)) {
                                                //    $(ReferralGridId).dataTable().fnClearTable();
                                                //    $(ReferralGridId).dataTable().fnDestroy();
                                                //}
                                                //  Patient_Referrals_Outgoing_Detail.EditableGrid.datatable.clear().draw();
                                                Patient_Referrals_Outgoing_Detail.EditableGrid = EMRUtility.MakeEditableGrid(PanelReferralGrid, ReferralGridId, Patient_Referrals_Outgoing_Detail, "0", false, false, false, false);

                                            }
                                            Patient_Referrals_Outgoing_Detail.SaveorSavePrintButtonClicked = false;
                                            Patient_Referrals_Outgoing_Detail.printReferral();

                                        }
                                        DashBoard.DashBoardOutgoingReferralGridLoad(null, null, null);
                                    }
                                    else if (Patient_Referrals_Outgoing_Detail.params.ParentCtrl == 'schcheckout') {
                                        $('#schcheckout #btnAddReferral').text('View Referral');
                                        $('#schcheckout #hfReferralId').val(response.ReferralId);
                                        if (issaveandprint == undefined && issaveandprint != '1') {
                                            Patient_Referrals_Outgoing_Detail.SaveorSavePrintButtonClicked = true;
                                            if (!IsMedText)
                                                Patient_Referrals_Outgoing_Detail.UnLoadTab();

                                        }
                                        else {
                                            if (ReferralProcedure[0].Procedure != undefined) {
                                                var PanelReferralGrid = "#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #pnlConsultation_Result";
                                                var ReferralGridId = "#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral";
                                                //$(ChargeGridId).dataTable().fnDestroy();
                                                // $(ReferralGridId + " tbody tr").remove();
                                                Patient_Referrals_Outgoing_Detail.EditableGrid = EMRUtility.MakeEditableGrid(PanelReferralGrid, ReferralGridId, Patient_Referrals_Outgoing_Detail, "0", false, false, false, false);
                                            }
                                            else {
                                                var PanelReferralGrid = "#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #pnlConsultation_Result";
                                                var ReferralGridId = "#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral";
                                                $(ReferralGridId + " tbody tr").remove();
                                                //if ($.fn.dataTable.isDataTable(ReferralGridId)) {
                                                //    $(ReferralGridId).dataTable().fnClearTable();
                                                //    $(ReferralGridId).dataTable().fnDestroy();
                                                //}
                                                //  Patient_Referrals_Outgoing_Detail.EditableGrid.datatable.clear().draw();
                                                Patient_Referrals_Outgoing_Detail.EditableGrid = EMRUtility.MakeEditableGrid(PanelReferralGrid, ReferralGridId, Patient_Referrals_Outgoing_Detail, "0", false, false, false, false);

                                            }
                                            Patient_Referrals_Outgoing_Detail.SaveorSavePrintButtonClicked = false;
                                            Patient_Referrals_Outgoing_Detail.printReferral();

                                        }

                                    }
                                    else {
                                        Patient_Referrals.Type = "Outgoing";
                                        Patient_Referrals.ReferralSearch().done(function () {
                                            Patient_Referrals.fillRefferal();
                                            if (issaveandprint == undefined && issaveandprint != '1') {
                                                Patient_Referrals_Outgoing_Detail.SaveorSavePrintButtonClicked = true;
                                                if (!IsMedText)
                                                    Patient_Referrals_Outgoing_Detail.UnLoadTab();

                                            }
                                            else {

                                                if (ReferralProcedure[0].Procedure != undefined) {
                                                    var PanelReferralGrid = "#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #pnlConsultation_Result";
                                                    var ReferralGridId = "#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral";
                                                    //$(ChargeGridId).dataTable().fnDestroy();
                                                    // $(ReferralGridId + " tbody tr").remove();

                                                    Patient_Referrals_Outgoing_Detail.EditableGrid = EMRUtility.MakeEditableGrid(PanelReferralGrid, ReferralGridId, Patient_Referrals_Outgoing_Detail, "0", false, false, false, false);
                                                }
                                                else {
                                                    var PanelReferralGrid = "#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #pnlConsultation_Result";
                                                    var ReferralGridId = "#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral";
                                                    $(ReferralGridId + " tbody tr").remove();
                                                    //if ($.fn.dataTable.isDataTable(ReferralGridId)) {
                                                    //    $(ReferralGridId).dataTable().fnClearTable();
                                                    //    $(ReferralGridId).dataTable().fnDestroy();
                                                    //}
                                                    //  Patient_Referrals_Outgoing_Detail.EditableGrid.datatable.clear().draw();
                                                    Patient_Referrals_Outgoing_Detail.EditableGrid = EMRUtility.MakeEditableGrid(PanelReferralGrid, ReferralGridId, Patient_Referrals_Outgoing_Detail, "0", false, false, false, false);

                                                }
                                                Patient_Referrals_Outgoing_Detail.loadReferralData();
                                                Patient_Referrals_Outgoing_Detail.SaveorSavePrintButtonClicked = false;
                                                Patient_Referrals_Outgoing_Detail.printReferral();

                                            }



                                        });
                                    }
                                    //Patient_Referrals_Outgoing_Detail.ReferralProcedureChange = false;
                                    //Patient_Referrals_Outgoing_Detail.ReferralProblemsChange = false;
                                    //$('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail").data('serialize', $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail").serialize());


                                }
                                else {
                                    utility.DisplayMessages(response.message, 3);
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
                $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + ' #frmPatientReferralsOutgoingDetail').data('serialize', $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + ' #frmPatientReferralsOutgoingDetail').serialize());
            }, 100);
        }
    },

    OpenMedText: function (Url) {

        var params = [];
        params["FromAdmin"] = "0";
        params["MedTextUrl"] = Url;
        params["ParentCtrl"] = "Patient_Referrals_Outgoing_Detail";
        LoadActionPan('Patient_MedText_Referrals', params);
    },

    OutGoingReferralSaveOnCloseButton: function () {


        if (EMRUtility.compareFormDataWithSerialized(Patient_Referrals_Outgoing_Detail.params.PanelID + ' #frmPatientReferralsOutgoingDetail') || Patient_Referrals_Outgoing_Detail.ReferralProcedureChange || Patient_Referrals_Outgoing_Detail.ReferralProblemsChange) {
            var ReferralId = $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #hfReferralID").val() != "" ? $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #hfReferralID").val() : "-1";
            if (parseInt(ReferralId) > 0) {
                Patient_Referrals_Outgoing_Detail.params.mode = "Edit";
            }
            else {
                Patient_Referrals_Outgoing_Detail.params.mode = "Add";
            }

            if ($('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #txtRefProvider").val() != '' || $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #txtSpecialtyFrom").val() != '') {
                var self = $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail");

                var myJSON = self != null ? self.getMyJSONByName() : "{}";
                var objData = JSON.parse(myJSON);

                if (Patient_Referrals_Outgoing_Detail.params.ParentCtrl == 'mstrTabDashBoard') {
                    objData["PatientId"] = Patient_Referrals_Outgoing_Detail.params.PatientId;
                }
                else {
                    objData["PatientId"] = $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #hfPatientId").val();//  $('#PatientProfile #hfPatientId').val();
                }

                var ReferralProblemList = [];
                $(self).find("#ulProblemLists td").each(function (index, item) {
                    if ($(this).find("input:checkbox").prop("checked")) {
                        var objProblem = {
                            ProblemId: $(this).find("input:checkbox").val(),
                            Description: $(this).text().trim(),
                            ICD10: $(this).find("input:checkbox").attr("icd10"),
                            ProblemName: $(this).find("input:checkbox").attr("ProblemName")
                        }
                        ReferralProblemList.push(objProblem);
                    }
                });

                objData["ReferralProblemList"] = ReferralProblemList;
                objData["Type"] = 'Outgoing';

                //"{"ConsultationProcedure-1":"86885%20Weak%20D%20antigen%20detection%20on%20red%20blood%20cells","ProcedureId-1":"86885%20Weak%20D%20antigen%20detection%20on%20red%20blood%20cells","Urgency-1":"1","Urgency-1_text":"Normal"}"



                ////Start 22-03-2016 Humaira Yousaf for status
                //if (sender == 'signorder' || sender == 'signprintorder')
                //    objData["Status"] = 'Signed';
                //else if (sender == 'save')
                //    objData["Status"] = 'Draft';
                ////End 22-03-2016 Humaira Yousaf for status

                myJSON = JSON.stringify(objData);

                //------------------------------------------------------------
                var ProcedureIds = $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral tbody tr").map(function () {
                    return this.id.replace("id", "");
                }).get().join(',');
                $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #pnlReferralProcedure_Result #hfReferralProcedureIds").val(ProcedureIds);
                $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + ' #frmPatientReferralsOutgoingDetail').data('serialize', $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + ' #frmPatientReferralsOutgoingDetail').serialize());
                var ReferralProcedure = [];
                $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral tbody tr").each(function (index) {
                    ProcedureId = $(this).attr("id");
                    var ProcedureRow = {};
                    var ParsedProcedureJson = JSON.parse($(this).getMyJSONByName());
                    ProcedureRow["ReferralProcedureId"] = ProcedureId;
                    ProcedureRow["Urgency"] = ParsedProcedureJson.Urgency;
                    ProcedureRow["Urgency_text"] = ParsedProcedureJson.Urgency_text;
                    ProcedureRow["Procedure"] = ParsedProcedureJson.Procedure;
                    ProcedureRow["CPTCode"] = $($("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral tbody tr")[index]).attr('cptcode');
                    ProcedureRow["CPTCodeDescription"] = $($("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral tbody tr")[index]).attr('cptdescription');
                    ReferralProcedure.push(ProcedureRow);
                });
                var objRad = new Object();
                objRad["ProcedureIds"] = ProcedureIds;
                objRad["Type"] = "Outgoing";
                objRad["ReferralProcedure"] = ReferralProcedure

                //JSONToSave = utility.MergeJSON(myJSON, myJSONReferralProcedure);
                var data = JSON.stringify(objRad);

                JSONToSave = utility.MergeJSON(data, myJSON);
                //  JSONToSave = decodeURIComponent(JSONToSave);
                JSONToSave = JSONToSave;

                //--------------------------------------------------------------

                if (Patient_Referrals_Outgoing_Detail.params.mode == "Add") {
                    var strMessage = "";
                    AppPrivileges.GetFormPrivileges("Referrals", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            Patient_Referrals_Outgoing_Detail.SaveReferral(JSONToSave).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    // utility.DisplayMessages(response.message, 1);
                                    if (Patient_Referrals.params.ParentCtrl == "clinicalTabProgressNote" && Patient_Referrals_Outgoing_Detail.params.ParentCtrl != 'Clinical_Treatment') {
                                        if (response.ReferralId != null && response.ReferralId != '') {

                                            Patient_Referrals.getReferralInfo(response.ReferralId);

                                        }
                                    }
                                    if (Patient_Referrals_Outgoing_Detail.params.ParentCtrl == 'schcheckout') {
                                        $('#schcheckout #btnAddReferral').text('View Referral');
                                        $('#schcheckout #hfReferralId').val(response.ReferralId);
                                        schcheckout.SavePatientReferral();

                                        //   Patient_Referrals_Outgoing_Detail.UnLoadTab();


                                    }
                                    else {
                                        Patient_Referrals.Type = "Outgoing";
                                        Patient_Referrals.ReferralSearch().done(function () {

                                            Patient_Referrals.fillRefferal();

                                            //  Patient_Referrals_Outgoing_Detail.UnLoadTab();



                                        });
                                    }
                                }
                                else {
                                    // utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                        else {
                            //  utility.DisplayMessages(strMessage, 2);
                        }
                    });
                }
                else if (Patient_Referrals_Outgoing_Detail.params.mode == "Edit") {
                    var strMessage = "";
                    AppPrivileges.GetFormPrivileges("Referrals", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            Patient_Referrals_Outgoing_Detail.updateReferral(JSONToSave, ReferralId).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {

                                    //   utility.DisplayMessages(response.message, 1);
                                    if (Patient_Referrals.params.ParentCtrl == "clinicalTabProgressNote" && Patient_Referrals_Outgoing_Detail.params.ParentCtrl != 'Clinical_Treatment') {
                                        if (response.ReferralId != null && response.ReferralId != '') {

                                            Patient_Referrals.getReferralInfo(response.ReferralId);

                                        }
                                    }
                                    if (Patient_Referrals_Outgoing_Detail.params.ParentCtrl == 'mstrTabDashBoard') {

                                        //  Patient_Referrals_Outgoing_Detail.UnLoadTab();

                                        DashBoard.DashBoardOutgoingReferralGridLoad(null, null, null);
                                    }
                                    else if (Patient_Referrals_Outgoing_Detail.params.ParentCtrl == 'schcheckout') {
                                        $('#schcheckout #btnAddReferral').text('View Referral');
                                        $('#schcheckout #hfReferralId').val(response.ReferralId);

                                        //  Patient_Referrals_Outgoing_Detail.UnLoadTab();

                                    }
                                    else {
                                        Patient_Referrals.Type = "Outgoing";
                                        Patient_Referrals.ReferralSearch().done(function () {
                                            Patient_Referrals.fillRefferal();

                                            //   Patient_Referrals_Outgoing_Detail.UnLoadTab();




                                        });
                                    }


                                }
                                else {
                                    //  utility.DisplayMessages(response.message, 3);
                                }
                            });
                        }
                        else {
                            // utility.DisplayMessages(strMessage, 2);
                        }
                    });

                }


            }

        }
    },
    //Function Name: ConsultationOrderSave
    //Author Name: Humaira Yousaf
    //Created Date: 02-03-2016
    //Description: Saves ConsultationOrder
    //Params: ConsultationOrderData
    SaveReferral: function (ReferralData) {
        var objData = JSON.parse(ReferralData);
        objData["commandType"] = "save_Referral";
        if (Patient_Referrals_Outgoing_Detail.params.ParentCtrlPanelID == "pnlClinicalProgressNote #pnlPatientReferrals") {
            objData["NoteId"] = $("#pnlClinicalProgressNote #hfNoteId").val();
        } else {
            objData["NoteId"] = "";
        }
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");
    },


    updateReferral: function (ReferralData, ReferralId) {

        var objData = JSON.parse(ReferralData);
        objData["IsActive"] = 1;
        objData["ReferralId"] = ReferralId;
        objData["commandType"] = "save_Referral";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");

    },
    //------------------ start M Ahmad Imran Code for editable grid

    bindAutoComplete: function (element) {
        var hiddenCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "Patient_Referrals_Outgoing_Detail", null, true);
    },
    openCPTCode: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Patient_Referrals_Outgoing_Detail";
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtCPTCode";
        params["ParentCtrlPanelID"] = Patient_Referrals_Outgoing_Detail.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, Patient_Referrals_Outgoing_Detail.params.PanelID);
    },
    BindProcedureGridItem: function (cptCode, procedureDescription, cptDescription, SnomedID, SnomedDescription) {

        var cptCode = cptCode;
        var procDesc = procedureDescription;
        var cptDesc = cptDescription;

        var currentRow = $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral tbody tr");
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

            Patient_Referrals_Outgoing_Detail.AddNewProcedureRow(null, null, null, cptCode, procDesc, cptDescription, SnomedID, SnomedDescription, null, true);
            setTimeout(function () {
                $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #txtReferralCPTCode").val('');
            }, 200);
        }
        else {
            utility.DisplayMessages("This code already exists in the referral.", 2);
        }
    },
    AddNewProcedureRow: function (RowId, mode, CurrRef, cptCode, procDesc, cptDescription, SnomedId, SnomedDescription, UrgencyId, fromInput) {

        var ConsultationGridId = "#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral";

        var CurrentRow = null;
        if (RowId && RowId > 0) {
            if (Patient_Referrals_Outgoing_Detail.params.ParentCtrl != null) {
                CurrentRow = Patient_Referrals_Outgoing_Detail.EditableGrid.rowAdd(RowId, "");
            }
            else {
                CurrentRow = Patient_Referrals_Outgoing_Detail.EditableGrid.rowAdd(RowId, Patient_Referrals_Outgoing_Detail.params.ConsultationId);
            }
        }
        else {
            if (Patient_Referrals_Outgoing_Detail.params.ParentCtrlPanelID != undefined)
                var TemplateRow = $("#" + Patient_Referrals_Outgoing_Detail.params.ParentCtrlPanelID + " #dgvProcedureReferral tbody tr[id*=-]").last();
            else
                var TemplateRow = $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral tbody tr[id*=-]").last();
            var TemplateRowId = 0;
            if (TemplateRow.length > 0) {
                if (TemplateRow.attr("id").indexOf("Child") != "-1")
                    TemplateRowId = parseInt(TemplateRow.prev().attr("id"));
                else
                    TemplateRowId = parseInt(TemplateRow.attr("id"));
            }
            CurrentRow = Patient_Referrals_Outgoing_Detail.EditableGrid.rowAdd(TemplateRowId - 1, "");
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
                $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail").data('serialize', $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail").serialize());
            });
        }
        else
            $(CurrentRow).attr("UrgencyId", UrgencyId);
        Patient_Referrals_Outgoing_Detail.enableRemoveRow($(CurrentRow));
        $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail").data('serialize', $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail").serialize());
        return CurrentRow;
    },
    enableRemoveRow: function (CurrentRow) {
        CurrentRow.find("td.actions .remove-row").removeClass("hidden");
        //    .each(function () {
        //    $(this).removeclass('hidden')
        //});
    },
    rowRemove: function ($row, obj) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Referrals", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    if ($row.hasClass('adding')) {
                    }
                    //var _self = obj;
                    //_self.datatable.row($row.get(0)).remove().draw();
                    if (parseInt($row.attr("id")) > 0) {
                        Patient_Referrals_Outgoing_Detail.DeleteReferralProcedure($row.attr("id"), $row, obj);
                        Patient_Referrals_Outgoing_Detail.loadReferralData();
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
    DeleteReferralProcedure: function (ReferralProcedureId, $row, obj) {

        Patient_Referrals_Outgoing_Detail.DeleteReferralProcedure_DBCall(ReferralProcedureId).done(function (response) {

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

    DeleteReferralProcedure_DBCall: function (ReferralProcedureId) {

        var objData = new Object();
        objData["ReferralProcedureId"] = ReferralProcedureId;
        objData["commandType"] = "DELETE_REFERRAL_PROCEDURE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");
    },
    ProcedureGridLoad: function (response) {
        var dfd = $.Deferred();
        var PanelReferralGrid = "#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #pnlReferralProcedure_Result";
        var ReferralGridId = "#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #dgvProcedureReferral";
        $(ReferralGridId + " tbody tr").remove();
        var arraTemp = [];
        if ($.fn.dataTable.isDataTable(ReferralGridId)) {
            $(ReferralGridId).dataTable().fnClearTable();
            $(ReferralGridId).dataTable().fnDestroy();
        }
        if (response.ProcedureListCount > 0) {
            Patient_Referrals_Outgoing_Detail.EditableGrid.datatable.clear().draw();
            var ReferralProcedureLoadJSONData = JSON.parse(response.ReferralProcedureListLoad_JSON);
            $.each(ReferralProcedureLoadJSONData, function (i, item) {
                var _dfd = $.Deferred();
                var CurrentRow = Patient_Referrals_Outgoing_Detail.AddNewProcedureRow(item.ReferralProcedureId, null, null, item.CPTCode, null, item.CPTCodeDescription, null, null, item.Urgency, false);
                var self = $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " tr#" + item.ReferralProcedureId);
                //$(self).loadDropDowns(true).done(function () {
                utility.bindMyJSONByName(true, item, false, $(CurrentRow)).done(function () {
                    $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail").data('serialize', $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail").serialize());
                    _dfd.resolve();
                });
                //});
                arraTemp.push(_dfd);
            });
            $.when.apply($, arraTemp).then(function () {
                $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail").data('serialize', $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail").serialize());
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
    //Start Problem Work///
    //Author: Farooq Ahmad
    //Date: 28-03-2016
    //Reason:to add more problem is Associated Problem list
    addProblem: function () {
        Patient_Referrals_Outgoing_Detail.SaveLocalyCheckedProbems();
        if (Patient_Referrals_Outgoing_Detail.params.mode == "Edit") {
            var params = [];
            params["IsFromNote"] = Patient_Referrals_Outgoing_Detail.params["IsFromNote"];
            params["CurrentNotesProviderId"] = Patient_Referrals_Outgoing_Detail.params["CurrentNotesProviderId"];
            params["RefForm"] = "frmPatientReferralsOutgoingDetail";
            params["FromOrderDetail"] = "1";
            params["FromAdmin"] = "0";
            params["ParentCtrl"] = "Patient_Referrals_Outgoing_Detail";
            LoadActionPan('Clinical_ProblemLists', params);
        }
        else {
            var params = [];
            params["CurrentNotesProviderId"] = Patient_Referrals_Outgoing_Detail.params["CurrentNotesProviderId"];
            params["IsFromNote"] = Patient_Referrals_Outgoing_Detail.params["IsFromNote"];
            params["RefForm"] = "frmPatientReferralsOutgoingDetail";
            params["FromOrderDetail"] = "1";
            params["FromAdmin"] = "0";
            params["ParentCtrl"] = "Patient_Referrals_Outgoing_Detail";
            LoadActionPan('Clinical_ProblemLists', params);
        }

    },
    SaveLocalyCheckedProbems: function () {
        var self = $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail");
        Patient_Referrals_Outgoing_Detail.ReferralProblems = [];
        $(self).find("#ulProblemLists td").each(function (index, item) {
            if ($(this).find("input:checkbox").prop("checked")) {
                var objProblem = {
                    ProblemId: $(this).find("input:checkbox").val(),
                    Description: $(this).text()
                }
                Patient_Referrals_Outgoing_Detail.ReferralProblems.push(objProblem);
            }
        });
    },

    CheckedPreviousProbems: function () {
        var dfd = $.Deferred();
        for (var index in Patient_Referrals_Outgoing_Detail.ReferralProblems) {
            $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #ulProblemLists td #chk" + Patient_Referrals_Outgoing_Detail.ReferralProblems[index].ProblemId).attr("checked", "checked");
        }
        dfd.resolve();
        return dfd.promise();
    },
    //Author: Farooq Ahmad
    //Date :  17-03-2016
    //Reason: Function to load problem list
    loadProblemList: function () {
        var dfd = new $.Deferred();

        Patient_Referrals_Outgoing_Detail.SearchProblemList().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.ProblemListCount > 0) {
                    var ProblemListLoadJSONData = JSON.parse(response.ProblemListLoad_JSON);
                    var ProblemListHistoryLoadJSONData = JSON.parse(response.ProblemListHistoryLoad_JSON);

                    var finalTr = '';
                    var counter = 2;
                    $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #ulProblemLists tbody tr").remove();
                    $.each(ProblemListLoadJSONData, function (i, item) {
                        if (item.Description.split("-")[2] != undefined) {
                            item.Description = item.Description.substring(item.Description.indexOf("-") + 2);
                        }
                        if (counter % 2 == 0) {
                            finalTr = finalTr + '<tr>';
                        }
                        finalTr = finalTr + '<td><div class="p-xs col-xs-12"><div class="checkbox-custom">';

                        if ($("#hfIMOProblem").val()) {
                            if ($("#hfIMOProblem").val() == item.ProblemName)
                                finalTr = finalTr + '<input checked="checked" type="checkbox" name="' + item.Code + '" icd10="' + item.ICD10 + '" ProblemName="' + item.ProblemName + '"  value="' + item.ProblemListId + '" id="chk' + item.ProblemListId + '">';
                            else
                                finalTr = finalTr + '<input type="checkbox" name="' + item.Code + '" icd10="' + item.ICD10 + '" ProblemName="' + item.ProblemName + '"  value="' + item.ProblemListId + '" id="chk' + item.ProblemListId + '">';
                        }
                        else
                            finalTr = finalTr + '<input type="checkbox" name="' + item.Code + '" icd10="' + item.ICD10 + '" ProblemName="' + item.ProblemName + '" value="' + item.ProblemListId + '" id="chk' + item.ProblemListId + '">';

                        finalTr = finalTr + '   <label for="chk' + item.ProblemListId + '" class="control-label">' + item.Description + '</label></div></div></td>';

                        if (counter % 2 == 1) {
                            finalTr = finalTr + '</tr>';
                        }
                        counter++;
                        var color = "";
                    });
                    $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #ulProblemLists tbody").append(finalTr);
                    dfd.resolve();
                }
                else {
                    dfd.resolve();
                }
                $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail").data('serialize', $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail").serialize());
            }
            else {
                //utility.DisplayMessages(response.Message, 3);
                dfd.resolve();
            }
        });
        return dfd.promise();
    },
    //Author: Farooq Ahmad
    //Date :  17-03-2016
    //Reason: Function to load problem list
    SearchProblemList: function () {


        var IsCheckedIn = null;
        IsCheckedIn = $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + ' #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive');
        if (IsCheckedIn == null) {
            IsCheckedIn = "1";
        }

        var PageNumber = 1;
        var RowsPerPage = 2000;

        var objData = new Object();
        objData["PatientId"] = $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #hfPatientId").val(); //$('#PatientProfile #hfPatientId').val();
        objData["IsActive"] = '1';
        // objData["ProblemListId"] = ProblemListId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "SEARCH_PROBLEMLIST";
        //objData["NoteId"] = Clinical_ProblemLists.params.NotesId == null ? 0 : Clinical_ProblemLists.params.NotesId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");

    },

    //Function Name: printReferral
    //Author Name: M Ahmad Imran
    //Created Date: 17-05-2016
    //Description: Creates PDF to view Referral
    printReferral: function () {
        var ReferralId = $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #hfReferralID").val();
        var params = [];
        params["FromAdmin"] = "0";
        params["UserId"] = globalAppdata['AppUserId'];
        params["PatientId"] = $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #hfPatientId").val(); //$('#PatientProfile #hfPatientId').val();
        params["ParentCtrl"] = "Patient_Referrals_Outgoing_Detail";
        params["ReferralId"] = ReferralId;

        Patient_ReferralsView.ReferralPreview(params.PatientId, params.UserId, params.ReferralId);

        //LoadReferralActionPan('Patient_ReferralsView', params);
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
        params["ParentCtrl"] = "Patient_Referrals_Outgoing_Detail";
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
        params["ParentCtrl"] = "Patient_Referrals_Outgoing_Detail";
        LoadActionPan('Admin_Facility', params);
    },


    documentImport: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Documents", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];

                var AccountNo = Patient_Referrals_Outgoing_Detail.params.AccountNumber;
                var PatientFullName = Patient_Referrals_Outgoing_Detail.params.PatientName;
                var PatientId = Patient_Referrals_Outgoing_Detail.params.PatientId;

                params["patientId"] = Patient_Referrals.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
                params["RefCtrl"] = "OutgoingReferral";
                params['ReferralId'] = Patient_Referrals_Outgoing_Detail.params.ReferralId;
                params['RefModuleName'] = "Outgoing Referral";
                params['TransitionId'] = Patient_Referrals_Outgoing_Detail.params.ReferralId;
                params["FromAdmin"] = "0";
                params["mode"] = "Add";
                params["ParentCtrl"] = 'Patient_Referrals_Outgoing_Detail';
                params["PatientName"] = PatientFullName;
                params["AccountNo"] = AccountNo;
                params["PatientId"] = PatientId;
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
                param["RefCtrl"] = "OutgoingReferral";
                param['RadiologyResultId'] = Patient_Referrals_Outgoing_Detail.params.ReferralId;
                param['RefModuleName'] = "Outgoing Referral";
                param['TransitionId'] = Patient_Referrals_Outgoing_Detail.params.ReferralId;
                param['patientID'] = Patient_Referrals.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
                param["ParentCtrl"] = 'Patient_Referrals_Outgoing_Detail';
                LoadActionPan('Document_Scan', param);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    //Function Name: loadAttachments
    //Author: Humaira Yousaf
    //Date :  02-05-2016
    //Description: Loads Attachments // modified by Abid
    loadAttachments: function (controlName, radiologyOrderId, resultId, tableId) {

        Patient_Referrals_Outgoing_Detail.loadAttachments_DbCall(radiologyOrderId).done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {

                var ulAttachment = null;

                if (controlName == null)
                    ulAttachment = $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + ' #menuViewAttachment');
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
                            $(ulAttachment).append('<li><a href="#" id="' + item.PatDocId + '" onclick="Patient_Referrals_Outgoing_Detail.showAttachment(\'' + item.PatDocId + '\',event)">' + item.ModifiedOn + '</a></li>');
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
        objData["TransitionId"] = Patient_Referrals_Outgoing_Detail.params.ReferralId;
        objData["RefModuleName"] = "Outgoing Referral";
        objData["PatientId"] = Patient_Referrals.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $("div#PatientProfile #hfPatientId").val();

        objData["commandType"] = "load_attachments";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");
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
                params["PatientID"] = Patient_Referrals.params["ParentCtrl"] == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
                params["PatDocID"] = PatDocID;
                params["mode"] = "Edit";
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = "Patient_Referrals_Outgoing_Detail";

                LoadActionPan('Document_Viewer', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    referralReset: function () {
        utility.myConfirm('22', function () {

            //selectors
            var form = "#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail";

            var $orderInfomation = $(form + " #divReferralInfo");
            var $testInformation = $(form + " #dgvProcedureReferral");
            var $problems = $(form + " #divProblems");

            $orderInfomation.resetAllControls(null);
            $testInformation.resetAllControls(null);
            $problems.resetAllControls(null);

            //Clear and draw the data table
            //Patient_Referrals_Outgoing_Detail.EditableGrid.datatable.clear().draw();

            utility.CreateDatePicker(Patient_Referrals_Outgoing_Detail.params.PanelID + ' #dtDate', function () {
                //on-change callback method
            }, true);
            utility.CreateDatePicker(Patient_Referrals_Outgoing_Detail.params.PanelID + ' #dtDateFrom', function () {
                //on-change callback method
            }, true);
            utility.CreateDatePicker(Patient_Referrals_Outgoing_Detail.params.PanelID + ' #dtDateTo', function () {
                //on-change callback method
            }, true);
            utility.ValidateFromToDate('frmPatientReferralsOutgoingDetail', "dtDateFrom", "dtDateTo", true);
            $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + ' #tmTime').timepicker({
                defaultTime: new Date()
            });

            //revalidate the required fields
            $(form).bootstrapValidator('revalidateField', 'Status').bootstrapValidator('revalidateField', 'RefProvider');

        }, function () { },
            '22'
        );
    },

    isProblemExists: function (problem) {

        var self = $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail");
        var exists = false;
        $(self).find("#ulProblemLists td").each(function (index, item) {
            if (problem == $(this).text().trim()) {
                exists = true;
            }
        });

        return exists;
    },

    sendAsFax: function () {
        var params = [];
        var ReferralId = $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #hfReferralID").val() != "" ? $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #hfReferralID").val() : "-1";
        var patientID = $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #hfPatientId").val();
        var userID = globalAppdata['AppUserId'];

        Patient_Referrals_Outgoing_Detail.previewReferral(patientID, userID, ReferralId).done(function (response) {
            try {
                response = JSON.parse(response);
                Patient_ReferralsView.pdf = response.ReferralHTML;
                //utility.documentPrint(response.ReferralHTML);
                var patchTemporary = "data:application/pdf;base64,";
                params["PDFBase64"] = response.ReferralHTML;
                params["ParentCtrl"] = "Patient_Referrals_Outgoing_Detail";
                params["PatientId"] = patientID;
                LoadActionPan("Batch_FaxSend", params);
            }
            catch (ex) {
                console.log(ex);
            }
        });
    },

    previewReferral: function (patientID, userID, ReferralId) {

        var objData = {};
        objData["PatientId"] = patientID;
        objData["ReferralId"] = ReferralId;
        objData["commandType"] = "preview_referral";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");
    },

    OpenSpecialtyFrom: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmPatientReferralsOutgoingDetail";
        params["FacilityFrom"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtSpecialtyFrom";
        params["RefHiddenIdCtrl"] = "hfSpecialtyFrom";
        params["ParentCtrl"] = "Patient_Referrals_Outgoing_Detail";
        LoadActionPan('Admin_Specialty', params);
    },


    BindFacilityFrom: function () {
        var Ctrl = $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #txtFacilityFrom");
        var func = function () { return Patient_Referrals_Outgoing_Detail.GetFacilityArrayByName(Ctrl.val(), 1) };
        var hfCtrl = $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #hfFacilityFrom");
        utility.BindKendoAutoComplete(Ctrl, 3, "value", "contains", null, func, hfCtrl);
    },

    BindFacilityTo: function () {
        var Ctrl = $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #txtFacilityTo");
        var func = function () { return Patient_Referrals_Outgoing_Detail.GetFacilityArrayByName(Ctrl.val(), 1) };
        var hfCtrl = $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #hfFacilityTo");
        utility.BindKendoAutoComplete(Ctrl, 3, "value", "contains", null, func, hfCtrl);
    },


    BindReferralFrom: function () {
        var Ctrl = $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #txtProvider");
        var func = function () { return Patient_Referrals_Outgoing_Detail.GetProviderArrayByName(Ctrl.val(), 1) };
        var hfCtrl = $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #hfProvider");
        //utility.BindKendoAutoComplete(Ctrl, 3, "value", "contains", null, func, hfCtrl);

        var onSelect = function (e) {
            if (!e)
                $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #hfProviderNPI").val("");
        };
        var onChange = function (obj) {
            $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #hfProviderNPI").val(obj.npi)
        };
        utility.BindKendoAutoComplete(Ctrl, 3, "value", "contains", null, func, hfCtrl, onChange, onSelect);
    },

    BindReferralTo: function () {
        var Ctrl = $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #txtRefProvider");
        var func = function () { return Patient_Referrals_Outgoing_Detail.GetRefProviderArrayByName(Ctrl.val(), 1) };
        var hfCtrl = $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #hfRefProvider");
        //utility.BindKendoAutoComplete(Ctrl, 3, "value", "contains", null, func, hfCtrl);

        var onSelect = function (e) {
            if(!e)
                $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #hfRefProviderNPI").val("");
        };
        var onChange = function (obj) {
            $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #hfRefProviderNPI").val(obj.npi)
            };
        utility.BindKendoAutoComplete(Ctrl, 3, "value", "contains", null, func, hfCtrl, onChange, onSelect);

    },
    BindSpecialty: function () {
        var Ctrl = $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #txtSpecialtyFrom");
        var func = function () { return Patient_Referrals_Outgoing_Detail.GetSpecialtyArrayByName(Ctrl.val(), 1) };
        var hfCtrl = $("#" + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #hfSpecialtyFrom");
        utility.BindKendoAutoComplete(Ctrl, 3, "value", "contains", null, func, hfCtrl);
    },
    LoadActiveFacilitiesByName: function (Searchstring) {
        var objData = new Object();
        objData["ShortName"] = Searchstring;
        objData["commandType"] = "SEARCH_FACILITY_BY_SHORTNAME";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");
    },
    LoadActiveProvidersByName: function (Searchstring) {
        var objData = new Object();
        objData["ShortName"] = Searchstring;
        objData["commandType"] = "SEARCH_PROVIDER_BY_SHORTNAME";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");
    },

    LoadActiveRefProvidersByName: function (Searchstring) {
        var objData = new Object();
        objData["ShortName"] = Searchstring;
        objData["commandType"] = "SEARCH_REFPROVIDER_BY_SHORTNAME";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");
    },
    LoadActiveSpecialtyByName: function (Searchstring) {
        var objData = new Object();
        objData["ShortName"] = Searchstring;
        objData["commandType"] = "SEARCH_SPECIALTY_BY_SHORTNAME";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");
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

                            AllPatients.push({ id: item.FacilityId, value: item.Description });


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

    GetProviderArrayByName: function (ShortName, IsAccountWithFullName, IsGetAll) {
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
            Patient_Referrals_Outgoing_Detail.LoadActiveProvidersByName(ShortName).done(function (responseData) {
                responseData = JSON.parse(responseData);
                if (responseData.status != false) {
                    if (responseData.PatientCount > 0) {
                        var PatientLoadJSONData = JSON.parse(responseData.PatientLoad_JSON);
                        $.each(PatientLoadJSONData, function (i, item) {

                            AllPatients.push({ id: item.ProviderId, value: item.FullName, npi:item.NPI });


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

    GetRefProviderArrayByName: function (ShortName, IsAccountWithFullName, IsGetAll) {
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
            Patient_Referrals_Outgoing_Detail.LoadActiveRefProvidersByName(ShortName).done(function (responseData) {
                responseData = JSON.parse(responseData);
                if (responseData.status != false) {
                    if (responseData.PatientCount > 0) {
                        var PatientLoadJSONData = JSON.parse(responseData.PatientLoad_JSON);
                        $.each(PatientLoadJSONData, function (i, item) {

                            AllPatients.push({ id: item.ReferringProviderId, value: item.FullName, npi: item.NPI });


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

    GetSpecialtyArrayByName: function (ShortName, IsAccountWithFullName, IsGetAll) {
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
            Patient_Referrals_Outgoing_Detail.LoadActiveSpecialtyByName(ShortName).done(function (responseData) {
                responseData = JSON.parse(responseData);
                if (responseData.status != false) {
                    if (responseData.PatientCount > 0) {
                        var PatientLoadJSONData = JSON.parse(responseData.PatientLoad_JSON);
                        $.each(PatientLoadJSONData, function (i, item) {

                            AllPatients.push({ id: item.Id, value: item.Description });


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

    validateAssignee: function (value) {
        if (value == "") {
            $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #frmPatientReferralsOutgoingDetail #hfAssignee").val("");
        }
    },

}