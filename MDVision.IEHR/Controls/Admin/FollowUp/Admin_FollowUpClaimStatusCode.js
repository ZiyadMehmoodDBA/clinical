Admin_FollowUpClaimStatusCode = {

    bIsFirstLoad: true,

    Load: function (params) {

        Admin_FollowUpClaimStatusCode.params = params;

        if (Admin_FollowUpClaimStatusCode.bIsFirstLoad) {
            Admin_FollowUpClaimStatusCode.bIsFirstLoad = false;

            var self = "";
            if (Admin_FollowUpClaimStatusCode.params["PanelID"] != "pnlAdminFollowUpClaimStatusCode") {
                self = $("#pnlAdminFollowUpClaimStatusCode");
                Admin_FollowUpClaimStatusCode.params["PanelID"] = "pnlAdminFollowUpClaimStatusCode";
            }
            else
                self = $('#' + Admin_FollowUpClaimStatusCode.params["PanelID"]);
            Admin_FollowUpClaimStatusCode.ClaimStatusCodeSearch();
        }
    },

    ClaimStatusCodeSearch: function (CSCodeId, PageNo, rpp) {
        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Practice", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            if ($('#' + Admin_FollowUpClaimStatusCode.params["PanelID"] + ' #pnlClaimStatusCode_Result').css("display") == "none") {
                $('#' + Admin_FollowUpClaimStatusCode.params["PanelID"] + ' #pnlClaimStatusCode_Result').show();
            }

            var self = $('#pnlAdminFollowUpClaimStatusCode');
            var myJSON = self.getMyJSON();

            Admin_FollowUpClaimStatusCode.SearchClaimStatusCode(myJSON, CSCodeId, PageNo, rpp).done(function (response) {
                if (response.status != false) {
                    Admin_FollowUpClaimStatusCode.ClaimStatusCodeGridLoad(response);
                    var TableControl = Admin_FollowUpClaimStatusCode.params["PanelID"] + " #dgvClaimStatusCode";
                    var PagingPanelControlID = Admin_FollowUpClaimStatusCode.params["PanelID"] + " #divCSCodePaging";
                    var ClassControlName = "Admin_FollowUpClaimStatusCode";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout(CreatePagination(response.ClaimStatusCodeCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        Admin_FollowUpClaimStatusCode.ClaimStatusCodeSearch(PrimaryID, PageNumber, ResultPerPage);
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

    SearchClaimStatusCode: function (RemittanceData, CSCodeId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "RemittanceData=" + RemittanceData + "&CSCodeId=" + CSCodeId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_CLAIMSTATUSCODE", "SEARCH_CLAIMSTATUSCODE");
    },

    ClaimStatusCodeGridLoad: function (response) {
        $('#dgvClaimStatusCode').dataTable().fnDestroy();
        $('#pnlClaimStatusCode_Result #dgvClaimStatusCode tbody').find("tr").remove();
        if (response.ClaimStatusCodeCount > 0) {
            var ClaimStatusCodeLoad_JSON = JSON.parse(response.ClaimStatusCodeLoad_JSON);
            $.each(ClaimStatusCodeLoad_JSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_FollowUpClaimStatusCode.ClaimStatusCodeEdit('" + item.CSCodeId + "',event);");
                $row.attr("id", "gvClaimStatusCode_row" + item.CSCodeId);
                $row.attr("CSCodeId", item.CSCodeId);

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

                $row.append('<td style="display:none;">' + item.CSCodeId + '</td><td><a class="btn btn-xs ' + ClassDisabled + '" href="#" onclick="Admin_FollowUpClaimStatusCode.ClaimStatusCodeDelete(' + item.CSCodeId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs " href="#" onclick="Admin_FollowUpClaimStatusCode.ClaimStatusCodeEdit(' + item.CSCodeId + ',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs ' + ClassDisabled + '" href="#" onclick="Admin_FollowUpClaimStatusCode.ClaimStatusCodeActiveInactive(' + item.CSCodeId + "," + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.Code + '</td><td class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.Description + '">' + item.Description + '</td>' + '</td><td>' + item.IsActive + '</td>' + '</td>');


                $("#pnlClaimStatusCode_Result #dgvClaimStatusCode tbody").last().append($row);
            });
        }
        else {
            $('#dgvClaimStatusCode').DataTable({
                "language": {
                    "emptyTable": "No Claim Status Code Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }

        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });

        if ($.fn.dataTable.isDataTable('#dgvClaimStatusCode'))
            ;
        else
            $("#pnlClaimStatusCode_Result #dgvClaimStatusCode").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    ClaimStatusCodeAdd: function () {

        var params = [];
        params["CSCodeId"] = "-1";
        params["mode"] = "Add";
        LoadActionPan('followUpClaimStatusCodeDetail', params);

    },

    ClaimStatusCodeEdit: function (CSCodeId,event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvClaimStatusCode_row' + CSCodeId));
        var params = [];
        params["CSCodeId"] = CSCodeId;
        params["mode"] = "Edit";
        LoadActionPan('followUpClaimStatusCodeDetail', params);

    },

    ClaimStatusCodeDelete: function (CSCodeId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvClaimStatusCode_row' + CSCodeId));
        //AppPrivileges.GetFormPrivileges("Appointment Status", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('1', function () {
                var selectedValue = CSCodeId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    Admin_FollowUpClaimStatusCode.DeleteClaimStatusCode(selectedValue).done(function (response) {
                        if (response.status != false) {
                            var table1 = $('#dgvClaimStatusCode').DataTable();
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

    ClaimStatusCodeActiveInactive: function (CSCodeId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        //AppPrivileges.GetFormPrivileges("Appointment Status", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('3', function () {
                var selectedValue = CSCodeId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    Admin_FollowUpClaimStatusCode.UpdateClaimStatusCodeActiveInactive(selectedValue, IsActive).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            Admin_FollowUpClaimStatusCode.ClaimStatusCodeSearch('0');

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

    DeleteClaimStatusCode: function (CSCodeId) {
        var data = "CSCodeId=" + CSCodeId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_CLAIMSTATUSCODE", "DELETE_CLAIMSTATUSCODE");
    },

    UpdateClaimStatusCodeActiveInactive: function (CSCodeId, IsActive) {
        var data = "CSCodeId=" + CSCodeId + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_CLAIMSTATUSCODE", "UPDATE_CLAIMSTATUSCODE_ACTIVE_INACTIVE");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}