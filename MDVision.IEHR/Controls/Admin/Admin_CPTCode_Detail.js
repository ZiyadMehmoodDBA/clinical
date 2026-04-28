cptcodeDetail = {
    params: [],
    Enable: false,
    Load: function (params) {
        cptcodeDetail.params = params;

        if (cptcodeDetail.params["PanelID"] != "cptcodeDetail") {
            cptcodeDetail.params["PanelID"] = cptcodeDetail.params["PanelID"] + " #cptcodeDetail";
        }

        var self = $('#' + cptcodeDetail.params["PanelID"]);

        self.loadDropDowns(true).done(function () {
            //if (globalAppdata['IsAdmin'] != "True") {
            //    $("#" + cptcodeDetail.params["PanelID"] + " #divCPTCode_Entity").css("display", "none");
            //    $("#" + cptcodeDetail.params["PanelID"] + " #ddlEntity").val(globalAppdata["SeletedEntityId"]);
            //}
            //    //Set Search Parameters if CPT Open from other.
            //    if (cptcodeDetail.params["EntityId"]) {
            //        $('#' + cptcodeDetail.params["PanelID"] + ' #ddlEntity').val(cptcodeDetail.params["EntityId"]);
            //        $('#' + cptcodeDetail.params["PanelID"] + ' #ddlEntity').attr("disabled", "disabled");
            //        cptcodeDetail.params["EntityId"] = null;
            //        //Change Specialty against Entity
            //        cptcodeDetail.LoadEntityBasedData(cptcodeDetail.params["EntityId"]);
            //    }


            cptcodeDetail.LoadCPTCode();
        });

    },
    LoadEntityBasedData: function (entityID) {

        if (entityID != "") {
            CacheManager.BindDropDownsByEntityID('#' + cptcodeDetail.params["PanelID"] + ' #lstSpecialtyId', 'GetSpecialty', false, entityID);
        }
        else {
            CacheManager.BindDropDownsByEntityID('#' + cptcodeDetail.params["PanelID"] + ' #lstSpecialtyId', 'GetSpecialty', true, entityID);

        }
        //$('#' + cptcodeDetail.params["PanelID"] + ' #txtCPTCode').val("");
        //CacheManager.BindAutoCompleteText('#' + cptcodeDetail.params["PanelID"] + ' #txtCPTCode', 'GetCPTCode', true, null, entityID);

    },
    BindCPTAutoComplete: function (element) {

        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', descriptionCrtl, null, true, -1, "CPT", true, "cptcodeDetail", null, false);
    },

    LoadCPTCode: function () {
        PageNo = null;
        rpp = null;
        $("#" + cptcodeDetail.params["PanelID"] + " #pnlCPTPlanInfo").removeClass('disableAll');
        if (cptcodeDetail.params.mode == "Add") {
            //$('#'+ cptcodeDetail.params["PanelID"] +' #txtShortName').attr("enabled", "enabled");

            $("#" + cptcodeDetail.params["PanelID"] + " #pnlCPTPlanInfo").addClass('disableAll');
            $("#" + cptcodeDetail.params["PanelID"] + " #pnlCPTNdcInfo").addClass('disableAll');

            cptcodeDetail.ValidationCPTCode();

            //Serialize data
            $('#frmCPTCodeDetail').data('serialize', $('#frmCPTCodeDetail').serialize());
        }
        else if (cptcodeDetail.params.mode == "Edit") {
            //$('#' + cptcodeDetail.params["PanelID"] + ' #txtShortName').attr("disabled", "disabled");
            cptcodeDetail.FillCPTCode(cptcodeDetail.params.CPTCodeId, PageNo, rpp).done(function (response) {
                if (response.status != false) {
                    var cptcode_detail = JSON.parse(response.CPTCodeFill_JSON);
                    var self = $("#" + cptcodeDetail.params["PanelID"]);
                    utility.bindMyJSON(true, cptcode_detail, false, self).done(function () {

                        //if (cptcode_detail.ddlEntity == null || cptcode_detail.ddlEntity == "")
                        //{
                        //    $('#' + cptcodeDetail.params["PanelID"] + ' #txtCPTCode').attr("disabled", "disabled");
                        //}

                        $("#" + cptcodeDetail.params["PanelID"] + " #pnlCPTPlanInfo").removeClass('disableAll');
                        $("#" + cptcodeDetail.params["PanelID"] + " #pnlCPTNdcInfo").removeClass('disableAll');
                        cptcodeDetail.ValidationCPTCode();

                        //Serialize data
                        $('#frmCPTCodeDetail').data('serialize', $('#frmCPTCodeDetail').serialize());

                        cptcodeDetail.LoadCPTCodePlanInfo().done(function (response) {
                            if (response.status != false) {
                                cptcodeDetail.CPTCodePlanGridLoad(response);
                                cptcodeDetail.NdcCodeGridLoad(response);
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

    CPTCodeSave: function () {
        var strMessage = "";
        var self = $("#" + cptcodeDetail.params["PanelID"]);
        var myJSON = self.getMyJSON();
        if (cptcodeDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("CPT", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    if (cptcodeDetail.params.CPTCodeId == "-1") {
                        cptcodeDetail.SaveCPTCode(myJSON).done(function (response) {
                            if (response.status != false) {
                                $("#" + cptcodeDetail.params["PanelID"] + " #pnlCPTPlanInfo").removeClass('disableAll');
                                cptcodeDetail.params.CPTCodeId = response.CPTCodeId;

                                //Editable Grid
                                var PanelGrid = "#cptcodeDetail #pnlCPTPlanInfo";
                                var GridId = "#cptcodeDetail #dgvCPTPlanInfo";
                                utility.MakeEditableGrid(PanelGrid, GridId, cptcodeDetail);


                                Admin_CPTCode.CPTCodeSearch();
                                utility.DisplayMessages(response.message, 1);
                                $('#frmCPTCodeDetail').data('serialize', $('#frmCPTCodeDetail').serialize());
                                // MDVisionService.reloadLookups = true;
                                //CacheManager.BindCodes('GetCPTCode', true);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                                $("#cptcodeDetail #hfLexiCode").val('');
                            }
                        });
                    }
                    else if (cptcodeDetail.params.CPTCodeId != "-1" && cptcodeDetail.params.CPTCodeId != "" && cptcodeDetail.params.CPTCodeId != "0") {
                        cptcodeDetail.UpdateCPTCode(myJSON, cptcodeDetail.params.CPTCodeId).done(function (response) {
                            if (response.status != false) {
                                Admin_CPTCode.CPTCodeSearch(cptcodeDetail.params.CPTCodeId);
                                utility.DisplayMessages(response.message, 1);
                                // CacheManager.BindCodes('GetCPTCode', true);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                                $("#cptcodeDetail #hfLexiCode").val('');
                            }
                        });
                    }
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else if (cptcodeDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("CPT", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    cptcodeDetail.UpdateCPTCode(myJSON, cptcodeDetail.params.CPTCodeId, 2).done(function (response) {
                        if (response.status != false) {
                            Admin_CPTCode.CPTCodeSearch(cptcodeDetail.params.CPTCodeId);
                            utility.DisplayMessages(response.message, 1);
                            $('#frmCPTCodeDetail').data('serialize', $('#frmCPTCodeDetail').serialize());
                            // CacheManager.BindCodes('GetCPTCode', true);
                            //UnloadActionPan();
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                            $("#cptcodeDetail #hfLexiCode").val('');
                        }
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
    },

    ValidationCPTCode: function () {
        $('#frmCPTCodeDetail')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  ShortName: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          }

                      }
                  },
                  Description: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          }

                      }
                  },
                  BasicUnits: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          }

                      }
                  },
                  Entity: {
                      group: '.size50per',
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
                          },
                          stringLength: {
                              min: 5,
                              message: 'CPT code not Valid'
                          }

                      }
                  },

              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           cptcodeDetail.CPTCodeSave();
       });
    },

    // ------------ CPT Code Plan Region (Detail Grid)
    CPTCodePlanGridLoad: function (response) {

        if (response.CPTPlanCount > 0) {
            var CPTCodePlanJSON = JSON.parse(response.CPTPlan_JSON);

            // get Actions
            var actions = "";
            $("#cptcodeDetail #dgvCPTPlanInfo tr th").each(function () {
                if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                    var arrActionType = [];
                    if ($(this).attr("ActionType") != null) {
                        arrActionType = $(this).attr("ActionType").split(',');
                        actions = EditableGrid.GetActions(arrActionType);
                    }
                }
            });

            $.each(CPTCodePlanJSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($(this))");
                $row.attr("id", item.CPTPlanId);

                $row.append('<td class="actions" id="' + item.CPTPlanId + '" >' + actions + '</td><td>' + item.PlanName + '</td><td>' + item.CPTPlanCode + '</td>');

                $("#cptcodeDetail #dgvCPTPlanInfo tbody").last().append($row);
            });
        }

        //Editable Grid
        var PanelGrid = "#cptcodeDetail #pnlCPTPlanInfo";
        var GridId = "#cptcodeDetail #dgvCPTPlanInfo";
        utility.MakeEditableGrid(PanelGrid, GridId, cptcodeDetail);
    },
    NdcCodeGridLoad: function (response) {

        if (response.CPTNdcCount > 0) {
            var CPTNdc_JSON = response.CPTNdc_JSON;

            // get Actions
            var actions = "";
            $("#cptcodeDetail #dgvCPTNdcInfo tr th").each(function () {
                if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                    var arrActionType = [];
                    if ($(this).attr("ActionType") != null) {
                        arrActionType = $(this).attr("ActionType").split(',');
                        actions = EditableGrid.GetActions(arrActionType);
                    }
                }
            });

            $.each(CPTNdc_JSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($(this))");
                $row.attr("id", item.CPTNdcId);

                $row.append('<td class="actions" id="' + item.CPTNdcId + '" >' + actions + '</td><td>' + item.NDCCode + '</td><td>' + item.NDCDescription + '</td><td>' + item.Unit + '</td><td>' + item.UnitPrice + '</td><td>' + item.NDCMeasurementText + '</td>');

                $("#cptcodeDetail #dgvCPTNdcInfo tbody").last().append($row);
            });
        }

        //Editable Grid
        var PanelGrid = "#cptcodeDetail #pnlCPTNdcInfo";
        var GridId = "#cptcodeDetail #dgvCPTNdcInfo";
        utility.MakeEditableGrid(PanelGrid, GridId, cptcodeDetail);
    },
    SaveCPTCodePlan: function (CPTCodePlanData, RowId) {
        var data = "CPTCodePlanData=" + CPTCodePlanData + "&CPTCodeId=" + cptcodeDetail.params.CPTCodeId + "&RowId=" + RowId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_CPT_CODE_DETAIL", "SAVE_CPT_CODE_PLAN_INFO");
    },
    SaveCPTNDC: function (CPTNDCData, RowId) {
        CPTNDCData = JSON.parse(CPTNDCData);
        var cptNDCObj={}
        cptNDCObj.CPTNdcId = -1;
        cptNDCObj.CPTCodeId = cptcodeDetail.params.CPTCodeId
        cptNDCObj.NDCCode = CPTNDCData["txtNDCCode" + RowId]
        cptNDCObj.NDCDescription = CPTNDCData["txtNDCDescription" + RowId]
        cptNDCObj.Unit = CPTNDCData["txtNDCUnits" + RowId]
        cptNDCObj.UnitPrice = CPTNDCData["txtNDCUnitPrice" + RowId]
        cptNDCObj.NDCMeasurementId = CPTNDCData["txtNDCMeasurementId" + RowId]
        ndcData = JSON.stringify(cptNDCObj);
        var data = "CPTNDCData=" + ndcData;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_CPT_CODE_DETAIL", "SAVE_CPT_CODE_NDC_INFO");
    },
    UpdateCPTNDC: function (CPTNDCData, RowId) {
        CPTNDCData = JSON.parse(CPTNDCData);
        var cptNDCObj = {}
        cptNDCObj.CPTNdcId = RowId;
        cptNDCObj.CPTCodeId = cptcodeDetail.params.CPTCodeId
        cptNDCObj.NDCCode = CPTNDCData["txtNDCCode" + RowId]
        cptNDCObj.NDCDescription = CPTNDCData["txtNDCDescription" + RowId]
        cptNDCObj.Unit = CPTNDCData["txtNDCUnits" + RowId]
        cptNDCObj.UnitPrice = CPTNDCData["txtNDCUnitPrice" + RowId]
        cptNDCObj.NDCMeasurementId = CPTNDCData["txtNDCMeasurementId" + RowId]
        ndcData = JSON.stringify(cptNDCObj);
        var data = "CPTNDCData=" + ndcData;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_CPT_CODE_DETAIL", "UPDATE_CPT_CODE_NDC_INFO");
    },
    UpdateCPTCodePlan: function (CPTCodePlanData, CPTCodePlanID) {
        var data = "CPTCodePlanData=" + CPTCodePlanData + "&CPTCodePlanID=" + CPTCodePlanID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_CPT_CODE_DETAIL", "UPDATE_CPT_CODE_PLAN_INFO");
    },

    FillCPTCodePlan: function (CPTCodePlanID, PageNo, rpp) {

        var data = "CPTCodePlanID=" + CPTCodePlanID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_CPT_CODE_DETAIL", "FILL_CPT_CODE_PLAN_INFO");
    },

    LoadCPTCodePlanInfo: function () {
        var data = "CPTCodeId=" + cptcodeDetail.params.CPTCodeId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_CPT_CODE_DETAIL", "LOAD_CPT_CODE_PLAN_INFO");
    },

    DeleteCPTCodePlanInfo: function (CPTCodePlanId) {
        var data = "CPTCodePlanId=" + CPTCodePlanId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_CPT_CODE_DETAIL", "DELETE_CPT_CODE_PLAN_INFO");
    },
    DeleteCPTCodeNdcnInfo: function (CPTNdcId) {
        var data = "CPTNdcId=" + CPTNdcId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_CPT_CODE_DETAIL", "DELETE_CPT_NDC_INFO");
    },
    OpenInsurancePlan: function (optional, Ctrl, HiddenCtrl) {

        var params = [];
        params["InsurancePlanId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "cptcodeDetail";
        if (Ctrl != null) {
            params["RefCtrl"] = Ctrl;
        }
        if (HiddenCtrl != null) {
            params["RefHiddenIdCtrl"] = HiddenCtrl;
        }
        LoadActionPan('Admin_InsurancePlan', params);
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
                else if ($this.find('select').length) {
                    return $.trim($this.find('select option:selected').text());

                } else {
                    return $.trim($this.find('input').val());
                }
            });

            var id = $row.attr("id");
            var myJSON = $row.getMyJSON();
            if ($row.parent().parent().attr("id") == "dgvCPTNdcInfo") {
                if (id && id > 0) {
                    //Edit Record
                    var strMessage = "";
                    AppPrivileges.GetFormPrivileges("CPT", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            cptcodeDetail.UpdateCPTNDC(myJSON, id).done(function (response) {
                                if (response.status != false) {

                                    utility.DisplayMessages(response.Message, 1);
                                    cptcodeDetail.rowDraw($row, _self, values);
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

                    AppPrivileges.GetFormPrivileges("CPT", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            cptcodeDetail.SaveCPTNDC(myJSON, id).done(function (response) {
                                if (response.status != false) {

                                    $row.attr("id", response.NDCId);
                                    $row.attr("onclick", "utility.SelectGridRow($(this))");
                                    utility.DisplayMessages(response.Message, 1);
                                    cptcodeDetail.rowDraw($row, _self, values);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                    $("#cptcodeDetail #hfLexiCode").val('');
                                }
                            });
                        }
                        else
                            utility.DisplayMessages(strMessage, 2);
                    });
                }
            }
            else {
                if (id && id > 0) {
                    //Edit Record
                    var strMessage = "";
                    AppPrivileges.GetFormPrivileges("CPT", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            cptcodeDetail.UpdateCPTCodePlan(myJSON, id).done(function (response) {
                                if (response.status != false) {

                                    utility.DisplayMessages(response.Message, 1);
                                    cptcodeDetail.rowDraw($row, _self, values);
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

                    AppPrivileges.GetFormPrivileges("CPT", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            cptcodeDetail.SaveCPTCodePlan(myJSON, id).done(function (response) {
                                if (response.status != false) {

                                    $row.attr("id", response.CPTPlanInfoId);
                                    $row.attr("onclick", "utility.SelectGridRow($(this))");
                                    utility.DisplayMessages(response.Message, 1);
                                    cptcodeDetail.rowDraw($row, _self, values);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                    $("#cptcodeDetail #hfLexiCode").val('');
                                }
                            });
                        }
                        else
                            utility.DisplayMessages(strMessage, 2);
                    });
                }
            }

        }
    },
    SetValidation: function (id) {
        id=id.replace("id_","");
        var self = $("#cptcodeDetail #pnlCPTNdcInfo tr#" + id)
        if ($(self).find("#txtNDCCode" + id).val() != "") {
            $(self).find("#txtNDCUnits" + id).attr("isoptional", "0");
            $(self).find("#txtNDCUnitPrice" + id).attr("isoptional", "0");
            $(self).find("#txtNDCMeasurementId" + id).attr("isoptional", "0");
        }
        else {
            $(self).find("#txtNDCUnits" + id).removeAttr("isoptional")
            $(self).find("#txtNDCUnitPrice" + id).removeAttr("isoptional")
            $(self).find("#txtNDCMeasurementId" + id).removeAttr("isoptional")
        }
    },
    rowAdd: function () {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("CPT", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var PanelGrid = "#cptcodeDetail #pnlCPTPlanInfo";
                var GridId = "#cptcodeDetail #dgvCPTPlanInfo";
                if ($.fn.dataTable.isDataTable(GridId)) {
                    $(GridId).dataTable().fnDestroy();
                }
                $.when(utility.MakeEditableGrid(PanelGrid, GridId, cptcodeDetail,"0")).then(function () {
                    utility.callbackAfterAllDOMLoaded(function () {
                        EditableGrid.rowAdd("-" + $('#' + cptcodeDetail.params.PanelID + ' #dgvCPTPlanInfo tr').length);
                    });

                });
                
            } else
                utility.DisplayMessages(strMessage, 2);
        });

    },
    rowAddNdc: function () {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("CPT", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                var PanelGrid = "#cptcodeDetail #pnlCPTNdcInfo";
                var GridId = "#cptcodeDetail #dgvCPTNdcInfo";
                

                if ($.fn.dataTable.isDataTable(GridId)) {
                    $(GridId).dataTable().fnDestroy();
                }
                $.when(utility.MakeEditableGrid(PanelGrid, GridId, cptcodeDetail,"0")).then(function () {
                    utility.callbackAfterAllDOMLoaded(function () {
                        EditableGrid.rowAdd("-" + $('#' + cptcodeDetail.params.PanelID + ' #dgvCPTNdcInfo tr').length);
                    });

                });
            } else
                utility.DisplayMessages(strMessage, 2);
        });

    },
    rowRemove: function ($row, obj) {

        var id = $row.attr("id");
        AppPrivileges.GetFormPrivileges("CPT", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = id;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        if ($row.parent().parent().attr("id") == "dgvCPTNdcInfo") {
                            cptcodeDetail.DeleteCPTCodeNdcnInfo(selectedValue).done(function (response) {
                                if (response.status != false) {

                                    if ($row.hasClass('adding')) {
                                    }
                                    var _self = obj;
                                    _self.datatable.row($row.get(0)).remove().draw();

                                    //Start 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
                                    utility.removePaginationFromGrid($('#' +cptcodeDetail.params.PanelID + ' #pnlCPTNdcInfo'));
                                    //End 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
                                    utility.DisplayMessages(response.Message, 1);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                        else {
                            cptcodeDetail.DeleteCPTCodePlanInfo(selectedValue).done(function (response) {
                                if (response.status != false) {

                                    if ($row.hasClass('adding')) {
                                    }
                                    var _self = obj;
                                    _self.datatable.row($row.get(0)).remove().draw();

                                    //Start 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
                                    utility.removePaginationFromGrid($('#' + cptcodeDetail.params.PanelID + ' #pnlCPTPlanInfo'));
                                    //End 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
                                    utility.DisplayMessages(response.Message, 1);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
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

    SaveCPTCode: function (CPTCodeData) {
        var data = "CPTCodeData=" + CPTCodeData;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_CPT_CODE_DETAIL", "SAVE_CPT_CODE");
    },

    UpdateCPTCode: function (CPTCodeData, CPTCodeID) {
        var data = "CPTCodeData=" + CPTCodeData + "&CPTCodeID=" + CPTCodeID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_CPT_CODE_DETAIL", "UPDATE_CPT_CODE");
    },

    FillCPTCode: function (CPTCodeID, PageNo, rpp) {
        if (PageNo == null) {
            PageNo = 1;
        }
        if (rpp == null) {
            rpp = 15;
        }
        var data = "CPTCodeID=" + CPTCodeID + "&PageNo=" + PageNo + "&rpp=" + rpp;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_CPT_CODE_DETAIL", "FILL_CPT_CODE");
    },

    UpdateCPTCodeActiveInactive: function (CPTCodeID, IsActive, PageNo, rpp) {
        if (PageNo == null) {
            PageNo = 1;
        }
        if (rpp == null) {
            rpp = 15;
        }
        var data = "CPTCodeID=" + CPTCodeID + "&IsActive=" + IsActive + "&PageNo=" + PageNo + "&rpp=" + rpp;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_CPT_CODE_DETAIL", "UPDATE_CPT_CODE_ACTIVE_INACTIVE");
    },

    SetControlValues: function (cptCodePlusDescription, lexiCode, ParentControl, ContainerCrtl, hiddenCtrl, GrandParent, customFormToolsParentId, cptcode) {

        if (cptCodePlusDescription != undefined) {

            var procedureDescription = "";
            var cptCode = "", cptDescription = "", SNOMEDId = "", SNOMEDDescription = "";

            var pFrom = lexiCode.indexOf("~");
            var pTo = lexiCode.indexOf("^");

            var SnomedCodeDescription = lexiCode.substring(pFrom + 1, pTo);
            if (SnomedCodeDescription != "") {
                var splitedSnomed = SnomedCodeDescription.split('+');
                if (splitedSnomed.length > 0) {
                    SNOMEDId = splitedSnomed[0];
                }
                if (splitedSnomed.length > 1) {
                    SNOMEDDescription = splitedSnomed[1];
                }

            }
            if (cptCodePlusDescription.split('-').length >= 2) {
                cptCode = cptCodePlusDescription.split('-')[0].trim();
                //cptDescription = cptCodePlusDescription.split('-')[2].trim();
                cptDescription = cptCodePlusDescription.substring(cptCodePlusDescription.indexOf('-') + 1).trim();
                procedureDescription = cptCodePlusDescription.substring(cptCodePlusDescription.indexOf('-') + 1).trim();
            }
            else if (cptCodePlusDescription.split('-').length == 1) {
                if (ParentControl == 'Clinical_SurgicalHx' || ParentControl == "Favorite_SurgicalHistoryDetail") {
                    cptCode = cptcode;
                }
                else {
                    cptCode = "";
                }
                cptDescription = cptCodePlusDescription.trim();
                procedureDescription = cptCodePlusDescription.trim();
            }
            else {

                // Faizan Ameen
                // Dated : 28-10-2016
                // EMR-1714
                // comment the below if condition and add a new if condition to handle CPT code.

                //  if (!$.isNumeric((cptCodePlusDescription.split('-')[0].trim()).substring(1, cptCodePlusDescription.split('-')[0].trim().length))) {
                if (!$.isNumeric((cptCodePlusDescription.split('-')[0].trim()).substring(0, cptCodePlusDescription.split('-')[0].trim().length - 1))) {

                    cptCode = "";
                    cptDescription = cptCodePlusDescription;
                    procedureDescription = cptCodePlusDescription;
                }
                else {
                    cptCode = cptCodePlusDescription.split('-')[0].trim();
                    cptDescription = cptCodePlusDescription.split('-')[1].trim();
                    procedureDescription = cptCodePlusDescription.substring(cptCodePlusDescription.indexOf('-') + 1).trim()
                }
            }

            if (GrandParent == "Admin_DrugCodeCost_Detail") {
                $("#pnlAdminDrugCodeCost #" + ContainerCrtl).val(cptCode);
            }

            if (ParentControl == "chargeSearchDetail") {
                $("#pnlBillChargeSearch #txtCPT").val(cptCode);

                setTimeout(function () { $("#pnlBillChargeSearch #txtCPT").val(cptCode); }, 105);
            }
            if (ParentControl == "OutOfOfficeVisits") {
                $("#pnlBillOutOfOfficeVisits #hfCPTCode").val(cptCode);
                //Begin Edit by Fahad Malik 13-Dec-2016, Bug# EMR-2189
                $("#pnlBillOutOfOfficeVisits #hfCPTDescription").val(procedureDescription);
                //End Edit by Fahad Malik 13-Dec-2016, Bug# EMR-2189
                setTimeout(function () { $("#pnlBillOutOfOfficeVisits #txtCPTCode").val(cptCode + " - " + procedureDescription); }, 105);
            }
            else if (ParentControl == "cptcodeDetail") {
                $("#cptcodeDetail #txtCPTCode").val(cptCode);
                $("#cptcodeDetail #txtDescription").val(cptDescription);
                $("#cptcodeDetail #hfLexiCode").val(lexiCode.split("*")[0].trim());
                setTimeout(function () {
                    $("#cptcodeDetail #txtCPTCode").val(cptCode); $("#cptcodeDetail #txtDescription").val(cptDescription); $("#cptcodeDetail #hflexiCode").val(lexiCode);

                    if ($('#frmCPTCodeDetail').data('bootstrapValidator') != null && typeof $('#frmCPTCodeDetail').data('bootstrapValidator') != 'undefined') {
                        $('#frmCPTCodeDetail').bootstrapValidator('revalidateField', 'Description');
                    }

                }, 105);

            }
            else if (ParentControl == "SupperBillDetail") {
                setTimeout(function () {
                    $("#SupperBillDetail").find(ContainerCrtl).val(cptDescription);
                }, 105);
            }
                //Start//11/01/2016//Ahmad Raza//logic implimented to fill Procedure field in MedicalHx and filling values in hidden fields
            else if (ParentControl == "Clinical_MedicalHx") {
                setTimeout(function () {
                    $("#pnlClinicalMedicalHx").find(ContainerCrtl).val(cptCode + procedureDescription);
                    $("#pnlClinicalMedicalHx #hfCPTCode").val(cptCode);
                    $("#pnlClinicalMedicalHx #hfCPTDescription").val(procedureDescription);
                    $("#pnlClinicalMedicalHx #hfCPTSNOMEDCode").val(SNOMEDId);
                    $("#pnlClinicalMedicalHx #hfCPTSNOMEDDescription").val(SNOMEDDescription);
                }, 105);
            }
                //End//11/01/2016//Ahmad Raza//logic implimented to fill Procedure field in MedicalHx and filling values in hidden fields
            else if (ParentControl == "Patient_Referrals") {
                setTimeout(function () {
                    $("#pnlPatientReferrals").find(ContainerCrtl).val(cptCode + procedureDescription);
                    $("#pnlPatientReferrals #hfCPTCode").val(cptCode);
                    $("#pnlPatientReferrals #hfCPTDescription").val(procedureDescription);
                }, 105);
            }
                //Start//22/01/2016//Abid Ali//logic implimented to fill Procedure field in HospitalizationHx and filling values in hidden fields
            else if (ParentControl == "Clinical_HospitalizationHx") {
                setTimeout(function () {
                    $("#pnlClinicalHospitalizationHx").find(ContainerCrtl).val(cptCode + " - " + procedureDescription);
                    $("#pnlClinicalHospitalizationHx #hfCPTCode").val(cptCode);
                    $("#pnlClinicalHospitalizationHx #hfCPTDescription").val(procedureDescription);
                    $("#pnlClinicalHospitalizationHx #hfCPTSNOMEDCode").val(SNOMEDId);
                    $("#pnlClinicalHospitalizationHx #hfCPTSNOMEDDescription").val(SNOMEDDescription);
                }, 105);
            }
                //End//22/01/2016//Abid Ali//logic implimented to fill Procedure field in HospitalizationHx and filling values in hidden fields

            else if (ParentControl == "BillingInformation") {
                setTimeout(function (hiddenCtrl, SNOMEDId, SNOMEDDescription) {
                    var Controls = [];
                    Controls = hiddenCtrl.split(',');
                    if (BillingInformation != null && BillingInformation.BillingInfoTime != null) {
                        var CurrentCpt = $.grep(BillingInformation.BillingInfoTime, function (a) {
                            return a.ENMCPT == cptCode
                        });
                        if (CurrentCpt.length > 0) {
                            if (CurrentCpt[0].Type == 'New') {
                                $("#" + BillingInformation.params.PanelID + " #ddlType").val('1');
                            }
                            else if (CurrentCpt[0].Type == 'Established') {
                                $("#" + BillingInformation.params.PanelID + " #ddlType").val('2');
                            }
                            BillingInformation.ddlType_Change($("#" + BillingInformation.params.PanelID + " #ddlType"), CurrentCpt[0].Description);
                        }
                    }
                    if (Controls.length > 0) {
                        for (var index in Controls) {
                            if (Controls[index].toString().indexOf('#') == -1) {
                                Controls[index] = "#" + Controls[index];
                            }
                            if (Controls[index].toString().indexOf("hfCPTCode") > -1) {
                                $("#pnlBillingInformation " + Controls[index]).val(cptCode);
                                $("#pnlBillingInformation " + Controls[index]).parent().parent().find('input[type=text]').val(cptCode + ' - ' + procedureDescription);
                            }

                            if (Controls[index].toString().indexOf("hfCPTDescription") > -1)
                                $("#pnlBillingInformation " + Controls[index]).val(procedureDescription);

                            if (Controls[index].toString().indexOf("hfBillingInfoCPTId") > -1)
                                $("#pnlBillingInformation " + Controls[index]).val("");

                            if (Controls[index].toString().indexOf("hfSNOMEDId") > -1)
                                $("#pnlBillingInformation " + Controls[index]).val(SNOMEDId);

                            if (Controls[index].toString().indexOf("hfSNOMEDDescription") > -1)
                                $("#pnlBillingInformation " + Controls[index]).val(SNOMEDDescription);

                            if (Controls[index].toString().indexOf("hfCPTSNOMEDCodeId") > -1)
                                $("#pnlBillingInformation " + Controls[index]).val(SNOMEDId);

                            if (Controls[index].toString().indexOf("hfCPTSNOMEDDescription") > -1)
                                $("#pnlBillingInformation " + Controls[index]).val(SNOMEDDescription);

                            if (cptCode == "") {
                                $("#pnlBillingInformation " + Controls[index]).parent().parent().find('input[type=text]').val('');
                                $("#pnlBillingInformation " + Controls[index]).val('');
                            }

                        }
                        $("#pnlBillingInformation " + Controls[0]).parent().parent().find('input[type=text]').trigger('blur');
                    }
                }, 105, hiddenCtrl, SNOMEDId, SNOMEDDescription)
            }

                //Start//22/01/2016//Syed zia//logic implimented to fill Procedure field in SurgicalHx and filling values in hidden fields
            else if (ParentControl == "Clinical_SurgicalHx") {
                setTimeout(function () {
                    $("#pnlClinicalSurgicalHx").find(ContainerCrtl).val(cptCode + procedureDescription);
                    $("#pnlClinicalSurgicalHx #hfCPTCode").val(cptCode);
                    $("#pnlClinicalSurgicalHx #hfCPTDescription").val(procedureDescription);
                    $("#pnlClinicalSurgicalHx #hfCPTSNOMEDCode").val(SNOMEDId);
                    $("#pnlClinicalSurgicalHx #hfCPTSNOMEDDescription").val(SNOMEDDescription);
                    /////////////////////
                    var currId = -1;
                    $("#pnlClinicalSurgicalHx #frmClinicalSurgicalHx #Surgical ul#ulSurgicalDisease li[id*='-']").each(function (i, item) {

                        currId = $(this).attr("id");

                    });

                    currId = parseInt(currId) + (-1);
                    /*Start 28/01/2016 Abid Ali, bug reslove : changed function call on delete*/
                    var li = "<li  id=" + currId + " onclick='Clinical_SurgicalHx.fillSurgicalHxDisease(this, event);' onmouseover='Clinical_SurgicalHx.showIcon(this);' onmouseout='Clinical_SurgicalHx.hideIcon(this);' cptCode=\"" + cptCode + "\" cptDesc=\"" + cptDescription + "\" snomedCode=\"" + SNOMEDId + "\" snomedDesc=\"" + SNOMEDDescription + "\"><a href='#'>" + cptDescription + "<span class='removeIconListHover' onclick='Clinical_SurgicalHx.deleteSurgicalHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"
                    /*End  28/01/2016 Abid Ali, bug reslove : changed function call on delete*/
                    var IsAlreadyExist = false;
                    $('#pnlClinicalSurgicalHx #ulSurgicalDisease li').each(function () {
                        if ($(this).attr('cptDesc') == null) {

                            if ($(this).text() == cptDescription) {

                                IsAlreadyExist = true;
                            }
                        } else {
                            if ($(this).attr('cptDesc') == cptDescription &&
                                $(this).attr('snomedCode') == SNOMEDId && $(this).attr('snomedDesc') == SNOMEDDescription) {
                                IsAlreadyExist = true;
                            }
                        }
                    });

                    if (!IsAlreadyExist) {
                        $('#pnlClinicalSurgicalHx #ulSurgicalDisease').append(li);
                        $(li).trigger('click');
                        $('#pnlClinicalSurgicalHx #txtDisease').val('');

                        if (Clinical_SurgicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                            var diseaseId = $('#' + Clinical_SurgicalHx.params.PanelID + " #ulSurgicalDisease > li.active").attr('id');
                            var disease = $(li).get(0).outerHTML;
                            var diseaseDetail = $('#' + Clinical_SurgicalHx.params.PanelID + " #sectionSurgicalDetails").clone();
                            $(diseaseDetail).resetAllControls(null);
                            var diseaseData = $(diseaseDetail).getMyJSONByName();
                            Clinical_SurgicalHx.cacheSurgicalHxJSON(diseaseId, diseaseData, disease);
                        }

                        //var isUnload = "false";
                        //var txt = $('#pnlClinicalSurgicalHx #txtDisease');
                        //if (txt.is('[data-popupunload]')) {
                        //    isUnload = txt.attr('data-popupunload');
                        //}

                        //if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                        //    txt.attr("data-popupunload", "false");
                        //    Admin_IMOICD.UnLoadTab();
                        //}
                    }
                    else {
                        utility.DisplayMessages('Procedure already added', 2);
                        $('#pnlClinicalSurgicalHx #txtDisease').val('');
                    }
                }, 105);

                /////////////////////////
            }
                //End//22/01/2016//Syed Zia//logic implimented to fill Procedure field in SurgicalHx and filling values in hidden fields

                //Start//10/05/2016//Ahmad Raza//logic implimented to fill Goal Ul in Plan of Care
            else if (ParentControl == "Clinical_PlanOfCare") {
                setTimeout(function () {
                    if (cptDescription != "" || procedureDescription != "" && cptCode != "") {


                        var currId = -1;
                        $("#pnlPlanOfCare #frmPlanOfCare ul#ulPlanOfCareGoal li[id*='-']").each(function (i, item) {

                            currId = $(this).attr("id");

                        });
                        currId = parseInt(currId) + (-1);
                        var cptCodePlusDash = cptCode != "" ? ' - ' + cptCode : cptCode;
                        var li = "<li  id=" + currId + " onclick = 'Clinical_PlanOfCare.fillPlanOfCareGoal(this, event);' onmouseover='Clinical_PlanOfCare.showIcon(this);' onmouseout='Clinical_PlanOfCare.hideIcon(this);'        snomedCode=\"" + SNOMEDId + "\" snomedDescription=\"" + SNOMEDDescription + "\"  cptCode=\"" + cptCode + "\" cptDescription=\"" + cptDescription + "\" procedureDescription=\"" + procedureDescription + "\"><a onclick='Clinical_PlanOfCare.activeInActive($(this), event);'>" + cptDescription + cptCodePlusDash + "<div id='deleteIcon' style='display:none' class='pull-right' onclick='Clinical_PlanOfCare.deletePlanOfCareGoal($($(this).parent()).parent(), event);'><i class='fa fa-close red'></i></div></a></li>";
                        var IsAlreadyExist = false;
                        $('#pnlPlanOfCare #ulPlanOfCareGoal li').each(function () {
                            if ($(this).attr('cptCode') == cptCode && $(this).attr('cptCode') == cptCode &&
                                $(this).attr('cptDescription') == cptDescription && $(this).attr('cptDescription') == cptDescription &&
                                $(this).attr('procedureDescription') == procedureDescription && $(this).attr('procedureDescription') == procedureDescription) {

                                IsAlreadyExist = true;
                            }
                        });

                        if (!IsAlreadyExist) {
                            $('#pnlPlanOfCare #ulPlanOfCareGoal').append(li);
                            $(li).trigger('click');
                            $('.modal-backdrop').removeClass('in');
                            $('.modal-backdrop').addClass('out');
                            $('.modal-backdrop').hide();
                            $('#pnlPlanOfCare #txtGoal').val('');
                            // Clinical_Procedures.AddInArray(currId, cptCode, cptDescription, true);//for record of procedure
                            //BindProcedureGridItem(currId, cptCode, cptDescription);//for make new row on grid for procedure
                            //if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab)
                            //    Admin_IMOICD.UnLoadTab();
                        }
                        else {
                            utility.DisplayMessages('Goal already added', 2);

                            $('#pnlPlanOfCare #txtGoal').val('');
                        }
                    }
                }, 105);

            }
            else if (ParentControl == "Immunization_AddVaccine") {
                if (cptDescription != "" != "" && cptCode != "") {
                    $(hiddenCtrl).val(cptCode != "" ? (cptCode + " - " + cptDescription) : cptDescription);
                    $(hiddenCtrl).attr("Code", cptCode);
                    $(hiddenCtrl).attr("Description", cptDescription);
                }
            }
            else if (ParentControl == "Immunization_TherapeuticDetail") {
                if (cptDescription != "" != "" && cptCode != "") {
                    $(hiddenCtrl).val(cptCode != "" ? (cptCode + " - " + cptDescription) : cptDescription);
                    $(hiddenCtrl).attr("Code", cptCode);
                    $(hiddenCtrl).attr("Description", cptDescription);
                }
            }
            else if (ParentControl == "CCMEnrolledGoals") {
                setTimeout(function () {
                    if (cptDescription != "" || procedureDescription != "" && cptCode != "") {
                        var currId = -1;
                        $("#pnlCCMEnrolledGoals #frmCCMEnrolledGoals ul#ulCCMEnrolledGoalsGoal li[id*='-']").each(function (i, item) {

                            currId = $(this).attr("id");

                        });
                        currId = parseInt(currId) + (-1);
                        var cptCodePlusDash = cptCode != "" ? ' - ' + cptCode : cptCode;
                        var li = "<li  id=" + currId + " onclick = 'CCMEnrolledGoals.fillPlanOfCareGoal(this, event);' onmouseover='CCMEnrolledGoals.showIcon(this);' onmouseout='CCMEnrolledGoals.hideIcon(this);'        snomedCode=\"" + SNOMEDId + "\" snomedDescription=\"" + SNOMEDDescription + "\"  cptCode=\"" + cptCode + "\" cptDescription=\"" + cptDescription + "\" procedureDescription=\"" + procedureDescription + "\"><a onclick='CCMEnrolledGoals.activeInActive($(this), event);'>" + cptDescription + cptCodePlusDash + "<div id='deleteIcon' style='display:none' class='pull-right' onclick='CCMEnrolledGoals.deletePlanOfCareGoal($($(this).parent()).parent(), event);'><i class='fa fa-close red'></i></div></a></li>";
                        var IsAlreadyExist = false;
                        $('#pnlCCMEnrolledGoals #ulCCMEnrolledGoalsGoal li').each(function () {
                            if ($(this).attr('cptCode') == cptCode && $(this).attr('cptCode') == cptCode &&
                                $(this).attr('cptDescription') == cptDescription && $(this).attr('cptDescription') == cptDescription &&
                                $(this).attr('procedureDescription') == procedureDescription && $(this).attr('procedureDescription') == procedureDescription) {

                                IsAlreadyExist = true;
                            }
                        });

                        if (!IsAlreadyExist) {
                            $('#pnlCCMEnrolledGoals #ulCCMEnrolledGoalsGoal').append(li);
                            $(li).trigger('click');
                            $('.modal-backdrop').removeClass('in');
                            $('.modal-backdrop').addClass('out');
                            $('.modal-backdrop').hide();
                            $('#pnlCCMEnrolledGoals #txtGoal').val('');
                        }
                        else {
                            utility.DisplayMessages('Goal already added', 2);

                            $('#pnlCCMEnrolledGoals #txtGoal').val('');
                        }
                    }
                }, 105);

            }
                //End//10/05/2016//Ahmad Raza//logic implimented to fill Goal Ul in Plan of Care

            else if (ParentControl == "Clinical_LabOrder") {
                setTimeout(function () {
                    $("#pnlClinicalLabOrder").find(ContainerCrtl).val(cptCode + procedureDescription);
                    $("#pnlClinicalLabOrder #hfCPTCode").val(cptCode);
                    $("#pnlClinicalLabOrder #hfCPTDescription").val(procedureDescription);
                }, 105);
            }
            else if (ParentControl == "Clinical_RadiologyOrder") {
                setTimeout(function () {

                    if (cptCode != null && cptCode != "") {
                        $("#pnlClinicalRadiologyOrder").find(ContainerCrtl).val(cptCode + " - " + procedureDescription);
                    }
                    else {
                        $("#pnlClinicalRadiologyOrder").find(ContainerCrtl).val(procedureDescription);
                    }

                    $("#pnlClinicalRadiologyOrder #hfCPTCode").val(cptCode);
                    $("#pnlClinicalRadiologyOrder #hfCPTDescription").val(procedureDescription);

                }, 105);
            }
            else if (ParentControl == "Clinical_LOINC") {
                setTimeout(function () {

                    if (cptCode != null && cptCode != "") {
                        $("#pnlClinicalLOINC").find(ContainerCrtl).val(cptCode + " - " + procedureDescription);
                    }
                    else {
                        $("#pnlClinicalLOINC").find(ContainerCrtl).val(procedureDescription);
                    }

                    $("#pnlClinicalLOINC #hfCPTCode").val(cptCode);
                    $("#pnlClinicalLOINC #hfCPTDescription").val(procedureDescription);
                    //$("#pnlClinicalRadiologyResultDetail #hfCPTSNOMEDCode").val(SNOMEDId);
                    //$("#pnlClinicalRadiologyResultDetail #hfCPTSNOMEDDescription").val(SNOMEDDescription);

                    Clinical_LOINC.BindResultGridItem(cptCode, procedureDescription, procedureDescription, SNOMEDId, SNOMEDDescription);


                }, 105);
            }
                //-------------------- start 17/03/2016 irfan code for orders in clinical

            else if (ParentControl == "ClinicalRadiologyOrderDetail") {
                setTimeout(function () {
                    //$("#pnlClinicalRadiologyOrderDetail #hfCPTSNOMEDCode").val(SNOMEDId);
                    //$("#pnlClinicalRadiologyOrderDetail #hfCPTSNOMEDDescription").val(SNOMEDDescription);
                    ClinicalRadiologyOrderDetail.BindProcedureGridItem(cptCode, procedureDescription, cptDescription, SNOMEDId, SNOMEDDescription);

                }, 105);
            }
            else if (ParentControl == "OrderSet_RadiologyOrderDetails") {
                setTimeout(function () {
                    //$("#OrderSet_RadiologyOrderDetails #hfCPTSNOMEDCode").val(SNOMEDId);
                    //$("#OrderSet_RadiologyOrderDetails #hfCPTSNOMEDDescription").val(SNOMEDDescription);
                    OrderSet_RadiologyOrderDetails.BindProcedureGridItem(cptCode, procedureDescription, cptDescription, SNOMEDId, SNOMEDDescription);

                }, 105);
            }

            else if (ParentControl == "ClinicalProcedureOrderDetail") {
                setTimeout(function () {
                    //$("#pnlClinicalProcedureOrder #hfCPTSNOMEDCode").val(SNOMEDId);
                    //$("#pnlClinicalProcedureOrder #hfCPTSNOMEDDescription").val(SNOMEDDescription);
                    ClinicalProcedureOrderDetail.BindProcedureGridItem(cptCode, procedureDescription, cptDescription, null, SNOMEDId, SNOMEDDescription);

                }, 105);
            }
            else if (ParentControl == "OrderSet_ProcedureOrderDetails") {
                setTimeout(function () {
                    //$("#pnlClinicalProcedureOrder #hfCPTSNOMEDCode").val(SNOMEDId);
                    //$("#pnlClinicalProcedureOrder #hfCPTSNOMEDDescription").val(SNOMEDDescription);
                    OrderSet_ProcedureOrderDetails.BindProcedureGridItem(cptCode, procedureDescription, cptDescription, null, SNOMEDId, SNOMEDDescription);

                }, 105);
            }
            else if (ParentControl == "ClinicalConsultationOrderDetail") {
                setTimeout(function () {
                    //$("#pnlClinicalConsultationOrderDetail #hfCPTSNOMEDCode").val(SNOMEDId);
                    //$("#pnlClinicalConsultationOrderDetail #hfCPTSNOMEDDescription").val(SNOMEDDescription);
                    ClinicalConsultationOrderDetail.BindProcedureGridItem(cptCode, procedureDescription, cptDescription, SNOMEDId, SNOMEDDescription);

                }, 105);
            }
                //Start//22/01/2016//Abid Ali//Bind Grid to
            else if (ParentControl == "ClinicalLabResultDetail") {
                setTimeout(function () {

                    ClinicalLabResultDetail.BindLabOrderResultGridItem(cptCode, procedureDescription, cptDescription);

                }, 105);
            }
            else if (ParentControl == "ClinicalRadiologyResultDetail") {
                setTimeout(function () {

                    ClinicalRadiologyResultDetail.BindRadiologyOrderResultGridItem(cptCode, procedureDescription, cptDescription);

                }, 105);
            }

                //---------------------- end 17/03/2016 irfan code for orders in clinical

                //Start//22/01/2016//Abid Ali//Bind Grid to
            else if (ParentControl == "ClinicalLabOrderDetail") {
                setTimeout(function () {

                    ClinicalLabOrderDetail.BindLabOrderGridItem(cptCode, procedureDescription, cptDescription);

                }, 105);
            }
            else if (ParentControl == "OrderSet_LabOrderDetails") {
                setTimeout(function () {

                    OrderSet_LabOrderDetails.BindLabOrderGridItem(cptCode, procedureDescription, cptDescription);

                }, 105);
            }

                //---------------------- end 17/03/2016 irfan code for orders in clinical
            else if (ParentControl == "Clinical_ImplantableDetail") {
                setTimeout(function () {
                    Clinical_ImplantableDetail.BindProcedureGridItem(cptCode, procedureDescription, cptDescription, SNOMEDId, SNOMEDDescription);//for make new row on grid for implantable device procedure
                }, 105);
            }

            else if (ParentControl == "providerDetail") {
                var currId = -1;
                $("#providerDetail ul#ulProceduresList li[id*='-']").each(function (i, item) {
                    currId = $(this).attr("id");
                });

                currId = parseInt(currId) + (-1);

                var li = "<li procedureListId = " + currId + "  id=" + currId + " cptCode=\"" + cptCode + "\" cptDesc=\"" + cptDescription + "\" snomedCode=\"" + SNOMEDId + "\" snomedDesc=\"" + SNOMEDDescription + "\"><a href='#'>" + cptCode + " - " + cptDescription + "<span class='removeIconListHover' onclick='providerDetail.deleteCPTFromCPTData(this,\"" + cptCode + "\",\"" + cptDescription + "\",\"" + SNOMEDId + "\",\"" + SNOMEDDescription + "\");'><i class='fa fa-close'></i></span></a></li>";

                var IsAlreadyExist = false;
                $('#providerDetail #ulProceduresList li').each(function () {
                    if ($(this).attr('cptCode') == cptCode && $(this).attr('cptdesc') == cptDescription) {
                        IsAlreadyExist = true;
                    }
                });

                if (!IsAlreadyExist) {
                    $('#providerDetail #ulProceduresList').addClass('panel-body p-none height-max150 height150 overflowY own-scroll');
                    $('#providerDetail #ulProceduresList').append(li);
                    $(li).trigger('click');

                    var item = {};
                    item["CPTCode"] = cptCode;
                    item["CPTCodeDescription"] = cptDescription;
                    item["SNOMED_Description"] = SNOMEDDescription;
                    item["SNOMEDID"] = SNOMEDId;
                    providerDetail.CPTData.push(item);

                    $('#providerDetail #txtProcedures').val('');
                }
                else {
                    utility.DisplayMessages('Procedure already added', 2);

                    $('#providerDetail #txtProcedures').val('');
                }
            }

            else if (ParentControl == "Patient_Referrals_Incoming_Detail") {
                setTimeout(function () {

                    Patient_Referrals_Incoming_Detail.BindProcedureGridItem(cptCode, procedureDescription, cptDescription, SNOMEDId, SNOMEDDescription);//for make new row on grid for Incoming Referral procedure
                }, 105);
            }
            else if (ParentControl == "Patient_Referrals_Outgoing_Detail") {
                setTimeout(function () {

                    Patient_Referrals_Outgoing_Detail.BindProcedureGridItem(cptCode, procedureDescription, cptDescription, SNOMEDId, SNOMEDDescription);//for make new row on grid for Outgoing Referral procedure
                }, 105);
            }
            else if (ParentControl == "OrderSet_Patient_Referrals_Outgoing_Detail") {
                setTimeout(function () {

                    OrderSet_Patient_Referrals_Outgoing_Detail.BindProcedureGridItem(cptCode, procedureDescription, cptDescription, SNOMEDId, SNOMEDDescription);//for make new row on grid for Outgoing Referral procedure
                }, 105);
            }
                //Start || 17 March, 2016 || ZeeshanAK || CPT search for Procedures
            else if (ParentControl == "Clinical_Procedures") {

                setTimeout(function () {

                    Clinical_Procedures.BindProcedureGridItem(cptCode, procedureDescription, cptDescription, SNOMEDId, SNOMEDDescription);//for make new row on grid for procedure

                }, 105);
                //if (snomedCode == "" && snomedDescription == "" && icd10Code == "" && icd10Description == "" && icd9Code == "" && icd9Description == "") {

                //}
                //else {
                //var currId = -1;
                //$("#pnlClinicalProcedures #frmClinicalProcedures ul#ulProcedures li[id*='-']").each(function (i, item) {

                //    currId = $(this).attr("id");

                //});
                //currId = parseInt(currId) + (-1);
                //var li = "<li  id=" + currId + " onclick='Clinical_Procedures.fillProceduresBox(this, event," + cptCode + "','" + cptDescription + "');'  cptCode=\"" + cptCode + "\" cptDescription=\"" + cptDescription + "\" procedureDescription=\"" + procedureDescription + "\"><a href='#'>" + cptCode + '  ' + cptDescription + "<span class='removeIconListHover' onclick='Clinical_Procedures.deleteProcedureFromBox(" + currId + ",event);'><i class='fa fa-times'></i></span></a></li>"
                //var IsAlreadyExist = false;
                //$('#pnlClinicalProcedures #ulProcedures li').each(function () {
                //    if ($(this).attr('cptCode') == cptCode && $(this).attr('cptCode') == cptCode &&
                //        $(this).attr('cptDescription') == cptDescription && $(this).attr('cptDescription') == cptDescription &&
                //        $(this).attr('procedureDescription') == procedureDescription && $(this).attr('procedureDescription') == procedureDescription) {

                //        IsAlreadyExist = true;
                //    }
                //});

                //if (!IsAlreadyExist) {
                //    $('#pnlClinicalProcedures #ulProcedures').append(li);
                //    $('.modal-backdrop').removeClass('in');
                //    $('.modal-backdrop').addClass('out');
                //    $('.modal-backdrop').hide();
                //    $('#pnlClinicalProcedures #txtProcedureCode').val('');
                //    Clinical_Procedures.AddInArray(currId, cptCode, cptDescription, true);//for record of procedure
                //    //BindProcedureGridItem(currId, cptCode, cptDescription);//for make new row on grid for procedure
                //    //if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab)
                //    //    Admin_IMOICD.UnLoadTab();
                //}
                //else {
                //    utility.DisplayMessages('Procedure already added', 2);

                //    $('#pnlClinicalProcedures #txtProcedureCode').val('');
                //}
                // }
            }
            else if (ParentControl == "Immunization_AddVaccine") {
                if (cptDescription != "" != "" && cptCode != "") {
                    $(hiddenCtrl).val(cptCode != "" ? (cptCode + " - " + cptDescription) : cptDescription);
                    $(hiddenCtrl).attr("Code", cptCode);
                    $(hiddenCtrl).attr("Description", cptDescription);
                }
            }
            else if (ParentControl == "Immunization_TherapeuticDetail") {
                if (cptDescription != "" != "" && cptCode != "") {
                    $(hiddenCtrl).val(cptCode != "" ? (cptCode + " - " + cptDescription) : cptDescription);
                    $(hiddenCtrl).attr("Code", cptCode);
                    $(hiddenCtrl).attr("Description", cptDescription);
                }
            }
                //End   || 17 March, 2016 || ZeeshanAK || CPT search for Procedures
            else if (ParentControl == "OrderSet_Procedures") {
                setTimeout(function () {
                    OrderSet_Procedures.BindProcedureGridItem(cptCode, procedureDescription, cptDescription, SNOMEDId, SNOMEDDescription);//for make new row on grid for procedure
                }, 105);
            }
            else if (ParentControl == "Favorite_ConsultationOrderDetail") {

                Favorite_ConsultationOrderDetail.bindToCPTList(cptCode, procedureDescription);
            }

            else if (ParentControl == "Favorite_ProcedureOrderDetail") {

                Favorite_ProcedureOrderDetail.bindToCPTList(cptCode, procedureDescription);
            }
            else if (ParentControl == "Favorite_ProcedureDetail") {

                Favorite_ProcedureDetail.bindToCPTList(cptCode, cptDescription, SNOMEDId, SNOMEDDescription);
            }
            else if (ParentControl == "Favorite_RadiologyOrderDetail") {

                Favorite_RadiologyOrderDetail.bindToCPTList(cptCode, procedureDescription);
            }
                //Start 31-03-2016 Humaira Yousaf for favorite history
            else if (ParentControl == "Favorite_FamilyHistoryDetail") {
                Favorite_FamilyHistoryDetail.bindToCPTList(cptCode, cptDescription);
            }
            else if (ParentControl == "Favorite_MedicalHistoryDetail") {
                Favorite_MedicalHistoryDetail.bindToCPTList(cptCode, cptDescription);
            }
            else if (ParentControl == "Favorite_SurgicalHistoryDetail") {
                //  Favorite_SurgicalHistoryDetail.bindToCPTList(cptCode, cptDescription);


                setTimeout(function () {
                    var currId = -1;
                    $("#pnlFavoriteSurgicalHistoryDetail #frmFavoriteSurgicalHistoryDetail ul#ulFavSurgicalHistoryDisease li[id*='-']").each(function (i, item) {

                        currId = $(this).attr("id");

                    });

                    currId = parseInt(currId) + (-1);

                    var li = "<li favouriteListICDId = " + currId + " FavouriteListId=" + currId + "  id=" + currId + " onmouseover='Favorite_SurgicalHistoryDetail.showIcon(this);' onmouseout='Favorite_SurgicalHistoryDetail.hideIcon(this);' cptCode=\"" + cptCode + "\" cptDesc=\"" + cptDescription + "\" snomedCode=\"" + SNOMEDId + "\" snomedDesc=\"" + SNOMEDDescription + "\"><a href='#'>" + cptDescription + "<span class='removeIconListHover' onclick='Favorite_SurgicalHistoryDetail.deleteCPTFromCPTData(this,\"" + cptCode + "\",\"" + cptDescription + "\",\"" + SNOMEDId + "\",\"" + SNOMEDDescription + "\");'><i class='fa fa-close'></i></span></a></li>";

                    var IsAlreadyExist = false;
                    $('#pnlFavoriteSurgicalHistoryDetail #ulFavSurgicalHistoryDisease li').each(function () {
                        if ($(this).attr('cptCode') == cptCode && $(this).attr('cptdesc') == cptDescription) {
                            IsAlreadyExist = true;
                        }
                    });

                    if (!IsAlreadyExist) {
                        $('#pnlFavoriteSurgicalHistoryDetail #ulFavSurgicalHistoryDisease').append(li);
                        $(li).trigger('click');
                        var item = {};
                        item["CPTCode"] = cptCode;
                        item["CPTCodeDescription"] = cptDescription;
                        item["SNOMED_Description"] = SNOMEDDescription;
                        item["SNOMEDID"] = SNOMEDId;
                        Favorite_SurgicalHistoryDetail.CPTData.push(item);

                        $('#pnlFavoriteSurgicalHistoryDetail #txtDisease').val('');

                        //var isUnload = "false";
                        //var txt = $('#pnlFavoriteSurgicalHistoryDetail #txtDiagnosis');
                        //if (txt.is('[data-popupunload]')) {
                        //    isUnload = txt.attr('data-popupunload');
                        //}

                        //if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                        //    txt.attr("data-popupunload", "false");
                        //    Admin_IMOICD.UnLoadTab();
                        //}
                    }
                    else {
                        utility.DisplayMessages('Procedure already added', 2);

                        $('#pnlFavoriteSurgicalHistoryDetail #txtDiagnose').val('');
                    }
                }, 105);
            }
                //End 31-03-2016 Humaira Yousaf for favorite history
            else if (ParentControl == "Favorite_LabOrderDetail") {

                // Favorite_LabOrderDetail.bindToCPTList(cptCode, cptDescription);
            }






            else if (ParentControl == "ReportsSSRSDashboard") {
                setTimeout(function () {
                    if (ContainerCrtl.indexOf("pnlReportsSSRSDashboard") >= 0)
                        $(ContainerCrtl).val(cptCode);
                    else
                        $("#pnlReportsSSRSDashboard").find(ContainerCrtl).val(cptCode);
                }, 105);
            }
            else if (ParentControl.indexOf("EncounterChargeCapture") >= 0) {
                setTimeout(function () {
                    //  if (ContainerCrtl.indexOf("pnlReportsSSRSDashboard") >= 0) {
                    //    $(ContainerCrtl).val(cptCode);
                    $("#" + hiddenCtrl.split(",")[1]).val(cptDescription);
                    CtrlId =hiddenCtrl.split(",")[0].replace("#hfCPT","")
                    EncounterChargeCapture.OnCptSelect(cptCode, CtrlId);
                    //} else {
                    //    $("#pnlReportsSSRSDashboard").find(ContainerCrtl).val(cptCode);
                    //}
                }, 105);
            }
            else if (ParentControl == "basicFeeScheduleDetail" || ParentControl == "POSFeeScheduleDetail" || ParentControl == "procedureFeeScheduleDetail" || ParentControl == "ERA_ChargeSearch" || ParentControl == "Patient_PreAuthorization_Detail") {
                setTimeout(function () {
                    $(ContainerCrtl).val(cptCode);
                }, 105);
            }

            else if (ParentControl == "Clinical_CarePlan") {
                setTimeout(function () {
                    $("#pnlClinicalCarePlan").find(ContainerCrtl).val(cptCode + procedureDescription);
                    $("#pnlClinicalCarePlan #hfCPTCode").val(cptCode);
                    $("#pnlClinicalCarePlan #hfCPTDescription").val(procedureDescription);
                    $("#pnlClinicalCarePlan #hfCPTSNOMEDCode").val(SNOMEDId);
                    $("#pnlClinicalCarePlan #hfCPTSNOMEDDescription").val(SNOMEDDescription);
                    /////////////////////
                    var currId = -1;
                    $("#pnlClinicalCarePlan #frmClinicalCarePlan #CarePlanGoals ul#ulCarePlanGoals li[id*='-']").each(function (i, item) {
                        currId = $(this).attr("id");
                    });

                    currId = parseInt(currId) + (-1);
                    var li = "<li  id=" + currId + " onclick='Clinical_CarePlan.fillGoalDetail(this, event);' onmouseover='Clinical_CarePlan.showIcon(this);' onmouseout='Clinical_CarePlan.hideIcon(this);' cptCode=\"" + cptCode + "\" cptDesc=\"" + cptDescription + "\" snomedCode=\"" + SNOMEDId + "\" snomedDesc=\"" + SNOMEDDescription + "\"><a href='#'>" + cptDescription + "<span class='removeIconListHover' onclick='Clinical_CarePlan.GoalDelete(\"" + currId + "\",true, event);'><i class='fa fa-close'></i></span></a></li>"

                    var IsAlreadyExist = false;
                    $('#pnlClinicalCarePlan #ulCarePlanGoals li').each(function () {
                        if ($(this).attr('cptDesc') == null) {

                            if ($(this).text() == cptDescription) {

                                IsAlreadyExist = true;
                            }
                        } else {
                            if ($(this).attr('cptDesc') == cptDescription &&
                                $(this).attr('snomedCode') == SNOMEDId && $(this).attr('snomedDesc') == SNOMEDDescription) {
                                IsAlreadyExist = true;
                            }
                        }
                    });

                    if (!IsAlreadyExist) {
                        $('#pnlClinicalCarePlan #ulCarePlanGoals').append(li);
                        if ($('#pnlClinicalCarePlan #sectionGoals').hasClass('disableAll') == true) {
                            $('#pnlClinicalCarePlan #sectionGoals').removeClass('disableAll');
                        }
                        $(li).trigger('click');
                        $('#pnlClinicalCarePlan #txtProcedure').val('');
                        $('#pnlClinicalCarePlan #Goals #DivCPTAutoComplete').addClass('disableAll');
                        $('#pnlClinicalCarePlan #Goals #txtProcedure').attr('readonly', true);

                    }
                    else {
                        utility.DisplayMessages('Procedure already added', 2);
                        $('#pnlClinicalCarePlan #txtProcedure').val('');
                    }
                }, 105);

                /////////////////////////
            }
            else if (ParentControl == "Admin_IMOCPT") {
                //setTimeout(function () {
                var containerCtrl = ContainerCrtl.split('@')[0];
                var containerCtrlDesc = ContainerCrtl.split('@')[1];
                if (GrandParent.indexOf("Patient_PreAuthorization_Detail") >= 0) {
                    $("#" + Admin_IMOCPT.params.PanelID + " #" + containerCtrl).val(cptCode);
                    utility.RevalidateControl($("#" + containerCtrl), null);
                    ////$("#" + containerCtrl).focus();
                    if (containerCtrlDesc != "undefined") {
                        $("#" + containerCtrlDesc).val(cptDescription);
                    }
                    $("#" + Admin_IMOCPT.params.PanelID + " #" + hiddenCtrl).val(cptCode);
                }
                //AST-519 by:MAHMAD
                else if (GrandParent.indexOf("OrderSet_Procedures") >= 0) {
                  
                        OrderSet_Procedures.BindProcedureGridItem(cptCode, procedureDescription, cptDescription, SNOMEDId, SNOMEDDescription);//for make new row on grid for procedure
                   
                }
                //AST-519 by:MAHMAD
                else if (GrandParent.indexOf("EncounterChargeCapture") >= 0) {
                    $("#" + containerCtrl).val(cptCode);
                    $("#" + hiddenCtrl.split(",")[1]).val(cptDescription);
                    if (containerCtrlDesc != "undefined") {
                        $("#" + containerCtrlDesc).val(cptDescription);
                    }
                    $("#" + hiddenCtrl).val(cptCode);
                    utility.SetAutoCompleteSourceforValidate($("#" + containerCtrl), cptCode, cptDescription);
                    $("#" + containerCtrl).focus();

                    CtrlId = hiddenCtrl.split(",")[0].replace("hfCPT", "")
                    EncounterChargeCapture.OnCptSelect(cptCode, CtrlId);

                }
                else if (GrandParent.indexOf("Clinical_MedicalHx") >= 0) {
                    // $("#pnlClinicalMedicalHx").find(ContainerCrtl).val(cptCode + cptDescription);
                    $("#pnlClinicalMedicalHx #hfCPTCode").val(cptCode);
                    $("#pnlClinicalMedicalHx #hfCPTDescription").val(cptDescription);
                    $("#pnlClinicalMedicalHx #hfCPTSNOMEDCode").val(SNOMEDId);
                    $("#pnlClinicalMedicalHx #hfCPTSNOMEDDescription").val(SNOMEDDescription);
                    if (cptCode != "")
                        $("#pnlClinicalMedicalHx #txtCPTCode").val(cptCode + " - " + cptDescription);
                    else
                        $("#pnlClinicalMedicalHx #txtCPTCode").val(cptDescription);
                }
                else if (GrandParent.indexOf("Patient_Referrals") >= 0) {
                    //Start 13-10-2016 Humaira Yousaf for outgoing referral procedures
                    if (cptCode != "")
                        $("#pnlPatientReferralsOutgoingDetail #txtReferralCPTCode").val(cptCode + " - " + cptDescription);
                    else
                        $("#pnlPatientReferralsOutgoingDetail #txtReferralCPTCode").val(cptDescription);
                    //End 13-10-2016 Humaira Yousaf for outgoing referral procedures
                }
                if (GrandParent.indexOf("OutOfOfficeVisits") >= 0) {
                    $("#pnlBillOutOfOfficeVisits #hfCPTCode").val(cptCode);

                    if (cptCode != "")
                        $("#pnlBillOutOfOfficeVisits #txtCPTCode").val(cptCode + " - " + cptDescription);
                    else
                        $("#pnlBillOutOfOfficeVisits #txtCPTCode").val(cptDescription);
                }

                else if (GrandParent.indexOf("Clinical_PlanOfCare") >= 0) {
                    var currId = -1;
                    $("#pnlPlanOfCare #frmPlanOfCare ul#ulPlanOfCareGoal li[id*='-']").each(function (i, item) {

                        currId = $(this).attr("id");

                    });
                    currId = parseInt(currId) + (-1);
                    var cptCodePlusDash1 = cptCode != "" ? ' - ' + cptCode : cptCode;
                    var li = "<li  id=" + currId + " onclick = 'Clinical_PlanOfCare.fillPlanOfCareGoal(this, event);' onmouseover='Clinical_PlanOfCare.showIcon(this);' onmouseout='Clinical_PlanOfCare.hideIcon(this);' snomedCode=\"" + SNOMEDId + "\" snomedDescription=\"" + SNOMEDDescription + "\"  cptCode=\"" + cptCode + "\" cptDescription=\"" + cptDescription + "\" procedureDescription=\"" + procedureDescription + "\"><a onclick='Clinical_PlanOfCare.activeInActive($(this), event);'>" + cptDescription + cptCodePlusDash1 + "<div id='deleteIcon' style='display:none' class='pull-right' onclick='Clinical_PlanOfCare.deletePlanOfCareGoal($($(this).parent()).parent(), event);'><i class='fa fa-close red'></i></div></a></li>";
                    var IsAlreadyExist = false;
                    $('#pnlPlanOfCare #ulPlanOfCareGoal li').each(function () {
                        if ($(this).attr('cptCode') == cptCode && $(this).attr('cptCode') == cptCode &&
                            $(this).attr('cptDescription') == cptDescription && $(this).attr('cptDescription') == cptDescription &&
                            $(this).attr('procedureDescription') == procedureDescription && $(this).attr('procedureDescription') == procedureDescription) {

                            IsAlreadyExist = true;
                        }
                    });

                    if (!IsAlreadyExist) {
                        $('#pnlPlanOfCare #ulPlanOfCareGoal').append(li);
                        $(li).trigger('click');
                        $('.modal-backdrop').removeClass('in');
                        $('.modal-backdrop').addClass('out');
                        $('.modal-backdrop').hide();
                        $('#pnlPlanOfCare #txtGoal').val('');
                        // Clinical_Procedures.AddInArray(currId, cptCode, cptDescription, true);//for record of procedure
                        //BindProcedureGridItem(currId, cptCode, cptDescription);//for make new row on grid for procedure
                        //if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab)
                        //    Admin_IMOICD.UnLoadTab();
                    }
                    else {
                        utility.DisplayMessages('Goal already added', 2);

                        $('#pnlPlanOfCare #txtGoal').val('');
                    }

                }
                else if (GrandParent.indexOf("Immunization_AddVaccine") >= 0) {
                    if (cptDescription != "" != "" && cptCode != "") {
                        $(hiddenCtrl).val(cptCode != "" ? (cptCode + " - " + cptDescription) : cptDescription);
                        $(hiddenCtrl).attr("Code", cptCode);
                        $(hiddenCtrl).attr("Description", cptDescription);
                    }
                }
                else if (GrandParent.indexOf("Immunization_TherapeuticDetail") >= 0) {
                    if (cptDescription != "" != "" && cptCode != "") {
                        $(hiddenCtrl).val(cptCode != "" ? (cptCode + " - " + cptDescription) : cptDescription);
                        $(hiddenCtrl).attr("Code", cptCode);
                        $(hiddenCtrl).attr("Description", cptDescription);
                    }
                }
                    //Start//21-03-2016//Ahmad Raza//logic to select CPT from Popup for ConsultationOrder
                else if (GrandParent.indexOf("Clinical_ConsultationOrder") >= 0) {
                    $("#pnlClinicalConsultationOrder #hfCPTCode").val(cptCode);
                    $("#pnlClinicalConsultationOrder #hfCPTDescription").val(cptDescription);
                    if (cptCode != "")
                        $("#pnlClinicalConsultationOrder #txtCPTCode").val(cptCode + " - " + cptDescription);
                    else
                        $("#pnlClinicalConsultationOrder #txtCPTCode").val(cptDescription);
                }
                    //End//21-03-2016//Ahmad Raza//logic to select CPT from Popup for ConsultationOrder

                    //Start//21-03-2016//Ahmad Raza//logic to select CPT from Popup for ProcedureOrder
                else if (GrandParent.indexOf("Clinical_ProcedureOrder") >= 0) {
                    $("#pnlClinicalProcedureOrder #hfCPTCode").val(cptCode);
                    $("#pnlClinicalProcedureOrder #hfCPTDescription").val(cptDescription);
                    $("#pnlClinicalProcedureOrder #hfCPTSNOMEDCode").val(SNOMEDId);
                    $("#pnlClinicalProcedureOrder #hfCPTSNOMEDDescription").val(SNOMEDDescription);
                    if (cptCode != "")
                        $("#pnlClinicalProcedureOrder #txtCPTCode").val(cptCode + " - " + cptDescription);
                    else
                        $("#pnlClinicalProcedureOrder #txtCPTCode").val(cptDescription);
                }
                    //End//21-03-2016//Ahmad Raza//logic to select CPT from Popup for ProcedureOrder

                    //Start//21-03-2016//Ahmad Raza//logic to select CPT from Popup for RadiologyOrder
                else if (GrandParent.indexOf("Clinical_RadiologyOrder") >= 0) {
                    $("#pnlClinicalRadiologyOrder #hfCPTCode").val(cptCode);

                    if (cptCode != "")
                        $("#pnlClinicalRadiologyOrder #txtCPTCode").val(cptCode + " - " + procedureDescription);
                    else
                        $("#pnlClinicalRadiologyOrder #txtCPTCode").val(procedureDescription);
                    $("#" + hiddenCtrl.split(",")[1]).val(procedureDescription);

                    if (containerCtrlDesc != "undefined") {
                        $("#" + containerCtrlDesc).val(procedureDescription);
                    }
                    $("#" + hiddenCtrl).val(cptCode);
                }
                    //End//21-03-2016//Ahmad Raza//logic to select CPT from Popup for RadiologyOrder

                    //Start//31-03-2016//Abid Ali//logic to select CPT from Popup for LabOrder
                else if (GrandParent.indexOf("Clinical_LabOrder") >= 0) {

                    if (cptCode != "")
                        $("#" + containerCtrl).val(cptCode + " - " + procedureDescription);
                    else
                        $("#" + containerCtrl).val(procedureDescription);

                    $("#" + hiddenCtrl.split(",")[1]).val(procedureDescription);

                    if (containerCtrlDesc != "undefined") {
                        $("#" + containerCtrlDesc).val(procedureDescription);
                    }
                    $("#" + hiddenCtrl).val(cptCode);
                }

                else if (GrandParent.indexOf("clinicalTabLabOrder") >= 0) {

                    if (cptCode != "")
                        $("#" + containerCtrl).val(cptCode + " - " + procedureDescription);
                    else
                        $("#" + containerCtrl).val(procedureDescription);

                    $("#" + hiddenCtrl.split(",")[1]).val(cptDescription);
                    if (containerCtrlDesc != "undefined") {
                        $("#" + containerCtrlDesc).val(cptDescription);
                    }
                    $("#" + hiddenCtrl).val(cptCode);
                }
                    //End//31-03-2016//Abid Ali//logic to select CPT from Popup for LabOrder


                    //Start//29/01/2016//Abid Ali//logic implimented to fill Procedure field in Clinical_SurgicalHx and filling values in hidden fields
                else if (GrandParent.indexOf("Clinical_SurgicalHx") >= 0) {

                    $("#pnlClinicalSurgicalHx #hfCPTCode").val(cptCode);
                    $("#pnlClinicalSurgicalHx #hfCPTDescription").val(cptDescription);
                    $("#pnlClinicalSurgicalHx #hfCPTSNOMEDCode").val(SNOMEDId);
                    $("#pnlClinicalSurgicalHx #hfCPTSNOMEDDescription").val(SNOMEDDescription);

                    //////////////////////////////////////////////////////////////////////////////////////
                    var currId = -1;
                    $("#pnlClinicalSurgicalHx #frmClinicalSurgicalHx #Surgical ul#ulSurgicalDisease li[id*='-']").each(function (i, item) {

                        currId = $(this).attr("id");

                    });

                    currId = parseInt(currId) + (-1);
                    /*Start 28/01/2016 Abid Ali, bug reslove : changed function call on delete*/
                    var li = "<li  id=" + currId + " onclick='Clinical_SurgicalHx.fillSurgicalHxDisease(this, event);' onmouseover='Clinical_SurgicalHx.showIcon(this);' onmouseout='Clinical_SurgicalHx.hideIcon(this);' cptCode=\"" + cptCode + "\" cptDesc=\"" + cptDescription + "\" snomedCode=\"" + SNOMEDId + "\" snomedDesc=\"" + SNOMEDDescription + "\"><a href='#'>" + cptDescription + "<span class='removeIconListHover' onclick='Clinical_SurgicalHx.deleteSurgicalHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"
                    /*End  28/01/2016 Abid Ali, bug reslove : changed function call on delete*/
                    var IsAlreadyExist = false;
                    $('#pnlClinicalSurgicalHx #ulSurgicalDisease li').each(function () {
                        if ($(this).attr('cptDesc') == null) {

                            if ($(this).text() == cptDescription) {

                                IsAlreadyExist = true;
                            }
                        } else {
                            if ($(this).attr('cptDesc') == cptDescription &&
                                $(this).attr('snomedCode') == SNOMEDId && $(this).attr('snomedDesc') == SNOMEDDescription) {
                                IsAlreadyExist = true;
                            }
                        }
                    });

                    if (!IsAlreadyExist) {
                        $('#pnlClinicalSurgicalHx #ulSurgicalDisease').append(li);
                        $(li).trigger('click');
                        $('#pnlClinicalSurgicalHx #txtDisease').val('');

                        if (Clinical_SurgicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                            var diseaseId = $('#' + Clinical_SurgicalHx.params.PanelID + " #ulSurgicalDisease > li.active").attr('id');
                            var disease = $(li).get(0).outerHTML;
                            var diseaseDetail = $('#' + Clinical_SurgicalHx.params.PanelID + " #sectionSurgicalDetails").clone();
                            $(diseaseDetail).resetAllControls(null);
                            var diseaseData = $(diseaseDetail).getMyJSONByName();
                            Clinical_SurgicalHx.cacheSurgicalHxJSON(diseaseId, diseaseData, disease);
                        }
                        //var isUnload = "false";
                        //var txt = $('#pnlClinicalSurgicalHx #txtDisease');
                        //if (txt.is('[data-popupunload]')) {
                        //    isUnload = txt.attr('data-popupunload');
                        //}

                        //if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                        //    txt.attr("data-popupunload", "false");
                        //    Admin_IMOICD.UnLoadTab();
                        //}
                    }
                    else {
                        utility.DisplayMessages('Procedure already added', 2);
                        $('#pnlClinicalSurgicalHx #txtDisease').val('');
                    }

                    //////////////////////////////////////////////////////////////////////////////////





                }
                    //End//29/01/2016//Abid Ali//logic implimented to fill Procedure field in Clinical_SurgicalHx and filling values in hidden fields

                else if (GrandParent.indexOf("Favorite_ConsultationOrderDetail") >= 0) {

                    Favorite_ConsultationOrderDetail.bindToCPTList(cptCode, cptDescription);
                }

                else if (GrandParent.indexOf("Favorite_ProcedureOrderDetail") >= 0) {

                    Favorite_ProcedureOrderDetail.bindToCPTList(cptCode, cptDescription);
                }
                else if (GrandParent.indexOf("Favorite_ProcedureDetail") >= 0) {
                    Favorite_ProcedureDetail.bindToCPTList(cptCode, cptDescription);
                }
                else if (GrandParent.indexOf("Favorite_RadiologyOrderDetail") >= 0) {

                    Favorite_RadiologyOrderDetail.bindToCPTList(cptCode, cptDescription);
                }
                    //Start 31-03-2016 Humaira Yousaf for favorite history
                else if (GrandParent.indexOf("Favorite_FamilyHistoryDetail") >= 0) {

                    Favorite_FamilyHistoryDetail.bindToCPTList(cptCode, cptDescription);
                }
                else if (GrandParent.indexOf("Favorite_MedicalHistoryDetail") >= 0) {

                    Favorite_MedicalHistoryDetail.bindToCPTList(cptCode, cptDescription);
                }
                else if (GrandParent.indexOf("Favorite_SurgicalHistoryDetail") >= 0) {

                    //Favorite_SurgicalHistoryDetail.bindToCPTList(cptCode, cptDescription);

                    var currId = -1;
                    $("#pnlFavoriteSurgicalHistoryDetail #frmFavoriteSurgicalHistoryDetail ul#ulFavSurgicalHistoryDisease li[id*='-']").each(function (i, item) {

                        currId = $(this).attr("id");

                    });

                    currId = parseInt(currId) + (-1);

                    var li = "<li favouriteListICDId = " + currId + " FavouriteListId=" + currId + "  id=" + currId + " onmouseover='Favorite_SurgicalHistoryDetail.showIcon(this);' onmouseout='Favorite_SurgicalHistoryDetail.hideIcon(this);' cptCode=\"" + cptCode + "\" cptDesc=\"" + cptDescription + "\" snomedCode=\"" + SNOMEDId + "\" snomedDesc=\"" + SNOMEDDescription + "\"><a href='#'>" + cptDescription + "<span class='removeIconListHover' onclick='Favorite_SurgicalHistoryDetail.deleteCPTFromCPTData(this,\"" + cptCode + "\",\"" + cptDescription + "\",\"" + SNOMEDId + "\",\"" + SNOMEDDescription + "\");'><i class='fa fa-close'></i></span></a></li>";

                    var IsAlreadyExist = false;
                    $('#pnlFavoriteSurgicalHistoryDetail #ulFavSurgicalHistoryDisease li').each(function () {
                        if ($(this).attr('cptCode') == cptCode && $(this).attr('cptdesc') == cptDescription) {
                            IsAlreadyExist = true;
                        }
                    });

                    if (!IsAlreadyExist) {
                        $('#pnlFavoriteSurgicalHistoryDetail #ulFavSurgicalHistoryDisease').append(li);
                        $(li).trigger('click');
                        var item = {};
                        item["CPTCode"] = cptCode;
                        item["CPTCodeDescription"] = cptDescription;
                        item["SNOMED_Description"] = SNOMEDDescription;
                        item["SNOMEDID"] = SNOMEDId;
                        Favorite_SurgicalHistoryDetail.CPTData.push(item);
                        $('#pnlFavoriteSurgicalHistoryDetail #frmFavoriteSurgicalHistoryDetail #txtDisease').val(cptDescription)
                        var bootstrapValidator = $('#pnlFavoriteSurgicalHistoryDetail #frmFavoriteSurgicalHistoryDetail').data('bootstrapValidator');
                         bootstrapValidator.revalidateField("Diagnosis");
                        $('#pnlFavoriteSurgicalHistoryDetail #txtDisease').val('');

                        //var isUnload = "false";
                        //var txt = $('#pnlFavoriteSurgicalHistoryDetail #txtDiagnosis');
                        //if (txt.is('[data-popupunload]')) {
                        //    isUnload = txt.attr('data-popupunload');
                        //}

                        //if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                        //    txt.attr("data-popupunload", "false");
                        //    Admin_IMOICD.UnLoadTab();
                        //}
                    }
                    else {
                        utility.DisplayMessages('Procedure already added', 2);

                        $('#pnlFavoriteSurgicalHistoryDetail #txtDiagnose').val('');
                    }
                }
                    //End 31-03-2016 Humaira Yousaf for favorite history
                else if (GrandParent.indexOf("Favorite_LabOrderDetail") >= 0) {

                    Favorite_LabOrderDetail.bindToCPTList(cptCode, cptDescription);
                }
                    //----------------------- start 17/03/2016 irfan code for radiology

                else if (GrandParent.indexOf("ClinicalRadiologyOrderDetail") >= 0) {

                    ClinicalRadiologyOrderDetail.BindProcedureGridItem(cptCode, procedureDescription, cptDescription);

                }
                else if (GrandParent.indexOf("OrderSet_RadiologyOrderDetails") >= 0) {

                    OrderSet_RadiologyOrderDetails.BindProcedureGridItem(cptCode, procedureDescription, cptDescription);

                }
                else if (GrandParent == "ClinicalProcedureOrderDetail") {
                    $("#pnlClinicalProcedureOrder #hfCPTSNOMEDCode").val(SNOMEDId);
                    $("#pnlClinicalProcedureOrder #hfCPTSNOMEDDescription").val(SNOMEDDescription);
                    ClinicalProcedureOrderDetail.BindProcedureGridItem(cptCode, procedureDescription, cptDescription);
                }
                else if (GrandParent == "OrderSet_ProcedureOrderDetails") {
                    $("#pnlOsProcedureOrderDetail #hfCPTSNOMEDCode").val(SNOMEDId);
                    $("#pnlOsProcedureOrderDetail #hfCPTSNOMEDDescription").val(SNOMEDDescription);
                    OrderSet_ProcedureOrderDetails.BindProcedureGridItem(cptCode, procedureDescription, cptDescription);
                }

                else if (GrandParent == "ClinicalConsultationOrderDetail") {

                    ClinicalConsultationOrderDetail.BindProcedureGridItem(cptCode, procedureDescription, cptDescription);

                }
                else if (GrandParent == "ClinicalLabOrderDetail") {
                    setTimeout(function () {

                        ClinicalLabOrderDetail.BindLabOrderGridItem(cptCode, procedureDescription, cptDescription);

                    }, 105);
                }
                else if (GrandParent == "OrderSet_LabOrderDetails") {
                    setTimeout(function () {

                        OrderSet_LabOrderDetails.BindLabOrderGridItem(cptCode, procedureDescription, cptDescription);

                    }, 105);
                }
                else if (GrandParent == "ClinicalLabResultDetail") {
                    setTimeout(function () {

                        ClinicalLabResultDetail.BindLabOrderResultGridItem(cptCode, procedureDescription, cptDescription);

                    }, 105);
                }
                else if (GrandParent == "Clinical_LOINC") {
                    setTimeout(function () {

                        Clinical_LOINC.BindResultGridItem(cptCode, procedureDescription, cptDescription);

                    }, 105);
                }
                else if (GrandParent == "ClinicalRadiologyResultDetail") {
                    setTimeout(function () {

                        ClinicalRadiologyResultDetail.BindRadiologyOrderResultGridItem(cptCode, procedureDescription, cptDescription);

                    }, 105);
                }
                    //----------------------- end 17/03/2016 irfan code for radiology


                    //Start//22/01/2016//Abid Ali//logic implimented to fill Procedure field in HospitalizationHx and filling values in hidden fields
                else if (GrandParent.indexOf("Clinical_HospitalizationHx") >= 0 || GrandParent.indexOf("clinicalTabHospitalizationHx") >= 0) {

                    $("#pnlClinicalHospitalizationHx").find("#hfCPTCode").val(cptCode);
                    $("#pnlClinicalHospitalizationHx").find("#hfCPTDescription").val(procedureDescription);
                    $("#pnlClinicalHospitalizationHx").find("#hfSNOMEDCode").val(SNOMEDId);
                    $("#pnlClinicalHospitalizationHx").find("#hfSNOMEDDescription").val(SNOMEDDescription);

                    if (cptCode != "") {
                        $("#pnlClinicalHospitalizationHx #txtHospitalizationCPTCode").val(cptCode + " - " + procedureDescription);
                        $('#pnlClinicalHospitalizationHx #frmClinicalHospitalizationHx #ddlHospitalizationStatus').prop("disabled", false);
                    }
                    else {
                        $("#pnlClinicalHospitalizationHx #txtHospitalizationCPTCode").val(procedureDescription);
                        $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #ddlHospitalizationStatus").val('');
                        $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #ddlHospitalizationStatus").prop("disabled", true);
                    }
                }
                    //End//22/01/2016//Abid Ali//logic implimented to fill Procedure field in HospitalizationHx and filling values in hidden fields

                else if (GrandParent.indexOf("mstrTabReports") >= 0) {
                    var cntnrCtrl = "#" + ContainerCrtl.substring(ContainerCrtl.indexOf("#") + 1, ContainerCrtl.indexOf(","));
                    if (cntnrCtrl.indexOf("txtCPTCodeProcedure") > -1) {

                        $("#pnlReportsSSRSDashboard " + cntnrCtrl).val(cptDescription);
                    }
                    else {
                        $("#pnlReportsSSRSDashboard " + cntnrCtrl).val(cptCode);
                    }
                }
                else if (GrandParent.indexOf("Clinical_Procedures") >= 0) {
                    Clinical_Procedures.BindProcedureGridItem(cptCode, procedureDescription, cptDescription, SNOMEDId, SNOMEDDescription);//for make new row on grid for procedure
                }
                else if (GrandParent.indexOf("Immunization_AddVaccine") >= 0) {
                    if (cptDescription != "" != "" && cptCode != "") {
                        $(hiddenCtrl).val(cptCode != "" ? (cptCode + " - " + cptDescription) : cptDescription);
                        $(hiddenCtrl).attr("Code", cptCode);
                        $(hiddenCtrl).attr("Description", cptDescription);
                    }
                }
                else if (GrandParent.indexOf("Immunization_TherapeuticDetail") >= 0) {
                    if (cptDescription != "" != "" && cptCode != "") {
                        $(hiddenCtrl).val(cptCode != "" ? (cptCode + " - " + cptDescription) : cptDescription);
                        $(hiddenCtrl).attr("Code", cptCode);
                        $(hiddenCtrl).attr("Description", cptDescription);
                    }
                }
                else if (GrandParent.indexOf("Patient_Referrals_Incoming_Detail") >= 0) {
                    Patient_Referrals_Incoming_Detail.BindProcedureGridItem(cptCode, procedureDescription, cptDescription, SNOMEDId, SNOMEDDescription);//for make new row on grid for Incoming Referral procedure
                }
                else if (GrandParent=="OrderSet_Patient_Referrals_Outgoing_Detail") {
                    OrderSet_Patient_Referrals_Outgoing_Detail.BindProcedureGridItem(cptCode, procedureDescription, cptDescription, SNOMEDId, SNOMEDDescription);//for make new row on grid for Outgoing procedure
                }
                else if (GrandParent.indexOf("Patient_Referrals_Outgoing_Detail") >= 0) {
                    Patient_Referrals_Outgoing_Detail.BindProcedureGridItem(cptCode, procedureDescription, cptDescription, SNOMEDId, SNOMEDDescription);//for make new row on grid for Outgoing procedure
                }
                
                
                else if (GrandParent == "Clinical_ImplantableDetail") {
                    setTimeout(function () {
                        Clinical_ImplantableDetail.BindProcedureGridItem(cptCode, procedureDescription, cptDescription, SNOMEDId, SNOMEDDescription);//for make new row on grid for implantable device procedure
                    }, 105);
                }

                else if (GrandParent == "providerDetail") {
                    var currId = -1;
                    $("#providerDetail ul#ulProceduresList li[id*='-']").each(function (i, item) {
                        currId = $(this).attr("id");
                    });

                    currId = parseInt(currId) + (-1);

                    var li = "<li procedureListId = " + currId + "  id=" + currId + " cptCode=\"" + cptCode + "\" cptDesc=\"" + cptDescription + "\" snomedCode=\"" + SNOMEDId + "\" snomedDesc=\"" + SNOMEDDescription + "\"><a href='#'>" + cptCode + " - " + cptDescription + "<span class='removeIconListHover' onclick='providerDetail.deleteCPTFromCPTData(this,\"" + cptCode + "\",\"" + cptDescription + "\",\"" + SNOMEDId + "\",\"" + SNOMEDDescription + "\");'><i class='fa fa-close'></i></span></a></li>";

                    var IsAlreadyExist = false;
                    $('#providerDetail #ulProceduresList li').each(function () {
                        if ($(this).attr('cptCode') == cptCode && $(this).attr('cptdesc') == cptDescription) {
                            IsAlreadyExist = true;
                        }
                    });

                    if (!IsAlreadyExist) {
                        $('#providerDetail #ulProceduresList').addClass('panel-body p-none height-max150 height150 overflowY own-scroll');
                        $('#providerDetail #ulProceduresList').append(li);
                        $(li).trigger('click');
                        var item = {};
                        item["CPTCode"] = cptCode;
                        item["CPTCodeDescription"] = cptDescription;
                        item["SNOMED_Description"] = SNOMEDDescription;
                        item["SNOMEDID"] = SNOMEDId;
                        providerDetail.CPTData.push(item);

                        $('#providerDetail #txtProcedures').val('');
                    }
                    else {
                        utility.DisplayMessages('Procedure already added', 2);

                        $('#providerDetail #txtProcedures').val('');
                    }
                }

                else if (GrandParent.indexOf("Clinical_CustomFormsPreview") != -1) {
                    var selectedProblemTool = $("#pnlClinicalCustomFormsPreview #frmCustomFormPreview").find('#' + customFormToolsParentId);
                    if (selectedProblemTool) {
                        var ContainerCrtl = $(selectedProblemTool).find('#txtProceduresCustomForm');
                        $(ContainerCrtl).val(cptCode + " " + cptDescription);
                        Clinical_CustomFormsPreview.BindProcedureListItem(cptCode, cptDescription, ContainerCrtl);
                    }
                }

                else if (GrandParent.indexOf("Batch_FaxSend") != -1) {
                    var selectedProblemTool = $('#' + Batch_FaxSend.params["PanelID"] + '  #frmBatch_FaxSend').find('#' + customFormToolsParentId);
                    if (selectedProblemTool) {
                        var ContainerCrtl = $(selectedProblemTool).find('#txtProceduresCustomForm');
                        $(ContainerCrtl).val(cptCode + " " + cptDescription);
                        Batch_FaxSend.BindProcedureListItem(cptCode, cptDescription, ContainerCrtl);
                    }
                }

                else if (GrandParent.indexOf("BillingInformation") >= 0) {
                    setTimeout(function (hiddenCtrl, SNOMEDId, SNOMEDDescription) {
                        utility.SetAutoCompleteSourceforValidate($("#pnlBillingInformation #" + ContainerCrtl), cptCode, cptDescription);
                        var Controls = [];
                        Controls = hiddenCtrl.split(',');
                        if (BillingInformation != null && BillingInformation.BillingInfoTime != null) {
                            var CurrentCpt = $.grep(BillingInformation.BillingInfoTime, function (a) {
                                return a.ENMCPT == cptCode
                            });
                            if (CurrentCpt.length > 0) {
                                if (CurrentCpt[0].Type == 'New') {
                                    $("#" + BillingInformation.params.PanelID + " #ddlType").val('1');
                                }
                                else if (CurrentCpt[0].Type == 'Established') {
                                    $("#" + BillingInformation.params.PanelID + " #ddlType").val('2');
                                }
                                BillingInformation.ddlType_Change($("#pnlBillingInformation #ddlType"), CurrentCpt[0].Description);
                            }
                        }
                        if (Controls.length > 0) {
                            for (var index in Controls) {
                                if (Controls[index].toString().indexOf('#') == -1) {
                                    Controls[index] = "#" + Controls[index];
                                }
                                if (Controls[index].toString().indexOf("hfCPTCode") > -1) {
                                    $("#pnlBillingInformation " + Controls[index]).val(cptCode);
                                    $("#pnlBillingInformation " + Controls[index]).parent().parent().find('input[type=text]').val(cptCode + ' - ' + procedureDescription);
                                }

                                if (Controls[index].toString().indexOf("hfCPTDescription") > -1)
                                    $("#pnlBillingInformation " + Controls[index]).val(procedureDescription);

                                if (Controls[index].toString().indexOf("hfSNOMEDId") > -1)
                                    $("#pnlBillingInformation " + Controls[index]).val(SNOMEDId);

                                if (Controls[index].toString().indexOf("hfSNOMEDDescription") > -1)
                                    $("#pnlBillingInformation " + Controls[index]).val(SNOMEDDescription);

                                if (Controls[index].toString().indexOf("hfBillingInfoCPTId") > -1)
                                    $("#pnlBillingInformation " + Controls[index]).val("");

                                if (Controls[index].toString().indexOf("hfCPTSNOMEDCodeId") > -1)
                                    $("#pnlBillingInformation " + Controls[index]).val(SNOMEDId);

                                if (Controls[index].toString().indexOf("hfCPTSNOMEDDescription") > -1)
                                    $("#pnlBillingInformation " + Controls[index]).val(SNOMEDDescription);
                                if (cptCode == "") {
                                    $("#pnlBillingInformation " + Controls[index]).parent().parent().find('input[type=text]').val('');
                                    $("#pnlBillingInformation " + Controls[index]).val('');
                                }
                            }
                            BillingInformation.AddNewChargeRowWrapper();
                        }
                    }, 105, hiddenCtrl, SNOMEDId, SNOMEDDescription)
                }
                else if (GrandParent.indexOf("Clinical_CarePlan") >= 0) {

                    $("#pnlClinicalCarePlan #hfCPTCode").val(cptCode);
                    $("#pnlClinicalCarePlan #hfCPTDescription").val(cptDescription);
                    $("#pnlClinicalCarePlan #hfCPTSNOMEDCode").val(SNOMEDId);
                    $("#pnlClinicalCarePlan #hfCPTSNOMEDDescription").val(SNOMEDDescription);

                    var currId = -1;
                    $("#pnlClinicalCarePlan #frmClinicalCarePlan #CarePlanGoals ul#ulCarePlanGoals li[id*='-']").each(function (i, item) {
                        currId = $(this).attr("id");
                    });

                    currId = parseInt(currId) + (-1);

                    var li = "<li  id=" + currId + " onclick='Clinical_CarePlan.fillGoalDetail(this, event);' onmouseover='Clinical_CarePlan.showIcon(this);' onmouseout='Clinical_CarePlan.hideIcon(this);' cptCode=\"" + cptCode + "\" cptDesc=\"" + cptDescription + "\" snomedCode=\"" + SNOMEDId + "\" snomedDesc=\"" + SNOMEDDescription + "\"><a href='#'>" + cptDescription + "<span class='removeIconListHover' onclick='Clinical_CarePlan.GoalDelete(\"" + currId + "\",true, event);'><i class='fa fa-close'></i></span></a></li>"

                    var IsAlreadyExist = false;
                    $('#pnlClinicalCarePlan #ulCarePlanGoals li').each(function () {
                        if ($(this).attr('cptDesc') == null) {

                            if ($(this).text() == cptDescription) {

                                IsAlreadyExist = true;
                            }
                        } else {
                            if ($(this).attr('cptDesc') == cptDescription &&
                                $(this).attr('snomedCode') == SNOMEDId && $(this).attr('snomedDesc') == SNOMEDDescription) {
                                IsAlreadyExist = true;
                            }
                        }
                    });

                    if (!IsAlreadyExist) {
                        $('#pnlClinicalCarePlan #ulCarePlanGoals').append(li);
                        if ($('#pnlClinicalCarePlan #sectionGoals').hasClass('disableAll') == true) {
                            $('#pnlClinicalCarePlan #sectionGoals').removeClass('disableAll');
                        }
                        $(li).trigger('click');
                        $('#pnlClinicalCarePlan #txtProcedure').val('');
                        $('#pnlClinicalCarePlan #Goals #DivCPTAutoComplete').addClass('disableAll');
                        $('#pnlClinicalCarePlan #Goals #txtProcedure').attr('readonly', true);

                    }
                    else {
                        utility.DisplayMessages('Procedure already added', 2);
                        $('#pnlClinicalCarePlan #txtProcedure').val('');
                    }
                }
                else {
                    $("#" + GrandParent + " #" + containerCtrl).val(cptCode);
                    if (containerCtrlDesc != "undefined") {
                        $("#" + GrandParent + " #" + containerCtrlDesc).val(cptDescription);
                    }
                    $("#" + GrandParent + " #" + hiddenCtrl).val(cptCode);
                }


                Admin_IMOCPT.UnLoadTab();
                //}, 130);
            }
            else if (ParentControl == "Patient_PreAuthorization_Detail") {
                $("#pnlPreAuthorizationDetail #txtCPTCode").val(cptCode);
            }
            else if (ParentControl.indexOf("Clinical_CustomFormsPreview") != -1) {
                var selectedProblemTool = $("#pnlClinicalCustomFormsPreview #frmCustomFormPreview").find('#' + customFormToolsParentId);
                if (selectedProblemTool) {
                    var ContainerCrtl = $(selectedProblemTool).find('#txtProceduresCustomForm');
                    $(ContainerCrtl).val(cptCode + " " + cptDescription);
                    Clinical_CustomFormsPreview.BindProcedureListItem(cptCode, cptDescription, ContainerCrtl);
                }
            }
            else if (ParentControl.indexOf("Batch_FaxSend") != -1) {
                var selectedProblemTool = $('#' + Batch_FaxSend.params["PanelID"] + '  #frmBatch_FaxSend').find('#' + customFormToolsParentId);
                if (selectedProblemTool) {
                    var ContainerCrtl = $(selectedProblemTool).find('#txtProceduresCustomForm');
                    $(ContainerCrtl).val(cptCode + " " + cptDescription);
                    Batch_FaxSend.BindProcedureListItem(cptCode, cptDescription, ContainerCrtl);
                }
            }
        }
        //
    },

    ShowHistory: function () {
        var PanelID = 'cptcodeDetail';
        var ParentCtrl = 'cptcodeDetail';
        var ProfileName = 'CPT';
        var DBTableName = 'CPTCode,CPTPlan';
        var ColumnKeyId = cptcodeDetail.params.CPTCodeId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },

    //SaveCPTPlanInfo: function (CPTPlan, Code) {
    //    var data = "CPTPlan=" + CPTPlan + "&CPTPlanCode=" + Code + "&CPTCodeID=" + cptcodeDetail.params.CPTCodeId;
    //    // serach parameter , class name, command name of class
    //    return MDVisionService.defaultService(data, "ADMIN_CPT_CODE_DETAIL", "SAVE_PLAN_INFO");
    //},

    //UpdateCPTPlanInfo: function (CPTPlan, Code, CPTPlanId) {
    //    var data = "CPTPlan=" + CPTPlan + "&CPTPlanCode=" + Code + "&CPTPlanId=" + CPTPlanId + "&CPTCodeID=" + cptcodeDetail.params.CPTCodeId;
    //    // serach parameter , class name, command name of class
    //    return MDVisionService.defaultService(data, "ADMIN_CPT_CODE_DETAIL", "UPDATE_PLAN_INFO");
    //},

    //LoadCPTPlanInfo: function () {
    //    var data = "CPTCodeID=" + cptcodeDetail.params.CPTCodeId;
    //    // serach parameter , class name, command name of class
    //    return MDVisionService.defaultService(data, "ADMIN_CPT_CODE_DETAIL", "LOAD_PLAN_INFO");
    //},

    //DeleteCPTPlanInfo: function (CPTPlanId) {
    //    var data = "CPTPlanId=" + CPTPlanId;
    //    // serach parameter , class name, command name of class
    //    return MDVisionService.defaultService(data, "ADMIN_CPT_CODE_DETAIL", "DELETE_PLAN_INFO");
    //},

    UnLoad: function () {
        if ($('#frmCPTCodeDetail').serialize() != $('#frmCPTCodeDetail').data('serialize')) {
            utility.myConfirm('2', function () {
                UnloadActionPan(cptcodeDetail.params["ParentCtrl"], "cptcodeDetail");
            }, function () { },
                    '2'
                );
        }
        else {
            UnloadActionPan(cptcodeDetail.params["ParentCtrl"], "cptcodeDetail");
        }
    },


    //LoadCPTPlans: function () {
    //    cptcodeDetail.LoadCPTPlanInfo().done(function (response) {
    //        if (response.status != false) {
    //            if (response.CPTPlanCount > 0) {
    //                var CPTPlanJSON = JSON.parse(response.CPTPlan_JSON);
    //                $.each(CPTPlanJSON, function (i, item) {
    //                    var $row = $('<tr/>');
    //                    $row.attr("CPTPlanInfoId", item.CPTPlanId)
    //                    $row.append('<td style="display:none;">' + item.CPTPlanId + '</td><td>' + item.PlanName + '</td><td>' + item.CPTPlanCode + '</td>');
    //                    $("#dgvCPTPlanInfo tbody").last().append($row);
    //                });
    //            }
    //        }
    //    });
    //},

    //CPTPlanInfo: function () {
    //    $(function () {
    //        var self = $('#gridform');
    //        self.loadDropDowns(true);
    //        var tbl = $("#dgvCPTPlanInfo");
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
    //        $("#popup-dialog-cptplan").dialog({
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
    //            AppPrivileges.GetFormPrivileges("CPT", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
    //                if (strMessage == "") {
    //                    var rowIndx = getRowIndx();
    //                    if (rowIndx != null) {
    //                        var DM = $grid.pqGrid("option", "dataModel");
    //                        var data = DM.data;
    //                        var row = data[rowIndx];
    //                        var $frm = $("form#frmCPTPlanInfo");
    //                        var optVal = $("#ddlInsurancePlan option:contains('" + row[1] + "')").attr('value');
    //                        $("#ddlInsurancePlan").val(optVal)
    //                        $frm.find("input[name='Code']").val(row[2]);

    //                        $("#popup-dialog-cptplan").dialog({
    //                            title: "Edit Record (" + (rowIndx + 1) + ")",
    //                            buttons: {
    //                                Update: function () {
    //                                    //save the record in DM.data.
    //                                    var planId = $frm.find("select[name='Plan']").val();
    //                                    row[1] = $frm.find("select[name='Plan'] option[value='" + planId + "']").text();
    //                                    row[2] = $frm.find("input[name='Code']").val();

    //                                    cptcodeDetail.UpdateCPTPlanInfo(planId, row[2], row[0]);

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
    //            AppPrivileges.GetFormPrivileges("CPT", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
    //                if (strMessage == "") {
    //                    //debugger;
    //                    var DM = $grid.pqGrid("option", "dataModel");
    //                    var data = DM.data;

    //                    var $frm = $("form#frmCPTPlanInfo");
    //                    $frm.find("input").val("");
    //                    $frm.find("select").val("");
    //                    //$frm.find("select[name='Plan'] option[value='']").attr('selected', 'selected');

    //                    $("#popup-dialog-cptplan").dialog({
    //                        title: "Add Record",
    //                        buttons: {
    //                            Add: function () {
    //                                var row = [];
    //                                //save the record in DM.data.
    //                                var planId = $frm.find("select[name='Plan']").val();
    //                                row[1] = $frm.find("select[name='Plan'] option[value='" + planId + "']").text();
    //                                row[2] = $frm.find("input[name='Code']").val();

    //                                cptcodeDetail.SaveCPTPlanInfo(planId, row[2]).done(function (response) {
    //                                    if (response.status != false) {
    //                                        row[0] = response.CPTPlanId;
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
    //                    $("#popup-dialog-cptplan").dialog("open");
    //                }
    //                else
    //                    utility.DisplayMessages(strMessage, 2);
    //            });
    //        }

    //        //delete Row.
    //        function deleteRow() {
    //            var strMessage = "";
    //            AppPrivileges.GetFormPrivileges("CPT", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
    //                if (strMessage == "") {
    //                    var rowIndx = getRowIndx();
    //                    if (rowIndx != null) {
    //                        var DM = $grid.pqGrid("option", "dataModel");
    //                        cptcodeDetail.DeleteCPTPlanInfo(DM.data[rowIndx][0]);
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
    ValidateUnit: function (id) {
        id = id.replace("id_", "");
        var self = $("#cptcodeDetail #pnlCPTNdcInfo tr#" + id + " #txtNDCUnits" + id)
        if (parseInt($(self).val()) <= 0) {
            (self).val("");
        }
    }
}
