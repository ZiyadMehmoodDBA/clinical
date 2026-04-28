OutOfOfficeVisits = {

    //Author: Abid Ali
    //Date: 13-06-2016
    //This file will handle all actions performed for Physiacal Exam Data Template
    bIsFirstLoad: true,
    params: [],
    FromAdmin: '0',

    Load: function (params) {
        OutOfOfficeVisits.params = params;
        if (OutOfOfficeVisits.params.PanelID != 'pnlBillOutOfOfficeVisits') {
            OutOfOfficeVisits.params.PanelID = OutOfOfficeVisits.params.PanelID + ' #pnlBillOutOfOfficeVisits';
        } else {
            OutOfOfficeVisits.params.PanelID = 'pnlBillOutOfOfficeVisits';
        }

        if (OutOfOfficeVisits.bIsFirstLoad) {
            OutOfOfficeVisits.bIsFirstLoad = false;
        }
        EMRUtility.ValidateFromToDate('frmOOOVisit', 'dpStartDate', 'dpToDate', true, function () { }, function () { }, "To Date should be greater than From Date");
        EMRUtility.ValidateFromToDate('frmOOOVisit', 'dpStartDate', 'dpToDate', true, function () { }, function () { }, "To Date should be greater than From Date");
        if ($('#PatientProfile #hfPatientId').val() == "") {
            $('#' + OutOfOfficeVisits.params.PanelID + " #modalbody").addClass('hidden');
        } else {
            $('#' + OutOfOfficeVisits.params.PanelID + " #modalbody").removeClass('hidden');
        }
        OutOfOfficeVisits.loadOOOVisit();
        //Load Facility Providers    
        OutOfOfficeVisits.loadProviderFailityddl();
        OutOfOfficeVisits.domReady();

    },

    domReady: function () {
        (function ($) {
            'use strict';
            $(function () {
                $("#" + OutOfOfficeVisits.params.PanelID + ' [data-plugin-ios-switch]').each(function () {
                    var $this = $(this);

                    $this.themePluginIOS7Switch();
                });
            });
        }).apply(this, [jQuery]);
    },


    OOOVisitsAddEdit: function (OOOVisitId, VisitId, caller) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Miscellaneous_eSuperbill", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {

            if (strMessage == "") {
                var params = [];
                if (OOOVisitId != null && parseInt(OOOVisitId) > 0) {
                    params["mode"] = "Edit";
                }
                else {
                    params["mode"] = "Add";
                }
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = 'billTabOutOfOfficeVisits';
                params["BillingInfoId"] = OOOVisitId;
                params["PatientId"] = $('#PatientProfile #hfPatientId').val();
                params["VisitId"] = VisitId;
                //params["NotesId"] = parseInt(noteId);
                //params["VisitId"] = vistId;
                //params["BillingInfoId"] = parseInt(billInfoId);
                //params["BtnClicked"] = btnClicked;

                //params["PatientType"] = patientType.indexOf("New") > -1 ? 1 : 2;

                LoadActionPan('BillingInformation', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    loadOOOVisit: function (OOOVisitId, PageNo, rpp) {

        var self = $("#" + OutOfOfficeVisits.params["PanelID"] + " #frmOOOVisit");
        if ($("#" + OutOfOfficeVisits.params["PanelID"] + " #pnlBillOOOVisits_Result").css("display") == "none") {
            $("#" + OutOfOfficeVisits.params["PanelID"] + " #pnlBillOOOVisits_Result").show();
        }
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        OutOfOfficeVisits.getAllESuperbillsDbCall(myJSON, OOOVisitId, PageNo, rpp).done(function (response) {

            response = JSON.parse(response);
            OutOfOfficeVisits.OOOVisitGridLoad(response);

            if (response.status != false) {

                $("#" + OutOfOfficeVisits.params["PanelID"] + " #dgvOOOVisit_Paging").css("display", "inline");

                var TableControl = OutOfOfficeVisits.params.PanelID + " #pnlBillOOOVisits_Result #dgvOOOVisit_Paging";
                var PagingPanelControlID = OutOfOfficeVisits.params.PanelID + " #dgvOOOVisit_Paging";
                var ClassControlName = "OutOfOfficeVisits";
                var PagesToDisplay = 15;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                //Paging control
                setTimeout(
                    CreatePagination(response.BillingInfoCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        OutOfOfficeVisits.loadOOOVisit(PrimaryID, PageNumber, ResultPerPage);
                    }), 10);
            }
        });
    },


    OOOVisitGridLoad: function (response) {

        var $pagingControl = $('#' + OutOfOfficeVisits.params.PanelID + " #dgvOOOVisit_Paging");

        if ($.fn.dataTable.isDataTable("#" + OutOfOfficeVisits.params["PanelID"] + " #pnlBillOOOVisits_Result #dgvOOOVisits")) {
            $("#" + OutOfOfficeVisits.params["PanelID"] + " #pnlBillOOOVisits_Result #dgvOOOVisits").dataTable().fnDestroy();
            $("#" + OutOfOfficeVisits.params["PanelID"] + " #pnlBillOOOVisits_Result #dgvOOOVisits tbody").find("tr").remove();
        }

        //response.OutOfOfficeVisitCount

        if (response.status == true) {

            response.BillingInfoFill_JSON = JSON.parse(response.BillingInfoFill_JSON);

            $.each(response.BillingInfoFill_JSON, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", "gvOutOfOfficeVisit_row" + item.BillingInfoId);
                $row.attr("outOfOfficeVisitId", item.BillingInfoId);

                if (item.IsActive == "True") {
                    isactive = 0;
                    activeRecord = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    activeRecord = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }

                // var MethodMode = ""// OutOfOfficeVisits.getPatientSelectMethodName(item.outOfOfficeVisitId, item.AccountNumber, item.FullName, item.FirstName, item.LastName);

                var DeleteMethod = "OutOfOfficeVisits.OOOVisitDelete('" + item.BillingInfoId.trim() + "','" + item.Status.trim() + "',event);";
                var EditMethod = "OutOfOfficeVisits.OOOVisitsAddEdit('" + item.BillingInfoId.trim() + "','" + item.VisitId.trim() + "','" + "',event);";
                var viewClaimMethod = "OutOfOfficeVisits.viewClaim('" + item.BillingInfoId.trim() + "','" + item.VisitId.trim() + "','" + item.Status.trim() + "',event);";

                $row.attr("onclick", "utility.SelectGridRow($('#gvOOOVisit_row" + item.BillingInfoId + "'))");

                var editOOOVisitLink = '<a class="btn  btn-xs" href="#" onclick="' + EditMethod + '" title="View Out of Ofice Visit"> <i class="fa fa-credit-card blue"></i></a>';
                var editLink = '<a class="btn  btn-xs" href="#" onclick="' + EditMethod + '" title="Edit Out of Ofice Visit"> <i class="fa fa-edit black"></i></a>';
                var viewClaimLink = '<a class="btn btn-xs " href="#"onclick="' + viewClaimMethod + '"  title="View Claim"> <i class="fa fa-calculator black"></i></a>';

                var strAction = '<a class="btn  btn-xs" href="#" onclick="' + DeleteMethod + '" title="Delete Out Of Visit Detail"><i class="fa fa-close red"></i></a>' + editLink + editOOOVisitLink + viewClaimLink + '&nbsp;';

                //item.SpecialtyIds = "";// item.SpecialtyIds != null ? item.SpecialtyIds : "";
                $row.append('<td style="display:none;">' + item.BillingInfoId + '</td><td>' + strAction + '</td><td>' + utility.RemoveTimeFromDate(null, item.ENMCPTDOSFrom) + '</td><td>' + utility.RemoveDateFromDateTime(null, item.CreatedOn) + '</td><td>' + item.Provider + '</td><td>' + item.Facility + '</td><td>' + item.ICDCodes + '</td><td>' + item.CPTCodes + '</td><td>' + item.Status + '</td><td>' + item.CreatedBy + '</td>');

                $("#" + OutOfOfficeVisits.params["PanelID"] + " #pnlBillOOOVisits_Result #dgvOOOVisits tbody").last().append($row);
            });
        }
        else {
            $("#" + OutOfOfficeVisits.params["PanelID"] + " #pnlBillOOOVisits_Result #dgvOOOVisits").DataTable({
                "bDestroy": true, "bInfo": false, "bPaginate": false,

                "language": {
                    "emptyTable": "No Out Of Office Visit Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
            //Remove paging control
            $pagingControl.empty();
        }
        if ($.fn.dataTable.isDataTable("#" + OutOfOfficeVisits.params["PanelID"] + " #pnlBillOOOVisits_Result #dgvOOOVisits"))
            ;
        else
            $("#" + OutOfOfficeVisits.params["PanelID"] + " #pnlBillOOOVisits_Result #dgvOOOVisits").DataTable({ "bDestroy": true, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

        if ($('#' + OutOfOfficeVisits.params.PanelID + " #dgvOOOVisit_Paging").length > 1) {
            EMRUtility.fixDataTableDuplication("#" + OutOfOfficeVisits.params.PanelID + " #pnlBillOOOVisits_Result");
        }

        //Set ToolTip for Comments.
        // $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
    },

    //Author: Muhamamd Arshad
    //Date :  23-06-2016
    //Description: Delete the selected Physical Exam Data Template
    OOOVisitDelete: function (OutOfOfficeVisitId, status, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        if (status == 'Signed') {
            utility.DisplayMessages("Signed visit can't be deleted", 3);
        }
        else {
            utility.myConfirm('1', function () {
                var selectedValue = OutOfOfficeVisitId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    OutOfOfficeVisits.deleteOutOfOfficeVisit(OutOfOfficeVisitId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            OutOfOfficeVisits.loadOOOVisit();
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

    },


    // Method Name: bindICD9AutoComplete
    // Author: Abid Ali
    // Date: 08/01/2016
    // Description: Load ICDs
    bindICD9AutoComplete: function (element) {
        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "OutOfOfficeVisits", null, false);
    },

    //Author: Abid Ali
    //Date :  31-03-2016
    //Description: This function will handle fill  CPT Code on input
    bindAutoComplete: function (element, refCtrlId) {

        var hiddenCrtl = $('#' + OutOfOfficeVisits.params.PanelID + ' #' + (refCtrlId != null ? refCtrlId : 'txtCPTCode'));
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "OutOfOfficeVisits", hiddenCrtl, true);

    },

    //Begin Edit by Fahad Malik 13-Dec-2016, Bug# EMR-2189
    OpenSearchPopup: function (SearchType, ctrl) {
        var controlToLoad = "";
        if (SearchType == "ICD") {
            controlToLoad = "Admin_IMOICD";
        }
        else if (SearchType == "CPT") {
            controlToLoad = "Admin_IMOCPT";
        }
        var params = [];
        params["FromAdmin"] = "0";
        params["RefCtrl"] = ctrl;
        params["ParentCtrl"] = "OutOfOfficeVisits";
        LoadActionPan(controlToLoad, params);
    },
    //End Edit by Fahad Malik 13-Dec-2016, Bug# EMR-2189
    viewClaim: function (billingInfoId, VisitId, Status, event) {

        if (Status != 'Signed') {
            utility.DisplayMessages("No claim found", 3);
        }
        else {
            var params = [];
            params["FromAdmin"] = 0;
            params["ParentCtrl"] = 'billTabOutOfOfficeVisits';

            params["VisitId"] = VisitId;
            params["patientID"] = $('#PatientProfile #hfPatientId').val();

            LoadActionPan('EncounterChargeCapture', params);
        }
    },

    deleteOutOfOfficeVisit: function (billingInfoId) {

        var objData = new Object();

        objData["BillingInfoId"] = billingInfoId;
        objData["commandType"] = "billing_information_delete";
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "BillingInformation", "BillingInformation");
    },

    loadProviderFailityddl: function () {
        OutOfOfficeVisits.BindDropDownsByID($('#' + OutOfOfficeVisits.params.PanelID + ' #frmOOOVisit #ddlProvider'), 'GetOutOfOfficeVisitProvider', true, 1).done(function () {
            $("#" + ClinicalLabOrderDetail.params.PanelID + " #ddlProvider option:first").text('- Select -');
        });

        OutOfOfficeVisits.BindDropDownsByID($('#' + OutOfOfficeVisits.params.PanelID + ' #frmOOOVisit #ddlFacility'), 'GetOutOfOfficeVisitFacility', true, 1).done(function () {
            $("#" + OutOfOfficeVisits.params.PanelID + " #ddlFacility option:first").text('- Select -');
        });
    },

    BindDropDownsByID: function (Crtl, methodName, reload, Id, customAttr, selectFirstOption) {
        BackgroundLoaderShow(true);

        var data = "StrID=" + $('#PatientProfile #hfPatientId').val();
        return MDVisionService.lookups(methodName, reload, data).done(function (result) {
            result = JSON.parse(result[methodName]);
            if ($(Crtl).length > 0)
                l = $(Crtl);

            l.empty();

            $.each(result, function (j, item) {
                if (customAttr != null) {
                    var arrcustomAttr = customAttr.split(",");
                    var option = $("<option />");

                    if (arrcustomAttr[0]) {
                        option.attr(arrcustomAttr[0], item.RefValue);
                    }

                    if (arrcustomAttr[1]) {
                        option.attr(arrcustomAttr[1], item.RefName);
                    }

                    option.val(item.Value).text(item.Name);

                    l.append(option);
                }
                else
                    l.append($("<option />").val(item.Value).text(item.Name));

            });
            if (selectFirstOption != null) {
                if (selectFirstOption == true) {
                    l.find("option:eq(1)").attr("selected", "selected").trigger("change");
                }
            }
            BackgroundLoaderShow(false);
        });
    },

    //Start// Db Calls

    //Author: Muhamamd Arshad
    //Date :  23-06-2016
    //Description: DB Call to Delete the selected Physical Exam Data Template
    deleteOOOVisitDbCall: function (OutOfOfficeVisitId) {
        var objData = {};
        objData["outOfOfficeVisitId"] = OutOfOfficeVisitId;
        objData["commandType"] = "DELETE_PHYSEXAM_DATATEMPLATE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OutOfOfficeVisits", "OutOfOfficeVisits");

    },

    getAllESuperbillsDbCall: function (json, billingInfoId, pageNo, rpp) {
        var objData = JSON.parse(json);

        objData["CPTCode"] = (objData["CPT"] == "" || objData["CPT"] == null) ? "" : objData["CPTCode"];
        objData["ICDCode"] = (objData["ICD"] == "" || objData["ICD"] == null) ? "" : objData["ICDCode"];


        objData["PageNumber"] = pageNo;
        objData["RowsPerPage"] = rpp;

        objData["commandType"] = "billing_information_select_out_of_offive_visit";
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "BillingInformation", "BillingInformation");
    },
    //End// Db Calls
    UnLoad: function (Tab) {
        RemoveAdminTab(Tab);
    },
}
