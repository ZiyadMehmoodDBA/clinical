Clinical_HPIFindings = {
    params: [],

    Load: function (params) {
        Clinical_HPIFindings.params = params;
        Clinical_HPIFindings.searchHPIFindings();

        if (Clinical_HPIFindings.params.PanelID != "pnlAdminHPIFindings")
            Clinical_HPIFindings.params.PanelID = Clinical_HPIFindings.params.PanelID + " #pnlAdminHPIFindings";
        else
            Clinical_HPIFindings.params.PanelID = "pnlAdminHPIFindings";

    },

    searchHPIFindings: function () {

        Clinical_HPIFindings.HPIFindingsGridLoad(1, 25);
    },

    HPIFindingsGridLoad: function (pageNo, rpp) {

        // Initialize Grid
        var grid = $("#" + Clinical_HPIFindings.params.PanelID + " #dvgHPIFindings").kendoGrid({
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
             { title: "Action", width: "100px", template: '#=Clinical_HPIFindings.ActionHPIFinding(data)#' },
             { title: "Findings", field: "Name", title: "Findings", width: "100px" },
             { title: "Status", title: "Status", template: '#=Clinical_HPIFindings.setActionValue(data)#', width: "100px" }
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

                    var name = $('#' + Clinical_HPIFindings.params.PanelID + ' #HPIFindings #txtName').val();
                    var status = $('#' + Clinical_HPIFindings.params.PanelID + ' #HPIFindings #ddlStatus').val();

                    Clinical_HPIFindings.loadHPIFinding_DBcall(status, name, e.data.page, e.data.pageSize).done(function (response) {
                        var data_ = { data: [], total: 0 };
                        if (response) {
                            var resposeData = JSON.parse(response);
                            if (resposeData.status != false && resposeData.FindingsCount > 0) {
                                if (resposeData.listFindings)
                                    data_.data = resposeData.listFindings;
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
                        utility.removePaginationFromGrid($('#' + Clinical_HPIFindings.params.PanelID + " #dvgHPIFindings"));
                        //End 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
                    });
                }
            }
        });
        grid.setDataSource(dataSource);
    },

    ActionHPIFinding: function (data) {
        if (Clinical_HPIFindings.params.ParentCtrl == "Clinical_HPISymptomsDetail") {

            var Id_ = data.HPIFindingsId;
            Clinical_HPISymptomsDetail.FindingsList.push(data);
            Clinical_HPISymptomsDetail.bIsTextChanged = true;
            var fillHPIFindingsMethod = "Clinical_HPISymptomsDetail.fillHPIFinding('" + Id_ + "')";

            return '<a class="btn  btn-xs" href="#" onclick="' + fillHPIFindingsMethod + '" title="Select Record"><i class="fa fa-check black"></i></a>';
        }
        else {
            var deleteHPIFindingsMethod = "Clinical_HPIFindings.HPIFindingsDelete('" + data.HPIFindingsId + "')";
            var editHPIFindingsMethod = "Clinical_HPIFindings.HPIFindingsEdit('" + data.HPIFindingsId + "')";

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
            var activeInactiveHPIFindingsMethod = "Clinical_HPIFindings.HPIFindingsActiveInactive('" + data.HPIFindingsId + "', '" + isactive + "', '" + data.Name + "')";

            return '<a class="btn  btn-xs" href="#" onclick="' + deleteHPIFindingsMethod + '" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn  btn-xs" href="javascript:;" onclick="' + editHPIFindingsMethod + '" title="Edit Finding"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="' + activeInactiveHPIFindingsMethod + '" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>';
        }
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

    loadHPIFinding_DBcall: function (status, name, PageNumber, RowsPerPage) {
        var objData = new Object();
        objData["IsActive"] = status;
        objData["Name"] = name;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "load_hpi_findings";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPIFindings");
    },

    HPIFindingsDelete: function (HPIFindingsId) {
        utility.myConfirm("1", function () {
            var data = "HPIFindingsId=" + HPIFindingsId;
            Clinical_HPIFindings.HPIFindingsDelete_DBcall(HPIFindingsId).done(function (response) {
                if (response != "") {
                    response = JSON.parse(response);
                    if (response.status == true) {
                        utility.DisplayMessages(response.Message, 1);
                        Clinical_HPIFindings.searchHPIFindings();
                    }
                    else
                        utility.DisplayMessages(response.Message, 3);
                }
                else
                    utility.DisplayMessages(response.Message, 3);
            });
        }, function () { });

    },

    HPIFindingsDelete_DBcall: function (HPIFindingsId) {
        var objData = new Object();
        objData["HPIFindingsId"] = HPIFindingsId;
        objData["commandType"] = "delete_hpi_findings";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPIFindings");
    },

    HPIFindingsEdit: function (HPIFindingsId) {

        var params = [];
        params["HPIFindingsId"] = HPIFindingsId;
        params["mode"] = "Edit";
        params["FromAdmin"] = Clinical_HPIFindings.params["FromAdmin"];
        if (Clinical_HPIFindings.params["FromAdmin"] == "0") {
            params["ParentCtrl"] = 'Clinical_HPIFindings';
            LoadActionPan('Clinical_HPIFindingsDetail', params, Clinical_HPIFindings.params.PanelID);
        }
        else
            LoadActionPan('Clinical_HPIFindingsDetail', params);
    },

    HPIFindingsAdd: function () {

        var params = [];
        params["HPIFindingsId"] = null;
        params["mode"] = "Add";
        params["FromAdmin"] = Clinical_HPIFindings.params["FromAdmin"];
        if (Clinical_HPIFindings.params["FromAdmin"] == "0") {
            params["ParentCtrl"] = 'Clinical_HPIFindings';
            LoadActionPan('Clinical_HPIFindingsDetail', params, Clinical_HPIFindings.params.PanelID);
        }
        else
            LoadActionPan('Clinical_HPIFindingsDetail', params);

    },

    HPIFindingsActiveInactive: function (HPIFindingsId, IsActive, Name) {
        utility.myConfirm("3", function () {
            Clinical_HPIFindings.HPIFindingsActiveInactive_DBcall(HPIFindingsId, IsActive, Name).done(function (response) {

                if (response != "") {
                    response = JSON.parse(response);
                    if (response.status == true) {
                        utility.DisplayMessages(response.Message, 1);
                        Clinical_HPIFindings.searchHPIFindings();
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

    HPIFindingsActiveInactive_DBcall: function (HPIFindingsId, IsActive, Name) {
        var objData = new Object();
        objData["HPIFindingsId"] = HPIFindingsId;
        objData["IsActive"] = IsActive;
        objData["Name"] = Name;
        objData["commandType"] = "active_inactive_hpi_findings";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPIFindings");
    },

    UnLoadTab: function () {

        if (Clinical_HPIFindings.params["FromAdmin"] == "0") {

            if (Clinical_HPIFindings.params != null && Clinical_HPIFindings.params.ParentCtrl != null) {
                UnloadActionPan(Clinical_HPIFindings.params.ParentCtrl, 'Clinical_HPIFindings');
            }
            else
                UnloadActionPan(null, 'Clinical_HPIFindings');

        }
        else {
            RemoveAdminTab();
        }

    },

}