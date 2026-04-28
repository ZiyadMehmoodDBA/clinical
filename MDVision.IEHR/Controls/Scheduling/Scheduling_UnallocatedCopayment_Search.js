Scheduling_UnallocatedCopayment_Search = {
    bIsFirstLoad: true,
    params: [],
    FromAdmin: '0',
    Load: function (params) {
        if (Scheduling_UnallocatedCopayment_Search.bIsFirstLoad) {
            Scheduling_UnallocatedCopayment_Search.bIsFirstLoad = false;
            Scheduling_UnallocatedCopayment_Search.params = params;
            var self = $('#' + Scheduling_UnallocatedCopayment_Search.params["PanelID"]);
            Scheduling_UnallocatedCopayment_Search.LoadAllControls();
            Scheduling_UnallocatedCopayment_Search.CopayReceiptSearch();
        }
    },
    CopayReceiptSearch: function (PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Copay Receipt", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlCopaymentReceipt_Result").css("display") == "none") {
                    $("#pnlCopaymentReceipt_Result").show();
                }
                var DOSTo = $('#' + Scheduling_UnallocatedCopayment_Search.params.PanelID + ' #frmCopaymentReceipt #dpToDate').val();
                var DOSFrom = $('#' + Scheduling_UnallocatedCopayment_Search.params.PanelID + ' #frmUnClaimedAppointment #dpStartDate').val();
                if (DOSTo == "" && DOSFrom != "") {
                    utility.CreateDatePicker(Scheduling_UnallocatedCopayment_Search.params.PanelID + ' #frmUnClaimedAppointment #dpToDate', function (ev) { }, true);
                }

                var self = $("#pnlCopaymentReceipt_Search");
                var myJSON = self.getMyJSON();

                Scheduling_UnallocatedCopayment_Search.SearchCopayReceipt(myJSON, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        if (response.UnAllocatedCopayListRecordCount > 0) {
                            $("#" + Scheduling_UnallocatedCopayment_Search.params.PanelID + " #dgvCopaymentReciept_Paging").css("display", "inline");
                            var TableControl = Scheduling_UnallocatedCopayment_Search.params.PanelID + " #dgvCopaymentReciept";
                            var PagingPanelControlID = Scheduling_UnallocatedCopayment_Search.params.PanelID + " #dgvCopaymentReciept_Paging";
                            var ClassControlName = "Scheduling_UnallocatedCopayment_Search";
                            var PagesToDisplay = 5;
                            var iTotalDisplayRecords = response.iTotalDisplayRecords;
                            setTimeout(CreatePagination(response.UnAllocatedCopayListRecordCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                                Scheduling_UnallocatedCopayment_Search.CopayReceiptSearch(PageNumber, ResultPerPage);
                            }), 10);
                        }
                        else {
                            $("#" + Scheduling_UnallocatedCopayment_Search.params.PanelID + " #divUnClaimAppPaging").css("display", "none");
                        }
                        Scheduling_UnallocatedCopayment_Search.UnAllocatedCopayGridLoad(response);
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
    UnAllocatedCopayGridLoad: function (res) {
        $("#dgvCopaymentReciept").dataTable().fnDestroy();
        $("#pnlCopaymentReceipt_Result #dgvCopaymentReciept tbody").find("tr").remove();
        if (res.UnAllocatedCopayListRecordCount > 0) {
            var UnAllocatedCopayJSONData = res.UnAllocatedCopayInfo_JSON;
            $.each(UnAllocatedCopayJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", "gvCopay_row" + i);
                //AST-192 Dollar sign add, right align copy amount,and show two figure in copyamount colunm.    
                $row.append('<td style="display:none;">' + item.UnAllocatedCopayId + '</td><td><a class="btn btn-xs" href="#" onclick="Scheduling_UnallocatedCopayment_Search.PrintPreviewOfCopayment(' + item.UnAllocatedCopayId + ');" title="Print Record"><i class="fa fa-print"></i></a></td><td>' + item.ReceiptNumber + '</td><td>' + (utility.RemoveTimeFromDate(null, item.ReceiptDate)) + '</td><td>' + item.AccountNumber + '</td><td>' + item.PatientName + '</td><td>' + (item.ProviderName == null ? "" : item.ProviderName) + '</td><td>' + (item.FacilityName == null ? "" : item.FacilityName) + '</td><td class=txt-align-right> $' + parseFloat(item.CopayAmount).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td>' + (utility.RemoveTimeFromDate(null, item.CreatedOn)) + '</td><td>' + item.CreatedByName + '</td><td>' + (item.Status == null ? "" : item.Status) + '</td>');


                $("#pnlCopaymentReceipt_Result #dgvCopaymentReciept tbody").last().append($row);
            });
        }
        else {
            $("#" + Scheduling_UnallocatedCopayment_Search.params.PanelID + " #dgvCopaymentReciept_Paging").css("display", "none");
            $('#dgvCopaymentReciept').DataTable({
                "language": {
                    "emptyTable": "No Record Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvCopaymentReciept'))
            ;
        else
            $("#" + Scheduling_UnallocatedCopayment_Search.params.PanelID + " #pnlCopaymentReceipt_Result #dgvCopaymentReciept").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] });
    },
    PrintPreviewOfCopayment: function (CopayReceiptID) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Copay Receipt", "PRINT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = {};
                params["UnallocatedCopaymentId"] = CopayReceiptID;
                params["ParentCtrl"] = "billTabCopayReceipt";
                LoadActionPan('Unallocated_CopaymentView', params);
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    SearchCopayReceipt: function (CopayReceiptData, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "CopayReceiptData=" + CopayReceiptData + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_UNALLOCATEDCOPAYMENT", "GET_UNALLOCATED_COPAYMENT");
    },
    LoadAllControls: function () {
        AppPrivileges.GetFormPrivileges("Facility", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                CacheManager.BindCodes('GetFacility', false).done(function (result) {
                    var Ctrl = $("#frmCopaymentReceipt #txtFacility");
                    var hfCtrl = $("#" + Scheduling_UnallocatedCopayment_Search.params.PanelID + " #hfFacility");
                    var onSelect = function (e) {
                        $("#" + Scheduling_UnallocatedCopayment_Search.params.PanelID + " #txtPractice").val(e.Practice);
                        $("#" + Scheduling_UnallocatedCopayment_Search.params.PanelID + " #hfPractice").val(e.PracticeId);
                    };
                    utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl, onSelect);
                });
            }
            //else
            //    utility.DisplayMessages(strMessage, 2);
        });
        AppPrivileges.GetFormPrivileges("Provider", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                CacheManager.BindCodes('GetProvider', false).done(function (result) {
                    var Ctrl = $("#" + Scheduling_UnallocatedCopayment_Search.params.PanelID + " #txtProvider");
                    var hfCtrl = $("#" + Scheduling_UnallocatedCopayment_Search.params.PanelID + " #hfProvider");
                    var onSelect = function (e) { Ctrl.attr("ProviderId", e.id); };
                    utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, onSelect);
                });
            }
            //else
            //    utility.DisplayMessages(strMessage, 2);
        });
        utility.ValidateFromToDate('frmCopaymentReceipt', 'dpStartDate', 'dpToDate', true, null, null, null, true);
        Scheduling_UnallocatedCopayment_Search.BindPatientAccount();
    },

    ResetForm: function () {

        $('#' + Scheduling_UnallocatedCopayment_Search.params.PanelID + ' #frmCopaymentReceipt').resetAllControls();
        $('#' + Scheduling_UnallocatedCopayment_Search.params["PanelID"] + ' #frmUnClaimedAppointment #dpToDate').attr('disabled', true);
        //var date_format = 'mm/dd/yyyy';
        //set default Date Formate
        //if (globalAppdata['DateFormat'])
        //    date_format = globalAppdata['DateFormat'];

        //$('#' + Scheduling_UnallocatedCopayment_Search.params.PanelID + ' #dpStartDate,#dpToDate')
        //    .datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), new Date()));
        //AST-165 Start
        $('#' + Scheduling_UnallocatedCopayment_Search.params["PanelID"] + ' #frmCopaymentReceipt #lnkProviderEdit').css("display", "none");
        $('#' + Scheduling_UnallocatedCopayment_Search.params["PanelID"] + ' #frmCopaymentReceipt #lblProvider').css("display", "block");

        $('#' + Scheduling_UnallocatedCopayment_Search.params["PanelID"] + ' #frmCopaymentReceipt #lnkFacilityEdit').css("display", "none");
        $('#' + Scheduling_UnallocatedCopayment_Search.params["PanelID"] + ' #frmCopaymentReceipt #lblFacility').css("display", "block");
        //AST-165-End
    },

    UnLoad: function (Tab) {
        RemoveAdminTab(Tab);
    },
    BindPatientAccount: function () {
        var Ctrl = $('#' + Scheduling_UnallocatedCopayment_Search.params.PanelID + ' #txtPatientName');
        var func = function () { return utility.GetPatientArray(Ctrl.val(), 0) };
        var hfCtrl = $("#" + Scheduling_UnallocatedCopayment_Search.params.PanelID + " #hfPatientId");
        var onSelect = function (e) { utility.InsertRecentPatient(e.id); };
        utility.BindKendoAutoComplete(Ctrl, 4, "AccountNumber", "contains", null, func, hfCtrl, onSelect);
    },

    FillPatientInfoFromSearch: function (AccountNo, PatientId, event) {

        if (event != null) {
            event.stopPropagation();
        }
        if ($('#pnlScheduling_UnallocatedCopayment_Search #frmCopaymentReceipt #txtPatientName').data('kendoAutoComplete'))
            utility.SetKendoAutoCompleteSourceforValidate($('#pnlScheduling_UnallocatedCopayment_Search #frmCopaymentReceipt #txtPatientName'), AccountNo, $('#pnlScheduling_UnallocatedCopayment_Search #frmCopaymentReceipt #hfPatientId'), PatientId, "AccountNumber");
        UnloadActionPan("billTabCopayReceipt");
        utility.InsertRecentPatient(PatientId);
    },

    OpenPatientSearch: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'billTabCopayReceipt';
        LoadActionPan('Patient_Search', params);
    },
    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmCopaymentReceipt";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "billTabCopayReceipt";
        LoadActionPan('Admin_Facility', params);
    },
    OpenFacilityDetail: function () {
        var params = [];
        params["FacilityId"] = $('#' + Scheduling_UnallocatedCopayment_Search.params.PanelID + ' #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'frmCopaymentReceipt';
        params["RefCtrl"] = "txtFacility";
        LoadActionPan('facilityDetail', params);
    },
    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmCopaymentReceipt";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "billTabCopayReceipt";
        LoadActionPan('Admin_Provider', params);
    },
    OpenProviderDetail: function () {
        var params = [];
        params["ProviderId"] = $('#' + Scheduling_UnallocatedCopayment_Search.params.PanelID + ' #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'billTabCopayReceipt';
        LoadActionPan('providerDetail', params);
    },
}
