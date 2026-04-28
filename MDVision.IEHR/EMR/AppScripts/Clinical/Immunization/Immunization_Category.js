/*
Class to handle operrations for Immunization Category
Author :  Khaleel Ur Rehman.
Date : 14-06-2016
*/

Immunization_Category = {
    bIsFirstLoad: true,
    params: [],
    myGrid: null,
    Load: function (params) {
        Immunization_Category.params = params;
        Immunization_Category.params.mode = "Add";
        if (Immunization_Category.params.PanelID != 'pnlImmunization_Category') {
            Immunization_Category.params.PanelID = Immunization_Category.params.PanelID + ' #pnlImmunization_Category';
        } else {
            Immunization_Category.params.PanelID = 'pnlImmunization_Category';
        }

        /*if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Immunization_Category.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }*/
        //objData["EntityId"] = globalAppdata["SeletedEntityId"];
        var self = $('#' + Immunization_Category.params.PanelID);
        if (Immunization_Category.bIsFirstLoad == true) {
            Immunization_Category.loadCategoryDrpdownList();
            self.loadDropDowns(true).done(function () {
                Immunization_Category.ValidateCategory();
                if (globalAppdata['AppUserName'] == DefaultUser) {
                    $("#" + Immunization_Category.params.PanelID + " #divEntity").show();
                }
                Immunization_Category.CategorySearch();
            });
        }

    },
    CategorySearch: function (VaccineGroupId, PageNo, rpp) {
        var strMessage = "";
        if ($("#" + Immunization_Category.params.PanelID + " #pnlCategory_Result").css("display") == "none") {
            $("#" + Immunization_Category.params.PanelID + " #pnlCategory_Result").show();
        }
        AppPrivileges.GetFormPrivileges("Immunization_Category", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Immunization_Category.SearchCategory(VaccineGroupId, PageNo, rpp).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        Immunization_Category.CategoryGridLoadNew(response);

                        var TableControl = Immunization_Category.params.PanelID + " #dgvCategory";
                        var PagingPanelControlID = Immunization_Category.params.PanelID + " #divCategorylistPaging";
                        var ClassControlName = "Immunization_Category";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.CategoryCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Immunization_Category.CategorySearch(PrimaryID, PageNumber, ResultPerPage);
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
    SearchCategory: function (vaccineGroupId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = new Object();
        // objData["VaccineGroupID"] = vaccineGroupId;
        objData["VaccineGroupID"] = $("#" + Immunization_Category.params.PanelID + " #ddlCategorySearch").val();
        objData["IsActive"] = $("#" + Immunization_Category.params.PanelID + " #ddlActiveSearch").val();
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        if (globalAppdata['AppUserName'] != DefaultUser) {
            objData["EntityId"] = globalAppdata["SeletedEntityId"];
        } else {
            objData["EntityId"] = null;
        }
        objData["commandType"] = "Get_All_Categories";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ImmunizationCategory");
    },
    CategoryGridLoadNew: function (response, PageNo, rpp) {
        $("#pnlImmunization_Category #dgvCategory").dataTable().fnDestroy();
        $("#pnlImmunization_Category #pnlCategory_Result #dgvCategory tbody").find("tr").remove();
        if (response.CategoryCount > 0) {
            var CategoryLoadJSONData = JSON.parse(response.CategoryLoad_JSON);
            var actions = "";
            $("#" + Immunization_Category.params.PanelID + " #dgvCategory tr th").each(function () {
                if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                    var arrActionType = [];
                    if ($(this).attr("ActionType") != null) {
                        arrActionType = $(this).attr("ActionType").split(',');
                        actions = EMREditableGrid.GetActions(arrActionType, "#" + Immunization_Category.params.PanelID + " #pnlCategory_Result");
                    }
                }
            });

            $.each(CategoryLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", item.VaccineGroupID);
                $row.attr("onclick", "utility.SelectGridRow($(this))");
                $row.attr("VaccineGroupID", item.VaccineGroupID);
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
                $row.append('<td class="actions" id="' + item.VaccineGroupID + '" >' + actions + '</td><td>' + item.ShortName + '</td>');
                //$row.append('<td style="display:none;">' + item.VaccineGroupID + '</td><td><a class="btn  btn-xs" href="#" onclick="Immunization_Category.CategoryDelete(' + item.VaccineGroupID + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Immunization_Category.CategoryEdit(' + item.VaccineGroupID + ');"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Immunization_Category.CategoryActiveInactive(' + item.VaccineGroupID + "," + isactive + ');" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.ShortName + '</td>');
                $('#' + Immunization_Category.params["PanelID"] + " #pnlCategory_Result #dgvCategory tbody").last().append($row);
            });
            var PanelGrid = "#" + Immunization_Category.params.PanelID + " #pnlCategory_Result";
            var GridId = "#" + Immunization_Category.params.PanelID + " #dgvCategory";

            if (Immunization_Category.myGrid != null) {
                if ($.fn.dataTable.isDataTable(Immunization_Category.myGrid)) {
                    Immunization_Category.myGrid.$table.dataTable().fnDestroy();
                } else {
                    Immunization_Category.myGrid = null;
                }
                if ($.fn.dataTable.isDataTable("#" + Immunization_Category.params.PanelID + " #pnlCategory_Result #dgvCategory")) {
                    $("#" + Immunization_Category.params.PanelID + " #pnlProblemLists_Result #dgvCategory").dataTable().fnDestroy();
                }
            }
            Immunization_Category.myGrid = EMRUtility.MakeEditableGrid(PanelGrid, GridId, Immunization_Category, 0, false, true, false, true, false, null);
            $("#" + Immunization_Category.params.PanelID + " #dgvCategory tbody tr").each(function () {
                if ($(this).attr("id") && $(this).attr("IsActive")) {
                    var arrActionType = [];
                    if ($(this).attr("ActionType") != null) {
                        arrActionType = $(this).attr("ActionType").split(',');
                        actions = EMREditableGrid.GetActions(arrActionType, "#" + Immunization_Category.params.PanelID + " #pnlCategory_Result");
                    }
                }
            });
        }
        else {
            $('#pnlImmunization_Category #dgvCategory').DataTable({
                "language": {
                    "emptyTable": "No Category Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }


        if ($.fn.dataTable.isDataTable('#' + Immunization_Category.params["PanelID"] + ' #dgvCategory'))
            ;
        else
            $('#' + Immunization_Category.params["PanelID"] + ' #pnlCategory_Result #dgvCategory').DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        EMRUtility.SwicthWidgetInializatoin();

    },
    UnLoadTab: function () {
        var objDeffered = $.Deferred();
        if (Immunization_Category.params["FromAdmin"] == "0") {
            if (Immunization_Category.params != null && Immunization_Category.params.ParentCtrl != null) {
                UnloadActionPan(Immunization_Category.params.ParentCtrl, 'Immunization_Category');
            }
            else
                UnloadActionPan(null, 'Immunization_Category');
        }
        else {

            RemoveAdminTab();
        }
        return objDeffered;
    },
    CategoryActiveInactive: function (VaccineGroupId, IsActive) {
        AppPrivileges.GetFormPrivileges("Immunization_Category", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = VaccineGroupId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Immunization_Category.IdentifyEntity();
                        Immunization_Category.UpdateCategoryActiveInactive(selectedValue, IsActive).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                Immunization_Category.loadCategoryDrpdownList();
                                utility.DisplayMessages(response.message, 1);
                                Immunization_Category.CategorySearch();
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

    UpdateCategoryActiveInactive: function (VaccineGroupId, IsActive) {
        var objData = new Object();
        objData["VaccineGroupID"] = VaccineGroupId;
        objData["IsActive"] = IsActive;
        objData["EntityId"] = Immunization_Category.params.EntityID;
        objData["commandType"] = "ActiveInactive_Category";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ImmunizationCategory");
    },
    CategoryDelete: function (catID) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Immunization_Category", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = catID;
                    var oTable = $('#dgvCategory').DataTable();
                    var ind = $(this).index();
                    var idx = oTable.row(this).index();
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Immunization_Category.DeleteCategory(selectedValue).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                Immunization_Category.loadCategoryDrpdownList();
                                Immunization_Category.CategorySearch();
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


    DeleteCategory: function (catId) {
        var objData = new Object();
        objData["VaccineGroupID"] = catId;
        objData["commandType"] = "DELETE_CATEGORY";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ImmunizationCategory");
    },

    CategoryEdit: function (catId) {
        Immunization_Category.SearchCategory(catId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $("#" + Immunization_Category.params.PanelID + " #txtCategory").val(JSON.parse(response.CategoryLoad_JSON)[0].ShortName);
                $("#" + Immunization_Category.params.PanelID + " #ddlActive").val(JSON.parse(response.CategoryLoad_JSON)[0].IsActive == "True" ? 1 : 0);
                Immunization_Category.params.VaccineGroupId = JSON.parse(response.CategoryLoad_JSON)[0].VaccineGroupID;
                Immunization_Category.params.mode = "Edit";
            }
        });
    },
    ImmunizationCategorySave: function () {
        $("#" + Immunization_Category.params.PanelID + " #frmImmunizationCategory").bootstrapValidator('revalidateField', 'ShortName');
        Immunization_Category.IdentifyEntity();
        if ($("#" + Immunization_Category.params.PanelID + " #frmImmunizationCategory #txtCategory").val() != "") {
            var strMessage = "";
            var self = $("#" + Immunization_Category.params.PanelID + " #frmImmunizationCategory");
            var myJSON = self.getMyJSONByName();
            if (Immunization_Category.params.mode == "Add") {
                AppPrivileges.GetFormPrivileges("Immunization_Category", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        Immunization_Category.SaveImmunizationCategory(myJSON).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                Immunization_Category.loadCategoryDrpdownList();
                                var PanelGrid = "#pnlImmunization_Category #pnlCategory_Result";
                                var GridId = "#pnlImmunization_Category #dgvCategory";
                                utility.MakeEditableGrid(PanelGrid, GridId, Immunization_Category);
                                utility.DisplayMessages(response.message, 1);
                                Immunization_Category.CategorySearch();
                                $("#" + Immunization_Category.params.PanelID + " #txtCategory").val('');
                                $("#" + Immunization_Category.params.PanelID + " #ddlActive").val(1);
                                utility.ClearFormValidation('#' + Immunization_Category.params.PanelID + ' #frmImmunizationCategory');
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
            else if (Immunization_Category.params.mode == "Edit") {
                AppPrivileges.GetFormPrivileges("Immunization_Category", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        Immunization_Category.UpdateImmunizationCategory(myJSON, Immunization_Category.params.VaccineGroupId).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                Immunization_Category.loadCategoryDrpdownList();
                                utility.DisplayMessages(response.message, 1);
                                Immunization_Category.CategorySearch();
                                Immunization_Category.params.mode = "Add";
                                $("#" + Immunization_Category.params.PanelID + " #txtCategory").val('');
                                $("#" + Immunization_Category.params.PanelID + " #ddlActive").val(1);
                                utility.ClearFormValidation('#' + Immunization_Category.params.PanelID + ' #frmImmunizationCategory');
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
        }
    },
    SaveImmunizationCategory: function (ImmunizationCategoryData) {
        var objData = JSON.parse(ImmunizationCategoryData);
        objData["EntityId"] = Immunization_Category.params.EntityID;
        objData["commandType"] = "Save_ImmunizationCategory";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ImmunizationCategory");
    },
    UpdateImmunizationCategory: function (ImmunizationCategoryData, VaccineGroupId) {
        var objData = JSON.parse(ImmunizationCategoryData);
        objData["EntityId"] = Immunization_Category.params.EntityID;
        objData["VaccineGroupID"] = VaccineGroupId;
        objData["commandType"] = "Update_ImmunizationCategory";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ImmunizationCategory");
    },
    IdentifyEntity: function () {
        if (globalAppdata['AppUserName'] == DefaultUser) {
            Immunization_Category.params.EntityID = $("#pnlImmunization_Category #ddlEntity").val();
        }
        else {
            Immunization_Category.params.EntityID = globalAppdata["SeletedEntityId"];
        }
    },
    ValidateCategory: function () {
        $("#" + Immunization_Category.params.PanelID + ' #frmImmunizationCategory')
          .bootstrapValidator('destroy');
        $("#" + Immunization_Category.params.PanelID + ' #frmImmunizationCategory')
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
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            Immunization_Category.ImmunizationCategorySave();
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
                AppPrivileges.GetFormPrivileges("Immunization_Category", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        Immunization_Category.UpdateImmunizationCategory(myJSON, id).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                Immunization_Category.loadCategoryDrpdownList();
                                utility.DisplayMessages(response.message, 1);
                                Immunization_Category.CategorySearch();
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
        //Start//22/12/2015//Ahmad Raza//Privileges Logic Implemented
        var strMessage = "";
        var catID = $row.attr("id");
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Immunization_Category", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = catID;
                    var oTable = $('#dgvCategory').DataTable();
                    var ind = $(this).index();
                    var idx = oTable.row(this).index();
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Immunization_Category.DeleteCategory(selectedValue).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                Immunization_Category.loadCategoryDrpdownList();
                                Immunization_Category.CategorySearch();
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
        AppPrivileges.GetFormPrivileges("Immunization_Category", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = id;                    
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Immunization_Category.IdentifyEntity();
                        Immunization_Category.UpdateCategoryActiveInactive(selectedValue, IsActive).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Immunization_Category.CategorySearch();
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
    loadCategoryDrpdownList: function () {
        Immunization_Category.SearchCategory(null, null, 1000).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var CategoryLoadJSONData = JSON.parse(response.CategoryLoad_JSON);
                var l = $("#" + Immunization_Category.params.PanelID + " #frmImmunizationCategory #ddlCategorySearch");
                l.empty();
                l.append($("<option />").val('').text('- Select -'));
                $.each(CategoryLoadJSONData, function (j, result) {
                    l.append($("<option />").val(result.VaccineGroupID).text(result.ShortName));
                });
            }
        });
    },
}