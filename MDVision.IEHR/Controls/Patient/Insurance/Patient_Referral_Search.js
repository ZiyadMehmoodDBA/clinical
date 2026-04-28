patientReferralSearch = {
    params: [],
    Load: function (params) {
        patientReferralSearch.params = params;
        patientReferralSearch.LoadReferrals();

        //patientReferralSearch.params["PanelID"] = patientReferralSearch.params["PanelID"] + ' #patientReferralSearch';

    },

    // -------------- Patint Referral ---------------------
    LoadReferrals: function () {
        if ($("#patientReferralSearch #pnlReferralSearch_Result").css("display") == "none") {
            $("#patientReferralSearch #pnlReferralSearch_Result").show();
        }

        if (patientReferralSearch.params.ParentCtrl && ($(patientReferralSearch.params.ParentCtrl.indexOf("EncounterChargeCapture") >= 0) || $(patientReferralSearch.params.ParentCtrl.indexOf("appointmentDetail") >= 0))) {
            $("#patientReferralSearch #btnAddReferral").css("display", "none");
        }

        if (patientReferralSearch.params.ReferralResponse != null) {
            patientReferralSearch.ReferralGridLoad(patientReferralSearch.params.ReferralResponse);
            patientReferralSearch.params.ReferralResponse = null;
        }
        else {
            if (patientReferralSearch.params.patientID == "" || patientReferralSearch.params.patientInsuranceID == undefined) {
                patientReferralSearch.SearchReferral(0, patientReferralSearch.params.patientInsuranceID, patientReferralSearch.params.ProviderId, patientReferralSearch.params.FacilityId, patientReferralSearch.params.ReferralDate, patientReferralSearch.params.Active).done(function (response) {
                    if (response.status != false) {
                        patientReferralSearch.ReferralGridLoad(response);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            } else {
                patientReferralSearch.SearchReferral(patientReferralSearch.params.ReferralType, patientReferralSearch.params.patientInsuranceID, patientReferralSearch.params.ProviderId, patientReferralSearch.params.FacilityId, patientReferralSearch.params.ReferralDate, patientReferralSearch.params.Active).done(function (response) {
                    if (response.status != false) {
                        patientReferralSearch.ReferralGridLoad(response);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }

    },

    ReferralGridLoad: function (response) {
        $("#dgvReferralSearch").dataTable().fnDestroy();
        $("#pnlReferralSearch_Result #dgvReferralSearch tbody").find("tr").remove();
        if (response.ReferralCount > 0) {
            var ReferralLoadJSONData = JSON.parse(response.ReferralLoad_JSON);
            $.each(ReferralLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", "gvReferral_row" + item.PatientReferralId);
                $row.attr("ReferralId", item.PatientReferralId);
                let ToDate = item.ToDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '')
                var disableclass = "";
                if (item.IsActive == "True") {
                    isactive = 0;
                    tglclass = "fa fa-toggle-on green";
                    $row.attr("onclick", "patientReferralSearch.SelectReferral('" + utility.decodeHtml(item.ReferralAuthNo) + "','" + item.ReferringFromName + "','" + item.PatientReferralId + "','" + item.VisitsAllowed + "','" + item.VisitsUsed + "','" + item.ReferringFromId + "','" + item.FacilityToId + "','" + item.ProviderId + "',event,'" + ToDate + "');");
                }
                else {
                    isactive = 1;
                    tglclass = "fa fa-toggle-on red";
                    disableclass = "disableAll";
                }



                //MethodMode = "patientReferralSearch.SelectReferral(" + item.ReferralAuthNo.trim() + ',' +  item.ReferringFromName.trim() + ");";

                $row.append('<td style="display:none;">' + item.PatientReferralId + '</td><td>&nbsp;<a class="btn btn-xs ' + disableclass + '" href="#" onclick="patientReferralSearch.SelectReferral(\'' + utility.decodeHtml(item.ReferralAuthNo) + '\', \'' + item.ReferringFromName + '\',\'' + item.PatientReferralId + '\', \'' + item.VisitsAllowed + '\',\'' + item.VisitsUsed + '\',\'' + item.ReferringFromId + '\',\'' + item.FacilityToId + '\',\'' + item.ProviderId + '\',event,\'' + ToDate + '\');" title="Select Record"><i class="fa fa-check black"></i></a><a class="btn btn-xs" href="#" onclick="patientReferralSearch.ReferralDelete(\'' + item.PatientReferralId + '\',\'' + utility.decodeHtml(item.ReferralAuthNo) + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="patientReferralSearch.ReferralEdit(\'' + item.PatientReferralId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="patientReferralSearch.ReferralActiveInactive(\'' + item.PatientReferralId + '\', ' + isactive + ',\'' + utility.decodeHtml(item.ReferralAuthNo) + '\',event);" title="Inactive Record"><i class="' + tglclass + '"></i></a></td><td>' + item.ReferralType + '</td><td>' + item.ReferringFromName + '</td><td>' + item.FromDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + item.ToDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + utility.decodeHtml(item.ReferralAuthNo) + '</td><td>' + item.VisitsAllowed + '</td><td>' + item.VisitsUsed + '</td>');

                $("#pnlReferralSearch_Result #dgvReferralSearch tbody").last().append($row);
            });
        }
        else {
            $('#pnlReferralSearch_Result #dgvReferralSearch').DataTable({
                "language": {
                    "emptyTable": "No Referral Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#pnlReferralSearch_Result #dgvReferralSearch'))
            ;
        else {
            $("#pnlReferralSearch_Result #dgvReferralSearch").DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] });
        }

    },

    AddReferral: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Referral Management", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ReferralID"] = "-1";
                params["mode"] = "Add";
                params["ParentCtrl"] = 'patientReferralSearch';
                params["patientID"] = patientReferralSearch.params.patientID;
                params["patientInsuranceID"] = patientReferralSearch.params.patientInsuranceID;
                params["ReferralType"] = patientReferralSearch.params.ReferralType;
                params["ProviderId"] = patientReferralSearch.params.ProviderId;
                params["FacilityId"] = patientReferralSearch.params.FacilityId;
                params["ReferralDate"] = patientReferralSearch.params.ReferralDate;
                params["Active"] = patientReferralSearch.params.Active;
                LoadActionPan('Patient_Referral', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ReferralEdit: function (ReferralId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($("#gvReferral_row" + ReferralId));
        AppPrivileges.GetFormPrivileges("Referral Management", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = ReferralId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["ReferralID"] = selectedValue;
                    params["ParentCtrl"] = 'patientReferralSearch';
                    params["patientID"] = patientReferralSearch.params.patientID;
                    params["patientInsuranceID"] = patientReferralSearch.params.patientInsuranceID;
                    params["ReferralType"] = patientReferralSearch.params.ReferralType;
                    params["ProviderId"] = patientReferralSearch.params.ProviderId;
                    params["FacilityId"] = patientReferralSearch.params.FacilityId;
                    params["ReferralDate"] = patientReferralSearch.params.ReferralDate;
                    params["Active"] = patientReferralSearch.params.Active;
                    params["mode"] = "Edit";
                    LoadActionPan('Patient_Referral', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ReferralDelete: function (ReferralId, ReferralAuthNo, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($("#gvReferral_row" + ReferralId));
        AppPrivileges.GetFormPrivileges("Referral Management", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = ReferralId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        patientReferralSearch.DeleteReferral(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#dgvReferralSearch').DataTable();
                                table1.row('.active').remove().draw(false);
                                patientReferralSearch.LoadReferrals('0');
                                utility.DisplayMessages(response.Message, 1);

                                if (patientReferralSearch.params.ParentCtrl == "appointmentDetail") {
                                    if (ReferralAuthNo.trim() == $('#appointmentDetail #txtReferralNo').val().trim()) {
                                        $('#appointmentDetail #txtReferralNo').val('');
                                    }
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
        });
    },

    ReferralActiveInactive: function (ReferralId, IsActive, ReferralAuthNo, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Referral Management", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = ReferralId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        patientReferralSearch.UpdateReferralActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                patientReferralSearch.LoadReferrals('0');
                                if (patientReferralSearch.params.ParentCtrl == "appointmentDetail") {
                                    if (ReferralAuthNo.trim() == $('#appointmentDetail #txtReferralNo').val().trim()) {
                                        $('#appointmentDetail #txtReferralNo').val('');
                                    }
                                }
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

    UnLoad: function () {

        if (patientReferralSearch.params != null && patientReferralSearch.params.ParentCtrl) {
            UnloadActionPan(patientReferralSearch.params.ParentCtrl, "patientReferralSearch");
            patientReferralSearch.params = null;
        }
        else {
            UnloadActionPan(null, "patientReferralSearch");
            //Patient_Search.params = null;
            var CurrentMasterTab = GetCurrentMasterTab();
            if (CurrentMasterTab.TabID == "mstrTabPatient" && PatientArray.length <= 0) {
                $("#actionPanPatient").find('div').first().hide('blind', 500, function () { $(this).remove() });
                SelectTab("mstrTabDashBoard");
            }
        }
    },

    //SearchReferral: function () {
    //    var data = "patientInsuranceID=" + patientReferralSearch.params.patientInsuranceID;
    //    // serach parameter , class name, command name of class
    //    return MDVisionService.defaultService(data, "PATIENT_REFERRAL", "SEARCH_REFERRAL");
    //},

    SearchReferral: function (ReferralType, patientInsuranceID, ProviderId, FacilityId, ReferralDate, IsActive,isFromSchedular) {

        if (ReferralType == null) {
            ReferralType = "";
        }

        if (patientInsuranceID == null) {
            patientInsuranceID = "";
        }
        if (ProviderId == null) {
            ProviderId = "";
        }
        if (FacilityId == null) {
            FacilityId = "";
        }
        if (ReferralDate == null) {
            ReferralDate = "";
        }
        if (IsActive == null) {
            IsActive = "";
        }

        var isFromAppointment = "";
        try {
            if (isFromSchedular || (patientReferralSearch && patientReferralSearch.params["ParentCtrl"] == 'appointmentDetail')) {
                isFromAppointment = true;
            }
        }
        catch (ex) {
            console.log(ex);
        }
       
        var data = "PatientInsuranceID=" + patientInsuranceID + "&ReferralType=" + ReferralType + "&FacilityToId=" + FacilityId + "&ReferringToId=" + ProviderId + "&ReferringDate=" + ReferralDate + "&IsActive=" + IsActive + "&IsFromAppointment=" + isFromAppointment;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_REFERRAL", "SEARCH_REFERRAL");
    },

    SelectReferral: function (ReferralNo, ReferringProvider, ReferralId, RefVisitAllowed, RefVisitUsed, ReferringFromId, FacilityToId, ProviderId, event, ToDate) {
        if (event != null) {
            event.stopPropagation();
        }

        if (patientReferralSearch.params.ParentCtrl == "appointmentDetail") {
            let appointmentDate = new Date($("#frmappointmentDetail #txtScheduleDate").val())
            var strToDate = new Date(ToDate)
            if (appointmentDate <= strToDate || !ToDate) {


                if (patientReferralSearch.params.ProviderId != ProviderId || patientReferralSearch.params.FacilityToId != FacilityToId) {

                    utility.myConfirm('Provider/Facility mismatch with the selected referral. Do you want to continue?', function () {

                        patientReferralSearch.SetReferral(ReferralNo, ReferringProvider, ReferralId, RefVisitAllowed, RefVisitUsed, ReferringFromId);

                    }, function () {
                        UnloadActionPan(patientReferralSearch.params["ParentCtrl"], "patientReferralSearch");
                    },
                    'Patient Referral');
                }
                else {
                    patientReferralSearch.SetReferral(ReferralNo, ReferringProvider, ReferralId, RefVisitAllowed, RefVisitUsed, ReferringFromId);
                }
            }
            else {
                utility.myConfirm('62', function () {
                    if (patientReferralSearch.params.ProviderId != ProviderId || patientReferralSearch.params.FacilityToId != FacilityToId) {

                        utility.myConfirm('Provider/Facility mismatch with the selected referral. Do you want to continue?', function () {

                            patientReferralSearch.SetReferral(ReferralNo, ReferringProvider, ReferralId, RefVisitAllowed, RefVisitUsed, ReferringFromId);

                        }, function () {
                            UnloadActionPan(patientReferralSearch.params["ParentCtrl"], "patientReferralSearch");
                        },
                        'Patient Referral');
                    }
                    else {
                        patientReferralSearch.SetReferral(ReferralNo, ReferringProvider, ReferralId, RefVisitAllowed, RefVisitUsed, ReferringFromId);
                    }
                });
            }
        }
        else if (patientReferralSearch.params.ParentCtrl == "EncounterChargeCapture") {
            var DOSFrom = new Date($("#pnlEncounterChargeCapture #dtpDOSFrom").val());
            var strToDate = new Date(ToDate);
            if (DOSFrom > strToDate) {
                utility.myConfirm('62', function () {
                    patientReferralSearch.SetReferral(ReferralNo, ReferringProvider, ReferralId, RefVisitAllowed, RefVisitUsed, ReferringFromId);
                });
            }
            else
                patientReferralSearch.SetReferral(ReferralNo, ReferringProvider, ReferralId, RefVisitAllowed, RefVisitUsed, ReferringFromId);
        }
        else {
            patientReferralSearch.SetReferral(ReferralNo, ReferringProvider, ReferralId, RefVisitAllowed, RefVisitUsed, ReferringFromId);
        }

        if (patientReferralSearch.params.ParentCtrl == "appointmentDetail") {
            appointmentDetail.BindRefProvider();
        }
    },

    SetReferral: function (ReferralNo, ReferringProvider, ReferralId, RefVisitAllowed, RefVisitUsed, ReferringFromId) {
        var RefCtrlReferralNo = " #txtReferralNo";
        var RefCtrlRefProvider = " #txtRefProvider";
        var RefHiddenIdCtrl = " #hfReferralNumerId";
        var RefProvHiddenFieldId = " #hfRefProvider";
        //var RefHiddenIdCtrl = " #hfFacility";
        if (patientReferralSearch.params["RefCtrlReferralNo"] != null) {
            RefCtrlReferralNo = " #" + patientReferralSearch.params["RefCtrlReferralNo"];
        }
        if (patientReferralSearch.params["RefCtrlRefProvider"] != null) {
            RefCtrlRefProvider = " #" + patientReferralSearch.params["RefCtrlRefProvider"];
        }
        if (patientReferralSearch.params["RefHiddenIdCtrl"] != null) {
            RefHiddenIdCtrl = " #" + patientReferralSearch.params["RefHiddenIdCtrl"];
        }

        //Edited by Azeem Raza Tayyab on 12-Apr-2016 to fix bug#:PMS-4851
        if (RefVisitUsed && RefVisitAllowed && Number(RefVisitUsed) < Number(RefVisitAllowed)) {
            $('#' + patientReferralSearch.params["PanelID"] + RefCtrlReferralNo).val(ReferralNo).focus();
            $('#' + patientReferralSearch.params["PanelID"] + RefHiddenIdCtrl).val(ReferralId);
        } else {
            $('#' + patientReferralSearch.params["PanelID"] + RefCtrlReferralNo).val("");
            $('#' + patientReferralSearch.params["PanelID"] + RefHiddenIdCtrl).val("");
            utility.DisplayMessages("Allowed visits are used", 3);

        }

        $('#' + patientReferralSearch.params["PanelID"] + RefCtrlRefProvider).val(ReferringProvider);
        $('#' + patientReferralSearch.params["PanelID"] + RefProvHiddenFieldId).val(ReferringFromId);
        utility.SetKendoAutoCompleteSourceforValidate($('#' + patientReferralSearch.params["PanelID"] + RefCtrlRefProvider), $('#' + patientReferralSearch.params["PanelID"] + RefCtrlRefProvider).val(), $('#' + patientReferralSearch.params["PanelID"] + RefProvHiddenFieldId), $('#' + patientReferralSearch.params["PanelID"] + RefProvHiddenFieldId).val());
        UnloadActionPan(patientReferralSearch.params["ParentCtrl"], "patientReferralSearch");
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

}