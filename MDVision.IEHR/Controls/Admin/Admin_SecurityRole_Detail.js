securityRoleDetail = {
    params: [],
    roleID: -1,
    moduleID: -1,
    moduleFormID: -1,
    moduleFormRolePrivilegeID: -1,
    formRowSelect: 0,
    moduleRowSelect: 0,
    IsFirstLoad: false,

    Load: function (params) {
        securityRoleDetail.params = params;
        securityRoleDetail.roleID = -1;
        securityRoleDetail.moduleID = -1;
        securityRoleDetail.moduleFormID = -1;
        securityRoleDetail.moduleFormRolePrivilegeID = -1;
        securityRoleDetail.formRowSelect = 0;
        securityRoleDetail.moduleRowSelect = 0;
        securityRoleDetail.IsFirstLoad = false;
        securityRoleDetail.LoadSecurityRole();

    },

    LoadSecurityRole: function () {
        securityRoleDetail.FillSecurityRole(securityRoleDetail.params.SecurityRoleId, 0, 0).done(function (response) {

            if (response.status != false) {
                $("#securityRoleDetail #pnlRights").removeClass('disableAll');

                securityRoleDetail.BindModules(response);
                securityRoleDetail.BindModuleForms(response);
                securityRoleDetail.BindPrivileges(response);
                if (securityRoleDetail.params.mode == "Add") {
                    $('#securityRoleDetail #txtShortName').attr("enabled", "enabled");
                    $('#frmRoleDetail').data('serialize', $('#frmRoleDetail').serialize());

                    if (globalAppdata['AppUserName'] != DefaultUser) {
                        $('#securityRoleDetail #pnlIsAdmin').css("display", "none");
                    }
                    $("#securityRoleDetail #pnlRights").addClass('disableAll');


                    securityRoleDetail.ValidateSecurityRole();
                }
                else if (securityRoleDetail.params.mode == "Edit") {

                    $("#securityRoleDetail h4").text("Edit Security Roles"); // Add || Talha Tanweer || 23 August 2016 || EMR-728

                    if (globalAppdata['AppUserName'] != DefaultUser) {
                        $('#securityRoleDetail #pnlIsAdmin').css("display", "none");
                        //$('#securityRoleDetail #pnlIsAdmin').css("display", "none");
                        if (securityRoleDetail.params.IsAdmin == "True") {
                            $("#securityRoleDetail #pnlRights").addClass('disableAll');
                        }
                    }

                    //$("#txtShortName").attr("disabled");
                    $("#securityRoleDetail #txtShortName").attr("disabled", "disabled");
                    securityRoleDetail.BindRole(response);
                    securityRoleDetail.ValidateSecurityRole();
                    $('#frmRoleDetail').data('serialize', $('#frmRoleDetail').serialize());
                }
            }
            else {
                UnloadActionPan();
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    BindRole: function (response) {
        var Role_Detail = JSON.parse(response.Roles_JSON);

        //if (globalAppdata['AppUserName'] != DefaultUser) {
        //    if (securityRoleDetail.params.IsAdmin == "True") {
        //        $("#securityRoleDetail #pnlModules").addClass('disableAll');
        //        $("#securityRoleDetail #pnlPrivileges").addClass('disableAll');
        //        $("#securityRoleDetail #pnlForms").addClass('disableAll');
        //    }
        //}

        var self = $("#securityRoleDetail");

        utility.bindMyJSON(true, Role_Detail, false, self);
        if (Role_Detail.chkActive == 'True')
            $("#securityRoleDetail #chkActive").attr("checked", true);
        else
            $("#securityRoleDetail #chkActive").attr("checked", false);
        if (Role_Detail.chkIsAdmin == "True") {
            $("#securityRoleDetail #chkIsAdmin").attr("checked", true);
        }
        else
            $("#securityRoleDetail #chkIsAdmin").attr("checked", false);
        //Start || 13 April, 2016 || ZeeshanAK || Changes for DOC 33- Emergency Access 

        if (Role_Detail.roleType == "Emergency Access") {
            $("#securityRoleDetail #roleType option:contains(Emergency Access)").attr('selected', true);
        }
        else
            $("#securityRoleDetail #roleType option:contains(Regular)").attr('selected', true);
        //End   || 13 April, 2016 || ZeeshanAK || Changes for DOC 33- Emergency Access 
    },

    BindModules: function (response) {
        var Module_Detail = JSON.parse(response.Modules_JSON);
        $("#securityRoleDetail #pnlModules").empty();
        var div = $('<table class="border table table-condensed" id="dgvModules"></table>');
        $.each(Module_Detail, function (i, item) {
            var $row = $('<tr/>');
            var $cell1 = $('<td width="22"/>');
            var $cell2 = $('<td />');
            if (securityRoleDetail.moduleRowSelect == 0)
                securityRoleDetail.moduleRowSelect = item.ModuleId;
            var chkSelect;
            if (item.IsSelected == 'True')
                chkSelect = true;
            else
                chkSelect = false;
            $cell1.append($('<input>').attr({
                type: 'checkbox', id: 'chkModules' + item.ModuleId, ModuleId: item.ModuleId, checked: chkSelect, //onclick: 'securityRoleDetail.ModuleDelete(chkModules' + item.ModuleId + ', ' + item.ModuleId + ', "CHECKED")',
            }));
            $cell2.append($('<label style="padding:2px 0 0 0;">').text(item.Name));
            $row.append($cell1);
            $row.append($cell2);

            $row.attr("onclick", "utility.SelectGridRow($('#gvModule_row" + item.ModuleId + "')," + item.ModuleId + ");");//securityRoleDetail.ModuleDelete(chkModules" + item.ModuleId + ", " + item.ModuleId + ", 'SELECT')
            //$row.attr("onclick", "utility.SelectGridRow($('#gvModule_row" + item.ModuleId + "')," + item.ModuleId + ");");
            $row.attr("id", "gvModule_row" + item.ModuleId);
            $row.attr("CurrentModuleId", item.ModuleId);
            div.append($row);
        });
        $("#securityRoleDetail #pnlModules").last().append(div);
        $("#securityRoleDetail #gvModule_row" + securityRoleDetail.moduleRowSelect).addClass('active');
        $("#dgvModules").on('click', 'tr', function (e) {
            if ($(e.target).is('input[type=checkbox]')) {
                //alert('1');
                securityRoleDetail.ModuleDelete($(this).closest('tr').attr('CurrentModuleId'));
                return;
            }
            else if ($(this).hasClass('active')) {
                //alert('2');
                securityRoleDetail.FillFormPrivilegesByModule($(this).closest('tr').attr('CurrentModuleId'));
            }
            //else {
            //    $('tr.selected').removeClass('active');
            //    alert('3');
            //    $(this).addClass('active');
            //}
        });
    },

    BindModuleForms: function (response) {
        var ModuleForm_Detail = JSON.parse(response.Forms_JSON);
        $("#securityRoleDetail #pnlForms").empty();
        var div = $('<table class="border table table-condensed" id="dgvForms"></table>');

        var prevFormGroupName = "";
        var secondChild = "";
        $.each(ModuleForm_Detail, function (i, item) {
            var $row = $('<tr/>');

            var $cell1 = $('<td width="22"/>');
            var $cell2 = $('<td/>');
            if (securityRoleDetail.formRowSelect == 0 && securityRoleDetail.IsFirstLoad == false) {
                securityRoleDetail.IsFirstLoad = true;
                securityRoleDetail.formRowSelect = item.FormsId;
                securityRoleDetail.moduleFormRoleID = item.MFRId;
            }
            var chkSelect;
            if (item.MFRId != "")
                chkSelect = true;
            else
                chkSelect = false;
            item.FormName = item.FormName.replace("FaceSheet", "ClinicalSummary");
            item.FormName = item.FormName.replace("Face Sheet", "Clinical Summary");

            var arrFormName = item.FormName.split('_');
            var currentFormGroupName = "";
            var currentFormName = "";
            var currentsecondChild = "";
            if (arrFormName.length > 1) {
                currentFormGroupName = arrFormName[0];
                currentFormName = arrFormName[1];
                if (arrFormName.length > 2) {
                    currentsecondChild = arrFormName[1];
                    currentFormName = arrFormName[2];
                }
                if (prevFormGroupName != currentFormGroupName) {
                    prevFormGroupName = currentFormGroupName;

                    var $rowGroup = $('<tr/>');

                    var $cellGroup2 = $('<td colspan=2 />');

                    $cellGroup2.append($('<label style="padding:2px 0 0 0; font-weight:bold;">').text(prevFormGroupName));

                    $rowGroup.append($cellGroup2);
                    div.append($rowGroup);
                }
                if (secondChild != currentsecondChild) {
                    secondChild = currentsecondChild;
                    var $rowGroup = $('<tr/>');

                    var $cellGroup2 = $('<td colspan=2 />');

                    $cellGroup2.append($('<label style="padding:2px 0 0 10px; font-weight:bold;">').text(secondChild));

                    $rowGroup.append($cellGroup2);
                    div.append($rowGroup);

                }

                $cell1.append($('<input>').attr({
                    type: 'checkbox', id: 'chkForm' + item.FormsId, FormId: item.FormsId, checked: chkSelect, ModuleFormRoleId: item.MFRId, style: 'margin-left: 15px;'

                }));
            }
            else {
                currentFormName = item.FormName;

                $cell1.append($('<input>').attr({
                    type: 'checkbox', id: 'chkForm' + item.FormsId, FormId: item.FormsId, checked: chkSelect, ModuleFormRoleId: item.MFRId,
                    //type: 'checkbox', id: 'chkForm' + item.FormsId, FormId: item.FormsId, checked: chkSelect, ModuleFormRoleId: item.MFRId, onclick: 'securityRoleDetail.ModuleFormSave(' + item.FormsId + ', ' + item.ModuleFormId + ',' + item.ModuleId + ', "' + item.MFRId + '", "CHECKED")',
                }));

            }


            $cell2.append($('<label style="padding:2px 0 0 0;">').text(currentFormName));


            $row.append($cell1);
            $row.append($cell2);

            $row.attr("onclick", "utility.SelectGridRow($('#gvForm_row" + item.FormsId + "'));");//securityRoleDetail.ModuleFormSave(" + item.FormsId + ", " + item.ModuleFormId + "," + item.ModuleId + ", '" + item.MFRId + "', 'SELECT')


            $row.attr("id", "gvForm_row" + item.FormsId);
            $row.attr("CurrentFormId", item.FormsId);
            $row.attr("ModuleFormId", item.ModuleFormId);
            $row.attr("CurrentModuleId", item.ModuleId);
            $row.attr("ModuleFormRoleId", item.MFRId);

            //Start 03-04-2017 Humaira Yousaf to show Sign right above Co-Sign
            if (item.FormName == "Notes_Sign") {
                $row.insertAfter($($(div).find("label:contains('Notes')")[0]).parents('tr'));
            }
            else {
                div.append($row);
            }
            //End 03-04-2017 Humaira Yousaf to show Sign right above Co-Sign
        });
        $("#securityRoleDetail #pnlForms").last().append(div);
        $("#securityRoleDetail #gvForm_row" + securityRoleDetail.formRowSelect).addClass('active');
        $("#securityRoleDetail #pnlForms").addClass("disableAll");

        if ($("#securityRoleDetail #chkModules" + securityRoleDetail.moduleRowSelect).is(':checked')) {
            $("#securityRoleDetail #pnlForms").removeClass("disableAll");
        }
        $("#dgvForms").on('click', 'tr', function (e) {
            if ($(e.target).is('input[type=checkbox]')) {
                //alert('1');
                securityRoleDetail.ModuleFormSave($(this).closest('tr').attr('CurrentFormId'), $(this).closest('tr').attr('ModuleFormId'), $(this).closest('tr').attr('CurrentModuleId'), $(this).closest('tr').attr('ModuleFormRoleId'));
                return;
            }
            else if ($(this).hasClass('active')) {
                //alert('2');
                securityRoleDetail.FillFormPrivilegesByForm($(this).closest('tr').attr('CurrentFormId'), $(this).closest('tr').attr('ModuleFormId'), $(this).closest('tr').attr('CurrentModuleId'), $(this).closest('tr').attr('ModuleFormRoleId'));
            }
        });
    },

    BindPrivileges: function (response) {
        var Privileges_Detail = JSON.parse(response.Privileges_JSON);
        $("#securityRoleDetail #pnlPrivileges").empty();

        $("#pnlPrivileges").append('<div class="pl-xs"><input type="checkbox" id="chkPrivliageSelectAll" onclick ="securityRoleDetail.PriviligeSelectAll(this);" /> <label>Select All</label></div>');

        var div = $('<table class="border table table-condensed" id="dgvPrivileges"></table>');
        $.each(Privileges_Detail, function (i, item) {
            var $row = $('<tr/>');
            var $cell1 = $('<td width="22"/>');
            var $cell2 = $('<td/>');
            var chkSelect;
            if (item.ModuleFormRolePrivilegesId != "")
                chkSelect = true;
            else
                chkSelect = false;
            $cell1.append($('<input>').attr({
                type: 'checkbox', id: 'chkPrivileges' + item.PrivilegeSelectionid, PrivilegeId: item.PrivilegeSelectionid, ModuleFormRolePrivilegeId: item.ModuleFormRolePrivilegesId, checked: chkSelect, //onclick: 'securityRoleDetail.ModuleFormPrivilegesSave(chkPrivileges' + item.PrivilegeSelectionid + ', ' + item.PrivilegeSelectionid + ')',
            }));
            $cell2.append($('<label style="padding:2px 0 0 0;">').text(item.PrivilegeName));
            $row.append($cell1);
            $row.append($cell2);

            $row.attr("onclick", "utility.SelectGridRow($('#gvPrivilege_row" + item.PrivilegeSelectionid + "'))");
            $row.attr("id", "gvPrivilege_row" + item.PrivilegeSelectionid);
            $row.attr("CurrentPrivilegeId", item.PrivilegeSelectionid);
            $row.attr("ModuleFormRolePrivilegeId", item.ModuleFormRolePrivilegesId);
            div.append($row);
        });
        $("#securityRoleDetail #pnlPrivileges").last().append(div);
        $("#securityRoleDetail #pnlPrivileges").addClass("disableAll");

        if ($("#securityRoleDetail #chkForm" + securityRoleDetail.formRowSelect).is(':checked')) {
            $("#securityRoleDetail #pnlPrivileges").removeClass("disableAll");

            if ($('#' + securityRoleDetail.params.PanelID + ' #dgvPrivileges input[type=checkbox]:checked').length == $('#' + securityRoleDetail.params.PanelID + ' #dgvPrivileges input[type=checkbox]').length) {
                $('#' + securityRoleDetail.params.PanelID + ' #pnlPrivileges #chkPrivliageSelectAll').attr('checked', 'checked')
            }

        }
        $("#dgvPrivileges").on('click', 'tr', function (e) {
            if ($(e.target).is('input[type=checkbox]')) {
                //console.log($(e.target).is(':checked'));

                if ($(e.target).is(':checked') == false) {
                    if ($('#' + securityRoleDetail.params.PanelID + ' #pnlPrivileges #chkPrivliageSelectAll').is(':checked')) {
                        $('#' + securityRoleDetail.params.PanelID + ' #pnlPrivileges #chkPrivliageSelectAll').attr('checked', false);
                    }
                }

                securityRoleDetail.ModuleFormPrivilegesSave($(this).closest('tr').attr('CurrentPrivilegeId'));
                return;
            }
            //else if ($(this).hasClass('active')) {
            //    alert('2');
            //    FillFormPrivileges($(this).closest('tr').attr('CurrentModuleId'));
            //}
        });
    },

    ModuleDelete: function (ModuleId) {
        securityRoleDetail.formRowSelect = 0;
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Security Roles", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                //if (Action == 'CHECKED') {
                if (!$('#chkModules' + ModuleId).is(":checked")) {
                    securityRoleDetail.DeleteModule(securityRoleDetail.params.SecurityRoleId, ModuleId).done(function (response) {
                        if (response.status == false) {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                //else
                //    $("#securityRoleDetail #gvModule_row" + securityRoleDetail.moduleRowSelect).addClass('active');

                //}
                //else {
                //    securityRoleDetail.moduleRowSelect = ModuleId;
                //}

                securityRoleDetail.FillSecurityRole(securityRoleDetail.params.SecurityRoleId, ModuleId, 0).done(function (response) {
                    if (response.status != false) {
                        //securityRoleDetail.BindModules(response);
                        securityRoleDetail.BindModuleForms(response);
                        securityRoleDetail.BindPrivileges(response);

                        if ($("#securityRoleDetail #chkModules" + ModuleId).is(':checked')) {
                            $("#securityRoleDetail #pnlForms").removeClass("disableAll");
                        }
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                        $("#securityRoleDetail #pnlForms").empty();
                        $("#securityRoleDetail #pnlPrivileges").empty();
                    }
                });

            }
            else {
                if ($("#securityRoleDetail #chkModules" + ModuleId).is(':checked'))
                    $('#securityRoleDetail #chkModules' + ModuleId).prop('checked', false);
                else
                    $('#securityRoleDetail #chkModules' + ModuleId).prop('checked', true);

                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    FillFormPrivilegesByModule: function (ModuleId) {
        securityRoleDetail.moduleRowSelect = ModuleId;
        securityRoleDetail.formRowSelect = 0;
        securityRoleDetail.IsFirstLoad = false;
        securityRoleDetail.FillSecurityRole(securityRoleDetail.params.SecurityRoleId, ModuleId, 0).done(function (response) {
            if (response.status != false) {
                securityRoleDetail.BindModuleForms(response);
                securityRoleDetail.BindPrivileges(response);

                if ($("#securityRoleDetail #chkModules" + ModuleId).is(':checked')) {
                    $("#securityRoleDetail #pnlForms").removeClass("disableAll");
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
                $("#securityRoleDetail #pnlForms").empty();
                $("#securityRoleDetail #pnlPrivileges").empty();
            }
        });
    },

    ModuleFormSave: function (FormId, ModuleFormId, ModuleId, ModuleFormRoleId) {
        securityRoleDetail.formRowSelect = FormId;
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Security Roles", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#securityRoleDetail #gvModule_row" + ModuleId).hasClass('active')) {
                    if ($("#securityRoleDetail #chkModules" + ModuleId).is(':checked')) {
                        if ($("#securityRoleDetail #chkForm" + FormId).is(':checked')) {
                            securityRoleDetail.SaveModuleForm(securityRoleDetail.params.SecurityRoleId, ModuleFormId).done(function (response) {
                                if (response.status != false) {
                                    securityRoleDetail.moduleFormRoleID = response.ModuleFormRoleId;
                                    securityRoleDetail.FillSecurityRole(securityRoleDetail.params.SecurityRoleId, ModuleId, ModuleFormId).done(function (response) {
                                        if (response.status != false) {
                                            securityRoleDetail.BindModuleForms(response);
                                            securityRoleDetail.BindPrivileges(response);
                                            if ($("#securityRoleDetail #chkForm" + FormId).is(':checked')) {
                                                $("#securityRoleDetail #pnlForms").removeClass("disableAll");
                                                $("#securityRoleDetail #pnlPrivileges").removeClass("disableAll");
                                                if ($('#' + securityRoleDetail.params.PanelID + ' #dgvPrivileges input[type=checkbox]:checked').length == $('#' + securityRoleDetail.params.PanelID + ' #dgvPrivileges input[type=checkbox]').length) {
                                                    $('#' + securityRoleDetail.params.PanelID + ' #pnlPrivileges #chkPrivliageSelectAll').attr('checked', 'checked')
                                                }
                                            }
                                        }
                                        else {
                                            utility.DisplayMessages(response.Message, 3);
                                        }
                                    });
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                        else {
                            var MFRoleId = $("#securityRoleDetail #chkForm" + ModuleFormId).attr("ModuleFormRoleId");
                            securityRoleDetail.DeleteModuleFormRole(ModuleFormRoleId).done(function (response) {
                                if (response.status != false) {
                                    securityRoleDetail.FillSecurityRole(securityRoleDetail.params.SecurityRoleId, ModuleId, ModuleFormId).done(function (response) {
                                        if (response.status != false) {
                                            securityRoleDetail.BindModuleForms(response);
                                            securityRoleDetail.moduleFormRoleID = 0;
                                            securityRoleDetail.BindPrivileges(response);
                                            if ($("#securityRoleDetail #chkForm" + ModuleFormId).is(':checked')) {
                                                $("#securityRoleDetail #pnlPrivileges").removeClass("disableAll");
                                            }
                                        }
                                        else {
                                            utility.DisplayMessages(response.Message, 3);
                                        }
                                    });
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                    }
                }
            }
            else {
                if ($("#securityRoleDetail #chkForm" + FormId).is(':checked'))
                    $('#securityRoleDetail #chkForm' + FormId).prop('checked', false);
                else
                    $('#securityRoleDetail #chkForm' + FormId).prop('checked', true);

                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    FillFormPrivilegesByForm: function (FormId, ModuleFormId, ModuleId, ModuleFormRoleId) {
        securityRoleDetail.formRowSelect = FormId;
        securityRoleDetail.moduleFormRoleID = ModuleFormRoleId;

        securityRoleDetail.FillSecurityRole(securityRoleDetail.params.SecurityRoleId, ModuleId, ModuleFormId).done(function (response) {
            if (response.status != false) {
                securityRoleDetail.BindModuleForms(response);
                securityRoleDetail.BindPrivileges(response);
                if ($("#securityRoleDetail #chkForm" + FormId).is(':checked')) {
                    $("#securityRoleDetail #pnlPrivileges").removeClass("disableAll");
                    if ($('#' + securityRoleDetail.params.PanelID + ' #dgvPrivileges input[type=checkbox]:checked').length == $('#' + securityRoleDetail.params.PanelID + ' #dgvPrivileges input[type=checkbox]').length) {
                        $('#' + securityRoleDetail.params.PanelID + ' #pnlPrivileges #chkPrivliageSelectAll').attr('checked', 'checked')
                    }
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    ModuleFormPrivilegesSave: function (PrivilegeId) {
        $("#securityRoleDetail #divPrivileges" + PrivilegeId).addClass('control-selected');
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Security Roles", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#securityRoleDetail #gvForm_row" + securityRoleDetail.formRowSelect).hasClass('active')) {
                    if ($("#securityRoleDetail #chkForm" + securityRoleDetail.formRowSelect).is(':checked')) {

                        if ($("#securityRoleDetail #chkPrivileges" + PrivilegeId).is(':checked')) {
                            securityRoleDetail.SaveModuleFormRolePrivileges(PrivilegeId, securityRoleDetail.moduleFormRoleID).done(function (response) {
                                if (response.status != false) {
                                    $('#chkPrivileges' + PrivilegeId).attr('ModuleFormRolePrivilegeId', response.ModuleFormRolePrivilegeId);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                        else {
                            var RolePrivilegeId = $('#chkPrivileges' + PrivilegeId).attr("ModuleFormRolePrivilegeId");
                            securityRoleDetail.DeleteModuleFormRolePrivileges(RolePrivilegeId).done(function (response) {
                                if (response.status != false) {
                                    $('#chkPrivileges' + PrivilegeId).attr('ModuleFormRolePrivilegeId', "");
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }



                    }
                }
            }
            else {
                if ($("#securityRoleDetail #chkPrivileges" + PrivilegeId).is(':checked'))
                    $('#securityRoleDetail #chkPrivileges' + PrivilegeId).prop('checked', false);
                else
                    $('#securityRoleDetail #chkPrivileges' + PrivilegeId).prop('checked', true);

                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    ValidateSecurityRole: function () {
        $('#frmRoleDetail')
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
                securityRoleDetail.SecurityRoleSave();
            });
    },

    SecurityRoleSave: function () {
        $('#frmRoleDetail').data('serialize', $('#frmRoleDetail').serialize());
        var strMessage = "";

        var self = $("#securityRoleDetail");
        var myJSON = self.getMyJSON();
        if (securityRoleDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Security Roles", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    if (securityRoleDetail.params.SecurityRoleId == "-1") {
                        securityRoleDetail.SaveSecurityRole(myJSON).done(function (response) {
                            if (response.status != false) {
                                $("#securityRoleDetail #pnlRights").removeClass('disableAll');
                                $("#securityRoleDetail #txtShortName").attr("disabled", "disabled");
                                roleID = response.SecurityRoleId;
                                securityRoleDetail.params.SecurityRoleId = response.SecurityRoleId;
                                utility.DisplayMessages(response.message, 1);

                                //update Grid
                                Admin_SecurityRoles.SecurityRoleSearch('0');

                                CacheManager.BindCodes('GetRoles', true);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                            $('#frmRoleDetail').data('serialize', $('#frmRoleDetail').serialize());
                        });
                    }
                    else if (securityRoleDetail.params.SecurityRoleId != "-1" && securityRoleDetail.params.SecurityRoleId != "" && securityRoleDetail.params.SecurityRoleId != "0") {
                        securityRoleDetail.UpdateSecurityRole(myJSON, securityRoleDetail.params.SecurityRoleId).done(function (response) {
                            if (response.status != false) {

                                //update Grid
                                Admin_SecurityRoles.SecurityRoleSearch('0');
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
        else if (securityRoleDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Security Roles", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    securityRoleDetail.UpdateSecurityRole(myJSON, securityRoleDetail.params.SecurityRoleId).done(function (response) {
                        if (response.status != false) {

                            CacheManager.BindCodes('GetRoles', true);

                            utility.DisplayMessages(response.message, 1);
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

    PriviligeSelectAll: function (ev) {
        var objprivilige = [];
        var ObjAction = "";
        if (!ev.checked) {
            $('#' + securityRoleDetail.params.PanelID + ' #dgvPrivileges input[type=checkbox]').each(function (i, item) {
                if ($(item).is(":checked") == true) {
                    objprivilige.push($(item).attr("ModuleFormRolePrivilegeId"));
                    $(item).prop('checked', false);
                    ObjAction = "DELETE";
                }
            });
        }
        else {
            $('#' + securityRoleDetail.params.PanelID + ' #dgvPrivileges input[type=checkbox]').each(function (i, item) {
                if ($(item).is(":checked") == false) {
                    objprivilige.push($(item).closest('tr').attr('CurrentPrivilegeId'));
                    $(item).prop('checked', true);
                    ObjAction = "SAVE";
                }
            });
        }

        if (objprivilige.length > 0)
            securityRoleDetail.ModuleFormRoleAllPrivilegesSave(ObjAction, JSON.stringify(objprivilige));

    },

    ModuleFormRoleAllPrivilegesSave: function (ObjAction, priviligedata) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Security Roles", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#securityRoleDetail #gvForm_row" + securityRoleDetail.formRowSelect).hasClass('active')) {
                    if ($("#securityRoleDetail #chkForm" + securityRoleDetail.formRowSelect).is(':checked')) {

                        securityRoleDetail.SaveModuleFormRoleAllPrivileges(ObjAction, priviligedata, securityRoleDetail.moduleFormRoleID).done(function (response) {

                            if (response.status != false) {

                                if (ObjAction == "SAVE") {
                                    var ModuleFormRole_Privilege = JSON.parse(response.JsonModuleFormRolePrivilege);
                                    $.each(ModuleFormRole_Privilege, function (i, item) {
                                        $('#chkPrivileges' + item.PrivilegeSelectionid).attr('ModuleFormRolePrivilegeId', item.ModuleFormRolePrivilegesId);
                                    });
                                }
                                else if (ObjAction == "DELETE") {
                                    $.each(JSON.parse(priviligedata), function (i, item) {
                                        $("#dgvPrivileges tr td").each(function (i, it) {
                                            $(this).find("input[type=checkbox][moduleformroleprivilegeid=" + item + "]").attr('ModuleFormRolePrivilegeId', "");
                                        });
                                    });
                                }
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }
            }
        });
    },

    SaveModuleFormRoleAllPrivileges: function (ObjAction, PriviligeData, ModuleFormRoleID) {
        var data = "Data=" + PriviligeData + "&ModuleFormRoleID=" + ModuleFormRoleID + "&Action=" + ObjAction;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SECURITY_ROLE_DETAIL", "SAVE_SECURITY_ROLE_MODULE_FORM_ALL_PRIVILEGE");
    },

    SaveSecurityRole: function (SecurityRoleData) {
        var data = "SecurityRoleData=" + SecurityRoleData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SECURITY_ROLE_DETAIL", "SAVE_SECURITY_ROLE");
    },

    SaveModuleForm: function (roleID, ModuleFormId) {
        var data = "RoleID=" + roleID + "&ModuleFormID=" + ModuleFormId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SECURITY_ROLE_DETAIL", "SAVE_SECURITY_ROLE_MODULE_FORM");
    },

    DeleteModuleFormRole: function (ModuleFormRoleId) {
        var data = "ModuleFormRoleID=" + ModuleFormRoleId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SECURITY_ROLE_DETAIL", "DELETE_SECURITY_ROLE_MODULE_FORM");
    },

    SaveModuleFormRolePrivileges: function (ModuleFormPrivilegeId, ModuleFormRoleID) {
        var data = "ModuleFormPrivilegeID=" + ModuleFormPrivilegeId + "&ModuleFormRoleID=" + ModuleFormRoleID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SECURITY_ROLE_DETAIL", "SAVE_SECURITY_ROLE_MODULE_FORM_PRIVILEGE");
    },

    DeleteModuleFormRolePrivileges: function (ModuleFormRolePrivilegeId) {
        var data = "ModuleFormRolePrivilegeID=" + ModuleFormRolePrivilegeId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SECURITY_ROLE_DETAIL", "DELETE_SECURITY_ROLE_MODULE_FORM_PRIVILEGE");
    },

    UpdateSecurityRole: function (SecurityRoleData, SecurityRoleID) {
        var data = "SecurityRoleData=" + SecurityRoleData + "&SecurityRoleID=" + SecurityRoleID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SECURITY_ROLE_DETAIL", "UPDATE_SECURITY_ROLE");
    },

    DeleteModule: function (RoleID, ModuleId) {
        var data = "RoleID=" + RoleID + "&ModuleId=" + ModuleId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SECURITY_ROLE_DETAIL", "DELETE_ROLE_MODULE");
    },

    FillSecurityRole: function (SecurityRoleID, ModuleID, ModuleFormID) {
        var data = "SecurityRoleID=" + SecurityRoleID + "&ModuleID=" + ModuleID + "&ModuleFormID=" + ModuleFormID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SECURITY_ROLE_DETAIL", "FILL_SECURITY_ROLE");
    },

    SecurityRoleActiveInactive: function (SecurityRoleID, IsActive) {
        var data = "SecurityRoleID=" + SecurityRoleID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SECURITY_ROLE_DETAIL", "SECURITY_ROLE_ACTIVE_INACTIVE");
    },

    UnLoad: function () {

        utility.UnLoadDialog("frmRoleDetail", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });

        //Start || 25 August, 2016 || Talha Tanweer || EMR-878
        {
            AppPrivileges.GetFormPrivileges("Security Roles", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_SecurityRoles.SecurityRoleSearch();
                }
            });
        }
        //End   || 25 August, 2016 || Talha Tanweer || EMR-878


    },



}
