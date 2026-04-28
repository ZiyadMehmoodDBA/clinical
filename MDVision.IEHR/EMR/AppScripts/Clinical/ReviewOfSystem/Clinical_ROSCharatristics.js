Clinical_ROSCharatristics = {
    params: [],

    Load: function (params) {
        Clinical_ROSCharatristics.params = params;
        Clinical_ROSCharatristics.searchROSCharatristics();

        if (Clinical_ROSCharatristics.params.PanelID != "pnlAdminROSChatristics")
            Clinical_ROSCharatristics.params.PanelID = Clinical_ROSCharatristics.params.PanelID + " #pnlAdminROSChatristics";
        else
            Clinical_ROSCharatristics.params.PanelID = "pnlAdminROSChatristics";

    },

    searchROSCharatristics: function () {

        Clinical_ROSCharatristics.ROSCharatristicsGridLoad(1, 25);
    },

    ROSCharatristicsGridLoad: function (pageNo, rpp) {

        // Initialize Grid
        var grid = $("#" + Clinical_ROSCharatristics.params.PanelID + " #dgvROSCharatristics").kendoGrid({
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
             { title: "Action", width: "100px", template: '#=Clinical_ROSCharatristics.ActionPEObservation(data)#' },
             { title: "Characteristics", field: "Name", width: "100px" },
             { title: "Status", title: "Status", template: '#=Clinical_ROSCharatristics.setActionValue(data)#', width: "100px" }
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

                    var name = $('#' + Clinical_ROSCharatristics.params.PanelID + ' #ROSCharatristics #txtName').val();
                    var status = $('#' + Clinical_ROSCharatristics.params.PanelID + ' #ROSCharatristics #ddlStatus').val();

                    Clinical_ROSCharatristics.loadROSCharatristics_DBcall(status, name, e.data.page, e.data.pageSize).done(function (response) {
                        var data_ = { data: [], total: 0 };
                        if (response != "") {
                            var resposeData = JSON.parse(response);
                            if (resposeData.status != false && resposeData.ObservationCount > 0) {
                                if (resposeData.ObservationLoad_JSON != undefined)
                                    data_.data = jQuery.parseJSON(resposeData.ObservationLoad_JSON);
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
                        utility.removePaginationFromGrid($('#' + Clinical_ROSCharatristics.params.PanelID + " #dgvROSCharatristics"));
                        //End 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
                    });
                }
            }
        });
        grid.setDataSource(dataSource);
    },

    ActionPEObservation: function (data) {


        if (Clinical_ROSCharatristics.params.ParentCtrl == "Clinical_ROSSystemsDetail") {

            var Id_ = data.ROSCharacteristicsId;
            Clinical_ROSSystemsDetail.CharatristicsList.push(data);
            Clinical_ROSSystemsDetail.bIsTextChanged = true;
            var fillROSCharatristicsMethod = "Clinical_ROSSystemsDetail.fillROSCharatristics('" + Id_ + "')";

            return '<a class="btn  btn-xs" href="#" onclick="' + fillROSCharatristicsMethod + '" title="Select Record"><i class="fa fa-check black"></i></a>';
        }
        else {

            var deletePEObservationMethod = "Clinical_ROSCharatristics.ROSCharatristicsDelete('" + data.ROSCharacteristicsId + "')";
            var editPEObservationMethod = "Clinical_ROSCharatristics.ROSCharacteristicsEdit('" + data.ROSCharacteristicsId + "')";

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
            var activeInactivePEObservationMethod = "Clinical_ROSCharatristics.ROSCharatristicsActiveInactive('" + data.ROSCharacteristicsId + "', '" + isactive + "', '" + data.Name + "')";

            return '<a class="btn  btn-xs" href="#" onclick="' + deletePEObservationMethod + '" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn  btn-xs" href="javascript:;" onclick="' + editPEObservationMethod + '" title="Edit Observation"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="' + activeInactivePEObservationMethod + '" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>';
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

    loadROSCharatristics_DBcall: function (status, name, PageNumber, RowsPerPage) {
        var objData = new Object();
        objData["IsActive"] = status;
        objData["Name"] = name;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "load_reviewofsystem_charatristics";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemRMP", "ROSCharatristics");
    },

    ROSCharatristicsDelete: function (ROSCharacteristicsId) {
        utility.myConfirm("1", function () {
            var data = "ROSCharacteristicsId=" + ROSCharacteristicsId;
            Clinical_ROSCharatristics.ROSCharatristicsDelete_DBcall(ROSCharacteristicsId).done(function (response) {
                if (response != "") {
                    response = JSON.parse(response);
                    if (response.status == true) {
                        utility.DisplayMessages(response.Message, 1);
                        Clinical_ROSCharatristics.searchROSCharatristics();
                    }
                    else
                        utility.DisplayMessages(response.Message, 3);
                }
                else
                    utility.DisplayMessages(response.Message, 3);
            });
        }, function () { });

    },

    ROSCharatristicsDelete_DBcall: function (ROSCharacteristicsId) {
        var objData = new Object();
        objData["ROSCharacteristicsId"] = ROSCharacteristicsId;
        objData["commandType"] = "delete_reviewofsystem_charatristics";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemRMP", "ROSCharatristics");
    },

    ROSCharacteristicsEdit: function (ROSCharacteristicsId) {

        var params = [];
        params["ROSCharacteristicsId"] = ROSCharacteristicsId;
        params["mode"] = "Edit";
        params["FromAdmin"] = Clinical_ROSCharatristics.params["FromAdmin"];
        if (Clinical_ROSCharatristics.params["FromAdmin"] == "0") {
            params["ParentCtrl"] = 'Clinical_ROSCharatristics';
            LoadActionPan('Clinical_ROSCharatristicsDetail', params, Clinical_ROSCharatristics.params.PanelID);
        }
        else
            LoadActionPan('Clinical_ROSCharatristicsDetail', params);
    },

    PEObservationsAdd: function () {

        var params = [];
        params["ROSCharatristicId"] = null;
        params["mode"] = "Add";
        params["FromAdmin"] = Clinical_ROSCharatristics.params["FromAdmin"];
        if (Clinical_ROSCharatristics.params["FromAdmin"] == "0") {
            params["ParentCtrl"] = 'Clinical_ROSCharatristics';
            LoadActionPan('Clinical_ROSCharatristicsDetail', params, Clinical_ROSCharatristics.params.PanelID);
        }
        else
            LoadActionPan('Clinical_ROSCharatristicsDetail', params);

    },

    ROSCharatristicsActiveInactive: function (ROSCharacteristicsId, IsActive, Name) {
        utility.myConfirm("3", function () {
            Clinical_ROSCharatristics.ROSCharatristicsActiveInactive_DBcall(ROSCharacteristicsId, IsActive, Name).done(function (response) {

                if (response != "") {
                    response = JSON.parse(response);
                    if (response.status == true) {
                        utility.DisplayMessages(response.Message, 1);
                        Clinical_ROSCharatristics.searchROSCharatristics();
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

    ROSCharatristicsActiveInactive_DBcall: function (ROSCharacteristicsId, IsActive, Name) {
        var objData = new Object();
        objData["ROSCharacteristicsId"] = ROSCharacteristicsId;
        objData["IsActive"] = IsActive; /*   Name   */
        objData["Name"] = Name;
        objData["commandType"] = "active_inactive_reviewofsystem_charatristics";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemRMP", "ROSCharatristics");
    },

    UnLoadTab: function () {

        if (Clinical_ROSCharatristics.params["FromAdmin"] == "0") {

            if (Clinical_ROSCharatristics.params != null && Clinical_ROSCharatristics.params.ParentCtrl != null) {
                UnloadActionPan(Clinical_ROSCharatristics.params.ParentCtrl, 'Clinical_ROSCharatristics');
            }
            else
                UnloadActionPan(null, 'Clinical_ROSCharatristics');

        }
        else {
            RemoveAdminTab();
        }

    },

}
