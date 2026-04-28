PQRS_MeasureGroups = {
    bIsFirstLoad: true,
    params: [],
    measureGroupTable: null,
    Load: function (params) {
        PQRS_MeasureGroups.params = params;
        PQRS_MeasureGroups.measureGroupTable = null;
        if (PQRS_MeasureGroups.params.PanelID != 'pnlPQRSMeasureGroups') {
            PQRS_MeasureGroups.params.PanelID = PQRS_MeasureGroups.params.PanelID + ' #pnlPQRSMeasureGroups';
        } else {
            PQRS_MeasureGroups.params.PanelID = 'pnlPQRSMeasureGroups';
        }

        var self = $('#' + PQRS_MeasureGroups.params.PanelID);
        self.loadDropDowns(true).done(function () {
            $('#' + PQRS_MeasureGroups.params["PanelID"] + ' select[multiple]').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true
            });
            PQRS_MeasureGroups.measureGroupsSearch();
        });

    },

    measureGroupsSearch: function (measureGroupId, PageNo, rpp) {
        if ($('#' + PQRS_MeasureGroups.params.PanelID + ' #pnlMeasureGroups_Result').css("display") == "none") {
            $('#' + PQRS_MeasureGroups.params.PanelID + ' #pnlMeasureGroups_Result').show();
        }

        var self = $('#' + PQRS_MeasureGroups.params.PanelID + ' form');
        var myJSON = self.getMyJSONByName();

        PQRS_MeasureGroups.SearchMeasureGroups_DbCall(myJSON, measureGroupId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                PQRS_MeasureGroups.MeasureGroupsGridLoad(response);
                var TableControl = PQRS_MeasureGroups.params.PanelID + '  #dgvMeasureGroups';
                var PagingPanelControlID = PQRS_MeasureGroups.params.PanelID + ' #dgvMeasureGroups_Paging';
                var ClassControlName = "PQRS_MeasureGroups";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.measureGroupCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                    PQRS_MeasureGroups.measureGroupsSearch(PrimaryID, PageNumber, ResultPerPage);
                }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    MeasureGroupsGridLoad: function (response) {
        var isactive = $('#' + PQRS_MeasureGroups.params.PanelID + ' #pnlMeasureGroups_Result #divSwitch #switchActive').attr('isactive');
        if ($.fn.dataTable.isDataTable('#' + PQRS_MeasureGroups.params.PanelID + " #dgvMeasureGroups")) {
            $("#" + PQRS_MeasureGroups.params.PanelID + " #dgvMeasureGroups").dataTable().fnClearTable();
            $('#' + PQRS_MeasureGroups.params.PanelID + " #dgvMeasureGroups").dataTable().fnDestroy();
            $('#' + PQRS_MeasureGroups.params.PanelID + " #dgvMeasureGroups tbody").find("tr").remove();
        }
      
        var arraTemp = [];
        if (response.measureGroupCount > 0) {
            var MeasureGroupsLoadJSONData = JSON.parse(response.measureGroupList_JSON);
            $.each(MeasureGroupsLoadJSONData, function (i, item) {
                var CurrentRowchilds = $();
                var $row = $('<tr/>');
                $row.attr("onclick", "");
                $row.attr("id", "gvMeasureGroups_row" + item.MeasureGroupsId);
                $row.attr("MeasureGroupsId", item.MeasureGroupsId);
               
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
                //on row click, edit the record, requirement from dr Hajjar
                //if (Isdisabled == '') {
                //    $row.attr("onclick", "PQRS_MeasureGroups.MeasureGroupsRowEdit(" + item.MeasureGroupsId + ",'Edit'" + ", event);");
                //}
                
                item.providersName = item.providersName.substring(0, item.providersName.length - 2);
                var MeasureGroupName = "<div class='pull-left size100perLes20' onclick=PQRS_MeasureGroups.MeasureGroupsEdit('" + item.measureGroupId + "','Edit',event)>" + item.measureGroupName + " (" + item.providersName.split(',').length + ") </div>" + '<a  href="javacript:void(0);" class="on-editing expand-row pull-right" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a>';
                $row.append('<td><a ' + Isdisabled
                    + ' class="btn  btn-xs" href="#" onclick="PQRS_MeasureGroups.MeasureGroupsDelete(' + item.measureGroupId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a ' + Isdisabled
                    + ' class="btn btn-xs" href="javascript:void(0);" onclick="PQRS_MeasureGroups.MeasureGroupsEdit(' + item.measureGroupId + ",'Edit'" + ', event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" ' + Isdisabled
                    + ' href="#" onclick="PQRS_MeasureGroups.MeasureGroupsActiveInactive(' + item.measureGroupId + "," + isactive + ', event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td>' +
                    '<td>' + MeasureGroupName + '</td><td>' + item.createdOn + '</td>');
                if (item.providersName != '') {
                    var currentHistory = '<tr class="childRow-bg" id="' + item.measureGroupId + '"><td></td><td>(' + item.providersName + ')</td><td></td></tr>';
                    CurrentRowchilds = CurrentRowchilds.add(currentHistory);
                    $('#' + PQRS_MeasureGroups.params.PanelID + ' #pnlMeasureGroups_Result #dgvMeasureGroups tbody').last().append($row);
                    arraTemp.push({ row: $row, childs: CurrentRowchilds });
                }
            });
        }
        else {
            $('#' + PQRS_MeasureGroups.params.PanelID + ' #pnlMeasureGroups_Result #dgvMeasureGroups').DataTable({
                "language": {
                    "emptyTable": "No Measure  is Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            });
        }

        var checked = '';
        var isactive = $('#' + PQRS_MeasureGroups.params.PanelID + ' #ddlActive').val();
        if (isactive == "0" || isactive == 0) {
            isactive = "0";
        } else if (isactive == null || isactive=='') {
            isactive = "1";
            checked = 'checked="checked"';
        } else {
            isactive = "1";
            checked = 'checked="checked"';
        }
        if ($.fn.dataTable.isDataTable('#' + PQRS_MeasureGroups.params.PanelID + ' #pnlMeasureGroups_Result #dgvMeasureGroups'))
            ;
        else {
            PQRS_MeasureGroups.measureGroupTable = $('#' + PQRS_MeasureGroups.params.PanelID + ' #pnlMeasureGroups_Result #dgvMeasureGroups').DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
            //rander childs
            $.each(arraTemp, function (i, item) {
                if (PQRS_MeasureGroups.measureGroupTable != null) {
                    var row = PQRS_MeasureGroups.measureGroupTable.row(item.row);
                    if (item.childs.length > 0) {
                        row.child(item.childs);
                    }
                    else {
                        //row.find("td:nth-child(1)").html("");
                    }
                }

            });
            $('#' + PQRS_MeasureGroups.params.PanelID + ' #pnlMeasureGroups_Result #dgvMeasureGroups').off()
            .on('click', 'a.expand-row', function (e) {
                e.preventDefault();

                PQRS_MeasureGroups.rowExpand($(this).closest('tr'));
            })
        }
        var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                      '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="PQRS_MeasureGroups.activeMeasureGroupsSearch(this);">' +
                       '</div><span class="pl-xs">Active</span>';

        $("#" + PQRS_MeasureGroups.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        EMRUtility.SwicthWidgetInializatoin();
    },

  
    rowExpand: function ($row) {
      
        var row = PQRS_MeasureGroups.measureGroupTable.row($row);
        if (row.child.isShown()) {
            // This row is already open - close it
            $row.find("td .fa-minus-square").attr("class", "fa fa-plus-square");
            row.child.hide();
            //tr.removeClass('shown');
        }
        else {
            $row.find("td .fa-plus-square").attr("class", "fa fa-minus-square");
            // Open this row
            row.child.show();
            if (row.child() != null && row.child().find('[data-toggle="tooltip"]').length > 0) {
                row.child().find('[data-toggle="tooltip"]').tooltip({ html: true });
            }
        }
    },
    activeMeasureGroupsSearch: function (objThis) {
        var isactive = $(objThis).attr('isactive');
        if (isactive == '1') {
            $(objThis).attr('isactive', '0');
            isactive = 0;
        }
        else if (isactive == '0') {
            $(objThis).attr('isactive', '1');
            isactive = 1;
        }
        $('#' + PQRS_MeasureGroups.params.PanelID + ' #ddlActive').val(isactive);
        PQRS_MeasureGroups.measureGroupsSearch();
    },
    MeasureGroupsEdit: function (measureGroupId, mode, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        //Start//22/12/2015//Ahmad Raza//Privileges Logic Implemented
        AppPrivileges.GetFormPrivileges("Clinical_PQRS_GPRO Submission", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                 var params = [];
                params["MeasureGroupId"] = measureGroupId;
                params["mode"] = mode;
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = PQRS_MeasureGroups.params["TabID"];
                LoadActionPan("PQRS_MeasureGroups_Detail", params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        //End//22/12/2015//Ahmad Raza//Privileges Logic Implemented
    },


    MeasureGroupsDelete: function (measureGroupId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }

        utility.myConfirm('1', function () {
            $('body').removeClass('modal-open');
            if (measureGroupId == "" || measureGroupId == "undefined") {
            }
            else {
                PQRS_MeasureGroups.measureGroupsDeleted_DbCall(measureGroupId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        PQRS_MeasureGroups.measureGroupsSearch();
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


    MeasureGroupsActiveInactive: function (measureGroupId, IsActive, event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.myConfirm('3', function () {
            if (measureGroupId == "" || measureGroupId == "undefined") {
            }
            else {
                PQRS_MeasureGroups.updateMeasureGroupsActiveInactive_Dbcall(measureGroupId, IsActive).done(function (response) {
                    if (response.status != false) {
                        var response = JSON.parse(response);
                        utility.DisplayMessages(response.Message, 1);
                        PQRS_MeasureGroups.measureGroupsSearch();
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

    SearchMeasureGroups_DbCall: function (MeasureGroupsData, MeasureGroupsId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = JSON.parse(MeasureGroupsData);
        objData["MeasureGroupId"] = MeasureGroupsId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "SEARCH_PQRS_MEASUREGROUPS";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PQRS", "PQRSAdmin_MeasureGroup");
    },

   
    /*
 Author: Muhammad Azhar Shahzad
 Purpose: to change active / in active records of Grid of Notes template 
 Creation Date: March 02,2016 */
    updateMeasureGroupsActiveInactive_Dbcall: function (MeasureGroupsId, IsActive) {
        var objData = {};
        objData["MeasureGroupId"] = MeasureGroupsId;
        objData["IsActive"] = IsActive;

        objData["commandType"] = "UPDATE_PQRS_MEASUREGROUPS_ACTIVE_INACTIVE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PQRS", "PQRSAdmin_MeasureGroup");
    },
    measureGroupsDeleted_DbCall: function (MeasureGroupsId) {
        var objData = {};
        objData["MeasureGroupId"] = MeasureGroupsId;
        objData["commandType"] = "DELETE_PQRS_MEASUREGROUPS";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PQRS", "PQRSAdmin_MeasureGroup");
    },
    AddMeasureGroups: function () {
        var params = [];
        params["ParentCtrl"] = "adminTabMeasureGroups";
        params["FromAdmin"] = 0;
        params["mode"] = "Add";
        LoadActionPan("PQRS_MeasureGroups_Detail", params);
    },

    UnLoadTab: function () {
        var objDeffered = $.Deferred();

        if (PQRS_MeasureGroups.params["FromAdmin"] == "0") {
            if (PQRS_MeasureGroups.params != null && PQRS_MeasureGroups.params.ParentCtrl != null) {
                UnloadActionPan(PQRS_MeasureGroups.params.ParentCtrl, 'PQRS_MeasureGroups');
            }
            else
                UnloadActionPan(null, 'PQRS_MeasureGroups');
        }
        else {

            RemoveAdminTab();
        }
        objDeffered.resolve();
        return objDeffered;
    },

    
}