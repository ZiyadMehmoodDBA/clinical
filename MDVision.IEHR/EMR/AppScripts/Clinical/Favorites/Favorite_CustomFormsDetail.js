Favorite_CustomFormsDetail = {
    bIsFirstLoad: true,
    params: [],
    Switch: 1,
    CPTData: [],
    providerCheckedIds: [],
    customFormCheckedIds: [],
    FormData: [],
    ProviderIds: '',
    CustomFormIds: '',
    Load: function (params) {
        Favorite_CustomFormsDetail.params = params;
        if (Favorite_CustomFormsDetail.params.PanelID != 'pnlFavoriteCustomFormsDetail') {
            Favorite_CustomFormsDetail.params.PanelID = Favorite_CustomFormsDetail.params.PanelID + ' #pnlFavoriteCustomFormsDetail';
        } else {
            Favorite_CustomFormsDetail.params.PanelID = 'pnlFavoriteCustomFormsDetail';
        }
        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Favorite_CustomFormsDetail.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        if (globalAppdata['AppUserName'] == DefaultUser) {
            $('#' + Favorite_CustomFormsDetail.params.PanelID + ' #frmFavoriteCustomFormsDetail #divEntity').show();
        }
        var self = $('#' + Favorite_CustomFormsDetail.params.PanelID);
        

        self.loadDropDowns(true).done(function () {
            Favorite_CustomFormsDetail.loadEntityProvider().done(function () {
                Favorite_CustomFormsDetail.IntializeMultiSelectDropDownCustomForms();
                Favorite_CustomFormsDetail.loadProviderBasedCustomForms().done(function () {
                    if (Favorite_CustomFormsDetail.params.mode == "Add") {
                        $('#' + Favorite_CustomFormsDetail.params.PanelID + ' #frmFavoriteCustomFormsDetail #IsActive').prop("disabled", true);
                    } else {
                        $('#' + Favorite_CustomFormsDetail.params.PanelID + ' #lblHeading').html('Edit Favorites List');
                        $('#' + Favorite_CustomFormsDetail.params.PanelID + ' #frmFavoriteCustomFormsDetail #IsActive').prop("disabled", false);
                    }
                    var favListId = Favorite_CustomFormsDetail.params.FavoriteListId
                    if (favListId != null) {

                        // set FavListId to hidden field
                        self.find('#hfFavoriteListId').val(favListId);
                        $("#" + Favorite_CustomForms.params.PanelID + " #hfFavoriteListId").val(favListId);
                        //Load Detail against FavListId
                        if (favListId > 0) {
                            Favorite_CustomFormsDetail.favoriteListSearch(favListId);
                        }
                    }
                    $('#frmFavoriteCustomFormsDetail').data('serialize', $('#frmFavoriteCustomFormsDetail').serialize());
                });

            });

        });
        Favorite_CustomFormsDetail.ValidateCustomFormsDetail();
        //var favListId = Favorite_CustomFormsDetail.params.FavoriteListId
        //if (favListId != null) {

        //    // set FavListId to hidden field
        //    self.find('#hfFavoriteListId').val(favListId);
        //    //Load Detail against FavListId
        //    if (favListId > 0) {
        //        Favorite_CustomFormsDetail.favoriteListSearch(favListId);
        //    }
        //}

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
    ValidateCustomFormsDetail: function () {
        var self = $("#" + Favorite_CustomFormsDetail.params.PanelID + " #frmFavoriteCustomFormsDetail");

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
                ProviderIds: {
                    group: '.col-sm-10',
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
                CustomFormIds: {
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
            var self = $('#' + Favorite_CustomFormsDetail.params.PanelID + ' #frmFavoriteCustomFormsDetail');
            var ProviderIds = self.find('#ddlProvider option:Selected');
            Favorite_CustomFormsDetail.ProviderIds = ProviderIds;
            var customFormIds = self.find('#ddlCustomForm option:Selected');
            Favorite_CustomFormsDetail.CustomFormIds = customFormIds;
            if (Favorite_CustomFormsDetail.ProviderIds.length > 0 && Favorite_CustomFormsDetail.CustomFormIds.length > 0) {
                //Allow to be submited
                Favorite_CustomFormsDetail.favConsultationOrderSave();
            }
            else if (Favorite_CustomFormsDetail.ProviderIds.length > 0 && Favorite_CustomFormsDetail.CustomFormIds.length < 1) {
                Favorite_CustomFormsDetail.validateProvider(2);
                Favorite_CustomFormsDetail.validateCustomForm(1);
                //self.data('bootstrapValidator').enableFieldValidators('ProviderIds', true);
                //self.data('bootstrapValidator').enableFieldValidators('CustomFormIds', true);
            } else if (Favorite_CustomFormsDetail.ProviderIds.length < 1 && Favorite_CustomFormsDetail.CustomFormIds.length > 0) {
                Favorite_CustomFormsDetail.validateProvider(1);
                Favorite_CustomFormsDetail.validateCustomForm(2);
            } else {
                Favorite_CustomFormsDetail.validateProvider(1);
                Favorite_CustomFormsDetail.validateCustomForm(1);
            }
        })
            .on('error.form.bv', function (e) {
                e.preventDefault();

                if (Favorite_CustomFormsDetail.ProviderIds.length > 0 && Favorite_CustomFormsDetail.CustomFormIds.length > 0 && self.find('#txtFavoriteListName').val() != "") {

                    Favorite_CustomFormsDetail.validateProvider(2);
                    Favorite_CustomFormsDetail.validateCustomForm(2);

                    //Disable validator on procedure
                    //self.data('bootstrapValidator').enableFieldValidators('ProviderIds', false);
                    //self.data('bootstrapValidator').enableFieldValidators('CustomFormIds', false);
                    //self.trigger('success.form.bv');
                }
                else if (Favorite_CustomFormsDetail.ProviderIds.length > 0 && Favorite_CustomFormsDetail.CustomFormIds.length < 1) {
                    Favorite_CustomFormsDetail.validateProvider(2);
                    Favorite_CustomFormsDetail.validateCustomForm(1);
                   
                } else if (Favorite_CustomFormsDetail.ProviderIds.length < 1 && Favorite_CustomFormsDetail.CustomFormIds.length > 0) {
                    Favorite_CustomFormsDetail.validateProvider(1);
                    Favorite_CustomFormsDetail.validateCustomForm(2);

                } else {
                    Favorite_CustomFormsDetail.validateProvider(1);
                    Favorite_CustomFormsDetail.validateCustomForm(1);
                }

            });

    },

    //Author: Abid Ali
    //Date: 23-03-2016
    //Logic search Consultation Order Detail
    favoriteListSearch: function (FavoriteListId, ListType, PageNo, rpp) {

        Favorite_CustomFormsDetail.searchFavoriteList_DBCall(ListType, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                var self = $('#' + Favorite_CustomFormsDetail.params.PanelID + ' #frmFavoriteCustomFormsDetail');
                var favListDetail = JSON.parse(response.FavoriteListJSON)[0];
                var providers = JSON.parse(response.FavoriteListProviders_JSON)
                $('#' + Favorite_CustomFormsDetail.params.PanelID + " #ddlProvider").val(providers);
                $('#' + Favorite_CustomFormsDetail.params.PanelID + ' #ddlProvider').multiselect("refresh");
                Favorite_CustomFormsDetail.loadProviderBasedCustomForms().done(function () {
                    //Bind to form
                    utility.bindMyJSONByName(true, favListDetail, false, self).done(function () {
                        var isActive = false;
                        if (favListDetail.IsActive === "True")
                            isActive = true;
                        //self.find('#Active').attr('checked', isActive);
                        self.find('#txtFavoriteListName').val(favListDetail.Name);
                        //Load CPT against favList
                        Favorite_CustomFormsDetail.favoriteList_CPTSearch(JSON.parse(response.FavoriteListJSON)[0].FavoriteListId);
                        $('#frmFavoriteCustomFormsDetail').data('serialize', $('#frmFavoriteCustomFormsDetail').serialize());
                    });
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
        if (Favorite_CustomFormsDetail.params.TabID == 'clinicalTabProgressNote') {
            params['FromProgressNote'] = 'pnlClinicalProgressNote';
            params["ParentCtrl"] = 'Favorite_CustomFormsDetail';
        }
        else
            params["ParentCtrl"] = 'Favorite_CustomFormsDetail';

        if (ctrl != null) {
            params["RefCtrl"] = ctrl;
        }
        if (hiddenCtrl != null) {
            params["RefHiddenCtrl"] = hiddenCtrl;
        }
        if (controlToLoad != "") {
            if (Favorite_CustomFormsDetail.params.TabID == 'clinicalTabProgressNote' && searchType == "ICD")
                LoadActionPan(controlToLoad, params, 'pnlClinicalProgressNote');
            else
                LoadActionPan(controlToLoad, params);
        }

    },
    //Author: Abid Ali
    //Date: 23-03-2016
    //Logic search Consultation Order CPT 
    favoriteList_CPTSearch: function (FavoriteListId, FavoriteListCustomFormId, PageNo, rpp) {

        Favorite_CustomFormsDetail.searchFavoriteList_CustomForm_DBCall(FavoriteListCustomFormId, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                //Bind to list
                $.each(JSON.parse(response.FavoriteListCustomFormJSON), function (index, CustomFormRow) {
                    Favorite_CustomFormsDetail.bindToCPTList(CustomFormRow.CustomFormId, CustomFormRow.CustomFormName);

                });
                $('#frmFavoriteCustomFormsDetail').data('serialize', $('#frmFavoriteCustomFormsDetail').serialize());
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    searchFavoriteList_CustomForm_DBCall: function (FavoriteListCustomFormId, FavoriteListId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = -1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = -1;
        }
        var objData = {};

        objData["FavoriteListId"] = FavoriteListId == null ? 0 : FavoriteListId;
        objData["FavoriteListCustomFormId"] = FavoriteListCustomFormId == null ? -1 : FavoriteListCustomFormId;
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;

        objData["commandType"] = "load_favoritelist_CustomForm";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },


    UnLoadTab: function (fromSave) {
        if (fromSave && fromSave != null) {
            Favorite_CustomFormsDetail.Unload();
        } else {
            utility.UnLoadDialog("frmFavoriteCustomFormsDetail", function () {
                Favorite_CustomFormsDetail.Unload();
            }, function () {
                Favorite_CustomFormsDetail.Unload();
            });
        }
    },
    Unload: function () {
        Favorite_CustomFormsDetail.FormData = [];
        var objDeffered = $.Deferred();
        if (Favorite_CustomFormsDetail.params["FromAdmin"] == "0") {
            if (Favorite_CustomFormsDetail.params != null && Favorite_CustomFormsDetail.params.ParentCtrl != null) {
                UnloadActionPan(Favorite_CustomFormsDetail.params.ParentCtrl, 'Favorite_CustomFormsDetail');
            }
            else
                UnloadActionPan(null, 'Favorite_CustomFormsDetail');
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
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "Favorite_CustomFormsDetail", null, true);

    },

    //Author: Abid Ali
    //Date: 23-03-2016
    //Logic implimented to bind CPT code to Procedure field of Consultation Order Detail
    openCPTCode: function () {
        var params = [];
        //params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Favorite_CustomFormsDetail";
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtCPTCode";
        params["ParentCtrlPanelID"] = Favorite_CustomFormsDetail.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, Favorite_CustomFormsDetail.params.PanelID);
    },

    //Author: Abid Ali
    //Date: 23-03-2016
    //Logic implimented to bind CPT code to Procedure field of Consultation Order Detail
    bindToCPTList: function (customFormId, customFormName) {

        if (!Favorite_CustomFormsDetail.isInCPTList(customFormId, customFormName)) {
            item = {}
            item["CustomFormId"] = customFormId;
            item["CustomFormName"] = customFormName;
            Favorite_CustomFormsDetail.FormData.push(item);

            $('#' + Favorite_CustomFormsDetail.params.PanelID + ' #ddlCustomForm option[value=' + customFormId + ']').attr('selected', true);//.trigger('click');
            $('#' + Favorite_CustomFormsDetail.params.PanelID + ' #ddlCustomForm').multiselect("refresh");


            //var $list = $("#" + Favorite_CustomFormsDetail.params.PanelID + " #ulFavConsultationOrderDisease");
            //cptDescArg = cptDescription;

            //if (cptDescArg != "") {
            //    cptDescArg = ",'" + cptDescArg.trim() + "'";
            //}
            //var onclick = "Favorite_CustomFormsDetail.deleteCPTFromCPTData(this,'" + cptCode + "','" + cptDescription + "');";
            //var li = '<li value =' + cptCode + '>' +
            //'<span class="pull-left pr-xlg">' + cptCode + " - " + cptDescription + ' </span>' +
            //           '<span class="removeIconListHover">' +
            //           '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="' + onclick + '"><i class="fa fa-times red"></i></a>' +
            //           '</span>' +
            //           '<div class="clearfix"></div>' +
            //           '</li>';


            //$list.append(li);

        }

        //$("#" + Favorite_CustomFormsDetail.params.PanelID + " #txtDiagnosis").blur(function () {
        //    $(this).val('');
        //});
        //setTimeout(function () {

        //    $("#" + Favorite_CustomFormsDetail.params.PanelID + " #txtDiagnosis").val("");

        //}, 150);


    },

    //Author: Abid Ali
    //Date: 23-03-2016
    //Delete CPT from CPTData Json Object
    deleteCPTFromCPTData: function (obj, cptCode, cptDescription) {

        cptDescription = typeof cptDescription == "undefined" ? "" : cptDescription.toString();
        $.each(Favorite_CustomFormsDetail.CPTData, function (index, item) {
            if (item["CPTCode"] == cptCode.toString() && item["CPTCodeDescription"] == cptDescription) {

                Favorite_CustomFormsDetail.CPTData.splice(index, 1);
                $(obj).parent().parent().remove();
                return false;
            }
        });
    },

    //Author: Abid Ali
    //Date: 23-03-2016
    //Find cptcode and discription in CPTData array
    isInCPTList: function (customFormId, customFormName) {
        var isExists = false;
        $.each(Favorite_CustomFormsDetail.FormData, function (index, item) {

            if (item["CustomFormId"] == customFormId && item["CustomFormName"] == customFormName) {
                isExists = true;
                return false;
            }
        });
        return isExists;
    },

    IntializeMultiSelectDropDownProviders: function () {
        $('#' + Favorite_CustomFormsDetail.params.PanelID + ' #ddlProvider').multiselect('destroy');
        $('#' + Favorite_CustomFormsDetail.params.PanelID + ' #ddlProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 200,
            nonSelectedText: 'Select',
            selectAll: false,
            onDropdownHide: function (event) {
            },
            onChange: function () {
                Favorite_CustomFormsDetail.CheckProviderValidation();
                Favorite_CustomFormsDetail.loadProviderBasedCustomForms();
            },
        });
    },
    IntializeMultiSelectDropDownCustomForms: function () {
        $('#' + Favorite_CustomFormsDetail.params.PanelID + ' #ddlCustomForm').multiselect('destroy');
        $('#' + Favorite_CustomFormsDetail.params.PanelID + ' #ddlCustomForm').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 200,
            nonSelectedText: 'Select',
            selectAll: false,
            onDropdownHide: function (event) {
            },
            onChange: function () {
                Favorite_CustomFormsDetail.CheckCustomFormValidation();
            },
        });
    },
    CheckProviderValidation: function () {
        var self = $("#" + Favorite_CustomFormsDetail.params.PanelID);
        var ProviderIds = self.find('#ddlProvider option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        var strProviderIds = ProviderIds;
        Favorite_CustomFormsDetail.providerCheckedIds = ProviderIds;
        if (Favorite_CustomFormsDetail.providerCheckedIds != '') {
            Favorite_CustomFormsDetail.validateProvider(2);
        } else {
            Favorite_CustomFormsDetail.validateProvider(1);
        }
    },
    CheckCustomFormValidation: function () {
        var self = $("#" + Favorite_CustomFormsDetail.params.PanelID);
        var ProviderIds = self.find('#ddlCustomForm option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        var strProviderIds = ProviderIds;
        Favorite_CustomFormsDetail.customFormCheckedIds = ProviderIds;
        if (Favorite_CustomFormsDetail.customFormCheckedIds != '') {
            Favorite_CustomFormsDetail.validateCustomForm(2);
        } else {
            Favorite_CustomFormsDetail.validateCustomForm(1);
        }
    },

    validateProvider: function (operationid) {
        var self = "#" + Favorite_CustomFormsDetail.params.PanelID + " #frmFavoriteCustomFormsDetail";
        $(self + " #divProvider").find("i").remove();
        if (operationid == 1) {
            $(self + " #divProvider .multiselect").css("border-color", "#cc2724");
            $(self + " #divProvider").find(".control-label").css("color", "#cc2724");
            $(self + " #divProvider").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-remove" data-bv-icon-for="Race" style="display: block;"></i>');
        } else if (operationid == 2) {
            $(self + " #divProvider .multiselect").css("border-color", "#3c763d");
            $(self + " #divProvider").find(".control-label").css("color", "#3c763d");
            $(self + " #divProvider").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-ok" data-bv-icon-for="Race" style="display: block;"></i>');
        } else {
            $(self + " #divProvider .multiselect").css("border-color", "#ccc");
            $(self + " #divProvider").find(".control-label").css("color", "#000000");
        }
    },
    validateCustomForm: function (operationid) {
        var self = "#" + Favorite_CustomFormsDetail.params.PanelID + " #frmFavoriteCustomFormsDetail";
        $(self + " #divCustomForm").find("i").remove();
        if (operationid == 1) {
            $(self + " #divCustomForm .multiselect").css("border-color", "#cc2724");
            $(self + " #divCustomForm").find(".control-label").css("color", "#cc2724");
            $(self + " #divCustomForm").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-remove" data-bv-icon-for="Race" style="display: block;"></i>');
        } else if (operationid == 2) {
            $(self + " #divCustomForm .multiselect").css("border-color", "#3c763d");
            $(self + " #divCustomForm").find(".control-label").css("color", "#3c763d");
            $(self + " #divCustomForm").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-ok" data-bv-icon-for="Race" style="display: block;"></i>');
        } else {
            $(self + " #divCustomForm .multiselect").css("border-color", "#ccc");
            $(self + " #divCustomForm").find(".control-label").css("color", "#000000");
        }
    },

    loadEntityProvider: function () {
        var objDeffered = $.Deferred();
        selectedEntity = globalAppdata["SeletedEntityId"];
        var data = "entityID=" + selectedEntity;
        if (selectedEntity != null && selectedEntity > 0) {

            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $('#' + Favorite_CustomFormsDetail.params.PanelID + ' #ddlProvider');
                var $providerHiddenDdl = $('#' + Favorite_CustomFormsDetail.params.PanelID + ' #ddlHiddenProvider');

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
                if (Favorite_CustomFormsDetail.ProviderIds != '') {
                    var Providers = Favorite_CustomFormsDetail.ProviderIds;
                    //if (Favorite_CustomFormsDetail.ProviderIds.length > 1) {
                    //     Providers = Favorite_CustomFormsDetail.ProviderIds.split(",");
                    //}
                    
                    Favorite_CustomFormsDetail.providerCheckedIds = Providers;
                    $('#ddlProvider').val(Providers);
                }

            }).then(function () {
                //Intialized in onhidden spacialty ddl.
                Favorite_CustomFormsDetail.IntializeMultiSelectDropDownProviders();
                objDeffered.resolve();

            });
        }
        else {
            objDeffered.resolve();
        }
        return objDeffered;
    },
    loadProviderBasedCustomForms: function () {
        var objDeffered = $.Deferred();
        selectedEntity = globalAppdata["SeletedEntityId"];


        var self = $('#' + Favorite_CustomFormsDetail.params.PanelID + ' #frmFavoriteCustomFormsDetail');
        var ProviderIds = self.find('#ddlProvider option:Selected').map(function () {
            return this.value;
        }).get().join(',');

        var data = "ProviderIDs=" + ProviderIds;
        if (ProviderIds != null && ProviderIds != '') {

            MDVisionService.lookups('GetCustomFormsByProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetCustomFormsByProvider"]);
                var options = result;
                var $providerDdl = $('#' + Favorite_CustomFormsDetail.params.PanelID + ' #ddlCustomForm');
                var $providerHiddenDdl = $('#' + Favorite_CustomFormsDetail.params.PanelID + ' #ddlHiddenCustomForm');

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
                if (Favorite_CustomFormsDetail.CustomFormIds != '') {
                    var customForms = Favorite_CustomFormsDetail.CustomFormIds;
                    //if (Favorite_CustomFormsDetail.CustomFormIds.length > 1) {
                    //    customForms = Favorite_CustomFormsDetail.CustomFormIds.split(",");
                    //}
                   
                    Favorite_CustomFormsDetail.customFormCheckedIds = customForms;
                    $('#ddlCustomForm').val(customForms);
                }

            }).then(function () {
                //Intialized in onhidden spacialty ddl.
                Favorite_CustomFormsDetail.IntializeMultiSelectDropDownCustomForms();
                objDeffered.resolve();

            });
        }
        else {
            objDeffered.resolve();
        }
        return objDeffered;
    },
    //Start//23/03/2016//Abid Ali//Implimented save Call for Fav ConsultationOrder
    favConsultationOrderSave: function () {
        var strMessage = "";
        var self = $('#' + Favorite_CustomFormsDetail.params.PanelID + ' #frmFavoriteCustomFormsDetail');
        var ProviderIds = self.find('#ddlProvider option:Selected');
        var customFormIds = self.find('#ddlCustomForm option:Selected');
        if (ProviderIds.length > 0 && customFormIds.length > 0) {
            //var myJSON = JSON.stringify(Favorite_CustomFormsDetail.FavConsultationOrder);
            AppPrivileges.GetFormPrivileges("Favorites_CustomForms", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Favorite_CustomFormsDetail.saveFavConsultationOrder().done(function (response) {
                        response = JSON.parse(response);
                        var self = $('#' + Favorite_CustomFormsDetail.params.PanelID + ' #frmFavoriteCustomFormsDetail');
                        if (response.status != false) {
                            self.find('#hfFavoriteListId').val(response.FavId);
                            $("#" + Favorite_CustomForms.params.PanelID + " #hfFavoriteListId").val(response.FavId);
                            Favorite_CustomFormsDetail.UnLoadTab(true);

                            //Empty Grid and ul and perform search
                            Favorite_CustomForms.emptyGridAndList();
                            Favorite_CustomForms.favoriteListSearch();
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
            utility.DisplayMessages("There is no Favorite Custom Form to add", 3);/// ask from babur bhai
        }
    },
    //End/22/03/2016/Abid Ali/Implimented save Call for Fav ConsultationOrder


    emptyGridAndList: function () {
        $("#" + Favorite_CustomForms.params.PanelID + ' #dgvFavoritiesListCustomForms tbody').empty();
        
    },
    //Start//23/03/2016//Abid Ali//Implimented save DbCall for Fav ConsultationOrder
    saveFavConsultationOrder: function () {
        //FavoriteListIcd
        var objData = {};
        var self = $('#' + Favorite_CustomFormsDetail.params.PanelID + ' #frmFavoriteCustomFormsDetail');
        favoriteListId = self.find('#hfFavoriteListId').val();
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objDetail = JSON.parse(myJSON);
        objData["FavoriteListId"] = favoriteListId;
        objData["ListType"] = "CustomForms";
        objData["EntityId"] = objDetail["EntityId"];
        objData["BodyPartId"] = objDetail["BodyPartId"];
        objData["FavoriteListName"] = objDetail["FavoriteListName"];
        if (globalAppdata['AppUserName'] != DefaultUser) {
            objData["EntityId"] = -1;
        }
        objData["IsActive"] = objDetail.IsActive;
        var ProviderIds = self.find('#ddlProvider option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objData["ProviderIds"] = ProviderIds;

        var CustomFormIds = self.find('#ddlCustomForm option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objData["CustomFormsIds"] = CustomFormIds;
        Favorite_CustomFormsDetail.FormData = [];
        $.each(self.find('#ddlCustomForm option:Selected'), function (index, val) {
            var item = {}
            item["CustomFormId"] = val.value;
            item["FormName"] = val.text;

            Favorite_CustomFormsDetail.FormData.push(item);
        });

        objData["FavoriteListCustomForms"] = Favorite_CustomFormsDetail.FormData;

        //   objData["CustomFormIDs"] = CustomFormIds;
        objData["commandType"] = "SAVE_FavCustomForms";
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

    //End//23/03/2016//Abid Ali/Implimented save DbCall for Fav ConsultationOrder

}