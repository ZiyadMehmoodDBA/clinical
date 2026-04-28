Admin_CCMCareTeam = {
    params: [],
    Load: function (params) {
        Admin_CCMCareTeam.params = params;
        EMRUtility.SwicthWidgetInializatoin();
        Admin_CCMCareTeam.loadCareTeams();
   
    },

    CCMCareTeamsLoad: function () {

        Admin_CCMCareTeam.loadCCMCareTeam(null, true, "", "").done(
        function (resp) {
        Admin_CCMCareTeam.CCMCareTeamGridLoad(resp);
        });
    },

    loadCCMCareTeam: function (CareTeamId, IsActive, ShortName, ProviderName) {
        var obj = new Object();
        obj["IsActive"] = IsActive;
        obj["ShortName"] = ShortName;
        obj["ProviderName"] = ProviderName;

        return Admin_CCMCareTeam.CCMLoadCareTeams(obj, CareTeamId, 1, 1000);
    },

    CCMLoadCareTeams: function (dataObj, CareTeamId, PageNumber, RowsPerPage) {
        dataObj = JSON.stringify(dataObj);
        var data = "CCMCareTeamData=" + dataObj + "&CareTeamID=" + CareTeamId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        return MDVisionService.defaultService(data, "ADMIN_CCMCARETEAM", "LOAD_CCMCARETEAM");
    },

    CCMCareTeamGridLoad: function (object) {
        if (object.status == false) {
            object = [];
        }
        else {
            object = JSON.parse(object.CCMCareTeamFill_JSON);
        }
        var data = new kendo.data.DataSource({
            data: object,
            pageSize: 15
        });
        $("#dgvCCMCareTeam").kendoGrid({
            dataSource: data,
            resizable: true,
            scrollable: false,
            noRecords: true,
            messages: {
                noRecords: "No Care Team Found."
            },
            pageable: {
                refresh: true,
                pageSizes: [5, 10, 20, 50, 100],
                buttonCount: 5
            },
            columns: [
            { title: "Action", width: "100px", template: '#=Admin_CCMCareTeam.ActionCCMCareTeams(data)#' },
            { title: "Name", field: "ShortName", title: "Name", width: "100px" },
            { title: "Provider Name", field: "ProviderName", width: "100px" },
            { title: "Care Manager", field: "CareManagerName", width: "100px" },
            { title: "Care Coordinator", field: "CareCoordinatorName", width: "100px" },
            { title: "Care Giver", field: "CareGiverName", width: "100px" },
            { title: "Last Updated", field: "ModifiedOn", width: "100px" }
            ],
        });
        //Start 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
        utility.removePaginationFromGrid($('#' + Admin_CCMCareTeam.params.PanelID + ' #dgvCCMCareTeam'));
        //End 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
    },

    ActionCCMCareTeams: function (data) {
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
        return '<a class="btn  btn-xs" href="#" onclick="Admin_CCMCareTeam.deleteCCMCareTeam(' + data.CareTeamId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_CCMCareTeam.CareTeamEdit(' + data.CareTeamId + ',' + isactive + ',event);"  title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_CCMCareTeam.CareTeamActiveInactive(\'' + data.CareTeamId + '\', ' + isactive + ').done(function () { Admin_CCMCareTeam.CCMCareTeamsLoad(); });" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>';
    },

    CareTeamActiveInactive: function (CareTeamId, isactive) {
        utility.myConfirm("3", function () {
            var data = "CareTeamId=" + CareTeamId + "&isactive=" + isactive;;
            MDVisionService.defaultService(data, "ADMIN_CCMCARETEAM", "ACTIVEINACTIVE_CCMCARETEAM").done(function () {

                Admin_CCMCareTeam.loadCareTeams();
            });
        }, function () { },
                    "3", null, null, null, isactive
                );
    },
    loadCareTeams: function () {
        var switcher = $('#CCMCareTeam #switchActive');
        if ($(switcher).attr('IsActive') == '0') {
            Admin_CCMCareTeam.loadCCMCareTeam(null, false, "", "").done(
            function (resp) {
            Admin_CCMCareTeam.CCMCareTeamGridLoad(resp);
         });
        }
        else if ($(switcher).attr('IsActive') == '1') {
            Admin_CCMCareTeam.loadCCMCareTeam(null, true, "", "").done(
            function (resp) {
            Admin_CCMCareTeam.CCMCareTeamGridLoad(resp);
           });
        }
    },
    CareTeamEdit: function (Id, IsActive) {
        params = [];
        params["Id"] = Id;
        if (IsActive == 1) {
            IsActive = false;
        }
        else {
            IsActive = true;
        }
        params["IsActive"] = IsActive;
        params["mode"] = "Edit";
        LoadActionPan('Admin_CCMCareTeamDetail', params);
    },

    changeSwitch: function (objThis) {
        var IsActive = $(objThis).attr('IsActive');
        if (IsActive == '1') {
            Admin_CCMCareTeam.loadCCMCareTeam(null, false, "", "").done(
               function (resp) {
                   Admin_CCMCareTeam.CCMCareTeamGridLoad(resp);
               });
            $(objThis).attr('IsActive', '0');
        }
        else if (IsActive == '0') {
            Admin_CCMCareTeam.loadCCMCareTeam(null, true, "", "").done(
           function (resp) {
               Admin_CCMCareTeam.CCMCareTeamGridLoad(resp);
           });
            $(objThis).attr('IsActive', '1');
        }
    },

    searchCCMCareTeam: function () {
        var name = $('#CCMCareTeam #txtName').val();
        var provider = $('#CCMCareTeam #txtProvider').val();
        var switcher = $('#CCMCareTeam #switchActive');
        if ($(switcher).attr('IsActive') == '0') {
            Admin_CCMCareTeam.loadCCMCareTeam(null, false, name, provider).done(
              function (resp) {
                  Admin_CCMCareTeam.CCMCareTeamGridLoad(resp);
              });
        }
        else {
            Admin_CCMCareTeam.loadCCMCareTeam(null, true, name, provider).done(
             function (resp) {
                 Admin_CCMCareTeam.CCMCareTeamGridLoad(resp);
             });
        }
    },

    deleteCCMCareTeam: function (CareTeamId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.myConfirm("1", function () {
            var data = "CareTeamID=" + CareTeamId;
            MDVisionService.defaultService(data, "ADMIN_CCMCARETEAM", "DELETE_CCMCARETEAM").done(function (resp) {
                if (resp.status == true) {
                    Admin_CCMCareTeam.loadCareTeams();
                    utility.DisplayMessages(resp.Message, 1);
                }
                else if (resp.status == false) {
                    utility.DisplayMessages(resp.Message, 2);
                }
            });
        }, function () { });

   
    },

    loadCCMCareTeamDetail: function () {
        var params = [];
        params["mode"] = "Add";
        LoadActionPan('Admin_CCMCareTeamDetail', params);
    },
   // AST-332
    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}
