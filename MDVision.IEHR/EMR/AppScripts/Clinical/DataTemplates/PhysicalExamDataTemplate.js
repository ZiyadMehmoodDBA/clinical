PhysicalExamDataTemplate = {

    //Author: Abid Ali
    //Date: 13-06-2016
    //This file will handle all actions performed for Physiacal Exam Data Template
    bIsFirstLoad: true,
    params: [],
    FromAdmin: '0',
    ActionBit: false,
    Switch: 1,
    Load: function (params) {
        PhysicalExamDataTemplate.params = params;
        if (PhysicalExamDataTemplate.params.PanelID != 'pnlPhysicalExamDataTemplate') {
            PhysicalExamDataTemplate.params.PanelID = PhysicalExamDataTemplate.params.PanelID + ' #pnlPhysicalExamDataTemplate';
        } else {
            PhysicalExamDataTemplate.params.PanelID = 'pnlPhysicalExamDataTemplate';
        }

        if (PhysicalExamDataTemplate.bIsFirstLoad) {
            PhysicalExamDataTemplate.bIsFirstLoad = false;
        }
        PhysicalExamDataTemplate.loadPhysExamDataTemplate();
        PhysicalExamDataTemplate.domReady();
    },

    domReady:function(){
        (function ($) {
            'use strict';
            $(function () {
                $("#" + PhysicalExamDataTemplate.params.PanelID + ' [data-plugin-ios-switch]').each(function () {
                    var $this = $(this);

                    $this.themePluginIOS7Switch();
                });
            });
        }).apply(this, [jQuery]);
    },

    //Author: Abid Ali
    //Date: 31-03-2016
    //Description:  Searches active/inactive list
    activeDataTemplateSearch: function (obj) {
        var isactive = $(obj).attr('isactive');

        if (isactive == '1') {
            $(obj).attr('isactive', '0');
            PhysicalExamDataTemplate.Switch = 0;
        }
        else if (isactive == '0') {
            $(obj).attr('isactive', '1');
            PhysicalExamDataTemplate.Switch = 1;
        }

        PhysicalExamDataTemplate.loadPhysExamDataTemplate();
    },

    PhysicalExamDataTemplateAddEdit: function (templateId, caller, dataTemplateId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Clinical_Data Template_Physical Exam", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                if (templateId != null && parseInt(templateId) > 0) {
                    params["TemplateId"] = templateId;
                    params["mode"] = "Edit";
                }
                else {
                    params["TemplateId"] = -1;
                    params["mode"] = "Add";
                }
                params["FromAdmin"] = '0';
                params["DataTemplateId"] = dataTemplateId;
                params["TemplateId"] = templateId;

                params["ParentCtrl"] = 'adminTabPhysicalExamDataTemplate';
                LoadActionPan('PhysicalExamDataTemplateDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    loadPhysExamDataTemplate: function (dataTemplateId, PageNo, rpp) {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Clinical_Data Template_Physical Exam", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#" + PhysicalExamDataTemplate.params["PanelID"] + " #pnlPhysicalExamDataTemplate_Result").css("display") == "none") {
                    $("#" + PhysicalExamDataTemplate.params["PanelID"] + " #pnlPhysicalExamDataTemplate_Result").show();
                }

                PhysicalExamDataTemplate.searchPhysExamDataTemplate(null, dataTemplateId, PageNo, rpp).done(function (response) {

                    response = JSON.parse(response);
                    PhysicalExamDataTemplate.dataTemplateGridLoad(response);

                    if (response.status != false) {                        
                        
                        $("#" + PhysicalExamDataTemplate.params["PanelID"] + " #dgvPhysicalExamDataTemplate_Paging").css("display", "inline");
                       
                        var TableControl = PhysicalExamDataTemplate.params.PanelID + " #pnlPhysicalExamDataTemplate_Result #dgvPhysicalExamDataTemplate_Paging";
                        var PagingPanelControlID = PhysicalExamDataTemplate.params.PanelID + " #dgvPhysicalExamDataTemplate_Paging";
                        var ClassControlName = "PhysicalExamDataTemplate";
                        var PagesToDisplay = 15;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        //Paging control
                        setTimeout(
                            CreatePagination(response.dataTemplateCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                                PhysicalExamDataTemplate.loadPhysExamDataTemplate(PrimaryID, PageNumber, ResultPerPage);
                            }), 10);
                        }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },


    dataTemplateGridLoad: function (response) {

        var $pagingControl = $('#' +PhysicalExamDataTemplate.params.PanelID + " #dgvPhysicalExamDataTemplate_Paging");

        $("#" + PhysicalExamDataTemplate.params["PanelID"] + " #pnlPhysicalExamDataTemplate_Result #dgvPhysicalExamDataTemplate").dataTable().fnDestroy();
        $("#" + PhysicalExamDataTemplate.params["PanelID"] + " #pnlPhysicalExamDataTemplate_Result #dgvPhysicalExamDataTemplate tbody").find("tr").remove();
        //response.PhysExamDataTemplateCount
       
        if (response.status == true) {
            var dataTemplateLoadJSONData = response;
            $.each(JSON.parse(dataTemplateLoadJSONData.PhysExamDataTemplateFill_JSON), function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", "gvPhysExamDataTemplate_row" + item.DataTemplateId);
                $row.attr("DataTemplateId", item.DataTemplateId);
                
                if (item.IsActive == "True") {
                    isactive = 0;
                    activeRecord = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    activeRecord = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }

                // var MethodMode = ""// PhysicalExamDataTemplate.getPatientSelectMethodName(item.DataTemplateId, item.AccountNumber, item.FullName, item.FirstName, item.LastName);


                var DeleteMethod = "PhysicalExamDataTemplate.PhysExamDataTemplateDelete('" + item.DataTemplateId.trim() + "',event);";
                var EditMethod = "PhysicalExamDataTemplate.PhysicalExamDataTemplateAddEdit('" + item.TemplateId.trim() + "','Edit','" + item.DataTemplateId.trim() + "')";
                var ActiveInacvtiveMethod = "PhysicalExamDataTemplate.physExamDataTemplateActiveInactive('" + item.DataTemplateId.trim() + "'," + isactive + ",event);";
                var RowEditMethod = "PhysicalExamDataTemplate.PhysicalExamDataTemplateAddEditRow('" + item.TemplateId.trim() + "','Edit','" + item.DataTemplateId.trim() + "',event);";
                $row.attr("onclick", RowEditMethod + "utility.SelectGridRow($('#gvPhysExamDataTemplate_row" + item.DataTemplateId + "'));");

                var strAction = '<a class="btn  btn-xs" href="#" onclick="' + DeleteMethod + '" title="Delete Physical Exam Data Template"><i class="fa fa-close red"></i></a><a class="btn btn-xs" href="#" onclick="' + EditMethod + '" title="Edit Record"><i class="fa fa-edit black"></i></a><a class="btn  btn-xs" href="#" onclick="' + ActiveInacvtiveMethod + '" title="' + activeRecord + '"><i class="' + tglclass + '"></i></a>&nbsp;';
                item.SpecialtyIds = item.SpecialtyIds != null ? item.SpecialtyIds : "";
                $row.append('<td style="display:none;">' + item.DataTemplateId + '</td><td>' + strAction + '</td><td>' + item.DataTemplateName + '</td><td>' + item.TemplateName + '</td><td>' + item.SpecialtyNames  + '</td><td>' + item.ModifiedOn + '</td>');
                
                $("#" + PhysicalExamDataTemplate.params["PanelID"] + " #pnlPhysicalExamDataTemplate_Result #dgvPhysicalExamDataTemplate tbody").last().append($row);
            });
        }
        else {
            $("#" + PhysicalExamDataTemplate.params["PanelID"] + " #pnlPhysicalExamDataTemplate_Result #dgvPhysicalExamDataTemplate").DataTable({
                "bDestroy": true, "bInfo": false, "bPaginate": false,

                "language": {
                    "emptyTable": "No Physical Exam DataTemplate Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
            //Remove paging control
            $pagingControl.empty();
        }
        if ($.fn.dataTable.isDataTable("#" + PhysicalExamDataTemplate.params["PanelID"] + " #pnlPhysicalExamDataTemplate_Result #dgvPhysicalExamDataTemplate"))
            ;
        else
            $("#" + PhysicalExamDataTemplate.params["PanelID"] + " #pnlPhysicalExamDataTemplate_Result #dgvPhysicalExamDataTemplate").DataTable({ "bDestroy":true, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

        //Set ToolTip for Comments.
        // $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
    },
    PhysicalExamDataTemplateAddEditRow: function (TemplateId, mode, DataTemplateId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        if ((event.srcElement instanceof HTMLAnchorElement || event.srcElement.nodeName.toLowerCase() == 'i') != true) {
            PhysicalExamDataTemplate.PhysicalExamDataTemplateAddEdit(TemplateId, mode, DataTemplateId);
        }
    },
    //Author: Muhamamd Arshad
    //Date :  23-06-2016
    //Description: Delete the selected Physical Exam Data Template
    PhysExamDataTemplateDelete: function (PhysExamDataTemplateId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }

        utility.myConfirm('1', function () {
            var selectedValue = PhysExamDataTemplateId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                PhysicalExamDataTemplate.deletePhysExamDataTemplate(PhysExamDataTemplateId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        PhysicalExamDataTemplate.loadPhysExamDataTemplate();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { },
            '1'
        );

    },

    //Author: Muhamamd Arshad
    //Date :  23-06-2016
    //Description: To handle update Active/InActive of Physical Exam Data Template

    physExamDataTemplateActiveInactive: function (dataTemplateId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($("#gvPhysExamDataTemplate_row" + dataTemplateId));

        utility.myConfirm('3', function () {
            var selectedValue = dataTemplateId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                PhysicalExamDataTemplate.activeInactivePhysExamDataTemplate(selectedValue, IsActive).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        PhysicalExamDataTemplate.loadPhysExamDataTemplate();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { },
              '3', null, null, null, IsActive
        );
    },

    //Start// Db Calls

    //Author: Muhamamd Arshad
    //Date :  23-06-2016
    //Description: DB Call to update Active/InActive of Physical Exam Data Template
    activeInactivePhysExamDataTemplate: function (DataTemplateId, IsActive) {

        var objData = {};

        objData["DataTemplateId"] = DataTemplateId;
        objData["commandType"] = "ACTIVE_INACTIVE_PHYSEXAM_DATATEMPLATE";

        if (IsActive === 0)
            objData["IsActive"] = "False";
        else
            objData["IsActive"] = "True";

        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "PhysicalExamDataTemplate", "PhysicalExamDataTemplate");
    },

    //Author: Muhamamd Arshad
    //Date :  23-06-2016
    //Description: DB Call to Delete the selected Physical Exam Data Template
    deletePhysExamDataTemplate: function (PhysExamDataTemplateId) {
        var objData = {};
        objData["DataTemplateId"] = PhysExamDataTemplateId;
        objData["commandType"] = "DELETE_PHYSEXAM_DATATEMPLATE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamDataTemplate", "PhysicalExamDataTemplate");

    },

    searchPhysExamDataTemplate: function (jsonResponse,dataTemplateId,pageNo,rpp) {

        var objData = jsonResponse != null ? JSON.stringify(jsonResponse) : {};

        objData["CommandType"] = "Search_PhysExam_DataTemplate";
        objData["PageNo"] = (pageNo != null && pageNo != "") ? pageNo : 1;
        objData["rpp"] = (rpp != null && rpp != "") ? rpp : 15;
        objData["IsActive"] = PhysicalExamDataTemplate.Switch;
      
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamDataTemplate", "PhysicalExamDataTemplate");

    }

    //End// Db Calls
}
