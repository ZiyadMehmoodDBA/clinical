PhysicalExamTemplateDetail = {
    //Author: Muhammad Arshad
    //Date: 26-02-2016
    //This file will handle all actions performed for Physical Exam Template
    bIsFirstLoad: true,
    EditableGrid: null,
    params: [],
    FamilyMembers: [],
    ExamDetails: {},
    SelectedSystem: '',
    array: [],
    myArr: [],
    parentCtrlGlobel: null,
    mainSelected: null,
    SectionNormalInfo: [],
    //Start 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
    selectedcharacteristicsIds: [],
    characteristicsWithData: [],
    selectedsubcharacteristicsIds: [],
    subcharacteristicsWithData: [],
    selectedData: null,
    isNormalTriggred: false,
    isBothUnCheck: false,
    specialityCheckedIds: [],
    providerSelectedIds: [],
    providerCheckedIds: [],
    normalSystemIdsGlobel: [],
    selectedPhyExamTempData: [],
    SpecialtyIds: '',
    ProviderIds: '',
    NewInsertedId: -1,

    Load: function (params) {
        BackgroundLoaderShow(true);
        PhysicalExamTemplateDetail.params = params;
        var isSelectedEntity = false
        PhysicalExamTemplateDetail.selectedPhyExamTempData = [];
        var self = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #tblPhysicalExamTemplateDetail');
        self.loadDropDowns(true).done(function () {
            if (PhysicalExamTemplateDetail.params["mode"] == "Edit") {
                PhysicalExamTemplateDetail.LoadPhysicalExamTemplateDetail(PhysicalExamTemplateDetail.params["PhysicalExamTemplateId"]);

                self.find("#btnPhysicalExamSaveAS").removeClass("hidden");
            }
            else {
                self.find("#btnPhysicalExamSaveAS").addClass("hidden");
            }
            if (globalAppdata.AppUserName.toLowerCase() != DefaultUser.toLowerCase()) {

                self.find("#ddlPhysicalExamTemplateEntity").val(globalAppdata["SeletedEntityId"]);

                if (self.find("div#divEntity").hasClass("hidden") == false) {
                    self.find("div#divEntity").addClass("hidden");
                }
                PhysicalExamTemplateDetail.loadEntitySpecialty(globalAppdata["SeletedEntityId"]);
                PhysicalExamTemplateDetail.loadEntityProvider(globalAppdata["SeletedEntityId"]);
                isSelectedEntity = true;
            }
            else {
                self.find("div#divEntity").removeClass("hidden");
            }
        });
        PhysicalExamTemplateDetail.domReady();

        $.when(PhysicalExamTemplateDetail.loadChildData(null, "mainpesystem")).then(function () {


            //Start Farooq Ahmad 03-03-2016 mark green and checked the loaded System
            for (var index in PhysicalExamTemplateDetail.selectedPhyExamTempData) {

                //if ($.parseJSON(PhysicalExamTemplateDetail.selectedPhyExamTempData[index].IsChecked.toString().toLowerCase()))
                //   $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystems").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].SystemId).addClass("green");
                //  else
                //$('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystems").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].SystemId).addClass("green");

                $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystems").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].SystemId + " input:checkbox").prop("checked", $.parseJSON(PhysicalExamTemplateDetail.selectedPhyExamTempData[index].IsChecked.toString().toLowerCase()));
                $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystems").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].SystemId + " label").attr("data-original-title", PhysicalExamTemplateDetail.selectedPhyExamTempData[index].SystemName);
                $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystems").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].SystemId + " label").text(PhysicalExamTemplateDetail.selectedPhyExamTempData[index].SystemName);
            }
            //End Farooq Ahmad 03-03-2016 mark green and checked the loaded System
        });
        PhysicalExamTemplateDetail.validatePhysicalExamTemplate();
        // Set Title Explicitly if it's passed as Parameter
        if (PhysicalExamTemplateDetail.params.Title != null)
            $("#" + PhysicalExamTemplateDetail.params["PanelID"] + " #headingTitle").text(PhysicalExamTemplateDetail.params.Title);
    },

    //Author: Farooq Ahmad
    //Date: 08/03/2016
    //This function will load the detail of the physical template detail by id
    LoadPhysicalExamTemplateDetail: function (templateId) {
        PhysicalExamTemplateDetail.PhysicalExamTemplateDetailLoad(templateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                response.PhysicalExamTemplate = JSON.parse(response.PhysicalExamTemplate);
                if (response.PhysicalExamTemplate.length > 0) {
                    response.PhysicalExamTemplate = response.PhysicalExamTemplate[0];
                    PhysicalExamTemplateDetail.selectedPhyExamTempData = response.PhysicalExamTemplate.SysSecCharSubcharData;
                    //Start Farooq Ahmad 03-03-2016 mark green and checked the loaded System
                    for (var index in PhysicalExamTemplateDetail.selectedPhyExamTempData) {
                        //if ($.parseJSON(PhysicalExamTemplateDetail.selectedPhyExamTempData[index].IsChecked.toString().toLowerCase()))
                        //    $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystems").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].SystemId).addClass("green");
                        //else
                        //    $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystems").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].SystemId).removeClass("green");

                        $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystems").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].SystemId + " input:checkbox").prop("checked", $.parseJSON(PhysicalExamTemplateDetail.selectedPhyExamTempData[index].IsChecked.toString().toLowerCase()));
                        $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystems").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].SystemId + " label").attr("data-original-title", PhysicalExamTemplateDetail.selectedPhyExamTempData[index].SystemName);
                        $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystems").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].SystemId + " label").text(PhysicalExamTemplateDetail.selectedPhyExamTempData[index].SystemName);
                    }
                    //End Farooq Ahmad 03-03-2016 mark green and checked the loaded System
                    var self = $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail");

                    utility.bindMyJSONByName(true, response.PhysicalExamTemplate, false, self).done(function () {
                        $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ddlPhysicalExamTemplateEntity').change();
                        PhysicalExamTemplateDetail.SpecialtyIds = response.PhysicalExamTemplate.SpecialtyIds;
                        PhysicalExamTemplateDetail.ProviderIds = response.PhysicalExamTemplate.ProviderIds;
                    });
                }

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    PhysicalExamTemplateDetailLoad: function (templateId) {
        var objData = new Object();
        objData["TemplateId"] = templateId;
        objData["commandType"] = "Load_PhyscialExam_Template_Detail";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },

    PhysicalExamTemplateDetailLoadOnDemand: function (templateId, systemId, sectionId, charId, commandType) {
        var objData = new Object();
        objData["TemplateId"] = templateId;
        if (systemId != null)
            objData["SystemId"] = systemId;
        if (sectionId != null)
            objData["SectionId"] = sectionId;
        if (charId != null)
            objData["CharacteristicId"] = charId;

        objData["commandType"] = commandType;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },


    //Author: Abid Ali
    //Date: 03-04-2016
    //This function will initialize Specialty multiselect ddl
    IntializeMultiSelectDropDownSpecialties: function () {
        $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect('destroy');
        $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'All',
            selectAll: false,
            onChange: function (option, checked, select) {

                PhysicalExamTemplateDetail.checkProvidersBySpecialityIds(option, checked, select);

            },
            onDropdownHide: function (event) {
                $.when(
                    PhysicalExamTemplateDetail.filterProvidersBySpecialtyIds()
                ).then(function () {
                    if (PhysicalExamTemplateDetail.ProviderIds != '') {
                        var Providers = PhysicalExamTemplateDetail.ProviderIds.split(",");

                        if (Providers != '' && typeof Providers != 'undefined') {

                            $.each(Providers, function (index, item) {
                                PhysicalExamTemplateDetail.providerCheckedIds = PhysicalExamTemplateDetail.removeFromArray(PhysicalExamTemplateDetail.providerCheckedIds, item);
                                PhysicalExamTemplateDetail.providerCheckedIds.push(item);
                            });
                        }
                    }
                    $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ddlPhysicalExamTemplateProvider').val(PhysicalExamTemplateDetail.providerCheckedIds);
                    PhysicalExamTemplateDetail.IntializeMultiSelectDropDownProviders();
                });
            },

            onDropdownShow: function (event) {
                //make items selected and initialize dropdownlist
                if (PhysicalExamTemplateDetail.SpecialtyIds != '') {
                    var spacialties = PhysicalExamTemplateDetail.SpecialtyIds.split(",");

                    if (spacialties != '' && typeof spacialties != 'undefined') {

                        $.each(spacialties, function (index, item) {
                            PhysicalExamTemplateDetail.specialityCheckedIds = PhysicalExamTemplateDetail.removeFromArray(PhysicalExamTemplateDetail.specialityCheckedIds, item);
                            PhysicalExamTemplateDetail.specialityCheckedIds.push(item);
                        });
                    }
                }
                PhysicalExamTemplateDetail.setSpacialtiesByselectedProviderIds();
                $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect('select', PhysicalExamTemplateDetail.specialityCheckedIds);
            },
        });
    },

    //Author: Abid Ali
    //Date: 03-04-2016
    //This function will set specialty Ids in specailChekedIds array
    setSpacialtiesByselectedProviderIds: function () {

        $.each(PhysicalExamTemplateDetail.providerCheckedIds, function (index, item) {

            $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ddlPhysicalExamTemplateProvider option').each(function () {
                if ($(this).val() != '') {
                    if ($(this).val() == item) {

                        PhysicalExamTemplateDetail.specialityCheckedIds = PhysicalExamTemplateDetail.removeFromArray(PhysicalExamTemplateDetail.specialityCheckedIds, $(this).attr('refname'));
                        PhysicalExamTemplateDetail.specialityCheckedIds.push($(this).attr('refname'));
                    }
                }
            });
        });
    },

    //Author: Abid Ali
    //Date: 03-04-2016
    //This function will split Ids on '-' from array
    stripIdsFromArray: function (array) {

        var resultantArray = jQuery.grep(array, function (item) {
            return item.split('-')[0];
        });
        return resultantArray;
    },

    //Author: Abid Ali
    //Date: 03-04-2016
    //This function will initialize provider multiselect ddl
    IntializeMultiSelectDropDownProviders: function () {
        $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ddlPhysicalExamTemplateProvider').multiselect('destroy');
        $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ddlPhysicalExamTemplateProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'All',
            selectAll: false,
            onChange: function (option, checked, select) {
                PhysicalExamTemplateDetail.checkSpecialtiesByProviderId(option, checked, select);
            },
            onDropdownHide: function (event) {
                // PhysicalExamTemplateDetail.specialitiesByProviderIds();
                //Refresh multiselect
                //  $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect('refresh');
            },


        });
    },


    //Author: Abid Ali
    //Date: 03-04-2016
    //This function will enable disable multiselect ddls provided
    enableDisableDropDownLists: function (ddlIds, isHide) {
        ddlCommaSeparatedIds = ddlIds.split(',');
        var parrentPanelId = "#" + PhysicalExamTemplateDetail.params["PanelID"];
        $.each(ddlCommaSeparatedIds, function (index, Item) {
            if (isHide) {
                $(parrentPanelId + " #" + Item).multiselect('disable');
            }
            else {
                $(parrentPanelId + " #" + Item).multiselect('enable');
            }
        });
    },

    //Author: Abid Ali
    //Date: 03-04-2016
    //This function will remove item from the "array and item" provided as input args
    removeFromArray: function (array, removeItem) {

        var resultantArray = jQuery.grep(array, function (item) {
            return item != removeItem;
        });
        return resultantArray;
    },

    //Author: Abid Ali
    //Date: 03-04-2016
    //This function will load entity based provider
    loadEntityProvider: function (entityId) {

        var data = "entityID=" + entityId;
        if (entityId != null && entityId > 0) {

            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ddlPhysicalExamTemplateProvider');
                var $providerHiddenDdl = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ddlHiddenPhysicalExamTemplateProvider');

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
                if (PhysicalExamTemplateDetail.ProviderIds != '') {
                    var Providers = PhysicalExamTemplateDetail.ProviderIds.split(",");
                    PhysicalExamTemplateDetail.providerCheckedIds = Providers;
                    $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ddlPhysicalExamTemplateProvider').val(Providers);
                }

            }).then(function () {
                // A hack to trigger the onDropDownHide event of Spacialty multiselect      
                $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #divPhysicalExamTemplateSpecialty').find('.dropdown-toggle').dropdown('toggle').dropdown('toggle');
                //Intialized in onhidden spacialty ddl.  
                PhysicalExamTemplateDetail.IntializeMultiSelectDropDownProviders();

            });
            //enable multiselect
            PhysicalExamTemplateDetail.enableDisableDropDownLists('ddlPhysicalExamTemplateProvider', false);
        }
        else {
            //disable multiselect
            PhysicalExamTemplateDetail.enableDisableDropDownLists('ddlPhysicalExamTemplateProvider', true);
        }
    },


    //Author: Abid Ali
    //Date: 03-04-2016
    //This function will perform tasks on document ready event
    domReady: function () {

        $(document).ready(function () {

            PhysicalExamTemplateDetail.enableDisableDropDownLists('ddlPhysicalExamTemplateSpecialty,ddlPhysicalExamTemplateProvider', true);

            //callback function on change event of entity ddl
            PhysicalExamTemplateDetail.entityChanged();

            //Initialize when the document is ready for the first time (just for the good look).
            PhysicalExamTemplateDetail.IntializeMultiSelectDropDownSpecialties();
            PhysicalExamTemplateDetail.IntializeMultiSelectDropDownProviders();


            $(document).click(function (event) {



                var $Item = $(PhysicalExamTemplateDetail.selectedListItem);
                var id = $Item.attr('id');

                // var allItems = 

                var SystemDetailDiv = $Item.find("div#divNameDetail" + id);
                var SystemNameLabel = $Item.find("#lblName" + id);
                var txtSystemName = SystemDetailDiv.find("#txtName" + id);

                var isEqual = true;

                //if not matched
                isEqual = $(event.target).closest('li#' + id).length == 0 ? false : true;

                if (!isEqual) {

                    //if not matched with self
                    if ($(event.target).attr('id') != id) {
                        isEqual = false;
                    }
                }
                if (!isEqual) {

                    SystemDetailDiv.addClass("hidden");
                    SystemNameLabel.removeClass("hidden");
                }
            });
        });
    },

    //Author: Abid Ali
    //Date: 03-04-2016
    //This function will  handle change event of entity ddl
    entityChanged: function () {

        $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ddlPhysicalExamTemplateEntity').change(function () {
            //Get the selected entity
            selectedEntity = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ddlPhysicalExamTemplateEntity :selected').val();

            $.when(PhysicalExamTemplateDetail.loadEntitySpecialty(selectedEntity)).then(function () {

                $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').multiselect('destroy').multiselect();
            });
            $.when(PhysicalExamTemplateDetail.loadEntityProvider(selectedEntity)).then(function () {

                $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ddlPhysicalExamTemplateProvider').multiselect('destroy').multiselect();

            });
        });
    },

    //Author: Abid Ali
    //Date: 03-04-2016
    //This function will load entity based specialty
    loadEntitySpecialty: function (entityID) {

        // Loads Spacialties Based on entityId
        if (entityID != null && entityID > 0) {

            providerDetail.FillSpecialty(entityID).done(function (response) {
                if (response.status != false) {

                    var spacialties = JSON.parse(response.SpecialtyLoad_JSON);
                    $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').empty();

                    $.each(spacialties, function (i, item) {
                        $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').append(
                            $('<option/>', {
                                value: item.SpecialtyId,
                                html: item.ShortName
                            })
                        );
                    });

                    //Assign server side spacialties to the specialityCheckedIds array
                    if (PhysicalExamTemplateDetail.SpecialtyIds != '') {

                        var Specialties = PhysicalExamTemplateDetail.SpecialtyIds.split(",");
                        PhysicalExamTemplateDetail.specialityCheckedIds = Specialties;
                        $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty').val(Specialties);
                    }
                }

            }).then(function () {
                PhysicalExamTemplateDetail.IntializeMultiSelectDropDownSpecialties();
                //enable dropdownlist
                PhysicalExamTemplateDetail.enableDisableDropDownLists('ddlPhysicalExamTemplateSpecialty', false);
            });
        }
        else {
            //Disable dropdownlist
            PhysicalExamTemplateDetail.enableDisableDropDownLists('ddlPhysicalExamTemplateSpecialty', true);
        }
    },

    //Author: Abid Ali
    //Date: 03-04-2016
    //This function will save spacialty ids and will show privders on spacialty selection
    filterProvidersBySpecialtyIds: function () {

        var providerHiddenContext = '#' + PhysicalExamTemplateDetail.params.PanelID + ' #ddlHiddenPhysicalExamTemplateProvider';

        var providerContext = '#' + PhysicalExamTemplateDetail.params.PanelID + ' #ddlPhysicalExamTemplateProvider';
        $(providerContext).empty();

        if (PhysicalExamTemplateDetail.specialityCheckedIds.length > 0) {

            $.each(PhysicalExamTemplateDetail.specialityCheckedIds, function (index, specialtyId) {

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

    //Author: Abid Ali
    //Date: 03-04-2016
    //This function will save spacialty ids and will show privders on spacialty selection
    checkProvidersBySpecialityIds: function (option, checked, select) {
        //specialty context
        var specialtyContext = '#' + PhysicalExamTemplateDetail.params.PanelID + ' #divPhysicalExamTemplateSpecialty';
        var isAllSpecialtySelected = $(specialtyContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var specialtyItems = $(specialtyContext).find('.dropdown-menu').find('li').length;
        var checkedSpecialtyItems = $(specialtyContext).find('.dropdown-menu').find('li.active').length;

        if (checkedSpecialtyItems <= 0) {
            PhysicalExamTemplateDetail.specialityCheckedIds = [];
            PhysicalExamTemplateDetail.providerCheckedIds = [];
            PhysicalExamTemplateDetail.ProviderIds = '';
            PhysicalExamTemplateDetail.SpecialtyIds = '';
        }
        else {
            if (!isAllSpecialtySelected && !(specialtyItems == checkedSpecialtyItems)) {


                var spacialityId = $(option).attr("value");
                if (checked && spacialityId != "") {
                    PhysicalExamTemplateDetail.specialityCheckedIds = PhysicalExamTemplateDetail.removeFromArray(PhysicalExamTemplateDetail.specialityCheckedIds, spacialityId);
                    PhysicalExamTemplateDetail.specialityCheckedIds.push(spacialityId);
                }
                else {

                    PhysicalExamTemplateDetail.specialityCheckedIds = PhysicalExamTemplateDetail.removeFromArray(PhysicalExamTemplateDetail.specialityCheckedIds, spacialityId);

                }


            }
            else {

                PhysicalExamTemplateDetail.specialityCheckedIds = [];
                $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ddlPhysicalExamTemplateSpecialty option').each(function () {
                    var spacialityId = $(this).attr("value");
                    PhysicalExamTemplateDetail.specialityCheckedIds.push(spacialityId);
                });

            }
        }
    },

    //Author: Abid Ali
    //Date: 03-04-2016
    //This function will save provider ids and will check speciality on provider selection
    checkSpecialtiesByProviderId: function (option, checked, select) {

        //provider context
        var providerContext = '#' + PhysicalExamTemplateDetail.params.PanelID + ' #divPhysicalExamTemplateProvider';
        var isAllProviderSelected = $(providerContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var providerItems = $(providerContext).find('.dropdown-menu').find('li').length;
        var checkedProviderItems = $(providerContext).find('.dropdown-menu').find('li.active').length;

        if (checkedProviderItems <= 0) {
            PhysicalExamTemplateDetail.providerCheckedIds = [];
            PhysicalExamTemplateDetail.ProviderIds = '';
        }
            //push all provider checked items
        else if (isAllProviderSelected) {
            PhysicalExamTemplateDetail.providerCheckedIds = [];
            $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ddlPhysicalExamTemplateProvider option').each(function () {
                var providerValue = $(this).val();
                PhysicalExamTemplateDetail.providerCheckedIds.push(providerValue);
            });
        }
        else {
            // provider value
            var providerValue = $(option).val();

            // add to provider array if checked
            if (checked) {
                PhysicalExamTemplateDetail.providerCheckedIds = PhysicalExamTemplateDetail.removeFromArray(PhysicalExamTemplateDetail.providerCheckedIds, providerValue);
                PhysicalExamTemplateDetail.providerCheckedIds.push(providerValue);
            }
                //delete from provider array if not checked
            else {
                PhysicalExamTemplateDetail.providerCheckedIds = PhysicalExamTemplateDetail.removeFromArray(PhysicalExamTemplateDetail.providerCheckedIds, $(option).val());
            }

        }
    },

    //Author: Abid Ali
    //Date: 03-04-2016
    //This function will return refname using (li's input value equals ddl option value)
    getRefValuefromDdl: function (ddlId, liId) {
        var $ddlOptions = $('#' + PhysicalExamTemplateDetail.params.PanelID + " #" + ddlId).find('option');
        var value = null;
        $ddlOptions.each(function () {

            if ($(this).attr('value') == liId) {
                value = $(this).attr('refname');
                return false;
            }
        });
        return value;
    },


    //Author: Muhammad Arshad
    //Date: 02-02-2016
    //This function will handle toggling of +ve/-ve checkboxes

    toggleCheckBoxes: function (chkObject) {
        //if (chkObject != null) {
        //    PhysicalExamTemplateDetail.isBothUnCheck = false;
        //    var isChecked = $(chkObject).prop("checked");
        //    //Start//25-02-2016//ahmad Raza//Logic to delete subCharacteristic on uncheck
        //    if ($(chkObject).parent().parent().parent().attr('id') == 'ulExamSubCharacteristics' && isChecked == false) {
        //        var subCharId = $(chkObject).attr('name');
        //        var subCharPKId = $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section#sectionPhysicalExam div#PhysicalExam #ulExamSubCharacteristics li#" + subCharId).data('SystemSubCharacteristicPk_' + subCharId);
        //        var physicalExamId = $('#' + PhysicalExamTemplateDetail.params.PanelID + " #hfPhysicalExamId").val();
        //        var charId = $(chkObject).parent().parent().attr('parentid');
        //        if (subCharPKId != '' && subCharPKId != null) {
        //            PhysicalExamTemplateDetail.deleteSubCharacteristicDetail('Characteristics', charId, physicalExamId, subCharPKId, subCharId);
        //        }
        //    }
        //    //End//25-02-2016//ahmad Raza//Logic to delete subCharacteristic on uncheck

        //    var examId = $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystems li.active").attr('id');
        //    var finalexam = examId + "exam";
        //    var sectionId = $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystemSection li.active").attr('id');
        //    var finalsectionids = [];
        //    if (PhysicalExamTemplateDetail.array.indexOf(sectionId) < 0) {
        //        finalsectionids.push(sectionId + "section");
        //    }

        //    var currentId = $(chkObject).attr("id");
        //    var character = [];
        //    if (PhysicalExamTemplateDetail.array.indexOf(currentId) < 0) {
        //        character.push(currentId + "character");
        //    }
        //    PhysicalExamTemplateDetail.array.push(finalexam, finalsectionids, character);
        //    var parentObj = $(chkObject).parent();
        //    parentObj.find("input[type='checkbox']").each(function (i, item) {
        //        $(item).prop("checked", false);
        //    });
        //    parentObj.find("input[id='" + currentId + "']").prop("checked", isChecked);
        //    if (isChecked == true) {
        //        //Start 12-02-2016 Humaira Yousaf
        //        $(parentObj).find('#btnShowSubCharacteristics' + $(chkObject).attr("name")).removeAttr('disabled');
        //        //End 12-02-2016 Humaira Yousaf
        //        var DetailExists = PhysicalExamTemplateDetail.isDetailsHaveData();;
        //        var isExist = PhysicalExamTemplateDetail.isDetailsHaveData();
        //        if (isExist == true) {

        //            var self = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #frmPhysicalExamTemplateDetail #CharacteristicsDetails')
        //            var myJSON = self != null ? self.getMyJSONByName() : "{}";

        //            var selectedJSON = JSON.parse(myJSON);

        //            var selectedLiId = PhysicalExamTemplateDetail.getNumberPart(selectedJSON)
        //            if (selectedLiId != null && selectedLiId != "") {
        //                if (PhysicalExamTemplateDetail.ExamDetails[selectedLiId] != null) {
        //                    PhysicalExamTemplateDetail.ExamDetails[selectedLiId] = PhysicalExamTemplateDetail.ExamDetails[selectedLiId].replace(PhysicalExamTemplateDetail.ExamDetails[selectedLiId], myJSON);
        //                }
        //                else {
        //                    PhysicalExamTemplateDetail.ExamDetails[selectedLiId] = myJSON;

        //                }
        //            }
        //        }
        //    }
        //    else {
        //        $(chkObject).closest('li').removeClass("green");
        //        $(parentObj).parent().removeClass("active");
        //        //Start 12-02-2016 Humaira Yousaf
        //        $(parentObj).find('#btnShowSubCharacteristics' + $(chkObject).attr("name")).attr('disabled', 'disabled');
        //        //End 12-02-2016 Humaira Yousaf
        //        var selectedLiId = $(parentObj).parent().attr("id");
        //    }

        //    // check selecteall for +ve checkbox if all child are checked
        //    var isAllPstiveChecked = false;
        //    var allPstivechk = parentObj.parent().parent().find("input[id*='+ve']").not("input[id='chkSelectAll+ve']");
        //    if (allPstivechk.filter(":checked").length == allPstivechk.length) {
        //        parentObj.parent().parent().find("input[id='chkSelectAll+ve']").prop("checked", true);
        //    }
        //    else {
        //        parentObj.parent().parent().find("input[id='chkSelectAll+ve']").prop("checked", false);
        //    }

        //    // check selecteall for -ve checkbox if all child are checked
        //    var allNgtivechk = parentObj.parent().parent().find("input[id*='-ve']").not("input[id='chkSelectAll-ve']");
        //    if (allNgtivechk.filter(":checked").length == allNgtivechk.length) {
        //        parentObj.parent().parent().find("input[id='chkSelectAll-ve']").prop("checked", true);
        //    }
        //    else {
        //        parentObj.parent().parent().find("input[id='chkSelectAll-ve']").prop("checked", false);
        //    }

        //    var currentIdPR = $(parentObj).parent().parent().attr("id");
        //    if (currentIdPR != null && currentIdPR != "")
        //        PhysicalExamTemplateDetail.setHiddenFieldValues(currentIdPR, currentId, parentObj);
        //}
    },

    //Author: Muhammad Arshad
    //Date: 02-02-2016
    //This function will check if any characteristic/subcharacteristic has data in details section
    isDetailsHaveData: function () {
        var DetailExists = false;
        var sectionDetails = "";
        var self = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #frmPhysicalExamTemplateDetail #CharacteristicsDetails').find('[type=hidden],[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
            if ($(this).prop("disabled") != true && DetailExists == false) {
                var currentElementTagName = this.tagName != null ? this.tagName : $(this).prop("tagName");
                if (($(this).attr('type') == 'text' || currentElementTagName.toLowerCase() == 'textarea') && $(this).val() != "") {
                    DetailExists = true;
                }
                if ($(this).attr('type') == 'checkbox' && this.checked == true) {
                    DetailExists = true;
                }
                if ($(this).attr('type') == 'radio' && $(this).attr('id').toLowerCase().indexOf("no") > -1 && this.checked == true) {
                    DetailExists = false;
                }
                if (currentElementTagName.toLowerCase() == 'select' && $(this).val() != null && $(this).val() != "") {
                    DetailExists = true;
                }
                if ($(this).attr('type') == 'hidden' && $(this).val() != "" && $(this).val() != "-1") {
                    DetailExists = true;
                }
            }
        });

        return DetailExists;
    },

    //Author: Muhammad Arshad
    //Date: 02-02-2016
    //This function will check if any characteristic/subcharacteristic has any value selected
    isDetailExists: function (TabType) {
        var DetailExists = false;
        var sectionDetails = "";
        var self = $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #PhysicalExam");
        var objCharacteristic = self.find("div#divExamCharacteristics");
        var objSubCharacteristic = self.find("section#CharacteristicsSubCharacteristics");

        if (objSubCharacteristic.hasClass("hidden") == false) {
            DetailExists = PhysicalExamTemplateDetail.isSystemChecked(objSubCharacteristic.find("ul#ulExamSubCharacteristics"));
            if (DetailExists == false) {
                DetailExists = PhysicalExamTemplateDetail.isSystemChecked(objCharacteristic.find("ul#ulExamCharacteristics"));
            }
        }
        else if (objCharacteristic.hasClass("hidden") == false) {
            DetailExists = PhysicalExamTemplateDetail.isSystemChecked(objCharacteristic.find("ul#ulExamCharacteristics"));
        }
        return DetailExists;

    },

    //Author: Muhammad Arshad
    //Date: 26-02-2016
    //This function will return comma Separated Ids of either selected Characteristics/SubCharacteristics from Given JSON on basis of characteristics Type as parameter
    getCommaSeparatedIds: function (arrSelectedJSON, IsSubCharacteristic) {
        var selectedIds = "";
        var isFirstSelected = false;
        //Start 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
        if (IsSubCharacteristic == true) {
            PhysicalExamTemplateDetail.subcharacteristicsWithData.length = 0;
            PhysicalExamTemplateDetail.selectedsubcharacteristicsIds.length = 0;
        }
        else if (IsSubCharacteristic == false) {
            PhysicalExamTemplateDetail.characteristicsWithData.length = 0;
            PhysicalExamTemplateDetail.selectedcharacteristicsIds.length = 0;
        }
        //End 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
        if (arrSelectedJSON != null)
            $.each(arrSelectedJSON, function (i, item) {
                var numericPart = 0;
                for (key in item) {
                    var myval = key;
                    if (key.indexOf("SubCharacteristicId") > -1) {
                        numericPart = key.replace(/[^\d]+/, '');
                    }
                }
                if (IsSubCharacteristic == true) {
                    var currentSubCharacteristicId = item["SubCharacteristicId" + numericPart];
                    var currentCharacteristicId = item["CharacteristicId" + numericPart];
                    if (currentSubCharacteristicId != null && currentSubCharacteristicId != "" && parseInt(currentSubCharacteristicId) > 0) {
                        if (isFirstSelected == false) {
                            selectedIds = currentSubCharacteristicId;
                            isFirstSelected = true;
                        }
                        else if (isFirstSelected == true) {
                            selectedIds += "," + currentSubCharacteristicId;
                        }
                        //Start 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
                        var num = PhysicalExamTemplateDetail.getNumberPart(item);

                        var Index = PhysicalExamTemplateDetail.selectedsubcharacteristicsIds.indexOf(num);
                        if (Index == -1 && item["CharacteristicId" + num] != num) {
                            PhysicalExamTemplateDetail.subcharacteristicsWithData.push(item);
                            PhysicalExamTemplateDetail.selectedsubcharacteristicsIds.push(num);
                        }
                        //End 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
                    }
                }
                else {
                    var currentSubCharacteristicId = item["CharacteristicId" + numericPart];
                    if (item["CharacteristicId" + numericPart] != "") {
                        var currentCharacteristicId = item["CharacteristicId" + numericPart];
                        if (currentCharacteristicId != null && currentCharacteristicId != "" && parseInt(currentCharacteristicId) > 0) {
                            if (isFirstSelected == false) {
                                selectedIds = currentCharacteristicId;
                                isFirstSelected = true;
                            }
                            else if (isFirstSelected == true) {
                                selectedIds += "," + currentCharacteristicId;
                            }
                            var currentSubCharacteristicId = item["SubCharacteristicId" + numericPart];
                            if (currentSubCharacteristicId == "" || currentSubCharacteristicId == "-1") {
                                //Start 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
                                PhysicalExamTemplateDetail.characteristicsWithData.push(item);
                                PhysicalExamTemplateDetail.selectedcharacteristicsIds.push(currentCharacteristicId);
                                //End 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
                            }

                        }
                    }
                }
            });

        return selectedIds;
    },

    //Author: Muhammad Arshad
    //Date: 26-02-2016
    //This function will handle load of child of either Systems/Section/Characteristics

    loadChildData: function (parentId, parentType) {
        var objDeffered = $.Deferred();
        if (parentType != null && parentType.toLowerCase() == "system") {

        }
        else if (parentType != null && parentType.toLowerCase() == "section") {

        }
        else if (parentType != null && parentType.toLowerCase() == "characteristics") {

        }
        else if (parentType != null && parentType.toLowerCase() == "subcharacteristics") {

        }
        PhysicalExamTemplateDetail.loadPhysicalExamStatuses(parentId, parentType).done(function () {

            objDeffered.resolve();

        });
        return objDeffered;
    },

    //Author: Abid Ali
    //Date: 09-02-2016
    //This function will remove newly items added to the lists
    deleteItem: function (obj, ctrlId, currentId) {

        var itemId = $(obj).closest("li").attr('id');

        if (ctrlId == "ulPhysicalExamSystems") {

            for (var counter in PhysicalExamTemplateDetail.selectedPhyExamTempData) {
                if (PhysicalExamTemplateDetail.selectedPhyExamTempData[counter].SystemId == itemId) {

                    PhysicalExamTemplateDetail.selectedPhyExamTempData.splice(counter, 1);
                }
            }

        } else if (ctrlId == "ulPhysicalExamSystemSection") {

            var isParentExist = false;
            for (var index in PhysicalExamTemplateDetail.selectedPhyExamTempData) {
                if (PhysicalExamTemplateDetail.selectedPhyExamTempData[index].SystemId == $(obj).closest("li").attr('parentid')) {

                    for (var counter in PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections) {
                        if (PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[counter].SectionId == itemId) {

                            PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections.splice(counter, 1);

                        }
                    }
                }
            }

        } else if (ctrlId == "ulExamCharacteristics") {

            for (var index in PhysicalExamTemplateDetail.selectedPhyExamTempData) {
                for (var innerIndex in PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections) {
                    if (PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].SectionId == $(obj).closest("li").attr('parentid')) {

                        for (var counter in PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics) {
                            if (PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[counter].CharacteristicId == itemId) {

                                PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics.splice(counter, 1);

                            }
                        }
                    }
                }
            }

        } else if (ctrlId == "ulExamSubCharacteristics") {

            for (var index in PhysicalExamTemplateDetail.selectedPhyExamTempData) {
                for (var innerIndex in PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections) {
                    for (var mostInnerIndex in PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics) {
                        if (PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].CharacteristicId == $(obj).closest("li").attr('parentid')) {

                            for (var counter in PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics) {
                                if (PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[counter].SubCharacteristicId == itemId) {

                                    PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics.splice(counter, 1);

                                }
                            }
                        }
                    }
                }
            }
        }
        PhysicalExamTemplateDetail.removeNewItemFromList(ctrlId, currentId);
    },

    removeNewItemFromList: function (ctrlId, currentId) {

        ctrlId = "#" + ctrlId;
        var self = $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #PhysicalExam ");
        var ulSystemId = "#ulPhysicalExamSystems";
        var ulSectionId = "#ulPhysicalExamSystemSection";
        var ulCharId = "#ulExamCharacteristics";
        var ulSubCharId = "#ulExamSubCharacteristics";

        if (ctrlId == ulSystemId) {

            var currentLi = self.find(ctrlId).find("li#" + currentId);
            var systemId = currentId;
            var sectionLi = self.find(ulSectionId).find("li[parentid *='" + systemId + "']");

            if (sectionLi.length > 0) {

                var sectionId = sectionLi.attr('id');
                var charLi = self.find(ulCharId).find("li[parentid *='" + sectionId + "']");

                if (charLi.length > 0) {

                    var charId = charLi.attr('id');
                    var subCharLi = self.find(ulSubCharId).find("li[parentid *='" + charId + "']");

                    if (subCharLi.length > 0) {
                        subCharLi.remove();
                    }
                    charLi.remove();
                }
                sectionLi.remove();
            }
            currentLi.remove();

        } else if (ctrlId == ulSectionId) {

            var currentLi = self.find(ctrlId).find("li#" + currentId);

            if (currentLi.length > 0) {

                var sectionId = currentLi.attr('id');
                var charLi = self.find(ulCharId).find("li[parentid *='" + sectionId + "']");

                if (charLi.length > 0) {

                    var charId = charLi.attr('id');
                    var subCharLi = self.find(ulSubCharId).find("li[parentid *='" + charId + "']");

                    if (subCharLi.length > 0) {
                        subCharLi.remove();
                    }
                    charLi.remove();
                }
                currentLi.remove();
            }

        } else if (ctrlId == ulCharId) {

            var currentLi = self.find(ctrlId).find("li#" + currentId);

            if (currentLi.length > 0) {

                var charId = currentLi.attr('id');
                var subCharLi = self.find(ulSubCharId).find("li[parentid *='" + charId + "']");

                if (subCharLi.length > 0) {
                    subCharLi.remove();
                }
                currentLi.remove();
            }
        }
        else if (ctrlId == ulSubCharId) {

            var currentLi = self.find(ctrlId).find("li#" + currentId);

            if (currentLi.length > 0) {
                currentLi.remove();
            }
        }
    },

    //Author: Muhammad Arshad
    //Date: 01-03-2016
    //This function will mark parent control as checked
    selectParentControls: function (chkObject) {
        if (chkObject != null) {
            var isChecked = $(chkObject).prop("checked");
            //if (isChecked == true) {

            //Start Farooq Ahmad 02-03-2016 Store the Selected Items in Json Object
            if ($(chkObject).closest("ul").attr("id") == "ulPhysicalExamSystems") {
                var obj = {
                    SystemId: $(chkObject).closest("li").attr('id'),
                    IsChecked: isChecked,
                    SystemName: $(chkObject).parent().find('label[id*="lblName"]').text(),
                    Sections: [],
                    IsModified: '1'
                };
                var isUpdated = false;
                for (var counter in PhysicalExamTemplateDetail.selectedPhyExamTempData) {
                    if (PhysicalExamTemplateDetail.selectedPhyExamTempData[counter].SystemId == obj.SystemId) {
                        PhysicalExamTemplateDetail.selectedPhyExamTempData[counter].IsModified = '1';
                        if (!isChecked) {
                            for (var innerIndex in PhysicalExamTemplateDetail.selectedPhyExamTempData[counter].Sections) {
                                PhysicalExamTemplateDetail.selectedPhyExamTempData[counter].Sections[innerIndex].IsChecked = isChecked;
                                PhysicalExamTemplateDetail.selectedPhyExamTempData[counter].Sections[innerIndex].IsModified = "1";
                                for (var mostInnerIndex in PhysicalExamTemplateDetail.selectedPhyExamTempData[counter].Sections[innerIndex].Characteristics) {
                                    PhysicalExamTemplateDetail.selectedPhyExamTempData[counter].Sections[innerIndex].Characteristics[mostInnerIndex].IsChecked = isChecked;
                                    PhysicalExamTemplateDetail.selectedPhyExamTempData[counter].Sections[innerIndex].Characteristics[mostInnerIndex].IsModified = "1";
                                    for (var innercounter in PhysicalExamTemplateDetail.selectedPhyExamTempData[counter].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics) {
                                        PhysicalExamTemplateDetail.selectedPhyExamTempData[counter].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[innercounter].IsChecked = isChecked;
                                        PhysicalExamTemplateDetail.selectedPhyExamTempData[counter].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[innercounter].IsModified = "1";
                                    }
                                }
                            }
                            $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulPhysicalExamSystemSection').find('input[type=checkbox]').prop("checked", false);
                            $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamCharacteristics').find('input[type=checkbox]').prop("checked", false);
                            $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamSubCharacteristics').find('input[type=checkbox]').prop("checked", false);

                            $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulPhysicalExamSystemSection').find('li').removeClass('green');
                            $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamCharacteristics').find('li').removeClass('green');
                            $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamSubCharacteristics').find('li').removeClass('green');

                        }
                        obj.Sections = PhysicalExamTemplateDetail.selectedPhyExamTempData[counter].Sections;
                        PhysicalExamTemplateDetail.selectedPhyExamTempData[counter] = obj;
                        isUpdated = true;
                    }
                }

                if (!isUpdated) {
                    if (PhysicalExamTemplateDetail.selectedPhyExamTempData == null)
                        PhysicalExamTemplateDetail.selectedPhyExamTempData = [];
                    PhysicalExamTemplateDetail.selectedPhyExamTempData.push(obj);
                }


            } else if ($(chkObject).closest("ul").attr("id") == "ulPhysicalExamSystemSection") {

                var isParentExist = false;
                for (var index in PhysicalExamTemplateDetail.selectedPhyExamTempData) {
                    if (PhysicalExamTemplateDetail.selectedPhyExamTempData[index].SystemId == $(chkObject).closest("li").attr('parentid')) {
                        PhysicalExamTemplateDetail.selectedPhyExamTempData[index].IsModified = '1';
                        var obj = {
                            SystemId: $(chkObject).closest("li").attr('parentid'),
                            SectionId: $(chkObject).closest("li").attr('id'),
                            IsChecked: isChecked,
                            SectionName: $(chkObject).parent().find('label[id*="lblName"]').text(),
                            Characteristics: [],
                            IsModified: '1'
                        };
                        var isUpdated = false;
                        for (var counter in PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections) {
                            if (PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[counter].SectionId == obj.SectionId) {
                                //PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[counter].IsModified = '1';
                                if (!isChecked) {

                                    for (var mostInnerIndex in PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[counter].Characteristics) {

                                        PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[counter].Characteristics[mostInnerIndex].IsChecked = isChecked;
                                        PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[counter].Characteristics[mostInnerIndex].IsModified = '1';

                                        for (var innercounter in PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[counter].Characteristics[mostInnerIndex].SubCharacteristics) {

                                            PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[counter].Characteristics[mostInnerIndex].SubCharacteristics[innercounter].IsChecked = isChecked;
                                            PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[counter].Characteristics[mostInnerIndex].SubCharacteristics[innercounter].IsModified = '1';
                                        }
                                    }

                                    $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamCharacteristics').find('input[type=checkbox]').prop("checked", false);
                                    $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamSubCharacteristics').find('input[type=checkbox]').prop("checked", false);


                                    $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamCharacteristics').find('li').removeClass('green');
                                    $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamSubCharacteristics').find('li').removeClass('green');

                                    if ($('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulPhysicalExamSystemSection').find('input:checked').length == 0) {

                                        var $systemLi = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulPhysicalExamSystems').find('li#' + obj.SystemId);

                                        $systemLi.find('input[type=checkbox]').prop("checked", false);
                                        $systemLi.removeClass('green');
                                        //Remove system from Json
                                        PhysicalExamTemplateDetail.selectedPhyExamTempData[index].IsModified = "1";
                                        PhysicalExamTemplateDetail.selectedPhyExamTempData[index].IsChecked = false;
                                    }

                                }
                                obj.Characteristics = PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[counter].Characteristics;
                                PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[counter] = obj;
                                isUpdated = true;
                            }
                        }
                        if (!isUpdated) {
                            if (PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections == null)
                                PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections = [];
                            PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections.push(obj);
                        }

                        isParentExist = true;
                    }
                }
                if (!isParentExist) {
                    var sectionId = $(chkObject).closest("li").attr('id');
                    var systemId = $(chkObject).closest("li").attr('parentid');
                    var systemName = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulPhysicalExamSystems #lblName' + systemId).text();//  $(chkObject).closest("li").attr('parentid');
                    var system = {
                        SystemId: systemId,
                        IsChecked: isChecked,
                        SystemName: systemName,
                        Sections: [{
                            SystemId: $(chkObject).closest("li").attr('parentid'),
                            SectionId: sectionId,
                            IsChecked: isChecked,
                            SectionName: $(chkObject).parent().find('label[id*="lblName"]').text(),
                            Characteristics: [],
                            IsModified: '1'
                        }],
                        IsModified: '1'
                    };
                    if (PhysicalExamTemplateDetail.selectedPhyExamTempData == null)
                        PhysicalExamTemplateDetail.selectedPhyExamTempData = [];
                    PhysicalExamTemplateDetail.selectedPhyExamTempData.push(system);
                }

            } else if ($(chkObject).closest("ul").attr("id") == "ulExamCharacteristics") {

                var isParentExist = false;
                for (var index in PhysicalExamTemplateDetail.selectedPhyExamTempData) {
                    for (var innerIndex in PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections) {
                        if (PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].SectionId == $(chkObject).closest("li").attr('parentid')) {

                            PhysicalExamTemplateDetail.selectedPhyExamTempData[index].IsModified = '1';
                            PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].IsModified = '1';

                            var obj = {
                                SectionId: $(chkObject).closest("li").attr('parentid'),
                                CharacteristicId: $(chkObject).closest("li").attr('id'),
                                CharName: $(chkObject).parent().find('label[id*="lblName"]').text(),
                                IsChecked: isChecked,
                                SubCharacteristics: [],
                                IsModified: '1'
                            };
                            var isUpdated = false;
                            for (var counter in PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics) {
                                if (PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[counter].CharacteristicId == obj.CharacteristicId) {


                                    if (!isChecked) {
                                        for (var innercounter in PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[counter].SubCharacteristics) {
                                            PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[counter].SubCharacteristics[innercounter].IsChecked = isChecked;
                                        }
                                        $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamSubCharacteristics').find('input[type=checkbox]').prop("checked", false);
                                        $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamSubCharacteristics').find('li').removeClass('green');
                                    }

                                    obj.SubCharacteristics = PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[counter].SubCharacteristics;

                                    if (!isChecked) {

                                        //Assign 1 isModified to child data
                                        if (obj.SubCharacteristics != null) {

                                            $.each(obj.SubCharacteristics, function (subCharIndex, item) {
                                                item.IsModified = '1';
                                            });
                                        }
                                    }

                                    PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[counter] = obj;
                                    isUpdated = true;
                                }
                            }
                            if (!isUpdated) {
                                if (PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics == null)
                                    PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics = [];
                                PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics.push(obj);
                            }

                            isParentExist = true;
                        }
                    }
                }
                if (!isParentExist) {
                    var sectionId = $(chkObject).closest("li").attr('parentid');
                    var systemId = $('#' + PhysicalExamTemplateDetail.params.PanelID + " #ulPhysicalExamSystemSection li#" + sectionId).attr('parentid');
                    var systemName = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulPhysicalExamSystems #lblName' + systemId).text();
                    var sectionName = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulPhysicalExamSystemSection #lblName' + sectionId).text();
                    for (var index in PhysicalExamTemplateDetail.selectedPhyExamTempData) {
                        if (PhysicalExamTemplateDetail.selectedPhyExamTempData.SystemId == systemId) {
                            var section = {
                                SystemId: systemId,
                                SectionId: sectionId,
                                IsChecked: isChecked,
                                SectionName: sectionName,
                                Characteristics: [{
                                    SectionId: $(chkObject).closest("li").attr('parentid'),
                                    CharacteristicId: $(chkObject).closest("li").attr('id'),
                                    CharName: $(chkObject).parent().find('label[id*="lblName"]').text(),
                                    IsChecked: isChecked,
                                    SubCharacteristics: [],
                                    IsModified: '1'
                                }],
                                IsModified: '1'
                            };
                            if (PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections == null)
                                PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections = [];
                            PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections.push(section);
                            isParentExist = true;
                        }
                    }
                    if (!isParentExist) {
                        var system = {
                            SystemId: systemId,
                            IsChecked: isChecked,
                            SystemName: systemName,
                            Sections: [{
                                SystemId: systemId,
                                SectionId: sectionId,
                                IsChecked: isChecked,
                                SectionName: sectionName,
                                Characteristics: [{
                                    SectionId: $(chkObject).closest("li").attr('parentid'),
                                    CharacteristicId: $(chkObject).closest("li").attr('id'),
                                    CharName: $(chkObject).parent().find('label[id*="lblName"]').text(),
                                    IsChecked: isChecked,
                                    SubCharacteristics: [],
                                    IsModified: '1'
                                }],
                                IsModified: '1'
                            }],
                            IsModified: '1'
                        };
                        if (PhysicalExamTemplateDetail.selectedPhyExamTempData == null)
                            PhysicalExamTemplateDetail.selectedPhyExamTempData = [];
                        PhysicalExamTemplateDetail.selectedPhyExamTempData.push(system);
                    }
                }

            } else if ($(chkObject).closest("ul").attr("id") == "ulExamSubCharacteristics") {
                var isParentExist = false;
                for (var index in PhysicalExamTemplateDetail.selectedPhyExamTempData) {
                    for (var innerIndex in PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections) {
                        for (var mostInnerIndex in PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics) {
                            if (PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].CharacteristicId == $(chkObject).closest("li").attr('parentid')) {

                                PhysicalExamTemplateDetail.selectedPhyExamTempData[index].IsModified = '1';
                                PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].IsModified = '1';
                                PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].IsModified = '1';

                                var obj = {
                                    CharacteristicId: $(chkObject).closest("li").attr('parentid'),
                                    SubCharacteristicId: $(chkObject).closest("li").attr('id'),
                                    SubCharName: $(chkObject).parent().find('label[id*="lblName"]').text(),
                                    IsChecked: isChecked,
                                    IsModified: '1'
                                };
                                var isUpdated = false;
                                for (var counter in PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics) {
                                    if (PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[counter].SubCharacteristicId == obj.SubCharacteristicId) {

                                        PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[counter] = obj;
                                        isUpdated = true;
                                    }
                                }
                                if (!isUpdated) {
                                    if (PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics == null)

                                        PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics = [];
                                    PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics.push(obj);
                                }

                                isParentExist = true;
                            }
                        }
                    }
                }
                if (!isParentExist) {
                    var characteristicId = $(chkObject).closest("li").attr('parentid');
                    var sectionId = $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section#sectionPhysicalExam div#PhysicalExam #ulExamCharacteristics li#" + characteristicId).attr('parentid');
                    var systemId = $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section#sectionPhysicalExam div#PhysicalExam #ulPhysicalExamSystemSection li#" + sectionId).attr('parentid');
                    var systemName = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulPhysicalExamSystems #lblName' + systemId).text();
                    var sectionName = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulPhysicalExamSystemSection #lblName' + sectionId).text();
                    var charName = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamCharacteristics #lblName' + characteristicId).text();
                    // $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section#sectionPhysicalExam div#PhysicalExam #ulExamCharacteristics li#" + characteristicId).addClass('green');
                    for (var index in PhysicalExamTemplateDetail.selectedPhyExamTempData) {
                        for (var innerIndex in PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections) {
                            if (PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].SectionId == sectionId) {
                                var char = {
                                    SectionId: sectionId,
                                    CharName: charName,
                                    IsChecked: isChecked,
                                    CharacteristicId: $(chkObject).closest("li").attr('parentid'),
                                    SubCharacteristics: [{
                                        CharacteristicId: $(chkObject).closest("li").attr('parentid'),
                                        SubCharacteristicId: $(chkObject).closest("li").attr('id'),
                                        SubCharName: $(chkObject).parent().find('label[id*="lblName"]').text(),
                                        IsChecked: isChecked,
                                        IsModified: '1'
                                    }],
                                    IsModified: '1'
                                };
                                if (PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics == null)
                                    PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics = [];
                                PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics.push(char);
                                isParentExist = true;
                            }
                        }
                    }
                    if (!isParentExist) {
                        for (var index in PhysicalExamTemplateDetail.selectedPhyExamTempData) {
                            if (PhysicalExamTemplateDetail.selectedPhyExamTempData.SystemId == systemId) {
                                var section = {
                                    SystemId: systemId,
                                    SectionId: sectionId,
                                    IsChecked: isChecked,
                                    SectionName: sectionName,
                                    Characteristics: [{
                                        SectionId: sectionId,
                                        CharName: charName,
                                        IsChecked: isChecked,
                                        CharacteristicId: $(chkObject).closest("li").attr('parentid'),
                                        SubCharacteristics: [{
                                            CharacteristicId: $(chkObject).closest("li").attr('parentid'),
                                            SubCharacteristicId: $(chkObject).closest("li").attr('id'),
                                            SubCharName: $(chkObject).parent().find('label[id*="lblName"]').text(),
                                            IsChecked: isChecked,
                                            IsModified: '1'
                                        }],
                                        IsModified: '1'
                                    }],
                                    IsModified: '1'
                                };
                                if (PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections == null)
                                    PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections = [];
                                PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections.push(section);
                                isParentExist = true;
                            }
                        }
                        if (!isParentExist) {
                            var system = {
                                SystemId: systemId,
                                IsChecked: isChecked,
                                SystemName: systemName,
                                Sections: [{
                                    SystemId: systemId,
                                    SectionId: sectionId,
                                    IsChecked: isChecked,
                                    SectionName: sectionName,
                                    Characteristics: [{
                                        SectionId: sectionId,
                                        CharName: charName,
                                        IsChecked: isChecked,
                                        CharacteristicId: $(chkObject).closest("li").attr('parentid'),
                                        SubCharacteristics: [{
                                            CharacteristicId: $(chkObject).closest("li").attr('parentid'),
                                            SubCharacteristicId: $(chkObject).closest("li").attr('id'),
                                            SubCharName: $(chkObject).parent().find('label[id*="lblName"]').text(),
                                            IsChecked: isChecked,
                                            IsModified: '1'
                                        }],
                                        IsModified: '1'
                                    }],
                                    IsModified: '1'
                                }],
                                IsModified: '1'
                            };
                            if (PhysicalExamTemplateDetail.selectedPhyExamTempData == null)
                                PhysicalExamTemplateDetail.selectedPhyExamTempData = [];
                            PhysicalExamTemplateDetail.selectedPhyExamTempData.push(system);
                        }

                    }
                }
            }
            // if ($(chkObject).prop('checked'))
            // $(chkObject).closest("li").addClass('green');
            // else {
            //  $(chkObject).closest("li").removeClass('green');
            // }
            //End Farooq Ahmad 02-03-2016 Store the Selected Items in Json Object

            var parentUlContrl = $(chkObject).parent().parent().parent();

            if (parentUlContrl != null && (parentUlContrl.attr("id") == "ulExamCharacteristics" || parentUlContrl.attr("id") == "ulExamSubCharacteristics")) {
                var currentParentId = $(chkObject).parent().parent().attr("parentid");

                if (parentUlContrl.attr("id") == "ulExamSubCharacteristics") {
                    var ParentCrtl = $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamCharacteristics");
                    var parentLi = ParentCrtl.find("li#" + currentParentId);
                    parentLi.find("input[id*='chk']").prop("checked", true);
                    var parentSysId = parentLi.attr("parentid");
                    PhysicalExamTemplateDetail.selectParentSysControls(parentSysId);
                }
                else if (parentUlContrl.attr("id") == "ulExamCharacteristics") {
                    PhysicalExamTemplateDetail.selectParentSysControls(currentParentId);
                }
            }
        }
    },

    //Author: Muhammad Arshad
    //Date: 01-03-2016
    //This function will mark parent system/subsystem control as checked
    selectParentSysControls: function (ParentLiId) {
        if (ParentLiId != null) {
            var ParentCrtl = $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystemSection");
            var parentLi = ParentCrtl.find("li#" + ParentLiId);
            parentLi.find("input[id*='chk']").prop("checked", true);

            var ParentSystCrtl = $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystems");
            var parentSysLi = ParentSystCrtl.find("li#" + parentLi.attr("parentid"));
            parentSysLi.find("input[id*='chk']").prop("checked", true);
        }
    },

    //Author: Muhammad Arshad
    //Date: 01-03-2016
    //This function will handle add new system/subsystem/characteristic/subcharacteristic
    addNewItem: function (itemType) {
        if (itemType != null && itemType != "") {

            var addSubCharIcon = "";

            charSelectAll = null;
            subCharSelectAll = null;

            var isSubCharacteristic = false;
            var ulControl = "";// $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #PhysicalExam");
            var currentLiClick = "";
            var currentCtrlId = "";
            var currentParentId = "";
            var currentId = "";
            currentId = PhysicalExamTemplateDetail.NewInsertedId--;
            var subcharacteristicExist = "";
            if (itemType.toLowerCase() == "system") {
                currentLiClick = "PhysicalExamTemplateDetail.showHideChildControls";
                currentCtrlId = "ulPhysicalExamSystems";
                ulControl = $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #" + currentCtrlId);
                var myval = "131";
                var myval = "133";
            }
            else if (itemType.toLowerCase() == "subsystem") {
                currentLiClick = "PhysicalExamTemplateDetail.showHideChildControls";
                currentCtrlId = "ulPhysicalExamSystemSection";
                ulControl = $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #" + currentCtrlId);
                var myval = "131";
                var myval = "133";
            }
            else if (itemType.toLowerCase() == "characteristic") {

                subcharacteristicExist = 'subcharacteristicExist = "true"';
                var addSubCharItem = "PhysicalExamTemplateDetail.addSubCharItem(event,this,'" + currentId + "');";

                addSubCharIcon = '<a class="btn btn-xs pull-right" href="#" onclick="' + addSubCharItem + '" title="Add SubCharacteristics"><i class="fa fa-plus blue"></i></a>';

                currentLiClick = "PhysicalExamTemplateDetail.showHideChildControls";
                currentCtrlId = "ulExamCharacteristics";
                ulControl = $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #" + currentCtrlId);

                charSelectAll = ulControl.find('li#chkboxSelectAllChars');

                var myval = "131";
                var myval = "133";
            }
            else if (itemType.toLowerCase() == "subcharacteristic") {
                currentLiClick = "PhysicalExamTemplateDetail.showHideChildControls";
                currentCtrlId = "ulExamSubCharacteristics";
                isSubCharacteristic = true;
                ulControl = $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #" + currentCtrlId);

                subCharSelectAll = ulControl.find('li#chkboxSelectAllSubChars');
                var myval = "131";
                var myval = "133";
            }
            if (ulControl != null && ulControl != "") {
                var currentLiClass = "";

                var arrNewlyAddedLi = ulControl.find("li[id*='-']");

                if (itemType.toLowerCase() != "system") {
                    currentParentId = ulControl.find("li:last").attr("parentid");
                    //Start Farooq Ahmad 16-03-2016 if no li is in ul
                    if (currentParentId == null)
                        currentParentId = ulControl.attr("ParentId");
                    //End Farooq Ahmad 16-03-2016 if no li is in ul
                }



                var onClick = "";// currentLiClick + "('" + currentCtrlId + "','" + String(currentId) + "');";
                //Start Farooq Ahmad 16-03-2016 set onclick prop
                onClick = "PhysicalExamTemplateDetail.showHideChildControls(this,'" + currentCtrlId + "','" + currentId + "');";

                var deleteFunction = "PhysicalExamTemplateDetail.deleteItem(this,'" + currentCtrlId + "','" + currentId + "');";

                var deleteIcon = '<a class="btn btn-xs pull-left" href="#" onclick="' + deleteFunction + '" title="Delete Record"><i class="fa fa-close red"></i></a>';
                //End Farooq Ahmad 16-03-2016 set onclick prop
                //Start Farooq Ahmad 03/03/2016 changing the on click function name
                liInnerText = '<div class="checkbox-custom checkboxTiny checkbox-success"><span id="btnOpenDetail' + currentId + '" onclick="PhysicalExamTemplateDetail.editName(this,\'' + currentId + '\');" class="btn btn-link btn-xs pull-left"><i class="fa fa-edit"></i></span>' + deleteIcon + addSubCharIcon + '<input checked type="checkbox" id="chk' + currentId + '+ve" name="' + currentId + '" class="pull-left  char" onclick="PhysicalExamTemplateDetail.selectParentControls(this);PhysicalExamTemplateDetail.toggleCheckBoxes(this);"><label id="lblName' + currentId + '" class=" hidden" data-toggle="tooltip"  title="" data-original-title="' + currentId + '">' + currentId + '</label><div id="divNameDetail' + currentId + '" class="rightInnerAddon pb-xs"><textarea rows="1" spellcheck="true" id="txtName' + currentId + '" onkeypress="PhysicalExamTemplateDetail.saveDetailComments(event,this)"  name="Name' + currentId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll"></textarea><div class="clearfix"></div><div class="rightInnerAddonBtn"><span id="btnSaveDetail' + currentId + '" onclick="PhysicalExamTemplateDetail.saveTemplateSysSecCharSubchar(this);" class="btn btn-link btn-xs"><i class="fa fa-save"></i></span></div></div><div class="clearfix"></div><div class="clearfix"></div></div>';
                //End Farooq Ahmad 03/03/2016 changing the on click function name

                var liTobeAdded = '<li id="' + currentId + '" ' + currentLiClass + ' parentid="' + currentParentId + '" onclick="' + onClick + '" value="' + currentId + '" refValue="' + currentId + '"' + subcharacteristicExist + '>' + liInnerText + '</li>';

                if (charSelectAll != null && charSelectAll.length > 0) {
                    $(liTobeAdded).insertAfter("#chkboxSelectAllChars");
                }
                else if (subCharSelectAll != null && subCharSelectAll.length > 0) {
                    $(liTobeAdded).insertAfter("#chkboxSelectAllSubChars");
                }
                else {
                    ulControl.prepend(liTobeAdded);
                }

                ulControl.find('li#' + currentId + ' #txtName' + currentId).focus()

            }

        }
    },

    FocusCommentTextBox: function (event) {
        event.stopPropagation();
    },

    addSubCharItem: function (event, obj, charId) {

        var $subChar = $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section#CharacteristicsSubCharacteristics");
        $subChar.removeClass('hidden');

        if ($subChar.find('#ulExamSubCharacteristics').attr('parentid') != charId)
            $subChar.find('#ulExamSubCharacteristics').empty();

        $subChar.find('#ulExamSubCharacteristics').attr('parentid', charId);

        PhysicalExamTemplateDetail.toggleVerticalWidth();
        PhysicalExamTemplateDetail.addNewItem('subcharacteristic');


        //PhysicalExamTemplateDetail.addNewItem("subcharacteristic");
        event.stopPropagation();
    },

    //Author: Farooq Ahmad
    //Date: 15-03-2016
    //This function will add new System Section Characteristics or Subcharacteristics
    addNewSysSecCharSubchar: function (event, obj) {
        if (event.which == 13) {
            event.preventDefault();
            $(obj).focus();
            $(obj).closest("ul").attr("id");
        }
    },

    //Author: Muhammad Arshad
    //Date: 01-03-2016
    //This function will handle show/hide of Name Label/Textbox
    editName: function (objButton, detailParentId, changeLabel) {
        if (objButton != null && detailParentId != null) {
            var liObject = $(objButton).parent().parent();
            var SystemDetailDiv = $(objButton).parent().find("div#divNameDetail" + detailParentId);
            var SystemNameLabel = $(objButton).parent().find("#lblName" + detailParentId);
            var txtSystemName = SystemDetailDiv.find("#txtName" + detailParentId);
            if (SystemDetailDiv.hasClass("hidden") == true) {
                SystemDetailDiv.removeClass("hidden");

                txtSystemName.val(SystemNameLabel.text());
                SystemNameLabel.addClass("hidden");
            }
            else {

                if (changeLabel != null) {
                    if (txtSystemName.val() != "") {
                        SystemNameLabel.text(txtSystemName.val());
                        SystemNameLabel.attr("data-original-title", txtSystemName.val());

                        txtSystemName.css({ "border": "1px solid #ccc" });
                    }
                    else {
                        //txtSystemName.css({ "border": "1px solid red" });
                        // return false;
                    }
                }

                SystemDetailDiv.addClass("hidden");
                SystemNameLabel.removeClass("hidden");

                if (changeLabel != null) {
                    PhysicalExamTemplateDetail.selectParentControls($(liObject).find('input:checkbox'));
                }

            }
        }
    },


    //closeEditIfTargetIsNotSame:function(event){

    //},
    //Author: Muhammad Arshad
    //Date: 26-02-2016
    //This function will handle fill of PhysicalExam childs
    loadPhysicalExamStatuses: function (parentId, parentType) {

        var currentLiClass = "";
        var currentLiClick = "";
        var currentCtrlId = "";
        var ParentDiv = "";
        var data = "";

        var selectedData = "";
        var templateId = PhysicalExamTemplateDetail.params.PhysicalExamTemplateId != null ? PhysicalExamTemplateDetail.params.PhysicalExamTemplateId : 0;

        if (parentType != null && parentType.toLowerCase() == "mainpesystem") {
            Crtl = '#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section#sectionPhysicalExam div#PhysicalExam #ulPhysicalExamSystems";
            currentLiClick = "PhysicalExamTemplateDetail.showHideChildControls";
            ParentDiv = "divPhysicalExamSystems";
            methodName = "GetPhysicalExamSystem";
            currentCtrlId = "ulPhysicalExamSystems";

            //   var templateId = PhysicalExamTemplateDetail.params.PhysicalExamTemplateId != null ? PhysicalExamTemplateDetail.params.PhysicalExamTemplateId : 0;
            data = "ID=" + templateId + "&ID2=0";

        }
        else if (parentType != null && parentType.toLowerCase() == "system") {
            Crtl = '#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section#sectionPhysicalExam div#PhysicalExam #ulPhysicalExamSystemSection";
            currentLiClick = "PhysicalExamTemplateDetail.showHideChildControls";
            ParentDiv = "divPhysicalExamSystemSection";
            methodName = "GetPhysicalExamSectionBySystemIdForTemplate"//"GetPhysicalExamSectionBySystemId";
            currentCtrlId = "ulPhysicalExamSystemSection";
            //Start 09-03-2016 Farooq Ahmad Sending Template ID as parameter
            data = "ID=" + parentId + "&ID2=" + templateId;
            //End 09-03-2016 Farooq Ahmad Sending Template ID as parameter
            //   PhysicalExamTemplateDetail.isNormalTriggred = selectedData != null ? selectedData.IsNormal : false;
        }
        else if (parentType != null && parentType.toLowerCase() == "section") {
            Crtl = '#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section#sectionPhysicalExam div#PhysicalExam #ulExamCharacteristics";
            currentLiClick = "PhysicalExamTemplateDetail.showHideChildControls";
            ParentDiv = "divExamCharacteristics";
            methodName = "GetPhysicalExamCharBySectionIdForTemplate"//"GetPhysicalExamCharcteristicBySectionId";
            currentCtrlId = "ulExamCharacteristics";
            //Start 09-03-2016 Farooq Ahmad Sending Template ID as parameter
            data = "ID=" + parentId + "&ID2=" + templateId;
            //End 09-03-2016 Farooq Ahmad Sending Template ID as parameter
        }
        else if (parentType != null && parentType.toLowerCase() == "characteristics") {
            Crtl = '#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section#sectionPhysicalExam div#PhysicalExam #ulExamSubCharacteristics";
            currentLiClick = "PhysicalExamTemplateDetail.showHideChildControls";
            ParentDiv = "divExamSubCharacteristics";

            methodName = "GetPhysicalExamSubCharByCharIdForTemplate"//"GetPhysicalExamSubCharcteristicByCharacteristicId";
            currentCtrlId = "ulExamSubCharacteristics";
            //Start 09-03-2016 Farooq Ahmad Sending Template ID as parameter
            data = "ID=" + parentId + "&ID2=" + templateId;
            //End 09-03-2016 Farooq Ahmad Sending Template ID as parameter
        }
        else if (parentType != null && parentType.toLowerCase() == "subcharacteristics") {
            //  Crtl = '#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section#sectionPhysicalExam div#PhysicalExam #CharacteristicsDetails";
            currentLiClick = "PhysicalExamTemplateDetail.showHideChildControls";
            ParentDiv = "amSubCharacteristics";
            methodName = "GetSocialHxCounsellingPeriod";
            currentCtrlId = "";
        }
        else {
            data = "ID=" + parentId;
        }

        return MDVisionService.lookups(methodName, true, data).done(function (result) {
            result = JSON.parse(result[methodName]);
            if ($(Crtl).length > 0)
                l = $(Crtl);
            if (parentType != null && parentType.toLowerCase() == "subcharacteristics") {
                return;
            }

            l.empty();


            var isFirstLi = true;
            var onClick = "";
            var appendSelectAll = false;

            var deffered = $.Deferred();

            var appenLis = function () {

                $.each(result, function (j, item) {
                    if (item.Value != "") {
                        if (isFirstLi == true) {
                            //currentLiClass = 'class="active"';
                            appendSelectAll = true;
                            isFirstLi = false;
                        }
                        else {
                            currentLiClass = "";
                        }
                        var physicalExamId = $('#' + PhysicalExamTemplateDetail.params.PanelID + " #hfPhysicalExamId").val();
                        var onClick = currentLiClick + "(this,'" + currentCtrlId + "','" + String(item.Value) + "');";
                        //Start//18-02-2016//Ahmad Raza//Delete system,section,Characteristics,SubCharacteristics detail
                        var deleteClick = "";
                        if (parentType.toLowerCase() == "system") {
                            deleteClick = "PhysicalExamTemplateDetail.deleteSectionDetail('" + parentType + "'," + parentId + "," + physicalExamId + "," + item.Value + ")";
                        }
                        else if (parentType.toLowerCase() == "section") {
                            deleteClick = "PhysicalExamTemplateDetail.deleteCharacteristicDetail('" + parentType + "'," + parentId + "," + physicalExamId + "," + item.Value + ")";
                        }
                        else if (parentType.toLowerCase() == "characteristics") {
                            deleteClick = "PhysicalExamTemplateDetail.deleteSubCharacteristicDetail('" + parentType + "'," + parentId + "," + physicalExamId + "," + item.Value + ")";
                        }
                        //End//18-02-2016//Ahmad Raza//Delete system,section,Characteristics,SubCharacteristics detail
                        //item.Value = item.Value == "" ? 0 : item.Value;
                        var liInnerText = '<a href="#' + ParentDiv + '">' + item.Name + '</a>';
                        var isSubCharacteristic = false;
                        if (parentType.toLowerCase() == "characteristics") {
                            isSubCharacteristic = true;
                        }

                        var selectAllInnerText = '<div class="pl-lg checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="chkboxSelectAllChars" name="Select All" class="pull-left" onclick="PhysicalExamTemplateDetail.selectAllChars(this);"><label id="lblSelectAll" class="" data-toggle="tooltip"  title="" data-original-title="Select All">Select All</label><div class="clearfix"></div><div class="clearfix"></div></div>';
                        var selectAllSubCharsInnerText = '<div class="pl-lg checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="chkboxSelectAllSubChars" name="Select All" class="pull-left" onclick="PhysicalExamTemplateDetail.selectAllSubChars(this);"><label id="lblSelectAll" class="" data-toggle="tooltip"  title="" data-original-title="Select All">Select All</label><div class="clearfix"></div><div class="clearfix"></div></div>';
                        //Start Farooq Ahmad 3/3/2016 Changing the on click function name 
                        if (item.RefName != "") {
                            liInnerText = '<div class="checkbox-custom checkboxTiny checkbox-success"><span id="btnOpenDetail' + item.Value + '" onclick="PhysicalExamTemplateDetail.editName(this,\'' + item.Value + '\');" class="btn btn-link btn-xs pull-left"><i class="fa fa-edit"></i></span><input type="checkbox" id="chk' + item.Value + '+ve" name="' + item.Value + '" class="pull-left  char" onclick="PhysicalExamTemplateDetail.selectParentControls(this);PhysicalExamTemplateDetail.toggleCheckBoxes(this);"><label id="lblName' + item.Value + '" class="" data-toggle="tooltip"  title="" data-original-title="' + item.Name + '">' + item.Name + '</label><div id="divNameDetail' + item.Value + '" class="rightInnerAddon pb-xs hidden"><textarea rows="1" spellcheck="true" id="txtName' + item.Value + '" onblur="" onkeypress="PhysicalExamTemplateDetail.saveDetailComments(event,this)" name="Name' + item.Value + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll"></textarea><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' + item.Value + '" onclick="PhysicalExamTemplateDetail.saveTemplateSysSecCharSubchar(this);" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><span id="btnShowSubCharacteristics' + item.Value + '" onclick="" class="pull-right" disabled="disabled"><i class="fa fa-caret-right blue"></i></span><div class="clearfix"></div><div class="clearfix"></div></div>';
                        }
                        else {
                            liInnerText = '<div class="checkbox-custom checkboxTiny checkbox-success"><span id="btnOpenDetail' + item.Value + '" onclick="PhysicalExamTemplateDetail.editName(this,\'' + item.Value + '\');" class="btn btn-link btn-xs pull-left"><i class="fa fa-edit"></i></span><input type="checkbox" id="chk' + item.Value + '+ve" name="' + item.Value + '" class="pull-left  char" onclick="PhysicalExamTemplateDetail.selectParentControls(this);PhysicalExamTemplateDetail.toggleCheckBoxes(this);"><label id="lblName' + item.Value + '" class="" data-toggle="tooltip"  title="" data-original-title="' + item.Name + '">' + item.Name + '</label><div id="divNameDetail' + item.Value + '" class="rightInnerAddon pb-xs hidden"><textarea rows="1" spellcheck="true" id="txtName' + item.Value + '" onkeypress="PhysicalExamTemplateDetail.saveDetailComments(event,this)" name="Name' + item.Value + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll"></textarea><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' + item.Value + '" onclick="PhysicalExamTemplateDetail.saveTemplateSysSecCharSubchar(this);" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div>';
                        }
                        if (parentType.toLowerCase() == "section" && appendSelectAll == true) {
                            l.append('<li id="chkboxSelectAllChars"  parentid="' + parentId + '"  value="Select All" refValue="Select All" >' + selectAllInnerText + '</li>');
                            appendSelectAll = false;
                        }
                        if (parentType.toLowerCase() == "characteristics" && appendSelectAll == true) {
                            l.append('<li id="chkboxSelectAllSubChars"  parentid="' + parentId + '"  value="Select All" refValue="Select All" >' + selectAllSubCharsInnerText + '</li>');
                            appendSelectAll = false;

                        }
                        //End Farooq Ahmad 3/3/2016 Changing the on click function name 
                        l.append('<li id="' + item.Value + '" ' + currentLiClass + ' parentid="' + parentId + '" onclick="' + onClick + '" value="' + item.Value + '" refValue="' + item.RefValue + '" subCharacteristicExist="' + item.RefName + ' ">' + liInnerText + '</li>');
                        // l.append('<li id="' + item.Value + '" ' + currentLiClass + ' parentid="' + parentId + '" onclick="' + onClick + '" value="' + item.Value + '" refValue="' + item.RefValue + '" subCharacteristicExist="' + item.RefName + ' ">' + liInnerText + '<span class="removeIconListHover" onclick="' + deleteClick + '" ><i class="fa fa-times"></i></span></li>');

                    }
                    if (j == result.length - 1) {
                        deffered.resolve();
                    }
                });
            }

            var loadGreenClasses = function () {


                //setTimeout(function (parentType, parentId) {
                if (parentType != null && parentType.toLowerCase() == "system") {

                    var objCurrentSys = $.grep(PhysicalExamTemplateDetail.selectedPhyExamTempData, function (item, index) {
                        return item.SystemId == parentId;
                    });
                    if (objCurrentSys != null && objCurrentSys.length > 0) {

                        if (!(objCurrentSys[0].Sections != null && objCurrentSys[0].Sections.length > 0) && PhysicalExamTemplateDetail.params.PhysicalExamTemplateId > 0 && objCurrentSys[0].IsModified != '1') {
                            PhysicalExamTemplateDetail.PhysicalExamTemplateDetailLoadOnDemand(PhysicalExamTemplateDetail.params.PhysicalExamTemplateId, parentId, null, null, 'Load_PhyscialExam_Template_Section').done(function (response) {
                                var response = JSON.parse(response);

                                var arrSections = JSON.parse(response.PhysicalExamTemplateSection);

                                $.each(arrSections, function (index, section) {
                                    section.IsModified = "1";
                                })

                                //for(var index = 0; index <PhysicalExamTemplateDetail.selectedPhyExamTempData  )

                                $.each(PhysicalExamTemplateDetail.selectedPhyExamTempData, function (index, sys) {

                                    if (sys.SystemId == parentId) {
                                        PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections = arrSections;
                                    }
                                });

                                // setTimeout(function (parentType, parentId) {
                                PhysicalExamTemplateDetail.addGreenClasses(parentType.toLowerCase(), parentId);
                                // }, 100, parentType, parentId);
                            });
                        }
                        setTimeout(function (parentType, parentId) {
                            PhysicalExamTemplateDetail.addGreenClasses(parentType.toLowerCase(), parentId);
                        }, 100, parentType, parentId);
                    }

                }
                else if (parentType != null && parentType.toLowerCase() == "section") {
                    var isExist = false;

                    $.each(PhysicalExamTemplateDetail.selectedPhyExamTempData, function (index, sys) {
                        if (sys.Sections != null && sys.Sections.length > 0) {
                            var objCurrentSec = $.grep(sys.Sections, function (item, index) {
                                return item.SectionId == parentId;
                            });
                            if (objCurrentSec.length > 0) {
                                if (!(objCurrentSec[0].Characteristics != null && objCurrentSec[0].Characteristics.length > 0) && objCurrentSec[0].IsModified == '1') {
                                    isExist = true;
                                    return false;
                                }
                            }
                        }
                    });
                    if (isExist && PhysicalExamTemplateDetail.params.PhysicalExamTemplateId > 0) {

                        //var dfd = $.Deferred();

                        PhysicalExamTemplateDetail.PhysicalExamTemplateDetailLoadOnDemand(PhysicalExamTemplateDetail.params.PhysicalExamTemplateId, null, parentId, null, 'Load_PhyscialExam_Template_Char').done(function (response) {
                            var response = JSON.parse(response);
                            var arrChar = JSON.parse(response.PhysicalExamTemplateChar);

                            $.each(arrChar, function (index, char) {
                                char.IsModified = "1";
                            });

                            $.each(PhysicalExamTemplateDetail.selectedPhyExamTempData, function (index, sys) {
                                if (sys.Sections != null && sys.Sections.length > 0) {

                                    $.each(sys.Sections, function (secindex, sec) {
                                        if (sec.SectionId == parentId) {
                                            PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[secindex].Characteristics = arrChar;
                                            return false;
                                        }

                                    });
                                }

                            });

                            setTimeout(function (parentType, parentId) {
                                PhysicalExamTemplateDetail.addGreenClasses(parentType.toLowerCase(), parentId);
                            }, 100, parentType, parentId);

                        });
                    }
                    setTimeout(function (parentType, parentId) {
                        PhysicalExamTemplateDetail.addGreenClasses(parentType.toLowerCase(), parentId);
                    }, 100, parentType, parentId);


                }
                else if (parentType != null && parentType.toLowerCase() == "characteristics") {


                    var isExist = false;

                    $.each(PhysicalExamTemplateDetail.selectedPhyExamTempData, function (index, sys) {
                        if (sys.Sections != null && sys.Sections.length > 0) {
                            $.each(sys.Sections, function (index, sec) {

                                if (sec.Characteristics != null && sec.Characteristics.length > 0) {
                                    var objCurrentChar = $.grep(sec.Characteristics, function (item, index) {
                                        return item.CharacteristicId == parentId;
                                    });
                                    if (objCurrentChar.length > 0) {
                                        if (!(objCurrentChar[0].SubCharacteristics != null && objCurrentChar[0].SubCharacteristics.length > 0) && objCurrentChar[0].IsModified == '1') {
                                            isExist = true;
                                            return false;
                                        }
                                    }

                                }
                            });
                            if (isExist) {
                                return false;
                            }
                        }

                    });
                    if (isExist && PhysicalExamTemplateDetail.params.PhysicalExamTemplateId > 0) {

                        PhysicalExamTemplateDetail.PhysicalExamTemplateDetailLoadOnDemand(PhysicalExamTemplateDetail.params.PhysicalExamTemplateId, null, null, parentId, 'Load_PhyscialExam_Template_SubChar').done(function (response) {

                            var response = JSON.parse(response);
                            var arrSubChar = JSON.parse(response.PhysicalExamTemplateSubChar);

                            $.each(arrSubChar, function (index, subChar) {
                                subChar.IsModified = "1";
                            });

                            $.each(PhysicalExamTemplateDetail.selectedPhyExamTempData, function (index, sys) {
                                if (sys.Sections != null && sys.Sections.length > 0) {
                                    $.each(sys.Sections, function (secindex, sec) {
                                        if (sec.Characteristics != null && sec.Characteristics.length > 0) {
                                            $.each(sec.Characteristics, function (charindex, char) {
                                                if (char.CharacteristicId == parentId) {
                                                    PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[secindex].Characteristics[charindex].SubCharacteristics = arrSubChar;
                                                    return false;
                                                }
                                            });
                                        }
                                    });
                                }

                            });

                            setTimeout(function (parentType, parentId) {
                                PhysicalExamTemplateDetail.addGreenClasses(parentType.toLowerCase(), parentId);
                            }, 100, parentType, parentId);

                        });
                    }

                    setTimeout(function (parentType, parentId) {
                        PhysicalExamTemplateDetail.addGreenClasses(parentType.toLowerCase(), parentId);
                    }, 100, parentType, parentId);

                }
                // }, 500, parentType, parentId);
                //deffered.done(function () {
                //    if (parentType != null && parentType.toLowerCase() == "system") {

                //        var objCurrentSys = $.grep(PhysicalExamTemplateDetail.selectedPhyExamTempData, function (item, index) {
                //            return item.SystemId == parentId;
                //        });

                //        if (!(objCurrentSys[0].Sections != null && objCurrentSys[0].Sections.length > 0)) {
                //            PhysicalExamTemplateDetail.PhysicalExamTemplateDetailLoadOnDemand(PhysicalExamTemplateDetail.params.PhysicalExamTemplateId, parentId, null, null, 'Load_PhyscialExam_Template_Section').done(function (response) {
                //                var response = JSON.parse(response);

                //                var arrSections = JSON.parse(response.PhysicalExamTemplateSection);

                //                //for(var index = 0; index <PhysicalExamTemplateDetail.selectedPhyExamTempData  )

                //                $.each(PhysicalExamTemplateDetail.selectedPhyExamTempData, function (index, sys) {

                //                    if (sys.SystemId == parentId) {
                //                        PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections = arrSections;
                //                    }
                //                });


                //            });
                //        }
                //        PhysicalExamTemplateDetail.addGreenClasses(parentType.toLowerCase(), parentId);

                //    }
                //    else if (parentType != null && parentType.toLowerCase() == "section") {
                //        var isExist = false;

                //        $.each(PhysicalExamTemplateDetail.selectedPhyExamTempData, function (index, sys) {
                //            if (sys.Sections != null && sys.Sections.length > 0) {
                //                var objCurrentSec = $.grep(sys.Sections, function (item, index) {
                //                    return item.SectionId == parentId;
                //                });
                //                if (objCurrentSec.length > 0) {
                //                    if (!(objCurrentSec[0].Characteristics != null && objCurrentSec[0].Characteristics.length > 0)) {
                //                        isExist = true;
                //                        return false;
                //                    }
                //                }
                //            }
                //        });
                //        if (isExist) {

                //            PhysicalExamTemplateDetail.PhysicalExamTemplateDetailLoadOnDemand(PhysicalExamTemplateDetail.params.PhysicalExamTemplateId, null, parentId, null, 'Load_PhyscialExam_Template_Char').done(function (response) {
                //                var response = JSON.parse(response);
                //                var arrChar = JSON.parse(response.PhysicalExamTemplateChar);


                //                $.each(PhysicalExamTemplateDetail.selectedPhyExamTempData, function (index, sys) {
                //                    if (sys.Sections != null && sys.Sections.length > 0) {
                //                        $.each(sys.Sections, function (secindex, sec) {
                //                            if (sec.SectionId == parentId) {
                //                                PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[secindex].Characteristics = arrChar;
                //                                return false;
                //                            }
                //                        });
                //                    }

                //                });
                //            });
                //        }
                //        PhysicalExamTemplateDetail.addGreenClasses(parentType.toLowerCase(), parentId);


                //    }
                //    else if (parentType != null && parentType.toLowerCase() == "characteristics") {


                //        var isExist = false;

                //        $.each(PhysicalExamTemplateDetail.selectedPhyExamTempData, function (index, sys) {
                //            if (sys.Sections != null && sys.Sections.length > 0) {
                //                $.each(sys.Sections, function (index, sec) {

                //                    if (sec.Characteristics != null && sec.Characteristics.length > 0) {
                //                        var objCurrentChar = $.grep(sec.Characteristics, function (item, index) {
                //                            return item.CharacteristicId == parentId;
                //                        });
                //                        if (objCurrentChar.length > 0) {
                //                            if (!(objCurrentChar[0].SubCharacteristics != null && objCurrentChar[0].SubCharacteristics.length > 0)) {
                //                                isExist = true;
                //                                return false;
                //                            }
                //                        }

                //                    }
                //                });
                //                if (isExist) {
                //                    return false;
                //                }
                //            }

                //        });
                //        if (isExist) {

                //            PhysicalExamTemplateDetail.PhysicalExamTemplateDetailLoadOnDemand(PhysicalExamTemplateDetail.params.PhysicalExamTemplateId, null, null, parentId, 'Load_PhyscialExam_Template_SubChar').done(function (response) {

                //                var response = JSON.parse(response);
                //                var arrSubChar = JSON.parse(response.PhysicalExamTemplateSubChar);


                //                $.each(PhysicalExamTemplateDetail.selectedPhyExamTempData, function (index, sys) {
                //                    if (sys.Sections != null && sys.Sections.length > 0) {
                //                        $.each(sys.Sections, function (secindex, sec) {
                //                            if (sec.Characteristics != null && sec.Characteristics.length > 0) {
                //                                $.each(sec.Characteristics, function (charindex, char) {
                //                                    if (char.CharacteristId == parentId) {
                //                                        PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[secindex].Characteristics[charindex].SubCharacteristics = arrSubChar;
                //                                        return false;
                //                                    }
                //                                });
                //                            }
                //                        });
                //                    }

                //                });
                //            });
                //        }

                //        PhysicalExamTemplateDetail.addGreenClasses(parentType.toLowerCase(), parentId);


                //    }
                //});

                // PhysicalExamTemplateDetail.addGreenClasses(parentType.toLowerCase(), parentId);
            }

            $.when(appenLis()).then(loadGreenClasses());

            //Start//28-07-2016//Ahmad Raza//Logic for SelectAll Characteristics

            var totalChars = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamCharacteristics li .char').length;
            var totalSelectedChars = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamCharacteristics li .char:checked').length;
            if (totalChars == totalSelectedChars) {
                $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamCharacteristics li#chkboxSelectAllChars input').prop("checked", true);
            }
            else {
                $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamCharacteristics li#chkboxSelectAllChars input').prop("checked", false);
            }
            //End//28-07-2016//Ahmad Raza//Logic for SelectAll Characteristics

            //Start//25-02-2016//Ahmad Raza//Setting ToolTip for Characteristics and subCharacteristics .
            $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
            //End//25-02-2016//Ahmad Raza//Setting ToolTip for Characteristics and subCharacteristics .

            // Added by Humaira Yousaf on Feb 9, 2016
            $('.textAreaScroll').slimScroll({
                position: 'right',
                height: '100%',
            });
        });
    },


    //Author: Ahmad Raza
    //Function Name: selectAllChars
    //Date: 27-07-2016
    //Description: This function will handle select all of characteristics
    selectAllChars: function (obj) {
        if ($(obj).prop('checked') == true) {
            $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamCharacteristics li .char').each(function () {
                if ($(this).attr("id") != $(obj).attr("id") && $(this).prop("checked") == false) {

                    $(this).parent().find("[type='checkbox'][id*='chk']").prop("checked", true);
                    PhysicalExamTemplateDetail.selectParentControls($(this).parent().find("[type='checkbox'][id*='chk']"));
                }

            });
        }
        else if ($(obj).prop('checked') == false) {
            $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamCharacteristics li .char').removeClass("green");
            $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamCharacteristics li .char').each(function () {
                if ($(this).attr("id") != $(obj).attr("id")) {
                    //this.checked = false;
                    //$(this).parent().find("[type='checkbox'][id*='chk']").trigger("click");
                    $(this).parent().find("[type='checkbox'][id*='chk']").prop("checked", false);
                    PhysicalExamTemplateDetail.selectParentControls($(this).parent().find("[type='checkbox'][id*='chk']"));
                }

            });
        }
    },

    //Author: Ahmad Raza
    //Function Name: selectAllSubChars
    //Date: 27-07-2016
    //Description: This function will handle select all of Sub characteristics
    selectAllSubChars: function (obj) {
        if ($(obj).prop('checked') == true) {
            $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamSubCharacteristics li .char').each(function () {
                if ($(this).attr("id") != $(obj).attr("id") && $(this).prop("checked") == false) {
                    //this.checked = true;
                    //$(this).parent().parent().trigger("click");
                    //$(this).parent().find("[type='checkbox'][id*='chk']").trigger("click");
                    $(this).parent().find("[type='checkbox'][id*='chk']").prop("checked", true);
                    PhysicalExamTemplateDetail.selectParentControls($(this).parent().find("[type='checkbox'][id*='chk']"));
                }

            });
        }
        else if ($(obj).prop('checked') == false) {
            $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamCharacteristics li .char').removeClass("green");
            $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamSubCharacteristics li .char').each(function () {
                if ($(this).attr("id") != $(obj).attr("id")) {
                    // $(this).parent().find("[type='checkbox'][id*='chk']").trigger("click");
                    $(this).parent().find("[type='checkbox'][id*='chk']").prop("checked", false);
                    PhysicalExamTemplateDetail.selectParentControls($(this).parent().find("[type='checkbox'][id*='chk']"));
                }

            });
        }
    },

    //Author: Muhammad Arshad
    //Date: 18-07-2016
    //This function will handle filtering of PhysicalExam Template Characteristics/Sub Characteristics
    filterOptions: function (obj, ulId) {
        if (obj != null && ulId != null) {
            var strSearch = $(obj).val();
            $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #" + ulId + " li").each(function () {
                var currentLiText = $(this).text();
                var showCurrentLi = currentLiText.toLowerCase().indexOf(strSearch.toLowerCase()) > -1 ? true : false;
                $(this).toggle(showCurrentLi);
            });

        }
    },

    addNewItemsFromJsonToList: function (listItems, itemType, parentId) {

        if (listItems != null && itemType != null && itemType != "") {

            var addSubCharIcon = "";

            charSelectAll = null;
            subCharSelectAll = null;

            var isSubCharacteristic = false;
            var ulControl = "";// $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #PhysicalExam");
            var currentLiClick = "";
            var currentCtrlId = "";
            var currentParentId = "";
            var currentId = "";
            var value = "";

            var subcharacteristicExist = "";
            if (itemType.toLowerCase() == "system") {
                currentLiClick = "PhysicalExamTemplateDetail.showHideChildControls";
                currentCtrlId = "ulPhysicalExamSystems";
                ulControl = $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #" + currentCtrlId);
                var myval = "131";
                var myval = "133";
            }
            else if (itemType.toLowerCase() == "subsystem") {
                currentLiClick = "PhysicalExamTemplateDetail.showHideChildControls";
                currentCtrlId = "ulPhysicalExamSystemSection";
                ulControl = $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #" + currentCtrlId);
                var myval = "131";
                var myval = "133";
            }
            else if (itemType.toLowerCase() == "characteristic") {

                subcharacteristicExist = 'subcharacteristicExist = "true"';
                var addSubCharItem = "PhysicalExamTemplateDetail.addSubCharItem(event,this,'" + currentId + "');";

                try {
                    if (listItems.length > 0 && listItems[0].SubCharacteristics != null && listItems[0].SubCharacteristics.length > 0)
                        addSubCharIcon = '<span id="btnShowSubCharacteristics' + currentId + '" onclick="" class="pull-right" disabled="disabled"><i class="fa fa-caret-right blue"></i></span>';
                    else
                        addSubCharIcon = '<a class="btn btn-xs pull-right" href="#" onclick="' + addSubCharItem + '" title="Add SubCharacteristics"><i class="fa fa-plus blue"></i></a>';
                }
                catch (ex) {
                    console.log(ex);
                }


                currentLiClick = "PhysicalExamTemplateDetail.showHideChildControls";
                currentCtrlId = "ulExamCharacteristics";
                ulControl = $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #" + currentCtrlId);

                charSelectAll = ulControl.find('li#chkboxSelectAllChars');

                var myval = "131";
                var myval = "133";
            }
            else if (itemType.toLowerCase() == "subcharacteristic") {
                currentLiClick = "PhysicalExamTemplateDetail.showHideChildControls";
                currentCtrlId = "ulExamSubCharacteristics";
                isSubCharacteristic = true;
                ulControl = $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #" + currentCtrlId);

                subCharSelectAll = ulControl.find('li#chkboxSelectAllSubChars');

                if (listItems.length > 0) {
                    // ulControl.removeClass('hidden');
                    $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section#CharacteristicsSubCharacteristics").removeClass('hidden');
                    //$('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamSubCharacteristics").empty()
                    PhysicalExamTemplateDetail.toggleVerticalWidth();
                }

                var myval = "131";
                var myval = "133";
            }

            $.each(listItems, function (index, item) {


                if (ulControl != null && ulControl != "") {
                    var currentLiClass = "";

                    if (itemType.toLowerCase() == "subsystem") {

                        currentId = item.SectionId;
                        currentParentId = item.SystemId;
                        value = item.SectionName;
                    }
                    else if (itemType.toLowerCase() == "characteristic") {

                        currentId = item.CharacteristicId;
                        currentParentId = item.SectionId;
                        value = item.CharName;

                        try {
                            if (item.SubCharacteristics != null && item.SubCharacteristics.length > 0)
                                addSubCharIcon = '<span id="btnShowSubCharacteristics' + currentId + '" onclick="" class="pull-right" disabled="disabled"><i class="fa fa-caret-right blue"></i></span>';
                            else
                                addSubCharIcon = '<a class="btn btn-xs pull-right" href="#" onclick="' + addSubCharItem + '" title="Add SubCharacteristics"><i class="fa fa-plus blue"></i></a>';
                        } catch (ex) {
                            console.log(ex);
                        }

                    }
                    else if (itemType.toLowerCase() == "subcharacteristic") {
                        currentId = item.SubCharacteristicId;
                        currentParentId = item.CharacteristicId;
                        value = item.SubCharName;
                    }
                    if (currentId > -1) {
                        return;
                    }
                    else if (parentId != currentParentId) {
                        return;
                    }
                    var onClick = "";
                    currentLiClick = currentLiClick + "(this,'" + currentCtrlId + "','" + currentId + "');";
                    var deleteFunction = "PhysicalExamTemplateDetail.deleteItem(this,'" + currentCtrlId + "','" + currentId + "');";

                    var deleteIcon = '<a class="btn btn-xs pull-left" href="#" onclick="' + deleteFunction + '" title="Delete Record"><i class="fa fa-close red"></i></a>';
                    //  liInnerText = '<div class="checkbox-custom checkboxTiny checkbox-success"><span id="btnOpenDetail' + currentId + '" onclick="PhysicalExamTemplateDetail.editName(this,\'' + currentId + '\');" class="btn btn-link btn-xs pull-left"><i class="fa fa-edit"></i></span>' + deleteIcon + addSubCharIcon + '<input checked type="checkbox" id="chk' + currentId + '+ve" name="' + currentId + '" class="pull-left  char" onclick="PhysicalExamTemplateDetail.selectParentControls(this);PhysicalExamTemplateDetail.toggleCheckBoxes(this);"><label id="lblName' + currentId + '" class=" hidden" data-toggle="tooltip"  title="" data-original-title="' + currentId + '">' + currentId + '</label><div id="divNameDetail' + currentId + '" class="rightInnerAddon pb-xs"><textarea rows="1" id="txtName' + currentId + '" onkeypress="PhysicalExamTemplateDetail.saveDetailComments(event,this)" name="Name' + currentId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll"></textarea><div class="clearfix"></div><div class="rightInnerAddonBtn"><span id="btnSaveDetail' + currentId + '" onclick="PhysicalExamTemplateDetail.saveTemplateSysSecCharSubchar(this);" class="btn btn-link btn-xs"><i class="fa fa-save"></i></span></div></div><div class="clearfix"></div><div class="clearfix"></div></div>';


                    var isChecked = item.IsChecked == true ? "checked" : "";

                    liInnerText = '<li id="' + currentId + '" parentid="' + currentParentId + '" onclick="' + currentLiClick + '" value="' + currentId + '" refvalue="" subcharacteristicexist=" "><div class="checkbox-custom checkboxTiny checkbox-success"><span id="btnOpenDetail"' + currentId + ' onclick="PhysicalExamTemplateDetail.editName(this,\'' + currentId + '\');" class="btn btn-link btn-xs pull-left"><i class="fa fa-edit"></i></span>' + deleteIcon + addSubCharIcon + '<input ' + isChecked + ' type="checkbox" id="chk' + currentId + '+ve" name="' + currentId + '" class="pull-left  char" onclick="PhysicalExamTemplateDetail.selectParentControls(this);PhysicalExamTemplateDetail.toggleCheckBoxes(this);"><label id="lblName' + currentId + '" class="" data-toggle="tooltip" title="" data-original-title="' + value + '" aria-describedby="tooltip913260">' + value + '</label><div id="divNameDetail' + currentId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" spellcheck="true" id="txtName' + currentId + '" onkeypress="PhysicalExamTemplateDetail.saveDetailComments(event,this)" name="Name' + currentId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px; background: rgb(0, 0, 0);"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; opacity: 0.2; z-index: 90; right: 1px; background: rgb(51, 51, 51);"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' + currentId + '" onclick="PhysicalExamTemplateDetail.saveTemplateSysSecCharSubchar(this);" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div></li>';




                    var liTobeAdded = liInnerText;//'<li id="' + item.Value + '" ' + currentLiClass + ' parentid="' + parentId + '" onclick="' + onClick + '" value="' + item.Value + '" refValue="' + item.RefValue + '" subCharacteristicExist="' + item.RefName + ' ">' + liInnerText + '</li>';

                    if (charSelectAll != null && charSelectAll.length > 0) {
                        $(liTobeAdded).insertAfter("#chkboxSelectAllChars");
                    }
                    else if (subCharSelectAll != null && subCharSelectAll.length > 0) {
                        $(liTobeAdded).insertAfter("#chkboxSelectAllSubChars");
                    }
                    else {
                        ulControl.prepend(liTobeAdded);
                    }
                }
            });

        }

    },


    //Author: Farooq Ahmad
    //Date: 09/03/2016
    //This function will handel if press enter in edit field
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
            onClickFunction = onClickFunction.replace('this', "$('#" + PhysicalExamTemplateDetail.params.PanelID + " #" + ULID + " #" + ID + "')");
            eval(onClickFunction);
        }
    },

    //Author: Muhammad Arshad
    //Date: 26-02-2016
    //This function will get number part from string
    getNumberPart: function (obj) {
        var innernumericPart = 0;
        $.each(obj, function (i, item) {
            if (i.indexOf("SystemId") > -1) {
                innernumericPart = i.replace(/[^\d]+/, '');
            }
        });
        return innernumericPart;
    },

    //Author: Muhammad Arshad
    //Date: 26-02-2016
    //This function will get object of clicked element
    getObjectOfClickedElement: function (parentType, parentId) {
        var objData = null;
        //retrieve data of sections from system li's
        if (parentType != null && parentType.toLowerCase() == "system") {
            var ctrl = '#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section#sectionPhysicalExam div#PhysicalExam #ulPhysicalExamSystems";
            objData = $(ctrl).find('li#' + parentId).data("SystemSectionIds_" + parentId);
        }
            //retrieve data of characteristics from section li's
        else if (parentType != null && parentType.toLowerCase() == "section") {
            var ctrl = '#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section#sectionPhysicalExam div#PhysicalExam #ulPhysicalExamSystemSection";
            objData = $(ctrl).find('li#' + parentId).data("SystemCharacteristicsIds_" + parentId);
        }
            //retrieve data of subCharacteristics from characteristics li's
        else if (parentType != null && parentType.toLowerCase() == "characteristics") {
            var ctrl = '#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section#sectionPhysicalExam div#PhysicalExam #ulExamCharacteristics";
            objData = $(ctrl).find('li#' + parentId).data("SystemSubCharacteristicsIds_" + parentId);
        }
        return objData;
    },

    //Author: Muhammad Arshad
    //Date: 26-02-2016
    //This function will handle enabling/disabling of Exercises controls on Miscellanous Tab
    enableDisableList: function (listId, isDisable) {
        if (listId != null && listId != "") {
            var self = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #frmPhysicalExamTemplateDetail ' + listId + " li").not(":first").each(function () {

                if (isDisable) {
                    if (!$(this).hasClass('disableAll'))
                        $(this).addClass('disableAll');
                }
                else
                    $(this).removeClass('disableAll');
            });

        }
    },

    //Author: Muhammad Arshad
    //Date: 26-02-2016
    //This function will handle setting/calculating width of PhysicalExam
    toggleVerticalWidth: function (obj) {

        var panelChildrens = null;
        var currentPanel = null;
        var applyWidth = null;

        if (obj != null) {
            currentPanel = $(obj.delegateTarget).parent();
            panelChildrens = currentPanel.children("section.panel");
            applyWidth = currentPanel;
            PhysicalExamTemplateDetail.toggleVerticalWidthCtrl(currentPanel, panelChildrens, applyWidth);
        }
        else {
            $('.toggleVertical').each(function (e) {
                currentPanel = $(this);
                panelChildrens = currentPanel.children().children("section.panel");
                applyWidth = currentPanel.children();
                PhysicalExamTemplateDetail.toggleVerticalWidthCtrl(currentPanel, panelChildrens, applyWidth);
            });
        }
    },

    //Author: Muhammad Arshad
    //Date: 26-02-2016
    //This function will handle calculating width of PhysicalExam

    toggleVerticalWidthCtrl: function (currentPanel, panelChildrens, applyWidth) {
        var totalWidth = 0;
        var hidden = 0;
        //find total width of panels
        panelChildrens.each(function (e) {
            totalWidth += $(this).outerWidth(true);
        });
        //find total width of hidden panel
        currentPanel.find("section.hidden").each(function (e) {
            hidden += $(this).outerWidth(true);
        });
        //apply width to div
        applyWidth.width(((totalWidth - hidden) + 60) + "px");

    },

    //Author: Muhammad Arshad
    //Date: 26-02-2016
    //This function will set the values in hidden field
    setHiddenFieldValues: function (currentUlId, currentId, parentObj) {
        var systemId = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #frmPhysicalExamTemplateDetail #ulPhysicalExamSystems li.active').attr("id");
        var sectionId = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #frmPhysicalExamTemplateDetail #ulPhysicalExamSystemSection li.active').attr("id");
        $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #frmPhysicalExamTemplateDetail #CharacteristicsDetails input[id*="hfSystemId"]').val(systemId);
        $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #frmPhysicalExamTemplateDetail #CharacteristicsDetails input[id*="hfSectionId"]').val(sectionId);
        var characteristicId = "";
        var isCharacteristicPostive = false;
        var isSubCharacteristicPostive = false;
        var subCharacteristicId = "";
        if (currentUlId.toLowerCase() == "ulexamcharacteristics") {
            if (currentId != null && currentId.indexOf("+ve") > -1) {
                isCharacteristicPostive = true;
            }
            characteristicId = $(parentObj).parent().attr("id");
            $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #frmPhysicalExamTemplateDetail #CharacteristicsDetails input[id*="hfCharacteristicId"]').val(characteristicId);
            $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #frmPhysicalExamTemplateDetail #CharacteristicsDetails input[id*="hfIsCharacteristicPositive"]').val(isCharacteristicPostive);


        }
        else if (currentUlId.toLowerCase() == "ulexamsubcharacteristics") {
            if (currentId != null && currentId.indexOf("+ve") > -1) {
                isSubCharacteristicPostive = true;
            }
            characteristicId = $(parentObj).parent().attr("parentid");
            subCharacteristicId = $(parentObj).parent().attr("id");

            $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #frmPhysicalExamTemplateDetail #CharacteristicsDetails input[id*="hfCharacteristicId"]').val(characteristicId);

            var chkOfCharacteristics = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #frmPhysicalExamTemplateDetail #ulExamCharacteristics li#' + characteristicId + '  input[type=checkbox]:checked').attr("id");
            if (chkOfCharacteristics != null && chkOfCharacteristics.indexOf("+ve") > -1) {
                isCharacteristicPostive = true;
            }
            $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #frmPhysicalExamTemplateDetail #CharacteristicsDetails input[id*="hfSubCharacteristicId"]').val(subCharacteristicId);
            $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #frmPhysicalExamTemplateDetail #CharacteristicsDetails input[id*="hfIsCharacteristicPositive"]').val(isCharacteristicPostive);
            $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #frmPhysicalExamTemplateDetail #CharacteristicsDetails input[id*="hfIsSubCharacteristicPositive"]').val(isSubCharacteristicPostive);
        }
    },

    //Author: Muhammad Arshad
    //Date: 26-02-2016
    //This function will handle show/hide of PhysicalExam child controls
    showHideChildControls: function (obj, parentCtrl, liId, event) {



        PhysicalExamTemplateDetail.parentCtrlGlobel = parentCtrl;

        var self = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #frmPhysicalExamTemplateDetail #CharacteristicsDetails')
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var selectedJSON = JSON.parse(myJSON);
        var selectedLiId = PhysicalExamTemplateDetail.getNumberPart(selectedJSON);

        if (selectedLiId != null && selectedLiId != "") {
            if (PhysicalExamTemplateDetail.ExamDetails[selectedLiId] != null) {
                PhysicalExamTemplateDetail.ExamDetails[selectedLiId] = PhysicalExamTemplateDetail.ExamDetails[selectedLiId].replace(PhysicalExamTemplateDetail.ExamDetails[selectedLiId], myJSON);
            }
            else {
                PhysicalExamTemplateDetail.ExamDetails[selectedLiId] = myJSON;

            }
        }
        //Start//09-02-2016//Ahmad Raza//Show/hide sections logic
        if (parentCtrl == "ulPhysicalExamSystems") {


            var charId = $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamSubCharacteristics").attr("parentId");
            var subSysId = $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamCharacteristics").attr("parentId");
            var sysId = $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystemSection").attr("parentId");

            if ($('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamCharacteristics li#" + charId).length == 0) {
                $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section#CharacteristicsSubCharacteristics").addClass('hidden');
            }
            if ($('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystemSection li#" + subSysId).length == 0) {
                $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section#SectionCharacteristics").addClass('hidden');
            }
            $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section#sectionPhysicalExamDetails").addClass('hidden');
            //$('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section#SectionCharacteristics").addClass('hidden');
            //$('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section#CharacteristicsSubCharacteristics").addClass('hidden');
        }
        if (parentCtrl == "ulPhysicalExamSystems" && liId != $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystems li.active").attr("id")) {

            $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section#sectionPhysicalExamDetails").addClass('hidden');
            $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section#SectionCharacteristics").addClass('hidden');
            $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section#CharacteristicsSubCharacteristics").addClass('hidden');
        }
        else if ((parentCtrl == "ulPhysicalExamSystemSection" && liId != $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystemSection li.active").attr("id"))) {
            $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section#sectionPhysicalExamDetails").addClass('hidden');
            $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section#CharacteristicsSubCharacteristics").addClass('hidden');

        }
        //End//09-02-2016//Ahmad Raza//Show/hide sections logic
        if (parentCtrl != null && parentCtrl != "") {

            var childPartialId = "";
            var isSystemSectionCtrl = "";
            var isCharacteristicsCtrl = "";
            var isSubCharacteristicsCtrl = "";
            if (parentCtrl.toLowerCase() == "ulphysicalexamsystems") {
                isSystemSectionCtrl = "1";
                childPartialId = "System";
                //$('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section[id^='SectionCharacteristics']").addClass("hidden");

            }
            else if (parentCtrl.toLowerCase() == "ulphysicalexamsystemsection") {
                isSystemSectionCtrl = "1";
                childPartialId = "Section";
            }
            else if (parentCtrl.toLowerCase() == "ulexamcharacteristics") {
                isCharacteristicsCtrl = "1";
                childPartialId = "Characteristics";
            }
            else if (parentCtrl.toLowerCase() == "ulexamsubcharacteristics") {
                isSubCharacteristicsCtrl = "1";
                childPartialId = "SubCharacteristics";
            }

            if (liId != null && liId != "") {
                $('#' + PhysicalExamTemplateDetail.parentCtrlGlobel).find("li").each(function (i, item) {
                    if ($(this).attr("id") != null && $(this).attr("id") == liId) {
                        //PhysicalExamTemplateDetail.loadChildData(liId, childPartialId);

                        var objCurrent = item;
                        $.when(PhysicalExamTemplateDetail.loadChildData(liId, childPartialId)).then(function () {

                            if (isSystemSectionCtrl == "1") {
                                if ($(objCurrent).hasClass("active") == false) {
                                    $(objCurrent).addClass("active");
                                    PhysicalExamTemplateDetail.SelectedSystem = objCurrent;
                                }
                                if (childPartialId != "" && childPartialId != "Characteristics") {

                                    $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section[id^='" + childPartialId + "']").removeClass("hidden");
                                    if ($('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail input[id='chkPhysicalExamsNormal']").prop("checked") == true) {

                                        // Added by Humaira Yousaf on Feb 9, 2016 to reset checkboxes
                                        var characteristicDiv = $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section#SectionCharacteristics");
                                        var subCharacteristicDiv = $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section#CharacteristicsSubCharacteristics");

                                        $(characteristicDiv).find("#ulExamCharacteristics > li").find('input:checkbox').prop('checked', false);
                                        $(subCharacteristicDiv).find("#ulExamSubCharacteristics> li").find('input:checkbox').prop('checked', false);
                                        // End
                                        $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section[id^='" + childPartialId + "'] li:first input[id='chkNormalSection']").trigger("click");
                                    }
                                    else {
                                        var system = null;
                                        if (childPartialId.toLowerCase() == "system") {
                                            var system = PhysicalExamTemplateDetail.getObjectOfClickedElement(childPartialId, liId);
                                            system = typeof system == 'undefined' ? null : system;
                                            var sectionExists = system != null ? (system.Sections != 'undefined' && system.Sections.length > 0 ? true : false) : false;
                                            var IsNormal = system != null ? system.IsNormal : false;
                                            if (($(objCurrent).hasClass("green") == true && !sectionExists && IsNormal)) {
                                                $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystemSection li:first input[id='chkNormalSection']").trigger("click");
                                            }
                                            else
                                                PhysicalExamTemplateDetail.enableDisableList('#ulPhysicalExamSystemSection', false);
                                        }
                                    }
                                }
                            }
                            else if (isCharacteristicsCtrl == "1") {
                                childPartialId = "Characteristics";
                                var objCheckBox = $(objCurrent).find("input[type='checkbox']");
                                var isChecked = false;
                                $.each(objCheckBox, function (i, item) {
                                    var id = $(item).attr("id");
                                    if (isChecked == false && $(item).prop("checked") == true) {
                                        isChecked = $(item).prop("checked");
                                    }
                                });
                                if ($(objCurrent).hasClass("active") == false) {
                                    $(objCurrent).addClass("active");
                                }
                                if ($.trim($(objCurrent).attr('subcharacteristicExist')) != "") {
                                    $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section[id^='" + childPartialId + "']").removeClass("hidden");
                                }
                                else {
                                    $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section[id^='" + childPartialId + "']").addClass("hidden");
                                }
                            }
                            else if (isSubCharacteristicsCtrl == "1") {
                                childPartialId = "SubCharacteristics";
                                var objCheckBox = $(objCurrent).find("input[type='checkbox']");
                                var isChecked = false;
                                $.each(objCheckBox, function (i, item) {
                                    var id = $(item).attr("id");
                                    if (isChecked == false && $(item).prop("checked") == true) {
                                        isChecked = $(item).prop("checked");
                                    }
                                });
                                if (isChecked == true) {
                                    if ($(objCurrent).hasClass("active") == false) {
                                        $(objCurrent).addClass("active");
                                    }
                                    $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section[id^='" + childPartialId + "']").removeClass("hidden");
                                }
                                else {
                                    $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail section[id^='" + childPartialId + "']").addClass("hidden");
                                }
                            }
                            PhysicalExamTemplateDetail.toggleVerticalWidth();


                            param1 = $(objCurrent).parent().attr('id');
                            param2 = $(objCurrent).find('div input[type="checkbox"]:checked').attr('id');
                            param3 = $(objCurrent).find('button').parent();

                            PhysicalExamTemplateDetail.setHiddenFieldValues(param1, param2, param3);

                            var self = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #frmPhysicalExamTemplateDetail #CharacteristicsDetails');

                            var finalStr = self != null ? self.getMyJSONByName() : "";

                            var selectedJSON = JSON.parse(finalStr);
                            var checkChar = '';
                            var subCheckChar = '';
                            var parentOfSubChar = 0;
                            if (PhysicalExamTemplateDetail.parentCtrlGlobel == "ulExamSubCharacteristics") {
                                $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #frmPhysicalExamTemplateDetail #ulExamSubCharacteristics .char').each(function () {
                                    if ($(this).prop("checked")) {
                                        parentOfSubChar = $(this).parent().parent().attr('parentid');
                                        if ($(this).attr('id').indexOf('-') > -1)
                                            subCheckChar += parseInt($(this).attr('id').replace(/[^\d]+/, '')) + 'N,';
                                        else
                                            subCheckChar += parseInt($(this).attr('id').replace(/[^\d]+/, '')) + 'P,';
                                    }
                                });
                                var subCharNum = PhysicalExamTemplateDetail.getNumberPart(selectedJSON);
                                if (subCheckChar.length > 0)
                                    subCheckChar = subCheckChar.substr(0, subCheckChar.length - 1);
                                selectedJSON['SubCharacteristicId' + parentOfSubChar] = subCheckChar;
                                var arrSelectedJSON = [];
                                arrSelectedJSON.push(selectedJSON);
                            }
                            if (PhysicalExamTemplateDetail.parentCtrlGlobel == "ulExamCharacteristics") {
                                $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #frmPhysicalExamTemplateDetail #ulExamCharacteristics .char').each(function () {
                                    if ($(this).prop("checked")) {
                                        if ($(this).attr('id').indexOf('-') > -1)
                                            checkChar += parseInt($(this).attr('id').replace(/[^\d]+/, '')) + 'N,';
                                        else
                                            checkChar += parseInt($(this).attr('id').replace(/[^\d]+/, '')) + 'P,';
                                    }
                                });
                                var charNum = PhysicalExamTemplateDetail.getNumberPart(selectedJSON);
                                if (checkChar.length > 0)
                                    checkChar = checkChar.substr(0, checkChar.length - 1);
                                selectedJSON['CharacteristicId' + charNum] = checkChar;
                                var arrSelectedJSON = [];
                                arrSelectedJSON.push(selectedJSON);
                            }
                            if (PhysicalExamTemplateDetail.parentCtrlGlobel.toLowerCase() == "ulexamcharacteristics") {
                                //Start Farooq Ahmad 18/02/2016 Store Charachteristics in JSON Array
                                var charNum = PhysicalExamTemplateDetail.getNumberPart(selectedJSON);
                                if (charNum != '') {
                                    var isAlreadyContain = false;
                                    objectToPop = -1;

                                    var charNumInner = PhysicalExamTemplateDetail.getNumberPart(selectedJSON);
                                    var val = selectedJSON['CharacteristicId' + charNumInner];
                                    selectedJSON['CharacteristicId' + charNumInner] = val.substr(val.indexOf(charNumInner), charNumInner.length + 1);

                                    for (var counter in PhysicalExamTemplateDetail.myArr) {
                                        if (PhysicalExamTemplateDetail.myArr[counter].hasOwnProperty('CharacteristicId' + charNum)) {
                                            PhysicalExamTemplateDetail.myArr[counter] = selectedJSON;
                                            objectToPop = counter;
                                            isAlreadyContain = true;
                                        }

                                    }
                                    if (isAlreadyContain == false)
                                        PhysicalExamTemplateDetail.myArr.push(selectedJSON);

                                    if ($('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamCharacteristics li#" + charNum + " input[type='checkbox']:checked").length == 0)
                                        if (parseInt(objectToPop) > -1) {
                                            PhysicalExamTemplateDetail.myArr.splice(parseInt(objectToPop), 1);
                                        }
                                }
                                var arrSelectedJSON = [];
                                arrSelectedJSON = PhysicalExamTemplateDetail.myArr;
                                //End Farooq Ahmad 18/02/2016 Store Charachteristics in JSON Array
                            }
                            if (PhysicalExamTemplateDetail.parentCtrlGlobel.toLowerCase() == "ulexamsubcharacteristics") {

                                for (var result in PhysicalExamTemplateDetail.myArr) {
                                    var finalsub = PhysicalExamTemplateDetail.myArr[result];
                                    var innernumericPartSub = 0;
                                    var subNumberValue = 0;
                                    $.each(finalsub, function (i, item) {
                                        if (i.indexOf("SystemId") > -1) {
                                            innernumericPartSub = i.replace(/[^\d]+/, '');
                                            subNumberValue = finalsub[i];
                                        }
                                    });
                                    if (innernumericPartSub == parentOfSubChar) {
                                        PhysicalExamTemplateDetail.myArr[result]["SubCharacteristicId" + innernumericPartSub] = subCheckChar;
                                    }
                                }
                            }
                            if (PhysicalExamTemplateDetail.parentCtrlGlobel.toLowerCase() == "ulphysicalexamsystems" || PhysicalExamTemplateDetail.parentCtrlGlobel.toLowerCase() == "ulphysicalexamsystemsection") {
                                arrSelectedJSON = [];
                                arrSelectedJSON = PhysicalExamTemplateDetail.myArr;
                            }

                            var numericPart = 0;
                            var selectedCharacteristicIds = PhysicalExamTemplateDetail.getCommaSeparatedIds(arrSelectedJSON, false);
                            if (arrSelectedJSON != null)
                                $.each(arrSelectedJSON, function (i, item) {

                                    for (key in item) {
                                        var myval = key;
                                        if (key.indexOf("SystemId") > -1) {
                                            numericPart = key.replace(/[^\d]+/, '');
                                        }
                                    }
                                });
                            if (arrSelectedJSON != null)
                                $.each(arrSelectedJSON, function (index, value) {
                                    numericPart = PhysicalExamTemplateDetail.getNumberPart(arrSelectedJSON[index]);

                                    if (arrSelectedJSON[index]["SystemId" + numericPart] == $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystems li.active").attr('id'))
                                        var charids = selectedCharacteristicIds.split(',');
                                    for (var out in charids) {
                                        var charnumpart = parseInt(charids[out]);
                                        if ($('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystemSection #" + arrSelectedJSON[index]["SectionId" + numericPart]).attr('id') == $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystemSection li.active").attr('id')) {

                                            if (charids[out].indexOf('N') > -1) {
                                                $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamCharacteristics li#" + charnumpart).find("input[id*='-ve']").prop("checked", true);
                                                //$('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamCharacteristics #chk" + charnumpart + "-ve").prop("checked", true);
                                            }
                                            if (charids[out].indexOf('P') > -1) {
                                                $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamCharacteristics li#" + charnumpart).find("input[id*='+ve']").prop("checked", true);
                                                //$('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamCharacteristics #chk" + charnumpart + "+ve").prop("checked", true);
                                            }
                                        }
                                        else {
                                            // $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystemSection #" + arrSelectedJSON[index]["SectionId" + numericPart]).addClass('green');
                                        }
                                    }
                                    var subNumPart = 0;
                                    if (PhysicalExamTemplateDetail.parentCtrlGlobel.toLowerCase() == "ulexamcharacteristics") {
                                        //var selectedSubCharacteristicIds = PhysicalExamTemplateDetail.getCommaSeparatedIds(arrSelectedJSON, true);

                                        for (var Count in PhysicalExamTemplateDetail.ExamDetails) {
                                            var CurrentObj = PhysicalExamTemplateDetail.ExamDetails[Count];
                                            CurrentObj = JSON.parse(CurrentObj);
                                            var CurrentIndex = PhysicalExamTemplateDetail.getNumberPart(CurrentObj)
                                            if (CurrentObj["CharacteristicId" + CurrentIndex] == liId) {
                                                var SubCharacteristicId = CurrentObj["SubCharacteristicId" + CurrentIndex];
                                                var IsSubCharacteristicPositive = CurrentObj["IsSubCharacteristicPositive" + CurrentIndex];
                                                if (SubCharacteristicId != "") {
                                                    if (IsSubCharacteristicPositive.toLowerCase() == "true") {
                                                        $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamSubCharacteristics li#" + SubCharacteristicId).find("input[id*='+ve']").prop("checked", true);
                                                        $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamSubCharacteristics li#" + SubCharacteristicId).find("input[id*='-ve']").prop("checked", false);
                                                    }
                                                    else {
                                                        $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamSubCharacteristics li#" + SubCharacteristicId).find("input[id*='+ve']").prop("checked", false);
                                                        $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamSubCharacteristics li#" + SubCharacteristicId).find("input[id*='-ve']").prop("checked", true);
                                                    }
                                                }

                                            }
                                        }
                                    }
                                });

                            param1 = $(objCurrent).parent().attr('id');
                            param2 = $(objCurrent).find('div input[type="checkbox"]:checked').attr('id');
                            param3 = $(objCurrent).find('div');
                            PhysicalExamTemplateDetail.setHiddenFieldValues(param1, param2, param3);

                            if ($(objCurrent).closest("ul").attr("id") == "ulPhysicalExamSystems") {
                                $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystemSection").attr("parentId", liId);
                                for (var index in PhysicalExamTemplateDetail.selectedPhyExamTempData) {
                                    for (var innerIndex in PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections) {
                                        //if ($.parseJSON(PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].IsChecked.toString().toLowerCase()))
                                        //$('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystemSection").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].SectionId).addClass("green");
                                        // else
                                        // $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystemSection").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].SectionId).removeClass("green");
                                        $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystemSection").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].SectionId + " input:checkbox").prop("checked", $.parseJSON(PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].IsChecked.toString().toLowerCase()));
                                        $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystemSection").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].SectionId + " label").text(PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].SectionName);
                                        $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystemSection").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].SectionId + " label").attr("data-original-title", PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].SectionName);
                                    }
                                }

                            } else if ($(objCurrent).closest("ul").attr("id") == "ulPhysicalExamSystemSection") {
                                $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamCharacteristics").attr("parentId", liId);
                                for (var index in PhysicalExamTemplateDetail.selectedPhyExamTempData) {
                                    for (var innerIndex in PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections) {
                                        for (var mostInnerIndex in PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics) {
                                            //if ($.parseJSON(PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].IsChecked.toString().toLowerCase()))
                                            //   $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamCharacteristics").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].CharacteristicId).addClass("green");
                                            // else
                                            //   $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamCharacteristics").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].CharacteristicId).removeClass("green");
                                            $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamCharacteristics").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].CharacteristicId + " input:checkbox").prop("checked", $.parseJSON(PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].IsChecked.toString().toLowerCase()));
                                            $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamCharacteristics").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].CharacteristicId + " label").text(PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].CharName);
                                            $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamCharacteristics").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].CharacteristicId + " label").attr("data-original-title", PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].CharName);
                                        }
                                    }
                                }
                                //Start//28-07-2016//Ahmad Raza//logic for select all characteristics
                                var totalChars = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamCharacteristics li .char').length;
                                var totalSelectedChars = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamCharacteristics li .char:checked').length;
                                if (totalChars == totalSelectedChars) {
                                    $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamCharacteristics li#chkboxSelectAllChars input').prop("checked", true);
                                }
                                else {
                                    $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamCharacteristics li#chkboxSelectAllChars input').prop("checked", false);
                                }
                                //End//28-07-2016//Ahmad Raza//logic for select all characteristics
                            }
                            else if ($(objCurrent).closest("ul").attr("id") == "ulExamCharacteristics") {
                                $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamSubCharacteristics").attr("parentId", liId);
                                for (var index in PhysicalExamTemplateDetail.selectedPhyExamTempData) {
                                    for (var innerIndex in PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections) {
                                        for (var mostInnerIndex in PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics) {
                                            for (var mostestInnerIndex in PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics) {
                                                //if ($.parseJSON(PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[mostestInnerIndex].IsChecked.toString().toLowerCase()))
                                                // $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamSubCharacteristics").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[mostestInnerIndex].SubCharacteristicId).addClass("green");
                                                // else
                                                // $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamSubCharacteristics").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[mostestInnerIndex].SubCharacteristicId).removeClass("green");
                                                $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamSubCharacteristics").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[mostestInnerIndex].SubCharacteristicId + " input:checkbox").prop("checked", $.parseJSON(PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[mostestInnerIndex].IsChecked.toString().toLowerCase()));
                                                $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamSubCharacteristics").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[mostestInnerIndex].SubCharacteristicId + " label").text(PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[mostestInnerIndex].SubCharName);
                                                $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamSubCharacteristics").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[mostestInnerIndex].SubCharacteristicId + " label").attr("data-original-title", PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[mostestInnerIndex].SubCharName);
                                            }

                                        }
                                    }
                                }

                            }
                            //Start//28-07-2016//Ahmad Raza//logic for select all sub characteristics
                            var totalSubChars = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamSubCharacteristics li .char').length;
                            var totalSelectedSubChars = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamSubCharacteristics li .char:checked').length;
                            if (totalSubChars == totalSelectedSubChars) {
                                $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamSubCharacteristics li#chkboxSelectAllSubChars input').prop("checked", true);
                            }
                            else {
                                $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamSubCharacteristics li#chkboxSelectAllSubChars input').prop("checked", false);
                            }

                        });
                    }
                    else {
                        if ($(this).hasClass("active") == true) {
                            $(this).removeClass("active");
                        }
                    }

                    //hide previous selected item.
                    if (PhysicalExamTemplateDetail.selectedListItem != null) {

                        var $Item = $(PhysicalExamTemplateDetail.selectedListItem);
                        var ul = $Item.closest('ul').attr('id');

                        if ($Item.attr('id') != $(obj).attr('id') || ul != $(obj).closest('ul').attr('id')) {

                            var isTextEmpty = false;

                            var id = $Item.attr('id');

                            var SystemDetailDiv = $Item.find("div#divNameDetail" + id);
                            var SystemNameLabel = $Item.find("#lblName" + id);
                            var txtSystemName = SystemDetailDiv.find("#txtName" + id);

                            // if (!isTextEmpty) {
                            SystemDetailDiv.addClass("hidden");
                            SystemNameLabel.removeClass("hidden");
                            // }
                        }
                    }
                    //selected item
                    PhysicalExamTemplateDetail.selectedListItem = obj;
                    //$(obj).clone().get(0).outerHTML;
                });
            }
        }
    },


    addGreenClasses: function (parentType, liId) {
        //Start Farooq Ahmad 02/03/2016 add green class if present in selectedphyexamtempdata
        if (parentType == "system") {

            var isSystemChecked = true;//$('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail div#divPhysicalExamSystems ul li#"+liId).find("input").prop('checked');

            $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystemSection").attr("parentId", liId);
            for (var index in PhysicalExamTemplateDetail.selectedPhyExamTempData) {
                for (var innerIndex in PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections) {
                    //if ($.parseJSON(PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].IsChecked.toString().toLowerCase()) && isSystemChecked)
                    //  $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystemSection").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].SectionId).addClass("green");
                    // else
                    // $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystemSection").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].SectionId).removeClass("green");
                    $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystemSection").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].SectionId + " input:checkbox").prop("checked", $.parseJSON(PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].IsChecked.toString().toLowerCase()) && isSystemChecked);
                    $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystemSection").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].SectionId + " label").text(PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].SectionName);
                    $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystemSection").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].SectionId + " label").attr("data-original-title", PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].SectionName);
                }
                if (PhysicalExamTemplateDetail.selectedPhyExamTempData[index].SystemId == liId)
                    PhysicalExamTemplateDetail.addNewItemsFromJsonToList(PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections, "subsystem", liId);
            }

        } else if (parentType == "section") {

            var isSectionChecked = true;//$('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulPhysicalExamSystemSection li#" + liId).find("input").prop('checked');
            $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamCharacteristics").attr("parentId", liId);
            for (var index in PhysicalExamTemplateDetail.selectedPhyExamTempData) {
                for (var innerIndex in PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections) {
                    for (var mostInnerIndex in PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics) {
                        //if ($.parseJSON(PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].IsChecked.toString().toLowerCase()) && isSectionChecked)
                        // $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamCharacteristics").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].CharacteristicId).addClass("green");
                        // else
                        //$('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamCharacteristics").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].CharacteristicId).removeClass("green");
                        $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamCharacteristics").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].CharacteristicId + " input:checkbox").prop("checked", $.parseJSON(PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].IsChecked.toString().toLowerCase()) && isSectionChecked);
                        $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamCharacteristics").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].CharacteristicId + " label").text(PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].CharName);
                        $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamCharacteristics").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].CharacteristicId + " label").attr("data-original-title", PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].CharName);
                    }
                    if (PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].SectionId == liId)
                        PhysicalExamTemplateDetail.addNewItemsFromJsonToList(PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics, "characteristic", liId);
                }
            }
            //Start//28-07-2016//Ahmad Raza//logic for select all characteristics
            var totalChars = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamCharacteristics li .char').length;
            var totalSelectedChars = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamCharacteristics li .char:checked').length;
            if (totalChars == totalSelectedChars) {
                $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamCharacteristics li#chkboxSelectAllChars input').prop("checked", true);
            }
            else {
                $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamCharacteristics li#chkboxSelectAllChars input').prop("checked", false);
            }
            //End//28-07-2016//Ahmad Raza//logic for select all characteristics
        }
        else if (parentType == "characteristics") {
            $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamSubCharacteristics").attr("parentId", liId);
            for (var index in PhysicalExamTemplateDetail.selectedPhyExamTempData) {
                for (var innerIndex in PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections) {
                    for (var mostInnerIndex in PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics) {
                        for (var mostestInnerIndex in PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics) {
                            //if ($.parseJSON(PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[mostestInnerIndex].IsChecked.toString().toLowerCase()))
                            //  $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamSubCharacteristics").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[mostestInnerIndex].SubCharacteristicId).addClass("green");
                            //else
                            //$('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamSubCharacteristics").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[mostestInnerIndex].SubCharacteristicId).removeClass("green");
                            $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamSubCharacteristics").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[mostestInnerIndex].SubCharacteristicId + " input:checkbox").prop("checked", $.parseJSON(PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[mostestInnerIndex].IsChecked.toString().toLowerCase()));
                            $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamSubCharacteristics").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[mostestInnerIndex].SubCharacteristicId + " label").text(PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[mostestInnerIndex].SubCharName);
                            $('#' + PhysicalExamTemplateDetail.params.PanelID + " #frmPhysicalExamTemplateDetail #ulExamSubCharacteristics").find("li#" + PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[mostestInnerIndex].SubCharacteristicId + " label").attr("data-original-title", PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics[mostestInnerIndex].SubCharName);
                        }
                        if (PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].CharacteristicId == liId)
                            PhysicalExamTemplateDetail.addNewItemsFromJsonToList(PhysicalExamTemplateDetail.selectedPhyExamTempData[index].Sections[innerIndex].Characteristics[mostInnerIndex].SubCharacteristics, "subcharacteristic", liId);
                    }
                }
            }
            var totalSubChars = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamSubCharacteristics li .char').length;
            var totalSelectedSubChars = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamSubCharacteristics li .char:checked').length;
            if (totalSubChars == totalSelectedSubChars) {
                $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamSubCharacteristics li#chkboxSelectAllSubChars input').prop("checked", true);
            }
            else {
                $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #ulExamSubCharacteristics li#chkboxSelectAllSubChars input').prop("checked", false);
            }
        }

    },

    LoadProvider: function () {
        $("#PhysicalExamTemplateDetail #pnlLicenseDetail").removeClass('disableAll');
        if (PhysicalExamTemplateDetail.params.mode == "Add") {
            $('#PhysicalExamTemplateDetail #txtShortName').attr("enabled", "enabled");

            $("#PhysicalExamTemplateDetail #pnlLicenseDetail").addClass('disableAll');
            PhysicalExamTemplateDetail.ValidateProvider();

            //serialize Data after all controls loaded.
            $('#frmPhysicalExamTemplateDetail').data('serialize', $('#frmPhysicalExamTemplateDetail').serialize());

        }
        else if (PhysicalExamTemplateDetail.params.mode == "Edit") {
            $('#PhysicalExamTemplateDetail #txtShortName').attr("disabled", "disabled");
            PhysicalExamTemplateDetail.LoadProviderLicense().done(function (response) {
                if (response.status != false) {

                    PhysicalExamTemplateDetail.ProviderLicenseGridLoad(response);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

            PhysicalExamTemplateDetail.FillProvider(PhysicalExamTemplateDetail.params.ProviderId).done(function (response) {
                if (response.status != false) {
                    var provider_detail = JSON.parse(response.ProviderFill_JSON);
                    var self = $("#PhysicalExamTemplateDetail");
                    utility.bindMyJSON(true, provider_detail, false, self).done(function () {

                        if (provider_detail.chkActive == 'True')
                            $("#PhysicalExamTemplateDetail #chkActive").attr("checked", true);
                        else
                            $("#PhysicalExamTemplateDetail #chkActive").attr("checked", false);

                        if (provider_detail.chkSpecialist == 'True')
                            $("#PhysicalExamTemplateDetail #chkSpecialist").attr("checked", true);
                        else
                            $("#PhysicalExamTemplateDetail #chkSpecialist").attr("checked", false);

                        $("#PhysicalExamTemplateDetail #pnlLicenseDetail").removeClass('disableAll');

                        PhysicalExamTemplateDetail.ValidateProvider();
                        //serialize Data after all controls loaded.
                        $('#frmPhysicalExamTemplateDetail').data('serialize', $('#frmPhysicalExamTemplateDetail').serialize());

                    });
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }

            });
        }
    },

    LoadEntityBasedData: function (entityID) {

        PhysicalExamTemplateDetail.LoadBasicFeeGroup(entityID).done(function () {

        });
        PhysicalExamTemplateDetail.LoadSupervisingProvider(entityID).done(function () {

        });
        PhysicalExamTemplateDetail.LoadSpecialty(entityID).done(function () {
            $('#frmPhysicalExamTemplateDetail').bootstrapValidator('revalidateField', $('#frmPhysicalExamTemplateDetail #ddlSpecialty').attr('name'));
        });
        if (entityID != "") {
            CacheManager.BindDropDownsByEntityID('#tblPhysicalExamTemplateDetail #ddlFeeGroup', 'GetFeeGroup', false, entityID);
        }
        else {
            CacheManager.BindDropDownsByEntityID('#tblPhysicalExamTemplateDetail #ddlFeeGroup', 'GetFeeGroup', true, null);
        }
    },

    LoadBasicFeeGroup: function (entityID) {
        // Loads Entity Based Basic Fee Group
        return PhysicalExamTemplateDetail.FillBasicFeeGroup(entityID).done(function (response) {
            if (response.status != false) {
                var basicfeegroup_detail = JSON.parse(response.BasicFeeGroupLoad_JSON);
                $("#PhysicalExamTemplateDetail #ddlBasicFeeGroup").empty();
                $("#PhysicalExamTemplateDetail #ddlBasicFeeGroup").append($('<option/>', {
                    value: "",
                    html: "- SELECT -"
                }));
                $.each(basicfeegroup_detail, function (i, item) {
                    $("#PhysicalExamTemplateDetail #ddlBasicFeeGroup").append(
                        $('<option/>', {
                            value: item.BasicFeeGroupId,
                            html: item.EntityName + " - " + item.ShortName
                        })
                    );
                });
            }

        });
    },

    LoadSupervisingProvider: function (entityID) {
        // Loads Entity Based Supervising Provider
        return PhysicalExamTemplateDetail.FillSupervisingProvider(entityID).done(function (response) {
            if (response.status != false) {
                var feegroup_detail = JSON.parse(response.SupervisingProviderLoad_JSON);
                $("#PhysicalExamTemplateDetail #ddlSupervisingProvider").empty();
                $("#PhysicalExamTemplateDetail #ddlSupervisingProvider").append($('<option/>', {
                    value: "",
                    html: "- SELECT -"
                }));
                $.each(feegroup_detail, function (i, item) {
                    $("#PhysicalExamTemplateDetail #ddlSupervisingProvider").append(
                        $('<option/>', {
                            value: item.ProviderId,
                            html: item.ShortName
                        })
                    );
                });
            }

        });
    },

    LoadSpecialty: function (entityID) {
        if (entityID != null && entityID > 0) {

            // Loads Entity Based Specialty
            return PhysicalExamTemplateDetail.FillSpecialty(entityID).done(function (response) {
                if (response.status != false) {
                    var feegroup_detail = JSON.parse(response.SpecialtyLoad_JSON);
                    $("#PhysicalExamTemplateDetail #ddlSpecialty").empty();
                    $("#PhysicalExamTemplateDetail #ddlSpecialty").append($('<option/>', {
                        value: "",
                        html: "- SELECT -"
                    }));
                    $.each(feegroup_detail, function (i, item) {
                        $("#PhysicalExamTemplateDetail #ddlSpecialty").append(
                            $('<option/>', {
                                value: item.SpecialtyId,
                                html: item.ShortName
                            })
                        );
                    });
                }

            });
        }
    },

    ValidateProvider: function () {
        $('#' + PhysicalExamTemplateDetail.params["PanelID"] + '  #frmPhysicalExamTemplateDetail')
         .bootstrapValidator({
             live: 'disabled',
             message: 'This value is not valid',
             feedbackIcons: {
                 valid: 'glyphicon glyphicon-ok',
                 invalid: 'glyphicon glyphicon-remove',
                 validating: 'glyphicon glyphicon-refresh'
             },
             fields: {
                 CDSTitle: {
                     group: '.col-sm-2',
                     validators: {
                         notEmpty: {
                             message: ''
                         }
                     }
                 },
                 CDSDeveloper: {
                     group: '.col-sm-2',
                     enabled: false,
                     validators: {
                         notEmpty: {
                             message: ''
                         }
                     }
                 },
                 CDSFundingSource: {
                     group: '.size60per',
                     enabled: false,
                     validators: {
                         notEmpty: {
                             message: ''
                         }
                     }
                 },
                 sex: {
                     group: '.col-sm-2',
                     validators: {
                         notEmpty: {
                             message: ''
                         }
                     }
                 },
                 speciality: {
                     group: '.col-sm-3',
                     validators: {
                         notEmpty: {
                             message: ''
                         }
                     }
                 },
                 qualification: {
                     group: '.col-sm-2',
                     validators: {
                         notEmpty: {
                             message: ''
                         }
                     }
                 },
                 npi: {
                     group: '.col-sm-2',
                     validators: {
                         notEmpty: {
                             message: ''
                         },
                         integer: {
                             message: ' '
                         }
                     }
                 },
                 profiletype: {
                     group: '.col-sm-2',
                     validators: {
                         notEmpty: {
                             message: ''
                         }
                     }
                 },
                 entity: {
                     group: '.col-sm-3',
                     validators: {
                         notEmpty: {
                             message: ''
                         }
                     }
                 },
                 providertype: {
                     group: '.col-sm-3',
                     validators: {
                         notEmpty: {
                             message: ''
                         }
                     }
                 },
                 Address: {
                     group: '.col-sm-3',
                     validators: {
                         notEmpty: {
                             message: ''
                         }
                     }
                 },
                 city: {
                     group: '.size60per',
                     validators: {
                         notEmpty: {
                             message: ''
                         }
                     }
                 },
                 state: {
                     group: '.size40per',
                     validators: {
                         notEmpty: {
                             message: ''
                         }
                     }
                 },
                 zip: {
                     group: '.size60per',
                     validators: {
                         notEmpty: {
                             message: ''
                         },
                     }
                 },
                 'email': {
                     group: '.col-sm-2',
                     enabled: false,
                     validators: {
                         emailAddress: {
                             message: 'Email not Valid'
                         }
                     }
                 },
                 'CDSReferenceURL': {
                     group: '.col-sm-2',
                     enabled: false,
                     validators: {
                         uri: {
                             message: 'Format not Valid'
                         }
                     }
                 },
                 'gender': {
                     feedbackIcons: false,
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
            PhysicalExamTemplateDetail.ProviderSave();
        });
    },

    ProviderSave: function () {
        $('#frmPhysicalExamTemplateDetail').data('serialize', $('#frmPhysicalExamTemplateDetail').serialize());
        var strMessage = "";
        var self = $("#PhysicalExamTemplateDetail");
        var myJSON = self.getMyJSON();
        if (PhysicalExamTemplateDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Provider", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    if (PhysicalExamTemplateDetail.params.ProviderId == "-1") {
                        PhysicalExamTemplateDetail.SaveProvider(myJSON).done(function (response) {
                            if (response.status != false) {
                                $("#PhysicalExamTemplateDetail #pnlLicenseDetail").removeClass('disableAll');
                                //  PhysicalExamTemplateDetail.params.ProviderId = response.ProviderId;

                                //Editable Grid
                                var PanelGrid = "#" + PhysicalExamTemplateDetail.params["PanelID"] + " #pnlLicenseDetail";
                                var GridId = "#" + PhysicalExamTemplateDetail.params["PanelID"] + " #dgvStateLicense";
                                utility.MakeEditableGrid(PanelGrid, GridId, PhysicalExamTemplateDetail);

                                //    Admin_Provider.ProviderSearch(response.ProviderId);

                                utility.DisplayMessages(response.message, 1);
                                $('#frmPhysicalExamTemplateDetail').data('serialize', $('#frmPhysicalExamTemplateDetail').serialize());
                                CacheManager.BindCodes('GetProvider', true);
                                UnloadActionPan(PhysicalExamTemplateDetail.params["ParentCtrl"], "PhysicalExamTemplateDetail");
                                Admin_Provider.ProviderSearch();
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                    else if (PhysicalExamTemplateDetail.params.ProviderId != "-1" && PhysicalExamTemplateDetail.params.ProviderId != "" && PhysicalExamTemplateDetail.params.ProviderId != "0") {
                        PhysicalExamTemplateDetail.UpdateProvider(myJSON, PhysicalExamTemplateDetail.params.ProviderId, 1).done(function (response) {
                            if (response.status != false) {
                                Admin_Provider.ProviderSearch(PhysicalExamTemplateDetail.params.ProviderId);
                                utility.DisplayMessages(response.message, 1);
                                CacheManager.BindCodes('GetProvider', true);
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
        else if (PhysicalExamTemplateDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Provider", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    PhysicalExamTemplateDetail.UpdateProvider(myJSON, PhysicalExamTemplateDetail.params.ProviderId, 1).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            CacheManager.BindCodes('GetProvider', true);
                            if (PhysicalExamTemplateDetail.params.RefCtrl != null && PhysicalExamTemplateDetail.params.RefCtrl != "") {
                                if (PhysicalExamTemplateDetail.params.ParentCtrl == "EncounterChargeCapture") {
                                    var PanelChargeGrid = "#" + EncounterChargeCapture.params.PanelID + " #pnlVisitCharge_Result";
                                    var ChargeGridId = "#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge";
                                    EditableGrid.initialize(ChargeGridId, EncounterChargeCapture, "0", false, false, false, false, undefined, false);
                                }

                                UnloadActionPan(PhysicalExamTemplateDetail.params["ParentCtrl"], "PhysicalExamTemplateDetail");

                            }
                            else {
                                Admin_Provider.ProviderSearch(PhysicalExamTemplateDetail.params.ProviderId);
                                UnloadActionPan(PhysicalExamTemplateDetail.params["ParentCtrl"], "PhysicalExamTemplateDetail");
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
    },

    // ------------ Provider License Region (Detail Grid)
    ProviderLicenseGridLoad: function (response) {

        if (response.ProviderLicenseCount > 0) {
            var ProviderLicenseJSON = JSON.parse(response.ProviderLicense_JSON);

            // get Actions
            var actions = "";
            $("#" + PhysicalExamTemplateDetail.params["PanelID"] + " #dgvStateLicense tr th").each(function () {
                if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                    var arrActionType = [];
                    if ($(this).attr("ActionType") != null) {
                        arrActionType = $(this).attr("ActionType").split(',');
                        actions = EditableGrid.GetActions(arrActionType);
                    }
                }
            });

            $.each(ProviderLicenseJSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($(this))");
                $row.attr("id", item.ProviderLicenseId);

                $row.append('<td class="actions" id="' + item.ProviderLicenseId + '" >' + actions + '</td><td>' + item.State + '</td><td>' + item.LicenseNo + '</td>');

                $("#" + PhysicalExamTemplateDetail.params["PanelID"] + " #dgvStateLicense tbody").last().append($row);
            });
        }

        //Editable Grid
        var PanelGrid = "#" + PhysicalExamTemplateDetail.params["PanelID"] + " #pnlLicenseDetail";
        var GridId = "#" + PhysicalExamTemplateDetail.params["PanelID"] + " #dgvStateLicense";
        utility.MakeEditableGrid(PanelGrid, GridId, PhysicalExamTemplateDetail);
    },

    SaveProviderLicense: function (ProviderLicenseData, RowId) {
        var data = "ProviderLicenseData=" + ProviderLicenseData + "&ProviderId=" + PhysicalExamTemplateDetail.params.ProviderId + "&RowId=" + RowId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER_DETAIL", "SAVE_LICENSE_INFO");
    },

    UpdateProviderLicense: function (ProviderLicenseData, ProviderLicenseID) {
        var data = "ProviderLicenseData=" + ProviderLicenseData + "&ProviderLicenseID=" + ProviderLicenseID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER_DETAIL", "UPDATE_LICENSE_INFO");
    },

    FillBasicFeeGroup: function (EntityID) {
        var data = "EntityID=" + EntityID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER_DETAIL", "FILL_BASIC_FEE_GROUP");
    },

    validateSelectedTemplateData: function () {

        var isValid = true;

        if ($(PhysicalExamTemplateDetail.selectedPhyExamTempData).length > 0) {
            $.each(PhysicalExamTemplateDetail.selectedPhyExamTempData, function (i, item) {


                if (item.IsModified == "1" && $.parseJSON(item.IsChecked.toString().toLowerCase()) == true) {

                    //isValid = false;

                    //if () {

                    isValid = false;

                    if ($(item.Sections).length > 0) {

                        $.each(item.Sections, function (i, item) {

                            isValid = false;

                            if (item.IsModified == "1" && $.parseJSON(item.IsChecked.toString().toLowerCase()) == true) {


                                if ($(item.Characteristics).length > 0) {

                                    $.each(item.Characteristics, function (counter, item) {

                                        if (item.IsModified == "1" && $.parseJSON(item.IsChecked.toString().toLowerCase()) == true) {
                                            isValid = true;
                                        }
                                    });
                                }
                                else {
                                    isValid = false;
                                }
                            }
                        });
                    }
                    else {
                        isValid = false;
                    }
                    // }
                }
            });
        }
        else {
            isValid = false;
        }
        return isValid;
    },

    //Author: Farooq Ahmad
    //Date: 02-03-2016
    //This function will save physical exam template
    physicalExamTemplateSave: function () {
        var isValid = false;
        var self = null;
        self = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #frmPhysicalExamTemplateDetail');

        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objData = JSON.parse(myJSON);
        objData.SpecialtyIds = objData.PhysicalExamTemplateSpecialty = PhysicalExamTemplateDetail.specialityCheckedIds.join();
        objData.ProviderIds = objData.PhysicalExamTemplateProvider = PhysicalExamTemplateDetail.providerCheckedIds.join();

        var isStillValid = false;


        //End 11-03-2016 Muhammad Arshad Checks if characteristic/sub-characteristic selected

        //Start 11-03-2016 Muhammad Arshad, perform checking if required data Exists prior to save Template
        if (PhysicalExamTemplateDetail.validateSelectedTemplateData()) {
            myJSON = JSON.stringify(objData);
            PhysicalExamTemplateDetail.savePhysicalExamTemplate(myJSON).done(function (response) {
                if (response != null && response != "") {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        if (response.phyExamTemplateId != "") {
                            PhysicalExamTemplateDetail.params.PhysicalExamTemplateId = response.phyExamTemplateId;
                            for (var count in PhysicalExamTemplateDetail.selectedPhyExamTempData) {
                                PhysicalExamTemplateDetail.selectedPhyExamTempData[count];
                            }
                        }
                        //PhysicalExamTemplateDetail.loadHospitalizationHx();

                        PhysicalExamTemplateDetail.params.mode = "Edit";
                        if (PhysicalExamTemplateDetail.params.ParentCtrl == "clinicalTabProgressNote" && UnloadHospitalizationhx == true) {
                            PhysicalExamTemplateDetail.getHospitalizationHxInfo(HospitalizationHxType, UnloadHospitalizationhx);
                        }
                        $('#' + PhysicalExamTemplateDetail.params.PanelID + " #hfHospitalizationHxId").val(response.HospitalizationHxId);
                        $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #frmClinicalHospitalizationHx').data('serialize', $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #frmClinicalHospitalizationHx').serialize());
                        //

                        // Empty global variables
                        PhysicalExamTemplateDetail.specialityCheckedIds = [];
                        PhysicalExamTemplateDetail.providerCheckedIds = [];
                        PhysicalExamTemplateDetail.providerSelectedIds = [];
                        PhysicalExamTemplateDetail.selectedPhyExamTempData = [];
                        PhysicalExamTemplateDetail.SpecialtyIds = "";
                        PhysicalExamTemplateDetail.ProviderIds = "";

                        //Start Farooq Ahmad 16-03-2016 Unload the Physical Exam on Save
                        UnloadActionPan(PhysicalExamTemplateDetail.params["ParentCtrl"], "PhysicalExamTemplateDetail");
                        if (PhysicalExamTemplate != null) {
                            PhysicalExamTemplate.loadPhysicalExamTemplate();
                        }
                        //End Farooq Ahmad 16-03-2016 Unload the Physical Exam on Save
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                }

            });
        }
        else {
            utility.DisplayMessages("Please select characteristic/Sub-characteristic", 3);
        }
        //End 11-03-2016 Muhammad Arshad, perform checking if required data Exists prior to save Template

    },

    //Author: Farooq Ahmad
    //Date: 02-03-2016
    //This function will handle save physical exam template
    //It represents service call to API
    savePhysicalExamTemplate: function (PhysicalExamTemplateData, TemplateName) {
        var self = null, IsActive = null;
        self = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #frmPhysicalExamTemplateDetail');
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objData = JSON.parse(myJSON);
        if (PhysicalExamTemplateDetail.params["mode"] == "Edit") {
            objData["TemplateId"] = (PhysicalExamTemplateDetail.params["PhysicalExamTemplateId"]);
        }
        else {
            objData["TemplateId"] = '-1';
        }

        if (TemplateName != null) {

            var mainTemplateName = $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #frmPhysicalExamTemplateDetail #txtPhysicalExamTemplateName').val();

            if (objData["TemplateId"] == '-1') {
                objData["PhysicalExamTemplateName"] = TemplateName;
                IsActive = "1";
            }
            else {
                objData["PhysicalExamTemplateName"] = mainTemplateName != "" ? mainTemplateName : TemplateName;
            }

            objData["SaveAsTemplateName"] = TemplateName;
        }

        objData["SpecialtyIds"] = objData["PhysicalExamTemplateSpecialty"];
        objData["ProviderIds"] = objData["PhysicalExamTemplateProvider"];
        objData.SpecialtyIds = objData.PhysicalExamTemplateSpecialty = PhysicalExamTemplateDetail.specialityCheckedIds.join();
        objData.ProviderIds = objData.PhysicalExamTemplateProvider = PhysicalExamTemplateDetail.providerCheckedIds.join();
        objData["commandType"] = "Save_PhyscialExam_Template";
        objData["SysSecCharSubcharData"] = PhysicalExamTemplateDetail.selectedPhyExamTempData;


        IsActive = $('#' + PhysicalExamTemplate.params.PanelID + ' #pnlPhysicalExamTemplate_Result #divSwitch #switchActive').attr('isactive');
        if (IsActive == null) {
            IsActive = "1";
        }

        objData["IsActive"] = IsActive;

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },

    //Author: Farooq Ahmad
    //Date: 02-03-2016
    //This function will handle save Template System or Section or Characteristics or SubCharacteristics
    saveTemplateSysSecCharSubchar: function (obj) {

        var MainId = '', Type = '';
        var objData = [];
        if ($(obj).closest("ul").attr("id") == "ulPhysicalExamSystems") {
            objData["MainId"] = $(obj).closest("li").attr('id');
            objData["Type"] = "PhysicalExamSystems";
        } else if ($(obj).closest("ul").attr("id") == "ulPhysicalExamSystemSection") {
            objData["MainId"] = $(obj).closest("li").attr('id');
            objData["Type"] = "PhysicalExamSystemSection";
        } else if ($(obj).closest("ul").attr("id") == "ulExamCharacteristics") {
            objData["MainId"] = $(obj).closest("li").attr('id');
            objData["Type"] = "ExamCharacteristics";
        } else if ($(obj).closest("ul").attr("id") == "ulExamSubCharacteristics") {
            objData["MainId"] = $(obj).closest("li").attr('id');
            objData["Type"] = "ExamSubCharacteristics";
        }

        var btnEdit = $(obj).parent().parent().parent().find("span[id^='btnOpenDetail']").get(0);

        PhysicalExamTemplateDetail.editName(btnEdit, objData["MainId"], true);
    },


    //Author: Farooq Ahmad
    //Date: 02-03-2016
    //This function will validate physical exam template
    validatePhysicalExamTemplate: function () {
        $('#' + PhysicalExamTemplateDetail.params.PanelID + ' #frmPhysicalExamTemplateDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   PhysicalExamTemplateName: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   PhysicalExamTemplateEntity: {
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
            PhysicalExamTemplateDetail.physicalExamTemplateSave();
        });
    },

    //Author: Farooq Ahmad
    //Date : 07-01-2016
    saveAsPhysicalExam: function () {
        var strMessage = "";
        var params = [];
        params["FromAdmin"] = '0';
        params["TabID"] = "PhysicalExamTemplateSaveAs";
        params["ParentCtrl"] = 'PhysicalExamTemplateDetail';


        LoadActionPan('PhysicalExamTemplateSaveAs', params, PhysicalExamTemplateDetail.params.PanelID);

    },

    FillSupervisingProvider: function (EntityID) {
        var data = "EntityID=" + EntityID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER_DETAIL", "FILL_SUPERVISING_PROVIDER");
    },

    FillSpecialty: function (EntityID) {
        var data = "EntityID=" + EntityID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER_DETAIL", "FILL_SPECIALTY");
    },

    LoadProviderLicense: function () {
        var data = "ProviderId=" + PhysicalExamTemplateDetail.params.ProviderId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER_DETAIL", "LOAD_LICENSE_INFO");
    },

    DeleteProviderLicense: function (ProviderLicenseId) {
        var data = "ProviderLicenseId=" + ProviderLicenseId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER_DETAIL", "DELETE_LICENSE_INFO");
    },

    UnLoadPlan: function () {

        if ($('#frmProviderLicense').serialize() != $('#frmProviderLicense').data('serialize')) {
            utility.myConfirm('2', function () {
                $('#ProviderLicenseDetailGrid').modal('hide');
            }, function () { },
                    '2'
                );
        }
        else {
            $('#ProviderLicenseDetailGrid').modal('hide');
        }


    },
    //----------------------------------------------------------------

    SaveProvider: function (ProviderData) {
        var data = "ProviderData=" + ProviderData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER_DETAIL", "SAVE_PROVIDER");
    },

    UpdateProvider: function (ProviderData, ProviderID, IsActive) {
        var data = "ProviderData=" + ProviderData + "&ProviderID=" + ProviderID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER_DETAIL", "UPDATE_PROVIDER");
    },

    FillProvider: function (ProviderID) {
        var data = "ProviderID=" + ProviderID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER_DETAIL", "FILL_PROVIDER");
    },

    UpdateProviderActiveInactive: function (ProviderID, IsActive) {
        var data = "ProviderID=" + ProviderID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER_DETAIL", "UPDATE_PROVIDER_ACTIVE_INACTIVE");
    },

    UnLoad: function () {

        // Empty global variables
        PhysicalExamTemplateDetail.specialityCheckedIds = [];
        PhysicalExamTemplateDetail.providerCheckedIds = [];
        PhysicalExamTemplateDetail.providerSelectedIds = [];
        PhysicalExamTemplateDetail.selectedPhyExamTempData = [];
        PhysicalExamTemplateDetail.SpecialtyIds = "";
        PhysicalExamTemplateDetail.ProviderIds = "";

        utility.UnLoadDialog("frmPhysicalExamTemplateDetail", function () {

            UnloadActionPan(PhysicalExamTemplateDetail.params["ParentCtrl"], "PhysicalExamTemplateDetail");
            if (PhysicalExamTemplate != null) {
                PhysicalExamTemplate.loadPhysicalExamTemplate();
            }
        }, function () {

            UnloadActionPan(PhysicalExamTemplateDetail.params["ParentCtrl"], "PhysicalExamTemplateDetail");
        });
        if (PhysicalExamTemplateDetail.params.ParentCtrl == "EncounterChargeCapture") {
            var PanelChargeGrid = "#" + EncounterChargeCapture.params.PanelID + " #pnlVisitCharge_Result";
            var ChargeGridId = "#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge";
            EditableGrid.initialize(ChargeGridId, EncounterChargeCapture, "0", false, false, false, false, undefined, false);
        }

    },


    //-------------------Editable Grid Methods Starts---

    rowSave: function ($row, obj) {

        if (obj.rowValidate($row)) {

            var _self = obj,
            $actions,
            values = [];

            if ($row.hasClass('adding')) {
                $row.removeClass('adding');
            }

            values = $row.find('td').map(function () {

                var $this = $(this);

                if ($this.hasClass('expand')) {
                    return '<a href="#" class="hidden on-editing expand-row" title="Expand/Collapse Record" ><i class="fa fa-plus-square"></i></a>';
                }
                else if ($this.hasClass('actions')) {

                    return _self.datatable.cell(this).data();
                }
                else if ($this.hasClass('ddl')) {
                    return $.trim($this.find('select').val());

                } else {
                    return $.trim($this.find('input').val());
                }
            });

            var id = $row.attr("id");
            var myJSON = $row.getMyJSON();


            if (id && id > 0) {
                //Edit Record
                var strMessage = "";
                AppPrivileges.GetFormPrivileges("Provider", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {

                        PhysicalExamTemplateDetail.UpdateProviderLicense(myJSON, id).done(function (response) {
                            if (response.status != false) {

                                utility.DisplayMessages(response.Message, 1);
                                PhysicalExamTemplateDetail.rowDraw($row, _self, values);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                });
            }
            else {
                //Add Record

                AppPrivileges.GetFormPrivileges("Provider", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        PhysicalExamTemplateDetail.SaveProviderLicense(myJSON, id).done(function (response) {
                            if (response.status != false) {

                                $row.attr("id", response.ProviderLicenseInfoId);
                                $row.attr("onclick", "utility.SelectGridRow($(this))");
                                utility.DisplayMessages(response.Message, 1);
                                PhysicalExamTemplateDetail.rowDraw($row, _self, values);
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
        }
    },

    rowAdd: function () {

        AppPrivileges.GetFormPrivileges("Provider", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                EditableGrid.rowAdd();
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    rowRemove: function ($row, obj) {

        var id = $row.attr("id");
        AppPrivileges.GetFormPrivileges("Provider", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = id;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        PhysicalExamTemplateDetail.DeleteProviderLicense(selectedValue).done(function (response) {
                            if (response.status != false) {

                                if ($row.hasClass('adding')) {
                                }
                                var _self = obj;
                                _self.datatable.row($row.get(0)).remove().draw();

                                utility.DisplayMessages(response.Message, 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                    '1'
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });


    },

    rowCancel: function ($row, obj) {


        var _self = obj,
            $actions,
            i,
            data;

        if ($row.hasClass('adding')) {
            _self.datatable.row($row.get(0)).remove().draw();

        } else {

            data = _self.datatable.row($row.get(0)).data();
            _self.datatable.row($row.get(0)).data(data);

            $actions = $row.find('td.actions');
            if ($actions.get(0)) {
                _self.rowSetActionsDefault($row);
            }

            _self.datatable.draw();
        }
    },

    rowDraw: function ($row, _self, values) {

        _self.datatable.row($row.get(0)).data(values);
        $actions = $row.find('td.actions');
        if ($actions.get(0)) {
            _self.rowSetActionsDefault($row);
        }
        _self.datatable.draw();
    },
    validateLength: function (ev) {
        if ($(ev).val().length != 10) {
            $(ev).val('');
        }

    }
    //-------------------Editable Grid Methods Ends---
}