revenuecodeDetail = {
    params: [],
    Enable: false,
    Load: function (params) {
        revenuecodeDetail.params = params;
        var self = null;
        if (revenuecodeDetail.params.PanelID == 'revenuecodeDetail')
            self = $('#revenuecodeDetail');
        else
            self = $('#' + revenuecodeDetail.params.PanelID + ' #revenuecodeDetail');

        if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
            self.find("#ddlEntity").attr('disabled', 'disabled');
        }

        self.loadDropDowns(true).done(function () {            
            //if (globalAppdata['IsAdmin'] != "True") {
            //    $("#tblrevenuecodeDetail #divRevenueCode_Entity").css("display", "none");
            //    $("#tblrevenuecodeDetail #ddlEntity").val(globalAppdata["SeletedEntityId"]);
            //}
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
            }
            revenuecodeDetail.LoadRevenueCode();
        });

    },

    LoadRevenueCode: function () {
        $("#revenuecodeDetail #pnlRevenueCodePlanInfo").removeClass('disableAll');
        if (revenuecodeDetail.params.mode == "Add") {
            $('#frmRevenueCodeDetail').data('serialize', $('#frmRevenueCodeDetail').serialize());
            $("#revenuecodeDetail #pnlRevenueCodePlanInfo").addClass('disableAll');
            revenuecodeDetail.ValidationRevenueCode();
        }
        else if (revenuecodeDetail.params.mode == "Edit") {
            $('#revenuecodeDetail #txtRevenueCode').attr("disabled", "disabled");
            revenuecodeDetail.FillRevenueCode(revenuecodeDetail.params.RevenueCodeId).done(function (response) {
                if (response.status != false) {
                    var revenuecode_detail = JSON.parse(response.RevenueCode_JSON);
                    var self = $("#revenuecodeDetail");
                    utility.bindMyJSON(true, revenuecode_detail, false, self);
                    $("#revenuecodeDetail #pnlRevenueCodePlanInfo").removeClass('disableAll');
                    if (revenuecode_detail.ChkIsActive == 'True') {
                        $("#revenuecodeDetail #chkIsActive").attr("checked", true);
                    }
                    else {
                        $("#revenuecodeDetail #chkIsActive").attr("checked", false);
                    }
                    revenuecodeDetail.ValidationRevenueCode();
                    $('#frmRevenueCodeDetail').data('serialize', $('#frmRevenueCodeDetail').serialize());

                    revenuecodeDetail.LoadRevenueCodePlanInfo().done(function (response) {
                        if (response.status != false) {
                            revenuecodeDetail.RevenueCodePlanGridLoad(response);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    RevenueCodeSave: function () {
        $('#frmRevenueCodeDetail').data('serialize', $('#frmRevenueCodeDetail').serialize());
        var strMessage = "";
        var self = $("#revenuecodeDetail");
        var myJSON = self.getMyJSON();
        if (revenuecodeDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Revenue Code", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    if (revenuecodeDetail.params.RevenueCodeId == "-1") {
                        revenuecodeDetail.SaveRevenueCode(myJSON).done(function (response) {
                            if (response.status != false) {
                                $("#revenuecodeDetail #pnlRevenueCodePlanInfo").removeClass('disableAll');
                                revenuecodeDetail.params.RevenueCodeId = response.RevenueCodeId;
                                //Editable Grid
                                var PanelGrid = "#revenuecodeDetail #pnlRevenueCodePlanInfo";
                                var GridId = "#revenuecodeDetail #dgvRevenueCodePlanInfo";
                                utility.MakeEditableGrid(PanelGrid, GridId, revenuecodeDetail);

                                Admin_RevenueCode.RevenueCodeSearch(response.RevenueCodeId);
                                utility.DisplayMessages(response.message, 1);
                                $('#frmRevenueCodeDetail').data('serialize', $('#frmRevenueCodeDetail').serialize());
                                CacheManager.BindCodes('GetRevenueCode', true);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                    else if (revenuecodeDetail.params.RevenueCodeId != "-1" && revenuecodeDetail.params.RevenueCodeId != "" && revenuecodeDetail.params.RevenueCodeId != "0") {
                        revenuecodeDetail.UpdateRevenueCode(myJSON, revenuecodeDetail.params.RevenueCodeId).done(function (response) {
                            if (response.status != false) {
                                Admin_RevenueCode.RevenueCodeSearch(revenuecodeDetail.params.RevenueCodeId);
                                utility.DisplayMessages(response.message, 1);
                                CacheManager.BindCodes('GetRevenueCode', true);
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
        else if (revenuecodeDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Revenue Code", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    revenuecodeDetail.UpdateRevenueCode(myJSON, revenuecodeDetail.params.RevenueCodeId).done(function (response) {
                        if (response.status != false) {
                            Admin_RevenueCode.RevenueCodeSearch(revenuecodeDetail.params.RevenueCodeId);
                            utility.DisplayMessages(response.message, 1);
                            CacheManager.BindCodes('GetRevenueCode', true);
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

    ValidationRevenueCode: function () {
        $('#frmRevenueCodeDetail')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  RevenueCode: {
                      group: '.col-sm-5',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Entity: {
                      group: '.col-sm-5',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Description: {
                      group: '.col-sm-10',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           revenuecodeDetail.RevenueCodeSave();
       });
    },


    // ------------ RevenueCode Plan Region (Detail Grid)
    RevenueCodePlanGridLoad: function (response) {

        if (response.RevenueCodePlanCount > 0) {
            var RevenueCodePlanJSON = JSON.parse(response.RevenueCodePlan_JSON);

            // get Actions
            var actions = "";
            $("#revenuecodeDetail #dgvRevenueCodePlanInfo tr th").each(function () {
                if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                    var arrActionType = [];
                    if ($(this).attr("ActionType") != null) {
                        arrActionType = $(this).attr("ActionType").split(',');
                        actions = EditableGrid.GetActions(arrActionType);
                    }
                }
            });

            $.each(RevenueCodePlanJSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($(this))");
                $row.attr("id",item.RevenuePlanId);

                $row.append('<td class="actions" id="' + item.RevenuePlanId + '" >'+actions+'</td><td>' + item.InsurancePlanName + '</td><td>' + item.Name + '</td>');

                $("#revenuecodeDetail #dgvRevenueCodePlanInfo tbody").last().append($row);
            });
        }

        //Editable Grid
        var PanelGrid = "#revenuecodeDetail #pnlRevenueCodePlanInfo";
        var GridId = "#revenuecodeDetail #dgvRevenueCodePlanInfo";
        utility.MakeEditableGrid(PanelGrid, GridId, revenuecodeDetail);
    },

    RevenueCodePlanDelete: function (RevenueCodePlanId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Revenue Code", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = RevenueCodePlanId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        revenuecodeDetail.DeleteRevenueCodePlanInfo(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#dgvRevenueCodePlanInfo').DataTable();
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

    SaveRevenueCodePlan: function (RevenueCodePlanData, RowId) {
        var data = "RevenueCodePlanData="+RevenueCodePlanData + "&RevenueCodeID=" + revenuecodeDetail.params.RevenueCodeId + "&RowId=" + RowId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_REVENUE_CODE_DETAIL", "SAVE_REVENUE_CODE_INFO");
    },

    UpdateRevenueCodePlan: function (RevenueCodePlanData, RevenueCodePlanID) {
        var data = "RevenueCodePlanData=" + RevenueCodePlanData + "&RevenueCodePlanID=" + RevenueCodePlanID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_REVENUE_CODE_DETAIL", "UPDATE_REVENUE_CODE_INFO");
    },

    //FillRevenueCodePlan: function (RevenueCodePlanID) {
    //    var data = "RevenueCodePlanID=" + RevenueCodePlanID;
    //    // serach parameter , class name, command name of class 
    //    return MDVisionService.defaultService(data, "ADMIN_REVENUE_CODE_DETAIL", "FILL_REVENUE_CODE_INFO");
    //},

    LoadRevenueCodePlanInfo: function () {
        var data = "RevenueCodeID=" + revenuecodeDetail.params.RevenueCodeId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_REVENUE_CODE_DETAIL", "LOAD_REVENUE_CODE_INFO");
    },

    DeleteRevenueCodePlanInfo: function (RevenueCodePlanId) {
        var data = "RevenueCodePlanId=" + RevenueCodePlanId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_REVENUE_CODE_DETAIL", "DELETE_REVENUE_CODE_INFO");
    },

    OpenInsurancePlan:function(optional,Ctrl, HiddenCtrl){
    
        var params = [];
        params["InsurancePlanId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "revenuecodeDetail";
        if (Ctrl != null) {
            params["RefCtrl"] = Ctrl;
        }
        if (HiddenCtrl != null) {
            params["RefHiddenIdCtrl"] = HiddenCtrl;
        }
        LoadActionPan('Admin_InsurancePlan', params);
    },

    //----------------------------------------------------------------

    SaveRevenueCode: function (RevenueCodeData) {
        var data = "RevenueCodeData=" + RevenueCodeData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_REVENUE_CODE_DETAIL", "SAVE_REVENUE_CODE");
    },

    UpdateRevenueCode: function (RevenueCodeData, RevenueCodeID) {
        var data = "RevenueCodeData=" + RevenueCodeData + "&RevenueCodeID=" + RevenueCodeID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_REVENUE_CODE_DETAIL", "UPDATE_REVENUE_CODE");
    },

    FillRevenueCode: function (RevenueCodeID) {
        var data = "RevenueCodeID=" + RevenueCodeID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_REVENUE_CODE_DETAIL", "FILL_REVENUE_CODE");
    },

    UpdateRevenueCodeActiveInactive: function (RevenueCodeID, IsActive) {
        var data = "RevenueCodeID=" + RevenueCodeID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_REVENUE_CODE_DETAIL", "UPDATE_REVENUE_CODE_ACTIVE_INACTIVE");
    },

    UnLoad: function () {

        utility.UnLoadDialog("frmRevenueCodeDetail", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });

    },

    ShowHistory: function () {
        var PanelID = 'revenuecodeDetail';
        var ParentCtrl = 'revenuecodeDetail';
        var ProfileName = 'Revenue Code';
        var DBTableName = 'RevenueCode';
        var ColumnKeyId = revenuecodeDetail.params.RevenueCodeId;

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
                AppPrivileges.GetFormPrivileges("Revenue Code", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {

                        revenuecodeDetail.UpdateRevenueCodePlan(myJSON, id).done(function (response) {
                            if (response.status != false) {

                                utility.DisplayMessages(response.Message, 1);
                                revenuecodeDetail.rowDraw($row,_self, values);
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

                AppPrivileges.GetFormPrivileges("Revenue Code", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        revenuecodeDetail.SaveRevenueCodePlan(myJSON,id).done(function (response) {
                            if (response.status != false) {
                                
                                $row.attr("id", response.RevenueCodePlanId);
                                $row.attr("onclick", "utility.SelectGridRow($(this))");
                                utility.DisplayMessages(response.Message, 1);
                                revenuecodeDetail.rowDraw($row,_self, values);
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

        AppPrivileges.GetFormPrivileges("Revenue Code", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                EditableGrid.rowAdd("-" + $('#' + revenuecodeDetail.params.PanelID + ' #dgvRevenueCodePlanInfo tr').length);
            }
            else
                utility.DisplayMessages(strMessage, 2);
      });
    },

    rowRemove: function ($row, obj) {

        var id = $row.attr("id");
        AppPrivileges.GetFormPrivileges("Revenue Code", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = id;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        revenuecodeDetail.DeleteRevenueCodePlanInfo(selectedValue).done(function (response) {
                            if (response.status != false) {
                               
                                if ($row.hasClass('adding')) {
                                }
                                var _self = obj;
                                _self.datatable.row($row.get(0)).remove().draw();

                                //Start 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
                                    utility.removePaginationFromGrid($('#' + revenuecodeDetail.params.PanelID + ' #pnlRevenueCodePlanInfo'));
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

    rowDraw: function ($row,_self, values) {

        _self.datatable.row($row.get(0)).data(values);
        $actions = $row.find('td.actions');
        if ($actions.get(0)) {
            _self.rowSetActionsDefault($row);
        }
        _self.datatable.draw();
    },

    //-------------------Editable Grid Methods Ends---
}