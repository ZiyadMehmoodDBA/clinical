privilegeGroupDetail = {
    params: [],
    securityGroupID: -1,
    Load: function (params) {
        privilegeGroupDetail.params = params;
        privilegeGroupDetail.LoadPrivilegeGroup();
    },

    LoadPrivilegeGroup: function () {
        privilegeGroupDetail.FillPrivilegeGroup(privilegeGroupDetail.params.PrivilegeGroupId, 0, 0).done(function (response) {
            if (response.status != false) {
                
                privilegeGroupDetail.BindFacility(response);
                privilegeGroupDetail.BindProviders(response);
                privilegeGroupDetail.BindResources(response);
                $("#privilegeGroupDetail #pnlPrivilegeGroup").DisabledAll(false);
                if (privilegeGroupDetail.params.mode == "Add") {
                    $('#privilegeGroupDetail #txtShortName').attr("enabled", "enabled");
                    $('#frmPrivilegeGroupDetail').data('serialize', $('#frmPrivilegeGroupDetail').serialize());
                    if (globalAppdata['AppUserName'] != DefaultUser) {
                        $('#privilegeGroupDetail #pnlIsAdmin').css("display", "none");
                    }
                    $('#privilegeGroupDetail #pnlPrivilegeGroup').DisabledAll(true);
                    privilegeGroupDetail.ValidatePrivilegeGroup();
                }
                else if (privilegeGroupDetail.params.mode == "Edit") {
                    $("#privilegeGroupDetail #txtShortName").attr("disabled", "disabled");
                    privilegeGroupDetail.BindPrivilegeGroup(response);
                    privilegeGroupDetail.ValidatePrivilegeGroup();
                    if (globalAppdata['AppUserName'] != DefaultUser) {
                        $('#privilegeGroupDetail #pnlIsAdmin').css("display", "none");
                        if (privilegeGroupDetail.params.IsAdmin == "True") {
                            $("#privilegeGroupDetail #pnlPrivilegeGroup").DisabledAll(true);
                            $('#privilegeGroupDetail #pnlIsActive').addClass("disableAll");
                        }
                    }
                    $('#frmPrivilegeGroupDetail').data('serialize', $('#frmPrivilegeGroupDetail').serialize());
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    BindPrivilegeGroup: function (response) {
        var PrivilegeGroup_Detail = JSON.parse(response.PrivilegeGroup_JSON);
        var self = $("#privilegeGroupDetail");

        utility.bindMyJSON(true, PrivilegeGroup_Detail, false, self);
        if (PrivilegeGroup_Detail.chkActive == 'True')
            $("#privilegeGroupDetail #chkActive").attr("checked", true);
        else
            $("#privilegeGroupDetail #chkActive").attr("checked", false);
        if (PrivilegeGroup_Detail.chkAdmin == 'True')
            $("#privilegeGroupDetail #chkAdmin").attr("checked", true);
        else
            $("#privilegeGroupDetail #chkAdmin").attr("checked", false);
    },

    BindFacility: function (response) {
        
        var Facility_Detail = JSON.parse(utility.decodeHtml(response.Facility_JSON));
        $("#privilegeGroupDetail #pnlFacility").empty();
        var rowSelect = 0;

        // $("#pnlFacility").append('<div class="ml-xs checkbox-custom checkbox-default"><input type="checkbox" id="chkFaciltySelectAll" onclick ="privilegeGroupDetail.FacilitySelectAll(this);" /> <label>Select All</label></div>');
        var rowSelect = 0;
        var table = $('<table class="table table-bordered table-condensed" id="dgvFacilities"><thead><tr><th class="hidden" ></th><th><span class="checkbox-custom checkbox-default"><input type="checkbox" id="chkFaciltySelectAll" onclick="privilegeGroupDetail.FacilitySelectAll(this);"> <label class="text-bold">Facility</label></span> </th><th>Description</th><th>Entity</th><th>Practice</th></tr></thead></table>');
        $.each(Facility_Detail, function (i, item) {
            var $row = $('<tr/>');
            var $div = $('<div class="checkbox-custom checkbox-default mb-tiny" />');
            var $cell0 = $('<td width="0" class="hidden" />');
            var $cell1 = $('<td width="100" />');
            var $cell2 = $('<td width="150" />');
            var $cell3 = $('<td width="40" />');
            var $cell4 = $('<td width="40" />');
            if (rowSelect == 0)
                rowSelect = item.FacilityId;
            var chkSelect;
            if (item.SecGroupFacilityId != "")
                chkSelect = true;
            else
                chkSelect = false;
            $div.append($('<input>').attr({
                type: 'checkbox', id: 'chkFacility' + item.FacilityId, FacilityId: item.FacilityId, SecGroupFacilityId: item.SecGroupFacilityId, EntityId: item.EntityId, PracticeId: item.PracticeId, checked: chkSelect, //onclick: 'privilegeGroupDetail.SecurityFacilityCheck(' + item.FacilityId + ', "CHECKED")',
            })).append($('<label>').text(item.ShortName));
            $cell0.append(item.FacilityId);
            $cell1.append($div);
            $cell2.append($('<label>').text(item.Description));
            $cell3.append($('<label>').text(item.EntityName));
            $cell4.append($('<label>').text(item.PracticeName));
            $row.append($cell0);
            $row.append($cell1);
            $row.append($cell2);
            $row.append($cell3);
            $row.append($cell4);

            $row.attr("onclick", "utility.SelectGridRow($('#gvFacility_row" + item.FacilityId + "'))");
            $row.attr("id", "gvFacility_row" + item.FacilityId);
            $row.attr("CurrentFacilityId", item.FacilityId);
            table.append($row);
        });
        
        $("#privilegeGroupDetail #pnlFacility").last().append(table);
        $("#privilegeGroupDetail #gvFacility_row" + rowSelect).addClass('active');



        if ($('#privilegeGroupDetail #pnlPrivilegeGroup #dgvFacilities tbody input[type=checkbox]:checked').length == $('#privilegeGroupDetail #pnlPrivilegeGroup #dgvFacilities tbody input[type=checkbox]').length) {
            $('#privilegeGroupDetail #pnlPrivilegeGroup #chkFaciltySelectAll').attr('checked', 'checked');
        }



        $("#privilegeGroupDetail #pnlPrivilegeGroup #dgvFacilities").on('click', 'tr', function (e) {
            if ($(e.target).is('input[type=checkbox]')) {
                //alert('1');
                if ($(e.target).is(":disabled"))
                    return false;
                if ($(e.target).is(':checked') == false) {
                    if ($('#' + privilegeGroupDetail.params.PanelID + ' #pnlFacility #chkFaciltySelectAll').is(':checked')) {
                        $('#' + privilegeGroupDetail.params.PanelID + ' #pnlFacility #chkFaciltySelectAll').attr('checked', false);
                    }
                }
                privilegeGroupDetail.SecurityFacilityCheck($(this).closest('tr').attr('CurrentFacilityId'));
                return;
            }
        });
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });

        $('#privilegeGroupDetail #pnlPrivilegeGroup #dgvFacilities').DataTable({
            "paging": false,
            "searching": false,
            "info": false,
            "scrollY": 300,
            "scrollX": true,
            "sDom": 'lfrtip',
            "order": [[0, "desc"]]
        });
        $('.dataTables_scrollBody').addClass('panel-body NoRadiusT p-none');
    },

    BindProviders: function (response) {
        var Provider_Detail = JSON.parse(utility.decodeHtml(response.Provider_JSON));
        $("#privilegeGroupDetail #pnlProvider").empty();
        var rowSelect = 0;

        // $("#pnlProvider").append('<div class="ml-xs checkbox-custom checkbox-default"><input type="checkbox" id="chkProviderSelectAll" onclick ="privilegeGroupDetail.ProviderSelectAll(this);" /> <label>Select All</label></div>');

        var table = $('<table class="table-bordered  table table-condensed" id="dgvProviders"><thead><tr><th class="hidden" ></th><th><span class="checkbox-custom checkbox-default"><input type="checkbox" id="chkProviderSelectAll" onclick="privilegeGroupDetail.ProviderSelectAll(this);"> <label class="text-bold">Provider</label></span> </th><th>Name</th><th>Entity</th></tr></thead></table>');
        $.each(Provider_Detail, function (i, item) {
            var $row = $('<tr/>');
            var $div = $('<div class="checkbox-custom checkbox-default mb-tiny" />');
            var $cell0 = $('<td width="0" class="hidden" />');
            var $cell1 = $('<td width="80"/>');
            var $cell2 = $('<td width="200"/>');
            var $cell3 = $('<td/>');
            if (rowSelect == 0)
                rowSelect = item.ProviderId;
            var chkSelect;
            if (item.SecGroupProviderId != "")
                chkSelect = true;
            else
                chkSelect = false;
            $div.append($('<input>').attr({
                type: 'checkbox', id: 'chkProvider' + item.ProviderId, ProviderId: item.ProviderId, SecGroupProviderId: item.SecGroupProviderId, checked: chkSelect, //onclick: 'privilegeGroupDetail.SecurityProviderCheck(' + item.ProviderId + ', "CHECKED")',
            })).append($('<label style="padding:2px 0 0 0;">').text(item.ShortName));
            $cell0.append(item.ProviderId);
            $cell1.append($div);
            $cell2.append($('<label>').text(item.LastName + ", " + item.FirstName));
            $cell3.append($('<label>').text(item.EntityName));
            $row.append($cell0);
            $row.append($cell1);
            $row.append($cell2);
            $row.append($cell3);

            $row.attr("onclick", "utility.SelectGridRow($('#gvProvider_row" + item.ProviderId + "'))");
            $row.attr("id", "gvProvider_row" + item.ProviderId);
            $row.attr("CurrentProviderId", item.ProviderId);
            table.append($row);
        });
        $("#privilegeGroupDetail #pnlProvider").last().append(table);
        $("#privilegeGroupDetail #gvProvider_row" + rowSelect).addClass('active');



        if ($('#privilegeGroupDetail #pnlPrivilegeGroup #dgvProviders tbody input[type=checkbox]:checked').length == $('#privilegeGroupDetail #pnlPrivilegeGroup #dgvProviders tbody input[type=checkbox]').length) {
            $('#privilegeGroupDetail #pnlPrivilegeGroup #chkProviderSelectAll').attr('checked', 'checked');
        }

        $("#dgvProviders").on('click', 'tr', function (e) {
            if ($(e.target).is('input[type=checkbox]')) {
                //alert('1');
                if ($(e.target).is(":disabled"))
                    return false;
                if ($(e.target).is(':checked') == false) {
                    if ($('#' + privilegeGroupDetail.params.PanelID + ' #pnlProvider #chkProviderSelectAll').is(':checked')) {
                        $('#' + privilegeGroupDetail.params.PanelID + ' #pnlProvider #chkProviderSelectAll').attr('checked', false);
                    }
                }

                privilegeGroupDetail.SecurityProviderCheck($(this).closest('tr').attr('CurrentProviderId'));
                return;
            }
        });
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });

        $('#dgvProviders').DataTable({
            "paging": false,
            "searching": false,
            "info": false,
            "scrollY": 300,
            "scrollX": true,
            "sDom": 'lfrtip',
            "order": [[0, "desc"]]
        });
        $('.dataTables_scrollBody').addClass('panel-body NoRadiusT p-none');
    },

    BindResources: function (response) {
        var Resources_Detail = JSON.parse(utility.decodeHtml(response.Resources_JSON));
        $("#privilegeGroupDetail #pnlResources").empty();
        var rowSelect = 0;

        //$("#pnlResources").append('<div class="ml-xs checkbox-custom checkbox-default"><input type="checkbox" id="chkResourcesSelectAll" onclick ="privilegeGroupDetail.ResourcesSelectAll(this);" /> <label style="padding:2px 0 0 0;">Select All</label></div>');

        var table = $('<table class="table-bordered  table table-condensed" id="dgvSecResources"><thead><tr><th class="hidden" ></th><th><span class="checkbox-custom checkbox-default"><input type="checkbox" id="chkResourcesSelectAll" onclick="privilegeGroupDetail.ResourcesSelectAll(this);"> <label class="text-bold">Resource</label></span> </th><th>Facility</th></tr></thead></table>');
        $.each(Resources_Detail, function (i, item) {
            var $row = $('<tr/>');
            var $div = $('<div class="checkbox-custom checkbox-default mb-tiny" />');
            var $cell0 = $('<td width="0" class="hidden" />');
            var $cell1 = $('<td/>');
            var $cell2 = $('<td/>');
            if (rowSelect == 0)
                rowSelect = item.ResourceId;
            var chkSelect;
            if (item.SecGroupResourceId != "")
                chkSelect = true;
            else
                chkSelect = false;
            $div.append($('<input>').attr({
                type: 'checkbox', id: 'chkResources' + item.ResourceId, ResourceId: item.ResourceId, SecGroupResourceId: item.SecGroupResourceId, checked: chkSelect, //onclick: 'privilegeGroupDetail.SecurityResourcesCheck(' + item.ResourceId + ', "CHECKED")',
            })).append($('<label style="padding:2px 0 0 0;">').text(item.ShortName));
            $cell0.append(item.ResourceId);
            $cell1.append($div);
            $cell2.append($('<label>').text(item.FacilityName));
            $row.append($cell0);
            $row.append($cell1);
            $row.append($cell2);

            $row.attr("onclick", "utility.SelectGridRow($('#gvResources_row" + item.ResourceId + "'))");
            $row.attr("id", "gvResources_row" + item.ResourceId);
            $row.attr("CurrentResourceId", item.ResourceId);
            table.append($row);
        });
        $("#privilegeGroupDetail #pnlResources").last().append(table);
        $("#privilegeGroupDetail #gvResources_row" + rowSelect).addClass('active');

        if ($('#privilegeGroupDetail #pnlPrivilegeGroup #dgvSecResources tbody input[type=checkbox]:checked').length == $('#privilegeGroupDetail #pnlPrivilegeGroup #dgvSecResources tbody input[type=checkbox]').length) {
            $('#privilegeGroupDetail #pnlPrivilegeGroup #chkResourcesSelectAll').attr('checked', 'checked');
        }

        $("#dgvSecResources").on('click', 'tr', function (e) {
            if ($(e.target).is('input[type=checkbox]')) {
                //alert('1');
                if ($(e.target).is(":disabled"))
                    return false;
                if ($(e.target).is(':checked') == false) {
                    if ($('#' + privilegeGroupDetail.params.PanelID + ' #pnlResources #chkResourcesSelectAll').is(':checked')) {
                        $('#' + privilegeGroupDetail.params.PanelID + ' #pnlResources #chkResourcesSelectAll').attr('checked', false);
                    }
                }

                privilegeGroupDetail.SecurityResourcesCheck($(this).closest('tr').attr('CurrentResourceId'));
                return;
            }
        });
        $('#dgvSecResources').DataTable({
            "paging": false,
            "searching": false,
            "info": false,
            "scrollY": 300,
            "scrollX": true,
            "sDom": 'lfrtip',
            "order": [[0, "desc"]]
        });
        $('.dataTables_scrollBody').addClass('panel-body NoRadiusT p-none');
    },

    SecurityFacilityCheck: function (FacilityId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Security Entity Group", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                //if (Action == 'CHECKED') {
                var PrivGroupEntityId = $("#privilegeGroupDetail #chkFacility" + FacilityId).attr("EntityId");
                var PrivGroupPracticeId = $("#privilegeGroupDetail #chkFacility" + FacilityId).attr("PracticeId");
                if ($("#privilegeGroupDetail #chkFacility" + FacilityId).is(':checked')) {
                    privilegeGroupDetail.SaveSecurityFacility(privilegeGroupDetail.params.PrivilegeGroupId, FacilityId, PrivGroupEntityId, PrivGroupPracticeId).done(function (response) {
                        if (response.status != false) {
                            $("#privilegeGroupDetail #chkFacility" + FacilityId).attr('SecGroupFacilityId', response.SecurityGroupFacilityId);
                            CacheManager.BindCodes('GetFacility', true);
                        }
                    });
                }
                else {
                    var PrivGroupFacilityId = $("#privilegeGroupDetail #chkFacility" + FacilityId).attr("SecGroupFacilityId");
                    privilegeGroupDetail.DeleteSecurityFacility(privilegeGroupDetail.params.PrivilegeGroupId, PrivGroupFacilityId, PrivGroupEntityId, PrivGroupPracticeId).done(function (response) {
                        if (response.status != false) {
                            CacheManager.BindCodes('GetFacility', true);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                //}
            }
            else {
                if ($("#privilegeGroupDetail #chkFacility" + FacilityId).is(':checked'))
                    $('#privilegeGroupDetail #chkFacility' + FacilityId).prop('checked', false);
                else
                    $('#privilegeGroupDetail #chkFacility' + FacilityId).prop('checked', true);

                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    SecurityFacilityCheckAll: function (FacilityJSON) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Security Entity Group", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#chkFaciltySelectAll").is(':checked')) {
                    privilegeGroupDetail.SaveSecurityFacilityCheckAll(privilegeGroupDetail.params.PrivilegeGroupId, FacilityJSON).done(function (response) {
                        if (response.status != false) {
                            //$("#privilegeGroupDetail #chkFacility" + FacilityId).attr('SecGroupFacilityId', response.SecurityGroupFacilityId);
                            Facilities = JSON.parse(response.Facility_JSON)
                            $.each(Facilities, function (i, item) {
                                $("#privilegeGroupDetail #chkFacility" + item.FacilityId).attr('secgroupfacilityid', item.SecGroupFacilityId);
                            });
                            CacheManager.BindCodes('GetFacility', true);
                        }
                    });
                }
                else {
                    //var PrivGroupFacilityId = $("#privilegeGroupDetail #chkFacility" + FacilityId).attr("SecGroupFacilityId");
                    privilegeGroupDetail.DeleteSecurityFacilityCheckAll(privilegeGroupDetail.params.PrivilegeGroupId, FacilityJSON).done(function (response) {
                        if (response.status != false) {
                            CacheManager.BindCodes('GetFacility', true);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                //}
            }
            else {
                Facility = JSON.parse(FacilityJSON)
                $.each(Facility, function (item) {
                    if ($("#privilegeGroupDetail #chkFacility" + item).is(':checked'))
                        $('#privilegeGroupDetail #chkFacility' + item).prop('checked', false);
                    else
                        $('#privilegeGroupDetail #chkFacility' + item).prop('checked', true);
                });

                if ($("#privilegeGroupDetail #chkFaciltySelectAll").is(':checked'))
                    $('#privilegeGroupDetail #chkFaciltySelectAll').prop('checked', false);
                else
                    $('#privilegeGroupDetail #chkFaciltySelectAll').prop('checked', true);

                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    SecurityProviderCheck: function (ProviderId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Security Entity Group", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                //if (Action == 'CHECKED') {
                if ($("#privilegeGroupDetail #chkProvider" + ProviderId).is(':checked')) {
                    privilegeGroupDetail.SaveSecurityProvider(privilegeGroupDetail.params.PrivilegeGroupId, ProviderId).done(function (response) {
                        if (response.status != false) {
                            $("#privilegeGroupDetail #chkProvider" + ProviderId).attr('SecGroupProviderId', response.SecurityGroupProviderId);
                        }
                    });
                }
                else {
                    var PrivGroupProviderId = $("#privilegeGroupDetail #chkProvider" + ProviderId).attr("SecGroupProviderId");
                    privilegeGroupDetail.DeleteSecurityProvider(PrivGroupProviderId).done(function (response) {
                        if (response.status != false) {
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                //}
            }
            else {
                if ($("#privilegeGroupDetail #chkProvider" + ProviderId).is(':checked'))
                    $('#privilegeGroupDetail #chkProvider' + ProviderId).prop('checked', false);
                else
                    $('#privilegeGroupDetail #chkProvider' + ProviderId).prop('checked', true);

                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    SecurityProviderCheckAll: function (ProviderJSON) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Security Entity Group", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                //if (Action == 'CHECKED') {
                if ($("#chkProviderSelectAll").is(':checked')) {
                    privilegeGroupDetail.SaveSecurityProviderCheckAll(privilegeGroupDetail.params.PrivilegeGroupId, ProviderJSON).done(function (response) {
                        if (response.status != false) {
                            Providers = JSON.parse(response.Provider_JSON)
                            $.each(Providers, function (i, item) {
                                $('#privilegeGroupDetail #chkProvider' + item.ProviderId).attr('secgroupproviderid', item.SecGroupProviderId);
                            });
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else {
                    privilegeGroupDetail.DeleteSecurityProviderCheckAll(ProviderJSON).done(function (response) {
                        if (response.status != false) {
                            //$('#' + privilegeGroupDetail.params.PanelID + ' #dgvProviders input[type=checkbox]').each(function () {
                            //    $(this).attr('moduleformuserprivilegeid', "");
                            //});
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            }
            else {
                Provider = JSON.parse(ProviderJSON)
                $.each(Provider, function (item) {
                    if ($("#privilegeGroupDetail #chkProvider" + item).is(':checked'))
                        $('#privilegeGroupDetail #chkProvider' + item).prop('checked', false);
                    else
                        $('#privilegeGroupDetail #chkProvider' + item).prop('checked', true);
                });

                if ($("#privilegeGroupDetail #chkProviderSelectAll").is(':checked'))
                    $('#privilegeGroupDetail #chkProviderSelectAll').prop('checked', false);
                else
                    $('#privilegeGroupDetail #chkProviderSelectAll').prop('checked', true);

                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    SecurityResourcesCheck: function (ResourceId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Security Entity Group", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                //if (Action == 'CHECKED') {
                if ($("#privilegeGroupDetail #chkResources" + ResourceId).is(':checked')) {
                    privilegeGroupDetail.SaveSecurityResource(privilegeGroupDetail.params.PrivilegeGroupId, ResourceId).done(function (response) {
                        if (response.status != false) {
                            $("#privilegeGroupDetail #chkResources" + ResourceId).attr('SecGroupResourceId', response.SecurityGroupResourceId);
                        }
                    });
                }
                else {
                    var PrivGroupResourceId = $("#privilegeGroupDetail #chkResources" + ResourceId).attr("SecGroupResourceId");
                    privilegeGroupDetail.DeleteSecurityResource(PrivGroupResourceId).done(function (response) {
                        if (response.status != false) {
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                //}
            }
            else {
                if ($("#privilegeGroupDetail #chkResources" + ResourceId).is(':checked'))
                    $('#privilegeGroupDetail #chkResources' + ResourceId).prop('checked', false);
                else
                    $('#privilegeGroupDetail #chkResources' + ResourceId).prop('checked', true);

                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    SecurityResourcesCheckAll: function (ResourceJSON) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Security Entity Group", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#chkResourcesSelectAll").is(':checked')) {
                    privilegeGroupDetail.SaveSecurityResourceCheckAll(privilegeGroupDetail.params.PrivilegeGroupId, ResourceJSON).done(function (response) {
                        if (response.status != false) {
                            Resources = JSON.parse(response.Resource_JSON)
                            $.each(Resources, function (i, item) {
                                $('#privilegeGroupDetail #chkResources' + item.ResourceId).attr('SecGroupResourceId', item.SecGroupResourceId);
                            });
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else {
                    privilegeGroupDetail.DeleteSecurityResourceCheckAll(ResourceJSON).done(function (response) {
                        if (response.status != false) {
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            }
            else {
                Resources = JSON.parse(ResourceJSON)
                $.each(Resources, function (i, item) {
                    if ($("#privilegeGroupDetail #chkResources" + item).is(':checked'))
                        $('#privilegeGroupDetail #chkResources' + item).prop('checked', false);
                    else
                        $('#privilegeGroupDetail #chkResources' + item).prop('checked', true);
                });

                if ($("#privilegeGroupDetail #chkResourcesSelectAll").is(':checked'))
                    $('#privilegeGroupDetail #chkResourcesSelectAll').prop('checked', false);
                else
                    $('#privilegeGroupDetail #chkResourcesSelectAll').prop('checked', true);

                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    ValidatePrivilegeGroup: function () {
        $('#frmPrivilegeGroupDetail')
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

                }
            })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            privilegeGroupDetail.PrivilegeGroupSave();
        });
    },

    PrivilegeGroupSave: function () {
        $('#frmPrivilegeGroupDetail').data('serialize', $('#frmPrivilegeGroupDetail').serialize());
        var strMessage = "";
        var self = $("#privilegeGroupDetail");
        var myJSON = self.getMyJSON();
        if (privilegeGroupDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Security Entity Group", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    if (privilegeGroupDetail.params.PrivilegeGroupId == "-1") {
                        privilegeGroupDetail.SavePrivilegeGroup(myJSON).done(function (response) {
                            if (response.status != false) {
                                $("#privilegeGroupDetail #pnlPrivilegeGroup").removeClass('disableAll');
                                $("#privilegeGroupDetail #pnlPrivilegeGroup").DisabledAll(false);
                                $("#privilegeGroupDetail #txtShortName").attr("disabled", "disabled");
                                privilegeGroupDetail.params.PrivilegeGroupId = response.PrivilegeGroupId;
                                Admin_PrivilegeGroup.PrivilegeGroupSearch();
                                $('#frmPrivilegeGroupDetail').data('serialize', $('#frmPrivilegeGroupDetail').serialize());
                                utility.DisplayMessages(response.message, 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                    else if (privilegeGroupDetail.params.PrivilegeGroupId != "-1" && privilegeGroupDetail.params.PrivilegeGroupId != "" && securityRoleDetail.params.PrivilegeGroupId != "0") {
                        privilegeGroupDetail.UpdatePrivilegeGroup(myJSON, privilegeGroupDetail.params.PrivilegeGroupId, 1).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });

                    }
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else if (privilegeGroupDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Security Entity Group", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    privilegeGroupDetail.UpdatePrivilegeGroup(myJSON, privilegeGroupDetail.params.PrivilegeGroupId, 1).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            Admin_PrivilegeGroup.PrivilegeGroupSearch(privilegeGroupDetail.params.PrivilegeGroupId);
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

    SavePrivilegeGroup: function (PrivilegeGroupData) {
        var data = "PrivilegeGroupData=" + PrivilegeGroupData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PRIVILEGE_GROUP_DETAIL", "SAVE_PRIVILEGE_GROUP");
    },

    UpdatePrivilegeGroup: function (PrivilegeGroupData, PrivilegeGroupID, IsActive) {
        var data = "PrivilegeGroupData=" + PrivilegeGroupData + "&PrivilegeGroupID=" + PrivilegeGroupID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PRIVILEGE_GROUP_DETAIL", "UPDATE_PRIVILEGE_GROUP");
    },

    FillPrivilegeGroup: function (PrivilegeGroupID) {
        var data = "PrivilegeGroupID=" + PrivilegeGroupID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PRIVILEGE_GROUP_DETAIL", "FILL_PRIVILEGE_GROUP");
    },

    SaveSecurityProvider: function (PrivilegeGroupId, ProviderId) {
        var data = "PrivilegeGroupID=" + PrivilegeGroupId + "&ProviderID=" + ProviderId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PRIVILEGE_GROUP_DETAIL", "SAVE_PRIVILEGE_GROUP_PROVIDER");
    },

    SaveSecurityProviderCheckAll: function (PrivilegeGroupId, ProviderJSON) {
        var data = "PrivilegeGroupID=" + PrivilegeGroupId + "&ProviderJSon=" + ProviderJSON;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PRIVILEGE_GROUP_DETAIL", "SAVE_PRIVILEGE_GROUP_PROVIDER_CHECK_ALL");
    },

    DeleteSecurityProvider: function (PrivGroupProviderId) {
        var data = "PrivilegeGroupProviderID=" + PrivGroupProviderId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PRIVILEGE_GROUP_DETAIL", "DELETE_PRIVILEGE_GROUP_PROVIDER");
    },
    DeleteSecurityProviderCheckAll: function (ProviderJSON) {
        var data = "PrivilegeGroupProviderJSon=" + ProviderJSON;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PRIVILEGE_GROUP_DETAIL", "DELETE_PRIVILEGE_GROUP_PROVIDER_CHECK_ALL");
    },

    SaveSecurityResource: function (PrivilegeGroupId, ResourceId) {
        var data = "PrivilegeGroupID=" + PrivilegeGroupId + "&ResourceID=" + ResourceId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PRIVILEGE_GROUP_DETAIL", "SAVE_PRIVILEGE_GROUP_RESOURCE");
    },
    SaveSecurityResourceCheckAll: function (PrivilegeGroupId, ResourceJSON) {
        var data = "PrivilegeGroupID=" + PrivilegeGroupId + "&ResourceJSON=" + ResourceJSON;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PRIVILEGE_GROUP_DETAIL", "SAVE_PRIVILEGE_GROUP_RESOURCE_CHECK_ALL");
    },

    DeleteSecurityResource: function (PrivGroupResourceId) {
        var data = "PrivilegeGroupResourceID=" + PrivGroupResourceId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PRIVILEGE_GROUP_DETAIL", "DELETE_PRIVILEGE_GROUP_RESOURCE");
    },
    DeleteSecurityResourceCheckAll: function (ResourceJSON) {
        var data = "ResourceJSON=" + ResourceJSON;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PRIVILEGE_GROUP_DETAIL", "DELETE_PRIVILEGE_GROUP_RESOURCE_CHECK_ALL");
    },

    SaveSecurityFacility: function (PrivilegeGroupId, FacilityId, EntityId, PracticeId) {
        var data = "PrivilegeGroupID=" + PrivilegeGroupId + "&FacilityID=" + FacilityId + "&EntityID=" + EntityId + "&PracticeID=" + PracticeId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PRIVILEGE_GROUP_DETAIL", "SAVE_PRIVILEGE_GROUP_FACILITY");
    },
    SaveSecurityFacilityCheckAll: function (PrivilegeGroupId, FacilityJSON) {
        var data = "PrivilegeGroupID=" + PrivilegeGroupId + "&FacilityJSon=" + FacilityJSON;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PRIVILEGE_GROUP_DETAIL", "SAVE_PRIVILEGE_GROUP_FACILITY_CHECK_ALL");
    },

    DeleteSecurityFacility: function (PrivilegeGroupId, PrivilegeGroupFacilityId, EntityId, PracticeId) {
        var data = "PrivilegeGroupID=" + PrivilegeGroupId + "&PrivilegeGroupFacilityID=" + PrivilegeGroupFacilityId + "&EntityID=" + EntityId + "&PracticeID=" + PracticeId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PRIVILEGE_GROUP_DETAIL", "DELETE_PRIVILEGE_GROUP_FACILITY");
    },
    DeleteSecurityFacilityCheckAll: function (PrivilegeGroupId, FacilityJSON) {
        var data = "PrivilegeGroupID=" + PrivilegeGroupId + "&FacilityJSON=" + FacilityJSON;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PRIVILEGE_GROUP_DETAIL", "DELETE_PRIVILEGE_GROUP_FACILITY_CHECK_ALL");
    },

    UpdatePrivilegeGroupActiveInactive: function (PrivilegeGroupID, IsActive) {
        var data = "PrivilegeGroupID=" + PrivilegeGroupID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PRIVILEGE_GROUP_DETAIL", "UPDATE_PRIVILEGE_GROUP_ACTIVE_INACTIVE");
    },

    UnLoad: function () {

        utility.UnLoadDialog("frmPrivilegeGroupDetail", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });

    },

    FacilitySelectAll: function (ev) {
        var AllFacility = [];
        if (!ev.checked) {
            $('#' + privilegeGroupDetail.params.PanelID + ' #dgvFacilities tbody input[type=checkbox]').each(function (i, item) {
                if ($(this).is(":checked") == true) {
                    AllFacility.push({ 'FacilityId': $(this).attr('SecGroupFacilityId'), 'PracticeId': $(this).attr('practiceid'), 'EntityId': $(this).attr('entityid') });
                    $(this).prop('checked', false);
                }
            });
        }
        else {
            $('#' + privilegeGroupDetail.params.PanelID + ' #dgvFacilities tbody input[type=checkbox]').each(function (i, item) {
                if ($(this).is(":checked") == false) {
                    AllFacility.push({ 'FacilityId': $(this).attr('FacilityId'), 'PracticeId': $(this).attr('practiceid'), 'EntityId': $(this).attr('entityid') });
                    $(this).prop('checked', true);
                }
            });
        }

        var FacilityJSON = JSON.stringify(AllFacility);
        privilegeGroupDetail.SecurityFacilityCheckAll(FacilityJSON);

    },

    ProviderSelectAll: function (ev) {

        var AllProvider = [];
        if (!ev.checked) {
            $('#' + privilegeGroupDetail.params.PanelID + ' #dgvProviders tbody input[type=checkbox]').each(function (i, item) {
                if ($(this).is(":checked") == true) {
                    AllProvider.push($(this).attr('secgroupproviderid'));
                    $(this).prop('checked', false);
                }
            });
        }
        else {
            $('#' + privilegeGroupDetail.params.PanelID + ' #dgvProviders tbody input[type=checkbox]').each(function (i, item) {
                if ($(this).is(":checked") == false) {
                    AllProvider.push($(this).attr('providerid'));
                    $(this).prop('checked', true);
                }
            });
        }

        var ProviderJSON = JSON.stringify(AllProvider);
        privilegeGroupDetail.SecurityProviderCheckAll(ProviderJSON);
    },
    ResourcesSelectAll: function (ev) {
        var AllResource = [];
        if (!ev.checked) {
            $('#' + privilegeGroupDetail.params.PanelID + ' #dgvSecResources tbody input[type=checkbox]').each(function (i, item) {
                if ($(this).is(":checked") == true) {
                    AllResource.push($(this).attr('secgroupresourceid'));
                    $(this).prop('checked', false);
                }
            });
        }
        else {
            $('#' + privilegeGroupDetail.params.PanelID + ' #dgvSecResources tbody input[type=checkbox]').each(function (i, item) {
                if ($(this).is(":checked") == false) {
                    AllResource.push($(this).attr('resourceid'));
                    $(this).prop('checked', true);
                }
            });
        }

        var ResourceJSON = JSON.stringify(AllResource);
        privilegeGroupDetail.SecurityResourcesCheckAll(ResourceJSON);
    },

}