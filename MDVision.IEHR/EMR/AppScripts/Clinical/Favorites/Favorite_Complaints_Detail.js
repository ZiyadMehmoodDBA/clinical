Favorite_Complaints_Detail = {
    bIsFirstLoad: true,
    params: [],
    Switch: 1,
    providerCheckedIds: [],
    ProviderIds: '',
    FavComplaints: [],
    Load: function (params) {
        Favorite_Complaints_Detail.params = params;
        if (Favorite_Complaints_Detail.params.PanelID != 'pnlFavoriteComplaintsDetail') {
            Favorite_Complaints_Detail.params.PanelID = Favorite_Complaints_Detail.params.PanelID + ' #pnlFavoriteComplaintsDetail';
        } else {
            Favorite_Complaints_Detail.params.PanelID = 'pnlFavoriteComplaintsDetail';
        }
        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Favorite_Complaints_Detail.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        if (globalAppdata['AppUserName'] == DefaultUser) {
            $('#' + Favorite_Complaints_Detail.params.PanelID + ' #frmFavoriteComplaintsDetail #divEntity').show();
        }
        var self = $('#' + Favorite_Complaints_Detail.params.PanelID);
        self.loadDropDowns(true).done(function () {
            selectedEntity = globalAppdata["SeletedEntityId"];
            Favorite_Complaints_Detail.loadEntityProvider(selectedEntity);
            if (Favorite_Complaints_Detail.params.mode == "Add") {
                $('#' + Favorite_Complaints_Detail.params.PanelID + ' #frmFavoriteComplaintsDetail #IsActive').prop("disabled", true);
                $('#' + Favorite_Complaints_Detail.params.PanelID + ' #frmFavoriteComplaintsDetail').data('serialize', $('#' + Favorite_Complaints_Detail.params.PanelID + ' #frmFavoriteComplaintsDetail').serialize());
            }
            else if (Favorite_Complaints_Detail.params.mode == "Edit") {
                $('#' + Favorite_Complaints_Detail.params.PanelID + ' #lblHeading').html('Edit Favorites List');
                $('#' + Favorite_Complaints_Detail.params.PanelID + ' #frmFavoriteComplaintsDetail #IsActive').prop("disabled", false);
                Favorite_Complaints_Detail.fillFavorite_Complaints(Favorite_Complaints_Detail.params.FavoriteListId);
            }
            Favorite_Complaints_Detail.params.IsActivePrevious = ($('#frmFavoriteComplaintsDetail #IsActive').prop("checked") == true ? "1" : "0");
            Favorite_Complaints_Detail.ValidateFavComplaint();
        });
    },
        
    loadEntityProvider: function (entityId) {
        var objDeffered = $.Deferred();
        var data = "entityID=" + entityId;
        if (entityId != null && entityId > 0) {

            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $('#' + Favorite_Complaints_Detail.params.PanelID + ' #ddlFavProvider');
                var $providerHiddenDdl = $('#' + Favorite_Complaints_Detail.params.PanelID + ' #ddlHiddenFavProvider');

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
                if (Favorite_Complaints_Detail.ProviderIds != '') {
                    var Providers = Favorite_Complaints_Detail.ProviderIds.split(",");
                    Favorite_Complaints_Detail.providerCheckedIds = Providers;
                    $('#' + Favorite_Complaints_Detail.params.PanelID + ' #ddlFavProvider').val(Providers);
                }
                $('#' + Favorite_Complaints_Detail.params.PanelID + ' #frmFavoriteComplaintsDetail').data('serialize', $('#' + Favorite_Complaints_Detail.params.PanelID + ' #frmFavoriteComplaintsDetail').serialize());
            }).then(function () {
                //Intialized in onhidden spacialty ddl.
                Favorite_Complaints_Detail.IntializeMultiSelectDropDownProviders();
                $('#' + Favorite_Complaints_Detail.params.PanelID + ' #frmFavoriteComplaintsDetail').data('serialize', $('#' + Favorite_Complaints_Detail.params.PanelID + ' #frmFavoriteComplaintsDetail').serialize());
                objDeffered.resolve();

            });
        }
        else {
            objDeffered.resolve();
        }
        return objDeffered;
    },

    IntializeMultiSelectDropDownProviders: function () {
        $('#' + Favorite_Complaints_Detail.params.PanelID + ' #ddlFavProvider').multiselect('destroy');
        $('#' + Favorite_Complaints_Detail.params.PanelID + ' #ddlFavProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 200,
            nonSelectedText: 'Select',
            selectAll: false,
            onDropdownHide: function (event) {
            },
            onChange: function () {
                Favorite_Complaints_Detail.CheckProviderValidation();
            },
        });
    },

    CheckProviderValidation: function () {
        var self = $("#" + Favorite_Complaints_Detail.params.PanelID);
        var ProviderIds = self.find('#ddlFavProvider option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        var strProviderIds = ProviderIds;
        Favorite_Complaints_Detail.providerCheckedIds = ProviderIds;
        if (Favorite_Complaints_Detail.providerCheckedIds != '') {
            Favorite_Complaints_Detail.validateProvider(2);
        } else {
            Favorite_Complaints_Detail.validateProvider(1);
        }
    },

    validateProvider: function (operationid) {
        var self = "#" + Favorite_Complaints_Detail.params.PanelID + " #frmFavoriteComplaintsDetail";
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
        $('#pnlFavoriteComplaintsDetail #txtDiagnosis').attr('data-popupunload', 'true');
        var params = [];
        params["FromAdmin"] = "0";
        //params["Parentctrl"] = Clinical_MedicalHx.params["TabID"];
        if (Favorite_Complaints_Detail.params.TabID == 'clinicalTabProgressNote') {
            params['FromProgressNote'] = 'pnlClinicalProgressNote';
            params["ParentCtrl"] = 'Favorite_Complaints_Detail';
        }
        else
            params["ParentCtrl"] = 'Favorite_Complaints_Detail';

        if (ctrl != null) {
            params["RefCtrl"] = ctrl;
        }
        if (hiddenCtrl != null) {
            params["RefHiddenCtrl"] = hiddenCtrl;
        }
        if (controlToLoad != "") {
            if (Favorite_Complaints_Detail.params.TabID == 'clinicalTabProgressNote' && searchType == "ICD")
                LoadActionPan(controlToLoad, params, 'pnlClinicalProgressNote');
            else
                LoadActionPan(controlToLoad, params);
        }

    },

    fillFavorite_Complaints: function (FavoriteListId) {
        Favorite_Complaints_Detail.fillFavorite_Complaints_DbCall(FavoriteListId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var FavoriteListProvidersJSON = JSON.parse(response.FavoriteListProviders_JSON);
                if (FavoriteListProvidersJSON != '' && FavoriteListProvidersJSON) {
                    $('#' + Favorite_Complaints_Detail.params.PanelID + " #ddlFavProvider").val(FavoriteListProvidersJSON);

                    $('#' + Favorite_Complaints_Detail.params.PanelID + " #ddlFavProvider").multiselect("refresh");
                    $('#' + Favorite_Complaints_Detail.params.PanelID + " #ddlFavProvider").multiselect({
                        includeSelectAllOption: true,
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                        maxHeight: 247
                    });
                    FavoriteListProvidersJSON.providerCheckedIds = FavoriteListProvidersJSON;
                } else {
                    $('#' + Favorite_Complaints_Detail.params.PanelID + " #ddlFavProvider").find("option:selected").removeAttr("selected");
                    $('#' + Favorite_Complaints_Detail.params.PanelID + " #ddlFavProvider").multiselect("refresh");
                }
                Favorite_Complaints_Detail.FavoriteListICD(response);
                $('#' + Favorite_Complaints_Detail.params.PanelID + ' #frmFavoriteComplaintsDetail').data('serialize', $('#' + Favorite_Complaints_Detail.params.PanelID + ' #frmFavoriteComplaintsDetail').serialize());
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
                    if (item.IsActive == "True") {
                        $('#' + Favorite_Complaints_Detail.params.PanelID + ' #frmFavoriteComplaintsDetail #IsActive').prop('checked', true);
                    } else {
                        $('#' + Favorite_Complaints_Detail.params.PanelID + ' #frmFavoriteComplaintsDetail #IsActive').prop('checked', false);
                    }
                    $('#' + Favorite_Complaints_Detail.params.PanelID + ' #frmFavoriteComplaintsDetail #txtFavoriteListName').val(item.FavoriteListName);
                    $('#' + Favorite_Complaints_Detail.params.PanelID + ' #frmFavoriteComplaintsDetail #lstEntityId').val(item.EntityId);
                    $('#' + Favorite_Complaints_Detail.params.PanelID + ' #frmFavoriteComplaintsDetail #ddlBodyParts').val(item.BodyPartId);
                }
                Favorite_Complaints_Detail.AddInArray(item.FavoriteListICDId, item.ICD9Code, item.ICD10Code, item.SNOMEDID, item.ICD10CodeDescription, true, item.FavoriteListICDId, item.ICD9CodeDescription, item.SNOMEDDescription);
                var li = "<li  id=" + item.FavoriteListICDId + " icd9Code='" + item.ICD9Code + "' icd10Code='" + item.ICD10Code + "' snomedCode='" + item.SNOMEDID + "' icd9Desc='" + item.ICD9CodeDescription + "' icd10Desc='" + item.ICD10CodeDescription + "' snomedDesc='" + item.SNOMEDDescription + "' ><a href='#' class='pr-sm'>" + item.ICD10CodeDescription + "<span class='removeIconListHover' onclick='Favorite_Complaints_Detail.deleteFavChiefComplaint(" + item.FavoriteListICDId + ",event);'><i class='fa fa-times'></i></span></a></li>"
                $('#' + Favorite_Complaints_Detail.params.PanelID + ' #ulFavCompliantDisease').append(li);
            });
        }
        Favorite_Complaints_Detail.params.IsActivePrevious = ($('#frmFavoriteComplaintsDetail #IsActive').prop("checked") == true ? "1" : "0");
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
        var detailObj = $.grep(Favorite_Complaints_Detail.FavComplaints, function (item, index) {
            return (item.FavoriteListICDId == event.target.closest('li').id)
        });

        Favorite_Complaints_Detail.FavComplaints.splice(detailObj, 1);
        event.target.closest('li').remove();
    },
    UnLoadTab: function () {

        if (EMRUtility.compareFormDataWithSerialized(Favorite_Complaints_Detail.params.PanelID + ' #frmFavoriteComplaintsDetail') || $('#' + Favorite_Complaints_Detail.params.PanelID + ' #ulFavCompliantDisease').find('li[id*="-"]').length > 0 || Favorite_Complaints_Detail.FavComplaints.length > 0) {
            utility.myConfirm('2', function () {
                Favorite_Complaints_Detail.UnloadFavComplaint();
            },
           '1'
           );
        } else {
            Favorite_Complaints_Detail.UnloadFavComplaint();
        }



    },
    UnloadFavComplaint: function () {

        if (Favorite_Complaints_Detail.params["FromAdmin"] == "0") {
            if (Favorite_Complaints_Detail.params != null && Favorite_Complaints_Detail.params.ParentCtrl != null) {
                UnloadActionPan(Favorite_Complaints_Detail.params.ParentCtrl, 'Favorite_Complaints_Detail');
            }
            else
                UnloadActionPan(null, 'Favorite_Complaints_Detail');
        }
        else {
            RemoveAdminTab();
        }


    },
    ValidateFavComplaint: function () {
        var self = $('#' + Favorite_Complaints_Detail.params.PanelID + ' #frmFavoriteComplaintsDetail');
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
                              message: 'Specify a Entity for the Favorite Complaint and try again. '
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
                  Complaints: {
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
           if (Favorite_Complaints_Detail.FavComplaints.length > 0) {
               Favorite_Complaints_Detail.FavComplaintsSave();
           }
           else
               self.data('bootstrapValidator').enableFieldValidators('Complaints', true);

       })
        .on('error.form.bv', function (e) {
            e.preventDefault();
            if (Favorite_Complaints_Detail.FavComplaints.length > 0 && self.find('#txtFavoriteListName').val() != "") {
                //Disable validator on procedure
                self.data('bootstrapValidator').enableFieldValidators('Complaints', false);
                self.trigger('success.form.bv');
            }
            else {
                //Disable validator if cpt has data
                if (Favorite_Complaints_Detail.FavComplaints.length > 0)
                    self.data('bootstrapValidator').enableFieldValidators('Complaints', false);
            }
       });

    },
    BindICD9AutoComplete: function (element) {
        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "Favorite_Complaints_Detail", null, false);
    },
    //Start//22/03/2016//M Ahmad Imran//Implimented Save Fav Complaint Detail
    FavComplaintsSave: function () {
        if (globalAppdata['AppUserName'] == DefaultUser) {
            $("#" + Favorite_Complaints_Detail.params.PanelID + " #frmFavoriteComplaintsDetail #lstEntityId").enable = true;
            $("#" + Favorite_Complaints_Detail.params.PanelID + " #frmFavoriteComplaintsDetail").bootstrapValidator('revalidateField', 'EntityId');
        }

        if (Favorite_Complaints_Detail.FavComplaints.length > 0) {
            Favorite_Complaints_Detail.SaveFavComplaint().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {

                    utility.DisplayMessages(response.Message, 1);
                    Favorite_Complaints.favoriteListSearch();
                    Favorite_Complaints_Detail.FavComplaints = [];
                    $('#' + Favorite_Complaints_Detail.params.PanelID + ' #frmFavoriteComplaintsDetail').data('serialize', $('#' + Favorite_Complaints_Detail.params.PanelID + ' #frmFavoriteComplaintsDetail').serialize());
                    Favorite_Complaints_Detail.UnloadFavComplaint();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else {
            utility.DisplayMessages("There is no Favorite Complaint to add", 3);/// ask from babur bhai
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
        Favorite_Complaints_Detail.FavComplaints.push(item);
        if (IsSelect) {
            //Favorite_Complaints_Detail.fillChiefComplaints($('#' + Favorite_Complaints_Detail.params.PanelID + " #frmClinicalComplaints #ulCompliantDisease li#" + id + ""), event);
        }

    },
    //End M Ahmad Imran 22/03/2016

    //Start//22/03/2016//M Ahmad Imran//Implimented Call to Controller for Fav Save Complaint Detail
    SaveFavComplaint: function () {
        //FavoriteListIcd
        var objData = {};
        var self = $('#pnlFavoriteComplaintsDetail');
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objDetail = JSON.parse(myJSON);
        if (objDetail["ProviderIds"] != '') {
            Favorite_Complaints_Detail.validateProvider(2);
            objData["IsActive"] = objDetail.IsActive;
            objData["ListType"] = "Complaints";
            objData["BodyPartId"] = objDetail["BodyPartId"];
            objData["EntityId"] = objDetail["EntityId"];
            objData["FavoriteListName"] = objDetail["FavoriteListName"];
            objData["ProviderIds"] = objDetail["ProviderIds"];
            if (globalAppdata['AppUserName'] != DefaultUser) {
                objData["EntityId"] = -1;
            }
            objData["FavoriteListIcd"] = Favorite_Complaints_Detail.FavComplaints;
            if (Favorite_Complaints_Detail.params.FavoriteListId != null) {
                objData["FavoriteListId"] = Favorite_Complaints_Detail.params.FavoriteListId;
            } else {
                objData["FavoriteListId"] = -1;
            }
            if (Favorite_Complaints_Detail.params.mode == "Edit") {
                objData["IsActivePrevious"] = Favorite_Complaints_Detail.params.IsActivePrevious;
                objData["commandType"] = "UPDATE_FAVCOMPLAINT";
            } else {
                objData["commandType"] = "SAVE_FavComplaint";
            }
            var data = JSON.stringify(objData);
            return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
        }
        else {
            Favorite_Complaints_Detail.validateProvider(1);
        }
    },
    //End M Ahmad Imran 22/03/2016
}