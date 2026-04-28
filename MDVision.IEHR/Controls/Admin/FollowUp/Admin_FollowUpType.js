Admin_FollowUpType = {

    bIsFirstLoad: true,

    Load: function (params) {

        Admin_FollowUpType.params = params;

        if (Admin_FollowUpType.bIsFirstLoad) {
            Admin_FollowUpType.bIsFirstLoad = false;

            //var self = $('#pnlAdminFollowUpType');
            //self.loadDropDowns(true);

            //AppPrivileges.GetFormPrivileges("Section", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            //if (strMessage == "") {
            //Admin_FollowUpType.TypeSearch();
            //}
            //});
            var self = "";
            if (Admin_FollowUpType.params["PanelID"] != "pnlAdminFollowUpType") {
                self = $("#pnlAdminFollowUpType");
                Admin_FollowUpType.params["PanelID"] = "pnlAdminFollowUpType";
            }
            else
                self = $('#' + Admin_FollowUpType.params["PanelID"]);
            // self = $('#pnlClinicalQuestion');
            self.loadDropDowns(true);//.done(function () {
            //AppPrivileges.GetFormPrivileges("Question", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            //    if (strMessage == "") {
            Admin_FollowUpType.TypeSearch();
            //    }
            //});
        }
    },

    TypeSearch: function (TypeId, PageNo, rpp) {
        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Practice", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            if ($('#' + Admin_FollowUpType.params["PanelID"] + ' #pnlType_Result').css("display") == "none") {
                $('#' + Admin_FollowUpType.params["PanelID"] + ' #pnlType_Result').show();
            }

            var self = $('#pnlAdminFollowUpType');
            var myJSON = self.getMyJSON();

            Admin_FollowUpType.SearchType(myJSON, TypeId, PageNo, rpp).done(function (response) {
                if (response.status != false) {
                    Admin_FollowUpType.TypeGridLoad(response);
                    var TableControl = Admin_FollowUpType.params["PanelID"] + " #dgvType";
                    var PagingPanelControlID = Admin_FollowUpType.params["PanelID"] + " #divTypePaging";
                    var ClassControlName = "Admin_FollowUpType";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout(CreatePagination(response.TypeCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        Admin_FollowUpType.TypeSearch(PrimaryID, PageNumber, ResultPerPage);
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

    SearchType: function (TypeData, TypeId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "TypeData=" + TypeData + "&TypeId=" + TypeId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUPTYPE", "SEARCH_TYPE");
    },

    TypeGridLoad: function (response) {
        $('#' + Admin_FollowUpType.params["PanelID"] + ' #dgvType').dataTable().fnDestroy();
        $('#' + Admin_FollowUpType.params["PanelID"] + ' #pnlType_Result #dgvType tbody').find("tr").remove();
        if (response.TypeCount > 0) {
            var TypeLoad_JSON = JSON.parse(response.TypeLoad_JSON);
            $.each(TypeLoad_JSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_FollowUpType.TypeEdit('" + item.ARTypeId + "','Edit'" + ",event);");
                $row.attr("id", "gvPractice_row" + item.ARTypeId);
                $row.attr("TypeId", item.ARTypeId);
                $row.attr("ShortName", item.ShortName);
                $row.attr("Description", item.Description);

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

                $row.append('<td style="display:none;">' + item.ARTypeId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_FollowUpType.TypeDelete(' + item.ARTypeId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_FollowUpType.TypeEdit("' + item.ARTypeId + '","Edit"' + ',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_FollowUpType.TypeActiveInactive(' + item.ARTypeId + "," + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.ShortName + '">' + item.ShortName + '</td><td class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.Description + '">' + item.Description + '</td>' + '</td><td>' + item.IsActive + '</td>');

                $("#pnlType_Result #dgvType tbody").last().append($row);
            });
        }
        else {
            $('#' + Admin_FollowUpType.params["PanelID"] + ' #dgvType').DataTable({
                "language": {
                    "emptyTable": "No Type Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable('#' + Admin_FollowUpType.params["PanelID"] + ' #dgvType'))
            ;
        else
            $('#' + Admin_FollowUpType.params["PanelID"] + ' #pnlType_Result #dgvType').DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        //$('#pnlType_Result #dgvType_info').html("Total Records: " + response.TypeCount);
    },

    TypeDelete: function (TypeId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvPractice_row' + TypeId));
        //AppPrivileges.GetFormPrivileges("Patient Family", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        utility.myConfirm('1', function () {
            var selectedValue = TypeId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Admin_FollowUpType.DeleteType(selectedValue).done(function (response) {
                    if (response.status != false) {

                        Admin_FollowUpType.TypeSearch();
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

    DeleteType: function (TypeId) {
        var data = "TypeId=" + TypeId;
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUPTYPE", "DELETE_TYPE");
    },

    TypeEdit: function (TypeId, mode, event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvPractice_row' + TypeId));
        var params = [];
        params["TypeId"] = TypeId;
        params["mode"] = mode;
        LoadActionPan('followUpTypeDetail', params);
    },

    TypeActiveInactive: function (TypeId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        // AppPrivileges.GetFormPrivileges("Demographic", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //  if (strMessage == "") {
        utility.myConfirm('3', function () {
            var selectedValue = TypeId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Admin_FollowUpType.TypeUpdateActiveInactive(selectedValue, IsActive).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        Admin_FollowUpType.TypeSearch("0");
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

    TypeUpdateActiveInactive: function (TypeId, IsActive) {
        var data = "TypeId=" + TypeId + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUPTYPE", "UPDATE_TYPE_ACTIVE_INACTIVE");
    },

    TypeAdd: function () {
        var params = [];
        params["TypeId"] = "-1";
        params["mode"] = "Add";
        LoadActionPan('followUpTypeDetail', params);
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