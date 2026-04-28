ProcedureTemplateDetail = {
    params: [],
    PanelID: "pnlProcedureTemplateDetail",
    bIsFirstLoad: true,
    EditableGrid: null,
    FamilyMembers: [],
    ExamDetails: {},
    SelectedSystem: '',
    array: [],
    myArr: [],
    procedures: [],
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
    CPTCode: "",
    CPTDescription: "",
    Load: function (params) {
        ProcedureTemplateDetail.params = params;
 

        var isSelectedEntity = false
        ProcedureTemplateDetail.selectedPhyExamTempData = [];
        var self = $('#' + ProcedureTemplateDetail.PanelID + ' #tblProcedureTemplateDetail');

        self.loadDropDowns(true).done(function () {
            $('#pnlProcedureTemplateDetail #ddlAssociationType').multiselect();
            if (ProcedureTemplateDetail.params["mode"] == "Edit") {
                var dfd = new $.Deferred();
                ProcedureTemplateDetail.loadDropDowns(dfd);
                dfd.done(function (n) {
                    ProcedureTemplateDetail.LoadProcedure();
                });
            }
            else {
                ProcedureTemplateDetail.loadDropDowns();
            }
            ProcedureTemplateDetail.buildSystemsAutoComplete();

            ProcedureTemplateDetail.validateProcedureTemplate();
            $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body', html: true });
            //  ProcedureTemplateDetail.toggleControls();
        });

    }
,
    LoadProcedure: function () {
        $("#observationContent").text('');
        $("#SystemPreview").removeClass('hidden');
        // LoadProcedures 
        ProcedureTemplateDetail.LoadProcedureTemplateTests(ProcedureTemplateDetail.params.ProcedureTemplateId).done(function (resp) {
            resp = JSON.parse(resp);
            $.each(resp.Procedures, function (i, item) {
                item["LOINICCODE"] = item["CPTCode"];
                item["LOINICDescription"] = item["CPTCodeDescription"];
                ProcedureTemplateDetail.pushLOINCAsCpt(item);
            });
        });

        ProcedureTemplateDetail.LoadProcedureTempSysObservations(ProcedureTemplateDetail.params.ProcedureTemplateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.ProcedureTemplateCount > 0) {
                    var res = JSON.parse(response.ProcedureTemplate_JSON);
                    var templateData = JSON.parse(utility.decodeHtml(res));
                    $('#' + ProcedureTemplateDetail.PanelID + " #frmProcedureTemplateDetail #txtProcedureTemplateDetailTemplateName").val(templateData[0].TemplateName);
                    $('#' + ProcedureTemplateDetail.PanelID + " #frmProcedureTemplateDetail #observationContent").text(templateData[0].TemplatePreview);

                    $('#' + ProcedureTemplateDetail.PanelID + ' #ddlLabId').val(templateData[0]['LabId']);
                    ProcedureTemplateDetail.updateCategory();

                    $('#' + ProcedureTemplateDetail.PanelID + ' #txtLabCPTCode').val(templateData[0]['CPTCode'] + ' ' + templateData[0]['CPTCodeDescription']);
                    ProcedureTemplateDetail.CPTCode = templateData[0]['CPTCode'];
                    ProcedureTemplateDetail.CPTDescription = templateData[0]['CPTCodeDescription'];

                    if (templateData[0].IsActive == "True") {
                        $('#' + ProcedureTemplateDetail.PanelID + ' #chkActive').prop('checked', true);
                    }
                    else {
                        $('#' + ProcedureTemplateDetail.PanelID + ' #chkActive').prop('checked', false);
                    }
                    $('#' + ProcedureTemplateDetail.PanelID + ' #ddlProcedureTemplateDetailProvider').multiselect('clearSelection', false);
                    $('#' + ProcedureTemplateDetail.PanelID + ' #ddlProcedureTemplateDetailProvider').multiselect('updateButtonText');
                    $('#' + ProcedureTemplateDetail.PanelID + " #ddlProcedureTemplateDetailProvider").val(templateData[0]['ProviderIds'].split(','));
                    $('#' + ProcedureTemplateDetail.PanelID + ' #ddlProcedureTemplateDetailProvider').multiselect("refresh");
                    ProcedureTemplateDetail.providerCheckedIds = templateData[0]['ProviderIds'].split(',');

                    $('#' + ProcedureTemplateDetail.PanelID + ' #ddlAssociationType').multiselect('clearSelection', false);
                    $('#' + ProcedureTemplateDetail.PanelID + ' #ddlAssociationType').multiselect('updateButtonText');
                    $('#' + ProcedureTemplateDetail.PanelID + " #ddlAssociationType").val(templateData[0]['AssociatedWithIds'].split(','));
                    $('#' + ProcedureTemplateDetail.PanelID + ' #ddlAssociationType').multiselect("refresh");
                }

                if (response.ProcedureTemplateSystemsCount > 0) {
                    $("#observationContent div").remove();
                    $("#ulProcedureTemplateDetailSystems li").remove();
                    var resTemplateSystems = JSON.parse(response.ProcedureTemplateSystems_JSON);
                    var SystemData = JSON.parse(resTemplateSystems);

                    for (var i = 0; i < SystemData.length; i++) {
                        if (SystemData[i].PESystemId != "") {
                            var li = ProcedureTemplateDetail.addSystem(SystemData[i].PESystemId, SystemData[i].SystemName);
                            $("#ulProcedureTemplateDetailSystems").append(li);

                            if (SystemData[i].IsSelectedSystem == "True") {
                                $("#ulProcedureTemplateDetailSystems #chk" + SystemData[i].PESystemId).prop("checked", true);
                            }

                            if (response.ProcedureSysObservationsCount > 0) {
                                var resTemplateObservations = JSON.parse(response.ProcedureSysObservations_JSON);
                                var ObservationData = JSON.parse(resTemplateObservations);

                                for (var j = 0; j < ObservationData.length; j++) {
                                    if (SystemData[i].PESystemId == ObservationData[j].PESystemId) {
                                        var li = ProcedureTemplateDetail.addObservations(ObservationData[j].PEObservationId, ObservationData[j].ObservationName, ObservationData[j].PESystemId);
                                        $("#ulProcedureTemplateDetailSystemSection").append(li);

                                        var objSelectedObservations = {
                                        };
                                        if (ObservationData[j].IsSelected == "True") {
                                            $("#ulProcedureTemplateDetailSystemSection #chk" + ObservationData[j].PEObservationId).prop("checked", true);
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

                                            //ProcedureTemplateDetail.handleDelimiter(ObservationData[j].PESystemId, ObservationData[j].PEObservationId, ObservationData[j].ObservationName, true);
                                        }
                                        else {
                                            $("#ulProcedureTemplateDetailSystemSection #chk" + ObservationData[j].PEObservationId).prop("checked", false);
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

                                            //ProcedureTemplateDetail.handleDelimiter(ObservationData[j].PESystemId, ObservationData[j].PEObservationId, ObservationData[j].ObservationName, false);
                                        }
                                        ProcedureTemplateDetail.selectedObservations.push(objSelectedObservations);
                                    }
                                }
                            }
                        }
                    }
                    $('#ulProcedureTemplateDetailSystems #' + SystemData[0].PESystemId).click();
                }
            }

        });
    },
    LoadProcedureTemplateTests: function (templateId) {
        var objData = new Object();
        objData["ProcedureTemplateId"] = templateId;
        objData["IsActive"] = "true";
        objData["commandType"] = "Load_ProcedureTemplateTests";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ProcedureTemplate", "ProcedureTemplate");
    },
    EnableDisableTestControls: function () {
        if ($('#' + ProcedureTemplateDetail.PanelID + ' #ddlLabId option:selected').text().trim() != "-Select-") {
            $('#' + ProcedureTemplateDetail.PanelID + ' #txtLabCPTCode').removeClass('disableAll');
        }
        else {
            $('#' + ProcedureTemplateDetail.PanelID + ' #txtLabCPTCode').addClass('disableAll');
        }
    },
    checkProvidersBySpecialityIds: function (option, checked, select) {
        //specialty context
        var specialtyContext = '#' + ProcedureTemplateDetail.PanelID + ' #divProcedureTemplateDetailSpecialty';
        var isAllSpecialtySelected = $(specialtyContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var specialtyItems = $(specialtyContext).find('.dropdown-menu').find('li').length;
        var checkedSpecialtyItems = $(specialtyContext).find('.dropdown-menu').find('li.active').length;

        if (checkedSpecialtyItems <= 0) {
            ProcedureTemplateDetail.specialityCheckedIds = [];
            ProcedureTemplateDetail.providerCheckedIds = [];
            ProcedureTemplateDetail.ProviderIds = '';
            ProcedureTemplateDetail.SpecialtyIds = '';
        }
        else {
            if (!isAllSpecialtySelected && !(specialtyItems == checkedSpecialtyItems)) {


                var spacialityId = $(option).attr("value");
                if (checked && spacialityId != "") {
                    ProcedureTemplateDetail.specialityCheckedIds = ProcedureTemplateDetail.removeFromArray(ProcedureTemplateDetail.specialityCheckedIds, spacialityId);
                    ProcedureTemplateDetail.specialityCheckedIds.push(spacialityId);
                }
                else {

                    ProcedureTemplateDetail.specialityCheckedIds = ProcedureTemplateDetail.removeFromArray(ProcedureTemplateDetail.specialityCheckedIds, spacialityId);

                }


            }
            else {

                ProcedureTemplateDetail.specialityCheckedIds = [];
                $('#' + ProcedureTemplateDetail.PanelID + ' #ddlProcedureTemplateDetailTemplateSpecialty option').each(function () {
                    var spacialityId = $(this).attr("value");
                    ProcedureTemplateDetail.specialityCheckedIds.push(spacialityId);
                });

            }
        }
    },
    IntializeMultiSelectDropDownSpecialties: function () {
        $('#' + ProcedureTemplateDetail.PanelID + ' #ddlProcedureTemplateDetailTemplateSpecialty').multiselect('destroy');
        $('#' + ProcedureTemplateDetail.PanelID + ' #ddlProcedureTemplateDetailTemplateSpecialty').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: false,
            onChange: function (option, checked, select) {

                ProcedureTemplateDetail.checkProvidersBySpecialityIds(option, checked, select);

            },
            onDropdownHide: function (event) {
                $.when(
                    ProcedureTemplateDetail.filterProvidersBySpecialtyIds()
                ).then(function () {
                    if (ProcedureTemplateDetail.ProviderIds != '') {
                        var Providers = ProcedureTemplateDetail.ProviderIds.split(",");

                        if (Providers != '' && typeof Providers != 'undefined') {

                            $.each(Providers, function (index, item) {
                                ProcedureTemplateDetail.providerCheckedIds = ProcedureTemplateDetail.removeFromArray(ProcedureTemplateDetail.providerCheckedIds, item);
                                ProcedureTemplateDetail.providerCheckedIds.push(item);
                            });
                        }
                    }
                    $('#' + ProcedureTemplateDetail.PanelID + ' #ddlProcedureTemplateDetailProvider').val(ProcedureTemplateDetail.providerCheckedIds);
                    ProcedureTemplateDetail.IntializeMultiSelectDropDownProviders();
                });
            },

            onDropdownShow: function (event) {
                //make items selected and initialize dropdownlist
                if (ProcedureTemplateDetail.SpecialtyIds != '') {
                    var spacialties = ProcedureTemplateDetail.SpecialtyIds.split(",");

                    if (spacialties != '' && typeof spacialties != 'undefined') {

                        $.each(spacialties, function (index, item) {
                            ProcedureTemplateDetail.specialityCheckedIds = ProcedureTemplateDetail.removeFromArray(ProcedureTemplateDetail.specialityCheckedIds, item);
                            ProcedureTemplateDetail.specialityCheckedIds.push(item);
                        });
                    }
                }
                ProcedureTemplateDetail.setSpacialtiesByselectedProviderIds();
                $('#' + ProcedureTemplateDetail.PanelID + ' #ddlProcedureTemplateDetailTemplateSpecialty').multiselect('select', ProcedureTemplateDetail.specialityCheckedIds);
            },
        });
    },

    IntializeMultiSelectDropDownProviders: function () {
        $('#' + ProcedureTemplateDetail.PanelID + ' #ddlProcedureTemplateDetailProvider').multiselect('destroy');
        $('#' + ProcedureTemplateDetail.PanelID + ' #ddlProcedureTemplateDetailProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: false,
            onChange: function (option, checked, select) {
                ProcedureTemplateDetail.checkSpecialtiesByProviderId(option, checked, select);
            },
            onDropdownHide: function (event) {
                // ProcedureTemplateDetail.specialitiesByProviderIds();
                //Refresh multiselect
                //  $('#' + ProcedureTemplateDetail.PanelID + ' #ddlProcedureTemplateDetailTemplateSpecialty').multiselect('refresh');
            },


        });
    },
    loadEntitySpecialty: function (entityID, dfd) {

        // Loads Spacialties Based on entityId
        if (entityID != null && entityID > 0) {

            providerDetail.FillSpecialty(entityID).done(function (response) {
                if (response.status != false) {

                    var spacialties = JSON.parse(response.SpecialtyLoad_JSON);
                    $('#' + ProcedureTemplateDetail.PanelID + ' #ddlProcedureTemplateDetailTemplateSpecialty').empty();

                    $.each(spacialties, function (i, item) {
                        $('#' + ProcedureTemplateDetail.PanelID + ' #ddlProcedureTemplateDetailTemplateSpecialty').append(
                            $('<option/>', {
                                value: item.SpecialtyId,
                                html: item.ShortName
                            })
                        );
                    });

                    //Assign server side spacialties to the specialityCheckedIds array
                    if (ProcedureTemplateDetail.SpecialtyIds != '') {

                        var Specialties = ProcedureTemplateDetail.SpecialtyIds.split(",");
                        ProcedureTemplateDetail.specialityCheckedIds = Specialties;
                        $('#' + ProcedureTemplateDetail.PanelID + ' #ddlProcedureTemplateDetailTemplateSpecialty').val(Specialties);
                    }
                }

            }).then(function () {
                ProcedureTemplateDetail.IntializeMultiSelectDropDownSpecialties();
                //enable dropdownlist
                ProcedureTemplateDetail.enableDisableDropDownLists('ddlProcedureTemplateDetailTemplateSpecialty', false);
                ProcedureTemplateDetail.loadEntityProvider(globalAppdata["SeletedEntityId"], dfd);


            });
        }
        else {
            //Disable dropdownlist
            ProcedureTemplateDetail.enableDisableDropDownLists('ddlProcedureTemplateDetailTemplateSpecialty', true);
        }
    },

    setSpacialtiesByselectedProviderIds: function () {

        $.each(ProcedureTemplateDetail.providerCheckedIds, function (index, item) {

            $('#' + ProcedureTemplateDetail.PanelID + ' #ddlProcedureTemplateDetailProvider option').each(function () {
                if ($(this).val() != '') {
                    if ($(this).val() == item) {

                        ProcedureTemplateDetail.specialityCheckedIds = ProcedureTemplateDetail.removeFromArray(ProcedureTemplateDetail.specialityCheckedIds, $(this).attr('refname'));
                        ProcedureTemplateDetail.specialityCheckedIds.push($(this).attr('refname'));
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
        var parrentPanelId = "#" + ProcedureTemplateDetail.PanelID;
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
                var $providerDdl = $('#' + ProcedureTemplateDetail.PanelID + ' #ddlProcedureTemplateDetailProvider');
                var $providerHiddenDdl = $('#' + ProcedureTemplateDetail.PanelID + ' #ddlHiddenProcedureTemplateDetailTemplateProvider');

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
                if (ProcedureTemplateDetail.ProviderIds != '') {
                    var Providers = ProcedureTemplateDetail.ProviderIds.split(",");
                    ProcedureTemplateDetail.providerCheckedIds = Providers;
                    $('#' + ProcedureTemplateDetail.PanelID + ' #ddlProcedureTemplateDetailProvider').val(Providers);
                }

            }).then(function () {
                // A hack to trigger the onDropDownHide event of Spacialty multiselect      
                $('#' + ProcedureTemplateDetail.PanelID + ' #divProcedureTemplateDetailSpecialty').find('.dropdown-toggle').dropdown('toggle').dropdown('toggle');
                //Intialized in onhidden spacialty ddl.  
                ProcedureTemplateDetail.IntializeMultiSelectDropDownProviders();
                if (dfd)
                    dfd.resolve();
            });
            //enable multiselect
            //ProcedureTemplateDetail.enableDisableDropDowLists('ddlProcedureTemplateDetailProvider', false);
        }
        else {
            //disable multiselect
            ProcedureTemplateDetail.enableDisableDropDownLists('ddlProcedureTemplateDetailProvider', true);
        }
    },
    validateProcedureTemplate: function () {
        $('#' + ProcedureTemplateDetail.PanelID + ' #frmProcedureTemplateDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {

                   ProcedureTemplateDetailTemplateName: {
                       group: '.col-sm-3',
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
            // Validate Provider
            if ($('#' + ProcedureTemplateDetail.PanelID + ' #ddlProcedureTemplateDetailProvider option:Selected').length <= 0) {
                utility.DisplayMessages("Please select a Provider.", 2);
            }
            else {
                ProcedureTemplateDetail.ProcedureTemplateSave();
            }
        });
    },
    saveProcedureTemplate: function (PhysicalExamTemplateData, TemplateName) {
        var self = null, IsActive = null;
        self = $('#' + ProcedureTemplateDetail.PanelID + ' #frmProcedureTemplateDetail');
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objData = JSON.parse(myJSON);
        if (ProcedureTemplateDetail.params["mode"] == "Edit") {
            objData["ProcedureTemplateId"] = (ProcedureTemplateDetail.params["ProcedureTemplateId"]);
        }
        else {
            objData["ProcedureTemplateId"] = '-1';
        }

        if (TemplateName != null) {

            var mainTemplateName = $('#' + ProcedureTemplateDetail.PanelID + ' #frmProcedureTemplateDetail #txtPhysicalExamTemplateName').val();

            if (objData["ProcedureTemplateId"] == '-1') {
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
        //objData.SpecialtyIds = objData.PhysicalExamTemplateSpecialty = ProcedureTemplateDetail.specialityCheckedIds.join();
        //objData.ProviderIds = objData.PhysicalExamTemplateProvider = ProcedureTemplateDetail.providerCheckedIds.join();

        //------------------------------------------------------

        var SpecialtyIds = self.find('#ddlProcedureTemplateDetailTemplateSpecialty option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objData["SpecialtyIds"] = SpecialtyIds;

        var ProviderIds = self.find('#ddlProcedureTemplateDetailProvider option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objData["ProviderIds"] = ProviderIds;


        //------------------------------------------------------

        objData["commandType"] = "Save_ProcedureTemplate";
        objData["SystemObservationData"] = ProcedureTemplateDetail.selectedObservations;
        objData["TemplatePreview"] = $("#observationContent").text();
        objData["PhysicalExamTemplateEntity"] = globalAppdata["SeletedEntityId"];

        //if ($('#frmProcedureTemplateDetail #ddlNoteViewType').val() != "") {
        //    objData["NoteViewTypeId"] = $('#frmProcedureTemplateDetail #ddlNoteViewType').val();
        //}
        //else
        //{
            
        //}

        var AssociationIds = "";
        for (i = 0; i < ($('#frmProcedureTemplateDetail #ddlAssociationType option:selected').length) ; i++) {
            if ($($('#frmProcedureTemplateDetail #ddlAssociationType option:selected')[i]).val() != "")
                AssociationIds += $($('#frmProcedureTemplateDetail #ddlAssociationType option:selected')[i]).val() + ",";
        }

        objData["AssociatedWithIds"] = AssociationIds;
        objData["IsActive"] = objData["IsActive"] == true ? "1" : "0";
        objData["CPTCode"] = ProcedureTemplateDetail.CPTCode;
        objData["CPTCodeDescription"] = ProcedureTemplateDetail.CPTDescription;
        objData["procedures"] = ProcedureTemplateDetail.procedures;
        objData["Name"] = objData["ProcedureTemplateDetailTemplateName"];

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ProcedureTemplate", "ProcedureTemplate");
    },

    ProcedureTemplateSave: function () {
        var isValid = false;
        var self = null;
        self = $('#' + ProcedureTemplateDetail.PanelID + ' #frmProcedureTemplateDetail');

        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objData = JSON.parse(myJSON);
        //objData.SpecialtyIds = objData.PhysicalExamTemplateSpecialty = ProcedureTemplateDetail.specialityCheckedIds.join();
        //objData.ProviderIds = objData.PhysicalExamTemplateProvider = ProcedureTemplateDetail.providerCheckedIds.join();

        var isStillValid = false;

        //if (ProcedureTemplateDetail.validateSelectedTemplateData())
        //{
        myJSON = JSON.stringify(objData);
        ProcedureTemplateDetail.saveProcedureTemplate(myJSON).done(function (response) {
            if (response != null && response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.message, 1);
                    if (response.phyExamTemplateId != "") {
                        ProcedureTemplateDetail.params.PhysicalExamTemplateId = response.phyExamTemplateId;
                        for (var count in ProcedureTemplateDetail.selectedPhyExamTempData) {
                            ProcedureTemplateDetail.selectedPhyExamTempData[count];
                        }
                    }
                    //ProcedureTemplateDetail.loadHospitalizationHx();

                    ProcedureTemplateDetail.params.mode = "Edit";
                    if (ProcedureTemplateDetail.params.ParentCtrl == "clinicalTabProgressNote" && UnloadHospitalizationhx == true) {
                        ProcedureTemplateDetail.getHospitalizationHxInfo(HospitalizationHxType, UnloadHospitalizationhx);
                    }
                    $('#' + ProcedureTemplateDetail.PanelID + " #hfHospitalizationHxId").val(response.HospitalizationHxId);
                    $('#' + ProcedureTemplateDetail.PanelID + ' #frmClinicalHospitalizationHx').data('serialize', $('#' + ProcedureTemplateDetail.PanelID + ' #frmClinicalHospitalizationHx').serialize());
                    //

                    // Empty global variables
                    ProcedureTemplateDetail.specialityCheckedIds = [];
                    ProcedureTemplateDetail.providerCheckedIds = [];
                    ProcedureTemplateDetail.providerSelectedIds = [];
                    ProcedureTemplateDetail.selectedPhyExamTempData = [];
                    ProcedureTemplateDetail.SpecialtyIds = "";
                    ProcedureTemplateDetail.ProviderIds = "";

                    ProcedureTemplateDetail.Unload(objData["IsActive"]);
                    if (PhysicalExamTemplate != null) {
                        PhysicalExamTemplate.loadPhysicalExamTemplateMK();
                        ProcedureTemplateDetail.selectedObservations = [];
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
        $("#ulProcedureTemplateDetailSystemSection #" + PEObservationId).remove();
        $("#observationContent #divSystem" + PESystemId + PEObservationId).remove();
        ProcedureTemplateDetail.removeLastDelimiter(PESystemId);

        if (ProcedureTemplateDetail.selectedObservations) {
            for (var i = 0 ; i < ProcedureTemplateDetail.selectedObservations.length; i++) {
                if (ProcedureTemplateDetail.selectedObservations[i].PESystemId == PESystemId && ProcedureTemplateDetail.selectedObservations[i].ObservationId == PEObservationId) {
                    ProcedureTemplateDetail.selectedObservations[i].IsObservationDeleted = 1;
                    ProcedureTemplateDetail.selectedObservations[i].IsChecked = false;
                    ProcedureTemplateDetail.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + PESystemId).text();
                }
            }
        }
    },
    filterProvidersBySpecialtyIds: function () {

        var providerHiddenContext = '#' + ProcedureTemplateDetail.PanelID + ' #ddlHiddenProcedureTemplateDetailTemplateProvider';

        var providerContext = '#' + ProcedureTemplateDetail.PanelID + ' #ddlProcedureTemplateDetailProvider';
        $(providerContext).empty();

        if (ProcedureTemplateDetail.specialityCheckedIds.length > 0) {

            $.each(ProcedureTemplateDetail.specialityCheckedIds, function (index, specialtyId) {

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
    ProcedureTemplateSystemsLoad: function (templateId) {
        var objData = new Object();
        objData["TemplateId"] = templateId;
        objData["commandType"] = "Load_PhyscialExam_Systems";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },
    ProcedureSystemObservationsLoad: function (PESystemId) {
        var objData = new Object();
        objData["SystemId"] = PESystemId;
        objData["commandType"] = "Load_PhyscialExam_System_Observations";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },

    buildSystemsAutoComplete: function (templateId) {
        ProcedureTemplateDetail.ProcedureTemplateSystemsLoad(templateId).done(function (response) {
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
                        var li = ProcedureTemplateDetail.addSystem(dataItem.id, dataItem.value);
                        if (li != undefined)
                            $("#ulProcedureTemplateDetailSystems").append(li);
                        $("#Systems").val('');
                        ProcedureTemplateDetail.loadObservations(dataItem.id);
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
        $("#ulProcedureTemplateDetailSystemSection").show();
        var isExist = false;

        if (ProcedureTemplateDetail.selectedObservations) {
            for (var i = 0 ; i < ProcedureTemplateDetail.selectedObservations.length; i++) {
                if (ProcedureTemplateDetail.selectedObservations[i].PESystemId == PESystemId) {
                    isExist = true;
                    break;
                }
            }
        }

        if (!isExist) {
            ProcedureTemplateDetail.PhysicalExamSystemObservationsLoad(PESystemId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.PEObservationCount > 0) {
                        var res = JSON.parse(response.PEObservation_JSON);
                        var resSystems = JSON.parse(res);
                        $("#SystemSections").removeClass('hidden');
                        $("#observationContent div").hide();
                        //$('#observationContent div[id^=divSystem]').hide();
                        $("#ulProcedureTemplateDetailSystemSection li").remove();

                        $("#ulProcedureTemplateDetailSystems li").removeClass('active');
                        $("#ulProcedureTemplateDetailSystems li[id=" + PESystemId + "]").addClass('active');

                        var selectAll = '<li id="chkboxSelectAllObservations" parentid="' + PESystemId + '" value="Select All" refvalue="Select All"><div class="pl-lg checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="chkboxSelectAllObservations" name="Select All" class="pull-left" '
                            + 'onclick="ProcedureTemplateDetail.selectAllChars(this);"><label id="lblSelectAll" class="" data-toggle="tooltip" title="" data-original-title="Select All">Select All</label><div class="clearfix"></div><div class="clearfix"></div></div></li>';
                        $("#ulProcedureTemplateDetailSystemSection").append(selectAll);

                        $.each(resSystems, function (i, item) {
                            var li = ProcedureTemplateDetail.addObservations(item.PEObservationId, item.Name, PESystemId);
                            $("#ulProcedureTemplateDetailSystemSection").append(li);


                            var objSelectedObservations = {
                                PESystemId: PESystemId, IsChecked: false, ObservationId: item.PEObservationId, ObservationName: item.Name, IsSystemChecked: false, SystemDescription: "", IsSystemDeleted: 0, IsObservationDeleted: 0
                            };
                            ProcedureTemplateDetail.selectedObservations.push(objSelectedObservations);

                        });
                    }
                    else {
                        // $('#observationContent div[id^=divSystem]').hide();
                        $("#ulProcedureTemplateDetailSystemSection li").remove();

                        $("#ulProcedureTemplateDetailSystems li").removeClass('active');
                        $("#ulProcedureTemplateDetailSystems li[id=" + PESystemId + "]").addClass('active');
                    }
                }
            });
        }
        else {
            $("#SystemSections").removeClass('hidden');
            $("#ulProcedureTemplateDetailSystemSection li").remove();

            var selectAll = '<li id="chkboxSelectAllObservations" parentid="' + PESystemId + '" value="Select All" refvalue="Select All"><div class="pl-lg checkbox-custom checkboxTiny checkbox-success"><input type="checkbox" id="chkboxSelectAllObservations" name="Select All" class="pull-left" '
                        + 'onclick="ProcedureTemplateDetail.selectAllChars(this);"><label id="lblSelectAll" class="" data-toggle="tooltip" title="" data-original-title="Select All">Select All</label><div class="clearfix"></div><div class="clearfix"></div></div></li>';
            $("#ulProcedureTemplateDetailSystemSection").append(selectAll);

            if (ProcedureTemplateDetail.selectedObservations) {
                //$('#observationContent div[id^=divSystem]').hide();
                $("#observationContent").text('');
                for (var i = 0 ; i < ProcedureTemplateDetail.selectedObservations.length; i++) {
                    if (ProcedureTemplateDetail.selectedObservations[i].PESystemId == PESystemId) {
                        var li = ProcedureTemplateDetail.addObservations(ProcedureTemplateDetail.selectedObservations[i].ObservationId, ProcedureTemplateDetail.selectedObservations[i].ObservationName, PESystemId);

                        if (ProcedureTemplateDetail.selectedObservations[i].ObservationId != "")
                            $("#ulProcedureTemplateDetailSystemSection").append(li);

                        $("#ulProcedureTemplateDetailSystems li").removeClass('active');
                        $("#ulProcedureTemplateDetailSystems li[id=" + PESystemId + "]").addClass('active');

                        if (ProcedureTemplateDetail.selectedObservations[i].IsChecked) {
                            if (ProcedureTemplateDetail.selectedObservations[i].IsObservationDeleted == 0) {
                                $("#ulProcedureTemplateDetailSystemSection #chk" + ProcedureTemplateDetail.selectedObservations[i].ObservationId).prop("checked", true);
                                $('#observationContent #divSystem' + PESystemId + ProcedureTemplateDetail.selectedObservations[i].ObservationId).show();
                                ProcedureTemplateDetail.selectedObservations[i].IsObservationDeleted = 0;
                                ProcedureTemplateDetail.selectedObservations[i].IsSystemDeleted = 0;
                                ProcedureTemplateDetail.handleDelimiter(PESystemId, ProcedureTemplateDetail.selectedObservations[i].ObservationId, ProcedureTemplateDetail.selectedObservations[i].ObservationName, true);
                            }
                            else {
                                $("#ulProcedureTemplateDetailSystemSection #chk" + ProcedureTemplateDetail.selectedObservations[i].ObservationId).prop("checked", false);
                                ProcedureTemplateDetail.selectedObservations[i].IsObservationDeleted = 0;
                                ProcedureTemplateDetail.selectedObservations[i].IsSystemDeleted = 0;
                                ProcedureTemplateDetail.selectedObservations[i].IsChecked = false;
                            }
                        }
                        else {
                            $("#ulProcedureTemplateDetailSystemSection #chk" + ProcedureTemplateDetail.selectedObservations[i].ObservationId).prop("checked", false);
                            ProcedureTemplateDetail.selectedObservations[i].IsObservationDeleted = 0;
                            ProcedureTemplateDetail.selectedObservations[i].IsSystemDeleted = 0;
                            ProcedureTemplateDetail.selectedObservations[i].IsChecked = false;
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
        var a = ProcedureTemplateDetail.selectedObservations;

        var itemtoRemove = "observation";

        var li = '<li id="' + PEObservationId + '" parentid="' + PESystemId + '" onclick="ProcedureTemplateDetail.PreviewObservations(' + PEObservationId + ',\'' + ObservatioName + '\', this, ' + PESystemId + ');" value="' + PEObservationId + '" refvalue="" subcharacteristicexist=" " class=""><div class="checkbox-custom checkboxTiny checkbox-success">' +
                 '<input type="checkbox" id="chk' + PEObservationId + '" name="' + PEObservationId + '" class="pull-left  char" ' +
                 'onclick="ProcedureTemplateDetail.PreviewObservations(' + PEObservationId + ',\'' + ObservatioName + '\', this, ' + PESystemId + ');"><label id="lblName' + PEObservationId + '" class="" data-toggle="tooltip" title="" data-original-title="' + ObservatioName + '">' + ObservatioName + '</label><div id="divNameDetail' + PEObservationId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" spellcheck="true" id="txtName' + PEObservationId + '" onkeypress="" name="Name' + PEObservationId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' +
                 PEObservationId + '" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div><a href="#"><span class="removeIconListHover" onclick="ProcedureTemplateDetail.removeObservation(' + PESystemId + ', ' + PEObservationId + ')"><i class="fa fa-close"></i></span></a></li>';
        return li;
    },

    addSystem: function (PESystemId, SystemName) {
        var itemtoRemove = "system";

        for (var i = 0 ; i < ProcedureTemplateDetail.selectedObservations.length; i++) {
            if (ProcedureTemplateDetail.selectedObservations[i].PESystemId == PESystemId && ProcedureTemplateDetail.selectedObservations[i].IsSystemDeleted != 1) {
                utility.DisplayMessages(SystemName + " already associated with the template", 3);
                return;
            }
        }

        for (var i = 0; i < $("#ulProcedureTemplateDetailSystems li").length; i++) {
            if ($($("#ulProcedureTemplateDetailSystems li label")[i]).text() == SystemName) {
                utility.DisplayMessages(SystemName + " already associated with the template", 3);
                return;
            }

        }

        var li = '<li id="' + PESystemId + '" parentid="null" onclick="ProcedureTemplateDetail.loadObservations(' + PESystemId + ')" value="' + PESystemId + '" refvalue="" subcharacteristicexist=" " class=""><div class="checkbox-custom checkboxTiny checkbox-success">' +
                 '<input type="checkbox" id="chk' + PESystemId + '" name="' + PESystemId + '" class="pull-left  char" onclick="ProcedureTemplateDetail.ManageObservations(' + PESystemId + ', this);"><label id="lblName' + PESystemId + '" class="" data-toggle="tooltip" title="" data-original-title="' + SystemName + '">' + SystemName + '</label><div id="divNameDetail' + PESystemId + '" class="rightInnerAddon pb-xs hidden"><div class="slimScrollDiv" style="position: relative; overflow: hidden; width: auto; height: 100%;"><textarea rows="1" spellcheck="true" id="txtName' + PESystemId + '" onkeypress="" name="Name' + PESystemId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll" style="overflow: hidden; width: auto; height: 100%;"></textarea><div class="slimScrollBar" style="background: rgb(0, 0, 0); width: 7px; position: absolute; top: 0px; opacity: 0.4; display: none; border-radius: 7px; z-index: 99; right: 1px;"></div><div class="slimScrollRail" style="width: 7px; height: 100%; position: absolute; top: 0px; display: none; border-radius: 7px; background: rgb(51, 51, 51); opacity: 0.2; z-index: 90; right: 1px;"></div></div><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' +
                 PESystemId + '" onclick="" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div><div class="clearfix"></div></div><a href="#"><span class="removeIconListHover" onclick="ProcedureTemplateDetail.removeSystem(' + PESystemId + ', event)";><i class="fa fa-close"></i></span></a></li>';
        // ProcedureTemplateDetail.removeSystem(' + PESystemId + ')
        return li;
    },
    removeSystem: function (PESystemId, event) {
        $("#ulProcedureTemplateDetailSystems #" + PESystemId).remove();
        $("#ulProcedureTemplateDetailSystemSection").hide();
        if (ProcedureTemplateDetail.selectedObservations) {
            for (var i = 0 ; i < ProcedureTemplateDetail.selectedObservations.length; i++) {
                if (ProcedureTemplateDetail.selectedObservations[i].PESystemId == PESystemId) {
                    ProcedureTemplateDetail.selectedObservations[i].IsSystemDeleted = 1;
                    ProcedureTemplateDetail.selectedObservations[i].IsObservationDeleted = 1;
                    ProcedureTemplateDetail.selectedObservations[i].IsChecked = false;
                    ProcedureTemplateDetail.selectedObservations[i].IsSystemChecked = false;
                    ProcedureTemplateDetail.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + PESystemId).text();
                }
            }
        }
        $("#observationContent div").remove();
        event.stopPropagation();
    },
    ManageObservations: function (PESystemId, obj) {
        $("#ulProcedureTemplateDetailSystems li").removeClass('active');
        $("#ulProcedureTemplateDetailSystems li[id=" + PESystemId + "]").addClass('active');

        if (!$(obj).is(':checked')) {
            $(obj).parent().parent().removeClass('active');

            for (var i = 0; i < $("#ulProcedureTemplateDetailSystemSection li").length; i++) {
                $("#ulProcedureTemplateDetailSystemSection #chk" + $("#ulProcedureTemplateDetailSystemSection li")[i].id).prop('checked', false);
            }
            if (ProcedureTemplateDetail.selectedObservations) {
                for (var i = 0 ; i < ProcedureTemplateDetail.selectedObservations.length; i++) {
                    if (ProcedureTemplateDetail.selectedObservations[i].PESystemId == PESystemId) {
                        ProcedureTemplateDetail.selectedObservations[i].IsSystemChecked = false;
                        ProcedureTemplateDetail.selectedObservations[i].IsChecked = false;
                        ProcedureTemplateDetail.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + PESystemId).text();
                        //break;
                    }
                }
            }
        }
        else {
            if (ProcedureTemplateDetail.selectedObservations) {
                for (var i = 0 ; i < ProcedureTemplateDetail.selectedObservations.length; i++) {
                    if (ProcedureTemplateDetail.selectedObservations[i].PESystemId == PESystemId) {
                        ProcedureTemplateDetail.selectedObservations[i].IsSystemChecked = true;
                        ProcedureTemplateDetail.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + PESystemId).text();
                        break;
                    }
                }
            }
        }
    },
    LoadProcedureTempSysObservations: function (templateId) {
        var objData = new Object();
        objData["ProcedureTemplateId"] = templateId;
        objData["commandType"] = "Load_Procedure_TemplatesFill";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ProcedureTemplate", "ProcedureTemplate");
    },

    checkSpecialtiesByProviderId: function (option, checked, select) {

        //provider context
        var providerContext = '#' + ProcedureTemplateDetail.PanelID + ' #divProcedureTemplateDetailTemplateProvider';
        var isAllProviderSelected = $(providerContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var providerItems = $(providerContext).find('.dropdown-menu').find('li').length;
        var checkedProviderItems = $(providerContext).find('.dropdown-menu').find('li.active').length;

        if (checkedProviderItems <= 0) {
            ProcedureTemplateDetail.providerCheckedIds = [];
            ProcedureTemplateDetail.ProviderIds = '';
        }
            //push all provider checked items
        else if (isAllProviderSelected) {
            ProcedureTemplateDetail.providerCheckedIds = [];
            $('#' + ProcedureTemplateDetail.PanelID + ' #ddlProcedureTemplateDetailProvider option').each(function () {
                var providerValue = $(this).val();
                ProcedureTemplateDetail.providerCheckedIds.push(providerValue);
            });
        }
        else {
            // provider value
            var providerValue = $(option).val();

            // add to provider array if checked
            if (checked) {
                ProcedureTemplateDetail.providerCheckedIds = ProcedureTemplateDetail.removeFromArray(ProcedureTemplateDetail.providerCheckedIds, providerValue);
                ProcedureTemplateDetail.providerCheckedIds.push(providerValue);
            }
                //delete from provider array if not checked
            else {
                ProcedureTemplateDetail.providerCheckedIds = ProcedureTemplateDetail.removeFromArray(ProcedureTemplateDetail.providerCheckedIds, $(option).val());
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
            $('#' + ProcedureTemplateDetail.PanelID + ' #ulProcedureTemplateDetailSystemSection li .char').each(function () {
                if ($(this).attr("id") != $(obj).attr("id") && $(this).prop("checked") == false) {
                    $(this).parent().find("[type='checkbox'][id*='chk']").prop("checked", true);

                    var id_ = $(this).parent().find("[type='checkbox'][id*='chk']").attr('id').substring(3)
                    var observationName = $("#divProcedureTemplateDetailSystemSection #lblName" + id_).text();
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

                    $("#ulProcedureTemplateDetailSystems #chk" + sysId).prop("checked", true);
                }
            });

            if (ProcedureTemplateDetail.selectedObservations) {
                for (var i = 0 ; i < ProcedureTemplateDetail.selectedObservations.length; i++) {
                    if (ProcedureTemplateDetail.selectedObservations[i].PESystemId == sysId) {
                        ProcedureTemplateDetail.selectedObservations[i].IsChecked = true;
                        ProcedureTemplateDetail.selectedObservations[i].IsSystemChecked = true;
                        ProcedureTemplateDetail.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + sysId).text();
                    }
                }
            }
        }
        else if ($(obj).prop('checked') == false) {
            $('#' + ProcedureTemplateDetail.PanelID + ' #ulProcedureTemplateDetailSystemSection li .char').removeClass("green");
            $('#' + ProcedureTemplateDetail.PanelID + ' #ulProcedureTemplateDetailSystemSection li .char').each(function () {
                if ($(this).attr("id") != $(obj).attr("id")) {
                    $(this).parent().find("[type='checkbox'][id*='chk']").prop("checked", false);
                    var id_ = $(this).parent().find("[type='checkbox'][id*='chk']").attr('id').substring(3);
                    var system_id = $($($(obj).parent().parent())[0]).attr('parentid');
                    $("#observationContent #divSystem" + system_id + id_).remove();
                }
            });

            if (ProcedureTemplateDetail.selectedObservations) {
                for (var i = 0 ; i < ProcedureTemplateDetail.selectedObservations.length; i++) {
                    if (ProcedureTemplateDetail.selectedObservations[i].PESystemId == sysId) {
                        ProcedureTemplateDetail.selectedObservations[i].IsChecked = false;
                        ProcedureTemplateDetail.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + sysId).text();
                        //$('#observationContent div[id^=divSystem' + sysId + ']').hide();
                    }
                }
            }
        }
        ProcedureTemplateDetail.removeLastDelimiter(sysId);
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
            currentId = ProcedureTemplateDetail.NewInsertedId--;
            var subcharacteristicExist = "";

            var saveMethodPE = "";

            if (itemType.toLowerCase() == "system") {
                currentLiClick = "";
                currentCtrlId = "ulProcedureTemplateDetailSystems";
                ulControl = $('#' + ProcedureTemplateDetail.PanelID + " #frmProcedureTemplateDetail #" + currentCtrlId);
                saveMethodPE = "ProcedureTemplateDetail.AddSystemPE(this, '" + currentId + "');";
            }
            else if (itemType.toLowerCase() == "subsystem") {
                currentLiClick = "";
                currentCtrlId = "ulProcedureTemplateDetailSystemSection";
                ulControl = $('#' + ProcedureTemplateDetail.PanelID + " #frmProcedureTemplateDetail #" + currentCtrlId);
                saveMethodPE = "ProcedureTemplateDetail.AddObservation(this, '" + currentId + "');";
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
                onClick = "";//"ProcedureTemplateDetail.showHideChildControls(this,'" + currentCtrlId + "','" + currentId + "');";
                var deleteFunction = "ProcedureTemplateDetail.deleteItem(this,'" + currentCtrlId + "','" + currentId + "');";

                var deleteIcon = '<a class="btn btn-xs pull-left" href="#" onclick="' + deleteFunction + '" title="Delete Record"><i class="fa fa-close red"></i></a>';

                liInnerText = '<div class="checkbox-custom checkboxTiny checkbox-success">' + deleteIcon + addSubCharIcon
                    + '<input checked type="checkbox" id="chk' + currentId + '+ve" name="' + currentId + '" class="pull-left  char" onclick="">'
                    //+ '<input checked type="checkbox" id="chk' + currentId + '+ve" name="' + currentId + '" class="pull-left  char" onclick="ProcedureTemplateDetail.selectParentControls(this);ProcedureTemplateDetail.toggleCheckBoxes(this);">'
                    + '<label id="lblName' + currentId + '" class=" hidden" data-toggle="tooltip"  title="" data-original-title="' + currentId + '">' + currentId + '</label>'
                    + '<div id="divNameDetail' + currentId + '" class="rightInnerAddon pb-xs">'
                    + '<textarea rows="1" spellcheck="true" id="txtName' + currentId + '" onkeypress="ProcedureTemplateDetail.saveDetailComments(event,this)"  name="Name' + currentId + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll"></textarea>'
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

        ProcedureTemplateDetail.savePESystem_DBCall(objData).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                utility.DisplayMessages(response.Message, 1);

                var li = ProcedureTemplateDetail.addSystem(response.PESystemId, objData["Name"]);
                $("#ulProcedureTemplateDetailSystems").append(li);

                //var objSelectedObservations = {
                //    PESystemId: response.PESystemId, IsChecked: false, ObservationId: '', ObservationName: '', IsSystemChecked: false, IsSystemDeleted: 0, IsObservationDeleted: 0
                //};
                //ProcedureTemplateDetail.selectedObservations.push(objSelectedObservations);
                ProcedureTemplateDetail.loadObservations(response.PESystemId);
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
            onClickFunction = onClickFunction.replace('this', "$('#" + ProcedureTemplateDetail.PanelID + " #" + ULID + " #" + ID + "')");
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
        var PESystemId = $("#ulProcedureTemplateDetailSystems li.active")[0].id;
        var objData = new Object();
        objData["PESystemId"] = PESystemId;
        objData["Name"] = $("#txtName" + controlId).val();
        objData["IsActive"] = true;

        if (objData["Name"] == null || objData["Name"] == "" || objData["Name"] == undefined) {
            utility.DisplayMessages("Please enter some value", 3);
            return;
        }

        ProcedureTemplateDetail.savePEObservation_DBCall(objData).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status) {
                    var res = JSON.parse(response.PEObservation_JSON);
                    utility.DisplayMessages(response.Message, 1);
                    ProcedureTemplateDetail.params.PEObservationId = res[0].PEObservationId;
                    var li = ProcedureTemplateDetail.addObservations(res[0].PEObservationId, objData["Name"], PESystemId);
                    $("#ulProcedureTemplateDetailSystemSection").append(li);
                    var sysChecked = $("#chk" + $("#ulProcedureTemplateDetailSystems li.active").attr('id')).is(':checked');
                    var objSelectedObservations = {
                        PESystemId: PESystemId, IsChecked: false, ObservationId: res[0].PEObservationId, ObservationName: objData["Name"], IsSystemChecked: sysChecked, IsSystemDeleted: 0, IsObservationDeleted: 0
                    };
                    ProcedureTemplateDetail.selectedObservations.push(objSelectedObservations);
                    $("#" + controlId).remove();
                }
                else {
                    utility.DisplayMessages(response.Message, 2);
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
            $('#' + ProcedureTemplateDetail.PanelID + ' #ulProcedureTemplateDetailSystemSection li .char').removeClass("green");
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

            if (ProcedureTemplateDetail.selectedObservations) {
                for (var i = 0 ; i < ProcedureTemplateDetail.selectedObservations.length; i++) {
                    if (ProcedureTemplateDetail.selectedObservations[i].PESystemId == PESystemId && ProcedureTemplateDetail.selectedObservations[i].ObservationId == observationId) {
                        ProcedureTemplateDetail.selectedObservations[i].IsChecked = true;
                        ProcedureTemplateDetail.selectedObservations[i].IsSystemChecked = true;
                        ProcedureTemplateDetail.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + PESystemId).text();
                    }
                    if (ProcedureTemplateDetail.selectedObservations[i].PESystemId == PESystemId) {
                        ProcedureTemplateDetail.selectedObservations[i].IsSystemChecked = true;
                        ProcedureTemplateDetail.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + PESystemId).text();
                    }
                }
            }

            $("#ulProcedureTemplateDetailSystems #chk" + PESystemId).prop("checked", true);
            ProcedureTemplateDetail.loadObservations(PESystemId);
        }
        else if ($(obj).prop('checked') == false) {
            if (ProcedureTemplateDetail.selectedObservations) {
                for (var i = 0 ; i < ProcedureTemplateDetail.selectedObservations.length; i++) {
                    if (ProcedureTemplateDetail.selectedObservations[i].PESystemId == PESystemId && ProcedureTemplateDetail.selectedObservations[i].ObservationId == observationId) {
                        ProcedureTemplateDetail.selectedObservations[i].IsChecked = false;
                        ProcedureTemplateDetail.selectedObservations[i].SystemDescription = $('#observationContent #divSystem' + PESystemId).text();
                    }
                }
            }
            var aa = $('#observationContent #divSystem' + PESystemId + observationId).text();
            $('#observationContent #divSystem' + PESystemId + observationId).remove();
            ProcedureTemplateDetail.removeLastDelimiter(PESystemId);
        }
        ProcedureTemplateDetail.removeLastDelimiter(PESystemId);
    },
    bindAutoComplete: function (element) {
        //var hiddenCrtl = $(element);
        // utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "ClinicalLabOrderDetail", null, true);


        var labId = $('#pnlProcedureTemplateDetail #ddlLabId').val();
        var codesystem = $('#' + ProcedureTemplateDetail.PanelID + ' #ddlLabId option:selected').attr('codesystem');
        EMRUtility.BindLOINCCodes(element, "ProcedureTemplateDetail", labId, null, '1');

    },
    removeProcedure: function (CPTCode, CPTCodeDescription, obj, event) {
        for (i = 0; i < ProcedureTemplateDetail.procedures.length ; i++) {
            if (ProcedureTemplateDetail.procedures[i].CPTCode == CPTCode && ProcedureTemplateDetail.procedures[i].CPTCodeDescription == CPTCodeDescription) {
                ProcedureTemplateDetail.procedures.splice(i, 1);
            }
        }
         
        $(obj).parent().remove();
        
    },
    pushLOINCAsCpt: function (JsonObj) {


        var count = 0;
        for (i = 0 ; i < ProcedureTemplateDetail.procedures.length ; i++) {
            if (ProcedureTemplateDetail.procedures[i].CPTCode == JsonObj['LOINICCODE'] && ProcedureTemplateDetail.procedures[i].CPTCodeDescription == JsonObj['LOINICDescription']) {
                count++;
            }
        }
        if (count > 0) {
            utility.DisplayMessages("This procedure is already added.", 2);
        }
        else
        {
            ProcedureTemplateDetail.procedures.push({ CPTCode: JsonObj['LOINICCODE'], CPTCodeDescription: JsonObj['LOINICDescription'] });
            $('#pnlProcedureTemplateDetail #txtLabCPTCode').val("");
            var li = "<li " +
            ">" + JsonObj["LOINICCODE"] + " " + JsonObj["LOINICDescription"] +
            "<span class='removeIconListHover' onclick='ProcedureTemplateDetail.removeProcedure(\"" + JsonObj["LOINICCODE"] + "\",\"" + String(JsonObj["LOINICDescription"]) + "\", this, event)';><i class='fa fa-close'></i></span></a></li>";
            // ProcedureTemplateDetail.removeSystem(" + PESystemId + ")
            $('#pnlProcedureTemplateDetail #ulProcedureTemplateDetailProcedures').append(li);
        }

    },

    loadDropDowns: function (dfd) {
        var self = $('#' + ProcedureTemplateDetail.PanelID + ' #tblProcedureTemplateDetail');
        if (globalAppdata.AppUserName.toLowerCase() != DefaultUser.toLowerCase()) {
            self.find("#ddlProcedureTemplateEntity").val(globalAppdata["SeletedEntityId"]);
            if (self.find("div#divEntity").hasClass("hidden") == false)
                self.find("div#divEntity").addClass("hidden");
            isSelectedEntity = true;
        }
        else {
            self.find("div#divEntity").removeClass("hidden");
        }
        ProcedureTemplateDetail.loadEntitySpecialty(globalAppdata["SeletedEntityId"], dfd);
        if (ProcedureTemplateDetail.params.Title != null)
            $("#" + ProcedureTemplateDetail.params["PanelID"] + " #headingTitle").text(ProcedureTemplateDetail.params.Title);
    },
    updateCategory: function () {
        var Lab = $('#' + ProcedureTemplateDetail.PanelID + ' #ddlLabId option:selected');
        $('#' + ProcedureTemplateDetail.PanelID + ' #txtCategory').val($(Lab).attr('category'));

        ProcedureTemplateDetail.EnableDisableTestControls();
    },

    Unload: function (IsActiveGrid) {
        // Empty global variables
        ProcedureTemplateDetail.procedures = [];
        ProcedureTemplateDetail.specialityCheckedIds = [];
        ProcedureTemplateDetail.providerCheckedIds = [];
        ProcedureTemplateDetail.providerSelectedIds = [];
        ProcedureTemplateDetail.selectedPhyExamTempData = [];
        ProcedureTemplateDetail.SpecialtyIds = "";
        ProcedureTemplateDetail.ProviderIds = "";
        ProcedureTemplateDetail.selectedObservations = [];
        ProcedureTemplateDetail.specialtyIdMKMK = [];
        ProcedureTemplateDetail.ProviderIdMKMK = [];

        if (IsActiveGrid != null && IsActiveGrid != undefined) {
            //  ProcedureTemplate.LoadProcedureTemplates(IsActiveGrid == true ? 1 : 0).done(function (response) {
            //  ProcedureTemplate.ProcedureTemplatesGridLoad(response);
            var switcher = $('#' + ProcedureTemplate.PanelID + ' #switchActive');

            $(switcher).attr('IsActive', IsActiveGrid == true ? '0' : '1');
            if (IsActiveGrid == "1" || IsActiveGrid == true) {
                $('#' + ProcedureTemplate.PanelID + ' .ios-switch').attr('class', 'ios-switch on');
                ProcedureTemplate.changeSwitch(switcher[0]);
            }
            else {
                $('#' + ProcedureTemplate.PanelID + ' .ios-switch').attr('class', 'ios-switch off');
                ProcedureTemplate.changeSwitch(switcher[0]);
            }

            //    });
        }
        if (ProcedureTemplateDetail.params.ParentCtrl) {
            UnloadActionPan(ProcedureTemplateDetail.params["ParentCtrl"], "ProcedureTemplateDetail");
        }
        else {
            UnloadActionPan();
        }

    }
}