Patient_PreAuthorization = {
    params: [],
    bIsFirstLoad: true,
    Load: function (params) {
        Patient_PreAuthorization.params = params;
        if (Patient_PreAuthorization.bIsFirstLoad) {
            Patient_PreAuthorization.bIsFirstLoad = false;
        }
        Patient_PreAuthorization.LoadPreAuthorizations();
        //Patient_PreAuthorization.FillPatientAccount(Patient_PreAuthorization.params.patientID);
    },
    PreAuthorizationAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Authorization", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["PreAuthorizationId"] = null;
                params["PatientID"] = Patient_PreAuthorization.params.patientID;
                params["mode"] = "Add";
                params["ParentCtrl"] = 'Patient_PreAuthorization';
                params["PlanResponse"] = Patient_PreAuthorization.params.PlanResponse;
                LoadActionPan('Patient_PreAuthorization_Detail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    PreAuthorizationEdit: function (AuthorizeId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($("#gvPreAuthorization_row" + AuthorizeId));
        AppPrivileges.GetFormPrivileges("Authorization", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["PreAuthorizationId"] = AuthorizeId;
                params["PatientID"] = Patient_PreAuthorization.params.patientID;
                params["mode"] = "Edit";
                params["ParentCtrl"] = 'Patient_PreAuthorization';
                params["PlanResponse"] = Patient_PreAuthorization.params.PlanResponse;
                LoadActionPan('Patient_PreAuthorization_Detail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },


    LoadPreAuthorizations: function () {
        if (Patient_PreAuthorization.params.ParentCtrl && Patient_PreAuthorization.params.ParentCtrl.indexOf("EncounterChargeCapture") >= 0) {
            $("#pnlPreAuthorization #btnAppointmentStatusAdd").css("display", "none");
        }
        AppPrivileges.GetFormPrivileges("Authorization", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlPreAuthorization #pnlPreAuthorization_Result").css("display") == "none") {
                    $("#pnlPreAuthorization #pnlPreAuthorization_Result").show();
                }
                Patient_PreAuthorization.PreAuthorizationLoad(Patient_PreAuthorization.params.patientID).done(function (response) {
                    if (response.status != false) {
                        Patient_PreAuthorization.params["PatientPreAuthorizationCount"] = response.PatientPreAuthorizationCount;
                        Patient_PreAuthorization.PreAuthorizationGridLoad(response);
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



    PreAuthorizationGridLoad: function (response) {
        $("#pnlPreAuthorization_Result #dgvPreAuthorizations").dataTable().fnDestroy();
        $("#pnlPreAuthorization_Result #dgvPreAuthorizations tbody").find("tr").remove();
        var firstPreAuthorizationId = "";
        if (response.PatientPreAuthorizationCount > 0) {
            var AuthorizationLoadJSONData = JSON.parse(response.PatientPreAuthorizationLoad_JSON);
            $.each(AuthorizationLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                var selectMethod = "";
                if (Patient_PreAuthorization.params.ParentCtrl && Patient_PreAuthorization.params.ParentCtrl == "EncounterChargeCapture") {
                    selectMethod = 'Patient_PreAuthorization.PreAuthorizationSelect(\'' + item.AuthorizeId + '\', \'' + item.PAN + '\', \'' + item.VisitsAllowed + '\',\'' + item.VisitsUsed + '\',event);';
                    $row.attr("onclick", selectMethod);
                    selectMethod = '<a class="btn btn-xs" href="#" onclick="' + selectMethod + '" title="Select Record"><i class="fa fa-check black"></i></a>&nbsp;';
                }
                else {
                    $row.attr("onclick", "Patient_PreAuthorization.PreAuthorizationEdit('" + item.AuthorizeId + "',event);");
                }
                $row.attr("id", "gvPreAuthorization_row" + item.AuthorizeId);
                $row.attr("AuthorizationId", item.AuthorizeId);
                //To Load First Family in Edit Mode
                if (firstPreAuthorizationId == "") {
                    firstPreAuthorizationId = item.AuthorizeId;
                }
                if (item.IsActive == "True") {
                    isactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }
                var CptCodeDetail = ((item.CPTCode == "") ? "" : item.CPTCode) + ((item.Description == "") ? "" : '-' + item.Description);
                $row.append('<td style="display:none;">' + item.AuthorizeId + '</td><td>' + selectMethod + '<a class="btn  btn-xs" href="#" onclick="Patient_PreAuthorization.DeletePreAuthorization(\'' + item.AuthorizeId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Patient_PreAuthorization.PreAuthorizationEdit(\'' + item.AuthorizeId + '\',event);"  title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Patient_PreAuthorization.ActiveInactivePreAuthorization(\'' + item.AuthorizeId + '\', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.InsurancePlanName + '</td><td>' + CptCodeDetail + '</td><td>' + item.FromDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + item.ToDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + item.PAN + '</td><td>' + item.VisitsAllowed + '</td>');

                $("#pnlPreAuthorization_Result #dgvPreAuthorizations tbody").last().append($row);
            });
        }
        else {

            $('#pnlPreAuthorization_Result #dgvPreAuthorizations').DataTable({
                "language": {
                    "emptyTable": "No Authorization Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
            utility.ClearFormValidation('#frmPreAuthorization', true);
            Patient_PreAuthorization.params["mode"] = "Add";
            $("#pnlPreAuthorization #chkActive").prop("checked", true);
            //Patient_PreAuthorization.ValidateFamily();

        }
        if ($.fn.dataTable.isDataTable('#pnlPreAuthorization_Result #dgvPreAuthorizations'))
            ;
        else
            $("#pnlPreAuthorization_Result #dgvPreAuthorizations").DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown


    },

    DeletePreAuthorization: function (PreAuthorizationId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($("#gvPreAuthorization_row" + PreAuthorizationId));
        AppPrivileges.GetFormPrivileges("Authorization", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = PreAuthorizationId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Patient_PreAuthorization.PreAuthorizationDelete(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#pnlPreAuthorization_Result #dgvPreAuthorizations').DataTable();
                                table1.row('.active').remove().draw(false);
                                Patient_PreAuthorization.LoadPreAuthorizations();
                                utility.DisplayMessages(response.Message, 1);
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

    ActiveInactivePreAuthorization: function (PreAuthorizationId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Authorization", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = PreAuthorizationId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Patient_PreAuthorization.PreAuthorizationUpdateActiveInactive(PreAuthorizationId, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Patient_PreAuthorization.LoadPreAuthorizations();
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


    PreAuthorizationDelete: function (PreAuthorizationID) {
        var data = "PreAuthorizationID=" + PreAuthorizationID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_PREAUTHORIZATION", "DELETE_PREAUTHORIZATION");
    },

    PreAuthorizationUpdateActiveInactive: function (PreAuthorizationId, IsActive) {
        var data = "PatientID=" + Patient_PreAuthorization.params.patientID + "&PreAuthorizationId=" + PreAuthorizationId + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_PREAUTHORIZATION", "UPDATE_PREAUTHORIZATION_ACTIVE_INACTIVE");
    },

    PreAuthorizationLoad: function (patientID) {
        var data = "PatientID=" + patientID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_PREAUTHORIZATION", "LOAD_PREAUTHORIZATION");
    },

    UnLoad: function () {

        if (Patient_PreAuthorization.params != null && Patient_PreAuthorization.params.ParentCtrl != null) {
            var selectedTab = GetCurrentSelectedTab();
            var parentPanelID = selectedTab.PanelID;
            UnloadActionPan(Patient_PreAuthorization.params.ParentCtrl, 'Patient_PreAuthorization', null, parentPanelID);
        }
        else
            UnloadActionPan(null, 'Patient_PreAuthorization');

    },

    PreAuthorizationSelect: function (AuthorizeId, PAN, RefVisitAllowed, RefVisitUsed, event) {
        if (event != null) {
            event.stopPropagation();
        }

        var RefCtrlPAN = " #txtPAN";
        var RefHiddenIdCtrl = " #hfAuthorizeId";

        //var RefHiddenIdCtrl = " #hfFacility";
        if (Patient_PreAuthorization.params["RefCtrlPAN"] != null) {
            RefCtrlPAN = " #" + Patient_PreAuthorization.params["RefCtrlPAN"];
        }

        if (Patient_PreAuthorization.params["RefHiddenIdCtrl"] != null) {
            RefHiddenIdCtrl = " #" + Patient_PreAuthorization.params["RefHiddenIdCtrl"];
        }
        //Edited by Azeem Raza Tayyab on 12-Apr-2016 to fix bug#:PMS-4851
        var visitUsed = RefVisitUsed == "" ? "0" : RefVisitUsed;
        if (visitUsed && RefVisitAllowed && Number(RefVisitUsed) < Number(RefVisitAllowed)) {
            $('#' + Patient_PreAuthorization.params["PanelID"] + RefCtrlPAN).val(PAN).focus();
            $('#' + Patient_PreAuthorization.params["PanelID"] + RefHiddenIdCtrl).val(AuthorizeId);
        } else {
            $('#' + Patient_PreAuthorization.params["PanelID"] + RefCtrlPAN).val("");
            $('#' + Patient_PreAuthorization.params["PanelID"] + RefHiddenIdCtrl).val("");
            utility.DisplayMessages("Allowed visits are used", 3);

        }
        UnloadActionPan(Patient_PreAuthorization.params["ParentCtrl"], "Patient_PreAuthorization");
    },


}