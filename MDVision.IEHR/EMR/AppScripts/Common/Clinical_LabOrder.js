Clinical_LabOrder = {
    //Author: Abid Ali
    //Date :  31-03-2016
    //This file will handle all actions performed for Lab Order
    bIsFirstLoad: true,
    params: [],
    EditableGrid: null,
    myGrid: null,
    patientId: null,
    detailParams: null,
    orderSearchGridId: 'dgvLabOrder',
    resultSearchGridId: 'dgvLabResult',
    EditableGridOrder: null,
    EditableGridOrderSent: null,
    labOrderRows: [],
    labOrderRowsSent: [],
    labResultRows: [],
    EditableGridOrderResult: null,
    //Author: Abid Ali
    //Date :  31-03-2016
    //This function will handle fill of Lab Order
    Load: function (params) {

        //Start 07-04-2016 Humaira Yousaf for retaining detail values
        Clinical_LabOrder.detailParams = Clinical_LabOrder.params;
        //End 07-04-2016 Humaira Yousaf for retaining detail values
        ClinicalLabResultDetail.explicitResultOrderId = [];
        Clinical_LabOrder.params = params;
        Clinical_LabOrder.patientId = $("div#PatientProfile #hfPatientId").val();
        Clinical_LabOrder.params.mode = "Add";

        if (Clinical_LabOrder.params.PanelID != 'pnlClinicalLabOrder') {
            Clinical_LabOrder.params.PanelID = Clinical_LabOrder.params.PanelID + ' #pnlClinicalLabOrder';
        } else {
            Clinical_LabOrder.params.PanelID = 'pnlClinicalLabOrder';
        }

        $('#' + Clinical_LabOrder.params.PanelID + ' #pnlLabOrder_Search').resetAllControls(null);

        if (Clinical_LabOrder.params.ParentCtrl == 'clinicalTabProgressNote') {

            $("#" + Clinical_LabOrder.params.PanelID + " #ordersPending").attr('href', '#PatientLabOrderPending_ProgressNote')
            $("#" + Clinical_LabOrder.params.PanelID + " #PatientLabOrderPending").attr('id', 'PatientLabOrderPending_ProgressNote');

            $("#" + Clinical_LabOrder.params.PanelID + " #ordersSent").attr('href', '#PatientLabOrderSent_ProgressNote');
            $("#" + Clinical_LabOrder.params.PanelID + " #PatientLabOrderSent").attr('id', 'PatientLabOrderSent_ProgressNote');

            $("#" + Clinical_LabOrder.params.PanelID + " #results").attr('href', '#PatientLabResult_ProgressNote');
            $("#" + Clinical_LabOrder.params.PanelID + " #PatientLabResult").attr('id', 'PatientLabResult_ProgressNote');

            $('.nav-tabs').tab();
            $("#" + Clinical_LabOrder.params.PanelID + " #btnAddLabOrderToNote").show();
            $("#" + Clinical_LabOrder.params.PanelID + " #btnAddLabResultToNote").show();

        }
        else {

            Clinical_LabOrder.orderSearchGridId = 'dgvLabOrder';
            Clinical_LabOrder.resultSearchGridId = 'dgvLabResult';

            $("#" + Clinical_LabOrder.params.PanelID + " #btnAddLabOrderToNote").hide();
            $("#" + Clinical_LabOrder.params.PanelID + " #btnAddLabResultToNote").hide();
        }

        IsBackgroundLoaderShow = false;
        ShowHideLoaderOnScreen(false);
        xhrPool = [];

        var self = $('#' + Clinical_LabOrder.params.PanelID);
        if (Clinical_LabOrder.bIsFirstLoad == true) {
            Clinical_LabOrder.bIsFirstLoad = false;
            //Load All Users
            CacheManager.BindDropDownsByID('#' + Clinical_LabOrder.params.PanelID + ' #ddlAssigneeTemplate', 'GetUsersFullName', true, 1);
            Clinical_LabOrder.LoadLabs('ddlLaboratory', Clinical_LabOrder.params.PanelID);
            Clinical_LabOrder.fillProvider();

            Clinical_LabOrder.ValidateLab();
            //Clinical_LabOrder.fillProvider('frmClinicalLabOrder');
            //Clinical_LabOrder.fillProvider('frmClinicalLabResult');


        }

        EMRUtility.ValidateFromToDate('frmClinicalLabOrder', 'dpStartDate', 'dpToDate', true, function () { }, function () { }, "To Date should be greater than From Date");
        EMRUtility.ValidateFromToDate('frmClinicalLabResult', 'dpStartDate', 'dpToDate', true, function () { }, function () { }, "To Date should be greater than From Date");
        //Start//17-06-2016//Ahmad Raza// Tabs selection Logic
        var orderHref = "#PatientLabOrderPending";
        var resultHref = "#PatientLabResult";

        if (Clinical_LabOrder.params.ParentCtrl == 'clinicalTabProgressNote') {
            orderHref = "#PatientLabOrderPending_ProgressNote";
            resultHref = "#PatientLabResult_ProgressNote";
        }
        if (Clinical_LabOrder.params.Type) {

            IsBackgroundLoaderShow = true;

            if (Clinical_LabOrder.params.Type == "Order") {
                $('#ulSocialHxTabsItems a[href="' + orderHref + '"]').trigger('click');
            }
            else if (Clinical_LabOrder.params.Type == "Result") {
                $('#ulSocialHxTabsItems a[href="' + resultHref + '"]').trigger('click');
            }
        }
        else {
            IsBackgroundLoaderShow = true;
            ShowHideLoaderOnScreen(true);
            Clinical_LabOrder.LabOrderSearch(null, null, null, null, "Pending").done(function () {
                ShowHideLoaderOnScreen(false);
            });
            IsBackgroundLoaderShow = false;
            Clinical_LabOrder.LabOrderSearch(null, null, null, null, "Transmitted");
            for (i = 0; i < $('#ulSocialHxTabsItems li').length; i++) {
                if ($($('#ulSocialHxTabsItems li')[i]).attr('class') == "active" && $($('#ulSocialHxTabsItems li')[i]).text().toLowerCase() == "results") {
                    Clinical_LabOrder.LabResultsSearch("", 1, 15);
                }
            }
            //Clinical_LabOrder.LabResultsSearch();
        }

        //End//17-06-2016//Ahmad Raza// Tabs selection Logic

        if (Clinical_LabOrder.params.ParentCtrl == "clinicalTabFaceSheet") {
            EMRUtility.MakeFaceSheetPager('#' + Clinical_LabOrder.params.PanelID + " div#FaceSheetPager", Clinical_LabOrder.params.FaceSheetComponents, 'LabOrders');
        } else if (Clinical_LabOrder.params.ParentCtrl == "Clinical_FaceSheet") {
            EMRUtility.MakeFaceSheetPager('#pnlClinicalFaceSheet #pnlClinicalLabOrder' + " div#FaceSheetPager", Clinical_LabOrder.params.FaceSheetComponents, 'LabOrders');
        }

        // collapse expand panel for search criteria IMP-1231
        Clinical_LabOrder.SetCollapseExpandPanelPending();
        Clinical_LabOrder.SetCollapseExpandPanelResult();

        utility.callbackAfterAllDOMLoaded(function () {

            var faceSheetpager = $('#FaceSheetPager');
            if (faceSheetpager.length > 0) {
                //show/hide button controls acording to resoltion
                EMRUtility.HideShowFaceSheetPagerBtnControls(faceSheetpager);
                $("#FaceSheetPager").find("div.slick-track").css("width", "1412px");
            }
        });
    },

    SetCollapseExpandPanelResult: function () {
        var pnlId = "PatientLabResult";
        if (Clinical_LabOrder.params.ParentCtrl == 'clinicalTabProgressNote') {
            pnlId = "PatientLabResult_ProgressNote";
        }
        //1- Initialization
        $('#' + Clinical_LabOrder.params.PanelID + ' #' + pnlId + ' .splitterBtn').html('<a></a>');
        EMRUtility.changeIcon($('#' + Clinical_LabOrder.params.PanelID + ' #' + pnlId + ' .splitterBtn a'));

        $('#' + Clinical_LabOrder.params.PanelID + ' #' + pnlId + ' .splitterBtn a').click(function (e) {
            $(this).parent('.splitterBtn').prev().slideToggle(250).toggleClass('active');
            var a = $(this);
            EMRUtility.changeIcon(a);
        });

        //2- Default settings
        if (globalAppdata['IsSearchCriteriaExpand'] && globalAppdata['IsSearchCriteriaExpand'].toLowerCase() == 'true') {
            $('#' + Clinical_LabOrder.params.PanelID + ' #' + pnlId + '  #splitterBody').attr('class', 'splitterBody active');
            $('#' + Clinical_LabOrder.params.PanelID + ' #' + pnlId + '  #splitterBody').show();
        }
        else {
            $('#' + Clinical_LabOrder.params.PanelID + ' #' + pnlId + '  #splitterBody').removeClass('splitterBody active');
            $('#' + Clinical_LabOrder.params.PanelID + ' #' + pnlId + '  #splitterBody').hide();
        }

    },

    SetCollapseExpandPanelPending: function () {
        var pnlId = "PatientLabOrderPending";
        if (Clinical_LabOrder.params.ParentCtrl == 'clinicalTabProgressNote') {
            pnlId = "PatientLabOrderPending_ProgressNote";
        }
        //1- Initialization
        $('#' + Clinical_LabOrder.params.PanelID + ' #' + pnlId + ' .splitterBtn').html('<a></a>');
        EMRUtility.changeIcon($('#' + Clinical_LabOrder.params.PanelID + ' #' + pnlId + ' .splitterBtn a'));

        $('#' + Clinical_LabOrder.params.PanelID + ' #' + pnlId + ' .splitterBtn a').click(function (e) {
            $(this).parent('.splitterBtn').prev().slideToggle(250).toggleClass('active');
            var a = $(this);
            EMRUtility.changeIcon(a);
        });

        //2- Default settings
        if (globalAppdata['IsSearchCriteriaExpand'] && globalAppdata['IsSearchCriteriaExpand'].toLowerCase() == 'true') {
            $('#' + Clinical_LabOrder.params.PanelID + ' #' + pnlId + '  #splitterBody').attr('class', 'splitterBody active');
            $('#' + Clinical_LabOrder.params.PanelID + ' #' + pnlId + '  #splitterBody').show();
        }
        else {
            $('#' + Clinical_LabOrder.params.PanelID + ' #' + pnlId + '  #splitterBody').removeClass('splitterBody active');
            $('#' + Clinical_LabOrder.params.PanelID + ' #' + pnlId + '  #splitterBody').hide();
        }

    },

    SetMarkAsReviewed: function (t_his) {
        if ($(t_his).val() != "")
            $(t_his).closest('div.panel-body').find("#chkMarkAsReviewed").prop('checked', true);
        else
            $(t_his).closest('div.panel-body').find("#chkMarkAsReviewed").prop('checked', false);
    },

    printLabOrderDir: function (LabId, status, event, PatientId) {
        Clinical_LabOrderView.previewLabOrder(PatientId, globalAppdata['AppUserId'], LabId).done(function (response) {
            response = JSON.parse(response);
            if (response.status) {

                if (status) {
                    ClinicalLabOrderDetail.generateBarcode();
                    params["BarCodeHtml"] = 'true';
                    params["IsSpecimen"] = true;
                    $('#' + ClinicalLabOrderDetail.params.PanelID + " #barcodeTarget").hide();
                    Clinical_LabOrder.printSpecimen(LabId, 'Transmitted', null, 'Clinical_LabOrder');
                    Clinical_LabOrderView.params["IsSpecimen"] = false;

                } else {
                    params["BarCodeHtml"] = 'true';
                    params["IsSpecimen"] = false;
                }
                utility.documentPrint(response.LabOrderHTML);
            }
            else {
                utility.DisplayMessages(response.Message, 2);
            }
        });

    },

    //Function Name: addProcedureOrderDetailParams
    //Author: Humaira Yousaf
    //Date: 07-04-2016
    //Description: Saves procedure order detail values at patient level
    addLabOrderDetailParams: function () {

        if (Clinical_LabOrder.detailParams.hasData == true && Clinical_LabOrder.detailParams.CurrentPatientId == $("div#PatientProfile #hfPatientId").val()) {
            Clinical_LabOrder.params.hasData = true;
            Clinical_LabOrder.params.ProviderName = Clinical_LabOrder.detailParams.ProviderName;
            Clinical_LabOrder.params.ProviderId = Clinical_LabOrder.detailParams.ProviderId;
            Clinical_LabOrder.params.AssigneeName = Clinical_LabOrder.detailParams.AssigneeName;
            Clinical_LabOrder.params.AssigneeId = Clinical_LabOrder.detailParams.AssigneeId;
            Clinical_LabOrder.params.Problems = Clinical_LabOrder.detailParams.Problems;

            Clinical_LabOrder.params.LabId = detailParams.params.LabId;
            Clinical_LabOrder.params.BillingTypeId = Clinical_LabOrder.detailParams.BillingTypeId;
            Clinical_LabOrder.params.FacilityName = Clinical_LabOrder.detailParams.FacilityName
            Clinical_LabOrder.params.FacilityId = Clinical_LabOrder.detailParams.FacilityId;

            Clinical_LabOrder.params.CurrentPatientId = Clinical_LabOrder.detailParams.CurrentPatientId;
        }
        else {
            Clinical_LabOrder.params.hasData = false;
            Clinical_LabOrder.params.ProviderName = "";
            Clinical_LabOrder.params.ProviderId = "";
            Clinical_LabOrder.params.AssigneeName = "";
            Clinical_LabOrder.params.AssigneeId = "";
            Clinical_LabOrder.params.Problems = "";

            Clinical_LabOrder.params.LabId = "";
            Clinical_LabOrder.params.BillingTypeId = "";
            Clinical_LabOrder.params.FacilityName = "";
            Clinical_LabOrder.params.FacilityId = "";

            Clinical_LabOrder.params.CurrentPatientId = "";
        }
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: This function will handle fill procedure text box on input
    bindAutoComplete: function (element, refCtrlId, parentDivId) {

        if (Clinical_LabOrder.params.ParentCtrl == 'clinicalTabProgressNote') {

            parentDivId = parentDivId + "_ProgressNote";
        }
        var hiddenCrtl = $('#' + Clinical_LabOrder.params.PanelID + ' #' + (refCtrlId != null ? refCtrlId : 'txtCPTCode'));
        //  utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "Clinical_LabOrder", null, true);
        EMRUtility.BindLOINCCodes(hiddenCrtl, "Clinical_LabOrder", null, parentDivId, -1);
    },

    //Called from LOINIC Control to pass code and description as Json obj
    pushLOINCAsCpt: function (JsonObj, $element) {

        var observation = JsonObj["Observation"];
        var LOINCCOde = JsonObj['LOINICCODE'];
        var LOINCDescription = JsonObj['LOINICDescription'];

        var txtCPT = $element.attr('id');
        //Set cpt and description as hidden field.
        if (txtCPT == "txtCPTCode") {
            $("#" + Clinical_LabOrder.params.PanelID + " #frmClinicalLabOrder #hfCPTCode").val(LOINCCOde);
            $("#" + Clinical_LabOrder.params.PanelID + " #frmClinicalLabOrder #hfCPTDescription").val(LOINCDescription);
        }
        else {
            $("#" + Clinical_LabOrder.params.PanelID + " #frmClinicalLabResult #hfCPTCode").val(LOINCCOde);
            $("#" + Clinical_LabOrder.params.PanelID + " #frmClinicalLabResult #hfCPTDescription").val(LOINCDescription);
        }

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: This function will handle opening of CPT Search Popup
    openCPTCode: function (refHiddenCtrl, refCtrl) {
        var params = [];

        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'clinicalTabLabOrder';

        if (refHiddenCtrl != null && refHiddenCtrl != "") {
            params["RefHiddenCtrl"] = refHiddenCtrl;
        }
        else {
            params["RefHiddenCtrl"] = "hfCPTCode";
        }

        if (refCtrl != null && refCtrl != "") {
            params["RefCtrl"] = Clinical_LabOrder.params.PanelID + " #" + refCtrl;
        }
        else {
            params["RefCtrl"] = Clinical_LabOrder.params.PanelID + " #txtCPTCode";
        }
        params["RefCtrlDescription"] = "";

        params["ParentCtrlPanelID"] = Clinical_LabOrder.params.PanelID;
        //  LoadActionPan('Admin_IMOCPT', params, Clinical_LabOrder.params.PanelID);

        LoadActionPan('Clinical_LOINC', params, Clinical_LabOrder.params.PanelID);
    },

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

    //Author: Abid Ali
    //Date :  31-03-2016
    //This function will handle Add/Edit of LabOrder
    LabOrderAddEdit: function (LabOrderId, ParentCtrl, event, ParentCtrlPanelID) {
        var strMessage = "";
        var permissionState = LabOrderId != null && parseInt(LabOrderId) > 0 ? "EDIT" : "ADD";
        AppPrivileges.GetFormPrivileges("Orders and Results_Lab Order", permissionState, "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                if (Clinical_LabOrder.params.ParentCtrl && Clinical_LabOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                    params["IsFromNote"] = true;
                }
                else {
                    params["IsFromNote"] = false;
                }
                // Ast 357
                params["CurrentNotesProviderId"] = Clinical_LabOrder.params["CurrentNotesProviderId"];
                //------------
                if (Clinical_LabOrder.params.isProblemAdded) {
                    params["isProblemAdded"] = true;
                } else {
                    params["isProblemAdded"] = false;
                }
                //------------

                if (LabOrderId != null && parseInt(LabOrderId) > 0) {
                    params["LabOrderId"] = LabOrderId;
                    params["mode"] = "Edit";
                }
                else {
                    params["LabOrderId"] = -1;
                    params["mode"] = "Add";
                    if (Clinical_LabOrder.params["LastLabName"]) {
                        params["LastLabName"] = Clinical_LabOrder.params["LastLabName"];
                    } else {
                        params["LastLabName"] = "";
                    }
                }
                params["FromAdmin"] = Clinical_LabOrder.params["FromAdmin"] != null ? Clinical_LabOrder.params["FromAdmin"] : "0";
                if (ParentCtrl != null && ParentCtrl != "") {
                    params["ParentCtrl"] = ParentCtrl;
                    params["ParentCtrlPanelID"] = ParentCtrlPanelID;//DashBoard.params.PanelID;
                    params["PanelID"] = ParentCtrlPanelID;
                    LoadActionPan('ClinicalLabOrderDetail', params, ParentCtrlPanelID);
                }
                else if (Clinical_LabOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
                    params["ParentCtrl"] = 'Clinical_LabOrder';
                    params["ParentCtrlPanelID"] = Clinical_LabOrder.params.PanelID;
                    params["FromNoteId"] = Clinical_LabOrder.params.NotesId;
                    LoadActionPan('ClinicalLabOrderDetail', params, Clinical_LabOrder.params.PanelID);
                }
                else if (Clinical_LabOrder.params["ParentCtrl"] == "clinicalTabFaceSheet" || Clinical_LabOrder.params["ParentCtrl"] == "Clinical_FaceSheet") {
                    params["ParentCtrl"] = 'Clinical_LabOrder';
                    params["ParentCtrlPanelID"] = Clinical_LabOrder.params.PanelID;
                    LoadActionPan('ClinicalLabOrderDetail', params, Clinical_LabOrder.params.PanelID);
                }

                else {
                    params["ParentCtrl"] = 'clinicalTabLabOrder';
                    LoadActionPan('ClinicalLabOrderDetail', params);
                }
                //params["TabID"] = 'ClinicalLabOrderDetail';


            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },


    //Author: Muhammad Arshad
    //Date :  15-04-2016
    //This function will handle Add/Edit of LabResult
    LabResultAddEdit: function (LabResultId, LabOrderId, isFromDetail, caller, ctrl) {
        var strMessage = "";

        var permissionState = LabOrderId != null && parseInt(LabOrderId) > 0 ? "EDIT" : "ADD";
        AppPrivileges.GetFormPrivileges("Orders and Results_Lab Result", permissionState, "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                if ($("#" + Clinical_LabOrder.params.PanelID + " #PatientlistLabOrderPending").attr('class') == 'active' || $("#" + Clinical_LabOrder.params.PanelID + " #PatientlistLabOrderSent").attr('class') == 'active') {
                    if ($(ctrl).text() == 'Add Result' || ctrl == 'Add Result')
                        params['FromLabDetail'] = true;
                }

                if (LabOrderId != null && parseInt(LabOrderId) > 0) {
                    params["LabResultId"] = LabResultId;
                    params["mode"] = "Edit";
                }
                else {
                    params["LabResultId"] = -1;
                    params["mode"] = "Add";
                    if (Clinical_LabOrder.params["LastLabResultName"]) {
                        params["LastLabResultName"] = Clinical_LabOrder.params["LastLabResultName"];
                    } else {
                        params["LastLabResultName"] = "";
                    }
                }
                params["LabOrderId"] = LabOrderId;

                params["FromAdmin"] = Clinical_LabOrder.params["FromAdmin"];

                if (isFromDetail) {
                    if (Clinical_LabOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
                        if (caller != null) {

                            params["ParentCtrl"] = "ClinicalLabOrderDetail";
                            params["ParentCtrlPanelID"] = Clinical_LabOrder.params.PanelID;
                            LoadActionPan('ClinicalLabResultDetail', params);
                        }
                        else {
                            params["ParentCtrl"] = 'Clinical_LabOrder';
                            params["ParentCtrlPanelID"] = Clinical_LabOrder.params.PanelID;
                            LoadActionPan('ClinicalLabResultDetail', params, Clinical_LabOrder.params.PanelID);
                        }
                    }
                    else {

                        params["ParentCtrl"] = "ClinicalLabOrderDetail";
                        LoadActionPan('ClinicalLabResultDetail', params);
                    }
                }
                else {
                    if (Clinical_LabOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
                        params["ParentCtrlPanelID"] = Clinical_LabOrder.params.PanelID;
                        params["ParentCtrl"] = 'Clinical_LabOrder';
                        LoadActionPan('ClinicalLabResultDetail', params, Clinical_LabOrder.params.PanelID);
                    }
                    else if (Clinical_LabOrder.params["ParentCtrl"] == "clinicalTabFaceSheet" || Clinical_LabOrder.params["ParentCtrl"] == "Clinical_FaceSheet") {
                        params["ParentCtrlPanelID"] = Clinical_LabOrder.params.PanelID;
                        params["ParentCtrl"] = 'Clinical_LabOrder';
                        LoadActionPan('ClinicalLabResultDetail', params, Clinical_LabOrder.params.PanelID);
                    }
                    else {
                        if (params["ParentCtrl"] == null) {
                            //EMR-2476
                            params['LabResultId'] = LabResultId;
                            //
                            params['RefModuleName'] = "Lab Results";
                            params['TransitionId'] = LabOrderId;
                        }
                        params["ParentCtrl"] = 'clinicalTabLabOrder';
                        LoadActionPan('ClinicalLabResultDetail', params);
                    }
                }

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //This function will handle validation of LabOrder
    ValidateLab: function () {
        $("#" + Clinical_LabOrder.params.PanelID + " #frmClinicalLabOrder")
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
            Clinical_LabOrder.LabSave();
        });
    },

    //Author: Ahmad Raza
    //Date :  21-04-2016
    //Description: checkUncheckAllOrders
    checkUncheckAllOrders: function (obj) {
        var tableId = $(obj.parentNode.parentNode.parentNode.parentNode).attr("id");
        if ($(obj).is(':checked')) {
            $("#" + Clinical_LabOrder.params.PanelID + " #" + tableId + " input[name='SelectCheckBoxOrder']:checkbox").prop('checked', true);
        } else {
            $("#" + Clinical_LabOrder.params.PanelID + " #" + tableId + " input[name='SelectCheckBoxOrder']:checkbox").prop('checked', false);
        }
        $("#" + Clinical_LabOrder.params.PanelID + " #" + tableId + " tbody").find('input[type="checkbox"]').each(function () {
            Clinical_LabOrder.enableAddOrder(this, null);
        });

    },
    checkUncheckAllResults: function (obj) {
        var tableId = $(obj.parentNode.parentNode.parentNode.parentNode).attr("id");
        if ($(obj).is(':checked')) {
            $("#" + Clinical_LabOrder.params.PanelID + " #" + tableId + " input[name='SelectCheckBoxResult']:checkbox").prop('checked', true);
        } else {
            $("#" + Clinical_LabOrder.params.PanelID + " #" + tableId + " input[name='SelectCheckBoxResult']:checkbox").prop('checked', false);
        }
        $("#" + Clinical_LabOrder.params.PanelID + " #" + tableId + " tbody").find('input[type="checkbox"]').each(function () {
            Clinical_LabOrder.enableAddResult(this, null);
        });

    },
    LabOrderSearch: function (LabId, PageNo, rpp, caller, OrderStatus) {

        //:(
        //if (OrderStatus == "Signed") {
        //    OrderStatus = "Transmitted";
        //}
        //:-(

        var dfd = $.Deferred();
        var strMessage = "";

        if (Clinical_LabOrder.params.ParentCtrl == "clinicalTabProgressNote") {
            //if ($("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrdertSent  thead tr #selectRecordOrders").length == 0) {
            //    $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrdertSent thead tr").prepend('<th style="width:50px"><input type="checkbox" onchange="Clinical_LabOrder.checkUncheckAllOrders(this);" controlname="selectRecordOrders" id="selectRecordOrders" name="chkHeader" class="input-block" coltype="checkbox"/></th>');
            //    $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrder thead tr").prepend('<th style="width:50px"><input type="checkbox" onchange="Clinical_LabOrder.checkUncheckAllOrders(this);" controlname="selectRecordOrders" id="selectRecordOrders" name="chkHeader" class="input-block" coltype="checkbox"/></th>');
            //}

            //if ($("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrdertSent thead tr #selectRecordSentOrders").length == 0) {

            //    $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrdertSent thead tr").prepend('<th style="width:50px"><input type="checkbox" onchange="Clinical_LabOrder.checkUncheckAllOrders(this);" controlname="selectRecordSentOrders" id="selectRecordSentOrders" name="chkHeader" class="input-block" coltype="checkbox"/></th>');
            //}

        } else {
            //$("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrdertSent" + " th#selectRecordOrders").remove();
            // $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrdertSent thead tr #selectRecordSentOrders").remove();
        }



        if ($("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_Result").css("display") == "none") {
            $("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_Result").show();
        }

        var self = $("#" + Clinical_LabOrder.params.PanelID + " form");

        var myJSON = self.getMyJSONByName();

        Clinical_LabOrder.searchLabOrder(myJSON, LabId, PageNo, rpp, OrderStatus).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (OrderStatus == "Transmitted") {
                    $.when(Clinical_LabOrder.LabGridLoadSent(response)).then(function () {
                        dfd.resolve();
                    });
                    //Start//21-06-2016//Ahmad Raza//logic for select All
                    var totalRows = $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrdertSent tr").length;
                    totalRows -= 1;
                    var selectedRows = $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrdertSent tbody tr input:checked").length;
                    if (totalRows == selectedRows) {
                        $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrdertSent tr #selectRecordOrders").prop("checked", true);
                    }
                    else {
                        $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrdertSent tr #selectRecordOrders").prop("checked", false);
                    }
                    //End//21-06-2016//Ahmad Raza//logic for select All
                    var TableControl = Clinical_LabOrder.params.PanelID + " #pnlLabOrder_Result #dgvLabOrdertSent";
                    var PagingPanelControlID = Clinical_LabOrder.params.PanelID + " #dgvLabOrderSent_Paging";
                    var ClassControlName = "Clinical_LabOrder";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    rpp = rpp == "" ? null : rpp;
                    Clinical_LabOrder.Transmitted = "Transmitted";

                    setTimeout(
                        CreatePagination(response.LabOrderCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Clinical_LabOrder.LabOrderSearch(0, PageNumber, ResultPerPage, null, Clinical_LabOrder.Transmitted);
                        }), 10);
                }
                else {
                    Clinical_LabOrder.LabGridLoad(response);
                    //Start//21-06-2016//Ahmad Raza//logic for select All
                    //var totalRows = $("#" + Clinical_LabOrder.params.PanelID + " #" + Clinical_LabOrder.orderSearchGridId + " tr").length;
                    //totalRows -= 1;
                    //var selectedRows = $("#" + Clinical_LabOrder.params.PanelID + " #" + Clinical_LabOrder.orderSearchGridId + " tbody tr input:checked").length;
                    //if (totalRows == selectedRows) {
                    //    $("#" + Clinical_LabOrder.params.PanelID + " #" + Clinical_LabOrder.orderSearchGridId + " #selectRecordOrders").prop("checked", true);
                    //}
                    //else {
                    //    $("#" + Clinical_LabOrder.params.PanelID + " #" + Clinical_LabOrder.orderSearchGridId + " #selectRecordOrders").prop("checked", false);
                    //}
                    //End//21-06-2016//Ahmad Raza//logic for select All
                    var TableControl = Clinical_LabOrder.params.PanelID + " #pnlLabOrder_Result #" + Clinical_LabOrder.orderSearchGridId;
                    var PagingPanelControlID = Clinical_LabOrder.params.PanelID + " #dgvLabOrder_Paging";
                    var ClassControlName = "Clinical_LabOrder";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    Clinical_LabOrder.pending = "Pending";
                    setTimeout(
                        CreatePagination(response.LabOrderCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Clinical_LabOrder.LabOrderSearch(null, PageNumber, ResultPerPage, null, Clinical_LabOrder.pending);
                        }), 10);

                    // EMR-3183 Fixation        Author : Bilawal Khan
                    //$.when(Clinical_LabOrder.LabOrderSearch(null, null, null, null, "Transmitted")).then(function () {
                    //    dfd.resolve();
                    //});
                }

                if (Clinical_LabOrder.bIsFirstLoad || caller != null || OrderStatus == undefined) {
                    Clinical_LabOrder.Transmitted = "Transmitted";
                    IsBackgroundLoaderShow = false;
                    setTimeout(function () {
                        Clinical_LabOrder.LabOrderSearch(null, null, null, null, Clinical_LabOrder.Transmitted)
                    }, 500);
                    IsBackgroundLoaderShow = true;
                    Clinical_LabOrder.bIsFirstLoad = false;
                    caller = null;
                }

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
            dfd.resolve();
        });

        return dfd;
    },


    LabResultsSearch: function (LabId, PageNo, rpp, caller) {
        var dfd = $.Deferred();
        var strMessage = "";

        //if (Clinical_LabOrder.params.ParentCtrl == "clinicalTabProgressNote") {
        //    if ($("#" + Clinical_LabOrder.params.PanelID + " #" + Clinical_LabOrder.resultSearchGridId + " thead tr #selectRecordOrders").length == 0) {
        //        $("#" + Clinical_LabOrder.params.PanelID + " #" + Clinical_LabOrder.resultSearchGridId + " thead tr").prepend('<th style="width:50px"><input type="checkbox" onchange="Clinical_LabOrder.checkUncheckAllOrders(this);" controlname="selectRecordOrders" id="selectRecordOrders" name="chkHeader" class="input-block" coltype="checkbox"/></th>');
        //    }
        //} else {
        //    $("#" + Clinical_LabOrder.params.PanelID + " #" + Clinical_LabOrder.resultSearchGridId + " th#selectRecordOrders").remove();
        //}



        if ($("#" + Clinical_LabOrder.params.PanelID + " #pnlLabResult_Result").css("display") == "none") {
            $("#" + Clinical_LabOrder.params.PanelID + " #pnlLabResult_Result").show();
        }

        var self = $("#" + Clinical_LabOrder.params.PanelID + " #frmClinicalLabResult");
        var myJSON = self.getMyJSONByName();
        DashBoard.GetAssignedLabResultsCount();
        Clinical_LabOrder.searchLabResults(myJSON, LabId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $.when(Clinical_LabOrder.LabResultsGridLoad(response)).then(function () {
                    dfd.resolve();
                });
                //Start//21-06-2016//Ahmad Raza//logic for select All
                var totalRows = $("#" + Clinical_LabOrder.params.PanelID + " #" + Clinical_LabOrder.resultSearchGridId + " tr").length;
                totalRows -= 1;
                var selectedRows = $("#" + Clinical_LabOrder.params.PanelID + " #" + Clinical_LabOrder.resultSearchGridId + " tbody tr input:checked").length;

                if (totalRows == selectedRows) {
                    $("#" + Clinical_LabOrder.params.PanelID + " #" + Clinical_LabOrder.resultSearchGridId + " #selectRecordOrders").prop("checked", true);
                }
                else {
                    $("#" + Clinical_LabOrder.params.PanelID + " #" + Clinical_LabOrder.resultSearchGridId + " #selectRecordOrders").prop("checked", false);
                }
                //End//21-06-2016//Ahmad Raza//logic for select All
                var TableControl = Clinical_LabOrder.params.PanelID + " #pnlLabResult_Result #" + Clinical_LabOrder.resultSearchGridId;
                var PagingPanelControlID = Clinical_LabOrder.params.PanelID + " #dgvLabResult_Paging";
                var ClassControlName = "Clinical_LabOrder";
                var PagesToDisplay = 15;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(
                    CreatePagination(response.LabResultCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        Clinical_LabOrder.LabResultsSearch(PrimaryID, PageNumber, ResultPerPage);
                    }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
                dfd.resolve();
            }
        });

        return dfd;
    },


    openLabTrends: function (LabOrderResultId, LabOrderId, PatientInfo, event) {
        var params = [];
        params["LabResultId"] = LabOrderResultId;
        params["LabOrderId"] = LabOrderId;
        params["ParentCtrl"] = (Clinical_LabOrder.params.ParentCtrl != "clinicalTabProgressNote" && Clinical_LabOrder.params.ParentCtrl != "clinicalTabFaceSheet" && Clinical_LabOrder.params.ParentCtrl != "Clinical_FaceSheet") ? Clinical_LabOrder.params.TabID : "Clinical_LabOrder";
        var comeFrom = "";
        var PanelID = "";

        if (event != null) {
            event.stopPropagation();
            if ($(event.target).closest('#widgetgridpanel').length > 0) {
                comeFrom = "pnlDashboard";
            }
        }

        if (comeFrom == "") {
            if (Clinical_LabOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                PanelID = 'pnlClinicalProgressNote #pnlClinicalLabOrder';
                params["PanelID"] = 'pnlClinicalProgressNote #pnlClinicalLabOrder';
                params["PrPanelID"] = 'pnlClinicalProgressNote #pnlClinicalLabOrder';
            }
            else if (Clinical_LabOrder.params.ParentCtrl == "clinicalTabFaceSheet" || Clinical_LabOrder.params.ParentCtrl == "Clinical_FaceSheet") {
                PanelID = 'pnlClinicalFaceSheet #pnlClinicalLabOrder';
                params["PanelID"] = 'pnlClinicalFaceSheet #pnlClinicalLabOrder';
                params["PrPanelID"] = 'pnlClinicalFaceSheet #pnlClinicalLabOrder';
            }
            else {
                PanelID = 'pnlClinicalLabOrder';
                params["PanelID"] = 'pnlClinicalLabOrder';
                params["PrPanelID"] = 'pnlClinicalLabOrder';
            }
        }
        else {
            PanelID = comeFrom;
            params["PanelID"] = comeFrom;
            params["PrPanelID"] = comeFrom;
            params["ParentCtrl"] = 'mstrTabDashBoard';
            params["PatientInfo"] = PatientInfo;
        }
        LoadActionPan("ClinicalLabResultTrends", params, PanelID);
    },
    AddComments: function (ResultId) {
        var comeFrom = "";
        if (event != null) {
            event.stopPropagation();
            if ($(event.target).closest('#widgetgridpanel').length > 0)
                comeFrom = "pnlDashboard";
        }
        var params = [];
        var PanelID = "";
        if (comeFrom == "") {
            if (Clinical_LabOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                params["ParentCtrl"] = 'Clinical_LabOrder';
                PanelID = 'pnlClinicalProgressNote #pnlClinicalLabOrder';
                PageNo = $(Clinical_LabOrder.params.PanelID + " #dgvLabResult_Paging li.active a").text();
            }
            else if (Clinical_LabOrder.params.ParentCtrl == "clinicalTabFaceSheet" || Clinical_LabOrder.params.ParentCtrl == "Clinical_FaceSheet") {
                params["ParentCtrl"] = 'Clinical_LabOrder';
                PanelID = 'pnlClinicalFaceSheet #pnlClinicalLabOrder';
            }
            else {
                params["ParentCtrl"] = 'clinicalTabLabOrder';
                PanelID = 'pnlClinicalLabOrder';
            }
        }
        else {
            params["ParentCtrl"] = 'mstrTabDashBoard';
            PanelID = 'pnlDashboard';
        }
        params["LabResultId"] = ResultId;
        params["FromAdmin"] = "0";
        LoadActionPan('Clinical_LabResultComments', params, PanelID);
    },
    LabResultsGridLoad: function (response) {
        var dfd = $.Deferred();

        //Start//Abid Ali
        Clinical_LabOrder.labResultRows = [];
        //End//Abid Ali
        var maxLabOrderId = 0;
        Clinical_LabOrder.params["LastLabResultName"] = "";
        //Start 24-05-2016 Muhammad Arshad Remove Duplicate search issue on Datatable
        if ($.fn.dataTable.isDataTable("#" + Clinical_LabOrder.params.PanelID + " #pnlLabResult_Result #" + Clinical_LabOrder.resultSearchGridId)) {
            $("#" + Clinical_LabOrder.params.PanelID + " #pnlLabResult_Result #" + Clinical_LabOrder.resultSearchGridId).dataTable().fnClearTable();
            $("#" + Clinical_LabOrder.params.PanelID + " #pnlLabResult_Result #" + Clinical_LabOrder.resultSearchGridId).dataTable().fnDestroy();
        }
        $("#" + Clinical_LabOrder.params.PanelID + " #pnlLabResult_Result #" + Clinical_LabOrder.resultSearchGridId + " tbody").find("tr").remove();

        if ($("#" + Clinical_LabOrder.params.PanelID + " #pnlLabResult_Result #" + Clinical_LabOrder.resultSearchGridId + " thead tr #selectResults").length == 0) {
            $("#" + Clinical_LabOrder.params.PanelID + " #pnlLabResult_Result #" + Clinical_LabOrder.resultSearchGridId + " thead tr").prepend('<th class="size100 size-min110"><input type="checkbox" onchange="Clinical_LabOrder.checkUncheckAllResults(this);" controlname="selectResults" id="selectResults" name="chkHeader" class="input-block" coltype="checkbox"/></th>');
        } else {
            $("#" + Clinical_LabOrder.params.PanelID + " #pnlLabResult_Result #" + Clinical_LabOrder.resultSearchGridId + " thead tr #selectResults").prop('checked', false);
        }
        if (response.LabResultCount > 0) {
            var LabLoadJSONData = JSON.parse(response.LabOrderResultModel_JSON)//JSON.parse(response.LabOrderLoad_JSON); //Parsing array to JSON
            $.each(LabLoadJSONData, function (i, item) {

                var currentDate = new Date();
                var currentTime = currentDate.toLocaleTimeString().replace(/([\d]+:[\d]{2})(:[\d]{2})(.*)/, "$1$3");


                var onclick = "Clinical_LabOrder.labOrderResultRowExpand(this,event);";
                var onCellclick = "Clinical_LabOrder.labOrderResultRowExpand(this,event,'cell');";
                var expandCollapseIcon = '<a href="#" onclick="' + onclick + '" class="tab_space" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a>';

                var CollectionDate = "", CollectionTime = "";
                //Convert Date to prper date time format
                if (item.ModifiedOn != null) {
                    currentDate = new Date(item.ModifiedOn).toLocaleDateString();
                    currentTime = new Date(item.ModifiedOn).toLocaleTimeString().replace(/([\d]+:[\d]{2})(:[\d]{2})(.*)/, "$1$3");
                }

                if (item.CollectionDateTime != null) {
                    CollectionDate = new Date(item.CollectionDateTime).toLocaleDateString();
                    CollectionTime = new Date(item.CollectionDateTime).toLocaleTimeString().replace(/([\d]+:[\d]{2})(:[\d]{2})(.*)/, "$1$3");
                }

                var $row = $('<tr/>');

                $row.attr("id", "gvLab_row" + item.LabResultId);
                $row.attr("LabOrderResultId", item.LabResultId);

                $row.attr("labid", item.LabOrderId);


                //-------- Start

                if (parseInt(item.LabResultId) > parseInt(maxLabOrderId)) {
                    maxLabOrderId = item.LabResultId;
                    Clinical_LabOrder.params["LastLabResultName"] = item.LabName;
                }

                //-------- End

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
                //Start//21-04-2016//Ahmad Raza//logic to check uncheck order if it is attached with Note
                var SelectionCheckBoxColumn = "";
                var Checked = "";
                if (item.IsNoteLinked == "True") {
                    if ($.inArray(item.LabResultId + 'ordr', Clinical_ProgressNote.AttachedNoteComponentIds) == -1) {
                        Clinical_ProgressNote.AttachedNoteComponentIds.push(item.LabResultId + 'rslt');
                    }
                    if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.LabResultId + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                        Checked = " ";
                    } else {
                        Checked = " checked";
                    }
                } else {
                    if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.LabResultId + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                        Checked = " checked";
                    } else {
                        Checked = "";
                    }
                }
                var parentControl = "";
                var parentControlPanelID = null;

                if (Clinical_LabOrder.params.ParentCtrl != "clinicalTabProgressNote") {
                    Checked = "";
                }
                SelectionCheckBoxColumn = '<a class="btn btn-xs" href="#" ><input type="checkbox" onchange="Clinical_LabOrder.enableAddResult(this,event);" id="' + item.LabResultId + '" name="SelectCheckBoxResult" ' + Checked + ' class="input-block"/></a>';
                if (Clinical_LabOrder.params.ParentCtrl == "clinicalTabProgressNote") {

                    parentControl = 'Clinical_LabOrder';
                    parentControlPanelID = Clinical_LabOrder.params.PanelID;

                    //SelectionCheckBoxColumn = '<td><input type="checkbox" onchange="Clinical_LabOrder.enableAddResult(this,event);" id="' + item.LabResultId + '" name="SelectCheckBoxOrder" ' + Checked + ' class="input-block"/></td>';

                }
                else if (Clinical_LabOrder.params.ParentCtrl == "clinicalTabFaceSheet" || Clinical_LabOrder.params.ParentCtrl == "Clinical_FaceSheet") {
                    parentControl = 'Clinical_LabOrder';
                    parentControlPanelID = Clinical_LabOrder.params.PanelID;
                }

                else {
                    parentControl = 'clinicalTabLabOrder';
                    //SelectionCheckBoxColumn = "";
                }


                if (item.LabOrderTests && item.LabOrderTests.length > 0 && item.LabOrderTests[0].CPTDescription == null) {
                    item.LabOrderTests[0].CPTDescription = "";
                }


                //var divLoinc = item.LabOrderTests.length > 0 && item.LabOrderTests[0].LabOrderResultDetails.length > 0 ? item.LabOrderTests[0].LabOrderResultDetails[0].LOINC + " " + item.LabOrderTests[0].LabOrderResultDetails[0].LOINCDescription : item.LabOrderTests[0].CPTDescription;
                var divLoinc = "";
                for (var i = 0; i < item.LabOrderTests.length; i++) {
                    if (item.LabOrderTests.length > 0) {
                        divLoinc += item.LabOrderTests[i].CPTDescription + "<br />";
                    }
                }
                if (divLoinc != "") {
                    divLoinc = divLoinc.trim();
                    //divLoinc = divLoinc.replace(/,\s*$/, "");
                }
                //var divLoinc = item.LabOrderTests.length > 0 ? item.LabOrderTests[0].CPTCode + " " + item.LabOrderTests[0].CPTDescription : "";
                var PrintResultClass = "";
                //if (item.IsAknowledged == true) {
                //    PrintResultClass = "";
                //}

                var ResultNTETextTooltip = "";
                //For Result NTE if HL7 is received
                if (item.NTEText != null && item.NTEText != "") {
                    ResultNTETextTooltip = ' <i class="fa fa-exclamation ellip100" style="color: blue;padding-left:10px;" data-toggle="tooltip" data-placement="right" title="' + item.NTEText + '"></i> ';
                }
                var editMode = 'onclick="Clinical_LabOrder.LabResultAddEdit(\'' + item.LabResultId + '\',\'' + item.LabOrderId + '\', false);"';
                //Start/ / 20 - 04 - 2016//Ahmad Raza//implimented logic to show icon for showing DBAudit History
                var comments = "";
                var decodedStripedHtml = "";
                var decodedStripedHtml = $("<div/>").html(item.Comments).text();
                var commentsMethod = "Clinical_LabOrder.AddComments('" + item.LabResultId + "');";
                comments = `<a href="#" id='comment_${item.LabResultId}' onclick="${commentsMethod}" data-toggle="tooltip" data-placement="right" title='${decodedStripedHtml.trim()}'><i class="fa fa-commenting blue"></i></a>`;

                $row.append('<td style="display:none;">' + item.LabResultId +
                        '</td><td>' + SelectionCheckBoxColumn + '&nbsp;<a class="btn  btn-xs ' + PrintResultClass + '" href="#" onclick="Clinical_LabOrder.printLabResult(\'' + item.LabOrderId + '\',\'' + item.LabResultId + '\',\'' + item.Status + '\',event,\'' + item.PatientId + '\' );" title="View Result"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_LabOrder.showLabResultsHistory(\'' + item.LabResultId + '\', event );" title="Activity Log"> <i class="fa fa-history blue"></i></a>&nbsp;<a title="Print Specimen Label" class="btn  btn-xs hidden" href="#"  onclick="Clinical_LabOrder.printSpecimen(\'' + item.LabOrderId + '\',\'' + item.Status + '\',event );" ><i class="fa fa-barcode"></i> </a><a class="btn btn-xs" href="#" onclick="Clinical_LabOrder.openLabTrends(\'' + item.LabResultId + '\',\'' + item.LabOrderId + '\',\'' + null + '\', event);" title="Trend Record"><i class="fa fa-line-chart green"></i></a>&nbsp;' + comments + '</td><td ' + editMode + '>'
                         + currentDate + " " + currentTime + '</td><td ' + editMode + '>' + CollectionDate + " " + CollectionTime + '</td><td onclick="' + onCellclick + '">' + expandCollapseIcon + divLoinc + '</td><td ' + editMode + '>' + item.LabName + '</td><td ' + editMode + '>' + item.OrderNo + '</td><td ' + editMode + '>' + item.Status + '</td><td ' + editMode + '>' + item.Provider + '</td><td ' + editMode + '>' + item.AssigneeName + '</td><td class="hidden">' + Number(new Date(currentDate + ' ' + currentTime)) + '</td>');
                var hfLabResultComments = $(`<input type="hidden" id='hfComments${item.LabResultId}' name="Comments" value='${decodedStripedHtml.trim()} '>`);
                $row.append(hfLabResultComments);
                //End//20-04-2016//Ahmad Raza//implimented logic to show icon for showing DBAudit History
                $("#" + Clinical_LabOrder.params.PanelID + " #pnlLabResult_Result #" + Clinical_LabOrder.resultSearchGridId + " tbody").last().append($row);

                var childRows = Clinical_LabOrder.buildLabOrderResultRowChild(item.LabOrderTests, item);
                Clinical_LabOrder.labResultRows.push({ row: $row, childs: childRows });

                utility.bindMyJSONByName(true, item, false, childRows).done(function () {
                    childRows.find('#txtComment').html(item.Comments);
                    childRows.find('#orderNo').text("Order Number: " + item.OrderNo);
                    childRows.find('#laboratory').text("Lab: " + item.LabName);

                    //Enable disable checkbox status
                    if (item.Status == "Final") {

                        childRows.find('#chkSentToPortal').prop('disabled', false);

                        if (item.IsSentToPortal) {
                            childRows.find('#chkSentToPortal').prop('checked', true);
                        }
                        else {
                            childRows.find('#chkSentToPortal').prop('checked', false);
                        }
                    }
                    else {
                        childRows.find('#chkSentToPortal').prop('disabled', true);
                    }

                    if (item.MarkAsReviewed) {
                        childRows.find('#chkMarkAsReviewed').prop('checked', true);
                    }
                    else {
                        childRows.find('#chkMarkAsReviewed').prop('checked', false);
                    }

                    var currentPatientAccount = item.AccountNumber;
                    var currentPatientId = item.PatientId;
                    var currentPatientName = item.PatientFullName;
                    var currentModuleName = "Lab Result";
                    var ResultId = item.LabResultId;
                    var LabOrderId = item.LabOrderId;
                    var CreatedBy = item.CreatedBy;
                    var btnScanDocument = childRows.find("#anchorDocumentScan").unbind('click').bind("click", function (e) {
                        EMRUtility.documentScan("clinicalTabLabOrder", currentModuleName, currentPatientId, currentPatientName, ResultId);
                    });

                    var btnUploadDocument = childRows.find("#anchorDocumentUpload").unbind('click').bind("click", function () {
                        EMRUtility.documentImport("clinicalTabLabOrder", currentModuleName, currentPatientId, currentPatientName, ResultId, currentPatientAccount);
                    });

                    //-----------------

                    var btnViewResultDocumentPDF = childRows.find("#btnViewPDF").unbind('click').bind("click", function () {
                        Clinical_LabOrder.viewPdfLabResult(currentPatientId, LabOrderId, ResultId);
                    });

                    //-----------------
                    var btnViewAttachment = childRows.find("#btnViewAttachment").unbind('click').bind("click", function (e) {
                        EMRUtility.loadAttachments("clinicalTabLabOrder", currentModuleName, "load_attachments", currentPatientId, ResultId, "#" + Clinical_LabOrder.params.PanelID + " #menuViewAttachment");

                    });
                    var onClick = "Clinical_LabOrder.SaveLabResult(this,'" + item.OrderNo + "','" + item.PatientId + "','" + item.ProviderId + "',event)";
                    childRows.find('#btnSaveResult').attr('onclick', onClick);
                    var btnViewHL7 = childRows.find("#btnShowHL7");
                    if (CreatedBy != null && CreatedBy.toLowerCase().indexOf("mirth") > -1) {
                        btnViewHL7.removeClass("disableAll");
                        btnViewHL7.unbind('click').bind("click", function (e) {
                            EMRUtility.viewHL7PDF("clinicalTabLabOrder", currentPatientId, LabOrderId, ResultId);

                        });
                    }
                    else {
                        btnViewHL7.addClass("disableAll");
                        btnViewHL7.unbind('click');
                    }
                });

                var options = $('#' + Clinical_LabOrder.params.PanelID + ' #ddlAssigneeTemplate').find('option').clone();
                childRows.find('#ddlAssigneeId').append(options);
                childRows.find('#ddlAssigneeId').val(item.AssigneeId)

                var options = $('#' + Clinical_LabOrder.params.PanelID + ' #ddlAssigneeTemplate').find('option').clone();
                childRows.find('#ddlReviewedById').append(options);
                if (item.ReviewedById == "") {
                    childRows.find('#ddlReviewedById').val(globalAppdata["AppUserId"]);
                } else {
                    childRows.find('#ddlReviewedById').val(item.ReviewedById);
                }


            });
        }
        else {

            $("#" + Clinical_LabOrder.params.PanelID + ' #pnlLabResult_Result #' + Clinical_LabOrder.resultSearchGridId).DataTable({
                "destroy": true,
                "destroy": true,
                "language": {
                    "emptyTable": "No Lab Result Found"
                }, "autoWidth": false, "bLengthChange": false, "order": [[9, "desc"]], "aoColumnDefs": [{
                    "orderable": false, "aTargets": [0]
                }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_LabOrder.params.PanelID + ' #pnlLabResult_Result #' + Clinical_LabOrder.resultSearchGridId))
            ;
        else {
            Clinical_LabOrder.EditableGridOrderResult = $("#" + Clinical_LabOrder.params.PanelID + " #pnlLabResult_Result #" + Clinical_LabOrder.resultSearchGridId).DataTable({
                "destroy": true,
                "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[2, "desc"]], "aoColumnDefs": [{
                    "orderable": false, "aTargets": [0]
                }]
            }); // to remove records per page dropdown
        }


        EMRUtility.fixDataTableDuplication("#" + Clinical_LabOrder.params.PanelID + ' #pnlLabResult_Result');

        $.each(Clinical_LabOrder.labResultRows, function (i, item) {

            if (Clinical_LabOrder.EditableGridOrderResult != null) {

                var row = Clinical_LabOrder.EditableGridOrderResult.row(item.row);
                if (item.childs.length > 0) {
                    row.child(item.childs);
                }
                else {
                    //$(item.row).find('td:first').find('a').hide();
                }
            }
        });
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body', html: true });
        dfd.resolve();
        return dfd;
    },

    //For Lab Result
    buildLabOrderResultRowChild: function (tests, object) {

        //  var tests = tests//childItems.Test.split('|');
        var CurrentRowchilds = $();
        var templateHtml = $("#" + Clinical_LabOrder.params.PanelID + " #LabResultTemplate").clone();

        if (tests != null && tests.length > 0) {

            var $tbody = templateHtml.find('#dgvLabResultTemplate').find('tbody');

            $.each(tests, function (i, item) {

                var $ChilRowTestDetail = $('<tr/>').addClass("childRowTest-bg");
                var ResultNTETextTooltip = "";
                //For Result NTE if HL7 is received
                if (item.NTEText != null && item.NTEText != "") {
                    ResultNTETextTooltip = ' <i class="fa fa-exclamation ellip100" style="color: blue;padding-left:10px;" data-toggle="tooltip" data-placement="right" title="' + item.NTEText + '"></i> ';
                }
                var Extraspace = '';
                if (Clinical_LabOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                    var checked = "";
                    if (item.IsResultTestNoteLinked) {
                        checked = "checked";
                    }
                    else {
                        checked = "";
                    }
                    //if ($('#detailCheckboxHead').length <= 0) {
                    //    $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabResultTemplate thead tr").prepend('<th style="width:30px" id="detailCheckboxHead"></th>');
                    //}
                    SelectionCheckBoxColumn = '<td><input type="checkbox" onclick="Clinical_LabOrder.attachLabOrderTestWithNote(this,' + item.LabOrderTestId + ',event);" id="' + item.LabOrderTestId + '" name="SelectCheckBoxOrder" ' + checked + ' class="input-block"/></td>';

                    span = 1;
                } else {
                    SelectionCheckBoxColumn = "";

                    span = 0;
                }
                $ChilRowTestDetail.append('<td class="bold" colspan="6" >' + item.CPTDescription + '</td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td>  <td class="hidden"></td> <td class="hidden"></td>');
                $tbody.append($ChilRowTestDetail);
                var NTEText = "";
                $.each(item.LabOrderResultDetails, function (i, detailItem) {

                    //---------------------------------------------------------
                    var color = "";

                    if (detailItem.Flag == "Abnormally High" || detailItem.Flag == "High") {
                        color = 'style = "color:red;font-weight:bold"'
                    }
                    if (detailItem.Flag == "Abnormally Low" || detailItem.Flag == "Low") {
                        color = 'style = "color:orange;font-weight:bold"'
                    }
                    if (detailItem.Flag == "Normal") {
                        color = 'style = "color:green;font-weight:bold"'
                    }
                    //----------------------------------------------------------

                    var ResultNTETextTooltip = "";
                    //For Result NTE if HL7 is received
                    //if (detailItem.NTEText != null && detailItem.NTEText != "") {
                    //    ResultNTETextTooltip = '<i class="fa fa-exclamation ellip100" style="color: blue;padding-left:10px;" data-toggle="tooltip" data-placement="right" title="' + detailItem.NTEText + '"></i>';
                    //}
                    if (detailItem.NTEText != "") {
                        NTEText = NTEText + detailItem.NTEText;
                    }
                    $ChilRowDetail = $('<tr/>').addClass("childRowDetail-bg");

                    $ChilRowDetail.append('' + Extraspace + '<td  >' + detailItem.LOINCDescription + ResultNTETextTooltip + '</td> <td ' + color + '>' + detailItem.Result + '</td>  <td>' + detailItem.UoM + '</td><td>' + detailItem.Range + '</td>  <td ' + color + '>' + detailItem.Flag + '</td> <td>' + object.Status + '</td>');

                    $tbody.append($ChilRowDetail);
                    $ChilRowDetail.attr('LabOrderResultExternalPDFId', detailItem.LabOrderResultExternalPDFId);

                });
                if (NTEText != "") {
                    NTEText = NTEText.replace(/\~/g, '<br>');
                    $ChilRowDetail = $('<tr/>').addClass("childRowDetail-bg");
                    $ChilRowDetail.append('<td colspan="6' + span + '">' + NTEText + '</td> <td style="display:none;"></td> <td style="display:none;"></td> <td style="display:none;"></td>  <td style="display:none;"></td> <td style="display:none;"></td>');
                    $tbody.append($ChilRowDetail);
                }
            });
        }

        var $row = $('<tr/>').addClass("childRow-bg");
        var cellpadding = '';

        $row.append('<td class="hidden"></td> <td class="hidden"> <td class="hidden"></td>  <td class="hidden"></td> <td colspan="10">' + templateHtml.html() + '</td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td>');
        return CurrentRowchilds = CurrentRowchilds.add($row);

    },
    attachLabOrderTestWithNote: function ($row, LabOrderTestId, event) {
        if (LabOrderTestId != "") {
            var objData = new Object();

            objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
            objData["LabOrderTestId"] = LabOrderTestId;
            if ($($row).prop('checked')) {
                objData["commandType"] = "attach_labordertest_with_notes";
            }
            else {
                objData["commandType"] = "detach_labordertest_with_notes";
            }

            var data = JSON.stringify(objData);
            MDVisionService.APIService(data, "LabResult", "LabResult");
        }
    },

    labOrderResultRowExpand: function ($row, event, from) {

        event.stopPropagation();
        event.preventDefault();
        var currentRowId = null;
        if (from == "cell") {
            $row = $($row).parent();
        } else {
            $row = $($row).parent().parent();
        }
        currentRowId = $($row).attr('id');

        var $actions,
         values = [];
        var row = Clinical_LabOrder.EditableGridOrderResult.row($row);
        //if ($row.hasClass('adding')) {
        //    $row.removeClass('adding');
        //}
        if (row.child.isShown()) {
            // This row is already open - close it
            $row.find(".fa-minus-square").attr("class", "fa fa-plus-square");
            row.child.hide();
            //tr.removeClass('shown');
        }
        else {
            $row.find(".fa-plus-square").attr("class", "fa fa-minus-square");
            // Open this row
            row.child.show();
        }
        $("#" + Clinical_LabOrder.params.PanelID + " #pnlLabResult_Result #" + Clinical_LabOrder.resultSearchGridId + " tbody tr").each(function (i, item) {
            if (currentRowId != $(item).attr('id')) {
                var allotherrows = Clinical_LabOrder.EditableGridOrderResult.row(item);
                if (allotherrows.child.isShown()) {
                    $(item).find(".fa-minus-square").attr("class", "fa fa-plus-square");
                    allotherrows.child.hide();

                }
            }
        });
    },

    SaveLabResult: function (btnSave, orderNo, patientId, providerId, event) {

        event.preventDefault();

        var ContainerDiv = $(btnSave).closest('div.panel-body');

        var Obj = $(ContainerDiv).getMyJSONByName();
        Obj = JSON.parse(Obj);

        var LabId = $(ContainerDiv).closest('tr').prev().attr('labid');

        Obj["LabOrderId"] = LabId;
        Obj["callFromGrid"] = "True";

        Obj["OrderNo"] = orderNo;
        Obj["PatientId"] = patientId;
        Obj["ProviderId"] = providerId;
        Obj["PracticeId"] = $('#PatientProfile #hfPatientPracticeId').val();
        Obj["PatientFullName"] = $('#PatientProfile #hfPatientFullNameOnly').val();
        Obj["IsSentToPortal"] = Obj["SentToPortal"];
        Obj["PracticeId"] = $("#PatientProfile #hfPatientPracticeId").val();
        Obj["PatientFacilityId"] = $('#PatientProfile #hfPatientFacilityId').val();
        Obj["commandType"] = "SAVE_LABRESULT";
        Obj["MarkAsReviewed"] = Obj["MarkAsReviewed"];
        Obj["Comments"] = $(ContainerDiv).find("#txtComment").html();
        Clinical_LabOrder.LabResultSave(Obj).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                utility.DisplayMessages(response.message, 1);
                var dgvId = "dgvLabResult";
                var pnlId = "pnlLabResult_Result";
                var comments = $(ContainerDiv).find("#txtComment").text().trim();
                var mainPnlId = "#" + Clinical_LabOrder.params.PanelID;
                var LabResultId = $(ContainerDiv).closest('tr').prev().attr('laborderresultid');
                var commentsIcon = $(mainPnlId + ' #' + pnlId + ' #' + dgvId + ' tbody tr[LabOrderResultId="' + LabResultId + '"] td:eq(1) a[id="comment_' + LabResultId + '"]');
                $(commentsIcon).attr('data-original-title', comments);
                $(mainPnlId + ' #' + pnlId + ' #' + dgvId + ' tbody tr[LabOrderResultId="' + LabResultId + '"] #hfComments' + LabResultId).val(comments);
                DashBoard.GetAssignedLabResultsCount();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    LabResultSave: function (LabOrderData) {

        var data = JSON.stringify(LabOrderData);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },

    LoadLabs: function (ddlId, controlPanelID) {
        var dfd = $.Deferred();
        return Clinical_Lab.searchLab(null, 0, 1, 5000, "True").done(function (response) {
            //Populate Distinct Values in typeArray
            response = JSON.parse(response);
            var NameArray = new Array();
            var labIds = new Array();
            var codeSystem = new Array();
            var category = new Array();
            var data = JSON.parse(response.ClinicalLab_JSON);
            $.each(data, function (i, row) {
                if (jQuery.inArray(row.Name, NameArray) === -1) {
                    if (controlPanelID.indexOf("Radiology") > -1 || controlPanelID.indexOf("Treatment") > -1) {
                        if (row.Name.trim().toLowerCase() == "point of care" || row.Name.trim().toLowerCase() == "external facility") {
                            NameArray.push(row.Name);
                            labIds.push(row.LabId);
                            codeSystem.push(row.CodeSystemId); // Code System 1 means CPT , 2 Means Lab Codes
                            category.push(row.CategoryName);
                        }
                    }
                    else {
                        NameArray.push(row.Name);
                        labIds.push(row.LabId);
                        codeSystem.push(row.CodeSystemId); // Code System 1 means CPT , 2 Means Lab Codes
                        category.push(row.CategoryName);
                    }
                }

            });
            var ddType = $('#' + controlPanelID + " #" + ddlId);
            ddType.empty();
            ddType.append($("<option />").val("").text('-Select-'));
            if (NameArray.length > 0) {
                $.each(NameArray, function (index, Name) {
                    if ((controlPanelID.indexOf("Radiology") > -1 || controlPanelID.indexOf("Treatment") > -1) && Name.trim().toLowerCase() == "external facility") {
                        if (controlPanelID != "pnlClinicalRadiologyOrder" && controlPanelID != "pnlClinicalProgressNote #pnlClinicalRadiologyOrder" && controlPanelID != "pnlClinicalProgressNote #pnlClinicalTreatment") {
                            ddType.append($("<option selected/>").val(labIds[index]).text(Name).attr({ CodeSystem: codeSystem[index], Category: category[index] }));
                        }
                        else {
                            ddType.append($("<option />").val(labIds[index]).text(Name).attr({ CodeSystem: codeSystem[index], Category: category[index] }));
                        }
                    }
                    else {
                        ddType.append($("<option />").val(labIds[index]).text(Name).attr({ CodeSystem: codeSystem[index], Category: category[index] }));
                    }


                });
            }
            dfd.resolve();
        });
        return dfd;
    },

    //Function Name: fillProvider
    //Author: Abid Ali
    //Date: 08-04-2016
    //Description: This function will fill Provider dropdown
    fillProvider: function (formId) {

        if (formId != null) {
            Clinical_LabOrder.fillproviderDBCall().done(function (response) {
                if (response != "") {
                    response = JSON.parse(response);

                    var $ddlProvider = $('#' + Clinical_LabOrder.params.PanelID + ' #' + formId + ' #ddlProvider');
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
        } else {
            Clinical_LabOrder.fillproviderDBCall().done(function (response) {
                if (response != "") {
                    response = JSON.parse(response);



                    var $ddlProviderLabOrder = $('#' + Clinical_LabOrder.params.PanelID + ' #frmClinicalLabOrder #ddlProvider');
                    var $ddlProviderLabResult = $('#' + Clinical_LabOrder.params.PanelID + ' #frmClinicalLabResult #ddlProvider');

                    $ddlProviderLabOrder.empty();
                    $ddlProviderLabResult.empty();
                    $.each(response, function (i, item) {

                        $ddlProviderLabOrder.append(
                            $('<option/>', {
                                value: item.Value,
                                html: item.Name,
                            })
                        );

                        $ddlProviderLabResult.append(
                            $('<option/>', {
                                value: item.Value,
                                html: item.Name,
                            })
                        );

                    });
                }

            });
        }
    },

    //Function Name: fillProvider
    //Author: Abid Ali
    //Date: 08-04-2016
    //Description: This function will call DB to fill Provider dropdown
    fillproviderDBCall: function () {
        var objData = new Object();
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["commandType"] = "getorderingprovider";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabOrder", "LabOrder");

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Loads Lab Orders data in grid
    //Params: response
    LabGridLoad: function (response) {

        //Start//Abid Ali
        Clinical_LabOrder.labOrderRows = [];

        var maxLabOrderId = 0;
        Clinical_LabOrder.params["LastLabName"] = "";
        //Start 24-05-2016 Muhammad Arshad Remove Duplicate search issue on Datatable
        if ($.fn.dataTable.isDataTable("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_Result #" + Clinical_LabOrder.orderSearchGridId)) {
            $("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_Result #" + Clinical_LabOrder.orderSearchGridId).dataTable().fnClearTable();
            $("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_Result #" + Clinical_LabOrder.orderSearchGridId).dataTable().fnDestroy();
        }
        $("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_Result #" + Clinical_LabOrder.orderSearchGridId + " tbody").find("tr").remove();
        if ($("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_Result #" + Clinical_LabOrder.orderSearchGridId + " thead tr #selectOrders").length == 0) {
            $("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_Result #" + Clinical_LabOrder.orderSearchGridId + " thead tr").prepend('<th style="width:50px"><input type="checkbox" onchange="Clinical_LabOrder.checkUncheckAllOrders(this);" controlname="selectOrders" id="selectOrders" name="chkHeader" class="input-block" coltype="checkbox"/></th>');
        } else {
            $("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_Result #" + Clinical_LabOrder.orderSearchGridId + " thead tr #selectOrders").prop('checked', false);
        }
        //End 24-05-2016 Muhammad Arshad Remove Duplicate search issue on Datatable
        //$("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_Result #dgvLabOrder").dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it
        //$("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_Result #dgvLabOrder tbody").find("tr").remove(); //Removing all the table data from table body
        if (response.LabOrderCount > 0) {
            var LabLoadJSONData = JSON.parse(response.LabOrderFill_JSON);
            //JSON.parse(response.LabLoad_JSON); //Parsing array to JSON
            $.each(LabLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                // $row.attr("onclick", "ClinicalLabOrderDetail.LabEdit('" + item.LabOrderId + "',event);");
                $row.attr("id", "gvLab_row" + item.LabOrderId);
                $row.attr("LabId", item.LabOrderId);

                //-------- Start

                if (parseInt(item.LabOrderId) > parseInt(maxLabOrderId)) {
                    maxLabOrderId = item.LabOrderId;
                    Clinical_LabOrder.params["LastLabName"] = item.LabName;
                }

                //-------- End

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
                var onclick = "Clinical_LabOrder.labOrderRowExpand(this,event);"
                var onCellClick = "Clinical_LabOrder.labOrderRowExpand(this,event,null,'cell');"
                //Start//21-04-2016//Ahmad Raza//logic to check uncheck order if it is attached with Note
                var expandCollapseIcon = '<a href="#" onclick="' + onclick + '" class="tab_space" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a>';


                var SelectionCheckBoxColumn = "";
                var Checked = "";
                if (item.IsNoteLinked == "True") {
                    if ($.inArray(item.LabOrderId + 'ordr', Clinical_ProgressNote.AttachedNoteComponentIds) == -1) {
                        Clinical_ProgressNote.AttachedNoteComponentIds.push(item.LabOrderId + 'ordr');
                    }
                    if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.LabOrderId + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                        Checked = " ";
                    } else {
                        Checked = " checked";
                    }
                } else {
                    if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.LabOrderId + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                        Checked = " checked";
                    } else {
                        Checked = "";
                    }
                }
                if (Clinical_LabOrder.params.ParentCtrl != "clinicalTabProgressNote") {
                    Checked = "";
                }
                //if (Clinical_LabOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                SelectionCheckBoxColumn = '<a class="btn btn-xs" href="#" ><input type="checkbox" onclick="Clinical_LabOrder.enableAddOrder(this,event);" id="' + item.LabOrderId + '" name="SelectCheckBoxOrder" ' + Checked + ' class="input-block"/></a>';
                //} else {
                //    SelectionCheckBoxColumn = '<input type="checkbox" id=' + item.LabOrderId + '  name="SelectCheckBoxOrder"  class="input-block"/>';
                //}
                var editMode = 'onclick="ClinicalLabOrderDetail.LabEdit(' + item.LabOrderId + ',event);"';
                var divLoinc = "";
                var totalTests = 1;
                if (item.Test.indexOf('|') > -1) {
                    totalTests = item.Test.split('|').length;
                }

                for (var i = 0; i < totalTests; i++) {
                    if (totalTests > 0) {
                        divLoinc += item.Test.split('|')[i] + "<br />";
                    }
                }
                if (divLoinc != "") {
                    divLoinc = divLoinc.trim();
                    //divLoinc = divLoinc.replace(/,\s*$/, "");
                }
                //End//21-04-2016//Ahmad Raza//logic to check uncheck order if it is attached with Note
                //Start//20-04-2016//Ahmad Raza//implimented logic to show icon for showing DBAudit History
                if (item.Status == "Transmitted") {

                    //Begin 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578
                    $row.append('<td style="display:none;">' + item.LabOrderId + '</td><td>' + SelectionCheckBoxColumn + '&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_LabOrder.printLabOrder(\'' + item.LabOrderId + '\',\'' + item.Status + '\',event );" title="View Result"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_LabOrder.showOrderHistory(\'' + item.LabOrderId + '\', event );" title="Activity Log"> <i class="fa fa-history blue"></i></a>&nbsp;<a title="Print Specimen Label" class="btn  btn-xs" href="#"  onclick="Clinical_LabOrder.printSpecimen(\'' + item.LabOrderId + '\',\'' + item.Status + '\',event );" ><i class="fa fa-barcode"></i> </a></td><td ' + editMode + '>'
                    + utility.RemoveTimeFromDate(null, item.OrderDate) + ' ' + item.OrderTime + '</td><td onclick="' + onCellClick + '">' + expandCollapseIcon + divLoinc + '</td><td ' + editMode + '>' + item.LabName + '</td><td ' + editMode + '>' + item.OrderNo + '</td><td ' + editMode + '>' + item.Status + '</td><td ' + editMode + '>' + item.Provider + '</td><td ' + editMode + '>' + item.AssigneeName + '</td>');
                    //End 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578

                }
                else {

                    //Begin 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578
                    $row.append('<td style="display:none;">' + item.LabOrderId + '</td><td>' + SelectionCheckBoxColumn + '&nbsp;<a class="btn  btn-xs disableAll" href="#" onclick="Clinical_LabOrder.printLabOrder(\'' + item.LabOrderId + '\',\'' + item.Status + '\',event );" title="View Result"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_LabOrder.showOrderHistory(\'' + item.LabOrderId + '\', event );" title="Activity Log"> <i class="fa fa-history blue"></i></a>&nbsp;<a title="Print Specimen Label" class="btn  btn-xs" href="#"  onclick="Clinical_LabOrder.printSpecimen(\'' + item.LabOrderId + '\',\'' + item.Status + '\',event );" ><i class="fa fa-barcode"></i> </a></td><td ' + editMode + '>'
                        + utility.RemoveTimeFromDate(null, item.OrderDate) + ' ' + item.OrderTime + '</td><td onclick="' + onCellClick + '">' + expandCollapseIcon + divLoinc + '</td><td ' + editMode + '>' + item.LabName + '</td><td ' + editMode + '>' + item.OrderNo + '</td><td ' + editMode + '>' + item.Status + '</td><td ' + editMode + '>' + item.Provider + '</td><td ' + editMode + '>' + item.AssigneeName + '</td>');
                    //End 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578
                }
                //End//20-04-2016//Ahmad Raza//implimented logic to show icon for showing DBAudit History
                $("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_Result #" + Clinical_LabOrder.orderSearchGridId + " tbody").last().append($row);

                var childRows = Clinical_LabOrder.buildLabOrderRowChild(item.LabOrderTests, item.LabOrderId);
                Clinical_LabOrder.labOrderRows.push({ row: $row, childs: childRows });
                //Bind values
                //childRows.loadDropDowns(true).done(function () {
                //    //Bind Values to the child row
                utility.bindMyJSONByName(true, item, false, childRows).done(function () {
                    childRows.find('#orderNo').text("Order Number: " + item.OrderNo);
                    childRows.find('#laboratory').text("Lab: " + item.LabName);
                });

            });

        }
        else {

            $("#" + Clinical_LabOrder.params.PanelID + ' #pnlLabOrder_Result #' + Clinical_LabOrder.orderSearchGridId).DataTable({
                "destroy": true,
                "language": {
                    "emptyTable": "No Lab Order Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_LabOrder.params.PanelID + ' #pnlLabOrder_Result #' + Clinical_LabOrder.orderSearchGridId))
            ;
        else {
            Clinical_LabOrder.EditableGridOrder = $("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_Result #" + Clinical_LabOrder.orderSearchGridId).DataTable({
                "destroy": true,
                "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1], "orderable": false, "aTargets": [0] }], "aaSorting": [[2, "desc"]]
            }); // to remove records per page dropdown
        }

        EMRUtility.fixDataTableDuplication("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_Result");

        $.each(Clinical_LabOrder.labOrderRows, function (i, item) {

            if (Clinical_LabOrder.EditableGridOrder != null) {

                var row = Clinical_LabOrder.EditableGridOrder.row(item.row);
                if (item.childs.length > 0) {
                    row.child(item.childs);
                }
                else {
                    //$(item.row).find('td:first').find('a').hide();
                }
            }
        });
    },

    printSpecimen: function (labOrderId, Status, event, ParentCtrl) {
        var params = [];
        params["PatientID"] = Clinical_LabOrder.params.patientID;
        params["LabOrderId"] = labOrderId;
        params["providerid"] = $('#pnlClinicalLabOrder #hfprovider').val();
        params["ParentCtrl"] = 'Clinical_LabOrder';
        if (Clinical_LabOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
            if (ParentCtrl == 'Clinical_LabOrderView') {
                params["ParentCtrl"] = ParentCtrl;
                LoadActionPan('ClinicalBarCodeView', params);
            } else {
                params["ParentCtrlPanelID"] = Clinical_LabOrder.params.PanelID;
                if (ParentCtrl != null) {
                    params["ParentCtrl"] = ParentCtrl;
                }
                LoadActionPan('ClinicalBarCodeView', params, Clinical_LabOrder.params.PanelID);
            }

        } else if (ParentCtrl == 'Clinical_LabOrderView') {
            params["ParentCtrl"] = ParentCtrl;
            LoadActionPan('ClinicalBarCodeView', params);
        } else if (ParentCtrl == 'Clinical_LabOrder') {
            params["ParentCtrl"] = ParentCtrl;
            LoadActionPan('ClinicalBarCodeView', params);
        }
        else if (ParentCtrl == 'ClinicalLabOrderDetail') {
            params["ParentCtrl"] = ParentCtrl;
            LoadActionPan('ClinicalBarCodeView', params);
        }
        else {
            LoadActionPan('ClinicalBarCodeView', params);
        }
    },
    showAttachment: function (ParentCtrl, PatientId, PatDocID, event, obj) {
        var strMessage = "";
        $(obj).attr('class', 'active');
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Documents", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["PatientID"] = PatientId;//$('#PatientProfile #hfPatientId').val();
                params["PatDocID"] = PatDocID;
                params["mode"] = "Edit";
                params["FromAdmin"] = "0";

                if (Clinical_LabOrder.params["ParentCtrl"] == "clinicalTabProgressNote" && ParentCtrl != "mstrTabDashBoard") {
                    params["ParentCtrl"] = 'Clinical_LabOrder';
                    params["ParentCtrlPanelID"] = Clinical_LabOrder.params.PanelID;
                    LoadActionPan('Document_Viewer', params, Clinical_LabOrder.params.PanelID);

                } else {
                    params["ParentCtrl"] = ParentCtrl;
                    LoadActionPan('Document_Viewer', params);
                }


            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    LabGridLoadSent: function (response) {
        var dfd = $.Deferred();
        //Start//Abid Ali
        Clinical_LabOrder.labOrderRowsSent = [];
        var maxLabOrderId = 0;
        //Start 24-05-2016 Muhammad Arshad Remove Duplicate search issue on Datatable
        if ($.fn.dataTable.isDataTable("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_ResultSent #dgvLabOrdertSent")) {
            $("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_ResultSent #dgvLabOrdertSent").dataTable().fnClearTable();
            $("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_ResultSent #dgvLabOrdertSent").dataTable().fnDestroy();
        }
        $("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_ResultSent #dgvLabOrdertSent tbody").find("tr").remove();
        if ($("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_ResultSent #dgvLabOrdertSent thead tr #selectOrders").length == 0) {
            $("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_ResultSent #dgvLabOrdertSent thead tr").prepend('<th style="width:50px"><input type="checkbox" onchange="Clinical_LabOrder.checkUncheckAllOrders(this);" controlname="selectOrders" id="selectOrders" name="chkHeader" class="input-block" coltype="checkbox"/></th>');
        } else {
            $("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_ResultSent #dgvLabOrdertSent thead tr #selectOrders").prop('checked', false);
        }
        //End 24-05-2016 Muhammad Arshad Remove Duplicate search issue on Datatable
        //$("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_Result #dgvLabOrder").dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it
        //$("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_Result #dgvLabOrder tbody").find("tr").remove(); //Removing all the table data from table body
        if (response.LabOrderCount > 0) {


            //var headerRow = '<th class="size30"></th>'
            //$("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_ResultSent #dgvLabOrdertSent").find('thead').find('tr').prepend(headerRow);


            var LabLoadJSONData = JSON.parse(response.LabOrderFill_JSON);
            //JSON.parse(response.LabLoad_JSON); //Parsing array to JSON
            $.each(LabLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                // $row.attr("onclick", "ClinicalLabOrderDetail.LabEdit('" + item.LabOrderId + "',event);");
                $row.attr("id", "gvLab_rowSent" + item.LabOrderId);
                $row.attr("LabId", item.LabOrderId);
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


                if ($("#" + Clinical_LabOrder.params.PanelID + ' #dgvLabOrder tbody').find("tr").length < 2) {
                    if (parseInt(item.LabOrderId) > parseInt(maxLabOrderId)) {
                        maxLabOrderId = item.LabOrderId;
                        Clinical_LabOrder.params["LastLabName"] = item.LabName;
                    }
                }

                var onclick = "Clinical_LabOrder.labOrderRowExpand(this,event,'sent');"
                //Start//21-04-2016//Ahmad Raza//logic to check uncheck order if it is attached with Note
                var expandCollapseIcon = '<a href="#" onclick="' + onclick + '" class="tab_space" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a>';


                var SelectionCheckBoxColumn = "";
                var Checked = "";
                if (item.IsNoteLinked == "True") {
                    if ($.inArray(item.LabOrderId + 'ordr', Clinical_ProgressNote.AttachedNoteComponentIds) == -1) {
                        Clinical_ProgressNote.AttachedNoteComponentIds.push(item.LabOrderId + 'ordr');
                    }
                    if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.LabOrderId + "ordr", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                        Checked = " ";
                    } else {
                        Checked = " checked";
                    }
                } else {
                    if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.LabOrderId + "ordr", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                        Checked = " checked";
                    } else {
                        Checked = "";
                    }
                }

                if (Clinical_LabOrder.params.ParentCtrl == "clinicalTabProgressNote") {

                    for (i = 0; i < ClinicalLabResultDetail.explicitResultOrderId.length; i++) {
                        if (item.OrderNo == ClinicalLabResultDetail.explicitResultOrderId[i]) {
                            Checked = "";
                        }
                    }

                }
                if (Clinical_LabOrder.params.ParentCtrl != "clinicalTabProgressNote") {
                    Checked = "";
                }
                SelectionCheckBoxColumn = '<a class="btn btn-xs" href="#" ><input type="checkbox" onclick="Clinical_LabOrder.enableAddOrder(this,event);" id="' + item.LabOrderId + '" name="SelectCheckBoxOrder" ' + Checked + ' class="input-block"/></a>';
                var editMode = 'onclick="ClinicalLabOrderDetail.LabEdit(\'' + item.LabOrderId + '\',event);"';

                var divLoinc = "";
                var totalTests = 1;
                if (item.Test.indexOf('|') > -1) {
                    totalTests = item.Test.split('|').length;
                }

                for (var i = 0; i < totalTests; i++) {
                    if (totalTests > 0) {
                        divLoinc += item.Test.split('|')[i] + "<br />";
                    }
                }
                if (divLoinc != "") {
                    divLoinc = divLoinc.trim();
                    //divLoinc = divLoinc.replace(/,\s*$/, "");
                }
                var onCellClick = "Clinical_LabOrder.labOrderRowExpand(this,event,'sent','cell')";
                //End//21-04-2016//Ahmad Raza//logic to check uncheck order if it is attached with Note
                //Start//20-04-2016//Ahmad Raza//implimented logic to show icon for showing DBAudit History
                if (item.Status == "Transmitted") {

                    //Begin 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578
                    $row.append('<td style="display:none;">' + item.LabOrderId + '</td><td>' + SelectionCheckBoxColumn + '&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_LabOrder.printLabOrder(\'' + item.LabOrderId + '\',\'' + item.Status + '\',event );" title="View Result"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_LabOrder.showOrderHistory(\'' + item.LabOrderId + '\', event );" title="Activity Log"> <i class="fa fa-history blue"></i></a>&nbsp;<a title="Print Specimen Label" class="btn  btn-xs" href="#"  onclick="Clinical_LabOrder.printSpecimen(\'' + item.LabOrderId + '\',\'' + item.Status + '\',event );" ><i class="fa fa-barcode"></i> </a></td><td ' + editMode + '>'
                    + utility.RemoveTimeFromDate(null, item.OrderDate) + ' ' + item.OrderTime + '</td><td onclick="' + onCellClick + '">' + expandCollapseIcon + divLoinc + '</td><td ' + editMode + '>' + item.LabName + '</td><td ' + editMode + '>' + item.OrderNo + '</td><td ' + editMode + '>' + item.Status + '</td><td ' + editMode + '>' + item.Provider + '</td><td ' + editMode + '>' + item.AssigneeName + '</td>');
                    //End 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578

                }
                else {

                    //Begin 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578
                    $row.append('<td style="display:none;">' + item.LabOrderId + '</td><td>' + SelectionCheckBoxColumn + '&nbsp;<a class="btn  btn-xs disableAll" href="#" onclick="Clinical_LabOrder.printLabOrder(\'' + item.LabOrderId + '\',\'' + item.Status + '\',event );" title="View Result"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_LabOrder.showOrderHistory(\'' + item.LabOrderId + '\', event );" title="Activity Log"> <i class="fa fa-history blue"></i></a>&nbsp;<a title="Print Specimen Label" class="btn  btn-xs" href="#"  onclick="Clinical_LabOrder.printSpecimen(\'' + item.LabOrderId + '\',\'' + item.Status + '\',event );" ><i class="fa fa-barcode"></i> </a></td><td ' + editMode + '>'
                        + utility.RemoveTimeFromDate(null, item.OrderDate) + ' ' + item.OrderTime + '</td><td >' + expandCollapseIcon + divLoinc + '</td><td ' + editMode + '>' + item.LabName + '</td><td ' + editMode + '>' + item.OrderNo + '</td><td ' + editMode + '>' + item.Status + '</td><td ' + editMode + '>' + item.Provider + '</td><td ' + editMode + '>' + item.AssigneeName + '</td>');
                    //End 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578
                }
                //End//20-04-2016//Ahmad Raza//implimented logic to show icon for showing DBAudit History
                $("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_ResultSent #dgvLabOrdertSent tbody").last().append($row);

                var childRows = Clinical_LabOrder.buildLabOrderRowChild(item.LabOrderTests, item.LabOrderId);
                Clinical_LabOrder.labOrderRowsSent.push({
                    row: $row, childs: childRows
                });

                utility.bindMyJSONByName(true, item, false, childRows).done(function () {
                    childRows.find('#orderNo').text("Order Number: " + item.OrderNo);
                    childRows.find('#laboratory').text("Lab: " + item.LabName);
                });


            });

        }
        else {

            $("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_ResultSent #dgvLabOrdertSent").DataTable({
                "destroy": true,
                "language": {
                    "emptyTable": "No Lab Order Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_ResultSent #dgvLabOrdertSent"))
            ;
        else {
            Clinical_LabOrder.EditableGridOrderSent = $("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_ResultSent #dgvLabOrdertSent").DataTable({
                "destroy": true,
                "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1], "orderable": false, "aTargets": [0] }]
            }); // to remove records per page dropdown
        }

        EMRUtility.fixDataTableDuplication("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_ResultSent");

        $.each(Clinical_LabOrder.labOrderRowsSent, function (i, item) {

            if (Clinical_LabOrder.EditableGridOrderSent != null) {

                var row = Clinical_LabOrder.EditableGridOrderSent.row(item.row);
                if (item.childs.length > 0) {
                    row.child(item.childs);
                }
                else {
                    //$(item.row).find('td:first').find('a').hide();
                }
            }
        });
        dfd.resolve();
        return dfd;
    },



    buildLabOrderRowChild: function (tests, labOrderId) {

        var CurrentRowchilds = $();
        var templateHtml = $("#" + Clinical_LabOrder.params.PanelID + " #LabOrderTemplate").clone();

        if (tests != null && tests.length > 0) {

            var $tbody = templateHtml.find('#dgvLabOrderTemplate').find('tbody');

            var onClick = "Clinical_LabOrder.LabResultAddEdit(-1,'" + labOrderId + "',false,null,this)";
            $.each(tests, function (i, item) {

                var link = item.LabOrderResultDetails.length > 0 ? "View Result" : "Add Result";
                var i = 1;
                do {
                    var $ChilRowDetail = $('<tr/>').addClass("childRowDetail-bg");
                    if (i == 1) {
                        $ChilRowDetail.append('<td class="bold" colspan="2" >' + item.CPTDescription + '</td> <td></td>');
                    }
                    else {
                        $ChilRowDetail.append('<td colspan="2" >' + item.CPTDescription + '</td> <td ><a onclick="' + onClick + ' " href="#" >' + link + '</a></td>');
                        if (item.AOEs != null || item.AOEs != "") {
                            item.AOEs = item.AOEs.replace(/;/g, '<br>');
                            item.AOEs = item.AOEs.replace(/Q-/g, '<b>');
                            item.AOEs = item.AOEs.replace(/A-/g, '</b>');
                            var $ChilRowDetailAOE = $('<tr/>').addClass("childRowDetail-bg");
                            $ChilRowDetailAOE.append('<td colspan="2" >' + item.AOEs + '</td> <td></td>');
                        }
                    }
                    $tbody.append($ChilRowDetail);
                    if ($ChilRowDetailAOE != undefined) {
                        $tbody.append($ChilRowDetailAOE);
                    }
                    i++;
                } while (i <= 2)
            });
        }

        var $row = $('<tr/>').addClass("childRow-bg");
        var cellpadding = '';

        $row.append('<td class="hidden"></td> <td class="hidden"> <td class="hidden"></td>  <td class="hidden"></td> <td colspan="9">' + templateHtml.html() + '</td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td>');
        return CurrentRowchilds = CurrentRowchilds.add($row);

    },


    labOrderRowExpand: function ($row, event, gridType, from) {
        event.stopPropagation();
        event.preventDefault();
        var currentRowId = null;
        var currentgrid = $("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_ResultSent #dgvLabOrdertSent tbody tr");
        if (from == "cell") {
            $row = $($row).parent();
        } else {
            $row = $($row).parent().parent();
        }
        currentRowId = $($row).attr('id');
        var $actions,
         values = [];
        var row = null;
        if (gridType != null) {
            row = Clinical_LabOrder.EditableGridOrderSent.row($row);

        }
        else {
            row = Clinical_LabOrder.EditableGridOrder.row($row);
            currentgrid = $("#" + Clinical_LabOrder.params.PanelID + " #pnlLabOrder_Result #" + Clinical_LabOrder.orderSearchGridId + " tbody tr");
        }

        if (row.child.isShown()) {
            // This row is already open - close it
            $row.find(".fa-minus-square").attr("class", "fa fa-plus-square");
            row.child.hide();
            //tr.removeClass('shown');
        }
        else {
            $row.find(".fa-plus-square").attr("class", "fa fa-minus-square");
            // Open this row
            row.child.show();
        }
        currentgrid.each(function (i, item) {
            if (currentRowId != $(item).attr('id')) {
                var allotherrows = null;
                if (gridType == null) {
                    allotherrows = Clinical_LabOrder.EditableGridOrder.row(item);
                } else {
                    allotherrows = Clinical_LabOrder.EditableGridOrderSent.row(item);
                }

                if (allotherrows.child.isShown()) {
                    $(item).find(".fa-minus-square").attr("class", "fa fa-plus-square");
                    allotherrows.child.hide();

                }
            }
        });

    },



    //Function Name:enableAddOrder
    //Author Name: Ahmad Raza
    //Created Date: 21-04-2016
    //Description: this function will add/remove attached/detached orders in/from json
    enableAddOrder: function (obj, event) {
        if (event != null) {
            event.stopPropagation();
        }

        var totalRows = $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrdertSent tr").length;
        totalRows -= 1;
        var selectedRows = $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrdertSent tbody tr input:checked").length;
        if (totalRows == selectedRows) {
            $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrdertSent #selectRecordOrders").prop("checked", true);
        }
        else {
            $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrdertSent #selectRecordOrders").prop("checked", false);
        }

        totalRows = $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrder tr").length;
        totalRows -= 1;
        selectedRows = $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrder tbody tr input:checked").length;
        if (totalRows == selectedRows) {
            $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrder #selectRecordOrders").prop("checked", true);
        }
        else {
            $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrder #selectRecordOrders").prop("checked", false);
        }

        // console.log("Id: " + obj.id + " || checked: " + $(obj).is(':checked') + " || Table: " + $(obj.parentElement.parentElement.parentElement.parentElement).attr("id"));
        if ($(obj).is(':checked')) {

            if ($.inArray(obj.id + 'ordr', Clinical_ProgressNote.AttachedNoteComponentIds) == -1) {
                Clinical_ProgressNote.AttachedNoteComponentIds.push(obj.id + 'ordr');
            }

            if ($.inArray(obj.id + 'ordr', Clinical_ProgressNote.DetachedNoteComponentIds) != -1) {
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

    getLabOrderInfo: function (labOrderId, detachedvalues, hideAlertMessage, fromResult) {
        if (labOrderId == null || labOrderId == '') {
            return false;
        }
        var dfd = new $.Deferred();
        Clinical_LabOrder.getOrdersForSOAP_DBCall(labOrderId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {

                    if (response.MedicationSoapCount > 0) {
                        Clinical_LabOrder.createMedicationBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', labOrderId, detachedvalues, hideAlertMessage, fromResult);

                    }
                    if (response.ProblemListSoapCount > 0) {
                        Clinical_ProblemLists.createProblemListBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, true, fromResult);
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
            dfd.resolve('ok');
        });
        return dfd.promise();
    },

    getOrdersForSOAP_DBCall: function (labOrderId) {
        var objData = new Object();
        objData["LabOrderIDs"] = labOrderId;
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["PatientId"] = Clinical_LabOrder.params.patientID;
        objData["ProviderId"] = $("#pnlClinicalProgressNote #hfProviderId").val();
        objData["commandType"] = "get_Orders_forsoap";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabOrder", "LabOrder");
    },
    checkMedicationsExists: function () {
        //if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_laborders').length == 0) {
        //    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList').append(' <li> <header>' +
        //                '<clinical_laborders title="Lab Orders"  id="' + this.id + '" class="NotesComponent">' +
        //                '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'LabOrders\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Lab Order">Lab Order</a> ' +
        //                                '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'LabOrders\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
        //                           '</clinical_laborders> </header></li>');
        //    Clinical_ProgressNote.CreateNotesComponent_Buttons($('#' + Clinical_ProgressNote.params["PanelID"] + ' #hfNotesId').val());
        //    $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
        //        $(this).find('.closeBtn').removeClass('hidden');
        //        $(this).css('background', '#EAF1F8');
        //    });

        //    $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
        //        $(this).find('.closeBtn').addClass('hidden');
        //        $(this).css('background', '#fff');
        //    });
        //}


        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_laborders').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #PlanNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="LabOrdersComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_laborders title="Lab Orders"  id="clinicalMenu_Orders_Lab" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'LabOrders\',\'clinicalMenu_Orders_Lab\',' + Clinical_ProgressNote.params.NotesId + ');" title="Lab Orders">Lab Orders</a> ' +
                        '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'LabOrders\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'Lab Orders\',\'clinicalMenu_Orders_Lab\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_laborders> </header></li>');
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
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_laborders').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
    },
    checkResultExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_labresults').length == 0) {

            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ObjectiveNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="LabResultsComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_labresults title="Lab Results"  id="' + this.id + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'LabResults\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Lab Results">Lab Results</a> ' +
                        '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'LabResults\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'LabResults\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_labresults> </header></li>');
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
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_labresults').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($('#' + Clinical_ProgressNote.params["PanelID"] + ' #hfNotesId').val());

    },
    createMedicationBodyHTML: function (response, noteHTMLCtrl, medicationsId, detachedvalues, hideAlertMessage, fromResult) {
        Clinical_LabOrder.checkMedicationsExists();
        if (response.MedicationSoap_JSON != null && response.MedicationSoap_JSON != '') {

            if (!fromResult) {
                $(".LabOrdersComponent section").remove();
                $(".LabOrdersComponent span").remove();

                $(".LabOrderComponent section").remove();
                $(".LabOrderComponent span").remove();
            }
            var sendOrderList = [];

            var MedicationsSOAPJSON = response.MedicationSoap_JSON != null ? JSON.parse(response.MedicationSoap_JSON) : null;
            var LabOrderTestSOAPJSON = response.LabOrderTest_JSON != null ? JSON.parse(response.LabOrderTest_JSON) : null;
            var LabOrderAssociatedProblemSOAPJSON = response.LabOrderProblem_JSON != null ? JSON.parse(response.LabOrderProblem_JSON) : null;

            //      var medicationReviewSoapJSON = [];
            if (response.MedicationReviewSoap_JSON != null) {
                //          medicationReviewSoapJSON = JSON.parse(response.MedicationReviewSoap_JSON);
            }

            var $mainDivMedications = $(document.createElement('div'));
            if (MedicationsSOAPJSON == null || MedicationsSOAPJSON.length == 0) {
                //if (medicationReviewSoapJSON != null || medicationReviewSoapJSON.length != 0) {
                //    $.each(medicationReviewSoapJSON, function (index, item) {
                //        if (item.ReviewedOn != null && item.ReviewedOn != '') {
                //            var dateFormat = item.ReviewedOn
                //            var ReviewedOndateFormat = $.datepicker.formatDate('MM dd, yy ', new Date(dateFormat)) + utility.RemoveDateFromDateTime(null, dateFormat);
                //            $ListMedications.append("<li>" + (item.ReviewedBy == '' ? "" : "Medications reviewed by " + item.ReviewedBy) +
                //       " on:  " + ReviewedOndateFormat + "</li>");
                //        }
                //    });
                //} else {
                //    return "";
                //}
            } else {
                if ($(noteHTMLCtrl + ' clinical_laborders').parent().parent().find('#Cli_LabOrderDetail_Main0').length != 0) {
                    $(noteHTMLCtrl + ' #Cli_LabOrderDetail_Main0').parent().remove();
                }
            }

            var AListId = [];

            $.each(MedicationsSOAPJSON, function (index, element) {
                var LabOrderTestSoapText = "", LabOrderAssociatedProblemSoapText = "", LabOrderDate = "";
                var ALid = element.LabOrderId;
                //             var $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(element.NDCID, 'clinicalTabProgressNote', 1);
                var $SectionBodyMedications = $(document.createElement('section'));
                $SectionBodyMedications.attr("data-status", element.Status);
                $SectionBodyMedications.attr('id', "Cli_LabOrderDetail_Main" + ALid);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_LabOrderDetail_" + ALid);
                var $ListMedications = $(document.createElement('ul'));
                var duration = "";
                $ListMedications.attr('class', 'list-unstyled')

                $SectionBodyMedications.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_LabOrderDetail_" + ALid + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_LabOrderDetail_Main" + ALid + '"  ><i class="fa fa-times"></i></a></div> ');
                var obj = $('<p>');
                if (element.SoapText != "") {
                    $(obj).html(element.SoapText);
                    //$(obj).html($(obj).text())
                    $ListMedications.append("<li>" + $(obj).text() + "</li>");
                }
                else {
                    if (LabOrderTestSOAPJSON) {

                        $.each(LabOrderTestSOAPJSON, function (i, item) {
                            if (element.LabOrderId == item.LabOrderId) {
                                LabOrderTestSoapText += (item.ShowCPTCode == 1 ? "<b>" + item.CPTCode + " " + item.CPTCodeDescription + "</b>" : "<b>" + item.CPTCodeDescription + "</b>")
                                    + (item.Specimen != "" ? "<b> Specimen:</b> " + item.Specimen : "")
                                    + (item.Volume != "" ? "<b> Volume:</b> " + item.Volume : "")
                                    + (item.UrgencyName != "" ? "<b> Urgency:</b> " + item.UrgencyName : "")
                                    + "<br/>";
                            }
                        });
                    }
                    if (LabOrderAssociatedProblemSOAPJSON) {

                        $.each(LabOrderAssociatedProblemSOAPJSON, function (ind, value) {
                            if (element.LabOrderId == value.LabOrderId) {
                                LabOrderAssociatedProblemSoapText += (value.SoapText != "" ? "<b>" + value.SoapText + "</b>" : "") + "</br>";
                            }
                        });
                    }
                    if (element.CreatedOn == element.ModifiedOn) {
                        LabOrderDate = " Added On " + utility.RemoveTimeFromDate(null, element.CreatedOn);
                    }
                    else {
                        LabOrderDate = " Last Updated On " + utility.RemoveTimeFromDate(null, element.ModifiedOn);
                    }
                    $ListMedications.append("<li>" + LabOrderTestSoapText + (LabOrderAssociatedProblemSoapText != "" ? " Associated Problem(s)  <br/>" + LabOrderAssociatedProblemSoapText : "") + LabOrderDate + "</li>");

                }
                //$ListMedications.append("<li>" + $(obj).text() + "</li>");

                //             $ListMedications.append((element.Comments == "" ? "" : "<li>Comments: " + element.Comments));
                //if (MedicationsSOAPJSON.length - 1 == index) {
                //$.each(medicationReviewSoapJSON, function (index, item) {
                //    if (item.ReviewedOn != null && item.ReviewedOn != '') {
                //        var dateFormat = item.ReviewedOn
                //        var ReviewedOndateFormat = $.datepicker.formatDate('MM dd, yy ', new Date(dateFormat)) + utility.RemoveDateFromDateTime(null, dateFormat);
                //        $ListMedications.append("<li>" + (item.ReviewedBy == '' ? "" : "Medications reviewed by " + item.ReviewedBy) +
                //   " on:  " + ReviewedOndateFormat + "</li>");
                //    }
                //});
                //}
                $DetailsDiv.append($ListMedications);
                $SectionBodyMedications.append($DetailsDiv);
                //if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML clinical_medications').parent().parent().find('#Cli_Medications_Main' + ALid).length == 0) {
                if ($(noteHTMLCtrl + ' clinical_laborders').parent().parent().find('#Cli_LabOrderDetail_Main' + ALid).length == 0) {
                    AListId.push(ALid);

                    if (element.Status == "Pending") {
                        if ($($mainDivMedications).find("#pendingordertext").length == 0) {
                            $mainDivMedications.append("<section id='pendingordertext'  class='pl-default text-info'>Pending Orders</section>");
                        }
                        $mainDivMedications.append($SectionBodyMedications);
                    } else {
                        sendOrderList.push($SectionBodyMedications);
                    }
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(noteHTMLCtrl + ' clinical_laborders').parent().parent().find('#Cli_LabOrderDetail_Main' + ALid + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(noteHTMLCtrl + ' clinical_laborders').parent().parent().find('#Cli_LabOrderDetail_Main' + ALid + ' ul li:Last').get(0).outerHTML;
                    }
                    $(noteHTMLCtrl + ' clinical_laborders').parent().parent().find('#Cli_LabOrderDetail_Main' + ALid).html($SectionBodyMedications.html());
                    $(noteHTMLCtrl + ' clinical_laborders').parent().parent().find('#Cli_LabOrderDetail_Main' + ALid + ' ul').append(CommentHTML);;
                }


            });

            var sentOrderTag = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find(".LabOrdersComponent > section#sentordertext");
            if (sendOrderList.length > 0 && sentOrderTag.length == 0) {
                $mainDivMedications.append("<section id='sentordertext' class='pl-default text-info'>Sent Orders</section>");
            }
            $.each(sendOrderList, function (index, element) {
                $mainDivMedications.append(element);
            });

            if (AListId.join(",") != "") {
                medicationsId = AListId.join(",");
            }
            if ($mainDivMedications.html() != '') {
                //Start 25-11-2016 Edit By Humaira Yousaf Bug# IMP-9
                Clinical_LabOrder.updateMedicationsHtml($mainDivMedications.html(), medicationsId, noteHTMLCtrl, null, hideAlertMessage, fromResult);
                //End 25-11-2016 Edit By Humaira Yousaf Bug# IMP-9
            } else {
                Clinical_LabOrder.updateMedicationsHtml('', medicationsId, noteHTMLCtrl, detachedvalues, hideAlertMessage, fromResult);
                //Clinical_ProgressNote.saveComponentSOAPText();
            }
        }

    },

    updateMedicationsHtml: function (medicationsHTML, medicationsID, noteHTMLCtrl, detachedvalues, hideAlertMessage, fromResult) {
        $(noteHTMLCtrl + ' clinical_laborders').parent().parent().addClass('initialVisitBody');
        if (medicationsHTML != '') {
            $(noteHTMLCtrl + ' clinical_laborders').parent().parent().append(medicationsHTML);
        }

        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (medicationsHTML != '' && medicationsID != null && medicationsID != '' && medicationsID != '0' && fromResult != true) {
            Clinical_LabOrder.attachMedicationFromNotes(medicationsID, hideAlertMessage);
        } else if (detachedvalues != null && detachedvalues != '' && fromResult != true) {
            Clinical_ProgressNote.saveComponentSOAPText("Lab Orders", hideAlertMessage);
        }
        else {
            Clinical_ProgressNote.saveComponentSOAPText("Lab Orders", hideAlertMessage);
        }
    },
    updateResultHtml: function (resultHTML, resultID, noteHTMLCtrl, hidemessage, bNotSaveCompt) {
        $(noteHTMLCtrl + ' clinical_labresults').parent().parent().addClass('initialVisitBody');
        if (resultHTML != '') {
            $(noteHTMLCtrl + ' clinical_labresults').parent().parent().append(resultHTML);
        }

        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (resultID != null && resultID != '' && resultID != '0') {
            Clinical_LabOrder.attachResultFromNotes(resultID, hidemessage, bNotSaveCompt);
        }

    },

    attachMedicationFromNotes: function (medicationID, hidemessage) {
        var selectedValue = medicationID;
        if (selectedValue == "" || selectedValue == "undefined") {
        }
        else {
            Clinical_LabOrder.attachMedicationsWithNotes_DBCall(selectedValue).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    Clinical_ProgressNote.saveComponentSOAPText('Lab Orders');
                    //Start 01-12-2016 Humaira Yousaf to hide/show eSuperbill link
                    Clinical_ProgressNote.HideShowBillingInfo();
                    //End 01-12-2016 Humaira Yousaf to hide/show eSuperbill link
                    $('#' + medicationID).remove();

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    attachResultFromNotes: function (resultID, hidemessage, bNotSaveCompt) {
        var selectedValue = resultID;
        if (selectedValue == "" || selectedValue == "undefined") {
        }
        else {
            Clinical_LabOrder.attachResultsWithNotes_DBCall(selectedValue).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (!bNotSaveCompt) {
                        Clinical_ProgressNote.saveComponentSOAPText("Lab Results");
                        var LabOrderIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_labresults').parent().parent().find('section[id*="Cli_LabResultDetail_Main"]').map(function () {
                            return $(this).attr('laborderid');
                        }).get().join(',');
                        Clinical_LabOrder.getLabOrderInfo(LabOrderIds, null, hidemessage, true);
                    }
                    $('#' + resultID).remove();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },



    attachMedicationsWithNotes_DBCall: function (medicationId) {
        var objData = new Object();
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["LabOrderIDs"] = medicationId;
        objData["commandType"] = "attach_orders_with_notes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabOrder", "LabOrder");
    },
    attachResultsWithNotes_DBCall: function (resultId) {
        var objData = new Object();
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["LabResultIDs"] = resultId;
        objData["commandType"] = "attach_results_with_notes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },


    addOrdersToNotes: function (selectedAttachedOrders, selectedDetachedOrders, hideAlertMessage) {
        var noteHTMLCtrl = '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML';
        if ($("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrdertSent").dataTable().fnSettings().aoData.length == 0 && $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrder").dataTable().fnSettings().aoData.length == 0) {
            //   Clinical_LabOrder.noActiveLabOrderSoapText();
        }
        else {

            if ($("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrdertSent" + " #selectRecordOrders").prop('checked') == true || $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrdertSent" + " #selectRecordOrders").prop('checked') == false) {

                $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrdertSent tbody").find('input[type="checkbox"]').each(function () {
                    Clinical_LabOrder.enableAddOrder(this, null);
                });
            }
            // for pending orders
            if ($("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrder" + " #selectRecordOrders").prop('checked') == true || $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrder" + " #selectRecordOrders").prop('checked') == false) {

                $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrder tbody").find('input[type="checkbox"]').each(function () {
                    Clinical_LabOrder.enableAddOrder(this, null);
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
                //Check for Medications Values
                //if (AttachSelectedOrdersAndResults.join().indexOf("prsc") != -1 || DettachSelectedOrdersAndResults.join().indexOf("prsc") != -1) {
                //    var AttachSelectedResults = EMRUtility.slicefunc(AttachSelectedOrdersAndResults, "prsc", 0, -4);
                //    var dettachSelectedResults = EMRUtility.slicefunc(DettachSelectedOrdersAndResults, "prsc", 0, -4);
                //    Clinical_Prescriptions.addPrescriptionsToNotes(AttachSelectedResults, dettachSelectedResults);
                //}
                AttachedSelectedOrders = EMRUtility.slicefunc(AttachSelectedOrdersAndResults, "ordr", 0, -4);
                DettachedSelectedOrders = EMRUtility.slicefunc(DettachSelectedOrdersAndResults, "ordr", 0, -4);
            }
            var detachedvalues = DettachedSelectedOrders;

            //////if (AttachedSelectedOrders != null && AttachedSelectedOrders != '') {
            //////    for (var i = 0; i < Clinical_ProgressNote.AttachedNoteComponentIds.length; i++) {
            //////        var ALid = Clinical_ProgressNote.AttachedNoteComponentIds[i];
            //////        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_LabOrderDetail_Main' + ALid).length != 0) {
            //////            var index = AttachedSelectedOrders.indexOf(ALid);
            //////            if (index > -1) {
            //////                AttachedSelectedOrders.splice(index, 1);
            //////            }
            //////        }
            //////    }
            //////}
            if (detachedvalues.join() != null && detachedvalues.join() != '') {
                Clinical_LabOrder.detachedLabOrder(detachedvalues).done(function () {
                    if (AttachedSelectedOrders.join() != null && AttachedSelectedOrders.join() != '') {
                        Clinical_LabOrder.attachedLabOrder(AttachedSelectedOrders, detachedvalues, hideAlertMessage);
                    } else {

                        $(".LabOrdersComponent section").remove();
                        $(".LabOrdersComponent span").remove();

                        $(".LabOrderComponent section").remove();
                        $(".LabOrderComponent span").remove();

                        Clinical_ProgressNote.saveComponentSOAPText("Lab Orders", hideAlertMessage);
                    }
                });
            }
            else if (AttachedSelectedOrders.join() != null && AttachedSelectedOrders.join() != '') {
                Clinical_LabOrder.attachedLabOrder(AttachedSelectedOrders, detachedvalues, hideAlertMessage);
            }
            if (Clinical_LabOrder.params && Clinical_LabOrder.params.ParentCtrl && Clinical_LabOrder.params.ParentCtrl == "clinicalTabProgressNote") {

                if ($("#" + Clinical_LabOrder.params.PanelID + " #PatientlistLabOrderPending").hasClass('active') || $("#" + Clinical_LabOrder.params.PanelID + " #PatientlistLabOrderSent").hasClass('active')) {
                    EMRUtility.scrollToPNcomponent('clinical_laborder');
                    EMRUtility.scrollToPNcomponent('clinical_laborders');
                }
                if ($("#" + Clinical_LabOrder.params.PanelID + " #PatientlistLabResult").hasClass('active')) {
                    EMRUtility.scrollToPNcomponent('clinical_labresults');
                    EMRUtility.scrollToPNcomponent('clinical_labresult');
                }
                UnloadActionPan(Clinical_LabOrder.params.ParentCtrl, 'Clinical_LabOrder');
            }
        }
    },

    attachedLabOrder: function (AttachedSelectedOrders, detachedvalues, hideAlertMessage) {
        Clinical_LabOrder.getLabOrderInfo(AttachedSelectedOrders.join(), detachedvalues, hideAlertMessage).done(function () {
            setTimeout(function () {
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                if (Clinical_LabOrder.params != null && Clinical_LabOrder.params.PanelID.indexOf('pnlClinicalLabOrder') != -1) {
                    Clinical_LabOrder.LabOrderSearch('0', null, null, null, 'Transmitted');
                }
            }, 5);
        });
    },
    detachedLabOrder: function (detachedvalues) {
        var dfd = new $.Deferred();

        Clinical_LabOrder.detachMedicationsFromNotes_DBCall(detachedvalues.join()).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                for (var i = 0; i < detachedvalues.length; i++) {
                    var ALid = detachedvalues[i];
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_LabOrderDetail_Main' + ALid).remove();
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
            dfd.resolve('ok');
        });
        return dfd.promise();
    },
    //noActiveLabOrderSoapText: function () {
    //    var noteHTMLCtrl = '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML';
    //    if ($(noteHTMLCtrl + ' clinical_radiologyorder').parent().parent().find('#Cli_RadiologyOrderDetail_Main0').length == 0) {
    //        Clinical_LabOrder.checkMedicationsExists();
    //        var htmlForNoMedication = '<section id="Cli_RadiologyOrderDetail_Main0"> <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="Cli_RadiologyOrderDetail_0"><i class="fa fa-edit"></i></a>' +
    //'<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="Cli_RadiologyOrderDetail_Main0"  ><i class="fa fa-times"></i></a></div> ' +
    //'<div id="Cli_RadiologyOrderDetail_0"><ul class="list-unstyled"><li> No Radiology Order Found</li></ul></div></section>';
    //        Clinical_LabOrder.updateMedicationsHtml(htmlForNoMedication, '0', noteHTMLCtrl);
    //        Clinical_ProgressNote.saveComponentSOAPText();
    //    }
    //},
    detachMedicationsFromNotes_DBCall: function (medicationsId) {
        var objData = new Object();
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["LabOrderIDs"] = medicationsId;
        objData["commandType"] = "detach_orders_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "LabOrder", "LabOrder");
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Searches Lab Orders
    //Params: LabOrderData, LabOrderId, PageNumber, RowsPerPage
    searchLabOrder: function (LabOrderData, LabOrderId, PageNumber, RowsPerPage, OrderStatus) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = {};
        if (LabOrderData != null) {
            objData = JSON.parse(LabOrderData);
        }
        objData["LabOrderId"] = LabOrderId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PatientId"] = Clinical_LabOrder.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : Clinical_LabOrder.patientId;
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;

        if (OrderStatus == "Transmitted") {
            if ($('#' + Clinical_LabOrder.params.PanelID + ' div[id*="PatientLabOrderSent"] #txtCPTCode').val() == undefined || $('#' + Clinical_LabOrder.params.PanelID + ' div[id*="PatientLabOrderSent"] #txtCPTCode').val() == "" || $('#' + Clinical_LabOrder.params.PanelID + ' div[id*="PatientLabOrderSent"] #txtCPTCode').val().toLowerCase() == "no record found")
                objData["Test"] = "";
            else
                objData["Test"] = objData["CPTCodeId"];

            objData["LabId"] = $('#' + Clinical_LabOrder.params.PanelID + ' div[id*="PatientLabOrderSent"] #frmClinicalLabOrder #ddlLaboratory').val();
            objData["ProviderId"] = $('#' + Clinical_LabOrder.params.PanelID + ' div[id*="PatientLabOrderSent"]  #frmClinicalLabOrder #ddlProvider').val();
            objData["OrderFromDate"] = $('#' + Clinical_LabOrder.params.PanelID + ' div[id*="PatientLabOrderSent"]  #frmClinicalLabOrder #dpStartDate').val();
            objData["OrderToDate"] = $('#' + Clinical_LabOrder.params.PanelID + ' div[id*="PatientLabOrderSent"]  #frmClinicalLabOrder #dpToDate').val();
            objData["OrderNo"] = $('#' + Clinical_LabOrder.params.PanelID + ' div[id*="PatientLabOrderSent"]  #frmClinicalLabOrder #txtOrderNumber').val();
        } else {
            objData["Test"] = $('#' + Clinical_LabOrder.params.PanelID + ' div[id*="PatientLabOrderPending"] #frmClinicalLabOrder #txtCPTCode').val();
            objData["LabId"] = $('#' + Clinical_LabOrder.params.PanelID + ' div[id*="PatientLabOrderPending"] #frmClinicalLabOrder #ddlLaboratory').val();
            objData["ProviderId"] = $('#' + Clinical_LabOrder.params.PanelID + ' div[id*="PatientLabOrderPending"] #frmClinicalLabOrder #ddlProvider').val();
            objData["OrderFromDate"] = $('#' + Clinical_LabOrder.params.PanelID + ' div[id*="PatientLabOrderPending"] #frmClinicalLabOrder #dpStartDate').val();
            objData["OrderToDate"] = $('#' + Clinical_LabOrder.params.PanelID + ' div[id*="PatientLabOrderPending"]  #frmClinicalLabOrder #dpToDate').val();
            objData["OrderNo"] = $('#' + Clinical_LabOrder.params.PanelID + ' div[id*="PatientLabOrderPending"]  #frmClinicalLabOrder #txtOrderNumber').val();
        }



        if (OrderStatus != null) {
            if (OrderStatus == "Pending") {
                objData["Status"] = "Pending";
            }
            else {
                objData["Status"] = "Transmitted";
            }
        }
        else {
            objData["Status"] = "Pending";//$('#' + Clinical_LabOrder.params.PanelID + ' #frmClinicalLabOrder #ddlStatus option:selected').val();
        }





        // objData["commandType"] = "search_Laborders";
        objData["commandType"] = "search_laborders_dashboard";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabOrder", "LabOrder");
    },

    searchLabResults: function (LabResultData, LabResultId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = {};
        if (LabResultData != null) {
            objData = JSON.parse(LabResultData);
        }
        objData["LabResultId"] = LabResultId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PatientId"] = Clinical_LabOrder.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : Clinical_LabOrder.patientId;
        objData["Test"] = objData["ResultCPTCode"];
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["ProviderId"] = $('#' + Clinical_LabOrder.params.PanelID + ' #frmClinicalLabResult #ddlProvider').val();
        objData["LabId"] = $('#' + Clinical_LabOrder.params.PanelID + ' #frmClinicalLabResult #ddlLaboratory').val();
        objData["OrderFromDate"] = $('#' + Clinical_LabOrder.params.PanelID + ' #frmClinicalLabResult #dpStartDate').val();
        objData["Status"] = $('#' + Clinical_LabOrder.params.PanelID + ' #frmClinicalLabResult #ddlStatus option:selected').val();
        objData["OrderToDate"] = $('#' + Clinical_LabOrder.params.PanelID + ' #frmClinicalLabResult #dpToDate').val();
        objData["OrderNo"] = $('#' + Clinical_LabOrder.params.PanelID + ' #frmClinicalLabResult #txtOrderNumber').val();
        objData["IsReviewed"] = 0;
        objData["IsAllResult"] = 1;
        //objData["commandType"] = "search_LabResults";

        objData["commandType"] = "search_labresults_dashboard"
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Lab Order Reset
    LabOrderReset: function () {

        utility.myConfirm('22', function () {
            $('#' + Clinical_LabOrder.params.PanelID + ' #frmClinicalLabOrder').resetAllControls(null);
            Clinical_LabOrder.ValidateLab();
        }, function () { },
            '22'
        );
    },

    //Author: Muhamamd Arshad
    //Date :  29-04-2016
    //Description: Lab Result Reset
    LabResultReset: function () {

        utility.myConfirm('22', function () {
            $('#' + Clinical_LabOrder.params.PanelID + ' #frmClinicalLabResult').resetAllControls(null);
            Clinical_LabOrder.ValidateLab();
        }, function () { },
            '22'
        );
    },
    //Author: Ahmad Raza
    //Date :  21-06-2016
    //Function Name: validateSpecialCharacters
    //Description: This function will validate the special characters
    validateSpecialCharacters: function (event) {
        var valid = (event.which == 32 || event.which >= 48 && event.which <= 57) || (event.which >= 65 && event.which <= 90) || (event.which >= 97 && event.which <= 122);
        if (!valid) {
            event.preventDefault();
        }

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Unload LabOrder
    UnLoadTab: function () {
        var parentPanelId = null;
        var objDeffered = $.Deferred();
        /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        if (Clinical_LabOrder.params.ParentCtrl == "clinicalTabFaceSheet" || Clinical_LabOrder.params.ParentCtrl == "Clinical_FaceSheet") {

            utility.UnLoadDialog(Clinical_LabOrder.params.PanelID + ' #frmClinicalCDS', function () {


                //$("#" + Clinical_LabOrder.params.PanelID + " #" + Clinical_LabOrder.orderSearchGridId).attr('id', 'dgvLabOrder')
                //$("#" + Clinical_LabOrder.params.PanelID + " #" + Clinical_LabOrder.resultSearchGridId).attr('id', 'dgvLabResult')

                if (Clinical_LabOrder.params["FromAdmin"] == "0") {
                    if (Clinical_LabOrder.params != null && Clinical_LabOrder.params.ParentCtrl != null) {
                        if (Clinical_LabOrder.params.ParentCtrl == "Clinical_FaceSheet") {
                            parentPanelId = GetTab(Clinical_FaceSheet.params["ParentCtrl"]).PanelID;
                            Clinical_FaceSheet.params.ChildPanelID = null;
                            UnloadActionPan(Clinical_LabOrder.params.ParentCtrl, 'Clinical_LabOrder', null, parentPanelId);
                        } else {
                            UnloadActionPan(Clinical_LabOrder.params.ParentCtrl, 'Clinical_LabOrder');
                        }
                    }
                    else
                        UnloadActionPan(null, 'Clinical_LabOrder');
                }
                else {
                    $("#mstrDivMedical #clinicalMenu_Medical_CDS").remove();
                    RemoveAdminTab();
                }
                objDeffered.resolve();
            }, function () {
                if (Clinical_LabOrder.params["FromAdmin"] == "0") {
                    if (Clinical_LabOrder.params != null && Clinical_LabOrder.params.ParentCtrl != null) {
                        UnloadActionPan(Clinical_LabOrder.params.ParentCtrl, 'Clinical_LabOrder');
                    }
                    else
                        UnloadActionPan(null, 'Clinical_LabOrder');
                }
                else {
                    $("#mstrDivMedical #clinicalMenu_Medical_CDS").remove();
                    RemoveAdminTab();
                }
                objDeffered.resolve();
            });
            Clinical_FaceSheet.loadFaceSheet();
        }
            /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        else {
            if (Clinical_LabOrder.params["FromAdmin"] == "0") {
                if (Clinical_LabOrder.params != null && Clinical_LabOrder.params.ParentCtrl != null) {
                    if ($("#" + Clinical_LabOrder.params.PanelID + " #PatientlistLabOrderPending").hasClass('active') || $("#" + Clinical_LabOrder.params.PanelID + " #PatientlistLabOrderSent").hasClass('active')) {
                        Clinical_LabOrder.addOrdersToNotes();
                        Clinical_LabOrder.addResultsToNotes();
                    }
                    if ($("#" + Clinical_LabOrder.params.PanelID + " #PatientlistLabResult").hasClass('active')) {
                        Clinical_LabOrder.addResultsToNotes();
                        Clinical_LabOrder.addOrdersToNotes();
                    }
                    UnloadActionPan(Clinical_LabOrder.params.ParentCtrl, 'Clinical_LabOrder');
                }
                else
                    Clinical_LabOrder.addResultsToNotes();
                if (Clinical_LabOrder.params.ParentCtrl != "Clinical_FaceSheet") {
                    UnloadActionPan(null, 'Clinical_LabOrder');
                    $('.modal-backdrop').removeClass('in');
                    $('.modal-backdrop').addClass('out');
                    $('.modal-backdrop').hide();
                    setTimeout(function () {
                        if ($(document.body).hasClass('modal-open')) {
                            $(document.body).removeClass('modal-open');
                        }
                    }, 501);
                }

            }
            else {
                $("#mstrDivMedical #clinicalMenu_Medical_CDS").remove();
                RemoveAdminTab();
            }
            objDeffered.resolve();
        }
        $('.modal-backdrop').removeClass('in');
        $('.modal-backdrop').addClass('out');
        $('.modal-backdrop').hide();
        setTimeout(function () {
            if ($(document.body).hasClass('modal-open')) {
                $(document.body).removeClass('modal-open');
            }
        }, 501);
        return objDeffered;
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Opens Provider
    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmClinicalLabOrder";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "clinicalTabLabOrder";
        LoadActionPan('Admin_Provider', params);
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Opens Provider
    openproviderdetail: function () {
        var params = [];
        params["providerid"] = $('#pnlClinicalLabOrder #hfprovider').val();
        params["mode"] = "edit";
        params["fromadmin"] = "0";
        params["refctrl"] = "ddlProvider";
        params["parentctrl"] = 'Clinical_LabOrder';
        loadactionpan('providerdetail', params);
    },


    labOrdersDelete: function (TabID) {
        var strMessage = "";

        var labOrderIDs = "";


        if ($("#pnlDashboard  #dgvLabOrderGrid #dgvLabOrderDashboard tbody tr input:checked").length == 0) {
            utility.DisplayMessages('Please select any order to delete', 4);
            return;
        } else {
            $("#pnlDashboard  #dgvLabOrderGrid #dgvLabOrderDashboard tbody tr input:checked").each(function (i, item) {

                labOrderIDs += "," + $(item).attr('id');
            });
        }

        var confirmType = '1';
        //if (IsTransmittedOrder == true) {
        //    confirmType = '41';
        //}
        //else {
        //    confirmType = '1';
        //}
        AppPrivileges.GetFormPrivileges("Orders and Results_Lab Order", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm(confirmType, function () {
                    var selectedValue = labOrderIDs;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        var self = $("#" + Clinical_LabOrder.params.PanelID + " form");
                        var myJSON = self.getMyJSONByName();
                        Clinical_LabOrder.deleteLabOrder(myJSON, labOrderIDs).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                if (response.Message == "Successfully Deleted") {
                                    utility.DisplayMessages(response.Message, 1);
                                } else {
                                    utility.DisplayMessages(response.Message, 3);
                                }

                                if (TabID == "mstrTabDashBoard") {
                                    var pageNo = $("#pnlDashboard #dgvLabOrderDashboard_Paging ul li.active a").html();
                                    DashBoard.DashBoardLabOrderGridLoad(null, pageNo, null);

                                    //var btnDashBoardLabOrderSearch = $("#pnlDashboard #btnLabSearch").trigger("click");
                                    //var btnDashBoardLabResultSearch = $("#pnlDashboard #btnLabResultSearch").trigger("click");
                                    //var btnDashBoardLabResultUnsolicitedSearch = $("#pnlDashboard #btnLabUnsoResultSearch");
                                    //var onclickLabOrder = btnDashBoardLabOrderSearch.attr("onclick");
                                    //var onclickLabResult = btnDashBoardLabResultSearch.attr("onclick");
                                    //var onclickLabResultUnsolicited = btnDashBoardLabResultUnsolicitedSearch.attr("onclick");
                                    //DashBoard.DashBoardLabOrderGridLoad(null, 1, 15);
                                }
                                else {
                                    Clinical_LabOrder.LabOrderSearch("", 1, 15);
                                    Clinical_LabOrder.LabResultsSearch("", 1, 15);
                                }

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
    LabOrderDelete: function (LabId, event, PageNo, rpp, PatientId, TabID, IsTransmittedOrder) {
        var LabId = "";


        if ($("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrdertSent tbody tr input:checked").length == 0) {
            utility.DisplayMessages('Please select any order to delete', 4);
            return;
        } else {
            $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrdertSent tbody tr input:checked").each(function (i, item) {

                LabId += "," + $(item).attr('id');
            });
        }

        AppPrivileges.GetFormPrivileges("Orders and Results_Lab Order", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = LabId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        var self = $("#" + Clinical_LabOrder.params.PanelID + " form");
                        var myJSON = self.getMyJSONByName();
                        Clinical_LabOrder.deleteLabOrder(myJSON, LabId).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                if (response.Message == "Successfully Deleted") {
                                    utility.DisplayMessages(response.Message, 1);
                                } else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                                if (TabID == "mstrTabDashBoard") {
                                    var pageNo = $("#pnlDashboard #dgvLabOrderDashboard_Paging ul li.active a").html();
                                    DashBoard.DashBoardLabOrderGridLoad(null, pageNo, null);

                                    //var btnDashBoardLabOrderSearch = $("#pnlDashboard #btnLabSearch").trigger("click");
                                    //var btnDashBoardLabResultSearch = $("#pnlDashboard #btnLabResultSearch").trigger("click");
                                    //var btnDashBoardLabResultUnsolicitedSearch = $("#pnlDashboard #btnLabUnsoResultSearch");
                                    //var onclickLabOrder = btnDashBoardLabOrderSearch.attr("onclick");
                                    //var onclickLabResult = btnDashBoardLabResultSearch.attr("onclick");
                                    //var onclickLabResultUnsolicited = btnDashBoardLabResultUnsolicitedSearch.attr("onclick");
                                    //DashBoard.DashBoardLabOrderGridLoad(null, 1, 15);
                                }
                                else {
                                    Clinical_LabOrder.LabOrderSearch("", 1, 15);
                                    Clinical_LabOrder.LabResultsSearch("", 1, 15);
                                }

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

    PendingLabOrderDelete: function (LabId, event, PageNo, rpp, PatientId, TabID, IsTransmittedOrder) {
        var LabId = "";


        if ($("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrder tbody tr input:checked").length == 0) {
            utility.DisplayMessages('Please select any order to delete', 4);
            return;
        } else {
            $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrder tbody tr input:checked").each(function (i, item) {

                LabId += "," + $(item).attr('id');
            });
        }

        AppPrivileges.GetFormPrivileges("Orders and Results_Lab Order", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = LabId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        var self = $("#" + Clinical_LabOrder.params.PanelID + " form");
                        var myJSON = self.getMyJSONByName();
                        Clinical_LabOrder.deleteLabOrder(myJSON, LabId).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                if (response.Message == "Successfully Deleted") {
                                    utility.DisplayMessages(response.Message, 1);
                                } else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                                if (TabID == "mstrTabDashBoard") {
                                    var pageNo = $("#pnlDashboard #dgvLabOrderDashboard_Paging ul li.active a").html();
                                    DashBoard.DashBoardLabOrderGridLoad(null, pageNo, null);

                                    //var btnDashBoardLabOrderSearch = $("#pnlDashboard #btnLabSearch").trigger("click");
                                    //var btnDashBoardLabResultSearch = $("#pnlDashboard #btnLabResultSearch").trigger("click");
                                    //var btnDashBoardLabResultUnsolicitedSearch = $("#pnlDashboard #btnLabUnsoResultSearch");
                                    //var onclickLabOrder = btnDashBoardLabOrderSearch.attr("onclick");
                                    //var onclickLabResult = btnDashBoardLabResultSearch.attr("onclick");
                                    //var onclickLabResultUnsolicited = btnDashBoardLabResultUnsolicitedSearch.attr("onclick");
                                    //DashBoard.DashBoardLabOrderGridLoad(null, 1, 15);
                                }
                                else {
                                    Clinical_LabOrder.LabOrderSearch("", 1, 15);
                                    Clinical_LabOrder.LabResultsSearch("", 1, 15);
                                }

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
    // Method Name:LabResultDelete
    //Author: Ahmad Raza
    //Date :  27-04-2016
    //Description: to Delete the selected Lab Result
    LabResultDelete: function () {
        var strMessage = "";
        var LabId = "";


        if ($("#" + Clinical_LabOrder.params.PanelID + " #pnlLabResult_Result #" + Clinical_LabOrder.resultSearchGridId + " tbody tr input:checked").length == 0) {
            utility.DisplayMessages('Please select any result to delete', 4);
            return;
        } else {
            $("#" + Clinical_LabOrder.params.PanelID + " #pnlLabResult_Result #" + Clinical_LabOrder.resultSearchGridId + " tbody tr input:checked").each(function (i, item) {

                LabId += "," + $(item).attr('id');
            });
        }
        AppPrivileges.GetFormPrivileges("Orders and Results_Lab Result", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = LabId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        var self = $("#" + Clinical_LabOrder.params.PanelID + " form");
                        var myJSON = self.getMyJSONByName();

                        Clinical_LabOrder.deleteLabResult(myJSON, LabId).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                if (response.Message == "Successfully Deleted") {
                                    utility.DisplayMessages(response.Message, 1);
                                } else {
                                    utility.DisplayMessages(response.Message, 3);
                                }

                                Clinical_LabOrder.LabOrderSearch();
                                Clinical_LabOrder.LabResultsSearch();

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

    LabResultsDelete: function (TabID, IsUnsolicited) {
        var strMessage = "";
        var LabId = "";


        if ($("#pnlDashboard  #LabResult #dgvLabOrderResult tbody tr .chklabResult:checkbox:checked").length == 0) {
            utility.DisplayMessages('Please select any result to delete', 4);
            return;
        } else {
            $("#pnlDashboard  #LabResult #dgvLabOrderResult tbody tr .chklabResult:checkbox:checked").each(function (i, item) {

                LabId += "," + $(item).attr('id');
            });
        }
        AppPrivileges.GetFormPrivileges("Orders and Results_Lab Result", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = LabId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        var self = $("#" + Clinical_LabOrder.params.PanelID + " form");
                        var myJSON = self.getMyJSONByName();

                        Clinical_LabOrder.deleteLabResult(myJSON, LabId).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                if (response.Message == "Successfully Deleted") {
                                    utility.DisplayMessages(response.Message, 1);
                                } else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                                if (TabID == "mstrTabDashBoard") {
                                    var spnLabOrderCount = $("#pnlDashboard #spnDashboard_LabOrderCount");
                                    var spnLabOrderOverallCount = $("#pnlDashboard #spnOrdersResults");
                                    var totalCount = parseInt(spnLabOrderCount.html());
                                    var MainCount = parseInt(spnLabOrderOverallCount.html());
                                    spnLabOrderCount.html(totalCount - 1);
                                    spnLabOrderOverallCount.html(MainCount - 1);
                                    if (IsUnsolicited == true) {
                                        var btnDashBoardLabResultUnsolicitedSearch = $("#pnlDashboard #btnLabUnsoResultSearch").trigger("click");
                                    }
                                    else {
                                        var PageNo = $("#pnlDashboard #dgvLabResultDashboard_Paging ul li.active a").html();
                                        DashBoard.DashBoardLabResultGridLoad(null, null, PageNo);
                                        //var btnDashBoardLabResultSearch = $("#pnlDashboard #btnLabResultSearch").trigger("click");
                                    }

                                    //var btnDashBoardLabResultUnsolicitedSearch = $("#pnlDashboard #btnLabUnsoResultSearch");
                                }
                                else {
                                    Clinical_LabOrder.LabOrderSearch();
                                    Clinical_LabOrder.LabResultsSearch();
                                }
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

    UnsolicitedLabResultsDelete: function (TabID, IsUnsolicited) {
        var strMessage = "";
        var LabId = "";


        if ($("#pnlDashboard  #LabUnsolicited #dgvLabUnsoOrderResult tbody tr input:checked").length == 0) {
            utility.DisplayMessages('Please select any result to delete', 4);
            return;
        } else {
            $("#pnlDashboard  #LabUnsolicited #dgvLabUnsoOrderResult tbody tr input:checked").each(function (i, item) {

                LabId += "," + $(item).attr('id');
            });
        }
        AppPrivileges.GetFormPrivileges("Orders and Results_Lab Result", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = LabId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        var self = $("#" + Clinical_LabOrder.params.PanelID + " form");
                        var myJSON = self.getMyJSONByName();

                        Clinical_LabOrder.deleteLabResult(myJSON, LabId).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                if (response.Message == "Successfully Deleted") {
                                    utility.DisplayMessages(response.Message, 1);
                                } else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                                if (TabID == "mstrTabDashBoard") {
                                    var spnLabOrderCount = $("#pnlDashboard #spnDashboard_LabOrderCount");
                                    var spnLabOrderOverallCount = $("#pnlDashboard #spnOrdersResults");
                                    var totalCount = parseInt(spnLabOrderCount.html());
                                    var MainCount = parseInt(spnLabOrderOverallCount.html());
                                    spnLabOrderCount.html(totalCount - 1);
                                    spnLabOrderOverallCount.html(MainCount - 1);
                                    if (IsUnsolicited == true) {
                                        var btnDashBoardLabResultUnsolicitedSearch = $("#pnlDashboard #btnLabUnsoResultSearch").trigger("click");
                                    }
                                    else {
                                        var PageNo = $("#pnlDashboard #dgvLabResultDashboard_Paging ul li.active a").html();
                                        DashBoard.DashBoardLabResultGridLoad(null, null, PageNo);
                                        //var btnDashBoardLabResultSearch = $("#pnlDashboard #btnLabResultSearch").trigger("click");
                                    }

                                    //var btnDashBoardLabResultUnsolicitedSearch = $("#pnlDashboard #btnLabUnsoResultSearch");
                                }
                                else {
                                    Clinical_LabOrder.LabOrderSearch();
                                    Clinical_LabOrder.LabResultsSearch();
                                }
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

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: DB Call to Delete the selected Lab Order
    deleteLabOrder: function (LabData, LabId, PageNumber, RowsPerPage, PatientId) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = JSON.parse(LabData);
        objData["LabOrderId"] = LabId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        var currentPatientId = PatientId != null && parseInt(PatientId) > 0 ? PatientId : $('#PatientProfile #hfPatientId').val();
        objData["PatientId"] = currentPatientId;
        objData["commandType"] = "delete_Laborder";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabOrder", "LabOrder");

    },

    // Method Name:deleteLabResult
    // Author: Ahmad Raza
    // Date :  27-04-2016
    // Description: DBCall to Delete the selected Lab Result
    deleteLabResult: function (LabData, LabId, PageNumber, RowsPerPage, PatientId) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = JSON.parse(LabData);
        objData["LabResultId"] = LabId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        var currentPatientId = PatientId != null && parseInt(PatientId) > 0 ? PatientId : $('#PatientProfile #hfPatientId').val();
        objData["PatientId"] = currentPatientId;
        objData["commandType"] = "delete_Labresult";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabResult", "LabResult");

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Previews Lab Order
    printLabOrder: function (LabId, status, event, PatientId) {
        if (event != null) {
            event.stopPropagation();
        }
        if (status == 'Transmitted') {
            var params = [];
            params["FromAdmin"] = "0";
            params["UserId"] = globalAppdata['AppUserId'];
            var currentPatientId = PatientId != null && parseInt(PatientId) > 0 ? PatientId : $('#PatientProfile #hfPatientId').val();
            params["PatientId"] = currentPatientId;
            // params["ParentCtrl"] = Clinical_LabOrder.params.TabID;
            params["ParentCtrl"] = (Clinical_LabOrder.params.ParentCtrl != "clinicalTabProgressNote" && Clinical_LabOrder.params.ParentCtrl != "clinicalTabFaceSheet" && Clinical_LabOrder.params.ParentCtrl != "Clinical_FaceSheet" && Clinical_LabOrder.params.ParentCtrl != null) ? Clinical_LabOrder.params.TabID : "Clinical_LabOrder";
            if ($('#mstrTabDashBoard').hasClass('active')) {
                params["ParentCtrl"] = undefined;
                Clinical_LabOrder.params["ParentCtrl"] = undefined;
            }
            params["LabOrderId"] = LabId;

            utility.myConfirm('Would you like to print the Specimen Label for this order?', function () {
                if (Clinical_LabOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
                    params["ParentCtrlPanelID"] = Clinical_LabOrder.params.PanelID;
                    params["BarCodeHtml"] = 'true';
                    params["IsSpecimen"] = true;
                    Clinical_LabOrder.printLabOrderDir(LabId, true, event, currentPatientId);

                } else {
                    params["BarCodeHtml"] = 'true';
                    params["IsSpecimen"] = true;
                    Clinical_LabOrder.printLabOrderDir(LabId, true, event, currentPatientId);
                }



            }, function () {
                if (Clinical_LabOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
                    params["BarCodeHtml"] = 'true'; // IMP-481 // MK
                    params["ParentCtrlPanelID"] = Clinical_LabOrder.params.PanelID;
                    Clinical_LabOrder.printLabOrderDir(LabId, false, event, currentPatientId);
                } else {
                    params["BarCodeHtml"] = 'true'; // IMP-481 // MK
                    Clinical_LabOrder.printLabOrderDir(LabId, false, event, currentPatientId);
                }

            },
                       'Specimen Label Printing');
        }




    },
    // Method Name:printLabResult
    // Author: Ahmad Raza
    // Date :  27-04-2016
    // Description: to show selected Lab Result in PDF form
    printLabResult: function (LabId, ResultId, status, event, PatientId) {
        var comeFrom = "";
        if (event != null) {
            event.stopPropagation();
            if ($(event.target).closest('#widgetgridpanel').length > 0) {
                comeFrom = "pnlDashboard";
            }
        }

        // if (status == 'Transmitted') {
        //var params = [];
        //params["FromAdmin"] = "0";
        //params["UserId"] = globalAppdata['AppUserId'];
        //var currentPatientId = PatientId != null && parseInt(PatientId) > 0 ? PatientId : $('#PatientProfile #hfPatientId').val();
        //params["PatientId"] = currentPatientId;
        //// params["ParentCtrl"] = Clinical_LabOrder.params.TabID;
        //params["ParentCtrl"] = (Clinical_LabOrder.params.ParentCtrl != "clinicalTabProgressNote" && Clinical_LabOrder.params.ParentCtrl != "clinicalTabFaceSheet") ? Clinical_LabOrder.params.TabID : "Clinical_LabOrder";
        //params["LabOrderId"] = LabId;
        //params["LabResultId"] = ResultId;
        //LoadActionPan('Clinical_LabResultView', params);


        //utility.myConfirm('Would you like to print the Specimen Label for this result?', function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["UserId"] = globalAppdata['AppUserId'];
        var currentPatientId = PatientId != null && parseInt(PatientId) > 0 ? PatientId : $('#PatientProfile #hfPatientId').val();
        params["PatientId"] = currentPatientId;

        params["ParentCtrl"] = (Clinical_LabOrder.params.ParentCtrl != "clinicalTabProgressNote" && Clinical_LabOrder.params.ParentCtrl != "clinicalTabFaceSheet" && Clinical_LabOrder.params.ParentCtrl != "Clinical_FaceSheet") ? Clinical_LabOrder.params.TabID : "Clinical_LabOrder";

        var PanelID = "";
        if (comeFrom == "") {
            if (Clinical_LabOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                PanelID = 'pnlClinicalProgressNote #pnlClinicalLabOrder';
                params["PanelID"] = 'pnlClinicalProgressNote #pnlClinicalLabOrder';
                params["PrPanelID"] = 'pnlClinicalProgressNote #pnlClinicalLabOrder';
            }
            else if (Clinical_LabOrder.params.ParentCtrl == "clinicalTabFaceSheet" || Clinical_LabOrder.params.ParentCtrl == "Clinical_FaceSheet") {
                PanelID = 'pnlClinicalFaceSheet #pnlClinicalLabOrder';
                params["PanelID"] = 'pnlClinicalFaceSheet #pnlClinicalLabOrder';
                params["PrPanelID"] = 'pnlClinicalFaceSheet #pnlClinicalLabOrder';
            }
            else {
                PanelID = 'pnlClinicalLabOrder';
                params["PanelID"] = 'pnlClinicalLabOrder';
                params["PrPanelID"] = 'pnlClinicalLabOrder';
            }
        }
        else {

            PanelID = comeFrom;
            params["PanelID"] = comeFrom;
            params["PrPanelID"] = comeFrom;
            params["ParentCtrl"] = 'mstrTabDashBoard';
        }


        var LabResultId = ResultId;
        var LabOrderId = LabId;
        params["BarCodeHtml"] = 'false';
        params["LabResultId"] = LabResultId;
        params["LabOrderId"] = LabOrderId;
        LoadActionPan('Clinical_LabResultView', params, PanelID);
        //}, function () {
        //    var params = [];
        //    params["FromAdmin"] = "0";
        //    params["UserId"] = globalAppdata['AppUserId'];
        //    var currentPatientId = PatientId != null && parseInt(PatientId) > 0 ? PatientId : $('#PatientProfile #hfPatientId').val();
        //    params["PatientId"] = currentPatientId;
        //    params["ParentCtrl"] = (Clinical_LabOrder.params.ParentCtrl != "clinicalTabProgressNote" && Clinical_LabOrder.params.ParentCtrl != "clinicalTabFaceSheet") ? Clinical_LabOrder.params.TabID : "Clinical_LabOrder";
        //    //if (Clinical_LabOrder.params.ParentCtrl == "clinicalTabProgressNote") {
        //    //    params["ParentCtrl"] = "clinicalTabProgressNote"
        //    //}

        //    var PanelID = "";
        //    if (Clinical_LabOrder.params.ParentCtrl == "clinicalTabProgressNote") {
        //        PanelID = 'pnlClinicalProgressNote #pnlClinicalLabOrder';
        //        params["PanelID"] = 'pnlClinicalProgressNote #pnlClinicalLabOrder';
        //        params["PrPanelID"] = 'pnlClinicalProgressNote #pnlClinicalLabOrder';
        //    }
        //    else if (Clinical_LabOrder.params.ParentCtrl == "clinicalTabFaceSheet") {
        //        PanelID = 'pnlClinicalFaceSheet #pnlClinicalLabOrder';
        //        params["PanelID"] = 'pnlClinicalFaceSheet #pnlClinicalLabOrder';
        //        params["PrPanelID"] = 'pnlClinicalFaceSheet #pnlClinicalLabOrder';
        //    }
        //    else {
        //        PanelID = 'pnlClinicalLabOrder';
        //        params["PanelID"] = 'pnlClinicalLabOrder';
        //        params["PrPanelID"] = 'pnlClinicalLabOrder';
        //    }

        //    var LabResultId = ResultId;
        //    var LabOrderId = LabId;
        //    params["BarCodeHtml"] = 'false';
        //    params["LabResultId"] = LabResultId;
        //    params["LabOrderId"] = LabOrderId;
        //    LoadActionPan('Clinical_LabResultView', params, PanelID);
        //},
        //             'Specimen Label Printing');


        //   }

    },

    // External PDF (VitalAxis)
    printLabResultExternalPDF: function (LabOrderResultExternalPDFId) {
        var params = [];
        params["IsExternalPDF"] = true;
        params["LabOrderResultExternalPDFId"] = LabOrderResultExternalPDFId;
        params["ParentCtrl"] = (Clinical_LabOrder.params.ParentCtrl != "clinicalTabProgressNote" && Clinical_LabOrder.params.ParentCtrl != "clinicalTabFaceSheet" && Clinical_LabOrder.params.ParentCtrl != "Clinical_FaceSheet") ? Clinical_LabOrder.params.TabID : "Clinical_LabOrder";
        params["FromAdmin"] = "0";
        params["UserId"] = globalAppdata['AppUserId'];
        params["ParentCtrl"] = 'mstrTabDashBoard';

        LoadActionPan('Clinical_LabResultView', params);
    },


    //Function Name: showOrderHistory
    //Author Name: Ahmad Raza
    //Created Date: 20-04-2016
    //Description: to show Lab order history
    showOrderHistory: function (labOrderId, event) {
        if (event != null) {
            event.stopPropagation();
        }

        var ParentCtrl = 'clinicalTabLabOrder';
        var ParentCtrlPanelID = null;
        if (Clinical_LabOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
            ParentCtrl = 'Clinical_LabOrder';
            ParentCtrlPanelID = Clinical_LabOrder.params.PanelID;
        }
        else if (Clinical_LabOrder.params["ParentCtrl"] == "clinicalTabFaceSheet" || Clinical_LabOrder.params["ParentCtrl"] == "Clinical_FaceSheet") {
            ParentCtrl = 'Clinical_LabOrder';
            ParentCtrlPanelID = Clinical_LabOrder.params.PanelID;
        }

        EMRUtility.showCurrentItemHistory(Clinical_LabOrder.params.PanelID, null, null, "LabOrder,LabOrderTest,LabOrderProblem", null, ParentCtrl, labOrderId, ParentCtrlPanelID);
    },


    //-----------Result attach.detach


    enableAddResult: function (obj, event) {
        if (event != null) {
            event.stopPropagation();
        }

        var totalRows = $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrdertSent tr").length;
        totalRows -= 1;
        var selectedRows = $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrdertSent tbody tr input:checked").length;

        if (totalRows == selectedRows) {
            $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrdertSent #selectRecordOrders").prop("checked", true);
        }
        else {
            $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrdertSent #selectRecordOrders").prop("checked", false);
        }

        if ($(obj).is(':checked')) {
            if ($.inArray(obj.id + 'rslt', Clinical_ProgressNote.AttachedNoteComponentIds) == -1) {
                if (obj.id != 'chkSentToPortal') {
                    Clinical_ProgressNote.AttachedNoteComponentIds.push(obj.id + 'rslt');
                }
            } if ($.inArray(obj.id + 'rslt', Clinical_ProgressNote.DetachedNoteComponentIds) != -1) {
                var index = Clinical_ProgressNote.DetachedNoteComponentIds.indexOf(obj.id + 'rslt');
                if (index > -1) {
                    Clinical_ProgressNote.DetachedNoteComponentIds.splice(index, 1);
                }
            }

        } else {

            var index = Clinical_ProgressNote.AttachedNoteComponentIds.indexOf(obj.id + 'rslt');
            if (index > -1) {
                Clinical_ProgressNote.AttachedNoteComponentIds.splice(index, 1);
            }
            if ($.inArray(obj.id + 'rslt', Clinical_ProgressNote.DetachedNoteComponentIds) == -1) {
                if (obj.id != 'chkSentToPortal') {
                    Clinical_ProgressNote.DetachedNoteComponentIds.push(obj.id + 'rslt');
                }
            }
        }
    },

    addResultsToNotes: function (selectedAttachedOrders, selectedDetachedOrders, hideAlertMessage) {
        var noteHTMLCtrl = '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML';
        if ($("#" + Clinical_LabOrder.params.PanelID + " #dgvLabResult").dataTable().fnSettings().aoData.length == 0) {
            //   Clinical_LabOrder.noActiveLabOrderSoapText();
        }
        else {

            if ($("#" + Clinical_LabOrder.params.PanelID + " #selectRecordOrders").prop('checked') == true || $("#" + Clinical_LabOrder.params.PanelID + " #selectRecordOrders").prop('checked') == false) {

                $("#" + Clinical_LabOrder.params.PanelID + " #" + Clinical_LabOrder.resultSearchGridId + " tbody").find('input[type="checkbox"]').each(function () {
                    Clinical_LabOrder.enableAddResult(this);
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
                //Check for Medications Values
                //if (AttachSelectedOrdersAndResults.join().indexOf("prsc") != -1 || DettachSelectedOrdersAndResults.join().indexOf("prsc") != -1) {
                //    var AttachSelectedResults = EMRUtility.slicefunc(AttachSelectedOrdersAndResults, "prsc", 0, -4);
                //    var dettachSelectedResults = EMRUtility.slicefunc(DettachSelectedOrdersAndResults, "prsc", 0, -4);
                //    Clinical_Prescriptions.addPrescriptionsToNotes(AttachSelectedResults, dettachSelectedResults);
                //}
                AttachedSelectedOrders = EMRUtility.slicefunc(AttachSelectedOrdersAndResults, "rslt", 0, -4);
                DettachedSelectedOrders = EMRUtility.slicefunc(DettachSelectedOrdersAndResults, "rslt", 0, -4);
            }

            if (AttachedSelectedOrders != null && AttachedSelectedOrders != '') {
                for (var i = 0; i < Clinical_ProgressNote.AttachedNoteComponentIds.length; i++) {
                    var ALid = Clinical_ProgressNote.AttachedNoteComponentIds[i];
                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_LabOrderDetail_Main' + ALid).length != 0) {
                        var index = AttachedSelectedOrders.indexOf(ALid);
                        if (index > -1) {
                            AttachedSelectedOrders.splice(index, 1);
                        }
                    }
                }
            }

            var detachedvalues = DettachedSelectedOrders;
            if (detachedvalues.join() != null && detachedvalues.join() != '') {
                //detachedvalues.splice($.inArray("chkMarkAsReviewed", detachedvalues), 1);
                Clinical_LabOrder.detachResultFromNotesAT(detachedvalues).done(function () {
                    if (AttachedSelectedOrders.join() != null && AttachedSelectedOrders.join() != '') {
                        //AttachedSelectedOrders.splice($.inArray("chkMarkAsReviewed", AttachedSelectedOrders), 1);
                        Clinical_LabOrder.attachResultFromNotesAT(AttachedSelectedOrders);
                    } else {
                        Clinical_ProgressNote.saveComponentSOAPText("Lab Results", hideAlertMessage);
                    }
                });

            } else if (AttachedSelectedOrders.join() != null && AttachedSelectedOrders.join() != '') {
                Clinical_LabOrder.attachResultFromNotesAT(AttachedSelectedOrders, hideAlertMessage);
            }
        }
        if (Clinical_LabOrder.params.ParentCtrl == "clinicalTabProgressNote") {
            if ($("#" + Clinical_LabOrder.params.PanelID + " #PatientlistLabOrderPending").hasClass('active') || $("#" + Clinical_LabOrder.params.PanelID + " #PatientlistLabOrderSent").hasClass('active')) {
                EMRUtility.scrollToPNcomponent('clinical_laborder');
                EMRUtility.scrollToPNcomponent('clinical_laborders');
            }
            if ($("#" + Clinical_LabOrder.params.PanelID + " #PatientlistLabResult").hasClass('active')) {
                EMRUtility.scrollToPNcomponent('clinical_labresults');
                EMRUtility.scrollToPNcomponent('clinical_labresult');
            }
            UnloadActionPan(Clinical_LabOrder.params.ParentCtrl, 'Clinical_LabOrder');
        }
    },

    attachResultFromNotesAT: function (AttachedSelectedOrders, hideAlertMessage) {
        Clinical_LabOrder.getLabResultInfo(AttachedSelectedOrders.join(), hideAlertMessage).done(function () {
            setTimeout(function () {
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                if (Clinical_LabOrder.params != null && Clinical_LabOrder.params.PanelID.indexOf('pnlClinicalLabOrder') != -1) {
                    Clinical_LabOrder.LabResultsSearch();
                }
            }, 5);
        });
    },
    detachResultFromNotesAT: function (detachedvalues) {
        var dfd = new $.Deferred();
        Clinical_LabOrder.detachLabResultFromNotesDBCall(detachedvalues.join()).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                for (var i = 0; i < detachedvalues.length; i++) {
                    var ALid = detachedvalues[i];
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_LabResultDetail_Main' + ALid).remove();
                }
            }
            else {
                // Bilal(Development Head) made me do that against my will :p
                if (response.Message != "Please click the cross sign to delete record")
                    utility.DisplayMessages(response.Message, 3);
            }
            dfd.resolve('ok');
        });
        return dfd.promise();
    },

    getLabResultInfo: function (labOrderId, hideAlertMessage) {
        if (labOrderId == null || labOrderId == '') {
            return false;
        }
        var dfd = new $.Deferred();
        Clinical_LabOrder.getResultsForSOAP_DBCall(labOrderId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.ResultSoapCount > 0) {
                        Clinical_LabOrder.createResultBodyHTML123(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', labOrderId, hideAlertMessage);
                        Clinical_ProgressNote.saveComponentSOAPText("Lab Results", true);
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
            dfd.resolve('ok');
        });
        return dfd.promise();
    },

    getResultsForSOAP_DBCall: function (labOrderId) {
        var objData = new Object();
        objData["LabResultIDs"] = labOrderId;
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["PatientId"] = Clinical_LabOrder.params.patientID;
        objData["ProviderId"] = $("#pnlClinicalProgressNote #hfProviderId").val();
        objData["commandType"] = "get_results_forsoap";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },

    createResultBodyHTML123: function (response, noteHTMLCtrl, resultId, hidemessage, bNotSaveComponent) {

        Clinical_LabOrder.checkResultExists();
        if (response.LabResultFill_JSON != null && response.LabResultFill_JSON != '') {
            var MedicationsSOAPJSON = JSON.parse(response.LabResultFill_JSON);
            var LabOrderResultParentSOAPJSON = JSON.parse(response.LabOrderResultParent_JSON);
            var LabOrderResultChildSOAPJSON = JSON.parse(response.LabOrderResultChild_JSON);

            //      var medicationReviewSoapJSON = [];
            if (response.ResultSoap_JSON != null) {
                //          medicationReviewSoapJSON = JSON.parse(response.MedicationReviewSoap_JSON);
            }

            var $mainDivMedications = $(document.createElement('div'));
            if (MedicationsSOAPJSON == null || MedicationsSOAPJSON.length == 0) {
                //if (medicationReviewSoapJSON != null || medicationReviewSoapJSON.length != 0) {
                //    $.each(medicationReviewSoapJSON, function (index, item) {
                //        if (item.ReviewedOn != null && item.ReviewedOn != '') {
                //            var dateFormat = item.ReviewedOn
                //            var ReviewedOndateFormat = $.datepicker.formatDate('MM dd, yy ', new Date(dateFormat)) + utility.RemoveDateFromDateTime(null, dateFormat);
                //            $ListMedications.append("<li>" + (item.ReviewedBy == '' ? "" : "Medications reviewed by " + item.ReviewedBy) +
                //       " on:  " + ReviewedOndateFormat + "</li>");
                //        }
                //    });
                //} else {
                //    return "";
                //}
            } else {
                if ($(noteHTMLCtrl + ' clinical_labresults').parent().parent().find('#Cli_LabResultDetail_Main0').length != 0) {
                    $(noteHTMLCtrl + ' #Cli_LabResultDetail_Main0').parent().remove();
                }
            }

            var AListId = [];
            $.each(MedicationsSOAPJSON, function (index, element) {
                var LabOrderResultSoapText = "";
                var ALid = element.LabOrderResultId;
                //             var $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(element.NDCID, 'clinicalTabProgressNote', 1);
                var $SectionBodyMedications = $(document.createElement('section'));
                $SectionBodyMedications.attr('id', "Cli_LabResultDetail_Main" + ALid);
                $SectionBodyMedications.attr('laborderid', element.LabOrderId);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_LabResultDetail_" + ALid);
                var $ListMedications = $(document.createElement('ul'));
                var duration = "";
                $ListMedications.attr('class', 'list-unstyled')

                $SectionBodyMedications.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_LabResultDetail_" + ALid + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_LabResultDetail_Main" + ALid + '"  ><i class="fa fa-times"></i></a></div> ');
                // var obj = $('<p>');
                // $(obj).html(element.SoapText);
                //$(obj).html($(obj).text())
                if (element.SoapText != "") {
                    $ListMedications.append("<li>" + element.SoapText + "</li>");
                }
                else {
                    $.each(LabOrderResultParentSOAPJSON, function (i, item) {
                        var LabOrderResultParentSoapText = "", LabOrderResultChildSoapText = "";
                        LabOrderResultParentSoapText += "<div class='table-responsive'><table class='table table-bordered table-condensed'><thead><tr><th align='left' colspan='6'>"
                                                        + (item.ShowCPTCode == 0 ? item.CPTCodeDescription : item.CPTCode + " " + item.CPTCodeDescription) + "</th></tr>"
                                                        + "<tr style='background-color:#0188CC'><th>Date & Time</th><th>Observation</th><th>Result</th><th>UoM</th><th>Flag</th><th>Range</th></tr></thead><tbody>"
                        $.each(LabOrderResultChildSOAPJSON, function (j, childItems) {
                            if (item.CPTCode == childItems.CPTCode && item.CPTCodeDescription == childItems.CPTCodeDescription) {
                                LabOrderResultChildSoapText += "<tr><td>" + childItems.ObservationDate + "</td>"
                                                               + "<td>" + childItems.LOINCDescription + "<a onclick=\"Clinical_InfoButtonView.getInfofromMediPlus('" + childItems.LOINC + "', 'clinicalTabProgressNote','3','')\" style=\"cursor:pointer\"><b>(Info)</b></a> " + "</td>"
                                                               + "<td>" + childItems.Result + "</td>"
                                                               + "<td>" + childItems.UoM + "</td>"
                                                               + "<td>" + childItems.Flag + "</td>"
                                                               + "<td>" + childItems.Range + "</td></tr>"
                            }
                        });
                        LabOrderResultSoapText += LabOrderResultParentSoapText + "" + LabOrderResultChildSoapText + "</tbody></table></div>";
                    });
                    //element.Comments = element.Comments.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
                    element.Comments = element.Comments.replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&amp;/g, '&');
                    element.Comments = element.Comments.replace(/&lt;/g, '<');
                    element.Comments = element.Comments.replace(/&gt;/g, '>');
                    element.Comments = element.Comments.replace(/&amp;/g, '&');
                    element.Comments = element.Comments.replace(/&nbsp;/g, ' ');

                    $ListMedications.append("<li>" + LabOrderResultSoapText + (element.Status == "" ? "" : "</br><b>Status:</b> " + element.Status + "</br>")
                                            + (element.Remarks == "" ? "" : "</br><b>Remarks:</b> " + element.Remarks + "</br>")
                                            + (element.Comments == "" ? "" : "</br><b>Comments:</b> " + $("<div>" + element.Comments + "</div>").html() + "</br>") + "</li>");


                    //             $ListMedications.append((element.Comments == "" ? "" : "<li>Comments: " + element.Comments));

                }
                if (MedicationsSOAPJSON.length - 1 == index) {
                    //$.each(medicationReviewSoapJSON, function (index, item) {
                    //    if (item.ReviewedOn != null && item.ReviewedOn != '') {
                    //        var dateFormat = item.ReviewedOn
                    //        var ReviewedOndateFormat = $.datepicker.formatDate('MM dd, yy ', new Date(dateFormat)) + utility.RemoveDateFromDateTime(null, dateFormat);
                    //        $ListMedications.append("<li>" + (item.ReviewedBy == '' ? "" : "Medications reviewed by " + item.ReviewedBy) +
                    //   " on:  " + ReviewedOndateFormat + "</li>");
                    //    }
                    //});
                }
                $DetailsDiv.append($ListMedications);
                $SectionBodyMedications.append($DetailsDiv);
                AListId.push(ALid);

                //if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML clinical_medications').parent().parent().find('#Cli_Medications_Main' + ALid).length == 0) {
                if ($(noteHTMLCtrl + ' clinical_labresults').parent().parent().find('#Cli_LabResultDetail_Main' + ALid).length == 0) {
                    $mainDivMedications.append($SectionBodyMedications);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(noteHTMLCtrl + ' clinical_labresults').parent().parent().find('#Cli_LabResultDetail_Main' + ALid + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(noteHTMLCtrl + ' clinical_labresults').parent().parent().find('#Cli_LabResultDetail_Main' + ALid + ' ul li:Last').get(0).outerHTML;
                    }
                    $(noteHTMLCtrl + ' clinical_labresults').parent().parent().find('#Cli_LabResultDetail_Main' + ALid).html($SectionBodyMedications.html());
                    $(noteHTMLCtrl + ' clinical_labresults').parent().parent().find('#Cli_LabResultDetail_Main' + ALid + ' ul').append(CommentHTML);;
                }


            });

            if (AListId.join(",") != "") {
                resultId = AListId.join(",");
            }
            if ($mainDivMedications.html() != '') {
                Clinical_LabOrder.updateResultHtml($mainDivMedications.html(), resultId, noteHTMLCtrl, hidemessage, bNotSaveComponent);
            } else {
                Clinical_LabOrder.updateResultHtml('', resultId, noteHTMLCtrl, hidemessage, bNotSaveComponent);
            }
        }

    },

    createResultBodyHTML: function (response, NoteHTMLCtrl, UnloadLabOrder) {
        Clinical_LabOrder.checkResultExists();
        if (response.LabResultFill_JSON != null && response.LabResultFill_JSON != '') {
            var LabOrderFill_Obj = JSON.parse(response.LabResultFill_JSON);
            var $mainDivLabOrder = $(document.createElement('div'));

            var LabResultId = LabOrderFill_Obj.LabResultId;
            if (LabResultId > 0) {
                var $SectionBodyLabOrder = $(document.createElement('section'));
                $SectionBodyLabOrder.attr('id', "Cli_LabResultDetail_Main" + LabResultId);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_LabResultDetail_" + LabResultId);
                var $ListLabOrder = $(document.createElement('ul'));

                $ListLabOrder.attr('class', 'list-unstyled')

                $SectionBodyLabOrder.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_LabResultDetail_" + LabResultId + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_LabResultDetail_Main" + LabResultId + '"  ><i class="fa fa-times"></i></a></div> ');


                $ListLabOrder.append("<li>" + LabOrderFill_Obj.SoapText + "</li>");
                $DetailsDiv.append($ListLabOrder);
                $SectionBodyLabOrder.append($DetailsDiv);
                if ($(NoteHTMLCtrl + ' clinical_labresults').parent().parent().find('#Cli_LabResultDetail_Main' + LabResultId).length == 0) {
                    $mainDivLabOrder.append($SectionBodyLabOrder);
                    Clinical_LabOrder.updateResultHtml($mainDivLabOrder.html(), LabResultId, NoteHTMLCtrl);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' clinical_labresults').parent().parent().find('#Cli_LabResultDetail_Main' + LabResultId + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' clinical_labresults').parent().parent().find('#Cli_LabResultDetail_Main' + LabResultId + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' clinical_labresults').parent().parent().find('#Cli_LabResultDetail_Main' + LabResultId).html($SectionBodyLabOrder.html());
                    $(NoteHTMLCtrl + ' clinical_labresults').parent().parent().find('#Cli_LabResultDetail_Main' + LabResultId + ' ul').append(CommentHTML);
                    Clinical_LabOrder.updateResultHtml("", LabResultId, NoteHTMLCtrl);

                }

                if (UnloadLabOrder == true) {
                    Clinical_LabOrder.Unload(ClinicalLabOrderDetail.bNextPrev);
                }
            }
        }
    },
    getLatestLabResultByPatientIdDBCall: function () {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["NoteId"] = Clinical_Notes.params.NotesId;
        objData["ProviderId"] = $("#pnlClinicalProgressNote #hfProviderId").val();
        objData["commandType"] = "getlatest_labresultby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },
    getLatestLabResultByPatientId: function (hidemessage, bNotSaveComponent) {
        var strMessage = '';
        //   AppPrivileges.GetFormPrivileges("Notes_Notes", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            Clinical_LabOrder.getLatestLabResultByPatientIdDBCall().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    Clinical_LabOrder.createResultBodyHTML123(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hidemessage, bNotSaveComponent);
                }
                else {
                    utility.DisplayMessages(strMessage, 3);
                }

            });
        }
        else {
            utility.DisplayMessages(strMessage, 3);
        }
    },

    detach_ComponentsLabResult: function (ComponentName, IsUpdate, LabResultComponentRemove) {
        var LabOrderIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_labresults').parent().parent().find('section[id*="Cli_LabResultDetail_Main"]').map(function () {
            return this.id.replace("Cli_LabResultDetail_Main", "");
        }).get().join(',');
        var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .LabResultsComponent').attr('NoteComponentId');
        if (LabResultComponentRemove) {
            if (NoteComponentId && NoteComponentId != "NCDummyId") {
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_labresults').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Lab Results', true))
                }
                else
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                $.when.apply($, promise).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_labresults').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                    $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Lab Results']").remove();
                });
            }
            else {
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Lab Results']").remove();
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_labresults').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Lab Results', true))
                }
                else {
                    if (NoteComponentId && NoteComponentId != "NCDummyId")
                        promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId));
                    else {
                        var def = $.Deferred();
                        promise.push(def);
                        def.resolve();
                    }
                }
                $.when.apply($, promise).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_labresults').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_labresults').parent().parent().find('section[id*="Cli_LabResultDetail_Main"]').remove();
        }

        if (LabOrderIds == "" || LabOrderIds == "undefined") {
            $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Lab Results']").remove();
            var promise = [];
            if (Clinical_ProgressNote.params["TemplateName"]) {
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_labresults').parent().parent().addClass('hidden');
                promise.push(Clinical_ProgressNote.saveComponentSOAPText('Lab Results', true))
            }
            else {
                if (NoteComponentId && NoteComponentId != "NCDummyId")
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId));
                else {
                    var def = $.Deferred();
                    promise.push(def);
                    def.resolve();
                }
            }
            $.when.apply($, promise).done(function () {
                if (Clinical_ProgressNote.params["TemplateName"] == "")
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_labresults').parent().parent().remove();
                utility.DisplayMessages('Successfully Deleted', 1);
                Clinical_ProgressNote.ShowHideComponetsHeaders();
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
            });
        }
        else {
            Clinical_LabOrder.detachLabResultFromNotesDBCall(LabOrderIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (IsUpdate) {
                        Clinical_ProgressNote.saveComponentSOAPText("Lab Results", true);
                    }
                    utility.DisplayMessages(response.Message, 1);
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },


    detachLabResultFromNotesDBCall: function (LabId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["LabResultIDs"] = LabId;
        if (Clinical_ProgressNote.params.NotesVisitId == "" || Clinical_ProgressNote.params.NotesVisitId == "undefined") {
            objData["VisitId"] = 0;
        } else {
            if (Clinical_ProgressNote.params.NotesVisitId < 1) {
                objData["VisitId"] = $('#pnlClinicalProgressNote #hfVisitId').val();
            } else {
                objData["VisitId"] = Clinical_ProgressNote.params.NotesVisitId;
            }

        }
        if (Clinical_ProgressNote.params.patientID == "" || Clinical_ProgressNote.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_ProgressNote.params.patientID;
        }
        objData["commandType"] = "detach_labresult_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },



    detachLabResultFromNotes: function (LabOrderId) {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('1', function () {
                EMRUtility.scrollToPNcomponent('clinical_labresult');
                EMRUtility.scrollToPNcomponent('clinical_labresults');
                var selectedValue = LabOrderId.replace('Cli_LabResultDetail_Main', '');
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    Clinical_LabOrder.detachLabResultFromNotes_DBCall(selectedValue).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            $('#' + LabOrderId).remove();

                            Clinical_ProgressNote.saveComponentSOAPText("Lab Results");
                            setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);

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
        // });
    },


    detachLabResultFromNotes_DBCall: function (LabOrderId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["LabResultIDs"] = LabOrderId;
        if (Clinical_ProgressNote.params.NotesVisitId == "" || Clinical_ProgressNote.params.NotesVisitId == "undefined") {
            objData["VisitId"] = 0;
        } else {
            if (Clinical_ProgressNote.params.NotesVisitId < 1) {
                objData["VisitId"] = $('#pnlClinicalProgressNote #hfVisitId').val();
            } else {
                objData["VisitId"] = Clinical_ProgressNote.params.NotesVisitId;
            }

        }
        if (Clinical_ProgressNote.params.patientID == "" || Clinical_ProgressNote.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_ProgressNote.params.patientID;
        }
        objData["commandType"] = "detach_Labresult_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },

    //Function Name: showLabResultsHistory
    //Author Name: Humaira Yousaf
    //Created Date: 02-05-2016
    //Description: Shows Lab order results history
    showLabResultsHistory: function (labOrderResultId) {
        if (event != null) {
            event.stopPropagation();
        }
        var ParentCtrl = 'clinicalTabLabOrder';
        var ParentCtrlPanelID = null;
        if (Clinical_LabOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
            ParentCtrl = 'Clinical_LabOrder';
            ParentCtrlPanelID = Clinical_LabOrder.params.PanelID;
        }
        else if (Clinical_LabOrder.params["ParentCtrl"] == "clinicalTabFaceSheet" || Clinical_LabOrder.params["ParentCtrl"] == "Clinical_FaceSheet") {
            ParentCtrl = 'Clinical_LabOrder';
            ParentCtrlPanelID = Clinical_LabOrder.params.PanelID;
        }

        EMRUtility.showCurrentItemHistory(Clinical_LabOrder.params.PanelID, null, null, "LabOrderResult,LabOrderResultDetail", Clinical_LabOrder.params.patientID, ParentCtrl, labOrderResultId, Clinical_LabOrder.params.PanelID);

    },

    viewPdfLabResult: function (currentPatientId, LabOrderId, ResultId) {

        var params = [];
        var PanelID = "";
        if (Clinical_LabOrder.params.ParentCtrl == "clinicalTabProgressNote") {
            PanelID = 'pnlClinicalProgressNote #pnlClinicalLabOrder';
            params["PanelID"] = 'pnlClinicalProgressNote #pnlClinicalLabOrder';
            params["PrPanelID"] = 'pnlClinicalProgressNote #pnlClinicalLabOrder';
        }
        else if (Clinical_LabOrder.params.ParentCtrl == "clinicalTabFaceSheet" || Clinical_LabOrder.params.ParentCtrl == "Clinical_FaceSheet") {
            PanelID = 'pnlClinicalFaceSheet #pnlClinicalLabOrder';
            params["PanelID"] = 'pnlClinicalFaceSheet #pnlClinicalLabOrder';
            params["PrPanelID"] = 'pnlClinicalFaceSheet #pnlClinicalLabOrder';
        }
        else {
            PanelID = 'pnlClinicalLabOrder';
            params["PanelID"] = 'pnlClinicalLabOrder';
            params["PrPanelID"] = 'pnlClinicalLabOrder';
        }

        params["ParentCtrl"] = (Clinical_LabOrder.params.ParentCtrl != "clinicalTabProgressNote" && Clinical_LabOrder.params.ParentCtrl != "clinicalTabFaceSheet" && Clinical_LabOrder.params.ParentCtrl != "Clinical_FaceSheet") ? Clinical_LabOrder.params.TabID : "Clinical_LabOrder";
        params["BarCodeHtml"] = 'true';
        params["FromAdmin"] = "0";
        params["UserId"] = globalAppdata['AppUserId'];
        params["PatientId"] = currentPatientId;
        //params["ParentCtrl"] = "Clinical_LabOrder";
        params["LabOrderId"] = LabOrderId;
        params["LabResultId"] = ResultId;
        // params["Caller"] = "viewpdf";          // This line is commented to show the requisition of the result (BioRef Format)
        LoadActionPan('Clinical_LabResultView', params, PanelID);


    },
}