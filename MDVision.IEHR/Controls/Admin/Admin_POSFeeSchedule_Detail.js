POSFeeScheduleDetail = {
    params: [],
    Load: function (params) {
        POSFeeScheduleDetail.params = params;

        var self = $('#POSFeeScheduleDetail');
        self.loadDropDowns(true).done(function () {

            POSFeeScheduleDetail.LoadPOSFeeSchedule();
        });

    },

    POSFeeScheduleValidateCode: function (obj) {

        //var entityId = $('#POSFeeScheduleDetail #ddlFeeGroup option:selected').attr('refvalue');
        //utility.ValidateCode(obj, 'CPT', 'POSFeeScheduleDetail #hfCPTCode', entityId);
        utility.ValidateAutoComplete(obj, 'POSFeeScheduleDetail #hfCPTCode', true, 1);
    },

    BindAutoComplete: function (element) {

        var entityId = $('#POSFeeScheduleDetail #ddlFeeGroup option:selected').attr('refvalue');
        var hiddenCrtl = $("#POSFeeScheduleDetail #hfCPTCode");
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, entityId, true, -1, "CPT", true, "POSFeeScheduleDetail", null, true);
        
        //if (globalAppdata['IMO_ID'] == "") {
        //    CacheManager.BindAutoCompleteText('#POSFeeScheduleDetail #txtCPTCode', 'GetCPTCode', true, '#POSFeeScheduleDetail #hfCPTCode', entityId);
        //}
        //else {
        //    utility.BindAutoCompleteText('#POSFeeScheduleDetail #txtCPTCode', 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', '#POSFeeScheduleDetail #hfCPTCode', entityId);
        //}

    },

    LoadPOSFeeSchedule: function () {

        $("#POSFeeScheduleDetail #pnlBFSPlanInfo").removeClass('disableAll');
        if (POSFeeScheduleDetail.params.mode == "Add") {

            $('#POSFeeScheduleDetail #pnlBFSPlanInfo').addClass("disableAll");
            //serialize data
            $('#frmPOSFeeScheduleDetail').data('serialize', $('#frmPOSFeeScheduleDetail').serialize());
            POSFeeScheduleDetail.ValidatePOSFeeSchedule();
        }
        else if (POSFeeScheduleDetail.params.mode == "Edit") {
            POSFeeScheduleDetail.FillPOSFeeSchedule(POSFeeScheduleDetail.params.POSFeeScheduleId).done(function (response) {
                if (response.status != false) {
                    var posFeeSchedule_detail = JSON.parse(response.POSFeeScheduleFill_JSON);
                    var self = $("#POSFeeScheduleDetail");

                    utility.bindMyJSON(true, posFeeSchedule_detail, false, self).done(function () {

                        $("#POSFeeScheduleDetail #pnlBFSPlanInfo").removeClass('disableAll');
                        POSFeeScheduleDetail.ValidatePOSFeeSchedule();

                        //serialize data
                        $('#frmPOSFeeScheduleDetail').data('serialize', $('#frmPOSFeeScheduleDetail').serialize());


                        POSFeeScheduleDetail.LoadPOSSchedulePlan().done(function (response) {
                            if (response.status != false) {
                                POSFeeScheduleDetail.POSFeePlanGridLoad(response);

                                //serialize data
                                $('#frmPOSFeeScheduleDetail').data('serialize', $('#frmPOSFeeScheduleDetail').serialize());
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

    ValidatePOSFeeSchedule: function () {
        $('#frmPOSFeeScheduleDetail')
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
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   PlanFeeLink: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   CPTCode: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   PlaceOfService: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Fee: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                           //numeric: {
                           //    message: ''
                           //},
                           stringLength: {
                               max: 10,
                               message: 'Fee must be less then 10 digits'
                           }
                       }
                   }
               }
           }).on('success.form.bv', function (e) {
               e.preventDefault();
               POSFeeScheduleDetail.POSFeeScheduleSave();
           });
    },

    POSFeeScheduleSave: function () {

        var self = $("#POSFeeScheduleDetail");
        var myJSON = self.getMyJSON();
        var CPTCode = $('#POSFeeScheduleDetail #txtCPTCode').val();

        if (POSFeeScheduleDetail.params.mode == "Add") {
            var strMessage = "";
            AppPrivileges.GetFormPrivileges("Fee Group Plan CPT POS", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_CPTCode.ValidateCPTCode(CPTCode).done(function (res) {
                        if (res.length != 0) {

                            if (POSFeeScheduleDetail.params.POSFeeScheduleId == "-1") {
                                POSFeeScheduleDetail.SavePOSFeeSchedule(myJSON).done(function (response) {
                                    if (response.status != false) {
                                        $("#POSFeeScheduleDetail #pnlBFSPlanInfo").removeClass('disableAll');
                                        POSFeeScheduleDetail.params.POSFeeScheduleId = response.POSFeeScheduleId;

                                        //Editable Grid
                                        var PanelGrid = "#POSFeeScheduleDetail #pnlBFSPlanInfo";
                                        var GridId = "#POSFeeScheduleDetail #dgvBFSPlanInfo";
                                        utility.MakeEditableGrid(PanelGrid, GridId, POSFeeScheduleDetail);

                                        Admin_POSFeeSchedule.POSFeeScheduleSearch(response.POSFeeScheduleId);
                                        utility.DisplayMessages(response.message, 1);

                                        //serialize data
                                        $('#frmPOSFeeScheduleDetail').data('serialize', $('#frmPOSFeeScheduleDetail').serialize());
                                    }
                                    else {
                                        utility.DisplayMessages(response.Message, 1);
                                    }
                                });
                            }
                            else if (POSFeeScheduleDetail.params.POSFeeScheduleId != "-1" && POSFeeScheduleDetail.params.POSFeeScheduleId != "" && POSFeeScheduleDetail.params.POSFeeScheduleId != "0") {
                                POSFeeScheduleDetail.UpdatePOSFeeSchedule(myJSON, POSFeeScheduleDetail.params.POSFeeScheduleId).done(function (response) {
                                    if (response.status != false) {
                                        Admin_POSFeeSchedule.POSFeeScheduleSearch('0');
                                        utility.DisplayMessages(response.message, 1);
                                    }
                                    else {
                                        utility.DisplayMessages(response.Message, 3);
                                    }
                                });
                            }
                        }
                    });

                } else {
                    utility.DisplayMessages(strMessage, 3);
                }
            });
        }
        else if (POSFeeScheduleDetail.params.mode == "Edit") {
            var strMessage = "";
            AppPrivileges.GetFormPrivileges("Fee Group Plan CPT POS", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_CPTCode.ValidateCPTCode(CPTCode).done(function (res) {
                        if (res.length != 0) {

                            POSFeeScheduleDetail.UpdatePOSFeeSchedule(myJSON, POSFeeScheduleDetail.params.POSFeeScheduleId).done(function (response) {
                                if (response.status != false) {
                                    Admin_POSFeeSchedule.POSFeeScheduleSearch(POSFeeScheduleDetail.params.POSFeeScheduleId);
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
        var $cptCode = $('#POSFeeScheduleDetail #txtCPTCode');
        $cptCode.val($cptCode.val().replace(/[^a-zA-Z0-9]/g, function () {
            return '';
        }));
        $('#POSFeeScheduleDetail #hfCPTCode').val("-1");
    },

    LoadEntityBasedData: function () {
        var entityID = $('#POSFeeScheduleDetail #ddlFeeGroup option:selected').attr("refvalue");
        if (entityID != "") {
            CacheManager.BindDropDownsByEntityID('#POSFeeScheduleDetail #ddlPlanFeeLink', 'GetPlanFeeLink', false, entityID);
            // CacheManager.BindDropDownsByEntityID(ControlPanel + ' #ddlCPTCode', 'GetCPTCode', false, entityID);
        }
        else {
            CacheManager.BindDropDownsByEntityID('#POSFeeScheduleDetail #ddlPlanFeeLink', 'GetPlanFeeLink', true, null);
            //  CacheManager.BindDropDownsByEntityID(ControlPanel + ' #ddlCPTCode', 'GetCPTCode', true, null);
        }
        $('#POSFeeScheduleDetail #txtCPTCode').val("");
        $('#POSFeeScheduleDetail #hfCPTCode').val("");
    },

    SavePOSFeeSchedule: function (POSFeeScheduleData) {
        var data = "POSFeeScheduleData=" + POSFeeScheduleData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_POS_FEE_SCHEDULE_DETAIL", "SAVE_POS_FEE_SCHEDULE");
    },

    UpdatePOSFeeSchedule: function (POSFeeScheduleData, POSFeeScheduleID) {
        var data = "POSFeeScheduleData=" + POSFeeScheduleData + "&POSFeeScheduleID=" + POSFeeScheduleID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_POS_FEE_SCHEDULE_DETAIL", "UPDATE_POS_FEE_SCHEDULE");
    },

    FillPOSFeeSchedule: function (POSFeeScheduleID) {
        var data = "POSFeeScheduleID=" + POSFeeScheduleID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_POS_FEE_SCHEDULE_DETAIL", "FILL_POS_FEE_SCHEDULE");
    },

    UpdatePOSFeeScheduleActiveInactive: function (POSFeeScheduleID, IsActive) {
        var data = "POSFeeScheduleID=" + POSFeeScheduleID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_POS_FEE_SCHEDULE_DETAIL", "UPDATE_POS_FEE_SCHEDULE_ACTIVE_INACTIVE");
    },

    // -------------- CPT Code -----------------

    OpenCPTCode: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "POSFeeScheduleDetail";
        params["EntityId"] = $('#POSFeeScheduleDetail #ddlFeeGroup option:selected').attr('refvalue');
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtCPTCode";
        LoadActionPan('Admin_IMOCPT', params);
    },


    // -------------- End CPT Code -------------

    // ------------ Basic Fee Schedule Plan Region (Detail Grid)
    POSFeePlanGridLoad: function (response) {

        if (response.POSPlanCount > 0) {
            var POSPlanJSON = JSON.parse(response.POSPlan_JSON);

            // get Actions
            var actions = "";
            $("#POSFeeScheduleDetail #dgvBFSPlanInfo tr th").each(function () {
                if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                    var arrActionType = [];
                    if ($(this).attr("ActionType") != null) {
                        arrActionType = $(this).attr("ActionType").split(',');
                        actions = EditableGrid.GetActions(arrActionType);
                    }
                }
            });

            $.each(POSPlanJSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($(this))");
                $row.attr("id", item.FeeGroupPOSModifierFeeSchId);

                if (item.IsRequired.toLowerCase() == "true") {
                    item.IsRequired = "Yes"
                }
                else {
                    item.IsRequired = "No"
                }

                $row.append('<td class="actions" id="' + item.FeeGroupPOSModifierFeeSchId + '" >' + actions + '</td><td>' + item.ModifierCode + '</td><td>' + utility.convertToFigure(item.Fee, false) + '</td><td>' + utility.convertToFigure(item.ExpectedFee, false) + '</td><td>' + item.IsRequired + '</td>');

                $("#POSFeeScheduleDetail #dgvBFSPlanInfo tbody").last().append($row);
            });
        }

        //Editable Grid
        var PanelGrid = "#POSFeeScheduleDetail #pnlBFSPlanInfo";
        var GridId = "#POSFeeScheduleDetail #dgvBFSPlanInfo";
        utility.MakeEditableGrid(PanelGrid, GridId, POSFeeScheduleDetail);
    },

    LoadPOSSchedulePlan: function () {
        var data = "POSFeeId=" + POSFeeScheduleDetail.params.POSFeeScheduleId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_POS_FEE_SCHEDULE_DETAIL", "LOAD_POS_PLAN_INFO");
    },

    UpdatePOSSchedulePlan: function (POSPlanData, POSPlanID) {

        var data = "POSPlanData=" + POSPlanData + "&POSPlanID=" + POSPlanID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_POS_FEE_SCHEDULE_DETAIL", "UPDATE_POS_PLAN_INFO");
    },

    SavePOSFeeSchedulePlan: function (POSFeeSchedulePlanData, RowId) {
        var data = "POSFeeSchedulePlanData=" + POSFeeSchedulePlanData + "&POSFeeId=" + POSFeeScheduleDetail.params.POSFeeScheduleId + "&RowId=" + RowId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_POS_FEE_SCHEDULE_DETAIL", "SAVE_POS_PLAN_INFO");
    },

    //FillPOSSchedulePlan: function (POSPlanId) {
    //    var data = "POSPlanId=" + POSPlanId;
    //    // serach parameter , class name, command name of class 
    //    return MDVisionService.defaultService(data, "ADMIN_POS_FEE_SCHEDULE_DETAIL", "FILL_POS_PLAN_INFO");
    //},

    DeletePOSSchedulePlan: function (POSPlanId) {
        var data = "POSPlanId=" + POSPlanId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_POS_FEE_SCHEDULE_DETAIL", "DELETE_POS_PLAN_INFO");
    },

    UnLoadPlan: function () {

        if ($('#frmBFSPlanInfo').serialize() != $('#frmBFSPlanInfo').data('serialize')) {
            utility.myConfirm('2', function () {
                $('#BFSPlanDetailGrid').modal('hide');
            }, function () { },
                    '2'
                );
        }
        else {
            $('#BFSPlanDetailGrid').modal('hide');
        }


    },

    OpenModifierDetail: function (optional, Ctrl, HiddenCtrl) {

        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "POSFeeScheduleDetail";
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


            Admin_Modifier.ValidateModifierCode($("#POSFeeScheduleDetail #txtModifier" + id).val()).done(function (res) {

                if (res.length > 0) {
                    //set value
                    $("#POSFeeScheduleDetail #hfModifier" + id).val(res[1].Value);
                    var myJSON = $row.getMyJSON();

                    if (id && id > 0) {
                        //Edit Record
                        var strMessage = "";
                        AppPrivileges.GetFormPrivileges("Fee Group Plan CPT POS", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                            if (strMessage == "") {

                                POSFeeScheduleDetail.UpdatePOSSchedulePlan(myJSON, id).done(function (response) {
                                    if (response.status != false) {

                                        utility.DisplayMessages(response.Message, 1);
                                        POSFeeScheduleDetail.rowDraw($row, _self, values);
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

                        AppPrivileges.GetFormPrivileges("Fee Group Plan CPT POS", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                            if (strMessage == "") {
                                POSFeeScheduleDetail.SavePOSFeeSchedulePlan(myJSON, id).done(function (response) {
                                    if (response.status != false) {

                                        $row.attr("id", response.POSPlanId);
                                        $row.attr("onclick", "utility.SelectGridRow($(this))");
                                        utility.DisplayMessages(response.Message, 1);
                                        POSFeeScheduleDetail.rowDraw($row, _self, values);
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

        AppPrivileges.GetFormPrivileges("Fee Group Plan CPT POS", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                EditableGrid.rowAdd();
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    rowRemove: function ($row, obj) {

        var id = $row.attr("id");
        AppPrivileges.GetFormPrivileges("Fee Group Plan CPT POS", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = id;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        POSFeeScheduleDetail.DeletePOSSchedulePlan(selectedValue).done(function (response) {
                            if (response.status != false) {

                                if ($row.hasClass('adding')) {
                                }
                                var _self = obj;
                                _self.datatable.row($row.get(0)).remove().draw();


                                //Start 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
                                    utility.removePaginationFromGrid($('#' + POSFeeScheduleDetail.params.PanelID + ' #pnlBFSPlanInfo'));
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

    UnLoad: function () {

        utility.UnLoadDialog("frmPOSFeeScheduleDetail", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });
        
    },

    ShowHistory: function () {
        var PanelID = 'POSFeeScheduleDetail';
        var ParentCtrl = 'POSFeeScheduleDetail';
        var ProfileName = 'Fee Group Plan CPT POS';
        var DBTableName = 'FeeGroupPOSSchedule';
        var ColumnKeyId = POSFeeScheduleDetail.params.POSFeeScheduleId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },

}