// Created By:  Muhammad Ahmad Imran
// Created Date: 3/15/2016
Clinical_Provider_Note_Template = {
    bIsFirstLoad: true,
    params: [],
    Switch: 1,
    specialityCheckedIds: [],
    providerCheckedIds: [],
    SpecialtyIds: '',
    ProviderIds: '',
    Load: function (params) {
        Clinical_Provider_Note_Template.params = params;

        if (Clinical_Provider_Note_Template.params.PanelID != 'pnlClinicalProviderNoteTemplate') {
            Clinical_Provider_Note_Template.params.PanelID = Clinical_Provider_Note_Template.params.PanelID + ' #pnlClinicalProviderNoteTemplate';
        } else {
            Clinical_Provider_Note_Template.params.PanelID = 'pnlClinicalProviderNoteTemplate';
        }

        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Clinical_Provider_Note_Template.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        var self = $('#' + Clinical_Provider_Note_Template.params.PanelID);
        self.loadDropDowns(true).done(function () {
            if (Clinical_Provider_Note_Template.isSuperAdmin()) {
                Clinical_Provider_Note_Template.enableDisableDropDownLists('ddlNotesTemplateSpecialty,ddlNotesTemplateProvider', true);
            } else {
                Clinical_Provider_Note_Template.enableDisableDropDownLists('ddlNotesTemplateSpecialty,ddlNotesTemplateProvider', false);
            }
            Clinical_Provider_Note_Template.isEntitySelected(Clinical_Provider_Note_Template.isSuperAdmin());
            Clinical_Provider_Note_Template.notesTemplateSearch();
        });
    },
    //Start//03/15/2016//M Ahmad Imran//Implimented "IntializeMultiSelectDropDown" which intialize all multi select dropdowns
    IntializeMultiSelectDropDown: function () {


        $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 116

        });
        $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 116

        });

    },
    //End M Ahmad Imran 03/15/2016
    //Start//03/15/2016//M Ahmad Imran//Implimented "AddProviderNoteTemplate" which open provider note template page in popup 
    AddProviderNoteTemplate: function () {
        var params = [];
        params["ParentCtrl"] = "Clinical_Provider_Note_Template";
        params["FromAdmin"] = 0;
        params["mode"] = "Add";
        LoadActionPan("Clinical_Add_Provider_Note_Template", params);
    },
    //End M Ahmad Imran 03/15/2016
    UnLoadTab: function () {
        var objDeffered = $.Deferred();

        if (Clinical_Provider_Note_Template.params["FromAdmin"] == "0") {
            if (Clinical_Provider_Note_Template.params != null && Clinical_Provider_Note_Template.params.ParentCtrl != null) {
                UnloadActionPan(Clinical_Provider_Note_Template.params.ParentCtrl, 'Clinical_Provider_Note_Template');
            }
            else
                UnloadActionPan(null, 'Clinical_Provider_Note_Template');
        }
        else {

            RemoveAdminTab();
        }
        objDeffered.resolve();
        return objDeffered;
    },
    //Start//03/16/2016//M Ahmad Imran//Implimented "enableDisableDropDownLists" If an Entity is selected then enable Speciality and Provider DDL's

    isEntitySelected: function (isSuperAdmin) {
        var objDeffered = $.Deferred();
        selectedEntity = $('#ddlEntity option:selected').val();
        if (isSuperAdmin == false) {
            selectedEntity = globalAppdata["SeletedEntityId"];
        }
        if (selectedEntity == null || selectedEntity == '') {
            $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty').val('');
            $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateProvider').val('');
            $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty').multiselect('refresh');
            $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateProvider').multiselect('refresh');
            Clinical_Provider_Note_Template.enableDisableDropDownLists('ddlNotesTemplateProvider', true);
            Clinical_Provider_Note_Template.enableDisableDropDownLists('ddlNotesTemplateSpecialty', true);

            objDeffered.resolve();
        } else {
            $.when(Clinical_Provider_Note_Template.loadEntitySpecialty(selectedEntity)).then(function () {
                Clinical_Provider_Note_Template.IntializeMultiSelectDropDownSpecialties();
                $.when(Clinical_Provider_Note_Template.loadEntityProvider(selectedEntity)).then(function () {

                    Clinical_Provider_Note_Template.IntializeMultiSelectDropDownProviders();

                    objDeffered.resolve();

                });
            });
        }
        return objDeffered;
    },
    //Start//03/16/2016//M Ahmad Imran//Implimented "loadEntitySpecialty" This function will load entity based specialty

    loadEntitySpecialty: function (entityID) {

        // Loads Spacialties Based on entityId
        if (entityID != null && entityID > 0) {

            providerDetail.FillSpecialty(entityID).done(function (response) {
                if (response.status != false) {

                    var spacialties = JSON.parse(response.SpecialtyLoad_JSON);
                    $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty').empty();

                    $.each(spacialties, function (i, item) {
                        $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty').append(
                            $('<option/>', {
                                value: item.SpecialtyId,
                                html: item.ShortName
                            })
                        );
                    });

                    //Assign server side spacialties to the specialityCheckedIds array
                    if (Clinical_Provider_Note_Template.SpecialtyIds != '') {

                        var Specialties = Clinical_Provider_Note_Template.SpecialtyIds.split(",");
                        Clinical_Provider_Note_Template.specialityCheckedIds = Specialties;
                        $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty').val(Specialties);
                    }
                }

            }).then(function () {
                Clinical_Provider_Note_Template.IntializeMultiSelectDropDownSpecialties();
                //enable dropdownlist
                Clinical_Provider_Note_Template.enableDisableDropDownLists('ddlNotesTemplateSpecialty', false);
            });
        }
        else {
            //Disable dropdownlist
            Clinical_Provider_Note_Template.enableDisableDropDownLists('ddlNotesTemplateSpecialty', true);
        }
    },

    //Start//03/16/2016//M Ahmad Imran//Implimented "IntializeMultiSelectDropDownSpecialties" This function will initialize Specialty multiselect ddl

    IntializeMultiSelectDropDownSpecialties: function () {
        $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty').multiselect('destroy');
        $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: false,
            onChange: function (option, checked, select) {

                Clinical_Provider_Note_Template.checkProvidersBySpecialityIds(option, checked, select);

            },
            onDropdownHide: function (event) {
                $.when(
                    Clinical_Provider_Note_Template.filterProvidersBySpecialtyIds()
                ).then(function () {
                    if (Clinical_Provider_Note_Template.ProviderIds != '') {
                        var Providers = Clinical_Provider_Note_Template.ProviderIds.split(",");

                        if (Providers != '' && typeof Providers != 'undefined') {

                            $.each(Providers, function (index, item) {
                                Clinical_Provider_Note_Template.providerCheckedIds = Clinical_Provider_Note_Template.removeFromArray(Clinical_Provider_Note_Template.providerCheckedIds, item);
                                Clinical_Provider_Note_Template.providerCheckedIds.push(item);
                            });
                        }
                    }
                    $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateProvider').val(Clinical_Provider_Note_Template.providerCheckedIds);
                    Clinical_Provider_Note_Template.IntializeMultiSelectDropDownProviders();
                });
            },

            onDropdownShow: function (event) {
                //make items selected and initialize dropdownlist
                if (Clinical_Provider_Note_Template.SpecialtyIds != '') {
                    var spacialties = Clinical_Provider_Note_Template.SpecialtyIds.split(",");

                    if (spacialties != '' && typeof spacialties != 'undefined') {

                        $.each(spacialties, function (index, item) {
                            Clinical_Provider_Note_Template.specialityCheckedIds = Clinical_Provider_Note_Template.removeFromArray(Clinical_Provider_Note_Template.specialityCheckedIds, item);
                            Clinical_Provider_Note_Template.specialityCheckedIds.push(item);
                        });
                    }
                }
                Clinical_Provider_Note_Template.setSpacialtiesByselectedProviderIds();
                $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty').multiselect('select', Clinical_Provider_Note_Template.specialityCheckedIds);
            },
        });
    },

    //Start//03/16/2016//M Ahmad Imran//Implimented "setSpacialtiesByselectedProviderIds" This function will set specialty Ids in specailChekedIds array
    setSpacialtiesByselectedProviderIds: function () {

        $.each(Clinical_Provider_Note_Template.providerCheckedIds, function (index, item) {

            $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateProvider option').each(function () {
                if ($(this).val() != '') {
                    if ($(this).val() == item) {

                        Clinical_Provider_Note_Template.specialityCheckedIds = Clinical_Provider_Note_Template.removeFromArray(Clinical_Provider_Note_Template.specialityCheckedIds, $(this).attr('refname'));
                        Clinical_Provider_Note_Template.specialityCheckedIds.push($(this).attr('refname'));
                    }
                }
            });
        });
    },
    //Start//03/16/2016//M Ahmad Imran//Implimented "filterProvidersBySpecialtyIds"  This function will save spacialty ids and will show privders on spacialty selection
    filterProvidersBySpecialtyIds: function () {

        var providerHiddenContext = '#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlHiddenNotesTemplateProvider';

        var providerContext = '#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateProvider';
        $(providerContext).empty();

        if (Clinical_Provider_Note_Template.specialityCheckedIds.length > 0) {

            $.each(Clinical_Provider_Note_Template.specialityCheckedIds, function (index, specialtyId) {

                $(providerHiddenContext).find('option').each(function (index, option) {
                    if ($(option).attr('refname') == specialtyId) {
                        $(providerContext).append(
                         $('<option/>', {
                             value: $(option).val(),
                             html: $(option).html(),
                             refname: $(option).attr('refname'),
                             refvalue: $(option).attr('refvalue')

                         }));
                    }
                });
            });
        }
        else {
            $(providerHiddenContext).find('option').each(function (index, option) {
                $(providerContext).append(
                         $('<option/>', {
                             value: $(option).val(),
                             html: $(option).html(),
                             refname: $(option).attr('refname'),
                             refvalue: $(option).attr('refvalue')

                         }));
            });
        }
    },
    //Start//03/16/2016//M Ahmad Imran//Implimented "checkProvidersBySpecialityIds" This function will save spacialty ids and will show privders on spacialty selection
    checkProvidersBySpecialityIds: function (option, checked, select) {
        //specialty context
        var specialtyContext = '#' + Clinical_Provider_Note_Template.params.PanelID + ' #divNotesTemplateSpecialty';
        var isAllSpecialtySelected = $(specialtyContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var specialtyItems = $(specialtyContext).find('.dropdown-menu').find('li').length;
        var checkedSpecialtyItems = $(specialtyContext).find('.dropdown-menu').find('li.active').length;

        if (checkedSpecialtyItems <= 0) {
            Clinical_Provider_Note_Template.specialityCheckedIds = [];
            Clinical_Provider_Note_Template.providerCheckedIds = [];
            Clinical_Provider_Note_Template.ProviderIds = '';
            Clinical_Provider_Note_Template.SpecialtyIds = '';
        }
        else {
            if (!isAllSpecialtySelected && !(specialtyItems == checkedSpecialtyItems)) {


                var spacialityId = $(option).attr("value");
                if (checked && spacialityId != "") {
                    Clinical_Provider_Note_Template.specialityCheckedIds = Clinical_Provider_Note_Template.removeFromArray(Clinical_Provider_Note_Template.specialityCheckedIds, spacialityId);
                    Clinical_Provider_Note_Template.specialityCheckedIds.push(spacialityId);
                }
                else {

                    Clinical_Provider_Note_Template.specialityCheckedIds = Clinical_Provider_Note_Template.removeFromArray(Clinical_Provider_Note_Template.specialityCheckedIds, spacialityId);

                }


            }
            else {

                Clinical_Provider_Note_Template.specialityCheckedIds = [];
                $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty option').each(function () {
                    var spacialityId = $(this).attr("value");
                    Clinical_Provider_Note_Template.specialityCheckedIds.push(spacialityId);
                });

            }
        }
    },
    //Start//03/16/2016//M Ahmad Imran//Implimented "removeFromArray" This function will remove item from the "array and item" provided as input args
    removeFromArray: function (array, removeItem) {

        var resultantArray = jQuery.grep(array, function (item) {
            return item != removeItem;
        });
        return resultantArray;
    },
    //Start//03/16/2016//M Ahmad Imran//Implimented "providersBySpecialityIds" This function will load providers by specialities ids checked
    providersBySpecialityIds: function () {

        var providerContext = '#' + Clinical_Provider_Note_Template.params.PanelID + ' #providerMultiList';
        //find match in both mutiselect and show it
        $(providerContext).find('.dropdown-menu').find('li').each(function () {

            var $li = $(this);
            var isExists = false;
            var value = $li.find('input').val();
            var ddlId = $(providerContext).find('select').attr('id');

            $.each(Clinical_Provider_Note_Template.specialityCheckedIds, function (index, specialtyId) {

                if (value != "") {
                    var refName = Clinical_Provider_Note_Template.getRefValuefromDdl(ddlId, value);

                    // if true then show and check the provider
                    if (specialtyId == refName) {
                        isExists = true;
                        return false;
                    }
                }
            });
            if (Clinical_Provider_Note_Template.specialityCheckedIds.length > 0) {
                if (!isExists) {
                    Clinical_Provider_Note_Template.showHideMultiSelectDdlOptions(true, ddlId, $li, value, 'provider');
                }
                else {
                    Clinical_Provider_Note_Template.showHideMultiSelectDdlOptions(false, ddlId, $li, value, 'provider');
                }
            }
            else {
                Clinical_Provider_Note_Template.showHideMultiSelectDdlOptions(false, ddlId, $li, value, 'provider', "unCheckAll");
            }
        });
    },
    //Start//03/16/2016//M Ahmad Imran//Implimented "getRefValuefromDdl" This function will return refname using (li's input value equals ddl option value)
    getRefValuefromDdl: function (ddlId, liId) {
        var $ddlOptions = $('#' + Clinical_Provider_Note_Template.params.PanelID + " #" + ddlId).find('option');
        var value = null;
        $ddlOptions.each(function () {

            if ($(this).attr('value') == liId) {
                value = $(this).attr('refname');
                return false;
            }
        });
        return value;
    },
    //Start//03/16/2016//M Ahmad Imran//Implimented "showHideMultiSelectDdlOptions"  This function will show hide the mutiselect li and ddl option
    showHideMultiSelectDdlOptions: function (isHide, ddlId, $multiselectLi, ddlOptionValue, calledBy, checkboxStatus) {
        if (isHide) {
            //populating provider
            if (calledBy.toLowerCase() == "provider") {
                if (!$multiselectLi.hasClass('filter') && !$multiselectLi.hasClass('multiselect-all')) {

                    $('#' + Clinical_Provider_Note_Template.params.PanelID + " #" + ddlId).find('option[value="' + ddlOptionValue + '"]').prop('disabled', true);
                    $multiselectLi.prop('disabled', true);
                    $multiselectLi.hide();

                }
            }
                //populating speciality
            else if (calledBy.toLowerCase() == "specialty") {
                // do not hide anything
            }
        }
        else {

            $multiselectLi.show();
            // if specialty the mark it as checked
            if (calledBy.toLowerCase() == "specialty") {

                if (!$multiselectLi.hasClass('active')) {
                    $multiselectLi.find('input').trigger('click');
                }
            }
            if (typeof checkboxStatus != 'undefined') {
                if (calledBy.toLowerCase() == "provider" && checkboxStatus.toLowerCase() == 'uncheckall') {

                    if ($multiselectLi.hasClass('active')) {
                        $multiselectLi.find('input').trigger('click');
                    }
                }
            }
            //show the li
            $multiselectLi.prop('disabled', false);
            $('#' + Clinical_Provider_Note_Template.params.PanelID + " #" + ddlId).find('option[value="' + ddlOptionValue + '"]').prop('disabled', false);

        }
    },
    //Start//03/16/2016//M Ahmad Imran//Implimented "loadEntityProvider" This function will load entity based provider
    loadEntityProvider: function (entityId) {

        var data = "entityID=" + entityId;
        if (entityId != null && entityId > 0) {

            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateProvider');
                var $providerHiddenDdl = $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlHiddenNotesTemplateProvider');

                //Empty both the providers ddls.
                $providerDdl.empty();
                $providerHiddenDdl.empty();

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
                        $providerHiddenDdl.append(
                             $('<option/>', {
                                 value: item.Value,
                                 html: item.Name,
                                 refname: item.RefName,
                                 refvalue: item.RefValue

                             })
                        );
                    }
                });
                // Assigned server side providers to providerCheckedIds array and made selected
                if (Clinical_Provider_Note_Template.ProviderIds != '') {
                    var Providers = Clinical_Provider_Note_Template.ProviderIds.split(",");
                    Clinical_Provider_Note_Template.providerCheckedIds = Providers;
                    $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateProvider').val(Providers);
                }

            }).then(function () {
                // A hack to trigger the onDropDownHide event of Spacialty multiselect      
                $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #divNotesTemplateSpecialty').find('.dropdown-toggle').dropdown('toggle').dropdown('toggle');
                //Intialized in onhidden spacialty ddl.  
                Clinical_Provider_Note_Template.IntializeMultiSelectDropDownProviders();

            });
            //enable multiselect
            Clinical_Provider_Note_Template.enableDisableDropDownLists('ddlNotesTemplateProvider', false);
        }
        else {
            //disable multiselect
            Clinical_Provider_Note_Template.enableDisableDropDownLists('ddlNotesTemplateProvider', true);
        }
    },
    //Start//03/16/2016//M Ahmad Imran//Implimented "IntializeMultiSelectDropDownProviders" This function will initialize provider multiselect ddl
    IntializeMultiSelectDropDownProviders: function () {
        $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateProvider').multiselect('destroy');
        $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: false,
            onChange: function (option, checked, select) {
                Clinical_Provider_Note_Template.checkSpecialtiesByProviderId(option, checked, select);
            },
            onDropdownHide: function (event) {
                // Clinical_Provider_Note_Template.specialitiesByProviderIds();
                //Refresh multiselect
                //  $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty').multiselect('refresh');
            },


        });
    },
    //Start//03/16/2016//M Ahmad Imran//Implimented "specialitiesByProviderIds" This function will load check spcialities by provider ids checked
    specialitiesByProviderIds: function () {
        //specialty context
        var specialtyContext = '#' + Clinical_Provider_Note_Template.params.PanelID + ' #specialityMultiList';
        var ddlId = $(specialtyContext).find('select').attr('id');
        //find match in both mutiselect and show it
        $(specialtyContext).find('.dropdown-menu').find('li').each(function () {

            var $li = $(this);
            //remove previous selected Data
            //if ($li.hasClass('active')) {
            //    $li.removeClass('active');
            //}
            //load selected Data by spacialty Ids
            var isExists = false;
            var value = $li.find('input').val();

            $.each(Clinical_Provider_Note_Template.providerCheckedIds, function (index, providerId) {

                if (value != "") {
                    if (providerId == value) {
                        isExists = true;
                        return false;
                    }
                }
            });
            if (Clinical_Provider_Note_Template.providerCheckedIds.length > 0) {
                if (!isExists) {
                    Clinical_Provider_Note_Template.showHideMultiSelectDdlOptions(true, ddlId, $li, value, 'specialty');
                }
                else {
                    Clinical_Provider_Note_Template.showHideMultiSelectDdlOptions(false, ddlId, $li, value, 'specialty');
                }
            }
            else {
                Clinical_Provider_Note_Template.showHideMultiSelectDdlOptions(true, ddlId, $li, value, 'specialty');
            }

        });
    },
    //Start//03/16/2016//M Ahmad Imran//Implimented "enableDisableDropDownLists" If user is Super Admin show the Entity dropdown otherwise don't

    isSuperAdmin: function () {
        if (globalAppdata['AppUserName'] == DefaultUser) {
            $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #entityDDL').show();
            return true;
        } else {
            $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #entityDDL').hide();
            return false;
        }
    },
    //Start//03/16/2016//M Ahmad Imran//Implimented "enableDisableDropDownLists" which will enable disable multiselect ddls provided
    enableDisableDropDownLists: function (ddlIds, isHide) {
        ddlCommaSeparatedIds = ddlIds.split(',');
        var parrentPanelId = "#" + Clinical_Provider_Note_Template.params["PanelID"];
        $.each(ddlCommaSeparatedIds, function (index, Item) {
            if (isHide) {
                $(parrentPanelId + " #" + Item).multiselect('disable');
            }
            else {
                $(parrentPanelId + " #" + Item).multiselect('enable');
            }
        });
    },
    //End M Ahmad Imran 03/16/2016
    //Start//03/15/2016//M Ahmad Imran//Implimented "IntializeMultiSelectDropDown" which intialize all multi select dropdowns
    IntializeMultiSelectDropDownSpecialties: function () {
        $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty').multiselect('destroy');
        $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: false,
            onChange: function (option, checked, select) {

                Clinical_Provider_Note_Template.checkProvidersBySpecialityIds(option, checked, select);

            },
            onDropdownHide: function (event) {
                $.when(
                    Clinical_Provider_Note_Template.filterProvidersBySpecialtyIds()
                ).then(function () {
                    if (Clinical_Provider_Note_Template.ProviderIds != '') {
                        var Providers = Clinical_Provider_Note_Template.ProviderIds.split(",");

                        if (Providers != '' && typeof Providers != 'undefined') {

                            $.each(Providers, function (index, item) {
                                Clinical_Provider_Note_Template.providerCheckedIds = Clinical_Provider_Note_Template.removeFromArray(Clinical_Provider_Note_Template.providerCheckedIds, item);
                                Clinical_Provider_Note_Template.providerCheckedIds.push(item);
                            });
                        }
                    }
                    $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateProvider').val(Clinical_Provider_Note_Template.providerCheckedIds);
                    Clinical_Provider_Note_Template.IntializeMultiSelectDropDownProviders();
                });
            },

            onDropdownShow: function (event) {
                //make items selected and initialize dropdownlist
                if (Clinical_Provider_Note_Template.SpecialtyIds != '') {
                    var spacialties = Clinical_Provider_Note_Template.SpecialtyIds.split(",");

                    if (spacialties != '' && typeof spacialties != 'undefined') {

                        $.each(spacialties, function (index, item) {
                            Clinical_Provider_Note_Template.specialityCheckedIds = Clinical_Provider_Note_Template.removeFromArray(Clinical_Provider_Note_Template.specialityCheckedIds, item);
                            Clinical_Provider_Note_Template.specialityCheckedIds.push(item);
                        });
                    }
                }
                Clinical_Provider_Note_Template.setSpacialtiesByselectedProviderIds();
                $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty').multiselect('select', Clinical_Provider_Note_Template.specialityCheckedIds);
            },
        });
    },
    //End M Ahmad Imran 03/15/2016
    //Start//03/15/2016//M Ahmad Imran//Implimented "checkSpecialtiesByProviderId"  This function will save privder ids and will check speciality on provider selection
    checkSpecialtiesByProviderId: function (option, checked, select) {

        //provider context
        var providerContext = '#' + Clinical_Provider_Note_Template.params.PanelID + ' #divNotesTemplateProvider';
        var isAllProviderSelected = $(providerContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var providerItems = $(providerContext).find('.dropdown-menu').find('li').length;
        var checkedProviderItems = $(providerContext).find('.dropdown-menu').find('li.active').length;

        if (checkedProviderItems <= 0) {
            Clinical_Provider_Note_Template.providerCheckedIds = [];
            Clinical_Provider_Note_Template.ProviderIds = '';
        }
            //push all provider checked items
        else if (isAllProviderSelected) {
            Clinical_Provider_Note_Template.providerCheckedIds = [];
            $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateProvider option').each(function () {
                var providerValue = $(this).val();
                Clinical_Provider_Note_Template.providerCheckedIds.push(providerValue);
            });
        }
        else {
            // provider value
            var providerValue = $(option).val();

            // add to provider array if checked
            if (checked) {
                Clinical_Provider_Note_Template.providerCheckedIds = Clinical_Provider_Note_Template.removeFromArray(Clinical_Provider_Note_Template.providerCheckedIds, providerValue);
                Clinical_Provider_Note_Template.providerCheckedIds.push(providerValue);
            }
                //delete from provider array if not checked
            else {
                Clinical_Provider_Note_Template.providerCheckedIds = Clinical_Provider_Note_Template.removeFromArray(Clinical_Provider_Note_Template.providerCheckedIds, $(option).val());
            }

        }
    },

    //******************************* Azhar Written Functions*************************
    ReviewOfSystemsTemplateAddEdit: function (NotesTemplateId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Clinical_Template_Provider Note Template", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                if (NotesTemplateId != null && parseInt(NotesTemplateId) > 0) {
                    params["NotesTemplateId"] = NotesTemplateId;
                    params["mode"] = "Edit";
                }
                else {
                    params["NotesTemplateId"] = -1;
                    params["mode"] = "Add";
                }
                params["FromAdmin"] = Clinical_Provider_Note_Template.params["FromAdmin"];
                params["ParentCtrl"] = 'adminTabReviewOfSystemsTemplate';
                LoadActionPan('ReviewOfSystemsTemplateDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    /*
    Author: Muhammad Azhar Shahzad
    Purpose: for Grid Load of Notes template
    Creation Date: March 02,2016 */
    notesTemplateSearch: function (NotesTemplateId, PageNo, rpp) {

        Clinical_Provider_Note_Template.searchNotesTemplate_DBCall(NotesTemplateId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_Provider_Note_Template.notesTemplateGridLoad(response);
                //Adding Pagination on 04 Dec 2015 by Azhar
                var TableControl = Clinical_Provider_Note_Template.params.PanelID + " #dgvNotesTemplate";
                var PagingPanelControlID = Clinical_Provider_Note_Template.params.PanelID + " #dgvNotesTemplate_Paging";
                var ClassControlName = "Clinical_Provider_Note_Template";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.NotesTemplateCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                    Clinical_Provider_Note_Template.notesTemplateSearch(PrimaryID, PageNumber, ResultPerPage);
                }), 10);

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    /*
   Author: Muhammad Azhar Shahzad
   Purpose: for Grid Load of Notes template rows html
   Creation Date: March 02,2016 */
    notesTemplateGridLoad: function (response) {
        var isactive = $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #pnlNoteTemplate_Result #divSwitch #switchActive').attr('isactive');
        if ($.fn.dataTable.isDataTable('#' + Clinical_Provider_Note_Template.params.PanelID + " #dgvNotesTemplate")) {
            $('#' + Clinical_Provider_Note_Template.params.PanelID + " #dgvNotesTemplate").dataTable().fnDestroy();
            $('#' + Clinical_Provider_Note_Template.params.PanelID + " #dgvNotesTemplate tbody").find("tr").remove();
        }

        if (response.NotesTemplateCount > 0) {
            var notesTemplateLoadJSONData = JSON.parse(response.NotesTemplateLoad_JSON);
            $.each(notesTemplateLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvnotesTemplate_row" + item.NotesTemplateId + "'))");
                $row.attr("id", "gvnotesTemplate_row" + item.NotesTemplateId);
                $row.attr("NotesTemplateId", item.NotesTemplateId);
                $row.attr("Active", item.IsActive);

                if (item.IsActive == 1) {
                    isactive = 1;
                    isEventactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 0;
                    isEventactive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }

                var Isdisabled = "";
                if (item.IsDefault == "True" || item.NoteTemplateName == "Default Template") {
                    globalAppdata["AppUserName"].toLowerCase() == "mdvision" ? Isdisabled = "" : Isdisabled = "disabled =true";
                    if (globalAppdata["AppUserName"].toLowerCase() == "mdvision") {
                        $row.attr("onclick", "Clinical_Provider_Note_Template.notesTemplateRowEdit(" + item.NotesTemplateId + ", event);");
                    }
                } else {
                    $row.attr("onclick", "Clinical_Provider_Note_Template.notesTemplateRowEdit(" + item.NotesTemplateId + ", event);");
                }

                $row.append('<td>' +
                    '<a ' + Isdisabled + ' class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_Provider_Note_Template.notesTemplateDelete(' + item.NotesTemplateId + ((item.NotesId != "" && item.NotesId != null) ? ',' + item.NotesId : '') + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;' +
                    '<a ' + Isdisabled + ' class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_Provider_Note_Template.notesTemplateEdit(' + item.NotesTemplateId + ', event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;' +
                    '<a ' + Isdisabled + ' class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_Provider_Note_Template.notesTemplateActiveInactive(' + item.NotesTemplateId + ', ' + isEventactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass +
                    '"></i></a>&nbsp;</td>' +
                    '<td>' + item.NoteTemplateName + '</td><td>' + item.SpecialtyNames + '</td><td>' + item.TemplateType + '</td><td>' + item.ModifiedOn.replace(/(.*)\D\d+/, '$1') + ' ' + item.ModifiedByName + '</td>');
                if (item.NoteTemplateName == "Default Template") {
                    $('#' + Clinical_Provider_Note_Template.params.PanelID + " #dgvNotesTemplate tbody").prepend($row);
                } else {
                    $('#' + Clinical_Provider_Note_Template.params.PanelID + " #dgvNotesTemplate tbody").last().append($row);
                }
            });
            var checked = '';
            if (isactive == "0" || isactive == 0) {
                isactive = "0";
            } else if (isactive == null) {
                isactive = "1";
                checked = 'checked="checked"';
            } else {
                isactive = "1";
                checked = 'checked="checked"';
            }
            if ($.fn.dataTable.isDataTable('#' + Clinical_Provider_Note_Template.params.PanelID + " #dgvNotesTemplate"))
                ;
            else {
                $('#' + Clinical_Provider_Note_Template.params.PanelID + " #dgvNotesTemplate").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown

            }
            var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                        '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="Clinical_Provider_Note_Template.activenotesTemplateSearch(this);">' +
                         '</div><span class="pl-xs">Active</span>' +
         '<a id="btnNoKnownProblems" class="btn btn-link btn-xs" style="display:none" onclick="Clinical_ProblemLists.NoKnownProblem();">No Known Problems</a>';

            $("#" + Clinical_Provider_Note_Template.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        }
        else {
            if (isactive == null) {
                isactive = "1";
                checked = 'checked="checked"';
            } else if (isactive == "1" || isactive == 1) {
                isactive = "1";
                checked = 'checked="checked"';
            } else {
                isactive = "0";
                checked = '';
            }
            $('#' + Clinical_Provider_Note_Template.params.PanelID + " #dgvNotesTemplate").DataTable({
                "language": {
                    "emptyTable": "No Template is Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            });
            var HtmlOfSwitch = '<span class="pr-xs">Inactive</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                      '<input id="switchActive" isactive="' + isactive + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="Clinical_Provider_Note_Template.activenotesTemplateSearch(this);">' +
                       '</div><span class="pl-xs">Active</span>' +
       '<a id="btnNoKnownProblems" class="btn btn-link btn-xs" style="display:none" onclick="Clinical_ProblemLists.NoKnownProblem();">No Known Problems</a>';

            $("#" + Clinical_Provider_Note_Template.params.PanelID + ' .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitch + '</div>');
        }

        EMRUtility.SwicthWidgetInializatoin();

    },
    /*
  Author: Muhammad Azhar Shahzad
  Purpose: QAC2-590 fix
  Creation Date: October 20,2016 */
    notesTemplateRowEdit: function (NotesTemplateId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        if ((event.srcElement instanceof HTMLAnchorElement || event.srcElement.nodeName.toLowerCase() == 'i') != true) {
            Clinical_Provider_Note_Template.notesTemplateEdit(NotesTemplateId, event);
        }
    },
    /*
   Author: Muhammad Azhar Shahzad
   Purpose: for Grid Load of Notes template for active/ inactive records 
   Creation Date: March 02,2016 */
    activenotesTemplateSearch: function (objThis) {
        var isactive = $(objThis).attr('isactive');
        if (isactive == '1') {
            $(objThis).attr('isactive', '0');
        }
        else if (isactive == '0') {
            $(objThis).attr('isactive', '1');
        }
        Clinical_Provider_Note_Template.notesTemplateSearch();
    },

    /*
   Author: Muhammad Azhar Shahzad
   Purpose: for Grid Load of Notes template to delete records
   Creation Date: March 02,2016 */
    notesTemplateDelete: function (NotesTemplateId, NotesId) {
        if (NotesId != null && NotesId != "") {
            utility.DisplayMessages('This template is currently associated with Provider Notes and cannot be deleted.', 3);
        } else {
            utility.myConfirm('30', function () {
                if (NotesTemplateId > 0) {
                    Clinical_Provider_Note_Template.notesTemplateDelete_DbCall(NotesTemplateId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Clinical_Provider_Note_Template.notesTemplateSearch();
                           
                            utility.DisplayMessages(response.Message, 1);
                        }
                        else {

                            var ResponseMessage;
                            if (response.Message == "Provider Template is associated with Progress Note") {
                                ResponseMessage = "Patient Reference Data Exists.";
                            }
                            else {
                                ResponseMessage = response.Message;
                            }

                            utility.DisplayMessages(ResponseMessage, 3);
                        }
                    });
                }
            });
        }
    },

    /*
   Author: Muhammad Azhar Shahzad
   Purpose: for Grid Load of Notes template for edit records
   Creation Date: March 02,2016 */
    notesTemplateEdit: function (NotesTemplateId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Clinical_Template_Provider Note Template", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                if (NotesTemplateId == "" || NotesTemplateId == "undefined") {
                }
                else {
                    var params = [];
                    params["NotesTemplateId"] = NotesTemplateId;
                    params["mode"] = "Edit";
                    params["FromAdmin"] = 0;
                    LoadActionPan('Clinical_Add_Provider_Note_Template', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    /*
   Author: Muhammad Azhar Shahzad
   Purpose: to change active / in active records of Grid of Notes template 
   Creation Date: March 02,2016 */
    notesTemplateActiveInactive: function (NotesTemplateId, IsActive, event) {
        utility.myConfirm('3', function () {
            if (NotesTemplateId == "" || NotesTemplateId == "undefined") {
            }
            else {
                Clinical_Provider_Note_Template.updatenotesTemplateActiveInactive_Dbcall(NotesTemplateId, IsActive).done(function (response) {
                    if (response.status != false) {
                        //Start || 14 July, 2016 || ZeeshanAK || Fix for EMR-1518
                        var response = JSON.parse(response);
                        //End   || 14 July, 2016 || ZeeshanAK || Fix for EMR-1518
                        utility.DisplayMessages(response.Message, 1);
                        Clinical_Provider_Note_Template.notesTemplateSearch();
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

    //--------------------------------- DbCall Functions of Notes Template  start----------------------
    /*
   Author: Muhammad Azhar Shahzad
   Purpose: for Grid Load of Notes template
   Creation Date: March 02,2016 */
    searchNotesTemplate_DBCall: function (NotesTemplateId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var IsActive = null;
        IsActive = $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #pnlNoteTemplate_Result #divSwitch #switchActive').attr('isactive');
        if (IsActive == null) {
            IsActive = "1";
        }
        var self = $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #frmClinicalProviderNoteTemplate')
        var objData = self != null ? self.getMyJSONByName() : "{}";
        objData = JSON.parse(objData);
        objData["NotesTemplateId"] = NotesTemplateId == null ? -1 : NotesTemplateId;
        objData.TemplateTypeId= objData.TemplateTypeId == '' ? -1 : objData.TemplateTypeId;
        objData["PageNumber"] = PageNumber;
        objData["IsActive"] = IsActive;
        objData["RowsPerPage"] = RowsPerPage;
        if (globalAppdata['AppUserName'] == DefaultUser) {
            objData["EntityId"] = $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlEntity').val();
        } else {
            objData["EntityId"] = globalAppdata["SeletedEntityId"];
        }
        objData["commandType"] = "SEARCH_NOTES_TEMPLATE";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "TemplateBuilder", "ProviderNoteTemplate");
    },

    /*
   Author: Muhammad Azhar Shahzad
   Purpose: for Grid Load of Notes template to delete records
   Creation Date: March 02,2016 */
    notesTemplateDelete_DbCall: function (NotesTemplateId) {
        var objData = {};
        objData["NotesTemplateId"] = NotesTemplateId;
        objData["commandType"] = "DELETE_CLINICAL_NOTES_TEMPLATE";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class 
        return MDVisionService.APIService(data, "TemplateBuilder", "ProviderNoteTemplate");
    },

    /*
   Author: Muhammad Azhar Shahzad
   Purpose: to change active / in active records of Grid of Notes template 
   Creation Date: March 02,2016 */
    updatenotesTemplateActiveInactive_Dbcall: function (NotesTemplateId, IsActive) {
        var objData = {};
        objData["NotesTemplateId"] = NotesTemplateId;
        objData["IsActive"] = IsActive;

        objData["commandType"] = "UPDATE_CLINICAL_NOTES_TEMPLATE_ACTIVE_INACTIVE";
        var data = JSON.stringify(objData);
        //  var data = "NotesId=" + NotesId + "&IsActive=" + IsActive;
        // sNotesch parameter , class name, command name of class 
        return MDVisionService.APIService(data, "TemplateBuilder", "ProviderNoteTemplate");
    }

    //--------------------------------- DbCall Functions of Notes Template  END----------------------
}