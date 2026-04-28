Clinical_HPISymptoms = {
    params: [],

    Load: function (params) {
        Clinical_HPISymptoms.params = params;

        //var self = $('#' + Clinical_HPISymptoms.params.PanelID);
        //if (Clinical_HPISymptoms.bIsFirstLoad == true) {
        //    self.loadDropDowns(true);
        //}

        Clinical_HPISymptoms.searchHPISymptoms();
    },

    searchHPISymptoms: function () {

        Clinical_HPISymptoms.HPISymptomsGridLoad(1, 25);
    },

    loadHPISymptoms_DBcall: function (IsActive, ShortName, PageNumber, RowsPerPage) {
        var objData = new Object();
        objData["IsActive"] = IsActive;
        objData["Name"] = ShortName;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "load_hpi_symptoms";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPISymptoms");
    },

    HPISymptomsGridLoad: function (pageNo, rpp) {

        // Initialize Grid
        var grid = $("#pnlAdminHPISymptoms #dgvHPISymptoms").kendoGrid({
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
                { title: "Action", width: "100px", template: '#=Clinical_HPISymptoms.ActionHPISymptoms(data)#' },
                { title: "Symptoms", field: "Name", title: "Symptoms", width: "100px" },
                { title: "Status", title: "Status", template: '#=Clinical_HPISymptoms.setActionValue(data)#', width: "100px" }
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

                    var name = $('#pnlAdminHPISymptoms #frmSearchHPISymptoms #txtName').val();
                    var status = $('#pnlAdminHPISymptoms #frmSearchHPISymptoms #ddlStatus').val();

                    Clinical_HPISymptoms.loadHPISymptoms_DBcall(status, name, e.data.page, e.data.pageSize).done(function (response) {
                        var data_ = { data: [], total: 0 };
                        if (response != "") {
                            var resposeData = JSON.parse(response);
                            if (resposeData.status != false && resposeData.SymptomsCount > 0) {
                                if (resposeData.listSymptoms)
                                    data_.data = resposeData.listSymptoms;
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
                        utility.removePaginationFromGrid($("#pnlAdminHPISymptoms #dgvHPISymptoms"));
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

    ActionHPISymptoms: function (data) {

        var deleteHPISymptomsMethod = "Clinical_HPISymptoms.HPISymptomsDelete('" + data.HPISymptomsId + "')";
        var editHPISymptomsMethod = "Clinical_HPISymptoms.HPISymptomsEdit('" + data.HPISymptomsId + "')";

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
        var activeInactiveHPISymptomsMethod = "Clinical_HPISymptoms.HPISymptomsActiveInactive('" + data.HPISymptomsId + "', '" + isactive + "', '" + data.Name + "')";


        //return '<a class="btn  btn-xs" href="javascript:;" onclick="' + editHPISymptomsMethod + '" title="Edit ICD Group"><i class="fa fa-edit black"></i></a>';
        return '<a class="btn  btn-xs" href="#" onclick="' + deleteHPISymptomsMethod + '" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn  btn-xs" href="javascript:;" onclick="' + editHPISymptomsMethod + '" title="Edit Symptom"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="' + activeInactiveHPISymptomsMethod + '" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>';
    },

    HPISymptomsEdit: function (HPISymptomsId) {

        var params = [];
        params["HPISymptomsId"] = HPISymptomsId;
        params["mode"] = "Edit";
        params["FromAdmin"] = Clinical_HPISymptoms.params["FromAdmin"];
        if (Clinical_HPISymptoms.params["FromAdmin"] == "0") {
            params["ParentCtrl"] = 'adminTabHPISymptoms';
        }

        LoadActionPan('Clinical_HPISymptomsDetail', params);
    },

    HPISymptomsDelete: function (HPISymptomsId) {
        utility.myConfirm("1", function () {
            var data = "HPISymptomsId=" + HPISymptomsId;
            Clinical_HPISymptoms.HPISymptomsDelete_DBcall(HPISymptomsId).done(function (response) {
                if (response != "") {
                    response = JSON.parse(response);
                    if (response.status == true) {
                        utility.DisplayMessages(response.Message, 1);
                        Clinical_HPISymptoms.searchHPISymptoms();
                    }
                    else
                        utility.DisplayMessages(response.Message, 3);
                }
                else
                    utility.DisplayMessages(response.Message, 3);
            });
        }, function () { });

    },

    HPISymptomsDelete_DBcall: function (HPISymptomsId) {
        var objData = new Object();
        objData["HPISymptomsId"] = HPISymptomsId;
        objData["commandType"] = "delete_hpi_symptoms";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPISymptoms");
    },

    HPISymptomsActiveInactive: function (HPISymptomsId, IsActive, Name) {
        utility.myConfirm("3", function () {
            Clinical_HPISymptoms.HPISymptomsActiveInactive_DBcall(HPISymptomsId, IsActive, Name).done(function (response) {

                if (response != "") {
                    response = JSON.parse(response);
                    if (response.status == true) {
                        utility.DisplayMessages(response.Message, 1);
                        Clinical_HPISymptoms.searchHPISymptoms();
                    }
                    else
                        utility.DisplayMessages(response.Message, 3);
                }
                else
                    utility.DisplayMessages(response.Message, 3);
            });
        }, function () { },"3", null, null, null, IsActive);
    },

    HPISymptomsActiveInactive_DBcall: function (HPISymptomsId, IsActive, Name) {
        var objData = new Object();
        objData["HPISymptomsId"] = HPISymptomsId;
        objData["IsActive"] = IsActive;
        objData["Name"] = Name;
        objData["commandType"] = "active_inactive_hpi_symptoms";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPISymptoms");
    },

    HPISymptomsAdd: function () {
        var strMessage = "";
        if (strMessage == "") {
            var params = [];
            params["HPISymptomsId"] = null;
            params["mode"] = "Add";
            params["FromAdmin"] = Clinical_HPISymptoms.params["FromAdmin"];
            if (Clinical_HPISymptoms.params["FromAdmin"] == "0") {
                params["ParentCtrl"] = 'adminTabHPISymptoms';
                LoadActionPan('Clinical_HPISymptomsDetail', params, Clinical_HPISymptoms.params.PanelID);
            }
            else {
                LoadActionPan('Clinical_HPISymptomsDetail', params);
            }

        }
    },

    UnLoadTab: function () {

        if (Clinical_HPISymptoms.params["FromAdmin"] == "0") {

            if (Clinical_HPISymptoms.params != null && Clinical_HPISymptoms.params.ParentCtrl != null && Clinical_HPISymptoms.params.PanelID != 'pnlAdminHPISymptoms') {
                UnloadActionPan(Clinical_HPISymptoms.params.ParentCtrl, 'Clinical_HPISymptoms', null, Clinical_HPISymptoms.params.PanelID);
            }
            else if (Clinical_HPISymptoms.params != null && Clinical_HPISymptoms.params.ParentCtrl != null) {
                UnloadActionPan(Clinical_HPISymptoms.params.ParentCtrl, 'Clinical_HPISymptoms');
            }
            else
                UnloadActionPan(null, 'Clinical_HPISymptoms');

        }
        else {
            RemoveAdminTab();
        }

    },
}
