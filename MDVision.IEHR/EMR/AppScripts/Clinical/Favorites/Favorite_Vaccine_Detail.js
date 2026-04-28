Favorite_Vaccine_Detail = {
    bIsFirstLoad: true,
    params: [],
    Switch: 1,
    FavVaccine: [],
    providerCheckedIds: [],
    ProviderIds: '',
    Load: function (params) {
        Favorite_Vaccine_Detail.params = params;
        if (Favorite_Vaccine_Detail.params.PanelID != 'pnlFavoriteVaccineDetail') {
            Favorite_Vaccine_Detail.params.PanelID = Favorite_Vaccine_Detail.params.PanelID + ' #pnlFavoriteVaccineDetail';
        } else {
            Favorite_Vaccine_Detail.params.PanelID = 'pnlFavoriteVaccineDetail';
        }
        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Favorite_Vaccine_Detail.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        if (globalAppdata['AppUserName'] == DefaultUser) {
            $('#' + Favorite_Vaccine_Detail.params.PanelID + ' #frmFavoriteVaccineDetail #divEntity').show();
        }
        var self = $('#' + Favorite_Vaccine_Detail.params.PanelID);
        if (Favorite_Vaccine_Detail.bIsFirstLoad) {
            self.loadDropDowns(true).done(function () {
                selectedEntity = globalAppdata["SeletedEntityId"];
                $.when(Favorite_Vaccine_Detail.loadEntityProvider(selectedEntity)).then(function () {
                    if (Favorite_Vaccine_Detail.params.mode == "Add") {
                        $('#' + Favorite_Vaccine_Detail.params.PanelID + ' #frmFavoriteVaccineDetail #IsActive').attr("disabled", true);
                    }
                    else {
                        Favorite_Vaccine_Detail.FillFavVaccineDetail(Favorite_Vaccine_Detail.params.FavoritiesListId);
                        $('#' + Favorite_Vaccine_Detail.params.PanelID + ' #frmFavoriteVaccineDetail #IsActive').attr("disabled", false);
                        $('#' + Favorite_Vaccine_Detail.params.PanelID + ' #lblHeading').html('Edit Favorites List');
                    }
                });

            });
            Favorite_Vaccine_Detail.ValidateFavVaccineDetail();
        }
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body', html: true });
    },
    loadEntityProvider: function (entityId) {
        var objDeffered = $.Deferred();
        var data = "entityID=" + entityId;
        if (entityId != null && entityId > 0) {

            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $('#' + Favorite_Vaccine_Detail.params.PanelID + ' #ddlProvider');
                var $providerHiddenDdl = $('#' + Favorite_Vaccine_Detail.params.PanelID + ' #ddlHiddenFavProvider');

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
                if (Favorite_Vaccine_Detail.ProviderIds != '') {
                    var Providers = Favorite_Vaccine_Detail.ProviderIds.split(",");
                    Favorite_Vaccine_Detail.providerCheckedIds = Providers;
                    $('#' + Favorite_Vaccine_Detail.params.PanelID + ' #ddlProvider').val(Providers);
                }

            }).then(function () {
                //Intialized in onhidden spacialty ddl.
                Favorite_Vaccine_Detail.IntializeMultiSelectDropDownProviders();
                objDeffered.resolve();

            });
        }
        else {
            objDeffered.resolve();
        }
        return objDeffered;
    },
    ValidateFavVaccineDetail: function () {
        $('#' + Favorite_Vaccine_Detail.params.PanelID + ' #frmFavoriteVaccineDetail')
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
                          group: '.col-sm-4',
                          validators: {
                              notEmpty: {
                                  message: 'Specify a Entity for the Favorite vaccine and try again. '
                              },
                          }
                      },

                      FavoriteListName: {
                          group: '.col-sm-4',
                          validators: {
                              notEmpty: {
                                  message: 'Specify a Name for the Favorite vaccine and try again. '
                              }
                          }
                      },

                      ProviderIds: {
                          group: '.col-xs-9',
                          validators: {
                              notEmpty: {
                                  message: 'Specify a Provider for the Favorite vaccine and try again. '
                              }
                          }
                      },


                  }
              }).on('success.form.bv', function (e) {

                  e.preventDefault();
                  Favorite_Vaccine_Detail.FavVaccineSave();

              });
    },
    FillFavVaccineDetail: function (FavoritiesListId) {
        Favorite_Vaccine.LoadFavImmunization_Detail_DBCALL(FavoritiesListId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.FavImmunizationCount > 0) {
                    Favorite_Vaccine_Detail.BindFavVaccineData(response.FavImmunization_JSON);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    BindFavVaccineData: function (FavImmunization_JSON) {
        $("#" + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #lstEntityId").val(FavImmunization_JSON[0].EntityId);
        $("#" + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #txtFavoriteListName").val(FavImmunization_JSON[0].FavoriteListName);
        if (FavImmunization_JSON[0].ProviderIds != '') {
            $("#" + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #ddlProvider").val(FavImmunization_JSON[0].ProviderIds.split(','));
            $("#" + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #ddlProvider").multiselect("refresh");
            $("#" + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #ddlProvider").multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                maxHeight: 247
            });
            var Providers = FavImmunization_JSON[0].ProviderIds.split(",");
            Favorite_Vaccine_Detail.providerCheckedIds = Providers;
        }
        $("#" + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #IsActive").prop("checked", FavImmunization_JSON[0].IsActive);
        $("#" + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #ddlCategory").val(FavImmunization_JSON[0].VaccineGroupId);
        Favorite_Vaccine_Detail.CategoryChange($("#" + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #ddlCategory"));
        $.each(FavImmunization_JSON, function (i, item) {


            $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #ulFavCompliantDisease").append('<li id=' + item.VaccineID + ' >' + item.VaccineName + '<a class="removeIconListHover" onclick="Favorite_Vaccine_Detail.DeleteVaccine(' + item.VaccineID + ')"><i class="fa fa-times"></i></a></li>');
            //class="tooltipstered" data-title="' + liTitle + '"

            var popover;
            popover = '<p class="mb-none">Category: ' + item.CategoryText + '</p>'
                   + '<strong>Vaccine: ' + item.VaccineName + '</strong> <br />'
                   + (item.VaccineCVX != '' ? '<span class="pull-left">CVX: ' + item.VaccineCVX + '</span><br />' : '')
                   + (item.CPTCode != '' ? '<span class="pull-left">CPT Code: ' + item.CPTCode + '</span>' : '');


            $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #ulFavCompliantDisease li#" + item.VaccineID).tooltipster({
                theme: 'tooltipster-shadow',
                content: $(popover),
                functionReady: function (instance, helper) {
                    var posTop = $(helper)[0].getBoundingClientRect().top;
                    var anchorBottom = $(this)[0].getBoundingClientRect().bottom;
                    if (posTop < 0) {
                        $('.tooltipster-base').css("top", (anchorBottom + 13) + "px");
                        $('.tooltipster-arrow').removeClass("tooltipster-arrow-top").addClass("tooltipster-arrow-bottom");
                    }
                }

            });
        });
        $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #ddlCategory").attr("disabled", true);
        $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #ddlVaccine").attr("disabled", false);
    },
    FavVaccineSave: function () {
        if (globalAppdata['AppUserName'] == DefaultUser) {
            $("#" + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #lstEntityId").enable = true;
            $("#" + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail").bootstrapValidator('revalidateField', 'EntityId');
        }
        if ($('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #ulFavCompliantDisease li").length == 0) {
            utility.DisplayMessages("Select Vaccine", 2);
        }
        else {
            AppPrivileges.GetFormPrivileges("Favorites_Vaccine", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    var ProviderIds = $("#" + Favorite_Vaccine_Detail.params.PanelID).find('#ddlProvider option:Selected').map(function () {
                        return this.value;
                    }).get().join(',');
                    if (ProviderIds != '') {
                        Favorite_Vaccine_Detail.saveFavVaccine_DBCALL().done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                utility.DisplayMessages(response.Message, 1);
                                Favorite_Vaccine_Detail.UnLoadTab();
                                Favorite_Vaccine.LoadFavImmunization('vaccine');
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                    else {
                        Favorite_Vaccine_Detail.validateProvider(1);
                    }
                }
                else {
                    utility.DisplayMessages(strMessage, 2);
                }
            });

        }

    },
    saveFavVaccine_DBCALL: function () {
        //var objData = {};
        var self = $('#' + Favorite_Vaccine_Detail.params.PanelID);
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objDetail = JSON.parse(myJSON);

        var ProviderIds = self.find('#ddlProvider option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        objDetail["ProviderIds"] = ProviderIds;

        var ProviderText = self.find('#ddlProvider option:Selected').map(function () {
            return this.text;
        }).get().join(', ');
        objDetail["ProviderIds_Text"] = ProviderText;

        var VaccineIds = "";
        $.each($('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #ulFavCompliantDisease li"), function (i, item) {
            VaccineIds += $(item).attr("id") + ",";
        });
        objDetail["VaccineIds"] = VaccineIds.substring(0, VaccineIds.length - 1);

        if (globalAppdata['AppUserName'] != DefaultUser) {
            objDetail["EntityId"] = -1;
        }
        objDetail["Type"] = 'vaccine';
        if (Favorite_Vaccine_Detail.params.FavoritiesListId != null) {
            objDetail["FavoritiesListId"] = Favorite_Vaccine_Detail.params.FavoritiesListId;
        } else {
            objDetail["FavoritiesListId"] = -1;
        }
        if (Favorite_Vaccine_Detail.params.mode == "Edit") {
            objDetail["IsActivePrevious"] = Favorite_Vaccine_Detail.params.IsActivePrevious;
            objDetail["commandType"] = "UPDATE_FAvVaccine";
        } else {
            objDetail["commandType"] = "SAVE_FavVaccine";
        }

        var data = JSON.stringify(objDetail);
        return MDVisionService.APIService(data, "FavoriteList", "Immunization");

    },

    IntializeMultiSelectDropDownProviders: function () {
        $('#' + Favorite_Vaccine_Detail.params.PanelID + ' #ddlProvider').multiselect('destroy');
        $('#' + Favorite_Vaccine_Detail.params.PanelID + ' #ddlProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 200,
            nonSelectedText: 'Select',
            selectAll: false,
            onDropdownHide: function (event) {
            },
            onChange: function () {
                Favorite_Vaccine_Detail.CheckProviderValidation();
            },
        });
    },
    CheckProviderValidation: function () {
        var self = $("#" + Favorite_Vaccine_Detail.params.PanelID);
        var ProviderIds = self.find('#ddlProvider option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        var strProviderIds = ProviderIds;
        Favorite_Vaccine_Detail.providerCheckedIds = ProviderIds;
        if (Favorite_Vaccine_Detail.providerCheckedIds != '') {
            Favorite_Vaccine_Detail.validateProvider(2);
        } else {
            Favorite_Vaccine_Detail.validateProvider(1);
        }
    },
    validateProvider: function (operationid) {
        var self = "#" + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail";
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
    CategoryChange: function (obj) {
        if ($(obj).val() != "") {
            $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #ddlVaccine").attr("disabled", false);
            var self = $('#' + Favorite_Vaccine_Detail.params.PanelID);
            self.find('.Vaccine > select').attr('ddlist', 'GetVaccine');
            var data = "IsActive=&StrID=" + $('#' + Favorite_Vaccine_Detail.params.PanelID + ' #ddlCategory').val() + "&StrID2=hx";
            self.find('.Vaccine').loadDropDowns(true, data).done(function () {
            });
        }
        else {
            var self = $('#' + Favorite_Vaccine_Detail.params.PanelID);
            $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #ddlVaccine").attr("disabled", true);
            self.find('#ddlVaccine').attr('ddlist', '');
            $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #ddlVaccine").html('<option selected="selected" value=""> -Select- </option>');
        }
    },
    VaccineChange: function (obj) {
        if ($(obj).val() != "") {
            var vaccineId = $(obj).val();
            Favorite_Vaccine_Detail.GetCVXAndAdministeredCode_DB_CALL(vaccineId, 0, 'vaccine').done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var CVX = "";
                    var CPTCode = "";
                    if (response.VaccineDataCount > 0) {
                        var VaccineDataJSON = JSON.parse(response.VaccineDataJSON);
                        CVX = VaccineDataJSON[0].CVX;
                        CPTCode = VaccineDataJSON[0].CPTCode;
                        $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #txtCVX").val(CVX);
                        $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #txtCptCode").val(CPTCode);
                    }
                    else {
                        $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #txtCVX").val("");
                        $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #txtCptCode").val("");
                    }

                }
                else {
                    $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #txtCVX").val("");
                    $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #txtCptCode").val("");
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else {
            $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #txtCVX").val("");
            $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #txtCptCode").val("");
        }
    },
    onBlurVaccine: function () {
        var vaccineId = $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #ddlVaccine option:selected").val();
        if (vaccineId != "") {
            if (!Favorite_Vaccine_Detail.CheckAlreadyExists(vaccineId)) {
                var CategoryText = $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #ddlCategory option:selected").text().trim();
                var selectedOptionText = $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #ddlVaccine option:selected").text().trim();
                var CVX = $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #txtCVX").val().trim();
                var CPTCode = $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #txtCptCode").val().trim();
                var liTitle = "Category: " + CategoryText + " Vaccine: " + selectedOptionText + " CVX: " + CVX + " CPT Code: " + CPTCode;



                $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #ulFavCompliantDisease").append('<li id=' + vaccineId + ' >' + selectedOptionText + '<a class="removeIconListHover" onclick="Favorite_Vaccine_Detail.DeleteVaccine(' + vaccineId + ')"><i class="fa fa-times"></i></a></li>');
                //class="tooltipstered" data-title="' + liTitle + '"

                var popover;
                popover = '<p class="mb-none">Category: ' + CategoryText + '</p>'
                       + '<strong>Vaccine: ' + selectedOptionText + '</strong> <br />'
                       + (CVX != "" ? '<span class="pull-left">CVX: ' + CVX + '</span><br />' : '')
                       + (CPTCode != "" ? '<span class="pull-left">CPT Code: ' + CPTCode + '</span>' : '');


                $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #ulFavCompliantDisease li#" + vaccineId).tooltipster({
                    theme: 'tooltipster-shadow',
                    content: $(popover),
                    functionReady: function (instance, helper) {
                        var posTop = $(helper)[0].getBoundingClientRect().top;
                        var anchorBottom = $(this)[0].getBoundingClientRect().bottom;
                        if (posTop < 0) {
                            $('.tooltipster-base').css("top", (anchorBottom + 13) + "px");
                            $('.tooltipster-arrow').removeClass("tooltipster-arrow-top").addClass("tooltipster-arrow-bottom");
                        }
                    }

                });


                //$('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body', html: true });
                if (typeof $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #ddlCategory").attr("disabled") === typeof undefined || !$('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #ddlCategory").attr("disabled")) {
                    $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #ddlCategory").attr("disabled", true);
                }

                //clear filed
                $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #txtCVX").val("");
                $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #txtCptCode").val("");
                $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #ddlVaccine").val("");
            }
            else {
                $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #txtCVX").val("");
                $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #txtCptCode").val("");
                $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #ddlVaccine").val("");
                utility.DisplayMessages("Already Exists", 2);
            }

        }
        else {
            $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #txtCVX").val("");
            $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #txtCptCode").val("");
            $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #ddlVaccine").val("");
        }
    },
    CheckAlreadyExists: function (vaccineId) {
        var found = false;
        $.each($('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #ulFavCompliantDisease li"), function (i, item) {
            if ($(item).attr("id") == vaccineId) {
                found = true;
            }
        });
        return found;
    },
    DeleteVaccine: function (vaccineId) {
        $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #ulFavCompliantDisease li#" + vaccineId).remove();
        if ($('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #ulFavCompliantDisease li").length == 0) {
            $('#' + Favorite_Vaccine_Detail.params.PanelID + " #frmFavoriteVaccineDetail #ddlCategory").attr("disabled", false);
        }
    },

    GetCVXAndAdministeredCode_DB_CALL: function (VaccineId, TherapueticId, Type) {
        var objData = new Object();
        objData["VaccineIds"] = VaccineId;
        objData["TherapueticIds"] = TherapueticId;
        objData["commandType"] = "Get_CVXAndAdministeredCode";
        objData["Type"] = Type;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "Immunization");
    },


    UnLoadTab: function () {
        if (Favorite_Vaccine_Detail.params["FromAdmin"] == "0") {
            if (Favorite_Vaccine_Detail.params != null && Favorite_Vaccine_Detail.params.ParentCtrl != null) {
                UnloadActionPan(Favorite_Vaccine_Detail.params.ParentCtrl, 'Favorite_Vaccine_Detail');
            }
            else
                UnloadActionPan(null, 'Favorite_Vaccine_Detail');
        }
        else {
            RemoveAdminTab();
        }
    },
}