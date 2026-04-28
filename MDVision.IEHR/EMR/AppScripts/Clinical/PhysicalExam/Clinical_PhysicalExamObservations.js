Clinical_PhysicalExamObservations = {
    params: [],

    Load: function (params) {
        Clinical_PhysicalExamObservations.params = params;
        Clinical_PhysicalExamObservations.searchPEObservations();

        if (Clinical_PhysicalExamObservations.params.PanelID != "pnlAdminPEObservations")
            Clinical_PhysicalExamObservations.params.PanelID = Clinical_PhysicalExamObservations.params.PanelID + " #pnlAdminPEObservations";
        else
            Clinical_PhysicalExamObservations.params.PanelID = "pnlAdminPEObservations";

    },

    searchPEObservations: function () {

        Clinical_PhysicalExamObservations.PEObservationsGridLoad(1, 25);
    },

    PEObservationsGridLoad: function (pageNo, rpp) {

        // Initialize Grid
        var grid = $("#" + Clinical_PhysicalExamObservations.params.PanelID + " #dgvPEObservations").kendoGrid({
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
             { title: "Action", width: "100px", template: '#=Clinical_PhysicalExamObservations.ActionPEObservation(data)#' },
             { title: "Observations", field: "Name", title: "Observations", width: "100px", encoded:false },
             { title: "Status", title: "Status", template: '#=Clinical_PhysicalExamObservations.setActionValue(data)#', width: "100px" }
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

                    var name = $('#' + Clinical_PhysicalExamObservations.params.PanelID + ' #PEObservations #txtName').val();
                    var status = $('#' + Clinical_PhysicalExamObservations.params.PanelID + ' #PEObservations #ddlStatus').val();

                    Clinical_PhysicalExamObservations.loadPEObservation_DBcall(status, name, e.data.page, e.data.pageSize).done(function (response) {
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
                        utility.removePaginationFromGrid($('#' + Clinical_PhysicalExamObservations.params.PanelID + " #dgvPEObservations"));
                        //End 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
                    });
                }
            }
        });
        grid.setDataSource(dataSource);
    },

    ActionPEObservation: function (data) {


        if (Clinical_PhysicalExamObservations.params.ParentCtrl == "Clinical_PhysicalExamSystemsDetail") {

            var Id_ = data.PEObservationId;
            Clinical_PhysicalExamSystemsDetail.ObservationsList.push(data);
            Clinical_PhysicalExamSystemsDetail.bIsTextChanged = true;
            var fillPEObservationMethod = "Clinical_PhysicalExamSystemsDetail.fillPEObservation('" + Id_ + "')";

            return '<a class="btn  btn-xs" href="#" onclick="' + fillPEObservationMethod + '" title="Select Record"><i class="fa fa-check black"></i></a>';
        }
        else {

            var deletePEObservationMethod = "Clinical_PhysicalExamObservations.PEObservationDelete('" + data.PEObservationId + "')";
            var editPEObservationMethod = "Clinical_PhysicalExamObservations.PEObservationEdit('" + data.PEObservationId + "')";

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
            var activeInactivePEObservationMethod = "Clinical_PhysicalExamObservations.PEObservationActiveInactive('" + data.PEObservationId + "', '" + isactive + "')";

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

    loadPEObservation_DBcall: function (status, name, PageNumber, RowsPerPage) {
        var objData = new Object();
        objData["IsActive"] = status;
        objData["Name"] = name;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "load_physicalexam_observation";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamECW", "PhysicalExamObservation");
    },

    PEObservationDelete: function (PEObservationId) {
        utility.myConfirm("1", function () {
            var data = "PEObservationId=" + PEObservationId;
            Clinical_PhysicalExamObservations.PEObservationDelete_DBcall(PEObservationId).done(function (response) {
                if (response != "") {
                    response = JSON.parse(response);
                    if (response.status == true) {
                        utility.DisplayMessages(response.Message, 1);
                        Clinical_PhysicalExamObservations.searchPEObservations();
                    }
                    else
                        utility.DisplayMessages(response.Message, 3);
                }
                else
                    utility.DisplayMessages(response.Message, 3);
            });
        }, function () { });

    },

    PEObservationDelete_DBcall: function (PEObservationId) {
        var objData = new Object();
        objData["PEObservationId"] = PEObservationId;
        objData["commandType"] = "delete_physicalexam_observation";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamECW", "PhysicalExamObservation");
    },

    PEObservationEdit: function (PEObservationId) {

        var params = [];
        params["PEObservationId"] = PEObservationId;
        params["mode"] = "Edit";
        params["FromAdmin"] = Clinical_PhysicalExamObservations.params["FromAdmin"];
        if (Clinical_PhysicalExamObservations.params["FromAdmin"] == "0") {
            params["ParentCtrl"] = 'Clinical_PhysicalExamObservations';
            LoadActionPan('Clinical_PhysicalExamObservationsDetail', params, Clinical_PhysicalExamObservations.params.PanelID);
        }
        else
            LoadActionPan('Clinical_PhysicalExamObservationsDetail', params);
    },

    PEObservationsAdd: function () {

        var params = [];
        params["PEObservationId"] = null;
        params["mode"] = "Add";
        params["FromAdmin"] = Clinical_PhysicalExamObservations.params["FromAdmin"];
        if (Clinical_PhysicalExamObservations.params["FromAdmin"] == "0") {
            params["ParentCtrl"] = 'Clinical_PhysicalExamObservations';
            LoadActionPan('Clinical_PhysicalExamObservationsDetail', params, Clinical_PhysicalExamObservations.params.PanelID);
        }
        else
            LoadActionPan('Clinical_PhysicalExamObservationsDetail', params);

    },

    PEObservationActiveInactive: function (PEObservationId, IsActive) {
        utility.myConfirm("3", function () {
            Clinical_PhysicalExamObservations.PEObservationActiveInactive_DBcall(PEObservationId, IsActive).done(function (response) {

                if (response != "") {
                    response = JSON.parse(response);
                    if (response.status == true) {
                        utility.DisplayMessages(response.Message, 1);
                        Clinical_PhysicalExamObservations.searchPEObservations();
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

    PEObservationActiveInactive_DBcall: function (PEObservationId, IsActive) {
        var objData = new Object();
        objData["PEObservationId"] = PEObservationId;
        objData["IsActive"] = IsActive;
        objData["commandType"] = "active_inactive_physicalexam_observation";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamECW", "PhysicalExamObservation");
    },

    UnLoadTab: function () {

        if (Clinical_PhysicalExamObservations.params["FromAdmin"] == "0") {

            if (Clinical_PhysicalExamObservations.params != null && Clinical_PhysicalExamObservations.params.ParentCtrl != null) {
                UnloadActionPan(Clinical_PhysicalExamObservations.params.ParentCtrl, 'Clinical_PhysicalExamObservations');
            }
            else
                UnloadActionPan(null, 'Clinical_PhysicalExamObservations');

        }
        else {
            RemoveAdminTab();
        }

    },
    // PRD-5 ,Dev By:MAhmad 
    resetAllFields:function(){
    
        $("#" + Clinical_PhysicalExamObservations.params.PanelID + " #txtName").val("");
        $("#" + Clinical_PhysicalExamObservations.params.PanelID + " #ddlStatus").find('option').each(function() {
           
            if ($(this).text().trim() == "- All -") {
                $(this).attr('selected', 'selected');
            }
        })
    }
    // PRD-5 ,Dev By:MAhmad 
}
