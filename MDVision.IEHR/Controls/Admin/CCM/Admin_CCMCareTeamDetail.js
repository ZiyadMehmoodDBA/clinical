Admin_CCMCareTeamDetail = {
    params: [],
    PCPCount: 0,
    CMCount: 0,
    CCCount: 0,
    CGCount: 0,
    ProviderCount: 0,
    Load: function (params) {
        Admin_CCMCareTeamDetail.params = params;
        Admin_CCMCareTeamDetail.ValidateCareTeam();
        Admin_CCMCareTeamDetail.bindPCP();
        Admin_CCMCareTeamDetail.bindProvider();
        if (Admin_CCMCareTeamDetail.params.mode == "Edit") {
            Admin_CCMCareTeamDetail.editCareTeam(Admin_CCMCareTeamDetail.params.Id, Admin_CCMCareTeamDetail.params.IsActive);
        }
        var This = $('#pnlAdmin_CCMCareTeamDetail');
        This.find("#CMDropDown").loadDropDowns(true, "IsActive=true&ID=1", Admin_CCMCareTeamDetail.params["PanelID"]).done(function () {
            This.find('#txtCM').on('change', function () {
                var name = This.find('#txtCM').find(":selected").text();
                var Id = This.find('#txtCM').find(":selected").attr('value');
                Admin_CCMCareTeamDetail.Grid("CM", Id, name);
            });
        });

        This.find("#CCDropDown").loadDropDowns(true, "IsActive=true&ID=1", Admin_CCMCareTeamDetail.params["PanelID"]).done(function () {
            This.find('#txtCC').on('change', function () {
                var name = This.find('#txtCC').find(":selected").text();
                var Id = This.find('#txtCC').find(":selected").attr('value');
                Admin_CCMCareTeamDetail.Grid("CC", Id, name);
            });
        });

        This.find("#CGDropDown").loadDropDowns(true, "IsActive=true&ID=1", Admin_CCMCareTeamDetail.params["PanelID"]).done(function () {
            This.find('#txtCG').on('change', function () {
                var name = This.find('#txtCG').find(":selected").text();
                var Id = This.find('#txtCG').find(":selected").attr('value');
                Admin_CCMCareTeamDetail.Grid("CG", Id, name);
            });
        });
    },

    bindPCP: function () {
        var Ctrl = $('#Admin_CCMCareTeamDetail #txtPCP');
        var func = function () { return utility.GetRefProviderArray(Ctrl.val()) };
        var onSelect = function (e) { Admin_CCMCareTeamDetail.Grid("PCP", e.id, e.value) };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, null, onSelect);
    },
    bindCM: function () {
        var shortName = $('#Admin_CCMCareTeamDetail #txtCM').val();
        utility.GetUserArray(shortName, null, true).done(function (response) {

            $('#Admin_CCMCareTeamDetail #txtCM').autocomplete({
                autoFocus: true,
                source: response,
                select: function (event, ui) {
                    setTimeout(function () {
                        Admin_CCMCareTeamDetail.Grid("CM", ui.item.id, ui.item.value);
                    }, 100);
                }
            });
        });
    },
    bindCC: function () {
        var shortName = $('#Admin_CCMCareTeamDetail #txtCC').val();
        utility.GetUserArray(shortName, null, true).done(function (response) {

            $('#Admin_CCMCareTeamDetail #txtCC').autocomplete({
                autoFocus: true,
                source: response,
                select: function (event, ui) {
                    setTimeout(function () {
                        Admin_CCMCareTeamDetail.Grid("CC", ui.item.id, ui.item.value);
                    }, 100);
                }
            });
        });
    },
    bindCG: function () {
        var shortName = $('#Admin_CCMCareTeamDetail #txtCG').val();
        utility.GetUserArray(shortName, null, true).done(function (response) {
            $('#Admin_CCMCareTeamDetail #txtCG').autocomplete({
                autoFocus: true,
                source: response,
                select: function (event, ui) {
                    setTimeout(function () {
                        Admin_CCMCareTeamDetail.Grid("CG", ui.item.id, ui.item.value);
                    }, 100);
                }
            });
        });
    },
    bindProvider: function () {
        var providers = [];
        var Ctrl = $('#Admin_CCMCareTeamDetail #txtProvider');
        var onSelect = function (e) { Admin_CCMCareTeamDetail.Grid("Provider", e.id, e.value) };
        CacheManager.BindCodes('GetProvider', true).done(function (response) {
            response = JSON.parse(response.GetProvider);
            $.each(response, function (i, item) {
                if (item.Name.search(Ctrl.val()) != -1) {
                    providers.push({ id: item.Value, value: item.Name });
                }
            });
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", providers, null, null, onSelect);
        });
    },
    Grid: function (type, Id, Name) {
        if (type == "CM") {
            //if (Admin_CCMCareTeamDetail.CMCount < 1) { // only value can be added
            
            $("#hfCM").val(Id); // add the selected id
            table = $('#Admin_CCMCareTeamDetail #dgvCM tbody');
            var ifExist = false;

            $(table).find('tr').each(function () {
                     var id = $(this).attr('id');
                     if (id === Id)
                     {
                         ifExist = true;
                         
                     }
            }
);
            if (ifExist == false) {
                table.append('<tr id=' + Id + ' style="background-color:#E8E8E8 ;"><td>  ' + Name + '</td><td style="float:right;"><a id="CM' + Id + '" class="btn  btn-xs" href="#" onclick="Admin_CCMCareTeamDetail.removeField(this, \'CM\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;</td></tr>');
                Admin_CCMCareTeamDetail.CMCount++;
            }
           
            $('#Admin_CCMCareTeamDetail #txtCM').children(':selected').text(Name);
            if (Admin_CCMCareTeamDetail.CMCount >= 3) { // only three value can be added
             $('#Admin_CCMCareTeamDetail #txtCM').attr('disabled', 'disabled');
              }
        }
        else if (type == "PCP") {
            if (Admin_CCMCareTeamDetail.PCPCount < 1) { // only value can be added
                Admin_CCMCareTeamDetail.PCPCount++;
                $("#hfPCP").val(Id); // add the selected id
                table = $('#Admin_CCMCareTeamDetail #dgvPCP tbody');
                table.append('<tr id=' + Id + ' style="background-color:#E8E8E8 ;"><td>  ' + Name + '</td><td style="float:right;"><a id="' + Id + '" class="btn  btn-xs" href="#" onclick="Admin_CCMCareTeamDetail.removeField(this, \'PCP\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;</td></tr>');
                $('#Admin_CCMCareTeamDetail #txtPCP').attr('disabled', 'disabled');
            }
        }
        else if (type == "CC") {
          //  if (Admin_CCMCareTeamDetail.CCCount < 1) { // only value can be added
           
            $("#hfCC").val(Id); // add the selected id
            table = $('#Admin_CCMCareTeamDetail #dgvCC tbody');

                var ifExist = false;

                $(table).find('tr').each(function () {
                    var id = $(this).attr('id');
                    if (id === Id) {
                        ifExist = true;

                    }
                }
    );
                
                if (ifExist == false) {
                    table.append('<tr id=' + Id + ' style="background-color:#E8E8E8 ;"><td>  ' + Name + '</td><td style="float:right;"><a id="CC' + Id + '" class="btn  btn-xs" href="#" onclick="Admin_CCMCareTeamDetail.removeField(this, \'CC\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;</td></tr>');
                    Admin_CCMCareTeamDetail.CCCount++;
                }
                $('#Admin_CCMCareTeamDetail #txtCC').children(':selected').text(Name);
                if (Admin_CCMCareTeamDetail.CCCount >= 3) {
                $('#Admin_CCMCareTeamDetail #txtCC').attr('disabled', 'disabled');

            }
        }
        else if (type == "CG") {
          //  if (Admin_CCMCareTeamDetail.CGCount < 1) { // only value can be added
               
                $("#hfCG").val(Id); // add the selected id
                table = $('#Admin_CCMCareTeamDetail #dgvCG tbody');
                var ifExist = false;

                $(table).find('tr').each(function () {
                    var id = $(this).attr('id');
                    if (id === Id) {
                        ifExist = true;

                    }
                }
    );
                if (ifExist == false)
                {
                    table.append('<tr id=' + Id + ' style="background-color:#E8E8E8 ;"><td>  ' + Name + '</td><td style="float:right;"><a id="CG' + Id + '" class="btn  btn-xs" href="#" onclick="Admin_CCMCareTeamDetail.removeField(this, \'CG\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;</td></tr>');
                    Admin_CCMCareTeamDetail.CGCount++;
                }
                
                $('#Admin_CCMCareTeamDetail #txtCG').children(':selected').text(Name);
                if (Admin_CCMCareTeamDetail.CGCount >=3)
{
                $('#Admin_CCMCareTeamDetail #txtCG').attr('disabled', 'disabled');
            }
        }
        else if (type == "Provider") {
            if (Admin_CCMCareTeamDetail.ProviderCount < 1) { // only value can be added
                Admin_CCMCareTeamDetail.ProviderCount++;
                $("#hfProvider").val(Id); // add the selected id
                table = $('#Admin_CCMCareTeamDetail #dgvProvider tbody');
                table.append('<tr id=' + Id + ' style="background-color:#E8E8E8 ;"><td>  ' + Name + '</td><td style="float:right;"><a id="Provider' + Id + '" class="btn  btn-xs" href="#" onclick="Admin_CCMCareTeamDetail.removeField(this, \'Provider\');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;</td></tr>');
                $('#Admin_CCMCareTeamDetail #txtProvider').attr('disabled', 'disabled');
            }
        }
    },
    removeField: function (obj, type) {
        if (type == "PCP") {
            var item = obj.parentElement.parentElement;
            item.remove();
            $('#Admin_CCMCareTeamDetail #txtPCP').attr('disabled', false);
            $('#Admin_CCMCareTeamDetail #txtPCP').val('');
            Admin_CCMCareTeamDetail.PCPCount--;
        }
        else if (type == "CM") {
            var item = obj.parentElement.parentElement;
            item.remove();
           
            Admin_CCMCareTeamDetail.CMCount--;
            if (Admin_CCMCareTeamDetail.CMCount < 4)
            {
                $('#Admin_CCMCareTeamDetail #txtCM').attr('disabled', false);
            }
            $("#txtCM").val($("#txtCM option:first").val());
        }
        else if (type == "CC") {
            var item = obj.parentElement.parentElement;
            item.remove();
           
            Admin_CCMCareTeamDetail.CCCount--;
            if (Admin_CCMCareTeamDetail.CCCount < 4)
            {
                $('#Admin_CCMCareTeamDetail #txtCC').attr('disabled', false);
            }
            $("#txtCC").val($("#txtCC option:first").val());
        }
        else if (type == "CG") {
            var item = obj.parentElement.parentElement;
            item.remove();
         
            Admin_CCMCareTeamDetail.CGCount--;
            if (Admin_CCMCareTeamDetail.CGCount < 4)
            {
                $('#Admin_CCMCareTeamDetail #txtCG').attr('disabled', false);
            }
            $("#txtCG").val($("#txtCG option:first").val());
        }
        else if (type == "Provider") {
            var item = obj.parentElement.parentElement;
            item.remove();
            $('#Admin_CCMCareTeamDetail #txtProvider').attr('disabled', false);
            $('#Admin_CCMCareTeamDetail #txtProvider').val('');
            Admin_CCMCareTeamDetail.ProviderCount--;
        }
    },

    ValidateCareTeam: function () {
        $('#frmAdmin_CCMCareTeamDetail')
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                    careteam: {
                        name: 'careteam',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    //PCP: {
                    //    name: 'PCP',
                    //    validators: {
                    //        notEmpty: {
                    //            message: ''
                    //        }
                    //    }
                    //},
                    //CM: {
                    //    name: 'CM',
                    //    validators: {
                    //        notEmpty: {
                    //            message: ''
                    //        }
                    //    }
                    //},
                    //CC: {
                    //    name: 'CC',
                    //    validators: {
                    //        notEmpty: {
                    //            message: ''
                    //        }
                    //    }
                    //},
                    //CG: {
                    //    name: 'CG',
                    //    validators: {
                    //        notEmpty: {
                    //            message: ''
                    //        }
                    //    }
                    //},
                    //Provider: {
                    //    name: 'Provider',
                    //    validators: {
                    //        notEmpty: {
                    //            message: ''
                    //        }
                    //    }
                    //}

                }
            }).on('success.form.bv', function (e) {
                e.preventDefault();
                Admin_CCMCareTeamDetail.saveCareTeam();
            });

    },
    saveCareTeam: function () {

        var careTeamName = $('#Admin_CCMCareTeamDetail #txtName');
        var PCP = $('#Admin_CCMCareTeamDetail #dgvPCP tbody tr').attr('id');
        var CM = []
        $('#Admin_CCMCareTeamDetail #dgvCM tbody tr').each(function () {
            var id = $(this).attr('id');
            CM.push(id);
        });
        var CMString = CM.join();

        var CC = []
        $('#Admin_CCMCareTeamDetail #dgvCC tbody tr').each(function () {
            var id = $(this).attr('id');
            CC.push(id);
        });
        var CCString = CC.join();

        var CG = []
        $('#Admin_CCMCareTeamDetail #dgvCG tbody tr').each(function () {
            var id = $(this).attr('id');
            CG.push(id);
        });
        var CGString = CG.join();

        var Provider = $('#Admin_CCMCareTeamDetail #dgvProvider tbody tr').attr('id');

        //if (PCP == undefined || PCP == null) {
        //    utility.DisplayMessages("Please add Primary Care Provider", 3);
        //}
        if (CM == undefined || CM == null|| CM.length==0) {
            utility.DisplayMessages("Please add Care Manager", 3);
        }
        else if (CC == undefined || CC == null|| CC.length==0) {
            utility.DisplayMessages("Please add Care Coordinator", 3);
        }
        else if (CG == undefined || CG == null || CG.length == 0) {
            utility.DisplayMessages("Please add Care Giver", 3);
        }
        else if (Provider == undefined || Provider == null) {
            utility.DisplayMessages("Please add Provider", 3);
        }
        else {
            var object = new Object();
            object["CareTeamName"] = careTeamName.val();
            object["Description"] = "";
            object["PCPId"] =  PCP == undefined ? "" : PCP;
            object["CareManagerId"] = CMString;
            object["CareCoordinatorId"] = CCString;
            object["CareGiverId"] = CGString;
            object["ProviderId"] = Provider;


            if (Admin_CCMCareTeamDetail.params.mode == "Add") {
                object["IsActive"] = true;
                Admin_CCMCareTeamDetail.CCMSaveCareTeam(object).done(function (resp) {
                    if (resp.status == true) {
                        utility.DisplayMessages("Care Team has been saved", 1);
                        Admin_CCMCareTeam.loadCareTeams();
                        Admin_CCMCareTeamDetail.UnLoad();
                    }
                    else {
                        utility.DisplayMessages(resp.Message, 2);
                    }
                });
            }
            else if (Admin_CCMCareTeamDetail.params.mode == "Edit") {
                object["CareTeamId"] = Admin_CCMCareTeamDetail.params.Id;
                object["IsActive"] = Admin_CCMCareTeamDetail.params.IsActive;
                Admin_CCMCareTeamDetail.CCMUpdateCareTeam(object).done(function (resp) {
                    if (resp.status == true) {
                        utility.DisplayMessages("Care Team has been updated", 1);
                        Admin_CCMCareTeam.loadCareTeams();
                        Admin_CCMCareTeamDetail.UnLoad();
                    }
                    else {
                        utility.DisplayMessages(resp.message, 2);
                    }
                });
            }
        }
    },
    CCMSaveCareTeam: function (dataObj) {
        dataObj = JSON.stringify(dataObj);
        var data = "CCMCareTeamData=" + dataObj;
        return MDVisionService.defaultService(data, "ADMIN_CCMCARETEAM", "SAVE_CCMCARETEAM");
    },
    CCMUpdateCareTeam: function (dataObj) {
        CareTeamId = dataObj["CareTeamId"];
        IsActive = dataObj["IsActive"];
        dataObj = JSON.stringify(dataObj);
        var data = "CCMCareTeamData=" + dataObj + "&CareTeamID=" + CareTeamId + "&IsActive=" + IsActive;
        return MDVisionService.defaultService(data, "ADMIN_CCMCARETEAM", "UPDATE_CCMCARETEAM");
    },
    OpenPCP: function () {
        var params = [];
        params["ReferringProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtPCP";
        params["Title"] = "Search PCP Provider";
        params["ParentCtrl"] = "Admin_CCMCareTeamDetail";
        params["CareTeamType"] = "PCP";
        LoadActionPan('Admin_ReferringProvider', params);
    },
    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmDemographic";
        params["Panel"] = 'pnlAdminProvider';
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Admin_CCMCareTeamDetail";
        LoadActionPan('Admin_Provider', params);
    },
    OpenUser: function (type) {
        var params = [];
        params["ParentCtrl"] = "Admin_CCMCareTeamDetail";
        params["CareTeamFieldType"] = type;
        LoadActionPan('Admin_User', params);

    },
    editCareTeam: function (Id, IsActive) {
        Admin_CCMCareTeam.loadCCMCareTeam(Id, IsActive, "", "").done(function (response) {
            var response = JSON.parse(response.CCMCareTeamFill_JSON);
            $('#Admin_CCMCareTeamDetail #txtName').val(response[0].ShortName);
            if (response[0].PCPId != "") {
                Admin_CCMCareTeamDetail.Grid("PCP", response[0].PCPId, response[0].PCPName);
            }
            for (i = 0; i < response.length; i++) {
                Admin_CCMCareTeamDetail.Grid("CG", response[i].CareGiverId, response[i].CareGiverName);
                Admin_CCMCareTeamDetail.Grid("CM", response[i].CareManagerId, response[i].CareManagerName);
                Admin_CCMCareTeamDetail.Grid("CC", response[i].CareCoordinatorId, response[i].CareCoordinatorName);

            }
        
            Admin_CCMCareTeamDetail.Grid("Provider", response[0].ProviderId, response[0].ProviderName);
        });
    },

    UnLoad: function () {
        Admin_CCMCareTeamDetail.ProviderCount = 0;
        Admin_CCMCareTeamDetail.CCCount = 0;
        Admin_CCMCareTeamDetail.CGCount = 0;
        Admin_CCMCareTeamDetail.CMCount = 0;
        Admin_CCMCareTeamDetail.PCPCount = 0;
        if (Admin_CCMCareTeamDetail.params != null && Admin_CCMCareTeamDetail.params.ParentCtrl) {
            UnloadActionPan(Admin_CCMCareTeamDetail.params.ParentCtrl);
        }
        else {
            UnloadActionPan();
        }
    },
}