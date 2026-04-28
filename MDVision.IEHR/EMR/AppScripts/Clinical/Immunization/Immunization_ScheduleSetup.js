Immunization_ScheduleSetup = {
    bIsFirstLoad: true,
    params: [],
    myGridBirth_2_years: null,
    myGrid2_18_years: null,
    myGridAdult: null,
    myGridRecurring: null,
    myGrid: null,
    AllVaccine: [],
    AllSearchedVaccine: [],
    AllSearchedForGridVaccine: [],
    Load: function (params) {
        Immunization_ScheduleSetup.params = params;
        Immunization_ScheduleSetup.AllVaccine = [];
        Immunization_ScheduleSetup.AllSearchedVaccine = [];
        Immunization_ScheduleSetup.AllSearchedForGridVaccine = [];
        Immunization_ScheduleSetup.params.mode = "Add";
        if (Immunization_ScheduleSetup.params.PanelID != 'pnlImmunization_ScheduleSetup') {
            Immunization_ScheduleSetup.params.PanelID = Immunization_ScheduleSetup.params.PanelID + ' #pnlImmunization_ScheduleSetup';
        } else {
            Immunization_ScheduleSetup.params.PanelID = 'pnlImmunization_ScheduleSetup';
        }
        var self = $('#' + Immunization_ScheduleSetup.params.PanelID);
        Immunization_ScheduleSetup.PopulateDropDown("1", "frmImmunizationScheduleSetup #ddlCategory", "1", "GetAdministerVaccine_Category");
        Immunization_ScheduleSetup.domReadyFunc();
        if (Immunization_ScheduleSetup.bIsFirstLoad == true) {
            CacheManager.BindDropDownsByID(self.find('#ddlScheduleType'), 'GetImmunizationScheduleType', false, null, null, true);
            CacheManager.BindDropDownsByID(self.find('#ddlScheduleTypeSearch'), 'GetImmunizationScheduleType', true, null, null, true);
            ("1", "frmImmunizationScheduleSetup #ddlCategory", "1", "GetAdministerVaccine_Category");
            self.loadDropDowns(true).done(function () {
                Immunization_ScheduleSetup.ValidateScheduleSetup();
                Immunization_ScheduleSetup.LoadSchedlerData("1", "Birth_2_years");
            });
        }
    },
    domReadyFunc: function () {
        $('#' + Immunization_ScheduleSetup.params.PanelID + ' #frmImmunizationScheduleSetup').on('keydown', '#txtStartDueDate', function (e) { -1 !== $.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) || /65|67|86|88/.test(e.keyCode) && (!0 === e.ctrlKey || !0 === e.metaKey) || 35 <= e.keyCode && 40 >= e.keyCode || (e.shiftKey || 48 > e.keyCode || 57 < e.keyCode) && (96 > e.keyCode || 105 < e.keyCode) && e.preventDefault() });
        $('#' + Immunization_ScheduleSetup.params.PanelID + ' #frmImmunizationScheduleSetup').on('keydown', '#txtEndOverDueDate', function (e) { -1 !== $.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) || /65|67|86|88/.test(e.keyCode) && (!0 === e.ctrlKey || !0 === e.metaKey) || 35 <= e.keyCode && 40 >= e.keyCode || (e.shiftKey || 48 > e.keyCode || 57 < e.keyCode) && (96 > e.keyCode || 105 < e.keyCode) && e.preventDefault() });
        $('#' + Immunization_ScheduleSetup.params.PanelID + ' #frmImmunizationScheduleSetup').on('keydown', '#txtMaxage', function (e) { -1 !== $.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) || /65|67|86|88/.test(e.keyCode) && (!0 === e.ctrlKey || !0 === e.metaKey) || 35 <= e.keyCode && 40 >= e.keyCode || (e.shiftKey || 48 > e.keyCode || 57 < e.keyCode) && (96 > e.keyCode || 105 < e.keyCode) && e.preventDefault() });
    },
    LoadSchedlerData: function (SchTypeID, TabId) {
        var dfd = $.Deferred();
        AppPrivileges.GetFormPrivileges("Immunization_Schedule Setup", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Immunization_ScheduleSetup.SearchSchedlerData_DBCall(SchTypeID).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        $.when(Immunization_ScheduleSetup.ScheduleSetupGridLoad(response, TabId)).then(function () {
                            dfd.resolve();
                        });
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                        dfd.resolve();
                    }
                });

            }
            else {
                utility.DisplayMessages(strMessage, 2);
                dfd.resolve();
            }
        });
        return dfd;
    },
    ScheduleSetupSearch: function () {
        var ScheduleTypeId = $("#" + Immunization_ScheduleSetup.params.PanelID + " #frmImmunizationScheduleSetup #ddlScheduleTypeSearch").val();
        Immunization_ScheduleSetup.LoadSchedlerData(ScheduleTypeId, Immunization_ScheduleSetup.ActiveTab());
    },
    SearchScheduleSetup: function (ScheduleTypeId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = new Object();
        objData["ScheduleTypeId"] = ScheduleTypeId;
        objData["VaccineGroupID"] = $("#" + Immunization_ScheduleSetup.params.PanelID + " #frmImmunizationScheduleSetup #ddlCategorySearch").val();
        objData["IsActive"] = $('#' + Immunization_ScheduleSetup.params.PanelID + ' #frmImmunizationScheduleSetup #divSwitch #switchActive').is(':checked');
        objData["VaccineId"] = $("#" + Immunization_ScheduleSetup.params.PanelID + " #frmImmunizationScheduleSetup #hfVaccineIdSearch").val();
        if (globalAppdata['AppUserName'] != DefaultUser) {
            objData["EntityId"] = globalAppdata["SeletedEntityId"];
        } else {
            objData["EntityId"] = null;
        }
        objData["commandType"] = "Get_All_ScheduleSetups";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ImmunizationScheduleSetup");
    },
    UnLoadTab: function () {
        var objDeffered = $.Deferred();
        if (Immunization_ScheduleSetup.params["FromAdmin"] == "0") {
            if (Immunization_ScheduleSetup.params != null && Immunization_ScheduleSetup.params.ParentCtrl != null) {
                UnloadActionPan(Immunization_ScheduleSetup.params.ParentCtrl, 'Immunization_ScheduleSetup');
            }
            else
                UnloadActionPan(null, 'Immunization_ScheduleSetup');
        }
        else {

            RemoveAdminTab();
        }
        return objDeffered;
    },
    ScheduleSetupActiveInactive: function (ScheduleTypeId, IsActive) {
        AppPrivileges.GetFormPrivileges("Immunization_Schedule Setup", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = ScheduleTypeId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        var selectedSchTypeId = $("#" + Immunization_ScheduleSetup.params.PanelID + " #frmImmunizationScheduleSetup #ddlScheduleType option:selected").val();
                        Immunization_ScheduleSetup.UpdateScheduleSetupActiveInactive(selectedValue, IsActive).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Immunization_ScheduleSetup.LoadSchedlerAndActiveTabData(Immunization_ScheduleSetup.ActiveTab(), true, $('#ulSocialHxTabsItems li.active'));
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                    '3', null, null, null, IsActive
                );
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    UpdateScheduleSetupActiveInactive: function (VaccineScheduleId, IsActive) {
        var objData = new Object();
        objData["VaccineScheduleId"] = VaccineScheduleId;
        objData["IsActive"] = IsActive;
        objData["commandType"] = "activeinactive_schedulesetup";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ImmunizationScheduleSetup");
    },
    ScheduleSetupDelete: function (catID) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Immunization_Schedule Setup", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = catID;
                    var oTable = $('#dgvScheduleSetup').DataTable();
                    var ind = $(this).index();
                    var idx = oTable.row(this).index();
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        var selectedSchTypeId = $("#" + Immunization_ScheduleSetup.params.PanelID + " #frmImmunizationScheduleSetup #ddlScheduleType option:selected").val();
                        Immunization_ScheduleSetup.DeleteScheduleSetup(selectedValue).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                Immunization_ScheduleSetup.LoadSchedlerAndActiveTabData(Immunization_ScheduleSetup.ActiveTab(), true, $('#ulSocialHxTabsItems li.active'));
                                utility.DisplayMessages(response.Message, 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () {

                },
                    '1'
                );

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    DeleteScheduleSetup: function (VaccineScheduleId) {
        var objData = new Object();
        objData["VaccineScheduleId"] = VaccineScheduleId;
        objData["commandType"] = "delete_schedulesetup";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ImmunizationScheduleSetup");
    },
    ScheduleSetupEdit: function (catId) {
        Immunization_ScheduleSetup.SearchScheduleSetup(catId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $("#" + Immunization_ScheduleSetup.params.PanelID + " #txtScheduleSetup").val(JSON.parse(response.ScheduleSetupLoad_JSON)[0].ShortName);
                $("#" + Immunization_ScheduleSetup.params.PanelID + " #ddlActive").val(JSON.parse(response.ScheduleSetupLoad_JSON)[0].IsActive == "True" ? 1 : 0);
                Immunization_ScheduleSetup.params.VaccineGroupId = JSON.parse(response.ScheduleSetupLoad_JSON)[0].VaccineGroupID;
                Immunization_ScheduleSetup.params.mode = "Edit";
            }
        });
    },
    ImmunizationScheduleSetupSave: function () {
        var strMessage = "";
        var self = $("#" + Immunization_ScheduleSetup.params.PanelID + " #frmImmunizationScheduleSetup");
        var myJSON = self.getMyJSONByName();
        if (Immunization_ScheduleSetup.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Immunization_Schedule Setup", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    var selectedSchTypeId = $("#" + Immunization_ScheduleSetup.params.PanelID + " #frmImmunizationScheduleSetup #ddlScheduleType option:selected").val();
                    Immunization_ScheduleSetup.SaveImmunizationScheduleSetup(myJSON).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            if ($('#PatientProfile #hfPatientId').val() != "") {
                                $.when(Immunization_AlertConfiguration.OnlySetImmunizationAlertCount($('#PatientProfile #hfPatientId').val())).then(function () {
                                    var PanelGrid = "#pnlImmunization_ScheduleSetup #pnlScheduleSetup_Result";
                                    var GridId = "#pnlImmunization_ScheduleSetup #dgvScheduleSetup";
                                    utility.MakeEditableGrid(PanelGrid, GridId, Immunization_ScheduleSetup);
                                    utility.DisplayMessages(response.message, 1);
                                    Immunization_ScheduleSetup.LoadSchedlerAndActiveTabData(Immunization_ScheduleSetup.ActiveTab(), true, $('#ulSocialHxTabsItems li.active'));
                                    utility.ClearFormValidation('#' + Immunization_ScheduleSetup.params.PanelID + ' #frmImmunizationScheduleSetup');
                                    Immunization_ScheduleSetup.ResetControlls();
                                });
                            }
                            else {
                                var PanelGrid = "#pnlImmunization_ScheduleSetup #pnlScheduleSetup_Result";
                                var GridId = "#pnlImmunization_ScheduleSetup #dgvScheduleSetup";
                                utility.MakeEditableGrid(PanelGrid, GridId, Immunization_ScheduleSetup);
                                utility.DisplayMessages(response.message, 1);
                                Immunization_ScheduleSetup.LoadSchedlerAndActiveTabData(Immunization_ScheduleSetup.ActiveTab(), true, $('#ulSocialHxTabsItems li.active'));
                                utility.ClearFormValidation('#' + Immunization_ScheduleSetup.params.PanelID + ' #frmImmunizationScheduleSetup');
                                Immunization_ScheduleSetup.ResetControlls();
                            }
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                } else {
                    utility.DisplayMessages(strMessage, 2);
                }
            });
        }
        else if (Immunization_ScheduleSetup.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Immunization_Schedule Setup", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    var selectedSchTypeId = $("#" + Immunization_ScheduleSetup.params.PanelID + " #frmImmunizationScheduleSetup #ddlScheduleType option:selected").val();
                    Immunization_ScheduleSetup.UpdateImmunizationScheduleSetup(myJSON, Immunization_ScheduleSetup.params.VaccineGroupId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            if ($('#PatientProfile #hfPatientId').val() != "") {
                                $.when(Immunization_AlertConfiguration.OnlySetImmunizationAlertCount($('#PatientProfile #hfPatientId').val())).then(function () {
                                    utility.DisplayMessages(response.message, 1);
                                    Immunization_ScheduleSetup.LoadSchedlerAndActiveTabData(Immunization_ScheduleSetup.ActiveTab(), true, $('#ulSocialHxTabsItems li.active'));
                                    Immunization_ScheduleSetup.params.mode = "Add";
                                    utility.ClearFormValidation('#' + Immunization_ScheduleSetup.params.PanelID + ' #frmImmunizationScheduleSetup');
                                    Immunization_ScheduleSetup.ResetControlls();
                                });
                            }
                            else {
                                utility.DisplayMessages(response.message, 1);
                                Immunization_ScheduleSetup.LoadSchedlerAndActiveTabData(Immunization_ScheduleSetup.ActiveTab(), true, $('#ulSocialHxTabsItems li.active'));
                                Immunization_ScheduleSetup.params.mode = "Add";
                                utility.ClearFormValidation('#' + Immunization_ScheduleSetup.params.PanelID + ' #frmImmunizationScheduleSetup');
                                Immunization_ScheduleSetup.ResetControlls();

                            }
                        }
                        else {
                            utility.DisplayMessages(response.message, 3);
                        }
                    });
                } else {
                    utility.DisplayMessages(strMessage, 2);
                }
            });
        }
    },
    ResetControlls: function () {

        $("#" + Immunization_ScheduleSetup.params.PanelID + " #txtStartDueDate").val("");
        $("#" + Immunization_ScheduleSetup.params.PanelID + " #txtEndOverDueDate").val("");
        $("#" + Immunization_ScheduleSetup.params.PanelID + " #txtMaxage").val("");

    },

    SaveImmunizationScheduleSetup: function (ImmunizationScheduleSetupData) {


        var objData = JSON.parse(ImmunizationScheduleSetupData);
        objData["commandType"] = "save_immunizationschedulesetup";
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ImmunizationScheduleSetup");
    },
    UpdateImmunizationScheduleSetup: function (ImmunizationScheduleSetupData, VaccineScheduleId) {
        var objData = JSON.parse(ImmunizationScheduleSetupData);
        objData["VaccineScheduleId"] = VaccineScheduleId;
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["commandType"] = "Update_ImmunizationScheduleSetup";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ImmunizationScheduleSetup");
    },
    IdentifyEntity: function () {
        if (globalAppdata['AppUserName'] == DefaultUser) {
            Immunization_ScheduleSetup.params.EntityID = $("#pnlImmunization_ScheduleSetup #ddlEntity").val();
        }
        else {
            Immunization_ScheduleSetup.params.EntityID = globalAppdata["SeletedEntityId"];
        }
    },
    ValidateScheduleSetup: function () {
        $('#' + Immunization_ScheduleSetup.params.PanelID + ' #frmImmunizationScheduleSetup')
              .bootstrapValidator({
                  live: 'disabled',
                  message: 'This value is not valid',
                  feedbackIcons: {
                      valid: 'glyphicon glyphicon-ok',
                      invalid: 'glyphicon glyphicon-remove',
                      validating: 'glyphicon glyphicon-refresh'
                  },
                  fields: {

                      ScheduleTypeId: {
                          group: '.col-sm-4',
                          validators: {
                              notEmpty: {
                                  message: ''
                              }
                          }
                      },
                      ScheduleId: {
                          group: '.col-sm-4',
                          validators: {
                              notEmpty: {
                                  message: ''
                              }
                          }
                      },
                      VaccineGroupID: {
                          group: '.col-sm-4',
                          validators: {
                              notEmpty: {
                                  message: ''
                              }
                          }
                      },

                  }
              }).on('success.form.bv', function (e) {

                  e.preventDefault();
                  Immunization_ScheduleSetup.ImmunizationScheduleSetupSave();

              });



    },
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

                if ($this.hasClass('actions')) {

                    return _self.datatable.cell(this).data();
                } else {
                    $obj_ = $this.find('input');
                    return $.trim($obj_.val());
                }
            });
            var id = $row.attr("id");
            var myJSON = $row.getMyJSONByName();
            var objData = JSON.parse(myJSON);
            var ScheduleTypeId = $row.attr("scheduletypeid");
            objData["ScheduleTypeId"] = ScheduleTypeId
            myJSON = JSON.stringify(objData);

            if (id && id > 0) {
                var strMessage = "";
                AppPrivileges.GetFormPrivileges("Immunization_Schedule Setup", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        var selectedSchTypeId = $("#" + Immunization_ScheduleSetup.params.PanelID + " #frmImmunizationScheduleSetup #ddlScheduleType option:selected").val();
                        Immunization_ScheduleSetup.UpdateImmunizationScheduleSetup(myJSON, id).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                //Immunization_ScheduleSetup.LoadSchedlerData(selectedSchTypeId, Immunization_ScheduleSetup.ActiveTab());
                                if ($('#PatientProfile #hfPatientId').val() != "") {
                                    $.when(Immunization_AlertConfiguration.OnlySetImmunizationAlertCount($('#PatientProfile #hfPatientId').val())).then(function () {
                                        Immunization_ScheduleSetup.LoadSchedlerAndActiveTabData(Immunization_ScheduleSetup.ActiveTab(), true, $('#ulSocialHxTabsItems li.active'));
                                    });
                                }
                                else {
                                    Immunization_ScheduleSetup.LoadSchedlerAndActiveTabData(Immunization_ScheduleSetup.ActiveTab(), true, $('#ulSocialHxTabsItems li.active'));
                                }
                            }
                            else {
                                utility.DisplayMessages(response.message, 3);
                            }
                        });
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
            }
        }
    },
    rowRemove: function ($row, obj) {
        var strMessage = "";
        var catID = $row.attr("id");
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Immunization_Schedule Setup", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = catID;
                    var oTable = $('#dgvScheduleSetup').DataTable();
                    var ind = $(this).index();
                    var idx = oTable.row(this).index();
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        var selectedSchTypeId = $("#" + Immunization_ScheduleSetup.params.PanelID + " #frmImmunizationScheduleSetup #ddlScheduleType option:selected").val();
                        Immunization_ScheduleSetup.DeleteScheduleSetup(selectedValue).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                Immunization_ScheduleSetup.LoadSchedlerAndActiveTabData(Immunization_ScheduleSetup.ActiveTab(), true, $('#ulSocialHxTabsItems li.active'));;
                                utility.DisplayMessages(response.Message, 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () {

                },
                    '1'
                );

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },
    rowInactive: function ($row, obj) {
        var strMessage = "";
        var id = $row.attr("id");
        var IsActive = $row.attr("isActive");
        AppPrivileges.GetFormPrivileges("Immunization_Schedule Setup", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = id;
                
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        var selectedSchTypeId = $("#" + Immunization_ScheduleSetup.params.PanelID + " #frmImmunizationScheduleSetup #ddlScheduleType option:selected").val();
                        Immunization_ScheduleSetup.UpdateScheduleSetupActiveInactive(selectedValue, IsActive).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Immunization_ScheduleSetup.LoadSchedlerAndActiveTabData(Immunization_ScheduleSetup.ActiveTab(), true, $('#ulSocialHxTabsItems li.active'));
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
    rowDetail: function ($row, ClassName) {
        var currentProblemListId = $($row).attr("id") != null ? $($row).attr("id") : -1;
    },
    PopulateDropDown: function (vaccineGroupCategoryId, controlId, forModule, Method) {
        var dfd = new $.Deferred();
        var data = "";
        if (vaccineGroupCategoryId !== "") {
            data = "StrID=" + forModule;
            Immunization_ScheduleSetup.LoadingDropDowns(true, " #" + controlId, Method, data).done(function () {
                dfd.resolve();
            });
        } else {
            Immunization_ScheduleSetup.RemoveDropDownItems(controlId);
        }
        return dfd;
    },
    RemoveDropDownItems: function (ddlId) {
        $(" #" + ddlId).children().remove();
    },
    LoadingDropDowns: function (isLoad, controlId, methodName, data) {
        var dfd = new $.Deferred();
        var contrainerid = "pnlImmunization_ScheduleSetup";
        if (data == null || data == "undefined") {
            data = "";
        }
        var ddl = " " + controlId + " ";
        var getDataMethod = methodName;
        if (true) {
            return MDVisionService.lookups(getDataMethod, isLoad, data).done(function (results) {
                results = JSON.parse(results[getDataMethod]);
                if (results) {
                    if (methodName == "GetAdministerVaccine_Category") {
                        Immunization_ScheduleSetup.LoadCategoryDropdwonlist(results);
                    }
                    else {
                        BackgroundLoaderShow(false);
                    }
                    dfd.resolve();
                }
            });
        }
        return dfd.promise();
    },
    LoadCategoryDropdwonlist: function (results) {
        var ddl = $("#" + Immunization_ScheduleSetup.params.PanelID + " #frmImmunizationScheduleSetup select[id^='ddlCategory']");
        for (var i = 0; i < ddl.length; i++) {
            var l = $(ddl[i]);
            l.empty();
            $.each(results, function (j, result) {
                l.append($("<option />").val(result.Value).text(result.Name).attr("RefValue", result.RefValue).attr("RefName", result.RefName));
            });
            BackgroundLoaderShow(false);
        }
    },
    LoadSchedlerAndActiveTabData: function (TabId, ComeFromHtml, $obj) {
        if (TabId != "") {
            $('#' + Immunization_ScheduleSetup.params.PanelID).find('[id*="list"]').removeClass('active');
            $('#' + Immunization_ScheduleSetup.params.PanelID).find('[id*="tab_"]').removeClass('active');
            $('#' + Immunization_ScheduleSetup.params.PanelID + ' #list' + TabId).addClass('active');
            $('#' + Immunization_ScheduleSetup.params.PanelID + ' #tab_' + TabId).addClass('active');
        }
        if (ComeFromHtml) {
            Immunization_ScheduleSetup.LoadSchedlerData($($obj).attr("scheduletypeid"), TabId);
        }

    },
    LoadSchedlerData: function (scheduletypeid, TabId) {
        var dfd = $.Deferred();
        Immunization_ScheduleSetup.SearchSchedlerData_DBCall(scheduletypeid).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $.when(Immunization_ScheduleSetup.ScheduleSetupGridLoad(response, TabId)).then(function () {
                    dfd.resolve();
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
                dfd.resolve();
            }
        });
        return dfd;
    },
    SearchSchedlerData_DBCall: function (TabId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = {};
        if (TabId)
            objData["ScheduleTypeId"] = TabId;
        else
            objData["ScheduleTypeId"] = $("#" + Immunization_ScheduleSetup.params.PanelID + " #frmImmunizationScheduleSetup #ddlScheduleTypeSearch option:selected").val();
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["IsActive"] = $('#' + Immunization_ScheduleSetup.params.PanelID + ' #frmImmunizationScheduleSetup #divSwitch #switchActive').is(':checked');
        objData["VaccineGroupID"] = $("#" + Immunization_ScheduleSetup.params.PanelID + " #frmImmunizationScheduleSetup #ddlCategorySearch option:selected").val();
        objData["ScheduleId"] = $("#" + Immunization_ScheduleSetup.params.PanelID + " #frmImmunizationScheduleSetup #ddlScheduleSearch option:selected").val();
        objData["commandType"] = "get_all_schedulesetup";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "ImmunizationScheduleSetup");
    },
    ScheduleSetupGridLoad: function (response, TabId) {
        var dfd = $.Deferred();
        $.when(Immunization_ScheduleSetup.setGridIds(TabId)).then(function () {
            var PanelGrid = "#" + Immunization_ScheduleSetup.params.PanelID + " #" + Immunization_ScheduleSetup.GridResultId;
            var GridId = "#" + Immunization_ScheduleSetup.params.PanelID + " #" + Immunization_ScheduleSetup.GriddgvId;
            if (Immunization_ScheduleSetup.myGrid != null) {
                if ($.fn.dataTable.isDataTable(Immunization_ScheduleSetup.myGrid)) {
                    Immunization_ScheduleSetup.myGrid.$table.dataTable().fnDestroy();
                    Immunization_ScheduleSetup.myGrid.$table.DataTable().clear().draw();
                } else {
                    Immunization_ScheduleSetup.myGrid = null;
                }
            }
            if ($.fn.dataTable.isDataTable("#" + Immunization_ScheduleSetup.params.PanelID + " #" + Immunization_ScheduleSetup.GridResultId + " #" + Immunization_ScheduleSetup.GriddgvId)) {
                $("#" + Immunization_ScheduleSetup.params.PanelID + " #" + Immunization_ScheduleSetup.GridResultId + " #" + Immunization_ScheduleSetup.GriddgvId).dataTable().fnDestroy();
            }
            $("#" + Immunization_ScheduleSetup.params.PanelID + ' #' + Immunization_ScheduleSetup.GriddgvId + ' tbody').find("tr").remove();
            if (response.ScheduleSetupCount > 0) {
                var ScheduleSetupLoad_JSON = JSON.parse(response.ScheduleSetupLoad_JSON);
                var actions = "";
                $("#" + Immunization_ScheduleSetup.params.PanelID + " #" + Immunization_ScheduleSetup.GriddgvId + " tr th").each(function () {
                    if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                        var arrActionType = [];
                        if ($(this).attr("ActionType") != null) {
                            arrActionType = $(this).attr("ActionType").split(',');
                            actions = EMREditableGrid.GetActions(arrActionType, "#" + Immunization_ScheduleSetup.params.PanelID + " #pnlScheduleSetup_Result");
                        }
                    }
                });
                $.each(ScheduleSetupLoad_JSON, function (i, item) {
                    var $row = $('<tr/>');
                    var isactive = "";
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
                    $row.attr("IsActive", isactive);
                    $row.attr("id", item.VaccineScheduleId);
                    $row.attr("scheduletypeid", item.ScheduleTypeId);
                    $row.append('<td class="actions" id="' + item.VaccineScheduleId + '" >' + actions + '</td><td>' + item.Schedule + '</td><td>' + item.Category + '</td><td>' + item.StartDueDate + '</td><td>' + item.EndOverDueDate + '</td><td>' + item.MaleMaxAge + '</td><td style="display:none;">' + item.ScheduleTypeId + '</td>');
                    $("#" + Immunization_ScheduleSetup.params.PanelID + ' #' + Immunization_ScheduleSetup.GriddgvId + ' tbody').last().append($row);
                });
                var PanelGrid = "#" + Immunization_ScheduleSetup.params.PanelID + " #" + Immunization_ScheduleSetup.GridResultId;
                var GridId = "#" + Immunization_ScheduleSetup.params.PanelID + " #" + Immunization_ScheduleSetup.GriddgvId;
                Immunization_ScheduleSetup.myGrid = EMRUtility.MakeEditableGrid(PanelGrid, GridId, Immunization_ScheduleSetup, 0, false, true, false, true, false, null);
            }
            if ($.fn.dataTable.isDataTable("#" + Immunization_ScheduleSetup.params.PanelID + " #" + Immunization_ScheduleSetup.GridResultId + " #" + Immunization_ScheduleSetup.GriddgvId))
                ;
            else
                $("#" + Immunization_ScheduleSetup.params.PanelID + " #" + Immunization_ScheduleSetup.GridResultId + " #" + Immunization_ScheduleSetup.GriddgvId).DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
            EMRUtility.SwicthWidgetInializatoin();
            dfd.resolve();
        });
        return dfd;
    },
    setGridIds: function (TabId) {
        if (TabId == "Birth_2_years") {
            Immunization_ScheduleSetup.GridResultId = "pnlScheduleSetup_Birth_2_years_Result";
            Immunization_ScheduleSetup.GriddgvId = "dgvScheduleSetup_Birth_2_years";
        }
        else if (TabId == "2_18_years") {
            Immunization_ScheduleSetup.GridResultId = "pnlScheduleSetup_2_18_years_Result";
            Immunization_ScheduleSetup.GriddgvId = "dgvScheduleSetup_2_18_years";
        }
        else if (TabId == "Adult") {
            Immunization_ScheduleSetup.GridResultId = "pnlScheduleSetup_Adult_Result";
            Immunization_ScheduleSetup.GriddgvId = "dgvScheduleSetup_Adult";
        }
        else if (TabId == "Recurring") {
            Immunization_ScheduleSetup.GridResultId = "pnlScheduleSetup_Recurrinng_Result";
            Immunization_ScheduleSetup.GriddgvId = "dgvScheduleSetup_Recurring";
        }
    },
    ActiveTab: function () {
        var TabId = "Birth_2_years";
        if ($('#' + Immunization_ScheduleSetup.params.PanelID + ' #ulSocialHxTabsItems li.active').attr('id'))
            TabId = $('#' + Immunization_ScheduleSetup.params.PanelID + ' #ulSocialHxTabsItems li.active').attr('id').replace('list', '');
        return TabId;
    },
    BindSchedule: function ($obj, ddl) {
        var ScheduleTypeid = $($obj).val();
        CacheManager.BindDropDownsByID("#" + Immunization_ScheduleSetup.params["PanelID"] + ' #' + ddl, 'GetImmunizationSchedule', true, ScheduleTypeid);
    },
    ActiveteSelectedTypeId: function () {
        var TabId = $("#" + Immunization_ScheduleSetup.params["PanelID"] + " #ddlScheduleTypeSearch option:selected").val();
        if (TabId != "") {
            $('#' + Immunization_ScheduleSetup.params.PanelID).find('[id*="list"]').removeClass('active');
            $('#' + Immunization_ScheduleSetup.params.PanelID).find('[id*="tab_"]').removeClass('active');
            $('#' + Immunization_ScheduleSetup.params.PanelID).find('[scheduletypeid*="' + TabId + '"]').addClass('active');
            /*$('#' + Immunization_ScheduleSetup.params.PanelID + ' #list' + TabId).addClass('active');
            $('#' + Immunization_ScheduleSetup.params.PanelID + ' #tab_' + TabId).addClass('active');*/
        }
    },
    GetGridByTabId: function (TabId) {
        var selectedGrid = Immunization_ScheduleSetup.myGridBirth_2_years;
        if (TabId == "Birth_2_years") {
            selectedGrid = Immunization_ScheduleSetup.myGrid2_18_years;
        }
        else if (TabId == "2_18_years") {
            selectedGrid = Immunization_ScheduleSetup.myGridBirth_2_years;
        }
        else if (TabId == "Adult") {
            selectedGrid = Immunization_ScheduleSetup.myGridAdult;
        }
        else if (TabId == "Recurring") {
            selectedGrid = Immunization_ScheduleSetup.myGridRecurring;
        }
        return selectedGrid;
    },

    rowDown: function ($row) {

        AppPrivileges.GetFormPrivileges("Immunization_Schedule Setup", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var from_Id = $row.attr("Id");
                var to_Id = $row.next('tr').attr("Id");

                if (from_Id != undefined && to_Id != undefined) {
                    Immunization_ScheduleSetup.SetPriority(from_Id, to_Id).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {

                            //utility.DisplayMessages(response.Message, 1);

                            // Draw row on grid
                            Immunization_ScheduleSetup.rowSwap($row.get(0), $row.next().get(0));
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

    },

    rowUp: function ($row) {

        AppPrivileges.GetFormPrivileges("Immunization_Schedule Setup", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {


                var from_Id = $row.attr("Id");
                var to_Id = $row.prev('tr').attr("Id");

                if (from_Id != undefined && to_Id != undefined) {

                    Immunization_ScheduleSetup.SetPriority(from_Id, to_Id).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {

                            //utility.DisplayMessages(response.Message, 1);

                            // Draw row on grid
                            Immunization_ScheduleSetup.rowSwap($row.prev().get(0), $row.get(0));

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

    },
    SetPriority: function (from_Id, to_Id) {
        var objData = {};
        objData.fromId = from_Id;
        objData.toId = to_Id;
        objData["commandType"] = "SetPriority";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ImmunizationScheduleSetup");
    },
    rowSwap: function (elm1, elm2) {

        var parent1, next1,
            parent2, next2;

        parent1 = elm1.parentNode;
        next1 = elm1.nextSibling;
        parent2 = elm2.parentNode;
        next2 = elm2.nextSibling;

        parent1.insertBefore(elm2, next1);
        parent2.insertBefore(elm1, next2);

    },
    rowTitle: function ($row) {

        $row.children('td').each(function (i) {

            var $this = $(this);

            if ($this.hasClass('actions')) {

            }
            else if ($this.hasClass('code')) {

                if ($this.css("display") != "none") {
                    $row.addClass('r_title');
                    $this.css("display", "none");
                }
                else {
                    $row.removeClass('r_title');
                    $this.css("display", "block");
                }

            }
            else {

                if (i == 3) {
                    //$row.hasClass('r_title')
                    if ($this.css("display") != "none") {
                        $this.css("display", "none");
                    }
                    else {
                        $this.css("display", "block");
                    }
                }

                if ($row.hasClass('r_title'))
                    $this.attr("colspan", "4");
                else
                    $this.attr("colspan", "0");
            }
        });

    },

}