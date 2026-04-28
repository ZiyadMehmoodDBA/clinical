Favorite_LabOrderDetail = {
    bIsFirstLoad: true,
    params: [],
    Switch: 1,
    CPTData: [],
    FavLabOrder: [],
    BioRefLabId: "",
    providerCheckedIds: [],
    ProviderIds: '',
    //Author: Abid Ali
    //Date: 31-03-2016
    //Logic to Load Lab Order
    Load: function (params) {
        Favorite_LabOrderDetail.params = params;

        // This logic is temporarily implemented, search only works for BioReference Compendiums. 
        MDVisionService.lookups("GetLabs", true, "IsActive=").done(
            function (response) {
                var resp = JSON.parse(response.GetLabs);
                $.each(resp, function (i, item) {
                    if (item.Name == "BioReference Laboratories") {
                        Favorite_LabOrderDetail.BioRefLabId = item.Value;
                    }
                });
            });


        if (Favorite_LabOrderDetail.params.PanelID != 'pnlFavoriteLabOrderDetail') {
            Favorite_LabOrderDetail.params.PanelID = Favorite_LabOrderDetail.params.PanelID + ' #pnlFavoriteLabOrderDetail';
        } else {
            Favorite_LabOrderDetail.params.PanelID = 'pnlFavoriteLabOrderDetail';
        }
        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Favorite_LabOrderDetail.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        if (globalAppdata['AppUserName'] == DefaultUser) {
            $('#' + Favorite_LabOrderDetail.params.PanelID + ' #frmFavoriteLabOrderDetail #divEntity').show();
        }
        var self = $('#' + Favorite_LabOrderDetail.params.PanelID);
        self.loadDropDowns(true).done(function () {

            // Favorite_LabOrderDetail.loadFavoriteLabOrder();
            selectedEntity = globalAppdata["SeletedEntityId"];
            $.when(Favorite_LabOrderDetail.loadEntityProvider(selectedEntity)).then(function () {
                Favorite_LabOrderDetail.LoadLabs('ddlLabId', Favorite_LabOrderDetail.params.PanelID).done(function () {
                    if (Favorite_LabOrderDetail.params.mode == "Add") {
                    }
                    else if (Favorite_LabOrderDetail.params.mode == "Edit") {
                        $('#' + Favorite_LabOrderDetail.params.PanelID + ' #lblHeading').html('Edit Favorites List');
                        Favorite_LabOrderDetail.fillFavoriteLabOrder(Favorite_LabOrderDetail.params.FavoriteLabOrderId, Favorite_LabOrderDetail.params.FavoriteListType);
                    }
                });
            });


        });

        Favorite_LabOrderDetail.validateLabOrderDetail();

        $(function () {
            $('[data-plugin-toggle]').each(function () {
                var $this = $(this),
                    opts = {};

                var pluginOptions = $this.data('plugin-options');
                if (pluginOptions)
                    opts = pluginOptions;

                $this.themePluginToggle(opts);
            });
        });

        $(document).ready(function () {

            (function ($) {
                'use strict';
                $(function () {
                    $('[data-plugin-ios-switch]').each(function () {
                        var $this = $(this);

                        $this.themePluginIOS7Switch();
                    });
                });
            }).apply(this, [jQuery]);


        });
    },
    CheckProviderValidation: function () {
        var self = $("#" + Favorite_LabOrderDetail.params.PanelID);
        var ProviderIds = self.find('#ddlLabOrderProvider option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        var strProviderIds = ProviderIds;
        Favorite_LabOrderDetail.providerCheckedIds = ProviderIds;
        if (Favorite_LabOrderDetail.providerCheckedIds != '') {
            Favorite_LabOrderDetail.validateProvider(2);
        } else {
            Favorite_LabOrderDetail.validateProvider(1);
        }
    },
    validateProvider: function (operationid) {
        var self = "#" + Favorite_LabOrderDetail.params.PanelID + " #frmFavoriteLabOrderDetail";
        $(self + " #divFavLabOrderProvider").find("i").remove();
        if (operationid == 1) {
            $(self + " .multiselect").css("border-color", "#cc2724");
            $(self + " #divFavLabOrderProvider").find(".control-label").css("color", "#cc2724");
            $(self + " #divFavLabOrderProvider").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-remove" data-bv-icon-for="Race" style="display: block;"></i>');
        } else if (operationid == 2) {
            $(self + " .multiselect").css("border-color", "#3c763d");
            $(self + " #divFavLabOrderProvider").find(".control-label").css("color", "#3c763d");
            $(self + " #divFavLabOrderProvider").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-ok" data-bv-icon-for="Race" style="display: block;"></i>');
        } else {
            $(self + " .multiselect").css("border-color", "#ccc");
            $(self + " #divFavLabOrderProvider").find(".control-label").css("color", "#000000");
        }
    },
    loadEntityProvider: function (entityId) {
        var objDeffered = $.Deferred();
        var data = "entityID=" + entityId;
        if (entityId != null && entityId > 0) {

            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $('#' + Favorite_LabOrderDetail.params.PanelID + ' #ddlLabOrderProvider');
                var $providerHiddenDdl = $('#' + Favorite_LabOrderDetail.params.PanelID + ' #ddlHiddenFavLabOrderProvider');

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
                if (Favorite_LabOrderDetail.ProviderIds != '') {
                    var Providers = Favorite_LabOrderDetail.ProviderIds.split(",");
                    Favorite_LabOrderDetail.providerCheckedIds = Providers;
                    $('#' + Favorite_LabOrderDetail.params.PanelID + ' #ddlLabOrderProvider').val(Providers);
                }

            }).then(function () {
                //Intialized in onhidden spacialty ddl.
                Favorite_LabOrderDetail.IntializeMultiSelectDropDownProviders();
                objDeffered.resolve();

            });
        }
        else {
            objDeffered.resolve();
        }
        return objDeffered;
    },
    IntializeMultiSelectDropDownProviders: function () {
        $('#' + Favorite_LabOrderDetail.params.PanelID + ' #ddlLabOrderProvider').multiselect('destroy');
        $('#' + Favorite_LabOrderDetail.params.PanelID + ' #ddlLabOrderProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 200,
            nonSelectedText: 'Select',
            selectAll: false,
            onDropdownHide: function (event) {
            },
            onChange: function () {
                Favorite_LabOrderDetail.CheckProviderValidation();
            },
        });
    },

    //Author: Abid Ali
    //Date: 31-03-2016
    //Logic search Validate Lab Order
    validateLabOrderDetail: function () {
        var self = $("#" + Favorite_LabOrderDetail.params.PanelID + " #frmFavoriteLabOrderDetail");

        self.bootstrapValidator({
            live: 'disabled',
            message: 'This value is not valid',
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {

                FavoriteListName: {
                    group: '.col-sm-10',
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
                LabId: {
                    group: '.col-sm-8',
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
                FavoriteTypeId: {
                    group: '.col-sm-10',
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
                Diagnosis: {
                    group: '.col-sm-10',
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
            if (Favorite_LabOrderDetail.CPTData.length > 0) {
                //Allow to be submited
                Favorite_LabOrderDetail.favLabOrderSave();
            }
            else
                self.data('bootstrapValidator').enableFieldValidators('Diagnosis', true);
        })
            .on('error.form.bv', function (e) {
                e.preventDefault();

                if (Favorite_LabOrderDetail.CPTData.length > 0 && self.find('#txtFavoriteListName').val() != "") {

                    //Disable validator on procedure
                    self.data('bootstrapValidator').enableFieldValidators('Diagnosis', false);
                    self.trigger('success.form.bv');
                }
                else {
                    //Disable validator if cpt has data
                    if (Favorite_LabOrderDetail.CPTData.length > 0)
                        self.data('bootstrapValidator').enableFieldValidators('Diagnosis', false);
                }

            });
    },

    //Author: Abid Ali
    //Date: 31-03-2016
    //Logic implimented to bind LOINIC code to Procedure field of Lab Order Detail
    openCPTCode: function (element) {
  
        var labId = $('#frmFavoriteLabOrderDetail #ddlLabId').val();
        if (labId != '') {
            var params = [];
            params["FromAdmin"] = Favorite_LabOrderDetail.params["FromAdmin"];
            params["ParentCtrl"] = 'Favorite_LabOrderDetail';

            LoadActionPan('Clinical_LOINC', params);
        }
        else {
            utility.DisplayMessages("Please Select Lab First !", 2);
        }
    },

    //Called from LOINIC Control to pass code and description as Json obj
    pushLOINCAsCpt: function (JsonObj) {

       var observation = JsonObj["Observation"];
       var LOINCCOde =  JsonObj['LOINICCODE'];
       var LOINCDescription = JsonObj['LOINICDescription'];
       var OrderTestLOINCId = JsonObj['OrderTestLOINCId'];
       
       Favorite_LabOrderDetail.bindToCPTList(LOINCCOde, LOINCDescription, OrderTestLOINCId);


    },
    //Author: Abid Ali
    //Date: 31-03-2016
    //Logic implimented to bind CPT code to Procedure field of Lab Order Detail
    bindAutoComplete: function (element) {
        var labId = $('#frmFavoriteLabOrderDetail #ddlLabId').val();
        var ddlFavoriteTypeId = $('#frmFavoriteLabOrderDetail #ddlFavoriteTypeId').val();
        if (labId != '' && ddlFavoriteTypeId != '') {
            var CodeSystemType = $('#pnlFavoriteLabOrderDetail #ddlLabId option:selected').attr('CodeSystem');
            EMRUtility.BindLOINCCodes(element, "Favorite_LabOrderDetail", labId,"",CodeSystemType);
        }
        else {
            if(labId == ''){
                utility.DisplayMessages("Please Select Lab First !", 2);
            }
            if (ddlFavoriteTypeId == '') {
                utility.DisplayMessages("Please Select Favorite List Type First !", 2);
            }
        }
    },
    //Author: Abid Ali
    //Date: 31-03-2016
    //Logic implimented to bind CPT code to Procedure field of Lab Order Detail
    bindToCPTList: function (cptCode, cptDescription, OrderTestLOINCId, FavoriteListCPTId, LabId, LabName, modifier) {

        if (!Favorite_LabOrderDetail.isInCPTList(cptCode, cptDescription)) {
            modifier = modifier == null ? '' : modifier;
            var item = {};
            item["CPTCode"] = cptCode;
            item["CPTCodeDescription"] = cptDescription;
            item["OrderTestLOINCId"] = OrderTestLOINCId;
            item["FavoriteListCPTId"] = FavoriteListCPTId ? FavoriteListCPTId : '';
            item["Modifier"] = modifier;
            item["LabId"] = LabId == null ?  $('#frmFavoriteLabOrderDetail #ddlLabId').val() : LabId;
            Favorite_LabOrderDetail.CPTData.push(item);

            var $list = $("#" + Favorite_LabOrderDetail.params.PanelID + " #ulFavLabOrderDisease");
            var currId = -1;
            $($list).find('li').each(function (i, item) {
                currId = $(this).attr("id");
            });
            currId = parseInt(currId) + (-1);

            cptDescArg = cptDescription;

            if (cptDescArg != "") {
                cptDescArg = ",'" + cptDescArg.trim() + "'";
            }
            var onclick = "Favorite_LabOrderDetail.deleteCPTFromCPTData(this,'" + cptCode + "','" + OrderTestLOINCId + "','" + FavoriteListCPTId + "');";
            var modifierCtrlId = 'txtModifier' + currId;
            var AutoCompleteSearch = "utility.BindAutoCompleteText(this, 'COMMON_CODE', 'GET_MODIFIER_CODE', '#" + modifierCtrlId + "', '',false,0);";
            var onBlur = "utility.ValidateCode(this, 'Modifier','" + modifierCtrlId + "');";
            var li = '';
            if ((LabName != null && LabName.length > 0) || $('#frmFavoriteLabOrderDetail #ddlFavoriteTypeId').val() == "LabOrderGroup") {
                LabName = (LabName != null && LabName.length > 0) > 0 ? LabName : $('#frmFavoriteLabOrderDetail #ddlLabId option:selected').text();
                li = '<li id=' + currId + ' value =' + cptCode + '  cptdesc="' + cptDescription + '" >' +
            '<span class="pull-left pr-xlg">' + cptCode + " - " + cptDescription + ' (<strong>' + LabName + '</strong>) </span>' +
            '<div class="clearfix"></div><div class="col-xs-2">Modifier</div><div class="col-xs-6">' +
           '<input id="' + modifierCtrlId + '" name="Modifier" type="text" oninput="' + AutoCompleteSearch + '" onblur="' + onBlur + '" class="form-control input-block p-tiny ml-xs" value="' + modifier + '"></div></div>'+
                       '<span class="removeIconListHover">' +
                       '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="' + onclick + '"><i class="fa fa-times red"></i></a>' +
                       '</span>' +
                       '<div class="clearfix"></div>' +
                       '</li>';
            } else {
                li = '<li id=' + currId + ' value =' + cptCode + ' cptdesc="' + cptDescription + '">' +
            '<span class="pull-left pr-xlg">' + cptCode + " - " + cptDescription + '</span>' +
                        '<div class="clearfix"></div><div class="col-xs-2">Modifier</div><div class="col-xs-6">' +
           '<input id="' + modifierCtrlId + '" name="Modifier" type="text" oninput="' + AutoCompleteSearch + '" onblur="' + onBlur + '" class="form-control input-block p-tiny ml-xs" value="' + modifier + '"></div></div>' +

                       '<span class="removeIconListHover">' +
                       '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="' + onclick + '"><i class="fa fa-times red"></i></a>' +
                       '</span>' +
                       '<div class="clearfix"></div>' +
                       '</li>';
            }

            $list.append(li);
            Favorite_LabOrderDetail.EnableDisableDropDownLaboratory();
        }
        else {
            utility.DisplayMessages("Test is already selected", 2);
        }

        $("#" + Favorite_LabOrderDetail.params.PanelID + " #txtDiagnosis").blur(function () {
            $(this).val('');
        });

    },
    EnableDisableDropDownLaboratory: function () {
        var $ddlLab = $("#" + Favorite_LabOrderDetail.params.PanelID + ' #ddlLabId');
        var $ddlFavoriteTypeId = $("#" + Favorite_LabOrderDetail.params.PanelID + ' #ddlFavoriteTypeId');
        if ($ddlLab.val() != '' && $ddlFavoriteTypeId.val() != 'LabOrderGroup') {
            $ddlLab.addClass('disableAll');
        }
        if ($ddlLab.val() != '') {
            $ddlFavoriteTypeId.addClass('disableAll');
        }
    },

    //Author: Abid Ali
    //Date: 31-03-2016
    //Delete CPT from CPTData Json Object
    deleteCPTFromCPTData: function (obj, cptCode, OrderTestLOINCId, FavoriteListCPTId) {

        OrderTestLOINCId = typeof OrderTestLOINCId == "undefined" ? "" : OrderTestLOINCId.toString();
        FavoriteListCPTId = typeof FavoriteListCPTId == "undefined" ? "" : FavoriteListCPTId.toString();
        $.each(Favorite_LabOrderDetail.CPTData, function (index, item) {
            if (item["CPTCode"] == cptCode.toString() && (item["OrderTestLOINCId"] == OrderTestLOINCId || item["FavoriteListCPTId"] == FavoriteListCPTId)) {

                Favorite_LabOrderDetail.CPTData.splice(index, 1);
                $(obj).parent().parent().remove();
                if (Favorite_LabOrderDetail.params.FavoriteLabOrderId && Favorite_LabOrderDetail.params.FavoriteLabOrderId>0 && typeof FavoriteListCPTId != typeof undefined) {
                    Favorite_LabOrderDetail.deleteCPT_DB_Call(Favorite_LabOrderDetail.params.FavoriteLabOrderId, FavoriteListCPTId);
                }
                return false;
            }
        });
        if ($("#" + Favorite_LabOrderDetail.params.PanelID + " #ulFavLabOrderDisease li").length > 0 && $("#" + Favorite_LabOrderDetail.params.PanelID + " #ddlFavoriteTypeId").val() != "LabOrderGroup") {
            $('#' + Favorite_LabOrderDetail.params.PanelID + ' #ddlLabId').addClass('disableAll');
        }
        else {
            $('#' + Favorite_LabOrderDetail.params.PanelID + ' #ddlLabId').removeClass('disableAll');
        }
    },
    deleteCPT_DB_Call: function (FavouriteListId, FavoriteListCPTId) {

        var objData = {};

        objData["FavoriteListId"] = FavouriteListId == null ? 0 : FavouriteListId;
        objData["FavoriteListCPTId"] = FavoriteListCPTId == null ? 0 : FavoriteListCPTId;
        objData["commandType"] = "delete_favoritelist_cpt";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },
    //Author: Abid Ali
    //Date: 31-03-2016
    //Finds cpt code and discription in CPTData array
    isInCPTList: function (cptCode, cptDescription) {
        var isExists = false;
        $.each(Favorite_LabOrderDetail.CPTData, function (index, item) {

            if (item["CPTCode"] == cptCode && item["CPTCodeDescription"] == cptDescription) {
                isExists = true;
                return false;
            }
        });
        return isExists;
    },

    UnLoadTab: function () {
        Favorite_LabOrderDetail.CPTData = [];
        var objDeffered = $.Deferred();
        if (Favorite_LabOrderDetail.params["FromAdmin"] == "0") {
            if (Favorite_LabOrderDetail.params != null && Favorite_LabOrderDetail.params.ParentCtrl != null) {
                UnloadActionPan(Favorite_LabOrderDetail.params.ParentCtrl, 'Favorite_LabOrderDetail');
            }
            else
                UnloadActionPan(null, 'Favorite_LabOrderDetail');
        }
        else {
            RemoveAdminTab();
        }
        objDeffered.resolve();
        return objDeffered;
    },

    BindICD9AutoComplete: function (element) {
        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "Favorite_LabOrderDetail", null, false);
    },
    getUnitsAndModifiers: function () {
        var $list = $("#" + Favorite_LabOrderDetail.params.PanelID + " #ulFavLabOrderDisease li");
        for (var i = 0; i < Favorite_LabOrderDetail.CPTData.length; i++) {
            for (var j = 0; j < $list.length; j++) {
                var cptCode = $($list[j]).attr('value');
                var desc = $($list[j]).attr('cptdesc');
                if (cptCode && Favorite_LabOrderDetail.CPTData[i].CPTCode == cptCode && Favorite_LabOrderDetail.CPTData[i].CPTCodeDescription == desc) {
                    Favorite_LabOrderDetail.CPTData[i].Modifier = $($list[j]).find("input[id^='txtModifier']").val();
                    break;
                }
            }
        }
    },
    favLabOrderSave: function (ComeFromDelete, ComeFromUnLoad) {
        var strMessage = "";
        if (Favorite_LabOrderDetail.CPTData.length > 0) {
            Favorite_LabOrderDetail.getUnitsAndModifiers();
            AppPrivileges.GetFormPrivileges("Favorites_Lab Order", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Favorite_LabOrderDetail.saveFavLab().done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Favorite_LabOrderDetail.UnLoadTab();
                            var self = $('#' + Favorite_LabOrderDetail.params.PanelID + ' #frmFavoriteLabOrderDetail');
                            self.find('#hfFavoriteListId').val(response.FavCPTId);
                            Favorite_LabOrder.favoriteListSearch();
                            if (!ComeFromDelete) {
                                utility.DisplayMessages(response.Message, 1);
                            }
                        }
                        else {
                            if (!ComeFromDelete) {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        }
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else {
            utility.DisplayMessages("There is no Favorite Lab to add.", 3);
        }

    },   

    //Author: Abid Ali
    //Date: 31-03-2016
    AddInArray: function (id, ICD9, ICD10, SNOMED, ICD10Des, IsSelect) {
        var item = {};
        item["DiagnosisId"] = id;
        item["ICD9"] = ICD9;
        item["ICD10"] = ICD10;
        item["ICD10Description"] = ICD10Des
        item["SNOMED"] = SNOMED;
        Favorite_LabOrderDetail.FavLabOrder.push(item);        

    },
    LoadLabs: function (ddlId, controlPanelID) {

        return Clinical_Lab.searchLab(null, 0, 1, 5000).done(function (response) {
            //Populate Distinct Values in typeArray
            response = JSON.parse(response);
            var NameArray = new Array();
            var labIds = new Array();
            var codeSystem = new Array();
            var data = JSON.parse(response.ClinicalLab_JSON);
            $.each(data, function (i, row) {
                if (jQuery.inArray(row.Name, NameArray) === -1) {
                    NameArray.push(row.Name);
                    labIds.push(row.LabId);
                    codeSystem.push(row.CodeSystemId); // Code System 1 means CPT , 2 Means Lab Codes
                }
            });
            var ddType = $('#' + controlPanelID + " #" + ddlId);
            ddType.empty();
            ddType.append($("<option />").val("").text('-Select-'));
            if (NameArray.length > 0) {
                $.each(NameArray, function (index, Name) {
                    ddType.append($("<option />").val(labIds[index]).text(Name).attr('CodeSystem', codeSystem[index]));
                });
            }
        });
    },

    //Author: Abid Ali
    //Date: 31-03-2016
    saveFavLab: function () {
        var objData = {};
        var self = $('#'+Favorite_LabOrderDetail.params.PanelID);
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objDetail = JSON.parse(myJSON);
        if (objDetail["ProviderIds"] != '') {
            Favorite_LabOrderDetail.validateProvider(2);
            objData["FavoriteListId"] = self.find('#hfFavoriteListId').val();;
            objData["ListType"] = $("#" + Favorite_LabOrderDetail.params.PanelID + ' #ddlFavoriteTypeId').val();//"LabOrder";
            objData["EntityId"] = objDetail["EntityId"];
            objData["IsActive"] = objDetail.Active;
            objData["FavoriteListName"] = objDetail["FavoriteListName"];
            if (globalAppdata['AppUserName'] != DefaultUser) {
                objData["EntityId"] = -1;
            }
            objData["ProviderIds"] = objDetail["ProviderIds"];
            objData["LabId"] = objDetail["LabId"];
            objData["FavoriteListCPT"] = Favorite_LabOrderDetail.CPTData;
            objData["commandType"] = "save_favprocedureorder";
            var data = JSON.stringify(objData);
            return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
        }
        else {
            Favorite_LabOrderDetail.validateProvider(1);
        }
    },

    //Author: Abid Ali
    //Date: 31-03-2016
    //Description:  fills favorite Lab
    fillFavoriteLabOrder: function (FavoriteListId,ListType, LabId) {
        Favorite_LabOrderDetail.fillFavoriteLabOrder_DBCall(FavoriteListId,null, ListType, LabId).done(function (response) {
            response = JSON.parse(response);
            //   console.log(response);
            if (response.status != false) {

                var self = $('#' + Favorite_LabOrderDetail.params.PanelID + ' #frmFavoriteLabOrderDetail');
                var favListDetail = JSON.parse(response.FavoriteListJSON)[0];
                //Bind to form
                utility.bindMyJSONByName(true, favListDetail, false, self).done(function () {
                    var isActive = false;
                    if (favListDetail.IsActive === "True")
                        isActive = true;
                    self.find('#Active').attr('checked', isActive);
                    self.find('#txtFavoriteListName').val(favListDetail.Name);
                    self.find('#ddlFavoriteTypeId').val(favListDetail.ListType);
                    //Load CPT against favList
                    var FavoriteListProvidersJSON = JSON.parse(response.FavoriteListProviders_JSON);
                    if (FavoriteListProvidersJSON != '' && FavoriteListProvidersJSON) {
                        $('#' + Favorite_LabOrderDetail.params.PanelID + " #ddlLabOrderProvider").val(FavoriteListProvidersJSON);

                        $('#' + Favorite_LabOrderDetail.params.PanelID + " #ddlLabOrderProvider").multiselect("refresh");
                        $('#' + Favorite_LabOrderDetail.params.PanelID + " #ddlLabOrderProvider").multiselect({
                            includeSelectAllOption: true,
                            enableFiltering: true,
                            enableCaseInsensitiveFiltering: true,
                            maxHeight: 247
                        });
                        Favorite_LabOrderDetail.providerCheckedIds = FavoriteListProvidersJSON;
                        // Patient_Demographic.multipleRaceIds = demographic_detail.strRaceIds;
                    } else {
                        $('#' + Favorite_LabOrderDetail.params.PanelID + " #ddlLabOrderProvider").find("option:selected").removeAttr("selected");
                        $('#' + Favorite_LabOrderDetail.params.PanelID + " #ddlLabOrderProvider").multiselect("refresh");
                    }
                    Favorite_LabOrderDetail.favoriteList_CPTSearch(JSON.parse(response.FavoriteListJSON)[0].FavoriteListId);
                });

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    //Author: Abid Ali
    //Date: 31-03-2016
    //Description:  searches favorite Lab
    favoriteList_CPTSearch: function (FavoriteListId, FavoriteListCPTId, PageNo, rpp) {
        Favorite_LabOrder.searchFavoriteList_CPT_DBCall(FavoriteListCPTId, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                //Bind to list
                $.each(JSON.parse(response.FavoriteListCPTJSON), function (index, CPTRow) {
                    CPTRow.CPTCodeDescription = CPTRow.CPTCodeDescription.replace(/&#39;/g, "'");
                    Favorite_LabOrderDetail.bindToCPTList(CPTRow.CPTCode, CPTRow.CPTCodeDescription, CPTRow.OrderTestLOINCId, CPTRow.FavoriteListCPTId, CPTRow.LabId, CPTRow.LabName, CPTRow.Modifier);
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    //Author: Abid Ali
    //Date: 31-03-2016
    //Description:  fills favorite Lab
    fillFavoriteLabOrder_DBCall: function (FavoriteListId, FavoriteListICDId, ListType, LabId) {

        var objData = {};
        // objData["IsActive"] = null;
        objData["FavoriteListId"] = FavoriteListId == null ? -1 : FavoriteListId;
        objData["FavoriteListICDId"] = FavoriteListICDId == null ? -1 : FavoriteListICDId;
        objData["ListType"] = ListType; //'LabOrder';
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["LabId"] = LabId == null ? objData["LabId"] = Favorite_LabOrderDetail.params.LabId : LabId;
        if (Favorite_LabOrder.Switch == 1) {
            objData["IsActive"] = true
        }
        else {
            objData["IsActive"] = false;
        }

        objData["PageNumber"] = -1;
        objData["RowsPerPage"] = -1;
        objData["commandType"] = "load_favoritelist";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },

    //Author: Abid Ali
    //Date: 31-03-2016
    //Description:  favorite Lab ICD
    FavoriteListICD: function (response) {
        if (response.FavoriteListICDCount > 0) {
            var FavoriteListJSON = JSON.parse(response.FavoriteListICDJSON);
            $.each(FavoriteListJSON, function (i, item) {
                if (i <= 1) {
                    if (item.IsActive == "True") {
                        $('#' + Favorite_LabOrderDetail.params.PanelID + ' #frmFavoriteLabOrderDetail #IsActive').prop('checked', true);
                    } else {
                        $('#' + Favorite_LabOrderDetail.params.PanelID + ' #frmFavoriteLabOrderDetail #IsActive').prop('checked', false);
                    }
                    $('#' + Favorite_LabOrderDetail.params.PanelID + ' #frmFavoriteLabOrderDetail #txtFavoriteListName').val(item.FavoriteListName);
                    $('#' + Favorite_LabOrderDetail.params.PanelID + ' #frmFavoriteLabOrderDetail #lstEntityId').val(item.EntityId);

                }
                Favorite_LabOrderDetail.AddInArray(item.FavoriteListICDId, item.ICD9Code, item.ICD10Code, item.SNOMEDID, item.ICD10CodeDescription, true, item.FavoriteListICDId);
                var li = "<li  id=" + item.FavoriteListICDId + " ><a href='#' class='pr-sm'>" + item.ICD10CodeDescription + "<span class='removeIconListHover' onclick='Favorite_Complaints_Detail.deleteFavChiefComplaint(" + item.FavoriteListICDId + ",event);'><i class='fa fa-times'></i></span></a></li>"
                $('#' + Favorite_LabOrderDetail.params.PanelID + ' #ulFavCompliantDisease').append(li);
            });
        }
    },

}