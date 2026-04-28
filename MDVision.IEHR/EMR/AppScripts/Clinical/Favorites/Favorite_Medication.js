Favorite_Medication = {
    bIsFirstLoad: true,
    params: [],
    Switch: 1,
    Load: function (params) {
        Favorite_Medication.params = params;
        if (Favorite_Medication.params.PanelID != 'pnlFavoriteMedication')
            Favorite_Medication.params.PanelID = Favorite_Medication.params.PanelID + ' #pnlFavoriteMedication';
         else 
            Favorite_Medication.params.PanelID = 'pnlFavoriteMedication';
        $("#" + Favorite_Medication.params.PanelID + ' #dgvFavoritiesListMedication tbody').empty();
        $("#" + Favorite_Medication.params.PanelID + ' #ulFavoritiesListMedication').empty();
        Favorite_Medication.favoriteListSearch();
        Favorite_Medication.domreadyFunctions();
    },
    domreadyFunctions: function () {
        $(function () {
            $("#" + Favorite_Medication.params.PanelID + ' [data-plugin-toggle]').each(function () {
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
                    $("#" + Favorite_Medication.params.PanelID + ' [data-plugin-ios-switch]').each(function () {
                        var $this = $(this);
                        $this.themePluginIOS7Switch();
                    });
                });
            }).apply(this, [jQuery]);
        });
    },
    favoriteListSearch: function (FavoriteListId, ListType, PageNo, rpp) {
        Favorite_Medication.searchFavoriteList_DBCall(ListType, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Favorite_Medication.FavoriteListGridLoad(response); //------
                Favorite_Medication.domreadyFunctions();
                if ($("#" + Favorite_Medication.params.PanelID + ' #ulFavoritiesListMedication li').length > 0)
                    $("#" + Favorite_Medication.params.PanelID + ' #ulFavoritiesListMedication li:first').trigger('click');
                else
                    $("#" + Favorite_Medication.params.PanelID + ' #dgvFavoritiesListMedication tbody').empty();
            }
            else
                utility.DisplayMessages(response.Message, 3);
        });
    },

    FavoriteListGridLoad: function (response) {
        $("#" + Favorite_Medication.params.PanelID + ' #ulFavoritiesListMedication').empty();
        if (response.FavoriteListCount > 0) {
            var FavoriteListJSON = JSON.parse(response.FavoriteListJSON);
            $.each(FavoriteListJSON, function (i, item) {
                var FavoriteListInfo = '<li onclick="Favorite_Medication.loadFavoriteListMedications(' + item.FavoriteListId + ',this);">' +
                       '<span class="pull-left pr-xlg">' + item.Name + ' </span>' +
                       '<span class="removeIconListHover">' +
                       '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Favorite_Medication.editFavoriteList(' + item.FavoriteListId + ');"><i class="fa fa-edit blue"></i></a>' +
                       '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Favorite_Medication.deleteFavoriteList(' + item.FavoriteListId + ');"><i class="fa fa-times red"></i></a>' +
                       '</span>' +
                       '<div class="clearfix"></div>' +
                       '</li>';
                $("#" + Favorite_Medication.params.PanelID + ' #ulFavoritiesListMedication').append(FavoriteListInfo);
            });
        }
    },
    
    deleteFavoriteList: function (FavoriteListId) {
        AppPrivileges.GetFormPrivileges("Favorites_Medication", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    Favorite_Medication.deleteFavoriteList_DBCall(FavoriteListId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false)
                            utility.DisplayMessages(response.Message, 1);
                        else
                            utility.DisplayMessages(response.Message, 3);
                        Favorite_Medication.favoriteListSearch();
                    });
                }, function () { },
                   '1'
        );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    favoriteList_MedicationSearch: function (FavoriteListId, FavoriteListMedicationId, PageNo, rpp) {
        Favorite_Medication.searchFavoriteList_Medication_DBCall(FavoriteListMedicationId, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false)
                Favorite_Medication.FavoriteListMedGridLoad(response);
            else 
                utility.DisplayMessages(response.Message, 3);
        });
    },
    FavoriteListMedGridLoad: function (response) {
        $("#" + Favorite_Medication.params.PanelID + ' #dgvFavoritiesListMedication tbody').empty();
        if (response.FavoriteListMedCount > 0) {
            var FavoriteListJSON = JSON.parse(response.FavoriteListMedJSON);
            $.each(FavoriteListJSON, function (i, item) {
                var $row = $('<tr/>');
                $row.append('<td>' + item.BrandName + " " + (item.Strength ? item.Strength : "") + '</td>');
                $("#" + Favorite_Medication.params.PanelID + ' #dgvFavoritiesListMedication tbody').last().append($row);
            });
        }
    },
    UnLoadTab: function () {
        if (Favorite_Medication.params["FromAdmin"] == "0") {
            if (Favorite_Medication.params && FavoriFavorite_Medicationte_Problems.params.ParentCtrl)
                UnloadActionPan(Favorite_Medication.params.ParentCtrl, 'Favorite_Medication');
            else
                UnloadActionPan(null, 'Favorite_Medication');
        }
        else
            RemoveAdminTab();
    },
    AddFavoriteMedicationDetail: function () {
        var params = [];
        params["ParentCtrl"] = "adminTabFavoritesMedication";
        params["mode"] = "Add";
        params["FromAdmin"] = 0;
        LoadActionPan("Favorite_MedicationDetail", params);
    },
    editFavoriteList: function (FavoriteListId) {
        var params = [];
        params["ParentCtrl"] = "adminTabFavoritesMedication";
        params["mode"] = "Edit";
        params["FromAdmin"] = 0;
        params["FavoriteListId"] = FavoriteListId;
        LoadActionPan("Favorite_MedicationDetail", params);
    },
    loadFavoriteListMedications: function (FavoriteListId, obj) {
        $("#" + Favorite_Medication.params.PanelID + ' #ulFavoritiesListMedication li').removeClass("active");
        $(obj).addClass("active");
        Favorite_Medication.favoriteList_MedicationSearch(FavoriteListId);
    },
    

    /****************************************************
            DB Calls For Favorite Lists
    *****************************************************/
    searchFavoriteList_Medication_DBCall: function (FavoriteListMedicationId, FavoriteListId, PageNumber, RowsPerPage, SearchData) {
        if (PageNumber == null)
            PageNumber = -1;
        if (RowsPerPage == null)
            RowsPerPage = -1;
        var objData = {};
        objData["IsActive"] = $('#' + Favorite_Medication.params.PanelID + ' #divSwitch #switchActive').is(':checked');
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
    searchFavoriteList_DBCall: function (ListType, FavoriteListId, PageNumber, RowsPerPage) {
        if (PageNumber == null)
            PageNumber = -1;
        if (RowsPerPage == null)
            RowsPerPage = -1;
        var objData = {};
        objData["IsActive"] = $('#' + Favorite_Medication.params.PanelID + ' #divSwitch #switchActive').is(':checked');
        objData["FavoriteListId"] = FavoriteListId == null ? 0 : FavoriteListId;
        objData["ListType"] = ListType ? ListType : 'Medication';
        if (globalAppdata['AppUserName'] == DefaultUser)
            objData["EntityId"] = -1;
        else 
            objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "load_favoritelist";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },
    deleteFavoriteList_DBCall: function (FavoriteListId) {
        var objData = new Object();
        objData["FavoriteListId"] = FavoriteListId;
        objData["commandType"] = "DELETE_FAVORITE_COMPLAINT";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },
}