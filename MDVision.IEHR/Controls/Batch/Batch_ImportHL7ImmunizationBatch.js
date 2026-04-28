Batch_ImportHL7ImmunizationBatch = {
    params: [],
    bIsFirstLoad: true,
    Load: function (params) {
        Batch_ImportHL7ImmunizationBatch.params = params;
        if (Batch_ImportHL7ImmunizationBatch.params.PanelID != 'pnlBatchImportHL7ImmunizationBatch') {
            Batch_ImportHL7ImmunizationBatch.params.PanelID = Batch_ImportHL7ImmunizationBatch.params.PanelID + ' #pnlBatchImportHL7ImmunizationBatch';
        } else {
            Batch_ImportHL7ImmunizationBatch.params.PanelID = 'pnlBatchImportHL7ImmunizationBatch';
        }
        Batch_ImportHL7ImmunizationBatch.ValidateFromToDate();
        Batch_ImportHL7ImmunizationBatch.BindPatient();
        Batch_ImportHL7ImmunizationBatch.BindFacility('txtFacility', 'lnkFacilityEdit', 'lblFacility', 'hfFacilityId', 'divOutBound');
        Batch_ImportHL7ImmunizationBatch.BindProvider('txtProvider', 'lnkProviderEdit', 'lblProvider', 'hfProviderId', 'divBatch');
        Batch_ImportHL7ImmunizationBatch.BindProvider('txtGivenBy', 'lnkGivenByProviderEdit', 'lblGivenByProvider', 'hfGivenByProviderId', 'divOutBound');
        var self = $('#pnlBatchImportHL7ImmunizationBatch');
        self.loadDropDowns(true).done(function () {
            Batch_ImportHL7ImmunizationBatch.BatchSearch();
        });
    },
    HL7ImmunizationGridTabChange: function (tab) {
        if (tab == 'Batch') {
            utility.ValidateFromToDate('frmImportHL7ImmunizationBatch #divBatch', 'dtpDateFrom', 'dtpDateTo', true);
            Batch_ImportHL7ImmunizationBatch.BatchSearch();
        }
        else if (tab == 'OutBound') {
            utility.ValidateFromToDate('frmImportHL7ImmunizationBatch #divOutBound', 'dtpDateFrom', 'dtpDateTo', true);
            Batch_ImportHL7ImmunizationBatch.QueueSearch();
        }
    },
    HL7ImmunizationBatchSearch_DbCall: function (HL7BatchData, PageNumber, RowsPerPage) {
        if (PageNumber == null)
            PageNumber = 1;
        if (RowsPerPage == null)
            RowsPerPage = 15;
        var data = "HL7BatchSearchData=" + HL7BatchData + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        return MDVisionService.defaultService(data, "BATCH_IMPORT_HL7_BATCH_CLINICAL", "LOAD_HL7BATCH");
    },
    BatchSearch: function (ID, PageNo, rpp) {
        if ($("#pnlBatchImportHL7ImmunizationBatch #pnlBatchImportHL7ImmunizationBatch_Result").css("display") == "none") {
            $("#pnlBatchImportHL7ImmunizationBatch #pnlBatchImportHL7ImmunizationBatch_Result").show();
        }
        var self = $("#pnlBatchImportHL7ImmunizationBatch #divBatch");
        var myJSON = self.getMyJSON();
        Batch_ImportHL7ImmunizationBatch.HL7ImmunizationBatchSearch_DbCall(myJSON, PageNo, rpp).done(function (response) {
            if (response.status != false) {

                Batch_ImportHL7ImmunizationBatch.HL7BatchGridLoad(response);
                var TableControl = "pnlBatchImportHL7ImmunizationBatch #divBatch #dgvBatch";
                var PagingPanelControlID = "pnlBatchImportHL7ImmunizationBatch #divBatch #divdgvBatchPaging";
                var ClassControlName = "Batch_ImportHL7ImmunizationBatch";
                var PagesToDisplay = 5;
                var iTotalDraftDisplayRecords = response.iTotalDisplayRecords;

                setTimeout(CreatePagination(response.iTotalDisplayRecords, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDraftDisplayRecords, function (ID, PageNumber, ResultPerPage) {
                    Batch_ImportHL7ImmunizationBatch.BatchSearch(ID, PageNumber, ResultPerPage);
                }), 10);

            }
            else
                utility.DisplayMessages(response.Message, 3);
        });
    },
    HL7BatchGridLoad: function (response) {
        $("#dgvBatch").dataTable().fnDestroy();
        $("#pnlBatchImportHL7ImmunizationBatch #pnlBatchImportHL7ImmunizationBatch_Result #dgvBatch tbody").find("tr").remove();
        if (response.HL7BatchCount > 0) {
            var listSearchHL7Batch = response.listSearchHL7Batch;
            var actions = '';
            $.each(listSearchHL7Batch, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("Id", "dgvBatch_row" + i);
                $row.attr("BatchId", item.HL7BatchId);
                actions = '<td><input type="checkbox"/><a class="btn  btn-xs" href="#" onclick="Batch_ImportHL7ImmunizationBatch.BatchDelete(\'' + item.HL7BatchId + '\',event , \'dgvBatch\');" title="Delete Batch"><i class="fa fa-close red"></i></a><a class="btn  btn-xs" href="#" onclick="Batch_ImportHL7ImmunizationBatch.DownloadBatch(' + item.HL7BatchId + ');" title="Download Record"><i class="fa fa-download"></i></a></td>';

                //$row.append('<td style="display:none;">' + item.HL7BatchId + '</td>' + actions + '<td>' + item.ProviderName + '</td><td>' + (utility.RemoveTimeFromDate(null, item.CreatedOn)) + '</td><td>' + item.Records + '</td><td>' + item.FileName + '</td><td>' + item.StatusName + '</td><td>' + (utility.RemoveTimeFromDate(null, item.CreatedOn)) + '</td>');
                var completionDate = "";
                if (item.IsCompleted.toLowerCase() == "true") {
                    completionDate = item.completionDate;
                }
                $row.append('<td style="display:none;">' + item.HL7BatchId + '</td>' + actions + '<td>' + item.ProviderName + '</td><td>' + item.CreatedOn + '</td><td>' + item.Records + '</td><td>' + item.FileName + '</td><td>' + item.StatusName + '</td><td>' + completionDate + '</td>');

                $("#pnlBatchImportHL7ImmunizationBatch #pnlBatchImportHL7ImmunizationBatch_Result #dgvBatch tbody").last().append($row);
            });
        }
        else {
            $("#" + Batch_ImportHL7ImmunizationBatch.params.PanelID + " #divdgvBatchPaging").css("display", "none");
            $('#dgvBatch').DataTable({
                "language": {
                    "emptyTable": "No Record Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvBatch'))
            ;
        else
            $("#" + Batch_ImportHL7ImmunizationBatch.params.PanelID + " #pnlBatchImportHL7ImmunizationBatch_Result #dgvBatch").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] });

    },
    HL7ImmunizationQueueSearch_DbCall: function (HL7QueueData, PageNumber, RowsPerPage) {
        if (PageNumber == null)
            PageNumber = 1;
        if (RowsPerPage == null)
            RowsPerPage = 15;
        var data = "HL7QueueSearchData=" + HL7QueueData + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        return MDVisionService.defaultService(data, "BATCH_IMPORT_HL7_BATCH_CLINICAL", "LOAD_HL7QUEUE");
    },
    QueueSearch: function (PageNo, rpp) {
        var dfd = new $.Deferred();
        if ($("#pnlBatchImportHL7ImmunizationBatch #pnlBatchImportHL7ImmunizationQueue_Result").css("display") == "none") {
            $("#pnlBatchImportHL7ImmunizationBatch #pnlBatchImportHL7ImmunizationQueue_Result").show();
        }
        var self = $("#pnlBatchImportHL7ImmunizationBatch #divOutBound");
        var myJSON = self.getMyJSON();
        console.log("before search");
        Batch_ImportHL7ImmunizationBatch.HL7ImmunizationQueueSearch_DbCall(myJSON, PageNo, rpp).done(function (response) {
            if (response.status != false) {
                console.log(response);
                $.when(Batch_ImportHL7ImmunizationBatch.HL7QueueGridLoad(response)).then(function () {
                    dfd.resolve();
                });
                var TableControl = "pnlBatchImportHL7ImmunizationBatch #divOutBound #dgvQueue";
                var PagingPanelControlID = "pnlBatchImportHL7ImmunizationBatch #divOutBound #divdgvQueuePaging";
                var ClassControlName = "Batch_ImportHL7ImmunizationBatch";
                var PagesToDisplay = 5;
                var iTotalDraftDisplayRecords = response.iTotalDisplayRecords;

                setTimeout(CreatePagination(response.iTotalDisplayRecords, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDraftDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                    Batch_ImportHL7ImmunizationBatch.QueueSearch(PageNumber, ResultPerPage);
                }), 10);

            }
            else {
                utility.DisplayMessages(response.Message, 3);
                dfd.resolve();
            }
        });
        return dfd;
    },
    HL7QueueGridLoad: function (response) {
        var dfd = new $.Deferred();
        Batch_ImportHL7ImmunizationBatch.params.QueueGridArray = [];
        $("#dgvQueue").dataTable().fnDestroy();
        $("#pnlBatchImportHL7ImmunizationBatch #pnlBatchImportHL7ImmunizationQueue_Result #dgvQueue tbody").find("tr").remove();
        var actions = '';
        if (response.HL7QueueCount > 0) {
            var listSearchHL7Queue = response.listSearchHL7Queue;
            $.each(listSearchHL7Queue, function (i, item) {
                console.log(i);
                var $row = $('<tr/>');
                $row.attr("id", "dgvQueue_row" + i);
                $row.attr("BatchId", item.HL7BatchId);
                var Parameters = item.VaccineHxId + "," + item.VaccineScheduleId + ",'" + item.VaccineCategory + "'," + item.VaccineCategoryId + ",'" + item.VaccineHxType + "','" + item.TabId + "'," + item.PatientId;

                var UploadIcon = '<span class="btn-xs ml-xxs btn-file" style="cursor: pointer;"><i class="fa fa-upload"></i><input type="file" class="btn btn-file" name="Import_Acknowledgements_file" accept="application/txt" id="Import_Acknowledgements_file" allowfiles="txt" onchange="Clinical_Immunization.UploadAcknowledgements( ' + item.VaccineHxId + ',this,1);"></span>';
                actions = '<td>' + UploadIcon + '&nbsp<input type="checkbox"/><a class="btn  btn-xs" href="#" onclick="Batch_ImportHL7ImmunizationBatch.BatchDelete(\'' + item.HL7BatchId + '\',event , \'dgvQueue\');" title="Delete Batch"><i class="fa fa-close red"></i></a><a title="View Note" class="btn  btn-xs" href="#" onclick="Batch_ImportHL7ImmunizationBatch.VaccineHxPreview(' + Parameters + ');"> <i class="fa fa-credit-card blue"></i></a></td>';


                var Acknowledgement = "";
                var AcknowledgmentMessage1 = "";
                var AcknowledgmentMessage2 = "";
                var Class = "";
                if (item.AcknowledgementCode) {
                    if (item.AcknowledgementCode.toLowerCase() == "aa") {
                        Acknowledgement = "Successful";
                        AcknowledgmentMessage1 = "Application Accept";
                        AcknowledgmentMessage2 = "Accept";
                        Class = "fa fa-bell pull-right";
                    }
                    else if (item.AcknowledgementCode.toLowerCase() == "ae") {
                        Acknowledgement = "Error";
                        AcknowledgmentMessage1 = "Application Error";
                        AcknowledgmentMessage2 = "Error";
                        Class = "fa fa-exclamation-triangle pull-right";
                    }
                    else if (item.AcknowledgementCode.toLowerCase() == "ar") {
                        Acknowledgement = "Error";
                        AcknowledgmentMessage1 = "Application Rejected";
                        AcknowledgmentMessage2 = "Reject";
                        Class = "fa fa-exclamation-triangle pull-right";
                    }
                    else {
                        AcknowledgmentMessage1 = "";
                        AcknowledgmentMessage2 = "";
                    }
                }
                var obj = {};
                obj.AcknowledgementCode = item.AcknowledgementCode;
                obj.VaccineHxId = item.VaccineHxId;
                obj.Message1 = AcknowledgmentMessage1;
                obj.Message2 = AcknowledgmentMessage2;
                obj.DOB = item.DOB;
                obj.Gender = item.Gender;
                obj.Years = parseInt(item.years);
                obj.AccountNo = item.AccountNumber;
                obj.Name = item.PatientName;
                Batch_ImportHL7ImmunizationBatch.params.QueueGridArray.push(obj);

                var acknowledgementDiv = '<div style="padding:2px;">  <a href="#" class="AcknowToolTip"    id=' + item.VaccineHxId + '_ToolTipId onclick="Clinical_Immunization.DownloadImmAcknowledgement(' + item.VaccineHxId + ')">' + Acknowledgement + '</a><i class="' + Class + '" ></i></div>';

                $row.append('<td style="display:none;">' + item.HL7BatchId + '</td>' + actions + '<td>' + item.PatientName + '</td><td>' + item.AccountNumber + '</td><td>' + item.Type + '</td><td>' + item.ProviderName + '</td><td>' + item.FacilityName + '</td><td>' + (utility.RemoveTimeFromDate(null, item.CreatedOn)) + '</td><td>' + acknowledgementDiv + '</td>');


                $("#pnlBatchImportHL7ImmunizationBatch #pnlBatchImportHL7ImmunizationQueue_Result #dgvQueue tbody").last().append($row);
            });
            
        }
        else {
            $("#" + Batch_ImportHL7ImmunizationBatch.params.PanelID + " #divdgvQueuePaging").css("display", "none");
            $('#dgvQueue').DataTable({
                "language": {
                    "emptyTable": "No Record Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvQueue'))
            ;
        else
            $("#" + Batch_ImportHL7ImmunizationBatch.params.PanelID + " #pnlBatchImportHL7ImmunizationQueue_Result #dgvQueue").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] });

        Batch_ImportHL7ImmunizationBatch.MakeToolTip();
        dfd.resolve();
        return dfd;
    },
    MakeToolTip: function () {
        $.each(Batch_ImportHL7ImmunizationBatch.params.QueueGridArray, function (i, item) {
            var popover = '<h5 class="pull-left m-none pr-xxs">' + item.Name + '</h5> <span>' + item.Years + ' Y,' + item.Gender + '; '
                                   + '<b>Account:</b> ' + item.AccountNo + '; '
                                   + '<b>DOB: </b> ' + item.DOB + '</span>'
                                   + '<hr class="stooltip " >'

                                   + '<ul class="list-unstyled pl-none">'

                                   + '<li style="width:100%;margin-bottom:8px;"><strong>' + (item.AcknowledgementCode != "" ? item.AcknowledgementCode.toUpperCase() : "") + ':</strong> ' + item.Message1 + '</li>'
                                   + '<li style="width:100%"><strong class="noWordBreak">Application Acknowledgement:</strong>' + item.Message2 + '</li>'
            + '</ul>'
            $('#' + Batch_ImportHL7ImmunizationBatch.params.PanelID + ' #' + item.VaccineHxId + "_ToolTipId").tooltipster({
                theme: 'tooltipster-shadow',
                content: $(popover),
                functionReady: function (instance, helper) {
                    var posTop = $(helper)[0].getBoundingClientRect().top;
                    var anchorBottom = $(this)[0].getBoundingClientRect().bottom;
                    if (posTop < 0) {
                        $('.tooltipster-base').css("top", (anchorBottom + 13) + "px");
                        $('.tooltipster-arrow').removeClass("tooltipster-arrow-top").addClass("tooltipster-arrow-bottom");
                    }
                }

            });
        });
    },
    SelectAllChkBx: function (obj, event, grid) {
        if (event != null)
            event.stopPropagation();
        var objCheck = $(obj);
        if ((objCheck).is(':checked'))
            $('#' + grid + ' input[type="checkbox"]').prop('checked', 'checked');
        else {
            $('#' + grid + ' input[type="checkbox"]').each(function () {
                $(this).attr('checked', false);
            });
        }
    },
    DeleteMultipleBatches: function (gridId) {
        var selected = [];
        var BatchIds = '';
        $('#' + gridId + ' > tbody > tr').each(function () {
            if ($(this).find('input:checked').is(':checked'))
                selected.push($(this).attr('BatchId'));
        });
        for (var w = 0; w <= selected.length; w++)
            BatchIds += selected[w] + ',';
        BatchIds = BatchIds.replace('undefined,', '');
        BatchIds = BatchIds.replace('undefined', '');
        if (BatchIds != "")
            Batch_ImportHL7ImmunizationBatch.DeleteMultipleBatches_Confirmatiom(BatchIds, gridId);
        else
            utility.DisplayMessages("Please Select Batch !", 3);
    },
    BatchDelete: function (batchId, event, gridId) {
        if (event != null)
            event.stopPropagation();
        Batch_ImportHL7ImmunizationBatch.DeleteMultipleBatches_Confirmatiom(batchId, gridId);
    },
    DeleteMultipleBatches_Confirmatiom: function (BatchIds, gridId) {
        utility.myConfirm('51', function () {
            Batch_ImportHL7ImmunizationBatch.DeleteBatch_DBCall(BatchIds).done(function (response) {
                if (response.status != false) {
                    var table1 = $('#' + gridId).DataTable();
                    table1.row('.active').remove().draw(false);
                    if (gridId == 'dgvBatch') {
                        Batch_ImportHL7ImmunizationBatch.BatchSearch();
                        $('#' + gridId + ' #chkSelectAllBatches_Batch').attr('checked', false);
                    }
                    else if (gridId == 'dgvQueue') {
                        console.log(gridId);
                        Batch_ImportHL7ImmunizationBatch.QueueSearch();
                        $('#' + gridId + ' #chkSelectAllBatches_Queue').attr('checked', false);
                    }
                    utility.DisplayMessages(response.Message, 1);
                } else
                    utility.DisplayMessages(response.Message, 3);
            });
        }, function () { }, 'Confirm Delete');
    },
    DeleteBatch_DBCall: function (BatchIds) {
        var data = "HL7BatchID=" + BatchIds;
        return MDVisionService.defaultService(data, "BATCH_IMPORT_HL7_BATCH_CLINICAL", "DELETE_HL7BATCH_IMMUNIZATION");
    },
    MarkAsComplete: function () {
        var selected = [];
        var BatchIds = '';
        $('#dgvBatch > tbody > tr').each(function () {
            if ($(this).find('input:checked').is(':checked'))
                selected.push($(this).attr('BatchId'));
        });
        for (var w = 0; w <= selected.length; w++)
            BatchIds += selected[w] + ',';
        BatchIds = BatchIds.replace('undefined,', '');
        BatchIds = BatchIds.replace('undefined', '');
        if (BatchIds != "")
            Batch_ImportHL7ImmunizationBatch.MarkAsComplete_Confirmation(BatchIds);
        else
            utility.DisplayMessages("Please Select Batch !", 3);
    },
    MarkAsComplete_Confirmation: function (BatchIds) {
        utility.myConfirm('52', function () {
            Batch_ImportHL7ImmunizationBatch.MarkAsComplete_DBCall(BatchIds).done(function (response) {
                if (response.status != false) {
                    Batch_ImportHL7ImmunizationBatch.BatchSearch();
                    $('#dgvBatch #chkSelectAllBatches_Batch').attr('checked', false);
                    utility.DisplayMessages(response.Message, 1);
                } else
                    utility.DisplayMessages(response.Message, 3);
            });
        }, function () { }, 'Batch Complete');
    },
    MarkAsComplete_DBCall: function (BatchIds) {
        var data = "HL7BatchID=" + BatchIds;
        return MDVisionService.defaultService(data, "BATCH_IMPORT_HL7_BATCH_CLINICAL", "MARK_COMPLETED_HL7BATCH_IMMUNIZATION");
    },

    DownloadBatch: function (HL7BatchId) {
        Batch_ImportHL7ImmunizationBatch.DownloadBatch_DBCall(HL7BatchId).done(function (response) {
            if (response.status != false) {
                var Batch_Response_JSON = response.listSearchHL7Batch;
                if (Batch_Response_JSON.length > 0) {
                    var uri = '';
                    var dt = new Date();
                    var strMimeType = "application/octet-stream";
                    download(uri + Batch_Response_JSON[0].Hl7MsgText, Batch_Response_JSON[0].FileName + ".txt", strMimeType);
                }
                else {
                    utility.DisplayMessages("No Record Found", 3);
                }
            } else
                utility.DisplayMessages(response.Message, 3);
        });

    },
    DownloadBatch_DBCall: function (BatchId) {
        var data = "HL7BatchID=" + BatchId;
        return MDVisionService.defaultService(data, "BATCH_IMPORT_HL7_BATCH_CLINICAL", "LOAD_HL7BATCH_BY_ID");
    },
    ReProcess: function () {
        var selected = [];
        var BatchIds = '';
        $('#dgvQueue > tbody > tr').each(function () {
            if ($(this).find('input:checked').is(':checked'))
                selected.push($(this).attr('BatchId'));
        });
        for (var w = 0; w <= selected.length; w++)
            BatchIds += selected[w] + ',';
        BatchIds = BatchIds.replace('undefined,', '');
        BatchIds = BatchIds.replace('undefined', '');
        if (BatchIds != "")
            Batch_ImportHL7ImmunizationBatch.ReProcess_Confirmation(BatchIds);
        else
            utility.DisplayMessages("Please Select Batch !", 3);
    },
    ReProcess_Confirmation: function (BatchIds) {
        utility.myConfirm('53', function () {
            Batch_ImportHL7ImmunizationBatch.ReProcess_DBCall(BatchIds).done(function (response) {
                if (response.status != false) {
                    Batch_ImportHL7ImmunizationBatch.QueueSearch();
                    $('#dgvQueue #chkSelectAllBatches_Queue').attr('checked', false);
                    utility.DisplayMessages(response.Message, 1);
                } else
                    utility.DisplayMessages(response.Message, 3);
            });
        }, function () { }, 'Batch Reprocess');
    },
    ReProcess_DBCall: function (BatchIds) {
        var data = "HL7BatchID=" + BatchIds;
        return MDVisionService.defaultService(data, "BATCH_IMPORT_HL7_BATCH_CLINICAL", "REPROCESS_HL7BATCH_IMMUNIZATION");
    },

    UnLoad: function () {
        if (Batch_ImportHL7ImmunizationBatch.params != null && Batch_ImportHL7ImmunizationBatch.params.ParentCtrl != null) {
            UnloadActionPan(Batch_ImportHL7ImmunizationBatch.params.ParentCtrl, 'Batch_ImportHL7ImmunizationBatch');
        }
        else {
            RemoveAdminTab('batchTabImportHL7ImmunizationBatch');
        }
    },
    ValidateFromToDate: function () {
        if ($('#' + Batch_ImportHL7ImmunizationBatch.params["PanelID"] + ' #liBatch').hasClass("active"))
            utility.ValidateFromToDate('frmImportHL7ImmunizationBatch #divBatch', 'dtpDateFrom', 'dtpDateTo', true);
        else if ($('#' + Batch_ImportHL7ImmunizationBatch.params["PanelID"] + ' #liOutbound').hasClass("active"))
            utility.ValidateFromToDate('frmImportHL7ImmunizationBatch #divOutBound', 'dtpDateFrom', 'dtpDateTo', true);
    },
    OpenPatientAccount: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Batch_ImportHL7ImmunizationBatch";
        LoadActionPan('Patient_Search', params);
    },
    OpenProvider: function (tabId) {
        var params = [];
        params["RefForm"] = "frmImportHL7ImmunizationBatch";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["TabId"] = tabId;
        if (tabId == 'divBatch') {
            var RefCtrl = "txtProvider";
            var RefCtrlHidden = "hfProvider";
            var RefCtrlLabel = "lblProvider";
            var RefCtrlLink = "lnkProviderEdit";
            params["RefCtrl"] = RefCtrl;
            params["RefCtrlHidden"] = RefCtrlHidden;
            params["RefCtrlLabel"] = RefCtrlLabel;
            params["RefCtrlLink"] = RefCtrlLink;
        }
        else if (tabId == 'divOutBound') {
            var RefCtrl = "txtGivenBy";
            var RefCtrlHidden = "hfGivenByProviderId";
            var RefCtrlLabel = "lblGivenByProvider";
            var RefCtrlLink = "lnkGivenByProviderEdit";
            params["RefCtrl"] = RefCtrl;
            params["RefCtrlHidden"] = RefCtrlHidden;
            params["RefCtrlLabel"] = RefCtrlLabel;
            params["RefCtrlLink"] = RefCtrlLink;
        }
        params["ParentCtrl"] = "Batch_ImportHL7ImmunizationBatch";
        LoadActionPan('Admin_Provider', params);
    },
    OpenFacility: function () {
        var params = [];
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Batch_ImportHL7ImmunizationBatch";
        LoadActionPan('Admin_Facility', params);
    },
    OpenProviderDetail: function (HiddenCtrl, TxtBoxCtrl) {
        var params = [];
        params["ProviderId"] = $('#' + Batch_ImportHL7ImmunizationBatch.params["PanelID"] + ' #divBatch ' + HiddenCtrl).val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = TxtBoxCtrl;
        params["ParentCtrl"] = 'Batch_ImportHL7ImmunizationBatch';
        LoadActionPan('providerDetail', params);
    },
    OpenFacilityDetail: function () {
        var params = [];
        params["FacilityId"] = $('#' + Batch_ImportHL7ImmunizationBatch.params.PanelID + ' #divBatch #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacility";
        params["ParentCtrl"] = 'Batch_ImportHL7ImmunizationBatch';
        LoadActionPan('facilityDetail', params);
    },
    BindProvider: function (Id, linkId, lblId, hfId, tabId) {
        if (tabId == 'divBatch') {
            if (!Id)
                Id = 'txtProvider';
            if (!linkId)
                linkId = 'lnkProviderEdit';
            if (!lblId)
                lblId = 'lblProvider';
            if (!hfId)
                hfId = 'hfProviderId';
        }
        else if (tabId == 'divOutBound') {
            if (!Id)
                Id = 'txtGivenBy';
            if (!linkId)
                linkId = 'lnkGivenByProviderEdit';
            if (!lblId)
                lblId = 'lblGivenByProvider';
            if (!hfId)
                hfId = 'hfGivenByProviderId';
        }

        var Ctrl = $("#" + Batch_ImportHL7ImmunizationBatch.params.PanelID + " #frmImportHL7ImmunizationBatch #" + tabId + " #" + Id);
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        var hfCtrl = $("#" + Batch_ImportHL7ImmunizationBatch.params.PanelID + " #frmImportHL7ImmunizationBatch #" + tabId + " #" + hfId);
        var onSelect = function (e) {
            if ($("#" + Batch_ImportHL7ImmunizationBatch.params.PanelID + " #frmImportHL7ImmunizationBatch #" + tabId + " #" + linkId).css("display") == "none") {
                $("#" + Batch_ImportHL7ImmunizationBatch.params.PanelID + " #frmImportHL7ImmunizationBatch #" + tabId + " #" + linkId).css("display", "inline");
                $("#" + Batch_ImportHL7ImmunizationBatch.params.PanelID + " #frmImportHL7ImmunizationBatch #" + tabId + " #" + lblId).css("display", "none");
            }
        };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect);
    },
    BindFacility: function (Id, linkId, lblId, hfId, tabId) {
        if (!Id)
            Id = 'txtFacility';
        if (!linkId)
            linkId = 'lnkFacilityEdit';
        if (!lblId)
            lblId = 'lblFacility';
        if (!hfId)
            hfId = 'hfFacilityId';

        var Ctrl = $("#" + Batch_ImportHL7ImmunizationBatch.params.PanelID + " #frmImportHL7ImmunizationBatch #" + tabId + " #" + Id);
        var func = function () { return utility.GetFacilityArray(Ctrl.val(), 0) };
        var hfCtrl = $("#" + Batch_ImportHL7ImmunizationBatch.params.PanelID + " #frmImportHL7ImmunizationBatch #" + tabId + " #" + hfId)
        var onSelect = function (e) {
            if ($("#" + Batch_ImportHL7ImmunizationBatch.params.PanelID + " #frmImportHL7ImmunizationBatch #" + tabId + " #" + linkId).css("display") == "none") {
                $("#" + Batch_ImportHL7ImmunizationBatch.params.PanelID + " #frmImportHL7ImmunizationBatch #" + tabId + " #" + linkId).css("display", "inline");
                $("#" + Batch_ImportHL7ImmunizationBatch.params.PanelID + " #frmImportHL7ImmunizationBatch #" + tabId + " #" + lblId).css("display", "none");
            }
        };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect);
    },
    BindPatient: function () {
        var Ctrl = $('#' + Batch_ImportHL7ImmunizationBatch.params["PanelID"] + ' #divOutBound #txtAccountNumber');
        var func = function () { return utility.GetPatientArray(Ctrl.val(), 0) };
        var hfCtrl = $("#" + Batch_ImportHL7ImmunizationBatch.params["PanelID"] + " #divOutBound #hfPatientId");
        var onSelect = function (e) { utility.InsertRecentPatient(e.id); };
        utility.BindKendoAutoComplete(Ctrl, 4, "value", "contains", null, func, hfCtrl, onSelect);
    },
    FillPatientInfoFromSearch: function (AccountNo, PatientId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        $('#' + Batch_ImportHL7ImmunizationBatch.params["PanelID"] + ' #divOutBound #hfPatientId').val(PatientId);
        $('#' + Batch_ImportHL7ImmunizationBatch.params["PanelID"] + ' #divOutBound #txtAccountNumber').val(AccountNo);
        if ($('#' + Batch_ImportHL7ImmunizationBatch.params["PanelID"] + ' #divOutBound #txtAccountNumber').data('kendoAutoComplete'))
            utility.SetKendoAutoCompleteSourceforValidate($('#' + Batch_ImportHL7ImmunizationBatch.params["PanelID"] + ' #divOutBound #txtAccountNumber'), AccountNo, $('#' + Batch_ImportHL7ImmunizationBatch.params["PanelID"] + ' #divOutBound #hfPatientId'), PatientId);
        UnloadActionPan(Batch_ImportHL7ImmunizationBatch.params["TabID"]);
        utility.InsertRecentPatient(PatientId);
    },
    VaccineHxPreview: function (VaccineHxId, VaccineScheduleId, VaccineCategoryId,VaccineCategory, Type, TabId, PatientID) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_Immunization", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                var PanelID = "";
                params["ParentCtrl"] = 'Batch_ImportHL7ImmunizationBatch';
                PanelID = 'pnlBatchImportHL7ImmunizationBatch';
                params["from"] = 'Batch_ImportHL7ImmunizationBatch';
                params["PanelID"] = 'pnlBatchImportHL7ImmunizationBatch';
                params["ParentPanelID"] = 'pnlBatchImportHL7ImmunizationBatch';
                params["VaccineHxId"] = VaccineHxId;
                params["OrderSetId"] = "";
                params["FromAdmin"] = 0;
                params["VaccineScheduleId"] = VaccineScheduleId;
                params["CategoryId"] = VaccineCategoryId;
                params["Category"] = VaccineCategory;
                params["VaccineHxId"] = VaccineHxId;
                params["Type"] = Type;
                params["TabId"] = "Batch";
                params["mode"] = "Edit";
                params["patientID"] = PatientID;
                LoadActionPan('Clinical_ImmunizationDetail', params, PanelID);
            } else
                utility.DisplayMessages(strMessage, 2);
        });
    },
}