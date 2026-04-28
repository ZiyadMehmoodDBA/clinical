multipleViewGroupDetail = {
    bIsFirstLoad: true,
    params: [],
    //securityGroupID: -1,pnlSchGroup
    Load: function (params) {

        multipleViewGroupDetail.params = params;

        multipleViewGroupDetail.bIsFirstLoad = false;

        var self = $('#multipleViewGroupDetail');
        self.loadDropDowns(true).done(function () {
            if (globalAppdata['IsAdmin'] != "True") {
                $("#" + multipleViewGroupDetail.params["PanelID"] + " #multipleViewGroupDetail #divMultipleView_Entity").css("display", "none");
                $("#" + multipleViewGroupDetail.params["PanelID"] + " #multipleViewGroupDetail #ddlEntity").val(globalAppdata["SeletedEntityId"]);
                $("#" + multipleViewGroupDetail.params["PanelID"] + " #multipleViewGroupDetail #ddlEntity").removeAttr("name");
            }

            multipleViewGroupDetail.LoadScheduleGroup();
        });




    },

    LoadScheduleGroup: function () {
        multipleViewGroupDetail.FillScheduleGroup(multipleViewGroupDetail.params.MSGroupId).done(function (response) {
            if (response.status != false) {
                multipleViewGroupDetail.BindProviders(response);
                multipleViewGroupDetail.BindResources(response);
                $("#multipleViewGroupDetail #pnlSchGroup").removeClass('disableAll');

                if (multipleViewGroupDetail.params.mode == "Add") {
                    $('#multipleViewGroupDetail #txtShortName').attr("enabled", "enabled");
                    $("#multipleViewGroupDetail #pnlSchGroup").addClass('disableAll');
                    multipleViewGroupDetail.ValidateScheduleGroup();
                    $('#frmMultipleViewGroupDetail').data('serialize', $('#frmMultipleViewGroupDetail').serialize());

                }
                else if (multipleViewGroupDetail.params.mode == "Edit") {
                    $("#multipleViewGroupDetail #txtShortName").attr("disabled", "disabled");

                    var schedulegroup_detail = JSON.parse(response.ScheduleGroupFill_JSON);

                    var self = $("#multipleViewGroupDetail");
                    utility.bindMyJSON(true, schedulegroup_detail, false, self).done(function () {
                        if (schedulegroup_detail.chkActive == 'True')
                            $("#multipleViewGroupDetail #chkActive").attr("checked", true);
                        else
                            $("#multipleViewGroupDetail #chkActive").attr("checked", false);
                        multipleViewGroupDetail.ValidateScheduleGroup();
                        $('#frmMultipleViewGroupDetail').data('serialize', $('#frmMultipleViewGroupDetail').serialize());
                    });

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }

            }
        });
    },

    BindProviders: function (response) {
        var Provider_Detail = JSON.parse(response.Provider_JSON);
        $("#multipleViewGroupDetail #pnlProvider").empty();
        var rowSelect = 0;
        var table = $('<table class="table-bordered  table table-condensed" id="dgvProviders"><thead><tr><th>Provider</th><th>Facility</th><th class="noWordBreak">From Date</th><th class="noWordBreak">To Date</th><th class="noWordBreak">From Time</th><th class="noWordBreak">To Time</th></tr></thead><tbody></tbody></table>');
        $.each(Provider_Detail, function (i, item) {
            var $row = $('<tr/>');
            var $div = $('<div class="checkbox-custom checkbox-default" />');
            var $cell1 = $('<td width="142"/>');
            var $cell2 = $('<td/>');
            var $cell3 = $('<td/>');
            var $cell4 = $('<td/>');
            var $cell5 = $('<td/>');
            var $cell6 = $('<td/>');
            if (rowSelect == 0)
                rowSelect = item.ProviderId;
            var chkSelect;
            if (item.GrpProvidersId != "")
                chkSelect = true;
            else
                chkSelect = false;
            $div.append($('<input>').attr({
                type: 'checkbox', id: 'chkProvider' + item.ProviderId + item.FacilityId + item.ProScheduleId, ProviderId: item.ProviderId, GrpProvidersId: item.GrpProvidersId, FacilityId: item.FacilityId, checked: chkSelect, //onclick: 'multipleViewGroupDetail.SecurityProviderCheck(' + item.ProviderId + ', "CHECKED")',
            })).append($('<label style="padding:2px 0 0 0;">').text(item.ProviderName));
            $cell1.append($div);
            $cell2.append($('<label>').text(item.FacilityName));
            //---
            $cell3.append($('<label>').text((item.FromDate).replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '')));
            $cell4.append($('<label>').text((item.ToDate).replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '')));
            $cell5.append($('<label>').text((item.FromTime)));
            $cell6.append($('<label>').text((item.ToTime)));
            //---
            $row.append($cell1);
            $row.append($cell2);
            $row.append($cell3);
            $row.append($cell4);
            $row.append($cell5);
            $row.append($cell6);

            $row.attr("onclick", "utility.SelectGridRow($('#gvProvider_row" + item.ProviderId + "'))");
            $row.attr("id", "gvProvider_row" + item.ProviderId);
            $row.attr("CurrentProviderId", item.ProviderId);
            $row.attr("CurrentFacilityId", item.FacilityId);
            $row.attr("CurrentProScheduleId", item.ProScheduleId);
            table.append($row);
        });
        $("#multipleViewGroupDetail #pnlProvider").last().append(table);
        $($(table)[0]).DataTable({
            "paging": false,
            "searching": false,
            "ordering": false,
            "info": false,
            "scrollY": 200,
            "scrollX": true,
            "bJQueryUI": true,
            "sDom": 'lfrtip'
        });
        $("#multipleViewGroupDetail #gvProvider_row" + rowSelect).addClass('active');
        $("#dgvProviders").on('click', 'tr', function (e) {
            if ($(e.target).is('input[type=checkbox]')) {
                //alert('1');
                multipleViewGroupDetail.SchProviderCheck($(this).closest('tr').attr('CurrentProviderId'), $(this).closest('tr').attr('CurrentFacilityId'), $(this).closest('tr').attr('CurrentProScheduleId'));
                return;
            }
        });
    },

    SchProviderCheck: function (ProviderId, FacilityId, ProScheduleId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Schedule Group", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                //if (Action == 'CHECKED') {
                if ($("#multipleViewGroupDetail #chkProvider" + ProviderId + FacilityId + ProScheduleId).is(':checked')) {
                    multipleViewGroupDetail.SaveSchGroupProvider(multipleViewGroupDetail.params.MSGroupId, ProviderId, FacilityId, ProScheduleId).done(function (response) {
                        if (response.status != false) {
                            $("#multipleViewGroupDetail #chkProvider" + ProviderId + FacilityId + ProScheduleId).attr('GrpProvidersId', response.GrpProvidersId);
                            utility.DisplayMessages(response.Message, 1);
                        }
                    });
                }
                else {
                    var GrpProvidersId = $("#multipleViewGroupDetail #chkProvider" + ProviderId + FacilityId + ProScheduleId).attr("GrpProvidersId");
                    multipleViewGroupDetail.DeleteSchGroupProvider(GrpProvidersId).done(function (response) {
                        if (response.status != false) {
                            $("#multipleViewGroupDetail #chkProvider" + ProviderId + FacilityId + ProScheduleId).attr('GrpProvidersId', '');
                            utility.DisplayMessages(response.Message, 1);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                //}
            }
            else {
                if ($("#multipleViewGroupDetail #chkProvider" + ProviderId + FacilityId + ProScheduleId).is(':checked')) {

                    $('#multipleViewGroupDetail #chkProvider' + ProviderId + FacilityId + ProScheduleId).prop('checked', false);
                }

                else
                    $('#multipleViewGroupDetail #chkProvider' + ProviderId + FacilityId + ProScheduleId).prop('checked', true);

                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    BindResources: function (response) {
        var Resources_Detail = JSON.parse(response.Resource_JSON);
        $("#multipleViewGroupDetail #pnlResources").empty();
        var rowSelect = 0;
        var table = $('<table class="table-bordered  table table-condensed" id="dgvResources"><thead><tr><th>Resource</th><th>Facility</th><th class="noWordBreak">From Date</th><th class="noWordBreak">To Date</th><th class="noWordBreak">From Time</th><th class="noWordBreak">To Time</th></tr></thead><tbody></tbody></table>');
        $.each(Resources_Detail, function (i, item) {
            var $row = $('<tr/>');
            var $div = $('<div class="checkbox-custom checkbox-default" />');
            var $cell1 = $('<td width="142"/>');
            var $cell2 = $('<td/>');
            var $cell2 = $('<td/>');
            var $cell3 = $('<td/>');
            var $cell4 = $('<td/>');
            var $cell5 = $('<td/>');
            var $cell6 = $('<td/>');
            if (rowSelect == 0)
                rowSelect = item.ResourceId;
            var chkSelect;
            if (item.GrpResourcesId != "")
                chkSelect = true;
            else
                chkSelect = false;
            $div.append($('<input>').attr({
                type: 'checkbox', id: 'chkResources' + item.ResourceId + item.FacilityId + item.ResScheduleId, ResourceId: item.ResourceId, GrpResourcesId: item.GrpResourcesId, FacilityId: item.FacilityId, checked: chkSelect, //onclick: 'multipleViewGroupDetail.SecurityResourcesCheck(' + item.ResourceId + ', "CHECKED")',
            })).append($('<label style="padding:2px 0 0 0;">').text(item.ShortName));
            $cell1.append($div);
            $cell2.append($('<label>').text(item.FacilityName));
            //---
            $cell3.append($('<label>').text((item.FromDate).replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '')));
            $cell4.append($('<label>').text((item.ToDate).replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '')));
            $cell5.append($('<label>').text((item.FromTime)));
            $cell6.append($('<label>').text((item.ToTime)));
            //---
            $row.append($cell1);
            $row.append($cell2);
            $row.append($cell3);
            $row.append($cell4);
            $row.append($cell5);
            $row.append($cell6);

            $row.attr("onclick", "utility.SelectGridRow($('#gvResources_row" + item.ResourceId + "'))");
            $row.attr("id", "gvResources_row" + item.ResourceId);
            $row.attr("CurrentResourceId", item.ResourceId);
            $row.attr("CurrentFacilityId", item.FacilityId);
            $row.attr("CurrentResScheduleId", item.ResScheduleId);
            table.append($row);
        });
        $("#multipleViewGroupDetail #pnlResources").last().append(table);
        $($(table)[0]).DataTable({
            "paging": false,
            "searching": false,
            "ordering": false,
            "info": false,
            "scrollY": 200,
            "scrollX": true,
            "bJQueryUI": true,
            "sDom": 'lfrtip'
        });
        $("#multipleViewGroupDetail #gvResources_row" + rowSelect).addClass('active');
        $("#dgvResources").on('click', 'tr', function (e) {
            if ($(e.target).is('input[type=checkbox]')) {
                //alert('1');
                multipleViewGroupDetail.SchResourcesCheck($(this).closest('tr').attr('CurrentResourceId'), $(this).closest('tr').attr('CurrentFacilityId'), $(this).closest('tr').attr('CurrentResScheduleId'));
                return;
            }
        });
    },

    SchResourcesCheck: function (ResourceId, FacilityId, ResScheduleId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Schedule Group", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                //if (Action == 'CHECKED') {
                if ($("#multipleViewGroupDetail #chkResources" + ResourceId + FacilityId + ResScheduleId).is(':checked')) {
                    multipleViewGroupDetail.SaveSchGroupResource(multipleViewGroupDetail.params.MSGroupId, ResourceId, FacilityId, ResScheduleId).done(function (response) {
                        if (response.status != false) {
                            //Azam Aftab Dated January 11,2015 PMS-3257 Multiview Group Schedule
                            $("#multipleViewGroupDetail #chkResources" + ResourceId + FacilityId + ResScheduleId).attr('GrpResourcesId', response.GrpResourcesId);
                            utility.DisplayMessages(response.message, 1);
                            //End PMS-3257 Multiview Group Schedule
                        }
                    });

                }
                else {
                    var SecGroupResourceId = $("#multipleViewGroupDetail #chkResources" + ResourceId + FacilityId + ResScheduleId).attr("GrpResourcesId");
                    multipleViewGroupDetail.DeleteSchGroupResource(SecGroupResourceId).done(function (response) {
                        if (response.status != false) {
                            //Azam Aftab Dated January 11,2015 PMS-3257 Multiview Group Schedule
                            $("#multipleViewGroupDetail #chkResources" + ResourceId + FacilityId + ResScheduleId).attr('GrpResourcesId', '');
                            utility.DisplayMessages(response.Message, 1);
                            //End PMS-3257 Multiview Group Schedule
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                //}
            }
            else {
                if ($("#multipleViewGroupDetail #chkResources" + ResourceId + FacilityId + ResScheduleId).is(':checked')) {
                    $('#multipleViewGroupDetail #chkResources' + ResourceId + FacilityId + ResScheduleId).prop('checked', false);
                }

                else
                    $('#multipleViewGroupDetail #chkResources' + ResourceId + FacilityId + ResScheduleId).prop('checked', true);

                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    ValidateScheduleGroup: function () {
        $('#frmMultipleViewGroupDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   ShortName: {
                       group: '.col-sm-2',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Entity: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   }
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            multipleViewGroupDetail.ScheduleGroupSave();
        });
    },

    ScheduleGroupSave: function () {



        var strMessage = "";
        var self = $("#multipleViewGroupDetail");
        var myJSON = self.getMyJSON();
        if (multipleViewGroupDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Schedule Group", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {

                    multipleViewGroupDetail.SaveScheduleGroup(myJSON).done(function (response) {
                        if (response.status != false) {
                            multipleViewGroup.ScheduleGroupSearch(response.MSGroupId);
                            utility.DisplayMessages(response.message, 1);
                            $("#multipleViewGroupDetail #pnlSchGroup").removeClass('disableAll');
                            $("#multipleViewGroupDetail #txtShortName").attr("disabled", "disabled");
                            //$("#multipleViewGroupDetail #btnMultpleSave").addClass('disableAll');
                            multipleViewGroupDetail.params.MSGroupId = response.MSGroupId;
                            multipleViewGroupDetail.params.mode = 'Edit';
                            var entity = $('#multipleViewGroupDetail #ddlEntity').val();
                            CacheManager.BindCodes('GetScheduleGroup', true).done(function (result) {
                                var Tab = GetCurrentSelectedTab();
                                eval(Tab.ContainerControlID + '.bIsFirstLoad=true');
                                SelectTab(Tab.TabID, 'false');
                                $("#multipleViewGroupDetail #ddlEntity option[value=" + entity + "]").attr('selected', 'selected');
                                setTimeout(function () {
                                    $("#multipleViewGroupDetail #ddlEntity option[value=" + entity + "]").attr('selected', 'selected');
                                    $('#frmMultipleViewGroupDetail').data('serialize', $('#frmMultipleViewGroupDetail').serialize());
                                }, 1000);

                            });
                            $("#multipleViewGroupDetail #ddlEntity option[value=" + entity + "]").attr('selected', 'selected');

                            $('#frmMultipleViewGroupDetail').data('serialize', $('#frmMultipleViewGroupDetail').serialize());
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });

        }
        else if (multipleViewGroupDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Schedule Group", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    multipleViewGroupDetail.UpdateScheduleGroup(myJSON, multipleViewGroupDetail.params.MSGroupId).done(function (response) {
                        if (response.status != false) {
                            multipleViewGroup.ScheduleGroupSearch(multipleViewGroupDetail.params.MSGroupId);
                            utility.DisplayMessages(response.message, 1);
                            UnloadActionPan(multipleViewGroupDetail.params["ParentCtrl"], "actionPanMultipleViewGroupDetail");
                            CacheManager.BindCodes('GetScheduleGroup', true).done(function (result) {

                                $('#frmMultipleViewGroupDetail').data('serialize', $('#frmMultipleViewGroupDetail').serialize());
                            });

                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
    },

    SaveScheduleGroup: function (MultipleViewGroupData) {
        var data = "MultipleViewGroupData=" + MultipleViewGroupData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "SCHEDULING_MULTIPLEVIEW_GROUP_DETAIL", "SAVE_SCHEDULE_GROUP");
    },

    FillScheduleGroup: function (MSGroupId) {
        var data = "MSGroupId=" + MSGroupId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "SCHEDULING_MULTIPLEVIEW_GROUP_DETAIL", "FILL_SCHEDULE_GROUP");
    },

    UpdateScheduleGroup: function (MultipleViewGroupData, MSGroupId) {
        var data = "MultipleViewGroupData=" + MultipleViewGroupData + "&MSGroupId=" + MSGroupId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "SCHEDULING_MULTIPLEVIEW_GROUP_DETAIL", "UPDATE_SCHEDULE_GROUP");
    },

    UpdateScheduleGroupActiveInactive: function (MSGroupId, IsActive) {
        var data = "MSGroupId=" + MSGroupId + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "SCHEDULING_MULTIPLEVIEW_GROUP_DETAIL", "UPDATE_SCHEDULE_GROUP_ACTIVE_INACTIVE");
    },


    SaveSchGroupProvider: function (MSGroupId, ProviderId, FacilityId, ProScheduleId) {
        var data = "MSGroupId=" + MSGroupId + "&ProviderID=" + ProviderId + "&FacilityId=" + FacilityId + "&ProScheduleId=" + ProScheduleId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "SCHEDULING_MULTIPLEVIEW_GROUP_DETAIL", "SAVE_SCH_GROUP_PROVIDER");
    },


    DeleteSchGroupProvider: function (GrpProvidersId) {
        var data = "GrpProvidersId=" + GrpProvidersId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "SCHEDULING_MULTIPLEVIEW_GROUP_DETAIL", "DELETE_SCH_GROUP_PROVIDER");
    },


    SaveSchGroupResource: function (MSGroupId, ResourceId, FacilityId, ResScheduleId) {
        var data = "MSGroupId=" + MSGroupId + "&ResourceId=" + ResourceId + "&FacilityId=" + FacilityId + "&ResScheduleId=" + ResScheduleId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "SCHEDULING_MULTIPLEVIEW_GROUP_DETAIL", "SAVE_SCH_GROUP_RESOURCE");
    },


    DeleteSchGroupResource: function (GrpResourcesId) {
        var data = "GrpResourcesId=" + GrpResourcesId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "SCHEDULING_MULTIPLEVIEW_GROUP_DETAIL", "DELETE_SCH_GROUP_RESOURCE");
    },

    UnLoad: function () {

        utility.UnLoadDialog('frmMultipleViewGroupDetail', function () {
            UnloadActionPan(multipleViewGroupDetail.params["ParentCtrl"], "actionPanMultipleViewGroupDetail");
        }, function () {
            UnloadActionPan(multipleViewGroupDetail.params["ParentCtrl"], "actionPanMultipleViewGroupDetail");
        });


    },
}