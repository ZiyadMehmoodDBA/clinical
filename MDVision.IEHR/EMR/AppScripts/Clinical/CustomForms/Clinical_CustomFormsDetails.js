Clinical_CustomFormsDetails = {
    bIsFirstLoad: true,
    params: [],
    specialityCheckedIds: [],
    providerCheckedIds: [],
    SpecialtyIds: '',
    ProviderIds: '',
    AttachCatIds: '',
    isDragged: false,
    attachCatCheckedIds: [],
    isBelongToQgroup: false,
    Load: function (params) {
        Clinical_CustomFormsDetails.params = params;
        if (Clinical_CustomFormsDetails.params["PanelID"] != 'pnlClinicalCustomFormsDetails')
            Clinical_CustomFormsDetails.params["PanelID"] = Clinical_CustomFormsDetails.params["PanelID"] + ' #pnlClinicalCustomFormsDetails';
        if (Clinical_CustomFormsDetails.bIsFirstLoad) {
            Clinical_CustomFormsDetails.bIsFirstLoad = false;
            selectedEntity = globalAppdata["SeletedEntityId"];
            var self = $('#' + Clinical_CustomFormsDetails.params.PanelID);
            self.loadDropDowns(true).done(function () {
                Clinical_CustomFormsDetails.ValidateCustomFormsDetails();
                $.when(Clinical_CustomFormsDetails.loadEntitySpecialty(selectedEntity)).then(function () {
                    Clinical_CustomFormsDetails.loadEntityProvider(selectedEntity);
                });
                Clinical_CustomFormsDetails.IntializeMultiSelectDropDownAttachCategory();
                Clinical_CustomFormsDetails.fillCategoryDropdown();
                if (Clinical_CustomFormsDetails.params["mode"] == "Edit" && Clinical_CustomFormsDetails.params.CustomFormId > 0) {
                    Clinical_CustomFormsDetails.customFormFill(Clinical_CustomFormsDetails.params.CustomFormId);
                }
                $('#frmCustomFormsDetails').data('serialize', $('#frmCustomFormsDetails').serialize());
            });
            $(".customFormHeading").focusout(function () {
                if ($(this).is(":visible") && $(this).val() != "") {
                    $(this).hide().siblings("#lblCustomFormHeading").show().text($(this).val());
                    $('#txtFormHeading').val($(this).val());
                    $('#lnkEditFormName').show();
                }
            });
        }
        utility.callbackAfterAllDOMLoaded(function () {
            Clinical_CustomFormsDetails.removeDivUiResizeableHandle();
        });
    },
    InitializeResizable: function () {
        $('.resizable').resizable({
            containment: '#customFormDetails',
            autoHide: true,
            handles: 'e',
            minWidth: 50,
            stop: function (e, ui) {
                var parent = ui.element.parent();
                ui.element.css({
                    width: (ui.size.width / parent.width() * 100) + "%",
                    height: 'auto'
                });
            }
        });
    },
    // Start for clear form after save close - ZeeshanAK on 28 September, 2016
    clearCustomForm: function () {
        $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlSpecialty').val('')
        $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlSpecialty').multiselect('clearSelection');
        $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlSpecialty').multiselect('refresh');
        Clinical_CustomFormsDetails.specialityCheckedIds = ''

        $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlProvider').val('')
        $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlProvider').multiselect('clearSelection');
        $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlProvider').multiselect('refresh');
        Clinical_CustomFormsDetails.providerCheckedIds = ''

    },
    // End for clear form after save close - ZeeshanAK on 28 September, 2016
    // Start for Attach Category multi select ddl - ZeeshanAK on 23 September, 2016
    fillCategoryDropdown: function () {
        Clinical_CustomFormsDetails.fillCategoryDropdown_Dbcall().done(function (response) {

            var response = JSON.parse(response);
            if (response.categoryCount > 0) {
                $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlAttachCategory').empty();
                $.each(JSON.parse(response.categoryList_JSON), function (i, item) {
                    if (item.ShortName != '' && item.ShortName != null) {
                        $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlAttachCategory').append(
                            $('<option/>', {
                                value: item.AttachCategoryId,
                                html: item.ShortName,
                            })
                        );
                    }
                });


                $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlAttachCategory').multiselect('destroy');

                $('#' + Clinical_CustomFormsDetails.params["PanelID"] + ' #ddlAttachCategory').multiselect({
                    includeSelectAllOption: true,
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true,
                    onDropdownShow: function (event) {
                        $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlAttachCategory').parent().find('.multiselect-container > li > a ').css('white-space', 'normal');
                    },

                });
                $('#frmCustomFormsDetails').data('serialize', $('#frmCustomFormsDetails').serialize());
            }
        });
    },
    fillCategoryDropdown_Dbcall: function () {
        var objData = new Object();
        objData["commandType"] = "LOOKUP_CATEGORY";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CustomForm", "CustomForm");
    },
    // End for Attach Category multi select ddl - ZeeshanAK on 23 September, 2016

    // Start for custom form Fill  - ZeeshanAK on 23 September, 2016
    customFormFill: function (formId) {
        Clinical_CustomFormsDetails.customFormFill_DBCall(formId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $.when(Clinical_CustomFormsDetails.customFormFillData(response)).then(function () {
                    Clinical_GlobalQuestionGroup.fillGlobalQuestions();
                    Clinical_GlobalQuestionGroup.fillGlobalQuestionGroups();
                    Clinical_CustomFormsDetails.InitializeDragable();
                    Clinical_CustomFormsDetails.InitilizeSortable();
                    Clinical_CustomFormsDetails.InitializeResizable();
                    $('#frmCustomFormsDetails').data('serialize', $('#frmCustomFormsDetails').serialize());
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    customFormFill_DBCall: function (formId) {
        var objData = {};
        objData["CustomFormId"] = formId;
        objData["commandType"] = "fill_custom_form";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CustomForm", "CustomForm");
    },

    customFormFillData: function (response) {
        var customFormData = response.listCustomForm;
        $('#' + Clinical_CustomFormsDetails.params.PanelID + ' input#txtFormName').val(customFormData[0].FormName);

        $('#' + Clinical_CustomFormsDetails.params.PanelID + ' input#txtFormHeading').val(customFormData[0].FormHeading);
        $('#' + Clinical_CustomFormsDetails.params.PanelID + ' input#chkActive').prop('checked', customFormData[0].IsActive == 'True' ? true : false);
        $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlCanvasCol').val(customFormData[0].CanvasCols);
        $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlSpecialty').multiselect('clearSelection', false);
        $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlSpecialty').multiselect('updateButtonText');
        if ($('#' + Clinical_CustomFormsDetails.params.PanelID + ' input#chkActive').hasClass('disableAll'))
            $('#' + Clinical_CustomFormsDetails.params.PanelID + ' input#chkActive').removeClass('disableAll');
        var self = $("#" + Clinical_CustomFormsDetails.params.PanelID + " #frmCustomFormsDetails");
        self.find('#pnlCustomeFormArea').removeClass('hidden');
        //self.find('#txtFormName').addClass('disableAll');
        if (self.find("#lblCustomFormHeading")) {
            if (self.find("#txtFormHeading").val() != "") {
                self.find("#lblCustomFormHeading").empty();
                self.find("#lblCustomFormHeading").append(self.find("#txtFormHeading").val());
            }
        }
        Clinical_CustomFormsDetails.params.CustomFormId = customFormData[0].CustomFormId;
        setTimeout(function () {
            if (customFormData[0].SpecialtyIds != "") {
                EMRUtility.selectOptionsByCommaSeprateValue($('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlSpecialty'), customFormData[0].SpecialtyIds);
                $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlSpecialty').multiselect("refresh");
                Clinical_CustomFormsDetails.specialityCheckedIds = customFormData[0].SpecialtyIds.split(',');
            }
            $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlProvider').multiselect('clearSelection', false);
            $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlProvider').multiselect('updateButtonText');
            if (customFormData[0].ProviderIds != "") {
                EMRUtility.selectOptionsByCommaSeprateValue($('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlProvider'), customFormData[0].ProviderIds);
                $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlProvider').multiselect("refresh");
                Clinical_CustomFormsDetails.providerCheckedIds = customFormData[0].ProviderIds.split(',');
            }
            if (customFormData[0].AttachCatIds != "") {
                EMRUtility.selectOptionsByCommaSeprateValue($('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlAttachCategory'), customFormData[0].AttachCatIds);
                $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlAttachCategory').multiselect("refresh");
            }

            Clinical_CustomFormsDetails.filterProvidersBySpecialtyIds()
            $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlProvider').val(Clinical_CustomFormsDetails.providerCheckedIds);
            Clinical_CustomFormsDetails.IntializeMultiSelectDropDownProviders();

            $('#frmCustomFormsDetails').data('serialize', $('#frmCustomFormsDetails').serialize());
        }, 200);
        if (customFormData[0].CustomFormHTML != "") {
            var html = customFormData[0].CustomFormHTML;
            $("#" + Clinical_CustomFormsDetails.params.PanelID + " #customFormDetails").html($(html));
            Clinical_CustomFormsDetails.InitializeDragableGroup();
            Clinical_CustomFormsDetails.customFormHTMLHoverEvent();
        }
    },

    // End  for custom form Fill  - ZeeshanAK on 23 September, 2016
    loadEntitySpecialty: function (entityID) {
        var objDeffered = $.Deferred();
        if (entityID != null && entityID > 0) {
            providerDetail.FillSpecialty(entityID).done(function (response) {
                if (response.status != false) {
                    var spacialties = JSON.parse(response.SpecialtyLoad_JSON);
                    $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlSpecialty').empty();
                    $.each(spacialties, function (i, item) {
                        $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlSpecialty').append(
                            $('<option/>', {
                                value: item.SpecialtyId,
                                html: item.ShortName
                            })
                        );
                    });
                    //Assign server side spacialties to the specialityCheckedIds array
                    if (Clinical_CustomFormsDetails.SpecialtyIds != '') {
                        var Specialties = Clinical_CustomFormsDetails.SpecialtyIds.split(",");
                        Clinical_CustomFormsDetails.specialityCheckedIds = Specialties;
                        $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlSpecialty').val(Specialties);
                    }
                    $('#frmCustomFormsDetails').data('serialize', $('#frmCustomFormsDetails').serialize());
                }

            }).then(function () {
                Clinical_CustomFormsDetails.IntializeMultiSelectDropDownSpecialties();
                $('#frmCustomFormsDetails').data('serialize', $('#frmCustomFormsDetails').serialize());
                objDeffered.resolve();
            });
        }
        else {

            objDeffered.resolve();
        }
        return objDeffered;
    },
    loadEntityProvider: function (entityId) {
        var objDeffered = $.Deferred();
        var data = "entityID=" + entityId;
        if (entityId != null && entityId > 0) {

            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlProvider');
                var $providerHiddenDdl = $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlHiddenProvider');

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
                if (Clinical_CustomFormsDetails.ProviderIds != '') {
                    var Providers = Clinical_CustomFormsDetails.ProviderIds.split(",");
                    Clinical_CustomFormsDetails.providerCheckedIds = Providers;
                    $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlProvider').val(Providers);
                }
                $('#frmCustomFormsDetails').data('serialize', $('#frmCustomFormsDetails').serialize());
            }).then(function () {
                // A hack to trigger the onDropDownHide event of Spacialty multiselect
                $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #divSpecialty').find('.dropdown-toggle').dropdown('toggle').dropdown('toggle');
                //Intialized in onhidden spacialty ddl.
                Clinical_CustomFormsDetails.IntializeMultiSelectDropDownProviders();
                $('#frmCustomFormsDetails').data('serialize', $('#frmCustomFormsDetails').serialize());
                objDeffered.resolve();

            });
        }
        else {
            objDeffered.resolve();
        }
        return objDeffered;
    },
    IntializeMultiSelectDropDownSpecialties: function () {
        $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlSpecialty').multiselect('destroy');
        $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlSpecialty').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: false,
            onChange: function (option, checked, select) {
                Clinical_CustomFormsDetails.checkProvidersBySpecialityIds(option, checked, select);
            },
            onDropdownHide: function (event) {
                $.when(
                    Clinical_CustomFormsDetails.filterProvidersBySpecialtyIds()
                ).then(function () {
                    if (Clinical_CustomFormsDetails.ProviderIds != '') {
                        var Providers = Clinical_CustomFormsDetails.ProviderIds.split(",");

                        if (Providers != '' && typeof Providers != 'undefined') {

                            $.each(Providers, function (index, item) {
                                Clinical_CustomFormsDetails.providerCheckedIds = Clinical_CustomFormsDetails.removeFromArray(Clinical_CustomFormsDetails.providerCheckedIds, item);
                                Clinical_CustomFormsDetails.providerCheckedIds.push(item);
                            });
                        }
                    }
                    $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlProvider').val(Clinical_CustomFormsDetails.providerCheckedIds);
                    Clinical_CustomFormsDetails.IntializeMultiSelectDropDownProviders();
                });
            },

            onDropdownShow: function (event) {
                //make items selected and initialize dropdownlist
                if (Clinical_CustomFormsDetails.SpecialtyIds != '') {
                    var spacialties = Clinical_CustomFormsDetails.SpecialtyIds.split(",");

                    if (spacialties != '' && typeof spacialties != 'undefined') {

                        $.each(spacialties, function (index, item) {
                            Clinical_CustomFormsDetails.specialityCheckedIds = Clinical_CustomFormsDetails.removeFromArray(Clinical_CustomFormsDetails.specialityCheckedIds, item);
                            Clinical_CustomFormsDetails.specialityCheckedIds.push(item);
                        });
                    }
                }
                Clinical_CustomFormsDetails.setSpacialtiesByselectedProviderIds();
                $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlSpecialty').multiselect('clearSelection', false);
                $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlSpecialty').multiselect('updateButtonText');
                $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlSpecialty').multiselect('select', Clinical_CustomFormsDetails.specialityCheckedIds);
                $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlSpecialty').multiselect('refresh');
            },
        });
    },
    IntializeMultiSelectDropDownProviders: function () {
        $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlProvider').multiselect('destroy');
        $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'Select',
            selectAll: false,
            onChange: function (option, checked, select) {
                Clinical_CustomFormsDetails.checkSpecialtiesByProviderId(option, checked, select);
            },
            onDropdownHide: function (event) {
            },
        });
    },
    filterProvidersBySpecialtyIds: function () {

        var providerHiddenContext = '#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlHiddenProvider';

        var providerContext = '#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlProvider';
        $(providerContext).empty();

        if (Clinical_CustomFormsDetails.specialityCheckedIds.length > 0) {

            $.each(Clinical_CustomFormsDetails.specialityCheckedIds, function (index, specialtyId) {

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
    setSpacialtiesByselectedProviderIds: function () {

        $.each(Clinical_CustomFormsDetails.providerCheckedIds, function (index, item) {

            $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlProvider option').each(function () {
                if ($(this).val() != '') {
                    if ($(this).val() == item) {
                        Clinical_CustomFormsDetails.specialityCheckedIds = Clinical_CustomFormsDetails.removeFromArray(Clinical_CustomFormsDetails.specialityCheckedIds, $(this).attr('refname'));
                        Clinical_CustomFormsDetails.specialityCheckedIds.push($(this).attr('refname'));
                    }
                }
            });
        });
    },
    checkSpecialtiesByProviderId: function (option, checked, select) {
        //provider context
        var providerContext = '#' + Clinical_CustomFormsDetails.params.PanelID + ' #divProvider';
        var isAllProviderSelected = $(providerContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var providerItems = $(providerContext).find('.dropdown-menu').find('li').length;
        var checkedProviderItems = $(providerContext).find('.dropdown-menu').find('li.active').length;

        var allProviders = $(providerContext).find('.dropdown-menu').find('li:not(".filter,.multiselect-all")').length;
        var selectedProviders = $(providerContext).find('.dropdown-menu').find('li.active:not(".filter,.multiselect-all")').length;

        if (checkedProviderItems <= 0) {
            Clinical_CustomFormsDetails.providerCheckedIds = [];
            Clinical_CustomFormsDetails.ProviderIds = '';
        }
            //push all provider checked items
        else if (isAllProviderSelected && allProviders == selectedProviders) {
            Clinical_CustomFormsDetails.providerCheckedIds = [];
            $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlProvider option').each(function () {
                var providerValue = $(this).val();
                Clinical_CustomFormsDetails.providerCheckedIds.push(providerValue);
            });
        }
        else {
            // provider value
            var providerValue = $(option).val();

            // add to provider array if checked
            if (checked) {
                Clinical_CustomFormsDetails.providerCheckedIds = Clinical_CustomFormsDetails.removeFromArray(Clinical_CustomFormsDetails.providerCheckedIds, providerValue);
                Clinical_CustomFormsDetails.providerCheckedIds.push(providerValue);
            }
                //delete from provider array if not checked
            else {
                Clinical_CustomFormsDetails.providerCheckedIds = Clinical_CustomFormsDetails.removeFromArray(Clinical_CustomFormsDetails.providerCheckedIds, $(option).val());
            }

        }
    },
    checkProvidersBySpecialityIds: function (option, checked, select) {
        //specialty context
        var specialtyContext = '#' + Clinical_CustomFormsDetails.params.PanelID + ' #divSpecialty';
        var isAllSpecialtySelected = $(specialtyContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var specialtyItems = $(specialtyContext).find('.dropdown-menu').find('li').length;
        var checkedSpecialtyItems = $(specialtyContext).find('.dropdown-menu').find('li.active').length;

        if (checkedSpecialtyItems <= 0) {
            Clinical_CustomFormsDetails.specialityCheckedIds = [];
            Clinical_CustomFormsDetails.providerCheckedIds = [];
            Clinical_CustomFormsDetails.ProviderIds = '';
            Clinical_CustomFormsDetails.SpecialtyIds = '';
        }
        else {
            if (!isAllSpecialtySelected && !(specialtyItems == checkedSpecialtyItems)) {
                var spacialityId = $(option).attr("value");
                if (checked && spacialityId != "") {
                    Clinical_CustomFormsDetails.specialityCheckedIds = Clinical_CustomFormsDetails.removeFromArray(Clinical_CustomFormsDetails.specialityCheckedIds, spacialityId);
                    Clinical_CustomFormsDetails.specialityCheckedIds.push(spacialityId);
                }
                else {

                    Clinical_CustomFormsDetails.specialityCheckedIds = Clinical_CustomFormsDetails.removeFromArray(Clinical_CustomFormsDetails.specialityCheckedIds, spacialityId);
                }
            }
            else {
                Clinical_CustomFormsDetails.specialityCheckedIds = [];
                $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlSpecialty option').each(function () {
                    var spacialityId = $(this).attr("value");
                    Clinical_CustomFormsDetails.specialityCheckedIds.push(spacialityId);
                });
            }
        }
    },
    IntializeMultiSelectDropDownAttachCategory: function () {
        $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlAttachCategory').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'All',
            selectAll: false,
            onChange: function (option, checked, select) {
                Clinical_CustomFormsDetails.checkAttachCatIds(option, checked, select);
            },
        });
        $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlAttachCategory, #' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlProvider').multiselect('selectAll', false);
    },
    checkAttachCatIds: function (option, checked, select) {
        //specialty context
        var AttachCategoryContext = '#' + Clinical_CustomFormsDetails.params.PanelID + ' #divAttachCategory';
        var isAllAttachCategorySelected = $(AttachCategoryContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var attachCategoryItems = $(AttachCategoryContext).find('.dropdown-menu').find('li').length;
        var checkedAttachCategoryItems = $(AttachCategoryContext).find('.dropdown-menu').find('li.active').length;

        if (checkedAttachCategoryItems <= 0) {
            Clinical_CustomFormsDetails.attachCatCheckedIds = [];
            Clinical_CustomFormsDetails.AttachCatIds = '';
        }
        else {
            if (!isAllAttachCategorySelected && !(attachCategoryItems == checkedAttachCategoryItems)) {
                var spacialityId = $(option).attr("value");
                if (checked && spacialityId != "") {
                    Clinical_CustomFormsDetails.attachCatCheckedIds = Clinical_CustomFormsDetails.removeFromArray(Clinical_CustomFormsDetails.attachCatCheckedIds, spacialityId);
                    Clinical_CustomFormsDetails.attachCatCheckedIds.push(spacialityId);
                }
                else {

                    Clinical_CustomFormsDetails.attachCatCheckedIds = Clinical_CustomFormsDetails.removeFromArray(Clinical_CustomFormsDetails.attachCatCheckedIds, spacialityId);
                }
            }
            else {
                Clinical_CustomFormsDetails.attachCatCheckedIds = [];
                $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #ddlAttachCategory option').each(function () {
                    var attachCategoryId = $(this).attr("value");
                    Clinical_CustomFormsDetails.attachCatCheckedIds.push(attachCategoryId);
                });
            }
        }
    },
    CustomFormLoad: function () {
        if (Clinical_CustomFormsDetails.params.mode == "Add") {
            $("#" + Clinical_CustomFormsDetails.params["PanelID"] + " #pnlQuestionsinfo").addClass('disableAll');
            $('#frmClinical_CustomFormsDetails').data('serialize', $('#frmClinical_CustomFormsDetails').serialize());
            Clinical_CustomFormsDetails.ValidateClinical_CustomFormsDetails(Clinical_CustomFormsDetails.params.SectionId);
        }
        else {
            Clinical_CustomFormsDetails.SectionLoadEditMode();
        }
    },
    UnLoadTab: function () {
        utility.UnLoadDialog("frmCustomFormsDetails", function () {
            UnloadActionPan(Clinical_CustomFormsDetails.params["ParentCtrl"], "Clinical_CustomFormsDetails");
            Clinical_CustomFormsDetails.clearCustomForm();
        }, function () {
            UnloadActionPan(Clinical_CustomFormsDetails.params["ParentCtrl"], "Clinical_CustomFormsDetails");
            Clinical_CustomFormsDetails.clearCustomForm();
        });
        Clinical_CustomForms.customFormsSearch();
    },
    removeFromArray: function (array, removeItem) {
        var resultantArray = jQuery.grep(array, function (item) {
            return item != removeItem;
        });
        return resultantArray;
    },
    CustomFormsDetailsSave: function () {
        var strMessage = "";
        var self = $("#" + Clinical_CustomFormsDetails.params.PanelID + " #frmCustomFormsDetails");
        var myJSON = self.getMyJSONByName();
        if (Clinical_CustomFormsDetails.params.mode == "Add") {
            Clinical_CustomFormsDetails.SaveCustomFormsDetails(myJSON).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var canvasCol = $("#" + Clinical_CustomFormsDetails.params.PanelID + " #frmCustomFormsDetails #ddlCanvasCol option:selected").val();
                    utility.DisplayMessages(response.Message, 1);
                    self.find('#pnlCustomeFormArea').removeClass('hidden');
                    //  self.find('#txtFormName').addClass('disableAll');
                    if (self.find("#txtFormHeading").val() != "") {
                        self.find("#lblCustomFormHeading").empty();
                        self.find("#lblCustomFormHeading").append(self.find("#txtFormHeading").val());
                    }
                    self.find('#customFormDetails').attr('canvasCol', canvasCol);
                    //Clinical_CustomFormsDetails.initilizeGridster();
                    Clinical_CustomFormsDetails.InitializeDragable();
                    Clinical_CustomFormsDetails.InitilizeSortable();
                    Clinical_CustomFormsDetails.params.CustomFormId = response.CustomFormId;
                    Clinical_CustomFormsDetails.params.mode = "Edit";
                    Clinical_GlobalQuestionGroup.fillGlobalQuestions();
                    Clinical_GlobalQuestionGroup.fillGlobalQuestionGroups();
                    Clinical_CustomFormsDetails.InitializeResizable();
                    //var gridster = $("#" + Clinical_CustomFormsDetails.params.PanelID + " #frmCustomFormsDetails #customFormDetails").data('gridster');
                    // var canvasCol = $("#" + Clinical_CustomFormsDetails.params.PanelID + " #frmCustomFormsDetails #ddlCanvasCol option:selected").val();
                    //  gridster.generate_stylesheet({ rows: Infinity, cols: parseInt(canvasCol) });
                    $('#frmCustomFormsDetails').data('serialize', $('#frmCustomFormsDetails').serialize());
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else if (Clinical_CustomFormsDetails.params.mode == "Edit") {
            var selectedSchTypeId = $("#" + Clinical_CustomFormsDetails.params.PanelID + " #frmCustomFormsDetails #ddlScheduleType option:selected").val();
            Clinical_CustomFormsDetails.UpdateCustomFormsDetails(myJSON, Clinical_CustomFormsDetails.params.VaccineGroupId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    var canvasCol = $("#" + Clinical_CustomFormsDetails.params.PanelID + " #frmCustomFormsDetails #ddlCanvasCol option:selected").val();
                    self.find('#pnlCustomeFormArea').removeClass('hidden');
                    //self.find('#txtFormName').addClass('disableAll');
                    if (self.find("#txtFormHeading").val() != "") {
                        self.find("#lblCustomFormHeading").empty();
                        self.find("#lblCustomFormHeading").append(self.find("#txtFormHeading").val());
                    }
                    self.find('#customFormDetails').attr('canvasCol', canvasCol);
                    Clinical_CustomFormsDetails.InitializeDragable();
                    Clinical_CustomFormsDetails.InitilizeSortable();
                    Clinical_CustomFormsDetails.params.mode = "Edit";
                    Clinical_GlobalQuestionGroup.fillGlobalQuestions();
                    Clinical_GlobalQuestionGroup.fillGlobalQuestionGroups();
                    Clinical_CustomFormsDetails.InitializeResizable();
                    $('#frmCustomFormsDetails').data('serialize', $('#frmCustomFormsDetails').serialize());
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    ValidateCustomFormsDetails: function () {
        $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #frmCustomFormsDetails')
        .bootstrapValidator('destroy');
        $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #frmCustomFormsDetails')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   FormName: {
                       group: '.col-sm-4',
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
            Clinical_CustomFormsDetails.CustomFormsDetailsSave();
        });
    },
    SaveCustomFormsDetails: function (jsonData) {
        var objData = JSON.parse(jsonData);
        objData["SpecialtyIds"] = $('#' + Clinical_CustomFormsDetails.params.PanelID + " #ddlSpecialty").val() ? $('#' + Clinical_CustomForms.params.PanelID + " #ddlSpecialty").val().join() : '';
        objData["ProviderIds"] = $('#' + Clinical_CustomFormsDetails.params.PanelID + " #ddlProvider").val() ? $('#' + Clinical_CustomForms.params.PanelID + " #ddlProvider").val().join() : '';
        objData["AttachCatIds"] = $('#' + Clinical_CustomFormsDetails.params.PanelID + " #ddlAttachCategory").val() ? $('#' + Clinical_CustomForms.params.PanelID + " #ddlAttachCategory").val().join() : '';;
        objData["commandType"] = "SAVE_CUSTOM_FORMS";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CustomForm", "CustomForm");
    },
    UpdateCustomFormsDetails: function (jsonData) {
        var objData = JSON.parse(jsonData);
        objData["CustomFormId"] = Clinical_CustomFormsDetails.params.CustomFormId;
        objData["SpecialtyIds"] = $('#' + Clinical_CustomFormsDetails.params.PanelID + " #ddlSpecialty").val() ? $('#' + Clinical_CustomForms.params.PanelID + " #ddlSpecialty").val().join() : '';
        objData["ProviderIds"] = $('#' + Clinical_CustomFormsDetails.params.PanelID + " #ddlProvider").val() ? $('#' + Clinical_CustomForms.params.PanelID + " #ddlProvider").val().join() : '';
        objData["AttachCatIds"] = $('#' + Clinical_CustomFormsDetails.params.PanelID + " #ddlAttachCategory").val() ? $('#' + Clinical_CustomForms.params.PanelID + " #ddlAttachCategory").val().join() : '';
        objData["CustomFormHTML"] = $('#' + Clinical_CustomFormsDetails.params["PanelID"] + ' #frmCustomFormsDetails #customFormDetails').html().trim();
        objData["commandType"] = "UPDATE_CUSTOM_FORMS";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CustomForm", "CustomForm");
    },
    editFormName: function (obj, event) {
        if (event != null) {
            event.stopPropagation();
        }
        $('#lblCustomFormHeading').hide().siblings(".customFormHeading").show().val($('#lblCustomFormHeading').text()).focus();
        $(obj).hide();
    },
    /*Initilize Quiestion to be drageable*/
    InitializeDragable: function () {
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        var canvasCol = $("#" + Clinical_CustomFormsDetails.params.PanelID + " #frmCustomFormsDetails #ddlCanvasCol option:selected").val();
        var gridster = $("#" + Clinical_CustomFormsDetails.params.PanelID + " #frmCustomFormsDetails #customFormDetails");
        if ($(gridster).attr('canvascol')) {
            canvasCol = $(gridster).attr('canvascol');

        }
        $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #frmCustomFormsDetails #toolbarContainer li').draggable({
            revert: "invalid",
            appendTo: "#pnlClinicalCustomFormsDetails #customFormDetails",
            containment: '.dragableCF',
            connectToSortable: '.dragableCF',
            //containment: "#pnlClinicalCustomFormsDetails #customFormDetails",
            stack: "#pnlClinicalCustomFormsDetails #customFormDetails",
            connectWith: '.dragableCF',
            helper: function (ev, ui) {
                //var bootStrpCss = 'col-xs-12';
                //switch (canvasCol) {
                //    case "1":
                //        bootStrpCss = 'col-xs-12'
                //        break;
                //    case "2":
                //        bootStrpCss = 'col-xs-12 col-sm-6'
                //        break;
                //    case "3":
                //        bootStrpCss = 'col-xs-12  col-sm-4'
                //        break;
                //    default:
                //        bootStrpCss = 'col-xs-12'
                //        break;

                //}
                var toolId = $(this).attr('id');
                var ctrlHtml = Clinical_CustomFormsDetails.getDefaultHTMLOfControl(toolId);
                var lstToAppend = '';
                if (toolId == "toolTable" || toolId == "toolHeader" || toolId == "toolTinyMCEditor") {
                    lstToAppend = '<li class="resizable draggable col-xs-12 mb-sm">' + ctrlHtml + '</li>';
                }
                else if (toolId == "toolQuestionGroup") {
                    lstToAppend = '<li class="resizable draggable col-xs-12 mb-sm">' + ctrlHtml + '</li>';
                    Clinical_CustomFormsDetails.InitializeDragableGroup($(ctrlHtml).attr('id'));
                }
                else {
                    lstToAppend = '<li class="resizable draggable mb-sm">' + ctrlHtml + '</li>';
                }
                return lstToAppend;

            },
            stop: function (ev, ui) {
                $(ui.helper).removeAttr('style');
                var toolId = $(this).attr('id');
                var canvasCol = '';
                if ($(ui.helper).parent() && $(ui.helper).parent().attr('id').indexOf('toolQuestionGroup') != -1) {
                    canvasCol = ui.helper.parent().attr('canvascol');
                }
                else {
                    canvasCol = $('#ddlCanvasCol').val()
                }
                var bootStrpCss = 'col-xs-12';
                switch (canvasCol) {
                    case "1":
                        bootStrpCss = 'col-xs-12'
                        break;
                    case "2":
                        bootStrpCss = 'col-xs-12 col-sm-6'
                        break;
                    case "3":
                        bootStrpCss = 'col-xs-12  col-sm-4'
                        break;
                    case "4":
                        bootStrpCss = 'col-xs-12  col-sm-3'
                        break;
                    case "6":
                        bootStrpCss = 'col-xs-12  col-sm-2'
                        break;
                    default:
                        bootStrpCss = 'col-xs-12'
                        break;

                }
                if (toolId == "toolTable" || toolId == "toolHeader" || toolId == "toolQuestionGroup" || toolId == "toolTinyMCEditor") {
                    if (!$(ui.helper).hasClass("col-xs-12"))
                        $(ui.helper).addClass("col-xs-12");
                }
                else if (toolId == "toolYesNo") {
                    (ui.helper).addClass("col-xs-12 col-sm-3");
                }
                else if (toolId == "toolFractionField") {
                    (ui.helper).addClass("col-xs-12 col-sm-4");
                }
                else
                    $(ui.helper).addClass(bootStrpCss);

                if (toolId == "toolTable") {
                    Clinical_CustomFormsDetails.createTableForContexMenu($(ui.helper).find('.toolcontroldiv'));
                    $(ui.helper).find('.toolcontroldiv').find(".contextTable tr td").click(function (event) {
                        Clinical_CustomFormsDetails.CreateTable($(this).attr('data-col'), $(this).attr('data-row'), $(ui.helper).find('.toolcontroldiv'));
                    });

                    $(ui.helper).find('.toolcontroldiv').find(".contextTable tr td").hover(function (event) {
                        Clinical_CustomFormsDetails.SelectTable(parseInt($(this).attr('id')), $(this).attr('data-col'), $(ui.helper).find('.toolcontroldiv'));
                    });
                }
                setTimeout(function () {
                    if (toolId != "toolTable") {
                        Clinical_CustomFormsDetails.updateCustomeFormHTML();
                        Clinical_CustomFormsDetails.customFormHTMLHoverEvent();
                    }
                    else if (toolId == "toolTable") {
                        $(ui.helper).find('.questionAction .btnQ_Remove').unbind("click");
                        var Removehandler = function (e) {
                            Clinical_CustomFormsDetails.RemoveQuestion(this, e);
                        };
                        $(ui.helper).find('.questionAction .btnQ_Remove').bind("click", Removehandler);
                    }
                    Clinical_CustomFormsDetails.InitilizeSortable();
                    Clinical_CustomFormsDetails.InitializeResizable();
                }, 300);
                Clinical_CustomFormsDetails.removeDivUiResizeableHandle();
            },

        });
    },
    /*Get the row for the quiestion to be droped*/
    getPrevwidgetrow: function () {
        var canvasCol = $("#" + Clinical_CustomFormsDetails.params.PanelID + " #frmCustomFormsDetails #ddlCanvasCol option:selected").val();
        var row = 1;
        var totalControls = $("#" + Clinical_CustomFormsDetails.params.PanelID + " #frmCustomFormsDetails #customFormDetails li").length;
        if (totalControls && totalControls > 0) {
            var ctrl = $("#" + Clinical_CustomFormsDetails.params.PanelID + " #frmCustomFormsDetails #customFormDetails li").eq(totalControls - 1);
            if ($(ctrl).attr('data-col') && parseInt($(ctrl).attr('data-col')) < parseInt(canvasCol)) {
                if ($(ctrl).attr('data-row')) {
                    row = parseInt($(ctrl).attr('data-row'));
                }
            }
            else if ($(ctrl).attr('data-row')) {
                row = parseInt($(ctrl).attr('data-row')) + 1;
            }
        }
        return row;
    },
    /*Get the column for the quiestion to be droped*/
    getPrevwidgetcol: function () {
        var canvasCol = $("#" + Clinical_CustomFormsDetails.params.PanelID + " #frmCustomFormsDetails #ddlCanvasCol option:selected").val();
        var col = 1;
        var totalControls = $("#" + Clinical_CustomFormsDetails.params.PanelID + " #frmCustomFormsDetails #customFormDetails li").length;
        if (totalControls && totalControls > 0) {
            var ctrl = $("#" + Clinical_CustomFormsDetails.params.PanelID + " #frmCustomFormsDetails #customFormDetails li").eq(totalControls - 1);
            if ($(ctrl).attr('data-col') && parseInt($(ctrl).attr('data-col')) < parseInt(canvasCol)) {
                col = parseInt($(ctrl).attr('data-col')) + 1;
            }
        }
        return col;
    },
    /*To Genrate HTML of draged Question*/
    getDefaultHTMLOfControl: function (ctrl) {
        var ctrlHtml = '';
        var canvasCol = $("#" + Clinical_CustomFormsDetails.params.PanelID + " #frmCustomFormsDetails #ddlCanvasCol option:selected").val();
        var uniquId = utility.makeRendomKey();
        switch (ctrl) {
            case "toolQuestionGroup":
                ctrlHtml = '<div id="toolQuestionGroup_' + Clinical_CustomFormsDetails.params.CustomFormId + '_' + uniquId + '" QuestionType="QuestionGroup"  class="toolQuestionGroup toolcontroldiv heightReset" data-plugin-toggle="" QuestionGroupId="-1" canvasCol="' + canvasCol + '" IsActive="true" QuestionGroupName="QuestionGroup">'
                + '<section class="toggle active">'
                + ' <label class="toggleEditableHeader">'
                + '<span>'
                + '  <span id="lblQuestionGroupTitle" class="">Question Group</span>'
                + ' <a class="btn btnEdit" id="lnkQuestionGroupTitle" href="#" onclick="Clinical_CustomFormsDetails.editQuestionGroupTitle(this, event);" title="Edit Form Name"><i class="fa fa-pencil black"></i></a>'
                + '<input class="form-control questionGroupTitle hidden" name="toolQuestionGroup" />'
                 + ' </span>'
                 + '<a class="toggleButton" href="#"><i class="fa fa-caret-down"></i><i class="fa fa-caret-up"></i></a>'
                 + '</label>'
                + '<div class="toggle-content panel-body NoRadiusT" style="display: block;">'
                + '<div class="pull-right questionAction"><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_copy" title="Copy" name="toolQuestionGroup_' + Clinical_CustomFormsDetails.params.CustomFormId + '_' + uniquId + '"><i class="fa fa-paste"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_SQGlobally" title="Save Globally" name="toolQuestionGroup_' + Clinical_CustomFormsDetails.params.CustomFormId + '_' + uniquId + '"><i class="fa fa-paperclip"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnQ_Remove" title="Delete Question Group"><i class="fa fa-times"></i></a></div>'
                + '<div class="spacer20"></div><div class="spacer10"></div>'
                + '<ul id="toolQuestionGroupHTML" class="list-unstyled dragableCF questionGroupUl" style="width:100% !important; min-height:100px;" canvasCol="' + canvasCol + '"></ul>'
                + '</div>'
                + '</section></div>';
                break;
            case "toolTextField":
                ctrlHtml = '<div  class="panel-body toolcontroldiv" questionid="-1" QuestionType="TextField" name="TextField" isnumber="false" isnewline="false" ismandatory="false" questiontitle="Select Label" issingleline="true" maxlength="" textcase="" defaultvalue="Text Field" id="toolTextField_' + Clinical_CustomFormsDetails.params.CustomFormId + '_' + uniquId + '">'
                 + '<div class="controlContainerDiv">'
                 + '<label class="control-label ellipses size-max100per resetLineHeight" for="toolTextField" id="lblTextField" data-toggle="tooltip" title="Select Label">Select Label</label>'
                 + '<div class="pull-right questionAction"><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_copy" title="Copy" name="toolTextField_' + Clinical_CustomFormsDetails.params.CustomFormId + '"+><i class="fa fa-paste"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_SQGlobally" title="Save Globally" name="toolTextField_' + Clinical_CustomFormsDetails.params.CustomFormId + '_' + uniquId + '"><i class="fa fa-paperclip"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnQ_Remove" title="Delete Question"><i class="fa fa-times"></i></a></div>'
                 + '<input class="form-control" name="TextField" id="txtTextField" type="text" value="Text Field" onchange="GlobalQuestionDetail.updateTextFieldVal(this);">'
                 + '</div>'
                 + '</div>';
                break;
            case "toolCheckBox":
                ctrlHtml = '<div  class="panel-body toolcontroldiv pad-a-labelsize " questionid="-1" questiontype="CheckBox" name="Check Box" isnewline="false" questionlabel="Label 1" selectionmode="0" defaultselection="" id="toolCheckBox_' + Clinical_CustomFormsDetails.params.CustomFormId + uniquId + '">'
                  + '<div class="controlContainerDiv p-none">'
                  + '<div class="pull-right questionAction"><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_copy" title="Copy" name="toolCheckBox_' + Clinical_CustomFormsDetails.params.CustomFormId + '"><i class="fa fa-paste"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_SQGlobally" title="Save Globally" name="toolCheckBox_' + Clinical_CustomFormsDetails.params.CustomFormId + '_' + uniquId + '"><i class="fa fa-paperclip"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnQ_Remove" title="Delete Question"><i class="fa fa-times"></i></a></div>'
                  + '<div class="col-sm-12">'
                  + '<label id="lblCheckBoxTitle" class="control-label ellipses size-max100per resetLineHeight" for="toolCheckBox" data-toggle="tooltip" title="Select Label">Select Label</label>'
                  + '</div>'
                  + '<div id="customFormCheckBoxPreview"class="checkbox-custom checkbox-default pull-left">'
                  + '<input type="checkbox" name="toolCheckBox" id="toolCheckBox_' + uniquId + '" onclick="GlobalQuestionDetail.checkUncheckBox(this)">'
                  + '<label class="control-label ellipses size-max100per resetLineHeight" for="toolCheckBox_' + uniquId + '">Label Name</label>'
                  + '</div>'
                    + '</div>'
                  + '</div>';
                break;
            case "toolYesNo":
                ctrlHtml = '<div  class="panel-body toolcontroldiv pad-a-labelsize" questionid="-1" questiontype="YesNo" name="" isnewline="false" ismandatory="false" questionlabel="Select Label" defaultselection="0" id="toolYesNo_' + Clinical_CustomFormsDetails.params.CustomFormId + uniquId + '">'
                 + '<div class="controlContainerDiv p-none">'
                 + '<label id="customFormYesNoLabel" class="control-label ellipses size-max100per resetLineHeight" for="toolYesNo" data-toggle="tooltip" title="Select Label">Select Label</label>'
                 + '<div class="pull-right questionAction"><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_copy" title="Copy" name="toolYesNo_' + Clinical_CustomFormsDetails.params.CustomFormId + '"+><i class="fa fa-paste"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_SQGlobally" title="Save Globally" name="toolYesNo_' + Clinical_CustomFormsDetails.params.CustomFormId + '"+><i class="fa fa-paperclip"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnQ_Remove" title="Delete Question" ><i class="fa fa-times"></i></a></div>'
                 + '<div id="customFormYesNoPreview"  class="">'
                 + '<div class="checkbox-custom checkbox-default pull-left">'
                 + '<input type="checkbox" id="chkYes_' + uniquId + '" onchange="GlobalQuestionDetail.YesOrNo(this);" name="Yes">'
                 + '<label class="control-label ellipses size-max100per resetLineHeight" for="chkYes_' + uniquId + '">Yes</label>'
                 + '</div>'
                 + '<div class="checkbox-custom checkbox-default pull-right">'
                 + '<input type="checkbox" name="No" onchange="GlobalQuestionDetail.YesOrNo(this);" id="chkNo_' + uniquId + '">'
                 + '<label class="control-label ellipses size-max100per resetLineHeight" for="chkNo_' + uniquId + '">No</label>'
                 + '</div>'
                 + '</div>'
                   + '</div>'
                 + '</div>';
                break;
            case "toolToggle":
                ctrlHtml = '<div  class="panel-body toolcontroldiv" questionid="-1" questiontype="Toggle" name="" isnewline="false" questionlabel="Select Label" defaultselection="0"  id="toolToggle_' + Clinical_CustomFormsDetails.params.CustomFormId + uniquId + '">'
                 + '<div class="controlContainerDiv">'
                 + '<div class="pull-right questionAction"><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_copy" title="Copy" name="toolToggle_' + Clinical_CustomFormsDetails.params.CustomFormId + '"+><i class="fa fa-paste"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_SQGlobally" title="Save Globally" name="toolToggle_' + Clinical_CustomFormsDetails.params.CustomFormId + '"+><i class="fa fa-paperclip"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnQ_Remove" title="Delete Question" name="Patient_Referrals_Main212"><i class="fa fa-times"></i></a></div>'
                 + '<span id="customFormToggleLabel" class="pr-xs">Select Label</span>'
                 + '<div class="btnWidgetSwitch switch switch-xs switch-success">'
                 + '<div id="customFormIosSwitchPreview" class="ios-switch on">'
                 + '<div class="on-background background-fill"></div>'
                 + '<div class="state-background background-fill"></div>'
                 + '<div class="handle"></div>'
                 + '</div>'
                 + '<input checked="checked" class="toggleCheck" id="switchActive" name="switch" onchange="GlobalQuestionDetail.updateSwitchValue(this);" style="display: none;" type="checkbox">'
                 + '</div>'
                   + '</div>'
                 //+ '<span class="pl-xs">Active</span>'
                 + '</div>';
                break;
            case "toolSingleSelectDropdown":
                ctrlHtml = '<div  class="panel-body toolcontroldiv" questionid="-1" questiontype="SingleSelectDropdown" name="" isnewline="false" ismandatory="false" questionlabel="Select Label" defaultselection="0" dropdownvalues="" id="toolSingleSelectDropdown_' + Clinical_CustomFormsDetails.params.CustomFormId + uniquId + '">'
                 + '<div class="controlContainerDiv">'
                 + '<label id="customFormSingleSelectDropdownLabel" class="control-label ellipses size-max100per resetLineHeight" for="TextField" data-toggle="tooltip" title="Select Label">Select Label</label>'
                 + '<div class="pull-right questionAction"><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_copy" title="Copy" name="toolSingleSelectDropdown_' + Clinical_CustomFormsDetails.params.CustomFormId + '"+><i class="fa fa-paste"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_SQGlobally" title="Save Globally" name="toolSingleSelectDropdown_' + Clinical_CustomFormsDetails.params.CustomFormId + '"+><i class="fa fa-paperclip"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnQ_Remove" title="Delete Question" name="Patient_Referrals_Main212"><i class="fa fa-times"></i></a></div>'
                 + '<div id="customFormSingleSelectDropdownList"> <select onchange="GlobalQuestionDetail.updateTextFieldVal(this);" class="form-control" name="CanvasCol" id="toolSingleSelectDropdown_"><option val="0">Single Select Dropdown</option></select></div>'
                   + '</div>'
                 + '</div>';
                break;
            case "toolMultipleSelectCombo":
                ctrlHtml = '<div  class="panel-body toolcontroldiv" questionid="-1" questiontype="MultipleSelectCombo" name="" isnewline="false" ismandatory="false" questionlabel="Select Label" defaultselection="" dropdownvalues=""  id="toolMultipleSelectCombo_' + Clinical_CustomFormsDetails.params.CustomFormId + uniquId + '">'
                  + '<div class="controlContainerDiv">'
                  + '<label id="customFormMultipleSelectComboLabel" class="control-label ellipses size-max100per resetLineHeight" for="TextField" data-toggle="tooltip" title="Select Label">Select Label</label>'
                  + '<div class="pull-right questionAction"><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_copy" title="Copy" name="toolMultipleSelectCombo_' + Clinical_CustomFormsDetails.params.CustomFormId + '"+><i class="fa fa-paste"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_SQGlobally" title="Save Globally" name="toolMultipleSelectCombo_' + Clinical_CustomFormsDetails.params.CustomFormId + '"+><i class="fa fa-paperclip"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnQ_Remove" title="Delete Question" name="Patient_Referrals_Main212"><i class="fa fa-times"></i></a></div>'
                 + '<div id="customFormMultipleSelectCombo_' + uniquId + '"> <select onchange="GlobalQuestionDetail.setMultiSelectValues(this)" class="form-control" name="CanvasCol" id="toolSingleSelectDropdown_"><option val="0">Multiple Select Combo</option></select></div>'
                    + '</div>'
                  + '</div>';
                break;
            case "toolImage":
                ctrlHtml = '<div name="image" class="panel-body toolcontroldiv Of-a" questionid="-1" questiontype="Image" questionlabel="Select Label" filename="" id="toolImage_' + Clinical_CustomFormsDetails.params.CustomFormId + '_' + uniquId + '">'
                 + '<div class="controlContainerDiv">'
                 + '<label id="customFormImageLabel" class="control-label ellipses size-max100per resetLineHeight" for="TextField" data-toggle="tooltip" title="Select Label">Select Label</label>'
                 + '<div class="pull-right questionAction"><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_copy" title="Copy" name="toolImage_' + Clinical_CustomFormsDetails.params.CustomFormId + '"+><i class="fa fa-paste"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_SQGlobally" title="Save Globally" name="toolImage_' + Clinical_CustomFormsDetails.params.CustomFormId + '"+><i class="fa fa-paperclip"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnQ_Remove" title="Delete Question" name="Patient_Referrals_Main212"><i class="fa fa-times"></i></a></div>'
                 + '<div id="customFormImage"> <input class="form-control" name="TextField" id="toolImage_" type="" value="Browse Image"> </div>'
                   + '</div>'
                 + '</div>';
                break;
            case "toolFractionField":
                ctrlHtml = '<div  class="panel-body toolcontroldiv pl-xs pb-xs" name="FractionField" questionid="-1" questionType="FractionField" isnewline="false" ismandatory="false" id="toolFractionField_' + Clinical_CustomFormsDetails.params.CustomFormId + uniquId + '">'
                + '<div class="controlContainerDiv">'
                + '<div class="pull-right questionAction"><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_copy" title="Copy" name="toolFractionField_' + Clinical_CustomFormsDetails.params.CustomFormId + '"+><i class="fa fa-paste"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_SQGlobally" title="Save Globally" name="toolFractionField_' + Clinical_CustomFormsDetails.params.CustomFormId + '"+><i class="fa fa-paperclip"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnQ_Remove" title="Delete Question" name="Patient_Referrals_Main212"><i class="fa fa-times"></i></a></div>'
                         + '<div class="col-xs-12" style="margin-bottom: -8px;">'
                         + '<label class="control-label ellipses size-max100per resetLineHeight" for="TextField" id="lblFractionTitle" data-toggle="tooltip" title="Title">Title</label>'
                         + '</div>'
                         + '<div class="col-xs-4">'
                             + '<label class="control-label" for="txtFractionField1" id="lblFractionField1">Label 1</label>'
                             + '<input onchange="GlobalQuestionDetail.updateTextFieldVal(this);" class="form-control" name="FractionField1" id="txtFractionField1" type="text">'
                         + '</div>'
                     + '<div class="col-xs-2 text-center pad-a-labelsize">/</div>'
                          + '<div class="col-xs-4">'
                             + '<label class="control-label" for="txtFractionField2" id="lblFractionField2">Label 2</label>'
                             + '<input onchange="GlobalQuestionDetail.updateTextFieldVal(this);" class="form-control" name="FractionField2" id="txtFractionField2" type="text">'
                         + '</div>'
                  + '</div>'
                 + '</div>';
                break;
            case "toolHeader":
                ctrlHtml = '<div  class="panel-body toolcontroldiv heightReset" questionType="Header" textcase="" fontSize="" questionid="-1" id="toolHeader_' + Clinical_CustomFormsDetails.params.CustomFormId + '_' + uniquId + '" name="Header">'
                 + '<div class="controlContainerDiv">'
                 + '<label class="control-label ellipsesHeader size-max100per resetLineHeight" for="toolHeader_" id="lblHeader" textcase="" fontSize="" >Add Header</label>'
                 + '<div class="pull-right questionAction"><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_copy" title="Copy" name="toolHeader_' + Clinical_CustomFormsDetails.params.CustomFormId + '"+><i class="fa fa-paste"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_SQGlobally" title="Save Globally" name="toolHeader_' + Clinical_CustomFormsDetails.params.CustomFormId + '"+><i class="fa fa-paperclip"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnQ_Remove" title="Delete Question" name="Patient_Referrals_Main212"><i class="fa fa-times"></i></a></div>'
                 + '</div>'
                 + '</div>';
                break;
            case "toolTable":
                ctrlHtml = '<div  class="panel-body toolcontroldiv heightReset"questionid="-1" id="toolTable_' + Clinical_CustomFormsDetails.params.CustomFormId + '_' + uniquId + '" showCaption="true" name="Table" questionType="Table" tablewidth="" tableheight="" cellspace="" cellPadding="" iscaption="false" align="">'
                 + '<div class="controlContainerDiv">'
                 + '<label class="control-label ellipses size-max100per resetLineHeight" for="TextField" id="lblTabelTitle" data-toggle="tooltip" title="Select Label">Select Label</label>'
                 + '<div class="pull-right questionAction"><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_copy" title="Copy" name="toolTable_' + Clinical_CustomFormsDetails.params.CustomFormId + '_' + uniquId + '"><i class="fa fa-paste"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_SQGlobally" title="Save Globally" name="toolTable_' + Clinical_CustomFormsDetails.params.CustomFormId + '"+><i class="fa fa-paperclip"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnQ_Remove" title="Delete Question" name="Patient_Referrals_Main212"><i class="fa fa-times"></i></a></div>'
                 + '<div class="dropup"> <a class="dropdown-toggle" type="button" data-toggle="dropdown"><span><i class="fa fa-table"></i></span>&nbsp Create Table'
                 + '<span class="caret"></span></a>'
                 //+'<table style="border:1px dashed #BBB;width:100%" id="tblContext"><tr><td style="border:1px dashed #BBB;">&nbsp</td>&nbsp<td style="border:1px dashed #BBB;"></td></tr><tr><td style="border:1px dashed #BBB;">&nbsp</td>&nbsp<td style="border:1px dashed #BBB;">&nbsp</td></tr></table>'
                 //+ '<div id="divContext">'
                 + '<ul class="dropdown-menu multi-level"><li id="contextMenu"></li></ul>'
                 + '<div id="divTable" class="Of-a"></div>'
                 + '</div>'
                 + '</div>'
                 + '</div>';
                break;
            case "toolFreeText":
                ctrlHtml = '<div  class="panel-body toolcontroldiv" questionid="-1" questionType="FreeText" textcase="" maxlength="" id="toolFreeText_' + Clinical_CustomFormsDetails.params.CustomFormId + '_' + uniquId + '" name="FreeText">'
                 + '<div class="controlContainerDiv p-none">'
                 + '<label class="control-label ellipses size-max100per resetLineHeight" for="FreeText" id="lblFreeText" data-toggle="tooltip" title="Text Field">Text Field</label>'
                 + '<div class="pull-right questionAction"><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_copy" title="Copy" name="toolFreeText_' + Clinical_CustomFormsDetails.params.CustomFormId + '_' + uniquId + '"><i class="fa fa-paste"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_SQGlobally" title="Save Globally" name="toolFreeText_' + Clinical_CustomFormsDetails.params.CustomFormId + '_' + uniquId + '"><i class="fa fa-paperclip"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnQ_Remove" title="Delete Question" name="Patient_Referrals_Main212"><i class="fa fa-times"></i></a></div>'
                 + '<textarea onchange="GlobalQuestionDetail.updateTextFieldVal(this);" onkeyup="GlobalQuestionDetail.autoExpandField(this);" id="txtFreeText" name="toolFreeText" spellcheck="true" style="width: 100%;-webkit-box-sizing: border-box; -moz-box-sizing: border-box;box-sizing: border-box; height:35px; ">Add Free Text Here</textarea>'
                 + '</div>'
                 + '</div>';
                break;
            case "toolDateField":
                ctrlHtml = '<div  class="panel-body toolcontroldiv" QuestionType="DateField" questionid="-1" isnewline="false" defaultdate="" ismandatory="false" dateformat="mm/dd/yyyy" id="toolDateField_' + Clinical_CustomFormsDetails.params.CustomFormId + '_' + uniquId + '" name="DateField">'
                 + '<div class="controlContainerDiv">'
                 + '<label class="control-label ellipses size-max100per resetLineHeight" for="DateField" id="lblDateFieldTitle" data-toggle="tooltip" title="Text Field">Text Field</label>'
                 + '<div class="pull-right questionAction"><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_copy" title="Copy" name="toolDateField_' + Clinical_CustomFormsDetails.params.CustomFormId + '"+><i class="fa fa-paste"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_SQGlobally" title="Save Globally" name="toolDateField_' + Clinical_CustomFormsDetails.params.CustomFormId + '_' + uniquId + '"><i class="fa fa-paperclip"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnQ_Remove" title="Delete Question" name="Patient_Referrals_Main212"><i class="fa fa-times"></i></a></div>'
                 + ' <div class="input-group pull-left">'
                 + ' <span class="input-group-addon"> <i class="fa fa-calendar"></i> </span>'
                 + '<input onchange="GlobalQuestionDetail.updateTextFieldVal(this);" id="dtpDateField" name="DateField" class="form-control dateField" type="text" data-plugin-datepicker="" maxlength="10" dateformat="mm/dd/yyyy">'
                 + '</div>'
                 + '</div>';
                break;
            case "toolTimeField":
                ctrlHtml = '<div style="overflow: hidden !important;" class="panel-body toolcontroldiv" QuestionType="TimeField" questionid="-1" isnewline="false" defaulttime=""  ismandatory="false" timeformat="24" id="toolTimeField_' + Clinical_CustomFormsDetails.params.CustomFormId + '_' + uniquId + '" name="TimeField">'
                 + '<div class="controlContainerDiv">'
                 + '<label class="control-label ellipses size-max100per resetLineHeight" for="TimeField" id="lblTimeFieldTitle" data-toggle="tooltip" title="Text Field">Select Label</label>'
                 + '<div class="pull-right questionAction"><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_copy" title="Copy" name="toolTimeField_' + Clinical_CustomFormsDetails.params.CustomFormId + '"+><i class="fa fa-paste"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_SQGlobally" title="Save Globally" name="toolTimeField_' + Clinical_CustomFormsDetails.params.CustomFormId + '"+><i class="fa fa-paperclip"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnQ_Remove" title="Delete Question" name="Patient_Referrals_Main212"><i class="fa fa-times"></i></a></div>'
                 + ' <div class="input-group pull-left">'
                 + ' <span class="input-group-addon"> <i class="fa fa-clock-o"></i> </span>'
                 + '<input onchange="GlobalQuestionDetail.updateTextFieldVal(this);" id="dtpTimeField" name="TimeField" class="form-control timeField" type="text" data-plugin-timepicker maxlength="10" timeformat="">'
                 + '</div>'
                 + '</div>';
                break;
            case "toolProcedures":
                ctrlHtml = '<div  class="panel-body toolcontroldiv" QuestionType="Procedures" questiontitle="Procedures" questionid="-1" ismandatory="false" id="toolProcedures_' + Clinical_CustomFormsDetails.params.CustomFormId + '_' + uniquId + '" name="Procedures">'
                 + '<div class="controlContainerDiv">'
                 + '<label class="control-label ellipses size-max100per resetLineHeight" for="Procedures" id="lblProceduresTitle" data-toggle="tooltip" title="Text Field">Procedures</label>'
                 + '<div class="pull-right questionAction"><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_copy" title="Copy" name="toolProcedures_' + Clinical_CustomFormsDetails.params.CustomFormId + '"+><i class="fa fa-paste"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_SQGlobally" title="Save Globally" name="toolProcedures_' + Clinical_CustomFormsDetails.params.CustomFormId + '"+><i class="fa fa-paperclip"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnQ_Remove" title="Delete Question" name="Patient_Referrals_Main212"><i class="fa fa-times"></i></a></div>'
                 + ' <div class="input-group pull-left">'
                 + '<input class="form-control" id="txtProceduresCustomForm" placeholder="search" type="text" name="ProcedureCPTCode" oninput="Clinical_CustomFormsPreview.bindAutoComplete(this)">'
                 + '<div class="input-group-btn"><button class="btn btn-primary btn-xs " type="button" onclick="Clinical_CustomFormsPreview.openCPTCode(this);"><i class="fa fa-search"></i></button></div>'
                 + '</div>'
                 + '</div>';
                break;
            case "toolProblems":
                ctrlHtml = '<div  class="panel-body toolcontroldiv" QuestionType="Problems" questiontitle="Problems" questionid="-1" ismandatory="false" id="toolProblems_' + Clinical_CustomFormsDetails.params.CustomFormId + '_' + uniquId + '" name="Problems">'
                 + '<div class="controlContainerDiv">'
                 + '<label class="control-label ellipses size-max100per resetLineHeight" for="Problems" id="lblProblemsTitle" data-toggle="tooltip" title="Text Field">Problems</label>'
                 + '<div class="pull-right questionAction"><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_copy" title="Copy" name="toolProblems_' + Clinical_CustomFormsDetails.params.CustomFormId + '"+><i class="fa fa-paste"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_SQGlobally" title="Save Globally" name="toolProblems_' + Clinical_CustomFormsDetails.params.CustomFormId + '"+><i class="fa fa-paperclip"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnQ_Remove" title="Delete Question" name="Patient_Referrals_Main212"><i class="fa fa-times"></i></a></div>'
                 + ' <div class="input-group pull-left">'
                 + '<input class="form-control" data-popupunload="false" id="txtProblemsCustomForm" placeholder="search" oninput="Clinical_CustomFormsPreview.BindICD9AutoComplete(this)" name="ProblemName" type="text" value="" data-bv-field="ProblemName">'
                 + '<div class="input-group-btn"><button class="btn btn-primary btn-xs " type="button" onclick="Clinical_CustomFormsPreview.openICDCode(this);"><i class="fa fa-search"></i></button></div>'
                 + '</div>'
                 + '</div>';
                break;
            case "toolTinyMCEditor":
                ctrlHtml = '<div  class="panel-body toolcontroldiv" QuestionType="TinyMCEditor" questiontitle="Editor" questionid="-1" ismandatory="false" id="toolTinyMCEditor_' + Clinical_CustomFormsDetails.params.CustomFormId + '_' + uniquId + '" name="TinyMCEditor" style="height: 100%;">'
               + '<div class="controlContainerDiv">'
               + '<label class="control-label ellipses size-max100per resetLineHeight" id="lblTinyEditorTitle" data-toggle="tooltip" title="TinyMCEditor">Editor</label>'
               + '<div class="pull-right questionAction"><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_copy" title="Copy" name="toolTinyMCEditor_' + Clinical_CustomFormsDetails.params.CustomFormId + '_' + uniquId + '"><i class="fa fa-paste"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs btn_SQGlobally" title="Save Globally" name="toolTinyMCEditor_' + Clinical_CustomFormsDetails.params.CustomFormId + '_' + uniquId + '"><i class="fa fa-paperclip"></i></a><a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnQ_Remove" title="Delete Question Group"><i class="fa fa-times"></i></a></div>'
               + '<div class="spacer20"></div>'
               + '<div id="customFormTinyMCEditorPreview" ></div>'
               + '</div>'
               + '</div>';
                break;
            default:
                break;

        }
        return ctrlHtml;
    },
    /*Create Contextmenu for Table Creation*/
    createTableForContexMenu: function ($toolcontroldiv) {
        var table = $('<table style="border:1px solid; width:100%"></table>').addClass('contextTable');
        var tdid = 1;
        for (r = 0; r < 50; r++) {
            var $row = $('<tr/>');
            {
                for (c = 0; c < 15; c++) {
                    $row.append('<td style="border:1px solid; height:12px;" id="' + tdid + '" data-col="' + c + '" data-row="' + r + '" ></td>');
                    tdid = tdid + 1;
                }
            }
            table.append($row);
        }
        $toolcontroldiv.find('#contextMenu').append('<div style="max-height:170px; overflow:auto;">' + $(table)[0].outerHTML + '</div>');
        $toolcontroldiv.find('#contextMenu').append('<div style="text-align: center;" id="cellNo"></div>');
    },
    /*Create Table from contextmenu*/
    CreateTable: function (cols, rows, $controlDivlId) {
        $controlDivlId.find("#divTable").find('#tblContext').remove();
        var y, x, html;
        html = '<table width="300" border="1" style="border-collapse: separate;" id="tblContext" contenteditable="true"><tbody>';
        for (y = 0; y <= rows; y++) {
            html += '<tr>';
            for (x = 0; x <= cols; x++) {
                html += '<td contenteditable="true" ></td>';
            }
            html += '</tr>';
        }
        html += '</tbody></table>';
        $controlDivlId.find("#divTable").append(html);
        $controlDivlId.find('.dropup .dropdown-toggle').remove();
        $controlDivlId.find('.dropup .dropdown-menu').remove();
        if (cols) {
            var totalCols = (parseInt(cols) + 1);
            var widthofTd = Math.floor(100 / totalCols);
            $controlDivlId.find(".tblContextTd").css({ 'width': widthofTd + '%' });
        }
        //Clinical_CustomFormsDetails.updateHeigtWidthOfWdgt();
        //Clinical_CustomFormsDetails.rebuildGridster();
        Clinical_CustomFormsDetails.customFormHTMLHoverEvent();
        Clinical_CustomFormsDetails.updateCustomeFormHTML();
    },
    /*Mouseover highlighting cell in contextmenu*/
    SelectTable: function (id, col, $controlDivlId) {
        $controlDivlId.find(".contextTable tr").each(function () { //get all rows in table
            $("td", this).each(function () { //get all tds in current row
                if (parseInt($(this).attr('id')) <= id && parseInt($(this).attr('data-col')) <= col) {
                    $(this).addClass('tdcontextTable');
                    var column_num = parseInt($(this).index()) + 1;
                    var row_num = parseInt($(this).parent().index()) + 1;
                    $('#cellNo').text(column_num + 'X' + row_num);
                }
                else {
                    $(this).removeClass('tdcontextTable');
                }
            });
        });
    },
    updateCustomeFormHTML: function () {
        var strMessage = "";
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if (Clinical_CustomFormsDetails.params.mode == "Edit") {
            var self = $("#" + Clinical_CustomFormsDetails.params["PanelID"] + " #frmCustomFormsDetails");
            var myJSON = self.getMyJSONByName();
            Clinical_CustomFormsDetails.UpdateCustomFormsDetails(myJSON).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }

    },
    removeDivUiResizeableHandle: function () {
        $("#" + Clinical_CustomFormsDetails.params.PanelID + " #frmCustomFormsDetails #customFormDetails li").each(function () {
            $.each($(this).find('.toolcontroldiv'), function () {
                if ($(this).nextAll("div.ui-resizable-handle.ui-resizable-e").length > 1)
                    $(this).nextAll('div.ui-resizable-handle.ui-resizable-e').not(':first').remove();
            });
        });
    },
    InitializeDragableGroup: function () {
        $("#" + Clinical_CustomFormsDetails.params.PanelID + " #frmCustomFormsDetails").find("ul.questionGroupUl").each(function () {
            var toolQuestionGroupId = $(this);
            if (toolQuestionGroupId) {
                var canvasCol = $(toolQuestionGroupId).attr('CanvasCol');
                if (!canvasCol)
                    canvasCol = $("#" + Clinical_CustomFormsDetails.params.PanelID + " #frmCustomFormsDetails #ddlCanvasCol option:selected").val();
                $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #frmCustomFormsDetails #toolbarContainer li').draggable({
                    revert: "invalid",
                    appendTo: "#pnlClinicalCustomFormsDetails #customFormDetails",
                    containment: '.dragableCF',
                    connectToSortable: '.dragableCF',
                    //containment: "#pnlClinicalCustomFormsDetails #customFormDetails",
                    stack: "#pnlClinicalCustomFormsDetails #customFormDetails",
                    connectWith: '.dragableCF',
                    helper: function (ev, ui) {
                        //var bootStrpCss = 'col-xs-12';
                        //switch (canvasCol) {
                        //    case "1":
                        //        bootStrpCss = 'col-xs-12'
                        //        break;
                        //    case "2":
                        //        bootStrpCss = 'col-xs-12 col-sm-6'
                        //        break;
                        //    case "3":
                        //        bootStrpCss = 'col-xs-12  col-sm-4'
                        //        break;
                        //    default:
                        //        bootStrpCss = 'col-xs-12'
                        //        break;

                        //}
                        var toolId = $(this).attr('id');
                        var ctrlHtml = Clinical_CustomFormsDetails.getDefaultHTMLOfControl(toolId);
                        var lstToAppend = '';
                        if (toolId == "toolTable" || toolId == "toolHeader" || toolId == "toolTinyMCEditor") {
                            lstToAppend = '<li class="resizable draggable col-xs-12 mb-sm">' + ctrlHtml + '</li>';
                        }
                        else if (toolId == "toolQuestionGroup") {
                            lstToAppend = '<li class="resizable draggable col-xs-12 mb-sm">' + ctrlHtml + '</li>';
                            Clinical_CustomFormsDetails.InitializeDragableGroup();//($(ctrlHtml).attr('id'));
                        }
                        else {
                            lstToAppend = '<li class="resizable draggable mb-sm">' + ctrlHtml + '</li>';
                        }
                        return lstToAppend;

                    },
                    stop: function (ev, ui) {
                        $(ui.helper).removeAttr('style');
                        var toolId = $(this).attr('id');
                        var canvasCol = '';
                        if ($(ui.helper).parent() && $(ui.helper).parent().attr('id').indexOf('toolQuestionGroup') != -1) {
                            canvasCol = ui.helper.parent().attr('canvascol');
                        }
                        else {
                            canvasCol = $('#ddlCanvasCol').val()
                        }
                        var bootStrpCss = 'col-xs-12';
                        switch (canvasCol) {
                            case "1":
                                bootStrpCss = 'col-xs-12'
                                break;
                            case "2":
                                bootStrpCss = 'col-xs-12 col-sm-6'
                                break;
                            case "3":
                                bootStrpCss = 'col-xs-12  col-sm-4'
                                break;
                            case "4":
                                bootStrpCss = 'col-xs-12  col-sm-3'
                                break;
                            case "6":
                                bootStrpCss = 'col-xs-12  col-sm-2'
                                break;
                            default:
                                bootStrpCss = 'col-xs-12'
                                break;

                        }
                        if (toolId == "toolTable" || toolId == "toolHeader" || toolId == "toolQuestionGroup" || toolId == "toolTinyMCEditor") {
                            if (!$(ui.helper).hasClass("col-xs-12"))
                                $(ui.helper).addClass("col-xs-12");
                        }
                        else if (toolId == "toolYesNo") {
                            (ui.helper).addClass("col-xs-12 col-sm-3");
                        }
                        else if (toolId == "toolFractionField") {
                            (ui.helper).addClass("col-xs-12 col-sm-4");
                        }
                        else
                            $(ui.helper).addClass(bootStrpCss);
                        if (toolId == "toolTable") {
                            Clinical_CustomFormsDetails.createTableForContexMenu($(ui.helper).find('.toolcontroldiv'));
                            $(ui.helper).find('.toolcontroldiv').find(".contextTable tr td").click(function (event) {
                                Clinical_CustomFormsDetails.CreateTable($(this).attr('data-col'), $(this).attr('data-row'), $(ui.helper).find('.toolcontroldiv'));
                            });

                            $(ui.helper).find('.toolcontroldiv').find(".contextTable tr td").hover(function (event) {
                                Clinical_CustomFormsDetails.SelectTable(parseInt($(this).attr('id')), $(this).attr('data-col'), $(ui.helper).find('.toolcontroldiv'));
                            });
                        }
                        setTimeout(function () {
                            if (toolId != "toolTable") {
                                Clinical_CustomFormsDetails.updateCustomeFormHTML();
                                Clinical_CustomFormsDetails.customFormHTMLHoverEvent();
                            }
                            else if (toolId == "toolTable") {
                                $(ui.helper).find('.questionAction .btnQ_Remove').unbind("click");
                                var Removehandler = function (e) {
                                    Clinical_CustomFormsDetails.RemoveQuestion(this, e);
                                };
                                $(ui.helper).find('.questionAction .btnQ_Remove').bind("click", Removehandler);
                            }
                            Clinical_CustomFormsDetails.InitilizeSortable();
                            Clinical_CustomFormsDetails.InitializeResizable();
                        }, 300);
                        Clinical_CustomFormsDetails.removeDivUiResizeableHandle();
                    },

                });
            }
        });
    },
    /*Initialize Gridster(Question to be drageable with in editing area)*/
    initilizeGridsterGroup: function (obj) {
        if (!obj)
            obj = $("#pnlClinicalCustomFormsDetails #toolQuestionGroupHTML");
        var canvasCol = $("#" + Clinical_CustomFormsDetails.params.PanelID + " #frmCustomFormsDetails #ddlCanvasCol option:selected").val();

        var gridster = $(obj).gridster({
            widget_base_dimensions: [canvasCol == 1 ? 750 : canvasCol == 2 ? 370 : canvasCol == 3 ? 240 : 240, 80],
            widget_margins: [5, 5],
            shift_widgets_up: true,
            max_cols: parseInt(canvasCol),
            shift_larger_widgets_down: false,
            //autogenerate_stylesheet: false,
            //widget_selector: '.dragableCF',
            collision: {
                wait_for_mouseup: false
            },
            avoid_overlapped_widgets: true,
            serialize_params: function ($w, wgd) {
                var widget;
                widget = (typeof wgd === 'undefined') ? $($w).data() : wgd;
                widget_object = {
                    id: $($w).data('id'),
                    col: widget.col,
                    row: widget.row,
                    size_x: widget.size_x,
                    size_y: widget.size_y,
                    html: $w.html(),
                }
                return widget_object;
            },
            draggable:
                {
                    //items: ".drageme",
                    widget_selector: '.dragableCF',
                    start: function (e, ui, $widget) {
                        //$('#' + Clinical_CustomFormsDetails.params["PanelID"] + ' .toolcontroldiv').unbind('mouseenter,mouseleave');
                    },
                    stop: function (e, ui, $widget) {
                        //var newDimensions = this.serialize($widget)[0];
                        //  Clinical_CustomFormsDetails.customFormHTMLHoverEvent();
                    },
                    drag: function (e, ui, $widget) {
                        // Clinical_CustomFormsDetails.customFormHTMLHoverEvent();
                        Clinical_CustomFormsDetails.isDragged = true;
                    }
                },
            resize: {
                enabled: false,
            },
            //generate_stylesheet: function (opts) {
            //    var styles = '';
            //    var max_size_x = this.options.max_size_x || this.cols;
            //    var max_rows = 0;
            //    var max_cols = 0;
            //    var i;
            //    var rules;
            //    opts = opts || (opts = {});
            //    opts.cols = opts.cols || (opts.cols = this.cols);
            //    opts.rows = opts.rows || (opts.rows = this.rows);
            //    opts.namespace = opts.namespace || (opts.namespace = this.options.namespace);
            //    opts.widget_base_dimensions = opts.widget_base_dimensions ||
            //            (opts.widget_base_dimensions = this.options.widget_base_dimensions);
            //    opts.widget_margins = opts.widget_margins ||
            //            (opts.widget_margins = this.options.widget_margins);
            //    opts.min_widget_width = (opts.widget_margins[0] * 2) +
            //            opts.widget_base_dimensions[0];
            //    opts.min_widget_height = (opts.widget_margins[1] * 2) +
            //            opts.widget_base_dimensions[1];

            //    // don't duplicate stylesheets for the same configuration
            //    var serialized_opts = $.param(opts);
            //    if ($.inArray(serialized_opts, Gridster.generated_stylesheets) >= 0) {
            //        return false;
            //    }

            //    this.generated_stylesheets.push(serialized_opts);
            //    Gridster.generated_stylesheets.push(serialized_opts);

            //    /* generate CSS styles for cols */
            //    for (i = opts.cols; i > 0; i--) {
            //        styles += (opts.namespace + ' [data-col="' + (i + 1) + '"] { left:' +
            //                ('calc(' + i * (100 / opts.cols) + '% + ' + opts.widget_margins[0]) + 'px); }\n');
            //    }
            //    styles += (opts.namespace + ' [data-col="1"] { left:' + opts.widget_margins[0] + 'px; }\n');

            //    /* generate CSS styles for rows */
            //    for (i = opts.rows; i >= 0; i--) {
            //        styles += (opts.namespace + ' [data-row="' + (i + 1) + '"] { top:' +
            //                ((i * opts.widget_base_dimensions[1]) +
            //                (i * opts.widget_margins[1]) +
            //                ((i + 1) * opts.widget_margins[1])) + 'px; }\n');
            //    }
            //    for (var y = 1; y <= opts.rows; y++) {
            //        styles += (opts.namespace + ' [data-sizey="' + y + '"] { height:' +
            //                (y * opts.widget_base_dimensions[1] +
            //                (y - 1) * (opts.widget_margins[1] * 2)) + 'px; }\n');
            //    }
            //    for (var x = 1; x <= max_size_x; x++) {
            //        styles += (opts.namespace + ' [data-sizex="' + x + '"] { width:' +
            //                'calc(' + x * (100 / opts.cols) + '% - ' + (opts.widget_margins[0] * 2) + 'px); }\n');
            //    }
            //    this.remove_style_tags();
            //    return this.add_style_tag(styles);
            //},
            //set_dom_grid_width: function (cols) {
            //    if (typeof cols === 'undefined') {
            //        cols = this.get_highest_occupied_cell().col;
            //    }
            //    var max_cols = (this.options.autogrow_cols ? this.options.max_cols :
            //    this.cols);

            //    cols = Math.min(max_cols, Math.max(cols, this.options.min_cols));
            //    this.container_width = cols * this.min_widget_width;
            //    this.$el.css('width', "100%");
            //    return this;
            //},
        }).data('gridster');
        $(obj).gridster().width("100%");
    },
    InitilizeSortable: function () {
        var canvasCol = $("#" + Clinical_CustomFormsDetails.params.PanelID + " #frmCustomFormsDetails #ddlCanvasCol option:selected").val();
        switch (canvasCol) {
            case "1":
                bootStrpCss = 'col-xs-12'
                break;
            case "2":
                bootStrpCss = 'col-xs-12 col-sm-6'
                break;
            case "3":
                bootStrpCss = 'col-xs-12  col-sm-4'
                break;
            case "4":
                bootStrpCss = 'col-xs-12  col-sm-3'
                break;
            case "6":
                bootStrpCss = 'col-xs-12  col-sm-2'
                break;
            default:
                bootStrpCss = 'col-xs-12'
                break;

        }
        $(".dragableCF").sortable({
            connectWith: ".dragableCF",
            placeholder: "dragArea " + bootStrpCss,
            start: function (e, ui) {
                ui.placeholder.width(ui.item.width());
                ui.placeholder.height(ui.item.height());
            },
        });
    },
    //This functions removed Component from Progress Note HTML
    RemoveComponentTab: function (ComponentName, ComponentId, NotesId) {
        utility.myConfirm('1', function () {
        }, function () { },
            '1'
        );

    },
    bindQuestionEvents: function () {
        $('#pnlClinicalCustomFormsDetails .toolcontroldiv').unbind("click");
        var EditQuestionhandler = function (e) {
            Clinical_CustomFormsDetails.EditQuestion(this, e);
        };
        $('#pnlClinicalCustomFormsDetails .toolcontroldiv').bind("click", EditQuestionhandler);

        //  $('#pnlClinicalCustomFormsDetails .toolQuestionGroup').unbind('click').click(function (e) {
        // Clinical_CustomFormsDetails.EditQuestionQroup(this, e);
        //});

        $('#pnlClinicalCustomFormsDetails .btnQ_Remove').unbind("click");
        var Removehandler = function (e) {
            Clinical_CustomFormsDetails.RemoveQuestion(this, e);
        };

        $('#pnlClinicalCustomFormsDetails .btnQ_Remove').bind("click", Removehandler);
        $('#pnlClinicalCustomFormsDetails .btn_copy').unbind("click");
        var Copyhandler = function (e) {
            Clinical_CustomFormsDetails.CopyQuestion(this, e);
        };
        $('#pnlClinicalCustomFormsDetails .btn_copy').bind("click", Copyhandler);
        $('#pnlClinicalCustomFormsDetails .btn_SQGlobally').unbind("click");
        var savehandler = function (e) {
            Clinical_CustomFormsDetails.SaveQuestion(this, e);
        };
        $('#pnlClinicalCustomFormsDetails .btn_SQGlobally').bind("click", savehandler);
    },
    //This Functions Removed Question
    RemoveQuestion: function (obj, event) {
        if (event != null)
            event.stopPropagation();
        utility.myConfirm('1', function () {
            if (obj) {
                var containerWdgt = $(obj).closest('li');
                var questionId = -1;
                var parentul = $(containerWdgt).parent('ul');
                var gridster = $(parentul).data('gridster');
                //  var questionHtm = $(containerWdgt).html().trim();
                //    if ($(questionHtm).attr("questionid"))
                //    questionId = parseInt($(questionHtm).attr("questionid"));
                /* if (questionId && questionId > 0) {
                     Clinical_CustomFormsDetails.SaveGlobalQuestion(questionId).done(function (response) {
                         response = JSON.parse(response);
                         if (response.status != false) {
                             utility.DisplayMessages(response.Message, 1);
                             if (gridster) {
                                 gridster.remove_widget($(containerWdgt));
                             } else
                                 $(containerWdgt).remove();
                             Clinical_CustomFormsDetails.updateCustomeFormHTML();
                         }
                         else {
                             utility.DisplayMessages(response.Message, 3);
                         }
                     });
                 }
                 else {*/
                if (gridster) {
                    gridster.remove_widget($(containerWdgt));
                    $(containerWdgt).remove();
                    Clinical_CustomFormsDetails.updateCustomeFormHTML();
                } else {
                    $(containerWdgt).remove();
                    Clinical_CustomFormsDetails.updateCustomeFormHTML();
                }
                // }
            }
        },
        function () { },
        '1'
        );
    },
    //This Functions edited Question
    EditQuestion: function (obj, event, selectedLi) {
        if (event != null) {
            //Prevent click event in case of toogle header/dropdown menu/context menu clicks
            if ($(event.target).attr('id') == "lblQuestionGroupTitle" || $(event.target).hasClass('toggleButton') || $(event.target).hasClass('toggleEditableHeader') || $(event.target).hasClass('dropdown-toggle') || $(event.target).hasClass('tdcontextTable')) {
                event.preventDefault();
                if (!$(event.target).hasClass('tdcontextTable'))
                    return false;
            }
            else {
                event.stopPropagation();
            }
        }
        //To avoid double clicks on Questions I am turning off the click event for one second after first click - ZeeshanAK for EMR-3040
        $(obj).unbind("click");
        setTimeout(function () {
            var EditQuestionhandler = function (e) {
                e.stopPropagation();
                Clinical_CustomFormsDetails.EditQuestion(this, e);
            };
            $(obj).bind("click", EditQuestionhandler);
        }, 1000);

        // if (obj && Clinical_CustomFormsDetails.isDragged == false) {
        var toolcontroldiv;
        var questionType;
        var questionId;
        var QuestionGroupId;
        if (selectedLi) {

            toolcontroldiv;
            questionType = $(selectedLi.find('input').val()).attr('questiontype');
            questionId = $(selectedLi.find('input').val()).attr('id');
            QuestionGroupId = $(selectedLi.find('input').val()).attr('QuestionGroupId');
            Clinical_CustomFormsDetails.QuestionEdit(questionType, questionId, QuestionGroupId, true);
        }
        else {
            toolcontroldiv = $(obj).closest('div.toolcontroldiv');
            questionType = $(toolcontroldiv).attr('questionType');
            questionId = $(toolcontroldiv).attr('id');
            QuestionGroupId = $(toolcontroldiv).attr('QuestionGroupId');
            Clinical_CustomFormsDetails.QuestionEdit(questionType, questionId, QuestionGroupId, false);
        }





        //$(containerWdgt).remove();
        //Clinical_CustomFormsDetails.updateCustomeFormHTML();
        //}
        //Clinical_CustomFormsDetails.isDragged = false;
    },
    //EditQuestionQroup: function (obj, event) {
    //    if (event != null)
    //        event.stopPropagation();
    //    // if (obj && Clinical_CustomFormsDetails.isDragged == false) {
    //    var toolcontroldiv = $(obj);//.closest('div.toolcontroldiv');
    //    var questionType = $(toolcontroldiv).attr('questionType');
    //    var questionId = $(toolcontroldiv).attr('id');
    //    var QuestionGroupId = $(toolcontroldiv).attr('QuestionGroupId');
    //    Clinical_CustomFormsDetails.QuestionEdit(questionType, questionId, QuestionGroupId);
    //    //$(containerWdgt).remove();
    //    //Clinical_CustomFormsDetails.updateCustomeFormHTML();
    //    //}
    //    //Clinical_CustomFormsDetails.isDragged = false;
    //},
    CopyQuestion: function (obj, event) {
        if (event != null)
            event.stopPropagation();
        if (obj) {
            var canvasCol = $("#" + Clinical_CustomFormsDetails.params.PanelID + " #frmCustomFormsDetails #ddlCanvasCol option:selected").val();

            var containerWdgt = $(obj).closest('li');
            var uniquId = utility.makeRendomKey();
            var content = $(containerWdgt).html();
            content = $(content).attr("id", $(content).attr("id") + uniquId)[0].outerHTML;
            var gridster = $(obj).closest('ul');//$("#" + Clinical_CustomFormsDetails.params.PanelID + " #frmCustomFormsDetails #customFormDetails")//.data('gridster');
            //var col = Clinical_CustomFormsDetails.getPrevwidgetcol();
            //var row = Clinical_CustomFormsDetails.getPrevwidgetrow();
            if ($(gridster).attr('canvasCol')) {
                canvasCol = $(gridster).attr('canvasCol');
            }
            switch (canvasCol) {
                case "1":
                    bootStrpCss = 'col-xs-12'
                    break;
                case "2":
                    bootStrpCss = 'col-xs-12 col-sm-6'
                    break;
                case "3":
                    bootStrpCss = 'col-xs-12  col-sm-4'
                    break;
                case "4":
                    bootStrpCss = 'col-xs-12  col-sm-3'
                    break;
                case "6":
                    bootStrpCss = 'col-xs-12  col-sm-2'
                    break;
                default:
                    bootStrpCss = 'col-xs-12'
                    break;

            }
            var questionType = $(content).attr('questionType');
            var $contentObj = $(content);
            if (questionType == "YesNo" || questionType == "CheckBox") {
                var randKey = utility.makeRendomKey();
                $.each($contentObj.find("input[type=checkbox]"), function (i, item) {
                    var id = $(item).attr("id").split("_");
                    if (id) {
                        if (questionType == "YesNo") {
                            id = id[0] + "_" + randKey
                        }
                        else {
                            id = id[0] + "_" + utility.makeRendomKey();
                        }
                        $(item).parent().find("label").attr("for", id);
                        $(item).parent().find("label").attr("id", id);
                        $(item).attr("id", id)
                    }
                })
                content = $contentObj[0].outerHTML;
            }
            if (questionType == "Header" || questionType == "QuestionGroup" || questionType == "Table") {
                gridster.append('<li class="resizable draggable col-xs-12 mb-sm">' + content + '</li>');
            }
            else {
                gridster.append('<li class="resizable draggable ' + bootStrpCss + ' mb-sm">' + content + '</li>');

            }
            //Clinical_CustomFormsDetails.updateHeigtWidthOfWdgt();
            Clinical_CustomFormsDetails.InitializeResizable();
            setTimeout(function () {
                //  Clinical_CustomFormsDetails.rebuildGridster();
                Clinical_CustomFormsDetails.customFormHTMLHoverEvent();
                Clinical_CustomFormsDetails.updateCustomeFormHTML();
            }, 300)
        }
    },
    SaveQuestion: function (obj, event, question) {
        if (event != null)
            event.stopPropagation();
        if (obj || question) {
            var containerWdgt = $(obj).closest('li');
            var toolcontroldiv = $(obj).closest('div.toolcontroldiv');
            if (question) {
                containerWdgt = question;
                toolcontroldiv = $(containerWdgt.find('input').val());
            }

            var questionType = $(toolcontroldiv).attr('questionType');
            var questionId = $(toolcontroldiv).attr('id');
            var QuestionGroupId = $(toolcontroldiv).attr('QuestionGroupId');
            if (QuestionGroupId) {
                if (QuestionGroupId && QuestionGroupId == "-1") {
                    Clinical_GlobalQuestionGroup.params.QuestionGroupId = QuestionGroupId;
                    var params = [];
                    params["UniqueQuestionId"] = questionId;
                    params["mode"] = 'Add';
                    params["FromAdmin"] = Clinical_CustomFormsDetails.params["FromAdmin"];
                    if (Clinical_CustomFormsDetails.params["FromAdmin"] == "0") {
                        params["ParentCtrl"] = 'Clinical_CustomFormsDetails';
                    }
                    LoadActionPan('Clinical_GlobalQuestionGroup', params);
                    //Clinical_GlobalQuestionGroup.params.UniqueQuestionId = questionId;
                    //var objData = {};
                    //objData["QuestionGroupTitle"] = $(toolcontroldiv).find("#lblQuestionGroupTitle").text();
                    //objData["QuestionGroupName"] = $(toolcontroldiv).attr('QuestionGroupName');
                    //objData["IsActive"] = $(toolcontroldiv).attr('isactive');
                    //objData["CanvasCols"] = $(toolcontroldiv).attr('CanvasCols');
                    //objData["QuestionGroupHTML"] = $(toolcontroldiv).parent().html().trim();
                    //objData["SaveGlobally"] = true;
                    //var JSONdata = JSON.stringify(objData);
                    //Clinical_GlobalQuestionGroup.SaveGlobalQuestionGroup(JSONdata).done(function (response) {
                    //    response = JSON.parse(response);
                    //    if (response.status != false) {
                    //        var customFormQuestionGroup = $('#' + questionId);
                    //        $(customFormQuestionGroup).attr('QuestionGroupId', response.QuestionGroupId);
                    //        utility.DisplayMessages(response.Message, 1);
                    //    }
                    //    else {
                    //        utility.DisplayMessages(response.Message, 3);
                    //    }
                    //});
                }
                else if (QuestionGroupId && QuestionGroupId != "-1") {
                    Clinical_GlobalQuestionGroup.params.QuestionGroupId = QuestionGroupId;
                    var params = [];
                    params["QuestionGroupId"] = QuestionGroupId;
                    params["UniqueQuestionId"] = questionId;
                    params["mode"] = 'Edit';
                    params["FromAdmin"] = Clinical_CustomFormsDetails.params["FromAdmin"];
                    if (Clinical_CustomFormsDetails.params["FromAdmin"] == "0") {
                        params["ParentCtrl"] = 'Clinical_CustomFormsDetails';
                    }
                    LoadActionPan('Clinical_GlobalQuestionGroup', params);
                    //Clinical_GlobalQuestionGroup.UpdateGlobalQuestionGroup(QuestionGroupId, questionId).done(function (response) {
                    //    response = JSON.parse(response);
                    //    if (response.status != false) {
                    //        var customFormQuestionGroup = $('#' + questionId);

                    //        $(customFormQuestionGroup).attr('QuestionGroupId', response.QuestionGroupId);
                    //        utility.DisplayMessages(response.Message, 1);
                    //    }
                    //    else {
                    //        utility.DisplayMessages(response.Message, 3);
                    //    }
                    //});
                }
            }
            else {
                Clinical_CustomFormsDetails.SaveGlobalQuestion(containerWdgt).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        Clinical_GlobalQuestionGroup.fillGlobalQuestions();
                        utility.DisplayMessages(response.Message, 1);
                        if (response.QuestionId) {
                            $($(obj).closest('div.toolcontroldiv')).attr("questionid", response.QuestionId)
                        }
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }

        }
    },
    customFormHTMLHoverEvent: function () {
        $('#' + Clinical_CustomFormsDetails.params["PanelID"] + ' .toolcontroldiv').unbind('mouseenter,mouseleave');
        $('#' + Clinical_CustomFormsDetails.params["PanelID"] + ' .toolcontroldiv').bind("mouseenter", function (e) {
            $(this).find('div.questionAction').removeClass('hidden');
            //$(this).children('.copyBtn').removeClass('hidden');
            //  $(this).children('.sqgBtn').removeClass('hidden');
            $(this).find('.closeBtn').removeClass('hidden');
            $(this).css('background', '#EAF1F8');
        });
        $('#' + Clinical_CustomFormsDetails.params["PanelID"] + ' .toolcontroldiv').bind("mouseleave", function (e) {
            $(this).find('div.questionAction').addClass('hidden');
            // $(this).children('.copyBtn').addClass('hidden');
            // $(this).children('.sqgBtn').removeClass('hidden');
            // $(this).find('.closeBtn').addClass('hidden');
            $(this).css('background', '#fff');
        });
        Clinical_CustomFormsDetails.bindQuestionEvents();
    },
    QuestionEdit: function (questionType, questionId, QuestionGroupId, fromQuestionGroup) {
        if (questionType == "TextField" || questionType == "YesNo" || questionType == "FractionField" || questionType == "DateField" || questionType == "Header" || questionType == "TimeField" || questionType == "CheckBox" || questionType == "Toggle" || questionType == "FreeText" || questionType == "SingleSelectDropdown" || questionType == "Table" || questionType == "MultipleSelectCombo" || questionType == "Image" || questionType == "Procedures" || questionType == "Problems" || questionType == "TinyMCEditor") {
            var params = [];
            params["QuestionType"] = questionType;
            params["QuestionID"] = questionId;
            params["mode"] = 'Edit';
            params["FromAdmin"] = Clinical_CustomFormsDetails.params["FromAdmin"];
            params["FromQuestionGroup"] = fromQuestionGroup;
            params["ParentCtrl"] = 'Clinical_CustomFormsDetails';

            if (questionType == "Table") {
                var table = $("#" + questionId).find('#tblContext');
                if (table && table.find('tr').length > 0) {
                    params["TableRows"] = table.find('tr').length;
                    params["TableCols"] = $(table.find('tr:first-child')).find('td').length;
                    LoadActionPan('GlobalQuestionDetail', params);
                }
            }
            else {
                LoadActionPan('GlobalQuestionDetail', params);
            }

        }
        else if (questionType == "QuestionGroup" && QuestionGroupId) {
            if (QuestionGroupId == "-1") {
                var params = [];
                params["QuestionGroupId"] = QuestionGroupId;
                params["UniqueQuestionId"] = questionId;
                params["mode"] = 'Add';
                params["FromAdmin"] = Clinical_CustomFormsDetails.params["FromAdmin"];
                params["ParentCtrl"] = 'Clinical_CustomFormsDetails';
                LoadActionPan('Clinical_GlobalQuestionGroup', params);
            }
            else {
                var params = [];
                params["QuestionGroupId"] = QuestionGroupId;
                params["UniqueQuestionId"] = questionId;
                params["mode"] = 'Edit';
                params["FromAdmin"] = Clinical_CustomFormsDetails.params["FromAdmin"];
                params["ParentCtrl"] = 'Clinical_CustomFormsDetails';
                LoadActionPan('Clinical_GlobalQuestionGroup', params);
                //Clinical_GlobalQuestionGroup.UpdateGlobalQuestionGroup(QuestionGroupId, questionId).done(function (response) {
                //    response = JSON.parse(response);
                //    if (response.status != false) {
                //        var customFormQuestionGroup = $('#' + questionId);
                //        $(customFormQuestionGroup).find("#lblQuestionGroupTitle").text(self.find('#txtGroupTitile').val());
                //        $(customFormQuestionGroup).attr('QuestionGroupId', response.QuestionGroupId);
                //        $(customFormQuestionGroup).attr('QuestionGroupName', self.find('#txtGroupName').val());
                //        $(customFormQuestionGroup).attr('saveglobally', self.find("#chkSaveGlobally").is(':checked'));
                //        $(customFormQuestionGroup).attr('IsActive', self.find("#chkIsActive").is(':checked'));
                //        $(customFormQuestionGroup).attr('QuestionGroupTitle', self.find('#txtGroupTitile').val());
                //        $(customFormQuestionGroup).attr('CanvasCols', self.find('#ddlCanvasCol option:selected').val());
                //        utility.DisplayMessages(response.Message, 1);
                //    }
                //    else {
                //        utility.DisplayMessages(response.Message, 3);
                //    }
                //});
            }
        }
    },
    SaveGlobalQuestion: function (containerWdgt) {
        var questionHtml = '';
        if (containerWdgt) {
            questionHtm = $(containerWdgt).html().trim();
        }
        //if (GlobalQuestionDetail.params["FromQuestionGroup"]) {
        //    questionHtml = $(containerWdgt.find('input').val());
        //}
        var objData = new Object();
        objData["QuestionName"] = $(questionHtm).attr("name");
        objData["QuestionHTML"] = questionHtm;
        objData["IsActive"] = "true";
        objData["commandType"] = "SAVE_GLOBAL_QUESTION";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CustomForm", "GlobalQuestion");
    },
    DeleteGlobalQuestion: function (questionId) {
        if (questionId && questionId > 0) {
            var objData = new Object();
            objData["QuestionId"] = questionId;
            objData["commandType"] = "DELETE_GLOBAL_QUESTION";
            var data = JSON.stringify(objData);
            return MDVisionService.APIService(data, "CustomForm", "GlobalQuestion");
        }

    },
    editQuestionGroupTitle: function (obj, event) {
        if (event != null) {
            event.stopPropagation();
        }
        $('#lblQuestionGroupTitle').addClass('hidden');
        $('#lblQuestionGroupTitle').siblings(".questionGroupTitle").val($('#lblQuestionGroupTitle').text()).focus();
        $(".questionGroupTitle").removeClass('hidden');
        $(obj).addClass('hidden');
        $(".questionGroupTitle").focus();
        $(".questionGroupTitle").focusout(function () {
            if (!($(this).hasClass("hidden")) && $(this).val() != "") {
                $("#lblQuestionGroupTitle").removeClass('hidden');
                $(this).addClass('hidden');
                $(this).siblings("#lblQuestionGroupTitle").text($(this).val());
                $('#lnkQuestionGroupTitle').removeClass('hidden');
            }
        });
    },
}