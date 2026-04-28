Admin_FollowUpClaimStatusCategoryCode = {

    bIsFirstLoad: true,

    Load: function (params) {

        Admin_FollowUpClaimStatusCategoryCode.params = params;

        if (Admin_FollowUpClaimStatusCategoryCode.bIsFirstLoad) {
            Admin_FollowUpClaimStatusCategoryCode.bIsFirstLoad = false;

            var self = "";
            if (Admin_FollowUpClaimStatusCategoryCode.params["PanelID"] != "pnlAdminFollowUpClaimStatusCategoryCode") {
                self = $("#pnlAdminFollowUpClaimStatusCategoryCode");
                Admin_FollowUpClaimStatusCategoryCode.params["PanelID"] = "pnlAdminFollowUpClaimStatusCategoryCode";
            }
            else
                self = $('#' + Admin_FollowUpClaimStatusCategoryCode.params["PanelID"]);
            Admin_FollowUpClaimStatusCategoryCode.ClaimStatusCategoryCodeSearch();
        }
    },

    ClaimStatusCategoryCodeSearch: function (CSCatCodeId, PageNo, rpp) {
        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Practice", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            if ($('#' + Admin_FollowUpClaimStatusCategoryCode.params["PanelID"] + ' #pnlClaimStatusCategoryCode_Result').css("display") == "none") {
                $('#' + Admin_FollowUpClaimStatusCategoryCode.params["PanelID"] + ' #pnlClaimStatusCategoryCode_Result').show();
            }

            var self = $('#pnlAdminFollowUpClaimStatusCategoryCode');
            var myJSON = self.getMyJSON();

            Admin_FollowUpClaimStatusCategoryCode.SearchClaimStatusCategoryCode(myJSON, CSCatCodeId, PageNo, rpp).done(function (response) {
                if (response.status != false) {
                    Admin_FollowUpClaimStatusCategoryCode.ClaimStatusCategoryCodeGridLoad(response);
                    var TableControl = Admin_FollowUpClaimStatusCategoryCode.params["PanelID"] + " #dgvClaimStatusCategoryCode";
                    var PagingPanelControlID = Admin_FollowUpClaimStatusCategoryCode.params["PanelID"] + " #divCSCatCodePaging";
                    var ClassControlName = "Admin_FollowUpClaimStatusCategoryCode";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout(CreatePagination(response.ClaimStatusCategoryCodeCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        Admin_FollowUpClaimStatusCategoryCode.ClaimStatusCategoryCodeSearch(PrimaryID, PageNumber, ResultPerPage);
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

    SearchClaimStatusCategoryCode: function (ClaimStatusCategoryCodeData, CSCatCodeId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "ClaimStatusCategoryCodeData=" + ClaimStatusCategoryCodeData + "&CSCatCodeId=" + CSCatCodeId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_CLAIMSTATUSCATCODE", "SEARCH_CLAIMSTATUSCATCODE");
    },

    ClaimStatusCategoryCodeGridLoad: function (response) {
        $('#dgvClaimStatusCategoryCode').dataTable().fnDestroy();
        $('#pnlClaimStatusCategoryCode_Result #dgvClaimStatusCategoryCode tbody').find("tr").remove();
        if (response.ClaimStatusCategoryCodeCount > 0) {
            var ClaimStatusCategoryCodeLoad_JSON = JSON.parse(response.ClaimStatusCategoryCodeLoad_JSON);
            $.each(ClaimStatusCategoryCodeLoad_JSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_FollowUpClaimStatusCategoryCode.ClaimStatusCategoryCodeEdit('" + item.CSCatCodeId + "',event);");
                $row.attr("id", "gvClaimStatusCategoryCode_row" + item.CSCatCodeId);
                $row.attr("CSCatCodeId", item.CSCatCodeId);

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

                $row.append('<td style="display:none;">' + item.CSCatCodeId + '</td><td><a class="btn btn-xs ' + ClassDisabled + '" href="#" onclick="Admin_FollowUpClaimStatusCategoryCode.ClaimStatusCategoryCodeDelete(' + item.CSCatCodeId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs " href="#" onclick="Admin_FollowUpClaimStatusCategoryCode.ClaimStatusCategoryCodeEdit(' + item.CSCatCodeId + ',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs ' + ClassDisabled + '" href="#" onclick="Admin_FollowUpClaimStatusCategoryCode.ClaimStatusCategoryCodeActiveInactive(' + item.CSCatCodeId + "," + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.Code + '</td><td class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.Description + '">' + item.Description + '</td>' + '</td><td>' + item.IsActive + '</td>' + '</td>');


                $("#pnlClaimStatusCategoryCode_Result #dgvClaimStatusCategoryCode tbody").last().append($row);
            });
        }
        else {
            $('#dgvClaimStatusCategoryCode').DataTable({
                "language": {
                    "emptyTable": "No Claim Status Category Code Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable('#dgvClaimStatusCategoryCode'))
            ;
        else
            $("#pnlClaimStatusCategoryCode_Result #dgvClaimStatusCategoryCode").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    ClaimStatusCategoryCodeAdd: function () {

        var params = [];
        params["CSCatCodeId"] = "-1";
        params["mode"] = "Add";
        LoadActionPan('followUpClaimStatusCategoryCodeDetail', params);

    },

    ClaimStatusCategoryCodeEdit: function (CSCatCodeId,event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvClaimStatusCategoryCode_row' + CSCatCodeId));
        var params = [];
        params["CSCatCodeId"] = CSCatCodeId;
        params["mode"] = "Edit";
        LoadActionPan('followUpClaimStatusCategoryCodeDetail', params);

    },

    ClaimStatusCategoryCodeDelete: function (CSCatCodeId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvClaimStatusCategoryCode_row' + CSCatCodeId));
        //AppPrivileges.GetFormPrivileges("Appointment Status", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('1', function () {
                var selectedValue = CSCatCodeId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    Admin_FollowUpClaimStatusCategoryCode.DeleteClaimStatusCategoryCode(selectedValue).done(function (response) {
                        if (response.status != false) {
                            var table1 = $('#dgvClaimStatusCategoryCode').DataTable();
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

    ClaimStatusCategoryCodeActiveInactive: function (CSCatCodeId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        //AppPrivileges.GetFormPrivileges("Appointment Status", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('3', function () {
                var selectedValue = CSCatCodeId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    Admin_FollowUpClaimStatusCategoryCode.UpdateClaimStatusCategoryCodeActiveInactive(selectedValue, IsActive).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            Admin_FollowUpClaimStatusCategoryCode.ClaimStatusCategoryCodeSearch('0');

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

    DeleteClaimStatusCategoryCode: function (CSCatCodeId) {
        var data = "CSCatCodeId=" + CSCatCodeId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_CLAIMSTATUSCATCODE", "DELETE_CLAIMSTATUSCATCODE");
    },

    UpdateClaimStatusCategoryCodeActiveInactive: function (CSCatCodeId, IsActive) {
        var data = "CSCatCodeId=" + CSCatCodeId + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_CLAIMSTATUSCATCODE", "UPDATE_CLAIMSTATUSCATCODE_ACTIVE_INACTIVE");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}