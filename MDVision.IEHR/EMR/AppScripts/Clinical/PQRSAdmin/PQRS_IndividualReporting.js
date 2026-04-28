PQRS_IndividualReporting = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        PQRS_IndividualReporting.params = params;

        if (PQRS_IndividualReporting.params.PanelID != 'pnlPQRSIndividualReporting') {
            PQRS_IndividualReporting.params.PanelID = PQRS_IndividualReporting.params.PanelID + ' #pnlPQRSIndividualReporting';
        } else {
            PQRS_IndividualReporting.params.PanelID = 'pnlPQRSIndividualReporting';
        }
        var self = $('#' + PQRS_IndividualReporting.params.PanelID+' #frmPQRSIndividualReporting');
        self.loadDropDowns(true).done(function () {
        });
        PQRS_IndividualReporting.measureIndividualSearch();
    },
    measureIndividualSearch: function (measureIndividualId, PageNo, rpp) {
        if ($('#' + PQRS_IndividualReporting.params.PanelID + ' #pnlIndividualReporting_Result').css("display") == "none") {
            $('#' + PQRS_IndividualReporting.params.PanelID + ' #pnlIndividualReporting_Result').show();
        }

        var self = $('#' + PQRS_IndividualReporting.params.PanelID + ' form');
        var myJSON = self.getMyJSONByName();

        PQRS_IndividualReporting.SearchMeasureIndividual_DBCall(myJSON, measureIndividualId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                PQRS_IndividualReporting.measureIndividualGridLoad(response);

                var TableControl = PQRS_IndividualReporting.params.PanelID + '  #dgvIndividualReporting';
                var PagingPanelControlID = PQRS_IndividualReporting.params.PanelID + ' #dgvIndividualReporting_Paging';
                var ClassControlName = "PQRS_IndividualReporting";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.individualReportingCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                    PQRS_IndividualReporting.measureIndividualSearch(PrimaryID, PageNumber, ResultPerPage);
                }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    measureIndividualGridLoad: function (response) {
        var isactive = $('#' + PQRS_IndividualReporting.params.PanelID + ' #pnlIndividualReporting_Result #divSwitch #switchActive').attr('isactive');
        if ($.fn.dataTable.isDataTable('#' + PQRS_IndividualReporting.params.PanelID + " #dgvIndividualReporting")) {
            $("#" + PQRS_IndividualReporting.params.PanelID + " #dgvIndividualReporting").dataTable().fnClearTable();
            $('#' + PQRS_IndividualReporting.params.PanelID + " #dgvIndividualReporting").dataTable().fnDestroy();
            $('#' + PQRS_IndividualReporting.params.PanelID + " #dgvIndividualReporting tbody").find("tr").remove();
        }
        if (response.individualReportingCount > 0) {
            var IndividualReportingLoadJSONData = JSON.parse(response.IndividualReportingLoad_JSON);
            $.each(IndividualReportingLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvIndividualReporting_row" + item.measureIndividualId + "'))");
                $row.attr("id", "gvIndividualReporting_row" + item.measureIndividualId);
                $row.attr("MeasureIndividualId", item.measureIndividualId);
                if (item.IsActive) {
                    isactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }

                var Isdisabled = "disabled =true";
                if (!item.IsReported) {
                    Isdisabled = "";
                }
                $row.append('<td><a ' + Isdisabled
                    + ' class="btn  btn-xs" href="#" onclick="PQRS_IndividualReporting.IndividualReportingDelete(' + item.measureIndividualId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a ' + Isdisabled
                    + ' class="btn btn-xs" href="javascript:void(0);" onclick="PQRS_IndividualReporting.IndividualReportingEdit(' + item.measureIndividualId + ",'Edit'" + ', event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" ' + Isdisabled
                    + ' href="#" onclick="PQRS_IndividualReporting.IndividualReportingActiveInactive(' + item.measureIndividualId + "," + isactive + ');" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td>' +
                    '<td>' + item.SubmissionYear + '</td><td>' + item.providerName + '</td><td>' + item.specialityName + '</td><td>' + utility.RemoveTimeFromDate(null, item.createdOn) + '</td>');
                $('#' + PQRS_IndividualReporting.params.PanelID + ' #pnlIndividualReporting_Result #dgvIndividualReporting tbody').last().append($row);
            });
        }
        else {
            $('#' + PQRS_IndividualReporting.params.PanelID + ' #pnlIndividualReporting_Result #dgvIndividualReporting').DataTable({
                "language": {
                    "emptyTable": "No Record  is Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        var checked = '';
        var isactive = $('#' + PQRS_IndividualReporting.params.PanelID + ' #ddlActive').val();
        if (isactive == "0" || isactive == 0) {
            isactive = "0";
        } else if (isactive == null || isactive == '') {
            isactive = "1";
            checked = 'checked="checked"';
        } else {
            isactive = "1";
            checked = 'checked="checked"';
        }

        if ($.fn.dataTable.isDataTable('#' + PQRS_IndividualReporting.params.PanelID + ' #pnlIndividualReporting_Result #dgvIndividualReporting'))
            ;
        else {
            $('#' + PQRS_IndividualReporting.params.PanelID + ' #pnlIndividualReporting_Result #dgvIndividualReporting').DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

        }
        var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                             '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="PQRS_IndividualReporting.activeMeasureIndividualSearch(this);">' +
                              '</div><span class="pl-xs">Active</span>';

        $("#" + PQRS_IndividualReporting.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        EMRUtility.SwicthWidgetInializatoin();
    },
    activeMeasureIndividualSearch: function (objThis) {
        var isactive = $(objThis).attr('isactive');
        if (isactive == '1') {
            $(objThis).attr('isactive', '0');
            isactive = 0;
        }
        else if (isactive == '0') {
            $(objThis).attr('isactive', '1');
            isactive = 1;
        }
        $('#' + PQRS_IndividualReporting.params.PanelID + ' #ddlActive').val(isactive);
        PQRS_IndividualReporting.measureIndividualSearch();
    },
    IndividualReportingActiveInactive: function (MeasureIndividualId, IsActive, event) {
        utility.myConfirm('3', function () {
            if (MeasureIndividualId == "" || MeasureIndividualId == "undefined") {
            }
            else {
                PQRS_IndividualReporting.updateMeasureIndividualActiveInactive_Dbcall(MeasureIndividualId, IsActive).done(function (response) {
                    if (response.status != false) {
                        var response = JSON.parse(response);
                        utility.DisplayMessages(response.Message, 1);
                        PQRS_IndividualReporting.measureIndividualSearch();
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

    IndividualReportingEdit: function (MeasureIndividualId, mode, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        //Start//22/12/2015//Ahmad Raza//Privileges Logic Implemented
        AppPrivileges.GetFormPrivileges("Clinical_PQRS_Individual Reporting", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["MeasureIndividualId"] = MeasureIndividualId;
                params["mode"] = mode;
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = PQRS_IndividualReporting.params["TabID"];
                LoadActionPan("PQRS_IndividualReporting_Detail", params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        //End//22/12/2015//Ahmad Raza//Privileges Logic Implemented
    },
    IndividualReportingDelete: function (MeasureIndividualId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }

        utility.myConfirm('1', function () {
            $('body').removeClass('modal-open');
            if (MeasureIndividualId == "" || MeasureIndividualId == "undefined") {
            }
            else {
                PQRS_IndividualReporting.MeasureIndividualDeleted_DbCall(MeasureIndividualId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        PQRS_IndividualReporting.measureIndividualSearch();
                        utility.DisplayMessages(response.Message, 1);
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
    SearchMeasureIndividual_DBCall: function (measureIndividualData, MeasureIndividualId, PageNumber, RowsPerPage) {
        
  
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = JSON.parse(measureIndividualData);
        objData["MeasureIndividualId"] = MeasureIndividualId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "SEARCH_PQRS_IndividualReporting";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PQRS", "PQRSAdmin_IndividualReporting");
    },
    updateMeasureIndividualActiveInactive_Dbcall: function (MeasureIndividualId, IsActive) {
        var objData = {};
        objData["MeasureIndividualId"] = MeasureIndividualId;
        objData["IsActive"] = IsActive;

        objData["commandType"] = "UPDATE_PQRS_MEASUREINDIVIDUAL_ACTIVE_INACTIVE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PQRS", "PQRSAdmin_IndividualReporting");
    },
    MeasureIndividualDeleted_DbCall: function (MeasureIndividualId) {
        var objData = {};
        objData["MeasureIndividualId"] = MeasureIndividualId;
        objData["commandType"] = "DELETE_PQRS_MEASUREINDIVIDUAL";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PQRS", "PQRSAdmin_IndividualReporting");
    },

    AddIndividualReporting: function () {
        var params = [];
        params["ParentCtrl"] = "adminTabIndividualReporting";
        params["FromAdmin"] = 0;
        params["mode"] = "Add";
        LoadActionPan("PQRS_IndividualReporting_Detail", params);
    },

    UnLoadTab: function () {
        var objDeffered = $.Deferred();

        if (PQRS_IndividualReporting.params["FromAdmin"] == "0") {
            if (PQRS_IndividualReporting.params != null && PQRS_IndividualReporting.params.ParentCtrl != null) {
                UnloadActionPan(PQRS_IndividualReporting.params.ParentCtrl, 'PQRS_IndividualReporting');
            }
            else
                UnloadActionPan(null, 'PQRS_IndividualReporting');
        }
        else {

            RemoveAdminTab();
        }
        objDeffered.resolve();
        return objDeffered;
    },
}