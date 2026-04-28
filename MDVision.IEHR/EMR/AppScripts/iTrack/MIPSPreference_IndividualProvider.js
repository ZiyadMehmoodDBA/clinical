MIPSPreference_IndividualProvider = {
    params: [],
    bIsFirstLoad: true,
    RequireSubmit: true,
    ActualVisitID: null,
    specialityCheckedIds: [],
    SpecialtyIds: '',
    ActualChargeCaptureID: null,
    IsChargeCapture: null,
    Load: function (params) {

        MIPSPreference_IndividualProvider.params = params;


        if (MIPSPreference_IndividualProvider.bIsFirstLoad) {
            MIPSPreference_IndividualProvider.bIsFirstLoad = false;
            // MIPSPreference_IndividualProvider.Search_IndividualProvider();
            var self = $('#pnlIndividualProvider');
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#lstEntityId").attr('disabled', 'disabled');
            }
            self.loadDropDowns(true).done(function () {
                if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                    self.find("#lstEntityId").val(globalAppdata["SeletedEntityId"]);
                    MIPSPreference_IndividualProvider.BindProvider();
                    MIPSPreference_IndividualProvider.Search_MIPSIndiviualPreferences();
                }
                //MIPSPreference_IndividualProvider.UserSearch('0');
            });
        }

    },
    changeEligibility: function (val) {
        if (val == 0) {
            $('#pnlIndividualProvider #ddlIneligible').removeClass('disableAll');
        } else {
            $('#pnlIndividualProvider #ddlIneligible').addClass('disableAll');
        }


    },
    Search_MIPSIndiviualPreferences: function (pageNumber, resultPerPage) {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("MIPS Preference_Individual Provider", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                MIPSPreference_IndividualProvider.Search_MIPSIndiviualPreferences_DBCall(pageNumber, resultPerPage).done(function (response) {
                    response = JSON.parse(response);

                    if (response.status != false && response.IndividualProCount > 0) {

                        var list = JSON.parse(response.IndividualProCountLoad_JSON);

                        MIPSPreference_IndividualProvider.GridLoad(list);
                        var TableControl = MIPSPreference_IndividualProvider.params.PanelID + " #dgvMIPSIndividualPreferences";
                        var PagingPanelControlID = MIPSPreference_IndividualProvider.params.PanelID + " #divdgvMIPSIndividualPreferencesPaging";
                        var ClassControlName = "MIPSPreference_IndividualProvider";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.IndividualProCount, pageNumber, 15, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, pageNumber, resultPerPage) {
                            MIPSPreference_IndividualProvider.Search_MIPSIndiviualPreferences(pageNumber, resultPerPage);
                        }), 10);

                    }
                    else {
                        MIPSPreference_IndividualProvider.GridLoad();
                    }


                });
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    Search_MIPSIndiviualPreferences_DBCall: function (pageNumber, rowsPerPage) {
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

        objData["ProviderId"] = $("#pnlIndividualProvider #frmMIPSlIndividualProvider #hfProvider").val();
        objData["Specialty"] = $("#ddlSpeciality :selected").val() == "-Select-" ? "" : $("#ddlSpeciality :selected").val();
        objData["NPI"] = $("#txtNPI").val();
        objData["EntityId"] = "100";// $("#ddlSpeciality :selected").text() == "-Select-" ? "" : $("#ddlSpeciality :selected").text();
        objData["PracticeType"] = $("#ddlPracticeType :selected").text() == "-Select-" ? "" : $("#ddlPracticeType :selected").text();
        objData["MIPSEligibilityStatus"] = $("#ddlMIPSEligibility :selected").val();
        objData["InEligibileReason"] = $("#ddlIneligible :selected").text() == "-Select-" ? "" : $("#ddlIneligible :selected").text();
        objData["ReportingType"] = $("#ddlReportingType :selected").text() == "-Select-" ? "" : $("#ddlReportingType :selected").text();
        objData["ReportingMethod"] = $("#ddlReportingMethod :selected").text() == "-Select-" ? "" : $("#ddlReportingMethod :selected").text();
        objData["IsReporting"] = $("#ddlReportingMIPS :selected").val();
        objData["IsActive"] = true;
        objData["commandType"] = "loadindividualprovider";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "iTrack", "iTrackAdmin");

    },
    GridLoad: function (list) {
        var isactivebtn = $('#' + MIPSPreference_IndividualProvider.params.PanelID + ' #switchActive').attr('isactive');
        var gridId = "#dgvMIPSIndividualPreferences";
        $(gridId + " tbody").empty();
        $(gridId).dataTable().fnDestroy();
        $(gridId + " tbody").find("tr").remove();

        var emptyTableMsg = "No Record Found";
        if (list != null && list.length > 0) {
            var firstRowId = "";

            $.each(list, function (i, item) {
                var $row = $('<tr/>');
                var _rowId = "";
                var emptyTableMsg = "";

                _rowId = "dgvMIPSIndividualPreferences_row" + i;

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
                $row.append('<td id= "MIPSProviderId" style="display:none;">' + item.ProviderId + '</td><td>' + item.ProviderName + '</td><td>' + item.Specialty + '</td><td>' + item.NPI + '</td><td>' + item.TIN + '</td><td>' + item.EntityName + '</td><td>' + item.MIPSEligibilityStatus + '</td><td>' + item.IsReporting + '</td><td>' + item.ReportingType + '</td><td>' + item.ReportingMethod + '</td><td>' + item.ReportingYear + ' </td>');
                $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); MIPSPreference_IndividualProvider.Loaddetail('" + item.ObjectId + "','" + item.ProviderId + "');");

                $(gridId + " tbody").last().append($row);
            });
        }
        else {
            $('#divdgvMIPSIndividualPreferencesPaging').css("display", "none");
            $(gridId).DataTable({
                "language": {
                    "emptyTable": "No Record Found"
                },
                "autoWidth": false,
                "bLengthChange": false,
                "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });

        }
        if ($.fn.dataTable.isDataTable('#' + MIPSPreference_IndividualProvider.params["PanelID"] + ' #dgvMIPSIndividualPreferences'))
            ;
        else {
            $('#' + MIPSPreference_IndividualProvider.params.PanelID + ' #dgvMIPSIndividualPreferences').DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
        }
        $('#divdgvMIPSIndividualPreferencesPaging').css('display', 'none');
        //var checked = '';
        //if (isactivebtn == "0" || isactivebtn == 0) {
        //} else if (isactivebtn == null) {
        //    isactivebtn = "1";
        //    checked = 'checked="checked"';
        //} else {
        //    isactivebtn = "1";
        //    checked = 'checked="checked"';
        //}

        //var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
        //                  '<input id="switchActive" isactive="' + isactivebtn + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="MIPS_AdminPreferenceGroup.ActiveIncactive(this);">' +
        //                   '</div><span class="pl-xs">Active</span>';

        //$("#" + MIPSPreference_IndividualProvider.params.PanelID + ' .datatables-header div:first').html(HtmlOfSwitch);
        //MIPSPreference_IndividualProvider.readyFunction();

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
    OpenProvider: function () {
        var params = [];

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
        var onSelect = function (e) {
            $("#pnlIndividualProvider #frmMIPSlIndividualProvider #hfProvider").val(e.id);
            utility.FillProviderNPI('#pnlIndividualProvider #frmMIPSlIndividualProvider', '#hfProvider', '#txtNPI');
        };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect);
    },
    // Zia Mehmood
    //Validation function

    // End validation function
    Loaddetail: function (preferenceId, providerId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("MIPS Preference_Individual Provider", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["PreferenceId"] = preferenceId == 'undefined' ? null : preferenceId;
                params["ProviderId"] = $('#pnlIndividualProvider #hfProvider').val() == "" ? providerId : $('#pnlIndividualProvider #hfProvider').val();
                params["mode"] = "Edit";
                params["FromAdmin"] = Admin_Provider.params["FromAdmin"];
                params["ParentCtrl"] = 'MIPSPreference_IndividualProvider';
                LoadActionPan('iTrack_AdminIPPreference', params);
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    ResetNPI: function () {
        $("#pnlIndividualProvider #hfProvider").val("");
        $("#pnlIndividualProvider #txtNPI").val("");
    },
    ResetProvider: function () {
        $('#pnlIndividualProvider #txtProvider').val('');
        $('#pnlIndividualProvider #hfProvider').val('');
        //if ($("#pnlIndividualProvider #frmMIPSlIndividualProvider #lnkProviderEdit").css("display") == "inline") {
        //    $("#pnlIndividualProvider #frmMIPSlIndividualProvider #lnkProviderEdit").css("display", "none");
        //    $("#pnlIndividualProvider #frmMIPSlIndividualProvider #lblProvider").css("display", "inline");
        //}
    },

    // Start -- Component Load
    ActivityLogsComponent: function (ProfileName, PatientId, DateAndTime, UserId, pageNumber, rowsPerPage) {
        MIPSPreference_IndividualProvider.searchActivityLogComponents_DBCall(ProfileName, PatientId, DateAndTime, UserId, pageNumber, rowsPerPage).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {


                var ActivityLogUser_JSON = JSON.parse(response.ActivityLogCompLoad_JSON);
                MIPSPreference_IndividualProvider.GridLoad(ActivityLogUser_JSON, response.ActivityLogCompCount, "User", " #pnlAuditbleEventsActivityLog #dgvUser");
                var TableControl = MIPSPreference_IndividualProvider.params.PanelID + " #dgvUser";
                var PagingPanelControlID = MIPSPreference_IndividualProvider.params.PanelID + " #dgvActivityLogComp_Paging";
                var ClassControlName = "AuditbleEvents_ActivityLog";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.ActivityLogCompCount, pageNumber, 15, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, pageNumber, resultPerPage) {
                    MIPSPreference_IndividualProvider.ActivityLogsComponent($('#pnlAuditbleEventsActivityLog #dgvNewEntry tr.active #rowprofilename').text(), $('#pnlAuditbleEventsActivityLog #dgvNewEntry tr.active #rowpatid').text(), $('#pnlAuditbleEventsActivityLog #dgvNewEntry tr.active #rowdateid').text(), $('#pnlAuditbleEventsActivityLog #dgvNewEntry tr.active #rowuserid').text(), pageNumber, resultPerPage);
                }), 10);
                if (ActivityLogUser_JSON.length > 0) {
                    $('#pnlAuditbleEventsActivityLog #dgvUser tbody tr:first').click();
                }
                else {
                    MIPSPreference_IndividualProvider.GridLoad(ActivityLogUser_JSON, response.ActivityLogCompCount, "Changes", " #pnlAuditbleEventsActivityLog #dgvChanges");
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
        MIPSPreference_IndividualProvider.searchActivityLogChanges_DBCall(ColumnKeyId, ProfileName, DateAndTime, pageNumber, rowsPerPage).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {


                var ActivityLogUser_JSON = JSON.parse(response.ActivityLogChangesLoad_JSON);
                MIPSPreference_IndividualProvider.GridLoad(ActivityLogUser_JSON, response.ActivityLogChangesCount, "Changes", " #pnlAuditbleEventsActivityLog #dgvChanges");
                var TableControl = MIPSPreference_IndividualProvider.params.PanelID + " #dgvChanges";
                var PagingPanelControlID = MIPSPreference_IndividualProvider.params.PanelID + " #dgvActivityLogChanges_Paging";
                var ClassControlName = "AuditbleEvents_ActivityLog";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.ActivityLogChangesCount, pageNumber, 15, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, pageNumber, resultPerPage) {
                    MIPSPreference_IndividualProvider.ActivityLogsChanges($('#pnlAuditbleEventsActivityLog #dgvUser tr.active #colkey').text(), $('#pnlAuditbleEventsActivityLog #dgvUser tr.active #profName').text(), $('#pnlAuditbleEventsActivityLog #dgvUser tr.active #datetime').text(), pageNumber, resultPerPage);
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








}
