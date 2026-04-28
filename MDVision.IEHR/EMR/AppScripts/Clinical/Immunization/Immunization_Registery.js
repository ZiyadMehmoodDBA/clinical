Immunization_Registery = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Immunization_Registery.params = params;
        if (Immunization_Registery.params.PanelID)
            Immunization_Registery.params.PanelID = 'pnlImmunization_Registery';
        else
            Immunization_Registery.params.PanelID = 'pnlImmunization_Registery';

        if (Immunization_Registery.bIsFirstLoad) {
            Immunization_Registery.bIsFirstLoad = false;
            var self = $('#pnlImmunization_Registery');
            self.loadDropDowns(true);
            Immunization_Registery.SearchRegisteryConfiguration();
        }
    },

    SearchRegisteryConfiguration: function (PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Registery", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var self = $("#pnlImmunization_Registery #frmRegistery");
                var myJSON = self.getMyJSONByName();
                Immunization_Registery.SearchRecords(myJSON, PageNo, rpp).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status == true) {
                        Immunization_Registery.GridLoad(response, PageNo, rpp);
                        var TableControl = "pnlImmunization_Registery #dgvRegistery";
                        var PagingPanelControlID = "pnlImmunization_Registery #dgvRegistery_Paging";
                        var ClassControlName = "Immunization_Registery";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.RecordCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Immunization_Registery.SearchRegisteryConfiguration(PageNumber, ResultPerPage);
                        }), 10);
                    }
                    else
                        utility.DisplayMessages(response.Message, 3);
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    GridLoad: function (response) {
        $("#pnlImmunization_Registery #dgvRegistery").dataTable().fnDestroy();
        $("#pnlImmunization_Registery #dgvRegistery tbody").find("tr").remove();
        var RegisteryLoad_JSON = JSON.parse(response.RegisteryLoad_JSON);
        if (RegisteryLoad_JSON.length > 0) {
            $.each(RegisteryLoad_JSON, function (i, item) {
                var RegistryConfigurationId = item.RegistryConfigurationId;
                var $row = $('<tr/>');
                $row.attr("id", "dgvImmunizationRegistery_row" + RegistryConfigurationId);
                var MethodMode = 'Immunization_Registery.EditRegistery(' + RegistryConfigurationId + ');';
                $row.attr("onclick", MethodMode);
                var status = "Testing";
                if (item.Status) {
                    if (item.Status == "P")
                        status = "Production";
                }
                var active = "";
                if (item.IsActive) {
                    if (item.IsActive == "True")
                        active = "Active";
                    else
                        active = "In-Active";
                }
                $row.append('<td><a class="btn  btn-xs" href="#" onclick="Immunization_Registery.DeleteRegistery(' + RegistryConfigurationId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a  class="btn btn-xs" href="javascript:void(0);" onclick="Immunization_Registery.EditRegistery(' + RegistryConfigurationId + ');"   title="Edit Record"><i class="fa fa-edit black"></i></a></td><td>' + item.ProviderName + '</td><td>' + item.ReceivingApplicationName + '</td><td>' + status + '</td><td>' + item.SubmissionName + '</td>' + '</td><td>' + active + '</td>' + '</td><td>' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '</td>');
                $("#" + Immunization_Registery.params.PanelID + " #dgvRegistery tbody").append($row);
            });
            var gridRows = $("#" + Immunization_Registery.params.PanelID + " #dgvRegistery tbody").find("tr");
            if (gridRows.length < 1) {
                $("#" + Immunization_Registery.params.PanelID + " #dgvRegistery").DataTable({
                    "language": {
                        "emptyTable": "No Record Found."
                    }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }, { "targets": [7], "visible": false }]
                });
                $("#pnlImmunization_Registery #dgvRegistery_Paging").css("display", "none");
            }
        }
        else {
            $("#" + Immunization_Registery.params.PanelID + " #dgvRegistery_Paging").css("display", "none");
            $("#" + Immunization_Registery.params.PanelID + " #dgvRegistery").DataTable({
                "language": {
                    "emptyTable": "No Record Found."
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
    },
    EditRegistery: function (RegistryConfigurationId) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#dgvImmunization_Registery_row' + RegistryConfigurationId));
        AppPrivileges.GetFormPrivileges("Registery", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = RegistryConfigurationId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["RegisteryDetailId"] = selectedValue;
                    params["mode"] = "Edit";
                    params["FromAdmin"] = "0";
                    params["Active"] = $("#" + Immunization_Registery.params.PanelID + " #frmRegistery #ddlActive").val();
                    LoadActionPan('Immunization_RegisteryDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    DeleteRegistery: function (RegistryConfigurationId) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#dgvImmunization_Registery_row' + RegistryConfigurationId));
        AppPrivileges.GetFormPrivileges("Registery", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = RegistryConfigurationId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Immunization_Registery.DeleteRecord(selectedValue).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Immunization_Registery.SearchRegisteryConfiguration();
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
    DeleteRecord: function (RegisteryDetailId) {
        var objData = new Object();
        RegisteryDetailId ? objData["RegistryConfigurationId"] = RegisteryDetailId : objData["RegistryConfigurationId"] = 0;
        objData["commandType"] = "DELETE_IMMUNIZATION_REGISTERY";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "ImmunizationRegistery");
    },
    SearchRecords: function (SearchData, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = JSON.parse(SearchData);
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "SEARCH_IMMUNIZATION_REGISTERY";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "ImmunizationRegistery");
    },

    AddRegistery: function () {

        var params = [];
        params["ParentCtrl"] = "Immunization_Registery";
        params["FromAdmin"] = 0;
        params["mode"] = "Add";
        LoadActionPan("Immunization_RegisteryDetail", params);

    },

    UnLoadTab: function () {

        if (Immunization_Registery.params["FromAdmin"] == "0") {
            if (Immunization_Registery.params != null && Immunization_Registery.params.ParentCtrl != null) {
                UnloadActionPan(Immunization_Registery.params.ParentCtrl, 'Immunization_Registery');
            }
            else
                UnloadActionPan(null, 'Immunization_Registery');
        }
        else {
            RemoveAdminTab();
        }

    },
};