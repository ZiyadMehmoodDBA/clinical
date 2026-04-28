userDetail = {
    params: [],
    userID: -1,
    moduleID: -1,
    moduleFormID: -1,
    moduleFormUserId: -1,
    moduleFormUserId: -1,
    formRowSelect: 0,
    moduleRowSelect: 0,
    priviligesRowSelect: 0,
    moduleFormUserPrivilegeID: -1,
    UsersEntityGroupId: -1,
    EntityId: -1,
    IsFirstLoad: false,
    FormUsedId: 0,
    FormIdclick: 0, 
    Load: function (params) {
        userDetail.userID = -1;
        userDetail.moduleID = -1;
        userDetail.moduleFormID = -1;
        userDetail.moduleFormUserId = -1;
        userDetail.moduleFormUserId = -1;
        userDetail.formRowSelect = 0;
        userDetail.moduleRowSelect = 0;
        userDetail.priviligesRowSelect = 0;
        userDetail.moduleFormUserPrivilegeID = -1;
        userDetail.UsersEntityGroupId = -1;
        userDetail.EntityId = 0;
        userDetail.IsFirstLoad = false;
        userDetail.params = params;

        if (userDetail.params.mode == "Edit") {
            $('#userDetail #headingTitle').text('Edit User');
        } else {
            $('#userDetail #headingTitle').text('Add User');
        }
        var self = null;
        if (userDetail.params.PanelID != "userDetail")
            self = $('#' + userDetail.params.PanelID + ' #userDetail');
        else
            self = $('#' + userDetail.params.PanelID);

        if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
            self.find("#lstEntityId").attr('disabled', 'disabled');
        }
        if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
            self.find("#pnlIsShowColBal").hide();
        }

        self.loadDropDowns(true).done(function () {
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#lstEntityId").val(globalAppdata["SeletedEntityId"]);
            }
            userDetail.IntializeMultiSelectDropDownFolder();
            userDetail.LoadUser();
            userDetail.DomeReadyFunc();
            $('#frmUserDetail').data('serialize', $('#frmUserDetail').serialize());




        });

        if (globalAppdata["isDataSegmentationPrivacy"] && globalAppdata["isDataSegmentationPrivacy"].toLowerCase() == "false")
            $('#' + userDetail.params.PanelID +" #pnlDataPrivacy").addClass("hidden");
    },


    IntializeMultiSelectDropDownFolder: function () {
        $('#' + userDetail.params.PanelID + ' #ddlFolder').multiselect('destroy');
        $('#' + userDetail.params.PanelID + ' #ddlFolder').multiselect({
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            includeSelectAllOption: true,
            maxHeight: 247,
            onChange: function (option, checked) {
                var options = $(option).parent().find('option');
                var Selectedoptions = $(option).parent().find('option:selected');
                if (option.length > 0) {
                    var optionText = $(option)[0].outerText;
                    var optionVal = $($(option)[0]).val();
                    if (checked) {
                        $('#' + userDetail.params.PanelID + " #ddlFolder").multiselect('refresh');
                    }
                    else {
                        options.each(function () {
                            var input = $('#pnlDocumentImport #ddlFolder input[type=checkbox][value="' + $(this).val() + '"]');
                            input.prop('disabled', false);
                        });
                    }
                }
            },
        });
        $('#' + userDetail.params.PanelID + ' #ddlFolder').val("");
       // userDetail.validateFolder(3);
    },
    validateFolder: function (operationid) {
        $('#' + userDetail.params.PanelID + ' #foldersddl label').find("i").remove();
        if (operationid == 1) {
            $('#' + userDetail.params.PanelID + ' #foldersddl .multiselect').css("border-color", "#cc2724");
            $('#' + userDetail.params.PanelID + ' #foldersddl ').find(".control-label").css("color", "#cc2724");
            $('#' + userDetail.params.PanelID + ' #foldersddl').find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-remove" data-bv-icon-for="Race" style="display: block;"></i>');
        } else if (operationid == 2) {
            $('#' + userDetail.params.PanelID + ' #foldersddl .multiselect').css("border-color", "#3c763d");
            $('#' + userDetail.params.PanelID + ' #foldersddl').find(".control-label").css("color", "#3c763d");
            $('#' + userDetail.params.PanelID + ' #foldersddl').find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-ok" data-bv-icon-for="Race" style="display: block;"></i>');
        } else {
            $('#' + userDetail.params.PanelID + ' #foldersddl .multiselect').css("border-color", "#ccc");
            $('#' + userDetail.params.PanelID + ' #foldersddl').find(".control-label").css("color", "#000000");
        }
    },
    //Start//6/04/2016//M Ahmad Imran//Implimented ready function which run at load time 
    DomeReadyFunc: function () {
        //$('#userDetail' + ' #frmUserDetail').on('keydown', '#txtAutoLogOff', function (e) { -1 !== $.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) || /65|67|86|88/.test(e.keyCode) && (!0 === e.ctrlKey || !0 === e.metaKey) || 35 <= e.keyCode && 40 >= e.keyCode || (e.shiftKey || 48 > e.keyCode || 57 < e.keyCode) && (96 > e.keyCode || 105 < e.keyCode) && e.preventDefault() });
        $('#userDetail #frmUserDetail #txtAutoLogOff').on("keypress", function (event) {
            if (event.which < 48 || event.which > 57) {
                event.preventDefault();
            }
        });

        $('#userDetail #frmUserDetail #MobSessionExpTime').on("keypress", function (event) {
            if (event.which < 48 || event.which > 57) {
                event.preventDefault();
            }
        });

        $("#" + userDetail.params.PanelID + " #frmUserDetail #chkIsExpiryAlert").on('click', function (e) {
            if (e.target.checked) {
                $("#" + userDetail.params.PanelID + " #frmUserDetail #pnlTxtDaysBeforeExpiry").removeClass("hidden");
                $("#" + userDetail.params.PanelID + " #frmUserDetail #txtDaysBeforeExpiry").val("30").focus();
            }
            else {
                $("#" + userDetail.params.PanelID + " #frmUserDetail #pnlTxtDaysBeforeExpiry").addClass("hidden");
            }
        });


    },
    LoadUser: function () {
        userDetail.FillUser(userDetail.params.UserId, 0, 0).done(function (response) {

            if (response.status != false) {
                userDetail.BindModules(response);
                userDetail.BindModuleForms(response);
                userDetail.BindPrivileges(response);
                userDetail.BindSecurityGroup(response);
                $("#userDetail #pnlAditionalPriviliges").removeClass('disableAll');
                $("#userDetail #pnlPriviligesGroupMain").removeClass('disableAll');
                if (userDetail.params.IsUAdmin != "True") {
                    $("#userDetail #lstUserRoles option[refvalue='True']").hide();
                }
                if (userDetail.params.mode == "Add") {

                    if (globalAppdata['AppUserName'] != DefaultUser) {
                        $('#userDetail #pnlIsAdmin').css("display", "none");
                        $('#userDetail #pnlIsEMR').css("display", "none");
                        $('#userDetail #pnlIsLocked').css("display", "none");
                        $('#userDetail #pnlIsUnSignNote').css("display", "none");

                    }
                    $('#userDetail #pnlAditionalPriviliges').addClass("disableAll");
                    $('#userDetail #pnlPriviligesGroupMain').addClass("disableAll");
                    $('input#txtUserPassword').removeAttr("type");
                    $('input#txtUserPassword').prop('type', 'password');

                    setTimeout(function () {
                        userDetail.ValidateUser();
                        $("#userDetail #frmUserDetail #txtAutoLogOff").val(30);
                        $("#userDetail #frmUserDetail #MobSessionExpTime").val(30);
                        //serialize Data.
                        $('#frmUserDetail').data('serialize', $('#frmUserDetail').serialize());
                        //var objdirect = $('#userDetail #frmUserDetail');
                        //var formValidation = objdirect.data("bootstrapValidator");
                        //formValidation.enableFieldValidators('Password', true);
                    }, "200");
                    $("#" + userDetail.params.PanelID + " #frmUserDetail #btnAudit").attr("disabled", "disabled");

                    $('#' + userDetail.params.PanelID + ' #ddlFolder option').prop('selected', true);
                    $('#' + userDetail.params.PanelID + ' #ddlFolder').multiselect("refresh");
                }
                else if (userDetail.params.mode == "Edit") {
                    userDetail.ValidateUser();
                    //var objdirect = $('#userDetail #frmUserDetail');
                    //var formValidation = objdirect.data("bootstrapValidator");
                    //formValidation.enableFieldValidators('Password', false);
                    userDetail.BindUser(response).done(function () {


                        $("#userDetail #txtUserName").attr("disabled", "disabled");
                        $("#userDetail #txtUserPassword").attr("disabled", "disabled");
                        $("#userDetail #btnChangePassword").show();
                        $("#userDetail").find("form :input:not(button):not(hidden):enabled:visible:first").focus();
                        if (globalAppdata['AppUserName'] != DefaultUser) {
                            $('#pnlIsAdmin').css("display", "none");
                            $('#pnlIsEMR').css("display", "none");
                            $('#pnlIsLocked').css("display", "none");
                            $('#pnlIsMobileLogin').css("display", "none");
                            $('#pnlIsMedText').css("display", "none");
                            $('#pnlIsUnSignNote').css("display", "none");

                            if (userDetail.params.IsUAdmin == "True") {
                                $('#userDetail #pnlPriviligesGroupMain').addClass("disableAll");
                                $('#userDetail #pnlIsActive').addClass("disableAll");
                                $('#userDetail #pnlIsAdmin').addClass("disableAll");
                                $('#userDetail #pnlIsEMR').addClass("disableAll");
                                $('#userDetail #pnlIsLocked').addClass("disableAll");
                                $('#userDetail #pnlRole').addClass("disableAll");
                            }
                            else {
                                $("#userDetail #lstUserRoles option[refvalue='True']").hide();
                            }
                        }
                        else {
                            $('#pnlMobSessionExpTime').css("display", "none");
                        }

                        $('#frmUserDetail').bootstrapValidator('destroy');
                        userDetail.ValidateUser();
                        //serialize Data.
                        $('#frmUserDetail').data('serialize', $('#frmUserDetail').serialize());
                        setTimeout(function () {
                            var User_Detail = JSON.parse(response.Users_JSON);
                            if (User_Detail.RCopialUser == "True") {
                                $("#userDetail #RCopialUser").attr("checked", true);
                                $('#userDetail #RcopiaUserInfoDiv').show();
                                var objdirect = $('#userDetail #frmUserDetail');
                                var formValidation = objdirect.data("bootstrapValidator");
                                formValidation.enableFieldValidators('RcUserName', true);
                                formValidation.enableFieldValidators('RcPassword', true);
                                formValidation.enableFieldValidators('RcSigPassword', true);
                            }
                            else {
                                $("#userDetail #RCopialUser").attr("checked", false);
                                $('#userDetail #RcopiaUserInfoDiv').hide();
                                var objdirect = $('#userDetail #frmUserDetail');
                                var formValidation = objdirect.data("bootstrapValidator");
                                formValidation.enableFieldValidators('RcUserName', false);
                                formValidation.enableFieldValidators('RcPassword', false);
                                formValidation.enableFieldValidators('RcSigPassword', false);
                            }

           
                            var documentsId = User_Detail.UserSelectedDocuments.split(',');
                            $('#' + userDetail.params.PanelID + ' #ddlFolder').val(documentsId);
                            $('#' + userDetail.params.PanelID + ' #ddlFolder').multiselect("refresh");
                            $('#frmUserDetail').data('serialize', $('#frmUserDetail').serialize());
                        }, "200");
                        //var objdirect = $('#userDetail #frmUserDetail');
                        //var formValidation = objdirect.data("bootstrapValidator");
                        //formValidation.enableFieldValidators('Password', false);

                    });

                    $("#" + userDetail.params.PanelID + " #frmUserDetail #btnAudit").removeAttr("disabled", "disabled");
                }
                $('#frmUserDetail').data('serialize', $('#frmUserDetail').serialize());
            }
            else {
                UnloadActionPan();
                utility.DisplayMessages(response.Message, 3);
            }
        });
        

    },

    BindUser: function (response) {
        var User_Detail = JSON.parse(response.Users_JSON);
        var self = $("#userDetail");
        //self.bindMyJSON(true, User_Detail, f);
        return utility.bindMyJSON(true, User_Detail, false, self).done(function () {

            $('input#txtUserPassword').removeAttr("type");
            $('input#txtUserPassword').prop('type', 'password');
            if (User_Detail.chkIsActive == "True") {
                $("#userDetail #chkIsActive").attr("checked", true);
            }
            else {
                $("#userDetail #chkIsActive").attr("checked", false);
            }
            if (User_Detail.chkIsAdmin == "True") {
                $("#userDetail #chkIsAdmin").attr("checked", true);
            }
            else {
                $("#userDetail #chkIsAdmin").attr("checked", false);
            }
            if (User_Detail.chkIsNoteUnSign == "True") {
                $("#userDetail #chkIsNoteUnSign").attr("checked", true);
            }
            else {
                $("#userDetail #chkIsNoteUnSign").attr("checked", false);
            }
            if (User_Detail.chkIsEMR == "True") {
                $("#userDetail #chkIsEMR").attr("checked", true);
            }
            else {
                $("#userDetail #chkIsEMR").attr("checked", false);
            }
            if (User_Detail.chkIsFullSSN == "True") {
                $("#userDetail #chkIsFullSSN").attr("checked", true);
            }
            else {
                $("#userDetail #chkIsFullSSN").attr("checked", false);
            }

            if (User_Detail.chkIsShowColBal == "True") {
                $("#userDetail #chkIsShowColBal").attr("checked", true);
            }
            else {
                $("#userDetail #chkIsShowColBal").attr("checked", false);
            }
            //  MU3 - 15- Faizan Ameen

            if (User_Detail.chkIsDataPrivacy == "True") {
                $("#userDetail #chkIsDataPrivacy").attr("checked", true);
            }
            else {
                $("#userDetail #chkIsDataPrivacy").attr("checked", false);
            }
            if (User_Detail.chkIsExpiryAlert == "True") {
                $("#" + userDetail.params.PanelID + " #frmUserDetail #chkIsExpiryAlert").attr("checked", true);
                $("#" + userDetail.params.PanelID + " #frmUserDetail #pnlTxtDaysBeforeExpiry").removeClass("hidden");
            }
            else {
                $("#" + userDetail.params.PanelID + " #frmUserDetail #chkIsExpiryAlert").attr("checked", false);
                $("#" + userDetail.params.PanelID + " #frmUserDetail #pnlTxtDaysBeforeExpiry").addClass("hidden");
            }



        });



    },

    BindSecurityGroup: function (response) {
        var SecurityGroup_Detail = JSON.parse(utility.decodeHtml(response.SecurityGroup_JSON));
        $("#pnlPriviligesGroup").empty();
        var div = $('<table class="border table table-condensed"></table>');
        $.each(SecurityGroup_Detail, function (i, item) {
            var $row = $('<tr/>');
            var $cell1 = $('<td width="20"/>');
            var $cell2 = $('<td/>');

            if (userDetail.priviligesRowSelect == 0)
                userDetail.priviligesRowSelect = item.SecGroupId;
            var chkSelect;

            if (item.UserEntityGroupId != "") {
                chkSelect = true;
            }
            else {
                chkSelect = false;
            }

            //if (globalAppdata['AppUserName'] != DefaultUser) {
            //    if (userDetail.params.IsUAdmin == "True" && chkSelect == true) {
            //        $cell1.append($('<input>').attr({
            //            type: 'checkbox', id: 'chkSecGroupId' + item.SecGroupId, SecGroupId: item.SecGroupId, UsersEntityGroupId: item.UserEntityGroupId, checked: chkSelect, disabled: '', onclick: 'userDetail.UserEntityGroup(chkSecGroupId' + item.SecGroupId + ', ' + item.SecGroupId + ', "CHECKED")',
            //        }));
            //    }
            //    else {
            //        $cell1.append($('<input>').attr({
            //            type: 'checkbox', id: 'chkSecGroupId' + item.SecGroupId, SecGroupId: item.SecGroupId, UsersEntityGroupId: item.UserEntityGroupId, checked: chkSelect, onclick: 'userDetail.UserEntityGroup(chkSecGroupId' + item.SecGroupId + ', ' + item.SecGroupId + ', "CHECKED")',
            //        }));
            //    }
            //}
            //else {
            $cell1.append($('<input>').attr({
                type: 'checkbox', id: 'chkSecGroupId' + item.SecGroupId, SecGroupId: item.SecGroupId, UsersEntityGroupId: item.UserEntityGroupId, checked: chkSelect, onclick: 'userDetail.UserEntityGroup(chkSecGroupId' + item.SecGroupId + ', ' + item.SecGroupId + ', "CHECKED")',
            }));
            // }

            $cell2.append($('<label  style="padding:2px 0 0 0;">').text(item.ShortName));
            $row.append($cell1);
            $row.append($cell2);

            $row.attr("onclick", "utility.SelectGridRow($('#gvModule_row" + item.SecGroupId + "')," + item.SecGroupId + ");");
            $row.attr("id", "gvModule_row" + item.SecGroupId);
            div.append($row);
        });
        $("#pnlPriviligesGroup").last().append(div);
        $("#gvModule_row" + userDetail.priviligesRowSelect).addClass('active');
    },

    BindModules: function (response) {
        var Module_Detail = JSON.parse(response.Modules_JSON);
        $("#userDetail #pnlModules").empty();
        var div = $('<table class="border table table-condensed" id="dgvModules"></table>');
        $.each(Module_Detail, function (i, item) {
            var $row = $('<tr/>');
            var $cell1 = $('<td width="22"/>');
            var $cell2 = $('<td />');
            if (userDetail.moduleRowSelect == 0)
                userDetail.moduleRowSelect = item.ModuleId;
            var chkSelect;
            if (item.IsSelected == 'True')
                chkSelect = true;
            else
                chkSelect = false;
            $cell1.append($('<input>').attr({
                type: 'checkbox', id: 'chkModules' + item.ModuleId, ModuleId: item.ModuleId, checked: chkSelect, //onclick: 'userDetail.ModuleDelete(chkModules' + item.ModuleId + ', ' + item.ModuleId + ', "CHECKED")',
            }));
            $cell2.append($('<label style="padding:2px 0 0 0;">').text(item.Name));
            $row.append($cell1);
            $row.append($cell2);

            $row.attr("onclick", "utility.SelectGridRow($('#userDetail #gvModule_row" + item.ModuleId + "'));");//userDetail.ModuleDelete(chkModules" + item.ModuleId + ", " + item.ModuleId + ", 'SELECT')
            $row.attr("id", "gvModule_row" + item.ModuleId);
            $row.attr("CurrentModuleId", item.ModuleId);
            div.append($row);
        });
        $("#userDetail #pnlModules").last().append(div);
        $("#userDetail #gvModule_row" + userDetail.moduleRowSelect).addClass('active');
        $("#dgvModules").on('click', 'tr', function (e) {
            if ($(e.target).is('input[type=checkbox]')) {
                //alert('1');
                userDetail.ModuleDelete($(this).closest('tr').attr('CurrentModuleId'));
                return;
            }
            else if ($(this).hasClass('active')) {
                //alert('2');
                userDetail.FillFormPrivilegesByModule($(this).closest('tr').attr('CurrentModuleId'));
            }
        });


    },

    BindModuleForms: function (response) {

        var moduleid = 0;

        var ModuleForm_Detail = JSON.parse(response.Forms_JSON);
        $("#pnlForms").empty();
        var div = $('<table class="border table table-condensed" id="dgvForms"></table>');

        var prevFormGroupName = "";
        var secondChild = "";
        $.each(ModuleForm_Detail, function (i, item) {

            moduleid = item.ModuleId;

            var $row = $('<tr/>');
            var $cell1 = $('<td width="22"/>');
            var $cell2 = $('<td/>');

            if (userDetail.formRowSelect == 0 && userDetail.IsFirstLoad == false) {
                userDetail.IsFirstLoad = true;
                userDetail.formRowSelect = item.FormsId;
                userDetail.moduleFormUserID = item.MFUId;
            }
            var chkSelect;
            if (item.MFUId != "")
                chkSelect = true;
            else
                chkSelect = false;

            ////////////

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
                    type: 'checkbox', id: 'chkForm' + item.FormsId, FormId: item.FormsId, checked: chkSelect, ModuleFormUserId: item.MFUId, style: 'margin-left: 15px;', //onclick: 'userDetail.ModuleFormSave(' + item.FormsId + ', ' + item.ModuleFormId + ',' + item.ModuleId + ', "' + item.MFUId + '", "CHECKED")',
                }));
            }
            else {
                currentFormName = item.FormName;

                $cell1.append($('<input>').attr({
                    type: 'checkbox', id: 'chkForm' + item.FormsId, FormId: item.FormsId, checked: chkSelect, ModuleFormUserId: item.MFUId, onclick: 'userDetail.SelectAllModuleFormPrivilige(' + item.FormsId + ', ' + item.ModuleFormId + ',' + item.ModuleId + ', "' + item.MFUId + '")',
                }));

            }

            /////////////////




            //    $cell2.append($('<label style="padding:2px 0 0 0;">').text(item.FormName));
            $cell2.append($('<label style="padding:2px 0 0 0;">').text(currentFormName));

            $row.append($cell1);
            $row.append($cell2);

            $row.attr("onclick", "utility.SelectGridRow($('#gvForm_row" + item.FormsId + "'));");//userDetail.ModuleFormSave(" + item.FormsId + ", " + item.ModuleFormId + "," + item.ModuleId + ", '" + item.MFUId + "', 'SELECT')
            $row.attr("id", "gvForm_row" + item.FormsId);
            $row.attr("CurrentFormId", item.FormsId);
            $row.attr("ModuleFormId", item.ModuleFormId);
            $row.attr("CurrentModuleId", item.ModuleId);
            $row.attr("ModuleFormUserId", item.MFUId);

            //Start 03-04-2017 Humaira Yousaf to show Sign right above Co-Sign
            if (item.FormName == "Notes_Sign") {
                $row.insertAfter($($(div).find("label:contains('Notes')")[0]).parents('tr'));
            }
            else {
                div.append($row);
            }
            //End 03-04-2017 Humaira Yousaf to show Sign right above Co-Sign
        });
        $("#pnlForms").last().append(div);
        $("#gvForm_row" + userDetail.formRowSelect).addClass('active');

        if ($("#userDetail #chkModules" + moduleid).is(':checked')) {
            $("#userDetail #pnlForms").removeClass("disableAll");
            $("#userDetail #pnlForms").find("tr:contains('Activity Log') [type=checkbox]").prop('disabled', true);
        }
        else {
            $("#userDetail #pnlForms").addClass("disableAll");
        }

        $("#dgvForms").on('click', 'tr', function (e) {
            if ($(e.target).is('input[type=checkbox]')) {
                //alert('1');
                userDetail.ModuleFormSave($(this).closest('tr').attr('CurrentFormId'), $(this).closest('tr').attr('ModuleFormId'), $(this).closest('tr').attr('CurrentModuleId'), $(this).closest('tr').attr('ModuleFormUserId'));
                return;
            }
            else if ($(this).hasClass('active')) {
                //alert('2');
                userDetail.FillFormPrivilegesByForm($(this).closest('tr').attr('CurrentFormId'), $(this).closest('tr').attr('ModuleFormId'), $(this).closest('tr').attr('CurrentModuleId'), $(this).closest('tr').attr('ModuleFormUserId'));
            }
        });


    },

    BindPrivileges: function (response) {
        var Privileges_Detail = JSON.parse(response.Privileges_JSON);
        $("#pnlPrivileges").empty();
        $("#pnlPrivileges").append('<div class="pl-xs"><input type="checkbox" id="chkPrivliageSelectAll" onclick ="userDetail.PriviligeSelectAll(this);" /> <label>Select All</label></div>');
        var div = $('<table class="border table table-condensed" id="dgvPrivileges"></table>');
        $.each(Privileges_Detail, function (i, item) {
            if (item.PrivilegeSelectionid || $("#userDetail #dgvForms tr.active").attr("currentformid") == "198") {
                var $row = $('<tr/>');
                var $cell1 = $('<td width="22"/>');
                var $cell2 = $('<td/>');
                var chkSelect;
                if (item.ModuleFormUserPriviligesId != "")
                    chkSelect = true;
                else
                    chkSelect = false;
                $cell1.append($('<input>').attr({
                    type: 'checkbox', id: 'chkPrivileges' + item.PrivilegeSelectionid, ModuleFormUserPrivilegeId: item.ModuleFormUserPriviligesId, PrivilegeId: item.PrivilegeSelectionid, checked: chkSelect, //onclick: 'userDetail.ModuleFormPrivilegesSave(chkPrivileges' + item.PrivilegeSelectionid + ', ' + item.PrivilegeSelectionid + ')',
                }));
                $cell2.append($('<label style="padding:2px 0 0 0;">').text(item.PrivilegeName));
                $row.append($cell1);
                $row.append($cell2);

                $row.attr("onclick", "utility.SelectGridRow($('#gvPrivilege_row" + item.PrivilegeSelectionid + "'))");
                $row.attr("id", "gvPrivilege_row" + item.PrivilegeSelectionid);
                $row.attr("CurrentPrivilegeId", item.PrivilegeSelectionid);
                div.append($row);
            }
        });
        $("#pnlPrivileges").last().append(div);
        $("#userDetail #pnlPrivileges").addClass("disableAll");

        if ($("#userDetail #chkForm" + userDetail.formRowSelect).is(':checked')) {
            if (!$('#userDetail #pnlForms').find("tr:contains('Activity Log')").hasClass('active')) {
                $("#userDetail #pnlPrivileges").removeClass("disableAll");
            }
            else {
                $("#userDetail #pnlPrivileges").addClass("disableAll");
            }
            if ($('#' + userDetail.params.PanelID + ' #dgvPrivileges input[type=checkbox]:checked').length == $('#' + userDetail.params.PanelID + ' #dgvPrivileges input[type=checkbox]').length) {
                $('#' + userDetail.params.PanelID + ' #pnlPrivileges #chkPrivliageSelectAll').attr('checked', 'checked');
            }
        }

        $("#dgvPrivileges").on('click', 'tr', function (e) {
            if ($(e.target).is('input[type=checkbox]')) {
                //alert('1');

                if ($(e.target).is(':checked') == false) {
                    if ($('#' + userDetail.params.PanelID + ' #pnlPrivileges #chkPrivliageSelectAll').is(':checked')) {
                        $('#' + userDetail.params.PanelID + ' #pnlPrivileges #chkPrivliageSelectAll').attr('checked', false);
                    }
                }


                userDetail.ModuleFormPrivilegesSave($(this).closest('tr').attr('CurrentPrivilegeId'), $(this).closest('tr').text(), $("#userDetail #pnlForms").find('tr.active').text(), $("#userDetail #txtUserName").attr("value"));
                return;
            }
        });
    },

    UserEntityGroup: function (CheckboxId, SecGroupId) {
        userDetail.UsersEntityGroupId = 0;
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Users", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (CheckboxId.checked) {
                    $("#userDetail #chkIsActive").attr.text
                    //var EntityId = $("#userDetail #lstEntityId").attr.val;
                    var EntityId = $("#userDetail #lstEntityId").val();
                    userDetail.SaveSecurityGroup(SecGroupId, userDetail.params.UserId, EntityId).done(function (response) {
                        if (response.status != false) {
                            userDetail.UsersEntityGroupId = response.UsersEntityGroupId;
                            $(CheckboxId).attr('UsersEntityGroupId', response.UsersEntityGroupId);
                        }
                    });
                }
                else {
                    userDetail.UsersEntityGroupId = $(CheckboxId).attr("UsersEntityGroupId");
                    userDetail.DeleteSecurityGroup(userDetail.UsersEntityGroupId).done(function (response) {
                        if (response.status != false) {

                        }
                    });
                }
            }
            else {
                CheckboxId.checked = false;
                utility.DisplayMessages(strMessage, 2);
                return;
            }
        });
    },

    ModuleDelete: function (ModuleId) {
        userDetail.formRowSelect = 0;
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Users", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                //if (Action == 'CHECKED') {
                if (!$("#userDetail #chkModules" + ModuleId).is(':checked')) {
                    userDetail.DeleteModuleForm(userDetail.params.UserId, ModuleId).done(function (response) {
                        if (response.status == false) {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                //}
                //else {
                //    userDetail.moduleRowSelect = ModuleId;
                //}
                userDetail.FillUser(userDetail.params.UserId, ModuleId, 0).done(function (response) {
                    if (response.status != false) {
                        userDetail.BindModuleForms(response);
                        userDetail.BindPrivileges(response);
                        if ($("#userDetail #chkModules" + ModuleId).is(':checked')) {
                            $("#userDetail #pnlForms").removeClass("disableAll");
                        }
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                        $("#pnlForms").empty();
                        $("#pnlPrivileges").empty();
                    }
                });
            }
            else {
                if ($("#userDetail #chkModules" + ModuleId).is(':checked'))
                    $('#userDetail #chkModules' + ModuleId).prop('checked', false);
                else
                    $('#userDetail #chkModules' + ModuleId).prop('checked', true);

                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    FillFormPrivilegesByModule: function (ModuleId) {
        userDetail.moduleRowSelect = ModuleId;
        userDetail.formRowSelect = 0;
        userDetail.priviligesRowSelect = 0;
        userDetail.IsFirstLoad = false;
        userDetail.FillUser(userDetail.params.UserId, ModuleId, 0).done(function (response) {
            if (response.status != false) {
                userDetail.BindModuleForms(response);
                userDetail.BindPrivileges(response);
                if ($("#userDetail #chkModules" + ModuleId).is(':checked')) {
                    $("#userDetail #pnlForms").removeClass("disableAll");
                }
                $('#frmUserDetail').data('serialize', $('#frmUserDetail').serialize());
            }
            else {
                utility.DisplayMessages(response.Message, 3);
                $("#pnlForms").empty();
                $("#pnlPrivileges").empty();
            }
        });
    },

    ModuleFormSave: function (FormId, ModuleFormId, ModuleId, ModuleFormUserId) {
        userDetail.formRowSelect = FormId;
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Users", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                //if (Action == 'CHECKED') {
                if ($("#userDetail #gvModule_row" + ModuleId).hasClass('active')) {
                    if ($("#chkModules" + ModuleId).is(':checked')) {
                        if ($("#userDetail #chkForm" + FormId).is(':checked')) {
                            userDetail.SaveModuleForm(userDetail.params.UserId, ModuleFormId).done(function (response) {
                                if (response.status != false) {
                                    userDetail.moduleFormUserID = response.ModuleFormUserId;
                                    userDetail.FillUser(userDetail.params.UserId, ModuleId, ModuleFormId).done(function (response) {
                                        if (response.status != false) {
                                            userDetail.BindModuleForms(response);
                                            userDetail.BindPrivileges(response);
                                            if ($("#userDetail #chkForm" + ModuleFormId).is(':checked')) {
                                                $("#userDetail #pnlPrivileges").removeClass("disableAll");
                                                if ($('#' + userDetail.params.PanelID + ' #dgvPrivileges input[type=checkbox]:checked').length == $('#' + userDetail.params.PanelID + ' #dgvPrivileges input[type=checkbox]').length) {
                                                    $('#' + userDetail.params.PanelID + ' #pnlPrivileges #chkPrivliageSelectAll').attr('checked', 'checked')
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
                            var MFUserId = $("#chkForm" + ModuleFormId).attr("ModuleFormUserId");
                            userDetail.DeleteModuleFormUser(ModuleFormUserId).done(function (response) {
                                if (response.status != false) {
                                    userDetail.FillUser(userDetail.params.UserId, ModuleId, ModuleFormId).done(function (response) {
                                        if (response.status != false) {
                                            userDetail.BindModuleForms(response);
                                            userDetail.moduleFormUserID = 0;
                                            userDetail.BindPrivileges(response);
                                            if ($("#userDetail #chkForm" + ModuleFormId).is(':checked')) {
                                                $("#userDetail #pnlPrivileges").removeClass("disableAll");
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
                //}

                //else {

                //    userDetail.moduleFormUserID = ModuleFormUserId;
                //    userDetail.FillUser(userDetail.params.UserId, ModuleId, ModuleFormId).done(function (response) {
                //        if (response.status != false) {
                //            userDetail.BindModuleForms(response);
                //            userDetail.BindPrivileges(response);
                //            if ($("#userDetail #chkForm" + ModuleFormId).is(':checked')) {
                //                $("#userDetail #pnlPrivileges").removeClass("disableAll");
                //            }

                //        }
                //        else {
                //            utility.DisplayMessages(response.Message, 3);
                //        }
                //    });
                //}
            }
            else {
                if ($("#userDetail #chkForm" + FormId).is(':checked'))
                    $('#userDetail #chkForm' + FormId).prop('checked', false);
                else
                    $('#userDetail #chkForm' + FormId).prop('checked', true);
                utility.DisplayMessages(strMessage, 2);
                return;
            }
        });
    },

    FillFormPrivilegesByForm: function (FormId, ModuleFormId, ModuleId, ModuleFormUserId) {
        userDetail.formRowSelect = FormId;
        userDetail.moduleFormUserID = ModuleFormUserId;

        userDetail.FillUser(userDetail.params.UserId, ModuleId, ModuleFormId).done(function (response) {
            if (response.status != false) {
                userDetail.BindModuleForms(response);
                userDetail.BindPrivileges(response);
                if ($("#userDetail #chkForm" + ModuleFormId).is(':checked')) {
                    $("#userDetail #pnlPrivileges").removeClass("disableAll");
                    if ($('#' + userDetail.params.PanelID + ' #dgvPrivileges input[type=checkbox]:checked').length == $('#' + userDetail.params.PanelID + ' #dgvPrivileges input[type=checkbox]').length) {
                        $('#' + userDetail.params.PanelID + ' #pnlPrivileges #chkPrivliageSelectAll').attr('checked', 'checked')
                    }
                }
                $('#frmUserDetail').data('serialize', $('#frmUserDetail').serialize());
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    ModuleFormPrivilegesSave: function (PrivilegeId, PrivilegeName, FormName, AssignedTo) {
        $("#divPrivileges" + PrivilegeId).addClass('control-selected');

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Users", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#gvForm_row" + userDetail.formRowSelect).hasClass('active')) {
                    if ($("#chkForm" + userDetail.formRowSelect).is(':checked')) {
                        if ($("#userDetail #chkPrivileges" + PrivilegeId).is(':checked')) {
                            userDetail.SaveModuleFormPrivileges(PrivilegeId, userDetail.moduleFormUserID, PrivilegeName, FormName, AssignedTo).done(function (response) {
                                if (response.status != false) {
                                    $("#userDetail #chkPrivileges" + PrivilegeId).attr('ModuleFormUserPrivilegeId', response.ModuleFormUserPrivilegeId);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                        else {
                            var UserPrivilegeId = $("#userDetail #chkPrivileges" + PrivilegeId).attr("ModuleFormUserPrivilegeId");
                            userDetail.DeleteModuleFormUserPrivileges(UserPrivilegeId, PrivilegeName, FormName, AssignedTo).done(function (response) {
                                if (response.status != false) {
                                    $("#userDetail #chkPrivileges" + PrivilegeId).attr('ModuleFormUserPrivilegeId', "");
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
                if ($("#userDetail #chkPrivileges" + PrivilegeId).is(':checked'))
                    $('#userDetail #chkPrivileges' + PrivilegeId).prop('checked', false);
                else
                    $('#userDetail #chkPrivileges' + PrivilegeId).prop('checked', true);

                utility.DisplayMessages(strMessage, 2);
                return;
            }
        });
    },

    SelectAllModuleFormPrivilegesSave: function (priviligeJSON) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Users", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#gvForm_row" + userDetail.formRowSelect).hasClass('active')) {
                    if ($("#userDetail #chkPrivliageSelectAll").is(':checked')) {
                        userDetail.SaveModuleFormPrivilegesSelectAll(priviligeJSON, userDetail.moduleFormUserID).done(function (response) {
                            if (response.status != false) {
                                Priviliges = JSON.parse(response.Privilige_JSON)
                                $.each(Priviliges, function (i, item) {
                                    $('#' + userDetail.params.PanelID + ' #dgvPrivileges #chkPrivileges' + item.PrivilegeSelectionId).attr('moduleformuserprivilegeid', item.ModuleFormUserPriviligesId);
                                });
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                    else {
                        userDetail.DeleteModuleFormUserPrivilegesSelectAll(priviligeJSON).done(function (response) {
                            if (response.status != false) {
                                $('#' + userDetail.params.PanelID + ' #dgvPrivileges input[type=checkbox]').each(function () {
                                    $(this).attr('moduleformuserprivilegeid', "");
                                });
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }
            }
            else {


                $('#' + userDetail.params.PanelID + ' #dgvPrivileges input[type=checkbox]').each(function () {

                    if ($(this).is(':checked'))
                        $(this).prop('checked', false);
                    else
                        $(this).prop('checked', true);
                });


                if ($("#userDetail #chkPrivliageSelectAll").is(':checked'))
                    $('#userDetail #chkPrivliageSelectAll').prop('checked', false);
                else
                    $('#userDetail #chkPrivliageSelectAll').prop('checked', true);

                utility.DisplayMessages(strMessage, 2);
                return;
            }


        });
    },

    ValidateUser: function () {
        $('#frmUserDetail')
    .bootstrapValidator({
        live: 'disabled',
        message: 'This value is not valid',
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            FirstName: {
                group: '.col-sm-3',
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            LastName: {
                group: '.col-sm-6',
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            Entity: {
                group: '.col-sm-3',
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            AutoLogOff: {
                group: '.col-sm-4',
                validators: {
                    notEmpty: {
                        message: ''
                    },
                    between: {
                        min: 10,
                        max: 30,
                        message: 'The Auto Log Off must be between 10 to 30 minutes'
                    }
                }
            },
            MobSessionExpTime: {
                group: '.col-sm-4',
                validators: {
                    notEmpty: {
                        message: ''
                    },
                    between: {
                        min: 10,
                        max: 30,
                        message: 'Check in App time out must be between 10 to 30 minutes'
                    }
                }
            },
            Roles: {
                group: '.col-sm-3',
                validators: {
                    notEmpty: {
                        message: ''
                    }
                }
            },
            RcUserName: {
                group: '.col-sm-4',
                enabled: false,
                validators: {
                    notEmpty: {
                        message: ''
                    },
                    stringLength: {
                        min: 2,
                        message: 'User Name must be 2 characters long'
                    }
                }
            },
            RcPassword: {
                group: '.col-sm-4',
                enabled: false,
                validators: {
                    notEmpty: {
                        message: ''
                    },
                    regexp: {
                        regexp: /((?=.*\d)(?=.*[a-zA-Z])(?=.*[\_\W]).{8,50})/,
                        message: 'The minimum 8 character password should have alphabets, digits & Special Characters'
                    }

                }
            },
            RcSigPassword: {
                group: '.col-sm-4',
                enabled: false,
                validators: {
                    notEmpty: {
                        message: ''
                    },
                }
            },
            UserName: {
                group: '.col-sm-6',
                validators: {
                    notEmpty: {
                        message: ''
                    },
                    stringLength: {
                        min: 2,
                        message: 'User Name must be 2 characters long'
                    }
                    //regexp: {
                    //    regexp: /^[a-zA-Z0-9_\.]+$/,
                    //    message: 'It should alphabetical, number, dot and underscore'
                    //},
                    //different: {
                    //    field: 'password',
                    //    message: 'UserName and Password cannot be same'
                    //}
                }
            },
            email: {
                group: '.col-sm-3',
                validators: {
                    notEmpty: {
                        message: ''
                    },
                    regexp: {
                        regexp: '^[^@\\s]+@([^@\\s]+\\.)+[^@\\s]+$',
                        message: 'Email not Valid'
                    }

                }
            },
            directAddress: {
                group: '.col-sm-6',
                enabled: false,
                validators: {
                    regexp: {
                        regexp: '^[^@\\s]+@([^@\\s]+\\.)+[^@\\s]+$',
                        message: 'Email not Valid'
                    }

                }
            },
            Password: {
                //feedbackIcons: 'false',
                group: '.col-sm-6',
                validators: {
                    notEmpty: {
                        message: ''
                    },
                    regexp: {
                        regexp: /((?=.*\d)(?=.*[a-zA-Z])(?=.*[\_\W]).{8,50})/,
                        message: 'The minimum 8 character password should have alphabets, digits & Special Characters'
                    }
                }
            }
        }
    }).on('success.form.bv', function (e) {
        e.preventDefault();
        userDetail.UserSave();
    }).on('submit.form.bv', function (e) {
        e.preventDefault();
        setTimeout(function () { $('#txtAutoLogOff').parent().find("[data-bv-validator='between']").hide() }, 3000);
    });
    },
    enabledirectvalidation: function (obj) {
        var objdirect = $('#userDetail #frmUserDetail');
        var formValidation = objdirect.data("bootstrapValidator");
        if ($(obj).val() != "") {
            formValidation.enableFieldValidators('directAddress', true);
        }
        else {
            formValidation.enableFieldValidators('directAddress', false);
        }
    },
    UserSave: function () {


        $('#frmUserDetail').data('serialize', $('#frmUserDetail').serialize());
        var strMessage = "";
        //adnan maqbool, pms-4913
        var isSecurityGroupCheck = false;
        //$('#userDetail #pnlPriviligesGroupMain #pnlPriviligesGroup tr [type=checkbox]').each(function () {
        //    if ($(this).prop('checked') == true) {
        //        isSecurityGroupCheck = true;
        //    }
        //});
        //if (isSecurityGroupCheck == true || userDetail.params.IsUAdmin == "True") {
        var self = $("#userDetail");
        var mj = self.getMyJSON();
        var myJSON = JSON.parse(mj);
        myJSON.UserSelectedDocuments = $('#' + userDetail.params.PanelID + ' #foldersddl ul.multiselect-container li input[type=checkbox]:checked').map(function () {
            return this.value;
        }).get().join(',');
        if (myJSON.UserSelectedDocuments.includes("multiselect-all,"))
        {
            myJSON.UserSelectedDocuments = myJSON.UserSelectedDocuments.replace("multiselect-all,","")
        }
        myJSON = JSON.stringify(myJSON);
        if (userDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Users", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    if (userDetail.params.UserId == "-1") {
                        userDetail.SaveUser(myJSON).done(function (response) {
                            if (response.status != false) {
                                userID = response.UserId;
                                userDetail.params.UserId = response.UserId;
                                userDetail.params.mode = "Edit"
                                userDetail.LoadUser();

                                $("#userDetail #pnlAditionalPriviliges").removeClass('disableAll');
                                $("#userDetail #pnlPriviligesGroupMain").removeClass('disableAll');
                                $("#userDetail #txtUserName").attr("disabled", "disabled");
                                Admin_User.UserSearch(userID);
                                utility.DisplayMessages(response.message, 1);
                                utility.DisplayMessages('Please select at least one Security Entity Group', 3);
                            }
                            else {

                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                    else if (userDetail.params.UserId != "-1" && userDetail.params.UserId != "" && userDetail.params.UserId != "0") {
                        $('#userDetail #pnlPriviligesGroupMain #pnlPriviligesGroup tr [type=checkbox]').each(function () {
                            if ($(this).prop('checked') == true) {
                                isSecurityGroupCheck = true;
                            }
                        });
                        if (isSecurityGroupCheck == true) {
                            userDetail.UpdateUser(myJSON, userDetail.params.UserId, 1).done(function (response) {
                                if (response.status != false) {
                                    utility.DisplayMessages(response.message, 1);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                        else {

                            utility.DisplayMessages('Please select at least one Security Entity Group', 3);
                        }
                    }
                }
                else

                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else if (userDetail.params.mode == "Edit") {
            $('#userDetail #pnlPriviligesGroupMain #pnlPriviligesGroup tr [type=checkbox]').each(function () {
                if ($(this).prop('checked') == true) {
                    isSecurityGroupCheck = true;
                }
            });
            if (isSecurityGroupCheck == true) {
                AppPrivileges.GetFormPrivileges("Users", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        userDetail.UpdateUser(myJSON, userDetail.params.UserId, 2).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                if (userDetail.params.RefCtrl != "LoggedInUsers") {
                                    Admin_User.UserSearch(userDetail.params.UserId);
                                }
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
            else {
                utility.DisplayMessages('Please select at least one Security Entity Group', 3);
            }
        }

    },

    SaveUser: function (UserData) {
        var data = "UserData=" + UserData;

        return MDVisionService.defaultService(data, "ADMIN_USER_DETAIL", "SAVE_USER");
    },

    SaveModuleForm: function (UserID, ModuleFormId) {
        var data = "UserID=" + UserID + "&ModuleFormID=" + ModuleFormId;
        return MDVisionService.defaultService(data, "ADMIN_USER_DETAIL", "SAVE_USER_MODULE_FORM");
    },

    DeleteModuleFormUser: function (ModuleFormUserId) {
        var data = "ModuleFormUserID=" + ModuleFormUserId;
        return MDVisionService.defaultService(data, "ADMIN_USER_DETAIL", "DELETE_USER_MODULE_FORM");
    },

    DeleteModuleFormUserPrivileges: function (ModuleFormUserPrivilegeId, PrivilegeName, FormName, AssignedTo) {
        var data = "ModuleFormUserPrivilegeID=" + ModuleFormUserPrivilegeId + "&PrivilegeName=" + PrivilegeName + "&FormName=" + FormName + "&AssignedTo=" + AssignedTo;
        return MDVisionService.defaultService(data, "ADMIN_USER_DETAIL", "DELETE_USER_MODULE_FORM_PRIVILEGE");
    },
    DeleteModuleFormUserPrivilegesSelectAll: function (priviligeJSON) {
        var data = "PriviligeJSon=" + priviligeJSON;
        return MDVisionService.defaultService(data, "ADMIN_USER_DETAIL", "DELETE_USER_MODULE_FORM_PRIVILEGE_SELECT_ALL");
    },

    SaveModuleFormPrivileges: function (ModuleFormPrivilegeId, ModuleFormUserID, PrivilegeName, FormName, AssignedTo) {
        var data = "ModuleFormPrivilegeID=" + ModuleFormPrivilegeId + "&ModuleFormUserID=" + ModuleFormUserID + "&PrivilegeName=" + PrivilegeName + "&FormName=" + FormName + "&AssignedTo=" + AssignedTo;
        return MDVisionService.defaultService(data, "ADMIN_USER_DETAIL", "SAVE_USER_MODULE_FORM_PRIVILEGE");
    },

    SaveModuleFormPrivilegesSelectAll: function (priviligeJSON, ModuleFormUserID) {
        var data = "PriviligeJSon=" + priviligeJSON + "&ModuleFormUserID=" + ModuleFormUserID;
        return MDVisionService.defaultService(data, "ADMIN_USER_DETAIL", "SAVE_USER_MODULE_FORM_PRIVILEGE_SELECT_ALL");
    },

    DeleteModuleForm: function (userID, ModuleId) {
        var data = "UserID=" + userID + "&ModuleId=" + ModuleId;
        return MDVisionService.defaultService(data, "ADMIN_USER_DETAIL", "DELETE_USER_MODULE");
    },

    UpdateUser: function (UserData, UserID, IsActive) {
        var data = "UserData=" + UserData + "&UserID=" + UserID + "&IsActive=" + IsActive;
        return MDVisionService.defaultService(data, "ADMIN_USER_DETAIL", "UPDATE_USER");
    },

    FillUser: function (UserID, ModuleID, ModuleFormID) {
        var data = "UserID=" + UserID + "&ModuleID=" + ModuleID + "&ModuleFormID=" + ModuleFormID;
        return MDVisionService.defaultService(data, "ADMIN_USER_DETAIL", "FILL_USER");
    },

    FillModules: function () {
        var data = "";
        return MDVisionService.defaultService(data, "ADMIN_USER_DETAIL", "FILL_MODULES");
    },

    FillModuleForms: function (ModuleId) {
        var data = "ModuleId=" + ModuleId;
        return MDVisionService.defaultService(data, "ADMIN_USER_DETAIL", "FILL_MODULE_FORMS");
    },

    FillPrivileges: function () {
        var data = "";
        return MDVisionService.defaultService(data, "ADMIN_USER_DETAIL", "FILL_PRIVILEGES");
    },

    DeleteSecurityGroup: function (UsersEntityGroupId) {
        var data = "UsersEntityGroupId=" + UsersEntityGroupId;
        return MDVisionService.defaultService(data, "ADMIN_USER_DETAIL", "DELETE_SECURITY_GROUP");
    },

    SaveSecurityGroup: function (SecurityGroupId, UserId, EntityId) {
        var data = "SecurityGroupId=" + SecurityGroupId + "&UserId=" + UserId + "&EntityId=" + EntityId;
        return MDVisionService.defaultService(data, "ADMIN_USER_DETAIL", "SAVE_SECURITY_GROUP");
    },

    UpdateUserActiveInactive: function (UserID, IsActive) {
        var data = "UserID=" + UserID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_USER_DETAIL", "UPDATE_USER_ACTIVE_INACTIVE");
    },

    UserNameAvailability: function () {
        var userName = $('#userDetail #txtUserName').val();
        if (userName != '') {
            userDetail.NameAvailability(userName).done(function (response) {
                if (response.status == false) {
                    if (response.Message == "1") {
                        if ($("#userDetail #lblAvaiability").css("display") == "none") {
                            $("#userDetail #lblAvaiability").css("display") == "block";

                            $("#userDetail #lblAvaiability").text("Not Availble");
                        }
                    }
                }
            });
        }
    },

    NameAvailability: function (userName) {
        var data = "UserName=" + userName;
        return MDVisionService.defaultService(data, "ADMIN_USER_DETAIL", "CHECK_USER_AVAILABILITY");
    },

    ChangePassword: function () {
        var params = [];
        params['UserID'] = userDetail.params.UserId;
        params["ParentCtrl"] = "userDetail";
        params["FromAdmin"] = "0";
        LoadActionPan('DashBoardChangePwd', params)
    },

    PriviligeSelectAll: function (ev) {
        var AllPrivilige = [];
        if (!ev.checked) {
            $('#' + userDetail.params.PanelID + ' #dgvPrivileges input[type=checkbox]').each(function (i, item) {
                if ($(this).is(":checked") == true) {
                    AllPrivilige.push({ 'ModuleFormUserPrivilegeId': $(this).attr('moduleformuserprivilegeid'), 'PrivilegeName': $(this).parent().next().text(), 'FormName': $("#userDetail #pnlModules").find('tr.active').text() + " - " + $("#userDetail #pnlForms").find('tr.active').text(), 'AssignedTo': $('#' + userDetail.params.PanelID + ' #txtUserName').val() });
                    // { 'FacilityId': $(this).attr('FacilityId'), 'PracticeId': $(this).attr('practiceid'), 'EntityId': $(this).attr('entityid') }
                    $(this).prop('checked', false);
                }
            });
        }
        else {
            $('#' + userDetail.params.PanelID + ' #dgvPrivileges input[type=checkbox]').each(function (i, item) {
                if ($(this).is(":checked") == false) {
                    AllPrivilige.push({ 'PrivilegeId': $(this).attr('privilegeid'), 'PrivilegeName': $(this).parent().next().text(), 'FormName': $("#userDetail #pnlModules").find('tr.active').text() + " - " + $("#userDetail #pnlForms").find('tr.active').text(), 'AssignedTo': $('#' + userDetail.params.PanelID + ' #txtUserName').val() });
                    $(this).prop('checked', true);
                }
            });
        }

        var priviligeJSON = JSON.stringify(AllPrivilige);
        userDetail.SelectAllModuleFormPrivilegesSave(priviligeJSON);

    },

    UnLoad: function () {
        var isSecurityGroupCheck = false;
        $('#userDetail #pnlPriviligesGroupMain #pnlPriviligesGroup tr [type=checkbox]').each(function () {
            if ($(this).prop('checked') == true) {
                isSecurityGroupCheck = true;
            }
        });
        if (isSecurityGroupCheck == true || userDetail.params.UserId == -1 || userDetail.params.mode == "Edit") {
                utility.UnLoadDialog("frmUserDetail", function () {
                    UnloadActionPan();
                }, function () {
                    UnloadActionPan();
                });
        }
        else {
            //EMR - 3481 -- as suggested
            UnloadActionPan();
            //utility.DisplayMessages('Please select at least one Security Entity Group', 3);
        }
    },

    UserRoleDropDown: function () {
        if (($('#chkIsAdmin').prop('checked')) != false) {
            $('#userDetail #lstUserRoles').find('option:first').prop('selected', true)
            $("#userDetail #lstUserRoles").find('option[refvalue=True]').show();
            $('#userDetail #lstUserRoles').trigger('change');
        }
        else {
            $('#userDetail #lstUserRoles').find('option:first').prop('selected', true)
            $('#userDetail #lstUserRoles').find('option[refvalue=True]').hide();
            $('#userDetail #lstUserRoles').trigger('change');
        }
    },



    //Disable user role field if emergency role is selected
    //Author: ZeeshanAK  | Date: April 13, 2016 
    disableUserRole: function (obj) {
        //    if ($(obj).val() == "") {
        //        $('#lstUserRoles').prop('disabled', false);
        //    } else {
        //        $("#lstUserRoles option:selected").removeAttr("selected");
        //        $('#lstUserRoles').prop('disabled', true);
        //    }
    },

    RCopialUserChange: function (obj) {
        var objdirect = $('#userDetail #frmUserDetail');
        var formValidation = objdirect.data("bootstrapValidator");
        if ($(obj).prop('checked')) {
            $('#userDetail #RcopiaUserInfoDiv').show();
            formValidation.enableFieldValidators('RcUserName', true);
            formValidation.enableFieldValidators('RcPassword', true);
            formValidation.enableFieldValidators('RcSigPassword', true);
        }
        else {
            $('#userDetail #RcopiaUserInfoDiv').hide();
            formValidation.enableFieldValidators('RcUserName', false);
            formValidation.enableFieldValidators('RcPassword', false);
            formValidation.enableFieldValidators('RcSigPassword', false);
        }
    },

    SelectAllModuleFormPrivilige: function (FormsId, ModuleFormId, ModuleId, MFUId) {
        var AllPrivilige = [];
        var FormCount = 0;
        if (FormsId == userDetail.FormUsedId || userDetail.FormUsedId == 0) {
            userDetail.FormIdclick++;
            userDetail.FormUsedId = FormsId;
        }
        else {
            userDetail.FormIdclick = 1;
            userDetail.FormUsedId = FormsId;
        }
        if (userDetail.FormIdclick == 5) {
            //userDetail.FormIdclick = 1;
            //var objdfd = jQuery.Deferred();
            //var FormLength = $('#frmUserDetail #pnlForms input').length;
            //$('#frmUserDetail #pnlForms input').each(function (i, item) {
            //    if ($(item).prop('checked') == false || $(item).parent().parent().hasClass('active')) {
            //        var dfd = jQuery.Deferred();
            //        userDetail.SaveModuleForm(userDetail.params.UserId, $(item).parent().parent().attr('ModuleFormId')).done(function (response) {
            //            if (response.status != false) {
            //                userDetail.moduleFormUserID = response.ModuleFormUserId;
            //                AllPrivilige = [{ "PrivilegeId": "1", "PrivilegeName": "Search", "FormName": $("#userDetail #pnlModules").find('tr.active').text() + " - ALL", "AssignedTo": $('#' + userDetail.params.PanelID + ' #txtUserName').val() }, { "PrivilegeId": "2", "PrivilegeName": "View", "FormName": $("#userDetail #pnlModules").find('tr.active').text() + "ALL", "AssignedTo": $('#' + userDetail.params.PanelID + ' #txtUserName').val() }, { "PrivilegeId": "3", "PrivilegeName": "Edit", "FormName": $("#userDetail #pnlModules").find('tr.active').text() + "ALL", "AssignedTo": $('#' + userDetail.params.PanelID + ' #txtUserName').val() }, { "PrivilegeId": "4", "PrivilegeName": "Delete", "FormName": $("#userDetail #pnlModules").find('tr.active').text() + "ALL", "AssignedTo": $('#' + userDetail.params.PanelID + ' #txtUserName').val() }, { "PrivilegeId": "5", "PrivilegeName": "Print", "FormName": $("#userDetail #pnlModules").find('tr.active').text() + "ALL", "AssignedTo": $('#' + userDetail.params.PanelID + ' #txtUserName').val() }, { "PrivilegeId": "6", "PrivilegeName": "Add", "FormName": $("#userDetail #pnlModules").find('tr.active').text() + "ALL", "AssignedTo": $('#' + userDetail.params.PanelID + ' #txtUserName').val() }, { "PrivilegeId": "7", "PrivilegeName": "Scan", "FormName": $("#userDetail #pnlModules").find('tr.active').text() + "ALL", "AssignedTo": $('#' + userDetail.params.PanelID + ' #txtUserName').val() }];
            //                var priviligeJSON = JSON.stringify(AllPrivilige);
            //                userDetail.SaveModuleFormPrivilegesSelectAll(priviligeJSON, response.ModuleFormUserId).done(function (response) {
            //                    if (response.status == false) {
            //                        utility.DisplayMessages(response.Message, 3);
            //                    }
            //                });
            //                FormCount++;
            //                if (FormCount == FormLength) {
            //                    objdfd.resolve();
            //                }
            //                dfd.resolve(response);
            //                return dfd.promise();

            //            }
            //            else {
            //                utility.DisplayMessages(response.Message, 3);
            //            }
            //        });
            //    }
            //    //objdfd.resolve();
            //});

            //objdfd.then(function () {
            //    $("#userDetail #pnlModules").find('tr.active').trigger('click');
            //    console.log('click');
            //});
            //return objdfd.promise();

        }
    },
    LoadUserActivityLog: function () {
        var params = [];
        params["ParentCtrl"] = "userDetail";
        params["UserId"] = userDetail.params.UserId;
        LoadActionPan('Activity_Log', params);
    },
}
