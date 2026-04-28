Favorite_RadiologyOrderDetail = {
    bIsFirstLoad: true,
    params: [],
    Switch: 1,
    CPTData: [],
    providerCheckedIds: [],
    ProviderIds: '',
    FavRadiologyOrder: [],
    Load: function (params) {
        Favorite_RadiologyOrderDetail.params = params;
        if (Favorite_RadiologyOrderDetail.params.PanelID != 'pnlFavoriteRadiologyOrderDetail') {
            Favorite_RadiologyOrderDetail.params.PanelID = Favorite_RadiologyOrderDetail.params.PanelID + ' #pnlFavoriteRadiologyOrderDetail';
        } else {
            Favorite_RadiologyOrderDetail.params.PanelID = 'pnlFavoriteRadiologyOrderDetail';
        }
        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Favorite_RadiologyOrderDetail.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        if (globalAppdata['AppUserName'] == DefaultUser) {
            $('#' + Favorite_RadiologyOrderDetail.params.PanelID + ' #frmFavoriteRadiologyOrderDetail #divEntity').show();
        }
        var self = $('#' + Favorite_RadiologyOrderDetail.params.PanelID);
        self.loadDropDowns(true).done(function () {
            selectedEntity = globalAppdata["SeletedEntityId"];
            $.when(Favorite_RadiologyOrderDetail.loadEntityProvider(selectedEntity)).then(function () {
                Favorite_RadiologyOrderDetail.LoadLabs('ddlLabId', Favorite_RadiologyOrderDetail.params.PanelID).done(function () {
                    if (Favorite_RadiologyOrderDetail.params.mode == "Add") {
                    }
                    else if (Favorite_RadiologyOrderDetail.params.mode == "Edit") {
                        $('#' + Favorite_RadiologyOrderDetail.params.PanelID + ' #lblHeading').html('Edit Favorites List');
                        Favorite_RadiologyOrderDetail.fillFavoriteRadiologyOrder(Favorite_RadiologyOrderDetail.params.FavoriteRadiologyOrderId);
                    }
                });
            });
        });

        Favorite_RadiologyOrderDetail.validateRadiologyOrderDetail();

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
    LoadLabs: function (ddlId, controlPanelID) {

        return Clinical_Lab.searchLab(null, 0, 1, 5000).done(function (response) {
            //Populate Distinct Values in typeArray
            response = JSON.parse(response);
            var NameArray = new Array();
            var labIds = new Array();
            var codeSystem = new Array();
            var data = JSON.parse(response.ClinicalLab_JSON);
            $.each(data, function (i, row) {
                if (jQuery.inArray(row.Name, NameArray) === -1) {
                    if (row.Name.trim().toLowerCase() == "point of care" || row.Name.trim().toLowerCase() == "external lab" || row.Name.trim().toLowerCase() == "external facility") {
                        NameArray.push(row.Name);
                        labIds.push(row.LabId);
                        codeSystem.push(row.CodeSystemId); // Code System 1 means CPT , 2 Means Lab Codes
                    }
                }
            });
            var ddType = $('#' + controlPanelID + " #" + ddlId);
            ddType.empty();
            ddType.append($("<option />").val("").text('-Select-'));
            if (NameArray.length > 0) {
                $.each(NameArray, function (index, Name) {
                    ddType.append($("<option />").val(labIds[index]).text(Name).attr('CodeSystem', codeSystem[index]));
                });
            }
        });
    },

    loadEntityProvider: function (entityId) {
        var objDeffered = $.Deferred();
        var data = "entityID=" + entityId;
        if (entityId != null && entityId > 0) {

            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $('#' + Favorite_RadiologyOrderDetail.params.PanelID + ' #ddlFavRDOProvider');
                var $providerHiddenDdl = $('#' + Favorite_RadiologyOrderDetail.params.PanelID + ' #ddlHiddenFavRDOProvider');

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
                if (Favorite_RadiologyOrderDetail.ProviderIds != '') {
                    var Providers = Favorite_RadiologyOrderDetail.ProviderIds.split(",");
                    Favorite_RadiologyOrderDetail.providerCheckedIds = Providers;
                    $('#' + Favorite_RadiologyOrderDetail.params.PanelID + ' #ddlFavRDOProvider').val(Providers);
                }

            }).then(function () {
                //Intialized in onhidden spacialty ddl.
                Favorite_RadiologyOrderDetail.IntializeMultiSelectDropDownProviders();
                objDeffered.resolve();

            });
        }
        else {
            objDeffered.resolve();
        }
        return objDeffered;
    },

    IntializeMultiSelectDropDownProviders: function () {
        $('#' + Favorite_RadiologyOrderDetail.params.PanelID + ' #ddlFavRDOProvider').multiselect('destroy');
        $('#' + Favorite_RadiologyOrderDetail.params.PanelID + ' #ddlFavRDOProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 200,
            nonSelectedText: 'Select',
            selectAll: false,
            onDropdownHide: function (event) {
            },
            onChange: function () {
                Favorite_RadiologyOrderDetail.CheckProviderValidation();
            },
        });
    },
    pushLOINCAsCpt: function (JsonObj) {
        var observation = JsonObj["Observation"];
        var LOINCCOde = JsonObj['LOINICCODE'];
        var LOINCDescription = JsonObj['LOINICDescription'];
        var OrderTestLOINCId = JsonObj['OrderTestLOINCId'];
        Favorite_RadiologyOrderDetail.bindToCPTList(LOINCCOde, LOINCDescription, OrderTestLOINCId);
    },

    openSearchPopup: function (searchType, ctrl, hiddenCtrl) {
        var labId = $('#pnlFavoriteRadiologyOrderDetail #ddlLabId').val();
        if (labId != '') {
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
            if (Favorite_RadiologyOrderDetail.params.TabID == 'clinicalTabProgressNote') {
                params['FromProgressNote'] = 'pnlClinicalProgressNote';
                params["ParentCtrl"] = 'Favorite_RadiologyOrderDetail';
            }
            else
                params["ParentCtrl"] = 'Favorite_RadiologyOrderDetail';

            if (ctrl != null) {
                params["RefCtrl"] = ctrl;
            }
            if (hiddenCtrl != null) {
                params["RefHiddenCtrl"] = hiddenCtrl;
            }
            if (controlToLoad != "") {
                if (Favorite_RadiologyOrderDetail.params.TabID == 'clinicalTabProgressNote' && searchType == "ICD")
                    LoadActionPan(controlToLoad, params, 'pnlClinicalProgressNote');
                else
                    LoadActionPan(controlToLoad, params);
            }
        }
        else {
            utility.DisplayMessages("Please Select Lab First !", 2);

        }
       

    },
    //Author: Abid Ali
    //Date: 23-03-2016
    //Logic search Validate Radiology Order
    validateRadiologyOrderDetail: function () {
        var self = $("#" + Favorite_RadiologyOrderDetail.params.PanelID + " #frmFavoriteRadiologyOrderDetail");

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
                LabId: {
                    group: '.col-sm-4',
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
            if (Favorite_RadiologyOrderDetail.CPTData.length > 0) {
                //Allow to be submited
                Favorite_RadiologyOrderDetail.favRadiologyOrderSave();
            }
            else
                self.data('bootstrapValidator').enableFieldValidators('Diagnosis', true);
        })
            .on('error.form.bv', function (e) {
                e.preventDefault();

                if (Favorite_RadiologyOrderDetail.CPTData.length > 0 && self.find('#txtFavoriteListName').val() != "") {

                    //Disable validator on procedure
                    self.data('bootstrapValidator').enableFieldValidators('Diagnosis', false);
                    self.trigger('success.form.bv');
                }
                else {
                    //Disable validator if cpt has data
                    if (Favorite_RadiologyOrderDetail.CPTData.length > 0)
                        self.data('bootstrapValidator').enableFieldValidators('Diagnosis', false);
                }

            });
    },

    CheckProviderValidation: function () {
        var self = $("#" + Favorite_RadiologyOrderDetail.params.PanelID); 
        var ProviderIds = self.find('#ddlFavRDOProvider option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        var strProviderIds = ProviderIds;
        Favorite_RadiologyOrderDetail.providerCheckedIds = ProviderIds;
        if (Favorite_RadiologyOrderDetail.providerCheckedIds != '') {
            Favorite_RadiologyOrderDetail.validateProvider(2);
        } else {
            Favorite_RadiologyOrderDetail.validateProvider(1);
        }
    },
    validateProvider: function (operationid) {
        var self = "#" + Favorite_RadiologyOrderDetail.params.PanelID + " #frmFavoriteRadiologyOrderDetail";
        $(self + " #divFavRDOProvider").find("i").remove();
        if (operationid == 1) {
            $(self+ " .multiselect").css("border-color", "#cc2724");
            $(self + " #divFavRDOProvider").find(".control-label").css("color", "#cc2724");
            $(self + " #divFavRDOProvider").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-remove" data-bv-icon-for="Race" style="display: block;"></i>');
        } else if (operationid == 2) {
            $(self + " .multiselect").css("border-color", "#3c763d");
            $(self + " #divFavRDOProvider").find(".control-label").css("color", "#3c763d");
            $(self + " #divFavRDOProvider").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-ok" data-bv-icon-for="Race" style="display: block;"></i>');
        } else {
            $(self + " .multiselect").css("border-color", "#ccc");
            $(self + " #divFavRDOProvider").find(".control-label").css("color", "#000000");
        }
    },
    //Author: Abid Ali
    //Date: 23-03-2016
    //Logic implimented to bind CPT code to Procedure field of Radiology Order Detail
    openCPTCode: function () {
        var params = [];
        //params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Favorite_ConsultationOrderDetail";
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtCPTCode";
        params["ParentCtrlPanelID"] = Favorite_RadiologyOrderDetail.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, Favorite_RadiologyOrderDetail.params.PanelID);
    },

    //Author: Abid Ali
    //Date: 23-03-2016
    //Logic implimented to bind CPT code to Procedure field of Radiology Order Detail
    //bindAutoComplete: function (element) {

    //    var hiddenCrtl = $(element);
    //    utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "Favorite_RadiologyOrderDetail", null, true);

    //},

    //Author: Abid Ali
    //Date: 23-03-2016
    //Logic implimented to bind CPT code to Procedure field of Radiology Order Detail
    bindToCPTList: function (cptCode, cptDescription, OrderTestLOINCId, FavoriteListCPTId) {

        if (!Favorite_RadiologyOrderDetail.isInCPTList(cptCode, cptDescription)) {
            var item = {};
            item["CPTCode"] = cptCode;
            item["CPTCodeDescription"] = cptDescription;
            item["OrderTestLOINCId"] = OrderTestLOINCId;
            item["FavoriteListCPTId"] = FavoriteListCPTId ? FavoriteListCPTId : 0;
            Favorite_RadiologyOrderDetail.CPTData.push(item);

            var $list = $("#" + Favorite_RadiologyOrderDetail.params.PanelID + " #ulFavRadiologyOrderDisease");
            cptDescArg = cptDescription;

            if (cptDescArg != "") {
                cptDescArg = ",'" + cptDescArg.trim() + "'";
            }

            var CPTCodeLabel = "";
            if (cptCode != "")
                CPTCodeLabel = cptCode + " - ";

            var onclick = "Favorite_RadiologyOrderDetail.deleteCPTFromCPTData(this,'" + cptCode + "','" + OrderTestLOINCId + "','" + FavoriteListCPTId + "');";
            var li = '<li value =' + cptCode + '>' +
            '<span class="pull-left pr-xlg">' + CPTCodeLabel + cptDescription + ' </span>' +
                       '<span class="removeIconListHover">' +
                       '<a class="btn btn-link btn-xs p-none" href="javascript:void(0);" onclick="' + onclick + '"><i class="fa fa-times red"></i></a>' +
                       '</span>' +
                       '<div class="clearfix"></div>' +
                       '</li>';


            $list.append(li);

            Favorite_RadiologyOrderDetail.EnableDisableDropDownLaboratory();
        }
        else {
            utility.DisplayMessages("Test is already selected", 2);
        }

        //clear textbox value to search
        setTimeout(function () {

            $("#" + Favorite_RadiologyOrderDetail.params.PanelID + " #frmFavoriteRadiologyOrderDetail #txtDiagnosis").val("");

        }, 110);
    },

    deleteCPT_DB_Call: function (FavouriteListId, FavoriteListCPTId) {

        var objData = {};

        objData["FavoriteListId"] = FavouriteListId == null ? 0 : FavouriteListId;
        objData["FavoriteListCPTId"] = FavoriteListCPTId == null ? 0 : FavoriteListCPTId;
        objData["commandType"] = "delete_favoritelist_cpt";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },
    //Author: Abid Ali
    //Date: 23-03-2016
    //Delete CPT from CPTData Json Object
    deleteCPTFromCPTData: function (obj, cptCode, OrderTestLOINCId, FavoriteListCPTId) {
        
        OrderTestLOINCId = typeof OrderTestLOINCId == "undefined" ? "" : OrderTestLOINCId.toString();
        FavoriteListCPTId = typeof FavoriteListCPTId == "undefined" ? "" : FavoriteListCPTId.toString();
        $.each(Favorite_RadiologyOrderDetail.CPTData, function (index, item) {
            if (item["CPTCode"] == cptCode.toString() && (item["OrderTestLOINCId"] == OrderTestLOINCId || item["FavoriteListCPTId"] == FavoriteListCPTId)) {

                Favorite_RadiologyOrderDetail.CPTData.splice(index, 1);
                if (Favorite_RadiologyOrderDetail.params.FavoriteRadiologyOrderId && Favorite_RadiologyOrderDetail.params.FavoriteRadiologyOrderId > 0 && typeof FavoriteListCPTId != typeof undefined) {
                    Favorite_RadiologyOrderDetail.deleteCPT_DB_Call(Favorite_RadiologyOrderDetail.params.FavoriteRadiologyOrderId, FavoriteListCPTId);
                }
                $(obj).parent().parent().remove();
                return false;
            }
        });
        if ($("#" + Favorite_RadiologyOrderDetail.params.PanelID + " #ulFavRadiologyOrderDisease li").length > 0) {
            $('#' + Favorite_RadiologyOrderDetail.params.PanelID + ' #ddlLabId').addClass('disableAll');
        }
        else {
            $('#' + Favorite_RadiologyOrderDetail.params.PanelID + ' #ddlLabId').removeClass('disableAll');
        }
    },


    //Author: Abid Ali
    //Date: 23-03-2016
    //Finds cpt code and discription in CPTData array
    isInCPTList: function (cptCode, cptDescription) {
        var isExists = false;
        $.each(Favorite_RadiologyOrderDetail.CPTData, function (index, item) {

            if (item["CPTCode"] == cptCode && item["CPTCodeDescription"] == cptDescription) {
                isExists = true;
                return false;
            }
        });
        return isExists;
    },
    EnableDisableDropDownLaboratory: function () {
        var $ddlLab = $("#" +Favorite_RadiologyOrderDetail.params.PanelID + ' #ddlLabId');
        if ($ddlLab.val() != '') {
            $ddlLab.addClass('disableAll');
        }
    },
    UnLoadTab: function () {
        Favorite_RadiologyOrderDetail.CPTData = [];
        var objDeffered = $.Deferred();
        if (Favorite_RadiologyOrderDetail.params["FromAdmin"] == "0") {
            if (Favorite_RadiologyOrderDetail.params != null && Favorite_RadiologyOrderDetail.params.ParentCtrl != null) {
                UnloadActionPan(Favorite_RadiologyOrderDetail.params.ParentCtrl, 'Favorite_RadiologyOrderDetail');
            }
            else
                UnloadActionPan(null, 'Favorite_RadiologyOrderDetail');
        }
        else {
            RemoveAdminTab();
        }
        objDeffered.resolve();
        return objDeffered;
    },
    BindICD9AutoComplete: function (element) {
        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "Favorite_RadiologyOrderDetail", null, false);
    },

    favRadiologyOrderSave: function (ComeFromDelete, ComeFromUnLoad) {
        var strMessage = "";
        if (Favorite_RadiologyOrderDetail.CPTData.length > 0) {
            AppPrivileges.GetFormPrivileges("Favorites_Diagnostic Imaging Order", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Favorite_RadiologyOrderDetail.saveFavRadiology().done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Favorite_RadiologyOrderDetail.UnLoadTab();
                            var self = $('#' + Favorite_RadiologyOrderDetail.params.PanelID + ' #frmFavoriteRadiologyOrderDetail');
                            self.find('#hfFavoriteListId').val(response.FavCPTId);
                            Favorite_RadiologyOrder.favoriteListSearch();
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
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else {
            utility.DisplayMessages("There is no Favorite Radiology to add.", 3);
        }

    },
    //End M Ahmad Imran 10/2/2016

    //Start//22/03/2016//M Ahmad Imran//For record Complaint
    AddInArray: function (id, ICD9, ICD10, SNOMED, ICD10Des, IsSelect) {
        var item = {};
        item["DiagnosisId"] = id;
        item["ICD9"] = ICD9;
        item["ICD10"] = ICD10;
        item["ICD10Description"] = ICD10Des
        item["SNOMED"] = SNOMED;
        Favorite_RadiologyOrderDetail.FavRadiologyOrder.push(item);
        if (IsSelect) {
            //Favorite_RadiologyOrderDetail.fillChiefRadiologyOrder($('#' + Favorite_RadiologyOrderDetail.params.PanelID + " #frmClinicalRadiologyOrder #ulCompliantDisease li#" + id + ""), event);
        }

    },
    //End M Ahmad Imran 22/03/2016

    saveFavRadiology: function () {

        var objData = {};
        var self = $('#pnlFavoriteRadiologyOrderDetail');
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objDetail = JSON.parse(myJSON);
        if (objDetail["ProviderIds"] != '') {
            Favorite_RadiologyOrderDetail.validateProvider(2);
            objData["FavoriteListId"] = self.find('#hfFavoriteListId').val();;
            objData["ListType"] = "RadiologyOrder";
            objData["EntityId"] = objDetail["EntityId"];
            objData["BodyPartId"] = objDetail["BodyPartId"];
            objData["IsActive"] = objDetail.Active;
            objData["FavoriteListName"] = objDetail["FavoriteListName"];
            objData["ProviderIds"] = objDetail["ProviderIds"];
            objData["LabId"] = objDetail["LabId"];
            if (globalAppdata['AppUserName'] != DefaultUser) {
                objData["EntityId"] = -1;
            }
            objData["FavoriteListCPT"] = Favorite_RadiologyOrderDetail.CPTData;
            objData["commandType"] = "save_favprocedureorder";
            var data = JSON.stringify(objData);
            return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
        }
        else {
            Favorite_RadiologyOrderDetail.validateProvider(1);
        }
        
    },
    bindAutoComplete: function (element) {
        var labId = $('#pnlFavoriteRadiologyOrderDetail #ddlLabId').val();
        if (labId != '') {
            var CodeSystemType = $('#pnlFavoriteRadiologyOrderDetail #ddlLabId option:selected').attr('CodeSystem');
            EMRUtility.BindLOINCCodes(element, "Favorite_RadiologyOrderDetail", labId, "",CodeSystemType);
        }
        else {
            utility.DisplayMessages("Please Select Lab First !", 2);
        }
    },

    //Method Name: fillFavoriteRadiologyOrder
    // Author Name: Humaira Yousaf
    //Craeted Date: 24-03-2016
    //Description:  fills favorite radiology
    fillFavoriteRadiologyOrder: function (FavoriteListId) {
        Favorite_RadiologyOrderDetail.fillFavoriteRadiologyOrder_DBCall(FavoriteListId).done(function (response) {
            response = JSON.parse(response);
            //   console.log(response);
            if (response.status != false) {

                var self = $('#' + Favorite_RadiologyOrderDetail.params.PanelID + ' #frmFavoriteRadiologyOrderDetail');
                utility.bindMyJSONByName(true, JSON.parse(response.FavoriteListJSON)[0], false, self).done(function () {


                    if (JSON.parse(response.FavoriteListJSON)[0].IsActive == "False")
                        self.find('#Active').attr('checked', false);
                    else
                        self.find('#Active').attr('checked', true);
                   var FavoriteListProvidersJSON = JSON.parse(response.FavoriteListProviders_JSON);
                   if (FavoriteListProvidersJSON != '' && FavoriteListProvidersJSON ) {
                       $('#' + Favorite_RadiologyOrderDetail.params.PanelID + " #ddlFavRDOProvider").val(FavoriteListProvidersJSON);

                       $('#' + Favorite_RadiologyOrderDetail.params.PanelID + " #ddlFavRDOProvider").multiselect("refresh");
                       $('#' + Favorite_RadiologyOrderDetail.params.PanelID + " #ddlFavRDOProvider").multiselect({
                            includeSelectAllOption: true,
                            enableFiltering: true,
                            enableCaseInsensitiveFiltering: true,
                            maxHeight: 247
                       });
                       Favorite_RadiologyOrderDetail.providerCheckedIds = FavoriteListProvidersJSON;
                       // Patient_Demographic.multipleRaceIds = demographic_detail.strRaceIds;
                    } else {
                       $('#' + Favorite_RadiologyOrderDetail.params.PanelID + " #ddlFavRDOProvider").find("option:selected").removeAttr("selected");
                       $('#' + Favorite_RadiologyOrderDetail.params.PanelID + " #ddlFavRDOProvider").multiselect("refresh");
                    }
                    self.find('#txtFavoriteListName').val(JSON.parse(response.FavoriteListJSON)[0].Name);
                    Favorite_RadiologyOrderDetail.favoriteList_CPTSearch(JSON.parse(response.FavoriteListJSON)[0].FavoriteListId);
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    //Method Name: favoriteList_CPTSearch
    // Author Name: Humaira Yousaf
    //Craeted Date: 24-03-2016
    //Description:  searches favorite radiology
    favoriteList_CPTSearch: function (FavoriteListId, FavoriteListCPTId, PageNo, rpp) {
        Favorite_RadiologyOrder.searchFavoriteList_CPT_DBCall(FavoriteListCPTId, FavoriteListId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                //Bind to list
                $.each(JSON.parse(response.FavoriteListCPTJSON), function (index, CPTRow) {
                    Favorite_RadiologyOrderDetail.bindToCPTList(CPTRow.CPTCode, CPTRow.CPTCodeDescription, CPTRow.OrderTestLOINCId, CPTRow.FavoriteListCPTId);
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },
    //Method Name: fillFavoriteRadiologyOrder_DBCall
    // Author Name: Humaira Yousaf
    //Craeted Date: 24-03-2016
    //Description:  fills favorite radiology
    fillFavoriteRadiologyOrder_DBCall: function (FavoriteListId, FavoriteListICDId) {

        var objData = {};
        // objData["IsActive"] = null;
        objData["FavoriteListId"] = FavoriteListId == null ? -1 : FavoriteListId;
        objData["FavoriteListICDId"] = FavoriteListICDId == null ? -1 : FavoriteListICDId;
        objData["ListType"] = 'RadiologyOrder';
        objData["EntityId"] = globalAppdata["SeletedEntityId"];

        //06/06/2016//Abid Ali//Added for loading in edit mode
        if (Favorite_RadiologyOrder.Switch == "0")
            objData["IsActive"] = false;
        else
            objData["IsActive"] = true;

        objData["PageNumber"] = -1;
        objData["RowsPerPage"] = -1;
        objData["commandType"] = "load_favoritelist";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },
    //Method Name: FavoriteListICD
    // Author Name: Humaira Yousaf
    //Craeted Date: 24-03-2016
    //Description:  favorite radiology ICD
    FavoriteListICD: function (response) {
        if (response.FavoriteListICDCount > 0) {
            var FavoriteListJSON = JSON.parse(response.FavoriteListICDJSON);
            $.each(FavoriteListJSON, function (i, item) {
                if (i <= 1) {
                    if (item.IsActive == "True") {
                        $('#' + Favorite_RadiologyOrderDetail.params.PanelID + ' #frmFavoriteRadiologyOrderDetail #IsActive').prop('checked', true);
                    } else {
                        $('#' + Favorite_RadiologyOrderDetail.params.PanelID + ' #frmFavoriteRadiologyOrderDetail #IsActive').prop('checked', false);
                    }
                    $('#' + Favorite_RadiologyOrderDetail.params.PanelID + ' #frmFavoriteRadiologyOrderDetail #txtFavoriteListName').val(item.FavoriteListName);
                    $('#' + Favorite_RadiologyOrderDetail.params.PanelID + ' #frmFavoriteRadiologyOrderDetail #lstEntityId').val(item.EntityId);

                }
                Favorite_RadiologyOrderDetail.AddInArray(item.FavoriteListICDId, item.ICD9Code, item.ICD10Code, item.SNOMEDID, item.ICD10CodeDescription, true, item.FavoriteListICDId);
                var li = "<li  id=" + item.FavoriteListICDId + " ><a href='#' class='pr-sm'>" + item.ICD10CodeDescription + "<span class='removeIconListHover' onclick='Favorite_Complaints_Detail.deleteFavChiefComplaint(" + item.FavoriteListICDId + ",event);'><i class='fa fa-times'></i></span></a></li>"
                $('#' + Favorite_RadiologyOrderDetail.params.PanelID + ' #ulFavCompliantDisease').append(li);
            });
        }
    },

}