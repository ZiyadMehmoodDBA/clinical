Admin_CCMTemplates = {

    Load: function (params) {
        Admin_CCMTemplates.params = params;
        EMRUtility.SwicthWidgetInializatoin();
        // Care Plan Grid Load
        Admin_CCMTemplates.loadCCMTemplates(null, 'Care Plan Template', true, "", "").done(function (object) {
            Admin_CCMTemplates.CarePlanTemplateGridLoad(object);
        });
        // health risk assessment grid load
        Admin_CCMTemplates.loadCCMTemplates(null, 'Health Risk Assessment', true, "", "").done(function (resp) {
            Admin_CCMTemplates.healthRiskAssessmentGridLoad(resp);
        });
    },

    loadCCMTemplates: function (tempid, templookupname, IsActive, ShortName, Description) {
        var obj = new Object();
        obj["IsActive"] = IsActive;
        obj["ShortName"] = ShortName;
        obj["Description"] = Description;
        return Admin_CCMTemplates.CCMLoadTemplates(obj, tempid, templookupname, 1, 1000);
    },

    CCMLoadTemplates: function (dataObj, templateId, templateLookUpName, PageNumber, RowsPerPage) {
        dataObj = JSON.stringify(dataObj);
        var data = "CCMTemplateData=" + dataObj + "&TemplateID=" + templateId + "&TemplateLookupName=" + templateLookUpName + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        return MDVisionService.defaultService(data, "ADMIN_CCMTEMPLATES", "LOAD_CCMTEMPLATES");
    },

    CarePlanTemplateGridLoad: function (object) {
        if (object.status == false) {
            object = [];
        }
        else {
            object = object.CCMTemplateFill_JSON;
        }
        var data = new kendo.data.DataSource({
            data: object,
            pageSize: 15
        });
        $("#dgvcarePlanTemplate").kendoGrid({
            dataSource: data,
            //sortable: true,
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
        { title: "Action", width: "100px", template: '#=Admin_CCMTemplates.ActionCCMTemplates(data,\'Care Plan Template\')#' },
        { title: "Name", field: "ShortName", title: "Name", width: "100px" },
            { title: "Description", field: "Description", title: "Description", width: "100px" },
            { title: "Last Updated", template: '#=kendo.toString(new Date(ModifiedOn), "g")#', title: "Last updated", width: "100px" }
            ],
        });

        //Start 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
        utility.removePaginationFromGrid($('#' + Admin_CCMTemplates.params.PanelID + ' #dgvcarePlanTemplate'));
        //End 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
    },
    GetDateTime: function (date) {
       // Admin_CCMTemplates.GetDateTime(ModifiedOn)
        return date.replace('T',' ');
    },
    ActionCCMTemplates: function (data, type) {
        if (data.IsActive == true || data.IsActive == "True") {
            isactive = 0;
            activeTitle = "Active Record";
            tglclass = "fa fa-toggle-on green";
        }
        else {
            isactive = 1;
            activeTitle = "Inactive Record";
            tglclass = "fa fa-toggle-on red fa-rotate-180";

        }
        if (type == "Health Risk Assessment") {
            return '<a class="btn  btn-xs" href="#" onclick="Admin_CCMTemplates.TemplateDelete(\'' + data.TemplateId + '\',event,\'' + type + '\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_CCMTemplates.CCMTemplatesEdit(\'' + data.TemplateId + '\',\'' + type + '\',event);"  title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_CCMTemplates.TemplateActiveInactive(\'' + data.TemplateId + '\', ' + isactive + ',\'' + type + '\');" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>';
        }
        else {
            return '<a class="btn  btn-xs" href="#" onclick="Admin_CCMTemplates.TemplateDelete(\'' + data.TemplateId + '\',event,\'' + type + '\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_CCMTemplates.CCMTemplatesEdit(\'' + data.TemplateId + '\',\'' + type + '\',event);"  title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_CCMTemplates.TemplateActiveInactive(\'' + data.TemplateId + '\', ' + isactive + ',\'' + type + '\');" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>';
        }
    },

    //TemplateActiveInactive: function (TemplateId, isactive) {
    //    var data = "TemplateId=" + TemplateId + "&isactive=" + isactive;;
    //    return MDVisionService.defaultService(data, "ADMIN_CCMTEMPLATES", "ACTIVEINACTIVE_CCMTEMPLATE");
    //},

    TemplateActiveInactive: function (TemplateId, isactive, type) {
        if (event != null) {
            utility.myConfirm('3', function () {
                if (TemplateId == "" || TemplateId == "undefined") {
                }
                else {
                    Admin_CCMTemplates.customFormActiveInactive_Dbcall(TemplateId, isactive).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            if (type == "Health Risk Assessment")
                                Admin_CCMTemplates.searchHealthRiskAssessment();
                            else
                                Admin_CCMTemplates.searchCarePlan();
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            }, function () { },
                           '3', null, null, null, isactive
                        );
        }
    },
    customFormActiveInactive_Dbcall: function (TemplateId, isActive) {
        var objData = {};
        objData["TemplateId"] = TemplateId;
        objData["IsActive"] = isActive;

        objData["commandType"] = "CCM_TEMPLATE_ACTIVE_INACTIVE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CCMTemplate", "CCMTemplate");
    },




    healthRiskAssessmentGridLoad: function (object) {
        if (object.status == false) {
            object = [];
        }
        else {
            object = object.CCMTemplateFill_JSON;
        }
        var data = new kendo.data.DataSource({
            data: object,
            pageSize: 15
        });

        $("#dgvHealthRiskAssessment").kendoGrid({
            dataSource: data,
            //sortable: true,
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
            { title: "Action", width: "100px", template: '#=Admin_CCMTemplates.ActionCCMTemplates(data,\'Health Risk Assessment\')#' },
            { title: "Name", field: "ShortName", title: "Name", width: "100px" },
            { title: "Description", field: "Description", title: "Description", width: "100px" },
            { title: "Last Updated", template: '#=kendo.toString(new Date(ModifiedOn), "g")#', title: "Last updated", width: "100px" }
            ],
        });
        //Start 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
        utility.removePaginationFromGrid($('#' + Admin_CCMTemplates.params.PanelID + ' #dgvHealthRiskAssessment'));
        //End 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
    },

    searchCarePlan: function () {
        var name = $('.carePlanTemplate #txtName').val();
        var description = $('.carePlanTemplate #txtDescription').val();
        var switcher = $('.carePlanTemplate #switchActive');
        if ($(switcher).attr('IsActive') == '0') {
            Admin_CCMTemplates.loadCCMTemplates(null, 'Care Plan Template', false, name, description).done(function (object) {
                Admin_CCMTemplates.CarePlanTemplateGridLoad(object);
            });
        }
        else {
            Admin_CCMTemplates.loadCCMTemplates(null, 'Care Plan Template', true, name, description).done(function (object) {
                Admin_CCMTemplates.CarePlanTemplateGridLoad(object);
            });
        }
    },

    searchHealthRiskAssessment: function () {
        var name = $('.healthRiskAssessment #txtName').val();
        var description = $('.healthRiskAssessment #txtDescription').val();
        var switcher = $('.healthRiskAssessment #switchActive');
        if ($(switcher).attr('IsActive') == '0') {
            Admin_CCMTemplates.loadCCMTemplates(null, 'Health Risk Assessment', false, name, description).done(function (resp) {
                Admin_CCMTemplates.healthRiskAssessmentGridLoad(resp);
            });
        }
        else {
            Admin_CCMTemplates.loadCCMTemplates(null, 'Health Risk Assessment', true, name, description).done(function (resp) {
                Admin_CCMTemplates.healthRiskAssessmentGridLoad(resp);
            });
        }
    },

    changeSwitchCarePlan: function (objThis) {
        var IsActive = $(objThis).attr('IsActive');
        if (IsActive == '1') {
            // Care Plan Grid Load
            Admin_CCMTemplates.loadCCMTemplates(null, 'Care Plan Template', false, "", "").done(function (object) {
                Admin_CCMTemplates.CarePlanTemplateGridLoad(object);
            });
            $(objThis).attr('IsActive', '0');
        }
        else if (IsActive == '0') {
            Admin_CCMTemplates.loadCCMTemplates(null, 'Care Plan Template', true, "", "").done(function (object) {
                Admin_CCMTemplates.CarePlanTemplateGridLoad(object);
            });
            $(objThis).attr('IsActive', '1');
        }
    },

    changeSwitchHealthRisk: function (objThis) {
        var IsActive = $(objThis).attr('IsActive');
        if (IsActive == '1') {
            Admin_CCMTemplates.loadCCMTemplates(null, 'Health Risk Assessment', false, "", "").done(function (object) {
                Admin_CCMTemplates.healthRiskAssessmentGridLoad(object);
            });
            $(objThis).attr('IsActive', '0');
        }
        else if (IsActive == '0') {
            Admin_CCMTemplates.loadCCMTemplates(null, 'Health Risk Assessment', true, "", "").done(function (object) {
                Admin_CCMTemplates.healthRiskAssessmentGridLoad(object);
            });
            $(objThis).attr('IsActive', '1');
        }
    },

    CCMTemplatesEdit: function (TemplateId, TemplateType) {
        var params = [];
        params["ParentCtrl"] = "Admin_CCMTemplates";
        params["mode"] = "Edit";
        params["TemplateId"] = TemplateId;
        params["TemplateType"] = TemplateType;
        LoadActionPan("Admin_CCMTemplateDetails", params);
    },

    Admin_CMTemplareDelete: function (TemplateId) {
        //var params = [];
        //params["ParentCtrl"] = "Admin_CCMTemplates";
        //params["mode"] = "Add";
        //params["FromAdmin"] = Admin_CCMTemplates.params["FromAdmin"];
        //if (Admin_CCMTemplates.params["FromAdmin"] == "0") {
        //    params["ParentCtrl"] = 'Admin_CCMTemplates';
        //}
        //LoadActionPan("Admin_CCMTemplateDetails", params);
    },

    CCMTemplatesAdd: function () {
        var TemplateType = "Care Plan Template";
        if ($('#lihealthRiskAssessment').hasClass('active'))
            TemplateType = "Health Risk Assessment";
        var params = [];
        params["ParentCtrl"] = "Admin_CCMTemplates";
        params["mode"] = "Add";
        params["TemplateType"] = TemplateType;
        LoadActionPan("Admin_CCMTemplateDetails", params);
    },
    TemplateDelete: function (TemplateId, event, type) {
        utility.myConfirm('1', function () {
            if (TemplateId && TemplateId > 0) {
                Admin_CCMTemplates.deleteTemplate_DBCall(TemplateId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        if (type == "Health Risk Assessment")
                            Admin_CCMTemplates.searchHealthRiskAssessment();
                        else
                            Admin_CCMTemplates.searchCarePlan();

                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        },
        function () { },
        '1'
        );
    },
    deleteTemplate_DBCall: function (TemplateId) {
        var objData = new Object();
        objData["commandType"] = "DELETE_CCM_TEMPLATE";
        objData["TemplateId"] = TemplateId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CCMTemplate", "CCMTemplate");

    },
}