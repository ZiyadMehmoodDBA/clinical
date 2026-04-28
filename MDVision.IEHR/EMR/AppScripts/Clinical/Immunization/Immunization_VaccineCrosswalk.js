Immunization_VaccineCrosswalk = {
    bIsFirstLoad: true,
    params: [],
    myGrid: null,
    AllVaccine: [],
    AllSearchedVaccine: [],
    AllSearchedForGridVaccine: [],
    Load: function (params) {
        Immunization_VaccineCrosswalk.params = params;
        Immunization_VaccineCrosswalk.AllVaccine = [];
        Immunization_VaccineCrosswalk.AllSearchedVaccine = [];
        Immunization_VaccineCrosswalk.AllSearchedForGridVaccine = [];
        Immunization_VaccineCrosswalk.params.mode = "Add";
        if (Immunization_VaccineCrosswalk.params.PanelID != 'pnlImmunization_VaccineCrosswalk') {
            Immunization_VaccineCrosswalk.params.PanelID = Immunization_VaccineCrosswalk.params.PanelID + ' #pnlImmunization_VaccineCrosswalk';
        } else {
            Immunization_VaccineCrosswalk.params.PanelID = 'pnlImmunization_VaccineCrosswalk';
        }
        var self = $('#' + Immunization_VaccineCrosswalk.params.PanelID);
        Immunization_VaccineCrosswalk.PopulateDropDown("1", "frmImmunizationVaccineCrosswalk #ddlCategory", "1", "GetAdministerVaccine_Category");
        if (Immunization_VaccineCrosswalk.bIsFirstLoad == true) {
            Immunization_VaccineCrosswalk.BindVaccineByCategory(null, '#lnkVaccineSearch', '#frmImmunizationVaccineCrosswalk #txtVaccine', '#frmImmunizationVaccineCrosswalk #hfVaccineId', '#frmImmunizationVaccineCrosswalk #lnkVaccine', '#frmImmunizationVaccineCrosswalk #lblVaccine');
            Immunization_VaccineCrosswalk.BindVaccineByCategory(null, '#lnkVaccine_Search', 'frmImmunizationVaccineCrosswalk #txtVaccineSearch', 'frmImmunizationVaccineCrosswalk #hfVaccineIdSearch');
            Immunization_VaccineCrosswalk.VaccineCrosswalkSearch();
            Immunization_VaccineCrosswalk.ValidateVaccineCrosswalk();
            Immunization_VaccineCrosswalk.bIsFirstLoad = false;
        }
        else {
            self.loadDropDowns(true).done(function () {
                Immunization_VaccineCrosswalk.BindVaccineByCategory(null, '#lnkVaccineSearch', '#frmImmunizationVaccineCrosswalk #txtVaccine', '#frmImmunizationVaccineCrosswalk #hfVaccineId', '#frmImmunizationVaccineCrosswalk #lnkVaccine', '#frmImmunizationVaccineCrosswalk #lblVaccine');
                Immunization_VaccineCrosswalk.BindVaccineByCategory(null, '#lnkVaccine_Search', 'frmImmunizationVaccineCrosswalk #txtVaccineSearch', 'frmImmunizationVaccineCrosswalk #hfVaccineIdSearch');
                Immunization_VaccineCrosswalk.VaccineCrosswalkSearch();
            });
        }
    },
    VaccineCrosswalkSearch: function (VaccineCrosswalkId, PageNo, rpp) {
        var strMessage = "";
        if ($("#" + Immunization_VaccineCrosswalk.params.PanelID + " #pnlVaccineCrosswalk_Result").css("display") == "none") {
            $("#" + Immunization_VaccineCrosswalk.params.PanelID + " #pnlVaccineCrosswalk_Result").show();
        }
        AppPrivileges.GetFormPrivileges("Immunization_Vaccine Crosswalk", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Immunization_VaccineCrosswalk.SearchVaccineCrosswalk(VaccineCrosswalkId, PageNo, rpp).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        Immunization_VaccineCrosswalk.VaccineCrosswalkGridLoadNew(response);
                        var TableControl = Immunization_VaccineCrosswalk.params.PanelID + " #dgvVaccineCrosswalk";
                        var PagingPanelControlID = Immunization_VaccineCrosswalk.params.PanelID + " #divVaccineCrosswalklistPaging";
                        var ClassControlName = "Immunization_VaccineCrosswalk";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.VaccineCrosswalkCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Immunization_VaccineCrosswalk.VaccineCrosswalkSearch(PrimaryID, PageNumber, ResultPerPage);
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
    SearchVaccineCrosswalk: function (VaccineCrosswalkId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = new Object();
        objData["VaccineCrosswalkID"] = VaccineCrosswalkId;
        objData["VaccineGroupID"] = $("#" + Immunization_VaccineCrosswalk.params.PanelID + " #frmImmunizationVaccineCrosswalk #ddlCategorySearch").val();
        objData["IsActive"] = $('#' + Immunization_VaccineCrosswalk.params.PanelID + ' #frmImmunizationVaccineCrosswalk #divSwitch #switchActive').is(':checked');
        objData["VaccineId"] = $("#" + Immunization_VaccineCrosswalk.params.PanelID + " #frmImmunizationVaccineCrosswalk #hfVaccineIdSearch").val();
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        if (globalAppdata['AppUserName'] != DefaultUser) {
            objData["EntityId"] = globalAppdata["SeletedEntityId"];
        } else {
            objData["EntityId"] = null;
        }
        objData["commandType"] = "Get_All_VaccineCrosswalks";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ImmunizationVaccineCrosswalk");
    },
    VaccineCrosswalkGridLoadNew: function (response, PageNo, rpp) {
        $("#pnlImmunization_VaccineCrosswalk #dgvVaccineCrosswalk").dataTable().fnDestroy();
        $("#pnlImmunization_VaccineCrosswalk #pnlVaccineCrosswalk_Result #dgvVaccineCrosswalk tbody").find("tr").remove();
        if (response.VaccineCrosswalkCount > 0) {
            var VaccineCrosswalkLoadJSONData = JSON.parse(response.VaccineCrosswalkLoad_JSON);
            var actions = "";
            $("#" + Immunization_VaccineCrosswalk.params.PanelID + " #dgvVaccineCrosswalk tr th").each(function () {
                if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                    var arrActionType = [];
                    if ($(this).attr("ActionType") != null) {
                        arrActionType = $(this).attr("ActionType").split(',');
                        actions = EMREditableGrid.GetActions(arrActionType, "#" + Immunization_VaccineCrosswalk.params.PanelID + " #pnlVaccineCrosswalk_Result");
                    }
                }
            });
            $.each(VaccineCrosswalkLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", item.VaccineCrosswalkId);
                $row.attr("onclick", "utility.SelectGridRow($(this))");
                $row.attr("VaccineCrosswalkId", item.VaccineCrosswalkId);
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
                $row.append('<td class="actions" id="' + item.VaccineCrosswalkId + '" >' + actions + '</td><td VaccineGroupId=' + item.VaccineGroupId + '>' + item.CategoryName + '</td><td VaccineId=' + item.VaccineId + '>' + item.CVXShortDescription + '</td><td>' + item.CVXCode + '</td><td>' + item.CPTCode + '</td><td>' + (item.IsDefault == "True" ? "Yes" : "No") + '</td>');
                $('#' + Immunization_VaccineCrosswalk.params["PanelID"] + " #pnlVaccineCrosswalk_Result #dgvVaccineCrosswalk tbody").last().append($row);
            });
            var PanelGrid = "#" + Immunization_VaccineCrosswalk.params.PanelID + " #pnlVaccineCrosswalk_Result";
            var GridId = "#" + Immunization_VaccineCrosswalk.params.PanelID + " #dgvVaccineCrosswalk";

            if (Immunization_VaccineCrosswalk.myGrid != null) {
                if ($.fn.dataTable.isDataTable(Immunization_VaccineCrosswalk.myGrid)) {
                    Immunization_VaccineCrosswalk.myGrid.$table.dataTable().fnDestroy();
                } else {
                    Immunization_VaccineCrosswalk.myGrid = null;
                }
                if ($.fn.dataTable.isDataTable("#" + Immunization_VaccineCrosswalk.params.PanelID + " #pnlVaccineCrosswalk_Result #dgvVaccineCrosswalk")) {
                    $("#" + Immunization_VaccineCrosswalk.params.PanelID + " #pnlProblemLists_Result #dgvVaccineCrosswalk").dataTable().fnDestroy();
                }
            }
            Immunization_VaccineCrosswalk.myGrid = EMRUtility.MakeEditableGrid(PanelGrid, GridId, Immunization_VaccineCrosswalk, 0, false, true, false, true, false, null);
        }
        else {
            $('#pnlImmunization_VaccineCrosswalk #dgvVaccineCrosswalk').DataTable({
                "language": {
                    "emptyTable": "No Vaccine Crosswalk Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }


        if ($.fn.dataTable.isDataTable('#' + Immunization_VaccineCrosswalk.params["PanelID"] + ' #dgvVaccineCrosswalk'))
            ;
        else
            $('#' + Immunization_VaccineCrosswalk.params["PanelID"] + ' #pnlVaccineCrosswalk_Result #dgvVaccineCrosswalk').DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        EMRUtility.SwicthWidgetInializatoin();

    },
    UnLoadTab: function () {
        var objDeffered = $.Deferred();
        if (Immunization_VaccineCrosswalk.params["FromAdmin"] == "0") {
            if (Immunization_VaccineCrosswalk.params != null && Immunization_VaccineCrosswalk.params.ParentCtrl != null) {
                UnloadActionPan(Immunization_VaccineCrosswalk.params.ParentCtrl, 'Immunization_VaccineCrosswalk');
            }
            else
                UnloadActionPan(null, 'Immunization_VaccineCrosswalk');
        }
        else {

            RemoveAdminTab();
        }
        return objDeffered;
    },
    VaccineCrosswalkActiveInactive: function (VaccineCrosswalkId, IsActive) {
        AppPrivileges.GetFormPrivileges("Immunization_Vaccine Crosswalk", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = VaccineCrosswalkId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {

                        Immunization_VaccineCrosswalk.UpdateVaccineCrosswalkActiveInactive(selectedValue, IsActive).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Immunization_VaccineCrosswalk.VaccineCrosswalkSearch();
                                UnloadActionPan();
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
    UpdateVaccineCrosswalkActiveInactive: function (VaccineCrosswalkId, IsActive) {
        var objData = new Object();
        objData["VaccineCrosswalkID"] = VaccineCrosswalkId;
        objData["IsActive"] = IsActive;
        objData["commandType"] = "ActiveInactive_VaccineCrosswalk";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ImmunizationVaccineCrosswalk");
    },
    VaccineCrosswalkDelete: function (catID) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Immunization_Vaccine Crosswalk", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = catID;
                    var oTable = $('#dgvVaccineCrosswalk').DataTable();
                    var ind = $(this).index();
                    var idx = oTable.row(this).index();
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Immunization_VaccineCrosswalk.DeleteVaccineCrosswalk(selectedValue).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                Immunization_VaccineCrosswalk.VaccineCrosswalkSearch();
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
    DeleteVaccineCrosswalk: function (VaccineCrosswalkId) {
        var objData = new Object();
        objData["VaccineCrosswalkID"] = VaccineCrosswalkId;
        objData["commandType"] = "DELETE_VaccineCrosswalk";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ImmunizationVaccineCrosswalk");
    },
    VaccineCrosswalkEdit: function (catId) {
        Immunization_VaccineCrosswalk.SearchVaccineCrosswalk(catId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $("#" + Immunization_VaccineCrosswalk.params.PanelID + " #txtVaccineCrosswalk").val(JSON.parse(response.VaccineCrosswalkLoad_JSON)[0].ShortName);
                $("#" + Immunization_VaccineCrosswalk.params.PanelID + " #ddlActive").val(JSON.parse(response.VaccineCrosswalkLoad_JSON)[0].IsActive == "True" ? 1 : 0);
                Immunization_VaccineCrosswalk.params.VaccineGroupId = JSON.parse(response.VaccineCrosswalkLoad_JSON)[0].VaccineGroupID;
                Immunization_VaccineCrosswalk.params.mode = "Edit";
            }
        });
    },
    ImmunizationVaccineCrosswalkSave: function () {
        var strMessage = "";
        var self = $("#" + Immunization_VaccineCrosswalk.params.PanelID + " #frmImmunizationVaccineCrosswalk");
        var myJSON = self.getMyJSONByName();
        if (Immunization_VaccineCrosswalk.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Immunization_Vaccine Crosswalk", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Immunization_VaccineCrosswalk.SaveImmunizationVaccineCrosswalk(myJSON).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            $('#' + Immunization_VaccineCrosswalk.params["PanelID"] + ' #vaccineCrosswalkDetails').resetAllControls();
                            utility.ClearFormValidation('#' + Immunization_VaccineCrosswalk.params.PanelID + ' #frmImmunizationVaccineCrosswalk');
                            var PanelGrid = "#pnlImmunization_VaccineCrosswalk #pnlVaccineCrosswalk_Result";
                            var GridId = "#pnlImmunization_VaccineCrosswalk #dgvVaccineCrosswalk";
                            utility.MakeEditableGrid(PanelGrid, GridId, Immunization_VaccineCrosswalk);
                            utility.DisplayMessages(response.message, 1);
                            Immunization_VaccineCrosswalk.VaccineCrosswalkSearch();
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
        else if (Immunization_VaccineCrosswalk.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Immunization_Vaccine Crosswalk", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Immunization_VaccineCrosswalk.UpdateImmunizationVaccineCrosswalk(myJSON, Immunization_VaccineCrosswalk.params.VaccineGroupId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            $('#' + Immunization_VaccineCrosswalk.params["PanelID"] + ' #vaccineCrosswalkDetails').resetAllControls();
                            utility.ClearFormValidation('#' + Immunization_VaccineCrosswalk.params.PanelID + ' #frmImmunizationVaccineCrosswalk');
                            utility.DisplayMessages(response.message, 1);
                            Immunization_VaccineCrosswalk.VaccineCrosswalkSearch();
                            Immunization_VaccineCrosswalk.params.mode = "Add";
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
    SaveImmunizationVaccineCrosswalk: function (ImmunizationVaccineCrosswalkData) {
        var objData = JSON.parse(ImmunizationVaccineCrosswalkData);
        objData["commandType"] = "Save_ImmunizationVaccineCrosswalk";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ImmunizationVaccineCrosswalk");
    },
    UpdateImmunizationVaccineCrosswalk: function (ImmunizationVaccineCrosswalkData, VaccineCrosswalkId) {
        var objData = JSON.parse(ImmunizationVaccineCrosswalkData);
        objData["VaccineCrosswalkID"] = VaccineCrosswalkId;
        objData["commandType"] = "Update_ImmunizationVaccineCrosswalk";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ImmunizationVaccineCrosswalk");
    },
    IdentifyEntity: function () {
        if (globalAppdata['AppUserName'] == DefaultUser) {
            Immunization_VaccineCrosswalk.params.EntityID = $("#pnlImmunization_VaccineCrosswalk #ddlEntity").val();
        }
        else {
            Immunization_VaccineCrosswalk.params.EntityID = globalAppdata["SeletedEntityId"];
        }
    },
    ValidateVaccineCrosswalk: function () {
        $('#' + Immunization_VaccineCrosswalk.params.PanelID + ' #frmImmunizationVaccineCrosswalk')
              .bootstrapValidator({
                  live: 'disabled',
                  message: 'This value is not valid',
                  feedbackIcons: {
                      valid: 'glyphicon glyphicon-ok',
                      invalid: 'glyphicon glyphicon-remove',
                      validating: 'glyphicon glyphicon-refresh'
                  },
                  fields: {


                      VaccineGroupID: {
                          group: '.col-sm-4',
                          validators: {
                              notEmpty: {
                                  message: ''
                              }
                          }
                      },
                      VaccineName: {
                          group: '.col-sm-4',
                          validators: {
                              notEmpty: {
                                  message: ''
                              }
                          }
                      }
                  }
              })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            Immunization_VaccineCrosswalk.ImmunizationVaccineCrosswalkSave();
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

            myJSON = JSON.stringify(objData);

            if (id && id > 0) {
                var strMessage = "";
                AppPrivileges.GetFormPrivileges("Immunization_Vaccine Crosswalk", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        Immunization_VaccineCrosswalk.UpdateImmunizationVaccineCrosswalk(myJSON, id).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Immunization_VaccineCrosswalk.VaccineCrosswalkSearch();
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
        AppPrivileges.GetFormPrivileges("Immunization_Vaccine Crosswalk", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = catID;
                    var oTable = $('#dgvVaccineCrosswalk').DataTable();
                    var ind = $(this).index();
                    var idx = oTable.row(this).index();
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Immunization_VaccineCrosswalk.DeleteVaccineCrosswalk(selectedValue).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                Immunization_VaccineCrosswalk.VaccineCrosswalkSearch();
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
        AppPrivileges.GetFormPrivileges("Immunization_Vaccine Crosswalk", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = id;

                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Immunization_VaccineCrosswalk.UpdateVaccineCrosswalkActiveInactive(selectedValue, IsActive).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Immunization_VaccineCrosswalk.VaccineCrosswalkSearch();
                                UnloadActionPan();
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
            if (controlId && controlId.indexOf("ddlVaccine") > -1) {
                data = "StrID=" + vaccineGroupCategoryId + "&StrID2=" + forModule;
            }
            else {
                data = "StrID=" + forModule;
            }
            Immunization_VaccineCrosswalk.LoadingDropDowns(true, " #" + controlId, Method, data).done(function () {
                dfd.resolve();
            });
        } else {
            Immunization_VaccineCrosswalk.RemoveDropDownItems(controlId);
        }
        return dfd;
    },
    RemoveDropDownItems: function (ddlId) {
        $(" #" + ddlId).children().remove();
    },
    LoadingDropDowns: function (isLoad, controlId, methodName, data) {
        var dfd = new $.Deferred();
        var contrainerid = "pnlImmunization_VaccineCrosswalk";
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
                        Immunization_VaccineCrosswalk.LoadCategoryDropdwonlist(results);
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
        var ddl = $("#" + Immunization_VaccineCrosswalk.params.PanelID + " #frmImmunizationVaccineCrosswalk select[id^='ddlCategory']");
        for (var i = 0; i < ddl.length; i++) {
            var l = $(ddl[i]);
            l.empty();
            $.each(results, function (j, result) {
                l.append($("<option />").val(result.Value).text(result.Name).attr("RefValue", result.RefValue).attr("RefName", result.RefName));
            });
            BackgroundLoaderShow(false);
        }
    },
    OpenSearchPopup: function (IsFromGrid, $obj, ddlCategory, Ctrl, HiddenCtrl) {
        var params = [];
        var VaccineGroupID = "0";
        var RefCPTCtrl = null;
        var RefCVXCtrl = null;
        params["FromAdmin"] = "0";
        if (IsFromGrid && IsFromGrid == 1) {
            var row = $($obj).closest('tr');
            $($obj).val('');
            VaccineGroupID = ($(row).find("select[id^='ddlCategory']")).val() == "" ? "0" : $(row).find("select[id^='ddlCategory']").val();
            RefCPTCtrl = $($(row).find("input[id^='txtCPTCode']")).attr("id");
            RefCVXCtrl = $($(row).find("input[id^='txtCVXCode']")).attr("id");
            Ctrl = $($(row).find("input[id^='txtVaccine']")).attr("id");
            HiddenCtrl = $(row).find("input[id^='hfVaccineId']").attr("id");
        } else if (IsFromGrid && IsFromGrid == 0) {
            VaccineGroupID = $(ddlCategory).val();
            $("#txtVaccine").val('');
            RefCPTCtrl = "frmImmunizationVaccineCrosswalk #txtCPTCode";
            RefCVXCtrl = "frmImmunizationVaccineCrosswalk #txtCVXCode";
        } else {
            $("#txtVaccineSearch").val('');
            VaccineGroupID = $(ddlCategory).val();
        }
        if (Ctrl != null) {
            params["RefCtrl"] = Ctrl;
        }
        if (HiddenCtrl != null) {
            params["RefHiddenCtrl"] = HiddenCtrl;
        }
        if (RefCVXCtrl != null) {
            params["RefCVXCtrl"] = RefCVXCtrl;
        }
        if (RefCPTCtrl != null) {
            params["RefCPTCtrl"] = RefCPTCtrl;
        }
        params["VaccineGroupID"] = "0";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Immunization_VaccineCrosswalk';
        LoadActionPan('Immunization_SearchVaccine', params);
    },
    BindVaccineList: function (Control, hdfield, CVXCtrl, CPTCtrl) {
        var AllVaccine = [];
        if (CVXCtrl)
            $(CVXCtrl).val('');
        if (CPTCtrl)
            $(CPTCtrl).val('');
        if (Control && $(Control).attr("id").indexOf("search") > -1) {
            AllVaccine = Immunization_VaccineCrosswalk.AllSearchedVaccine;
        }
        else {
            AllVaccine = Immunization_VaccineCrosswalk.AllVaccine;
        }
        $(Control).autocomplete({
            autoFocus: true,
            source: AllVaccine, // pass an array
            select: function (event, ui) {
                setTimeout(function () {
                    $(Control).val(ui.item.value);
                    $(hdfield).val(ui.item.id);
                    if (CVXCtrl)
                        $(CVXCtrl).val(ui.item.RefValue);
                    if (CPTCtrl)
                        $(CPTCtrl).val(ui.item.RefName);

                }, 100);
            }
        });
    },
    ClearFields: function () {
        if ($('#' + Immunization_VaccineCrosswalk.params.PanelID + ' #frmImmunizationVaccineCrosswalk #txtPatientName').val() == "") {
            $('#' + Immunization_VaccineCrosswalk.params.PanelID + ' #frmImmunizationVaccineCrosswalk #lblVaccine').removeClass("hidden");
            $('#' + Immunization_VaccineCrosswalk.params.PanelID + ' #frmImmunizationVaccineCrosswalk #lnkAVaccine').addClass('hidden');
            $('#' + Immunization_VaccineCrosswalk.params.PanelID + ' #frmImmunizationVaccineCrosswalk #hfVaccineId').val('');
            $('#' + Immunization_VaccineCrosswalk.params.PanelID + ' #frmImmunizationVaccineCrosswalk #txrVaccine').val('');
        }
        else {
            $('#' + Immunization_VaccineCrosswalk.params.PanelID + ' #frmImmunizationVaccineCrosswalk #lblVaccine').addClass("hidden");
            $('#' + Immunization_VaccineCrosswalk.params.PanelID + ' #frmImmunizationVaccineCrosswalk #lnkAVaccine').removeClass('hidden');
        }
    },
    BindVaccineByCategory: function ($obj, lnkButtion, control, hdn, CVXCtrl, CPTCtrl) {
        //$(lnkButtion).removeClass("disableAll");
        if (CVXCtrl)
            $(CVXCtrl).val('');
        if (CPTCtrl)
            $(CPTCtrl).val('');
        //var Cateogry = "";
        //if ($obj) {
        //    Cateogry = $($obj).val();
        //}
        var forModule = "administer";
        var data = "StrID=" + null + "&StrID2=" + forModule;
        var AllVaccine = [];
        MDVisionService.lookups("GetAdministerVaccine_Vaccine", true, data).done(function (results) {
            results = JSON.parse(results["GetAdministerVaccine_Vaccine"]);
            AllVaccine = [];
            $.each(results, function (j, item) {
                if (item.Name.toUpperCase() != "- SELECT -")
                    AllVaccine.push({ id: item.Value, value: item.Name, RefValue: item.RefValue, RefName: item.RefName });
            });
            Immunization_VaccineCrosswalk.AllVaccine = AllVaccine;
            if (control && control.indexOf("search") > -1) {
                Immunization_VaccineCrosswalk.AllSearchedVaccine = AllVaccine;
            }
            else {
                Immunization_VaccineCrosswalk.AllVaccine = AllVaccine;
            }
            $(control).autocomplete({
                autoFocus: true,
                source: AllVaccine, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        if (control)
                            $(control).val(ui.item.value);
                        if (hdn)
                            $(hdn).val(ui.item.id);
                        if (CVXCtrl)
                            $(CVXCtrl).val(ui.item.RefName);
                        if (CPTCtrl)
                            $(CPTCtrl).val(ui.item.RefName);

                    }, 100);
                }
            });
        });
    },
    ValidateAutoComplete: function ($obj, hdn, CVXCodeCtrl, CPTCodeCtrl) {
        var isInitilized = $($obj).data('ui-autocomplete') != undefined;
        $('#' + Immunization_VaccineCrosswalk.params["PanelID"] + ' #frmImmunizationVaccineCrosswalk').bootstrapValidator('revalidateField', 'VaccineName');
        if (isInitilized) {
            var isValid = utility.ValidateAutoComplete($obj, 'frmImmunizationVaccineCrosswalk #hfVaccineId', false);
            if (isValid) {
            }
            else {
                if (CVXCodeCtrl)
                    $(CVXCodeCtrl).val('');
                if (CPTCodeCtrl)
                    $(CPTCodeCtrl).val('');
                $(hdn).val('');
            }
        }
        else {
            if (CVXCodeCtrl)
                $(CVXCodeCtrl).val('');
            if (CPTCodeCtrl)
                $(CPTCodeCtrl).val('');
            $(hdn).val('');
        }
        $('#' + Immunization_VaccineCrosswalk.params["PanelID"] + ' #frmImmunizationVaccineCrosswalk').bootstrapValidator('revalidateField', 'VaccineName');
    },
    /*BindVaccineListForGrid: function ($obj) {
        var row = $($obj).closest('tr');
        // $($obj).val('');
        var VaccineGroupID = ($(row).find("select[id^='ddlCategory']")).val() == "" ? "0" : $(row).find("select[id^='ddlCategory']").val();
        CPTCtrl = $($(row).find("input[id^='txtCPTCode']")).attr("id");
        CVXCtrl = $($(row).find("input[id^='txtCVXCode']")).attr("id");
        Ctrl = $($(row).find("input[id^='txtVaccine']")).attr("id");
        hdfield = $(row).find("input[id^='hfVaccineId']").attr("id");
        var AllVaccine = [];
    },*/
    BindVaccineByForGridCategory: function ($obj) {
        var row = $($obj).closest('tr');
        // $($obj).val('');
        var VaccineGroupID = ($(row).find("select[id^='ddlCategory']")).val() == "" ? "0" : $(row).find("select[id^='ddlCategory']").val();
        var control = $(row).find("input[id^='txtVaccine']");
        var CPTCtrl = $($(row).find("input[id^='txtCPTCode']")).attr("id");
        var CVXCtrl = $($(row).find("input[id^='txtCVXCode']")).attr("id");
        var Ctrl = $($(row).find("input[id^='txtVaccine']")).attr("id");
        if (control) {
            $(control).attr("oninput", "Immunization_VaccineCrosswalk.BindVaccineListForGrid(this)");
            control.attr("customname", "Vaccine")
        }
        $(row).find("input[id^='hfVaccineId']").attr("name", "VaccineId");
        var hdn = $(row).find("input[id^='hfVaccineId']").attr("id");
        if (CVXCtrl)
            $(CVXCtrl).val('');
        if (CPTCtrl)
            $(CPTCtrl).val('');
        //var Cateogry = "";
        //if ($obj) {
        //    Cateogry = $($obj).val();
        //}
        var forModule = "administer";
        var data = "StrID=" + null + "&StrID2=" + forModule;
        var AllVaccine = [];
        MDVisionService.lookups("GetAdministerVaccine_Vaccine", true, data).done(function (results) {
            results = JSON.parse(results["GetAdministerVaccine_Vaccine"]);
            AllVaccine = [];
            $.each(results, function (j, item) {
                if (item.Name.toUpperCase() != "- SELECT -")
                    AllVaccine.push({ id: item.Value, value: item.Name, RefValue: item.RefValue, RefName: item.RefName });
            });
            Immunization_VaccineCrosswalk.AllSearchedForGridVaccine = AllVaccine;
            $(control).autocomplete({
                autoFocus: true,
                source: AllVaccine, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        if (control)
                            $(control).val(ui.item.value);
                        if (hdn)
                            $(hdn).val(ui.item.id);
                        if (CVXCtrl)
                            $(CVXCtrl).val(ui.item.RefName);
                        if (CPTCtrl)
                            $(CPTCtrl).val(ui.item.RefName);

                    }, 100);
                }
            });
        });
    },
    BindVaccineListForGrid: function ($obj) {
        var row = $($obj).closest('tr');
        // $($obj).val('');
        var VaccineGroupID = ($(row).find("select[id^='ddlCategory']")).val() == "" ? "0" : $(row).find("select[id^='ddlCategory']").val();
        var control = $(row).find("input[id^='txtVaccine']");
        var CPTCtrl = $($(row).find("input[id^='txtCPTCode']")).attr("id");
        var CVXCtrl = $($(row).find("input[id^='txtCVXCode']")).attr("id");
        var Ctrl = $($(row).find("input[id^='txtVaccine']")).attr("id");
        $(row).find("input[id^='hfVaccineId']").attr("name", "VaccineId");
        var hdn = $(row).find("input[id^='hfVaccineId']").attr("id");
        var AllVaccine = [];
        if (CVXCtrl)
            $(CVXCtrl).val('');
        if (CPTCtrl)
            $(CPTCtrl).val('');

        AllVaccine = Immunization_VaccineCrosswalk.AllSearchedForGridVaccine;
        $(control).autocomplete({
            autoFocus: true,
            source: AllVaccine, // pass an array
            select: function (event, ui) {
                setTimeout(function () {
                    $(control).val(ui.item.value);
                    $(hdn).val(ui.item.id);
                    if (CVXCtrl)
                        $("#"+CVXCtrl).val(ui.item.RefValue);
                    if (CPTCtrl)
                        $("#" + CPTCtrl).val(ui.item.RefName);

                }, 100);
            }
        });
    },
}