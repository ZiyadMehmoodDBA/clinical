Favorite_Procedure = {
    bIsFirstLoad: true,
    params: [],
    Switch: 1,
    Load: function (params) {
        Favorite_Procedure.params = params; $("#" + Favorite_Procedure.params.PanelID + ' #dgvFavoritiesListCPT tbody').empty();
        if (Favorite_Procedure.params.PanelID != 'pnlFavoriteProcedure') {
            Favorite_Procedure.params.PanelID = Favorite_Procedure.params.PanelID + ' #pnlFavoriteProcedure';
        } else {
            Favorite_Procedure.params.PanelID = 'pnlFavoriteProcedure';
        }
        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Favorite_Procedure.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        $("#" + Favorite_Procedure.params.PanelID + ' #dgvFavoritiesListCPT tbody').empty();
        $("#" + Favorite_Procedure.params.PanelID + ' #ulFavoritiesList').empty();
        Favorite_Procedure.favoriteListSearch();
    },

    domreadyFunctions: function () {
        $(function () {
            $("#" + Favorite_Procedure.params.PanelID + ' [data-plugin-toggle]').each(function () {
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
                    $("#" + Favorite_Procedure.params.PanelID + ' [data-plugin-ios-switch]').each(function () {
                        var $this = $(this);

                        $this.themePluginIOS7Switch();
                    });
                });
            }).apply(this, [jQuery]);
        });

    },

    favoriteListSearch: function (FavoriteListId, ListType, PageNo, rpp) {
        Favorite_Procedure.searchFavoriteList_DBCall(ListType, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Favorite_Procedure.FavoriteListGridLoad(response);
                Favorite_Procedure.domreadyFunctions();
                //if ($("#" + Favorite_Procedure.params.PanelID + ' #ulFavoritiesList li').length > 0) {
                //Start 24-03-2016 Humaira Yousaf for active/inactive
                if (response.FavoriteListCount > 0) {

                    if ($("#" + Favorite_Procedure.params.PanelID + " #ulFavoritiesList #favli_" + Favorite_Procedure.params["SelectedFavriteItem"]).length > 0) {
                        $("#" + Favorite_Procedure.params.PanelID + " #ulFavoritiesList #favli_" + Favorite_Procedure.params["SelectedFavriteItem"]).trigger('click');
                    }
                    else { $("#" + Favorite_Procedure.params.PanelID + ' #ulFavoritiesList li:first').trigger('click'); }
                }
                else {
                    $("#" + Favorite_Procedure.params.PanelID + ' #dgvFavoritiesListCPT tbody').empty();
                }
                //End 24-03-2016 Humaira Yousaf for active/inactive

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    FavoriteListGridLoad: function (response) {

        if (response.FavoriteListCount > 0) {
            $("#" + Favorite_Procedure.params.PanelID + ' #ulFavoritiesList').empty();
            var FavoriteListJSON = JSON.parse(response.FavoriteListJSON);
            $.each(FavoriteListJSON, function (i, item) {
                var FavoriteListInfo = '<li id="favli_' + item.FavoriteListId + '" onclick="Favorite_Procedure.loadFavoriteListCPT(' + item.FavoriteListId + ',this);">' +
                       '<span class="pull-left pr-xlg">' + item.Name + ' </span>' +
                       '<span class="removeIconListHover">' +
                       '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Favorite_Procedure.editFavoriteList(' + item.FavoriteListId + ');"><i class="fa fa-edit blue"></i></a>' +
                       '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Favorite_Procedure.deleteFavoriteList(' + item.FavoriteListId + ');"><i class="fa fa-times red"></i></a>' +
                       '</span>' +
                       '<div class="clearfix"></div>' +
                       '</li>';

                $("#" + Favorite_Procedure.params.PanelID + ' #ulFavoritiesList').append(FavoriteListInfo);
            });
        }
            //Start 24-03-2016 Humaira Yousaf for active/inactive
        else {
            $("#" + Favorite_Procedure.params.PanelID + ' #ulFavoritiesList').empty();
            $("#" + Favorite_Procedure.params.PanelID + ' #ulFavoritiesList').append('<li><span class="pull-left pr-xlg">No Favorites Found.</span></li>');
        }
        //End 24-03-2016 Humaira Yousaf for active/inactive

    },
    loadFavoriteListCPT: function (FavoriteListId, obj) {
        $("#" + Favorite_Procedure.params.PanelID + ' #ulFavoritiesList li').removeClass("active");
        $(obj).addClass("active");
        Favorite_Procedure.favoriteList_CPTSearch(FavoriteListId);
    },

    //Method Name: deleteFavoriteList
    // Author Name: Abid Ali
    //Craeted Date: 24-03-2016
    //Description:  This function will delete favorite list procedure order
    editFavoriteList: function (favoriteListId) {

        var params = [];
        params["ParentCtrl"] = "adminTabFavoritesProcedure";
        params["mode"] = "Edit";
        params["FromAdmin"] = 0;
        params["FavoriteListId"] = favoriteListId;
        Favorite_Procedure.params["SelectedFavriteItem"] = favoriteListId;
        LoadActionPan("Favorite_ProcedureDetail", params);
    },


    favoriteList_CPTSearch: function (FavoriteListId, FavoriteListCPTId, PageNo, rpp) {
        Favorite_Procedure.searchFavoriteList_CPT_DBCall(FavoriteListCPTId, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Favorite_Procedure.FavoriteListCPTGridLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    FavoriteListCPTGridLoad: function (response) {
        if (response.FavoriteListCPTCount > 0) {
            $("#" + Favorite_Procedure.params.PanelID + ' #dgvFavoritiesListCPT tbody').empty();
            var FavoriteListJSON = JSON.parse(response.FavoriteListCPTJSON);
            $.each(FavoriteListJSON, function (i, item) {
                var $row = $('<tr/>');
                $row.append('<td>' + item.CPTCode + '</td>');
                $row.append('<td>' + item.CPTCodeDescription + '</td>');
                $row.append('<td>' + item.Unit + '</td>');
                $row.append('<td>' + item.Modifier + '</td>');
                //   $row.append('<td>' + item.SNOMEDID + '</td>');
                // $row.append('<td>' + item.CPT10CodeDescription + '</td>');

                $("#" + Favorite_Procedure.params.PanelID + ' #dgvFavoritiesListCPT tbody').last().append($row);
            });
        }
    },
    UnLoadTab: function () {
        if (Favorite_Procedure.params["FromAdmin"] == "0") {
            if (Favorite_Procedure.params != null && Favorite_Procedure.params.ParentCtrl != null) {
                UnloadActionPan(Favorite_Procedure.params.ParentCtrl, 'Favorite_Procedure');
            }
            else
                UnloadActionPan(null, 'Favorite_Procedure');
        }
        else {
            RemoveAdminTab();
        }
    },
    AddFavoriteProcedureDetail: function (favoriteListId) {

        var params = [];
        params["ParentCtrl"] = "adminTabFavoritesProcedure";
        params["mode"] = "Add";
        params["FromAdmin"] = 0;
        params["FavoriteListId"] = favoriteListId;
        LoadActionPan("Favorite_ProcedureDetail", params);
    },


    searchFavoriteList_CPT_DBCall: function (FavoriteListCPTId, FavoriteListId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = -1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = -1;
        }
        var objData = {};

        objData["FavoriteListId"] = FavoriteListId == null ? 0 : FavoriteListId;
        objData["FavoriteListCPTId"] = FavoriteListCPTId == null ? 0 : FavoriteListCPTId;
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["IsActive"] = true;
        objData["commandType"] = "load_favoritelist_CPT";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },
    searchFavoriteList_DBCall: function (ListType, FavoriteListId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = -1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = -1;
        }
        var objData = {};
        objData["FavoriteListId"] = FavoriteListId == null ? 0 : FavoriteListId;
        objData["ListType"] = ListType == null ? 'Procedure' : ListType;

        if (globalAppdata['AppUserName'] == DefaultUser) {
            objData["EntityId"] = -1;
        }
        else {

            objData["EntityId"] = globalAppdata["SeletedEntityId"];
        }
        //Start Humaira Yousaf 24-03-2016 for active/inactive
        if (Favorite_Procedure.Switch == 1) {
            objData["IsActive"] = true
        }
        else {
            objData["IsActive"] = false;
        }
        //End Humaira Yousaf 24-03-2016 for active/inactive

        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;

        objData["commandType"] = "load_favoritelist";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },

    //Method Name: deleteFavoriteList
    // Author Name: Ahmad Raza
    //Craeted Date: 24-03-2016
    //Description:  This function will delete favorite list procedure order
    deleteFavoriteList: function (FavoriteListId) {
        AppPrivileges.GetFormPrivileges("Favorites_Procedure", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {

                    Favorite_Procedure.deleteFavoriteList_DBCall(FavoriteListId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }

                        Favorite_Procedure.favoriteListSearch();

                    });

                }, function () { },
                   '1'
        );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    //Method Name: deleteFavoriteList_DBCall
    // Author Name: Ahmad Raza
    //Craeted Date: 24-03-2016
    //Description:  This function will call DB to delete favorite list procedure order
    deleteFavoriteList_DBCall: function (favoriteListId) {

        var objData = new Object();
        objData["FavoriteListId"] = favoriteListId;
        objData["commandType"] = "DELETE_FAVORITELIST_PROCEDURE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },


    //Method Name: activeFavoritesSearch
    // Author Name: Humaira Yousaf
    //Craeted Date: 24-03-2016
    //Description:  Searches active and iactive records
    activeFavoritesSearch: function (obj) {
        var isactive = $(obj).attr('isactive');

        if (isactive == '1') {
            $(obj).attr('isactive', '0');
            Favorite_Procedure.Switch = 0;
        }
        else if (isactive == '0') {
            $(obj).attr('isactive', '1');
            Favorite_Procedure.Switch = 1;
        }

        Favorite_Procedure.favoriteListSearch();
    },
}