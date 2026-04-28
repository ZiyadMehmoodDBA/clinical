Patient_Referral = {
    params: [],
    Load: function (params) {
        Patient_Referral.params = params;
        if (Patient_Referral.params.PanelID != null && Patient_Referral.params.PanelID != "pnlPatientReferral") {
            Patient_Referral.params.PanelID += " #pnlPatientReferral";
        }
        var self = $("#" + Patient_Referral.params.PanelID);

        self.loadDropDowns(true).done(function () {

            //////////$("#" + Patient_Referral.params.PanelID + " #frmPatientReferral #ddlFacilityFrom").autocomplete();


            //charges are made for  http://192.168.0.16:8080/browse/PMS-2927 to save load time by Arsalan.
            //set ddlFacilityFrom source from ddlFacilityTo source to save a server call because both have same fields.
            //var $options = $(self).find("#ddlFacilityTo > option").clone();
            //$(self).find('#ddlFacilityFrom').append($options);

            //CacheManager.BindDropDownsByID("#pnlPatientReferral #ddlInsurancePlan", 'GetPatientInsurance', true, Patient_Referral.params.patientID).done(function () {

            //    if (self.find("#ddlInsurancePlan option").length == 2) {
            //        $(self.find("#ddlInsurancePlan option")[1]).prop('selected', true);
            //    }
            //    Patient_Referral.LoadReferral();
            //});

            CacheManager.BindDropDownsByID("#pnlPatientReferral #ddlInsurancePlan", 'GetPatientInsurance', true, Patient_Referral.params.patientID).done(function () {

                if (self.find("#ddlInsurancePlan option").length == 2) {
                    $(self.find("#ddlInsurancePlan option")[1]).prop('selected', true);
                }

                setTimeout(function () {
                    //var $options = $("#" + Patient_Referral.params.PanelID + " #frmPatientReferral").find("#ddlFacilityTo > option").clone();
                    //$("#" + Patient_Referral.params.PanelID + " #frmPatientReferral").find('#ddlFacilityFrom').append($options);

                    // set OutGoing Controls
                    var $self = $("#" + Patient_Referral.params.PanelID + " #divReferringFromTo");
                    var $options_from = $($self).find("select[ddlist=GetProvider] > option").clone();
                    var $options_to = $($self).find("select[ddlist=GetRefProviders] > option").clone();
                    $("#" + Patient_Referral.params.PanelID + " #ddlOutgoingReferringFrom").append($options_from);
                    $("#" + Patient_Referral.params.PanelID + " #ddlOutgoingReferringTo").append($options_to);

                    $($self).find("#ddlOutgoingReferringFrom").attr("ddlist", "GetProvider");
                    $($self).find("#ddlOutgoingReferringTo").attr("ddlist", "GetRefProviders");
                    Patient_Referral.LoadReferral();
                }, 1000);

            });

        });

        Patient_Referral.ValidatePatientReferral();
        Patient_Referral.BindRefProvider('ddlReferringFrom', 'hfIncomingRefProviderFrom');
        Patient_Referral.BindRefProvider('ddlReferringTo', 'hfIncomingfRefProviderTo');
        Patient_Referral.BindRefProvider('ddlOutgoingReferringFrom', 'hfOutgoingRefProviderFrom');
        Patient_Referral.BindRefProvider('ddlOutgoingReferringTo', 'hfOutgoingRefProviderTo');
        Patient_Referral.BindFacility('ddlFacilityFrom', 'hfFromFacility');
        Patient_Referral.BindFacility('ddlFacilityTo', 'hfToFacility');
    },
    ChangeValues: function (obj) {
        var ReferralType = $(obj).val();
        if (ReferralType.toLowerCase() == "incoming") {
            $("#" + Patient_Referral.params.PanelID + " #divReferringFromTo").show();
            $("#" + Patient_Referral.params.PanelID + " #divOutgoingReferringFromTo").hide();
        }
        else if (ReferralType.toLowerCase() == "outgoing") {
            $("#" + Patient_Referral.params.PanelID + " #divReferringFromTo").hide();
            $("#" + Patient_Referral.params.PanelID + " #divOutgoingReferringFromTo").show();
        }
    },
    SetReferringFromTo: function (obj, referringFrom, referringTo) {
        var ReferralType = $(obj).val();
        if (ReferralType != "") {

            Patient_Referral.ChangeValues(obj);

            if (referringFrom != null) {
                if (utility.UserBrowser().toLowerCase() == "safari") {
                    $("#" + Patient_Referral.params.PanelID + " #ddlReferringFrom option[selected=selected]").removeAttr("selected")
                    $("#" + Patient_Referral.params.PanelID + " #ddlReferringFrom option[value='" + referringFrom + "']").prop("selected", true);

                    $("#" + Patient_Referral.params.PanelID + " #ddlOutgoingReferringFrom option[selected=selected]").removeAttr("selected")
                    $("#" + Patient_Referral.params.PanelID + " #ddlOutgoingReferringFrom option[value='" + referringFrom + "']").prop("selected", true);
                }
                else {
                    $("#" + Patient_Referral.params.PanelID + " #ddlReferringFrom option[value='" + referringFrom + "']").attr("selected", "selected");
                    $("#" + Patient_Referral.params.PanelID + " #ddlOutgoingReferringFrom option[value='" + referringFrom + "']").attr("selected", "selected");
                }

            }
            if (referringTo != null) {
                if (utility.UserBrowser().toLowerCase() == "safari") {
                    $("#" + Patient_Referral.params.PanelID + " #ddlReferringTo option[selected=selected]").removeAttr("selected")
                    $("#" + Patient_Referral.params.PanelID + " #ddlReferringTo option[value='" + referringTo + "']").prop("selected", true);

                    $("#" + Patient_Referral.params.PanelID + " #ddlOutgoingReferringTo option[selected=selected]").removeAttr("selected")
                    $("#" + Patient_Referral.params.PanelID + " #ddlOutgoingReferringTo option[value='" + referringTo + "']").prop("selected", true);
                }
                else {
                    $("#" + Patient_Referral.params.PanelID + " #ddlReferringTo option[value='" + referringTo + "']").attr("selected", "selected");
                    $("#" + Patient_Referral.params.PanelID + " #ddlOutgoingReferringTo option[value='" + referringTo + "']").attr("selected", "selected");
                }


            }
        }
        else {
            $("#" + Patient_Referral.params.PanelID + " #ddlReferringFrom,#ddlReferringTo,#ddlOutgoingReferringFrom,#ddlOutgoingReferringTo").each(function () {
                $(this).attr("disabled", "disabled");
            });
        }
    },

    LoadReferral: function () {
        AppPrivileges.GetFormPrivileges("Referral Management", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (Patient_Referral.params.mode == "Add") {                            
                    if (Patient_Referral.params.patientInsuranceID != null) {
                        $("#" + Patient_Referral.params.PanelID + " #ddlInsurancePlan option").each(function () {
                            if ($(this).val() == Patient_Referral.params.patientInsuranceID) {
                                $(this).attr("selected", "selected");
                                return true;
                            }
                        });
                    }

                    if (Patient_Referral.params.ReferralType != null) {
                        $("#" + Patient_Referral.params.PanelID + " #ddlReferralType option").each(function () {
                            if ($(this).val().toLowerCase() == Patient_Referral.params.ReferralType.toLowerCase()) {
                                $(this).attr("selected", "selected");
                                return true;
                            }
                        });
                    }

                    if (Patient_Referral.params.FacilityId != null) {
                        $("#" + Patient_Referral.params.PanelID + " #ddlFacilityTo option").each(function () {
                            if ($(this).val() == Patient_Referral.params.FacilityId) {
                                $(this).attr("selected", "selected");
                                return true;
                            }
                        });
                    }
                    if (Patient_Referral.params.PatientInsuranceId) {
                        $("#" + Patient_Referral.params.PanelID + " #ddlInsurancePlan").val(Patient_Referral.params.PatientInsuranceId);
                    }


                    //serialize data
                    $("#frmPatientReferral").data("serialize", $("#frmPatientReferral").serialize());

                }
                else if (Patient_Referral.params.mode == "Edit") {
                    Patient_Referral.FillReferral(Patient_Referral.params.ReferralID).done(function (response) {
                        if (response.status != false) {
                            var ReferralDetail = JSON.parse(response.ReferralFill_JSON);
                            var self = $("#pnlPatientReferral");
                            utility.bindMyJSON(true, ReferralDetail, false, self).done(function () {
                                Patient_Referral.SetReferringFromTo($("#pnlPatientReferral #ddlReferralType"), ReferralDetail.ddlReferringFrom, ReferralDetail.ddlReferringTo)
                                // utility.bindMyJSON(true, ReferralDetail, false, self);
                                if (ReferralDetail.chkActive == 'True')
                                    $("#pnlPatientReferral #chkActive").attr("checked", true);
                                else
                                    $("#pnlPatientReferral #chkActive").attr("checked", false);

                                //make Visits Used disable
                                $("#pnlPatientReferral #txtVisitsUsed").attr("disabled", "disabled");
                                
                                Patient_Referral.intializeAutoCompleteDataSource('ddlReferringFrom', 'hfIncomingRefProviderFrom');
                                Patient_Referral.intializeAutoCompleteDataSource('ddlReferringTo', 'hfIncomingfRefProviderTo');
                                Patient_Referral.intializeAutoCompleteDataSource('ddlOutgoingReferringFrom', 'hfOutgoingRefProviderFrom');
                                Patient_Referral.intializeAutoCompleteDataSource('ddlOutgoingReferringTo', 'hfOutgoingRefProviderTo');
                                Patient_Referral.intializeAutoCompleteDataSource('ddlFacilityFrom', 'hfFromFacility');
                                Patient_Referral.intializeAutoCompleteDataSource('ddlFacilityTo', 'hfToFacility');
                               
                                //serialize data
                                $("#frmPatientReferral").data("serialize", $("#frmPatientReferral").serialize());
                            });

                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            }
            else {
                utility.DisplayMessages(strMessage, 3);
            }
        });
    },

    ValidatePatientReferral: function () {
        $("#frmPatientReferral")
        .bootstrapValidator({
            live: 'disabled',
            message: 'This value is not valid',
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {
                ReferringFrom: {
                    group: '.col-sm-3',
                    validators: {
                        notEmpty: {
                            message: ''
                        },
                    }
                },
                //ReferringTo: {
                //    group: '.col-sm-3',
                //    validators: {
                //        notEmpty: {
                //            message: ''
                //        },
                //    }
                //},
                OutgoingReferringFrom: {
                    group: '.col-sm-3',
                    validators: {
                        notEmpty: {
                            message: ''
                        },
                    }
                },
                //OutgoingReferringTo: {
                //    group: '.col-sm-3',
                //    validators: {
                //        notEmpty: {
                //            message: ''
                //        },
                //    }
                //},
                ReferralType: {
                    group: '.col-sm-3',
                    validators: {
                        notEmpty: {
                            message: ''
                        },
                    }
                },
                InsurancePlan: {
                    group: '.col-sm-3',
                    validators: {
                        notEmpty: {
                            message: ''
                        },
                    }
                },
                ReferralAuthNo: {
                    group: '.col-sm-3',
                    validators: {
                        notEmpty: {
                            message: ''
                        },
                    }
                },
            }
        })
        .on('success.form.bv', function (e) {
            Patient_Referral.ReferralSave();
        });
    },

    ReferralSave: function () {
        var strMessage = "";
        var self = $("#pnlPatientReferral");
        // set Patient Id
        if (Patient_Referral.params.patientID)
            $("#" + Patient_Referral.params.PanelID + " #hfPatientID").val(Patient_Referral.params.patientID);
        var myJSON = self.getMyJSON();
        if (Patient_Referral.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Referral Management", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Patient_Referral.SaveReferral(myJSON, Patient_Referral.params.PatientInsuranceId).done(function (response) {
                        if (response.status != false) {
                            Patient_Referral.params.ReferralID = response.PatientReferralId;
                            //adnan maqbool , pms-3903, 12/02/2016
                            $('#pnlPatientReferral #dtpFromDate').trigger('blur');
                            $('#pnlPatientReferral #dtpToDate').trigger('blur');
                            //end
                            utility.DisplayMessages(response.message, 1);
                            //if (Patient_Referral.params.ParentCtrl != null && Patient_Referral.params.ParentCtrl == "patientReferralSearch") {
                            //    patientReferralSearch.LoadReferrals();
                            //    UnloadActionPan(Patient_Referral.params.ParentCtrl, 'Patient_Referral');
                            //}
                            //else if (Patient_Referral.params.ParentCtrl != null && Patient_Referral.params.ParentCtrl == "Patient_Insurance") {
                            //    Patient_Insurance.LoadReferrals();
                            //    UnloadActionPan(Patient_Referral.params.ParentCtrl, 'Patient_Referral');
                            //}
                            //else if (Patient_Referral.params.ParentCtrl != null && (Patient_Referral.params.ParentCtrl.indexOf("EncounterChargeCapture") >= 0 || Patient_Referral.params.ParentCtrl.indexOf("appointmentDetail") >= 0)) {
                            //    UnloadActionPan(Patient_Referral.params.ParentCtrl, 'Patient_Referral');
                            //}
                            //else {
                            //    Patient_Insurance.LoadReferrals();
                            //    UnloadActionPan(null, 'Patient_Insurance');
                            //}
                            Patient_Insurance.LoadReferrals();
                            $("#frmPatientReferral").data("serialize", $("#frmPatientReferral").serialize());
                            Patient_Referral.UnLoad();
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
        else if (Patient_Referral.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Referral Management", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Patient_Referral.UpdateReferral(myJSON, Patient_Referral.params.ReferralID).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            //adnan maqbool , pms-3903, 12/02/2016
                            if ($('#pnlPatientReferral #dtpFromDate').text() != null && $('#pnlPatientReferral #dtpFromDate').text().length > 0)
                                $('#pnlPatientReferral #dtpFromDate').trigger('blur');

                            if ($('#pnlPatientReferral #dtpToDate').text() != null && $('#pnlPatientReferral #dtpToDate').text().length > 0)
                                $('#pnlPatientReferral #dtpToDate').trigger('blur');
                            //end
                            //if (Patient_Referral.params.ParentCtrl != null && Patient_Referral.params.ParentCtrl == "patientReferralSearch") {
                            //    patientReferralSearch.LoadReferrals();
                            //    UnloadActionPan(Patient_Referral.params.ParentCtrl, 'Patient_Referral');
                            //}
                            //else if (Patient_Referral.params.ParentCtrl != null && Patient_Referral.params.ParentCtrl == "Patient_Insurance") {
                            //    Patient_Insurance.LoadReferrals();
                            //    UnloadActionPan(Patient_Referral.params.ParentCtrl, 'Patient_Referral');
                            //}
                            //else if (Patient_Referral.params.ParentCtrl != null && (Patient_Referral.params.ParentCtrl.indexOf("EncounterChargeCapture") >= 0 || Patient_Referral.params.ParentCtrl.indexOf("appointmentDetail") >= 0)) {
                            //    UnloadActionPan(Patient_Referral.params.ParentCtrl, 'Patient_Referral');
                            //}
                            //else {
                            //    Patient_Insurance.LoadReferrals();
                            //    UnloadActionPan(null, 'Patient_Insurance');
                            //}
                            if (Patient_Referral.params.ParentCtrl != null && Patient_Referral.params.ParentCtrl == "patientReferralSearch")
                                patientReferralSearch.LoadReferrals();
                            else
                                Patient_Insurance.LoadReferrals();

                            $("#frmPatientReferral").data("serialize", $("#frmPatientReferral").serialize());
                            Patient_Referral.UnLoad();
                            //UnloadActionPan();
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
    },

    FillReferral: function (PatientReferralID) {
        var data = "PatientReferralID=" + PatientReferralID;
        return MDVisionService.defaultService(data, "PATIENT_REFERRAL", "FILL_REFERRAL");
    },

    SaveReferral: function (ReferralData, PatientInsuranceId) {
        var data = "ReferralData=" + ReferralData + "&PatientInsuranceId=" + PatientInsuranceId;
        return MDVisionService.defaultService(data, "PATIENT_REFERRAL", "SAVE_REFERRAL");
    },

    UpdateReferral: function (ReferralData, PatientReferralID) {
        var data = "ReferralData=" + ReferralData + "&PatientReferralID=" + PatientReferralID;
        return MDVisionService.defaultService(data, "PATIENT_REFERRAL", "UPDATE_REFERRAL");
    },

    //DeleteReferral: function (ReferralID) {
    //    var data = "ReferralID=" + ReferralID;
    //    return MDVisionService.defaultService(data, "PATIENT_REFERRAL", "DELETE_REFERRAL");
    //},


    BindFacility: function (controlId, hiddenFieldId) {
        var Ctrl = $("#" + Patient_Referral.params.PanelID + " #frmPatientReferral #" + controlId);
        var func = function () { return utility.GetFacilityArray(Ctrl.val()) };
        var hfCtrl = $("#" + Patient_Demographic.params.PanelID + " #frmPatientReferral #" + hiddenFieldId);
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },
    BindRefProvider: function (controlId, hiddenFieldId) {
        var Ctrl = $("#" + Patient_Referral.params.PanelID + " #frmPatientReferral #" + controlId);
        var func = function () { return utility.GetRefProviderArray(Ctrl.val()) };
        var hfCtrl = $("#" + Patient_Referral.params.PanelID + " #frmPatientReferral #" + hiddenFieldId);
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    BindProvider: function (controlId, hiddenFieldId, isFullName, shortName) {

        if (!shortName)
            shortName = $("#" + Patient_Referral.params.PanelID + " #frmPatientReferral #" + controlId).val();

        utility.GetProviderArray(shortName).done(function (response) {

            $("#" + Patient_Referral.params.PanelID + " #frmPatientReferral #" + controlId).autocomplete({
                autoFocus: true,
                source: response,
                select: function (event, ui) {
                    setTimeout(function () {

                        $("#" + Patient_Demographic.params.PanelID + " #frmPatientReferral #" + controlId).val(ui.item.value);
                        $("#" + Patient_Demographic.params.PanelID + " #frmPatientReferral #" + hiddenFieldId).val(ui.item.id);
                    }, 100);
                }
            });
        });
    },
    OpenFacility: function (controlId, hiddenFieldId) {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmPatientReferral";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Patient_Referral";
        params["RefCtrl"] = controlId;
        params["RefHiddenIdCtrl"] = hiddenFieldId;
        LoadActionPan('Admin_Facility', params);
    },

    OpenRefProvider: function (controlId, hiddenFieldId, IsOptional) {
        var params = [];
        params["ReferringProviderIdReferral"] = "-1";
        params["FromAdmin"] = "0";
        params["RefForm"] = "frmPatientReferral";
        params["IsOptional"] = IsOptional;
        params["RefCtrl"] = controlId;
        params["ParentCtrl"] = "Patient_Referral";
        params["RefCtrlHidden"] = hiddenFieldId;
        LoadActionPan('Admin_ReferringProvider', params);
    },
    OpenProvider: function (controlId, hiddenFieldId) {
        var params = [];
        params["ReferringProviderIdReferral"] = "-1";
        params["FromAdmin"] = "0";
        params["RefForm"] = "frmPatientReferral";
        params["IsOptional"] = false;
        params["RefCtrl"] = controlId;
        params["ParentCtrl"] = "Patient_Referral";
        params["RefCtrlHidden"] = hiddenFieldId;
        LoadActionPan('Admin_Provider', params);
    },
    UnLoad: function () {
        
        if (Patient_Referral.params != null && Patient_Referral.params.ParentCtrl != null) {

            utility.UnLoadDialog('frmPatientReferral', function () {
                // Start 15/02/2016 Muhammad Irfan for bug #  PMS-3886

                if (Patient_Referral.params.ParentCtrl == 'Patient_Insurance' || Patient_Referral.params.ParentCtrl == 'patTabInsurance') {
                    UnloadActionPan(Patient_Referral.params.ParentCtrl, 'Patient_Referral', null, 'pnlPatientInsurance');
                    if (Patient_Insurance.IsLoadEligiblitySc != false) {
                       // PMS - 4388
                        setTimeout(function () { Patient_Insurance.OpenPatientEligibility(true); }, 1000);

                    }
                }
                else if (Patient_Referral.params.ParentCtrl == 'appointmentDetail') {
                    UnloadActionPan(Patient_Referral.params.ParentCtrl, 'Patient_Referral', null, 'appointmentDetail');

                } else if (Patient_Referral.params.ParentCtrl.indexOf("EncounterChargeCapture") >= 0) {
                    UnloadActionPan(Patient_Referral.params.ParentCtrl, 'Patient_Referral');
                    //UnloadActionPan(Patient_Referral.params.ParentCtrl, 'Patient_Referral', null, 'pnlEncounterChargeCapture');
                } else if (Patient_Referral.params.ParentCtrl == 'patientReferralSearch') {
                    UnloadActionPan(Patient_Referral.params.ParentCtrl, 'Patient_Referral', null, 'patientReferralSearch');
                } else {
                    UnloadActionPan(Patient_Referral.params.ParentCtrl, 'Patient_Referral');
                }
                //PMS-4388
                if (!Patient_Insurance.IsLoadEligiblitySc != false)
                Patient_Referral.params = null;
            }, function () {
                if (Patient_Referral.params.ParentCtrl == 'Patient_Insurance' || Patient_Referral.params.ParentCtrl == 'patTabInsurance') {
                    UnloadActionPan(Patient_Referral.params.ParentCtrl, 'Patient_Referral', null, 'pnlPatientInsurance');
                    if (Patient_Insurance.IsLoadEligiblitySc != false) {
                        //PMS-4388
                        setTimeout(function () { Patient_Insurance.OpenPatientEligibility(true); }, 1000);
                        
                    }
                }
                else if (Patient_Referral.params.ParentCtrl == 'appointmentDetail') {
                    UnloadActionPan(Patient_Referral.params.ParentCtrl, 'Patient_Referral', null, 'appointmentDetail');

                } else if (Patient_Referral.params.ParentCtrl && Patient_Referral.params.ParentCtrl.indexOf("EncounterChargeCapture") >= 0) {
                    UnloadActionPan(Patient_Referral.params.ParentCtrl, 'Patient_Referral');
                    //UnloadActionPan(Patient_Referral.params.ParentCtrl, 'Patient_Referral', null, 'pnlEncounterChargeCapture');
                } else if (Patient_Referral.params.ParentCtrl == 'patientReferralSearch') {
                    UnloadActionPan(Patient_Referral.params.ParentCtrl, 'Patient_Referral', null, 'patientReferralSearch');
                } else {
                    UnloadActionPan(Patient_Referral.params.ParentCtrl, 'Patient_Referral');
                }
                if (!Patient_Insurance.IsLoadEligiblitySc != false)
                  Patient_Referral.params = null;
                // End 15/02/2016 Muhammad Irfan for bug #  PMS-3886
            });

        }
        else {

            utility.UnLoadDialog('frmPatientReferral', function () {
                UnloadActionPan(null, 'Patient_Insurance');
            }, function () {
                UnloadActionPan(null, 'Patient_Insurance');
            });

        }

      
        
    },
    intializeAutoCompleteDataSource: function ($ctr, $hfctr) {
        var $ctrl = $("#" + Patient_Referral.params.PanelID + " #frmPatientReferral #" + $ctr);
        var $hfctrl = $("#" + Patient_Referral.params.PanelID + " #frmPatientReferral #" + $hfctr);
        utility.SetKendoAutoCompleteSourceforValidate($ctrl, $ctrl.val(), $hfctrl, $hfctrl.val());
    },
}