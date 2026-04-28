typeOfServiceDetail = {
    params: [],
    Load: function (params) {
        typeOfServiceDetail.params = params;
        
        var self = $('#typeOfServiceDetail');
        self.loadDropDowns(true).done(function () {

            typeOfServiceDetail.LoadTypeOfService();
        });
    },

    LoadTypeOfService: function () {
        $("#typeOfServiceDetail #pnlTOSPlanInfo").removeClass('disableAll');
        if (typeOfServiceDetail.params.mode == "Add") {
            
            $("#typeOfServiceDetail #pnlTOSPlanInfo").addClass('disableAll');
            typeOfServiceDetail.ValidateTypeOfService();

            //Serialize data
            $('#frmTypeOfServiceDetail').data('serialize', $('#frmTypeOfServiceDetail').serialize());
        }
        else if (typeOfServiceDetail.params.mode == "Edit") {
            typeOfServiceDetail.FillTypeOfService(typeOfServiceDetail.params.TypeOfServiceId).done(function (response) {
                if (response.status != false) {
                    var typeOfService_detail = JSON.parse(response.TypeOfServiceFill_JSON);
                    var self = $("#typeOfServiceDetail");
                    utility.bindMyJSON(true, typeOfService_detail, false, self).done(function () {


                        $("#typeOfServiceDetail #pnlTOSPlanInfo").removeClass('disableAll');

                        typeOfServiceDetail.ValidateTypeOfService();

                        //Serialize data
                        $('#frmTypeOfServiceDetail').data('serialize', $('#frmTypeOfServiceDetail').serialize());

                        typeOfServiceDetail.LoadTypeOfServicePlanInfo().done(function (response) {
                            if (response.status != false) {
                                typeOfServiceDetail.TOSPlanGridLoad(response);

                                //Serialize data
                                $('#frmTypeOfServiceDetail').data('serialize', $('#frmTypeOfServiceDetail').serialize());
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });

                    });
                   
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    ValidateTypeOfService: function () {
        $('#frmTypeOfServiceDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   Code: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Name: {
                       group: '.col-sm-8',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   }
               }
           }).on('success.form.bv', function (e) {
               e.preventDefault();
               typeOfServiceDetail.TypeOfServiceSave();
           });
    },

    TypeOfServiceSave: function () {
        var self = $("#typeOfServiceDetail");
        var myJSON = self.getMyJSON();
        if (typeOfServiceDetail.params.mode == "Add") {
            var strMessage = "";
            AppPrivileges.GetFormPrivileges("Type Of Service", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    if (typeOfServiceDetail.params.TypeOfServiceId == "-1") {
                        typeOfServiceDetail.SaveTypeOfService(myJSON).done(function (response) {
                            if (response.status != false) {
                                $("#typeOfServiceDetail #pnlTOSPlanInfo").removeClass('disableAll');
                                Admin_TypeOfService.TypeOfServiceSearch(response.TypeOfServiceId);
                                typeOfServiceDetail.params.TypeOfServiceId = response.TypeOfServiceId;

                                //Editable Grid
                                var PanelGrid = "#typeOfServiceDetail #pnlTOSPlanInfo";
                                var GridId = "#typeOfServiceDetail #dgvTOSPlanInfo";
                                utility.MakeEditableGrid(PanelGrid, GridId, typeOfServiceDetail);

                                utility.DisplayMessages(response.message, 1);
                                CacheManager.BindCodes('GetTypeOfService', true);
                                $('#frmTypeOfServiceDetail').data('serialize', $('#frmTypeOfServiceDetail').serialize());
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                    else if (typeOfServiceDetail.params.TypeOfServiceId != "-1" && typeOfServiceDetail.params.TypeOfServiceId != "" && typeOfServiceDetail.params.TypeOfServiceId != "0") {
                        typeOfServiceDetail.UpdateTypeOfService(myJSON, typeOfServiceDetail.params.TypeOfServiceId).done(function (response) {
                            if (response.status != false) {
                                Admin_TypeOfService.TypeOfServiceSearch(typeOfServiceDetail.params.TypeOfServiceId);
                                utility.DisplayMessages(response.message, 1);
                                CacheManager.BindCodes('GetTypeOfService', true);
                                $('#frmTypeOfServiceDetail').data('serialize', $('#frmTypeOfServiceDetail').serialize());
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else if (typeOfServiceDetail.params.mode == "Edit") {
            var strMessage = "";
            AppPrivileges.GetFormPrivileges("Type Of Service", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    typeOfServiceDetail.UpdateTypeOfService(myJSON, typeOfServiceDetail.params.TypeOfServiceId).done(function (response) {
                        if (response.status != false) {
                            Admin_TypeOfService.TypeOfServiceSearch(typeOfServiceDetail.params.TypeOfServiceId);
                            utility.DisplayMessages(response.message, 1);
                            CacheManager.BindCodes('GetTypeOfService', true);
                            $('#frmTypeOfServiceDetail').data('serialize', $('#frmTypeOfServiceDetail').serialize());
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
    },

    // ------------ TOS Plan Region (Detail Grid)
    TOSPlanGridLoad: function (response) {

        if (response.TOSPlanCount > 0) {
            var TOSPlanJSON = JSON.parse(response.TOSPlan_JSON);
            $.each(TOSPlanJSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($(this))");
                $row.attr("id", item.PlanOfTOSId);

                // get Actions
                var actions = "";
                $("#typeOfServiceDetail #dgvTOSPlanInfo tr th").each(function () {
                    if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                        var arrActionType = [];
                        if ($(this).attr("ActionType") != null) {
                            arrActionType = $(this).attr("ActionType").split(',');
                            actions = EditableGrid.GetActions(arrActionType);
                        }
                    }
                });

                $row.append('<td class="actions" id="' + item.PlanOfTOSId + '" >' + actions + '</td><td>' + item.InsurancePlanName + '</td><td>' + item.Name + '</td>');

                $("#typeOfServiceDetail #dgvTOSPlanInfo tbody").last().append($row);
            });
        }
        
        //Editable Grid
        var PanelGrid = "#typeOfServiceDetail #pnlTOSPlanInfo";
        var GridId = "#typeOfServiceDetail #dgvTOSPlanInfo";
        utility.MakeEditableGrid(PanelGrid, GridId, typeOfServiceDetail);
    },

    SaveTOSPlan: function (TOSPlanData, RowId) {
        var data = "TOSPlanData=" + TOSPlanData + "&TypeOfServiceID=" + typeOfServiceDetail.params.TypeOfServiceId + "&RowId=" + RowId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_TYPE_OF_SERVICE_DETAIL", "SAVE_TOS_PLAN_INFO");
    },

    UpdateTOSPlan: function (TOSPlanData, TOSPlanId) {
        var data = "TOSPlanData=" + TOSPlanData + "&TOSPlanId=" + TOSPlanId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_TYPE_OF_SERVICE_DETAIL", "UPDATE_TOS_PLAN_INFO");
    },

    FillTOSPlan: function (TOSPlanID) {
        var data = "TOSPlanID=" + TOSPlanID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_TYPE_OF_SERVICE_DETAIL", "FILL_TOS_PLAN_INFO");
    },

    DeleteTypeOfServicePlanInfo: function (TOSPlanId) {
        var data = "TOSPlanId=" + TOSPlanId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_TYPE_OF_SERVICE_DETAIL", "DELETE_TOS_PLAN_INFO");
    },

    LoadTypeOfServicePlanInfo: function () {
        var data = "TypeOfServiceID=" + typeOfServiceDetail.params.TypeOfServiceId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_TYPE_OF_SERVICE_DETAIL", "LOAD_TOS_PLAN_INFO");
    },

    OpenInsurancePlan: function (optional, Ctrl, HiddenCtrl) {

        var params = [];
        params["InsurancePlanId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "typeOfServiceDetail";
        if (Ctrl != null) {
            params["RefCtrl"] = Ctrl;
        }
        if (HiddenCtrl != null) {
            params["RefHiddenIdCtrl"] = HiddenCtrl;
        }
        LoadActionPan('Admin_InsurancePlan', params);
    },

    //----------------------------------------------------------------

    SaveTypeOfService: function (TypeOfServiceData) {
        var data = "TypeOfServiceData=" + TypeOfServiceData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_TYPE_OF_SERVICE_DETAIL", "SAVE_TYPE_OF_SERVICE");
    },

    UpdateTypeOfService: function (TypeOfServiceData, TypeOfServiceID) {
        var data = "TypeOfServiceData=" + TypeOfServiceData + "&TypeOfServiceID=" + TypeOfServiceID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_TYPE_OF_SERVICE_DETAIL", "UPDATE_TYPE_OF_SERVICE");
    },

    FillTypeOfService: function (TypeOfServiceID) {
        var data = "TypeOfServiceID=" + TypeOfServiceID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_TYPE_OF_SERVICE_DETAIL", "FILL_TYPE_OF_SERVICE");
    },

    UpdateTypeOfServiceActiveInactive: function (TypeOfServiceID, IsActive) {
        var data = "TypeOfServiceID=" + TypeOfServiceID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_TYPE_OF_SERVICE_DETAIL", "UPDATE_TYPE_OF_SERVICE_ACTIVE_INACTIVE");
    },

    //SaveTypeOfServicePlanInfo: function (TOSPlan, TOS) {
    //    var data = "TOSPlan=" + TOSPlan + "&TOS=" + TOS + "&TypeOfServiceID=" + typeOfServiceDetail.params.TypeOfServiceId;
    //    // serach parameter , class name, command name of class 
    //    return MDVisionService.defaultService(data, "ADMIN_TYPE_OF_SERVICE_DETAIL", "SAVE_TOS_PLAN_INFO");
    //},

    //UpdateTypeOfServicePlanInfo: function (TOSPlan, TOS, TOSPlanId) {
    //    var data = "TOSPlan=" + TOSPlan + "&TOS=" + TOS + "&TOSPlanId=" + TOSPlanId + "&TypeOfServiceID=" + typeOfServiceDetail.params.TypeOfServiceId;
    //    // serach parameter , class name, command name of class 
    //    return MDVisionService.defaultService(data, "ADMIN_TYPE_OF_SERVICE_DETAIL", "UPDATE_TOS_PLAN_INFO");
    //},

    //LoadTypeOfServicePlanInfo: function () {
    //    var data = "TypeOfServiceID=" + typeOfServiceDetail.params.TypeOfServiceId;
    //    // serach parameter , class name, command name of class 
    //    return MDVisionService.defaultService(data, "ADMIN_TYPE_OF_SERVICE_DETAIL", "LOAD_TOS_PLAN_INFO");
    //},

    //DeleteTypeOfServicePlanInfo: function (TOSPlanId) {
    //    var data = "TOSPlanId=" + TOSPlanId;
    //    // serach parameter , class name, command name of class 
    //    return MDVisionService.defaultService(data, "ADMIN_TYPE_OF_SERVICE_DETAIL", "DELETE_TOS_PLAN_INFO");
    //},

    UnLoad: function () {

        utility.UnLoadDialog("frmTypeOfServiceDetail", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });

    },

    ShowHistory: function () {
        var PanelID = 'typeOfServiceDetail';
        var ParentCtrl = 'typeOfServiceDetail';
        var ProfileName = 'Type Of Service';
        var DBTableName = 'TypeOfService';
        var ColumnKeyId = typeOfServiceDetail.params.TypeOfServiceId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },

    //-------------------Editable Grid Methods Starts---

    rowSave: function ($row, obj) {

        if (obj.rowValidate($row)) {

            var _self = obj,
            $actions,
            values = [];

            if ($row.hasClass('adding')) {
                $row.removeClass('adding');
            }

            values = $row.find('td').map(function () {

                var $this = $(this);

                if ($this.hasClass('expand')) {
                    return '<a href="#" class="hidden on-editing expand-row" title="Expand/Collapse Record" ><i class="fa fa-plus-square"></i></a>';
                }
                else if ($this.hasClass('actions')) {

                    return _self.datatable.cell(this).data();
                }
                else if ($this.hasClass('ddl')) {
                    return $.trim($this.find('select').val());

                } else {
                    return $.trim($this.find('input').val());
                }
            });

            var id = $row.attr("id");
            var myJSON = $row.getMyJSON();

            if (id && id > 0) {
                //Edit Record
                var strMessage = "";
                AppPrivileges.GetFormPrivileges("Type Of Service", "Edit", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {

                        typeOfServiceDetail.UpdateTOSPlan(myJSON, id).done(function (response) {
                            if (response.status != false) {

                                utility.DisplayMessages(response.Message, 1);
                                typeOfServiceDetail.rowDraw($row, _self, values);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                });
            }
            else {
                //Add Record

                AppPrivileges.GetFormPrivileges("Type Of Service", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        typeOfServiceDetail.SaveTOSPlan(myJSON,id).done(function (response) {
                            if (response.status != false) {

                                $row.attr("id", response.TOSPlanId);
                                $row.attr("onclick", "utility.SelectGridRow($(this))");
                                utility.DisplayMessages(response.Message, 1);
                                typeOfServiceDetail.rowDraw($row, _self, values);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
            }

        }
    },

    rowAdd: function () {

        AppPrivileges.GetFormPrivileges("Type Of Service", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                EditableGrid.rowAdd("-" + $('#' + typeOfServiceDetail.params.PanelID + ' #dgvTOSPlanInfo tr').length);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    rowRemove: function ($row, obj) {

        var id = $row.attr("id");
        AppPrivileges.GetFormPrivileges("Type Of Service", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = id;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        typeOfServiceDetail.DeleteTypeOfServicePlanInfo(selectedValue).done(function (response) {
                            if (response.status != false) {

                                if ($row.hasClass('adding')) {
                                }
                                var _self = obj;
                                _self.datatable.row($row.get(0)).remove().draw();

                                //Start 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
                                    utility.removePaginationFromGrid($('#' + typeOfServiceDetail.params.PanelID + ' #pnlTOSPlanInfo'));
                                //End 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
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

    rowCancel: function ($row, obj) {


        var _self = obj,
            $actions,
            i,
            data;

        if ($row.hasClass('adding')) {
            _self.datatable.row($row.get(0)).remove().draw();

        } else {

            data = _self.datatable.row($row.get(0)).data();
            _self.datatable.row($row.get(0)).data(data);

            $actions = $row.find('td.actions');
            if ($actions.get(0)) {
                _self.rowSetActionsDefault($row);
            }

            _self.datatable.draw();
        }
    },

    rowDraw: function ($row, _self, values) {

        _self.datatable.row($row.get(0)).data(values);
        $actions = $row.find('td.actions');
        if ($actions.get(0)) {
            _self.rowSetActionsDefault($row);
        }
        _self.datatable.draw();
    },

    //-------------------Editable Grid Methods Ends---

    //LoadTOSPlans: function () {
    //    typeOfServiceDetail.LoadTypeOfServicePlanInfo().done(function (response) {
    //        if (response.status != false) {
    //            if (response.TOSPlanCount > 0) {
    //                var TOSPlanJSON = JSON.parse(response.TOSPlan_JSON);
    //                $.each(TOSPlanJSON, function (i, item) {
    //                    var $row = $('<tr/>');
    //                    $row.attr("TOSPlanId", item.PlanOfTOSId)
    //                    $row.append('<td style="display:none;">' + item.PlanOfTOSId + '</td><td>' + item.InsurancePlanName + '</td><td>' + item.Name + '</td>');
    //                    $("#dgvTOSPlanInfo tbody").last().append($row);
    //                });
    //            }
    //        }
    //    });
    //},

    //TypeOfServiceInfo: function () {
    //    $(function () {
    //        var self = $('#gridform');
    //        self.loadDropDowns(true);
    //        var tbl = $("#dgvTOSPlanInfo");
    //        var obj = $.paramquery.tableToArray(tbl);

    //        var newObj = {
    //            width: 700,
    //            height: 460,
    //            sortIndx: 0,
    //            numberCell: false,
    //            flexWidth: true,
    //            bottomVisible: false,
    //            editable: false,
    //            flexHeight: true,
    //            title: "PLAN SPECIFIC",
    //            freezeCols: 1,
    //            resizable: false,
    //            scrollModel: { horizontal: false },
    //            selectionModel: {
    //                type: 'row'
    //            },
    //            editModel: {
    //                clicksToEdit: 2
    //            },
    //            selectionModel: {
    //                mode: 'single'
    //            }
    //        };

    //        newObj.dataModel = {
    //            data: obj.data,
    //            paging: "local",
    //            rPP: 15,
    //            rPPOptions: [10, 15, 20, 50, 100]
    //        };
    //        newObj.colModel = obj.colModel;

    //        newObj.colModel[0].width = 100;
    //        newObj.colModel[1].width = 100;
    //        newObj.colModel[2].width = 100;
    //        newObj.colModel[0].hidden = true;
    //        //append or prepend the CRUD toolbar to .pq-grid-top or .pq-grid-bottom
    //        $("#grid_crud").on("pqgridrender", function (evt, obj) {
    //            var $toolbar = $("<div class='pq-grid-toolbar pq-grid-toolbar-crud'></div>").appendTo($(".pq-grid-top", this));

    //            $("<a class='btn  btn-xs'  title='Add Record'><i class='fa fa-plus-circle '></i></a>").appendTo($toolbar).button({
    //                icons: {
    //                    primary: "ui-icon-circle-plus"
    //                }
    //            }).click(function (evt) {
    //                addRow();
    //            });
    //            $("<a class='btn  btn-xs'  title='Edit Record'><i class='fa fa-edit '></i></a>").appendTo($toolbar).button({
    //                icons: {
    //                    primary: "ui-icon-pencil"
    //                }
    //            }).click(function (evt) {
    //                editRow();
    //            });
    //            $("<a class='btn  btn-xs'  title='Delete Record'><i class='fa fa-trash-o '></i></a>").appendTo($toolbar).button({
    //                icons: {
    //                    primary: "ui-icon-circle-minus"
    //                }
    //            }).click(function () {
    //                deleteRow();
    //            });
    //            $toolbar.disableSelection();
    //        });

    //        var $grid = $("#grid_crud").pqGrid(newObj);
    //        //create popup dialog.
    //        $("#popup-dialog-tosplan").dialog({
    //            width: 300,
    //            modal: true,
    //            open: function () {
    //                $(".ui-dialog").position({
    //                    of: "#grid_crud"
    //                });
    //            },
    //            autoOpen: false
    //        });

    //        //edit Row
    //        function editRow() {
    //            var strMessage = "";
    //            AppPrivileges.GetFormPrivileges("Type Of Service", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
    //                if (strMessage == "") {
    //                    var rowIndx = getRowIndx();
    //                    if (rowIndx != null) {
    //                        var DM = $grid.pqGrid("option", "dataModel");
    //                        var data = DM.data;
    //                        var row = data[rowIndx];
    //                        var $frm = $("form#frmTOSPlanInfo");
    //                        var optVal = $("#ddlInsurancePlan option:contains('" + row[1] + "')").attr('value');
    //                        $("#ddlInsurancePlan").val(optVal)
    //                        $frm.find("input[name='Code']").val(row[2]);

    //                        $("#popup-dialog-tosplan").dialog({
    //                            title: "Edit Record (" + (rowIndx + 1) + ")",
    //                            buttons: {
    //                                Update: function () {
    //                                    //save the record in DM.data.
    //                                    var planId = $frm.find("select[name='Plan']").val();
    //                                    row[1] = $frm.find("select[name='Plan'] option[value='" + planId + "']").text();
    //                                    row[2] = $frm.find("input[name='Code']").val();

    //                                    typeOfServiceDetail.UpdateTypeOfServicePlanInfo(planId, row[2], row[0]);

    //                                    $grid.pqGrid("refreshRow", {
    //                                        rowIndx: rowIndx
    //                                    }).pqGrid('setSelection', {
    //                                        rowIndx: rowIndx
    //                                    });
    //                                    $(this).dialog("close");
    //                                },
    //                                Cancel: function () {
    //                                    $(this).dialog("close");
    //                                }
    //                            }
    //                        }).dialog("open");
    //                    }
    //                }
    //                else
    //                    utility.DisplayMessages(strMessage, 2);
    //            });
    //        }

    //        //append Row
    //        function addRow() {
    //            var strMessage = "";
    //            AppPrivileges.GetFormPrivileges("Type Of Service", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
    //                if (strMessage == "") {
    //                    //debugger;
    //                    var DM = $grid.pqGrid("option", "dataModel");
    //                    var data = DM.data;

    //                    var $frm = $("form#frmTOSPlanInfo");
    //                    $frm.find("input").val("");
    //                    $frm.find("select").val("");
    //                    //$frm.find("select[name='Plan'] option[value='']").attr('selected', 'selected');

    //                    $("#popup-dialog-tosplan").dialog({
    //                        title: "Add Record",
    //                        buttons: {
    //                            Add: function () {
    //                                var row = [];
    //                                //save the record in DM.data.
    //                                var planId = $frm.find("select[name='Plan']").val();
    //                                row[1] = $frm.find("select[name='Plan'] option[value='" + planId + "']").text();
    //                                row[2] = $frm.find("input[name='Code']").val();

    //                                typeOfServiceDetail.SaveTypeOfServicePlanInfo(planId, row[2]).done(function (response) {
    //                                    if (response.status != false) {
    //                                        row[0] = response.TOSPlanId;
    //                                    }
    //                                });

    //                                data.push(row);
    //                                $grid.pqGrid("refreshDataAndView");
    //                                $(this).dialog("close");
    //                            },
    //                            Cancel: function () {
    //                                $(this).dialog("close");
    //                            }
    //                        }
    //                    });
    //                    $("#popup-dialog-tosplan").dialog("open");
    //                }
    //                else
    //                    utility.DisplayMessages(strMessage, 2);
    //            });
    //        }

    //        //delete Row.
    //        function deleteRow() {
    //            var strMessage = "";
    //            AppPrivileges.GetFormPrivileges("Type Of Service", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
    //                if (strMessage == "") {
    //                    var rowIndx = getRowIndx();
    //                    if (rowIndx != null) {
    //                        var DM = $grid.pqGrid("option", "dataModel");
    //                        typeOfServiceDetail.DeleteTypeOfServicePlanInfo(DM.data[rowIndx][0]);
    //                        DM.data.splice(rowIndx, 1);
    //                        $grid.pqGrid("refreshDataAndView");
    //                        $grid.pqGrid("setSelection", {
    //                            rowIndx: rowIndx
    //                        });
    //                    }
    //                }
    //                else
    //                    utility.DisplayMessages(strMessage, 2);
    //            });
    //        }

    //        function getRowIndx() {
    //            //debugger;
    //            var arr = $grid.pqGrid("selection", {
    //                type: 'row',
    //                method: 'getSelection'
    //            });
    //            if (arr && arr.length > 0) {
    //                var rowIndx = arr[0].rowIndx;
    //                return rowIndx;
    //            } else {
    //                //alert("Select a row.");
    //                utility.DisplayMessages("Select a row.", 3);
    //                return null;
    //            }
    //        }
    //    });
    //}




    //TypeOfServiceInfo: function () {
    //    (function ($) {
    //        'use strict';
    //        var mode;
    //        var EditableTable = {
    //            options: {
    //                addButton: '#addToTable',
    //                table: '#datatable-editable',
    //                dialog: {
    //                    wrapper: '#dialog',
    //                    cancelButton: '#dialogCancel',
    //                    confirmButton: '#dialogConfirm',
    //                }
    //            },

    //            initialize: function () {
    //                this
    //                    .setVars()
    //                    .build()
    //                    .events();
    //            },

    //            setVars: function () {
    //                this.$table = $(this.options.table);
    //                this.$addButton = $(this.options.addButton);

    //                // dialog
    //                this.dialog = {};
    //                this.dialog.$wrapper = $(this.options.dialog.wrapper);
    //                this.dialog.$cancel = $(this.options.dialog.cancelButton);
    //                this.dialog.$confirm = $(this.options.dialog.confirmButton);
    //                return this;
    //            },

    //            build: function () {
    //                this.datatable = this.$table.DataTable({
    //                    paging: false,
    //                    searching: false,
    //                    aoColumns: [
    //                        { "bSortable": false },
    //                        null,
    //                        null
    //                    ]
    //                });

    //                var oTable = this.$table.DataTable();

    //                typeOfServiceDetail.LoadTypeOfServicePlanInfo().done(function (response) {
    //                    if (response.status != false) {
    //                        $("#datatable-editable tbody").find("tr").remove();
    //                        if (response.TOSPlanCount > 0) {
    //                            var TypeOfServicePlanJSON = JSON.parse(response.TOSPlan_JSON);
    //                            $.each(TypeOfServicePlanJSON, function (i, item) {
    //                                var actions,
    //                                    data;
    //                                var $row;
    //                                actions = [
    //                                   '<a href="#" class="hidden on-editing save-row"><i class="fa fa-save"></i></a>',
    //                                   '<a href="#" class="hidden on-editing cancel-row"><i class="fa fa-times"></i></a>',
    //                                   '<a href="#" class="on-default edit-row"><i class="fa fa-pencil"></i></a>'
    //                                   //, '<a href="#" class="on-default remove-row"><i class="fa fa-trash-o"></i></a>'
    //                                ].join(' ');

    //                                data = oTable.row.add([actions, item.InsurancePlanName, item.Name]);
    //                                $row = oTable.row(data[0]).nodes().to$();
    //                                $row.attr("TOSPlanId", item.PlanOfTOSId)
    //                                $row.addClass('adding').find('td:first').addClass('actions');
    //                                $("#datatable-editable tbody").last().append($row);
    //                            });
    //                        }
    //                    }
    //                });

    //                window.dt = this.datatable;
    //                return this;
    //            },

    //            events: function () {
    //                var _self = this;

    //                this.$table
    //                    .on('click', 'a.save-row', function (e) {
    //                        e.preventDefault();

    //                        _self.rowSave($(this).closest('tr'));
    //                    })
    //                    .on('click', 'a.cancel-row', function (e) {
    //                        e.preventDefault();

    //                        _self.rowCancel($(this).closest('tr'));
    //                    })
    //                    .on('click', 'a.edit-row', function (e) {
    //                        e.preventDefault();

    //                        _self.rowEdit($(this).closest('tr'));
    //                    })
    //                    .on('click', 'a.remove-row', function (e) {
    //                        e.preventDefault();

    //                        var $row = $(this).closest('tr');

    //                        $.magnificPopup.open({
    //                            items: {
    //                                src: '#dialog',
    //                                type: 'inline'
    //                            },
    //                            preloader: false,
    //                            modal: true,
    //                            callbacks: {
    //                                change: function () {
    //                                    _self.dialog.$confirm.on('click', function (e) {
    //                                        e.preventDefault();

    //                                        _self.rowRemove($row);
    //                                        $.magnificPopup.close();
    //                                    });
    //                                },
    //                                close: function () {
    //                                    _self.dialog.$confirm.off('click');
    //                                }
    //                            }
    //                        });
    //                    });

    //                this.$addButton.on('click', function (e) {
    //                    e.preventDefault();

    //                    _self.rowAdd();
    //                });

    //                this.dialog.$cancel.on('click', function (e) {
    //                    e.preventDefault();
    //                    $.magnificPopup.close();
    //                });

    //                return this;
    //            },

    //            // ==========================================================================================
    //            // ROW FUNCTIONS
    //            // ==========================================================================================
    //            rowAdd: function () {
    //                this.$addButton.attr({ 'disabled': 'disabled' });

    //                var actions,
    //                    data,
    //                    $row;

    //                actions = [
    //                    '<a href="#" class="hidden on-editing save-row"><i class="fa fa-save"></i></a>',
    //                    '<a href="#" class="hidden on-editing cancel-row"><i class="fa fa-times"></i></a>',
    //                    '<a href="#" class="on-default edit-row"><i class="fa fa-pencil"></i></a>'
    //                    //, '<a href="#" class="on-default remove-row"><i class="fa fa-trash-o"></i></a>'
    //                ].join(' ');

    //                data = this.datatable.row.add([actions, '', '']);
    //                $row = this.datatable.row(data[0]).nodes().to$();
    //                mode = "Add";
    //                $row
    //                    .addClass('adding')
    //                    .find('td:first')
    //                    .addClass('actions');

    //                this.rowEdit($row);

    //                this.datatable.order([0, 'asc']).draw(); // always show fields
    //            },

    //            rowCancel: function ($row) {
    //                var _self = this,
    //                    $actions,
    //                    i,
    //                    data;

    //                if ($row.hasClass('adding')) {
    //                    this.rowRemove($row);
    //                } else {
    //                    data = this.datatable.row($row.get(0)).data();
    //                    this.datatable.row($row.get(0)).data(data);

    //                    $actions = $row.find('td.actions');
    //                    if ($actions.get(0)) {
    //                        this.rowSetActionsDefault($row);
    //                    }

    //                    this.datatable.draw();
    //                }
    //            },

    //            rowEdit: function ($row) {
    //                var _self = this,
    //                    data;

    //                var $select = $('<select />');
    //                MDVisionService.lookups("GetInsurancePlan").done(function (results) {
    //                    $.each(results, function (j, result) {
    //                        $select.append($("<option />").val(result.Value).text(result.Name));
    //                    })
    //                });

    //                data = this.datatable.row($row.get(0)).data();
    //                if (mode == "Add")
    //                    mode = "Add";
    //                else
    //                    mode = "Edit";
    //                $row.children('td').each(function (i) {
    //                    var $this = $(this);

    //                    if ($this.hasClass('actions')) {
    //                        _self.rowSetActionsEditing($row);
    //                    } else {
    //                        $this.html('<input type="text" class="form-control input-block" value="' + data[i] + '"/>');
    //                        //$this.html('<select id="ddlPlanCategory" class="form-control" ddlist="GetPlanCategory"></select>');
    //                    }
    //                });
    //            },

    //            rowSave: function ($row) {
    //                var _self = this,
    //                    $actions,
    //                    values = [];

    //                if ($row.hasClass('adding')) {
    //                    this.$addButton.removeAttr('disabled');
    //                    $row.removeClass('adding');
    //                }

    //                values = $row.find('td').map(function () {
    //                    var $this = $(this);

    //                    if ($this.hasClass('actions')) {
    //                        _self.rowSetActionsDefault($row);
    //                        return _self.datatable.cell(this).data();
    //                    } else {
    //                        return $.trim($this.find('input').val());
    //                    }
    //                });
    //                if (mode == "Edit") {
    //                    var ProvLicenseId = $row.attr("TOSPlanId");
    //                    typeOfServiceDetail.UpdateTypeOfServicePlanInfo(values[1], values[2], ProvLicenseId);
    //                    mode = "";
    //                }
    //                else {
    //                    typeOfServiceDetail.SaveTypeOfServicePlanInfo(values[1], values[2]).done(function (response) {
    //                        if (response.status != false) {
    //                            $row.attr("TOSPlanId", response.Id)
    //                        }
    //                    });
    //                    mode = "";
    //                }

    //                this.datatable.row($row.get(0)).data(values);

    //                $actions = $row.find('td.actions');
    //                if ($actions.get(0)) {
    //                    this.rowSetActionsDefault($row);
    //                }

    //                this.datatable.draw();
    //            },

    //            rowRemove: function ($row) {
    //                if ($row.hasClass('adding')) {
    //                    this.$addButton.removeAttr('disabled');
    //                }

    //                this.datatable.row($row.get(0)).remove().draw();
    //            },

    //            rowSetActionsEditing: function ($row) {
    //                $row.find('.on-editing').removeClass('hidden');
    //                $row.find('.on-default').addClass('hidden');
    //            },

    //            rowSetActionsDefault: function ($row) {
    //                $row.find('.on-editing').addClass('hidden');
    //                $row.find('.on-default').removeClass('hidden');
    //            }
    //        };

    //        $(function () {
    //            EditableTable.initialize();
    //        });

    //    }).apply(this, [jQuery]);

    //}
}