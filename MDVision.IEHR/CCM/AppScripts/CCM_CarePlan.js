CCM_CarePlan = {
    IsCarePlanFirstLoad: true,
    CarePlanLoad: function () {
        if (CCM_CarePlan.IsCarePlanFirstLoad) {
            CCM_CarePlan.IsCarePlanFirstLoad = false;
            CCM_CarePlan.searchCarePlan();
        }
    },

    searchCarePlan: function (CarePlanId, PageNo, rpp) {
        if ($("#" + CCM_Patient_Hub.params.PanelID + " #pnlCarePlan_Result").css("display") == "none") {
            $("#" + CCM_Patient_Hub.params.PanelID + " #pnlCarePlan_Result").css("display", 'block');
        }

        CCM_CarePlan.searchCarePlan_DBCall(CarePlanId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                CCM_CarePlan.CarePlanGridLoad(response);
                var TableControl = CCM_Patient_Hub.params.PanelID + " #dgvCarePlan";
                var PagingPanelControlID = CCM_Patient_Hub.params.PanelID + " #divCarePlan_Paging";
                var ClassControlName = "CCM_CarePlan";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.CPCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords,
                    function (PrimaryID, PageNumber, ResultPerPage) {
                        CCM_CarePlan.searchCarePlan(PrimaryID, PageNumber, ResultPerPage);
                    }), 10);

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    carePlanDelete: function (carePlanId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        if (carePlanId > 0 && carePlanId > 0) {
            utility.myConfirm('1', function () {
                CCM_CarePlan.carePlanDelete_DbCall(carePlanId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        CCM_CarePlan.searchCarePlan();
                        utility.DisplayMessages(response.Message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });

            });
        }
    },

    carePlanEditRow: function (carePlanId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        if ((event.srcElement instanceof HTMLAnchorElement || event.srcElement.nodeName.toLowerCase() == 'i') != true) {
            CCM_CarePlan.carePlanEdit(carePlanId, event);
        }
    },

    addCarePlan: function () {
        var params = [];
        params["ParentCtrl"] = "CCM_Patient_Hub";
        params["PatientId"] = CCM_Patient_Hub.params.PatientId;
        params["EnrollmentInfoId"] = CCM_Patient_Hub.params.EnrollmentInfoId;
        params["mode"] = "Add";
        params["FromAdmin"] = "0";
        LoadActionPan('CCM_CarePlanDetail', params);
    },
    carePlanEdit: function (carePlanId, event) {
        var params = [];
        params["ParentCtrl"] = "CCM_Patient_Hub";
        params["FromAdmin"] = "0";
        params["CarePlanId"] = carePlanId;
        params["PatientId"] = CCM_Patient_Hub.params.PatientId;
        params["EnrollmentInfoId"] = CCM_Patient_Hub.params.EnrollmentInfoId;
        params["mode"] = "Edit";
        LoadActionPan('CCM_CarePlanDetail', params);
    },

    carePlanActiveInactive: function (carePlanId, IsActive, event) {
        utility.myConfirm('3', function () {
            if (carePlanId == "" || carePlanId == "undefined") {
            }
            else {
                CCM_CarePlan.carePlanActiveInactive_Dbcall(carePlanId, IsActive).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {

                        utility.DisplayMessages(response.Message, 1);
                        CCM_CarePlan.searchCarePlan();
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

    ActiveCarePlanearch: function (objThis) {
        var isactive = $(objThis).attr('isactive');

        if (isactive == '1') {
            $(objThis).attr('isactive', '0');
        }
        else if (isactive == '0') {
            $(objThis).attr('isactive', '1');
        }

        CCM_CarePlan.searchCarePlan();
    },

    CarePlanGridLoad: function (response) {
        var isactive = $('#' + CCM_Patient_Hub.params.PanelID + ' #pnlCarePlan_Result #divSwitch #switchActive').attr('isactive');
        $("#" + CCM_Patient_Hub.params.PanelID + " #dgvCarePlan").dataTable().fnDestroy();
        $("#" + CCM_Patient_Hub.params.PanelID + " #dgvCarePlan tbody").find("tr").remove();
        if (response.CPCount > 0) {
            var CarePlanJSONData = JSON.parse(response.CPList_JSON);
            $.each(CarePlanJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvCarePlan_row" + item.CarePlanId + "'));CCM_CarePlan.carePlanEditRow(" + item.CarePlanId + ", event);");
                $row.attr("CarePlanId", item.CarePlanId);
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

                $row.append('<td><a class="btn btn-xs" href="javascript:void(0);" onclick="CCM_CarePlan.carePlanDelete(' + item.CarePlanId + ', event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;' +
                                '<a class="btn btn-xs" href="javascript:void(0);" onclick="CCM_CarePlan.carePlanEdit(' + item.CarePlanId + ', event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;' +
                                '<a class="btn  btn-xs" href="javascript:void(0);" onclick="CCM_CarePlan.carePlanActiveInactive(' + item.CarePlanId + "," + isEventactive + ', event);" title="' + activeTitle +
                                '"><i class="' + tglclass + '"></i></a></td>' +
                    '<td>' + item.Name + '</td><td>' + item.Description + '</td>' + '</td><td>' + item.CreatedByName + '</td>' + '</td><td>' + item.ModifiedOn + ' by ' + item.ModifiedByName + '</td>');
                $("#" + CCM_Patient_Hub.params.PanelID + " #dgvCarePlan tbody").last().append($row);
            });

        } else {

            $("#" + CCM_Patient_Hub.params.PanelID + " #dgvCarePlan").DataTable({
                "language": {
                    "emptyTable": "No Care Plan is Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            });
        }

        if ($.fn.dataTable.isDataTable("#" + CCM_Patient_Hub.params.PanelID + " #dgvCarePlan"))
            ;
        else {
            $("#" + CCM_Patient_Hub.params.PanelID + " #dgvCarePlan").DataTable({
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
                        '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="CCM_CarePlan.ActiveCarePlanearch(this);">' +
                         '</div><span class="pl-xs">Active</span>';

        $("#" + CCM_Patient_Hub.params.PanelID + ' #pnlCarePlan_Result .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        EMRUtility.SwicthWidgetInializatoin();
    },

    carePlanActiveInactive_Dbcall: function (carePlanId, IsActive) {
        var objData = {};
        objData["CarePlanId"] = carePlanId;
        objData["IsActive"] = IsActive;

        objData["commandType"] = "UPDATE_CARE_PLAN_ACTIVE_INACTIVE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Patient_Hub", "CarePlan");
    },

    searchCarePlan_DBCall: function (carePlanId, PageNumber, RowsPerPage) {
        var IsActive = null;
        IsActive = $('#' + CCM_Patient_Hub.params.PanelID + ' #pnlCarePlan_Result #divSwitch #switchActive').attr('isactive');
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
        objData["CarePlanId"] = carePlanId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "SEARCH_CAREPLAN_LIST";
        objData["EnrollmentInfoId"] = CCM_Patient_Hub.params.EnrollmentInfoId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Patient_Hub", "CarePlan");
    },

    carePlanDelete_DbCall: function (carePlanId) {
        var objData = {};
        objData["CarePlanId"] = carePlanId;
        objData["commandType"] = "DELETE_CCM_CARE_PLAN";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class 
        return MDVisionService.APIService(data, "Patient_Hub", "CarePlan");
    }
}