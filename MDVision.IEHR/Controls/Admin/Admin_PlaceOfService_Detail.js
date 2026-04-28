placeOfServiceDetail = {
    params: [],
    Load: function (params) {
        placeOfServiceDetail.params = params;

        var self = $('#placeOfServiceDetail');
        self.loadDropDowns(true).done(function () {

            placeOfServiceDetail.LoadPlaceOfService();
        });
    },

    LoadPlaceOfService: function () {
        $("#placeOfServiceDetail #pnlPOSPlanInfo").removeClass('disableAll');
        if (placeOfServiceDetail.params.mode == "Add") {

            $("#placeOfServiceDetail #pnlPOSPlanInfo").addClass('disableAll');
            placeOfServiceDetail.ValidatePlaceOfService();
            $('#frmPlaceOfServiceDetail').data('serialize', $('#frmPlaceOfServiceDetail').serialize());
        }
        else if (placeOfServiceDetail.params.mode == "Edit") {

            placeOfServiceDetail.FillPlaceOfService(placeOfServiceDetail.params.PlaceOfServiceId).done(function (response) {
                if (response.status != false) {
                    var placeOfService_detail = JSON.parse(response.PlaceOfServiceFill_JSON);
                    var self = $("#placeOfServiceDetail");
                    utility.bindMyJSON(true, placeOfService_detail, false, self).done(function () {

                        $("#placeOfServiceDetail #pnlPOSPlanInfo").removeClass('disableAll');
                        placeOfServiceDetail.ValidatePlaceOfService();

                        //Serialize data
                        $('#frmPlaceOfServiceDetail').data('serialize', $('#frmPlaceOfServiceDetail').serialize());

                        placeOfServiceDetail.LoadPlaceOfServicePlanInfo().done(function (response) {
                            if (response.status != false) {
                                placeOfServiceDetail.POSPlanGridLoad(response);

                                //Serialize data
                                $('#frmPlaceOfServiceDetail').data('serialize', $('#frmPlaceOfServiceDetail').serialize());
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

    ValidatePlaceOfService: function () {
        $('#frmPlaceOfServiceDetail')
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
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
               }
           }).on('success.form.bv', function (e) {
               e.preventDefault();
               placeOfServiceDetail.PlaceOfServiceSave();
           });
    },

    PlaceOfServiceSave: function () {
        var self = $("#placeOfServiceDetail");
        var myJSON = self.getMyJSON();
        if (placeOfServiceDetail.params.mode == "Add") {
            var strMessage = "";
            AppPrivileges.GetFormPrivileges("Place Of Service", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    if (placeOfServiceDetail.params.PlaceOfServiceId == "-1") {
                        placeOfServiceDetail.SavePlaceOfService(myJSON).done(function (response) {
                            if (response.status != false) {
                                $("#placeOfServiceDetail #pnlPOSPlanInfo").removeClass('disableAll');

                                //Editable Grid
                                var PanelGrid = "#placeOfServiceDetail #pnlPOSPlanInfo";
                                var GridId = "#placeOfServiceDetail #dgvPOSPlanInfo";
                                utility.MakeEditableGrid(PanelGrid, GridId, placeOfServiceDetail);

                                //$("#placeOfServiceDetail #gridform").removeClass('disableAll');
                                Admin_PlaceOfService.PlaceOfServiceSearch(response.PlaceOfServiceId);
                                placeOfServiceDetail.params.PlaceOfServiceId = response.PlaceOfServiceId;
                                utility.DisplayMessages(response.message, 1);
                                $('#frmPlaceOfServiceDetail').data('serialize', $('#frmPlaceOfServiceDetail').serialize());
                                CacheManager.BindCodes('GetPlaceOfService', true);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                    else if (placeOfServiceDetail.params.PlaceOfServiceId != "-1" && placeOfServiceDetail.params.PlaceOfServiceId != "" && placeOfServiceDetail.params.PlaceOfServiceId != "0") {
                        placeOfServiceDetail.UpdatePlaceOfService(myJSON, placeOfServiceDetail.params.PlaceOfServiceId).done(function (response) {
                            if (response.status != false) {
                                Admin_PlaceOfService.PlaceOfServiceSearch(placeOfServiceDetail.params.PlaceOfServiceId);
                                utility.DisplayMessages(response.message, 1);
                                CacheManager.BindCodes('GetPlaceOfService', true);
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
        else if (placeOfServiceDetail.params.mode == "Edit") {
            var strMessage = "";
            AppPrivileges.GetFormPrivileges("Place Of Service", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    placeOfServiceDetail.UpdatePlaceOfService(myJSON, placeOfServiceDetail.params.PlaceOfServiceId).done(function (response) {
                        if (response.status != false) {
                            Admin_PlaceOfService.PlaceOfServiceSearch(placeOfServiceDetail.params.PlaceOfServiceId);
                            utility.DisplayMessages(response.message, 1);
                            CacheManager.BindCodes('GetPlaceOfService', true);
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

    // ------------ POS Plan Region (Detail Grid)
    POSPlanGridLoad: function (response) {

        if (response.POSPlanCount > 0) {
            var POSPlanJSON = JSON.parse(response.POSPlan_JSON);

            // get Actions
            var actions = "";
            $("#placeOfServiceDetail #dgvPOSPlanInfo tr th").each(function () {
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
                $row.attr("id", item.PlanOfPOSId);

                $row.append('<td class="actions" id="' + item.PlanOfPOSId + '" >' + actions + '</td><td>' + item.InsurancePlanName + '</td><td>' + item.Name + '</td>');

                $("#placeOfServiceDetail #dgvPOSPlanInfo tbody").last().append($row);
            });
        }

        //Editable Grid
        var PanelGrid = "#placeOfServiceDetail #pnlPOSPlanInfo";
        var GridId = "#placeOfServiceDetail #dgvPOSPlanInfo";
        utility.MakeEditableGrid(PanelGrid, GridId, placeOfServiceDetail);

    },

    SavePOSPlan: function (POSPlanData, RowId) {
        var data = "POSPlanData=" + POSPlanData + "&PlaceOfServiceID=" + placeOfServiceDetail.params.PlaceOfServiceId + "&RowId=" + RowId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PLACE_OF_SERVICE_DETAIL", "SAVE_POS_PLAN_INFO");
    },

    UpdatePOSPlan: function (POSPlanData, POSPlanID) {
        var data = "POSPlanData=" + POSPlanData + "&POSPlanID=" + POSPlanID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PLACE_OF_SERVICE_DETAIL", "UPDATE_POS_PLAN_INFO");
    },

    //FillPOSPlan: function (POSPlanID) {
    //    var data = "POSPlanID=" + POSPlanID;
    //    // serach parameter , class name, command name of class 
    //    return MDVisionService.defaultService(data, "ADMIN_PLACE_OF_SERVICE_DETAIL", "FILL_POS_PLAN_INFO");
    //},

    LoadPlaceOfServicePlanInfo: function () {
        var data = "PlaceOfServiceID=" + placeOfServiceDetail.params.PlaceOfServiceId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PLACE_OF_SERVICE_DETAIL", "LOAD_POS_PLAN_INFO");
    },

    DeletePlaceOfServicePlanInfo: function (POSPlanId) {
        var data = "POSPlanId=" + POSPlanId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PLACE_OF_SERVICE_DETAIL", "DELETE_POS_PLAN_INFO");
    },

    OpenInsurancePlan: function (optional, Ctrl, HiddenCtrl) {

        var params = [];
        params["InsurancePlanId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "placeOfServiceDetail";
        if (Ctrl != null) {
            params["RefCtrl"] = Ctrl;
        }
        if (HiddenCtrl != null) {
            params["RefHiddenIdCtrl"] = HiddenCtrl;
        }
        LoadActionPan('Admin_InsurancePlan', params);
    },

    //----------------------------------------------------------------

    SavePlaceOfService: function (PlaceOfServiceData) {
        var data = "PlaceOfServiceData=" + PlaceOfServiceData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PLACE_OF_SERVICE_DETAIL", "SAVE_PLACE_OF_SERVICE");
    },

    UpdatePlaceOfService: function (PlaceOfServiceData, PlaceOfServiceID) {
        var data = "PlaceOfServiceData=" + PlaceOfServiceData + "&PlaceOfServiceID=" + PlaceOfServiceID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PLACE_OF_SERVICE_DETAIL", "UPDATE_PLACE_OF_SERVICE");
    },

    FillPlaceOfService: function (PlaceOfServiceID) {
        var data = "PlaceOfServiceID=" + PlaceOfServiceID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PLACE_OF_SERVICE_DETAIL", "FILL_PLACE_OF_SERVICE");
    },

    UpdatePlaceOfServiceActiveInactive: function (PlaceOfServiceID, IsActive) {
        var data = "PlaceOfServiceID=" + PlaceOfServiceID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PLACE_OF_SERVICE_DETAIL", "UPDATE_PLACE_OF_SERVICE_ACTIVE_INACTIVE");
    },

    UnLoad: function () {

        utility.UnLoadDialog("frmPlaceOfServiceDetail", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });

    },

    ShowHistory: function () {
        var PanelID = 'placeOfServiceDetail';
        var ParentCtrl = 'placeOfServiceDetail';
        var ProfileName = 'Place Of Service';
        var DBTableName = 'PlaceOfService';
        var ColumnKeyId = placeOfServiceDetail.params.PlaceOfServiceId;

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

            var PlanId = "#hfPlan" + id;

            if (id && id > 0) {
                //Edit Record
                var strMessage = "";
                AppPrivileges.GetFormPrivileges("Place Of Service", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {

                        placeOfServiceDetail.UpdatePOSPlan(myJSON, id).done(function (response) {
                            if (response.status != false) {

                                utility.DisplayMessages(response.Message, 1);
                                placeOfServiceDetail.rowDraw($row, _self, values);
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

                AppPrivileges.GetFormPrivileges("Place Of Service", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        placeOfServiceDetail.SavePOSPlan(myJSON, id).done(function (response) {
                            if (response.status != false) {

                                $row.attr("id", response.POSPlanId);
                                $row.attr("onclick", "utility.SelectGridRow($(this))");
                                utility.DisplayMessages(response.Message, 1);
                                placeOfServiceDetail.rowDraw($row, _self, values);
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

        AppPrivileges.GetFormPrivileges("Place Of Service", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                EditableGrid.rowAdd("-" + $('#' + placeOfServiceDetail.params.PanelID + ' #dgvPOSPlanInfo tr').length);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },


    rowRemove: function ($row, obj) {

        var id = $row.attr("id");
        AppPrivileges.GetFormPrivileges("Place Of Service", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = id;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        placeOfServiceDetail.DeletePlaceOfServicePlanInfo(selectedValue).done(function (response) {
                            if (response.status != false) {

                                if ($row.hasClass('adding')) {
                                }
                                var _self = obj;
                                _self.datatable.row($row.get(0)).remove().draw();

                                //Start 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
                                utility.removePaginationFromGrid($('#' + placeOfServiceDetail.params.PanelID + ' #pnlPOSPlanInfo'));
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
}