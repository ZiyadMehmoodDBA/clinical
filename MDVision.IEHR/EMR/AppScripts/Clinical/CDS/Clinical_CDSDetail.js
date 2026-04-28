ClinicalCDSDetail = {
    params: [],
    bIsFirstLoad: true,
    questionnairId: -1,
    labResultId: -1,
    QuestionnaireDropDownValues: [],
    providerCheckedIds: [],
    ProviderIds: '',
    Load: function (params) {
        ClinicalCDSDetail.labResultId = -1;
        BackgroundLoaderShow(true);
        params["TabID"] = 'ClinicalCDSDetail';
        ClinicalCDSDetail.params = params;

        $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #hfPatientId").val($("div#PatientProfile #hfPatientId").val());
        var self = $("#" + ClinicalCDSDetail.params["PanelID"] + " #tblClinicalCDSDetail");

        if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
            $("#" + ClinicalCDSDetail.params["PanelID"] + " #tblClinicalCDSDetail #ddlEntity").attr('disabled', 'disabled');
        }

        // Set Title Explicitly if it's passed as Parameter
        if (ClinicalCDSDetail.params.Title != null)
            $("#" + ClinicalCDSDetail.params["PanelID"] + " #headingTitle").text(ClinicalCDSDetail.params.Title);
  

        ClinicalCDSDetail.GetCDSCompleteAllergies();
        //End//03-03-2016//Ahmad Raza//Loading Allergies for AutoComplete

        //Start//03-03-2016//Ahmad Raza//Loading Medications for AutoComplete

        ClinicalCDSDetail.GetCDSMedications();

        //End//03-03-2016//Ahmad Raza//Loading Medications for AutoComplete
        //Start//14-05-2016//Ahmad Raza//Loading Lab Results for AutoComplete
        //var elementLabResults = $("#" + ClinicalCDSDetail.params["PanelID"] + " #txtCDSLabResults");
        // EMRUtility.bindAutoCompleteLabResults(elementLabResults);
       // ClinicalCDSDetail.GetCDSLabResults();
        //End//14-05-2016//Ahmad Raza//Loading Lab Results for AutoComplete

        ClinicalCDSDetail.bindInsuranceAutocomplete();

        //Start 02-03-2016 Humaira Yousaf to load dropdowns
        if (ClinicalCDSDetail.bIsFirstLoad == true) {
            ClinicalCDSDetail.bIsFirstLoad = false;
            self.loadDropDowns(true).done(function () {
                var selectedEntity = globalAppdata["SeletedEntityId"];
                ClinicalCDSDetail.loadEntityProvider(selectedEntity);

                $.when(ClinicalCDSDetail.LoadingDropDowns('ClinicalCDSDetail')).then(function () {
                    //Start 04-03-2016 Humaira Yousaf to load CDS
                    ClinicalCDSDetail.loadCDS();
                    $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail").data('serialize', $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail").serialize());
                    //End 04-03-2016 Humaira Yousaf to load CDS

                    //Start//04-03-2016//Ahmad Raza//Logic to show/hide divs  on multilist selection change
                    $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSRuleType").multiselect({
                        includeSelectAllOption: true,
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                        onChange: function (element, checked) {
                            var selectedValue = $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSRuleType option:selected");
                            var selected = [];
                            $(selectedValue).each(function (index, val) {
                                selected.push($(this).text());
                            });

                            //    ClinicalCDSDetail.enableDisableReminderControls();
                            var unSelect = '';
                            $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail .comList").addClass('hidden');
                            $(selected).each(function (i, item) {
                                var sectionName = item;
                                unSelect += sectionName + ',';
                                $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #dv" + item.replace(/\s/g, '')).removeClass('hidden');
                            });
                        }
                    });
                    //End//04-03-2016//Ahmad Raza//Logic to show/hide divs  on multilist selection change
                    //Start//05-04-2016//Ahmad Raza//Creating multiselect for dropdowns
                    $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #lstCDSUserRoles").multiselect({
                        includeSelectAllOption: true,
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,

                    });
                    $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSTriggerLocation").multiselect({
                        includeSelectAllOption: true,
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,

                    });
                    $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #lstCDOrderSet").multiselect({
                        includeSelectAllOption: true,
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                        nonSelectedText: 'Select',

                    });
                    $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSSex").multiselect({
                        includeSelectAllOption: true,
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,

                    });
                    $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSEthnicity").multiselect({
                        includeSelectAllOption: true,
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,

                    });
                    $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSRace").multiselect({
                        includeSelectAllOption: true,
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,

                    });
                    $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSLanguage").multiselect({
                        includeSelectAllOption: true,
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,

                    });
                    //End//05-04-2016//Ahmad Raza//Creating multiselect for dropdowns
                    ClinicalCDSDetail.validateCDSDetail();
                });

            });
            
            Clinical_LabOrder.LoadLabs('ddlLabId', ClinicalCDSDetail.params.PanelID).done(function () {
               
            });

        }
        //End 02-03-2016 Humaira Yousaf to load dropdowns

        //Start 03-03-2016 Humaira Yousaf to load dropdowns
        utility.CreateDatePicker('ClinicalCDSDetail #tblClinicalCDSDetail #dtCDSRevisionDate', function () {
            //on-change callback method
        });
        utility.CreateDatePicker('ClinicalCDSDetail #tblClinicalCDSDetail #txtCDSRelease', function () {
            //on-change callback method
        });
        //End 03-03-2016 Humaira Yousaf to load dropdowns


        ClinicalCDSDetail.initializeKeypad();
    },

    // Medications
    GetCDSMedications: function () {
        
        var Ctrl = $('#' + ClinicalCDSDetail.params.PanelID + " input#txtCDSMedications");
        //var hfCtrl = $('#' + ClinicalCDSDetail.params.PanelID + " #hfPaymentBatch");
        var func = function () {
            return ClinicalCDSDetail.GetCDSDrugs(Ctrl.val())
        };
        var onSelect = function (e) {
            setTimeout(function () {
            EMRUtility.buildMedicationLi(e.id, e.value, e.rxnormId);
            $('#ClinicalCDSDetail #txtCDSMedications').data("kendoAutoComplete").value('');
                // e.preventDefault();
            }, 100);
        };
        utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", null, func, null, onSelect);
      

    },

    GetCDSDrugs: function (SearchDrug) {
        var AllCDSDrugs = [];
        var dfd = new $.Deferred();
        if (SearchDrug.length >= 3) {
            ClinicalCDSDetail.LoadCDSDrugs_DBCall(SearchDrug).done(function (responseData) {
                if (responseData.status == true) {

                    var MedDrugs = JSON.parse(responseData.MedicationLoad_JSON);
                    $.each(MedDrugs, function (i, item) {
                        AllCDSDrugs.push({ id: item.DrugId, value: item.MedicationName, Name: item.MedicationName, rxnormId: item.RxnormID });
                    });
                }

                dfd.resolve(AllCDSDrugs);
            });
        }
        else {
            dfd.resolve(AllCDSDrugs);
        }

        return dfd.promise();
    },

    LoadCDSDrugs_DBCall: function (DrugSearch) {
        return MDVisionService.PMSAPIService(DrugSearch, "MEDICAL", "GetCDSDrugs");
    },

    // End Medications

    // Allergies Auto Complete
    
    GetCDSCompleteAllergies: function () {
        
        var Ctrl = $('#' + ClinicalCDSDetail.params.PanelID + " input#txtCDSAllergies");
            
            var func = function () {
                return ClinicalCDSDetail.GetCDSCompleteAllergiesSearch(Ctrl.val())
            };
            var onSelect = function (e) {
                setTimeout(function () {
                    EMRUtility.buildAllergyLi(e.id, e.value, e.RefValue);
                    $('#ClinicalCDSDetail #txtCDSAllergies').data("kendoAutoComplete").value('');
                    // e.preventDefault();
                }, 100);
            };
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", null, func, null, onSelect);
      

        },

    GetCDSCompleteAllergiesSearch: function (SearchLabResults) {
    var AllCDSAllergies = [];
    var dfd = new $.Deferred();
    if (SearchLabResults.length >= 3) {
        ClinicalCDSDetail.LoadCDSLabResults_DBCall(SearchLabResults).done(function (responseData) {
            if (responseData.status == true) {

                var CDSLabResults = JSON.parse(responseData.allergiesLoad_JSON);
                $.each(CDSLabResults, function (i, item) {
                    AllCDSAllergies.push({ id: item.LookupId, value: item.Name, Name: item.Name, RefValue: item.LookupId });
                });
            }

            dfd.resolve(AllCDSAllergies);
        });
    }
    else {
        dfd.resolve(AllCDSAllergies);
    }

    return dfd.promise();
},

    LoadCDSLabResults_DBCall: function (DrugSearch) {
    return MDVisionService.PMSAPIService(DrugSearch, "MEDICAL", "GetCDSAllergies");
    },


    LoadingDropDowns: function (PanelId) {
        var methodName = ['GetRaceForDemographics', 'GetEthnicityForDemographics'];
        var dfd = new $.Deferred();
        BackgroundLoaderShow(true);
        MDVisionService.lookups(methodName, true, "").done(function (results) {
            var htmlRace = '';
            var htmlEthnicity = '';
            var races = JSON.parse(results['GetRaceForDemographics']);
            var ethnicities = JSON.parse(results['GetEthnicityForDemographics']);
            var ParentRacesArray = [];
            ParentRacesArray = jQuery.grep(races, function (value) {
                return value.ParentId == null || value.ParentId == 0 || value.ParentId == "0";
            });
            var ParentEthnicityArray = [];
            ParentEthnicityArray = jQuery.grep(ethnicities, function (value) {
                return value.ParentId == null || value.ParentId == 0 || value.ParentId == "0";
            });

            BackgroundLoaderShow(false);
            $.each(ParentRacesArray, function (j, result) {
                if (!result.ParentId) {
                    htmlRace += '<option refval="hidden" value="' + result.Id + '">' + result.Name + '</option>';
                    var childRaces = $.grep(races, function (a) {
                        return a.ParentId == result.Id;
                    });
                    $.each(childRaces, function (j, itm) {
                        htmlRace += '<option value="' + itm.Id + '">' + itm.Name + '</option>';
                    });
                }
                else
                    htmlRace += '<option value="' + result.Id + '">' + result.Name + '</option>';
            });
            $.each(ParentEthnicityArray, function (j, result) {
                if (!result.ParentId) {
                    htmlEthnicity += '<option refval="hidden" value="' + result.Id + '">' + result.Name + '</option>';
                    var childEthnicity = $.grep(ethnicities, function (a) {
                        return a.ParentId == result.Id;
                    });
                    $.each(childEthnicity, function (j, itm) {
                        htmlEthnicity += '<option value="' + itm.Id + '">' + itm.Name + '</option>';
                    });
                }
                else
                    htmlEthnicity += '<option value="' + result.Id + '">' + result.Name + '</option>';
            });
            $('#' + PanelId + ' #ddlCDSRace').html(htmlRace);
            $('#' + PanelId + ' #ddlCDSEthnicity').html(htmlEthnicity);
           
            dfd.resolve();
        });
        return dfd;
    },

    //CallCityState: function (control, field1, field2) {
    //    var zipcode = $('#ClinicalCDSDetail ' + control).val();
    //    var cityname = null;
    //    var statename = null;
    //    ClinicalCDSDetail.FillCityState(zipcode, cityname, statename).done(function (response) {
    //        if (response.status != false) {
    //            var citystate = JSON.parse(response.CITYSTATE_JSON);
    //            $('#ClinicalCDSDetail ' + field1).val(citystate.txtCity);
    //            $('#ClinicalCDSDetail ' + field2).val(citystate.txtState);
    //            //var self = $("#ClinicalCDSDetail");
    //            ////self.bindMyJSON(true, citystate, true);
    //            //utility.bindMyJSON(true, citystate, true, self);
    //            //ClinicalCDSDetail.ValidateProvider();
    //        }
    //        else {
    //            $('#ClinicalCDSDetail ' + field1).val('');
    //            $('#ClinicalCDSDetail ' + field2).val('');
    //        }
    //    });
    //},


    //Author: Ahmad Raza
    //Date :  04-03-2016
    //Reason: Function to apply bootstrap validations
    validateCDSDetail: function () {
        $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   CDSTitle: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   CDSReminderPeriod: {
                       group: '.col-xs-8',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   CDSReminderLength: {
                       group: '.col-xs-4',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   }
                   ,
                   CDSRecursivePeriod: {
                       group: '.col-xs-8',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   CDSRecursiveLength: {
                       group: '.col-xs-4',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   VitalLogicalOperator: {
                       group: '.col-sm-2',
                       enabled: true,
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
            if (e.type == "success") {
                ClinicalCDSDetail.CDSSave();
            }
            e.type = "";
        });

    },

    loadEntityProvider: function (entityId) {
        var objDeffered = $.Deferred();
        var data = "entityID=" + entityId;
        if (entityId != null && entityId > 0) {
            MDVisionService.lookups('GetEntityProvider', true, data).done(function (result) {
                result = JSON.parse(result["GetEntityProvider"]);
                var options = result;
                var $providerDdl = $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSProviders");
                var $providerHiddenDdl = $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail #ddlHiddenCDSProvider');
                $providerDdl.empty();
                $providerHiddenDdl.empty();
                $.each(options, function (i, item) {
                    if (item.Value != "" && typeof item.Value != 'undefined') {
                        $providerDdl.append(
                            $('<option/>', {
                                value: item.Value,
                                html: item.Name,
                                refname: item.RefName,
                                refvalue: item.RefValue
                            })
                        );
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
                if (ClinicalCDSDetail.ProviderIds != '') {
                    var Providers = ClinicalCDSDetail.ProviderIds.split(",");
                    ClinicalCDSDetail.providerCheckedIds = Providers;
                    $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail #ddlHiddenCDSProvider').val(Providers);
                }
            }).then(function () {
                //Intialized in onhidden spacialty ddl.
                ClinicalCDSDetail.IntializeMultiSelectDropDownProviders();
                objDeffered.resolve();
            });
        }
        else
            objDeffered.resolve();
        return objDeffered;
    },
    IntializeMultiSelectDropDownProviders: function () {
        $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSProviders").multiselect('destroy');
        $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSProviders").multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 200,
            nonSelectedText: 'Select',
            selectAll: false,
            onDropdownHide: function (event) {
            },
            onChange: function () {
                ClinicalCDSDetail.CheckProviderValidation();
            },
        });
    },
    CheckProviderValidation: function () {
        var self = $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail");
        var ProviderIds = self.find('#ddlCDSProviders option:Selected').map(function () {
            return this.value;
        }).get().join(',');
        var strProviderIds = ProviderIds;
        ClinicalCDSDetail.providerCheckedIds = ProviderIds;
        if (ClinicalCDSDetail.providerCheckedIds != '')
            ClinicalCDSDetail.validateProvider(2);
        else
            ClinicalCDSDetail.validateProvider(1);
    },
    validateProvider: function (operationid) {
        var self = "#" + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail";
        $(self + " #divCDSProvider").find("i").remove();
        if (operationid == 1) {
            $(self + " #divCDSProvider .multiselect").css("border-color", "#cc2724");
            $(self + " #divCDSProvider").find(".control-label").css("color", "#cc2724");
            $(self + " #divCDSProvider").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-remove" data-bv-icon-for="Race" style="display: block;"></i>');
        } else if (operationid == 2) {
            $(self + " #divCDSProvider .multiselect").css("border-color", "#3c763d");
            $(self + " #divCDSProvider").find(".control-label").css("color", "#3c763d");
            $(self + " #divCDSProvider").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-ok" data-bv-icon-for="Race" style="display: block;"></i>');
        } else {
            $(self + " #divCDSProvider .multiselect").css("border-color", "#ccc");
            $(self + " #divCDSProvider").find(".control-label").css("color", "#000000");
        }
    },

    //Author: Ahmad Raza
    //Date :  04-03-2016
    //Reason: Binding numpad with height, weight, systolic and diastolic fields


    initializeKeypad: function (Ctrl) {
        var self = "";
        if (Ctrl != null) {
            self = Ctrl;
        }
        else {
            self = $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail');
        }

        self.find('[data-plugin-keyboard-numpad]').keyboard({
            customLayout: {
                'default': [
                    '7 8 9 {b}',
                    '4 5 6 {clear}',
                    '1 2 3 {t}',
                    '0   .  {a} {c} '
                ]
            },
            change: function (e, keyboard, el) {
                if (keyboard.$preview.attr('maxlength') != null && !keyboard.$preview.keyboard().getkeyboard().options.maxLength) {
                    keyboard.$preview.keyboard().getkeyboard().options.maxLength = keyboard.$preview.attr('maxlength');
                }
                if (keyboard.$preview.attr('oninput') != null) {
                    keyboard.$preview.trigger('oninput');
                }
                // Fix # EMR-96
                if (keyboard.$preview.attr('name') == 'Height') {
                    if (keyboard.$preview.attr('onkeyup') != null) {
                        keyboard.$preview.trigger('onkeyup');
                        EMRUtility.ValidateHeight(e, keyboard.$preview);
                    }
                } else if (keyboard.$preview.attr('onkeyup') != null) {
                    keyboard.$preview.trigger('onkeyup');
                }

            },
            layout: 'custom',
            reposition: true,
            appendLocally: this,
            restrictInput: true,
            preventPaste: true,
            usePreview: false,
            autoAccept: true,
            tabNavigation: true
        }).addTyping();
    },

    //Start 10-05-2016 Edit By Humaira Yousaf Bug# EMR-776
    setKeypad: function (sender) {
        $(sender).next().addClass('mt-xl');
    },
    //End 10-05-2016 Edit By Humaira Yousaf Bug# EMR-776

    //Author: Ahmad Raza
    //Date :  04-03-2016
    //Reason: To validate blood pressure
    ValidateBP: function (objSystolic, objDiastolic) {
        var systolicVal = 0;
        var diastolicVal = 0;
        if (objSystolic != null) {
            systolicVal = $(objSystolic).val();
        }
        else if (objDiastolic != null) {
            objSystolic = $($(objDiastolic).parent().parent().prevAll()[0]).find("input[id*='txtSystolic']");
            systolicVal = $(objSystolic).val();

        }
        if (objDiastolic != null) {
            diastolicVal = $(objDiastolic).val();
        }
        else if (objSystolic != null) {
            objDiastolic = $($(objSystolic).parent().parent().nextAll()[0]).find("input[id*='txtDiastolic']");
            diastolicVal = $(objDiastolic).val();
        }
        if ((diastolicVal != "" && systolicVal != "") && (parseInt(diastolicVal) >= parseInt(systolicVal))) {
            $(objDiastolic).css("border", "1px solid red");
            utility.DisplayMessages("Diastolic should be less than Systolic", 3);
        }
        else {
            if (systolicVal != "") {
                $(objSystolic).css("border", "1px solid #ccc");
                if (diastolicVal == "") {
                    $(objDiastolic).css("border", "1px solid red");
                }
                else {
                    $(objDiastolic).css("border", "1px solid #ccc");
                }

            }
            else if (diastolicVal != "") {
                $(objDiastolic).css("border", "1px solid #ccc");
                if (systolicVal == "") {
                    $(objSystolic).css("border", "1px solid red");
                }
                else {
                    $(objSystolic).css("border", "1px solid #ccc");
                }
            }
            else {
                $(objDiastolic).css("border", "1px solid #ccc");
                $(objSystolic).css("border", "1px solid #ccc");
            }
        }

    },

    //Author: Ahmad Raza
    //Date :  04-03-2016
    //Reason: To validate BSA
    calculateBSA: function (objWeight, objHeightInFeet, TargetCtrl) {

        var WeightInLbs = "";
        if (objWeight != null) {
            WeightInLbs = $(objWeight).val();
        }
        else {
            WeightInLbs = $("#" + ClinicalCDSDetail.params.PanelID + " #txtWeight").val();
        }

        var HeightInFeet = "";
        if (objHeightInFeet != null) {
            HeightInFeet = $(objHeightInFeet).val();
        }
        else {
            HeightInFeet = $("#" + ClinicalCDSDetail.params.PanelID + " #txtHeight").val();
        }

        var CtrlName = "";
        if (TargetCtrl != null) {
            CtrlName = $(TargetCtrl);
        }
        else {
            CtrlName = $("#" + ClinicalCDSDetail.params.PanelID + " #txtBSA");
        }

        if (WeightInLbs == "" || WeightInLbs == ".")
            var weightInLbs = 0;
        else
            var weightInLbs = parseFloat(WeightInLbs);
        if (HeightInFeet == "" || HeightInFeet == ".")
            heightInFeet = 0;
        else
            var heightInFeet = parseFloat(HeightInFeet);

        var weightInKG = ClinicalCDSDetail.convertWeight(weightInLbs);
        var heightIn_cm = ClinicalCDSDetail.convertHeightTo_cm(heightInFeet);
        var result = 0.007184 * Math.pow(heightIn_cm, 0.725) * Math.pow(weightInKG, 0.425);
        var BSA = result.toFixed(2)
        if (WeightInLbs != "" && HeightInFeet != "")
            CtrlName.val(BSA);
        else
            CtrlName.val('');
    },

    //Author: Ahmad Raza
    //Date :  04-03-2016
    //Reason: To validate Weight
    convertWeight: function (pounds) {
        return pounds / 2.20462262185;
    },

    //Author: Ahmad Raza
    //Date :  04-03-2016
    //Reason: To validate height
    convertHeightTo_cm: function (feet) {
        return feet * 12 * 2.54;
    },

    //Author: Ahmad Raza
    //Date :  04-03-2016
    //Reason: To calculate BMI on the basis of height and weight
    calculateBMI: function (objWeight, objHeightInFeet, TargetCtrl) {

        var WeightInLbs = "";
        if (objWeight != null) {
            WeightInLbs = $(objWeight).val();
        }
        else {
            WeightInLbs = $("#" + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #txtWeight").val();
        }

        var HeightInFeet = "";
        if (objHeightInFeet != null) {
            HeightInFeet = $(objHeightInFeet).val();
        }
        else {
            HeightInFeet = $("#" + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #txtHeight").val();
        }

        var CtrlName = "";
        if (TargetCtrl != null) {
            CtrlName = $(TargetCtrl);
        }
        else {
            CtrlName = $("#" + ClinicalCDSDetail.params.PanelID + " #txtBMI");
        }

        if (WeightInLbs == "" || WeightInLbs == ".")
            var weightInLbs = 0;
        else
            var weightInLbs = parseFloat(WeightInLbs);
        if (HeightInFeet == "" || HeightInFeet == ".")
            heightInFeet = 0;
        else
            if (HeightInFeet.split('.')[1] != null) {
                var toFix = 0;
                if (HeightInFeet.split('.')[1].length == 2) {
                    toFix = 2;
                }
                else if (HeightInFeet.split('.')[1].length == 1) {
                    toFix = 1;
                }
            }

        var heightInFeet = parseFloat(HeightInFeet).toFixed(toFix);

        var heightInInches = ClinicalCDSDetail.convertHeightInches(heightInFeet);

        var result = (weightInLbs / (heightInInches * heightInInches)) * 703;
        var BMI = result.toFixed(2);
        if (WeightInLbs != "" && HeightInFeet != "" && BMI != "Infinity")
            CtrlName.val(BMI);
        else
            CtrlName.val('');
    },

    //Author: Ahmad Raza
    //Date :  04-03-2016
    //Reason: To convert height to inches
    convertHeightInches: function (feet) {
        var newFeet = feet.toString();
        var a = newFeet.split(".");
        var fee = parseInt(a[0]);
        var inch = parseInt(a[1]);
        if (isNaN(inch))
            return (fee * 12);
        else
            return (fee * 12) + inch;
    },

    //Author: Ahmad Raza
    //Date :  03-03-2016
    //Reason: Loading ICD Codes for Problem List AutoComplete
    bindICD9AutoComplete: function (element) {
        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "ClinicalCDSDetail", null, false);
    },

    //Author: Ahmad Raza
    //Date :  03-03-2016
    //Reason: Loading ICD Codes for Popup
    OpenSearchPopup: function (SearchType, Ctrl, HiddenCtrl) {
        var controlToLoad = "";
        if (SearchType == "ICD") {

            controlToLoad = "Admin_IMOICD";
        }
        else if (SearchType == "CPT") {
            controlToLoad = "Admin_IMOCPT";
        }
        else if (SearchType == "Modifier") {
            controlToLoad = "Admin_Modifier";
        }

        $('#ClinicalCDSDetail #txtCDSProblemList').attr("data-popupunload", "true");
        var params = [];
        params["FromAdmin"] = "0";
        if (ClinicalCDSDetail.params.TabID == 'clinicalTabProgressNote') {
            params['FromProgressNote'] = 'pnlClinicalProgressNote';
            params["ParentCtrl"] = 'ClinicalCDSDetail';
        }

        else {
            params["ParentCtrl"] = ClinicalCDSDetail.params["TabID"];

        }
        params["PanelID"] = ClinicalCDSDetail.params["PanelID"];

        params["ActionPanContainer"] = ClinicalCDSDetail.params["ActionPanContainer"];
        if (Ctrl != null) {
            params["RefCtrl"] = Ctrl;
        }
        if (HiddenCtrl != null) {
            params["RefHiddenCtrl"] = HiddenCtrl;
        }
        if (controlToLoad != "") {
            if (ClinicalCDSDetail.params.TabID == 'clinicalTabProgressNote' && SearchType == "ICD")
                LoadActionPan(controlToLoad, params, 'pnlClinicalProgressNote');
            else
                LoadActionPan(controlToLoad, params, ClinicalCDSDetail.params.PanelID);
        }

    },

    //Author: Ahmad Raza
    //Date :  03-03-2016
    //Reason: deleting Lis of Problem list
    deleteProblemList: function (obj, ev) {
        ev.stopPropagation();
        var problemListId = $(obj).attr('id');
        if ($(obj).hasClass('fromDB') == false) {
            $(obj).parent().remove();
            if ($('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSProblemList li").length > 0) {
                $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSProblemList li:first select").remove();
            }
        } else {
            AppPrivileges.GetFormPrivileges("ClinicalDecisionSupport_Clinical Decision Support", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    utility.myConfirm('1', function () {
                        var selectedValue = problemListId;
                        if (selectedValue == "" || selectedValue == "undefined") {
                        }
                        else {
                            ClinicalCDSDetail.cdsProblemListDelete(selectedValue).done(function (response) {

                                response = JSON.parse(response);
                                if (response.status != false) {
                                    $(obj).remove();
                                    if ($('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSProblemList li").length > 0) {
                                        $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSProblemList li:first select").remove();
                                    }
                                    // dfd.resolve();
                                    utility.DisplayMessages(response.Message, 1);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                    }, function () { },
                        '1'
                    );
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }

    },

    //Author: Ahmad Raza
    //Date :  16-03-2016
    //Reason: deleting CDS Problem List
    cdsProblemListDelete: function (problemListId) {
        var objData = new Object();
        objData["CDSProblemId"] = problemListId;
        objData["CDSId"] = ClinicalCDSDetail.params.CDSId;
        objData["commandType"] = "DELETE_CDS_ProblemList";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CDS", "CDS");
    },

    //Author: Ahmad Raza
    //Date :  16-03-2016
    //Reason: deleting CDS Allergy
    cdsAllergyDelete: function (allergyId) {
        var objData = new Object();
        objData["CDSAllergyId"] = allergyId;
        objData["CDSId"] = ClinicalCDSDetail.params.CDSId;
        objData["commandType"] = "DELETE_CDS_Allergy";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CDS", "CDS");
    },

    //Author: Ahmad Raza
    //Date :  16-03-2016
    //Reason: deleting CDS Medication
    cdsMedicationDelete: function (medicationId) {
        var objData = new Object();
        objData["CDSMedicationId"] = medicationId;
        objData["CDSId"] = ClinicalCDSDetail.params.CDSId;
        objData["commandType"] = "DELETE_CDS_Medication";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CDS", "CDS");
    },
    cdsLabResultDelete: function (LabTestId) {
        var objData = new Object();
        objData["TestId"] = LabTestId;
        objData["CDSId"] = ClinicalCDSDetail.params.CDSId;
        objData["commandType"] = "delete_cds_labresult";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CDS", "CDS");
    },

    //Author: Ahmad Raza
    //Date :  03-03-2016
    //Reason: deleting Lis of Allergies list
    deleteAllergy: function (obj, ev) {
        ev.stopPropagation();
        var allergyId = $(obj).attr('id');
        if ($(obj).hasClass('fromDB') == false) {
            $(obj).parent().remove();
            if ($('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSAllergies li").length > 0) {
                $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSAllergies li:first select").remove();
            }
        } else {
            AppPrivileges.GetFormPrivileges("ClinicalDecisionSupport_Clinical Decision Support", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    utility.myConfirm('1', function () {
                        var selectedValue = allergyId;
                        if (selectedValue == "" || selectedValue == "undefined") {
                        }
                        else {
                            ClinicalCDSDetail.cdsAllergyDelete(selectedValue).done(function (response) {

                                response = JSON.parse(response);
                                if (response.status != false) {
                                    $(obj).remove();
                                    if ($('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSAllergies li").length > 0) {
                                        $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSAllergies li:first select").remove();
                                    }
                                    //  dfd.resolve();
                                    utility.DisplayMessages(response.Message, 1);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                    }, function () { },
                        '1'
                    );
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }

    },

    //Author: Ahmad Raza
    //Date :  03-03-2016
    //Reason: deleting Lis of Medications list
    deleteMedication: function (obj, ev) {
        ev.stopPropagation();
        var medicationId = $(obj).attr('medicationId');
        if ($(obj).hasClass('fromDB') == false) {
            $(obj).parent().remove();
            if ($('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSMedications li").length > 0) {
                $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSMedications li:first select").remove();
            }

        } else {

            AppPrivileges.GetFormPrivileges("ClinicalDecisionSupport_Clinical Decision Support", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    utility.myConfirm('1', function () {
                        var selectedValue = medicationId;
                        if (selectedValue == "" || selectedValue == "undefined") {
                        }
                        else {
                            ClinicalCDSDetail.cdsMedicationDelete(selectedValue).done(function (response) {

                                response = JSON.parse(response);
                                if (response.status != false) {
                                    $(obj).remove();
                                    if ($('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSMedications li").length > 0) {
                                        $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSMedications li:first select").remove();
                                    }
                                    //  dfd.resolve();
                                    utility.DisplayMessages(response.Message, 1);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                    }, function () { },
                        '1'
                    );
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }

    },

    //Author: Ahmad Raza
    //Date :  14-05-2016
    //Reason: deleting Lis of Lab Result list
    deleteLabResult: function (obj, ev) {
        ev.stopPropagation();
        var CurrentLab = $(obj).closest("[data-lab]");
        var currentTestId = $(obj).closest("[data-test]").attr("id");
        if ($(obj).hasClass('fromDB') == false) {
            $(obj).closest("[data-test]").remove();

            if (CurrentLab.find("[data-test]").length == 0) {
                CurrentLab.remove();
            }
        } else {
            AppPrivileges.GetFormPrivileges("ClinicalDecisionSupport_Clinical Decision Support", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    utility.myConfirm('1', function () {
                        var selectedValue = currentTestId;
                        if (selectedValue == "" || selectedValue == "undefined") {
                        }
                        else {
                            ClinicalCDSDetail.cdsLabResultDelete(selectedValue).done(function (response) {

                                response = JSON.parse(response);
                                if (response.status != false) {
                                    $(obj).closest("[data-test]").remove();

                                    if (CurrentLab.find("[data-test]").length == 0) {
                                        CurrentLab.remove();
                                    }
                                    //  dfd.resolve();
                                    utility.DisplayMessages(response.Message, 1);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                    }, function () { },
                        '1'
                    );
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }

    },


    //Author: Ahmad Raza
    //Date :  03-03-2016
    //Reason: to show delete icon on hover
    showIcon: function (obj) {

        $(obj).find('div').css('display', '');

    },

    //Author: Ahmad Raza
    //Date :  03-03-2016
    //Reason: to hide delete icon on mouse leave
    hideIcon: function (obj) {

        if ($(obj).hasClass("active") == false) {
            $(obj).find('div').css('display', 'none');
        }

    },

    //Author: Ahmad Raza
    //Date :  16-03-2016
    //Reason: Unloading form
    UnLoad: function (caller) {
        //Start 03-03-2016 Humaira Yousaf to close form after save
        if (caller == 'saveExit') {
            UnloadActionPan(ClinicalCDSDetail.params["ParentCtrl"], "ClinicalCDSDetail");
        }
            //End 03-03-2016 Humaira Yousaf to close form after save
        else {
            utility.UnLoadDialog("frmClinicalCDSDetail", function () {
                UnloadActionPan(ClinicalCDSDetail.params["ParentCtrl"], "ClinicalCDSDetail", null, ClinicalCDSDetail.params["PanelID"]);
            }, function () {
                UnloadActionPan(ClinicalCDSDetail.params["ParentCtrl"], "ClinicalCDSDetail");
            });
        }
    },

    //Function Name: CDSSave
    //Author Name: Humaira Yousaf
    //Created Date: 02-03-2016
    //Description: Saves CDS
    CDSSave: function () {


        var CDSId = $('#' + ClinicalCDSDetail.params.PanelID + " #hfCDSID").val() != "" ? $('#' + ClinicalCDSDetail.params.PanelID + " #hfCDSID").val() : "-1";
        if (parseInt(CDSId) > 0) {
            ClinicalCDSDetail.params.mode = "Edit";
        }
        else {
            ClinicalCDSDetail.params.mode = "Add";
        }

        var self = $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail");
        //Start 03-03-2016 Humaira Yousaf to validate title
        var title = self.find("#txtCDSTitle").val();
        var ProviderIds = self.find($('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSProviders option:selected")).map(function () {
            return this.value;
        }).get().join(',');


        if (title != '' && ProviderIds != "") {
            ClinicalCDSDetail.validateProvider(2);
            //End 03-03-2016 Humaira Yousaf to validate title
            var mainErrorMessage = "";

            //Start 07-03-2016 Humaira Yousaf to validate reminder
            var reminderValidMsg = ClinicalCDSDetail.isReminderValid();
            if (reminderValidMsg != "") {
                mainErrorMessage = reminderValidMsg;
            }
            //End 07-03-2016 Humaira Yousaf to validate reminder

            //Start 11-03-2016 Humaira Yousaf to validate age condition
            if ($('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail #ddlCDSAgeCondition').val() != "") {
                var ageConditionMsg = ClinicalCDSDetail.isAgeConditionValid();
                if (ageConditionMsg != "") {
                    mainErrorMessage = ageConditionMsg;
                }
            }
            //End 11-03-2016 Humaira Yousaf to validate age condition
            var questionnairValidMsg = ClinicalCDSDetail.isQuestionnaireValid(self);
            if (questionnairValidMsg != "") {
                mainErrorMessage = questionnairValidMsg;
            }
            if (mainErrorMessage == "") {
                var myJSON = self != null ? self.getMyJSONByName() : "{}";
                var objData = JSON.parse(myJSON);

                objData["ProviderIds"] = ProviderIds;
                //Start 08-03-2016 Humaira Yousaf for ruleTypes
                var ruleTypeIds = self.find($('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSRuleType option:selected")).map(function () {
                    return this.value;
                }).get().join(',');
                objData["RuleTypes"] = ruleTypeIds;
                //End 08-03-2016 Humaira Yousaf for ruleTypes
                //Start//05-04-2016//Ahmad Raza//geting comma separated values of multiselects
                var roleIds = self.find($('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #lstCDSUserRoles option:selected")).map(function () {
                    return this.value;
                }).get().join(',');
                objData["UserRoles"] = roleIds;

                var triggerLocationIds = self.find($('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSTriggerLocation option:selected")).map(function () {
                    return this.value;
                }).get().join(',');
                objData["TriggerLocations"] = "1";

                var genderIds = self.find($('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSSex option:selected")).map(function () {
                    return this.value;
                }).get().join(',');
                objData["Genders"] = genderIds;

                var ethnicityIds = self.find($('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSEthnicity option:selected")).map(function () {
                    return this.value;
                }).get().join(',');
                objData["Ethnicities"] = ethnicityIds;

                var raceIds = self.find($('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSRace option:selected")).map(function () {
                    return this.value;
                }).get().join(',');
                objData["Races"] = raceIds;

                var languageIds = self.find($('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSLanguage option:selected")).map(function () {
                    return this.value;
                }).get().join(',');
                objData["Languages"] = languageIds;
                var orderSetIds = self.find($('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #lstCDOrderSet option:selected")).map(function () {
                    return this.value;
                }).get().join(',');
                objData["OrderSetIds"] = orderSetIds;
                //End//05-04-2016//Ahmad Raza//geting comma separated values of multiselects

                //Start 09-03-2016 Humaira Yousaf for medication
                var medicationsData = [];
                self.find($('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSMedications li")).each(function (index, item) {
                    var medication = null;
                    if (index == 0) {
                        medication = {
                            DrugId: $(this).attr('id'),
                            RxNormId: $(this).attr('rxnormid') ? $(this).attr('rxnormid') : "",
                            DrugDescription: $(this).text(),//.split('-').length > 1 ? $(this).text().split('-')[1].trim() : $(this).text(),
                            MedicationCode: $(this).text(),//.split('-').length > 1 ? $(this).text().split('-')[0].trim() : "",
                            MedicationOperator: ''
                        };
                    }
                    else {
                        medication = {
                            DrugId: $(this).attr('id'),
                            RxNormId: $(this).attr('rxnormid') ? $(this).attr('rxnormid') : "",
                            DrugDescription: $(this).find('a').text(),//.split('-').length > 1 ? $(this).find('a').text().split('-')[1].trim() : $(this).find('a').text(),
                            MedicationCode: $(this).find('a').text(),//.split('-').length > 1 ? $(this).find('a').text().split('-')[0].trim() : "",
                            MedicationOperator: $(this).find('#ddlMedications' + $(this).attr('id')).val()
                        };
                    }

                    medicationsData.push(medication);
                });
                //Start//14-03-2016//Ahmad Raza//logic to get the data for allergies
                var allergyData = [];
                self.find($('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSAllergies li")).each(function (index, item) {
                    var allergy = null;
                    if (index == 0) {
                        allergy = {
                            Allergen: $(this).find('a').text(),
                            AllergyForQuery: $(this).attr("RxNormId"),
                            AllergyOperator: '',
                            CDSAllergyId: $(this).attr('id')
                        };
                    }
                    else {
                        allergy = {
                            Allergen: $(this).find('a').text(),
                            AllergyForQuery: $(this).attr("RxNormId"),
                            AllergyOperator: $(this).find('#ddlAllergies' + $(this).attr('id')).val(),
                            CDSAllergyId: $(this).attr('id')
                        };
                    }

                    allergyData.push(allergy);
                });
                //End//14-03-2016//Ahmad Raza//logic to get the data for allergies

                //Start//14-03-2016//Ahmad Raza//logic to get the data for allergies

                //End//14-03-2016//Ahmad Raza//logic to get the data for allergies


                //Start//14-03-2016//Ahmad Raza//logic to get the data for Problems
                var problemData = [];
                self.find($('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSProblemList li")).each(function (index, item) {
                    var problem = null;
                    if (index == 0) {
                        problem = {
                            Problem: $(this).find('a').text(),
                            ProblemForQuery: $(this).attr('data'),
                            ProblemOperator: '',
                            CDSProblemId: $(this).attr('id')
                        };
                    }
                    else {
                        problem = {
                            Problem: $(this).find('a').text(),
                            ProblemForQuery: $(this).attr('data'),
                            ProblemOperator: $(this).find('#ddlProblemList' + $(this).attr('id')).val(),
                            CDSProblemId: $(this).attr('id')
                        };
                    }

                    problemData.push(problem);
                });
                var questionnaireDataData = [];
                self.find("#dgvQuestionnaire tr").each(function (index, item) {
                    if ($(this).attr('id')) {
                        var questionnaire = {
                            Description: $(this).find('input').val(),
                            QuestionnaireControlTypeId: $(this).find('#ddlControlType' + $(this).attr('id')).val(),
                            CDSQuestionnaireId: $(this).attr('id'),
                            dropDownValues: ClinicalCDSDetail.QuestionnaireDropDownValues[$(this).attr('id')],
                        };
                        questionnaireDataData.push(questionnaire);
                    }
                });
                // if(! $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #dvVitals").hasClass('hidden')){
                //     objData["IsVitalInsertUpdate"] = true;
                // }



                //End//14-03-2016//Ahmad Raza//logic to get the data for Problems
                objData["MedicationData"] = medicationsData;
                objData["AllergyData"] = allergyData;
                objData["ProblemData"] = problemData;
                objData["LabResultData"] = ClinicalCDSDetail.getLabResultData();
                objData["QuestionnaireData"] = questionnaireDataData;
                objData["QuestionnaireHTML"] = ClinicalCDSDetail.getQuestionnaireHTML(self);
                objData["VitalData"] = ClinicalCDSDetail.getVitalsData();
                objData["InsuranceData"] = ClinicalCDSDetail.getInsuranceData();
                //End 09-03-2016 Humaira Yousaf for medication
                myJSON = JSON.stringify(objData);

                var a = JSON.parse(myJSON);
                if (a.CDSReferenceURL != '' && !(a.CDSReferenceURL.startsWith('http://') || a.CDSReferenceURL.startsWith('https://'))) {
                    a.CDSReferenceURL = 'http://'.concat(a.CDSReferenceURL);
                }
                myJSON = a;
                myJSON = JSON.stringify(myJSON);

                if (ClinicalCDSDetail.params.mode == "Add") {

                    AppPrivileges.GetFormPrivileges("ClinicalDecisionSupport_Clinical Decision Support", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            ClinicalCDSDetail.saveCDS(myJSON).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {



                                    //Start 07-03-2016 Humaira Yousaf to save CDS Id
                                    $('#' + ClinicalCDSDetail.params.PanelID + " #hfCDSID").val(response.CDSId);
                                    //End 07-03-2016 Humaira Yousaf to save CDS Id
                                    utility.DisplayMessages(response.message, 1);
                                    Clinical_CDS.CDSSearch(null, null, null, null, null, "loadCurrentCDS");
                                    //Start 03-03-2016 Humaira Yousaf to unload form on save
                                    ClinicalCDSDetail.UnLoad('saveExit');
                                    //End 03-03-2016 Humaira Yousaf to unload form on save
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                        else
                            utility.DisplayMessages(strMessage, 2);
                    });
                }
                else if (ClinicalCDSDetail.params.mode == "Edit") {

                    AppPrivileges.GetFormPrivileges("ClinicalDecisionSupport_Clinical Decision Support", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            ClinicalCDSDetail.updateCDS(myJSON, CDSId).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {

                                    //Start 07-03-2016 Humaira Yousaf to save CDS Id
                                    $('#' + ClinicalCDSDetail.params.PanelID + " #hfCDSID").val(response.CDSId);
                                    //End 07-03-2016 Humaira Yousaf to save CDS Id
                                    utility.DisplayMessages(response.message, 1);
                                    Clinical_CDS.CDSSearch(null, null, null, null, null, "loadCurrentCDS");
                                    //Start 03-03-2016 Humaira Yousaf to unload form on save
                                    ClinicalCDSDetail.UnLoad('saveExit');
                                    //End 03-03-2016 Humaira Yousaf to unload form on save
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                        else
                            utility.DisplayMessages(strMessage, 2);
                    });

                }
            }
            else {
                utility.DisplayMessages(mainErrorMessage, 3);
            }
        }
        else {
            ClinicalCDSDetail.validateProvider(1);
            //      utility.DisplayMessages("Title is required.", 3);
        }
    },


    //Function Name: CDSSave
    //Author Name: Humaira Yousaf
    //Created Date: 02-03-2016
    //Description: Saves CDS
    //Params: CDSData
    saveCDS: function (CDSData) {
        var objData = JSON.parse(CDSData);
        objData["commandType"] = "save_cds";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CDS", "CDS");
    },
    //Function Name: openCDSAlert
    //Author Name: Ahmad Raza
    //Created Date: 09-03-2016
    //Description: To show CDS Search in Popup
    openCDSAlert: function () {

        if ($(" #mainForm #hfTriggerLocation").val() == 'CCDA' || $(" #mainForm #hfTriggerLocation").val() == 'LabResult') {
            BackgroundLoaderShow(true);
            var params = [];
            params["PatientId"] = $(" #mainForm #hfTriggerPatientId").val();

            params["FromAdmin"] = 0;
            params["isPopup"] = 1;
            LoadActionPan("Clinical_CDSAlert", params);
        }
        else {
            if ($(" #mainForm  li#CDSAlert span").text() != '' && $('#PatientProfile #hfPatientId').val() != '') {
                BackgroundLoaderShow(true);
                var params = [];

                params["PatientId"] = $('#PatientProfile #hfPatientId').val();
                params["FromAdmin"] = 0;
                params["isPopup"] = 1;
                LoadActionPan("Clinical_CDSAlert", params);
            }
        }
    },

    //Function Name: loadCDS
    //Author Name: Humaira Yousaf
    //Created Date: 04-03-2016
    //Description: Loads CDS data
    loadCDS: function () {
        if (ClinicalCDSDetail.params.mode == "Add") {
            $('#' + ClinicalCDSDetail.params.PanelID + " #ClinicalCDSDetail #txtCDSTitle").attr("enabled", "enabled");
            $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail #ddlCDSReminderPeriod').val(1);
            $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail #txtCDSReminderLength').val(1);

        }
        else if (ClinicalCDSDetail.params.mode == "Edit") {
            $('#' + ClinicalCDSDetail.params.PanelID + " #ClinicalCDSDetail #txtCDSTitle").attr("enabled", "enabled");;
            ClinicalCDSDetail.fillCDS(ClinicalCDSDetail.params.CDSId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var cdsDetail = JSON.parse(response.CDSJSON);
                    var self = $('#' + ClinicalCDSDetail.params.PanelID + " #ClinicalCDSDetail");
                    utility.bindMyJSON(true, cdsDetail, false, self);
                    if (cdsDetail.chkCDSActive == 'True')
                        $('#' + ClinicalCDSDetail.params.PanelID + " #ClinicalCDSDetail #chkActive").attr("checked", true);
                    else
                        $('#' + ClinicalCDSDetail.params.PanelID + " #ClinicalCDSDetail #chkActive").attr("checked", false);

                    if (cdsDetail.txtStatus != "") {
                        $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #txtCDSStatus").val(cdsDetail.txtStatus);
                    }
                    else {
                        $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #txtCDSStatus").val('Due');
                    }

                    //Start 08-03-2016 Humaira Yousaf for CDS Vitals
                    var cdsVitals = JSON.parse(response.CDSVitalsJSON);
                    ClinicalCDSDetail.loadCDSVitals(cdsVitals);
                    // utility.bindMyJSON(true, cdsVitals, false, self);

                    $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSRuleType").val(cdsDetail.ddlCDSRuleType.split(','));
                    $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSRuleType").multiselect("refresh");
                    //    ClinicalCDSDetail.enableDisableReminderControls();
                    //Start//05-04-2016//Ahmad Raza//binding multiselects
                    $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #lstCDSUserRoles").val(cdsDetail.lstCDSUserRoles.split(','));
                    $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #lstCDSUserRoles").multiselect("refresh");

                    $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSTriggerLocation").val(cdsDetail.ddlCDSTriggerLocation.split(','));
                    $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSTriggerLocation").multiselect("refresh");

                    $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSSex").val(cdsDetail.ddlCDSSex.split(','));
                    $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSSex").multiselect("refresh");

                    $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSEthnicity").val(cdsDetail.ddlCDSEthnicity.split(','));
                    $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSEthnicity").multiselect("refresh");

                    $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSRace").val(cdsDetail.ddlCDSRace.split(','));
                    $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSRace").multiselect("refresh");

                    $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSLanguage").val(cdsDetail.ddlCDSLanguage.split(','));
                    $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSLanguage").multiselect("refresh");

                    $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #lstCDOrderSet").val(cdsDetail.OrderSetIds.split(','));
                    $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #lstCDOrderSet").multiselect("refresh");

                    $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSProviders").val(cdsDetail.ddlCDSProviders.split(','));
                    $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSProviders").multiselect("refresh");
                    ClinicalCDSDetail.ProviderIds = cdsDetail.ddlCDSProviders;
                    ClinicalCDSDetail.providerCheckedIds = cdsDetail.ddlCDSProviders;
                    //End//05-04-2016//Ahmad Raza//binding multiselects
                    ClinicalCDSDetail.hideShowDataDivs();
                    //End 08-03-2016 Humaira Yousaf for CDS Vitals

                    //Start 10-03-2016 Humaira Yousaf for CDS Medications
                    var cdsMedications = JSON.parse(response.CDSMedicationJSON);
                    if (cdsMedications.length > 0) {
                        ClinicalCDSDetail.loadCDSMedications(cdsMedications);
                    }
                    //End 10-03-2016 Humaira Yousaf for CDS Medications

                    //Start 14-05-2016 Ahmad Raza for CDS Lab Result
                    var cdsLabResults = JSON.parse(response.CDSLabResultJSON);
                    if (cdsLabResults.length > 0) {
                        ClinicalCDSDetail.loadCDSLabResults(response.CDSLabResultJSON);
                    }
                    //End 14-05-2016 Ahmad Raza for CDS Lab Result

                    //Start//16-03-2016//Ahmad Raza//logic to load allergies
                    var cdsAllergies = JSON.parse(response.CDSAllergyJSON);
                    if (cdsAllergies.length > 0) {
                        ClinicalCDSDetail.loadCDSAllergies(cdsAllergies);
                    }
                    //End//16-03-2016//Ahmad Raza//logic to load allergies

                    //Start//16-03-2016//Ahmad Raza//logic to load problem list
                    var cdsProblems = JSON.parse(response.CDSProblemJSON);
                    if (cdsProblems.length > 0) {
                        ClinicalCDSDetail.loadCDSProblems(cdsProblems);
                    }
                    //End//16-03-2016//Ahmad Raza//logic to load problem list

                    var cdsInsurance = JSON.parse(response.CDSInsuranceJSON);
                    if (cdsInsurance.length > 0) {
                        ClinicalCDSDetail.loadCDSInsurance(cdsInsurance);
                    }




                    //Start 10-03-2016 Humaira Yousaf for CDS Age Condition
                    ClinicalCDSDetail.addAgeConditionValues();
                    //End 10-03-2016 Humaira Yousaf for CDS Age Condition

                    //Start 07-03-2016 Humaira Yousaf to save CDS Id
                    $('#' + ClinicalCDSDetail.params.PanelID + " #hfCDSID").val(cdsDetail.CDSId);
                    //End 07-03-2016 Humaira Yousaf to save CDS Id
                    var cdsQuestionnaire = JSON.parse(response.CDSQuestionnaireJSON);
                    if (cdsQuestionnaire.length > 0) {
                        ClinicalCDSDetail.loadCDSQuestionnaire(cdsQuestionnaire);
                    }
                    $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail").data('serialize', $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail").serialize());
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    //Function Name: fillCDS
    //Author Name: Humaira Yousaf
    //Created Date: 04-03-2016
    //Description: Fills CDS
    //Params: CDSId
    fillCDS: function (CDSId) {
        var objData = new Object();
        objData["CDSId"] = CDSId;

        if (Clinical_CDS.Switch == 1) {
            objData["IsActive"] = true;
        }
        else {
            objData["IsActive"] = false;
        }

        objData["commandType"] = "fill_cds";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CDS", "CDS");
    },

    //Function Name: updateCDS
    //Author Name: Humaira Yousaf5
    //Created Date: 07-03-2016
    //Description: Updates CDS
    //Params: CDSData, CDSId
    updateCDS: function (CDSData, CDSId) {

        var objData = JSON.parse(CDSData);
        objData["CDSId"] = CDSId;
        if (Clinical_CDS.Switch == 1) {
            objData["CDSActive"] = true;
        }
        else {
            objData["CDSActive"] = false;
        }
        objData["commandType"] = "save_cds";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CDS", "CDS");

    },

    //Function Name: addAgeConditionValues
    //Author Name: Humaira Yousaf
    //Created Date: 07-03-2016
    //Description: Shows respective cotrols based on selected age condition
    addAgeConditionValues: function () {
        var ageCondition = $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSAgeCondition").val();
        //Start 1-03-2016 Humaira Yousaf
        if (ageCondition == "") {
            $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ageConditionRange").addClass("hidden");
            $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ageConditionValue").addClass("hidden");
            var ageToValue = $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail input[id*=txtCDSAgeValue]').val('');
            var ageToValue = $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail input[id*=txtCDSAgeFrom]').val('');
            var ageToValue = $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail input[id*=txtCDSAgeTo]').val('');
        }
            //End 1-03-2016 Humaira Yousaf
        else if (ageCondition == "6") {
            $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ageConditionRange").removeClass("hidden");
            $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ageConditionValue").addClass("hidden");
        }
        else {
            $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ageConditionValue").removeClass("hidden");
            $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ageConditionRange").addClass("hidden");
        }
    },

    //Function Name: isReminderValid
    //Author Name: Humaira Yousaf
    //Created Date: 07-03-2016
    //Description: Validates reminder
    isReminderValid: function () {
        var Message = "";

        var reminder = $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail input[id*=txtCDSReminderLength]');
        var stayLength = $(reminder).val();
        var ddlVal = $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail #ddlCDSReminderPeriod').val();
        if (stayLength != null && stayLength != '') {
            if (ddlVal == null || ddlVal == '') {
                $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail #ddlCDSReminderPeriod').focus();
                Message = "Please select Reminder Period.";
            }
        }

        if (ddlVal != null && ddlVal != '') {
            if (stayLength == null || stayLength == '') {
                $(reminder).focus();
                Message = "Please enter Reminder Period.";
            }
        }

        return Message;
    },

    //Function Name: hideShowDataDivs
    //Author Name: Humaira Yousaf
    //Created Date: 08-03-2016
    //Description: HideShow Vitals,problems list etc divs on editing
    hideShowDataDivs: function () {
        var selectedValue = $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSRuleType option:selected");
        var selected = [];
        $(selectedValue).each(function (index, val) {
            selected.push($(this).text());
        });
        var unSelect = '';
        $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail .comList").addClass('hidden');
        $(selected).each(function (i, item) {
            var sectionName = item;
            unSelect += sectionName + ',';
            $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #dv" + item.replace(/\s/g, '')).removeClass('hidden');
        });
    },
    //Author: Ahmad Raza
    //Date :  09-03-2016
    //Reason: to show CDS Alert on Dashboard
    showCDSAlert: function (triggerLocation, patientId) {
        var dfd = new $.Deferred();
        ClinicalCDSDetail.showCDSAlertDBCall(triggerLocation, patientId).done(function (response) {
            response = JSON.parse(response);
            //Start//09-03-2016//Ahmad Raza//setting hiddenField values
            $(" #mainForm  li#CDSAlert input#hfCDSIDs").val('');
            $(" #mainForm  li#CDSAlert input#hfCDSIDs").val(function (i, val) {
                return val + (val ? ', ' : '') + response.CDSIDs;
            });
            //End//09-03-2016//Ahmad Raza//setting hiddenField values
            if (response.status != false) {

                if (response.alertCount > 0) {
                    $(" #mainForm  li#CDSAlert span").text(response.alertCount);
                    utility.VerifyMUAlert("CDS Alert", "", patientId != 0 ? patientId : params.patientID, true, "IA");
                }
                else {
                    $(" #mainForm  li#CDSAlert span").text('');
                    utility.VerifyMUAlert("CDS Alert", "", patientId != 0 ? patientId : params.patientID, false, "IA");
                }
                dfd.resolve();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
                dfd.resolve();
            }
        });

        return dfd;
    },

    //Author: Ahmad Raza
    //Date :  09-03-2016
    //Reason: DBCall to show CDS Alert on Dashboard
    showCDSAlertDBCall: function (triggerLocation, patientId) {
        var objData = new Object();
        if (triggerLocation == 'FaceSheet') {
            objData["CDSTriggerLocation"] = '1';
        }
        else if (triggerLocation == 'Notes') {
            objData["CDSTriggerLocation"] = '2';
        }
        else {
            objData["CDSTriggerLocation"] = '1';
        }
        if (patientId == 0) {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        }
        else {
            objData["PatientId"] = patientId;
        }
        $(" #mainForm #hfTriggerPatientId").val(patientId);

        // objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["commandType"] = "show_cds_alert";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CDS", "CDS");
    },

    //Function Name: loadCDSMedications
    //Author Name: Humaira Yousaf
    //Created Date: 10-03-2016
    //Description: Loads CDS Medications
    //Params: cdsMedications
    loadCDSMedications: function (cdsMedications) {
        $(cdsMedications).each(function (index, item) {

            var li = '';
            var code = "";
            if (item.MedicationCode != "") {
                code = item.MedicationCode + " - ";
            }
            else {
                code = "";
            }
            if (index == 0) {
                li = "<li class='fromDB' medicationId=" + item.MedicationId + " id=" + item.drugId + " rxnormid =" + (item.rxnormid ? "'" + item.rxnormid + "'" : "''") + " onclick='' \"><div class='col-sm-4 col-lg-2 pl-none pt-tiny fromDB'></div><div class='col-sm-8 col-lg-10'><a href='#'>" + item.MedicationCode + "<span class='removeIconListHover' onclick='ClinicalCDSDetail.deleteMedication($($(this).parent()).parent().parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }
            else {
                li = "<li class='fromDB' medicationId=" + item.MedicationId + " id=" + item.drugId + " rxnormid =" + (item.rxnormid ? "'" + item.rxnormid + "'" : "''") + " onclick='' \"><div class='col-sm-4 col-lg-2 pl-none pt-tiny fromDB'><select id='ddlMedications" + item.drugId + "' name = 'CDSMedications" + item.drugId + "' class='form-control'><option value='AND'>AND</option><option value='OR'>OR</option></select></div><div class='col-sm-8 col-lg-10'><a href='#'>" + item.MedicationCode + "<span class='removeIconListHover' onclick='ClinicalCDSDetail.deleteMedication($($(this).parent()).parent().parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }

            $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSMedications").append(li);

            if (index != 0) {
                if (item.medicationOperator == 'AND') {
                    $($('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSMedications li#" + item.drugId).find("#ddlMedications" + item.drugId + " option")[0]).attr('selected', true);
                }
                else {
                    $($('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSMedications li#" + item.drugId).find("#ddlMedications" + item.drugId + " option")[1]).attr('selected', true);
                }
            }
            $('.modal-backdrop').removeClass('in');
            $('.modal-backdrop').addClass('out');
            $('.modal-backdrop').hide();
            $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #txtCDSMedications").val('');
        });
    },

    //Function Name: loadCDSLabResults
    //Author Name: Ahmad Raza
    //Created Date: 14-05-2016
    //Description: Loads CDS Lab Result
    //Params: cdsMedications
    loadCDSLabResults: function (cdsLabResults) {
        //rizwan
        var parsedJson = JSON.parse(cdsLabResults);

        var LabId = null;
        var testId = null;
        var labName = null;
        var testName = null;

        var cdsLabResults = new Array();
        var obj = null;

        $.each(parsedJson, function (i, item) {

            if (testId == null) {
                testId = item.LabTestId;
            }
            if (testId == item.LabTestId) {

                cdsLabResults.push(item);
                LabId = item.LabId;
                testName = item.TestName;
                labName = item.LabName;
            }
            else {

                cdsLabResults = JSON.stringify(cdsLabResults);

                ClinicalCDSDetail.buildLabResultLi(null, testName, cdsLabResults, true, LabId, labName, testId);
                cdsLabResults = new Array();
                cdsLabResults.push(item);
                LabId = item.LabId;
                testName = item.TestName;
                labName = item.LabName;
                testId = item.LabTestId;
            }
        });

        cdsLabResults = JSON.stringify(cdsLabResults);

        ClinicalCDSDetail.buildLabResultLi(null, testName, cdsLabResults, true, LabId, labName, testId);

    },


    //Function Name: loadCDSAllergies
    //Author Name: Ahmad Raza
    //Created Date: 16-03-2016
    //Description: Loads CDS Allergies
    //Params: cdsAllergies
    loadCDSAllergies: function (cdsAllergies) {
        $(cdsAllergies).each(function (index, item) {

            var li = '';

            if (index == 0) {
                li = "<li rxnormid='" + item.AllergyForQuery + "' class='fromDB' id=" + item.AllergyId + " onclick='' \"><div class='col-sm-4 col-lg-2 pl-none pt-tiny'></div><div class='col-sm-8 col-lg-10'><a href='#'>" + item.Allergen + "<span class='removeIconListHover' onclick='ClinicalCDSDetail.deleteAllergy($($(this).parent()).parent().parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }
            else {
                li = "<li rxnormid='" + item.AllergyForQuery + "' class='fromDB' id=" + item.AllergyId + " onclick='' \"><div class='col-sm-4 col-lg-2 pl-none pt-tiny'><select id='ddlAllergies" + item.AllergyId + "' name = 'CDSAllergies" + item.AllergyId + "' class='form-control'><option value='AND'>AND</option><option value='OR'>OR</option></select></div><div class='col-sm-8 col-lg-10'><a href='#'>" + item.Allergen + "<span class='removeIconListHover' onclick='ClinicalCDSDetail.deleteAllergy($($(this).parent()).parent().parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }

            $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSAllergies").append(li);

            if (index != 0) {
                if (item.AllergyOperator == 'AND') {
                    $($('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSAllergies li#" + item.AllergyId).find("#ddlAllergies" + item.AllergyId + " option")[0]).attr('selected', true);
                }
                else {
                    $($('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSAllergies li#" + item.AllergyId).find("#ddlAllergies" + item.AllergyId + " option")[1]).attr('selected', true);
                }
            }
            $('.modal-backdrop').removeClass('in');
            $('.modal-backdrop').addClass('out');
            $('.modal-backdrop').hide();
            $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #txtCDSAllergies").val('');
        });
    },

    //Function Name: loadCDSProblems
    //Author Name: Ahmad Raza
    //Created Date: 16-03-2016
    //Description: Loads CDS Problems
    //Params: cdsProblems
    loadCDSProblems: function (cdsProblems) {
        $(cdsProblems).each(function (index, item) {

            var li = '';

            if (index == 0) {
                li = "<li class='fromDB' data='" + item.ProblemForQuery + "' id=" + item.ProblemId + " name='" + item.Problem + "' onclick='' \"><div class='col-sm-4 col-lg-2 pl-none pt-tiny'></div><div class='col-sm-8 col-lg-10'><a href='#'>" + item.Problem + "<span class='removeIconListHover' onclick='ClinicalCDSDetail.deleteProblemList($($(this).parent()).parent().parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }
            else {
                //Start 04-05-2016 Edit By Humaira Yousaf Bug# EMR-794
                li = "<li class='fromDB' data='" + item.ProblemForQuery + "' id=" + item.ProblemId + " name='" + item.Problem + "' onclick='' \"><div class='col-sm-4 col-lg-2 pl-none pt-tiny'><select id='ddlProblemList" + item.ProblemId + "' name = 'CDSProblemList" + item.ProblemId + "' class='form-control'><option value='AND'>AND</option><option value='OR'>OR</option></select></div><div class='col-sm-8 col-lg-10'><a href='#'>" + item.Problem + "<span class='removeIconListHover' onclick='ClinicalCDSDetail.deleteProblemList($($(this).parent()).parent().parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
                //End 04-05-2016 Edit By Humaira Yousaf Bug# EMR-794
            }

            $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSProblemList").append(li);

            if (index != 0) {
                if (item.ProblemOperator == 'AND') {
                    $($('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSProblemList li#" + item.ProblemId).find("#ddlProblemList" + item.ProblemId + " option")[0]).attr('selected', true);
                }
                else {
                    $($('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSProblemList li#" + item.ProblemId).find("#ddlProblemList" + item.ProblemId + " option")[1]).attr('selected', true);
                }
            }
            $('.modal-backdrop').removeClass('in');
            $('.modal-backdrop').addClass('out');
            $('.modal-backdrop').hide();
            $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #txtCDSProblemList").val('');
        });
    },
    //Function Name: isAgeConditionValid
    //Author Name: Humaira Yousaf
    //Created Date: 11-03-2016
    //Description: Validates Age Condition
    isAgeConditionValid: function () {
        var Message = "";
        var ddlAgeConditionVal = $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail #ddlCDSAgeCondition').val();

        if (ddlAgeConditionVal == '6') {

            var ageFromValue = $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail input[id*=txtCDSAgeFrom]').val();
            var ageToValue = $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail input[id*=txtCDSAgeTo]').val();

            if ((ageFromValue == null || ageFromValue == '') && (ageToValue == null || ageToValue == '')) {
                $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail input[id*=txtCDSAgeFrom]').focus();
                $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail input[id*=txtCDSAgeTo]').focus();
                Message = "Please enter From Age and To Age.";

            }
            else {
                if (ageFromValue != null || ageFromValue != '') {
                    if (ageToValue == null || ageToValue == '') {
                        $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail input[id*=txtCDSAgeTo]').focus();
                        Message = "Please enter To Age.";
                    }
                }

                if (ageToValue != null || ageToValue != '') {
                    if (ageFromValue == null || ageFromValue == '') {
                        $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail input[id*=txtCDSAgeFrom]').focus();
                        Message = "Please enter From Age.";
                    }
                }
            }

        }
        else {
            var ageValue = $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail input[id*=txtCDSAgeValue]').val();
            if (ageValue == null || ageValue == '') {
                $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail input[id*=txtCDSAgeValue]').focus();
                Message = "Please enter Age.";
            }
        }

        return Message;
    },

    resetControlValue: function (obj) {
        var temp = $('#' + ClinicalCDSDetail.params.PanelID + " #hfCDSID").val();
        var tempDeveloper = $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #txtCDSDeveloper").val();
        var tempFundingSource = $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #txtCDSFundingSource").val();
        var tempCDSStatus = $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #txtCDSStatus").val();
        $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail').resetAllControls();
        $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail #ddlCDSTriggerLocation').multiselect("clearSelection");
        $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail #lstCDSUserRoles').multiselect("clearSelection");
        $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail #ddlCDSSex').multiselect("clearSelection");
        $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail #ddlCDSEthnicity').multiselect("clearSelection");
        $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail #ddlCDSRace').multiselect("clearSelection");
        $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail #ddlCDSLanguage').multiselect("clearSelection");
        $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail #ddlCDSRuleType').multiselect("clearSelection");
        $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail #lstCDOrderSet').multiselect("clearSelection");
        $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail .comList").addClass('hidden');
        $('#' + ClinicalCDSDetail.params.PanelID + " #hfCDSID").val(temp);
        $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #txtCDSDeveloper").val(tempDeveloper);
        $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #txtCDSFundingSource").val(tempFundingSource);
        $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #txtCDSStatus").val(tempCDSStatus);
    },
    //Start 05-05-2016 Edit By Humaira Yousaf Bug# EMR-777
    //Function Name: validateAge
    //Author Name: Humaira Yousaf
    //Created Date: 05-05-2016
    //Description: Validates Age
    validateAge: function (objFromAge, objToAge) {
        var toVal = 0;
        var fromVal = 0;
        if (objToAge != null) {
            toVal = $(objToAge).val();
        }
        else if (objFromAge != null) {
            objToAge = $(objFromAge).parent().parent().find("input[id*='txtCDSAgeTo']");
            toVal = $(objToAge).val();

        }
        if (objFromAge != null) {
            fromVal = $(objFromAge).val();
        }
        else if (objToAge != null) {
            objFromAge = $($(objToAge).parent().parent()).find("input[id*='txtCDSAgeFrom']");
            fromVal = $(objFromAge).val();
        }
        if ((fromVal != "" && toVal != "") && (parseInt(fromVal) >= parseInt(toVal))) {
            $(objFromAge).css("border", "1px solid red");
            utility.DisplayMessages("From age should be less than To age.", 3);
        }
        else {
            if (toVal != "") {
                $(objToAge).css("border", "1px solid #ccc");
                if (fromVal == "") {
                    $(objFromAge).css("border", "1px solid red");
                }
                else {
                    $(objFromAge).css("border", "1px solid #ccc");
                }

            }
            else if (fromVal != "") {
                $(objFromAge).css("border", "1px solid #ccc");
                if (toVal == "") {
                    $(objToAge).css("border", "1px solid red");
                }
                else {
                    $(objToAge).css("border", "1px solid #ccc");
                }
            }
            else {
                $(objFromAge).css("border", "1px solid #ccc");
                $(objToAge).css("border", "1px solid #ccc");
            }
        }

    },
    //End 05-05-2016 Edit By Humaira Yousaf Bug# EMR-777



    VitalTypeChange: function (ddlVitalType) {
        var divVitalsTemplateId = $(ddlVitalType).closest("div[id*='divVitalsTemplate']").attr("id");
        var self = $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #" + divVitalsTemplateId);

        ClinicalCDSDetail.validateVitalType(ddlVitalType);

        switch ($(ddlVitalType).val()) {
            case "Weight":
                self.find("#ddlVitalUnit").val("lbs");
                break;
            case "Height":
                self.find("#ddlVitalUnit").val("Feet");
                break;
            case "BMI":
                self.find("#ddlVitalUnit").val("kg/m2");
                break;
            case "Systolic":
                self.find("#ddlVitalUnit").val("mmHg");
                break;
            case "Diastolic":
                self.find("#ddlVitalUnit").val("mmHg");
                break;
            case "Pulse":
                self.find("#ddlVitalUnit").val("bpm");
                break;
            case "PulseResult":
                self.find("#ddlVitalUnit").val("mmHg");
                break;
            case "Temprature":
                self.find("#ddlVitalUnit").val("F");
                break;
            case "RespirationResult":
                self.find("#ddlVitalUnit").val("rpm");
                break;
            case "OxygenSaturation":
                self.find("#ddlVitalUnit").val("LMin");
                break;
            case "HeartRate":
                self.find("#ddlVitalUnit").val("bpm");
                break;
            case "RespiratoryRate":
                self.find("#ddlVitalUnit").val("rpm");
                break;
            default:
                self.find("#ddlVitalUnit").val("");
                break;
        }

    },

    validateVitalType: function (ddlVitalType) {

        var currentRowId = $(ddlVitalType).closest("div[id*='divVitalsTemplate']").attr("id");
        var vitalTypeDropDowns = $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #dvVitals div[id*='divVitalsTemplate']:not([id='" + currentRowId + "']) #ddlVitalType");

        $.each(vitalTypeDropDowns, function () {
            if ($(this).val() != "" && $(this).val() == $(ddlVitalType).val()) {
                utility.DisplayMessages("This Vital is already Added.", 1);
                $(ddlVitalType).val($($(ddlVitalType).attr("id") + " option:first").val());
            }
        });


    },

    VitalLogicalOperatorChange: function (ddlVitalLogicalOperator) {
        var divVitalsTemplateId = $(ddlVitalLogicalOperator).closest("div[id*='divVitalsTemplate']").attr("id");
        var self = $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #" + divVitalsTemplateId);
        var VitalLogicalOperatorValue = $(ddlVitalLogicalOperator).val();

        if (VitalLogicalOperatorValue == "") {
            self.find("#VitalValue").addClass("hidden");
            self.find("#VitalValueRange").addClass("hidden");
            self.find("#txtVitalValue").val("");
            self.find("#txtVitalValueFrom").val("");
            self.find("#txtVitalValueTo").val("");
        }
        else if (VitalLogicalOperatorValue == "6") {
            self.find("#VitalValueRange").removeClass("hidden");
            self.find("#VitalValue").addClass("hidden");
        }
        else {
            self.find("#VitalValue").removeClass("hidden");
            self.find("#VitalValueRange").addClass("hidden");
        }
    },

    LabResultsLogicalOperatorChange: function (ddlLogicalOperator) {
        var divVitalsTemplateId = $(ddlLogicalOperator).closest("li").attr("data-attribute-id");
        var li = $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSLabResults [data-attribute-id='" + divVitalsTemplateId + "']");
        var VitalLogicalOperatorValue = $(ddlLogicalOperator).val();

        if (VitalLogicalOperatorValue == "") {
            li.find("#LabResultValue").addClass("hidden");
            li.find("#LabResultValueRange").addClass("hidden");
            li.find("#txtLabResultValue").val("");
            li.find("#txtLabResultValueFrom").val("");
            li.find("#txtLabResultValueTo").val("");
        }
        else if (VitalLogicalOperatorValue == "6") {
            li.find("#LabResultValueRange").removeClass("hidden");
            li.find("#LabResultValue").addClass("hidden");
        }
        else {
            li.find("#LabResultValue").removeClass("hidden");
            li.find("#LabResultValueRange").addClass("hidden");
        }
    },

    AddNewVitalRow: function () {

        var isEmptryRecords = false;


        var self = $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #dvVitals");

        $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #dvVitals #ddlVitalType").each(function () {
            if ($(this).val() == "") {
                isEmptryRecords = true;
            }
        });

        if (isEmptryRecords) {

            utility.DisplayMessages("Please Fill Empry Records First", 3);
            return false;
        }
        var totalVitalsAdded = Number(self.find("div[id*='divVitalsTemplate']").last().attr("data-Vital-Template-ID")) + 1;
        var templateDom = self.find("#divVitalsTemplate");
        //clone the div, change the id and insert after specifid element
        var newRow = self.find("#divVitalsTemplate").clone().attr('id', templateDom.attr("id") + totalVitalsAdded).attr('data-Vital-Template-ID', totalVitalsAdded).insertAfter(self.find("div[id*='divVitalsTemplate']").last());

        newRow.removeAttr("CDSVitalsId");
        newRow.find("#ddlVitalType,#ddlVitalLogicalOperator,#ddlVitalUnit").val("");
        newRow.find("input").val("");
        newRow.find("#ddlVitalsLogic").removeClass("hidden");
        newRow.find("#divDeleteButton").removeClass("hidden");
        newRow.removeClass("fromDB");
        newRow.find("#VitalValue").removeClass("hidden").addClass("hidden");
        newRow.find("#VitalValueRange").removeClass("hidden").addClass("hidden");

        ClinicalCDSDetail.initializeKeypad(newRow);

        $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail').bootstrapValidator('addField', $(newRow).find("#ddlVitalLogicalOperator"));

        return newRow;
    },

    deleteVital: function (obj, ev) {
        ev.stopPropagation();
        var divVitalsTemplate = $(obj).closest("div[id*='divVitalsTemplate']");
        var CDSVitalsId = $(divVitalsTemplate).attr('CDSVitalsId');

        if (!$(divVitalsTemplate).hasClass('fromDB')) {
            $(divVitalsTemplate).remove();
        } else {
            AppPrivileges.GetFormPrivileges("ClinicalDecisionSupport_Clinical Decision Support", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    utility.myConfirm('1', function () {
                        var selectedValue = CDSVitalsId;
                        if (selectedValue == "" || selectedValue == "undefined") {
                        }
                        else {
                            ClinicalCDSDetail.cdsVitalDelete(selectedValue).done(function (response) {

                                response = JSON.parse(response);
                                if (response.status != false) {
                                    $(divVitalsTemplate).remove();
                                    utility.DisplayMessages(response.Message, 1);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                    }, function () { },
                        '1'
                    );
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }

    },

    cdsVitalDelete: function (CDSVitalsId) {
        var objData = new Object();
        objData["CDSVitalsId"] = CDSVitalsId;
        objData["CDSId"] = ClinicalCDSDetail.params.CDSId;
        objData["commandType"] = "DELETE_CDS_Vital";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CDS", "CDS");
    },
    loadCDSVitals: function (cdsVitals) {
        $(cdsVitals).each(function (index, item) {
            var newRow = null;

            if (index > 0 && item != null && item != "") {
                newRow = ClinicalCDSDetail.AddNewVitalRow();
            }
            else {
                newRow = $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #dvVitals #divVitalsTemplate");
            }

            utility.bindMyJSONByName(true, item, false, newRow).done(function () {

                newRow.attr("CDSVitalsId", item.CDSVitalsId);
                newRow.addClass("fromDB");
                //   newRow.find("#ddlVitalsLogic").trigger("change");
                newRow.find("#ddlVitalType").trigger("change");
                newRow.find("#ddlVitalLogicalOperator").trigger("change");

            });

        });
    },

    getVitalsData: function () {
        var self = $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #dvVitals");

        var totalVitals = self.find("div[id*='divVitalsTemplate']");

        var vitalsData = [];
        $.each(totalVitals, function (index, item) {
            var vital = null;

            if ($(this).find('#ddlVitalType').val() != "") {
                if (index == 0) {
                    vital = {
                        //  VitalsLogic: '',
                        CDSVitalsId: $(this).attr("CDSVitalsId"),
                        VitalType: $(this).find('#ddlVitalType').val(),
                        VitalLogicalOperator: $(this).find('#ddlVitalLogicalOperator').val(),
                        VitalValue: $(this).find('#ddlVitalLogicalOperator').val() == 6 ? null : $(this).find('#txtVitalValue').val(),
                        VitalValueFrom: $(this).find('#ddlVitalLogicalOperator').val() == 6 ? $(this).find('#txtVitalValueFrom').val() : null,
                        VitalValueTo: $(this).find('#ddlVitalLogicalOperator').val() == 6 ? $(this).find('#txtVitalValueTo').val() : null,
                        VitalUnit: $(this).find('#ddlVitalUnit').val()
                    };
                }
                else {
                    vital = {
                        CDSVitalsId: $(this).attr("CDSVitalsId"),
                        VitalsLogic: $(this).find('#ddlVitalsLogic').val(),
                        VitalType: $(this).find('#ddlVitalType').val(),
                        VitalLogicalOperator: $(this).find('#ddlVitalLogicalOperator').val(),
                        VitalValue: $(this).find('#ddlVitalLogicalOperator').val() == 6 ? null : $(this).find('#txtVitalValue').val(),
                        VitalValueFrom: $(this).find('#ddlVitalLogicalOperator').val() == 6 ? $(this).find('#txtVitalValueFrom').val() : null,
                        VitalValueTo: $(this).find('#ddlVitalLogicalOperator').val() == 6 ? $(this).find('#txtVitalValueTo').val() : null,
                        VitalUnit: $(this).find('#ddlVitalUnit').val()
                    };
                }
                vitalsData.push(vital);
            }
        });

        return vitalsData;
    },

    bindAutoComplete: function (element) {
        var CodeSystemType = $('#' + ClinicalCDSDetail.params.PanelID + ' #ddlLabId option:selected').attr('CodeSystem');
        var labId = $('#' + ClinicalCDSDetail.params.PanelID + ' #ddlLabId').val();
        EMRUtility.BindLOINCCodes(element, "ClinicalCDSDetail", labId, '', CodeSystemType);


    },

    pushLOINCAsCpt: function (JsonObj) {

        var observation = JsonObj["Observation"];
        var LOINCCOde = JsonObj['LOINICCODE'];
        var LOINCDescription = JsonObj['LOINICDescription'];

        ClinicalCDSDetail.bindLabTestResultAttributes(LOINCCOde, LOINCDescription);
    },

    bindLabTestResultAttributes: function (LOINCCOde, LOINCDescription) {
        var _selectedLabId = $('#' + ClinicalCDSDetail.params.PanelID + " #ddlLabId").val();

        if (_selectedLabId != "") {
            ClinicalCDSDetail.loadLabTest_DBCall(_selectedLabId, LOINCCOde).done(function (response) {

                var data = JSON.parse(response);
                if (data.status != false) {
                    var LabTestJSON = JSON.parse(data.ClinicalLabTest_JSON);
                    if (LabTestJSON.length > 0) {
                        //
                        if (LabTestJSON[0].IsActive != "False") {
                            if (LabTestJSON[0].IsTemplate == "False") {
                                ClinicalCDSDetail.buildLabResultLi(LOINCCOde, LOINCDescription, data.ClinicalLabTestAttribute_JSON);
                            }
                            else {
                                utility.DisplayMessages("No Attributes added against this Test", 3);

                            }
                        }
                        else {
                            utility.DisplayMessages("No Attributes added against this Test", 3);

                        }

                    }
                    else {

                        utility.DisplayMessages("No Attributes added against this Test", 3);
                    }

                }
            });
        }
    },

    loadLabTest_DBCall: function (LabId, LOINICCODE) {
        var objData = new Object();
        objData["LabId"] = LabId;
        objData["LOINICCODE"] = LOINICCODE;
        objData["commandType"] = "load_clinical_lab_test_and_attributes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalLab", "ClinicalLabTest");
    },

    buildLabResultLi: function (icdCodeAndDescription, labTestName, labTestAttributes, isFill, selectedLabId, selectedLabName, LabTestId) {
        // Ali Awan

        var attributeObject = JSON.parse(labTestAttributes);
        var mainListTemplate = $("#ClinicalCDSDetail #LabResultMarkupTemplate").clone();

        if (isFill == null) {
            selectedLabId = $("#" + ClinicalCDSDetail.params.PanelID + " #ddlLabId").val();
            selectedLabName = $('#' + ClinicalCDSDetail.params.PanelID + ' #ddlLabId option:selected').text();
            LabTestId = attributeObject[0].LabTestId;
        }
        selectedLabName = selectedLabName.toUpperCase();
        var LabAlreadyExist = false;
        var TestAlreadyExist = false;

        $('#ClinicalCDSDetail #ulCDSLabResults  [data-lab]').each(function () {
            if ($(this).attr("id") == selectedLabId) {

                LabAlreadyExist = true;
                mainListTemplate = $(this);
            }

            $(this).find("[data-test]").each(function () {

                if ($(this).attr("id") == LabTestId) {

                    TestAlreadyExist = true;
                    // utility.DisplayMessages(labTestName + '  already added', 2);
                    return;
                }

            });

        });

        if (LabAlreadyExist) {

            var LabTestMarkupTemplate = '';

            if (TestAlreadyExist) {
                utility.DisplayMessages(labTestName + '  already added', 2);
                return;
            }
            else {
                LabTestMarkupTemplate = $("#ClinicalCDSDetail #LabResultMarkupTemplate #LabTestMarkupTemplate").clone();

                LabTestMarkupTemplate.find("#spnTestName").text(labTestName);
                LabTestMarkupTemplate.attr("data-test", labTestName);
                //  LabTestMarkupTemplate.find("#ddlLabTestLogic").removeClass("hidden");
                if (isFill) {
                    LabTestMarkupTemplate.addClass("fromDB");
                }
                //NOTE: KEEP THIS AT THE END BECAUSE THIS WILL CHANGE THE ID OF CURRENT ELEMENT
                LabTestMarkupTemplate.attr("id", LabTestId);


                $.each(attributeObject, function (i, item) {

                    //for first record exiting row will be filled
                    var self = '';
                    if (i > 0) {
                        self = $("#ClinicalCDSDetail #LabResultMarkupTemplate #LabTestAttributeTemplate").clone();
                        self.find("#ddlLabResultsOperator").removeClass("hidden");
                    } else {
                        self = $(LabTestMarkupTemplate).find("#LabTestAttributeTemplate");
                        self.find("#ddlLabResultsOperator").addClass("hidden");
                    }
                    self.attr("data-lab-id", selectedLabId);
                    self.attr("data-test-id", item.LabTestId);
                    self.attr("data-lab-name", selectedLabName);
                    self.attr("data-test-name", labTestName);
                    self.attr("data-attribute-id", item.LabTestAttributeId);
                    self.attr("data-cds-lab-result-id", ClinicalCDSDetail.labResultId--);

                    self.find("#txtLabResultName").val(item.AttributeName);

                    if (isFill) {
                        self.attr("data-cds-lab-result-id", item.labResultId);
                        self.attr("data-cds-lab-result-id", item.CDSLabResultId);
                        self.find("#ddlLabResultsOperator").val(item.LabResults);
                        self.find("#ddlLabResultLogicalOperator").val(item.LabResultLogicalOperator);
                        self.find("#txtLabResultValue").val(item.LabResultValue);
                        self.find("#txtLabResultValueFrom").val(item.LabResultValueFrom);
                        self.find("#txtLabResultValueTo").val(item.LabResultValueTo);
                    }
                    // self.attr("id", item.LabTestAttributeId);
                    if (i > 0) {
                        $(LabTestMarkupTemplate).find("#testing").closest("ul").append(self);
                    }


                });

                mainListTemplate.find("ul:first").append(LabTestMarkupTemplate);
                LabTestMarkupTemplate.find("#ddlLabResultLogicalOperator").trigger("change");
            }
        }
        else {

            if ($('#ClinicalCDSDetail #ulCDSLabResults [data-lab]').attr("id") != selectedLabId) {
                //  mainListTemplate.find("#divLabResultLogic").html("");
            }
            //Lab info
            mainListTemplate.attr("id", selectedLabId);
            mainListTemplate.attr("data-lab", selectedLabName);
            mainListTemplate.find("#spnLabName").text(selectedLabName);


            //Test Info
            mainListTemplate.find("#spnTestName").text(labTestName);
            mainListTemplate.find("#LabTestMarkupTemplate").attr("data-test", labTestName);
            if (isFill) {
                mainListTemplate.find("#LabTestMarkupTemplate").addClass("fromDB");
            }
            //NOTE: KEEP THIS AT THE END BECAUSE THIS WILL CHANGE THE ID OF CURRENT ELEMENT
            mainListTemplate.find("#LabTestMarkupTemplate").attr("id", LabTestId);

            //t will be the inconming attributes
            $.each(attributeObject, function (i, item) {

                //for first record exiting row will be filled
                var self = '';
                if (i > 0) {
                    self = $("#ClinicalCDSDetail #LabResultMarkupTemplate #LabTestAttributeTemplate").clone();
                    self.find("#ddlLabResultsOperator").removeClass("hidden");
                }
                else {
                    self = mainListTemplate.find("#LabTestAttributeTemplate");
                    self.find("#ddlLabResultsOperator").addClass("hidden");
                }
                self.attr("data-lab-id", selectedLabId);
                self.attr("data-test-id", item.LabTestId);
                self.attr("data-lab-name", selectedLabName);
                self.attr("data-test-name", labTestName);
                self.attr("data-attribute-id", item.LabTestAttributeId);
                self.attr("data-cds-lab-result-id", ClinicalCDSDetail.labResultId--);

                self.find("#txtLabResultName").val(item.AttributeName);

                if (isFill) {
                    self.attr("data-cds-lab-result-id", item.labResultId);
                    self.attr("data-cds-lab-result-id", item.CDSLabResultId);
                    self.find("#ddlLabResultsOperator").val(item.LabResults);
                    self.find("#ddlLabResultLogicalOperator").val(item.LabResultLogicalOperator);
                    self.find("#txtLabResultValue").val(item.LabResultValue);
                    self.find("#txtLabResultValueFrom").val(item.LabResultValueFrom);
                    self.find("#txtLabResultValueTo").val(item.LabResultValueTo);
                }
                // self.attr("id", item.LabTestAttributeId);
                if (i > 0) {
                    mainListTemplate.find("#testing").closest("ul").append(self);
                }


            });


            $('#ClinicalCDSDetail #ulCDSLabResults').append(mainListTemplate);
            $("#ClinicalCDSDetail #ulCDSLabResults #ddlLabResultLogicalOperator").trigger("change");


            $('.modal-backdrop').removeClass('in');
            $('.modal-backdrop').addClass('out');
            $('.modal-backdrop').hide();
            $('#ClinicalCDSDetail #txtCDSLabResults').val('');
            //     Clinical_Complaints.AddInArray(currId, icd10Description, true);
            var isUnload = "false";
            var txt = $('#ClinicalCDSDetail #txtCDSLabResults');
            if (txt.is('[data-popupunload]')) {
                isUnload = txt.attr('data-popupunload');
            }

            if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
                txt.attr("data-popupunload", "false");
                Admin_IMOICD.UnLoadTab();
            }

        }

    },

    enableRecursiveValidation: function () {
        $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail').data('bootstrapValidator').enableFieldValidators('CDSRecursivePeriod', false);
        $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail').data('bootstrapValidator').enableFieldValidators('CDSRecursiveLength', false);
        var stayLength = $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail #txtCDSRecursiveLength').val();
        var ddlVal = $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail #ddlCDSRecursivePeriod').val();
        if (stayLength != null && stayLength != "") {
            $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail').data('bootstrapValidator').enableFieldValidators('CDSRecursivePeriod', true);
            $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail #lblRecursive').html('Recursive<span class="required">*</span>');
        }
        else {
            $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail').data('bootstrapValidator').enableFieldValidators('CDSRecursivePeriod', false);
            $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail #lblRecursive').html('Recursive');
        }
        if (ddlVal != null && ddlVal != "") {
            $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail').data('bootstrapValidator').enableFieldValidators('CDSRecursiveLength', true);
            $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail #lblRecursive').html('Recursive<span class="required">*</span>');

        } else if (stayLength != "") {
            $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail').data('bootstrapValidator').enableFieldValidators('CDSRecursiveLength', true);
        } else {
            $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail').data('bootstrapValidator').enableFieldValidators('CDSRecursiveLength', false);
            $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail #lblRecursive').html('Recursive');
        }

    },
    enableReminderValidation: function () {
        $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail').data('bootstrapValidator').enableFieldValidators('CDSReminderPeriod', false);
        $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail').data('bootstrapValidator').enableFieldValidators('CDSReminderLength', false);


        var stayLength = $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail #txtCDSReminderLength').val();

        if (stayLength != "" && stayLength <= 0) {

            $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail #txtCDSReminderLength').val("");
            utility.DisplayMessages("value should be greater than Zero ", 2);
        }


        var ddlVal = $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail #ddlCDSReminderPeriod').val();
        if (stayLength != null && stayLength != "") {
            $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail').data('bootstrapValidator').enableFieldValidators('CDSReminderPeriod', true);
            $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail #lblDuration').html('Reminder<span class="required">*</span>');
        }
        else {
            $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail').data('bootstrapValidator').enableFieldValidators('CDSReminderPeriod', false);
            $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail #lblDuration').html('Reminder');
        }
        if (ddlVal != null && ddlVal != "") {
            $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail').data('bootstrapValidator').enableFieldValidators('CDSReminderLength', true);
            $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail #lblDuration').html('Reminder<span class="required">*</span>');

        }
        else if (stayLength != "") {
            $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail').data('bootstrapValidator').enableFieldValidators('CDSReminderLength', true);
        }
        else {
            $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail').data('bootstrapValidator').enableFieldValidators('CDSReminderLength', false);
            $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail #lblDuration').html('Reminder');
        }

    },
    AddNewQuestion: function () {
        var ddlcontrolType = $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail').find('#ddlControlType').clone();
        $(ddlcontrolType).attr('id', 'ddlControlType' + ClinicalCDSDetail.questionnairId);
        var table = $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail').find('#dgvQuestionnaire');
        var $row = '<tr id="' + ClinicalCDSDetail.questionnairId + '"><td class="size80per"><input type="text" id="txtQuestionnaire" name="QuestDescription" maxlength="200" class="form-control"></td>' +
            '<td class="size20per">' + ddlcontrolType[0].outerHTML + '<td>' +
            '<td><a href="#"><span class="removeIconListHover" onclick="ClinicalCDSDetail.deleteQuestionnaire($($(this).parent()).parent(), event);"><i class="fa fa-times"></i></span></a></td>' +

            '</tr>';
        table.append($row);
        ClinicalCDSDetail.questionnairId = ClinicalCDSDetail.questionnairId - 1;
    },
    loadCDSQuestionnaire: function (cdsQuestionnaire) {
        var table = $('#' + ClinicalCDSDetail.params.PanelID + ' #frmClinicalCDSDetail').find('#dgvQuestionnaire');
        $(cdsQuestionnaire).each(function (index, item) {
            ClinicalCDSDetail.QuestionnaireDropDownValues[item.CDSQuestionnaireId] = item.dropDownValues;
            var $row = '<tr class="fromDB" id="' + item.CDSQuestionnaireId + '"><td class="size80per"><input type="text" id="txtQuestionnaire" name="QuestDescription" maxlength="100" class="form-control" value="' + item.Description + '"/></td>' +
           '<td class="size20per"><select id="ddlControlType' + item.CDSQuestionnaireId + '" name="QuestionnaireControlTypeId" onchange="ClinicalCDSDetail.QuestionnaireControlTypeChanged(this);" class="form-control" ddlist="getCDSQuestionnaireControlType"></select><td>' +
          '<td><a href="#"><span class="removeIconListHover" onclick="ClinicalCDSDetail.deleteQuestionnaire($($(this).parent()).parent(), event);"><i class="fa fa-times"></i></span></a></td>' +
           '</tr>';
            table.append($row);
        });
        table.loadDropDowns(true).done(function () {
            $(cdsQuestionnaire).each(function (index, item) {
                table.find('#ddlControlType' + item.CDSQuestionnaireId + ' option').filter(function () {
                    return $.trim($(this).val()) == item.QuestionnaireControlTypeId
                }).prop('selected', true);
            });
        });
    },
    isQuestionnaireValid: function (self) {
        var Message = "";
        self.find("#dgvQuestionnaire tr").each(function (index, item) {
            if ($(this).attr('id')) {
                var controlDes = $(this).find('input');
                var ControlType = $(this).find('#ddlControlType' + $(this).attr('id'));
                if (controlDes && controlDes.val() == "") {
                    $(controlDes).css("border", "1px solid red");
                    Message = "Please enter Question title.";
                    $(controlDes).focus();
                    return false;
                }
                else {
                    $(controlDes).css("border", "");
                }
                if (ControlType && ControlType.val() == "") {
                    $(ControlType).css("border", "1px solid red");
                    Message = "Please Select Control Type.";
                    $(ControlType).focus();
                    return false;
                }
                else {
                    $(ControlType).css("border", "");
                }
            }
        });
        return Message;
    },
    getQuestionnaireHTML: function (self) {
        var questionnaireHTML = "";
        self.find("#dgvQuestionnaire tr").each(function (index, item) {
            var selectedCtrl = $(this).find('#ddlControlType' + $(this).attr('id') + ' option:selected').text();
            if ($(this).attr('id') && selectedCtrl) {
                var ctrlHtml = "";
                var description = $(this).find('input').val();
                switch (selectedCtrl.toLowerCase()) {
                    case "yes no":
                        ctrlHtml = '<div class="yesNo col-xs-12" id="' + $(this).attr('id') + '">'
                        + '<div class="col-sm-12 p-none">'
                        + '<label id="TemplateYesNoLabel" class="control-label size-max100per resetLineHeight questionTitle" for="toolYesNo" data-toggle="tooltip" title=""' + description + '"">' + description + '</label>'
                         + '</div>'
                          + '<div id="customFormYesNoPreview"  class="col-sm-12 p-none">'
                         + '<div class="checkbox-custom checkbox-default col-sm-4">'
                        + '<input type="checkbox" id="chkYes" onchange="ClinicalCDSDetail.YesOrNo(this);" name="Yes">'
                        + '<label class="control-label size-max100per resetLineHeight" for="Yes">Yes</label>'
                        + '</div>'
                         + '<div class="checkbox-custom checkbox-default col-sm-4">'
                        + '<input type="checkbox" name="No" onchange="ClinicalCDSDetail.YesOrNo(this);" id="chkNo">'
                        + '<label class="control-label size-max100per resetLineHeight" for="No">No</label>'
                        + '</div>'
                        + '</div>'
                        + '</div>'
                        break;
                    case "yes no with note":
                        ctrlHtml = '<div class="yesNoNote col-xs-12" id="' + $(this).attr('id') + '">'
                        + '<div class="col-sm-12 p-none">'
                        + '<label id="TemplateYesNoLabel" class="control-label size-max100per resetLineHeight questionTitle" for="toolYesNo" data-toggle="tooltip" title=""' + description + '"">' + description + '</label>'
                         + '</div>'
                        + '<div id="customFormYesNoPreview"  class="col-sm-12 p-none">'
                        + '<div class="checkbox-custom checkbox-default col-sm-4">'
                        + '<input type="checkbox" id="chkYes" onchange="ClinicalCDSDetail.YesOrNo(this);" name="Yes">'
                        + '<label class="control-label size-max100per resetLineHeight" for="Yes">Yes</label>'
                        + '</div>'
                        + '<div class="checkbox-custom checkbox-default col-sm-4">'
                        + '<input type="checkbox" name="No" onchange="ClinicalCDSDetail.YesOrNo(this);" id="chkNo">'
                        + '<label class="control-label size-max100per resetLineHeight" for="No">No</label>'
                        + '</div>'
                        + '<div class="checkbox-custom checkbox-default col-sm-4">'
                             + '<input class="form-control " name="TextField" id="txtTextField" type="text" maxLength="150">'
                        + '</div>'
                        + '</div>'
                        + '</div>'
                        break;
                    case "text box":
                        ctrlHtml = '<div class="textbox col-xs-12" id="' + $(this).attr('id') + '">'
                                    + '<label class="control-label size-max100per resetLineHeight questionTitle"for="toolTextField" id="lblTextField" data-toggle="tooltip" title="' + description + '">' + description + '</label>'
                                 + '<input class="form-control " name="TextField" id="txtTextField" type="text" maxLength="150">'
                                   + '</div>'
                        break;
                    case "text area":
                        ctrlHtml = '<div class="textarea col-xs-12" id="' + $(this).attr('id') + '">'
                                    + '<label class="control-label size-max100per resetLineHeight questionTitle"for="toolTextField" id="lblTextField" data-toggle="tooltip" title="' + description + '">' + description + '</label>'
                                   + '<textarea id="txtFreeText" class="form-control" maxLength="500" rows="4" spellcheck="true"> </textarea>'
                                   + '</div>'
                        break;
                    case "dropdown":
                        ctrlHtml = ClinicalCDSDetail.getQuestionnaireDropDownHTML(description, $(this).attr('id'));
                        break;
                    default:
                        break;
                }
                questionnaireHTML += ctrlHtml;
            }
        });
        return questionnaireHTML;
    },

    getQuestionnaireDropDownHTML: function (description, CDSQuestionnaireId) {

        var HTML = '<div class="dropdown col-xs-12" id="' + CDSQuestionnaireId + '"><label class="control-label size-max100per resetLineHeight questionTitle" '
                   + 'data-toggle="tooltip" title="' + description + '">' + description + '</label><select onchange="Clinical_CDSAlertDetail.addSelecteAttributeToDropdown(this)" class="form-control " name="dropDown" id="ddDropDown">';

        //fixme

        $.each(ClinicalCDSDetail.QuestionnaireDropDownValues[CDSQuestionnaireId].split(","), function (i, item) {
            HTML += '<option value=' + item + '>' + item + '</option>';
        });

        HTML += '</select></div>';

        return HTML;
    },

    YesOrNo: function (obj) {
        $(obj).closest("#customFormYesNoPreview").find('#chkYes').prop('checked', false);
        $(obj).closest("#customFormYesNoPreview").find('#chkNo').prop('checked', false);
        $(obj).closest("#customFormYesNoPreview").find('#chkNo').removeAttr('checked', '');
        $(obj).closest("#customFormYesNoPreview").find('#chkYes').removeAttr('checked', '');

        if (obj.id == "chkYes") {
            $(obj).closest("#customFormYesNoPreview").find('#chkYes').prop('checked', true);
            $(obj).closest("#customFormYesNoPreview").find('#chkYes').attr('checked', 'checked');
            if ($(obj).parents("[id^='toolYesNo_']").attr('style') !== undefined) {
                $(obj).parents("[id^='toolYesNo_']").removeAttr('style')
            }
        } else if (obj.id == "chkNo") {
            $(obj).closest("#customFormYesNoPreview").find('#chkNo').prop('checked', true);
            $(obj).closest("#customFormYesNoPreview").find('#chkNo').attr('checked', 'checked');
            if ($(obj).parents("[id^='toolYesNo_']").attr('style') !== undefined) {
                $(obj).parents("[id^='toolYesNo_']").removeAttr('style')
            }
        }
    },


    getLabResultData: function () {

        var labResultData = [];

        var self = $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSLabResults");

        var totalLabResults = self.find("[data-lab]");

        self.find(totalLabResults).each(function (i, Lab) {

            $(Lab).find("[data-test]").each(function (j, Test) {

                $(Test).find("[data-attribute]").each(function (k, Attribute) {
                    var labResult = null;
                    if (k == 0) {
                        labResult = {
                            LabId: $(this).attr('data-lab-id'),
                            LabName: $(this).attr('data-lab-name'),
                            TestId: $(this).attr('data-test-id'),
                            TestName: $(this).attr('data-test-name'),
                            AttributeId: $(this).attr('data-attribute-id'),

                            CDSLabResultId: $(this).attr('data-cds-lab-result-id'),
                            LabResultName: $(this).find("#txtLabResultName").val(),
                            LabResultOperator: '',
                            LabResultLogicalOperator: $(this).find('#ddlLabResultLogicalOperator').val(),
                            LabResultValue: $(this).find('#ddlLabResultLogicalOperator').val() == 6 ? null : $(this).find('#txtLabResultValue').val(),
                            LabResultValueFrom: $(this).find('#ddlLabResultLogicalOperator').val() == 6 ? $(this).find('#txtLabResultValueFrom').val() : null,
                            LabResultValueTo: $(this).find('#ddlLabResultLogicalOperator').val() == 6 ? $(this).find('#txtLabResultValueTo').val() : null,
                        };
                    }
                    else {
                        labResult = {
                            LabId: $(this).attr('data-lab-id'),
                            LabName: $(this).attr('data-lab-name'),
                            TestId: $(this).attr('data-test-id'),
                            TestName: $(this).attr('data-test-name'),
                            AttributeId: $(this).attr('data-attribute-id'),

                            CDSLabResultId: $(this).attr('data-cds-lab-result-id'),
                            LabResultName: $(this).find("#txtLabResultName").val(),
                            LabResultOperator: $(this).find('#ddlLabResultsOperator').val(),
                            LabResultLogicalOperator: $(this).find('#ddlLabResultLogicalOperator').val(),
                            LabResultValue: $(this).find('#ddlLabResultLogicalOperator').val() == 6 ? null : $(this).find('#txtLabResultValue').val(),
                            LabResultValueFrom: $(this).find('#ddlLabResultLogicalOperator').val() == 6 ? $(this).find('#txtLabResultValueFrom').val() : null,
                            LabResultValueTo: $(this).find('#ddlLabResultLogicalOperator').val() == 6 ? $(this).find('#txtLabResultValueTo').val() : null,
                        };
                    }

                    labResultData.push(labResult);
                });

            });

        });

        return labResultData;
    },


    deleteQuestionnaire: function (obj, ev) {
        ev.stopPropagation();
        var questionnaireId = $(obj).parent().attr('id');
        if ($(obj).parent().hasClass('fromDB') == false) {
            $(obj).parent().remove();

        } else {
            AppPrivileges.GetFormPrivileges("ClinicalDecisionSupport_Clinical Decision Support", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    utility.myConfirm('1', function () {
                        var selectedValue = questionnaireId;
                        if (selectedValue == "" || selectedValue == "undefined") {
                        }
                        else {
                            ClinicalCDSDetail.cdsQuestionnaireDelete(selectedValue).done(function (response) {

                                response = JSON.parse(response);
                                if (response.status != false) {
                                    $(obj).parent().remove();
                                    utility.DisplayMessages(response.Message, 1);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                    }, function () {
                    },
                        '1'
                    );
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }

    },

    cdsQuestionnaireDelete: function (questionnaireId) {
        var objData = new Object();
        objData["CDSQuestionnaireId"] = questionnaireId;
        objData["CDSId"] = ClinicalCDSDetail.params.CDSId;
        objData["commandType"] = "DELETE_CDS_Questionnaire";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CDS", "CDS");
    },


    bindInsuranceAutocomplete: function () {
        var self = $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail");

        CacheManager.BindCodes('GetInsurancePlan', false).done(function (result) {
            self.find("#txtCDSInsurance").autocomplete({
                autoFocus: true,
                source: InsurancePlans,
                select: function (event, ui) {
                    setTimeout(function () {
                        ClinicalCDSDetail.buildInsuranceLi(ui.item.id, ui.item.value);
                        //fixme
                        //self.find("#hfInsurancePlan").val(ui.item.id);
                        //Patient_Insurance.FillInsurancePlanAddress(ui.item.id);
                    }, 100);
                }
            });
        });
    },

    buildInsuranceLi: function (InsuranceId, InsuranceName) {

        var currId = -1;
        $("#ClinicalCDSDetail #frmClinicalCDSDetail ul#ulCDSInsurance li[id*='-']").each(function (i, item) {

            currId = $(this).attr("id");

        });
        var li = '';
        currId = parseInt(currId) + (-1);
        if ($('#ClinicalCDSDetail #ulCDSInsurance li:first').length > 0) {
            li = "<li id=" + InsuranceId + " onclick='' \"><div class='col-sm-4 col-lg-2 pl-none pt-tiny'><select id='ddlInsurance" + InsuranceId + "' name = 'CDSInsurance" + InsuranceId + "' class='form-control'><option value='OR'>OR</option><option value='AND'>AND</option></select></div><div class='col-sm-8 col-lg-10'><a href='#'>" + InsuranceName + "<span class='removeIconListHover' onclick='ClinicalCDSDetail.deleteInsurance($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
        }
        else {
            li = "<li id=" + InsuranceId + " onclick='' \"><div class='col-sm-4 col-lg-2 pl-none pt-tiny'></div><div class='col-sm-8 col-lg-10'><a href='#'>" + InsuranceName + "<span class='removeIconListHover' onclick='ClinicalCDSDetail.deleteInsurance($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
        }

        var IsAlreadyExist = false;
        $('#ClinicalCDSDetail #ulCDSInsurance li').each(function () {
            if ($(this).attr('id') == InsuranceId) {

                IsAlreadyExist = true;
            }
        });

        if (!IsAlreadyExist) {
            $('#ClinicalCDSDetail #ulCDSInsurance').append(li);
            //$('.modal-backdrop').removeClass('in');
            //$('.modal-backdrop').addClass('out');
            //$('.modal-backdrop').hide();
            $('#ClinicalCDSDetail #txtCDSInsurance').val('');
            //     Clinical_Complaints.AddInArray(currId, icd10Description, true);

            //var isUnload = "false";
            //var txt = $('#ClinicalCDSDetail #txtCDSMedications');
            //if (txt.is('[data-popupunload]')) {
            //    isUnload = txt.attr('data-popupunload');
            //}

            //if (Admin_IMOICD != null && Admin_IMOICD.UnLoadTab && isUnload == "true") {
            //    txt.attr("data-popupunload", "false");
            //    Admin_IMOICD.UnLoadTab();
            //}
        }
        else {
            utility.DisplayMessages(InsuranceName + ' is already added', 2);

            $('#ClinicalCDSDetail #txtCDSInsurance').val('');
        }

    },

    deleteInsurance: function (obj, ev) {
        ev.stopPropagation();
        var insuranceId = $(obj).attr('insuranceId');
        if ($(obj).hasClass('fromDB') == false) {
            $(obj).parent().remove();
            if ($('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSInsurance li").length > 0) {
                $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSInsurance li:first select").remove();
            }

        } else {

            AppPrivileges.GetFormPrivileges("ClinicalDecisionSupport_Clinical Decision Support", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    utility.myConfirm('1', function () {
                        var selectedValue = insuranceId;
                        if (selectedValue == "" || selectedValue == "undefined") {
                        }
                        else {
                            ClinicalCDSDetail.cdsInsuranceDelete(selectedValue).done(function (response) {

                                response = JSON.parse(response);
                                if (response.status != false) {
                                    $(obj).remove();
                                    if ($('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSInsurance li").length > 0) {
                                        $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSInsurance li:first select").remove();
                                    }
                                    utility.DisplayMessages(response.Message, 1);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                    }, function () { },
                        '1'
                    );
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }

    },

    cdsInsuranceDelete: function (insuranceId) {
        var objData = new Object();
        objData["CDSInsuranceId"] = insuranceId;
        objData["CDSId"] = ClinicalCDSDetail.params.CDSId;
        objData["commandType"] = "DELETE_CDS_Insurance";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CDS", "CDS");
    },


    getInsuranceData: function () {
        var self = $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail");

        var insuranceData = new Array();
        self.find("#ulCDSInsurance li").each(function (index, item) {
            var insurance = null;
            if (index == 0) {
                insurance = {
                    InsurancePlanId: $(this).attr('id'),
                    InsuranceName: $(this).find('a').text(),
                    InsuranceOperator: ''
                };
            }
            else {
                insurance = {
                    InsurancePlanId: $(this).attr('id'),
                    InsuranceName: $(this).find('a').text(),
                    InsuranceOperator: $(this).find('#ddlInsurance' + $(this).attr('id')).val()
                };
            }

            insuranceData.push(insurance);
        });

        return insuranceData;
    },


    loadCDSInsurance: function (cdsInsurance) {
        $(cdsInsurance).each(function (index, item) {

            var li = '';

            if (index == 0) {
                li = "<li class='fromDB' insuranceId=" + item.CDSInsuranceId + " id=" + item.InsurancePlanId + " onclick='' \"><div class='col-sm-4 col-lg-2 pl-none pt-tiny fromDB'></div><div class='col-sm-8 col-lg-10'><a href='#'>" + item.InsuranceName + "<span class='removeIconListHover' onclick='ClinicalCDSDetail.deleteInsurance($($(this).parent()).parent().parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }
            else {
                li = "<li class='fromDB' insuranceId=" + item.CDSInsuranceId + " id=" + item.InsurancePlanId + " onclick='' \"><div class='col-sm-4 col-lg-2 pl-none pt-tiny fromDB'><select id='ddlInsurance" + item.InsurancePlanId + "' name = 'CDSInsurance" + item.InsurancePlanId + "' class='form-control'><option value='AND'>AND</option><option value='OR'>OR</option></select></div><div class='col-sm-8 col-lg-10'><a href='#'>" + item.InsuranceName + "<span class='removeIconListHover' onclick='ClinicalCDSDetail.deleteInsurance($($(this).parent()).parent().parent(), event);'><i class='fa fa-times'></i></span></a></div><div class='clearfix'></div></li>";
            }

            $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSInsurance").append(li);

            if (index != 0) {
                if (item.InsuranceOperator == 'AND') {
                    $($('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSInsurance li#" + item.InsurancePlanId).find("#ddlInsurance" + item.InsurancePlanId + " option")[0]).attr('selected', true);
                }
                else {
                    $($('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ulCDSInsurance li#" + item.InsurancePlanId).find("#ddlInsurance" + item.InsurancePlanId + " option")[1]).attr('selected', true);
                }
            }
            //$('.modal-backdrop').removeClass('in');
            //$('.modal-backdrop').addClass('out');
            //$('.modal-backdrop').hide();
            $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #txtCDSInsurance").val('');
        });
    },


    QuestionnaireControlTypeChanged: function (obj) {

        if ($(obj).val() == "5") {

            var params = [];

            params["CDSQuestionnaireId"] = $(obj).closest("tr").attr("id");
            params["FromAdmin"] = 0;
            params["ParentCtrl"] = 'ClinicalCDSDetail';
            LoadActionPan("ClinicalCDSQuestionnaireDropdown", params);

        }
    },

    enableDisableReminderControls: function (enable) {

        var totalRulesSelected = $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #ddlCDSRuleType option:selected").length;

        if (totalRulesSelected > 0) {
            $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #divReminder #txtCDSReminderLength").val("");
            $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #divReminder #ddlCDSReminderPeriod").val("");
            $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #divReminder").addClass("disableAll");
        } else {
            $('#' + ClinicalCDSDetail.params.PanelID + " #frmClinicalCDSDetail #divReminder").removeClass("disableAll");


        }
    },
}