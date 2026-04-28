Favorite_Complaints = {
    bIsFirstLoad: true,
    params: [],
    Switch: 1,
    Load: function (params) {
        Favorite_Complaints.params = params;
        if (Favorite_Complaints.params.PanelID != 'pnlFavoriteComplaints') {
            Favorite_Complaints.params.PanelID = Favorite_Complaints.params.PanelID + ' #pnlFavoriteComplaints';
        } else {
            Favorite_Complaints.params.PanelID = 'pnlFavoriteComplaints';
        }
        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Favorite_Complaints.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        $("#" + Favorite_Complaints.params.PanelID + ' #dgvFavoritiesListICD tbody').empty();
        $("#" + Favorite_Complaints.params.PanelID + ' #ulFavoritiesList').empty();
        Favorite_Complaints.favoriteListSearch();
    },

    domreadyFunctions: function () {
        $(function () {
            $("#" + Favorite_Complaints.params.PanelID + ' [data-plugin-toggle]').each(function () {
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
                    $("#" + Favorite_Complaints.params.PanelID + ' [data-plugin-ios-switch]').each(function () {
                        var $this = $(this);

                        $this.themePluginIOS7Switch();
                    });
                });
            }).apply(this, [jQuery]);
        });

    },

    favoriteListSearch: function (FavoriteListId, ListType, PageNo, rpp) {
        Favorite_Complaints.searchFavoriteList_DBCall(ListType, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Favorite_Complaints.FavoriteListGridLoad(response);
                Favorite_Complaints.domreadyFunctions();
                if ($("#" + Favorite_Complaints.params.PanelID + ' #ulFavoritiesList li').length > 0) {
                    $("#" + Favorite_Complaints.params.PanelID + ' #ulFavoritiesList li:first').trigger('click');
                } else {
                    $("#" + Favorite_Complaints.params.PanelID + ' #dgvFavoritiesListICD tbody').empty();
                }
                
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    FavoriteListGridLoad: function (response) {
        $("#" + Favorite_Complaints.params.PanelID + ' #ulFavoritiesList').empty();
        if (response.FavoriteListCount > 0) {
            var FavoriteListJSON = JSON.parse(response.FavoriteListJSON);
            $.each(FavoriteListJSON, function (i, item) {
                var FavoriteListInfo = '<li onclick="Favorite_Complaints.loadFavoriteListICD(' + item.FavoriteListId + ',this);">' +
                       '<span class="pull-left pr-xlg">' + item.Name + ' </span>' +
                       '<span class="removeIconListHover">' +
                       '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Favorite_Complaints.editFavoriteList(' + item.FavoriteListId + ');"><i class="fa fa-edit blue"></i></a>' +
                       '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="Favorite_Complaints.deleteFavoriteList(' + item.FavoriteListId + ');"><i class="fa fa-times red"></i></a>' +
                       '</span>' +
                       '<div class="clearfix"></div>' +
                       '</li>';

                $("#" + Favorite_Complaints.params.PanelID + ' #ulFavoritiesList').append(FavoriteListInfo);
            });
        }
    },
    loadFavoriteListICD: function (FavoriteListId, obj) {
        $("#" + Favorite_Complaints.params.PanelID + ' #ulFavoritiesList li').removeClass("active");
        $(obj).addClass("active");
        Favorite_Complaints.favoriteList_ICDSearch(FavoriteListId);
    },
    
    deleteFavoriteList: function (FavoriteListId) {

        //////////////////

       
        //Start//3/23/2016 by Babur //Delete logic implemented
        AppPrivileges.GetFormPrivileges("Favorites_Complaints", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {

                    Favorite_Complaints.deleteFavoriteList_DBCall(FavoriteListId).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {

                                //if ($row.hasClass('adding')) {
                                //}
                                //var _self = obj;
                                //_self.datatable.row($row.get(0)).remove().draw();

                                utility.DisplayMessages(response.Message, 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }

                            Favorite_Complaints.favoriteListSearch();

                        });
                    



                }, function () { },
                   '1'
        );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        //End//3/23/2016 by Babur //Delete logic implemented

        //////////////////



    },
    editFavoriteList: function (FavoriteListId) {
        var params = [];
        params["ParentCtrl"] = "adminTabFavoritesComplaint";
        params["mode"] = "Edit";
        params["FromAdmin"] = 0;
        params["FavoriteListId"] = FavoriteListId;
        LoadActionPan("Favorite_Complaints_Detail", params);
    },
    

    favoriteList_ICDSearch: function (FavoriteListId, FavoriteListICDId, PageNo, rpp, searchData) {
        Favorite_Complaints.searchFavoriteList_ICD_DBCall(FavoriteListICDId, FavoriteListId, PageNo, rpp, searchData).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Favorite_Complaints.FavoriteListICDGridLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    FavoriteListICDGridLoad: function (response) {
        $("#" + Favorite_Complaints.params.PanelID + ' #dgvFavoritiesListICD tbody').empty();
        if (response.FavoriteListICDCount > 0) {
            var FavoriteListJSON = JSON.parse(response.FavoriteListICDJSON);
            $.each(FavoriteListJSON, function (i, item) {
                var $row = $('<tr/>');
                //$row.append('<td>' + item.ICD9Code + '</td>');
                $row.append('<td>' + item.ICD10Code + '</td>');
                //$row.append('<td>' + item.SNOMEDID + '</td>');
                $row.append('<td>' + item.ICD10CodeDescription + '</td>');

                $("#" + Favorite_Complaints.params.PanelID + ' #dgvFavoritiesListICD tbody').last().append($row);
            });
        }
    },
    UnLoadTab: function () {
        if (Favorite_Complaints.params["FromAdmin"] == "0") {
            if (Favorite_Complaints.params != null && Favorite_Complaints.params.ParentCtrl != null) {
                UnloadActionPan(Favorite_Complaints.params.ParentCtrl, 'Favorite_Complaints');
            }
            else
                UnloadActionPan(null, 'Favorite_Complaints');
        }
        else {
            RemoveAdminTab();
        }
    },
    AddFavoriteComplaintsDetail: function () {

        var params = [];
        params["ParentCtrl"] = "adminTabFavoritesComplaint";
        params["mode"] = "Add";
        params["FromAdmin"] = 0;
        LoadActionPan("Favorite_Complaints_Detail", params);
    },

    /****************************************************
            DB Calls For Favorite Lists
    *****************************************************/
    searchFavoriteList_ICD_DBCall: function (FavoriteListICDId, FavoriteListId, PageNumber, RowsPerPage, SearchData) {
        if (PageNumber == null) {
            PageNumber = -1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = -1;
        }
        var objData = {};
        objData["IsActive"] = $('#' + Favorite_Complaints.params.PanelID + ' #divSwitch #switchActive').is(':checked');
        objData["FavoriteListId"] = FavoriteListId == null ? 0 : FavoriteListId;
        objData["FavoriteListICDId"] = FavoriteListICDId == null ? 0 : FavoriteListICDId;
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["PageNumber"] = PageNumber;
        objData["SearchData"] = SearchData;
        objData["RowsPerPage"] = RowsPerPage;

        objData["commandType"] = "load_favoritelist_icd";
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
        objData["IsActive"] = $('#' + Favorite_Complaints.params.PanelID + ' #divSwitch #switchActive').is(':checked');
        objData["FavoriteListId"] = FavoriteListId == null ? 0 : FavoriteListId;
        objData["ListType"] = ListType == null ? 'Complaints' : ListType;
        if (globalAppdata['AppUserName'] == DefaultUser) {
            objData["EntityId"] = -1;
        }
        else {
            objData["EntityId"] = globalAppdata["SeletedEntityId"];
        }
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;

        objData["commandType"] = "load_favoritelist";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },

    //////////////
    deleteFavoriteList_DBCall: function (FavoriteListId) {

        var objData = new Object();
        objData["FavoriteListId"] = FavoriteListId;
        objData["commandType"] = "DELETE_FAVORITE_COMPLAINT";
        //   objData["PatientId"] = Clinical_ProblemLists.params.patientID;
        //   objData["Description"] = Description;
        //   objData["StartDate"] = StartDate;      

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },
    /////////////


}