OrderSet_RadiologyOrder = {
    //Author: Humaira Yousaf
    //Date: 17-03-2016
    //This file will handle all actions performed for Radiology Order
    bIsFirstLoad: true,
    params: [],
    patientId: null,
    orderSearchGridId: 'dgvRadiologyOrder',
    resultSearchGridId: 'dgvRadiologyResult',
    detailParams: null,

    //Author: Humaira Yousaf
    //Date: 17-03-2016
    //This function will handle fill of Radiology Order
    Load: function (params) {


        //Start//13-06-2016 Abid Ali for retaining detail values
        OrderSet_RadiologyOrder.detailParams = OrderSet_RadiologyOrder.params;
        //End//13-06-2016 Abid Ali for retaining detail values

        OrderSet_RadiologyOrder.params = params;
        OrderSet_RadiologyOrder.patientId = $("div#PatientProfile #hfPatientId").val();
        OrderSet_RadiologyOrder.params.mode = "Add";

        if (OrderSet_RadiologyOrder.params.PanelID != 'pnlOrderSetRadiologyOrder') {
            OrderSet_RadiologyOrder.params.PanelID = OrderSet_RadiologyOrder.params.PanelID + ' #pnlOrderSetRadiologyOrder';
        } else {
            OrderSet_RadiologyOrder.params.PanelID = 'pnlOrderSetRadiologyOrder';
        }

        $('#' + OrderSet_RadiologyOrder.params.PanelID + ' #pnlRadiologyOrder_Search').resetAllControls(null);
        if (OrderSet_RadiologyOrder.params.ParentCtrl == 'clinicalTabProgressNote') {


            //For Data table in notes
            //   OrderSet_RadiologyOrder.orderSearchGridId = "dgvRadiologyOrderNote";
            //  OrderSet_RadiologyOrder.resultSearchGridId = "dgvRadiologyResultNote";

            //  $("#" + OrderSet_RadiologyOrder.params.PanelID + " #dgvRadiologyOrder").attr('id', OrderSet_RadiologyOrder.orderSearchGridId)
            //   $("#" + OrderSet_RadiologyOrder.params.PanelID + " #dgvRadiologyResult").attr('id', OrderSet_RadiologyOrder.resultSearchGridId)

            $("#" + OrderSet_RadiologyOrder.params.PanelID + " #orders").attr('href', '#RadiologyOrder1')
            $("#" + OrderSet_RadiologyOrder.params.PanelID + " #results").attr('href', '#RadiologyResult1')
            $("#" + OrderSet_RadiologyOrder.params.PanelID + " #RadiologyOrder").attr('id', 'RadiologyOrder1')
            $("#" + OrderSet_RadiologyOrder.params.PanelID + " #RadiologyResult").attr('id', 'RadiologyResult1')
            $('.nav-tabs').tab();

            $("#" + OrderSet_RadiologyOrder.params.PanelID + " #btnAddRadiologyOrderToNote").show();
            $("#" + OrderSet_RadiologyOrder.params.PanelID + " #btnAddRadiologyResultToNote").show();


        }
        else {
            //For Data table in tab control
            OrderSet_RadiologyOrder.orderSearchGridId = "dgvRadiologyOrder";
            OrderSet_RadiologyOrder.resultSearchGridId = "dgvRadiologyResult";


            $("#" + OrderSet_RadiologyOrder.params.PanelID + " #btnAddRadiologyOrderToNote").hide();
            $("#" + OrderSet_RadiologyOrder.params.PanelID + " #btnAddRadiologyResultToNote").hide();
        }

        var self = $('#' + OrderSet_RadiologyOrder.params.PanelID);
        if (OrderSet_RadiologyOrder.bIsFirstLoad == true) {
            //Start 21-03-2016 Humaira Yousaf to validate Radiology
            OrderSet_RadiologyOrder.ValidateRadiology();
            //End 21-03-2016 Humaira Yousaf to validate Radiology

            OrderSet_RadiologyOrder.fillProvider('frmOrderSetRadiologyOrder');
            OrderSet_RadiologyOrder.fillProvider('frmClinicalRadiologyResult');

            OrderSet_RadiologyOrder.radiologyOrderSearch();
            OrderSet_RadiologyOrder.radiologyResultsSearch();

            //Start 13-04-2016 Abid Ali to load Laboratories
            OrderSet_LabOrder.LoadLabs('ddlLaboratory', OrderSet_RadiologyOrder.params.PanelID);
            //End 13-04-2016 Abid Ali to load Laboratories
        }

        EMRUtility.ValidateFromToDate('frmOrderSetRadiologyOrder', 'dpStartDate', 'dpToDate', true, function () { }, function () { }, "To Date should be greater than From Date");
        EMRUtility.ValidateFromToDate('frmClinicalRadiologyResult', 'dpStartDate', 'dpToDate', true, function () { }, function () { }, "To Date should be greater than From Date");
        //Start//17-06-2016//Ahmad Raza// Tabs Selection Logic
        var orderHref = "#RadiologyOrder";
        var resultHref = "#RadiologyResult";
        if (OrderSet_RadiologyOrder.params.ParentCtrl == 'clinicalTabProgressNote') {
            orderHref = "#RadiologyOrder1";
            resultHref = "#RadiologyResult1";
        }
        if (OrderSet_RadiologyOrder.params.Type == "Order") {
            $('#ulRadilogyOrderTabsItems a[href="' + orderHref + '"]').trigger('click');
        }
        else if (OrderSet_RadiologyOrder.params.Type == "Result") {
            $('#ulRadilogyOrderTabsItems a[href="' + resultHref + '"]').trigger('click');
        }
        //End//17-06-2016//Ahmad Raza// Tabs Selection Logic

        if (OrderSet_RadiologyOrder.params.ParentCtrl == "clinicalTabFaceSheet") {
            EMRUtility.MakeFaceSheetPager('#' + OrderSet_RadiologyOrder.params.PanelID + " div#FaceSheetPager", OrderSet_RadiologyOrder.params.FaceSheetComponents, 'radiologyorders');
        }
    },


    //Author: Abid Ali
    //Date: 13-06-2016
    //Description: Saves Radiology order detail values at patient level
    addRadiologyOrderDetailParams: function () {

        if (OrderSet_RadiologyOrder.detailParams.hasData == true && OrderSet_RadiologyOrder.detailParams.CurrentPatientId == $("div#PatientProfile #hfPatientId").val()) {
            OrderSet_RadiologyOrder.params.hasData = true;
            OrderSet_RadiologyOrder.params.ProviderName = OrderSet_RadiologyOrder.detailParams.ProviderName;
            OrderSet_RadiologyOrder.params.ProviderId = OrderSet_RadiologyOrder.detailParams.ProviderId;
            OrderSet_RadiologyOrder.params.AssigneeName = OrderSet_RadiologyOrder.detailParams.AssigneeName;
            OrderSet_RadiologyOrder.params.AssigneeId = OrderSet_RadiologyOrder.detailParams.AssigneeId;
            OrderSet_RadiologyOrder.params.Problems = OrderSet_RadiologyOrder.detailParams.Problems;

            OrderSet_RadiologyOrder.params.RadiologyId = detailParams.params.RadiologyId;
            OrderSet_RadiologyOrder.params.BillingTypeId = OrderSet_RadiologyOrder.detailParams.BillingTypeId;
            OrderSet_RadiologyOrder.params.FacilityName = OrderSet_RadiologyOrder.detailParams.FacilityName
            OrderSet_RadiologyOrder.params.FacilityId = OrderSet_RadiologyOrder.detailParams.FacilityId;

            OrderSet_RadiologyOrder.params.CurrentPatientId = OrderSet_RadiologyOrder.detailParams.CurrentPatientId;
        }
        else {
            OrderSet_RadiologyOrder.params.hasData = false;
            OrderSet_RadiologyOrder.params.ProviderName = "";
            OrderSet_RadiologyOrder.params.ProviderId = "";
            OrderSet_RadiologyOrder.params.AssigneeName = "";
            OrderSet_RadiologyOrder.params.AssigneeId = "";
            OrderSet_RadiologyOrder.params.Problems = "";

            OrderSet_RadiologyOrder.params.RadiologyId = "";
            OrderSet_RadiologyOrder.params.BillingTypeId = "";
            OrderSet_RadiologyOrder.params.FacilityName = "";
            OrderSet_RadiologyOrder.params.FacilityId = "";

            OrderSet_RadiologyOrder.params.CurrentPatientId = "";
        }
    },

    //Function Name: fillProvider
    //Author: Ahmad Raza
    //Date: 08-04-2016
    //Description: This function will fill Provider dropdown
    fillProvider: function (formId) {

        OrderSet_RadiologyOrder.fillproviderDBCall().done(function (response) {
            if (response != "") {
                response = JSON.parse(response);

                var $ddlProvider = $('#' + OrderSet_RadiologyOrder.params.PanelID + ' #' + formId + ' #txtProvider');
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
    //Author: Ahmad Raza
    //Date: 08-04-2016
    //Description: This function will call DB to fill Provider dropdown
    fillproviderDBCall: function () {
        var objData = new Object();
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["commandType"] = "getorderingprovider";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyOrder", "RadiologyOrder");

    },

    //Method Name: bindAutoComplete
    //Author: Ahmad Raza
    //Date: 21-03-2016
    //Description: This function will handle fill procedure text box on input
    bindAutoComplete: function (element, refCtrlId) {

        var hiddenCrtl = $('#' + OrderSet_LabOrder.params.PanelID + ' #' + (refCtrlId != null ? refCtrlId : 'txtCPTCode'));
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "OrderSet_RadiologyOrder", hiddenCrtl, true);

    },

    //Method Name: openCPTCode
    //Author: Ahmad Raza
    //Date: 21-03-2016
    //Description: This function will handle opening of CPT Search Popup
    /*openCPTCode: function () {
        var params = [];

        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "OrderSet_RadiologyOrder";
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtCPTCode";
        params["ParentCtrlPanelID"] = OrderSet_RadiologyOrder.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, OrderSet_RadiologyOrder.params.PanelID);
    },*/

    openCPTCode: function (refHiddenCtrl, refCtrl) {
        var params = [];

        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'OrderSet_RadiologyOrder';

        if (refHiddenCtrl != null && refHiddenCtrl != "") {
            params["RefHiddenCtrl"] = refHiddenCtrl;
        }
        else {
            params["RefHiddenCtrl"] = "hfCPTCode";
        }

        if (refCtrl != null && refCtrl != "") {
            params["RefCtrl"] = OrderSet_RadiologyOrder.params.PanelID + " #" + refCtrl;
        }
        else {
            params["RefCtrl"] = OrderSet_RadiologyOrder.params.PanelID + " #txtCPTCode";
        }
        params["RefCtrlDescription"] = "";

        params["ParentCtrlPanelID"] = OrderSet_RadiologyOrder.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, OrderSet_RadiologyOrder.params.PanelID);
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
    //End//24-02-2016//Ahmad Raza//plugin for Toggle switch
    //Author: Muhammad Arshad
    //Date: 24-02-2016
    //This function will handle Add/Edit of CDS based on given CDSId
    RadiologyOrderAddEdit: function (radiologyOrderId) {
        var strMessage = "";
        var permissionState = radiologyOrderId != null && parseInt(radiologyOrderId) > 0 ? "EDIT" : "ADD";
        AppPrivileges.GetFormPrivileges("Orders and Results_Radiology Order", permissionState, "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                if (radiologyOrderId != null && parseInt(radiologyOrderId) > 0) {
                    params["RadiologyOrderId"] = radiologyOrderId;
                    params["mode"] = "Edit";
                }
                else {
                    params["RadiologyOrderId"] = -1;
                    params["mode"] = "Add";
                }

                params["FromAdmin"] = OrderSet_RadiologyOrder.params["FromAdmin"];

                if (OrderSet_RadiologyOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {

                    params["ParentCtrl"] = 'OrderSet_RadiologyOrder';
                    params["ParentCtrlPanelID"] = OrderSet_RadiologyOrder.params.PanelID;
                    LoadActionPan('OrderSet_RadiologyOrderDetails', params, OrderSet_RadiologyOrder.params.PanelID);

                }
                else if (OrderSet_RadiologyOrder.params["ParentCtrl"] == "clinicalTabFaceSheet") {
                    params["ParentCtrl"] = 'OrderSet_RadiologyOrder';
                    params["ParentCtrlPanelID"] = OrderSet_RadiologyOrder.params.PanelID;
                    LoadActionPan('OrderSet_RadiologyOrderDetails', params, OrderSet_RadiologyOrder.params.PanelID);
                }
                else {
                    params["ParentCtrl"] = 'OrderSet_RadiologyOrder';
                    LoadActionPan('OrderSet_RadiologyOrderDetails', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    //Author: Muhammad Arshad
    //Date :  21-04-2016
    //This function will handle Add/Edit of RadiologyResult
    radiologyResultAddEdit: function (RadiologyResultId, RadiologyOrderId, isFromDetail, caller) {
        var strMessage = "";
        var permissionState = RadiologyOrderId != null && parseInt(RadiologyOrderId) > 0 ? "EDIT" : "ADD";
        AppPrivileges.GetFormPrivileges("Orders and Results_Radiology Result", permissionState, "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                var params = [];
                if (RadiologyOrderId != null && parseInt(RadiologyOrderId) > 0) {
                    params["RadiologyResultId"] = RadiologyResultId;
                    params["mode"] = "Edit";
                }
                else {
                    params["RadiologyResultId"] = -1;
                    params["mode"] = "Add";
                }
                params["RadiologyOrderId"] = RadiologyOrderId;
                params["FromAdmin"] = OrderSet_RadiologyOrder.params["FromAdmin"];


                if (isFromDetail) {
                    if (OrderSet_RadiologyOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
                        if (caller != null) {

                            params["ParentCtrl"] = "OrderSet_RadiologyOrderDetails";
                            LoadActionPan('ClinicalRadiologyResultDetail', params);
                        }
                        else {
                            params["ParentCtrl"] = 'OrderSet_RadiologyOrder';
                            params["ParentCtrlPanelID"] = OrderSet_RadiologyOrder.params.PanelID;
                            LoadActionPan('ClinicalRadiologyResultDetail', params, OrderSet_RadiologyOrder.params.PanelID);
                        }
                    }
                    else {
                        params["ParentCtrl"] = "OrderSet_RadiologyOrderDetails";
                        LoadActionPan('ClinicalRadiologyResultDetail', params);
                    }
                }
                else {
                    if (OrderSet_RadiologyOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
                        params["ParentCtrlPanelID"] = OrderSet_RadiologyOrder.params.PanelID;
                        params["ParentCtrl"] = 'OrderSet_RadiologyOrder';
                        LoadActionPan('ClinicalRadiologyResultDetail', params, OrderSet_RadiologyOrder.params.PanelID);
                    }
                    else if (OrderSet_RadiologyOrder.params["ParentCtrl"] == "clinicalTabFaceSheet") {
                        params["ParentCtrlPanelID"] = OrderSet_RadiologyOrder.params.PanelID;
                        params["ParentCtrl"] = 'OrderSet_RadiologyOrder';
                        LoadActionPan('ClinicalRadiologyResultDetail', params, OrderSet_RadiologyOrder.params.PanelID);
                    }
                    else {
                        params["ParentCtrl"] = 'OrderSet_RadiologyOrder';
                        LoadActionPan('ClinicalRadiologyResultDetail', params);
                    }
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ValidateRadiology: function () {
        $("#" + OrderSet_RadiologyOrder.params.PanelID + " #frmOrderSetRadiologyOrder")
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
            OrderSet_RadiologyOrder.RadiologySave();
        });
    },

    //Function Name: radiologyOrderSearch
    //Author Name: Humaira Yousaf
    //Created Date: 17-03-2016
    //Description: Searches Radiology Orders
    //Params: radiologyId, PageNo, rpp
    radiologyOrderSearch: function (radiologyId, PageNo, rpp, caller) {

        var strMessage = "";

        if (OrderSet_RadiologyOrder.params.ParentCtrl == "clinicalTabProgressNote") {
            if ($("#" + OrderSet_RadiologyOrder.params.PanelID + " #" + OrderSet_RadiologyOrder.orderSearchGridId + " thead tr #selectRecordOrders").length == 0) {
                $("#" + OrderSet_RadiologyOrder.params.PanelID + " #" + OrderSet_RadiologyOrder.orderSearchGridId + " thead tr").prepend('<th style="width:50px"><input type="checkbox" onchange="OrderSet_RadiologyOrder.checkUncheckAllOrders(this);" controlname="selectRecordOrders" id="selectRecordOrders" name="chkHeader" class="input-block" coltype="checkbox"/></th>');
            }
        } else {
            $("#" + OrderSet_RadiologyOrder.params.PanelID + " #" + OrderSet_RadiologyOrder.orderSearchGridId + "+ th#selectRecordOrders").remove();
        }

        if ($("#" + OrderSet_RadiologyOrder.params.PanelID + " #pnlRadiologyOrder_Result").css("display") == "none") {
            $("#" + OrderSet_RadiologyOrder.params.PanelID + " #pnlRadiologyOrder_Result").show();
        }
        AppPrivileges.GetFormPrivileges("Orders and Results_Radiology Order", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var self = $("#" + OrderSet_RadiologyOrder.params.PanelID + " form");
                var myJSON = self.getMyJSONByName();

                // search specific on caller Id
                if (caller != null) {
                    if (caller.indexOf("RadiologyOrderDetail") >= 0) {
                        myJSON = null;
                        // Reload Providers
                        OrderSet_RadiologyOrder.fillProvider('frmOrderSetRadiologyOrder');
                    }
                }

                OrderSet_RadiologyOrder.searchRadiologyOrder(myJSON, radiologyId, PageNo, rpp).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        OrderSet_RadiologyOrder.radiologyGridLoad(response);
                        var totalRows = $("#" + OrderSet_RadiologyOrder.params.PanelID + " #" + OrderSet_RadiologyOrder.orderSearchGridId + " tr").length;
                        totalRows -= 1;
                        var selectedRows = $("#" + OrderSet_RadiologyOrder.params.PanelID + " #" + OrderSet_RadiologyOrder.orderSearchGridId + " tbody tr input:checked").length;
                        if (totalRows == selectedRows) {
                            $("#" + OrderSet_RadiologyOrder.params.PanelID + " #" + OrderSet_RadiologyOrder.orderSearchGridId + " #selectRecordOrders").prop("checked", true);
                        }
                        else {
                            $("#" + OrderSet_RadiologyOrder.params.PanelID + " #" + OrderSet_RadiologyOrder.orderSearchGridId + " #selectRecordOrders").prop("checked", false);
                        }
                        var TableControl = OrderSet_RadiologyOrder.params.PanelID + " #pnlRadiologyOrder_Result #" + OrderSet_RadiologyOrder.orderSearchGridId;
                        var PagingPanelControlID = OrderSet_RadiologyOrder.params.PanelID + " #dgvRadiologyOrder_Paging";
                        var ClassControlName = "OrderSet_RadiologyOrder";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(
                            CreatePagination(response.radiologyOrderCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                                OrderSet_RadiologyOrder.radiologyOrderSearch(PrimaryID, PageNumber, ResultPerPage);
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
    //Function Name: radiologyGridLoad
    //Author Name: Humaira Yousaf
    //Created Date: 17-03-2016
    //Description: Loads Radiology Orders data in grid
    //Params: response
    radiologyGridLoad: function (response) {

        //Start 24-05-2016 Muhammad Arshad Remove Duplicate search issue on Datatable
        if ($.fn.dataTable.isDataTable("#" + OrderSet_RadiologyOrder.params.PanelID + " #pnlRadiologyOrder_Result #" + OrderSet_RadiologyOrder.orderSearchGridId)) {
            $("#" + OrderSet_RadiologyOrder.params.PanelID + " #pnlRadiologyOrder_Result #" + OrderSet_RadiologyOrder.orderSearchGridId).dataTable().fnClearTable();
            $("#" + OrderSet_RadiologyOrder.params.PanelID + " #pnlRadiologyOrder_Result #" + OrderSet_RadiologyOrder.orderSearchGridId).dataTable().fnDestroy();

        }

        $("#" + OrderSet_RadiologyOrder.params.PanelID + " #pnlRadiologyOrder_Result #" + OrderSet_RadiologyOrder.orderSearchGridId + " tbody").find("tr").remove();
        ////Data table is not destroying fully, so we have to initialize it once
        //$("#" + OrderSet_RadiologyOrder.params.PanelID + " #pnlRadiologyOrder_Result #" + OrderSet_RadiologyOrder.orderSearchGridId).dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it

        ////Remove Previous rows from table
        //$("#" + OrderSet_RadiologyOrder.params.PanelID + " #pnlRadiologyOrder_Result #" + OrderSet_RadiologyOrder.orderSearchGridId + " tbody").find("tr").remove(); //Removing all the table data from table body

        if (response.radiologyOrderCount > 0) {
            var radiologyLoadJSONData = JSON.parse(response.RadiologyLoad_JSON); //Parsing array to JSON
            $.each(radiologyLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                //   $row.attr("onclick", "OrderSet_RadiologyOrderDetails.radiologyEdit('" + item.RadiologyOrderId + "',event);");
                $row.attr("id", "gvRadiology_row" + item.RadiologyOrderId);
                $row.attr("RadiologyId", item.RadiologyOrderId);
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

                //Start//22-04-2016//Ahmad Raza//logic to check uncheck order if it is attached with Note
                var SelectionCheckBoxColumn = "";
                var Checked = "";
                if (item.IsNoteLinked == "True") {
                    if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.RadiologyOrderId + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                        Checked = " ";
                    } else {
                        Checked = " checked";
                    }
                } else {
                    if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.RadiologyOrderId + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                        Checked = " checked";
                    } else {
                        Checked = "";
                    }
                }

                if (OrderSet_RadiologyOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                    SelectionCheckBoxColumn = '<td><input type="checkbox" onchange="OrderSet_RadiologyOrder.enableAddOrder(this,event);" id="' + item.RadiologyOrderId + '" name="SelectCheckBoxOrder" ' + Checked + ' class="input-block"/></td>';
                } else {
                    SelectionCheckBoxColumn = "";
                }
                //End//22-04-2016//Ahmad Raza//logic to check uncheck order if it is attached with Note
                //Start//20-04-2016//Ahmad Raza//logic to show History icon
                if (item.Status == "Signed") {

                    //Begin 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578
                    $row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.RadiologyOrderId + '</td><td><a class="btn btn-xs disableAll" href="#" onclick="OrderSet_RadiologyOrder.radiologyOrderDelete(\'' + item.RadiologyOrderId + '\', event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="OrderSet_RadiologyOrderDetails.radiologyEdit(\'' + item.RadiologyOrderId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="OrderSet_RadiologyOrder.printRadiologyOrder(\'' + item.RadiologyOrderId + '\',\'' + item.Status + '\',event );" title="View Result"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="OrderSet_RadiologyOrder.showOrderHistory(\'' + item.RadiologyOrderId + '\', event );" title="Record History"> <i class="fa fa-history blue"></i></a></td><td>'
                        + utility.RemoveTimeFromDate(null, item.OrderDate) + ' ' + item.OrderTime + '</td><td>' + item.Test.replace("|", "<br/>") + '</td><td>' + item.LabName + '</td><td>' + item.OrderNo + '</td><td>' + item.Status + '</td><td>' + item.Provider + '</td><td>' + item.AssigneeName + '</td>');
                    //End 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578

                }
                else {

                    //Begin 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578
                    $row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.RadiologyOrderId + '</td><td><a class="btn btn-xs" href="#" onclick="OrderSet_RadiologyOrder.radiologyOrderDelete(\'' + item.RadiologyOrderId + '\', event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="OrderSet_RadiologyOrderDetails.radiologyEdit(\'' + item.RadiologyOrderId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs disableAll" href="#" onclick="OrderSet_RadiologyOrder.printRadiologyOrder(\'' + item.RadiologyOrderId + '\',\'' + item.Status + '\',event );" title="View Result"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="OrderSet_RadiologyOrder.showOrderHistory(\'' + item.RadiologyOrderId + '\', event );" title="Record History"> <i class="fa fa-history blue"></i></a></td><td>'
                    + utility.RemoveTimeFromDate(null, item.OrderDate) + ' ' + item.OrderTime + '</td><td>' + item.Test.replace("|", "<br/>") + '</td><td>' + item.LabName + '</td><td>' + item.OrderNo + '</td><td>' + item.Status + '</td><td>' + item.Provider + '</td><td>' + item.AssigneeName + '</td>');
                    //End 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578
                }
                //End//20-04-2016//Ahmad Raza//logic to show History icon
                $("#" + OrderSet_RadiologyOrder.params.PanelID + " #pnlRadiologyOrder_Result #" + OrderSet_RadiologyOrder.orderSearchGridId + " tbody").last().append($row);
            });
        }
        else {
            $("#" + OrderSet_RadiologyOrder.params.PanelID + ' #pnlRadiologyOrder_Result #' + OrderSet_RadiologyOrder.orderSearchGridId).DataTable({
                "language": {
                    "emptyTable": "No Diagnostic Imaging Order Found"
                }, "autoWidth": false, "bLengthChange": false, "aaSorting": [[1, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + OrderSet_RadiologyOrder.params.PanelID + ' #pnlRadiologyOrder_Result #' + OrderSet_RadiologyOrder.orderSearchGridId))
            ;
        else {
            $("#" + OrderSet_RadiologyOrder.params.PanelID + " #pnlRadiologyOrder_Result #" + OrderSet_RadiologyOrder.orderSearchGridId).DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [[1, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        }

        EMRUtility.fixDataTableDuplication("#" + OrderSet_RadiologyOrder.params.PanelID + " #pnlRadiologyOrder_Result");
    },

    //Function Name: searchRadiologyOrder
    //Author Name: Humaira Yousaf
    //Created Date: 17-03-2016
    //Description: Searches Radiology Orders
    //Params: radiologyOrderData, radiologyOrderId, PageNumber, RowsPerPage
    searchRadiologyOrder: function (radiologyOrderData, radiologyOrderId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = {};
        if (radiologyOrderData != null)
            objData = JSON.parse(radiologyOrderData);

        objData["RadiologyOrderId"] = radiologyOrderId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PatientId"] = OrderSet_RadiologyOrder.patientId;
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["Test"] = objData["CPTCode"];
        objData["ProviderId"] = $('#' + OrderSet_RadiologyOrder.params.PanelID + ' #frmOrderSetRadiologyOrder #txtProvider').val();

        /* Start 27/05/2015 Abid Ali / labId for radiology order */
        objData["LabId"] = $('#' + OrderSet_RadiologyOrder.params.PanelID + ' #frmOrderSetRadiologyOrder #ddlLaboratory').val();
        /* End 27/05/2015 Abid Ali / labId for radiology order */

        objData["OrderFromDate"] = $('#' + OrderSet_RadiologyOrder.params.PanelID + ' #frmOrderSetRadiologyOrder #dpStartDate').val();
        objData["Status"] = $('#' + OrderSet_RadiologyOrder.params.PanelID + ' #frmOrderSetRadiologyOrder #ddlStatus option:selected').val();
        objData["OrderToDate"] = $('#' + OrderSet_RadiologyOrder.params.PanelID + ' #frmOrderSetRadiologyOrder #dpToDate').val();
        objData["OrderNo"] = $('#' + OrderSet_RadiologyOrder.params.PanelID + ' #frmOrderSetRadiologyOrder #txtOrderNumber').val();



        objData["commandType"] = "search_radiologyorders";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyOrder", "RadiologyOrder");
    },

    //Function Name: radiologyOrderReset
    //Author Name: Farooq Ahmad
    //Created Date: 29-03-2016
    //Description: radiology Order Reset
    radiologyOrderReset: function () {

        utility.myConfirm('22', function () {
            $('#' + OrderSet_RadiologyOrder.params.PanelID + ' #frmOrderSetRadiologyOrder').resetAllControls(null);
            OrderSet_RadiologyOrder.ValidateRadiology();
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
    //Date :  03-05-2016
    //Description: Radiology Result Reset
    radiologyResultReset: function () {

        utility.myConfirm('22', function () {
            $('#' + OrderSet_RadiologyOrder.params.PanelID + ' #frmClinicalRadiologyResult').resetAllControls(null);
            OrderSet_RadiologyOrder.ValidateRadiology();
        }, function () { },
            '22'
        );
    },

    UnLoadTab: function () {
        var objDeffered = $.Deferred();
        /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        if (OrderSet_RadiologyOrder.params.ParentCtrl == "clinicalTabFaceSheet") {
            utility.UnLoadDialog(OrderSet_RadiologyOrder.params.PanelID + ' #frmClinicalCDS', function () {

                //$("#" + OrderSet_RadiologyOrder.params.PanelID + " #" + OrderSet_RadiologyOrder.orderSearchGridId).attr('id', 'dgvRadiologyOrder')
                //$("#" + OrderSet_RadiologyOrder.params.PanelID + " #" + OrderSet_RadiologyOrder.resultSearchGridId).attr('id', 'dgvRadiologyResult')


                if (OrderSet_RadiologyOrder.params["FromAdmin"] == "0") {
                    if (OrderSet_RadiologyOrder.params != null && OrderSet_RadiologyOrder.params.ParentCtrl != null) {
                        UnloadActionPan(OrderSet_RadiologyOrder.params.ParentCtrl, 'OrderSet_RadiologyOrder');
                    }
                    else
                        UnloadActionPan(null, 'OrderSet_RadiologyOrder');
                }
                else {
                    $("#mstrDivMedical #clinicalMenu_Medical_CDS").remove();
                    RemoveAdminTab();
                }
                objDeffered.resolve();
            }, function () {
                if (OrderSet_RadiologyOrder.params["FromAdmin"] == "0") {
                    if (OrderSet_RadiologyOrder.params != null && OrderSet_RadiologyOrder.params.ParentCtrl != null) {
                        UnloadActionPan(OrderSet_RadiologyOrder.params.ParentCtrl, 'OrderSet_RadiologyOrder');
                    }
                    else
                        UnloadActionPan(null, 'OrderSet_RadiologyOrder');
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
            if (OrderSet_RadiologyOrder.params["FromAdmin"] == "0") {
                if (OrderSet_RadiologyOrder.params != null && OrderSet_RadiologyOrder.params.ParentCtrl != null) {
                    if ($("#" + OrderSet_RadiologyOrder.params.PanelID + " #listRadiologyOrder").hasClass('active')) {
                        OrderSet_RadiologyOrder.addOrdersToNotes();
                    }
                    else if ($("#" + OrderSet_RadiologyOrder.params.PanelID + " #listRadiologyResult").hasClass('active')) {
                        OrderSet_RadiologyOrder.addResultsToNotes();
                    }
                    UnloadActionPan(OrderSet_RadiologyOrder.params.ParentCtrl, 'OrderSet_RadiologyOrder');
                }
                else {
                    if ($("#" + OrderSet_RadiologyOrder.params.PanelID + " #listRadiologyOrder").hasClass('active')) {
                        OrderSet_RadiologyOrder.addOrdersToNotes();
                    }
                    else if ($("#" + OrderSet_RadiologyOrder.params.PanelID + " #listRadiologyResult").hasClass('active')) {
                        OrderSet_RadiologyOrder.addResultsToNotes();
                    }
                    UnloadActionPan(null, 'OrderSet_RadiologyOrder');
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
        params["RefForm"] = "frmOrderSetRadiologyOrder";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "OrderSet_RadiologyOrder";
        LoadActionPan('Admin_Provider', params);
    },

    //Function Name: openproviderdetail
    //Author Name: Humaira Yousaf
    //Created Date: 21-03-2016
    //Description: Opens Provider
    openproviderdetail: function () {
        var params = [];
        params["providerid"] = $('#pnlOrderSetRadiologyOrder #hfprovider').val();
        params["mode"] = "edit";
        params["fromadmin"] = "0";
        params["refctrl"] = "txtprovider";
        params["parentctrl"] = 'OrderSet_RadiologyOrder';
        loadactionpan('providerdetail', params);
    },

    //Function Name: radiologyOrderDelete
    //Author Name: Ahmad Raza
    //Created Date: 22-03-2016
    //Description: Delete the selected Radiology Order
    radiologyOrderDelete: function (radiologyId, event, PageNo, rpp) {
        var strMessage = "";
        event.stopPropagation();
        AppPrivileges.GetFormPrivileges("Orders and Results_Radiology Order", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = radiologyId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        var self = $("#" + OrderSet_RadiologyOrder.params.PanelID + " form");
                        var myJSON = self.getMyJSONByName();

                        OrderSet_RadiologyOrder.deleteRadiologyOrder(myJSON, radiologyId, PageNo, rpp).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                utility.DisplayMessages(response.Message, 1);
                                OrderSet_RadiologyOrder.radiologyOrderSearch("", 1, 15);
                                OrderSet_RadiologyOrder.radiologyResultsSearch("", 1, 15);
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

    //Function Name: deleteRadiologyOrder
    //Author Name: Ahmad Raza
    //Created Date: 22-03-2016
    //Description: DB Call to Delete the selected Radiology Order
    deleteRadiologyOrder: function (radiologyData, radiologyId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = JSON.parse(radiologyData);
        objData["RadiologyOrderId"] = radiologyId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PatientId"] = OrderSet_RadiologyOrder.patientId;
        objData["commandType"] = "delete_radiologyorder";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyOrder", "RadiologyOrder");

    },
    //Function Name: printRadiologyOrder
    //Author Name: Humaira Yousaf
    //Created Date: 28-03-2016
    //Description: Previews Radiology Order
    printRadiologyOrder: function (radiologyId, status, event) {
        if (status == 'Signed') {
            var PanelID = "";
            var params = [];
            params["FromAdmin"] = "0";
            params["UserId"] = globalAppdata['AppUserId'];
            params["PatientId"] = $('#PatientProfile #hfPatientId').val();
            // params["ParentCtrl"] = OrderSet_RadiologyOrder.params.TabID;
            params["ParentCtrl"] = (OrderSet_RadiologyOrder.params.ParentCtrl != "clinicalTabProgressNote" && OrderSet_RadiologyOrder.params.ParentCtrl != "clinicalTabFaceSheet") ? OrderSet_RadiologyOrder.params.TabID : "OrderSet_RadiologyOrder";
            params["RadiologyOrderId"] = radiologyId;
            if (OrderSet_RadiologyOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                PanelID = 'pnlClinicalProgressNote #pnlOrderSetRadiologyOrder';
                params["PanelID"] = 'pnlClinicalProgressNote #pnlOrderSetRadiologyOrder';
                params["PrPanelID"] = 'pnlClinicalProgressNote #pnlOrderSetRadiologyOrder';
            }
            else if (OrderSet_LabOrder.params.ParentCtrl == "clinicalTabFaceSheet") {
                PanelID = 'pnlClinicalFaceSheet #pnlOrderSetRadiologyOrder';
                params["PanelID"] = 'pnlClinicalFaceSheet #pnlOrderSetRadiologyOrder';
                params["PrPanelID"] = 'pnlClinicalFaceSheet #pnlOrderSetRadiologyOrder';
            }
            else {
                PanelID = 'pnlOrderSetRadiologyOrder';
                params["PanelID"] = 'pnlOrderSetRadiologyOrder';
                params["PrPanelID"] = 'pnlOrderSetRadiologyOrder';
            }
            LoadActionPan('OrderSet_RadiologyOrderView', params, PanelID);
        }
        event.stopPropagation();
    },

    //Function Name: showOrderHistory
    //Author Name: Ahmad Raza
    //Created Date: 20-04-2016
    //Description: to show Radiology order history
    showOrderHistory: function (radiologyOrderId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        //Modified by Abid for opening in notes
        var ParentCtrl = 'clinicalTabRadiologyOrder';
        var ParentCtrlPanelID = null;
        if (OrderSet_RadiologyOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
            ParentCtrl = 'OrderSet_RadiologyOrder';
            ParentCtrlPanelID = OrderSet_RadiologyOrder.params.PanelID;
        }
        else if (OrderSet_RadiologyOrder.params["ParentCtrl"] == "clinicalTabFaceSheet") {
            ParentCtrl = 'OrderSet_RadiologyOrder';
            ParentCtrlPanelID = OrderSet_RadiologyOrder.params.PanelID;
        }
        EMRUtility.showCurrentItemHistory(OrderSet_RadiologyOrder.params.PanelID, null, null, "RadiologyOrder,RadiologyOrderTest, RadiologyOrderProblem", null, ParentCtrl, radiologyOrderId, ParentCtrlPanelID);
    },



    //Author: Ahmad Raza
    //Date :  22-04-2016
    //Description: checkUncheckAllOrders
    checkUncheckAllOrders: function (obj) {
        if ($(obj).is(':checked')) {
            $("#" + OrderSet_RadiologyOrder.params.PanelID + " input[name='SelectCheckBoxOrder']:checkbox").prop('checked', true);
        } else {
            $("#" + OrderSet_RadiologyOrder.params.PanelID + " input[name='SelectCheckBoxOrder']:checkbox").prop('checked', false);
        }
        $("#" + OrderSet_RadiologyOrder.params.PanelID + " #" + OrderSet_RadiologyOrder.orderSearchGridId + " tbody").find('input[type="checkbox"]').each(function () {
            OrderSet_RadiologyOrder.enableAddOrder(this);
        });

    },

    //Function Name:enableAddOrder
    //Author Name: Ahmad Raza
    //Created Date: 22-04-2016
    //Description: This function will add/remove attached/detached orders in/from json
    enableAddOrder: function (obj, event) {
        if (event != null) {
            event.stopPropagation();
        }

        var totalRows = $("#" + OrderSet_RadiologyOrder.params.PanelID + " #" + OrderSet_RadiologyOrder.orderSearchGridId + " tr").length;
        totalRows -= 1;
        var selectedRows = $("#" + OrderSet_RadiologyOrder.params.PanelID + " #" + OrderSet_RadiologyOrder.orderSearchGridId + " tbody tr input:checked").length;

        if (totalRows == selectedRows) {
            $("#" + OrderSet_RadiologyOrder.params.PanelID + " #" + OrderSet_RadiologyOrder.orderSearchGridId + " #selectRecordOrders").prop("checked", true);
        }
        else {
            $("#" + OrderSet_RadiologyOrder.params.PanelID + " #" + OrderSet_RadiologyOrder.orderSearchGridId + " #selectRecordOrders").prop("checked", false);
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

    getRadiologyOrderInfo: function (radiologyOrderId) {
        if (radiologyOrderId == null || radiologyOrderId == '') {
            return false;
        }
        var dfd = new $.Deferred();
        OrderSet_RadiologyOrder.getOrdersForSOAP_DBCall(radiologyOrderId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.radiologySoapCount > 0) {
                        OrderSet_RadiologyOrder.createRadiologyBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', radiologyOrderId);
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

    getOrdersForSOAP_DBCall: function (radiologyOrderId) {
        var objData = new Object();
        objData["RadiologyOrderIDs"] = radiologyOrderId;
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["PatientId"] = OrderSet_RadiologyOrder.params.patientID;
        objData["commandType"] = "get_Orders_forsoap";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyOrder", "RadiologyOrder");
    },

    createRadiologyBodyHTML: function (response, noteHTMLCtrl, radiologyOrderId) {
        OrderSet_RadiologyOrder.checkRadiologyOrderExists();
        if (response.radiologySoap_JSON != null && response.radiologySoap_JSON != '') {
            var radiologyOrderSOAPJSON = JSON.parse(response.radiologySoap_JSON);
            if (response.radiologySoap_JSON != null) {
                //          medicationReviewSoapJSON = JSON.parse(response.radiologySoap_JSON);
            }

            var $mainDivRadiology = $(document.createElement('div'));
            if (radiologyOrderSOAPJSON == null || radiologyOrderSOAPJSON.length == 0) {
            } else {
                if ($(noteHTMLCtrl + ' OrderSet_RadiologyOrder').parent().parent().find('#Cli_RadiologyOrderDetail_Main0').length != 0) {
                    $(noteHTMLCtrl + ' #Cli_RadiologyOrderDetail_Main0').parent().remove();
                }
            }

            var AListId = [];
            $.each(radiologyOrderSOAPJSON, function (index, element) {

                var ALid = element.RadiologyOrderId;
                //             var $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(element.NDCID, 'clinicalTabProgressNote', 1);
                var $SectionBodyRadiology = $(document.createElement('section'));
                $SectionBodyRadiology.attr('id', "Cli_RadiologyOrderDetail_Main" + ALid);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_RadiologyOrderDetail_" + ALid);
                var $ListRadiology = $(document.createElement('ul'));
                var duration = "";
                $ListRadiology.attr('class', 'list-unstyled')

                $SectionBodyRadiology.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_RadiologyOrderDetail_" + ALid + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_RadiologyOrderDetail_Main" + ALid + '"  ><i class="fa fa-times"></i></a></div> ');
                var obj = $('<p>');
                $(obj).html(element.SoapText);
                //$(obj).html($(obj).text())
                $ListRadiology.append("<li>" + $(obj).text() + "</li>");

                //             $ListRadiology.append((element.Comments == "" ? "" : "<li>Comments: " + element.Comments));
                if (radiologyOrderSOAPJSON.length - 1 == index) {
                    //$.each(medicationReviewSoapJSON, function (index, item) {
                    //    if (item.ReviewedOn != null && item.ReviewedOn != '') {
                    //        var dateFormat = item.ReviewedOn
                    //        var ReviewedOndateFormat = $.datepicker.formatDate('MM dd, yy ', new Date(dateFormat)) + utility.RemoveDateFromDateTime(null, dateFormat);
                    //        $ListRadiology.append("<li>" + (item.ReviewedBy == '' ? "" : "Medications reviewed by " + item.ReviewedBy) +
                    //   " on:  " + ReviewedOndateFormat + "</li>");
                    //    }
                    //});
                }
                $DetailsDiv.append($ListRadiology);
                $SectionBodyRadiology.append($DetailsDiv);
                //if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML clinical_medications').parent().parent().find('#Cli_Medications_Main' + ALid).length == 0) {
                if ($(noteHTMLCtrl + ' OrderSet_RadiologyOrder').parent().parent().find('#Cli_RadiologyOrderDetail_Main' + ALid).length == 0) {
                    AListId.push(ALid);
                    $mainDivRadiology.append($SectionBodyRadiology);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(noteHTMLCtrl + ' OrderSet_RadiologyOrder').parent().parent().find('#Cli_RadiologyOrderDetail_Main' + ALid + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(noteHTMLCtrl + ' OrderSet_RadiologyOrder').parent().parent().find('#Cli_RadiologyOrderDetail_Main' + ALid + ' ul li:Last').get(0).outerHTML;
                    }
                    $(noteHTMLCtrl + ' OrderSet_RadiologyOrder').parent().parent().find('#Cli_RadiologyOrderDetail_Main' + ALid).html($SectionBodyRadiology.html());
                    $(noteHTMLCtrl + ' OrderSet_RadiologyOrder').parent().parent().find('#Cli_RadiologyOrderDetail_Main' + ALid + ' ul').append(CommentHTML);;
                }


            });

            if (AListId.join(",") != "") {
                radiologyOrderId = AListId.join(",");
            }
            if ($mainDivRadiology.html() != '') {
                OrderSet_RadiologyOrder.updateRadiologyHtml($mainDivRadiology.html(), radiologyOrderId, noteHTMLCtrl);
            } else {
                OrderSet_RadiologyOrder.updateRadiologyHtml('', radiologyOrderId, noteHTMLCtrl);
                Clinical_ProgressNote.saveComponentSOAPText('Radiology Order');
            }
        }

    },

    updateRadiologyHtml: function (radiologyHTML, radiologyId, noteHTMLCtrl) {
        $(noteHTMLCtrl + ' OrderSet_RadiologyOrder').parent().parent().addClass('initialVisitBody');
        if (radiologyHTML != '') {
            $(noteHTMLCtrl + ' OrderSet_RadiologyOrder').parent().parent().append(radiologyHTML);
        }

        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (radiologyHTML != '' && radiologyId != null && radiologyId != '' && radiologyId != '0') {
            OrderSet_RadiologyOrder.attachRadiologyOrderFromNotes(radiologyId);
        }

    },

    attachRadiologyOrderFromNotes: function (radiologyId) {
        var selectedValue = radiologyId;
        if (selectedValue == "" || selectedValue == "undefined") {
        }
        else {
            OrderSet_RadiologyOrder.attachRadiologyOrderWithNotes_DBCall(selectedValue).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    Clinical_ProgressNote.saveComponentSOAPText('Radiology Order');
                  //Start 01-12-2016 Humaira Yousaf to hide show eSuperbill link
		    Clinical_ProgressNote.HideShowBillingInfo();
		    //End 01-12-2016 Humaira Yousaf to hide show eSuperbill link
                    $('#' + radiologyId).remove();

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    attachRadiologyOrderWithNotes_DBCall: function (radiologyId) {
        var objData = new Object();
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["RadiologyOrderIDs"] = radiologyId;
        objData["commandType"] = "attach_orders_with_notes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyOrder", "RadiologyOrder");
    },

    checkRadiologyOrderExists: function () {
        //if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML OrderSet_RadiologyOrder').length == 0) {
        //    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList').append(' <li> <header>' +
        //                '<OrderSet_RadiologyOrder title="Radiology Orders"  id="' + this.id + '" class="NotesComponent">' +
        //                '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'RadiologyOrder\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Radiology Order">Radiology Order</a> ' +
        //                                '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'RadiologyOrder\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
        //                           '</OrderSet_RadiologyOrder> </header></li>');
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

        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML OrderSet_RadiologyOrder').length == 0) {

            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #PlanNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="BirthHxComponent"> <header>' +
                        '<OrderSet_RadiologyOrder title="Radiology Order"  id="clinicalMenu_Orders_Radiology" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'RadiologyOrder\',\'clinicalMenu_Orders_Radiology\',' + Clinical_ProgressNote.params.NotesId + ');" title="Diagnostic Imaging Order">Diagnostic Imaging Order</a> ' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'Radiology Order\',\'clinicalMenu_Orders_Radiology\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</OrderSet_RadiologyOrder> </header></li>');
            Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
                $(this).find('.closeBtn').removeClass('hidden');
                $(this).css('background', '#EAF1F8');
            });

            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
                $(this).find('.closeBtn').addClass('hidden');
                $(this).css('background', '#fff');
            });
        }

    },

    addOrdersToNotes: function (selectedAttachedOrders, selectedDetachedOrders) {
        var noteHTMLCtrl = '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML';
        if ($("#" + OrderSet_RadiologyOrder.orderSearchGridId).dataTable().fnSettings().aoData.length == 0) {
            //OrderSet_RadiologyOrder.noActiveRadiologyOrderSoapText();
        }
        else {

            if ($("#" + OrderSet_RadiologyOrder.params.PanelID + " #selectRecordOrders").prop('checked') == true || $("#" + OrderSet_RadiologyOrder.params.PanelID + " #selectRecordOrders").prop('checked') == false) {

                $("#" + OrderSet_RadiologyOrder.params.PanelID + " #" + OrderSet_RadiologyOrder.orderSearchGridId + " tbody").find('input[type="checkbox"]').each(function () {
                    OrderSet_RadiologyOrder.enableAddOrder(this);
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
                //Check for results Values
                //if (AttachSelectedOrdersAndResults.join().indexOf("prsc") != -1 || DettachSelectedOrdersAndResults.join().indexOf("prsc") != -1) {
                //    var AttachSelectedResults = EMRUtility.slicefunc(AttachSelectedOrdersAndResults, "prsc", 0, -4);
                //    var dettachSelectedResults = EMRUtility.slicefunc(DettachSelectedOrdersAndResults, "prsc", 0, -4);
                //    Clinical_Prescriptions.addPrescriptionsToNotes(AttachSelectedResults, dettachSelectedResults);
                //}
                AttachedSelectedOrders = EMRUtility.slicefunc(AttachSelectedOrdersAndResults, "ordr", 0, -4);
                DettachedSelectedOrders = EMRUtility.slicefunc(DettachSelectedOrdersAndResults, "ordr", 0, -4);
            }


            if (AttachedSelectedOrders != null && AttachedSelectedOrders != '') {
                for (var i = 0; i < Clinical_ProgressNote.AttachedNoteComponentIds.length; i++) {
                    var ALid = Clinical_ProgressNote.AttachedNoteComponentIds[i];
                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_RadiologyOrderDetail_Main' + ALid).length != 0) {
                        var index = AttachedSelectedOrders.indexOf(ALid);
                        if (index > -1) {
                            AttachedSelectedOrders.splice(index, 1);
                        }
                    }
                }

            }

            var detachedvalues = DettachedSelectedOrders;
            if (detachedvalues.join() != null && detachedvalues.join() != '') {
                OrderSet_RadiologyOrder.detachRadiologyOrderFromNotesAT(detachedvalues).done(function () {
                    if (AttachedSelectedOrders.join() != null && AttachedSelectedOrders.join() != '') {
                        OrderSet_RadiologyOrder.attachRadiologyOrderFromNotesAT(AttachedSelectedOrders);
                    } else {
                        Clinical_ProgressNote.saveComponentSOAPText('Radiology Order');
                    }
                });

            } else if (AttachedSelectedOrders.join() != null && AttachedSelectedOrders.join() != '') {
                OrderSet_RadiologyOrder.attachRadiologyOrderFromNotesAT(AttachedSelectedOrders);
            }

            //$("#" + Clinical_Medications.params.PanelID + " #btnAddMedications").prop('disabled', true);

        }
    },

    attachRadiologyOrderFromNotesAT: function (AttachedSelectedOrders) {
        OrderSet_RadiologyOrder.getRadiologyOrderInfo(AttachedSelectedOrders.join()).done(function () {
            setTimeout(function () {
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                if (OrderSet_RadiologyOrder.params != null && OrderSet_RadiologyOrder.params.PanelID.indexOf('pnlOrderSetRadiologyOrder') != -1) {
                    OrderSet_RadiologyOrder.radiologyOrderSearch();
                }
            }, 5);
        });
    },

    detachRadiologyOrderFromNotesAT: function (detachedvalues) {
        var dfd = new $.Deferred();
        OrderSet_RadiologyOrder.detachRadiologyOrderFromNotes_DBCall(detachedvalues.join()).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                for (var i = 0; i < detachedvalues.length; i++) {
                    var ALid = detachedvalues[i];
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_RadiologyOrderDetail_Main' + ALid).remove();
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
            dfd.resolve('ok');
        });
        return dfd.promise();
    },

    detachRadiologyOrderFromNotes_DBCall: function (radiologyId) {
        var objData = new Object();
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["RadiologyOrderIDs"] = radiologyId;
        objData["commandType"] = "detach_orders_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "RadiologyOrder", "RadiologyOrder");
    },

    noActiveRadiologyOrderSoapText: function () {
        var noteHTMLCtrl = '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML';
        if ($(noteHTMLCtrl + ' OrderSet_RadiologyOrder').parent().parent().find('#Cli_RadiologyOrderDetail_Main0').length == 0) {
            OrderSet_RadiologyOrder.checkRadiologyOrderExists();
            var htmlForNoRadiology = '<section id="Cli_RadiologyOrderDetail_Main0"> <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="Cli_RadiologyOrderDetail_0"><i class="fa fa-edit"></i></a>' +
    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="Cli_RadiologyOrderDetail_Main0"  ><i class="fa fa-times"></i></a></div> ' +
    '<div id="Cli_RadiologyOrderDetail_0"><ul class="list-unstyled"><li> No Diagnostic Imaging Order Found</li></ul></div></section>';
            OrderSet_RadiologyOrder.updateRadiologyHtml(htmlForNoRadiology, '0', noteHTMLCtrl);
            Clinical_ProgressNote.saveComponentSOAPText('Radiology Order');
        }
    },


    radiologyResultsSearch: function (radId, PageNo, rpp, caller) {
        var strMessage = "";

        if (OrderSet_RadiologyOrder.params.ParentCtrl == "clinicalTabProgressNote") {
            if ($("#" + OrderSet_RadiologyOrder.params.PanelID + " #" + OrderSet_RadiologyOrder.resultSearchGridId + " thead tr #selectRecordOrders").length == 0) {
                $("#" + OrderSet_RadiologyOrder.params.PanelID + " #" + OrderSet_RadiologyOrder.resultSearchGridId + " thead tr").prepend('<th style="width:50px"><input type="checkbox" onchange="OrderSet_RadiologyOrder.checkUncheckAllOrders(this);" controlname="selectRecordOrders" id="selectRecordOrders" name="chkHeader" class="input-block" coltype="checkbox"/></th>');
            }
        } else {
            $("#" + OrderSet_RadiologyOrder.params.PanelID + " #" + OrderSet_RadiologyOrder.resultSearchGridId + " th#selectRecordOrders").remove();
        }



        if ($("#" + OrderSet_RadiologyOrder.params.PanelID + " #pnlRadiologyResult_Result").css("display") == "none") {
            $("#" + OrderSet_RadiologyOrder.params.PanelID + " #pnlRadiologyResult_Result").show();
        }
        AppPrivileges.GetFormPrivileges("Orders and Results_Radiology Result", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var self = $("#" + OrderSet_RadiologyOrder.params.PanelID + " form");

                var myJSON = self.getMyJSONByName();

                // search specific on caller Id
                if (caller != null) {
                    if (caller.indexOf("RadiologyResultDetail") >= 0) {
                        myJSON = null;
                        // Reload Providers
                        OrderSet_RadiologyOrder.fillProvider("frmClinicalRadiologyResult");
                    }
                }

                OrderSet_RadiologyOrder.searchRadiologyResults(myJSON, radId, PageNo, rpp).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {

                        OrderSet_RadiologyOrder.radiologyResultsGridLoad(response);

                        var totalRows = $("#" + OrderSet_RadiologyOrder.params.PanelID + " #" + OrderSet_RadiologyOrder.resultSearchGridId + " tr").length;
                        totalRows -= 1;
                        var selectedRows = $("#" + OrderSet_RadiologyOrder.params.PanelID + " #" + OrderSet_RadiologyOrder.resultSearchGridId + " tbody tr input:checked").length;
                        if (totalRows == selectedRows) {
                            $("#" + OrderSet_RadiologyOrder.params.PanelID + " #" + OrderSet_RadiologyOrder.resultSearchGridId + " #selectRecordOrders").prop("checked", true);
                        }
                        else {
                            $("#" + OrderSet_RadiologyOrder.params.PanelID + " #" + OrderSet_RadiologyOrder.resultSearchGridId + " #selectRecordOrders").prop("checked", false);
                        }

                        var TableControl = OrderSet_RadiologyOrder.params.PanelID + " #pnlRadiologyResult_Result #" + OrderSet_RadiologyOrder.resultSearchGridId;
                        var PagingPanelControlID = OrderSet_RadiologyOrder.params.PanelID + " #dgvRadiologyResult_Paging";
                        var ClassControlName = "OrderSet_RadiologyOrder";
                        var PagesToDisplay = 15;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(
                            CreatePagination(response.RadiologyResultCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                                OrderSet_RadiologyOrder.radiologyResultsSearch(PrimaryID, PageNumber, ResultPerPage);
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


    radiologyResultDelete: function (radiologyResultId, event, PageNo, rpp) {
        var strMessage = "";
        event.stopPropagation();
        AppPrivileges.GetFormPrivileges("Orders and Results_Radiology Result", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = radiologyResultId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        var self = $("#" + OrderSet_RadiologyOrder.params.PanelID + " form");
                        var myJSON = self.getMyJSONByName();

                        OrderSet_RadiologyOrder.deleteRadiologyResult(myJSON, radiologyResultId, PageNo, rpp).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                utility.DisplayMessages(response.Message, 1);
                                OrderSet_RadiologyOrder.radiologyResultsSearch();
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

    deleteRadiologyResult: function (radiologyData, radiologyResultId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = JSON.parse(radiologyData);
        objData["RadiologyResultId"] = radiologyResultId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PatientId"] = OrderSet_RadiologyOrder.patientId;
        objData["commandType"] = "delete_radiologyresult";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyResult", "RadiologyResult");

    },


    searchRadiologyResults: function (radResultData, radResultId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = {};
        if (radResultData != null) {
            objData = JSON.parse(radResultData);
        }
        objData["RadiologyResultId"] = radResultId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PatientId"] = OrderSet_RadiologyOrder.patientId;
        objData["Test"] = objData["ResultCPTCode"];
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;

        objData["LabId"] = $('#' + OrderSet_RadiologyOrder.params.PanelID + ' #frmClinicalRadiologyResult #ddlLaboratory').val();

        objData["ProviderId"] = $('#' + OrderSet_RadiologyOrder.params.PanelID + ' #frmClinicalRadiologyResult #txtProvider').val();
        objData["OrderFromDate"] = $('#' + OrderSet_RadiologyOrder.params.PanelID + ' #frmClinicalRadiologyResult #dpStartDate').val();
        objData["Status"] = $('#' + OrderSet_RadiologyOrder.params.PanelID + ' #frmClinicalRadiologyResult #ddlStatus option:selected').val();
        objData["OrderToDate"] = $('#' + OrderSet_RadiologyOrder.params.PanelID + ' #frmClinicalRadiologyResult #dpToDate').val();
        objData["OrderNo"] = $('#' + OrderSet_RadiologyOrder.params.PanelID + ' #frmClinicalRadiologyResult #txtOrderNumber').val();
        objData["AllResults"] = 1;

        objData["commandType"] = "search_radiologyResults";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyResult", "RadiologyResult");
    },

    radiologyResultsGridLoad: function (response) {

        //Start 24-05-2016 Muhammad Arshad Remove Duplicate search issue on Datatable
        if ($.fn.dataTable.isDataTable("#" + OrderSet_RadiologyOrder.params.PanelID + " #pnlRadiologyResult_Result #" + OrderSet_RadiologyOrder.resultSearchGridId)) {
            $("#" + OrderSet_RadiologyOrder.params.PanelID + " #pnlRadiologyResult_Result #" + OrderSet_RadiologyOrder.resultSearchGridId).dataTable().fnClearTable();
            $("#" + OrderSet_RadiologyOrder.params.PanelID + " #pnlRadiologyResult_Result #" + OrderSet_RadiologyOrder.resultSearchGridId).dataTable().fnDestroy();

        }
        $("#" + OrderSet_RadiologyOrder.params.PanelID + " #pnlRadiologyResult_Result #" + OrderSet_RadiologyOrder.resultSearchGridId + " tbody").find("tr").remove();
        ////Data table is not destroying fully, so we have to initialize it once
        //$("#" + OrderSet_RadiologyOrder.params.PanelID + " #pnlRadiologyResult_Result #" + OrderSet_RadiologyOrder.resultSearchGridId).dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it

        ////Remove the data from table body
        //$("#" + OrderSet_RadiologyOrder.params.PanelID + " #pnlRadiologyResult_Result #" + OrderSet_RadiologyOrder.resultSearchGridId + " tbody").find("tr").remove(); //Removing all the table data from table body

        if (response.RadiologyResultCount > 0) {
            var RadiologyLoadJSONData = JSON.parse(response.RadiologyLoad_JSON); //Parsing array to JSON
            $.each(RadiologyLoadJSONData, function (i, item) {


                var currentDate = new Date();
                var currentTime = currentDate.toLocaleTimeString().replace(/([\d]+:[\d]{2})(:[\d]{2})(.*)/, "$1$3");

                //Convert Date to prper date time format
                if (item.CreatedOn != null) {
                    currentDate = new Date(item.CreatedOn).toLocaleDateString();
                    currentTime = new Date(item.CreatedOn).toLocaleTimeString().replace(/([\d]+:[\d]{2})(:[\d]{2})(.*)/, "$1$3");
                }

                var $row = $('<tr/>');
                // $row.attr("onclick", "ClinicalLabOrderDetail.LabEdit('" + item.LabOrderId + "',event);");
                $row.attr("id", "gvLab_row" + item.RadiologyOrderResultId);
                $row.attr("RadiologyOrderResultId", item.RadiologyOrderResultId);
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
                    if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.RadiologyOrderResultId + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                        Checked = " ";
                    } else {
                        Checked = " checked";
                    }
                } else {
                    if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.RadiologyOrderResultId + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                        Checked = " checked";
                    } else {
                        Checked = "";
                    }
                }
                var parentControl = "";
                var parentControlPanelID = null;

                if (OrderSet_RadiologyOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                    parentControl = 'OrderSet_RadiologyOrder';
                    parentControlPanelID = OrderSet_RadiologyOrder.params.PanelID;

                    SelectionCheckBoxColumn = '<td><input type="checkbox" onchange="OrderSet_RadiologyOrder.enableAddResult(this,event);" id="' + item.RadiologyOrderResultId + '" name="SelectCheckBoxOrder" ' + Checked + ' class="input-block"/></td>';
                }
                else if (OrderSet_RadiologyOrder.params.ParentCtrl == "clinicalTabFaceSheet") {
                    parentControl = 'OrderSet_RadiologyOrder';
                    parentControlPanelID = OrderSet_RadiologyOrder.params.PanelID;
                }
                else {
                    parentControl = 'clinicalTabRadiologyOrder';
                    SelectionCheckBoxColumn = "";
                }
                //End//21-04-2016//Ahmad Raza//logic to check uncheck order if it is attached with Note

                var loincArray = item.LOINC.split('|');
                var loincDescriptionArray = item.LOINCDescription.split('|');


                var divLoinc = '<div>';
                $.each(loincDescriptionArray, function (index, item) {

                    var loinc = "";//(loincArray[index] == null || loincArray[index] == "") ? "" : loincArray[index] + " - ";
                    var loincDescription = loincDescriptionArray[index];
                    var loincSegment = "<p>" + loinc + loincDescription + Clinical_InfoButtonView.GenerateInfoLink(loinc, parentControl, 3, parentControlPanelID) + "</p>";
                    divLoinc = divLoinc + loincSegment;

                });
                divLoinc = divLoinc + "</div>"
                var attachementIcon = "";
                if (item.IsAttachmentExists == "True") {
                    attachementIcon = '<div class="dropdown" style="display:inline-block;">' +
                                    '<a id="btnViewAttachment"' + item.RadiologyOrderResultId + ' href="#" class="dropdown-toggle fa fa-paperclip btn btn-link btn-xs p-none" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false" onclick="ClinicalRadiologyResultDetail.loadAttachments(\'OrderSet_RadiologyOrder\',\'' + item.RadiologyOrderId + '\',\'' + item.RadiologyOrderResultId + ' \',\'' + OrderSet_RadiologyOrder.resultSearchGridId + '\')"></a>' +
                                    '<ul id="menuViewAttachment' + item.RadiologyOrderResultId + '" class="dropdown-menu" aria-labelledby="btnViewAttachment"></ul></div>';

                }
                //Start//20-04-2016//Ahmad Raza//implimented logic to show icon for showing DBAudit History
                $row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.RadiologyOrderResultId + '</td><td><a class="btn btn-xs" href="#" onclick="OrderSet_RadiologyOrder.radiologyResultDelete(\'' + item.RadiologyOrderResultId + '\', event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="OrderSet_RadiologyOrder.radiologyResultAddEdit(\'' + item.RadiologyOrderResultId + '\',\'' + item.RadiologyOrderId + '\', false);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="OrderSet_RadiologyOrder.printRadiologyResult(\'' + item.RadiologyOrderId + '\',\'' + item.RadiologyOrderResultId + '\',\'' + item.Status + '\',event );" title="View Result"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="OrderSet_RadiologyOrder.showRadiologyResultHistory(\'' + item.RadiologyOrderResultId + '\', event );" title="Record History"> <i class="fa fa-history blue"></i></a>' + attachementIcon + '</td><td>'
                        + currentDate + " " + currentTime + '</td><td>' + divLoinc + '</td><td>' + item.Laboratory + '</td><td>' + item.OrderNo + '</td><td>' + item.Status + '</td><td>' + item.ProviderName + '</td><td>' + item.AssignedTo + '</td>');
                $("#" + OrderSet_RadiologyOrder.params.PanelID + " #pnlRadiologyResult_Result #" + OrderSet_RadiologyOrder.resultSearchGridId + " tbody").last().append($row);
            });
        }
        else {
            $("#" + OrderSet_RadiologyOrder.params.PanelID + ' #pnlRadiologyResult_Result #' + OrderSet_RadiologyOrder.resultSearchGridId).DataTable({
                "language": {
                    "emptyTable": "No Radiology Result Found"
                }, "autoWidth": false, "bLengthChange": false, "aaSorting": [[1, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + OrderSet_RadiologyOrder.params.PanelID + ' #pnlRadiologyResult_Result #' + OrderSet_RadiologyOrder.resultSearchGridId))
            ;
        else {
            $("#" + OrderSet_RadiologyOrder.params.PanelID + " #pnlRadiologyResult_Result #" + OrderSet_RadiologyOrder.resultSearchGridId).DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [[1, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        }

        EMRUtility.fixDataTableDuplication("#" + OrderSet_RadiologyOrder.params.PanelID + " #pnlRadiologyResult_Result");
    },

    printRadiologyResult: function (LabId, ResultId, status, event) {
        // if (status == 'Signed') {
        var params = [];
        var PanelID = "";
        params["FromAdmin"] = "0";
        params["UserId"] = globalAppdata['AppUserId'];
        params["PatientId"] = $('#PatientProfile #hfPatientId').val();
        //  params["ParentCtrl"] = Clinical_LabOrder.params.TabID;
        params["ParentCtrl"] = (OrderSet_RadiologyOrder.params.ParentCtrl != "clinicalTabProgressNote" && OrderSet_RadiologyOrder.params.ParentCtrl != "clinicalTabFaceSheet") ? OrderSet_RadiologyOrder.params.TabID : "OrderSet_RadiologyOrder";
        params["RadiologyOrderId"] = LabId;
        params["RadiologyResultId"] = ResultId;
        if (OrderSet_RadiologyOrder.params.ParentCtrl == "clinicalTabProgressNote") {
            PanelID = 'pnlClinicalProgressNote #pnlOrderSetRadiologyOrder';
            params["PanelID"] = 'pnlClinicalProgressNote #pnlOrderSetRadiologyOrder';
            params["PrPanelID"] = 'pnlClinicalProgressNote #pnlOrderSetRadiologyOrder';
        }
        else if (OrderSet_RadiologyOrder.params.ParentCtrl == "clinicalTabFaceSheet") {
            PanelID = 'pnlClinicalFaceSheet #pnlClinicalLabOrder';
            params["PanelID"] = 'pnlClinicalFaceSheet #pnlOrderSetRadiologyOrder';
            params["PrPanelID"] = 'pnlClinicalFaceSheet #pnlOrderSetRadiologyOrder';
        }
        else {
            PanelID = 'pnlOrderSetRadiologyOrder';
            params["PanelID"] = 'pnlOrderSetRadiologyOrder';
            params["PrPanelID"] = 'pnlOrderSetRadiologyOrder';
        }
        LoadActionPan('Clinical_RadiologyResultView', params, PanelID);
        //   }
        event.stopPropagation();
    },



    enableAddResult: function (obj, event) {
        if (event != null) {
            event.stopPropagation();
        }

        var totalRows = $("#" + OrderSet_RadiologyOrder.params.PanelID + " #" + OrderSet_RadiologyOrder.resultSearchGridId + " tr").length;
        totalRows -= 1;
        var selectedRows = $("#" + OrderSet_RadiologyOrder.params.PanelID + " #" + OrderSet_RadiologyOrder.resultSearchGridId + " tbody tr input:checked").length;
        if (totalRows == selectedRows) {
            $("#" + OrderSet_RadiologyOrder.params.PanelID + " #" + OrderSet_RadiologyOrder.resultSearchGridId + " #selectRecordOrders").prop("checked", true);
        }
        else {
            $("#" + OrderSet_RadiologyOrder.params.PanelID + " #" + OrderSet_RadiologyOrder.resultSearchGridId + " #selectRecordOrders").prop("checked", false);
        }

        if ($(obj).is(':checked')) {
            if ($.inArray(obj.id + 'rslt', Clinical_ProgressNote.AttachedNoteComponentIds) == -1) {
                Clinical_ProgressNote.AttachedNoteComponentIds.push(obj.id + 'rslt');
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
                Clinical_ProgressNote.DetachedNoteComponentIds.push(obj.id + 'rslt');
            }
        }
    },


    addResultsToNotes: function (selectedAttachedOrders, selectedDetachedOrders) {
        var noteHTMLCtrl = '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML';
        //if ($.fn.dataTable.isDataTable("#" + OrderSet_RadiologyOrder.params.PanelID + " #pnlRadiologyOrder_Result #" + OrderSet_RadiologyOrder.orderSearchGridId)) {
        //    $("#" + OrderSet_RadiologyOrder.params.PanelID + " #pnlRadiologyOrder_Result #" + OrderSet_RadiologyOrder.orderSearchGridId).dataTable().fnClearTable();
        //    $("#" + OrderSet_RadiologyOrder.params.PanelID + " #pnlRadiologyOrder_Result #" + OrderSet_RadiologyOrder.orderSearchGridId).dataTable().fnDestroy();
            if ($("#" + OrderSet_RadiologyOrder.resultSearchGridId + "").dataTable().fnSettings().aoData.length == 0) {
                //   Clinical_LabOrder.noActiveLabOrderSoapText();
            }
      //  }
        else {

            if ($("#" + OrderSet_RadiologyOrder.params.PanelID + " #selectRecordOrders").prop('checked') == true || $("#" + OrderSet_RadiologyOrder.params.PanelID + " #selectRecordOrders").prop('checked') == false) {

                $("#" + OrderSet_RadiologyOrder.params.PanelID + " #" + OrderSet_RadiologyOrder.resultSearchGridId + " tbody").find('input[type="checkbox"]').each(function () {
                    OrderSet_RadiologyOrder.enableAddResult(this);
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
                AttachedSelectedOrders = EMRUtility.slicefunc(AttachSelectedOrdersAndResults, "rslt", 0, -4);
                DettachedSelectedOrders = EMRUtility.slicefunc(DettachSelectedOrdersAndResults, "rslt", 0, -4);
            }

            if (AttachedSelectedOrders != null && AttachedSelectedOrders != '') {
                for (var i = 0; i < Clinical_ProgressNote.AttachedNoteComponentIds.length; i++) {
                    var ALid = Clinical_ProgressNote.AttachedNoteComponentIds[i];
                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_RadiologyOrderDetail_Main' + ALid).length != 0) {
                        var index = AttachedSelectedOrders.indexOf(ALid);
                        if (index > -1) {
                            AttachedSelectedOrders.splice(index, 1);
                        }
                    }
                }
            }
            var detachedvalues = DettachedSelectedOrders;
            if (detachedvalues.join() != null && detachedvalues.join() != '') {
                OrderSet_RadiologyOrder.detachRadiologyResult_FromNotes(detachedvalues).done(function () {
                    if (AttachedSelectedOrders.join() != null && AttachedSelectedOrders.join() != '') {
                        OrderSet_RadiologyOrder.attachRadiologyResult_WithNotes(AttachedSelectedOrders);
                    } else {
                        Clinical_ProgressNote.saveComponentSOAPText('Radiology Order');
                    }
                });
            }else
            if (AttachedSelectedOrders.join() != null && AttachedSelectedOrders.join() != '') {
                OrderSet_RadiologyOrder.attachRadiologyResult_WithNotes(AttachedSelectedOrders);
            }

        }
    },
    attachRadiologyResult_WithNotes: function (AttachedSelectedOrders) {
        OrderSet_RadiologyOrder.getRadiologyResultInfo(AttachedSelectedOrders.join()).done(function () {
            setTimeout(function () {
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                if (OrderSet_RadiologyOrder.params != null && OrderSet_RadiologyOrder.params.PanelID.indexOf('pnlOrderSetRadiologyOrder') != -1) {
                    OrderSet_RadiologyOrder.radiologyResultsSearch();
                }
            }, 5);
        });
    },
    detachRadiologyResult_FromNotes: function (detachedvalues) {
        var dfd = new $.Deferred();
        OrderSet_RadiologyOrder.detachRadiologyResultFromNotesDBCall(detachedvalues.join()).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                for (var i = 0; i < detachedvalues.length; i++) {
                    var ALid = detachedvalues[i];
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_RadiologyResultDetail_Main' + ALid).remove();
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
            dfd.resolve('ok');
        });
        return dfd.promise();
    },
    getRadiologyResultInfo: function (radiologyOrderId) {
        var dfd = new $.Deferred();
        if (radiologyOrderId == null || radiologyOrderId == '') {
            return false;
        }

        OrderSet_RadiologyOrder.getResultsForSOAP_DBCall(radiologyOrderId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.ResultSoapCount > 0) {
                        OrderSet_RadiologyOrder.createResultBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', radiologyOrderId);
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
        objData["RadiologyResultIDs"] = labOrderId;
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["PatientId"] = OrderSet_RadiologyOrder.params.patientID;
        objData["commandType"] = "get_results_forsoap";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyResult", "RadiologyResult");
    },


    detachRadiologyResultFromNotesDBCall: function (LabId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["RadiologyResultIDs"] = LabId;
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
        objData["commandType"] = "detach_radiologyresult_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "RadiologyResult", "RadiologyResult");
    },

    createResultBodyHTML: function (response, noteHTMLCtrl, resultId,hidemessage) {
        OrderSet_RadiologyOrder.checkResultExists();
        if (response.RadiologyResultFill_JSON != null && response.RadiologyResultFill_JSON != '') {
            var MedicationsSOAPJSON = JSON.parse(response.RadiologyResultFill_JSON);
            //      var medicationReviewSoapJSON = [];
            if (response.MedicationReviewSoap_JSON != null) {
                //          medicationReviewSoapJSON = JSON.parse(response.MedicationReviewSoap_JSON);
            }

            var $mainDivMedications = $(document.createElement('div'));
            if (MedicationsSOAPJSON == null || MedicationsSOAPJSON.length == 0) {
                //}
            } else {
                if ($(noteHTMLCtrl + ' clinical_radiologyresults').parent().parent().find('#Cli_RadiologyResultDetail_Main0').length != 0) {
                    $(noteHTMLCtrl + ' #Cli_RadiologyResultDetail_Main0').parent().remove();
                }
            }

            var AListId = [];
            $.each(MedicationsSOAPJSON, function (index, element) {

                var ALid = element.RadiologyOrderResultId;
                //             var $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(element.NDCID, 'clinicalTabProgressNote', 1);
                var $SectionBodyMedications = $(document.createElement('section'));
                $SectionBodyMedications.attr('id', "Cli_RadiologyResultDetail_Main" + ALid);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_RadiologyResultDetail_" + ALid);
                var $ListMedications = $(document.createElement('ul'));
                var duration = "";
                $ListMedications.attr('class', 'list-unstyled')

                $SectionBodyMedications.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_RadiologyResultDetail_" + ALid + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_RadiologyResultDetail_Main" + ALid + '"  ><i class="fa fa-times"></i></a></div> ');
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
                //if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML clinical_medications').parent().parent().find('#Cli_Medications_Main' + ALid).length == 0) {
                if ($(noteHTMLCtrl + ' clinical_radiologyresults').parent().parent().find('#Cli_RadiologyResultDetail_Main' + ALid).length == 0) {
                    AListId.push(ALid);
                    $mainDivMedications.append($SectionBodyMedications);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(noteHTMLCtrl + ' clinical_radiologyresults').parent().parent().find('#Cli_RadiologyResultDetail_Main' + ALid + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(noteHTMLCtrl + ' clinical_radiologyresults').parent().parent().find('#Cli_RadiologyResultDetail_Main' + ALid + ' ul li:Last').get(0).outerHTML;
                    }
                    $(noteHTMLCtrl + ' clinical_radiologyresults').parent().parent().find('#Cli_RadiologyResultDetail_Main' + ALid).html($SectionBodyMedications.html());
                    $(noteHTMLCtrl + ' clinical_radiologyresults').parent().parent().find('#Cli_RadiologyResultDetail_Main' + ALid + ' ul').append(CommentHTML);;
                }


            });

            if (AListId.join(",") != "") {
                resultId = AListId.join(",");
            }
            if ($mainDivMedications.html() != '') {
                OrderSet_RadiologyOrder.updateResultHtml($mainDivMedications.html(), resultId, noteHTMLCtrl, hidemessage);
            } else {
                OrderSet_RadiologyOrder.updateResultHtml('', resultId, noteHTMLCtrl, hidemessage);
                Clinical_ProgressNote.saveComponentSOAPText('Radiology Results', hidemessage);
            }
        }

    },

    checkResultExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_radiologyresults').length == 0) {

            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ObjectiveNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="RadiologyResultsComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_radiologyresults title="Radiology Results"  id="' + this.id + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'RadiologyResults\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Radiology Results">Radiology Results</a> ' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'RadiologyResults\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_radiologyresults> </header></li>');
            Clinical_ProgressNote.CreateNotesComponent_Buttons($('#' + Clinical_ProgressNote.params["PanelID"] + ' #hfNotesId').val());
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
                $(this).find('.closeBtn').removeClass('hidden');
                $(this).css('background', '#EAF1F8');
            });

            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
                $(this).find('.closeBtn').addClass('hidden');
                $(this).css('background', '#fff');
            });
        }
    },

    updateResultHtml: function (resultHTML, resultID, noteHTMLCtrl,hidemessage) {
        $(noteHTMLCtrl + ' clinical_radiologyresults').parent().parent().addClass('initialVisitBody');
        if (resultHTML != '') {
            $(noteHTMLCtrl + ' clinical_radiologyresults').parent().parent().append(resultHTML);
        }

        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (resultHTML != '' && resultID != null && resultID != '' && resultID != '0') {
            OrderSet_RadiologyOrder.attachResultFromNotes(resultID, hidemessage);
        }

    },

    attachResultFromNotes: function (resultID, hidemessage) {
        var selectedValue = resultID;
        if (selectedValue == "" || selectedValue == "undefined") {
        }
        else {
            OrderSet_RadiologyOrder.attachResultsWithNotes_DBCall(selectedValue).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    Clinical_ProgressNote.saveComponentSOAPText('Radiology Results', hidemessage);
                    $('#' + resultID).remove();

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    attachResultsWithNotes_DBCall: function (resultId) {
        var objData = new Object();
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["RadiologyResultIDs"] = resultId;
        objData["commandType"] = "attach_results_with_notes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyResult", "RadiologyResult");
    },

    detach_ComponentsRadiologyResult: function (ComponentName, IsUpdate, radiologyResultComponentRemove) {
        var radiologyOrderIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_radiologyresults').parent().parent().find('section[id*="Cli_RadiologyResultDetail_Main"]').map(function () {
            return this.id.replace("Cli_RadiologyResultDetail_Main", "");
        }).get().join(',');
        if (radiologyOrderIds == "" || radiologyOrderIds == "undefined") {
            Clinical_ProgressNote.saveComponentSOAPText('Radiology Order', true);
            utility.DisplayMessages('Successfully Deleted', 1);
        }
        else {
            OrderSet_RadiologyOrder.detachRadiologyResultFromNotesDBCall(radiologyOrderIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (IsUpdate) {

                        Clinical_ProgressNote.saveComponentSOAPText('Radiology Results',true);
                    }
                    utility.DisplayMessages(response.Message, 1);
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        if (radiologyResultComponentRemove) {
            $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Radiology Results']").remove();
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_radiologyresults').parent().parent().remove();
        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_radiologyresults').parent().parent().find('section[id*="Cli_RadiologyResultDetail_Main"]').remove();
        }
    },

    detachRadiologyResultFromNotesDBCall: function (radiologyId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["RadiologyResultIDs"] = radiologyId;
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
        objData["commandType"] = "detach_radiologyresult_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "RadiologyResult", "RadiologyResult");
    },

    detachRadiologyResultFromNotes: function (radiologyOrderId) {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('1', function () {
                var selectedValue = radiologyOrderId.replace('Cli_RadiologyResultDetail_Main', '');
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    OrderSet_RadiologyOrder.detachRadiologyResultFromNotes_DBCall(selectedValue).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            $('#' + radiologyOrderId).remove();

                            Clinical_ProgressNote.saveComponentSOAPText('Radiology Results');
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

    detachRadiologyResultFromNotes_DBCall: function (radiologyOrderId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["RadiologyResultIDs"] = radiologyOrderId;
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
        objData["commandType"] = "detach_radiologyresult_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "RadiologyResult", "RadiologyResult");
    },
    getLatestRadiologyResultByPatientId: function (hidemessage) {
        var strMessage = '';
        //   AppPrivileges.GetFormPrivileges("Notes_Notes", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            OrderSet_RadiologyOrder.getLatestRadiologyResultByPatientIdDBCall().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    OrderSet_RadiologyOrder.createResultBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hidemessage);
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

    getLatestRadiologyResultByPatientIdDBCall: function () {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["NoteId"] = Clinical_Notes.params.NotesId;
        objData["commandType"] = "getlatest_RadiologyResultby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyResult", "RadiologyResult");
    },

    //Function Name: showRadiologyResultHistory
    //Author Name: Humaira Yousaf
    //Created Date: 02-05-2016
    //Description: Shows Radiology order results history
    showRadiologyResultHistory: function (radiologyResultId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var ParentCtrl = 'clinicalTabRadiologyOrder';
        var ParentCtrlPanelID = null;
        if (OrderSet_RadiologyOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
            ParentCtrl = 'OrderSet_RadiologyOrder';
            ParentCtrlPanelID = OrderSet_RadiologyOrder.params.PanelID;
        }
        else if (OrderSet_RadiologyOrder.params["ParentCtrl"] == "clinicalTabFaceSheet") {
            ParentCtrl = 'OrderSet_RadiologyOrder';
            ParentCtrlPanelID = OrderSet_RadiologyOrder.params.PanelID;
        }
        EMRUtility.showCurrentItemHistory(OrderSet_RadiologyOrder.params.PanelID, null, null, "RadiologyOrderResult,RadiologyOrderResultDetail", null, ParentCtrl, radiologyResultId, ParentCtrlPanelID);
    },

    //Function Name: showAttachment
    //Author: Humaira Yousaf
    //Date :  02-05-2016
    //Description: shows Attachments
    showAttachment: function (PatDocID, event, obj) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        if (obj != null) {
            $(obj).parent().parent().parent().removeClass('open');
        }
        AppPrivileges.GetFormPrivileges("Documents", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["PatientID"] = $('#PatientProfile #hfPatientId').val();
                params["PatDocID"] = PatDocID;
                params["mode"] = "Edit";
                params["FromAdmin"] = "0";
                // params["ParentCtrl"] = "OrderSet_RadiologyOrder";


                if (OrderSet_RadiologyOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {

                    params["ParentCtrl"] = 'OrderSet_RadiologyOrder';
                    params["ParentCtrlPanelID"] = OrderSet_RadiologyOrder.params.PanelID;
                    LoadActionPan('Document_Viewer', params, OrderSet_RadiologyOrder.params.PanelID);
                }
                else {
                    params["ParentCtrl"] = 'clinicalTabRadiologyOrder';
                    LoadActionPan('Document_Viewer', params);
                }

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
}