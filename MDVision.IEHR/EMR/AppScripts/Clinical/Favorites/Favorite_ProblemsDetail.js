Favorite_ProblemsDetail = {
    bIsFirstLoad: true,
    params: [],
    Switch: 1,
    providerCheckedIds: [],
    ProviderIds: '',
    FavComplaints: [],
    Load: function (params) {
        Favorite_ProblemsDetail.params = params;
        if (Favorite_ProblemsDetail.params.PanelID != 'pnlFavoriteProblemsDetail') {
            Favorite_ProblemsDetail.params.PanelID = Favorite_ProblemsDetail.params.PanelID + ' #pnlFavoriteProblemsDetail';
        } else {
            Favorite_ProblemsDetail.params.PanelID = 'pnlFavoriteProblemsDetail';
        }
        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Favorite_ProblemsDetail.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        if (globalAppdata['AppUserName'] == DefaultUser) {
            $('#' + Favorite_ProblemsDetail.params.PanelID + ' #frmFavoriteProblemsDetail #divEntity').show();
        }
        var self = $('#' + Favorite_ProblemsDetail.params.PanelID);
        self.loadDropDowns(true).done(function () {
            selectedEntity = globalAppdata["SeletedEntityId"];
            $.when(Favorite_ProblemsDetail.loadEntityProvider(selectedEntity)).then(function () {
                if (Favorite_ProblemsDetail.params.mode == "Add") {

                    $('#' + Favorite_ProblemsDetail.params.PanelID + ' #frmFavoriteProblemsDetail #IsActive').prop("disabled", true);
                    $('#' + Favorite_ProblemsDetail.params.PanelID + ' #lblTitleHeading ').text("Add Favorites List");
                    $('#' + Favorite_ProblemsDetail.params.PanelID + ' #frmFavoriteProblemsDetail').data('serialize', $('#' + Favorite_ProblemsDetail.params.PanelID + ' #frmFavoriteProblemsDetail').serialize());
                }
                else if (Favorite_ProblemsDetail.params.mode == "Edit") {
                    $('#' + Favorite_ProblemsDetail.params.PanelID + ' #lblTitleHeading ').text("Edit Favorites List");
                    $('#' + Favorite_ProblemsDetail.params.PanelID + ' #frmFavoriteProblemsDetail #IsActive').prop("disabled", false);
                    Favorite_ProblemsDetail.fillFavorite_Complaints(Favorite_ProblemsDetail.params.FavoriteListId);
                }
            });
           

            Favorite_ProblemsDetail.params.IsActivePrevious = ($('#frmFavoriteProblemsDetail #IsActive').prop("checked") == true ? "1" : "0");
            /*if (globalAppdata['AppUserName'] == DefaultUser) {
                Favorite_ProblemsDetail.params.EntityIdPrevious = $('#frmFavoriteProblemsDetail #lstEntityId').val();
            }*/
            Favorite_ProblemsDetail.ValidateFavComplaint();
            $('#' + Favorite_ProblemsDetail.params.PanelID + ' #frmFavoriteProblemsDetail').data('serialize', $('#' + Favorite_ProblemsDetail.params.PanelID + ' #frmFavoriteProblemsDetail').serialize());
        });
    },

    loadEntityProvider: function (entityId) {
        var objDeffered = $.Deferred();
        var data = "entityID=" + entityId;
        if (entityId != null && entityId > 0) {

            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $('#' + Favorite_ProblemsDetail.params.PanelID + ' #ddlFavProvider');
                var $providerHiddenDdl = $('#' + Favorite_ProblemsDetail.params.PanelID + ' #ddlHiddenFavProvider');

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
                if (Favorite_ProblemsDetail.ProviderIds != '') {
                    var Providers = Favorite_ProblemsDetail.ProviderIds.split(",");
                    Favorite_ProblemsDetail.providerCheckedIds = Providers;
                    $('#' + Favorite_ProblemsDetail.params.PanelID + ' #ddlFavProvider').val(Providers);
                }

            }).then(function () {
                //Intialized in onhidden spacialty ddl.
                Favorite_ProblemsDetail.IntializeMultiSelectDropDownProviders();
                objDeffered.resolve();

            });
        }
        else {
            objDeffered.resolve();
        }
        return objDeffered;
    },

    IntializeMultiSelectDropDownProviders: function () {
        $('#' + Favorite_ProblemsDetail.params.PanelID + ' #ddlFavProvider').multiselect('destroy');
        $('#' + Favorite_ProblemsDetail.params.PanelID + ' #ddlFavProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 200,
            nonSelectedText: 'Select',
            selectAll: false,
            onDropdownHide: function (event) {
            },
            onChange: function () {
                Favorite_ProblemsDetail.CheckProviderValidation();
            },
        });
    },

    CheckProviderValidation: function () {
        var self = $("#" + Favorite_ProblemsDetail.params.PanelID);
        var ProviderIds = self.find('#ddlFavProvider option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        var strProviderIds = ProviderIds;
        Favorite_ProblemsDetail.providerCheckedIds = ProviderIds;
        if (Favorite_ProblemsDetail.providerCheckedIds != '') {
            Favorite_ProblemsDetail.validateProvider(2);
        } else {
            Favorite_ProblemsDetail.validateProvider(1);
        }
    },

    validateProvider: function (operationid) {
        var self = "#" + Favorite_ProblemsDetail.params.PanelID + " #frmFavoriteProblemsDetail";
        $(self + " #divFavProvider").find("i").remove();
        if (operationid == 1) {
            $(self + " .multiselect").css("border-color", "#cc2724");
            $(self + " #divFavProvider").find(".control-label").css("color", "#cc2724");
            $(self + " #divFavProvider").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-remove" data-bv-icon-for="Race" style="display: block;"></i>');
        } else if (operationid == 2) {
            $(self + " .multiselect").css("border-color", "#3c763d");
            $(self + " #divFavProvider").find(".control-label").css("color", "#3c763d");
            $(self + " #divFavProvider").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-ok" data-bv-icon-for="Race" style="display: block;"></i>');
        } else {
            $(self + " .multiselect").css("border-color", "#ccc");
            $(self + " #divFavProvider").find(".control-label").css("color", "#000000");
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
        $('#pnlFavoriteProblemsDetail #txtDiagnosis').attr('data-popupunload', 'true');
        var params = [];
        params["FromAdmin"] = "0";
        //params["Parentctrl"] = Clinical_MedicalHx.params["TabID"];
        if (Favorite_ProblemsDetail.params.TabID == 'clinicalTabProgressNote') {
            params['FromProgressNote'] = 'pnlClinicalProgressNote';
            params["ParentCtrl"] = 'Favorite_ProblemsDetail';
        }
        else
            params["ParentCtrl"] = 'Favorite_ProblemsDetail';

        if (ctrl != null) {
            params["RefCtrl"] = ctrl;
        }
        if (hiddenCtrl != null) {
            params["RefHiddenCtrl"] = hiddenCtrl;
        }
        if (controlToLoad != "") {
            if (Favorite_ProblemsDetail.params.TabID == 'clinicalTabProgressNote' && searchType == "ICD")
                LoadActionPan(controlToLoad, params, 'pnlClinicalProgressNote');
            else
                LoadActionPan(controlToLoad, params);
        }

    },
    fillFavorite_Complaints: function (FavoriteListId) {
        Favorite_ProblemsDetail.fillFavorite_Complaints_DbCall(FavoriteListId).done(function (response) {
            response = JSON.parse(response);
            if (JSON.parse(response.FavoriteListICDJSON)[0].IsActive == 'True') {
                $('#frmFavoriteProblemsDetail #IsActive').prop('checked', true);
            } else {
                $('#frmFavoriteProblemsDetail #IsActive').prop('checked', false);
            }
            Favorite_ProblemsDetail.params.IsActivePrevious = ($('#frmFavoriteProblemsDetail #IsActive').prop("checked") == true ? "1" : "0");
            /*if (globalAppdata['AppUserName'] == DefaultUser) {
                Favorite_ProblemsDetail.params.EntityIdPrevious = $('#frmFavoriteProblemsDetail #lstEntityId').val();
            }*/
            //console.log(response);IsActive
            if (response.status != false) {
                var FavoriteListProvidersJSON = JSON.parse(response.FavoriteListProviders_JSON);
                if (FavoriteListProvidersJSON != '' && FavoriteListProvidersJSON) {
                    $('#' + Favorite_ProblemsDetail.params.PanelID + " #ddlFavProvider").val(FavoriteListProvidersJSON);

                    $('#' + Favorite_ProblemsDetail.params.PanelID + " #ddlFavProvider").multiselect("refresh");
                    $('#' + Favorite_ProblemsDetail.params.PanelID + " #ddlFavProvider").multiselect({
                        includeSelectAllOption: true,
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                        maxHeight: 247
                    });
                    FavoriteListProvidersJSON.providerCheckedIds = FavoriteListProvidersJSON;
                    // Patient_Demographic.multipleRaceIds = demographic_detail.strRaceIds;
                } else {
                    $('#' + Favorite_ProblemsDetail.params.PanelID + " #ddlFavProvider").find("option:selected").removeAttr("selected");
                    $('#' + Favorite_ProblemsDetail.params.PanelID + " #ddlFavProvider").multiselect("refresh");
                }
                Favorite_ProblemsDetail.FavoriteListICD(response);
                $('#' + Favorite_ProblemsDetail.params.PanelID + ' #frmFavoriteProblemsDetail').data('serialize', $('#' + Favorite_ProblemsDetail.params.PanelID + ' #frmFavoriteProblemsDetail').serialize());
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    FavoriteListICD: function (response) {
        if (response.FavoriteListICDCount > 0) {
            var FavoriteListJSON = JSON.parse(response.FavoriteListICDJSON);
            $.each(FavoriteListJSON, function (i, item) {
                if (i <= 1) {
                    /*if (item.IsActive == "True") {
                        $('#' + Favorite_ProblemsDetail.params.PanelID + ' #frmFavoriteProblemsDetail #IsActive').prop('checked', true);
                    } else {
                        $('#' + Favorite_ProblemsDetail.params.PanelID + ' #frmFavoriteProblemsDetail #IsActive').prop('checked', false);
                    }*/
                    $('#' + Favorite_ProblemsDetail.params.PanelID + ' #frmFavoriteProblemsDetail #txtFavoriteListName').val(item.FavoriteListName);
                    $('#' + Favorite_ProblemsDetail.params.PanelID + ' #frmFavoriteProblemsDetail #lstEntityId').val(item.EntityId);
                    $('#' + Favorite_ProblemsDetail.params.PanelID + ' #frmFavoriteProblemsDetail #ddlBodyParts').val(item.BodyPartId);

                }
                Favorite_ProblemsDetail.AddInArray(item.FavoriteListICDId, item.ICD9Code, item.ICD10Code, item.SNOMEDID, item.ICD10CodeDescription, true, item.FavoriteListICDId, item.ICD9CodeDescription, item.SNOMEDDescription);
                //ICD9CodeDescription ICD10CodeDescription SNOMEDDescription
                var li = "<li  id=" + item.FavoriteListICDId + " icd9Code='" + item.ICD9Code + "' icd10Code='" + item.ICD10Code + "' snomedCode='" + item.SNOMEDID + "' icd9Desc='" + item.ICD9CodeDescription + "' icd10Desc='" + item.ICD10CodeDescription + "' snomedDesc='" + item.SNOMEDDescription + "' ><a href='#' class='pr-sm'>" + item.ICD10CodeDescription + "<span class='removeIconListHover' onclick='Favorite_ProblemsDetail.deleteFavChiefComplaint(" + item.FavoriteListICDId + ",event);'><i class='fa fa-times'></i></span></a></li>"
                $('#' + Favorite_ProblemsDetail.params.PanelID + ' #ulFavCompliantDisease').append(li);
            });
        }
        Favorite_ProblemsDetail.params.IsActivePrevious = ($('#frmFavoriteProblemsDetail #IsActive').prop("checked") == true ? "1" : "0");
    },
    fillFavorite_Complaints_DbCall: function (FavoriteListId, FavoriteListICDId) {

        var objData = {};
        // objData["IsActive"] = null;
        objData["FavoriteListId"] = FavoriteListId == null ? -1 : FavoriteListId;
        objData["FavoriteListICDId"] = FavoriteListICDId == null ? -1 : FavoriteListICDId;
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["PageNumber"] = -1;
        objData["RowsPerPage"] = -1;
        objData["commandType"] = "FILL_FavComplaint";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },

    deleteFavChiefComplaint: function (FavoriteListICDId, event) {
        var CopyFavComplaints = [];
        var detailObj = $.grep(Favorite_ProblemsDetail.FavComplaints, function (item, index) {
            if (item.DiagnosisId != FavoriteListICDId) {
                CopyFavComplaints.push(item);
            }
        });

        Favorite_ProblemsDetail.FavComplaints = CopyFavComplaints;
        event.target.closest('li').remove();
    },
    UnLoadTab: function () {

        if (EMRUtility.compareFormDataWithSerialized(Favorite_ProblemsDetail.params.PanelID + ' #frmFavoriteProblemsDetail') || $('#' + Favorite_ProblemsDetail.params.PanelID + ' #ulFavCompliantDisease').find('li[id*="-"]').length > 0 || Favorite_ProblemsDetail.FavComplaintsSave.length > 0) {
            utility.myConfirm('2', function () {
                Favorite_ProblemsDetail.UnloadFavComplaint();
            },
           '1'
           );
        } else {
            Favorite_ProblemsDetail.UnloadFavComplaint();
        }



    },
    UnloadFavComplaint: function () {

        if (Favorite_ProblemsDetail.params["FromAdmin"] == "0") {
            if (Favorite_ProblemsDetail.params != null && Favorite_ProblemsDetail.params.ParentCtrl != null) {
                UnloadActionPan(Favorite_ProblemsDetail.params.ParentCtrl, 'Favorite_ProblemsDetail');
            }
            else
                UnloadActionPan(null, 'Favorite_ProblemsDetail');
        }
        else {
            RemoveAdminTab();
        }


    },
    ValidateFavComplaint: function () {

        $('#' + Favorite_ProblemsDetail.params.PanelID + ' #frmFavoriteProblemsDetail')
          .bootstrapValidator({
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
                              message: 'Specify a Entity for the Favorite Problem and try again. '
                          },
                      }
                  },
                  FavoriteListName: {
                      group: '.col-sm-10',
                      validators: {
                          notEmpty: {
                              message: 'Specify a Name for the Favorite Complaint and try again. '
                          },
                      }
                  },


              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           Favorite_ProblemsDetail.FavComplaintsSave();

       });

    },
    BindICD9AutoComplete: function (element) {
        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "Favorite_ProblemsDetail", null, false);
    },
    MakeEmpty: function (element) {
        $(element).val('');
    },
    //Start//22/03/2016//M Ahmad Imran//Implimented Save Fav Complaint Detail
    FavComplaintsSave: function () {
        if (globalAppdata['AppUserName'] == DefaultUser) {
            $("#" + Favorite_ProblemsDetail.params.PanelID + " #frmFavoriteProblemsDetail #lstEntityId").enable = true;
            $("#" + Favorite_ProblemsDetail.params.PanelID + " #frmFavoriteProblemsDetail").bootstrapValidator('revalidateField', 'EntityId');
        }

        if (Favorite_ProblemsDetail.FavComplaints.length > 0) {
            Favorite_ProblemsDetail.SaveFavComplaint().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {

                    utility.DisplayMessages(response.Message, 1);
                    Favorite_Problems.favoriteListSearch();
                    Favorite_ProblemsDetail.FavComplaints = [];
                    //Favorite_ProblemsDetail.FavComplaints.splice(0);//kr
                    $('#' + Favorite_ProblemsDetail.params.PanelID + ' #frmFavoriteProblemsDetail').data('serialize', $('#' + Favorite_ProblemsDetail.params.PanelID + ' #frmFavoriteProblemsDetail').serialize());
                    Favorite_ProblemsDetail.UnloadFavComplaint();
                    //Favorite_ProblemsDetail.fillFavorite_Complaints(Favorite_ProblemsDetail.params.FavoriteListId);//kr
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else {
            utility.DisplayMessages("There is no Problem to add", 3);/// ask from babur bhai
        }
    },
    //End M Ahmad Imran 22/03/2016

    //Start//22/03/2016//M Ahmad Imran//For record Complaint
    AddInArray: function (id, ICD9, ICD10, SNOMED, ICD10Des, IsSelect, FavoriteListICDId, ICD9Des, SnomedDes) {
        var item = {};
        item["DiagnosisId"] = id;
        item["ICD9"] = ICD9;
        item["ICD10"] = ICD10;
        item["ICD10Description"] = ICD10Des;
        item["SNOMED"] = SNOMED;
        item["SNOMEDDescription"] = SnomedDes;
        item["ICD9Description"] = ICD9Des;
        item["FavoriteListICDId"] = FavoriteListICDId == null ? -1 : FavoriteListICDId;
        Favorite_ProblemsDetail.FavComplaints.push(item);
        if (IsSelect) {
            //Favorite_ProblemsDetail.fillChiefComplaints($('#' + Favorite_ProblemsDetail.params.PanelID + " #frmClinicalComplaints #ulCompliantDisease li#" + id + ""), event);
        }

    },
    //End M Ahmad Imran 22/03/2016

    //Start//22/03/2016//M Ahmad Imran//Implimented Call to Controller for Fav Save Complaint Detail
    SaveFavComplaint: function () {
        //FavoriteListIcd
        var objData = {};
        var self = $('#pnlFavoriteProblemsDetail');
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objDetail = JSON.parse(myJSON);
        if (objDetail["ProviderIds"] != '') {
            Favorite_ProblemsDetail.validateProvider(2);
            objData["IsActive"] = objDetail.IsActive;
            objData["BodyPartId"] = objDetail["BodyPartId"];
            objData["ListType"] = "Problems";
            objData["EntityId"] = objDetail["EntityId"];
            objData["FavoriteListName"] = objDetail["FavoriteListName"];
            objData["ProviderIds"] = objDetail["ProviderIds"];
            if (globalAppdata['AppUserName'] != DefaultUser) {
                objData["EntityId"] = -1;
            }
            objData["FavoriteListIcd"] = Favorite_ProblemsDetail.FavComplaints;
            if (Favorite_ProblemsDetail.params.FavoriteListId != null) {
                objData["FavoriteListId"] = Favorite_ProblemsDetail.params.FavoriteListId;
            } else {
                objData["FavoriteListId"] = -1;
            }
            if (Favorite_ProblemsDetail.params.mode == "Edit") {
                objData["IsActivePrevious"] = Favorite_ProblemsDetail.params.IsActivePrevious;
                objData["commandType"] = "UPDATE_FAVCOMPLAINT";
            } else {
                objData["commandType"] = "SAVE_FavComplaint";
            }
            var data = JSON.stringify(objData);
            return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
        }
        else {
            Favorite_ProblemsDetail.validateProvider(1);
        }
    },
    //End M Ahmad Imran 22/03/2016
}