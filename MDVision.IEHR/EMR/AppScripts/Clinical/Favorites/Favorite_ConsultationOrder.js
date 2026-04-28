Favorite_ConsultationOrder = {
    bIsFirstLoad: true,
    params: [],
    Switch: 1,
    Load: function (params) {
        Favorite_ConsultationOrder.params = params;
        if (Favorite_ConsultationOrder.params.PanelID != 'pnlFavoriteConsultationOrder') {
            Favorite_ConsultationOrder.params.PanelID = Favorite_ConsultationOrder.params.PanelID + ' #pnlFavoriteConsultationOrder';
        } else {
            Favorite_ConsultationOrder.params.PanelID = 'pnlFavoriteConsultationOrder';
        }
        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Favorite_ConsultationOrder.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        Favorite_ConsultationOrder.emptyGridAndList();
        Favorite_ConsultationOrder.favoriteListSearch();
    },

    domreadyFunctions: function () {
        $(function () {
            $("#" + Favorite_ConsultationOrder.params.PanelID + ' [data-plugin-toggle]').each(function () {
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
                    $("#" + Favorite_ConsultationOrder.params.PanelID + ' [data-plugin-ios-switch]').each(function () {
                        var $this = $(this);

                        $this.themePluginIOS7Switch();
                    });
                });
            }).apply(this, [jQuery]);
        });

    },

    favoriteListSearch: function (FavoriteListId, ListType, PageNo, rpp) {
        Favorite_ConsultationOrder.searchFavoriteList_DBCall(ListType, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Favorite_ConsultationOrder.FavoriteListGridLoad(response);
                Favorite_ConsultationOrder.domreadyFunctions();
                if ($("#" + Favorite_ConsultationOrder.params.PanelID + ' #ulFavoritiesList li').length > 0) {
                    $("#" + Favorite_ConsultationOrder.params.PanelID + ' #ulFavoritiesList li:first').trigger('click');
                }

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    FavoriteListGridLoad: function (response) {
        if (response.FavoriteListCount > 0) {
            $("#" + Favorite_ConsultationOrder.params.PanelID + ' #ulFavoritiesList').empty();
            var FavoriteListJSON = JSON.parse(response.FavoriteListJSON);
            $.each(FavoriteListJSON, function (i, item) {
                var FavoriteListInfo = '<li onclick="Favorite_ConsultationOrder.loadFavoriteListCPT(' + item.FavoriteListId + ',this);">' +
                       '<span class="pull-left pr-xlg">' + item.Name + ' </span>' +
                       '<span class="removeIconListHover">' +
                       '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Favorite_ConsultationOrder.editFavoriteList(' + item.FavoriteListId + ');"><i class="fa fa-edit blue"></i></a>' +
                       '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Favorite_ConsultationOrder.deleteFavoriteList(' + item.FavoriteListId + ');"><i class="fa fa-times red"></i></a>' +
                       '</span>' +
                       '<div class="clearfix"></div>' +
                       '</li>';

                $("#" + Favorite_ConsultationOrder.params.PanelID + ' #ulFavoritiesList').append(FavoriteListInfo);
            });
        }
        //Start 24-03-2016 Humaira Yousaf for active/inactive
        else {
            $("#" + Favorite_ConsultationOrder.params.PanelID + ' #ulFavoritiesList').empty();
            $("#" + Favorite_ConsultationOrder.params.PanelID + ' #ulFavoritiesList').append('<li><span class="pull-left pr-xlg">No Favorites Found.</span></li>');
            //Begin 15-07-2016 Edit By Humaira Yousaf Bug#1380
	    $("#" + Favorite_ConsultationOrder.params.PanelID + " #dgvFavoritiesListCPT tr").not(':first').remove();
	    //End 15-07-2016 Edit By Humaira Yousaf Bug#1380
        }
        //End 24-03-2016 Humaira Yousaf for active/inactive
    },

    loadFavoriteListCPT: function (FavoriteListId, obj) {
        $("#" + Favorite_ConsultationOrder.params.PanelID + ' #ulFavoritiesList li').removeClass("active");
        $(obj).addClass("active");
        Favorite_ConsultationOrder.favoriteList_CPTSearch(FavoriteListId);
    },

    //Method Name: deleteFavoriteList
    // Author Name: Abid Ali
    //Craeted Date: 24-03-2016
    //Description:  This function will delete favorite list consultation order
    editFavoriteList: function (favoriteListId) {
        //Load Consultation Order Detail for edit
        Favorite_ConsultationOrder.AddFavoriteConsultationOrderDetail(favoriteListId);
    },

    //Method Name: deleteFavoriteList
    // Author Name: Abid Ali
    //Craeted Date: 24-03-2016
    //Description:  This function will delete favorite list consultation order
    deleteFavoriteList: function (FavoriteListId) {
        AppPrivileges.GetFormPrivileges("Favorites_ConsultationOrder", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {

                    Favorite_ConsultationOrder.deleteFavoriteList_DBCall(FavoriteListId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }

                        Favorite_ConsultationOrder.favoriteListSearch();

                    });

                }, function () { },
                   '1'
        );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    favoriteList_CPTSearch: function (FavoriteListId, FavoriteListCPTId, PageNo, rpp) {
        Favorite_ConsultationOrder.searchFavoriteList_CPT_DBCall(FavoriteListCPTId, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Favorite_ConsultationOrder.FavoriteListCPTGridLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    FavoriteListCPTGridLoad: function (response) {
        if (response.FavoriteListCPTCount > 0) {
            $("#" + Favorite_ConsultationOrder.params.PanelID + ' #dgvFavoritiesListCPT tbody').empty();
            var FavoriteListJSON = JSON.parse(response.FavoriteListCPTJSON);
            $.each(FavoriteListJSON, function (i, item) {
                var $row = $('<tr/>');
                $row.append('<td>' + item.CPTCode + '</td>');
                $row.append('<td>' + item.CPTCodeDescription + '</td>');
                //$row.append('<td>' + item.SNOMEDID + '</td>');
                //$row.append('<td>' + item.CPT10CodeDescription + '</td>');

                $("#" + Favorite_ConsultationOrder.params.PanelID + ' #dgvFavoritiesListCPT tbody').last().append($row);
            });
        }
    },

    UnLoadTab: function () {
        if (Favorite_ConsultationOrder.params["FromAdmin"] == "0") {
            if (Favorite_ConsultationOrder.params != null && Favorite_ConsultationOrder.params.ParentCtrl != null) {
                UnloadActionPan(Favorite_ConsultationOrder.params.ParentCtrl, 'Favorite_ConsultationOrder');
            }
            else
                UnloadActionPan(null, 'Favorite_ConsultationOrder');
        }
        else {
            RemoveAdminTab();
        }
    },

    AddFavoriteConsultationOrderDetail: function (favoriteListId) {

        var params = [];
        params["ParentCtrl"] = "adminTabFavoritesConsultationOrder";
        if (favoriteListId > 0)
            params["mode"] = "Edit";
        else
            params["mode"] = "Add";
        params["FromAdmin"] = 0;
        params["FavoriteListId"] = favoriteListId;
        LoadActionPan("Favorite_ConsultationOrderDetail", params);
    },

    //Method Name: activeFavoritesSearch
    // Author Name: Abid Ali
    //Craeted Date: 25-03-2016
    //Description:  Searches active and iactive records
    activeFavoritesSearch: function (obj) {
        var isactive = $(obj).attr('isactive');

        if (isactive == '1') {
            $(obj).attr('isactive', '0');
            Favorite_ConsultationOrder.Switch = 0;
        }
        else if (isactive == '0') {
            $(obj).attr('isactive', '1');
            Favorite_ConsultationOrder.Switch = 1;
        }

        Favorite_ConsultationOrder.emptyGridAndList();
        Favorite_ConsultationOrder.favoriteListSearch();
    },

    emptyGridAndList:function(){
        $("#" + Favorite_ConsultationOrder.params.PanelID + ' #dgvFavoritiesListCPT tbody').empty();
        $("#" + Favorite_ConsultationOrder.params.PanelID + ' #ulFavoritiesList').empty();
    },

    /****************************************************
            DB Calls For Favorite Lists
    *****************************************************/

    //Method Name: searchFavoriteList_DBCall
    // Author Name: Abid Ali
    //Craeted Date: 24-03-2016
    //Description:  This function will call DB to search favorite list CPT consultation order
    searchFavoriteList_CPT_DBCall: function (FavoriteListCPTId, FavoriteListId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = -1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = -1;
        }
        var objData = {};

        objData["FavoriteListId"] = FavoriteListId == null ? 0 : FavoriteListId;
        objData["FavoriteListCPTId"] = FavoriteListCPTId == null ? -1 : FavoriteListCPTId;
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;

        objData["commandType"] = "load_favoritelist_CPT";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },

    //Method Name: searchFavoriteList_DBCall
    // Author Name: Abid Ali
    //Craeted Date: 24-03-2016
    //Description:  This function will call DB to search favorite list consultation order
    searchFavoriteList_DBCall: function (ListType, FavoriteListId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = -1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = -1;
        }
        var objData = {};
        objData["FavoriteListId"] = FavoriteListId == null ? 0 : FavoriteListId;
        objData["ListType"] = ListType == null ? 'ConsultationOrder' : ListType;
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        
        if (Favorite_ConsultationOrder.Switch == 1) {
            objData["IsActive"] = true
        }
        else {
            objData["IsActive"] = false;
        }     


        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;       
        objData["commandType"] = "load_favoritelist";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },

    //Method Name: deleteFavoriteList_DBCall
    // Author Name: Abid Ali
    //Craeted Date: 24-03-2016
    //Description:  This function will call DB to delete favorite list consultation order
    deleteFavoriteList_DBCall: function (favoriteListId) {

        var objData = new Object();
        objData["FavoriteListId"] = favoriteListId;
        objData["commandType"] = "DELETE_FAVORITELIST_PROCEDURE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },

   
}