Favorite_ProcedureDetail = {
    bIsFirstLoad: true,
    params: [],
    Switch: 1,
    CPTData: [],
    FavProcedure: [],
    ProviderIds: '',
    providerCheckedIds: [],
    Load: function (params) {
        Favorite_ProcedureDetail.params = params;
        if (Favorite_ProcedureDetail.params.PanelID != 'pnlFavoriteProcedureDetail') {
            Favorite_ProcedureDetail.params.PanelID = Favorite_ProcedureDetail.params.PanelID + ' #pnlFavoriteProcedureDetail';
        } else {
            Favorite_ProcedureDetail.params.PanelID = 'pnlFavoriteProcedureDetail';
        }
        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Favorite_ProcedureDetail.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        if (globalAppdata['AppUserName'] == DefaultUser) {
            $('#' + Favorite_ProcedureDetail.params.PanelID + ' #frmFavoriteProcedureDetail #divEntity').show();
        }
        var self = $('#' + Favorite_ProcedureDetail.params.PanelID);
        self.loadDropDowns(true).done(function () {
            selectedEntity = globalAppdata["SeletedEntityId"];
            $.when(Favorite_ProcedureDetail.loadEntityProvider(selectedEntity)).then(function () {

                Favorite_ProcedureDetail.ValidateFavProcedure();

                var favListId = Favorite_ProcedureDetail.params.FavoriteListId
                if (favListId != null) {
                    self.find('#hfFavoriteListId').val(favListId);
                    if (favListId > 0) {
                        Favorite_ProcedureDetail.favoriteListSearch(favListId);
                    }
                }
            });
            if (Favorite_ProcedureDetail.params.mode == "Add") {

                $('#' + Favorite_ProcedureDetail.params.PanelID + ' #frmFavoriteProcedureDetail #Active').prop("disabled", true);
            }
            else if (Favorite_ProcedureDetail.params.mode == "Edit") {
                $('#' + Favorite_ProcedureDetail.params.PanelID + ' #lblHeading').html('Edit Favorites List');
                $('#' + Favorite_ProcedureDetail.params.PanelID + ' #frmFavoriteProcedureDetail #Active').prop("disabled", false);
            }
        });

        $(function () {
            $('[data-plugin-toggle]').each(function () {
                var $this = $(this),
                    opts = {};

                var pluginOptions = $this.data('plugin-options');
                if (pluginOptions)
                    opts = pluginOptions;

                $this.themePluginToggle(opts);
            });
        });

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


        });
    },
    loadEntityProvider: function (entityId) {
        var objDeffered = $.Deferred();
        var data = "entityID=" + entityId;
        if (entityId != null && entityId > 0) {

            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $('#' + Favorite_ProcedureDetail.params.PanelID + ' #ddlProcedureProvider');
                var $providerHiddenDdl = $('#' + Favorite_ProcedureDetail.params.PanelID + ' #ddlHiddenFavProcedureProvider');

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
                if (Favorite_ProcedureDetail.ProviderIds != '') {
                    var Providers = Favorite_ProcedureDetail.ProviderIds.split(",");
                    Favorite_ProcedureDetail.providerCheckedIds = Providers;
                    $('#' + Favorite_ProcedureDetail.params.PanelID + ' #ddlProcedureProvider').val(Providers);
                }

            }).then(function () {
                //Intialized in onhidden spacialty ddl.
                Favorite_ProcedureDetail.IntializeMultiSelectDropDownProviders();
                objDeffered.resolve();

            });
        }
        else {
            objDeffered.resolve();
        }
        return objDeffered;
    },
    IntializeMultiSelectDropDownProviders: function () {
        $('#' + Favorite_ProcedureDetail.params.PanelID + ' #ddlProcedureProvider').multiselect('destroy');
        $('#' + Favorite_ProcedureDetail.params.PanelID + ' #ddlProcedureProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 200,
            nonSelectedText: 'Select',
            selectAll: false,
            onDropdownHide: function (event) {
            },
            onChange: function () {
                Favorite_ProcedureDetail.CheckProviderValidation();
            },
        });
    },
    validateProvider: function (operationid) {
        var self = "#" + Favorite_ProcedureDetail.params.PanelID + " #frmFavoriteProcedureDetail";
        $(self + " #divFavProcedureProvider").find("i").remove();
        if (operationid == 1) {
            $(self + " .multiselect").css("border-color", "#cc2724");
            $(self + " #divFavProcedureProvider").find(".control-label").css("color", "#cc2724");
            $(self + " #divFavProcedureProvider").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-remove" data-bv-icon-for="Race" style="display: block;"></i>');
        } else if (operationid == 2) {
            $(self + " .multiselect").css("border-color", "#3c763d");
            $(self + " #divFavProcedureProvider").find(".control-label").css("color", "#3c763d");
            $(self + " #divFavProcedureProvider").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-ok" data-bv-icon-for="Race" style="display: block;"></i>');
        } else {
            $(self + " .multiselect").css("border-color", "#ccc");
            $(self + " #divFavProcedureProvider").find(".control-label").css("color", "#000000");
        }
    },
    CheckProviderValidation: function () {
        var self = $("#" + Favorite_ProcedureDetail.params.PanelID);
        var ProviderIds = self.find('#ddlProcedureProvider option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        var strProviderIds = ProviderIds;
        Favorite_ProcedureDetail.providerCheckedIds = ProviderIds;
        if (Favorite_ProcedureDetail.providerCheckedIds != '') {
            Favorite_ProcedureDetail.validateProvider(2);
        } else {
            Favorite_ProcedureDetail.validateProvider(1);
        }
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
        if (Favorite_ProcedureDetail.params.TabID == 'clinicalTabProgressNote') {
            params['FromProgressNote'] = 'pnlClinicalProgressNote';
            params["ParentCtrl"] = 'Favorite_ProcedureDetail';
        }
        else
            params["ParentCtrl"] = 'Favorite_ProcedureDetail';

        if (ctrl != null) {
            params["RefCtrl"] = ctrl;
        }
        if (hiddenCtrl != null) {
            params["RefHiddenCtrl"] = hiddenCtrl;
        }
        if (controlToLoad != "") {
            if (Favorite_ProcedureDetail.params.TabID == 'clinicalTabProgressNote' && searchType == "ICD")
                LoadActionPan(controlToLoad, params, 'pnlClinicalProgressNote');
            else
                LoadActionPan(controlToLoad, params);
        }

    },

    //Author: Abid Ali
    //Date: 23-03-2016
    //Validate Procedure  Fav List
    ValidateFavProcedure: function () {
        var self = $('#' + Favorite_ProcedureDetail.params.PanelID + ' #frmFavoriteProcedureDetail');
          self.bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  EntityId: {
                      enable: false,
                      group: '.col-sm-10',
                      validators: {
                          notEmpty: {
                              message: 'Specify a Entity for the Favorite Procedure and try again. '
                          },
                      }
                  },
                  FavoriteListName: {
                      group: '.col-sm-10',
                      validators: {
                          notEmpty: {
                              message: 'Specify a Name for the Favorite Procedure and try again. '
                          },
                      }
                  },
                  Procedure: {
                      group: '.col-sm-10',
                      validators: {
                          notEmpty: {
                              message: ''
                          }
                      }
                  },

              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           if (Favorite_ProcedureDetail.CPTData.length > 0) {
               Favorite_ProcedureDetail.FavProcedureSave();
           }
           else
               self.data('bootstrapValidator').enableFieldValidators('Procedure', true);

       })
        .on('error.form.bv', function (e) {
            e.preventDefault();
            if (Favorite_ProcedureDetail.CPTData.length > 0 && self.find('#txtFavoriteListName').val() != "") {
                //Disable validator on procedure
                self.data('bootstrapValidator').enableFieldValidators('Procedure', false);
                self.trigger('success.form.bv');
            }
            else {
                //Disable validator if cpt has data
                if (Favorite_ProcedureDetail.CPTData.length > 0)
                    self.data('bootstrapValidator').enableFieldValidators('Procedure', false);
            }

        });
    },


    //Author: Abid Ali
    //Date: 23-03-2016
    //Search procedure  Detail
    favoriteListSearch: function (FavoriteListId, ListType, PageNo, rpp) {

        Favorite_Procedure.searchFavoriteList_DBCall(ListType, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                var self = $('#' + Favorite_ProcedureDetail.params.PanelID + ' #frmFavoriteProcedureDetail');
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
                        $('#' + Favorite_ProcedureDetail.params.PanelID + " #ddlProcedureProvider").val(FavoriteListProvidersJSON);

                        $('#' + Favorite_ProcedureDetail.params.PanelID + " #ddlProcedureProvider").multiselect("refresh");
                        $('#' + Favorite_ProcedureDetail.params.PanelID + " #ddlProcedureProvider").multiselect({
                            includeSelectAllOption: true,
                            enableFiltering: true,
                            enableCaseInsensitiveFiltering: true,
                            maxHeight: 247
                        });
                        Favorite_ProcedureDetail.providerCheckedIds = FavoriteListProvidersJSON;
                    } else {
                        $('#' + Favorite_ProcedureDetail.params.PanelID + " #ddlProcedureProvider").find("option:selected").removeAttr("selected");
                        $('#' + Favorite_ProcedureDetail.params.PanelID + " #ddlProcedureProvider").multiselect("refresh");
                    }
                    Favorite_ProcedureDetail.favoriteList_CPTSearch(JSON.parse(response.FavoriteListJSON)[0].FavoriteListId);
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    //Author: Abid Ali
    //Date: 23-03-2016
    //Search procedure  CPT 
    favoriteList_CPTSearch: function (FavoriteListId, FavoriteListCPTId, PageNo, rpp) {

        Favorite_Procedure.searchFavoriteList_CPT_DBCall(FavoriteListCPTId, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                //Bind to list
                $.each(JSON.parse(response.FavoriteListCPTJSON), function (index, CPTRow) {
                    Favorite_ProcedureDetail.bindToCPTList(CPTRow.CPTCode, CPTRow.CPTCodeDescription, CPTRow.SNOMEDID, CPTRow.SNOMED_DESCRIPTION, CPTRow.Unit, CPTRow.Modifier, CPTRow.FavoriteListCPTId);                    
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },


    UnLoadTab: function () {
        Favorite_ProcedureDetail.CPTData = [];

        var objDeffered = $.Deferred();
        if (Favorite_ProcedureDetail.params["FromAdmin"] == "0") {
            if (Favorite_ProcedureDetail.params != null && Favorite_ProcedureDetail.params.ParentCtrl != null) {
                UnloadActionPan(Favorite_ProcedureDetail.params.ParentCtrl, 'Favorite_ProcedureDetail');
            }
            else
                UnloadActionPan(null, 'Favorite_ProcedureDetail');
        }
        else {
            RemoveAdminTab();
        }
        objDeffered.resolve();
        return objDeffered;
    },

    //Author: Abid Ali
    //Date: 23-03-2016
    //Logic implimented to bind CPT code to Procedure field of Proceudre  Detail
    bindAutoComplete: function (element) {

        var hiddenCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "Favorite_ProcedureDetail", null, true);



    },

    //Author: Abid Ali
    //Date: 23-03-2016
    //Logic implimented to bind CPT code to Procedure field of Proceudre  Detail
    bindToCPTList: function (cptCode, cptDescription, SnomedID, SnomedDescription, unit, modifier, FavoriteListCPTId) {
        cptDescription = utility.decodeHtml(cptDescription);

        if (!Favorite_ProcedureDetail.isInCPTList(cptCode, cptDescription)) {
            unit = unit == null ? '' : unit;
            modifier = modifier == null ? '' : modifier;
         
            var item = {};
            item["CPTCode"] = cptCode;
            item["CPTCodeDescription"] = cptDescription;
            item["SNOMEDID"] = SnomedID;
            item["SNOMED_DESCRIPTION"] = SnomedDescription;
            item["Unit"] = unit;
            item["Modifier"] = modifier;
            item["FavoriteListCPTId"] = FavoriteListCPTId;
            Favorite_ProcedureDetail.CPTData.push(item);

            var $list = $("#" + Favorite_ProcedureDetail.params.PanelID + " #ulFavProcedureDisease");
            cptDescArg = cptDescription;

            if (cptDescArg != "") {
                cptDescArg = ",'" + cptDescArg.trim() + "'";
            }

            var currId = -1;
            $($list).find('li').each(function (i, item) {
                currId = $(this).attr("id");
            });

            currId = parseInt(currId) + (-1);

            var onclick = "Favorite_ProcedureDetail.deleteCPTFromCPTData(this,'" + cptCode + "','" + cptDescription + "','" + FavoriteListCPTId + "');";
            var modifierCtrlId = 'txtModifier' + currId;
            var unitCtrlId = 'txtUnit' + currId;
            var AutoCompleteSearch = "utility.BindAutoCompleteText(this, 'COMMON_CODE', 'GET_MODIFIER_CODE', '#" + modifierCtrlId + "', '',false,0);";
            var onBlur = "utility.ValidateCode(this, 'Modifier','" + modifierCtrlId + "');";

            var li = '<li id=' + currId + ' value =' + cptCode + ' cptdesc="' + cptDescription + '">' +
           '<span class="pull-left pr-xlg">' + (cptCode != "" ? cptCode + " - " : "") + cptDescription + ' </span>' +
           '<div class="clearfix"></div><div class="row"><div class="col-xs-1">Unit</div><div class="col-xs-3">' +
           '<input id="' + unitCtrlId + '" name="Unit" type="text" class="form-control input-block p-tiny ml-xs" value="' + unit + '"></div>' +
           '<div class="col-xs-2">Modifier</div><div class="col-xs-6">' +
           '<input id="' + modifierCtrlId + '" name="Modifier" type="text" oninput="' + AutoCompleteSearch + '" onblur="' + onBlur + '" class="form-control input-block p-tiny ml-xs" value="' + modifier + '"></div></div>' +
                      '<span class="removeIconListHover">' +
                      '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="' + onclick + '"><i class="fa fa-times red"></i></a>' +
                      '</span>' +
                      '<div class="clearfix"></div>' +
                      '</li>';

            $list.append(li);
        }
        setTimeout(function () {
            $("#" + Favorite_ProcedureDetail.params.PanelID + ' #frmFavoriteProcedureDetail #txtDiagnosis').val(cptDescription)
            var bootstrapValidator = $("#" + Favorite_ProcedureDetail.params.PanelID + ' #frmFavoriteProcedureDetail').data('bootstrapValidator');
             bootstrapValidator.revalidateField("Procedure");
            $("#" + Favorite_ProcedureDetail.params.PanelID + " #txtDiagnosis").val('');
            $('input[id*=txtUnit]').on("input", function () {
                var val = Math.abs(parseInt(this.value, 10) || 1);
                this.value = val > 999 ? 999 : val;
            });
        }, 200);

        $("#" + Favorite_ProcedureDetail.params.PanelID + " #ulFavProcedureDisease").on('keydown', '#' + unitCtrlId, function (e) { -1 !== $.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) || /65|67|86|88/.test(e.keyCode) && (!0 === e.ctrlKey || !0 === e.metaKey) || 35 <= e.keyCode && 40 >= e.keyCode || (e.shiftKey || 48 > e.keyCode || 57 < e.keyCode) && (96 > e.keyCode || 105 < e.keyCode) && e.preventDefault() });
        $($list).find('input[id*="txtUnit"]').bind("paste", function (e) {
            e.preventDefault();
        });
    },

    //Author: Abid Ali
    //Date: 23-03-2016
    //Delete CPT from CPTData Json Object
    deleteCPTFromCPTData: function (obj, cptCode, cptDescription, FavoriteListCPTId) {

        cptDescription = typeof cptDescription == "undefined" ? "" : cptDescription.toString();
        $.each(Favorite_ProcedureDetail.CPTData, function (index, item) {
            if (item["CPTCode"] == cptCode.toString() && utility.decodeHtml(item["CPTCodeDescription"]) == cptDescription) {

                Favorite_ProcedureDetail.CPTData.splice(index, 1);
                $(obj).parent().parent().remove();
                if (Favorite_ProcedureDetail.params.FavoriteListId >0  &&   FavoriteListCPTId) {
                    Favorite_ProcedureDetail.deleteCPT_DB_Call(Favorite_ProcedureDetail.params.FavoriteListId, FavoriteListCPTId);
                }              
                return false;
            }
        });
    },

    //Author: Abid Ali
    //Date: 23-03-2016
    //Finds cpt code and discription in CPTData array
    isInCPTList: function (cptCode, cptDescription) {
        var isExists = false;
        $.each(Favorite_ProcedureDetail.CPTData, function (index, item) {

            if (item["CPTCode"] == cptCode && utility.decodeHtml(item["CPTCodeDescription"]) == cptDescription) {
                isExists = true;
                return false;
            }
        });
        return isExists;
    },


    BindICD9AutoComplete: function (element) {
        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "Favorite_ProcedureDetail", null, false);
    },

    deleteCPT_DB_Call: function (FavouriteListId, FavoriteListCPTId) {
        var objData = {};
        objData["FavoriteListId"] = FavouriteListId == null ? 0 : FavouriteListId;
        objData["FavoriteListCPTId"] = FavoriteListCPTId == null ? 0 : FavoriteListCPTId;
        objData["commandType"] = "delete_favoritelist_cpt";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },
    //Start//22/03/2016//M Ahmad Imran//Implimented Save Fav Complaint Detail
    FavProcedureSave: function () {
        if (globalAppdata['AppUserName'] == DefaultUser) {
            $("#" + Favorite_ProcedureDetail.params.PanelID + " #frmFavoriteProcedureDetail #lstEntityId").enable = true;
            $("#" + Favorite_ProcedureDetail.params.PanelID + " #frmFavoriteProcedureDetail").bootstrapValidator('revalidateField', 'EntityId');
        }
        if (Favorite_ProcedureDetail.CPTData.length > 0) {
            //var myJSON = JSON.stringify(Favorite_ProcedureDetail.FavProcedure);
            Favorite_ProcedureDetail.getUnitsAndModifiers();
            var strMessage = "";
            AppPrivileges.GetFormPrivileges("Favorites_Procedure", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Favorite_ProcedureDetail.SaveFavProcedure().done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Favorite_ProcedureDetail.UnLoadTab();
                            var self = $('#' + Favorite_ProcedureDetail.params.PanelID + ' #frmFavoriteProcedureDetail');
                            self.find('#hfFavoriteListId').val(response.FavCPTId);
                            Favorite_Procedure.favoriteListSearch();

                            utility.DisplayMessages(response.Message, 1);


                        }
                        else {

                            utility.DisplayMessages(response.Message, 3);

                        }
                    });
                }
                else {
                    utility.DisplayMessages(strMessage, 2);
                }
            });
        }
        else {
            utility.DisplayMessages("There is no Favorite Complaint to add", 3);/// ask from babur bhai
        }




    },
    //End M Ahmad Imran 10/2/2016

    //Start//22/03/2016//M Ahmad Imran//For record Complaint
    AddInArray: function (id, CPTCode, CPTDescription, IsSelect) {
        var item = {};
        item["DiagnosisId"] = id;
        item["CPTCode"] = CPTCode;
        item["CPTCodeDescription"] = CPTDescription;
        Favorite_ProcedureDetail.FavProcedure.push(item);
        if (IsSelect) {
            //Favorite_ProcedureDetail.fillChiefProcedure($('#' + Favorite_ProcedureDetail.params.PanelID + " #frmClinicalProcedure #ulCompliantDisease li#" + id + ""), event);
        }

    },
    //End M Ahmad Imran 22/03/2016

    //Start//22/03/2016//M Ahmad Imran//Implimented Call to Controller for Fav Save Complaint Detail
    SaveFavProcedure: function () {
        //FavoriteListIcd
        var objData = {};
        var self = $('#' + Favorite_ProcedureDetail.params.PanelID + ' #frmFavoriteProcedureDetail');
        favoriteListId = self.find('#hfFavoriteListId').val();

        var self = $('#pnlFavoriteProcedureDetail');
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objDetail = JSON.parse(myJSON);
        if (objDetail["ProviderIds"] != '') {
            Favorite_ProcedureDetail.validateProvider(2);
            objData["FavoriteListId"] = favoriteListId;
            objData["ListType"] = "Procedure";
            objData["EntityId"] = objDetail["EntityId"];
            objData["BodyPartId"] = objDetail["BodyPartId"];
            objData["IsActive"] = objDetail.Active;
            objData["FavoriteListName"] = objDetail["FavoriteListName"];
            objData["ProviderIds"] = objDetail["ProviderIds"];
            if (globalAppdata['AppUserName'] != DefaultUser) {
                objData["EntityId"] = -1;
            }
            objData["FavoriteListCPT"] = Favorite_ProcedureDetail.CPTData;
            objData["commandType"] = "SAVE_FavProcedureOrder";
            var data = JSON.stringify(objData);
            return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
        }
        else {
            Favorite_ProcedureDetail.validateProvider(1);
        }
    },
    //End M Ahmad Imran 22/03/2016

    getUnitsAndModifiers: function () {
        var $list = $("#" + Favorite_ProcedureDetail.params.PanelID + " #ulFavProcedureDisease li");
        for (var i = 0; i < Favorite_ProcedureDetail.CPTData.length; i++) {
            for (var j = 0; j < $list.length; j++) {
                var cptCode = $($list[j]).attr('value');
                var desc = $($list[j]).attr('cptdesc');
                if (cptCode && Favorite_ProcedureDetail.CPTData[i].CPTCode == cptCode && Favorite_ProcedureDetail.CPTData[i].CPTCodeDescription == desc) {
                    Favorite_ProcedureDetail.CPTData[i].Unit = $($list[j]).find("input[id^='txtUnit']").val();
                    Favorite_ProcedureDetail.CPTData[i].Modifier = $($list[j]).find("input[id^='txtModifier']").val();
                    break;
                }
            }
        }
    },
}