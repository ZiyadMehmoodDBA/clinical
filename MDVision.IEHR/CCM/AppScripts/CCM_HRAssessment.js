CCM_HRAssessment = {
    IsHRAssessmentFirstLoad: true,
    HRAssessmentLoad: function () {
        if (CCM_HRAssessment.IsHRAssessmentFirstLoad) {
            CCM_HRAssessment.IsHRAssessmentFirstLoad = false;
            CCM_HRAssessment.searchHRAssessment();
        }
    },

    searchHRAssessment: function (HRAssessmentId, PageNo, rpp) {
        if ($("#" + CCM_Patient_Hub.params.PanelID + " #pnlHRAssessment_Result").css("display") == "none") {
            $("#" + CCM_Patient_Hub.params.PanelID + " #pnlHRAssessment_Result").css("display", 'block');
        }

        CCM_HRAssessment.searchHRAssessment_DBCall(HRAssessmentId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                CCM_HRAssessment.HRAssessmentGridLoad(response);
                var TableControl = CCM_Patient_Hub.params.PanelID + " #dgvHRAssessment";
                var PagingPanelControlID = CCM_Patient_Hub.params.PanelID + " #divHRAssessment_Paging";
                var ClassControlName = "CCM_HRAssessment";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.AssessmentCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords,
                    function (PrimaryID, PageNumber, ResultPerPage) {
                        CCM_HRAssessment.searchHRAssessment(PrimaryID, PageNumber, ResultPerPage);
                    }), 10);

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    HRAssessmentDelete: function (HRAssessmentId, event) {
        if (event != null) { 
            event.stopPropagation();
        }
        if (HRAssessmentId > 0 && HRAssessmentId > 0) {
            utility.myConfirm('1', function () {
                CCM_HRAssessment.HRAssessmentDelete_DbCall(HRAssessmentId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        CCM_HRAssessment.searchHRAssessment();
                        utility.DisplayMessages(response.Message, 1);
                        CCM_Patient_Hub.DeletePatientHubRiskAssessmentScoreTemplate(HRAssessmentId);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });

            });
        }
    },

    HRAssessmentEditRow: function (HRAssessmentId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        if ((event.srcElement instanceof HTMLAnchorElement || event.srcElement.nodeName.toLowerCase() == 'i') != true) {
            CCM_HRAssessment.HRAssessmentEdit(HRAssessmentId, event);
        }
    },

    addHRAssessment: function () {
        var params = [];
        params["ParentCtrl"] = "CCM_Patient_Hub";
        params["PatientId"] = CCM_Patient_Hub.params.PatientId;
        params["EnrollmentInfoId"] = CCM_Patient_Hub.params.EnrollmentInfoId;
        params["mode"] = "Add";
        params["FromAdmin"] = "0";
        LoadActionPan('CCM_HRAssessmentDetail', params);
    },
    HRAssessmentEdit: function (HRAssessmentId, event) {
        var params = [];
        params["ParentCtrl"] = "CCM_Patient_Hub";
        params["FromAdmin"] = "0";
        params["HRAssessmentId"] = HRAssessmentId;
        params["PatientId"] = CCM_Patient_Hub.params.PatientId;
        params["EnrollmentInfoId"] = CCM_Patient_Hub.params.EnrollmentInfoId;
        params["mode"] = "Edit";
        LoadActionPan('CCM_HRAssessmentDetail', params);
    },

    HRAssessmentActiveInactive: function (HRAssessmentId, IsActive, event) {
        utility.myConfirm('3', function () {
            if (HRAssessmentId == "" || HRAssessmentId == "undefined") {
            }
            else {
                CCM_HRAssessment.HRAssessmentActiveInactive_Dbcall(HRAssessmentId, IsActive).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {

                        utility.DisplayMessages(response.Message, 1);
                        CCM_HRAssessment.searchHRAssessment();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { },
                             '3', null, null, null, IsActive
                        );
    },

    ActiveHRAssessmentearch: function (objThis) {
        var isactive = $(objThis).attr('isactive');

        if (isactive == '1') {
            $(objThis).attr('isactive', '0');
        }
        else if (isactive == '0') {
            $(objThis).attr('isactive', '1');
        }

        CCM_HRAssessment.searchHRAssessment();
    },

    HRAssessmentGridLoad: function (response) {
        var isactive = $('#' + CCM_Patient_Hub.params.PanelID + ' #pnlHRAssessment_Result #divSwitch #switchActive').attr('isactive');
        $("#" + CCM_Patient_Hub.params.PanelID + " #dgvHRAssessment").dataTable().fnDestroy();
        $("#" + CCM_Patient_Hub.params.PanelID + " #dgvHRAssessment tbody").find("tr").remove();
        if (response.AssessmentCount > 0) {
            var HRAssessmentJSONData = JSON.parse(response.AssessmentList_JSON);
            $.each(HRAssessmentJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvHRAssessment_row" + item.HRAssessmentId + "'));CCM_HRAssessment.HRAssessmentEditRow(" + item.HRAssessmentId + ", event);");
                $row.attr("HRAssessmentId", item.HRAssessmentId);
                $row.attr("PatientId", item.PatientId);
                if (item.IsActive) {
                    isactive = 1;
                    isEventactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 0;
                    isEventactive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }

                $row.append('<td><a class="btn btn-xs" href="javascript:void(0);" onclick="CCM_HRAssessment.HRAssessmentDelete(' + item.HRAssessmentId + ', event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;' +
                                '<a class="btn btn-xs" href="javascript:void(0);" onclick="CCM_HRAssessment.HRAssessmentEdit(' + item.HRAssessmentId + ', event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;' +
                                '<a class="btn  btn-xs" href="javascript:void(0);" onclick="CCM_HRAssessment.HRAssessmentActiveInactive(' + item.HRAssessmentId + "," + isEventactive + ', event);" title="' + activeTitle +
                                '"><i class="' + tglclass + '"></i></a></td>' +
                    '<td>' + item.Name + '</td><td>' + item.Description + '</td><td>' + item.RiskScore + '</td><td>' + item.CreatedByName + '</td><td>' + item.ModifiedOn + ' by ' + item.ModifiedByName + '</td>');
                $("#" + CCM_Patient_Hub.params.PanelID + " #dgvHRAssessment tbody").last().append($row);
            });

        } else {

            $("#" + CCM_Patient_Hub.params.PanelID + " #dgvHRAssessment").DataTable({
                "language": {
                    "emptyTable": "No Health Risk Assessment is Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            });
        }

        if ($.fn.dataTable.isDataTable("#" + CCM_Patient_Hub.params.PanelID + " #dgvHRAssessment"))
            ;
        else {
            $("#" + CCM_Patient_Hub.params.PanelID + " #dgvHRAssessment").DataTable({
                "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [],
                "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            }); // to remove records per page dropdown

        }
        var checked = '';
        if (isactive == "0") {
        } else if (isactive == null) {
            isactive = "1";
            checked = 'checked="checked"';
        } else {
            isactive = "1";
            checked = 'checked="checked"';
        }
        var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                        '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="CCM_HRAssessment.ActiveHRAssessmentearch(this);">' +
                         '</div><span class="pl-xs">Active</span>';

        $("#" + CCM_Patient_Hub.params.PanelID + ' #pnlHRAssessment_Result .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        EMRUtility.SwicthWidgetInializatoin();
    },

    HRAssessmentActiveInactive_Dbcall: function (HRAssessmentId, IsActive) {
        var objData = {};
        objData["HRAssessmentId"] = HRAssessmentId;
        objData["IsActive"] = IsActive;

        objData["commandType"] = "UPDATE_HR_ASSESSMENT_ACTIVE_INACTIVE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Patient_Hub", "HRAssessment");
    },

    searchHRAssessment_DBCall: function (HRAssessmentId, PageNumber, RowsPerPage) {
        var IsActive = null;
        IsActive = $('#' + CCM_Patient_Hub.params.PanelID + ' #pnlHRAssessment_Result #divSwitch #switchActive').attr('isactive');
        if (IsActive == null) {
            IsActive = "1";
        }
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = new Object();
        objData["PatientId"] = CCM_Patient_Hub.params.PatientId;
        objData["IsActive"] = IsActive;
        objData["HRAssessmentId"] = HRAssessmentId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "search_hrassessment_list";
        objData["EnrollmentInfoId"] = CCM_Patient_Hub.params.EnrollmentInfoId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Patient_Hub", "HRAssessment");
    },

    HRAssessmentDelete_DbCall: function (HRAssessmentId) {
        var objData = {};
        objData["HRAssessmentId"] = HRAssessmentId;
        objData["commandType"] = "DELETE_CCM_HR_ASSESSMENT";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class 
        return MDVisionService.APIService(data, "Patient_Hub", "HRAssessment");
    }
}