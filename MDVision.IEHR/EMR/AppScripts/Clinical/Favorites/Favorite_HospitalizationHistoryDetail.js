
Favorite_HospitalizationHistoryDetail = {
    bIsFirstLoad: true,
    params: [],
    Switch: 1,
    CPTData: [],
    FavHospitalizationHistory: [],
    providerCheckedIds: [],
    ProviderIds: '',

    Load: function (params) {
        Favorite_HospitalizationHistoryDetail.params = params;
        Favorite_HospitalizationHistoryDetail.providerCheckedIds = [];
        if (Favorite_HospitalizationHistoryDetail.params.PanelID != 'pnlFavoriteHospitalizationHistoryDetail') {
            Favorite_HospitalizationHistoryDetail.params.PanelID = 'pnlFavoriteHospitalizationHistory #pnlFavoriteHospitalizationHistoryDetail';
        } else {
            Favorite_HospitalizationHistoryDetail.params.PanelID = 'pnlFavoriteHospitalizationHistoryDetail';
        }
        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Favorite_HospitalizationHistoryDetail.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        if (globalAppdata['AppUserName'] == DefaultUser) {
            $('#' + Favorite_HospitalizationHistoryDetail.params.PanelID + ' #frmFavoriteHospitalizationHistoryDetail #divEntity').show();
        }
        if (Favorite_HospitalizationHistoryDetail.params.mode == "Edit")
            $('#' + Favorite_HospitalizationHistoryDetail.params.PanelID + ' #lblHeading').html('Edit Favorites List');

        var self = $('#' + Favorite_HospitalizationHistoryDetail.params.PanelID);
        self.loadDropDowns(true).done(function () {
            Favorite_HospitalizationHistoryDetail.loadEntityProvider().done(function () {
                var favListId = Favorite_HospitalizationHistoryDetail.params.FavoriteListId
                if (favListId != null) {
                    $('#' + Favorite_HospitalizationHistoryDetail.params.PanelID + ' #frmFavoriteHospitalizationHistoryDetail #Active').attr('disabled', false);
                    self.find('#hfFavoriteListId').val(favListId);
                    if (favListId > 0) {
                        Favorite_HospitalizationHistoryDetail.favoriteListSearch(favListId);
                    }
                }
                else {
                    $('#' + Favorite_HospitalizationHistoryDetail.params.PanelID + ' #frmFavoriteHospitalizationHistoryDetail #Active').attr('disabled', true);
                }
                $('#' + Favorite_HospitalizationHistoryDetail.params.PanelID + ' #frmFavoriteHospitalizationHistoryDetail').data('serialize', $('#' + Favorite_HospitalizationHistoryDetail.params.PanelID + ' #frmFavoriteHospitalizationHistoryDetail').serialize());
            });
        });

        Favorite_HospitalizationHistoryDetail.validateFavoriteList();
        if (globalAppdata['AppUserName'] == DefaultUser) {
            $('#' + Favorite_HospitalizationHistoryDetail.params.PanelID + ' #frmFavoriteHospitalizationHistoryDetail').data('bootstrapValidator').enableFieldValidators('EntityId', true);
        }
       

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

    IntializeMultiSelectDropDownProviders: function () {
        $(' #frmFavoriteHospitalizationHistoryDetail #ddlProvider').multiselect('destroy');
        $(' #frmFavoriteHospitalizationHistoryDetail #ddlProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 200,
            nonSelectedText: 'Select',
            selectAll: false,
            onDropdownHide: function (event) {
            },
            onChange: function () {
                Favorite_HospitalizationHistoryDetail.CheckProviderValidation();
            },
        });
    },
    CheckProviderValidation: function () {
        var self = $("#" + Favorite_HospitalizationHistoryDetail.params.PanelID);
        var ProviderIds = self.find('#frmFavoriteHospitalizationHistoryDetail #ddlProvider option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        var strProviderIds = ProviderIds;
        Favorite_HospitalizationHistoryDetail.providerCheckedIds = ProviderIds;
        if (Favorite_HospitalizationHistoryDetail.providerCheckedIds != '') {
            Favorite_HospitalizationHistoryDetail.validateProvider(2);
        } else {
            Favorite_HospitalizationHistoryDetail.validateProvider(1);
        }
    },

    validateProvider: function (operationid) {
        var self = "#" + Favorite_HospitalizationHistoryDetail.params.PanelID + " #frmFavoriteHospitalizationHistoryDetail";
        $(self + " #divProvider").find("i").remove();
        if (operationid == 1) {
            $(self + " .multiselect").css("border-color", "#cc2724");
            $(self + " #divProvider").find(".control-label").css("color", "#cc2724");
            $(self + " #divProvider").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-remove" data-bv-icon-for="Race" style="display: block;"></i>');
        } else if (operationid == 2) {
            $(self + " .multiselect").css("border-color", "#3c763d");
            $(self + " #divProvider").find(".control-label").css("color", "#3c763d");
            $(self + " #divProvider").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-ok" data-bv-icon-for="Race" style="display: block;"></i>');
        } else {
            $(self + " .multiselect").css("border-color", "#ccc");
            $(self + " #divProvider").find(".control-label").css("color", "#000000");
        }
    },

    loadEntityProvider: function () {
        var objDeffered = $.Deferred();
        selectedEntity = globalAppdata["SeletedEntityId"];
        var data = "entityID=" + selectedEntity;
        if (selectedEntity != null && selectedEntity > 0) {

            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $(' #frmFavoriteHospitalizationHistoryDetail #ddlProvider');
                var $providerHiddenDdl = $('#' + Favorite_HospitalizationHistoryDetail.params.PanelID + ' #ddlHiddenProvider');

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
                if (Favorite_HospitalizationHistoryDetail.ProviderIds != '') {
                    var Providers = Favorite_HospitalizationHistoryDetail.ProviderIds.split(",");
                    Favorite_HospitalizationHistoryDetail.providerCheckedIds = Providers;
                    $('#' + Favorite_HospitalizationHistoryDetail.params.PanelID + ' #ddlProvider').val(Providers);
                }

            }).then(function () {
                //Intialized in onhidden spacialty ddl.
                Favorite_HospitalizationHistoryDetail.IntializeMultiSelectDropDownProviders();
                objDeffered.resolve();

            });
        }
        else {
            objDeffered.resolve();
        }
        return objDeffered;
    },
    //Function Name: validateFavoriteList
    //Author Name: Humaira Yousaf
    //Craeted Date: 31-03-2016
    //Description:  Validates favorite list
    validateFavoriteList: function () {
        var self = $('#' + Favorite_HospitalizationHistoryDetail.params.PanelID + ' #frmFavoriteHospitalizationHistoryDetail');

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
                Diagnosis: {
                    group: '.col-sm-10',
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
                EntityId: {
                    group: '.col-sm-10',
                    enabled: false,
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
            if (Favorite_HospitalizationHistoryDetail.CPTData.length > 0) {
                //Allow to be submited
                Favorite_HospitalizationHistoryDetail.favHospitalizationHistorySave();
            }
            else
                self.data('bootstrapValidator').enableFieldValidators('Diagnosis', true);
        })
            .on('error.form.bv', function (e) {
                e.preventDefault();

                if (Favorite_HospitalizationHistoryDetail.CPTData.length > 0 && self.find('#txtFavoriteListName').val() != "") {

                    //Disable validator on procedure
                    self.data('bootstrapValidator').enableFieldValidators('Diagnosis', false);
                    self.trigger('success.form.bv');
                }
                else {
                    //Disable validator if cpt has data
                    if (Favorite_HospitalizationHistoryDetail.CPTData.length > 0)
                        self.data('bootstrapValidator').enableFieldValidators('Diagnosis', false);
                }

            });
    },

    openSearchPopup: function (searchType, ctrl, hiddenCtrl) {
        var controlToLoad = "";
        if (searchType == "ICD") {
            controlToLoad = "Admin_IMOICD";
        }
        else if (searchType == "CPT") {
            controlToLoad = "Admin_IMOCPT";
        }
        else if (searchType == "Modifier") {
            controlToLoad = "Admin_Modifier";
        }
        $('#pnlFavoriteHospitalizationHistoryDetail #txtDiagnosis').attr('data-popupunload', 'true');
        var params = [];
        params["FromAdmin"] = "0";
        //params["Parentctrl"] = Clinical_MedicalHx.params["TabID"];
        if (Favorite_HospitalizationHistoryDetail.params.TabID == 'clinicalTabProgressNote') {
            params['FromProgressNote'] = 'pnlClinicalProgressNote';
            params["ParentCtrl"] = 'Favorite_HospitalizationHistoryDetail';
        }
        else
            params["ParentCtrl"] = 'Favorite_HospitalizationHistoryDetail';

        if (ctrl != null) {
            params["RefCtrl"] = ctrl;
        }
        if (hiddenCtrl != null) {
            params["RefHiddenCtrl"] = hiddenCtrl;
        }
        if (controlToLoad != "") {
            if (Favorite_HospitalizationHistoryDetail.params.TabID == 'clinicalTabProgressNote' && searchType == "ICD")
                LoadActionPan(controlToLoad, params, 'pnlClinicalProgressNote');
            else
                LoadActionPan(controlToLoad, params);
        }

    },


    //Function Name: favoriteListSearch
    //Author Name: Humaira Yousaf
    //Craeted Date: 31-03-2016
    //Description:  Searches favorite list

    bindICD9AutoComplete: function (element) {
        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "Favorite_HospitalizationHistoryDetail", null, false);
    },


    favoriteListSearch: function (FavoriteListId, ListType, PageNo, rpp) {

        Favorite_HospitalizationHistory.searchFavoriteList_DBCall(ListType, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                var self = $('#' + Favorite_HospitalizationHistoryDetail.params.PanelID + ' #frmFavoriteHospitalizationHistoryDetail');
                var favListDetail = JSON.parse(response.FavoriteListJSON)[0];
                //Bind to form
                utility.bindMyJSONByName(true, favListDetail, false, self).done(function () {
                    var isActive = false;
                    if (favListDetail.IsActive === "True")
                        isActive = true;
                    self.find('#Active').attr('checked', isActive);
                    self.find('#txtFavoriteListName').val(favListDetail.Name);
                    
                    var FavoriteListProvidersJSON = JSON.parse(response.FavoriteListProviders_JSON);
                    if (FavoriteListProvidersJSON != '' && FavoriteListProvidersJSON) {
                        $('#' + Favorite_HospitalizationHistoryDetail.params.PanelID + " #ddlProvider").val(FavoriteListProvidersJSON);

                        $('#' + Favorite_HospitalizationHistoryDetail.params.PanelID + " #ddlProvider").multiselect("refresh");
                        $('#' + Favorite_HospitalizationHistoryDetail.params.PanelID + " #ddlProvider").multiselect({
                            includeSelectAllOption: true,
                            enableFiltering: true,
                            enableCaseInsensitiveFiltering: true,
                            maxHeight: 247
                        });
                        Favorite_HospitalizationHistoryDetail.providerCheckedIds = FavoriteListProvidersJSON;
                        // Patient_Demographic.multipleRaceIds = demographic_detail.strRaceIds;
                    } else {
                        $('#' + Favorite_HospitalizationHistoryDetail.params.PanelID + " #ddlProvider").find("option:selected").removeAttr("selected");
                        $('#' + Favorite_HospitalizationHistoryDetail.params.PanelID + " #ddlProvider").multiselect("refresh");
                    }
                    Favorite_HospitalizationHistoryDetail.favoriteList_CPTSearch(JSON.parse(response.FavoriteListJSON)[0].FavoriteListId);
                    $('#' + Favorite_HospitalizationHistoryDetail.params.PanelID + ' #frmFavoriteHospitalizationHistoryDetail').data('serialize', $('#' + Favorite_HospitalizationHistoryDetail.params.PanelID + ' #frmFavoriteHospitalizationHistoryDetail').serialize());
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    //Function Name: favoriteList_CPTSearch
    //Author Name: Humaira Yousaf
    //Craeted Date: 31-03-2016
    //Description:  Searches favorite CPT list
    favoriteList_CPTSearch: function (FavoriteListId, FavoriteListCPTId, PageNo, rpp) {

        Favorite_HospitalizationHistory.searchFavoriteList_CPT_DBCall(FavoriteListCPTId, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                //Bind to list
                $.each(JSON.parse(response.FavoriteListICDJSON), function (index, CPTRow) {
                    Favorite_HospitalizationHistoryDetail.bindToCPTList(CPTRow.FavoriteListICDId, CPTRow.FavoriteListId, CPTRow.ICD9Code, CPTRow.ICD9CodeDescription, CPTRow.ICD10Code, CPTRow.ICD10CodeDescription, CPTRow.SNOMEDID, CPTRow.SNOMEDDescription);

                });
                $('#' + Favorite_HospitalizationHistoryDetail.params.PanelID + ' #frmFavoriteHospitalizationHistoryDetail').data('serialize', $('#' + Favorite_HospitalizationHistoryDetail.params.PanelID + ' #frmFavoriteHospitalizationHistoryDetail').serialize());
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    //Function Name: UnLoadTab
    //Author Name: Humaira Yousaf
    //Craeted Date: 31-03-2016
    //Description:  Unloads tab
    UnLoadTab: function (fromSave) {
        if (fromSave == "Save") {
            Favorite_HospitalizationHistoryDetail.CPTData = [];
            Favorite_HospitalizationHistoryDetail.Unload();
        }
        else if (EMRUtility.compareFormDataWithSerialized(Favorite_HospitalizationHistoryDetail.params.PanelID + ' #frmFavoriteHospitalizationHistoryDetail') || $('#' + Favorite_HospitalizationHistoryDetail.params.PanelID + ' #ulFavHospitalizationHistoryDisease').find('li[id*="-"]').length > 0 || (Favorite_HospitalizationHistoryDetail.params.mode == "Add" && Favorite_HospitalizationHistoryDetail.CPTData.length > 0)) {
            utility.myConfirm('37', function () {
                $("#" + Favorite_HospitalizationHistoryDetail.params.PanelID + " #frmFavoriteHospitalizationHistoryDetail").bootstrapValidator('revalidateField', 'FavoriteListName');
                if (Favorite_HospitalizationHistoryDetail.CPTData.length > 0)
                    $("#" + Favorite_HospitalizationHistoryDetail.params.PanelID + " #frmFavoriteHospitalizationHistoryDetail").data('bootstrapValidator').enableFieldValidators('Diagnosis', true);
                else
                    $("#" + Favorite_HospitalizationHistoryDetail.params.PanelID + " #frmFavoriteHospitalizationHistoryDetail").bootstrapValidator('revalidateField', 'Diagnosis');

                if (Favorite_HospitalizationHistoryDetail.providerCheckedIds != "") {
                    Favorite_HospitalizationHistoryDetail.validateProvider(2);
                } else {
                    Favorite_HospitalizationHistoryDetail.validateProvider(1);
                }
                if ($('#' + Favorite_HospitalizationHistoryDetail.params.PanelID + ' #txtFavoriteListName').val() != '' && $('#' + Favorite_HospitalizationHistoryDetail.params.PanelID + ' #ddlProvider').val() != null && Favorite_HospitalizationHistoryDetail.CPTData.length > 0) {
                    Favorite_HospitalizationHistoryDetail.favHospitalizationHistorySave(null, "UnLoad");
                    Favorite_HospitalizationHistoryDetail.Unload();
                }
            }, function () {
                Favorite_HospitalizationHistoryDetail.Unload();
            },'1');
        }
        else {
            Favorite_HospitalizationHistoryDetail.CPTData = [];
            Favorite_HospitalizationHistoryDetail.Unload();
        }
    },

    Unload: function(){
        var objDeffered = $.Deferred();
        if (Favorite_HospitalizationHistoryDetail.params["FromAdmin"] == "0") {
            if (Favorite_HospitalizationHistoryDetail.params != null && Favorite_HospitalizationHistoryDetail.params.ParentCtrl != null) {
                UnloadActionPan(Favorite_HospitalizationHistoryDetail.params.ParentCtrl, 'Favorite_HospitalizationHistoryDetail');
            }
            else
                UnloadActionPan(null, 'Favorite_HospitalizationHistoryDetail');
        }
        else {
            RemoveAdminTab();
        }
        objDeffered.resolve();
        return objDeffered;
    },

    //Function Name: bindAutoComplete
    //Author Name: Humaira Yousaf
    //Craeted Date: 31-03-2016
    //Description:  Binds CPT code   
    bindAutoComplete: function (element) {

        var hiddenCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "Favorite_HospitalizationHistoryDetail", null, true);

    },

    //Function Name: bindToCPTList
    //Author Name: Humaira Yousaf
    //Craeted Date: 31-03-2016
    //Description:  Binds CPT code   
    bindToCPTList: function (favouriteListICDId, FavouriteListId, ICD9Code, ICD9CodeDescription, ICD10Code, ICD10CodeDescription, SNOMEDID, SNOMEDDescription) {

        if (!Favorite_HospitalizationHistoryDetail.isInCPTList(ICD9Code, ICD9CodeDescription)) {
            var item = {};
            item["ICD9"] = ICD9Code;
            item["ICD9Description"] = ICD9CodeDescription;
            item["ICD10"] = ICD10Code;
            item["ICD10Description"] = ICD10CodeDescription;
            item["SNOMED"] = SNOMEDID;
            item["SNOMEDDescription"] = SNOMEDDescription;
            Favorite_HospitalizationHistoryDetail.CPTData.push(item);

            var $list = $("#" + Favorite_HospitalizationHistoryDetail.params.PanelID + " #ulFavHospitalizationHistoryDisease");
            cptDescArg = ICD9CodeDescription;

            if (cptDescArg != "") {
                cptDescArg = ",'" + cptDescArg.trim() + "'";
            }
            var onclick = "Favorite_HospitalizationHistoryDetail.deleteCPTFromCPTData(this,'" + ICD9Code + "','" + ICD9CodeDescription + "');";
            var li = '<li  favouriteListICDId = ' + favouriteListICDId + ' FavouriteListId = ' + FavouriteListId + ' value =' + ICD9Code + '>' +
            '<span class="pull-left pr-xlg">' + ICD9CodeDescription + ' </span>' +
                       '<span class="removeIconListHover">' +
                       '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="' + onclick + '"><i class="fa fa-times red"></i></a>' +
                       '</span>' +
                       '<div class="clearfix"></div>' +
                       '</li>';


            $list.append(li);

        }

        $("#" + Favorite_HospitalizationHistoryDetail.params.PanelID + " #txtDiagnosis").blur(function () {
            $(this).val('');
        });

        $('#' + Favorite_HospitalizationHistoryDetail.params.PanelID + ' #frmFavoriteHospitalizationHistoryDetail').data('serialize', $('#' + Favorite_HospitalizationHistoryDetail.params.PanelID + ' #frmFavoriteHospitalizationHistoryDetail').serialize());
    },
    //Function Name: deleteCPTFromCPTData
    //Author Name: Humaira Yousaf
    //Craeted Date: 31-03-2016
    //Description:Deletes CPT from CPTData Json Object
    deleteCPTFromCPTData: function (obj, icd9Code, icd9Description, iscd10Code, icd10Description, snomedCode, snomedDescription) {

        icd9Description = typeof icd9Description == "undefined" ? "" : icd9Description.toString();
        $.each(Favorite_HospitalizationHistoryDetail.CPTData, function (index, item) {
            if (item["ICD9"] == icd9Code.toString() && item["ICD9Description"] == icd9Description) {

                Favorite_HospitalizationHistoryDetail.CPTData.splice(index, 1);
                if ($(obj).parent().parent().attr('FavouriteListId') > -1 && $(obj).parent().parent().attr('favouriteListICDId') > -1) {
                    Favorite_HospitalizationHistoryDetail.deleteICD_DB_Call($(obj).parent().parent().attr('FavouriteListId'), $(obj).parent().parent().attr('favouriteListICDId'));
                }
                $(obj).parent().parent().remove();
                return false;
            }
        });
    },

    deleteICD_DB_Call: function (FavouriteListId, favouriteListICDId) {

        var objData = {};

        objData["FavoriteListId"] = FavouriteListId == null ? 0 : FavouriteListId;
        objData["FavoriteListICDId"] = favouriteListICDId == null ? 0 : favouriteListICDId;
        objData["commandType"] = "delete_favoritelist_ICD";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },

    //Function Name: isInCPTList
    //Author Name: Humaira Yousaf
    //Craeted Date: 31-03-2016
    //Description: Finds cpt code and discription in CPTData array
    isInCPTList: function (ICD9Code, ICD9CodeDescription) {
        var isExists = false;
        $.each(Favorite_HospitalizationHistoryDetail.CPTData, function (index, item) {

            if (item["ICD9Code"] == ICD9Code && item["CPTCodeDescription"] == ICD9CodeDescription) {
                isExists = true;
                return false;
            }
        });
        return isExists;
    },

    //Function Name: BindICD9AutoComplete
    //Author Name: Humaira Yousaf
    //Craeted Date: 31-03-2016
    //Description: Binds cpt code
    BindICD9AutoComplete: function (element) {
        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "Favorite_HospitalizationHistoryDetail", null, false);
    },
    //Function Name: favHospitalizationHistorySave
    //Author Name: Humaira Yousaf
    //Craeted Date: 31-03-2016
    //Description: Saves favorite history
    favHospitalizationHistorySave: function (ComeFromDelete, ComeFromUnLoad) {
        if (Favorite_HospitalizationHistoryDetail.CPTData.length > 0) {
            var strMessage = "";
            AppPrivileges.GetFormPrivileges("Favorites_HospitalizationHistory", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Favorite_HospitalizationHistoryDetail.saveFavHospitalizationHistory().done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Favorite_HospitalizationHistoryDetail.CPTData = [];
                            if (ComeFromUnLoad != "UnLoad") {
                                Favorite_HospitalizationHistoryDetail.UnLoadTab("Save");
                            }
                            var self = $('#' + Favorite_HospitalizationHistoryDetail.params.PanelID + ' #frmFavoriteHospitalizationHistoryDetail');
                            self.find('#hfFavoriteListId').val(response.FavICDId);
                            Favorite_HospitalizationHistory.favoriteListSearch();
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
                else {
                    utility.DisplayMessages(strMessage, 2);
                }
            });
        }
        else {
            utility.DisplayMessages("There is no Favorite Complaint to add", 3);/// ask from babur bhai
        }

    },
    //Function Name: AddInArray
    //Author Name: Humaira Yousaf
    //Craeted Date: 31-03-2016
    //Description: Adds CPT codes
    AddInArray: function (id, CPTCode, CPTDescription, IsSelect) {
        var item = {};
        item["DiagnosisId"] = id;
        item["CPTCode"] = CPTCode;
        item["CPTCodeDescription"] = CPTDescription;
        Favorite_HospitalizationHistoryDetail.FavHospitalizationHistory.push(item);
        if (IsSelect) {
        }

    },
    //Function Name: saveFavHospitalizationHistory
    //Author Name: Humaira Yousaf
    //Craeted Date: 31-03-2016
    //Description: Saves favorite history
    saveFavHospitalizationHistory: function () {
        //FavoriteListIcd
        var objData = {};
        var self = $('#' + Favorite_HospitalizationHistoryDetail.params.PanelID + ' #frmFavoriteHospitalizationHistoryDetail');
        favoriteListId = self.find('#hfFavoriteListId').val();

        var self = $('#pnlFavoriteHospitalizationHistoryDetail');
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objDetail = JSON.parse(myJSON);

        if (objDetail["ProviderIds"] != '') {
            Favorite_HospitalizationHistoryDetail.validateProvider(2);

            objData["FavoriteListId"] = favoriteListId;
            objData["ListType"] = "HospitalizationHistory";
            objData["EntityId"] = globalAppdata["SelectedEntityId"];//objDetail["EntityId"]; 
            objData["IsActive"] = objDetail.Active;
            objData["ProviderIds"] = objDetail["ProviderIds"];
            objData["FavoriteListName"] = objDetail["FavoriteListName"];
            if (globalAppdata['AppUserName'] == DefaultUser) {
                objData["EntityId"] = $('#' + Favorite_HospitalizationHistoryDetail.params.PanelID + ' #lstEntityId').val();
            }
            objData["FavoriteListIcd"] = Favorite_HospitalizationHistoryDetail.CPTData;
            objData["commandType"] = "save_favHospitalizationhx";
            var data = JSON.stringify(objData);
            return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
        }
        else {
            Favorite_HospitalizationHistoryDetail.validateProvider(1);
        }
    },
    showIcon: function (obj) {

        $(obj).find('div').css('display', '');

    },
    hideIcon: function (obj) {

        if ($(obj).hasClass("active") == false) {
            $(obj).find('div').css('display', 'none');
        }

    },

}