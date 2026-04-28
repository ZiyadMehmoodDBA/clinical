Admin_DrugCodeCost_Detail = {
    params: [],

    Load: function (params) {
        Admin_DrugCodeCost_Detail.params = params;
        // DrugCodeCostDetail.bIsFirstLoad = true;

        var self = $('#DrugCodeCostDetail');
        self.loadDropDowns(true).done(function () {

            Admin_DrugCodeCost_Detail.LoadDrugCodeCostDetail();

        });
    },

    LoadDrugCodeCostDetail: function () {
        $("#DrugCodeCostDetail #pnlAdminDrugCodeCost").removeClass('disableAll');
        if (Admin_DrugCodeCost_Detail.params.mode == "Add") {


            $('#basicFeeScheduleDetail #pnlAdminDrugCodeCost').addClass("disableAll");
            Admin_DrugCodeCost_Detail.ValidateDrugCodeCost();

            //serialize data
            $('#frmDrugCodeCostDetailDetail').data('serialize', $('#frmDrugCodeCostDetailDetail').serialize());
        }
        else if (Admin_DrugCodeCost_Detail.params.mode == "Edit") {
            Admin_DrugCodeCost_Detail.FillDrugCodeCost(Admin_DrugCodeCost_Detail.params.CPTCodeCostID).done(function (response) {
                if (response.status != false) {
                    var DrugCodeCost_detail = JSON.parse(response.DrugCodeCostFill_JSON);
                    var self = $("#DrugCodeCostDetail");
                    utility.bindMyJSON(true, DrugCodeCost_detail, false, self);
                    if (DrugCodeCost_detail.chkActive == 'True')
                        $("#DrugCodeCostDetail #chkIsActive").attr("checked", true);
                    else
                        $("#DrugCodeCostDetail #chkIsActive").attr("checked", false);
                    Admin_DrugCodeCost_Detail.ValidateDrugCodeCost();
                    $('#frmDrugCodeCostDetailDetail').data('serialize', $('#frmDrugCodeCostDetailDetail').serialize());
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    ValidateDrugCodeCost: function () {
        $('#frmDrugCodeCostDetailDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   CPTCode: {
                       group: '.col-md-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Cost: {
                       group: '.col-md-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                           numeric: {
                               message: ''
                           },
                           stringLength: {
                               max: 10,
                               message: 'Fee must be less then 10 digits'
                           }
                       }
                   }
               }
           }).on('success.form.bv', function (e) {
               e.preventDefault();
               Admin_DrugCodeCost_Detail.DrugCodeCostSave();
           });
    },

    DrugCodeCostSave: function () {
        var self = $("#DrugCodeCostDetail");
        var myJSON = self.getMyJSON();
        var CPTCode = $('#DrugCodeCostDetail #txtCPTCode').val();

        if (Admin_DrugCodeCost_Detail.params.mode == "Add") {
            var strMessage = "";
            AppPrivileges.GetFormPrivileges("DrugCodeCost", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_CPTCode.ValidateCPTCode(CPTCode).done(function (res) {
                        if (res.length != 0) {

                            if (Admin_DrugCodeCost_Detail.params.DrugCodeCostId == null) {
                                Admin_DrugCodeCost_Detail.SaveDrugCodeCost(myJSON).done(function (response) {
                                    if (response.status != false) {

                                        $("#basicFeeScheduleDetail #pnlAdminDrugCodeCost").removeClass('disableAll');
                                        Admin_DrugCodeCost_Detail.params.DrugCodeCostId = response.CPTCodeCostID;

                                        //Editable Grid
                                        //var PanelGrid = "#basicFeeScheduleDetail #pnlAdminDrugCodeCost";
                                        //var GridId = "#basicFeeScheduleDetail #dgvBFSPlanInfo";
                                       // utility.MakeEditableGrid(PanelGrid, GridId, basicFeeScheduleDetail);

                                        Admin_Drug_Code_Cost.DrugCodeCostSearch(response.DrugCodeCostId);
                                       
                                        utility.DisplayMessages(response.message, 1);
                                        // Admin_DrugCodeCost_Detail.UnloadPan();
                                        
                                        //serialize data
                                        $('#frmDrugCodeCostDetailDetail').data('serialize', $('#frmDrugCodeCostDetailDetail').serialize());
                                        UnloadActionPan();
                                    }
                                    else {
                                        utility.DisplayMessages(response.Message, 3);
                                    }
                                });
                            }
                            else if (Admin_DrugCodeCost_Detail.params.CPTCodeCostID != "-1" && Admin_DrugCodeCost_Detail.params.CPTCodeCostID != "" && basicFeeScheduleDetail.params.BasicFeeScheduleId != "0") {
                                Admin_DrugCodeCost_Detail.UpdateDrugCodeCost(myJSON, Admin_DrugCodeCost_Detail.params.CPTCodeCostID).done(function (response) {
                                    if (response.status != false) {
                                        Admin_Drug_Code_Cost.DrugCodeCostSearch(Admin_DrugCodeCost_Detail.params.CPTCodeCostID);
                                        utility.DisplayMessages(response.message, 1);
                                        //CacheManager.BindCodes('GetBasicFee',true);
                                    }
                                    else {
                                        utility.DisplayMessages(response.Message, 3);
                                    }
                                });
                            }
                        }
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else if (Admin_DrugCodeCost_Detail.params.mode == "Edit") {
            var strMessage = "";
            AppPrivileges.GetFormPrivileges("DrugCodeCost", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {

                    Admin_CPTCode.ValidateCPTCode(CPTCode).done(function (res) {

                        if (res.length != 0) {

                            Admin_DrugCodeCost_Detail.UpdateDrugCodeCost(myJSON, Admin_DrugCodeCost_Detail.params.CPTCodeCostID).done(function (response) {
                                if (response.status != false) {
                                    Admin_Drug_Code_Cost.DrugCodeCostSearch(Admin_DrugCodeCost_Detail.params.CPTCodeCostID);
                                    utility.DisplayMessages(response.message, 1);

                                    UnloadActionPan();
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
    },

    // ------------ Basic Fee Schedule Plan Region (Detail Grid)

    BasicFeeSchedulePlanGridLoad: function (response) {

        if (response.BFSPlanCount > 0) {
            var BasicFeeSchedulePlanJSON = JSON.parse(response.BFSPlan_JSON);

            // get Actions
            var actions = "";
            $("#basicFeeScheduleDetail #dgvBFSPlanInfo tr th").each(function () {
                if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                    var arrActionType = [];
                    if ($(this).attr("ActionType") != null) {
                        arrActionType = $(this).attr("ActionType").split(',');
                        actions = EditableGrid.GetActions(arrActionType);
                    }
                }
            });

            $.each(BasicFeeSchedulePlanJSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($(this))");
                $row.attr("id", item.BasicFeeSchModifierFeeSchId);

                if (item.IsRequired.toLowerCase() == "true")
                {
                    item.IsRequired = "Yes"
                }
                else {
                    item.IsRequired = "No"
                }

                $row.append('<td class="actions" id="' + item.BasicFeeSchModifierFeeSchId + '" >' + actions + '</td><td>' + item.ModifierCode + '</td><td>' + utility.convertToFigure(item.Fee, false) + '</td><td>' + utility.convertToFigure(item.ExpectedFee, false) + '</td><td>' + item.IsRequired + '</td>');

                $("#basicFeeScheduleDetail #dgvBFSPlanInfo tbody").last().append($row);
            });
        }

        //Editable Grid
        var PanelGrid = "#basicFeeScheduleDetail #pnlAdminDrugCodeCost";
        var GridId = "#basicFeeScheduleDetail #dgvBFSPlanInfo";
        utility.MakeEditableGrid(PanelGrid, GridId, basicFeeScheduleDetail);
    },

    DrugCodeCostValidateCode: function (obj) {

        //var entityId = $('#basicFeeScheduleDetail #ddlBasicFeeGroup option:selected').attr('refvalue');
        //utility.ValidateCode(obj, 'CPT', 'basicFeeScheduleDetail #hfCPTCode', entityId);
        utility.ValidateAutoComplete(obj, 'DrugCodeCostDetail #hfCPTCode', true, 1);
    },
    ShowHistory: function (ParentControlId) {
        //var strMessage = "";
        //AppPrivileges.GetFormPrivileges("ERA Adjustment Codes", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        // if (strMessage == "") {
        var params = [];
        params["PanelID"] = 'pnlActivityLog';
       // params["ChargeCapId"] = Admin_DrugCodeCost_Detail.params["ChargeCapId"];
        // params["CPTCode"] = $('#DrugCodeCostDetail #frmDrugCodeCostDetailDetail #txtCPTCode').val();
        params["CPTCodeCostID"] = Admin_DrugCodeCost_Detail.params.CPTCodeCostID;
        params["ParentCtrl"] = 'Admin_DrugCodeCost_Detail';
        LoadActionPan('Activity_Log', params, ParentControlId);
        // }
        // else
        //    utility.DisplayMessages(strMessage, 2);
        // });
    },

    BindAutoComplete: function (element) {

        var entityId = $('#DrugCodeCostDetail #ddlBasicFeeGroup option:selected').attr('refvalue');
        var hiddenCrtl = $("#DrugCodeCostDetail #hfCPTCode");
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, entityId, true, -1, "CPT", true, "DrugCodeCostDetail", null, true);

        //if (globalAppdata['IMO_ID'] == "") {
        //    CacheManager.BindAutoCompleteText('#basicFeeScheduleDetail #txtCPTCode', 'GetCPTCode', true, '#basicFeeScheduleDetail #hfCPTCode', entityId);
        //}
        //else {
        //    utility.BindAutoCompleteText('#basicFeeScheduleDetail #txtCPTCode', 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', '#basicFeeScheduleDetail #hfCPTCode', entityId);
        //}

    },

    LoadEntityBasedData: function () {

        var entityID = $("#basicFeeScheduleDetail #ddlBasicFeeGroup option:selected").attr("refvalue");

        $('#basicFeeScheduleDetail #hfCPTCode').val("");
        $('#basicFeeScheduleDetail #txtCPTCode').val("");
    },

    ResetHiddenValue: function () {
        var $cptCode = $('#basicFeeScheduleDetail #txtCPTCode');
        $cptCode.val($cptCode.val().replace(/[^a-zA-Z0-9]/g, function () {
            return '';
        }));
        $('#basicFeeScheduleDetail #hfCPTCode').val("-1");
    },

    SaveBasicFeeSchedulePlan: function (BasicFeeSchedulePlanData, RowId) {

        var data = "BasicFeeSchedulePlanData=" + BasicFeeSchedulePlanData + "&BasicFeeScheduleId=" + basicFeeScheduleDetail.params.BasicFeeScheduleId + "&RowId=" + RowId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_DRUGCODECOST_DETAIL", "SAVE_DRUGCODECOST");
    },

    UpdateBasicFeeSchedulePlan: function (BasicFeeSchedulePlanData, BasicFeeSchedulePlanID) {
        var data = "BasicFeeSchedulePlanData=" + BasicFeeSchedulePlanData + "&BasicFeeSchedulePlanID=" + BasicFeeSchedulePlanID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_BASIC_FEE_SCHEDULE_DETAIL", "UPDATE_BFS_PLAN_INFO");
    },

    //FillBasicFeeSchedulePlan: function (BasicFeeSchedulePlanID) {
    //    var data = "BasicFeeSchedulePlanID=" + BasicFeeSchedulePlanID;
    //    // serach parameter , class name, command name of class 
    //    return MDVisionService.defaultService(data, "ADMIN_BASIC_FEE_SCHEDULE_DETAIL", "FILL_BFS_PLAN_INFO");
    //},

    LoadBasicFeeSchedulePlan: function () {
        var data = "BasicFeeScheduleId=" + basicFeeScheduleDetail.params.BasicFeeScheduleId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_BASIC_FEE_SCHEDULE_DETAIL", "LOAD_BFS_PLAN_INFO");
    },

    DeleteBasicFeeSchedulePlan: function (BasicFeeSchedulePlanId) {
        var data = "BasicFeeSchedulePlanId=" + BasicFeeSchedulePlanId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_BASIC_FEE_SCHEDULE_DETAIL", "DELETE_BFS_PLAN_INFO");
    },

    OpenModifierDetail: function (optional, Ctrl, HiddenCtrl) {

        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "basicFeeScheduleDetail";
        if (Ctrl != null) {
            params["RefCtrl"] = Ctrl;
        }
        if (HiddenCtrl != null) {
            params["RefHiddenIdCtrl"] = HiddenCtrl;
        }
        LoadActionPan('Admin_Modifier', params);
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
                    $obj_ = $this.find('input');

                    if ($obj_.attr('type') == "checkbox") {
                        if ($obj_.prop('checked'))
                            return $.trim("Yes");
                        else
                            return $.trim("No");
                    }
                    else
                        return $.trim($obj_.val());
                }
            });

            var id = $row.attr("id");

            Admin_Modifier.ValidateModifierCode($("#basicFeeScheduleDetail #txtModifier" + id).val()).done(function (res) {
                if (res.length != 0) {

                    //set value
                    $("#basicFeeScheduleDetail #hfModifier" + id).val(res[1].Value);
                    var myJSON = $row.getMyJSON();

                    if (id && id > 0) {

                        //Edit Record
                        var strMessage = "";
                        AppPrivileges.GetFormPrivileges("Basic Fee Schedule", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                            if (strMessage == "") {

                                basicFeeScheduleDetail.UpdateBasicFeeSchedulePlan(myJSON, id).done(function (response) {
                                    if (response.status != false) {

                                        utility.DisplayMessages(response.Message, 1);
                                        basicFeeScheduleDetail.rowDraw($row, _self, values);
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

                        AppPrivileges.GetFormPrivileges("Basic Fee Schedule", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                            if (strMessage == "") {
                                basicFeeScheduleDetail.SaveBasicFeeSchedulePlan(myJSON, id).done(function (response) {
                                    if (response.status != false) {

                                        $row.attr("id", response.BFSPlanId);
                                        $row.attr("onclick", "utility.SelectGridRow($(this))");
                                        utility.DisplayMessages(response.Message, 1);
                                        basicFeeScheduleDetail.rowDraw($row, _self, values);
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
            });


        }
    },

    rowAdd: function () {

        AppPrivileges.GetFormPrivileges("Basic Fee Schedule", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                EditableGrid.rowAdd();
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    rowRemove: function ($row, obj) {

        var id = $row.attr("id");
        AppPrivileges.GetFormPrivileges("Basic Fee Schedule", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = id;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        basicFeeScheduleDetail.DeleteBasicFeeSchedulePlan(selectedValue).done(function (response) {
                            if (response.status != false) {

                                if ($row.hasClass('adding')) {
                                }
                                var _self = obj;
                                _self.datatable.row($row.get(0)).remove().draw();

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

    //----------------------------------------------------------------

    SaveDrugCodeCost: function (BasicFeeScheduleData) {
        var data = "DrugCodeCostData=" + BasicFeeScheduleData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_DRUGCODECOST_DETAIL", "SAVE_DRUGCODECOST");
    },

    UpdateDrugCodeCost: function (DrugCodeCostData, DrugCodeCostDataID) {
        var data = "DrugCodeCostData=" + DrugCodeCostData + "&DrugCodeCostDataID=" + DrugCodeCostDataID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_DRUGCODECOST_DETAIL", "UPDATE_DRUGCODECOST");
    },

    FillDrugCodeCost: function (CPTCodeCostID) {
        var data = "CPTCodeCostID=" + CPTCodeCostID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_DRUGCODECOST_DETAIL", "FILL_DRUGCODECOST");
    },

    UpdateDrugCodeCostActiveInactive: function (DrugCodeCostID, IsActive) {
        var data = "DrugCodeCostID=" + DrugCodeCostID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_DRUGCODECOST_DETAIL", "UPDATE_DRUGCODECOST_ACTIVE_INACTIVE");
    },

    // -------------- CPT Code -----------------

    OpenCPTCode: function () {
        var params = [];
        //params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Admin_DrugCodeCost_Detail";
        params["EntityId"] = $('#DrugCodeCostDetail #txtCPTCode option:selected').attr('refvalue');
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtCPTCode";

        DrugCodeCostDetail
        LoadActionPan('Admin_IMOCPT', params);
    },

    // -------------- End CPT Code -------------

    //SaveBasicFeeSchedulePlanInfo: function (BFSPlan, BFS) {
    //    var data = "BFSPlan=" + BFSPlan + "&BFS=" + BFS + "&BasicFeeScheduleID=" + basicFeeScheduleDetail.params.BasicFeeScheduleId;
    //    // serach parameter , class name, command name of class 
    //    return MDVisionService.defaultService(data, "ADMIN_BASIC_FEE_SCHEDULE_DETAIL", "SAVE_BFS_PLAN_INFO");
    //},

    //UpdateBasicFeeSchedulePlanInfo: function (BFSPlan, BFS, BFSPlanId) {
    //    var data = "BFSPlan=" + BFSPlan + "&BFS=" + BFS + "&BFSPlanId=" + BFSPlanId + "&BasicFeeScheduleID=" + basicFeeScheduleDetail.params.BasicFeeScheduleId;
    //    // serach parameter , class name, command name of class 
    //    return MDVisionService.defaultService(data, "ADMIN_BASIC_FEE_SCHEDULE_DETAIL", "UPDATE_BFS_PLAN_INFO");
    //},

    //LoadBasicFeeSchedulePlanInfo: function () {
    //    var data = "BasicFeeScheduleID=" + basicFeeScheduleDetail.params.BasicFeeScheduleId;
    //    // serach parameter , class name, command name of class 
    //    return MDVisionService.defaultService(data, "ADMIN_BASIC_FEE_SCHEDULE_DETAIL", "LOAD_BFS_PLAN_INFO");
    //},

    //DeleteBasicFeeSchedulePlanInfo: function (BFSPlanId) {
    //    var data = "BFSPlanId=" + BFSPlanId;
    //    // serach parameter , class name, command name of class 
    //    return MDVisionService.defaultService(data, "ADMIN_BASIC_FEE_SCHEDULE_DETAIL", "DELETE_BFS_PLAN_INFO");
    //},

    UnLoad: function () {
        
        utility.UnLoadDialog("frmDrugCodeCostDetailDetail", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });
    },
    UnloadPan: function () {

        if (Admin_DrugCodeCost_Detail.params != null && Admin_DrugCodeCost_Detail.params.ParentCtrl != null) {
            UnloadActionPan(Admin_DrugCodeCost_Detail.params.ParentCtrl, 'DrugCodeCostDetail');
        }
        else
            UnloadActionPan();
    },
    //LoadBFSPlans: function () {
    //    basicFeeScheduleDetail.LoadBasicFeeSchedulePlanInfo().done(function (response) {
    //        if (response.status != false) {
    //            if (response.BFSPlanCount > 0) {
    //                var BFSPlanJSON = JSON.parse(response.BFSPlan_JSON);
    //                $.each(BFSPlanJSON, function (i, item) {
    //                    var $row = $('<tr/>');
    //                    $row.attr("BFSPlanId", item.BasicFeeSchModifierFeeSchId)
    //                    $row.append('<td style="display:none;">' + item.BasicFeeSchModifierFeeSchId + '</td><td>' + item.InsurancePlanName + '</td><td>' + item.Name + '</td>');
    //                    $("#dgvBFSPlanInfo tbody").last().append($row);
    //                });
    //            }
    //        }
    //    });
    //},

    //BasicFeeScheduleInfo: function () {
    //    $(function () {
    //        var self = $('#gridform');
    //        self.loadDropDowns(true);
    //        var tbl = $("#dgvBFSPlanInfo");
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
    //            title: "MODIFIER FEE",
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
    //        newObj.colModel[3].width = 80;
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
    //        $("#popup-dialog-bfsplan").dialog({
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
    //            AppPrivileges.GetFormPrivileges("Basic Fee Schedule", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
    //                if (strMessage == "") {
    //                    var rowIndx = getRowIndx();
    //                    if (rowIndx != null) {
    //                        var DM = $grid.pqGrid("option", "dataModel");
    //                        var data = DM.data;
    //                        var row = data[rowIndx];
    //                        var $frm = $("form#frmBFSPlanInfo");
    //                        var optVal = $("#ddlModifier option:contains('" + row[1] + "')").attr('value');
    //                        $("#ddlModifier").val(optVal)
    //                        $frm.find("input[name='Code']").val(row[2]);

    //                        $("#popup-dialog-bfsplan").dialog({
    //                            title: "Edit Record (" + (rowIndx + 1) + ")",
    //                            buttons: {
    //                                Update: function () {
    //                                    //save the record in DM.data.
    //                                    var planId = $frm.find("select[name='Plan']").val();
    //                                    row[1] = $frm.find("select[name='Plan'] option[value='" + planId + "']").text();
    //                                    row[2] = $frm.find("input[name='Code']").val();

    //                                    basicFeeScheduleDetail.UpdateBasicFeeSchedulePlanInfo(planId, row[2], row[0]);

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
    //            AppPrivileges.GetFormPrivileges("Basic Fee Schedule", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
    //                if (strMessage == "") {
    //                    //debugger;
    //                    var DM = $grid.pqGrid("option", "dataModel");
    //                    var data = DM.data;

    //                    var $frm = $("form#frmBFSPlanInfo");
    //                    $frm.find("input").val("");
    //                    $frm.find("select").val("");
    //                    //$frm.find("select[name='Plan'] option[value='']").attr('selected', 'selected');

    //                    $("#popup-dialog-bfsplan").dialog({
    //                        title: "Add Record",
    //                        buttons: {
    //                            Add: function () {
    //                                var row = [];
    //                                //save the record in DM.data.
    //                                var planId = $frm.find("select[name='Plan']").val();
    //                                row[1] = $frm.find("select[name='Plan'] option[value='" + planId + "']").text();
    //                                row[2] = $frm.find("input[name='Code']").val();

    //                                basicFeeScheduleDetail.SaveBasicFeeSchedulePlanInfo(planId, row[2]).done(function (response) {
    //                                    if (response.status != false) {
    //                                        row[0] = response.BFSPlanId;
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
    //                    $("#popup-dialog-bfsplan").dialog("open");
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
    //                        basicFeeScheduleDetail.DeleteBasicFeeSchedulePlanInfo(DM.data[rowIndx][0]);
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
}