Favorite_TherapueticInjection = {
    bIsFirstLoad: true,
    params: [],
    Switch: 1,
    Load: function (params) {
        Favorite_TherapueticInjection.params = params;
        if (Favorite_TherapueticInjection.params.PanelID != 'pnlFavoriteTherapueticInjection') {
            Favorite_TherapueticInjection.params.PanelID = Favorite_TherapueticInjection.params.PanelID + ' #pnlFavoriteTherapueticInjection';
        } else {
            Favorite_TherapueticInjection.params.PanelID = 'pnlFavoriteTherapueticInjection';
        }
        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Favorite_TherapueticInjection.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        $("#" + Favorite_TherapueticInjection.params.PanelID + ' #dgvFavoritiesListTherapuetic tbody').empty();
        $("#" + Favorite_TherapueticInjection.params.PanelID + ' #ulFavoritiesList').empty();
        Favorite_TherapueticInjection.LoadFavImmunization('therapuetic');
        Favorite_TherapueticInjection.domReadyFunctions();


        //$("#" + Favorite_TherapueticInjection.params.PanelID + ' #dgvFavoritiesListICD tbody').empty();
        //$("#" + Favorite_TherapueticInjection.params.PanelID + ' #ulFavoritiesList').empty();
        //Favorite_TherapueticInjection.favoriteListSearch();
    },
    domReadyFunctions: function () {
        $(function () {
            $("#" + Favorite_TherapueticInjection.params.PanelID + ' [data-plugin-toggle]').each(function () {
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
                    $("#" + Favorite_TherapueticInjection.params.PanelID + ' [data-plugin-ios-switch]').each(function () {
                        var $this = $(this);

                        $this.themePluginIOS7Switch();
                    });
                });
            }).apply(this, [jQuery]);
        });

    },
    UnLoadTab: function () {
        if (Favorite_TherapueticInjection.params["FromAdmin"] == "0") {
            if (Favorite_TherapueticInjection.params != null && Favorite_TherapueticInjection.params.ParentCtrl != null) {
                UnloadActionPan(Favorite_TherapueticInjection.params.ParentCtrl, 'Favorite_Complaints');
            }
            else
                UnloadActionPan(null, 'Favorite_TherapueticInjection');
        }
        else {
            RemoveAdminTab();
        }
    },
    AddFavoriteTherapueticInjectionDetail: function () {
        var params = [];
        params["ParentCtrl"] = "adminTabFavoritesTherapueticInjection";
        params["mode"] = "Add";
        params["FromAdmin"] = 0;
        LoadActionPan("Favorite_TherapueticInjection_Detail", params);
    },

    editFavoriteList: function (FavoritiesListId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var params = [];
        params["FavoritiesListId"] = FavoritiesListId;
        params["ParentCtrl"] = "adminTabFavoritesTherapueticInjection";
        params["mode"] = "Edit";
        params["FromAdmin"] = 0;
        LoadActionPan("Favorite_TherapueticInjection_Detail", params);
    },
    LoadFavImmunization: function (type) {
        $("#" + Favorite_TherapueticInjection.params.PanelID + ' #ulFavoritiesList').empty();
        if (type != "") {
            Favorite_TherapueticInjection.LoadFavImmunization_DBCALL(type).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (response.FavImmunizationCount > 0) {
                        $.each(response.FavImmunization_JSON, function (i, item) {
                            var FavoriteListInfo = '<li onclick="Favorite_TherapueticInjection.loadFavImmunizationDetail(' + item.FavoritiesListId + ',this);">' +
                      '<span class="pull-left pr-xlg">' + item.FavoriteListName + ' </span>' +
                      '<span class="removeIconListHover">' +
                      '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Favorite_TherapueticInjection.editFavoriteList(' + item.FavoritiesListId + ',event);"><i class="fa fa-edit blue"></i></a>' +
                      '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Favorite_TherapueticInjection.deleteFavoriteList(' + item.FavoritiesListId + ',event);"><i class="fa fa-times red"></i></a>' +
                      '</span>' +
                      '<div class="clearfix"></div>' +
                      '</li>';
                            $("#" + Favorite_TherapueticInjection.params.PanelID + ' #ulFavoritiesList').append(FavoriteListInfo);
                        });
                        $("#" + Favorite_TherapueticInjection.params.PanelID + ' #ulFavoritiesList li:first').trigger('click');
                    }
                    else {
                        $("#" + Favorite_TherapueticInjection.params.PanelID + ' #dgvFavoritiesListTherapuetic tbody').empty();
                    }
                }
                else {
                    $("#" + Favorite_TherapueticInjection.params.PanelID + ' #dgvFavoritiesListTherapuetic tbody').empty();
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    loadFavImmunizationDetail: function (FavoritiesListId) {
        $("#" + Favorite_TherapueticInjection.params.PanelID + ' #dgvFavoritiesListTherapuetic tbody').empty();
        Favorite_TherapueticInjection.LoadFavImmunization_Detail_DBCALL(FavoritiesListId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.FavImmunizationCount > 0) {
                    Favorite_TherapueticInjection.FavImmunizationGridLoad(response.FavImmunization_JSON);
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
            $row.append('<td>' + item.VaccineName + '</td>');
            $row.append('<td>' + (item.VaccineCVX!=null?item.VaccineCVX:"") + '</td>');
            $row.append('<td>' + item.AdministeredCode + '</td>');
            $row.append('<td>' + item.CPTCode + '</td>');
            $("#" + Favorite_TherapueticInjection.params.PanelID + ' #dgvFavoritiesListTherapuetic tbody').last().append($row);
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
        objData["EntityId"] = globalAppdata["SeletedEntityId"]
        objData["IsActive"] = $('#' + Favorite_TherapueticInjection.params.PanelID + ' #divSwitch #switchActive').is(':checked');
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "Immunization");
    },
    deleteFavoriteList: function (FavoriteListId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Favorites_TherapueticInjection", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {

                    Favorite_TherapueticInjection.deleteFavoriteList_DBCall(FavoriteListId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            Favorite_TherapueticInjection.LoadFavImmunization('Therapuetic');
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