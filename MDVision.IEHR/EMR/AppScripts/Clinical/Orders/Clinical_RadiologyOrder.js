Clinical_RadiologyOrder = {
    //Author: Humaira Yousaf
    //Date: 17-03-2016
    //This file will handle all actions performed for Radiology Order
    bIsFirstLoad: true,
    params: [],
    patientId: null,
    orderSearchGridId: 'dgvRadiologyOrder',
    orderExternalSearchGridId: 'dgvExternalRadiologyOrder',
    orderGridTableId: 'pnlRadiologyOrder_Result',
    orderExternalGridTableId: 'pnlExternalRadiologyOrder_Result',
    resultSearchGridId: 'dgvRadiologyResult',
    externalResultSearchGridId: 'dgvExternalRadiologyResult',

    detailParams: null,
    internalLabId: 0,
    //Author: Humaira Yousaf
    //Date: 17-03-2016 
    //This function will handle fill of Radiology Order
    Load: function (params) {


        //Start//13-06-2016 Abid Ali for retaining detail values
        Clinical_RadiologyOrder.detailParams = Clinical_RadiologyOrder.params;
        //End//13-06-2016 Abid Ali for retaining detail values

        Clinical_RadiologyOrder.params = params;
        Clinical_RadiologyOrder.patientId = $("div#PatientProfile #hfPatientId").val();
        if (Clinical_RadiologyOrder.params.ParentCtrl == "Clinical_FaceSheet") {
            Clinical_RadiologyOrder.patientId = Clinical_FaceSheet.params.patientID;
        }
        Clinical_RadiologyOrder.params.mode = "Add";

        if (Clinical_RadiologyOrder.params.PanelID != 'pnlClinicalRadiologyOrder') {
            Clinical_RadiologyOrder.params.PanelID = Clinical_RadiologyOrder.params.PanelID + ' #pnlClinicalRadiologyOrder';
        } else {
            Clinical_RadiologyOrder.params.PanelID = 'pnlClinicalRadiologyOrder';
        }

        $('#' + Clinical_RadiologyOrder.params.PanelID + ' #pnlRadiologyOrder_Search').resetAllControls(null);
        $('#' + Clinical_RadiologyOrder.params.PanelID + ' #pnlExternalRadiologyOrder_Search').resetAllControls(null);

        $('#' + Clinical_RadiologyOrder.params.PanelID + ' #pnlRadiologyResult_Search').resetAllControls(null);
        $('#' + Clinical_RadiologyOrder.params.PanelID + ' #pnlExternalRadiologyResult_Search').resetAllControls(null);

        if (Clinical_RadiologyOrder.params.ParentCtrl == 'clinicalTabProgressNote') {

            $("#" + Clinical_RadiologyOrder.params.PanelID + " #orders").attr('href', '#RadiologyOrder1');
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #results").attr('href', '#RadiologyResult1');

            //$("#" + Clinical_RadiologyOrder.params.PanelID + " #internalorders").attr('href', '#RadiologyInternalOrder1');
            //$("#" + Clinical_RadiologyOrder.params.PanelID + " #externalorders").attr('href', '#RadiologyExternalOrder1');
            //$("#" + Clinical_RadiologyOrder.params.PanelID + " #internalresults").attr('href', '#RadiologyInternalResult1');
            //$("#" + Clinical_RadiologyOrder.params.PanelID + " #externalresults").attr('href', '#RadiologyExternalResult1');


            $("#" + Clinical_RadiologyOrder.params.PanelID + " #RadiologyOrder").attr('id', 'RadiologyOrder1');
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #RadiologyResult").attr('id', 'RadiologyResult1');

            //$("#" + Clinical_RadiologyOrder.params.PanelID + " #RadiologyInternalOrder").attr('id', '#RadiologyInternalOrder1');
            //$("#" + Clinical_RadiologyOrder.params.PanelID + " #RadiologyExternalOrder").attr('id', '#RadiologyExternalOrder1');
            //$("#" + Clinical_RadiologyOrder.params.PanelID + " #RadiologyInternalResult").attr('id', '#RadiologyInternalResult1');
            //$("#" + Clinical_RadiologyOrder.params.PanelID + " #RadiologyExternalResult").attr('id', '#RadiologyExternalResult1');


            $('.nav-tabs').tab();

            $("#" + Clinical_RadiologyOrder.params.PanelID + " #btnAddRadiologyOrderToNote").show();
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #btnAddRadiologyResultToNote").show();


        }
        else {
            //For Data table in tab control
            Clinical_RadiologyOrder.orderSearchGridId = "dgvRadiologyOrder";
            Clinical_RadiologyOrder.resultSearchGridId = "dgvRadiologyResult";


            $("#" + Clinical_RadiologyOrder.params.PanelID + " #btnAddRadiologyOrderToNote").hide();
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #btnAddRadiologyResultToNote").hide();
        }

        IsBackgroundLoaderShow = false;
        ShowHideLoaderOnScreen(false);
        xhrPool = [];

        var self = $('#' + Clinical_RadiologyOrder.params.PanelID);
        if (Clinical_RadiologyOrder.bIsFirstLoad == true) {
            //Start 21-03-2016 Humaira Yousaf to validate Radiology
            Clinical_RadiologyOrder.ValidateRadiology();
            //End 21-03-2016 Humaira Yousaf to validate Radiology

            Clinical_RadiologyOrder.fillProvider();

            IsBackgroundLoaderShow = true;
            ShowHideLoaderOnScreen(true);
            Clinical_RadiologyOrder.radiologyOrderSearch().done(function () {
                ShowHideLoaderOnScreen(false);
            });
            IsBackgroundLoaderShow = false;
            Clinical_RadiologyOrder.externalRadiologyOrderSearch();
            Clinical_RadiologyOrder.radiologyResultsSearch();
            Clinical_RadiologyOrder.externalRadiologyResultsSearch();


            //Start 13-04-2016 Abid Ali to load Laboratories
            //Clinical_LabOrder.LoadLabs('ddlLaboratory', Clinical_RadiologyOrder.params.PanelID);           
            //End 13-04-2016 Abid Ali to load Laboratories
        }

        EMRUtility.ValidateFromToDate('frmClinicalRadiologyOrder', 'dpStartDate', 'dpToDate', true, function () { }, function () { }, "To Date should be greater than From Date");
        EMRUtility.ValidateFromToDate('frmClinicalRadiologyResult', 'dpStartDate', 'dpToDate', true, function () { }, function () { }, "To Date should be greater than From Date");
        //Start//17-06-2016//Ahmad Raza// Tabs Selection Logic
        var orderHref = "#RadiologyOrder";
        var resultHref = "#RadiologyResult";
        if (Clinical_RadiologyOrder.params.ParentCtrl == 'clinicalTabProgressNote') {
            orderHref = "#RadiologyOrder1";
            resultHref = "#RadiologyResult1";
        }
        if (Clinical_RadiologyOrder.params.Type == "Order") {
            $('#ulRadilogyOrderTabsItems a[href="' + orderHref + '"]').trigger('click');
        }
        else if (Clinical_RadiologyOrder.params.Type == "Result") {
            $('#ulRadilogyOrderTabsItems a[href="' + resultHref + '"]').trigger('click');
        }
        //End//17-06-2016//Ahmad Raza// Tabs Selection Logic

        if (Clinical_RadiologyOrder.params.ParentCtrl == "clinicalTabFaceSheet") {
            EMRUtility.MakeFaceSheetPager('#' + Clinical_RadiologyOrder.params.PanelID + " div#FaceSheetPager", Clinical_RadiologyOrder.params.FaceSheetComponents, 'radiologyorders');
        } else if (Clinical_RadiologyOrder.params.ParentCtrl == "Clinical_FaceSheet") {
            EMRUtility.MakeFaceSheetPager('#pnlClinicalFaceSheet #pnlClinicalRadiologyOrder' + " div#FaceSheetPager", Clinical_RadiologyOrder.params.FaceSheetComponents, 'radiologyorders');
        }

        // collapse expand panel for search criteria IMP-1231
        Clinical_RadiologyOrder.SetCollapseExpandPanelResult();
        Clinical_RadiologyOrder.SetCollapseExpandPanelOrders();

        utility.callbackAfterAllDOMLoaded(function () {

            var faceSheetpager = $('#FaceSheetPager');
            if (faceSheetpager.length > 0) {
                //show/hide button controls acording to resoltion
                EMRUtility.HideShowFaceSheetPagerBtnControls(faceSheetpager);
                $("#FaceSheetPager").find("div.slick-track").css("width", "1412px");
            }
        });

     // #fix EMR-6357
     var listRadInternal=   $("#" + Clinical_RadiologyOrder.params.PanelID + " #listRadiologyInternalOrder");
     var listRadExternal= $("#" + Clinical_RadiologyOrder.params.PanelID + " #listRadiologyExternalOrder");
     if (!listRadInternal.hasClass('active') && !listRadExternal.hasClass('active'))
     {
         Clinical_RadiologyOrder.activeTab('Internal');
     }

     Clinical_RadiologyOrder.activeTabResult('Internal');

    },

    activeTab: function (tab) {
        $("#" + Clinical_RadiologyOrder.params.PanelID + " #listRadiologyExternalOrder,#listRadiologyInternalOrder,#RadiologyExternalOrder,#RadiologyInternalOrder").removeClass('active');
        if (tab == "Internal") {
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #listRadiologyInternalOrder").addClass('active');
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #RadiologyInternalOrder").addClass('active');
        }
        else {
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #listRadiologyExternalOrder").addClass('active');
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #RadiologyExternalOrder").addClass('active');
        }
    },
    activeTabResult: function (tab) {
        $("#" + Clinical_RadiologyOrder.params.PanelID + " #listRadiologyInternalResult,#listRadiologyExternalResult,#RadiologyInternalResult,#RadiologyExternalResult").removeClass('active');
        if (tab == "Internal") {
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #listRadiologyInternalResult").addClass('active');
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #RadiologyInternalResult").addClass('active');
        }
        else {
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #listRadiologyExternalResult").addClass('active');
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #RadiologyExternalResult").addClass('active');
        }
    },
    SetCollapseExpandPanelResult: function () {
        var pnlId = "RadiologyResult";
        if (Clinical_RadiologyOrder.params.ParentCtrl == 'clinicalTabProgressNote') {
            pnlId = "RadiologyResult1";
        }

        //1- Initialization
        $('#' + Clinical_RadiologyOrder.params.PanelID + ' #' + pnlId + ' .splitterBtn').html('<a></a>');
        EMRUtility.changeIcon($('#' + Clinical_RadiologyOrder.params.PanelID + ' #' + pnlId + ' .splitterBtn a'));

        $('#' + Clinical_RadiologyOrder.params.PanelID + ' #' + pnlId + ' .splitterBtn a').click(function (e) {
            $(this).parent('.splitterBtn').prev().slideToggle(250).toggleClass('active');
            var a = $(this);
            EMRUtility.changeIcon(a);
        });

        //2- Default settings
        if (globalAppdata['IsSearchCriteriaExpand'] && globalAppdata['IsSearchCriteriaExpand'].toLowerCase() == 'true') {
            $('#' + Clinical_RadiologyOrder.params.PanelID + ' #' + pnlId + '  #splitterBody').attr('class', 'splitterBody active');
            $('#' + Clinical_RadiologyOrder.params.PanelID + ' #' + pnlId + '  #splitterBody').show();
        }
        else {
            $('#' + Clinical_RadiologyOrder.params.PanelID + ' #' + pnlId + '  #splitterBody').removeClass('splitterBody active');
            $('#' + Clinical_RadiologyOrder.params.PanelID + ' #' + pnlId + '  #splitterBody').hide();
        }

    },
    SetCollapseExpandPanelOrders: function () {
        var pnlId = "RadiologyOrder";
        if (Clinical_RadiologyOrder.params.ParentCtrl == 'clinicalTabProgressNote') {
            pnlId = "RadiologyOrder1";
        }

        //1- Initialization
        $('#' + Clinical_RadiologyOrder.params.PanelID + ' #' + pnlId + ' .splitterBtn').html('<a></a>');
        EMRUtility.changeIcon($('#' + Clinical_RadiologyOrder.params.PanelID + ' #' + pnlId + ' .splitterBtn a'));

        $('#' + Clinical_RadiologyOrder.params.PanelID + ' #' + pnlId + ' .splitterBtn a').click(function (e) {
            $(this).parent('.splitterBtn').prev().slideToggle(250).toggleClass('active');
            var a = $(this);
            EMRUtility.changeIcon(a);
        });

        //2- Default settings
        if (globalAppdata['IsSearchCriteriaExpand'] && globalAppdata['IsSearchCriteriaExpand'].toLowerCase() == 'true') {
            $('#' + Clinical_RadiologyOrder.params.PanelID + ' #' + pnlId + '  #splitterBody').attr('class', 'splitterBody active');
            $('#' + Clinical_RadiologyOrder.params.PanelID + ' #' + pnlId + '  #splitterBody').show();
        }
        else {
            $('#' + Clinical_RadiologyOrder.params.PanelID + ' #' + pnlId + '  #splitterBody').removeClass('splitterBody active');
            $('#' + Clinical_RadiologyOrder.params.PanelID + ' #' + pnlId + '  #splitterBody').hide();
        }

    },
    //Author: Abid Ali
    //Date: 13-06-2016
    //Description: Saves Radiology order detail values at patient level
    addRadiologyOrderDetailParams: function () {

        if (Clinical_RadiologyOrder.detailParams.hasData == true && Clinical_RadiologyOrder.detailParams.CurrentPatientId == $("div#PatientProfile #hfPatientId").val()) {
            Clinical_RadiologyOrder.params.hasData = true;
            Clinical_RadiologyOrder.params.ProviderName = Clinical_RadiologyOrder.detailParams.ProviderName;
            Clinical_RadiologyOrder.params.ProviderId = Clinical_RadiologyOrder.detailParams.ProviderId;
            Clinical_RadiologyOrder.params.AssigneeName = Clinical_RadiologyOrder.detailParams.AssigneeName;
            Clinical_RadiologyOrder.params.AssigneeId = Clinical_RadiologyOrder.detailParams.AssigneeId;
            Clinical_RadiologyOrder.params.Problems = Clinical_RadiologyOrder.detailParams.Problems;

            Clinical_RadiologyOrder.params.RadiologyId = detailParams.params.RadiologyId;
            Clinical_RadiologyOrder.params.BillingTypeId = Clinical_RadiologyOrder.detailParams.BillingTypeId;
            Clinical_RadiologyOrder.params.FacilityName = Clinical_RadiologyOrder.detailParams.FacilityName
            Clinical_RadiologyOrder.params.FacilityId = Clinical_RadiologyOrder.detailParams.FacilityId;

            Clinical_RadiologyOrder.params.CurrentPatientId = Clinical_RadiologyOrder.detailParams.CurrentPatientId;
        }
        else {
            Clinical_RadiologyOrder.params.hasData = false;
            Clinical_RadiologyOrder.params.ProviderName = "";
            Clinical_RadiologyOrder.params.ProviderId = "";
            Clinical_RadiologyOrder.params.AssigneeName = "";
            Clinical_RadiologyOrder.params.AssigneeId = "";
            Clinical_RadiologyOrder.params.Problems = "";

            Clinical_RadiologyOrder.params.RadiologyId = "";
            Clinical_RadiologyOrder.params.BillingTypeId = "";
            Clinical_RadiologyOrder.params.FacilityName = "";
            Clinical_RadiologyOrder.params.FacilityId = "";

            Clinical_RadiologyOrder.params.CurrentPatientId = "";
        }
    },

    //Function Name: fillProvider
    //Author: Ahmad Raza
    //Date: 08-04-2016
    //Description: This function will fill Provider dropdown
    fillProvider: function () {
        Clinical_RadiologyOrder.fillproviderDBCall().done(function (response) {
            if (response != "") {
                response = JSON.parse(response);

                var $ddlProvider = $('#' + Clinical_RadiologyOrder.params.PanelID + ' #RadiologySection #txtProvider');
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

        var hiddenCrtl = $('#' + Clinical_LabOrder.params.PanelID + ' #' + (refCtrlId != null ? refCtrlId : 'txtCPTCode'));
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "Clinical_RadiologyOrder", hiddenCrtl, true);

    },

    //Method Name: openCPTCode
    //Author: Ahmad Raza
    //Date: 21-03-2016
    //Description: This function will handle opening of CPT Search Popup
    /*openCPTCode: function () {
        var params = [];

        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Clinical_RadiologyOrder";
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtCPTCode";
        params["ParentCtrlPanelID"] = Clinical_RadiologyOrder.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, Clinical_RadiologyOrder.params.PanelID);
    },*/

    openCPTCode: function (refHiddenCtrl, refCtrl) {
        var params = [];

        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Clinical_RadiologyOrder';

        if (refHiddenCtrl != null && refHiddenCtrl != "") {
            params["RefHiddenCtrl"] = refHiddenCtrl;
        }
        else {
            params["RefHiddenCtrl"] = "hfCPTCode";
        }

        if (refCtrl != null && refCtrl != "") {
            params["RefCtrl"] = Clinical_RadiologyOrder.params.PanelID + " #" + refCtrl;
        }
        else {
            params["RefCtrl"] = Clinical_RadiologyOrder.params.PanelID + " #txtCPTCode";
        }
        params["RefCtrlDescription"] = "";

        params["ParentCtrlPanelID"] = Clinical_RadiologyOrder.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, Clinical_RadiologyOrder.params.PanelID);
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
        AppPrivileges.GetFormPrivileges("Orders and Results_Diagnostic Imaging Order", permissionState, "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                //------------
                if (Clinical_RadiologyOrder.params.ParentCtrl && Clinical_RadiologyOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                    params["IsFromNote"] = true;
                }
                else {
                    params["IsFromNote"] = false;
                }
                params["CurrentNotesProviderId"] = Clinical_RadiologyOrder.params["CurrentNotesProviderId"];
                if (Clinical_RadiologyOrder.params.isProblemAdded) {
                    params["isProblemAdded"] = true;
                } else {
                    params["isProblemAdded"] = false;
                }
                //------------
                if (radiologyOrderId != null && parseInt(radiologyOrderId) > 0) {
                    params["RadiologyOrderId"] = radiologyOrderId;
                    params["mode"] = "Edit";
                }
                else {
                    params["RadiologyOrderId"] = -1;
                    params["mode"] = "Add";
                    if (Clinical_RadiologyOrder.params["LastRadiologyLabId"]) {
                        params["LastRadiologyLabId"] = Clinical_RadiologyOrder.params["LastRadiologyLabId"];
                    } else {
                        params["LastRadiologyLabId"] = "";
                    }
                }

                params["FromAdmin"] = Clinical_RadiologyOrder.params["FromAdmin"];

                if (Clinical_RadiologyOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {

                    params["ParentCtrl"] = 'Clinical_RadiologyOrder';
                    params["ParentCtrlPanelID"] = Clinical_RadiologyOrder.params.PanelID;
                    params["FromNoteId"] = Clinical_RadiologyOrder.params.NotesId;
                    LoadActionPan('ClinicalRadiologyOrderDetail', params, Clinical_RadiologyOrder.params.PanelID);

                }
                else if (Clinical_RadiologyOrder.params["ParentCtrl"] == "clinicalTabFaceSheet" || Clinical_RadiologyOrder.params["ParentCtrl"] == "Clinical_FaceSheet") {
                    params["ParentCtrl"] = 'Clinical_RadiologyOrder';
                    params["ParentCtrlPanelID"] = Clinical_RadiologyOrder.params.PanelID;
                    LoadActionPan('ClinicalRadiologyOrderDetail', params, Clinical_RadiologyOrder.params.PanelID);
                }
                else {
                    params["ParentCtrl"] = 'clinicalTabRadiologyOrder';
                    LoadActionPan('ClinicalRadiologyOrderDetail', params);
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
        AppPrivileges.GetFormPrivileges("Orders and Results_Diagnostic Imaging Result", permissionState, "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                var params = [];
                if (RadiologyOrderId != null && parseInt(RadiologyOrderId) > 0) {
                    params["RadiologyResultId"] = RadiologyResultId;
                    params["mode"] = "Edit";
                }
                else {
                    params["RadiologyResultId"] = -1;
                    params["mode"] = "Add";

                    if (Clinical_RadiologyOrder.params["LastRadiologyResultLabName"]) {
                        params["LastRadiologyResultLabName"] = Clinical_RadiologyOrder.params["LastRadiologyResultLabName"];
                    } else {
                        params["LastRadiologyResultLabName"] = "";
                    }


                }
                params["RadiologyOrderId"] = RadiologyOrderId;
                params["FromAdmin"] = Clinical_RadiologyOrder.params["FromAdmin"];


                if (isFromDetail) {
                    if (Clinical_RadiologyOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
                        if (caller != null) {
                            params["ParentCtrlPanelID"] = Clinical_RadiologyOrder.params.PanelID;
                            params["ParentCtrl"] = "ClinicalRadiologyOrderDetail";
                            LoadActionPan('ClinicalRadiologyResultDetail', params);
                        }
                        else {
                            params["ParentCtrl"] = 'Clinical_RadiologyOrder';
                            params["ParentCtrlPanelID"] = Clinical_RadiologyOrder.params.PanelID;
                            LoadActionPan('ClinicalRadiologyResultDetail', params, Clinical_RadiologyOrder.params.PanelID);
                        }
                    }
                    else {
                        params["ParentCtrl"] = "ClinicalRadiologyOrderDetail";
                        LoadActionPan('ClinicalRadiologyResultDetail', params);
                    }
                }
                else {
                    if (Clinical_RadiologyOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
                        params["ParentCtrlPanelID"] = Clinical_RadiologyOrder.params.PanelID;
                        params["ParentCtrl"] = 'Clinical_RadiologyOrder';
                        LoadActionPan('ClinicalRadiologyResultDetail', params, Clinical_RadiologyOrder.params.PanelID);
                    }
                    else if (Clinical_RadiologyOrder.params["ParentCtrl"] == "clinicalTabFaceSheet" || Clinical_RadiologyOrder.params["ParentCtrl"] == "Clinical_FaceSheet") {
                        params["ParentCtrlPanelID"] = Clinical_RadiologyOrder.params.PanelID;
                        params["ParentCtrl"] = 'Clinical_RadiologyOrder';
                        LoadActionPan('ClinicalRadiologyResultDetail', params, Clinical_RadiologyOrder.params.PanelID);
                    }
                    else {
                        params["ParentCtrl"] = 'clinicalTabRadiologyOrder';
                        LoadActionPan('ClinicalRadiologyResultDetail', params);
                    }
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ValidateRadiology: function () {
        $("#" + Clinical_RadiologyOrder.params.PanelID + " #frmClinicalRadiologyOrder")
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
            Clinical_RadiologyOrder.RadiologySave();
        });
    },

    //Function Name: radiologyOrderSearch
    //Author Name: Humaira Yousaf
    //Created Date: 17-03-2016
    //Description: Searches Radiology Orders
    //Params: radiologyId, PageNo, rpp
    radiologyOrderSearch: function (radiologyId, PageNo, rpp, caller) {
        var dfd = $.Deferred();
        var strMessage = "";

        if (Clinical_RadiologyOrder.params.ParentCtrl == "clinicalTabProgressNote") {
            if ($("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.orderSearchGridId + " thead tr #selectRecordOrders").length == 0) {
                let params = "this,'dgvRadiologyOrder'";
                $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.orderSearchGridId + " thead tr").prepend('<th style="width:50px"><input type="checkbox" onchange="Clinical_RadiologyOrder.checkUncheckAllOrders(' + params + ');" controlname="selectRecordOrders" id="selectRecordOrders" name="chkHeader" class="input-block" coltype="checkbox"/></th>');
            }
        } else {
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.orderSearchGridId + "+ th#selectRecordOrders").remove();
        }

        if ($("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyOrder_Result").css("display") == "none") {
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyOrder_Result").show();
        }

        var self = $("#" + Clinical_RadiologyOrder.params.PanelID + " form");
        var myJSON = self.getMyJSONByName();

        // search specific on caller Id
        if (caller != null) {
            if (caller.indexOf("RadiologyOrderDetail") >= 0) {
                myJSON = null;
                // Reset Controls 
                Clinical_RadiologyOrder.radiologyOrderReset('pnlRadiologyOrder_Search', false);
                Clinical_RadiologyOrder.radiologyOrderReset('pnlExternalRadiologyOrder_Search', false);
            }
        }

        Clinical_RadiologyOrder.searchRadiologyOrder(myJSON, radiologyId, PageNo, rpp, "Internal").done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                $.when(Clinical_RadiologyOrder.radiologyGridLoad(response)).then(function () {
                    dfd.resolve();
                });
                var totalRows = $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.orderSearchGridId + " tr").length;
                totalRows -= 1;
                var selectedRows = $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.orderSearchGridId + " tbody tr input:checked").length;
                if (totalRows == selectedRows) {
                    $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.orderSearchGridId + " #selectRecordOrders").prop("checked", true);
                }
                else {
                    $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.orderSearchGridId + " #selectRecordOrders").prop("checked", false);
                }
                var TableControl = Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyOrder_Result #" + Clinical_RadiologyOrder.orderSearchGridId;
                var PagingPanelControlID = Clinical_RadiologyOrder.params.PanelID + " #dgvRadiologyOrder_Paging";
                var ClassControlName = "Clinical_RadiologyOrder";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(
                    CreatePagination(response.radiologyOrderCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        Clinical_RadiologyOrder.radiologyOrderSearch(PrimaryID, PageNumber, ResultPerPage);
                    }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
                dfd.resolve();
            }
        });

        return dfd;
    },

    externalRadiologyOrderSearch: function (radiologyId, PageNo, rpp, caller) {
        var dfd = $.Deferred();
        var strMessage = "";

        if (Clinical_RadiologyOrder.params.ParentCtrl == "clinicalTabProgressNote") {
            if ($("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.orderExternalSearchGridId + " thead tr #selectRecordOrders").length == 0) {
                let params = "this,'dgvExternalRadiologyOrder'";
                $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.orderExternalSearchGridId + " thead tr").prepend('<th style="width:50px"><input type="checkbox" onchange="Clinical_RadiologyOrder.checkUncheckAllOrders(' + params + ');" controlname="selectRecordOrders" id="selectRecordOrders" name="chkHeader" class="input-block" coltype="checkbox"/></th>');
            }
        } else {
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.orderExternalSearchGridId + "+ th#selectRecordOrders").remove();
        }

        if ($("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlExternalRadiologyOrder_Result").css("display") == "none") {
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlExternalRadiologyOrder_Result").show();
        }

        var self = $("#" + Clinical_RadiologyOrder.params.PanelID + " form");
        var myJSON = self.getMyJSONByName();

        // search specific on caller Id
        if (caller != null) {
            if (caller.indexOf("RadiologyOrderDetail") >= 0) {
                myJSON = null;
                // Reset Controls 
                Clinical_RadiologyOrder.radiologyOrderReset('pnlRadiologyOrder_Search', false);
                Clinical_RadiologyOrder.radiologyOrderReset('pnlExternalRadiologyOrder_Search', false);
            }
        }

        Clinical_RadiologyOrder.searchRadiologyOrder(myJSON, radiologyId, PageNo, rpp, "External").done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                $.when(Clinical_RadiologyOrder.radiologyExternalGridLoad(response)).then(function () {
                    dfd.resolve();
                });
                var totalRows = $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.orderExternalSearchGridId + " tr").length;
                totalRows -= 1;
                var selectedRows = $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.orderExternalSearchGridId + " tbody tr input:checked").length;
                if (totalRows == selectedRows) {
                    $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.orderExternalSearchGridId + " #selectRecordOrders").prop("checked", true);
                }
                else {
                    $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.orderExternalSearchGridId + " #selectRecordOrders").prop("checked", false);
                }
                var TableControl = Clinical_RadiologyOrder.params.PanelID + " #pnlExternalRadiologyOrder_Result #" + Clinical_RadiologyOrder.orderExternalSearchGridId;
                var PagingPanelControlID = Clinical_RadiologyOrder.params.PanelID + " #dgvExternalRadiologyOrder_Paging";
                var ClassControlName = "Clinical_RadiologyOrder";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(
                    CreatePagination(response.radiologyOrderCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        Clinical_RadiologyOrder.externalRadiologyOrderSearch(PrimaryID, PageNumber, ResultPerPage);
                    }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
                dfd.resolve();
            }
        });

        return dfd;
    },

    //radiologyOrderGridLoad: function (response) {
    //    Clinical_RadiologyOrder.radiologyGridLoad(response, Clinical_RadiologyOrder.orderSearchGridId, Clinical_RadiologyOrder.orderGridTableId, "Internal");
    //    Clinical_RadiologyOrder.radiologyGridLoad(response, Clinical_RadiologyOrder.orderExternalSearchGridId, Clinical_RadiologyOrder.orderExternalGridTableId, "External");
    //},
    //Function Name: radiologyGridLoad
    //Author Name: Humaira Yousaf
    //Created Date: 17-03-2016
    //Description: Loads Radiology Orders data in grid
    //Params: response
    radiologyGridLoad: function (response) {
        var dfd = $.Deferred();
        //Start 24-05-2016 Muhammad Arshad Remove Duplicate search issue on Datatable
        if ($.fn.dataTable.isDataTable("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyOrder_Result #" + Clinical_RadiologyOrder.orderSearchGridId)) {
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyOrder_Result #" + Clinical_RadiologyOrder.orderSearchGridId).dataTable().fnClearTable();
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyOrder_Result #" + Clinical_RadiologyOrder.orderSearchGridId).dataTable().fnDestroy();

        }

        $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyOrder_Result #" + Clinical_RadiologyOrder.orderSearchGridId + " tbody").find("tr").remove();
        ////Data table is not destroying fully, so we have to initialize it once
        //$("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyOrder_Result #" + Clinical_RadiologyOrder.orderSearchGridId).dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it

        ////Remove Previous rows from table
        //$("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyOrder_Result #" + Clinical_RadiologyOrder.orderSearchGridId + " tbody").find("tr").remove(); //Removing all the table data from table body

        var maxLabOrderId = 0;
        Clinical_RadiologyOrder.params["LastRadiologyLabId"] = "";

        if (response.radiologyOrderCount > 0) {
            var radiologyLoadJSONData = JSON.parse(response.RadiologyLoad_JSON); //Parsing array to JSON
            $.each(radiologyLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                //   $row.attr("onclick", "ClinicalRadiologyOrderDetail.radiologyEdit('" + item.RadiologyOrderId + "',event);");
                $row.attr("id", "gvRadiology_row" + item.RadiologyOrderId);
                $row.attr("RadiologyId", item.RadiologyOrderId);

                if (parseInt(item.RadiologyOrderId) > parseInt(maxLabOrderId)) {
                    maxLabOrderId = item.RadiologyOrderId;
                    Clinical_RadiologyOrder.params["LastRadiologyLabId"] = item.LabId;
                }

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

                if (Clinical_RadiologyOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                    SelectionCheckBoxColumn = '<td><input type="checkbox" onchange="Clinical_RadiologyOrder.enableAddOrder(this,event);" id="' + item.RadiologyOrderId + '" name="SelectCheckBoxOrder" ' + Checked + ' class="input-block"/></td>';
                } else {
                    SelectionCheckBoxColumn = "";
                }
                //End//22-04-2016//Ahmad Raza//logic to check uncheck order if it is attached with Note
                //Start//20-04-2016//Ahmad Raza//logic to show History icon
                if (item.Status == "Signed") {

                    //Begin 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578
                    $row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.RadiologyOrderId + '</td><td><a class="btn btn-xs disableAll" href="#" onclick="Clinical_RadiologyOrder.radiologyOrderDelete(\'' + item.RadiologyOrderId + '\', event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="ClinicalRadiologyOrderDetail.radiologyEdit(\'' + item.RadiologyOrderId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_RadiologyOrder.printRadiologyOrder(\'' + item.RadiologyOrderId + '\',\'' + item.Status + '\',event );" title="View Result"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_RadiologyOrder.showOrderHistory(\'' + item.RadiologyOrderId + '\', event );" title="Activity Log"> <i class="fa fa-history blue"></i></a></td><td>'
                        + utility.RemoveTimeFromDate(null, item.OrderDate) + ' ' + item.OrderTime + '</td><td>' + item.Test.replace("|", "<br/>") + '</td><td>' + item.OrderNo + '</td><td>' + item.Status + '</td><td>' + item.Provider + '</td><td>' + item.AssigneeName + '</td>');
                    //End 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578

                }
                else {

                    //Begin 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578
                    $row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.RadiologyOrderId + '</td><td><a class="btn btn-xs" href="#" onclick="Clinical_RadiologyOrder.radiologyOrderDelete(\'' + item.RadiologyOrderId + '\', event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="ClinicalRadiologyOrderDetail.radiologyEdit(\'' + item.RadiologyOrderId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs disableAll" href="#" onclick="Clinical_RadiologyOrder.printRadiologyOrder(\'' + item.RadiologyOrderId + '\',\'' + item.Status + '\',event );" title="View Result"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_RadiologyOrder.showOrderHistory(\'' + item.RadiologyOrderId + '\', event );" title="Activity Log"> <i class="fa fa-history blue"></i></a></td><td>'
                    + utility.RemoveTimeFromDate(null, item.OrderDate) + ' ' + item.OrderTime + '</td><td>' + item.Test.replace("|", "<br/>") + '</td><td>' + item.OrderNo + '</td><td>' + item.Status + '</td><td>' + item.Provider + '</td><td>' + item.AssigneeName + '</td>');
                    //End 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578
                }
                //End//20-04-2016//Ahmad Raza//logic to show History icon
                $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyOrder_Result #" + Clinical_RadiologyOrder.orderSearchGridId + " tbody").last().append($row);
            });
        }
        else {
            $("#" + Clinical_RadiologyOrder.params.PanelID + ' #pnlRadiologyOrder_Result #' + Clinical_RadiologyOrder.orderSearchGridId).DataTable({
                "language": {
                    "emptyTable": "No Diagnostic Imaging Order Found"
                }, "autoWidth": false, "bLengthChange": false, "aaSorting": [[1, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_RadiologyOrder.params.PanelID + ' #pnlRadiologyOrder_Result #' + Clinical_RadiologyOrder.orderSearchGridId))
            ;
        else {
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyOrder_Result #" + Clinical_RadiologyOrder.orderSearchGridId).DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [[1, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        }

        EMRUtility.fixDataTableDuplication("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyOrder_Result");
        dfd.resolve();
        return dfd;
    },

    radiologyExternalGridLoad: function (response) {
        var dfd = $.Deferred();
        //Start 24-05-2016 Muhammad Arshad Remove Duplicate search issue on Datatable
        if ($.fn.dataTable.isDataTable("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlExternalRadiologyOrder_Result #" + Clinical_RadiologyOrder.orderExternalSearchGridId)) {
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlExternalRadiologyOrder_Result #" + Clinical_RadiologyOrder.orderExternalSearchGridId).dataTable().fnClearTable();
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlExternalRadiologyOrder_Result #" + Clinical_RadiologyOrder.orderExternalSearchGridId).dataTable().fnDestroy();

        }
       
        else {
            //for stop make dublicate Datatables
            $.each($.fn.DataTable.fnTables(), function () {
                if (this.id == Clinical_RadiologyOrder.orderExternalSearchGridId) {
                    $(this).dataTable().fnClearTable();
                    $(this).dataTable().fnDestroy();
                    $(this).find("tbody tr").remove();
                    $("#" + Clinical_Immunization.params.PanelID + " #pnlExternalRadiologyOrder_Result " + "#"+Clinical_RadiologyOrder.orderExternalSearchGridId + " tbody").find("tr").remove();
                    $("#" + Clinical_Immunization.params.PanelID + " #pnlExternalRadiologyOrder_Result #" + Clinical_RadiologyOrder.orderExternalSearchGridId).parent().parent().find('div.row').remove();
                }
            })
        
        }

        $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlExternalRadiologyOrder_Result #" + Clinical_RadiologyOrder.orderExternalSearchGridId + " tbody").find("tr").remove();
        ////Data table is not destroying fully, so we have to initialize it once
        //$("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyOrder_Result #" + Clinical_RadiologyOrder.orderSearchGridId).dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it

        ////Remove Previous rows from table
        //$("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyOrder_Result #" + Clinical_RadiologyOrder.orderSearchGridId + " tbody").find("tr").remove(); //Removing all the table data from table body

        var maxLabOrderId = 0;
        Clinical_RadiologyOrder.params["LastRadiologyLabId"] = "";

        if (response.radiologyOrderCount > 0) {
            var radiologyLoadJSONData = JSON.parse(response.RadiologyLoad_JSON); //Parsing array to JSON
            $.each(radiologyLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                //   $row.attr("onclick", "ClinicalRadiologyOrderDetail.radiologyEdit('" + item.RadiologyOrderId + "',event);");
                $row.attr("id", "gvRadiology_row" + item.RadiologyOrderId);
                $row.attr("RadiologyId", item.RadiologyOrderId);

                if (parseInt(item.RadiologyOrderId) > parseInt(maxLabOrderId)) {
                    maxLabOrderId = item.RadiologyOrderId;
                    Clinical_RadiologyOrder.params["LastRadiologyLabId"] = item.LabId;
                }

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

                if (Clinical_RadiologyOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                    SelectionCheckBoxColumn = '<td><input type="checkbox" onchange="Clinical_RadiologyOrder.enableAddOrder(this,event);" id="' + item.RadiologyOrderId + '" name="SelectCheckBoxOrder" ' + Checked + ' class="input-block"/></td>';
                } else {
                    SelectionCheckBoxColumn = "";
                }
                //End//22-04-2016//Ahmad Raza//logic to check uncheck order if it is attached with Note
                //Start//20-04-2016//Ahmad Raza//logic to show History icon
                if (item.Status == "Signed") {

                    //Begin 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578
                    $row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.RadiologyOrderId + '</td><td><a class="btn btn-xs disableAll" href="#" onclick="Clinical_RadiologyOrder.radiologyOrderDelete(\'' + item.RadiologyOrderId + '\', event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="ClinicalRadiologyOrderDetail.radiologyEdit(\'' + item.RadiologyOrderId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_RadiologyOrder.printRadiologyOrder(\'' + item.RadiologyOrderId + '\',\'' + item.Status + '\',event );" title="View Result"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_RadiologyOrder.showOrderHistory(\'' + item.RadiologyOrderId + '\', event );" title="Activity Log"> <i class="fa fa-history blue"></i></a></td><td>'
                        + utility.RemoveTimeFromDate(null, item.OrderDate) + ' ' + item.OrderTime + '</td><td>' + item.Test.replace("|", "<br/>") + '</td><td>' + item.OrderNo + '</td><td>' + item.Status + '</td><td>' + item.Provider + '</td><td>' + item.AssigneeName + '</td>');
                    //End 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578

                }
                else {

                    //Begin 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578
                    $row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.RadiologyOrderId + '</td><td><a class="btn btn-xs" href="#" onclick="Clinical_RadiologyOrder.radiologyOrderDelete(\'' + item.RadiologyOrderId + '\', event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="ClinicalRadiologyOrderDetail.radiologyEdit(\'' + item.RadiologyOrderId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs disableAll" href="#" onclick="Clinical_RadiologyOrder.printRadiologyOrder(\'' + item.RadiologyOrderId + '\',\'' + item.Status + '\',event );" title="View Result"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_RadiologyOrder.showOrderHistory(\'' + item.RadiologyOrderId + '\', event );" title="Activity Log"> <i class="fa fa-history blue"></i></a></td><td>'
                    + utility.RemoveTimeFromDate(null, item.OrderDate) + ' ' + item.OrderTime + '</td><td>' + item.Test.replace("|", "<br/>") + '</td><td>' + item.OrderNo + '</td><td>' + item.Status + '</td><td>' + item.Provider + '</td><td>' + item.AssigneeName + '</td>');
                    //End 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578
                }
                //End//20-04-2016//Ahmad Raza//logic to show History icon
                $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlExternalRadiologyOrder_Result #" + Clinical_RadiologyOrder.orderExternalSearchGridId + " tbody").last().append($row);
            });
        }
        else {
            $("#" + Clinical_RadiologyOrder.params.PanelID + ' #pnlExternalRadiologyOrder_Result #' + Clinical_RadiologyOrder.orderExternalSearchGridId).DataTable({
                "language": {
                    "emptyTable": "No Diagnostic Imaging Order Found"
                }, "autoWidth": false, "bLengthChange": false, "aaSorting": [[1, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_RadiologyOrder.params.PanelID + ' #pnlExternalRadiologyOrder_Result #' + Clinical_RadiologyOrder.orderExternalSearchGridId))
            ;
        else {
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlExternalRadiologyOrder_Result #" + Clinical_RadiologyOrder.orderExternalSearchGridId).DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [[1, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        }

        EMRUtility.fixDataTableDuplication("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlExternalRadiologyOrder_Result");
        dfd.resolve();
        return dfd;
    },
    //Function Name: searchRadiologyOrder
    //Author Name: Humaira Yousaf
    //Created Date: 17-03-2016
    //Description: Searches Radiology Orders
    //Params: radiologyOrderData, radiologyOrderId, PageNumber, RowsPerPage
    searchRadiologyOrder: function (radiologyOrderData, radiologyOrderId, PageNumber, RowsPerPage, type) {
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
        objData["PatientId"] = Clinical_RadiologyOrder.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : Clinical_RadiologyOrder.patientId;
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["Test"] = objData["CPTCode"];

        var divId = " #pnlRadiologyOrder_Search"
        if (type == "External") {
            divId = " #pnlExternalRadiologyOrder_Search"
        }
        objData["ProviderId"] = $('#' + Clinical_RadiologyOrder.params.PanelID + divId + ' #frmClinicalRadiologyOrder #txtProvider').val();

        /* Start 27/05/2015 Abid Ali / labId for radiology order */
        //objData["LabId"] = $('#' + Clinical_RadiologyOrder.params.PanelID + divId +' #frmClinicalRadiologyOrder #ddlLaboratory').val();
        /* End 27/05/2015 Abid Ali / labId for radiology order */

        objData["OrderFromDate"] = $('#' + Clinical_RadiologyOrder.params.PanelID + divId + ' #frmClinicalRadiologyOrder #dpStartDate').val();
        objData["Status"] = $('#' + Clinical_RadiologyOrder.params.PanelID + divId + ' #frmClinicalRadiologyOrder #ddlStatus option:selected').val();
        objData["OrderToDate"] = $('#' + Clinical_RadiologyOrder.params.PanelID + divId + ' #frmClinicalRadiologyOrder #dpToDate').val();
        objData["OrderNo"] = $('#' + Clinical_RadiologyOrder.params.PanelID + divId + ' #frmClinicalRadiologyOrder #txtOrderNumber').val();
        objData["RadiologyType"] = type;

        objData["commandType"] = "search_radiologyorders";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyOrder", "RadiologyOrder");
    },

    //Function Name: radiologyOrderReset
    //Author Name: Farooq Ahmad
    //Created Date: 29-03-2016
    //Description: radiology Order Reset
    radiologyOrderReset: function (divId, IsToShoAlert) {

        if (IsToShoAlert == false) {
            $('#' + Clinical_RadiologyOrder.params.PanelID + ' #' + divId + ' #frmClinicalRadiologyOrder').resetAllControls(null);
            Clinical_RadiologyOrder.ValidateRadiology();
        }
        else {
            utility.myConfirm('22', function () {
                $('#' + Clinical_RadiologyOrder.params.PanelID + ' #' + divId + ' #frmClinicalRadiologyOrder').resetAllControls(null);
                Clinical_RadiologyOrder.ValidateRadiology();
            }, function () { },
            '22'
            );
        }
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
    radiologyResultReset: function (IsToShoAlert) {

        if (IsToShoAlert == false) {
            $('#' + Clinical_RadiologyOrder.params.PanelID + ' #frmClinicalRadiologyResult').resetAllControls(null);
            Clinical_RadiologyOrder.ValidateRadiology();
        }
        else {
            utility.myConfirm('22', function () {
                $('#' + Clinical_RadiologyOrder.params.PanelID + ' #frmClinicalRadiologyResult').resetAllControls(null);
                Clinical_RadiologyOrder.ValidateRadiology();
            }, function () { },
                        '22'
                    );
        }

    },

    UnLoadTab: function () {
        var parentPanelId = null;
        var objDeffered = $.Deferred();
        if (Clinical_RadiologyOrder.params["CurrentNotesProviderId"])
            Clinical_RadiologyOrder.params["CurrentNotesProviderId"] = "";
        /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        if (Clinical_RadiologyOrder.params.ParentCtrl == "clinicalTabFaceSheet" || Clinical_RadiologyOrder.params.ParentCtrl == "Clinical_FaceSheet") {
            utility.UnLoadDialog(Clinical_RadiologyOrder.params.PanelID + ' #frmClinicalCDS', function () {

                if (Clinical_RadiologyOrder.params["FromAdmin"] == "0") {
                    if (Clinical_RadiologyOrder.params != null && Clinical_RadiologyOrder.params.ParentCtrl != null) {
                        if (Clinical_RadiologyOrder.params.ParentCtrl == "Clinical_FaceSheet") {
                            parentPanelId = GetTab(Clinical_FaceSheet.params["ParentCtrl"]).PanelID;
                            Clinical_FaceSheet.params.ChildPanelID = null;
                            UnloadActionPan(Clinical_RadiologyOrder.params.ParentCtrl, 'Clinical_RadiologyOrder', null, parentPanelId);
                        } else {
                            UnloadActionPan(Clinical_RadiologyOrder.params.ParentCtrl, 'Clinical_RadiologyOrder');
                        }
                    }
                    else
                        UnloadActionPan(null, 'Clinical_RadiologyOrder');
                }
                else {
                    $("#mstrDivMedical #clinicalMenu_Medical_CDS").remove();
                    RemoveAdminTab();
                }
                objDeffered.resolve();
            }, function () {
                if (Clinical_RadiologyOrder.params["FromAdmin"] == "0") {
                    if (Clinical_RadiologyOrder.params != null && Clinical_RadiologyOrder.params.ParentCtrl != null) {
                        UnloadActionPan(Clinical_RadiologyOrder.params.ParentCtrl, 'Clinical_RadiologyOrder');
                    }
                    else
                        UnloadActionPan(null, 'Clinical_RadiologyOrder');
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
            if (Clinical_RadiologyOrder.params["FromAdmin"] == "0") {
                if (Clinical_RadiologyOrder.params != null && Clinical_RadiologyOrder.params.ParentCtrl != null) {
                    if ($("#" + Clinical_RadiologyOrder.params.PanelID + " #listRadiologyOrder").hasClass('active')) {
                        Clinical_RadiologyOrder.addOrdersToNotes();
                    }
                    else if ($("#" + Clinical_RadiologyOrder.params.PanelID + " #listRadiologyResult").hasClass('active')) {
                        Clinical_RadiologyOrder.addResultsToNotes();
                    }
                    UnloadActionPan(Clinical_RadiologyOrder.params.ParentCtrl, 'Clinical_RadiologyOrder');
                }
                else {
                    if ($("#" + Clinical_RadiologyOrder.params.PanelID + " #listRadiologyOrder").hasClass('active')) {
                        Clinical_RadiologyOrder.addOrdersToNotes();
                    }
                    else if ($("#" + Clinical_RadiologyOrder.params.PanelID + " #listRadiologyResult").hasClass('active')) {
                        Clinical_RadiologyOrder.addResultsToNotes();
                    }
                    UnloadActionPan(null, 'Clinical_RadiologyOrder');
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
        params["RefForm"] = "frmClinicalRadiologyOrder";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Clinical_RadiologyOrder";
        LoadActionPan('Admin_Provider', params);
    },

    //Function Name: openproviderdetail
    //Author Name: Humaira Yousaf
    //Created Date: 21-03-2016
    //Description: Opens Provider
    openproviderdetail: function () {
        var params = [];
        params["providerid"] = $('#pnlClinicalRadiologyOrder #hfprovider').val();
        params["mode"] = "edit";
        params["fromadmin"] = "0";
        params["refctrl"] = "txtprovider";
        params["parentctrl"] = 'Clinical_RadiologyOrder';
        loadactionpan('providerdetail', params);
    },

    //Function Name: radiologyOrderDelete
    //Author Name: Ahmad Raza
    //Created Date: 22-03-2016
    //Description: Delete the selected Radiology Order
    radiologyOrderDelete: function (radiologyId, event, PageNo, rpp) {
        var strMessage = "";
        event.stopPropagation();

        utility.myConfirm('1', function () {
            var selectedValue = radiologyId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                var self = $("#" + Clinical_RadiologyOrder.params.PanelID + " form");
                var myJSON = self.getMyJSONByName();

                Clinical_RadiologyOrder.deleteRadiologyOrder(myJSON, radiologyId, PageNo, rpp).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        Clinical_RadiologyOrder.radiologyOrderSearch("", 1, 15);
                        Clinical_RadiologyOrder.radiologyResultsSearch("", 1, 15);
                        //AST-328 by:MAHMAD
                        Clinical_RadiologyOrder.externalRadiologyOrderSearch("", 1, 15);
                        //AST-328 by:MAHMAD
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
        objData["PatientId"] = Clinical_RadiologyOrder.patientId;
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
            //var PanelID = "";
            //var params = [];
            //params["FromAdmin"] = "0";
            //params["UserId"] = globalAppdata['AppUserId'];
            //params["PatientId"] = $('#PatientProfile #hfPatientId').val();
            //// params["ParentCtrl"] = Clinical_RadiologyOrder.params.TabID;
            //params["ParentCtrl"] = (Clinical_RadiologyOrder.params.ParentCtrl != "clinicalTabProgressNote" && Clinical_RadiologyOrder.params.ParentCtrl != "clinicalTabFaceSheet") ? Clinical_RadiologyOrder.params.TabID : "Clinical_RadiologyOrder";
            //params["RadiologyOrderId"] = radiologyId;
            //if (Clinical_RadiologyOrder.params.ParentCtrl == "clinicalTabProgressNote") {
            //    PanelID = 'pnlClinicalProgressNote #pnlClinicalRadiologyOrder';
            //    params["PanelID"] = 'pnlClinicalProgressNote #pnlClinicalRadiologyOrder';
            //    params["PrPanelID"] = 'pnlClinicalProgressNote #pnlClinicalRadiologyOrder';
            //}
            //else if (Clinical_LabOrder.params.ParentCtrl == "clinicalTabFaceSheet") {
            //    PanelID = 'pnlClinicalFaceSheet #pnlClinicalRadiologyOrder';
            //    params["PanelID"] = 'pnlClinicalFaceSheet #pnlClinicalRadiologyOrder';
            //    params["PrPanelID"] = 'pnlClinicalFaceSheet #pnlClinicalRadiologyOrder';
            //}
            //else {
            //    PanelID = 'pnlClinicalRadiologyOrder';
            //    params["PanelID"] = 'pnlClinicalRadiologyOrder';
            //    params["PrPanelID"] = 'pnlClinicalRadiologyOrder';
            //}
            //LoadActionPan('Clinical_RadiologyOrderView', params, PanelID);
            ClinicalRadiologyOrderDetail.radiologyOrderPreview($('#PatientProfile #hfPatientId').val(), globalAppdata['AppUserId'], radiologyId);
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
        if (Clinical_RadiologyOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
            ParentCtrl = 'Clinical_RadiologyOrder';
            ParentCtrlPanelID = Clinical_RadiologyOrder.params.PanelID;
        }
        else if (Clinical_RadiologyOrder.params["ParentCtrl"] == "clinicalTabFaceSheet" || Clinical_RadiologyOrder.params["ParentCtrl"] == "Clinical_FaceSheet") {
            ParentCtrl = 'Clinical_RadiologyOrder';
            ParentCtrlPanelID = Clinical_RadiologyOrder.params.PanelID;
        }
        EMRUtility.showCurrentItemHistory(Clinical_RadiologyOrder.params.PanelID, null, null, "RadiologyOrder,RadiologyOrderTest, RadiologyOrderProblem", null, ParentCtrl, radiologyOrderId, ParentCtrlPanelID);
    },



    //Author: Ahmad Raza
    //Date :  22-04-2016
    //Description: checkUncheckAllOrders
    checkUncheckAllOrders: function (obj,tableId) {
        if ($(obj).is(':checked')) {
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + tableId + " tbody input[name='SelectCheckBoxOrder']:checkbox").prop('checked', true);
        } else {
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + tableId + " tbody input[name='SelectCheckBoxOrder']:checkbox").prop('checked', false);
        }
        $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + tableId + " tbody").find('input[type="checkbox"]').each(function () {
            Clinical_RadiologyOrder.enableAddOrder(this);
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

        var totalRows = $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.orderSearchGridId + " tr").length;
        totalRows -= 1;
        var selectedRows = $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.orderSearchGridId + " tbody tr input:checked").length;

        if (totalRows == selectedRows) {
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.orderSearchGridId + " #selectRecordOrders").prop("checked", true);
        }
        else {
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.orderSearchGridId + " #selectRecordOrders").prop("checked", false);
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
        Clinical_RadiologyOrder.getOrdersForSOAP_DBCall(radiologyOrderId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.radiologySoapCount > 0) {
                        Clinical_RadiologyOrder.createRadiologyBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', radiologyOrderId);
                    }
                    if (response.ProblemListSoapCount > 0) {
                        Clinical_ProblemLists.createProblemListBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, true);
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
        objData["PatientId"] = Clinical_RadiologyOrder.params.patientID;
        objData["ProviderId"] = $("#pnlClinicalProgressNote #hfProviderId").val();
        objData["commandType"] = "get_Orders_forsoap";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyOrder", "RadiologyOrder");
    },

    createRadiologyBodyHTML: function (response, noteHTMLCtrl, radiologyOrderId, hideAlert) {
        Clinical_RadiologyOrder.checkRadiologyOrderExists();
        if (response.radiologySoap_JSON != null && response.radiologySoap_JSON != '') {
            var radiologyOrderSOAPJSON = response.radiologySoap_JSON != null ? JSON.parse(response.radiologySoap_JSON) : null;
            var radiologyOrderTestSOAPJSON = response.radiologyOrderTest_JSON != null ? JSON.parse(response.radiologyOrderTest_JSON) : null;
            var radiologyOrderProblemSOAPJSON = response.radiologyOrderProblem_JSON != null ? JSON.parse(response.radiologyOrderProblem_JSON) : null;


            if (response.radiologySoap_JSON != null) {
                //          medicationReviewSoapJSON = JSON.parse(response.radiologySoap_JSON);
            }

            var $mainDivRadiology = $(document.createElement('div'));
            if (radiologyOrderSOAPJSON == null || radiologyOrderSOAPJSON.length == 0) {
                return;
            } else {
                if ($(noteHTMLCtrl + ' clinical_radiologyorder').parent().parent().find('#Cli_RadiologyOrderDetail_Main0').length != 0) {
                    $(noteHTMLCtrl + ' #Cli_RadiologyOrderDetail_Main0').parent().remove();
                }
            }

            var AListId = [];
            $.each(radiologyOrderSOAPJSON, function (index, element) {
                var RadiologyOrderTestSoapText = "", RadiologyOrderAssociatedProblemSoapText = "", RadiologyOrderDate = "";
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
                if (element.SoapText != "") {
                    $(obj).html(element.SoapText);
                    //$(obj).html($(obj).text())
                    $ListRadiology.append("<li>" + $(obj).text() + "</li>");
                }
                else {
                    if (radiologyOrderTestSOAPJSON) {
                        $.each(radiologyOrderTestSOAPJSON, function (i, item) {
                            if (element.RadiologyOrderId == item.RadiologyOrderId) {
                                RadiologyOrderTestSoapText += (item.ShowCPTCode == 1 ? "<b>" + item.CPTCode + " " + item.CPTCodeDescription + "</b>" : "<b>" + item.CPTCodeDescription + "</b>")
                                    + (item.Specimen != "" ? "<b> Specimen:</b> " + item.Specimen : "")
                                    + (item.Volume != "" ? "<b> Volume:</b> " + item.Volume : "")
                                    + (item.UrgencyName != "" ? "<b> Urgency:</b> " + item.UrgencyName : "")
                                    + "<br/>";
                            }
                        });
                    }
                    if (radiologyOrderProblemSOAPJSON) {
                        $.each(radiologyOrderProblemSOAPJSON, function (ind, value) {
                            if (element.RadiologyOrderId == value.RadiologyOrderId) {
                                RadiologyOrderAssociatedProblemSoapText += (value.SoapText != "" ? "<b>" + value.SoapText + "</b>" : "") + "</br>";
                            }
                        });
                    }

                    if (element.CreatedOn == element.ModifiedOn) {
                        RadiologyOrderDate = " Added On " + utility.RemoveTimeFromDate(null, element.CreatedOn);
                    }
                    else {
                        RadiologyOrderDate = " Last Updated On " + utility.RemoveTimeFromDate(null, element.ModifiedOn);
                    }
                    $ListRadiology.append("<li>" + RadiologyOrderTestSoapText + (RadiologyOrderAssociatedProblemSoapText != "" ? " Associated Problem(s)  <br/>" + RadiologyOrderAssociatedProblemSoapText : "") + (element.Comments != "" ? element.Comments : "") + RadiologyOrderDate + "</li>");

                }


                //$ListRadiology.append("<li>" + $(obj).text() + "</li>");

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
                if ($(noteHTMLCtrl + ' clinical_radiologyorder').parent().parent().find('#Cli_RadiologyOrderDetail_Main' + ALid).length == 0) {
                    AListId.push(ALid);
                    $mainDivRadiology.append($SectionBodyRadiology);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(noteHTMLCtrl + ' clinical_radiologyorder').parent().parent().find('#Cli_RadiologyOrderDetail_Main' + ALid + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(noteHTMLCtrl + ' clinical_radiologyorder').parent().parent().find('#Cli_RadiologyOrderDetail_Main' + ALid + ' ul li:Last').get(0).outerHTML;
                    }
                    $(noteHTMLCtrl + ' clinical_radiologyorder').parent().parent().find('#Cli_RadiologyOrderDetail_Main' + ALid).html($SectionBodyRadiology.html());
                    $(noteHTMLCtrl + ' clinical_radiologyorder').parent().parent().find('#Cli_RadiologyOrderDetail_Main' + ALid + ' ul').append(CommentHTML);;
                }


            });

            if (AListId.join(",") != "") {
                radiologyOrderId = AListId.join(",");
            }
            if ($mainDivRadiology.html() != '') {
                Clinical_RadiologyOrder.updateRadiologyHtml($mainDivRadiology.html(), radiologyOrderId, noteHTMLCtrl, hideAlert);
                Clinical_ProgressNote.saveComponentSOAPText("Diagnostic Imaging Order", true);
            } else {
                Clinical_RadiologyOrder.updateRadiologyHtml('', radiologyOrderId, noteHTMLCtrl, hideAlert);
                Clinical_ProgressNote.saveComponentSOAPText("Diagnostic Imaging Order", true);
            }
        }

    },

    updateRadiologyHtml: function (radiologyHTML, radiologyId, noteHTMLCtrl, hideAlert) {
        $(noteHTMLCtrl + ' clinical_radiologyorder').parent().parent().addClass('initialVisitBody');
        if (radiologyHTML != '') {
            $(noteHTMLCtrl + ' clinical_radiologyorder').parent().parent().append(radiologyHTML);
        }

        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (radiologyHTML != '' && radiologyId != null && radiologyId != '' && radiologyId != '0') {
            Clinical_RadiologyOrder.attachRadiologyOrderFromNotes(radiologyId, hideAlert);
        }
        else {
            //var docId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_radiologyorder').parent().parent().find("section[id='Cli_RadiologyOrderDetail_Main" + radiologyId + "']").attr("patdocid");

            var docIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_radiologyorder').parent().parent().find('section[id*="Cli_RadiologyOrderDetail_Main"]').map(function () {
                return $(this).attr('patdocid');
            }).get().join(',');

            if (docIds.length > 0) {
                Clinical_ProgressNote.detachImagesComponentFromNotes_DBCall(docIds).done(function (responseDoc) {
                    responseDoc = JSON.parse(responseDoc);
                    if (responseDoc.status != false) {
                        Patient_Document.DeleteDocument(docIds).done(function (response) {
                            Clinical_ProgressNote.SaveAndAttachOrderReport("Radiology Order", radiologyId, false, hideAlert).done(function () {
                                Clinical_ProgressNote.saveComponentSOAPText("Diagnostic Imaging Order", true);
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

    attachRadiologyOrderFromNotes: function (radiologyId, hideAlert) {
        var selectedValue = radiologyId;
        if (selectedValue == "" || selectedValue == "undefined") {
        }
        else {
            Clinical_RadiologyOrder.attachRadiologyOrderWithNotes_DBCall(selectedValue).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {

                    Clinical_ProgressNote.SaveAndAttachOrderReport("Radiology Order", radiologyId, false, hideAlert).done(function () {
                        Clinical_ProgressNote.saveComponentSOAPText("Diagnostic Imaging Order", true);
                    });

                    //Clinical_ProgressNote.saveComponentSOAPText("Radiology Order", true).done(function () {
                    //    Clinical_ProgressNote.SaveAndAttachOrderReport("Radiology Order", radiologyId);
                    //});
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
        //if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_radiologyorder').length == 0) {
        //    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList').append(' <li> <header>' +
        //                '<clinical_radiologyorder title="Radiology Orders"  id="' + this.id + '" class="NotesComponent">' +
        //                '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'RadiologyOrder\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Radiology Order">Radiology Order</a> ' +
        //                                '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'RadiologyOrder\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
        //                           '</clinical_radiologyorder> </header></li>');
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

        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_radiologyorder').length == 0) {

            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #PlanNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="RadiologyOrderComponent DiagnosticImagingOrderComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_radiologyorder title="Radiology Order"  id="clinicalMenu_Orders_Radiology" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'RadiologyOrder\',\'clinicalMenu_Orders_Radiology\',' + Clinical_ProgressNote.params.NotesId + ');" title="Diagnostic Imaging Order">Diagnostic Imaging Order</a> ' +
                       '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this, \'RadiologyOrder' + '\', event);" class=" btn btn-link btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'Radiology Order\',\'clinicalMenu_Orders_Radiology\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_radiologyorder> </header></li>');
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
                $(this).find('.closeBtn').removeClass('hidden');
                $(this).css('background', '#EAF1F8');
            });

            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
                $(this).find('.closeBtn').addClass('hidden');
                $(this).css('background', '#fff');
            });
        }
        else
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_radiologyorder').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($('#' + Clinical_ProgressNote.params["PanelID"] + ' #hfNotesId').val());
    },

    addOrdersToNotes: function (selectedAttachedOrders, selectedDetachedOrders) {
        var noteHTMLCtrl = '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML';
        var ordergrid = " #dgvRadiologyOrder";
        var type = $("#" + Clinical_RadiologyOrder.params.PanelID + " #ulRadilogyOrderTypeTabsItems > li.active").text();
        if (type == "External") {
            ordergrid = " #dgvExternalRadiologyOrder"
        }
        if ($("#" + Clinical_RadiologyOrder.params.PanelID + ordergrid).dataTable().fnSettings().aoData.length == 0) {
            //Clinical_RadiologyOrder.noActiveRadiologyOrderSoapText();
        }
        else {

            if ($("#" + Clinical_RadiologyOrder.params.PanelID + ordergrid + " #selectRecordOrders").prop('checked') == true || $("#" + Clinical_RadiologyOrder.params.PanelID + ordergrid + " #selectRecordOrders").prop('checked') == false) {

                $("#" + Clinical_RadiologyOrder.params.PanelID + ordergrid + " tbody").find('input[type="checkbox"]').each(function () {
                    Clinical_RadiologyOrder.enableAddOrder(this);
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
                Clinical_RadiologyOrder.detachRadiologyOrderFromNotesAT(detachedvalues).done(function () {
                    if (AttachedSelectedOrders.join() != null && AttachedSelectedOrders.join() != '') {
                        Clinical_RadiologyOrder.attachRadiologyOrderFromNotesAT(AttachedSelectedOrders);
                    } else {
                        Clinical_ProgressNote.saveComponentSOAPText("Diagnostic Imaging Order");
                    }
                });

            } else if (AttachedSelectedOrders.join() != null && AttachedSelectedOrders.join() != '') {
                Clinical_RadiologyOrder.attachRadiologyOrderFromNotesAT(AttachedSelectedOrders);
            }

            //$("#" + Clinical_Medications.params.PanelID + " #btnAddMedications").prop('disabled', true);
            if (Clinical_RadiologyOrder.params && Clinical_RadiologyOrder.params.ParentCtrl && Clinical_RadiologyOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                UnloadActionPan(Clinical_RadiologyOrder.params.ParentCtrl, 'Clinical_RadiologyOrder');
                if ($("#" + Clinical_RadiologyOrder.params.PanelID + " #listRadiologyOrder").hasClass('active')) {
                    EMRUtility.scrollToPNcomponent('clinical_radiologyorders');
                    EMRUtility.scrollToPNcomponent('clinical_radiologyorder');
                }
                else if ($("#" + Clinical_RadiologyOrder.params.PanelID + " #listRadiologyResult").hasClass('active')) {
                    EMRUtility.scrollToPNcomponent('clinical_diagnosticimagingresults');
                    EMRUtility.scrollToPNcomponent('clinical_radiologyresults');
                }
            }
        }
    },

    attachRadiologyOrderFromNotesAT: function (AttachedSelectedOrders) {
        Clinical_RadiologyOrder.getRadiologyOrderInfo(AttachedSelectedOrders.join()).done(function () {
            setTimeout(function () {
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                if (Clinical_RadiologyOrder.params != null && Clinical_RadiologyOrder.params.PanelID.indexOf('pnlClinicalRadiologyOrder') != -1) {
                    Clinical_RadiologyOrder.radiologyOrderSearch();
                }
            }, 5);
        });
    },

    detachRadiologyOrderFromNotesAT: function (detachedvalues) {
        var dfd = new $.Deferred();
        Clinical_RadiologyOrder.detachRadiologyOrderFromNotes_DBCall(detachedvalues.join()).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                var docIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_radiologyorder').parent().parent().find('section[id*="Cli_RadiologyOrderDetail_Main"]').map(function () {
                    return $(this).attr('patdocid');
                }).get().join(',');

                if (docIds.length > 0) {
                    Clinical_ProgressNote.detachImagesComponentFromNotes_DBCall(docIds).done(function (responseDoc) {
                        responseDoc = JSON.parse(responseDoc);
                        if (responseDoc.status != false) {
                            Patient_Document.DeleteDocument(docIds);
                            Clinical_ProgressNote.updateAttachDocumentButtonImg();
                        }
                        else {
                            utility.DisplayMessages(responseDoc.Message, 3);
                        }
                    });
                }

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
        if ($(noteHTMLCtrl + ' clinical_radiologyorder').parent().parent().find('#Cli_RadiologyOrderDetail_Main0').length == 0) {
            Clinical_RadiologyOrder.checkRadiologyOrderExists();
            var htmlForNoRadiology = '<section id="Cli_RadiologyOrderDetail_Main0"> <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="Cli_RadiologyOrderDetail_0"><i class="fa fa-edit"></i></a>' +
    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="Cli_RadiologyOrderDetail_Main0"  ><i class="fa fa-times"></i></a></div> ' +
    '<div id="Cli_RadiologyOrderDetail_0"><ul class="list-unstyled"><li> No Diagnostic Imaging Order Found</li></ul></div></section>';
            Clinical_RadiologyOrder.updateRadiologyHtml(htmlForNoRadiology, '0', noteHTMLCtrl);
            Clinical_ProgressNote.saveComponentSOAPText("Diagnostic Imaging Order");
        }
    },


    radiologyResultsSearch: function (radId, PageNo, rpp, caller, RadiologyOrderIdShouldBeNull) {
        var dfd = $.Deferred();
        var strMessage = "";

        
        
        if (Clinical_RadiologyOrder.params.ParentCtrl == "clinicalTabProgressNote") {
            if ($("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.resultSearchGridId + " thead tr #selectRecordOrders").length == 0) {
                let params = "this,'dgvRadiologyResult'";
                $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.resultSearchGridId + " thead tr").prepend('<th style="width:50px"><input type="checkbox" onchange="Clinical_RadiologyOrder.checkUncheckAllOrders(' + params + ');" controlname="selectRecordOrders" id="selectRecordOrders" name="chkHeader" class="input-block" coltype="checkbox"/></th>');
            }
        } else {
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.resultSearchGridId + " th#selectRecordOrders").remove();
        }



        if ($("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyResult_Result").css("display") == "none") {
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyResult_Result").show();
        }

        var self = $("#" + Clinical_RadiologyOrder.params.PanelID + " form");
        var myJSON = self.getMyJSONByName();

        // search specific on caller Id
        if (caller != null) {
            if (caller.indexOf("RadiologyResultDetail") >= 0) {
                myJSON = null;
                // Reset Controlls
                Clinical_RadiologyOrder.radiologyResultReset(false);
            }
        }

        Clinical_RadiologyOrder.searchRadiologyResults(myJSON, radId, PageNo, rpp, RadiologyOrderIdShouldBeNull, "Internal").done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                $.when(Clinical_RadiologyOrder.radiologyResultsGridLoad(response)).then(function () {
                    dfd.resolve();
                });

                var totalRows = $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.resultSearchGridId + " tr").length;
                totalRows -= 1;
                var selectedRows = $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.resultSearchGridId + " tbody tr input:checked").length;
                if (totalRows == selectedRows) {
                    $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.resultSearchGridId + " #selectRecordOrders").prop("checked", true);
                }
                else {
                    $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.resultSearchGridId + " #selectRecordOrders").prop("checked", false);
                }

                var TableControl = Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyResult_Result #" + Clinical_RadiologyOrder.resultSearchGridId;
                var PagingPanelControlID = Clinical_RadiologyOrder.params.PanelID + " #dgvRadiologyResult_Paging";
                var ClassControlName = "Clinical_RadiologyOrder";
                var PagesToDisplay = 15;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(
                    CreatePagination(response.RadiologyResultCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        Clinical_RadiologyOrder.radiologyResultsSearch(PrimaryID, PageNumber, ResultPerPage);
                    }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
                dfd.resolve();
            }
        });
        return dfd;
    },

    externalRadiologyResultsSearch: function (radId, PageNo, rpp, caller, RadiologyOrderIdShouldBeNull) {
        var dfd = $.Deferred();
        var strMessage = "";

        if (Clinical_RadiologyOrder.params.ParentCtrl == "clinicalTabProgressNote") {
            if ($("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.externalResultSearchGridId + " thead tr #selectRecordOrders").length == 0) {
                let params = "this,'dgvExternalRadiologyResult'";
                $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.externalResultSearchGridId + " thead tr").prepend('<th style="width:50px"><input type="checkbox" onchange="Clinical_RadiologyOrder.checkUncheckAllOrders(' + params + ');" controlname="selectRecordOrders" id="selectRecordOrders" name="chkHeader" class="input-block" coltype="checkbox"/></th>');
            }
        } else {
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.externalResultSearchGridId + " th#selectRecordOrders").remove();
        }



        if ($("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlExternalRadiologyResult_Result").css("display") == "none") {
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlExternalRadiologyResult_Result").show();
        }

        var self = $("#" + Clinical_RadiologyOrder.params.PanelID + " form");
        var myJSON = self.getMyJSONByName();

        // search specific on caller Id
        if (caller != null) {
            if (caller.indexOf("RadiologyResultDetail") >= 0) {
                myJSON = null;
                // Reset Controlls
                Clinical_RadiologyOrder.radiologyResultReset(false);
            }
        }

        Clinical_RadiologyOrder.searchRadiologyResults(myJSON, radId, PageNo, rpp, RadiologyOrderIdShouldBeNull, "External").done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                $.when(Clinical_RadiologyOrder.externalRadiologyResultsGridLoad(response)).then(function () {
                    dfd.resolve();
                });

                var totalRows = $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.externalResultSearchGridId + " tr").length;
                totalRows -= 1;
                var selectedRows = $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.externalResultSearchGridId + " tbody tr input:checked").length;
                if (totalRows == selectedRows) {
                    $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.externalResultSearchGridId + " #selectRecordOrders").prop("checked", true);
                }
                else {
                    $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.externalResultSearchGridId + " #selectRecordOrders").prop("checked", false);
                }

                var TableControl = Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyResult_Result #" + Clinical_RadiologyOrder.externalResultSearchGridId;
                var PagingPanelControlID = Clinical_RadiologyOrder.params.PanelID + " #dgvExternalRadiologyResult_Paging";
                var ClassControlName = "Clinical_RadiologyOrder";
                var PagesToDisplay = 15;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(
                    CreatePagination(response.RadiologyResultCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        Clinical_RadiologyOrder.externalRadiologyResultsSearch(PrimaryID, PageNumber, ResultPerPage);
                    }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
                dfd.resolve();
            }
        });
        return dfd;
    },

    radiologyResultDelete: function (radiologyResultId, event, PageNo, rpp) {
        var strMessage = "";
        event.stopPropagation();

        utility.myConfirm('1', function () {
            var selectedValue = radiologyResultId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                var self = $("#" + Clinical_RadiologyOrder.params.PanelID + " form");
                var myJSON = self.getMyJSONByName();

                Clinical_RadiologyOrder.deleteRadiologyResult(myJSON, radiologyResultId, PageNo, rpp).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        Clinical_RadiologyOrder.radiologyResultsSearch();
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
        objData["PatientId"] = Clinical_RadiologyOrder.patientId;
        objData["commandType"] = "delete_radiologyresult";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyResult", "RadiologyResult");

    },


    searchRadiologyResults: function (radResultData, radResultId, PageNumber, RowsPerPage, RadiologyOrderIdShouldBeNull, Type) {
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
        if (typeof RadiologyOrderIdShouldBeNull != typeof undefined && RadiologyOrderIdShouldBeNull != null && RadiologyOrderIdShouldBeNull) {
            objData["RadiologyOrderId"] = "";
        }
        objData["RadiologyResultId"] = radResultId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PatientId"] = Clinical_RadiologyOrder.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : Clinical_RadiologyOrder.patientId;
        objData["Test"] = objData["ResultCPTCode"];
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;

        var divId = " #pnlRadiologyResult_Search"
        if (Type == "External") {
            divId = " #pnlExternalRadiologyResult_Search"
        }

        //objData["LabId"] = $('#' + Clinical_RadiologyOrder.params.PanelID + divId + ' #frmClinicalRadiologyResult #ddlLaboratory').val();

        objData["ProviderId"] = $('#' + Clinical_RadiologyOrder.params.PanelID + divId + ' #frmClinicalRadiologyResult #txtProvider').val();
        objData["OrderFromDate"] = $('#' + Clinical_RadiologyOrder.params.PanelID + divId + ' #frmClinicalRadiologyResult #dpStartDate').val();
        objData["Status"] = $('#' + Clinical_RadiologyOrder.params.PanelID + divId + ' #frmClinicalRadiologyResult #ddlStatus option:selected').val();
        objData["OrderToDate"] = $('#' + Clinical_RadiologyOrder.params.PanelID + divId + ' #frmClinicalRadiologyResult #dpToDate').val();
        objData["OrderNo"] = $('#' + Clinical_RadiologyOrder.params.PanelID + divId + ' #frmClinicalRadiologyResult #txtOrderNumber').val();
        objData["AllResults"] = 1;
        objData["RadiologyType"] = Type;

        objData["commandType"] = "search_radiologyResults";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyResult", "RadiologyResult");
    },

    radiologyResultsGridLoad: function (response) {
        var dfd = $.Deferred();
        //Start 24-05-2016 Muhammad Arshad Remove Duplicate search issue on Datatable

        if ($.fn.dataTable.isDataTable("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyResult_Result  #" + Clinical_RadiologyOrder.resultSearchGridId)) {
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyResult_Result  #" +Clinical_RadiologyOrder.resultSearchGridId).dataTable().fnClearTable();
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyResult_Result  #" + Clinical_RadiologyOrder.resultSearchGridId).dataTable().fnDestroy();
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyResult_Result  #" + Clinical_RadiologyOrder.resultSearchGridId+" tbody").find("tr").remove();
        } else {
            //for stop make dublicate Datatables
            $.each($.fn.DataTable.fnTables(), function () {
                if (this.id == Clinical_RadiologyOrder.resultSearchGridId) {
                    $(this).dataTable().fnClearTable();
                    $(this).dataTable().fnDestroy();
                    $(this).find("tbody tr").remove();
                    $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyResult_Result  #" + Clinical_RadiologyOrder.resultSearchGridId+" tbody").find("tr").remove();
                    $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyResult_Result  #" + Clinical_RadiologyOrder.resultSearchGridId).parent().parent().find('div.row').remove();
                }
            })
        }
        //if ($.fn.dataTable.isDataTable("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyResult_Result #" + Clinical_RadiologyOrder.resultSearchGridId)) {
        //    $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyResult_Result #" + Clinical_RadiologyOrder.resultSearchGridId).dataTable().fnClearTable();
        //    $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyResult_Result #" + Clinical_RadiologyOrder.resultSearchGridId).dataTable().fnDestroy();

        //}
        //$("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyResult_Result #" + Clinical_RadiologyOrder.resultSearchGridId + " tbody").find("tr").remove();
       

        var maxLabOrderId = 0;
        Clinical_RadiologyOrder.params["LastRadiologyResultLabName"] = "";


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

                if (parseInt(item.RadiologyOrderResultId) > parseInt(maxLabOrderId)) {
                    maxLabOrderId = item.RadiologyOrderResultId;
                    Clinical_RadiologyOrder.params["LastRadiologyResultLabName"] = item.Laboratory;
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

                if (Clinical_RadiologyOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                    parentControl = 'Clinical_RadiologyOrder';
                    parentControlPanelID = Clinical_RadiologyOrder.params.PanelID;

                    SelectionCheckBoxColumn = '<td><input type="checkbox" onchange="Clinical_RadiologyOrder.enableAddResult(this,event);" id="' + item.RadiologyOrderResultId + '" name="SelectCheckBoxOrder" ' + Checked + ' class="input-block"/></td>';
                }
                else if (Clinical_RadiologyOrder.params.ParentCtrl == "clinicalTabFaceSheet") {
                    parentControl = 'Clinical_RadiologyOrder';
                    parentControlPanelID = Clinical_RadiologyOrder.params.PanelID;
                }
                else {
                    parentControl = 'clinicalTabRadiologyOrder';
                    SelectionCheckBoxColumn = "";
                }
                //End//21-04-2016//Ahmad Raza//logic to check uncheck order if it is attached with Note

                l = item.LOINC.split('|');
                var loincDescriptionArray = item.LOINCDescription.split('|');


                var divLoinc = '<div>';
                $.each(loincDescriptionArray, function (index, item) {

                    var loinc = "";//(loincArray[index] == null || loincArray[index] == "") ? "" : loincArray[index] + " - ";
                    var loincDescription = loincDescriptionArray[index];
                    loincDescription = loincDescription.substring(loincDescription.indexOf(" ") + 3, loincDescription.length);
                    var loincSegment = "<p>" + loinc + loincDescription + Clinical_InfoButtonView.GenerateInfoLink(loinc, parentControl, 3, parentControlPanelID) + "</p>";
                    divLoinc = divLoinc + loincSegment;

                });
                divLoinc = divLoinc + "</div>"
                var attachementIcon = "";
                if (item.IsAttachmentExists == "True") {
                    attachementIcon = '<div class="dropdown" style="display:inline-block;">' +
                                    '<a id="btnViewAttachment"' + item.RadiologyOrderResultId + ' href="#" class="dropdown-toggle fa fa-paperclip btn btn-link btn-xs p-none" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false" onclick="ClinicalRadiologyResultDetail.loadAttachments(\'Clinical_RadiologyOrder\',\'' + item.RadiologyOrderId + '\',\'' + item.RadiologyOrderResultId + ' \',\'' + Clinical_RadiologyOrder.resultSearchGridId + '\')"></a>' +
                                    '<ul id="menuViewAttachment' + item.RadiologyOrderResultId + '" class="dropdown-menu" aria-labelledby="btnViewAttachment"></ul></div>';

                }
                //Start//20-04-2016//Ahmad Raza//implimented logic to show icon for showing DBAudit History
                $row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.RadiologyOrderResultId + '</td><td><a class="btn btn-xs" href="#" onclick="Clinical_RadiologyOrder.radiologyResultDelete(\'' + item.RadiologyOrderResultId + '\', event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_RadiologyOrder.radiologyResultAddEdit(\'' + item.RadiologyOrderResultId + '\',\'' + item.RadiologyOrderId + '\', false);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_RadiologyOrder.printRadiologyResult(\'' + item.RadiologyOrderId + '\',\'' + item.RadiologyOrderResultId + '\',\'' + item.Status + '\',event );" title="View Result"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_RadiologyOrder.showRadiologyResultHistory(\'' + item.RadiologyOrderResultId + '\', event );" title="Activity Log"> <i class="fa fa-history blue"></i></a>' + attachementIcon + '</td><td>'
                        + currentDate + " " + currentTime + '</td><td>' + divLoinc + '</td><td>' + item.OrderNo + '</td><td>' + item.Status + '</td><td>' + item.ProviderName + '</td><td>' + item.AssignedTo + '</td>');
                $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyResult_Result #" + Clinical_RadiologyOrder.resultSearchGridId + " tbody").last().append($row);
            });
        }
        else {
            $("#" + Clinical_RadiologyOrder.params.PanelID + ' #pnlRadiologyResult_Result #' + Clinical_RadiologyOrder.resultSearchGridId).DataTable({
                "language": {
                    "emptyTable": "No Diagnostic Imaging Result Found"
                }, "autoWidth": false, "bLengthChange": false, "aaSorting": [[1, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_RadiologyOrder.params.PanelID + ' #pnlRadiologyResult_Result #' + Clinical_RadiologyOrder.resultSearchGridId))
            ;
        else {
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyResult_Result #" + Clinical_RadiologyOrder.resultSearchGridId).DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [[1, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        }

        EMRUtility.fixDataTableDuplication("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlRadiologyResult_Result");
        dfd.resolve();
        return dfd;
    },

    externalRadiologyResultsGridLoad: function (response) {
        var dfd = $.Deferred();
        //Start 24-05-2016 Muhammad Arshad Remove Duplicate search issue on Datatable


        if ($.fn.dataTable.isDataTable("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlExternalRadiologyResult_Result  #" + Clinical_RadiologyOrder.externalResultSearchGridId)) {
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlExternalRadiologyResult_Result  #" + Clinical_RadiologyOrder.externalResultSearchGridId).dataTable().fnClearTable();
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlExternalRadiologyResult_Result  #" + Clinical_RadiologyOrder.externalResultSearchGridId).dataTable().fnDestroy();
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlExternalRadiologyResult_Result  #" + Clinical_RadiologyOrder.externalResultSearchGridId + " tbody").find("tr").remove();
        } else {
            //for stop make dublicate Datatables
            $.each($.fn.DataTable.fnTables(), function () {
                if (this.id == Clinical_RadiologyOrder.externalResultSearchGridId) {
                    $(this).dataTable().fnClearTable();
                    $(this).dataTable().fnDestroy();
                    $(this).find("tbody tr").remove();
                    $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlExternalRadiologyResult_Result  #" + Clinical_RadiologyOrder.externalResultSearchGridId + " tbody").find("tr").remove();
                    $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlExternalRadiologyResult_Result  #" + Clinical_RadiologyOrder.externalResultSearchGridId).parent().parent().find('div.row').remove();
                }
            })
        }

        //if ($.fn.dataTable.isDataTable("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlExternalRadiologyResult_Result #" + Clinical_RadiologyOrder.externalResultSearchGridId)) {
        //    $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlExternalRadiologyResult_Result #" + Clinical_RadiologyOrder.externalResultSearchGridId).dataTable().fnClearTable();
        //    $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlExternalRadiologyResult_Result #" + Clinical_RadiologyOrder.externalResultSearchGridId).dataTable().fnDestroy();

        //}
        //$("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlExternalRadiologyResult_Result #" + Clinical_RadiologyOrder.externalResultSearchGridId + " tbody").find("tr").remove();

        var maxLabOrderId = 0;
        Clinical_RadiologyOrder.params["LastRadiologyResultLabName"] = "";


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

                if (parseInt(item.RadiologyOrderResultId) > parseInt(maxLabOrderId)) {
                    maxLabOrderId = item.RadiologyOrderResultId;
                    Clinical_RadiologyOrder.params["LastRadiologyResultLabName"] = item.Laboratory;
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

                if (Clinical_RadiologyOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                    parentControl = 'Clinical_RadiologyOrder';
                    parentControlPanelID = Clinical_RadiologyOrder.params.PanelID;

                    SelectionCheckBoxColumn = '<td><input type="checkbox" onchange="Clinical_RadiologyOrder.enableAddResult(this,event);" id="' + item.RadiologyOrderResultId + '" name="SelectCheckBoxOrder" ' + Checked + ' class="input-block"/></td>';
                }
                else if (Clinical_RadiologyOrder.params.ParentCtrl == "clinicalTabFaceSheet") {
                    parentControl = 'Clinical_RadiologyOrder';
                    parentControlPanelID = Clinical_RadiologyOrder.params.PanelID;
                }
                else {
                    parentControl = 'clinicalTabRadiologyOrder';
                    SelectionCheckBoxColumn = "";
                }
                //End//21-04-2016//Ahmad Raza//logic to check uncheck order if it is attached with Note

                l = item.LOINC.split('|');
                var loincDescriptionArray = item.LOINCDescription.split('|');


                var divLoinc = '<div>';
                $.each(loincDescriptionArray, function (index, item) {

                    var loinc = "";//(loincArray[index] == null || loincArray[index] == "") ? "" : loincArray[index] + " - ";
                    var loincDescription = loincDescriptionArray[index];
                    loincDescription = loincDescription.substring(loincDescription.indexOf(" ") + 3, loincDescription.length);
                    var loincSegment = "<p>" + loinc + loincDescription + Clinical_InfoButtonView.GenerateInfoLink(loinc, parentControl, 3, parentControlPanelID) + "</p>";
                    divLoinc = divLoinc + loincSegment;

                });
                divLoinc = divLoinc + "</div>"
                var attachementIcon = "";
                if (item.IsAttachmentExists == "True") {
                    attachementIcon = '<div class="dropdown" style="display:inline-block;">' +
                                    '<a id="btnViewAttachment"' + item.RadiologyOrderResultId + ' href="#" class="dropdown-toggle fa fa-paperclip btn btn-link btn-xs p-none" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false" onclick="ClinicalRadiologyResultDetail.loadAttachments(\'Clinical_RadiologyOrder\',\'' + item.RadiologyOrderId + '\',\'' + item.RadiologyOrderResultId + ' \',\'' + Clinical_RadiologyOrder.externalResultSearchGridId + '\')"></a>' +
                                    '<ul id="menuViewAttachment' + item.RadiologyOrderResultId + '" class="dropdown-menu" aria-labelledby="btnViewAttachment"></ul></div>';

                }
                //Start//20-04-2016//Ahmad Raza//implimented logic to show icon for showing DBAudit History
                $row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.RadiologyOrderResultId + '</td><td><a class="btn btn-xs" href="#" onclick="Clinical_RadiologyOrder.radiologyResultDelete(\'' + item.RadiologyOrderResultId + '\', event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_RadiologyOrder.radiologyResultAddEdit(\'' + item.RadiologyOrderResultId + '\',\'' + item.RadiologyOrderId + '\', false);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_RadiologyOrder.printRadiologyResult(\'' + item.RadiologyOrderId + '\',\'' + item.RadiologyOrderResultId + '\',\'' + item.Status + '\',event );" title="View Result"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_RadiologyOrder.showRadiologyResultHistory(\'' + item.RadiologyOrderResultId + '\', event );" title="Activity Log"> <i class="fa fa-history blue"></i></a>' + attachementIcon + '</td><td>'
                        + currentDate + " " + currentTime + '</td><td>' + divLoinc + '</td><td>' + item.OrderNo + '</td><td>' + item.Status + '</td><td>' + item.ProviderName + '</td><td>' + item.AssignedTo + '</td>');
                $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlExternalRadiologyResult_Result #" + Clinical_RadiologyOrder.externalResultSearchGridId + " tbody").last().append($row);
            });
        }
        else {
            $("#" + Clinical_RadiologyOrder.params.PanelID + ' #pnlExternalRadiologyResult_Result #' + Clinical_RadiologyOrder.externalResultSearchGridId).DataTable({
                "language": {
                    "emptyTable": "No Diagnostic Imaging Result Found"
                }, "autoWidth": false, "bLengthChange": false, "aaSorting": [[1, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_RadiologyOrder.params.PanelID + ' #pnlExternalRadiologyResult_Result #' + Clinical_RadiologyOrder.externalResultSearchGridId))
            ;
        else {
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlExternalRadiologyResult_Result #" + Clinical_RadiologyOrder.externalResultSearchGridId).DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [[1, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        }

        EMRUtility.fixDataTableDuplication("#" + Clinical_RadiologyOrder.params.PanelID + " #pnlExternalRadiologyResult_Result");
        dfd.resolve();
        return dfd;
    },

    printRadiologyResult: function (LabId, ResultId, status, event) {
        // if (status == 'Signed') {
        var params = [];
        var PanelID = "";
        params["FromAdmin"] = "0";
        params["UserId"] = globalAppdata['AppUserId'];
        params["PatientId"] = $('#PatientProfile #hfPatientId').val();
        //  params["ParentCtrl"] = Clinical_LabOrder.params.TabID;
        params["ParentCtrl"] = (Clinical_RadiologyOrder.params.ParentCtrl != "clinicalTabProgressNote" && Clinical_RadiologyOrder.params.ParentCtrl != "clinicalTabFaceSheet") ? Clinical_RadiologyOrder.params.TabID : "Clinical_RadiologyOrder";
        params["RadiologyOrderId"] = LabId;
        params["RadiologyResultId"] = ResultId;
        if (Clinical_RadiologyOrder.params.ParentCtrl == "clinicalTabProgressNote") {
            PanelID = 'pnlClinicalProgressNote #pnlClinicalRadiologyOrder';
            params["PanelID"] = 'pnlClinicalProgressNote #pnlClinicalRadiologyOrder';
            params["PrPanelID"] = 'pnlClinicalProgressNote #pnlClinicalRadiologyOrder';
        }
        else if (Clinical_RadiologyOrder.params.ParentCtrl == "clinicalTabFaceSheet") {
            PanelID = 'pnlClinicalFaceSheet #pnlClinicalLabOrder';
            params["PanelID"] = 'pnlClinicalFaceSheet #pnlClinicalRadiologyOrder';
            params["PrPanelID"] = 'pnlClinicalFaceSheet #pnlClinicalRadiologyOrder';
        }
        else {
            PanelID = 'pnlClinicalRadiologyOrder';
            params["PanelID"] = 'pnlClinicalRadiologyOrder';
            params["PrPanelID"] = 'pnlClinicalRadiologyOrder';
        }
        LoadActionPan('Clinical_RadiologyResultView', params, PanelID);
        //   }
        event.stopPropagation();
    },



    enableAddResult: function (obj, event) {
        if (event != null) {
            event.stopPropagation();
        }

        var totalRows = $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.resultSearchGridId + " tr").length;
        totalRows -= 1;
        var selectedRows = $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.resultSearchGridId + " tbody tr input:checked").length;
        if (totalRows == selectedRows) {
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.resultSearchGridId + " #selectRecordOrders").prop("checked", true);
        }
        else {
            $("#" + Clinical_RadiologyOrder.params.PanelID + " #" + Clinical_RadiologyOrder.resultSearchGridId + " #selectRecordOrders").prop("checked", false);
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

        var ordergrid = " #dgvRadiologyResult";
        var type = $("#" + Clinical_RadiologyOrder.params.PanelID + " #ulRadilogyResultTypeTabsItems > li.active").text();
        if (type == "External") {
            ordergrid = " #dgvExternalRadiologyResult"
        }

        if ($("#" + Clinical_RadiologyOrder.params.PanelID + ordergrid).dataTable().fnSettings().aoData.length == 0) {
            //   Clinical_LabOrder.noActiveLabOrderSoapText();
        }
            //  }
        else {

            if ($("#" + Clinical_RadiologyOrder.params.PanelID + ordergrid + " #selectRecordOrders").prop('checked') == true || $("#" + Clinical_RadiologyOrder.params.PanelID + ordergrid + " #selectRecordOrders").prop('checked') == false) {

                $("#" + Clinical_RadiologyOrder.params.PanelID + ordergrid + " tbody").find('input[type="checkbox"]').each(function () {
                    Clinical_RadiologyOrder.enableAddResult(this);
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
                Clinical_RadiologyOrder.detachRadiologyResult_FromNotes(detachedvalues).done(function () {
                    if (AttachedSelectedOrders.join() != null && AttachedSelectedOrders.join() != '') {
                        Clinical_RadiologyOrder.attachRadiologyResult_WithNotes(AttachedSelectedOrders);
                    } else {
                        Clinical_ProgressNote.saveComponentSOAPText("Diagnostic Imaging Results");
                    }
                });
            } else
                if (AttachedSelectedOrders.join() != null && AttachedSelectedOrders.join() != '') {
                    Clinical_RadiologyOrder.attachRadiologyResult_WithNotes(AttachedSelectedOrders);
                }
            if (Clinical_RadiologyOrder.params && Clinical_RadiologyOrder.params.ParentCtrl && Clinical_RadiologyOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                UnloadActionPan(Clinical_RadiologyOrder.params.ParentCtrl, 'Clinical_RadiologyOrder');
                if ($("#" + Clinical_RadiologyOrder.params.PanelID + " #listRadiologyOrder").hasClass('active')) {
                    EMRUtility.scrollToPNcomponent('clinical_radiologyorders');
                    EMRUtility.scrollToPNcomponent('clinical_radiologyorder');
                }
                else if ($("#" + Clinical_RadiologyOrder.params.PanelID + " #listRadiologyResult").hasClass('active')) {
                    EMRUtility.scrollToPNcomponent('clinical_diagnosticimagingresults');
                    EMRUtility.scrollToPNcomponent('clinical_diagnosticimagingresults');
                    EMRUtility.scrollToPNcomponent('clinical_radiologyresults');
                }
            }
        }
    },
    attachRadiologyResult_WithNotes: function (AttachedSelectedOrders) {
        Clinical_RadiologyOrder.getRadiologyResultInfo(AttachedSelectedOrders.join()).done(function () {
            setTimeout(function () {
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                if (Clinical_RadiologyOrder.params != null && Clinical_RadiologyOrder.params.PanelID.indexOf('pnlClinicalRadiologyOrder') != -1) {
                    Clinical_RadiologyOrder.radiologyResultsSearch();
                }
            }, 5);
        });
    },
    detachRadiologyResult_FromNotes: function (detachedvalues) {
        var dfd = new $.Deferred();
        Clinical_RadiologyOrder.detachRadiologyResultFromNotesDBCall(detachedvalues.join()).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                for (var i = 0; i < detachedvalues.length; i++) {
                    var ALid = detachedvalues[i];
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_DiagnosticImagingResultDetail_Main' + ALid).remove();
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

        Clinical_RadiologyOrder.getResultsForSOAP_DBCall(radiologyOrderId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.ResultSoapCount > 0) {
                        Clinical_RadiologyOrder.createResultBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', radiologyOrderId);
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
        objData["PatientId"] = Clinical_RadiologyOrder.params.patientID;
        objData["ProviderId"] = $("#pnlClinicalProgressNote #hfProviderId").val();
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

    createResultBodyHTML: function (response, noteHTMLCtrl, resultId, hidemessage, bNotSaveCompt) {
        Clinical_RadiologyOrder.checkResultExists();
        if (response.RadiologyResultFill_JSON != null && response.RadiologyResultFill_JSON != '') {
            var MedicationsSOAPJSON = JSON.parse(response.RadiologyResultFill_JSON);
            var RadOrderResultParentSOAPJSON = JSON.parse(response.RadiologyOrderResultParent_JSON);
            var RadOrderResultChildSOAPJSON = JSON.parse(response.RadiologyOrderResultChild_JSON);


            //      var medicationReviewSoapJSON = [];
            if (response.MedicationReviewSoap_JSON != null) {
                //          medicationReviewSoapJSON = JSON.parse(response.MedicationReviewSoap_JSON);
            }

            var $mainDivMedications = $(document.createElement('div'));
            if (MedicationsSOAPJSON == null || MedicationsSOAPJSON.length == 0) {
                //}
            } else {
                if ($(noteHTMLCtrl + ' clinical_diagnosticimagingresults').parent().parent().find('#Cli_DiagnosticImagingResultDetail_Main0').length != 0) {
                    $(noteHTMLCtrl + ' #Cli_DiagnosticImagingResultDetail_Main0').parent().remove();
                }
            }

            var AListId = [];
            $.each(MedicationsSOAPJSON, function (index, element) {
                var RadOrderResultSoapText = "";
                var ALid = element.RadiologyOrderResultId;
                //             var $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(element.NDCID, 'clinicalTabProgressNote', 1);
                var $SectionBodyMedications = $(document.createElement('section'));
                $SectionBodyMedications.attr('id', "Cli_DiagnosticImagingResultDetail_Main" + ALid);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_DiagnosticImagingResultDetail_" + ALid);
                var $ListMedications = $(document.createElement('ul'));
                var duration = "";
                $ListMedications.attr('class', 'list-unstyled')

                $SectionBodyMedications.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_DiagnosticImagingResultDetail_" + ALid + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_DiagnosticImagingResultDetail_Main" + ALid + '"  ><i class="fa fa-times"></i></a></div> ');
                // var obj = $('<p>');
                //  $(obj).html(element.SoapText);
                //$(obj).html($(obj).text())
                //$ListMedications.append("<li>" + element.SoapText + "</li>");

                $.each(RadOrderResultParentSOAPJSON, function (i, item) {
                    var RadOrderResultParentSoapText = "", RadOrderResultChildSoapText = "";
                    RadOrderResultParentSoapText += "<div class='table-responsive'><table class='table table-bordered table-condensed'><thead><tr><th align='left' colspan='6'>"
                                                    + (item.ShowCPTCode == 0 ? item.CPTCodeDescription : item.CPTCode + " " + item.CPTCodeDescription) + "</th></tr>"
                                                    + "<tr style='background-color:#0188CC'><th>Date & Time</th><th>Observation</th><th>UoM</th><th>Flag</th><th>Range</th></tr></thead><tbody>"
                    $.each(RadOrderResultChildSOAPJSON, function (j, childItems) {
                        if (item.CPTCode == childItems.CPTCode && item.CPTCodeDescription == childItems.CPTCodeDescription) {
                            RadOrderResultChildSoapText += "<tr><td>" + childItems.ObservationDate + "</td>"
                                                           + "<td>" + childItems.LOINCDescription + "<a onclick=\"Clinical_InfoButtonView.getInfofromMediPlus('" + childItems.LOINC + "', 'clinicalTabProgressNote','3','')\" style=\"cursor:pointer\"><b>(Info)</b></a> " + "</td>"
                                                           //+ "<td>" + childItems.Result + "</td>"
                                                           + "<td>" + childItems.UoM + "</td>"
                                                           + "<td>" + childItems.Flag + "</td>"
                                                           + "<td>" + childItems.Range + "</td></tr>"
                        }
                    });
                    RadOrderResultSoapText += RadOrderResultParentSoapText + "" + RadOrderResultChildSoapText + "</tbody></table></div>";
                });
                //element.Comments = element.Comments.replace(/&lt;/g, '<').replace(/&gt;/g, '>')
                element.Comments = element.Comments.replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&amp;/g, '&');
                element.Comments = element.Comments.replace(/&lt;/g, '<');
                element.Comments = element.Comments.replace(/&gt;/g, '>');
                element.Comments = element.Comments.replace(/&amp;/g, '&');
                element.Comments = element.Comments.replace(/&nbsp;/g, ' ');
                $ListMedications.append("<li>" + RadOrderResultSoapText + (element.Status == "" ? "" : "</br><b>Status:</b> " + element.Status + "</br>")
                                        + (element.Remarks == "" ? "" : "</br><b>Remarks:</b> " + element.Remarks + "</br>")
                                        + (element.Comments == "" ? "" : "</br><b>Comments:</b> " + $("<div>" + element.Comments + "</div>").html() + "</br>") + "</li>");

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
                if ($(noteHTMLCtrl + ' clinical_diagnosticimagingresults').parent().parent().find('#Cli_DiagnosticImagingResultDetail_Main' + ALid).length == 0) {
                    AListId.push(ALid);
                    $mainDivMedications.append($SectionBodyMedications);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(noteHTMLCtrl + ' clinical_diagnosticimagingresults').parent().parent().find('#Cli_DiagnosticImagingResultDetail_Main' + ALid + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(noteHTMLCtrl + ' clinical_diagnosticimagingresults').parent().parent().find('#Cli_DiagnosticImagingResultDetail_Main' + ALid + ' ul li:Last').get(0).outerHTML;
                    }
                    $(noteHTMLCtrl + ' clinical_diagnosticimagingresults').parent().parent().find('#Cli_DiagnosticImagingResultDetail_Main' + ALid).html($SectionBodyMedications.html());
                    $(noteHTMLCtrl + ' clinical_diagnosticimagingresults').parent().parent().find('#Cli_DiagnosticImagingResultDetail_Main' + ALid + ' ul').append(CommentHTML);;
                }


            });

            if (AListId.join(",") != "") {
                resultId = AListId.join(",");
            }
            if ($mainDivMedications.html() != '') {
                Clinical_RadiologyOrder.updateResultHtml($mainDivMedications.html(), resultId, noteHTMLCtrl, hidemessage, bNotSaveCompt);
            } else {
                Clinical_RadiologyOrder.updateResultHtml('', resultId, noteHTMLCtrl, hidemessage, bNotSaveCompt);
            }
        }

    },

    checkResultExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_diagnosticimagingresults').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ObjectiveNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="DiagnosticImagingResultsComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_diagnosticimagingresults title="Diagnostic Imaging Results"  id="' + this.id + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'RadiologyResults\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Diagnostic Imaging Results">Diagnostic Imaging Results</a> ' +
                        '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'RadiologyResults\');" class=" btn btn-link btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'RadiologyResults\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_diagnosticimagingresults> </header></li>');
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
                $(this).find('.closeBtn').removeClass('hidden');
                $(this).css('background', '#EAF1F8');
            });

            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
                $(this).find('.closeBtn').addClass('hidden');
                $(this).css('background', '#fff');
            });
        }
        else
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_diagnosticimagingresults').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
    },

    updateResultHtml: function (resultHTML, resultID, noteHTMLCtrl, hidemessage, bNotSaveCompt) {
        $(noteHTMLCtrl + ' clinical_diagnosticimagingresults').parent().parent().addClass('initialVisitBody');
        if (resultHTML != '') {
            $(noteHTMLCtrl + ' clinical_diagnosticimagingresults').parent().parent().append(resultHTML);
        }

        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (resultHTML != '' && resultID != null && resultID != '' && resultID != '0') {
            Clinical_RadiologyOrder.attachResultFromNotes(resultID, hidemessage, bNotSaveCompt);
        }

    },

    attachResultFromNotes: function (resultID, hidemessage, bNotSaveCompt) {
        var selectedValue = resultID;
        if (selectedValue == "" || selectedValue == "undefined") {
        }
        else {
            Clinical_RadiologyOrder.attachResultsWithNotes_DBCall(selectedValue).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (!bNotSaveCompt)
                        Clinical_ProgressNote.saveComponentSOAPText("Diagnostic Imaging Results", hidemessage);
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
        var radiologyOrderIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_diagnosticimagingresults').parent().parent().find('section[id*="Cli_DiagnosticImagingResultDetail_Main"]').map(function () {
            return this.id.replace("Cli_DiagnosticImagingResultDetail_Main", "");
        }).get().join(',');
        var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .DiagnosticImagingResultsComponent').attr('NoteComponentId');

        if (radiologyOrderIds == "" || radiologyOrderIds == "undefined") {
            Clinical_ProgressNote.saveComponentSOAPText("Diagnostic Imaging Results", true);
            utility.DisplayMessages('Successfully Deleted', 1);
        }
        else {
            Clinical_RadiologyOrder.detachRadiologyResultFromNotesDBCall(radiologyOrderIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (IsUpdate) {

                        Clinical_ProgressNote.saveComponentSOAPText("Diagnostic Imaging Results", true);
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
            if (NoteComponentId && NoteComponentId != "NCDummyId") {
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_diagnosticimagingresults').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Diagnostic Imaging Results', true))
                }
                else
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                $.when.apply($, promise).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_diagnosticimagingresults').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
            else {
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Radiology Results']").remove();
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_diagnosticimagingresults').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Diagnostic Imaging Results', true))
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
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_diagnosticimagingresults').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_diagnosticimagingresults').parent().parent().find('section[id*="Cli_DiagnosticImagingResultDetail_Main"]').remove();
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

        utility.myConfirm('1', function () {
            EMRUtility.scrollToPNcomponent('clinical_diagnosticimagingresult');
            EMRUtility.scrollToPNcomponent('clinical_radiologyresult');
            EMRUtility.scrollToPNcomponent('clinical_diagnosticimagingresults');
            EMRUtility.scrollToPNcomponent('clinical_radiologyresults');
            var selectedValue = radiologyOrderId.replace('Cli_DiagnosticImagingResultDetail_Main', '');
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Clinical_RadiologyOrder.detachRadiologyResultFromNotes_DBCall(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        $('#' + radiologyOrderId).remove();

                        Clinical_ProgressNote.saveComponentSOAPText("Diagnostic Imaging Results");
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
    getLatestRadiologyResultByPatientId: function (hidemessage, bNotSaveCompt) {

        Clinical_RadiologyOrder.getLatestRadiologyResultByPatientIdDBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_RadiologyOrder.createResultBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hidemessage, bNotSaveCompt);
            }
            else {
                utility.DisplayMessages(strMessage, 3);
            }

        });
    },

    getLatestRadiologyResultByPatientIdDBCall: function () {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["NoteId"] = Clinical_Notes.params.NotesId;
        objData["ProviderId"] = $("#pnlClinicalProgressNote #hfProviderId").val();
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
        if (Clinical_RadiologyOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
            ParentCtrl = 'Clinical_RadiologyOrder';
            ParentCtrlPanelID = Clinical_RadiologyOrder.params.PanelID;
        }
        else if (Clinical_RadiologyOrder.params["ParentCtrl"] == "clinicalTabFaceSheet" || Clinical_RadiologyOrder.params["ParentCtrl"] == "Clinical_FaceSheet") {
            ParentCtrl = 'Clinical_RadiologyOrder';
            ParentCtrlPanelID = Clinical_RadiologyOrder.params.PanelID;
        }
        EMRUtility.showCurrentItemHistory(Clinical_RadiologyOrder.params.PanelID, null, null, "RadiologyOrderResult,RadiologyOrderResultDetail", Clinical_RadiologyOrder.params.patientID, ParentCtrl, radiologyResultId, ParentCtrlPanelID);
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
                // params["ParentCtrl"] = "Clinical_RadiologyOrder";


                if (Clinical_RadiologyOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {

                    params["ParentCtrl"] = 'Clinical_RadiologyOrder';
                    params["ParentCtrlPanelID"] = Clinical_RadiologyOrder.params.PanelID;
                    LoadActionPan('Document_Viewer', params, Clinical_RadiologyOrder.params.PanelID);
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