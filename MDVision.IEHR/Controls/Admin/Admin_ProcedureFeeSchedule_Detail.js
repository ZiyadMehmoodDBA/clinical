procedureFeeScheduleDetail = {

    params: [],
    Load: function (params) {
        procedureFeeScheduleDetail.params = params;

        var self = $('#procedureFeeScheduleDetail');
        self.loadDropDowns(true).done(function () {

            procedureFeeScheduleDetail.LoadProcedureFeeSchedule();
        });

    },

    ProcedureFeeScheduleValidateCode: function (obj) {

        //var entityId = $('#procedureFeeScheduleDetail #ddlFeeGroup option:selected').attr('refvalue');
        //utility.ValidateCode(obj, 'CPT', 'procedureFeeScheduleDetail #hfCPTCode', entityId);

        utility.ValidateAutoComplete(obj, 'procedureFeeScheduleDetail #hfCPTCode', true, 1);
    },

    BindAutoComplete: function (element) {

        var entityId = $('#procedureFeeScheduleDetail #ddlFeeGroup option:selected').attr('refvalue');
        var hiddenCrtl = $("#procedureFeeScheduleDetail #hfCPTCode");
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, entityId, true, -1, "CPT", true, "procedureFeeScheduleDetail", null, true);

        //var entityId = $('#procedureFeeScheduleDetail #ddlFeeGroup option:selected').attr('refvalue');
        //if (globalAppdata['IMO_ID'] == "") {
        //    CacheManager.BindAutoCompleteText('#procedureFeeScheduleDetail #txtCPTCode', 'GetCPTCode', true, '#procedureFeeScheduleDetail #hfCPTCode', entityId);
        //}
        //else {
        //    utility.BindAutoCompleteText('#procedureFeeScheduleDetail #txtCPTCode', 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', '#procedureFeeScheduleDetail #hfCPTCode', entityId);
        //}

    },

    LoadProcedureFeeSchedule: function () {
        $("#procedureFeeScheduleDetail #pnlPFSPlanInfo").removeClass('disableAll');
        if (procedureFeeScheduleDetail.params.mode == "Add") {

            //serialize data
            $('#procedureFeeScheduleDetail #pnlPFSPlanInfo').addClass("disableAll");
            $('#frmProcedureFeeScheduleDetail').data('serialize', $('#frmProcedureFeeScheduleDetail').serialize());
            procedureFeeScheduleDetail.ValidateProcedureFeeSchedule();
        }
        else if (procedureFeeScheduleDetail.params.mode == "Edit") {
            procedureFeeScheduleDetail.FillProcedureFeeSchedule(procedureFeeScheduleDetail.params.ProcedureFeeScheduleId).done(function (response) {
                if (response.status != false) {
                    var procFeeSchedule_detail = JSON.parse(response.ProcedureFeeScheduleFill_JSON);
                    var self = $("#procedureFeeScheduleDetail");
                    utility.bindMyJSON(true, procFeeSchedule_detail, false, self).done(function () {

                        procedureFeeScheduleDetail.ValidateProcedureFeeSchedule();
                        //serialize data
                        $('#frmProcedureFeeScheduleDetail').data('serialize', $('#frmProcedureFeeScheduleDetail').serialize());

                        procedureFeeScheduleDetail.LoadProcedureFeeSchedulePlan().done(function (response) {
                            if (response.status != false) {
                                procedureFeeScheduleDetail.ProcedureFeeSchedulePlanGridLoad(response);
                                //serialize data
                                $('#frmBFSPlanInfo').data('serialize', $('#frmBFSPlanInfo').serialize());
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

    LoadEntityBasedData: function () {
        var entityID = $('#procedureFeeScheduleDetail #ddlFeeGroup option:selected').attr("refvalue");

        if (entityID != "") {
            CacheManager.BindDropDownsByEntityID('#procedureFeeScheduleDetail #ddlPlanFeeLink', 'GetPlanFeeLink', false, entityID);
            // CacheManager.BindDropDownsByEntityID(ControlPanel + ' #ddlCPTCode', 'GetCPTCode', false, entityID);
        }
        else {
            CacheManager.BindDropDownsByEntityID('#procedureFeeScheduleDetail #ddlPlanFeeLink', 'GetPlanFeeLink', true, null);
            //  CacheManager.BindDropDownsByEntityID(ControlPanel + ' #ddlCPTCode', 'GetCPTCode', true, null);
        }

        $('#procedureFeeScheduleDetail #txtCPTCode').val("");
        $('#procedureFeeScheduleDetail #hfCPTCode').val("");
    },

    ValidateProcedureFeeSchedule: function () {
        $('#frmProcedureFeeScheduleDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   FeeGroup: {
                       group: '.col-md-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   PlanFeeLink: {
                       group: '.col-md-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   CPTCode: {
                       group: '.col-md-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Fee: {
                       group: '.col-md-2',
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
               procedureFeeScheduleDetail.ProcedureFeeScheduleSave();
           });
    },

    ProcedureFeeScheduleSave: function () {
        $('#frmProcedureFeeScheduleDetail').data('serialize', $('#frmProcedureFeeScheduleDetail').serialize());
        var self = $("#procedureFeeScheduleDetail");
        var myJSON = self.getMyJSON();
        var CPTCode = $('#procedureFeeScheduleDetail #txtCPTCode').val();

        if (procedureFeeScheduleDetail.params.mode == "Add") {
            var strMessage = "";
            AppPrivileges.GetFormPrivileges("Fee Group Plan CPT", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {

                    Admin_CPTCode.ValidateCPTCode(CPTCode).done(function (res) {
                        if (res.length != 0) {
                            if (procedureFeeScheduleDetail.params.ProcedureFeeScheduleId == "-1") {
                                procedureFeeScheduleDetail.SaveProcedureFeeSchedule(myJSON).done(function (response) {
                                    if (response.status != false) {
                                        $("#procedureFeeScheduleDetail #pnlPFSPlanInfo").removeClass('disableAll');
                                        procedureFeeScheduleDetail.params.ProcedureFeeScheduleId = response.ProcedureFeeScheduleId;

                                        //Editable Grid
                                        var PanelGrid = "#procedureFeeScheduleDetail #pnlPFSPlanInfo";
                                        var GridId = "#procedureFeeScheduleDetail #dgvPFSPlanInfo";
                                        utility.MakeEditableGrid(PanelGrid, GridId, procedureFeeScheduleDetail);
                                        Admin_ProcedureFeeSchedule.ProcedureFeeScheduleSearch(response.ProcedureFeeScheduleId);

                                        utility.DisplayMessages(response.message, 1);
                                        //UnloadActionPan();

                                        $('#frmProcedureFeeScheduleDetail').data('serialize', $('#frmProcedureFeeScheduleDetail').serialize());
                                    }
                                    else {
                                        utility.DisplayMessages(response.Message, 3);
                                    }
                                });
                            }
                            else if (procedureFeeScheduleDetail.params.ProcedureFeeScheduleId != "-1" && procedureFeeScheduleDetail.params.ProcedureFeeScheduleId != "" && procedureFeeScheduleDetail.params.ProcedureFeeScheduleId != "0") {
                                procedureFeeScheduleDetail.UpdateProcedureFeeSchedule(myJSON, procedureFeeScheduleDetail.params.ProcedureFeeScheduleId).done(function (response) {
                                    if (response.status != false) {

                                        Admin_ProcedureFeeSchedule.ProcedureFeeScheduleSearch(procedureFeeScheduleDetail.params.ProcedureFeeScheduleId);
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
        else if (procedureFeeScheduleDetail.params.mode == "Edit") {
            var strMessage = "";
            AppPrivileges.GetFormPrivileges("Fee Group Plan CPT", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_CPTCode.ValidateCPTCode(CPTCode).done(function (res) {
                        if (res.length != 0) {

                            procedureFeeScheduleDetail.UpdateProcedureFeeSchedule(myJSON, procedureFeeScheduleDetail.params.ProcedureFeeScheduleId).done(function (response) {
                                if (response.status != false) {
                                    Admin_ProcedureFeeSchedule.ProcedureFeeScheduleSearch(procedureFeeScheduleDetail.params.ProcedureFeeScheduleId);
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

    ResetHiddenValue: function () {
        var $cptCode = $('#procedureFeeScheduleDetail #txtCPTCode');
        $cptCode.val($cptCode.val().replace(/[^a-zA-Z0-9]/g, function () {
            return '';
        }));
        $('#procedureFeeScheduleDetail #hfCPTCode').val("-1");
    },



    // -------------- CPT Code -----------------

    OpenCPTCode: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "procedureFeeScheduleDetail";
        params["EntityId"] = $('#procedureFeeScheduleDetail #ddlFeeGroup option:selected').attr('refvalue');
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtCPTCode";
        LoadActionPan('Admin_IMOCPT', params);
    },

    // -------------- End CPT Code -------------

    // ------------ Fee Group Plan CPT Region (Detail Grid)
    ProcedureFeeSchedulePlanGridLoad: function (response) {

        if (response.PGSPlanCount > 0) {
            var PFeeSchedulePlanJSON = JSON.parse(response.PGSPlan_JSON);

            // get Actions
            var actions = "";
            $("#procedureFeeScheduleDetail #dgvPFSPlanInfo tr th").each(function () {
                if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                    var arrActionType = [];
                    if ($(this).attr("ActionType") != null) {
                        arrActionType = $(this).attr("ActionType").split(',');
                        actions = EditableGrid.GetActions(arrActionType);
                    }
                }
            });

            $.each(PFeeSchedulePlanJSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($(this))");
                $row.attr("id", item.FeeGroupProcModifierFeeSchId);

                if (item.IsRequired.toLowerCase() == "true") {
                    item.IsRequired = "Yes"
                }
                else {
                    item.IsRequired = "No"
                }

                $row.append('<td class="actions" id="' + item.FeeGroupProcModifierFeeSchId + '" >' + actions + '</td><td>' + item.ModifierCode + '</td><td>' + utility.convertToFigure(item.Fee, false) + '</td><td>' + utility.convertToFigure(item.ExpectedFee, false) + '</td><td>' + item.IsRequired + '</td>');

                $("#procedureFeeScheduleDetail #dgvPFSPlanInfo tbody").last().append($row);
            });
        }

        //Editable Grid
        var PanelGrid = "#procedureFeeScheduleDetail #pnlPFSPlanInfo";
        var GridId = "#procedureFeeScheduleDetail #dgvPFSPlanInfo";
        utility.MakeEditableGrid(PanelGrid, GridId, procedureFeeScheduleDetail);
    },

    ResetHiddenValue: function () {

        var $cptCode = $('#procedureFeeScheduleDetail #txtCPTCode');
        $cptCode.val($cptCode.val().replace(/[^a-zA-Z0-9]/g, function () {
            return '';
        }));
        $('#procedureFeeScheduleDetail #hfCPTCode').val("-1");
    },

    SaveProcedureFeeSchedulePlan: function (ProcedureFeeSchedulePlanData, RowId) {
        var data = "ProcedureFeeSchedulePlanData=" + ProcedureFeeSchedulePlanData + "&ProcedureFeeScheduleId=" + procedureFeeScheduleDetail.params.ProcedureFeeScheduleId + "&RowId=" + RowId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROCEDURE_FEE_SCHEDULE_DETAIL", "SAVE_PROCEDURE_FEE_SCHEDULE_PLAN_CPT");
    },

    UpdateProcedureFeeSchedulePlan: function (ProcedureFeeSchedulePlanData, procedureFeeSchedulePlanId) {
        var data = "ProcedureFeeSchedulePlanData=" + ProcedureFeeSchedulePlanData + "&procedureFeeSchedulePlanId=" + procedureFeeSchedulePlanId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROCEDURE_FEE_SCHEDULE_DETAIL", "UPDATE_PROCEDURE_FEE_SCHEDULE_PLAN_CPT");
    },

    //FillProcedureFeeSchedulePlan: function (procedureFeeSchedulePlanId) {
    //    var data = "procedureFeeSchedulePlanId=" + procedureFeeSchedulePlanId;
    //    // serach parameter , class name, command name of class 
    //    return MDVisionService.defaultService(data, "ADMIN_PROCEDURE_FEE_SCHEDULE_DETAIL", "FILL_PROCEDURE_FEE_SCHEDULE_PLAN_CPT");
    //},

    LoadProcedureFeeSchedulePlan: function () {
        var data = "feeGroupProcId=" + procedureFeeScheduleDetail.params.ProcedureFeeScheduleId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROCEDURE_FEE_SCHEDULE_DETAIL", "LOAD_PROCEDURE_FEE_SCHEDULE_PLAN_CPT");
    },

    DeleteProcedureFeeSchedulePlan: function (procedureFeeSchedulePlanId) {
        var data = "procedureFeeSchedulePlanId=" + procedureFeeSchedulePlanId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROCEDURE_FEE_SCHEDULE_DETAIL", "DELETE_PROCEDURE_FEE_SCHEDULE_PLAN_CPT");
    },

    OpenModifierDetail: function (optional, Ctrl, HiddenCtrl) {

        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "procedureFeeScheduleDetail";
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

            Admin_Modifier.ValidateModifierCode($("#procedureFeeScheduleDetail #txtModifier" + id).val()).done(function (res) {

                if (res.length != 0) {

                    //set value
                    $("#procedureFeeScheduleDetail #hfModifier" + id).val(res[1].Value);
                    var myJSON = $row.getMyJSON();

                    if (id && id > 0) {

                        //Edit Record
                        var strMessage = "";
                        AppPrivileges.GetFormPrivileges("Fee Group Plan CPT", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                            if (strMessage == "") {

                                procedureFeeScheduleDetail.UpdateProcedureFeeSchedulePlan(myJSON, id).done(function (response) {
                                    if (response.status != false) {

                                        utility.DisplayMessages(response.Message, 1);
                                        procedureFeeScheduleDetail.rowDraw($row, _self, values);
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

                        AppPrivileges.GetFormPrivileges("Fee Group Plan CPT", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                            if (strMessage == "") {
                                procedureFeeScheduleDetail.SaveProcedureFeeSchedulePlan(myJSON, id).done(function (response) {
                                    if (response.status != false) {

                                        $row.attr("id", response.PGSPlanId);
                                        $row.attr("onclick", "utility.SelectGridRow($(this))");
                                        utility.DisplayMessages(response.Message, 1);
                                        procedureFeeScheduleDetail.rowDraw($row, _self, values);
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

        AppPrivileges.GetFormPrivileges("Fee Group Plan CPT", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                EditableGrid.rowAdd();
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    rowRemove: function ($row, obj) {

        var id = $row.attr("id");
        AppPrivileges.GetFormPrivileges("Fee Group Plan CPT", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = id;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        procedureFeeScheduleDetail.DeleteProcedureFeeSchedulePlan(selectedValue).done(function (response) {
                            if (response.status != false) {

                                if ($row.hasClass('adding')) {
                                }
                                var _self = obj;
                                _self.datatable.row($row.get(0)).remove().draw();


                                //Start 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
                                utility.removePaginationFromGrid($('#' + procedureFeeScheduleDetail.params.PanelID + ' #pnlPFSPlanInfo'));
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

    //---------------------------------------------------------------------

    SaveProcedureFeeSchedule: function (ProcedureFeeScheduleData) {
        var data = "ProcedureFeeScheduleData=" + ProcedureFeeScheduleData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROCEDURE_FEE_SCHEDULE_DETAIL", "SAVE_PROCEDURE_FEE_SCHEDULE");
    },

    UpdateProcedureFeeSchedule: function (ProcedureFeeScheduleData, ProcedureFeeScheduleID) {
        var data = "ProcedureFeeScheduleData=" + ProcedureFeeScheduleData + "&ProcedureFeeScheduleID=" + ProcedureFeeScheduleID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROCEDURE_FEE_SCHEDULE_DETAIL", "UPDATE_PROCEDURE_FEE_SCHEDULE");
    },

    FillProcedureFeeSchedule: function (ProcedureFeeScheduleID) {
        var data = "ProcedureFeeScheduleID=" + ProcedureFeeScheduleID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROCEDURE_FEE_SCHEDULE_DETAIL", "FILL_PROCEDURE_FEE_SCHEDULE");
    },

    UpdateProcedureFeeScheduleActiveInactive: function (ProcedureFeeScheduleID, IsActive) {
        var data = "ProcedureFeeScheduleID=" + ProcedureFeeScheduleID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROCEDURE_FEE_SCHEDULE_DETAIL", "UPDATE_PROCEDURE_FEE_SCHEDULE_ACTIVE_INACTIVE");
    },

    UnLoad: function () {

        utility.UnLoadDialog("frmProcedureFeeScheduleDetail", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });

    },
    ShowHistory: function () {
        var PanelID = 'procedureFeeScheduleDetail';
        var ParentCtrl = 'procedureFeeScheduleDetail';
        var ProfileName = 'Fee Group Plan CPt';
        var DBTableName = 'FeeGroupProceduralSchedule';
        var ColumnKeyId = procedureFeeScheduleDetail.params.ProcedureFeeScheduleId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },

}