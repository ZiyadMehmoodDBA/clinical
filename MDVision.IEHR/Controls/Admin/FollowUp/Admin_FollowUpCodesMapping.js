Admin_FollowUpCodesMapping = {

    bIsFirstLoad: true,

    Load: function (params) {

        Admin_FollowUpCodesMapping.params = params;

        if (Admin_FollowUpCodesMapping.bIsFirstLoad) {
            Admin_FollowUpCodesMapping.bIsFirstLoad = false;

            var self = "";
            if (Admin_FollowUpCodesMapping.params["PanelID"] != "pnlAdminFollowUpCodesMapping") {
                self = $("#pnlAdminFollowUpCodesMapping");
                Admin_FollowUpCodesMapping.params["PanelID"] = "pnlAdminFollowUpCodesMapping";
            }
            else
                self = $('#' + Admin_FollowUpCodesMapping.params["PanelID"]);

            self.loadDropDowns(true).done(function () {
                Admin_FollowUpCodesMapping.CodesMappingSearch();
            });

        }
    },

    CodesMappingSearch: function (CodesMappingId, PageNo, rpp) {
        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Practice", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            if ($('#' + Admin_FollowUpCodesMapping.params["PanelID"] + ' #pnlCodesMapping_Result').css("display") == "none") {
                $('#' + Admin_FollowUpCodesMapping.params["PanelID"] + ' #pnlCodesMapping_Result').show();
            }

            var self = $('#pnlAdminFollowUpCodesMapping');
            var myJSON = self.getMyJSON();

            Admin_FollowUpCodesMapping.SearchCodesMapping(myJSON, CodesMappingId, PageNo, rpp).done(function (response) {
                if (response.status != false) {
                    Admin_FollowUpCodesMapping.CodesMappingGridLoad(response);
                    var TableControl = Admin_FollowUpCodesMapping.params["PanelID"] + " #dgvCodesMapping";
                    var PagingPanelControlID = Admin_FollowUpCodesMapping.params["PanelID"] + " #divCodesMappingPaging";
                    var ClassControlName = "Admin_FollowUpCodesMapping";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout(CreatePagination(response.CodesMappingCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        Admin_FollowUpCodesMapping.CodesMappingSearch(PrimaryID, PageNumber, ResultPerPage);
                    }), 10);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else
            utility.DisplayMessages(strMessage, 2);
        //});
    },

    SearchCodesMapping: function (CodesMappingData, CodesMappingId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "CodesMappingData=" + CodesMappingData + "&CodesMappingId=" + CodesMappingId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_CODESMAPPING", "SEARCH_CODESMAPPING");
    },

    CodesMappingGridLoad: function (response) {
        $('#dgvCodesMapping').dataTable().fnDestroy();
        $('#pnlCodesMapping_Result #dgvCodesMapping tbody').find("tr").remove();
        if (response.CodesMappingCount > 0) {
            var CodesMappingLoad_JSON = JSON.parse(response.CodesMappingLoad_JSON);
            $.each(CodesMappingLoad_JSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_FollowUpCodesMapping.CodesMappingEdit('" + item.CodesMappingId + "',event);");
                $row.attr("id", "gvCodesMapping_row" + item.CodesMappingId);
                $row.attr("CodesMappingId", item.CodesMappingId);

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

                //var ClassDisabled = item.CreatedBy == "MDVISION" ? "disabled" : "";

                $row.append('<td style="display:none;">' + item.CodesMappingId + '</td><td><a class="btn btn-xs " href="#" onclick="Admin_FollowUpCodesMapping.CodesMappingDelete(' + item.CodesMappingId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs " href="#" onclick="Admin_FollowUpCodesMapping.CodesMappingEdit(' + item.CodesMappingId + ',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs " href="#" onclick="Admin_FollowUpCodesMapping.CodesMappingActiveInactive(' + item.CodesMappingId + "," + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.ClaimStatusCode + '">' + item.ClaimStatusCode + '</td><td class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.ClaimStatusCategoryCode + '">' + item.ClaimStatusCategoryCode + '</td>' + '</td><td>' + item.Action + '</td><td class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.Reason + '">' + item.Reason + '</td><td class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.Description + '">' + item.Description + '</td>' + '</td>');


                $("#pnlCodesMapping_Result #dgvCodesMapping tbody").last().append($row);
            });
        }
        else {
            $('#dgvCodesMapping').DataTable({
                "language": {
                    "emptyTable": "No Codes Mapping Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable('#dgvCodesMapping'))
            ;
        else
            $("#pnlCodesMapping_Result #dgvCodesMapping").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    CodesMappingAdd: function () {

        var params = [];
        params["CodesMappingId"] = "-1";
        params["mode"] = "Add";
        LoadActionPan('followUpCodesMappingDetail', params);

    },

    CodesMappingEdit: function (CodesMappingId,event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvCodesMapping_row' + CodesMappingId));
        var params = [];
        params["CodesMappingId"] = CodesMappingId;
        params["mode"] = "Edit";
        LoadActionPan('followUpCodesMappingDetail', params);

    },

    CodesMappingDelete: function (CodesMappingId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvCodesMapping_row' + CodesMappingId));
        //AppPrivileges.GetFormPrivileges("Appointment Status", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('1', function () {
                var selectedValue = CodesMappingId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    Admin_FollowUpCodesMapping.DeleteCodesMapping(selectedValue).done(function (response) {
                        if (response.status != false) {
                            var table1 = $('#dgvCodesMapping').DataTable();
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
        //});


    },

    CodesMappingActiveInactive: function (CodesMappingId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        //AppPrivileges.GetFormPrivileges("Appointment Status", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('3', function () {
                var selectedValue = CodesMappingId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    Admin_FollowUpCodesMapping.UpdateCodesMappingActiveInactive(selectedValue, IsActive).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            Admin_FollowUpCodesMapping.CodesMappingSearch('0');

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
        //});

    },

    DeleteCodesMapping: function (CodesMappingId) {
        var data = "CodesMappingId=" + CodesMappingId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_CODESMAPPING", "DELETE_CODESMAPPING");
    },

    UpdateCodesMappingActiveInactive: function (CodesMappingId, IsActive) {
        var data = "CodesMappingId=" + CodesMappingId + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_CODESMAPPING", "UPDATE_CODESMAPPING_ACTIVE_INACTIVE");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}