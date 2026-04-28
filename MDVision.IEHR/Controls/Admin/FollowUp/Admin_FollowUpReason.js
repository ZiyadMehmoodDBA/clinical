Admin_FollowUpReason = {

    bIsFirstLoad: true,

    Load: function (params) {

        Admin_FollowUpReason.params = params;

        if (Admin_FollowUpReason.bIsFirstLoad) {
            Admin_FollowUpReason.bIsFirstLoad = false;

            //var self = $('#pnlAdminFollowUpReason');
            //self.loadDropDowns(true);

            //AppPrivileges.GetFormPrivileges("Section", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            //if (strMessage == "") {
            //Admin_FollowUpReason.ReasonSearch();
            //}
            //});
            var self = "";
            if (Admin_FollowUpReason.params["PanelID"] != "pnlAdminFollowUpReason") {
                self = $("#pnlAdminFollowUpReason");
                Admin_FollowUpReason.params["PanelID"] = "pnlAdminFollowUpReason";
            }
            else
                self = $('#' + Admin_FollowUpReason.params["PanelID"]);
            // self = $('#pnlClinicalQuestion');
            self.loadDropDowns(true);//.done(function () {
            //AppPrivileges.GetFormPrivileges("Question", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            //    if (strMessage == "") {
            Admin_FollowUpReason.ReasonSearch();
            //    }
            //});
        }
    },

    ReasonSearch: function (reasonId, PageNo, rpp) {
        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Practice", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            if ($('#' + Admin_FollowUpReason.params["PanelID"] + ' #pnlReason_Result').css("display") == "none") {
                $('#' + Admin_FollowUpReason.params["PanelID"] + ' #pnlReason_Result').show();
            }

            var self = $('#pnlAdminFollowUpReason');
            var myJSON = self.getMyJSON();

            Admin_FollowUpReason.SearchReason(myJSON, reasonId, PageNo, rpp).done(function (response) {
                if (response.status != false) {
                    Admin_FollowUpReason.ReasonGridLoad(response);
                    var TableControl = Admin_FollowUpReason.params["PanelID"] + " #dgvReason";
                    var PagingPanelControlID = Admin_FollowUpReason.params["PanelID"] + " #divReasonPaging";
                    var ClassControlName = "Admin_FollowUpReason";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout(CreatePagination(response.ReasonCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        Admin_FollowUpReason.ReasonSearch(PrimaryID, PageNumber, ResultPerPage);
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

    SearchReason: function (reasonData, reasonId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "reasonData=" + reasonData + "&reasonId=" + reasonId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUPREASON", "SEARCH_REASON");
    },

    ReasonGridLoad: function (response) {
        $('#' + Admin_FollowUpReason.params["PanelID"] + ' #dgvReason').dataTable().fnDestroy();
        $('#' + Admin_FollowUpReason.params["PanelID"] + ' #pnlReason_Result #dgvReason tbody').find("tr").remove();
        if (response.ReasonCount > 0) {
            var ReasonLoad_JSON = JSON.parse(response.ReasonLoad_JSON);
            $.each(ReasonLoad_JSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_FollowUpReason.ReasonEdit('" + item.ReasonId + "','Edit',event);");
                $row.attr("id", "gvPractice_row" + item.ReasonId);
                $row.attr("ReasonId", item.ReasonId);
                $row.attr("ShortName", item.ShortName);
                $row.attr("Description", item.Description);
                $row.attr("PhoneNo", item.ARTypeId);
                $row.attr("ARType", item.ARType);
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

                $row.append('<td style="display:none;">' + item.ReasonId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_FollowUpReason.ReasonDelete(' + item.ReasonId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_FollowUpReason.ReasonEdit(' + item.ReasonId + ',"Edit",event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_FollowUpReason.ReasonActiveInactive(' + item.ReasonId + "," + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td class="ellip30" data-toggle="tooltip" data-placement="left" title="' + item.ShortName + '">' + item.ShortName + '</td><td class="ellip150" data-toggle="tooltip" data-placement="left" title="' + item.Description + '">' + item.Description + '</td>' + '</td><td class="ellip50" data-toggle="tooltip" data-placement="left" title="' + item.ARType + '">' + item.ARType + '</td>' + '</td><td>' + item.IsActive + '</td>');

                $("#pnlReason_Result #dgvReason tbody").last().append($row);
            });
        }
        else {
            $('#' + Admin_FollowUpReason.params["PanelID"] + ' #dgvReason').DataTable({
                "language": {
                    "emptyTable": "No Reason Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable('#' + Admin_FollowUpReason.params["PanelID"] + ' #dgvReason'))
            ;
        else
            $('#' + Admin_FollowUpReason.params["PanelID"] + ' #pnlReason_Result #dgvReason').DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        //$('#pnlReason_Result #dgvReason_info').html("Total Records: " + response.ReasonCount);
    },

    ReasonDelete: function (reasonId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvPractice_row' + reasonId));
        //AppPrivileges.GetFormPrivileges("Patient Family", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        utility.myConfirm('1', function () {
            var selectedValue = reasonId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Admin_FollowUpReason.DeleteReason(selectedValue).done(function (response) {
                    if (response.status != false) {

                        Admin_FollowUpReason.ReasonSearch();
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
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
    },

    DeleteReason: function (reasonId) {
        var data = "reasonId=" + reasonId;
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUPREASON", "DELETE_REASON");
    },

    ReasonEdit: function (reasonId, mode, event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvPractice_row' + reasonId));
        var params = [];
        params["reasonId"] = reasonId;
        params["mode"] = mode;
        LoadActionPan('followUpReasonDetail', params);
    },

    ReasonActiveInactive: function (reasonId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        // AppPrivileges.GetFormPrivileges("Demographic", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //  if (strMessage == "") {
        utility.myConfirm('3', function () {
            var selectedValue = reasonId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Admin_FollowUpReason.ReasonUpdateActiveInactive(selectedValue, IsActive).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        Admin_FollowUpReason.ReasonSearch("0");
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { },
           '3', null, null, null, IsActive
        );
        // }
        //  else
        //   utility.DisplayMessages(strMessage, 2);
        // });
    },

    ReasonUpdateActiveInactive: function (reasonId, IsActive) {
        var data = "reasonId=" + reasonId + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUPREASON", "UPDATE_REASON_ACTIVE_INACTIVE");
    },

    ReasonAdd: function () {
        var params = [];
        params["ReasonId"] = "-1";
        params["mode"] = "Add";
        LoadActionPan('followUpReasonDetail', params);
    },

    SelectGridRow: function (obj) {
        if (obj.className != 'active') {
            $(obj).parent().children().removeClass('active');
            $(obj).addClass('active');
        }
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}