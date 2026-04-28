
Admin_FeeGroup = {
    bIsFirstLoad: true,
    params:null,
    Load: function (params) {
        Admin_FeeGroup.params = params;
        if (Admin_FeeGroup.bIsFirstLoad) {
            Admin_FeeGroup.bIsFirstLoad = false;
            
            var self = null;
            if (Admin_FeeGroup.params.PanelID == "pnlAdminFeeGroup")
                self = $('#pnlAdminFeeGroup');
            else
                self = $('#' + Admin_FeeGroup.params.PanelID + ' #pnlAdminFeeGroup');

            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#ddlEntity").attr('disabled', 'disabled');
            }
            self.loadDropDowns(true).done(function () {                
                //if (globalAppdata['IsAdmin'] != "True") {
                //    $("#pnlFeeGroup_Search #divFeeGroup_Entity").css("display", "none");
                //    $("#pnlFeeGroup_Search #ddlEntity").val(globalAppdata["SeletedEntityId"]);
                //}
                if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                    self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
                }
                Admin_FeeGroup.FeeGroupSearch();
            });
        }
    },

    FeeGroupAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Fee Group", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FeeGroupId"] = null;
                params["mode"] = "Add";

                LoadActionPan('feegroupDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    FeeGroupEdit: function (FeeGroupId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvFeeGroup_row' + FeeGroupId));
        AppPrivileges.GetFormPrivileges("Fee Group", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = FeeGroupId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["FeeGroupId"] = selectedValue;
                    params["mode"] = "Edit";
                    LoadActionPan('feegroupDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    FeeGroupDelete: function (FeeGroupId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvFeeGroup_row' + FeeGroupId));
        AppPrivileges.GetFormPrivileges("Fee Group", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = FeeGroupId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_FeeGroup.DeleteFeeGroup(selectedValue).done(function (response) {
                            if (response.status != false) {
                                Admin_FeeGroup.FeeGroupSearch();
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
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    FeeGroupActiveInactive: function (FeeGroupId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Fee Group", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = FeeGroupId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        feegroupDetail.UpdateFeeGroupActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_FeeGroup.FeeGroupSearch('0');
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

    FeeGroupSearch: function (FeeGroupId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Fee Group", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminFeeGroup #pnlFeeGroup_Result").css("display") == "none") {
                    $("#pnlAdminFeeGroup #pnlFeeGroup_Result").show();
                }

                var self = $("#pnlFeeGroup_Search");
                var myJSON = self.getMyJSON();

                Admin_FeeGroup.SearchFeeGroup(myJSON, FeeGroupId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_FeeGroup.FeeGroupGridLoad(response);
                        var TableControl = "pnlAdminFeeGroup #dgvFeeGroup";
                        var PagingPanelControlID = "pnlAdminFeeGroup #divFeeGroupPaging";
                        var ClassControlName = "Admin_FeeGroup";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.FeeGroupCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_FeeGroup.FeeGroupSearch(PrimaryID, PageNumber, ResultPerPage);
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

    FeeGroupGridLoad: function (response) {
        $("#dgvFeeGroup").dataTable().fnDestroy();
        $("#pnlFeeGroup_Result #dgvFeeGroup tbody").find("tr").remove();
        if (response.FeeGroupCount > 0) {
            var FeeGroupLoadJSONData = JSON.parse(response.FeeGroupLoad_JSON);
            $.each(FeeGroupLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_FeeGroup.FeeGroupEdit('" + item.FeeGroupId + "',event);");
                $row.attr("id", "gvFeeGroup_row" + item.FeeGroupId);
                $row.attr("FeeGroupId", item.FeeGroupId);

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
                $row.append('<td style="display:none;">' + item.FeeGroupId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_FeeGroup.FeeGroupDelete(' + item.FeeGroupId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_FeeGroup.FeeGroupEdit(' + item.FeeGroupId + ',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_FeeGroup.FeeGroupActiveInactive(' + item.FeeGroupId + ', ' + isactive + ',event);" title="'+activeTitle+'"><i class="' + tglclass + '"></i></a></td><td>' + item.ShortName + '</td><td>' + item.Description + '</td>');

                $("#pnlFeeGroup_Result #dgvFeeGroup tbody").last().append($row);
            });
        }
        else {
            $('#dgvFeeGroup').DataTable({
                "language": {
                    "emptyTable": "No Fee Group Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvFeeGroup'))
            ;
        else
            $("#pnlFeeGroup_Result #dgvFeeGroup").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchFeeGroup: function (FeeGroupData, FeeGroupId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "FeeGroupData=" + FeeGroupData + "&FeeGroupID=" + FeeGroupId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FEE_GROUP", "SEARCH_FEE_GROUP");
    },

    DeleteFeeGroup: function (FeeGroupId) {
        var data = "FeeGroupID=" + FeeGroupId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FEE_GROUP_DETAIL", "DELETE_FEE_GROUP");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}