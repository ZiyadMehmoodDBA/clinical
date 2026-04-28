//Author: Humaira Yousaf
//Date: 31-03-2016
//This file will handle all actions performed for favorite Medical history
Favorite_MedicalHistoryDetail = {
    bIsFirstLoad: true,
    params: [],
    Switch: 1,
    CPTData: [],
    FavMedicalHistory: [],
    providerCheckedIds: [],
    ProviderIds: '',
    Load: function (params) {
        Favorite_MedicalHistoryDetail.params = params;
        Favorite_MedicalHistoryDetail.providerCheckedIds = [];
        if (Favorite_MedicalHistoryDetail.params.PanelID != 'pnlFavoriteMedicalHistoryDetail') {
            Favorite_MedicalHistoryDetail.params.PanelID = Favorite_MedicalHistoryDetail.params.PanelID + ' #pnlFavoriteMedicalHistoryDetail';
        } else {
            Favorite_MedicalHistoryDetail.params.PanelID = 'pnlFavoriteMedicalHistoryDetail';
        }
        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Favorite_MedicalHistoryDetail.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        if (globalAppdata['AppUserName'] == DefaultUser) {
            $('#' + Favorite_MedicalHistoryDetail.params.PanelID + ' #frmFavoriteMedicalHistoryDetail #divEntity').show();
        }

        if (Favorite_MedicalHistoryDetail.params.mode == "Edit")
            $('#' + Favorite_MedicalHistoryDetail.params.PanelID + ' #lblHeading').html('Edit Favorites List');
        var self = $('#' + Favorite_MedicalHistoryDetail.params.PanelID);
        self.loadDropDowns(true).done(function () {

            Favorite_MedicalHistoryDetail.loadEntityProvider().done(function () {
            var favListId = Favorite_MedicalHistoryDetail.params.FavoriteListId
            if (favListId != null) {
                $('#' + Favorite_MedicalHistoryDetail.params.PanelID + ' #frmFavoriteMedicalHistoryDetail #Active').attr('disabled', false);
                self.find('#hfFavoriteListId').val(favListId);
                if (favListId > 0) {
                    Favorite_MedicalHistoryDetail.favoriteListSearch(favListId);
                }
            }
            else {
                $('#' + Favorite_MedicalHistoryDetail.params.PanelID + ' #frmFavoriteMedicalHistoryDetail #Active').attr('disabled', true);
            }
            $('#' + Favorite_MedicalHistoryDetail.params.PanelID + ' #frmFavoriteMedicalHistoryDetail').data('serialize', $('#' + Favorite_MedicalHistoryDetail.params.PanelID + ' #frmFavoriteMedicalHistoryDetail').serialize());
            });
        });

        Favorite_MedicalHistoryDetail.validateFavoriteList();

        if (globalAppdata['AppUserName'] == DefaultUser) {
            $('#' + Favorite_MedicalHistoryDetail.params.PanelID + ' #frmFavoriteMedicalHistoryDetail').data('bootstrapValidator').enableFieldValidators('EntityId', true);
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

    loadEntityProvider: function () {
        var objDeffered = $.Deferred();
        selectedEntity = globalAppdata["SeletedEntityId"];
        var data = "entityID=" + selectedEntity;
        if (selectedEntity != null && selectedEntity > 0) {

            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $('#' + Favorite_MedicalHistoryDetail.params.PanelID + ' #ddlProvider');
                var $providerHiddenDdl = $('#' + Favorite_MedicalHistoryDetail.params.PanelID + ' #ddlHiddenProvider');

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
                if (Favorite_MedicalHistoryDetail.ProviderIds != '') {
                    var Providers = Favorite_MedicalHistoryDetail.ProviderIds.split(",");
                    Favorite_MedicalHistoryDetail.providerCheckedIds = Providers;
                    $('#' + Favorite_MedicalHistoryDetail.params.PanelID + ' #ddlProvider').val(Providers);
                }

            }).then(function () {
                //Intialized in onhidden spacialty ddl.
                Favorite_MedicalHistoryDetail.IntializeMultiSelectDropDownProviders();
                objDeffered.resolve();

            });
        }
        else {
            objDeffered.resolve();
        }
        return objDeffered;
    },

    IntializeMultiSelectDropDownProviders: function () {
        $('#' + Favorite_MedicalHistoryDetail.params.PanelID + ' #ddlProvider').multiselect('destroy');
        $('#' + Favorite_MedicalHistoryDetail.params.PanelID + ' #ddlProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 200,
            nonSelectedText: 'Select',
            selectAll: false,
            onDropdownHide: function (event) {
            },
            onChange: function () {
                Favorite_MedicalHistoryDetail.CheckProviderValidation();
            },
        });
    },
    CheckProviderValidation: function () {
        var self = $("#" + Favorite_MedicalHistoryDetail.params.PanelID);
        var ProviderIds = self.find('#ddlProvider option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        var strProviderIds = ProviderIds;
        Favorite_MedicalHistoryDetail.providerCheckedIds = ProviderIds;
        if (Favorite_MedicalHistoryDetail.providerCheckedIds != '') {
            Favorite_MedicalHistoryDetail.validateProvider(2);
        } else {
            Favorite_MedicalHistoryDetail.validateProvider(1);
        }
    },

    validateProvider: function (operationid) {
        var self = "#" + Favorite_MedicalHistoryDetail.params.PanelID + " #frmFavoriteMedicalHistoryDetail";
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
    //Function Name: validateFavoriteList
    //Author Name: Humaira Yousaf
    //Craeted Date: 31-03-2016
    //Description:  Validates favorite list
    validateFavoriteList: function () {
        var self = $('#' + Favorite_MedicalHistoryDetail.params.PanelID + ' #frmFavoriteMedicalHistoryDetail');

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
                },
            }
        })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            if (Favorite_MedicalHistoryDetail.CPTData.length > 0) {
                //Allow to be submited
                Favorite_MedicalHistoryDetail.favMedicalHistorySave();
            }
            else
                self.data('bootstrapValidator').enableFieldValidators('Diagnosis', true);
        })
            .on('error.form.bv', function (e) {
                e.preventDefault();

                if (Favorite_MedicalHistoryDetail.CPTData.length > 0 && self.find('#txtFavoriteListName').val() != "") {

                    //Disable validator on procedure
                    self.data('bootstrapValidator').enableFieldValidators('Diagnosis', false);
                    self.trigger('success.form.bv');
                }
                else {
                    //Disable validator if cpt has data
                    if (Favorite_MedicalHistoryDetail.CPTData.length > 0)
                        self.data('bootstrapValidator').enableFieldValidators('Diagnosis', false);
                }

            });
    },

    //Function Name: favoriteListSearch
    //Author Name: Humaira Yousaf
    //Craeted Date: 31-03-2016
    //Description:  Searches favorite list
    favoriteListSearch: function (FavoriteListId, ListType, PageNo, rpp) {

        Favorite_MedicalHistory.searchFavoriteList_DBCall(ListType, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                var self = $('#' + Favorite_MedicalHistoryDetail.params.PanelID + ' #frmFavoriteMedicalHistoryDetail');
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
                        $('#' + Favorite_MedicalHistoryDetail.params.PanelID + " #ddlProvider").val(FavoriteListProvidersJSON);

                        $('#' + Favorite_MedicalHistoryDetail.params.PanelID + " #ddlProvider").multiselect("refresh");
                        $('#' + Favorite_MedicalHistoryDetail.params.PanelID + " #ddlProvider").multiselect({
                            includeSelectAllOption: true,
                            enableFiltering: true,
                            enableCaseInsensitiveFiltering: true,
                            maxHeight: 247
                        });
                        Favorite_MedicalHistoryDetail.providerCheckedIds = FavoriteListProvidersJSON;
                        // Patient_Demographic.multipleRaceIds = demographic_detail.strRaceIds;
                    } else {
                        $('#' + Favorite_MedicalHistoryDetail.params.PanelID + " #ddlProvider").find("option:selected").removeAttr("selected");
                        $('#' + Favorite_MedicalHistoryDetail.params.PanelID + " #ddlProvider").multiselect("refresh");
                    }
                    Favorite_MedicalHistoryDetail.favoriteList_CPTSearch(JSON.parse(response.FavoriteListJSON)[0].FavoriteListId);
                    $('#' + Favorite_MedicalHistoryDetail.params.PanelID + ' #frmFavoriteMedicalHistoryDetail').data('serialize', $('#' + Favorite_MedicalHistoryDetail.params.PanelID + ' #frmFavoriteMedicalHistoryDetail').serialize());
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

        Favorite_MedicalHistory.searchFavoriteList_CPT_DBCall(FavoriteListCPTId, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                //Bind to list
                $.each(JSON.parse(response.FavoriteListICDJSON), function (index, CPTRow) {
                    Favorite_MedicalHistoryDetail.bindToCPTList(CPTRow.FavoriteListICDId, CPTRow.FavoriteListId, CPTRow.ICD9Code, CPTRow.ICD9CodeDescription, CPTRow.ICD10Code, CPTRow.ICD10CodeDescription, CPTRow.SNOMEDID, CPTRow.SNOMEDDescription);

                });
                $('#' + Favorite_MedicalHistoryDetail.params.PanelID + ' #frmFavoriteMedicalHistoryDetail').data('serialize', $('#' + Favorite_MedicalHistoryDetail.params.PanelID + ' #frmFavoriteMedicalHistoryDetail').serialize());
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
            Favorite_MedicalHistoryDetail.CPTData = [];
            Favorite_MedicalHistoryDetail.Unload();
        }
        else if (EMRUtility.compareFormDataWithSerialized(Favorite_MedicalHistoryDetail.params.PanelID + ' #frmFavoriteMedicalHistoryDetail') || $('#' + Favorite_MedicalHistoryDetail.params.PanelID + ' #ulFavMedicalHistoryDisease').find('li[id*="-"]').length > 0 || (Favorite_MedicalHistoryDetail.params.mode == "Add" && Favorite_MedicalHistoryDetail.CPTData.length > 0)) {
            utility.myConfirm('37', function () {
                $("#" + Favorite_MedicalHistoryDetail.params.PanelID + " #frmFavoriteMedicalHistoryDetail").bootstrapValidator('revalidateField', 'FavoriteListName');
                if (Favorite_MedicalHistoryDetail.CPTData.length > 0)
                    $("#" + Favorite_MedicalHistoryDetail.params.PanelID + " #frmFavoriteMedicalHistoryDetail").data('bootstrapValidator').enableFieldValidators('Diagnosis', true);
                else
                    $("#" + Favorite_MedicalHistoryDetail.params.PanelID + " #frmFavoriteMedicalHistoryDetail").bootstrapValidator('revalidateField', 'Diagnosis');

                if (Favorite_MedicalHistoryDetail.providerCheckedIds != "") {
                    Favorite_MedicalHistoryDetail.validateProvider(2);
                } else {
                    Favorite_MedicalHistoryDetail.validateProvider(1);
                }
                if ($('#' + Favorite_MedicalHistoryDetail.params.PanelID + ' #txtFavoriteListName').val() != '' && $('#' + Favorite_MedicalHistoryDetail.params.PanelID + ' #ddlProvider').val() != null && Favorite_MedicalHistoryDetail.CPTData.length > 0) {
                    Favorite_MedicalHistoryDetail.favMedicalHistorySave(null, "UnLoad");
                    Favorite_MedicalHistoryDetail.Unload();
                }
            }, function () {
                Favorite_MedicalHistoryDetail.Unload();
            }, '1');
        }
        else {
            Favorite_MedicalHistoryDetail.CPTData = [];
            Favorite_MedicalHistoryDetail.Unload();
        }
    },

    Unload: function(){
        var objDeffered = $.Deferred();
        if (Favorite_MedicalHistoryDetail.params["FromAdmin"] == "0") {
            if (Favorite_MedicalHistoryDetail.params != null && Favorite_MedicalHistoryDetail.params.ParentCtrl != null) {
                UnloadActionPan(Favorite_MedicalHistoryDetail.params.ParentCtrl, 'Favorite_MedicalHistoryDetail');
            }
            else
                UnloadActionPan(null, 'Favorite_MedicalHistoryDetail');
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
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "Favorite_MedicalHistoryDetail", null, true);

    },

    //Function Name: bindToCPTList
    //Author Name: Humaira Yousaf
    //Craeted Date: 31-03-2016
    //Description:  Binds CPT code   
    bindToCPTList: function (favouriteListICDId, FavouriteListId, ICD9Code, ICD9CodeDescription, ICD10Code, ICD10CodeDescription, SNOMEDID, SNOMEDDescription) {

        if (!Favorite_MedicalHistoryDetail.isInCPTList(ICD9Code, ICD9CodeDescription)) {
            var item = {};
            item["ICD9"] = ICD9Code;
            item["ICD9Description"] = ICD9CodeDescription;
            item["ICD10"] = ICD10Code;
            item["ICD10Description"] = ICD10CodeDescription;
            item["SNOMED"] = SNOMEDID;
            item["SNOMEDDescription"] = SNOMEDDescription;

            Favorite_MedicalHistoryDetail.CPTData.push(item);

            var $list = $("#" + Favorite_MedicalHistoryDetail.params.PanelID + " #ulFavMedicalHistoryDisease");
            cptDescArg = ICD9CodeDescription;

            if (cptDescArg != "") {
                cptDescArg = ",'" + cptDescArg.trim() + "'";
            }
            var onclick = "Favorite_MedicalHistoryDetail.deleteCPTFromCPTData(this,'" + ICD9Code + "','" + ICD9CodeDescription + "');";
            var li = '<li  favouriteListICDId = ' + favouriteListICDId + ' FavouriteListId = ' + FavouriteListId + ' value =' + ICD9Code + '>' +
            '<span class="pull-left pr-xlg">' + ICD9CodeDescription + ' </span>' +
                       '<span class="removeIconListHover">' +
                       '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="' + onclick + '"><i class="fa fa-times red"></i></a>' +
                       '</span>' +
                       '<div class="clearfix"></div>' +
                       '</li>';


            $list.append(li);

        }

        $("#" + Favorite_MedicalHistoryDetail.params.PanelID + " #txtDiagnosis").blur(function () {
            $(this).val('');
        });

        $('#' + Favorite_MedicalHistoryDetail.params.PanelID + ' #frmFavoriteMedicalHistoryDetail').data('serialize', $('#' + Favorite_MedicalHistoryDetail.params.PanelID + ' #frmFavoriteMedicalHistoryDetail').serialize());
    },
    //Function Name: deleteCPTFromCPTData
    //Author Name: Humaira Yousaf
    //Craeted Date: 31-03-2016
    //Description:Deletes CPT from CPTData Json Object
    deleteCPTFromCPTData: function (obj, icd9Code, icd9Description, iscd10Code, icd10Description, snomedCode, snomedDescription) {

        icd9Description = typeof icd9Description == "undefined" ? "" : icd9Description.toString();
        $.each(Favorite_MedicalHistoryDetail.CPTData, function (index, item) {
            if (item["ICD9"] == icd9Code.toString() && item["ICD9Description"] == icd9Description) {

                Favorite_MedicalHistoryDetail.CPTData.splice(index, 1);

                if ($(obj).parent().parent().attr('FavouriteListId') > -1 && $(obj).parent().parent().attr('favouriteListICDId') > -1) {
                    Favorite_MedicalHistoryDetail.deleteICD_DB_Call($(obj).parent().parent().attr('FavouriteListId'), $(obj).parent().parent().attr('favouriteListICDId'));
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
        $.each(Favorite_MedicalHistoryDetail.CPTData, function (index, item) {

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
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "Favorite_MedicalHistoryDetail", null, false);
    },
    //Function Name: favMedicalHistorySave
    //Author Name: Humaira Yousaf
    //Craeted Date: 31-03-2016
    //Description: Saves favorite history
    favMedicalHistorySave: function (ComeFromDelete, ComeFromUnLoad) {
        if (Favorite_MedicalHistoryDetail.CPTData.length > 0) {
            var strMessage = "";
            AppPrivileges.GetFormPrivileges("Favorites_MedicalHistory", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Favorite_MedicalHistoryDetail.saveFavMedicalHistory().done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Favorite_MedicalHistoryDetail.CPTData = [];

                            if (ComeFromUnLoad != "UnLoad") {
                                Favorite_MedicalHistoryDetail.UnLoadTab("Save");
                            }
                            var self = $('#' + Favorite_MedicalHistoryDetail.params.PanelID + ' #frmFavoriteMedicalHistoryDetail');
                            self.find('#hfFavoriteListId').val(response.FavICDId);
                            Favorite_MedicalHistory.favoriteListSearch();
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
        Favorite_MedicalHistoryDetail.FavMedicalHistory.push(item);
        if (IsSelect) {
        }

    },
    //Function Name: saveFavMedicalHistory
    //Author Name: Humaira Yousaf
    //Craeted Date: 31-03-2016
    //Description: Saves favorite history
    saveFavMedicalHistory: function () {
        //FavoriteListIcd
        var objData = {};
        var self = $('#' + Favorite_MedicalHistoryDetail.params.PanelID + ' #frmFavoriteMedicalHistoryDetail');
        favoriteListId = self.find('#hfFavoriteListId').val();

        var self = $('#pnlFavoriteMedicalHistoryDetail');
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objDetail = JSON.parse(myJSON);

        if (objDetail["ProviderIds"] != '') {
            Favorite_MedicalHistoryDetail.validateProvider(2);
        objData["FavoriteListId"] = favoriteListId;
        objData["ListType"] = "MedicalHistory";
        objData["EntityId"] = globalAppdata["SelectedEntityId"];
        if (globalAppdata['AppUserName'] == DefaultUser) {
            objData["EntityId"] = $('#' + Favorite_MedicalHistoryDetail.params.PanelID + ' #lstEntityId').val();
        }
       
        objData["IsActive"] = objDetail.Active;
        objData["ProviderIds"] = objDetail["ProviderIds"];
        objData["FavoriteListName"] = objDetail["FavoriteListName"];
        if (globalAppdata['AppUserName'] != DefaultUser) {
            objData["EntityId"] = -1;
        }
        objData["FavoriteListIcd"] = Favorite_MedicalHistoryDetail.CPTData;
        objData["commandType"] = "save_favmedicalhx";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
        }
        else {
            Favorite_MedicalHistoryDetail.validateProvider(1);
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

        $('#pnlFavoriteMedicalHistoryDetail #txtDiagnosis').attr('data-popupunload', 'true');
        var params = [];
        params["FromAdmin"] = "0";
        //params["Parentctrl"] = Clinical_MedicalHx.params["TabID"];
        if (Favorite_MedicalHistoryDetail.params.TabID == 'clinicalTabProgressNote') {
            params['FromProgressNote'] = 'pnlClinicalProgressNote';
            params["ParentCtrl"] = 'Favorite_MedicalHistoryDetail';
        }
        else
            params["ParentCtrl"] = 'Favorite_MedicalHistoryDetail';

        if (ctrl != null) {
            params["RefCtrl"] = ctrl;
        }
        if (hiddenCtrl != null) {
            params["RefHiddenCtrl"] = hiddenCtrl;
        }
        if (controlToLoad != "") {
            if (Favorite_MedicalHistoryDetail.params.TabID == 'clinicalTabProgressNote' && searchType == "ICD")
                LoadActionPan(controlToLoad, params, 'pnlClinicalProgressNote');
            else
                LoadActionPan(controlToLoad, params);
        }

    },


    bindICD9AutoComplete: function (element) {
        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "Favorite_MedicalHistoryDetail", null, false);
    },
}