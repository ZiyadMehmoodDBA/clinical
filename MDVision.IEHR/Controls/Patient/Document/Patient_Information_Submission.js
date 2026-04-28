Patient_Information_Submission = {
    params: [],
    Load: function (params) {
        Patient_Information_Submission.params = params;
        if (Patient_Information_Submission.params["PanelID"] != 'pnlPatientInformationSubmission')
            Patient_Information_Submission.params["PanelID"] = Patient_Information_Submission.params["PanelID"] + ' #pnlPatientInformationSubmission';
        if (Patient_Information_Submission.params.TabID == "patTabDocuments")
            utility.VerifyMUAlert("Patient Portal Document", "", Patient_Information_Submission.params.PatientId, false, "IA");
        Patient_Information_Submission.InformationSearch();
    },

    InformationSearch: function (Id, PageNumber, ResultPerPage) {              
        Patient_Information_Submission.PatientInformationSubmission(Id, PageNumber, ResultPerPage).done(function (response) {
            if (response.status != false) {
                if ($("#pnlPatientInformationSubmission #pnlPatientInformationSubmission_Result").css("display") == "none") {
                    $("#pnlPatientInformationSubmission #pnlPatientInformationSubmission_Result").show();
                }
                Patient_Information_Submission.params["PatientInformationSubmissionCount"] = response.PatientInformationSubmissionCount;
                Patient_Information_Submission.Information_SubmissionGridLoad(response);
                var TableControl = Patient_Information_Submission.params.PanelID + " #pnlPatientInformationSubmission_Result #dgvPatientInformationSubmission";
                var PagingPanelControlID = Patient_Information_Submission.params.PanelID + " #dvgPatientInformationSubmission_Paging";
                var ClassControlName = "Patient_Information_Submission";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(
                    CreatePagination(response.PatientInformationSubmissionCount, PageNumber, ResultPerPage, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (Id, PageNumber, ResultPerPage) {
                        Patient_Information_Submission.InformationSearch(Id, PageNumber, ResultPerPage);
                    }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    Information_SubmissionGridLoad: function (response) {
        $("#pnlPatientInformationSubmission_Result #dgvPatientInformationSubmission").dataTable().fnDestroy();
        $("#pnlPatientInformationSubmission_Result #dgvPatientInformationSubmission tbody").find("tr").remove();
        if (response.PatientInformationSubmissionCount > 0) {
            var PatientInformationSubmissionJSONData = JSON.parse(response.PatientInformationSubmissionLoad_JSON);
            $.each(PatientInformationSubmissionJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", "gvPatientInformationSubmission_row" + item.Id);
                $row.attr("Id", item.Id);
                $row.append('<td style="display:none;">' + item.Id + '</td><td><a class="btn  btn-xs" onclick="Patient_Information_Submission.UploadInformation(\'' + item.PatientId + '\', \'' + item.Id + '\',event);" title="Import Document"><i class="fa fa-download blue"></i></a>&nbsp;<a class="btn btn-xs" onclick="Patient_Information_Submission.ViewInformation(\'' + item.Id + '\',\'' + item.FileType + '\',\'' + encodeURIComponent(item.Url) + '\',event);"  title="View Record"><i class="fa fa-eye blue"></i></a>&nbsp;<a class="btn btn-xs" onclick="Patient_Information_Submission.DeleteInformation(\'' + item.Id + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a></td><td>' + item.FileName + '</td><td>' + item.FileType + '</td><td>' + item.CreatedOn.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + item.Comments + '</td>');

                $("#pnlPatientInformationSubmission_Result #dgvPatientInformationSubmission tbody").last().append($row);
            });
        }
        else {

            $('#pnlPatientInformationSubmission_Result #dgvPatientInformationSubmission').DataTable({
                "language": {
                    "emptyTable": "No Information Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "bPaginate": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#pnlPatientInformationSubmission_Result #dgvPatientInformationSubmission'))
            ;
        else
            $("#pnlPatientInformationSubmission_Result #dgvPatientInformationSubmission").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "bPaginate": false, "aTargets": [1] }] }); // to remove records per page dropdown


    },

    PatientInformationSubmission: function (Id, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var Status = "Pending";
        var data = "PatientID=" + Patient_Information_Submission.params.PatientId + "&status=" + Status + "&pageNumber=" + PageNumber + "&rowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "SEARCH_PATIENT_INFORMATION_SUBMISSION");
    },

    DeleteInformation: function (Id, event) {
        if (event) {
            event.stopPropagation();
        }
        var params = [];
        params["Id"] = Id;
        params["ParentCtrl"] = 'Patient_Information_Submission';
        LoadActionPan('Patient_Information_Delete', params);

    },

    UploadInformation: function (PatientId, Id, event) {
        if (event) {
            event.stopPropagation();
        }
        var params = [];
        params["PatPortalDocId"] = Id;
        params["ParentCtrl"] = 'Patient_Information_Submission';
        params["PatientId"] = PatientId;
        params["FolderId"] = Patient_Information_Submission.params.FolderId;
        LoadActionPan('Patient_Information_Import', params);

    },

    ViewInformation: function (Id, FileType, Url, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($("dgvPatientInformationSubmission_row" + Id));
        AppPrivileges.GetFormPrivileges("Documents", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ParentCtrl"] = 'Patient_Information_Submission';
                params["FileType"] = FileType;
                params["Url"] = decodeURI(Url);

                LoadActionPan('Document_Viewer', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },


    UnLoad: function () {
        if (Patient_Information_Submission.params && Patient_Information_Submission.params.ParentCtrl) {
            UnloadActionPan(Patient_Information_Submission.params.ParentCtrl, 'Patient_Information_Submission');
        }
        else
            UnloadActionPan(null, 'Patient_Information_Submission');
    },
}