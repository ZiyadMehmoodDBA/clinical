Admin_FollowUpGroup_Detail = {
    params: [],
    isFirstLoad: true,
    Load: function (params) {
        Admin_FollowUpGroup_Detail.params = params;
        if (Admin_FollowUpGroup_Detail.params.PanelID != "pnlAdminFollowUpGroupDetail") {
            Admin_FollowUpGroup_Detail.params.PanelID = Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlAdminFollowUpGroupDetail';
        }
        if (Admin_FollowUpGroup_Detail.isFirstLoad) {
            Admin_FollowUpGroup_Detail.LoadFacility();
            Admin_FollowUpGroup_Detail.ValidateGroupDetail();
            
        }

    },
    LoadFacility: function () {
        Admin_Facility.SearchFacility(null, null).done(function (response) {
            if (response.status != false) {
                Admin_FollowUpGroup_Detail.BindFacility(response);
                $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #frmAdminFollowUpGroupDetail').data('serialize', $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #frmAdminFollowUpGroupDetail').serialize());
                Admin_FollowUpGroup_Detail.LoadProvider();
                
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    LoadProvider: function () {
        Admin_Provider.SearchProvider(null, null).done(function (response) {
            if (response.status != false) {
                Admin_FollowUpGroup_Detail.BindProvider(response);
                $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #frmAdminFollowUpGroupDetail').data('serialize', $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #frmAdminFollowUpGroupDetail').serialize());
                Admin_FollowUpGroup_Detail.LoadPOS();
                
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    LoadPOS: function () {
        Admin_PlaceOfService.SearchPlaceOfService(null, null).done(function (response) {
            if (response.status != false) {
                Admin_FollowUpGroup_Detail.BindPOS(response);
                $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #frmAdminFollowUpGroupDetail').data('serialize', $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #frmAdminFollowUpGroupDetail').serialize());
                Admin_FollowUpGroup_Detail.LoadPlanCategory();
                
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    LoadPlanCategory: function () {
        Admin_PlanCategory.SearchPlanCategory(null, null).done(function (response) {
            if (response.status != false) {
                Admin_FollowUpGroup_Detail.BindPlanCategory(response);
                $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #frmAdminFollowUpGroupDetail').data('serialize', $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #frmAdminFollowUpGroupDetail').serialize());
                Admin_FollowUpGroup_Detail.LoadPlanType();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    LoadPlanType: function () {
        MDVisionService.lookups("GetPlanType", true).done(function (results) {
            results = JSON.parse(results['GetPlanType']);
            if (results.length > 0) {
                results = results.splice(1);
                Admin_FollowUpGroup_Detail.BindPlanType(results);
                $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #frmAdminFollowUpGroupDetail').data('serialize', $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #frmAdminFollowUpGroupDetail').serialize());
                if (Admin_FollowUpGroup_Detail.params.mode.toLowerCase() == "edit") {
                    Admin_FollowUpGroup_Detail.GroupFill();
                    $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlARGroupSection').removeClass('disableAll');
                }
            }
        });
    },

    BindFacility: function (response) {
        var Facilities = JSON.parse(response.FacilityLoad_JSON);
        $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlFacility').empty();
        $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlFacility').append('<div class="pl-xs"><input type="checkbox" id="chkFaciltySelectAll" onclick ="Admin_FollowUpGroup_Detail.FacilitySelectAll(this);" /> <label>Select All</label></div>');
        var div = $('<table class="border table table-condensed" id="dgvFacility"></table>');
        $.each(Facilities, function (i, item) {
            var $row = $('<tr/>');
            var $cell1 = $('<td width="22"/>');
            var $cell2 = $('<td/>');
            //var chkSelect;
            //if (item.FacilityId != "")
            //    chkSelect = true;
            //else
            //    chkSelect = false;
            $cell1.append($('<input>').attr({
                type: 'checkbox', id: 'chkFacility' + item.FacilityId, checked: false,
            }));
            $cell2.append($('<label style="padding:2px 0 0 0;">').text(item.ShortName));
            $row.append($cell1);
            $row.append($cell2);

            $row.attr("onclick", "utility.SelectGridRow($('#gvFacility_row" + item.FacilityId + "'))");
            $row.attr("id", "gvFacility_row" + item.FacilityId);
            $row.attr("CurrentFacilityId", item.FacilityId);
            div.append($row);
        });
        $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlFacility').last().append(div);
        $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlFacility').addClass("disableAll");

        // if ($("#userDetail #chkForm" + userDetail.formRowSelect).is(':checked')) {
        $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlFacility').removeClass("disableAll");

        //}

        $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlFacility #dgvFacility').on('click', 'tr', function (e) {
                if ($(e.target).is('input[type=checkbox]')) {
                    if ($(e.target).is(":checked")) {
                        Admin_FollowUpGroup_Detail.ARFacilitySave($(this).closest('tr').attr('CurrentFacilityId'), $(e.target));
                    }
                    else {
                        Admin_FollowUpGroup_Detail.FacilityARGroupDelete($(e.target).attr('FacilityGroupId'), $(e.target));
                    }
                return;
            }
        });
    },
    BindProvider: function (response) {
        var Providers = JSON.parse(response.ProviderLoad_JSON);
        $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlProvider').empty();
        $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlProvider').append('<div class="pl-xs"><input type="checkbox" id="chkProviderSelectAll" onclick ="Admin_FollowUpGroup_Detail.ProviderSelectAll(this);" /> <label>Select All</label></div>');
        var div = $('<table class="border table table-condensed" id="dgvProvider"></table>');
        $.each(Providers, function (i, item) {
            var $row = $('<tr/>');
            var $cell1 = $('<td width="22"/>');
            var $cell2 = $('<td/>');
            //var chkSelect;
            //if (item.FacilityId != "")
            //    chkSelect = true;
            //else
            //    chkSelect = false;
            $cell1.append($('<input>').attr({
                type: 'checkbox', id: 'chkProvider' + item.ProviderId, checked: false,
            }));
            $cell2.append($('<label style="padding:2px 0 0 0;">').text(item.ShortName));
            $row.append($cell1);
            $row.append($cell2);

            $row.attr("onclick", "utility.SelectGridRow($('#gvProvider_row" + item.ProviderId + "'))");
            $row.attr("id", "gvProvider_row" + item.ProviderId);
            $row.attr("CurrentProviderId", item.ProviderId);
            div.append($row);
        });
        $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlProvider').last().append(div);
        $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlProvider').addClass("disableAll");

        // if ($("#userDetail #chkForm" + userDetail.formRowSelect).is(':checked')) {
        $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlProvider').removeClass("disableAll");

        //}

        $("#" + Admin_FollowUpGroup.params.PanelID + ' #pnlProvider #dgvProvider').on('click', 'tr', function (e) {
            if ($(e.target).is('input[type=checkbox]')) {
                if ($(e.target).is(":checked")) {
                    Admin_FollowUpGroup_Detail.ARProviderSave($(this).closest('tr').attr('CurrentProviderId'), $(e.target));
                }
                else {
                    Admin_FollowUpGroup_Detail.providerARGroupDelete($(e.target).attr('providerGroupId'), $(e.target));
                }
                return;
            }
        });
    },
    BindPOS: function (response) {
        var POS = JSON.parse(response.PlaceOfServiceLoad_JSON);
        $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlPOS').empty();
        $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlPOS').append('<div class="pl-xs"><input type="checkbox" id="chkPOSSelectAll" onclick ="Admin_FollowUpGroup_Detail.POSSelectAll(this);" /> <label>Select All</label></div>');
        var div = $('<table class="border table table-condensed" id="dgvPOS"></table>');
        $.each(POS, function (i, item) {
            var $row = $('<tr/>');
            var $cell1 = $('<td width="22"/>');
            var $cell2 = $('<td/>');
            //var chkSelect;
            //if (item.FacilityId != "")
            //    chkSelect = true;
            //else
            //    chkSelect = false;
            $cell1.append($('<input>').attr({
                type: 'checkbox', id: 'chkPOS' + item.POSId, checked: false,
            }));
            $cell2.append($('<label style="padding:2px 0 0 0;">').text(item.POSCode));
            $row.append($cell1);
            $row.append($cell2);

            $row.attr("onclick", "utility.SelectGridRow($('#gvPOS_row" + item.POSId + "'))");
            $row.attr("id", "gvPOS_row" + item.POSId);
            $row.attr("CurrentPOSId", item.POSId);
            div.append($row);
        });
        $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlPOS').last().append(div);
        $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlPOS').addClass("disableAll");

        // if ($("#userDetail #chkForm" + userDetail.formRowSelect).is(':checked')) {
        $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlPOS').removeClass("disableAll");

        //}

        $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlPOS #dgvPOS').on('click', 'tr', function (e) {
            if ($(e.target).is('input[type=checkbox]')) {
                if ($(e.target).is(":checked")) {
                    Admin_FollowUpGroup_Detail.ARPOSSave($(this).closest('tr').attr('CurrentPOSId'), $(e.target));
                }
                else {
                    Admin_FollowUpGroup_Detail.POSARGroupDelete($(e.target).attr('POSGroupId'), $(e.target));
                }
                return;
            }
        });
    },
    BindPlanCategory: function (response) {
        var PlanCategories = JSON.parse(response.PlanCategoryLoad_JSON);
        $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlPlanCategory').empty();
        $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlPlanCategory').append('<div class="pl-xs"><input type="checkbox" id="chkPlanCategorySelectAll" onclick ="Admin_FollowUpGroup_Detail.PlanCategorySelectAll(this);" /> <label>Select All</label></div>');
        var div = $('<table class="border table table-condensed" id="dgvPlanCategory"></table>');
        $.each(PlanCategories, function (i, item) {
            var $row = $('<tr/>');
            var $cell1 = $('<td width="22"/>');
            var $cell2 = $('<td/>');
            //var chkSelect;
            //if (item.FacilityId != "")
            //    chkSelect = true;
            //else
            //    chkSelect = false;
            $cell1.append($('<input>').attr({
                type: 'checkbox', id: 'chkPlanCategory' + item.PlanId, checked: false,
            }));
            $cell2.append($('<label style="padding:2px 0 0 0;">').text(item.ShortName));
            $row.append($cell1);
            $row.append($cell2);

            $row.attr("onclick", "utility.SelectGridRow($('#gvPlanCategory_row" + item.PlanId + "'))");
            $row.attr("id", "gvPlanCategory_row" + item.PlanId);
            $row.attr("CurrentPlanCategoryId", item.PlanId);
            div.append($row);
        });
        $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlPlanCategory').last().append(div);
        $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlPlanCategory').addClass("disableAll");

        // if ($("#userDetail #chkForm" + userDetail.formRowSelect).is(':checked')) {
        $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlPlanCategory').removeClass("disableAll");

        //}

        $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlPlanCategory #dgvPlanCategory').on('click', 'tr', function (e) {
            if ($(e.target).is('input[type=checkbox]')) {
                if ($(e.target).is(":checked")) {
                    Admin_FollowUpGroup_Detail.ARPlanCategorySave($(this).closest('tr').attr('CurrentPlanCategoryId'), $(e.target));
                }
                else {
                    Admin_FollowUpGroup_Detail.PlanCategoryARGroupDelete($(e.target).attr('PlanCategoryGroupId'), $(e.target));
                }
                return;
            }
        });
    },
    BindPlanType: function (results) {
        $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlPlanType').empty();
        $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlPlanType').append('<div class="pl-xs"><input type="checkbox" id="chkPlanTypeSelectAll" onclick ="Admin_FollowUpGroup_Detail.PlanTypeSelectAll(this);" /> <label>Select All</label></div>');
        var div = $('<table class="border table table-condensed" id="dgvPlanType"></table>');
        $.each(results, function (i, item) {
            var $row = $('<tr/>');
            var $cell1 = $('<td width="22"/>');
            var $cell2 = $('<td/>');
            //var chkSelect;
            //if (item.FacilityId != "")
            //    chkSelect = true;
            //else
            //    chkSelect = false;
            $cell1.append($('<input>').attr({
                type: 'checkbox', id: 'chkPlanType' + item.Value, checked: false,
            }));
            $cell2.append($('<label style="padding:2px 0 0 0;">').text(item.Name));
            $row.append($cell1);
            $row.append($cell2);

            $row.attr("onclick", "utility.SelectGridRow($('#gvPlanType_row" + item.Value + "'))");
            $row.attr("id", "gvPlanType_row" + item.Value);
            $row.attr("CurrentPlanTypeId", item.Value);
            div.append($row);
        });
        $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlPlanType').last().append(div);
        $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlPlanType').addClass("disableAll");

        // if ($("#userDetail #chkForm" + userDetail.formRowSelect).is(':checked')) {
        $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlPlanType').removeClass("disableAll");

        //}

        $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlPlanType #dgvPlanType').on('click', 'tr', function (e) {
            if ($(e.target).is('input[type=checkbox]')) {
                if ($(e.target).is(":checked")) {
                    Admin_FollowUpGroup_Detail.ARPlanTypeSave($(this).closest('tr').attr('CurrentPlanTypeId'), $(e.target));
                }
                else {
                    Admin_FollowUpGroup_Detail.PlanTypeARGroupDelete($(e.target).attr('PlanTypeGroupId'), $(e.target));
                }
                return;
            }
        });
    },


    ValidateGroupDetail: function () {
        $('#frmAdminFollowUpGroupDetail')
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
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  }
              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           Admin_FollowUpGroup_Detail.GroupSave();
       });
    },

    GroupSave: function () {
        var strMessage = "";
        var self = $('#' + Admin_FollowUpGroup_Detail.params.PanelID);
        var myJSON = self.getMyJSON();
        if (Admin_FollowUpGroup_Detail.params.mode.toLowerCase() == "add") {
            Admin_FollowUpGroup_Detail.SaveGroup(myJSON).done(function (response) {
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    Admin_FollowUpGroup_Detail.params.GroupId = response.GroupId;
                    $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlARGroupSection').removeClass('disableAll');
                    $('#frmAdminFollowUpGroupDetail').data('serialize', $('#frmAdminFollowUpGroupDetail').serialize());
                    Admin_FollowUpGroup_Detail.params.mode = "edit";
                    Admin_FollowUpGroup.GroupSearch();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

        }
        else if (Admin_FollowUpGroup_Detail.params.mode.toLowerCase() == "edit") {
            Admin_FollowUpGroup_Detail.EditGroup(myJSON, Admin_FollowUpGroup_Detail.params.GroupId).done(function (response) {
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    $('#frmAdminFollowUpGroupDetail').data('serialize', $('#frmAdminFollowUpGroupDetail').serialize());
                    Admin_FollowUpGroup.GroupSearch();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    SaveGroup: function (groupData) {
        var data = "groupData=" + groupData;
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_GROUP", "SAVE_GROUP");
    },
    EditGroup: function (groupData, groupId) {
        var data = "groupData=" + groupData + "&groupId=" + groupId;
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_GROUP", "UPDATE_GROUP");
    },

    GroupFill: function () {
        Admin_FollowUpGroup_Detail.FillGroup(Admin_FollowUpGroup_Detail.params.GroupId).done(function (response) {
            if (response.status != false) {

                var self = $('#' + Admin_FollowUpGroup_Detail.params.PanelID);
                utility.bindMyJSON(true, JSON.parse(response.GroupLoad_JSON), false, self);
                $.each($.parseJSON(response.FollowUpFacilityGroupLoad_JSON), function (i, item) {
                    $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #chkFacility' + item.FacilityId).prop('checked', 'checked')
                    $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #chkFacility' + item.FacilityId).attr('FacilityGroupId', item.FacilityGrpId);
                });

                $.each($.parseJSON(response.FollowUpPOSGroupLoad_JSON), function (i, item) {
                    $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #chkPOS' + item.PlaceOfServiceId).prop('checked', 'checked');
                    $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #chkPOS' + item.PlaceOfServiceId).attr('POSGroupId', item.POSGrpId);
                });

                $.each($.parseJSON(response.FollowUpPlanCategoryGroupLoad_JSON), function (i, item) {
                    $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #chkPlanCategory' + item.PlanCategoryId).prop('checked', 'checked');
                    $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #chkPlanCategory' + item.PlanCategoryId).attr('plancategorygroupid', item.PCGrpId);
                });

                $.each($.parseJSON(response.FollowUpPlanTypeGroupLoad_JSON), function (i, item) {
                    $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #chkPlanType' + item.PlanTypeId).prop('checked', 'checked');
                    $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #chkPlanType' + item.PlanTypeId).attr('PlanTypeGroupId', item.PTGrpId);
                });

                $.each($.parseJSON(response.FollowUpProviderGroupLoad_JSON), function (i, item) {
                    $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #chkProvider' + item.ProviderId).prop('checked', 'checked');
                    $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #chkProvider' + item.ProviderId).attr('providerGroupId', item.ProviderGrpId);
                });

                if ($("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlFacility #dgvFacility input[type=checkbox]:checked').length == $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlFacility #dgvFacility input[type=checkbox]').length) {
                    $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlFacility #chkFaciltySelectAll').prop('checked', 'checked');
                }
                if ($("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlProvider #dgvProvider input[type=checkbox]:checked').length == $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlProvider #dgvProvider input[type=checkbox]').length) {
                    $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlProvider #chkProviderSelectAll').prop('checked', 'checked');
                }
                if ($("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlPOS #dgvPOS input[type=checkbox]:checked').length == $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlPOS #dgvPOS input[type=checkbox]').length) {
                    $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlPOS #chkPOSSelectAll').prop('checked', 'checked');
                }
                if ($("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlPlanType #dgvPlanType input[type=checkbox]:checked').length == $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlPlanType #dgvPlanType input[type=checkbox]').length) {
                    $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlPlanType #chkPlanTypeSelectAll').prop('checked', 'checked');
                }
                if ($("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlPlanCategory #dgvPlanCategory input[type=checkbox]:checked').length == $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlPlanCategory #dgvPlanCategory input[type=checkbox]').length) {
                    $("#" + Admin_FollowUpGroup_Detail.params.PanelID + ' #pnlPlanCategory #chkPlanCategorySelectAll').prop('checked', 'checked');
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
            $('#frmAdminFollowUpGroupDetail').data('serialize', $('#frmAdminFollowUpGroupDetail').serialize());
        });
    },
    FillGroup: function (groupId) {
        var data = "groupId=" + groupId;
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_GROUP", "FILL_GROUP");
    },
    //___________________________________________________________________________________________
            // Provider AR Group

    ARProviderSave: function (providerId,objChkDOM) {
        if (Admin_FollowUpGroup_Detail.params.mode.toLowerCase() == "edit")
        {
            Admin_FollowUpGroup_Detail.saveProviderAR(providerId).done(function (response) {
                if (response.status != false) {
                    $(objChkDOM).attr('providerGroupId', response.ProviderGroupId);
                    utility.DisplayMessages(response.Message, 1);
                }

            });
        }

    },
    saveProviderAR: function (providerId) {
        var data = "providerId=" + providerId+"&ARGroupId="+Admin_FollowUpGroup_Detail.params.GroupId;
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_GROUP", "SAVE_AR_PROVIDER_GROUP");
    },
    providerARGroupDelete: function (providerGroupId, objChkDOM) {
        Admin_FollowUpGroup_Detail.deleteProviderARGroup(providerGroupId).done(function (response) {
            if (response.status != false) {
                $(objChkDOM).removeAttr('providerGroupId');
                utility.DisplayMessages(response.Message, 1);
            }

        });
    },
    deleteProviderARGroup: function (providerGroupId) {
        var data = "providerGroupId=" + providerGroupId;
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_GROUP", "DELETE_AR_PROVIDER_GROUP");
    },
    ProviderSelectAll: function (ev) {
        var AllProvider = [];
        if (!ev.checked) {
            $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #dgvProvider input[type=checkbox]').each(function (i, item) {
                if ($(this).is(":checked") == true) {
                    AllProvider.push({ 'ProviderId': $(this).attr('providergroupid') });
                    $(this).prop('checked', false);
                }
            });
        }
        else {
            $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #dgvProvider input[type=checkbox]').each(function (i, item) {
                if ($(this).is(":checked") == false) {
                    AllProvider.push({ 'ProviderId': $(this).closest('tr').attr('currentproviderid') });
                    $(this).prop('checked', true);
                }
            });
        }

        var ProviderJSON = JSON.stringify(AllProvider);
        Admin_FollowUpGroup_Detail.SecurityProviderCheckAll(ProviderJSON);
    },
    SecurityProviderCheckAll: function (ProviderJSON) {
        if ($('#' + Admin_FollowUpGroup_Detail.params.PanelID + "  #chkProviderSelectAll").is(':checked')) {
            Admin_FollowUpGroup_Detail.SaveSecurityProviderCheckAll(ProviderJSON).done(function (response) {
                if (response.status != false) {
                    Facilities = JSON.parse(response.Provider_JSON)
                    $.each(Facilities, function (i, item) {
                        $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #chkProvider' + item.ProviderId).attr('providergroupid', item.ProviderGrpId);
                    });
                    CacheManager.BindCodes('GetProvider', true);
                    utility.DisplayMessages(response.Message, 1);
                }
            });
        }
        else {
            Admin_FollowUpGroup_Detail.DeleteSecurityProviderCheckAll(ProviderJSON).done(function (response) {
                if (response.status != false) {
                    CacheManager.BindCodes('GetProvider', true);
                    utility.DisplayMessages(response.Message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    SaveSecurityProviderCheckAll: function (ProviderJSON) {
        var data = "&ProviderJSON=" + ProviderJSON + "&ARGroupId=" + Admin_FollowUpGroup_Detail.params.GroupId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_GROUP", "SAVE_AR_GROUP_PROVIDER_CHECK_ALL");
    },
    // END Facility AR Group
    DeleteSecurityProviderCheckAll: function (ProviderJSON) {
        var data = "&ProviderJSON=" + ProviderJSON;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_GROUP", "DELETE_AR_GROUP_PROVIDER_CHECK_ALL");
    },
    // END Provider AR Group

//___________________________________________________________________________________________
         // Facility AR Group

    ARFacilitySave: function (FacilityId, objChkDOM) {
        if (Admin_FollowUpGroup_Detail.params.mode.toLowerCase() == "edit") {
            Admin_FollowUpGroup_Detail.saveFacilityAR(FacilityId).done(function (response) {
                if (response.status != false) {
                    $(objChkDOM).attr('FacilityGroupId', response.FacilityGroupId);
                    utility.DisplayMessages(response.Message, 1);
                }

            });
        }

    },
    saveFacilityAR: function (FacilityId) {
        var data = "FacilityId=" + FacilityId + "&ARGroupId=" + Admin_FollowUpGroup_Detail.params.GroupId;
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_GROUP", "SAVE_AR_FACILITY_GROUP");
    },
    FacilityARGroupDelete: function (FacilityGroupId, objChkDOM) {
        Admin_FollowUpGroup_Detail.deleteFacilityARGroup(FacilityGroupId).done(function (response) {
            if (response.status != false) {
                $(objChkDOM).removeAttr('FacilityGroupId');
                utility.DisplayMessages(response.Message, 1);
            }

        });
    },
    deleteFacilityARGroup: function (FacilityGroupId) {
        var data = "FacilityGroupId=" + FacilityGroupId;
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_GROUP", "DELETE_AR_FACILITY_GROUP");
    },

    FacilitySelectAll: function (ev) {
        var AllFacility = [];
        if (!ev.checked) {
            $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #dgvFacility input[type=checkbox]').each(function (i, item) {
                if ($(this).is(":checked") == true) {
                    AllFacility.push({ 'FacilityId': $(this).attr('facilitygroupid')});
                    $(this).prop('checked', false);
                }
            });
        }
        else {
            $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #dgvFacility input[type=checkbox]').each(function (i, item) {
                if ($(this).is(":checked") == false) {
                    AllFacility.push({ 'FacilityId': $(this).closest('tr').attr('CurrentFacilityId')});
                    $(this).prop('checked', true);
                }
            });
        }

        var FacilityJSON = JSON.stringify(AllFacility);
        Admin_FollowUpGroup_Detail.SecurityFacilityCheckAll(FacilityJSON);
    },
    SecurityFacilityCheckAll: function (FacilityJSON) {
        if ($('#' + Admin_FollowUpGroup_Detail.params.PanelID + "  #chkFaciltySelectAll").is(':checked')) {
            Admin_FollowUpGroup_Detail.SaveSecurityFacilityCheckAll(FacilityJSON).done(function (response) {
                        if (response.status != false) {
                            Facilities = JSON.parse(response.Facility_JSON)
                            $.each(Facilities, function (i, item) {
                                $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #chkFacility' + item.FacilityId).attr('facilitygroupid', item.FacilityGrpId);
                            });
                            CacheManager.BindCodes('GetFacility', true);
                            utility.DisplayMessages(response.Message, 1);
                        }
                    });
                }
                else {
            Admin_FollowUpGroup_Detail.DeleteSecurityFacilityCheckAll(FacilityJSON).done(function (response) {
                        if (response.status != false) {
                            CacheManager.BindCodes('GetFacility', true);
                            utility.DisplayMessages(response.Message, 1);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
},
    SaveSecurityFacilityCheckAll: function (FacilityJSON) {
    var data = "&FacilityJSon=" + FacilityJSON + "&ARGroupId=" + Admin_FollowUpGroup_Detail.params.GroupId;
    // serach parameter , class name, command name of class 
    return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_GROUP", "SAVE_AR_GROUP_FACILITY_CHECK_ALL");
},
    // END Facility AR Group
    DeleteSecurityFacilityCheckAll: function (FacilityJSON) {
    var data = "&FacilityJSON=" + FacilityJSON;
    // serach parameter , class name, command name of class 
    return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_GROUP", "DELETE_AR_GROUP_FACILITY_CHECK_ALL");
},

    //___________________________________________________________________________________________
    // Plan Category AR Group

    ARPlanCategorySave: function (PlanCategoryId, objChkDOM) {
        if (Admin_FollowUpGroup_Detail.params.mode.toLowerCase() == "edit") {
            Admin_FollowUpGroup_Detail.savePlanCategoryAR(PlanCategoryId).done(function (response) {
                if (response.status != false) {
                    $(objChkDOM).attr('PlanCategoryGroupId', response.PlanCategoryGroupId);
                    utility.DisplayMessages(response.Message, 1);
                }

            });
        }

    },
    savePlanCategoryAR: function (PlanCategoryId) {
        var data = "PlanCategoryId=" + PlanCategoryId + "&ARGroupId=" + Admin_FollowUpGroup_Detail.params.GroupId;
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_GROUP", "SAVE_AR_PLAN_CATEGORY_GROUP");
    },
    PlanCategoryARGroupDelete: function (PlanCategoryGroupId, objChkDOM) {
        Admin_FollowUpGroup_Detail.deletePlanCategoryARGroup(PlanCategoryGroupId).done(function (response) {
            if (response.status != false) {
                $(objChkDOM).removeAttr('PlanCategoryGroupId');
                utility.DisplayMessages(response.Message, 1);
            }

        });
    },
    deletePlanCategoryARGroup: function (PlanCategoryGroupId) {
        var data = "PlanCategoryGroupId=" + PlanCategoryGroupId;
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_GROUP", "DELETE_AR_PLAN_CATEGORY_GROUP");
    },
    PlanCategorySelectAll: function (ev) {
        var AllPC = [];
        if (!ev.checked) {
            $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #dgvPlanCategory input[type=checkbox]').each(function (i, item) {
                if ($(this).is(":checked") == true) {
                    AllPC.push({ 'PlanCategoryId': $(this).attr('plancategorygroupid') });
                    $(this).prop('checked', false);
                }
            });
        }
        else {
            $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #dgvPlanCategory input[type=checkbox]').each(function (i, item) {
                if ($(this).is(":checked") == false) {
                    AllPC.push({ 'PlanCategoryId': $(this).closest('tr').attr('currentplancategoryid') });
                    $(this).prop('checked', true);
                }
            });
        }

        var PCJSON = JSON.stringify(AllPC);
        Admin_FollowUpGroup_Detail.SecurityPlanCategoryCheckAll(PCJSON);
    },
    SecurityPlanCategoryCheckAll: function (PCJSON) {
        if ($('#' + Admin_FollowUpGroup_Detail.params.PanelID + "  #chkPlanCategorySelectAll").is(':checked')) {
            Admin_FollowUpGroup_Detail.SaveSecurityPlanCategoryCheckAll(PCJSON).done(function (response) {
                if (response.status != false) {
                    PCs = JSON.parse(response.PC_JSON)
                    $.each(PCs, function (i, item) {
                        $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #chkPlanCategory' + item.PlanCategoryId).attr('plancategorygroupid', item.PCGrpId);
                    });
                    utility.DisplayMessages(response.Message, 1);
                }
            });
        }
        else {
            Admin_FollowUpGroup_Detail.DeleteSecurityPlanCategoryCheckAll(PCJSON).done(function (response) {
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    SaveSecurityPlanCategoryCheckAll: function (PCJSON) {
        var data = "PCJSON=" + PCJSON + "&ARGroupId=" + Admin_FollowUpGroup_Detail.params.GroupId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_GROUP", "SAVE_AR_GROUP_PLANCATEGORY_CHECK_ALL");
    },
    // END Facility AR Group
    DeleteSecurityPlanCategoryCheckAll: function (PCJSON) {
        var data = "PCJSON=" + PCJSON;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_GROUP", "DELETE_AR_GROUP_PLANCATEGORY_CHECK_ALL");
    },
    // END  Plan Category AR Group


    //___________________________________________________________________________________________
    // POS AR Group

    ARPOSSave: function (POSId, objChkDOM) {
        if (Admin_FollowUpGroup_Detail.params.mode.toLowerCase() == "edit") {
            Admin_FollowUpGroup_Detail.savePOSAR(POSId).done(function (response) {
                if (response.status != false) {
                    $(objChkDOM).attr('POSGroupId', response.POSGroupId);
                    utility.DisplayMessages(response.Message, 1);
                }

            });
        }

    },
    savePOSAR: function (POSId) {
        var data = "POSId=" + POSId + "&ARGroupId=" + Admin_FollowUpGroup_Detail.params.GroupId;
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_GROUP", "SAVE_AR_POS_GROUP");
    },
    POSARGroupDelete: function (POSGroupId, objChkDOM) {
        Admin_FollowUpGroup_Detail.deletePOSARGroup(POSGroupId).done(function (response) {
            if (response.status != false) {
                $(objChkDOM).removeAttr('POSGroupId');
                utility.DisplayMessages(response.Message, 1);
            }

        });
    },
    deletePOSARGroup: function (POSGroupId) {
        var data = "POSGroupId=" + POSGroupId;
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_GROUP", "DELETE_AR_POS_GROUP");
    },
    POSSelectAll: function (ev) {
        var AllPOS = [];
        if (!ev.checked) {
            $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #dgvPOS input[type=checkbox]').each(function (i, item) {
                if ($(this).is(":checked") == true) {
                    AllPOS.push({ 'POSId': $(this).attr('posgroupid') });
                    $(this).prop('checked', false);
                }
            });
        }
        else {
            $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #dgvPOS input[type=checkbox]').each(function (i, item) {
                if ($(this).is(":checked") == false) {
                    AllPOS.push({ 'POSId': $(this).closest('tr').attr('currentposid') });
                    $(this).prop('checked', true);
                }
            });
        }

        var POSJSON = JSON.stringify(AllPOS);
        Admin_FollowUpGroup_Detail.SecurityPOSCheckAll(POSJSON);
    },
    SecurityPOSCheckAll: function (POSJSON) {
        if ($('#' + Admin_FollowUpGroup_Detail.params.PanelID + "  #chkPOSSelectAll").is(':checked')) {
            Admin_FollowUpGroup_Detail.SaveSecurityPOSCheckAll(POSJSON).done(function (response) {
                if (response.status != false) {
                    POSs = JSON.parse(response.POS_JSON)
                    $.each(POSs, function (i, item) {
                        $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #chkPOS' + item.PlaceOfServiceId).attr('posgroupid', item.POSGrpId);
                    });
                    utility.DisplayMessages(response.Message, 1);
                }
            });
        }
        else {
            Admin_FollowUpGroup_Detail.DeleteSecurityPOSCheckAll(POSJSON).done(function (response) {
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    SaveSecurityPOSCheckAll: function (POSJSON) {
        var data = "&POSJSON=" + POSJSON + "&ARGroupId=" + Admin_FollowUpGroup_Detail.params.GroupId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_GROUP", "SAVE_AR_GROUP_POS_CHECK_ALL");
    },
    // END Facility AR Group
    DeleteSecurityPOSCheckAll: function (POSJSON) {
        var data = "&POSJSON=" + POSJSON;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_GROUP", "DELETE_AR_GROUP_POS_CHECK_ALL");
    },
    // END  POS AR Group

    //___________________________________________________________________________________________
    // Plan Type AR Group

    ARPlanTypeSave: function (PlanTypeId, objChkDOM) {
        if (Admin_FollowUpGroup_Detail.params.mode.toLowerCase() == "edit") {
            Admin_FollowUpGroup_Detail.savePlanTypeAR(PlanTypeId).done(function (response) {
                if (response.status != false) {
                    $(objChkDOM).attr('PlanTypeGroupId', response.PlanTypeGroupId);
                    utility.DisplayMessages(response.Message, 1);
                }

            });
        }

    },
    savePlanTypeAR: function (PlanTypeId) {
        var data = "PlanTypeId=" + PlanTypeId + "&ARGroupId=" + Admin_FollowUpGroup_Detail.params.GroupId;
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_GROUP", "SAVE_AR_PLAN_TYPE_GROUP");
    },
    PlanTypeARGroupDelete: function (PlanTypeGroupId, objChkDOM) {
        Admin_FollowUpGroup_Detail.deletePlanTypeARGroup(PlanTypeGroupId).done(function (response) {
            if (response.status != false) {
                $(objChkDOM).removeAttr('PlanTypeGroupId');
                utility.DisplayMessages(response.Message, 1);
            }

        });
    },
    deletePlanTypeARGroup: function (PlanTypeGroupId) {
        var data = "PlanTypeGroupId=" + PlanTypeGroupId;
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_GROUP", "DELETE_AR_PLAN_TYPE_GROUP");
    },
    PlanTypeSelectAll: function (ev) {
        var AllPT = [];
        if (!ev.checked) {
            $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #dgvPlanType input[type=checkbox]').each(function (i, item) {
                if ($(this).is(":checked") == true) {
                    AllPT.push({ 'PlanTypeId': $(this).attr('plantypegroupid') });
                    $(this).prop('checked', false);
                }
            });
        }
        else {
            $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #dgvPlanType input[type=checkbox]').each(function (i, item) {
                if ($(this).is(":checked") == false) {
                    AllPT.push({ 'PlanTypeId': $(this).closest('tr').attr('currentplantypeid') });
                    $(this).prop('checked', true);
                }
            });
        }

        var PTJSON = JSON.stringify(AllPT);
        Admin_FollowUpGroup_Detail.SecurityPlanTypeCheckAll(PTJSON);
    },
    SecurityPlanTypeCheckAll: function (PTJSON) {
        if ($('#' + Admin_FollowUpGroup_Detail.params.PanelID + "  #chkPlanTypeSelectAll").is(':checked')) {
            Admin_FollowUpGroup_Detail.SaveSecurityPlanTypeCheckAll(PTJSON).done(function (response) {
                if (response.status != false) {
                    PTs = JSON.parse(response.PT_JSON)
                    $.each(PTs, function (i, item) {
                        $('#' + Admin_FollowUpGroup_Detail.params.PanelID + ' #chkPlanType' + item.PlanTypeId).attr('plantypegroupid', item.PTGrpId);
                    });
                    utility.DisplayMessages(response.Message, 1);
                }
            });
        }
        else {
            Admin_FollowUpGroup_Detail.DeleteSecurityPlanTypeCheckAll(PTJSON).done(function (response) {
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    SaveSecurityPlanTypeCheckAll: function (PTJSON) {
        var data = "&PTJSON=" + PTJSON + "&ARGroupId=" + Admin_FollowUpGroup_Detail.params.GroupId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_GROUP", "SAVE_AR_GROUP_PLANTYPE_CHECK_ALL");
    },
    // END Facility AR Group
    DeleteSecurityPlanTypeCheckAll: function (PTJSON) {
        var data = "&PTJSON=" + PTJSON;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_GROUP", "DELETE_AR_GROUP_PLANTYPE_CHECK_ALL");
    },
    // END  Plan type AR Group

    
    UnLoad: function () {

        utility.UnLoadDialog(Admin_FollowUpGroup_Detail.params.PanelID + ' #frmAdminFollowUpGroupDetail', function () {
            UnloadActionPan(Admin_FollowUpGroup_Detail.params["ParentCtrl"], "Admin_FollowUpGroup_Detail");
        }, function () {
            UnloadActionPan(Admin_FollowUpGroup_Detail.params["ParentCtrl"], "Admin_FollowUpGroup_Detail");
        });

    },
}