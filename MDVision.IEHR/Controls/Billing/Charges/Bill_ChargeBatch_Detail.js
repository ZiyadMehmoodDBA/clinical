chargeBatchDetail = {
    params: [],
    bIsFirstLoad: true,
    Load: function (params) {

        chargeBatchDetail.params = params;
        var ObjDeferred = $.Deferred();

        if (chargeBatchDetail.bIsFirstLoad) {
            chargeBatchDetail.bIsFirstLoad = false;
            chargeBatchDetail.LoadAllControls();
            var self = null
            if (chargeBatchDetail.params.PanelID != "pnlChargeBatchDetail")
                self = $('#' + chargeBatchDetail.params.PanelID + ' #pnlChargeBatchDetail');
            else
                self = $('#' + chargeBatchDetail.params.PanelID);
            self.loadDropDowns(true).done(function () {
                self.find('#ddlBatchStatus option').filter(function () { return $(this).val() == '1'; }).prop("selected", true);
                ObjDeferred.resolve();
            });
        }
        else
            ObjDeferred.resolve();

        $.when(ObjDeferred).then(function () {

            if (chargeBatchDetail.params.mode == "edit") {
                chargeBatchDetail.EnableAndDisableBatchToolBar(true);
                chargeBatchDetail.BatchChargeFill(chargeBatchDetail.params.BatchId);
                $('#pnlChargeBatchDetail #BtnUpdate').show();
                $('#pnlChargeBatchDetail #BtnSave').hide();
            }
            else {
                chargeBatchDetail.EnableAndDisableBatchToolBar(false);
                $('#' + chargeBatchDetail.params.PanelID + ' #ContainerBatchDocuments').hide();
                chargeBatchDetail.LoadDefaultData();
                chargeBatchDetail.ValidateChargeBatch();
            }
            chargeBatchDetail.EnableAndDisableBatchStatusContainer(false);
            $('#' + chargeBatchDetail.params.PanelID + ' #frmChargeBatchDetail').data('serialize', $('#' + chargeBatchDetail.params.PanelID + ' #frmChargeBatchDetail').serialize());

        });

        if (chargeBatchDetail.params.PanelID == "pnlBillChargeBatchSearch" && chargeBatchDetail.params.mode == "edit") {
            $('#pnlChargeBatchDetail #frmChargeBatchDetail #btnImportFolder').hide();
            $('#pnlChargeBatchDetail #frmChargeBatchDetail #btnScanDoc').hide();
        }


    },

    // -------------- Facility ---------------------
    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmChargeBatchDetail";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "chargeBatchDetail";
        LoadActionPan('Admin_Facility', params);
    },
    OpenFacilityDetail: function () {
        var params = [];
        params["FacilityId"] = $('#pnlChargeBatchDetail #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'chargeBatchDetail';
        params["RefCtrl"] = "txtFacility";
        LoadActionPan('facilityDetail', params);
    },
    HideFacilityLink: function () {
        $('#pnlChargeBatchDetail #txtFacility').attr("FacilityId", "-1");
        $('#pnlChargeBatchDetail #hfFacility').val("-1");
        $('#pnlChargeBatchDetail #lnkFacilityEdit').css("display", "none");
        $('#pnlChargeBatchDetail #lblFacility').css("display", "inline");
    },
    // -------------- End Facility -----------------

    // -------------- Provider ---------------------
    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmChargeBatchDetail";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "chargeBatchDetail";
        LoadActionPan('Admin_Provider', params);
    },
    OpenProviderDetail: function () {
        var params = [];
        params["ProviderId"] = $('#pnlChargeBatchDetail #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'chargeBatchDetail';
        LoadActionPan('providerDetail', params);
    },
    HideProviderLink: function () {
        $('#pnlChargeBatchDetail #txtProvider').attr("ProviderId", "-1");
        $('#pnlChargeBatchDetail #hfProvider').val("-1");
        $("#pnlChargeBatchDetail #lnkProviderEdit").css("display", "none");
        $("#pnlChargeBatchDetail #lblProvider").css("display", "inline");
    },
    // -------------- End Provider -----------------

    //---------------DB Helper -----------------------
    ChargeBatchSave: function () {
        var self = $("#pnlChargeBatchDetail");
        var myJSON = self.getMyJSON();
        chargeBatchDetail.SaveChargeBatch(myJSON).done(function (response) {
            if (response.status != false) {
                $('#pnlChargeBatchDetail #hfBatchID').val(response.BatchID);
                $('#pnlChargeBatchDetail #txtBatchNumber').val(response.BatchNumber);
                chargeBatchDetail.params['BatchId'] = Number(response.BatchID);
                self.find('#BtnSave').hide();
                self.find('#BtnUpdate').show();
                chargeBatchDetail.EnableAndDisableBatchToolBar(true);
                chargeBatchDetail.BatchChargeFill(chargeBatchDetail.params.BatchId);
                Bill_ChargeBatchSearch.BatchChargeChargeSearch();
                utility.DisplayMessages(response.message, 1);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    SaveChargeBatch: function (BatchChargeData) {
        var data = "BatchChargeData=" + BatchChargeData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "BILLING_BATCHCHARGE_DETAIL", "ADD_BATCHCHARGE");
    },
    GetImportDocuments: function (requestFiles) {
        if (requestFiles != null) {
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
            frmData.append("BatchID", $('#pnlChargeBatchDetail #hfBatchID').val());
            objDefIportSave.then(function () {
                chargeBatchDetail.SaveChargeBatchDocuments(frmData).done(function (response) {
                    if (response.status != false) {
                        chargeBatchDetail.BatchChargeDocumentSearch();
                        utility.DisplayMessages(response.message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            });
        }
    },
    GetScannedDocuments: function (base64, fileType, FileName) {
        //if (Number((base64.length * 0.75 / (1024 * 1024))) > Number(globalAppdata['FileSize'])) {
        //    utility.DisplayMessages("Maximum " + Number(globalAppdata['FileSize']) + "MB  is allowed", 4);
        //    return false;
        //}
        //else {
        var data = new FormData();
        var objDef = $.Deferred();
        if (isFileCompressed) {
            var zip = new JSZip();
            zip.file(FileName + ".pdf", base64, { base64: true });
            zip.generateAsync({ type: "blob", compression: "DEFLATE", compressionOptions: { level: 9 } }).then(function (blob) {
                data.append(0, blob);
                objDef.resolve("ok");
            });
        } else {
            objDef.resolve("ok");
        }
        objDef.then(function () {
            data.append("base64", base64);
            data.append("fileType", fileType);
            data.append("BatchID", $('#pnlChargeBatchDetail #hfBatchID').val());
            data.append("FileName", FileName + ".pdf");
            chargeBatchDetail.SaveChargeBatchDocuments(data).done(function (response) {
                if (response.status != false) {
                    chargeBatchDetail.BatchChargeDocumentSearch();
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
    SaveChargeBatchDocuments: function (data) {
        // serach parameter , class name, command name of class 
        return MDVisionService.fileService(data, "BILLING_BATCHCHARGE_DETAIL", "ADD_BATCHCHARGE_DOCUMENT");
    },
    BatchChargeDocumentSearch: function () {
        var batchID = $('#pnlChargeBatchDetail #hfBatchID').val();
        chargeBatchDetail.SearchBatchChargeDocument('0', batchID, '0').done(function (response) {
            if (response.status != false) {
                $('#' + chargeBatchDetail.params.PanelID + ' #ContainerBatchDocuments').show();
                chargeBatchDetail.DocumentGridLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }


        });
    },
    SearchBatchChargeDocument: function (BatchDocId, BatchId, isFileStream) {
        var data = "BatchDocId=" + BatchDocId + "&BatchId=" + BatchId + "&isFileStream=" + isFileStream;
        return MDVisionService.defaultService(data, "BILLING_BATCHCHARGE_DETAIL", "SEARCH_BATCH_CHARGE_DOCUMENT");
    },
    DeleteBatchChargeDocument: function (BatchDocId) {
        var data = "BatchDocId=" + BatchDocId;
        return MDVisionService.defaultService(data, "BILLING_BATCHCHARGE_DETAIL", "DELETE_BATCH_CHARGE_DOCUMENT");
    },
    FillBatchCharge: function (batchId) {
        var data = "BatchId=" + batchId;
        return MDVisionService.defaultService(data, "BILLING_BATCHCHARGE_DETAIL", "FILL_BATCH_CHARGE");
    },
    BatchChargeUpdate: function () {
        var self = $("#pnlChargeBatchDetail");
        var myJSON = self.getMyJSON();
        var batchId = chargeBatchDetail.params.BatchId;
        chargeBatchDetail.UpdateBatchCharge(myJSON, batchId).done(function (response) {
            if (response.status == true) {
                Bill_ChargeBatchSearch.BatchChargeChargeSearch();
                $('#' + chargeBatchDetail.params.PanelID + ' #frmChargeBatchDetail').data('serialize', $('#' + chargeBatchDetail.params.PanelID + ' #frmChargeBatchDetail').serialize());
                utility.DisplayMessages(response.message, 1);
            }
            else {
                utility.DisplayMessages(response.message, 3);
            }
        });
    },
    UpdateBatchCharge: function (frmData, batchId) {
        var data = "batchChargeData=" + frmData + "&BatchId=" + batchId;
        return MDVisionService.defaultService(data, "BILLING_BATCHCHARGE_DETAIL", "UPDATE_BATCH_CHARGE");

    },
    ReCalculate: function () {
        chargeBatchDetail.BatchChargeFill(chargeBatchDetail.params.BatchId);
    },
    //--------------END ----------------------------------

    // -------------- Open  Screens ---------------------
    OpenImportDocument: function (mode) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Documents", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["RefCtrl"] = "chargeBatchDetail";
                params["FromAdmin"] = "0";
                params["mode"] = mode;
                params["ParentCtrl"] = 'chargeBatchDetail';
                params["BatchID"] = $('#pnlChargeBatchDetail #hfBatchID').val();
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
                param["RefCtrl"] = "chargeBatchDetail";
                param["ParentCtrl"] = 'chargeBatchDetail';
                param["BatchID"] = $('#pnlChargeBatchDetail #hfBatchID').val();
                LoadActionPan('Document_Scan', param);
            }
            else
                utility.DisplayMessages(strMessage, 2);

        });
    },
    LoadClaimView: function () {
        var params = [];
        params["BatchId"] = chargeBatchDetail.params.BatchId;
        params["BatchNumber"] = chargeBatchDetail.params.BatchNumber;
        params["ParentCtrl"] = "chargeBatchDetail";
        LoadActionPan('claimViewDetail', params);
    },
    LoadChargeView: function () {
        var params = [];
        params["BatchId"] = chargeBatchDetail.params.BatchId;
        params["BatchNumber"] = chargeBatchDetail.params.BatchNumber;
        params["ParentCtrl"] = "chargeBatchDetail";
        LoadActionPan('chargesViewDetail', params);
    },
    // -------------- End -----------------

    //--------------HELPERS ----------
    LoadAllControls: function () {
        CacheManager.BindCodes('GetFacility', false).done(function (result) {
            var Ctrl = $("#frmChargeBatchDetail #txtFacility");
            var hfCtrl = $("#pnlChargeBatchDetail #hfFacility");
            var onSelect = function (e) { $("#pnlChargeBatchDetail #txtPractice").val(e.Practice); $("#pnlChargeBatchDetail #hfPractice").val(e.PracticeId); }
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl, onSelect);
        });
        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            var Ctrl = $("#frmChargeBatchDetail #txtProvider");
            var hfCtrl = $("#pnlChargeBatchDetail #hfProvider");
            var onSelect = function (e) { $("#pnlChargeBatchDetail #txtProvider").attr("ProviderId", e.id); }
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, onSelect);
        });
        CacheManager.BindCodes('GetPractice', false).done(function (result) {
            var Ctrl = $("#frmChargeBatchDetail #txtPractice");
            var hfCtrl = $("#frmChargeBatchDetail #hfPractice");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Practices, null, hfCtrl);
        });
        utility.ValidateFromToDate('frmChargeBatchDetail', 'dtpFromDOS', 'dtpToDOS', true, function () {

            //from date change callback
            chargeBatchDetail.toDateRequired();

        },

        function () {

            //to date change callback
            //var formValidation = $('#' + chargeBatchDetail.params.PanelID + ' #frmChargeBatchDetail').data("bootstrapValidator");
            if ($('#' + chargeBatchDetail.params.PanelID + ' #frmChargeBatchDetail').data('bootstrapValidator') != null && typeof $('#' + chargeBatchDetail.params.PanelID + ' #frmChargeBatchDetail').data('bootstrapValidator') != 'undefined') {
                $('#' + chargeBatchDetail.params.PanelID + ' #frmChargeBatchDetail').bootstrapValidator('revalidateField', 'DOSTo');
            }

            //formValidation.enableFieldValidators('DOSTo', false);

        }

        );
        utility.ValidateFromToDate('frmChargeBatchDetail', 'dtpStarDate', 'dtpEndDate', true);
    },

    OpenPractice: function () {
        var params = [];
        params["PracticeId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "chargeBatchDetail";
        LoadActionPan('Admin_Practice', params);
    },

    LoadDefaultData: function () {
        var self = null;
        if (chargeBatchDetail.params.ParentCtrl != null)
            self = $('#' + chargeBatchDetail.params.PanelID + ' #pnlChargeBatchDetail');
        else
            self = $('#' + chargeBatchDetail.params.PanelID);

        if (globalAppdata['DefaultProviderName'] != "") {
            self.find('#txtProvider').val(globalAppdata['DefaultProviderName']);
            self.find('#hfProvider').val(globalAppdata['DefaultProviderId']);
            self.find('#lnkProviderEdit').css("display", "inline");
            self.find('#lblProvider').css("display", "none");
        }
        if (globalAppdata['DefaultFacilityName'] != "") {
            self.find('#txtFacility').val(globalAppdata['DefaultFacilityName']);
            self.find('#hfFacility').val(globalAppdata['DefaultFacilityId']);
            self.find('#lnkFacilityEdit').css("display", "inline");
            self.find('#lblFacility').css("display", "none");
        }
        //utility.CreateDatePicker(chargeBatchDetail.params.PanelID + ' #frmChargeBatchDetail #dtpFromDOS,#dtpToDOS',
        //function (ev){

        //    //on-change callback method 
        //},true);
    },
    EnableAndDisableBatchStatusContainer: function (active) {
        if (!active) {
            $('#pnlChargeBatchDetail #ContainerBatchStatus').find('input, textarea, button, select').each(function () {
                $(this).prop('disabled', true);
            });
        }
        else {
            $('#pnlChargeBatchDetail #ContainerBatchStatus').find('input, textarea, button, select').each(function () {
                $(this).prop('disabled', false);
            });
        }
    },
    EnableAndDisableBatchToolBar: function (active) {
        if (!active) {
            $('#pnlChargeBatchDetail #chageBatchToolbar').find('a').each(function () {
                $(this).addClass('disabled');
            });
        }
        else {
            $('#pnlChargeBatchDetail #chageBatchToolbar').find('a').each(function () {
                $(this).removeClass('disabled');
            });
        }
    },
    BatchChargeFill: function (batchId) {
        chargeBatchDetail.FillBatchCharge(batchId).done(function (response) {
            if (response.status == true) {

                var self = $('#pnlChargeBatchDetail');
                utility.bindMyJSON(true, JSON.parse(response.BatchChargeLoad_JSON), false, self);
                chargeBatchDetail.params.BatchNumber = $.parseJSON(response.BatchChargeLoad_JSON).txtBatchNumber;
                $('#' + chargeBatchDetail.params.PanelID + " #ddlBatchStatus").prop('disabled', false);
                chargeBatchDetail.BatchChargeDocumentSearch();
                $('#' + chargeBatchDetail.params.PanelID + ' #frmChargeBatchDetail #txtCopaymentCollected').val(utility.convertToFigure($('#' + chargeBatchDetail.params.PanelID + ' #frmChargeBatchDetail #txtCopaymentCollected').val()));
                $('#' + chargeBatchDetail.params.PanelID + ' #frmChargeBatchDetail').data('serialize', $('#' + chargeBatchDetail.params.PanelID + ' #frmChargeBatchDetail').serialize());

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });
    },
    ValidateChargeBatch: function () {
        $('#' + chargeBatchDetail.params.PanelID + ' #frmChargeBatchDetail')
           .bootstrapValidator({
               //live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {


                   Facility: {
                       group: '.col-sm-3',
                       enabled: true,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },


                   DOSFrom: {
                       group: '.col-sm-3',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },

                   DOSTo: {
                       group: '.col-sm-3',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },

               },





           }).on('success.form.bv', function (e) {
               e.preventDefault();
               chargeBatchDetail.ChargeBatchSave();
           });


    },
    //------------END - ----------------------



    //------------------- GRID FUNCTION -------------------

    DocumentGridLoad: function (response) {
        $('#pnlChargeBatchDetail #ContainerBatchDocuments #dgvChargeBatchDocument').dataTable().fnDestroy();

        $('#pnlChargeBatchDetail #ContainerBatchDocuments #dgvChargeBatchDocument tbody').find("tr").remove();


        if (response.BatchChargeDocumentCount > 0) {
            var DocumentLoad_JSONData = JSON.parse(response.BatchChargeDocumentLoad_JSON);
            $.each(DocumentLoad_JSONData, function (i, item) {
                var DemographicsMethod = "utility.PatientDemographics('" + item.PatientId + "', 'chargeBatchDetail', event);";
                var VisitDetail = "utility.LoadVisitDetail('" + item.VisitId + "', '" + item.PatientId + "', 'chargeBatchDetail', event, true);";
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#dgvChargeBatchDocument_row" + item.BatchDocId + "'))");
                $row.attr("id", "dgvChargeBatchDocument_row" + item.BatchDocId);
                $row.attr("BatchDocId", item.BatchDocId);
                $row.attr("BatchId", item.BatchId);
                var action = '<td><a class="btn  btn-xs" href="#" onclick="chargeBatchDetail.BatchChargeDocumentDelete(' + item.BatchDocId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="chargeBatchDetail.BatchChargeDocumentEdit(' + item.BatchDocId + '   );"   title="Edit Record"><i class="fa fa-edit black"></i></a></td>';
                $row.append('' + action + '<td patientId="' + item.PatientId + '"><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.AccountNumber + '</a></td><td><a href="#" onclick="' + VisitDetail + '"  title="View Claim Detail">' + item.ClaimNumber + '</a></td><td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td><td class="ellip150" data-toggle="tooltip" data-placement="left" title=' + item.FilePath + ' >' + item.FilePath + '</td><td>' + item.FileType + '</td><td>' + item.Pages + '</td><td>' + item.ActionName + '</td><td>' + item.ReasonName + '</td><td class="ellip150" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '">' + item.Comments + '</td>');
                if (item.ClaimNumber != "") {
                    $('#' + chargeBatchDetail.params.PanelID + " #ddlBatchStatus").removeAttr('disabled');
                }
                $('#pnlChargeBatchDetail #ContainerBatchDocuments #dgvChargeBatchDocument tbody').last().append($row);

            });


        }



        else {
            $('#pnlChargeBatchDetail #ContainerBatchDocuments #dgvChargeBatchDocument').DataTable({
                "language": {
                    "emptyTable": "No Documents Found for this Batch "
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });


        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });

        if ($.fn.dataTable.isDataTable('#pnlChargeBatchDetail #ContainerBatchDocuments #dgvChargeBatchDocument'));
        else
            $('#pnlChargeBatchDetail #ContainerBatchDocuments #dgvChargeBatchDocument').DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

    },
    BatchChargeDocumentDelete: function (BatChDocId) {
        utility.myConfirm('1', function () {
            var selectedValue = BatChDocId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                AppPrivileges.GetFormPrivileges("Charge Batch", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        chargeBatchDetail.DeleteBatchChargeDocument(BatChDocId).done(function (response) {
                            if (response.status == true) {
                                chargeBatchDetail.BatchChargeDocumentSearch();
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
    BatchChargeDocumentEdit: function (BatChDocId) {
        AppPrivileges.GetFormPrivileges("Charge Batch", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["BatchDocId"] = BatChDocId;
                params["BatchId"] = $('#pnlChargeBatchDetail #hfBatchID').val();
                params["BatchNumber"] = chargeBatchDetail.params.BatchNumber;
                params["ParentCtrl"] = "chargeBatchDetail";
                LoadActionPan('ChargeBatch_Viewer', params);


            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    //--------------------END GRID FUNCTION ---------------

    UnLoad: function () {

        utility.UnLoadDialog(chargeBatchDetail.params.PanelID + ' #frmChargeBatchDetail', function () {
            UnloadActionPan(chargeBatchDetail.params["ParentCtrl"], "chargeBatchDetail");
        }, function () {
            UnloadActionPan(chargeBatchDetail.params["ParentCtrl"], "chargeBatchDetail");
        });

    },


    toDateRequired: function () {


        var formValidation = $('#' + chargeBatchDetail.params.PanelID + ' #frmChargeBatchDetail').data("bootstrapValidator");
        if ($('#' + chargeBatchDetail.params.PanelID + ' #frmChargeBatchDetail').data('bootstrapValidator') != null && typeof $('#' + chargeBatchDetail.params.PanelID + ' #frmChargeBatchDetail').data('bootstrapValidator') != 'undefined') {
            if ($('#' + chargeBatchDetail.params.PanelID + ' #frmChargeBatchDetail #dtpFromDOS').val() != "") {
                formValidation.enableFieldValidators('DOSTo', true);
            }
            else {
                formValidation.enableFieldValidators('DOSTo', false);
            }
        }
    },

}