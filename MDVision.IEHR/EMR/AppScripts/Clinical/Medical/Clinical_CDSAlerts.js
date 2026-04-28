Clinical_CDSAlerts = {
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
        Clinical_CDSAlerts.params = params;
        if ($("div#PatientProfile #hfPatientId").val() != "") {
            Clinical_CDSAlerts.patientId = $("div#PatientProfile #hfPatientId").val();
        }
        else {
            Clinical_CDSAlerts.patientId = Clinical_CDSAlerts.params.PatientId;
        }


        Clinical_CDSAlerts.params.mode = "Add";

        if (Clinical_CDSAlerts.params.PanelID != 'pnlClinicalCDSAlerts') {
            Clinical_CDSAlerts.params.PanelID = 'pnlClinicalCDSAlerts';
            //  Clinical_CDSAlerts.params.PanelID = Clinical_CDSAlerts.params.PanelID + ' #pnlClinicalCDSAlerts';
        } else {
            Clinical_CDSAlerts.params.PanelID = 'pnlClinicalCDSAlerts';
        }

        var self = $('#' + Clinical_CDSAlerts.params.PanelID);
        if (Clinical_CDSAlerts.bIsFirstLoad == true) {
            Clinical_CDSAlerts.CDSSearch(null, null, null);
        }
    },
    //Author: Ahmad Raza
    //Date: 06-03-2016
    //This function will handle fill of CDS Alert
    CDSSearch: function (cdsId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_CDS Alerts", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#" + Clinical_CDSAlerts.params.PanelID + " #pnlCDS_Results").css("display") == "none") {
                    $("#" + Clinical_CDSAlerts.params.PanelID + " #pnlCDS_Results").show();
                    $("#" + Clinical_CDSAlerts.params.PanelID + " #pnlCDS_Results").hide();
                }
                Clinical_CDSAlerts.searchCDS(cdsId, PageNo, rpp).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        Clinical_CDSAlerts.CDSGridLoad(response);
                        var TableControl = Clinical_CDSAlerts.params.PanelID + " #pnlCDS_Results #dgvCDSAlerts";
                        var PagingPanelControlID = Clinical_CDSAlerts.params.PanelID + " #dgvCDS_PagingAlerts";
                        var ClassControlName = "Clinical_CDSAlerts";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(
                            CreatePagination(response.CDSCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                                Clinical_CDSAlerts.CDSSearch(PrimaryID, PageNumber, ResultPerPage);
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
    searchCDS: function (CDSId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = new Object();
        objData["IsActive"] = true;
        objData["CDSId"] = CDSId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PatientId"] = Clinical_CDSAlerts.patientId;
        objData["commandType"] = "search_cds_against_patient";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CDS", "CDS");

    },
    //Author: Ahmad Raza
    //Date: 06-03-2016
    //This function will handle fill of CDS Alert Grid Load
    CDSGridLoad: function (response) {
        //Start 24-05-2016 Muhammad Arshad Remove Duplicate search issue on Datatable
        if ($.fn.dataTable.isDataTable("#" + Clinical_CDSAlerts.params.PanelID + " #pnlCDS_Results #dgvCDSAlerts")) {
            $("#" + Clinical_CDSAlerts.params.PanelID + " #pnlCDS_Results #dgvCDSAlerts").dataTable().fnClearTable();
            $("#" + Clinical_CDSAlerts.params.PanelID + " #pnlCDS_Results #dgvCDSAlerts").dataTable().fnDestroy();
            $("#" + Clinical_CDSAlerts.params.PanelID + " #pnlCDS_Results #dgvCDSAlerts tbody").find("tr").remove();
        }
        //End 24-05-2016 Muhammad Arshad Remove Duplicate search issue on Datatable


        //   $("#" + Clinical_CDSAlerts.params.PanelID + " #pnlCDS_Results #dgvCDSAlerts").dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it
        //    $("#" + Clinical_CDSAlerts.params.PanelID + " #pnlCDS_Results #dgvCDSAlerts tbody").find("tr").remove(); //Removing all the table data from table body
        if (response.CDSCount > 0) {
            var CDSLoadJSONData = JSON.parse(response.CDSLoad_JSON); //Parsing array to JSON
            $.each(CDSLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Clinical_CDSAlerts.CDSEdit('" + item.CDSId + "','" + item.CDSPatientStatusId + "',event);");
                $row.attr("id", "gvCDS_row" + item.CDSPatientStatusId);
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
                //  if (Clinical_CDSAlerts.params.TabID == "clinicalTabFaceSheet") {
                $row.append('<td style="display:none;">' + item.CDSPatientStatusId + '</td><td>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_CDSAlerts.CDSEdit(\'' + item.CDSId + '\',\'' + item.CDSPatientStatusId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a><a href="#" class="btn-xs row-history mr-none btn"onclick="Clinical_CDSAlerts.CDSStatusHistory(\'' + item.CDSPatientStatusId + '\',event);" title="Activity Log" ><i class="fa fa-history blue"></i></a></td><td>' + item.Title + '</td><td>' + item.RuleTypeDes + '</td><td>' + item.Status + '</td><td>' + item.Comments + '</td>');
                //    }
                //else {
                //    $row.append('<td style="display:none;">' + item.CDSId + '</td><td><a class="btn btn-xs" href="#" onclick="Clinical_CDSAlerts.CDSDelete(\'' + item.CDSId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_CDSAlerts.CDSEdit(\'' + item.CDSId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_CDSAlerts.CDSActiveInactive(\'' + item.CDSId + '\', ' + isactive + ',event);"><i class="' + tglclass + '"></i></a></td><td>' + item.Title + '</td><td>' + item.Comments + '</td>');
                //}


                $("#" + Clinical_CDSAlerts.params.PanelID + " #pnlCDS_Results #dgvCDSAlerts tbody").last().append($row);
            });
        }
        else {
            $("#" + Clinical_CDSAlerts.params.PanelID + ' #pnlCDS_Results #dgvCDSAlerts').DataTable({
                "language": {
                    "emptyTable": "No CDS Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_CDSAlerts.params.PanelID + ' #pnlCDS_Results #dgvCDSAlerts'))
            ;
        else {
            $("#" + Clinical_CDSAlerts.params.PanelID + " #pnlCDS_Results #dgvCDSAlerts").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        }
    },
    //Author: Ahmad Raza
    //Date: 06-03-2016
    //This function will handle Load of CDS Alert Detail Form
    CDSEdit: function (CDSId,CDSPatientStatusId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($("#" + Clinical_CDSAlerts.params.PanelID + " #pnlCDS_Results #dgvCDSAlerts #gvCDS_row" + CDSPatientStatusId));
        AppPrivileges.GetFormPrivileges("Medical_CDS Alerts", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (CDSId == "" || CDSId == "undefined" || CDSPatientStatusId == "" || CDSPatientStatusId == "undefined") {
                }
                else {
                    var params = [];
                    params["CDSId"] = CDSId;
                    params["CDSPatientStatusId"] = CDSPatientStatusId;
                    params["mode"] = "Edit";
                    params["PatientId"] = Clinical_CDSAlerts.params.PatientId;
                    params["FromAdmin"] = 0;
                    params["ForOnlyStauts"] = 1;
                    params["ParentCtrl"] = "Clinical_CDSAlerts";
                    LoadActionPan('Clinical_CDSAlertDetails', params);
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
        if (Clinical_CDSAlerts.params.ParentCtrl == "clinicalTabFaceSheet") {
            utility.UnLoadDialog(Clinical_CDSAlerts.params.PanelID + ' #frmClinicalCDS', function () {
                if (Clinical_CDSAlerts.params["FromAdmin"] == "0") {
                    if (Clinical_CDSAlerts.params != null && Clinical_CDSAlerts.params.ParentCtrl != null) {
                        UnloadActionPan(Clinical_CDSAlerts.params.ParentCtrl, 'Clinical_CDSAlerts');
                    }
                    else
                        UnloadActionPan(null, 'Clinical_CDSAlerts');
                }
                else {
                    $("#mstrDivMedical #clinicalMenu_Medical_CDS").remove();
                    RemoveAdminTab();
                }
                objDeffered.resolve();
            }, function () {
                if (Clinical_CDSAlerts.params["FromAdmin"] == "0") {
                    if (Clinical_CDSAlerts.params != null && Clinical_CDSAlerts.params.ParentCtrl != null) {
                        UnloadActionPan(Clinical_CDSAlerts.params.ParentCtrl, 'Clinical_CDSAlerts');
                    }
                    else
                        UnloadActionPan(null, 'Clinical_CDSAlerts');
                }
                else {
                    $("#mstrDivMedical #clinicalMenu_Medical_CDS").remove();
                    RemoveAdminTab();
                }
                objDeffered.resolve();
            });
        }
        else {
            if (Clinical_CDSAlerts.params["FromAdmin"] == "0") {
                if (Clinical_CDSAlerts.params != null && Clinical_CDSAlerts.params.ParentCtrl != null) {
                    UnloadActionPan(Clinical_CDSAlerts.params.ParentCtrl, 'Clinical_CDSAlerts');
                }
                else
                    UnloadActionPan(null, 'Clinical_CDSAlerts');
            }
            else {
                $("#mstrDivMedical #clinicalMenu_Medical_CDS").remove();
                RemoveAdminTab();
            }
            objDeffered.resolve();
        }
        return objDeffered;
    },
    CDSStatusHistory: function (CDSPatientStatusId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        EMRUtility.showCurrentItemHistory(Clinical_CDSAlerts.params.PanelID, null, CDSPatientStatusId, "CDSPatientStatus", Clinical_CDSAlerts.params.patientID, "Clinical_CDSAlerts", null);
    },
}