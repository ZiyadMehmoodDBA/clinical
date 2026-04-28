MIPS_AdminPreferenceGroup = {
    params: [],
    bIsFirstLoad: true,
    RequireSubmit: true,
    ActualVisitID: null,
    specialityCheckedIds: [],
    SpecialtyIds: '',
    ActualChargeCaptureID: null,
    IsChargeCapture: null,
    Load: function (params) {

        MIPS_AdminPreferenceGroup.params = params;

        if (MIPS_AdminPreferenceGroup.bIsFirstLoad) {
            MIPS_AdminPreferenceGroup.bIsFirstLoad = false;
            MIPS_AdminPreferenceGroup.LoadPracticLookup();

            //MIPS_AdminPreferenceGroup.Search_IndividualProvider();
            MIPS_AdminPreferenceGroup.Search_MIPSGroupPreferences();
            var self = $('#pnlMIPSAdminPreferenceGroup');
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#lstEntityId").attr('disabled', 'disabled');
            }
            self.loadDropDowns(true).done(function () {
                if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                    self.find("#lstEntityId").val(globalAppdata["SeletedEntityId"]);

                }
                //MIPS_AdminPreferenceGroup.UserSearch('0');
            });
        }
        MIPS_AdminPreferenceGroup.Search_MIPSGroupPreferences();
    },
    LoadPracticLookup: function () {
        MIPS_AdminPreferenceGroup.LoadPracticLookup_DBCall().done(function (response) {
            response = JSON.parse(response);

            if (response.status != false) {
                $("#" + MIPS_AdminPreferenceGroup.params.PanelID + " #ddlGroupName").empty();
                $("#" + MIPS_AdminPreferenceGroup.params.PanelID + " #ddlGroupName").append('<option value="" >-Select-</option>');
                var list = JSON.parse(response.IndividualProCountLoad_JSON);
                $.each(list, function (i, item) {
                    $("#" + MIPS_AdminPreferenceGroup.params.PanelID + " #ddlGroupName").append('<option value=' + item.GroupId + ' >' + item.GroupName + '</option>')
                });

            }
            else {

            }

        });

    },
    LoadPracticLookup_DBCall: function () {
        var objData = new Object();
        objData["EntityId"] = globalAppdata.SeletedEntityId;
        objData["commandType"] = "loadgroupnamelookup";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "iTrack", "iTrackAdmin");

    },

    Search_MIPSGroupPreferences: function (pageNumber, resultPerPage) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("MIPS Preference_Group / Virtual Group", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                MIPS_AdminPreferenceGroup.Search_MIPSGroupPreferences_DBCall(pageNumber, resultPerPage).done(function (response) {
                    response = JSON.parse(response);

                    if (response.status != false && response.IndividualProCount > 0) {

                        var list = JSON.parse(response.IndividualProCountLoad_JSON);

                        MIPS_AdminPreferenceGroup.GridLoad(list);
                        var TableControl = MIPS_AdminPreferenceGroup.params.PanelID + " #dgvMIPSAdminGroupPreferences";
                        var PagingPanelControlID = MIPS_AdminPreferenceGroup.params.PanelID + " #divdgvMIPSAdminGroupPreferencesPaging";
                        var ClassControlName = "MIPS_AdminPreferenceGroup";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.IndividualProCount, pageNumber, 15, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, pageNumber, resultPerPage) {
                            MIPS_AdminPreferenceGroup.LoadMIPSProviders(null, pageNumber, resultPerPage);
                        }), 10);

                    }
                    else {
                        MIPS_AdminPreferenceGroup.GridLoad();
                    }

                });
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });

    },
    Search_MIPSGroupPreferences_DBCall: function (pageNumber, resultPerPage) {
        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (resultPerPage == null) {
            resultPerPage = 15;
        }
        var GroupName = null;
        if ($('#pnlMIPSAdminPreferenceGroup #ddlGroupName :selected').val()) {
            GroupName = $('#pnlMIPSAdminPreferenceGroup #ddlGroupName :selected').text();
        }
        var performanceyear;
        if ($('#pnlMIPSAdminPreferenceGroup #ddlPerformanceYear :selected').val()) {
            performanceyear = $('#pnlMIPSAdminPreferenceGroup #ddlPerformanceYear :selected').text();
        }
        var objData = new Object();
        objData["EntityId"] = globalAppdata.SeletedEntityId;
        objData["GroupName"] = GroupName;
        objData["PerformanceYear"] = performanceyear;
        objData["IsActive"] = $('#pnlMIPSAdminPreferenceGroup #switchActive').prop('checked');
        objData["pageNumber"] = pageNumber;
        objData["RowsPerPage"] = resultPerPage;
        objData["commandType"] = "searchmimpsgrouppreferences";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "iTrack", "iTrackAdmin");

    },
    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmMIPSlIndividualProvider";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "MIPSPreference_IndividualProvider";
        LoadActionPan('Admin_Provider', params);
    },
    HideProviderLink: function (value) {
        if (value == "") {
            $('#pnlIndividualProvider #txtProvider').attr("ProviderId", "-1");
            $('#pnlIndividualProvider #hfProvider').val("-1");
            $("#pnlIndividualProvider #lnkProviderEdit").css("display", "none");
            $("#pnlIndividualProvider #lblProvider").css("display", "inline");
        }
    },
    BindProvider: function () {
        var Ctrl = $("#pnlIndividualProvider #frmMIPSlIndividualProvider #txtProvider");
        var hfCtrl = $("#pnlIndividualProvider #frmMIPSlIndividualProvider #hfProvider");
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, null);
    },
    // Zia Mehmood
    //Validation function

    // End validation function
    Loaddetail: function () {

        var params = [];
        params["ProviderId"] = "-1";
        params["mode"] = "Add";
        params["FromAdmin"] = MIPS_AdminPreferenceGroup.params["FromAdmin"];
        params["ParentCtrl"] = 'MIPS_AdminPreferenceGroup';
        LoadActionPan('MIPS_AdminPreferenceGroupDetail', params);
    },

    // Start-- search Activity Log
    Search_IndividualProvider: function (pageNumber, rowsPerPage) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("MIPS Preference_Group / Virtual Group", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                MIPS_AdminPreferenceGroup.Search_IndividualProvider_DBCall(pageNumber, rowsPerPage).done(function (response) {
                    response = JSON.parse(response);

                    if (response.status != false) {


                        var ActivityLogUser_JSON = JSON.parse(response.IndividualProCountLoad_JSON);
                        MIPS_AdminPreferenceGroup.GridLoad(ActivityLogUser_JSON, response.IndividualProCount, "NewEntry", " #pnlAuditbleEventsActivityLog #dgvNewEntry");
                        var TableControl = MIPS_AdminPreferenceGroup.params.PanelID + " #dgvNewEntry";
                        var PagingPanelControlID = MIPS_AdminPreferenceGroup.params.PanelID + " #dgvActivityLogUser_Paging";
                        var ClassControlName = "AuditbleEvents_ActivityLog";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.IndividualProCount, pageNumber, 15, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, pageNumber, resultPerPage) {
                            MIPS_AdminPreferenceGroup.Search_ActivityLog(pageNumber, resultPerPage);
                        }), 10);
                        if (ActivityLogUser_JSON.length > 0) {
                            $('#pnlAuditbleEventsActivityLog #dgvNewEntry tbody tr:first').click();
                        }
                        else {
                            MIPS_AdminPreferenceGroup.GridLoad(ActivityLogUser_JSON, response.ActivityLogCompCount, "User", " #pnlAuditbleEventsActivityLog #dgvUser");
                            MIPS_AdminPreferenceGroup.GridLoad(ActivityLogUser_JSON, response.ActivityLogCompCount, "Changes", " #pnlAuditbleEventsActivityLog #dgvChanges");
                        }


                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }

                });
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    Search_IndividualProvider_DBCall: function (pageNumber, rowsPerPage) {
        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 15;
        }
        var objData = new Object();
        //objData["AuditReportId"] = AuditReportId;
        objData["PageNumber"] = pageNumber;
        objData["RowsPerPage"] = rowsPerPage;

        //objData["ProviderId"] = Clinical_ProgressNote.params.CurrentNotesProviderId
        objData["ProviderName"] = $("#NoteProviderText").val();
        objData["Specialty"] = $("#ddlSpeciality :selected").text();
        objData["NPI"] = $("#txtNPI").val();
        objData["Entity"] = $("#ddlSpeciality :selected").text();
        objData["PracticeType"] = $("#ddlPracticeType :selected").text();
        objData["MIPSEligibility"] = $("#ddlMIPSEligibility :selected").text();
        objData["Ineligible"] = $("#ddlIneligible :selected").text();
        objData["ReportingType"] = $("#ddlReportingType :selected").text();
        objData["ReportingMethod"] = $("#ddlReportingMethod :selected").text();
        objData["ReportingMIPS"] = $("#ddlReportingMIPS :selected").text();
        objData["IsActive"] = 1;
        objData["commandType"] = "loadindividualprovider";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "iTrack", "iTrackAdmin");

    },
    // End -- search Activity Log

    // Start -- Component Load
    ActivityLogsComponent: function (ProfileName, PatientId, DateAndTime, UserId, pageNumber, rowsPerPage) {
        MIPS_AdminPreferenceGroup.searchActivityLogComponents_DBCall(ProfileName, PatientId, DateAndTime, UserId, pageNumber, rowsPerPage).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {


                var ActivityLogUser_JSON = JSON.parse(response.ActivityLogCompLoad_JSON);
                MIPS_AdminPreferenceGroup.GridLoad(ActivityLogUser_JSON, response.ActivityLogCompCount, "User", " #pnlAuditbleEventsActivityLog #dgvUser");
                var TableControl = MIPS_AdminPreferenceGroup.params.PanelID + " #dgvUser";
                var PagingPanelControlID = MIPS_AdminPreferenceGroup.params.PanelID + " #dgvActivityLogComp_Paging";
                var ClassControlName = "AuditbleEvents_ActivityLog";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.ActivityLogCompCount, pageNumber, 15, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, pageNumber, resultPerPage) {
                    MIPS_AdminPreferenceGroup.ActivityLogsComponent($('#pnlAuditbleEventsActivityLog #dgvNewEntry tr.active #rowprofilename').text(), $('#pnlAuditbleEventsActivityLog #dgvNewEntry tr.active #rowpatid').text(), $('#pnlAuditbleEventsActivityLog #dgvNewEntry tr.active #rowdateid').text(), $('#pnlAuditbleEventsActivityLog #dgvNewEntry tr.active #rowuserid').text(), pageNumber, resultPerPage);
                }), 10);
                if (ActivityLogUser_JSON.length > 0) {
                    $('#pnlAuditbleEventsActivityLog #dgvUser tbody tr:first').click();
                }
                else {
                    MIPS_AdminPreferenceGroup.GridLoad(ActivityLogUser_JSON, response.ActivityLogCompCount, "Changes", " #pnlAuditbleEventsActivityLog #dgvChanges");
                }

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });
    },
    searchActivityLogComponents_DBCall: function (ProfileName, PatientId, DateAndTime, UserId, pageNumber, rowsPerPage) {
        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 15;
        }
        var objData = new Object();
        //objData["AuditReportId"] = AuditReportId;
        objData["PageNumber"] = pageNumber;
        objData["RowsPerPage"] = rowsPerPage;
        objData["ProfileName"] = ProfileName;
        objData["PatientId"] = PatientId;
        objData["DateAndTime"] = DateAndTime;
        objData["UserId"] = UserId;
        objData["commandType"] = "auditbleeventsactivitylogcomponents";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "AuditbleEventsActivityLog", "AuditbleEventsActivityLog");
    },
    // End -- Components load
    // Start -- ActivityLog Changes
    ActivityLogsChanges: function (ColumnKeyId, ProfileName, DateAndTime, pageNumber, rowsPerPage) {
        MIPS_AdminPreferenceGroup.searchActivityLogChanges_DBCall(ColumnKeyId, ProfileName, DateAndTime, pageNumber, rowsPerPage).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {


                var ActivityLogUser_JSON = JSON.parse(response.ActivityLogChangesLoad_JSON);
                MIPS_AdminPreferenceGroup.GridLoad(ActivityLogUser_JSON, response.ActivityLogChangesCount, "Changes", " #pnlAuditbleEventsActivityLog #dgvChanges");
                var TableControl = MIPS_AdminPreferenceGroup.params.PanelID + " #divTablesData";
                var PagingPanelControlID = MIPS_AdminPreferenceGroup.params.PanelID + " #divdgvMIPSAdminGroupPreferencesPaging";
                var ClassControlName = "MIPS_AdminPreferenceGroup";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.ActivityLogChangesCount, pageNumber, 15, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, pageNumber, resultPerPage) {
                    MIPS_AdminPreferenceGroup.ActivityLogsChanges($('#pnlAuditbleEventsActivityLog #dgvUser tr.active #colkey').text(), $('#pnlAuditbleEventsActivityLog #dgvUser tr.active #profName').text(), $('#pnlAuditbleEventsActivityLog #dgvUser tr.active #datetime').text(), pageNumber, resultPerPage);
                }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });
    },
    searchActivityLogChanges_DBCall: function (ColumnKeyId, ProfileName, DateAndTime, pageNumber, rowsPerPage) {
        if (pageNumber == null) {
            pageNumber = 1;
        }
        if (rowsPerPage == null) {
            rowsPerPage = 15;
        }
        var objData = new Object();
        //objData["AuditReportId"] = AuditReportId;
        objData["PageNumber"] = pageNumber;
        objData["RowsPerPage"] = rowsPerPage;
        objData["ColumnKeyId"] = ColumnKeyId;
        objData["DateAndTime"] = DateAndTime;
        objData["ProfileName"] = ProfileName;
        objData["commandType"] = "auditbleeventsactivitylogChanges";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "AuditbleEventsActivityLog", "AuditbleEventsActivityLog");

    },
    // ENd -- ActivityLog Changes
    GridLoad: function (list) {
        var isactivebtn = $('#' + MIPS_AdminPreferenceGroup.params.PanelID + ' #switchActive').attr('isactive');
        var gridId = "#dgvMIPSAdminGroupPreferences";
        if ($.fn.dataTable.isDataTable('#' + MIPS_AdminPreferenceGroup.params.PanelID + " #dgvMIPSAdminGroupPreferences")) {
            $("#" + MIPS_AdminPreferenceGroup.params.PanelID + " #dgvMIPSAdminGroupPreferences").dataTable().fnClearTable();
            $('#' + MIPS_AdminPreferenceGroup.params.PanelID + " #dgvMIPSAdminGroupPreferences").dataTable().fnDestroy();
            $('#' + MIPS_AdminPreferenceGroup.params.PanelID + " #dgvMIPSAdminGroupPreferences tbody").find("tr").remove();
        }

        var emptyTableMsg = "No Record Found";

        var listGroup = null;
        var listGroupDetail = null;
        if (list && list.Groups && list.GroupDetail) {
            listGroup = list.Groups;
            listGroupDetail = list.GroupDetail;
        }
        MIPS_AdminPreferenceGroup.params.MemberProviders = listGroupDetail;
        var arraTemp = [];
        if (listGroup != null && listGroup.length > 0) {
            var firstRowId = "";
            $.each(listGroup, function (i, item) {
                var CurrentRowchilds = $();
                var groupDetails = $.grep(MIPS_AdminPreferenceGroup.params.MemberProviders, function (a) {
                    return a.GroupId == item.GroupId;
                });
                var members = "";
                if (groupDetails && groupDetails.length > 0) {
                    $.each(groupDetails, function (i, item) {

                        members += "," + item.ProviderName;

                    });
                }
                if (members != "") {
                    members = members.substring(1);
                }
                var $row = $('<tr/>');
                var _rowId = "";
                var emptyTableMsg = "";

                _rowId = "dgvMIPSAdminGroupPreferences_row" + i;
                if (item.ColumnKeyId == null || item.ColumnKeyId == "") {
                    temp_colkey = null;
                }
                else {
                    temp_colkey = item.ColumnKeyId;
                }
                if (item.IsActive == "True") {
                    isactive = 1;
                    isEventactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isEventactive = 1;
                    isactive = 0;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }
                var Providers = members;
                $row.append('<td id= "MIPSProviderId" style="display:none;">' + item.GroupId + '</td><td><a class="btn btn-xs" href="#" onclick="MIPS_AdminPreferenceGroup.GroupPreferencesEdit(\'' + item.GroupId + '\');"  title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="MIPS_AdminPreferenceGroup.GroupPreferencesActiveInactive(\'' + item.GroupId + '\',this,event ,\'' + isactive + '\');"><i class="' + tglclass + '"></i></a></td><td><div class="pull-left size100perLes20">' + item.GroupName + '</div><a  href="javacript:void(0);" class="on-editing expand-row pull-right" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a></td><td>' + item.TIN + '</td><td>' + item.PerformanceYear + '</td><td>' + item.CreatedOn.replace(" 12:00:00 AM", "") + '</td>');
                //$row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); MIPS_AdminPreferenceGroup.GroupPreferencesEdit('" + item.GroupId + "');");
                if (Providers != '') {
                    var currentHistory = '<tr class="childRow-bg" id="' + item.GroupId + '"><td style="display:none;"></td><td></td><td>(' + Providers + ')</td><td></td><td></td><td></td></tr>';
                    CurrentRowchilds = CurrentRowchilds.add(currentHistory);
                    arraTemp.push({ row: $row, childs: CurrentRowchilds });
                }


                $(gridId + " tbody").last().append($row);
            });
        }
        else {
            $('#divdgvMIPSAdminGroupPreferencesPaging').css("display", "none");
            $(gridId).DataTable({
                "language": {
                    "emptyTable": emptyTableMsg
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [2] }]
            });


        }
        if ($.fn.dataTable.isDataTable('#' + MIPS_AdminPreferenceGroup.params["PanelID"] + ' #dgvMIPSAdminGroupPreferences'))
            ;
        else {
            MIPS_AdminPreferenceGroup.Table = $('#' + MIPS_AdminPreferenceGroup.params["PanelID"] + ' #dgvMIPSAdminGroupPreferences').DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "searching": true, "bFilter": false, "order": [[3, "desc"]] }); // to remove records per page dropdown
            $.each(arraTemp, function (i, item) {
                if (MIPS_AdminPreferenceGroup.Table != null) {
                    var row = MIPS_AdminPreferenceGroup.Table.row(item.row);
                    if (item.childs.length > 0) {
                        row.child(item.childs);
                    }
                    else {
                        //row.find("td:nth-child(1)").html("");
                    }
                }

            });

            $('#' + MIPS_AdminPreferenceGroup.params["PanelID"] + ' #dgvMIPSAdminGroupPreferences').off()
        .on('click', 'a.expand-row', function (e) {
            e.preventDefault();

            MIPS_AdminPreferenceGroup.rowExpand($(this).closest('tr'));
        })

        }
        var checked = '';
        if (isactivebtn == "0" || isactivebtn == 0) {
        } else if (isactivebtn == null) {
            isactivebtn = "1";
            checked = 'checked="checked"';
        } else {
            isactivebtn = "1";
            checked = 'checked="checked"';
        }

        var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                          '<input id="switchActive" isactive="' + isactivebtn + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="MIPS_AdminPreferenceGroup.ActiveIncactive(this);">' +
                           '</div><span class="pl-xs">Active</span>';

        $("#" + MIPS_AdminPreferenceGroup.params.PanelID + ' .datatables-header div:first').html(HtmlOfSwitch);
        MIPS_AdminPreferenceGroup.readyFunction();

    },

    rowExpand: function ($row) {

        var row = MIPS_AdminPreferenceGroup.Table.row($row);
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
    GroupPreferencesActiveInactive: function (GroupId, obj, ev, isactive) {
        ev.stopPropagation();
        utility.myConfirm('3', function () {
            if (GroupId == "" || GroupId == "undefined") {
            }
            else {
                if (isactive == "1") {
                    isactive = 0

                } else {
                    isactive = 1
                }

                MIPS_AdminPreferenceGroup.GroupPreferencesActiveInactive_Dbcall(GroupId, isactive).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        MIPS_AdminPreferenceGroup.Search_MIPSGroupPreferences();
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
    GroupPreferencesActiveInactive_Dbcall: function (GroupId, isactive) {
        var objData = new Object();
        objData["GroupId"] = GroupId;
        objData["IsActive"] = isactive;
        objData["commandType"] = "activeinactivemipspreferencesgroup";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "iTrack", "iTrackAdmin");
    },
    GroupPreferencesEdit: function (GroupId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("MIPS Preference_Group / Virtual Group", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["GroupId"] = GroupId;
                params["mode"] = "Edit";
                params["FromAdmin"] = MIPS_AdminPreferenceGroup.params["FromAdmin"];
                params["ParentCtrl"] = 'MIPS_AdminPreferenceGroup';
                LoadActionPan('MIPS_AdminPreferenceGroupDetail', params);
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    readyFunction: function () {

        (function ($) {
            'use strict';
            $(function () {
                $('[data-plugin-ios-switch]').each(function () {
                    var $this = $(this);

                    $this.themePluginIOS7Switch();
                });
            });
        }).apply(this, [jQuery]);
    },
    ActiveIncactive: function (obj) {

        var isactive = $(obj).attr('isactive');
        if (isactive == '1') {
            $(obj).attr('isactive', '0');
        }
        else if (isactive == '0') {
            $(obj).attr('isactive', '1');
        }
        MIPS_AdminPreferenceGroup.Search_MIPSGroupPreferences();

    },






}
