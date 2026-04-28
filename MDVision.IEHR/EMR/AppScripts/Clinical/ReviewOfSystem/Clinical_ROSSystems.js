Clinical_ROSSystems = {
    params: [],

    Load: function (params) {
        Clinical_ROSSystems.params = params;

        //var self = $('#' + Clinical_ROSSystems.params.PanelID);
        //if (Clinical_ROSSystems.bIsFirstLoad == true) {
        //    self.loadDropDowns(true);
        //}

        Clinical_ROSSystems.searchROSSystems();
    },

    searchROSSystems: function () {

        Clinical_ROSSystems.ROSSystemsGridLoad(1, 25);
    },

    loadROSSystems_DBcall: function (IsActive, ShortName, PageNumber, RowsPerPage) {
        var objData = new Object();
        objData["IsActive"] = IsActive;
        objData["Name"] = ShortName;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "load_reviewofsystems_system";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemRMP", "ROSSystem");
    },

    ROSSystemsGridLoad: function (pageNo, rpp) {

        // Initialize Grid
        var grid = $("#pnlROSSystems #dgvPESystems").kendoGrid({
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
                { title: "Action", width: "100px", template: '#=Clinical_ROSSystems.ActionPESystems(data)#' },
                { title: "Systems", field: "Name", title: "Systems", width: "100px" },
                { title: "Status", title: "Status", template: '#=Clinical_ROSSystems.setActionValue(data)#', width: "100px" }
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

                    var name = $('#pnlROSSystems #frmSearchROSSystems #txtName').val();
                    var status = $('#pnlROSSystems #frmSearchROSSystems #ddlStatus').val();

                    Clinical_ROSSystems.loadROSSystems_DBcall(status, name, e.data.page, e.data.pageSize).done(function (response) {
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
                        utility.removePaginationFromGrid($("#pnlROSSystems #dgvPESystems"));
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

        var deleteROSSystemsMethod = "Clinical_ROSSystems.ROSSystemsDelete('" + data.ROSSystemId + "')";
        var editPESystemsMethod = "Clinical_ROSSystems.PESystemsEdit('" + data.ROSSystemId + "')";

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
        var activeInactivePESystemsMethod = "Clinical_ROSSystems.ROSSystemsActiveInactive('" + data.ROSSystemId + "', '" + isactive + "', '" + data.Name + "')";


        //return '<a class="btn  btn-xs" href="javascript:;" onclick="' + editPESystemsMethod + '" title="Edit ICD Group"><i class="fa fa-edit black"></i></a>';
        return '<a class="btn  btn-xs" href="#" onclick="' + deleteROSSystemsMethod + '" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn  btn-xs" href="javascript:;" onclick="' + editPESystemsMethod + '" title="Edit System"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="' + activeInactivePESystemsMethod + '" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>';
    },

    PESystemsEdit: function (PESystemId) {

        var params = [];
        params["ROSSystemId"] = PESystemId;
        params["mode"] = "Edit";
        params["FromAdmin"] = Clinical_ROSSystems.params["FromAdmin"];
        if (Clinical_ROSSystems.params["FromAdmin"] == "0") {
            params["ParentCtrl"] = 'adminTabSystems';
        }

        LoadActionPan('Clinical_ROSSystemsDetail', params);
    },

    ROSSystemsDelete: function (ROSSystemId) {
        utility.myConfirm("1", function () {
            var data = "ROSSystemId=" + ROSSystemId;
            Clinical_ROSSystems.ROSSystemsDelete_DBcall(ROSSystemId).done(function (response) {
                if (response != "") {
                    response = JSON.parse(response);
                    if (response.status == true) {
                        utility.DisplayMessages(response.Message, 1);
                        Clinical_ROSSystems.searchROSSystems();
                    }
                    else
                        utility.DisplayMessages(response.Message, 3);
                }
                else
                    utility.DisplayMessages(response.Message, 3);
            });
        }, function () { });

    },

    ROSSystemsDelete_DBcall: function (ROSSystemId) {
        var objData = new Object();
        objData["ROSSystemId"] = ROSSystemId;
        objData["commandType"] = "delete_reviewofsystem_system";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemRMP", "ROSSystem");
    },

    ROSSystemsActiveInactive: function (PESystemId, IsActive, Name) {
        utility.myConfirm("3", function () {
            Clinical_ROSSystems.ROSSystemsActiveInactive_DBcall(PESystemId, IsActive, Name).done(function (response) {

                if (response != "") {
                    response = JSON.parse(response);
                    if (response.status == true) {
                        utility.DisplayMessages(response.Message, 1);
                        Clinical_ROSSystems.searchROSSystems();
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

    ROSSystemsActiveInactive_DBcall: function (PESystemId, IsActive, Name) {
        var objData = new Object();
        objData["ROSSystemId"] = PESystemId;
        objData["IsActive"] = IsActive;
        objData["Name"] = Name;
        objData["commandType"] = "active_inactive_reviewofsystem_system";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemRMP", "ROSSystem");
    },

    ROSSystemsAdd: function () {
        var strMessage = "";
        if (strMessage == "") {
            var params = [];
            params["PESystemsId"] = null;
            params["mode"] = "Add";
            params["FromAdmin"] = Clinical_ROSSystems.params["FromAdmin"];
            if (Clinical_ROSSystems.params["FromAdmin"] == "0") {
                params["ParentCtrl"] = 'adminTabSystems';
                LoadActionPan('Clinical_ROSSystemsDetail', params, Clinical_ROSSystems.params.PanelID);
            }
            else {
                LoadActionPan('Clinical_ROSSystemsDetail', params);
            }

        }
    },

    UnLoadTab: function () {

        if (Clinical_ROSSystems.params["FromAdmin"] == "0") {

            if (Clinical_ROSSystems.params != null && Clinical_ROSSystems.params.ParentCtrl != null && Clinical_ROSSystems.params.PanelID != 'pnlROSSystems') {
                UnloadActionPan(Clinical_ROSSystems.params.ParentCtrl, 'Clinical_ROSSystems', null, Clinical_ROSSystems.params.PanelID);
            }
            else if (Clinical_ROSSystems.params != null && Clinical_ROSSystems.params.ParentCtrl != null) {
                UnloadActionPan(Clinical_ROSSystems.params.ParentCtrl, 'Clinical_ROSSystems');
            }
            else
                UnloadActionPan(null, 'Clinical_ROSSystems');

        }
        else {
            RemoveAdminTab();
        }

    },
}
