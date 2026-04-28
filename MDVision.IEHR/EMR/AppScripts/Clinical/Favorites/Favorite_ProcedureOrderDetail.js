Favorite_ProcedureOrderDetail = {
    bIsFirstLoad: true,
    params: [],
    Switch: 1,
    CPTData: [],
    FavProcedureOrder: [],
    providerCheckedIds: [],
    ProviderIds: '',
    Load: function (params) {
        Favorite_ProcedureOrderDetail.params = params;
        if (Favorite_ProcedureOrderDetail.params.PanelID != 'pnlFavoriteProcedureOrderDetail') {
            Favorite_ProcedureOrderDetail.params.PanelID = Favorite_ProcedureOrderDetail.params.PanelID + ' #pnlFavoriteProcedureOrderDetail';
        } else {
            Favorite_ProcedureOrderDetail.params.PanelID = 'pnlFavoriteProcedureOrderDetail';
        }
        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Favorite_ProcedureOrderDetail.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        if (globalAppdata['AppUserName'] == DefaultUser) {
            $('#' + Favorite_ProcedureOrderDetail.params.PanelID + ' #frmFavoriteProcedureOrderDetail #divEntity').show();
        }
        if (Favorite_ProcedureOrderDetail.params.mode == "Edit")
            $('#' + Favorite_ProcedureOrderDetail.params.PanelID + ' #lblHeading').html('Edit Favorites List');

        var self = $('#' + Favorite_ProcedureOrderDetail.params.PanelID);
        self.loadDropDowns(true).done(function () {
            selectedEntity = globalAppdata["SeletedEntityId"];
            $.when(Favorite_ProcedureOrderDetail.loadEntityProvider(selectedEntity)).then(function () {
                var favListId = Favorite_ProcedureOrderDetail.params.FavoriteListId;
                if (favListId != null) {

                    // set FavListId to hidden field
                    self.find('#hfFavoriteListId').val(favListId);
                    //Load Detail against FavListId
                    if (favListId > 0) {
                        Favorite_ProcedureOrderDetail.favoriteListSearch(favListId);
                    }
                }
            });
            if (Add_Letter_Template.params.mode == "Add") {


            }
            else if (Add_Letter_Template.params.mode == "Edit") {


            }
            //Favorite_ProcedureOrderDetail.ValidateFavComplaint();

        });


        //Start//30/03/2016//Abid Ali//FavList edit functionality
        Favorite_ProcedureOrderDetail.validateFavoriteList();
        //var favListId = Favorite_ProcedureOrderDetail.params.FavoriteListId
        //if (favListId != null) {

        //    // set FavListId to hidden field
        //    self.find('#hfFavoriteListId').val(favListId);
        //    //Load Detail against FavListId
        //    if (favListId > 0) {
        //        Favorite_ProcedureOrderDetail.favoriteListSearch(favListId);
        //    }
        //}
        //End//30/03/2016//Abid Ali//FavList edit functionality

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
        if (Favorite_ProcedureOrderDetail.params.TabID == 'clinicalTabProgressNote') {
            params['FromProgressNote'] = 'pnlClinicalProgressNote';
            params["ParentCtrl"] = 'Favorite_ProcedureOrderDetail';
        }
        else
            params["ParentCtrl"] = 'Favorite_ProcedureOrderDetail';

        if (ctrl != null) {
            params["RefCtrl"] = ctrl;
        }
        if (hiddenCtrl != null) {
            params["RefHiddenCtrl"] = hiddenCtrl;
        }
        if (controlToLoad != "") {
            if (Favorite_ProcedureOrderDetail.params.TabID == 'clinicalTabProgressNote' && searchType == "ICD")
                LoadActionPan(controlToLoad, params, 'pnlClinicalProgressNote');
            else
                LoadActionPan(controlToLoad, params);
        }

    },
    //Author: Abid Ali
    //Date: 23-03-2016
    //Validate Procedure Order Fav List
    validateFavoriteList: function () {
        var self = $('#' + Favorite_ProcedureOrderDetail.params.PanelID + ' #frmFavoriteProcedureOrderDetail');

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
            if (Favorite_ProcedureOrderDetail.CPTData.length > 0) {
                //Allow to be submited
                Favorite_ProcedureOrderDetail.FavProcedureOrderSave();
            }
            else
                self.data('bootstrapValidator').enableFieldValidators('Diagnosis', true);
        })
            .on('error.form.bv', function (e) {
                e.preventDefault();

                if (Favorite_ProcedureOrderDetail.CPTData.length > 0 && self.find('#txtFavoriteListName').val() != "") {

                    //Disable validator on procedure
                    self.data('bootstrapValidator').enableFieldValidators('Diagnosis', false);
                    self.trigger('success.form.bv');
                }
                else {
                    //Disable validator if cpt has data
                    if (Favorite_ProcedureOrderDetail.CPTData.length > 0)
                        self.data('bootstrapValidator').enableFieldValidators('Diagnosis', false);
                }

            });       
    },


    //Author: Abid Ali
    //Date: 23-03-2016
    //Search procedure Order Detail
    favoriteListSearch: function (FavoriteListId, ListType, PageNo, rpp) {

        Favorite_ProcedureOrder.searchFavoriteList_DBCall(ListType, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                var self = $('#' + Favorite_ProcedureOrderDetail.params.PanelID + ' #frmFavoriteProcedureOrderDetail');
                var favListDetail = JSON.parse(response.FavoriteListJSON)[0];
                //Bind to form
                utility.bindMyJSONByName(true, favListDetail, false, self).done(function () {
                    var isActive = false;
                    if (favListDetail.IsActive === "True")
                        isActive = true;
                    self.find('#Active').attr('checked', isActive);
                    self.find('#txtFavoriteListName').val(favListDetail.Name);

                    var FavoriteListProvidersJSON = JSON.parse(response.FavoriteListProviders_JSON);
                    if (FavoriteListProvidersJSON != '' && FavoriteListProvidersJSON) {
                        $('#' + Favorite_ProcedureOrderDetail.params.PanelID + " #ddlProcedureOrderProvider").val(FavoriteListProvidersJSON);

                        $('#' + Favorite_ProcedureOrderDetail.params.PanelID + " #ddlProcedureOrderProvider").multiselect("refresh");
                        $('#' + Favorite_ProcedureOrderDetail.params.PanelID + " #ddlProcedureOrderProvider").multiselect({
                            includeSelectAllOption: true,
                            enableFiltering: true,
                            enableCaseInsensitiveFiltering: true,
                            maxHeight: 247
                        });
                        Favorite_ProcedureOrderDetail.providerCheckedIds = FavoriteListProvidersJSON;                        
                    } else {
                        $('#' + Favorite_ProcedureOrderDetail.params.PanelID + " #ddlProcedureOrderProvider").find("option:selected").removeAttr("selected");
                        $('#' + Favorite_ProcedureOrderDetail.params.PanelID + " #ddlProcedureOrderProvider").multiselect("refresh");
                    }

                    //Load CPT against favList
                    Favorite_ProcedureOrderDetail.favoriteList_CPTSearch(JSON.parse(response.FavoriteListJSON)[0].FavoriteListId);
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });     

    },

    //Author: Abid Ali
    //Date: 23-03-2016
    //Search procedure Order CPT 
    favoriteList_CPTSearch: function (FavoriteListId, FavoriteListCPTId, PageNo, rpp) {

        Favorite_ProcedureOrder.searchFavoriteList_CPT_DBCall(FavoriteListCPTId, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                //Bind to list
                $.each(JSON.parse(response.FavoriteListCPTJSON), function (index, CPTRow) {
                    Favorite_ProcedureOrderDetail.bindToCPTList(CPTRow.CPTCode, CPTRow.CPTCodeDescription, CPTRow.FavoriteListCPTId);

                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },


    UnLoadTab: function () {
        Favorite_ProcedureOrderDetail.CPTData = [];

        var objDeffered = $.Deferred();
        if (Favorite_ProcedureOrderDetail.params["FromAdmin"] == "0") {
            if (Favorite_ProcedureOrderDetail.params != null && Favorite_ProcedureOrderDetail.params.ParentCtrl != null) {
                UnloadActionPan(Favorite_ProcedureOrderDetail.params.ParentCtrl, 'Favorite_ProcedureOrderDetail');
            }
            else
                UnloadActionPan(null, 'Favorite_ProcedureOrderDetail');
        }
        else {
            RemoveAdminTab();
        }
        objDeffered.resolve();
        return objDeffered;
    },

    //Author: Abid Ali
    //Date: 23-03-2016
    //Logic implimented to bind CPT code to Procedure field of Proceudre Order Detail
    bindAutoComplete: function (element) {

        var hiddenCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "Favorite_ProcedureOrderDetail", null, true);

    },

    //Author: Abid Ali
    //Date: 23-03-2016
    //Logic implimented to bind CPT code to Procedure field of Proceudre Order Detail
    bindToCPTList: function (cptCode, cptDescription, cptFavoriteListCPTId) {
        cptDescription = utility.decodeHtml(cptDescription);

        if (!Favorite_ProcedureOrderDetail.isInCPTList(cptCode, cptDescription)) {
            var item = {};
            item["CPTCode"] = cptCode;
            item["CPTCodeDescription"] = cptDescription;
            item["FavoriteListCPTId"] = cptFavoriteListCPTId;
            Favorite_ProcedureOrderDetail.CPTData.push(item);

            var $list = $("#" + Favorite_ProcedureOrderDetail.params.PanelID + " #ulFavProcedureOrderDisease");
            cptDescArg = cptDescription;

            if (cptDescArg != "") {
                cptDescArg = ",'" + cptDescArg.trim() + "'";
            }          
            var onclick = "Favorite_ProcedureOrderDetail.deleteCPTFromCPTData(this,'" + cptCode + "','" + cptDescription + "');";
            var li = '<li value =' + cptCode + '>' +
            '<span class="pull-left pr-xlg">' + cptCode + " - " + cptDescription + ' </span>' +
                       '<span class="removeIconListHover">' +
                       '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="' + onclick + '"><i class="fa fa-times red"></i></a>' +
                       '</span>' +
                       '<div class="clearfix"></div>' +
                       '</li>';


            $list.append(li);
            setTimeout(function () {
                $("#" + Favorite_ProcedureOrderDetail.params.PanelID + " #txtDiagnosis").val(cptDescription)
                var bootstrapValidator = $("#" + Favorite_ProcedureOrderDetail.params.PanelID + ' #frmFavoriteProcedureOrderDetail').data('bootstrapValidator');
                bootstrapValidator.revalidateField("Diagnosis");
                $("#" + Favorite_ProcedureOrderDetail.params.PanelID + " #txtDiagnosis").val('');
            }, 200);
            
        }
        else {
            //AST-380 BY:MAHMAD
            $("#" + Favorite_ProcedureOrderDetail.params.PanelID + " #txtDiagnosis").val('');
            utility.DisplayMessages('Procedure already added', 2);
            //AST-380 BY:MAHMAD
        }
    },

    //Author: Abid Ali
    //Date: 23-03-2016
    //Delete CPT from CPTData Json Object
    deleteCPTFromCPTData: function (obj, cptCode, cptDescription) {

        cptDescription = typeof cptDescription == "undefined" ? "" : cptDescription.toString();
        $.each(Favorite_ProcedureOrderDetail.CPTData, function (index, item) {
            if (item["CPTCode"] == cptCode.toString() && item["CPTCodeDescription"] == cptDescription) {
                var FavoriteListCPTId = item["FavoriteListCPTId"];
                Favorite_ProcedureOrderDetail.CPTData.splice(index, 1);
                $(obj).parent().parent().remove();
                if (Favorite_ProcedureOrderDetail.params.FavoriteListId) {
                    Favorite_ProcedureOrderDetail.deleteCPT_DB_Call(Favorite_ProcedureOrderDetail.params.FavoriteListId, FavoriteListCPTId);
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
        $.each(Favorite_ProcedureOrderDetail.CPTData, function (index, item) {

            if (item["CPTCode"] == cptCode && item["CPTCodeDescription"] == cptDescription) {
                isExists = true;
                return false;
            }
        });
        return isExists;
    },

    deleteCPT_DB_Call: function (FavouriteListId, FavoriteListCPTId) {
        var objData = {};
        objData["FavoriteListId"] = FavouriteListId == null ? 0 : FavouriteListId;
        objData["FavoriteListCPTId"] = FavoriteListCPTId == null ? 0 : FavoriteListCPTId;
        objData["commandType"] = "delete_favoritelist_cpt";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },

    BindICD9AutoComplete: function (element) {
        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "Favorite_ProcedureOrderDetail", null, false);
    },


    //Start//22/03/2016//M Ahmad Imran//Implimented Save Fav Complaint Detail
    FavProcedureOrderSave: function (ComeFromDelete, ComeFromUnLoad) {
  //      $("#" + Favorite_ProcedureOrderDetail.params.PanelID + " #frmFavoriteProcedureOrderDetail").bootstrapValidator('revalidateField', 'FavoriteListName');
   //     $("#" + Favorite_ProcedureOrderDetail.params.PanelID + " #frmFavoriteProcedureOrderDetail").bootstrapValidator('revalidateField', 'FavoriteListName');
        if (Favorite_ProcedureOrderDetail.CPTData.length > 0) {
            //var myJSON = JSON.stringify(Favorite_ProcedureOrderDetail.FavProcedureOrder);
            var strMessage = "";
            AppPrivileges.GetFormPrivileges("Favorites_ProcedureOrder", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Favorite_ProcedureOrderDetail.SaveFavComplaint().done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Favorite_ProcedureOrderDetail.UnLoadTab();
                            var self = $('#' + Favorite_ProcedureOrderDetail.params.PanelID + ' #frmFavoriteProcedureOrderDetail');
                             self.find('#hfFavoriteListId').val(response.FavCPTId);
                            Favorite_ProcedureOrder.favoriteListSearch();
                            //Favorite_ProcedureOrderDetail.loadCiefComplaintComponent(function () {
                            //    // Favorite_ProcedureOrderDetail.getLatestComplaintByPatientId();
                            //    Favorite_ProcedureOrderDetail.createComplaintBodyHTML(OverAllComments, CopyComplainDetail, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML');
                            //});
                            if (!ComeFromDelete) {
                                utility.DisplayMessages(response.Message, 1);
                            }

                        }
                        else {
                            if (!ComeFromDelete) {
                                utility.DisplayMessages(response.Message, 3);
                            }
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
        Favorite_ProcedureOrderDetail.FavProcedureOrder.push(item);
        if (IsSelect) {
            //Favorite_ProcedureOrderDetail.fillChiefProcedureOrder($('#' + Favorite_ProcedureOrderDetail.params.PanelID + " #frmClinicalProcedureOrder #ulCompliantDisease li#" + id + ""), event);
        }

    },
    //End M Ahmad Imran 22/03/2016

    //Start//22/03/2016//M Ahmad Imran//Implimented Call to Controller for Fav Save Complaint Detail
    SaveFavComplaint: function () {
        //FavoriteListIcd
        var objData = {};
        var self = $('#' + Favorite_ProcedureOrderDetail.params.PanelID + ' #frmFavoriteProcedureOrderDetail');
        favoriteListId = self.find('#hfFavoriteListId').val();

        var self = $('#pnlFavoriteProcedureOrderDetail');
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objDetail = JSON.parse(myJSON);

        if (objDetail["ProviderIds"] != '') {
            Favorite_ProcedureOrderDetail.validateProvider(2);
            objData["FavoriteListId"] = favoriteListId;
            objData["ListType"] = "ProcedureOrder";
            objData["EntityId"] = objDetail["EntityId"];
            objData["IsActive"] = objDetail.Active;
            objData["FavoriteListName"] = objDetail["FavoriteListName"];
            if (globalAppdata['AppUserName'] != DefaultUser) {
                objData["EntityId"] = -1;
            }
            objData["ProviderIds"] = objDetail["ProviderIds"];
            objData["FavoriteListCPT"] = Favorite_ProcedureOrderDetail.CPTData;
            objData["commandType"] = "SAVE_FavProcedureOrder";
            var data = JSON.stringify(objData);
            return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
        }
        else {
            Favorite_ProcedureOrderDetail.validateProvider(1);
        }
    },
    //End M Ahmad Imran 22/03/2016

    loadEntityProvider: function (entityId) {
        var objDeffered = $.Deferred();
        var data = "entityID=" + entityId;
        if (entityId != null && entityId > 0) {

            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $('#' + Favorite_ProcedureOrderDetail.params.PanelID + ' #ddlProcedureOrderProvider');
                var $providerHiddenDdl = $('#' + Favorite_ProcedureOrderDetail.params.PanelID + ' #ddlHiddenFavProcedureOrderProvider');

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
                if (Favorite_ProcedureOrderDetail.ProviderIds != '') {
                    var Providers = Favorite_ProcedureOrderDetail.ProviderIds.split(",");
                    Favorite_ProcedureOrderDetail.providerCheckedIds = Providers;
                    $('#' + Favorite_ProcedureOrderDetail.params.PanelID + ' #ddlProcedureOrderProvider').val(Providers);
                }

            }).then(function () {
                //Intialized in onhidden spacialty ddl.
                Favorite_ProcedureOrderDetail.IntializeMultiSelectDropDownProviders();
                objDeffered.resolve();

            });
        }
        else {
            objDeffered.resolve();
        }
        return objDeffered;
    },

    IntializeMultiSelectDropDownProviders: function () {
        $('#' + Favorite_ProcedureOrderDetail.params.PanelID + ' #ddlProcedureOrderProvider').multiselect('destroy');
        $('#' + Favorite_ProcedureOrderDetail.params.PanelID + ' #ddlProcedureOrderProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 200,
            nonSelectedText: 'Select',
            selectAll: false,
            onDropdownHide: function (event) {
            },
            onChange: function () {
                Favorite_ProcedureOrderDetail.CheckProviderValidation();
            },
        });
    },

    CheckProviderValidation: function () {
        var self = $("#" + Favorite_ProcedureOrderDetail.params.PanelID);
        var ProviderIds = self.find('#ddlProcedureOrderProvider option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        var strProviderIds = ProviderIds;
        Favorite_ProcedureOrderDetail.providerCheckedIds = ProviderIds;
        if (Favorite_ProcedureOrderDetail.providerCheckedIds != '') {
            Favorite_ProcedureOrderDetail.validateProvider(2);
        } else {
            Favorite_ProcedureOrderDetail.validateProvider(1);
        }
    },

    validateProvider: function (operationid) {
        var self = "#" + Favorite_ProcedureOrderDetail.params.PanelID + " #frmFavoriteProcedureOrderDetail";
        $(self + " #divFavProcedureOrderProvider").find("i").remove();
        if (operationid == 1) {
            $(self + " .multiselect").css("border-color", "#cc2724");
            $(self + " #divFavProcedureOrderProvider").find(".control-label").css("color", "#cc2724");
            $(self + " #divFavProcedureOrderProvider").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-remove" data-bv-icon-for="Race" style="display: block;"></i>');
        } else if (operationid == 2) {
            $(self + " .multiselect").css("border-color", "#3c763d");
            $(self + " #divFavProcedureOrderProvider").find(".control-label").css("color", "#3c763d");
            $(self + " #divFavProcedureOrderProvider").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-ok" data-bv-icon-for="Race" style="display: block;"></i>');
        } else {
            $(self + " .multiselect").css("border-color", "#ccc");
            $(self + " #divFavProcedureOrderProvider").find(".control-label").css("color", "#000000");
        }
    },
}