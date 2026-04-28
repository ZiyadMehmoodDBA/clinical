VisitTypeDurationGroup = {
    params: [],
    bIsFirstLoad: true,
    Load: function (params) {
        VisitTypeDurationGroup.params = params;

        if (VisitTypeDurationGroup.bIsFirstLoad) {
            VisitTypeDurationGroup.bIsFirstLoad = false;
            var self = $('#pnlAdminVisitTypeDurationGroup');
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                $("#" + VisitTypeDurationGroup.params["PanelID"] + " #ddlEntity").attr('disabled', 'disabled');
            }
            
            self.loadDropDowns(true).done(function () {
                if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                    $("#" + VisitTypeDurationGroup.params["PanelID"] + " #ddlEntity").val(globalAppdata["SeletedEntityId"]);
                }

            });
        }
        AppPrivileges.GetFormPrivileges("VisitTypeDurationGroup", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            VisitTypeDurationGroup.VisitTypeDurationGroupSearch();
        });
    },

    VisitTypeDurationGroupGridLoad: function (response) {
        $("#" + VisitTypeDurationGroup.params["PanelID"] + " #dgvVisitTypeDurationGroup").dataTable().fnDestroy();
        $("#" + VisitTypeDurationGroup.params["PanelID"] + " #pnlVisitTypeDurationGroup_Result #dgvVisitTypeDurationGroup tbody").find("tr").remove();
        if (response.VisitTypeDurationGroupCount > 0) {
            var VisitTypeLoadJSONData = JSON.parse(response.VisitTypeDurationGroupLoad_JSON);
            $.each(VisitTypeLoadJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", "gvVisitTypeDurationGroup_row" + item.Id);
                $row.attr("VisitTypeDurationGroupId", item.Id);

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
                $row.append('<td style="display:none;">' + item.Id + '</td><td><a class="btn btn-xs" href="#" onclick="VisitTypeDurationGroup.VisitTypeDurationGroupDelete(\'' + item.Id + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="VisitTypeDurationGroup.VisitTypeDurationGroupEdit(\'' + item.Id + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="VisitTypeDurationGroup.VisitTypeActiveInactive(\'' + item.Id + '\', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.Name + '</td><td>' + item.EntityName + '</td>');
                $("#pnlVisitTypeDurationGroup_Result #dgvVisitTypeDurationGroup tbody").last().append($row);
            });
        }
        else {
            $('#dgvVisitTypeDurationGroup').DataTable({
                "language": {
                    "emptyTable": "No Record Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvVisitTypeDurationGroup'))
            ;
        else
            $("#" + VisitTypeDurationGroup.params.PanelID + " #pnlVisitTypeDurationGroup_Result #dgvVisitTypeDurationGroup").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] });

    },

    VisitTypeDurationGroupSearch: function (VisitTypeDurationGroupId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("VisitTypeDurationGroup", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#" + VisitTypeDurationGroup.params["PanelID"] + " #pnlVisitTypeDurationGroup_Result").css("display") == "none") {
                    $("#" + VisitTypeDurationGroup.params["PanelID"] + " #pnlVisitTypeDurationGroup_Result").show();
                }

                var self = $("#" + VisitTypeDurationGroup.params["PanelID"] + " #pnlVisitTypeDurationGroup_Search");
                var myJSON = self.getMyJSON();

                VisitTypeDurationGroup.SearchVisitTypeDurationGroup(myJSON, VisitTypeDurationGroupId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        VisitTypeDurationGroup.VisitTypeDurationGroupGridLoad(response);
                        var TableControl = "pnlAdminVisitTypeDurationGroup #dgvVisitTypeDurationGroup";
                        var PagingPanelControlID = "pnlAdminVisitTypeDurationGroup #divVisitTypeDurationGroupPaging";
                        var ClassControlName = "Admin_VisitTypeDurationGroup";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.VisitTypeDurationGroupCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            VisitTypeDurationGroup.VisitTypeDurationGroupSearch(PrimaryID, PageNumber, ResultPerPage);
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

    SearchVisitTypeDurationGroup: function (VisitTypeData, VisitTypeDurationGroupId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "VisitTypeData=" + VisitTypeData + "&VisitTypeDurationGroupId=" + VisitTypeDurationGroupId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        return MDVisionService.defaultService(data, "ADMIN_VISITTYPEDURATIONGROUP", "SEARCH_VISIT_TYPE_DURATION_GROUP");
    },

    VisitTypeDurationGroupAdd: function (VisitTypeDurationGroupId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("VisitTypeDurationGroup", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["VisitTypeDurationGroupId"] = VisitTypeDurationGroupId;
                params["TabID"] = "VisitTypeDetail";
                params["mode"] = "Add";

                LoadActionPan('VisitTypeDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    VisitTypeDurationGroupDelete: function (VisitTypeDurationGroupId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("VisitTypeDurationGroup", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var VisitTypeId = VisitTypeDurationGroupId;
                    if (VisitTypeId == "" || VisitTypeId == "undefined") {
                    }
                    else {
                        VisitTypeDurationGroup.DeleteVisitType(VisitTypeId).done(function (response) {
                            if (response.status == true) {
                                VisitTypeDurationGroup.VisitTypeDurationGroupSearch();
                                utility.DisplayMessages(response.Message, 1);
                            }
                            else
                                utility.DisplayMessages(response.Message, 3);
                            
                        });
                    }
                });

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    DeleteVisitType: function (VisitTypeDurationGroupId) {
        var data = "VisitTypeDurationGroupID=" + VisitTypeDurationGroupId;
        return MDVisionService.defaultService(data, "ADMIN_VISITTYPEDURATIONGROUP", "DELETE_VISIT_TYPE_DURATION_GROUP");
    },

    VisitTypeDurationGroupEdit: function (VisitTypeDurationGroupId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($("#" + VisitTypeDurationGroup.params["PanelID"] + ' #gvVisitTypeDurationGroup_row' + VisitTypeDurationGroupId));
        AppPrivileges.GetFormPrivileges("VisitTypeDurationGroup", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["VisitTypeDurationGroupId"] = VisitTypeDurationGroupId;
                params["TabID"] = "VisitTypeDetail";
                params["mode"] = "Edit";

                LoadActionPan('VisitTypeDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    VisitTypeActiveInactive: function (VisitTypeDurationGroupId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("VisitTypeDurationGroup", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var VisitTypeId = VisitTypeDurationGroupId;
                    if (VisitTypeId == "" || VisitTypeId == "undefined") {
                    }
                    else {
                        VisitTypeDurationGroup.UpdateVisitType(VisitTypeId, IsActive).done(function (response) {
                            if (response.status != false) {
                                VisitTypeDurationGroup.VisitTypeDurationGroupSearch();
                                utility.DisplayMessages(response.message, 1);
                            }
                            else
                                utility.DisplayMessages(response.Message, 3);
                            
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

    UpdateVisitType: function (VisitGroupId, IsActive) {
        var data = "VisitTypeDurationGroupId=" + VisitGroupId + "&IsActive=" + IsActive;
        return MDVisionService.defaultService(data, "ADMIN_VISITTYPEDURATIONGROUP", "UPDATE_VISIT_TYPE_GROUP_ACTIVE_INACTIVE");
    },

    UnloadTab: function (Tab) {

        if (VisitTypeDurationGroup.params["FromAdmin"] == "0") {


            if (VisitTypeDurationGroup.params != null && VisitTypeDurationGroup.params.ParentCtrl != null && VisitTypeDurationGroup.params.PanelID != 'pnlAdminVisitTypeDurationGroup') {
                UnloadActionPan(VisitTypeDurationGroup.params.ParentCtrl, 'VisitTypeDurationGroup', null, VisitTypeDurationGroup.params.PanelID);
            }

            else if (VisitTypeDurationGroup.params != null && VisitTypeDurationGroup.params.ParentCtrl != null) {
                UnloadActionPan(VisitTypeDurationGroup.params.ParentCtrl, 'VisitTypeDurationGroup');
            }

            else
                UnloadActionPan(null, 'VisitTypeDurationGroup');
        }
        else {
            RemoveAdminTab();
        }

    }
}