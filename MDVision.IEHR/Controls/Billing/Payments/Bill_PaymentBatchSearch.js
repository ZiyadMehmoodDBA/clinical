Bill_PaymentBatchSearch = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        if (params.RefCtrl)
            params.FromAdmin = '0';
        else
            params.FromAdmin = '1';
        Bill_PaymentBatchSearch.params = params;


        if (Bill_PaymentBatchSearch.params["PanelID"] != 'pnlBillPaymentBatchSearch')
            self = $('#' + Bill_PaymentBatchSearch.params["PanelID"] + ' #pnlBillPaymentBatchSearch');
        else
            self = $('#pnlBillPaymentBatchSearch');

        //  var self = $('#pnlBillPaymentBatchSearch');

        if (Bill_PaymentBatchSearch.bIsFirstLoad) {
            Bill_PaymentBatchSearch.bIsFirstLoad = false;
            //Bill_PaymentBatchSearch.LoadDefaultData();
            self.loadDropDowns(true).done(function () {
                Bill_PaymentBatchSearch.LoadAllControls();
                Bill_PaymentBatchSearch.PaymentBatchSearch();
            });
        }
    },




    PaymentBatchSearch: function (PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Payment Batch", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var self = "";
                if (Bill_PaymentBatchSearch.params["PanelID"] != 'pnlBillPaymentBatchSearch')
                    self = $('#' + Bill_PaymentBatchSearch.params["PanelID"] + ' #pnlBillPaymentBatchSearch');
                else
                    self = $('#pnlBillPaymentBatchSearch');

                var myJSON = self.getMyJSON();
                var BatchId = null;
                Bill_PaymentBatchSearch.SearchPaymentBatch(myJSON, BatchId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {

                        //-----------------Pagination------------

                        if (response.PaymentBatchCount > 0) {
                            $('#' + Bill_PaymentBatchSearch.params["PanelID"] + " #divPaymentBatchPaging").css("display", "inline");
                            //Showing 1 to 15 of 15 entries
                            var RecordsPerPage = rpp != null ? rpp : 15;
                            var CurrentPage = PageNo != null ? PageNo : 1;

                            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                            if (PageNo == null) {
                                utility.GetCustomPaging("divPaymentBatchPaging", response.iTotalDisplayRecords, 5, "Bill_PaymentBatchSearch", CurrentPage, RecordsPerPage);
                            }
                            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                            $('#' + Bill_PaymentBatchSearch.params["PanelID"] + " #divPaymentBatchPaging #divShowingEntries").text(showingText);
                            // Change Background Color to Black for selected page
                            $('#' + Bill_PaymentBatchSearch.params["PanelID"] + " li").each(function () {
                                if ($(this).text() == CurrentPage) {
                                    $(this).attr("class", "active");
                                }
                                else
                                    $(this).removeAttr("class");
                            });
                        }
                        else {
                            $('#' + Bill_PaymentBatchSearch.params["PanelID"] + " #divPaymentBatchPaging").css("display", "none");
                        }

                        //--------------------End Pagination-------------------

                        Bill_PaymentBatchSearch.PaymentBatchGridLoad(response);
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
    SearchPaymentBatch: function (PaymentBatchData, BatchId, PageNumber, RowsPerPage) {

        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "PaymentBatchData=" + PaymentBatchData + "&BatchId=" + BatchId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        return MDVisionService.defaultService(data, "BILLING_PAYMENT_BATCH", "SEARCH_PAYMENT_BATCH");
    },
    OpenPaymentBatch: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Payment Batch", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params['mode'] = 'Add';
                params["ParentCtrl"] = "Bill_PaymentBatchSearch";

                var parentPanelId = null;
                if (Bill_PaymentBatchSearch.params["ParentCtrl"] != null)
                    parentPanelId = GetTab(Bill_PaymentBatchSearch.params["ParentCtrl"]).PanelID;

                LoadActionPan('paymentBatchDetail', params, parentPanelId);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmPaymentBatchSearch";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Bill_PaymentBatchSearch";


        var parentPanelId = null;
        if (Bill_PaymentBatchSearch.params["ParentCtrl"] != null)
            parentPanelId = GetTab(Bill_PaymentBatchSearch.params["ParentCtrl"]).PanelID;

        LoadActionPan('Admin_Facility', params, parentPanelId);
    },
    OpenFacilityDetail: function () {
        var params = [];
        params["FacilityId"] = $("#" + Bill_PaymentBatchSearch.params.PanelID + " #hfFacility").val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Bill_PaymentBatchSearch';
        params["RefCtrl"] = "txtFacility";

        var parentPanelId = null;
        if (Bill_PaymentBatchSearch.params["ParentCtrl"] != null)
            parentPanelId = GetTab(Bill_PaymentBatchSearch.params["ParentCtrl"]).PanelID;


        LoadActionPan('facilityDetail', params, parentPanelId);
    },
    OpenPractice: function () {
        var params = [];
        params["PracticeId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Bill_PaymentBatchSearch";


        var parentPanelId = null;
        if (Bill_PaymentBatchSearch.params["ParentCtrl"] != null)
            parentPanelId = GetTab(Bill_PaymentBatchSearch.params["ParentCtrl"]).PanelID;

        LoadActionPan('Admin_Practice', params, parentPanelId);
    },
    OpenPracticeDetail: function () {
        var params = [];
        params["PracticeId"] = $('#' + Bill_PaymentBatchSearch.params.PanelID + ' #hfPractice').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtPractice";
        params["ParentCtrl"] = 'Bill_PaymentBatchSearch';

        var parentPanelId = null;
        if (Bill_PaymentBatchSearch.params["ParentCtrl"] != null)
            parentPanelId = GetTab(Bill_PaymentBatchSearch.params["ParentCtrl"]).PanelID;

        LoadActionPan('practiceDetail', params, parentPanelId);
    },
    LoadAllControls: function () {
        CacheManager.BindCodes('GetFacility', false).done(function (result) {
            var Ctrl = $("#frmPaymentBatchSearch #txtFacility");
            var hfCtrl = $("#" + Bill_PaymentBatchSearch.params.PanelID + " #hfFacility");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl);
        });
        CacheManager.BindCodes('GetPractice', false).done(function (result) {
            var Ctrl = $("#frmPaymentBatchSearch #txtPractice");
            var hfCtrl = $("#" + Bill_PaymentBatchSearch.params.PanelID + " #hfPractice");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Practices, null, hfCtrl);
        });
        utility.CreateDatePicker(Bill_PaymentBatchSearch.params.PanelID + ' #frmPaymentBatchSearch #dtpDepositDate', function (ev) { }, false);
        utility.ValidateFromToDate('frmPaymentBatchSearch', 'dpEntryDatefrm', 'dpEntryDateTo', true);
    },
    LoadDefaultData: function () {
        $("#" + Bill_PaymentBatchSearch.params["PanelID"] + ' #txtPractice').val(globalAppdata['DefaultPracticeName']);
        $("#" + Bill_PaymentBatchSearch.params["PanelID"] + ' #hfPractice').val(globalAppdata['DefaultPracticeId']);
        $("#" + Bill_PaymentBatchSearch.params["PanelID"] + ' #lnkPracticeEdit').css("display", "inline");
        $("#" + Bill_PaymentBatchSearch.params["PanelID"] + ' #lblPractice').css("display", "none");
        $("#" + Bill_PaymentBatchSearch.params["PanelID"] + ' #txtFacility').val(globalAppdata['DefaultFacilityName']);
        $("#" + Bill_PaymentBatchSearch.params["PanelID"] + ' #hfFacility').val(globalAppdata['DefaultFacilityId']);
        $("#" + Bill_PaymentBatchSearch.params["PanelID"] + ' #lnkFacilityEdit').css("display", "inline");
        $("#" + Bill_PaymentBatchSearch.params["PanelID"] + ' #lblFacility').css("display", "none");
        utility.CreateDatePicker(Bill_PaymentBatchSearch.params.PanelID + ' #frmPaymentBatchSearch #dtpDepositDate,#dpEntryDatefrm,#dpEntryDateTo', function (ev) { }, true);

    },
    PaymentBatchGridLoad: function (response) {
        $("#" + Bill_PaymentBatchSearch.params["PanelID"] + " #pnlPaymentBatch_Result").show();
        $("#" + Bill_PaymentBatchSearch.params["PanelID"] + " #pnlPaymentBatch_Result #dgvPaymentBatch").dataTable().fnDestroy();
        $("#" + Bill_PaymentBatchSearch.params["PanelID"] + " #pnlPaymentBatch_Result #dgvPaymentBatch tbody").find("tr").remove();
        $("#" + Bill_PaymentBatchSearch.params["PanelID"] + " #pnlPaymentBatch_Result #dgvPaymentBatch tfoot").find("tr").remove();
        if (response.PaymentBatchCount > 0) {
            var data = JSON.parse(response.PaymentBatchLoad_JSON);
            var TtlPlanAmount = 0, TtlPlanAmountPosted = 0, TtlPatientAmountPosted = 0, TtlPatientAmount = 0, TtlCopayment = 0;
            $.each(data, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", "gvPaymentBatch_row" + item.PmtBatchId);
                $row.attr("BatchId", item.PmtBatchId);
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

                var selectBillCharge = "";
                var selectMethod = "";
                var actions = "";
                if (Bill_PaymentBatchSearch.params["FromAdmin"] == "0") {
                    if (Bill_PaymentBatchSearch.params["ParentCtrl"] == "Bill_PaymentPosting" || Bill_PaymentBatchSearch.params["ParentCtrl"] == "billTabPaymentPosting") {
                        selectMethod = "Bill_PaymentPosting.fillPaymentBatch('" + item.PmtBatchNumber + "','" + item.PmtBatchId + "','" + item.CheckNumber + "','" + utility.RemoveTimeFromDate(null, item.CheckDate) + "',event);Bill_PaymentBatchSearch.UnLoadTab();";
                        selectMethod = "Bill_PaymentPosting.fillPaymentBatch('" + item.PmtBatchNumber + "','" + item.PmtBatchId + "','" + item.CheckNumber + "','" + utility.RemoveTimeFromDate(null, item.CheckDate) + "',event);Bill_PaymentBatchSearch.UnLoadTab();";
                        selectBillCharge = '&nbsp;<a class="btn  btn-xs" href="#" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';
                    }
                    //else {
                    //    selectBillCharge = '&nbsp;<a class="btn  btn-xs" href="#" onclick="Bill_PaymentBatchSearch.FillBatchCharge(\'' + item.PmtBatchNumber + '\', \'' + item.PmtBatchId + '\');" title="Select Record"><i class="fa fa-check black"></i></a>';
                    //    actions = '<a class="btn  btn-xs" href="#" onclick="Bill_PaymentBatchSearch.PaymentBatchDelete(' + item.PmtBatchId + ');" title="Delete"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Bill_PaymentBatchSearch.EditPaymentBatch(' + item.PmtBatchNumber + ',' + item.PmtBatchId + ');" title="Edit"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Bill_PaymentBatchSearch.LoadPaymentPosting(' + item.PmtBatchNumber + ',' + item.PmtBatchId + ');" title="Payment Posting"><i class="fa fa-usd green"></i></a>';
                    //}
                    $row.attr("onclick", selectMethod);
                }
                else {
                    $row.attr("onclick", "Bill_PaymentBatchSearch.EditPaymentBatch('" + item.PmtBatchNumber + "','" + item.PmtBatchId + "',event);");
                    actions = '<a class="btn  btn-xs" href="#" onclick="Bill_PaymentBatchSearch.PaymentBatchDelete(' + item.PmtBatchId + ',event);" title="Delete"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Bill_PaymentBatchSearch.EditPaymentBatch(' + item.PmtBatchNumber + ',' + item.PmtBatchId + ',event);" title="Edit"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Bill_PaymentBatchSearch.LoadPaymentPosting(\'' + item.PmtBatchNumber + '\',\'' + item.PmtBatchId + '\',event,\'' + item.CheckNumber + '\',\'' + utility.RemoveTimeFromDate(null, item.CheckDate) + '\');" title="Payment Posting"><i class="fa fa-usd green"></i></a>';

                }
                $row.append('<td style="display:none;">' + item.PmtBatchId + '</td><td>' + actions + selectBillCharge + '</td> <td>' + item.PmtBatchNumber + '</td> <td class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.Description + '">' + item.Description + '</td> <td>' + item.PracticeName + '</td> <td>' + item.FacilityName + '</td> <td>' + utility.RemoveTimeFromDate(null, item.DepositDate) + '</td><td class="ellip200" data-toggle="tooltip" data-placement="right" title="' + item.CheckNumber + '">' + item.CheckNumber + '</td> <td>' + utility.RemoveTimeFromDate(null, item.CheckDate) + '</td><td class="text-right">' + globalAppdata.DefaultCurrency + Number(item.PlanAmount).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td class="text-right">' + globalAppdata.DefaultCurrency + Number(item.PlanAmountPosted).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td class="text-right">' + globalAppdata.DefaultCurrency + Number(item.PatientAmount).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td class="text-right">' + globalAppdata.DefaultCurrency + Number(item.PatientAmountPosted).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td class="text-right">' + globalAppdata.DefaultCurrency + Number(item.Copayment).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td>' + item.BillerName + '</td><td class="noWordBreak">' + item.Status + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.EndDate) + '</td><td>' + item.BatchEnteredBy + '</td>');
                $("#" + Bill_PaymentBatchSearch.params["PanelID"] + " #pnlPaymentBatch_Result #dgvPaymentBatch tbody").last().append($row);
                if (item.PlanAmount)
                    TtlPlanAmount = parseFloat(TtlPlanAmount) + parseFloat(item.PlanAmount);
                if (item.PlanAmountPosted)
                    TtlPlanAmountPosted = parseFloat(TtlPlanAmountPosted) + parseFloat(item.PlanAmountPosted);
                if (item.PatientAmountPosted)
                    TtlPatientAmountPosted = parseFloat(TtlPatientAmountPosted) + parseFloat(item.PatientAmountPosted);
                if (item.PatientAmount)
                    TtlPatientAmount = parseFloat(TtlPatientAmount) + parseFloat(item.PatientAmount);
                if (item.Copayment)
                    TtlCopayment = parseFloat(TtlCopayment) + parseFloat(item.Copayment);
                
            });
            var $FooterRow = $('<tr class="bold black bg-default" style="text-align: right" />');
            $FooterRow.append('<td colspan="8">Total:</td>' +
                           '<td>' + utility.convertToFigure(TtlPlanAmount, true) + '</td>' +
                           '<td>' + utility.convertToFigure(TtlPlanAmountPosted, true) + '</td>' +
                           '<td>' + utility.convertToFigure(TtlPatientAmount, true) + '</td>' +
                           '<td>' + utility.convertToFigure(TtlPatientAmountPosted, true) + '</td>' +
                           '<td >' + utility.convertToFigure(TtlCopayment, true) + '</td>'+
                           '<td colspan="5"></td>'
                           );
            $("#" + Bill_PaymentBatchSearch.params["PanelID"] + " #pnlPaymentBatch_Result #dgvPaymentBatch tfoot").last().append($FooterRow);
        }
        else {
            $('#' + Bill_PaymentBatchSearch.params["PanelID"] + " #divPaymentBatchPaging").css("display", "none");
            $("#" + Bill_PaymentBatchSearch.params["PanelID"] + " #pnlPaymentBatch_Result #dgvPaymentBatch").DataTable({
                "language": {
                    "emptyTable": "No Payment Batch Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Bill_PaymentBatchSearch.params["PanelID"] + " #pnlPaymentBatch_Result #dgvPaymentBatch"))
            ;
        else if (!$("#" + Bill_PaymentBatchSearch.params["PanelID"] + " #pnlPaymentBatch_Result #dgvPaymentBatch").parent().parent().hasClass("dataTables_wrapper"))
            $("#" + Bill_PaymentBatchSearch.params["PanelID"] + " #pnlPaymentBatch_Result #dgvPaymentBatch").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        $("#pnlPaymentBatch_Result div.table-responsive").css("overflow", "auto");
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
    },
    PaymentBatchDelete: function (BatchID, event) {
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Payment Batch", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = BatchID;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Bill_PaymentBatchSearch.DeletePaymentBatch(BatchID).done(function (response) {
                            if (response.status == true) {
                                Bill_PaymentBatchSearch.PaymentBatchSearch();
                                utility.DisplayMessages(response.Message, 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }

                        });
                    }
                });
                }
            else
                utility.DisplayMessages(strMessage, 2);

        });
    },
    DeletePaymentBatch: function (BatchID) {
        var data = "BatchId=" + BatchID;
        return MDVisionService.defaultService(data, "BILLING_PAYMENT_BATCH_DETAIL", "DELETE_PAYMENT_BATCH");
    },
    EditPaymentBatch: function (batchNumber, BatchId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Payment Batch", "Edit", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["BatchId"] = BatchId;
                params["BatchNumber"] = batchNumber;
                //params["ParentCtrl"] = "billTabChargeBatchSearch";
                params["mode"] = "edit";
                LoadActionPan('paymentBatchDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);

        });
    },
    LoadPaymentPosting: function (BatchNumber, BatchId, event, CheckNumber, CheckDate) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Payment Posting", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FromAdmin"] = 0;
                params["ParentCtrl"] = 'billTabPaymentBatchSearch';

                params["BatchNumber"] = BatchNumber;
                params["BatchId"] = BatchId;
                params["CheckNumber"] = CheckNumber;
                params["CheckDate"] = CheckDate;
                params['mode'] = 'edit';
                params["PaymentRef"] = "pnlBillPaymentBatchSearch";
                LoadActionPan('Bill_PaymentPosting', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    UnLoadTab: function (Tab) {

        if (Bill_PaymentBatchSearch.params["ParentCtrl"] != null) {
            if (Bill_PaymentBatchSearch.params["ParentCtrl"] == "Bill_PaymentPosting" && Bill_PaymentBatchSearch.params.PaymentRef != null)
                UnloadActionPan(Bill_PaymentBatchSearch.params.ParentCtrl, "Bill_PaymentBatchSearch", null, Bill_PaymentBatchSearch.params.PaymentRef);

            else {

                UnloadActionPan(Bill_PaymentBatchSearch.params["ParentCtrl"]);
            }

        }

        else {
            RemoveAdminTab(Tab);
        }

    },

    //------Pagination Functions------
    SelectedPageClick: function (PageNo, objPage) {
        // Change Background Color to Black for selected page
        $("#pnlPaymentBatch_Result li").each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });
        Bill_PaymentBatchSearch.PaymentBatchSearch(PageNo, 15);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#pnlPaymentBatch_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            Bill_PaymentBatchSearch.PaymentBatchSearch(currentPageNo, 15);

        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {

        var currentPageNo = "";
        $("#pnlPaymentBatch_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });

        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo <= TotalPages) {
            Bill_PaymentBatchSearch.PaymentBatchSearch(currentPageNo, 15);
        }
    },

    BillPaymentBatchReset: function () {
        //$('#' + Bill_PaymentBatchSearch.params["PanelID"] + ' #frmPaymentBatchSearch').find('[data-plugin-datepicker]').each(function () {
        //    $(this).datepicker('setDate', new Date());
        //});

        $('#' + Bill_PaymentBatchSearch.params["PanelID"] + ' #frmPaymentBatchSearch').resetAllControls();


        $('#' + Bill_PaymentBatchSearch.params["PanelID"] + ' #frmPaymentBatchSearch [type="text"][onblur]').each(function () {
            $(this).trigger("blur");
        });
        if ($('#' + Bill_PaymentBatchSearch.params["PanelID"] + ' #frmPaymentBatchSearch #lnkFacilityEdit').is(":visible")) {
            $('#' + Bill_PaymentBatchSearch.params["PanelID"] + ' #frmPaymentBatchSearch #lnkFacilityEdit').hide();
            $('#' + Bill_PaymentBatchSearch.params["PanelID"] + ' #frmPaymentBatchSearch #lblFacility').show();
        }
        if ($('#' + Bill_PaymentBatchSearch.params["PanelID"] + ' #frmPaymentBatchSearch #lnkPracticeEdit').is(":visible")) {
            $('#' + Bill_PaymentBatchSearch.params["PanelID"] + ' #frmPaymentBatchSearch #lnkPracticeEdit').hide();
            $('#' + Bill_PaymentBatchSearch.params["PanelID"] + ' #frmPaymentBatchSearch #lblPractice').show();
        }
    },
}
