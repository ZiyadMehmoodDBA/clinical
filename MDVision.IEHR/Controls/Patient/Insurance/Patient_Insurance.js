Patient_Insurance = {
    params: [],
    bIsFirstLoad: true,
    Enable: false,
    PlanResponse: '',
    isChange: false,
    isProcessed: '0',
    ProcessedData: null,
    InsuranceCardChanged: 'false',
    insurancePlanPriority: 0,
    InsAppointmentsArray: [],
    AppointmentDetails: "",
    InsurancePlanId: "",
    InsuranceActiveStatus: "",
    PatientSSN: '',
    IsLoadEligiblitySc: '',
    oldVisitCopayValue: '',
    oldSpecialistCopayValue: '',
    InsuranceSaveUpdateResponse: {},
    AppointmentDetailsForCopay: "",
    Load: function (parameters) {
        Patient_Insurance.params = parameters;
        Patient_Insurance.insurancePlanPriority = 0;
        //if (Patient_Insurance.params["PanelID"] != null && Patient_Insurance.params["PanelID"] != 'pnlPatientInsurance')
        //    Patient_Insurance.params["PanelID"] = Patient_Insurance.params["PanelID"] + ' #pnlPatientInsurance'
        //else
        //    Patient_Insurance.params["PanelID"] = 'pnlPatientInsurance';


        //--------- Banner Checks (IrFan)
        if (Patient_Insurance.params.PatBanner == true) {
            //$("#" + Patient_Insurance.params["PanelID"] + " #lnkOpen").addClass("disableAll"); 
            //$("#" + Patient_Insurance.params["PanelID"] + " #lnkCopyPatient").addClass("disableAll");
            //  $("#" + Patient_Insurance.params["PanelID"] + " #lnkAddInsurance").addClass("disableAll");
            //$("#" + Patient_Insurance.params["PanelID"] + " #lnkScan").addClass("disableAll");
        }
        if ($('#' + Patient_Insurance.params["PanelID"]).find('.modal-dialog-lg').length > 0)
            $('#' + Patient_Insurance.params["PanelID"]).find('.modal-dialog-lg:not(#SchedulingFormDiv)').removeClass('modal-dialog-lg').addClass('modal-dialog-full');
        //


        //
        //--------------------------
        if (Patient_Insurance.bIsFirstLoad) {
            if (Patient_Insurance.params["PanelID"] != "appointmentDetail")
                Patient_Insurance.bIsFirstLoad = false;
            $("#" + Patient_Insurance.params["PanelID"] + " #chkActive").prop('checked', true);
            $("#" + Patient_Insurance.params["PanelID"] + " #chkActive").attr("disabled", false);
            Patient_Insurance.ValidatePatientInsurance();
            Patient_Insurance.LoadAllAutocomplete();
            //if (globalAppdata['OCRLicenseKey'] == "") {
            //    $("#pnlPatientInsurance #lnkScan").removeClass("btn btn-primary").addClass("btn btn-primary disabled");
            //}
            //var self = $("#" + Patient_Insurance.params["PanelID"]);

            var self = "";
            if (Patient_Insurance.params["PanelID"] != "pnlPatientInsurance") {
                self = $("#" + Patient_Insurance.params["PanelID"] + " #pnlPatientInsurance");
            }
            else {
                self = $('#pnlPatientInsurance #frmPatientInsurance');
            }
            self.loadDropDowns(true).done(function () {
                //utility.ClearFormValidation('#pnlPatientInsurance #frmPatientInsurance');

                Patient_Insurance.LoadInsuranceList();
            });

        } else {
            Patient_Insurance.LoadInsuranceList();
        }
        if (params.PreviousTab != null && params.PreviousTab.TabID == "patTabDemographic") {
            Patient_Demographic.isChangeInDemographic(Patient_Demographic.params.mode);
        }


        //$('#pnlPatientInsurance #frmPatientInsurance').resetAllControls();
        //utility.ClearFormValidation('#pnlPatientInsurance #frmPatientInsurance');
        //if (Patient_Demographic.params["patientID"] != "") {
        //    Patient_Demographic.FillPatientInfo(Patient_Demographic.params).done(function (response) {



        //    });
        //}
        
        Patient_Insurance.InsuranceCardChanged = 'false';
        //Patient_Insurance.ValidatePatientInsurance();
        //Patient_Insurance.params.PatientInsuranceId = "4";
        //Patient_Insurance.LoadReferrals();
    },

    BindInsurancePlan: function () {


        var shortName = $("#" + Patient_Insurance.params["PanelID"] + " input#txtInsurancePlan").val();
        if (shortName) {
            Patient_Insurance.LoadGetInsurancePlan_DBCall(shortName).done(function (result) {
                var insurancePlans = [];
                $.each(result, function (i, item) {
                    if (item.Name.toUpperCase() != "- SELECT -")
                        insurancePlans.push({ id: item.Value, value: item.Name, searchPattern: item.RefValue, IPDescription: item.RefName, Isreferral: item.IsReferral }); //add item to an array
                });

                $("#" + Patient_Insurance.params["PanelID"] + " input#txtInsurancePlan").autocomplete({
                    autoFocus: true,
                    source: insurancePlans,
                    select: function (event, ui) {

                        setTimeout(function () {
                            Patient_Insurance.SelectInsurancePlan(ui);
                        }, 100);
                    }
                });

            });
        }

    },

    BindScannedInsurancePlan: function () {


        var shortName = $("#" + Patient_Insurance.params["PanelID"] + " input#txtInsurancePlan").val();
        if (shortName) {
            Patient_Insurance.LoadGetInsurancePlan_DBCall(shortName).done(function (result) {
                var uiObj = {};
                var insurancePlans = [];
                $.each(result, function (i, item) {
                    if (item.Name.toUpperCase() != "- SELECT -") {
                        insurancePlans.push({ id: item.Value, value: item.Name, searchPattern: item.RefValue, IPDescription: item.RefName, Isreferral: item.IsReferral });
                        uiObj = { item: { id: item.Value, value: item.Name, searchPattern: item.RefValue, IPDescription: item.RefName, Isreferral: item.IsReferral } };
                        return false;
                    }
                });

                if (uiObj.item && insurancePlans.length > 0) {
                    var $Ctrl = $("#" + Patient_Insurance.params["PanelID"] + " #txtInsurancePlan");
                    var $hfCtrl = $("#" + Patient_Insurance.params["PanelID"] + " #hfInsurancePlan");

                    $hfCtrl.val(uiObj.item.id);
                    utility.SetAutoCompleteSource($Ctrl, $hfCtrl, insurancePlans[0]);
                    Patient_Insurance.SelectInsurancePlan(uiObj);
                }

            });
        }
    },

    SelectInsurancePlan: function (ui) {
        $("#" + Patient_Insurance.params.PanelID + " #hfIsReferralRequired").val(ui.item.Isreferral);
        $("#" + Patient_Insurance.params["PanelID"] + " #hfInsurancePlan").val(ui.item.id); // add the selected id
        $("#" + Patient_Insurance.params["PanelID"] + " #hfInsurancePlansearchPattern").val(ui.item.searchPattern);
        if ($("#" + Patient_Insurance.params["PanelID"] + " #lnkInsurancePlanDetail").css("display") == "none") {
            $("#" + Patient_Insurance.params["PanelID"] + " #lnkInsurancePlanDetail").css("display", "inline");
            $("#" + Patient_Insurance.params["PanelID"] + " #lblInsurancePlan").css("display", "none");
        }
        Patient_Insurance.FillInsurancePlanAddress(ui.item.id);
        Patient_Insurance.setPlanToolTip(ui.item.IPDescription);
    },

    setPlanToolTip: function (value_) {
        //tooltip for selected insurance
        $("#" + Patient_Insurance.params["PanelID"] + " #txtInsurancePlan").attr("data-original-title", value_);
        $("#" + Patient_Insurance.params["PanelID"] + " #txtInsurancePlan").attr("data-toggle", "tooltip");

        $("#" + Patient_Insurance.params["PanelID"] + " #txtInsurancePlan").attr('data-placement', "top");
        $("#" + Patient_Insurance.params["PanelID"] + " #txtInsurancePlan").attr('data-originalval', value_);
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ contents: value_ });

    },

    LoadAllAutocomplete: function () {
        // Patient Employer
        var Ctrl = $("#" + Patient_Insurance.params["PanelID"] + " input#txtEmployer");
        var hfCtrl = $("#" + Patient_Insurance.params["PanelID"] + " #hfEmployer");
        var func = function () {
            return Patient_Insurance.GetEmployerArray(Ctrl.val())
        };
        utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", null, func, hfCtrl);

        // Patient Lawyer
        var $Ctrl = $("#" + Patient_Insurance.params["PanelID"] + " input#txtLawyer");
        var $hfCtrl = $("#" + Patient_Insurance.params["PanelID"] + " #hfLawyer");
        var func_lawyer = function () {
            return Patient_Insurance.GetLawyerArray($Ctrl.val())
        };
        utility.BindKendoAutoComplete($Ctrl, 1, "value", "contains", null, func_lawyer, $hfCtrl);
    },

    GetEmployerArray: function (name) {
        var AllGetEmployer = [];
        var dfd = new $.Deferred();
        if (name) {
            Patient_Insurance.LoadGetEmployer_DBCall(name).done(function (responseData) {
                if (responseData.length > 0) {
                    $.each(responseData, function (i, item) {
                        AllGetEmployer.push({ id: item.Value, value: item.Name });
                    });
                }

                dfd.resolve(AllGetEmployer);
            });
        }
        else {
            dfd.resolve(AllGetEmployer);
        }

        return dfd.promise();
    },

    LoadGetEmployer_DBCall: function (name) {
        return MDVisionService.PMSAPIService(name, "Patient", "GetPatientEmployer");
    },

    GetLawyerArray: function (name) {
        var AllGetLawyer = [];
        var dfd = new $.Deferred();
        if (name) {
            Patient_Insurance.LoadGetLawyer_DBCall(name).done(function (responseData) {
                if (responseData.length > 0) {
                    $.each(responseData, function (i, item) {
                        AllGetLawyer.push({ id: item.Value, value: item.Name });
                    });
                }

                dfd.resolve(AllGetLawyer);
            });
        }
        else {
            dfd.resolve(AllGetLawyer);
        }

        return dfd.promise();
    },

    LoadGetLawyer_DBCall: function (name) {
        return MDVisionService.PMSAPIService(name, "Patient", "GetPatientLawyer");
    },

    LoadGetInsurancePlan_DBCall: function (shortname) {
        return MDVisionService.PMSAPIService(shortname, "Patient", "GetInsurancePlan");
    },

    UploadCardImage: function () {
        var params = [];
        //params = Patient_Insurance.params
        var PanelID = null;
        if (Patient_Insurance.params["PanelID"] != "pnlPatientInsurance")
            PanelID = Patient_Insurance.params["PanelID"];
        if (Patient_Insurance.params.ParentCtrl == "demographicDetail" || Patient_Insurance.params.ParentCtrl == "EncounterChargeCapture" || Patient_Insurance.params.ParentCtrl == "appointmentDetail")
            params["ParentCtrl"] = 'Patient_Insurance';
        else
            params["ParentCtrl"] = 'patTabInsurance';
        params["FromAdmin"] = "0";
        //params["ParentCtrl"] = "patTabInsurance";
        LoadActionPan('uploadImage', params, PanelID);
    },
    setImageSource: function (sourceString) {
        $('#pnlPatientInsurance  #image').attr('src', sourceString);
        // $('#scanImage #image').css({ "cursor": "pointer" });
        Patient_Insurance.isProcessed = '1';
    },
    LoadInsuranceList: function (PatientInsuranceId) {
        Patient_Insurance.SearchPatientInsurance(Patient_Insurance.params.patientID, Patient_Insurance.params.PatientInsuranceId).done(function (response) {
            if (response.status != false) {

                Patient_Insurance.InsuranceListLoad(response);
                Patient_Insurance.params.PlanResponse = response;
            }
            else {
                Patient_Insurance.params.PlanResponse = '';
                utility.DisplayMessages(response.Message, 3);
            }
        });
        //Adnan Maqbool Dated Jan 12,2015 PMS-3276
        $("#pnlPatientInsurance #lstInsurances").sortable({
            cancel: '.unassigned',
            out: function (event, ui) {
                utility.myConfirm('5', function () {
                    var sortedIDs = $("#" + Patient_Insurance.params["PanelID"] + " #lstInsurances").sortable("toArray");

                    Patient_Insurance.UpdateInsurancePriority(sortedIDs).done(function (response) {
                        if (response.status != false) {
                            Patient_Insurance.AppointmentDetails = response.PatientFutureAppointment;
                            if (response.PatientFutureAppointment != null && response.PatientFutureAppointment.length > 0) {
                                Patient_Insurance.ShowAppointmentUpdateAlert();
                            }

                            $("#mainForm  li#CDSAlert").show();
                            if (!Patient_Insurance.params.ParentCtrl || Patient_Insurance.params.ParentCtrl != "appointmentDetail") {
                                $.when(setPatientBanner($("#hfPatientId").val(), "1")).then(function () {
                                    if (demographicDetail.params && demographicDetail.params.TabID == "clinicalTabProgressNote")
                                        Clinical_ProgressNote.LoadCDSAlerts();
                                });
                            }
                            if (parseInt($("#" + Patient_Insurance.params["PanelID"] + " #lstInsurances").find("li").length) > 0) {
                                if ($("#" + Patient_Insurance.params["PanelID"] + " #lstInsurances").find("li").first().attr("isactive") == "False") {
                                    $("#PatientProfile #PriInsurance").html("<strong>Pri. Ins:</strong> " + "SelfPay");
                                } else {
                                    $("#PatientProfile #PriInsurance").html("<strong>Pri. Ins:</strong> " + $("#" + Patient_Insurance.params["PanelID"] + " #lstInsurances").find("li").first().text());
                                }
                            }


                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });

                }, function () {

                    $(ui.sender).sortable('cancel');
                },
                             '5'
              );
            },
        });
        $("#lstInsurances").disableSelection();
        //
        $("#" + Patient_Insurance.params["PanelID"] + " #lstInsurances").on("sortupdate", function (event, ui) {


        });
        //end Dated Jan 12,2015 PMS-3276
    },

    InsuranceListLoad: function (response) {
        $("#" + Patient_Insurance.params["PanelID"] + " #lstInsurances").empty();
        $("#" + Patient_Insurance.params["PanelID"] + " #lstInsurancePriority").empty();

        if (response.PatientInsuranceCount > 0) {
            //$("#" + Patient_Insurance.params["PanelID"] + " #hfInsurancePlanPriority").val(response.PatientInsuranceCount + 1);
            Patient_Insurance.insurancePlanPriority = response.PatientInsuranceCount;
            var InsuranceLoadJSONData = JSON.parse(response.PatientInsuranceLoad_JSON);

            var FirstInsuranceId = "";
            $.each(InsuranceLoadJSONData, function (i, item) {
                if (FirstInsuranceId == "") {// Set First Patient InsuraceId
                    FirstInsuranceId = item.InsuranceId;
                }
                if (Patient_Insurance.params.PatientInsuranceId != '' && typeof Patient_Insurance.params.PatientInsuranceId != 'undefined' && item.InsuranceId == Patient_Insurance.params.PatientInsuranceId) {
                    $("#" + Patient_Insurance.params.PanelID + " #hfIsReferralRequired").val(item.isreferral);
                }

                //var bgcolor = "";
                var lstInsurance = "";
                if (item.IsActive != "True" && item.UnAssigned != 'True') {
                    lstInsurance = '<li id="' + item.InsuranceId + '" isactive="' + item.IsActive + '" class="ui-state-default bg-danger unassigned"><a href="#" onclick="Patient_Insurance.PatientInsuranceLoad(' + item.InsuranceId + ');" class="unassigned"><p class="white ellipses unassigned">' + item.InsurancePlanName + '</p></a></li>';
                }
                else if (item.IsActive != "True" && item.UnAssigned == 'True') {
                    lstInsurance = '<li id="' + item.InsuranceId + '" isactive="' + item.IsActive + '" class="ui-state-default unassigned" style="background: #f9b758;color: white;"><a href="#" onclick="Patient_Insurance.PatientInsuranceLoad(' + item.InsuranceId + ');" class="unassigned"><p class="white ellipses unassigned">' + item.InsurancePlanName + '</p></a></li>';
                }
                else {
                    lstInsurance = '<li id="' + item.InsuranceId + '" isactive="' + item.IsActive + '" class="ui-state-default"><a href="#" onclick="Patient_Insurance.PatientInsuranceLoad(' + item.InsuranceId + ');"><p class="white ellipses">' + item.InsurancePlanName + '</p></a></li>';
                }

                var lstPriority = "";
                if (item.IsActive != "True" || item.UnAssigned == 'True') {
                    lstPriority = '<li><b style="margin-left:4px;color:red">X</b></li>';
                }
                else {
                    lstPriority = '<li>' + (i + 1) + '</li>';
                }
                //lstInsurance.append(item.LawyerId);
                if (i == 0) {

                    //if first insurance is inactive
                    if (item.IsActive == "True") {
                        $("#PatientProfile #PriInsurance").html("<strong>Pri. Ins:</strong> " + item.InsurancePlanName);

                    } else {
                        $("#PatientProfile #PriInsurance").html("<strong>Pri. Ins:</strong> " + "SelfPay");

                    }
                    //update banner with item.InsurancePlanName

                }
                $("#" + Patient_Insurance.params["PanelID"] + " #lstInsurances").last().append(lstInsurance);
                $("#" + Patient_Insurance.params["PanelID"] + " #lstInsurancePriority").last().append(lstPriority);
            });
            if (FirstInsuranceId != "") {//Load First Patient Insurance
                if (Patient_Insurance.params.PatientInsuranceId != '' && typeof Patient_Insurance.params.PatientInsuranceId != 'undefined') {
                    Patient_Insurance.PatientInsuranceLoad(Patient_Insurance.params.PatientInsuranceId, JSON.parse(response.PatientInsuranceFill_JSON));

                } else {

                    Patient_Insurance.PatientInsuranceLoad(FirstInsuranceId, JSON.parse(response.PatientInsuranceFill_JSON));
                }
            }
            $("#" + Patient_Insurance.params["PanelID"] + " #divPatientReferrals").css("display", "inline");
            Patient_Insurance.params["mode"] = "Edit";
        }
        else {
            $("#" + Patient_Insurance.params["PanelID"] + " #divPatientReferrals").css("display", "none");
            $('#pnlPatientInsurance #frmPatientInsurance #chkAssgBenefits,#chkActive').prop("checked", "checked");
            Patient_Insurance.params["mode"] = "Add";
            $("#hfInsurancePlanPriority").val("1");
            $("#" + Patient_Insurance.params["PanelID"] + " #lnkInsurancePlanDetail").css("display", "none");
            $("#" + Patient_Insurance.params["PanelID"] + " #lblInsurancePlan").css("display", "inline");

            $("#" + Patient_Insurance.params["PanelID"] + " #lnkLawyerDetail").css("display", "none");
            $("#" + Patient_Insurance.params["PanelID"] + " #lblLawyer").css("display", "inline");
            $("#" + Patient_Insurance.params["PanelID"] + " #lnkEmployerDetail").css("display", "none");
            $("#" + Patient_Insurance.params["PanelID"] + " #lblEmployer").css("display", "inline");
            //serialize data
            $('#frmPatientInsurance').data('serialize', $('#frmPatientInsurance').serialize());
            try {
                if (Patient_Insurance.params["PanelID"] && Patient_Insurance.params["PanelID"].indexOf("pnlPatientInsurance") > -1 && $("#" + Patient_Insurance.params["PanelID"]).is(":visible") == true) {
                    // serialize all data in global variable when form is  loaded
                    Patient_Insurance.AddInsurancePlan(true);
                    $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance  #hfImageChange').val("false");
                    //$("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance").trigger('reset');
                    params.defaultDemographicSerailizeForm = $('#frmPatientInsurance').serialize();
                    params.IsDemographicInfoGlobalyUpdated = false;
                    params.DemographicAutoUpdateActiveTab = "Insurance";
                    $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance").data('bootstrapValidator').resetForm();

                }
                else if ($("#pnlPatientInsurance #frmPatientInsurance").length > 0 && $("#pnlPatientInsurance #frmPatientInsurance").is(":visible") == true) {
                    // serialize all data in global variable when form is  loaded
                    $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance").trigger('reset');
                    $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance  #hfImageChange').val("false");
                    params.defaultDemographicSerailizeForm = $('#frmPatientInsurance').serialize();
                    params.IsDemographicInfoGlobalyUpdated = false;
                    params.DemographicAutoUpdateActiveTab = "Insurance";
                    $("#pnlPatientInsurance #frmPatientInsurance").data('bootstrapValidator').resetForm();

                }
            }
            catch (e) { }
            Patient_Insurance.oldSpecialistCopayValue = $('#' + Patient_Insurance.params["PanelID"] + ' #txtSpecialistCopay').val();
            Patient_Insurance.oldVisitCopayValue = $('#' + Patient_Insurance.params["PanelID"] + ' #txtVisitCopayment').val();
        }

    },

    PatientInsuranceLoad: function (PatientInsuranceId, data) {
        AppPrivileges.GetFormPrivileges("Patient Insurance", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                $("#" + Patient_Insurance.params["PanelID"] + " #chkActive").attr("disabled", false);
                var totalInsurances = $("#lstInsurances li").length;
                $("#lstInsurances li").each(function () {
                    if (totalInsurances == 1) {
                        if ($(this).hasClass("bg-danger")) {
                            return;
                        }
                    }
                    else {
                        if ($(this).attr("id") == PatientInsuranceId)
                            $(this).addClass("active-plan");
                        else
                            $(this).removeClass("active-plan");
                    }

                });

                utility.ClearFormValidation('#pnlPatientInsurance #frmPatientInsurance');
                if (data != null) {
                    Patient_Insurance.FillInsurance(PatientInsuranceId, data);
                }
                else {
                    Patient_Insurance.FillPatientInsurance(PatientInsuranceId).done(function (response) {
                        Patient_Insurance.FillInsurance(PatientInsuranceId, response);
                    });
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);

        });
    },

    OpenScannedImage: function () {
        var strMessage = "";
        //if (event != null) {
        //    event.stopPropagation();
        //}
        //utility.SelectGridRow($("dgvPatientDocument_row" + PatDocID));
        AppPrivileges.GetFormPrivileges("Documents", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlPatientInsurance #hfPatDocId").val()) {
                    var params = [];
                    params["PatientID"] = Patient_Insurance.params.patientID;
                    params["PatDocID"] = $("#pnlPatientInsurance #hfPatDocId").val();
                    params["mode"] = "Edit";
                    params["RefDemographic"] = "View_Only";
                    params["FromAdmin"] = Patient_Insurance.params["FromAdmin"];
                    if (Patient_Insurance.params["FromAdmin"] == "0") {
                        params["ParentCtrl"] = Patient_Insurance.params["TabID"];
                    }
                    LoadActionPan('Document_Viewer', params);
                }

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    FillInsurance: function (PatientInsuranceId, response) {
        if (response.status != false) {
            var isnurance_detail = JSON.parse(response.InsuranceFill_JSON);
            $("#" + Patient_Insurance.params.PanelID + " #hfIsReferralRequired").val(isnurance_detail.IsReferral);
            if (isnurance_detail.chkUnassigned == "True") {
                $("#" + Patient_Insurance.params["PanelID"] + " #chkActive").attr("disabled", true);
            }
            else {
                $("#" + Patient_Insurance.params["PanelID"] + " #chkActive").attr("disabled", false);
            }

            $("#" + Patient_Insurance.params["PanelID"] + " #hfPriority").val(isnurance_detail.Priority);
            $("#" + Patient_Insurance.params["PanelID"] + " #hfClaimFlagDescription").val(isnurance_detail.ClaimFlagDescription);

            //adnan maqbool, pms-3991, 17-02-2016
            var self = "";
            if (Patient_Insurance.params["PanelID"] != "pnlPatientInsurance") {
                self = $("#" + Patient_Insurance.params["PanelID"] + " #pnlPatientInsurance");
            }
            else {
                self = $('#pnlPatientInsurance #frmPatientInsurance');
            }
            //end
            // Patient_Insurance.ScanPrivilige = isnurance_detail.Scan;
            // Patient_Insurance.OCRPrivilige = isnurance_detail.OCR;

            $.when(Patient_Insurance.FillInsurancePlanAddress(isnurance_detail.hfInsurancePlan)).then(function () {
                utility.bindMyJSON(true, isnurance_detail, false, self).done(function () {
                    setTimeout(function () {

                        Patient_Insurance.InsurancePlanId = $("#" + Patient_Insurance.params["PanelID"] + " #hfInsurancePlan").val();
                        Patient_Insurance.InsuranceActiveStatus = $("#" + Patient_Insurance.params["PanelID"] + " #chkActive").prop('checked')
                        Patient_Insurance.ValidateSearchPatternForSelectedInsurancePlan();

                        $Ctrl = $("#" + Patient_Insurance.params.PanelID + " #frmPatientInsurance #txtInsurancePlan");
                        $hfCtrl = $("#" + Patient_Insurance.params.PanelID + " #frmPatientInsurance #hfInsurancePlan");
                        //Insurance Plan
                        utility.SetAutoCompleteSource($Ctrl, $hfCtrl);

                        // Begin 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3407
                        Patient_Insurance.params["mode"] = "Edit";
                        //  $("#" + Patient_Insurance.params["PanelID"] + ' #ddlPlanAddress option[value="' + isnurance_detail.ddlPlanAddress + '"]').attr("selected", "selected"); 
                        var AddressCount = $('#pnlPatientInsurance #ddlPlanAddress option').length;
                        //if single address is there, then should be selected by default
                        if (isnurance_detail.ddlPlanAddress != "") {
                            $("#" + Patient_Insurance.params["PanelID"] + ' #ddlPlanAddress option[value="' + isnurance_detail.ddlPlanAddress + '"]').attr("selected", "selected");
                        } else if (AddressCount == 2) {
                            $('#pnlPatientInsurance #ddlPlanAddress option:nth-child(2)').prop("selected", true);
                            $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance').bootstrapValidator('revalidateField', 'PlanAddress');
                        }
                        // End 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3407
                        if (isnurance_detail.txtInsurancePlan != "") {
                            $("#" + Patient_Insurance.params["PanelID"] + " #lnkInsurancePlanDetail").css("display", "inline");
                            $("#" + Patient_Insurance.params["PanelID"] + " #lblInsurancePlan").css("display", "none");
                            Patient_Insurance.setPlanToolTip(isnurance_detail.IPDescription);

                        } else {
                            $("#" + Patient_Insurance.params["PanelID"] + " #lnkInsurancePlanDetail").css("display", "none");
                            $("#" + Patient_Insurance.params["PanelID"] + " #lblInsurancePlan").css("display", "inline");
                        }
                        if (isnurance_detail.txtLawyer != "") {
                            $("#" + Patient_Insurance.params["PanelID"] + " #lnkLawyerDetail").css("display", "inline");
                            $("#" + Patient_Insurance.params["PanelID"] + " #lblLawyer").css("display", "none");
                        } else {
                            $("#" + Patient_Insurance.params["PanelID"] + " #lnkLawyerDetail").css("display", "none");
                            $("#" + Patient_Insurance.params["PanelID"] + " #lblLawyer").css("display", "inline");
                        }
                        if (isnurance_detail.txtEmployer != "") {
                            $("#" + Patient_Insurance.params["PanelID"] + " #lnkEmployerDetail").css("display", "inline");
                            $("#" + Patient_Insurance.params["PanelID"] + " #lblEmployer").css("display", "none");
                        } else {
                            $("#" + Patient_Insurance.params["PanelID"] + " #lnkEmployerDetail").css("display", "none");
                            $("#" + Patient_Insurance.params["PanelID"] + " #lblEmployer").css("display", "inline");
                        }

                        Patient_Insurance.EnableValidation($("#" + Patient_Insurance.params["PanelID"] + " #ddlRelation"), false);
                        if (isnurance_detail.txtSSN) {
                            Patient_Insurance.PatientSSN = isnurance_detail.txtSSN;
                            if (globalAppdata.IsFullSSN.toLowerCase() === 'true') {

                                $('#pnlPatientInsurance #frmPatientInsurance #txtSSNIns').attr("placeholder", "999-99-9999");
                                $('#pnlPatientInsurance #frmPatientInsurance #txtSSNIns').attr("data-mask", "999-99-9999");
                                $('#pnlPatientInsurance #frmPatientInsurance #txtSSNIns').val(isnurance_detail.txtSSN);
                                $('#pnlPatientInsurance #frmPatientInsurance #txtSSNIns').attr("disabled", false);
                            }
                            else {
                                $('#pnlPatientInsurance #frmPatientInsurance #txtSSNIns').attr("placeholder", "XXX-XX-9999");
                                $('#pnlPatientInsurance #frmPatientInsurance #txtSSNIns').attr("data-mask", "XXX-XX-9999");
                                var lastFourDigit = isnurance_detail.txtSSN.slice(-4);
                                var formatSSN = "XXX-XX-" + lastFourDigit;
                                $('#pnlPatientInsurance #frmPatientInsurance #txtSSNIns').val(formatSSN);
                                $('#pnlPatientInsurance #frmPatientInsurance #txtSSNIns').attr("disabled", true);
                            }
                        }
                        else {
                            Patient_Insurance.PatientSSN = "";
                            $('#pnlDemographic #frmDemographic #txtSSNIns').attr("placeholder", "999-99-9999");
                            $('#pnlDemographic #frmDemographic #txtSSNIns').attr("data-mask", "999-99-9999");
                            $('#pnlDemographic #frmDemographic #txtSSNIns').val(Patient_Insurance.PatientSSN);
                            $('#pnlDemographic #frmDemographic #txtSSNIns').attr("disabled", false);
                        }
                        //serialize data
                        $('#frmPatientInsurance').data('serialize', $('#frmPatientInsurance').serialize());
                        if ($("#" + Patient_Insurance.params.PanelID + " #ddlRelation option:selected").text().toLowerCase() != "self") {
                            $("#" + Patient_Insurance.params.PanelID + " #PatAddreeslnk").show();
                        }
                        else {
                            $("#" + Patient_Insurance.params.PanelID + " #PatAddreeslnk").hide();
                        }
                        try {
                            if (Patient_Insurance.params["PanelID"] && Patient_Insurance.params["PanelID"].indexOf("pnlPatientInsurance") > -1 && $("#" + Patient_Insurance.params["PanelID"]).is(":visible") == true) {
                                // serialize all data in global variable when form is  loaded
                                params.defaultDemographicSerailizeForm = $('#frmPatientInsurance').serialize();
                                params.IsDemographicInfoGlobalyUpdated = false;
                                params.DemographicAutoUpdateActiveTab = "Insurance";
                                $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance").data('bootstrapValidator').resetForm();
                                $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance  #hfImageChange').val("false");
                            }
                            else if ($("#pnlPatientInsurance #frmPatientInsurance").length > 0 && $("#pnlPatientInsurance #frmPatientInsurance").is(":visible") == true) {
                                // serialize all data in global variable when form is  loaded
                                params.defaultDemographicSerailizeForm = $('#frmPatientInsurance').serialize();
                                params.IsDemographicInfoGlobalyUpdated = false;
                                params.DemographicAutoUpdateActiveTab = "Insurance";
                                $("#pnlPatientInsurance #frmPatientInsurance").data('bootstrapValidator').resetForm();
                                $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance  #hfImageChange').val("false");
                            }
                        }
                        catch (e) { }
                        Patient_Insurance.oldSpecialistCopayValue = $('#' + Patient_Insurance.params["PanelID"] + ' #txtSpecialistCopay').val();
                        Patient_Insurance.oldVisitCopayValue = $('#' + Patient_Insurance.params["PanelID"] + ' #txtVisitCopayment').val();
                    }, 200)

                });

                //$('#pnlPatientInsurance #ddlPlanAddress').val(isnurance_detail.ddlPlanAddress);
            });
            //$.when().done(function () {

            //});

            utility.RemoveTimeFromDate("#pnlPatientInsurance #dtpDOB", $("#pnlPatientInsurance #dtpDOB").val());
            Patient_Insurance.params.PatientInsuranceId = PatientInsuranceId;
            if (isnurance_detail.chkActive == 'True')
                $("#pnlPatientInsurance #chkActive").attr("checked", true);
            else
                $("#pnlPatientInsurance #chkActive").attr("checked", false);
            if (isnurance_detail.chkAssgBenefits == 'True')
                $("#pnlPatientInsurance #chkAssgBenefits").attr("checked", true);
            else
                $("#pnlPatientInsurance #chkAssgBenefits").attr("checked", false);
            if ($("#" + Patient_Insurance.params.PanelID + " #hfInsurancePlan").val() != "") {
                $("#pnlPatientInsurance #lnkInsurancePlanDetail").css("display", "inline");
                $("#pnlPatientInsurance #lblInsurancePlan").css("display", "none");
            }

            if ($("#pnlPatientInsurance #hfEmployer").val() != "") {
                $("#pnlPatientInsurance #lnkEmployerDetail").css("display", "inline");
                $("#pnlPatientInsurance #lblEmployer").css("display", "none");
            }
            if ($("#pnlPatientInsurance #hfLawyer").val() != "") {
                $("#pnlPatientInsurance #lnkLawyerDetail").css("display", "inline");
                $("#pnlPatientInsurance #lblLawyer").css("display", "none");
            }
            // alert(isnurance_detail.IsFileStream);
            if (isnurance_detail.FileStream != "") {

                //Start 11/02/2016 Muhammad Irfan for bug #  PMS-3897
                $("#" + Patient_Insurance.params["PanelID"] + ' #scanImage').empty();
                $("#" + Patient_Insurance.params["PanelID"] + " #btnRemoveInsurance").show();
                $("#" + Patient_Insurance.params["PanelID"] + ' #scanImage').append("<img class=\"img-responsive img-center insurance-img-box\" id='image' onclick='Patient_Insurance.OpenScannedImage();' src=\"data:image/jpg;base64," + JSON.parse(response.InsuranceFill_JSON).FileStream + "\">");
                //   $("#" + Patient_Insurance.params["PanelID"] + ' #scanImage').append("<img class=\"img-responsive img-center insurance-img-box\" id='image' onclick='Patient_Insurance.OpenScannedImage();' src='" + slicedImage + "'/>");
                //   $("#pnlPatientInsurance #image").attr('src', "data:image/jpg;base64," + JSON.parse(response.InsuranceFill_JSON).FileStream);
                $("#" + Patient_Insurance.params["PanelID"] + " #backimage").hide();
                $('#scanImage #image').css({ "cursor": "pointer" });
                //End 11/02/2016 Muhammad Irfan for bug #  PMS-3897

            }
            else {
                //$('#scanImage img').attr('src', 'Content/images/idcard1.png');
                //$('#scanBackImage img').attr('src', 'Content/images/idcard2.png');
                //$("#pnlPatientInsurance #image").attr('src', "Content/images/idcard1.png");
                //$("#pnlPatientInsurance #backimage").attr('src', "Content/images/idcard2.png");
                //$('#scanImage').empty();
                //$("#pnlPatientInsurance #image").hide();
                $('#scanImage #image').attr("src", "Content/images/idcard1.png");
                $('#scanImage #image').css({ "cursor": "default" });
                $("#pnlPatientInsurance #backimage").hide();
                $("#pnlPatientInsurance #btnRemoveInsurance").hide();
                $("#pnlPatientInsurance #btnRemoveInsurance").hide();

            }
            Patient_Insurance.LoadReferrals();
            $("#" + Patient_Insurance.params["PanelID"] + " #divPatientReferrals").css("display", "inline");
            Patient_Insurance.params.mode == "Edit";
            //Patient_Insurance.ValidatePatientInsurance();

            //Patient_Insurance.EnableValidation($("#" + Patient_Insurance.params["PanelID"] + " #ddlRelation"), false);
            //serialize data

            $('#frmPatientInsurance').data('serialize', $('#frmPatientInsurance').serialize());
            try {
                if (Patient_Insurance.params["PanelID"] && Patient_Insurance.params["PanelID"].indexOf("pnlPatientInsurance") > -1 && $("#" + Patient_Insurance.params["PanelID"]).is(":visible") == true) {
                    // serialize all data in global variable when form is  loaded
                    params.defaultDemographicSerailizeForm = $('#frmPatientInsurance').serialize();
                    params.IsDemographicInfoGlobalyUpdated = false;
                    params.DemographicAutoUpdateActiveTab = "Insurance";
                    $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance").data('bootstrapValidator').resetForm();
                    $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance  #hfImageChange').val("false");
                }
                else if ($("#pnlPatientInsurance #frmPatientInsurance").length > 0 && $("#pnlPatientInsurance #frmPatientInsurance").is(":visible") == true) {
                    // serialize all data in global variable when form is  loaded
                    params.defaultDemographicSerailizeForm = $('#frmPatientInsurance').serialize();
                    params.IsDemographicInfoGlobalyUpdated = false;
                    params.DemographicAutoUpdateActiveTab = "Insurance";
                    $("#pnlPatientInsurance #frmPatientInsurance").data('bootstrapValidator').resetForm();
                    $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance  #hfImageChange').val("false");
                }
            } catch (e) { }
        }
        else {
            utility.DisplayMessages(response.Message, 3);
        }

    },
    AddInsurancePlan: function (isFromScan) {
        Patient_Insurance.HideEmployerLink();
        Patient_Insurance.HideLawyerLink();
        Patient_Insurance.HideInsurancePlanLink();
        if (isFromScan != null && isFromScan) {
            $("#pnlPatientInsurance #image").attr('src', "Content/images/idcard1.png");
            $('#scanImage #image').css({ "cursor": "default" });
            $("#pnlPatientInsurance #backimage").show();
        }

        //$('#scanImage').empty();
        $("#pnlPatientInsurance #backimage").hide();
        $("#pnlPatientInsurance #btnRemoveInsurance").hide();
        $("#" + Patient_Insurance.params["PanelID"] + " #divPatientReferrals").css("display", "none");
        Patient_Insurance.params["mode"] = "Add";
        //$('#pnlPatientInsurance #frmPatientInsurance').resetAllControls();
        $('#pnlPatientInsurance #frmPatientInsurance').find('input').filter(function () {
            return $.data($(this).get(0), 'datepicker') != null
        }).each(function () {
            //  $(this).datepicker("setDate", new Date());
            $(this).val(new Date()).datepicker('update');
        });
        utility.ClearFormValidation('#pnlPatientInsurance #frmPatientInsurance', true);
        $('#pnlPatientInsurance #frmPatientInsurance').resetAllControls();
        // Begin 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3407
        $("#lstInsurances li").each(function () {
            $(this).removeClass("active-plan");
        });
        // End 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3407
        $('#pnlPatientInsurance #frmPatientInsurance #chkAssgBenefits,#chkActive').prop("checked", "checked");
        //$('#pnlPatientInsurance #hfInsurancePlanPriority').val($("#pnlPatientInsurance #lstInsurancePriority li").size() + 1);
        $('#pnlPatientInsurance #hfInsurancePlanPriority').val(Patient_Insurance.insurancePlanPriority + 1);
        $("#" + Patient_Insurance.params["PanelID"] + " #txtInsurancePlan").removeAttr("data-original-title");

        Patient_Insurance.PatientSSN = "";
        $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtSSNIns").attr("placeholder", "999-99-9999");
        $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtSSNIns").attr("data-mask", "999-99-9999");
        $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtSSNIns").attr("disabled", false);
    },

    // -------------- Patint Referral ---------------------
    LoadReferrals: function (PrimaryID, PageNumber, ResultPerPage) {
        if ($("#" + Patient_Insurance.params["PanelID"] + " #pnlReferral_Result").css("display") == "none") {
            $("#" + Patient_Insurance.params["PanelID"] + " #pnlReferral_Result").show();
        }

        Patient_Insurance.SearchReferral(PrimaryID, PageNumber, ResultPerPage).done(function (response) {
            if (response.status != false) {
                Patient_Insurance.ReferralGridLoad(response);
                var TableControl = $("#" + Patient_Insurance.params["PanelID"] + " #dgvReferral");
                var PagingPanelControlID = Patient_Insurance.params.PanelID + " #dgvReferral_Paging";
                var ClassControlName = "Patient_Insurance";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(
                    CreatePagination(response.ReferralCount, PageNumber, ResultPerPage, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        Patient_Insurance.LoadReferrals(PrimaryID, PageNumber, ResultPerPage);
                    }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    ReferralGridLoad: function (response) {
        if ($.fn.dataTable.isDataTable("#" + Patient_Insurance.params["PanelID"] + " #dgvReferral"))
            $("#" + Patient_Insurance.params["PanelID"] + " #dgvReferral").dataTable().fnDestroy();
        $("#" + Patient_Insurance.params["PanelID"] + " #dgvReferral tbody").find("tr").remove();
        if (response.ReferralCount > 0) {
            var ReferralLoadJSONData = JSON.parse(response.ReferralLoad_JSON);
            $.each(ReferralLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvReferral_row" + item.PatientReferralId + "'))");
                $row.attr("id", "gvReferral_row" + item.PatientReferralId);
                $row.attr("ReferralId", item.PatientReferralId);

                if (item.IsActive == "True") {
                    isactive = 0;
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    tglclass = "fa fa-toggle-on red";
                }
                $row.append('<td style="display:none;">' + item.PatientReferralId + '</td><td><a class="btn btn-xs" href="#" onclick="Patient_Insurance.ReferralDelete(' + item.PatientReferralId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Patient_Insurance.ReferralEdit(' + item.PatientReferralId + ');" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Patient_Insurance.ReferralActiveInactive(' + item.PatientReferralId + ', ' + isactive + ');" title="Inactive Record"><i class="' + tglclass + '"></i></a></td><td>' + item.ReferralType + '</td><td>' + item.ReferringFromName + '</td><td>' + item.FromDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + item.ToDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + item.ReferralAuthNo + '</td><td>' + item.VisitsAllowed + '</td><td>' + item.VisitsUsed + '</td>');

                $("#" + Patient_Insurance.params["PanelID"] + " #pnlReferral_Result #dgvReferral tbody").last().append($row);
            });
        }
        else {
            if (!$("#" + Patient_Insurance.params["PanelID"] + " #dgvReferral").parent().parent().hasClass("dataTables_wrapper")) {
                $("#" + Patient_Insurance.params["PanelID"] + " #dgvReferral").DataTable({
                    "language": {
                        "emptyTable": "No Referral Found"
                    }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bPaginate": false, "bSortable": false, "aTargets": [1] }]
                });
            }
            else {
                var $row = $('<tr/>');
                $row.append('<td colspan="9" class="center" > No Referral Found </td>');
                $("#" + Patient_Insurance.params["PanelID"] + " #dgvReferral tbody").last().append($row);
            }
        }
        if ($.fn.dataTable.isDataTable("#" + Patient_Insurance.params["PanelID"] + " #dgvReferral") || $("#" + Patient_Insurance.params["PanelID"] + " #dgvReferral").parent().parent().hasClass("dataTables_wrapper"))
            ;
        else {
            $("#" + Patient_Insurance.params["PanelID"] + " #dgvReferral").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "bPaginate": false, "aTargets": [1] }] });
        }
    },

    AddReferral: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Referral Management", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                var PanelID = null;
                if (Patient_Insurance.params["PanelID"] != "pnlPatientInsurance")
                    PanelID = Patient_Insurance.params["PanelID"];
                if (Patient_Insurance.params.ParentCtrl == "demographicDetail" || Patient_Insurance.params.ParentCtrl == "EncounterChargeCapture" || Patient_Insurance.params.ParentCtrl == "appointmentDetail")
                    params["ParentCtrl"] = 'Patient_Insurance';
                else
                    params["ParentCtrl"] = 'patTabInsurance';
                params["ReferralID"] = "-1";
                params["mode"] = "Add";
                params["patientID"] = Patient_Insurance.params.patientID;
                params["PatientInsuranceId"] = Patient_Insurance.params.PatientInsuranceId;
                LoadActionPan('Patient_Referral', params, PanelID);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ReferralEdit: function (ReferralId) {
        var strMessage = "";
        utility.SelectGridRow($("#gvReferral_row" + ReferralId));
        AppPrivileges.GetFormPrivileges("Referral Management", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = ReferralId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    var PanelID = null;
                    if (Patient_Insurance.params["PanelID"] != "pnlPatientInsurance")
                        PanelID = Patient_Insurance.params["PanelID"];
                    if (Patient_Insurance.params.ParentCtrl == "demographicDetail" || Patient_Insurance.params.ParentCtrl == "En4counterChargeCapture" || Patient_Insurance.params.ParentCtrl == "appointmentDetail")
                        params["ParentCtrl"] = 'Patient_Insurance';
                    else
                        params["ParentCtrl"] = 'patTabInsurance';
                    params["ReferralID"] = selectedValue;
                    params["patientID"] = Patient_Insurance.params.patientID;
                    params["mode"] = "Edit";
                    params["PatientInsuranceId"] = Patient_Insurance.params.PatientInsuranceId;;
                    LoadActionPan('Patient_Referral', params, PanelID);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    UnassignedInsChange: function () {
        var UnsignedInsuranceId = $("#" + Patient_Insurance.params["PanelID"] + " #hfInsurancePlan").val();
        if ($("#" + Patient_Insurance.params["PanelID"] + " #chkUnassigned").prop('checked') == true) {
            utility.myConfirm('The insurance will not fall under Primary, Secondary or Tertiary sequence. This insurance information will not be used during claims generation. Do you want to proceed? ', function () {
                $("#" + Patient_Insurance.params["PanelID"] + " #chkActive").prop('checked', false);
                $("#" + Patient_Insurance.params["PanelID"] + " #chkActive").attr("disabled", true);

            }, function () {
                $("#" + Patient_Insurance.params["PanelID"] + " #chkUnassigned").prop('checked', false);
                $("#" + Patient_Insurance.params["PanelID"] + " #chkActive").attr("disabled", false);
            },
                       'Unassigned confirmation'
                   );
        }
        else {
            utility.myConfirm('This insurance information can be used during claims generation. Do you want to proceed? ', function () {
                $("#" + Patient_Insurance.params["PanelID"] + " #chkActive").prop('checked', true);
                $("#" + Patient_Insurance.params["PanelID"] + " #chkActive").attr("disabled", false);
            }, function () {

                $("#" + Patient_Insurance.params["PanelID"] + " #chkUnassigned").prop('checked', true);

            },
                     'Unassigned confirmation'
            );
        }
    },

    ReferralDelete: function (ReferralId) {
        var strMessage = "";
        utility.SelectGridRow($("#gvReferral_row" + ReferralId));
        AppPrivileges.GetFormPrivileges("Referral Management", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = ReferralId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Patient_Insurance.DeleteReferral(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $("#" + Patient_Insurance.params["PanelID"] + " #dgvReferral").DataTable();
                                table1.row('.active').remove().draw(false);
                                utility.DisplayMessages(response.Message, 1);
                                Patient_Insurance.LoadReferrals();
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
        });
    },

    ReferralActiveInactive: function (ReferralId, IsActive) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Referral Management", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = ReferralId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Patient_Insurance.UpdateReferralActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Patient_Insurance.LoadReferrals('0');
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
    // -------------- End Patient Referral -----------------

    ValidatePatientInsurance: function () {

        $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {


                  InsurancePlan: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          }
                      }
                  },
                  SubscriberID: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          }
                      }
                  },

                  Relation: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          }
                      }
                  },
                  LastName: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          }
                      }
                  },
                  FirstName: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          }
                      }
                  },
                  DOB: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          }
                      }
                  },
                  Sex: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          }
                      }
                  },
                  PlanAddress: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          }
                      }
                  },

                  Address1: {
                      group: '.col-sm-6',
                      enabled: false,
                      validators: {
                          notEmpty: {
                              message: ''
                          }
                      }
                  },
                  City: {
                      group: '.col-xs-8',
                      enabled: false,
                      validators: {
                          notEmpty: {
                              message: ''
                          }
                      }
                  },
                  State: {
                      group: '.col-xs-4',
                      enabled: false,
                      validators: {
                          notEmpty: {
                              message: ''
                          }
                      }
                  },
                  Zip: {
                      group: '.col-xs-8',
                      enabled: false,
                      validators: {
                          zipCode: {
                              country: 'US',
                              message: ' '
                          },
                          notEmpty: {
                              message: ''
                          }
                      }
                  }
              }
          })
            .on('error.form.bv', function (e, data) {
                // if auto save submit request fail
                if (params.IsDemographicInfoGlobalyUpdated) {
                    params.IsDemographicInfoGlobalyUpdated = false;
                    params.DemographicAutoUpdateActiveTab = "Insurance";
                    if (drfdAutoSave) drfdAutoSave.reject();
                }
            })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           // validate when auto save submit data call run
           if (params.IsDemographicInfoGlobalyUpdated) {

               if ($('#' + Patient_Insurance.params.PanelID + ' #hfInsurancePlansearchPattern').val() != '') {
                   Patient_Insurance.InusrancePlanNameValidation();
                   if (drfdAutoSave) drfdAutoSave.reject();
                   params.IsDemographicInfoGlobalyUpdated = false;
                   params.DemographicAutoUpdateActiveTab = "Insurance";
               }
               else { Patient_Insurance.PatientInsuranceSave(); }

           }
           else {
               if ($('#' + Patient_Insurance.params.PanelID + ' #hfInsurancePlansearchPattern').val() == '') {
                   Patient_Insurance.PatientInsuranceSave();
               } else {
                   Patient_Insurance.InusrancePlanNameValidation();
                   // Patient_Insurance.ValidateSubScriberInsurancePlan();
               }
           }


       });

    },

    EnableValidation: function (obj, isValidate) {

        if ($(obj).find('option:selected').text().toLowerCase() == "self") {
            $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance').data('bootstrapValidator').enableFieldValidators('Address1', false);
            $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance').data('bootstrapValidator').enableFieldValidators('City', false);
            $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance').data('bootstrapValidator').enableFieldValidators('State', false);
            $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance').data('bootstrapValidator').enableFieldValidators('Zip', false);
            $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance #spnaddress1').addClass("hidden");
            $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance #spncity').addClass("hidden");
            $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance #spnstate').addClass("hidden");
            $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance #spnzip').addClass("hidden");
            var revalidation = false;
        }

        else {
            $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance').data('bootstrapValidator').enableFieldValidators('Address1', true);
            $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance').data('bootstrapValidator').enableFieldValidators('City', true);
            $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance').data('bootstrapValidator').enableFieldValidators('State', true);
            $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance').data('bootstrapValidator').enableFieldValidators('Zip', true);
            $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance #spnaddress1').removeClass("hidden");
            $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance #spncity').removeClass("hidden");
            $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance #spnstate').removeClass("hidden");
            $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance #spnzip').removeClass("hidden");
            var revalidation = true;
        }

        if (revalidation) {
            $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance').bootstrapValidator('revalidateField', 'Address1');
            $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance').bootstrapValidator('revalidateField', 'City');
            $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance').bootstrapValidator('revalidateField', 'State');
            $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance').bootstrapValidator('revalidateField', 'Zip');
        }

    },

    ValidateInsuranceAutoComplete: function () {
        //utility.ValidateAutoComplete(this, 'frmPatientInsurance #hfInsurancePlan', false);
        utility.ValidateAutoComplete($("#" + Patient_Insurance.params.PanelID + " #txtInsurancePlan"), Patient_Insurance.params.PanelID + ' #hfInsurancePlan', false)
    },

    ValidateValues: function () {
        if (!utility.ValidateAutoComplete($("#" + Patient_Insurance.params.PanelID + " #txtInsurancePlan"), Patient_Insurance.params.PanelID + ' #hfInsurancePlan', false)) {
            return false;
            //$("#txtInsurancePlan").val()!="" && $("#hfInsurancePlan").val()==""
        }
        //else {
        //    utility.ValidateAutoComplete($("#" + Patient_Insurance.params.PanelID + " #txtLawyer"), Patient_Insurance.params.PanelID + ' #hfLawyer', true);
        //}
        return true;
    },

    PatientInsuranceSave: function () {
        Patient_Insurance.SaveInsurance();
    },
    SaveInsurance: function () {
        var strMessage = "";
        var objDef = $.Deferred();
        var self = null;
        var ScanDoc = "";
        var scanImage = "";
        var IsFromCopayUpdateAlert = false;
        Patient_Insurance.InsuranceSaveUpdateResponse = {};
        if (Patient_Insurance.params["PanelID"] != "pnlPatientInsurance") {
            self = $("#" + Patient_Insurance.params["PanelID"] + " #pnlPatientInsurance");
        }
        else {
            self = $('#pnlPatientInsurance');
        }
        if (self.find("#txtSSNIns").val()) {
            var txtValue = self.find("#txtSSNIns").val();   // get ssn value
            if (txtValue) {
                if (globalAppdata.IsFullSSN.toLowerCase() === 'false') {
                    if (Patient_Insurance.PatientSSN) {
                        self.find("#txtSSNIns").val(Patient_Insurance.PatientSSN);
                    }
                }
            }
            else {
                self.find("#txtSSNIns").val("");
            }
        }
        var myJSON = self.getMyJSON();
        self.find("#txtSSNIns").val(txtValue);
        var insurnaceJson = JSON.parse(myJSON);
        if (Patient_Insurance.isProcessed == '1') {
            //In case of default image pic will not update in DB - EMR-3487
            if ($('#scanImage img').attr('src').indexOf("images/idcard1.png") > -1) {
                Patient_Insurance.isProcessed = '0'
                scanImage = "";
            }
            else {
                if (isFileCompressed) {
                    Patient_Insurance.isProcessed = '1'
                    var zip = new JSZip();
                    var scancardsrc = $('#scanImage img').attr('src').replace('data:image/jpg;base64,', '').replace('data:image/jpeg;base64,', '').replace('data:image/png;base64,', '').replace('data:image/gif;base64,', '').replace('data:image/bmp;base64,', '');
                    scanImage = scancardsrc;
                    zip.file("Insurance Card.jpeg", scancardsrc, { base64: true });
                    zip.generateAsync({ type: "blob", compression: "DEFLATE", compressionOptions: { level: 9 } }).then(function (blob) {
                        ScanDoc = blob
                        objDef.resolve("ok");
                    });
                }
                else {
                    scanImage = $('#scanImage img').attr('src').replace('data:image/jpg;base64,', '').replace('data:image/jpeg;base64,', '').replace('data:image/png;base64,', '').replace('data:image/gif;base64,', '').replace('data:image/bmp;base64,', '');
                    objDef.resolve("ok");
                }
            }
        }
        else {
            objDef.resolve("ok");
        }
        myJSON = JSON.stringify(insurnaceJson);
        myJSON = decodeURIComponent(myJSON);

        if ($('#' + Patient_Insurance.params.PanelID + ' #hfInsurancePlan').val() == "-1") {
            utility.DisplayMessages("Insurance Plan not Valid", 2);
        }
        else if ($('#' + Patient_Insurance.params.PanelID + " #ddlPlanAddress").val() == "-1" || $('#' + Patient_Insurance.params.PanelID + " #ddlPlanAddress").val() == "" || $('#' + Patient_Insurance.params.PanelID + " #ddlPlanAddress").val() == null) {
            utility.DisplayMessages("Insurance Plan Address not selected", 2);
        }

        if (($('#' + Patient_Insurance.params["PanelID"] + ' #txtSpecialistCopay').val() != Patient_Insurance.oldSpecialistCopayValue) || ($('#' + Patient_Insurance.params["PanelID"] + ' #txtVisitCopayment').val() != Patient_Insurance.oldVisitCopayValue)) {
            IsFromCopayUpdateAlert = true;
        }
        objDef.then(function () {
            var result;
            result = Patient_Insurance.ValidateValues();
            if (result) {
                if (Patient_Insurance.params.mode == "Add") {
                    AppPrivileges.GetFormPrivileges("Patient Insurance", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            Patient_Insurance.SavePatientInsurance(myJSON, ScanDoc, scanImage, IsFromCopayUpdateAlert).done(function (response) {
                                if (response.status != false) {
                                    Patient_Insurance.showCopayAlertOrLoadSaveUpdateResponse(response);
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
                else if (Patient_Insurance.params.mode == "Edit") {
                    AppPrivileges.GetFormPrivileges("Patient Insurance", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            Patient_Insurance.UpdatePatientInsurance(myJSON, Patient_Insurance.params.PatientInsuranceId, ScanDoc, scanImage, IsFromCopayUpdateAlert).done(function (response) {
                                if (response.status != false) {
                                    Patient_Insurance.showCopayAlertOrLoadSaveUpdateResponse(response);
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
            }
        });
    },
    showCopayAlertOrLoadSaveUpdateResponse:function(response){
        Patient_Insurance.InsuranceSaveUpdateResponse = response;
        if (response.PatientFutureAppointmentForCopay != null && response.PatientFutureAppointmentForCopay.length > 0) {
            Patient_Insurance.AppointmentDetailsForCopay = response.PatientFutureAppointmentForCopay;
            Patient_Insurance.showCopayChangeAlert();
        }
        else
            Patient_Insurance.LoadAfterSaveUpdate(response, Patient_Insurance.params.mode);
    },
    LoadAfterSaveUpdate:function(response, mode){
        Patient_Insurance.InsuranceCardChanged = "false";
        Patient_Insurance.setTCMCountOnDashboard(response, Patient_Insurance.params.mode);
        Patient_Insurance.params.PatientInsuranceId = response.PatientInsuranceId;
        
        if (!params.IsDemographicInfoGlobalyUpdated) {  // when user update form directly from save button
            if (mode == "Add")
                Patient_Insurance.LoadInsuranceList();
            else if (mode == "Edit")
                Patient_Insurance.LoadInsuranceList(Patient_Insurance.params.PatientInsuranceId);
            utility.DisplayMessages(response.message, 1);
        }
        else {
            if (Patient_Insurance.params["PanelID"] && Patient_Insurance.params["PanelID"].indexOf("pnlPatientInsurance") > -1 && $("#" + Patient_Insurance.params["PanelID"]).is(":visible") == true) {
                params.defaultDemographicSerailizeForm = $('#frmPatientInsurance').serialize();
                if (drfdAutoSave)
                    drfdAutoSave.resolve();
                $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance  #hfImageChange').val("false");
            }
        }  // when auto save flow run

        $(" #mainForm  li#CDSAlert").show();
        if (!Patient_Insurance.params.ParentCtrl || Patient_Insurance.params.ParentCtrl != "appointmentDetail") {
            $.when(setPatientBanner(Patient_Insurance.params.patientID, "1")).then(function () {
                if (demographicDetail.params && demographicDetail.params.TabID == "clinicalTabProgressNote")
                    Clinical_ProgressNote.LoadCDSAlerts();
            });
        }

        if (mode == "Add") {
            $("#frmPatientInsurance").trigger('reset');
            Patient_Insurance.showReferralAlertOrCheckEligibility(response);
        }
        else if (mode == "Edit") {
            if (Patient_Insurance.InsurancePlanId != $("#" + Patient_Insurance.params["PanelID"] + " #hfInsurancePlan").val()) {
                Patient_Insurance.showReferralAlertOrCheckEligibility(response);
            }
            else if (Patient_Insurance.InsurancePlanId == $("#" + Patient_Insurance.params["PanelID"] + " #hfInsurancePlan").val() && Patient_Insurance.InsuranceActiveStatus != $("#" + Patient_Insurance.params["PanelID"] + " #chkActive").prop('checked')) {
                Patient_Insurance.showReferralAlertOrCheckEligibility(response);
            }
            else {
                Patient_Insurance.IsLoadEligiblitySc = false;
                if (globalAppdata.IsReferralRequired && globalAppdata.IsReferralRequired.toString().toLowerCase() == 'true' && $("#" + Patient_Insurance.params.PanelID + " #hfIsReferralRequired").val() && $("#" + Patient_Insurance.params.PanelID + " #hfIsReferralRequired").val().toString().toLowerCase() == 'true') {
                    if (response.PatientInsuranceId)
                        Patient_Insurance.PatientReferralAlertDisplay(response.PatientInsuranceId, Patient_Insurance.params['patientID'], response);
                }
            }
        }
    },
    showReferralAlertOrCheckEligibility: function (response) {
        if (response.PatientFutureAppointment != null && response.PatientFutureAppointment.length > 0) {
            Patient_Insurance.IsLoadEligiblitySc = true;
        }

        if (globalAppdata.IsReferralRequired && globalAppdata.IsReferralRequired.toString().toLowerCase() == 'true' && $("#" + Patient_Insurance.params.PanelID + " #hfIsReferralRequired").val() && $("#" + Patient_Insurance.params.PanelID + " #hfIsReferralRequired").val().toString().toLowerCase() == 'true') {
            if (response.PatientInsuranceId)
                Patient_Insurance.PatientReferralAlertDisplay(response.PatientInsuranceId, Patient_Insurance.params['patientID'], response);
        }
        else {
            Patient_Insurance.CheckEligiblity(response);
        }
    },
    setTCMCountOnDashboard: function(response, mode){
        if(mode == "Add"){
            if (response.ClaimFlagDescription == "Medicare Part B" && response.IsTCM == "True" && response.Priority == "1") {
                var count = parseInt($('#spnTCM').text()) + 1;
                $('#pnlDashboard #TCM .badge').text(count);
                $('#spnTCM').text(count);
            }
        }
        else if (mode == "Edit") {
            var previousPriority = $("#" + Patient_Insurance.params["PanelID"] + " #hfPriority").val();
            var previousClaimFlag = $("#" + Patient_Insurance.params["PanelID"] + " #hfClaimFlagDescription").val();
            if (previousClaimFlag != "Medicare Part B" && (response.ClaimFlagDescription == "Medicare Part B" && response.IsTCM == "True" && response.Priority == "1")) {
                var count = parseInt($('#spnTCM').text()) + 1;
                $('#pnlDashboard #TCM .badge').text(count);
                $('#spnTCM').text(count);
            }
            else if ((previousClaimFlag == "Medicare Part B" && previousPriority != 1) && (response.ClaimFlagDescription == "Medicare Part B" && response.IsTCM == "True" && response.Priority == "1")) {
                var count = parseInt($('#spnTCM').text()) + 1;
                $('#pnlDashboard #TCM .badge').text(count);
                $('#spnTCM').text(count);
            }
            else if ((previousClaimFlag == "Medicare Part B" && previousPriority == 1) && (response.ClaimFlagDescription != "Medicare Part B" && response.IsTCM == "True" && response.Priority == "1")) {
                var count = parseInt($('#spnTCM').text()) - 1;
                $('#pnlDashboard #TCM .badge').text(count);
                $('#spnTCM').text(count);
            }
            else if ((previousClaimFlag == "Medicare Part B" && previousPriority == 1) && (response.ClaimFlagDescription == "Medicare Part B" && response.IsTCM != "True" && response.Priority == "1")) {
                var count = parseInt($('#spnTCM').text()) - 1;
                $('#pnlDashboard #TCM .badge').text(count);
                $('#spnTCM').text(count);
            }
            else if ((previousClaimFlag == "Medicare Part B" && previousPriority == 1) && (response.ClaimFlagDescription == "Medicare Part B" && response.IsTCM == "True" && response.Priority != "1")) {
                var count = parseInt($('#spnTCM').text()) - 1;
                $('#pnlDashboard #TCM .badge').text(count);
                $('#spnTCM').text(count);
            }
        }
    },
    ShowAppointmentUpdateAlert: function () {
        Patient_Insurance.ShowFutureAppointments(Patient_Insurance.AppointmentDetails, false);
    },
    ValidateSearchPatternForSelectedInsurancePlan: function () {
        setTimeout(function () {
            if ($('#' + Patient_Insurance.params.PanelID + ' #hfInsurancePlan').val() != "") {
                var selectedPlan = $('#' + Patient_Insurance.params.PanelID + ' #txtInsurancePlan').autocomplete('option', 'source');
                if (typeof (selectedPlan) == 'function') {
                    var obj = { term: "" }
                    selectedPlan(obj, function (res) {
                        selectedPlan = res;
                    });

                }
                var filteredPlan = selectedPlan.filter(function (item) {
                    return item.id == $('#' + Patient_Insurance.params.PanelID + ' #hfInsurancePlan').val() && item.searchPattern;
                });
                if (filteredPlan.length > 0) {
                    $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance #hfInsurancePlansearchPattern').val(filteredPlan[0].searchPattern);
                } else {
                    $('#' + Patient_Insurance.params.PanelID + ' #hfInsurancePlansearchPattern').val('');
                }

            } else {
                $('#' + Patient_Insurance.params.PanelID + ' #hfInsurancePlansearchPattern').val('');
            }
        }, 200);
    },
    // -------------- Insurance Plan ---------------------
    FillInsurancePlanName: function (InsurancePlanId, InsurancePlanName, SearchPattern, event, IPDescription, isReferralRequired) {
        InsurancePlanName = InsurancePlanName.replace('&#39;', "'");

        $Ctr = $("#" + Patient_Insurance.params.PanelID + " #txtInsurancePlan");
        $hfCtr = $("#" + Patient_Insurance.params.PanelID + " #hfInsurancePlan");

        $("#" + Patient_Insurance.params.PanelID + " #hfIsReferralRequired").val(isReferralRequired);
        $Ctr.val(InsurancePlanName);
        $hfCtr.val(InsurancePlanId);
        $('#' + Patient_Insurance.params.PanelID + ' #hfInsurancePlansearchPattern').val(SearchPattern);
        $("#" + Patient_Insurance.params.PanelID + " #lnkInsurancePlanDetail").css("display", "inline");
        $("#" + Patient_Insurance.params.PanelID + " #lblInsurancePlan").css("display", "none");
        $Ctr.removeAttr("data-original-title");
        Patient_Insurance.setPlanToolTip(IPDescription);

        var obj_ = { id: InsurancePlanId, value: InsurancePlanName, searchPattern: SearchPattern, IPDescription: IPDescription, Isreferral: isReferralRequired };
        utility.SetAutoCompleteSource($Ctr, $hfCtr, obj_);

        if (Patient_Insurance.params["PanelID"] == "pnldemographicDetail" && demographicDetail.params["PanelID"] == "pnlBillChargeSearch #pnldemographicDetail") {
            UnloadActionPan(Admin_InsurancePlan.params.ParentCtrl, 'Admin_InsurancePlan', null, Admin_InsurancePlan.params.PanelID);
        } else {
            if (Admin_InsurancePlan.params != null && Admin_InsurancePlan.params.ParentCtrl != null) {
                UnloadActionPan(Admin_InsurancePlan.params.ParentCtrl, 'Admin_InsurancePlan', null, Admin_InsurancePlan.params.PanelID);
            }
            else
                UnloadActionPan(null, 'Admin_InsurancePlan');
        }
        Patient_Insurance.FillInsurancePlanAddress(InsurancePlanId);
        if ($("#" + Patient_Insurance.params.PanelID + " #frmPatientInsurance").data("bootstrapValidator") != null) {
            $("#" + Patient_Insurance.params.PanelID + " #frmPatientInsurance").bootstrapValidator('revalidateField', 'InsurancePlan');
        }
        //no neeed to call this because autocomplete is now on oninput, source is set expectily and search pattrern is also set.
        //$("#" + Patient_Insurance.params.PanelID + " #txtInsurancePlan").trigger('blur');

    },

    SetReferralRequired: function (isReferral) {
        $("#" + Patient_Insurance.params.PanelID + " #hfIsReferralRequired").val(isReferral);
    },
    OpenInsurancePlan: function (PlanProvider) {
        var params = [];
        var PanelID = null;
        if (Patient_Insurance.params["PanelID"] != "pnlPatientInsurance")
            PanelID = Patient_Insurance.params["PanelID"];
        if (Patient_Insurance.params.ParentCtrl == "Admin_InsurancePlan" || Patient_Insurance.params.ParentCtrl == "demographicDetail" || Patient_Insurance.params.ParentCtrl == "EncounterChargeCapture" || Patient_Insurance.params.ParentCtrl == "appointmentDetail")
            params["ParentCtrl"] = 'Patient_Insurance';
        else {
            if (Patient_Insurance.params.ParentCtrl == "demographicDetail") {
                params["ParentCtrl"] = 'Patient_Insurance';
            } else {
                params["ParentCtrl"] = 'patTabInsurance';
            }
        }
        params["InsurancePlanId"] = "-1";
        params["FromAdmin"] = "0";
        if (PlanProvider != null)
            params["RefCtrl"] = PlanProvider;
        LoadActionPan('Admin_InsurancePlan', params, PanelID);
    },

    OpenInsurancePlanAddress: function (PlanProvider) {
        var params = [];
        var PanelID = null;
        if (Patient_Insurance.params["PanelID"] != "pnlPatientInsurance")
            PanelID = Patient_Insurance.params["PanelID"];
        if (Patient_Insurance.params.ParentCtrl == "Admin_InsurancePlan" || Patient_Insurance.params.ParentCtrl == "demographicDetail" || Patient_Insurance.params.ParentCtrl == "EncounterChargeCapture" || Patient_Insurance.params.ParentCtrl == "appointmentDetail")
            params["ParentCtrl"] = 'Patient_Insurance';
        else {
            if (Patient_Insurance.params.ParentCtrl == "demographicDetail") {
                params["ParentCtrl"] = 'Patient_Insurance';
            } else {
                params["ParentCtrl"] = 'patTabInsurance';
            }
        }
        params["InsurancePlanId"] = "0";
        params["FromAdmin"] = "0";
        params["RefHiddenIdCtrl"] = "frmPatientInsurance #hfInsurancePlan";
        params["RefCtrl"] = "frmPatientInsurance #txtInsurancePlan";
        params["SearchPattern"] = "frmPatientInsurance #hfInsurancePlansearchPattern";
        params["Address"] = "frmPatientInsurance #ddlPlanAddress";
        LoadActionPan('Admin_InsurancePlanAddress', params, PanelID);
    },
 
    OpenSubScriberInsurancePlan: function (obj, strSubscriberId) {
        var SubscriberId = "";
        if (strSubscriberId) {
            SubscriberId = strSubscriberId;
        }
        else {
            SubscriberId = $(obj).val();
        }

        var insurancePlan = $('#' + Patient_Insurance.params.PanelID + ' #txtInsurancePlan').val();
        if (SubscriberId != "") {
            if (insurancePlan == "") {

                Admin_InsurancePlan.SearchInsurancePlan(null, null, SubscriberId).done(function (response) {
                    if (response.status != false) {

                        if (response.InsurancePlanCount > 0) {
                            if (response.InsurancePlanCount == 1) {
                                var ObjIns = JSON.parse(response.InsurancePlanLoad_JSON)[0];
                                var shortName = ObjIns.ShortName;
                                var InsPlanID = ObjIns.InsurancePlanId;
                                var SearchPattern = ObjIns.SearchPattern;
                                $('#' + Patient_Insurance.params.PanelID + ' #txtInsurancePlan').val(ObjIns.Description);
                                $('#' + Patient_Insurance.params.PanelID + ' #hfInsurancePlan').val(InsPlanID);
                                $('#' + Patient_Insurance.params.PanelID + ' #txtInsurancePlan').focus();
                                $("#" + Patient_Insurance.params["PanelID"] + " #hfInsurancePlansearchPattern").val(SearchPattern);

                                var obj_ = { id: InsPlanID, value: ObjIns.Description, searchPattern: SearchPattern, IPDescription: shortName, Isreferral: ObjIns.IsReferralRequired };
                                utility.SetAutoCompleteSource($('#' + Patient_Insurance.params.PanelID + ' #txtInsurancePlan'), $('#' + Patient_Insurance.params.PanelID + ' #hfInsurancePlan'), obj_);
                                Patient_Insurance.setPlanToolTip(ObjIns.Description);

                                utility.ValidateAutoComplete($('#' + Patient_Insurance.params.PanelID + ' #txtInsurancePlan'), 'frmPatientInsurance #hfInsurancePlan', false);
                                $('#frmPatientInsurance').bootstrapValidator('revalidateField', 'InsurancePlan');
                                Patient_Insurance.ProcessedData = null;
                                Patient_Insurance.FillInsurancePlanAddress(InsPlanID);
                            }
                            else {

                                var params = [];
                                params["InsurancePlanId"] = "-1";
                                params["FromAdmin"] = "0";
                                params["SubScriberId"] = SubscriberId;
                                params["MatchedInsurancePlan"] = response;
                                if (strSubscriberId) {
                                    setTimeout(function () {
                                        LoadActionPan('Admin_InsurancePlan', params);
                                    }, 600)
                                }
                                else {
                                    LoadActionPan('Admin_InsurancePlan', params);
                                }

                            }
                        }
                        else {
                            if (Patient_Insurance.ProcessedData != null) {

                                //  var PlanProvider = Patient_Insurance.ProcessedData.PlanProvider;
                                // setTimeout(function () { Patient_Insurance.OpenInsurancePlan(PlanProvider) }, 1000)

                                //var parameters = [];

                                //if (Patient_Insurance.params.ParentCtrl == "demographicDetail")
                                //    parameters["ParentCtrl"] = 'Patient_Insurance';
                                //else
                                //    parameters["ParentCtrl"] = 'patTabInsurance';

                                //parameters["InsurancePlanId"] = "-1";
                                //parameters["FromAdmin"] = "0";
                                //parameters["InsurancePlanDescription"] = PlanProvider;
                                //LoadActionPan('Admin_InsurancePlan', parameters);
                                Patient_Insurance.ProcessedData = null;

                            }
                        }
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }

                });

            }
        }

    },

    OpenPreAuthorization: function () {
        var params = [];
        var PanelID = null;
        if (Patient_Insurance.params["PanelID"] != "pnlPatientInsurance")
            PanelID = Patient_Insurance.params["PanelID"];
        if (Patient_Insurance.params.ParentCtrl == "demographicDetail" || Patient_Insurance.params.ParentCtrl == "EncounterChargeCapture" || Patient_Insurance.params.ParentCtrl == "appointmentDetail")
            params["ParentCtrl"] = 'Patient_Insurance';
        else
            params["ParentCtrl"] = 'patTabInsurance';
        params["PreAuthorizationId"] = "-1";
        params["FromAdmin"] = "0";
        params["patientID"] = Patient_Insurance.params.patientID;
        params["PlanResponse"] = Patient_Insurance.params.PlanResponse;
        LoadActionPan('Patient_PreAuthorization', params, PanelID);
    },

    OpenScanViewer: function () {
        if (globalAppdata['OCRLicenseKey'] == "") {
            utility.DisplayMessages('You do not have rights for scan. Please contact with administrator', 2);
        }
        else {
            var strMessage = "";
            var params = [];
            params["PatientID"] = Patient_Insurance.params.patientID;
            params['ParentCtrl'] = 'patTabInsurance';
            LoadActionPan('OCR_Scanner', params, PanelID);
            $('#pnlOCRScannerViewer #modaldialog').removeClass('modal-dialog-lg');
        }
    },

    OpenPatientEligibility: function (isFromSave) {
        var params = [];
        params["FromAdmin"] = "0";

        var PanelID = null;
        if (Patient_Insurance.params["PanelID"] != "pnlPatientInsurance")
            PanelID = Patient_Insurance.params["PanelID"];
        if (Patient_Insurance.params.ParentCtrl == "demographicDetail" || Patient_Insurance.params.ParentCtrl == "EncounterChargeCapture" || Patient_Insurance.params.ParentCtrl == "appointmentDetail")
            params["ParentCtrl"] = 'Patient_Insurance';
        else
            params["ParentCtrl"] = 'patTabInsurance';

        params["IsFromInsuranceSave"] = isFromSave;
        params["patientID"] = Patient_Insurance.params.patientID;
        params["patientAccount"] = Patient_Demographic.params.PatientAccountNo;
        params["patientLastName"] = Patient_Demographic.params.PatientLastName;
        params["patientFirstName"] = Patient_Demographic.params.PatientFirstName;
        params["patientInsurancePlanId"] = Patient_Insurance.params.PatientInsuranceId;
        params["Provider"] = Patient_Demographic.params.PatientProvider;
        params["ProviderId"] = Patient_Demographic.params.PatientProviderId;
        LoadActionPan('Patient_Eligibility', params, PanelID);
    },

    OpenPatientEligibilityFromSch: function (patientID, PatientProvider, PatientProviderId, PatientAccountNo, PatientLastName, PatientFirstName, PatientInsuranceId) {
        var params = [];
        params["FromAdmin"] = "0";
        var isFromSave = true;
        var PanelID = null;
        //else
        params["ParentCtrl"] = 'schTabCalendar';
        params["IsFromInsuranceSave"] = isFromSave;
        params["patientID"] = patientID;
        params["patientAccount"] = PatientAccountNo;
        params["patientLastName"] = PatientLastName;
        params["patientFirstName"] = PatientFirstName;
        params["patientInsurancePlanId"] = PatientInsuranceId;
        params["Provider"] = PatientProvider;
        params["ProviderId"] = PatientProviderId;
        LoadActionPan('Patient_Eligibility', params, PanelID);
    },

    RemoveInsuranceImage: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Documents", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    if (Patient_Insurance.params.PatientInsuranceId) {
                        Patient_Insurance.DeleteInsuranceDocument(Patient_Insurance.params.PatientInsuranceId).done(function (response) {
                            if (response.status != false) {
                                //$('#scanImage img').attr('src', null);
                                //  $('#scanImage').empty();
                                Patient_Insurance.isProcessed = 0;
                                $('#scanImage #image').attr("src", "Content/images/idcard1.png");
                                $('#scanImage #image').css({ "cursor": "default" });
                                $("#pnlPatientInsurance #hfPatDocId").val("");
                                //$("#pnlPatientInsurance #image").hide();
                                $("#pnlPatientInsurance #backimage").hide();
                                $("#pnlPatientInsurance #btnRemoveInsurance").hide();
                                utility.DisplayMessages(response.Message, 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    } else {
                        utility.DisplayMessages("No Insurance is selected to remove card.", 3);
                    }

                }, function () { },
                    '1'
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        //Patient_Insurance.params.PatientInsuranceId
        //$('#scanImage img').attr('src', 'Content/images/idcard1.png');
        //$('#scanBackImage img').attr('src', 'Content/images/idcard2.png');

        // $("#pnlPatientInsurance #image").attr('src', "Content/images/idcard1.png?" + currentTime.getTime());
        //  $("#pnlPatientInsurance #backimage").attr('src', "Content/images/idcard2.png?" + currentTime.getTime());
        // $("#pnlPatientInsurance #backimage").show();
    },
    DeleteInsuranceDocument: function (patientInsuranceId) {
        if (localStorage.SelectedPatientId) {
            var data = "PatientInsuranceId=" + patientInsuranceId + "&PatientID=" + localStorage.SelectedPatientId;
            // serach parameter , class name, command name of class
            return MDVisionService.defaultService(data, "PATIENT_INSURANCE", "DELETE_PATIENT_INSURANCE_DOCUMENT");
        } else {

        }
    },

    //start ,adnan maqbool , PMS-4813
    SetSelfRelation: function (text) {
        if (text != "") {
            var CurrentOptionText = $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #ddlRelation option[value='" + text + "']").text();
            if (CurrentOptionText.toLowerCase() == "self") {
                Patient_Insurance.CopyPatient();

                $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #PatAddreeslnk").hide();
            }
            else {
                $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #PatAddreeslnk").show();
                $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtLastName").val("");

                $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtFirstName").val("");
                $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtMiddleInitial").val("");
                $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #dtpDOB").val("");
                $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #ddlSex").val("");
                $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtSSNIns").val("");
                $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtAddress1").val("");
                $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtAddress2").val("");
                $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtCity").val("");
                $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtState").val("");
                $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtZip").val("");
                $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtZipExt").val("");
                $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtHomeTel").val("");

                $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance').data('bootstrapValidator').enableFieldValidators('LastName', true);
                $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance').data('bootstrapValidator').enableFieldValidators('FirstName', true);
                $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance').data('bootstrapValidator').enableFieldValidators('DOB', true);
                $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance').data('bootstrapValidator').enableFieldValidators('Sex', true);
                $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance').data('bootstrapValidator').enableFieldValidators('Address1', true);
                $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance').data('bootstrapValidator').enableFieldValidators('City', true);
                $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance').data('bootstrapValidator').enableFieldValidators('State', true);
                $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance').data('bootstrapValidator').enableFieldValidators('Zip', true);

                $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance').bootstrapValidator('revalidateField', 'LastName');
                $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance').bootstrapValidator('revalidateField', 'FirstName');
                $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance').bootstrapValidator('revalidateField', 'DOB');
                $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance').bootstrapValidator('revalidateField', 'Sex');
                $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance').bootstrapValidator('revalidateField', 'Address1');
                $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance').bootstrapValidator('revalidateField', 'City');
                $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance').bootstrapValidator('revalidateField', 'State');
                $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance').bootstrapValidator('revalidateField', 'Zip');
                Patient_Insurance.PatientSSN = "";
                $('#pnlPatientInsurance #frmPatientInsurance #txtSSNIns').attr("placeholder", "999-99-9999");
                $('#pnlPatientInsurance #frmPatientInsurance #txtSSNIns').attr("data-mask", "999-99-9999");
                $('#pnlPatientInsurance #frmPatientInsurance #txtSSNIns').attr("disabled", false);

            }
        }
    },

    CopyPatient: function () {
        //if (Patient_Insurance.params.ParentCtrl == "demographicDetail" || Patient_Insurance.params.ParentCtrl == "EncounterChargeCapture") {
        var patientId = null;
        if (localStorage.SelectedPatientId) {
            patientId = localStorage.SelectedPatientId;
        }
        else {
            patientId = Patient_Insurance.params.patientID;
        }
        Patient_Insurance.GetPatientRelationshipInfo(Patient_Insurance.params.PateintId).done(function (response) {
            var patientRelInfoObj = JSON.parse(response.PatientRelationshipInfoLoad_JSON);
            $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtLastName").val(patientRelInfoObj[0].LastName);
            $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtFirstName").val(patientRelInfoObj[0].FirstName);
            $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtMiddleInitial").val(patientRelInfoObj[0].MI);
            //$("#pnlPatientInsurance #frmPatientInsurance #dtpDOB").val($('#pnldemographicDetail #frmdemographicDetail #dtpDOB').val());
            if (patientRelInfoObj[0].DOB)
                $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #dtpDOB").datepicker("setDate", utility.RemoveTimeFromDate(null, patientRelInfoObj[0].DOB));

            $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #ddlSex option").each(function () {
                if ($(this).text() == patientRelInfoObj[0].Gender) {
                    $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #ddlSex").val($(this).val());
                }
            });
            $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtSSNIns").val(patientRelInfoObj[0].SSN);
            $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtAddress1").val(patientRelInfoObj[0].Address1);
            $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtAddress2").val(patientRelInfoObj[0].Address2);
            $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtCity").val(patientRelInfoObj[0].City);
            $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtState").val(patientRelInfoObj[0].State);
            $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtZip").val(patientRelInfoObj[0].ZIPCode);
            $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtZipExt").val(patientRelInfoObj[0].ZIPCodeExt);
            $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtHomeTel").val(patientRelInfoObj[0].HomePhoneNo);
            $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #ddlRelation").val(8);
            if ($("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #ddlRelation option:selected").text().toLowerCase() == "self") {
                if (patientRelInfoObj[0].SSN) {
                    Patient_Insurance.PatientSSN = patientRelInfoObj[0].SSN;
                    if (globalAppdata.IsFullSSN.toLowerCase() === 'true') {
                        $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtSSNIns").attr("placeholder", "999-99-9999");
                        $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtSSNIns").attr("data-mask", "999-99-9999");
                        $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtSSNIns").val(patientRelInfoObj[0].SSN);
                        $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtSSNIns").attr("disabled", false);
                    }
                    else {
                        $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtSSNIns").attr("placeholder", "XXX-XX-9999");
                        $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtSSNIns").attr("data-mask", "XXX-XX-9999");
                        var lastFourDigit = patientRelInfoObj[0].SSN.slice(-4);
                        var formatSSN = "XXX-XX-" + lastFourDigit;
                        $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtSSNIns").val(formatSSN);
                        $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtSSNIns").attr("disabled", true);
                    }
                }
            } else {
                Patient_Insurance.PatientSSN = "";
                $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtSSNIns").attr("placeholder", "999-99-9999");
                $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtSSNIns").attr("data-mask", "999-99-9999");
                $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtSSNIns").val(patientRelInfoObj[0].SSN);
                $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtSSNIns").attr("disabled", false);
            }

            if ($('#frmPatientInsurance').data('bootstrapValidator') != null && typeof $('#frmPatientInsurance').data('bootstrapValidator') != 'undefined') {
                $('#frmPatientInsurance').bootstrapValidator('revalidateField', 'Relation');
                $('#frmPatientInsurance').bootstrapValidator('revalidateField', 'LastName');
                $('#frmPatientInsurance').bootstrapValidator('revalidateField', 'FirstName');
                //$('#frmPatientInsurance').bootstrapValidator('revalidateField', 'MiddleInitial');
                $('#frmPatientInsurance').bootstrapValidator('revalidateField', 'DOB');
                $('#frmPatientInsurance').bootstrapValidator('revalidateField', 'Sex');
            }
            Patient_Insurance.EnableValidation($("#" + Patient_Insurance.params["PanelID"] + " #ddlRelation"), true);
        });
    },
    LoadPatientAddress: function () {
        var patientId = null;
        if (localStorage.SelectedPatientId) {
            patientId = localStorage.SelectedPatientId;
        }
        else {
            patientId = Patient_Insurance.params.patientID;
        }
        Patient_Insurance.GetPatientRelationshipInfo(Patient_Insurance.params.PateintId).done(function (response) {
            var patientRelInfoObj = JSON.parse(response.PatientRelationshipInfoLoad_JSON);
            $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtAddress1").val(patientRelInfoObj[0].Address1);
            $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtAddress2").val(patientRelInfoObj[0].Address2);
            $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtCity").val(patientRelInfoObj[0].City);
            $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtState").val(patientRelInfoObj[0].State);
            $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtZip").val(patientRelInfoObj[0].ZIPCode);
            $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtZipExt").val(patientRelInfoObj[0].ZIPCodeExt);
            $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance #txtHomeTel").val(patientRelInfoObj[0].HomePhoneNo);
            if ($('#frmPatientInsurance').data('bootstrapValidator') != null && typeof $('#frmPatientInsurance').data('bootstrapValidator') != 'undefined') {
                $('#frmPatientInsurance').bootstrapValidator('revalidateField', 'Address1');
                $('#frmPatientInsurance').bootstrapValidator('revalidateField', 'City');
                $('#frmPatientInsurance').bootstrapValidator('revalidateField', 'State');
                $('#frmPatientInsurance').bootstrapValidator('revalidateField', 'Zip');
            }
        });
    },
    //Un Used function //////////////////////////////////////
    ScanCard: function () {
        //Patient_Insurance.CardScan("e:\\yyy.bmp");
        Patient_Insurance.CardScan(Patient_Insurance.params.patientID).done(function (response) {
            if (response.status != false) {
                var OCRCardScan_detail = JSON.parse(response.OCRCardScan_JSON);
                var self = $("#pnlPatientInsurance");
                utility.bindMyJSON(true, OCRCardScan_detail, false, self).done(function () {
                    Patient_Insurance.ValidatePatientInsurance();
                    //serialize data
                    $('#frmPatientInsurance').data('serialize', $('#frmPatientInsurance').serialize());
                    try {
                        if (Patient_Insurance.params["PanelID"] && Patient_Insurance.params["PanelID"].indexOf("pnlPatientInsurance") > -1 && $("#" + Patient_Insurance.params["PanelID"]).is(":visible") == true) {
                            // serialize all data in global variable when form is  loaded
                            params.defaultDemographicSerailizeForm = $('#frmPatientInsurance').serialize();
                            params.IsDemographicInfoGlobalyUpdated = false;
                            params.DemographicAutoUpdateActiveTab = "Insurance";
                            $("#" + Patient_Insurance.params["PanelID"] + " #frmPatientInsurance").data('bootstrapValidator').resetForm();
                            $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance  #hfImageChange').val("false");
                        }
                        else if ($("#pnlPatientInsurance #frmPatientInsurance").length > 0 && $("#pnlPatientInsurance #frmPatientInsurance").is(":visible") == true) {
                            // serialize all data in global variable when form is  loaded
                            params.defaultDemographicSerailizeForm = $('#frmPatientInsurance').serialize();
                            params.IsDemographicInfoGlobalyUpdated = false;
                            params.DemographicAutoUpdateActiveTab = "Insurance";
                            $("#pnlPatientInsurance #frmPatientInsurance").data('bootstrapValidator').resetForm();
                            $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance  #hfImageChange').val("false");
                        }
                    } catch (e) {

                    }


                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    GetPatientRelationshipInfo: function (patientId) {
        patientID = Patient_Insurance.params.patientID;
        var data = "PatientID=" + patientID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_INSURANCE", "SEARCH_PATIENT_RELATIONSHIP_INFO");
    },
    ///////////////////////////////////////////////
    OpenInsurancePlanDetail: function () {
        //Admin_InsurancePlan.InsurancePlanEdit($("#pnlPatientInsurance #hfInsurancePlan").val());
        var params = [];
        var PanelID = null;
        if (Patient_Insurance.params["PanelID"] != "pnlPatientInsurance")
            PanelID = Patient_Insurance.params["PanelID"];
        if (Patient_Insurance.params.ParentCtrl == "demographicDetail" || Patient_Insurance.params.ParentCtrl == "EncounterChargeCapture" || Patient_Insurance.params.ParentCtrl == "appointmentDetail")
            params["ParentCtrl"] = 'Patient_Insurance';
        else
            params["ParentCtrl"] = 'patTabInsurance';
        params["InsurancePlanId"] = $("#" + Patient_Insurance.params.PanelID + " #hfInsurancePlan").val();
        params["PlanAddressId"] = $("#pnlPatientInsurance #ddlPlanAddress").last().val();
        params["mode"] = "Edit";
        Admin_InsurancePlan.params["FromAdmin"] == "0";
        //params["ParentCtrl"] = 'patTabInsurance';
        LoadActionPan('insurancePlanDetail', params, PanelID);
    },

    HideInsurancePlanLink: function () {
        $("#" + Patient_Insurance.params.PanelID + " #frmPatientInsurance #hfInsurancePlan").val("-1");
        $("#pnlPatientInsurance #lnkInsurancePlanDetail").css("display", "none");
        $("#pnlPatientInsurance #lblInsurancePlan").css("display", "inline");
        $('#pnlPatientInsurance #ddlPlanAddress').empty();
        $('#pnlPatientInsurance #ddlPlanAddress').append($("<option />").val("").text("- SELECT -"));
    },
    // -------------- End Insurance Plan -----------------

    FillInsurancePlanAddress: function (InsurancePlanId, PlanAddressValId, FromAddressplanSearch) {
        var objDeffered = $.Deferred();
        CacheManager.BindDropDownsByID('#' + Patient_Insurance.params.PanelID + ' #ddlPlanAddress', 'GetInsurancePlanAddress', true, InsurancePlanId, "refValue").done(function () {
            var AddressCount = $('#' + Patient_Insurance.params.PanelID + ' #ddlPlanAddress option').length;
            //if single address is there, then should be selected by default
            if (AddressCount == 2) {
                $('#' + Patient_Insurance.params.PanelID + ' #ddlPlanAddress option:nth-child(2)').prop("selected", true);
                // Begin 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3407
                $('#' + Patient_Insurance.params.PanelID + ' #frmPatientInsurance').bootstrapValidator('revalidateField', 'PlanAddress');
                // End 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3407
            } else if (FromAddressplanSearch == true) {
                for (var i = 0; i < $('#' + Patient_Insurance.params.PanelID + ' #ddlPlanAddress option').length; i++) {
                    if ($('#' + Patient_Insurance.params.PanelID + ' #ddlPlanAddress option')[i].value == PlanAddressValId) {
                        var j = i + 1;
                        $('#' + Patient_Insurance.params.PanelID + ' #ddlPlanAddress option:nth-child(' + j + ')').prop("selected", true);
                    }
                }
            }
            objDeffered.resolve();
            Patient_Insurance.FillPhoneNumber($("#ddlPlanAddress").change());
            return objDeffered;
        });;
        return objDeffered;
    },

    // -------------- Lawyer ---------------------
    FillLawyerName: function (LawyerId, LawyerName) {
        var $Ctrl = $("#" + Patient_Insurance.params["PanelID"] + " input#txtLawyer");
        var $hfCtrl = $("#" + Patient_Insurance.params["PanelID"] + " #hfLawyer");
        utility.SetKendoAutoCompleteSourceforValidate($Ctrl, LawyerName, $hfCtrl, LawyerId);

        $("#pnlPatientInsurance #lnkLawyerDetail").css("display", "inline");
        $("#pnlPatientInsurance #lblLawyer").css("display", "none");
        UnloadActionPan(Patient_Lawyer.params["ParentCtrl"]);
        $("#frmPatientInsurance").removeClass('disableAll');
    },

    OpenLawyer: function () {
        var params = [];
        var PanelID = null;
        if (Patient_Insurance.params["PanelID"] != "pnlPatientInsurance")
            PanelID = Patient_Insurance.params["PanelID"];
        if (Patient_Insurance.params.ParentCtrl == "demographicDetail" || Patient_Insurance.params.ParentCtrl == "EncounterChargeCapture" || Patient_Insurance.params.ParentCtrl == "appointmentDetail")
            params["ParentCtrl"] = 'Patient_Insurance';
        else
            params["ParentCtrl"] = 'patTabInsurance';
        params["LawyerId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "frmPatientInsurance";
        LoadActionPan('Patient_Lawyer', params, PanelID);
        $('#frmPatientInsurance').addClass('disableAll');
    },

    OpenLawyerDetail: function () {
        //Patient_Lawyer.LawyerEdit($("#pnlPatientInsurance #hfLawyer").val(), "patTabInsurance");
        var params = [];
        params["LawyerId"] = $("#" + Patient_Insurance.params["PanelID"] + " #hfLawyer").val();
        params["mode"] = "Edit";
        params["ParentCtrl"] = 'Patient_Insurance';
        LoadActionPan('lawyerDetail', params);
    },

    HideLawyerLink: function () {
        $("#pnlPatientInsurance #hfLawyer").val("-1");
        $("#pnlPatientInsurance #lnkLawyerDetail").css("display", "none");
        $("#pnlPatientInsurance #lblLawyer").css("display", "inline");
    },
    // -------------- End Lawyer -----------------

    // -------------- Employer -----------------
    FillEmployerName: function (EmployerId, EmployerName) {
        var $Ctrl = $("#" + Patient_Insurance.params["PanelID"] + " input#txtEmployer");
        var $hfCtrl = $("#" + Patient_Insurance.params["PanelID"] + " #hfEmployer");
        utility.SetKendoAutoCompleteSourceforValidate($Ctrl, EmployerName, $hfCtrl, EmployerId);
        UnloadActionPan(Patient_Employer.params["ParentCtrl"]);
        $("#frmPatientInsurance").removeClass('disableAll');
        $("#pnlPatientInsurance #txtEmployer").focus().blur();
    },

    OpenEmployer: function () {
        var params = [];
        var PanelID = null;
        if (Patient_Insurance.params["PanelID"] != "pnlPatientInsurance")
            PanelID = Patient_Insurance.params["PanelID"];
        if (Patient_Insurance.params.ParentCtrl == "demographicDetail" || Patient_Insurance.params.ParentCtrl == "EncounterChargeCapture" || Patient_Insurance.params.ParentCtrl == "appointmentDetail")
            params["ParentCtrl"] = 'Patient_Insurance';
        else
            params["ParentCtrl"] = 'patTabInsurance';
        params["EmployerId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "frmPatientInsurance";
        LoadActionPan('Patient_Employer', params, PanelID);
        $('#frmPatientInsurance').addClass('disableAll');
    },

    OpenEmployerDetail: function () {
        //Patient_Employer.EmployerEdit($("#" + Patient_Insurance.params["PanelID"] + " #hfEmployer").val(), "patTabInsurance");
        var params = [];
        params["EmployerId"] = $("#" + Patient_Insurance.params["PanelID"] + " #hfEmployer").val();
        params["mode"] = "Edit";
        params["ParentCtrl"] = 'Patient_Insurance';
        LoadActionPan('employerDetail', params);
    },

    HideEmployerLink: function () {
        $("#pnlPatientInsurance #hfEmployer").val("-1");
        $("#pnlPatientInsurance #lnkEmployerDetail").css("display", "none");
        $("#pnlPatientInsurance #lblEmployer").css("display", "inline");
    },
    // -------------- End Employer -------------

    SearchPatientInsurance: function (patientID, PatientInsuranceId) {
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

    FillPatientInsurance: function (PatientInsuranceID) {
        var data = "PatientInsuranceID=" + PatientInsuranceID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_INSURANCE", "FILL_PATIENT_INSURANCE");
    },

    SavePatientInsurance: function (PatientInsuranceData, ScanDoc, scanImage, IsFromCopayUpdateAlert) {
        var data = new FormData();
        data.append("0", ScanDoc);
        data.append("scanImage", scanImage);
        data.append("PatientInsuranceData", PatientInsuranceData);
        //data.append("PatientInsuranceID", PatientInsuranceID);
        data.append("PatientID", Patient_Insurance.params.patientID);
        data.append("BStream", Patient_Insurance.isProcessed);
        data.append("InsuranceCardChanged", Patient_Insurance.InsuranceCardChanged);
        data.append("IsFromCopayUpdateAlert", IsFromCopayUpdateAlert);
        //var data = "PatientInsuranceData=" + PatientInsuranceData + "&PatientID=" + Patient_Insurance.params.patientID + "&BStream=" + Patient_Insurance.isProcessed;
        // serach parameter , class name, command name of class 
        return MDVisionService.fileService(data, "PATIENT_INSURANCE", "SAVE_PATIENT_INSURANCE");
    },

    UpdatePatientInsurance: function (PatientInsuranceData, PatientInsuranceID, ScanDoc, scanImage, IsFromCopayUpdateAlert) {
        var data = new FormData();
        data.append("0", ScanDoc);
        data.append("scanImage", scanImage);
        data.append("PatientInsuranceData", PatientInsuranceData);
        data.append("PatientInsuranceID", PatientInsuranceID);
        data.append("PatientID", Patient_Insurance.params.patientID);
        data.append("BStream", Patient_Insurance.isProcessed);
        data.append("InsuranceCardChanged", Patient_Insurance.InsuranceCardChanged);
        data.append("IsFromCopayUpdateAlert", IsFromCopayUpdateAlert);
        //var data = "PatientInsuranceData=" + PatientInsuranceData + "&PatientInsuranceID=" + PatientInsuranceID + "&PatientID=" + Patient_Insurance.params.patientID + "&BStream=" + Patient_Insurance.isProcessed + "&InsuranceCardChanged=" + Patient_Insurance.InsuranceCardChanged + "&ScanDoc=" + ScanDoc;
        // serach parameter , class name, command name of class 
        return MDVisionService.fileService(data, "PATIENT_INSURANCE", "UPDATE_PATIENT_INSURANCE");
    },

    UpdateInsurancePriority: function (Priority) {
        var data = "InsurancePriority=" + Priority + "&PatientID=" + Patient_Insurance.params.patientID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_INSURANCE", "UPDATE_PATIENT_INSURANCE_PRIORITY");
    },

    LoadInsurancePlanAddress: function (InsurancePlanId) {
        var data = "InsurancePlanId=" + InsurancePlanId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_INSURANCE_PLAN_DETAIL", "LOAD_ADDRESS_INFO");
    },

    SearchReferral: function (PrimaryID, PageNumber, ResultPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (ResultPerPage == null) {
            ResultPerPage = 15;
        }
        var data = "PatientInsuranceID=" + Patient_Insurance.params.PatientInsuranceId + "&pageNumber=" + PageNumber + "&rowsPerPage=" + ResultPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_REFERRAL", "SEARCH_REFERRAL");
    },

    DeleteReferral: function (ReferralId) {
        var data = "ReferralID=" + ReferralId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_REFERRAL", "DELETE_REFERRAL");
    },

    UpdateReferralActiveInactive: function (LawyerID, IsActive) {
        var data = "ReferralID=" + LawyerID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_REFERRAL", "UPDATE_REFERRAL_ACTIVE_INACTIVE");
    },
    CardScan: function (patientID) {
        var data = "patientID=" + patientID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "OCR_SCAN_CARD", "OCR_SCAN");
    },
    ValidateInsurancePlanName: function (subscriberID, format) {
        var changeFormat = utility.replaceSymbols(format, "%", "!per")
        var data = "subscriberID=" + subscriberID + "&format=" + changeFormat;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_INSURANCE", "VALIDATE_INSURANCE_FORMAT");
    },
    InusrancePlanNameValidation: function () {
        if ($('#' + Patient_Insurance.params.PanelID + ' #txtSubscriberID').val() != "") {
            var selectedPlan = $('#' + Patient_Insurance.params.PanelID + ' #txtInsurancePlan').autocomplete('option', 'source').filter(function (item) {
                return item.id == $('#' + Patient_Insurance.params.PanelID + ' #hfInsurancePlan').val() && item.searchPattern;
            });

            if (selectedPlan.length > 0) {
                var searchpattern = selectedPlan[0].searchPattern;
                $('#' + Patient_Insurance.params.PanelID + ' #hfInsurancePlansearchPattern').val(searchpattern)
                Patient_Insurance.ValidateInsurancePlanName($('#' + Patient_Insurance.params.PanelID + ' #txtSubscriberID').val(), searchpattern).done(function (response) {
                    if (response.status != false) {
                        if (!response.isValid) {
                            // $(ev).val('');
                            //   $('#' + Patient_Insurance.params.PanelID + ' #lnkInsurancePlanDetail').hide();
                            //  $('#' + Patient_Insurance.params.PanelID + ' #lblInsurancePlan').show();
                            //   $('#' + Patient_Insurance.params.PanelID + ' #hfInsurancePlan').val('-1');
                            $('#' + Patient_Insurance.params.PanelID + ' #txtSubscriberID').focus();
                            //utility.DisplayMessages("Subscriber ID is not valid", 2);

                            var SearchPattern = $('#' + Patient_Insurance.params.PanelID + ' #hfInsurancePlansearchPattern').val();
                            SearchPattern = SearchPattern.replace(/~/g, 'A');
                            SearchPattern = SearchPattern.replace(/#/g, '9');
                            SearchPattern = SearchPattern.replace(/\*/g, 'W');
                            SearchPattern = SearchPattern.replace(/%/g, 'Like');
                            utility.myConfirm('Subscriber ID does not match with the Pattern of insurance plan. Do you want to save it ?', function () {
                                Patient_Insurance.PatientInsuranceSave();
                                return true;
                            }
                            , function () {
                                return false;
                            },
                              'Confirm Pattern'
                              //
                            );

                            //  utility.DisplayMessages("Subscriber Id should be in " + SearchPattern + " Pattern", 3);

                        } else {
                            Patient_Insurance.PatientInsuranceSave();
                            return true;
                        }

                    }
                    else {

                        utility.DisplayMessages(response.Message, 3);
                        return false;
                    }
                });
            }
        }
    },
    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
    isChangeInInsurance: function (mode) {
        if ($('#frmPatientInsurance').serialize() != $('#frmPatientInsurance').data('serialize')) {
            utility.myConfirm('12', function () {

                if (mode != undefined)
                    Patient_Insurance.params.mode = mode;
                Patient_Insurance.PatientInsuranceSave();

            }, function () {
            });
        }
    },

    getProcessedData: function (isNewPlan) {
        $('#' + Patient_Insurance.params.PanelID + ' #overlayMenu').removeClass('open');
        var accessKey = $('#' + Patient_Insurance.params.PanelID + ' #txtAccessKey').val();

        if (typeof (Storage) !== "undefined") {
            localStorage.setItem("AccessKey", accessKey);
        }
        else {
            alert("Local storage not supported.");
        }
        Patient_Insurance.fillProcessedData(accessKey, isNewPlan);
    },
    toggleOverlayMenu: function () {
        $("#" + Patient_Insurance.params.PanelID + ' #overlayMenu').toggleClass('open');
        var accessKey = $('#' + Patient_Insurance.params.PanelID + ' #txtAccessKey').val();
        if (typeof (Storage) !== "undefined") {
            $('#' + Patient_Insurance.params.PanelID + ' #txtAccessKey').val(localStorage.getItem("AccessKey"))
            localStorage.setItem("AccessKey", accessKey);
        }
        else {
            alert("Local storage not supported.");
        }
    },
    fillProcessedData: function (accessKey, isNewPlan) {
        //Access key should be in base64 string format.
        var authinfo = $.base64.encode(accessKey);

        //Accesing the web service..
        $.ajax({
            type: "GET",
            url: "https://cssnwebservices.com/CSSNService/CardProcessor/getmedinsurancedata",
            cache: false,
            contentType: 'application/octet-stream; charset=utf-8;',
            dataType: "json",
            processData: false,
            beforeSend: function (xhr) {
                xhr.setRequestHeader("Authorization", "AccessKey " + authinfo);
            },
            success: function (data) {

                //Converts data to string before parsing
                var medicalCard = JSON.stringify(data);
                medicalCard = $.parseJSON(medicalCard);

                //Display error description if there are errors.
                if (medicalCard.ResponseCodeAuthorization < 0) {
                    utility.DisplayMessages("CSSN Error Code: " + medicalCard.ResponseMessageAuthorization, 3);
                }
                else if (medicalCard.WebResponseCode < 1) {
                    utility.DisplayMessages("CSSN Error Code: " + medicalCard.WebResponseDescription, 3);
                }
                else {

                    //Display data returned by the web service
                    var FirstName = medicalCard.FirstName;
                    var LastName = medicalCard.LastName;
                    var MI = medicalCard.MiddleName;
                    var MemberId = medicalCard.MemberId;
                    var GroupNumber = medicalCard.GroupNumber;
                    var CopayEr = medicalCard.CopayEr;
                    var CopayOv = medicalCard.CopayOv;
                    var CopaySp = medicalCard.CopaySp;
                    var CopayUc = medicalCard.CopayUc;
                    var Coverage = medicalCard.Coverage;
                    var DOB = medicalCard.DateOfBirth;
                    var Deductible = medicalCard.Deductible;
                    var EffectiveDate = medicalCard.EffectiveDate;
                    var Employer = medicalCard.Employer;
                    var ExpirationDate = medicalCard.ExpirationDate;
                    var GroupName = medicalCard.GroupName;
                    var IssuerNumber = medicalCard.IssuerNumber;
                    var Other = medicalCard.Other;
                    var PayerID = medicalCard.PayerId;
                    var PlanAdmin = medicalCard.PlanAdmin;
                    var PlanProvider = medicalCard.PlanProvider;
                    var PlanType = medicalCard.PlanType;
                    var RxBin = medicalCard.RxBin;
                    var RxGroup = medicalCard.RxGroup;
                    var RxId = medicalCard.RxId;
                    var RxPcn = medicalCard.RxPcn;

                    var FullAddress, State, City, Zip, Street;
                    if (medicalCard.ListAddress.length > 0) {
                        FullAddress = medicalCard.ListAddress[0].FullAddress;
                        Street = medicalCard.ListAddress[0].Street;
                        City = medicalCard.ListAddress[0].City;
                        State = medicalCard.ListAddress[0].State;
                        Zip = medicalCard.ListAddress[0].Zip;
                    }

                    var WebAddress;
                    if (medicalCard.ListWeb.length > 0) {
                        WebAddress = medicalCard.ListWeb[0].Value;
                    }

                    var Email;
                    if (medicalCard.ListEmail.length > 0) {
                        Email = medicalCard.ListEmail[0].Value;
                    }

                    var TelePhone;
                    if (medicalCard.ListTelephone.length > 0) {
                        TelePhone = medicalCard.ListTelephone[0].Value;
                    }

                    var Deductible;
                    if (medicalCard.ListDeductible.length > 0) {
                        Deductible = medicalCard.ListDeductible[0].Value;
                    }

                    var PlanCode;
                    if (medicalCard.ListPlanCode.length > 0) {
                        PlanCode = medicalCard.ListPlanCode[0].Value;
                    }

                    //Display reformatted images on UI
                    //Note: Images are returned in base64 string.
                    var base64FrontImage = "data:image/jpg;base64," + medicalCard.Base64FrontReformattedImage;
                    var base64BackImage = "data:image/jpg;base64," + medicalCard.Base64BackReformattedImage;

                    $("#" + Patient_Insurance.params.PanelID + " #txtSubscriberID").val(MemberId);
                    if (isNewPlan) {
                        Patient_Insurance.AddInsurancePlan();
                        $("#" + Patient_Insurance.params.PanelID + " #txtInsurancePlan").val(PlanProvider);

                    }
                    $("#" + Patient_Insurance.params.PanelID + " #txtSubscriberGroupID").val(GroupNumber);
                    $("#" + Patient_Insurance.params.PanelID + " #txtVisitCopayment").val(CopayOv);
                    $("#" + Patient_Insurance.params.PanelID + " #txtSpecialistCopay").val(CopaySp);
                    $("#" + Patient_Insurance.params.PanelID + " #txtFirstName").val(FirstName);
                    $("#" + Patient_Insurance.params.PanelID + " #txtLastName").val(LastName);
                    $("#" + Patient_Insurance.params.PanelID + " #txtMiddleInitial").val(MI);
                    $("#" + Patient_Insurance.params.PanelID + " #dtpCoverageDateFrom").val(EffectiveDate);
                    $("#" + Patient_Insurance.params.PanelID + " #dtpCoverageDateTo").val(ExpirationDate);

                    var c = document.getElementById("myCanvas");
                    var ctx = c.getContext("2d");
                    var imageObj1 = new Image();
                    var imageObj2 = new Image();
                    imageObj1.src = base64FrontImage;
                    //   imageObj1.onload = function () {
                    c.width = imageObj1.width;
                    c.height = imageObj1.height * 2;
                    ctx.drawImage(imageObj1, 0, 0);
                    imageObj2.src = base64BackImage;
                    //  imageObj2.onload = function () {
                    ctx.drawImage(imageObj2, 0, imageObj1.height);
                    var img = c.toDataURL("image/png");
                    $('#' + Patient_Insurance.params.PanelID + ' #image').attr('src', img);
                    $('#' + Patient_Insurance.params.PanelID + ' #backimage').attr('src', img);
                    $('#scanImage #image').css({ "cursor": "pointer" });

                    //document.write('<img src="' + img + '" id="dhuzz" />');
                    // }
                    // };
                    Patient_Insurance.isProcessed = '1';

                }
            },
            error: function (e) {
                utility.DisplayMessages("Error: " + e, 3);
            }
        });
    },

    OpenDocumentScan: function () {
        ScannerLoaded = "ScanShell 800DX";
        AppPrivileges.GetFormPrivileges("Documents", "SCAN", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var pid = $("#frmDemographic #hfPractice").val();
                if (!pid) {
                    pid = $("#" + Patient_Insurance.params["PanelID"] + " #hfPractice").val();
                }
                practiceDetail.DemographicPractice(pid).done(function (response) {
                    if (response.status != false) {
                        var medication_detail = JSON.parse(response.PracticeFill_JSON);
                        ScanPrivilige = medication_detail.chkScan;
                        OCRPrivilige = medication_detail.chkOCR;
                        if (Patient_Demographic.params.PanelID == "pnlDemographic"); {
                            Patient_Demographic.ScanPrivilige = medication_detail.chkScan;
                            Patient_Demographic.OCRPrivilige = medication_detail.chkOCR;
                        }
                    }
                    else {
                        ScanPrivilige = false;
                        OCRPrivilige = false;
                        if (Patient_Demographic.params.PanelID == "pnlDemographic"); {
                            Patient_Demographic.ScanPrivilige = false;
                            Patient_Demographic.OCRPrivilige = false;
                        }
                    }
                    if (ScanPrivilige == "True") {

                        var param = [];
                        var PanelID = null;

                        if (Patient_Insurance.params["PanelID"] != "pnlPatientInsurance")
                            PanelID = Patient_Insurance.params["PanelID"];

                        if (Patient_Insurance.params.ParentCtrl == "demographicDetail" || Patient_Insurance.params.ParentCtrl == "EncounterChargeCapture" || Patient_Insurance.params.ParentCtrl == "appointmentDetail")
                            param["ParentCtrl"] = 'Patient_Insurance';
                        else
                            param["ParentCtrl"] = 'patTabInsurance';

                        param["RefCtrl"] = "patTabInsurance";
                        param["PracticeId"] = pid;

                        Patient_Insurance.params["PreDocument_ScanParams"] = Document_Scan.params;
                        Document_Scan.params = param;
                        //param["ParentCtrl"] = 'patTabInsurance';
                        LoadActionPan('Document_Scanner', param, PanelID);
                    }
                    else {
                        utility.DisplayMessages("Practice does not have the privileges to Scan document. Please contact your administrator", 2);
                    }
                });

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    fillProcessedDataWithAPI: function (isNewPlan, frontImage, backImage) {

        var frontImageBlob;
        var backImagerBlob;
        var frmImageToProcess = new FormData();
        frontImageBlob = utility.basr64URLtoBlob(frontImage);
        backImagerBlob = utility.basr64URLtoBlob(backImage);

        frmImageToProcess.append("backImage", frontImageBlob);
        frmImageToProcess.append("frontImage", backImagerBlob);

        //Accesing the web service..
        $.ajax({
            type: "POST",
            url: "https://cssnwebservices.com/CSSNService/CardProcessor/ProcessMedInsuranceCard/true/0/150/" + "false",
            data: frmImageToProcess,
            cache: false,
            contentType: 'application/octet-stream; charset=utf-8;',
            dataType: "json",
            processData: false,
            beforeSend: function (xhr) {
                BackgroundLoaderShow(true);
                xhr.setRequestHeader("Authorization", "LicenseKey " + $.base64.encode("B3E4DBF19BED"));
            },
            success: function (data) {

                //Converts data to string before parsing
                var medicalCard = JSON.stringify(data);
                medicalCard = $.parseJSON(medicalCard);

                //Display error description if there are errors.
                if (medicalCard.ResponseCodeAuthorization < 0) {
                    utility.DisplayMessages("CSSN Error Code: " + medicalCard.ResponseMessageAuthorization, 3);
                }
                else if (medicalCard.WebResponseCode < 1) {
                    utility.DisplayMessages("CSSN Error Code: " + medicalCard.WebResponseDescription, 3);
                }
                else {
                    try {
                        //Display data returned by the web service
                        var FirstName = medicalCard.FirstName;
                        var LastName = medicalCard.LastName;
                        var MI = medicalCard.MiddleName;
                        var MemberId = medicalCard.MemberId;
                        var GroupNumber = medicalCard.GroupNumber;
                        var CopayEr = medicalCard.CopayEr;
                        if (medicalCard.CopayEr && medicalCard.CopayEr.indexOf('$') > -1) {
                            CopayEr = medicalCard.CopayEr.replace('$', '').trim();
                        }
                        var CopayOv = medicalCard.CopayOv;
                        if (medicalCard.CopayOv && medicalCard.CopayOv.indexOf('$') > -1) {
                            CopayOv = medicalCard.CopayOv.replace('$', '').trim();
                        }
                        var CopaySp = medicalCard.CopaySp;
                        if (medicalCard.CopaySp && medicalCard.CopaySp.indexOf('$') > -1) {
                            CopaySp = medicalCard.CopaySp.replace('$', '').trim();
                        }
                        var CopayUc = medicalCard.CopayUc;
                        if (medicalCard.CopayUc && medicalCard.CopayUc.indexOf('$') > -1) {
                            CopayUc = medicalCard.CopayUc.replace('$', '').trim();
                        }
                        //var CopayEr = medicalCard.CopayEr;
                        //var CopayOv = medicalCard.CopayOv;
                        //var CopaySp = medicalCard.CopaySp;
                        //var CopayUc = medicalCard.CopayUc;
                        var Coverage = medicalCard.Coverage;
                        var DOB = medicalCard.DateOfBirth;
                        var Deductible = medicalCard.Deductible;
                        var EffectiveDate = medicalCard.EffectiveDate;
                        var Employer = medicalCard.Employer;
                        var ExpirationDate = medicalCard.ExpirationDate;
                        var GroupName = medicalCard.GroupName;
                        var IssuerNumber = medicalCard.IssuerNumber;
                        var Other = medicalCard.Other;
                        var PayerID = medicalCard.PayerId;
                        var PlanAdmin = medicalCard.PlanAdmin;
                        var PlanProvider = medicalCard.PlanProvider;
                        var PlanType = medicalCard.PlanType;
                        var RxBin = medicalCard.RxBin;
                        var RxGroup = medicalCard.RxGroup;
                        var RxId = medicalCard.RxId;
                        var RxPcn = medicalCard.RxPcn;

                        var FullAddress, State, City, Zip, Street;
                        if (medicalCard.ListAddress && medicalCard.ListAddress.length > 0) {
                            FullAddress = medicalCard.ListAddress[0].FullAddress;
                            Street = medicalCard.ListAddress[0].Street;
                            City = medicalCard.ListAddress[0].City;
                            State = medicalCard.ListAddress[0].State;
                            Zip = medicalCard.ListAddress[0].Zip;
                        }

                        var WebAddress;
                        if (medicalCard.ListWeb && medicalCard.ListWeb.length > 0) {
                            WebAddress = medicalCard.ListWeb[0].Value;
                        }

                        var Email;
                        if (medicalCard.ListEmail && medicalCard.ListEmail.length > 0) {
                            Email = medicalCard.ListEmail[0].Value;
                        }

                        var TelePhone;
                        if (medicalCard.ListTelephone && medicalCard.ListTelephone.length > 0) {
                            TelePhone = medicalCard.ListTelephone[0].Value;
                        }

                        var Deductible;
                        if (medicalCard.ListDeductible && medicalCard.ListDeductible.length > 0) {
                            Deductible = medicalCard.ListDeductible[0].Value;
                        }

                        var PlanCode;
                        if (medicalCard.ListPlanCode.length > 0) {
                            PlanCode = medicalCard.ListPlanCode[0].Value;
                        }

                        //Display reformatted images on UI
                        //Note: Images are returned in base64 string.

                        //var base64FrontImage = "data:image/jpg;base64," + medicalCard.Base64FrontReformattedImage;
                        //var base64BackImage = "data:image/jpg;base64," + medicalCard.Base64BackReformattedImage;



                        if (isNewPlan) {
                            Patient_Insurance.AddInsurancePlan();
                            $("#" + Patient_Insurance.params.PanelID + " #txtInsurancePlan").val(PlanProvider);
                            $("#pnlPatientInsurance #backimage").show();
                            $("#pnlPatientInsurance #backimage img").attr('src', '');
                        }
                        $('#backimage').hide();
                        $("#" + Patient_Insurance.params.PanelID + " #txtSubscriberID").val(MemberId);
                        $("#" + Patient_Insurance.params.PanelID + " #txtSubscriberGroupID").val(GroupNumber);
                        $("#" + Patient_Insurance.params.PanelID + " #txtVisitCopayment").val(CopayOv);
                        $("#" + Patient_Insurance.params.PanelID + " #txtSpecialistCopay").val(CopaySp);
                        $("#" + Patient_Insurance.params.PanelID + " #txtFirstName").val(FirstName);
                        $("#" + Patient_Insurance.params.PanelID + " #txtLastName").val(LastName);
                        $("#" + Patient_Insurance.params.PanelID + " #txtMiddleInitial").val(MI);
                        $("#" + Patient_Insurance.params.PanelID + " #dtpCoverageDateFrom").val(EffectiveDate);
                        $("#" + Patient_Insurance.params.PanelID + " #dtpCoverageDateTo").val(ExpirationDate);



                        var c = document.getElementById("myCanvas");
                        if (c && c.getContext) {
                            var ctx = c.getContext("2d");
                            var imageObj1 = new Image();
                            var imageObj2 = new Image();
                            imageObj1.onload = function () {
                                //   imageObj1.onload = function () {
                                c.width = imageObj1.width;
                                c.height = imageObj1.height * 2;
                                ctx.drawImage(imageObj1, 0, 0);
                                imageObj2.onload = function () {
                                    //  imageObj2.onload = function () {
                                    ctx.drawImage(imageObj2, 0, imageObj1.height);

                                    var img = c.toDataURL("image/png");

                                    $('#' + Patient_Insurance.params.PanelID + ' #image').attr('src', img);
                                    //   $('#' + Patient_Insurance.params.PanelID + ' #backimage').attr('src', img);
                                    $('#' + Patient_Insurance.params.PanelID + ' #scanImage #image').css({ "cursor": "pointer" });
                                    Patient_Insurance.UnloadDialogue(medicalCard);
                                }
                                imageObj2.src = frontImage;
                            }

                            imageObj1.src = backImage;

                        }

                        //var c = document.getElementById("myCanvas");
                        //var ctx = c.getContext("2d");
                        //var imageObj1 = new Image();
                        //var imageObj2 = new Image();
                        //imageObj1.src = backImage;
                        ////   imageObj1.onload = function () {
                        //c.width = imageObj1.width;
                        //c.height = imageObj1.height * 2;
                        //ctx.drawImage(imageObj1, 0, 0);
                        //imageObj2.src = frontImage;
                        ////  imageObj2.onload = function () {
                        //ctx.drawImage(imageObj2, 0, imageObj1.height);
                        //var img = c.toDataURL("image/png");
                        //$('#' + Patient_Insurance.params.PanelID + ' #image').attr('src', img);
                        //$('#' + Patient_Insurance.params.PanelID + ' #backimage').attr('src', img);
                        //$('#scanImage #image').css({ "cursor": "pointer" });
                        //document.write('<img src="' + img + '" id="dhuzz" />');
                        // }
                        // };
                        //Patient_Insurance.isProcessed = '1';
                        //Document_Scan.UnLoadTab();
                        //BackgroundLoaderShow(false);
                        //Patient_Insurance.ProcessedData = medicalCard;
                        //$("#" + Patient_Insurance.params.PanelID + " #txtSubscriberID").trigger('blur');        
                        //var downKeyEvent = $.Event("keydown");
                        //downKeyEvent.keyCode = $.ui.keyCode.DOWN;  // event for pressing "down" key
                        //var enterKeyEvent = $.Event("keydown");
                        //enterKeyEvent.keyCode = $.ui.keyCode.ENTER;
                        //$("#" + Patient_Insurance.params.PanelID + " #txtInsurancePlan").trigger(downKeyEvent);  // First downkey invokes search
                        //$("#" + Patient_Insurance.params.PanelID + " #txtInsurancePlan").trigger(enterKeyEvent);
                        Patient_Insurance.BindScannedInsurancePlan();
                    }
                    catch (e) {

                    }
                }
            },
            error: function (e) {
                BackgroundLoaderShow(false);
                utility.DisplayMessages("Error: " + e, 3);
            }
        });
    },
    UnloadDialogue: function (medicalCard) {
        Patient_Insurance.isProcessed = '1';
        if (Patient_Insurance.params["IsFromIfram"] = true) {
            Document_Scanner.UnLoadTab();
        }
        else {
            Document_Scan.UnLoadTab();
        }

        BackgroundLoaderShow(false);
        Patient_Insurance.ProcessedData = medicalCard;
        var subscriberID = $("#" + Patient_Insurance.params.PanelID + " #txtSubscriberID").val();
        Patient_Insurance.OpenSubScriberInsurancePlan(null, subscriberID);
        //$("#" + Patient_Insurance.params.PanelID + " #txtSubscriberID").trigger('blur');
        //var downKeyEvent = $.Event("keydown");
        //downKeyEvent.keyCode = $.ui.keyCode.DOWN;  // event for pressing "down" key
        //var enterKeyEvent = $.Event("keydown");
        //enterKeyEvent.keyCode = $.ui.keyCode.ENTER;
        //$("#" + Patient_Insurance.params.PanelID + " #txtInsurancePlan").trigger(downKeyEvent);  // First downkey invokes search
        //$("#" + Patient_Insurance.params.PanelID + " #txtInsurancePlan").trigger(enterKeyEvent);

    },
    UnLoad: function () {
        // when Patient Insurance is open in modal.
        if (!params.IsDemographicInfoGlobalyUpdated && params.DemographicAutoUpdateActiveTab) {
            delete params.DemographicAutoUpdateActiveTab;
        }
        utility.UnLoadDialog('frmPatientInsurance', function () {

            //uploadImage.RemovePicture(demographicDetail.params.PatientId);
            if (Patient_Insurance.params != null && Patient_Insurance.params.ParentCtrl != "") {
                UnloadActionPan(Patient_Insurance.params.ParentCtrl);
                if (Patient_Insurance.params.RefCtrl != null)
                    $("#" + Patient_Insurance.params.RefCtrl).removeClass('disableAll');
            }
            else
                UnloadActionPan();

        }, function () {

            if (Patient_Insurance.params != null && Patient_Insurance.params.ParentCtrl != "") {
                UnloadActionPan(Patient_Insurance.params.ParentCtrl);
                if (Patient_Insurance.params.RefCtrl != null)
                    $("#" + Patient_Insurance.params.RefCtrl).removeClass('disableAll');
                if (Patient_Insurance.params.ParentCtrl == "appointmentDetail") {
                    appointmentDetail.LoadInsurancePlans();
                }
            }
            else
                UnloadActionPan();

        });
    },
    sliceImage: function (base64Stream, firstHalf) {

        firstHalf = true;

        var objDeffered = $.Deferred();
        var base64ImageData = "";
        var img = new Image();
        img.src = "data:image/jpg;base64," + base64Stream;

        img.onload = function () {
            var cropMarginWidth = 5,
                canvas = $('<canvas/>').attr({ width: img.width, height: img.height / 2 }).show().appendTo('body');
            ctx = canvas.get(0).getContext('2d');
            //  a = $('<a id="alirazaawan" download="cropped-image" title="click to download the image" />'),
            sourceImage = {
                x: 0,
                y: firstHalf ? 0 : img.height / 2,
                width: img.width,
                height: img.height / 2,
            };
            OutputImage = {
                x: 0,
                y: 0,
                width: img.width,
                height: img.height / 2,
            };
            //ctx.drawImage(image, sx, sy, sWidth, sHeight, dx, dy, dWidth, dHeight);
            ctx.drawImage(img, sourceImage.x, sourceImage.y, sourceImage.width, sourceImage.height, OutputImage.x, OutputImage.y, OutputImage.width, OutputImage.height);
            base64ImageData = canvas.get(0).toDataURL();

            canvas.remove();
            objDeffered.resolve(base64ImageData);

            // return base64ImageData;
        }
        return objDeffered.promise();
    },
    OpenParticipantProvider: function (Title, RefCtrl, RefCtrlHidden, RefCtrlLabel, RefCtrlLink) {
        var params = [];
        params["Title"] = Title;
        params["RefCtrl"] = RefCtrl;
        params["RefCtrlHidden"] = RefCtrlHidden;
        params["RefCtrlLabel"] = RefCtrlLabel;
        params["RefCtrlLink"] = RefCtrlLink;
        params["IsOptional"] = false;
        params["RefForm"] = "frmPatientInsurance";
        if (Patient_Insurance.params["PanelID"] == "pnldemographicDetail") {
            params["ProviderId"] = $("#pnldemographicDetail #frmdemographicDetail").find("#hfProvider").val();
            params["ProviderName"] = $("#pnldemographicDetail #frmdemographicDetail").find("#txtProvider").val();
        } else {
            params["ProviderId"] = $("#pnlDemographic #frmDemographic").find("#hfProvider").val();
            params["ProviderName"] = $("#pnlDemographic #frmDemographic").find("#txtProvider").val();
        }
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Patient_Insurance';
        LoadActionPan('Admin_ParticipentProvider', params);
    },
    FillPhoneNumber: function (ctrl) {
        var selectedValue = $(ctrl).val();
        var PhoneNo = $('#' + Patient_Insurance.params.PanelID).find("#ddlPlanAddress option:selected").attr("refvalue");
        if (selectedValue != "" && PhoneNo != "") {
            $('#' + Patient_Insurance.params.PanelID).find("#txtTelephone").val(PhoneNo);
        }
        else {
            $('#' + Patient_Insurance.params.PanelID).find("#txtTelephone").val("");
        }
    },
    checkUncheckAll: function (obj, tableId) {
        $('#' + tableId + ' input[id*="chkDetailDialogBox"]').prop("checked", $(obj).prop("checked"));
        Patient_Insurance.EnableDisableUpdateInsuranceButton(tableId);
    },
    EnableDisableUpdateInsuranceButton: function (tableId) {
        if ($('#' + tableId + ' input[id*="chkDetailDialogBox"]').length == $('#' + tableId + ' input[id*="chkDetailDialogBox"]:checked').length) {
            $(".modal-footer").find("#btnUpdate").removeClass("disableAll");
            $('#' + tableId + ' input[id="chkMasterDialogBox"]').prop("checked", true);
        }
        else if ($('#' + tableId + ' input[id*="chkDetailDialogBox"]:checked').length < $('#' + tableId + ' input[id*="chkDetailDialogBox"]').length && $('#' + tableId + ' input[id*="chkDetailDialogBox"]:checked').length > 0) {
            $(".modal-footer").find("#btnUpdate").removeClass("disableAll");
            $('#' + tableId + ' input[id="chkMasterDialogBox"]').prop("checked", false);
        }
        else {
            $(".modal-footer").find("#btnUpdate").addClass("disableAll");
        }
    },
    UpdateInsuranceInScedular: function () {
        Patient_Insurance.InsAppointmentsArray = [];
        $('#PatientFutureAppointment input[id*="chkDetailDialogBox"]').each(function (i, item) {
            if ($(this).context.checked) {
                var AppointmentId = $(this).context.value;
                if (Patient_Insurance.InsAppointmentsArray.indexOf(AppointmentId) == -1) {
                    Patient_Insurance.InsAppointmentsArray.push(AppointmentId);
                }
                if (Patient_Insurance.params.ParentCtrl == "appointmentDetail" && appointmentDetail.params.AppointmentId == AppointmentId) {
                    appointmentDetail.params.InsurancePlanLinkedId = Patient_Insurance.params.PatientInsuranceId;
                }
            }
        });
        var AppointmentsString = Patient_Insurance.InsAppointmentsArray.join();
        Patient_Insurance.PatientSchedularUpdate(AppointmentsString, Patient_Insurance.params.PatientInsuranceId);
    },
    PatientSchedularUpdate: function (appointmentstring, insuranceId) {
        Patient_Insurance.UpdatePatientSchedular(appointmentstring, insuranceId).done(function (response) {
            if (response.status != false) {
                //  Patient_Insurance.params.PatientInsuranceId = response.PatientInsuranceId;
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    UpdatePatientSchedular: function (PatientAppointmentData, PatientInsuranceID) {
        var data = "PatientAppointmentData=" + PatientAppointmentData + "&PatientInsuranceID=" + PatientInsuranceID;
        return MDVisionService.defaultService(data, "PATIENT_INSURANCE", "UPDATE_INSURANCE_IN_SCHEDULAR");
    },
    ShowFutureAppointments: function (data, IsFromCopayUpdateAlert) {
        var rendomKey = utility.makeRendomKey();
        utility.makeFuncArray(rendomKey, null, null);
        var tableId = 'PatientFutureAppointment';
        var dialogTitle = 'Do you want to update the appointment(s) with the New Insurance Plan?';
        var onClickUpdateFunc = 'Patient_Insurance.UpdateInsuranceInScedular()';
        var CopayColumnTH = '';
        var onClickCancelFunc = '';
        var DialogBoxSize = 'modal-dialog-md';
        if (IsFromCopayUpdateAlert) {
            tableId = 'CopayWithAppointment';
            dialogTitle = 'Do you want to update the Copay with the appointment(s)?';
            CopayColumnTH = '<th class="noWordBreak no-sort">Copay Type</th><th class="no-sort">Copay</th>';
            onClickUpdateFunc = 'Patient_Insurance.UpdateAppointmentCopay()';
            onClickCancelFunc = 'Patient_Insurance.CancelAppointmentCopay()';
            DialogBoxSize = 'modal-dialog-lg';
        }

        if (data != null && data.length > 0) {
            if ($.fn.dataTable.isDataTable("#" + tableId)) {
                $("#" + tableId).dataTable().fnClearTable();
                $("#" + tableId).dataTable().fnDestroy();
            }

            $("#" + tableId + " tbody").empty();
            var markUp = '<div id="modal-from-dom" class="modal fade">' +
                           '<div class="modal-dialog ' + DialogBoxSize + ' modal-top-adjust">' +
                           '<div class="modal-content">' +
                           '<div class="modal-header">' +
                              '<label class="noWordBreak" style="font-size:14px;"><strong><span style="font-size:14px;color:#ff0000;">Alert! </span>' + dialogTitle + '</strong></label>' +
                           '</div>' +
                           '<div class="modal-body bg-white table-responsive Of-a" >' +
                            '<table id="' + tableId + '" class="table table-bordered table-striped table-condensed table-hover mb-none">' +
                            '<thead>' +
                            '<tr>' +
                            '<th><input type="checkbox" id="chkMasterDialogBox" onclick="Patient_Insurance.checkUncheckAll(this,\'' + tableId + '\');"></th>' +
                            '<th class="no-sort">Date</th>' +
                            '<th class="no-sort">Time</th>' +
                            '<th class="noWordBreak no-sort">Insurance Plan</th>' +
                            '<th class="no-sort">Provider</th>' +
                            '<th class="no-sort">Facility</th>' + CopayColumnTH +
                            '</tr>' +
                            '</thead>' +
                            '<body>';
            $(data).each(function (i, item) {
                markUp += '<tr>';
                markUp += '<td><input type="checkbox" id="chkDetailDialogBox' + item.AppointmentId + '" value = "' + item.AppointmentId + '" onchange="Patient_Insurance.EnableDisableUpdateInsuranceButton(\'' + tableId + '\')"  ></td>';
                markUp += '<td class="noWordBreak">' + utility.DateTemplate(item.Date) + '</td>';
                markUp += '<td class="noWordBreak">' + item.Time + '</td>';
                markUp += '<td>' + item.InsurancePlan + '</td>';
                markUp += '<td class="noWordBreak">' + item.Provider + '</td>';
                markUp += '<td class="noWordBreak">' + item.Facility + '</td>';
                if (IsFromCopayUpdateAlert) {
                    markUp += '<td>' + item.CopayType + '</td><td>' + (utility.IsNullOrEmptyString(item.Copay) == true ? "" : item.Copay) + '</td>';
                }
                markUp += '</tr>';
            });
            markUp += '</body>';
            markUp += '</table>';
            markUp += '</div>';
            markUp += '<div class="modal-footer">';
            markUp += '<a href="#" id="btnUpdate"   data-dismiss="modal" class="btn btn-success disableAll " onclick="' + onClickUpdateFunc + '" >Update</a>';
            markUp += '<a href="#" id="btnCancel"  data-dismiss="modal" class="btn btn-danger" onclick="' + onClickCancelFunc + '" >Cancel</a>';
            markUp += '</div>';
            markUp += '</div> <div></div>';

            $(markUp).modal({
                show: 'true',
                backdrop: 'static',
                keyboard: false
            }).on('shown.bs.modal', function () {
                $('#btnCancel', this).focus();
                if ($.fn.dataTable.isDataTable("#" + tableId))
                    ;
                else
                    $("#" + tableId).DataTable({ "bInfo": false, "bPaginate": false, "bFilter": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "orderable" : false, "aTargets": [0, 'no-sort'] }] });
            }).on('hidden.bs.modal', function () {
                if ($('body').find('.modal-backdrop').length > 0) {
                    if ($('body').css('overflow').toLowerCase() != "scroll") {
                        $('body').addClass('modal-open');
                    }
                    else {
                        $('body').addClass('modal-open');
                    }
                }
            });
            $('#' + tableId + ' input[id*="chkDetailDialogBox"]').attr('checked', false);
        }
    },
    CheckEligiblity: function (response) {
        //IMP-873
        if (Patient_Insurance.IsLoadEligiblitySc != false) {
            Patient_Insurance.AppointmentDetails = response.PatientFutureAppointment;
            if (response.PatientFutureAppointment != null && response.PatientFutureAppointment.length > 0) {
                Patient_Insurance.OpenPatientEligibility(true);
            }
        }
    },

    PatientReferralAlertDisplay: function (insuranceId, PatientId, InsuranceResponse) {
        Patient_Insurance.DisplayAlertReferralPatient(insuranceId, PatientId).done(function (response) {
            if (response.status != false) {
                if (response.IsPatientReffralALert == '1') {
                    utility.myConfirm('Referral is required for this insurance. Do you want to add?', function () {
                        Patient_Insurance.AddReferral();
                        $("#" + Patient_Insurance.params.PanelID + ' #divPatientReferrals .toggle-content').css('display', 'block')
                        $("#" + Patient_Insurance.params.PanelID + ' #divPatientReferrals section').addClass('active');
                    }
               , function () {
                   Patient_Insurance.CheckEligiblity(InsuranceResponse);
               }, 'Alert! referral required');
                }
            } // end if Is Patient Refrral Alert. 
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    DisplayAlertReferralPatient: function (PatientInsuranceID, PatientId) {
        var data = "PatientInsuranceID=" + PatientInsuranceID + "&PatientId=" + PatientId;
        return MDVisionService.defaultService(data, "PATIENT_INSURANCE", "PATIENT_REFERRAL_ALERT");
    },
    showCopayChangeAlert: function () {
        var currSpecialistCopayValue = $('#' + Patient_Insurance.params["PanelID"] + ' #txtSpecialistCopay').val();
        var currVistCopayValue = $('#' + Patient_Insurance.params["PanelID"] + ' #txtVisitCopayment').val();
        if ((currSpecialistCopayValue != Patient_Insurance.oldSpecialistCopayValue) || (currVistCopayValue != Patient_Insurance.oldVisitCopayValue))
            Patient_Insurance.ShowFutureAppointments(Patient_Insurance.AppointmentDetailsForCopay, true);
        else
            Patient_Insurance.LoadAfterSaveUpdate(Patient_Insurance.InsuranceSaveUpdateResponse, Patient_Insurance.params.mode);
    },
    UpdateAppointmentCopay: function () {
        Patient_Insurance.InsAppointmentsArray = [];
        $('#CopayWithAppointment input[id*="chkDetailDialogBox"]').each(function (i, item) {
            if ($(this).context.checked) {
                var AppointmentId = $(this).context.value;
                if (Patient_Insurance.InsAppointmentsArray.indexOf(AppointmentId) == -1) {
                    Patient_Insurance.InsAppointmentsArray.push(AppointmentId);
                }
            }
        });
        var AppointmentIds = Patient_Insurance.InsAppointmentsArray.join();
        var objData = new Object();
        objData["AppointmentIds"] = AppointmentIds;
        objData["InsuranceId"] = Patient_Insurance.params.PatientInsuranceId;
        var PatientAppointmentData = JSON.stringify(objData);

        Patient_Insurance.UpdateAppointmentCopay_DBCall(PatientAppointmentData).done(function (response) {
            if (response.status == true) {
                Patient_Insurance.LoadAfterSaveUpdate(Patient_Insurance.InsuranceSaveUpdateResponse, Patient_Insurance.params.mode);
            }
            else
                utility.DisplayMessages(response.Message, 3);
        });
    },
    UpdateAppointmentCopay_DBCall: function (PatientAppointmentData) {
        var data = "PatientAppointmentData=" + PatientAppointmentData;
        return MDVisionService.defaultService(data, "PATIENT_INSURANCE", "UPDATE_APPOINTMENT_COPAY");
    },
    CancelAppointmentCopay: function () {
        Patient_Insurance.LoadAfterSaveUpdate(Patient_Insurance.InsuranceSaveUpdateResponse, Patient_Insurance.params.mode);
    },
    PrintInsurance: function () {
        var params = [];
        params["InsuranceId"] = Patient_Insurance.params.PatientInsuranceId;
        params["InsuranceJSON"] = $('#pnlPatientInsurance').getMyJSONByName();
        params["ParentCtrl"] = 'Patient_Insurance';
        params["PreviewPrintFor"] = "Insurance";
        LoadActionPan('Patient_Demographic_PrintView', params, Patient_Insurance.params.PanelID);
    },
}