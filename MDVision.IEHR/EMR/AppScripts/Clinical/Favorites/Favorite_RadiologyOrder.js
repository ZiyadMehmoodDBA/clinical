Favorite_RadiologyOrder = {
    bIsFirstLoad: true,
    params: [],
    Switch: 1,
    Load: function (params) {
        Favorite_RadiologyOrder.params = params;
        if (Favorite_RadiologyOrder.params.PanelID != 'pnlFavoriteRadiologyOrder') {
            Favorite_RadiologyOrder.params.PanelID = Favorite_RadiologyOrder.params.PanelID + ' #pnlFavoriteRadiologyOrder';
        } else {
            Favorite_RadiologyOrder.params.PanelID = 'pnlFavoriteRadiologyOrder';
        }
        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Favorite_RadiologyOrder.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        $("#" + Favorite_RadiologyOrder.params.PanelID + ' #dgvFavoritiesListCPT tbody').empty();
        $("#" + Favorite_RadiologyOrder.params.PanelID + ' #ulFavoritiesList').empty();
        Favorite_RadiologyOrder.favoriteListSearch();
    },

    domreadyFunctions: function () {
        $(function () {
            $("#" + Favorite_RadiologyOrder.params.PanelID + ' [data-plugin-toggle]').each(function () {
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
                    $("#" + Favorite_RadiologyOrder.params.PanelID + ' [data-plugin-ios-switch]').each(function () {
                        var $this = $(this);

                        $this.themePluginIOS7Switch();
                    });
                });
            }).apply(this, [jQuery]);
        });

    },

    favoriteListSearch: function (FavoriteListId, ListType, PageNo, rpp) {
        Favorite_RadiologyOrder.searchFavoriteList_DBCall(ListType, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Favorite_RadiologyOrder.FavoriteListGridLoad(response);
                Favorite_RadiologyOrder.domreadyFunctions();
		        //Start 24-03-2016 Humaira Yousaf for active/inactive
                if (response.FavoriteListCount > 0) {
                    $("#" + Favorite_RadiologyOrder.params.PanelID + ' #ulFavoritiesList li:first').trigger('click');
                }
                else {
                    $("#" + Favorite_RadiologyOrder.params.PanelID + ' #dgvFavoritiesListCPT tbody').empty();
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
            $("#" + Favorite_RadiologyOrder.params.PanelID + ' #ulFavoritiesList').empty();
            var FavoriteListJSON = JSON.parse(response.FavoriteListJSON);
            $.each(FavoriteListJSON, function (i, item) {
                var FavoriteListInfo = '<li onclick="Favorite_RadiologyOrder.loadFavoriteListCPT(' + item.FavoriteListId + ',this);">' +
                       '<span class="pull-left pr-xlg">' + item.Name + ' </span>' +
                       '<span class="removeIconListHover">' +
                       '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Favorite_RadiologyOrder.editFavoriteList(' + item.FavoriteListId + ');"><i class="fa fa-edit blue"></i></a>' +
                       '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Favorite_RadiologyOrder.deleteFavoriteList(' + item.FavoriteListId + ');"><i class="fa fa-times red"></i></a>' +
                       '</span>' +
                       '<div class="clearfix"></div>' +
                       '</li>';

                $("#" + Favorite_RadiologyOrder.params.PanelID + ' #ulFavoritiesList').append(FavoriteListInfo);
            });
        }
	//Start 24-03-2016 Humaira Yousaf for active/inactive
        else {
            $("#" + Favorite_RadiologyOrder.params.PanelID + ' #ulFavoritiesList').empty();
            $("#" + Favorite_RadiologyOrder.params.PanelID + ' #ulFavoritiesList').append('<li><span class="pull-left pr-xlg">No Favorites Found.</span></li>');
        }
	//End 24-03-2016 Humaira Yousaf for active/inactive

    },
    loadFavoriteListCPT: function (FavoriteListId, obj) {
        $("#" + Favorite_RadiologyOrder.params.PanelID + ' #ulFavoritiesList li').removeClass("active");
        $(obj).addClass("active");
        Favorite_RadiologyOrder.favoriteList_CPTSearch(FavoriteListId);
    },
    //Method Name: editFavoriteList
    // Author Name: Humaira Yousaf
    //Craeted Date: 24-03-2016
    //Description:  Updates favourite list
    editFavoriteList: function (favoriteListId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Favorites_ProcedureOrder", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
               var params = [];
                if (favoriteListId != null && parseInt(favoriteListId) > 0) {
                    params["FavoriteRadiologyOrderId"] = favoriteListId;
                    params["mode"] = "Edit";
                }
                else {
                    params["FavoriteRadiologyOrderId"] = -1;
                    params["mode"] = "Add";
                }
                params["FromAdmin"] = Clinical_ConsultationOrder.params["FromAdmin"];                
                params["ParentCtrl"] = 'adminTabFavoritesRadiologyOrder';
                params["TabID"] = 'Favorite_RadiologyOrderDetail';
                params["FromAdmin"] = 0;
                LoadActionPan('Favorite_RadiologyOrderDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },
  
    //Method Name: deleteFavoriteList
    // Author Name: Humaira Yousaf
    //Craeted Date: 24-03-2016
    //Description:  This function will delete favorite list radiology order
    deleteFavoriteList: function (FavoriteListId) {
        AppPrivileges.GetFormPrivileges("Favorites_ProcedureOrder", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {

                    Favorite_RadiologyOrder.deleteFavoriteList_DBCall(FavoriteListId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }

                        Favorite_RadiologyOrder.favoriteListSearch();

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
        Favorite_RadiologyOrder.searchFavoriteList_CPT_DBCall(FavoriteListCPTId, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Favorite_RadiologyOrder.FavoriteListCPTGridLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    FavoriteListCPTGridLoad: function (response) {
        if (response.FavoriteListCPTCount > 0) {
            $("#" + Favorite_RadiologyOrder.params.PanelID + ' #dgvFavoritiesListCPT tbody').empty();
            var FavoriteListJSON = JSON.parse(response.FavoriteListCPTJSON);
            $.each(FavoriteListJSON, function (i, item) {
                var $row = $('<tr/>');
		    //Start 24-03-2016 Humaira Yousaf
                $row.append('<td>' + item.CPTCode + '</td>');
                $row.append('<td>' + item.CPTCodeDescription + '</td>');
		    //End 24-03-2016 Humaira Yousaf
                $("#" + Favorite_RadiologyOrder.params.PanelID + ' #dgvFavoritiesListCPT tbody').last().append($row);
            });
        }
    },
    UnLoadTab: function () {
        if (Favorite_RadiologyOrder.params["FromAdmin"] == "0") {
            if (Favorite_RadiologyOrder.params != null && Favorite_RadiologyOrder.params.ParentCtrl != null) {
                UnloadActionPan(Favorite_RadiologyOrder.params.ParentCtrl, 'Favorite_RadiologyOrder');
            }
            else
                UnloadActionPan(null, 'Favorite_RadiologyOrder');
        }
        else {
            RemoveAdminTab();
        }
    },
    AddFavoriteRadiologyOrderDetail: function () {

        var params = [];
        params["ParentCtrl"] = "adminTabFavoritesRadiologyOrder";
        params["mode"] = "Add";
        params["FromAdmin"] = 0;
        LoadActionPan("Favorite_RadiologyOrderDetail", params);
    },

    /****************************************************
            DB Calls For Favorite Lists
    *****************************************************/
    searchFavoriteList_CPT_DBCall: function (FavoriteListCPTId, FavoriteListId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = -1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = -1;
        }
        var objData = {};

        objData["FavoriteListId"] = FavoriteListId == null ? -1 : FavoriteListId;
        objData["FavoriteListCPTId"] = FavoriteListCPTId == null ? -1 : FavoriteListCPTId;
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;

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
        objData["ListType"] = ListType == null ? 'RadiologyOrder' : ListType;
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        //Start 24-03-2016 Humaira Yousaf for active/active
        if (Favorite_RadiologyOrder.Switch == 1) {
            objData["IsActive"] = true
        }
        else {
            objData["IsActive"] = false;
        }
        //End 24-03-2016 Humaira Yousaf for active/active
        objData["commandType"] = "load_favoritelist";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },
    //Method Name: activeFavoritesSearch
    // Author Name: Humaira Yousaf
    //Craeted Date: 24-03-2016
    //Description:  Searches active/inactive list
    activeFavoritesSearch: function (obj) {
        var isactive = $(obj).attr('isactive');

        if (isactive == '1') {
            $(obj).attr('isactive', '0');
            Favorite_RadiologyOrder.Switch = 0;
        }
        else if (isactive == '0') {
            $(obj).attr('isactive', '1');
            Favorite_RadiologyOrder.Switch = 1;
        }

        Favorite_RadiologyOrder.favoriteListSearch();
    },

    //Method Name: deleteFavoriteList_DBCall
    // Author Name: Humaira Yousaf
    //Craeted Date: 24-03-2016
    //Description:  This function will call DB to delete favorite list radiology order
    deleteFavoriteList_DBCall: function (favoriteListId) {

        var objData = new Object();
        objData["FavoriteListId"] = favoriteListId;
        objData["commandType"] = "DELETE_FAVORITELIST_PROCEDURE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },
}