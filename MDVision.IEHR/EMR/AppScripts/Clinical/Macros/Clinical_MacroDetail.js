Clinical_MacroDetail = {
    params: [],
    Load: function (params) {
        Clinical_MacroDetail.params = params;
        Clinical_MacroDetail.loadNoteComponents();
        $('#pnlClinicalMacroDetail  #macTitle').html("Add");
        Clinical_MacroDetail.loadUsers().done(function (result) {
            if (params["Mode"] == "Edit") {
                var item = params["MacroId"];
                Clinical_MacroDetail.editMacroDetail(item);
                $('#pnlClinicalMacroDetail  #macTitle').html("Edit");
            }
        });
        Clinical_MacroDetail.validateMacro();
       
        $(function () {
            $('#pnlClinicalMacroDetail #txtKeyword').keydown(function (e) {
                if (e.keyCode == 32 || e.keyCode==188 || e.keyCode==190) // 32 is the ASCII value for a space
                    e.preventDefault();
            });
        });  
    },
    validateMacro: function () {


        $('#pnlClinicalMacroDetail #frmClinicalMacroDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {

                   MacroName: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Keyword: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Description: {
                       group: '.col-sm-12',
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
            Clinical_MacroDetail.Save();
        });

    },
    Save: function () {
        var self = $('#pnlClinicalMacroDetail');
        var objData = {};
        objData["MacroName"] = $('#pnlClinicalMacroDetail #txtMacroName').val();
        objData["Keyword"] = $('#pnlClinicalMacroDetail #txtKeyword').val();
        objData["Description"] = $('#pnlClinicalMacroDetail #txtDescription').val();
        //objData["Description"] = objData["Description"].replace(/\r?\n/gi, "<br>");
        var NoteComponentIds = self.find('#ddlMacroDetailComponentNames option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objData["NoteComponentIds"] = NoteComponentIds;

        var UserIds = self.find('#ddlMacroDetailUsers option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objData["UsersIds"] = UserIds;

    
            objData["IsIndependent"] = "false";
        

        if (Clinical_MacroDetail.params["Mode"] == "Edit") {
            objData["commandType"] = "Edit_Macro";
            objData["MacroID"] = Clinical_MacroDetail.params["MacroId"];
            Clinical_MacroDetail.Update_DBCall(objData).done(function (response) {
                if (response) {
                    utility.DisplayMessages("Successfully updated.", 1);
                    Clinical_Macro.Load();
                    Clinical_MacroDetail.UnLoad();
                }
                else {

                }
            });
        }
        else {
            objData["commandType"] = "Save_Macro";
            Clinical_MacroDetail.Save_DBCall(objData).done(function (response) {
                if (response) {
                    utility.DisplayMessages("Successfully Added.", 1);
                    Clinical_Macro.Load();
                    Clinical_MacroDetail.UnLoad();
                }
            });
        }

    },
    Save_DBCall: function (objData) {
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Macro", "Macro");
    },
    Update_DBCall: function (objData) {
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Macro", "Macro");
    },
    loadNoteComponents: function (dfd) {

        MDVisionService.lookups('GetNoteComponents', true).done(function (result) {
            result = JSON.parse(result["GetNoteComponents"]);
            var options = result;
            var $providerDdl = $('#pnlClinicalMacroDetail #ddlMacroDetailComponentNames');
            //var $providerHiddenDdl = $('#' + AOETemplateDetail.PanelID + ' #ddlHiddenAOETemplateDetailTemplateProvider');

            //Empty both the providers ddls.
            $providerDdl.empty();
            // $providerHiddenDdl.empty();

            //Loop through all providers loaded from the server
            $.each(options, function (i, item) {
                if (item.Value != "" && typeof item.Value != 'undefined') {

                    // User will see these providers in multiSelect dropdownlist
                    $providerDdl.append(
                        $('<option/>', {
                            value: item.Value,
                            html: item.Name,
                            refname: item.RefName,
                            refvalue: item.RefValue

                        })
                    );
                    // Populate hidden ddl provider
                    //A Hack to load all the providers in hidden dropdownlist
                    //$providerHiddenDdl.append(
                    //     $('<option/>', {
                    //         value: item.Value,
                    //         html: item.Name,
                    //         refname: item.RefName,
                    //         refvalue: item.RefValue

                    //     })
                    //);
                }
            });
            // Assigned server side providers to providerCheckedIds array and made selected
            if (AOETemplateDetail.ProviderIds != '') {
                var Providers = AOETemplateDetail.ProviderIds.split(",");
                AOETemplateDetail.providerCheckedIds = Providers;
                $('#' + AOETemplateDetail.PanelID + ' #ddlAOETemplateDetailProvider').val(Providers);
            }

        }).then(function () {
            // A hack to trigger the onDropDownHide event of Spacialty multiselect      
            // $('#' + AOETemplateDetail.PanelID + ' #divAOETemplateDetailSpecialty').find('.dropdown-toggle').dropdown('toggle').dropdown('toggle');
            //Intialized in onhidden spacialty ddl.  
            Clinical_MacroDetail.IntializeMultiSelectDropDownNoteComponents();
            if (dfd)
                dfd.resolve();
        });
        //enable multiselect
        //AOETemplateDetail.enableDisableDropDowLists('ddlAOETemplateDetailProvider', false);

    },
    IntializeMultiSelectDropDownNoteComponents: function () {
        $('#pnlClinicalMacroDetail #ddlMacroDetailComponentNames').multiselect('destroy');
        $('#pnlClinicalMacroDetail #ddlMacroDetailComponentNames').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: false,
            onChange: function (option, checked, select) {
                //AOETemplateDetail.checkSpecialtiesByProviderId(option, checked, select);
            },
            onDropdownHide: function (event) {
                // AOETemplateDetail.specialitiesByProviderIds();
                //Refresh multiselect
                //  $('#' + AOETemplateDetail.PanelID + ' #ddlAOETemplateDetailTemplateSpecialty').multiselect('refresh');
            },


        });
    },
    loadUsers: function () {
        var dfd = new $.Deferred();
        MDVisionService.lookups('GetUsersFullName', true).done(function (result) {
            result = JSON.parse(result["GetUsersFullName"]);
            var options = result;
            var $providerDdl = $('#pnlClinicalMacroDetail #ddlMacroDetailUsers');
            //var $providerHiddenDdl = $('#' + AOETemplateDetail.PanelID + ' #ddlHiddenAOETemplateDetailTemplateProvider');

            //Empty both the providers ddls.
            $providerDdl.empty();
            // $providerHiddenDdl.empty();

            //Loop through all providers loaded from the server
            $.each(options, function (i, item) {
                if (item.Value != "" && typeof item.Value != 'undefined') {

                    // User will see these providers in multiSelect dropdownlist
                    $providerDdl.append(
                        $('<option/>', {
                            value: item.Value,
                            html: item.Name,
                            refname: item.RefName,
                            refvalue: item.RefValue

                        })
                    );
                    // Populate hidden ddl provider
                    //A Hack to load all the providers in hidden dropdownlist
                    //$providerHiddenDdl.append(
                    //     $('<option/>', {
                    //         value: item.Value,
                    //         html: item.Name,
                    //         refname: item.RefName,
                    //         refvalue: item.RefValue

                    //     })
                    //);
                }
            });
            // Assigned server side providers to providerCheckedIds array and made selected
            if (AOETemplateDetail.ProviderIds != '') {
                var Providers = AOETemplateDetail.ProviderIds.split(",");
                AOETemplateDetail.providerCheckedIds = Providers;
                $('#' + AOETemplateDetail.PanelID + ' #ddlAOETemplateDetailProvider').val(Providers);
            }

        }).then(function () {
            // A hack to trigger the onDropDownHide event of Spacialty multiselect      
            // $('#' + AOETemplateDetail.PanelID + ' #divAOETemplateDetailSpecialty').find('.dropdown-toggle').dropdown('toggle').dropdown('toggle');
            //Intialized in onhidden spacialty ddl.  
            Clinical_MacroDetail.IntializeMultiSelectDropDownUsers();
            if (dfd)
                dfd.resolve();
        });

        return dfd.promise();
        //enable multiselect
        //AOETemplateDetail.enableDisableDropDowLists('ddlAOETemplateDetailProvider', false);

    },
    IntializeMultiSelectDropDownUsers: function () {
        $('#pnlClinicalMacroDetail #ddlMacroDetailUsers').multiselect('destroy');
        $('#pnlClinicalMacroDetail #ddlMacroDetailUsers').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: false,
            onChange: function (option, checked, select) {
                //AOETemplateDetail.checkSpecialtiesByProviderId(option, checked, select);
            },
            onDropdownHide: function (event) {
                // AOETemplateDetail.specialitiesByProviderIds();
                //Refresh multiselect
                //  $('#' + AOETemplateDetail.PanelID + ' #ddlAOETemplateDetailTemplateSpecialty').multiselect('refresh');
            },


        });
        $("#pnlClinicalMacroDetail #ddlMacroDetailUsers").val(globalAppdata.AppUserId);
        $('#pnlClinicalMacroDetail #ddlMacroDetailUsers').multiselect("refresh");
    },
    editMacroDetail: function (item) {
        var data;

        Clinical_MacroDetail.LoadMacro(item).done(function (response) {
            response = JSON.parse(response);
            data = response.macros;
            $('#pnlClinicalMacroDetail #txtMacroName').val(data[0].MacroName);
            $('#pnlClinicalMacroDetail #txtKeyword').val(data[0].Keyword);
            $('#pnlClinicalMacroDetail #txtDescription').val(data[0].Description);


            $('#pnlClinicalMacroDetail #ddlMacroDetailUsers').multiselect('clearSelection', false);
            $('#pnlClinicalMacroDetail #ddlMacroDetailUsers').multiselect('updateButtonText');
            $("#pnlClinicalMacroDetail #ddlMacroDetailUsers").val(data[0].UsersIds.split(','));
            $('#pnlClinicalMacroDetail #ddlMacroDetailUsers').multiselect("refresh");

            $('#pnlClinicalMacroDetail #ddlMacroDetailComponentNames').multiselect('clearSelection', false);
            $('#pnlClinicalMacroDetail #ddlMacroDetailComponentNames').multiselect('updateButtonText');
            $("#pnlClinicalMacroDetail #ddlMacroDetailComponentNames").val(data[0].NoteComponentIds.split(','));
            $('#pnlClinicalMacroDetail #ddlMacroDetailComponentNames').multiselect("refresh");
        });


    },
    LoadMacro: function (item) {

        var obj = new Object();
        obj["MacroId"] = item;
        obj["commandType"] = "Get_Macro";
        var data = JSON.stringify(obj);
        return MDVisionService.APIService(data, "Macro", "Macro");
    },
    UnLoad: function () {

        UnloadActionPan();

    }
}
/*CacheManager.BindCodes('GetNoteComponents', false).done(function () {
                compDefer.resolve('ok');
            });*/
