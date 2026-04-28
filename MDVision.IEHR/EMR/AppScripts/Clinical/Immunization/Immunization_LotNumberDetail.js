Immunization_LotNumberDetail = {
    bIsFirstLoad: true,
    params: [],
    oldQuantity: -1,
    oldQuantityLeft: -1,
    oldQuantityLeft: -1,
    Load: function (params) {
        Immunization_LotNumberDetail.params = params;
        // Immunization_LotNumberDetail.params.mode = "Add";
        Immunization_LotNumberDetail.Type = "Incoming";
        if (Immunization_LotNumberDetail.params.PanelID != 'pnlImmunization_LotNumberDetail') {
            Immunization_LotNumberDetail.params.PanelID = Immunization_LotNumberDetail.params.PanelID + ' #pnlImmunization_LotNumberDetail';
        } else {
            Immunization_LotNumberDetail.params.PanelID = 'pnlImmunization_LotNumberDetail';
        }
        var self = $('#' + Immunization_LotNumberDetail.params.PanelID);

        if (Immunization_LotNumberDetail.bIsFirstLoad == true) {
            Immunization_LotNumberDetail.bIsFirstLoad = false;
            self.loadDropDowns(true).done(function () {

                if (Immunization_LotNumberDetail.isSuperAdmin()) {
                    Immunization_LotNumberDetail.enableDisableDropDownLists('ddlLotProvider', true);
                } else {
                    Immunization_LotNumberDetail.enableDisableDropDownLists('ddlLotProvider', false);
                    Immunization_LotNumberDetail.isEntitySelected(Immunization_LotNumberDetail.isSuperAdmin());
                }



                if (Immunization_LotNumberDetail.params.Type == "TherapeuticInjection") {
                    Immunization_LotNumberDetail.HideOrShowFields(Immunization_LotNumberDetail.params.Type);
                }
                else {

                    Immunization_LotNumberDetail.HideOrShowFields(Immunization_LotNumberDetail.params.Type);
                }
                Immunization_LotNumberDetail.ValidateLotNumberDetail();
                if (Immunization_LotNumberDetail.params.Type == "TherapeuticInjection") {
                    $('#' + Immunization_LotNumberDetail.params.PanelID + ' #frmImmunization_LotNumberDetail').data('bootstrapValidator').enableFieldValidators('TherapeuticInjection', true);
                    $('#' + Immunization_LotNumberDetail.params.PanelID + ' #frmImmunization_LotNumberDetail').data('bootstrapValidator').enableFieldValidators('LotVaccineIds', false);
                }
                else {
                    $('#' + Immunization_LotNumberDetail.params.PanelID + ' #frmImmunization_LotNumberDetail').data('bootstrapValidator').enableFieldValidators('TherapeuticInjection', false);
                    $('#' + Immunization_LotNumberDetail.params.PanelID + ' #frmImmunization_LotNumberDetail').data('bootstrapValidator').enableFieldValidators('LotVaccineIds', true);
                }
                Immunization_LotNumberDetail.LoadAllControls(self);
                if (Immunization_LotNumberDetail.params.Type == "TherapeuticInjection") {
                    $("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #hfTherapeuticInjection").val(Immunization_LotNumberDetail.params["TherapeuticInjectionId"]);
                    $("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #txtTherapeuticInjection").val(Immunization_LotNumberDetail.params["TherapeuticInjectionText"])
                    //$("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #TherapeuticInjectionDiv").addClass('disableAll');
                    // $("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #txtTherapeuticInjection").attr('disabled', 'disabled');
                    //Immunization_LotNumberDetail.PopulateManufacturer(0);
                    if (Immunization_LotNumberDetail.params["TherapeuticInjectionId"] != null && Immunization_LotNumberDetail.params["TherapeuticInjectionId"] != '') {
                        Immunization_LotNumberDetail.PopulateManufacturer(0, Immunization_LotNumberDetail.params["TherapeuticInjectionId"]);
                    }
                }
                else {
                    Immunization_LotNumberDetail.IntializeMultiSelectDropDown();
                    if (Immunization_LotNumberDetail.params["VaccineId"] != null && Immunization_LotNumberDetail.params["VaccineId"] != '') {
                        Immunization_LotNumberDetail.enableDisableDropDownLists('ddlVaccine', false);
                        $('#' + Immunization_LotNumberDetail.params.PanelID + ' #ddlVaccine').multiselect('clearSelection', false);
                        $('#' + Immunization_LotNumberDetail.params.PanelID + ' #ddlVaccine').multiselect('updateButtonText');
                        $('#' + Immunization_LotNumberDetail.params.PanelID + " #ddlVaccine").val(Immunization_LotNumberDetail.params["VaccineId"].split(','));
                        $('#' + Immunization_LotNumberDetail.params.PanelID + ' #ddlVaccine').multiselect("refresh");

                        $("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #VaccineDiv").addClass('disabledAll');
                        Immunization_LotNumberDetail.PopulateVISDate_VISURL_and_ManufacturerDropDown();
                    }
                    else {
                        $('#' + Immunization_LotNumberDetail.params.PanelID + ' #ddlVaccine').prop('disabled', false);
                        //Immunization_LotNumberDetail.LoadVaccineAutoCompelete(self);
                    }
                }

                if (Immunization_LotNumberDetail.params["VaccineLotNoId"] && (Immunization_LotNumberDetail.params["mode"] && Immunization_LotNumberDetail.params["mode"].toLowerCase() == "edit")) {
                    Immunization_LotNumberDetail.oldQuantity = -1;
                    Immunization_LotNumberDetail.oldQuantityLeft = -1;
                    Immunization_LotNumberDetail.FillLotDetails(Immunization_LotNumberDetail.params["VaccineLotNoId"]);
                }

                $('#frmImmunization_LotNumberDetail').data('serialize', $('#frmImmunization_LotNumberDetail').serialize());
            });
        }
        else {
            if (Immunization_LotNumberDetail.params.Type == "TherapeuticInjection") {
                $('#' + Immunization_LotNumberDetail.params.PanelID + ' #frmImmunization_LotNumberDetail').data('bootstrapValidator').enableFieldValidators('TherapeuticInjectionIds', true);
                $('#' + Immunization_LotNumberDetail.params.PanelID + ' #frmImmunization_LotNumberDetail').data('bootstrapValidator').enableFieldValidators('LotVaccineIds', false);
            }
            else {
                $('#' + Immunization_LotNumberDetail.params.PanelID + ' #frmImmunization_LotNumberDetail').data('bootstrapValidator').enableFieldValidators('TherapeuticInjectionIds', false);
                $('#' + Immunization_LotNumberDetail.params.PanelID + ' #frmImmunization_LotNumberDetail').data('bootstrapValidator').enableFieldValidators('LotVaccineIds', true);
            }
        }
        $('#' + Immunization_LotNumberDetail.params.PanelID + ' #frmImmunization_LotNumberDetail #txtLotNo').focus();
        $('#frmImmunization_LotNumberDetail').data('serialize', $('#frmImmunization_LotNumberDetail').serialize());

    },
    enableDisableDropDownLists: function (ddlIds, isHide) {
        ddlCommaSeparatedIds = ddlIds.split(',');
        var parrentPanelId = "#" + Immunization_LotNumberDetail.params["PanelID"];
        $.each(ddlCommaSeparatedIds, function (index, Item) {
            if (isHide) {
                $(parrentPanelId + " #" + Item).multiselect('disable');
            }
            else {
                $(parrentPanelId + " #" + Item).multiselect('enable');
            }
        });
    },
    IntializeMultiSelectDropDown: function () {
        $('#' + Immunization_LotNumberDetail.params.PanelID + ' #frmImmunization_LotNumberDetail #ddlVaccine').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247

        });
    },

    BindAutocomplete: function (From, frmId, ctrlId, hfCtrlId, type, obj, NotRefreahAutocomplete) {
        var Inj = $(obj).val();
        utility.Keyupdelay(function () {
            Immunization_LotNumberDetail.GetTheraArray(Inj, type, From).done(function (response) {
                $("#" + frmId + " #" + ctrlId).autocomplete({
                    autoFocus: true,
                    source: response,
                    select: function (event, ui) {
                        setTimeout(function (From, frmId, ctrlId, hfCtrlId, type) {
                            if (From == "Immunization_LotNumberDetail" && type == "2") {
                                $("#" + frmId + " #" + hfCtrlId).val(ui.item.id);
                                $("#" + frmId + " #" + ctrlId).val(ui.item.value);
                                Immunization_TherapeuticInjection.IsChangeTherapeuticInjection = true;
                                Immunization_LotNumberDetail.PopulateManufacturer(0, ui.item.id);
                                Immunization_ImmunizationAddImmInj.GetVaccineInformation('Therapeutic', ui.item.id).done(function (response) {
                                    $('#' + Immunization_LotNumberDetail.params.PanelID + ' #frmImmunization_LotNumberDetail #txtNDCCode').val(response.NDCCode);
                                });
                            }
                            else if (From == "Immunization_TherapeuticInjection" && type == "2") {

                                $("#" + frmId + " #" + ctrlId).val(ui.item.value); // add the selected id
                                $("#" + frmId + " #" + hfCtrlId).val(ui.item.id);
                                Immunization_TherapeuticInjection.IsChangeTherapeuticInjection = true;
                                Immunization_TherapeuticInjection.TherapueticInjectionChange();
                                // GetNotesTemplates($("#pnlClinicalNotes #txtProvider"), ui.item.id);
                                Immunization_ImmunizationAddImmInj.GetVaccineInformation('Therapeutic', ui.item.id).done(function (response) {
                                    if (ctrlId == "txtTherapeuticInjection") {
                                        $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #txtDose').val(response.Dose);
                                        $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #ddlAmount').val(response.Amount);
                                    }
                                    else {
                                        $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #txtDoseHistory').val(response.Dose);
                                        $('#' + Immunization_TherapeuticInjection.params.PanelID + ' #ddlAmountHistory').val(response.Amount);
                                    }
                                });

                            }
                            else {
                                if (From == "OrderSet_TherapeuticDetail" && ctrlId == "txtTherapeuticInjection") {
                                    OrderSet_TherapeuticDetail.PopulateLotNumber(ui.item.id);
                                }
                                $("#" + frmId + " #" + hfCtrlId).val(ui.item.id);
                                $("#" + frmId + " #" + ctrlId).val(ui.item.value);
                                Immunization_ImmunizationAddImmInj.GetVaccineInformation('Therapeutic', ui.item.id).done(function (response) {
                                    if (ctrlId == "txtTherapeuticInjection") {
                                        $('#' + frmId + ' #txtDose').val(response.Dose);
                                        $('#' + frmId + ' #ddlAmount').val(response.Amount);
                                    }
                                    else {
                                        $('#' + frmId + ' #txtDoseHistory').val(response.Dose);
                                        $('#' + frmId + ' #ddlAmountHistory').val(response.Amount);
                                    }
                                });
                            }

                        }, 100, From, frmId, ctrlId, hfCtrlId, type);
                    }
                });

                $("#" + frmId + " #" + ctrlId).autocomplete("search");

            });
        });
    },
    GetTheraArray: function (Inj, type, From) {
        var AllImmThera = [];
        var IsValid = false;

        if (Inj != null && Inj.length > 2) {
            IsValid = true;
        }

        else {
            IsValid = false;
        }



        var dfd = new $.Deferred();
        if (IsValid) {
            // serach parameter , class name, command name of class
            Immunization_LotNumberDetail.GetImmAndTheraArray_DBCALL(Inj, type, From).done(function (responseData) {
                if (responseData.status != false) {
                    responseData = JSON.parse(responseData)
                    if (responseData.VaccineNameCount > 0) {
                        var VaccineNamEJSON = responseData.VaccineName_JSON;
                        $.each(VaccineNamEJSON, function (i, item) {
                            AllImmThera.push({ id: item.ImmunizationId, value: item.ImmunizationName, Type: item.Type });
                        });
                    }
                }

                dfd.resolve(AllImmThera);
            });
        }
        else {
            dfd.resolve(AllImmThera);
        }

        return dfd.promise();

    },
    GetImmAndTheraArray_DBCALL: function (ImmThera, Type, From) {

        var objData = new Object();
        objData["ImmunizationName"] = ImmThera;
        objData["Type"] = Type;
        if (typeof From != typeof undefined && From != null && (From == "Immunization_LotNumberDetail" || From == "Immunization_TherapeuticInjection" || From == "OrderSet_TherapeuticDetail")) {
            objData["CptBaseSearch"] = true;
        }
        else {
            objData["CptBaseSearch"] = false;
        }

        objData["commandType"] = "Get_ImmAndThera_Array";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "AddVaccineAndTherapeutic");
    },
    HideOrShowFields: function (Type) {
        if (Immunization_LotNumberDetail.params.Type == "TherapeuticInjection") {
            if (!$("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #VaccineDiv").hasClass("hidden")) {
                $("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #VaccineDiv").addClass("hidden")
            }
            if (!$("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #VISDateDiv").hasClass("hidden")) {
                $("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #VISDateDiv").addClass("hidden")
            }
            if (!$("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #ViewDocumDiv").hasClass("hidden")) {
                $("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #ViewDocumDiv").addClass("hidden")
            }
            if ($("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #TherapeuticInjectionDiv").hasClass("hidden")) {
                $("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #TherapeuticInjectionDiv").removeClass("hidden")
            }
        }
        else {
            if ($("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #VaccineDiv").hasClass("hidden")) {
                $("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #VaccineDiv").removeClass("hidden")
            }
            if ($("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #VISDateDiv").hasClass("hidden")) {
                $("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #VISDateDiv").removeClass("hidden")
            }
            if ($("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #ViewDocumDiv").hasClass("hidden")) {
                $("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #ViewDocumDiv").removeClass("hidden")
            }
            if (!$("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #TherapeuticInjectionDiv").hasClass("hidden")) {
                $("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #TherapeuticInjectionDiv").addClass("hidden")
            }
        }

    },
    QuantityChange: function (Quantity) {
        if (Quantity == "") {
            Quantity = -1;
        }
        if (Quantity > 0) {
            if (Immunization_LotNumberDetail.params["mode"] && Immunization_LotNumberDetail.params["mode"].toLowerCase() == "edit") {
                var usedQuantity = Immunization_LotNumberDetail.oldQuantity - Immunization_LotNumberDetail.oldQuantityLeft;
                if (Quantity < usedQuantity) {
                    $("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #txtQuantity").val(Immunization_LotNumberDetail.oldQuantity);
                    $("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #txtQuantityLeft").val(Immunization_LotNumberDetail.oldQuantityLeft);
                    utility.DisplayMessages("Quantity should be greater then equal to used quantity which is: " + usedQuantity, 2);
                }
                else if (Quantity == usedQuantity) {
                    $("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #txtQuantityLeft").val(0);
                }
                else {
                    $("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #txtQuantityLeft").val(Quantity - usedQuantity);
                }
            }
            else {
                $("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #txtQuantityLeft").val(Quantity);
            }
        }
        else {
            if (Immunization_LotNumberDetail.params["mode"] && Immunization_LotNumberDetail.params["mode"].toLowerCase() == "edit") {
                $("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #txtQuantity").val(Immunization_LotNumberDetail.oldQuantity);
                $("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #txtQuantityLeft").val(Immunization_LotNumberDetail.oldQuantityLeft);
            }
            else {
                $("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #txtQuantity").val("");
                $("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #txtQuantityLeft").val("");
            }
            if (Quantity == 0) {
                utility.DisplayMessages("Quantity should be greater then zero", 2);
            }
        }
    },
    LoadVaccineAutoCompelete: function (self) {
        alert("LoadVaccineAutoCompelete");
    },
    FillLotDetails: function (VaccineLotNoId) {
        //AppPrivileges.GetFormPrivileges("Immunization_Lot Management", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        Immunization_LotNumberDetail.SearchLotNumber(VaccineLotNoId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var self = $('#' + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail");
                var LotNumberDataJSONData = JSON.parse(response.LotNumberLoad_JSON)[0];
                if (Immunization_LotNumberDetail.params.Type == "TherapeuticInjection") {
                    $.when(Immunization_LotNumberDetail.PopulateManufacturer(0, LotNumberDataJSONData.TherapeuticInjectionId)).then(function () {
                        utility.bindMyJSONByName(true, LotNumberDataJSONData, false, self).done(function () {
                            $('#' + Immunization_LotNumberDetail.params.PanelID + ' #txtTherapeuticInjection').val(LotNumberDataJSONData.VaccineName);
                            $('#' + Immunization_LotNumberDetail.params.PanelID + ' #ddlManufacturer').val(LotNumberDataJSONData.VacManufacturerId);
                            Immunization_LotNumberDetail.oldQuantity = LotNumberDataJSONData.Quantity;
                            Immunization_LotNumberDetail.oldQuantityLeft = LotNumberDataJSONData.QuantityLeft;
                            $('#' + Immunization_LotNumberDetail.params.PanelID + ' #ddlEntity').val(LotNumberDataJSONData['EntityId']);
                            $.when(Immunization_LotNumberDetail.isEntitySelected(Immunization_LotNumberDetail.isSuperAdmin())).then(function () {
                                Immunization_LotNumberDetail.enableDisableDropDownLists('ddlLotProvider', false);
                                $('#' + Immunization_LotNumberDetail.params.PanelID + ' #ddlLotProvider').multiselect('clearSelection', false);
                                $('#' + Immunization_LotNumberDetail.params.PanelID + ' #ddlLotProvider').multiselect('updateButtonText');
                                $('#' + Immunization_LotNumberDetail.params.PanelID + " #ddlLotProvider").val(LotNumberDataJSONData['ProviderIds'].split(','));
                                // Then refresh
                                $('#' + Immunization_LotNumberDetail.params.PanelID + ' #ddlLotProvider').multiselect("refresh");
                                $('#frmImmunization_LotNumberDetail').data('serialize', $('#frmImmunization_LotNumberDetail').serialize());
                            });
                        });
                    });
                }
                else {
                    $.when(Immunization_LotNumberDetail.PopulateManufacturer(LotNumberDataJSONData.LotVaccineIds.split(',')[0], 0)).then(function () {
                        utility.bindMyJSONByName(true, LotNumberDataJSONData, false, self).done(function () {
                            $('#' + Immunization_LotNumberDetail.params.PanelID + ' #ddlManufacturer').val(LotNumberDataJSONData.VacManufacturerId);

                            Immunization_LotNumberDetail.oldQuantity = LotNumberDataJSONData.Quantity;
                            Immunization_LotNumberDetail.oldQuantityLeft = LotNumberDataJSONData.QuantityLeft;

                            if (LotNumberDataJSONData.HTMLURL) {
                                $(" #pnlImmunization_LotNumberDetail #lnklblAdministerVaccine_VISURL").attr("href", LotNumberDataJSONData.HTMLURL);
                                if ($(" #pnlImmunization_LotNumberDetail #lnklblAdministerVaccine_VISURL").attr("href") !== "") {
                                    $(" #pnlImmunization_LotNumberDetail #lnklblAdministerVaccine_VISURL").attr('disabled', false);
                                }
                            }
                            else {
                                $(" #pnlImmunization_LotNumberDetail #lnklblAdministerVaccine_VISURL").attr("href", "");
                                $(" #pnlImmunization_LotNumberDetail #lnklblAdministerVaccine_VISURL").attr('disabled', 'disabled');
                            }
                            $('#' + Immunization_LotNumberDetail.params.PanelID + ' #ddlEntity').val(LotNumberDataJSONData['EntityId']);
                            $.when(Immunization_LotNumberDetail.isEntitySelected(Immunization_LotNumberDetail.isSuperAdmin())).then(function () {
                                Immunization_LotNumberDetail.enableDisableDropDownLists('ddlLotProvider', false);
                                $('#' + Immunization_LotNumberDetail.params.PanelID + ' #ddlLotProvider').multiselect('clearSelection', false);
                                $('#' + Immunization_LotNumberDetail.params.PanelID + ' #ddlLotProvider').multiselect('updateButtonText');
                                $('#' + Immunization_LotNumberDetail.params.PanelID + " #ddlLotProvider").val(LotNumberDataJSONData['ProviderIds'].split(','));
                                // Then refresh
                                $('#' + Immunization_LotNumberDetail.params.PanelID + ' #ddlLotProvider').multiselect("refresh");
                                $('#frmImmunization_LotNumberDetail').data('serialize', $('#frmImmunization_LotNumberDetail').serialize());
                            });

                            Immunization_LotNumberDetail.enableDisableDropDownLists('ddlVaccine', false);
                            $('#' + Immunization_LotNumberDetail.params.PanelID + ' #ddlVaccine').multiselect('clearSelection', false);
                            $('#' + Immunization_LotNumberDetail.params.PanelID + ' #ddlVaccine').multiselect('updateButtonText');
                            $('#' + Immunization_LotNumberDetail.params.PanelID + " #ddlVaccine").val(LotNumberDataJSONData['LotVaccineIds'].split(','));
                            $('#' + Immunization_LotNumberDetail.params.PanelID + ' #ddlVaccine').multiselect("refresh");
                        });
                    });
                }

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
    },

    LoadAllControls: function (self) {
        utility.CreateDatePicker(Immunization_LotNumberDetail.params.PanelID + ' #frmImmunization_LotNumberDetail #dtpExpiryDate', function (ev) { }, false, "frmImmunization_LotNumberDetail", false);
    },
    UnLoadTab: function () {
        //var objDeffered = $.Deferred();
        //if (Immunization_LotNumberDetail.params["FromAdmin"] == "0") {
        //    if (Immunization_LotNumberDetail.params != null && Immunization_LotNumberDetail.params.ParentCtrl != null) {
        //        UnloadActionPan(Immunization_LotNumberDetail.params.ParentCtrl, 'Immunization_LotNumberDetail');
        //    }
        //    else
        //        UnloadActionPan(null, 'Immunization_LotNumberDetail');
        //}
        //else {

        //    RemoveAdminTab();
        //}
        //return objDeffered;
        utility.UnLoadDialog("frmImmunization_LotNumberDetail", function () {
            UnloadActionPan(Immunization_LotNumberDetail.params["ParentCtrl"], "Immunization_LotNumberDetail");
        }, function () {
            UnloadActionPan(Immunization_LotNumberDetail.params["ParentCtrl"], "Immunization_LotNumberDetail");
        });
    },
    // AutoComplete for Vaccine
    // Author: M Ahmad Imran | Date: June 08, 2016
    bindVaccineAutoComplete: function () {
        alert("bindVaccineAutoComplete");
    },

    // AutoComplete for Manufacturer
    // Author: M Ahmad Imran | Date: June 08, 2016
    bindManufacturerAutoComplete: function () {
        var ManufacturerText = $('#' + Immunization_LotNumberDetail.params.PanelID + ' #txtManufacturer');
        var allVaccines = [];
        if (ManufacturerText.val().length > 2) {
            utility.Keyupdelay(function () {
                Immunization_LotNumberDetail.loadManufacturers_DBCall(ManufacturerText.val()).done(function (response) {
                    var response = JSON.parse(response);
                    if (response.status != false) {
                        if (response.ManufacturerCount > 0) {
                            var Vaccines = JSON.parse(response.ManufacturerLoad_JSON);

                            $.each(Vaccines, function (i, item) {
                                allVaccines.push({ id: item.VaccineManufacturerId, value: item.Manufacturer });
                            });
                            ManufacturerText.autocomplete({
                                autoFocus: true,
                                source: allVaccines,
                                select: function (event, ui) {
                                    setTimeout(function () {
                                        $("#" + Immunization_LotNumberDetail.params.PanelID + " #txtManufacturer").val(ui.item.value);
                                        $("#" + Immunization_LotNumberDetail.params.PanelID + " #txtManufacturerId").val(ui.item.id);

                                    }, 100);
                                }
                            });
                        }
                    }
                    ManufacturerText.autocomplete("search");
                });
            });
        }

    },
    // Search for Vaccine
    // Author: M Ahmad Imran | Date: June 08, 2016
    OpenVacineSearch: function () {
        alert("bindVaccineAutoComplete");
    },

    // DB call to get Vaccine
    // Author: M Ahmad Imran | Date: June 08, 2016
    loadVaccines_DBCall: function (VaccineText) {
        var objData = new Object();
        objData["SearchText"] = VaccineText;
        objData["commandType"] = "get_all_Vaccines";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "LotNumber");
    },
    // DB call to get Manufacturer
    // Author: M Ahmad Imran | Date: June 08, 2016
    loadManufacturers_DBCall: function (ManufacturerText) {
        var objData = new Object();
        objData["SearchText"] = ManufacturerText;
        objData["commandType"] = "get_all_Manufacturers";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "LotNumber");
    },
    PopulateVISDate_VISURL_and_ManufacturerDropDown: function () {
        var vaccineId = "";
        var dfd = new $.Deferred();
        //vaccineId = $("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #txtVaccineId").val();

        vaccineId = $("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail").find('#ddlVaccine option:Selected').map(function () {
            return this.value;
        }).get().join(',').split(',')[0];

        if (vaccineId) {
            var forModule = "ADMINISTER";
            Immunization_LotNumberDetail.PopulateVISURL(vaccineId, forModule);
            Immunization_LotNumberDetail.PopulateVISDate(vaccineId, forModule);
            Immunization_LotNumberDetail.PopulateManufacturer(vaccineId, 0);

            Immunization_ImmunizationAddImmInj.GetVaccineInformation('immunization', vaccineId).done(function (response) {
                $('#' + Immunization_LotNumberDetail.params.PanelID + ' #frmImmunization_LotNumberDetail #txtNDCCode').val(response.NDCCode);
                $('#frmImmunization_LotNumberDetail').data('serialize', $('#frmImmunization_LotNumberDetail').serialize());
            });

            dfd.resolve();
        }
        else {
            dfd.resolve();
        }
        return dfd;
    },
    PopulateManufacturer: function (VaccineId, TherapeuticId) {
        var dfd = $.Deferred();
        $('#' + Immunization_LotNumberDetail.params.PanelID + ' #ddlManufacturer').prop('disabled', false);
        var self = $('#' + Immunization_LotNumberDetail.params.PanelID);
        self.find('.Manufacturer > select').attr('ddlist', 'GetVaccineManufacturer');
        var data = "StrID=" + VaccineId + "&StrID2=" + TherapeuticId;
        //var data = "StrID=" + vaccineId + "&StrID2=" + forModule;
        self.find('.Manufacturer').loadDropDowns(true, data).done(function () {
            $('#frmImmunization_LotNumberDetail').data('serialize', $('#frmImmunization_LotNumberDetail').serialize());
            dfd.resolve();
        });
        return dfd;
    },
    PopulateVISDate: function (vaccineId, forModule) {
        var objData = new Object();
        objData["VaccineID"] = vaccineId;
        objData["commandType"] = "Get_VISDate";

        var data = JSON.stringify(objData);
        MDVisionService.APIService(data, "MEDICAL", "Immunization").done(function (response) {
            response = JSON.parse(response);
            if (response != null && response.status === true) {
                $("#" + Immunization_LotNumberDetail.params.PanelID + ' #frmImmunization_LotNumberDetail #dtpVISDate').val(response.VISDate);
                //$("#" + Immunization_LotNumberDetail.params.PanelID + " #VisDateId").val(response.VISId);
            }
            else {
                $(' #dpAdministerVaccine_VISDate').val("");
                // $("#" + Immunization_LotNumberDetail.params.PanelID + " #VisDateId").val(0);
            }
            $('#frmImmunization_LotNumberDetail').data('serialize', $('#frmImmunization_LotNumberDetail').serialize());
        });

    },
    PopulateVISURL: function (vaccineId, forModule) {
        var objData = new Object();
        objData["VaccineID"] = vaccineId;
        objData["commandType"] = "get_visurl";
        var data = JSON.stringify(objData);
        MDVisionService.APIService(data, "MEDICAL", "Immunization").done(function (response) {
            response = JSON.parse(response);
            if (response != null && response.VIS_url !== "") {
                $(" #pnlImmunization_LotNumberDetail #lnklblAdministerVaccine_VISURL").attr("href", response.VIS_url);
                if ($(" #pnlImmunization_LotNumberDetail #lnklblAdministerVaccine_VISURL").attr("href") !== "") {
                    $(" #pnlImmunization_LotNumberDetail #lnklblAdministerVaccine_VISURL").attr('disabled', false);
                }
            }
            else {
                $(" #pnlImmunization_LotNumberDetail #lnklblAdministerVaccine_VISURL").attr("href", "");
                $(" #pnlImmunization_LotNumberDetail #lnklblAdministerVaccine_VISURL").attr('disabled', 'disabled');
            }
            $('#frmImmunization_LotNumberDetail').data('serialize', $('#frmImmunization_LotNumberDetail').serialize());
        });
    },
    LotNumberReset: function () {
        $('#' + Immunization_LotNumberDetail.params["PanelID"] + ' #frmImmunization_LotNumberDetail').resetAllControls();
    },
    ImmunizationLotNumberDetailSave: function () {

        var strMessage = "";
        var self = $("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail");
        var myJSON = self.getMyJSONByName();
        myJSON = JSON.parse(myJSON);
        var ProviderIds = self.find('#ddlLotProvider option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        myJSON.ProviderIds = ProviderIds;
        var VaccineIds = self.find('#ddlVaccine option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        var checkCondition = false;
        if (Immunization_LotNumberDetail.params.Type == "TherapeuticInjection") {
            if (ProviderIds != '') {
                checkCondition = true;
            }
        }
        else {
            if (ProviderIds != '' && VaccineIds != '') {
                checkCondition = true;
            }
        }
        if (checkCondition) {
            if (Immunization_LotNumberDetail.params.mode == "Add") {
                //AppPrivileges.GetFormPrivileges("Immunization_Lot Management", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                //    if (strMessage == "") {
                Immunization_LotNumberDetail.SaveImmunizationLotNumberDetail(myJSON).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        Immunization_LotNumber.AddOrUpdateLot = true;
                        utility.DisplayMessages(response.message, 1);
                        Immunization_LotNumber.LoadLotNumberList();
                        UnloadActionPan(Immunization_LotNumberDetail.params["ParentCtrl"], "Immunization_LotNumberDetail");
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
                //    } else {
                //        utility.DisplayMessages(strMessage, 2);
                //    }
                //});
            }
            else if (Immunization_LotNumberDetail.params.mode == "Edit") {
                //AppPrivileges.GetFormPrivileges("Immunization_Lot Management", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                //    if (strMessage == "") {
                var selectedSchTypeId = $("#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail #ddlScheduleType option:selected").val();
                Immunization_LotNumberDetail.UpdateImmunizationLotNumberDetail(myJSON, Immunization_LotNumberDetail.params.VaccineGroupId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        Immunization_LotNumber.AddOrUpdateLot = true;
                        utility.DisplayMessages(response.message, 1);
                        Immunization_LotNumber.LoadLotNumberList();
                        UnloadActionPan(Immunization_LotNumberDetail.params["ParentCtrl"], "Immunization_LotNumberDetail");
                    }
                    else {
                        utility.DisplayMessages(response.message, 3);
                    }
                });
                //    } else {
                //        utility.DisplayMessages(strMessage, 2);
                //    }
                //});
            }
        }
        else {
            if (ProviderIds == '') {
                Immunization_LotNumberDetail.validateProvider(1, 'divLotProvider');
            }

            if (Immunization_LotNumberDetail.params.Type != "TherapeuticInjection") {
                if (VaccineIds == '') {
                    Immunization_LotNumberDetail.validateProvider(1, 'VaccineDiv');
                }
            }

        }
    },
    ValidateLotNumberDetail: function () {
        //$('#' + Immunization_LotNumberDetail.params.PanelID + ' #frmImmunization_LotNumberDetail')
        //.bootstrapValidator('destroy');
        $('#' + Immunization_LotNumberDetail.params.PanelID + ' #frmImmunization_LotNumberDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   LotProvider: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   TherapeuticInjection: {
                       group: '.col-sm-4',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   LotVaccineIds: {
                       group: '.col-sm-4',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   LotNo: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   VacManufacturerId: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   ExpiryDate: {
                       group: '.col-sm-4',
                       validators: {
                           date: {
                               format: 'MM/DD/YYYY',
                               message: ''
                           },
                           notEmpty: {
                               message: ''
                           },
                       }
                   },
                   Quantity: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                           integer: {
                               message: 'Quantity should be numeric',
                           }
                       }
                   }
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            Immunization_LotNumberDetail.ImmunizationLotNumberDetailSave();
        });
    },
    SaveImmunizationLotNumberDetail: function (ImmunizationLotNumerData) {
        var objData = ImmunizationLotNumerData;
        objData["commandType"] = "save_immunizationlotnumber";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ImmunizationLotNumber");
    },
    UpdateImmunizationLotNumberDetail: function (ImmunizationLotNumerData) {
        var objData = ImmunizationLotNumerData;
        objData["VaccineLotNoId"] = Immunization_LotNumberDetail.params["VaccineLotNoId"];
        objData["commandType"] = "update_immunizationlotnumber";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ImmunizationLotNumber");
    },
    ValidateVaccine: function ($obj) {
        alert("ValidateVaccine");
    },
    SearchLotNumber: function (VaccineLotNoId, PageNumber, RowsPerPage) {
        var objData = new Object();
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 1000;
        }
        objData["VaccineLotNoId"] = VaccineLotNoId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["commandType"] = "get_lotnumber_by_id";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ImmunizationLotNumber");
    },
    isEntitySelected: function (isSuperAdmin) {
        var objDeffered = $.Deferred();
        selectedEntity = $('#' + Immunization_LotNumberDetail.params.PanelID + ' #ddlEntity option:selected').val();
        if (isSuperAdmin == false) {
            selectedEntity = globalAppdata["SeletedEntityId"];
        }

        $.when(Immunization_LotNumberDetail.loadEntityProvider(selectedEntity)).then(function () {

            Immunization_LotNumberDetail.IntializeMultiSelectDropDownProviders();

            objDeffered.resolve();

        });

        return objDeffered;
    },
    loadEntityProvider: function (entityId) {
        var objDeffered = $.Deferred();
        var data = "entityID=" + entityId;
        if (entityId != null && entityId > 0) {

            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $('#' + Immunization_LotNumberDetail.params.PanelID + ' #ddlLotProvider');
                //Empty both the providers ddls.
                $providerDdl.empty();


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
                    }
                    $('#frmImmunization_LotNumberDetail').data('serialize', $('#frmImmunization_LotNumberDetail').serialize());
                });
            }).then(function () {
                Immunization_LotNumberDetail.IntializeMultiSelectDropDownProviders();
                Immunization_LotNumberDetail.enableDisableDropDownLists('ddlLotProvider', false);
                $('#frmImmunization_LotNumberDetail').data('serialize', $('#frmImmunization_LotNumberDetail').serialize());
                objDeffered.resolve();
            });
        }
        else {
            //disable multiselect
            Immunization_LotNumberDetail.enableDisableDropDownLists('ddlLotProvider', true);
            $('#frmImmunization_LotNumberDetail').data('serialize', $('#frmImmunization_LotNumberDetail').serialize());
            objDeffered.resolve();
        }
        return objDeffered;
    },
    isSuperAdmin: function () {
        if (globalAppdata['AppUserName'] == DefaultUser) {
            $('#' + Immunization_LotNumberDetail.params.PanelID + ' #entityDDL').show();
            return true;
        } else {
            $('#' + Immunization_LotNumberDetail.params.PanelID + ' #entityDDL').hide();
            return false;
        }
    },
    IntializeMultiSelectDropDownProviders: function () {
        $('#' + Immunization_LotNumberDetail.params.PanelID + ' #ddlLotProvider').multiselect('destroy');
        $('#' + Immunization_LotNumberDetail.params.PanelID + ' #ddlLotProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: '- Select -',
            selectAll: false,
        });
    },
    CheckProviderValidation: function () {
        var self = $("#" + Immunization_LotNumberDetail.params.PanelID);
        var ProviderIds = self.find('#ddlLotProvider option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        var strProviderIds = ProviderIds;
        //Immunization_LotNumberDetail.providerCheckedIds = ProviderIds;
        if (ProviderIds != '') {
            Immunization_LotNumberDetail.validateProvider(2, 'divLotProvider');
        } else {
            Immunization_LotNumberDetail.validateProvider(1, 'divLotProvider');
        }


        var VaccineIds = self.find('#ddlVaccine option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        var strVaccineIds = VaccineIds;
        //Immunization_LotNumberDetail.providerCheckedIds = ProviderIds;
        if (VaccineIds != '') {
            Immunization_LotNumberDetail.validateProvider(2, 'VaccineDiv');
        } else {
            Immunization_LotNumberDetail.validateProvider(1, 'VaccineDiv');
        }
    },
    validateProvider: function (operationid, divId) {
        var self = "#" + Immunization_LotNumberDetail.params.PanelID + " #frmImmunization_LotNumberDetail";
        $(self + " #" + divId).find("i").remove();
        if (operationid == 1) {
            $(self + " #" + divId + " .multiselect").css("border-color", "#cc2724");
            $(self + " #" + divId).find(".control-label").css("color", "#cc2724");
            $(self + " #" + divId).find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-remove" data-bv-icon-for="Race" style="display: block;"></i>');
        } else if (operationid == 2) {
            $(self + " #" + divId + " .multiselect").css("border-color", "#3c763d");
            $(self + " #" + divId).find(".control-label").css("color", "#3c763d");
            $(self + " #" + divId).find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-ok" data-bv-icon-for="Race" style="display: block;"></i>');
        } else {
            $(self + " #" + divId + " .multiselect").css("border-color", "#ccc");
            $(self + " #" + divId).find(".control-label").css("color", "#000000");
        }
    },

}