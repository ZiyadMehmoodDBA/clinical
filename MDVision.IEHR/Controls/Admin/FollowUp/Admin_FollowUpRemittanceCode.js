Admin_FollowUpRemittanceCode = {

    bIsFirstLoad: true,

    Load: function (params) {

        Admin_FollowUpRemittanceCode.params = params;

        if (Admin_FollowUpRemittanceCode.bIsFirstLoad) {
            Admin_FollowUpRemittanceCode.bIsFirstLoad = false;

            var self = "";
            if (Admin_FollowUpRemittanceCode.params["PanelID"] != "pnlAdminFollowUpRemittanceCode") {
                self = $("#pnlAdminFollowUpRemittanceCode");
                Admin_FollowUpRemittanceCode.params["PanelID"] = "pnlAdminFollowUpRemittanceCode";
            }
            else
                self = $('#' + Admin_FollowUpRemittanceCode.params["PanelID"]);
            // self = $('#pnlClinicalQuestion');
            //self.loadDropDowns(true);//.done(function () {
            AppPrivileges.GetFormPrivileges("Remittance Code", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_FollowUpRemittanceCode.RemittanceCodeSearch();
                }
            });
        }
    },

    RemittanceCodeSearch: function (RemittanceId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Remittance Code", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($('#' + Admin_FollowUpRemittanceCode.params["PanelID"] + ' #pnlRemittanceCode_Result').css("display") == "none") {
                    $('#' + Admin_FollowUpRemittanceCode.params["PanelID"] + ' #pnlRemittanceCode_Result').show();
                }

                var self = $('#pnlAdminFollowUpRemittanceCode');
                var myJSON = self.getMyJSON();

                Admin_FollowUpRemittanceCode.SearchRemittanceCode(myJSON, RemittanceId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_FollowUpRemittanceCode.RemittanceCodeGridLoad(response);
                        var TableControl = Admin_FollowUpRemittanceCode.params["PanelID"] + " #dgvRemittanceCode";
                        var PagingPanelControlID = Admin_FollowUpRemittanceCode.params["PanelID"] + " #divRemittancePaging";
                        var ClassControlName = "Admin_FollowUpRemittanceCode";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.RemittanceCodeCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_FollowUpRemittanceCode.RemittanceCodeSearch(PrimaryID, PageNumber, ResultPerPage);
                        }), 10);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    SearchRemittanceCode: function (RemittanceData, RemittanceId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "RemittanceData=" + RemittanceData + "&RemittanceId=" + RemittanceId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_REMITTANCECODE", "SEARCH_REMITTANCECODE");
    },

    RemittanceCodeGridLoad: function (response) {
        $('#dgvRemittanceCode').dataTable().fnDestroy();
        $('#pnlRemittanceCode_Result #dgvRemittanceCode tbody').find("tr").remove();
        if (response.RemittanceCodeCount > 0) {
            var RemittanceCodeLoad_JSON = JSON.parse(response.RemittanceCodeLoad_JSON);
            $.each(RemittanceCodeLoad_JSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_FollowUpRemittanceCode.RemittanceCodeEdit('" + item.RemittanceId + "',event);");
                $row.attr("id", "gvRemittanceCode_row" + item.RemittanceId);
                $row.attr("RemittanceId", item.RemittanceId);

                if (item.IsActive == "True") {
                    isactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }

                var ClassDisabled = item.CreatedBy == "MDVISION" ? "disabled" : "";

                $row.append('<td style="display:none;">' + item.RemittanceId + '</td><td><a class="btn btn-xs ' + ClassDisabled + '" href="#" onclick="Admin_FollowUpRemittanceCode.RemittanceCodeDelete(' + item.RemittanceId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs " href="#" onclick="Admin_FollowUpRemittanceCode.RemittanceCodeEdit(' + item.RemittanceId + ',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs ' + ClassDisabled + '" href="#" onclick="Admin_FollowUpRemittanceCode.RemittanceCodeActiveInactive(' + item.RemittanceId + "," + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.Code + '">' + item.Code + '</td><td class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.Description + '">' + item.Description + '</td>' + '</td><td class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.Rejection + '">' + item.Rejection + '</td>' + '</td>');


                $("#pnlRemittanceCode_Result #dgvRemittanceCode tbody").last().append($row);
            });
        }
        else {
            $('#dgvRemittanceCode').DataTable({
                "language": {
                    "emptyTable": "No Remittance Code Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable('#dgvRemittanceCode'))
            ;
        else
            $("#pnlRemittanceCode_Result #dgvRemittanceCode").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    RemittanceCodeAdd: function () {

        var params = [];
        params["RemittanceId"] = "-1";
        params["mode"] = "Add";
        LoadActionPan('followUpRemittanceCodeDetail', params);

    },

    RemittanceCodeEdit: function (RemittanceId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvRemittanceCode_row' + RemittanceId));
        var params = [];
        params["RemittanceId"] = RemittanceId;
        params["mode"] = "Edit";
        LoadActionPan('followUpRemittanceCodeDetail', params);

    },

    RemittanceCodeDelete: function (RemittanceId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvRemittanceCode_row' + RemittanceId));
        AppPrivileges.GetFormPrivileges("Remittance Code", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = RemittanceId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_FollowUpRemittanceCode.DeleteRemittanceCode(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#dgvRemittanceCode').DataTable();
                                table1.row('.active').remove().draw(false);
                                utility.DisplayMessages(response.Message, 1);

                                //CacheManager.BindCodes('GetAppointmentStatus', true);
                                //MDVisionService.lookups("GetAppointmentStatus", true);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                    '1'
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });


    },

    RemittanceCodeActiveInactive: function (RemittanceId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Remittance Code", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = RemittanceId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_FollowUpRemittanceCode.UpdateRemittanceCodeActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.Message, 1);
                                Admin_FollowUpRemittanceCode.RemittanceCodeSearch('0');

                                //CacheManager.BindCodes('GetAppointmentStatus', true);
                                //MDVisionService.lookups("GetAppointmentStatus", true);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                  '3', null, null, null, IsActive
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    DeleteRemittanceCode: function (RemittanceId) {
        var data = "RemittanceId=" + RemittanceId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_REMITTANCECODE", "DELETE_REMITTANCECODE");
    },

    UpdateRemittanceCodeActiveInactive: function (RemittanceId, IsActive) {
        var data = "RemittanceId=" + RemittanceId + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_REMITTANCECODE", "UPDATE_REMITTANCECODE_ACTIVE_INACTIVE");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}