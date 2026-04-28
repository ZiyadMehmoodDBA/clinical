HPITemplate = {
    bIsFirstLoad: true,
    params: [],

    Load: function (params) {
        HPITemplate.params = params;
        HPITemplate.params.mode = "Add";

        if (HPITemplate.params.PanelID != 'pnlHPITemplate') {
            HPITemplate.params.PanelID = HPITemplate.params.PanelID + ' #pnlHPITemplate';
        } else {
            HPITemplate.params.PanelID = 'pnlHPITemplate';
        }
            
        var self = $('#' + HPITemplate.params.PanelID);
      
        if (HPITemplate.bIsFirstLoad == true) {
            HPITemplate.loadHPITemplate();
        }
                 
        $('#' + HPITemplate.params.PanelID + ' #pnlHPITemplate_Result #dgvHPITemplates').DataTable({
            "language": {
                "emptyTable": "No HPI Template Found."
            }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bDestroy": true
        });

        var HtmlOfSwitch = '<div id="divSwitch"><span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                         '<input id="switchActive" isactive="1" type="checkbox" checked="checked" name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="">' +
                          '</div><span class="pl-xs">Active</span> </div>';

        $("#" + HPITemplate.params.PanelID + ' .datatables-header div:first').html(HtmlOfSwitch);
        HPITemplate.readyFunction();       
    },

    readyFunction: function () {

        (function ($) {
            'use strict';
            $(function () {
                $('[data-plugin-ios-switch]').each(function () {
                    var $this = $(this);

                    $this.themePluginIOS7Switch();
                });
            });
        }).apply(this, [jQuery]);
    },

    loadHPITemplate: function () {
        HPITemplate.HPITemplateLoad().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                HPITemplate.HPITemplateGridLoad(response);
                var TableControl = HPITemplate.params.PanelID + " #pnlHPITemplate_Result #dgvHPITemplates";
                var PagingPanelControlID = HPITemplate.params.PanelID + " #dgvPhysExamTemplates_Paging";
                var ClassControlName = "HPITemplate";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                //setTimeout(
                //    CreatePagination(response.PhysExamTemplateCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                //        PhysicalExamTemplate.PhysExamTemplateSearch(PrimaryID, PageNumber, ResultPerPage);
                //    }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    HPITemplateLoad: function () {
        var objData = new Object();
        var IsActive = null;
        IsActive = $('#' + HPITemplate.params.PanelID + ' #pnlHPITemplate_Result #divSwitch #switchActive').attr('isactive');
        if (IsActive == null) {
            IsActive = "1";
        }
        var entityId = 0;
        if (globalAppdata.AppUserName.toLowerCase() != DefaultUser.toLowerCase()) {
            entityId = globalAppdata["SeletedEntityId"];
        }
        objData["HPITemplateId"] = 0;
        objData["IsActive"] = IsActive;
        objData["EntityId"] = entityId;
        objData["commandType"] = "load_hpi_templates";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPI");
    },

    HPITemplateGridLoad: function (response) {
        var isactive = $('#' + HPITemplate.params.PanelID + ' #pnlHPITemplate_Result #divSwitch #switchActive').attr('isactive');
        $("#" + HPITemplate.params.PanelID + " #pnlHPITemplate_Result #dgvHPITemplates").dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it
        $("#" + HPITemplate.params.PanelID + " #pnlHPITemplate_Result #dgvHPITemplates tbody").find("tr").remove(); //Removing all the table data from table body

        if (response.HPITemplateCount > 0) {
            var HPITemplateJSONData =response.listHPITemplate; //Parsing array to JSON

            $.each(HPITemplateJSONData, function (i, item) {
                var $row = $('<tr/>');               
                $row.attr("id", "dgvHPITemplates" + item.HPITemplateId);
                $row.attr("HPITemplateId", item.HPITemplateId);

                var IsDefault = false;

                var editCall = "HPITemplate.HPITemplateEdit('" + item.HPITemplateId + "',event);";
               // $row.attr("onclick", editCall);
                if (item.IsActive == "True") {
                    isactive = 1;
                    isEventactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isEventactive = 1;
                    isactive = 0;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }

                var dateInNumberFormat = Number(new Date(item.ModifiedOn));

                $row.append('<td class="hidden">' + dateInNumberFormat + '</td><td style="display:none;">' + item.HPITemplateId + '</td><td><a class="btn btn-xs" href="#" onclick="HPITemplate.HPITemplateDelete(\'' + item.HPITemplateId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="HPITemplate.HPITemplateEdit(\'' + item.HPITemplateId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="HPITemplate.HPITemplateActiveInactive(\'' + item.HPITemplateId + '\', ' + isEventactive + ',event);"><i class="' + tglclass + '"></i></a></td><td>' + item.Name + '</td><td>' + item.ModifiedOn + '</td>');

                $("#" + HPITemplate.params.PanelID + " #pnlHPITemplate_Result #dgvHPITemplates tbody").last().append($row);
            });
        }
        else {
            $('#' + HPITemplate.params.PanelID + ' #pnlHPITemplate_Result #dgvHPITemplates').DataTable({
                "language": {
                    "emptyTable": "No HPI Template Found."
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bDestroy": true, "bFilter": false, "bInfo": false, "bPaginate": false,
            });

        }
        if ($.fn.dataTable.isDataTable("#" + HPITemplate.params.PanelID + ' #pnlHPITemplate_Result #dgvHPITemplates'))
            ;
        else {
            $("#" + HPITemplate.params.PanelID + " #pnlHPITemplate_Result #dgvHPITemplates").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]] });
        }
        var checked = '';
        if (isactive == "0" || isactive == 0) {
        } else if (isactive == null) {
            isactive = "1";
            checked = 'checked="checked"';
        } else {
            isactive = "1";
            checked = 'checked="checked"';
        }
        var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                       '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="HPITemplate.activeHPITemplateSearch(this);">' +
                        '</div><span class="pl-xs">Active</span>';

        $("#" + HPITemplate.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        EMRUtility.SwicthWidgetInializatoin();

    },

    HPITemplateEdit: function (TemplateId, event) {
        event.stopPropagation();
        HPITemplate.HPITemplateAddEdit(TemplateId);
    },
    activeHPITemplateSearch: function (objThis) {
        var isactive = $(objThis).attr('isactive');
        if (isactive == '1') {
            $(objThis).attr('isactive', '0');
        }
        else if (isactive == '0') {
            $(objThis).attr('isactive', '1');
        }
        HPITemplate.loadHPITemplate();
    },


    HPITemplateAddEdit: function (HPITemplateId) {
                var params = [];
                if (HPITemplateId && parseInt(HPITemplateId) > 0) {
                    params["HPITemplateId"] = HPITemplateId;
                    params["mode"] = "Edit";
                }
                else {
                    params["HPITemplateId"] = -1;
                    params["mode"] = "Add";
                }
                params["FromAdmin"] = HPITemplate.params["FromAdmin"];
                params["ParentCtrl"] = 'adminTabHPITemplate';
                LoadActionPan('HPITemplateDetail', params);

    },

    HPITemplateDelete: function (TemplateId, event) {
            utility.myConfirm('Are you sure to delete template?', function () {
                HPITemplate.DeleteHPITemplate(TemplateId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        HPITemplate.loadHPITemplate();
                    }
                    else {
                        utility.DisplayMessages(response.message, 3);
                    }
                });
            }, function () { },
                  'Confirm Delete'
              );

        event.stopPropagation();
    },


    DeleteHPITemplate: function (TemplateId) {
        var objData = new Object();
        objData["HPITemplateId"] = TemplateId;
        objData["commandType"] = "DELETE_HPITemplate";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPI");
    },

    HPITemplateActiveInactive: function (TemplateId,isactive, event) {
       
            utility.myConfirm('3', function () {
                if (TemplateId == "" || TemplateId == "undefined") {
                }
                else {
                    HPITemplate.updateHPITemplateActiveInactive_Dbcall(TemplateId, isactive).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            HPITemplate.loadHPITemplate();
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            }, function () { },
                      '3', null, null, null, isactive
                   );
        
        event.stopPropagation();
    },

    updateHPITemplateActiveInactive_Dbcall: function (TemplateId, IsActive) {
        var objData = {};
        objData["HPITemplateId"] = TemplateId;
        objData["IsActive"] = IsActive;

        objData["commandType"] = "UPDATE_HPITemplate_ACTIVE_INACTIVE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPI");
    },
}