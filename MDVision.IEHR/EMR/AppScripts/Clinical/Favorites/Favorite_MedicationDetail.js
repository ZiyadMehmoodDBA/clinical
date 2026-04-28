Favorite_MedicationDetail = {
    bIsFirstLoad: true,
    params: [],
    Switch: 1,
    MedicationData: [],
    ProviderIds: '',
    providerCheckedIds: [],
    Load: function (params) {
        Favorite_MedicationDetail.params = params;
        if (Favorite_MedicationDetail.params.PanelID != 'pnlFavoriteMedicationDetail')
            Favorite_MedicationDetail.params.PanelID = Favorite_MedicationDetail.params.PanelID + ' #pnlFavoriteMedicationDetail';
        else
            Favorite_MedicationDetail.params.PanelID = 'pnlFavoriteMedicationDetail';
        if (globalAppdata['AppUserName'] == DefaultUser)
            $('#' + Favorite_MedicationDetail.params.PanelID + ' #frmFavoriteMedicationDetail #divEntity').show();
        var self = $('#' + Favorite_MedicationDetail.params.PanelID);
        self.loadDropDowns(true).done(function () {
            selectedEntity = globalAppdata["SeletedEntityId"];
            $.when(Favorite_MedicationDetail.loadEntityProvider(selectedEntity)).then(function () {
                var favListId = Favorite_MedicationDetail.params.FavoriteListId
                if (favListId) {
                    self.find('#hfFavoriteListId').val(favListId);
                    if (favListId > 0)
                        Favorite_MedicationDetail.LoadMedicationData(favListId);
                }
            });
            if (Favorite_MedicationDetail.params.mode == "Add")
                $('#' + Favorite_MedicationDetail.params.PanelID + ' #frmFavoriteMedicationDetail #Active').prop("disabled", true);
            else if (Favorite_MedicationDetail.params.mode == "Edit") {
                $('#' + Favorite_MedicationDetail.params.PanelID + ' #lblHeading').html('Edit Favorites List');
                $('#' + Favorite_MedicationDetail.params.PanelID + ' #frmFavoriteMedicationDetail #Active').prop("disabled", false);
                $('#' + Favorite_MedicationDetail.params.PanelID + ' #divFavMedicationSearchList').removeClass('disableAll');
            }
        });
        Favorite_MedicationDetail.ValidateFavMedication();
    },
    LoadMedicationData: function (FavoriteListId, ListType, PageNo, rpp) {
        Favorite_Medication.searchFavoriteList_DBCall(ListType, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var self = $('#' + Favorite_MedicationDetail.params.PanelID + ' #frmFavoriteMedicationDetail');
                var favListDetail = JSON.parse(response.FavoriteListJSON)[0];
                utility.bindMyJSONByName(true, favListDetail, false, self).done(function () {
                    var isActive = false;
                    if (favListDetail.IsActive === "True")
                        isActive = true;
                    self.find('#Active').attr('checked', isActive);
                    self.find('#txtFavoriteListName').val(favListDetail.Name);
                    var FavoriteListProvidersJSON = JSON.parse(response.FavoriteListProviders_JSON);
                    Favorite_MedicationDetail.IntializeMultiSelectDropDownProviders();
                    if (FavoriteListProvidersJSON != '' && FavoriteListProvidersJSON) {
                        $('#' + Favorite_MedicationDetail.params.PanelID + " #ddlMedicationProvider").val(FavoriteListProvidersJSON);
                        $('#' + Favorite_MedicationDetail.params.PanelID + " #ddlMedicationProvider").multiselect("refresh");
                        Favorite_MedicationDetail.providerCheckedIds = FavoriteListProvidersJSON;
                    } else {
                        $('#' + Favorite_MedicationDetail.params.PanelID + " #ddlMedicationProvider").find("option:selected").removeAttr("selected");
                        $('#' + Favorite_MedicationDetail.params.PanelID + " #ddlMedicationProvider").multiselect("refresh");
                    }
                    Favorite_MedicationDetail.loadFavoriteListMedications(FavoriteListId);
                });
            }
            else
                utility.DisplayMessages(response.Message, 3);
        });
    },
    loadFavoriteListMedications: function (FavoriteListId, FavoriteListMedicationId, PageNo, rpp) {
        Favorite_MedicationDetail.searchFavoriteList_Medication_DBCall(FavoriteListMedicationId, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false)
                Favorite_MedicationDetail.FavoriteListMedGridLoad(response);
            else
                utility.DisplayMessages(response.Message, 3);
        });
    },
    deleteMedication: function (obj, FavoriteListId, FavMedicationId) {
        utility.myConfirm('1', function () {
            $(obj).parent().parent().remove();
            if (Favorite_MedicationDetail.params.FavoriteListId > 0 && FavMedicationId)
            {
                Favorite_MedicationDetail.deleteCPT_DB_Call(Favorite_MedicationDetail.params.FavoriteListId, FavMedicationId);
                Favorite_MedicationDetail.loadFavoriteListMedications(Favorite_MedicationDetail.params.FavoriteListId);
                Favorite_Medication.favoriteList_MedicationSearch(Favorite_MedicationDetail.params.FavoriteListId);
            }
                
        }, function () { }, '1');
        

    },
    deleteCPT_DB_Call: function (FavoriteListId, FavMedicationId) {
        var objData = {};
        objData["FavoriteListId"] = FavoriteListId;
        objData["Id"] = FavMedicationId;
        objData["commandType"] = "delete_medicationdetail";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteListMedicationDetail");
    },
    FavoriteListMedGridLoad: function (response) {
        $("#" + Favorite_MedicationDetail.params.PanelID + ' #ulFavMedication').empty();
        if (response.FavoriteListMedCount > 0) {
            var FavoriteListJSON = JSON.parse(response.FavoriteListMedJSON);
            $.each(FavoriteListJSON, function (i, item) {
                var onclick = "Favorite_MedicationDetail.deleteMedication(this,'" + item.FavoriteListId + "','" + item.Id + "');";
                var $list = $("#" + Favorite_MedicationDetail.params.PanelID + " #ulFavMedication");
                var li = '<li id=' + item.Id + ' FavListId=' + item.FavoriteListId + ' value =' + item.BrandName + " " + (item.Strength ? item.Strength : "") + '">' +
                   '<span class="pull-left pr-xlg">' + item.BrandName + " " + (item.Strength ? item.Strength : "") + ' </span>' +
                              '<span class="removeIconListHover">' +
                              '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Favorite_MedicationDetail.EditMedication(' + item.FavoriteListId + "," + item.Id + ');"><i class="fa fa-edit blue"></i></a>' +
                              '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="' + onclick + '"><i class="fa fa-times red"></i></a>' +
                              '</span>' +
                              '<div class="clearfix"></div>' +
                              '</li>';

                $list.append(li);
            });
        }
    },
    EditMedication: function (FavoriteListId, FavMedicationId) {
        var params = [];
        params["ParentCtrl"] = "Favorite_MedicationDetail";
        params["mode"] = "Edit";
        params["FromAdmin"] = 0;
        params["FavoriteListId"] = FavoriteListId;
        params["FavMedicationId"] = FavMedicationId;
        LoadActionPan("FavMedication_Detail", params);
    },
    FavMedicationSave: function () {
        if (globalAppdata['AppUserName'] == DefaultUser) {
            $("#" + Favorite_MedicationDetail.params.PanelID + " #frmFavoriteMedicationDetail #lstEntityId").enable = true;
            $("#" + Favorite_MedicationDetail.params.PanelID + " #frmFavoriteMedicationDetail").bootstrapValidator('revalidateField', 'EntityId');
        }
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Favorites_Medication", Favorite_MedicationDetail.params.mode.toUpperCase(), "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                $.when(defferedResponse = Favorite_MedicationDetail.SaveFavMedication()).done(function () {
                    if (defferedResponse.response != "") {
                        response = JSON.parse(defferedResponse.response);
                        if (response.status != false) {
                            var self = $('#' + Favorite_MedicationDetail.params.PanelID + ' #frmFavoriteMedicationDetail');
                            if (Favorite_MedicationDetail.params.mode == "Edit" && (self.find('#hfFavoriteListId').val() != "") || parseInt(self.find('#hfFavoriteListId').val()) >= 0)
                             Favorite_MedicationDetail.UnLoadTab();
                            Favorite_MedicationDetail.params.mode = "Edit";
                            $('#' + Favorite_MedicationDetail.params.PanelID + ' #frmFavoriteMedicationDetail #Active').prop("disabled", false);
                            self.find('#hfFavoriteListId').val(response.FavMedicationId);
                            $('#' + Favorite_MedicationDetail.params.PanelID + ' #divFavMedicationSearchList').removeClass('disableAll');
                            Favorite_Medication.favoriteListSearch();
                            utility.DisplayMessages(response.Message, 1);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    SaveFavMedication: function () {
        var objDeffered = $.Deferred();
        var objData = {};
        var self = $('#' + Favorite_MedicationDetail.params.PanelID + ' #frmFavoriteMedicationDetail');
        favoriteListId = self.find('#hfFavoriteListId').val();
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objDetail = JSON.parse(myJSON);
        if (objDetail["ProviderIds"] != '') {
            Favorite_MedicationDetail.validateProvider(2);
            objData["FavoriteListId"] = favoriteListId;
            objData["ListType"] = "Medication";
            objData["EntityId"] = objDetail["EntityId"];
            objData["BodyPartId"] = objDetail["BodyPartId"];
            objData["IsActive"] = objDetail.Active;
            objData["FavoriteListName"] = objDetail["FavoriteListName"];
            objData["ProviderIds"] = objDetail["ProviderIds"];
            if (globalAppdata['AppUserName'] != DefaultUser) {
                objData["EntityId"] = -1;
            }
            objData["FavoriteListMedicationIds"] = "";
            objData["commandType"] = "save_FavMedication";
            var data = JSON.stringify(objData);
            MDVisionService.APIService(data, "FavoriteList", "FavoriteList").done(function (response) {
                objDeffered.response = response;
                objDeffered.resolve();
            });
        }
        else {
            Favorite_MedicationDetail.validateProvider(1);
            objDeffered.resolve();
            objDeffered.response = "";
        }
        return objDeffered;
    },
    ValidateFavMedication: function () {
        var self = $('#' + Favorite_MedicationDetail.params.PanelID + ' #frmFavoriteMedicationDetail');
        self.bootstrapValidator({
            live: 'disabled',
            message: 'This value is not valid',
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {
                EntityId: {
                    enable: false,
                    group: '.col-sm-10',
                    validators: {
                        notEmpty: {
                            message: ''
                        },
                    }
                },
                FavoriteListName: {
                    group: '.col-sm-10',
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
         Favorite_MedicationDetail.FavMedicationSave();
     });
    },
    searchFavoriteList_Medication_DBCall: function (FavoriteListMedicationId, FavoriteListId, PageNumber, RowsPerPage, SearchData) {
        if (PageNumber == null)
            PageNumber = -1;
        if (RowsPerPage == null)
            RowsPerPage = -1;
        var objData = {};
        objData["IsActive"] = true;
        objData["FavoriteListId"] = FavoriteListId == null ? 0 : FavoriteListId;
        objData["FavoriteListMedicationId"] = FavoriteListMedicationId == null ? 0 : FavoriteListMedicationId;
        if (globalAppdata['AppUserName'] == DefaultUser)
            objData["EntityId"] = -1;
        else
            objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "load_favoritelist_Medication";
        objData["SearchData"] = SearchData;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },
    loadEntityProvider: function (entityId) {
        var objDeffered = $.Deferred();
        var data = "entityID=" + entityId;
        if (entityId != null && entityId > 0) {
            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $('#' + Favorite_MedicationDetail.params.PanelID + ' #ddlMedicationProvider');
                var $providerHiddenDdl = $('#' + Favorite_MedicationDetail.params.PanelID + ' #ddlHiddenFavMedicationProvider');
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
                if (Favorite_MedicationDetail.ProviderIds != '') {
                    var Providers = Favorite_MedicationDetail.ProviderIds.split(",");
                    Favorite_MedicationDetail.providerCheckedIds = Providers;
                    $('#' + Favorite_MedicationDetail.params.PanelID + ' #ddlMedicationProvider').val(Providers);
                }
            }).then(function () {
                //Intialized in onhidden spacialty ddl.
                Favorite_MedicationDetail.IntializeMultiSelectDropDownProviders();
                objDeffered.resolve();
            });
        }
        else
            objDeffered.resolve();
        return objDeffered;
    },
    IntializeMultiSelectDropDownProviders: function () {
        $('#' + Favorite_MedicationDetail.params.PanelID + ' #ddlMedicationProvider').multiselect('destroy');
        $('#' + Favorite_MedicationDetail.params.PanelID + ' #ddlMedicationProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 200,
            nonSelectedText: 'Select',
            selectAll: false,
            onChange: function () {
                Favorite_MedicationDetail.CheckProviderValidation();
            },
        });
    },
    CheckProviderValidation: function () {
        var self = $("#" + Favorite_MedicationDetail.params.PanelID);
        var ProviderIds = self.find('#ddlMedicationProvider option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        var strProviderIds = ProviderIds;
        Favorite_MedicationDetail.providerCheckedIds = ProviderIds;
        if (Favorite_MedicationDetail.providerCheckedIds != '')
            Favorite_MedicationDetail.validateProvider(2);
        else
            Favorite_MedicationDetail.validateProvider(1);
    },
    validateProvider: function (operationid) {
        var self = "#" + Favorite_MedicationDetail.params.PanelID + " #frmFavoriteMedicationDetail";
        $(self + " #divFavMedicationProvider").find("i").remove();
        if (operationid == 1) {
            $(self + " .multiselect").css("border-color", "#cc2724");
            $(self + " #divFavMedicationProvider").find(".control-label").css("color", "#cc2724");
            $(self + " #divFavMedicationProvider").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-remove" data-bv-icon-for="ProviderIds" style="display: block;"></i>');
        } else if (operationid == 2) {
            $(self + " .multiselect").css("border-color", "#3c763d");
            $(self + " #divFavMedicationProvider").find(".control-label").css("color", "#3c763d");
            $(self + " #divFavMedicationProvider").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-ok" data-bv-icon-for="ProviderIds" style="display: block;"></i>');
        } else {
            $(self + " .multiselect").css("border-color", "#ccc");
            $(self + " #divFavMedicationProvider").find(".control-label").css("color", "#000000");
        }
    },
    BindMedicationAutoComplete: function () {
        var ctrl = $('#' + Favorite_MedicationDetail.params.PanelID + ' #frmFavoriteMedicationDetail #txtMedication');
        var Med = $('#' + Favorite_MedicationDetail.params.PanelID + ' #frmFavoriteMedicationDetail #txtMedication').val();
        utility.Keyupdelay(function () {
            EMRUtility.GetMedicationsFromDrFirst(ctrl).done(function (response) {
                var responseMedication = response;
                $(ctrl).autocomplete({
                    autoFocus: true,
                    source: response,
                    select: function (event, ui) {
                        setTimeout(function () {
                            //$(ctrl).val(ui.item.value);
                            //var MedicationObject = ui.item;
                            Favorite_MedicationDetail.AddMedicationDetail(ui.item, $('#' + Favorite_MedicationDetail.params.PanelID + ' #frmFavoriteMedicationDetail #hfFavoriteListId').val());
                            $(ctrl).val("");
                        }, 100);
                    }
                });
                $(ctrl).autocomplete("search");
            });
        });
    },
    AddMedicationDetail: function (MedicationObject, FavoriteListId) {
        var params = [];
        params["ParentCtrl"] = "Favorite_MedicationDetail";
        params["mode"] =  "Add";
        params["FromAdmin"] = 0;
        params["FavoriteListId"] = FavoriteListId;
        params["MedicationObject"] = MedicationObject;
        LoadActionPan("FavMedication_Detail", params);
    },
    UnLoadTab: function () {
        var objDeffered = $.Deferred();
        if (Favorite_MedicationDetail.params["FromAdmin"] == "0") {
            if (Favorite_MedicationDetail.params != null && Favorite_MedicationDetail.params.ParentCtrl != null)
                UnloadActionPan(Favorite_MedicationDetail.params.ParentCtrl, 'Favorite_MedicationDetail');
            else
                UnloadActionPan(null, 'Favorite_MedicationDetail');
        }
        else
            RemoveAdminTab();
        objDeffered.resolve();
        return objDeffered;
    },
}