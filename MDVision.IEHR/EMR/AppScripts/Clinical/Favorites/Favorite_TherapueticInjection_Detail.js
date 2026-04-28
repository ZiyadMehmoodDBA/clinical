Favorite_TherapueticInjection_Detail = {
    bIsFirstLoad: true,
    params: [],
    Switch: 1,
    FavVaccine: [],
    providerCheckedIds: [],
    ProviderIds: '',
    Load: function (params) {
        Favorite_TherapueticInjection_Detail.params = params;
        if (Favorite_TherapueticInjection_Detail.params.PanelID != 'pnlFavoriteTherapueticInjectionDetail') {
            Favorite_TherapueticInjection_Detail.params.PanelID = Favorite_TherapueticInjection_Detail.params.PanelID + ' #pnlFavoriteTherapueticInjectionDetail';
        } else {
            Favorite_TherapueticInjection_Detail.params.PanelID = 'pnlFavoriteTherapueticInjectionDetail';
        }
        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Favorite_TherapueticInjection_Detail.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        if (globalAppdata['AppUserName'] == DefaultUser) {
            $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + ' #frmFavoriteTherapueticInjectionDetail #divEntity').show();
        }


        var self = $('#' + Favorite_TherapueticInjection_Detail.params.PanelID);
        if (Favorite_TherapueticInjection_Detail.bIsFirstLoad) {
            self.loadDropDowns(true).done(function () {
                Favorite_TherapueticInjection_Detail.LoadAllAutocomplete();
                selectedEntity = globalAppdata["SeletedEntityId"];
                $.when(Favorite_TherapueticInjection_Detail.loadEntityProvider(selectedEntity)).then(function () {
                    if (Favorite_TherapueticInjection_Detail.params.mode == "Add") {
                        $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + ' #frmFavoriteTherapueticInjectionDetail #IsActive').attr("disabled", true);
                    }
                    else {
                        Favorite_TherapueticInjection_Detail.FillFavVaccineDetail(Favorite_TherapueticInjection_Detail.params.FavoritiesListId);
                        $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + ' #frmFavoriteTherapueticInjectionDetail #IsActive').attr("disabled", false);
                        $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + ' #lblHeading').html('Edit Favorites List');
                    }
                });

            });
            Favorite_TherapueticInjection_Detail.ValidateFavVaccineDetail();
        }
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body', html: true });
    },
    LoadAllAutocomplete: function () {
        CacheManager.BindCodes('GetTherapeuticInjection', false).done(function (result) {
            $("#frmFavoriteTherapueticInjectionDetail #txtTherapeuticInjection").autocomplete({
                autoFocus: true,
                source: TherapeuticInjection, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #txtTherapeuticInjection").val(ui.item.value); // add the selected id
                        $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #hfTherapeuticInjection").val(ui.item.id);
                        Immunization_TherapeuticInjection.IsChangeTherapeuticInjection = true;
                        Immunization_TherapeuticInjection.TherapueticInjectionChange();
                        // GetNotesTemplates($("#pnlClinicalNotes #txtProvider"), ui.item.id);
                        Favorite_TherapueticInjection_Detail.TherapueticInjectionSelect(ui.item.id);
                    }, 100);
                }
            });
        });
    },
    TherapueticOnBlur: function (obj) {
        utility.ValidateAutoComplete(obj, 'frmFavoriteTherapueticInjectionDetail #hfTherapeuticInjection', true);
        if ($(obj).val() != "") {
            Favorite_TherapueticInjection_Detail.onBlurTherapueticInjection();
        }
    },

    loadEntityProvider: function (entityId) {
        var objDeffered = $.Deferred();
        var data = "entityID=" + entityId;
        if (entityId != null && entityId > 0) {

            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + ' #ddlProvider');
                var $providerHiddenDdl = $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + ' #ddlHiddenFavProvider');

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
                if (Favorite_TherapueticInjection_Detail.ProviderIds != '') {
                    var Providers = Favorite_TherapueticInjection_Detail.ProviderIds.split(",");
                    Favorite_TherapueticInjection_Detail.providerCheckedIds = Providers;
                    $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + ' #ddlProvider').val(Providers);
                }

            }).then(function () {
                //Intialized in onhidden spacialty ddl.
                Favorite_TherapueticInjection_Detail.IntializeMultiSelectDropDownProviders();
                objDeffered.resolve();

            });
        }
        else {
            objDeffered.resolve();
        }
        return objDeffered;
    },
    ValidateFavVaccineDetail: function () {
        $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + ' #frmFavoriteTherapueticInjectionDetail')
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
                                  message: 'Specify a Entity for the Favorite Therapeutic Injection and try again. '
                              },
                          }
                      },

                      FavoriteListName: {
                          group: '.col-sm-4',
                          validators: {
                              notEmpty: {
                                  message: 'Specify a Name for the Favorite Therapeutic Injection and try again. '
                              }
                          }
                      },
                  }
              }).on('success.form.bv', function (e) {

                  e.preventDefault();
                  Favorite_TherapueticInjection_Detail.FavVaccineSave();

              });
    },
    FillFavVaccineDetail: function (FavoritiesListId) {
        Favorite_Vaccine.LoadFavImmunization_Detail_DBCALL(FavoritiesListId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.FavImmunizationCount > 0) {
                    Favorite_TherapueticInjection_Detail.BindFavVaccineData(response.FavImmunization_JSON);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    BindFavVaccineData: function (FavImmunization_JSON) {
        $("#" + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #lstEntityId").val(FavImmunization_JSON[0].EntityId);
        $("#" + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #txtFavoriteListName").val(FavImmunization_JSON[0].FavoriteListName);
        if (FavImmunization_JSON[0].ProviderIds != '') {
            $("#" + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #ddlProvider").val(FavImmunization_JSON[0].ProviderIds.split(','));
            $("#" + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #ddlProvider").multiselect("refresh");
            $("#" + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #ddlProvider").multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                maxHeight: 247
            });
            var Providers = FavImmunization_JSON[0].ProviderIds.split(",");
            Favorite_TherapueticInjection_Detail.providerCheckedIds = Providers;
        }
        $("#" + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #IsActive").prop("checked", FavImmunization_JSON[0].IsActive);

        $.each(FavImmunization_JSON, function (i, item) {


            $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #ulFavTherapueticInjection").append('<li id=' + item.InjectionId + ' >' + item.VaccineName + '<a class="removeIconListHover" onclick="Favorite_TherapueticInjection_Detail.DeleteTheapueticInjection(' + item.InjectionId + ')"><i class="fa fa-times"></i></a></li>');
            //class="tooltipstered" data-title="' + liTitle + '"

            var popover;
            popover = '<strong><p class="mb-none">Injection: ' + item.VaccineName + '</p></strong>'
                        + (item.VaccineCVX != ("" || null) ? '<span class="pull-left">CVX: ' + item.VaccineCVX + '</span> <br />' : '')
                        + (item.CPTCode != "" ? '<span>CPT Code: ' + item.CPTCode + '</span> <br />' : '')
                        + (item.AdministeredCode != "" ? '<span class="pull-left">Administered Code: ' + item.AdministeredCode + '</span>' : '');


            $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #ulFavTherapueticInjection li#" + item.InjectionId).tooltipster({
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

    },
    FavVaccineSave: function () {
        if (globalAppdata['AppUserName'] == DefaultUser) {
            $("#" + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #lstEntityId").enable = true;
            $("#" + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail").bootstrapValidator('revalidateField', 'EntityId');
        }
        if ($('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #ulFavTherapueticInjection li").length == 0) {
            utility.DisplayMessages("Select Therpuetic Injection", 2);
        }
        else {
            AppPrivileges.GetFormPrivileges("Favorites_TherapueticInjection", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    var ProviderIds = $('#' + Favorite_TherapueticInjection_Detail.params.PanelID).find('#ddlProvider option:Selected').map(function () {
                        return this.value;
                    }).get().join(',');
                    if (ProviderIds != '') {
                        Favorite_TherapueticInjection_Detail.saveFavVaccine_DBCALL().done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                utility.DisplayMessages(response.Message, 1);
                                Favorite_TherapueticInjection_Detail.UnLoadTab();
                                Favorite_TherapueticInjection.LoadFavImmunization('Therapuetic');
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                    else {
                        Favorite_TherapueticInjection_Detail.validateProvider(1);
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
        var self = $('#' + Favorite_TherapueticInjection_Detail.params.PanelID);
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

        var TherapueticIds = "";
        $.each($('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #ulFavTherapueticInjection li"), function (i, item) {
            TherapueticIds += $(item).attr("id") + ",";
        });
        objDetail["TherapueticIds"] = TherapueticIds.substring(0, TherapueticIds.length - 1);

        if (globalAppdata['AppUserName'] != DefaultUser) {
            objDetail["EntityId"] = -1;
        }
        objDetail["Type"] = 'Therapuetic';
        if (Favorite_TherapueticInjection_Detail.params.FavoritiesListId != null) {
            objDetail["FavoritiesListId"] = Favorite_TherapueticInjection_Detail.params.FavoritiesListId;
        } else {
            objDetail["FavoritiesListId"] = -1;
        }
        if (Favorite_TherapueticInjection_Detail.params.mode == "Edit") {
            objDetail["IsActivePrevious"] = Favorite_TherapueticInjection_Detail.params.IsActivePrevious;
            objDetail["commandType"] = "UPDATE_FAvVaccine";
        } else {
            objDetail["commandType"] = "SAVE_FavVaccine";
        }

        var data = JSON.stringify(objDetail);
        return MDVisionService.APIService(data, "FavoriteList", "Immunization");

    },

    IntializeMultiSelectDropDownProviders: function () {
        $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + ' #ddlProvider').multiselect('destroy');
        $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + ' #ddlProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 200,
            nonSelectedText: 'Select',
            selectAll: false,
            onDropdownHide: function (event) {
            },
            onChange: function () {
                Favorite_TherapueticInjection_Detail.CheckProviderValidation();
            },
        });
    },
    CheckProviderValidation: function () {
        var self = $("#" + Favorite_TherapueticInjection_Detail.params.PanelID);
        var ProviderIds = self.find('#ddlProvider option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        var strProviderIds = ProviderIds;
        Favorite_TherapueticInjection_Detail.providerCheckedIds = ProviderIds;
        if (Favorite_TherapueticInjection_Detail.providerCheckedIds != '') {
            Favorite_TherapueticInjection_Detail.validateProvider(2);
        } else {
            Favorite_TherapueticInjection_Detail.validateProvider(1);
        }
    },
    validateProvider: function (operationid) {
        var self = "#" + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail";
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
            $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #ddlVaccine").attr("disabled", false);
            var self = $('#' + Favorite_TherapueticInjection_Detail.params.PanelID);
            self.find('.Vaccine > select').attr('ddlist', 'GetVaccine');
            var data = "IsActive=&StrID=" + $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + ' #ddlCategory').val() + "&StrID2=hx";
            self.find('.Vaccine').loadDropDowns(true, data).done(function () {
            });
        }
        else {
            var self = $('#' + Favorite_TherapueticInjection_Detail.params.PanelID);
            $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #ddlVaccine").attr("disabled", true);
            self.find('#ddlVaccine').attr('ddlist', '');
            $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #ddlVaccine").html('<option selected="selected" value=""> -Select- </option>');
        }
    },
    TherapueticInjectionSelect: function (TherapueticInjectionId) {
        if (TherapueticInjectionId != "") {
            Favorite_TherapueticInjection_Detail.GetCVXAndAdministeredCode_DB_CALL(0, TherapueticInjectionId, 'Therapuetic').done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var CVX = "";
                    var CPTCode = "";
                    if (response.VaccineDataCount > 0) {
                        var VaccineDataJSON = JSON.parse(response.VaccineDataJSON);
                        CVX = VaccineDataJSON[0].CVX;
                        CPTCode = VaccineDataJSON[0].CPTCode;
                        AdmCode = VaccineDataJSON[0].AdministeredCode;
                        $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #txtCVX").val(CVX);
                        $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #txtAdministeredCode").val(AdmCode);
                        $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #txtCptCode").val(CPTCode);
                    }
                    else {
                        $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #txtAdministeredCode").val("");
                        $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #txtCVX").val("");
                        $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #txtCptCode").val("");
                    }

                }
                else {
                    $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #txtAdministeredCode").val("");
                    $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #txtCVX").val("");
                    $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #txtCptCode").val("");
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else {
            $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #txtAdministeredCode").val("");
            $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #txtCVX").val("");
            $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #txtCptCode").val("");
        }
    },
    onBlurTherapueticInjection: function () {
        var TherapeuticInjectionId = $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #hfTherapeuticInjection").val();
        if (TherapeuticInjectionId != "") {
            if (!Favorite_TherapueticInjection_Detail.CheckAlreadyExists(TherapeuticInjectionId)) {
                var AdministeredCode = $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #txtAdministeredCode").val().trim();
                var InjectionText = $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #txtTherapeuticInjection").val().trim();
                var CVX = $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #txtCVX").val().trim();
                var CPTCode = $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #txtCptCode").val().trim();




                $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #ulFavTherapueticInjection").append('<li id=' + TherapeuticInjectionId + ' >' + InjectionText + '<a class="removeIconListHover" onclick="Favorite_TherapueticInjection_Detail.DeleteTheapueticInjection(' + TherapeuticInjectionId + ')"><i class="fa fa-times"></i></a></li>');
                //class="tooltipstered" data-title="' + liTitle + '"

                var popover;
                popover = '<strong><p class="mb-none">Injection: ' + InjectionText.replace(CPTCode + " - ", "") + '</p></strong>'
                        + (CVX != "" ? '<span class="pull-left">CVX: ' + CVX + '</span> <br />' : '')
                        + (CPTCode != "" ? '<span>CPT Code: ' + CPTCode + '</span> <br />' : '')
                        + (AdministeredCode != "" ? '<span class="pull-left">Administered Code: ' + AdministeredCode + '</span>' : '');


                $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #ulFavTherapueticInjection li#" + TherapeuticInjectionId).tooltipster({
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




                //clear filed
                $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #txtCVX").val("");
                $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #txtAdministeredCode").val("");
                $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #txtCptCode").val("");
                $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #txtTherapeuticInjection").val("");
                $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #hfTherapeuticInjection").val("");
            }
            else {
                //clear filed
                $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #txtCVX").val("");
                $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #txtAdministeredCode").val("");
                $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #txtCptCode").val("");
                $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #txtTherapeuticInjection").val("");
                $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #hfTherapeuticInjection").val("");
                utility.DisplayMessages("Already Exists", 2);
            }

        }
        else {
            //clear filed
            $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #txtCVX").val("");
            $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #txtAdministeredCode").val("");
            $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #txtCptCode").val("");
            $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #txtTherapeuticInjection").val("");
            $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #hfTherapeuticInjection").val("");
        }
    },
    CheckAlreadyExists: function (TherapueticInjectionId) {
        var found = false;
        $.each($('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #ulFavTherapueticInjection li"), function (i, item) {
            if ($(item).attr("id") == TherapueticInjectionId) {
                found = true;
            }
        });
        return found;
    },
    DeleteTheapueticInjection: function (TherapueticInjectionId) {
        $('#' + Favorite_TherapueticInjection_Detail.params.PanelID + " #frmFavoriteTherapueticInjectionDetail #ulFavTherapueticInjection li#" + TherapueticInjectionId).remove();
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
        if (Favorite_TherapueticInjection_Detail.params["FromAdmin"] == "0") {
            if (Favorite_TherapueticInjection_Detail.params != null && Favorite_TherapueticInjection_Detail.params.ParentCtrl != null) {
                UnloadActionPan(Favorite_TherapueticInjection_Detail.params.ParentCtrl, 'Favorite_TherapueticInjection_Detail');
            }
            else
                UnloadActionPan(null, 'Favorite_TherapueticInjection_Detail');
        }
        else {
            RemoveAdminTab();
        }
    }

}