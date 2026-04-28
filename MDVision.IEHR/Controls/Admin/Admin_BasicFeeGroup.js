
Admin_BasicFeeGroup = {
    bIsFirstLoad: true,
    params:null,
    Load: function (params) {
        Admin_BasicFeeGroup.params = params;
        if (Admin_BasicFeeGroup.bIsFirstLoad) {
            Admin_BasicFeeGroup.bIsFirstLoad = false;
            
            var self = null;
            if (Admin_BasicFeeGroup.params.PanelID == "pnlAdminBasicFeeGroup")
                self = $('#pnlAdminBasicFeeGroup');
            else
                self = $('#' + Admin_BasicFeeGroup.params.PanelID + ' #pnlAdminBasicFeeGroup');

            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#ddlEntity").attr('disabled', 'disabled');
            }
            self.loadDropDowns(true).done(function () {               
                //if (globalAppdata['IsAdmin'] != "True") {
                //    $("#pnlBasicFeeGroup_Search #divBasicFeeGroup_Entity").css("display", "none");
                //    $("#pnlBasicFeeGroup_Search #ddlEntity").val(globalAppdata["SeletedEntityId"]);
                //}
                if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                    self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
                }
                Admin_BasicFeeGroup.BasicFeeGroupSearch();
            });
        }
    },

    BasicFeeGroupAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Basic Fee Group", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["BasicFeeGroupId"] = null;
                params["mode"] = "Add";

                LoadActionPan('basicfeegroupDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    BasicFeeGroupEdit: function (BasicFeeGroupId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvBasicFeeGroup_row' + BasicFeeGroupId));
        AppPrivileges.GetFormPrivileges("Basic Fee Group", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = BasicFeeGroupId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["BasicFeeGroupId"] = selectedValue;
                    params["mode"] = "Edit";
                    LoadActionPan('basicfeegroupDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    BasicFeeGroupDelete: function (BasicFeeGroupId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvBasicFeeGroup_row' + BasicFeeGroupId));
        AppPrivileges.GetFormPrivileges("Basic Fee Group", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = BasicFeeGroupId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_BasicFeeGroup.DeleteBasicFeeGroup(selectedValue).done(function (response) {
                            if (response.status != false) {
                                //var table1 = $('#dgvBasicFeeGroup').DataTable();
                                //table1.row('.active').remove().draw(false);
                                Admin_BasicFeeGroup.BasicFeeGroupSearch();
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

    BasicFeeGroupActiveInactive: function (BasicFeeGroupId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Basic Fee Group", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = BasicFeeGroupId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        basicfeegroupDetail.UpdateBasicFeeGroupActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_BasicFeeGroup.BasicFeeGroupSearch('0');
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

    BasicFeeGroupSearch: function (BasicFeeGroupId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Basic Fee Group", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminBasicFeeGroup #pnlBasicFeeGroup_Result").css("display") == "none") {
                    $("#pnlAdminBasicFeeGroup #pnlBasicFeeGroup_Result").show();
                }

                var self = $("#pnlBasicFeeGroup_Search");
                var myJSON = self.getMyJSON();

                Admin_BasicFeeGroup.SearchBasicFeeGroup(myJSON, BasicFeeGroupId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_BasicFeeGroup.BasicFeeGroupGridLoad(response);
                        var TableControl = "pnlAdminBasicFeeGroup #dgvBasicFeeGroup";
                        var PagingPanelControlID = "pnlAdminBasicFeeGroup #divBasicFeeGroupPaging";
                        var ClassControlName = "Admin_BasicFeeGroup";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.BasicFeeGroupCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_BasicFeeGroup.BasicFeeGroupSearch(PrimaryID, PageNumber, ResultPerPage);
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

    BasicFeeGroupGridLoad: function (response) {
        $("#dgvBasicFeeGroup").dataTable().fnDestroy();
        $("#pnlBasicFeeGroup_Result #dgvBasicFeeGroup tbody").find("tr").remove();
        if (response.BasicFeeGroupCount > 0) {
            var BasicFeeGroupLoadJSONData = JSON.parse(response.BasicFeeGroupLoad_JSON);
            $.each(BasicFeeGroupLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_BasicFeeGroup.BasicFeeGroupEdit('" + item.BasicFeeGroupId + "',event);");
                $row.attr("id", "gvBasicFeeGroup_row" + item.BasicFeeGroupId);
                $row.attr("BasicFeeGroupId", item.BasicFeeGroupId);

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
                $row.append('<td style="display:none;">' + item.BasicFeeGroupId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_BasicFeeGroup.BasicFeeGroupDelete(' + item.BasicFeeGroupId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_BasicFeeGroup.BasicFeeGroupEdit(' + item.BasicFeeGroupId + ',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_BasicFeeGroup.BasicFeeGroupActiveInactive(' + item.BasicFeeGroupId + ', ' + isactive + ',event);" title="'+activeTitle+'"><i class="' + tglclass + '"></i></a></td><td>' + item.ShortName + '</td><td>' + item.Description + '</td>');

                //else
                //    $row.append('<td style="display:none;">' + item.ProcCategoryId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_ProcedureCategory.ProcedureCategoryDelete(' + item.ProcCategoryId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_ProcedureCategory.ProcedureCategoryEdit(' + item.ProcCategoryId + ');" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_ProcedureCategory.ProcedureCategoryActiveInactive(' + item.ProcCategoryId + ', 1);" title="Active Record"><i class="fa fa-toggle-on green"></i></a></td><td>' + item.Name + '</td><td>' + item.Description + '</td>');

                $("#pnlBasicFeeGroup_Result #dgvBasicFeeGroup tbody").last().append($row);
            });
        }
        else {
            $('#dgvBasicFeeGroup').DataTable({
                "language": {
                    "emptyTable": "No Basic Fee Group Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvBasicFeeGroup'))
            ;
        else
            $("#pnlBasicFeeGroup_Result #dgvBasicFeeGroup").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchBasicFeeGroup: function (BasicFeeGroupData, BasicFeeGroupId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "BasicFeeGroupData=" + BasicFeeGroupData + "&BasicFeeGroupID=" + BasicFeeGroupId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_BASIC_FEE_GROUP", "SEARCH_BASIC_FEE_GROUP");
    },

    DeleteBasicFeeGroup: function (BasicFeeGroupId) {
        var data = "BasicFeeGroupID=" + BasicFeeGroupId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_BASIC_FEE_GROUP_DETAIL", "DELETE_BASIC_FEE_GROUP");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}