Favorite_ConsultationOrderDetail = {
    bIsFirstLoad: true,
    params: [],
    Switch: 1,
    CPTData: [],
    providerCheckedIds: [],
    ProviderIds: '',
    Load: function (params) {
        Favorite_ConsultationOrderDetail.params = params;
        if (Favorite_ConsultationOrderDetail.params.PanelID != 'pnlFavoriteConsultationOrderDetail') {
            Favorite_ConsultationOrderDetail.params.PanelID = Favorite_ConsultationOrderDetail.params.PanelID + ' #pnlFavoriteConsultationOrderDetail';
        } else {
            Favorite_ConsultationOrderDetail.params.PanelID = 'pnlFavoriteConsultationOrderDetail';
        }
        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Favorite_ConsultationOrderDetail.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        if (globalAppdata['AppUserName'] == DefaultUser) {
            $('#' + Favorite_ConsultationOrderDetail.params.PanelID + ' #frmFavoriteConsultationOrderDetail #divEntity').show();
        }
        var self = $('#' + Favorite_ConsultationOrderDetail.params.PanelID);
        if(Favorite_ConsultationOrderDetail.params.mode == "Edit")
            $('#' + Favorite_ConsultationOrderDetail.params.PanelID + ' #lblHeading').html('Edit Favorites List');
        self.loadDropDowns(true).done(function () {

            selectedEntity = globalAppdata["SeletedEntityId"];
            $.when(Favorite_ConsultationOrderDetail.loadEntityProvider(selectedEntity)).then(function () {
                var favListId = Favorite_ConsultationOrderDetail.params.FavoriteListId
                if (favListId != null) {

                    // set FavListId to hidden field
                    self.find('#hfFavoriteListId').val(favListId);
                    //Load Detail against FavListId
                    if (favListId > 0) {
                        Favorite_ConsultationOrderDetail.favoriteListSearch(favListId);
                    }
                }
            });

        });
        Favorite_ConsultationOrderDetail.ValidateConsultationOrderDetail();
        

        $(document).ready(function () {

            (function ($) {
                'use strict';
                $(function () {
                    $('[data-plugin-ios-switch]').each(function () {
                        var $this = $(this);

                        $this.themePluginIOS7Switch();
                    });
                });
            }).apply(this, [jQuery]);

            $('[data-plugin-toggle]').each(function () {
                var $this = $(this),
                    opts = {};

                var pluginOptions = $this.data('plugin-options');
                if (pluginOptions)
                    opts = pluginOptions;

                $this.themePluginToggle(opts);
            });


        });

    },


    //Author: Abid Ali
    //Date: 23-03-2016
    //Logic search Validate Consultation Order
    ValidateConsultationOrderDetail: function () {
        var self = $("#" + Favorite_ConsultationOrderDetail.params.PanelID + " #frmFavoriteConsultationOrderDetail");

        self.bootstrapValidator({
            live: 'disabled',
            message: 'This value is not valid',
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {

                FavoriteListName: {
                    group: '.col-sm-10',
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
                Diagnosis: {
                    group: '.col-sm-10',
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                }
            }
        })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            if (Favorite_ConsultationOrderDetail.CPTData.length > 0) {
                //Allow to be submited
                Favorite_ConsultationOrderDetail.favConsultationOrderSave();
            }
            else
                self.data('bootstrapValidator').enableFieldValidators('Diagnosis', true);
        })
            .on('error.form.bv', function (e) {
                e.preventDefault();

                if (Favorite_ConsultationOrderDetail.CPTData.length > 0 && self.find('#txtFavoriteListName').val() != "") {

                    //Disable validator on procedure
                    self.data('bootstrapValidator').enableFieldValidators('Diagnosis', false);
                    self.trigger('success.form.bv');
                }
                else {
                    //Disable validator if cpt has data
                    if (Favorite_ConsultationOrderDetail.CPTData.length > 0)
                        self.data('bootstrapValidator').enableFieldValidators('Diagnosis', false);
                }

            });

    },

    //Author: Abid Ali
    //Date: 23-03-2016
    //Logic search Consultation Order Detail
    favoriteListSearch: function (FavoriteListId, ListType, PageNo, rpp) {

        Favorite_ConsultationOrder.searchFavoriteList_DBCall(ListType, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                var self = $('#' + Favorite_ConsultationOrderDetail.params.PanelID + ' #frmFavoriteConsultationOrderDetail');
                var favListDetail = JSON.parse(response.FavoriteListJSON)[0];
                //Bind to form
                utility.bindMyJSONByName(true, favListDetail, false, self).done(function () {
                    var isActive = false;
                    if (favListDetail.IsActive === "True")
                        isActive = true;
                    self.find('#Active').attr('checked', isActive);
                    self.find('#txtFavoriteListName').val(favListDetail.Name);
                    //Load CPT against favList

                    var FavoriteListProvidersJSON = JSON.parse(response.FavoriteListProviders_JSON);
                    if (FavoriteListProvidersJSON != '' && FavoriteListProvidersJSON) {
                        $('#' + Favorite_ConsultationOrderDetail.params.PanelID + " #ddlConsultationOrderProvider").val(FavoriteListProvidersJSON);

                        $('#' + Favorite_ConsultationOrderDetail.params.PanelID + " #ddlConsultationOrderProvider").multiselect("refresh");
                        $('#' + Favorite_ConsultationOrderDetail.params.PanelID + " #ddlConsultationOrderProvider").multiselect({
                            includeSelectAllOption: true,
                            enableFiltering: true,
                            enableCaseInsensitiveFiltering: true,
                            maxHeight: 247
                        });
                        Favorite_ConsultationOrderDetail.providerCheckedIds = FavoriteListProvidersJSON;
                    } else {
                        $('#' + Favorite_ConsultationOrderDetail.params.PanelID + " #ddlConsultationOrderProvider").find("option:selected").removeAttr("selected");
                        $('#' + Favorite_ConsultationOrderDetail.params.PanelID + " #ddlConsultationOrderProvider").multiselect("refresh");
                    }

                    Favorite_ConsultationOrderDetail.favoriteList_CPTSearch(JSON.parse(response.FavoriteListJSON)[0].FavoriteListId);
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

        //Bind to cpt list



    },
    openSearchPopup: function (searchType, ctrl, hiddenCtrl) {
        var controlToLoad = "";
        if (searchType == "ICD") {
            controlToLoad = "Admin_IMOICD";
        }
        else if (searchType == "CPT") {
            controlToLoad = "Admin_IMOCPT";
        }
        else if (searchType == "Modifier") {
            controlToLoad = "Admin_Modifier";
        }
        var params = [];
        params["FromAdmin"] = "0";
        //params["Parentctrl"] = Clinical_MedicalHx.params["TabID"];
        if (Favorite_ConsultationOrderDetail.params.TabID == 'clinicalTabProgressNote') {
            params['FromProgressNote'] = 'pnlClinicalProgressNote';
            params["ParentCtrl"] = 'Favorite_ConsultationOrderDetail';
        }
        else
            params["ParentCtrl"] = 'Favorite_ConsultationOrderDetail';

        if (ctrl != null) {
            params["RefCtrl"] = ctrl;
        }
        if (hiddenCtrl != null) {
            params["RefHiddenCtrl"] = hiddenCtrl;
        }
        if (controlToLoad != "") {
            if (Favorite_ConsultationOrderDetail.params.TabID == 'clinicalTabProgressNote' && searchType == "ICD")
                LoadActionPan(controlToLoad, params, 'pnlClinicalProgressNote');
            else
                LoadActionPan(controlToLoad, params);
        }

    },
    //Author: Abid Ali
    //Date: 23-03-2016
    //Logic search Consultation Order CPT 
    favoriteList_CPTSearch: function (FavoriteListId, FavoriteListCPTId, PageNo, rpp) {

        Favorite_ConsultationOrder.searchFavoriteList_CPT_DBCall(FavoriteListCPTId, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                //Bind to list
                $.each(JSON.parse(response.FavoriteListCPTJSON), function (index, CPTRow) {
                    Favorite_ConsultationOrderDetail.bindToCPTList(CPTRow.CPTCode, CPTRow.CPTCodeDescription, CPTRow.FavoriteListCPTId);

                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    UnLoadTab: function () {
        Favorite_ConsultationOrderDetail.CPTData = [];
        var objDeffered = $.Deferred();
        if (Favorite_ConsultationOrderDetail.params["FromAdmin"] == "0") {
            if (Favorite_ConsultationOrderDetail.params != null && Favorite_ConsultationOrderDetail.params.ParentCtrl != null) {
                UnloadActionPan(Favorite_ConsultationOrderDetail.params.ParentCtrl, 'Favorite_ConsultationOrderDetail');
            }
            else
                UnloadActionPan(null, 'Favorite_ConsultationOrderDetail');
        }
        else {
            RemoveAdminTab();
        }
        objDeffered.resolve();
        return objDeffered;
    },

    //Author: Abid Ali
    //Date: 23-03-2016
    //Logic implimented to bind CPT code to Procedure field of Consultation Order Detail
    bindAutoComplete: function (element) {

        var hiddenCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "Favorite_ConsultationOrderDetail", null, true);

    },

    //Author: Abid Ali
    //Date: 23-03-2016
    //Logic implimented to bind CPT code to Procedure field of Consultation Order Detail
    openCPTCode: function () {
        var params = [];
        //params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Favorite_ConsultationOrderDetail";
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtCPTCode";
        params["ParentCtrlPanelID"] = Favorite_ConsultationOrderDetail.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, Favorite_ConsultationOrderDetail.params.PanelID);
    },

    //Author: Abid Ali
    //Date: 23-03-2016
    //Logic implimented to bind CPT code to Procedure field of Consultation Order Detail
    bindToCPTList: function (cptCode, cptDescription, FavoriteListCPTId) {
        cptDescription = utility.decodeHtml(cptDescription);

        if (!Favorite_ConsultationOrderDetail.isInCPTList(cptCode, cptDescription)) {
            var item = {};
            item["CPTCode"] = cptCode;
            item["CPTCodeDescription"] = cptDescription;
            item["FavoriteListCPTId"] = FavoriteListCPTId;
            Favorite_ConsultationOrderDetail.CPTData.push(item);

            var $list = $("#" + Favorite_ConsultationOrderDetail.params.PanelID + " #ulFavConsultationOrderDisease");
            cptDescArg = cptDescription;

            if (cptDescArg != "") {
                cptDescArg = ",'" + cptDescArg.trim() + "'";
            }
            var onclick = "Favorite_ConsultationOrderDetail.deleteCPTFromCPTData(this,'" + cptCode + "','" + cptDescription + "','" + FavoriteListCPTId + "');";
            var li = '<li value =' + cptCode + '>' +
            '<span class="pull-left pr-xlg">' + cptCode + " - " + cptDescription + ' </span>' +
                       '<span class="removeIconListHover">' +
                       '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="' + onclick + '"><i class="fa fa-times red"></i></a>' +
                       '</span>' +
                       '<div class="clearfix"></div>' +
                       '</li>';


            $list.append(li);

        }

        //$("#" + Favorite_ConsultationOrderDetail.params.PanelID + " #txtDiagnosis").blur(function () {
        //    $(this).val('');
        //});
        setTimeout(function () {

            $("#" + Favorite_ConsultationOrderDetail.params.PanelID + " #txtDiagnosis").val(cptDescription)
            var bootstrapValidator = $("#" + Favorite_ConsultationOrderDetail.params.PanelID + ' #frmFavoriteConsultationOrderDetail').data('bootstrapValidator');
            bootstrapValidator.revalidateField("Diagnosis");
            $("#" + Favorite_ConsultationOrderDetail.params.PanelID + " #txtDiagnosis").val("");

        }, 150);
       

    },

    //Author: Abid Ali
    //Date: 23-03-2016
    //Delete CPT from CPTData Json Object
    deleteCPTFromCPTData: function (obj, cptCode, cptDescription, FavoriteListCPTId) {

        cptDescription = typeof cptDescription == "undefined" ? "" : cptDescription.toString();
        $.each(Favorite_ConsultationOrderDetail.CPTData, function (index, item) {
            if (item["CPTCode"] == cptCode.toString() && item["CPTCodeDescription"] == cptDescription) {

                Favorite_ConsultationOrderDetail.CPTData.splice(index, 1);
                $(obj).parent().parent().remove();
                if (Favorite_ConsultationOrderDetail.params.FavoriteListId > 0 && FavoriteListCPTId) {
                    Favorite_ConsultationOrderDetail.deleteCPT_DB_Call(Favorite_ConsultationOrderDetail.params.FavoriteListId, FavoriteListCPTId);
                }
                return false;
            }
        });
    },

    //Author: Abid Ali
    //Date: 23-03-2016
    //Find cptcode and discription in CPTData array

    deleteCPT_DB_Call: function (FavouriteListId, FavoriteListCPTId) {
        var objData = {};
        objData["FavoriteListId"] = FavouriteListId == null ? 0 : FavouriteListId;
        objData["FavoriteListCPTId"] = FavoriteListCPTId == null ? 0 : FavoriteListCPTId;
        objData["commandType"] = "delete_favoritelist_cpt";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },
    isInCPTList: function (cptCode, cptDescription) {
        var isExists = false;
        $.each(Favorite_ConsultationOrderDetail.CPTData, function (index, item) {

            if (item["CPTCode"] == cptCode && item["CPTCodeDescription"] == cptDescription) {
                isExists = true;
                return false;
            }
        });
        return isExists;
    },

    //Start//23/03/2016//Abid Ali//Implimented save Call for Fav ConsultationOrder
    favConsultationOrderSave: function () {
        var strMessage = "";
        if (Favorite_ConsultationOrderDetail.CPTData.length > 0) {
            //var myJSON = JSON.stringify(Favorite_ConsultationOrderDetail.FavConsultationOrder);
            AppPrivileges.GetFormPrivileges("Favorites_ConsultationOrder", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Favorite_ConsultationOrderDetail.saveFavConsultationOrder().done(function (response) {
                        response = JSON.parse(response);
                        var self = $('#' + Favorite_ConsultationOrderDetail.params.PanelID + ' #frmFavoriteConsultationOrderDetail');
                        if (response.status != false) {
                            Favorite_ConsultationOrderDetail.UnLoadTab();

                            //Empty Grid and ul and perform search
                            Favorite_ConsultationOrder.emptyGridAndList();
                            Favorite_ConsultationOrder.favoriteListSearch();

                            self.find('#hfFavoriteListId').val(response.FavCPTId);
                            utility.DisplayMessages(response.Message, 1);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 2);
                        }
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else {
            utility.DisplayMessages("There is no Favorite Consultation to add", 3);/// ask from babur bhai
        }
    },
    //End/22/03/2016/Abid Ali/Implimented save Call for Fav ConsultationOrder

    //Start//23/03/2016//Abid Ali//Implimented save DbCall for Fav ConsultationOrder
    saveFavConsultationOrder: function () {
        //FavoriteListIcd
        var objData = {};
        var self = $('#' + Favorite_ConsultationOrderDetail.params.PanelID + ' #frmFavoriteConsultationOrderDetail');
        favoriteListId = self.find('#hfFavoriteListId').val();
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objDetail = JSON.parse(myJSON);
        if (objDetail["ProviderIds"] != '') {
            Favorite_ConsultationOrderDetail.validateProvider(2);
            objData["FavoriteListId"] = favoriteListId;
            objData["ListType"] = "ConsultationOrder";
            objData["EntityId"] = objDetail["EntityId"];
            objData["FavoriteListName"] = objDetail["FavoriteListName"];
            if (globalAppdata['AppUserName'] != DefaultUser) {
                objData["EntityId"] = -1;
            }
            objData["IsActive"] = objDetail.Active;
            objData["FavoriteListCPT"] = Favorite_ConsultationOrderDetail.CPTData;
            objData["ProviderIds"] = objDetail["ProviderIds"];
            objData["commandType"] = "SAVE_FavConsultationOrder";
            var data = JSON.stringify(objData);
            return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
        }
        else {
            Favorite_ConsultationOrderDetail.validateProvider(1);
        }
    },

    loadEntityProvider: function (entityId) {
        var objDeffered = $.Deferred();
        var data = "entityID=" + entityId;
        if (entityId != null && entityId > 0) {

            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $('#' + Favorite_ConsultationOrderDetail.params.PanelID + ' #ddlConsultationOrderProvider');
                var $providerHiddenDdl = $('#' + Favorite_ConsultationOrderDetail.params.PanelID + ' #ddlHiddenFavConsultationOrderProvider');

                $providerDdl.empty();
                $providerHiddenDdl.empty();

                //Loop through all providers loaded from the server
                $.each(options, function (i, item) {
                    if (item.Value != "" && typeof item.Value != 'undefined') {

                        // User will see these providers in multiSelect dropdownlist
                        $providerDdl.append(
                            $('<option/>', {
                                value: item.Value,
                                html: item.Name,
                                refname: item.RefName,
                                refvalue: item.RefValue

                            })
                        );
                        // Populate hidden ddl provider
                        //A Hack to load all the providers in hidden dropdownlist
                        $providerHiddenDdl.append(
                             $('<option/>', {
                                 value: item.Value,
                                 html: item.Name,
                                 refname: item.RefName,
                                 refvalue: item.RefValue

                             })
                        );
                    }
                });
                // Assigned server side providers to providerCheckedIds array and made selected
                if (Favorite_ConsultationOrderDetail.ProviderIds != '') {
                    var Providers = Favorite_ConsultationOrderDetail.ProviderIds.split(",");
                    Favorite_ConsultationOrderDetail.providerCheckedIds = Providers;
                    $('#' + Favorite_ConsultationOrderDetail.params.PanelID + ' #ddlConsultationOrderProvider').val(Providers);
                }

            }).then(function () {
                //Intialized in onhidden spacialty ddl.
                Favorite_ConsultationOrderDetail.IntializeMultiSelectDropDownProviders();
                objDeffered.resolve();

            });
        }
        else {
            objDeffered.resolve();
        }
        return objDeffered;
    },

    IntializeMultiSelectDropDownProviders: function () {
        $('#' + Favorite_ConsultationOrderDetail.params.PanelID + ' #ddlConsultationOrderProvider').multiselect('destroy');
        $('#' + Favorite_ConsultationOrderDetail.params.PanelID + ' #ddlConsultationOrderProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 200,
            nonSelectedText: 'Select',
            selectAll: false,
            onDropdownHide: function (event) {
            },
            onChange: function () {
                Favorite_ConsultationOrderDetail.CheckProviderValidation();
            },
        });
    },
    CheckProviderValidation: function () {
        var self = $("#" + Favorite_ConsultationOrderDetail.params.PanelID);
        var ProviderIds = self.find('#ddlConsultationOrderProvider option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        var strProviderIds = ProviderIds;
        Favorite_ConsultationOrderDetail.providerCheckedIds = ProviderIds;
        if (Favorite_ConsultationOrderDetail.providerCheckedIds != '') {
            Favorite_ConsultationOrderDetail.validateProvider(2);
        } else {
            Favorite_ConsultationOrderDetail.validateProvider(1);
        }
    },
    //End//23/03/2016//Abid Ali/Implimented save DbCall for Fav ConsultationOrder
    validateProvider: function (operationid) {
        var self = "#" + Favorite_ConsultationOrderDetail.params.PanelID + " #frmFavoriteConsultationOrderDetail";
        $(self + " #divFavConsultationOrderProvider").find("i").remove();
        if (operationid == 1) {
            $(self + " .multiselect").css("border-color", "#cc2724");
            $(self + " #divFavConsultationOrderProvider").find(".control-label").css("color", "#cc2724");
            $(self + " #divFavConsultationOrderProvider").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-remove" data-bv-icon-for="Race" style="display: block;"></i>');
        } else if (operationid == 2) {
            $(self + " .multiselect").css("border-color", "#3c763d");
            $(self + " #divFavConsultationOrderProvider").find(".control-label").css("color", "#3c763d");
            $(self + " #divFavConsultationOrderProvider").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-ok" data-bv-icon-for="Race" style="display: block;"></i>');
        } else {
            $(self + " .multiselect").css("border-color", "#ccc");
            $(self + " #divFavConsultationOrderProvider").find(".control-label").css("color", "#000000");
        }
    },
}