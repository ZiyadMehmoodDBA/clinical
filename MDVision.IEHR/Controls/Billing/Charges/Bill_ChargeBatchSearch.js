Bill_ChargeBatchSearch = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {

        if (params.RefCtrl)
            params.FromAdmin = '0';
        else
            params.FromAdmin = '1';
        Bill_ChargeBatchSearch.params = params;

        if (Bill_ChargeBatchSearch.bIsFirstLoad) {
            Bill_ChargeBatchSearch.bIsFirstLoad = false;

            var self = "";//$('#pnlBillChargeBatchSearch');
            if (Bill_ChargeBatchSearch.params["PanelID"] != 'pnlBillChargeBatchSearch')
                self = $('#' + Bill_ChargeBatchSearch.params["PanelID"] + ' #pnlBillChargeBatchSearch');
            else
                self = $('#pnlBillChargeBatchSearch');

            self.loadDropDowns(true).done(function () {
                Bill_ChargeBatchSearch.LoadAllControls();
                //Bill_ChargeBatchSearch.LoadDefaultData();
                Bill_ChargeBatchSearch.BatchChargeChargeSearch();
            });
        }
    },

    ChargeBatchAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Charge Batch", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                //params["AppointmentId"] = null;
                params["mode"] = "Add";
                params["FromAdmin"] = Bill_ChargeBatchSearch.params["FromAdmin"];
                //if (Bill_ChargeBatchSearch.params["FromAdmin"] == "0") {
                params["ParentCtrl"] = 'Bill_ChargeBatchSearch';
                //}
                LoadActionPan('chargeBatchDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    BatchChargeChargeSearch: function (BatchId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Charge Batch", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlBillChargeBatch_Result").css("display") == "none") {
                    $("#pnlBillChargeBatch_Result").show();
                }
                if ($("#" + Bill_ChargeBatchSearch.params["PanelID"] + " #pnlBillChargeBatch_Result").css("display") == "none") {
                    $("#" + Bill_ChargeBatchSearch.params["PanelID"] + " #pnlBillChargeBatch_Result").show();
                }

                var self = "";//$('#pnlBillChargeBatchSearch');
                if (Bill_ChargeBatchSearch.params["PanelID"] != 'pnlBillChargeBatchSearch')
                    self = $('#' + Bill_ChargeBatchSearch.params["PanelID"] + ' #pnlBillChargeBatchSearch');
                else
                    self = $('#pnlBillChargeBatchSearch');

                var myJSON = self.getMyJSON();

                Bill_ChargeBatchSearch.SearchBatchCharge(myJSON, BatchId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {

                        //-----------------Pagination------------

                        if (response.ChargeBatchCount > 0) {
                            $('#' + Bill_ChargeBatchSearch.params["PanelID"] + " #dviChargeBatchPaging").css("display", "inline");
                            //Showing 1 to 15 of 15 entries
                            var RecordsPerPage = rpp != null ? rpp : 15;
                            var CurrentPage = PageNo != null ? PageNo : 1;

                            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                            if (PageNo == null) {
                                utility.GetCustomPaging("dviChargeBatchPaging", response.iTotalDisplayRecords, 5, "Bill_ChargeBatchSearch", CurrentPage, RecordsPerPage);
                            }
                            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                            $('#' + Bill_ChargeBatchSearch.params["PanelID"] + " #dviChargeBatchPaging #divShowingEntries").text(showingText);
                            // Change Background Color to Black for selected page
                            //Edited by Azeem Raza Tayyab on 8-Mar-2016 to fix of selected active class of claim li on Encounter Screen #PMS-4375
                            $('#' + Bill_ChargeBatchSearch.params["PanelID"] + " #pnlBillChargeBatch_Result li").each(function () {
                                if ($(this).text() == CurrentPage) {
                                    $(this).attr("class", "active");
                                }
                                else
                                    $(this).removeAttr("class");
                            });
                        }
                        else {
                            $('#' + Bill_ChargeBatchSearch.params["PanelID"] + " #dviChargeBatchPaging").css("display", "none");
                        }

                        //--------------------End Pagination-------------------

                        Bill_ChargeBatchSearch.ChargeBatchGridLoad(response);
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

    BatchChargeChargeReset: function () {
        $('#' + Bill_ChargeBatchSearch.params["PanelID"] + ' #frmChargeBatchSearch').find('[data-plugin-datepicker]').each(function () { $(this).datepicker('setDate', new Date()); });
        $('#' + Bill_ChargeBatchSearch.params["PanelID"] + ' #frmChargeBatchSearch').resetAllControls();
        $('#' + Bill_ChargeBatchSearch.params["PanelID"] + ' #frmChargeBatchSearch [type="text"][onblur]').each(function () {
            $(this).trigger("blur");
        });
        $('#' + Bill_ChargeBatchSearch.params["PanelID"] + ' #frmChargeBatchSearch #dpDOSTo').attr('disabled', true);
        $('#' + Bill_ChargeBatchSearch.params["PanelID"] + ' #frmChargeBatchSearch #dpEntryDateTo').attr('disabled', true);

        //AST-165 Start
        if ($('#' + Bill_ChargeBatchSearch.params["PanelID"] + ' #frmChargeBatchSearch #lnkPracticeEdit').is(":visible")) {
            $('#' + Bill_ChargeBatchSearch.params["PanelID"] + ' #frmChargeBatchSearch #lnkPracticeEdit').hide();
            $('#' + Bill_ChargeBatchSearch.params["PanelID"] + ' #frmChargeBatchSearch #lblPractice').show();
        }
        if ($('#' + Bill_ChargeBatchSearch.params["PanelID"] + ' #frmChargeBatchSearch #lnkFacilityEdit').is(":visible")) {
            $('#' + Bill_ChargeBatchSearch.params["PanelID"] + ' #frmChargeBatchSearch #lnkFacilityEdit').hide();
            $('#' + Bill_ChargeBatchSearch.params["PanelID"] + ' #frmChargeBatchSearch #lblFacility').show();
        }
        if ($('#' + Bill_ChargeBatchSearch.params["PanelID"] + ' #frmChargeBatchSearch #lnkProviderEdit').is(":visible")) {
            $('#' + Bill_ChargeBatchSearch.params["PanelID"] + ' #frmChargeBatchSearch #lnkProviderEdit').hide();
            $('#' + Bill_ChargeBatchSearch.params["PanelID"] + ' #frmChargeBatchSearch #lblProvider').show();
        }
        //AST-165 End
    },

    ChargeBatchGridLoad: function (response) {
        $("#" + Bill_ChargeBatchSearch.params["PanelID"] + " #pnlBillChargeBatch_Result #dgvBatchCharge").dataTable().fnDestroy();
        $("#" + Bill_ChargeBatchSearch.params["PanelID"] + " #pnlBillChargeBatch_Result #dgvBatchCharge tbody").find("tr").remove();
        if (response.ChargeBatchCount > 0) {
            var BillBatchChargeLoadJSONData = JSON.parse(response.ChargeBatchLoad_JSON);
            $.each(BillBatchChargeLoadJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", "gvBatchCharge_row" + item.BatchId);
                $row.attr("BatchId", item.BatchId);
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
                var actions = "";
                if (Bill_ChargeBatchSearch.params["FromAdmin"] == "0") {
                    var selectMethod = "Bill_ChargeBatchSearch.FillBatchCharge('" + item.BatchNumber + "','" + item.BatchId + "');"
                    selectBillCharge = '&nbsp;<a class="btn  btn-xs" href="#" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';
                    $row.attr("onclick", selectMethod);
                    actions = '<td>' + selectBillCharge + '</td> ';
                }
                else {
                    $row.attr("onclick", "Bill_ChargeBatchSearch.UpdateBatchCharge('" + item.BatchNumber.toString() + "','" + item.BatchId + "',event);");
                    actions = '<td><a class="btn  btn-xs" href="#" onclick="Bill_ChargeBatchSearch.BatchChargeDelete(\'' + item.BatchId + '\',event);" title="Delete Batch Charge"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Bill_ChargeBatchSearch.UpdateBatchCharge(\'' + item.BatchNumber.toString() + '\',\'' + item.BatchId + '\',event);" title="Edit"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Bill_ChargeBatchSearch.LoadChargeView(\'' + item.BatchNumber + '\',\'' + item.BatchId + '\',event);" title="Charges"><i class="fa fa-credit-card"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Bill_ChargeBatchSearch.LoadClaimView(\'' + item.BatchNumber + '\',\'' + item.BatchId + '\',event);" title="Claims"><i class="fa fa-calculator black"></i></a></td> ';
                }
                //$row.append('<td style="display:none;">' + item.BatchId + '</td><td><a class="btn  btn-xs" href="#" onclick="Bill_ChargeBatchSearch.BatchChargeDelete(' + item.BatchId + ');" title="Delete Batch Charge"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Bill_ChargeBatchSearch.LoadChargeView(' + item.BatchNumber + ',' + item.BatchId + ');" title="Charges"><i class="fa fa-credit-card"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Bill_ChargeBatchSearch.UpdateBatchCharge(' + item.BatchNumber + ',' + item.BatchId + ');" title="Edit"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Bill_ChargeBatchSearch.LoadClaimView(' + item.BatchNumber + ',' + item.BatchId + ');" title="Claims"><i class="fa fa-calculator black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectBillCharge + '</td> <td>' + item.BatchNumber + '</td> <td>' + item.Description + '</td> <td>' + item.ProviderName + '</td> <td>' + item.FacilityName + '</td> <td>' + item.UserName + '</td> <td>' + item.DOSFrom + '</td> <td>' + item.DOSTo + '</td> <td>' + item.BatchStatusName + '</td>');
                $row.append('<td style="display:none;">' + item.BatchId + '</td>' + actions + '<td>' + item.BatchNumber + '</td> <td class="ellip150" data-toggle="tooltip" data-placement="right" title="' + item.Description + '">' + item.Description + '</td> <td>' + item.ProviderName + '</td> <td>' + item.FacilityName + '</td><td>' + item.PracticeName + '</td>  <td>' + item.UserName + '</td> <td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td> <td>' + utility.RemoveTimeFromDate(null, item.DOSTo) + '</td> <td>' + item.BatchStatusName + '</td><td>' + utility.RemoveTimeFromDate(null, item.BatchEntryDate) + '</td><td>' + item.BatchEnteredBy + '</td>');

                $("#" + Bill_ChargeBatchSearch.params["PanelID"] + " #pnlBillChargeBatch_Result #dgvBatchCharge tbody").last().append($row);
            });
        }
        else {
            $('#' + Bill_ChargeBatchSearch.params["PanelID"] + " #dviChargeBatchPaging").css("display", "none");
            $("#" + Bill_ChargeBatchSearch.params["PanelID"] + " #pnlBillChargeBatch_Result #dgvBatchCharge").DataTable({
                "language": {
                    "emptyTable": "No Batch Charge Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable("#" + Bill_ChargeBatchSearch.params["PanelID"] + " #pnlBillChargeBatch_Result #dgvBatchCharge") || $("#" + Bill_ChargeBatchSearch.params["PanelID"] + " #pnlBillChargeBatch_Result").find("div").hasClass("dataTables_wrapper"))
            ;
        else
            $("#" + Bill_ChargeBatchSearch.params["PanelID"] + " #pnlBillChargeBatch_Result #dgvBatchCharge").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    FillBatchCharge: function (BatchNumber, BatchId) {

        var RefCtrl = "txtChargeBatchNumber";
        var RefHiddenIdCtrl = "hfChargeBatchNumberId";
        var RefHiddenCtrl = "hfChargeBatchNumber";

        if (Bill_ChargeBatchSearch.params.RefCtrl != null)
            RefCtrl = Bill_ChargeBatchSearch.params.RefCtrl;

        if (Bill_ChargeBatchSearch.params.RefHiddenIdCtrl != null)
            RefHiddenIdCtrl = Bill_ChargeBatchSearch.params.RefHiddenIdCtrl;

        if (Bill_ChargeBatchSearch.params.RefHiddenCtrl != null)
            RefHiddenCtrl = Bill_ChargeBatchSearch.params.RefHiddenCtrl;


        $('#' + Bill_ChargeBatchSearch.params.PanelID + ' #' + RefCtrl).val(BatchNumber);
        $('#' + Bill_ChargeBatchSearch.params.PanelID + ' #' + RefHiddenCtrl).val(BatchNumber);
        $('#' + Bill_ChargeBatchSearch.params.PanelID + ' #' + RefHiddenIdCtrl).val(BatchId);

        UnloadActionPan(Bill_ChargeBatchSearch.params["ParentCtrl"], "Bill_ChargeBatchSearch");

    },

    LoadChargeView: function (BatchNumber, BatchId, event) {

        if (event != null) {
            event.stopPropagation();
        }
        //var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Charges", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        var params = [];
        params["BatchId"] = BatchId;
        params["BatchNumber"] = BatchNumber;
        params["ParentCtrl"] = "Bill_ChargeBatchSearch";
        LoadActionPan('chargesViewDetail', params);
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});

    },

    LoadClaimView: function (BatchNumber, BatchId, event) {
        if (event != null) {
            event.stopPropagation();
        }

        //var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Charges", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        var params = [];
        params["BatchId"] = BatchId;
        params["BatchNumber"] = BatchNumber;
        params["ParentCtrl"] = "Bill_ChargeBatchSearch";
        LoadActionPan('claimViewDetail', params);
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});

    },


    LoadAllControls: function () {
        var self = "";
        if (Bill_ChargeBatchSearch.params["PanelID"] != 'pnlBillChargeBatchSearch')
            self = $('#' + Bill_ChargeBatchSearch.params["PanelID"] + ' #pnlBillChargeBatchSearch');
        else
            self = $('#pnlBillChargeBatchSearch');

        if (self.find('.modal-dialog').length > 0) {
            self.find('.modal-dialog').removeClass('modal-dialog-lg').addClass('modal-dialog-full');
        }

        CacheManager.BindCodes('GetFacility', false).done(function (result) {
            var Ctrl = self.find("#frmChargeBatchSearch #txtFacility");
            var hfCtrl = $("#" + Bill_ChargeBatchSearch.params.PanelID + " #hfFacility");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl);
        });
        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            var Ctrl = self.find("#frmChargeBatchSearch #txtProvider");
            var hfCtrl = $("#" + Bill_ChargeBatchSearch.params.PanelID + " #hfProvider");
            var onSelect = function (e) { Ctrl.attr("ProviderId", e.id); };
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, onSelect);
        });
        CacheManager.BindCodes('GetPractice', false).done(function (result) {
            var Ctrl = self.find("#frmChargeBatchSearch #txtPractice");
            var hfCtrl = $("#" + Bill_ChargeBatchSearch.params.PanelID + " #hfPractice");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Practices, null, hfCtrl);
        });
        //utility.CreateDatePicker(Bill_ChargeBatchSearch.params.PanelID + ' #frmChargeBatchSearch #dpEntryDatefrm,#dpEntryDateTo', function (ev) { }, false);
        utility.ValidateFromToDate('frmChargeBatchSearch', 'dpEntryDatefrm', 'dpEntryDateTo', true);
        utility.ValidateFromToDate('frmChargeBatchSearch', 'dpDSOFrm', 'dpDOSTo', true);
    },
    LoadDefaultData: function () {
        $("#" + Bill_ChargeBatchSearch.params["PanelID"] + ' #txtPractice').val(globalAppdata['DefaultPracticeName']);
        $("#" + Bill_ChargeBatchSearch.params["PanelID"] + ' #hfPractice').val(globalAppdata['DefaultPracticeId']);
        $("#" + Bill_ChargeBatchSearch.params["PanelID"] + ' #lnkPracticeEdit').css("display", "inline");
        $("#" + Bill_ChargeBatchSearch.params["PanelID"] + ' #lblPractice').css("display", "none");

        $("#" + Bill_ChargeBatchSearch.params["PanelID"] + ' #txtProvider').val(globalAppdata['DefaultProviderName']);
        $("#" + Bill_ChargeBatchSearch.params["PanelID"] + ' #hfProvider').val(globalAppdata['DefaultProviderId']);
        $("#" + Bill_ChargeBatchSearch.params["PanelID"] + ' #lnkProviderEdit').css("display", "inline");
        $("#" + Bill_ChargeBatchSearch.params["PanelID"] + ' #lblProvider').css("display", "none");
        $("#" + Bill_ChargeBatchSearch.params["PanelID"] + ' #txtFacility').val(globalAppdata['DefaultFacilityName']);
        $("#" + Bill_ChargeBatchSearch.params["PanelID"] + ' #hfFacility').val(globalAppdata['DefaultFacilityId']);
        $("#" + Bill_ChargeBatchSearch.params["PanelID"] + ' #lnkFacilityEdit').css("display", "inline");
        $("#" + Bill_ChargeBatchSearch.params["PanelID"] + ' #lblFacility').css("display", "none");
        utility.CreateDatePicker(Bill_ChargeBatchSearch.params.PanelID + ' #frmChargeBatchSearch #dpEntryDatefrm,#dpEntryDateTo,#dpDSOFrm,#dpDOSTo', function (ev) { }, true);

    },
    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmChargeBatchSearch";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Bill_ChargeBatchSearch";
        LoadActionPan('Admin_Facility', params);
    },
    OpenFacilityDetail: function () {
        var params = [];
        params["FacilityId"] = $('#' + Bill_ChargeBatchSearch.params.PanelID + ' #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Bill_ChargeBatchSearch';
        params["RefCtrl"] = "txtFacility";
        LoadActionPan('facilityDetail', params);
    },
    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmChargeBatchSearch";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Bill_ChargeBatchSearch";
        LoadActionPan('Admin_Provider', params);
    },
    OpenProviderDetail: function () {
        var params = [];
        params["ProviderId"] = $('#' + Bill_ChargeBatchSearch.params.PanelID + ' #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'Bill_ChargeBatchSearch';
        LoadActionPan('providerDetail', params);
    },
    OpenPractice: function () {
        var params = [];
        params["PracticeId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Bill_ChargeBatchSearch";
        LoadActionPan('Admin_Practice', params);
    },
    OpenPracticeDetail: function () {
        var params = [];
        params["PracticeId"] = $('#' + Bill_ChargeBatchSearch.params.PanelID + ' #hfPractice').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtPractice";
        params["ParentCtrl"] = 'Bill_ChargeBatchSearch';
        LoadActionPan('practiceDetail', params);
    },

    LoadChargeCapture: function (BatchId, BatchNumber) {
        var params = [];
        params["FromAdmin"] = 0;
        params["VisitId"] = "-1";
        params["ParentCtrl"] = 'Bill_ChargeBatchSearch';
        LoadActionPan('EncounterChargeCapture', params);
    },
    SearchBatchCharge: function (BatchChargeData, BatchId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "BatchChargeData=" + BatchChargeData + "&BatchId=" + BatchId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "BILLING_BATCHCHARGE", "GET_BATCHCHARGE_SEARCH");
    },
    UpdateBatchCharge: function (BatchNumber, BatchId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var params = [];
        params["BatchId"] = BatchId;
        //params["BatchNumber"] = BatchNumber;  // That was creating issue . It change the batch Number 052015203 to  11016835
        params["ParentCtrl"] = "Bill_ChargeBatchSearch";
        params["mode"] = "edit";
        LoadActionPan('chargeBatchDetail', params);
    },
    BatchChargeDelete: function (BatchId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.myConfirm('1', function () {
            var selectedValue = BatchId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Bill_ChargeBatchSearch.DeleteBatchCharge(BatchId).done(function (response) {
                    if (response.status == true) {
                        Bill_ChargeBatchSearch.BatchChargeChargeSearch();
                        utility.DisplayMessages(response.Message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }

                });
            }
        }, function () { });
    },
    DeleteBatchCharge: function (BatchId) {
        var data = "BatchId=" + BatchId;
        return MDVisionService.defaultService(data, "BILLING_BATCHCHARGE_DETAIL", "DELETE_BATCH_CHARGE");
    },
    UnLoadTab: function (Tab) {

        if (Bill_ChargeBatchSearch.params["FromAdmin"] == "0") {
            if (Bill_ChargeBatchSearch.params != null && Bill_ChargeBatchSearch.params.ParentCtrl != null) {
                UnloadActionPan(Bill_ChargeBatchSearch.params.ParentCtrl, 'Bill_ChargeBatchSearch');
            }
            else
                UnloadActionPan(null, 'Bill_ChargeBatchSearch');
        } else {
            RemoveAdminTab(Tab);
        }

    },
    //--------Pagination Functions--------
    SelectedPageClick: function (PageNo, objPage) {
        // Change Background Color to Black for selected page
        $("#pnlBillChargeBatch_Result li").each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });
        Bill_ChargeBatchSearch.BatchChargeChargeSearch(0, PageNo, 15);
    },
    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#pnlBillChargeBatch_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            Bill_ChargeBatchSearch.BatchChargeChargeSearch(0, currentPageNo, 15);

        }
    },
    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {

        var currentPageNo = "";
        $("#pnlBillChargeBatch_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });

        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo <= TotalPages) {
            Bill_ChargeBatchSearch.BatchChargeChargeSearch(0, currentPageNo, 15);
        }
    },
}
