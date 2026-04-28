// Created By:  Muhammad Ahmad Imran
// Created Date: 3/15/2016
Clinical_Add_Provider_Note_Template = {
    bIsFirstLoad: true,
    params: [],
    imageSize: 0,
    TemplateContent: "",
    Iferror: false,
    specialityCheckedIds: [],
    providerCheckedIds: [],
    SpecialtyIds: '',
    ProviderIds: '',
    Load: function (params) {
        Clinical_Add_Provider_Note_Template.params = params;

        if (Clinical_Add_Provider_Note_Template.params.PanelID != 'pnlClinicalAddProviderNoteTemplate') {
            Clinical_Add_Provider_Note_Template.params.PanelID = Clinical_Add_Provider_Note_Template.params.PanelID + ' #pnlClinicalAddProviderNoteTemplate';
        } else {
            Clinical_Add_Provider_Note_Template.params.PanelID = 'pnlClinicalAddProviderNoteTemplate';
        }
        Clinical_Add_Provider_Note_Template.specialityCheckedIds = [];
        Clinical_Add_Provider_Note_Template.providerCheckedIds = [];
        Clinical_Add_Provider_Note_Template.SpecialtyIds = "";
        Clinical_Add_Provider_Note_Template.ProviderIds = "";

        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Clinical_Add_Provider_Note_Template.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }

        if (globalAppdata['AppUserName'] == DefaultUser) {
            $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #frmClinicalAddProviderNoteTemplate #divEntity').show();
        }

        var self = $('#' + Clinical_Add_Provider_Note_Template.params.PanelID);

        self.loadDropDowns(true).done(function () {

            if (Clinical_Add_Provider_Note_Template.isSuperAdmin()) {
                Clinical_Add_Provider_Note_Template.enableDisableDropDownLists('ddlNotesTemplateSpecialty,ddlNotesTemplateProvider', true);
            } else {
                Clinical_Add_Provider_Note_Template.enableDisableDropDownLists('ddlNotesTemplateSpecialty,ddlNotesTemplateProvider', false);
            }

            if (Clinical_Add_Provider_Note_Template.params.mode == "Add") {
                Clinical_Add_Provider_Note_Template.isEntitySelected(Clinical_Add_Provider_Note_Template.isSuperAdmin());
                //serialize data
                //$('#frmClinicalAddProviderNoteTemplate').data('serialize', $('#frmClinicalAddProviderNoteTemplate').serialize());
                $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #frmClinicalAddProviderNoteTemplate').data('serialize', $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #frmClinicalAddProviderNoteTemplate').serialize());
                setTimeout(function () {
                    Clinical_Add_Provider_Note_Template.TemplateContent = tinyMCE.activeEditor.getContent();
                    $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #frmClinicalAddProviderNoteTemplate').data('serialize', $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #frmClinicalAddProviderNoteTemplate').serialize());
                }, 250);

            }
            else if (Clinical_Add_Provider_Note_Template.params.mode == "Edit") {
                //EMR-835 fix, Issue is fixed now, but it's now improvement, Super admin can't change entity of created Template in any case during edit operation of Provide template Notes
                if (Clinical_Add_Provider_Note_Template.isSuperAdmin()) {
                    $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlEntity').addClass('disableAll');
                }
                $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #headerId').html("Edit Provider Note Template");
                Clinical_Add_Provider_Note_Template.LoadTemplateLetter();

            }

            Clinical_Add_Provider_Note_Template.ValidateProviderNoteTemplate();

        });


    },





    //Start//3/16/2016//M Ahmad Imran//Implimented load Provider Note Template Detail
    LoadTemplateLetter: function () {

        if (Clinical_Add_Provider_Note_Template.params.mode == "Edit") {

            Clinical_Provider_Note_Template.searchNotesTemplate_DBCall(Clinical_Add_Provider_Note_Template.params.NotesTemplateId, 1, 1).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.NotesTemplateCount != 0) {
                        var self = $("#" + Clinical_Add_Provider_Note_Template.params.PanelID);
                        var contentNoteTempt = JSON.parse(response.NotesTemplateLoad_JSON);
                        if (contentNoteTempt != null && contentNoteTempt.length > 0) {
                            contentNoteTempt = contentNoteTempt[0];

                            utility.bindMyJSONByName(true, contentNoteTempt, false, self);

                            if (contentNoteTempt['IsActive'] == 1)
                                $("#" + Clinical_Add_Provider_Note_Template.params.PanelID + ' #Active').attr("checked", true);
                            else
                                $("#" + Clinical_Add_Provider_Note_Template.params.PanelID + ' #Active').attr("checked", false);
                            ////intialize TinyMCE instance on textarea control and enabled tinymce
                            if (contentNoteTempt['PEDataTemptId'] == -1) {
                                $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlPEDataTemplate').val('');
                            }
                            if (contentNoteTempt['ROSDataTemptId'] == -1) {
                                $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlROSDataTemplate').val('');
                            }
                            if (contentNoteTempt['HPITemplateId'] == -1) {
                                $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlHPITemplate').val('');
                            }

                            //set provider and speciality
                            $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlEntity').val(contentNoteTempt['EntityId']);
                            $.when(Clinical_Add_Provider_Note_Template.isEntitySelected(Clinical_Add_Provider_Note_Template.isSuperAdmin())).then(function () {
                                Clinical_Add_Provider_Note_Template.enableDisableDropDownLists('ddlNotesTemplateSpecialty,ddlNotesTemplateProvider', false);

                                $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty').multiselect('clearSelection', false);
                                $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty').multiselect('updateButtonText');
                                // Set the value
                                //EMRUtility.selectOptionsByCommaSeprateValue($('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty'), contentNoteTempt['SpecialtyIds']);
                                $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + " #ddlNotesTemplateSpecialty").val(contentNoteTempt['SpecialtyIds'].split(','));
                                // Then refresh
                                $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty').multiselect("refresh");



                                $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateProvider').multiselect('clearSelection', false);
                                $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateProvider').multiselect('updateButtonText');
                                // Set the value
                                //EMRUtility.selectOptionsByCommaSeprateValue($('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateProvider'), contentNoteTempt['ProviderIds']);
                                $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + " #ddlNotesTemplateProvider").val(contentNoteTempt['ProviderIds'].split(','));
                                // Then refresh
                                $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateProvider').multiselect("refresh");
                                $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #frmClinicalAddProviderNoteTemplate').data('serialize', $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #frmClinicalAddProviderNoteTemplate').serialize());
                                self.find('.OrderSet > select').attr('ddlist', 'GetOrderSetTemplate');
                                var data = "IsActive=1&StrID=" + contentNoteTempt['ProviderIds'].split(',').join(',');
                                self.find('.OrderSet').loadDropDowns(true, data).done(function () {
                                    $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + " #OrderSet").val(contentNoteTempt.OrderSetId);
                                });
                            });
                            EMRUtility.InitTinymceControl(false).done(function () {
                                tinymce.activeEditor.setContent(contentNoteTempt['HTMLTemplate']);// Fixes related to IMP-692
                                Clinical_Add_Provider_Note_Template.TemplateContent = contentNoteTempt['HTMLTemplate'];
                                $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #frmClinicalAddProviderNoteTemplate').data('serialize', $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #frmClinicalAddProviderNoteTemplate').serialize());
                            });
                        }
                    } else {
                        utility.DisplayMessages(response.Message, 3);
                    }


                }
                else {

                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    //Start//03/16/2016//M Ahmad Imran//Implimented "enableDisableDropDownLists" If an Entity is selected then enable Speciality and Provider DDL's

    isEntitySelected: function (isSuperAdmin) {
        var objDeffered = $.Deferred();
        selectedEntity = $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlEntity option:selected').val();
        if (isSuperAdmin == false) {
            selectedEntity = globalAppdata["SeletedEntityId"];
        }

        $.when(Clinical_Add_Provider_Note_Template.loadEntitySpecialty(selectedEntity)).then(function () {
            Clinical_Add_Provider_Note_Template.IntializeMultiSelectDropDownSpecialties();
            $.when(Clinical_Add_Provider_Note_Template.loadEntityProvider(selectedEntity)).then(function () {

                Clinical_Add_Provider_Note_Template.IntializeMultiSelectDropDownProviders();
                $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #frmClinicalAddProviderNoteTemplate').data('serialize', $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #frmClinicalAddProviderNoteTemplate').serialize());
                objDeffered.resolve();

            });
        });
        return objDeffered;
    },
    //Start//03/16/2016//M Ahmad Imran//Implimented "loadEntitySpecialty" This function will load entity based specialty

    loadEntitySpecialty: function (entityID) {
        // Loads Spacialties Based on entityId
        var objDeffered = $.Deferred();
        // Loads Spacialties Based on entityId
        if (entityID != null && entityID > 0) {

            providerDetail.FillSpecialty(entityID).done(function (response) {
                if (response.status != false) {

                    var spacialties = JSON.parse(response.SpecialtyLoad_JSON);
                    $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty').empty();

                    $.each(spacialties, function (i, item) {
                        $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty').append(
                            $('<option/>', {
                                value: item.SpecialtyId,
                                html: item.ShortName
                            })
                        );
                    });

                    //Assign server side spacialties to the specialityCheckedIds array
                    if (Clinical_Add_Provider_Note_Template.SpecialtyIds != '') {

                        var Specialties = Clinical_Add_Provider_Note_Template.SpecialtyIds.split(",");
                        Clinical_Add_Provider_Note_Template.specialityCheckedIds = Specialties;
                        $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty').val(Specialties);
                    }
                    $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #frmClinicalAddProviderNoteTemplate').data('serialize', $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #frmClinicalAddProviderNoteTemplate').serialize());
                }

            }).then(function () {
                Clinical_Add_Provider_Note_Template.IntializeMultiSelectDropDownSpecialties();
                //enable dropdownlist
                Clinical_Add_Provider_Note_Template.enableDisableDropDownLists('ddlNotesTemplateSpecialty', false);
                objDeffered.resolve();
            });
        }
        else {
            //Disable dropdownlist
            Clinical_Add_Provider_Note_Template.enableDisableDropDownLists('ddlNotesTemplateSpecialty', true);
            objDeffered.resolve();
        }
        return objDeffered;
    },

    //Start//03/16/2016//M Ahmad Imran//Implimented "IntializeMultiSelectDropDownSpecialties" This function will initialize Specialty multiselect ddl

    GetOrderSetTemplate: function () {

        var self = $('#' + Clinical_Add_Provider_Note_Template.params.PanelID);
        var Providerid = $("#" + Clinical_Add_Provider_Note_Template.params.PanelID + " #ddlNotesTemplateProvider").val();

        if (Providerid == null) {
            Providerid = "0";
        } else {
            Providerid = Providerid.join(',');
        }
        self.find('.OrderSet > select').attr('ddlist', 'GetOrderSetTemplate');
        var data = "IsActive=1&StrID=" + Providerid;
        self.find('.OrderSet').loadDropDowns(true, data).done(function () {
            self.find(".OrderSet option:contains(" + orderSetDefaultValue + ")").attr('selected', true);
        });

    },
    IntializeMultiSelectDropDownSpecialties: function () {
        $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty').multiselect('destroy');
        $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: false,
            onChange: function (option, checked, select) {

                Clinical_Add_Provider_Note_Template.checkProvidersBySpecialityIds(option, checked, select);

            },
            onDropdownHide: function (event) {
                $.when(
                    Clinical_Add_Provider_Note_Template.filterProvidersBySpecialtyIds()
                ).then(function () {
                    if (Clinical_Add_Provider_Note_Template.ProviderIds != '') {
                        var Providers = Clinical_Add_Provider_Note_Template.ProviderIds.split(",");

                        if (Providers != '' && typeof Providers != 'undefined') {

                            $.each(Providers, function (index, item) {
                                Clinical_Add_Provider_Note_Template.providerCheckedIds = Clinical_Add_Provider_Note_Template.removeFromArray(Clinical_Add_Provider_Note_Template.providerCheckedIds, item);
                                Clinical_Add_Provider_Note_Template.providerCheckedIds.push(item);
                            });
                        }
                    }
                    $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateProvider').val(Clinical_Add_Provider_Note_Template.providerCheckedIds);
                    Clinical_Add_Provider_Note_Template.IntializeMultiSelectDropDownProviders();
                });
            },

            onDropdownShow: function (event) {
                //make items selected and initialize dropdownlist
                if (Clinical_Add_Provider_Note_Template.SpecialtyIds != '') {
                    var spacialties = Clinical_Add_Provider_Note_Template.SpecialtyIds.split(",");

                    if (spacialties != '' && typeof spacialties != 'undefined') {

                        $.each(spacialties, function (index, item) {
                            Clinical_Add_Provider_Note_Template.specialityCheckedIds = Clinical_Add_Provider_Note_Template.removeFromArray(Clinical_Add_Provider_Note_Template.specialityCheckedIds, item);
                            Clinical_Add_Provider_Note_Template.specialityCheckedIds.push(item);
                        });
                    }
                }
                Clinical_Add_Provider_Note_Template.setSpacialtiesByselectedProviderIds();
                $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty').multiselect('clearSelection', false);
                $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty').multiselect('updateButtonText');
                $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty').multiselect('select', Clinical_Add_Provider_Note_Template.specialityCheckedIds);
                $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty').multiselect('refresh');
            },
        });
    },

    //Start//03/16/2016//M Ahmad Imran//Implimented "setSpacialtiesByselectedProviderIds" This function will set specialty Ids in specailChekedIds array
    setSpacialtiesByselectedProviderIds: function () {

        $.each(Clinical_Add_Provider_Note_Template.providerCheckedIds, function (index, item) {

            $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateProvider option').each(function () {
                if ($(this).val() != '') {
                    if ($(this).val() == item) {

                        Clinical_Add_Provider_Note_Template.specialityCheckedIds = Clinical_Add_Provider_Note_Template.removeFromArray(Clinical_Add_Provider_Note_Template.specialityCheckedIds, $(this).attr('refname'));
                        Clinical_Add_Provider_Note_Template.specialityCheckedIds.push($(this).attr('refname'));
                    }
                }
            });
        });
    },
    //Start//03/16/2016//M Ahmad Imran//Implimented "filterProvidersBySpecialtyIds"  This function will save spacialty ids and will show privders on spacialty selection
    filterProvidersBySpecialtyIds: function () {

        var providerHiddenContext = '#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlHiddenNotesTemplateProvider';

        var providerContext = '#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateProvider';
        $(providerContext).empty();

        if (Clinical_Add_Provider_Note_Template.specialityCheckedIds.length > 0) {

            $.each(Clinical_Add_Provider_Note_Template.specialityCheckedIds, function (index, specialtyId) {

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
        var specialtyContext = '#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #divNotesTemplateSpecialty';
        var isAllSpecialtySelected = $(specialtyContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var specialtyItems = $(specialtyContext).find('.dropdown-menu').find('li').length;
        var checkedSpecialtyItems = $(specialtyContext).find('.dropdown-menu').find('li.active').length;

        if (checkedSpecialtyItems <= 0) {
            Clinical_Add_Provider_Note_Template.specialityCheckedIds = [];
            Clinical_Add_Provider_Note_Template.providerCheckedIds = [];
            Clinical_Add_Provider_Note_Template.ProviderIds = '';
            Clinical_Add_Provider_Note_Template.SpecialtyIds = '';
        }
        else {
            if (!isAllSpecialtySelected && !(specialtyItems == checkedSpecialtyItems)) {


                var spacialityId = $(option).attr("value");
                if (checked && spacialityId != "") {
                    Clinical_Add_Provider_Note_Template.specialityCheckedIds = Clinical_Add_Provider_Note_Template.removeFromArray(Clinical_Add_Provider_Note_Template.specialityCheckedIds, spacialityId);
                    Clinical_Add_Provider_Note_Template.specialityCheckedIds.push(spacialityId);
                }
                else {

                    Clinical_Add_Provider_Note_Template.specialityCheckedIds = Clinical_Add_Provider_Note_Template.removeFromArray(Clinical_Add_Provider_Note_Template.specialityCheckedIds, spacialityId);

                }


            }
            else {

                Clinical_Add_Provider_Note_Template.specialityCheckedIds = [];
                $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty option').each(function () {
                    var spacialityId = $(this).attr("value");
                    Clinical_Add_Provider_Note_Template.specialityCheckedIds.push(spacialityId);
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

        var providerContext = '#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #providerMultiList';
        //find match in both mutiselect and show it
        $(providerContext).find('.dropdown-menu').find('li').each(function () {

            var $li = $(this);
            var isExists = false;
            var value = $li.find('input').val();
            var ddlId = $(providerContext).find('select').attr('id');

            $.each(Clinical_Add_Provider_Note_Template.specialityCheckedIds, function (index, specialtyId) {

                if (value != "") {
                    var refName = Clinical_Add_Provider_Note_Template.getRefValuefromDdl(ddlId, value);

                    // if true then show and check the provider
                    if (specialtyId == refName) {
                        isExists = true;
                        return false;
                    }
                }
            });
            if (Clinical_Add_Provider_Note_Template.specialityCheckedIds.length > 0) {
                if (!isExists) {
                    Clinical_Add_Provider_Note_Template.showHideMultiSelectDdlOptions(true, ddlId, $li, value, 'provider');
                }
                else {
                    Clinical_Add_Provider_Note_Template.showHideMultiSelectDdlOptions(false, ddlId, $li, value, 'provider');
                }
            }
            else {
                Clinical_Add_Provider_Note_Template.showHideMultiSelectDdlOptions(false, ddlId, $li, value, 'provider', "unCheckAll");
            }
        });
    },
    //Start//03/16/2016//M Ahmad Imran//Implimented "getRefValuefromDdl" This function will return refname using (li's input value equals ddl option value)
    getRefValuefromDdl: function (ddlId, liId) {
        var $ddlOptions = $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + " #" + ddlId).find('option');
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

                    $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + " #" + ddlId).find('option[value="' + ddlOptionValue + '"]').prop('disabled', true);
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
            $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + " #" + ddlId).find('option[value="' + ddlOptionValue + '"]').prop('disabled', false);

        }
    },
    //Start//03/16/2016//M Ahmad Imran//Implimented "loadEntityProvider" This function will load entity based provider
    loadEntityProvider: function (entityId) {
        var objDeffered = $.Deferred();
        var data = "entityID=" + entityId;
        if (entityId != null && entityId > 0) {

            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateProvider');
                var $providerHiddenDdl = $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlHiddenNotesTemplateProvider');

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
                if (Clinical_Add_Provider_Note_Template.ProviderIds != '') {
                    var Providers = Clinical_Add_Provider_Note_Template.ProviderIds.split(",");
                    Clinical_Add_Provider_Note_Template.providerCheckedIds = Providers;
                    $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateProvider').val(Providers);
                }

            }).then(function () {
                // A hack to trigger the onDropDownHide event of Spacialty multiselect
                $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #divNotesTemplateSpecialty').find('.dropdown-toggle').dropdown('toggle').dropdown('toggle');
                //Intialized in onhidden spacialty ddl.
                Clinical_Add_Provider_Note_Template.IntializeMultiSelectDropDownProviders();
                Clinical_Add_Provider_Note_Template.enableDisableDropDownLists('ddlNotesTemplateProvider', false);
                objDeffered.resolve();
            });
            //enable multiselect

        }
        else {
            //disable multiselect
            Clinical_Add_Provider_Note_Template.enableDisableDropDownLists('ddlNotesTemplateProvider', true);
            objDeffered.resolve();
        }
        return objDeffered;
    },
    //Start//03/16/2016//M Ahmad Imran//Implimented "IntializeMultiSelectDropDownProviders" This function will initialize provider multiselect ddl
    IntializeMultiSelectDropDownProviders: function () {
        $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateProvider').multiselect('destroy');
        $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: false,
            onChange: function (option, checked, select) {
                Clinical_Add_Provider_Note_Template.GetOrderSetTemplate();
                Clinical_Add_Provider_Note_Template.checkSpecialtiesByProviderId(option, checked, select);
            },
            onDropdownHide: function (event) {
                // Clinical_Add_Provider_Note_Template.specialitiesByProviderIds();
                //Refresh multiselect
                //  $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty').multiselect('refresh');
            },


        });
    },
    //Start//03/16/2016//M Ahmad Imran//Implimented "specialitiesByProviderIds" This function will load check spcialities by provider ids checked
    specialitiesByProviderIds: function () {
        //specialty context
        var specialtyContext = '#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #specialityMultiList';
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

            $.each(Clinical_Add_Provider_Note_Template.providerCheckedIds, function (index, providerId) {

                if (value != "") {
                    if (providerId == value) {
                        isExists = true;
                        return false;
                    }
                }
            });
            if (Clinical_Add_Provider_Note_Template.providerCheckedIds.length > 0) {
                if (!isExists) {
                    Clinical_Add_Provider_Note_Template.showHideMultiSelectDdlOptions(true, ddlId, $li, value, 'specialty');
                }
                else {
                    Clinical_Add_Provider_Note_Template.showHideMultiSelectDdlOptions(false, ddlId, $li, value, 'specialty');
                }
            }
            else {
                Clinical_Add_Provider_Note_Template.showHideMultiSelectDdlOptions(true, ddlId, $li, value, 'specialty');
            }

        });
    },
    //Start//03/16/2016//M Ahmad Imran//Implimented "enableDisableDropDownLists" If user is Super Admin show the Entity dropdown otherwise don't

    isSuperAdmin: function () {
        if (globalAppdata['AppUserName'] == DefaultUser) {
            $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #entityDDL').show();
            return true;
        } else {
            $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #entityDDL').hide();
            return false;
        }
    },
    //Start//03/16/2016//M Ahmad Imran//Implimented "enableDisableDropDownLists" which will enable disable multiselect ddls provided
    enableDisableDropDownLists: function (ddlIds, isHide) {
        ddlCommaSeparatedIds = ddlIds.split(',');
        var parrentPanelId = "#" + Clinical_Add_Provider_Note_Template.params["PanelID"];
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
        $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty').multiselect('destroy');
        $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: false,
            onChange: function (option, checked, select) {

                Clinical_Add_Provider_Note_Template.checkProvidersBySpecialityIds(option, checked, select);

            },
            onDropdownHide: function (event) {
                $.when(
                    Clinical_Add_Provider_Note_Template.filterProvidersBySpecialtyIds()
                ).then(function () {
                    if (Clinical_Add_Provider_Note_Template.ProviderIds != '') {
                        var Providers = Clinical_Add_Provider_Note_Template.ProviderIds.split(",");

                        if (Providers != '' && typeof Providers != 'undefined') {

                            $.each(Providers, function (index, item) {
                                Clinical_Add_Provider_Note_Template.providerCheckedIds = Clinical_Add_Provider_Note_Template.removeFromArray(Clinical_Add_Provider_Note_Template.providerCheckedIds, item);
                                Clinical_Add_Provider_Note_Template.providerCheckedIds.push(item);
                            });
                        }
                    }
                    $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateProvider').val(Clinical_Add_Provider_Note_Template.providerCheckedIds);
                    Clinical_Add_Provider_Note_Template.IntializeMultiSelectDropDownProviders();
                });
            },

            onDropdownShow: function (event) {
                //make items selected and initialize dropdownlist
                if (Clinical_Add_Provider_Note_Template.SpecialtyIds != '') {
                    var spacialties = Clinical_Add_Provider_Note_Template.SpecialtyIds.split(",");

                    if (spacialties != '' && typeof spacialties != 'undefined') {

                        $.each(spacialties, function (index, item) {
                            Clinical_Add_Provider_Note_Template.specialityCheckedIds = Clinical_Add_Provider_Note_Template.removeFromArray(Clinical_Add_Provider_Note_Template.specialityCheckedIds, item);
                            Clinical_Add_Provider_Note_Template.specialityCheckedIds.push(item);
                        });
                    }
                }
                Clinical_Add_Provider_Note_Template.setSpacialtiesByselectedProviderIds();
                $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateSpecialty').multiselect('select', Clinical_Add_Provider_Note_Template.specialityCheckedIds);
            },
        });
    },
    //End M Ahmad Imran 03/15/2016
    //Start//03/15/2016//M Ahmad Imran//Implimented "checkSpecialtiesByProviderId"  This function will save privder ids and will check speciality on provider selection
    checkSpecialtiesByProviderId: function (option, checked, select) {

        //provider context
        var providerContext = '#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #divNotesTemplateProvider';
        var isAllProviderSelected = $(providerContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var providerItems = $(providerContext).find('.dropdown-menu').find('li').length;
        var checkedProviderItems = $(providerContext).find('.dropdown-menu').find('li.active').length;

        var allProviders = $(providerContext).find('.dropdown-menu').find('li:not(".filter,.multiselect-all")').length;
        var selectedProviders = $(providerContext).find('.dropdown-menu').find('li.active:not(".filter,.multiselect-all")').length;

        if (checkedProviderItems <= 0) {
            Clinical_Add_Provider_Note_Template.providerCheckedIds = [];
            Clinical_Add_Provider_Note_Template.ProviderIds = '';
        }
            //push all provider checked items
        else if (isAllProviderSelected  && allProviders == selectedProviders) {
            Clinical_Add_Provider_Note_Template.providerCheckedIds = [];
            $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNotesTemplateProvider option').each(function () {
                var providerValue = $(this).val();
                Clinical_Add_Provider_Note_Template.providerCheckedIds.push(providerValue);
            });
        }
        else {
            // provider value
            var providerValue = $(option).val();

            // add to provider array if checked
            if (checked) {
                Clinical_Add_Provider_Note_Template.providerCheckedIds = Clinical_Add_Provider_Note_Template.removeFromArray(Clinical_Add_Provider_Note_Template.providerCheckedIds, providerValue);
                Clinical_Add_Provider_Note_Template.providerCheckedIds.push(providerValue);
            }
                //delete from provider array if not checked
            else {
                Clinical_Add_Provider_Note_Template.providerCheckedIds = Clinical_Add_Provider_Note_Template.removeFromArray(Clinical_Add_Provider_Note_Template.providerCheckedIds, $(option).val());
            }

        }
    },

    ValidateProviderNoteTemplate: function () {

        $('#frmClinicalAddProviderNoteTemplate')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {

                  NoteTemplateName: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: 'Specify a Name for the Template and try again. '
                          },
                      }
                  },
                  TemplateTypeId: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: 'Specify a Category for the Template and try again. '
                          },
                      }
                  },
                  EntityId: {
                      enable: false,
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: 'Specify a Entity for the Template and try again. '
                          },
                      }
                  }
              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           Clinical_Add_Provider_Note_Template.SaveProviderNoteTemplate();

       });

    },
    //Start//1/03/2016//M Ahmad Imran//Implimented InsertTag function which Insert Tag in Template letter.
    InsertTag: function () {
        var TagCategory = $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNoteTemplateTagCategory option:selected').text();
        var TagNameId = $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #NoteTemplatTagName').val();
        var TagCategoryId = $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNoteTemplateTagCategory').val();
        var TagNameText = TagCategory + " " + $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #NoteTemplatTagName option:selected').text();

        if (TagNameId != "") {
            if (typeof tinyMCE != 'undefined') {

                // var InsertFieldInput = '<input type="text" class="TagInserted" readonly id="' + TagNameId + '" value="{{ ' + TagNameText + ' }}" style="min-width: 10px; border: none;width:' + (((TagNameText.length + 4) * 7) - 12) + 'px;"/>';
                if (TagCategory == "Appointment" && TagNameText == "Appointment Reason") {
                    var InsertFieldInput = '<span class="TagInserted appointmentReason" value="{{ ' + TagNameText + ' }}" id="' + TagNameId + '" > {{ ' + TagNameText + ' }} </span>';
                }
                else {
                    var InsertFieldInput = '<span class="TagInserted" value="{{ ' + TagNameText + ' }}" id="' + TagNameId + '" > {{ ' + TagNameText + ' }} </span>';
                }
                if (TagCategory.toLowerCase().trim() == "clinical") {
                    InsertFieldInput = '<input type="text" class="TagInserted" readonly id="' + TagNameId + '" value="{{ ' + TagNameText + ' }}"  style="min-width: 10px; border: none;width:' + (((TagNameText.length + 4) * 7)) + 'px;"/>';
                }
                if (TagCategory == "Custom Forms") {
                    InsertFieldInput = '<input type="text" class="TagInserted" readonly id="' + TagNameId + '" value="{{ ' + TagNameText.replace('Forms', 'Form') + ' }}"  style="min-width: 10px; border: none;width:' + (((TagNameText.length + 4) * 7)) + 'px;"/>';
                }
                if (TagNameText == "Appointment Comments") {
                    InsertFieldInput = '<span class="TagInserted" id="199" > {{ ' + TagNameText + ' }} </span>';
                }
                if (TagNameText == "Miscellaneous Free text") {
                    InsertFieldInput = '<div class="TagInserted" id="' + TagNameId + '" data-title="{FT} Free Text {/FT}"   > {FT}  Free Text  {/FT} </div>';
                    // InsertFieldInput = '<span class="TagInserted" id="' + TagNameId + '" data-title="{FT} Free Text {/FT}" >{FT} ' + TagNameText + ' {/FT} </span>';
                }
                var ed = tinyMCE.activeEditor;
                var currentPositionNode = $(ed.selection.getNode());
                if (currentPositionNode.is('span')) {
                    ed.dom.insertAfter($(InsertFieldInput), ed.selection.getNode());
                }
                else {
                    tinymce.execCommand('mceInsertContent', false, InsertFieldInput);
                }
                ed.selection.select(ed.getBody(), true); // ed is the editor instance
                ed.selection.collapse(false);
            }
        }
        else {
            if ($('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNoteTemplateTagCategory').val() != "") {
                utility.DisplayMessages("Select Tag Name", 3);
            }
            else {
                utility.DisplayMessages("Select Tag Category", 3);
            }
        }
    },
    //End M Ahmad Imran 03/15/2016
    UnLoadTab: function () {

        //Start || 18 August, 2016 || Talha Tanweer || EMR-661
        var tinyMCEContentToCompare = tinyMCE.activeEditor.getContent();

        if (Clinical_Add_Provider_Note_Template.TemplateContent != tinyMCE.activeEditor.getContent()) {
            var TopContentToRemove = "<body>\n<p>&nbsp;</p>";
            if (tinyMCEContentToCompare.indexOf(TopContentToRemove) !== -1) {
                var TopContentWithRemovedSpaces = tinyMCEContentToCompare.replace(TopContentToRemove, "<body>");

                tinyMCEContentToCompare = TopContentWithRemovedSpaces;
                //   tinyMCE.activeEditor.setContent(TopContentWithRemovedSpaces);
            }
            var BottomContentToRemove = "<p>&nbsp;</p>\n</body>";
            if (tinyMCEContentToCompare.indexOf(BottomContentToRemove) !== -1) {
                var BottomContentWithRemovedSpaces = tinyMCEContentToCompare.replace(BottomContentToRemove, "</body>");
                // tinyMCE.activeEditor.setContent(BottomContentWithRemovedSpaces);
                tinyMCEContentToCompare = BottomContentWithRemovedSpaces;
            }
        }
        //End   || 18 August, 2016 || Talha Tanweer ||| EMR-661





        if (EMRUtility.compareFormDataWithSerialized(Clinical_Add_Provider_Note_Template.params.PanelID + ' #frmClinicalAddProviderNoteTemplate') || Clinical_Add_Provider_Note_Template.TemplateContent != tinyMCEContentToCompare) {
            utility.myConfirm('2', function () {
                Clinical_Add_Provider_Note_Template.UnloadNotesTemplate();
            },
           '1'
           );
        } else {
            Clinical_Add_Provider_Note_Template.UnloadNotesTemplate();
        }
    },
    UnloadNotesTemplate: function () {
        if (Clinical_Add_Provider_Note_Template.params["FromAdmin"] == "0") {
            if (Clinical_Add_Provider_Note_Template.params != null && Clinical_Add_Provider_Note_Template.params.ParentCtrl != null) {
                UnloadActionPan(Clinical_Add_Provider_Note_Template.params.ParentCtrl, 'Clinical_Add_Provider_Note_Template');
            }
            else
                UnloadActionPan(null, 'Clinical_Add_Provider_Note_Template');
        }
        else {

            RemoveAdminTab();
        }
    },
    //Start//03/15/2016//M Ahmad Imran//Implimented TagCategoryChange function which Load Tagname dropdown
    TagCategoryChange: function () {
        if ($('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNoteTemplateTagCategory').val() != "") {
            $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #NoteTemplatTagName').prop('disabled', false);
            var self = $('#' + Clinical_Add_Provider_Note_Template.params.PanelID);
            self.find('.NoteTemplatTagName > select').attr('ddlist', 'GetNoteTemplateTagName');
            var data = "IsActive=&ID=" + $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #ddlNoteTemplateTagCategory').val();
            self.find('.NoteTemplatTagName').loadDropDowns(true, data).done(function () {
            });
        }
        else {
            var self = $('#' + Clinical_Add_Provider_Note_Template.params.PanelID);
            $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #NoteTemplatTagName').prop('disabled', true);
            self.find('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #NoteTemplatTagName').attr('ddlist', '');
        }
    },

    CreateContentToSave: function (div) {
        var finalContent = '';
        if ($(div).has('div').length == 0) {
             finalContent += $(div)[0].outerHTML;
        }
        else {          
                $($(div)[0].childNodes).each(function (j, childDiv) {
                    if ($(childDiv).has('div').length == 0) {
                        finalContent += $(childDiv)[0].outerHTML;
                    }
                    else {
                        finalContent += Clinical_Add_Provider_Note_Template.CreateContentToSave(childDiv);
                    }
                });
        }
        return finalContent;                    
    },
    //End M Ahmad Imran 03/15/2016
    //Start//03/15/2016//M Ahmad Imran//Implimented SaveProviderNoteTemplate function which Save Provider Note Template.
    SaveProviderNoteTemplate: function () {
        if (globalAppdata['AppUserName'] == DefaultUser) {
            $("#" + Clinical_Add_Provider_Note_Template.params.PanelID + " #frmClinicalAddProviderNoteTemplate #ddlEntity").enable = true;
            $("#" + Clinical_Add_Provider_Note_Template.params.PanelID + " #frmClinicalAddProviderNoteTemplate").bootstrapValidator('revalidateField', 'EntityId');
        }
        if (EMRUtility.compareFormDataWithSerialized(Clinical_Add_Provider_Note_Template.params.PanelID + ' #frmClinicalAddProviderNoteTemplate') || Clinical_Add_Provider_Note_Template.TemplateContent != tinyMCE.activeEditor.getContent()) {
            setTimeout(function () {

                var finalContent = '';
                var nestedDivs = $("#elm1_ifr").contents().find("#tinymce").find('div').has('div');
                if (nestedDivs.length > 0) {
                    var contentDivs = $("#elm1_ifr").contents().find("#tinymce").find('div:first').siblings().addBack();
                    $(contentDivs).each(function (i, div) {
                        finalContent += Clinical_Add_Provider_Note_Template.CreateContentToSave(div);
                    });
                }
                else {
                    finalContent = tinyMCE.activeEditor.getContent();
                }

                tinyMCE.activeEditor.setContent(finalContent);
                
                var strMessage = "";
                var self = $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + " #frmClinicalAddProviderNoteTemplate");
                var myJSON = self != null ? self.getMyJSONByName() : "{}";
                myJSON = JSON.parse(myJSON);
                myJSON.IsActive = myJSON.IsActive == true ? 1 : 0;
                myJSON.HTMLTemplate = tinyMCE.activeEditor.getContent();
                myJSON.NotesTagNameIds = Clinical_Add_Provider_Note_Template.GetAllTagIDs(tinyMCE.activeEditor.getContent());

                var SpecialtyIds = self.find('#ddlNotesTemplateSpecialty option:Selected').map(function () {
                    return this.value;
                }).get().join(',');
                myJSON.SpecialtyIds = SpecialtyIds;

                var SpecialtyNames = self.find('#ddlNotesTemplateSpecialty option:Selected').map(function () {
                    return this.text;
                }).get().join(', ');
                myJSON.SpecialtyNames = SpecialtyNames;

                var ProviderIds = self.find('#ddlNotesTemplateProvider option:Selected').map(function () {
                    return this.value;
                }).get().join(',');
                myJSON.ProviderIds = ProviderIds;

                var ProviderNames = self.find('#ddlNotesTemplateProvider option:Selected').map(function () {
                    return this.text;
                }).get().join(', ');
                myJSON.ProviderNames = ProviderNames;
                //Start 28-05-2018 Edit by Humaira Yousaf Bug# EMR-6289
                myJSON.OrderSetId = myJSON.OrderSetId == '' ? -1 : myJSON.OrderSetId;
                //End 28-05-2018 Edit by Humaira Yousaf Bug# EMR-6289
                myJSON.ROSDataTemptId = myJSON.ROSDataTemptId == '' ? -1 : myJSON.ROSDataTemptId;
                myJSON.PEDataTemptId = myJSON.PEDataTemptId == '' ? -1 : myJSON.PEDataTemptId;
                myJSON.HPITemplateId = myJSON.HPITemplateId == '' ? -1 : myJSON.HPITemplateId;
                myJSON = JSON.stringify(myJSON);
                if (Clinical_Add_Provider_Note_Template.params.mode == "Add") {
                    AppPrivileges.GetFormPrivileges("Clinical_Template_Provider Note Template", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            Clinical_Add_Provider_Note_Template.ProviderNoteTemplateSave(myJSON).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    Clinical_Provider_Note_Template.notesTemplateSearch();

                                    utility.DisplayMessages(response.Message, 1);



                                    Clinical_Add_Provider_Note_Template.TemplateContent = tinyMCE.activeEditor.getContent();
                                    $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #frmClinicalAddProviderNoteTemplate').data('serialize', $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #frmClinicalAddProviderNoteTemplate').serialize());
                                    Clinical_Add_Provider_Note_Template.params["NotesTemplateId"] = response.NotesTemplateId;
                                    Clinical_Add_Provider_Note_Template.params.mode = "Edit";
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
                else if (Clinical_Add_Provider_Note_Template.params.mode == "Edit") {

                    AppPrivileges.GetFormPrivileges("Clinical_Template_Provider Note Template", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            Clinical_Add_Provider_Note_Template.ProviderNoteTemplateUpdate(myJSON).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    Clinical_Provider_Note_Template.notesTemplateSearch();
                                    utility.DisplayMessages(response.Message, 1);
                                    Clinical_Add_Provider_Note_Template.TemplateContent = tinyMCE.activeEditor.getContent();
                                    $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #frmClinicalAddProviderNoteTemplate').data('serialize', $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #frmClinicalAddProviderNoteTemplate').serialize());

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
            }, 5);
        } else {
            utility.DisplayMessages("Please make any changes to save/update Provider Note Template", 3);
            setTimeout(function () {
                $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #frmClinicalAddProviderNoteTemplate').data('serialize', $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + ' #frmClinicalAddProviderNoteTemplate').serialize());
            }, 100);
        }
    },
    //Start//03/15/2016//M Ahmad Imran//Implimented ProviderNoteTemplateSave which Call to Controller for Save Template letter Detail
    ProviderNoteTemplateSave: function (ProviderNoteTemplateData) {
        var objData = JSON.parse(ProviderNoteTemplateData);
        objData["commandType"] = "SAVE_NOTE_TEMPLATE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "TemplateBuilder", "ProviderNoteTemplate");
    },

    //Start//03/17/2016//M Ahmad Imran//Implimented ProviderNoteTemplateSave which Call to Controller for Save Template letter Detail
    ProviderNoteTemplateUpdate: function (ProviderNoteTemplateData) {
        var objData = JSON.parse(ProviderNoteTemplateData);
        objData["NotesTemplateId"] = Clinical_Add_Provider_Note_Template.params.NotesTemplateId;
        objData["commandType"] = "UPDATE_NOTE_TEMPLATE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "TemplateBuilder", "ProviderNoteTemplate");
    },
    //Start//03/15/2016//M Ahmad Imran//Implimented GetAllTagIDs function which return all Tag Ids from template content

    GetAllTagIDs: function (TemplateLetterHtml) {
        var TagIds = "";
        $($(TemplateLetterHtml).find('span.TagInserted', ':input.TagInserted')).each(function () {

            if ($(this).attr("id") == null)
                return true;

            if (TagIds == "") {
                TagIds = $(this).attr("id");
            }
            else {
                TagIds = TagIds + "," + $(this).attr("id");
            }

        });
        return TagIds;
    },
    //Start//03/17/2016//M Ahmad Imran//Implimented AddNewNoteType function which replace note type dropdown to text box
    AddNewNoteType: function () {
        $("#" + Clinical_Add_Provider_Note_Template.params.PanelID + " #NoteTypeDropDown").hide();
        $("#" + Clinical_Add_Provider_Note_Template.params.PanelID + " #NoteTypeDTextBox").show();
        $("#" + Clinical_Add_Provider_Note_Template.params.PanelID + " #NoteTypeText").focus();
    },
    //Start//03/17/2016//M Ahmad Imran//Implimented SaveNewNoteType function which Save New Note Type and refresh the note type dropdown
    SaveNewNoteType: function () {

        if ($("#" + Clinical_Add_Provider_Note_Template.params.PanelID + " #NoteTypeText").val() != "") {
            Clinical_Add_Provider_Note_Template.Iferror = false;
            Clinical_Add_Provider_Note_Template.NewNoteTypeSave().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    Clinical_Add_Provider_Note_Template.HideNoteTypeTextbox();
                    var self = $('#' + Clinical_Add_Provider_Note_Template.params.PanelID);
                    self.find('#NoteTypeDropDown').loadDropDowns(true).done(function () {
                        $('#' + Clinical_Add_Provider_Note_Template.params.PanelID + " #ddlNoteTemplateType option").each(function () {
                            if ($(this).text() == $("#" + Clinical_Add_Provider_Note_Template.params.PanelID + " #NoteTypeText").val()) {
                                $(this).attr('selected', 'selected');
                            }
                        });
                        $("#" + Clinical_Add_Provider_Note_Template.params.PanelID + " #NoteTypeText").val("");
                    });
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else {
            Clinical_Add_Provider_Note_Template.Iferror = true;
            utility.DisplayMessages("Please enter value to save New Note Type", 3);
            setTimeout(function () {
                Clinical_Add_Provider_Note_Template.AddNewNoteType();
            }, 400);
        }
    },

    //Start//03/15/2016//M Ahmad Imran//Implimented NewNoteTypeSave which Call to Controller for Save New Note Type
    NewNoteTypeSave: function () {
        var objData = {};
        objData["NoteTypeText"] = $("#" + Clinical_Add_Provider_Note_Template.params.PanelID + " #NoteTypeText").val();
        objData["commandType"] = "SAVE_NOTE_TYPE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "TemplateBuilder", "ProviderNoteTemplate");
    },


    HideNoteTypeTextbox: function () {
        setTimeout(function () {
            if (!Clinical_Add_Provider_Note_Template.Iferror) {
                $("#" + Clinical_Add_Provider_Note_Template.params.PanelID + " #NoteTypeDropDown").show();
                $("#" + Clinical_Add_Provider_Note_Template.params.PanelID + " #NoteTypeDTextBox").hide();
                $("#" + Clinical_Add_Provider_Note_Template.params.PanelID + " #NoteTypeText").val("");
            }

        }, 300);
    },
    //Start//03/18/2016//M Ahmad Imran//Implimented KeyDownOnNoteTypeText this function call save if enter key is press
    KeyDownOnNoteTypeText: function (event) {
        event.stopPropagation();
        if (event.keyCode == 13) {
            Clinical_Add_Provider_Note_Template.SaveNewNoteType();
        }
    },
}