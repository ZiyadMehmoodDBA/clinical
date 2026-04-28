Patient_Referrals_Incoming_Detail = {
    bIsFirstLoad: true,
    params: [],
    EditableGrid: null,
    ReferralProblems: [],
    ReferralProcedureChange: false,
    ReferralProblemsChange: false,
    Load: function (params) {
        Patient_Referrals_Incoming_Detail.params = params;

        if (Patient_Referrals_Incoming_Detail.params.PanelID != 'pnlPatientReferralsIncomingDetail') {
            Patient_Referrals_Incoming_Detail.params.PanelID = Patient_Referrals_Incoming_Detail.params.PanelID + ' #pnlPatientReferralsIncomingDetail';
        }
        else {
            Patient_Referrals_Incoming_Detail.params.PanelID = 'pnlPatientReferralsIncomingDetail';
        }

        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        if (Patient_Referrals.params.ParentCtrl == "Clinical_FaceSheet") {
            $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #hfPatientId").val(Clinical_FaceSheet.params.patientID);
        }
        utility.CreateDatePicker(Patient_Referrals_Incoming_Detail.params.PanelID + ' #dtDate', function () {
            //on-change callback method 
        }, true);
        utility.CreateDatePicker(Patient_Referrals_Incoming_Detail.params.PanelID + ' #dtDateFrom', function () {
            //on-change callback method 
        }, true);
        utility.CreateDatePicker(Patient_Referrals_Incoming_Detail.params.PanelID + ' #dtDateTo', function () {
            //on-change callback method 
        }, true);
        utility.ValidateFromToDate('frmPatientReferralsIncomingDetail', "dtDateFrom", "dtDateTo", true);
        $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + ' #tmTime').timepicker({
            defaultTime: new Date()
        });

        if (Patient_Referrals_Incoming_Detail.params.ParentCtrl == "appointmentDetail" || Patient_Referrals_Incoming_Detail.params.ParentCtrl == "schcheckin" || Patient_Referrals_Incoming_Detail.params.ParentCtrl == "schcheckout") {
            $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #hfPatientId").val(Patient_Referrals_Incoming_Detail.params.PatientId);
            $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #txtProvider").val(Patient_Referrals_Incoming_Detail.params.ReferralTo);
            $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #hfProvider").val(Patient_Referrals_Incoming_Detail.params.ReferralToId);
            $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #txtReason").val(Patient_Referrals_Incoming_Detail.params.Reason);

            $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #ddlReferralStatus").val(1);
        }

        var PanelReferralGrid = "#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #pnlConsultation_Result";
        var ReferralGridId = "#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #dgvProcedureReferral";

        $(ReferralGridId + " tbody tr").remove();
        Patient_Referrals_Incoming_Detail.EditableGrid = EMRUtility.MakeEditableGrid(PanelReferralGrid, ReferralGridId, Patient_Referrals_Incoming_Detail, "0", false, false, false, false);
        Patient_Referrals_Incoming_Detail.LoadAllAutocomplete();
        Patient_Referrals_Incoming_Detail.ReferralProblems = [];
        Patient_Referrals_Incoming_Detail.DomeReady();

        var self = $('#' + Patient_Referrals_Incoming_Detail.params.PanelID);
        if (Patient_Referrals_Incoming_Detail.bIsFirstLoad == true) {
            self.loadDropDowns(true).done(function () {
                var $options = $(self).find("#ddlFacilityTo > option").clone();
                $(self).find('#ddlFacilityFrom').append($options);

                if (Patient_Referrals_Incoming_Detail.params.ParentCtrl == "mstrTabDashBoard") {
                    $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #hfPatientId").val(Patient_Referrals_Incoming_Detail.params.PatientId);
                }
                var patientId = $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #hfPatientId").val();
                CacheManager.BindDropDownsByID("#pnlPatientReferralsIncomingDetail #ddlInsurance", 'GetPatientInsurance', true, patientId).done(function () {

                    if (Patient_Referrals_Incoming_Detail.params.ParentCtrl == 'appointmentDetail') {
                        $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #ddlInsurance").val(Patient_Referrals_Incoming_Detail.params.InsurancePlan);
                    }
                    else {
                        if (self.find("#ddlInsurance option").length > 1) {
                            $(self.find("#ddlInsurance option")[1]).prop('selected', true);
                        }
                    }

                    Patient_Referrals_Incoming_Detail.ValidateIncomingReferrals();
                    if (Patient_Referrals_Incoming_Detail.params.mode == "Edit") {
                        $.when(Patient_Referrals_Incoming_Detail.loadReferralData()).then(function () {
                            setTimeout(function () {
                                $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #PrintButton").prop("disabled", false);
                                $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #faxButton").prop("disabled", false);
                                $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #frmPatientReferralsIncomingDetail").data('serialize', $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #frmPatientReferralsIncomingDetail").serialize());
                            }, 200)

                        });
                    }
                    else {
                        $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #frmPatientReferralsIncomingDetail").data('serialize', $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #frmPatientReferralsIncomingDetail").serialize());
                    }

                    //$.when(Patient_Referrals_Incoming_Detail.loadProblemList()).then(function () {
                    //    Patient_Referrals_Incoming_Detail.ValidateIncomingReferrals();
                    //    if (Patient_Referrals_Incoming_Detail.params.mode == "Edit") {
                    //        $.when(Patient_Referrals_Incoming_Detail.loadReferralData()).then(function () {
                    //            setTimeout(function () {
                    //                $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #PrintButton").prop("disabled", false);
                    //                $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #frmPatientReferralsIncomingDetail").data('serialize', $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #frmPatientReferralsIncomingDetail").serialize());
                    //            }, 200)

                    //        });
                    //    }
                    //    else {
                    //        $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #frmPatientReferralsIncomingDetail").data('serialize', $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #frmPatientReferralsIncomingDetail").serialize());
                    //    }
                    //});
                });
                Patient_Referrals_Incoming_Detail.IntializeMultiSelectDropDownStatusReason(Patient_Referrals_Incoming_Detail.params.PanelID);
            });
        }

        Patient_Referrals_Incoming_Detail.BindFacilityFrom();
        Patient_Referrals_Incoming_Detail.BindFacilityTo();

        if (Patient_Referrals_Incoming_Detail.params.mode == "Add") {
            $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #menuAttach").remove();
            $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #attachmentDiv").html('<a id="btnViewAttachment" href="#" class="dropdown-toggle btn btn-link btn-xs p-none" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false" onclick="">View Attachment</a>');
            $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #btnScanResult,#btnViewAttachment").addClass("disableAll");
            $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #frmPatientReferralsIncomingDetail #txtFacilityFrom").val(globalAppdata["DefaultFacilityDescription"]);
            $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #frmPatientReferralsIncomingDetail #hfFacilityFrom").val(globalAppdata["DefaultFacilityId"]);
            var $ctr = $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #frmPatientReferralsIncomingDetail #txtFacilityFrom");
            var $hfctr = $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #frmPatientReferralsIncomingDetail #hfFacilityFrom");
            utility.SetKendoAutoCompleteSourceforValidate($ctr, $ctr.val(), $hfctr, $hfctr.val());
        }
        else if (Patient_Referrals_Incoming_Detail.params.mode == "Edit") {
            $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #attachDiv").append('<ul id="menuAttach" class="dropdown-menu" aria-labelledby="btnScanResult">' +
               '<li><a href="#" onclick="Patient_Referrals_Incoming_Detail.documentScan()">Scan</a></li><li><a href="#" onclick="Patient_Referrals_Incoming_Detail.documentImport()">Upload</a></li></ul>');

            $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #attachmentDiv").html('<a id="btnViewAttachment" href="#" class="dropdown-toggle btn btn-link btn-xs p-none" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false" onclick="Patient_Referrals_Incoming_Detail.loadAttachments()">View Attachment</a><ul id="menuViewAttachment" class="dropdown-menu" aria-labelledby="btnViewAttachment"></ul>');
            $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #btnScanResult,#btnViewAttachment").removeClass("disableAll");
        }

        if (Patient_Referrals_Incoming_Detail.params["IsViewOnly"]
            && Patient_Referrals_Incoming_Detail.params["IsViewOnly"] == true) {
            $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #frmPatientReferralsIncomingDetail .canDisable").addClass("disableAll");
            $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #IncomingScreenTitle").html("Incoming Referral");
        }

    },

    DomeReady: function () {
        $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #dgvProcedureReferral").change(function () {
            Patient_Referrals_Incoming_Detail.ReferralProcedureChange = true;

        });

        $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #ulProblemLists").change(function () {
            Patient_Referrals_Incoming_Detail.ReferralProblemsChange = true;
        });
    },

    ValidateIncomingReferrals: function () {

        var $self = $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #frmPatientReferralsIncomingDetail");
        var fields = {};
        fields["Status"] = {
            group: '.col-sm-3',
            validators: {
                notEmpty: {
                    message: ''
                }
            }
        };
        fields["RefProvider"] = {
            group: '.col-sm-3',
            validators: {
                notEmpty: {
                    message: ''
                }
            }
        },
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

        //$('#frmPatientReferralsIncomingDetail')
        //   .bootstrapValidator({
        //       live: 'disabled',
        //       message: 'This value is not valid',
        //       feedbackIcons: {
        //           valid: 'glyphicon glyphicon-ok',
        //           invalid: 'glyphicon glyphicon-remove',
        //           validating: 'glyphicon glyphicon-refresh'
        //       },
        //       fields: {

        //           RefProvider: {
        //               group: '.col-sm-3',
        //               validators: {
        //                   notEmpty: {
        //                       message: ''
        //                   }
        //               }
        //           },               
        //           Status: {
        //               group: '.col-sm-3',
        //               validators: {
        //                   notEmpty: {
        //                       message: ''
        //                   }
        //               }
        //           },
        //       }
        //   })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            Patient_Referrals_Incoming_Detail.IncomingReferralSave();
        });
    },

    loadReferralData: function () {
        var dfd = $.Deferred();
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Referrals", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Patient_Referrals_Incoming_Detail.fillReferral(Patient_Referrals_Incoming_Detail.params.ReferralId).done(function (response) {
                    if (response != "") {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            var ReferralDetail = JSON.parse(response.ReferralListLoad_JSON);
                            $.when(Patient_Referrals_Incoming_Detail.ProcedureGridLoad(response)).then(function () {
                                $.when(Patient_Referrals_Incoming_Detail.GetStatusReasons(Patient_Referrals_Incoming_Detail.params.PanelID, ReferralDetail.Status)).done(function () {
                                    utility.callbackAfterAllDOMLoaded(function () {
                                    var self = $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #frmPatientReferralsIncomingDetail");
                                    utility.bindMyJSONByName(true, ReferralDetail, false, self).done(function () {
                                        if (Patient_Referrals_Incoming_Detail.params.ParentCtrl == 'appointmentDetail') {
                                            $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #ddlInsurance").val(Patient_Referrals_Incoming_Detail.params.InsurancePlan);
                                        }
                                        $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + ' #dtDateFrom').datepicker('setDate', ReferralDetail.DateFrom);
                                        $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + ' #dtDateTo').datepicker('setDate', ReferralDetail.DateTo);

                                        var $ctr = $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #frmPatientReferralsIncomingDetail #txtFacilityFrom");
                                        var $hfctr = $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #frmPatientReferralsIncomingDetail #hfFacilityFrom");
                                        utility.SetKendoAutoCompleteSourceforValidate($ctr, $ctr.val(), $hfctr, $hfctr.val());

                                        if (ReferralDetail.StatusReasonIds) {
                                            var arrStatusReasonIds = ReferralDetail.StatusReasonIds.split(',');
                                            $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #ddlStatusReasons").val(arrStatusReasonIds);
                                            $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #ddlStatusReasons").multiselect("refresh");
                                        }

                                        if (response.ReferralProblemListLoad_JSON != null) {
                                            response.ReferralProblemListLoad_JSON = JSON.parse(response.ReferralProblemListLoad_JSON);
                                            for (var index in response.ReferralProblemListLoad_JSON) {
                                                $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #ulProblemLists td #chk" + response.ReferralProblemListLoad_JSON[index].ProblemId).attr("checked", "checked");
                                            }
                                        }
                                        $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #hfReferralID").val(ReferralDetail.ReferralId);
                                        dfd.resolve();
                                    });
                                    });
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

    fillReferral: function (ReferralId) {
        var objData = {};
        objData["ReferralId"] = ReferralId;
        objData["LoadFor"] = "Edit";
        objData["Type"] = "Incoming";
        objData["commandType"] = "SEARCH_REFERRAL";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");
    },


    //End Edit Work

    //Author: M Ahmad Imran
    //Date :  10-05-2016
    //Reason: unload Patient_Referrals_Incoming_Detail
    UnLoadTab: function () {

        if (Patient_Referrals_Incoming_Detail.params.ParentCtrl == 'patTabReferrals') { // || Patient_Referrals_Incoming_Detail.params.ParentCtrl == 'Patient_Referrals'
            UnloadActionPan(Patient_Referrals_Incoming_Detail.params["ParentCtrl"], "Patient_Referrals_Incoming_Detail");
        } else if (Patient_Referrals_Incoming_Detail.params.ParentCtrl == 'appointmentDetail') {
            UnloadActionPan(Patient_Referrals_Incoming_Detail.params.ParentCtrl, 'Patient_Referrals_Incoming_Detail', null, "pnlScheduleCalendar");
        }
        else if (Patient_Referrals_Incoming_Detail.params.ParentCtrl == 'schcheckin') {
            UnloadActionPan(Patient_Referrals_Incoming_Detail.params.ParentCtrl, 'Patient_Referrals_Incoming_Detail', null, "schcheckin");
        }
        else if (Patient_Referrals_Incoming_Detail.params.ParentCtrl == 'schcheckout') {
            UnloadActionPan(Patient_Referrals_Incoming_Detail.params.ParentCtrl, 'Patient_Referrals_Incoming_Detail', null, "schcheckout");
        }
        else if (Patient_Referrals_Incoming_Detail.params.ParentCtrl == 'mstrTabDashBoard') {
            UnloadActionPan(Patient_Referrals_Incoming_Detail.params.ParentCtrl, 'Patient_Referrals_Incoming_Detail', null, "pnlDashboard");
        } else if (Patient_Referrals_Incoming_Detail.params.ParentCtrl == 'Patient_Referrals') {
            //UnloadActionPan(Patient_Referrals_Outgoing_Detail.params["ParentCtrl"], "Patient_Referrals_Outgoing_Detail", null, "pnlClinicalFaceSheet #pnlPatientReferrals");
            UnloadActionPan(Patient_Referrals_Incoming_Detail.params["ParentCtrl"], "Patient_Referrals_Incoming_Detail", null, Patient_Referrals_Incoming_Detail.params["ParentCtrlPanelID"]);
        }
        else if (Patient_Referrals_Incoming_Detail.params.ParentCtrl == 'Patient_Referrals') {
            UnloadActionPan(Patient_Referrals_Incoming_Detail.params["ParentCtrl"], "Patient_Referrals_Incoming_Detail", null, Patient_Referrals_Incoming_Detail.params["ParentCtrlPanelID"]);
        }
        else {
            UnloadActionPan(Patient_Referrals_Incoming_Detail.params.ParentCtrl, 'Patient_Referrals_Incoming_Detail', null, "pnlClinicalProgressNote #pnlPatientReferrals");
        }
    },
    //Author: M Ahmad Imran
    //Date :  10-05-2016
    //Reason: open provider form
    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmPatientReferralsIncomingDetail";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Patient_Referrals_Incoming_Detail";
        LoadActionPan('Admin_Provider', params);
    },


    OpenRefProvider: function () {
        var params = [];
        params["ReferringProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefForm"] = "frmPatientReferralsIncomingDetail";
        params["IsOptional"] = false;
        params["RefCtrl"] = "txtRefProvider";
        params["ParentCtrl"] = "Patient_Referrals_Incoming_Detail";
        LoadActionPan('Admin_ReferringProvider', params);
    },

    OpenFacilityTo: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmPatientReferralsIncomingDetail";
        params["FacilityTo"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacilityTo";
        params["RefHiddenIdCtrl"] = "hfFacilityTo";
        params["LoadAllFacility"] = "True";
        params["ParentCtrl"] = "Patient_Referrals_Incoming_Detail";
        LoadActionPan('Admin_Facility', params);
    },

    OpenFacilityFrom: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmPatientReferralsIncomingDetail";
        params["FacilityFrom"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacilityFrom";
        params["RefHiddenIdCtrl"] = "hfFacilityFrom";
        params["ParentCtrl"] = "Patient_Referrals_Incoming_Detail";
        LoadActionPan('Admin_Facility', params);
    },

    //Function Name: OpenAssignee
    //Author: M Ahmad Imran
    //Date :  10-05-2016
    //Description: auto complete for assignee
    OpenAssignee: function () {

        CacheManager.BinachCodes('GetUsers', true).done(function (result) {
            $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #frmPatientReferralsIncomingDetail #txtAssignee").autocomplete({
                autoFocus: true,
                source: Users,
                select: function (event, ui) {

                    setTimeout(function () {

                        $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #frmPatientReferralsIncomingDetail  #hfAssignee").val(ui.item.id);
                    }, 100);
                }
            });
        });


        //var params = [];
        //params["IsOptional"] = true;
        //params["RefForm"] = "frmPatientReferralsIncomingDetail";
        //params["AssigneeId"] = "-1";
        //params["FromAdmin"] = "0";
        //params["ParentCtrl"] = "Patient_Referrals_Incoming_Detail";
        //params["RefCtrl"] = "txtAssignee";
        //params["RefCtrlHidden"] = "hfAssignee";
        //params["RefCtrlLink"] = "lnkAssignee";
        //LoadActionPan('Admin_Provider', params);
    },
    LoadAllAutocomplete: function () {

        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            $("#frmPatientReferralsIncomingDetail #txtProvider").autocomplete({
                autoFocus: true,
                source: Providers, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#pnlPatientReferralsIncomingDetail #txtProvider").attr("ProviderId", ui.item.id); // add the selected id
                        $("#pnlPatientReferralsIncomingDetail #hfProvider").val(ui.item.id); // add the selected id
                        if ($("#pnlPatientReferralsIncomingDetail #lnkProviderEdit").css("display") == "none") {
                            $("#pnlPatientReferralsIncomingDetail #lnkProviderEdit").css("display", "inline");
                            $("#pnlPatientReferralsIncomingDetail #lblProvider").css("display", "none");
                        }
                    }, 100);
                }
            });
        });
        //CacheManager.BindCodes('GetProvider', false).done(function (result) {
        //    $("input#txtAssignee").autocomplete({
        //        autoFocus: true,
        //        source: Providers, // pass an array
        //        select: function (event, ui) {
        //            setTimeout(function () {
        //                $("#pnlPatientReferralsIncomingDetail #txtAssignee").attr("AssigneeId", ui.item.id); // add the selected id
        //                $("#pnlPatientReferralsIncomingDetail #hfAssignee").val(ui.item.id); // add the selected id
        //                if ($("#pnlPatientReferralsIncomingDetail #lnkAssigneeEdit").css("display") == "none") {
        //                    $("#pnlPatientReferralsIncomingDetail #lnkAssigneeEdit").css("display", "inline");
        //                    $("#pnlPatientReferralsIncomingDetail #lblAssignee").css("display", "none");
        //                }
        //            }, 100);
        //        }
        //    });
        //});
        CacheManager.BindCodes('GetRefProviders', false).done(function (result) {
            $("input#txtRefProvider").autocomplete({
                autoFocus: true,
                minLength: 3,  //added min length, Abdur Rehman Latif - PMS 2986
                source: RefProviders, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#pnlPatientReferralsIncomingDetail #hfRefProvider").val(ui.item.id);
                        if ($("#pnlPatientReferralsIncomingDetail #lnkRefProviderEdit").css("display") == "none") {
                            $("#pnlPatientReferralsIncomingDetail #lnkRefProviderEdit").css("display", "inline");
                            $("#pnlPatientReferralsIncomingDetail #lblRefProvider").css("display", "none");
                        }
                    }, 100);

                }
            });
        });
    },
    BindFacilityFrom: function () {
        var Ctrl = $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #frmPatientReferralsIncomingDetail #txtFacilityFrom");
        var func = function () {
            return Patient_Referrals_Outgoing_Detail.GetFacilityArrayByName(Ctrl.val(), 1)
        };
        var hfCtrl = $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #frmPatientReferralsIncomingDetail #hfFacilityFrom");
        utility.BindKendoAutoComplete(Ctrl, 3, "value", "contains", null, func, hfCtrl);
    },

    BindFacilityTo: function () {
        var Ctrl = $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #frmPatientReferralsIncomingDetail #txtFacilityTo");
        var func = function () { return Patient_Referrals_Outgoing_Detail.GetFacilityArrayByName(Ctrl.val(), 1) };
        var hfCtrl = $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #frmPatientReferralsIncomingDetail #hfFacilityTo");
        utility.BindKendoAutoComplete(Ctrl, 3, "value", "contains", null, func, hfCtrl);
    },


    LoadIncomingReferalGrid: function () {
        Patient_Referrals.Type = "Incoming";
        Patient_Referrals.ReferralSearch().done(function () {
            Patient_Referrals.fillRefferal();
            Patient_Referrals_Incoming_Detail.UnLoadTab();
        });
    },
    //Function Name: IncomingReferralSave
    //Author Name: M Ahmad Imran
    //Created Date: 12-05-2016
    //Description: Saves CIncoming Referral    
    IncomingReferralSave: function () {

        if (EMRUtility.compareFormDataWithSerialized(Patient_Referrals_Incoming_Detail.params.PanelID + ' #frmPatientReferralsIncomingDetail')) {
            var ReferralId = $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #hfReferralID").val() != "" ? $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #hfReferralID").val() : "-1";
            if (parseInt(ReferralId) > 0) {
                Patient_Referrals_Incoming_Detail.params.mode = "Edit";
            }
            else {
                Patient_Referrals_Incoming_Detail.params.mode = "Add";
            }

            var self = $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #frmPatientReferralsIncomingDetail");

            var mainErrorMessage = "";

            if (mainErrorMessage == "") {
                var myJSON = self != null ? self.getMyJSONByName() : "{}";
                var objData = JSON.parse(myJSON);

                if (Patient_Referrals_Incoming_Detail.params.ParentCtrl == 'mstrTabDashBoard') {
                    objData["PatientId"] = Patient_Referrals_Incoming_Detail.params.PatientId;
                }
                else {
                    objData["PatientId"] = $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #hfPatientId").val(); // $('#PatientProfile #hfPatientId').val();
                }

                objData["Type"] = 'Incoming';

                myJSON = JSON.stringify(objData);

                //------------------------------------------------------------

                //JSONToSave = decodeURIComponent(myJSON);
                JSONToSave = myJSON;

                //--------------------------------------------------------------
                var strMessage = "";
                if (Patient_Referrals_Incoming_Detail.params.mode == "Add") {
                    var strMessage = "";
                    AppPrivileges.GetFormPrivileges("Referrals", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            Patient_Referrals_Incoming_Detail.SaveReferral(JSONToSave).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    utility.DisplayMessages(response.message, 1);

                                    if (Patient_Referrals_Incoming_Detail.params.ParentCtrl == 'appointmentDetail') {
                                        $('#appointmentDetail #btnAddReferral').text('View Referral');
                                        $('#appointmentDetail #hfReferralId').val(response.ReferralId);
                                        appointmentDetail.ReferralId = response.ReferralId;
                                        Patient_Referrals_Incoming_Detail.UnLoadTab();
                                    }
                                    else if (Patient_Referrals_Incoming_Detail.params.ParentCtrl == 'schcheckin') {
                                        $('#schcheckin #btnAddReferral').text('View Referral');
                                        $('#schcheckin #hfReferralId').val(response.ReferralId);
                                        Patient_Referrals_Incoming_Detail.UnLoadTab();
                                    }
                                    else if (Patient_Referrals_Incoming_Detail.params.ParentCtrl == 'schcheckout') {
                                        $('#schcheckout #btnAddReferral').text('View Referral');
                                        $('#schcheckout #hfReferralId').val(response.ReferralId);
                                        Patient_Referrals_Incoming_Detail.UnLoadTab();
                                    }
                                    else {
                                        if (Patient_Referrals.params.ParentCtrl == "clinicalTabProgressNote" && Patient_Referrals_Outgoing_Detail.params.ParentCtrl != 'Clinical_Treatment') {
                                            if (response.ReferralId != null && response.ReferralId != '')
                                                Patient_Referrals.getReferralInfo(response.ReferralId).done(function () {
                                                    Patient_Referrals_Incoming_Detail.LoadIncomingReferalGrid();
                                                });
                                        }
                                        else
                                            Patient_Referrals_Incoming_Detail.LoadIncomingReferalGrid();
                                    }
                                    //$('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #hfReferralID").val(response.ReferralId);
                                    //Patient_Referrals_Incoming_Detail.params.mode = "Edit";
                                    //Patient_Referrals_Incoming_Detail.ReferralProcedureChange = false;
                                    //Patient_Referrals_Incoming_Detail.ReferralProblemsChange = false;
                                    //$('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #frmPatientReferralsIncomingDetail").data('serialize', $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #frmPatientReferralsIncomingDetail").serialize());

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

                else if (Patient_Referrals_Incoming_Detail.params.mode == "Edit") {

                    AppPrivileges.GetFormPrivileges("Referrals", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            Patient_Referrals_Incoming_Detail.updateReferral(JSONToSave, ReferralId).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {

                                    utility.DisplayMessages(response.message, 1);
                                    if (Patient_Referrals_Incoming_Detail.params.ParentCtrl == 'appointmentDetail') {
                                        $('#appointmentDetail #btnAddReferral').text('View Referral');
                                        $('#appointmentDetail #hfReferralId').val(response.ReferralId);
                                        appointmentDetail.ReferralId = response.ReferralId;
                                        Patient_Referrals_Incoming_Detail.UnLoadTab();
                                    }
                                    else if (Patient_Referrals_Incoming_Detail.params.ParentCtrl == 'schcheckin') {
                                        $('#schcheckin #btnAddReferral').text('View Referral');
                                        $('#schcheckin #hfReferralId').val(response.ReferralId);
                                        Patient_Referrals_Incoming_Detail.UnLoadTab();
                                    }
                                    else if (Patient_Referrals_Incoming_Detail.params.ParentCtrl == 'schcheckout') {
                                        $('#schcheckout #btnAddReferral').text('View Referral');
                                        $('#schcheckout #hfReferralId').val(response.ReferralId);
                                        Patient_Referrals_Incoming_Detail.UnLoadTab();
                                    }
                                    else if (Patient_Referrals_Incoming_Detail.params.ParentCtrl == 'mstrTabDashBoard') {
                                        Patient_Referrals_Incoming_Detail.UnLoadTab();
                                        DashBoard.DashBoardIncomingReferralGridLoad(null, null, null);
                                    }
                                    else {
                                        if (Patient_Referrals_Incoming_Detail.params.ParentCtrl == "Patient_Referrals" && Patient_Referrals.params.ParentCtrl == "clinicalTabProgressNote") {
                                            Clinical_ProgressNote.AttachedNoteComponentIds.push(response.ReferralId);
                                        }
                                        if (Patient_Referrals.params.ParentCtrl == "clinicalTabProgressNote" && Patient_Referrals_Outgoing_Detail.params.ParentCtrl != 'Clinical_Treatment') {
                                            if (response.ReferralId != null && response.ReferralId != '')
                                                Patient_Referrals.getReferralInfo(response.ReferralId).done(function () {
                                                    Patient_Referrals_Incoming_Detail.LoadIncomingReferalGrid();
                                                });
                                        }
                                        else
                                            Patient_Referrals_Incoming_Detail.LoadIncomingReferalGrid();
                                    }
                                    //Patient_Referrals_Incoming_Detail.ReferralProcedureChange = false;
                                    //Patient_Referrals_Incoming_Detail.ReferralProblemsChange = false;
                                    //$('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #frmPatientReferralsIncomingDetail").data('serialize', $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #frmPatientReferralsIncomingDetail").serialize());
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
                utility.DisplayMessages(mainErrorMessage, 3);
            }
        } else {
            utility.DisplayMessages("Please make any changes to save/update Incoming Referral", 3);
            setTimeout(function () {
                $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + ' #frmPatientReferralsIncomingDetail').data('serialize', $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + ' #frmPatientReferralsIncomingDetail').serialize());
            }, 100);
        }

    },

    sendAsFax: function () {
        var params = [];
        var ReferralId = $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #hfReferralID").val() != "" ? $('#' + Patient_Referrals_Outgoing_Detail.params.PanelID + " #hfReferralID").val() : "-1";
        var patientID = $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #hfPatientId").val();
        var userID = globalAppdata['AppUserId'];

        Patient_Referrals_Outgoing_Detail.previewReferral(patientID, userID, ReferralId).done(function (response) {
            try {
                response = JSON.parse(response);
                Patient_ReferralsView.pdf = response.ReferralHTML;
                //utility.documentPrint(response.ReferralHTML);
                var patchTemporary = "data:application/pdf;base64,";
                params["PDFBase64"] = response.ReferralHTML;
                params["ParentCtrl"] = "Patient_Referrals_Incoming_Detail";
                params["PatientId"] = patientID;
                LoadActionPan("Batch_FaxSend", params);
            }
            catch (ex) {
                console.log(ex);
            }
        });
    },

    //Function Name: ConsultationOrderSave
    //Author Name: Humaira Yousaf
    //Created Date: 02-03-2016
    //Description: Saves ConsultationOrder 
    //Params: ConsultationOrderData
    SaveReferral: function (ReferralData) {
        var objData = JSON.parse(ReferralData);
        objData["commandType"] = "save_Referral";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");
    },
    updateReferral: function (ReferralData, ReferralId) {

        var objData = JSON.parse(ReferralData);
        objData["ReferralId"] = ReferralId;
        objData["commandType"] = "save_Referral";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");

    },
    //------------------ start M Ahmad Imran Code for editable grid

    bindAutoComplete: function (element) {
        var hiddenCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "Patient_Referrals_Incoming_Detail", null, true);
    },
    openCPTCode: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Patient_Referrals_Incoming_Detail";
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtCPTCode";
        params["ParentCtrlPanelID"] = Patient_Referrals_Incoming_Detail.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, Patient_Referrals_Incoming_Detail.params.PanelID);
    },
    BindProcedureGridItem: function (cptCode, procedureDescription, cptDescription, SnomedID, SnomedDescription) {

        var cptCode = cptCode;
        var procDesc = procedureDescription;
        var cptDesc = cptDescription;

        var currentRow = $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #dgvProcedureReferral tbody tr");
        var isTestAlreadySelected = false;
        $.each(currentRow, function (i, item) {
            var currentRowCPTDescription = $(item).find("input[id*='Procedure']").val() != null ? $(item).find("input[id*='Procedure']").val().replace(cptCode, "").trim() : "";
            var currentRowCPTDescription = currentRowCPTDescription.split('(')[0].trim();
            if ((cptDescription).toLowerCase() == currentRowCPTDescription.toLowerCase()) {
                isTestAlreadySelected = true;
                return;
            }
        });

        if (isTestAlreadySelected != true) {

            Patient_Referrals_Incoming_Detail.AddNewProcedureRow(null, null, null, cptCode, procDesc, cptDescription);
            setTimeout(function () {
                $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #txtReferralCPTCode").val('');
            }, 200);
        }
        else {
            utility.DisplayMessages("Procedure is already selected", 2);
        }
    },
    AddNewProcedureRow: function (RowId, mode, CurrRef, cptCode, procDesc, cptDescription) {

        var ReferralGridId = "#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #dgvProcedureReferral";

        var CurrentRow = null;
        if (RowId && RowId > 0) {
            if (Patient_Referrals_Incoming_Detail.params.ParentCtrl != null) {
                CurrentRow = Patient_Referrals_Incoming_Detail.EditableGrid.rowAdd(RowId, "");
            }
            else {
                CurrentRow = Patient_Referrals_Incoming_Detail.EditableGrid.rowAdd(RowId, Patient_Referrals_Incoming_Detail.params.ReferralId);
            }
        }
        else {
            var TemplateRow = $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #dgvProcedureReferral tbody tr[id*=-]").last();
            var TemplateRowId = 0;
            if (TemplateRow.length > 0) {
                if (TemplateRow.attr("id").indexOf("Child") != "-1")
                    TemplateRowId = parseInt(TemplateRow.prev().attr("id"));
                else
                    TemplateRowId = parseInt(TemplateRow.attr("id"));
            }
            CurrentRow = Patient_Referrals_Incoming_Detail.EditableGrid.rowAdd(TemplateRowId - 1, "");
        }

        $(CurrentRow).find('input[id*="Procedure"]').val(cptCode != "" ? cptCode + " " + cptDescription : cptDescription);

        Patient_Referrals_Incoming_Detail.enableRemoveRow($(CurrentRow));

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
                        Patient_Referrals_Incoming_Detail.DeleteReferralProcedure($row.attr("id"), $row, obj);
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

        Patient_Referrals_Incoming_Detail.DeleteReferralProcedure_DBCall(ReferralProcedureId).done(function (response) {

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
        //var response = JSON.parse(response);
        var PanelReferralGrid = "#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #pnlReferralProcedure_Result";
        var ReferralGridId = "#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #dgvProcedureReferral";
        $(ReferralGridId + " tbody tr").remove();
        if ($.fn.dataTable.isDataTable(ReferralGridId)) {
            $(ReferralGridId).dataTable().fnClearTable();
            $(ReferralGridId).dataTable().fnDestroy();
        }
        Patient_Referrals_Incoming_Detail.EditableGrid.datatable.clear().draw();
        var ReferralProcedureLoadJSONData = JSON.parse(response.ReferralProcedureListLoad_JSON);
        var TotalProcedures = ReferralProcedureLoadJSONData.length;
        var procedureNo = 0;
        $.each(ReferralProcedureLoadJSONData, function (i, item) {
            var ReferralProcedureId = item.ReferralProcedureId;
            var CurrentRow = Patient_Referrals_Incoming_Detail.AddNewProcedureRow(ReferralProcedureId, null, null, null, null, null, null, null);
            var self = $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " tr#" + ReferralProcedureId);
            var row = Patient_Referrals_Incoming_Detail.EditableGrid.datatable.row(CurrentRow);
            var ReferralPocedureTable = $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #dgvProcedureReferral");

            //Start Farooq Ahmad 03/28/2016 bind values to the table 
            var counter = 0;

            var BindFunction = function (counter, item, CurrentRow, TotalProcedures, procedureNo) {

                utility.bindMyJSONByName(true, item, false, $(CurrentRow)).done(function () {

                });
                procedureNo++;
                if (TotalProcedures == procedureNo) {
                    if (TotalProcedures == 1) {
                        setTimeout(function () {
                            dfd.resolve();
                        }, 1000);
                    }
                    else {
                        dfd.resolve();
                    }

                }
                if (counter++ < 5)
                    setTimeout(BindFunction, 1000, counter, item, CurrentRow, TotalProcedures, procedureNo);

            }
            BindFunction(counter, item, CurrentRow, TotalProcedures, procedureNo);
            //End Farooq Ahmad 03/28/2016 bind values to the table 
        });
        if (TotalProcedures == 0) {
            dfd.resolve();
        }
        return dfd.promise();
    },
    //Start Problem Work///
    //Author: Farooq Ahmad
    //Date: 28-03-2016
    //Reason:to add more problem is Associated Problem list
    addProblem: function () {
        Patient_Referrals_Incoming_Detail.SaveLocalyCheckedProbems();
        if (Patient_Referrals_Incoming_Detail.params.mode == "Edit") {
            var params = [];
            params["RefForm"] = "frmPatientReferralsIncomingDetail";
            params["FromOrderDetail"] = "1";
            params["FromAdmin"] = "0";
            params["ParentCtrl"] = "Patient_Referrals_Incoming_Detail";
            LoadActionPan('Clinical_ProblemLists', params);
        }
        else {
            var params = [];
            params["RefForm"] = "frmPatientReferralsIncomingDetail";
            params["FromOrderDetail"] = "1";
            params["FromAdmin"] = "0";
            params["ParentCtrl"] = "Patient_Referrals_Incoming_Detail";
            LoadActionPan('Clinical_ProblemLists', params);
        }

    },

    SaveLocalyCheckedProbems: function () {
        var self = $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #frmPatientReferralsIncomingDetail");
        Patient_Referrals_Incoming_Detail.ReferralProblems = [];
        $(self).find("#ulProblemLists td").each(function (index, item) {
            if ($(this).find("input:checkbox").prop("checked")) {
                var objProblem = {
                    ProblemId: $(this).find("input:checkbox").val(),
                    Description: $(this).text()
                }
                Patient_Referrals_Incoming_Detail.ReferralProblems.push(objProblem);
            }
        });
    },

    CheckedPreviousProbems: function () {
        var dfd = $.Deferred();
        for (var index in Patient_Referrals_Incoming_Detail.ReferralProblems) {
            $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #ulProblemLists td #chk" + Patient_Referrals_Incoming_Detail.ReferralProblems[index].ProblemId).attr("checked", "checked");
        }
        dfd.resolve();
        return dfd.promise();
    },
    //Author: Farooq Ahmad
    //Date :  17-03-2016
    //Reason: Function to load problem list
    loadProblemList: function () {
        var dfd = new $.Deferred();
        Patient_Referrals_Incoming_Detail.SearchProblemList().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.ProblemListCount > 0) {
                    var ProblemListLoadJSONData = JSON.parse(response.ProblemListLoad_JSON);
                    var ProblemListHistoryLoadJSONData = JSON.parse(response.ProblemListHistoryLoad_JSON);

                    var finalTr = '';
                    var counter = 2;
                    $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #ulProblemLists tbody tr").remove();
                    $.each(ProblemListLoadJSONData, function (i, item) {
                        if (counter % 2 == 0) {
                            finalTr = finalTr + '<tr>';
                        }
                        finalTr = finalTr + '<td><div class="p-xs col-xs-12"><div class="checkbox-custom">';

                        if ($("#hfIMOProblem").val()) {
                            if ($("#hfIMOProblem").val() == item.ProblemName)
                                finalTr = finalTr + '<input checked="checked" type="checkbox" name="' + item.Code + '" value="' + item.ProblemListId + '" id="chk' + item.ProblemListId + '">';
                            else
                                finalTr = finalTr + '<input type="checkbox" name="' + item.Code + '" value="' + item.ProblemListId + '" id="chk' + item.ProblemListId + '">';
                        }
                        else
                            finalTr = finalTr + '<input type="checkbox" name="' + item.Code + '" value="' + item.ProblemListId + '" id="chk' + item.ProblemListId + '">';
                        //finalTr = finalTr + '<input type="checkbox" name="' + item.Code + '" value="' + item.ProblemListId + '" id="chk' + item.ProblemListId + '">';

                        finalTr = finalTr + '   <label class="control-label">' + item.Description + '</label></div></div></td>';

                        if (counter % 2 == 1) {
                            finalTr = finalTr + '</tr>';
                        }
                        counter++;
                        var color = "";
                    });
                    $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #ulProblemLists tbody").append(finalTr);
                    dfd.resolve();
                }
                else {
                    dfd.resolve();
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
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
        IsCheckedIn = $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + ' #pnlProblemLists_Result #divSwitch #switchActive').attr('isactive');
        if (IsCheckedIn == null) {
            IsCheckedIn = "1";
        }

        var PageNumber = 1;
        var RowsPerPage = 2000;

        var objData = new Object();
        objData["PatientId"] = Patient_Referrals.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        objData["IsActive"] = '1';
        // objData["ProblemListId"] = ProblemListId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "SEARCH_PROBLEMLIST";
        //objData["NoteId"] = Clinical_ProblemLists.params.NotesId == null ? 0 : Clinical_ProblemLists.params.NotesId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");

    },




    //Function Name: printReferral
    //Author Name: M Ahmad Imran
    //Created Date: 17-05-2016
    //Description: Creates PDF to view Referral
    printReferral: function () {
        var ReferralId = $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + " #hfReferralID").val();
        var params = [];
        params["FromAdmin"] = "0";
        params["UserId"] = globalAppdata['AppUserId'];
        params["PatientId"] = $("#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #hfPatientId").val(); // $('#PatientProfile #hfPatientId').val();
        params["ParentCtrl"] = "Patient_Referrals_Incoming_Detail";
        params["ReferralId"] = ReferralId;
        Patient_ReferralsView.ReferralPreview(params.PatientId, params.UserId, params.ReferralId);
        //LoadActionPan('Patient_ReferralsView', params);
    },

    documentImport: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Documents", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];

                var AccountNo = $("#PatientProfile #hfAccountNo").val();
                var PatientFullName = $("#PatientProfile #hfPatientFullName").val();
                var PatientId = Patient_Referrals.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $("#PatientProfile #hfPatientId").val();
                var PatientName = "";
                if (PatientFullName.length > 0) {
                    var Firstname = PatientFullName.split(" ")[1];
                    var Lastname = PatientFullName.split(" ")[0];
                    var MiddleInitial = PatientFullName.split(" ")[2];
                    PatientName = Lastname + " " + Firstname + " " + MiddleInitial;
                }
                params["patientId"] = Patient_Referrals.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
                params["RefCtrl"] = "IncomingReferral";
                params['ReferralId'] = Patient_Referrals_Incoming_Detail.params.ReferralId;
                params['RefModuleName'] = "Incoming Referral";
                params['TransitionId'] = Patient_Referrals_Incoming_Detail.params.ReferralId;
                params["FromAdmin"] = "0";
                params["mode"] = "Add";
                params["ParentCtrl"] = 'Patient_Referrals_Incoming_Detail';
                params["PatientName"] = PatientName;
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
                param["RefCtrl"] = "IncomingReferral";
                param['RadiologyResultId'] = Patient_Referrals_Incoming_Detail.params.ReferralId;
                param['RefModuleName'] = "Incoming Referral";
                param['TransitionId'] = Patient_Referrals_Incoming_Detail.params.ReferralId;
                param['patientID'] = Patient_Referrals.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
                param["ParentCtrl"] = 'Patient_Referrals_Incoming_Detail';
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

        Patient_Referrals_Incoming_Detail.loadAttachments_DbCall(radiologyOrderId).done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {

                var ulAttachment = null;

                if (controlName == null)
                    ulAttachment = $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + ' #menuViewAttachment');
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
                            $(ulAttachment).append('<li><a href="#" id="' + item.PatDocId + '" onclick="Patient_Referrals_Incoming_Detail.showAttachment(\'' + item.PatDocId + '\',event)">' + item.ModifiedOn + '</a></li>');
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
        objData["TransitionId"] = Patient_Referrals_Incoming_Detail.params.ReferralId;
        objData["RefModuleName"] = "Incoming Referral";
        objData["PatientId"] = Patient_Referrals.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $("div#PatientProfile #hfPatientId").val();

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
                params["PatientID"] = Patient_Referrals.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
                params["PatDocID"] = PatDocID;
                params["mode"] = "Edit";
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = "Patient_Referrals_Incoming_Detail";

                LoadActionPan('Document_Viewer', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    referralReset: function () {
        utility.myConfirm('22', function () {

            //selectors
            var form = "#" + Patient_Referrals_Incoming_Detail.params.PanelID + " #frmPatientReferralsIncomingDetail";

            var $referralInfomation = $(form + " #divIncomingReferralInfo");
            $referralInfomation.resetAllControls(null);

            $(form + " #txtComments").val('');

            //Clear and draw the data table
            Patient_Referrals_Incoming_Detail.EditableGrid.datatable.clear().draw();

            utility.CreateDatePicker(Patient_Referrals_Incoming_Detail.params.PanelID + ' #dtDate', function () {
                //on-change callback method 
            }, true);
            utility.CreateDatePicker(Patient_Referrals_Incoming_Detail.params.PanelID + ' #dtDateFrom', function () {
                //on-change callback method 
            }, true);
            utility.CreateDatePicker(Patient_Referrals_Incoming_Detail.params.PanelID + ' #dtDateTo', function () {
                //on-change callback method 
            }, true);
            utility.ValidateFromToDate('frmPatientReferralsIncomingDetail', "dtDateFrom", "dtDateTo", true);
            $('#' + Patient_Referrals_Incoming_Detail.params.PanelID + ' #tmTime').timepicker({
                defaultTime: new Date()
            });
            //revalidate the required fields
            $(form).bootstrapValidator('revalidateField', 'Status').bootstrapValidator('revalidateField', 'RefProvider');

        }, function () { },
            '22'
        );
    },
    GetStatusReasons: function (PanelId, ReferralStatusId) {
        var StatusId = '';
        if (PanelId.indexOf('pnlPatientReferralsIncomingDetail') > 0)
            StatusId = Patient_Referrals_Incoming_Detail.params.mode == "Edit" ? ReferralStatusId : $('#' + PanelId + ' #ddlReferralStatus option:selected').val();
        else {
            StatusId = $('#' + PanelId + ' #ddlReferralStatus option:selected').val();
            $('#' + PanelId + ' #StatusReasonDate').css('display', 'none');
            $('#' + PanelId + ' #StatusReasonTime').css('display', 'none');
        }

        if (StatusId) {
            $('#' + PanelId + ' #ddlStatusReasons').attr('disabled', false);
            $('#' + PanelId + ' #ddlStatusReasons').css('display', 'inline');
            Patient_Referrals_Incoming_Detail.getStatusReasons_DBCall(StatusId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var statusReasons = JSON.parse(response.StatusReasons_JSON);
                    var $ddlStatusReasons = $('#' + PanelId + ' #ddlStatusReasons');
                    $ddlStatusReasons.empty();
                    
                    $.each(statusReasons, function (i, item) {
                        $ddlStatusReasons.append(
                            $('<option/>', {
                                value: item.Id,
                                html: item.Description,
                            })
                        );
                    });
                    Patient_Referrals_Incoming_Detail.IntializeMultiSelectDropDownStatusReason(PanelId);
                }
            });
        }
        else {
            $('#' + PanelId + ' #ddlStatusReasons').attr('disabled', true);
            $('#' + PanelId + " #ddlStatusReasons").val("");
        }
    },
    getStatusReasons_DBCall:function(StatusId){
        var objData = new Object();
        objData["Status"] = StatusId;
        objData["commandType"] = "get_status_reasons";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");
    },
    IntializeMultiSelectDropDownStatusReason: function (PanelId) {
        $('#' + PanelId + ' #ddlStatusReasons').multiselect('destroy');
        $('#' + PanelId + ' #ddlStatusReasons').multiselect({
            includeSelectAllOption: false,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            onChange: function (element, checked) {
            }
        });
        $('#' + PanelId + " #ddlStatusReasons").val("");
    },
    HideShowDateTimeOnDashboard: function (PanelId, obj) {
        var selectedStatusReasons = $('#' + PanelId + ' #ddlStatusReasons option:selected').text();
        if (selectedStatusReasons == 'Requested Follow Up' || selectedStatusReasons == 'Pending Carrier Approval' || selectedStatusReasons == 'Scheduled for appointment') {
            $('#' + PanelId + ' #StatusReasonDate').css('display', 'inline');
        }

        if (selectedStatusReasons == 'Scheduled for appointment') {
            $('#' + PanelId + ' #StatusReasonTime').css('display', 'inline');
        }
    },
}