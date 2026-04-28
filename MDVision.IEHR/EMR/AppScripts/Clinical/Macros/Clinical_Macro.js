Clinical_Macro = {
    Ids: [],
    params: [],
    Load: function (params) {
        Clinical_Macro.Ids = [];
        Clinical_Macro.params = params;

        utility.ValidateFromToDate("pnlClinicalMacro #frmClinicalMacro", 'dpDateFrom', 'dpDateTo', true);

        Clinical_Macro.loadComponents();
        Clinical_Macro.loadUsers();
        Clinical_Macro.loadMacro();
    },
    openMacroDetails: function () {
        var params = [];
        params["Mode"] = 'Add';
        params["TabID"] = 'Clinical_MacroDetail';
        LoadActionPan("Clinical_MacroDetail", params);

    },
    editMacroDetails: function (item) {
        var params = [];
        params["Mode"] = 'Edit';
        params["MacroId"] = item.MacroId;
        params["TabID"] = 'Clinical_MacroDetail';
        LoadActionPan("Clinical_MacroDetail", params);

    },
    deleteMacroDetails: function () {
        var obj = new Object();
        obj["IdsToDelete"] = String(Clinical_Macro.Ids);
        obj["commandType"] = "Delete_Macro";
        var data = JSON.stringify(obj);

        return MDVisionService.APIService(data, "Macro", "Macro");
        // Clinical_Macro.Ids = [];
        //Clinical_Macro.Load();

    },
    loadMacro: function () {
        Clinical_Macro.Ids = [];
        Clinical_Macro.LoadMacro().done(function (response) {
            Clinical_Macro.MacroGridLoad(response);
        });
    },
    LoadMacro: function (objData) {
        var obj = new Object();
        if (objData) {
            obj = objData;
        }
        obj["commandType"] = "Get_Macro";
        var data = JSON.stringify(obj);
        return MDVisionService.APIService(data, "Macro", "Macro");
    },
    htmlDecode: function (value) {
        return value.replace("<br>", /\r?\n/gi);
    },
    MacroGridLoad: function (object) {
        object = JSON.parse(object);

        var data = new kendo.data.DataSource({
            data: object.macros,
            pageSize: 15
        });
        $("#macrotable").kendoGrid({
            dataSource: data,
            resizable: true,
            scrollable: false,
            noRecords: true,
            messages: {
                noRecords: "No Records Found."
            },
            selectable: "multiselection",
            pageable: {
                refresh: true,
                pageSizes: [5, 10, 20, 50, 100],
                buttonCount: 5
            },
            persistSelection: true,
            columns: [
                { headerTemplate: '<label><input type= \'checkbox\' id = \'checkAll\' onchange=\'Clinical_Macro.ActionCheckAll();\'/></label>', template: '#=Clinical_Macro.ActionCheckMacro(data)#', width: "50px" },
            { title: "Macro Name", field: "MacroName", width: "50px" },
            { title: "Keyword", field: "Keyword", width: "50px" },
            { title: "Keyword Description", template: '#=Clinical_Macro.MicroDescription(data)#', width: "200px" },
            { title: "Component", field: "NoteComponentsNames", width: "100px" },
            { title: "Visible to Users", field: "UserNames", width: "80px" },
            { title: "Created/Updated By", template: '#=Clinical_Macro.ActionMacro(data)#', width: "100px" }
            ]

        });

        //Start 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
        utility.removePaginationFromGrid($('#' + Clinical_Macro.PanelID + ' #macrotable'));
        //End 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537

        $("#macrotable").click(function () {
            var grid = $("#macrotable").data("kendoGrid");
            var item = grid.dataItem(grid.select());
            if (item)
                Clinical_Macro.editMacroDetails(item);
            grid.clearSelection();

        });
        Clinical_Macro.SetTooltipDescription();
    },
    ActionMacro: function (data) {
        return data.CreatedOn + " By " + data.CreatedBy + "</br> <b>" + data.ModifiedOn + " By " + data.ModifiedBy + "</b>";
    },
    ActionCheckMacro: function (data) {
        // data.Description = data.Description.replace(/<br>/g, " ");
        return '<input type ="checkbox" id=' + data.MacroId + ' onchange="Clinical_Macro.ActionMacroCheck(' + data.MacroId + ');">';
    },
    MicroDescription: function (data) {
        if (data.Description.length > 100)
        { return data.Description.substring(0, 100) + "..." }
        else {
            return data.Description;
        }
    },
    ActionMacroCheck: function (data) {


        if ($(('#' + String(data))).is(':checked')) {
            Clinical_Macro.Ids.push(String(data));
        }
        else {
            Clinical_Macro.Ids.splice(Clinical_Macro.Ids.findIndex(function (element) { return element == data }), 1);
            $("#checkAll").prop("checked", false);
        }
        if (Clinical_Macro.isAllCheckboxSelected()) {

            $("#checkAll").prop("checked", true);
        }
    },
    ActionCheckAll: function () {
        var data = $("#macrotable").data("kendoGrid").dataSource._data;
        if ($('#checkAll').is(':checked')) {

            for (i = 0; i < data.length; i++) {
                // only get Checked Mark CheckBox EMR-6965
                $('#' + String(data[i].MacroId)).prop('checked', true)
                if ($('#' + String(data[i].MacroId)).prop('checked') == true) {
                    Clinical_Macro.Ids.push(String(data[i].MacroId));
                }

            }
        }
        else {
            for (i = 0; i < data.length; i++) {

                Clinical_Macro.Ids = [];
                $('#' + String(data[i].MacroId)).prop('checked', false);

            }

        }
    },
    deleteMacro: function () {


        //  AppPrivileges.GetFormPrivileges("TinymceEditor", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //      if (strMessage == "") {
        if (Clinical_Macro.Ids.length > 0) {
            utility.myConfirm('Do you want to delete  macro(s)?', function () {
                Clinical_Macro.deleteMacroDetails().done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        Clinical_Macro.Load();
                        Clinical_Macro.Ids = [];

                    }
                    else {
                        utility.DisplayMessages(response.message, 3);
                    }



                });
            }, function () { }, ' Confirm Delete');
        }
        else {
            utility.DisplayMessages("Please select any Macro(s) to delete.", 3);
        }
        // }
        // else
        //    utility.DisplayMessages(strMessage, 2);
        // });

    },
    SetTooltipDescription: function () {
        var tdColumn = "";

        tdColumn = "td:nth-child(4)";

        $("#macrotable").kendoTooltip({
            filter: tdColumn,
            position: "right",
            animation: {
                close: {
                    effects: 'fade:out'
                },
                open: {
                    effects: 'fade:in'
                }
            },
            content: function (e) {
                var dataItem = $("#macrotable").data("kendoGrid").dataItem(e.target.closest("tr"));
                var content = dataItem.Description;
                return "<div style='width: " + content.length * .6 + "em; padding:6px; border-radius:4px; background:black; color:white; min-width:20em; max-width:20em; max-height:300px !important; overflow-y: auto;'>" + content + "</div>";
            }
        }).data("kendoTooltip");

        $("#macrotable").click(function () {
            $("#macrotable").data("kendoTooltip").hide();
        });
    },
    isAllCheckboxSelected: function () {
        var data = $("#macrotable").data("kendoGrid").dataSource._data;
        var IsAllChkboxChecked = false;
        for (i = 0; i < data.length; i++) {

            if ($('#' + String(data[i].MacroId)).is(":checked")) {
                IsAllChkboxChecked = true;
            }

            else {
                IsAllChkboxChecked = false;
                break;

            }
        }
        return IsAllChkboxChecked;
    },
    MacroSearch: function () {
        var self = $('#pnlClinicalMacro');
        var objData = new Object();
        objData["MacroName"] = self.find('#txtMacroName').val();
        objData["Keyword"] = self.find('#txtKeyword').val();
        var ComponentIds = self.find('#ddlMacroComponentNames option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objData["NoteComponentIds"] = ComponentIds;

        var UserIds = self.find('#ddlMacroUsers option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objData["UsersIds"] = UserIds;
        objData["DateFrom"] = self.find('#dpDateFrom').val();
        objData["DateTo"] = self.find('#dpDateTo').val();

        Clinical_Macro.LoadMacro(objData).done(function (response) {
            Clinical_Macro.MacroGridLoad(response);
        });
    },
    loadUsers: function () {
        var dfd = new $.Deferred();
        MDVisionService.lookups('GetUsersFullName', true).done(function (result) {
            result = JSON.parse(result["GetUsersFullName"]);
            var options = result;
            var $usersDdl = $('#pnlClinicalMacro #ddlMacroUsers');

            //Empty both the providers ddls.
            $usersDdl.empty();

            //Loop through all providers loaded from the server
            $.each(options, function (i, item) {
                if (item.Value != "" && typeof item.Value != 'undefined') {
                    // User will see these users in multiSelect dropdownlist
                    $usersDdl.append(
                        $('<option/>', {
                            value: item.Value,
                            html: item.Name,
                            refname: item.RefName,
                            refvalue: item.RefValue
                        })
                    );
                }
            });
        }).then(function () {  
            Clinical_Macro.IntializeMultiSelectDropDownUsers();
            if (dfd)
                dfd.resolve();
        });

        return dfd.promise();
    },
    loadComponents: function () {
        MDVisionService.lookups('GetNoteComponents', true).done(function (result) {
            result = JSON.parse(result["GetNoteComponents"]);
            var options = result;
            var $componentsDdl = $('#pnlClinicalMacro #ddlMacroComponentNames');

            //Empty both the providers ddls.
            $componentsDdl.empty();

            //Loop through all providers loaded from the server
            $.each(options, function (i, item) {
                if (item.Value != "" && typeof item.Value != 'undefined') {

                    // User will see these components in multiSelect dropdownlist
                    $componentsDdl.append(
                        $('<option/>', {
                            value: item.Value,
                            html: item.Name,
                            refname: item.RefName,
                            refvalue: item.RefValue
                        })
                    );
                }
            });
        }).then(function () {  
            Clinical_Macro.IntializeMultiSelectDropDownComponents();
        });
    },
    IntializeMultiSelectDropDownComponents: function () {
        $('#pnlClinicalMacro #ddlMacroComponentNames').multiselect('destroy');
        $('#pnlClinicalMacro #ddlMacroComponentNames').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: false,
        });
    },
    IntializeMultiSelectDropDownUsers: function () {
        $('#pnlClinicalMacro #ddlMacroUsers').multiselect('destroy');
        $('#pnlClinicalMacro #ddlMacroUsers').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: false,
        });
    },
    MacroReset: function () {
        $("#pnlClinicalMacro #frmClinicalMacro #divMacroSearch").resetAllControls();
        //to remove default selection in multiselect dropdown
        $("#pnlClinicalMacro #ddlMacroUsers option:selected").prop("selected", false);
        $('#pnlClinicalMacro #ddlMacroUsers').multiselect("refresh");
        $("#pnlClinicalMacro #ddlMacroComponentNames option:selected").prop("selected", false);
        $('#pnlClinicalMacro #ddlMacroComponentNames').multiselect("refresh");
    },
}
