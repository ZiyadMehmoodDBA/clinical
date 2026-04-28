iTrack_AdminPreferenceGroupReporting = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
    
        iTrack_AdminPreferenceGroupReporting.params = params;

        if (iTrack_AdminPreferenceGroupReporting.params.PanelID != 'pnlMIPSAdminPreferenceGroupReporting') {
            iTrack_AdminPreferenceGroupReporting.params.PanelID = iTrack_AdminPreferenceGroupReporting.params.PanelID + ' #pnlMIPSAdminPreferenceGroupReporting';
        } else {
            iTrack_AdminPreferenceGroupReporting.params.PanelID = 'pnlMIPSAdminPreferenceGroupReporting';
        }
        var self = $('#' + iTrack_AdminPreferenceGroupReporting.params.PanelID + ' #frmiTrackAdminPreferenceGroupReporting');
        self.loadDropDowns(true).done(function () {
            iTrack_AdminPreferenceGroupReporting.LoadGroupLookup();
            $('#' + iTrack_AdminPreferenceGroupReporting.params["PanelID"] + ' #ddlMemberProvider').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                buttonTitle: function (options, select) {
                    var buttonTitle = "";
                    $.each(options, function (i, item) {
                        if (buttonTitle != "") {
                            buttonTitle += "," + $(item).attr("refvalue");
                        }
                        else {
                            buttonTitle += $(item).attr("refvalue");
                        }

                    });

                    return buttonTitle;
                }
            });

        });
        iTrack_AdminPreferenceGroupReporting.measureGroupSearch();
    },
    rowExpand: function ($row) {

        var row = iTrack_AdminPreferenceGroupReporting.measureGroupTable.row($row);
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
    LoadGroupLookup: function () {
        iTrack_AdminIPPreference.LoadGroupLookup_DBCall().done(function (response) {
            response = JSON.parse(response);
            $('#' + iTrack_AdminPreferenceGroupReporting.params["PanelID"] + ' #ddlGroupName').find('option').remove();
            $('#' + iTrack_AdminPreferenceGroupReporting.params["PanelID"] + ' #ddlGroupName').append('<option selected="selected" value="" >-Select-</option>');
            if (response.status != false) {
                var list = JSON.parse(response.IndividualProCountLoad_JSON);
                $.each(list, function (i, item) {
                    $('#' + iTrack_AdminPreferenceGroupReporting.params["PanelID"] + ' #ddlGroupName').append('<option value=' + item.GroupId + ' >' + item.GroupName + '</option>')
                });
            }
        });
    },

    loadGroupData: function () {

        iTrack_AdminIPPreference.loadGroupData_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                var dta = JSON.parse(response.IndividualProCountLoad_JSON)
               
                var members = "";
                if (dta.GroupDetail && dta.GroupDetail.length > 0) {
                    $.each(dta.GroupDetail, function (i, item) {
                        members += "," + item.ProviderId;
                    });
                    $('#' + iTrack_AdminPreferenceGroupReporting.params["PanelID"] + ' #ddlMemberProvider').val(members.split(','));
                    $('#' + iTrack_AdminPreferenceGroupReporting.params["PanelID"] + ' #ddlMemberProvider').multiselect("refresh");
                    $('#' + iTrack_AdminPreferenceGroupReporting.params["PanelID"] + ' #ddlMemberProvider').multiselect('rebuild');
                }
            }
        });
    },
   
    AddGroupReporting: function () {
        var params = [];
        params["ParentCtrl"] = "adminTabMIPSGroupReporting";
        params["FromAdmin"] = 0;
        params["mode"] = "Add";
        LoadActionPan("iTrack_AdminPreferenceGroupReportingDetail", params);
    },
    measureGroupSearch: function ( PageNo, rpp) {
        if ($('#' + iTrack_AdminPreferenceGroupReporting.params.PanelID + ' #pnlGroupReporting_Result').css("display") == "none") {
            $('#' + iTrack_AdminPreferenceGroupReporting.params.PanelID + ' #pnlGroupReporting_Result').show();
        }

        var self = $('#' + iTrack_AdminPreferenceGroupReporting.params.PanelID + ' form');
        var myJSON = self.getMyJSONByName();

        iTrack_AdminPreferenceGroupReporting.SearchMeasureGroup_DBCall(myJSON, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                iTrack_AdminPreferenceGroupReporting.MeasureGroupsGridLoad(response);

                var TableControl = iTrack_AdminPreferenceGroupReporting.params.PanelID + '  #dgvGroupReporting';
                var PagingPanelControlID = iTrack_AdminPreferenceGroupReporting.params.PanelID + ' #dgvGroupReporting_Paging';
                var ClassControlName = "iTrack_AdminPreferenceGroupReporting";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.RecordCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                    iTrack_AdminPreferenceGroupReporting.measureGroupSearch(PrimaryID, PageNumber, ResultPerPage);
                }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    activeMeasureGroupSearch: function (objThis) {
        var isactive = $(objThis).attr('isactive');
        if (isactive == '1') {
            $(objThis).attr('isactive', '0');
            isactive = 0;
        }
        else if (isactive == '0') {
            $(objThis).attr('isactive', '1');
            isactive = 1;
        }
        $('#' + iTrack_AdminPreferenceGroupReporting.params.PanelID + ' #ddlActive').val(isactive);
        iTrack_AdminPreferenceGroupReporting.measureGroupSearch();
    },
    GroupReportingActiveInactive: function (MeasureGroupId, IsActive, event) {
        utility.myConfirm('3', function () {
            if (MeasureGroupId == "" || MeasureGroupId == "undefined") {
            }
            else {
                iTrack_AdminPreferenceGroupReporting.updateMeasureGroupActiveInactive_Dbcall(MeasureGroupId, IsActive).done(function (response) {
                    if (response.status != false) {
                        var response = JSON.parse(response);
                        utility.DisplayMessages(response.Message, 1);
                        iTrack_AdminPreferenceGroupReporting.measureGroupSearch();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { },
                        '3'
                    );
    },

    MeasureGroupsGridLoad: function (response) {
        var isactive = $('#' + iTrack_AdminPreferenceGroupReporting.params.PanelID + ' #pnlGroupReporting_Result #divSwitch #switchActive').attr('isactive');
        if ($.fn.dataTable.isDataTable('#' + iTrack_AdminPreferenceGroupReporting.params.PanelID + " #dgvGroupReporting")) {
            $("#" + iTrack_AdminPreferenceGroupReporting.params.PanelID + " #dgvGroupReporting").dataTable().fnClearTable();
            $('#' + iTrack_AdminPreferenceGroupReporting.params.PanelID + " #dgvGroupReporting").dataTable().fnDestroy();
            $('#' + iTrack_AdminPreferenceGroupReporting.params.PanelID + " #dgvGroupReporting tbody").find("tr").remove();
        }

        var arraTemp = [];
        if (response.RecordCount > 0) {
            var MeasureGroupsLoadJSONData = JSON.parse(response.measureGroupList_JSON);
            $.each(MeasureGroupsLoadJSONData, function (i, item) {
                var CurrentRowchilds = $();
                var $row = $('<tr/>');
                $row.attr("onclick", "");
                $row.attr("id", "gvGroupReporting_row" + item.MeasureGroupId);
                $row.attr("MeasureGroupId", item.MeasureGroupId);

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

                // item.providersName = item.providersName.substring(0, item.providersName.length - 2);
                var grpName = item.GroupName.trim().replace(' ','%20');
                var MeasureGroupName = "<div class='pull-left size100perLes20' onclick=iTrack_AdminPreferenceGroupReporting.GroupReportingEdit('" + item.MeasureGroupId + "','Edit',event,'" + grpName + "')>" + item.GroupName + " (" + item.MemberProvider.split(',').length + ") </div>" + '<a  href="javacript:void(0);" class="on-editing expand-row pull-right" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a>';
                var editgrp = "iTrack_AdminPreferenceGroupReporting.GroupReportingEdit(" + item.MeasureGroupId + ",'Edit', event,'" + grpName + "')";
                $row.append('<td><a ' + Isdisabled
                    + ' class="btn  btn-xs" href="#" onclick="iTrack_AdminPreferenceGroupReporting.GroupReportingDelete(' + item.MeasureGroupId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a ' + Isdisabled
                    + ' class="btn btn-xs" href="javascript:void(0);" onclick="'+editgrp+'"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" ' + Isdisabled
                    + ' href="#" onclick="iTrack_AdminPreferenceGroupReporting.GroupReportingActiveInactive(' + item.MeasureGroupId + "," + isactive + ', event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td>' +
                    '<td>' + MeasureGroupName + '</td><td>' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '</td>');
                if (item.MemberProvider != '') {
                    var currentHistory = '<tr class="childRow-bg" id="' + item.MeasureGroupId + '"><td></td><td>(' + item.MemberProvider + ')</td><td></td></tr>';
                    CurrentRowchilds = CurrentRowchilds.add(currentHistory);
                    $('#' + iTrack_AdminPreferenceGroupReporting.params.PanelID + ' #pnlGroupReporting_Result #dgvGroupReporting tbody').last().append($row);
                    arraTemp.push({ row: $row, childs: CurrentRowchilds });
                }
                else
                    $('#' + iTrack_AdminPreferenceGroupReporting.params.PanelID + ' #pnlGroupReporting_Result #dgvGroupReporting tbody').last().append($row);
            });
        }
        else {
            $('#' + iTrack_AdminPreferenceGroupReporting.params.PanelID + ' #pnlGroupReporting_Result #dgvGroupReporting').DataTable({
                "language": {
                    "emptyTable": "No Measure Group is Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            });
        }

        var checked = '';
        var isactive = $('#' + iTrack_AdminPreferenceGroupReporting.params.PanelID + ' #ddlActive').val();
        if (isactive == "0" || isactive == 0) {
            isactive = "0";
        } else if (isactive == null || isactive == '') {
            isactive = "1";
            checked = 'checked="checked"';
        } else {
            isactive = "1";
            checked = 'checked="checked"';
        }
        if ($.fn.dataTable.isDataTable('#' + iTrack_AdminPreferenceGroupReporting.params.PanelID + ' #pnlGroupReporting_Result #dgvGroupReporting'))
            ;
        else {
            iTrack_AdminPreferenceGroupReporting.measureGroupTable = $('#' + iTrack_AdminPreferenceGroupReporting.params.PanelID + ' #pnlGroupReporting_Result #dgvGroupReporting').DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
            //rander childs
            $.each(arraTemp, function (i, item) {
                if (iTrack_AdminPreferenceGroupReporting.measureGroupTable != null) {
                    var row = iTrack_AdminPreferenceGroupReporting.measureGroupTable.row(item.row);
                    if (item.childs.length > 0) {
                        row.child(item.childs);
                    }
                    else {
                        //row.find("td:nth-child(1)").html("");
                    }
                }

            });
            $('#' + iTrack_AdminPreferenceGroupReporting.params.PanelID + ' #pnlGroupReporting_Result #dgvGroupReporting').off()
            .on('click', 'a.expand-row', function (e) {
                e.preventDefault();

                iTrack_AdminPreferenceGroupReporting.rowExpand($(this).closest('tr'));
            })
        }
        var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                      '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="iTrack_AdminPreferenceGroupReporting.activeMeasureGroupSearch(this);">' +
                       '</div><span class="pl-xs">Active</span>';

        $("#" + iTrack_AdminPreferenceGroupReporting.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        EMRUtility.SwicthWidgetInializatoin();
    },


    GroupReportingEdit: function (MeasureGroupId, mode, event, GroupName) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        //Start//22/12/2015//Ahmad Raza//Privileges Logic Implemented
        AppPrivileges.GetFormPrivileges("Clinical_PQRS_Individual Reporting", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["MeasureGroupId"] = MeasureGroupId;
                params["DropDownMeasureGroupName"] = GroupName;
                params["mode"] = mode;
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = iTrack_AdminPreferenceGroupReporting.params["TabID"];
                LoadActionPan("iTrack_AdminPreferenceGroupReportingDetail", params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        //End//22/12/2015//Ahmad Raza//Privileges Logic Implemented
    },
   
    GroupReportingDelete: function (MeasureGroupId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }

        utility.myConfirm('1', function () {
            $('body').removeClass('modal-open');
            if (MeasureGroupId == "" || MeasureGroupId == "undefined") {
            }
            else {
                iTrack_AdminPreferenceGroupReporting.MeasureGroupDeleted_DbCall(MeasureGroupId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        iTrack_AdminPreferenceGroupReporting.measureGroupSearch();
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
    SearchMeasureGroup_DBCall: function (measureGroupData, MeasureGroupId, PageNumber, RowsPerPage) {


        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = JSON.parse(measureGroupData);
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "fill_pqrs_measuregroupdata";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PQRS", "PQRSAdmin_IndividualReporting");
    },
    updateMeasureGroupActiveInactive_Dbcall: function (MeasureIndividualId, IsActive) {
        var objData = {};
        objData["MeasureGroupId"] = MeasureIndividualId;
        objData["is_Active"] = IsActive;

        objData["commandType"] = "update_pqrs_measuregroup_active_inactive";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PQRS", "PQRSAdmin_IndividualReporting");
    },
    MeasureGroupDeleted_DbCall: function (MeasureGroupId) {
        var objData = {};
        objData["MeasureGroupId"] = MeasureGroupId;
        objData["commandType"] = "delete_pqrs_measuregroup";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PQRS", "PQRSAdmin_IndividualReporting");
    },

    //AddIndividualReporting: function () {
    //    var params = [];
    //    params["ParentCtrl"] = "adminTabMIPSGroupReporting";
    //    params["FromAdmin"] = 0;
    //    params["mode"] = "Add";
    //    LoadActionPan("iTrack_AdminPreferenceGroupReportingDetail", params);
    //},

    UnLoadTab: function () {
        var objDeffered = $.Deferred();

        if (iTrack_AdminPreferenceGroupReporting.params["FromAdmin"] == "0") {
            if (iTrack_AdminPreferenceGroupReporting.params != null && iTrack_AdminPreferenceGroupReporting.params.ParentCtrl != null) {
                UnloadActionPan(iTrack_AdminPreferenceGroupReporting.params.ParentCtrl, 'iTrack_AdminPreferenceGroupReporting');
            }
            else
                UnloadActionPan(null, 'iTrack_AdminPreferenceGroupReporting');
        }
        else {

            RemoveAdminTab();
        }
        objDeffered.resolve();
        return objDeffered;
    },
}