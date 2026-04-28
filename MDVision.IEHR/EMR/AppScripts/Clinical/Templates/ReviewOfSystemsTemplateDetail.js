ReviewOfSystemsTemplateDetail = {
    bIsFirstLoad: true,
    params: [],
    CharacteristicsInfo: [],
    specialityCheckedIds: [],
    providerCheckedIds: [],
    SpecialtyIds: '',
    ProviderIds: '',
    numOfCharsChecked: 0,

    Load: function (params) {

        ReviewOfSystemsTemplateDetail.params = params;
        if (ReviewOfSystemsTemplateDetail.params.PanelID != 'ReviewOfSystemsTemplateDetail') {
            ReviewOfSystemsTemplateDetail.params.PanelID = ReviewOfSystemsTemplateDetail.params.PanelID + ' #ReviewOfSystemsTemplateDetail';
        }

        ReviewOfSystemsTemplateDetail.CharacteristicsInfo = [];
        ReviewOfSystemsTemplateDetail.specialityCheckedIds = [];
        ReviewOfSystemsTemplateDetail.providerCheckedIds = [];

        if (ReviewOfSystemsTemplateDetail.bIsFirstLoad) {

            var self = $('#' + ReviewOfSystemsTemplateDetail.params.PanelID);
            var isSuperAdmin = ReviewOfSystemsTemplateDetail.isSuperAdmin();
            if (isSuperAdmin) {
                self = $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #entityDDL');
            }

            self.loadDropDowns(true).done(function () {

                if (ReviewOfSystemsTemplateDetail.params["mode"] = "Edit" && ReviewOfSystemsTemplateDetail.params.ROSTemplateId > 0) {
                    ReviewOfSystemsTemplateDetail.rosTemplateLoad(ReviewOfSystemsTemplateDetail.params.ROSTemplateId);
                    ReviewOfSystemsTemplateDetail.loadSystems(true);
                    $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #btnSaveAs').show();
                    $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' .modal-title').first().html("Edit ROS Template");
                } else {
                    $.when(ReviewOfSystemsTemplateDetail.isEntitySelected(isSuperAdmin)).then(function () {
                        ReviewOfSystemsTemplateDetail.loadSystems(false);
                        $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #btnSaveAs').hide();
                    });
                }

            });

            ReviewOfSystemsTemplateDetail.domReadyFunction();
        }
    },

    domReadyFunction: function () {

        ReviewOfSystemsTemplateDetail.enableDisableDropDownLists('ddlSpecialty,ddlProvider', ReviewOfSystemsTemplateDetail.isSuperAdmin());
        ReviewOfSystemsTemplateDetail.IntializeMultiSelectDropDown();
        $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #frmROSTemplate').on('change', function () {
            $("#" + ReviewOfSystemsTemplateDetail.params["PanelID"] + ' #frmROSTemplate #hfIsFormHasChange').val('true');
        });
        ReviewOfSystemsTemplateDetail.validateROSTemplate();
        ReviewOfSystemsTemplateDetail.bIsFirstLoad = false;
    },
    /*
   Author: Muhammad Azhar Shahzad
   Purpose: for Grid Load of Ros template
   Creation Date: March 02,2016 */
    rosTemplateLoad: function (ROSTemplateId) {

        ReviewOfSystemsTemplateDetail.searchROSTemplate_DBCall(ROSTemplateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                ReviewOfSystemsTemplateDetail.rosTemplateDataLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    rosTemplateDataLoad: function (response) {

        var ROSTemplateLoadJSONData = JSON.parse(response.ROSTemplateLoad_JSON);

        $.each(ROSTemplateLoadJSONData, function (i, item) {

            $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' input#txtTemplateName').val(item.TemplateName);
            $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlEntity').val(item.EntityId);

            $.when(ReviewOfSystemsTemplateDetail.isEntitySelected(ReviewOfSystemsTemplateDetail.isSuperAdmin())).then(function () {

                //ReviewOfSystemsTemplateDetail.enableDisableDropDownLists('ddlSpecialty,ddlProvider', false);
                //$('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlSpecialty').multiselect('clearSelection', false);
                //$('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlSpecialty').multiselect('updateButtonText');
                //if (item.IsSpecialtyAll == "False") {
                //    // Set the value                
                //    EMRUtility.selectOptionsByCommaSeprateValue($('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlSpecialty'), item.SpecialtyIds);
                //    // Then refresh
                //    $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlSpecialty').multiselect("refresh");
                //    ReviewOfSystemsTemplateDetail.specialityCheckedIds = $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlSpecialty').val();
                //} else {
                //    $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlSpecialty').multiselect('selectAll', false);
                //    $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlSpecialty').multiselect('updateButtonText');
                //}

                //$('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlProvider').multiselect('clearSelection', false);
                //$('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlProvider').multiselect('updateButtonText');

                //if (item.IsProviderAll == "False") {
                //    // Set the value
                //    EMRUtility.selectOptionsByCommaSeprateValue($('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlProvider'), item.ProviderIds);
                //    // Then refresh
                //    $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlProvider').multiselect("refresh");
                //} else {
                //    ReviewOfSystemsTemplateDetail.updateProviderDDL();
                //    $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlProvider').multiselect('selectAll', false);
                //    $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlProvider').multiselect('updateButtonText');
                //}

                if (item.ProviderIds != "") {
                    $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlProvider').multiselect('clearSelection', false);
                    $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlProvider').multiselect('updateButtonText');
                    $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + " #ddlProvider").val(item.ProviderIds.split(','));
                    $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlProvider').multiselect("refresh");
                }
                if (item.SpecialtyIds != "") {
                    $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlSpecialty').multiselect('clearSelection', false);
                    $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlSpecialty').multiselect('updateButtonText');
                    $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + " #ddlSpecialty").val(item.SpecialtyIds.split(','));
                    $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlSpecialty').multiselect("refresh");
                }


            });

        });
    },

    // This Function will load all the Systems and add it as two column sortable list 
    // Author: ZeeshanAK | Date: February 29, 2016 */
    loadSystems: function (EditMode) {

        ReviewOfSystemsTemplateDetail.getROSSystems_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                var $ROSSystemsUL = $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ROSSystemsUL');
                var SystemsJSON = JSON.parse(response.Systems_JSON);

                $ROSSystemsUL.empty();

                $.each(SystemsJSON, function (index, element) {
                    $ROSSystemsUL.append(ReviewOfSystemsTemplateDetail.createSystemliHtml(element.ROSSystemId, element.SystemName, element.ROSTemplateId, EditMode));
                });

                ReviewOfSystemsTemplateDetail.iniailizeSystemEditName();
                ReviewOfSystemsTemplateDetail.sortingInitializSystem();

            }
            else {
                utility.DisplayMessages(strMessage, 3);
            }

        });
    },

    iniailizeSystemEditName: function () {
        var Edithandler = function (event) {
            ReviewOfSystemsTemplateDetail.editLabelSystemInfo(event, this)
        };
        $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ROSSystemsUL .editSystem').unbind("click", Edithandler).bind("click", Edithandler);
        var Deletehandler = function (event) {
            ReviewOfSystemsTemplateDetail.deleteLabelSystemInfo(event, this)
        };
        $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ROSSystemsUL .removeSystem').unbind("click", Deletehandler).bind("click", Deletehandler);
    },

    createSystemliHtml: function (ROSSystemId, SystemName, ROSTemplateId, EditMode) {
        var checked = "";
        if (EditMode != null && EditMode) {
            checked = " checked='checked'";
        }
        return "<li id='" + ROSSystemId + "' onclick='ReviewOfSystemsTemplateDetail.getCharacteristics(this," + ROSSystemId + ",\"" + SystemName + "\"," + ROSTemplateId +
                        ", event);'><div> <div class='pull-left'> <div class='checkbox-custom checkboxTiny checkbox-success'> <input type='checkbox' onchange='ReviewOfSystemsTemplateDetail.checkUncheckSystem(this,event);' " + checked + "/>  <label class='control-label'>" +
                        SystemName + "</label> </div></div> " +

            "<a class='btn btn-xs btn-link pull-right mr-sm editSystem'  href='javascript:void(0);'><i class='fa fa-edit'></i></a>" +
            "<a class='btn btn-xs btn-link removeIconListHover removeSystem' href='javascript:void(0);'><i class='fa fa-remove'></i></a>" +
            "<div class='clearfix'></div> </div><input type='hidden' id='hdnROSTemplateId' value='" + ROSTemplateId + "'/></li>";
    },

    editLabelSystemInfo: function (event, obj) {

        ReviewOfSystemsTemplateDetail.renameSystem(true, obj);
    },

    deleteLabelSystemInfo: function (event, obj) {

        utility.myConfirm('1', function () {
            $(obj).closest('li').remove();
            $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #divSelectAll').remove();
            $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ROSSystemsCharacteristicsUL').html('');
            $("#" + ReviewOfSystemsTemplateDetail.params["PanelID"] + ' #frmROSTemplate #hfIsFormHasChange').val('true');
        }, function () { });

    },

    //This function will enable disable multiselect ddls provided
    // Author: ZeeshanAK | Date: March 08, 2016 
    enableDisableDropDownLists: function (ddlIds, isDisable) {

        ddlCommaSeparatedIds = ddlIds.split(',');
        var parrentPanelId = "#" + ReviewOfSystemsTemplateDetail.params["PanelID"];
        $.each(ddlCommaSeparatedIds, function (index, Item) {
            if (isDisable) {
                $(parrentPanelId + " #" + Item).multiselect('disable');
            }
            else {
                $(parrentPanelId + " #" + Item).multiselect('enable');
            }
        });
    },

    //This Function will help in renaming a System or Characteristics name
    // Author: ZeeshanAK | Date: March 02, 2016 
    renameSystem: function (isUpdate, obj) {
        var ROSSystemsUL = '#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ROSSystemsUL';
        if (isUpdate && obj != null) {
            var currentLiText = $(obj).prev().find('label').text();
            var textFieldHTML = "<div class='rightInnerAddon pb-xs'><textarea rows='1' spellcheck='true' maxlength='50' data-attribute-prev-title='" + currentLiText + "'  class='form-control pr-xlg height-max105 textAreaScroll' onkeydown=\"ReviewOfSystemsTemplateDetail.saveNewNameofSystem(event,this,0);\">" + currentLiText + "</textarea><div class='clearfix'></div><div class='rightInnerAddonBtn'><a class='btn btn-link btn-xs ml-xs mr-xs' onclick=\"ReviewOfSystemsTemplateDetail.saveNewNameofSystem(event,this,1);\"><i class='fa fa-save'></i></a></div></div>";
            $(obj).parent().addClass('hidden');
            $(obj).parent().parent().append(textFieldHTML);
            $(ROSSystemsUL + ' li').not($(obj).parent().parent()).find('a').toggleClass('hidden');

            var data = $(obj).parent().parent().find('textarea').val();
            $(obj).parent().parent().find('textarea').focus().val('').val(data);
        } else {
            if ($(ROSSystemsUL + ' li textarea').length <= 0) {
                var ROSSystemsLength = $(ROSSystemsUL + ' li').length;
                $(ROSSystemsUL).prepend(ReviewOfSystemsTemplateDetail.createSystemliHtml('-' + ROSSystemsLength, '', ReviewOfSystemsTemplateDetail.params.ROSTemplateId, false));
                //  ReviewOfSystemsTemplateDetail.iniailizeSystemEditName();
                var Edithandler = function (event) {
                    ReviewOfSystemsTemplateDetail.editLabelSystemInfo(event, this)
                };
                var deletehandler = function (event) {
                    ReviewOfSystemsTemplateDetail.deleteLabelSystemInfo(event, this)
                };
                $(ROSSystemsUL + ' #-' + ROSSystemsLength + ' .editSystem').unbind("click", Edithandler).bind("click", Edithandler);
                $(ROSSystemsUL + ' #-' + ROSSystemsLength + ' .removeSystem').unbind("click", deletehandler).bind("click", deletehandler);
                ReviewOfSystemsTemplateDetail.editLabelSystemInfo(event, $(ROSSystemsUL + ' #-' + ROSSystemsLength + ' .editSystem'))
            }
        }
        ReviewOfSystemsTemplateDetail.bindBlurEvent();

    },

    bindBlurEvent: function () {

        $('#' + ReviewOfSystemsTemplateDetail.params.PanelID).off().on('click', function (event) {
            var $ROSSystemsULItem = $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #divROSSystemsUL');
            var $ROSSystemsCharacteristicsULItem = $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ROSSystemsCharacteristicsUL');
            var $item;
            if ($ROSSystemsULItem.find('textarea').length > 0) {
                $item = $ROSSystemsULItem;
                ReviewOfSystemsTemplateDetail.eventBlurFunc($item, $ROSSystemsULItem, null);
            }
            if ($ROSSystemsCharacteristicsULItem.find('textarea').length > 0) {
                $item = $ROSSystemsCharacteristicsULItem;
                ReviewOfSystemsTemplateDetail.eventBlurFunc($item, null, $ROSSystemsCharacteristicsULItem);
            }
            if ($ROSSystemsULItem.find('textarea').length == 0 && $ROSSystemsCharacteristicsULItem.find('textarea').length == 0) {
                $('#' + ReviewOfSystemsTemplateDetail.params.PanelID).unbind("click");
            }
        });

    },

    eventBlurFunc: function ($item, $typeSys, $typeCharc) {
        var id = $item.find('textarea').closest('li').attr('id');
        var eventId = $(event.target).closest('li#' + id).attr('id');
        if ($item.find('textarea').length > 0 && id != eventId && event.srcElement.localName != 'i') {
            var currentLiText = $item.find('textarea').attr('data-attribute-prev-title');
            if ($typeSys != null && event.srcElement.id != 'btn-addSys') {
                ReviewOfSystemsTemplateDetail.saveNewNameofSystem(event, $item.find('textarea'), 2, currentLiText);
            }
            if ($typeCharc != null && event.srcElement.id != 'btn-addChac') {
                ReviewOfSystemsTemplateDetail.saveNewValueOfCharc(event, $item.find('textarea'), 2, currentLiText);
            }
        } else {
            $item.find('textarea').focus();
        }
    },
    //This Function will save a renamed System or Characteristics name
    // Author: ZeeshanAK | Date: March 02, 2016 
    saveNewNameofSystem: function (e, obj, btnClicked, currentLiText) {
        e.stopPropagation();

        var ROSSystemsUL = '#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ROSSystemsUL';
        var ROSSystemsLength = $(obj).parents('li').attr('id');

        var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
        if (keyCode == 13 || btnClicked == 1) {
            var newName = $(obj).parents('li').find('textarea').val();
            if ($.trim(newName).length > 0) {
                $(ROSSystemsUL + ' li').each(function (index, elem) {
                    if (($.trim($(elem).text())).toLowerCase() === ($.trim(newName)).toLowerCase()) {
                        utility.DisplayMessages('A duplicate System already exist.', 3);
                        $(obj).closest("div.rightInnerAddon").remove();
                        $(ROSSystemsUL + ' #' + ROSSystemsLength + ' .editSystem').trigger('click');
                        e.preventDefault();
                        return false;
                    }
                });
                ReviewOfSystemsTemplateDetail.updateSystemLiValue(obj, newName, true);
            } else {
                utility.DisplayMessages('Please enter System Name', 3);
                e.preventDefault();
            }
        } else if (btnClicked == 2) {
            if (currentLiText != '') {
                ReviewOfSystemsTemplateDetail.updateSystemLiValue(obj, currentLiText, false);
            } else {
                $(obj).closest("li").remove();
            }
        }
    },

    updateSystemLiValue: function (obj, newName, haschange) {
        var ROSSystemsUL = '#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ROSSystemsUL';
        var ROSSystemsLength = $(obj).parents('li').attr('id');
        $(obj).parents('li').find('.hidden').toggleClass('hidden');
        $(obj).parents('li').find('label').text(newName);

        $(obj).parents('li').trigger('click');

        $(obj).closest("div.rightInnerAddon").remove();
        if ($(ROSSystemsUL + ' #' + ROSSystemsLength).find('textarea').length > 0) {
            $(ROSSystemsUL + ' li a.editSystem').addClass('hidden');
        } else {
            //$('#ROSSystemsUL li').find('[name=EditSystem]').removeClass('hidden');
            $(ROSSystemsUL + ' li a.editSystem').removeClass('hidden');
        }
        if (haschange) {
            $("#" + ReviewOfSystemsTemplateDetail.params["PanelID"] + ' #frmROSTemplate #hfIsFormHasChange').val('true');
        }
    },
    //This Function will load all the Characteristics for a specific System
    // Author: ZeeshanAK | Date: March 01, 2016 
    getCharacteristics: function (obj, SystemID, SystemName, ROSTemplateId, event) {
        event.stopPropagation();

        var charcSystemDiv = $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #divROSSystemsCharacteristics');
        var CurrentSystem = $($(obj).closest('ul')).find("li.active");
        if (CurrentSystem.text() != null && CurrentSystem.text() != '') {
            var CurrentSystemID = $(CurrentSystem).attr('id');
            if (CurrentSystemID != null && ROSTemplateId != null) {
                ReviewOfSystemsTemplateDetail.CacheCharacteristicInfo(Number(CurrentSystemID), $(CurrentSystem).find('label').text(), ROSTemplateId);
            }
        } else {
            CurrentSystem = $(obj);
        }
        if (obj != null) {
            $($(obj).parent()).find("li").each(function () {
                $(this).removeClass("active");
            });
            if ($(obj).hasClass("active") == false) {
                $(obj).addClass("active");
            }
        }

        var HTMLSystemCharacteristicsCached = ReviewOfSystemsTemplateDetail.bindCacheCharacteristicInfo(obj);
        if (HTMLSystemCharacteristicsCached) {
            var charcSystemDiv = $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #divROSSystemsCharacteristics');
            var isCurrentSystemChecked = $($(obj).closest('ul')).find("li.active").find('input:checkbox').is(':checked');
            if ($(charcSystemDiv).find('input:checkbox[name]:checked').length == $(charcSystemDiv).find('input:checkbox[name]').length) {
                if (isCurrentSystemChecked) {
                    $(charcSystemDiv).find('input:checkbox').prop('checked', true);
                } else {
                    $(charcSystemDiv).find('input:checkbox').prop('checked', false);
                }
            }
            ReviewOfSystemsTemplateDetail.sortingInitialzSystemCharc();

        } else
            if (SystemID > 0 && ROSTemplateId > 0) {
                ReviewOfSystemsTemplateDetail.getROSSystemsCharacteristics_DBCall(SystemID, ROSTemplateId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var HTMLSystemCharacteristics = "";
                        var HTMLSelectAllCheckBox = "";
                        var DivRow = $(document.createElement('div'));
                        var SystemCharacteristicsJSON = JSON.parse(response.SystemCharacteristics_JSON);
                        var $ROSSystemsCharacteristicsUL = $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ROSSystemsCharacteristicsUL');
                        if (response.CharcCount > 0) {
                            $.each(SystemCharacteristicsJSON, function (index, element) {


                                HTMLSystemCharacteristics += ReviewOfSystemsTemplateDetail.createCharCliHtml(SystemID, element.ROSSystemCharacteristicsId, element.CharacteristicsName, ROSTemplateId);

                            });
                        } else {
                            $ROSSystemsCharacteristicsUL.empty();
                            $($(obj).closest('ul')).find("li.active input[type=checkbox]").prop('checked', false);
                        }
                        HTMLSelectAllCheckBox += "<div id='divSelectAll' class='col-sm-6 col-md-4' style='margin-left: 25px;'> <div class='pull-left'><div class='checkbox-custom checkboxTiny checkbox-success'><input type='checkbox' onchange='ReviewOfSystemsTemplateDetail.selectAllCharC(this);'/><label class='control-label'><strong>Select All</strong></label></div></div></div>";
                        $ROSSystemsCharacteristicsUL.empty();
                        $ROSSystemsCharacteristicsUL.append(HTMLSystemCharacteristics);
                        $(charcSystemDiv).html($ROSSystemsCharacteristicsUL[0].outerHTML);
                        $(charcSystemDiv).prepend(HTMLSelectAllCheckBox);

                        if ($(obj).find('input:checkbox').is(':checked')) {
                            $(charcSystemDiv).find('input:checkbox').prop('checked', true);
                        } else {
                            $(charcSystemDiv).find('input:checkbox').prop('checked', false);
                        }

                        ReviewOfSystemsTemplateDetail.CacheCharacteristicInfo(SystemID, SystemName, ROSTemplateId);
                        ReviewOfSystemsTemplateDetail.sortingInitialzSystemCharc();

                    }
                    else {
                        utility.DisplayMessages(strMessage, 3);
                    }

                });
            } else {
                var $ROSSystemsCharacteristicsUL = $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ROSSystemsCharacteristicsUL');

                var HTMLSelectAllCheckBox = "<div id='divSelectAll' class='col-sm-6 col-md-4' style='margin-left: 25px;'> <div class='pull-left'><div class='checkbox-custom checkboxTiny checkbox-success'><input type='checkbox' onchange='ReviewOfSystemsTemplateDetail.selectAllCharC(this);'/><label class='control-label'><strong>Select All</strong></label></div></div></div>";

                $ROSSystemsCharacteristicsUL.empty();
                $(charcSystemDiv).html($ROSSystemsCharacteristicsUL[0].outerHTML);
                $(charcSystemDiv).prepend(HTMLSelectAllCheckBox);

                if ($(obj).find('input:checkbox').is(':checked')) {
                    $(ROSSystemsCharacteristicsUL).find('input:checkbox').prop('checked', true);
                } else {
                    $(ROSSystemsCharacteristicsUL).find('input:checkbox').prop('checked', false)
                }
                ReviewOfSystemsTemplateDetail.sortingInitialzSystemCharc();
            }


    },

    createCharCliHtml: function (ROSSystemId, CharacteristicsId, CharacteristicsName, ROSTemplateId) {
        return "<li class='col-sm-6 col-md-4' id='" + CharacteristicsId + "'> <div id='mainLiDiv'> <div class='pull-left pr-xs' name='EditCharacteristic'>" +
             "<a class='btn btn-xs btn-link' onclick='ReviewOfSystemsTemplateDetail.removeSystemCharcName(this,event);'><i class='fa fa-remove'></i></a>" +
            "<a class='btn btn-xs btn-link' onclick='ReviewOfSystemsTemplateDetail.editSystemCharcName(this,event);'><i class='fa fa-edit'></i></a></div>" +
            "<div class='pull-left'><div class='checkbox-custom checkboxTiny checkbox-success'><input type='checkbox' name='" + CharacteristicsName + "'onchange='ReviewOfSystemsTemplateDetail.checkUncheckCharacteristics(this,event);' />" +
            "<label></label></div></div><span class='pull-left size70per'>" + CharacteristicsName + "</span><input type='hidden' value='" + ROSTemplateId + "' name='ROSTemplateId' id='hdnROSTemplateId'/></div>" +

        "</li>";
    },

    checkUncheckSystem: function (obj, event) {
        event.stopPropagation();
        var charcSystemDiv = $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #divROSSystemsCharacteristics');
        var isCurrentSystemChecked = $($(obj).closest('ul')).find("li.active").find('input:checkbox').is(':checked');
        if (isCurrentSystemChecked) {
            if ($(charcSystemDiv).find('ul input:checkbox').length == 0) {

                $($(obj).closest('ul')).find("li.active").find('input:checkbox').prop('checked', false);
            } else {
                $(charcSystemDiv).find('input:checkbox').prop('checked', true);
            }
        } else {
            $(charcSystemDiv).find('input:checkbox').prop('checked', false);
        }
        $("#" + ReviewOfSystemsTemplateDetail.params["PanelID"] + ' #frmROSTemplate #hfIsFormHasChange').val('true');
    },

    //This Function will handle  check/unchek of a characteristics
    // Author: ZeeshanAK | Date: March 02, 2016 
    checkUncheckCharacteristics: function (obj, event) {
        //if Checkbox for Char li is checked
        var ischecked = $(obj).is(':checked');
        $('#ROSSystemsUL li.active').find('input:checkbox').prop('checked', ischecked);
        //if all characteristics are checked then check Select All too
        ReviewOfSystemsTemplateDetail.areAllCharsChecked();
        $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #divSelectAll').find('input:checkbox').prop('checked', !ischecked);
    },

    //If all chars are checked then check SelectAll checkbox
    // Author: ZeeshanAK | Date: March 08, 2016 
    areAllCharsChecked: function () {
        var numOfCharacteristics = $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ROSSystemsCharacteristicsUL li').length;
        var checkedCharacteristics = $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ROSSystemsCharacteristicsUL li :checkbox:checked').length;
        var checked = (checkedCharacteristics == numOfCharacteristics);
        $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #divSelectAll').find('input:checkbox').prop('checked', checked);

        if (checkedCharacteristics == 0) {
            $('#ROSSystemsUL li.active').find('input:checkbox').prop('checked', false);
        }

        $("#" + ReviewOfSystemsTemplateDetail.params["PanelID"] + ' #frmROSTemplate #hfIsFormHasChange').val('true');

    },

    selectAllCharC: function (obj) {
        $('#ROSSystemsUL li.active').find('input:checkbox').prop('checked', obj.checked);

        $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ROSSystemsCharacteristicsUL [type=checkbox]').prop('checked', obj.checked);

    },

    editSystemCharcName: function (obj, event) {

        ReviewOfSystemsTemplateDetail.renameCharacteristics(true, obj);
    },

    removeSystemCharcName: function (obj, event) {

        utility.myConfirm('1', function () {

            $(obj).closest('li').remove();
            $("#" + ReviewOfSystemsTemplateDetail.params["PanelID"] + ' #frmROSTemplate #hfIsFormHasChange').val('true');
            var $CurrentSystem = $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ROSSystemsUL li.active');
            if ($CurrentSystem && $CurrentSystem.text()) {
                var CurrentSystemID = $CurrentSystem.attr('id');
                var $ROSTemplateIdSystem = $CurrentSystem.find('#hdnROSTemplateId').val();
                if ($ROSTemplateIdSystem) {
                    ReviewOfSystemsTemplateDetail.CacheCharacteristicInfo(Number(CurrentSystemID), $CurrentSystem.find('label').text(), ROSTemplateIdSystem);
                }
            }
            if ($('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ROSSystemsCharacteristicsUL li').length == 0) {
                $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #divROSSystemsCharacteristics #divSelectAll input[type=checkbox]').prop('checked', false);
                $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ROSSystemsUL li.active input[type=checkbox]').prop('checked', false);
            }
        }, function () { });
    },
    //This Function will help in renaming a System or Characteristics name
    // Author: ZeeshanAK | Date: March 02, 2016 
    renameCharacteristics: function (isUpdate, obj) {
        var ROSCharacUL = '#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ROSSystemsCharacteristicsUL';

        if (isUpdate && obj != null) {
            $(ROSCharacUL).find('[name=EditCharacteristic]').addClass('disableAll');

            var currentLiText = $(obj).closest('li').find('span').text();
            var textFieldHTML = "<div class='rightInnerAddon pb-xs'><textarea rows='1' data-attribute-prev-title='" + currentLiText + "' class='form-control pr-xlg height-max105 textAreaScroll' maxlength='50' spellcheck='true' onkeydown=\"ReviewOfSystemsTemplateDetail.saveNewValueOfCharc(event,this,0);\">" + currentLiText + "</textarea><div class='clearfix'></div><div class='rightInnerAddonBtn'><a class='btn btn-link btn-xs'onclick=\"ReviewOfSystemsTemplateDetail.saveNewValueOfCharc(event,this,1);\"><i class='fa fa-save'></i></a></div></div>";
            $(obj).closest('li').find('[id=mainLiDiv]').addClass('hidden');
            $(obj).parents('li').append(textFieldHTML);
            $(obj).closest('li').find('textarea').focus().val('').val(currentLiText);
        } else {
            if ($(ROSCharacUL + ' li textarea').length <= 0) {
                $(ROSCharacUL).find('[name=EditCharacteristic]').addClass('disableAll');
                var ROSCharcLength = $(ROSCharacUL + ' li').length;
                var SystemId = $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ROSSystemsUL li.active').attr('id');
                if (SystemId != null && SystemId != '') {
                    $(ROSCharacUL).prepend(ReviewOfSystemsTemplateDetail.createCharCliHtml(SystemId, '-' + ROSCharcLength, '', ReviewOfSystemsTemplateDetail.params.ROSTemplateId));

                    ReviewOfSystemsTemplateDetail.editSystemCharcName($(ROSCharacUL + ' #-' + ROSCharcLength + ' a'));
                } else {
                    utility.DisplayMessages('Please select System to add Characteristic', 3);
                }
            }
        }
        ReviewOfSystemsTemplateDetail.bindBlurEvent();
    },

    //This Function will save a renamed System or Characteristics name
    // Author: ZeeshanAK | Date: March 02, 2016 
    saveNewValueOfCharc: function (e, obj, btnClicked, currentLiText) {
        e.stopPropagation();

        var ROSSystemCharUL = '#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ROSSystemsCharacteristicsUL';
        var ROSSystemsLength = $(obj).parents('li').attr('id');

        var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
        if (keyCode == 13 || btnClicked == 1) {
            var newName = $(obj).parents('li').find('textarea').val();
            var isUpdate = true;
            if ($.trim(newName).length > 0) {
                $(ROSSystemCharUL + ' li').each(function (index, elem) {
                    if (($.trim($(elem).text())).toLowerCase() === ($.trim(newName)).toLowerCase()) {
                        utility.DisplayMessages('A duplicate Characteristics already exist.', 3);
                        isUpdate = false;
                        e.preventDefault();
                        return false;
                    }
                });
                if (isUpdate) {
                    ReviewOfSystemsTemplateDetail.updateCharcliValue(obj, newName, true);
                }
            } else {
                utility.DisplayMessages('Please enter Characteristic Name', 3);
                e.preventDefault();
            }
        } else if (btnClicked == 2) {
            if (currentLiText != '') {
                ReviewOfSystemsTemplateDetail.updateCharcliValue(obj, currentLiText, false);
            } else {
                $(obj).closest("li").remove();
                $(ROSSystemCharUL).find('[name=EditCharacteristic]').removeClass('disableAll');
            }
        }
    },

    updateCharcliValue: function (obj, newName, haschange) {
        var ROSSystemCharUL = '#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ROSSystemsCharacteristicsUL';
        var ROSSystemsLength = $(obj).parents('li').attr('id');
        $(obj).parents('li').find('.hidden').toggleClass('hidden');
        $(obj).closest('li').find('span').text(newName);
        $(obj).closest('li').find('input[type=checkbox]').attr('name', newName);
        $(obj).closest('li').find("div.rightInnerAddon").remove();
        if (ROSSystemsLength != null && $(ROSSystemCharUL + ' #' + ROSSystemsLength).find('textarea').length > 0) {
            $(ROSSystemCharUL).find('[name=EditCharacteristic]').addClass('disableAll');
        } else {
            $(ROSSystemCharUL).find('[name=EditCharacteristic]').removeClass('disableAll');
        }
        if (ROSSystemsLength != null && ROSSystemsLength.startsWith('-')) {
            $(ROSSystemCharUL + ' #' + ROSSystemsLength).find('input:checkbox').prop('checked', true);
            //Start || 01 July, 2016 || ZeeshanAK || Change for checking Select All on char add
            ReviewOfSystemsTemplateDetail.areAllCharsChecked();
            //End   || 01 July, 2016 || ZeeshanAK || Change for checking Select All on char add
            //ReviewOfSystemsTemplateDetail.numOfCharsChecked++;
            $(ROSSystemCharUL + ' li.active').find('input:checkbox').prop('checked', true);
        }
    },

    //"enableDisableDropDownLists" If an Entity is selected then enable Speciality and Provider DDL's

    isEntitySelected: function (isSuperAdmin) {
        var objDeffered = $.Deferred();
        selectedEntity = $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlEntity option:selected').val();
        if (isSuperAdmin == false) {
            selectedEntity = globalAppdata["SeletedEntityId"];
        }

        $.when(ReviewOfSystemsTemplateDetail.loadEntitySpecialty(selectedEntity)).then(function () {

            $.when(ReviewOfSystemsTemplateDetail.loadEntityProvider(selectedEntity)).then(function () {

                objDeffered.resolve();

            });
        });
        return objDeffered;
    },

    loadEntitySpecialty: function (entityID) {
        var objDeffered = $.Deferred();
        // Loads Spacialties Based on entityId
        if (entityID != null && entityID > 0) {

            providerDetail.FillSpecialty(entityID).done(function (response) {
                if (response.status != false) {

                    var spacialties = JSON.parse(response.SpecialtyLoad_JSON);
                    $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlSpecialty').empty();

                    $.each(spacialties, function (i, item) {
                        $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlSpecialty').append(
                            $('<option/>', {
                                value: item.SpecialtyId,
                                html: item.ShortName
                            })
                        );
                    });

                    //Assign server side spacialties to the specialityCheckedIds array
                    if (ReviewOfSystemsTemplateDetail.SpecialtyIds != '') {

                        var Specialties = ReviewOfSystemsTemplateDetail.SpecialtyIds.split(",");
                        ReviewOfSystemsTemplateDetail.specialityCheckedIds = Specialties;
                        $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlSpecialty').val(Specialties);
                    }
                }

            }).then(function () {
                ReviewOfSystemsTemplateDetail.IntializeMultiSelectDropDownSpecialties();
                //enable dropdownlist
                ReviewOfSystemsTemplateDetail.enableDisableDropDownLists('ddlSpecialty', false);
                objDeffered.resolve();
            });
        }
        else {
            //Disable dropdownlist
            ReviewOfSystemsTemplateDetail.enableDisableDropDownLists('ddlSpecialty', true);
            objDeffered.resolve();
        }
        return objDeffered;
    },

    // "IntializeMultiSelectDropDownSpecialties" This function will initialize Specialty multiselect ddl

    IntializeMultiSelectDropDownSpecialties: function () {

        $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlSpecialty').multiselect('destroy');
        $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlSpecialty').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'All',
            selectAll: false,
            onChange: function (option, checked, select) {

                ReviewOfSystemsTemplateDetail.checkProvidersBySpecialityIds(option, checked, select);

            },
            onDropdownHide: function (event) {
                ReviewOfSystemsTemplateDetail.updateProviderDDL();
            },

            onDropdownShow: function (event) {
                //make items selected and initialize dropdownlist
                if (ReviewOfSystemsTemplateDetail.SpecialtyIds != '') {
                    var spacialties = ReviewOfSystemsTemplateDetail.SpecialtyIds.split(",");

                    if (spacialties != '' && typeof spacialties != 'undefined') {

                        $.each(spacialties, function (index, item) {
                            ReviewOfSystemsTemplateDetail.specialityCheckedIds = ReviewOfSystemsTemplateDetail.removeFromArray(ReviewOfSystemsTemplateDetail.specialityCheckedIds, item);
                            ReviewOfSystemsTemplateDetail.specialityCheckedIds.push(item);
                        });
                    }
                }
                ReviewOfSystemsTemplateDetail.setSpacialtiesByselectedProviderIds();
                $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlSpecialty').multiselect('select', ReviewOfSystemsTemplateDetail.specialityCheckedIds);
            },
        });
    },

    updateProviderDDL: function () {
        $.when(ReviewOfSystemsTemplateDetail.filterProvidersBySpecialtyIds()).then(function () {

            if (ReviewOfSystemsTemplateDetail.ProviderIds != '') {
                var Providers = ReviewOfSystemsTemplateDetail.ProviderIds.split(",");

                if (Providers != '' && typeof Providers != 'undefined') {

                    $.each(Providers, function (index, item) {
                        ReviewOfSystemsTemplateDetail.providerCheckedIds = ReviewOfSystemsTemplateDetail.removeFromArray(ReviewOfSystemsTemplateDetail.providerCheckedIds, item);
                        ReviewOfSystemsTemplateDetail.providerCheckedIds.push(item);
                    });
                }
            }
            $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlProvider').val(ReviewOfSystemsTemplateDetail.providerCheckedIds);
            ReviewOfSystemsTemplateDetail.IntializeMultiSelectDropDownProviders();
        });
    },
    //Start//03/16/2016//M Ahmad Imran//Implimented "setSpacialtiesByselectedProviderIds" This function will set specialty Ids in specailChekedIds array
    setSpacialtiesByselectedProviderIds: function () {

        $.each(ReviewOfSystemsTemplateDetail.providerCheckedIds, function (index, item) {

            $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlProvider option').each(function () {
                if ($(this).val() != '') {
                    if ($(this).val() == item) {

                        ReviewOfSystemsTemplateDetail.specialityCheckedIds = ReviewOfSystemsTemplateDetail.removeFromArray(ReviewOfSystemsTemplateDetail.specialityCheckedIds, $(this).attr('refname'));
                        ReviewOfSystemsTemplateDetail.specialityCheckedIds.push($(this).attr('refname'));
                    }
                }
            });
        });
    },
    //Start//03/16/2016//M Ahmad Imran//Implimented "filterProvidersBySpecialtyIds"  This function will save spacialty ids and will show privders on spacialty selection
    filterProvidersBySpecialtyIds: function () {

        var providerHiddenContext = '#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlHiddenNotesTemplateProvider';

        var providerContext = '#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlProvider';
        $(providerContext).empty();

        if (ReviewOfSystemsTemplateDetail.specialityCheckedIds.length > 0) {

            $.each(ReviewOfSystemsTemplateDetail.specialityCheckedIds, function (index, specialtyId) {

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
        var specialtyContext = '#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #divNotesTemplateSpecialty';
        var isAllSpecialtySelected = $(specialtyContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var specialtyItems = $(specialtyContext).find('.dropdown-menu').find('li').length;
        var checkedSpecialtyItems = $(specialtyContext).find('.dropdown-menu').find('li.active').length;

        if (checkedSpecialtyItems <= 0) {
            ReviewOfSystemsTemplateDetail.specialityCheckedIds = [];
            ReviewOfSystemsTemplateDetail.providerCheckedIds = [];
            ReviewOfSystemsTemplateDetail.ProviderIds = '';
            ReviewOfSystemsTemplateDetail.SpecialtyIds = '';
        }
        else {
            if (!isAllSpecialtySelected && !(specialtyItems == checkedSpecialtyItems)) {


                var spacialityId = $(option).attr("value");
                if (checked && spacialityId != "") {
                    ReviewOfSystemsTemplateDetail.specialityCheckedIds = ReviewOfSystemsTemplateDetail.removeFromArray(ReviewOfSystemsTemplateDetail.specialityCheckedIds, spacialityId);
                    ReviewOfSystemsTemplateDetail.specialityCheckedIds.push(spacialityId);
                }
                else {

                    ReviewOfSystemsTemplateDetail.specialityCheckedIds = ReviewOfSystemsTemplateDetail.removeFromArray(ReviewOfSystemsTemplateDetail.specialityCheckedIds, spacialityId);

                }


            }
            else {

                ReviewOfSystemsTemplateDetail.specialityCheckedIds = [];
                $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlSpecialty option').each(function () {
                    var spacialityId = $(this).attr("value");
                    ReviewOfSystemsTemplateDetail.specialityCheckedIds.push(spacialityId);
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

        var providerContext = '#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #providerMultiList';
        //find match in both mutiselect and show it
        $(providerContext).find('.dropdown-menu').find('li').each(function () {

            var $li = $(this);
            var isExists = false;
            var value = $li.find('input').val();
            var ddlId = $(providerContext).find('select').attr('id');

            $.each(ReviewOfSystemsTemplateDetail.specialityCheckedIds, function (index, specialtyId) {

                if (value != "") {
                    var refName = ReviewOfSystemsTemplateDetail.getRefValuefromDdl(ddlId, value);

                    // if true then show and check the provider
                    if (specialtyId == refName) {
                        isExists = true;
                        return false;
                    }
                }
            });
            if (ReviewOfSystemsTemplateDetail.specialityCheckedIds.length > 0) {
                if (!isExists) {
                    ReviewOfSystemsTemplateDetail.showHideMultiSelectDdlOptions(true, ddlId, $li, value, 'provider');
                }
                else {
                    ReviewOfSystemsTemplateDetail.showHideMultiSelectDdlOptions(false, ddlId, $li, value, 'provider');
                }
            }
            else {
                ReviewOfSystemsTemplateDetail.showHideMultiSelectDdlOptions(false, ddlId, $li, value, 'provider', "unCheckAll");
            }
        });
    },
    //Start//03/16/2016//M Ahmad Imran//Implimented "getRefValuefromDdl" This function will return refname using (li's input value equals ddl option value)
    getRefValuefromDdl: function (ddlId, liId) {
        var $ddlOptions = $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + " #" + ddlId).find('option');
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

                    $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + " #" + ddlId).find('option[value="' + ddlOptionValue + '"]').prop('disabled', true);
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
            $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + " #" + ddlId).find('option[value="' + ddlOptionValue + '"]').prop('disabled', false);

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
                var $providerDdl = $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlProvider');
                var $providerHiddenDdl = $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlHiddenNotesTemplateProvider');

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
                if (ReviewOfSystemsTemplateDetail.ProviderIds != '') {
                    var Providers = ReviewOfSystemsTemplateDetail.ProviderIds.split(",");
                    ReviewOfSystemsTemplateDetail.providerCheckedIds = Providers;
                    $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlProvider').val(Providers);
                }

            }).then(function () {
                // A hack to trigger the onDropDownHide event of Spacialty multiselect      
                $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #divNotesTemplateSpecialty').find('.dropdown-toggle').dropdown('toggle').dropdown('toggle');
                //Intialized in onhidden spacialty ddl.  
                ReviewOfSystemsTemplateDetail.IntializeMultiSelectDropDownProviders();
                objDeffered.resolve();

            });
            //enable multiselect
            ReviewOfSystemsTemplateDetail.enableDisableDropDownLists('ddlProvider', false);
        }
        else {
            //disable multiselect
            ReviewOfSystemsTemplateDetail.enableDisableDropDownLists('ddlProvider', true);
            objDeffered.resolve();
        }
        return objDeffered;
    },
    //Start//03/16/2016//M Ahmad Imran//Implimented "IntializeMultiSelectDropDownProviders" This function will initialize provider multiselect ddl
    IntializeMultiSelectDropDownProviders: function () {
        $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlProvider').multiselect('destroy');
        $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'All',
            selectAll: false,
            onChange: function (option, checked, select) {
                ReviewOfSystemsTemplateDetail.checkSpecialtiesByProviderId(option, checked, select);
            }
        });
    },
    //Start//03/16/2016//M Ahmad Imran//Implimented "specialitiesByProviderIds" This function will load check spcialities by provider ids checked
    specialitiesByProviderIds: function () {
        //specialty context
        var specialtyContext = '#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #specialityMultiList';
        var ddlId = $(specialtyContext).find('select').attr('id');
        //find match in both mutiselect and show it
        $(specialtyContext).find('.dropdown-menu').find('li').each(function () {

            var $li = $(this);
            //load selected Data by spacialty Ids
            var isExists = false;
            var value = $li.find('input').val();
            if (ReviewOfSystemsTemplateDetail.providerCheckedIds.length > 0) {

                $.each(ReviewOfSystemsTemplateDetail.providerCheckedIds, function (index, providerId) {
                    if (value != "") {
                        if (providerId == value) {
                            isExists = true;
                            return false;
                        }
                    }
                });

                ReviewOfSystemsTemplateDetail.showHideMultiSelectDdlOptions(isExists, ddlId, $li, value, 'specialty');

            } else {
                ReviewOfSystemsTemplateDetail.showHideMultiSelectDdlOptions(true, ddlId, $li, value, 'specialty');
            }

        });
    },
    //Start//03/16/2016//M Ahmad Imran//Implimented "enableDisableDropDownLists" If user is Super Admin show the Entity dropdown otherwise don't

    isSuperAdmin: function () {
        if (globalAppdata['AppUserName'] == DefaultUser) {
            $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #entityDDL').show();
            return true;
        } else {
            $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #entityDDL').hide();
            return false;
        }
    },
    //Start//03/16/2016//M Ahmad Imran//Implimented "enableDisableDropDownLists" which will enable disable multiselect ddls provided
    enableDisableDropDownLists: function (ddlIds, isHide) {
        ddlCommaSeparatedIds = ddlIds.split(',');
        var parrentPanelId = "#" + ReviewOfSystemsTemplateDetail.params["PanelID"];
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

        $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlSpecialty').multiselect('destroy');
        $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlSpecialty').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'All',
            selectAll: false,
            onChange: function (option, checked, select) {

                ReviewOfSystemsTemplateDetail.checkProvidersBySpecialityIds(option, checked, select);

            },
            onDropdownHide: function (event) {
                $.when(
                    ReviewOfSystemsTemplateDetail.filterProvidersBySpecialtyIds()
                ).then(function () {
                    if (ReviewOfSystemsTemplateDetail.ProviderIds != '') {
                        var Providers = ReviewOfSystemsTemplateDetail.ProviderIds.split(",");

                        if (Providers != '' && typeof Providers != 'undefined') {

                            $.each(Providers, function (index, item) {
                                ReviewOfSystemsTemplateDetail.providerCheckedIds = ReviewOfSystemsTemplateDetail.removeFromArray(ReviewOfSystemsTemplateDetail.providerCheckedIds, item);
                                ReviewOfSystemsTemplateDetail.providerCheckedIds.push(item);
                            });
                        }
                    }
                    $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlProvider').val(ReviewOfSystemsTemplateDetail.providerCheckedIds);
                    ReviewOfSystemsTemplateDetail.IntializeMultiSelectDropDownProviders();
                });
            },

            onDropdownShow: function (event) {
                //make items selected and initialize dropdownlist
                if (ReviewOfSystemsTemplateDetail.SpecialtyIds != '') {
                    var spacialties = ReviewOfSystemsTemplateDetail.SpecialtyIds.split(",");

                    if (spacialties != '' && typeof spacialties != 'undefined') {

                        $.each(spacialties, function (index, item) {
                            ReviewOfSystemsTemplateDetail.specialityCheckedIds = ReviewOfSystemsTemplateDetail.removeFromArray(ReviewOfSystemsTemplateDetail.specialityCheckedIds, item);
                            ReviewOfSystemsTemplateDetail.specialityCheckedIds.push(item);
                        });
                    }
                }
                ReviewOfSystemsTemplateDetail.setSpacialtiesByselectedProviderIds();
                $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlSpecialty').multiselect('select', ReviewOfSystemsTemplateDetail.specialityCheckedIds);
            },
        });
    },
    //End M Ahmad Imran 03/15/2016
    //Start//03/15/2016//M Ahmad Imran//Implimented "checkSpecialtiesByProviderId"  This function will save privder ids and will check speciality on provider selection
    checkSpecialtiesByProviderId: function (option, checked, select) {

        //provider context
        var providerContext = '#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #divNotesTemplateProvider';
        var isAllProviderSelected = $(providerContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var providerItems = $(providerContext).find('.dropdown-menu').find('li').length;
        var checkedProviderItems = $(providerContext).find('.dropdown-menu').find('li.active').length;

        if (checkedProviderItems <= 0) {
            ReviewOfSystemsTemplateDetail.providerCheckedIds = [];
            ReviewOfSystemsTemplateDetail.ProviderIds = '';
        }
            //push all provider checked items
        else if (isAllProviderSelected) {
            ReviewOfSystemsTemplateDetail.providerCheckedIds = [];
            $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlProvider option').each(function () {
                var providerValue = $(this).val();
                ReviewOfSystemsTemplateDetail.providerCheckedIds.push(providerValue);
            });
        }
        else {
            // provider value
            var providerValue = $(option).val();
            //delete from provider array if not checked
            ReviewOfSystemsTemplateDetail.providerCheckedIds = ReviewOfSystemsTemplateDetail.removeFromArray(ReviewOfSystemsTemplateDetail.providerCheckedIds, providerValue);
            // add to provider array if checked
            if (checked) {
                ReviewOfSystemsTemplateDetail.providerCheckedIds.push(providerValue);
            }

        }
    },

    //*****************************************
    //********** Save/Update Operations********
    //*****************************************

    //Binding Validation Function
    validateROSTemplate: function () {
        $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #frmROSTemplate').bootstrapValidator('destroy');
        $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #frmROSTemplate')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  TemplateName: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: 'Specify a Name for the Template and try again.'
                          },
                      }
                  },
                  Entity: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  }

              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           if ($('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ROSSystemsUL li').find('[type=checkbox]:checked').length > 0) {
               ReviewOfSystemsTemplateDetail.saveROSTemplate(true);
           } else {
               utility.DisplayMessages('Please select at least one System to save/update Template', 4);
           }

       });
    },

    saveROSTemplate: function (unLoad, ROSTemplateId, isSaveAS) {

        if ($("#" + ReviewOfSystemsTemplateDetail.params["PanelID"] + ' #frmROSTemplate #hfIsFormHasChange').val() != '') {

            var $self = $("#" + ReviewOfSystemsTemplateDetail.params["PanelID"] + ' #rosTempNameSpecProvDiv');
            var $CurrentSystem = $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ROSSystemsUL li.active');

            if ($CurrentSystem.text()) {
                var CurrentSystemID = $CurrentSystem.attr('id');
                var ROSTemplateIdSystem = $CurrentSystem.find('#hdnROSTemplateId').val();
                if (CurrentSystemID && ROSTemplateIdSystem) {
                    ReviewOfSystemsTemplateDetail.CacheCharacteristicInfo(Number(CurrentSystemID), $CurrentSystem.find('label').text(), ROSTemplateIdSystem);
                }
            }
            if (ROSTemplateId == null) {
                ROSTemplateId = ReviewOfSystemsTemplateDetail.params.ROSTemplateId
            }
            var myJSON = $self.getMyJSONByName();
            ReviewOfSystemsTemplateDetail.saveROSTemplate_DbCall(myJSON, ROSTemplateId, isSaveAS).done(function (response) {

                response = JSON.parse(response);

                if (response.status != false) {
                    ReviewOfSystemsTemplate.rosTemplateSearch();
                    utility.DisplayMessages(response.Message, 1);

                    if (unLoad) {
                        UnloadActionPan();
                    } else {
                        ReviewOfSystemsTemplateDetail.params.ROSTemplateId = response.ROSTemplateId;
                        ReviewOfSystemsTemplateDetail.CharacteristicsInfo = [];

                        $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ROSSystemsCharacteristicsUL').empty();
                        $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #divROSSystemsCharacteristics div:not(#ROSSystemsCharacteristicsUL)').remove();
                        $("#" + ReviewOfSystemsTemplateDetail.params["PanelID"] + ' #frmROSTemplate #hfIsFormHasChange').val('');

                        ReviewOfSystemsTemplateDetail.loadSystems(true);
                    }

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        } else {
            utility.DisplayMessages('Please make any changes to save/update', 1);
        }
    },

   
    //*********************************************************
    //*************save as functions***************************
    saveAsRosTemplate: function () {
        if (ReviewOfSystemsTemplateDetail.params.ROSTemplateId <= 0) {
            utility.DisplayMessages('You cannot perform this action in new add mode.', 3);
        } else {
            $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #txtTemplateName').val($('#txtTemplateNameSaveAs').val());
            if ($('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ROSSystemsUL li').find('[type=checkbox]:checked').length > 0) {
                $("#" + ReviewOfSystemsTemplateDetail.params["PanelID"] + ' #frmROSTemplate #hfIsFormHasChange').val('true');
                ReviewOfSystemsTemplateDetail.saveROSTemplate(false, null, true);
                ReviewOfSystemsTemplateDetail.unloadReviewOfSystemsTemplateDetailSaveAs();
            } else {
                utility.DisplayMessages('Please select at least one System to save/update Template', 4);
            }
        }
    },

    // this function is used by both Notes and Progress Note Form
    showRosTemplateSaveAs: function (ActionPanID) {

        $(ActionPanID).prepend($("#ReviewOfSystemsTemplateDetail_SaveAs").html());
        $(ActionPanID).modal({
            show: 'true',
            backdrop: 'static',
            keyboard: false

        }).on('hidden.bs.modal', function () {
            $('body').addClass('modal-open');
        })
.on('shown.bs.modal', function () {
    ReviewOfSystemsTemplateDetail.validateROSTemplateSaveAs(ActionPanID);
});


    },

    validateROSTemplateSaveAs: function (ActionPanID) {
        $(ActionPanID + ' #frmROSTemplateSaveAs').bootstrapValidator('destroy');
        $(ActionPanID + ' #frmROSTemplateSaveAs')
          .bootstrapValidator({
              live: 'enabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  TemplateNameSaveAs: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           ReviewOfSystemsTemplateDetail.saveAsRosTemplate();
       });
    },

    unloadReviewOfSystemsTemplateDetailSaveAs: function () {
        var ActionPanId = '#ReviewOfSystemsTemplateDetail #actionPanClinicalReviewofSystems';

        $(ActionPanId).modal('hide');

        setTimeout(function () {
            $(ActionPanId).find('div').first().remove();
        }, 300);

    },
    //**********************end save as functions****************
    resetRosTemplate: function () {
        if ($("#" + ReviewOfSystemsTemplateDetail.params["PanelID"] + ' #frmROSTemplate #hfIsFormHasChange').val() != '') {
            utility.myConfirm('26', function () {
                ReviewOfSystemsTemplateDetail.CharacteristicsInfo = [];

                if (ReviewOfSystemsTemplateDetail.isSuperAdmin()) {
                    ReviewOfSystemsTemplateDetail.enableDisableDropDownLists('ddlSpecialty,ddlProvider', true);
                } else {
                    ReviewOfSystemsTemplateDetail.enableDisableDropDownLists('ddlSpecialty,ddlProvider', false);
                }
                if (ReviewOfSystemsTemplateDetail.params["mode"] = "Edit" && ReviewOfSystemsTemplateDetail.params.ROSTemplateId > 0) {
                    ReviewOfSystemsTemplateDetail.rosTemplateLoad(ReviewOfSystemsTemplateDetail.params.ROSTemplateId);
                    ReviewOfSystemsTemplateDetail.loadSystems(true);
                    $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #btnSaveAs').show();
                } else {
                    ReviewOfSystemsTemplateDetail.loadSystems(false);
                    $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #btnSaveAs').hide();
                    $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #rosTempNameSpecProvDiv').resetAllControls();
                    $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlProvider').multiselect('clearSelection', false);
                    $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlProvider').multiselect('updateButtonText');
                    $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlSpecialty').multiselect('clearSelection', false);
                    $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlSpecialty').multiselect('updateButtonText');
                }
                $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ROSSystemsCharacteristicsUL').empty();
                $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #divROSSystemsCharacteristics div:not(#ROSSystemsCharacteristicsUL)').remove();
                $("#" + ReviewOfSystemsTemplateDetail.params["PanelID"] + ' #frmROSTemplate #hfIsFormHasChange').val('');
            }, function () { });
        }
    },
    //************End Save/Update operations***

    

    unloadReviewOfSystemsTemplateDetail: function () {
        if ($("#" + ReviewOfSystemsTemplateDetail.params["PanelID"] + ' #frmROSTemplate #hfIsFormHasChange').val() != '') {
            utility.myConfirm('Data has been modified. Are you sure you want to save the changes?', function () {
                $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #frmROSTemplate').submit();
            },
            function () {
                UnloadActionPan(ReviewOfSystemsTemplateDetail.params["ParentCtrl"], "ReviewOfSystemsTemplateDetail");
            },
             'Confirmation'
             );
        } else {
            UnloadActionPan(ReviewOfSystemsTemplateDetail.params["ParentCtrl"], "ReviewOfSystemsTemplateDetail");
        }
    },


    IntializeMultiSelectDropDown: function () {


        $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlSpecialty').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'All',
            allSelectedText: 'Selected All'
        });

        $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'All',
            allSelectedText: 'Selected All'

        });
        var $multiSelectCntrl = $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlSpecialty, #' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlProvider');
        $multiSelectCntrl.multiselect('selectAll', false);
        $multiSelectCntrl.multiselect('updateButtonText');
    },

    sortingInitializSystem: function () {
        var SystemUl = '#' + ReviewOfSystemsTemplateDetail.params.PanelID + " div#divROSSystemsUL ul";
        $(SystemUl).sortable({
            connectWith: SystemUl,
            placeholder: "ui-state-highlight",
            stop: function (event, ui) {
                setTimeout(function () {
                    var sortedIdsInOrder = $(SystemUl).sortable("toArray");
                    $("#" + ReviewOfSystemsTemplateDetail.params["PanelID"] + ' #frmROSTemplate #hfIsFormHasChange').val('true');
                }, 5);
            }
        });
    },

    sortingInitialzSystemCharc: function () {
        var CharcUl = '#' + ReviewOfSystemsTemplateDetail.params.PanelID + " div#divROSSystemsCharacteristics ul";
        $(CharcUl).sortable({
            connectWith: CharcUl,
            //  revert: true,
            placeholder: "ui-state-highlight",
            //helper: 'clone'
            stop: function (event, ui) {
                setTimeout(function () {
                    var sortedIdsInOrder = $(CharcUl).sortable("toArray");
                    $("#" + ReviewOfSystemsTemplateDetail.params["PanelID"] + ' #frmROSTemplate #hfIsFormHasChange').val('true');
                }, 5);
            }
        });
    },

    //*****************************************
    //************* Chaching functions*********
    //*****************************************

    // Binds Characteristics details
    bindCacheCharacteristicInfo: function (cntrl) {
        if (ReviewOfSystemsTemplateDetail.CharacteristicsInfo.length > 0) {
            var ROSSystemId = Number(cntrl.id);
            var detail = $.grep(ReviewOfSystemsTemplateDetail.CharacteristicsInfo, function (item, index) {
                return item.ROSSystemId == ROSSystemId;
            });
            if ((detail != null && detail.length != 0)) {
                HTMLSystemCharacteristicsCached = detail[0].HTMLDetail;
                HTMLSystemCharacteristics_JSON = detail[0].DetailJSON;

                var $ROSSystemsCharacteristicsUL = $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ROSSystemsCharacteristicsUL');
                var $charcSystemDiv = $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #divROSSystemsCharacteristics');
                var HTMLSelectAllCheckBox = "<div id='divSelectAll' class='col-sm-6 col-md-4' style='margin-left: 25px;'> <div class='pull-left'><div class='checkbox-custom checkboxTiny checkbox-success'><input type='checkbox' onchange='ReviewOfSystemsTemplateDetail.selectAllCharC(this);'/><label class='control-label'><strong>Select All</strong></label></div></div></div>";

                $ROSSystemsCharacteristicsUL.empty();
                $ROSSystemsCharacteristicsUL.append(HTMLSystemCharacteristicsCached);
                $charcSystemDiv.html($ROSSystemsCharacteristicsUL[0].outerHTML);
                $charcSystemDiv.prepend(HTMLSelectAllCheckBox);

                utility.bindMyJSONByName(true, JSON.parse(HTMLSystemCharacteristics_JSON), false, $charcSystemDiv);

                return true;
            }
        }
        return false;
    },

    // Caches all Characteristics details info for a selected system
    CacheCharacteristicInfo: function (ROSSystemId, SystemName, ROSTemplateId) {
        var indexCh = -1;
        var CharcUl = '#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ROSSystemsCharacteristicsUL';
        var DetailData = $(CharcUl).html();

        if (DetailData != null && DetailData != '' && ROSSystemId != null && SystemName != null) {

            $.grep(ReviewOfSystemsTemplateDetail.CharacteristicsInfo, function (item, index) {
                if (item.ROSSystemId == ROSSystemId) {
                    indexCh = index;
                    return;
                }
            });

            if (indexCh != -1) {
                ReviewOfSystemsTemplateDetail.CharacteristicsInfo[indexCh].HTMLDetail = DetailData;
                ReviewOfSystemsTemplateDetail.CharacteristicsInfo[indexCh].SystemName = SystemName;
                ReviewOfSystemsTemplateDetail.CharacteristicsInfo[indexCh].DetailJSON = $(CharcUl).getMyJSONByName();
            }
            else {

                var CharsInfo = {
                    ROSSystemId: ROSSystemId,
                    SystemName: SystemName,
                    ROSTemplateId: ROSTemplateId,
                    DetailJSON: $(CharcUl).getMyJSONByName(),
                    HTMLDetail: $(CharcUl).html()
                };
                ReviewOfSystemsTemplateDetail.CharacteristicsInfo.push(CharsInfo);
            }
        } else {
            ReviewOfSystemsTemplateDetail.CharacteristicsInfo = [];
        }
    },

    //***************************************************************
    //******************** DB CALLS *********************************
    //***************************************************************

    // DB call to load all Systems
    // Author: ZeeshanAK | Date: February 29, 2016 
    getROSSystems_DBCall: function () {
        var objData = new Object();
        if (ReviewOfSystemsTemplateDetail.params.ROSTemplateId == null || ReviewOfSystemsTemplateDetail.params.ROSTemplateId == '') {
            ReviewOfSystemsTemplateDetail.params.ROSTemplateId = -1;
        }
        objData["ROSTemplateId"] = ReviewOfSystemsTemplateDetail.params.ROSTemplateId;
        objData["commandType"] = "LOAD_ROS_TEMPLATE_SYSTEMS";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemTemplate", "ReviewOfSystemTemplate");
    },

    // DB call to load all Characteristics for a specific System
    // Author: ZeeshanAK | Date: March 01, 2016 
    getROSSystemsCharacteristics_DBCall: function (SystemId, ROSTemplateId) {
        var objData = new Object();
        objData["ROSTemplateId"] = ROSTemplateId == null ? -1 : ROSTemplateId;

        objData["ROSSystemId"] = SystemId;
        objData["commandType"] = "load_ros_systems_characteristics";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemTemplate", "ReviewOfSystemTemplate");
    },

    /*  Author: Muhammad Azhar Shahzad
  Purpose: for Grid Load of Ros template
  Creation Date: March 02,2016 */
    searchROSTemplate_DBCall: function (ROSTemplateId) {
        var objData = {};
        objData["ROSTemplateId"] = ROSTemplateId == null ? -1 : ROSTemplateId;
        objData["commandType"] = "fill_ros_systems_template";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemTemplate", "ReviewOfSystemTemplate");
    },

    saveROSTemplate_DbCall: function (myJSON, ROSTemplateId, isSaveAS) {

        var objCharCList = [];
        var objSystemList = [];
        //Getting Systems Selected List
        var TemplateSystemsList = $.grep($('#' + ReviewOfSystemsTemplateDetail.params.PanelID + " div#divROSSystemsUL ul li"), function (item) {
            return $(item).find('input[type=checkbox]').is(':checked');
        });

        $(TemplateSystemsList).each(function (index, item) {
            var objDetail = {};
            objDetail["ROSSystemId"] = ROSTemplateId > 0 ? $(item).attr('id') : -(index + 1);
            objDetail["SystemName"] = $(item).find('label').text();
            objDetail["SortingOrder"] = index;
            objDetail["ROSTemplateId"] = ROSTemplateId;
            objSystemList.push(objDetail);
        });
        // end Getting Systems Selected List

        //Getting Characteristics List from Cache
        $(ReviewOfSystemsTemplateDetail.CharacteristicsInfo).each(function (index, item) {
            $(item.HTMLDetail).find('[type=checkbox]').each(function (i, elem) {
                var SortingOrder = i;
                $.each(JSON.parse(item.DetailJSON), function (indx, element) {

                    if (indx == $(elem).attr('name') && element) {
                        var objDetail = {};
                        objDetail["CharacteristicsId"] = $(elem).closest('li').attr('id');
                        objDetail["CharacteristicsName"] = $(elem).closest('li').find('span').text();
                        objDetail["ROSSystemId"] = ROSTemplateId > 0 ? item.ROSSystemId : -(SortingOrder + 1);
                        objDetail["SystemName"] = item.SystemName;
                        objDetail["ROSTemplateId"] = item.ROSTemplateId;
                        objDetail["SortingOrder"] = SortingOrder;
                        objCharCList.push(objDetail);
                        SortingOrder++;
                    }
                });

            });

        });

        var objData = {};
        objData["TemplateName"] = $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #txtTemplateName').val();

        if (globalAppdata['AppUserName'] == DefaultUser) {
            objData["EntityId"] = $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlEntity').val();
        } else {
            objData["EntityId"] = null;
        }
        //System and Characteristics List
        objData["SystemsList"] = objSystemList;
        objData["CharacteristicsList"] = objCharCList;
        //******************** Getting Specialty and Provider values********************
        var SpecialtyObj = '#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlSpecialty';
        if ($(SpecialtyObj).val() == null || ($(SpecialtyObj).val() != null && $(SpecialtyObj).val().length == $(SpecialtyObj + ' option').length)) {
            objData["IsSpecialityAll"] = true;
        } else {
            objData["IsSpecialityAll"] = false;
            //objData["SpecialityIds"] = $(SpecialtyObj).val().join();
            objData["SpecialityNames"] = $(SpecialtyObj + ' :selected').map(function (i, item) { return $(item).text() }).get().join();
        }
        var ProviderObj = '#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlProvider';
        if ($(ProviderObj).val() == null || ($(ProviderObj).val() != null && $(ProviderObj).val().length == $(ProviderObj + ' option').length)) {
            objData["IsProviderAll"] = true;
        } else {
            objData["IsProviderAll"] = false;
            //objData["ProviderIds"] = $(ProviderObj).val().join();
            objData["ProviderNames"] = $(ProviderObj + ' :selected').map(function (i, item) { return $(item).text() }).get().join();
        }


        //-----------------------------


        //------------------------------------------------------

        var SpecialtyIds = $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlSpecialty option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objData["SpecialityIds"] = SpecialtyIds;

        var ProviderIds = $('#' + ReviewOfSystemsTemplateDetail.params.PanelID + ' #ddlProvider option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objData["ProviderIds"] = ProviderIds;

        //------------------------------------------------------


        //-----------------------------

        //******************** end Getting Specialty and Provider values********************
        objData["ROSTemplateId"] = ROSTemplateId;
        objData["isSaveAS"] = isSaveAS == null ? false : true;
        objData["commandType"] = "SAVE_ROSTemplate";
        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "ReviewOfSystemTemplate", "ReviewOfSystemTemplate");
    },
    //***************************************************************
    //******************** END OF DB CALLS  *************************
    //***************************************************************
}