ProcedureTemplate = {
    params: [],
    PanelID: "pnlProcedureTemplate",
    Load: function (params) {
        $('#adminTabProcedureTemplate').attr('title', 'Procedure Templates');
        $('#adminTabProcedureTemplate').text('Procedure Templates');
        EMRUtility.SwicthWidgetInializatoin();
        var switcher = $('#' + ProcedureTemplate.PanelID + ' #switchActive');

        $(switcher).attr('IsActive', '0');
        $('.ios-switch').attr('class', 'ios-switch on');
        ProcedureTemplate.changeSwitch(switcher[0]);

    },
    OpenProcedureTemplateDetail: function () {
        var params = [];
        LoadActionPan("ProcedureTemplateDetail", params);
    },
    LoadProcedureTemplates: function (IsActive) {
        var obj = new Object();
        obj["IsActive"] = IsActive;
        obj["commandType"] = "Load_ProcedureTemplates";
        var data = JSON.stringify(obj);
        return MDVisionService.APIService(data, "ProcedureTemplate", "ProcedureTemplate");
    },
    ProcedureTemplateAddEdit: function (ProcedureTemplateId) {
        var params = [];
        if (ProcedureTemplateId != null && parseInt(ProcedureTemplateId) > 0) {
            params["ProcedureTemplateId"] = ProcedureTemplateId;
            params["mode"] = "Edit";
        }
        else {
            params["ProcedureTemplateId"] = -1;
            params["mode"] = "Add";
        }
        params["FromAdmin"] = ProcedureTemplate.params["FromAdmin"];
        params["ParentCtrl"] = 'adminTabProcedureTemplate';
        LoadActionPan('ProcedureTemplateDetail', params);
    },
    loadProcedureTemplates: function () {
        var switcher = $('#' + ProcedureTemplate.PanelID + ' #switchActive');
        if ($(switcher).attr('IsActive') == '0') {
            ProcedureTemplate.LoadProcedureTemplates("1").done(function (response) {
                ProcedureTemplate.ProcedureTemplatesGridLoad(response);
            });
        }
        else if ($(switcher).attr('IsActive') == '1') {
            ProcedureTemplate.LoadProcedureTemplates("0").done(function (response) {
                ProcedureTemplate.ProcedureTemplatesGridLoad(response);
            });
        }
    },
    changeSwitch: function (objThis) {
        var IsActive = $(objThis).attr('IsActive');
        if (IsActive == '1') {
            ProcedureTemplate.loadProcedureTemplates();
            $(objThis).attr('IsActive', '0');
        }
        else if (IsActive == '0') {
            ProcedureTemplate.loadProcedureTemplates();
            $(objThis).attr('IsActive', '1');
        }
    },
    ActionProcedureTemplates: function (data) {
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
        return '<a class="btn  btn-xs" href="#" onclick="ProcedureTemplate.ProcedureTemplateDelete(' + data.ProcedureTemplateId + ', this);" ' +
            ' title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" ' +
            ' onclick="ProcedureTemplate.ProcedureTemplateAddEdit(' + data.ProcedureTemplateId + ');" ' +
            ' title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="ProcedureTemplate.ProcedureTemplateActiveInactive(\'' + data.ProcedureTemplateId + '\', ' + isactive + ').done(function () { Admin_CCMCareTeam.CCMCareTeamsLoad(); });" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>';
    },
    ProcedureTemplateActiveInactive: function (ProcedureTemplateId, isactive) {
        utility.myConfirm("3", function () {
            var objData = new Object();
            objData["ProcedureTemplateId"] = ProcedureTemplateId;
            objData["IsActive"] = isactive;
            objData["commandType"] = "ProcedureTemplate_ActiveInactive";
            var data = JSON.stringify(objData);
            MDVisionService.APIService(data, "ProcedureTemplate", "ProcedureTemplate").done(function () {
                ProcedureTemplate.LoadProcedureTemplates(isactive == 0 ? 1 : 0).done(function (response) {
                    ProcedureTemplate.ProcedureTemplatesGridLoad(response);
                });
            });
        }, function () { }, "3", null, null, null, isactive );
    },


    ProcedureTemplateDelete: function (TemplateId, obj) {
        utility.myConfirm('Are you sure to delete template?', function () {
            ProcedureTemplate.DeleteProcedureTemplate(TemplateId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.message, 1);
                    $(obj).parent().parent().remove();
                }
                else {
                    utility.DisplayMessages(response.message, 3);
                }
            });
        }, function () { },
              'Confirm Delete'
          );

    },
    DeleteProcedureTemplate: function (TemplateId) {
        var objData = new Object();
        objData["ProcedureTemplateId"] = TemplateId;
        objData["commandType"] = "DELETE_ProcedureTemplate";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ProcedureTemplate", "ProcedureTemplate");
    },
    ProcedureTemplatesGridLoad: function (object) {
        object = JSON.parse(object);
        if (object.status == false) {
            object = [];
        }
        else {
            object = object.ProcedureTemplates;
        }
        var data = new kendo.data.DataSource({
            data: object,
            pageSize: 15
        });
        $("#dgvProcedureTemplates").kendoGrid({
            dataSource: data,
            resizable: true,
            scrollable: false,
            noRecords: true,
            messages: {
                noRecords: "No Template Found."
            },
            pageable: {
                refresh: true,
                pageSizes: [5, 10, 20, 50, 100],
                buttonCount: 5
            },
            columns: [
            { title: "Action", width: "100px", template: '#=ProcedureTemplate.ActionProcedureTemplates(data)#' },
            { title: "Template Name", field: "Name", width: "100px" },
            { title: "Providers", field: "ProviderNames", width: "200px" },
            { title: "Procedure", field: "TempProcedures", width: "200px" },
            { title: "Last Updated", field: "LastUpdated", width: "100px" }
            ],
        });

        //Start 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
        utility.removePaginationFromGrid($('#' + ProcedureTemplate.PanelID + ' #dgvProcedureTemplates'));
        //End 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
    },
    Unload: function () { }
}
