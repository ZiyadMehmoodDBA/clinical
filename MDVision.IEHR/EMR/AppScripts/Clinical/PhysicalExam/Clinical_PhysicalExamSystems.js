Clinical_PhysicalExamSystems = {
    params: [],

    Load: function (params) {
        Clinical_PhysicalExamSystems.params = params;

        //var self = $('#' + Clinical_PhysicalExamSystems.params.PanelID);
        //if (Clinical_PhysicalExamSystems.bIsFirstLoad == true) {
        //    self.loadDropDowns(true);
        //}

        Clinical_PhysicalExamSystems.searchPESystems();
    },

    searchPESystems: function () {

        Clinical_PhysicalExamSystems.PESystemsGridLoad(1, 25);
    },

    loadPESystems_DBcall: function (IsActive, ShortName, PageNumber, RowsPerPage) {
        var objData = new Object();
        objData["IsActive"] = IsActive;
        objData["Name"] = ShortName;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "load_physicalexam_system";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamECW", "PhysicalExamSystem");
    },

    PESystemsGridLoad: function (pageNo, rpp) {

        // Initialize Grid
        var grid = $("#pnlAdminPESystems #dgvPESystems").kendoGrid({
            sortable: true,
            resizable: true,
            scrollable: false,
            messages: {
                noRecords: "There is no data on current page"
            },
            pageable: {
                refresh: true,
                pageSizes: [5, 10, 20, 50, 100],
                buttonCount: 5
            },
            columns: [
                { title: "Action", width: "100px", template: '#=Clinical_PhysicalExamSystems.ActionPESystems(data)#' },
                { title: "Systems", field: "Name", title: "Systems", width: "100px" },
                { title: "Status", title: "Status", template: '#=Clinical_PhysicalExamSystems.setActionValue(data)#', width: "100px" }
                ],
        }).data("kendoGrid");


        //Data Source of Grid
        var dataSource = new kendo.data.DataSource({
            schema: {
                data: "data",
                total: "total",
            },
            serverPaging: true,
            pageSize: rpp,
            page: pageNo,
            transport: {
                read: function (e) {

                    var name = $('#pnlAdminPESystems #frmSearchPESystems #txtName').val();
                    var status = $('#pnlAdminPESystems #frmSearchPESystems #ddlStatus').val();

                    Clinical_PhysicalExamSystems.loadPESystems_DBcall(status, name, e.data.page, e.data.pageSize).done(function (response) {
                        var data_ = { data: [], total: 0 };
                        if (response != "") {
                            var resposeData = JSON.parse(response);
                            if (resposeData.status != false && resposeData.SystemCount > 0) {
                                if (resposeData.SystemLoad_JSON != undefined)
                                    data_.data = jQuery.parseJSON(resposeData.SystemLoad_JSON);
                                data_.total = resposeData.iTotalDisplayRecords;
                                e.success(data_);
                            }
                            else {
                                utility.DisplayMessages(resposeData.Message, 3);
                                e.success(data_);
                            }
                        }
                        else {
                            e.error();
                        }

                        //Start 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
                        utility.removePaginationFromGrid($("#pnlAdminPESystems #dgvPESystems"));
                        //End 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
                    });
                }
            }
        });
        grid.setDataSource(dataSource);
    },

    setActionValue: function (data) {

        var isactive = "";
        if (data.IsActive == "True") {
            isactive = "Active";
        }
        else {
            isactive = "Inactive";
        }
        return isactive;
    },

    ActionPESystems: function (data) {

        var deletePESystemsMethod = "Clinical_PhysicalExamSystems.PESystemsDelete('" + data.PESystemId + "')";
        var editPESystemsMethod = "Clinical_PhysicalExamSystems.PESystemsEdit('" + data.PESystemId + "')";

        if (data.IsActive == "True") {
            isactive = 0;
            activeTitle = "Active Record";
            tglclass = "fa fa-toggle-on green";
        }
        else {
            isactive = 1;
            activeTitle = "Inactive Record";
            tglclass = "fa fa-toggle-on red fa-rotate-180";
        }
        var activeInactivePESystemsMethod = "Clinical_PhysicalExamSystems.PESystemsActiveInactive('" + data.PESystemId + "', '" + isactive + "')";


        //return '<a class="btn  btn-xs" href="javascript:;" onclick="' + editPESystemsMethod + '" title="Edit ICD Group"><i class="fa fa-edit black"></i></a>';
        return '<a class="btn  btn-xs" href="#" onclick="' + deletePESystemsMethod + '" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn  btn-xs" href="javascript:;" onclick="' + editPESystemsMethod + '" title="Edit System"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="' + activeInactivePESystemsMethod + '" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>';
    },

    PESystemsEdit: function (PESystemId) {

        var params = [];
        params["PESystemId"] = PESystemId;
        params["mode"] = "Edit";
        params["FromAdmin"] = Clinical_PhysicalExamSystems.params["FromAdmin"];
        if (Clinical_PhysicalExamSystems.params["FromAdmin"] == "0") {
            params["ParentCtrl"] = 'adminTabSystems';
        }

        LoadActionPan('Clinical_PhysicalExamSystemsDetail', params);
    },

    PESystemsDelete: function (PESystemId) {
        utility.myConfirm("1", function () {
            var data = "PESystemId=" + PESystemId;
            Clinical_PhysicalExamSystems.PESystemsDelete_DBcall(PESystemId).done(function (response) {
                if (response != "") {
                    response = JSON.parse(response);
                    if (response.status == true) {
                        utility.DisplayMessages(response.Message, 1);
                        Clinical_PhysicalExamSystems.searchPESystems();
                    }
                    else
                        utility.DisplayMessages(response.Message, 3);
                }
                else
                    utility.DisplayMessages(response.Message, 3);
            });
        }, function () { });

    },

    PESystemsDelete_DBcall: function (PESystemId) {
        var objData = new Object();
        objData["PESystemId"] = PESystemId;
        objData["commandType"] = "delete_physicalexam_system";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamECW", "PhysicalExamSystem");
    },

    PESystemsActiveInactive: function (PESystemId, IsActive) {
        utility.myConfirm("3", function () {
            Clinical_PhysicalExamSystems.PESystemsActiveInactive_DBcall(PESystemId, IsActive).done(function (response) {

                if (response != "") {
                    response = JSON.parse(response);
                    if (response.status == true) {
                        utility.DisplayMessages(response.Message, 1);
                        Clinical_PhysicalExamSystems.searchPESystems();
                    }
                    else
                        utility.DisplayMessages(response.Message, 3);
                }
                else
                    utility.DisplayMessages(response.Message, 3);
            });
        }, function () { },
                    "3", null, null, null, IsActive
                );
    },

    PESystemsActiveInactive_DBcall: function (PESystemId, IsActive) {
        var objData = new Object();
        objData["PESystemId"] = PESystemId;
        objData["IsActive"] = IsActive;
        objData["commandType"] = "active_inactive_physicalexam_system";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamECW", "PhysicalExamSystem");
    },

    PESystemsAdd: function () {
        var strMessage = "";
        if (strMessage == "") {
           var params = [];
            params["PESystemsId"] = null;
            params["mode"] = "Add";
            params["FromAdmin"] = Clinical_PhysicalExamSystems.params["FromAdmin"];
            if (Clinical_PhysicalExamSystems.params["FromAdmin"] == "0") {
                params["ParentCtrl"] = 'adminTabSystems';
                LoadActionPan('Clinical_PhysicalExamSystemsDetail', params, Clinical_PhysicalExamSystems.params.PanelID);
            }
            else {
                LoadActionPan('Clinical_PhysicalExamSystemsDetail', params);
            }
            
        }
    },

    UnLoadTab: function () {

        if (Clinical_PhysicalExamSystems.params["FromAdmin"] == "0") {

            if (Clinical_PhysicalExamSystems.params != null && Clinical_PhysicalExamSystems.params.ParentCtrl != null && Clinical_PhysicalExamSystems.params.PanelID != 'pnlAdminPESystems') {
                UnloadActionPan(Clinical_PhysicalExamSystems.params.ParentCtrl, 'Clinical_PhysicalExamSystems', null, Clinical_PhysicalExamSystems.params.PanelID);
            }
            else if (Clinical_PhysicalExamSystems.params != null && Clinical_PhysicalExamSystems.params.ParentCtrl != null) {
                UnloadActionPan(Clinical_PhysicalExamSystems.params.ParentCtrl, 'Clinical_PhysicalExamSystems');
            }
            else
                UnloadActionPan(null, 'Clinical_PhysicalExamSystems');

        }
        else {
            RemoveAdminTab();
        }

    },
}
