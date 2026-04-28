paymentBatchDetail = {
    params: [],
    bIsFirstLoad: true,
    Load: function (params) {
        paymentBatchDetail.params = params;

        var self = null
        if (paymentBatchDetail.params.PanelID != "pnlPaymentBatchDetail")
            self = $('#' + paymentBatchDetail.params.PanelID + ' #pnlPaymentBatchDetail');
        else
            self = $('#' + paymentBatchDetail.params.PanelID);
        if (paymentBatchDetail.bIsFirstLoad) {
            paymentBatchDetail.LoadAllControls();
            paymentBatchDetail.bIsFirstLoad = false;
            self.loadDropDowns(true).done(function () {
                self.find('#ddlBatchStatus option').filter(function () { return $(this).val() == '1'; }).prop("selected", true);
                $('#' + paymentBatchDetail.params.PanelID + ' #frmPaymentBatchDetail').data('serialize', $('#' + paymentBatchDetail.params.PanelID + ' #frmPaymentBatchDetail').serialize());
                if (paymentBatchDetail.params.mode == "Add") {
                    paymentBatchDetail.EnableAndDisableBatchToolBar(false);
                    $('#' + paymentBatchDetail.params.PanelID + ' #ContainerBatchDocuments').hide();
                }
                else {
                    paymentBatchDetail.EnableAndDisableBatchToolBar(true);
                    paymentBatchDetail.paymentBatchFill(paymentBatchDetail.params.BatchId);
                    $('#' + paymentBatchDetail.params.PanelID + ' #BtnUpdate').show();
                    $('#' + paymentBatchDetail.params.PanelID + ' #btnSave').hide();
                    $('#' + paymentBatchDetail.params.PanelID + ' #hfBatchId').val(paymentBatchDetail.params.BatchId);
                }
            });
        }
        paymentBatchDetail.EnableAndDisableBatchStatusContainer(false);
        $('#' + paymentBatchDetail.params.PanelID + ' #frmPaymentBatchDetail').data('serialize', $('#' + paymentBatchDetail.params.PanelID + ' #frmPaymentBatchDetail').serialize());


    },

    //----------------Open Screens-------------------//
    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmPaymentBatchDetail";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "paymentBatchDetail";
        LoadActionPan('Admin_Facility', params);
    },
    OpenFacilityDetail: function () {
        var params = [];
        params["FacilityId"] = $("#" + paymentBatchDetail.params.PanelID + " #hfFacility").val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'paymentBatchDetail';
        params["RefCtrl"] = "txtFacility";
        LoadActionPan('facilityDetail', params);
    },
    OpenPractice: function () {
        var params = [];
        params["PracticeId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "paymentBatchDetail";
        LoadActionPan('Admin_Practice', params);
    },
    OpenPracticeDetail: function () {
        var params = [];
        params["PracticeId"] = $('#' + paymentBatchDetail.params.PanelID + ' #hfPractice').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtPractice";
        params["ParentCtrl"] = 'paymentBatchDetail';
        LoadActionPan('practiceDetail', params);
    },
    OpenImportDocument: function (mode) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Documents", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["RefCtrl"] = "paymentBatchDetail";
                params["FromAdmin"] = "0";
                params["mode"] = mode;
                params["ParentCtrl"] = 'paymentBatchDetail';
                params["BatchID"] = $('#' + paymentBatchDetail.params.PanelID + ' #hfBatchId').val();
                LoadActionPan('Document_Import', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    OpenScanDocument: function () {
        AppPrivileges.GetFormPrivileges("Documents", "SCAN", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var param = [];
                param["RefCtrl"] = "paymentBatchDetail";
                param["ParentCtrl"] = 'paymentBatchDetail';
                param["BatchID"] = $('#' + paymentBatchDetail.params.PanelID + ' #hfBatchId').val();
                LoadActionPan('Document_Scan', param);
            }
            else
                utility.DisplayMessages(strMessage, 2);

        });
    },
    PaymentPosting: function (event) {
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Payment Posting", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FromAdmin"] = 0;
                params["ParentCtrl"] = "paymentBatchDetail";
                params["BatchNumber"] = paymentBatchDetail.params.BatchNumber;
                params["BatchId"] = paymentBatchDetail.params.BatchId;
                params["CheckNumber"] = $("#" + paymentBatchDetail.params.PanelID + " #txtCheckNumber").val();
                params["CheckDate"] = $("#" + paymentBatchDetail.params.PanelID + " #dtpCheckDate").val();
                params["mode"] = "edit";
                params["OpenFrom"] = "paymentBatchDetail";
                LoadActionPan('Bill_PaymentPosting', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    //------------------------------------------//
    EnableAndDisableBatchStatusContainer: function (active) {
        if (!active) {
            $('#' + paymentBatchDetail.params.PanelID + ' #ContainerBatchStatus').find('input, textarea, button, select').each(function () {
                $(this).prop('disabled', true);
            });
        }
        else {
            $('#' + paymentBatchDetail.params.PanelID + ' #ContainerBatchStatus').find('input, textarea, button, select').each(function () {
                $(this).prop('disabled', false);
            });
        }
    },
    EnableAndDisableBatchToolBar: function (active) {
        if (!active) {
            $('#' + paymentBatchDetail.params.PanelID + ' #toolBar').find('a').each(function () {
                $(this).addClass('disabled');
            });
        }
        else {
            $('#' + paymentBatchDetail.params.PanelID + ' #toolBar').find('a').each(function () {
                $(this).removeClass('disabled');
            });
        }
    },
    LoadAllControls: function () {
        var self = null
        if (paymentBatchDetail.params.PanelID != "pnlPaymentBatchDetail")
            self = $('#' + paymentBatchDetail.params.PanelID + ' #pnlPaymentBatchDetail');
        else
            self = $('#' + paymentBatchDetail.params.PanelID);

        CacheManager.BindCodes('GetFacility', false).done(function (result) {
            var Ctrl = self.find("#frmPaymentBatchDetail #txtFacility");
            var hfCtrl = self.find("#hfFacility");
            var onSelect = function (e) {
                self.find("#txtPractice").val(e.Practice);
                self.find("#hfPractice").val(e.PracticeId);
            }
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl, onSelect);
        });
        CacheManager.BindCodes('GetPractice', false).done(function (result) {
            var Ctrl = self.find("#frmPaymentBatchDetail #txtPractice");
            var hfCtrl = self.find("#hfPractice");
            var onSelect = function (e) {
                self.find("#hfFacility").val('');
                self.find("#txtFacility").val('');
                self.find("#lnkFacilityEdit").css("display", "none");
                self.find("#lblFacility").css("display", "inline");
            }
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl, onSelect);
        });
        utility.CreateDatePicker(paymentBatchDetail.params.PanelID + ' #frmPaymentBatchDetail #dtpDepositDate,#dtpCheckDate,#dtpStarDate,#dtpEndDate', function (ev) { }, true);
    },
    GetImportDocuments: function (requestFiles) {
        var frmData = new FormData();
        var filenameFull = "";
        var FileType = "";
        var blob = "";
        var objDefIportSave = $.Deferred();
        var fileCount = Document_Import.FilesContainer.Files.length;
        var counter = 0;
        if (isFileCompressed) {
            $.each(requestFiles, function (key, value) {
                var objDef = $.Deferred();
                filenameFull = value.name;
                FileType = value.type;
                frmData.append("FileName", filenameFull);
                frmData.append("fileType", FileType);
                var zipFileToLoad = value;
                var fileReader = new FileReader();
                fileReader.addEventListener("load", function () {
                    blob = fileReader.result;
                    objDef.resolve("ok")
                }, false);
                if (zipFileToLoad) {
                    fileReader.readAsDataURL(zipFileToLoad);
                }
                objDef.then(function () {
                    var byteArrBase64 = blob.split(',')[1];
                    var zip = new JSZip();
                    zip.file(filenameFull, blob.split(',')[1], { base64: true });
                    zip.generateAsync({ type: "blob", compression: "DEFLATE", compressionOptions: { level: 9 } }).then(function (blob) {
                        frmData.append(key, blob);
                        counter = counter + 1;
                        if (fileCount == counter) {
                            objDefIportSave.resolve("ok")
                        }
                    });
                });
            });
        } else {
            $.each(requestFiles, function (key, value) {
                frmData.append("FileName", value.name);
                frmData.append("fileType", value.type);
                frmData.append(key, value, value.name);
                counter = counter + 1;
                if (fileCount == counter) {
                    objDefIportSave.resolve("ok")
                }
            });
        }
        frmData.append("BatchID", $('#' + paymentBatchDetail.params.PanelID + ' #hfBatchId').val());
        objDefIportSave.then(function () {
            paymentBatchDetail.SavePaymentBatchDocuments(frmData).done(function (response) {
                if (response.status != false) {
                    paymentBatchDetail.PaymentBatchDocumentSearch();
                    utility.DisplayMessages(response.message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        });
    },
    GetScannedDocuments: function (base64, fileType, FileName) {
        //if (Number((base64.length * 0.75 / (1024 * 1024))) > Number(globalAppdata['FileSize'])) {
        //    utility.DisplayMessages("Maximum " + Number(globalAppdata['FileSize']) + "MB  is allowed", 4);
        //    return false;
        //}
        //else {
        var objDef = $.Deferred();
        var data = new FormData();
        if (isFileCompressed) {
            var zip = new JSZip();
            zip.file(FileName, base64, { base64: true });
            zip.generateAsync({ type: "blob", compression: "DEFLATE", compressionOptions: { level: 9 } }).then(function (blob) {
                data.append(0, blob);
                objDef.resolve("ok")
            });
        } else {
            objDef.resolve("ok");
        }
        objDef.then(function () {
            data.append("base64", base64);
            data.append("fileType", fileType);
            data.append("BatchID", $('#' + paymentBatchDetail.params.PanelID + ' #hfBatchId').val());
            data.append("FileName", FileName);
            paymentBatchDetail.SavePaymentBatchDocuments(data).done(function (response) {
                if (response.status != false) {
                    paymentBatchDetail.PaymentBatchDocumentSearch();
                    utility.DisplayMessages(response.message, 1);
                    UnloadActionPan(Document_Scan.params.ParentCtrl, 'Document_Scan');
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        });
        //}
    },
    SavePaymentBatchDocuments: function (data) {
        // serach parameter , class name, command name of class 
        return MDVisionService.fileService(data, "BILLING_PAYMENT_BATCH_DETAIL", "ADD_PAYMENTBATCH_DOCUMENT");
    },
    OpenScanDocument: function () {
        AppPrivileges.GetFormPrivileges("Documents", "SCAN", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var param = [];
                param["RefCtrl"] = "paymentBatchDetail";
                param["ParentCtrl"] = 'paymentBatchDetail';
                param["BatchID"] = $('#' + paymentBatchDetail.params.PanelID + ' #hfBatchId').val();
                LoadActionPan('Document_Scan', param);
            }
            else
                utility.DisplayMessages(strMessage, 2);

        });
    },
    paymentBatchFill: function (batchId) {
        paymentBatchDetail.FillPaymentBatch(batchId).done(function (response) {
            if (response.status == true) {
                var self = null
                if (paymentBatchDetail.params.PanelID != "pnlPaymentBatchDetail")
                    self = $('#' + paymentBatchDetail.params.PanelID + ' #pnlPaymentBatchDetail');
                else
                    self = $('#' + paymentBatchDetail.params.PanelID);

                utility.bindMyJSON(true, JSON.parse(response.PaymentBatchLoad_JSON), false, self);
                $('#' + paymentBatchDetail.params.PanelID + " #ddlBatchStatus").removeAttr('disabled');
                paymentBatchDetail.PaymentBatchDocumentSearch();
                $('#' + paymentBatchDetail.params.PanelID + ' #frmPaymentBatchDetail').data('serialize', $('#' + paymentBatchDetail.params.PanelID + ' #frmPaymentBatchDetail').serialize());
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });
    },
    FillPaymentBatch: function (batchId) {
        var data = "BatchId=" + batchId;
        return MDVisionService.defaultService(data, "BILLING_PAYMENT_BATCH_DETAIL", "FILL_PAYMENT_BATCH");
    },
    BatchChargeUpdate: function (event) {
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Payment Batch", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var self = $("#pnlPaymentBatchDetail");
                var myJSON = self.getMyJSON();
                var batchId = paymentBatchDetail.params.BatchId;
                paymentBatchDetail.UpdateBatchCharge(myJSON, batchId).done(function (response) {
                    if (response.status == true) {
                        utility.DisplayMessages(response.message, 1);
                        $('#' + paymentBatchDetail.params.PanelID + ' #frmPaymentBatchDetail').data('serialize', $('#' + paymentBatchDetail.params.PanelID + ' #frmPaymentBatchDetail').serialize());
                        //**********Add condition by Azeem Raza Tayyab to fix bug #3067**********//
                        //**********Start**********//
                        if (paymentBatchDetail.params.ParentCtrl != "billTabPaymentPosting") {
                            Bill_PaymentBatchSearch.PaymentBatchSearch();
                        }
                        //**********End Bug#3067 Fix **********//
                    }
                    else {
                        utility.DisplayMessages(response.message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);



        });
    },
    UpdateBatchCharge: function (frmData, batchId) {
        var data = "paymentBatchData=" + frmData + "&BatchId=" + batchId;
        return MDVisionService.defaultService(data, "BILLING_PAYMENT_BATCH_DETAIL", "UPDATE_PAYMENT_BATCH");

    },
    paymentBatchSave: function () {
        AppPrivileges.GetFormPrivileges("Payment Batch", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var self = $('#' + paymentBatchDetail.params.PanelID + " #frmPaymentBatchDetail");
                var myJSON = self.getMyJSON();
                paymentBatchDetail.savePaymentBatch(myJSON).done(function (response) {
                    if (response) {
                        if (paymentBatchDetail.params.PanelID != "pnlPaymentBatchDetail") {
                            $('#' + paymentBatchDetail.params.PanelID + " #pnlPaymentBatchDetail #txtBatchNumber").val(response.BatchNumber);
                            $('#' + paymentBatchDetail.params.PanelID + " #pnlPaymentBatchDetail #hfBatchId").val(response.BatchID);
                        }
                        else {
                            $('#' + paymentBatchDetail.params.PanelID + " #txtBatchNumber").val(response.BatchNumber);
                            $('#' + paymentBatchDetail.params.PanelID + " #hfBatchId").val(response.BatchID);
                        }
                        paymentBatchDetail.params['BatchId'] = response.BatchID;
                        paymentBatchDetail.params["BatchNumber"] = response.BatchNumber;
                        paymentBatchDetail.EnableAndDisableBatchToolBar(true);
                        paymentBatchDetail.paymentBatchFill(paymentBatchDetail.params.BatchId);
                        //**********Add condition by Azeem Raza Tayyab to fix bug #3067**********//
                        //**********Start**********//
                        if (paymentBatchDetail.params.ParentCtrl != "billTabPaymentPosting") {
                            Bill_PaymentBatchSearch.PaymentBatchSearch();
                        }
                        //**********End Bug#3067 Fix **********//
                        $('#' + paymentBatchDetail.params.PanelID + ' #BtnUpdate').show();
                        $('#' + paymentBatchDetail.params.PanelID + ' #btnSave').hide();
                        CacheManager.BindCodes('GetPaymentBatch', true);
                        utility.DisplayMessages(response.message, 1);
                    }
                    else {

                    }

                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    savePaymentBatch: function (frmData) {
        var data = "PaymentBatchData=" + frmData;
        return MDVisionService.defaultService(data, "BILLING_PAYMENT_BATCH_DETAIL", "ADD_PAYMENTBATCH");
    },
    PaymentBatchDocumentSearch: function () {
        AppPrivileges.GetFormPrivileges("Payment Batch", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                $('#' + paymentBatchDetail.params.PanelID + ' #ContainerBatchDocuments').show();
                var batchID = $('#' + paymentBatchDetail.params.PanelID + ' #hfBatchId').val();
                paymentBatchDetail.SearchPaymentBatchDocument('0', batchID, '0').done(function (response) {
                    if (response.status != false) {
                        paymentBatchDetail.DocumentGridLoad(response);
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
    SearchPaymentBatchDocument: function (BatchDocId, BatchId, isFileStream) {
        var data = "BatchDocId=" + BatchDocId + "&BatchId=" + BatchId + "&isFileStream=" + isFileStream;
        return MDVisionService.defaultService(data, "BILLING_PAYMENT_BATCH_DETAIL", "SEARCH_PAYMENT_BATCH_DOCUMENT");
    },
    DocumentGridLoad: function (response) {
        $('#' + paymentBatchDetail.params.PanelID + ' #ContainerBatchDocuments #dgvChargeBatchDocument').dataTable().fnDestroy();
        $('#' + paymentBatchDetail.params.PanelID + ' #ContainerBatchDocuments #dgvChargeBatchDocument tbody').find("tr").remove();
        if (response.PaymentBatchDocumentCount > 0) {
            var DocumentLoad_JSONData = JSON.parse(response.PaymentBatchDocumentLoad_JSON);
            $.each(DocumentLoad_JSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "paymentBatchDetail.PaymentBatchDocumentEdit('" + item.PmtBthDocId + "',event);utility.SelectGridRow($(this));");
                $row.attr("id", "dgvChargeBatchDocument_row" + item.PmtBthDocId);
                $row.attr("BatchDocId", item.PmtBthDocId);
                $row.attr("BatchId", item.BatchId);
                var action = '<td><a class="btn  btn-xs" href="#" onclick="paymentBatchDetail.PaymentBatchDocumentDelete(' + item.PmtBthDocId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="paymentBatchDetail.PaymentBatchDocumentEdit(' + item.PmtBthDocId + ',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a></td>';
                $row.append('' + action + '<td>' + item.CheckNumber + '</td><td>' + utility.RemoveTimeFromDate(null, item.CheckDate) + '</td><td class="ellip150" data-toggle="tooltip" data-placement="left" title=' + item.FilePath + '>' + item.FilePath + '</td><td>' + item.FileType + '</td><td>' + item.Pages + '</td><td >' + item.ActionName + '</td><td >' + item.ReasonName + '</td><td class="ellip150" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '">' + item.Comments + '</td>');
                $('#' + paymentBatchDetail.params.PanelID + ' #ContainerBatchDocuments #dgvChargeBatchDocument tbody').last().append($row);

            });

        }
        else {
            $('#' + paymentBatchDetail.params.PanelID + ' #ContainerBatchDocuments #dgvChargeBatchDocument').DataTable({
                "language": {
                    "emptyTable": "No Documents Found for this Batch "
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });


        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable('#' + paymentBatchDetail.params.PanelID + ' #ContainerBatchDocuments #dgvChargeBatchDocument'));
        else
            $('#' + paymentBatchDetail.params.PanelID + ' #ContainerBatchDocuments #dgvChargeBatchDocument').DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

    },
    PaymentBatchDocumentDelete: function (BatChDocId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.myConfirm('1', function () {
            var selectedValue = BatChDocId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                AppPrivileges.GetFormPrivileges("Payment Batch", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        paymentBatchDetail.DeletePaymentBatchDocument(BatChDocId).done(function (response) {
                            if (response.status == true) {
                                paymentBatchDetail.PaymentBatchDocumentSearch();
                                utility.DisplayMessages(response.Message, 1);
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
        }, function () { });


    },
    DeletePaymentBatchDocument: function (BatchDocId) {
        var data = "BatchDocId=" + BatchDocId;
        return MDVisionService.defaultService(data, "BILLING_PAYMENT_BATCH_DETAIL", "DELETE_PAYMENT_BATCH_DOCUMENT");
    },
    PaymentBatchDocumentEdit: function (BatchDocId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#dgvChargeBatchDocument_row' + BatchDocId));
        AppPrivileges.GetFormPrivileges("Charge Batch", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["BatchDocId"] = BatchDocId;
                params["BatchId"] = $('#' + paymentBatchDetail.params.PanelID + ' #hfBatchId').val();
                params["BatchNumber"] = paymentBatchDetail.params.BatchNumber;
                params["ParentCtrl"] = "paymentBatchDetail";
                LoadActionPan('ChargeBatch_Viewer', params);


            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    ReCalculate: function () {
        paymentBatchDetail.paymentBatchFill(paymentBatchDetail.params.BatchId);
    },
    LoadPaymentByBatch: function () {
        var params = [];
        params["PaymentBatchID"] = paymentBatchDetail.params.BatchId;
        params["ParentCtrl"] = "paymentBatchDetail";
        LoadActionPan('Bill_PaymentByBatch', params);
    },
    LoadInsurancePaymentByBatch: function () {
        var params = [];
        params["PaymentBatchID"] = paymentBatchDetail.params.BatchId;
        params["ParentCtrl"] = "paymentBatchDetail";
        LoadActionPan('Bill_InsurancePaymentByBatch', params);
    },
    UnLoad: function () {





        if (paymentBatchDetail.params != null && paymentBatchDetail.params.ParentCtrl != null && paymentBatchDetail.params.PanelID != 'pnlPaymentBatchDetail') {
            UnloadActionPan(paymentBatchDetail.params.ParentCtrl, 'paymentBatchDetail', null, paymentBatchDetail.params.PanelID);
        }
        else {
            utility.UnLoadDialog(paymentBatchDetail.params.PanelID + ' #frmPaymentBatchDetail', function () {
                if (paymentBatchDetail.params != null && paymentBatchDetail.params.ParentCtrl != null) {
                    UnloadActionPan(paymentBatchDetail.params.ParentCtrl, "paymentBatchDetail");
                }
                else
                    UnloadActionPan(null, 'paymentBatchDetail');
            }, function () {
                UnloadActionPan(paymentBatchDetail.params.ParentCtrl, "paymentBatchDetail");
            });
        }
    },
}