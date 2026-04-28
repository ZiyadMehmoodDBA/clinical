Patient_Case_Document = {
    Load: function (paramerters) {
        
        Patient_Case_Document.params = paramerters;
        if (Patient_Case_Document.params.PanelID != null && Patient_Case_Document.params.PanelID != 'pnlCaseDocument') {
            Patient_Case_Document.params["PanelID"] = Patient_Case_Document.params["PanelID"] + ' #pnlCaseDocument';
        }
        else {
            Patient_Case_Document.params["PanelID"] = 'pnlCaseDocument';
        }
        if (Patient_Case_Document.bIsFirstLoad) {
            Patient_Case_Document.bIsFirstLoad = false;
            var self = $('#' + Patient_Case_Document.params["PanelID"]);
            self.loadDropDowns(true);
        }
        Patient_Case_Document.CaseDocumentsSearch();
    },
    CaseDocumentsSearch: function () {
        if (Patient_Case_Document.params["CaseId"] && Patient_Case_Document.params["CaseId"] > 0) {
           
            Patient_Case_Document.SearchCaseDocuments(Patient_Case_Document.params["CaseId"]).done(function (response) {
                if (response.status != false) {
                    Patient_Case_Document.DocumentGridLoad(response);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    DocumentGridLoad: function (response) {
        $('#' + Patient_Case_Document.params.PanelID + ' #ContainerCaseDocuments #dgvCaseDocuments').dataTable().fnDestroy();

        $('#' + Patient_Case_Document.params.PanelID + ' #ContainerCaseDocuments #dgvCaseDocuments tbody').find("tr").remove();

        if (response.ClaimChargeDocumentCount > 0) {
            var DocumentLoad_JSONData = JSON.parse(response.ClaimChargeDocumentLoad_JSON);
            $.each(DocumentLoad_JSONData, function (i, item) {
                var DemographicsMethod = "utility.PatientDemographics('" + item.PatientId + "', 'Patient_Case_Document', event);";
                var VisitDetail = "utility.LoadVisitDetail('" + item.VisitId + "', '" + item.PatientId + "', 'Patient_Case_Document', event, true);";
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#dgvClaimDocuments_row" + item.BatchDocId + "'))");
                $row.attr("id", "dgvCaseDocuments_row" + item.BatchDocId);
                $row.attr("BatchDocId", item.BatchDocId);
                $row.attr("BatchId", item.BatchId);

                if (item.BatchId) {
                    var action = '<td><a class="btn  btn-xs" href="#" onclick="Patient_Case_Document.BatchChargeDocumentDelete(' + item.BatchDocId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Patient_Case_Document.BatchChargeDocumentEdit(' + item.BatchDocId + ',' + item.BatchId + ',' + item.BatchNumber + ' );"   title="Edit Record"><i class="fa fa-edit black"></i></a></td>';
                    $row.append('' + action + '<td patientId="' + item.PatientId + '"><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.AccountNumber + '</a></td><td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td><td class="ellip150" data-toggle="tooltip" data-placement="left" title=' + item.FilePath + ' >' + item.FilePath + '</td><td>' + item.FileType + '</td><td>' + item.Pages + '</td><td>' + item.ActionName + '</td><td>' + item.ReasonName + '</td><td class="ellip150" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '">' + item.Comments + '</td>');
                }
                else {
                    var action = '<td><a class="btn  btn-xs" href="#" onclick="Patient_Case_Document.PatientDocumentDelete(' + item.BatchDocId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Patient_Case_Document.PatientDocumentEdit(' + item.PatientId + ',' + item.BatchDocId + ',event );"   title="Edit Record"><i class="fa fa-edit black"></i></a></td>';
                    $row.append('' + action + '<td patientId="' + item.PatientId + '"><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.AccountNumber + '</a></td><td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td><td class="ellip150" data-toggle="tooltip" data-placement="left" title=' + item.FilePath + ' >' + item.FilePath + '</td><td>' + item.FileType + '</td><td>' + item.Pages + '</td><td>' + item.ActionName + '</td><td>' + item.ReasonName + '</td><td class="ellip150" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '">' + item.Comments + '</td>');
                }


                $('#' + Patient_Case_Document.params.PanelID + ' #ContainerCaseDocuments #dgvCaseDocuments tbody').last().append($row);

            });
        }
        else {
            $('#' + Patient_Case_Document.params.PanelID + ' #ContainerCaseDocuments #dgvCaseDocuments').DataTable({
                "language": {
                    "emptyTable": "No Documents Found for this Claim "
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });


        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });

        if ($.fn.dataTable.isDataTable('#' + Patient_Case_Document.params.PanelID + ' #ContainerCaseDocuments #dgvCaseDocuments'));
        else
            $('#' + Patient_Case_Document.params.PanelID + ' #ContainerCaseDocuments #dgvCaseDocuments').DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

    },
    BatchChargeDocumentEdit: function (BatChDocId, BatchId, BatchNumber) {
        AppPrivileges.GetFormPrivileges("Charge Batch", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["BatchDocId"] = BatChDocId;
                params["BatchId"] = BatchId;
                params["BatchNumber"] = BatchNumber;
                params["ParentCtrl"] = "Patient_Case_Document";
                LoadActionPan('ChargeBatch_Viewer', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    BatchChargeDocumentDelete: function (BatChDocId) {
        utility.myConfirm('1', function () {
            var selectedValue = BatChDocId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {

                AppPrivileges.GetFormPrivileges("Charge Batch", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        chargeBatchDetail.DeleteBatchChargeDocument(BatChDocId).done(function (response) {
                            if (response.status == true) {
                                Patient_Case_Document.CaseDocumentsSearch();
                                utility.DisplayMessages(response.Message, 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }

                        });
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);

                });
            }
        }, function () { });
    },
    PatientDocumentDelete: function (BatChDocId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.myConfirm('1', function () {
            var selectedValue = BatChDocId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                AppPrivileges.GetFormPrivileges("Charge Batch", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        Patient_Case_Document.DeletePatientDocument(BatChDocId).done(function (response) {
                            if (response.status == true) {
                               
                                Patient_Case_Document.CaseDocumentsSearch();
                                utility.DisplayMessages(response.Message, 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }

                        });
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);

                });
            }
        }, function () { });
    },
    PatientDocumentEdit: function (PatientID, PatDocID, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Documents", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["PatientID"] = PatientID;
                params["PatDocID"] = PatDocID;
                params["mode"] = "Edit";
                params["FromAdmin"] = 0;
                params["ParentCtrl"] = "Patient_Case_Document";

                LoadActionPan('Document_Viewer', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    DeletePatientDocument: function (DocumentID) {
        var data = "DocumentID=" + DocumentID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "DELETE_PATIENT_DOCUMENT");
    },
    SearchCaseDocuments: function (CaseId) {
        var data = "CaseID=" + CaseId;
        return MDVisionService.defaultService(data, "PATIENT_CASE", "DOCUMENT_DETAIL");
    },
    UnLoadTab: function () {
        UnloadActionPan(Patient_Case_Document.params.ParentCtrl, 'Patient_Case_Document'); 
    }
}