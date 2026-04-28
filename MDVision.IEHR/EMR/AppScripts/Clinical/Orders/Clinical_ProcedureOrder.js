Clinical_ProcedureOrder = {
    //Author: Humaira Yousaf
    //Date: 17-03-2016
    //This file will handle all actions performed for Procedure orders
    bIsFirstLoad: true,
    params: [],
    EditableGrid: null,
    myGrid: null,
    patientId: null,
    detailParams: null,
    orderSearchGridId: 'dgvProcedureOrder',
    //Author: Humaira Yousaf
    //Date: 17-03-2016
    //This function will handle fill of Procedure orders
    Load: function (params) {

        //Start 07-04-2016 Humaira Yousaf for retaining detail values
        Clinical_ProcedureOrder.detailParams = Clinical_ProcedureOrder.params;
        //End 07-04-2016 Humaira Yousaf for retaining detail values

        Clinical_ProcedureOrder.params = params;

        //Start 07-04-2016 Humaira Yousaf for retaining detail values
        Clinical_ProcedureOrder.addProcedureOrderDetailParams();
        //End 07-04-2016 Humaira Yousaf for retaining detail values

        Clinical_ProcedureOrder.patientId = $("div#PatientProfile #hfPatientId").val();
        if (Clinical_ProcedureOrder.params.ParentCtrl == "Clinical_FaceSheet") {
            Clinical_ProcedureOrder.patientId = Clinical_FaceSheet.params.patientID;
            Clinical_ProcedureOrder.params.patientID = Clinical_FaceSheet.params.patientID;
        }
        Clinical_ProcedureOrder.params.mode = "Add";

        if (Clinical_ProcedureOrder.params.PanelID != 'pnlClinicalProcedureOrder') {
            Clinical_ProcedureOrder.params.PanelID = Clinical_ProcedureOrder.params.PanelID + ' #pnlClinicalProcedureOrder';
        } else {
            Clinical_ProcedureOrder.params.PanelID = 'pnlClinicalProcedureOrder';
        }
        var self = $('#' + Clinical_ProcedureOrder.params.PanelID);
        if (Clinical_ProcedureOrder.bIsFirstLoad == true) {
            //Start 21-03-2016 Humaira Yousaf to validate
            Clinical_ProcedureOrder.ValidateProcedureOrder();
            //End 21-03-2016 Humaira Yousaf to validate

            Clinical_ProcedureOrder.fillProvider();
            Clinical_ProcedureOrder.procedureOrderSearch();
        }

        //Start 21-03-2016 Humaira Yousaf to create datepickers and timer
        utility.CreateDatePicker(Clinical_ProcedureOrder.params.PanelID + ' #dpStartDate', function () {
            //on-change callback method
        });

        utility.CreateDatePicker(Clinical_ProcedureOrder.params.PanelID + ' #dpToDate', function () {
            //on-change callback method
        });
        //End 21-03-2016 Humaira Yousaf to create datepickers and timer
        EMRUtility.ValidateFromToDate(Clinical_ProcedureOrder.params.PanelID + ' #frmClinicalProcedureOrder', 'dpStartDate', 'dpToDate', true, function () { }, function () { }, "To Date should be greater than From Date");
        if (Clinical_ProcedureOrder.params.ParentCtrl == "clinicalTabFaceSheet") {
            EMRUtility.MakeFaceSheetPager('#' + Clinical_ProcedureOrder.params.PanelID + " div#FaceSheetPager", Clinical_ProcedureOrder.params.FaceSheetComponents, 'procedureorders');
        } else if (Clinical_ProcedureOrder.params.ParentCtrl == "Clinical_FaceSheet") {
            EMRUtility.MakeFaceSheetPager('#pnlClinicalFaceSheet #pnlClinicalProcedureOrder' + " div#FaceSheetPager", Clinical_ProcedureOrder.params.FaceSheetComponents, 'procedureorders');
        }

        if (Clinical_ProcedureOrder.params.ParentCtrl == 'clinicalTabProgressNote') {

            $("#" + Clinical_ProcedureOrder.params.PanelID + " #btnAddProcedureOrderToNote").show();
        }
        else {
            Clinical_ProcedureOrder.orderSearchGridId = 'dgvProcedureOrder';
            $("#" + Clinical_ProcedureOrder.params.PanelID + " #btnAddProcedureOrderToNote").hide();
        }
        Clinical_ProcedureOrder.SetCollapseExpandPanel();

        utility.callbackAfterAllDOMLoaded(function () {

            var faceSheetpager = $('#FaceSheetPager');
            if (faceSheetpager.length > 0) {
                //show/hide button controls acording to resoltion
                EMRUtility.HideShowFaceSheetPagerBtnControls(faceSheetpager);
                $("#FaceSheetPager").find("div.slick-track").css("width", "1412px");
            }
        });
    },
    SetCollapseExpandPanel: function () {

        //1- Initialization
        $('#' + Clinical_ProcedureOrder.params.PanelID + ' .splitterBtn').html('<a></a>');
        EMRUtility.changeIcon($('#' + Clinical_ProcedureOrder.params.PanelID + ' .splitterBtn a'));

        $('#' + Clinical_ProcedureOrder.params.PanelID + ' .splitterBtn a').click(function (e) {
            $(this).parent('.splitterBtn').prev().slideToggle(250).toggleClass('active');
            var a = $(this);
            EMRUtility.changeIcon(a);
        });

        //2- Default settings
        if (globalAppdata['IsSearchCriteriaExpand'] && globalAppdata['IsSearchCriteriaExpand'].toLowerCase() == 'true') {
            $('#' + Clinical_ProcedureOrder.params.PanelID + ' #splitterBody').attr('class', 'splitterBody active');
            $('#' + Clinical_ProcedureOrder.params.PanelID + ' #splitterBody').show();
        }
        else {
            $('#' + Clinical_ProcedureOrder.params.PanelID + ' #splitterBody').removeClass('splitterBody active');
            $('#' + Clinical_ProcedureOrder.params.PanelID + ' #splitterBody').hide();
        }

    },

    //Function Name: fillProvider
    //Author: Ahmad Raza
    //Date: 08-04-2016
    //Description: This function will fill Provider dropdown
    fillProvider: function () {
        IsBackgroundLoaderShow = false;
        Clinical_ProcedureOrder.fillproviderDBCall().done(function (response) {
            if (response != "") {
                response = JSON.parse(response);

                var $ddlProvider = $('#' + Clinical_ProcedureOrder.params.PanelID + ' #frmClinicalProcedureOrder #txtProvider');
                $ddlProvider.empty();

                $.each(response, function (i, item) {
                    $ddlProvider.append(
                        $('<option/>', {
                            value: item.Value,
                            html: item.Name,
                        })
                    );
                });
            }

        });
        IsBackgroundLoaderShow = true;
    },

    //Function Name: fillProvider
    //Author: Ahmad Raza
    //Date: 08-04-2016
    //Description: This function will call DB to fill Provider dropdown
    fillproviderDBCall: function () {
        var objData = new Object();
        objData["PatientId"] = Clinical_ProcedureOrder.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        objData["commandType"] = "getorderingprovider";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ProcedureOrder", "ProcedureOrder");

    },
    //Function Name: addProcedureOrderDetailParams
    //Author: Humaira Yousaf
    //Date: 07-04-2016
    //Description: Saves procedure order detail values at patient level
    addProcedureOrderDetailParams: function () {

        if (Clinical_ProcedureOrder.detailParams.hasData == true && Clinical_ProcedureOrder.detailParams.CurrentPatientId == (Clinical_ProcedureOrder.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $("div#PatientProfile #hfPatientId").val())) {
            Clinical_ProcedureOrder.params.hasData = true;
            Clinical_ProcedureOrder.params.ProviderName = Clinical_ProcedureOrder.detailParams.ProviderName;
            Clinical_ProcedureOrder.params.ProviderId = Clinical_ProcedureOrder.detailParams.ProviderId;
            Clinical_ProcedureOrder.params.AssigneeName = Clinical_ProcedureOrder.detailParams.AssigneeName;
            Clinical_ProcedureOrder.params.AssigneeId = Clinical_ProcedureOrder.detailParams.AssigneeId;
            Clinical_ProcedureOrder.params.Problems = Clinical_ProcedureOrder.detailParams.Problems;
            Clinical_ProcedureOrder.params.CurrentPatientId = Clinical_ProcedureOrder.detailParams.CurrentPatientId;
        }
        else {
            Clinical_ProcedureOrder.params.hasData = false;
            Clinical_ProcedureOrder.params.ProviderName = "";
            Clinical_ProcedureOrder.params.ProviderId = "";
            Clinical_ProcedureOrder.params.AssigneeName = "";
            Clinical_ProcedureOrder.params.AssigneeId = "";
            Clinical_ProcedureOrder.params.Problems = "";
            Clinical_ProcedureOrder.params.CurrentPatientId = "";
        }
    },

    //Author: Muhammad Arshad
    //Date: 12-04-2016
    //This function will show Current Order History
    showOrderHistory: function (procedureOrderId) {

        var ParentCtrl = 'clinicalTabProcedureOrder';
        var ParentCtrlPanelID = null;
        if (Clinical_ProcedureOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
            ParentCtrl = 'Clinical_ProcedureOrder';
            ParentCtrlPanelID = Clinical_ProcedureOrder.params.PanelID;
        }

        EMRUtility.showCurrentItemHistory(Clinical_ProcedureOrder.params.PanelID, null, null, "ProcedureOrder,ProcedureOrderTest, ProcedureOrderProblem", null, ParentCtrl, procedureOrderId, ParentCtrlPanelID);
    },

    //Method Name: bindAutoComplete
    //Author: Ahmad Raza
    //Date: 21-03-2016
    //Description: This function will handle fill procedure text box on input
    bindAutoComplete: function (element) {


        var hiddenCrtl = $('#' + Clinical_ProcedureOrder.params.PanelID + ' #txtCPTCode');
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "Clinical_ProcedureOrder", null, true);

    },

    //Start//25-03-2016//Abid Ali//For bug# 564
    procedureOrderReset: function () {

        utility.myConfirm('22', function () {
            $('#' + Clinical_ProcedureOrder.params.PanelID + ' #frmClinicalProcedureOrder').resetAllControls(null);
            Clinical_ProcedureOrder.ValidateProcedureOrder();
        }, function () { },
            '22'
        );
    },
    //End//25-03-2016//Abid Ali//For bug# 564

    //Method Name: openCPTCode
    //Author: Ahmad Raza
    //Date: 21-03-2016
    //Description: This function will handle opening of CPT Search Popup
    openCPTCode: function () {
        var params = [];
        //params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Clinical_ProcedureOrder";
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtCPTCode";
        params["ParentCtrlPanelID"] = Clinical_ProcedureOrder.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, Clinical_ProcedureOrder.params.PanelID);
    },
    //Start//24-02-2016//Ahmad Raza//plugin for Toggle switch
    readyFunction: function () {

        (function ($) {
            'use strict';
            $(function () {
                $('[data-plugin-ios-switch]').each(function () {
                    var $this = $(this);

                    $this.themePluginIOS7Switch();
                });
            });
        }).apply(this, [jQuery]);
    },
    //End//24-02-2016//Ahmad Raza//plugin for Toggle switch
    //Author: Muhammad Arshad
    //Date: 24-02-2016
    //This function will handle Add/Edit of CDS based on given CDSId
    procedureOrderAddEdit: function (procedureId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Orders and Results_Procedure", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                //Ast 357 
                if (Clinical_ProcedureOrder.params.ParentCtrl && Clinical_ProcedureOrder.params.ParentCtrl == "clinicalTabProgressNote")
                    params["IsFromNote"] = true;
                params["CurrentNotesProviderId"] = Clinical_ProcedureOrder.params["CurrentNotesProviderId"];
                if (procedureId != null && parseInt(procedureId) > 0) {
                    params["ProcedureOrderId"] = procedureId;
                    params["mode"] = "Edit";
                }
                else {
                    params["ProcedureOrderId"] = -1;
                    params["mode"] = "Add";
                }
                params["FromAdmin"] = Clinical_ProcedureOrder.params["FromAdmin"];
                params["ParentCtrl"] = 'clinicalTabProcedureOrder';
                params["TabID"] = 'ClinicalProcedureOrderDetail';

                if (Clinical_ProcedureOrder.params["ParentCtrl"] == "clinicalTabProgressNote" || Clinical_ProcedureOrder.params["ParentCtrl"] == "clinicalTabFaceSheet" || Clinical_ProcedureOrder.params["ParentCtrl"] == "Clinical_FaceSheet") {
                    params["ParentCtrl"] = 'Clinical_ProcedureOrder';
                    params["ParentCtrlPanelID"] = Clinical_ProcedureOrder.params.PanelID;
                    LoadActionPan('ClinicalProcedureOrderDetail', params, Clinical_ProcedureOrder.params.PanelID);
                }
                else {
                    params["ParentCtrl"] = 'clinicalTabProcedureOrder';
                    LoadActionPan('ClinicalProcedureOrderDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ValidateProcedureOrder: function () {
        $("#" + Clinical_ProcedureOrder.params.PanelID + " #frmClinicalProcedureOrder")
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {

                   // Start 27/11/2015 Muhammad Irfan Bug # 91,92

                   ProblemName: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Provider: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   }

                   // End 27/11/2015 Muhammad Irfan Bug # 91,92

                   //Color: {
                   //    group: '.col-sm-4',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //}
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            Clinical_ProcedureOrder.procedureOrderSave();
        });
    },

    //Author: Muhammad Arshad
    //Date: 22-03-2016
    //This file will handle Delete current Procedure order
    procedureOrderDelete: function (procedureId, PageNo, rpp) {
        var strMessage = "";

        utility.myConfirm('1', function () {
            var selectedValue = procedureId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                var self = $("#" + Clinical_ProcedureOrder.params.PanelID + " form");
                var myJSON = self.getMyJSONByName();

                Clinical_ProcedureOrder.deleteProcedureOrder(myJSON, procedureId, PageNo, rpp).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        Clinical_ProcedureOrder.procedureOrderSearch("", 1, 15);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { },
            '1'
        );
    },

    //Function Name: prodcedureOrderSearch
    //Author Name: Humaira Yousaf
    //Created Date: 17-03-2016
    //Description: Searches Prodcedure Orders data
    //Params: procedureId, PageNo, rpp
    procedureOrderSearch: function (procedureId, PageNo, rpp, caller) {
        var dfd = $.Deferred();
        var strMessage = "";

        if (Clinical_ProcedureOrder.params.ParentCtrl == "clinicalTabProgressNote") {
            if ($("#" + Clinical_ProcedureOrder.params.PanelID + " #" + Clinical_ProcedureOrder.orderSearchGridId + " thead tr #selectRecordOrders").length == 0) {
                $("#" + Clinical_ProcedureOrder.params.PanelID + " #" + Clinical_ProcedureOrder.orderSearchGridId + " thead tr").prepend('<th style="width:50px"><input type="checkbox" onchange="Clinical_ProcedureOrder.checkUncheckAllOrders(this);" controlname="selectRecordOrders" id="selectRecordOrders" name="chkHeader" class="input-block" coltype="checkbox"/></th>');
            }
        } else {
            $("#" + Clinical_ProcedureOrder.params.PanelID + " #" + Clinical_ProcedureOrder.orderSearchGridId + " th#selectRecordOrders").remove();
        }

        if ($("#" + Clinical_ProcedureOrder.params.PanelID + " #pnlProcedureOrder_Result").css("display") == "none") {
            $("#" + Clinical_ProcedureOrder.params.PanelID + " #pnlProcedureOrder_Result").show();
        }

        //uncomment it
        var self = $("#" + Clinical_ProcedureOrder.params.PanelID + " form");
        var myJSON = self.getMyJSONByName();

        // search specific on caller Id
        if (caller != null) {
            if (caller.indexOf("ProcedureOrderDetail") >= 0) {
                myJSON = null;
                // Reload Providers
                Clinical_ProcedureOrder.fillProvider();
            }
        }

        Clinical_ProcedureOrder.searchProcedureOrder(myJSON, procedureId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $.when(Clinical_ProcedureOrder.procedureOrderGridLoad(response)).then(function () {
                    dfd.resolve();
                });

                //Start//02-07-2016//Ahmad Raza//logic for select All
                var totalRows = $("#" + Clinical_ProcedureOrder.params.PanelID + " #" + Clinical_ProcedureOrder.orderSearchGridId + " tr").length;
                totalRows -= 1;
                var selectedRows = $("#" + Clinical_ProcedureOrder.params.PanelID + " #" + Clinical_ProcedureOrder.orderSearchGridId + " tbody tr input:checked").length;
                if (totalRows == selectedRows) {
                    $("#" + Clinical_ProcedureOrder.params.PanelID + " #" + Clinical_ProcedureOrder.orderSearchGridId + " #selectRecordOrders").prop("checked", true);
                }
                else {
                    $("#" + Clinical_ProcedureOrder.params.PanelID + " #" + Clinical_ProcedureOrder.orderSearchGridId + " #selectRecordOrders").prop("checked", false);
                }
                //End//02-07-2016//Ahmad Raza//logic for select All

                var TableControl = Clinical_ProcedureOrder.params.PanelID + " #pnlProcedureOrder_Result #dgvProcedureOrder";
                var PagingPanelControlID = Clinical_ProcedureOrder.params.PanelID + " #dgvProcedureOrder_Paging";
                var ClassControlName = "Clinical_ProcedureOrder";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(
                    CreatePagination(response.procedureOrderCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        Clinical_ProcedureOrder.procedureOrderSearch(PrimaryID, PageNumber, ResultPerPage);
                    }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
                dfd.resolve();
            }
        });

        return dfd;
    },

    //Function Name: checkUncheckAllOrders
    //Author Name: Ahmad Raza
    //Created Date: 02-07-2016
    //Description: this function will handle check/uncheck of all the grid rows
    checkUncheckAllOrders: function (obj) {
        if ($(obj).is(':checked')) {
            $("#" + Clinical_ProcedureOrder.params.PanelID + " input[name='SelectCheckBoxOrder']:checkbox").prop('checked', true);
        } else {
            $("#" + Clinical_ProcedureOrder.params.PanelID + " input[name='SelectCheckBoxOrder']:checkbox").prop('checked', false);
        }
        $("#" + Clinical_ProcedureOrder.params.PanelID + " #" + Clinical_ProcedureOrder.orderSearchGridId + " tbody").find('input[type="checkbox"]').each(function () {
            Clinical_ProcedureOrder.enableAddOrder(this, null);
        });

    },
    //Function Name: enableAddOrder
    //Author Name: Ahmad Raza
    //Created Date: 02-07-2016
    //Description: this function will handle adding orders to JSON array
    enableAddOrder: function (obj, event) {
        if (event != null) {
            event.stopPropagation();
        }

        var totalRows = $("#" + Clinical_ProcedureOrder.params.PanelID + " #" + Clinical_ProcedureOrder.orderSearchGridId + " tr").length;
        totalRows -= 1;
        var selectedRows = $("#" + Clinical_ProcedureOrder.params.PanelID + " #" + Clinical_ProcedureOrder.orderSearchGridId + " tbody tr input:checked").length;
        if (totalRows == selectedRows) {
            $("#" + Clinical_ProcedureOrder.params.PanelID + " #" + Clinical_ProcedureOrder.orderSearchGridId + " #selectRecordOrders").prop("checked", true);
        }
        else {
            $("#" + Clinical_ProcedureOrder.params.PanelID + " #" + Clinical_ProcedureOrder.orderSearchGridId + " #selectRecordOrders").prop("checked", false);
        }

        if ($(obj).is(':checked')) {
            if ($.inArray(obj.id + 'ordr', Clinical_ProgressNote.AttachedNoteComponentIds) == -1) {
                Clinical_ProgressNote.AttachedNoteComponentIds.push(obj.id + 'ordr');
            } if ($.inArray(obj.id + 'ordr', Clinical_ProgressNote.DetachedNoteComponentIds) != -1) {
                var index = Clinical_ProgressNote.DetachedNoteComponentIds.indexOf(obj.id + 'ordr');
                if (index > -1) {
                    Clinical_ProgressNote.DetachedNoteComponentIds.splice(index, 1);
                }
            }
        } else {

            var index = Clinical_ProgressNote.AttachedNoteComponentIds.indexOf(obj.id + 'ordr');
            if (index > -1) {
                Clinical_ProgressNote.AttachedNoteComponentIds.splice(index, 1);
            }
            if ($.inArray(obj.id + 'ordr', Clinical_ProgressNote.DetachedNoteComponentIds) == -1) {
                Clinical_ProgressNote.DetachedNoteComponentIds.push(obj.id + 'ordr');
            }
        }
    },

    //Function Name: addOrdersToNotes
    //Author Name: Ahmad Raza
    //Created Date: 02-07-2016
    //Description: this function will handle adding orders to notes
    addOrdersToNotes: function (selectedAttachedOrders, selectedDetachedOrders) {
        var noteHTMLCtrl = '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML';
        if ($("#" + Clinical_ProcedureOrder.orderSearchGridId).dataTable().fnSettings().aoData.length == 0) {
            //   Clinical_ProcedureOrder.noActiveLabOrderSoapText();
        }
        else {

            if ($("#" + Clinical_ProcedureOrder.params.PanelID + " #selectRecordOrders").prop('checked') == true || $("#" + Clinical_ProcedureOrder.params.PanelID + " #selectRecordOrders").prop('checked') == false) {

                $("#" + Clinical_ProcedureOrder.params.PanelID + " #" + Clinical_ProcedureOrder.orderSearchGridId + " tbody").find('input[type="checkbox"]').each(function () {
                    Clinical_ProcedureOrder.enableAddOrder(this, null);
                });
            }


            var AttachedSelectedOrders = [];
            var DettachedSelectedOrders = [];
            if ((selectedAttachedOrders != '' && selectedAttachedOrders != null) || (selectedDetachedOrders != '' && selectedDetachedOrders != null)) {
                AttachedSelectedOrders = selectedAttachedOrders;
                DettachedSelectedOrders = selectedDetachedOrders;
            } else {
                var AttachSelectedOrdersAndResults = Clinical_ProgressNote.AttachedNoteComponentIds.slice();
                var DettachSelectedOrdersAndResults = Clinical_ProgressNote.DetachedNoteComponentIds.slice();
                AttachedSelectedOrders = EMRUtility.slicefunc(AttachSelectedOrdersAndResults, "ordr", 0, -4);
                DettachedSelectedOrders = EMRUtility.slicefunc(DettachSelectedOrdersAndResults, "ordr", 0, -4);
            }

            var dfd = new $.Deferred();
            var detachedvalues = DettachedSelectedOrders;
            if (detachedvalues.join() != null && detachedvalues.join() != '') {
                Clinical_ProcedureOrder.detachProcedureOrderFromNotes_DBCall(detachedvalues.join()).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var docIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_procedureorder').parent().parent().find('section[id*="Cli_ProcedureOrderDetail_Main"]').map(function () {
                            return $(this).attr('patdocid');
                        }).get().join(',');

                        if (docIds.length > 0) {
                            Clinical_ProgressNote.detachImagesComponentFromNotes_DBCall(docIds).done(function (responseDoc) {
                                responseDoc = JSON.parse(responseDoc);
                                if (responseDoc.status != false) {
                                    Patient_Document.DeleteDocument(docIds).done(function (responseDoc) {
                                        dfd.resolve();
                                    });
                                    Clinical_ProgressNote.updateAttachDocumentButtonImg();
                                }
                                else {
                                    dfd.resolve();
                                    utility.DisplayMessages(responseDoc.Message, 3);
                                }
                            });
                        }
                        else {
                            dfd.resolve();
                        }
                        for (var i = 0; i < detachedvalues.length; i++) {
                            var ALid = detachedvalues[i];
                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_ProcedureOrderDetail_Main' + ALid).remove();
                        }

                        Clinical_ProgressNote.saveComponentSOAPText('ProcedureOrder');
                        setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                        if (Clinical_ProcedureOrder.params != null && Clinical_ProcedureOrder.params.PanelID.indexOf('pnlClinicalProcedureOrder') != -1) {
                            Clinical_ProcedureOrder.procedureOrderSearch();
                        }
                    }
                    else {
                        dfd.resolve();
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else {
                dfd.resolve();
            }
            dfd.then(function () {
                if (AttachedSelectedOrders != null && AttachedSelectedOrders != '') {
                    for (var i = 0; i < Clinical_ProgressNote.AttachedNoteComponentIds.length; i++) {
                        var ALid = Clinical_ProgressNote.AttachedNoteComponentIds[i];
                        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_ProcedureOrderDetail_Main' + ALid).length != 0) {
                            var index = AttachedSelectedOrders.indexOf(ALid);
                            if (index > -1) {
                                AttachedSelectedOrders.splice(index, 1);
                            }
                        }
                    }
                    if (AttachedSelectedOrders.join() != null && AttachedSelectedOrders.join() != '') {
                        Clinical_ProcedureOrder.getProcedureOrderInfo(AttachedSelectedOrders.join());
                    }
                }
            });



        }
        if (Clinical_ProcedureOrder.params && Clinical_ProcedureOrder.params.ParentCtrl && Clinical_ProcedureOrder.params.ParentCtrl == "clinicalTabProgressNote") {
            UnloadActionPan(Clinical_ProcedureOrder.params.ParentCtrl, 'Clinical_ProcedureOrder');
            EMRUtility.scrollToPNcomponent('clinical_procedureorder');
        }
    },


    //Function Name: detachProcedureOrderFromNotes_DBCall
    //Author Name: Ahmad Raza
    //Created Date: 02-07-2016
    //Description: This function will call DB to Detach procedure order from notes
    detachProcedureOrderFromNotes_DBCall: function (procedureOrderID) {
        var objData = new Object();
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["ProcedureOrderIDs"] = procedureOrderID;
        objData["commandType"] = "detach_orders_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "ProcedureOrder", "ProcedureOrder");
    },

    getProcedureOrderInfo: function (procedureOrderId) {
        if (procedureOrderId == null || procedureOrderId == '') {
            return false;
        }

        Clinical_ProcedureOrder.getOrdersForSOAP_DBCall(procedureOrderId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {

                    if (response.ProcedureOrderSoapCount > 0) {

                        Clinical_ProcedureOrder.createProcedureOrderBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', procedureOrderId);
                        if (Clinical_ProcedureOrder.params != null && Clinical_ProcedureOrder.params.PanelID.indexOf('pnlClinicalProcedureOrder') != -1) {
                            Clinical_ProcedureOrder.procedureOrderSearch();
                        }
                    }
                    if (response.ProblemListSoapCount > 0) {
                        Clinical_ProblemLists.createProblemListBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, true);
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
        });
    },

    //Function Name: getOrdersForSOAP_DBCall
    //Author Name: Ahmad Raza
    //Created Date: 02-07-2016
    //Description: this function will call DB for loading soap text of procedure order
    getOrdersForSOAP_DBCall: function (procedureOrderId) {
        var objData = new Object();
        objData["ProcedureOrderIDs"] = procedureOrderId;
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["PatientId"] = Clinical_ProcedureOrder.params.patientID;
        objData["commandType"] = "get_Orders_forsoap";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ProcedureOrder", "ProcedureOrder");
    },

    //Function Name: createProcedureOrderBodyHTML
    //Author Name: Ahmad Raza
    //Created Date: 02-07-2016
    //Description: this function will create procedure order soap text on Progress Note
    createProcedureOrderBodyHTML: function (response, noteHTMLCtrl, procedureOrderId) {
        Clinical_ProcedureOrder.checkProcedureOrderExists();
        if (response.ProcedureOrderSoap_JSON != null && response.ProcedureOrderSoap_JSON != '') {
            var ProcedureOrdersSOAPJSON = JSON.parse(response.ProcedureOrderSoap_JSON);
            if (response.ProcedureOrderReviewSoap_JSON != null) {
            }

            var $mainDivProcedureOrder = $(document.createElement('div'));
            if (ProcedureOrdersSOAPJSON == null || ProcedureOrdersSOAPJSON.length == 0) {
                Clinical_ProgressNote.saveComponentSOAPText('ProcedureOrder');
            } else {
                if ($(noteHTMLCtrl + ' clinical_procedureorder').parent().parent().find('#Cli_ProcedureOrderDetail_Main0').length != 0) {
                    $(noteHTMLCtrl + ' #Cli_ProcedureOrderDetail_Main0').parent().remove();
                }
            }
            var AListId = [];
            $.each(ProcedureOrdersSOAPJSON, function (index, element) {
                var ALid = element.ProcedureOrderId;
                //             var $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(element.NDCID, 'clinicalTabProgressNote', 1);
                var $SectionBodyProcedureOrder = $(document.createElement('section'));
                $SectionBodyProcedureOrder.attr('id', "Cli_ProcedureOrderDetail_Main" + ALid);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_ProcedureOrderDetail_" + ALid);
                var $ListProcedureOrder = $(document.createElement('ul'));
                var duration = "";
                $ListProcedureOrder.attr('class', 'list-unstyled')

                $SectionBodyProcedureOrder.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_ProcedureOrderDetail_" + ALid + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_ProcedureOrderDetail_Main" + ALid + '"  ><i class="fa fa-times"></i></a></div> ');
                var obj = $('<p>');
                $(obj).html(element.SoapText);
                //$(obj).html($(obj).text())
                $ListProcedureOrder.append("<li>" + $(obj).text() + "</li>");
                if (ProcedureOrdersSOAPJSON.length - 1 == index) {
                }
                $DetailsDiv.append($ListProcedureOrder);
                $SectionBodyProcedureOrder.append($DetailsDiv);
                //if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML clinical_medications').parent().parent().find('#Cli_Medications_Main' + ALid).length == 0) {
                if ($(noteHTMLCtrl + ' clinical_procedureorder').parent().parent().find('#Cli_ProcedureOrderDetail_Main' + ALid).length == 0) {
                    AListId.push(ALid);
                    $mainDivProcedureOrder.append($SectionBodyProcedureOrder);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(noteHTMLCtrl + ' clinical_procedureorder').parent().parent().find('#Cli_ProcedureOrderDetail_Main' + ALid + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(noteHTMLCtrl + ' clinical_procedureorder').parent().parent().find('#Cli_ProcedureOrderDetail_Main' + ALid + ' ul li:Last').get(0).outerHTML;
                    }
                    $(noteHTMLCtrl + ' clinical_procedureorder').parent().parent().find('#Cli_ProcedureOrderDetail_Main' + ALid).html($SectionBodyProcedureOrder.html());
                    $(noteHTMLCtrl + ' clinical_procedureorder').parent().parent().find('#Cli_ProcedureOrderDetail_Main' + ALid + ' ul').append(CommentHTML);;
                }
            });

            if (AListId.join(",") != "") {
                procedureOrderId = AListId.join(",");
            }
            if ($mainDivProcedureOrder.html() != '') {
                Clinical_ProcedureOrder.updateProcedureOrderHtml($mainDivProcedureOrder.html(), procedureOrderId, noteHTMLCtrl);
            } else {
                Clinical_ProcedureOrder.updateProcedureOrderHtml('', procedureOrderId, noteHTMLCtrl);
                Clinical_ProgressNote.saveComponentSOAPText('ProcedureOrder', true);
            }
        }

    },

    //Function Name: checkProcedureOrderExists
    //Author Name: Ahmad Raza
    //Created Date: 02-07-2016
    //Description: This function will check procedure order exists or not
    checkProcedureOrderExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_procedureorder').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #PlanNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="ProcedureOrderComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_procedureorder title="Procedure Order"  id="' + this.id + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'ProcedureOrder\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Procedure Order">Procedure Order</a> ' +
                        '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'ProcedureOrder\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'ProcedureOrder\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_procedureorder> </header></li>');
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
                $(this).find('.closeBtn').removeClass('hidden');
                $(this).find('.btnPNC_Edit').removeClass('hidden');
                $(this).css('background', '#EAF1F8');
            });

            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
                $(this).find('.closeBtn').addClass('hidden');
                $(this).find('.btnPNC_Edit').addClass('hidden');
                $(this).css('background', '#fff');
            });
        }
        else
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_procedureorder').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
    },

    //Function Name: updateProcedureOrderHtml
    //Author Name: Ahmad Raza
    //Created Date: 02-07-2016
    //Description: This function will update the html of procedure order
    updateProcedureOrderHtml: function (procedureOrderHTML, procedureOrderID, noteHTMLCtrl) {
        $(noteHTMLCtrl + ' clinical_procedureorder').parent().parent().addClass('initialVisitBody');
        if (procedureOrderHTML != '') {
            $(noteHTMLCtrl + ' clinical_procedureorder').parent().parent().append(procedureOrderHTML);
        }

        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (procedureOrderHTML != '' && procedureOrderID != null && procedureOrderID != '' && procedureOrderID != '0') {
            Clinical_ProcedureOrder.attachProcedureOrderWithNotes(procedureOrderID);
        }
        else {
            var docIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_procedureorder').parent().parent().find('section[id*="Cli_ProcedureOrderDetail_Main"]').map(function () {
                return $(this).attr('patdocid');
            }).get().join(',');

            if (docIds.length > 0) {
                Clinical_ProgressNote.detachImagesComponentFromNotes_DBCall(docIds).done(function (responseDoc) {
                    responseDoc = JSON.parse(responseDoc);
                    if (responseDoc.status != false) {
                        Patient_Document.DeleteDocument(docIds).done(function (response) {
                            Clinical_ProgressNote.SaveAndAttachOrderReport("Procedure Order", procedureOrderID).done(function () {
                                Clinical_ProgressNote.saveComponentSOAPText("ProcedureOrder", true);
                            });
                        });
                    }
                    else {
                        utility.DisplayMessages(responseDoc.Message, 3);
                    }
                });
            }
        }

    },

    //Function Name: attachProcedureOrderWithNotes
    //Author Name: Ahmad Raza
    //Created Date: 02-07-2016
    //Description: This function will attach procedure order with notes
    attachProcedureOrderWithNotes: function (procedureOrderID) {
        var selectedValue = procedureOrderID;
        if (selectedValue == "" || selectedValue == "undefined") {
        }
        else {
            Clinical_ProcedureOrder.attachProcedureOrderWithNotes_DBCall(selectedValue).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    Clinical_ProgressNote.saveComponentSOAPText('ProcedureOrder').done(function () {
                        Clinical_ProgressNote.SaveAndAttachOrderReport("Procedure Order", procedureOrderID, false);
                    });
                    $('#' + procedureOrderID).remove();

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    //Function Name: attachProcedureOrderWithNotes_DBCall
    //Author Name: Ahmad Raza
    //Created Date: 02-07-2016
    //Description: This function will call DB to attach procedure order with notes
    attachProcedureOrderWithNotes_DBCall: function (procedureOrderID) {
        var objData = new Object();
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["procedureOrderIDs"] = procedureOrderID;
        objData["commandType"] = "attach_orders_with_notes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ProcedureOrder", "ProcedureOrder");
    },


    //Function Name: procedureOrderGridLoad
    //Author Name: Humaira Yousaf
    //Created Date: 17-03-2016
    //Description: Loads Procedure Orders in grid
    //Params: response
    procedureOrderGridLoad: function (response) {
        var dfd = $.Deferred();
        try {
            $("#" + Clinical_ProcedureOrder.params.PanelID + " #pnlProcedureOrder_Result #dgvProcedureOrder").dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it
        } catch (ex) {
            console.log(ex);
        }
        try {
            $("#" + Clinical_ProcedureOrder.params.PanelID + " #pnlProcedureOrder_Result #dgvProcedureOrder tbody").find("tr").remove(); //Removing all the table data from table body
        } catch (ex) {
            console.log(ex);
        }
        if (response.procedureOrderCount > 0) {
            var procedureLoadJSONData = JSON.parse(response.ProcedureLoad_JSON); //Parsing array to JSON
            $.each(procedureLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                //$row.attr("onclick", "Clinical_ProcedureOrder.procedureOrderAddEdit('" + item.ProcedureOrderId + "',event);");
                $row.attr("id", "gvProcedure_row" + item.ProcedureOrderId);
                $row.attr("ProcedureId", item.ProcedureOrderId);
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

                //Start//02-07-2016//Ahmad Raza//logic to check uncheck order if it is attached with Note
                var SelectionCheckBoxColumn = "";
                var Checked = "";
                if (item.IsNoteLinked == "True") {
                    if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.ProcedureOrderId + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                        Checked = " ";
                    } else {
                        Checked = " checked";
                    }
                } else {
                    if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.ProcedureOrderId + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                        Checked = " checked";
                    } else {
                        Checked = "";
                    }
                }

                if (Clinical_ProcedureOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                    SelectionCheckBoxColumn = '<td><input type="checkbox" onclick="Clinical_ProcedureOrder.enableAddOrder(this,event);" id="' + item.ProcedureOrderId + '" name="SelectCheckBoxOrder" ' + Checked + ' class="input-block"/></td>';
                } else {
                    SelectionCheckBoxColumn = "";
                }
                //End//02-07-2016//Ahmad Raza//logic to check uncheck order if it is attached with Note


                //Begin 08-04-2016 Edit by Humaira Yousaf Bug# EMR-626
                if (item.Status == "Signed") {
                    //Begin 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578
                    $row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.ProcedureOrderId + '</td><td><a class="btn btn-xs disableAll" href="#" onclick="Clinical_ProcedureOrder.procedureOrderDelete(\'' + item.ProcedureOrderId + '\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_ProcedureOrder.procedureOrderAddEdit(\'' + item.ProcedureOrderId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_ProcedureOrder.printProcedureOrder(\'' + item.ProcedureOrderId + '\',\'' + item.Status + '\' );" title="View Result"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_ProcedureOrder.showOrderHistory(\'' + item.ProcedureOrderId + '\' );" title="Activity Log"> <i class="fa fa-history blue"></i></a></td><td>'
                    + utility.RemoveTimeFromDate(null, item.OrderDate) + ' ' + item.OrderTime + '</td><td>' + item.Procedures.replace(/\|/g, '<br/>') + '</td><td>' + item.OrderNo + '</td><td>' + item.Status + '</td><td>' + item.ProviderName + '</td><td>' + item.AssigneeName + '</td>');
                    //End 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578
                }
                else {
                    $row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.ProcedureOrderId + '</td><td><a class="btn btn-xs" href="#" onclick="Clinical_ProcedureOrder.procedureOrderDelete(\'' + item.ProcedureOrderId + '\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_ProcedureOrder.procedureOrderAddEdit(\'' + item.ProcedureOrderId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs disableAll" href="#" onclick="Clinical_ProcedureOrder.printProcedureOrder(\'' + item.ProcedureOrderId + '\',\'' + item.Status + '\' );" title="View Result"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_ProcedureOrder.showOrderHistory(\'' + item.ProcedureOrderId + '\' );" title="Activity Log"> <i class="fa fa-history blue"></i></a></td><td>'
                        + utility.RemoveTimeFromDate(null, item.OrderDate) + ' ' + item.OrderTime + '</td><td>' + item.Procedures.replace(/\|/g, '<br/>') + '</td><td>' + item.OrderNo + '</td><td>' + item.Status + '</td><td>' + item.ProviderName + '</td><td>' + item.AssigneeName + '</td>');
                }
                //End 08-04-2016 Edit by Humaira Yousaf Bug# EMR-626
                $("#" + Clinical_ProcedureOrder.params.PanelID + " #pnlProcedureOrder_Result #dgvProcedureOrder tbody").last().append($row);
            });
        }
        else {
            $("#" + Clinical_ProcedureOrder.params.PanelID + ' #pnlProcedureOrder_Result #dgvProcedureOrder').DataTable({
                "language": {
                    "emptyTable": "No Procedure Order Found"
                }, "autoWidth": false, "bLengthChange": false, "aaSorting": [[1, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_ProcedureOrder.params.PanelID + ' #pnlProcedureOrder_Result #dgvProcedureOrder'))
            ;
        else {
            $("#" + Clinical_ProcedureOrder.params.PanelID + " #pnlProcedureOrder_Result #dgvProcedureOrder").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [[1, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        }

        EMRUtility.fixDataTableDuplication("#" + Clinical_ProcedureOrder.params.PanelID + " #pnlProcedureOrder_Result");
        dfd.resolve();
        return dfd;
    },
    //Function Name: searchProcedureOrder
    //Author Name: Humaira Yousaf
    //Created Date: 17-03-2016
    //Description: Searches Procedure Orders
    //Params: procedureData, procedureId, PageNumber, RowsPerPage
    searchProcedureOrder: function (procedureData, procedureId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = {};

        if (procedureData != null)
            objData = JSON.parse(procedureData);
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["ProcedureOrderId"] = procedureId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["ProcedureOrderTitle"] = objData["CPTCode"];
        objData["ProviderId"] = $('#' + Clinical_ProcedureOrder.params.PanelID + ' #frmClinicalProcedureOrder #txtProvider').val();
        objData["PatientId"] = Clinical_ProcedureOrder.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : Clinical_ProcedureOrder.patientId;
        objData["commandType"] = "search_procedureorders";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ProcedureOrder", "ProcedureOrder");

    },

    //Author: Muhammad Arshad
    //Date: 22-03-2016
    //This file will handle Delete current Procedure order

    deleteProcedureOrder: function (procedureData, procedureId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = JSON.parse(procedureData);
        objData["ProcedureOrderId"] = procedureId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PatientId"] = Clinical_ProcedureOrder.patientId;
        objData["commandType"] = "delete_procedureorder";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ProcedureOrder", "ProcedureOrder");

    },
    UnLoadTab: function () {
        var parentPanelId = null;
        var objDeffered = $.Deferred();
        if (Clinical_ProcedureOrder.params["CurrentNotesProviderId"])
            Clinical_ProcedureOrder.params["CurrentNotesProviderId"] = "";
        /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        if (Clinical_ProcedureOrder.params.ParentCtrl == "clinicalTabFaceSheet" || Clinical_ProcedureOrder.params.ParentCtrl == "Clinical_FaceSheet") {
            //Commented by Azeem Raza Tayyab to remove seialize form data as it was using frmClinicalCDS
            //utility.UnLoadDialog(Clinical_ProcedureOrder.params.PanelID + ' #frmClinicalCDS', function () {
            /*if (Clinical_ProcedureOrder.params["FromAdmin"] == "0") {
                if (Clinical_ProcedureOrder.params != null && Clinical_ProcedureOrder.params.ParentCtrl != null) {
                    UnloadActionPan(Clinical_ProcedureOrder.params.ParentCtrl, 'Clinical_ProcedureOrder');
                }
                else
                    UnloadActionPan(null, 'Clinical_ProcedureOrder');
            }
            else {
                $("#mstrDivMedical #clinicalMenu_Medical_CDS").remove();
                RemoveAdminTab();
            }
            objDeffered.resolve();*/
            // }, function () {
            if (Clinical_ProcedureOrder.params["FromAdmin"] == "0") {
                if (Clinical_ProcedureOrder.params != null && Clinical_ProcedureOrder.params.ParentCtrl != null) {
                    if (Clinical_ProcedureOrder.params.ParentCtrl == "Clinical_FaceSheet") {
                        parentPanelId = GetTab(Clinical_FaceSheet.params["ParentCtrl"]).PanelID;
                        Clinical_FaceSheet.params.ChildPanelID = null;
                        UnloadActionPan(Clinical_ProcedureOrder.params.ParentCtrl, 'Clinical_ProcedureOrder', null, parentPanelId);
                    } else {
                        UnloadActionPan(Clinical_ProcedureOrder.params.ParentCtrl, 'Clinical_ProcedureOrder');
                    }
                }
                else
                    UnloadActionPan(null, 'Clinical_ProcedureOrder');
            }
            else {
                $("#mstrDivMedical #clinicalMenu_Medical_CDS").remove();
                RemoveAdminTab();
            }
            objDeffered.resolve();
            // });
            Clinical_FaceSheet.loadFaceSheet();
        }
            /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        else if (Clinical_ProcedureOrder.params.ParentCtrl == "clinicalTabProgressNote") {
            var exist = false;
            $("#" + Clinical_ProcedureOrder.params.PanelID + " #dgvProcedureOrder tbody").find('input[type="checkbox"]').each(function () {
                if (this.checked) {
                    exist = true;
                }
                if (exist) {
                    return false;
                }
            });
            if (exist) {
                utility.myConfirmNote('1', function () {
                    Clinical_ProcedureOrder.addOrdersToNotes();
                    UnloadActionPan(Clinical_ProcedureOrder.params.ParentCtrl, 'Clinical_ProcedureOrder');
                    EMRUtility.scrollToPNcomponent('clinical_procedureorder');
                }, "", function () {
                    UnloadActionPan(Clinical_ProcedureOrder.params.ParentCtrl, 'Clinical_ProcedureOrder');
                    EMRUtility.scrollToPNcomponent('clinical_procedureorder');
                });
            }
            else {
                UnloadActionPan(Clinical_ProcedureOrder.params.ParentCtrl, 'Clinical_ProcedureOrder');
                EMRUtility.scrollToPNcomponent('clinical_procedureorder');
            }
        }
        else {
            if (Clinical_ProcedureOrder.params["FromAdmin"] == "0") {
                if (Clinical_ProcedureOrder.params != null && Clinical_ProcedureOrder.params.ParentCtrl != null) {
                    Clinical_ProcedureOrder.addOrdersToNotes();
                    UnloadActionPan(Clinical_ProcedureOrder.params.ParentCtrl, 'Clinical_ProcedureOrder');
                }
                else {
                    Clinical_ProcedureOrder.addOrdersToNotes();
                    UnloadActionPan(null, 'Clinical_ProcedureOrder');
                }
            }
            else {
                $("#mstrDivMedical #clinicalMenu_Medical_CDS").remove();
                RemoveAdminTab();
            }
            objDeffered.resolve();
        }
        return objDeffered;
    },
    //Function Name: OpenProvider
    //Author Name: Humaira Yousaf
    //Created Date: 21-03-2016
    //Description: Opens Provider
    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmClinicalProcedureOrder";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Clinical_ProcedureOrder";
        LoadActionPan('Admin_Provider', params);
    },
    //Function Name: openproviderdetail
    //Author Name: Humaira Yousaf
    //Created Date: 21-03-2016
    //Description: Opens Provider
    openproviderdetail: function () {
        var params = [];
        params["providerid"] = $('#pnlClinicalProcedureOrder #hfprovider').val();
        params["mode"] = "edit";
        params["fromadmin"] = "0";
        params["refctrl"] = "txtprovider";
        params["parentctrl"] = 'Clinical_ProcedureOrder';
        loadactionpan('providerdetail', params);
    },
    //Function Name: printProcedureOrder
    //Author Name: Humaira Yousaf
    //Created Date: 28-03-2016
    //Description: Previews Procedure Order
    printProcedureOrder: function (procedureOrderId, status) {
        if (status == 'Signed') {
            var params = [];
            params["FromAdmin"] = "0";
            params["UserId"] = globalAppdata['AppUserId'];
            params["PatientId"] = Clinical_ProcedureOrder.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();

            if (Clinical_ProcedureOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                params["ParentCtrl"] = 'Clinical_ProcedureOrder';
                var panelId = 'pnlClinicalProgressNote #pnlClinicalProcedureOrder'
                params["ProcedureOrderId"] = procedureOrderId;
                LoadActionPan('Clinical_ProcedureOrderView', params, panelId);
            }
            else {
                params["ParentCtrl"] = Clinical_ProcedureOrder.params.TabID;//'mstrTabDashBoard';
                params["ProcedureOrderId"] = procedureOrderId;
                LoadActionPan('Clinical_ProcedureOrderView', params);
            }

        }
    },
    //Function Name: CDSEdit
    //Author Name: Humaira Yousaf
    //Created Date: 04-03-2016
    //Description: Opens CDS popup to edit data
    //Params: CDSId, event
    //CDSEdit: function (CDSId, event) {
    //    var strMessage = "";
    //    if (event != null) {
    //        event.stopPropagation();
    //    }
    //    utility.SelectGridRow($("#" + Clinical_ProcedureOrder.params.PanelID + " #pnlCDS_Result #dgvCDS #gvCDS_row" + CDSId));
    //    AppPrivileges.GetFormPrivileges("Orders and Results_Procedure", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
    //        if (strMessage == "") {
    //            var selectedValue = CDSId;
    //            if (Clinical_ProcedureOrder.params.TabID == "clinicalTabFaceSheet") {
    //                if (selectedValue == "" || selectedValue == "undefined") {
    //                }
    //                else {
    //                    params = [];
    //                    params["CDSId"] = selectedValue;
    //                    params["mode"] = "Edit";
    //                    params["FromAdmin"] = 0;
    //                    params["ParentCtrl"] = "Clinical_ProcedureOrder";
    //                    LoadActionPan('Clinical_ProcedureOrderAlertDetail', params);
    //                }
    //            }
    //            else {
    //                if (selectedValue == "" || selectedValue == "undefined") {
    //                }
    //                else {
    //                    params = [];
    //                    params["CDSId"] = selectedValue;
    //                    params["mode"] = "Edit";
    //                    LoadActionPan('ClinicalCDSDetail', params);
    //                }
    //            }
    //        }
    //        else
    //            utility.DisplayMessages(strMessage, 2);
    //    });
    //},
}