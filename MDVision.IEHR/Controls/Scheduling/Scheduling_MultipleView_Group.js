multipleViewGroup = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        multipleViewGroup.params = params;

        multipleViewGroup.bIsFirstLoad = false;
        BackgroundLoaderShow(false);
        var self = $('#multipleViewGroup');
        self.loadDropDowns(true).done(function () {
            BackgroundLoaderShow(false);
            if (globalAppdata['IsAdmin'] != "True") {
                $("#" + multipleViewGroup.params["PanelID"] + " #multipleViewGroup #divMultipleView_Entity").css("display", "none");
                $("#" + multipleViewGroup.params["PanelID"] + " #multipleViewGroup #ddlEntity").val(globalAppdata["SeletedEntityId"]);
            }
            multipleViewGroup.ScheduleGroupSearch();
        });
    },

    ScheduleGroupAdd: function () {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Schedule Group", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ParentCtrl"] = "multipleViewGroup";
                params["mode"] = "Add";
                LoadActionPan('multipleViewGroupDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ScheduleGroupEdit: function (MSGroupId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Schedule Group", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = MSGroupId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["MSGroupId"] = selectedValue;
                    params["mode"] = "Edit";
                    params["ParentCtrl"] = "multipleViewGroup";
                    LoadActionPan('multipleViewGroupDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ScheduleGroupSearch: function (MSGroupId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Schedule Group", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#multipleViewGroup #pnlMultipleViewGroup_Result").css("display") == "none") {
                    $("#multipleViewGroup #pnlMultipleViewGroup_Result").show();
                }

                var self = $("#multipleViewGroup");
                var myJSON = self.getMyJSON();

                multipleViewGroup.SearchScheduleGroup(myJSON, MSGroupId).done(function (response) {
                    if (response.status != false) {
                        multipleViewGroup.ScheduleGroupGridLoad(response);
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

    ScheduleGroupDelete: function (MSGroupId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }

        utility.SelectGridRow($("#gvMultipleViewGroup_row" + MSGroupId));
        AppPrivileges.GetFormPrivileges("Schedule Group", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = MSGroupId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        multipleViewGroup.DeleteScheduleGroup(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#dgvMultipleViewGroup').DataTable();
                                table1.row('.active').remove().draw(false);
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

    ScheduleGroupActiveInactive: function (MSGroupId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Schedule Group", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = MSGroupId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        multipleViewGroupDetail.UpdateScheduleGroupActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                CacheManager.BindCodes('GetScheduleGroup', true).done(function (result) {
                                    var Tab = GetCurrentSelectedTab();
                                    eval(Tab.ContainerControlID + '.bIsFirstLoad=true');
                                    SelectTab(Tab.TabID, 'false');
                                    $("#multipleViewGroupDetail #ddlEntity option[value=" + entity + "]").attr('selected', 'selected');
                                });
                                multipleViewGroup.ScheduleGroupSearch('0');
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


    SelectGroup: function (MSGroupId) {
        $("#pnlScheduleMuliView #Group option[value=" + MSGroupId + "]").attr('selected', 'selected');
        //Start 12/02/2016 Muhammad Irfan for bug # PMS-3951
        if ($('#pnlScheduleMuliView #frmSchedulingMuliView').data('bootstrapValidator') != null && typeof $('#pnlScheduleMuliView #frmSchedulingMuliView').data('bootstrapValidator') != 'undefined') {
            $('#pnlScheduleMuliView #frmSchedulingMuliView').bootstrapValidator('revalidateField', 'Group');
        }
        //End 12/02/2016 Muhammad Irfan for bug # PMS-3951
        UnloadActionPan();
    },

    ScheduleGroupGridLoad: function (response) {
        $("#dgvMultipleViewGroup").dataTable().fnDestroy();
        $("#multipleViewGroup #dgvMultipleViewGroup tbody").find("tr").remove();
        if (response.MultipleViewGroupCount > 0) {
            var MultipleViewGroupLoadJSONData = JSON.parse(response.MultipleViewGroupLoad_JSON);
            $.each(MultipleViewGroupLoadJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", "gvMultipleViewGroup_row" + item.MSGroupId);
                $row.attr("MSGroupId", item.MSGroupId);

                if (item.IsActive == "True") {
                    isactive = 0;
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    tglclass = "fa fa-toggle-on red";
                }
                var SelectMethod = "";
                //adnan maqbool, pms-3950, 15-02-2016
                if (item.IsActive != "False") {
                    SelectMethod = "multipleViewGroup.SelectGroup('" + item.MSGroupId + "');"
                }
                //end
                if (SelectMethod != "") {
                    $row.attr("onclick", SelectMethod);
                }
                
                $row.append('<td style="display:none;">' + item.MSGroupId + '</td><td><a class="btn btn-xs" href="#" onclick=' + SelectMethod + '  title="Select Record"><i class="fa fa-check black"></i></a><a class="btn  btn-xs" href="#" onclick="multipleViewGroup.ScheduleGroupDelete(' + item.MSGroupId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="multipleViewGroup.ScheduleGroupEdit(' + item.MSGroupId + ',event);"  title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="multipleViewGroup.ScheduleGroupActiveInactive(' + item.MSGroupId + ', ' + isactive + ',event);" title="Inactive Record"><i class="' + tglclass + '"></i></a></td><td>' + item.ShortName + '</td><td>' + item.Description + '</td><td>' + item.IsActive + '</td>');


                $("#multipleViewGroup #dgvMultipleViewGroup tbody").last().append($row);
            });
        }
        else {
            $('#dgvMultipleViewGroup').DataTable({
                "language": {
                    "emptyTable": "No View Group Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvMultipleViewGroup'));
        else
            $("#pnlMultipleViewGroup_Result #dgvMultipleViewGroup").DataTable({ "bLengthChange": false, "order": [[0, "desc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchScheduleGroup: function (MultipleViewGroupData, MSGroupId) {
        var data = "MultipleViewGroupData=" + MultipleViewGroupData + "&MSGroupId=" + MSGroupId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "SCHEDULING_MULTIPLEVIEW_GROUP", "SEARCH_SCHEDULE_GROUP");
    },

    DeleteScheduleGroup: function (MSGroupId) {
        var data = "MSGroupId=" + MSGroupId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "SCHEDULING_MULTIPLEVIEW_GROUP_DETAIL", "DELETE_SCHEDULE_GROUP");
    },

    UnLoad: function () {

        UnloadActionPan();
    },
}