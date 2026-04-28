//Author: Humaira Yousaf
//Date: 31-03-2016
//This file will handle all actions performed for favorite Medical history
Favorite_MedicalHistory = {
    bIsFirstLoad: true,
    params: [],
    Switch: 1,
    Load: function (params) {
        Favorite_MedicalHistory.params = params; $("#" + Favorite_MedicalHistory.params.PanelID + ' #dgvFavoritiesListCPT tbody').empty();
        if (Favorite_MedicalHistory.params.PanelID != 'pnlFavoriteMedicalHistory') {
            Favorite_MedicalHistory.params.PanelID = Favorite_MedicalHistory.params.PanelID + ' #pnlFavoriteMedicalHistory';
        } else {
            Favorite_MedicalHistory.params.PanelID = 'pnlFavoriteMedicalHistory';
        }
        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Favorite_MedicalHistory.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        $("#" + Favorite_MedicalHistory.params.PanelID + ' #dgvFavoritiesListCPT tbody').empty();
        $("#" + Favorite_MedicalHistory.params.PanelID + ' #ulFavoritiesList').empty();
	//Start 30-03-2016 Humaira Yousaf
        $("#" + Favorite_FamilyHistory.params.PanelID + ' #adminMenuFavoritesFamilyHistory').removeClass('active');
        $("#" + Favorite_FamilyHistory.params.PanelID + ' #adminMenuFavoritesMedicalHistory').addClass('active');
        $("#" + Favorite_FamilyHistory.params.PanelID + ' #adminMenuFavoritesSurgicalHistory').removeClass('active');
	//End 30-03-2016 Humaira Yousaf
        Favorite_MedicalHistory.favoriteListSearch();
    },

    domreadyFunctions: function () {
        $(function () {
            $("#" + Favorite_MedicalHistory.params.PanelID + ' [data-plugin-toggle]').each(function () {
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
                    $("#" + Favorite_MedicalHistory.params.PanelID + ' [data-plugin-ios-switch]').each(function () {
                        var $this = $(this);

                        $this.themePluginIOS7Switch();
                    });
                });
            }).apply(this, [jQuery]);
        });

    },
    //Function Name: favoriteListSearch
    //Author Name: Humaira Yousaf 
    //Craeted Date: 31-03-2016
    //Description:  Searches Favorite list
    favoriteListSearch: function (FavoriteListId, ListType, PageNo, rpp) {
        Favorite_MedicalHistory.searchFavoriteList_DBCall(ListType, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Favorite_MedicalHistory.favoriteListGridLoad(response);
                Favorite_MedicalHistory.domreadyFunctions();
                if (response.FavoriteListCount > 0) {
                    $("#" + Favorite_MedicalHistory.params.PanelID + ' #ulFavoritiesList li:first').trigger('click');
                }
                else {
                    $("#" + Favorite_MedicalHistory.params.PanelID + ' #dgvFavoritiesListCPT tbody').empty();
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },
    //Function Name: favoriteListGridLoad
    //Author Name: Humaira Yousaf 
    //Craeted Date: 31-03-2016
    //Description:  Loads Favorite list
    favoriteListGridLoad: function (response) {

        if (response.FavoriteListCount > 0) {
            $("#" + Favorite_MedicalHistory.params.PanelID + ' #ulFavoritiesList').empty();
            var FavoriteListJSON = JSON.parse(response.FavoriteListJSON);
            $.each(FavoriteListJSON, function (i, item) {
                var FavoriteListInfo = '<li onclick="Favorite_MedicalHistory.loadFavoriteListCPT(' + item.FavoriteListId + ',this);">' +
                       '<span class="pull-left pr-xlg">' + item.Name + ' </span>' +
                       '<span class="removeIconListHover">' +
                       '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Favorite_MedicalHistory.showHistory(' + item.FavoriteListId + ');"><i class="fa fa-history blue"></i></a>' +
                       '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Favorite_MedicalHistory.editFavoriteList(' + item.FavoriteListId + ');"><i class="fa fa-edit blue"></i></a>' +
                       '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Favorite_MedicalHistory.deleteFavoriteList(' + item.FavoriteListId + ');"><i class="fa fa-times red"></i></a>' +
                       '</span>' +
                       '<div class="clearfix"></div>' +
                       '</li>';

                $("#" + Favorite_MedicalHistory.params.PanelID + ' #ulFavoritiesList').append(FavoriteListInfo);
            });
        }
        else {
            $("#" + Favorite_MedicalHistory.params.PanelID + ' #ulFavoritiesList').empty();
            $("#" + Favorite_MedicalHistory.params.PanelID + ' #ulFavoritiesList').append('<li><span class="pull-left pr-xlg">No Favorites Found.</span></li>');
        }
    },
    //Function Name: loadFavoriteListCPT
    //Author Name: Humaira Yousaf 
    //Craeted Date: 31-03-2016
    //Description:  Loads Favorite CPT list
    loadFavoriteListCPT: function (FavoriteListId, obj) {
        $("#" + Favorite_MedicalHistory.params.PanelID + ' #ulFavoritiesList li').removeClass("active");
        $(obj).addClass("active");
        Favorite_MedicalHistory.favoriteList_CPTSearch(FavoriteListId);
    },

    //Function Name: loadFavoriteListCPT
    //Author Name: Humaira Yousaf 
    //Craeted Date: 31-03-2016
    //Description:  add/edit favorite list
    editFavoriteList: function (favoriteListId) {

        Favorite_MedicalHistory.addFavoriteMedicalHistoryDetail(favoriteListId);
    },
    //Function Name: favoriteList_CPTSearch
    //Author Name: Humaira Yousaf 
    //Craeted Date: 31-03-2016
    //Description:  searches favorite CPT list
    favoriteList_CPTSearch: function (FavoriteListId, FavoriteListCPTId, PageNo, rpp) {
        Favorite_MedicalHistory.searchFavoriteList_CPT_DBCall(FavoriteListCPTId, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Favorite_MedicalHistory.FavoriteListCPTGridLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    //Function Name: FavoriteListCPTGridLoad
    //Author Name: Humaira Yousaf 
    //Craeted Date: 31-03-2016
    //Description:  loads favorite CPT list
    FavoriteListCPTGridLoad: function (response) {
        if (response.FavoriteListICDCount > 0) {
            $("#" + Favorite_MedicalHistory.params.PanelID + ' #dgvFavoritiesListCPT tbody').empty();
            var FavoriteListJSON = JSON.parse(response.FavoriteListICDJSON);
            $.each(FavoriteListJSON, function (i, item) {
                var $row = $('<tr/>');
                //$row.append('<td>' + item.ICD9Code + '</td>');
                $row.append('<td>' + item.ICD10Code + '</td>');
                //$row.append('<td>' + item.SNOMEDID + '</td>');
                $row.append('<td>' + item.ICD10CodeDescription + '</td>');
                $("#" + Favorite_MedicalHistory.params.PanelID + ' #dgvFavoritiesListCPT tbody').last().append($row);
            });
        }
    },
    //Function Name: UnLoadTab
    //Author Name: Humaira Yousaf 
    //Craeted Date: 31-03-2016
    //Description: Unloads Tab 
    UnLoadTab: function () {
        if (Favorite_MedicalHistory.params["FromAdmin"] == "0") {
            if (Favorite_MedicalHistory.params != null && Favorite_MedicalHistory.params.ParentCtrl != null) {
                UnloadActionPan(Favorite_MedicalHistory.params.ParentCtrl, 'Favorite_MedicalHistory');
            }
            else
                UnloadActionPan(null, 'Favorite_MedicalHistory');
        }
        else {
            RemoveAdminTab();
        }
    },
    //Function Name: addFavoriteMedicalHistoryDetail
    //Author Name: Humaira Yousaf 
    //Craeted Date: 31-03-2016
    //Description: add/edit favorite list 
    addFavoriteMedicalHistoryDetail: function (favoriteListId) {

        var params = [];
        params["ParentCtrl"] = "adminTabFavoritesMedicalHistory";
        if (favoriteListId != undefined && favoriteListId > 0) {
            params["mode"] = "Edit";
        }
        else {
            params["mode"] = "Add";
        }
        params["FromAdmin"] = 0;
        params["FavoriteListId"] = favoriteListId;
        LoadActionPan("Favorite_MedicalHistoryDetail", params);
    },

    //Function Name: searchFavoriteList_CPT_DBCall
    //Author Name: Humaira Yousaf 
    //Craeted Date: 31-03-2016
    //Description: searches favorite CPT list 
    searchFavoriteList_CPT_DBCall: function (FavoriteListCPTId, FavoriteListId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = -1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = -1;
        }
        var objData = {};

        objData["FavoriteListId"] = FavoriteListId == null ? 0 : FavoriteListId;
        objData["FavoriteListICDId"] = FavoriteListCPTId == null ? 0 : FavoriteListCPTId;
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["IsActive"] = true;
        objData["commandType"] = "load_favoritelist_ICD";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },
    //Function Name: searchFavoriteList_DBCall
    //Author Name: Humaira Yousaf 
    //Craeted Date: 31-03-2016
    //Description: searches favorite list 
    searchFavoriteList_DBCall: function (ListType, FavoriteListId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = -1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = -1;
        }
        var objData = {};
        objData["FavoriteListId"] = FavoriteListId == null ? 0 : FavoriteListId;
        objData["ListType"] = ListType == null ? 'MedicalHistory' : ListType;
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        if (Favorite_MedicalHistory.Switch == 1) {
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

    //Method Name: deleteFavoriteList
    //Author Name: Humaira Yousaf
    //Craeted Date: 31-03-2016
    //Description:  Delete favorite list
    deleteFavoriteList: function (FavoriteListId) {
        AppPrivileges.GetFormPrivileges("Favorites_MedicalHistory", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {

                    Favorite_MedicalHistory.deleteFavoriteList_DBCall(FavoriteListId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }

                        Favorite_MedicalHistory.favoriteListSearch();

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
    //Author Name: Humaira Yousaf
    //Craeted Date: 31-03-2016
    //Description:  Delete favorite list
    deleteFavoriteList_DBCall: function (favoriteListId) {

        var objData = new Object();
        objData["FavoriteListId"] = favoriteListId;
        objData["commandType"] = "DELETE_FAVORITELIST_PROCEDURE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },


    //Method Name: activeFavoritesSearch
    //Author Name: Humaira Yousaf
    //Craeted Date: 31-03-2016
    //Description:  Searches active and inactive records
    activeFavoritesSearch: function (obj) {
        var isactive = $(obj).attr('isactive');

        if (isactive == '1') {
            $(obj).attr('isactive', '0');
            Favorite_MedicalHistory.Switch = 0;
        }
        else if (isactive == '0') {
            $(obj).attr('isactive', '1');
            Favorite_MedicalHistory.Switch = 1;
        }

        Favorite_MedicalHistory.favoriteListSearch();
    },

    showHistory: function (historyId) {
        EMRUtility.showCurrentItemHistory(Favorite_MedicalHistory.params.PanelID, null, null, 'FavoriteList,FavoriteListICD', null, Favorite_MedicalHistory.params.TabID, historyId);
    }
}