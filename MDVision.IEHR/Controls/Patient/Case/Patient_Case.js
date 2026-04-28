Patient_Case = {
    bIsFirstLoad: true,
    params: [],
    Load: function (paramerters) {
        Patient_Case.params = paramerters;

        if (Patient_Case.params.PanelID != null && Patient_Case.params.PanelID != 'pnlPatientCase') {
            Patient_Case.params["PanelID"] = Patient_Case.params["PanelID"] + ' #pnlPatientCase';
        }
        else {
            Patient_Case.params["PanelID"] = 'pnlPatientCase';
        }

        if (!Patient_Case.params.patientID) {
            $('#' + Patient_Case.params["PanelID"] + " #btnPatientCaseAdd").css("display", "none");
        }

        if (Patient_Case.bIsFirstLoad) {
            Patient_Case.bIsFirstLoad = false;
            var self = $('#' + Patient_Case.params["PanelID"]);
            self.loadDropDowns(true);
            if (Patient_Case.params["ParentCtrl"] == "billTabPaymentPosting") {
                $('#' + Patient_Case.params["PanelID"] + " #divAccount").css("display", "block");
                $('#' + Patient_Case.params["PanelID"] + " #divClaimNumber").css("display", "block");
                //$('#' + Patient_Case.params["PanelID"] + " #DivPatientInsuranceAutoComplete").css("display", "inline");
                $('#' + Patient_Case.params["PanelID"] + " #DivPatientInsurance").css("display", "none");
                //CacheManager.BindDropDownsByID("#" + Patient_Case.params["PanelID"] + ' #ddlPatientInsuranceId', 'GetPatientInsurance', true, "-1");
            }
            else {
                $('#' + Patient_Case.params["PanelID"] + " #divAccount").css("display", "none");
                $('#' + Patient_Case.params["PanelID"] + " #divClaimNumber").css("display", "none");
                //$('#' + Patient_Case.params["PanelID"] + " #DivPatientInsuranceAutoComplete").css("display", "none");
                $('#' + Patient_Case.params["PanelID"] + " #DivPatientInsurance").css("display", "inline");
                CacheManager.BindDropDownsByID("#" + Patient_Case.params["PanelID"] + ' #ddlPatientInsuranceId', 'GetPatientInsurance', true, Patient_Case.params.patientID);

            }

            if (Patient_Case.params["ParentCtrl"] && Patient_Case.params["ParentCtrl"].indexOf("EncounterChargeCapture") > -1) {
                $('#' + Patient_Case.params["PanelID"] + " #btnPatientCaseAdd").addClass("hidden");
            }
            Patient_Case.BindFacility();
            Patient_Case.BindReferralProvider();
            Patient_Case.BindClaimNumber();
            Patient_Case.BindPatientAccount();
            // Patient_Demographic.FillPatientInfo(Patient_Case.params);
        }

        utility.CreateDatePicker('pnlPatientCase #dtpCalledDate,#pnlPatientCase #dtpEntryDate', function () {
            //  on-change callback method
        }, true);

        Patient_Case.SearchPatientCase();

        if (params.PreviousTab.TabID == "patTabDemographic") {
            Patient_Demographic.isChangeInDemographic(Patient_Demographic.params.mode);
        }
        else if (params.PreviousTab.TabID == "patTabInsurance") {
            Patient_Insurance.isChangeInInsurance(Patient_Insurance.params.mode);
        }


    },

    BindFacility: function () {
        var Ctrl = $("#" + Patient_Case.params.PanelID + " #frmPatientCase #txtFacility");
        var func = function () { return utility.GetFacilityArray(Ctrl.val()) };
        var hfCtrl = $("#" + Patient_Case.params.PanelID + " #frmPatientCase #hfFacility");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    BindReferralProvider: function () {
        var Ctrl = $("#" + Patient_Case.params.PanelID + " #frmPatientCase #txtRefProvider");
        var hfCtrl = $("#" + Patient_Case.params["PanelID"] + " #hfProvider");
        var func = function () { return utility.GetRefProviderArray(Ctrl.val()) };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    //OpenInsurancePlan: function () {
    //    params = [];
    //    params["InsurancePlanId"] = "-1";
    //    params["FromAdmin"] = "0";
    //    params["ParentCtrl"] = "patTabCaseManagement";
    //    LoadActionPan('Admin_InsurancePlan', params);
    //},

    //OpenInsurancePlanDetail: function () {
    //    params = [];
    //    params["InsurancePlanId"] = $('#' + Patient_Case.params["PanelID"] + " #hfInsurancePlan").val();
    //    params["mode"] = "Edit";
    //    params["FromAdmin"] = "0";
    //    params["ParentCtrl"] = "patTabCaseManagement";
    //    LoadActionPan('insurancePlanDetail', params);
    //},

    //FillInsurancePlanName: function (InsurancePlanId, InsurancePlanName) {
    //    $('#' + Patient_Case.params["PanelID"] + " #txtInsurancePlan").val(InsurancePlanName);
    //    $('#' + Patient_Case.params["PanelID"] + " #hfInsurancePlan").val(InsurancePlanId);
    //    $('#' + Patient_Case.params["PanelID"] + " #lnkInsurancePlanDetail").css("display", "inline");
    //    $('#' + Patient_Case.params["PanelID"] + " #lblInsurancePlan").css("display", "none");
    //    UnloadActionPan(Admin_InsurancePlan.params["ParentCtrl"]);
    //    //Patient_Insurance.FillInsurancePlanAddress(InsurancePlanId);
    //},

    //OpenProvider: function () {
    //    params = [];
    //    params["ProviderId"] = "-1";
    //    params["FromAdmin"] = "0";
    //    params["ParentCtrl"] = "patTabCaseManagement";
    //    LoadActionPan('Admin_Provider', params);
    //},

    // -------------- Ref Provider -----------------

    FillRefProviderName: function (RefProviderId, RefProviderName) {
        $('#pnlPatientCase #txtRefProvider').val(RefProviderName);
        $('#pnlPatientCase #hfProvider').val(RefProviderId);
        UnloadActionPan(Admin_ReferringProvider.params["ParentCtrl"]);
    },

    OpenRefProvider: function (Title, RefCtrl, RefCtrlHidden, RefCtrlLabel, RefCtrlLink) {

        var params = [];
        var PanelID = null;
        if (Patient_Case.params["PanelID"] != "pnlPatientCase")
            PanelID = Patient_Case.params["PanelID"] + " #pnlPatientCase";
        else
            PanelID = Patient_Case.params["PanelID"];
        params["Title"] = Title;
        params["RefCtrl"] = RefCtrl;
        params["RefCtrlHidden"] = RefCtrlHidden;
        params["RefCtrlLabel"] = RefCtrlLabel;
        params["RefCtrlLink"] = RefCtrlLink;
        params["ReferringProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Patient_Case";
        LoadActionPan('Admin_ReferringProvider', params);
    },

    OpenRefProviderDetail: function () {

        var params = [];
        params["ReferringProviderId"] = $('#' + Patient_Case.params["PanelID"] + ' #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtRefProvider";
        params["ParentCtrl"] = 'Patient_Case';
        LoadActionPan('referringproviderDetail', params);
    },

    OpenPatientSearch: function () {
        var params = [];
        params["FromAdmin"] = "0";
        // params["RefCtrl"] = "txtPatientName";
        params["ParentCtrl"] = 'Patient_Case';
        LoadActionPan('Patient_Search', params);
    },

    FillPatientInfoFromSearch: function (PatientId, AccountNo, event) {
        if (event != null) {
            event.stopPropagation();
        }
        $("#" + Patient_Case.params["PanelID"] + " #hfPatientId").val(PatientId);
        $("#" + Patient_Case.params["PanelID"] + " #txtPatientName").val(AccountNo);
        utility.SetKendoAutoCompleteSourceforValidate($("#" + Patient_Case.params["PanelID"] + " #txtPatientName"), AccountNo, $("#" + Patient_Case.params["PanelID"] + " #hfPatientId"), PatientId);
        var ParentCtrl = 'Patient_Case';
        UnloadActionPan(ParentCtrl);
        utility.InsertRecentPatient(PatientId);
    },
    OpenEncounter: function () {
        var params = [];
        params["FromAdmin"] = 0;
        //if (Bill_PaymentPosting.params.TabID == 'billTabPaymentBatchSearch' || Bill_PaymentPosting.params.TabID == 'paymentBatchDetail')
        //    params["ParentCtrl"] = 'Bill_PaymentPosting';
        //else
        //    params["ParentCtrl"] = Bill_PaymentPosting.params["TabID"];
        params["ParentCtrl"] = 'Patient_Case';
        if ($("#" + Patient_Case.params.PanelID + " #txtPatientName").val().trim() == "")
            params["patientID"] = 0;
        else
            params["patientID"] = Number($('#' + Patient_Case.params.PanelID + ' #hfPatientId').val());
        LoadActionPan('Encounter_Visits', params);
        //$('#CloseVisits').remove();
        // $('#OpenVisits').remove();
        //if (Bill_PaymentPosting.bVisitFirst) {
        //    $($('body #OpenVisits')[0]).attr('id', 'OpenVisits1')
        //    $($('body #CloseVisits')[0]).attr('id', 'CloseVisits1');
        //    Bill_PaymentPosting.bVisitFirst = false;
        //}
    },
    FillClaimNumberFromSearch: function (ClaimNumber, AccountNumber, PatientName, PatientId, DOSFrom, VisitId) {
        //$("#" + Bill_PaymentPosting.params.PanelID + " #txtClaimNumber").val(ClaimNumber + ' - ( ' + AccountNumber + ' - ' + PatientName + ' )');
        $("#" + Patient_Case.params.PanelID + " #frmPatientCase #txtClaimNumber").val(ClaimNumber);
        $("#" + Patient_Case.params.PanelID + " #dpDOSfrm").val(DOSFrom);
        //$("#" + Bill_PaymentPosting.params.PanelID + " #hfPatientId").val(PatientId);
        //$("#" + Bill_PaymentPosting.params.PanelID + " #txtPatientName").val(AccountNumber + ' - ' + PatientName);
        //$("#" + Bill_PaymentPosting.params["PanelID"] + " #txtPatientFullName").val(PatientName);
        $("#" + Patient_Case.params.PanelID + " #hfVisitId").val(VisitId);
        //Bill_PaymentPosting.LoadPatientCase(PatientId);
        utility.SetKendoAutoCompleteSourceforValidate($("#" + Patient_Case.params.PanelID + " #frmPatientCase #txtClaimNumber"), ClaimNumber, $("#" + Patient_Case.params["PanelID"] + " #hfPatientId"), VisitId, "ClaimNumber");

        //UnloadActionPan(Bill_PaymentPosting.params["TabID"]);
        Encounter_Visits.UnLoad();
    },
    //LoadPatientCase: function (PatientId) {
    //    if (PatientId == null) {
    //        PatientId = -1;
    //    }

    //    $('#' + Patient_Case.params.PanelID + " input#txtCaseNumber").val('');
    //    $("#" + Patient_Case.params.PanelID + " #hfCaseId").val('');
    //    CacheManager.BindPatientData('GetPatientCase', true, PatientId).done(function (result) {
    //        $('#' + Bill_PaymentPosting.params.PanelID + " input#txtCaseNumber").autocomplete({
    //            autoFocus: true,
    //            source: PatientCase, // pass an array
    //            select: function (event, ui) {
    //                setTimeout(function () {
    //                    $("#" + Bill_PaymentPosting.params.PanelID + " #hfCaseId").val(ui.item.id); // add the selected id
    //                    if ($("#" + Bill_PaymentPosting.params.PanelID + " #lnkCaseNumberEdit").css("display") == "none") {
    //                        $("#" + Bill_PaymentPosting.params.PanelID + " #lnkCaseNumberEdit").css("display", "inline");
    //                        $("#" + Bill_PaymentPosting.params.PanelID + " #lblCaseNumber").css("display", "none");
    //                    }
    //                }, 100);
    //            }
    //        });
    //    });
    //},
    //FillPatientInfoFromSearch: function (PatientId, patFullName) {
    //    $("#" + Patient_Case.params["PanelID"] + " #hfPatientId").val(PatientId);
    //    $("#" + Patient_Case.params["PanelID"] + " #txtPatientName").val(patFullName.split(" ")[0]);
    //   // $("#" + Bill_PaymentPosting.params["PanelID"] + " #txtPatientFullName").val(patFullName.split(" ")[2]);
    //   // Patient_Case.LoadPatientCase(PatientId);
    //    var ParentCtrl = null;
    //    params["ParentCtrl"] = 'Patient_Case';
    //    UnloadActionPan(ParentCtrl);
    //},
    //LoadPatientCase: function (PatientId) {
    //    if (PatientId == null) {
    //        PatientId = -1;
    //    }

    //    $('#' + Patient_Case.params.PanelID + " input#txtCaseNumber").val('');
    //    $("#" + Patient_Case.params.PanelID + " #hfCaseId").val('');
    //    CacheManager.BindPatientData('GetPatientCase', true, PatientId).done(function (result) {
    //        $('#' + Bill_PaymentPosting.params.PanelID + " input#txtCaseNumber").autocomplete({
    //            autoFocus: true,
    //            source: PatientCase, // pass an array
    //            select: function (event, ui) {
    //                setTimeout(function () {
    //                    $("#" + Bill_PaymentPosting.params.PanelID + " #hfCaseId").val(ui.item.id); // add the selected id
    //                    if ($("#" + Bill_PaymentPosting.params.PanelID + " #lnkCaseNumberEdit").css("display") == "none") {
    //                        $("#" + Bill_PaymentPosting.params.PanelID + " #lnkCaseNumberEdit").css("display", "inline");
    //                        $("#" + Bill_PaymentPosting.params.PanelID + " #lblCaseNumber").css("display", "none");
    //                    }
    //                }, 100);
    //            }
    //        });
    //    });
    //},


    HideRefProviderLink: function () {
        $('#pnlPatientCase #hfProvider').val("-1");
        $('#pnlPatientCase #lnkProviderEdit').css("display", "none");
        $('#pnlPatientCase #lblProvider').css("display", "inline");
    },
    // -------------- End Ref Provider -------------

    OpenFacility: function () {
        var params = [];
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Patient_Case";


        if (Encounter_Visits.params.PanelID != 'pnlPatientCase')
            LoadActionPan('Admin_Facility', params, Patient_Case.params.PanelID);
        else
            LoadActionPan('Admin_Facility', params);


    },

    OpenFacilityDetail: function () {
        //Admin_Facility.FacilityEdit($('#' + demographicDetail.params.PanelID + ' #hfFacility').val(), "demographicDetail");
        var params = [];
        params["FacilityId"] = $('#' + Patient_Case.params.PanelID + ' #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacility";
        params["ParentCtrl"] = 'Patient_Case';
        LoadActionPan('facilityDetail', params);
    },

    CaseAddEdit: function (CaseId, mode, AssignedToId, StatusId, event, patientId) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvPatientCase_row' + CaseId));
        AppPrivileges.GetFormPrivileges("Case Management", "Edit", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["CaseId"] = CaseId;
                //  params["PatientId"] = Patient_Case.params.patientID;
                if (patientId)
                    params["PatientId"] = patientId;
                else
                    params["PatientId"] = Patient_Case.params.patientID;
                params["mode"] = mode;
                if (Patient_Case.params.ParentCtrl != null) {
                    params["ParentCtrl"] = 'Patient_Case';
                }
                else
                    params["ParentCtrl"] = "Patient_Case";
                if (Patient_Case.params.PanelID == 'pnlPatientCase')
                    LoadActionPan('Patient_Case_Detail', params);
                else
                    LoadActionPan('Patient_Case_Detail', params, Patient_Case.params.PanelID);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    SearchPatientCase: function (CaseId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Case Management", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#" + Patient_Case.params["PanelID"] + " #pnlPatientCase_Result").css("display") == "none") {
                    $("#" + Patient_Case.params["PanelID"] + " #pnlPatientCase_Result").show();
                }

                var PatientID = null;
                var AssignedToUserId = null;
                PatientID = Patient_Case.params.patientID;
                AssignedToUserId = null;
                //var self = $("#pnlPatientCase");
                var self = $("#" + Patient_Case.params["PanelID"]);
                var myJSON = self.getMyJSON();
                //$('#pnlPatientCase #hfProvider').val("");
                //$('#pnlPatientCase #hfFacility').val("");

                Patient_Case.CaseSearch(myJSON, CaseId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        if (response.CaseCount > 0) {

                            //----------------Pagination---------------

                            $("#" + Patient_Case.params["PanelID"] + " #divPatientCasePaging").css("display", "inline");
                            //Showing 1 to 15 of 15 Record(s)
                            var RecordsPerPage = rpp != null ? rpp : 15;
                            var CurrentPage = PageNo != null ? PageNo : 1;

                            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                            if (PageNo == null) {
                                utility.GetCustomPaging("" + Patient_Case.params["PanelID"] + " #divPatientCasePaging", response.iTotalDisplayRecords, 5, "Patient_Case", CurrentPage, RecordsPerPage);
                            }
                            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                            $("#" + Patient_Case.params["PanelID"] + " #divPatientCasePaging #divShowingEntries").text(showingText);
                            // Change Background Color to Black for selected page
                            $("#" + Patient_Case.params["PanelID"] + " #divPatientCasePaging" + " li").each(function () {
                                if ($(this).text() == CurrentPage) {
                                    $(this).attr("class", "active");
                                }
                                else
                                    $(this).removeAttr("class");
                            });

                            //----------------End Pagination-----------

                        }
                        else {
                            $("#" + Patient_Case.params["PanelID"] + " #divPatientCasePaging").css("display", "none");
                        }
                        Patient_Case.CaseGridLoad(response);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
                //Patient_Case.CaseGridLoad();

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    CaseGridLoad: function (response) {
        if ($.fn.dataTable.isDataTable("#" + Patient_Case.params["PanelID"] + " #pnlPatientCase_Result #dgvPatientCase"))
            $("#" + Patient_Case.params["PanelID"] + " #pnlPatientCase_Result #dgvPatientCase").dataTable().fnDestroy();
        $("#" + Patient_Case.params["PanelID"] + " #pnlPatientCase_Result #dgvPatientCase tbody").find("tr").remove();
        if (response.CaseCount > 0) {
            var CaseLoadJSONData = JSON.parse(response.CaseLoad_JSON);
            $.each(CaseLoadJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", "gvPatientCase_row" + item.CaseMgmtId);
                $row.attr("CaseId", item.CaseMgmtId);

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
                var selectCase = "";
                var selectMethod = "";
                if (Patient_Case.params["RefCtrl"] != null) {
                    selectMethod = "Patient_Case.FillCaseNumber('" + item.CaseMgmtId + "','" + item.CaseNumber + "');";
                    selectCase = '&nbsp;<a class="btn  btn-xs" href="#" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';
                    $row.attr("onclick", selectMethod);
                }
                else {
                    $row.attr("onclick", "Patient_Case.CaseAddEdit(" + item.CaseMgmtId.trim() + ",'Edit',null,null,event," + item.PatientId.trim() + ");");
                }
                var EditMethod = "Patient_Case.CaseAddEdit(" + item.CaseMgmtId.trim() + ",'Edit',null,null,event," + item.PatientId.trim() + ");";
                var ActiveInacvtiveMethod = "Patient_Case.ActiveInactiveCase(" + item.CaseMgmtId.trim() + "," + isactive + ",event);";

                $row.append('<td style="display:none;">' + item.CaseMgmtId + '</td><td><a class="btn  btn-xs" href="#" onclick="Patient_Case.DeletePatientCase(' + item.CaseMgmtId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a><a class="btn btn-xs" href="#" onclick="' + EditMethod + '"  title="Edit Record"><i class="fa fa-edit black"></i></a><a class="btn  btn-xs" href="#" onclick="' + ActiveInacvtiveMethod + '" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectCase + '</td><td>' + item.CaseNumber + '</td><td>' + item.CaseType + '</td><td>' + item.InsuranceName + '</td><td>' + item.AccountNumber + '</td><td>' + item.ProviderName + '</td><td>' + item.FacilityName + '</td><td>' + item.RefProviderName + '</td><td>' + item.ProviderName + '</td>');

                $("#" + Patient_Case.params["PanelID"] + " #pnlPatientCase_Result #dgvPatientCase tbody").last().append($row);
            });
        }
        else {
            if (!$("#" + Patient_Case.params["PanelID"] + " #pnlPatientCase_Result #dgvPatientCase").parent().parent().hasClass("dataTables_wrapper")) {
                $("#" + Patient_Case.params["PanelID"] + " #divPatientCasePaging").css("display", "none");
                $("#" + Patient_Case.params["PanelID"] + " #pnlPatientCase_Result #dgvPatientCase").DataTable({
                    "language": {
                        "emptyTable": "No Case Found"
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
            }
            else {
                var $row = $('<tr/>');
                $row.append('<td colspan="10" class="center" >No Case Found</td>');
                $("#" + Patient_Case.params["PanelID"] + " #pnlPatientCase_Result #dgvPatientCase tbody").last().append($row);
            }


        }

        if ($.fn.dataTable.isDataTable("#" + Patient_Case.params["PanelID"] + " #pnlPatientCase_Result #dgvPatientCase") || $("#" + Patient_Case.params["PanelID"] + " #pnlPatientCase_Result #dgvPatientCase").parent().parent().hasClass("dataTables_wrapper"))
            ;
        else
            $("#" + Patient_Case.params["PanelID"] + " #pnlPatientCase_Result #dgvPatientCase").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "order": [[0, "desc"]] }); // to remove records per page dropdown


    },

    ActiveInactiveCase: function (CaseId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Case Management", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = CaseId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Patient_Case.CaseUpdateActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Patient_Case.SearchPatientCase(selectedValue);
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

    DeletePatientCase: function (CaseId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvPatientCase_row' + CaseId));
        AppPrivileges.GetFormPrivileges("Case Management", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = CaseId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Patient_Case.CaseDelete(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#' + Patient_Case.params["PanelID"] + ' #pnlPatientCase_Result #dgvPatientCase').DataTable();
                                table1.row('.active').remove().draw(false);
                                Patient_Case.SearchPatientCase();
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

    FillCaseNumber: function (CaseId, CaseNumber) {

        var RefCtrl = " #txtProvider";
        var RefCtrlHidden = " #hfProvider";
        var RefCtrlLabel = " #lblProvider";
        var RefCtrlLink = " #lnkProviderEdit";
        if (Patient_Case.params["RefCtrl"] != null)
            RefCtrl = " #" + Patient_Case.params["RefCtrl"];
        if (Patient_Case.params["RefCtrlHidden"] != null)
            RefCtrlHidden = " #" + Patient_Case.params["RefCtrlHidden"];
        if (Patient_Case.params["RefCtrlLabel"] != null)
            RefCtrlLabel = " #" + Patient_Case.params["RefCtrlLabel"];
        if (Patient_Case.params["RefCtrlLink"] != null)
            RefCtrlLink = " #" + Patient_Case.params["RefCtrlLink"];

        $('#' + Patient_Case.params["PanelID"].replace(' #pnlPatientCase', '') + RefCtrl).val(CaseNumber);
        $('#' + Patient_Case.params["PanelID"].replace(' #pnlPatientCase', '') + RefCtrlHidden).val(CaseId);
        $('#' + Patient_Case.params["PanelID"].replace(' #pnlPatientCase', '') + RefCtrlLabel).css("display", "none");
        $('#' + Patient_Case.params["PanelID"].replace(' #pnlPatientCase', '') + RefCtrlLink).css("display", "inline");
        if (Patient_Case.params["IsOptional"] != null && Patient_Case.params["RefForm"] != null && Patient_Case.params["IsOptional"] == false) {
            $('#' + Patient_Case.params["PanelID"].replace(' #pnlPatientCase', '') + ' #' + Patient_Case.params["RefForm"]).bootstrapValidator('revalidateField', $('#' + Patient_Case.params["PanelID"].replace(' #pnlPatientCase', '') + RefCtrl).attr("name"));
        }
        UnloadActionPan(Patient_Case.params["ParentCtrl"], "Patient_Case");
        $('#' + Patient_Case.params["PanelID"].replace(' #pnlPatientCase', '') + RefCtrl).focus();
    },

    CaseSearch: function (PatientCaseData, CaseId, PageNumber, RowsPerPage) {
        if (PatientCaseData == null) {
            PatientCaseData = "";
        }
        if (CaseId == null) {
            CaseId = 0;
        }
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }

        //var InsurancePlanId = "";
        //if ($('#' + Patient_Case.params["PanelID"] + " #DivPatientInsurance").css('display') == 'block') {
        //    InsurancePlanId = $('#' + Patient_Case.params["PanelID"] + " #ddlPatientInsuranceId").val();
        //}
        //else {
        //    InsurancePlanId = "";
        //}

        var data = "CaseData=" + PatientCaseData + "&PatientID=" + Patient_Case.params.patientID + "&CaseID=" + CaseId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // var data = "CaseData=" + PatientCaseData + "&PatientID=''&CaseID=" + CaseId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_CASE", "SEARCH_CASE");
    },

    CaseUpdateActiveInactive: function (CaseID, IsActive) {
        var data = "CaseId=" + CaseID + "&PatientID=" + Patient_Case.params.patientID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_CASE", "UPDATE_CASE_ACTIVE_INACTIVE");
    },

    CaseDelete: function (CaseID) {
        var data = "CaseId=" + CaseID + "&PatientID=" + Patient_Case.params.patientID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_CASE", "DELETE_CASE");
    },

    UnLoad: function () {
        if (Patient_Case.params != null && Patient_Case.params.ParentCtrl != null) {
            UnloadActionPan(Patient_Case.params.ParentCtrl, 'Patient_Case');
        }
        else {
            UnloadActionPan(null, 'Patient_Case');
        }
    },
    //-----------Pagination Functions--------------
    SelectedPageClick: function (PageNo, objPage) {
        // Change Background Color to Black for selected page
        $("#pnlPatientCase_Result li").each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });
        Patient_Case.SearchPatientCase(0, PageNo, 15);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#pnlPatientCase_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            Patient_Case.SearchPatientCase(0, currentPageNo, 15);

        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {

        var currentPageNo = "";
        $("#pnlPatientCase_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });

        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo <= TotalPages) {
            Patient_Case.SearchPatientCase(0, currentPageNo, 15);
        }
    },


    BindPatientAccount: function () {
        var Ctrl = $("#" + Patient_Case.params.PanelID + " #txtPatientName");
        var hfCtrl = $("#" + Patient_Case.params["PanelID"] + " #hfPatientId");
        var func = function () { return Bill_PaymentPosting.GetActivePatientsArray(Ctrl.val()); };
        var onSelect = function (e) { $("#" + Patient_Case.params.PanelID + " #txtPatientName").val(e.value.split(" ")[0]); };
        utility.BindKendoAutoComplete(Ctrl, 3, "value", "contains", null, func, hfCtrl, onSelect);
    },

    GetActivePatientsArray: function (name) {
        var AllPatients = [];
        var dfd = new $.Deferred();
        appointmentDetail.LoadActivePatients(name).done(function (responseData) {
            if (responseData.status != false) {
                if (responseData.PatientCount > 0) {
                    var PatientLoadJSONData = JSON.parse(responseData.PatientLoad_JSON);
                    $.each(PatientLoadJSONData, function (i, item) {
                        AllPatients.push({ id: item.PatientId, value: item.AccountNumber, AccountNumber: item.AccountNumber, FullName: item.FullName, RefferingProviderName: item.ReferringProviderName, RefferingProviderId: item.ReferringProviderId, ProviderID: item.ProviderId, Providername: item.ProviderName, FacilityID: item.FacilityId, Facilityname: item.FacilityName });
                    });
                }
            }
            dfd.resolve(AllPatients);
        });
        return dfd.promise();
    },

    //ValidateSearchPatternForSelectedInsurancePlan: function () {
    //    setTimeout(function () {
    //        if ($('#' + Patient_Case.params.PanelID + ' #hfInsurancePlan').val() != "") {
    //            var selectedPlan = $('#' + Patient_Case.params.PanelID + ' #txtInsurancePlan').autocomplete('option', 'source').filter(function (item) {
    //                return item.id == $('#' + Patient_Case.params.PanelID + ' #hfInsurancePlan').val();
    //            });
    //            if (selectedPlan.length > 0) {
    //                $('#' + Patient_Case.params.PanelID + ' #frmPatientCase #hfInsurancePlansearchPattern').val(selectedPlan[0].searchPattern);
    //            } else {
    //                $('#' + Patient_Case.params.PanelID + ' #hfInsurancePlansearchPattern').val('');
    //            }

    //        } else {
    //            $('#' + Patient_Case.params.PanelID + ' #hfInsurancePlansearchPattern').val('');
    //        }
    //    }, 200);
    //},


    //OpenInsurancePlan: function () {
    //    var params = [];
    //    var PanelID = Patient_Case.params["PanelID"];
    //    params["ParentCtrl"] = 'Patient_Case';
    //    params["InsurancePlanId"] = "-1";
    //    params["FromAdmin"] = "0";
    //    //if (PlanProvider != null)
    //    //    params["RefCtrl"] = PlanProvider;
    //    LoadActionPan('Admin_InsurancePlan', params, PanelID);
    //},
    //FillInsurancePlanName: function (InsurancePlanId, InsurancePlanName) {
    //    $("#" + Patient_Case.params.PanelID +" #txtInsurancePlan").val(InsurancePlanName);
    //    $("#" + Patient_Case.params.PanelID + " #hfInsurancePlan").val(InsurancePlanId);
    //    Admin_InsurancePlan.UnLoadTab();
    //},

    BindClaimNumber: function () {
        var Ctrl = $("#" + Patient_Case.params.PanelID + " #frmPatientCase #txtClaimNumber");
        var hfCtrl = $("#" + Patient_Case.params.PanelID + " #hfVisitId");
        var func = function () { return Bill_ClaimSubmission.GetClaimNumberArray(Ctrl.val()); };
        var onSelect = function (e) { $("#" + Patient_Case.params.PanelID + " #dpDOSfrm").val(e.DOSFrom); };
        utility.BindKendoAutoComplete(Ctrl, 3, "ClaimNumber", "contains", null, func, hfCtrl, onSelect);
    },


}