Favorite_Vaccine = {
    bIsFirstLoad: true,
    params: [],
    Switch: 1,
    Load: function (params) {
        Favorite_Vaccine.params = params;
        if (Favorite_Vaccine.params.PanelID != 'pnlFavoriteVaccine') {
            Favorite_Vaccine.params.PanelID = Favorite_Vaccine.params.PanelID + ' #pnlFavoriteVaccine';
        } else {
            Favorite_Vaccine.params.PanelID = 'pnlFavoriteVaccine';
        }
        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Favorite_Vaccine.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        
        $("#" + Favorite_Vaccine.params.PanelID + ' #dgvFavoritiesListVaccine tbody').empty();
        $("#" + Favorite_Vaccine.params.PanelID + ' #ulFavoritiesList').empty();
        Favorite_Vaccine.LoadFavImmunization('vaccine');
        Favorite_Vaccine.domReadyFunctions();
    },
    domReadyFunctions: function () {
        $(function () {
            $("#" + Favorite_Vaccine.params.PanelID + ' [data-plugin-toggle]').each(function () {
                var $this = $(this),
                    opts = {};

                var pluginOptions = $this.data('plugin-options');
                if (pluginOptions)
                    opts = pluginOptions;

                $this.themePluginToggle(opts);
            });


            (function ($) {
                'use strict';
                $(function () {
                    $("#" + Favorite_Vaccine.params.PanelID + ' [data-plugin-ios-switch]').each(function () {
                        var $this = $(this);

                        $this.themePluginIOS7Switch();
                    });
                });
            }).apply(this, [jQuery]);
        });

    },
    UnLoadTab: function () {
        if (Favorite_Vaccine.params["FromAdmin"] == "0") {
            if (Favorite_Vaccine.params != null && Favorite_Vaccine.params.ParentCtrl != null) {
                UnloadActionPan(Favorite_Vaccine.params.ParentCtrl, 'Favorite_Vaccine');
            }
            else
                UnloadActionPan(null, 'Favorite_Vaccine');
        }
        else {
            RemoveAdminTab();
        }
    },
    AddFavoriteVaccineDetail: function () {
        var params = [];
        params["ParentCtrl"] = "adminTabFavoritesVaccine";
        params["mode"] = "Add";
        params["FromAdmin"] = 0;
        LoadActionPan("Favorite_Vaccine_Detail", params);

    },
    editFavoriteList: function (FavoritiesListId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var params = [];
        params["FavoritiesListId"] = FavoritiesListId;
        params["ParentCtrl"] = "adminTabFavoritesVaccine";
        params["mode"] = "Edit";
        params["FromAdmin"] = 0;
        LoadActionPan("Favorite_Vaccine_Detail", params);
    },
    LoadFavImmunization: function (type) {
        $("#" + Favorite_Vaccine.params.PanelID + ' #ulFavoritiesList').empty();
        if (type != "") {
            Favorite_Vaccine.LoadFavImmunization_DBCALL(type).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.FavImmunizationCount > 0) {
                        $.each(response.FavImmunization_JSON, function (i, item) {
                            var FavoriteListInfo = '<li onclick="Favorite_Vaccine.loadFavImmunizationDetail(' + item.FavoritiesListId + ',this);">' +
                      '<span class="pull-left pr-xlg">' + item.FavoriteListName + ' </span>' +
                      '<span class="removeIconListHover">' +
                      '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Favorite_Vaccine.editFavoriteList(' + item.FavoritiesListId + ',event);"><i class="fa fa-edit blue"></i></a>' +
                      '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Favorite_Vaccine.deleteFavoriteList(' + item.FavoritiesListId + ',event);"><i class="fa fa-times red"></i></a>' +
                      '</span>' +
                      '<div class="clearfix"></div>' +
                      '</li>';
                            $("#" + Favorite_Vaccine.params.PanelID + ' #ulFavoritiesList').append(FavoriteListInfo);
                        });
                        $("#" + Favorite_Vaccine.params.PanelID + ' #ulFavoritiesList li:first').trigger('click');
                    }
                    else {
                        $("#" + Favorite_Vaccine.params.PanelID + ' #dgvFavoritiesListVaccine tbody').empty();
                    }
                }
                else {
                    $("#" + Favorite_Vaccine.params.PanelID + ' #dgvFavoritiesListVaccine tbody').empty();
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    loadFavImmunizationDetail: function (FavoritiesListId) {
        $("#" + Favorite_Vaccine.params.PanelID + ' #dgvFavoritiesListVaccine tbody').empty();
        Favorite_Vaccine.LoadFavImmunization_Detail_DBCALL(FavoritiesListId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.FavImmunizationCount > 0) {
                    Favorite_Vaccine.FavImmunizationGridLoad(response.FavImmunization_JSON);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    FavImmunizationGridLoad: function (FavoriteListJSON) {
        $.each(FavoriteListJSON, function (i, item) {
            var $row = $('<tr/>');
            $row.append('<td>' + item.CategoryText + '</td>');
            $row.append('<td>' + item.VaccineCVX + '</td>');
            $row.append('<td>' + item.VaccineName + '</td>');
            $row.append('<td>' + item.CPTCode + '</td>');

            $("#" + Favorite_Vaccine.params.PanelID + ' #dgvFavoritiesListVaccine tbody').last().append($row);
        });
    },
    LoadFavImmunization_Detail_DBCALL: function (FavoritiesListId) {
        var objData = new Object();
        objData["commandType"] = "Load_Fav_Immunization_Detail";
        objData["FavoritiesListId"] = FavoritiesListId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "Immunization");
    },
    LoadFavImmunization_DBCALL: function (Type) {
        var objData = new Object();
        objData["commandType"] = "Load_Fav_Immunization";
        objData["Type"] = Type;
        objData["ProviderIds"] = 0;
        objData["EntityId"]=globalAppdata["SeletedEntityId"]
        objData["IsActive"] = $('#' + Favorite_Vaccine.params.PanelID + ' #divSwitch #switchActive').is(':checked');
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "Immunization");
    },
    deleteFavoriteList: function (FavoriteListId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Favorites_Vaccine", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {

                    Favorite_Vaccine.deleteFavoriteList_DBCall(FavoriteListId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            Favorite_Vaccine.LoadFavImmunization('vaccine');
                        }
                        else {
                            var ErrorMessage = response.Message.split('|Delete issue.');
                            utility.DisplayMessages(ErrorMessage[0], 3);
                        }

                        

                    });

                }, function () { },
                   '1'
        );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    deleteFavoriteList_DBCall: function (FavoritiesListId) {

        var objData = new Object();
        objData["FavoritiesListId"] = FavoritiesListId;
        objData["commandType"] = "DELETE_FAVORITE_IMMUNIZATION";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "Immunization");
    },
}