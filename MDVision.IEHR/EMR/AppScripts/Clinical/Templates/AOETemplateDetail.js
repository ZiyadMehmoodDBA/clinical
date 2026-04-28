AOETemplateDetail = {
    params: [],
    PanelID: "pnlAOETemplateDetail",
    bIsFirstLoad: true,
    EditableGrid: null,
    FamilyMembers: [],
    ExamDetails: {},
    SelectedSystem: '',
    array: [],
    myArr: [],
    parentCtrlGlobel: null,
    mainSelected: null,
    SectionNormalInfo: [],
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
    selectedObservations: [],
    specialtyIdMKMK: "",
    ProviderIdMKMK: "",
    CPTCode : "",
    CPTDescription: "",
    Load: function (params) {
        AOETemplateDetail.params = params;
        Clinical_LabOrder.LoadLabs('ddlLabId', "pnlAOETemplateDetail").done(function (response) {
        });

        
        var isSelectedEntity = false
        AOETemplateDetail.selectedPhyExamTempData = [];
        var self = $('#' + AOETemplateDetail.PanelID + ' #tblAOETemplateDetail');
        
        self.loadDropDowns(true).done(function () {
            if (AOETemplateDetail.params["mode"] == "Edit") {
                var dfd = new $.Deferred();
                AOETemplateDetail.loadDropDowns(dfd);
                dfd.done(function (n) {
                    AOETemplateDetail.LoadAOE();
                });
            }
            else {
                AOETemplateDetail.loadDropDowns();
            }
            AOETemplateDetail.buildSystemsAutoComplete();

            AOETemplateDetail.validateAOETemplate();
            $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body', html: true });
          //  AOETemplateDetail.toggleControls();
        });

    }
,
    LoadAOE: function () {
        $("#observationContent").text('');
        $("#SystemPreview").removeClass('hidden');
        AOETemplateDetail.LoadAOETempSysObservations(AOETemplateDetail.params.AOETemplateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.AOETemplateCount > 0) {
                    var res = JSON.parse(response.AOETemplate_JSON);
                    var templateData = JSON.parse(utility.decodeHtml(res));
                    if (templateData.length > 0 && templateData[0] != null) {

                        $('#' + AOETemplateDetail.PanelID + " #frmAOETemplateDetail #txtAOETemplateDetailTemplateName").val(templateData[0].TemplateName);
                        $('#' + AOETemplateDetail.PanelID + " #frmAOETemplateDetail #observationContent").text(templateData[0].TemplatePreview);

                        $('#' + AOETemplateDetail.PanelID + ' #ddlLabId').val(templateData[0]['LabId']);
                        AOETemplateDetail.updateCategory();

                        $('#' + AOETemplateDetail.PanelID + ' #txtLabCPTCode').val(templateData[0]['CPTCode'] + ' ' + templateData[0]['CPTCodeDescription']);
                        AOETemplateDetail.CPTCode = templateData[0]['CPTCode'];
                        AOETemplateDetail.CPTDescription = templateData[0]['CPTCodeDescription'];

                        if (templateData[0].IsActive == "True") {
                            $('#' + AOETemplateDetail.PanelID + ' #chkActive').prop('checked', true);
                        }
                        else {
                            $('#' + AOETemplateDetail.PanelID + ' #chkActive').prop('checked', false);
                        }
                        $('#' + AOETemplateDetail.PanelID + ' #ddlAOETemplateDetailProvider').multiselect('clearSelection', false);
                        $('#' + AOETemplateDetail.PanelID + ' #ddlAOETemplateDetailProvider').multiselect('updateButtonText');
                        $('#' + AOETemplateDetail.PanelID + " #ddlAOETemplateDetailProvider").val(templateData[0]['ProviderIds'].split(','));
                        $('#' + AOETemplateDetail.PanelID + ' #ddlAOETemplateDetailProvider').multiselect("refresh");
                        //Start 16-10-2017 Humaira Yousaf Bug# EMR-4972
                        if (templateData[0]['ProviderIds'] != "") {
                            AOETemplateDetail.providerCheckedIds = templateData[0]['ProviderIds'].split(',');
                        }
                        //End 16-10-2017 Humaira Yousaf Bug# EMR-4972

                        $('#' + AOETemplateDetail.PanelID + ' #ddlAOETemplateDetailTemplateSpecialty').multiselect('clearSelection', false);
                        $('#' + AOETemplateDetail.PanelID + ' #ddlAOETemplateDetailTemplateSpecialty').multiselect('updateButtonText');
                        $('#' + AOETemplateDetail.PanelID + " #ddlAOETemplateDetailTemplateSpecialty").val(templateData[0]['SpecialtyIds'].split(','));
                        $('#' + AOETemplateDetail.PanelID + ' #ddlAOETemplateDetailTemplateSpecialty').multiselect("refresh");
                        //Start 16-10-2017 Humaira Yousaf Bug# EMR-4972
                        if (templateData[0]['SpecialtyIds'] != "") {
                            AOETemplateDetail.specialityCheckedIds = templateData[0]['SpecialtyIds'].split(',');
                        }
                        //End 16-10-2017 Humaira Yousaf Bug# EMR-4972
                    }
                }

                if (response.AOETemplateSystemsCount > 0) {
                    $("#observationContent div").remove();
                    $("#ulAOETemplateDetailSystems li").remove();
                    var resTemplateSystems = JSON.parse(response.AOETemplateSystems_JSON);
                    var SystemData = JSON.parse(resTemplateSystems);

                    for (var i = 0; i < SystemData.length; i++) {
                        if (SystemData[i].PESystemId != "") {
                            var li = AOETemplateDetail.addSystem(SystemData[i].PESystemId, SystemData[i].SystemName);
                            $("#ulAOETemplateDetailSystems").append(li);

                            if (SystemData[i].IsSelectedSystem == "True") {
                                $("#ulAOETemplateDetailSystems #chk" + SystemData[i].PESystemId).prop("checked", true);
                            }

                            if (response.AOESysObservationsCount > 0) {
                                var resTemplateObservations = JSON.parse(response.AOESysObservations_JSON);
                                var ObservationData = JSON.parse(resTemplateObservations);

                                for (var j = 0; j < ObservationData.length; j++) {
                                    if (SystemData[i].PESystemId == ObservationData[j].PESystemId) {
                                        var li = AOETemplateDetail.addObservations(ObservationData[j].PEObservationId, ObservationData[j].ObservationName, ObservationData[j].PESystemId);
                                        $("#ulAOETemplateDetailSystemSection").append(li);

                                        var objSelectedObservations = {
                                        };
                                        if (ObservationData[j].IsSelected == "True") {
                                            $("#ulAOETemplateDetailSystemSection #chk" + ObservationData[j].PEObservationId).prop("checked", true);
                                            if (SystemData[i].IsSelectedSystem == "True") {
                                                objSelectedObservations = {
                                                    PESystemId: ObservationData[j].PESystemId, IsChecked: true, ObservationId: ObservationData[j].PEObservationId, ObservationName: ObservationData[j].ObservationName, IsSystemChecked: true, IsSystemDeleted: 0, IsObservationDeleted: 0
                                                };
                                            }
                                            else {
                                                objSelectedObservations = {
                                                    PESystemId: ObservationData[j].PESystemId, IsChecked: true, ObservationId: ObservationData[j].PEObservationId, ObservationName: ObservationData[j].ObservationName, IsSystemChecked: false, IsSystemDeleted: 0, IsObservationDeleted: 0
                                                };
                                            }

                                            //AOETemplateDetail.handleDelimiter(ObservationData[j].PESystemId, ObservationData[j].PEObservationId, ObservationData[j].ObservationName, true);
                                        }
                                        else {
                                            $("#ulAOETemplateDetailSystemSection #chk" + ObservationData[j].PEObservationId).prop("checked", false);
                                            if (SystemData[i].IsSelectedSystem == "True") {
                                                objSelectedObservations = {
                                                    PESystemId: ObservationData[j].PESystemId, IsChecked: false, ObservationId: ObservationData[j].PEObservationId, ObservationName: ObservationData[j].ObservationName, IsSystemChecked: true, IsSystemDeleted: 0, IsObservationDeleted: 0
                                                };
                                            }
                                            else {
                                                objSelectedObservations = {
                                                    PESystemId: ObservationData[j].PESystemId, IsChecked: false, ObservationId: ObservationData[j].PEObservationId, ObservationName: ObservationData[j].ObservationName, IsSystemChecked: false, IsSystemDeleted: 0, IsObservationDeleted: 0
                                                };
                                            }

                                            //AOETemplateDetail.handleDelimiter(ObservationData[j].PESystemId, ObservationData[j].PEObservationId, ObservationData[j].ObservationName, false);
                                        }
                                        AOETemplateDetail.selectedObservations.push(objSelectedObservations);
                                    }
                                }
                            }
                        }
                    }
                    $('#ulAOETemplateDetailSystems #' + SystemData[0].PESystemId).click();
                }
            }
        });
    },
    EnableDisableTestControls: function() {
        if ($('#' + AOETemplateDetail.PanelID + ' #ddlLabId option:selected').text().trim() != "-Select-")
        {
            $('#' + AOETemplateDetail.PanelID + ' #txtLabCPTCode').removeClass('disableAll');
        }
        else {
            $('#' + AOETemplateDetail.PanelID + ' #txtLabCPTCode').addClass('disableAll');
        }
    },
    checkProvidersBySpecialityIds: function (option, checked, select) {
        //specialty context
        var specialtyContext = '#' + AOETemplateDetail.PanelID + ' #divAOETemplateDetailSpecialty';
        var isAllSpecialtySelected = $(specialtyContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var specialtyItems = $(specialtyContext).find('.dropdown-menu').find('li').length;
        var checkedSpecialtyItems = $(specialtyContext).find('.dropdown-menu').find('li.active').length;

        if (checkedSpecialtyItems <= 0) {
            AOETemplateDetail.specialityCheckedIds = [];
            AOETemplateDetail.providerCheckedIds = [];
            AOETemplateDetail.ProviderIds = '';
            AOETemplateDetail.SpecialtyIds = '';
        }
        else {
            if (!isAllSpecialtySelected && !(specialtyItems == checkedSpecialtyItems)) {


                var spacialityId = $(option).attr("value");
                if (checked && spacialityId != "") {
                    AOETemplateDetail.specialityCheckedIds = AOETemplateDetail.removeFromArray(AOETemplateDetail.specialityCheckedIds, spacialityId);
                    AOETemplateDetail.specialityCheckedIds.push(spacialityId);
                }
                else {

                    AOETemplateDetail.specialityCheckedIds = AOETemplateDetail.removeFromArray(AOETemplateDetail.specialityCheckedIds, spacialityId);

                }


            }
            else {

                AOETemplateDetail.specialityCheckedIds = [];
                $('#' + AOETemplateDetail.PanelID + ' #ddlAOETemplateDetailTemplateSpecialty option').each(function () {
                    var spacialityId = $(this).attr("value");
                    AOETemplateDetail.specialityCheckedIds.push(spacialityId);
                });

            }
        }
    },
    IntializeMultiSelectDropDownSpecialties: function () {
        $('#' + AOETemplateDetail.PanelID + ' #ddlAOETemplateDetailTemplateSpecialty').multiselect('destroy');
        $('#' + AOETemplateDetail.PanelID + ' #ddlAOETemplateDetailTemplateSpecialty').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: false,
            onChange: function (option, checked, select) {

                AOETemplateDetail.checkProvidersBySpecialityIds(option, checked, select);

            },
            onDropdownHide: function (event) {
                $.when(
                    AOETemplateDetail.filterProvidersBySpecialtyIds()
                ).then(function () {
                    if (AOETemplateDetail.ProviderIds != '') {
                        var Providers = AOETemplateDetail.ProviderIds.split(",");

                        if (Providers != '' && typeof Providers != 'undefined') {

                            $.each(Providers, function (index, item) {
                                AOETemplateDetail.providerCheckedIds = AOETemplateDetail.removeFromArray(AOETemplateDetail.providerCheckedIds, item);
                                AOETemplateDetail.providerCheckedIds.push(item);
                            });
                        }
                    }
                    $('#' + AOETemplateDetail.PanelID + ' #ddlAOETemplateDetailProvider').val(AOETemplateDetail.providerCheckedIds);
                    AOETemplateDetail.IntializeMultiSelectDropDownProviders();
                });
            },

            onDropdownShow: function (event) {
                //make items selected and initialize dropdownlist
                if (AOETemplateDetail.SpecialtyIds != '') {
                    var spacialties = AOETemplateDetail.SpecialtyIds.split(",");

                    if (spacialties != '' && typeof spacialties != 'undefined') {

                        $.each(spacialties, function (index, item) {
                            AOETemplateDetail.specialityCheckedIds = AOETemplateDetail.removeFromArray(AOETemplateDetail.specialityCheckedIds, item);
                            AOETemplateDetail.specialityCheckedIds.push(item);
                        });
                    }
                }
                AOETemplateDetail.setSpacialtiesByselectedProviderIds();
                $('#' + AOETemplateDetail.PanelID + ' #ddlAOETemplateDetailTemplateSpecialty').multiselect('clearSelection', false);
                $('#' + AOETemplateDetail.PanelID + ' #ddlAOETemplateDetailTemplateSpecialty').multiselect('updateButtonText');
                $('#' + AOETemplateDetail.PanelID + ' #ddlAOETemplateDetailTemplateSpecialty').multiselect('select', AOETemplateDetail.specialityCheckedIds);
                $('#' + AOETemplateDetail.PanelID + ' #ddlAOETemplateDetailTemplateSpecialty').multiselect('refresh');
            },
        });
    },

    IntializeMultiSelectDropDownProviders: function () {
        $('#' + AOETemplateDetail.PanelID + ' #ddlAOETemplateDetailProvider').multiselect('destroy');
        $('#' + AOETemplateDetail.PanelID + ' #ddlAOETemplateDetailProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: false,
            onChange: function (option, checked, select) {
                AOETemplateDetail.checkSpecialtiesByProviderId(option, checked, select);
            },
            onDropdownHide: function (event) {
                // AOETemplateDetail.specialitiesByProviderIds();
                //Refresh multiselect
                //  $('#' + AOETemplateDetail.PanelID + ' #ddlAOETemplateDetailTemplateSpecialty').multiselect('refresh');
            },


        });
    },
    loadEntitySpecialty: function (entityID, dfd) {

        // Loads Spacialties Based on entityId
        if (entityID != null && entityID > 0) {

            providerDetail.FillSpecialty(entityID).done(function (response) {
                if (response.status != false) {

                    var spacialties = JSON.parse(response.SpecialtyLoad_JSON);
                    $('#' + AOETemplateDetail.PanelID + ' #ddlAOETemplateDetailTemplateSpecialty').empty();

                    $.each(spacialties, function (i, item) {
                        $('#' + AOETemplateDetail.PanelID + ' #ddlAOETemplateDetailTemplateSpecialty').append(
                            $('<option/>', {
                                value: item.SpecialtyId,
                                html: item.ShortName
                            })
                        );
                    });

                    //Assign server side spacialties to the specialityCheckedIds array
                    if (AOETemplateDetail.SpecialtyIds != '') {

                        var Specialties = AOETemplateDetail.SpecialtyIds.split(",");
                        AOETemplateDetail.specialityCheckedIds = Specialties;
                        $('#' + AOETemplateDetail.PanelID + ' #ddlAOETemplateDetailTemplateSpecialty').val(Specialties);
                    }
                }

            }).then(function () {
                AOETemplateDetail.IntializeMultiSelectDropDownSpecialties();
                //enable dropdownlist
                AOETemplateDetail.enableDisableDropDownLists('ddlAOETemplateDetailTemplateSpecialty', false);
                AOETemplateDetail.loadEntityProvider(globalAppdata["SeletedEntityId"], dfd);


            });
        }
        else {
            //Disable dropdownlist
            AOETemplateDetail.enableDisableDropDownLists('ddlAOETemplateDetailTemplateSpecialty', true);
        }
    },

    setSpacialtiesByselectedProviderIds: function () {

        $.each(AOETemplateDetail.providerCheckedIds, function (index, item) {

            $('#' + AOETemplateDetail.PanelID + ' #ddlAOETemplateDetailProvider option').each(function () {
                if ($(this).val() != '') {
                    if ($(this).val() == item) {

                        AOETemplateDetail.specialityCheckedIds = AOETemplateDetail.removeFromArray(AOETemplateDetail.specialityCheckedIds, $(this).attr('refname'));
                        AOETemplateDetail.specialityCheckedIds.push($(this).attr('refname'));
                    }
                }
            });
        });
    },
    removeFromArray: function (array, removeItem) {

        var resultantArray = jQuery.grep(array, function (item) {
            return item != removeItem;
        });
        return resultantArray;
    },
    enableDisableDropDownLists: function (ddlIds, isHide) {
        ddlCommaSeparatedIds = ddlIds.split(',');
        var parrentPanelId = "#" + AOETemplateDetail.PanelID;
        $.each(ddlCommaSeparatedIds, function (index, Item) {
            if (isHide) {
                $(parrentPanelId + " #" + Item).multiselect('disable');
            }
            else {
                $(parrentPanelId + " #" + Item).multiselect('enable');
            }
        });
    },

    loadEntityProvider: function (entityId, dfd) {

        var data = "entityID=" + entityId;
        if (entityId != null && entityId > 0) {

            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $('#' + AOETemplateDetail.PanelID + ' #ddlAOETemplateDetailProvider');
                var $providerHiddenDdl = $('#' + AOETemplateDetail.PanelID + ' #ddlHiddenAOETemplateDetailTemplateProvider');

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
                if (AOETemplateDetail.ProviderIds != '') {
                    var Providers = AOETemplateDetail.ProviderIds.split(",");
                    AOETemplateDetail.providerCheckedIds = Providers;
                    $('#' + AOETemplateDetail.PanelID + ' #ddlAOETemplateDetailProvider').val(Providers);
                }

            }).then(function () {
                // A hack to trigger the onDropDownHide event of Spacialty multiselect      
                $('#' + AOETemplateDetail.PanelID + ' #divAOETemplateDetailSpecialty').find('.dropdown-toggle').dropdown('toggle').dropdown('toggle');
                //Intialized in onhidden spacialty ddl.  
                AOETemplateDetail.IntializeMultiSelectDropDownProviders();
                if (dfd)
                    dfd.resolve();
            });
            //enable multiselect
            //AOETemplateDetail.enableDisableDropDowLists('ddlAOETemplateDetailProvider', false);
        }
        else {
            //disable multiselect
            AOETemplateDetail.enableDisableDropDownLists('ddlAOETemplateDetailProvider', true);
        }
    },
    validateAOETemplate: function () {
        $('#' + AOETemplateDetail.PanelID + ' #frmAOETemplateDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   AOETemplateName: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   AOETemplateEntity: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   LabId: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   AOETemplateDetailTemplateName: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   LabCPTCode: {
                       group: '.col-sm-8',
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
            // Validate Provider
            if ($('#' + AOETemplateDetail.PanelID + ' #ddlAOETemplateDetailProvider option:Selected').length <= 0) {
                utility.DisplayMessages("Please select a Provider.", 2);
            }
            else {
                AOETemplateDetail.AOETemplateSave();
            }
        });
    },
    saveAOETemplate: function (PhysicalExamTemplateData, TemplateName) {
        var self = null, IsActive = null;
        self = $('#' + AOETemplateDetail.PanelID + ' #frmAOETemplateDetail');
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objData = JSON.parse(myJSON);
        if (AOETemplateDetail.params["mode"] == "Edit") {
            objData["AOETemplateId"] = (AOETemplateDetail.params["AOETemplateId"]);
        }
        else {
            objData["AOETemplateId"] = '-1';
        }

        if (TemplateName != null) {

            var mainTemplateName = $('#' + AOETemplateDetail.PanelID + ' #frmAOETemplateDetail #txtPhysicalExamTemplateName').val();

            if (objData["AOETemplateId"] == '-1') {
                objData["PhysicalExamTemplateName"] = TemplateName;
                IsActive = "1";
            }
            else {
                objData["PhysicalExamTemplateName"] = mainTemplateName != "" ? mainTemplateName : TemplateName;
            }

            objData["SaveAsTemplateName"] = TemplateName;
        }

        //objData["SpecialtyIds"] = objData["PhysicalExamTemplateSpecialty"];
        //objData["ProviderIds"] = objData["PhysicalExamTemplateProvider"];
        //objData.SpecialtyIds = objData.PhysicalExamTemplateSpecialty = AOETemplateDetail.specialityCheckedIds.join();
        //objData.ProviderIds = objData.PhysicalExamTemplateProvider = AOETemplateDetail.providerCheckedIds.join();

        //------------------------------------------------------

        var SpecialtyIds = self.find('#ddlAOETemplateDetailTemplateSpecialty option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objData["SpecialtyIds"] = SpecialtyIds;

        var ProviderIds = self.find('#ddlAOETemplateDetailProvider option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objData["ProviderIds"] = ProviderIds;


        //------------------------------------------------------

        objData["commandType"] = "Save_AOETemplate";
        objData["SystemObservationData"] = AOETemplateDetail.selectedObservations;
        objData["TemplatePreview"] = $("#observationContent").text();
        objData["PhysicalExamTemplateEntity"] = globalAppdata["SeletedEntityId"];


        objData["IsActive"] = objData["IsActive"] == true ? "1" : "0";
        objData["CPTCode"] = AOETemplateDetail.CPTCode;
        objData["CPTCodeDescription"] = AOETemplateDetail.CPTDescription;
        objData["Name"] = objData["AOETemplateDetailTemplateName"];
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "AOETemplate", "AOETemplate");
    },
    
    AOETemplateSave: function () {
        var isValid = false;
        var self = null;
        self = $('#' + AOETemplateDetail.PanelID + ' #frmAOETemplateDetail');

        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objData = JSON.parse(myJSON);
        //objData.SpecialtyIds = objData.PhysicalExamTemplateSpecialty = AOETemplateDetail.specialityCheckedIds.join();
        //objData.ProviderIds = objData.PhysicalExamTemplateProvider = AOETemplateDetail.providerCheckedIds.join();

        var isStillValid = false;

        //if (AOETemplateDetail.validateSelectedTemplateData())
        //{
        myJSON = JSON.stringify(objData);
        AOETemplateDetail.saveAOETemplate(myJSON).done(function (response) {
            if (response != null && response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.message, 1);
                    if (response.phyExamTemplateId != "") {
                        AOETemplateDetail.params.PhysicalExamTemplateId = response.phyExamTemplateId;
                        for (var count in AOETemplateDetail.selectedPhyExamTempData) {
                            AOETemplateDetail.selectedPhyExamTempData[count];
                        }
                    }
                    //AOETemplateDetail.loadHospitalizationHx();

                    AOETemplateDetail.params.mode = "Edit";
                    if (AOETemplateDetail.params.ParentCtrl == "clinicalTabProgressNote" && UnloadHospitalizationhx == true) {
                        AOETemplateDetail.getHospitalizationHxInfo(HospitalizationHxType, UnloadHospitalizationhx);
                    }
                    $('#' + AOETemplateDetail.PanelID + " #hfHospitalizationHxId").val(response.HospitalizationHxId);
                    $('#' + AOETemplateDetail.PanelID + ' #frmClinicalHospitalizationHx').data('serialize', $('#' + AOETemplateDetail.PanelID + ' #frmClinicalHospitalizationHx').serialize());
                    //

                    // Empty global variables
                    AOETemplateDetail.specialityCheckedIds = [];
                    AOETemplateDetail.providerCheckedIds = [];
                    AOETemplateDetail.providerSelectedIds = [];
                    AOETemplateDetail.selectedPhyExamTempData = [];
                    AOETemplateDetail.SpecialtyIds = "";
                    AOETemplateDetail.ProviderIds = "";

                    AOETemplateDetail.Unload(objData["IsActive"]);
                    if (PhysicalExamTemplate != null) {
                        PhysicalExamTemplate.loadPhysicalExamTemplateMK();
                        AOETemplateDetail.selectedObservations = [];
                    }
                    //End Farooq Ahmad 16-03-2016 Unload the Physical Exam on Save
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
        });
        //}
        //else {
        //    utility.DisplayMessages("Please select characteristic/Sub-characteristic", 3);
        //}
    },
    removeObservation: function (PESystemId, PEObservationId) {
        $("#ulAOETemplateDetailSystemSection #" + PEObservationId).remove();
        $("#observationContent #divSystem" + PESystemId + PEObservationId).remove();
        AOETemplateDetail.removeLastDelimiter(PESystemId);

        if (AOETemplateDetail.selectedObservations) {
            for (var i = 0 ; i < AOETemplateDetail.selectedObservations.length; i++) {
                if (AOETemplateDetail.selectedObservations[i].PESystemId == PESystemId && AOETemplateDetail.selectedObservations[i].ObservationId == PEObservationId) {
                    AOETemplateDetail.selectedObservations[i].IsObservationDeleted = 1;
                    AOETemplateDetail.selectedObservations[i].IsChecked = false;
                    AOETemplateDetail.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + PESystemId).text();
                }
            }
        }
    },
    filterProvidersBySpecialtyIds: function () {

        var providerHiddenContext = '#' + AOETemplateDetail.PanelID + ' #ddlHiddenAOETemplateDetailTemplateProvider';

        var providerContext = '#' + AOETemplateDetail.PanelID + ' #ddlAOETemplateDetailProvider';
        $(providerContext).empty();

        if (AOETemplateDetail.specialityCheckedIds.length > 0) {

            $.each(AOETemplateDetail.specialityCheckedIds, function (index, specialtyId) {

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
    AOETemplateSystemsLoad: function (templateId) {
        var objData = new Object();
        objData["TemplateId"] = templateId;
        objData["commandType"] = "Load_PhyscialExam_Systems";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },
    AOESystemObservationsLoad: function (PESystemId) {
        var objData = new Object();
        objData["SystemId"] = PESystemId;
        objData["commandType"] = "Load_PhyscialExam_System_Observations";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },

    buildSystemsAutoComplete: function (templateId) {
        AOETemplateDetail.AOETemplateSystemsLoad(templateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var res = JSON.parse(response.PESystems_JSON);
                var resSystems = JSON.parse(res);
                var data = [];
                $.each(resSystems, function (i, item) {
                    data.push({ id: item.PESystemId, value: item.Name });
                });

                $("#Systems").kendoAutoComplete({
                    dataSource: data,
                    filter: "contains",
                    dataTextField: "value",
                    placeholder: "Select System...",
                    select: function (e) {
                        var dataItem = this.dataItem(e.item.index());
                        //alert(kendo.stringify(dataItem));
                        var li = AOETemplateDetail.addSystem(dataItem.id, dataItem.value);
                        if (li != undefined)
                            $("#ulAOETemplateDetailSystems").append(li);
                        $("#Systems").val('');
                        AOETemplateDetail.loadObservations(dataItem.id);
                        e.preventDefault();
                    },
                    footerTemplate: 'Total <strong>#: instance.dataSource.total() #</strong> items found'
                });

                $("#Systems").parent().addClass('size100per');
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    loadObservations: function (PESystemId) {
        $("#ulAOETemplateDetailSystemSection").show();
        var isExist = false;

        if (AOETemplateDetail.selectedObservations) {
            for (var i = 0 ; i < AOETemplateDetail.selectedObservations.length; i++) {
                if (AOETemplateDetail.selectedObservations[i].PESystemId == PESystemId) {
                    isExist = true;
                    break;
                }
            }
        }

        if (!isExist) {
            AOETemplateDetail.PhysicalExamSystemObservationsLoad(PESystemId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.PEObservationCount > 0) {
                        var res = JSON.parse(response.PEObservation_JSON);
                        var resSystems = JSON.parse(res);
                        $("#SystemSections").removeClass('hidden');
                        $("#observationContent div").hide();
                        //$('#observationContent div[id^=divSystem]').hide();
                        $("#ulAOETemplateDetailSystemSection li").remove();

                        $("#ulAOETemplateDetailSystems li").removeClass('active');
                        $("#ulAOETemplateDetailSystems li[id=" + PESystemId + "]").addClass('active');

                        var selectAll = '<li id="chkboxSelectAllObservations" parentid="' + PESystemId + '" value="Select All" refvalue="Select All"><div class="pl-lg checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="chkboxSelectAllObservations" name="Select All" class="pull-left" '
                            + 'onclick="AOETemplateDetail.selectAllChars(this);"><label id="lblSelectAll" class="" data-toggle="tooltip" title="" data-original-title="Select All">Select All</label><div class="clearfix"></div><div class="clearfix"></div></div></li>';
                        $("#ulAOETemplateDetailSystemSection").append(selectAll);

                        $.each(resSystems, function (i, item) {
                            var li = AOETemplateDetail.addObservations(item.PEObservationId, item.Name, PESystemId);
                            $("#ulAOETemplateDetailSystemSection").append(li);


                            var objSelectedObservations = {
                                PESystemId: PESystemId, IsChecked: false, ObservationId: item.PEObservationId, ObservationName: item.Name, IsSystemChecked: false, SystemDescription: "", IsSystemDeleted: 0, IsObservationDeleted: 0
                            };
                            AOETemplateDetail.selectedObservations.push(objSelectedObservations);

                        });
                    }
                    else {
                        // $('#observationContent div[id^=divSystem]').hide();
                        $("#ulAOETemplateDetailSystemSection li").remove();

                        $("#ulAOETemplateDetailSystems li").removeClass('active');
                        $("#ulAOETemplateDetailSystems li[id=" + PESystemId + "]").addClass('active');
                    }
                }
            });
        }
        else {
            $("#SystemSections").removeClass('hidden');
            $("#ulAOETemplateDetailSystemSection li").remove();

            var selectAll = '<li id="chkboxSelectAllObservations" parentid="' + PESystemId + '" value="Select All" refvalue="Select All"><div class="pl-lg checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="chkboxSelectAllObservations" name="Select All" class="pull-left" '
                        + 'onclick="AOETemplateDetail.selectAllChars(this);"><label id="lblSelectAll" class="" data-toggle="tooltip" title="" data-original-title="Select All">Select All</label><div class="clearfix"></div><div class="clearfix"></div></div></li>';
            $("#ulAOETemplateDetailSystemSection").append(selectAll);

            if (AOETemplateDetail.selectedObservations) {
                //$('#observationContent div[id^=divSystem]').hide();
                $("#observationContent").text('');
                for (var i = 0 ; i < AOETemplateDetail.selectedObservations.length; i++) {
                    if (AOETemplateDetail.selectedObservations[i].PESystemId == PESystemId) {
                        var li = AOETemplateDetail.addObservations(AOETemplateDetail.selectedObservations[i].ObservationId, AOETemplateDetail.selectedObservations[i].ObservationName, PESystemId);

                        if (AOETemplateDetail.selectedObservations[i].ObservationId != "")
                            $("#ulAOETemplateDetailSystemSection").append(li);

                        $("#ulAOETemplateDetailSystems li").removeClass('active');
                        $("#ulAOETemplateDetailSystems li[id=" + PESystemId + "]").addClass('active');

                        if (AOETemplateDetail.selectedObservations[i].IsChecked) {
                            if (AOETemplateDetail.selectedObservations[i].IsObservationDeleted == 0) {
                                $("#ulAOETemplateDetailSystemSection #chk" + AOETemplateDetail.selectedObservations[i].ObservationId).prop("checked", true);
                                $('#observationContent #divSystem' + PESystemId + AOETemplateDetail.selectedObservations[i].ObservationId).show();
                                AOETemplateDetail.selectedObservations[i].IsObservationDeleted = 0;
                                AOETemplateDetail.selectedObservations[i].IsSystemDeleted = 0;
                                AOETemplateDetail.handleDelimiter(PESystemId, AOETemplateDetail.selectedObservations[i].ObservationId, AOETemplateDetail.selectedObservations[i].ObservationName, true);
                            }
                            else {
                                $("#ulAOETemplateDetailSystemSection #chk" + AOETemplateDetail.selectedObservations[i].ObservationId).prop("checked", false);
                                AOETemplateDetail.selectedObservations[i].IsObservationDeleted = 0;
                                AOETemplateDetail.selectedObservations[i].IsSystemDeleted = 0;
                                AOETemplateDetail.selectedObservations[i].IsChecked = false;
                            }
                        }
                        else {
                            $("#ulAOETemplateDetailSystemSection #chk" + AOETemplateDetail.selectedObservations[i].ObservationId).prop("checked", false);
                            AOETemplateDetail.selectedObservations[i].IsObservationDeleted = 0;
                            AOETemplateDetail.selectedObservations[i].IsSystemDeleted = 0;
                            AOETemplateDetail.selectedObservations[i].IsChecked = false;
                        }
                    }
                }
            }
        }
    },
    PhysicalExamSystemObservationsLoad: function (PESystemId) {
        var objData = new Object();
        objData["SystemId"] = PESystemId;
        objData["commandType"] = "Load_PhyscialExam_System_Observations";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },
    addObservations: function (PEObservationId, ObservatioName, PESystemId) {
        var a = AOETemplateDetail.selectedObservations;

        var itemtoRemove = "observation";

        var li = '<li id="' + PEObservationId + '" parentid="' + PESystemId + '" onclick="AOETemplateDetail.PreviewObservations(' + PEObservationId + ',\'' + ObservatioName + '\', this, ' + PESystemId + ');" value="' + PEObservationId + '" refvalue="" subcharacteristicexist=" " class=""><div class="checkbox-custom checkboxTiny checkbox-success">' +
                 '<input type="checkbox" id="chk' + PEObservationId + '" name="' + PEObservationId + '" class="pull-left  char" ' +
                 'onclick="AOETemplateDetail.PreviewObservations(' + PEObservationId + ',\'' + ObservatioName + '\', this, ' + PESystemId + ');"><label id="lblName' + PEObservationId + '" class="" data-toggle="tooltip" title="" data-original-title="' + ObservatioName + '">' + ObservatioName + '</label><div id="divNameDetail' + PEObservationId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" spellcheck="true" id="txtName' + PEObservationId + '" onkeypress="" name="Name' + PEObservationId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' +
                 PEObservationId + '" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div><a href="#"><span class="removeIconListHover" onclick="AOETemplateDetail.removeObservation(' + PESystemId + ', ' + PEObservationId + ')"><i class="fa fa-close"></i></span></a></li>';
        return li;
    },

    addSystem: function (PESystemId, SystemName) {
        var itemtoRemove = "system";

        for (var i = 0 ; i < AOETemplateDetail.selectedObservations.length; i++) {
            if (AOETemplateDetail.selectedObservations[i].PESystemId == PESystemId && AOETemplateDetail.selectedObservations[i].IsSystemDeleted != 1) {
                utility.DisplayMessages(SystemName + " already associated with the template", 3);
                return;
            }
        }

        for (var i = 0; i < $("#ulAOETemplateDetailSystems li").length; i++) {
            if ($($("#ulAOETemplateDetailSystems li label")[i]).text() == SystemName) {
                utility.DisplayMessages(SystemName + " already associated with the template", 3);
                return;
            }

        }

        var li = '<li id="' + PESystemId + '" parentid="null" onclick="AOETemplateDetail.loadObservations(' + PESystemId + ')" value="' + PESystemId + '" refvalue="" subcharacteristicexist=" " class=""><div class="checkbox-custom checkboxTiny checkbox-success">' +
                 '<input type="checkbox" id="chk' + PESystemId + '" name="' + PESystemId + '" class="pull-left  char" onclick="AOETemplateDetail.ManageObservations(' + PESystemId + ', this);"><label id="lblName' + PESystemId + '" class="" data-toggle="tooltip" title="" data-original-title="' + SystemName + '">' + SystemName + '</label><div id="divNameDetail' + PESystemId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" spellcheck="true" id="txtName' + PESystemId + '" onkeypress="" name="Name' + PESystemId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' +
                 PESystemId + '" onclick="" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div><a href="#"><span class="removeIconListHover" onclick="AOETemplateDetail.removeSystem(' + PESystemId + ', event)";><i class="fa fa-close"></i></span></a></li>';
        // AOETemplateDetail.removeSystem(' + PESystemId + ')
        return li;
    },
    removeSystem: function (PESystemId, event) {
        $("#ulAOETemplateDetailSystems #" + PESystemId).remove();
        $("#ulAOETemplateDetailSystemSection").hide();
        if (AOETemplateDetail.selectedObservations) {
            for (var i = 0 ; i < AOETemplateDetail.selectedObservations.length; i++) {
                if (AOETemplateDetail.selectedObservations[i].PESystemId == PESystemId) {
                    AOETemplateDetail.selectedObservations[i].IsSystemDeleted = 1;
                    AOETemplateDetail.selectedObservations[i].IsObservationDeleted = 1;
                    AOETemplateDetail.selectedObservations[i].IsChecked = false;
                    AOETemplateDetail.selectedObservations[i].IsSystemChecked = false;
                    AOETemplateDetail.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + PESystemId).text();
                }
            }
        }
        $("#observationContent div").remove();
        event.stopPropagation();
    },
    ManageObservations: function (PESystemId, obj) {
        $("#ulAOETemplateDetailSystems li").removeClass('active');
        $("#ulAOETemplateDetailSystems li[id=" + PESystemId + "]").addClass('active');

        if (!$(obj).is(':checked')) {
            $(obj).parent().parent().removeClass('active');

            for (var i = 0; i < $("#ulAOETemplateDetailSystemSection li").length; i++) {
                $("#ulAOETemplateDetailSystemSection #chk" + $("#ulAOETemplateDetailSystemSection li")[i].id).prop('checked', false);
            }
            if (AOETemplateDetail.selectedObservations) {
                for (var i = 0 ; i < AOETemplateDetail.selectedObservations.length; i++) {
                    if (AOETemplateDetail.selectedObservations[i].PESystemId == PESystemId) {
                        AOETemplateDetail.selectedObservations[i].IsSystemChecked = false;
                        AOETemplateDetail.selectedObservations[i].IsChecked = false;
                        AOETemplateDetail.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + PESystemId).text();
                        //break;
                    }
                }
            }
        }
        else {
            if (AOETemplateDetail.selectedObservations) {
                for (var i = 0 ; i < AOETemplateDetail.selectedObservations.length; i++) {
                    if (AOETemplateDetail.selectedObservations[i].PESystemId == PESystemId) {
                        AOETemplateDetail.selectedObservations[i].IsSystemChecked = true;
                        AOETemplateDetail.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + PESystemId).text();
                        break;
                    }
                }
            }
        }
    },
    LoadAOETempSysObservations: function (templateId) {
        var objData = new Object();
        objData["AOETemplateId"] = templateId;
        objData["commandType"] = "Load_AOE_TemplatesFill";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "AOETemplate", "AOETemplate");
    },

    checkSpecialtiesByProviderId: function (option, checked, select) {

        //provider context
        var providerContext = '#' + AOETemplateDetail.PanelID + ' #divAOETemplateDetailTemplateProvider';
        var isAllProviderSelected = $(providerContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var providerItems = $(providerContext).find('.dropdown-menu').find('li').length;
        var checkedProviderItems = $(providerContext).find('.dropdown-menu').find('li.active').length;

        var allProviders = $(providerContext).find('.dropdown-menu').find('li:not(".filter,.multiselect-all")').length;
        var selectedProviders = $(providerContext).find('.dropdown-menu').find('li.active:not(".filter,.multiselect-all")').length;

        if (checkedProviderItems <= 0) {
            AOETemplateDetail.providerCheckedIds = [];
            AOETemplateDetail.ProviderIds = '';
        }
            //push all provider checked items
        else if (isAllProviderSelected && allProviders == selectedProviders) {
            AOETemplateDetail.providerCheckedIds = [];
            $('#' + AOETemplateDetail.PanelID + ' #ddlAOETemplateDetailProvider option').each(function () {
                var providerValue = $(this).val();
                AOETemplateDetail.providerCheckedIds.push(providerValue);
            });
        }
        else {
            // provider value
            var providerValue = $(option).val();

            // add to provider array if checked
            if (checked) {
                AOETemplateDetail.providerCheckedIds = AOETemplateDetail.removeFromArray(AOETemplateDetail.providerCheckedIds, providerValue);
                AOETemplateDetail.providerCheckedIds.push(providerValue);
            }
                //delete from provider array if not checked
            else {
                AOETemplateDetail.providerCheckedIds = AOETemplateDetail.removeFromArray(AOETemplateDetail.providerCheckedIds, $(option).val());
            }

        }
    },
    handleDelimiter: function (PESystemId, PEObservationId, ObservationName, IsChecked) {
        //$("#observationContent").text('');
        if (IsChecked) {
            var delimator = $("#delimator option:selected").text() + " ";
            if ($("#observationContent #divSystem" + PESystemId + PEObservationId).length > 0) {
                $('#observationContent #divSystem' + PESystemId + PEObservationId).remove();
                var $newDiv = $("<div></div>");
                $newDiv.attr("id", "divSystem" + PESystemId + PEObservationId);
                $newDiv.attr("style", "display: inline;");

                $("#observationContent").append($newDiv);
                $('#observationContent #divSystem' + PESystemId + PEObservationId).show();
                var txtToAppend = ObservationName;

                if ($('#observationContent div').length > 1)
                    txtToAppend = delimator + ObservationName;

                $("#observationContent #divSystem" + PESystemId + PEObservationId).append(txtToAppend);
            }
            else {
                var $newDiv = $("<div></div>");
                $newDiv.attr("id", "divSystem" + PESystemId + PEObservationId);
                $newDiv.attr("style", "display: inline;");

                $("#observationContent").append($newDiv);
                $('#observationContent #divSystem' + PESystemId + PEObservationId).show();
                var txtToAppend = ObservationName;

                if ($('#observationContent div').length > 1)
                    txtToAppend = delimator + ObservationName;

                $("#observationContent #divSystem" + PESystemId + PEObservationId).append(txtToAppend);
            }
        }
    },
    selectAllChars: function (obj) {
        $('#observationContent div[id^=divSystem]').remove();
        if (obj) var sysId = $($($(obj).parent().parent())[0]).attr('parentid');
        if ($(obj).prop('checked') == true) {
            $("#SystemPreview").removeClass('hidden');
            $('#' + AOETemplateDetail.PanelID + ' #ulAOETemplateDetailSystemSection li .char').each(function () {
                if ($(this).attr("id") != $(obj).attr("id") && $(this).prop("checked") == false) {
                    $(this).parent().find("[type='checkbox'][id*='chk']").prop("checked", true);

                    var id_ = $(this).parent().find("[type='checkbox'][id*='chk']").attr('id').substring(3)
                    var observationName = $("#divAOETemplateDetailSystemSection #lblName" + id_).text();
                    var delimator = $("#delimator option:selected").text() + " ";

                    if ($("#observationContent #divSystem" + sysId + id_).length > 0) {
                        $('#observationContent #divSystem' + sysId + id_).remove();
                        var $newDiv = $("<div></div>");
                        $newDiv.attr("id", "divSystem" + sysId + id_);
                        $newDiv.attr("style", "display: inline;");

                        $("#observationContent").append($newDiv);
                        $('#observationContent #divSystem' + sysId + id_).show();
                        var txttoAppend = observationName;
                        if ($('#observationContent div').length > 1)
                            txttoAppend = delimator + observationName;

                        $("#observationContent #divSystem" + sysId + id_).append(txttoAppend);
                    }
                    else {
                        var $newDiv = $("<div></div>");
                        $newDiv.attr("id", "divSystem" + sysId + id_);
                        $newDiv.attr("style", "display: inline;");

                        $("#observationContent").append($newDiv);
                        $('#observationContent #divSystem' + sysId + id_).show();

                        var txttoAppend = observationName;
                        if ($('#observationContent div').length > 1)
                            txttoAppend = delimator + observationName;

                        $("#observationContent #divSystem" + sysId + id_).append(txttoAppend);
                    }

                    $("#ulAOETemplateDetailSystems #chk" + sysId).prop("checked", true);
                }
            });

            if (AOETemplateDetail.selectedObservations) {
                for (var i = 0 ; i < AOETemplateDetail.selectedObservations.length; i++) {
                    if (AOETemplateDetail.selectedObservations[i].PESystemId == sysId) {
                        AOETemplateDetail.selectedObservations[i].IsChecked = true;
                        AOETemplateDetail.selectedObservations[i].IsSystemChecked = true;
                        AOETemplateDetail.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + sysId).text();
                    }
                }
            }
        }
        else if ($(obj).prop('checked') == false) {
            $('#' + AOETemplateDetail.PanelID + ' #ulAOETemplateDetailSystemSection li .char').removeClass("green");
            $('#' + AOETemplateDetail.PanelID + ' #ulAOETemplateDetailSystemSection li .char').each(function () {
                if ($(this).attr("id") != $(obj).attr("id")) {
                    $(this).parent().find("[type='checkbox'][id*='chk']").prop("checked", false);
                    var id_ = $(this).parent().find("[type='checkbox'][id*='chk']").attr('id').substring(3);
                    var system_id = $($($(obj).parent().parent())[0]).attr('parentid');
                    $("#observationContent #divSystem" + system_id + id_).remove();
                }
            });

            if (AOETemplateDetail.selectedObservations) {
                for (var i = 0 ; i < AOETemplateDetail.selectedObservations.length; i++) {
                    if (AOETemplateDetail.selectedObservations[i].PESystemId == sysId) {
                        AOETemplateDetail.selectedObservations[i].IsChecked = false;
                        AOETemplateDetail.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + sysId).text();
                        //$('#observationContent div[id^=divSystem' + sysId + ']').hide();
                    }
                }
            }
        }
        AOETemplateDetail.removeLastDelimiter(sysId);
    },

    applyStyle: function (style) {
        if (style == 'bold') {
            $("#observationContent").css('font-weight', 'bold');
        }
        else if (style == 'italic') {
            $("#observationContent").css('font-style', 'italic');
        }
        else if (style == 'underline') {
            $("#observationContent").css('text-decoration', 'underline');
        }
        else if (style == 'reset') {
            $("#observationContent").attr("style", " ");
        }
        else if (style == 'clear') {
            $("#observationContent").text('');
        }
    },
    addNewItem: function (itemType) {
        if (itemType != null && itemType != "") {

            var addSubCharIcon = "";

            charSelectAll = null;
            subCharSelectAll = null;

            var isSubCharacteristic = false;
            var ulControl = "";
            var currentLiClick = "";
            var currentCtrlId = "";
            var currentParentId = "";
            var currentId = "";
            currentId = AOETemplateDetail.NewInsertedId--;
            var subcharacteristicExist = "";

            var saveMethodPE = "";

            if (itemType.toLowerCase() == "system") {
                currentLiClick = "";
                currentCtrlId = "ulAOETemplateDetailSystems";
                ulControl = $('#' + AOETemplateDetail.PanelID + " #frmAOETemplateDetail #" + currentCtrlId);
                saveMethodPE = "AOETemplateDetail.AddSystemPE(this, '" + currentId + "');";
            }
            else if (itemType.toLowerCase() == "subsystem") {
                currentLiClick = "";
                currentCtrlId = "ulAOETemplateDetailSystemSection";
                ulControl = $('#' + AOETemplateDetail.PanelID + " #frmAOETemplateDetail #" + currentCtrlId);
                saveMethodPE = "AOETemplateDetail.AddObservation(this, '" + currentId + "');";
            }

            if (ulControl != null && ulControl != "") {
                var currentLiClass = "";

                var arrNewlyAddedLi = ulControl.find("li[id*='-']");

                if (itemType.toLowerCase() != "system") {
                    currentParentId = ulControl.find("li:last").attr("parentid");
                    if (currentParentId == null)
                        currentParentId = ulControl.attr("ParentId");
                }

                var onClick = "";
                onClick = "";//"AOETemplateDetail.showHideChildControls(this,'" + currentCtrlId + "','" + currentId + "');";
                var deleteFunction = "AOETemplateDetail.deleteItem(this,'" + currentCtrlId + "','" + currentId + "');";

                var deleteIcon = '<a class="btn btn-xs pull-left" href="#" onclick="' + deleteFunction + '" title="Delete Record"><i class="fa fa-close red"></i></a>';

                liInnerText = '<div class="checkbox-custom checkboxTiny checkbox-success">' + deleteIcon + addSubCharIcon
                    + '<input checked type="checkbox" id="chk' + currentId + '+ve" name="' + currentId + '" class="pull-left  char" onclick="">'
                    //+ '<input checked type="checkbox" id="chk' + currentId + '+ve" name="' + currentId + '" class="pull-left  char" onclick="AOETemplateDetail.selectParentControls(this);AOETemplateDetail.toggleCheckBoxes(this);">'
                    + '<label id="lblName' + currentId + '" class=" hidden" data-toggle="tooltip"  title="" data-original-title="' + currentId + '">' + currentId + '</label>'
                    + '<div id="divNameDetail' + currentId + '" class="rightInnerAddon pb-xs">'
                    + '<textarea rows="1" spellcheck="true" id="txtName' + currentId + '" onkeypress="AOETemplateDetail.saveDetailComments(event,this)"  name="Name' + currentId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll"></textarea>'
                    + '<div class="clearfix"></div><div class="rightInnerAddonBtn"><span id="btnSaveDetail' + currentId + '" onclick="' + saveMethodPE + '" class="btn btn-link btn-xs">'
                    + '<i class="fa fa-save"></i></span></div></div><div class="clearfix"></div><div class="clearfix"></div></div>';

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
    savePEObservation_DBCall: function (data) {

        var objData = new Object();
        if (data)
            objData = data;

        objData["commandType"] = "insert_physicalexam_observation_";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamECW", "PhysicalExamObservation");
    },
    AddSystemPE: function (obj, controlId) {

        var objData = new Object();
        objData["IsGlobal"] = false;
        objData["Name"] = $("#txtName" + controlId).val();
        objData["IsActive"] = true;

        if (objData["Name"] == null || objData["Name"] == "" || objData["Name"] == undefined) {
            utility.DisplayMessages("Please enter some value", 3);
            return;
        }

        AOETemplateDetail.savePESystem_DBCall(objData).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                
                utility.DisplayMessages(response.Message, 1);

                var li = AOETemplateDetail.addSystem(response.PESystemId, objData["Name"]);
                $("#ulAOETemplateDetailSystems").append(li);

                //var objSelectedObservations = {
                //    PESystemId: response.PESystemId, IsChecked: false, ObservationId: '', ObservationName: '', IsSystemChecked: false, IsSystemDeleted: 0, IsObservationDeleted: 0
                //};
                //AOETemplateDetail.selectedObservations.push(objSelectedObservations);
                AOETemplateDetail.loadObservations(response.PESystemId);
                $("#" + controlId).remove();
            }
            else {
                
                utility.DisplayMessages(response.Message, 1);
            }
        });
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
            onClickFunction = onClickFunction.replace('this', "$('#" + AOETemplateDetail.PanelID + " #" + ULID + " #" + ID + "')");
            eval(onClickFunction);
        }
    },
    savePESystem_DBCall: function (data) {

        var objData = new Object();
        if (data)
            objData = data;

        objData["commandType"] = "insert_physicalexam_system";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamECW", "PhysicalExamSystem");
    },
    AddObservation: function (obj, controlId) {
        var PESystemId = $("#ulAOETemplateDetailSystems li.active")[0].id;
        var objData = new Object();
        objData["PESystemId"] = PESystemId;
        objData["Name"] = $("#txtName" + controlId).val();
        objData["IsActive"] = true;

        if (objData["Name"] == null || objData["Name"] == "" || objData["Name"] == undefined) {
            utility.DisplayMessages("Please enter some value", 3);
            return;
        }

        AOETemplateDetail.savePEObservation_DBCall(objData).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status) {
                    var res = JSON.parse(response.PEObservation_JSON);
                    utility.DisplayMessages(response.Message, 1);
                    AOETemplateDetail.params.PEObservationId = res[0].PEObservationId;
                    var li = AOETemplateDetail.addObservations(res[0].PEObservationId, objData["Name"], PESystemId);
                    $("#ulAOETemplateDetailSystemSection").append(li);
                    var sysChecked = $("#chk" + $("#ulAOETemplateDetailSystems li.active").attr('id')).is(':checked');
                    var objSelectedObservations = {
                        PESystemId: PESystemId, IsChecked: false, ObservationId: res[0].PEObservationId, ObservationName: objData["Name"], IsSystemChecked: sysChecked, IsSystemDeleted: 0, IsObservationDeleted: 0
                    };
                    AOETemplateDetail.selectedObservations.push(objSelectedObservations);
                    $("#" + controlId).remove();
                }
                else {
                    utility.DisplayMessages(response.Message,2);
                }
            }
        });
    },
    removeLastDelimiter: function (PESystemId) {

        var delii = $("#delimator option:selected").text();
        var str = "";
        if (delii == ",") str = $($('#observationContent div[id^=divSystem' + PESystemId + ']')[0]).text().replace(/,/g, "");
        if (delii == ".") str = $($('#observationContent div[id^=divSystem' + PESystemId + ']')[0]).text().replace(/./g, "");
        if (delii == ":") str = $($('#observationContent div[id^=divSystem' + PESystemId + ']')[0]).text().replace(/:/g, "");
        if (delii == ";") str = $($('#observationContent div[id^=divSystem' + PESystemId + ']')[0]).text().replace(/;/g, "");
        if (delii == "-") str = $($('#observationContent div[id^=divSystem' + PESystemId + ']')[0]).text().replace(/-/g, "");

        var id = $($('#observationContent div')[0]).attr('id');
        $("#observationContent #" + id).text(str);
    },
    PreviewObservations: function (observationId, ObservationName, obj, PESystemId) {

        $("#SystemPreview").removeClass('hidden');

        if ($(obj).prop('checked') == true) {
            $(this).find("[type='checkbox'][id*='chk']").prop("checked", true);
        }
        else if ($(obj).prop('checked') == false) {
            $('#' + AOETemplateDetail.PanelID + ' #ulAOETemplateDetailSystemSection li .char').removeClass("green");
            $(this).find("[type='checkbox'][id*='chk']").prop("checked", false);
        }

        var isChk = $(obj).prop('checked') == true ? true : false;

        var objSelectedObservations =
        {
            PESystemId: PESystemId,
            IsChecked: isChk,
            ObservationId: observationId,
            ObservationName: ObservationName,
            IsModified: '1',
            IsSystemChecked: false,
            SystemDescription: ""
        };


        if ($(obj).prop('checked') == true) {
            var deli = $("#delimator option:selected").text() + " ";

            //$('#observationContent div[id^=divSystem]').remove();
            if ($("#observationContent #divSystem" + PESystemId + observationId).length > 0) {
                $('#observationContent #divSystem' + PESystemId + observationId).remove();
                var $newDiv = $("<div></div>");
                $newDiv.attr("id", "divSystem" + PESystemId + observationId);
                $newDiv.attr("style", "display: inline;");

                $("#observationContent").append($newDiv);
                $('#observationContent #divSystem' + PESystemId + observationId).show();
                var txttoAppend = ObservationName;
                if ($('#observationContent div').length > 1)
                    txttoAppend = deli + ObservationName;

                $("#observationContent #divSystem" + PESystemId + observationId).append(txttoAppend);
            }
            else {
                var $newDiv = $("<div></div>");
                $newDiv.attr("id", "divSystem" + PESystemId + observationId);
                $newDiv.attr("style", "display: inline;");

                $("#observationContent").append($newDiv);
                $('#observationContent #divSystem' + PESystemId + observationId).show();
                var txttoAppend = ObservationName;

                if ($('#observationContent div').length > 1)
                    txttoAppend = deli + ObservationName;

                $("#observationContent #divSystem" + PESystemId + observationId).append(txttoAppend);
            }

            if (AOETemplateDetail.selectedObservations) {
                for (var i = 0 ; i < AOETemplateDetail.selectedObservations.length; i++) {
                    if (AOETemplateDetail.selectedObservations[i].PESystemId == PESystemId && AOETemplateDetail.selectedObservations[i].ObservationId == observationId) {
                        AOETemplateDetail.selectedObservations[i].IsChecked = true;
                        AOETemplateDetail.selectedObservations[i].IsSystemChecked = true;
                        AOETemplateDetail.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + PESystemId).text();
                    }
                    if (AOETemplateDetail.selectedObservations[i].PESystemId == PESystemId) {
                        AOETemplateDetail.selectedObservations[i].IsSystemChecked = true;
                        AOETemplateDetail.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + PESystemId).text();
                    }
                }
            }

            $("#ulAOETemplateDetailSystems #chk" + PESystemId).prop("checked", true);
            AOETemplateDetail.loadObservations(PESystemId);
        }
        else if ($(obj).prop('checked') == false) {
            if (AOETemplateDetail.selectedObservations) {
                for (var i = 0 ; i < AOETemplateDetail.selectedObservations.length; i++) {
                    if (AOETemplateDetail.selectedObservations[i].PESystemId == PESystemId && AOETemplateDetail.selectedObservations[i].ObservationId == observationId) {
                        AOETemplateDetail.selectedObservations[i].IsChecked = false;
                        AOETemplateDetail.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + PESystemId).text();
                    }
                }
            }
            var aa = $('#observationContent #divSystem' + PESystemId + observationId).text();
            $('#observationContent #divSystem' + PESystemId + observationId).remove();
            AOETemplateDetail.removeLastDelimiter(PESystemId);
        }
        AOETemplateDetail.removeLastDelimiter(PESystemId);
    },
    bindAutoComplete: function (element) {
        //var hiddenCrtl = $(element);
        // utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "ClinicalLabOrderDetail", null, true);

        
        var labId = $('#pnlAOETemplateDetail #ddlLabId').val();
        var codesystem = $('#' + AOETemplateDetail.PanelID + ' #ddlLabId option:selected').attr('codesystem');
        EMRUtility.BindLOINCCodes(element, "AOETemplateDetail", labId, null, codesystem);

    },

    pushLOINCAsCpt: function (JsonObj) {

        var observation = JsonObj["Observation"];
        AOETemplateDetail.CPTCode = JsonObj['LOINICCODE'];
        AOETemplateDetail.CPTDescription = JsonObj['LOINICDescription'];
        var SampleStorage = JsonObj['SampleStorage'];

    },

    loadDropDowns: function (dfd) {
        var self = $('#' + AOETemplateDetail.PanelID + ' #tblAOETemplateDetail');
        if (globalAppdata.AppUserName.toLowerCase() != DefaultUser.toLowerCase()) {
            self.find("#ddlAOETemplateEntity").val(globalAppdata["SeletedEntityId"]);
            if (self.find("div#divEntity").hasClass("hidden") == false)
                self.find("div#divEntity").addClass("hidden");
            isSelectedEntity = true;
        }
        else {
            self.find("div#divEntity").removeClass("hidden");
        }
        AOETemplateDetail.loadEntitySpecialty(globalAppdata["SeletedEntityId"], dfd);
        if (AOETemplateDetail.params.Title != null)
            $("#" + AOETemplateDetail.params["PanelID"] + " #headingTitle").text(AOETemplateDetail.params.Title);
    },
    updateCategory: function() {
        var Lab = $('#' + AOETemplateDetail.PanelID + ' #ddlLabId option:selected');
        $('#' + AOETemplateDetail.PanelID + ' #txtCategory').val($(Lab).attr('category'));

        AOETemplateDetail.EnableDisableTestControls();
    },
    
    Unload: function (IsActiveGrid)
    {
        // Empty global variables
        AOETemplateDetail.specialityCheckedIds = [];
        AOETemplateDetail.providerCheckedIds = [];
        AOETemplateDetail.providerSelectedIds = [];
        AOETemplateDetail.selectedPhyExamTempData = [];
        AOETemplateDetail.SpecialtyIds = "";
        AOETemplateDetail.ProviderIds = "";
        AOETemplateDetail.selectedObservations = [];
        AOETemplateDetail.specialtyIdMKMK = [];
        AOETemplateDetail.ProviderIdMKMK = [];

        if (IsActiveGrid != null && IsActiveGrid != undefined)
        {
          //  AOETemplate.LoadAOETemplates(IsActiveGrid == true ? 1 : 0).done(function (response) {
              //  AOETemplate.AOETemplatesGridLoad(response);
                var switcher = $('#' + AOETemplate.PanelID + ' #switchActive');

                $(switcher).attr('IsActive', IsActiveGrid == true ? '0' : '1');
                if (IsActiveGrid == "1" || IsActiveGrid == true) {
                    $('#' + AOETemplate.PanelID + ' .ios-switch').attr('class', 'ios-switch on');
                    AOETemplate.changeSwitch(switcher[0]);
                }
                else {
                    $('#' + AOETemplate.PanelID + ' .ios-switch').attr('class', 'ios-switch off');
                    AOETemplate.changeSwitch(switcher[0]);
                }

        //    });
        }
        if (AOETemplateDetail.params.ParentCtrl) {
            UnloadActionPan(AOETemplateDetail.params["ParentCtrl"], "AOETemplateDetail");
        }
        else {
            UnloadActionPan();
        }

    }
}