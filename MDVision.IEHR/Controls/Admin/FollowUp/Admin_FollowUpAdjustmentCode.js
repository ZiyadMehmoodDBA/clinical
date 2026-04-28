Admin_FollowUpAdjustmentCode = {

    bIsFirstLoad: true,

    Load: function (params) {

        Admin_FollowUpAdjustmentCode.params = params;

        if (Admin_FollowUpAdjustmentCode.bIsFirstLoad) {
            Admin_FollowUpAdjustmentCode.bIsFirstLoad = false;

            var self = "";
            if (Admin_FollowUpAdjustmentCode.params["PanelID"] != "pnlAdminFollowUpAdjustmentCode") {
                self = $("#pnlAdminFollowUpAdjustmentCode");
                Admin_FollowUpAdjustmentCode.params["PanelID"] = "pnlAdminFollowUpAdjustmentCode";
            }
            else
                self = $('#' + Admin_FollowUpAdjustmentCode.params["PanelID"]);
            // self = $('#pnlClinicalQuestion');
            //self.loadDropDowns(true);//.done(function () {
            //AppPrivileges.GetFormPrivileges("Question", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            //    if (strMessage == "") {
            Admin_FollowUpAdjustmentCode.AdjustmentCodeSearch();
            //    }
            //});
        }
    },

    AdjustmentCodeSearch: function (AdjustmentId, PageNo, rpp ){
        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Practice", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            if ($('#' + Admin_FollowUpAdjustmentCode.params["PanelID"] + ' #pnlAdjustmentCode_Result').css("display") == "none") {
                $('#' + Admin_FollowUpAdjustmentCode.params["PanelID"] + ' #pnlAdjustmentCode_Result').show();
            }

            var self = $('#pnlAdminFollowUpAdjustmentCode');
            var myJSON = self.getMyJSON();

            Admin_FollowUpAdjustmentCode.SearchAdjustmentCode(myJSON, AdjustmentId, PageNo, rpp).done(function (response) {
                if (response.status != false) {
                    Admin_FollowUpAdjustmentCode.AdjustmentCodeGridLoad(response);

                    var TableControl = "pnlAdminFollowUpAdjustmentCode #dgvAdjustmentCode";
                    var PagingPanelControlID = "pnlAdminFollowUpAdjustmentCode #divFollowUpAdjustmentCodePaging";
                    var ClassControlName = "Admin_FollowUpAdjustmentCode";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout(CreatePagination(response.AdjustmentCodeCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        Admin_FollowUpAdjustmentCode.AdjustmentCodeSearch(PrimaryID, PageNumber, ResultPerPage);
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

    SearchAdjustmentCode: function (AdjustmentData, AdjustmentId, pageNo, recordPerPage) {
        if (pageNo == null) {
            pageNo = 1;
        }
        if (recordPerPage == null) {
            recordPerPage = 15;
        }
        var data = "AdjustmentData=" + AdjustmentData + "&AdjustmentId=" + AdjustmentId+ "&pageNo=" + pageNo + "&recordPerPage="+recordPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_ADJUSTMENTCODE", "SEARCH_ADJUSTMENTCODE");
    },

    AdjustmentCodeGridLoad: function (response) {
        $('#dgvAdjustmentCode').dataTable().fnDestroy();
        $('#pnlAdjustmentCode_Result #dgvAdjustmentCode tbody').find("tr").remove();
        if (response.AdjustmentCodeCount > 0) {
            var AdjustmentCodeLoad_JSON = JSON.parse(response.AdjustmentCodeLoad_JSON);
            $.each(AdjustmentCodeLoad_JSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_FollowUpAdjustmentCode.AdjustmentCodeEdit('" + item.AdjustmentId + "',event);");
                $row.attr("id", "gvAdjustmentCode_row" + item.AdjustmentId);
                $row.attr("AdjustmentId", item.AdjustmentId);

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

                $row.append('<td style="display:none;">' + item.AdjustmentId + '</td><td><a class="btn btn-xs ' + ClassDisabled + '" href="#" onclick="Admin_FollowUpAdjustmentCode.AdjustmentCodeDelete(' + item.AdjustmentId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs " href="#" onclick="Admin_FollowUpAdjustmentCode.AdjustmentCodeEdit(' + item.AdjustmentId + ',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs ' + ClassDisabled + '" href="#" onclick="Admin_FollowUpAdjustmentCode.AdjustmentCodeActiveInactive(' + item.AdjustmentId + "," + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.Code + '">' + item.Code + '</td><td class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.Description + '">' + item.Description + '</td>' + '</td><td>' + item.IsActive + '</td>' + '</td>');


                $("#pnlAdjustmentCode_Result #dgvAdjustmentCode tbody").last().append($row);
            });
        }
        else {
            $('#dgvAdjustmentCode').DataTable({
                "language": {
                    "emptyTable": "No Adjustment Code Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable('#dgvAdjustmentCode'))
            ;
        else
            $("#pnlAdjustmentCode_Result #dgvAdjustmentCode").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    AdjustmentCodeAdd: function () {

        var params = [];
        params["AdjustmentId"] = "-1";
        params["mode"] = "Add";
        LoadActionPan('followUpAdjustmentCodeDetail', params);

    },

    AdjustmentCodeEdit: function (AdjustmentId,event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvAdjustmentCode_row' + AdjustmentId));
        var params = [];
        params["AdjustmentId"] = AdjustmentId;
        params["mode"] = "Edit";
        LoadActionPan('followUpAdjustmentCodeDetail', params);

    },

    AdjustmentCodeDelete: function (AdjustmentId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvAdjustmentCode_row' + AdjustmentId));
        //AppPrivileges.GetFormPrivileges("Appointment Status", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('1', function () {
                var selectedValue = AdjustmentId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    Admin_FollowUpAdjustmentCode.DeleteAdjustmentCode(selectedValue).done(function (response) {
                        if (response.status != false) {
                            var table1 = $('#dgAdjustmentCode').DataTable();
                            table1.row('.active').remove().draw(false);
                            utility.DisplayMessages(response.Message, 1);
                            Admin_FollowUpAdjustmentCode.AdjustmentCodeSearch();
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

    AdjustmentCodeActiveInactive: function (AdjustmentId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        //AppPrivileges.GetFormPrivileges("Appointment Status", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('3', function () {
                var selectedValue = AdjustmentId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    Admin_FollowUpAdjustmentCode.UpdateAdjustmentCodeActiveInactive(selectedValue, IsActive).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            Admin_FollowUpAdjustmentCode.AdjustmentCodeSearch('0');

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

    DeleteAdjustmentCode: function (AdjustmentId) {
        var data = "AdjustmentId=" + AdjustmentId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_ADJUSTMENTCODE", "DELETE_ADJUSTMENTCODE");
    },

    UpdateAdjustmentCodeActiveInactive: function (AdjustmentId, IsActive) {
        var data = "AdjustmentId=" + AdjustmentId + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_ADJUSTMENTCODE", "UPDATE_ADJUSTMENTCODE_ACTIVE_INACTIVE");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}