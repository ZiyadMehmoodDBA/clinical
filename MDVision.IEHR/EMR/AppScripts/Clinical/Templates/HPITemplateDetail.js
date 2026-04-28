HPITemplateDetail = {

    bIsFirstLoad: true,
    params: [],
    selectedHPITempData: [],
    SpecialtyIds: '',
    ProviderIds: '',
    specialityCheckedIds: [],
    providerCheckedIds: [],
    providerSelectedIds: [],
    NewInsertedId: -1,
    selectedHPITempData: [],
    selectedFindings: [],
    IsUpdate: false,
    SymptomOrder: 1,
    Load: function (params) {

        HPITemplateDetail.params = params;
        var isSelectedEntity = false
        HPITemplateDetail.selectedHPITempData = [];
        HPITemplateDetail.selectedFindings = [];
        var self = $('#' + HPITemplateDetail.params.PanelID + ' #tblHPITemplateDetail');
        
        self.loadDropDowns(true).done(function () {
            if (HPITemplateDetail.params["mode"] == "Edit") {
                var dfd = new $.Deferred();
                HPITemplateDetail.loadDropDowns(dfd);
                dfd.done(function (n) {
                    HPITemplateDetail.LoadHPITemplate();
                });
            }
            else {
                HPITemplateDetail.loadDropDowns();
            }

            HPITemplateDetail.buildSymptomsAutoComplete();
            HPITemplateDetail.validateHPITemplate();
            $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body', html: true });
            HPITemplateDetail.toggleControls();
        });

        HPITemplateDetail.bindDetailsChangeEvents();
    },

    validateHPITemplate: function () {
        $('#' + HPITemplateDetail.params.PanelID + ' #frmHPITemplateDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   Name: {
                       group: '.col-sm-3',
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
            HPITemplateDetail.HPITemplateSave();
        });
    },

    loadDropDowns: function (dfd) {
        HPITemplateDetail.loadEntitySpecialty(globalAppdata["SeletedEntityId"], dfd);
        HPITemplateDetail.IntializeMultiSelectDropDown();
    },

    loadEntitySpecialty: function (entityID, dfd) {

        // Loads Spacialties Based on entityId
        if (entityID != null && entityID > 0) {

            providerDetail.FillSpecialty(entityID).done(function (response) {
                if (response.status != false) {

                    var spacialties = JSON.parse(response.SpecialtyLoad_JSON);
                    $('#' + HPITemplateDetail.params.PanelID + ' #ddlHPITemplateSpecialty').empty();

                    $.each(spacialties, function (i, item) {
                        $('#' + HPITemplateDetail.params.PanelID + ' #ddlHPITemplateSpecialty').append(
                            $('<option/>', {
                                value: item.SpecialtyId,
                                html: item.ShortName
                            })
                        );
                    });

                    //Assign server side spacialties to the specialityCheckedIds array
                    if (HPITemplateDetail.SpecialtyIds != '') {

                        var Specialties = HPITemplateDetail.SpecialtyIds.split(",");
                        HPITemplateDetail.specialityCheckedIds = Specialties;
                        $('#' + HPITemplateDetail.params.PanelID + ' #ddlHPITemplateSpecialty').val(Specialties);
                    }
                    $('#frmHPITemplateDetail').data('serialize', $('#frmHPITemplateDetail').serialize());
                }

            }).then(function () {
                HPITemplateDetail.IntializeMultiSelectDropDownSpecialties();
                //enable dropdownlist
                HPITemplateDetail.enableDisableDropDownLists('ddlHPITemplateSpecialty', false);
                HPITemplateDetail.loadEntityProvider(globalAppdata["SeletedEntityId"], dfd);


            });
        }
        else {
            //Disable dropdownlist
            HPITemplateDetail.enableDisableDropDownLists('ddlHPITemplateSpecialty', true);
        }
    },

    enableDisableDropDownLists: function (ddlIds, isHide) {
        ddlCommaSeparatedIds = ddlIds.split(',');
        var parrentPanelId = "#" + HPITemplateDetail.params["PanelID"];
        $.each(ddlCommaSeparatedIds, function (index, Item) {
            if (isHide) {
                $(parrentPanelId + " #" + Item).multiselect('disable');
            }
            else {
                $(parrentPanelId + " #" + Item).multiselect('enable');
            }
        });
    },
    IntializeMultiSelectDropDownSpecialties: function () {
        $('#' + HPITemplateDetail.params.PanelID + ' #ddlHPITemplateSpecialty').multiselect('destroy');
        $('#' + HPITemplateDetail.params.PanelID + ' #ddlHPITemplateSpecialty').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: false,
            onChange: function (option, checked, select) {
                HPITemplateDetail.checkProvidersBySpecialityIds(option, checked, select);
            },
            onDropdownHide: function (event) {
                $.when(
                    HPITemplateDetail.filterProvidersBySpecialtyIds()
                ).then(function () {
                    if (HPITemplateDetail.ProviderIds != '') {
                        var Providers = HPITemplateDetail.ProviderIds.split(",");

                        if (Providers != '' && typeof Providers != 'undefined') {

                            $.each(Providers, function (index, item) {
                                HPITemplateDetail.providerCheckedIds = HPITemplateDetail.removeFromArray(HPITemplateDetail.providerCheckedIds, item);
                                HPITemplateDetail.providerCheckedIds.push(item);
                            });
                        }
                    }
                    $('#' + HPITemplateDetail.params.PanelID + ' #ddlHPITemplateProvider').val(HPITemplateDetail.providerCheckedIds);
                    HPITemplateDetail.IntializeMultiSelectDropDownProviders();
                });
            },

            onDropdownShow: function (event) {
                //make items selected and initialize dropdownlist
                if (HPITemplateDetail.SpecialtyIds != '') {
                    var spacialties = HPITemplateDetail.SpecialtyIds.split(",");

                    if (spacialties != '' && typeof spacialties != 'undefined') {

                        $.each(spacialties, function (index, item) {
                            HPITemplateDetail.specialityCheckedIds = HPITemplateDetail.removeFromArray(HPITemplateDetail.specialityCheckedIds, item);
                            HPITemplateDetail.specialityCheckedIds.push(item);
                        });
                    }
                }
                HPITemplateDetail.setSpacialtiesByselectedProviderIds();
                $('#' + HPITemplateDetail.params.PanelID + ' #ddlHPIemplateSpecialty').multiselect('select', HPITemplateDetail.specialityCheckedIds);
            },
        });
    },

    checkProvidersBySpecialityIds: function (option, checked, select) {
        //specialty context
        var specialtyContext = '#' + HPITemplateDetail.params.PanelID + ' #divHPITemplateSpecialty';
        var isAllSpecialtySelected = $(specialtyContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var specialtyItems = $(specialtyContext).find('.dropdown-menu').find('li').length;
        var checkedSpecialtyItems = $(specialtyContext).find('.dropdown-menu').find('li.active').length;

        if (checkedSpecialtyItems <= 0) {
            HPITemplateDetail.specialityCheckedIds = [];
            HPITemplateDetail.providerCheckedIds = [];
            HPITemplateDetail.ProviderIds = '';
            HPITemplateDetail.SpecialtyIds = '';
        }
        else {
            if (!isAllSpecialtySelected && !(specialtyItems == checkedSpecialtyItems)) {
                var spacialityId = $(option).attr("value");
                if (checked && spacialityId != "") {
                    HPITemplateDetail.specialityCheckedIds = HPITemplateDetail.removeFromArray(HPITemplateDetail.specialityCheckedIds, spacialityId);
                    HPITemplateDetail.specialityCheckedIds.push(spacialityId);
                }
                else {

                    HPITemplateDetail.specialityCheckedIds = HPITemplateDetail.removeFromArray(HPITemplateDetail.specialityCheckedIds, spacialityId);
                }
            }
            else {

                HPITemplateDetail.specialityCheckedIds = [];
                $('#' + HPITemplateDetail.params.PanelID + ' #ddlHPITemplateSpecialty option').each(function () {
                    var spacialityId = $(this).attr("value");
                    HPITemplateDetail.specialityCheckedIds.push(spacialityId);
                });
            }
        }
    },

    filterProvidersBySpecialtyIds: function () {

        var providerHiddenContext = '#' + HPITemplateDetail.params.PanelID + ' #ddlHiddenHPITemplateProvider';

        var providerContext = '#' + HPITemplateDetail.params.PanelID + ' #ddlHPITemplateProvider';
        $(providerContext).empty();

        if (HPITemplateDetail.specialityCheckedIds.length > 0) {

            $.each(HPITemplateDetail.specialityCheckedIds, function (index, specialtyId) {

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

    removeFromArray: function (array, removeItem) {

        var resultantArray = jQuery.grep(array, function (item) {
            return item != removeItem;
        });
        return resultantArray;
    },

    IntializeMultiSelectDropDownProviders: function () {
        $('#' + HPITemplateDetail.params.PanelID + ' #ddlHPITemplateProvider').multiselect('destroy');
        $('#' + HPITemplateDetail.params.PanelID + ' #ddlHPITemplateProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: false,
            onChange: function (option, checked, select) {
                HPITemplateDetail.checkSpecialtiesByProviderId(option, checked, select);
            },
            onDropdownHide: function (event) {
            },
        });
    },

    checkSpecialtiesByProviderId: function (option, checked, select) {

        //provider context
        var providerContext = '#' + HPITemplateDetail.params.PanelID + ' #divHPITemplateProvider';
        var isAllProviderSelected = $(providerContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var providerItems = $(providerContext).find('.dropdown-menu').find('li').length;
        var checkedProviderItems = $(providerContext).find('.dropdown-menu').find('li.active').length;

        if (checkedProviderItems <= 0) {
            HPITemplateDetail.providerCheckedIds = [];
            HPITemplateDetail.ProviderIds = '';
        }
            //push all provider checked items
        else if (isAllProviderSelected) {
            HPITemplateDetail.providerCheckedIds = [];
            $('#' + HPITemplateDetail.params.PanelID + ' #ddlHPITemplateProvider option').each(function () {
                var providerValue = $(this).val();
                HPITemplateDetail.providerCheckedIds.push(providerValue);
            });
        }
        else {
            // provider value
            var providerValue = $(option).val();

            // add to provider array if checked
            if (checked) {
                HPITemplateDetail.providerCheckedIds = HPITemplateDetail.removeFromArray(HPITemplateDetail.providerCheckedIds, providerValue);
                HPITemplateDetail.providerCheckedIds.push(providerValue);
            }
                //delete from provider array if not checked
            else {
                HPITemplateDetail.providerCheckedIds = HPITemplateDetail.removeFromArray(HPITemplateDetail.providerCheckedIds, $(option).val());
            }

        }
    },

    loadEntityProvider: function (entityId, dfd) {

        var data = "entityID=" + entityId;
        if (entityId != null && entityId > 0) {

            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $('#' + HPITemplateDetail.params.PanelID + ' #ddlHPITemplateProvider');
                var $providerHiddenDdl = $('#' + HPITemplateDetail.params.PanelID + ' #ddlHiddenHPITemplateProvider');

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
                if (HPITemplateDetail.ProviderIds != '') {
                    var Providers = HPITemplateDetail.ProviderIds.split(",");
                    HPITemplateDetail.providerCheckedIds = Providers;
                    $('#' + HPITemplateDetail.params.PanelID + ' #ddlHPITemplateProvider').val(Providers);
                }
                $('#frmHPITemplateDetail').data('serialize', $('#frmHPITemplateDetail').serialize());
            }).then(function () {
                // A hack to trigger the onDropDownHide event of Spacialty multiselect      
                $('#' + HPITemplateDetail.params.PanelID + ' #divHPITemplateSpecialty').find('.dropdown-toggle').dropdown('toggle').dropdown('toggle');
                //Intialized in onhidden spacialty ddl.  
                HPITemplateDetail.IntializeMultiSelectDropDownProviders();
                $('#frmHPITemplateDetail').data('serialize', $('#frmHPITemplateDetail').serialize());
                if (dfd)
                    dfd.resolve();
            });
            //enable multiselect           
        }
        else {
            //disable multiselect
            HPITemplateDetail.enableDisableDropDownLists('ddlHPITemplateProvider', true);
        }
    },

    setSpacialtiesByselectedProviderIds: function () {

        $.each(HPITemplateDetail.providerCheckedIds, function (index, item) {
            $('#' + HPITemplateDetail.params.PanelID + ' #ddlHPITemplateProvider option').each(function () {
                if ($(this).val() != '') {
                    if ($(this).val() == item) {
                        HPITemplateDetail.specialityCheckedIds = HPITemplateDetail.removeFromArray(HPITemplateDetail.specialityCheckedIds, $(this).attr('refname'));
                        HPITemplateDetail.specialityCheckedIds.push($(this).attr('refname'));
                    }
                }
            });
        });
    },

    LoadHPITemplate: function () {
        $("#findingContent").text('');
        $("#SymptomPreview").removeClass('hidden');
        HPITemplateDetail.LoadHPITempSymFindings(HPITemplateDetail.params.HPITemplateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                if (response.HPITemplateCount > 0) {
                    var templateData = JSON.parse(response.HPITemplate_JSON);
                    //var templateData = JSON.parse(res);
                    if (templateData.length > 0 && templateData[0] != null) {
                        $('#' + HPITemplateDetail.params.PanelID + " #frmHPITemplateDetail #txtHPITemplateName").val(templateData[0].Name);
                        $('#' + HPITemplateDetail.params.PanelID + " #frmHPITemplateDetail #findingContent").append(templateData[0].TemplatePreview);
                        $('#' + HPITemplateDetail.params.PanelID + " #frmHPITemplateDetail #txtOverallComments").val(templateData[0].Comments);

                        $('#' + HPITemplateDetail.params.PanelID + ' #ddlHPITemplateProvider').multiselect('clearSelection', false);
                        $('#' + HPITemplateDetail.params.PanelID + ' #ddlHPITemplateProvider').multiselect('updateButtonText');
                        $('#' + HPITemplateDetail.params.PanelID + " #ddlHPITemplateProvider").val(templateData[0]['ProviderIds'].split(','));
                        $('#' + HPITemplateDetail.params.PanelID + ' #ddlHPITemplateProvider').multiselect("refresh");
                        //Start 16-10-2017 Humaira Yousaf Bug# EMR-4972
                        if (templateData[0]['ProviderIds'] != "") {
                            HPITemplateDetail.providerCheckedIds = templateData[0]['ProviderIds'].split(',');
                        }
                        //End 16-10-2017 Humaira Yousaf Bug# EMR-4972

                        $('#' + HPITemplateDetail.params.PanelID + ' #ddlHPITemplateSpecialty').multiselect('clearSelection', false);
                        $('#' + HPITemplateDetail.params.PanelID + ' #ddlHPITemplateSpecialty').multiselect('updateButtonText');
                        $('#' + HPITemplateDetail.params.PanelID + " #ddlHPITemplateSpecialty").val(templateData[0]['SpecialtyIds'].split(','));
                        $('#' + HPITemplateDetail.params.PanelID + ' #ddlHPITemplateSpecialty').multiselect("refresh");
                        //Start 16-10-2017 Humaira Yousaf Bug# EMR-4972
                        if (templateData[0]['SpecialtyIds'] != "") {
                            HPITemplateDetail.specialityCheckedIds = templateData[0]['SpecialtyIds'].split(',');
                        }
                        //End 16-10-2017 Humaira Yousaf Bug# EMR-4972
                    }
                }

                if (response.HPITemplateSymptomsCount > 0) {
                    //   $("#findingContent div").remove();
                    $('#' + HPITemplateDetail.params.PanelID + ' #ulHPISymptoms tr:gt(0)').remove();

                    //  $("#ulHPISymptoms tr").remove();
                    var SymptomData = JSON.parse(response.HPITemplateSymptoms_JSON);
                    //var SymptomData = JSON.parse(resTemplateSymptoms);

                    for (var i = 0; i < SymptomData.length; i++) {
                        if (SymptomData[i].HPISymptomId != "") {
                            var tr = HPITemplateDetail.addSymptom(SymptomData[i].HPISymptomId, SymptomData[i].SymptomName, SymptomData[i].HPISymptomsAnswersId, SymptomData[i].SymptomOrder);
                            $("#ulHPISymptoms").removeClass('hidden');
                            $("#ulHPISymptoms tbody").append(tr);
                            $('#' + HPITemplateDetail.params.PanelID + ' #sectionFindingDetails').removeClass('disableAll');

                            //if (SymptomData[i].IsSelectedSymptom == "True") {
                            //    $("#ulHPISymptoms #chk" + SymptomData[i].HPISymptomId).prop("checked", true);
                            //}

                            if (response.HPISymFindingsCount > 0) {
                                var FindingData = JSON.parse(response.HPISymFindings_JSON);
                                // var FindingData = JSON.parse(resTemplateFindings);

                                for (var j = 0; j < FindingData.length; j++) {
                                    if (SymptomData[i].HPISymptomId == FindingData[j].HPISymptomId) {
                                        if (FindingData[j].HPIFindingId != "0") {
                                            var li = HPITemplateDetail.addFindings(FindingData[j].HPIFindingId, FindingData[j].FindingName, FindingData[j].HPISymptomId, FindingData[j].FindingOrder);
                                            $("#ulHPISymptomFindings").append(li);
                                        }

                                        var objSelectedFindings = {
                                        };
                                        if (FindingData[j].IsSelected == "True") {
                                            $("#ulHPISymptomFindings #chk" + FindingData[j].HPIFindingId).prop("checked", true);
                                            if (SymptomData[i].IsSelectedSymptom == "True") {
                                                objSelectedFindings = {
                                                    HPISymptomId: FindingData[j].HPISymptomId, SymptomOrder: SymptomData[i].SymptomOrder, IsChecked: true, FindingId: FindingData[j].HPIFindingId, FindingName: FindingData[j].FindingName, IsSymptomChecked: true, IsSymptomDeleted: 0, IsFindingDeleted: 0, HPISymptomsAnswersId: SymptomData[i].HPISymptomsAnswersId != "" ? SymptomData[i].HPISymptomsAnswersId : null,
                                                    IsSymptomsDetail: FindingData[j].IsSymptomsDetail, HPISymptoms_SeverityId: FindingData[j].HPISymptoms_SeverityId, HPISymptoms_LocationIds: FindingData[j].HPISymptoms_LocationIds, HPISymptoms_RadiationId: FindingData[j].HPISymptoms_RadiationId,
                                                    HPISymptoms_QualityId: FindingData[j].HPISymptoms_QualityId, Onset: FindingData[j].Onset,
                                                    AssociatedWith: FindingData[j].AssociatedWith, HPISymptoms_AggravatedById: FindingData[j].HPISymptoms_AggravatedById, HPISymptoms_RelievedById: FindingData[j].HPISymptoms_RelievedById, FindingOrder: FindingData[j].FindingOrder
                                                };
                                            }
                                            else {
                                                objSelectedFindings = {
                                                    HPISymptomId: FindingData[j].HPISymptomId, SymptomOrder: SymptomData[i].SymptomOrder, IsChecked: true, FindingId: FindingData[j].HPIFindingId, FindingName: FindingData[j].FindingName, IsSymptomChecked: false, IsSymptomDeleted: 0, IsFindingDeleted: 0, HPISymptomsAnswersId: SymptomData[i].HPISymptomsAnswersId != "" ? SymptomData[i].HPISymptomsAnswersId : null,
                                                    IsSymptomsDetail: FindingData[j].IsSymptomsDetail, HPISymptoms_SeverityId: FindingData[j].HPISymptoms_SeverityId, HPISymptoms_LocationIds: FindingData[j].HPISymptoms_LocationIds, HPISymptoms_RadiationId: FindingData[j].HPISymptoms_RadiationId,
                                                    HPISymptoms_QualityId: FindingData[j].HPISymptoms_QualityId, Onset: FindingData[j].Onset,
                                                    AssociatedWith: FindingData[j].AssociatedWith, HPISymptoms_AggravatedById: FindingData[j].HPISymptoms_AggravatedById, HPISymptoms_RelievedById: FindingData[j].HPISymptoms_RelievedById, FindingOrder: FindingData[j].FindingOrder
                                                };
                                            }
                                        }
                                        else {
                                            $("#ulHPISymptomFindings #chk" + FindingData[j].HPIFindingId).prop("checked", false);
                                            if (SymptomData[i].IsSelectedSymptom == "True") {
                                                objSelectedFindings = {
                                                    HPISymptomId: FindingData[j].HPISymptomId, SymptomOrder: SymptomData[i].SymptomOrder, IsChecked: false, FindingId: FindingData[j].HPIFindingId, FindingName: FindingData[j].FindingName, IsSymptomChecked: true, IsSymptomDeleted: 0, IsFindingDeleted: 0, HPISymptomsAnswersId: SymptomData[i].HPISymptomsAnswersId != "" ? SymptomData[i].HPISymptomsAnswersId : null,
                                                    IsSymptomsDetail: FindingData[j].IsSymptomsDetail, HPISymptoms_SeverityId: FindingData[j].HPISymptoms_SeverityId, HPISymptoms_LocationIds: FindingData[j].HPISymptoms_LocationIds, HPISymptoms_RadiationId: FindingData[j].HPISymptoms_RadiationId,
                                                    HPISymptoms_QualityId: FindingData[j].HPISymptoms_QualityId, Onset: FindingData[j].Onset,
                                                    AssociatedWith: FindingData[j].AssociatedWith, HPISymptoms_AggravatedById: FindingData[j].HPISymptoms_AggravatedById, HPISymptoms_RelievedById: FindingData[j].HPISymptoms_RelievedById, FindingOrder: FindingData[j].FindingOrder
                                                };
                                            }
                                            else {
                                                objSelectedFindings = {
                                                    HPISymptomId: FindingData[j].HPISymptomId, SymptomOrder: SymptomData[i].SymptomOrder, IsChecked: false, FindingId: FindingData[j].HPIFindingId, FindingName: FindingData[j].FindingName, IsSymptomChecked: false, IsSymptomDeleted: 0, IsFindingDeleted: 0, HPISymptomsAnswersId: SymptomData[i].HPISymptomsAnswersId != "" ? SymptomData[i].HPISymptomsAnswersId : null,
                                                    IsSymptomsDetail: FindingData[j].IsSymptomsDetail, HPISymptoms_SeverityId: FindingData[j].HPISymptoms_SeverityId, HPISymptoms_LocationIds: FindingData[j].HPISymptoms_LocationIds, HPISymptoms_RadiationId: FindingData[j].HPISymptoms_RadiationId,
                                                    HPISymptoms_QualityId: FindingData[j].HPISymptoms_QualityId, Onset: FindingData[j].Onset,
                                                    AssociatedWith: FindingData[j].AssociatedWith, HPISymptoms_AggravatedById: FindingData[j].HPISymptoms_AggravatedById, HPISymptoms_RelievedById: FindingData[j].HPISymptoms_RelievedById, FindingOrder: FindingData[j].FindingOrder
                                                };
                                            }
                                        }
                                        HPITemplateDetail.selectedFindings.push(objSelectedFindings);
                                    }
                                }
                            }
                            if (SymptomData[i].HPISymptomsAnswersId == "2") {
                                $('#' + HPITemplateDetail.params.PanelID + ' #ulHPISymptomFindings').addClass('disableAll');
                                $('#' + HPITemplateDetail.params.PanelID + ' #sectionFindingDetails').addClass('disableAll');
                            }
                        }
                    }
                    if (SymptomData[0].HPISymptomId) {
                        $('#ulHPISymptoms #' + SymptomData[0].HPISymptomId).click();
                    }
                }
                $('#frmHPITemplateDetail').data('serialize', $('#frmHPITemplateDetail').serialize());
            }
        });
    },


    LoadHPITempSymFindings: function (templateId) {
        var objData = new Object();
        objData["HPITemplateId"] = templateId;
        objData["commandType"] = "Load_HPITemplate_Fill";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPI");
    },

    UnLoad: function () {

        // Empty global variables
        HPITemplateDetail.specialityCheckedIds = [];
        HPITemplateDetail.providerCheckedIds = [];
        HPITemplateDetail.providerSelectedIds = [];
        HPITemplateDetail.selectedPhyExamTempData = [];
        HPITemplateDetail.SpecialtyIds = "";
        HPITemplateDetail.ProviderIds = "";
        HPITemplateDetail.SymptomOrder = 1;

        utility.UnLoadDialog("frmHPITemplateDetail", function () {
            UnloadActionPan(HPITemplateDetail.params["ParentCtrl"], "HPITemplateDetail");
            if (HPITemplateDetail != null) {
                // HPITemplateDetail.loadPhysicalExamTemplateMK();
                HPITemplateDetail.selectedFindings = [];
            }
        }, function () {

            UnloadActionPan(HPITemplateDetail.params["ParentCtrl"], "HPITemplateDetail");
        });
    },

    addNewItem: function (itemType) {
        if (itemType != null && itemType != "") {

            //  var addSubCharIcon = "";

            var symptomsSelectAll = null;
            var findingsSelectAll = null;

            // var isSubCharacteristic = false;
            var ulControl = "";
            var currentLiClick = "";
            var currentCtrlId = "";
            var currentParentId = "";
            var currentId = "";
            currentId = HPITemplateDetail.NewInsertedId--;
            var isExist = "";

            var saveMethodHPI = "";

            if (itemType.toLowerCase() == "symptom") {
                currentLiClick = "";
                currentCtrlId = "ulHPISymptoms";
                ulControl = $('#' + HPITemplateDetail.params.PanelID + " #frmHPITemplateDetail #" + currentCtrlId);
                saveMethodHPI = "HPITemplateDetail.AddSymptomHPI(this, '" + currentId + "',event);";
            }
            else if (itemType.toLowerCase() == "finding") {
                currentLiClick = "";
                currentCtrlId = "ulHPISymptomFindings";
                ulControl = $('#' + HPITemplateDetail.params.PanelID + " #frmHPITemplateDetail #" + currentCtrlId);
                saveMethodHPI = "HPITemplateDetail.AddFindingHPI(this, '" + currentId + "',event);";
            }

            if (ulControl != null && ulControl != "") {
                var currentLiClass = "";

                var arrNewlyAddedLi = ulControl.find("li[id*='-']");

                if (itemType.toLowerCase() != "symptom") {
                    currentParentId = ulControl.find("li:last").attr("parentid");
                    if (currentParentId == null)
                        currentParentId = ulControl.attr("ParentId");
                }

                var onClick = "";
                var deleteFunction = "HPITemplateDetail.deleteItem(this,'" + currentCtrlId + "','" + currentId + "');";
                //var deleteIcon = '<a class="btn btn-xs pull-left" href="#" onclick="' + deleteFunction + '" title="Delete Record"><i class="fa fa-close red"></i></a>';

                //liInnerText = '<div class="checkbox-custom checkboxTiny checkbox-success">' + deleteIcon
                //    + '<input checked type="checkbox" id="chk' + currentId + '+ve" name="' + currentId + '" class="pull-left  char" onclick="">'                  
                //    + '<label id="lblName' + currentId + '" class=" hidden" data-toggle="tooltip"  title="" data-original-title="' + currentId + '">' + currentId + '</label>'
                //    + '<div id="divNameDetail' + currentId + '" class="rightInnerAddon pb-xs">'
                //    + '<textarea rows="1" id="txtName' + currentId + '" onkeypress="HPITemplateDetail.saveDetailComments(event,this)"  name="Name' + currentId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll"></textarea>'
                //    + '<div class="clearfix"></div><div class="rightInnerAddonBtn"><span id="btnSaveDetail' + currentId + '" onclick="' + saveMethodHPI + '" class="btn btn-link btn-xs">'
                //    + '<i class="fa fa-save"></i></span></div></div><div class="clearfix"></div><div class="clearfix"></div></div>';

                //var liTobeAdded = '<li id="' + currentId + '" ' + currentLiClass + ' parentid="' + currentParentId + '" onclick="' + onClick + '" value="' + currentId + '" refValue="' + currentId + '"' + isExist + '>' + liInnerText + '</li>';
                var trTobeAdded = '';
                var HPISymptomId = $('#' + HPITemplateDetail.params.PanelID + ' #ulHPISymptoms tr.active').attr('id');

                if (itemType.toLowerCase() == "symptom") {
                    trTobeAdded = '<tr id="' + currentId + '" parentid="null" onclick="HPITemplateDetail.loadFindings(' + currentId + ')" value="' + currentId + '" refvalue="" subcharacteristicexist=" "><td class="text-center">' +
                                      '<input type="radio" symptomtype="1" id="chk' + currentId + '" name="SymptomsType' + currentId + '" onclick="HPITemplateDetail.ManageFindings(' + currentId + ', this,event);"></td><td class="text-center"><input type="radio" symptomtype="2" id="chk' + currentId + '" name="SymptomsType' + currentId + '" onclick="HPITemplateDetail.ManageFindings(' + currentId + ', this,event);"></td><td>' +
                                      '<div class="col-xs-11 p-none"><textarea rows="1" spellcheck="true" id="txtName' + currentId + '" onkeypress=""  name="Name' + currentId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll"></textarea>'
                                      + '<div class="rightInnerAddonBtn"><span id="btnSaveDetail' + currentId + '" onclick="' + saveMethodHPI + '" class="btn btn-link btn-xs">'
                                      + '<i class="fa fa-save"></i></span></div></div><a href="#" class="removeIconListHover pull-right" onclick="' + deleteFunction + '"><i class="fa fa-close"></i></a>'
                                     + '</td></tr>';
                }

                else if (itemType.toLowerCase() == "finding") {
                    var trTobeAdded = '<li id="' + currentId + '" parentid="' + HPISymptomId + '" onclick="HPITemplateDetail.PreviewFindings(' + currentId + ',\'' + '' + '\', this, ' + HPISymptomId + ');" value="' + currentId + '" refvalue="" subcharacteristicexist=" " class=""><div class="pull-left checkbox-custom checkboxTiny checkbox-success">' +
                   '<input type="checkbox" id="chk' + currentId + '" name="' + currentId + '" class="pull-left" ' +
                   'onclick="HPITemplateDetail.PreviewFindings(' + currentId + ',\'' + '' + '\', this, ' + HPISymptomId + ');"><label></label></div><div class="col-xs-10 p-none"><textarea rows="1" spellcheck="true" id="txtName' + currentId + '" onkeypress="HPITemplateDetail.saveDetailComments(event,this)"  name="Name' + currentId + '" type="text" class="form-control pr-xlg height-max105 textAreaScroll"></textarea>' +
                   '<div class="rightInnerAddonBtn"><span id="btnSaveDetail' + currentId + '" onclick="' + saveMethodHPI + '" class="btn btn-link btn-xs">'
                                      + '<i class="fa fa-save"></i></span></div></div><a href="#" class="removeIconList" onclick="' + deleteFunction + '"><i class="fa fa-close"></i></a><div class="clearfix"></div></li>';
                }

                if (symptomsSelectAll != null && symptomsSelectAll.length > 0) {
                    $(trTobeAdded).insertAfter("#chkboxSelectAllChars");
                }
                else if (findingsSelectAll != null && findingsSelectAll.length > 0) {
                    $(trTobeAdded).insertAfter("#chkboxSelectAllSubChars");
                }
                else {
                    if (itemType.toLowerCase() == "symptom") {
                        ulControl.find('tr:first').after(trTobeAdded);
                        $(ulControl).removeClass('hidden');
                    }
                    else {
                        if (ulControl.find('li:first').length == 0) {
                            var selectAll = '<li id="chkboxSelectAllFindings" parentid="' + HPISymptomId + '" value="Select All" refvalue="Select All"><div class="pl-lg checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="chkboxSelectAllFindings" name="Select All" class="pull-left" '
                            + 'onclick="HPITemplateDetail.selectAllFindings(this);"><label id="lblSelectAll" class="" data-toggle="tooltip" title="" data-original-title="Select All">Select All</label><div class="clearfix"></div><div class="clearfix"></div></div></li>';
                            $(ulControl).append(selectAll);                            
                        }
                        ulControl.find('li:first').after(trTobeAdded);
                        $(ulControl).removeClass('hidden');

                    }
                }
                ulControl.find('tr#' + currentId + ' #txtName' + currentId).focus()
            }
        }
    },

    deleteItem: function (obj, ctrlId, currentId) {

        var itemId = $(obj).closest("li").attr('id');

        if (ctrlId == "ulHPISymptoms") {
            $("#" + currentId).remove();

        } else if (ctrlId == "ulHPISymptomFindings") {
            $("#" + currentId).remove();
        }
    },

    saveDetailComments: function (event, obj) {
        if (event.which == 13) {
            event.preventDefault();
            if ($(obj).val() == '') {
                utility.DisplayMessages("Please enter some value", 3);
                return;
            }

            $(obj).focusout();
            $(obj).blur();

            this.currentIdOfText = $(obj).attr("id").replace("txtName", '');
            var onClickFunction = $(obj).parent().parent().find('.btn-link').attr("onclick");
            var ID = $(obj).parent().parent().find('.btn-link').attr("id");
            var ULID = $(obj).parent().parent().find('.btn-link').closest("ul").attr("id");
            onClickFunction = onClickFunction.replace('this', "$('#" + HPITemplateDetail.params.PanelID + " #" + ULID + " #" + ID + "')");
            eval(onClickFunction);
        }
    },

    toggleControls: function () {
        $("#btnBold").click(function () {
            $('#findingContent div[id^=divSymptom]').toggleClass("bold");
        });

        $("#btnItalic").click(function () {
            $('#findingContent div[id^=divSymptom]').toggleClass("italic");
        });

        $("#btnUnderline").click(function () {
            $('#findingContent div[id^=divSymptom]').toggleClass("underline");
        });

    },

    HPITemplateSave: function () {

        var prevFindingId = $('#' + HPITemplateDetail.params.PanelID + ' #ulHPISymptomFindings li.active').attr('id');
        var HPISymptomId = $("#ulHPISymptoms tr.active").attr('id');
        HPITemplateDetail.addFindingsDetails(HPISymptomId, prevFindingId);

        //if (prevFindingId) {
        //    var HPISymptomId = $("#ulHPISymptoms tr.active").attr('id');
        //    HPITemplateDetail.addFindingsDetails(HPISymptomId, prevFindingId);
        //}

        var isValid = false;
        var self = null;
        self = $('#' + HPITemplateDetail.params.PanelID + ' #frmHPITemplateDetail');

        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objData = JSON.parse(myJSON);
        var isStillValid = false;

        myJSON = JSON.stringify(objData);
        HPITemplateDetail.saveHPITemplate(myJSON).done(function (response) {
            if (response != null && response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    if (response.HPITemplateId != "") {
                        HPITemplateDetail.params.HPITemplateId = response.HPITemplateId;
                        for (var count in HPITemplateDetail.selectedHPITempData) {
                            HPITemplateDetail.selectedHPITempData[count];
                        }
                    }

                    HPITemplateDetail.params.mode = "Edit";

                    // Empty global variables
                    HPITemplateDetail.specialityCheckedIds = [];
                    HPITemplateDetail.providerCheckedIds = [];
                    HPITemplateDetail.providerSelectedIds = [];
                    HPITemplateDetail.selectedHPITempData = [];
                    HPITemplateDetail.SpecialtyIds = "";
                    HPITemplateDetail.ProviderIds = "";

                    UnloadActionPan(HPITemplateDetail.params["ParentCtrl"], "HPITemplateDetail");
                    if (HPITemplate != null) {
                        HPITemplate.loadHPITemplate();
                        HPITemplateDetail.selectedFindings = [];
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
        });
    },

    saveHPITemplate: function (HPITemplateData, TemplateName) {
        var self = null, IsActive = null;
        self = $('#' + HPITemplateDetail.params.PanelID + ' #frmHPITemplateDetail');
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objData = JSON.parse(myJSON);
        if (HPITemplateDetail.params["mode"] == "Edit") {
            objData["HPITemplateId"] = (HPITemplateDetail.params["HPITemplateId"]);
        }
        else {
            objData["HPITemplateId"] = '-1';
        }

        var TemplateName = $('#' + HPITemplateDetail.params.PanelID + ' #frmHPITemplateDetail #txtHPITemplateName').val();
        objData["Name"] = TemplateName;

        //if (objData["HPITemplateId"] == '-1') {
        //    objData["Name"] = TemplateName;
        //    IsActive = "1";
        //}
        //else {
        //    objData["Name"] = mainTemplateName != "" ? mainTemplateName : TemplateName;
        //}
        objData["SaveAsTemplateName"] = TemplateName;
        var SpecialtyIds = self.find('#ddlHPITemplateSpecialty option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objData["SpecialtyIds"] = SpecialtyIds;

        var ProviderIds = self.find('#ddlHPITemplateProvider option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objData["ProviderIds"] = ProviderIds;

        objData["commandType"] = "Save_HPI_Template";

        $(HPITemplateDetail.selectedFindings).each(function (index, item) {
            if (item.FindingId == 0 || item.FindingId == -1) {
                item.FindingId = -1;
                item.FindingOrder = -1;
            }
            if (item.HPISymptomsAnswersId == '2') {
                item.AssociatedWith = '';
                item.HPISymptoms_AggravatedById = '';
                item.HPISymptoms_LocationIds = '';
                item.HPISymptoms_QualityId = '';
                item.HPISymptoms_RadiationId = '';
                item.HPISymptoms_RelievedById = '';
                item.HPISymptoms_SeverityId = '';
                item.Onset = '';
            }
        });
        objData["SymptomFindingData"] = HPITemplateDetail.selectedFindings;
        objData["TemplatePreview"] = $.trim($("#findingContent").html()); // $.trim($("#findingContent").text());
        objData["Comments"] = $('#txtOverallComments').val();
        objData["EntityId"] = globalAppdata["SeletedEntityId"];

        IsActive = $('#' + HPITemplate.params.PanelID + ' #pnlHPITemplate_Result #divSwitch #switchActive').attr('isactive');
        if (IsActive == null) {
            IsActive = "1";
        }

        objData["IsActive"] = IsActive;

        var negativeCounter = -1;
        for (var i = 0; i < HPITemplateDetail.selectedFindings.length; i++) {
            if (HPITemplateDetail.selectedFindings[i].HPISymptomId == undefined || HPITemplateDetail.selectedFindings[i].HPISymptomId == 'undefined') {
                HPITemplateDetail.selectedFindings[i].HPISymptomId = negativeCounter;
            }
            negativeCounter--;
        }

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPI");
    },

    buildSymptomsAutoComplete: function (templateId) {
        HPITemplateDetail.HPITemplateSymptomsLoad(templateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var resSymptoms = response.HPISymptoms_JSON;
                var data = [];
                $.each(resSymptoms, function (i, item) {
                    data.push({ id: item.HPISymptomId, value: item.Name, type: item.HPISymptomsAnswersId });
                });

                $("#Symptoms").kendoAutoComplete({
                    dataSource: data,
                    filter: "contains",
                    dataTextField: "value",
                    placeholder: "Select Symptom...",
                    select: function (e) {
                        var dataItem = this.dataItem(e.item.index());
                        var symptomOrder = 1;                        
                        var lastOrder = $("#ulHPISymptoms tr").last().attr('symptomOrder');
                        if (lastOrder != null) {
                            symptomOrder = parseInt(lastOrder) + 1;
                        }
                        else {
                            symptomOrder = 1;
                        }

                        var tr = HPITemplateDetail.addSymptom(dataItem.id, dataItem.value, dataItem.type, symptomOrder);
                        if (tr != undefined) {
                            $("#ulHPISymptoms").removeClass('hidden');
                            $("#ulHPISymptoms tbody").append(tr);
                            HPITemplateDetail.loadFindings(dataItem.id);
                            $("#ulHPISymptomFindings").addClass('disableAll');
                        }
                        $("#Symptoms").val('');
                        e.preventDefault();
                    },
                    footerTemplate: 'Total <strong>#: instance.dataSource.total() #</strong> items found'
                });

                $("#Systems").parent().addClass('size100per');
                $('#frmHPITemplateDetail').data('serialize', $('#frmHPITemplateDetail').serialize());
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    HPITemplateSymptomsLoad: function (templateId) {
        var objData = new Object();
        objData["HPITemplateId"] = templateId;
        objData["commandType"] = "Load_HPI_Symptoms";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPI");
    },

    addSymptom: function (HPISymptomsId, SymptomName, symptomType, symptomOrder) {
        var itemtoRemove = "symptom";

        for (var i = 0 ; i < HPITemplateDetail.selectedFindings.length; i++) {
            if (HPITemplateDetail.selectedFindings[i].HPISymptomId == HPISymptomsId && HPITemplateDetail.selectedFindings[i].IsSymptomDeleted != 1) {
                utility.DisplayMessages(SymptomName + " already associated with the template", 3);
                return;
            }
        }

        for (var i = 0; i < $("#ulHPISymptoms tr").length; i++) {
            if ($($("#ulHPISymptoms tr label")[i]).text() == SymptomName) {
                utility.DisplayMessages(SymptomName + " already associated with the template", 3);
                return;
            }

        }

        var tr = '';
        var order = 1;
        if (HPITemplateDetail.params["mode"] == "Edit") {
            order = symptomOrder;
        }
        else {
            order = HPITemplateDetail.SymptomOrder++;
        }
        if (symptomType == 1) {
            tr = '<tr symptomorder="' + order + '" id="' + HPISymptomsId + '" parentid="null" isselected="1" onclick="HPITemplateDetail.loadFindings(' + HPISymptomsId + ')" value="' + HPISymptomsId + '" refvalue="" subcharacteristicexist=" "><td class="text-center">' +
                '<input type="radio" checked="checked" symptomtype="1" id="chk' + HPISymptomsId + '" class="MK" name="SymptomsType' + HPISymptomsId + '" onclick="HPITemplateDetail.ManageFindings(' + HPISymptomsId + ', this,event);"></td><td class="text-center"><input type="radio" class="MK" symptomtype="2" id="chk' + HPISymptomsId + '" name="SymptomsType' + HPISymptomsId + '" onclick="HPITemplateDetail.ManageFindings(' + HPISymptomsId + ', this,event);"></td><td><label id="lblName' + HPISymptomsId + '" class="" data-toggle="tooltip" title="" data-original-title="' + SymptomName + '">' + SymptomName + '</label>'
                + '<a href="#"><span class="removeIconListHover pull-right" onclick="HPITemplateDetail.removeSymptoms(' + HPISymptomsId + ', event)";><i class="fa fa-close"></i></span></a></td></tr>';
        }
        else if (symptomType == 2) {
            tr = '<tr symptomorder="' + order + '" id="' + HPISymptomsId + '" parentid="null" isselected="1" onclick="HPITemplateDetail.loadFindings(' + HPISymptomsId + ')" value="' + HPISymptomsId + '" refvalue="" subcharacteristicexist=" "><td class="text-center">' +
               '<input type="radio" class="MK" symptomtype="1" id="chk' + HPISymptomsId + '"  name="SymptomsType' + HPISymptomsId + '" onclick="HPITemplateDetail.ManageFindings(' + HPISymptomsId + ', this,event);"></td><td class="text-center"><input type="radio" checked="checked" class="MK" symptomtype="2" id="chk' + HPISymptomsId + '" name="SymptomsType' + HPISymptomsId + '" onclick="HPITemplateDetail.ManageFindings(' + HPISymptomsId + ', this,event);"></td><td><label id="lblName' + HPISymptomsId + '" class="" data-toggle="tooltip" title="" data-original-title="' + SymptomName + '">' + SymptomName + '</label>'
               + '<a href="#"><span class="removeIconListHover pull-right" onclick="HPITemplateDetail.removeSymptoms(' + HPISymptomsId + ', event)";><i class="fa fa-close"></i></span></a></td></tr>';
        }
        else {
            tr = '<tr symptomorder="' + order + '" id="' + HPISymptomsId + '" parentid="null" isselected="0" onclick="HPITemplateDetail.loadFindings(' + HPISymptomsId + ')" value="' + HPISymptomsId + '" refvalue="" subcharacteristicexist=" "><td class="text-center">' +
                    '<input type="radio" class="MK" symptomtype="1" id="chk' + HPISymptomsId + '" name="SymptomsType' + HPISymptomsId + '" onclick="HPITemplateDetail.ManageFindings(' + HPISymptomsId + ', this,event);"></td><td class="text-center"><input type="radio" class="MK" symptomtype="2" id="chk' + HPISymptomsId + '" name="SymptomsType' + HPISymptomsId + '" onclick="HPITemplateDetail.ManageFindings(' + HPISymptomsId + ', this,event);"></td><td><label id="lblName' + HPISymptomsId + '" class="" data-toggle="tooltip" title="" data-original-title="' + SymptomName + '">' + SymptomName + '</label>'
               + '<a href="#"><span class="removeIconListHover pull-right" onclick="HPITemplateDetail.removeSymptoms(' + HPISymptomsId + ', event)";><i class="fa fa-close"></i></span></a></td></tr>';

        }
        return tr;


        //var li = '<li id="' + PESystemId + '" parentid="null" onclick="PhysicalExamTemplateDetailRevamp.loadObservations(' + PESystemId + ')" value="' + PESystemId + '" refvalue="" subcharacteristicexist=" " class=""><div class="checkbox-custom checkboxTiny checkbox-success">' +
        //        '<input type="checkbox" id="chk' + PESystemId + '" name="' + PESystemId + '" class="pull-left  char" onclick="PhysicalExamTemplateDetailRevamp.ManageObservations(' + PESystemId + ', this);"><label id="lblName' + PESystemId + '" class="" data-toggle="tooltip" title="" data-original-title="' + SystemName + '">' + SystemName + '</label>
        //        <div id="divNameDetail' + PESystemId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" id="txtName' + PESystemId + '" onkeypress="" name="Name' + PESystemId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' +
        //        PESystemId + '" onclick="" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div><a href="#"><span class="removeIconListHover" onclick="PhysicalExamTemplateDetailRevamp.removeSystem(' + PESystemId + ', event)";><i class="fa fa-close"></i></span></a></li>';


    },

    loadFindings: function (HPISymptomId) {

        if ($('#' + HPITemplateDetail.params.PanelID + ' #ulHPISymptomFindings li').length > 0) {
            var prevSymptomId = $('#' + HPITemplateDetail.params.PanelID + ' #ulHPISymptoms tr.active').attr('id');
            var prevFindingId = $('#' + HPITemplateDetail.params.PanelID + ' #ulHPISymptomFindings li.active').attr('id');
            HPITemplateDetail.addFindingsDetails(prevSymptomId, prevFindingId);
        }

        $("#ulHPISymptomFindings").show();
        var isExist = false;

        if (HPITemplateDetail.selectedFindings) {
            for (var i = 0 ; i < HPITemplateDetail.selectedFindings.length; i++) {
                if (HPITemplateDetail.selectedFindings[i].HPISymptomId == HPISymptomId) {
                    isExist = true;
                    break;
                }
            }
        }

        if (!isExist) {
            HPITemplateDetail.HPISymptomFindingsLoad(HPISymptomId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.HPIFindingCount > 0) {
                        var resSymptom = response.HPIFinding_JSON;

                        $("#SymptomFindings").removeClass('hidden');
                        // $("#findingContent div").hide();
                        $("#ulHPISymptomFindings li").remove();

                        $("#ulHPISymptoms tr").removeClass('active');
                        $("#ulHPISymptoms tr[id=" + HPISymptomId + "]").addClass('active');
                        var symptomOrder = $("#ulHPISymptoms tr[id=" + HPISymptomId + "]").attr('symptomorder');
                         
                        var selectAll = '<li id="chkboxSelectAllFindings" parentid="' + HPISymptomId + '" value="Select All" refvalue="Select All"><div class="pl-lg checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="chkboxSelectAllFindings" name="Select All" class="pull-left" '
                            + 'onclick="HPITemplateDetail.selectAllFindings(this);"><label id="lblSelectAll" class="" data-toggle="tooltip" title="" data-original-title="Select All">Select All</label><div class="clearfix"></div><div class="clearfix"></div></div></li>';
                        $("#ulHPISymptomFindings").append(selectAll);

                        $.each(resSymptom, function (i, item) {
                            var li = HPITemplateDetail.addFindings(item.HPIFindingId, item.FindingName, HPISymptomId, item.FindingOrder);
                            $("#ulHPISymptomFindings").append(li);

                            var objselectedFindings = {
                                HPISymptomId: HPISymptomId, SymptomOrder:symptomOrder, IsChecked: false, FindingId: item.HPIFindingId, FindingName: item.FindingName, IsSymptomChecked: false,
                                SymptomDescription: "", IsSymptomDeleted: 0, IsFindingDeleted: 0, HPISymptomsAnswersId: null, IsSymptomsDetail: false, HPISymptoms_SeverityId: 0, HPISymptoms_LocationIds: '', HPISymptoms_RadiationId: 0,
                                HPISymptoms_QualityId: 0, Onset: '', AssociatedWith: '', HPISymptoms_AggravatedById: 0, HPISymptoms_RelievedById: 0, FindingOrder: item.FindingOrder
                            };
                            HPITemplateDetail.selectedFindings.push(objselectedFindings);

                        });
                    }
                    else {
                        $("#ulHPISymptomFindings li").remove();
                        $("#ulHPISymptoms tr").removeClass('active');
                        $("#ulHPISymptoms tr[id=" + HPISymptomId + "]").addClass('active');
                        var symptomOrder = $("#ulHPISymptoms tr[id=" + HPISymptomId + "]").attr('symptomorder');

                        var symtomType = $('#' + HPITemplateDetail.params.PanelID + ' #ulHPISymptoms tr.active').find('input:checked').attr("symptomtype");
                        if (symtomType == null || symtomType == "2") {
                            $('#' + HPITemplateDetail.params.PanelID + ' #ulHPISymptomFindings').addClass('disableAll');
                            $('#' + HPITemplateDetail.params.PanelID + ' #sectionFindingDetails').addClass('disableAll');
                        }
                        else {
                            $('#' + HPITemplateDetail.params.PanelID + ' #ulHPISymptomFindings').removeClass('disableAll');
                            $('#' + HPITemplateDetail.params.PanelID + ' #sectionFindingDetails').removeClass('disableAll');

                        }

                        var objselectedFindings = {
                            HPISymptomId: HPISymptomId, SymptomOrder: symptomOrder, IsChecked: false, FindingId: -1, FindingName: '', IsSymptomChecked: false,
                            SymptomDescription: "", IsSymptomDeleted: 0, IsFindingDeleted: 0, HPISymptomsAnswersId: null, IsSymptomsDetail: true, HPISymptoms_SeverityId: 0, HPISymptoms_LocationIds: '', HPISymptoms_RadiationId: 0,
                            HPISymptoms_QualityId: 0, Onset: '', AssociatedWith: '', HPISymptoms_AggravatedById: 0, HPISymptoms_RelievedById: 0, FindingOrder: -1
                        };
                        HPITemplateDetail.selectedFindings.push(objselectedFindings);
                    }
                }
            });
        }
        else {
            $("#SymptomFindings").removeClass('hidden');
            $("#ulHPISymptomFindings li").remove();

            var selectAll = '<li id="chkboxSelectAllFindings" parentid="' + HPISymptomId + '" value="Select All" refvalue="Select All"><div class="pl-lg checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="chkboxSelectAllFindings" name="Select All" class="pull-left" '
                        + 'onclick="HPITemplateDetail.selectAllFindings(this);"><label id="lblSelectAll" class="" data-toggle="tooltip" title="" data-original-title="Select All">Select All</label><div class="clearfix"></div><div class="clearfix"></div></div></li>';

            if (HPITemplateDetail.selectedFindings) {
                //$("#findingContent").text('');
                for (var i = 0 ; i < HPITemplateDetail.selectedFindings.length; i++) {
                    if (HPITemplateDetail.selectedFindings[i].HPISymptomId == HPISymptomId) {

                        if (HPITemplateDetail.selectedFindings[i].IsSymptomsDetail == false || HPITemplateDetail.selectedFindings[i].IsSymptomsDetail == null) {
                            var li = HPITemplateDetail.addFindings(HPITemplateDetail.selectedFindings[i].FindingId, HPITemplateDetail.selectedFindings[i].FindingName, HPISymptomId, HPITemplateDetail.selectedFindings[i].FindingOrder);

                            if (HPITemplateDetail.selectedFindings[i].FindingId != "" && HPITemplateDetail.selectedFindings[i].FindingId != "0") {
                                if ($('#ulHPISymptomFindings li').find('#chkboxSelectAllFindings').length == 0) {
                                    $("#ulHPISymptomFindings").append(selectAll);
                                }
                                $("#ulHPISymptomFindings").append(li);
                            }

                            $("#ulHPISymptoms tr").removeClass('active');
                            $("#ulHPISymptoms tr[id=" + HPISymptomId + "]").addClass('active');

                            var symtomType = $('#' + HPITemplateDetail.params.PanelID + ' #ulHPISymptoms tr.active').find('input:checked').attr("symptomtype");
                            if (symtomType == "1") {
                                $('#' + HPITemplateDetail.params.PanelID + ' #ulHPISymptomFindings').removeClass('disableAll');
                                $('#' + HPITemplateDetail.params.PanelID + ' #sectionFindingDetails').removeClass('disableAll');
                            }
                            else {
                                $('#' + HPITemplateDetail.params.PanelID + ' #ulHPISymptomFindings').addClass('disableAll');
                                $('#' + HPITemplateDetail.params.PanelID + ' #sectionFindingDetails').addClass('disableAll');
                            }

                            if (HPITemplateDetail.selectedFindings[i].IsChecked) {
                                if (HPITemplateDetail.selectedFindings[i].IsFindingDeleted == 0) {
                                    $("#ulHPISymptomFindings #chk" + HPITemplateDetail.selectedFindings[i].FindingId).prop("checked", true);
                                    $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPITemplateDetail.selectedFindings[i].FindingId).show();
                                    HPITemplateDetail.selectedFindings[i].IsFindingDeleted = 0;
                                    HPITemplateDetail.selectedFindings[i].IsSymptomDeleted = 0;
                                    // HPITemplateDetail.handleDelimiter(HPISymptomId, HPITemplateDetail.selectedFindings[i].FindingId, HPITemplateDetail.selectedFindings[i].FindingName, true);
                                }
                                else {
                                    $("#ulHPISymptomFindings #chk" + HPITemplateDetail.selectedFindings[i].FindingId).prop("checked", false);
                                    HPITemplateDetail.selectedFindings[i].IsFindingDeleted = 0;
                                    HPITemplateDetail.selectedFindings[i].IsSymptomDeleted = 0;
                                    HPITemplateDetail.selectedFindings[i].IsChecked = false;
                                }

                                var noOfFindings =  $.grep(HPITemplateDetail.selectedFindings, function (item, index) {
                                    return item.HPISymptomId == HPITemplateDetail.selectedFindings[i].HPISymptomId;
                                });
                                if (noOfFindings.length == $("#ulHPISymptomFindings li").find('input:checked').length) {
                                    $("#chkboxSelectAllFindings").find('input:checkbox').prop('checked', true);
                                }
                                else {
                                    $("#chkboxSelectAllFindings").find('input:checkbox').prop('checked', false);
                                }
                            }
                            else {
                                $("#ulHPISymptomFindings #chk" + HPITemplateDetail.selectedFindings[i].FindingId).prop("checked", false);
                                HPITemplateDetail.selectedFindings[i].IsFindingDeleted = 0;
                                HPITemplateDetail.selectedFindings[i].IsSymptomDeleted = 0;
                                HPITemplateDetail.selectedFindings[i].IsChecked = false;
                            }
                        }
                        else {
                            if ($('#ulHPISymptomFindings li').length == 0) {
                                $("#ulHPISymptoms tr").removeClass('active');
                                $("#ulHPISymptoms tr[id=" + HPISymptomId + "]").addClass('active');

                                var symtomType = $('#' + HPITemplateDetail.params.PanelID + ' #ulHPISymptoms tr.active').find('input:checked').attr("symptomtype");
                                if (symtomType == null || symtomType == "2") {
                                    $('#' + HPITemplateDetail.params.PanelID + ' #ulHPISymptomFindings').addClass('disableAll');
                                    $('#' + HPITemplateDetail.params.PanelID + ' #sectionFindingDetails').addClass('disableAll');
                                }
                                else {
                                    $('#' + HPITemplateDetail.params.PanelID + ' #ulHPISymptomFindings').removeClass('disableAll');
                                    $('#' + HPITemplateDetail.params.PanelID + ' #sectionFindingDetails').removeClass('disableAll');

                                }

                            }
                            HPITemplateDetail.loadFindingsDetails(HPISymptomId, HPITemplateDetail.selectedFindings[i].FindingId);
                        }
                    }
                }
            }
        }

    },

    HPISymptomFindingsLoad: function (HPISymptomId) {
        var objData = new Object();
        objData["HPISymptomId"] = HPISymptomId;
        objData["commandType"] = "Load_HPI_Symptom_Findings";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPI");
    },

    ManageFindings: function (HPISymptomId, obj, event) {

        //event.stopPropagation();
        
        $('#' + HPITemplateDetail.params.PanelID + ' #sectionFindingDetails').resetAllControls(null);
        $('#' + HPITemplateDetail.params.PanelID + " ddlLocation option").removeAttr("selected");
        $('#' + HPITemplateDetail.params.PanelID + ' #ddlLocation').multiselect('clearSelection');
        $('#' + HPITemplateDetail.params.PanelID + ' #ddlLocation').multiselect("refresh");

        $("#ulHPISymptoms tr").removeClass('active');
        $("#ulHPISymptoms tr[id=" + HPISymptomId + "]").addClass('active');

        $('#' + HPITemplateDetail.params.PanelID + ' #sectionFindingDetails').removeClass('disableAll');

        var symptomAnswerType = $(obj).attr('symptomtype');

        if (!$(obj).is(':checked')) {
            $(obj).parent().parent().removeClass('active');

            //for (var i = 0; i < $("#ulHPISymptomFindings li").length; i++)
            //{
                $("#ulHPISymptomFindings li").find(".MK").prop('checked', false);
            //}
            if (HPITemplateDetail.selectedFindings) {
                for (var i = 0 ; i < HPITemplateDetail.selectedFindings.length; i++)
                {
                    if (HPITemplateDetail.selectedFindings[i].HPISymptomId == HPISymptomId) {
                        HPITemplateDetail.selectedFindings[i].IsSymptomChecked = false;
                        HPITemplateDetail.selectedFindings[i].IsChecked = false;
                        HPITemplateDetail.selectedFindings[i].SymptomDescription = $('#findingContent #divSymptom' + HPISymptomId).text();
                        HPITemplateDetail.selectedFindings[i].HPISymptomsAnswersId = symptomAnswerType;
                    }
                }
            }
        }
        else {
            if (HPITemplateDetail.selectedFindings) {
                for (var i = 0 ; i < HPITemplateDetail.selectedFindings.length; i++) {
                    if (HPITemplateDetail.selectedFindings[i].HPISymptomId == HPISymptomId) {
                        HPITemplateDetail.selectedFindings[i].IsSymptomChecked = true;
                        HPITemplateDetail.selectedFindings[i].SymptomDescription = $('#findingContent #divSymptom' + HPISymptomId).text();
                        HPITemplateDetail.selectedFindings[i].HPISymptomsAnswersId = symptomAnswerType;

                    }
                }
            }
        }

        if (symptomAnswerType == "2") { // 'Denies'
            $("#ulHPISymptomFindings").addClass('disableAll');
            $('#' + HPITemplateDetail.params.PanelID + ' #sectionFindingDetails').addClass('disableAll');
            if (HPITemplateDetail.selectedFindings) {
                for (var i = 0 ; i < HPITemplateDetail.selectedFindings.length; i++) {
                    if (HPITemplateDetail.selectedFindings[i].HPISymptomId == HPISymptomId) {
                        HPITemplateDetail.selectedFindings[i].IsSymptomChecked = true;
                        HPITemplateDetail.selectedFindings[i].IsChecked = false;
                        HPITemplateDetail.selectedFindings[i].SymptomDescription = $('#findingContent #divSymptom' + HPISymptomId).text();
                        HPITemplateDetail.selectedFindings[i].HPISymptomsAnswersId = symptomAnswerType;
                    }
                }
            }
            $("#ulHPISymptomFindings input").prop('checked', false);
        }
        else {
            $("#ulHPISymptomFindings").removeClass('disableAll');
            $('#' + HPITemplateDetail.params.PanelID + ' #sectionFindingDetails').removeClass('disableAll');

        }

        $("#SymptomPreview").removeClass('hidden');
        var symptomName = $("#ulHPISymptoms tr[id=" + HPISymptomId + "] label").text();
        var deli = $("#delimator option:selected").text() + " ";

        if ($("#findingContent #divSymptom" + HPISymptomId).length > 0) {
            // $('#findingContent #divSymptom' + HPISymptomId).remove();
            //var $newDiv = $("<div></div>");
            //$newDiv.attr("id", "divSymptom" + HPISymptomId);
            //$newDiv.attr("style", "display: inline;");

            //$("#findingContent").append($newDiv);
            // $('#findingContent #divSymptom' + HPISymptomId).show();
            var txttoAppendType = '';
            var textAdded = $("#findingContent #divSymptom" + HPISymptomId).text();
            if (symptomAnswerType == "1") {
                if (textAdded.toLowerCase().indexOf('c/o') > -1) {
                    HPITemplateDetail.UnselectSymptom(HPISymptomId);
                    $(obj).prop('checked', false);
                }
                else {
                    txttoAppendType = 'C/O ' + symptomName + ' ';
                    $("#findingContent #divSymptom" + HPISymptomId).attr('style', 'display:inline');
                    $("#findingContent #divSymptom" + HPISymptomId).text(txttoAppendType);
                    $("#ulHPISymptomFindings input").prop('checked', false);

                    if (HPITemplateDetail.selectedFindings) {
                        for (var i = 0 ; i < HPITemplateDetail.selectedFindings.length; i++) {
                            if (HPITemplateDetail.selectedFindings[i].HPISymptomId == HPISymptomId) {                                
                                HPITemplateDetail.selectedFindings[i].IsChecked = false;                               
                            }
                        }
                    }
                }                
            }
            else {

                if (textAdded.toLowerCase().indexOf('denies') > -1) {
                    HPITemplateDetail.UnselectSymptom(HPISymptomId);
                    $(obj).prop('checked', false);
                }
                else {

                    txttoAppendType = 'Denies: ' + symptomName;

                    if (HPITemplateDetail.selectedFindings) {
                        for (var i = 0 ; i < HPITemplateDetail.selectedFindings.length; i++) {
                            if (HPITemplateDetail.selectedFindings[i].HPISymptomId == HPISymptomId) {
                                var findingid = HPITemplateDetail.selectedFindings[i].FindingId;
                                $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + findingid).remove();
                            }
                        }
                    }
                    $("#findingContent #divSymptom" + HPISymptomId).attr('style', '');
                    $("#findingContent #divSymptom" + HPISymptomId).text(txttoAppendType);
                }
                
            }

            //if ($('#findingContent div').length > 1 && symptomAnswerType == "1")
            //    txttoAppendType = deli + txttoAppendType;

            //txttoAppendType = deli + txttoAppendType;
            //$("#findingContent #divSymptom" + HPISymptomId).text(txttoAppendType);

            // $("#findingContent #divSymptom" + HPISymptomId).append(txttoAppendType);
        }
        else {
            var $newDiv = $("<div></div>");
            $newDiv.attr("id", "divSymptom" + HPISymptomId);
            $newDiv.attr("style", "display: inline;");

            $("#findingContent").append('<div></div>');

            $("#findingContent").append($newDiv);
            $('#findingContent #divSymptom' + HPISymptomId).show();
            var txttoAppendType = '';
            if (symptomAnswerType == "1") {
                txttoAppendType = 'C/O ' + symptomName + ' ';
            }
            else {
                txttoAppendType = 'Denies: ' + symptomName;
                if (HPITemplateDetail.selectedFindings) {
                    for (var i = 0 ; i < HPITemplateDetail.selectedFindings.length; i++) {
                        if (HPITemplateDetail.selectedFindings[i].HPISymptomId == HPISymptomId) {
                            var findingid = HPITemplateDetail.selectedFindings[i].FindingId;
                            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + findingid).remove();
                        }
                    }
                }
                $('#findingContent #divSymptom' + HPISymptomId).removeAttr('style');
            }

            //if ($('#findingContent div').length > 1 && symptomAnswerType == "1")
            //    txttoAppendType = deli + txttoAppendType;

            //txttoAppendType = deli + txttoAppendType;

            $("#findingContent #divSymptom" + HPISymptomId).append(txttoAppendType);
        }
    },

    UnselectSymptom: function (HPISymptomId) {
            $("#ulHPISymptomFindings").addClass('disableAll');
            $('#' + HPITemplateDetail.params.PanelID + ' #sectionFindingDetails').addClass('disableAll');
            $("#ulHPISymptomFindings input").prop('checked', false);

            if (HPITemplateDetail.selectedFindings) {
                for (var i = 0 ; i < HPITemplateDetail.selectedFindings.length; i++) {
                    if (HPITemplateDetail.selectedFindings[i].HPISymptomId == HPISymptomId) {
                        HPITemplateDetail.selectedFindings[i].IsSymptomChecked = false;
                        HPITemplateDetail.selectedFindings[i].IsChecked = false;
                        HPITemplateDetail.selectedFindings[i].SymptomDescription = '';
                        HPITemplateDetail.selectedFindings[i].HPISymptomsAnswersId = null;
                    }
                }
            }
            $("#findingContent #divSymptom" + HPISymptomId).remove();
    },
    //ManageFindings: function (HPISymptomId, obj, event) {

    //    //event.stopPropagation();

    //    $("#ulHPISymptoms tr").removeClass('active');
    //    $("#ulHPISymptoms tr[id=" + HPISymptomId + "]").addClass('active');

    //    $('#' + HPITemplateDetail.params.PanelID + ' #sectionFindingDetails').removeClass('disableAll');

    //    var symptomAnswerType = $(obj).attr('symptomtype');

    //    if (!$(obj).is(':checked')) {
    //        $(obj).parent().parent().removeClass('active');

    //        for (var i = 0; i < $("#ulHPISymptomFindings li").length; i++) {
    //            $("#ulHPISymptomFindings #chk" + $("#ulHPISymptomFindings li")[i].id).prop('checked', false);
    //        }
    //        if (HPITemplateDetail.selectedFindings) {
    //            for (var i = 0 ; i < HPITemplateDetail.selectedFindings.length; i++) {
    //                if (HPITemplateDetail.selectedFindings[i].HPISymptomId == HPISymptomId) {
    //                    HPITemplateDetail.selectedFindings[i].IsSymptomChecked = false;
    //                    HPITemplateDetail.selectedFindings[i].IsChecked = false;
    //                    HPITemplateDetail.selectedFindings[i].SymptomDescription = $('#findingContent #divSymptom' + HPISymptomId).text();
    //                    HPITemplateDetail.selectedFindings[i].HPISymptomsAnswersId = symptomAnswerType;
    //                }
    //            }
    //        }
    //    }
    //    else {
    //        if (HPITemplateDetail.selectedFindings) {
    //            for (var i = 0 ; i < HPITemplateDetail.selectedFindings.length; i++) {
    //                if (HPITemplateDetail.selectedFindings[i].HPISymptomId == HPISymptomId) {
    //                    HPITemplateDetail.selectedFindings[i].IsSymptomChecked = true;
    //                    HPITemplateDetail.selectedFindings[i].SymptomDescription = $('#findingContent #divSymptom' + HPISymptomId).text();
    //                    HPITemplateDetail.selectedFindings[i].HPISymptomsAnswersId = symptomAnswerType;

    //                }
    //            }
    //        }
    //    }

    //    if (symptomAnswerType == "2") { // 'Denies'
    //        $("#ulHPISymptomFindings").addClass('disableAll');
    //        $('#' + HPITemplateDetail.params.PanelID + ' #sectionFindingDetails').addClass('disableAll');
    //        if (HPITemplateDetail.selectedFindings) {
    //            for (var i = 0 ; i < HPITemplateDetail.selectedFindings.length; i++) {
    //                if (HPITemplateDetail.selectedFindings[i].HPISymptomId == HPISymptomId) {
    //                    HPITemplateDetail.selectedFindings[i].IsSymptomChecked = true;
    //                    HPITemplateDetail.selectedFindings[i].IsChecked = false;
    //                    HPITemplateDetail.selectedFindings[i].SymptomDescription = $('#findingContent #divSymptom' + HPISymptomId).text();
    //                    HPITemplateDetail.selectedFindings[i].HPISymptomsAnswersId = symptomAnswerType;
    //                }
    //            }
    //        }
    //        $("#ulHPISymptomFindings input").prop('checked', false);
    //    }
    //    else {
    //        $("#ulHPISymptomFindings").removeClass('disableAll');
    //        $('#' + HPITemplateDetail.params.PanelID + ' #sectionFindingDetails').removeClass('disableAll');

    //    }

    //    $("#SymptomPreview").removeClass('hidden');
    //    var symptomName = $("#ulHPISymptoms tr[id=" + HPISymptomId + "] label").text();
    //    var deli = $("#delimator option:selected").text() + " ";

    //    if ($("#findingContent #divSymptom" + HPISymptomId).length > 0) {
    //        // $('#findingContent #divSymptom' + HPISymptomId).remove();
    //        //var $newDiv = $("<div></div>");
    //        //$newDiv.attr("id", "divSymptom" + HPISymptomId);
    //        //$newDiv.attr("style", "display: inline;");

    //        //$("#findingContent").append($newDiv);
    //        // $('#findingContent #divSymptom' + HPISymptomId).show();
    //        var txttoAppendType = '';
    //        if (symptomAnswerType == "1") {
    //            txttoAppendType = 'C/O ' + symptomName + ' ';
    //            $("#findingContent #divSymptom" + HPISymptomId).attr('style', 'display:inline');
    //        }
    //        else {

    //            txttoAppendType = 'Denies: ' + symptomName;

    //            if (HPITemplateDetail.selectedFindings) {
    //                for (var i = 0 ; i < HPITemplateDetail.selectedFindings.length; i++) {
    //                    if (HPITemplateDetail.selectedFindings[i].HPISymptomId == HPISymptomId) {
    //                        var findingid = HPITemplateDetail.selectedFindings[i].FindingId;
    //                        $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + findingid).remove();
    //                    }
    //                }
    //            }
    //            $("#findingContent #divSymptom" + HPISymptomId).attr('style', '');
    //        }

    //        //if ($('#findingContent div').length > 1 && symptomAnswerType == "1")
    //        //    txttoAppendType = deli + txttoAppendType;

    //        //txttoAppendType = deli + txttoAppendType;
    //        $("#findingContent #divSymptom" + HPISymptomId).text(txttoAppendType);

    //        // $("#findingContent #divSymptom" + HPISymptomId).append(txttoAppendType);
    //    }
    //    else {
    //        var $newDiv = $("<div></div>");
    //        $newDiv.attr("id", "divSymptom" + HPISymptomId);
    //        $newDiv.attr("style", "display: inline;");

    //        $("#findingContent").append('<div></div>');

    //        $("#findingContent").append($newDiv);
    //        $('#findingContent #divSymptom' + HPISymptomId).show();
    //        var txttoAppendType = '';
    //        if (symptomAnswerType == "1") {
    //            txttoAppendType = 'C/O ' + symptomName + ' ';
    //        }
    //        else {
    //            txttoAppendType = 'Denies: ' + symptomName;
    //            if (HPITemplateDetail.selectedFindings) {
    //                for (var i = 0 ; i < HPITemplateDetail.selectedFindings.length; i++) {
    //                    if (HPITemplateDetail.selectedFindings[i].HPISymptomId == HPISymptomId) {
    //                        var findingid = HPITemplateDetail.selectedFindings[i].FindingId;
    //                        $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + findingid).remove();
    //                    }
    //                }
    //            }
    //            $('#findingContent #divSymptom' + HPISymptomId).removeAttr('style');
    //        }

    //        //if ($('#findingContent div').length > 1 && symptomAnswerType == "1")
    //        //    txttoAppendType = deli + txttoAppendType;

    //        //txttoAppendType = deli + txttoAppendType;

    //        $("#findingContent #divSymptom" + HPISymptomId).append(txttoAppendType);
    //    }
    //},
    addFindings: function (HPIFindingId, FindingName, HPISymptomId,findingOrder) {
        var a = HPITemplateDetail.selectedFindings;

        var itemtoRemove = "finding";

        var li = '<li findingOrder="' + findingOrder + '" id="' + HPIFindingId + '" parentid="' + HPISymptomId + '" onclick="HPITemplateDetail.PreviewFindings(' + HPIFindingId + ',\'' + FindingName + '\', this, ' + HPISymptomId + ');" value="' + HPIFindingId + '" refvalue="" subcharacteristicexist=" " class=""><div class="checkbox-custom checkboxTiny checkbox-success">' +
                 '<input type="checkbox" id="chk' + HPIFindingId + '" name="' + HPIFindingId + '" class="pull-left  char" ' +
                 'onclick="HPITemplateDetail.PreviewFindings(' + HPIFindingId + ',\'' + FindingName + '\', this, ' + HPISymptomId + ');"><label id="lblName' + HPIFindingId + '" class="" data-toggle="tooltip" title="" data-original-title="' + FindingName + '">' + FindingName + '</label><div id="divNameDetail' + HPIFindingId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" spellcheck="true" id="txtName' + HPIFindingId + '" onkeypress="" name="Name' + HPIFindingId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' +
                 HPIFindingId + '" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div><a href="#"><span class="removeIconListHover" onclick="HPITemplateDetail.removeFinding(' + HPISymptomId + ', ' + HPIFindingId + ')"><i class="fa fa-close"></i></span></a></li>';
        return li;
    },

    removeSymptoms: function (HPISymptomId, event) {
        $("#ulHPISymptoms #" + HPISymptomId).remove();
        $("#ulHPISymptomFindings").hide();
        if (HPITemplateDetail.selectedFindings) {
            if (HPITemplateDetail.params["mode"] == "Add") {
                HPITemplateDetail.selectedFindings = $.grep(HPITemplateDetail.selectedFindings, function (e) {
                    return e.HPISymptomId != HPISymptomId;
                });             
            }
            else {
                for (var i = 0 ; i < HPITemplateDetail.selectedFindings.length; i++) {
                    if (HPITemplateDetail.selectedFindings[i].HPISymptomId == HPISymptomId) {
                        HPITemplateDetail.selectedFindings[i].IsSymptomDeleted = 1;
                        HPITemplateDetail.selectedFindings[i].IsFindingDeleted = 1;
                        HPITemplateDetail.selectedFindings[i].IsChecked = false;
                        HPITemplateDetail.selectedFindings[i].IsSymtomChecked = false;
                        HPITemplateDetail.selectedFindings[i].SymptomDescription = "";
                    }
                }
           }
        }

        $('#findingContent #divSymptom' + HPISymptomId).remove();
        event.stopPropagation();
    },

    removeFinding: function (HPISymptomId, HPIFindingId) {
        $("#ulHPISymptomFindings #" + HPIFindingId).remove();
        $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).remove();
        HPITemplateDetail.removeLastDelimiter(HPISymptomId);

        if (HPITemplateDetail.selectedFindings) {
            for (var i = 0 ; i < HPITemplateDetail.selectedFindings.length; i++) {
                if (HPITemplateDetail.selectedFindings[i].HPISymptomId == HPISymptomId && HPITemplateDetail.selectedFindings[i].FindingId == HPIFindingId) {
                    HPITemplateDetail.selectedFindings[i].IsFindingDeleted = 1;
                    HPITemplateDetail.selectedFindings[i].IsChecked = false;
                    HPITemplateDetail.selectedFindings[i].SymptomDescription = $('#findingContent #divSymptom' + HPISymptomId).text();
                }
            }
        }
    },

    selectAllFindings: function (obj) {

        if (obj) var symptomId = $($($(obj).parent().parent())[0]).attr('parentid');

            $("#findingContent #divSymptom" + symptomId + 'ddlSeverity').remove();
            $("#findingContent #divSymptom" + symptomId + 'ddlLocation').remove();
            $("#findingContent #divSymptom" + symptomId + 'ddlRadiation').remove();
            $("#findingContent #divSymptom" + symptomId + 'ddlQuality').remove();
            $("#findingContent #divSymptom" + symptomId + 'txtOnset').remove();
            $("#findingContent #divSymptom" + symptomId + 'txtAssociatedWith').remove();
            $("#findingContent #divSymptom" + symptomId + 'ddlAggravatedBy').remove();
            $("#findingContent #divSymptom" + symptomId + 'ddlRevieledBy').remove();


        if ($(obj).prop('checked') == true) {
            $("#SymptomPreview").removeClass('hidden');
            $('#' + HPITemplateDetail.params.PanelID + ' #ulHPISymptomFindings li .char').each(function () {
                if ($(this).attr("id") != $(obj).attr("id") && $(this).prop("checked") == false) {
                    $(this).parent().find("[type='checkbox'][id*='chk']").prop("checked", true);

                    var id_ = $(this).parent().find("[type='checkbox'][id*='chk']").attr('id').substring(3)
                    var findingName = $("#divHPISymptomFindings #lblName" + id_).text();
                    var delimator = $("#delimator option:selected").text() + " ";

                    if ($("#findingContent #divSymptom" + symptomId + 'Finding' + id_).length > 0) {
                        $('#findingContent #divSymptom' + symptomId + 'Finding' + id_).remove();
                        var $newDiv = $("<div></div>");
                        $newDiv.attr("id", "divSymptom" + symptomId + 'Finding' + id_);
                        $newDiv.attr("style", "display: inline;");

                        $("#findingContent").find("div[id='divSymptom" +symptomId + "']").append($newDiv);
                        $('#findingContent #divSymptom' + symptomId + 'Finding' + id_).show();
                        var txttoAppend = findingName;

                        if ($("#findingContent").find("div[id='divSymptom" + symptomId + "']").length > 1)
                            txttoAppend = findingName + delimator;

                        $("#findingContent #divSymptom" + symptomId + 'Finding' + id_).append(txttoAppend);
                    }
                    else {
                        var $newDiv = $("<div></div>");
                        $newDiv.attr("id", "divSymptom" + symptomId + 'Finding' + id_);
                        $newDiv.attr("style", "display: inline;");

                        var txttoAppend = findingName;

                        if ($('#findingContent div').length > 1)
                            txttoAppend = findingName + delimator;

                        $("#findingContent").find("div[id='divSymptom" +symptomId + "']").append($newDiv);

                      //  $("#findingContent").find("div[id^='divSymptom" + symptomId + "']").last().after($newDiv);

                        $('#findingContent #divSymptom' + symptomId + 'Finding' + id_).show();
                        //var txttoAppend = findingName;

                        //if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length > 1)
                        //    txttoAppend = deli + findingName;

                        $("#findingContent #divSymptom" + symptomId + 'Finding' + id_).append(txttoAppend);

                        //$("#findingContent").append($newDiv);
                        //$('#findingContent #divSymptom' + symptomId + id_).show();

                        //var txttoAppend = findingName;
                        //if ($('#findingContent div').length > 1)
                        //    txttoAppend = findingName + delimator;

                        //$("#findingContent #divSymptom" + symptomId + id_).append(txttoAppend);
                    }

                    //$("#ulHPISymptoms #chk" + symptomId).prop("checked", true);
                }
            });

            if (HPITemplateDetail.selectedFindings) {
                for (var i = 0 ; i < HPITemplateDetail.selectedFindings.length; i++) {
                    if (HPITemplateDetail.selectedFindings[i].HPISymptomId == symptomId) {
                        HPITemplateDetail.selectedFindings[i].IsChecked = true;
                        HPITemplateDetail.selectedFindings[i].IsSymptomChecked = true;
                        HPITemplateDetail.selectedFindings[i].SymptomDescription = $('#findingContent #divSymptom' + symptomId).text();
                    }
                }
            }
        }
        else if ($(obj).prop('checked') == false) {
            $('#' + HPITemplateDetail.params.PanelID + ' #ulHPISymptomFindings li .char').removeClass("green");
            $('#' + HPITemplateDetail.params.PanelID + ' #ulHPISymptomFindings li .char').each(function () {
                if ($(this).attr("id") != $(obj).attr("id")) {
                    $(this).parent().find("[type='checkbox'][id*='chk']").prop("checked", false);
                    var id_ = $(this).parent().find("[type='checkbox'][id*='chk']").attr('id').substring(3);
                    var symptom_id = $($($(obj).parent().parent())[0]).attr('parentid');
                    $("#findingContent #divSymptom" + symptom_id + 'Finding' + id_).remove();
                }
            });

            if (HPITemplateDetail.selectedFindings) {
                for (var i = 0 ; i < HPITemplateDetail.selectedFindings.length; i++) {
                    if (HPITemplateDetail.selectedFindings[i].HPISymptomId == symptomId) {
                        HPITemplateDetail.selectedFindings[i].IsChecked = false;
                        HPITemplateDetail.selectedFindings[i].SymptomDescription = $('#observationContent #divSystem' + symptomId).text();
                    }
                }
            }
        }
        HPITemplateDetail.removeLastDelimiter(symptomId);
    },

    removeLastDelimiter: function (HPISymptomId) {

        var delii = $("#delimator option:selected").text();
        var str = "";
        if (delii == ",") str = $($('#findingContent div[id^=divSymptom' + HPISymptomId + ']')[0]).text().replace(/,/g, "");
        if (delii == ".") str = $($('#findingContent div[id^=divSymptom' + HPISymptomId + ']')[0]).text().replace(/./g, "");
        if (delii == ":") str = $($('#findingContent div[id^=divSymptom' + HPISymptomId + ']')[0]).text().replace(/:/g, "");
        if (delii == ";") str = $($('#findingContent div[id^=divSymptom' + HPISymptomId + ']')[0]).text().replace(/;/g, "");
        if (delii == "-") str = $($('#findingContent div[id^=divSymptom' + HPISymptomId + ']')[0]).text().replace(/-/g, "");

        var id = $($('#findingContent div')[0]).attr('id');
        //  $("#findingContent #" + id).text(str);
    },

    PreviewFindings: function (findingId, findingName, obj, HPISymptomId) {

        var prevFinding = $('#' + HPITemplateDetail.params.PanelID + ' #ulHPISymptomFindings li.active').attr('id');
        if (prevFinding) {
            HPITemplateDetail.addFindingsDetails(HPISymptomId, prevFinding);
        }
        $('#' + HPITemplateDetail.params.PanelID + ' #sectionFindingDetails').removeClass('disableAll');
        HPITemplateDetail.loadFindingsDetails(HPISymptomId, findingId);

        $("#SymptomPreview").removeClass('hidden');
        $('#' + HPITemplateDetail.params.PanelID + ' #ulHPISymptomFindings li').removeClass('active');
        $(obj).addClass('active');

        if ($(obj).prop('checked') == true) {
            $(this).find("[type='checkbox'][id*='chk']").prop("checked", true);
        }
        else if ($(obj).prop('checked') == false) {
            $('#' + HPITemplateDetail.params.PanelID + ' #ulHPISymptomFindings li .char').removeClass("green");
            $(this).find("[type='checkbox'][id*='chk']").prop("checked", false);
        }

        var isChk = $(obj).prop('checked') == true ? true : false;

        var objSelectedFindings =
        {
            HPISymptomId: HPISymptomId,
            IsChecked: isChk,
            FindingId: findingId,
            FindingName: findingName,
            IsModified: '1',
            IsSymptomChecked: false,
            SymptomDescription: ""
        };

        if ($(obj).prop('checked') == true) {

            $("#findingContent #divSymptom" + HPISymptomId + 'ddlSeverity').remove();
            $("#findingContent #divSymptom" + HPISymptomId + 'ddlLocation').remove();
            $("#findingContent #divSymptom" + HPISymptomId + 'ddlRadiation').remove();
            $("#findingContent #divSymptom" + HPISymptomId + 'ddlQuality').remove();
            $("#findingContent #divSymptom" + HPISymptomId + 'txtOnset').remove();
            $("#findingContent #divSymptom" + HPISymptomId + 'txtAssociatedWith').remove();
            $("#findingContent #divSymptom" + HPISymptomId + 'ddlAggravatedBy').remove();
            $("#findingContent #divSymptom" + HPISymptomId + 'ddlRevieledBy').remove();
            
            var deli = $("#delimator option:selected").text() + " ";

            if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + findingId).length > 0) {
                //$('#findingContent #divSymptom' + HPISymptomId + 'Finding' + findingId).remove();
             //   var $newDiv = $("<div></div>");
              //  $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + findingId);
             //   $newDiv.attr("style", "display: inline;");

               // $("#findingContent").append($newDiv);
              //  $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + findingId).show();
                var txttoAppend = findingName;
                //if ($('#findingContent div').length > 1)
                //    txttoAppend = deli + findingName;

                if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length > 1)
                    txttoAppend = findingName + deli;

                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + findingId).append(txttoAppend);
            }
            else {
                var symptomName = '';
                if ($("#findingContent #divSymptom" + HPISymptomId).length == 0) {
                    symptomName = $("#ulHPISymptoms tr[id=" + HPISymptomId + "] label").text();
                }
                var $newDiv = $("<div></div>");
                $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + findingId);
                $newDiv.attr("style", "display: inline;");

                var txttoAppend = findingName;

                if ($('#findingContent div').length > 1)
                    txttoAppend = findingName + deli;

                $("#findingContent").find("div[id='divSymptom" + HPISymptomId + "']").append($newDiv);

               // $("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").last().after($newDiv);

                $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + findingId).show();
                //var txttoAppend = findingName;

                //if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length > 1)
                //    txttoAppend = deli + findingName;

                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + findingId).append(txttoAppend);
            }

            if (HPITemplateDetail.selectedFindings) {
                for (var i = 0 ; i < HPITemplateDetail.selectedFindings.length; i++) {
                    if (HPITemplateDetail.selectedFindings[i].HPISymptomId == HPISymptomId && HPITemplateDetail.selectedFindings[i].FindingId == findingId) {
                        HPITemplateDetail.selectedFindings[i].IsChecked = true;
                        HPITemplateDetail.selectedFindings[i].IsSymptomChecked = true;
                        HPITemplateDetail.selectedFindings[i].SymptomDescription = $('#findingContent #divSymptom' + HPISymptomId).text();
                    }
                    if (HPITemplateDetail.selectedFindings[i].HPISymptomId == HPISymptomId) {
                        HPITemplateDetail.selectedFindings[i].IsSymptomChecked = true;
                        HPITemplateDetail.selectedFindings[i].SymptomDescription = $('#findingContent #divSymptom' + HPISymptomId).text();
                    }
                }
            }

            // $("#ulHPISymptoms #chk" + HPISymptomId).prop("checked", true);
            $("#ulHPISymptoms tr.active input[symptomtype = '1']").prop("checked", true);

            //  HPITemplateDetail.loadFindings(HPISymptomId);
        }
        else if ($(obj).prop('checked') == false) {
            if (HPITemplateDetail.selectedFindings) {
                for (var i = 0 ; i < HPITemplateDetail.selectedFindings.length; i++) {
                    if (HPITemplateDetail.selectedFindings[i].HPISymptomId == HPISymptomId && HPITemplateDetail.selectedFindings[i].FindingId == findingId) {
                        HPITemplateDetail.selectedFindings[i].IsChecked = false;
                        HPITemplateDetail.selectedFindings[i].SymptomDescription = $('#findingContent #divSymptom' + HPISymptomId).text();
                    }
                }
            }
            var aa = $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + findingId).text();
            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + findingId).remove();
            HPITemplateDetail.removeLastDelimiter(HPISymptomId);
        }
        HPITemplateDetail.removeLastDelimiter(HPISymptomId);
    },

    handleDelimiter: function (HPISymptomId, HPIFindingId, FindingName, IsChecked) {

        if (IsChecked) {
            var delimator = $("#delimator option:selected").text() + " ";
            if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).length > 0) {
                $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).remove();
                var $newDiv = $("<div></div>");
                $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId);
                $newDiv.attr("style", "display: inline;");

                $("#findingContent").append($newDiv);
                $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();
                var txtToAppend = FindingName;

                if ($('#findingContent div').length > 1)
                    txtToAppend = delimator + FindingName;

                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append(txtToAppend);
            }
            else {
                var $newDiv = $("<div></div>");
                $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId);
                $newDiv.attr("style", "display: inline;");

                $("#findingContent").append($newDiv);
                $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();
                var txtToAppend = FindingName;

                if ($('#findingContent div').length > 1)
                    txtToAppend = delimator + FindingName;

                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append(txtToAppend);
            }
        }
    },

    IntializeMultiSelectDropDown: function () {
        $('#' + HPITemplateDetail.params.PanelID + ' #sectionFindingDetails #ddlLocation').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247
        });
    },

    addFindingsDetails: function (HPISymptomId, findingId) {

        var LocationIDS = $('#ddlLocation option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        var severity = $("#ddlSeverity option:Selected").val();
        var radiation = $("#ddlRadiation option:Selected").val();
        var quality = $("#ddlQuality option:Selected").val();
        var onset = $("#txtOnset").val();
        var associatedwith = $("#txtAssociatedWith").val();
        var aggravated = $("#ddlAggravatedBy option:Selected").val();
        var relieved = $("#ddlRevieledBy option:Selected").val();

        if (HPITemplateDetail.selectAllFindings) {
            if (findingId) {
                for (var i = 0 ; i < HPITemplateDetail.selectedFindings.length; i++) {
                    if (HPITemplateDetail.selectedFindings[i].HPISymptomId == HPISymptomId) {
                        if (HPITemplateDetail.selectedFindings[i].FindingId == findingId) {
                            HPITemplateDetail.selectedFindings[i].HPISymptoms_SeverityId = severity;
                            HPITemplateDetail.selectedFindings[i].HPISymptoms_LocationIds = LocationIDS;
                            HPITemplateDetail.selectedFindings[i].HPISymptoms_RadiationId = radiation;
                            HPITemplateDetail.selectedFindings[i].HPISymptoms_QualityId = quality;
                            HPITemplateDetail.selectedFindings[i].Onset = onset;
                            HPITemplateDetail.selectedFindings[i].AssociatedWith = associatedwith;
                            HPITemplateDetail.selectedFindings[i].HPISymptoms_AggravatedById = aggravated;
                            HPITemplateDetail.selectedFindings[i].HPISymptoms_RelievedById = relieved;
                            HPITemplateDetail.selectedFindings[i].IsSymptomsDetail = false;
                        }


                        //var indexSymptom = -1;

                        //$.grep(HPITemplateDetail.selectedFindings, function (item, index) {
                        //    if (item.HPISymptomId == HPISymptomId && item.IsSymptomsDetail == true) {
                        //        indexSymptom = index;
                        //        return;
                        //    }
                        //});

                        //if (indexSymptom!= -1) {
                        //    HPITemplateDetail.selectedFindings[indexSymptom].IsSymptomDeleted = 1;
                        //    HPITemplateDetail.selectedFindings[indexSymptom].IsFindingDeleted = 1;
                        //    HPITemplateDetail.selectedFindings[indexSymptom].IsChecked = false;
                        //    HPITemplateDetail.selectedFindings[indexSymptom].IsSymptomChecked = false;
                        //}                 
                    }
                }
            }
            else {
                var prevSymptomId = $("#ulHPISymptoms tr.active").attr('id');
                var symptomorder = $("#ulHPISymptoms tr.active").attr('symptomorder');
                var detailExists = false;
                if (LocationIDS) {
                    detailExists = true;
                }
                if (severity != '0') {
                    detailExists = true;
                }
                if (radiation != '0') {
                    detailExists = true;
                }
                if (quality != '0') {
                    detailExists = true;
                }
                if (onset != '') {
                    detailExists = true;
                }
                if (associatedwith != '') {
                    detailExists = true;
                }
                if (aggravated != '0') {
                    detailExists = true;
                }
                if (relieved != '0') {
                    detailExists = true;
                }

                //var detail = LocationIDS || severity || radiation || quality || onset || associatedwith || aggravated || relieved;
                //var detailExists = parseInt(detail) ? true : false;

                var findings = $('#' + HPITemplateDetail.params.PanelID + ' #ulHPISymptomFindings li').length;


                if (detailExists || findings == 0) {
                    var indexSymptom = -1;

                    $.grep(HPITemplateDetail.selectedFindings, function (item, index) {
                        if (item.HPISymptomId == prevSymptomId && item.IsSymptomsDetail == true) {
                            indexSymptom = index;
                            return;
                        }
                    });
                    if (indexSymptom == -1) {
                        var objSelectedFindings = {
                            HPISymptomId: prevSymptomId, SymptomOrder: symptomorder, IsChecked: false, FindingId: -1, FindingName: '', IsSymptomChecked: true, IsSymptomDeleted: 0, IsFindingDeleted: 0, HPISymptomsAnswersId: $("#ulHPISymptoms tr.active  input:checked").attr('symptomtype'),
                            HPISymptoms_SeverityId: severity, HPISymptoms_LocationIds: LocationIDS, HPISymptoms_RadiationId: radiation, HPISymptoms_QualityId: quality, Onset: onset,
                            AssociatedWith: associatedwith, HPISymptoms_AggravatedById: aggravated, HPISymptoms_RelievedById: relieved, IsSymptomsDetail: true
                        };

                        HPITemplateDetail.selectedFindings.push(objSelectedFindings);
                    }
                    else {
                        HPITemplateDetail.selectedFindings[indexSymptom].FindingId = -1;
                        HPITemplateDetail.selectedFindings[indexSymptom].HPISymptoms_SeverityId = severity;
                        HPITemplateDetail.selectedFindings[indexSymptom].HPISymptoms_LocationIds = LocationIDS;
                        HPITemplateDetail.selectedFindings[indexSymptom].HPISymptoms_RadiationId = radiation;
                        HPITemplateDetail.selectedFindings[indexSymptom].HPISymptoms_QualityId = quality;
                        HPITemplateDetail.selectedFindings[indexSymptom].Onset = onset;
                        HPITemplateDetail.selectedFindings[indexSymptom].AssociatedWith = associatedwith;
                        HPITemplateDetail.selectedFindings[indexSymptom].HPISymptoms_AggravatedById = aggravated;
                        HPITemplateDetail.selectedFindings[indexSymptom].HPISymptoms_RelievedById = relieved;
                        HPITemplateDetail.selectedFindings[indexSymptom].HPISymptomsAnswersId = $("#ulHPISymptoms tr.active  input:checked").attr('symptomtype');
                    }

                    for (var i = 0 ; i < HPITemplateDetail.selectedFindings.length; i++) {
                        if (HPITemplateDetail.selectedFindings[i].HPISymptomId == prevSymptomId) {
                            if (HPITemplateDetail.selectedFindings[i].IsSymptomsDetail == false) {
                                HPITemplateDetail.selectedFindings[i].HPISymptoms_SeverityId = '';
                                HPITemplateDetail.selectedFindings[i].HPISymptoms_LocationIds = '';
                                HPITemplateDetail.selectedFindings[i].HPISymptoms_RadiationId = '';
                                HPITemplateDetail.selectedFindings[i].HPISymptoms_QualityId = '';
                                HPITemplateDetail.selectedFindings[i].Onset = '';
                                HPITemplateDetail.selectedFindings[i].AssociatedWith = '';
                                HPITemplateDetail.selectedFindings[i].HPISymptoms_AggravatedById = '';
                                HPITemplateDetail.selectedFindings[i].HPISymptoms_RelievedById = '';
                                HPITemplateDetail.selectedFindings[i].IsChecked = false;
                            }
                        }
                    }
                }
                else {
                    //var findings = $('#' + HPITemplateDetail.params.PanelID + ' #ulHPISymptomFindings li').length;
                    //if (findings == 0) {

                    //}
                }
            }

            $('#' + HPITemplateDetail.params.PanelID + ' #sectionFindingDetails').resetAllControls(null);
            $('#' + HPITemplateDetail.params.PanelID + " ddlLocation option").removeAttr("selected");
            $('#' + HPITemplateDetail.params.PanelID + ' #ddlLocation').multiselect('clearSelection');
            $('#' + HPITemplateDetail.params.PanelID + ' #ddlLocation').multiselect("refresh");
        }
    },

    loadFindingsDetails: function (HPISymptomId, findingId) {
        var isExist = false;

        if (HPITemplateDetail.selectedFindings) {
            for (var i = 0 ; i < HPITemplateDetail.selectedFindings.length; i++) {
                if (HPITemplateDetail.selectedFindings[i].HPISymptomId == HPISymptomId) {
                    isExist = true;
                    break;
                }
            }
        }
        if (HPITemplateDetail.selectedFindings) {
            for (var i = 0 ; i < HPITemplateDetail.selectedFindings.length; i++) {
                if (HPITemplateDetail.selectedFindings[i].HPISymptomId == HPISymptomId) {
                    if (HPITemplateDetail.selectedFindings[i].FindingId == findingId) {
                        $('#' + HPITemplateDetail.params.PanelID + ' #sectionFindingDetails').resetAllControls(null);
                        $('#' + HPITemplateDetail.params.PanelID + " ddlLocation option").removeAttr("selected");
                        $('#' + HPITemplateDetail.params.PanelID + ' #ddlLocation').multiselect('clearSelection');
                        $('#' + HPITemplateDetail.params.PanelID + ' #ddlLocation').multiselect("refresh");
                        var self = $('#' + HPITemplateDetail.params.PanelID + " #sectionFindingDetails");

                        utility.bindMyJSONByName(true, HPITemplateDetail.selectedFindings[i], false, self).done(function () {
                            if (HPITemplateDetail.selectedFindings[i].HPISymptoms_LocationIds) {
                                $('#' + HPITemplateDetail.params.PanelID + " #ddlLocation").val(HPITemplateDetail.selectedFindings[i].HPISymptoms_LocationIds.split(','));
                                $('#' + HPITemplateDetail.params.PanelID + ' #ddlLocation').multiselect("refresh");
                            }
                        });
                    }
                }
            }
        }
    },

    HPISymptomFindingsDetailsLoad: function (HPISymptomDetailId, HPITemplateSymptomId) {
        var objData = new Object();
        objData["HPISymptomDetailId"] = HPISymptomDetailId;
        objData["HPITemplateSymptomId"] = HPITemplateSymptomId;
        objData["commandType"] = "Load_HPI_Symptom_Findings_Detail";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPI");
    },

    AddSymptomHPI: function (obj, controlId, event) {
        event.stopPropagation();
        var objData = new Object();
        objData["IsGlobal"] = false;
        objData["Name"] = $("#txtName" + controlId).val();
        objData["IsActive"] = true;

        if (objData["Name"] == null || objData["Name"] == "" || objData["Name"] == undefined) {
            utility.DisplayMessages("Please enter some value", 3);
            return;
        }

        HPITemplateDetail.saveHPISDymptom_DBCall(objData).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                utility.DisplayMessages(response.Message, 1);
                var symptomOrder = 1;
                if ($("#ulHPISymptoms tr").length > 1) {
                    var lastOrder = $("#ulHPISymptoms tr").last().attr('symptomOrder');
                    if (lastOrder != null) {
                        symptomOrder = parseInt(lastOrder) + 1;                        
                    }
                    else {
                        symptomOrder = 1;
                    }                    
                }
                var li = HPITemplateDetail.addSymptom(response.HPISymptomsId, objData["Name"], "",symptomOrder);
                $("#ulHPISymptoms").append(li);
                $("#ulHPISymptoms tr").removeClass('active');
                $("#ulHPISymptoms tr[id=" + response.HPISymptomsId + "]").addClass('active');
                var symptomOrder = $("#ulHPISymptoms tr[id=" + response.HPISymptomsId + "]").attr('symptomorder');
                var objselectedFindings = {
                    HPISymptomId: response.HPISymptomsId, SymptomOrder:symptomOrder, IsChecked: false, FindingId: -1, FindingName: '', IsSymptomChecked: false,
                    SymptomDescription: "", IsSymptomDeleted: 0, IsFindingDeleted: 0, HPISymptomsAnswersId: null, IsSymptomsDetail: true, HPISymptoms_SeverityId: 0, HPISymptoms_LocationIds: '', HPISymptoms_RadiationId: 0,
                    HPISymptoms_QualityId: 0, Onset: '', AssociatedWith: '', HPISymptoms_AggravatedById: 0, HPISymptoms_RelievedById: 0, FindingOrder: -1
                };
                HPITemplateDetail.selectedFindings.push(objselectedFindings);               
                $("#" + controlId).remove();
                $("#ulHPISymptomFindings").empty();
                $("#ulHPISymptomFindings").addClass('disableAll');
            }
            else {
                utility.DisplayMessages(response.Message, 1);
            }
        });
    },
    saveHPISDymptom_DBCall: function (data) {

        var objData = new Object();
        if (data)
            objData = data;

        objData["commandType"] = "insert_hpi_symptoms";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPISymptoms");
    },

    AddFindingHPI: function (obj, controlId, event) {
        event.stopPropagation();

        var hpiSymptomsId = $("#ulHPISymptoms tr.active")[0].id;
        var symptomorder = $("#ulHPISymptoms tr.active").attr('symptomorder');
        var objData = new Object();
        objData["HPISymptomsId"] = hpiSymptomsId;
        objData["Name"] = $("#txtName" + controlId).val();
        objData["IsActive"] = true;

        if (objData["Name"] == null || objData["Name"] == "" || objData["Name"] == undefined) {
            utility.DisplayMessages("Please enter some value", 3);
            return;
        }
        var findingOrder = 1;
        if ($("#ulHPISymptomFindings li").length > 1) {
            var lastOrder = $("#ulHPISymptomFindings li").last().attr('findingOrder');
            if (lastOrder != null) {
                findingOrder = parseInt(lastOrder) + 1;
            }
            else {
                findingOrder = 1;
            }
        }

        HPITemplateDetail.saveHPIFindings_DBCall(objData).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    var res = response;
                    utility.DisplayMessages(response.Message, 1);
                    HPITemplateDetail.params.HPIFindingId = res.HPIFindingsId;

                    HPITemplateDetail.updateHPISymptoms_DBCall(res.HPIFindingsId, hpiSymptomsId, findingOrder).done(function (response) {
                        if (response != "") {
                            response = JSON.parse(response);
                            if (response.status != false) {

                                var li = HPITemplateDetail.addFindings(res.HPIFindingsId, objData["Name"], hpiSymptomsId, findingOrder);
                                $("#ulHPISymptomFindings").append(li);
                                var sysChecked = $("#ulHPISymptoms tr.active .MK").is(':checked');

                                var objselectedFindings = {
                                    HPISymptomId: hpiSymptomsId, SymptomOrder:symptomorder, IsChecked: false, FindingId: res.HPIFindingsId, FindingName: objData["Name"], IsSymptomChecked: sysChecked,
                                    SymptomDescription: "", IsSymptomDeleted: 0, IsFindingDeleted: 0, HPISymptomsAnswersId: "1", IsSymptomsDetail: false, HPISymptoms_SeverityId: 0, HPISymptoms_LocationIds: '', HPISymptoms_RadiationId: 0,
                                    HPISymptoms_QualityId: 0, Onset: '', AssociatedWith: '', HPISymptoms_AggravatedById: 0, HPISymptoms_RelievedById: 0, FindingOrder: findingOrder
                                };

                                HPITemplateDetail.selectedFindings.push(objselectedFindings);
                                $("#" + controlId).remove();
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        }
                    });
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
        });
    },

    saveHPIFindings_DBCall: function (data) {

        var objData = new Object();
        if (data)
            objData = data;

        objData["commandType"] = "insert_hpi_findings";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPIFindings");
    },

    updateHPISymptoms_DBCall: function (HPIFindingsId, HPISymptomsId, findingOrder) {
        var objData = new Object();

        objData["HPIFindingsIds"] = HPIFindingsId;
        objData["HPISymptomsId"] = HPISymptomsId;
        objData["IsActive"] = true;
        objData["IsGlobal"] = true;
        objData["Name"] = $("#ulHPISymptoms tr.active").text();
        objData["FindingOrder"] = findingOrder;
        objData["commandType"] = "update_hpi_symptoms";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HPI", "HPISymptoms");

    },

    bindDetailsChangeEvents: function () {

        $('#' + HPITemplateDetail.params.PanelID + ' #sectionFindingDetails').on('input', function (e) {
            var controlId = $(e.target).attr('id');

            var HPISymptomId = $("#ulHPISymptoms tr.active").attr('id');
            var HPIFindingId = 0;
            if ($("#ulHPISymptomFindings li.active").length > 0) {
                HPIFindingId = $("#ulHPISymptomFindings li.active").attr('id');
            }

            var delimator = $("#delimator option:selected").text() + " ";

            if (HPIFindingId) {
                if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).length > 0) {
                    var txtToAppend = '';

                    if (controlId == 'ddlSeverity') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlSeverity').length > 0) {
                            txtToAppend = 'Severity: ' + $('#ddlSeverity option:selected').text() + delimator;
                            if ($("#ddlSeverity").val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlSeverity').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlSeverity').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlSeverity');
                            $newDiv.attr("style", "display: inline;");

                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                            txtToAppend = 'Severity: ' + $('#ddlSeverity option:selected').text() + delimator;
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlSeverity').append(txtToAppend);

                        }
                    }
                    else if (controlId == 'ddlRadiation') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRadiation').length > 0) {
                            txtToAppend = 'Radiation: ' + $('#ddlRadiation option:selected').text() + delimator;
                            if ($('#ddlRadiation').val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRadiation').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRadiation').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRadiation');
                            $newDiv.attr("style", "display: inline;");

                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                            txtToAppend = 'Radiation: ' + $('#ddlRadiation option:selected').text() + delimator;
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRadiation').append(txtToAppend);
                        }
                    }
                    else if (controlId == 'ddlQuality') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlQuality').length > 0) {
                            txtToAppend = 'Quality: ' + $('#ddlQuality option:selected').text() + delimator;
                            if ($('#ddlQuality').val() == 0) {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlQuality').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlQuality').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlQuality');
                            $newDiv.attr("style", "display: inline;");

                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                            txtToAppend = 'Quality: ' + $('#ddlQuality option:selected').text() + delimator;
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlQuality').append(txtToAppend);
                        }
                    }
                    else if (controlId == 'txtOnset') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtOnset').length > 0) {
                            txtToAppend = 'Onset: ' + $('#txtOnset').val() + delimator;
                            if ($('#txtOnset').val() == '') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtOnset').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtOnset').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtOnset');
                            $newDiv.attr("style", "display: inline;");

                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                            txtToAppend = 'Onset: ' + $('#txtOnset').val() + delimator;
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtOnset').append(txtToAppend);
                        }
                    }
                    else if (controlId == 'txtAssociatedWith') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtAssociatedWith').length > 0) {
                            txtToAppend = 'Associated With: ' + $('#txtAssociatedWith').val() + delimator;
                            if ($('#txtAssociatedWith').val() == '') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtAssociatedWith').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtAssociatedWith').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtAssociatedWith');
                            $newDiv.attr("style", "display: inline;");

                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                            txtToAppend = 'Associated With: ' + $('#txtAssociatedWith').val() + delimator;
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'txtAssociatedWith').append(txtToAppend);
                        }
                    }
                    else if (controlId == 'ddlAggravatedBy') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlAggravatedBy').length > 0) {
                            txtToAppend = 'Aggravated By: ' + $('#ddlAggravatedBy option:selected').text() + delimator;
                            if ($('#ddlAggravatedBy').val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlAggravatedBy').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlAggravatedBy').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlAggravatedBy');
                            $newDiv.attr("style", "display: inline;");

                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                            txtToAppend = 'Aggravated By: ' + $('#ddlAggravatedBy option:selected').text() + delimator;
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlAggravatedBy').append(txtToAppend);
                        }
                    }
                    else if (controlId == 'ddlRevieledBy') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRevieledBy').length > 0) {
                            txtToAppend = 'Revieled By: ' + $('#ddlRevieledBy option:selected').text() + delimator;
                            if ($('#ddlRevieledBy').val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRevieledBy').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRevieledBy').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRevieledBy');
                            $newDiv.attr("style", "display: inline;");

                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                            $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                            txtToAppend = 'Revieled By: ' + $('#ddlRevieledBy option:selected').text() + delimator;
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlRevieledBy').append(txtToAppend);
                        }
                    }

                }
            }
            else {
                if ($("#findingContent #divSymptom" + HPISymptomId).length > 0) {
                    var txtToAppend = '';
                    HPITemplateDetail.resetFindings(HPISymptomId);
                    if (controlId == 'ddlSeverity') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'ddlSeverity').length > 0) {
                            txtToAppend = ' Severity: ' + $('#ddlSeverity option:selected').text() + delimator;
                            if ($("#ddlSeverity").val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlSeverity').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlSeverity').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'ddlSeverity');
                            $newDiv.attr("style", "display: inline;");

                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = ' Severity: ' + $('#ddlSeverity option:selected').text() + delimator;;
                            }
                            else {
                                txtToAppend = 'Severity: ' + $('#ddlSeverity option:selected').text() + delimator;
                            }

                            $("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'ddlSeverity').append(txtToAppend);

                        }
                    }
                    else if (controlId == 'ddlRadiation') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'ddlRadiation').length > 0) {
                            txtToAppend = ' Radiation: ' + $('#ddlRadiation option:selected').text() + delimator;
                            if ($('#ddlRadiation').val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlRadiation').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlRadiation').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'ddlRadiation');
                            $newDiv.attr("style", "display: inline;");

                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = ' Radiation: ' + $('#ddlRadiation option:selected').text() + delimator;

                            }
                            else {
                                txtToAppend = 'Radiation: ' + $('#ddlRadiation option:selected').text() + delimator;
                            }

                            $("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'ddlRadiation').append(txtToAppend);
                        }
                    }

                    else if (controlId == 'ddlQuality') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'ddlQuality').length > 0) {
                            txtToAppend = ' Quality: ' + $('#ddlQuality option:selected').text() + delimator;
                            if ($('#ddlQuality').val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlQuality').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlQuality').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'ddlQuality');
                            $newDiv.attr("style", "display: inline;");

                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = ' Quality: ' + $('#ddlQuality option:selected').text() + delimator;
                            }
                            else {
                                txtToAppend = 'Quality: ' + $('#ddlQuality option:selected').text() + delimator;
                            }

                            $("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'ddlQuality').append(txtToAppend);
                        }
                    }

                    else if (controlId == 'txtOnset') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'txtOnset').length > 0) {
                            txtToAppend = ' Onset: ' + $('#txtOnset').val() + delimator;
                            if ($('#txtOnset').val() == '') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'txtOnset').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'txtOnset').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'txtOnset');
                            $newDiv.attr("style", "display: inline;");

                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = ' Onset: ' + $('#txtOnset').val() + delimator;
                            }
                            else {
                                txtToAppend = 'Onset: ' + $('#txtOnset').val() + delimator;
                            }
                            $("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'txtOnset').append(txtToAppend);
                        }
                    }
                    else if (controlId == 'txtAssociatedWith') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'txtAssociatedWith').length > 0) {
                            txtToAppend = ' Associated With: ' + $('#txtAssociatedWith').val() + delimator;
                            if ($('#txtAssociatedWith').val() == '') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'txtAssociatedWith').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'txtAssociatedWith').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'txtAssociatedWith');
                            $newDiv.attr("style", "display: inline;");
                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = ' Associated With: ' + $('#txtAssociatedWith').val() + delimator;
                            }
                            else {
                                txtToAppend = 'Associated With: ' + $('#txtAssociatedWith').val() + delimator;
                            }

                            $("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'txtAssociatedWith').append(txtToAppend);
                        }
                    }

                    else if (controlId == 'ddlAggravatedBy') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'ddlAggravatedBy').length > 0) {
                            txtToAppend = ' Aggravated By: ' + $('#ddlAggravatedBy option:selected').text() + delimator;
                            if ($('#ddlAggravatedBy').val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlAggravatedBy').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlAggravatedBy').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'ddlAggravatedBy');
                            $newDiv.attr("style", "display: inline;");
                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = ' Aggravated By: ' + $('#ddlAggravatedBy option:selected').text() + delimator;
                            }
                            else {
                                txtToAppend = 'Aggravated By: ' + $('#ddlAggravatedBy option:selected').text() + delimator;
                            }
                            $("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'ddlAggravatedBy').append(txtToAppend);
                        }
                    }

                    else if (controlId == 'ddlRevieledBy') {
                        if ($("#findingContent #divSymptom" + HPISymptomId + 'ddlRevieledBy').length > 0) {
                            txtToAppend = ' Revieled By: ' + $('#ddlRevieledBy option:selected').text() + delimator;
                            if ($('#ddlRevieledBy').val() == '0') {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlRevieledBy').remove();
                            }
                            else {
                                $("#findingContent #divSymptom" + HPISymptomId + 'ddlRevieledBy').text(txtToAppend);
                            }
                        }
                        else {
                            var $newDiv = $("<div></div>");
                            $newDiv.attr("id", "divSymptom" + HPISymptomId + 'ddlRevieledBy');
                            $newDiv.attr("style", "display: inline;");

                            if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                                txtToAppend = ' Revieled By: ' + $('#ddlRevieledBy option:selected').text() + delimator;
                            }
                            else {
                                txtToAppend = 'Revieled By: ' + $('#ddlRevieledBy option:selected').text() + delimator;
                            }
                            $("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                            $('#findingContent #divSymptom' + HPISymptomId).show();

                            $("#findingContent #divSymptom" + HPISymptomId + 'ddlRevieledBy').append(txtToAppend);
                        }
                    }
                }
            }
        });


        $('#' + HPITemplateDetail.params.PanelID + ' #ddlLocation').on('change', function () {
            var HPISymptomId = $("#ulHPISymptoms tr.active").attr('id');
            var HPIFindingId = 0;
            if ($("#ulHPISymptomFindings li.active").length > 0) {
                HPIFindingId = $("#ulHPISymptomFindings li.active").attr('id');
            }

            var delimator = $("#delimator option:selected").text() + " ";

            if (HPIFindingId) {
                if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).length > 0) {
                    var txtToAppend = '';
                    if ($("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlLocation').length > 0) {
                        txtToAppend = 'Location: ' + HPITemplateDetail.getLocationText($('#' + HPITemplateDetail.params.PanelID + ' #ddlLocation')) + delimator; // $('#ddlLocation option:selected').text();
                        if (!$("#ddlLocation").val()) {
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlLocation').remove();
                        }
                        else {
                            $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlLocation').text(txtToAppend);
                        }
                    }
                    else {
                        var $newDiv = $("<div></div>");
                        $newDiv.attr("id", "divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlLocation');
                        $newDiv.attr("style", "display: inline;");
                        if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                            txtToAppend = ' Location: ' + HPITemplateDetail.getLocationText($('#' + HPITemplateDetail.params.PanelID + ' #ddlLocation')) + delimator; //$('#ddlLocation option:selected').text();
                        }
                        else {
                            txtToAppend = 'Location: ' + HPITemplateDetail.getLocationText($('#' + HPITemplateDetail.params.PanelID + ' #ddlLocation')) + delimator; //$('#ddlLocation option:selected').text();
                        }
                        $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId).append($newDiv);
                        $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + HPIFindingId).show();

                        $("#findingContent #divSymptom" + HPISymptomId + 'Finding' + HPIFindingId + 'ddlLocation').append(txtToAppend);

                    }
                }
            }
            else {
                if ($("#findingContent #divSymptom" + HPISymptomId).length > 0) {
                    var txtToAppend = '';
                    HPITemplateDetail.resetFindings(HPISymptomId);

                    if ($("#findingContent #divSymptom" + HPISymptomId + 'ddlLocation').length > 0) {
                        txtToAppend = ' Location: ' + HPITemplateDetail.getLocationText($('#' + HPITemplateDetail.params.PanelID + ' #ddlLocation')) + delimator;
                        if (!$("#ddlLocation").val()) {
                            $("#findingContent #divSymptom" + HPISymptomId + 'ddlLocation').remove();
                        }
                        else {
                            $("#findingContent #divSymptom" + HPISymptomId + 'ddlLocation').text(txtToAppend);
                        }
                    }
                    else {
                        var $newDiv = $("<div></div>");
                        $newDiv.attr("id", "divSymptom" + HPISymptomId + 'ddlLocation');
                        $newDiv.attr("style", "display: inline;");
                        if ($("#findingContent").find("div[id^='divSymptom" + HPISymptomId + "']").length == 1) {
                            txtToAppend = ' Location: ' + HPITemplateDetail.getLocationText($('#' + HPITemplateDetail.params.PanelID + ' #ddlLocation')) + delimator;
                        }
                        else {
                            txtToAppend = 'Location: ' + HPITemplateDetail.getLocationText($('#' + HPITemplateDetail.params.PanelID + ' #ddlLocation')) + delimator;
                        }
                        $("#findingContent #divSymptom" + HPISymptomId).append($newDiv);
                        $('#findingContent #divSymptom' + HPISymptomId).show();

                        $("#findingContent #divSymptom" + HPISymptomId + 'ddlLocation').append(txtToAppend);

                    }
                }
            }
        });
    },

    getLocationText: function (ddlLocation) {
        var str = '';
        var selectedText = '';
        var selectedValues = ddlLocation.find('option:selected').map(function () {
            return this.text
        });
        if (selectedValues.length > 0) {
            var selectedText = $.makeArray(selectedValues).join();
            selectedText = selectedText.replace(/,/g, ", ");
            str += selectedText.replace(/,([^,]*)$/, ' and $1');
        }
        return str;
    },

    resetFindings: function (HPISymptomId) {
        if (HPITemplateDetail.selectedFindings) {
            for (var i = 0 ; i < HPITemplateDetail.selectedFindings.length; i++) {
                if (HPITemplateDetail.selectedFindings[i].HPISymptomId == HPISymptomId) {
                    var findingid = HPITemplateDetail.selectedFindings[i].FindingId;
                    $('#findingContent #divSymptom' + HPISymptomId + 'Finding' + findingid).remove();

                    HPITemplateDetail.selectedFindings[i].HPISymptoms_SeverityId = '';
                    HPITemplateDetail.selectedFindings[i].HPISymptoms_LocationIds = '';
                    HPITemplateDetail.selectedFindings[i].HPISymptoms_RadiationId = '';
                    HPITemplateDetail.selectedFindings[i].HPISymptoms_QualityId = '';
                    HPITemplateDetail.selectedFindings[i].Onset = '';
                    HPITemplateDetail.selectedFindings[i].AssociatedWith = '';
                    HPITemplateDetail.selectedFindings[i].HPISymptoms_AggravatedById = '';
                    HPITemplateDetail.selectedFindings[i].HPISymptoms_RelievedById = '';
                    HPITemplateDetail.selectedFindings[i].IsChecked = false;
                }
            }
        }

        for (var i = 0; i < $("#ulHPISymptomFindings li").length; i++) {
            $("#ulHPISymptomFindings #chk" + $("#ulHPISymptomFindings li")[i].id).prop('checked', false);
        }

    },
}
