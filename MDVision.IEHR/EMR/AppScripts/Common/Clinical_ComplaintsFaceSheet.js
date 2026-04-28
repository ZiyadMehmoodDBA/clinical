Clinical_ComplaintsFaceSheet = {
    bIsFirstLoad: true,
    params: [],
    myGrid: null,
    Load: function (params) {
        Clinical_ComplaintsFaceSheet.params = params;


        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#pnlClinicalImmunization #hfPatientId").val($('#PatientProfile #hfPatientId').val());
            Clinical_ComplaintsFaceSheet.params.patientID = $('#PatientProfile #hfPatientId').val();
        }
        if (Clinical_ComplaintsFaceSheet.params.PanelID != 'pnlClinicalComplaintsFaceSheet') {
            Clinical_ComplaintsFaceSheet.params.PanelID = Clinical_ComplaintsFaceSheet.params.PanelID + ' #pnlClinicalComplaintsFaceSheet';
        } else {
            Clinical_ComplaintsFaceSheet.params.PanelID = 'pnlClinicalComplaintsFaceSheet';
        }

        if (Clinical_ComplaintsFaceSheet.params.ParentCtrl == "Clinical_FaceSheet") {
            $("#pnlClinicalImmunization #hfPatientId").val(Clinical_FaceSheet.params.patientID);
        }
        Clinical_ComplaintsFaceSheet.LoadAllComplaints();

    },

    LoadAllComplaints: function (ComplaintId, PageNo, rpp) {
        var dfd = $.Deferred();
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Medical_Immunization", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            Clinical_ComplaintsFaceSheet.SearchAllComplaints_DBCall(PageNo, rpp).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    Clinical_ComplaintsFaceSheet.ComplaintsFaceSheetGridLoad(response);
                    var TableControl = Clinical_ComplaintsFaceSheet.params.PanelID + " #dgvComplaintsFaceSheet";
                    var PagingPanelControlID = Clinical_ComplaintsFaceSheet.params.PanelID + " #dgvComplaintsFaceSheet_Paging";
                    var ClassControlName = "Clinical_ComplaintsFaceSheet";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout(CreatePagination(response.ComplaintListCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        Clinical_ComplaintsFaceSheet.LoadAllComplaints(PrimaryID, PageNumber, ResultPerPage);
                    }), 10);
                    dfd.resolve();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                    dfd.resolve();
                }
            });
        } else {
            dfd.resolve();
            utility.DisplayMessages(strMessage, 2);
        }
        //});
        return dfd;
    },
    ComplaintsFaceSheetGridLoad: function (response) {
        var dfd = $.Deferred();

        $("#" + Clinical_ComplaintsFaceSheet.params.PanelID + ' #dgvComplaintsFaceSheet tbody').empty();
        if (response.ComplaintListCount > 0) {
            var ComplaintLoad_JSON = JSON.parse(response.ComplaintListLoad_JSON);
            $.each(ComplaintLoad_JSON, function (i, item) {
                var $row;
                $row = $('<tr/>');
                if (item.IsChiefComplaint == "False") {
                    $row.append('<td>' + item.ComplaintDescription + '</td>');
                }
                else {
                    $row.append('<td>' + item.ComplaintDescription + ' (CC)</td>');
                }
                var visitDate = item.VisitDate;
                var d = new Date(visitDate);

                $row.append('<td>' + $.datepicker.formatDate('mm/dd/yy ', new Date(d)) + '</td>');
                $("#" + Clinical_ComplaintsFaceSheet.params.PanelID + ' #dgvComplaintsFaceSheet tbody').last().append($row);
            });
            dfd.resolve();
        }
        return dfd;
    },
    SearchAllComplaints_DBCall: function (PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = {};

        objData["PatientId"] = Clinical_ComplaintsFaceSheet.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : Clinical_ComplaintsFaceSheet.params.patientID;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "load_SearchAllComplaints";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "Complaint");
    },
    UnLoadTab: function () {
        var parentPanelId = null;
        if (Clinical_ComplaintsFaceSheet.params["FromAdmin"] == "0") {
            if (Clinical_ComplaintsFaceSheet.params != null && Clinical_ComplaintsFaceSheet.params.ParentCtrl != null) {
                if (Clinical_ComplaintsFaceSheet.params.ParentCtrl == "Clinical_FaceSheet") {
                    parentPanelId = GetTab(Clinical_FaceSheet.params["ParentCtrl"]).PanelID;
                    Clinical_FaceSheet.params.ChildPanelID = null;
                    UnloadActionPan(Clinical_ComplaintsFaceSheet.params.ParentCtrl, 'Clinical_ComplaintsFaceSheet', null, parentPanelId);
                } else {
                    UnloadActionPan(Clinical_ComplaintsFaceSheet.params.ParentCtrl, 'Clinical_ComplaintsFaceSheet');
                }
            } else
                UnloadActionPan(null, 'Clinical_ComplaintsFaceSheet');
        } else {
            RemoveAdminTab();
        }
        if (Clinical_Immunization.params.ParentCtrl == "clinicalTabFaceSheet") {
            Clinical_FaceSheet.loadFaceSheet();
        }

    },
}