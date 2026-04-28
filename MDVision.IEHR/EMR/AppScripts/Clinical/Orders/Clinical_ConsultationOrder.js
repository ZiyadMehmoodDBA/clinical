Clinical_ConsultationOrder = {
    //Author: Humaira Yousaf
    //Date: 17-03-2016
    //This file will handle all actions performed for Consultation Orders
    bIsFirstLoad: true,
    params: [],
    EditableGrid: null,
    myGrid: null,
    patientId: null,
    orderSearchGridId: 'dgvConsultationOrder',

    //Author: Humaira Yousaf
    //Date: 17-03-2016
    //This function will handle fill of Consultation Orders
    Load: function (params) {
        Clinical_ConsultationOrder.params = params;
        Clinical_ConsultationOrder.patientId = $("div#PatientProfile #hfPatientId").val();
        Clinical_ConsultationOrder.params.mode = "Add";

        if (Clinical_ConsultationOrder.params.PanelID != 'pnlClinicalConsultationOrder') {
            Clinical_ConsultationOrder.params.PanelID = Clinical_ConsultationOrder.params.PanelID + ' #pnlClinicalConsultationOrder';
        } else {
            Clinical_ConsultationOrder.params.PanelID = 'pnlClinicalConsultationOrder';
        }

        if (Clinical_ConsultationOrder.params.ParentCtrl == 'clinicalTabProgressNote') {

            $("#" + Clinical_ConsultationOrder.params.PanelID + " #btnAddConsultationOrderToNote").show();
        }
        else {
            Clinical_ConsultationOrder.orderSearchGridId = 'dgvConsultationOrder';
            //Clinical_ConsultationOrder.resultSearchGridId = 'dgvLabResult';

            $("#" + Clinical_ConsultationOrder.params.PanelID + " #btnAddConsultationOrderToNote").hide();
        }


        var self = $('#' + Clinical_ConsultationOrder.params.PanelID);
        if (Clinical_ConsultationOrder.bIsFirstLoad == true) {
            //Start 21-03-2016 Humaira Yousaf to validate consultation order
            Clinical_ConsultationOrder.validateConsultationOrder();
            //End 21-03-2016 Humaira Yousaf to validate consultation order
            Clinical_ConsultationOrder.fillProvider();
            Clinical_ConsultationOrder.consultationOrderSearch();
        }

        //Start 21-03-2016 Humaira Yousaf
        utility.CreateDatePicker(Clinical_ConsultationOrder.params.PanelID + ' #dpStartDate', function () {
            //on-change callback method
        });

        utility.CreateDatePicker(Clinical_ConsultationOrder.params.PanelID + ' #dpToDate', function () {
            //on-change callback method
        });
        //End 21-03-2016 Humaira Yousaf
        EMRUtility.ValidateFromToDate(Clinical_ConsultationOrder.params.PanelID + ' #frmClinicalConsultationOrder', 'dpStartDate', 'dpToDate', true, function () { }, function () { }, "To Date should be greater than From Date");
        Clinical_ConsultationOrder.SetCollapseExpandPanel();
    },


    SetCollapseExpandPanel: function () {

        //1- Initialization
        $('#' + Clinical_ConsultationOrder.params.PanelID + ' .splitterBtn').html('<a></a>');
        EMRUtility.changeIcon($('#' + Clinical_ConsultationOrder.params.PanelID + ' .splitterBtn a'));

        $('#' + Clinical_ConsultationOrder.params.PanelID + ' .splitterBtn a').click(function (e) {
            $(this).parent('.splitterBtn').prev().slideToggle(250).toggleClass('active');
            var a = $(this);
            EMRUtility.changeIcon(a);
        });

        //2- Default settings
        if (globalAppdata['IsSearchCriteriaExpand'] && globalAppdata['IsSearchCriteriaExpand'].toLowerCase() == 'true') {
            $('#' + Clinical_ConsultationOrder.params.PanelID + ' #splitterBody').attr('class', 'splitterBody active');
            $('#' + Clinical_ConsultationOrder.params.PanelID + ' #splitterBody').show();
        }
        else {
            $('#' + Clinical_ConsultationOrder.params.PanelID + ' #splitterBody').removeClass('splitterBody active');
            $('#' + Clinical_ConsultationOrder.params.PanelID + ' #splitterBody').hide();
        }

    },
    //Function Name: fillProvider
    //Author: Ahmad Raza
    //Date: 08-04-2016
    //Description: This function will fill Provider dropdown
    fillProvider: function () {
        IsBackgroundLoaderShow = false;
        Clinical_ConsultationOrder.fillproviderDBCall().done(function (response) {
            if (response != "") {
                response = JSON.parse(response);

                var $ddlProvider = $('#' + Clinical_ConsultationOrder.params.PanelID + ' #frmClinicalConsultationOrder #txtProvider');
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
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["commandType"] = "getorderingprovider";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ConsultationOrder", "ConsultationOrder");

    },

    //Method Name: bindAutoComplete
    //Author: Ahmad Raza
    //Date: 21-03-2016
    //Description: This function will handle fill procedure text box on input
    bindAutoComplete: function (element) {

        var hiddenCrtl = $('#' + Clinical_ConsultationOrder.params.PanelID + ' #txtCPTCode');
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "Clinical_ConsultationOrder", null, true);

    },

    //Function Name: consultationOrderDelete
    //Author Name: Ahmad Raza
    //Created Date: 22-03-2016
    //Description: Delete the selected Consultation Order
    consultationOrderDelete: function (consultationId, PageNo, rpp) {
        var strMessage = "";

        AppPrivileges.GetFormPrivileges("Orders and Results_Consultation", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = consultationId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        var self = $("#" + Clinical_ConsultationOrder.params.PanelID + " form");
                        var myJSON = self.getMyJSONByName();

                        Clinical_ConsultationOrder.deleteConsultationOrder(myJSON, consultationId, PageNo, rpp).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                utility.DisplayMessages(response.Message, 1);
                                Clinical_ConsultationOrder.consultationOrderSearch("", 1, 15);
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

    //Function Name: deleteConsultationOrder
    //Author Name: Ahmad Raza
    //Created Date: 22-03-2016
    //Description: DB Call to Delete the selected Consultation Order
    deleteConsultationOrder: function (consultationData, consultationId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = JSON.parse(consultationData);
        objData["ConsultationOrderId"] = consultationId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PatientId"] = Clinical_ConsultationOrder.patientId;
        objData["commandType"] = "delete_consultationorder";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ConsultationOrder", "ConsultationOrder");

    },


    //Method Name: openCPTCode
    //Author: Ahmad Raza
    //Date: 21-03-2016
    //Description: This function will handle opening of CPT Search Popup
    openCPTCode: function () {
        var params = [];
        //params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Clinical_ConsultationOrder";
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtCPTCode";
        params["ParentCtrlPanelID"] = Clinical_ConsultationOrder.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, Clinical_ConsultationOrder.params.PanelID);
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
    consultationOrderAddEdit: function (consultationOrderId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Orders and Results_Consultation", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                // Ast 357
                if (Clinical_ConsultationOrder.params.ParentCtrl && Clinical_ConsultationOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                    params["IsFromNote"] = true;
                }
                else {
                    params["IsFromNote"] = false;
                }
                params["CurrentNotesProviderId"] = Clinical_ConsultationOrder.params["CurrentNotesProviderId"];
                if (consultationOrderId != null && parseInt(consultationOrderId) > 0) {
                    params["ConsultationOrderId"] = consultationOrderId;
                    params["mode"] = "Edit";
                }
                else {
                    params["ConsultationOrderId"] = -1;
                    params["mode"] = "Add";
                }
                params["FromAdmin"] = Clinical_ConsultationOrder.params["FromAdmin"];

                params["ParentCtrl"] = 'clinicalTabConsultationOrder';
                params["TabID"] = 'ClinicalConsultationOrderDetail';
                if (Clinical_ConsultationOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
                    params["ParentCtrl"] = 'Clinical_ConsultationOrder';
                    params["ParentCtrlPanelID"] = 'pnlClinicalProgressNote';
                    LoadActionPan('ClinicalConsultationOrderDetail', params, Clinical_ConsultationOrder.params.PanelID);
                }
                else {
                    params["ParentCtrl"] = 'clinicalTabConsultationOrder';
                    LoadActionPan('ClinicalConsultationOrderDetail', params);
                }

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    validateConsultationOrder: function () {
        $('#' + Clinical_ConsultationOrder.params.PanelID + ' #frmClinicalConsultationOrder')
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
            Clinical_ConsultationOrder.consultationOrderSave();
        });
    },

    //Function Name: consultationOrderSearch
    //Author Name: Humaira Yousaf
    //Created Date: 17-03-2016
    //Description: Searches Consultation Orders
    //Params: consultationId, PageNo, rpp
    consultationOrderSearch: function (consultationId, PageNo, rpp, caller) {

        if (Clinical_ConsultationOrder.params.ParentCtrl == "clinicalTabProgressNote") {
            if ($("#" + Clinical_ConsultationOrder.params.PanelID + " #" + Clinical_ConsultationOrder.orderSearchGridId + " thead tr #selectRecordOrders").length == 0) {
                $("#" + Clinical_ConsultationOrder.params.PanelID + " #" + Clinical_ConsultationOrder.orderSearchGridId + " thead tr").prepend('<th style="width:50px"><input type="checkbox" onchange="Clinical_ConsultationOrder.checkUncheckAllOrders(this);" controlname="selectRecordOrders" id="selectRecordOrders" name="chkHeader" class="input-block" coltype="checkbox"/></th>');
            }
        } else {
            $("#" + Clinical_ConsultationOrder.params.PanelID + " #" + Clinical_ConsultationOrder.orderSearchGridId + " th#selectRecordOrders").remove();
        }


        if ($("#" + Clinical_ConsultationOrder.params.PanelID + " #pnlConsultationOrder_Result").css("display") == "none") {
            $("#" + Clinical_ConsultationOrder.params.PanelID + " #pnlConsultationOrder_Result").show();
        }

        var self = $("#" + Clinical_ConsultationOrder.params.PanelID + " form");
        var myJSON = self.getMyJSONByName();

        // search specific on caller Id
        if (caller != null) {
            if (caller.indexOf("ConsultationOrderDetail") >= 0) {
                myJSON = null;
                // Reset Controls
                Clinical_ConsultationOrder.consultationOrderReset;
                //// Reload Providers
                //Clinical_ConsultationOrder.fillProvider();
            }
        }

        Clinical_ConsultationOrder.searchconsultationOrder(myJSON, consultationId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_ConsultationOrder.consultationOrderGridLoad(response);
                //Start//28-06-2016//Ahmad Raza//logic for select All
                var totalRows = $("#" + Clinical_ConsultationOrder.params.PanelID + " #" + Clinical_ConsultationOrder.orderSearchGridId + " tr").length;
                totalRows -= 1;
                var selectedRows = $("#" + Clinical_ConsultationOrder.params.PanelID + " #" + Clinical_ConsultationOrder.orderSearchGridId + " tbody tr input:checked").length;
                if (totalRows == selectedRows) {
                    $("#" + Clinical_ConsultationOrder.params.PanelID + " #" + Clinical_ConsultationOrder.orderSearchGridId + " #selectRecordOrders").prop("checked", true);
                }
                else {
                    $("#" + Clinical_ConsultationOrder.params.PanelID + " #" + Clinical_ConsultationOrder.orderSearchGridId + " #selectRecordOrders").prop("checked", false);
                }
                //End//28-06-2016//Ahmad Raza//logic for select All
                var TableControl = Clinical_ConsultationOrder.params.PanelID + " #pnlConsultationOrder_Result #dgvConsultationOrder";
                var PagingPanelControlID = Clinical_ConsultationOrder.params.PanelID + " #dgvConsultationOrder_Paging";
                var ClassControlName = "Clinical_ConsultationOrder";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(
                    CreatePagination(response.consultationOrderCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        Clinical_ConsultationOrder.consultationOrderSearch(PrimaryID, PageNumber, ResultPerPage);
                    }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    //Function Name: checkUncheckAllOrders
    //Author Name: Ahmad Raza
    //Created Date: 28-06-2016
    //Description: this function will handle check/uncheck of all the grid rows
    checkUncheckAllOrders: function (obj) {
        if ($(obj).is(':checked')) {
            $("#" + Clinical_ConsultationOrder.params.PanelID + " input[name='SelectCheckBoxOrder']:checkbox").prop('checked', true);
        } else {
            $("#" + Clinical_ConsultationOrder.params.PanelID + " input[name='SelectCheckBoxOrder']:checkbox").prop('checked', false);
        }
        $("#" + Clinical_ConsultationOrder.params.PanelID + " #" + Clinical_ConsultationOrder.orderSearchGridId + " tbody").find('input[type="checkbox"]').each(function () {
            Clinical_ConsultationOrder.enableAddOrder(this, null);
        });

    },


    //Function Name: consultationOrderGridLoad
    //Author Name: Humaira Yousaf
    //Created Date: 17-03-2016
    //Description: Loads Consultation Orders data in grid
    //Params: response
    consultationOrderGridLoad: function (response) {
        //$("#" + Clinical_ConsultationOrder.params.PanelID + " #pnlConsultationOrder_Result #dgvConsultationOrder").dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it
        //$("#" + Clinical_ConsultationOrder.params.PanelID + " #pnlConsultationOrder_Result #dgvConsultationOrder tbody").find("tr").remove(); //Removing all the table data from table body
        if ($.fn.dataTable.isDataTable("#" + Clinical_ConsultationOrder.params.PanelID + " #pnlConsultationOrder_Result #" + Clinical_ConsultationOrder.orderSearchGridId)) {
            $("#" + Clinical_ConsultationOrder.params.PanelID + " #pnlConsultationOrder_Result #" + Clinical_ConsultationOrder.orderSearchGridId).dataTable().fnClearTable();
            $("#" + Clinical_ConsultationOrder.params.PanelID + " #pnlConsultationOrder_Result #" + Clinical_ConsultationOrder.orderSearchGridId).dataTable().fnDestroy();
        }
        $("#" + Clinical_ConsultationOrder.params.PanelID + " #pnlConsultationOrder_Result #" + Clinical_ConsultationOrder.orderSearchGridId + " tbody").find("tr").remove();
        if (response.consultationOrderCount > 0) {
            var consultationLoadJSONData = JSON.parse(response.ConsultationLoad_JSON); //Parsing array to JSON
            $.each(consultationLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                //$row.attr("onclick", "Clinical_ConsultationOrder.consultationOrderAddEdit1('" + item.ConsultationOrderId + "',event);");
                $row.attr("id", "gvConsultation_row" + item.ConsultationOrderId);
                $row.attr("ConsultationOrderId", item.ConsultationOrderId);
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
                    if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.ConsultationOrderId + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                        Checked = " ";
                    } else {
                        Checked = " checked";
                    }
                } else {
                    if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.ConsultationOrderId + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                        Checked = " checked";
                    } else {
                        Checked = "";
                    }
                }

                if (Clinical_ConsultationOrder.params.ParentCtrl == "clinicalTabProgressNote") {
                    SelectionCheckBoxColumn = '<td><input type="checkbox" onclick="Clinical_ConsultationOrder.enableAddOrder(this,event);" id="' + item.ConsultationOrderId + '" name="SelectCheckBoxOrder" ' + Checked + ' class="input-block"/></td>';
                } else {
                    SelectionCheckBoxColumn = "";
                }
                //End//21-04-2016//Ahmad Raza//logic to check uncheck order if it is attached with Note

                //Begin 08-04-2016 Edit by Humaira Yousaf Bug# EMR-626
                if (item.Status == "Signed") {
                    //Begin 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578
                    $row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.ConsultationOrderId + '</td><td><a class="btn btn-xs disableAll" href="#" onclick="Clinical_ConsultationOrder.consultationOrderDelete(\'' + item.ConsultationOrderId + '\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_ConsultationOrder.consultationOrderAddEdit(\'' + item.ConsultationOrderId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_ConsultationOrder.printConsultationOrder(\'' + item.ConsultationOrderId + '\', \'' + item.Status + '\');" title="View Result"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_ConsultationOrder.showOrderHistory(\'' + item.ConsultationOrderId + '\', event );" title="Activity Log"> <i class="fa fa-history blue"></i></a></td><td>'
                        + utility.RemoveTimeFromDate(null, item.OrderDate) + ' ' + item.OrderTime + '</td><td>' + item.Procedures.replace(/\|/g, "<br/>") + '</td><td>' + item.OrderNo + '</td><td>' + item.Status + '</td><td>' + item.ProviderName + '</td><td>' + item.AssigneeName + '</td>');
                    //End 28-03-2016 Edit by Humaira Yousaf Bug# EMR-578
                }
                else {
                    $row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.ConsultationOrderId + '</td><td><a class="btn btn-xs" href="#" onclick="Clinical_ConsultationOrder.consultationOrderDelete(\'' + item.ConsultationOrderId + '\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_ConsultationOrder.consultationOrderAddEdit(\'' + item.ConsultationOrderId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs disableAll" href="#" onclick="Clinical_ConsultationOrder.printConsultationOrder(\'' + item.ConsultationOrderId + '\', \'' + item.Status + '\');" title="View Result"> <i class="fa fa-credit-card blue"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_ConsultationOrder.showOrderHistory(\'' + item.ConsultationOrderId + '\', event );" title="Activity Log"> <i class="fa fa-history blue"></i></a></td><td>'
                        + utility.RemoveTimeFromDate(null, item.OrderDate) + ' ' + item.OrderTime + '</td><td>' + item.Procedures.replace(/\|/g, "<br/>") + '</td><td>' + item.OrderNo + '</td><td>' + item.Status + '</td><td>' + item.ProviderName + '</td><td>' + item.AssigneeName + '</td>');
                }
                //End 08-04-2016 Edit by Humaira Yousaf Bug# EMR-626
                $("#" + Clinical_ConsultationOrder.params.PanelID + " #pnlConsultationOrder_Result #dgvConsultationOrder tbody").last().append($row);

            });
        }
        else {
            $("#" + Clinical_ConsultationOrder.params.PanelID + ' #pnlConsultationOrder_Result #dgvConsultationOrder').DataTable({
                "bdestroy": true,
                "language": {
                    "emptyTable": "No Consultation Order Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_ConsultationOrder.params.PanelID + ' #pnlConsultationOrder_Result #dgvConsultationOrder'))
            ;
        else {
            $("#" + Clinical_ConsultationOrder.params.PanelID + " #pnlConsultationOrder_Result #dgvConsultationOrder").DataTable({ "destroy": true, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        }

        EMRUtility.fixDataTableDuplication("#" + Clinical_ConsultationOrder.params.PanelID + " #pnlConsultationOrder_Result");
    },
    //Function Name: searchconsultationOrder
    //Author Name: Humaira Yousaf
    //Created Date: 17-03-2016
    //Description: Searches Consultation Orders
    //Params: CDSId, PageNumber, RowsPerPage
    searchconsultationOrder: function (consultationData, consultationId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }

        var objData = {};

        if (consultationData != null)
            objData = JSON.parse(consultationData);

        objData["ConsultationOrderId"] = consultationId;
        objData["ProcedureOrderTitle"] = objData["CPTCode"];
        objData["ProviderId"] = $('#' + Clinical_ConsultationOrder.params.PanelID + ' #frmClinicalConsultationOrder #txtProvider').val();
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PatientId"] = Clinical_ConsultationOrder.patientId;
        objData["commandType"] = "search_consultationorders";
        objData["NoteId"] = Clinical_ConsultationOrder.params.NotesId == null ? 0 : Clinical_ConsultationOrder.params.NotesId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ConsultationOrder", "ConsultationOrder");
    },

    //Function Name: consultationOrderReset
    //Author Name: Farooq Ahmad
    //Created Date: 29-03-2016
    //Description: consultation Order Reset
    consultationOrderReset: function () {

        utility.myConfirm('22', function () {
            //Begin 23-06-2016 Edit by Humaira Yousaf Bug# PMS-5577
            $('#' + Clinical_ConsultationOrder.params.PanelID + ' #frmClinicalConsultationOrder').resetAllControls(null);
            //End 23-06-2016 Edit by Humaira Yousaf Bug# PMS-5577
            Clinical_ConsultationOrder.validateConsultationOrder();
        }, function () { },
            '22'
        );
    },

    UnLoadTab: function () {
        var objDeffered = $.Deferred();
        /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        if (Clinical_ConsultationOrder.params.ParentCtrl == "clinicalTabFaceSheet") {
            utility.UnLoadDialog(Clinical_ConsultationOrder.params.PanelID + ' #frmClinicalConsultationOrderDetail', function () {
                if (Clinical_ConsultationOrder.params["FromAdmin"] == "0") {
                    if (Clinical_ConsultationOrder.params != null && Clinical_ConsultationOrder.params.ParentCtrl != null) {
                        UnloadActionPan(Clinical_ConsultationOrder.params.ParentCtrl, 'Clinical_ConsultationOrder');
                    }
                    else
                        UnloadActionPan(null, 'Clinical_ConsultationOrder');
                }
                else {
                    $("#mstrDivMedical #clinicalMenu_Medical_CDS").remove();
                    RemoveAdminTab();
                }
                objDeffered.resolve();
            }, function () {
                if (Clinical_ConsultationOrder.params["FromAdmin"] == "0") {
                    if (Clinical_ConsultationOrder.params != null && Clinical_ConsultationOrder.params.ParentCtrl != null) {
                        UnloadActionPan(Clinical_ConsultationOrder.params.ParentCtrl, 'Clinical_ConsultationOrder');
                    }
                    else
                        UnloadActionPan(null, 'Clinical_ConsultationOrder');
                }
                else {
                    $("#mstrDivMedical #clinicalMenu_Medical_CDS").remove();
                    RemoveAdminTab();
                }
                objDeffered.resolve();
            });

            Clinical_FaceSheet.loadFaceSheet();
        }
        else if (Clinical_ConsultationOrder.params.ParentCtrl == "clinicalTabProgressNote") {
            var exist = false;
            $("#" + Clinical_ConsultationOrder.params.PanelID + " #dgvConsultationOrder tbody").find('input[type="checkbox"]').each(function () {
                if (this.checked) {
                    exist = true;
                }
                if (exist) {
                    return false;
                }
            });
            if (exist) {
                utility.myConfirmNote('1', function () {
                    Clinical_ConsultationOrder.addOrdersToNotes();
                    UnloadActionPan(Clinical_ConsultationOrder.params.ParentCtrl, 'Clinical_ConsultationOrder');
                    EMRUtility.scrollToPNcomponent('clinical_consultationorder');
                }, "", function () {
                    UnloadActionPan(Clinical_ConsultationOrder.params.ParentCtrl, 'Clinical_ConsultationOrder');
                    EMRUtility.scrollToPNcomponent('clinical_consultationorder');
                });
            }
            else {
                UnloadActionPan(Clinical_ConsultationOrder.params.ParentCtrl, 'Clinical_ConsultationOrder');
                EMRUtility.scrollToPNcomponent('clinical_consultationorder');
            }

        }
            /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        else {
            if (Clinical_ConsultationOrder.params["FromAdmin"] == "0") {
                if (Clinical_ConsultationOrder.params != null && Clinical_ConsultationOrder.params.ParentCtrl != null) {
                    Clinical_ConsultationOrder.addOrdersToNotes();
                    UnloadActionPan(Clinical_ConsultationOrder.params.ParentCtrl, 'Clinical_ConsultationOrder');
                }
                else {
                    Clinical_ConsultationOrder.addOrdersToNotes();
                    UnloadActionPan(null, 'Clinical_ConsultationOrder');
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
        params["RefForm"] = "frmClinicalConsultationOrder";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Clinical_ConsultationOrder";
        LoadActionPan('Admin_Provider', params);
    },
    //Function Name: openproviderdetail
    //Author Name: Humaira Yousaf
    //Created Date: 21-03-2016
    //Description: Opens Provider
    openproviderdetail: function () {
        var params = [];
        params["providerid"] = $('#pnlclinicalconsultationorder #hfprovider').val();
        params["mode"] = "edit";
        params["fromadmin"] = "0";
        params["refctrl"] = "txtprovider";
        params["parentctrl"] = 'Clinical_ConsultationOrder';
        loadactionpan('providerdetail', params);
    },

    //Method Name: activeFavoritesSearch
    // Author Name: Abid Ali
    //Craeted Date: 25-03-2016
    //Description:  Searches active and iactive records
    activeFavoritesSearch: function (obj) {
        var isactive = $(obj).attr('isactive');

        if (isactive == '1') {
            $(obj).attr('isactive', '0');
            Favorite_ProcedureOrder.Switch = 0;
        }
        else if (isactive == '0') {
            $(obj).attr('isactive', '1');
            Favorite_ProcedureOrder.Switch = 1;
        }

        Clinical_ConsultationOrder.consultationOrderSearch();
    },
    //Function Name: printConsultationOrder
    //Author Name: Humaira Yousaf
    //Created Date: 28-03-2016
    //Description: Previews Consultation Order
    printConsultationOrder: function (consultationId, status) {
        if (status == 'Signed') {
            var params = [];
            params["FromAdmin"] = "0";
            params["UserId"] = globalAppdata['AppUserId'];
            params["PatientId"] = $('#PatientProfile #hfPatientId').val();
            params["ConsultationOrderId"] = consultationId;
            if (Clinical_ConsultationOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
                params["ParentCtrl"] = 'Clinical_ConsultationOrder';
                params["ParentCtrlPanelID"] = 'pnlClinicalProgressNote';
                LoadActionPan('Clinical_ConsultationOrderView', params, Clinical_ConsultationOrder.params.PanelID);
            }else{
                params["ParentCtrl"] = "clinicalTabConsultationOrder";
                LoadActionPan('Clinical_ConsultationOrderView', params);
            }
        }
    },

    //Function Name: showOrderHistory
    //Author Name: Ahmad Raza
    //Created Date: 20-04-2016
    //Description: to show consultation order history
    showOrderHistory: function (consultationOrderId, event) {
        if (event != null) {
            event.stopPropagation();
        }

        var ParentCtrl = 'clinicalTabConsultationOrder';
        var ParentCtrlPanelID = null;
        if (Clinical_ConsultationOrder.params["ParentCtrl"] == "clinicalTabProgressNote") {
            ParentCtrl = 'Clinical_ConsultationOrder';
            ParentCtrlPanelID = Clinical_ConsultationOrder.params.PanelID;
        }
        //else if (Clinical_ConsultationOrder.params["ParentCtrl"] == "clinicalTabFaceSheet") {
        //    ParentCtrl = 'Clinical_ConsultationOrder';
        //    ParentCtrlPanelID = Clinical_ConsultationOrder.params.PanelID;
        //}

        EMRUtility.showCurrentItemHistory(Clinical_ConsultationOrder.params.PanelID, null, null, "ConsultationOrder,ConsultationOrderTest, ConsultationOrderProblem", null, ParentCtrl, consultationOrderId, ParentCtrlPanelID);
    },

    //Function Name: addOrdersToNotes
    //Author Name: Ahmad Raza
    //Created Date: 28-06-2016
    //Description: this function will handle adding orders to notes
    addOrdersToNotes: function (selectedAttachedOrders, selectedDetachedOrders) {
        var noteHTMLCtrl = '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML';
        if ($("#" + Clinical_ConsultationOrder.orderSearchGridId).dataTable().fnSettings().aoData.length == 0) {
            //   Clinical_ConsultationOrder.noActiveLabOrderSoapText();
        }
        else {

            if ($("#" + Clinical_ConsultationOrder.params.PanelID + " #selectRecordOrders").prop('checked') == true || $("#" + Clinical_ConsultationOrder.params.PanelID + " #selectRecordOrders").prop('checked') == false) {

                $("#" + Clinical_ConsultationOrder.params.PanelID + " #" + Clinical_ConsultationOrder.orderSearchGridId + " tbody").find('input[type="checkbox"]').each(function () {
                    Clinical_ConsultationOrder.enableAddOrder(this, null);
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
            if (AttachedSelectedOrders != null && AttachedSelectedOrders != '') {
                for (var i = 0; i < Clinical_ProgressNote.AttachedNoteComponentIds.length; i++) {
                    var ALid = Clinical_ProgressNote.AttachedNoteComponentIds[i];
                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_ConsultationOrderDetail_Main' + ALid).length != 0) {
                        var index = AttachedSelectedOrders.indexOf(ALid);
                        if (index > -1) {
                            AttachedSelectedOrders.splice(index, 1);
                        }
                    }
                }
            }
            var detachedvalues = DettachedSelectedOrders;
            if (detachedvalues.join() != null && detachedvalues.join() != '') {
                Clinical_ConsultationOrder.detachConsultationOrderFromNotes(detachedvalues).done(function () {
                    if (AttachedSelectedOrders.join() != null && AttachedSelectedOrders.join() != '') {
                        Clinical_ConsultationOrder.attachConsultationOrderFromNotes(AttachedSelectedOrders);
                    } else {
                        Clinical_ProgressNote.saveComponentSOAPText("ConsultationOrder");
                    }
                });
            } else if (AttachedSelectedOrders.join() != null && AttachedSelectedOrders.join() != '') {
                Clinical_ConsultationOrder.attachConsultationOrderFromNotes(AttachedSelectedOrders);
            }
        }
        if (Clinical_ConsultationOrder.params && Clinical_ConsultationOrder.params.ParentCtrl && Clinical_ConsultationOrder.params.ParentCtrl == "clinicalTabProgressNote") {
            UnloadActionPan(Clinical_ConsultationOrder.params.ParentCtrl, 'Clinical_ConsultationOrder');
            EMRUtility.scrollToPNcomponent('clinical_consultationorder');
        }
    },

    attachConsultationOrderFromNotes: function (AttachedSelectedOrders) {
        Clinical_ConsultationOrder.getConsultationOrderInfo(AttachedSelectedOrders.join()).done(function () {
            setTimeout(function () {
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                if (Clinical_ConsultationOrder.params != null && Clinical_ConsultationOrder.params.PanelID.indexOf('pnlClinicalConsultationOrder') != -1) {
                    Clinical_ConsultationOrder.consultationOrderSearch();
                }
            }, 5);
        });
    },

    detachConsultationOrderFromNotes: function (detachedvalues) {
        var dfd = new $.Deferred();
        Clinical_ConsultationOrder.detachConsultationOrderFromNotes_DBCall(detachedvalues.join()).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                for (var i = 0; i < detachedvalues.length; i++) {
                    var ALid = detachedvalues[i];
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_ConsultationOrderDetail_Main' + ALid).remove();
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
            dfd.resolve('ok');
        });
        return dfd.promise();
    },

    //Function Name: enableAddOrder
    //Author Name: Ahmad Raza
    //Created Date: 28-06-2016
    //Description: this function will handle adding orders to JSON array
    enableAddOrder: function (obj, event) {
        if (event != null) {
            event.stopPropagation();
        }

        var totalRows = $("#" + Clinical_ConsultationOrder.params.PanelID + " #" + Clinical_ConsultationOrder.orderSearchGridId + " tr").length;
        totalRows -= 1;
        var selectedRows = $("#" + Clinical_ConsultationOrder.params.PanelID + " #" + Clinical_ConsultationOrder.orderSearchGridId + " tbody tr input:checked").length;
        if (totalRows == selectedRows) {
            $("#" + Clinical_ConsultationOrder.params.PanelID + " #" + Clinical_ConsultationOrder.orderSearchGridId + " #selectRecordOrders").prop("checked", true);
        }
        else {
            $("#" + Clinical_ConsultationOrder.params.PanelID + " #" + Clinical_ConsultationOrder.orderSearchGridId + " #selectRecordOrders").prop("checked", false);
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

    //Function Name: getConsultationOrderInfo
    //Author Name: Ahmad Raza
    //Created Date: 28-06-2016
    //Description: this function will handle loading soap text of consultation order
    getConsultationOrderInfo: function (consultationOrderId) {
        if (consultationOrderId == null || consultationOrderId == '') {
            return false;
        }
        var dfd = new $.Deferred();
        Clinical_ConsultationOrder.getOrdersForSOAP_DBCall(consultationOrderId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.ConsultationOrderSoapCount > 0) {
                        Clinical_ConsultationOrder.createConsultationOrderBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', consultationOrderId);
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

    //Function Name: getOrdersForSOAP_DBCall
    //Author Name: Ahmad Raza
    //Created Date: 28-06-2016
    //Description: this function will call DB for loading soap text of consultation order
    getOrdersForSOAP_DBCall: function (consultationOrderId) {
        var objData = new Object();
        objData["ConsultationOrderIDs"] = consultationOrderId;
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["PatientId"] = Clinical_ConsultationOrder.params.patientID;
        objData["commandType"] = "get_Orders_forsoap";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ConsultationOrder", "ConsultationOrder");
    },

    //Function Name: createConsultationOrderBodyHTML
    //Author Name: Ahmad Raza
    //Created Date: 28-06-2016
    //Description: this function will create consultation order soap text on Progress Note
    createConsultationOrderBodyHTML: function (response, noteHTMLCtrl, consultationId) {
        Clinical_ConsultationOrder.checkConsultationOrderExists();
        if (response.ConsultationOrderSoap_JSON != null && response.ConsultationOrderSoap_JSON != '') {
            var ConsultationOrdersSOAPJSON = JSON.parse(response.ConsultationOrderSoap_JSON);
            if (response.ConsultationOrderReviewSoap_JSON != null) {
                //          medicationReviewSoapJSON = JSON.parse(response.ConsultationOrderReviewSoap_JSON);
            }

            var $mainDivConsultationOrder = $(document.createElement('div'));
            if (ConsultationOrdersSOAPJSON == null || ConsultationOrdersSOAPJSON.length == 0) {
            } else {
                if ($(noteHTMLCtrl + ' clinical_consultationorder').parent().parent().find('#Cli_ConsultationOrderDetail_Main0').length != 0) {
                    $(noteHTMLCtrl + ' #Cli_ConsultationOrderDetail_Main0').parent().remove();
                }
            }
            var AListId = [];
            $.each(ConsultationOrdersSOAPJSON, function (index, element) {
                var ALid = element.ConsultationOrderId;
                //             var $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(element.NDCID, 'clinicalTabProgressNote', 1);
                var $SectionBodyConsultationOrder = $(document.createElement('section'));
                $SectionBodyConsultationOrder.attr('id', "Cli_ConsultationOrderDetail_Main" + ALid);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_ConsultationOrderDetail_" + ALid);
                var $ListConsultationOrder = $(document.createElement('ul'));
                var duration = "";
                $ListConsultationOrder.attr('class', 'list-unstyled')

                $SectionBodyConsultationOrder.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_ConsultationOrderDetail_" + ALid + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_ConsultationOrderDetail_Main" + ALid + '"  ><i class="fa fa-times"></i></a></div> ');
                var obj = $('<p>');
                $(obj).html(element.SoapText);
                //$(obj).html($(obj).text())
                $ListConsultationOrder.append("<li>" + $(obj).text() + "</li>");
                if (ConsultationOrdersSOAPJSON.length - 1 == index) {
                }
                $DetailsDiv.append($ListConsultationOrder);
                $SectionBodyConsultationOrder.append($DetailsDiv);
                //if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML clinical_medications').parent().parent().find('#Cli_Medications_Main' + ALid).length == 0) {
                if ($(noteHTMLCtrl + ' clinical_consultationorder').parent().parent().find('#Cli_ConsultationOrderDetail_Main' + ALid).length == 0) {
                    AListId.push(ALid);
                    $mainDivConsultationOrder.append($SectionBodyConsultationOrder);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(noteHTMLCtrl + ' clinical_consultationorder').parent().parent().find('#Cli_ConsultationOrderDetail_Main' + ALid + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(noteHTMLCtrl + ' clinical_consultationorder').parent().parent().find('#Cli_ConsultationOrderDetail_Main' + ALid + ' ul li:Last').get(0).outerHTML;
                    }
                    $(noteHTMLCtrl + ' clinical_consultationorder').parent().parent().find('#Cli_ConsultationOrderDetail_Main' + ALid).html($SectionBodyConsultationOrder.html());
                    $(noteHTMLCtrl + ' clinical_consultationorder').parent().parent().find('#Cli_ConsultationOrderDetail_Main' + ALid + ' ul').append(CommentHTML);;
                }
            });

            if (AListId.join(",") != "") {
                consultationId = AListId.join(",");
            }
            if ($mainDivConsultationOrder.html() != '') {
                Clinical_ConsultationOrder.updateConsultationOrderHtml($mainDivConsultationOrder.html(), consultationId, noteHTMLCtrl);
            } else {
                Clinical_ConsultationOrder.updateConsultationOrderHtml('', consultationId, noteHTMLCtrl);
                Clinical_ProgressNote.saveComponentSOAPText("ConsultationOrder");
            }
        }

    },

    //Function Name: updateConsultationOrderHtml
    //Author Name: Ahmad Raza
    //Created Date: 28-06-2016
    //Description: This function will update the html of consultation order
    updateConsultationOrderHtml: function (consultationOrderHTML, consultationOrderID, noteHTMLCtrl) {
        $(noteHTMLCtrl + ' clinical_consultationorder').parent().parent().addClass('initialVisitBody');
        if (consultationOrderHTML != '') {
            $(noteHTMLCtrl + ' clinical_consultationorder').parent().parent().append(consultationOrderHTML);
        }
        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (consultationOrderHTML != '' && consultationOrderID != null && consultationOrderID != '' && consultationOrderID != '0') {
            Clinical_ConsultationOrder.attachConsultationOrderWithNotes(consultationOrderID);
        }

    },

    //Function Name: attachConsultationOrderWithNotes
    //Author Name: Ahmad Raza
    //Created Date: 28-06-2016
    //Description: This function will attach consultation order with notes
    attachConsultationOrderWithNotes: function (consultationOrderID) {
        var selectedValue = consultationOrderID;
        if (selectedValue == "" || selectedValue == "undefined") {
        }
        else {
            Clinical_ConsultationOrder.attachConsultationOrderWithNotes_DBCall(selectedValue).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    Clinical_ProgressNote.saveComponentSOAPText("ConsultationOrder");
                    $('#' + consultationOrderID).remove();

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    //Function Name: attachConsultationOrderWithNotes_DBCall
    //Author Name: Ahmad Raza
    //Created Date: 28-06-2016
    //Description: This function will call DB to attach consultation order with notes
    attachConsultationOrderWithNotes_DBCall: function (consultationOrderID) {
        var objData = new Object();
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["ConsultationOrderIDs"] = consultationOrderID;
        objData["commandType"] = "attach_orders_with_notes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ConsultationOrder", "ConsultationOrder");
    },

    //Function Name: detachConsultationOrderFromNotes_DBCall
    //Author Name: Ahmad Raza
    //Created Date: 28-06-2016
    //Description: This function will call DB to Detach consultation order from notes
    detachConsultationOrderFromNotes_DBCall: function (consultationOrderID) {
        var objData = new Object();
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["ConsultationOrderIDs"] = consultationOrderID;
        objData["commandType"] = "detach_orders_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "ConsultationOrder", "ConsultationOrder");
    },

    //Function Name: checkConsultationOrderExists
    //Author Name: Ahmad Raza
    //Created Date: 28-06-2016
    //Description: This function will check consultation order exists or not
    checkConsultationOrderExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_consultationorder').length == 0) {

            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #PlanNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="ConsultationOrderComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_consultationorder title="Consultation Orders"  id="' + this.id + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'ConsultationOrder\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Consultation Order">Consultation Order</a> ' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'ConsultationOrder\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_consultationorder> </header></li>');
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
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_consultationorder').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($('#' + Clinical_ProgressNote.params["PanelID"] + ' #hfNotesId').val());
    },

}