AOETemplate = {
    params: [],
    PanelID: "pnlAOETemplate",
    Load: function (params) {
        $('#adminTabAOETemplate').attr('title', 'AOE Templates');
        $('#adminTabAOETemplate').text('AOE Templates');
        EMRUtility.SwicthWidgetInializatoin();
        var switcher = $('#' + AOETemplate.PanelID + ' #switchActive');

        $(switcher).attr('IsActive', '0');
        $('.ios-switch').attr('class', 'ios-switch on');
        AOETemplate.changeSwitch(switcher[0]);

    },
    OpenAOETemplateDetail: function() {
        var params = [];
        LoadActionPan("AOETemplateDetail", params);
    },
    LoadAOETemplates: function(IsActive) {
        var obj = new Object();
        obj["IsActive"] = IsActive;
        obj["commandType"] = "Load_AOETemplates";
        var data = JSON.stringify(obj);
        return MDVisionService.APIService(data, "AOETemplate", "AOETemplate");
    },
    AOETemplateAddEdit: function (AOETemplateId) {
                var params = [];
                if (AOETemplateId != null && parseInt(AOETemplateId) > 0) {
                    params["AOETemplateId"] = AOETemplateId;
                    params["mode"] = "Edit";
                }
                else {
                    params["AOETemplateId"] = -1;
                    params["mode"] = "Add";
                }
                params["FromAdmin"] = AOETemplate.params["FromAdmin"];
                params["ParentCtrl"] = 'adminTabAOETemplate';
                LoadActionPan('AOETemplateDetail', params);
    },
    loadAOETemplates: function () {
        var switcher = $('#' + AOETemplate.PanelID + ' #switchActive');
        if ($(switcher).attr('IsActive') == '0') {
            AOETemplate.LoadAOETemplates("1").done(function (response) {
                AOETemplate.AOETemplatesGridLoad(response);
            });
        }
        else if ($(switcher).attr('IsActive') == '1') {
            AOETemplate.LoadAOETemplates("0").done(function (response) {
                AOETemplate.AOETemplatesGridLoad(response);
            });
        }
    },
    changeSwitch: function (objThis) {
        var IsActive = $(objThis).attr('IsActive');
        if (IsActive == '1') {
            AOETemplate.loadAOETemplates();
            $(objThis).attr('IsActive', '0');
        }
        else if (IsActive == '0') {
            AOETemplate.loadAOETemplates();
            $(objThis).attr('IsActive', '1');
        }
    },
    ActionAOETemplates: function (data) {
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
        return '<a class="btn  btn-xs" href="#" onclick="AOETemplate.AOETemplateDelete(' + data.AOETemplateId + ', this);" ' +
            ' title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" ' +
            ' onclick="AOETemplate.AOETemplateAddEdit(' + data.AOETemplateId + ');" ' +
            ' title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="AOETemplate.AOETemplateActiveInactive(\'' + data.AOETemplateId + '\', ' + isactive + ').done(function () { Admin_CCMCareTeam.CCMCareTeamsLoad(); });" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>';
    },
    AOETemplateActiveInactive: function (AOETemplateId, isactive) {
        utility.myConfirm("3", function () {
            var objData = new Object();
            objData["AOETemplateId"] = AOETemplateId;
            objData["IsActive"] = isactive;
            objData["commandType"] = "AOETemplate_ActiveInactive";
            var data = JSON.stringify(objData);
            MDVisionService.APIService(data, "AOETemplate", "AOETemplate").done(function () {
                AOETemplate.LoadAOETemplates(isactive == 0 ? 1 : 0).done(function (response) {
                    AOETemplate.AOETemplatesGridLoad(response);
                });
            });
        }, function () { },
                    "3", null, null, null, isactive
                );
    },


    AOETemplateDelete: function (TemplateId,obj) {
            utility.myConfirm('Are you sure to delete template?', function () {
                AOETemplate.DeleteAOETemplate(TemplateId).done(function (response) {
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
    DeleteAOETemplate: function (TemplateId) {
        var objData = new Object();
        objData["AOETemplateId"] = TemplateId;
        objData["commandType"] = "DELETE_AOETemplate";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "AOETemplate", "AOETemplate");
    },
    AOETemplatesGridLoad: function (object) {
        object = JSON.parse(object);
        if (object.status == false) {
            object = [];
        }
        else {
            object = object.AOETemplates;
        }
        var data = new kendo.data.DataSource({
            data: object,
            pageSize: 15
        });
        $("#dgvAOETemplates").kendoGrid({
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
            { title: "Action", width: "100px", template: '#=AOETemplate.ActionAOETemplates(data)#' },
            { title: "Template Name", field: "Name", width: "100px" },
            { title: "Speciality", field: "SpecialityNames", width: "300px" },
            { title: "Providers", field: "ProviderNames", width: "300px" },
            { title: "Test", field: "CPTCodeDescription", width: "100px" },
            { title: "Laboratory", field: "LabName", width: "100px" },
            { title: "Last Updated", field: "LastUpdated", width: "100px" }
            ],
        });

        //Start 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
        utility.removePaginationFromGrid($('#' + AOETemplate.PanelID + ' #dgvAOETemplates'));
        //End 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
    },
    Unload: function () { }
}
    