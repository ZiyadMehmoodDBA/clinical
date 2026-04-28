//Author: Humaira Yousaf
//Date: 31-03-2016
//This file will handle all actions performed for favorite Surgical history
Favorite_SurgicalHistoryDetail = {
    bIsFirstLoad: true,
    params: [],
    Switch: 1,
    CPTData: [],
    FavSurgicalHistory: [],
    providerCheckedIds: [],
    ProviderIds: '',
    Load: function (params) {
        Favorite_SurgicalHistoryDetail.params = params;
        Favorite_SurgicalHistoryDetail.providerCheckedIds = [];
        if (Favorite_SurgicalHistoryDetail.params.PanelID != 'pnlFavoriteSurgicalHistoryDetail') {
            Favorite_SurgicalHistoryDetail.params.PanelID = Favorite_SurgicalHistoryDetail.params.PanelID + ' #pnlFavoriteSurgicalHistoryDetail';
        } else {
            Favorite_SurgicalHistoryDetail.params.PanelID = 'pnlFavoriteSurgicalHistoryDetail';
        }
        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Favorite_SurgicalHistoryDetail.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        if (globalAppdata['AppUserName'] == DefaultUser) {
            $('#' + Favorite_SurgicalHistoryDetail.params.PanelID + ' #frmFavoriteSurgicalHistoryDetail #divEntity').show();
        }
        if (Favorite_SurgicalHistoryDetail.params.mode == "Edit")
            $('#' + Favorite_SurgicalHistoryDetail.params.PanelID + ' #lblHeading').html('Edit Favorites List');

        var self = $('#' + Favorite_SurgicalHistoryDetail.params.PanelID);
        self.loadDropDowns(true).done(function () {

            Favorite_SurgicalHistoryDetail.loadEntityProvider().done(function () {
                var favListId = Favorite_SurgicalHistoryDetail.params.FavoriteListId
                if (favListId != null) {
                    $('#' + Favorite_SurgicalHistoryDetail.params.PanelID + ' #frmFavoriteSurgicalHistoryDetail #Active').attr('disabled', false);
                    self.find('#hfFavoriteListId').val(favListId);
                    if (favListId > 0) {
                        Favorite_SurgicalHistoryDetail.favoriteListSearch(favListId);
                    }
                }
                else {
                    $('#' + Favorite_SurgicalHistoryDetail.params.PanelID + ' #frmFavoriteSurgicalHistoryDetail #Active').attr('disabled', true);
                }
                $('#' + Favorite_SurgicalHistoryDetail.params.PanelID + ' #frmFavoriteSurgicalHistoryDetail').data('serialize', $('#' + Favorite_SurgicalHistoryDetail.params.PanelID + ' #frmFavoriteSurgicalHistoryDetail').serialize());
            });
        });

        Favorite_SurgicalHistoryDetail.validateFavoriteList();
        if (globalAppdata['AppUserName'] == DefaultUser) {
            $('#' + Favorite_SurgicalHistoryDetail.params.PanelID + ' #frmFavoriteSurgicalHistoryDetail').data('bootstrapValidator').enableFieldValidators('EntityId', true);
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
        $('#' + Favorite_SurgicalHistoryDetail.params.PanelID + ' #ddlProvider').multiselect('destroy');
        $('#' + Favorite_SurgicalHistoryDetail.params.PanelID + ' #ddlProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 200,
            nonSelectedText: 'Select',
            selectAll: false,
            onDropdownHide: function (event) {
            },
            onChange: function () {
                Favorite_SurgicalHistoryDetail.CheckProviderValidation();
            },
        });
    },
    CheckProviderValidation: function () {
        var self = $("#" + Favorite_SurgicalHistoryDetail.params.PanelID);
        var ProviderIds = self.find('#ddlProvider option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        var strProviderIds = ProviderIds;
        Favorite_SurgicalHistoryDetail.providerCheckedIds = ProviderIds;
        if (Favorite_SurgicalHistoryDetail.providerCheckedIds != '') {
            Favorite_SurgicalHistoryDetail.validateProvider(2);
        } else {
            Favorite_SurgicalHistoryDetail.validateProvider(1);
        }
    },

    validateProvider: function (operationid) {
        var self = "#" + Favorite_SurgicalHistoryDetail.params.PanelID + " #frmFavoriteSurgicalHistoryDetail";
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
                var $providerDdl = $('#' + Favorite_SurgicalHistoryDetail.params.PanelID + ' #ddlProvider');
                var $providerHiddenDdl = $('#' + Favorite_SurgicalHistoryDetail.params.PanelID + ' #ddlHiddenProvider');

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
                if (Favorite_SurgicalHistoryDetail.ProviderIds != '') {
                    var Providers = Favorite_SurgicalHistoryDetail.ProviderIds.split(",");
                    Favorite_SurgicalHistoryDetail.providerCheckedIds = Providers;
                    $('#' + Favorite_SurgicalHistoryDetail.params.PanelID + ' #ddlProvider').val(Providers);
                }

            }).then(function () {
                //Intialized in onhidden spacialty ddl.
                Favorite_SurgicalHistoryDetail.IntializeMultiSelectDropDownProviders();
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
        var self = $('#' + Favorite_SurgicalHistoryDetail.params.PanelID + ' #frmFavoriteSurgicalHistoryDetail');

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
            if (Favorite_SurgicalHistoryDetail.CPTData.length > 0) {
                //Allow to be submited
                Favorite_SurgicalHistoryDetail.favSurgicalHistorySave();
            }
            else
                self.data('bootstrapValidator').enableFieldValidators('Diagnosis', true);
        })
            .on('error.form.bv', function (e) {
                e.preventDefault();

                if (Favorite_SurgicalHistoryDetail.CPTData.length > 0 && self.find('#txtFavoriteListName').val() != "") {

                    //Disable validator on procedure
                    self.data('bootstrapValidator').enableFieldValidators('Diagnosis', false);
                    self.trigger('success.form.bv');
                }
                else {
                    //Disable validator if cpt has data
                    if (Favorite_SurgicalHistoryDetail.CPTData.length > 0)
                        self.data('bootstrapValidator').enableFieldValidators('Diagnosis', false);
                }

            });
    },

    //Function Name: favoriteListSearch
    //Author Name: Humaira Yousaf
    //Craeted Date: 31-03-2016
    //Description:  Searches favorite list
    favoriteListSearch: function (FavoriteListId, ListType, PageNo, rpp) {

        Favorite_SurgicalHistory.searchFavoriteList_DBCall(ListType, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                var self = $('#' + Favorite_SurgicalHistoryDetail.params.PanelID + ' #frmFavoriteSurgicalHistoryDetail');
                var favListDetail = JSON.parse(response.FavoriteListJSON)[0];
                //Bind to form
                utility.bindMyJSONByName(true, favListDetail, false, self).done(function () {
                    var isActive = false;
                    if (favListDetail.IsActive === "True")
                        isActive = true;
                    self.find('#Active').attr('checked', isActive);
                    self.find('#txtFavoriteListName').val(favListDetail.Name);
                    //Load CPT against favList
                    var a = JSON.parse(response.FavoriteListJSON);
                    var FavoriteListProvidersJSON = JSON.parse(response.FavoriteListProviders_JSON);
                    if (FavoriteListProvidersJSON != '' && FavoriteListProvidersJSON) {
                        $('#' + Favorite_SurgicalHistoryDetail.params.PanelID + " #ddlProvider").val(FavoriteListProvidersJSON);

                        $('#' + Favorite_SurgicalHistoryDetail.params.PanelID + " #ddlProvider").multiselect("refresh");
                        $('#' + Favorite_SurgicalHistoryDetail.params.PanelID + " #ddlProvider").multiselect({
                            includeSelectAllOption: true,
                            enableFiltering: true,
                            enableCaseInsensitiveFiltering: true,
                            maxHeight: 247
                        });
                        Favorite_SurgicalHistoryDetail.providerCheckedIds = FavoriteListProvidersJSON;
                        // Patient_Demographic.multipleRaceIds = demographic_detail.strRaceIds;
                    } else {
                        $('#' + Favorite_SurgicalHistoryDetail.params.PanelID + " #ddlProvider").find("option:selected").removeAttr("selected");
                        $('#' + Favorite_SurgicalHistoryDetail.params.PanelID + " #ddlProvider").multiselect("refresh");
                    }
                    Favorite_SurgicalHistoryDetail.favoriteList_CPTSearch(a[0].FavoriteListId);
                    $('#' + Favorite_SurgicalHistoryDetail.params.PanelID + ' #frmFavoriteSurgicalHistoryDetail').data('serialize', $('#' + Favorite_SurgicalHistoryDetail.params.PanelID + ' #frmFavoriteSurgicalHistoryDetail').serialize());
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

        Favorite_SurgicalHistory.searchFavoriteList_CPT_DBCall(FavoriteListCPTId, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                //Bind to list
                $.each(JSON.parse(response.FavoriteListCPTJSON), function (index, CPTRow) {
                    Favorite_SurgicalHistoryDetail.bindToCPTList(CPTRow.FavoriteListCPTId, CPTRow.FavoriteListId, CPTRow.CPTCode, CPTRow.CPTCodeDescription, CPTRow.SNOMEDID, CPTRow.SNOMED_DESCRIPTION);

                });
                $('#' + Favorite_SurgicalHistoryDetail.params.PanelID + ' #frmFavoriteSurgicalHistoryDetail').data('serialize', $('#' + Favorite_SurgicalHistoryDetail.params.PanelID + ' #frmFavoriteSurgicalHistoryDetail').serialize());
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
            Favorite_SurgicalHistoryDetail.CPTData = [];
            Favorite_SurgicalHistoryDetail.Unload();
        }

        else if (EMRUtility.compareFormDataWithSerialized(Favorite_SurgicalHistoryDetail.params.PanelID + ' #frmFavoriteSurgicalHistoryDetail') || $('#' + Favorite_SurgicalHistoryDetail.params.PanelID + ' #ulFavSurgicalHistoryDisease').find('li[id*="-"]').length > 0 || (Favorite_SurgicalHistoryDetail.params.mode == "Add" && Favorite_SurgicalHistoryDetail.CPTData.length > 0)) {
            utility.myConfirm('37', function () {
                $("#" + Favorite_SurgicalHistoryDetail.params.PanelID + " #frmFavoriteSurgicalHistoryDetail").bootstrapValidator('revalidateField', 'FavoriteListName');
                if (Favorite_SurgicalHistoryDetail.CPTData.length > 0)
                    $("#" + Favorite_SurgicalHistoryDetail.params.PanelID + " #frmFavoriteSurgicalHistoryDetail").data('bootstrapValidator').enableFieldValidators('Diagnosis', true);
                else
                    $("#" + Favorite_SurgicalHistoryDetail.params.PanelID + " #frmFavoriteSurgicalHistoryDetail").bootstrapValidator('revalidateField', 'Diagnosis');

                if (Favorite_SurgicalHistoryDetail.providerCheckedIds != "") {
                    Favorite_SurgicalHistoryDetail.validateProvider(2);
                } else {
                    Favorite_SurgicalHistoryDetail.validateProvider(1);
                }
                if ($('#' + Favorite_SurgicalHistoryDetail.params.PanelID + ' #txtFavoriteListName').val() != '' && $('#' + Favorite_SurgicalHistoryDetail.params.PanelID + ' #ddlProvider').val() != null && Favorite_SurgicalHistoryDetail.CPTData.length > 0) {
                    Favorite_SurgicalHistoryDetail.favSurgicalHistorySave(null, "UnLoad");
                    Favorite_SurgicalHistoryDetail.Unload();
                }

            }, function () {
                Favorite_SurgicalHistoryDetail.Unload();
            }, '1');
        }
        else {
            Favorite_SurgicalHistoryDetail.CPTData = [];
            Favorite_SurgicalHistoryDetail.Unload();
        }
    },

    Unload: function(){
        var objDeffered = $.Deferred();
        if (Favorite_SurgicalHistoryDetail.params["FromAdmin"] == "0") {
            if (Favorite_SurgicalHistoryDetail.params != null && Favorite_SurgicalHistoryDetail.params.ParentCtrl != null) {
                UnloadActionPan(Favorite_SurgicalHistoryDetail.params.ParentCtrl, 'Favorite_SurgicalHistoryDetail');
            }
            else
                UnloadActionPan(null, 'Favorite_SurgicalHistoryDetail');
        }
        else {
            RemoveAdminTab();
        }
        objDeffered.resolve();
        return objDeffered;
    },

    openCPTCode: function () {
        var params = [];
        //params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Favorite_SurgicalHistoryDetail";
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtDisease";
        params["ParentCtrlPanelID"] = Favorite_SurgicalHistoryDetail.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, Favorite_SurgicalHistoryDetail.params.PanelID);
    },

    //Function Name: bindAutoComplete
    //Author Name: Humaira Yousaf
    //Craeted Date: 31-03-2016
    //Description:  Binds CPT code   
    bindAutoComplete: function (element) {

        var hiddenCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "Favorite_SurgicalHistoryDetail", null, true);

    },

    //Function Name: bindToCPTList
    //Author Name: Humaira Yousaf
    //Craeted Date: 31-03-2016
    //Description:  Binds CPT code   
    bindToCPTList: function (favouriteListICDId, FavouriteListId, CPTCode, CPTCodeDescription, SNOMEDID, SNOMEDDescription) {

        if (!Favorite_SurgicalHistoryDetail.isInCPTList(CPTCode, CPTCodeDescription)) {
            var item = {};
            item["FavoriteListCPTId"] = favouriteListICDId;
            item["CPTCode"] = CPTCode;
            item["CPTCodeDescription"] = CPTCodeDescription;
            item["SNOMEDID"] = SNOMEDID;
            item["SNOMED_Description"] = SNOMEDDescription;

            Favorite_SurgicalHistoryDetail.CPTData.push(item);

            var $list = $("#" + Favorite_SurgicalHistoryDetail.params.PanelID + " #ulFavSurgicalHistoryDisease");
            cptDescArg = CPTCodeDescription;

            if (cptDescArg != "") {
                cptDescArg = ",'" + cptDescArg.trim() + "'";
            }
            var onclick = "Favorite_SurgicalHistoryDetail.deleteCPTFromCPTData(this,'" + CPTCode + "','" + CPTCodeDescription + "');";
            var li = '<li  cptCode="' + CPTCode + '" cptDesc="' + CPTCodeDescription + '" snomedCode="' + SNOMEDID + '" snomedDesc="' + SNOMEDDescription + '"   favouriteListICDId = ' + favouriteListICDId + ' FavouriteListId = ' + FavouriteListId + ' value =' + CPTCode + '>' +
            '<span class="pull-left pr-xlg">' + CPTCodeDescription + ' </span>' +
                       '<span class="removeIconListHover">' +
                       '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="' + onclick + '"><i class="fa fa-times red"></i></a>' +
                       '</span>' +
                       '<div class="clearfix"></div>' +
                       '</li>';




            $list.append(li);

        }

        $("#" + Favorite_SurgicalHistoryDetail.params.PanelID + " #txtDiagnosis").blur(function () {
            $(this).val('');
        });

        $('#' + Favorite_SurgicalHistoryDetail.params.PanelID + ' #frmFavoriteSurgicalHistoryDetail').data('serialize', $('#' + Favorite_SurgicalHistoryDetail.params.PanelID + ' #frmFavoriteSurgicalHistoryDetail').serialize());

    },
    //Function Name: deleteCPTFromCPTData
    //Author Name: Humaira Yousaf
    //Craeted Date: 31-03-2016
    //Description:Deletes CPT from CPTData Json Object
    deleteCPTFromCPTData: function (obj, cptCode, cptDescription, snomedCode, snomedDescription) {

        cptDescription = typeof cptDescription == "undefined" ? "" : cptDescription.toString();
        
        $.each(Favorite_SurgicalHistoryDetail.CPTData, function (index, item) {
            if (item["CPTCode"] == cptCode.toString() && utility.decodeHtml(item["CPTCodeDescription"]) == cptDescription) {

                Favorite_SurgicalHistoryDetail.CPTData.splice(index, 1);
                if ($(obj).parent().parent().attr('FavouriteListId') > -1 && $(obj).parent().parent().attr('favouriteListICDId') > -1) {
                    Favorite_SurgicalHistoryDetail.deleteICD_DB_Call($(obj).parent().parent().attr('FavouriteListId'), $(obj).parent().parent().attr('favouriteListICDId'));
                }

                $(obj).parent().parent().remove();
                return false;
            }
        });
    },

    deleteICD_DB_Call: function (FavouriteListId, favouriteListICDId) {

        var objData = {};

        objData["FavoriteListId"] = FavouriteListId == null ? 0 : FavouriteListId;
        objData["FavoriteListCPTId"] = favouriteListICDId == null ? 0 : favouriteListICDId;
        objData["commandType"] = "delete_favoritelist_cpt";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },

    //Function Name: isInCPTList
    //Author Name: Humaira Yousaf
    //Craeted Date: 31-03-2016
    //Description: Finds cpt code and discription in CPTData array
    isInCPTList: function (ICD9Code, ICD9CodeDescription) {
        var isExists = false;
        $.each(Favorite_SurgicalHistoryDetail.CPTData, function (index, item) {

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
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "Favorite_SurgicalHistoryDetail", null, false);
    },
    //Function Name: favSurgicalHistorySave
    //Author Name: Humaira Yousaf
    //Craeted Date: 31-03-2016
    //Description: Saves favorite history
    favSurgicalHistorySave: function (ComeFromDelete, ComeFromUnLoad) {
        if (Favorite_SurgicalHistoryDetail.CPTData.length > 0) {
            var strMessage = "";
            AppPrivileges.GetFormPrivileges("Favorites_SurgicalHistory", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Favorite_SurgicalHistoryDetail.saveFavSurgicalHistory().done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Favorite_SurgicalHistoryDetail.CPTData = [];
                            if (ComeFromUnLoad != "UnLoad") {
                                Favorite_SurgicalHistoryDetail.UnLoadTab("Save");
                            }
                            var self = $('#' + Favorite_SurgicalHistoryDetail.params.PanelID + ' #frmFavoriteSurgicalHistoryDetail');
                            self.find('#hfFavoriteListId').val(response.FavCPTId);
                            Favorite_SurgicalHistory.favoriteListSearch();
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
        Favorite_SurgicalHistoryDetail.FavSurgicalHistory.push(item);
        if (IsSelect) {
        }

    },
    //Function Name: saveFavSurgicalHistory
    //Author Name: Humaira Yousaf
    //Craeted Date: 31-03-2016
    //Description: Saves favorite history
    saveFavSurgicalHistory: function () {
        //FavoriteListIcd
        var objData = {};
        var self = $('#' + Favorite_SurgicalHistoryDetail.params.PanelID + ' #frmFavoriteSurgicalHistoryDetail');
        favoriteListId = self.find('#hfFavoriteListId').val();

        var self = $('#pnlFavoriteSurgicalHistoryDetail');
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objDetail = JSON.parse(myJSON);

        if (objDetail["ProviderIds"] != '') {
            Favorite_SurgicalHistoryDetail.validateProvider(2);
        objData["FavoriteListId"] = favoriteListId;
        objData["ListType"] = "SurgicalHistory";
        objData["EntityId"] = globalAppdata["SelectedEntityId"];
        objData["IsActive"] = objDetail.Active;
        objData["ProviderIds"] = objDetail["ProviderIds"];
        objData["FavoriteListName"] = objDetail["FavoriteListName"];
        if (globalAppdata['AppUserName'] == DefaultUser) {
            objData["EntityId"] = $('#' + Favorite_SurgicalHistoryDetail.params.PanelID + ' #lstEntityId').val();
        }
        objData["FavoriteListCPT"] = Favorite_SurgicalHistoryDetail.CPTData;
        objData["commandType"] = "save_favsurgicalhx";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
        }
        else {
            Favorite_SurgicalHistoryDetail.validateProvider(1);
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
        $('#pnlFavoriteSurgicalHistoryDetail #txtDiagnosis').attr('data-popupunload', 'true');
        var params = [];
        params["FromAdmin"] = "0";
        //params["Parentctrl"] = Clinical_MedicalHx.params["TabID"];
        if (Favorite_SurgicalHistoryDetail.params.TabID == 'clinicalTabProgressNote') {
            params['FromProgressNote'] = 'pnlClinicalProgressNote';
            params["ParentCtrl"] = 'Favorite_SurgicalHistoryDetail';
        }
        else
            params["ParentCtrl"] = 'Favorite_SurgicalHistoryDetail';

        if (ctrl != null) {
            params["RefCtrl"] = ctrl;
        }
        if (hiddenCtrl != null) {
            params["RefHiddenCtrl"] = hiddenCtrl;
        }
        if (controlToLoad != "") {
            if (Favorite_SurgicalHistoryDetail.params.TabID == 'clinicalTabProgressNote' && searchType == "ICD")
                LoadActionPan(controlToLoad, params, 'pnlClinicalProgressNote');
            else
                LoadActionPan(controlToLoad, params);
        }

    },

    bindICD9AutoComplete: function (element) {
        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "Favorite_SurgicalHistoryDetail", null, false);
    },
}