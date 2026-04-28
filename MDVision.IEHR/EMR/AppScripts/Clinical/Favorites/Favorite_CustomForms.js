Favorite_CustomForms = {
    bIsFirstLoad: true,
    params: [],
    Switch: 1,
    Load: function (params) {
        Favorite_CustomForms.params = params;
        if (Favorite_CustomForms.params.PanelID != 'pnlFavoriteCustomForms') {
            Favorite_CustomForms.params.PanelID = Favorite_CustomForms.params.PanelID + ' #pnlFavoriteCustomForms';
        } else {
            Favorite_CustomForms.params.PanelID = 'pnlFavoriteCustomForms';
        }
        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Favorite_CustomForms.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        Favorite_CustomForms.emptyGridAndList();
        Favorite_CustomForms.favoriteListSearch();
    },

    domreadyFunctions: function () {
        $(function () {
            $("#" + Favorite_CustomForms.params.PanelID + ' [data-plugin-toggle]').each(function () {
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
                    $("#" + Favorite_CustomForms.params.PanelID + ' [data-plugin-ios-switch]').each(function () {
                        var $this = $(this);

                        $this.themePluginIOS7Switch();
                    });
                });
            }).apply(this, [jQuery]);
        });

    },

    favoriteListSearch: function (FavoriteListId, ListType, PageNo, rpp) {
        Favorite_CustomForms.searchFavoriteList_DBCall(ListType, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Favorite_CustomForms.FavoriteListGridLoad(response);
                Favorite_CustomForms.domreadyFunctions();
                if ($("#" + Favorite_CustomForms.params.PanelID + ' #ulFavoritiesList li').length > 0) {
                    var favListId = $("#" + Favorite_CustomForms.params.PanelID + " #hfFavoriteListId").val();
                    $("#" + Favorite_CustomForms.params.PanelID + ' #ulFavoritiesList li#' + favListId).trigger('click');
                }

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },
    loadfavoriteListContent: function (favListId, obj) {
        if (favListId != null) {
            var selectedOption = favListId;
            if (favListId != "-1") {
                Favorite_CustomForms.searchFavoriteList_CF_DBCall(selectedOption).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        $("#" + Favorite_CustomForms.params.PanelID + ' #ulFavoritiesList li').removeClass("active");
                        $(obj).addClass("active");

                        Favorite_CustomForms.FavoriteListCPTGridLoad(response);


                    }
                });
                // Select_CustomForm.favoriteList_CPTSearch(selectedOption.attr("id"));
            }
            else {
                $('#' + Favorite_CustomForms.params.PanelID + ' #ulFavoritiesList').empty();
                // $('#' + Select_CustomForm.params.PanelID + ' #favSelectAllLink').addClass('disableAll');
            }
        }
    },
    FavoriteListGridLoad: function (response) {
        if (response.FavoriteListCount > 0) {
            $("#" + Favorite_CustomForms.params.PanelID + ' #ulFavoritiesList').empty();
            var FavoriteListJSON = JSON.parse(response.FavoriteListJSON);
            $.each(FavoriteListJSON, function (i, item) {
                var FavoriteListInfo = '<li id=' + item.FavoriteListId + ' onclick="Favorite_CustomForms.loadfavoriteListContent(' + item.FavoriteListId + ',this);">' +
                       '<span class="pull-left pr-xlg">' + item.Name + ' </span>' +
                       '<span class="removeIconListHover">' +
                       '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Favorite_CustomForms.editFavoriteList(' + item.FavoriteListId + ');"><i class="fa fa-edit blue"></i></a>' +
                       '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Favorite_CustomForms.deleteFavoriteList(' + item.FavoriteListId + ');"><i class="fa fa-times red"></i></a>' +
                       '</span>' +
                       '<div class="clearfix"></div>' +
                       '</li>';

                $("#" + Favorite_CustomForms.params.PanelID + ' #ulFavoritiesList').append(FavoriteListInfo);
            });
        }
            //Start 24-03-2016 Humaira Yousaf for active/inactive
        else {
            $("#" + Favorite_CustomForms.params.PanelID + ' #ulFavoritiesList').empty();
            $("#" + Favorite_CustomForms.params.PanelID + ' #ulFavoritiesList').append('<li><span class="pull-left pr-xlg">No Favorites Found.</span></li>');
            //Begin 15-07-2016 Edit By Humaira Yousaf Bug#1380
            $("#" + Favorite_CustomForms.params.PanelID + " #dgvFavoritiesListCustomForms tr").not(':first').remove();
            //End 15-07-2016 Edit By Humaira Yousaf Bug#1380
        }
        //End 24-03-2016 Humaira Yousaf for active/inactive
    },

    loadFavoriteListCPT: function (FavoriteListId, obj) {
        $("#" + Favorite_CustomForms.params.PanelID + ' #ulFavoritiesList li').removeClass("active");
        $(obj).addClass("active");
        Favorite_CustomForms.favoriteList_CPTSearch(FavoriteListId);
    },

    //Method Name: deleteFavoriteList
    // Author Name: Abid Ali
    //Craeted Date: 24-03-2016
    //Description:  This function will delete favorite list consultation order
    editFavoriteList: function (favoriteListId) {
        //Load Consultation Order Detail for edit
        Favorite_CustomForms.AddFavoriteCustomFormsDetail(favoriteListId);
    },

    //Method Name: deleteFavoriteList
    // Author Name: Abid Ali
    //Craeted Date: 24-03-2016
    //Description:  This function will delete favorite list consultation order
    deleteFavoriteList: function (FavoriteListId) {
        AppPrivileges.GetFormPrivileges("Favorites_CustomForms", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {

                    Favorite_CustomForms.deleteFavoriteList_DBCall(FavoriteListId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }

                        Favorite_CustomForms.favoriteListSearch();

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
        Favorite_CustomForms.searchFavoriteList_CPT_DBCall(FavoriteListCPTId, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Favorite_CustomForms.FavoriteListCPTGridLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    FavoriteListCPTGridLoad: function (response) {
        $("#" + Favorite_CustomForms.params.PanelID + ' #dgvFavoritiesListCustomForms tbody').empty();
        if (response.customFormCount > 0) {
            var FavoriteListJSON = response.listCustomForm;
            $.each(FavoriteListJSON, function (i, item) {
                if (item.ProviderNames != "") {
                    var $row = $('<tr/>');
                    var CustomFormPreview = "Favorite_CustomForms.CustomFormPreview('" + item.CustomFormId + "','" + item.FormName + "',event,false,true)";
                    $row.append('<td>' + item.FormName + '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a style="float:right;" title="View Custom Form" class="btn  btn-xs" href="#" onclick="' + CustomFormPreview + '"> <i class="fa fa-credit-card blue"></i></a></td>');

                    //$row.append('<td>' + item.SNOMEDID + '</td>');
                    //$row.append('<td>' + item.CPT10CodeDescription + '</td>');

                    $("#" + Favorite_CustomForms.params.PanelID + ' #dgvFavoritiesListCustomForms tbody').last().append($row);
                }
            });
        }
    },

    searchFavoriteList_CF_DBCall: function (FavoriteListId, PageNumber, PageNumber) {

        var objData = {};

        objData["FavoriteListId"] = FavoriteListId == null ? 0 : FavoriteListId;
        objData["IsActive"] = "1";
        objData["PageNumber"] = 1;
        objData["RowsPerPage"] = 15;

        objData["commandType"] = "load_favoritelist_customforms";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CustomForm", "CustomForm");
    },

    CustomFormPreview: function (formId, customFormName, event, isAddToNote, fromAdmin) {
        if (event != null) {
            event.stopPropagation();
        }

        var params = [];
        var PanelID = "";
        if (Clinical_CustomForms.params && Clinical_CustomForms.params.ParentCtrl == "clinicalTabProgressNote") {
            params["ParentCtrl"] = 'clinicalTabProgressNote';
            params["IsFromAdminOrNot"] = "0";
            params["IsAddToNote"] = isAddToNote;
        }
        //else if (Select_CustomForm.params && Select_CustomForm.params.PanelID) {
        //    params["ParentCtrl"] = 'Select_CustomForm';
        //    PanelID = 'pnlPatientCustomForm #pnlSelectCustomForm';
        //    params["IsFromAdminOrNot"] = "0";
        //    params["IsAddToNote"] = isAddToNote;
        //}
        if (fromAdmin) {
            params["ParentCtrl"] = 'Favorite_CustomForms';
            PanelID = 'pnlFavoriteCustomForms';
            params["IsFromAdminOrNot"] = "1";
        }

        params["FromAdmin"] = Clinical_CustomForms.params["FromAdmin"];
        params["CustomFormId"] = formId;
        params["CustomFormName"] = customFormName;
        params["PanelID"] = PanelID;

        if (!fromAdmin) {
            Clinical_CustomForms.UnLoadTab();
        }
        setTimeout(function () {
            LoadActionPan("Clinical_CustomFormsPreview", params);
        }, 510);

    },
    UnLoadTab: function () {
        if (Favorite_CustomForms.params["FromAdmin"] == "0") {
            if (Favorite_CustomForms.params != null && Favorite_CustomForms.params.ParentCtrl != null) {
                UnloadActionPan(Favorite_CustomForms.params.ParentCtrl, 'Favorite_CustomForms');
            }
            else
                UnloadActionPan(null, 'Favorite_CustomForms');
        }
        else {
            RemoveAdminTab();
        }
    },

    AddFavoriteCustomFormsDetail: function (favoriteListId) {

        var params = [];
        params["ParentCtrl"] = "adminTabFavoritesCustomForms";
        if (favoriteListId > 0)
            params["mode"] = "Edit";
        else
            params["mode"] = "Add";
        params["FromAdmin"] = 0;
        params["FavoriteListId"] = favoriteListId;
        LoadActionPan("Favorite_CustomFormsDetail", params);
    },

    //Method Name: activeFavoritesSearch
    // Author Name: Abid Ali
    //Craeted Date: 25-03-2016
    //Description:  Searches active and iactive records
    activeFavoritesSearch: function (obj) {
        var isactive = $(obj).attr('isactive');

        if (isactive == '1') {
            $(obj).attr('isactive', '0');
            Favorite_CustomForms.Switch = 0;
        }
        else if (isactive == '0') {
            $(obj).attr('isactive', '1');
            Favorite_CustomForms.Switch = 1;
        }

        Favorite_CustomForms.emptyGridAndList();
        Favorite_CustomForms.favoriteListSearch();
    },

    emptyGridAndList: function () {
        $("#" + Favorite_CustomForms.params.PanelID + ' #dgvFavoritiesListCustomForms tbody').empty();
        $("#" + Favorite_CustomForms.params.PanelID + ' #ulFavoritiesList').empty();
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

        objData["commandType"] = "load_favoritelist_customform";
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
        objData["ListType"] = ListType == null ? 'CustomForms' : ListType;
        objData["EntityId"] = globalAppdata["SeletedEntityId"];

        if (Favorite_CustomForms.Switch == 1) {
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
        objData["commandType"] = "delete_favoritelist_customforms";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },


}