Favorite_ProcedureOrder = {
    bIsFirstLoad: true,
    params: [],
    Switch: 1,
    Load: function (params) {
        Favorite_ProcedureOrder.params = params; $("#" + Favorite_ProcedureOrder.params.PanelID + ' #dgvFavoritiesListCPT tbody').empty();
        if (Favorite_ProcedureOrder.params.PanelID != 'pnlFavoriteProcedureOrder') {
            Favorite_ProcedureOrder.params.PanelID = Favorite_ProcedureOrder.params.PanelID + ' #pnlFavoriteProcedureOrder';
        } else {
            Favorite_ProcedureOrder.params.PanelID = 'pnlFavoriteProcedureOrder';
        }
        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Favorite_ProcedureOrder.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        $("#" + Favorite_ProcedureOrder.params.PanelID + ' #dgvFavoritiesListCPT tbody').empty();
        $("#" + Favorite_ProcedureOrder.params.PanelID + ' #ulFavoritiesList').empty();
        Favorite_ProcedureOrder.favoriteListSearch();
    },

    domreadyFunctions: function () {
        $(function () {
            $("#" + Favorite_ProcedureOrder.params.PanelID + ' [data-plugin-toggle]').each(function () {
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
                    $("#" + Favorite_ProcedureOrder.params.PanelID + ' [data-plugin-ios-switch]').each(function () {
                        var $this = $(this);

                        $this.themePluginIOS7Switch();
                    });
                });
            }).apply(this, [jQuery]);
        });

    },
 
    favoriteListSearch: function (FavoriteListId, ListType, PageNo, rpp,ProviderId) {
        Favorite_ProcedureOrder.searchFavoriteList_DBCall(ListType, FavoriteListId, PageNo, rpp, ProviderId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Favorite_ProcedureOrder.FavoriteListGridLoad(response);
                Favorite_ProcedureOrder.domreadyFunctions();
                //if ($("#" + Favorite_ProcedureOrder.params.PanelID + ' #ulFavoritiesList li').length > 0) {
                //Start 24-03-2016 Humaira Yousaf for active/inactive
                if (response.FavoriteListCount > 0) {
                    $("#" + Favorite_ProcedureOrder.params.PanelID + ' #ulFavoritiesList li:first').trigger('click');
                }
                else {
                    $("#" + Favorite_ProcedureOrder.params.PanelID + ' #dgvFavoritiesListCPT tbody').empty();
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
            $("#" + Favorite_ProcedureOrder.params.PanelID + ' #ulFavoritiesList').empty();
            var FavoriteListJSON = JSON.parse(response.FavoriteListJSON);
            $.each(FavoriteListJSON, function (i, item) {
                var FavoriteListInfo = '<li onclick="Favorite_ProcedureOrder.loadFavoriteListCPT(' + item.FavoriteListId + ',this);">' +
                       '<span class="pull-left pr-xlg">' + item.Name + ' </span>' +
                       '<span class="removeIconListHover">' +
                       '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Favorite_ProcedureOrder.editFavoriteList(' + item.FavoriteListId + ');"><i class="fa fa-edit blue"></i></a>' +
                       '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Favorite_ProcedureOrder.deleteFavoriteList(' + item.FavoriteListId + ');"><i class="fa fa-times red"></i></a>' +
                       '</span>' +
                       '<div class="clearfix"></div>' +
                       '</li>';

                $("#" + Favorite_ProcedureOrder.params.PanelID + ' #ulFavoritiesList').append(FavoriteListInfo);
            });
        }
        //Begin 03-07-2016 Edit By Humaira Yousaf Bug# 1380
        else {
            $("#" + Favorite_ProcedureOrder.params.PanelID + ' #ulFavoritiesList').empty();
            $("#" + Favorite_ProcedureOrder.params.PanelID + ' #ulFavoritiesList').append('<li><span class="pull-left pr-xlg">No Favorites Found.</span></li>');
        }
        //End 03-07-2016 Edit By Humaira Yousaf Bug# 1380

    },
    loadFavoriteListCPT: function (FavoriteListId, obj) {
        $("#" + Favorite_ProcedureOrder.params.PanelID + ' #ulFavoritiesList li').removeClass("active");
        $(obj).addClass("active");
        Favorite_ProcedureOrder.favoriteList_CPTSearch(FavoriteListId);
    },

    //Method Name: deleteFavoriteList
    // Author Name: Abid Ali
    //Craeted Date: 24-03-2016
    //Description:  This function will delete favorite list procedure order
    editFavoriteList: function (favoriteListId) {

        Favorite_ProcedureOrder.AddFavoriteProcedureOrderDetail(favoriteListId);
    },


    favoriteList_CPTSearch: function (FavoriteListId, FavoriteListCPTId, PageNo, rpp, SearchData) {
        Favorite_ProcedureOrder.searchFavoriteList_CPT_DBCall(FavoriteListCPTId, FavoriteListId, PageNo, rpp,SearchData).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Favorite_ProcedureOrder.FavoriteListCPTGridLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    FavoriteListCPTGridLoad: function (response) {
        if (response.FavoriteListCPTCount > 0) {
            $("#" + Favorite_ProcedureOrder.params.PanelID + ' #dgvFavoritiesListCPT tbody').empty();
            var FavoriteListJSON = JSON.parse(response.FavoriteListCPTJSON);
            $.each(FavoriteListJSON, function (i, item) {
                var $row = $('<tr/>');
                $row.append('<td>' + item.CPTCode + '</td>');
                $row.append('<td>' + item.CPTCodeDescription + '</td>');
             //   $row.append('<td>' + item.SNOMEDID + '</td>');
               // $row.append('<td>' + item.CPT10CodeDescription + '</td>');

                $("#" + Favorite_ProcedureOrder.params.PanelID + ' #dgvFavoritiesListCPT tbody').last().append($row);
            });
        }
    },
    UnLoadTab: function () {
        if (Favorite_ProcedureOrder.params["FromAdmin"] == "0") {
            if (Favorite_ProcedureOrder.params != null && Favorite_ProcedureOrder.params.ParentCtrl != null) {
                UnloadActionPan(Favorite_ProcedureOrder.params.ParentCtrl, 'Favorite_ProcedureOrder');
            }
            else
                UnloadActionPan(null, 'Favorite_ProcedureOrder');
        }
        else {
            RemoveAdminTab();
        }
    },
    AddFavoriteProcedureOrderDetail: function (favoriteListId) {

        var params = [];
        params["ParentCtrl"] = "adminTabFavoritesProcedureOrder";
        if (favoriteListId > 0)
            params["mode"] = "Edit";
        else
            params["mode"] = "Add";
        params["FromAdmin"] = 0;
        params["FavoriteListId"] = favoriteListId;
        LoadActionPan("Favorite_ProcedureOrderDetail", params);
    },

    
    searchFavoriteList_CPT_DBCall: function (FavoriteListCPTId, FavoriteListId, PageNumber, RowsPerPage, SearchData) {
        if (PageNumber == null) {
            PageNumber = -1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = -1;
        }
        var objData = {};

        objData["FavoriteListId"] = FavoriteListId == null ? -1 : FavoriteListId;
        objData["FavoriteListCPTId"] = FavoriteListCPTId == null ? 0 : FavoriteListCPTId;
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["IsActive"] = true;
        objData["SearchData"] = SearchData;
        objData["commandType"] = "load_favoritelist_CPT";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },
    searchFavoriteList_DBCall: function (ListType, FavoriteListId, PageNumber, RowsPerPage, ProviderId, IsSelectForLookUp) {
        if (PageNumber == null) {
            PageNumber = -1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = -1;
        }
        var objData = {};
        objData["FavoriteListId"] = FavoriteListId == null ? 0 : FavoriteListId;
        objData["ListType"] = ListType == null ? 'ProcedureOrder' : ListType;
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        //Start Humaira Yousaf 24-03-2016 for active/inactive
        if (Favorite_ProcedureOrder.Switch == 1){
            objData["IsActive"] = true
        }
        else{
            objData["IsActive"] = false;
        }
        //End Humaira Yousaf 24-03-2016 for active/inactive
        objData["IsSelectForLookUp"] = IsSelectForLookUp;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["ProviderId"] = ProviderId;

        objData["commandType"] = "load_favoritelist";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },

     //Method Name: deleteFavoriteList
    // Author Name: Ahmad Raza
    //Craeted Date: 24-03-2016
    //Description:  This function will delete favorite list procedure order
    deleteFavoriteList: function (FavoriteListId) {
        AppPrivileges.GetFormPrivileges("Favorites_ProcedureOrder", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {

                    Favorite_ProcedureOrder.deleteFavoriteList_DBCall(FavoriteListId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }

                        Favorite_ProcedureOrder.favoriteListSearch();

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
            Favorite_ProcedureOrder.Switch = 0;
        }
        else if (isactive == '0') {
            $(obj).attr('isactive', '1');
            Favorite_ProcedureOrder.Switch = 1;
        }

        Favorite_ProcedureOrder.favoriteListSearch();
    },
}