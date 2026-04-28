OrderSet_LabOrder = {
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
        OrderSet_LabOrder.detailParams = OrderSet_LabOrder.params;
        //End 07-04-2016 Humaira Yousaf for retaining detail values

        OrderSet_LabOrder.params = params;
        OrderSet_LabOrder.patientId = $("div#PatientProfile #hfPatientId").val();
        OrderSet_LabOrder.params.mode = "Add";

        if (OrderSet_LabOrder.params.PanelID != 'pnlOrderSetLabOrder') {
            OrderSet_LabOrder.params.PanelID = OrderSet_LabOrder.params.PanelID + ' #pnlOrderSetLabOrder';
        } else {
            OrderSet_LabOrder.params.PanelID = 'pnlOrderSetLabOrder';
        }

        $('#' + OrderSet_LabOrder.params.PanelID + ' #pnlLabOrder_Search').resetAllControls(null);

        if (OrderSet_LabOrder.params.ParentCtrl == 'clinicalTabProgressNote') {

            $("#" + OrderSet_LabOrder.params.PanelID + " #ordersPending").attr('href', '#PatientLabOrderPending_ProgressNote')
            $("#" + OrderSet_LabOrder.params.PanelID + " #PatientLabOrderPending").attr('id', 'PatientLabOrderPending_ProgressNote');

            $("#" + OrderSet_LabOrder.params.PanelID + " #ordersSent").attr('href', '#PatientLabOrderSent_ProgressNote');
            $("#" + OrderSet_LabOrder.params.PanelID + " #PatientLabOrderSent").attr('id', 'PatientLabOrderSent_ProgressNote');

            $("#" + OrderSet_LabOrder.params.PanelID + " #results").attr('href', '#PatientLabResult_ProgressNote');
            $("#" + OrderSet_LabOrder.params.PanelID + " #PatientLabResult").attr('id', 'PatientLabResult_ProgressNote');

            $('.nav-tabs').tab();
            $("#" + OrderSet_LabOrder.params.PanelID + " #btnAddLabOrderToNote").show();
            $("#" + OrderSet_LabOrder.params.PanelID + " #btnAddLabResultToNote").show();

        }
        else {

            OrderSet_LabOrder.orderSearchGridId = 'dgvLabOrder';
            OrderSet_LabOrder.resultSearchGridId = 'dgvLabResult';

            $("#" + OrderSet_LabOrder.params.PanelID + " #btnAddLabOrderToNote").hide();
            $("#" + OrderSet_LabOrder.params.PanelID + " #btnAddLabResultToNote").hide();
        }


        var self = $('#' + OrderSet_LabOrder.params.PanelID);
        if (OrderSet_LabOrder.bIsFirstLoad == true) {

            //Load All Users
            CacheManager.BindDropDownsByID('#' + OrderSet_LabOrder.params.PanelID + ' #ddlAssigneeTemplate', 'GetUsers', true, 1).done(function () {
            });
            OrderSet_LabOrder.ValidateLab();

            OrderSet_LabOrder.LoadLabs('ddlLaboratory', OrderSet_LabOrder.params.PanelID);

            OrderSet_LabOrder.fillProvider('frmOrderSetLabOrder');
            OrderSet_LabOrder.fillProvider('frmOrderSetLabResult');


        }

        OrderSet_LabOrder.LabOrderSearch(null, null, null, null, "Pending");

        //utility.CreateDatePicker(OrderSet_LabOrder.params.PanelID + ' #dpStartDate', function () {
        //    //on-change callback method
        //});

        //utility.CreateDatePicker(OrderSet_LabOrder.params.PanelID + ' #dpToDate', function () {
        //    //on-change callback method
        //});

        EMRUtility.ValidateFromToDate('frmOrderSetLabOrder', 'dpStartDate', 'dpToDate', true, function () { }, function () { }, "To Date should be greater than From Date");
        EMRUtility.ValidateFromToDate('frmOrderSetLabResult', 'dpStartDate', 'dpToDate', true, function () { }, function () { }, "To Date should be greater than From Date");
        //Start//17-06-2016//Ahmad Raza// Tabs selection Logic
        var orderHref = "#PatientLabOrderPending";
        var resultHref = "#PatientLabResult";

        if (OrderSet_LabOrder.params.ParentCtrl == 'clinicalTabProgressNote') {
            orderHref = "#PatientLabOrderPending_ProgressNote";
            resultHref = "#PatientLabResult_ProgressNote";
        }
        if (OrderSet_LabOrder.params.Type == "Order") {
            $('#ulSocialHxTabsItems a[href="' + orderHref + '"]').trigger('click');
        }
        else if (OrderSet_LabOrder.params.Type == "Result") {
            $('#ulSocialHxTabsItems a[href="' + resultHref + '"]').trigger('click');
        }
        //End//17-06-2016//Ahmad Raza// Tabs selection Logic

        if (OrderSet_LabOrder.params.ParentCtrl == "clinicalTabFaceSheet") {
            EMRUtility.MakeFaceSheetPager('#' + OrderSet_LabOrder.params.PanelID + " div#FaceSheetPager", OrderSet_LabOrder.params.FaceSheetComponents, 'LabOrders');
        }



    },
    //Function Name: addProcedureOrderDetailParams
    //Author: Humaira Yousaf
    //Date: 07-04-2016
    //Description: Saves procedure order detail values at patient level
    addLabOrderDetailParams: function () {

        if (OrderSet_LabOrder.detailParams.hasData == true && OrderSet_LabOrder.detailParams.CurrentPatientId == $("div#PatientProfile #hfPatientId").val()) {
            OrderSet_LabOrder.params.hasData = true;
            OrderSet_LabOrder.params.ProviderName = OrderSet_LabOrder.detailParams.ProviderName;
            OrderSet_LabOrder.params.ProviderId = OrderSet_LabOrder.detailParams.ProviderId;
            OrderSet_LabOrder.params.AssigneeName = OrderSet_LabOrder.detailParams.AssigneeName;
            OrderSet_LabOrder.params.AssigneeId = OrderSet_LabOrder.detailParams.AssigneeId;
            OrderSet_LabOrder.params.Problems = OrderSet_LabOrder.detailParams.Problems;

            OrderSet_LabOrder.params.LabId = detailParams.params.LabId;
            OrderSet_LabOrder.params.BillingTypeId = OrderSet_LabOrder.detailParams.BillingTypeId;
            OrderSet_LabOrder.params.FacilityName = OrderSet_LabOrder.detailParams.FacilityName
            OrderSet_LabOrder.params.FacilityId = OrderSet_LabOrder.detailParams.FacilityId;

            OrderSet_LabOrder.params.CurrentPatientId = OrderSet_LabOrder.detailParams.CurrentPatientId;
        }
        else {
            OrderSet_LabOrder.params.hasData = false;
            OrderSet_LabOrder.params.ProviderName = "";
            OrderSet_LabOrder.params.ProviderId = "";
            OrderSet_LabOrder.params.AssigneeName = "";
            OrderSet_LabOrder.params.AssigneeId = "";
            OrderSet_LabOrder.params.Problems = "";

            OrderSet_LabOrder.params.LabId = "";
            OrderSet_LabOrder.params.BillingTypeId = "";
            OrderSet_LabOrder.params.FacilityName = "";
            OrderSet_LabOrder.params.FacilityId = "";

            OrderSet_LabOrder.params.CurrentPatientId = "";
        }
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: This function will handle fill procedure text box on input
    bindAutoComplete: function (element, refCtrlId, parentDivId) {

        if (OrderSet_LabOrder.params.ParentCtrl == 'clinicalTabProgressNote') {

            parentDivId = parentDivId + "_ProgressNote";
        }
        var hiddenCrtl = $('#' + OrderSet_LabOrder.params.PanelID + ' #' + (refCtrlId != null ? refCtrlId : 'txtCPTCode'));
        //  utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "OrderSet_LabOrder", null, true);
        EMRUtility.BindLOINCCodes(hiddenCrtl, "OrderSet_LabOrder", null, parentDivId);
    },

    //Called from LOINIC Control to pass code and description as Json obj
    pushLOINCAsCpt: function (JsonObj, $element) {

        var observation = JsonObj["Observation"];
        var LOINCCOde = JsonObj['LOINICCODE'];
        var LOINCDescription = JsonObj['LOINICDescription'];

        var txtCPT = $element.attr('id');
        //Set cpt and description as hidden field.
        if (txtCPT == "txtCPTCode") {
            $("#" + OrderSet_LabOrder.params.PanelID + " #frmOrderSetLabOrder #hfCPTCode").val(LOINCCOde);
            $("#" + OrderSet_LabOrder.params.PanelID + " #frmOrderSetLabOrder #hfCPTDescription").val(LOINCDescription);
        }
        else {
            $("#" + OrderSet_LabOrder.params.PanelID + " #frmOrderSetLabResult #hfCPTCode").val(LOINCCOde);
            $("#" + OrderSet_LabOrder.params.PanelID + " #frmOrderSetLabResult #hfCPTDescription").val(LOINCDescription);
        }

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: This function will handle opening of CPT Search Popup
    openCPTCode: function (refHiddenCtrl, refCtrl) {
        var params = [];

        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'OrderSetTabLabOrder';

        if (refHiddenCtrl != null && refHiddenCtrl != "") {
            params["RefHiddenCtrl"] = refHiddenCtrl;
        }
        else {
            params["RefHiddenCtrl"] = "hfCPTCode";
        }

        if (refCtrl != null && refCtrl != "") {
            params["RefCtrl"] = OrderSet_LabOrder.params.PanelID + " #" + refCtrl;
        }
        else {
            params["RefCtrl"] = OrderSet_LabOrder.params.PanelID + " #txtCPTCode";
        }
        params["RefCtrlDescription"] = "";

        params["ParentCtrlPanelID"] = OrderSet_LabOrder.params.PanelID;
        //  LoadActionPan('Admin_IMOCPT', params, OrderSet_LabOrder.params.PanelID);

        LoadActionPan('OrderSet_LOINC', params, OrderSet_LabOrder.params.PanelID);
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
                if (LabOrderId != null && parseInt(LabOrderId) > 0) {
                    params["LabOrderId"] = LabOrderId;
                    params["mode"] = "Edit";
                }
                else {
                    params["LabOrderId"] = -1;
                    params["mode"] = "Add";
                }
                params["FromAdmin"] = OrderSet_LabOrder.params["FromAdmin"] != null ? OrderSet_LabOrder.params["FromAdmin"] : "0";
                if (ParentCtrl != null && ParentCtrl != "") {
                    params["ParentCtrl"] = ParentCtrl;
                    params["ParentCtrlPanelID"] = ParentCtrlPanelID;//DashBoard.params.PanelID;
                    params["PanelID"] = ParentCtrlPanelID;
                    LoadActionPan('OrderSet_LabOrderDetails', params, ParentCtrlPanelID);
                }
                else if (OrderSet_LabOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
                    params["ParentCtrl"] = 'OrderSet_LabOrder';
                    params["ParentCtrlPanelID"] = OrderSet_LabOrder.params.PanelID;
                    LoadActionPan('OrderSet_LabOrderDetails', params, OrderSet_LabOrder.params.PanelID);
                }
                else if (OrderSet_LabOrder.params["ParentCtrl"] == "clinicalTabFaceSheet") {
                    params["ParentCtrl"] = 'OrderSet_LabOrder';
                    params["ParentCtrlPanelID"] = OrderSet_LabOrder.params.PanelID;
                    LoadActionPan('OrderSet_LabOrderDetails', params, OrderSet_LabOrder.params.PanelID);
                }

                else {
                    params["ParentCtrl"] = 'OrderSet_LabOrder';
                    LoadActionPan('OrderSet_LabOrderDetails', params);
                }
                //params["TabID"] = 'OrderSet_LabOrderDetails';


            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },


    //Author: Muhammad Arshad
    //Date :  15-04-2016
    //This function will handle Add/Edit of LabResult
    LabResultAddEdit: function (LabResultId, LabOrderId, isFromDetail, caller) {
        var strMessage = "";

        var permissionState = LabOrderId != null && parseInt(LabOrderId) > 0 ? "EDIT" : "ADD";
        AppPrivileges.GetFormPrivileges("Orders and Results_Lab Result", permissionState, "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                if (LabOrderId != null && parseInt(LabOrderId) > 0) {
                    params["LabResultId"] = LabResultId;
                    params["mode"] = "Edit";
                }
                else {
                    params["LabResultId"] = -1;
                    params["mode"] = "Add";
                }
                params["LabOrderId"] = LabOrderId;

                params["FromAdmin"] = OrderSet_LabOrder.params["FromAdmin"];

                if (isFromDetail) {
                    if (OrderSet_LabOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
                        if (caller != null) {

                            params["ParentCtrl"] = "OrderSet_LabOrderDetails";
                            LoadActionPan('OrderSetLabResultDetail', params);
                        }
                        else {
                            params["ParentCtrl"] = 'OrderSet_LabOrder';
                            params["ParentCtrlPanelID"] = OrderSet_LabOrder.params.PanelID;
                            LoadActionPan('OrderSetLabResultDetail', params, OrderSet_LabOrder.params.PanelID);
                        }
                    }
                    else {
                        params["ParentCtrl"] = "OrderSet_LabOrderDetails";
                        LoadActionPan('OrderSetLabResultDetail', params);
                    }
                }
                else {
                    if (OrderSet_LabOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
                        params["ParentCtrlPanelID"] = OrderSet_LabOrder.params.PanelID;
                        params["ParentCtrl"] = 'OrderSet_LabOrder';
                        LoadActionPan('OrderSetLabResultDetail', params, OrderSet_LabOrder.params.PanelID);
                    }
                    else if (OrderSet_LabOrder.params["ParentCtrl"] == "clinicalTabFaceSheet") {
                        params["ParentCtrlPanelID"] = OrderSet_LabOrder.params.PanelID;
                        params["ParentCtrl"] = 'OrderSet_LabOrder';
                        LoadActionPan('OrderSetLabResultDetail', params, OrderSet_LabOrder.params.PanelID);
                    }
                    else {
                        if (params["ParentCtrl"] == null) {
                            //EMR-2476
                            params['LabResultId'] = LabResultId;
                            //
                            params['RefModuleName'] = "Lab Results";
                            params['TransitionId'] = LabOrderId;
                        }
                        params["ParentCtrl"] = 'OrderSetTabLabOrder';
                        LoadActionPan('OrderSetLabResultDetail', params);
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
        $("#" + OrderSet_LabOrder.params.PanelID + " #frmOrderSetLabOrder")
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
            OrderSet_LabOrder.LabSave();
        });
    },

    //Author: Ahmad Raza
    //Date :  21-04-2016
    //Description: checkUncheckAllOrders
    checkUncheckAllOrders: function (obj) {
        if ($(obj).is(':checked')) {
            $("#" + OrderSet_LabOrder.params.PanelID + " input[name='SelectCheckBoxOrder']:checkbox").prop('checked', true);
        } else {
            $("#" + OrderSet_LabOrder.params.PanelID + " input[name='SelectCheckBoxOrder']:checkbox").prop('checked', false);
        }
        $("#" + OrderSet_LabOrder.params.PanelID + " #dgvLabOrdertSent tbody").find('input[type="checkbox"]').each(function () {
            OrderSet_LabOrder.enableAddOrder(this, null);
        });

    },




    LabOrderSearch: function (LabId, PageNo, rpp, caller, OrderStatus) {

        var strMessage = "";

        if (OrderSet_LabOrder.params.ParentCtrl == "clinicalTabProgressNote") {
            if ($("#" + OrderSet_LabOrder.params.PanelID + " #dgvLabOrdertSent  thead tr #selectRecordOrders").length == 0) {
                $("#" + OrderSet_LabOrder.params.PanelID + " #dgvLabOrdertSent thead tr").prepend('<th style="width:50px"><input type="checkbox" onchange="OrderSet_LabOrder.checkUncheckAllOrders(this);" controlname="selectRecordOrders" id="selectRecordOrders" name="chkHeader" class="input-block" coltype="checkbox"/></th>');
            }

            //if ($("#" + OrderSet_LabOrder.params.PanelID + " #dgvLabOrdertSent thead tr #selectRecordSentOrders").length == 0) {

            //    $("#" + OrderSet_LabOrder.params.PanelID + " #dgvLabOrdertSent thead tr").prepend('<th style="width:50px"><input type="checkbox" onchange="OrderSet_LabOrder.checkUncheckAllOrders(this);" controlname="selectRecordSentOrders" id="selectRecordSentOrders" name="chkHeader" class="input-block" coltype="checkbox"/></th>');
            //}

        } else {
            $("#" + OrderSet_LabOrder.params.PanelID + " #dgvLabOrdertSent" + " th#selectRecordOrders").remove();
            // $("#" + OrderSet_LabOrder.params.PanelID + " #dgvLabOrdertSent thead tr #selectRecordSentOrders").remove();
        }



        if ($("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabOrder_Result").css("display") == "none") {
            $("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabOrder_Result").show();
        }
        AppPrivileges.GetFormPrivileges("Orders and Results_Lab Order", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var self = $("#" + OrderSet_LabOrder.params.PanelID + " form");

                var myJSON = self.getMyJSONByName();

                // search specific on caller Id
                if (caller != null) {
                    if (caller.indexOf("LabOrderDetail") >= 0) {
                        myJSON = null;
                        // Reload Providers
                        OrderSet_LabOrder.fillProvider('frmOrderSetLabOrder');
                        OrderSet_LabOrder.fillProvider('frmOrderSetLabResult');
                    }
                }

                OrderSet_LabOrder.searchLabOrder(myJSON, LabId, PageNo, rpp, OrderStatus).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        if (OrderStatus == "Signed") {
                            OrderSet_LabOrder.LabGridLoadSent(response);
                            //Start//21-06-2016//Ahmad Raza//logic for select All
                            var totalRows = $("#" + OrderSet_LabOrder.params.PanelID + " #dgvLabOrdertSent tr").length;
                            totalRows -= 1;
                            var selectedRows = $("#" + OrderSet_LabOrder.params.PanelID + " #dgvLabOrdertSent tbody tr input:checked").length;
                            if (totalRows == selectedRows) {
                                $("#" + OrderSet_LabOrder.params.PanelID + " #dgvLabOrdertSent tr #selectRecordOrders").prop("checked", true);
                            }
                            else {
                                $("#" + OrderSet_LabOrder.params.PanelID + " #dgvLabOrdertSent tr #selectRecordOrders").prop("checked", false);
                            }
                            //End//21-06-2016//Ahmad Raza//logic for select All
                            var TableControl = OrderSet_LabOrder.params.PanelID + " #pnlLabOrder_Result #dgvLabOrdertSent";
                            var PagingPanelControlID = OrderSet_LabOrder.params.PanelID + " #dgvLabOrderSent_Paging";
                            var ClassControlName = "OrderSet_LabOrder";
                            var PagesToDisplay = 5;
                            var iTotalDisplayRecords = response.iTotalDisplayRecords;
                            rpp = rpp == "" ? null : rpp;
                            OrderSet_LabOrder.signed = "Signed";

                            setTimeout(
                                CreatePagination(response.LabOrderCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                                    OrderSet_LabOrder.LabOrderSearch(0, PageNumber, ResultPerPage, null, OrderSet_LabOrder.signed);
                                }), 10);
                        }
                        else {
                            OrderSet_LabOrder.LabGridLoad(response);
                            //Start//21-06-2016//Ahmad Raza//logic for select All
                            //var totalRows = $("#" + OrderSet_LabOrder.params.PanelID + " #" + OrderSet_LabOrder.orderSearchGridId + " tr").length;
                            //totalRows -= 1;
                            //var selectedRows = $("#" + OrderSet_LabOrder.params.PanelID + " #" + OrderSet_LabOrder.orderSearchGridId + " tbody tr input:checked").length;
                            //if (totalRows == selectedRows) {
                            //    $("#" + OrderSet_LabOrder.params.PanelID + " #" + OrderSet_LabOrder.orderSearchGridId + " #selectRecordOrders").prop("checked", true);
                            //}
                            //else {
                            //    $("#" + OrderSet_LabOrder.params.PanelID + " #" + OrderSet_LabOrder.orderSearchGridId + " #selectRecordOrders").prop("checked", false);
                            //}
                            //End//21-06-2016//Ahmad Raza//logic for select All
                            var TableControl = OrderSet_LabOrder.params.PanelID + " #pnlLabOrder_Result #" + OrderSet_LabOrder.orderSearchGridId;
                            var PagingPanelControlID = OrderSet_LabOrder.params.PanelID + " #dgvLabOrder_Paging";
                            var ClassControlName = "OrderSet_LabOrder";
                            var PagesToDisplay = 5;
                            var iTotalDisplayRecords = response.iTotalDisplayRecords;
                            OrderSet_LabOrder.pending = "Pending";
                            setTimeout(
                                CreatePagination(response.LabOrderCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                                    OrderSet_LabOrder.LabOrderSearch(null, PageNumber, ResultPerPage, null, OrderSet_LabOrder.pending);
                                }), 10);
                        }

                        if (OrderSet_LabOrder.bIsFirstLoad || caller != null) {
                            OrderSet_LabOrder.signed = "Signed";
                            OrderSet_LabOrder.LabOrderSearch(null, null, null, null, OrderSet_LabOrder.signed);
                            OrderSet_LabOrder.bIsFirstLoad = false;
                            caller = null;
                        }


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





    LabResultsSearch: function (LabId, PageNo, rpp, caller) {
        var strMessage = "";

        if (OrderSet_LabOrder.params.ParentCtrl == "clinicalTabProgressNote") {
            if ($("#" + OrderSet_LabOrder.params.PanelID + " #" + OrderSet_LabOrder.resultSearchGridId + " thead tr #selectRecordOrders").length == 0) {
                $("#" + OrderSet_LabOrder.params.PanelID + " #" + OrderSet_LabOrder.resultSearchGridId + " thead tr").prepend('<th style="width:50px"><input type="checkbox" onchange="OrderSet_LabOrder.checkUncheckAllOrders(this);" controlname="selectRecordOrders" id="selectRecordOrders" name="chkHeader" class="input-block" coltype="checkbox"/></th>');
            }
        } else {
            $("#" + OrderSet_LabOrder.params.PanelID + " #" + OrderSet_LabOrder.resultSearchGridId + " th#selectRecordOrders").remove();
        }



        if ($("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabResult_Result").css("display") == "none") {
            $("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabResult_Result").show();
        }
        AppPrivileges.GetFormPrivileges("Orders and Results_Lab Result", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var self = $("#" + OrderSet_LabOrder.params.PanelID + " #frmOrderSetLabResult");

                var myJSON = self.getMyJSONByName();

                // search specific on caller Id
                if (caller != null) {
                    if (caller.indexOf("LabResultDetail") >= 0) {
                        myJSON = null;
                        // Reload Providers
                        //OrderSet_LabOrder.fillProvider('frmOrderSetLabOrder');
                        OrderSet_LabOrder.fillProvider('frmOrderSetLabResult');
                    }
                }
                OrderSet_LabOrder.searchLabResults(myJSON, LabId, PageNo, rpp).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        OrderSet_LabOrder.LabResultsGridLoad(response);
                        //Start//21-06-2016//Ahmad Raza//logic for select All
                        var totalRows = $("#" + OrderSet_LabOrder.params.PanelID + " #" + OrderSet_LabOrder.resultSearchGridId + " tr").length;
                        totalRows -= 1;
                        var selectedRows = $("#" + OrderSet_LabOrder.params.PanelID + " #" + OrderSet_LabOrder.resultSearchGridId + " tbody tr input:checked").length;

                        if (totalRows == selectedRows) {
                            $("#" + OrderSet_LabOrder.params.PanelID + " #" + OrderSet_LabOrder.resultSearchGridId + " #selectRecordOrders").prop("checked", true);
                        }
                        else {
                            $("#" + OrderSet_LabOrder.params.PanelID + " #" + OrderSet_LabOrder.resultSearchGridId + " #selectRecordOrders").prop("checked", false);
                        }
                        //End//21-06-2016//Ahmad Raza//logic for select All
                        var TableControl = OrderSet_LabOrder.params.PanelID + " #pnlLabResult_Result #" + OrderSet_LabOrder.resultSearchGridId;
                        var PagingPanelControlID = OrderSet_LabOrder.params.PanelID + " #dgvLabResult_Paging";
                        var ClassControlName = "OrderSet_LabOrder";
                        var PagesToDisplay = 15;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(
                            CreatePagination(response.LabResultCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                                OrderSet_LabOrder.LabResultsSearch(PrimaryID, PageNumber, ResultPerPage);
                            }), 10);
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

    LabResultsGridLoad: function (response) {


        //Start//Abid Ali
        OrderSet_LabOrder.labResultRows = [];
        //End//Abid Ali
        //Start 24-05-2016 Muhammad Arshad Remove Duplicate search issue on Datatable
        if ($.fn.dataTable.isDataTable("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabResult_Result #" + OrderSet_LabOrder.resultSearchGridId)) {
            $("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabResult_Result #" + OrderSet_LabOrder.resultSearchGridId).dataTable().fnClearTable();
            $("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabResult_Result #" + OrderSet_LabOrder.resultSearchGridId).dataTable().fnDestroy();
        }
        $("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabResult_Result #" + OrderSet_LabOrder.resultSearchGridId + " tbody").find("tr").remove();
        if (response.LabResultCount > 0) {
            var LabLoadJSONData = JSON.parse(response.LabOrderResultModel_JSON)//JSON.parse(response.LabOrderLoad_JSON); //Parsing array to JSON
            $.each(LabLoadJSONData, function (i, item) {

                var currentDate = new Date();
                var currentTime = currentDate.toLocaleTimeString().replace(/([\d]+:[\d]{2})(:[\d]{2})(.*)/, "$1$3");


                var onclick = "OrderSet_LabOrder.labOrderResultRowExpand(this,event);";
                var expandCollapseIcon = '<a href="#" onclick="' + onclick + '" class="tab_space" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a>';


                //Convert Date to prper date time format
                if (item.ModifiedOn != null) {
                    currentDate = new Date(item.ModifiedOn).toLocaleDateString();
                    currentTime = new Date(item.ModifiedOn).toLocaleTimeString().replace(/([\d]+:[\d]{2})(:[\d]{2})(.*)/, "$1$3");
                }


                var $row = $('<tr/>');

                $row.attr("id", "gvLab_row" + item.LabResultId);
                $row.attr("LabOrderResultId", item.LabResultId);

                $row.attr("labid", item.LabOrderId);

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
                    if (OrderSet_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.LabResultId + "", OrderSet_ProgressNote.DetachedNoteComponentIds) > -1) {
                        Checked = " ";
                    } else {
                        Checked = " checked";
                    }
                }
                var parentControl = "";
                var parentControlPanelID = null;
                if (OrderSet_LabOrder.params.ParentCtrl == "clinicalTabProgressNote") {

                    parentControl = 'OrderSet_LabOrder';
                    parentControlPanelID = OrderSet_LabOrder.params.PanelID;

                    SelectionCheckBoxColumn = '<td><input type="checkbox" onchange="OrderSet_LabOrder.enableAddResult(this,event);" id="' + item.LabResultId + '" name="SelectCheckBoxOrder" ' + Checked + ' class="input-block"/></td>';

                }
                else if (OrderSet_LabOrder.params.ParentCtrl == "clinicalTabFaceSheet") {
                    parentControl = 'OrderSet_LabOrder';
                    parentControlPanelID = OrderSet_LabOrder.params.PanelID;
                }

                else {
                    parentControl = 'OrderSetTabLabOrder';
                    SelectionCheckBoxColumn = "";
                }


                if (item.LabOrderTests[0].CPTDescription == null) {
                    item.LabOrderTests[0].CPTDescription = "";
                }


                var divLoinc = item.LabOrderTests.length > 0 && item.LabOrderTests[0].LabOrderResultDetails.length > 0 ? item.LabOrderTests[0].LabOrderResultDetails[0].LOINC + " " + item.LabOrderTests[0].LabOrderResultDetails[0].LOINCDescription : item.LabOrderTests[0].CPTDescription;
                var PrintResultClass = "disableAll";
                if (item.IsAknowledged == true) {
                    PrintResultClass = "";
                }

                var ResultNTETextTooltip = "";
                //For Result NTE if HL7 is received
                if (item.NTEText != null && item.NTEText != "") {
                    ResultNTETextTooltip = ' <i class="fa fa-exclamation ellip100" style="color: blue;padding-left:10px;" data-toggle="tooltip" data-placement="right" title="' + item.NTEText + '"></i> ';
                }

                //Start/ / 20 - 04 - 2016//Ahmad Raza//implimented logic to show icon for showing DBAudit History
                $row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.LabResultId + '</td><td><a class="btn btn-xs" href="#" onclick="OrderSet_LabOrder.LabResultDelete(\'' + item.LabResultId + '\', event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="OrderSet_LabOrder.LabResultAddEdit(\'' + item.LabResultId + '\',\'' + item.LabOrderId + '\', false);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs ' + PrintResultClass + '" href="#" onclick="OrderSet_LabOrder.printLabResult(\'' + item.LabOrderId + '\',\'' + item.LabResultId + '\',\'' + item.Status + '\',event,\'' + item.PatientId + '\' );" title="View Result"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="OrderSet_LabOrder.showLabResultsHistory(\'' + item.LabResultId + '\', event );" title="Record History"> <i class="fa fa-history blue"></i></a>&nbsp;<a title="Print Specimen Label" class="btn  btn-xs hidden" href="#"  onclick="OrderSet_LabOrder.printSpecimen(\'' + item.LabOrderId + '\',\'' + item.Status + '\',event );" ><i class="fa fa-barcode"></i> </a></td><td>'
                         + currentDate + " " + currentTime + '</td><td>' + expandCollapseIcon + divLoinc + '</td><td>' + item.LabName + '</td><td>' + item.OrderNo + '</td><td>' + item.Status + '</td><td>' + item.Provider + '</td><td>' + item.AssigneeName + '</td><td class="hidden">' + Number(new Date(currentDate + ' ' + currentTime)) + '</td>');
                //End//20-04-2016//Ahmad Raza//implimented logic to show icon for showing DBAudit History
                $("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabResult_Result #" + OrderSet_LabOrder.resultSearchGridId + " tbody").last().append($row);

                var childRows = OrderSet_LabOrder.buildLabOrderResultRowChild(item.LabOrderTests, item);
                OrderSet_LabOrder.labResultRows.push({ row: $row, childs: childRows });

                utility.bindMyJSONByName(true, item, false, childRows).done(function () {
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
                    var currentPatientAccount = item.AccountNumber;
                    var currentPatientId = item.PatientId;
                    var currentPatientName = item.PatientFullName;
                    var currentModuleName = "Lab Result";
                    var ResultId = item.LabResultId;
                    var LabOrderId = item.LabOrderId;
                    var CreatedBy = item.CreatedBy;
                    var btnScanDocument = childRows.find("#anchorDocumentScan").unbind('click').bind("click", function (e) {
                        EMRUtility.documentScan("OrderSetTabLabOrder", currentModuleName, currentPatientId, currentPatientName, ResultId);
                    });

                    var btnUploadDocument = childRows.find("#anchorDocumentUpload").unbind('click').bind("click", function () {
                        EMRUtility.documentImport("OrderSetTabLabOrder", currentModuleName, currentPatientId, currentPatientName, ResultId, currentPatientAccount);
                    });

                    var btnViewAttachment = childRows.find("#btnViewAttachment").unbind('click').bind("click", function (e) {
                        EMRUtility.loadAttachments("OrderSetTabLabOrder", currentModuleName, "load_attachments", currentPatientId, ResultId, "#" + OrderSet_LabOrder.params.PanelID + " #menuViewAttachment");

                    });
                    var onClick = "OrderSet_LabOrder.SaveLabResult(this,'" + item.OrderNo + "','" + item.PatientId + "','" + item.ProviderId + "',event)";
                    childRows.find('#btnSaveResult').attr('onclick', onClick);
                    var btnViewHL7 = childRows.find("#btnShowHL7");
                    if (CreatedBy != null && CreatedBy.toLowerCase().indexOf("mirth") > -1) {
                        btnViewHL7.removeClass("disableAll");
                        btnViewHL7.unbind('click').bind("click", function (e) {
                            EMRUtility.viewHL7PDF("OrderSetTabLabOrder", currentPatientId, LabOrderId, ResultId);

                        });
                    }
                    else {
                        btnViewHL7.addClass("disableAll");
                        btnViewHL7.unbind('click');
                    }
                });

                var options = $('#' + OrderSet_LabOrder.params.PanelID + ' #ddlAssigneeTemplate').find('option').clone();
                childRows.find('#ddlAssigneeId').append(options);
                childRows.find('#ddlAssigneeId').val(item.AssigneeId)

                var options = $('#' + OrderSet_LabOrder.params.PanelID + ' #ddlAssigneeTemplate').find('option').clone();
                childRows.find('#ddlReviewedById').append(options);
                childRows.find('#ddlReviewedById').val(item.ReviewedById)


            });
        }
        else {

            $("#" + OrderSet_LabOrder.params.PanelID + ' #pnlLabResult_Result #' + OrderSet_LabOrder.resultSearchGridId).DataTable({
                "destroy": true,
                "destroy": true,
                "language": {
                    "emptyTable": "No Lab Result Found"
                }, "autoWidth": false, "bLengthChange": false, "order": [[9, "desc"]]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + OrderSet_LabOrder.params.PanelID + ' #pnlLabResult_Result #' + OrderSet_LabOrder.resultSearchGridId))
            ;
        else {
            OrderSet_LabOrder.EditableGridOrderResult = $("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabResult_Result #" + OrderSet_LabOrder.resultSearchGridId).DataTable({
                "destroy": true,
                "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[3, "desc"]]
            }); // to remove records per page dropdown
        }


        EMRUtility.fixDataTableDuplication("#" + OrderSet_LabOrder.params.PanelID + ' #pnlLabResult_Result');

        $.each(OrderSet_LabOrder.labResultRows, function (i, item) {

            if (OrderSet_LabOrder.EditableGridOrderResult != null) {

                var row = OrderSet_LabOrder.EditableGridOrderResult.row(item.row);
                if (item.childs.length > 0) {
                    row.child(item.childs);
                }
                else {
                    //$(item.row).find('td:first').find('a').hide();
                }
            }
        });
    },

    //For Lab Result
    buildLabOrderResultRowChild: function (tests, object) {

        //  var tests = tests//childItems.Test.split('|');
        var CurrentRowchilds = $();
        var templateHtml = $("#" + OrderSet_LabOrder.params.PanelID + " #LabResultTemplate").clone();

        if (tests != null && tests.length > 0) {

            var $tbody = templateHtml.find('#dgvLabResultTemplate').find('tbody');

            $.each(tests, function (i, item) {

                var $ChilRowTestDetail = $('<tr/>').addClass("childRowTest-bg");
                var ResultNTETextTooltip = "";
                //For Result NTE if HL7 is received
                if (item.NTEText != null && item.NTEText != "") {
                    ResultNTETextTooltip = ' <i class="fa fa-exclamation ellip100" style="color: blue;padding-left:10px;" data-toggle="tooltip" data-placement="right" title="' + item.NTEText + '"></i> ';
                }
                $ChilRowTestDetail.append('<td class="bold" colspan="6" >' + item.CPTCode + " " + item.CPTDescription + '</td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td>  <td class="hidden"></td> <td class="hidden"></td>');
                $tbody.append($ChilRowTestDetail);

                $.each(item.LabOrderResultDetails, function (i, detailItem) {
                    var ResultNTETextTooltip = "";
                    //For Result NTE if HL7 is received
                    if (detailItem.NTEText != null && detailItem.NTEText != "") {
                        ResultNTETextTooltip = '<i class="fa fa-exclamation ellip100" style="color: blue;padding-left:10px;" data-toggle="tooltip" data-placement="right" title="' + detailItem.NTEText + '"></i>';
                    }
                    $ChilRowDetail = $('<tr/>').addClass("childRowDetail-bg");
                    $ChilRowDetail.append('<td  >' + detailItem.LOINC + " " + detailItem.LOINCDescription + ResultNTETextTooltip + '</td> <td>' + detailItem.Result + '</td> <td>' + detailItem.UoM + '</td> <td>' + detailItem.Range + '</td>  <td>' + detailItem.Flag + '</td> <td>' + object.Status + '</td>');

                    $tbody.append($ChilRowDetail);

                });
            });
        }

        var $row = $('<tr/>').addClass("childRow-bg");
        var cellpadding = '';

        $row.append('<td class="hidden"></td> <td class="hidden"> <td class="hidden"></td>  <td class="hidden"></td> <td colspan="9">' + templateHtml.html() + '</td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td>');
        return CurrentRowchilds = CurrentRowchilds.add($row);

    },

    labOrderResultRowExpand: function ($row, event) {

        $row = $($row).parent().parent();

        var $actions,
         values = [];
        var row = OrderSet_LabOrder.EditableGridOrderResult.row($row);
        //if ($row.hasClass('adding')) {
        //    $row.removeClass('adding');
        //}
        if (row.child.isShown()) {
            // This row is already open - close it
            $row.find("td:eq(3) .fa-minus-square").attr("class", "fa fa-plus-square");
            row.child.hide();
            //tr.removeClass('shown');
        }
        else {
            $row.find("td:eq(3) .fa-plus-square").attr("class", "fa fa-minus-square");
            // Open this row
            row.child.show();
        }
        event.preventDefault();
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

        OrderSet_LabOrder.LabResultSave(Obj).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                utility.DisplayMessages(response.message, 1);

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

        return Clinical_Lab.searchLab(null, 0, 1, 5000).done(function (response) {
            //Populate Distinct Values in typeArray
            response = JSON.parse(response);
            var NameArray = new Array();
            var labIds = new Array();
            var codeSystem = new Array();
            var data = JSON.parse(response.ClinicalLab_JSON);
            $.each(data, function (i, row) {
                if (jQuery.inArray(row.Name, NameArray) === -1) {
                    NameArray.push(row.Name);
                    labIds.push(row.LabId);
                    codeSystem.push(row.CodeSystemId); // Code System 1 means CPT , 2 Means Lab Codes
                }
            });
            var ddType = $('#' + controlPanelID + " #" + ddlId);
            ddType.empty();
            ddType.append($("<option />").val("").text('-Select-'));
            if (NameArray.length > 0) {
                $.each(NameArray, function (index, Name) {
                    ddType.append($("<option />").val(labIds[index]).text(Name).attr('CodeSystem', codeSystem[index]));
                });
            }
        });
    },

    //Function Name: fillProvider
    //Author: Abid Ali
    //Date: 08-04-2016
    //Description: This function will fill Provider dropdown
    fillProvider: function (formId) {

        OrderSet_LabOrder.fillproviderDBCall().done(function (response) {
            if (response != "") {
                response = JSON.parse(response);

                var $ddlProvider = $('#' + OrderSet_LabOrder.params.PanelID + ' #' + formId + ' #ddlProvider');
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
        return MDVisionService.APIService(data, "OrderSet", "LabOrder");

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Loads Lab Orders data in grid
    //Params: response
    LabGridLoad: function (response) {

        //Start//Abid Ali
        OrderSet_LabOrder.labOrderRows = [];

        //Start 24-05-2016 Muhammad Arshad Remove Duplicate search issue on Datatable
        if ($.fn.dataTable.isDataTable("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabOrder_Result #" + OrderSet_LabOrder.orderSearchGridId)) {
            $("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabOrder_Result #" + OrderSet_LabOrder.orderSearchGridId).dataTable().fnClearTable();
            $("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabOrder_Result #" + OrderSet_LabOrder.orderSearchGridId).dataTable().fnDestroy();
        }
        $("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabOrder_Result #" + OrderSet_LabOrder.orderSearchGridId + " tbody").find("tr").remove();
        //End 24-05-2016 Muhammad Arshad Remove Duplicate search issue on Datatable
        //$("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabOrder_Result #dgvLabOrder").dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it
        //$("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabOrder_Result #dgvLabOrder tbody").find("tr").remove(); //Removing all the table data from table body
        if (response.LabOrderCount > 0) {
            var LabLoadJSONData = JSON.parse(response.LabOrderFill_JSON);
            //JSON.parse(response.LabLoad_JSON); //Parsing array to JSON
            $.each(LabLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                // $row.attr("onclick", "OrderSet_LabOrderDetails.LabEdit('" + item.LabOrderId + "',event);");
                $row.attr("id", "gvLab_row" + item.LabOrderId);
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
                var onclick = "OrderSet_LabOrder.labOrderRowExpand(this,event);"
                //Start//21-04-2016//Ahmad Raza//logic to check uncheck order if it is attached with Note
                var expandCollapseIcon = '<a href="#" onclick="' + onclick + '" class="tab_space" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a>';


                var SelectionCheckBoxColumn = "";
                var Checked = "";
                if (item.IsNoteLinked == "True") {
                    if (OrderSet_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.LabOrderId + "", OrderSet_ProgressNote.DetachedNoteComponentIds) > -1) {
                        Checked = " ";
                    } else {
                        Checked = " checked";
                    }
                }

                if (OrderSet_LabOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                    SelectionCheckBoxColumn = ""; //'<td><input type="checkbox" onclick="OrderSet_LabOrder.enableAddOrder(this,event);" id="' + item.LabOrderId + '" name="SelectCheckBoxOrder" ' + Checked + ' class="input-block"/></td>';
                } else {
                    SelectionCheckBoxColumn = "";
                }
                //End//21-04-2016//Ahmad Raza//logic to check uncheck order if it is attached with Note
                //Start//20-04-2016//Ahmad Raza//implimented logic to show icon for showing DBAudit History
                if (item.Status == "Signed") {

                    //Begin 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578
                    $row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.LabOrderId + '</td><td><a class="btn btn-xs disableAll" href="#" onclick="OrderSet_LabOrder.LabOrderDelete(\'' + item.LabOrderId + '\', event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="OrderSet_LabOrderDetails.LabEdit(\'' + item.LabOrderId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="OrderSet_LabOrder.printLabOrder(\'' + item.LabOrderId + '\',\'' + item.Status + '\',event );" title="View Result"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="OrderSet_LabOrder.showOrderHistory(\'' + item.LabOrderId + '\', event );" title="Record History"> <i class="fa fa-history blue"></i></a>&nbsp;<a title="Print Specimen Label" class="btn  btn-xs" href="#"  onclick="OrderSet_LabOrder.printSpecimen(\'' + item.LabOrderId + '\',\'' + item.Status + '\',event );" ><i class="fa fa-barcode"></i> </a></td><td>'
                    + utility.RemoveTimeFromDate(null, item.OrderDate) + ' ' + item.OrderTime + '</td><td>' + expandCollapseIcon + item.Test.split('|')[0] + '</td><td>' + item.LabName + '</td><td>' + item.OrderNo + '</td><td>' + item.Status + '</td><td>' + item.Provider + '</td><td>' + item.AssigneeName + '</td>');
                    //End 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578

                }
                else {

                    //Begin 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578
                    $row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.LabOrderId + '</td><td><a class="btn btn-xs" href="#" onclick="OrderSet_LabOrder.LabOrderDelete(\'' + item.LabOrderId + '\', event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="OrderSet_LabOrderDetails.LabEdit(\'' + item.LabOrderId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs disableAll" href="#" onclick="OrderSet_LabOrder.printLabOrder(\'' + item.LabOrderId + '\',\'' + item.Status + '\',event );" title="View Result"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="OrderSet_LabOrder.showOrderHistory(\'' + item.LabOrderId + '\', event );" title="Record History"> <i class="fa fa-history blue"></i></a>&nbsp;<a title="Print Specimen Label" class="btn  btn-xs" href="#"  onclick="OrderSet_LabOrder.printSpecimen(\'' + item.LabOrderId + '\',\'' + item.Status + '\',event );" ><i class="fa fa-barcode"></i> </a></td><td>'
                        + utility.RemoveTimeFromDate(null, item.OrderDate) + ' ' + item.OrderTime + '</td><td>' + expandCollapseIcon + item.Test.split('|')[0] + '</td><td>' + item.LabName + '</td><td>' + item.OrderNo + '</td><td>' + item.Status + '</td><td>' + item.Provider + '</td><td>' + item.AssigneeName + '</td>');
                    //End 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578
                }
                //End//20-04-2016//Ahmad Raza//implimented logic to show icon for showing DBAudit History
                $("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabOrder_Result #" + OrderSet_LabOrder.orderSearchGridId + " tbody").last().append($row);

                var childRows = OrderSet_LabOrder.buildLabOrderRowChild(item.LabOrderTests, item.LabOrderId);
                OrderSet_LabOrder.labOrderRows.push({ row: $row, childs: childRows });
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

            $("#" + OrderSet_LabOrder.params.PanelID + ' #pnlLabOrder_Result #' + OrderSet_LabOrder.orderSearchGridId).DataTable({
                "destroy": true,
                "language": {
                    "emptyTable": "No Lab Order Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + OrderSet_LabOrder.params.PanelID + ' #pnlLabOrder_Result #' + OrderSet_LabOrder.orderSearchGridId))
            ;
        else {
            OrderSet_LabOrder.EditableGridOrder = $("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabOrder_Result #" + OrderSet_LabOrder.orderSearchGridId).DataTable({
                "destroy": true,
                "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "aaSorting": [[2, "desc"]]
            }); // to remove records per page dropdown
        }

        EMRUtility.fixDataTableDuplication("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabOrder_Result");

        $.each(OrderSet_LabOrder.labOrderRows, function (i, item) {

            if (OrderSet_LabOrder.EditableGridOrder != null) {

                var row = OrderSet_LabOrder.EditableGridOrder.row(item.row);
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
        params["PatientID"] = OrderSet_LabOrder.params.patientID;
        params["LabOrderId"] = labOrderId;
        params["providerid"] = $('#pnlOrderSetLabOrder #hfprovider').val();
        params["ParentCtrl"] = 'OrderSet_LabOrder';
        if (OrderSet_LabOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
            if (ParentCtrl == 'OrderSet_LabOrderView') {
                params["ParentCtrl"] = ParentCtrl;
                LoadActionPan('ClinicalBarCodeView', params);
            } else {
                params["ParentCtrlPanelID"] = OrderSet_LabOrder.params.PanelID;
                if (ParentCtrl != null) {
                    params["ParentCtrl"] = ParentCtrl;
                }
                LoadActionPan('ClinicalBarCodeView', params, OrderSet_LabOrder.params.PanelID);
            }

        } else if (ParentCtrl == 'OrderSet_LabOrderView') {
            params["ParentCtrl"] = ParentCtrl;
            LoadActionPan('ClinicalBarCodeView', params);
        } else {
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

                if (OrderSet_LabOrder.params["ParentCtrl"] == "clinicalTabProgressNote" && ParentCtrl != "mstrTabDashBoard") {
                    params["ParentCtrl"] = 'OrderSet_LabOrder';
                    params["ParentCtrlPanelID"] = OrderSet_LabOrder.params.PanelID;
                    LoadActionPan('Document_Viewer', params, OrderSet_LabOrder.params.PanelID);

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

        //Start//Abid Ali
        OrderSet_LabOrder.labOrderRowsSent = [];

        //Start 24-05-2016 Muhammad Arshad Remove Duplicate search issue on Datatable
        if ($.fn.dataTable.isDataTable("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabOrder_ResultSent #dgvLabOrdertSent")) {
            $("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabOrder_ResultSent #dgvLabOrdertSent").dataTable().fnClearTable();
            $("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabOrder_ResultSent #dgvLabOrdertSent").dataTable().fnDestroy();
        }
        $("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabOrder_ResultSent #dgvLabOrdertSent tbody").find("tr").remove();
        //End 24-05-2016 Muhammad Arshad Remove Duplicate search issue on Datatable
        //$("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabOrder_Result #dgvLabOrder").dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it
        //$("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabOrder_Result #dgvLabOrder tbody").find("tr").remove(); //Removing all the table data from table body
        if (response.LabOrderCount > 0) {


            //var headerRow = '<th class="size30"></th>'
            //$("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabOrder_ResultSent #dgvLabOrdertSent").find('thead').find('tr').prepend(headerRow);


            var LabLoadJSONData = JSON.parse(response.LabOrderFill_JSON);
            //JSON.parse(response.LabLoad_JSON); //Parsing array to JSON
            $.each(LabLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                // $row.attr("onclick", "OrderSet_LabOrderDetails.LabEdit('" + item.LabOrderId + "',event);");
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
                var onclick = "OrderSet_LabOrder.labOrderRowExpand(this,event,'sent');"
                //Start//21-04-2016//Ahmad Raza//logic to check uncheck order if it is attached with Note
                var expandCollapseIcon = '<a href="#" onclick="' + onclick + '" class="tab_space" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a>';


                var SelectionCheckBoxColumn = "";
                var Checked = "";
                if (item.IsNoteLinked == "True") {
                    if (OrderSet_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.LabOrderId + "ordr", OrderSet_ProgressNote.DetachedNoteComponentIds) > -1) {
                        Checked = " ";
                    } else {
                        Checked = " checked";
                    }
                } else {
                    //if (OrderSet_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.LabOrderId + "ordr", OrderSet_ProgressNote.AttachedNoteComponentIds) > -1) {
                    //    Checked = " checked";
                    //} else {
                    //    Checked = "";
                    //}
                }

                if (OrderSet_LabOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                    SelectionCheckBoxColumn = '<td><input type="checkbox" onclick="OrderSet_LabOrder.enableAddOrder(this,event);" id="' + item.LabOrderId + '" name="SelectCheckBoxOrder" ' + Checked + ' class="input-block"/></td>';
                } else {
                    SelectionCheckBoxColumn = "";
                }
                //End//21-04-2016//Ahmad Raza//logic to check uncheck order if it is attached with Note
                //Start//20-04-2016//Ahmad Raza//implimented logic to show icon for showing DBAudit History
                if (item.Status == "Signed") {

                    //Begin 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578
                    $row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.LabOrderId + '</td><td><a class="btn btn-xs disableAll" href="#" onclick="OrderSet_LabOrder.LabOrderDelete(\'' + item.LabOrderId + '\', event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="OrderSet_LabOrderDetails.LabEdit(\'' + item.LabOrderId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="OrderSet_LabOrder.printLabOrder(\'' + item.LabOrderId + '\',\'' + item.Status + '\',event );" title="View Result"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="OrderSet_LabOrder.showOrderHistory(\'' + item.LabOrderId + '\', event );" title="Record History"> <i class="fa fa-history blue"></i></a>&nbsp;<a title="Print Specimen Label" class="btn  btn-xs" href="#"  onclick="OrderSet_LabOrder.printSpecimen(\'' + item.LabOrderId + '\',\'' + item.Status + '\',event );" ><i class="fa fa-barcode"></i> </a></td><td>'
                    + utility.RemoveTimeFromDate(null, item.OrderDate) + ' ' + item.OrderTime + '</td><td>' + expandCollapseIcon + item.Test.split('|')[0] + '</td><td>' + item.LabName + '</td><td>' + item.OrderNo + '</td><td>' + item.Status + '</td><td>' + item.Provider + '</td><td>' + item.AssigneeName + '</td>');
                    //End 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578

                }
                else {

                    //Begin 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578
                    $row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.LabOrderId + '</td><td><a class="btn btn-xs" href="#" onclick="OrderSet_LabOrder.LabOrderDelete(\'' + item.LabOrderId + '\', event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="OrderSet_LabOrderDetails.LabEdit(\'' + item.LabOrderId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs disableAll" href="#" onclick="OrderSet_LabOrder.printLabOrder(\'' + item.LabOrderId + '\',\'' + item.Status + '\',event );" title="View Result"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="OrderSet_LabOrder.showOrderHistory(\'' + item.LabOrderId + '\', event );" title="Record History"> <i class="fa fa-history blue"></i></a>&nbsp;<a title="Print Specimen Label" class="btn  btn-xs" href="#"  onclick="OrderSet_LabOrder.printSpecimen(\'' + item.LabOrderId + '\',\'' + item.Status + '\',event );" ><i class="fa fa-barcode"></i> </a></td><td>'
                        + utility.RemoveTimeFromDate(null, item.OrderDate) + ' ' + item.OrderTime + '</td><td>' + expandCollapseIcon + item.Test.split('|')[0] + '</td><td>' + item.LabName + '</td><td>' + item.OrderNo + '</td><td>' + item.Status + '</td><td>' + item.Provider + '</td><td>' + item.AssigneeName + '</td>');
                    //End 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578
                }
                //End//20-04-2016//Ahmad Raza//implimented logic to show icon for showing DBAudit History
                $("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabOrder_ResultSent #dgvLabOrdertSent tbody").last().append($row);

                var childRows = OrderSet_LabOrder.buildLabOrderRowChild(item.LabOrderTests, item.LabOrderId);
                OrderSet_LabOrder.labOrderRowsSent.push({
                    row: $row, childs: childRows
                });

                utility.bindMyJSONByName(true, item, false, childRows).done(function () {
                    childRows.find('#orderNo').text("Order Number: " + item.OrderNo);
                    childRows.find('#laboratory').text("Lab: " + item.LabName);
                });


            });

        }
        else {

            $("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabOrder_ResultSent #dgvLabOrdertSent").DataTable({
                "destroy": true,
                "language": {
                    "emptyTable": "No Lab Order Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabOrder_ResultSent #dgvLabOrdertSent"))
            ;
        else {
            OrderSet_LabOrder.EditableGridOrderSent = $("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabOrder_ResultSent #dgvLabOrdertSent").DataTable({
                "destroy": true,
                "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            }); // to remove records per page dropdown
        }

        EMRUtility.fixDataTableDuplication("#" + OrderSet_LabOrder.params.PanelID + " #pnlLabOrder_ResultSent");

        $.each(OrderSet_LabOrder.labOrderRowsSent, function (i, item) {

            if (OrderSet_LabOrder.EditableGridOrderSent != null) {

                var row = OrderSet_LabOrder.EditableGridOrderSent.row(item.row);
                if (item.childs.length > 0) {
                    row.child(item.childs);
                }
                else {
                    //$(item.row).find('td:first').find('a').hide();
                }
            }
        });
    },



    buildLabOrderRowChild: function (tests, labOrderId) {

        var CurrentRowchilds = $();
        var templateHtml = $("#" + OrderSet_LabOrder.params.PanelID + " #LabOrderTemplate").clone();

        if (tests != null && tests.length > 0) {

            var $tbody = templateHtml.find('#dgvLabOrderTemplate').find('tbody');

            var onClick = "OrderSet_LabOrder.LabResultAddEdit(-1,'" + labOrderId + "',false)";
            $.each(tests, function (i, item) {

                var link = item.LabOrderResultDetails.length > 0 ? "View Result" : "Add Result";
                var i = 1;
                do {
                    var $ChilRowDetail = $('<tr/>').addClass("childRowDetail-bg");
                    if (i == 1) {
                        $ChilRowDetail.append('<td class="bold" colspan="2" >' + item.CPTCode + " " + item.CPTDescription + '</td> <td></td>');
                    }
                    else {
                        $ChilRowDetail.append('<td colspan="2" >' + item.CPTCode + " " + item.CPTDescription + '</td> <td ><a onclick="' + onClick + ' " href="#" >' + link + '</a></td>');
                    }
                    $tbody.append($ChilRowDetail);
                    i++;
                } while (i <= 2)
            });
        }

        var $row = $('<tr/>').addClass("childRow-bg");
        var cellpadding = '';

        $row.append('<td class="hidden"></td> <td class="hidden"> <td class="hidden"></td>  <td class="hidden"></td> <td colspan="9">' + templateHtml.html() + '</td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td> <td class="hidden"></td>');
        return CurrentRowchilds = CurrentRowchilds.add($row);

    },


    labOrderRowExpand: function ($row, event, gridType) {

        $row = $($row).parent().parent();

        var $actions,
         values = [];
        var row = null;
        if (gridType != null) {
            row = OrderSet_LabOrder.EditableGridOrderSent.row($row);
        }
        else {
            row = OrderSet_LabOrder.EditableGridOrder.row($row);
        }
        //if ($row.hasClass('adding')) {
        //    $row.removeClass('adding');
        //}
        if (row.child.isShown()) {
            // This row is already open - close it
            $row.find("td:eq(3) .fa-minus-square").attr("class", "fa fa-plus-square");
            row.child.hide();
            //tr.removeClass('shown');
        }
        else {
            $row.find("td:eq(3) .fa-plus-square").attr("class", "fa fa-minus-square");
            // Open this row
            row.child.show();
        }
        event.preventDefault();
    },



    //Function Name:enableAddOrder
    //Author Name: Ahmad Raza
    //Created Date: 21-04-2016
    //Description: this function will add/remove attached/detached orders in/from json
    enableAddOrder: function (obj, event) {
        if (event != null) {
            event.stopPropagation();
        }

        var totalRows = $("#" + OrderSet_LabOrder.params.PanelID + " #dgvLabOrdertSent tr").length;
        totalRows -= 1;
        var selectedRows = $("#" + OrderSet_LabOrder.params.PanelID + " #dgvLabOrdertSent tbody tr input:checked").length;
        if (totalRows == selectedRows) {
            $("#" + OrderSet_LabOrder.params.PanelID + " #dgvLabOrdertSent #selectRecordOrders").prop("checked", true);
        }
        else {
            $("#" + OrderSet_LabOrder.params.PanelID + " #dgvLabOrdertSent #selectRecordOrders").prop("checked", false);
        }

        if ($(obj).is(':checked')) {

            if ($.inArray(obj.id + 'ordr', OrderSet_ProgressNote.AttachedNoteComponentIds) == -1) {
                OrderSet_ProgressNote.AttachedNoteComponentIds.push(obj.id + 'ordr');
            }

            if ($.inArray(obj.id + 'ordr', OrderSet_ProgressNote.DetachedNoteComponentIds) != -1) {
                var index = OrderSet_ProgressNote.DetachedNoteComponentIds.indexOf(obj.id + 'ordr');
                if (index > -1) {
                    OrderSet_ProgressNote.DetachedNoteComponentIds.splice(index, 1);
                }
            }
        } else {

            var index = OrderSet_ProgressNote.AttachedNoteComponentIds.indexOf(obj.id + 'ordr');
            if (index > -1) {
                OrderSet_ProgressNote.AttachedNoteComponentIds.splice(index, 1);
            }
            if ($.inArray(obj.id + 'ordr', OrderSet_ProgressNote.DetachedNoteComponentIds) == -1) {
                OrderSet_ProgressNote.DetachedNoteComponentIds.push(obj.id + 'ordr');
            }
        }
    },

    getLabOrderInfo: function (labOrderId, detachedvalues, hideAlertMessage) {
        if (labOrderId == null || labOrderId == '') {
            return false;
        }
        var dfd = new $.Deferred();
        OrderSet_LabOrder.getOrdersForSOAP_DBCall(labOrderId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {

                    if (response.MedicationSoapCount > 0) {
                        OrderSet_LabOrder.createMedicationBodyHTML(response, '#' + OrderSet_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', labOrderId, detachedvalues, hideAlertMessage);

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
        objData["PatientId"] = OrderSet_LabOrder.params.patientID;
        objData["commandType"] = "get_Orders_forsoap";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "LabOrder");
    },
    checkMedicationsExists: function () {
        //if ($('#' + OrderSet_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML OrderSet_laborders').length == 0) {
        //    $('#' + OrderSet_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList').append(' <li> <header>' +
        //                '<OrderSet_laborders title="Lab Orders"  id="' + this.id + '" class="NotesComponent">' +
        //                '<a class="btn btn-link btn-xs" onclick="OrderSet_ProgressNote.SelectNotesComponentTab(\'LabOrders\',\'-1\',' + OrderSet_ProgressNote.params.NotesId + ');" title="Lab Order">Lab Order</a> ' +
        //                                '<a onclick="OrderSet_ProgressNote.RemoveComponentTab(\'LabOrders\',\'-1\',' + OrderSet_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
        //                           '</OrderSet_laborders> </header></li>');
        //    OrderSet_ProgressNote.CreateNotesComponent_Buttons($('#' + OrderSet_ProgressNote.params["PanelID"] + ' #hfNotesId').val());
        //    $('#' + OrderSet_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
        //        $(this).find('.closeBtn').removeClass('hidden');
        //        $(this).css('background', '#EAF1F8');
        //    });

        //    $('#' + OrderSet_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
        //        $(this).find('.closeBtn').addClass('hidden');
        //        $(this).css('background', '#fff');
        //    });
        //}


        if ($('#' + OrderSet_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML OrderSet_laborders').length == 0) {
            var CompnentSelector = '#' + OrderSet_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #PlanNoteComponentList';
            if (OrderSet_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + OrderSet_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="LabOrdersComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<OrderSet_laborders title="Lab Orders"  id="clinicalMenu_Orders_Lab" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="OrderSet_ProgressNote.SelectNotesComponentTab(\'LabOrders\',\'clinicalMenu_Orders_Lab\',' + OrderSet_ProgressNote.params.NotesId + ');" title="Lab Orders">Lab Orders</a> ' +
                        '<a onclick="OrderSet_ProgressNote.Edit_ComponentAttached(this);" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="OrderSet_ProgressNote.RemoveComponentTab(\'Lab Orders\',\'clinicalMenu_Orders_Lab\',' + OrderSet_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</OrderSet_laborders> </header></li>');
            OrderSet_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
            $('#' + OrderSet_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
                $(this).find('.closeBtn').removeClass('hidden');
                $(this).find('.btnPNC_Edit').removeClass('hidden');
                $(this).css('background', '#EAF1F8');
            });

            $('#' + OrderSet_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
                $(this).find('.closeBtn').addClass('hidden');
                $(this).find('.btnPNC_Edit').addClass('hidden');
                $(this).css('background', '#fff');
            });



        }
    },
    checkResultExists: function () {
        if ($('#' + OrderSet_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML OrderSet_labresults').length == 0) {

            var CompnentSelector = '#' + OrderSet_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ObjectiveNoteComponentList';
            if (OrderSet_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + OrderSet_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="LabResultComponent"> <header>' +
                        '<OrderSet_labresults title="Lab Results"  id="' + this.id + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="OrderSet_ProgressNote.SelectNotesComponentTab(\'LabResults\',\'-1\',' + OrderSet_ProgressNote.params.NotesId + ');" title="Lab Results">Lab Results</a> ' +
                        '<a onclick="OrderSet_ProgressNote.Edit_ComponentAttached(this);" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="OrderSet_ProgressNote.RemoveComponentTab(\'LabResults\',\'-1\',' + OrderSet_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</OrderSet_labresults> </header></li>');
            OrderSet_ProgressNote.CreateNotesComponent_Buttons($('#' + OrderSet_ProgressNote.params["PanelID"] + ' #hfNotesId').val());
            $('#' + OrderSet_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
                $(this).find('.closeBtn').removeClass('hidden');
                $(this).find('.btnPNC_Edit').removeClass('hidden');
                $(this).css('background', '#EAF1F8');
            });

            $('#' + OrderSet_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
                $(this).find('.closeBtn').addClass('hidden');
                $(this).find('.btnPNC_Edit').addClass('hidden');
                $(this).css('background', '#fff');
            });


        }
    },
    createMedicationBodyHTML: function (response, noteHTMLCtrl, medicationsId, detachedvalues, hideAlertMessage) {
        OrderSet_LabOrder.checkMedicationsExists();
        if (response.MedicationSoap_JSON != null && response.MedicationSoap_JSON != '') {
            var MedicationsSOAPJSON = JSON.parse(response.MedicationSoap_JSON);
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
                if ($(noteHTMLCtrl + ' OrderSet_laborders').parent().parent().find('#Cli_LabOrderDetail_Main0').length != 0) {
                    $(noteHTMLCtrl + ' #Cli_LabOrderDetail_Main0').parent().remove();
                }
            }

            var AListId = [];
            $.each(MedicationsSOAPJSON, function (index, element) {

                var ALid = element.LabOrderId;
                //             var $infoButtonrow = OrderSet_InfoButtonView.GenerateInfoLink(element.NDCID, 'clinicalTabProgressNote', 1);
                var $SectionBodyMedications = $(document.createElement('section'));
                $SectionBodyMedications.attr('id', "Cli_LabOrderDetail_Main" + ALid);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_LabOrderDetail_" + ALid);
                var $ListMedications = $(document.createElement('ul'));
                var duration = "";
                $ListMedications.attr('class', 'list-unstyled')

                $SectionBodyMedications.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_LabOrderDetail_" + ALid + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_LabOrderDetail_Main" + ALid + '"  ><i class="fa fa-times"></i></a></div> ');
                var obj = $('<p>');
                $(obj).html(element.SoapText);
                //$(obj).html($(obj).text())
                $ListMedications.append("<li>" + $(obj).text() + "</li>");

                //             $ListMedications.append((element.Comments == "" ? "" : "<li>Comments: " + element.Comments));
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
                //if ($('#' + OrderSet_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML OrderSet_medications').parent().parent().find('#Cli_Medications_Main' + ALid).length == 0) {
                if ($(noteHTMLCtrl + ' OrderSet_laborders').parent().parent().find('#Cli_LabOrderDetail_Main' + ALid).length == 0) {
                    AListId.push(ALid);
                    $mainDivMedications.append($SectionBodyMedications);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(noteHTMLCtrl + ' OrderSet_laborders').parent().parent().find('#Cli_LabOrderDetail_Main' + ALid + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(noteHTMLCtrl + ' OrderSet_laborders').parent().parent().find('#Cli_LabOrderDetail_Main' + ALid + ' ul li:Last').get(0).outerHTML;
                    }
                    $(noteHTMLCtrl + ' OrderSet_laborders').parent().parent().find('#Cli_LabOrderDetail_Main' + ALid).html($SectionBodyMedications.html());
                    $(noteHTMLCtrl + ' OrderSet_laborders').parent().parent().find('#Cli_LabOrderDetail_Main' + ALid + ' ul').append(CommentHTML);;
                }


            });

            if (AListId.join(",") != "") {
                medicationsId = AListId.join(",");
            }
            if ($mainDivMedications.html() != '') {
                //Start 25-11-2016 Edit By Humaira Yousaf Bug# IMP-9
                OrderSet_LabOrder.updateMedicationsHtml($mainDivMedications.html(), medicationsId, noteHTMLCtrl, null, hideAlertMessage);
                //End 25-11-2016 Edit By Humaira Yousaf Bug# IMP-9
            } else {
                OrderSet_LabOrder.updateMedicationsHtml('', medicationsId, noteHTMLCtrl, detachedvalues, hideAlertMessage);
                //OrderSet_ProgressNote.updateProgressNoteHTML();
            }
        }

    },

    updateMedicationsHtml: function (medicationsHTML, medicationsID, noteHTMLCtrl, detachedvalues, hideAlertMessage) {
        $(noteHTMLCtrl + ' OrderSet_laborders').parent().parent().addClass('initialVisitBody');
        if (medicationsHTML != '') {
            $(noteHTMLCtrl + ' OrderSet_laborders').parent().parent().append(medicationsHTML);
        }
        //Binding Hovering and onClick functions to Progress Note HTML
        OrderSet_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (medicationsHTML != '' && medicationsID != null && medicationsID != '' && medicationsID != '0') {
            OrderSet_LabOrder.attachMedicationFromNotes(medicationsID, hideAlertMessage);
        } else if (detachedvalues != null && detachedvalues != '') {
            OrderSet_ProgressNote.updateProgressNoteHTML(null, null, hideAlertMessage);
        }
    },
    updateResultHtml: function (resultHTML, resultID, noteHTMLCtrl, hidemessage) {
        $(noteHTMLCtrl + ' OrderSet_labresults').parent().parent().addClass('initialVisitBody');
        if (resultHTML != '') {
            $(noteHTMLCtrl + ' OrderSet_labresults').parent().parent().append(resultHTML);
        }

        //Binding Hovering and onClick functions to Progress Note HTML
        OrderSet_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (resultHTML != '' && resultID != null && resultID != '' && resultID != '0') {
            OrderSet_LabOrder.attachResultFromNotes(resultID, hidemessage);
        }

    },

    attachMedicationFromNotes: function (medicationID, hidemessage) {
        var selectedValue = medicationID;
        if (selectedValue == "" || selectedValue == "undefined") {
        }
        else {
            OrderSet_LabOrder.attachMedicationsWithNotes_DBCall(selectedValue).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    OrderSet_ProgressNote.updateProgressNoteHTML(null, null, hidemessage);
                    //Start 01-12-2016 Humaira Yousaf to hide/show eSuperbill link
                    OrderSet_ProgressNote.HideShowBillingInfo();
                    //End 01-12-2016 Humaira Yousaf to hide/show eSuperbill link
                    $('#' + medicationID).remove();

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    attachResultFromNotes: function (resultID, hidemessage) {
        var selectedValue = resultID;
        if (selectedValue == "" || selectedValue == "undefined") {
        }
        else {
            OrderSet_LabOrder.attachResultsWithNotes_DBCall(selectedValue).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    OrderSet_ProgressNote.updateProgressNoteHTML(null, null, hidemessage);
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
        objData["LabOrderIDs"] = medicationId;
        objData["commandType"] = "attach_orders_with_notes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "LabOrder");
    },
    attachResultsWithNotes_DBCall: function (resultId) {
        var objData = new Object();
        objData["LabResultIDs"] = resultId;
        objData["commandType"] = "attach_results_with_notes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },


    addOrdersToNotes: function (selectedAttachedOrders, selectedDetachedOrders, hideAlertMessage) {
        var noteHTMLCtrl = '#' + OrderSet_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML';
        if ($("#dgvLabOrdertSent").dataTable().fnSettings().aoData.length == 0) {
            //   OrderSet_LabOrder.noActiveLabOrderSoapText();
        }
        else {

            if ($("#" + OrderSet_LabOrder.params.PanelID + " #selectRecordOrders").prop('checked') == true || $("#" + OrderSet_LabOrder.params.PanelID + " #selectRecordOrders").prop('checked') == false) {

                $("#" + OrderSet_LabOrder.params.PanelID + " #dgvLabOrdertSent tbody").find('input[type="checkbox"]').each(function () {
                    OrderSet_LabOrder.enableAddOrder(this, null);
                });
            }


            var AttachedSelectedOrders = [];
            var DettachedSelectedOrders = [];
            if ((selectedAttachedOrders != '' && selectedAttachedOrders != null) || (selectedDetachedOrders != '' && selectedDetachedOrders != null)) {
                AttachedSelectedOrders = selectedAttachedOrders;
                DettachedSelectedOrders = selectedDetachedOrders;
            } else {
                var AttachSelectedOrdersAndResults = OrderSet_ProgressNote.AttachedNoteComponentIds.slice();
                var DettachSelectedOrdersAndResults = OrderSet_ProgressNote.DetachedNoteComponentIds.slice();
                //Check for Medications Values
                //if (AttachSelectedOrdersAndResults.join().indexOf("prsc") != -1 || DettachSelectedOrdersAndResults.join().indexOf("prsc") != -1) {
                //    var AttachSelectedResults = EMRUtility.slicefunc(AttachSelectedOrdersAndResults, "prsc", 0, -4);
                //    var dettachSelectedResults = EMRUtility.slicefunc(DettachSelectedOrdersAndResults, "prsc", 0, -4);
                //    OrderSet_Prescriptions.addPrescriptionsToNotes(AttachSelectedResults, dettachSelectedResults);
                //}
                AttachedSelectedOrders = EMRUtility.slicefunc(AttachSelectedOrdersAndResults, "ordr", 0, -4);
                DettachedSelectedOrders = EMRUtility.slicefunc(DettachSelectedOrdersAndResults, "ordr", 0, -4);
            }
            var detachedvalues = DettachedSelectedOrders;

            if (AttachedSelectedOrders != null && AttachedSelectedOrders != '') {
                for (var i = 0; i < OrderSet_ProgressNote.AttachedNoteComponentIds.length; i++) {
                    var ALid = OrderSet_ProgressNote.AttachedNoteComponentIds[i];
                    if ($('#' + OrderSet_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_LabOrderDetail_Main' + ALid).length != 0) {
                        var index = AttachedSelectedOrders.indexOf(ALid);
                        if (index > -1) {
                            AttachedSelectedOrders.splice(index, 1);
                        }
                    }
                }
            }
            if (detachedvalues.join() != null && detachedvalues.join() != '') {
                OrderSet_LabOrder.detachedLabOrder(detachedvalues).done(function () {
                    if (AttachedSelectedOrders.join() != null && AttachedSelectedOrders.join() != '') {
                        OrderSet_LabOrder.attachedLabOrder(AttachedSelectedOrders, detachedvalues, hideAlertMessage);
                    } else {
                        OrderSet_ProgressNote.updateProgressNoteHTML(null, null, hideAlertMessage);
                    }
                });
            }
            else if (AttachedSelectedOrders.join() != null && AttachedSelectedOrders.join() != '') {
                OrderSet_LabOrder.attachedLabOrder(AttachedSelectedOrders, detachedvalues, hideAlertMessage);
            }
        }
    },

    attachedLabOrder: function (AttachedSelectedOrders, detachedvalues, hideAlertMessage) {
        OrderSet_LabOrder.getLabOrderInfo(AttachedSelectedOrders.join(), detachedvalues, hideAlertMessage).done(function () {
            setTimeout(function () {
                OrderSet_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                if (OrderSet_LabOrder.params != null && OrderSet_LabOrder.params.PanelID.indexOf('pnlOrderSetLabOrder') != -1) {
                    OrderSet_LabOrder.LabOrderSearch('0', null, null, null, 'Signed');
                }
            }, 5);
        });
    },
    detachedLabOrder: function (detachedvalues) {
        var dfd = new $.Deferred();

        OrderSet_LabOrder.detachMedicationsFromNotes_DBCall(detachedvalues.join()).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                for (var i = 0; i < detachedvalues.length; i++) {
                    var ALid = detachedvalues[i];
                    $('#' + OrderSet_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_LabOrderDetail_Main' + ALid).remove();
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
    //    var noteHTMLCtrl = '#' + OrderSet_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML';
    //    if ($(noteHTMLCtrl + ' OrderSet_radiologyorder').parent().parent().find('#Cli_RadiologyOrderDetail_Main0').length == 0) {
    //        OrderSet_LabOrder.checkMedicationsExists();
    //        var htmlForNoMedication = '<section id="Cli_RadiologyOrderDetail_Main0"> <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="Cli_RadiologyOrderDetail_0"><i class="fa fa-edit"></i></a>' +
    //'<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="Cli_RadiologyOrderDetail_Main0"  ><i class="fa fa-times"></i></a></div> ' +
    //'<div id="Cli_RadiologyOrderDetail_0"><ul class="list-unstyled"><li> No Radiology Order Found</li></ul></div></section>';
    //        OrderSet_LabOrder.updateMedicationsHtml(htmlForNoMedication, '0', noteHTMLCtrl);
    //        OrderSet_ProgressNote.updateProgressNoteHTML();
    //    }
    //},
    detachMedicationsFromNotes_DBCall: function (medicationsId) {
        var objData = new Object();
        objData["LabOrderIDs"] = medicationsId;
        objData["commandType"] = "detach_orders_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "OrderSet", "LabOrder");
    },

    //Author: Abid Ali
    //Date :  31-03-2016.
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
        objData["PatientId"] = OrderSet_LabOrder.patientId;

        if (OrderStatus == "Signed") {
            if ($('#' + OrderSet_LabOrder.params.PanelID + ' div[id*="PatientLabOrderSent"] #txtCPTCode').val() == undefined || $('#' + OrderSet_LabOrder.params.PanelID + ' div[id*="PatientLabOrderSent"] #txtCPTCode').val() == "" || $('#' + OrderSet_LabOrder.params.PanelID + ' div[id*="PatientLabOrderSent"] #txtCPTCode').val().toLowerCase() == "no record found")
                objData["Test"] = "";
            else
                objData["Test"] = objData["CPTCodeId"];

            objData["LabId"] = $('#' + OrderSet_LabOrder.params.PanelID + ' div[id*="PatientLabOrderSent"] #frmOrderSetLabOrder #ddlLaboratory').val();
            objData["ProviderId"] = $('#' + OrderSet_LabOrder.params.PanelID + ' div[id*="PatientLabOrderSent"]  #frmOrderSetLabOrder #ddlProvider').val();
            objData["OrderFromDate"] = $('#' + OrderSet_LabOrder.params.PanelID + ' div[id*="PatientLabOrderSent"]  #frmOrderSetLabOrder #dpStartDate').val();
            objData["OrderToDate"] = $('#' + OrderSet_LabOrder.params.PanelID + ' div[id*="PatientLabOrderSent"]  #frmOrderSetLabOrder #dpToDate').val();
            objData["OrderNo"] = $('#' + OrderSet_LabOrder.params.PanelID + ' div[id*="PatientLabOrderSent"]  #frmOrderSetLabOrder #txtOrderNumber').val();
        } else {
            objData["Test"] = $('#' + OrderSet_LabOrder.params.PanelID + ' div[id*="PatientLabOrderPending"] #frmOrderSetLabOrder #txtCPTCode').val();
            objData["LabId"] = $('#' + OrderSet_LabOrder.params.PanelID + ' div[id*="PatientLabOrderPending"] #frmOrderSetLabOrder #ddlLaboratory').val();
            objData["ProviderId"] = $('#' + OrderSet_LabOrder.params.PanelID + ' div[id*="PatientLabOrderPending"] #frmOrderSetLabOrder #ddlProvider').val();
            objData["OrderFromDate"] = $('#' + OrderSet_LabOrder.params.PanelID + ' div[id*="PatientLabOrderPending"] #frmOrderSetLabOrder #dpStartDate').val();
            objData["OrderToDate"] = $('#' + OrderSet_LabOrder.params.PanelID + ' div[id*="PatientLabOrderPending"]  #frmOrderSetLabOrder #dpToDate').val();
            objData["OrderNo"] = $('#' + OrderSet_LabOrder.params.PanelID + ' div[id*="PatientLabOrderPending"]  #frmOrderSetLabOrder #txtOrderNumber').val();
        }



        if (OrderStatus != null) {
            if (OrderStatus == "Pending") {
                objData["Status"] = "Pending";
            }
            else {
                objData["Status"] = "Signed";
            }
        }
        else {
            objData["Status"] = "Pending";//$('#' + OrderSet_LabOrder.params.PanelID + ' #frmOrderSetLabOrder #ddlStatus option:selected').val();
        }





        // objData["commandType"] = "search_Laborders";
        objData["commandType"] = "search_laborders_dashboard";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "LabOrder");
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
        objData["PatientId"] = OrderSet_LabOrder.patientId;
        objData["Test"] = objData["ResultCPTCode"];
        objData["ProviderId"] = $('#' + OrderSet_LabOrder.params.PanelID + ' #frmOrderSetLabResult #ddlProvider').val();
        objData["LabId"] = $('#' + OrderSet_LabOrder.params.PanelID + ' #frmOrderSetLabResult #ddlLaboratory').val();
        objData["OrderFromDate"] = $('#' + OrderSet_LabOrder.params.PanelID + ' #frmOrderSetLabResult #dpStartDate').val();
        objData["Status"] = $('#' + OrderSet_LabOrder.params.PanelID + ' #frmOrderSetLabResult #ddlStatus option:selected').val();
        objData["OrderToDate"] = $('#' + OrderSet_LabOrder.params.PanelID + ' #frmOrderSetLabResult #dpToDate').val();
        objData["OrderNo"] = $('#' + OrderSet_LabOrder.params.PanelID + ' #frmOrderSetLabResult #txtOrderNumber').val();

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
            $('#' + OrderSet_LabOrder.params.PanelID + ' #frmOrderSetLabOrder').resetAllControls(null);
            OrderSet_LabOrder.ValidateLab();
        }, function () { },
            '22'
        );
    },

    //Author: Muhamamd Arshad
    //Date :  29-04-2016
    //Description: Lab Result Reset
    LabResultReset: function () {

        utility.myConfirm('22', function () {
            $('#' + OrderSet_LabOrder.params.PanelID + ' #frmOrderSetLabResult').resetAllControls(null);
            OrderSet_LabOrder.ValidateLab();
        }, function () { },
            '22'
        );
    },
    //Author: Ahmad Raza
    //Date :  21-06-2016
    //Function Name: validateSpecialCharacters
    //Description: This function will validate the special characters
    validateSpecialCharacters: function (event) {
        var valid = (event.which >= 48 && event.which <= 57) || (event.which >= 65 && event.which <= 90) || (event.which >= 97 && event.which <= 122);
        if (!valid) {
            event.preventDefault();
        }

    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Unload LabOrder
    UnLoadTab: function () {
        var objDeffered = $.Deferred();

        /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        if (OrderSet_LabOrder.params.ParentCtrl == "clinicalTabFaceSheet") {

            utility.UnLoadDialog(OrderSet_LabOrder.params.PanelID + ' #frmClinicalCDS', function () {


                //$("#" + OrderSet_LabOrder.params.PanelID + " #" + OrderSet_LabOrder.orderSearchGridId).attr('id', 'dgvLabOrder')
                //$("#" + OrderSet_LabOrder.params.PanelID + " #" + OrderSet_LabOrder.resultSearchGridId).attr('id', 'dgvLabResult')

                if (OrderSet_LabOrder.params["FromAdmin"] == "0") {
                    if (OrderSet_LabOrder.params != null && OrderSet_LabOrder.params.ParentCtrl != null) {
                        UnloadActionPan(OrderSet_LabOrder.params.ParentCtrl, 'OrderSet_LabOrder');
                    }
                    else
                        UnloadActionPan(null, 'OrderSet_LabOrder');
                }
                else {
                    $("#mstrDivMedical #clinicalMenu_Medical_CDS").remove();
                    RemoveAdminTab();
                }
                objDeffered.resolve();
            }, function () {
                if (OrderSet_LabOrder.params["FromAdmin"] == "0") {
                    if (OrderSet_LabOrder.params != null && OrderSet_LabOrder.params.ParentCtrl != null) {
                        UnloadActionPan(OrderSet_LabOrder.params.ParentCtrl, 'OrderSet_LabOrder');
                    }
                    else
                        UnloadActionPan(null, 'OrderSet_LabOrder');
                }
                else {
                    $("#mstrDivMedical #clinicalMenu_Medical_CDS").remove();
                    RemoveAdminTab();
                }
                objDeffered.resolve();
            });
            OrderSet_FaceSheet.loadFaceSheet();
        }
            /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        else {
            if (OrderSet_LabOrder.params["FromAdmin"] == "0") {
                if (OrderSet_LabOrder.params != null && OrderSet_LabOrder.params.ParentCtrl != null) {
                    if ($("#" + OrderSet_LabOrder.params.PanelID + " #PatientlistLabOrderSent").hasClass('active')) {
                        OrderSet_LabOrder.addOrdersToNotes();
                    }
                    if ($("#" + OrderSet_LabOrder.params.PanelID + " #PatientlistLabResult").hasClass('active')) {
                        OrderSet_LabOrder.addResultsToNotes();
                    }
                    UnloadActionPan(OrderSet_LabOrder.params.ParentCtrl, 'OrderSet_LabOrder');
                }
                else
                    OrderSet_LabOrder.addResultsToNotes();
                UnloadActionPan(null, 'OrderSet_LabOrder');
            }
            else {
                $("#mstrDivMedical #clinicalMenu_Medical_CDS").remove();
                RemoveAdminTab();
            }
            objDeffered.resolve();
        }
        return objDeffered;
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Opens Provider
    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmOrderSetLabOrder";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "OrderSetTabLabOrder";
        LoadActionPan('Admin_Provider', params);
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Opens Provider
    openproviderdetail: function () {
        var params = [];
        params["providerid"] = $('#pnlOrderSetLabOrder #hfprovider').val();
        params["mode"] = "edit";
        params["fromadmin"] = "0";
        params["refctrl"] = "ddlProvider";
        params["parentctrl"] = 'OrderSet_LabOrder';
        loadactionpan('providerdetail', params);
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: Delete the selected Lab Order
    LabOrderDelete: function (LabId, event, PageNo, rpp, PatientId, TabID) {
        var strMessage = "";
        event.stopPropagation();
        AppPrivileges.GetFormPrivileges("Orders and Results_Lab Order", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = LabId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        var self = $("#" + OrderSet_LabOrder.params.PanelID + " form");
                        var myJSON = self.getMyJSONByName();
                        OrderSet_LabOrder.deleteLabOrder(myJSON, LabId, PageNo, rpp).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                utility.DisplayMessages(response.Message, 1);
                                if (TabID == "mstrTabDashBoard") {
                                    var btnDashBoardLabOrderSearch = $("#pnlDashboard #btnLabSearch").trigger("click");
                                    //var btnDashBoardLabResultSearch = $("#pnlDashboard #btnLabResultSearch").trigger("click");
                                    //var btnDashBoardLabResultUnsolicitedSearch = $("#pnlDashboard #btnLabUnsoResultSearch");
                                    //var onclickLabOrder = btnDashBoardLabOrderSearch.attr("onclick");
                                    //var onclickLabResult = btnDashBoardLabResultSearch.attr("onclick");
                                    //var onclickLabResultUnsolicited = btnDashBoardLabResultUnsolicitedSearch.attr("onclick");
                                    //DashBoard.DashBoardLabOrderGridLoad(null, 1, 15);
                                }
                                else {
                                    OrderSet_LabOrder.LabOrderSearch("", 1, 15);
                                    OrderSet_LabOrder.LabResultsSearch("", 1, 15);
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
    LabResultDelete: function (LabId, event, PageNo, rpp, PatientId, TabID, IsUnsolicited) {
        var strMessage = "";
        event.stopPropagation();
        AppPrivileges.GetFormPrivileges("Orders and Results_Lab Result", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = LabId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        var self = $("#" + OrderSet_LabOrder.params.PanelID + " form");
                        var myJSON = self.getMyJSONByName();

                        OrderSet_LabOrder.deleteLabResult(myJSON, LabId, PageNo, rpp).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                utility.DisplayMessages(response.Message, 1);
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
                                        var btnDashBoardLabResultSearch = $("#pnlDashboard #btnLabResultSearch").trigger("click");
                                    }

                                    //var btnDashBoardLabResultUnsolicitedSearch = $("#pnlDashboard #btnLabUnsoResultSearch");
                                }
                                else {
                                    OrderSet_LabOrder.LabOrderSearch();
                                    OrderSet_LabOrder.LabResultsSearch();
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
        return MDVisionService.APIService(data, "OrderSet", "LabOrder");

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
        if (status == 'Signed') {
            var params = [];
            params["FromAdmin"] = "0";
            params["UserId"] = globalAppdata['AppUserId'];
            var currentPatientId = PatientId != null && parseInt(PatientId) > 0 ? PatientId : $('#PatientProfile #hfPatientId').val();
            params["PatientId"] = currentPatientId;
            // params["ParentCtrl"] = OrderSet_LabOrder.params.TabID;
            params["ParentCtrl"] = (OrderSet_LabOrder.params.ParentCtrl != "clinicalTabProgressNote" && OrderSet_LabOrder.params.ParentCtrl != "clinicalTabFaceSheet" && OrderSet_LabOrder.params.ParentCtrl != null) ? OrderSet_LabOrder.params.TabID : "OrderSet_LabOrder";
            if ($('#mstrTabDashBoard').hasClass('active')) {
                params["ParentCtrl"] = OrderSet_LabOrder.params.TabID;
            }
            params["LabOrderId"] = LabId;
            utility.myConfirm('Would you like to print the Specimen Label for this order?', function () {
                if (OrderSet_LabOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
                    params["ParentCtrlPanelID"] = OrderSet_LabOrder.params.PanelID;
                    params["BarCodeHtml"] = 'true';
                    params["IsSpecimen"] = true;
                    LoadActionPan('OrderSet_LabOrderView', params, OrderSet_LabOrder.params.PanelID);

                } else {
                    params["BarCodeHtml"] = 'true';
                    params["IsSpecimen"] = true;

                    LoadActionPan('OrderSet_LabOrderView', params);
                }



            }, function () {
                if (OrderSet_LabOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
                    params["BarCodeHtml"] = '';
                    params["ParentCtrlPanelID"] = OrderSet_LabOrder.params.PanelID;
                    LoadActionPan('OrderSet_LabOrderView', params, OrderSet_LabOrder.params.PanelID);
                } else {
                    params["BarCodeHtml"] = '';
                    LoadActionPan('OrderSet_LabOrderView', params);
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

        // if (status == 'Signed') {
        var params = [];
        //params["FromAdmin"] = "0";
        //params["UserId"] = globalAppdata['AppUserId'];
        //var currentPatientId = PatientId != null && parseInt(PatientId) > 0 ? PatientId : $('#PatientProfile #hfPatientId').val();
        //params["PatientId"] = currentPatientId;
        //// params["ParentCtrl"] = OrderSet_LabOrder.params.TabID;
        //params["ParentCtrl"] = (OrderSet_LabOrder.params.ParentCtrl != "clinicalTabProgressNote" && OrderSet_LabOrder.params.ParentCtrl != "clinicalTabFaceSheet") ? OrderSet_LabOrder.params.TabID : "OrderSet_LabOrder";
        //params["LabOrderId"] = LabId;
        //params["LabResultId"] = ResultId;
        //LoadActionPan('OrderSet_LabResultView', params);


        utility.myConfirm('Would you like to print the Specimen Label for this result?', function () {
            var params = [];
            params["FromAdmin"] = "0";
            params["UserId"] = globalAppdata['AppUserId'];
            var currentPatientId = PatientId != null && parseInt(PatientId) > 0 ? PatientId : $('#PatientProfile #hfPatientId').val();
            params["PatientId"] = currentPatientId;

            params["ParentCtrl"] = (OrderSet_LabOrder.params.ParentCtrl != "clinicalTabProgressNote" && OrderSet_LabOrder.params.ParentCtrl != "clinicalTabFaceSheet") ? OrderSet_LabOrder.params.TabID : "OrderSet_LabOrder";

            var PanelID = "";
            if (comeFrom == "") {
                if (OrderSet_LabOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                    PanelID = 'pnlClinicalProgressNote #pnlOrderSetLabOrder';
                    params["PanelID"] = 'pnlClinicalProgressNote #pnlOrderSetLabOrder';
                    params["PrPanelID"] = 'pnlClinicalProgressNote #pnlOrderSetLabOrder';
                }
                else if (OrderSet_LabOrder.params.ParentCtrl == "clinicalTabFaceSheet") {
                    PanelID = 'pnlClinicalFaceSheet #pnlOrderSetLabOrder';
                    params["PanelID"] = 'pnlClinicalFaceSheet #pnlOrderSetLabOrder';
                    params["PrPanelID"] = 'pnlClinicalFaceSheet #pnlOrderSetLabOrder';
                }
                else {
                    PanelID = 'pnlOrderSetLabOrder';
                    params["PanelID"] = 'pnlOrderSetLabOrder';
                    params["PrPanelID"] = 'pnlOrderSetLabOrder';
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
            params["BarCodeHtml"] = 'true';
            params["LabResultId"] = LabResultId;
            params["LabOrderId"] = LabOrderId;
            LoadActionPan('OrderSet_LabResultView', params, PanelID);
        }, function () {
            var params = [];
            params["FromAdmin"] = "0";
            params["UserId"] = globalAppdata['AppUserId'];
            var currentPatientId = PatientId != null && parseInt(PatientId) > 0 ? PatientId : $('#PatientProfile #hfPatientId').val();
            params["PatientId"] = currentPatientId;
            params["ParentCtrl"] = (OrderSet_LabOrder.params.ParentCtrl != "clinicalTabProgressNote" && OrderSet_LabOrder.params.ParentCtrl != "clinicalTabFaceSheet") ? OrderSet_LabOrder.params.TabID : "OrderSet_LabOrder";
            //if (OrderSet_LabOrder.params.ParentCtrl == "clinicalTabProgressNote") {
            //    params["ParentCtrl"] = "clinicalTabProgressNote"
            //}

            var PanelID = "";
            if (OrderSet_LabOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                PanelID = 'pnlClinicalProgressNote #pnlOrderSetLabOrder';
                params["PanelID"] = 'pnlClinicalProgressNote #pnlOrderSetLabOrder';
                params["PrPanelID"] = 'pnlClinicalProgressNote #pnlOrderSetLabOrder';
            }
            else if (OrderSet_LabOrder.params.ParentCtrl == "clinicalTabFaceSheet") {
                PanelID = 'pnlClinicalFaceSheet #pnlOrderSetLabOrder';
                params["PanelID"] = 'pnlClinicalFaceSheet #pnlOrderSetLabOrder';
                params["PrPanelID"] = 'pnlClinicalFaceSheet #pnlOrderSetLabOrder';
            }
            else {
                PanelID = 'pnlOrderSetLabOrder';
                params["PanelID"] = 'pnlOrderSetLabOrder';
                params["PrPanelID"] = 'pnlOrderSetLabOrder';
            }

            var LabResultId = ResultId;
            var LabOrderId = LabId;
            params["BarCodeHtml"] = 'false';
            params["LabResultId"] = LabResultId;
            params["LabOrderId"] = LabOrderId;
            LoadActionPan('OrderSet_LabResultView', params, PanelID);
        },
                     'Specimen Label Printing');


        //   }

    },

    //Function Name: showOrderHistory
    //Author Name: Ahmad Raza
    //Created Date: 20-04-2016
    //Description: to show Lab order history
    showOrderHistory: function (labOrderId, event) {
        if (event != null) {
            event.stopPropagation();
        }

        var ParentCtrl = 'OrderSetTabLabOrder';
        var ParentCtrlPanelID = null;
        if (OrderSet_LabOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
            ParentCtrl = 'OrderSet_LabOrder';
            ParentCtrlPanelID = OrderSet_LabOrder.params.PanelID;
        }
        else if (OrderSet_LabOrder.params["ParentCtrl"] == "clinicalTabFaceSheet") {
            ParentCtrl = 'OrderSet_LabOrder';
            ParentCtrlPanelID = OrderSet_LabOrder.params.PanelID;
        }

        EMRUtility.showCurrentItemHistory(OrderSet_LabOrder.params.PanelID, null, null, "LabOrder,LabOrderTest,LabOrderProblem", null, ParentCtrl, labOrderId, ParentCtrlPanelID);
    },


    //-----------Result attach.detach


    enableAddResult: function (obj, event) {
        if (event != null) {
            event.stopPropagation();
        }

        var totalRows = $("#" + OrderSet_LabOrder.params.PanelID + " #dgvLabOrdertSent tr").length;
        totalRows -= 1;
        var selectedRows = $("#" + OrderSet_LabOrder.params.PanelID + " #dgvLabOrdertSent tbody tr input:checked").length;

        if (totalRows == selectedRows) {
            $("#" + OrderSet_LabOrder.params.PanelID + " #dgvLabOrdertSent #selectRecordOrders").prop("checked", true);
        }
        else {
            $("#" + OrderSet_LabOrder.params.PanelID + " #dgvLabOrdertSent #selectRecordOrders").prop("checked", false);
        }

        if ($(obj).is(':checked')) {
            if ($.inArray(obj.id + 'rslt', OrderSet_ProgressNote.AttachedNoteComponentIds) == -1) {
                if (obj.id != 'chkSentToPortal') {
                    OrderSet_ProgressNote.AttachedNoteComponentIds.push(obj.id + 'rslt');
                }
            } if ($.inArray(obj.id + 'rslt', OrderSet_ProgressNote.DetachedNoteComponentIds) != -1) {
                var index = OrderSet_ProgressNote.DetachedNoteComponentIds.indexOf(obj.id + 'rslt');
                if (index > -1) {
                    OrderSet_ProgressNote.DetachedNoteComponentIds.splice(index, 1);
                }
            }
        } else {

            var index = OrderSet_ProgressNote.AttachedNoteComponentIds.indexOf(obj.id + 'rslt');
            if (index > -1) {
                OrderSet_ProgressNote.AttachedNoteComponentIds.splice(index, 1);
            }
            if ($.inArray(obj.id + 'rslt', OrderSet_ProgressNote.DetachedNoteComponentIds) == -1) {
                if (obj.id != 'chkSentToPortal') {
                    OrderSet_ProgressNote.DetachedNoteComponentIds.push(obj.id + 'rslt');
                }
            }
        }
    },

    addResultsToNotes: function (selectedAttachedOrders, selectedDetachedOrders, hideAlertMessage) {
        var noteHTMLCtrl = '#' + OrderSet_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML';
        if ($("#" + OrderSet_LabOrder.resultSearchGridId).dataTable().fnSettings().aoData.length == 0) {
            //   OrderSet_LabOrder.noActiveLabOrderSoapText();
        }
        else {

            if ($("#" + OrderSet_LabOrder.params.PanelID + " #selectRecordOrders").prop('checked') == true || $("#" + OrderSet_LabOrder.params.PanelID + " #selectRecordOrders").prop('checked') == false) {

                $("#" + OrderSet_LabOrder.params.PanelID + " #" + OrderSet_LabOrder.resultSearchGridId + " tbody").find('input[type="checkbox"]').each(function () {
                    OrderSet_LabOrder.enableAddResult(this);
                });
            }

            var AttachedSelectedOrders = [];
            var DettachedSelectedOrders = [];
            if ((selectedAttachedOrders != '' && selectedAttachedOrders != null) || (selectedDetachedOrders != '' && selectedDetachedOrders != null)) {
                AttachedSelectedOrders = selectedAttachedOrders;
                DettachedSelectedOrders = selectedDetachedOrders;
            } else {
                var AttachSelectedOrdersAndResults = OrderSet_ProgressNote.AttachedNoteComponentIds.slice();
                var DettachSelectedOrdersAndResults = OrderSet_ProgressNote.DetachedNoteComponentIds.slice();
                //Check for Medications Values
                //if (AttachSelectedOrdersAndResults.join().indexOf("prsc") != -1 || DettachSelectedOrdersAndResults.join().indexOf("prsc") != -1) {
                //    var AttachSelectedResults = EMRUtility.slicefunc(AttachSelectedOrdersAndResults, "prsc", 0, -4);
                //    var dettachSelectedResults = EMRUtility.slicefunc(DettachSelectedOrdersAndResults, "prsc", 0, -4);
                //    OrderSet_Prescriptions.addPrescriptionsToNotes(AttachSelectedResults, dettachSelectedResults);
                //}
                AttachedSelectedOrders = EMRUtility.slicefunc(AttachSelectedOrdersAndResults, "rslt", 0, -4);
                DettachedSelectedOrders = EMRUtility.slicefunc(DettachSelectedOrdersAndResults, "rslt", 0, -4);
            }

            if (AttachedSelectedOrders != null && AttachedSelectedOrders != '') {
                for (var i = 0; i < OrderSet_ProgressNote.AttachedNoteComponentIds.length; i++) {
                    var ALid = OrderSet_ProgressNote.AttachedNoteComponentIds[i];
                    if ($('#' + OrderSet_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_LabOrderDetail_Main' + ALid).length != 0) {
                        var index = AttachedSelectedOrders.indexOf(ALid);
                        if (index > -1) {
                            AttachedSelectedOrders.splice(index, 1);
                        }
                    }
                }
            }

            var detachedvalues = DettachedSelectedOrders;
            if (detachedvalues.join() != null && detachedvalues.join() != '') {
                OrderSet_LabOrder.detachResultFromNotesAT(detachedvalues).done(function () {
                    if (AttachedSelectedOrders.join() != null && AttachedSelectedOrders.join() != '') {
                        OrderSet_LabOrder.attachResultFromNotesAT(AttachedSelectedOrders);
                    } else {
                        OrderSet_ProgressNote.updateProgressNoteHTML(null, null, hideAlertMessage);
                    }
                });

            } else if (AttachedSelectedOrders.join() != null && AttachedSelectedOrders.join() != '') {
                OrderSet_LabOrder.attachResultFromNotesAT(AttachedSelectedOrders, hideAlertMessage);
            }
        }
    },

    attachResultFromNotesAT: function (AttachedSelectedOrders, hideAlertMessage) {
        OrderSet_LabOrder.getLabResultInfo(AttachedSelectedOrders.join(), hideAlertMessage).done(function () {
            setTimeout(function () {
                OrderSet_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                if (OrderSet_LabOrder.params != null && OrderSet_LabOrder.params.PanelID.indexOf('pnlOrderSetLabOrder') != -1) {
                    OrderSet_LabOrder.LabResultsSearch();
                }
            }, 5);
        });
    },
    detachResultFromNotesAT: function (detachedvalues) {
        var dfd = new $.Deferred();
        OrderSet_LabOrder.detachLabResultFromNotesDBCall(detachedvalues.join()).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                for (var i = 0; i < detachedvalues.length; i++) {
                    var ALid = detachedvalues[i];
                    $('#' + OrderSet_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_LabResultDetail_Main' + ALid).remove();
                }
            }
            else {
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
        OrderSet_LabOrder.getResultsForSOAP_DBCall(labOrderId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.ResultSoapCount > 0) {
                        OrderSet_LabOrder.createResultBodyHTML123(response, '#' + OrderSet_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', labOrderId, hideAlertMessage);
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
        objData["PatientId"] = OrderSet_LabOrder.params.patientID;
        objData["commandType"] = "get_results_forsoap";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },

    createResultBodyHTML123: function (response, noteHTMLCtrl, resultId, hidemessage) {
        OrderSet_LabOrder.checkResultExists();
        if (response.LabResultFill_JSON != null && response.LabResultFill_JSON != '') {
            var MedicationsSOAPJSON = JSON.parse(response.LabResultFill_JSON);
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
                if ($(noteHTMLCtrl + ' OrderSet_labresults').parent().parent().find('#Cli_LabResultDetail_Main0').length != 0) {
                    $(noteHTMLCtrl + ' #Cli_LabResultDetail_Main0').parent().remove();
                }
            }

            var AListId = [];
            $.each(MedicationsSOAPJSON, function (index, element) {

                var ALid = element.LabOrderResultId;
                //             var $infoButtonrow = OrderSet_InfoButtonView.GenerateInfoLink(element.NDCID, 'clinicalTabProgressNote', 1);
                var $SectionBodyMedications = $(document.createElement('section'));
                $SectionBodyMedications.attr('id', "Cli_LabResultDetail_Main" + ALid);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_LabResultDetail_" + ALid);
                var $ListMedications = $(document.createElement('ul'));
                var duration = "";
                $ListMedications.attr('class', 'list-unstyled')

                $SectionBodyMedications.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_LabResultDetail_" + ALid + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_LabResultDetail_Main" + ALid + '"  ><i class="fa fa-times"></i></a></div> ');
                // var obj = $('<p>');
                //  $(obj).html(element.SoapText);
                //$(obj).html($(obj).text())
                $ListMedications.append("<li>" + element.SoapText + "</li>");

                //             $ListMedications.append((element.Comments == "" ? "" : "<li>Comments: " + element.Comments));
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
                //if ($('#' + OrderSet_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML OrderSet_medications').parent().parent().find('#Cli_Medications_Main' + ALid).length == 0) {
                if ($(noteHTMLCtrl + ' OrderSet_labresults').parent().parent().find('#Cli_LabResultDetail_Main' + ALid).length == 0) {
                    AListId.push(ALid);
                    $mainDivMedications.append($SectionBodyMedications);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(noteHTMLCtrl + ' OrderSet_labresults').parent().parent().find('#Cli_LabResultDetail_Main' + ALid + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(noteHTMLCtrl + ' OrderSet_labresults').parent().parent().find('#Cli_LabResultDetail_Main' + ALid + ' ul li:Last').get(0).outerHTML;
                    }
                    $(noteHTMLCtrl + ' OrderSet_labresults').parent().parent().find('#Cli_LabResultDetail_Main' + ALid).html($SectionBodyMedications.html());
                    $(noteHTMLCtrl + ' OrderSet_labresults').parent().parent().find('#Cli_LabResultDetail_Main' + ALid + ' ul').append(CommentHTML);;
                }


            });

            if (AListId.join(",") != "") {
                resultId = AListId.join(",");
            }
            if ($mainDivMedications.html() != '') {
                OrderSet_LabOrder.updateResultHtml($mainDivMedications.html(), resultId, noteHTMLCtrl, hidemessage);
            } else {
                OrderSet_LabOrder.updateResultHtml('', resultId, noteHTMLCtrl, hidemessage);
                OrderSet_ProgressNote.updateProgressNoteHTML(null, null, hidemessage);
            }
        }

    },

    createResultBodyHTML: function (response, NoteHTMLCtrl, UnloadLabOrder) {
        OrderSet_LabOrder.checkResultExists();
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
                if ($(NoteHTMLCtrl + ' OrderSet_labresults').parent().parent().find('#Cli_LabResultDetail_Main' + LabResultId).length == 0) {
                    $mainDivLabOrder.append($SectionBodyLabOrder);
                    OrderSet_LabOrder.updateResultHtml($mainDivLabOrder.html(), LabResultId, NoteHTMLCtrl);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' OrderSet_labresults').parent().parent().find('#Cli_LabResultDetail_Main' + LabResultId + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' OrderSet_labresults').parent().parent().find('#Cli_LabResultDetail_Main' + LabResultId + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' OrderSet_labresults').parent().parent().find('#Cli_LabResultDetail_Main' + LabResultId).html($SectionBodyLabOrder.html());
                    $(NoteHTMLCtrl + ' OrderSet_labresults').parent().parent().find('#Cli_LabResultDetail_Main' + LabResultId + ' ul').append(CommentHTML);
                    OrderSet_ProgressNote.updateProgressNoteHTML();
                    OrderSet_LabOrder.updateResultHtml("", LabResultId, NoteHTMLCtrl);

                }

                if (UnloadLabOrder == true) {
                    OrderSet_LabOrder.Unload(OrderSet_LabOrderDetails.bNextPrev);
                }
            }
        }
    },
    getLatestLabResultByPatientIdDBCall: function () {
        var objData = new Object();
        if (OrderSet_Notes.params.patientID == "" || OrderSet_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = OrderSet_Notes.params.patientID;
        }
        objData["commandType"] = "getlatest_labresultby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },
    getLatestLabResultByPatientId: function (hidemessage) {
        var strMessage = '';
        //   AppPrivileges.GetFormPrivileges("Notes_Notes", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            OrderSet_LabOrder.getLatestLabResultByPatientIdDBCall().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    OrderSet_LabOrder.createResultBodyHTML123(response, '#' + OrderSet_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hidemessage);
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
        var LabOrderIds = $('#' + OrderSet_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML OrderSet_labresults').parent().parent().find('section[id*="Cli_LabResultDetail_Main"]').map(function () {
            return this.id.replace("Cli_LabResultDetail_Main", "");
        }).get().join(',');
        if (LabOrderIds == "" || LabOrderIds == "undefined") {
            OrderSet_ProgressNote.updateProgressNoteHTML(null, null, true);
            utility.DisplayMessages('Successfully Deleted', 1);
        }
        else {
            OrderSet_LabOrder.detachLabResultFromNotesDBCall(LabOrderIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (IsUpdate) {

                        OrderSet_ProgressNote.updateProgressNoteHTML(null, null, true);
                    }
                    utility.DisplayMessages(response.Message, 1);
                    setTimeout(OrderSet_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        if (LabResultComponentRemove) {
            $('#' + OrderSet_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Lab Results']").remove();
            $('#' + OrderSet_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML OrderSet_labresults').parent().parent().remove();
        } else {
            $('#' + OrderSet_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML OrderSet_labresults').parent().parent().find('section[id*="Cli_LabResultDetail_Main"]').remove();
        }
    },


    detachLabResultFromNotesDBCall: function (LabId) {
        var objData = {};
        objData["LabResultIDs"] = LabId;
        if (OrderSet_ProgressNote.params.NotesVisitId == "" || OrderSet_ProgressNote.params.NotesVisitId == "undefined") {
            objData["VisitId"] = 0;
        } else {
            if (OrderSet_ProgressNote.params.NotesVisitId < 1) {
                objData["VisitId"] = $('#pnlClinicalProgressNote #hfVisitId').val();
            } else {
                objData["VisitId"] = OrderSet_ProgressNote.params.NotesVisitId;
            }

        }
        if (OrderSet_ProgressNote.params.patientID == "" || OrderSet_ProgressNote.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = OrderSet_ProgressNote.params.patientID;
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
                var selectedValue = LabOrderId.replace('Cli_LabResultDetail_Main', '');
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    OrderSet_LabOrder.detachLabResultFromNotes_DBCall(selectedValue).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            $('#' + LabOrderId).remove();

                            OrderSet_ProgressNote.updateProgressNoteHTML();
                            setTimeout(OrderSet_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);

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
        objData["LabResultIDs"] = LabOrderId;
        if (OrderSet_ProgressNote.params.NotesVisitId == "" || OrderSet_ProgressNote.params.NotesVisitId == "undefined") {
            objData["VisitId"] = 0;
        } else {
            if (OrderSet_ProgressNote.params.NotesVisitId < 1) {
                objData["VisitId"] = $('#pnlClinicalProgressNote #hfVisitId').val();
            } else {
                objData["VisitId"] = OrderSet_ProgressNote.params.NotesVisitId;
            }

        }
        if (OrderSet_ProgressNote.params.patientID == "" || OrderSet_ProgressNote.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = OrderSet_ProgressNote.params.patientID;
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
        var ParentCtrl = 'OrderSetTabLabOrder';
        var ParentCtrlPanelID = null;
        if (OrderSet_LabOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
            ParentCtrl = 'OrderSet_LabOrder';
            ParentCtrlPanelID = OrderSet_LabOrder.params.PanelID;
        }
        else if (OrderSet_LabOrder.params["ParentCtrl"] == "clinicalTabFaceSheet") {
            ParentCtrl = 'OrderSet_LabOrder';
            ParentCtrlPanelID = OrderSet_LabOrder.params.PanelID;
        }

        EMRUtility.showCurrentItemHistory(OrderSet_LabOrder.params.PanelID, null, null, "LabOrderResult,LabOrderResultDetail", null, ParentCtrl, labOrderResultId, OrderSet_LabOrder.params.PanelID);

    },

}