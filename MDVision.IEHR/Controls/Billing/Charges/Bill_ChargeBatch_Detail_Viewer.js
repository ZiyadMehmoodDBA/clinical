ChargeBatch_Viewer = {
    params: [],
    bIsFirstLoad: true,
    bVisitFirst: true,
    interval: null,
    needCanvasReset: false,
    isExtendedMode: true,
    Load: function (params) {

        var ObjDeferred = $.Deferred();
        ChargeBatch_Viewer.params = params;

        var self = null;
        if (ChargeBatch_Viewer.params.PanelID == "pnlChargeBatchViewer")
            self = $('#' + ChargeBatch_Viewer.params.PanelID);
        else
            self = $('#' + ChargeBatch_Viewer.params.PanelID + ' #pnlChargeBatchViewer');

        if (ChargeBatch_Viewer.bIsFirstLoad) {
            if (ChargeBatch_Viewer.params.ParentCtrl == "chargeBatchDetail") {
                ChargeBatch_Viewer.bIsFirstLoad = false;
                self.find('#titleDocumentViewer').text('Charge Batch Document Viewer');
                self.find('#toolbarbatchCharge').show();
                self.find('#chargeBatchViewerContainer').show();
            }
            else if (ChargeBatch_Viewer.params.ParentCtrl == "paymentBatchDetail") {
                self.find('#titleDocumentViewer').text('Payment Batch Document Viewer');
                self.find('#lnkPaymentPosting').show();
                self.find('#PaymentBatchContainer').show();
                self.find('#dpCheckDate').show();
                utility.CreateDatePicker(ChargeBatch_Viewer.params.PanelID + ' #dpCheckDate', function (ev) { }, false);
            }
            else if (ChargeBatch_Viewer.params.ParentCtrl == "EncounterChargeCapture") {
                ChargeBatch_Viewer.bIsFirstLoad = false;
                self.find('#titleDocumentViewer').text('Charge Batch Document Viewer');
                self.find('#lnkNextDocument').hide();
                self.find('#lnkPreviousDocument').hide();
                self.find('#toolbarbatchCharge').hide();
                self.find('#chargeBatchViewerContainer').show();
                self.find('#txtClaimNumber').prop('disabled', true);
                self.find('#lnkClaimNumber').prop('disabled', true);
            }

            self.loadDropDowns(true).done(function () {
                if (ChargeBatch_Viewer.params.ParentCtrl == "chargeBatchDetail") {
                    ChargeBatch_Viewer.ValidateViewer();
                } else if (ChargeBatch_Viewer.params.ParentCtrl == "paymentBatchDetail") {
                    ChargeBatch_Viewer.ValidateViewerPayment();
                }
                ObjDeferred.resolve();
            });
        }
        else
            ObjDeferred.resolve();

        $.when(ObjDeferred).then(function () {
            if (ChargeBatch_Viewer.params.ParentCtrl == "chargeBatchDetail" || ChargeBatch_Viewer.params.ParentCtrl == "EncounterChargeCapture") {
                ChargeBatch_Viewer.LoadPatientCase("-1");
            }
            ChargeBatch_Viewer.DocumentFill();
        });


    },
    //__________Open Screens ____________  //
    OpenPatientSearch: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'ChargeBatch_Viewer';
        LoadActionPan('Patient_Search', params);
    },
    FillPatientInfoFromSearch: function (PatientId, patFullName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        if ($("#" + ChargeBatch_Viewer.params.PanelID + " #frmChargeBatchViewer #txtPatientName").data('kendoAutoComplete'))
            utility.SetKendoAutoCompleteSourceforValidate($("#" + ChargeBatch_Viewer.params.PanelID + " #txtPatientName"), patFullName, $("#" + ChargeBatch_Viewer.params.PanelID + " #hfPatientId"), PatientId, "value");
        ChargeBatch_Viewer.BatchChargeDocumentUpdate(false);
        $('#' + ChargeBatch_Viewer.params.PanelID + ' #lnkChargeCapture').removeClass('disableAll');
        UnloadActionPan("ChargeBatch_Viewer");
        utility.InsertRecentPatient(PatientId);
    },
    BindPatientAccount: function (fromFill) {
        var valid = false;
        var Ctrl = $("#" + ChargeBatch_Viewer.params.PanelID + " #txtPatientName");
        var hfCtrl = $("#" + ChargeBatch_Viewer.params.PanelID + " #hfPatientId");
        var onChange = function () {
            var id_;
            var value_;
            var link = $(Ctrl).parent().parent().prev('a');
            var data = this.dataSource.data();
            var haveObject = data.filter(function (obj) {
                if ((obj.value && obj.value.toLowerCase() == $(Ctrl).val().toLowerCase()) || (obj.FullName && obj.FullName.toLowerCase() == $(Ctrl).val().toLowerCase())) {
                    id_ = obj.id;
                    value_ = obj.value;
                    return true;
                }
                else { return false; }
            });
            if (haveObject.length > 0) {
                if(hfCtrl)
                    $(hfCtrl).val(id_);
                this.value(value_);
                $(link).show();
                $(link).prev().hide();
            }

            else {
                if (hfCtrl)
                    $(hfCtrl).val('');
                this.value('');
                $(link).hide();
                $(link).prev().show();
            }
        };
        var onSelect = function (e) {
            var dataItem = this.dataItem(e.item.index());
            Ctrl.val(dataItem.AccountNumber + ' - ' + dataItem.FullName);
            $(hfCtrl).val(dataItem.id);
            ChargeBatch_Viewer.LoadPatientCase(dataItem.id);
            ChargeBatch_Viewer.BatchChargeDocumentUpdate(false);
            $('#' + ChargeBatch_Viewer.params.PanelID + ' #lnkChargeCapture').removeClass('disableAll');
            utility.InsertRecentPatient(dataItem.id);
        }

        if (Ctrl.data("kendoAutoComplete"))
            Ctrl.data("kendoAutoComplete").destroy();

        $(Ctrl).kendoAutoComplete({
            dataTextField: 'value',
            filter: 'contains',
            minLength: 4,
            select: onSelect,
            change: onChange,
            dataSource: {
                serverFiltering: true,
                transport: {
                    read: function (e) {
                        utility.GetPatientArrayByName(Ctrl.val(), 1).done(function (response) {
                            e.success(response);
                        });
                    },
                }
            },
        });
    },
    OpenEncounter: function () {
        var params = [];
        params["FromAdmin"] = 0;
        params["ParentCtrl"] = 'ChargeBatch_Viewer';
        if ($("#" + ChargeBatch_Viewer.params.PanelID + " #txtPatientName").val().trim() == "")
            params["patientID"] = 0;
        else
            params["patientID"] = Number($('#' + ChargeBatch_Viewer.params.PanelID + ' #hfPatientId').val());
        LoadActionPan('Encounter_Visits', params);
        //$('#CloseVisits').remove();
        // $('#OpenVisits').remove();
        if (ChargeBatch_Viewer.bVisitFirst) {
            $($('body #OpenVisits')[0]).attr('id', 'OpenVisits1')
            $($('body #CloseVisits')[0]).attr('id', 'CloseVisits1');
            ChargeBatch_Viewer.bVisitFirst = false;
        }

    },
    FillClaimNumberFromSearch: function (ClaimNumber, AccountNumber, PatientName, PatientId, DOSFrom, VisitId) {
        //$("#" + ChargeBatch_Viewer.params.PanelID + " #txtClaimNumber").val(ClaimNumber + ' - ( ' + AccountNumber + ' - ' + PatientName + ' )');
        $("#" + ChargeBatch_Viewer.params.PanelID + " #txtClaimNumber").val(ClaimNumber);
        $("#" + ChargeBatch_Viewer.params.PanelID + " #dpDOSfrm").val(utility.RemoveTimeFromDate(null, DOSFrom));
        $("#" + ChargeBatch_Viewer.params.PanelID + " #hfPatientId").val(PatientId);
        $("#" + ChargeBatch_Viewer.params.PanelID + " #txtPatientName").val(AccountNumber + ' - ' + PatientName);
        $("#" + ChargeBatch_Viewer.params.PanelID + " #hfVisitId").val(VisitId);
        ChargeBatch_Viewer.LoadPatientCase(PatientId);


        //UnloadActionPan("ChargeBatch_Viewer");
        Encounter_Visits.UnLoad();
    },
    BindClaimNumber: function () {
        $("#" + ChargeBatch_Viewer.params.PanelID + " #txtClaimNumber").autocomplete({
            autoFocus: true,
            source: function (request, response) {
                var ClaimNumber = $('#' + ChargeBatch_Viewer.params.PanelID + ' #txtClaimNumber').val();
                if (ClaimNumber.length > 2) {
                    ChargeBatch_Viewer.LoadClaimNumers(ClaimNumber).done(function (responseData) {
                        if (responseData.status != false) {
                            if (responseData.ClaimsCount > 0) {
                                var Claims = JSON.parse(responseData.ClaimsLoad_JSON);
                                var AllClaimsVisits = [];
                                $.each(Claims, function (i, item) {
                                    AllClaimsVisits.push({ id: item.VisitId, value: item.ClaimNumber + ' - ( ' + item.AccountNumber + ' - ' + item.PatientName + ' )', PatientId: item.PatientId, DOSFrom: item.DOSFrom, PatientName: item.AccountNumber + ' - ' + item.PatientName, ClaimNumber: item.ClaimNumber });
                                });
                                response(AllClaimsVisits);
                            }
                        }
                    });
                }
            },

            select: function (event, ui) {
                setTimeout(function () {
                    $("#" + ChargeBatch_Viewer.params.PanelID + " #hfVisitId").val(ui.item.id);
                    $("#" + ChargeBatch_Viewer.params.PanelID + " #dpDOSfrm").val(ui.item.DOSFrom);
                    $("#" + ChargeBatch_Viewer.params.PanelID + " #hfPatientId").val(ui.item.PatientId);
                    $("#" + ChargeBatch_Viewer.params.PanelID + " #txtPatientName").val(ui.item.PatientName);
                    $("#" + ChargeBatch_Viewer.params.PanelID + " #txtClaimNumber").val(ui.item.ClaimNumber);
                    ChargeBatch_Viewer.LoadPatientCase(ui.item.PatientId);
                }, 100);

                //$("#hfpatientid").val(ui.item.id);
            }
        });
    },
    LoadClaimNumers: function (claimNumber) {
        var data = "ClaimNumber=" + claimNumber;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "BILLING_BATCHCHARGE_DETAIL", "SEARCH_VISIT_CLAIM");
    },
    LoadPatientCase: function (PatientId) {
        $('#' + ChargeBatch_Viewer.params.PanelID + " input#txtCaseNumber").val('');
        $("#" + ChargeBatch_Viewer.params.PanelID + " #hfCaseId").val('');
        CacheManager.BindPatientData('GetPatientCase', true, PatientId).done(function (result) {
            $('#' + ChargeBatch_Viewer.params.PanelID + " input#txtCaseNumber").autocomplete({
                autoFocus: true,
                source: PatientCase, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#" + ChargeBatch_Viewer.params.PanelID + " #hfCaseId").val(ui.item.id); // add the selected id
                        if ($("#" + ChargeBatch_Viewer.params.PanelID + " #lnkCaseNumberEdit").css("display") == "none") {
                            $("#" + ChargeBatch_Viewer.params.PanelID + " #lnkCaseNumberEdit").css("display", "inline");
                            $("#" + ChargeBatch_Viewer.params.PanelID + " #lblCaseNumber").css("display", "none");
                        }
                    }, 100);
                }
            });
        });
    },
    OpenCaseDetail: function (HiddenCtrl) {
        var params = [];
        params["CaseId"] = parseInt($('#' + ChargeBatch_Viewer.params["PanelID"] + ' #' + HiddenCtrl).val());
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["PatientId"] = $("#" + ChargeBatch_Viewer.params["PanelID"] + " #hfPatientId").val();
        params["RefCtrl"] = "txtCaseNumber";
        params["ParentCtrl"] = "ChargeBatch_Viewer";
        LoadActionPan('Patient_Case_Detail', params);
    },
    OpenCase: function (Title, RefCtrl, RefCtrlHidden, RefCtrlLabel, RefCtrlLink) {
        var params = [];
        var PanelID = ChargeBatch_Viewer.params["TabID"];
        params["Title"] = Title;
        params["RefCtrl"] = RefCtrl;
        params["RefCtrlHidden"] = RefCtrlHidden;
        params["RefCtrlLabel"] = RefCtrlLabel;
        params["RefCtrlLink"] = RefCtrlLink;
        params["CaseId"] = "-1";
        params["patientID"] = $("#" + ChargeBatch_Viewer.params["PanelID"] + " #hfPatientId").val();
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "ChargeBatch_Viewer";
        LoadActionPan('Patient_Case', params);
    },
    OpenChargeCapture: function () {
        var params = [];
        params["FromAdmin"] = 0;
        params["ParentCtrl"] = 'chargeBatchDetail';

        //if ($("#" + ChargeBatch_Viewer.params.PanelID + " #txtPatientName").val().trim() == "")
        //    params["patientID"] = "-1";
        //else
        //    params["patientID"] = Number($('#' + ChargeBatch_Viewer.params.PanelID + ' #hfPatientId').val());

        if ($("#" + ChargeBatch_Viewer.params.PanelID + " #txtClaimNumber").val().trim() == "" && $("#" + ChargeBatch_Viewer.params.PanelID + " #txtPatientName").val().trim() == "")
            params["patientID"] = "-1";
        else if ($("#" + ChargeBatch_Viewer.params.PanelID + " #txtClaimNumber").val().trim() == "" && $("#" + ChargeBatch_Viewer.params.PanelID + " #txtPatientName").val().trim() != "") {
            params["pID"] = Number($('#' + ChargeBatch_Viewer.params.PanelID + ' #hfPatientId').val());
            params["pAccountNumber"] = $('#' + ChargeBatch_Viewer.params.PanelID + ' #txtPatientName').val();
        }
        else {
            params["VisitId"] = $("#" + ChargeBatch_Viewer.params.PanelID + " #hfVisitId").val();
            params["patientID"] = Number($('#' + ChargeBatch_Viewer.params.PanelID + ' #hfPatientId').val());
        }
        params['BatchNumber'] = ChargeBatch_Viewer.params.BatchNumber;
        params['BatchId'] = ChargeBatch_Viewer.params.BatchId;
        params['flag'] = true;
        params['mode'] = "Edit";
        LoadActionPan('EncounterChargeCapture', params);
        $('#' + ChargeBatch_Viewer.params.PanelID + ' #actionPanChargeBatchDetail').draggable({
            handle: "#pnlChargeBatchViewer .modal-content"
        });
        $('#' + ChargeBatch_Viewer.params.PanelID + ' #alternateActionPanChargeBatchDetail').draggable({
            handle: "#pnlEncounterChargeCapture .modal-content",
            start: function (event, ui) { $('#' + ChargeBatch_Viewer.params.PanelID + ' #alternateActionPanChargeBatchDetail #containerModalDialog').addClass('mt-none') },
        });
        $('#' + ChargeBatch_Viewer.params.PanelID + ' #alternateActionPanChargeBatchDetail').css('overflow-y', 'scroll');

        $('#' + ChargeBatch_Viewer.params.PanelID + ' #alternateActionPanChargeBatchDetail #modalbody').css('height', '550px');
        $('#' + ChargeBatch_Viewer.params.PanelID + ' #alternateActionPanChargeBatchDetail #modalbody').removeClass('panel-body tab-content-featured tabs-custome-panel-body mt-xs');
        $('#' + ChargeBatch_Viewer.params.PanelID + ' #alternateActionPanChargeBatchDetail #modalbody').css('overflow', 'auto');
        $('#' + ChargeBatch_Viewer.params.PanelID + ' #actionPanChargeBatchDetail').css('overflow-y', 'hidden');
        //$('#' + ChargeBatch_Viewer.params.PanelID + ' #actionPanChargeBatchDetail .panel-body').css('height', '600px');
        $('#' + ChargeBatch_Viewer.params.PanelID + ' #actionPanChargeBatchDetail .panel-body').css('overflow', 'auto');

        $('#' + ChargeBatch_Viewer.params.PanelID + ' #actionPanChargeBatchDetail').css('z-index', 100);
        $('#' + ChargeBatch_Viewer.params.PanelID + ' #alternateActionPanChargeBatchDetail').css('z-index', 200);

        $('#' + ChargeBatch_Viewer.params.PanelID + ' #actionPanChargeBatchDetail .modal-dialog .modal-content').click(function (ev) {
            if (!ev.target.id && ev.target.id != 'lnkChargeCapture') {
                document.getElementById(ChargeBatch_Viewer.params.PanelID).querySelector('#actionPanChargeBatchDetail').querySelector('.modal-dialog-full').setAttribute("style", "max-width:96%;margin:0 !important");
                $('#' + ChargeBatch_Viewer.params.PanelID + ' #alternateActionPanChargeBatchDetail').css('z-index', 100);
                $('#' + ChargeBatch_Viewer.params.PanelID + ' #actionPanChargeBatchDetail').css('z-index', 200);

            }
        });
        $('#' + ChargeBatch_Viewer.params.PanelID + ' #alternateActionPanChargeBatchDetail .modal-dialog .modal-content').click(function (ev) {
            $('#' + ChargeBatch_Viewer.params.PanelID + ' #alternateActionPanChargeBatchDetail #pnlEncounterChargeCapture').removeClass('row');
            document.getElementById(ChargeBatch_Viewer.params.PanelID).querySelector('#alternateActionPanChargeBatchDetail').querySelector('#containerModalDialog').setAttribute("style", "max-width:96%;margin:0 !important");
            $('#' + ChargeBatch_Viewer.params.PanelID + ' #alternateActionPanChargeBatchDetail').css('z-index', 200);
            $('#' + ChargeBatch_Viewer.params.PanelID + ' #actionPanChargeBatchDetail').css('z-index', 100);

        });

        ChargeBatch_Viewer.ActiveBtns(false);


        return false;
    },
    BindVisitAndClaimNummer: function (visitId, claimNumber, patientName, PatientID) {
        $('#' + ChargeBatch_Viewer.params.PanelID + ' #txtClaimNumber').val(claimNumber);
        $('#' + ChargeBatch_Viewer.params.PanelID + ' #hfVisitId').val(visitId);
        $('#' + ChargeBatch_Viewer.params.PanelID + ' #txtPatientName').val(patientName);
        $('#' + ChargeBatch_Viewer.params.PanelID + ' #hfPatientId').val(PatientID);
    },
    OpenPaymentPosting: function () {
        AppPrivileges.GetFormPrivileges("Payment Posting", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FromAdmin"] = 0;
                params["ParentCtrl"] = 'paymentBatchDetail';
                params["BatchNumber"] = ChargeBatch_Viewer.params.BatchNumber;
                params["CheckNumber"] = $('#' + ChargeBatch_Viewer.params.PanelID + ' #actionPanPaymentBatchDetail #txtPaymentCheckNumber').val();;
                params["CheckDate"] = $('#' + ChargeBatch_Viewer.params.PanelID + ' #actionPanPaymentBatchDetail #dpCheckDate').val();;
                params["BatchId"] = ChargeBatch_Viewer.params.BatchId;
                params["OpenFrom"] = "ChargeBatch_Viewer";
                params['flag'] = true;
                params['mode'] = 'edit';
                LoadActionPan('Bill_PaymentPosting', params);

                $('#' + ChargeBatch_Viewer.params.PanelID + ' #actionPanPaymentBatchDetail').draggable({
                    handle: "#pnlChargeBatchViewer .modal-content"
                });
                $('#' + ChargeBatch_Viewer.params.PanelID + ' #alternateActionPanPaymentBatchDetail').draggable({
                    handle: "#pnlBillPaymentPosting .modal-content"
                });
                //$('#' + ChargeBatch_Viewer.params.PanelID + ' #alternateActionPanPaymentBatchDetail').css('overflow-y', 'hidden');
                $('#' + ChargeBatch_Viewer.params.PanelID + ' #alternateActionPanPaymentBatchDetail #modalbody').css('height', '550px');
                $('#' + ChargeBatch_Viewer.params.PanelID + ' #alternateActionPanPaymentBatchDetail #modalbody').removeClass('panel-body tab-content-featured tabs-custome-panel-body mt-xs');
                //$('#' + ChargeBatch_Viewer.params.PanelID + ' #alternateActionPanPaymentBatchDetail #modalbody').css('overflow', 'auto');
                //$('#' + ChargeBatch_Viewer.params.PanelID + ' #actionPanPaymentBatchDetail').css('overflow-y', 'hidden');
                //$('#' + ChargeBatch_Viewer.params.PanelID + ' #actionPanPaymentBatchDetail .panel-body').css('height', '600px');
                $('#' + ChargeBatch_Viewer.params.PanelID + ' #actionPanPaymentBatchDetail .panel-body').css('overflow', 'auto');

                $('#' + ChargeBatch_Viewer.params.PanelID + ' #actionPanPaymentBatchDetail').css('z-index', 100);
                $('#' + ChargeBatch_Viewer.params.PanelID + ' #alternateActionPanPaymentBatchDetail').css('z-index', 200);

                $('#' + ChargeBatch_Viewer.params.PanelID + ' #actionPanPaymentBatchDetail .modal-dialog .modal-content').click(function (ev) {
                    if (!ev.target.id && ev.target.id != 'lnkPaymentPosting') {
                        document.getElementById(ChargeBatch_Viewer.params.PanelID).querySelector('#actionPanPaymentBatchDetail').querySelector('.modal-dialog-full').setAttribute("style", "max-width:96%;margin:0 !important");
                        $('#' + ChargeBatch_Viewer.params.PanelID + ' #alternateActionPanPaymentBatchDetail').css('z-index', 100);
                        $('#' + ChargeBatch_Viewer.params.PanelID + ' #actionPanPaymentBatchDetail').css('z-index', 200);

                    }
                });
                $('#' + ChargeBatch_Viewer.params.PanelID + ' #alternateActionPanPaymentBatchDetail .modal-dialog .modal-content').click(function (ev) {
                    $('#' + ChargeBatch_Viewer.params.PanelID + ' #alternateActionPanPaymentBatchDetail #pnlBillPaymentPosting').removeClass('row');
                    $('#' + ChargeBatch_Viewer.params.PanelID + ' #alternateActionPanPaymentBatchDetail #modalDialog').get(0).setAttribute("style", "max-width:96%;margin:0 !important");
                    $('#' + ChargeBatch_Viewer.params.PanelID + ' #alternateActionPanPaymentBatchDetail').css('z-index', 200);
                    $('#' + ChargeBatch_Viewer.params.PanelID + ' #actionPanPaymentBatchDetail').css('z-index', 100);

                });
                ChargeBatch_Viewer.PaymentBatchActiveBtns(false);
                return false;

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    //__________END ____________  //

    //__________Update Call ____________  //
    BatchChargeDocumentUpdate: function (isExit) {
        var self = $("#" + ChargeBatch_Viewer.params.PanelID + " #pnlChargeBatchViewer #formDocument");
        var myJSON = self.getMyJSON();
        var batchDocId = ChargeBatch_Viewer.params.BatchDocId;
        ChargeBatch_Viewer.UpdateBatchCharge(myJSON, batchDocId).done(function (response) {
            if (response.status == true) {
                if (ChargeBatch_Viewer.params["ParentCtrl"] == "EncounterChargeCapture") {
                    EncounterChargeCapture.ClaimDocumentsSearch();
                }
                else if (ChargeBatch_Viewer.params["ParentCtrl"] == "chargeBatchDetail") {
                    chargeBatchDetail.BatchChargeDocumentSearch();
                }
                utility.DisplayMessages(response.message, 1);
                $('#' + ChargeBatch_Viewer.params.PanelID + ' #frmChargeBatchViewer').data('serialize', $('#' + ChargeBatch_Viewer.params.PanelID + ' #frmChargeBatchViewer').serialize());
                if (isExit) {
                    ChargeBatch_Viewer.UnLoad();
                }
            }
            else {
                utility.DisplayMessages(response.message, 3);
            }
        });
    },
    UpdateBatchCharge: function (frmData, batchDocId) {
        var data = "batchChargeDocumentData=" + frmData + "&BatchDocId=" + batchDocId;
        return MDVisionService.defaultService(data, "BILLING_BATCHCHARGE_DETAIL", "UPDATE_BATCH_CHARGE_DOCUMENT");

    },


    PaymentBatchDocumentUpdate: function () {
        AppPrivileges.GetFormPrivileges("Payment Batch", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var self = $("#" + ChargeBatch_Viewer.params.PanelID + " #pnlChargeBatchViewer #formDocument");
                var myJSON = self.getMyJSON();
                var batchDocId = ChargeBatch_Viewer.params.BatchDocId;
                ChargeBatch_Viewer.UpdatePaymentBatch(myJSON, batchDocId).done(function (response) {
                    if (response.status == true) {
                        paymentBatchDetail.PaymentBatchDocumentSearch();
                        utility.DisplayMessages(response.message, 1);
                        UnloadActionPan(ChargeBatch_Viewer.params.ParentCtrl, "ChargeBatch_Viewer");
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
    UpdatePaymentBatch: function (frmData, batchDocId) {
        var data = "batchChargeDocumentData=" + frmData + "&BatchDocId=" + batchDocId;
        return MDVisionService.defaultService(data, "BILLING_PAYMENT_BATCH_DETAIL", "UPDATE_PAYMENT_BATCH_DOCUMENT");

    },


    //__________END ____________  //

    //_________________Document View __________//
    DocumentFill: function () {
        var totalDocument = $('#' + ChargeBatch_Viewer.params.PanelID + ' #dgvChargeBatchDocument tr').length - 1;
        if (totalDocument == 1) {
            //$('#' + ChargeBatch_Viewer.params.PanelID + ' #lnkNextDocument,#lnkPreviousDocument').hide();
            $('#' + ChargeBatch_Viewer.params.PanelID + ' #lnkPreviousDocument').addClass('disabled');
            $('#' + ChargeBatch_Viewer.params.PanelID + ' #lnkNextDocument').addClass('disabled');
        }
        var batchDocID = ChargeBatch_Viewer.params.BatchDocId;
        var documentCall = null;
        if (ChargeBatch_Viewer.params.ParentCtrl == "chargeBatchDetail" || ChargeBatch_Viewer.params.ParentCtrl == "EncounterChargeCapture") {
            documentCall = ChargeBatch_Viewer.FillBatchChargeDocument(batchDocID);
        }
        else if (ChargeBatch_Viewer.params.ParentCtrl == "paymentBatchDetail") {
            documentCall = ChargeBatch_Viewer.FillPaymentBatchDocument(batchDocID);
        }

        $.when(documentCall).done(function (response) {
            if (response.status != false) {
                var document_details = JSON.parse(response.DocumentLoad_JSON);
                var self = $('#' + ChargeBatch_Viewer.params.PanelID);
                utility.bindMyJSON(true, document_details, false, self).done(function () {
                    // Begin Added by Azeem Raza Tayyab on 04-Feb-2016 to fix Bug #PMS-3703
                    if ($('#' + ChargeBatch_Viewer.params.PanelID + ' #txtPatientName').val() != "") {
                        $('#' + ChargeBatch_Viewer.params.PanelID + ' #txtPatientName').removeAttr('onblur');
                        $('#' + ChargeBatch_Viewer.params.PanelID + ' #txtPatientName').attr('disabled', true);
                        $('#' + ChargeBatch_Viewer.params.PanelID + ' #lnkPatientAccount').attr('disabled', true);
                        // End Added by Azeem Raza Tayyab on 04-Feb-2016 to fix Bug #PMS-3703
                    }
                    else
                        ChargeBatch_Viewer.BindPatientAccount(true);
                });
                //Start PRD-635 TahreeMalik     If no patient selected in patient field, then 'Create New Claim' hyperlink will be disabled otherwise enabled
                if ($('#' + ChargeBatch_Viewer.params.PanelID + ' #hfPatientId').val() && $('#' + ChargeBatch_Viewer.params.PanelID + ' #hfPatientId').val() != '-1')
                    $('#' + ChargeBatch_Viewer.params.PanelID + ' #lnkChargeCapture').removeClass('disableAll');
                else
                    $('#' + ChargeBatch_Viewer.params.PanelID + ' #lnkChargeCapture').addClass('disableAll');
                //End PRD-635 TahreeMalik
                var FileType = document_details.FileType;
                var LoadPrevious = document_details.txtLoadPrevious;

                if (FileType.indexOf("pdf") > -1) {
                    $('#' + ChargeBatch_Viewer.params.PanelID + ' #canvasContainer').hide();
                    $('#' + ChargeBatch_Viewer.params.PanelID + ' #imagesControls').hide();
                    $('#' + ChargeBatch_Viewer.params.PanelID + ' #OpenDocumentIF').show();
                    $('#' + ChargeBatch_Viewer.params.PanelID + ' #extraContorls').hide();

                    //var byteCharacters = atob(document_details.Base64FileStream);
                    //var byteNumbers = new Array(byteCharacters.length);
                    //for (var i = 0; i < byteCharacters.length; i++) {
                    //    byteNumbers[i] = byteCharacters.charCodeAt(i);
                    //}
                    //var byteArray = new Uint8Array(byteNumbers);
                    //var blob = new Blob([byteArray], { type: "application/pdf" });
                    //var blobUrl = URL.createObjectURL(blob);
                    //$('#' + ChargeBatch_Viewer.params.PanelID + ' #OpenDocumentIF').attr('data', blobUrl);
                    if (LoadPrevious == "0") {
                        utility.LoadFileData(document_details.Base64FileStream, ChargeBatch_Viewer.params.PanelID + ' #OpenDocumentIF');
                    } else {
                        utility.PDFViewer(document_details.Base64FileStream, false, ChargeBatch_Viewer.params.PanelID + ' #OpenDocumentIF');
                    }

                }
                else if (FileType.indexOf("image") > -1) {
                    try {
                        $('#' + ChargeBatch_Viewer.params.PanelID + ' #OpenDocumentIF').hide();
                        $('#' + ChargeBatch_Viewer.params.PanelID + ' #canvasContainer').show();
                        $('#' + ChargeBatch_Viewer.params.PanelID + ' #imagesControls').show();
                        $('#' + ChargeBatch_Viewer.params.PanelID + ' #extraContorls').show();
                        var imageObj = new Image();
                        imageObj.src = "data:" + document_details.FileType + ";base64," + document_details.Base64FileStream;
                        var canvas = document.getElementById("canvas");
                        setTimeout(function () {
                            canvas.width = imageObj.width;
                            canvas.height = imageObj.height;
                        }, 1000)

                        var context = canvas.getContext("2d");
                        function draw() {
                            var scale = 1;
                            var originx = 0;
                            var originy = 0;
                            context.save();
                            context.setTransform(1, 0, 0, 1, 0, 0);
                            context.clearRect(0, 0, canvas.width, canvas.height);
                            context.restore();
                            context.drawImage(imageObj, 0, 0);
                        }
                        setTimeout(function () { draw(); }, 1000);
                        ChargeBatch_Viewer.interval = setInterval(function () { draw(); }, 1000);

                    }
                    catch (ex) {
                        console.log(ex);
                        utility.DisplayMessages(ex, 2);
                    }
                }

            }


            else {
                utility.DisplayMessages(response.Message, 3);
            }
            $('#' + ChargeBatch_Viewer.params.PanelID + ' #frmChargeBatchViewer').data('serialize', $('#' + ChargeBatch_Viewer.params.PanelID + ' #frmChargeBatchViewer').serialize());
        });

        var DocTable = $('#' + ChargeBatch_Viewer.params.PanelID + ' #dgvChargeBatchDocument');
        var index = $.map(DocTable.find('tr'), function (obj, index) {
            if (obj == DocTable.find('tr.active')[0]) {
                return index;
            }
        })[0];
        var CurrPage = $(DocTable).DataTable().page.info().page;
        var pages = $(DocTable).DataTable().page.info().pages;
        if (index == DocTable.find('tr').length - 1) {
            if ((pages - 1) == CurrPage) {
                $('#' + ChargeBatch_Viewer.params.PanelID + ' #lnkNextDocument').addClass('disabled');
                $('#' + ChargeBatch_Viewer.params.PanelID + ' #lnkPreviousDocument').removeClass('disabled');

            }
        }
        if (index == 1) {
            if (CurrPage == 0) {
                $('#' + ChargeBatch_Viewer.params.PanelID + ' #lnkPreviousDocument').addClass('disabled');
                $('#' + ChargeBatch_Viewer.params.PanelID + ' #lnkNextDocument').removeClass('disabled');
            }
        }
        if (index == 1 && (DocTable.find('tr').length - 1) == 1) {
            $('#' + ChargeBatch_Viewer.params.PanelID + ' #lnkPreviousDocument').addClass('disabled');
            $('#' + ChargeBatch_Viewer.params.PanelID + ' #lnkNextDocument').addClass('disabled');
        }


    },
    ValidateViewer: function () {
        $('#frmChargeBatchViewer')
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                    FileName: {
                        group: '.col-md-2',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                }
            }).on('success.form.bv', function (e) {
                e.preventDefault();
                ChargeBatch_Viewer.BatchChargeDocumentUpdate(true);
            });;
    },
    ValidateViewerPayment: function () {
        $('#frmChargeBatchViewer')
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                    FileNamePayment: {
                        group: '.col-md-2',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                }
            }).on('success.form.bv', function (e) {
                e.preventDefault();
                ChargeBatch_Viewer.PaymentBatchDocumentUpdate();
            });;
    },
    FillBatchChargeDocument: function (BatchDocId) {
        var data = "BatchDocId=" + BatchDocId;
        return MDVisionService.defaultService(data, "BILLING_BATCHCHARGE_DETAIL", "FILL_BATCH_CHARGE_DOCUMENT");
    },
    FillPaymentBatchDocument: function (BatchDocId) {
        var data = "BatchDocId=" + BatchDocId;
        return MDVisionService.defaultService(data, "BILLING_PAYMENT_BATCH_DETAIL", "FILL_PAYMENT_BATCH_DOCUMENT");
    },
    ZoomIn: function () {
        ChargeBatch_Viewer.drawCanvas(2);
    },
    ZoomOut: function () {
        ChargeBatch_Viewer.drawCanvas(0.5);
    },
    drawCanvas: function (scale) {
        var canvas = document.getElementById('canvas');
        var context = canvas.getContext('2d');
        if (ChargeBatch_Viewer.needCanvasReset) {
            context.setTransform(1, 0, 0, 1, 0.5, 0.5);
            ChargeBatch_Viewer.needCanvasReset = false;
        }
        context.scale(scale, scale);
        context.beginPath();
        context.restore();
    },
    navigateCanvas: function (navigation) {
        var canvas = document.getElementById("canvas");
        var context = canvas.getContext("2d");
        needCanvasReset = true;
        switch (navigation) {
            case "up":
                context.translate(0, -10);
                break;
            case "down":
                context.translate(0, 10);
                break;
            case "left":
                context.translate(-10, 0);
                break;
            case "right":
                context.translate(10, 0);
                break;
            case "reset":
                context.setTransform(1, 0, 0, 1, 0.5, 0.5);
                break;
        }
    },
    previousDocument: function () {

        $('#' + ChargeBatch_Viewer.params.PanelID + ' #lnkNextDocument').removeClass('disabled');
        var batchDocId = '';
        var table = $('#' + ChargeBatch_Viewer.params.PanelID + ' #dgvChargeBatchDocument');
        var activeRow = $(table).find('tr.active');
        var pages = $(table).DataTable().page.info().pages;
        var CurrPage = $(table).DataTable().page.info().page;
        batchDocId = $(activeRow).prev().attr('batchDocId');
        if (batchDocId != undefined) {
            clearInterval(ChargeBatch_Viewer.interval);
            $(activeRow).prev().addClass('active');
            $(activeRow).removeClass('active');
            ChargeBatch_Viewer.params['BatchDocId'] = batchDocId;
            ChargeBatch_Viewer.DocumentFill();
        }
        else {
            if (CurrPage != 0) {
                $(table).DataTable().page('previous').draw(false);
                var activeRow = $($(table).find('tr')[$(table).DataTable().page.info().length]).addClass('active');
                batchDocId = $(activeRow).prev().attr('batchDocId');
                clearInterval(ChargeBatch_Viewer.interval);
                ChargeBatch_Viewer.params['BatchDocId'] = batchDocId;
                ChargeBatch_Viewer.DocumentFill();
            }
            else
                utility.myConfirm('9', function () { }, function () { }, '9');
        }

    },
    nextDocument: function () {
        $('#' + ChargeBatch_Viewer.params.PanelID + ' #lnkPreviousDocument').removeClass('disabled');
        var batchDocId = '';
        var table = $('#' + ChargeBatch_Viewer.params.PanelID + ' #dgvChargeBatchDocument');
        var activeRow = $(table).find('tr.active');
        var pages = $(table).DataTable().page.info().pages;
        var CurrPage = $(table).DataTable().page.info().page;
        batchDocId = $(activeRow).next().attr('batchDocId');
        if (batchDocId != undefined) {
            clearInterval(ChargeBatch_Viewer.interval);
            $(activeRow).next().addClass('active');
            $(activeRow).removeClass('active');
            ChargeBatch_Viewer.params['BatchDocId'] = batchDocId;
            ChargeBatch_Viewer.DocumentFill();
        }
        else {
            if ((pages - 1) != CurrPage) {
                $(table).DataTable().page('next').draw(false);
                var activeRow = $($(table).find('tr')[1]).addClass('active');
                batchDocId = $(activeRow).next().attr('batchDocId');
                clearInterval(ChargeBatch_Viewer.interval);
                ChargeBatch_Viewer.params['BatchDocId'] = batchDocId;
                ChargeBatch_Viewer.DocumentFill();
            }
            else
                utility.myConfirm('9', function () { }, function () { }, '9');
        }
    },
    PrintDocument: function () {
        var canvasObj = document.getElementById(ChargeBatch_Viewer.params.PanelID).querySelector('#canvas');
        var canvasContext = canvasObj.getContext("2d");
        $('#' + ChargeBatch_Viewer.params.PanelID + ' #printHelper').append('<img src="' + canvasObj.toDataURL() + '"/>');
        $('#' + ChargeBatch_Viewer.params.PanelID + ' #printHelper').printMe();
        $('#' + ChargeBatch_Viewer.params.PanelID + ' #printHelper').empty();
    },
    ActiveBtns: function (isActive) {
        if (isActive) {
            $('#' + ChargeBatch_Viewer.params.PanelID + ' #toolbarbatchCharge #lnkChargeCapture').removeClass('disabled');
            $('#' + ChargeBatch_Viewer.params.PanelID + ' #btnClose').removeClass('disableAll');
            $('#' + ChargeBatch_Viewer.params.PanelID + ' #btnCloseChargeBatchDetail').removeClass('disableAll');

            $('#' + ChargeBatch_Viewer.params.PanelID + '  #lnkExtensionMode').addClass('disabled');
        }
        else {
            $('#' + ChargeBatch_Viewer.params.PanelID + ' #toolbarbatchCharge #lnkChargeCapture').addClass('disabled');
            $('#' + ChargeBatch_Viewer.params.PanelID + ' #btnClose').addClass('disableAll');
            $('#' + ChargeBatch_Viewer.params.PanelID + ' #btnCloseChargeBatchDetail').addClass('disableAll');
            $('#' + ChargeBatch_Viewer.params.PanelID + ' #lnkExtensionMode').removeClass('disabled');
        }
    },
    PaymentBatchActiveBtns: function (isActive) {
        if (isActive) {
            $('#' + ChargeBatch_Viewer.params.PanelID + ' #toolbarbatchCharge #lnkPaymentPosting').removeClass('disabled');
            $('#' + ChargeBatch_Viewer.params.PanelID + ' #btnClose').removeClass('disableAll');
            $('#' + ChargeBatch_Viewer.params.PanelID + ' #lnkExtensionMode').addClass('disabled');
        }
        else {
            $('#' + ChargeBatch_Viewer.params.PanelID + ' #toolbarbatchCharge #lnkPaymentPosting').addClass('disabled');
            $('#' + ChargeBatch_Viewer.params.PanelID + ' #btnClose').addClass('disableAll');
            $('#' + ChargeBatch_Viewer.params.PanelID + ' #lnkExtensionMode').removeClass('disabled');
        }
    },
    //_________________END____________________//
    EnableExtendedMode: function () {
        if (ChargeBatch_Viewer.params.ParentCtrl == "chargeBatchDetail") {
            if (ChargeBatch_Viewer.isExtendedMode) {
                $('#' + ChargeBatch_Viewer.params.PanelID + ' #actionPanChargeBatchDetail,#alternateActionPanChargeBatchDetail').css('width', '48%');
                $('#' + ChargeBatch_Viewer.params.PanelID + ' #alternateActionPanChargeBatchDetail #actionPanEncounterChargeCapture').addClass('nested');
                ChargeBatch_Viewer.isExtendedMode = false;
            }
            else {
                $('#' + ChargeBatch_Viewer.params.PanelID + ' #actionPanChargeBatchDetail,#alternateActionPanChargeBatchDetail').css('width', '95%');
                $('#' + ChargeBatch_Viewer.params.PanelID + ' #alternateActionPanChargeBatchDetail #actionPanEncounterChargeCapture').removeClass('nested');
                ChargeBatch_Viewer.isExtendedMode = true;
            }
        }
        else {
            if (ChargeBatch_Viewer.isExtendedMode) {
                $('#' + ChargeBatch_Viewer.params.PanelID + ' #actionPanPaymentBatchDetail,#alternateActionPanPaymentBatchDetail').css('width', '48%');
                $('#' + ChargeBatch_Viewer.params.PanelID + ' #alternateActionPanPaymentBatchDetail #actionPanBillPaymentPosting').addClass('nested');
                ChargeBatch_Viewer.isExtendedMode = false;
            }
            else {
                $('#' + ChargeBatch_Viewer.params.PanelID + ' #actionPanPaymentBatchDetail,#alternateActionPanPaymentBatchDetail').css('width', '95%');
                $('#' + ChargeBatch_Viewer.params.PanelID + ' #alternateActionPanPaymentBatchDetail #actionPanBillPaymentPosting').removeClass('nested');
                ChargeBatch_Viewer.isExtendedMode = true;
            }
        }
    },

    UnLoad: function () {
        clearInterval(ChargeBatch_Viewer.interval);

        utility.UnLoadDialog('frmChargeBatchViewer', function () {
            if (ChargeBatch_Viewer.params != null && ChargeBatch_Viewer.params.ParentCtrl != null) {
                UnloadActionPan(ChargeBatch_Viewer.params.ParentCtrl, "ChargeBatch_Viewer");
            }
        }, function () {
            UnloadActionPan(ChargeBatch_Viewer.params.ParentCtrl, "ChargeBatch_Viewer");
        });

    },

}