
Admin_Modifier = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {

        Admin_Modifier.params = params;

        if (Admin_Modifier.bIsFirstLoad) {
            Admin_Modifier.bIsFirstLoad = false;

            AppPrivileges.GetFormPrivileges("Modifier", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_Modifier.ModifierSearch();
                }
            });
        }
    },

    ModifierAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Modifier", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ModifierId"] = null;
                params["mode"] = "Add";

                if (Admin_Modifier.params["FromAdmin"] == "0") {
                    params["ParentCtrl"] = 'Admin_Modifier';
                }

                LoadActionPan('modifierDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ModifierEdit: function (ModifierId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvModifier_row' + ModifierId));
        AppPrivileges.GetFormPrivileges("Modifier", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = ModifierId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["ModifierId"] = selectedValue;
                    params["mode"] = "Edit";

                    if (Admin_Modifier.params["FromAdmin"] == "0") {
                        params["ParentCtrl"] = 'Admin_Modifier';
                    }

                    LoadActionPan('modifierDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ModifierDelete: function (ModifierId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvModifier_row' + ModifierId));
        AppPrivileges.GetFormPrivileges("Modifier", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = ModifierId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_Modifier.DeleteModifier(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#dgvModifier').DataTable();
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

    ModifierActiveInactive: function (ModifierId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Modifier", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = ModifierId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        modifierDetail.UpdateModifierActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_Modifier.ModifierSearch('0');
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

    ModifierSearch: function (ModifierId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Modifier", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminModifier #pnlModifier_Result").css("display") == "none") {
                    $("#pnlAdminModifier #pnlModifier_Result").show();
                }

                var self = $('#' + Admin_Modifier.params["PanelID"] + ' #pnlModifier_Search');
                var myJSON = self.getMyJSON();

                Admin_Modifier.SearchModifier(myJSON, ModifierId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_Modifier.ModifierGridLoad(response);
                        var TableControl = "pnlAdminModifier #dgvModifier";
                        var PagingPanelControlID = "pnlAdminModifier #divModifierPaging";
                        var ClassControlName = "Admin_Modifier";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.ModifierCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_Modifier.ModifierSearch(PrimaryID, PageNumber, ResultPerPage);
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

    ModifierGridLoad: function (response) {
        $("#dgvModifier").dataTable().fnDestroy();
        $("#pnlModifier_Result #dgvModifier tbody").find("tr").remove();
        if (response.ModifierCount > 0) {
            var ModifierLoadJSONData = JSON.parse(response.ModifierLoad_JSON);
            $.each(ModifierLoadJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", "gvModifier_row" + item.ModifierId);
                $row.attr("ModifierId", item.ModifierId);

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

                var selectModifier = "";
                if (Admin_Modifier.params["FromAdmin"] == "0") {
                    var selectMethod = "Admin_Modifier.FillModifierCode('" + item.ModifierId + "','" + item.ModifierCode + "','" + item.Description + "');"
                    selectModifier = '&nbsp;<a class="btn  btn-xs" href="#" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';
                    $row.attr("onclick", selectMethod);
                }
                else {
                    $row.attr("onclick", "Admin_Modifier.ModifierEdit('" + item.ModifierId + "',event);");
                }
                $row.append('<td style="display:none;">' + item.ModifierId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_Modifier.ModifierDelete(\'' + item.ModifierId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_Modifier.ModifierEdit(\'' + item.ModifierId + '\',event);"  title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_Modifier.ModifierActiveInactive(\'' + item.ModifierId + '\',' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectModifier + '</td><td>' + item.ModifierCode + '</td><td>' + item.Description + '</td>');

                $("#pnlModifier_Result #dgvModifier tbody").last().append($row);
            });
        }
        else {
            $('#dgvModifier').DataTable({
                "language": {
                    "emptyTable": "No Modifier Found"
                },
                "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvModifier'))
            ;
        else
            $('#pnlModifier_Result #dgvModifier').DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        //   $("#pnlModifier_Result #dgvModifier").DataTable({ "bLengthChange": false, "order": [[0, "desc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },


    FillModifierCode: function (ModifierId, ModifierCode, Description) {

        var RefCtrl = " #txtModifierCode";
        var RefHiddenIdCtrl = " #hfmodifiercode";
        var RefHiddenCtrl = " #hfModifierCode";
        var RefCtrlDescription = " #hfModifierDescription";

        if (Admin_Modifier.params["RefCtrl"] != null) {
            RefCtrl = " #" + Admin_Modifier.params["RefCtrl"];
        }
        if (Admin_Modifier.params["RefHiddenIdCtrl"] != null) {
            RefHiddenIdCtrl = " #" + Admin_Modifier.params["RefHiddenIdCtrl"];
        }
        if (Admin_Modifier.params["RefHiddenCtrl"] != null) {
            RefHiddenCtrl = " #" + Admin_Modifier.params["RefHiddenCtrl"];
        }
        if (Admin_Modifier.params["RefCtrlDescription"] != null) {
            RefCtrlDescription = " #" + Admin_Modifier.params["RefCtrlDescription"];
        }

        $('#' + Admin_Modifier.params["PanelID"] + RefCtrl).val(ModifierCode);
        $('#' + Admin_Modifier.params["PanelID"] + RefHiddenIdCtrl).val(ModifierId);
        $('#' + Admin_Modifier.params["PanelID"] + RefHiddenCtrl).val(ModifierCode);
        $('#' + Admin_Modifier.params["PanelID"] + RefCtrlDescription).val(Description);

        UnloadActionPan(Admin_Modifier.params["ParentCtrl"], "Admin_Modifier");
    },

    SearchModifier: function (ModifierData, ModifierId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "ModifierData=" + ModifierData + "&ModifierID=" + ModifierId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_MODIFIER", "SEARCH_MODIFIER");
    },

    ValidateModifierCode: function (ModifierCode) {


        return Admin_Modifier.LoadModifierCode("", ModifierCode).done(function (response) {

            if (response.length == 0) {
                utility.DisplayMessages("Invalid Modifier Code.", 3);
            }
        });
    },
    LoadModifierCode: function (entityID, ModifierCode) {
        var data = "ModifierCode=" + ModifierCode + "&entityID=" + entityID

        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "COMMON_CODE", "CHECK_MODIFIER_CODE");
    },

    DeleteModifier: function (ModifierId) {
        var data = "ModifierID=" + ModifierId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_MODIFIER_DETAIL", "DELETE_MODIFIER");
    },

    UnLoadTab: function (Tab) {

        if (Admin_Modifier.params["FromAdmin"] == "0") {
            if (Admin_Modifier.params != null && Admin_Modifier.params.ParentCtrl != null) {
                UnloadActionPan(Admin_Modifier.params.ParentCtrl, 'Admin_Modifier');
            }
            else
                UnloadActionPan(null, 'Admin_Modifier');

        }
        else {
            RemoveAdminTab();
        }
    },
}