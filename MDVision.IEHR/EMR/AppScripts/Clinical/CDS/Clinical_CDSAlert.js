Clinical_CDSAlert = {
    //Author: Ahmad Raza
    //Date: 06-03-2016
    //This file will handle all actions performed for CDS Alert
    bIsFirstLoad: true,
    params: [],
    EditableGrid: null,
    myGrid: null,
    patientId: null,

    //Author: Ahmad Raza
    //Date: 06-03-2016
    //This function will handle load of CDS Alert
    Load: function (params) {
        Clinical_CDSAlert.params = params;
        // check for cds alert dialoag without banner patient
        if (Clinical_CDSAlert.params && Clinical_CDSAlert.params.TriggerLocation && Clinical_CDSAlert.params.TriggerLocation == "FromBulkSign") {
            Clinical_CDSAlert.patientId = Clinical_CDSAlert.params.PatientId;
        } else {
            if ($("div#PatientProfile #hfPatientId").val() != "") {
                Clinical_CDSAlert.patientId = $("div#PatientProfile #hfPatientId").val();
            }
            else {
                Clinical_CDSAlert.patientId = Clinical_CDSAlert.params.PatientId;
            }
        }


        Clinical_CDSAlert.params.mode = "Add";

        if (Clinical_CDSAlert.params.PanelID != 'pnlClinicalCDSAlert') {
            Clinical_CDSAlert.params.PanelID = 'pnlClinicalCDSAlert';
            //  Clinical_CDSAlert.params.PanelID = Clinical_CDSAlert.params.PanelID + ' #pnlClinicalCDSAlert';
        } else {
            Clinical_CDSAlert.params.PanelID = 'pnlClinicalCDSAlert';
        }

        var self = $('#' + Clinical_CDSAlert.params.PanelID);
        if (Clinical_CDSAlert.bIsFirstLoad == true) {
            // check for cds alert dialoag without banner patient
            if (Clinical_CDSAlert.params && Clinical_CDSAlert.params.TriggerLocation && Clinical_CDSAlert.params.TriggerLocation == "FromBulkSign") {
                Clinical_CDSAlert.CDSSearch(null, null, null, Clinical_CDSAlert.params.CDSIds, 'Yes');
            } else {
                Clinical_CDSAlert.CDSSearch(null, null, null, $(" #mainForm  li#CDSAlert input").val(), 'Yes');
            }
        }
    },
    //Author: Ahmad Raza
    //Date: 06-03-2016
    //This function will handle fill of CDS Alert
    CDSSearch: function (cdsId, PageNo, rpp, CDSIDs, isPopup) {
        var strMessage = "";


        AppPrivileges.GetFormPrivileges("ClinicalDecisionSupport_Clinical Decision Support", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#" + Clinical_CDSAlert.params.PanelID + " #pnlCDS_Result").css("display") == "none") {
                    $("#" + Clinical_CDSAlert.params.PanelID + " #pnlCDS_Result").show();
                    $("#" + Clinical_CDSAlert.params.PanelID + " #pnlCDS_Result").hide();
                }
                Clinical_CDSAlert.searchCDS(cdsId, PageNo, rpp, CDSIDs, isPopup).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        Clinical_CDSAlert.CDSGridLoad(response);
                        var TableControl = Clinical_CDSAlert.params.PanelID + " #pnlCDS_Result #dgvCDSAlert";
                        var PagingPanelControlID = Clinical_CDSAlert.params.PanelID + " #dgvCDS_Paging";
                        var ClassControlName = "Clinical_CDSAlert";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(
                            CreatePagination(response.CDSCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                                Clinical_CDSAlert.CDSSearch(PrimaryID, PageNumber, ResultPerPage);
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
    //Author: Ahmad Raza
    //Date: 06-03-2016
    //This function will call DB To fill CDS Alert
    searchCDS: function (CDSId, PageNumber, RowsPerPage, CDSIDs, isPopup) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 2000;
        }
        var objData = new Object();
        objData["IsActive"] = true;
        objData["CDSId"] = CDSId;
        objData["CDSIDs"] = CDSIDs;
        objData["isPopup"] = isPopup;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PatientId"] = Clinical_CDSAlert.patientId;
        objData["commandType"] = "load_cds_alerts";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CDS", "CDS");

    },
    //Author: Ahmad Raza
    //Date: 06-03-2016
    //This function will handle fill of CDS Alert Grid Load
    CDSGridLoad: function (response) {
        //Start 24-05-2016 Muhammad Arshad Remove Duplicate search issue on Datatable
        if ($.fn.dataTable.isDataTable("#" + Clinical_CDSAlert.params.PanelID + " #pnlCDS_Result #dgvCDSAlert")) {
            $("#" + Clinical_CDSAlert.params.PanelID + " #pnlCDS_Result #dgvCDSAlert").dataTable().fnClearTable();
            $("#" + Clinical_CDSAlert.params.PanelID + " #pnlCDS_Result #dgvCDSAlert").dataTable().fnDestroy();
            $("#" + Clinical_CDSAlert.params.PanelID + " #pnlCDS_Result #dgvCDSAlert tbody").find("tr").remove();
        }
        //End 24-05-2016 Muhammad Arshad Remove Duplicate search issue on Datatable


        //   $("#" + Clinical_CDSAlert.params.PanelID + " #pnlCDS_Result #dgvCDSAlert").dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it
        //    $("#" + Clinical_CDSAlert.params.PanelID + " #pnlCDS_Result #dgvCDSAlert tbody").find("tr").remove(); //Removing all the table data from table body
        if (response.CDSCount > 0) {
            var CDSLoadJSONData = JSON.parse(response.CDSLoad_JSON); //Parsing array to JSON
            $.each(CDSLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Clinical_CDSAlert.CDSEdit('" + item.CDSId + "','" + item.CDSPatientStatusId + "',event);");
                $row.attr("id", "gvCDS_row" + item.CDSId);
                $row.attr("CDSId", item.CDSId);
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
                //  if (Clinical_CDSAlert.params.TabID == "clinicalTabFaceSheet") {
                $row.append('<td style="display:none;">' + item.CDSId + '</td><td>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_CDSAlert.CDSEdit(\'' + item.CDSId + '\',\'' + item.CDSPatientStatusId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a></td><td>' + item.Title + '</td><td>' + item.Comments + '</td>');
                //    }
                //else {
                //    $row.append('<td style="display:none;">' + item.CDSId + '</td><td><a class="btn btn-xs" href="#" onclick="Clinical_CDSAlert.CDSDelete(\'' + item.CDSId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_CDSAlert.CDSEdit(\'' + item.CDSId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_CDSAlert.CDSActiveInactive(\'' + item.CDSId + '\', ' + isactive + ',event);"><i class="' + tglclass + '"></i></a></td><td>' + item.Title + '</td><td>' + item.Comments + '</td>');
                //}


                $("#" + Clinical_CDSAlert.params.PanelID + " #pnlCDS_Result #dgvCDSAlert tbody").last().append($row);
            });
        }
        else {
            $("#" + Clinical_CDSAlert.params.PanelID + ' #pnlCDS_Result #dgvCDSAlert').DataTable({
                "language": {
                    "emptyTable": "No CDS Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_CDSAlert.params.PanelID + ' #pnlCDS_Result #dgvCDSAlert'))
            ;
        else {
            $("#" + Clinical_CDSAlert.params.PanelID + " #pnlCDS_Result #dgvCDSAlert").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        }
    },
    //Author: Ahmad Raza
    //Date: 06-03-2016
    //This function will handle Load of CDS Alert Detail Form
    CDSEdit: function (CDSId, CDSPatientStatusId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($("#" + Clinical_CDSAlert.params.PanelID + " #pnlCDS_Result #dgvCDSAlert #gvCDS_row" + CDSId));
        AppPrivileges.GetFormPrivileges("ClinicalDecisionSupport_Clinical Decision Support", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = CDSId;

                if (CDSId == "" || CDSId == "undefined" || CDSPatientStatusId == "" || CDSPatientStatusId == "undefined") {
                }
                else {
                    var params = [];
                    params["CDSId"] = CDSId;
                    params["mode"] = "Edit";
                    params["PatientId"] = Clinical_CDSAlert.params.PatientId;
                    params["CDSPatientStatusId"] = CDSPatientStatusId;
                    params["FromAdmin"] = 0;
                    params["ParentCtrl"] = "Clinical_CDSAlert";
                    LoadActionPan('Clinical_CDSAlertDetail', params);
                }


            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    //Author: Ahmad Raza
    //Date: 06-03-2016
    //This function will handle UnLoad of CDS Alert Form
    UnLoadTab: function () {
        var objDeffered = $.Deferred();
        if (Clinical_CDSAlert.params.ParentCtrl == "clinicalTabFaceSheet") {
            utility.UnLoadDialog(Clinical_CDSAlert.params.PanelID + ' #frmClinicalCDS', function () {
                if (Clinical_CDSAlert.params["FromAdmin"] == "0") {
                    if (Clinical_CDSAlert.params != null && Clinical_CDSAlert.params.ParentCtrl != null) {
                        UnloadActionPan(Clinical_CDSAlert.params.ParentCtrl, 'Clinical_CDSAlert');
                    }
                    else
                        UnloadActionPan(null, 'Clinical_CDSAlert');
                }
                else {
                    $("#mstrDivMedical #clinicalMenu_Medical_CDS").remove();
                    RemoveAdminTab();
                }
                objDeffered.resolve();
            }, function () {
                if (Clinical_CDSAlert.params["FromAdmin"] == "0") {
                    if (Clinical_CDSAlert.params != null && Clinical_CDSAlert.params.ParentCtrl != null) {
                        UnloadActionPan(Clinical_CDSAlert.params.ParentCtrl, 'Clinical_CDSAlert');
                    }
                    else
                        UnloadActionPan(null, 'Clinical_CDSAlert');
                }
                else {
                    $("#mstrDivMedical #clinicalMenu_Medical_CDS").remove();
                    RemoveAdminTab();
                }
                objDeffered.resolve();
            });
        }
        else {
            if (Clinical_CDSAlert.params["FromAdmin"] == "0") {
                if (Clinical_CDSAlert.params != null && Clinical_CDSAlert.params.ParentCtrl != null) {
                    UnloadActionPan(Clinical_CDSAlert.params.ParentCtrl, 'Clinical_CDSAlert');
                }
                else
                    UnloadActionPan(null, 'Clinical_CDSAlert');
            }
            else {
                $("#mstrDivMedical #clinicalMenu_Medical_CDS").remove();
                RemoveAdminTab();
            }
            objDeffered.resolve();
        }
        // refresh Missing info tab on cds alert dialog close
        if (Clinical_CDSAlert.params && Clinical_CDSAlert.params.TriggerLocation && Clinical_CDSAlert.params.TriggerLocation == "FromBulkSign") {
            Clinical_CDSAlert.params.TriggerLocation = "";
            DashBoard.DashBoardEncounterSearchMissingInfo(DashBoard.pageNoMissingInfo, null, null);
            if (Clinical_CDSAlert.params.PatientId == $("div#PatientProfile #hfPatientId").val()) {
                ClinicalCDSDetail.showCDSAlert("", Clinical_CDSAlert.params.PatientId);
            }
        }
        return objDeffered;
    },
}